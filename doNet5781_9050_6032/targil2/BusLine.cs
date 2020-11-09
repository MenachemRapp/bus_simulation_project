using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace targil2
{
    public class BusLine:IComparable<BusLine>
    {
        public int BusNumber { get; set; }

        private List<BusStopLine> stations = new List<BusStopLine>();

        public List<BusStopLine> Stations
        {
            get
            {
                return stations;
            }
        }
        public Area Area { get; set; }

        public BusStopLine FirstStation { get => Stations[0]; set => Stations[0] = value; }
        public BusStopLine LastStation { get => Stations[stations.Count - 1]; set => Stations[stations.Count - 1] = value; }

        public override string ToString()
        {
            return String.Format("[ {0}, {1} ]", BusNumber, Area);
            //add list
        }

        public void add(BusStopLine newStop, int priveousStop)
        {
            //BusStopLine stop= new BusStopLine { }
            //if
            stations.Add(newStop);

            //add to the middle or end of the list
        //Todo
        }

        public void remove()
        {
            //Todo
        }

        public bool findStop(BusStop val)
        {
            foreach (BusStopLine stop in stations)
                if (val.BusStationKey == stop.BusStationKey)
                    return true;
            return false;
        }

        public double stopsDistance(BusStop first, BusStop last)
        {
            BusLine sub = subroute(first, last);
            return sub.fullLineDistance();
        }

        public TimeSpan stopsTime(BusStop first, BusStop last)
        {
            BusLine sub = subroute(first, last);
            return sub.fullLineTime();

        }

        private TimeSpan fullLineTime()
        {
            TimeSpan sum = TimeSpan.Zero;
            foreach (BusStopLine stop in stations)
                sum += stop.Zman;

            return sum;
        }

        private double fullLineDistance()
        {
            double sum = 0;
            foreach (BusStopLine stop in stations)
                sum += stop.Distance;

            return sum;
        }


        private BusLine subroute(BusStop first, BusStop last)
        {

            BusLine subLine= new BusLine { };
            bool flag = false;
            foreach (BusStopLine stop in stations)
            {
                if (flag)
                {
                    subLine.stations.Add(stop);
                    if (stop.BusStationKey == last.BusStationKey)
                        break;
                }
                else if (stop.BusStationKey == first.BusStationKey)
                    flag = true;
            }
                
            return subLine;
        }
                   
        public int CompareTo(BusLine other)
        {
            return fullLineTime().CompareTo(other.fullLineTime());
        }


    }
}
