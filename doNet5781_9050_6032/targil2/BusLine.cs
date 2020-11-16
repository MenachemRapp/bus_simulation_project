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
            string keys = string.Join(",", stations.Select(x=>x.Stop.BusStationKey));
            return String.Format("Line Number: {0}, Area: {1} \nList of Stations: {2}" , BusNumber, Area, keys);
        }

        //add a stop to the line
        public void add(BusStopLine newStopLine, int index, double nextDistance=0, TimeSpan nextZman= new TimeSpan() )
        {
            
           
            if (index > stations.Count || index < 0)
                throw new ArgumentException(String.Format("value bust be between 1 and {0}", stations.Count+ 1));
         
            //test if already is in the list, and adds if not
            CounterList.add(newStopLine.Stop);


            if (index == stations.Count)
                stations.Add(newStopLine);
            else
            {
                stations.ElementAt(index).Zman = nextZman;
                stations.ElementAt(index).Distance = nextDistance;
                stations.Insert(index, newStopLine);
            }
        }

        //remove a stop from the line
        public void remove(BusStop stop, double distance, TimeSpan Zman)
        {

            int index = stations.FindIndex(x => x.Stop.BusStationKey == stop.BusStationKey);
            if (index != 0 && index!= stations.Count-1)
            {
                stations.ElementAt(index + 1).Distance = stations.ElementAt(index).Distance;
                stations.ElementAt(index + 1).Zman = stations.ElementAt(index).Zman;
            }
            else if (index==0)
            {
                stations.ElementAt(index + 1).Distance = 0;
                stations.ElementAt(index + 1).Zman = TimeSpan.Zero;
            }
            stations.RemoveAt(index);

            //updates the counter list
            CounterList.remove(stop);
        }

        //test if the stop is on this line
        public bool findStop(BusStop val)
        {
            foreach (BusStopLine lineStop in stations)
                if (val.BusStationKey == lineStop.Stop.BusStationKey)
                    return true;
            return false;
        }
        public bool findStop(int val)
        {
            foreach (BusStopLine lineStop in stations)
                if (val == lineStop.Stop.BusStationKey)
                    return true;
            return false;
        }

        //distance between 2 stops
        public double stopsDistance(BusStop first, BusStop last)
        {
            BusLine sub = subroute(first, last);
            return sub.fullLineDistance();
        }

        //time between 2 stops
        public TimeSpan stopsTime(BusStop first, BusStop last)
        {
            BusLine sub = subroute(first, last);
            if(sub !=null)
                return sub.fullLineTime();
            return TimeSpan.Zero;

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


        //create a sub-route
        private BusLine subroute(BusStop first, BusStop last)
        {

            BusLine subLine= new BusLine { };
            subLine.Area = this.Area;
            subLine.BusNumber = this.BusNumber;
            bool flag = false;
            foreach (BusStopLine lineStop in stations)
            {
                if (!flag)
                {
                    if (lineStop.Stop.BusStationKey == first.BusStationKey)
                    {
                        flag = true;
                        BusStopLine subFirstStop = new BusStopLine(lineStop.Stop, 0, TimeSpan.Zero);
                        subLine.stations.Add(subFirstStop);
                    }
                    continue;
                }
                
                subLine.stations.Add(lineStop);
                if (lineStop.Stop.BusStationKey == last.BusStationKey)
                    break;
            }
            // if 2 stop don'n found return null
            if (!flag || subLine.LastStation.Stop.BusStationKey != last.BusStationKey)
                return null;
            return subLine;
        }

        public BusLine subroute(int first, int last)
        {
            return subroute(new BusStop(first, 0, 0, ""), new BusStop(last, 0, 0, ""));
        }
          
        //compare lines by there full ride time
        public int CompareTo(BusLine other)
        {
            return fullLineTime().CompareTo(other.fullLineTime());
        }


    }
}
