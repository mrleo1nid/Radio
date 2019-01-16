using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
using Radio.Helper;
using Radio.Workers;
using WPFSoundVisualizationLib;

namespace Radio.Views
{
    /// <summary>
    /// Логика взаимодействия для DopWindow.xaml
    /// </summary>
    public partial class DopWindow : Window
    {
        public DopWindow()
        {
            InitializeComponent();
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "(*.mp3, *.m4a)|*.mp3;*.m4a";
            if (openDialog.ShowDialog() == true)
            {
                BassEngine.Instance.OpenFile(openDialog.FileName);
                BassEngine.Instance.Play();
            }
            BassEngine bassEngine = BassEngine.Instance;
            bassEngine.PropertyChanged += BassEngine_PropertyChanged;
            spectrumAnalyzer.RegisterSoundPlayer(bassEngine);
            linetime.RegisterSoundPlayer(bassEngine);
        }

        private void BassEngine_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            BassEngine engine = BassEngine.Instance;
            switch (e.PropertyName)
            {
                case "FileTag":
                    if (engine.FileTag != null)
                    {
                        TagLib.Tag tag = engine.FileTag.Tag;
                        if (tag.Pictures.Length > 0)
                        {
                            using (MemoryStream albumArtworkMemStream = new MemoryStream(tag.Pictures[0].Data.Data))
                            {
                                try
                                {
                                    BitmapImage albumImage = new BitmapImage();
                                    albumImage.BeginInit();
                                    albumImage.CacheOption = BitmapCacheOption.OnLoad;
                                    albumImage.StreamSource = albumArtworkMemStream;
                                    albumImage.EndInit();
                                    albumArtPanel.AlbumArtImage = albumImage;
                                }
                                catch (NotSupportedException)
                                {
                                    albumArtPanel.AlbumArtImage = null;
                                    // System.NotSupportedException:
                                    // No imaging component suitable to complete this operation was found.
                                }
                                albumArtworkMemStream.Close();
                            }
                        }
                        else
                        {
                            albumArtPanel.AlbumArtImage = null;
                        }
                    }
                    else
                    {
                        albumArtPanel.AlbumArtImage = null;
                    }
                    break;
                case "ChannelPosition":
                    DigitalClock.Time = TimeSpan.FromSeconds(engine.ChannelPosition);
                    break;
                default:
                    // Do Nothing
                    break;
            }

        }
    }
}
