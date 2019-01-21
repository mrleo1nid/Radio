using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Radio.Behaviors;
using Radio.Models;
using Radio.Views;
using Radio.Workers;
using Un4seen.Bass;

namespace Radio.ViewModels
{
   public class MainViewModel : ViewModel
    {
        public MainViewModel()
        {
            BassEngine = BassEngine.Instance;
            PlaylistsVM = new PlaylistsViewModel();
            Settings = Settings.LoadSettings();
            CanClose = !Settings.MinimizeToTrayOnClose;
        }


        public static MainWindow MainWindow { get; set; }
        public static PlaylistsViewModel PlaylistsVM { get; set; }
        public static BassEngine  BassEngine { get; set; }
        public static SettingsWindow SettingsWindow { get; set; }


        public static bool CanClose { get; set; }
        private static Settings settings;
        public static Settings Settings
        {
            get { return settings; }
            set
            {
                settings = value;
                CanClose = !settings.MinimizeToTrayOnClose;
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
