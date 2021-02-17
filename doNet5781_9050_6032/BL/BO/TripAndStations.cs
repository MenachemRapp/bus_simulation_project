using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class TripAndStations
    {
        public int TripId { get; set; }
        public int LineId { get; set; }
        public TimeSpan startTime { get; set; }
            
        public string Destination { get; set; }
        public IEnumerable<StationTime> ListOfStationTime { get; set; }
    }
}
