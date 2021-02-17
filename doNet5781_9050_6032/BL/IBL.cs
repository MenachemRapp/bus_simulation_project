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

        void UpdateLineArea(int lineId, BO.Areas area);

        #endregion

        #region LineAndStations
        BO.LineAndStations GetLineAndStations(int id);

        #endregion

        #region Station

        BO.Station GetStation(int Code);

        void AddStation(BO.Station station);

        void UpdateStation(BO.Station station);
        IEnumerable<BO.Station> GetAllStations();

        IEnumerable<BO.Station> GetAllOtherStations(int prevCode, int NextCode);
        IEnumerable<BO.Station> GetAllOtherStations(int code);

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
        BO.TripAndStations GetTripAndStations(int tripId);
        IEnumerable<BO.TripAndStations> GetTripListByStation(int station);
        IEnumerable<LineTiming> GetLineTimingsFromFullList(int station, IEnumerable<BO.TripAndStations> tripAndStations);
        IEnumerable<BO.TripAndStations> UpdateNewTimingInList(IEnumerable<BO.TripAndStations> fullTimingList, BO.LineTiming newTiming);
        #endregion

        #region SimulationTimer
        void StartSimulator(TimeSpan startTime, int Rate, Action<TimeSpan> updateTime);
        void StopSimulator();

        TimeSpan GetTime();

        int GetRate();
        #endregion

        #region simulation Driver
        void SetStationPanel(int station, Action<LineTiming> updateBus);
        #endregion

        #region Line Timing
        IEnumerable<LineTiming> GetLineTimingsByStation(int station);
        #endregion
    }
}
