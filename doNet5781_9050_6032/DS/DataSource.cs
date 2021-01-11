using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace DS
{
    public static class DataSource
    {
        public static List<AdjacentStations> List;
        public static List<Bus> ListBus;
        public static List<BusOnTrip> ListBusOnTrip;
        public static List<Line> ListLine;
        public static List<LineStation> ListLineStation;
        public static List<LineTrip> ListLineTrip;
        public static List<Station> ListStation;
        public static List<AdjacentStations> ListAdjacentStations;
        public static List<Trip> ListTrip;
        public static List<User> ListUser;


        static DataSource()
        {
            InitAllLists();
        }
        static void InitAllLists()
        {
            ListBus = new List<Bus>
            {
                new Bus
                {
                    LicenseNum=12345678,
                    FromDate= DateTime.Today.AddYears(-3),
                    ToatalTrip=7500,
                    FuelRemain=350,
                    Status=BUS_STATUS.AVAILABLE


                }
            };

            ListLine = new List<Line>
            {
                new Line
                {
                    Id= 1,
                    Code= 123,
                    Area= Areas.Jerusalem,
                    FirstStation= 123456,
                    LastStation= 111111
           
                }
            };

            ListLineStation = new List<LineStation>
            {
                new LineStation
                {
                    LineId=1,
                    Station= 123456,
                    PrevStation=0,
                    NextStation=111111
                },


                new LineStation
                {
                    LineId=1,
                    Station= 111111,
                    PrevStation=12456,
                    NextStation=0
                }

            };


            ListAdjacentStations = new List<AdjacentStations>
            {
                new AdjacentStations
                {
                    Station1=123456,
                    Station2= 111111,
                    Distance= 24.6,
                    Time= TimeSpan.FromMinutes(14)
                }
            };
        }
    }
}
