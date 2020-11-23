using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace targil2
{
    public class BusStopLine 
    {
        public BusStop Stop { get; set; }
        public double Distance { get; set; }//distance from the last stop
        public TimeSpan Zman { get; set; } //time tthat past from the last stop

        public BusStopLine(BusStop myBusStop, double myDistance, TimeSpan myZman)
        {
            Stop = new BusStop(myBusStop);
            Distance = myDistance;
            Zman = myZman;
        }


        public override string ToString()
        {
            String result =  Stop.ToString();
            result += String.Format(", Distance: {0:f}, Time: {1}", Distance, Zman);
            return result;
        }
    }
}
