using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace targil2
{
    class BusLineData : IEnumerable<BusLine>
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

        //return list of all lines that stoping in this stop
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




        /*
         * Return line according to line number.
         * If there are 2 lines with the same number, the first station number must also be provided
         */
        public BusLine this[int index, int numFirstStop=-1]
        {
            get 
            {
                if (numFirstStop != -1)//if one line number
                {
                    foreach (BusLine bus in buses)
                        if (bus.BusNumber == index && bus.FirstStation.Stop.BusStationKey == numFirstStop)
                        {
                            return bus;
                        }
                    return null; 
                }

                BusLine tempBus = findLine(index);
                if(tempBus ==null)
                     throw new ArgumentException(String.Format("error line {0} no exsit",index));
                return tempBus;
            }
        }

        //find line by the num kine
        private BusLine findLine(int num_line)
        {
            foreach (BusLine bus in buses)
                if (bus.BusNumber == num_line)
                    return bus;
            return null;
        }

        //return list of buses by time of drive
        public BusLineData sortedList()
        {
            BusLineData temp = new BusLineData();
            temp.buses = new List<BusLine>(buses);
            temp.buses.Sort();
            return temp;
        }

        //remove the line 
        public void deleteLine(int num, int first_id)
        {
            buses.Remove(buses.Find(bus => bus.BusNumber == num && bus.FirstStation.Stop.BusStationKey == first_id));
        }
        public override string ToString()
        {
            string temp="";
            foreach (BusLine bus in buses)
                temp += String.Format("{0}\n\n", bus);
            return temp;
        }

        public IEnumerator GetEnumerator()
        {
            return this.buses.GetEnumerator();

        }

        IEnumerator<BusLine> IEnumerable<BusLine>.GetEnumerator()
        {
            return this.buses.GetEnumerator();
        }

        //retun how many lines have in the data, than num is input. retrun 0/1/2.
        public int busesInLine(int num)
        {
            int count = 0;
            foreach (BusLine bus in buses)
                if (bus.BusNumber == num)
                    count++;
            return count;
        }
    }
}
