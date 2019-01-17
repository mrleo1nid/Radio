using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radio.Models;
using Radio.Workers;

namespace Radio.ViewModels
{
   public class EqualizerViewModel : ViewModel
    {
        public static BassEngine BassEngine { get; set; }
        public MainViewModel MainVM { get; set; }

        public float[] EqualizerValues
        {
            get => _equalizerValues;
            set
            {
                if (_equalizerValues != value)
                {
                    _equalizerValues = value;
                    OnPropertyChanged(nameof(EqualizerValues));
                }
            }
        }

        private float[] _equalizerValues;

        public EqualizerViewModel()
        {
            BassEngine = MainViewModel.BassEngine;
        }
    }
}
