using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Radio.Models;
using Radio.Workers;

namespace Radio.ViewModels
{
   public class NotConectionControlViewModel : ViewModel
    {
        private MainViewModel mainViewModel { get; set; }
        private Timer ReconnectTimer = null;

        public NotConectionControlViewModel(MainViewModel main)
        {
            this.mainViewModel = main;
            mainViewModel.Conected = PlaylistDownloader.CheckForInternetConnection();
            NotConectedVisibility = !mainViewModel.Conected;
            TimerCallback tm = new TimerCallback(CheckTimerConnect);
            Timer timer = new Timer(tm, null, 0, 60000);
            if (NotConectedVisibility)
            {
                StartReconnectTimer();
            }
        }
        private int _secondToReconnect = 30;
        public int SecondToReconnect
        {
            get { return _secondToReconnect; }
            set
            {
                _secondToReconnect = value;
                OnPropertyChanged(nameof(SecondToReconnect));
            }
        }
        private bool _notConectedVisibility;
        public bool NotConectedVisibility
        {
            get { return _notConectedVisibility; }
            set
            {
                _notConectedVisibility = value;
                OnPropertyChanged(nameof(NotConectedVisibility));
            }
        }

        private void Reconnect(object obj)
        {
            SecondToReconnect -= 1;
            if (SecondToReconnect==0)
            {
                CheckTimerConnect(null);
                SecondToReconnect = 30;
            }
        }

        private void CheckTimerConnect(object obj)
        {
            mainViewModel.Conected = PlaylistDownloader.CheckForInternetConnection();
            NotConectedVisibility = !mainViewModel.Conected;
            mainViewModel.CanClose = NotConectedVisibility;
            if (NotConectedVisibility)
            {
                StartReconnectTimer();
            }
            else
            {
                mainViewModel.PlaylistsVM = new PlaylistsViewModel(mainViewModel);
            }
        }

        private void StartReconnectTimer()
        {
                TimerCallback tm = new TimerCallback(Reconnect);
                ReconnectTimer = new Timer(tm, null, 0, 1000);
        }
    }
}
