using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DesktopAppWorkingTime.Models
{
    class Day : INotifyPropertyChanged
    {
        
        
        //---INotifyPropertyChanged---
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
