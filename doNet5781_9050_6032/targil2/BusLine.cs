using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

        public void add(int key, double myLatitude, double myLongitude, string myAddress, double myDistance, TimeSpan myZman, int index)
        {
            BusStopLine newStop = new BusStopLine { 
                BusStationKey = key, Latitude = myLatitude ,Longitude = myLongitude, Address = myAddress, Distance = myDistance, Zman = myZman 
            };
            //BusStopLine newStop = new BusStopLine(key, myLatitude, myLongitude, myAddress, myDistance, myZman);
            
            /*
            bool flag = false;
            foreach (BusStop stop in stopCounter)
                if (newStop.BusStationKey==stop.BusStationKey)
                {
                    if (newStop == stop)
                        stop.mone++;
                    else
                        throw new ArgumentException
                            (string.Format("station numbeer {0} is already exits", newStop.BusStationKey));
                    flag = true;
                    break;
                }
            if (!flag)
            {
                StopCount newCount = new StopCount(newStop);
                stopCounter.add(newCount);
            }
            */

            if (index > stations.Count + 1 || index < 0)
                throw new ArgumentException (String.Format("value bust be between 0 and {0}", stations.Count + 1));



            if (index == stations.Count + 1)
                stations.Add(newStop);
            else
                stations.Insert(index, newStop);
        }

        public void remove(BusStop stop)
        {
            stations.RemoveAt(stations.FindIndex(x => x.BusStationKey == stop.BusStationKey));
            //
            /*foreach (BusStop station in stopCounter)
                if (station.BusStationKey == stop.BusStationKey)
              */  
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
                if (!flag)
                    if (stop.BusStationKey == first.BusStationKey)
                        flag = true;
                    else
                        continue;
                             
                subLine.stations.Add(stop);
                if (stop.BusStationKey == last.BusStationKey)
                    break;
            }
                        
            return subLine;
        }
                   
        public int CompareTo(BusLine other)
        {
            return fullLineTime().CompareTo(other.fullLineTime());
        }


    }
}
