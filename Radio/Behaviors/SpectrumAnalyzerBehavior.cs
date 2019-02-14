using System;
using System.Windows;
using System.Windows.Interactivity;
using Radio.Helpers;
using WPFSoundVisualizationLib;

namespace Radio.Behaviors
{
    class SpectrumAnalyzerBehavior : Behavior<SpectrumAnalyzer>
    {

        public static readonly DependencyProperty SpectrumAnalyzerBehaviorProperty =
            DependencyProperty.Register(
                nameof(SpectrumAnalyzerBehavior),
                typeof(string),
                typeof(SpectrumAnalyzerBehavior));

        public string SpectrumAnalyzerFieldBehavior
        {
            get { return (string)GetValue(SpectrumAnalyzerBehaviorProperty); }
            set { SetValue(SpectrumAnalyzerBehaviorProperty, value); }
        }

        private void AssociatedObject_Initialized(object sender, EventArgs e)
        {
            var associatedObject = sender as SpectrumAnalyzer;
            associatedObject.RegisterSoundPlayer(NAudioEngine.Instance);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Initialized += AssociatedObject_Initialized;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
        }
    }
}
