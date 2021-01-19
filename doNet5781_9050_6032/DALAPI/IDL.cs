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
        #region Bus
        IEnumerable<DO.Bus> GetAllBuss();
        IEnumerable<DO.Bus> GetAllBussBy(Predicate<DO.Bus> predicate);
        DO.Bus GetBus(int LicenseNum);
        void AddBus(DO.Bus bus);
        void UpdateBus(DO.Bus bus);
        void UpdateBus(int LicenseNum, Action<DO.Bus> update); //method that knows to updt specific fields in Bus
        void DeleteBus(int LicenseNum);
        #endregion

        #region Station
        IEnumerable<DO.Station> GetAllStations();
        IEnumerable<DO.Station> GetAllStationsBy(Predicate<DO.Station> predicate);
        DO.Station GetStation(int Code);
        void AddStation(DO.Station station);
        void UpdateStation(DO.Station sation);
        void UpdateStation(int Code, Action<DO.Station> update); //method that knows to updt specific fields in Station
        void DeleteStation(int Code);

        #endregion

        #region AdjacentStations
        IEnumerable<DO.AdjacentStations> GetAllAdjacentStationss();
        IEnumerable<DO.AdjacentStations> GetAllAdjacentStationssBy(Predicate<DO.AdjacentStations> predicate);
        DO.AdjacentStations GetAdjacentStations(int CodeStation1, int CodeStation2);
        void AddAdjacentStations(DO.AdjacentStations Adjacent_Stations);
        void UpdateAdjacentStations(DO.AdjacentStations Adjacent_Stations);
        void UpdateAdjacentStations(int CodeStation1, int CodeStation2, Action<DO.AdjacentStations> update); //method that knows to updt specific fields in AdjacentStations
        void DeleteAdjacentStations(int CodeStation1, int CodeStation2);

        #endregion

        #region BusOnTrip
        IEnumerable<DO.BusOnTrip> GetAllBusOnTrips();
        IEnumerable<DO.BusOnTrip> GetAllBusOnTripsBy(Predicate<DO.BusOnTrip> predicate);
        DO.BusOnTrip GetBusOnTrip(int LicenseNum, int LineId, TimeSpan PlannedTakeOff);
        DO.BusOnTrip GetBusOnTrip(int Id);
        void AddBusOnTrip(DO.BusOnTrip bus_on_trip);
        void UpdateBusOnTrip(DO.BusOnTrip bus_on_trip);
        void UpdateBusOnTrip(int LicenseNum, int LineId, TimeSpan PlannedTakeOff, Action<DO.BusOnTrip> update); //method that knows to updt specific fields in BusOnTrip
        void UpdateBusOnTrip(int Id, Action<DO.BusOnTrip> update); //method that knows to updt specific fields in BusOnTrip
        void DeleteBusOnTrip(int LicenseNum, int LineId, TimeSpan PlannedTakeOff);
        void DeleteBusOnTrip(int Id);
        #endregion

        #region Line
        IEnumerable<DO.Line> GetAllLines();
        IEnumerable<DO.Line> GetAllLinesBy(Predicate<DO.Line> predicate);
        DO.Line GetLine(int Id);
        void AddLine(DO.Line line);
        void UpdateLine(DO.Line line);
        void UpdateLine(int Id, Action<DO.Line> update); //method that knows to updt specific fields in Line
        void DeleteLine(int Id);
        #endregion

        #region LineStation
        IEnumerable<DO.LineStation> GetAllLineStationBy(Predicate<DO.LineStation> predicate);


        void UpdateLineArea(int lineId, DO.Areas area);

        void DeleteLineStation(int LineId, int Station);

        void DeleteLineFromAllStations(int Id);
        #endregion



        //#region LineTrip
        //IEnumerable<DO.LineTrip> GetAllLineTrips();
        //IEnumerable<DO.LineTrip> GetAllLineTripsBy(Predicate<DO.LineTrip> predicate);
        //DO.LineTrip GetLineTrip(int LicenseNum);
        //void AddLineTrip(DO.LineTrip line_trip);
        //void UpdateLineTrip(DO.LineTrip line_trip);
        //void UpdateLineTrip(int LicenseNum, Action<DO.LineTrip> update); //method that knows to updt specific fields in LineTrip
        //void DeleteLineTrip(int LicenseNum);
        //#endregion



        IEnumerable<DO.LineStation> GeLineStationsInLine(int lineId);









    }
}
