using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS
{
    public static class RunnerNumber
    {
        private static int id_bus_on_trip = 1;
        private static int id_line = 1;
        private static int id_trip = 1;

        public static int GetIdBusOnTrip(){return id_bus_on_trip++; }
        public static int GetIdLine() { return id_line++; }
        public static int GetIdTrip() { return id_trip++; }
    }
}
