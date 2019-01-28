using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;
using Radio.Models;
using Radio.ViewModels;
using Radio.Views;

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
