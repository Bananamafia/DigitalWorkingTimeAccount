using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DesktopAppWorkingTime.Models
{
    class Day
    {
        public DateTime Date { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        private int _lunchInMin = 45;
        public int LunchInMin
        {
            get { return _lunchInMin; }
            set
            {
                _lunchInMin = value;
            }
        }

        public TimeSpan Balance
        {
            get
            {
                return EndTime - StartTime - new TimeSpan(0, LunchInMin, 0);
            }
        }
    }
}
