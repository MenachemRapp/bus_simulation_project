using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    class BusInDrive
    {
        public int id_run_num{ get; set; }
        public int license_num { get; set; }
        public int line_id { get; set; }
        public TimeSpan formal_time_exit { get; set; }
        public TimeSpan actual_time_exit { get; set; }
        public int last_stop_stopped_id { get; set; }
        public TimeSpan last_stop_stopped_time { get; set; }
        public TimeSpan next_stop_time { get; set; }


    }
}
