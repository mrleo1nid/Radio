using System.Collections.Generic;
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

        private List<string> musicList;
        public List<string> MusicList
        {
            get { return musicList; }
            set
            {
                musicList = value;
                OnPropertyChanged("MusicList");
            }
        }

        private List<string> gifList;
        public List<string> GifList
        {
            get { return gifList; }
            set
            {
                gifList = value;
                OnPropertyChanged("GifList");
            }
        }

        private string playedTrack;
        public string PlayedTrack
        {
            get { return playedTrack; }
            set
            {
                playedTrack = value;
                OnPropertyChanged("PlayedTrack");
            }
        }
        private List<string> previousTracks = new List<string>();
        public List<string> PreviousTracks
        {
            get { return previousTracks; }
            set
            {
                previousTracks = value;
                OnPropertyChanged("PreviousTracks");
            }
        }

        private string playedGif;
        public string PlayedGif
        {
            get
            { return playedGif;}
            set
            {
                playedGif = value;
                OnPropertyChanged("PlayedGif");
            }
        }
        private List<string> previousGif = new List<string>();
        public List<string> PreviousGif
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

