using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BLAPI
{
    public interface IBL
    {
        IEnumerable<BO.ListedLineStation> GetStationCodeNameDistanceTimeInLine(int LineId);

        #region BasicLine
        BO.BasicLine GetLine(int id);
        IEnumerable<BO.BasicLine> GetAllLines();

        void DeleteLine(int id);

     //   void UpdateLineArea(int lineId, BO.Areas area);

        #endregion

        #region Station

        BO.Station GetStation(int Code);

        void AddStation(BO.Station station);

        void UpdateStation(BO.Station station);
        IEnumerable<BO.Station> GetAllStations();

        //IEnumerable<BO.Station> GetAllOtherStations(int prevCode, int NextCode);
       
        // IEnumerable<BO.Station> GetAllOtherStations(int code);

        void DeleteStation(int code);
        #endregion

        #region StationWithLines
        BO.StationWithLines GetStationWithLines(int code);
        #endregion

        #region AdjacentStations
        void UpdateAdjacentStations(BO.AdjacentStations adjStations);
        void AddAdjacentStations(BO.AdjacentStations adjStations);
        #endregion

        #region NewLine
        void AddTimeAndDistance(BO.AdjacentStations adj, NewLine line);
        void AddLastStation(int Code, NewLine line);
        void DelLastStation(NewLine line);

        void AddTripToLine(TimeSpan tripTime, NewLine line);
        void DelTripFromLine(ListedLineTrip trip, NewLine line);

        bool HasTimeAndDistance(int Code, NewLine line);

        void SaveLine(NewLine line);
        #endregion

        #region Line Total
        LineTotal GetLineNew(int Id=0);
        //bool  SaveLine(LineTotal line);
        //void RefreshLine(LineTotal line);
        void AddStatToLine(int station1_id, int station2_id, LineTotal line);
                
        void DelStatFromLine(int index, LineTotal line);

        void AddTripToLine(TimeSpan tripTime, LineTotal line);
        void DelTripFromLine(ListedLineTrip trip, LineTotal line);


        void SaveLine(LineTotal line);
        #endregion

        #region Trip and Stations

        /// <summary>
        /// Get a trip with the List of stations with there times
        /// </summary>
        /// <param name="tripId"></param>
        /// <returns></returns>
        BO.TripAndStations GetTripAndStations(int tripId);


        /// <summary>
        /// get all trips lists which pass through this station
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        IEnumerable<BO.TripAndStations> GetTripListByStation(int station);
      
        

        /// <summary>
        /// update a Trip List with a new timing
        /// </summary>
        /// <param name="fullTimingList"></param>
        /// <param name="newTiming"></param>
        /// <returns></returns>
        IEnumerable<BO.TripAndStations> UpdateNewTimingInList(IEnumerable<BO.TripAndStations> fullTimingList, BO.LineTiming newTiming);
       


        /// <summary>
        /// initialize trip list property "BusArived" based on current time
        /// </summary>
        /// <param name="tripList"></param>
        /// <returns></returns>
        IEnumerable<BO.TripAndStations> InitTripListFromNow(IEnumerable<BO.TripAndStations> tripList);
        #endregion

        #region SimulationTimer

        /// <summary>
        /// Start timer simulator
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="Rate"></param>
        /// <param name="updateTime"></param>
        void StartSimulator(TimeSpan startTime, int Rate, Action<TimeSpan> updateTime);

        /// <summary>
        /// Stop timer Simulator
        /// </summary>
        void StopSimulator();

        

        /// <summary>
        /// Get the time on the timer
        /// </summary>
        /// <returns></returns>
        TimeSpan GetTime();

        /// <summary>
        /// Gets the days on the timer
        /// </summary>
        /// <returns></returns>
        int GetDays();

        /// <summary>
        /// Get the timer rate
        /// </summary>
        /// <returns></returns>
        int GetRate();

        /// <summary>
        /// Add an action when the time has changed
        /// </summary>
        /// <param name="updateTime"></param>
        void AddToObserver(Action<TimeSpan> updateTime);

        /// <summary>
        /// remove an action when the time has changed 
        /// </summary>
        /// <param name="updateTime"></param>
        void RemoveFromObserver(Action<TimeSpan> updateTime);
        #endregion

        #region simulation Driver
        void SetStationPanel(int station, Action<LineTiming> updateBus);
        #endregion

        #region Line Timing
        /// <summary>
        /// Get all line timings for a station
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        IEnumerable<LineTiming> GetLineTimingsByStation(int station);

        /// <summary>
        /// Get a Line Timing list of the first trip of each line
        /// </summary>
        /// <param name="timingList"></param>
        /// <returns></returns>
        IEnumerable<LineTiming> GetFirstTimingForEachLine(IEnumerable<LineTiming> timingList);

        /// <summary>
        /// Get all line timings for a station from a list with all stations
        /// </summary>
        /// <param name="station"></param>
        /// <param name="tripAndStations"></param>
        /// <returns></returns>
        IEnumerable<LineTiming> GetLineTimingsFromFullList(int station, IEnumerable<BO.TripAndStations> tripAndStations);

        /// <summary>
        /// returns last Line timing which has past 
        /// </summary>
        /// <param name="timingList"></param>
        /// <returns></returns>
        LineTiming LastLineTiming(IEnumerable<LineTiming> timingList);

        /// <summary>
        /// updates the property TimeFromNow based on the given current time
        /// </summary>
        /// <param name="timingList"></param>
        /// <param name="curentTime"></param>
        /// <returns></returns>
        IEnumerable<LineTiming> UpdateTimeNow(IEnumerable<LineTiming> timingList, TimeSpan curentTime);
        #endregion
    }
}
