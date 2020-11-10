using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace targil2
{
    class StopAndCounter
    {
        public BusStop stop { set; get; }

        private int counter;

        public StopAndCounter(BusStop myBus)
        {
            stop = new BusStop(myBus);
            counter = 1;
        }
        public int Counter
        {
            get => counter;
        }

        public void increase()
        {
        counter++;
        }

        public void decrease()
        {
            if (counter <= 0)
                throw new ArgumentException("counter cannot be negative");
            counter--;
        }
    }


    
}
