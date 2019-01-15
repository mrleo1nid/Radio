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
using Radio.Workers;
using Un4seen.Bass;

namespace Radio.ViewModels
{
    class MainViewModel : ViewModel
    {
        public MainViewModel()
        {
            PlaylistsVM = new PlaylistsViewModel();
        }

        public PlaylistsViewModel PlaylistsVM { get; set; }
            

        private RelayCommand _addCommand;
        public RelayCommand AddCommand => _addCommand ?? (_addCommand = new RelayCommand(AddPlaylist));
       
       
        private void AddPlaylist()
        {
            throw new NotImplementedException();
        }
    }
}
