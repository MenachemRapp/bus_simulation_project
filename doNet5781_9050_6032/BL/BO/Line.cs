﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    class Line
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public Areas Area { get; set; }

        public TimeSpan totalTime { get; set; }

        public double totalDistance { get; set; }

        public IEnumerable<ListedLineStation> ListOfStation { get; set; }
    }
}
