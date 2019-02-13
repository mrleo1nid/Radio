using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Radio.Models
{
   public class Gif
    {
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
        private string localPath;
        public string LocalPath
        {
            get { return localPath; }
            set
            {
                localPath = value;
                OnPropertyChanged("LocalPath");
            }
        }
        private bool haveLocalPath;
        public bool HaveLocalPath
        {
            get { return haveLocalPath; }
            set
            {
                haveLocalPath = value;
                OnPropertyChanged("HaveLocalPath");
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
