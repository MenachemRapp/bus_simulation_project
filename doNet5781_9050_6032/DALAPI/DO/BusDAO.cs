using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    class BusDAO
    {
        public int id { get; set; }
        public DateTime LicensingDate { get; set; }
        public int mileage { get; set; }
        public int AmountFuel { get; set; }
        public BUS_STATUS status { get; set; }

        public override string ToString()
        {
            return this.ToStringProperty();
        }

    }
}
