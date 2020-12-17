using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    class BusDAO
    {
         int id { get; set; }
        DateTime LicensingDate { get; set; }
        int mileage { get; set; }
        int AmountFuel { get; set; }
        BUS_STATUS status { get; set; }

        public override string ToString()
        {
            return this.ToStringProperty();
        }

    }
}
