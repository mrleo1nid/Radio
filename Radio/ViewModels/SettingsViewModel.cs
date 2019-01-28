using Radio.Models;
using Radio.Views;

namespace Radio.ViewModels
{
    public class SettingsViewModel : ViewModel
    {
        public MainViewModel mainViewModel { get; set; }
        public SettingsViewModel(MainViewModel mainViewModel)
        {
           Storage.VmStorage["SettingsViewModel"] = this;
           this.mainViewModel = mainViewModel;
        }

        private RelayCommand _saveSettingsCommand;
        public RelayCommand SaveSettingsCommand => _saveSettingsCommand ?? (_saveSettingsCommand = new RelayCommand(SaveSettingsFunc));
        private RelayCommand _cancelSettingsWindowCommand;
        public RelayCommand CancelSettingsWindowCommand => _cancelSettingsWindowCommand ?? (_cancelSettingsWindowCommand = new RelayCommand(CancelSettingsWindowFunc));

        private void SaveSettingsFunc()
        {
            SettingsWindow settingsWindow = Storage.WindowStorage["SettingsWindow"] as SettingsWindow;
            Settings.SaveSettings(mainViewModel.Settings);
            mainViewModel.Settings = Settings.LoadSettings();
            settingsWindow.Close();
        }
        private void CancelSettingsWindowFunc()
        {
            SettingsWindow settingsWindow = Storage.WindowStorage["SettingsWindow"] as SettingsWindow;
            settingsWindow.Close();
        }
    }
}
