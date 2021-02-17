using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class StationTime
    {
        public int station { get; set; }

        public string name { get; set; }//may not be needed

        public int index { get; set; }

        public TimeSpan timeAtStop { get; set; }

        public TimeSpan timeToNextStop { get; set; }
    }
}
