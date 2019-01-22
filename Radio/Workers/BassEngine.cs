
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;
using Un4seen.Bass;
using WPFSoundVisualizationLib;

namespace Radio.Workers
{
    public class BassEngine : ISpectrumPlayer
    {
        #region Fields
        private static BassEngine instance;
        private readonly DispatcherTimer positionTimer = new DispatcherTimer(DispatcherPriority.ApplicationIdle);
        private readonly int fftDataSize = (int)FFTDataSize.FFT2048;
        private readonly int maxFFT = (int)(BASSData.BASS_DATA_AVAILABLE | BASSData.BASS_DATA_FFT2048);
        private readonly BackgroundWorker waveformGenerateWorker = new BackgroundWorker();
        private readonly SYNCPROC endTrackSyncProc;
        private readonly SYNCPROC repeatSyncProc;
        private int sampleFrequency = 44100;
        private int activeStreamHandle;
        private TagLib.File fileTag;
        private bool canPlay;
        private bool canPause;
        private bool isPlaying;
        private bool canStop;
        private double channelLength;
        private double currentChannelPosition;
        private float[] fullLevelData;
        private bool inChannelSet;
        private bool inChannelTimerUpdate;
        private int repeatSyncId;
        private string pendingWaveformPath;
        private TimeSpan repeatStart;
        private TimeSpan repeatStop;
        private bool inRepeatSet;

        #endregion

        #region Constants
        private const int repeatThreshold = 200;
        #endregion

        #region Constructor
        private BassEngine()
        {
            Initialize();
            endTrackSyncProc = EndTrack;
            repeatSyncProc = RepeatCallback;
        }
        #endregion

        #region ISpectrumPlayer
        public int GetFFTFrequencyIndex(int frequency)
        {
            return Utils.FFTFrequency2Index(frequency, fftDataSize, sampleFrequency);
        }

        public bool GetFFTData(float[] fftDataBuffer)
        {
            return (Bass.BASS_ChannelGetData(ActiveStreamHandle, fftDataBuffer, maxFFT)) > 0;
        }
        #endregion

        #region IWaveformPlayer
        public TimeSpan SelectionBegin
        {
            get { return repeatStart; }
            set
            {
                if (!inRepeatSet)
                {
                    inRepeatSet = true;
                    TimeSpan oldValue = repeatStart;
                    repeatStart = value;
                    if (oldValue != repeatStart)
                        NotifyPropertyChanged("SelectionBegin");
                    SetRepeatRange(value, SelectionEnd);
                    inRepeatSet = false;
                }
            }
        }

        public TimeSpan SelectionEnd
        {
            get { return repeatStop; }
            set
            {
                if (!inChannelSet)
                {
                    inRepeatSet = true;
                    TimeSpan oldValue = repeatStop;
                    repeatStop = value;
                    if (oldValue != repeatStop)
                        NotifyPropertyChanged("SelectionEnd");
                    SetRepeatRange(SelectionBegin, value);
                    inRepeatSet = false;
                }
            }
        }

        public double ChannelLength
        {
            get { return channelLength; }
            protected set
            {
                double oldValue = channelLength;
                channelLength = value;
                if (oldValue != channelLength)
                    NotifyPropertyChanged("ChannelLength");
            }
        }

        public double ChannelPosition
        {
            get { return currentChannelPosition; }
            set
            {
                if (!inChannelSet)
                {
                    inChannelSet = true; // Avoid recursion
                    double oldValue = currentChannelPosition;
                    double position = Math.Max(0, Math.Min(value, ChannelLength));
                    if (!inChannelTimerUpdate)
                        Bass.BASS_ChannelSetPosition(ActiveStreamHandle, Bass.BASS_ChannelSeconds2Bytes(ActiveStreamHandle, position));
                    currentChannelPosition = position;
                    if (oldValue != currentChannelPosition)
                        NotifyPropertyChanged("ChannelPosition");
                    inChannelSet = false;
                }
            }
        }
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        #endregion

        #region Singleton Instance
        public static BassEngine Instance
        {
            get
            {
                if (instance == null)
                    instance = new BassEngine();
                return instance;
            }
        }
        #endregion

