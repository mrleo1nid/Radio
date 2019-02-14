using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Radio.Models
{
    public class Content
    {
        private Track track;
        public Track Track
        {
            get { return track; }
            set
            {
                track = value;
                OnPropertyChanged("Track");
            }
        }
        private Gif gif;
        public Gif Gif
        {
            get { return gif; }
            set
            {
                gif = value;
                OnPropertyChanged("Gif");
            }
        }
        private Playlist ownerPlaylist;
        public Playlist OwnerPlaylist
        {
            get { return ownerPlaylist; }
            set
            {
                ownerPlaylist = value;
                OnPropertyChanged("OwnerPlaylist");
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
