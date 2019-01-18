using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

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
            }
        }

        private bool minimizeToTrayOnClose;
        public bool MinimizeToTrayOnClose
        {
            get { return minimizeToTrayOnClose; }
            set
            {
                minimizeToTrayOnClose = value;
            }
        }

        private bool downoloadTrackLocal;
        public bool DownoloadTrackLocal
        {
            get { return downoloadTrackLocal; }
            set
            {
                downoloadTrackLocal = value;
            }
        }
        private bool autoPlayTracks;
        public bool AutoPlayTracks
        {
            get { return autoPlayTracks; }
            set
            {
                autoPlayTracks = value;
            }
        }
        private bool showPoppup;
        public bool ShowPoppup
        {
            get { return showPoppup; }
            set
            {
                showPoppup = value;
            }
        }

        public static Settings LoadSettings()
        {
            Settings settings = new Settings();
            settings.DownoloadTrackLocal = Properties.Settings.Default.DownoloadTrackLocal;
            settings.MinimizeToTrayOnClose = Properties.Settings.Default.MinimizeToTrayOnClose;
            settings.ShovSpectumAnalizer = Properties.Settings.Default.ShovSpectumAnalizer;
            settings.AutoPlayTracks = Properties.Settings.Default.AutoPlayTracks;
            settings.ShowPoppup = Properties.Settings.Default.ShowPoppup;
            return settings;
        }

        public static void SaveSettings(Settings settings)
        {
            Properties.Settings.Default.DownoloadTrackLocal = settings.DownoloadTrackLocal;
            Properties.Settings.Default.MinimizeToTrayOnClose = settings.MinimizeToTrayOnClose;
            Properties.Settings.Default.ShovSpectumAnalizer = settings.ShovSpectumAnalizer;
            Properties.Settings.Default.AutoPlayTracks = settings.AutoPlayTracks;
            Properties.Settings.Default.ShowPoppup = settings.ShowPoppup;
            Properties.Settings.Default.Save();
        }
    }
}
