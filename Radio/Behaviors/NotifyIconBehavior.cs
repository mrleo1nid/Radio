using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;
using Hardcodet.Wpf.TaskbarNotification;
using Radio.ViewModels;
using WPFSoundVisualizationLib;

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
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
        }
    }
}
