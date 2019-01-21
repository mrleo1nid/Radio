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
        public SettingsViewModel()
        {
           MainViewModel.Settings = Settings.LoadSettings();
        }

        private RelayCommand _saveSettingsCommand;
        public RelayCommand SaveSettingsCommand => _saveSettingsCommand ?? (_saveSettingsCommand = new RelayCommand(SaveSettingsFunc));
        private RelayCommand _cancelSettingsWindowCommand;
        public RelayCommand CancelSettingsWindowCommand => _cancelSettingsWindowCommand ?? (_cancelSettingsWindowCommand = new RelayCommand(CancelSettingsWindowFunc));

        private void SaveSettingsFunc()
        {
            Models.Settings.SaveSettings(MainViewModel.Settings);
            MainViewModel.Settings = Settings.LoadSettings();
            MainViewModel.SettingsWindow.Close();
        }
        private void CancelSettingsWindowFunc()
        {
            MainViewModel.SettingsWindow.Close();
        }

    }
}
