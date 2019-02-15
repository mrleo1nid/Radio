using Radio.Models;
using System;
using System.ComponentModel;
using System.IO;
using System.Net;

namespace Radio.Helpers
{
    public class LocalDownoloadHelper
    {
        private readonly BackgroundWorker downoloadBackgroundWorker = new BackgroundWorker();
        private Content pendingDownoloadWorkerContent;
        public int GifProgress;
        public int TrackProgress;

        public void InitWorkers()
        {
            downoloadBackgroundWorker.DoWork += downoloadBackgroundWorker_DoWork;
            downoloadBackgroundWorker.RunWorkerCompleted += downoloadBackgroundWorker_RunWorkerCompleted;
            downoloadBackgroundWorker.WorkerSupportsCancellation = true;
        }
        public bool CheckLocalPath(string path)
        {
            FileInfo info = new FileInfo(path);
            return info.Exists;
        }
        private void downoloadBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Content Params = e.Argument as Content;
            FileInfo giFileInfo = new FileInfo(Params.Gif.LocalPath);
            FileInfo trackFileInfo = new FileInfo(Params.Track.LocalPath);
            if (!giFileInfo.Exists)
            {
                DownloadFile(Params.Gif.Url,giFileInfo);
            }
            if (!trackFileInfo.Exists)
            {
                DownloadFile(Params.Track.Url, trackFileInfo);
            }
        }
        private void downoloadBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                if (!downoloadBackgroundWorker.IsBusy)
                    downoloadBackgroundWorker.RunWorkerAsync(pendingDownoloadWorkerContent);
            }
        }
        public void DownoloadContentLocal(Content content)
        {
            if (downoloadBackgroundWorker.IsBusy)
            {
                pendingDownoloadWorkerContent = content;
                downoloadBackgroundWorker.CancelAsync();
                return;
            }
            else
            {
                downoloadBackgroundWorker.RunWorkerAsync(content);
            }      
        }

        private static void DownloadFile(string url, FileInfo savePath)
        {
            try
            {
                DirectoryInfo directory = new DirectoryInfo(savePath.DirectoryName);
                if (!directory.Exists)
                {
                    directory.Create();
                }
                WebClient webClient = new WebClient();
                webClient.DownloadFile(url, savePath.FullName);
            }
            catch (Exception e)
            {
                return;
            }
        }
    }
}
