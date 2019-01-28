using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radio.Models;

namespace Radio.ViewModels
{
    class NotConectionControlViewModel : ViewModel
    {
        public NotConectionControlViewModel()
        {
          
        }
        private int timer;
        public int Timer
        {
            get { return timer; }
            set
            {
                timer = value;
                OnPropertyChanged(nameof(Timer));
            }
        }
    }
}
