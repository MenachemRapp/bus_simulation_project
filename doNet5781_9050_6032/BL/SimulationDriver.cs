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
     
        public Action<BO.LineTiming> UpdatedTiming;
     
        #region singleton
        static readonly SimulationDriver instance = new SimulationDriver();
        static SimulationDriver() { }// static ctor to ensure instance init is done just before first usage
        SimulationDriver() { } // default => private
        public static SimulationDriver Instance { get => instance; }// The public Instance property to use
        #endregion

        IBL bl = BLFactory.GetBL("1");
        IDL dl = DLFactory.GetDL();

        static Random rand = new Random(DateTime.Now.Millisecond);
        public bool isDriveRun;


        public void run(int station)
        {

            IEnumerable<BO.TripAndStations> fullTripList = bl.GetTripListByStation(station);

            
            TimeSpan timeNow = bl.GetTime();
            int rate = bl.GetRate();
            fullTripList = fullTripList.Select(tr =>
            {
                if (tr.startTime > timeNow || tr.ListOfStationTime.Last().timeAtStop < timeNow)//no need or unable to correct list of stations
                    return tr;
                else //driver should start from one of the middle stations
                    return new BO.TripAndStations
                    {
                        TripId=tr.TripId,
                        Destination = tr.Destination,
                        LineId = tr.LineId,
                        ListOfStationTime = tr.ListOfStationTime.Where(t => t.timeAtStop > timeNow),
                        startTime = tr.ListOfStationTime.FirstOrDefault(t => t.timeAtStop > timeNow).timeAtStop
                    };
            }).
            Where(tr => tr.startTime > timeNow).// removes all trips which have finished completly
            OrderBy(tr => tr.startTime);

            foreach (BO.TripAndStations trip in fullTripList)
            {
                if (isDriveRun)
                {
                    timeNow = bl.GetTime();
                    if (trip.startTime > timeNow)// need to correct end of day
                    {
                        Thread.Sleep(Convert.ToInt32((trip.startTime - timeNow).TotalMilliseconds / rate));
                    }
                    Thread driveThread = new Thread(new ParameterizedThreadStart(drive));
                    
                    if (isDriveRun)
                    {
                        driveThread.Start(trip);
                    }
                }

            }



        }


        public void drive(object tripObj)
        {
            int rate = bl.GetRate();
            BO.TripAndStations trip = (BO.TripAndStations)tripObj;
            foreach (BO.StationTime station in trip.ListOfStationTime)
            {
                if (isDriveRun)
                {
                    station.timeAtStop = bl.GetTime();
                    BO.LineTiming d = (new BO.LineTiming
                    {
                        TripId = trip.TripId,
                        Code = station.station,
                        Destination = trip.ListOfStationTime.Last().Name,
                        LineId = trip.LineId,
                        StartTime = trip.startTime,
                        TimeAtStop = station.timeAtStop
                    });
                    UpdateTimingEventArgs args = new UpdateTimingEventArgs(new BO.LineTiming
                    {
                        TripId= trip.TripId,
                        Code = station.station,
                        Destination = trip.ListOfStationTime.Last().Name,
                        LineId = trip.LineId,
                        StartTime = trip.startTime,
                        TimeAtStop = station.timeAtStop
                    });
                    if (UpdatedTiming != null)
                    {
                                              
                        UpdatedTiming(d);
                    }


                    for (int i = station.index; i < trip.ListOfStationTime.Count(); i++)
                    {
                        trip.ListOfStationTime.ElementAt(i).timeAtStop = trip.ListOfStationTime.ElementAt(i - 1).timeAtStop + trip.ListOfStationTime.ElementAt(i - 1).timeToNextStop;
                    }
                    if (rand.Next(2) == 1)
                        Thread.Sleep((Convert.ToInt32(station.timeToNextStop.TotalMilliseconds * rand.NextDouble() * 2)) / rate);
                    else
                        Thread.Sleep((Convert.ToInt32(station.timeToNextStop.TotalMilliseconds - station.timeToNextStop.TotalMilliseconds * rand.NextDouble() * 0.1)) / rate);
                }
            }

        }





    }
    public class UpdateTimingEventArgs : EventArgs
    {
        public readonly BO.LineTiming NewValue;

        public UpdateTimingEventArgs(BO.LineTiming newTemp)
        {
            NewValue = newTemp;
        }
    }
}
