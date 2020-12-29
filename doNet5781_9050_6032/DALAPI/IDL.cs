using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using DO;

namespace DALAPI
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





    }
}
