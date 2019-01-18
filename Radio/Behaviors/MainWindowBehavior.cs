using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using Radio.Models;
using Radio.ViewModels;
using WPFSoundVisualizationLib;

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
            MainViewModel.MainWindow = wind;
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
                MainViewModel.MainWindow.Hide();
                e.Cancel = true;
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
        }
    }
}
