using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    class BusExits
    {
        public int line_id { get; set; }
        public TimeSpan exit_time { get; set; }//to chack if time spane
        public int frequency { get; set; }
        public TimeSpan end_time { get; set; }

    }
}
