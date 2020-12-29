using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class PairConsecutiveStop
    {
        public int first_stop_id { get; set; }
        public int second_stop_id { get; set; }
        public int distanceKM { get; set; }
        public TimeSpan average_drive_time { get; set; }
    }
}
