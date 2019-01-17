using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;
using Radio.ViewModels;
using WPFSoundVisualizationLib;

namespace Radio.Behaviors
{
    class GifPlayerBehavior : Behavior<MediaElement>
    {

        public static readonly DependencyProperty GifPlayerBehaviorProperty =
            DependencyProperty.Register(
                nameof(GifPlayerBehavior),
                typeof(string),
                typeof(GifPlayerBehavior));

        public string GifPlayerFieldBehavior
        {
            get { return (string)GetValue(GifPlayerBehaviorProperty); }
            set { SetValue(GifPlayerBehaviorProperty, value); }
        }

        private void AssociatedObject_Initialized(object sender, EventArgs e)
        {
            var associatedObject = sender as MediaElement;
            associatedObject.Play();
        }
        private void AssociatedObject_MediaEnded(object sender, EventArgs e)
        {
            var associatedObject = sender as MediaElement;
            associatedObject.Position = TimeSpan.MinValue;
            associatedObject.Play();
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Initialized += AssociatedObject_Initialized;
            AssociatedObject.MediaEnded += AssociatedObject_MediaEnded;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
        }
    }
}
