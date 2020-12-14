using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApplicationWorkingTime
{
    class Day
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan LunchInMin { get; set; } = new TimeSpan(0, 45, 0);
        public TimeSpan Balance
        {
            get
            {
                return EndTime - StartTime - LunchInMin;
            }
        }
    }
}
