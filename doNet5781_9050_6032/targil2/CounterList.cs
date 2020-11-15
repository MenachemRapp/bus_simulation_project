using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace targil2
{
    static class CounterList
    {
        private static List<StopAndCounter> stopAndCounterList = new List<StopAndCounter>();
        public static void add(BusStop value)
        {
            bool inList = false;
            foreach (StopAndCounter station in CounterList.StopAndCounterList)
                if (value.BusStationKey == station.stop.BusStationKey)
                {
                    if (value == station.stop)
                        station.increase();
                    else
                        throw new ArgumentException
                            (string.Format("station number {0} is already exits", value.BusStationKey));
                    inList = true;
                    break;
                }

            if (!inList)
            {
                StopAndCounter newStop = new StopAndCounter(value);
                stopAndCounterList.Add(newStop);
                stopAndCounterList.Sort();
            }
                        
        }

        public static void remove(BusStop value)
        {
            
            foreach (StopAndCounter station in CounterList.StopAndCounterList)
                if (station.stop.BusStationKey == value.BusStationKey)
                {
                    if (station.Counter == 1)
                       stopAndCounterList.Remove(station);
                    

                    else
                        station.decrease();
                    break;
                }                     
        }

        public static List<StopAndCounter> StopAndCounterList
        {
            get => stopAndCounterList;
        }
       

    }

    
}
