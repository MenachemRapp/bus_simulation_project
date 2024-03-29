﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class BasicLine
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public Areas Area { get; set; }
        public int FirstStation { get; set; }
        public int LastStation { get; set; }

        public string Destination { get; set; }

      public override string ToString() => this.ToStringProperty();
    }
}
