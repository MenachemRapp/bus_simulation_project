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
        
        public event EventHandler UpdatedTiming;

        IBL bl = BLFactory.GetBL("1");
        IDL dl = DLFactory.GetDL();
        #region singleton
        static readonly SimulationDriver instance = new SimulationDriver();
        static SimulationDriver() { }// static ctor to ensure instance init is done just before first usage
        SimulationDriver() { } // default => private
        public static SimulationDriver Instance { get => instance; }// The public Instance property to use
        #endregion


        static Random rand = new Random(DateTime.Now.Millisecond);
        public bool isDriveRun;
       
   

     
       
        public void run(int station)
        {
           
            IEnumerable<BO.TripAndStations> fullTripList = bl.GetTripListByStation(station);
           

            TimeSpan timeNow = bl.GetTime();
            int rate = bl.GetRate();
            fullTripList = fullTripList.Where(tr => tr.startTime > timeNow);
            foreach (BO.TripAndStations trip in fullTripList)
            {
                while (isDriveRun)
                {
                    timeNow = bl.GetTime();
                    if (trip.startTime > timeNow)// need to correct end of day
                    {
                        Thread.Sleep(Convert.ToInt32((trip.startTime - timeNow).TotalMilliseconds / rate));
                    }
                    Thread driveThread = new Thread(new ParameterizedThreadStart(drive));
                    driveThread.Start(trip);
                }
               
            }

            

        }

       
        public void drive(object tripObj)
        {
            int rate = bl.GetRate();
            BO.TripAndStations trip = (BO.TripAndStations)tripObj;
            foreach (BO.StationTime station in trip.ListOfStationTime)
            {
                while (isDriveRun)
                {
                       station.timeAtStop = bl.GetTime();
                        UpdateTimingEventArgs args = new UpdateTimingEventArgs(new BO.LineTiming
                        {
                            Code = station.station,
                            Destination = trip.ListOfStationTime.Last().station.ToString(),//bl.GetStation(trip.ListOfStationTime.Last().station).Name,
                            LineId = trip.LineId,
                            StartTime = trip.startTime,
                            TimeAtStop = station.timeAtStop
                        });
                        if (UpdatedTiming != null)
                        {
                            UpdatedTiming(this, args);
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
