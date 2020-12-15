using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DesktopAppWorkingTime.Models
{
    class Day : INotifyPropertyChanged
    {
        public DateTime Date { get; set; }

        private DateTime _startTime;
        public DateTime StartTime
        {
            get { return _startTime; }
            set
            {
                _startTime = value;
                OnPropertyChanged("StartTime");
                OnPropertyChanged("Balance");
            }
        }

        private DateTime _endTime;
        public DateTime EndTime
        {
            get { return _endTime; }
            set
            {
                _endTime = value;
                OnPropertyChanged("EndTime");
                OnPropertyChanged("Balance");
            }
        }

        private TimeSpan _lunchInMin = new TimeSpan(0, 45, 0);
        public TimeSpan LunchInMin
        {
            get { return _lunchInMin; }
            set
            {
                _lunchInMin = value;
                OnPropertyChanged("LunchInMin");
                OnPropertyChanged("Balance");
            }
        }

        public TimeSpan Balance
        {
            get
            {
                return EndTime - StartTime - LunchInMin;
            }
        }


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
