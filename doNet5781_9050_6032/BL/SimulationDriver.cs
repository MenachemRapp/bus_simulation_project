using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using DLAPI;
using BLAPI;

namespace BL
{
    sealed class SimulationDriver 
    {
        IBL bl = BLFactory.GetBL("1");
        IDL dl = DLFactory.GetDL();
        #region singleton
        static readonly SimulationDriver instance = new SimulationDriver();
        static SimulationDriver() { }// static ctor to ensure instance init is done just before first usage
        SimulationDriver() { } // default => private
        public static SimulationDriver Instance { get => instance; }// The public Instance property to use

        static Random rand = new Random(DateTime.Now.Millisecond);
        BO.LineTiming timing;
        IEnumerable<BO.LineTiming> timingList;
        //  DO.LineTrip trip;
        int stationCode;

        #endregion
       /* public void run(IEnumerable<DO.LineTrip> trips, int station, Action<BO.LineTiming> updateBus)
        {
            stationCode = station;
            foreach (var item in trips)
            {
                
                DO.LineTrip trip = item;
                Thread driveThread = new Thread(new ParameterizedThreadStart(drive));
                //Thread driveThread = new Thread(()=>drive(trip));
                driveThread.Start(trip);

              // Thread t = new Thread(new ParameterizedThreadStart(myMethod));
               // t.Start(myParameterObject);
            }
        }*/
       /* public void run(int station)
        {
            IEnumerable<DO.LineStation> lineStations = dl.GetAllLineStationBy(st => st.Station == station);
            IEnumerable<BO.LineTotal> lines = lineStations.Select(st => bl.GetLineNew(st.LineId));
            IEnumerable<DO.LineTrip> trips = lineStations.Select(st => dl.GetLineTrip(st.LineId)).OrderBy(tr => tr.StartAt);
            TimeSpan timeNow = bl.GetTime();
            trips = trips.Where(tr => tr.StartAt > timeNow);
        }
*/
        public void run(int station)
        {
            //could move this to Ibl
            IEnumerable<DO.LineStation> lineStations = dl.GetAllLineStationBy(st => st.Station == station);
            IEnumerable<int> tripList = new List<int>();
            foreach (DO.LineStation line in lineStations)
            {
                tripList = tripList.Concat(((dl.GetAllLineTripsBy(trip => trip.LineId == line.LineId)).Select(tr => tr.Id)));
            }
            IEnumerable<BO.TripAndStations> fullTripList = tripList.Select(tr => bl.GetTripAndStations(tr)).OrderBy(tr=> tr.startTime);
           //until here

            TimeSpan timeNow = bl.GetTime();
            int rate = bl.GetRate();
            fullTripList = fullTripList.Where(tr => tr.startTime > timeNow);
            foreach (BO.TripAndStations trip in fullTripList)
            {
                timeNow = bl.GetTime();
                if (trip.startTime>timeNow)// need to correct end of day
                {
                    Thread.Sleep(Convert.ToInt32((trip.startTime - timeNow).TotalMilliseconds/rate));
                }
                Thread driveThread = new Thread(new ParameterizedThreadStart(drive));
                driveThread.Start(trip);
            }



        }

        /*public void drive(object _trip)
        {
            DO.LineTrip trip = (DO.LineTrip)_trip; 
            DO.Station station = dl.GetStation(stationCode);
            var line = bl.GetLineAndStations(trip.LineId);
            timing = new BO.LineTiming { Code = stationCode, Destination = dl.GetStation(stationCode).Name, LineId = trip.LineId, StartTime = trip.StartAt };

        }*/

        public void drive(object tripObj)
        {
            int rate = bl.GetRate();
            BO.TripAndStations trip = (BO.TripAndStations)tripObj;
            foreach (var station in trip.ListOfStationTime)
            {
                station.timeAtStop = bl.GetTime();
                //update observable
                for (int i = station.index; i < trip.ListOfStationTime.Count(); i++)
                {
                    trip.ListOfStationTime.ElementAt(i).timeAtStop = trip.ListOfStationTime.ElementAt(i - 1).timeAtStop + trip.ListOfStationTime.ElementAt(i - 1).timeToNextStop;
                }
                if (rand.Next(1) == 1)
                    Thread.Sleep((Convert.ToInt32(station.timeToNextStop.TotalMilliseconds * rand.NextDouble() * 2))/rate);
                else
                    Thread.Sleep((Convert.ToInt32(station.timeToNextStop.TotalMilliseconds-station.timeToNextStop.TotalMilliseconds * rand.NextDouble() * 0.1))/rate);
                                
            }
            
        }

        public void drive(DO.LineTrip trip)
        {
            DO.Station station = dl.GetStation(stationCode);
            var line = bl.GetLineAndStations(trip.LineId);
            timing =new BO.LineTiming { Code =stationCode, Destination = dl.GetStation(stationCode).Name, LineId = trip.LineId, StartTime = trip.StartAt};

        }
        
    }
}
