using Radio.Models;

namespace Radio.ViewModels
{
    public class SettingsViewModel : ViewModel
    {
        private MainViewModel mainViewModel;
        public SettingsViewModel(MainViewModel mainViewModel)
        {
           this.mainViewModel = mainViewModel;
           this.mainViewModel.Settings = Settings.LoadSettings();
        }

        private RelayCommand _saveSettingsCommand;
        public RelayCommand SaveSettingsCommand => _saveSettingsCommand ?? (_saveSettingsCommand = new RelayCommand(SaveSettingsFunc));
        private RelayCommand _cancelSettingsWindowCommand;
        public RelayCommand CancelSettingsWindowCommand => _cancelSettingsWindowCommand ?? (_cancelSettingsWindowCommand = new RelayCommand(CancelSettingsWindowFunc));

        private void SaveSettingsFunc()
        {
            Models.Settings.SaveSettings(this.mainViewModel.Settings);
            this.mainViewModel.Settings = Settings.LoadSettings();
            this.mainViewModel.SettingsWindow.Close();
        }
        private void CancelSettingsWindowFunc()
        {
            this.mainViewModel.SettingsWindow.Close();
        }

    }
}
