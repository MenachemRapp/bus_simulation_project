using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace targil2
{
    public class BusStopLine : BusStop
    {
        public double Distance { get; set; }
        public TimeSpan Zman { get; set; }

        public override string ToString()
        {
            String result =  base.ToString();
            result += String.Format(" Distance: {0}, Zman {1}", Distance, Zman);
            return result;
        }
    }
}
