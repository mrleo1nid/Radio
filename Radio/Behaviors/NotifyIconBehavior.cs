using Hardcodet.Wpf.TaskbarNotification;
using Radio.Models;
using System;
using System.Windows;
using System.Windows.Interactivity;

namespace Radio.Behaviors
{
    class NotifyIconBehavior : Behavior<TaskbarIcon>
    {
        public static readonly DependencyProperty NotifyIconBehaviorProperty =
            DependencyProperty.Register(
                nameof(NotifyIconBehavior),
                typeof(string),
                typeof(NotifyIconBehavior));

        public string SpectrumAnalyzerFieldBehavior
        {
            get { return (string)GetValue(NotifyIconBehaviorProperty); }
            set { SetValue(NotifyIconBehaviorProperty, value); }
        }

        private void AssociatedObject_Initialized(object sender, EventArgs e)
        {
            var associatedObject = sender as TaskbarIcon;
            AssociatedObject.Icon = Properties.Resources.icon;
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Initialized += AssociatedObject_Initialized;
            AssociatedObject.TrayMouseDoubleClick += AssociatedObjectOnTrayMouseDoubleClick;
        }

        private void AssociatedObjectOnTrayMouseDoubleClick(object sender, RoutedEventArgs routedEventArgs)
        {
            MainWindow mainWindow = Storage.WindowStorage["MainWindow"] as MainWindow;
            if (mainWindow.IsVisible)
            {
                mainWindow.Hide();
            }
            else
            {
                mainWindow.Show();
                mainWindow.Activate();
                mainWindow.Focus();
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
        }
    }
}
