using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
     public class LineTiming
    {
        public int TripId { get; set; }
        public int LineId { get; set; }

        public int Code { get; set; }
        
        public TimeSpan StartTime { get; set; }

        public string Destination { get; set; }

        public TimeSpan TimeAtStop { get; set; }

        public TimeSpan TimeFromNow { get; set; }

        public bool busArrived { get; set; }

    }
}
