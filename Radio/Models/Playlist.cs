using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Radio.Models
{
    public class Playlist
    {

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        private string url;
        public string Url
        {
            get { return url; }
            set
            {
                url = value;
                OnPropertyChanged("Url");
            }
        }

        private string imagePath;
        public string ImagePath
        {
            get { return imagePath; }
            set
            {
                imagePath = value;
                OnPropertyChanged("ImagePath");
            }
        }

        private string backgroundImagePath;
        public string BackgroundImagePath
        {
            get { return backgroundImagePath; }
            set
            {
                backgroundImagePath = value;
                OnPropertyChanged("BackgroundImagePath");
            }
        }

        private ObservableCollection<Track> musicList;
        public ObservableCollection<Track> MusicList
        {
            get { return musicList; }
            set
            {
                musicList = value;
                OnPropertyChanged("MusicList");
            }
        }

        private ObservableCollection<Gif> gifList;
        public ObservableCollection<Gif> GifList
        {
            get { return gifList; }
            set
            {
                gifList = value;
                OnPropertyChanged("GifList");
            }
        }

        private Track playedTrack;
        public Track PlayedTrack
        {
            get { return playedTrack; }
            set
            {
                playedTrack = value;
                OnPropertyChanged("PlayedTrack");
            }
        }
        private List<Track> previousTracks = new List<Track>();
        public List<Track> PreviousTracks
        {
            get { return previousTracks; }
            set
            {
                previousTracks = value;
                OnPropertyChanged("PreviousTracks");
            }
        }

        private Gif playedGif;
        public Gif PlayedGif
        {
            get
            { return playedGif;}
            set
            {
                playedGif = value;
                OnPropertyChanged("PlayedGif");
            }
        }
        private List<Gif> previousGif = new List<Gif>();
        public List<Gif> PreviousGif
        {
            get { return previousGif; }
            set
            {
                previousGif = value;
                OnPropertyChanged("PreviousGif");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}

