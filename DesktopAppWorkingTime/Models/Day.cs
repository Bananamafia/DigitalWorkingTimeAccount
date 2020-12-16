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

        private TimeSpan _lunchInMin = new TimeSpan(0, 45, 0);
        public TimeSpan LunchInMin
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
                return EndTime - StartTime - LunchInMin;
            }
        }
    }
}
