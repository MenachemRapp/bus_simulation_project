using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class Bus
    {
        public int LicenseNum { get; set; }
        public DateTime FromDate { get; set; }
        public double ToatalTrip { get; set; }
        public double FuelRemain { get; set; }
        public BUS_STATUS Status { get; set; }

        public override string ToString()
        {
            return this.ToStringProperty();
        }

    }
}
