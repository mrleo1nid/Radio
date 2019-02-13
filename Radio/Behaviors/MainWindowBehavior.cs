using Radio.Models;
using Radio.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Interactivity;

namespace Radio.Behaviors
{
    class MainWindowBehavior : Behavior<MainWindow>
    {

        public static readonly DependencyProperty MainWindowBehaviorProperty =
            DependencyProperty.Register(
                nameof(MainWindowBehavior),
                typeof(string),
                typeof(MainWindowBehavior));

        private void AssociatedObject_Initialized(object sender, EventArgs e)
        {
            MainWindow wind = sender as MainWindow;
            Storage.WindowStorage["MainWindow"]= wind;
            Settings settings = Settings.LoadSettings();
            ChangeWindowStyle(settings.WindowStyle, wind);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Initialized += AssociatedObject_Initialized;
            AssociatedObject.Closing += AssociatedObjectOnClosing;
        }

        private void AssociatedObjectOnClosing(object sender, CancelEventArgs e)
        {
            MainWindow window = AssociatedObject as MainWindow;
            MainViewModel mainViewModel = Storage.VmStorage["MainViewModel"] as MainViewModel;
            if (!mainViewModel.CanClose)
            {
                e.Cancel = true;
                window.Hide();
            }
        }

        private void ChangeWindowStyle(int style, MainWindow window)
        {
            switch (style)
            {
                case 1:
                    window.WindowState = WindowState.Maximized;
                    break;
                case 2:
                    window.WindowState = WindowState.Maximized;
                    window.WindowStyle = WindowStyle.None;
                    break;
                default:
                    break;
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
        }
    }
}
