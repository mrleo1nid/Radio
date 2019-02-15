using Radio.Helpers;
using Radio.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Radio.ViewModels
{
    public class PlaylistsViewModel : ViewModel
    {
        private NAudioEngine Engine;
        public MainViewModel mainViewModel { get; set; }

        private LocalDownoloadHelper downoloadHelper = new LocalDownoloadHelper();

        PlaylistDownloader playlistDownloader  = new PlaylistDownloader();

        public PlaylistsViewModel(MainViewModel mainViewModel)
        {
            Storage.VmStorage["PlaylistsViewModel"] = this;
            this.mainViewModel = mainViewModel;
            Engine = mainViewModel.Engine;
            downoloadHelper.InitWorkers();
            Playlists = playlistDownloader.LoadPlaylists();
            SelectedPlaylist = Playlists.FirstOrDefault();
            Action calledMethod = LoadIconsFromUIThread;
            Application.Current.Dispatcher.BeginInvoke(calledMethod);
            Volume = 50;
        }

        public ObservableCollection<Playlist> Playlists { get; set; }

        private Playlist _selectedPlaylist;
        public Playlist SelectedPlaylist
        {
            get { return _selectedPlaylist; }
            set
            {
                if (value.Url!=null)
                {
                    if (_selectedPlaylist != value)
                    {
                        _selectedPlaylist = value;
                        OnPropertyChanged(nameof(SelectedPlaylist));
                        SelectedPlaylistChange();
                    }
                }
            }
        }
        private Content playedContent;
        public Content PlayedContent
        {
            get { return playedContent; }
            set
            {
                playedContent = value;
                OnPropertyChanged(nameof(PlayedContent));
                PlayedContentChanged();
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
        private bool ispaneOpen;
        public bool IspaneOpen
        {
            get { return ispaneOpen; }
            set
            {
                ispaneOpen = value;
                OnPropertyChanged(nameof(IspaneOpen));
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
                if (Engine.IsPlaying)
                {
                    Engine.Pause();
                }
                else
                {
                    Engine.Play();
                }
        }
        private void Next()
        {
            playlistDownloader.GenerateNextContant(SelectedPlaylist);
            PlayedContent = SelectedPlaylist.PlayedContent;
        }
        private void Previus()
        {
            playlistDownloader.ReturnPrevius(SelectedPlaylist);
            PlayedContent = SelectedPlaylist.PlayedContent;
        }
        private void SelectedPlaylistChange()
        {
            PlayedContent = SelectedPlaylist.PlayedContent;
        }
        public void ReloadWithReconnect()
        {
            var downloader = new PlaylistDownloader();
            Playlists = downloader.LoadPlaylists();
            SelectedPlaylist = Playlists.FirstOrDefault();
            Action calledMethod = LoadIconsFromUIThread;
            Application.Current.Dispatcher.BeginInvoke(calledMethod);
            Volume = 50;
        }

        private void OpenGamburgerMenu()
        {
            IspaneOpen = !IspaneOpen;
            if (!IspaneOpen)
            {
                mainViewModel.OneColumnWidth = 55;
            }
            else
            {
                mainViewModel.OneColumnWidth = 160;
            }
        }

        private void VolumeChanged()
        {
            float newvalue = (float) Volume / 100;
            Engine.ChangeValue(newvalue);
        }
        private void PlayedContentChanged()
        {
            if (Settings.LoadSettings().DownoloadTrackLocal)
            {
                downoloadHelper.DownoloadContentLocal(SelectedPlaylist.PlayedContent);
            }
            SelectedPlaylist.PlayedContent.Track.HaveLocalPath =
                downoloadHelper.CheckLocalPath(SelectedPlaylist.PlayedContent.Track.LocalPath);
            SelectedPlaylist.PlayedContent.Gif.HaveLocalPath =
                downoloadHelper.CheckLocalPath(SelectedPlaylist.PlayedContent.Gif.LocalPath);
            Engine.OpenFileAndUrl(PlayedContent.Track);
            Engine.Play();
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
        private RelayCommand previusCommand;
        public RelayCommand PreviusCommand
        {
            get
            {
                return previusCommand ??
                       (previusCommand = new RelayCommand(Previus));
            }
        }
        private RelayCommand openHamburgMenu;
        public RelayCommand OpenHamburgMenu
        {
            get
            {
                return openHamburgMenu ??
                       (openHamburgMenu = new RelayCommand(OpenGamburgerMenu));
            }
        }
        #endregion
    }

}
