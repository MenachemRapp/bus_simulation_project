using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace targil2
{
    class Program
    {
        static void Main(string[] args)
        {
            BusLine play = new BusLine();
            BusStop first = new BusStop(1, 34, 50, "a");
            BusStop last = new BusStop(4, 34.0, 43.0, "a");
            play.add(1, 34, 50, "a", 0, TimeSpan.FromMinutes(0), 0);
            play.add(2, 34, 43, "b", 1, TimeSpan.FromMinutes(21), 1);
            play.add(3, 34, 43, "c", 2, TimeSpan.FromMinutes(21), 2);
            play.add(4, 34, 43, "d", 3, TimeSpan.FromMinutes(21), 3);
            play.remove(first);


            //Console.WriteLine(play.stopsDistance(first,last));

        }
    }
}
