﻿using System;
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
        private BassEngine bassEngine;
        private string PlayedTrack;


        public PlaylistsViewModel()
        {
            if (!PlaylistDownloader.CheckForInternetConnection())
            {
               var res = MessageBox.Show("Отсуствует соединение с сервером. Попробывать ещё раз?", "Ошибка");
            }
            else
            {
                bassEngine = MainViewModel.BassEngine;
                var downloader = new PlaylistDownloader();
                Playlists = downloader.LoadPlaylists();
                Action calledMethod = LoadIconsFromUIThread;
                Application.Current.Dispatcher.BeginInvoke(calledMethod);
                Volume = 50;
            }
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

            SelectedPlaylist = Playlists.FirstOrDefault();
        }

        #region Actions
        private void Play()
        {
            if (PlayedTrack!=SelectedPlaylist.PlayedTrack)
            {
                bassEngine.OpenUrl(SelectedPlaylist.PlayedTrack);
                PlayedTrack = SelectedPlaylist.PlayedTrack;
                bassEngine.Play();
            }
            else
            {
                if (bassEngine.IsPlaying)
                {
                    bassEngine.Pause();
                }
                else
                {
                    bassEngine.Play();
                }
            }
        }
        private void Next()
        {
            SelectedPlaylist.PreviousTracks.Add(SelectedPlaylist.PlayedTrack);
            SelectedPlaylist.PreviousGif.Add(SelectedPlaylist.PlayedGif);
            SelectedPlaylist = PlaylistDownloader.GenerateNewPlayed(SelectedPlaylist);
            bassEngine.OpenUrl(SelectedPlaylist.PlayedTrack);
            PlayedTrack = SelectedPlaylist.PlayedTrack;
            bassEngine.Play();
        }
        private void Previous()
        {
            var prevTrack = SelectedPlaylist.PreviousTracks.LastOrDefault();
            var prevGif = SelectedPlaylist.PreviousGif.LastOrDefault();
            if (prevTrack != null && prevGif != null)
            {
                SelectedPlaylist = PlaylistDownloader.GenerateNewPlayed(SelectedPlaylist);
                SelectedPlaylist.PlayedTrack = prevTrack;
                SelectedPlaylist.PlayedGif = prevGif;
                SelectedPlaylist.PreviousTracks.Remove(prevTrack);
                SelectedPlaylist.PreviousGif.Remove(prevGif);
                bassEngine.OpenUrl(SelectedPlaylist.PlayedTrack);
                PlayedTrack = SelectedPlaylist.PlayedTrack;
                bassEngine.Play();
            }   
        }

        private void VolumeChanged()
        {
            float newvalue = (float) Volume / 100;
            bassEngine.ChangeValue(newvalue);
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
