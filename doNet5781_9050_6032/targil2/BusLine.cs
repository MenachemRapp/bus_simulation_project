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


        public void add()
        { }

        public void remove()
        { }

        public bool findStop(BusStop val)
        {
            return false;
        }

        public double stopsDistance(BusStop first, BusStop last)
        {
            return 0;
        }

        public TimeSpan stopsTime(BusStop first, BusStop last)
        {
            return new TimeSpan {};
        }

        public BusStopLine subroute(BusStop first, BusStop last)
        {
            return new BusStopLine { };
        }
                   
        public int CompareTo(BusLine other)
        {
            return stopsDistance(FirstStation,LastStation).CompareTo(other.stopsDistance(other.FirstStation, other.LastStation));
        }
    }
}