        #region Public Methods
        public void Stop()
        {
            ChannelPosition = SelectionBegin.TotalSeconds;
            if (ActiveStreamHandle != 0)
            {
                Bass.BASS_ChannelStop(ActiveStreamHandle);
                Bass.BASS_ChannelSetPosition(ActiveStreamHandle, ChannelPosition);
            }
            IsPlaying = false;
            CanStop = false;
            CanPlay = true;
            CanPause = false;
        }

        public void Pause()
        {
            if (IsPlaying && CanPause)
            {
                Bass.BASS_ChannelPause(ActiveStreamHandle);
                IsPlaying = false;
                CanPlay = true;
                CanPause = false;
            }
        }

        public void Play()
        {
            if (CanPlay)
            {
                PlayCurrentStream();
                IsPlaying = true;
                CanPause = true;
                CanPlay = false;
                CanStop = true;
            }
        }
        public void ChangeValue(float value)
        {
            Bass.BASS_ChannelSetAttribute(ActiveStreamHandle, BASSAttribute.BASS_ATTRIB_VOL, value);
        }
        public void ChangeEqualizer(float[] value)
        {
            Bass.BASS_ChannelSetAttribute(ActiveStreamHandle, BASSAttribute.BASS_ATTRIB_EAXMIX, value[0]);
        }
        public bool OpenUrl(string url)
        {
            Stop();

            if (ActiveStreamHandle != 0)
            {
                ClearRepeatRange();
                ChannelPosition = 0;
                Bass.BASS_StreamFree(ActiveStreamHandle);
            }

            if (true)
            {
                FileStreamHandle = ActiveStreamHandle = Bass.BASS_StreamCreateURL(url,0, BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_PRESCAN, null, new IntPtr(0));
                ChannelLength = Bass.BASS_ChannelBytes2Seconds(FileStreamHandle, Bass.BASS_ChannelGetLength(FileStreamHandle));
                if (ActiveStreamHandle != 0)
                {
                    // Obtain the sample rate of the stream
                    BASS_CHANNELINFO info = new BASS_CHANNELINFO();
                    Bass.BASS_ChannelGetInfo(ActiveStreamHandle, info);
                    sampleFrequency = info.freq;

                    // Set the stream to call Stop() when it ends.
                    int syncHandle = Bass.BASS_ChannelSetSync(ActiveStreamHandle,
                        BASSSync.BASS_SYNC_END,
                        0,
                        endTrackSyncProc,
                        IntPtr.Zero);

                    if (syncHandle == 0)
                        throw new ArgumentException("Error establishing End Sync on file stream.", "url");

                    CanPlay = true;
                    return true;
                }
                else
                {
                    ActiveStreamHandle = 0;
                    FileTag = null;
                    CanPlay = false;
                }
            }
            return false;
        }
        #endregion

        #region Event Handleres
        private void positionTimer_Tick(object sender, EventArgs e)
        {
            if (ActiveStreamHandle == 0)
            {
                ChannelPosition = 0;
            }
            else
            {
                inChannelTimerUpdate = true;
                ChannelPosition = Bass.BASS_ChannelBytes2Seconds(ActiveStreamHandle, Bass.BASS_ChannelGetPosition(ActiveStreamHandle, 0));
                inChannelTimerUpdate = false;
            }
        }
        #endregion
        #region Private Utility Methods
        private void Initialize()
        {
            positionTimer.Interval = TimeSpan.FromMilliseconds(50);
            positionTimer.Tick += positionTimer_Tick;

            IsPlaying = false;

            Window mainWindow = Application.Current.MainWindow;
            WindowInteropHelper interopHelper = new WindowInteropHelper(mainWindow);

            if (Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_SPEAKERS, interopHelper.Handle))
            {
                int pluginAAC = Bass.BASS_PluginLoad("bass_aac.dll");
#if DEBUG
                BASS_INFO info = new BASS_INFO();
                Bass.BASS_GetInfo(info);
                Debug.WriteLine(info.ToString());
                BASS_PLUGININFO aacInfo = Bass.BASS_PluginGetInfo(pluginAAC);
                foreach (BASS_PLUGINFORM f in aacInfo.formats)
                    Debug.WriteLine("Type={0}, Name={1}, Exts={2}", f.ctype, f.name, f.exts);
#endif
            }
            else
            {
                MessageBox.Show(mainWindow, "Bass initialization error!");
                mainWindow.Close();
            }
        }

