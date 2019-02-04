﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using MahApps.Metro.Controls;
using Radio.Models;
using Radio.ViewModels;

namespace Radio.Behaviors
{
    class HamburgerBehavior : Behavior<ListBox>
    {

        public static readonly DependencyProperty HamburgerBehaviorProperty =
            DependencyProperty.Register(
                nameof(HamburgerBehavior),
                typeof(string),
                typeof(HamburgerBehavior));

        public string HamburgerFieldBehavior
        {
            get { return (string)GetValue(HamburgerBehaviorProperty); }
            set { SetValue(HamburgerBehaviorProperty, value); }
        }

        private void AssociatedObject_Initialized(object sender, EventArgs e)
        {
            var associatedObject = sender as ListBox;
            associatedObject.Name = "";
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Initialized += AssociatedObject_Initialized;
            AssociatedObject.Loaded += AssociatedObjectOnLoaded;
            AssociatedObject.SelectionChanged += AssociatedObjectOnSelectionChanged;
        }

        private void AssociatedObjectOnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            var associatedObject = sender as ListBox;
            var index = associatedObject.SelectedIndex;
            if (index==0)
            {
                PlaylistsViewModel playlistsViewModel = Storage.VmStorage["PlaylistsViewModel"] as PlaylistsViewModel;
                playlistsViewModel.IspaneOpen = !playlistsViewModel.IspaneOpen;
                associatedObject.SelectedItem = selectionChangedEventArgs.RemovedItems[0];
            }
        }

        private void AssociatedObjectOnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var associatedObject = sender as ListBox;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
        }
    }
}
