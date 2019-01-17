using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using Radio.ViewModels;
using WPFSoundVisualizationLib;

namespace Radio.Behaviors
{
    class MainWindowBehavior : Behavior<MainWindow>
    {
        private static MainWindow window;

        public static readonly DependencyProperty MainWindowBehaviorProperty =
            DependencyProperty.Register(
                nameof(MainWindowBehavior),
                typeof(string),
                typeof(MainWindowBehavior));

        public string MainWindowFieldBehavior
        {
            get { return (string)GetValue(MainWindowBehaviorProperty); }
            set { SetValue(MainWindowBehaviorProperty, value); }
        }

        private void AssociatedObject_Initialized(object sender, EventArgs e)
        {
            MainWindow wind = sender as MainWindow;
            window = wind;
            createTrayIcon();
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Initialized += AssociatedObject_Initialized;
            AssociatedObject.Closing += AssociatedObjectOnClosing;
        }

        private void AssociatedObjectOnClosing(object sender, CancelEventArgs e)
        {
            if (!MainViewModel.CanClose)
            { 
                e.Cancel = true; 
                MainViewModel.CurrentWindowState = window.WindowState;
                (MainViewModel.TrayMenu.Items[0] as MenuItem).Header = "Show";
               window.Hide();
            }
            else
            {
                MainViewModel.TrayIcon.Visible = false;
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
        }

        private bool createTrayIcon()
        {
            bool result = false;
            if (MainViewModel.TrayIcon == null)
            { 
                MainViewModel.TrayIcon = new System.Windows.Forms.NotifyIcon(); 
                MainViewModel.TrayIcon.Icon = Radio.Properties.Resources.favicon;
                MainViewModel.TrayIcon.Text = "Here is tray icon text.";
                MainViewModel.TrayMenu = window.Resources["TrayMenu"] as ContextMenu;
                MainViewModel.TrayIcon.Click += delegate (object sender, EventArgs e) {
                    if ((e as System.Windows.Forms.MouseEventArgs).Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        ShowHideMainWindow();
                    }
                    else
                    {
                        MainViewModel.TrayMenu.IsOpen = true;
                        window.Activate();
                    }
                };
                result = true;
            }
            else
            { 
                result = true;
            }
            MainViewModel.TrayIcon.Visible = true; 
            return result;
        }

        private void ShowHideMainWindow()
        {
            MainViewModel.TrayMenu.IsOpen = false; 
            if (window.IsVisible)
            {
                window.Hide();
                (MainViewModel.TrayMenu.Items[0] as MenuItem).Header = "Show";
            }
            else
            {
                window.Show();
                (MainViewModel.TrayMenu.Items[0] as MenuItem).Header = "Hide";
                window.WindowState = MainViewModel.CurrentWindowState;
                window.Activate(); 
            }
        }
        public static void MenuExitClick()
        {
            MainViewModel.CanClose = true;
            window.Close();
        }

    }
}
