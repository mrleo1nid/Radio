using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Radio.Models;
using Radio.ViewModels;
using System.Drawing;
using System.Drawing.Imaging;
using ImageMagick;

namespace Radio.Workers
{
    public class PlaylistDownloader
    {
        private const string siteUrl = "http://www.theradio.ml";
        private const string playlistsUrl = siteUrl + "/playlists";

        public ObservableCollection<Playlist> LoadPlaylists()
        {
            var playlists = new ObservableCollection<Playlist>();
            var playlistsUrlArray = GetUrlArrayFromRequest(playlistsUrl);
            foreach (var url in playlistsUrlArray)
            {
                var playlist = FillPlaylistProperties(url);
                playlists.Add(playlist);
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
                ChangeImageResolution(fileCover.FullName);
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
            playlist.GifList = GetUrlArrayFromRequest(playlist.Url + "gifs").ToList();
            playlist.MusicList = GetUrlArrayFromRequest(playlist.Url + "music").ToList();
            playlist = GenerateNewPlayed(playlist);
            return playlist;
        }

        private static string[] GetUrlArrayFromRequest(string targetUrl)
        {
            var req = (HttpWebRequest) WebRequest.Create(targetUrl);
            string[] result;
            using (var resp = (HttpWebResponse) req.GetResponse())
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

        private static void DownloadFile(string url, FileInfo savePath)
        {
            WebClient webClient = new WebClient();
            webClient.DownloadFile(url, savePath.FullName);
        }

        public static Playlist GenerateNewPlayed(Playlist playlist)
        {
            Random rnd = new Random();
            int gifind = rnd.Next(playlist.GifList.Count);
            int trackind = rnd.Next(playlist.MusicList.Count);
            playlist.PlayedGif = siteUrl + playlist.GifList[gifind];
            playlist.PlayedTrack = siteUrl + playlist.MusicList[trackind];
            return playlist;
        }

        private static void ChangeImageResolution(string filepath)
        {
            FileInfo file = new FileInfo(filepath);
            MagickImage image = new MagickImage(file);
            image.Resize(128,128);
            image.Write(file);
        }

    }
}
