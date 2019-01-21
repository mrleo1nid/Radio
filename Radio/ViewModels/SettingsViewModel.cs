using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radio.Models;
using Radio.Views;

namespace Radio.ViewModels
{
    class SettingsViewModel : ViewModel
    {
        public static Settings Settings { get; set; }
        public static SettingsWindow Window { get; set; }

        public SettingsViewModel()
        {
           Settings = Settings.LoadSettings();
           Window = MainViewModel.SettingsWindow;
        }

        private RelayCommand _saveSettingsCommand;
        public RelayCommand SaveSettingsCommand => _saveSettingsCommand ?? (_saveSettingsCommand = new RelayCommand(SaveSettingsFunc));
        private RelayCommand _cancelSettingsWindowCommand;
        public RelayCommand CancelSettingsWindowCommand => _cancelSettingsWindowCommand ?? (_cancelSettingsWindowCommand = new RelayCommand(CancelSettingsWindowFunc));

        private void SaveSettingsFunc()
        {
            Models.Settings.SaveSettings(Settings);
            Settings = Settings.LoadSettings();
            MainViewModel.Settings = Settings;
            Window.Close();
        }
        private void CancelSettingsWindowFunc()
        {
            Window.Close();
        }

    }
}
