using Radio.Models;
using Radio.Views;

namespace Radio.ViewModels
{
    public class SettingsViewModel : ViewModel
    {
        private MainViewModel mainViewModel;
        public SettingsViewModel(MainViewModel mainViewModel)
        {
           Storage.VmStorage["SettingsViewModel"] = this;
           this.mainViewModel = mainViewModel;
           this.mainViewModel.Settings = Settings.LoadSettings();
        }

        private RelayCommand _saveSettingsCommand;
        public RelayCommand SaveSettingsCommand => _saveSettingsCommand ?? (_saveSettingsCommand = new RelayCommand(SaveSettingsFunc));
        private RelayCommand _cancelSettingsWindowCommand;
        public RelayCommand CancelSettingsWindowCommand => _cancelSettingsWindowCommand ?? (_cancelSettingsWindowCommand = new RelayCommand(CancelSettingsWindowFunc));

        private void SaveSettingsFunc()
        {
            SettingsWindow settingsWindow = Storage.WindowStorage["SettingsWindow"] as SettingsWindow;
            Models.Settings.SaveSettings(this.mainViewModel.Settings);
            this.mainViewModel.Settings = Settings.LoadSettings();
            settingsWindow.Close();
        }
        private void CancelSettingsWindowFunc()
        {
            SettingsWindow settingsWindow = Storage.WindowStorage["SettingsWindow"] as SettingsWindow;
            settingsWindow.Close();
        }

    }
}
