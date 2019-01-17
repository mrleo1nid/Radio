using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;
using System.Windows.Media;
using MahApps.Metro.Controls;
using Radio.ViewModels;
using WPFSoundVisualizationLib;

namespace Radio.Behaviors
{
    class EqualizerBehavior : Behavior<Equalizer>
    {

        public static readonly DependencyProperty EqualizerBehaviorProperty =
            DependencyProperty.Register(
                nameof(EqualizerBehavior),
                typeof(string),
                typeof(EqualizerBehavior));

        public string EqualizerFieldBehavior
        {
            get { return (string)GetValue(EqualizerBehaviorProperty); }
            set { SetValue(EqualizerBehaviorProperty, value); }
        }

        private void AssociatedObject_Initialized(object sender, EventArgs e)
        {
        }


        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Initialized += AssociatedObject_Initialized;
            AssociatedObject.DataContextChanged += AssociatedObject_DataContextChanged;
        }

        private void AssociatedObject_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var associatedObject = sender as Equalizer;
            ((EqualizerViewModel)associatedObject.DataContext).EqualizerValues = associatedObject.EqualizerValues;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
        }
    }
}
