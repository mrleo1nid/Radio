using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace Radio.Models
{
   public class Settings
    {
        private bool shovSpectumAnalizer;
        public bool ShovSpectumAnalizer
        {
            get { return shovSpectumAnalizer; }
            set
            {
                shovSpectumAnalizer = value;
                OnPropertyChanged(nameof(ShovSpectumAnalizer));
            }
        }

        private bool minimizeToTrayOnClose;
        public bool MinimizeToTrayOnClose
        {
            get { return minimizeToTrayOnClose; }
            set
            {
                minimizeToTrayOnClose = value;
                OnPropertyChanged(nameof(MinimizeToTrayOnClose));
            }
        }

        private bool downoloadTrackLocal;
        public bool DownoloadTrackLocal
        {
            get { return downoloadTrackLocal; }
            set
            {
                downoloadTrackLocal = value;
                OnPropertyChanged(nameof(DownoloadTrackLocal));
            }
        }
        private bool autoPlayTracks;
        public bool AutoPlayTracks
        {
            get { return autoPlayTracks; }
            set
            {
                autoPlayTracks = value;
                OnPropertyChanged(nameof(AutoPlayTracks));
            }
        }
        private bool showPoppup;
        public bool ShowPoppup
        {
            get { return showPoppup; }
            set
            {
                showPoppup = value;
                OnPropertyChanged(nameof(ShowPoppup));
            }
        }
        private int windowStyle;
        public int WindowStyle
        {
            get { return windowStyle; }
            set
            {
                windowStyle = value;
                OnPropertyChanged(nameof(WindowStyle));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public static Settings LoadSettings()
        {
            Settings settings = new Settings();
            settings.DownoloadTrackLocal = Properties.Settings.Default.DownoloadTrackLocal;
            settings.MinimizeToTrayOnClose = Properties.Settings.Default.MinimizeToTrayOnClose;
            settings.ShovSpectumAnalizer = Properties.Settings.Default.ShovSpectumAnalizer;
            settings.AutoPlayTracks = Properties.Settings.Default.AutoPlayTracks;
            settings.ShowPoppup = Properties.Settings.Default.ShowPoppup;
            settings.WindowStyle = Properties.Settings.Default.WindowStyle;
            return settings;
        }

        public static void SaveSettings(Settings settings)
        {
            Properties.Settings.Default.DownoloadTrackLocal = settings.DownoloadTrackLocal;
            Properties.Settings.Default.MinimizeToTrayOnClose = settings.MinimizeToTrayOnClose;
            Properties.Settings.Default.ShovSpectumAnalizer = settings.ShovSpectumAnalizer;
            Properties.Settings.Default.AutoPlayTracks = settings.AutoPlayTracks;
            Properties.Settings.Default.ShowPoppup = settings.ShowPoppup;
            Properties.Settings.Default.WindowStyle = settings.WindowStyle;
            Properties.Settings.Default.Save();
        }
    }
}
