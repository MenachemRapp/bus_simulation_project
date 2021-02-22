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
          

            ListLine = new List<Line>
            {
                new Line
                {
                    Id= RunnerNumber.GetIdLine(),
                    Code= 123,
                    Area= Areas.Jerusalem,
                    FirstStation= 123456,
                    LastStation= 111111
           
                }
            };

            ListStation = new List<Station>
            {
                new Station
                {
                    Code= 123456,
                    Name= "first station",
                    Latitude= 31.2,
                    Longitude= 33.5
                },
                new Station
                {
                    Code= 111111,
                    Name= "another station",
                    Latitude= 32.2,
                    Longitude= 33.7
                },
                new Station
                {
                    Code= 111115,
                    Name= "2 st",
                    Latitude= 32.2,
                    Longitude= 33.7
                },
                new Station
                {
                    Code= 110011,
                    Name= "town center",
                    Latitude= 32.2,
                    Longitude= 33.7
                },
                new Station
                {
                    Code= 114811,
                    Name= "entrance",
                    Latitude= 32.2,
                    Longitude= 33.7
                },
                new Station
                {
                    Code= 111468,
                    Name= "green station",
                    Latitude= 32.2,
                    Longitude= 33.7
                },
                new Station
                {
                    Code= 489111,
                    Name= "orange station",
                    Latitude= 32.2,
                    Longitude= 33.7
                }
            };

            ListLineStation = new List<LineStation>
            {
                new LineStation
                {
                    LineId=1,
                    Station= 123456,
                    LineStationIndex=0,
                    PrevStation=0,
                    NextStation=111111
                },


                new LineStation
                {
                    LineId=1,
                    Station= 111111,
                    LineStationIndex=1,
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