        private void SetRepeatRange(TimeSpan startTime, TimeSpan endTime)
        {
            if (repeatSyncId != 0)
                Bass.BASS_ChannelRemoveSync(ActiveStreamHandle, repeatSyncId);

            if ((endTime - startTime) > TimeSpan.FromMilliseconds(repeatThreshold))
            {
                long channelLength = Bass.BASS_ChannelGetLength(ActiveStreamHandle);
                long endPosition = (long)((endTime.TotalSeconds / ChannelLength) * channelLength);
                repeatSyncId = Bass.BASS_ChannelSetSync(ActiveStreamHandle,
                    BASSSync.BASS_SYNC_POS,
                    (long)endPosition,
                    repeatSyncProc,
                    IntPtr.Zero);
                ChannelPosition = SelectionBegin.TotalSeconds;
            }
            else
                ClearRepeatRange();
        }

        private void ClearRepeatRange()
        {
            if (repeatSyncId != 0)
            {
                Bass.BASS_ChannelRemoveSync(ActiveStreamHandle, repeatSyncId);
                repeatSyncId = 0;
            }
        }

        private void PlayCurrentStream()
        {
            // Play Stream
            if (ActiveStreamHandle != 0 && Bass.BASS_ChannelPlay(ActiveStreamHandle, false))
            {
                // Do nothing
            }
#if DEBUG
            else
            {
                Debug.WriteLine("Error={0}", Bass.BASS_ErrorGetCode());
            }
#endif
        }
        #endregion

        #region Callbacks
        private void EndTrack(int handle, int channel, int data, IntPtr user)
        {
            App.Current.Dispatcher.BeginInvoke(new Action(() => Stop()));
        }

        private void RepeatCallback(int handle, int channel, int data, IntPtr user)
        {
            App.Current.Dispatcher.BeginInvoke(new Action(() => ChannelPosition = SelectionBegin.TotalSeconds));
        }
        #endregion

        #region Public Properties
        public int FileStreamHandle
        {
            get { return activeStreamHandle; }
            protected set
            {
                int oldValue = activeStreamHandle;
                activeStreamHandle = value;
                if (oldValue != activeStreamHandle)
                    NotifyPropertyChanged("FileStreamHandle");
            }
        }

        public int ActiveStreamHandle
        {
            get { return activeStreamHandle; }
            protected set
            {
                int oldValue = activeStreamHandle;
                activeStreamHandle = value;
                if (oldValue != activeStreamHandle)
                    NotifyPropertyChanged("ActiveStreamHandle");
            }
        }

        public TagLib.File FileTag
        {
            get { return fileTag; }
            set
            {
                TagLib.File oldValue = fileTag;
                fileTag = value;
                if (oldValue != fileTag)
                    NotifyPropertyChanged("FileTag");
            }
        }

        public bool CanPlay
        {
            get { return canPlay; }
            protected set
            {
                bool oldValue = canPlay;
                canPlay = value;
                if (oldValue != canPlay)
                    NotifyPropertyChanged("CanPlay");
            }
        }

        public bool CanPause
        {
            get { return canPause; }
            protected set
            {
                bool oldValue = canPause;
                canPause = value;
                if (oldValue != canPause)
                    NotifyPropertyChanged("CanPause");
            }
        }

        public bool CanStop
        {
            get { return canStop; }
            protected set
            {
                bool oldValue = canStop;
                canStop = value;
                if (oldValue != canStop)
                    NotifyPropertyChanged("CanStop");
            }
        }

        public bool IsPlaying
        {
            get { return isPlaying; }
            protected set
            {
                bool oldValue = isPlaying;
                isPlaying = value;
                if (oldValue != isPlaying)
                    NotifyPropertyChanged("IsPlaying");
                positionTimer.IsEnabled = value;
            }
        }

        #endregion
    }
}
