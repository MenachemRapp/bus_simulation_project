using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace targil2
{
    class BusLineData :IEnumerable
    {
        private List<BusLine> buses = new List<BusLine>();

        public bool IsNull => throw new NotImplementedException();

        /*
         * Input - bus line.
            The function puts it into data.
            The function can only contain up to two lines in the same number,
            provided that their first and last stops are reversed otherwise we throw an exception
         */
        public void addLineBus(BusLine bus)
        {

        }

        public List<BusLine> linesAtStop(int id_stop)
        {
            List<BusLine> temp = new List<BusLine>();
            foreach (BusLine line in buses)
                if (line.findStop(new ))
                    temp.Add(line);
        }

 
        public  IEnumerator GetEnumerator()
        {
            return this.buses.GetEnumerator();
        }

        //public void linesByTimes

        public BusLine this[int index]
        {
            get 
            {
                foreach (BusLine bus in buses)
                    if (bus.BusNumber == index)
                        return bus;
                //todo -throw exeption
                return null;
            }
        }
    }
}
