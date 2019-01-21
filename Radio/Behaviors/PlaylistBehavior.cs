using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using Radio.ViewModels;
using Radio.Views.UserControls;
using WPFSoundVisualizationLib;

namespace Radio.Behaviors
{
    class PlaylistBehavior : Behavior<PlaylistNewView>
    {


        public static readonly DependencyProperty PlaylistBehaviorProperty =
            DependencyProperty.Register(
                nameof(PlaylistBehavior),
                typeof(string),
                typeof(PlaylistBehavior));

        public string SpectrumAnalyzerFieldBehavior
        {
            get { return (string) GetValue(PlaylistBehaviorProperty); }
            set { SetValue(PlaylistBehaviorProperty, value); }
        }

        private void AssociatedObject_Initialized(object sender, EventArgs e)
        {
            var associatedObject = sender as PlaylistNewView;
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Initialized += AssociatedObject_Initialized;
            AssociatedObject.MouseEnter += AssociatedObjectOnMouseEnter;
            AssociatedObject.MouseDown += AssociatedObjectOnMouseDown;
        }

        private void AssociatedObjectOnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var playlist = sender as PlaylistNewView;
            if (playlist.Visibility == Visibility.Visible)
            {
                playlist.Visibility = Visibility.Hidden;
            }
        }

        private void AssociatedObjectOnMouseEnter(object sender, MouseEventArgs e)
        {
            var playlist = sender as PlaylistNewView;
            if (playlist.Visibility == Visibility.Hidden)
            {
                playlist.Visibility = Visibility.Visible;
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
        }
    }
}
