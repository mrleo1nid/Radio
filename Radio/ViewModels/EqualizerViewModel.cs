using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radio.Models;
using Radio.Workers;
using WPFSoundVisualizationLib;

namespace Radio.ViewModels
{
    class EqualizerViewModel : ViewModel
    {
        public static BassEngine BassEngine { get; set; }
        public MainViewModel MainVM { get; set; }

        public EqualizerViewModel()
        {
            BassEngine = MainViewModel.BassEngine;
        }
    }
}
