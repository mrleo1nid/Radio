using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Radio.Models;
using Radio.Workers;
using Un4seen.Bass;

namespace Radio.ViewModels
{
    public class PlaylistsViewModel : ViewModel
    {
        private readonly MusicPlayer player = new MusicPlayer();

        public PlaylistsViewModel()
        {
            var downloader = new PlaylistDownloader();
            Volume = 50;
            Playlists = downloader.LoadPlaylists();
            Action calledMethod = LoadIconsFromUIThread;
            Application.Current.Dispatcher.BeginInvoke(calledMethod);
        }

        private Playlist _selectedPlaylist;

        public ObservableCollection<Playlist> Playlists { get; set; }

        public Playlist SelectedPlaylist
        {
            get { return _selectedPlaylist; }
            set
            {
                _selectedPlaylist = value;
                OnPropertyChanged(nameof(SelectedPlaylist));
            }
        }
        private int volume;
        public int Volume
        {
            get { return volume; }
            set
            {
                volume = value;
                OnPropertyChanged(nameof(Volume));
                VolumeChanged();
            }
        }

        private void LoadIconsFromUIThread()
        {
            var downloader = new PlaylistDownloader();
            for (int i = 0; i < Playlists.Count; i++)
            {
                Playlists[i] = downloader.LoadIcon(Playlists[i]);
            }

            SelectedPlaylist = Playlists[0];
        }


        #region Actions
        private void Play()
        {
            player.PlayOrPause(SelectedPlaylist.PlayedTrack);
        }
        private void Next()
        {
            SelectedPlaylist.PreviousTracks.Add(SelectedPlaylist.PlayedTrack);
            SelectedPlaylist = PlaylistDownloader.GenerateNewPlayed(SelectedPlaylist);
            player.StartPlay(SelectedPlaylist.PlayedTrack);
        }
        private void Previous()
        {
            var prev = SelectedPlaylist.PreviousTracks.LastOrDefault();
            if (prev != null)
            {
                SelectedPlaylist = PlaylistDownloader.GenerateNewPlayed(SelectedPlaylist);
                SelectedPlaylist.PlayedTrack = prev;
                SelectedPlaylist.PreviousTracks.Remove(prev);
                player.StartPlay(SelectedPlaylist.PlayedTrack);
            }   
        }

        private void VolumeChanged()
        {
            float newvalue = (float) Volume / 100;
            player.ChangeValue(newvalue);
        }
        #endregion

        #region Command
        private RelayCommand playCommand;
        public RelayCommand PlayCommand
        {
            get
            {
                return playCommand ??
                       (playCommand = new RelayCommand(Play));
            }
        }
        private RelayCommand nextCommand;
        public RelayCommand NextCommand
        {
            get
            {
                return nextCommand ??
                       (nextCommand = new RelayCommand(Next));
            }
        }
        private RelayCommand previousCommand;
        public RelayCommand PreviousCommand
        {
            get
            {
                return previousCommand ??
                       (previousCommand = new RelayCommand(Previous));
            }
        }
        #endregion
    }

}
