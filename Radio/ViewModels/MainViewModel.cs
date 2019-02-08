using System.Threading.Tasks;
using System.Windows;
using Radio.Models;
using Radio.Views;
using Radio.Workers;

namespace Radio.ViewModels
{
   public class MainViewModel : ViewModel
    {
        public MainViewModel()
        {
            OneColumnWidth = 55;
            twoColumnWidth = 745;
            MainWindowWidth = 800;
            Storage.VmStorage["MainViewModel"]= this;
            Conected = PlaylistDownloader.CheckForInternetConnection();
            Settings = Settings.LoadSettings();
            ChangeElementVisibility();
            NotConnectedVM = new NotConectionControlViewModel(this);
            PlaylistsVM = new PlaylistsViewModel(this);
            SettingsVM = new SettingsViewModel(this);
        }

        public  PlaylistsViewModel PlaylistsVM { get; set; }
        public SettingsViewModel SettingsVM { get; set; }
        public NotConectionControlViewModel NotConnectedVM { get; set; }

        public  bool CanClose { get; set; }

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
        private bool conected;
        public bool Conected
        {
            get { return conected; }
            set
            {
                conected = value;
                OnPropertyChanged(nameof(Conected));
            }
        }
        private Visibility exitButtVisibilyty;
        public Visibility ExitButtVisibilyty
        {
            get { return exitButtVisibilyty; }
            set
            {
                exitButtVisibilyty = value;
                OnPropertyChanged(nameof(ExitButtVisibilyty));
            }
        }

        #region Size
        private int oneColumnWidth;
        public int OneColumnWidth
        {
            get { return oneColumnWidth; }
            set
            {
                oneColumnWidth = value;
                TwoColumnWidth = MainWindowWidth - value;
                OnPropertyChanged(nameof(OneColumnWidth));
            }
        }
        private int twoColumnWidth;
        public int TwoColumnWidth
        {
            get { return twoColumnWidth; }
            set
            {
                twoColumnWidth = value;
                OnPropertyChanged(nameof(TwoColumnWidth));
            }
        }
        private int mainWindowWidth;
        public int MainWindowWidth
        {
            get { return mainWindowWidth; }
            set
            {
                mainWindowWidth = value;
                OnPropertyChanged(nameof(MainWindowWidth));
            }
        }
        #endregion



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
        private void ChangeElementVisibility()
        {
            if (Settings.WindowStyle == 2)
            {
                exitButtVisibilyty = Visibility.Visible;
            }
            else
            {
                exitButtVisibilyty = Visibility.Collapsed;
            }
        }

    }
}
