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
        private static List<StopAndCounter> StopAndCounterList = new List<StopAndCounter>();
        public static void add(BusStop value)
        {
            StopAndCounter newStop = new StopAndCounter(value);
            StopAndCounterList.Add(newStop);
        }

        public static void remove(BusStop value)
        {
            StopAndCounter newStop = new StopAndCounter(value);
            StopAndCounterList.Remove(newStop);
        }

        public static List<StopAndCounter> get()
        {
            return StopAndCounterList;
        }
       

    }

    
}
