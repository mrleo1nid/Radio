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
        private ObservableCollection<Content> contentCollection;
        public ObservableCollection<Content> ContentCollection
        {
            get { return contentCollection; }
            set
            {
                contentCollection = value;
                OnPropertyChanged("ContentCollection");
            }
        }
        private Content playedContent;
        public Content PlayedContent
        {
            get { return playedContent; }
            set
            {
                playedContent = value;
                OnPropertyChanged("PlayedContent");
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

