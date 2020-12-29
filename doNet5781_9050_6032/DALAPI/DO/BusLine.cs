using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
   public class BusLine
    {
        public int id { get; set; }
        public int num_line { get; set; }
        public Area area_in_country { get; set; }
        public int first_stop_id { get; set; }
        public int last_stop_id { get; set; }
    }
}
