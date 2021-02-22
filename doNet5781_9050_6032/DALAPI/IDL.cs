using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using DO;

namespace DLAPI
{
    public interface IDL
    {
       #region Station
        IEnumerable<DO.Station> GetAllStations();
       IEnumerable<DO.LineStation> GeLineStationsInLine(int lineId);
        DO.Station GetStation(int Code);
        void AddStation(DO.Station station);
        void UpdateStation(DO.Station sation);
        void DeleteStation(int Code);

        #endregion

        #region AdjacentStations
        DO.AdjacentStations GetAdjacentStations(int CodeStation1, int CodeStation2);
        void AddAdjacentStations(DO.AdjacentStations Adjacent_Stations);
        void UpdateAdjacentStations(DO.AdjacentStations Adjacent_Stations);
        #endregion
             
        #region Line
        IEnumerable<DO.Line> GetAllLines();
        IEnumerable<DO.Line> GetAllLinesBy(Predicate<DO.Line> predicate);
        DO.Line GetLine(int Id);
        int AddLine(DO.Line line);
        void UpdateLine(DO.Line line);
        void DeleteLine(int Id);
        #endregion

        #region LineStation
        IEnumerable<DO.LineStation> GetAllLineStationBy(Predicate<DO.LineStation> predicate);

        void AddLineStation(DO.LineStation line_station);
        void UpdateLineArea(int lineId, DO.Areas area);
              
        void DeleteLineFromAllStations(int Id);
        #endregion

        #region LineTrip
        IEnumerable<DO.LineTrip> GetAllLineTripsBy(Predicate<DO.LineTrip> predicate);
        DO.LineTrip GetLineTrip(int id);
        void AddLineTrip(DO.LineTrip line_trip);
        void DeleteLineTrip(int id);
        void DeleteLineFromAllTrips(int Id);

        #endregion



       
        








    }
}
