using Radio.Models;
using Radio.Views;
using System;
using System.Windows;
using System.Windows.Interactivity;

namespace Radio.Behaviors
{
    class SettingsWindowBehavior : Behavior<SettingsWindow>
    {

        public static readonly DependencyProperty SettingsWindowBehaviorProperty =
            DependencyProperty.Register(
                nameof(SettingsWindowBehavior),
                typeof(string),
                typeof(SettingsWindowBehavior));

        public string SettingsWindowFieldBehavior
        {
            get { return (string)GetValue(SettingsWindowBehaviorProperty); }
            set { SetValue(SettingsWindowBehaviorProperty, value); }
        }

        private void AssociatedObject_Initialized(object sender, EventArgs e)
        {
            var wind = AssociatedObject as SettingsWindow;
            Storage.WindowStorage["SettingsWindow"] = wind;
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Initialized += AssociatedObject_Initialized;
            AssociatedObject.Closed += AssociatedObjectOnClosed;
        }

        private void AssociatedObjectOnClosed(object sender, EventArgs eventArgs)
        {
            Storage.WindowStorage.Remove("SettingsWindow");
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
        }
    }
}
