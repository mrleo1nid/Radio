using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;
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
            SecondToReconnect = 30;
            if (NotConectedVisibility)
            {
                StartReconnectTimer();
            }
        }
        private int _secondToReconnect;
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
            if (SecondToReconnect<=0)
            {
                SecondToReconnect = 30;
                ReconnectClick();
            }
        }
        private void ReconnectClick()
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
                mainViewModel.PlaylistsVM.ReloadWithReconnect();
            }
        }
        private void StartReconnectTimer()
        {
            TimerCallback tm = new TimerCallback(Reconnect);
            ReconnectTimer = new Timer(tm, null, 0, 1000);
        }

        private RelayCommand _reconnectCommand;
        public RelayCommand ReconnectCommand => _reconnectCommand ?? (_reconnectCommand = new RelayCommand(ReconnectClick));
    }
}
