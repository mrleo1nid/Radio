using Radio.Models;
using Radio.Views;
using Radio.Workers;

namespace Radio.ViewModels
{
   public class MainViewModel : ViewModel
    {
        public MainViewModel()
        {
            Storage.VmStorage["MainViewModel"]= this;
            Conected = PlaylistDownloader.CheckForInternetConnection();
            if (Conected)
            {
                PlaylistsVM = new PlaylistsViewModel(this);
                SettingsVM = new SettingsViewModel(this);
                Settings = Settings.LoadSettings();
            }
        }
        public  PlaylistsViewModel PlaylistsVM { get; set; }
        public SettingsViewModel SettingsVM { get; set; }

        public  bool CanClose { get; set; }
        public bool Conected { get; set; }

        private  Settings settings;
        public Settings Settings
        {
            get { return settings; }
            set
            {
                settings = value;
                CanClose = !settings.MinimizeToTrayOnClose;
                OnPropertyChanged(nameof(Settings));
            }
        }

        private RelayCommand _openDopWindow;
        public RelayCommand OpenDopWindowCommand => _openDopWindow ?? (_openDopWindow = new RelayCommand(OpenSettingsWindow));
        private RelayCommand _closeProgrammCommand;
        public RelayCommand CloseProgrammCommand => _closeProgrammCommand ?? (_closeProgrammCommand = new RelayCommand(CloseProgrammFunc));
        private RelayCommand _showHideMainWindCommand;
        public RelayCommand ShowHideMainWindCommand => _showHideMainWindCommand ?? (_showHideMainWindCommand = new RelayCommand(ShowHideMainWindFunc));

        private void OpenSettingsWindow()
        {
            MainWindow mainWindow = Storage.WindowStorage["MainWindow"] as MainWindow;
            SettingsWindow SettingsWindow = new SettingsWindow();
            SettingsWindow.DataContext = SettingsVM;
            SettingsWindow.Owner = mainWindow;
            SettingsWindow.Show();
        }

    
        private void CloseProgrammFunc()
        {
            MainWindow mainWindow = Storage.WindowStorage["MainWindow"] as MainWindow;
            CanClose = true;
            mainWindow.Close();
        }
        private void ShowHideMainWindFunc()
        {
            MainWindow mainWindow = Storage.WindowStorage["MainWindow"] as MainWindow;
            if (mainWindow.IsVisible)
            {
                mainWindow.Hide();
            }
            else
            {
                mainWindow.Show();
                mainWindow.Activate();
            }
        }

    }
}
