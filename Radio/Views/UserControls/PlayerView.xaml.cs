using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Un4seen.Bass;


namespace Radio.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для PlayerView.xaml
    /// </summary>
    public partial class PlayerView : UserControl
    {
        public PlayerView()
        {
            InitializeComponent();
        }

        private void FrameworkElement_OnInitialized(object sender, EventArgs e)
        {
            MediaElement media = (MediaElement) sender;
            media.Play();
        }

        private void MediaElement_OnMediaEnded(object sender, RoutedEventArgs e)
        {
            MediaElement media = (MediaElement)sender;
            media.Position = TimeSpan.MinValue;
            media.Play();
        }
    }
}
