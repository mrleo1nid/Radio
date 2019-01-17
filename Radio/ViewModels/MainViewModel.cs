using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
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
        }
       
        public static PlaylistsViewModel PlaylistsVM { get; set; }
        public static BassEngine  BassEngine { get; set; }


        private RelayCommand _addCommand;
        public RelayCommand AddCommand => _addCommand ?? (_addCommand = new RelayCommand(AddPlaylist));
       
       
        private void AddPlaylist()
        {
            throw new NotImplementedException();
        }

        private RelayCommand _openDopWindow;
        public RelayCommand OpenDopWindowCommand => _openDopWindow ?? (_openDopWindow = new RelayCommand(OpenDopWindow));


        private void OpenDopWindow()
        {
            EqualizerWindow wind = new EqualizerWindow { DataContext = new EqualizerViewModel()};
            wind.Show();
        }

       
    }
}
