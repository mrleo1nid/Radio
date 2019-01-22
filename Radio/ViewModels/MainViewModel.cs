using Radio.Models;
using Radio.Views;
using Radio.Workers;

namespace Radio.ViewModels
{
   public class MainViewModel : ViewModel
    {
        public MainViewModel()
        {
            PlaylistsVM = new PlaylistsViewModel(this);
            SettingsVM = new SettingsViewModel(this);
        }


        public  MainWindow MainWindow { get; set; }
        public  PlaylistsViewModel PlaylistsVM { get; set; }
        public SettingsViewModel SettingsVM { get; set; }
        public  BassEngine  BassEngine { get; set; }
        public  SettingsWindow SettingsWindow { get; set; }


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

        private RelayCommand _openDopWindow;
        public RelayCommand OpenDopWindowCommand => _openDopWindow ?? (_openDopWindow = new RelayCommand(OpenSettingsWindow));
        private RelayCommand _closeProgrammCommand;
        public RelayCommand CloseProgrammCommand => _closeProgrammCommand ?? (_closeProgrammCommand = new RelayCommand(CloseProgrammFunc));
        private RelayCommand _showHideMainWindCommand;
        public RelayCommand ShowHideMainWindCommand => _showHideMainWindCommand ?? (_showHideMainWindCommand = new RelayCommand(ShowHideMainWindFunc));

        private void OpenSettingsWindow()
        {
            SettingsWindow = new SettingsWindow();
            SettingsWindow.Owner = MainWindow;
            SettingsWindow.Show();
        }

    
        private void CloseProgrammFunc()
        {
            CanClose = true;
            MainWindow.Close();
        }
        private void ShowHideMainWindFunc()
        {
            if (MainWindow.IsVisible)
            {
                    MainWindow.Hide();
            }
            else
            {
                MainWindow.Show();
                MainWindow.Activate();
            }
        }

    }
}
