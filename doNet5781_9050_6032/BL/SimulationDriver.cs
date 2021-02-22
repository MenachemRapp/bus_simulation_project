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
        volatile public bool isDriveRun;
        int rate;

        public List<Thread> threads = new List<Thread>();
        public void run(int station, IEnumerable<BO.TripAndStations> tripList)
        {

            
            IEnumerable<BO.TripAndStations> fullTripList = tripList;


            TimeSpan timeNow = bl.GetTime();
            int days = bl.GetDays();
            rate = bl.GetRate();
            
                IEnumerable<BO.TripAndStations> timedList = fullTripList.
               Select(tr =>
                {
                    if (tr.startTime > timeNow || tr.ListOfStationTime.Last().timeAtStop < timeNow)//no need to start from the middle of the ride
                    return
                         new BO.TripAndStations
                         {
                             TripId = tr.TripId,
                             Destination = tr.Destination,
                             LineId = tr.LineId,
                             ListOfStationTime = tr.ListOfStationTime.Take(tr.ListOfStationTime.FirstOrDefault(st => st.station == station).index),// stop trips after the selected station
                         startTime = tr.startTime
                         };
                    else //driver should start from one of the middle stations
                    return new BO.TripAndStations
                        {
                            TripId = tr.TripId,
                            Destination = tr.Destination,
                            LineId = tr.LineId,
                            ListOfStationTime = tr.ListOfStationTime.
                                                Take(tr.ListOfStationTime.FirstOrDefault(st => st.station == station).index)// stop trips after the selected station
                                                .Where(t => t.timeAtStop > timeNow).ToList(),//driver should start from one of the middle stations
                                                                                             //for using timeNow, Immediate Execution is needed
                        startTime = tr.ListOfStationTime.FirstOrDefault(t => t.timeAtStop > timeNow).timeAtStop
                        };
                }).
                Where(tr => tr.startTime > timeNow).ToList().// removes all trips which have finished completly
                                                             //for using timeNow, Immediate Execution is needed
                OrderBy(tr => tr.startTime);
            while (isDriveRun)
            {
                foreach (BO.TripAndStations trip in timedList)
                {
                    if (isDriveRun)
                    {
                        timeNow = bl.GetTime();
                        if (trip.startTime > timeNow)// need to correct end of day
                        {
                            Thread.Sleep(Convert.ToInt32((trip.startTime - timeNow).TotalMilliseconds / rate));
                        }
                        Thread driveThread = new Thread(new ParameterizedThreadStart(drive));
                        driveThread.Name = String.Format("trip Number {0}", trip.TripId);
                        threads.Add(driveThread);
                        if (isDriveRun)
                        {
                            driveThread.Start(trip);
                        }
                    }
                   


                }
                //sleep until tommorow zzzzzzz.....
                fullTripList = tripList;
                if (bl.GetDays()<=days)
                {
                    Thread.Sleep((Convert.ToInt32((TimeSpan.FromDays(1) - timeNow).TotalMilliseconds) / rate));
                }
                if (UpdatedTiming != null)
                {
                    UpdatedTiming(new BO.LineTiming { TripId = -1 }); //informs a new day has started
                }
                days++;                
            }

        }


        public void drive(object tripObj)
        {
            BO.TripAndStations trip = (BO.TripAndStations)tripObj;
            foreach (BO.StationTime station in trip.ListOfStationTime)
            {
                if (isDriveRun)
                {
                    station.timeAtStop = bl.GetTime();
                    BO.LineTiming newTiming = (new BO.LineTiming
                    {
                        TripId = trip.TripId,
                        Code = station.station,
                        Destination = trip.ListOfStationTime.Last().Name,
                        LineId = trip.LineId,
                        StartTime = trip.startTime,
                        TimeAtStop = station.timeAtStop
                    });
                  
                    if (UpdatedTiming != null)
                    {
                        UpdatedTiming(newTiming); //bus arrived at the station
                    }


                    for (int i = station.index; i < trip.ListOfStationTime.Count(); i++)  //update next stop timings
                    {
                        trip.ListOfStationTime.ElementAt(i).timeAtStop = trip.ListOfStationTime.ElementAt(i - 1).timeAtStop + trip.ListOfStationTime.ElementAt(i - 1).timeToNextStop;
                    }
                    
                    try
                    {

                        if (rand.Next(2) == 1)
                            Thread.Sleep((Convert.ToInt32(station.timeToNextStop.TotalMilliseconds * rand.NextDouble() * 2)) / rate);
                        else
                            Thread.Sleep((Convert.ToInt32(station.timeToNextStop.TotalMilliseconds - station.timeToNextStop.TotalMilliseconds * rand.NextDouble() * 0.1)) / rate);
                    }
                    catch (ThreadInterruptedException)// thread interrupted intentionally
                    {
                       
                    }

                }

            }

        }





    }
   
}
