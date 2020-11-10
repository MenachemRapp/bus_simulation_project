using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace targil2
{
    class BusLineData :IEnumerable<BusLine>
    {
        private List<BusLine> buses = new List<BusLine>();

        //public bool IsNull => throw new NotImplementedException();

        /*
         * Input - bus line.
            The function puts it into data.
            The function can only contain up to two lines in the same number,
            provided that their first and last stops are reversed otherwise we throw an exception
         */
        public void AddLineBus(BusLine bus)
        {
            BusLine temp = findLine(bus.BusNumber);
            if (temp != null && temp.Equals(bus) && (temp.FirstStation != bus.LastStation || temp.LastStation != bus.FirstStation))
                throw new ArgumentException(String.Format("error line bus {0} already exsit", bus.BusNumber));
            else
                buses.Add(bus);
        }

        public List<BusLine> linesAtStop(int id_stop)
        {
            List<BusLine> temp = new List<BusLine>();
            foreach (BusLine line in buses)
                if (line.findStop(id_stop))
                    temp.Add(line);
            return temp;
        }

        public void printLineAtStop(int id_stop)
        {
            List<BusLine> temp = linesAtStop(id_stop);
            foreach(BusLine bus in temp)
                Console.WriteLine(bus);
        }

        //public void linesByTimes

        public BusLine this[int index]
        {
            get 
            {
                BusLine bus = findLine(index);
                if(bus ==null)
                     throw new ArgumentException(String.Format("error line {0} no exsit",index));
                return bus;
            }
        }

        private BusLine findLine(int num_line)
        {
            foreach (BusLine bus in buses)
                if (bus.BusNumber == num_line)
                    return bus;
            return null;
        }

        public List<BusLine> sortedList()
        {
            List <BusLine> temp = new List<BusLine>(buses);
            temp.Sort();
            return temp;
        }

        IEnumerator<BusLine> IEnumerable<BusLine>.GetEnumerator()
        {
            return this.buses.GetEnumerator();
        }
    }
}
