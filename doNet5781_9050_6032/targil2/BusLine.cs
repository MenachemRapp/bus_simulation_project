using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
            string keys = string.Join("\n", stations.Select(x=>x.Stop.BusStationKey));
            return String.Format("[ {0}, {1} ]\n{2}" , BusNumber, Area, keys);
        }

        //add a stop to the line
        public void add(int key, double myLatitude, double myLongitude, string myAddress, int index, TimeSpan myZman = new TimeSpan(),double myDistance=0)
        {
            
            BusStop newBusStop = new BusStop(key, myLatitude, myLongitude, myAddress);
            BusStopLine newStopLine = new BusStopLine(newBusStop, myDistance, myZman);
            
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
                stations.Add(newStopLine);
            else
                stations.Insert(index, newStopLine);
        }

        //remove a stop from the line
        public void remove(BusStop stop)
        {
            stations.RemoveAt(stations.FindIndex(x => x.Stop.BusStationKey == stop.BusStationKey));
            //
            /*foreach (BusStop station in stopCounter)
                if (station.BusStationKey == stop.BusStationKey)
                    if BusStop.counter==0
                        stopCounter.remove(BusStop);
                    else
                        BusStop.counter--;
              */
        }

        //test if the stop is on this line
        public bool findStop(BusStop val)
        {
            foreach (BusStopLine lineStop in stations)
                if (val.BusStationKey == lineStop.Stop.BusStationKey)
                    return true;
            return false;
        }

        //distnace between 2 stops
        public double stopsDistance(BusStop first, BusStop last)
        {
            BusLine sub = subroute(first, last);
            return sub.fullLineDistance();
        }

        //time between 2 stops
        public TimeSpan stopsTime(BusStop first, BusStop last)
        {
            BusLine sub = subroute(first, last);
            return sub.fullLineTime();

        }

        //time of the whole line
        private TimeSpan fullLineTime()
        {
            TimeSpan sum = TimeSpan.Zero;
            foreach (BusStopLine stop in stations)
                sum += stop.Zman;

            return sum;
        }

        //distance of the whole line
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
            foreach (BusStopLine lineStop in stations)
            {
                if (!flag)
                    if (lineStop.Stop.BusStationKey == first.BusStationKey)
                        flag = true;
                    else
                        continue;
                             
                subLine.stations.Add(lineStop);
                if (lineStop.Stop.BusStationKey == last.BusStationKey)
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
