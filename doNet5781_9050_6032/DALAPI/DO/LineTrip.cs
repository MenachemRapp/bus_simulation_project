﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class LineTrip
    {
        public int Id { get; set; }
        public int LineId { get; set; }
        public TimeSpan StartAt { get; set; }
       
        /* bonus
        
        public TimeSpan Frequency { get; set; }
        public TimeSpan FinishAt { get; set; }
       */

    }
}
