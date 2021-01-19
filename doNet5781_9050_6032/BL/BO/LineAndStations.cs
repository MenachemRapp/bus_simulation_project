using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
   public class LineAndStations :BasicLine
    {
        public TimeSpan totalTime { get; set; }

        public double totalDistance { get; set; }

        public IEnumerable<ListedLineStation> ListOfStation { get; set; }
        

    }
}
