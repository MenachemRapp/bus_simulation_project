using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace targil2
{
    class BusLineData 
    {
        private List<BusLine> buses = new List<BusLine>();

        /*
         * Input - bus line.
            The function puts it into data.
            The function can only contain up to two lines in the same number,
            provided that their first and last stops are reversed otherwise we throw an exception
         */
        public void addLineBus(BusLine bus)
        {

        }

        public void lineAtStop(int id_stop)
        {

        }

 
        public  IEnumerator GetEnumerator()
        {
            yield return buses.First();
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
