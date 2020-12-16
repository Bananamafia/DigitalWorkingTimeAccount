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

        public TimeSpan LunchInMin { get; set; }

        public TimeSpan Balance
        {
            get
            {
                return EndTime - StartTime - LunchInMin;
            }
        }
    }
}
