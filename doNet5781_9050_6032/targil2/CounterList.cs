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
            StopAndCounter newStop = new StopAndCounter(value);
            stopAndCounterList.Add(newStop);
        }

        public static void remove(BusStop value)
        {
            StopAndCounter newStop = new StopAndCounter(value);
            stopAndCounterList.Remove(newStop);
        }

        public static List<StopAndCounter> StopAndCounterList
        {
            get => stopAndCounterList;
        }
       

    }

    
}
