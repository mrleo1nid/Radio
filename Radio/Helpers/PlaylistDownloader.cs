using Newtonsoft.Json;
using Radio.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Radio.Helpers
{
    public class PlaylistDownloader
    {
        private const string siteUrl = "http://www.theradio.ml";
        private const string playlistsUrl = siteUrl + "/playlists";

        public ObservableCollection<Playlist> LoadPlaylists()
        {
            var playlists = new ObservableCollection<Playlist>();
            var playlistsUrlArray = GetUrlArrayFromRequest(playlistsUrl);
            if (playlistsUrlArray!=null)
            {
                foreach (var url in playlistsUrlArray)
                {
                    var playlist = FillPlaylistProperties(url);
                    playlists.Add(playlist);
                }
            }
            return playlists;
        }


        public Playlist LoadIcon(Playlist playlist)
        {
            var directory = new DirectoryInfo("Data");
            var fileBackground = new FileInfo($"Data\\{playlist.Name}-background.gif");
            var fileCover = new FileInfo($"Data\\{playlist.Name}-cover.gif");
            playlist.BackgroundImagePath = $"{playlist.Url}background.gif";
            playlist.ImagePath = $"{playlist.Url}cover.gif";
            if (!directory.Exists)
            {
                Directory.CreateDirectory("Data");
            }
            if (!fileBackground.Exists)
            {
                DownloadFile(playlist.BackgroundImagePath, fileBackground);
                playlist.BackgroundImagePath = fileBackground.FullName;
            }
            else
            {
                playlist.BackgroundImagePath = fileBackground.FullName;
            }
            if (!fileCover.Exists)
            {
                DownloadFile(playlist.ImagePath, fileCover);
                playlist.ImagePath = fileCover.FullName;
            }
            else
            {
                playlist.BackgroundImagePath = fileBackground.FullName;
            }

            return playlist;
        }

        private Playlist FillPlaylistProperties(string elem)
        {
            var playlist = new Playlist
            {
                Name = elem,
                Url = $"{siteUrl}/{elem}/"
            };
            playlist.GifList = LoadGifList(playlist);
            playlist.MusicList = LoadTrackList(playlist);
            playlist.ContentCollection = new ObservableCollection<Content>();
            GenerateNextContant(playlist);
            return playlist;
        }

        private static string[] GetUrlArrayFromRequest(string targetUrl)
        {
            try
            {
                string[] result;
                var req = (HttpWebRequest)WebRequest.Create(targetUrl);
                using (var resp = (HttpWebResponse)req.GetResponse())
                {
                    string response;
                    using (var stream = new StreamReader(resp.GetResponseStream(), Encoding.UTF8))
                    {
                        response = stream.ReadToEnd();
                    }
                    result = JsonConvert.DeserializeObject<string[]>(response);
                }
                return result;
            }
            catch (Exception e)
            {
                return null;
            }  
        }
        private ObservableCollection<Track> LoadTrackList(Playlist playlist)
        {
            string trackUrl = playlist.Url + "music";
            var urlarray = GetUrlArrayFromRequest(trackUrl);
            ObservableCollection<Track> tracks = new ObservableCollection<Track>();
            foreach (var url in urlarray)
            {
                Track track = new Track();
                track.Url =siteUrl+ url;
                track.OwnerPlaylist = playlist;
                track.Name = url.Split('/').LastOrDefault();
                track.LocalPath = GenerateLocalPath(track);
                FileInfo info = new FileInfo(track.LocalPath);
                track.HaveLocalPath = info.Exists;
                tracks.Add(track);
            }

            return tracks;
        }
        private ObservableCollection<Gif> LoadGifList(Playlist playlist)
        {
            string gifkUrl = playlist.Url + "gifs";
            var urlarray = GetUrlArrayFromRequest(gifkUrl);
            ObservableCollection<Gif> gifs = new ObservableCollection<Gif>();
            foreach (var url in urlarray)
            {
                Gif gif = new Gif();
                gif.Url = siteUrl+ url;
                gif.OwnerPlaylist = playlist;
                gif.LocalPath = GenerateLocalPath(gif);
                FileInfo info = new FileInfo(gif.LocalPath);
                gif.HaveLocalPath = info.Exists;
                gifs.Add(gif);
            }
            return gifs;
        }
        private static void DownloadFile(string url, FileInfo savePath)
        {
            try
            {
                WebClient webClient = new WebClient();
                webClient.DownloadFile(url, savePath.FullName);
            }
            catch (Exception e)
            {
             return;
            }
        }
        public void GenerateNextContant(Playlist playlist)
        {
            int index = playlist.ContentCollection.IndexOf(playlist.PlayedContent);
            if (index < playlist.ContentCollection.Count-1)
            {
                playlist.PlayedContent = playlist.ContentCollection[index + 1];
            }
            else
            {
                Random rnd = new Random();
                Content content = new Content();
                content.Track = playlist.MusicList[rnd.Next(0, playlist.MusicList.Count)]; ;
                content.Gif = playlist.GifList[rnd.Next(0, playlist.GifList.Count)];
                content.OwnerPlaylist = playlist;
                content.Id = Guid.NewGuid();
                playlist.ContentCollection.Add(content);
                playlist.PlayedContent = content;
                if (playlist.ContentCollection.Count >= 50)
                {
                    playlist.ContentCollection.RemoveAt(0);
                }
            }
        }
        public void ReturnPrevius(Playlist playlist)
        {
            int index = playlist.ContentCollection.IndexOf(playlist.PlayedContent);
            if (index>0)
            {
                playlist.PlayedContent = playlist.ContentCollection[index - 1];
            }
        }

        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://www.theradio.ml"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public string GenerateLocalPath(Track track)
        {
            string localPath = Environment.CurrentDirectory + "\\Downoload\\Music";
            localPath += $"\\{track.OwnerPlaylist.Name}\\{track.Name}";
            return localPath;
        }
        public string GenerateLocalPath(Gif gif)
        {
            string localPath = Environment.CurrentDirectory + "\\Downoload\\Gif";
            localPath += $"\\{gif.OwnerPlaylist.Name}\\{gif.Url.Split('/').LastOrDefault()}";
            return localPath;
        }
    }
}
