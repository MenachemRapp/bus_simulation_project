using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLAPI;
using DO;
using DS;

namespace DL
{
    internal class DLObject:IDL
    {
        #region singelton
        static readonly DLObject instance = new DLObject();
        static DLObject() { }// static ctor to ensure instance init is done just before first usage
        DLObject() { } // default => private
        public static DLObject Instance { get => instance; }// The public Instance property to use
        #endregion

        //Implement IDL methods, CRUD
        #region Bus
        public Bus GetBus(int LicenseNum)
        {
            DO.Bus bus = DataSource.ListBus.Find(b => b.LicenseNum == LicenseNum);
            if (bus != null)
                return bus.Clone();
            else
                throw new Exception(string.Format("bad license number: {0}", LicenseNum));//create exception
        }

        public IEnumerable<Bus> GetAllBuss()
        {
            return from bus in DataSource.ListBus
                   select bus.Clone();
        }

        //not implemented
        public IEnumerable<Bus> GetAllBussBy(Predicate<Bus> predicate)
        {
            throw new NotImplementedException();
        }

        public void AddBus(Bus bus)
        {
            if (DataSource.ListBus.FirstOrDefault(b => b.LicenseNum == bus.LicenseNum) != null)
                throw new Exception(string.Format("duplicate license number: {0}", bus.LicenseNum));//create exception
            DataSource.ListBus.Add(bus.Clone());
        }
       
        public void DeleteBus(int LicenseNum)
        {
            DO.Bus bus = DataSource.ListBus.Find(b => b.LicenseNum == LicenseNum);
            if (bus != null)
            {
                DataSource.ListBus.Remove(bus);
            }
            else
                throw new Exception(string.Format("bad license number: {0}", LicenseNum));//create exception
        }
           
          

        public void UpdateBus(Bus bus)
        {
            DO.Bus myBus = DataSource.ListBus.Find(b => b.LicenseNum == bus.LicenseNum);
            if (myBus != null)
            {
                DataSource.ListBus.Remove(bus);
                DataSource.ListBus.Add(bus);
            }
            else
                throw new Exception(string.Format("bad license number: {0}", bus.LicenseNum));//create exception
        }

        //not implemented
        public void UpdateBus(int LicenseNum, Action<Bus> update)
        {
            throw new NotImplementedException();
        }


        #endregion

        #region Station
       public Station GetStation(int Code)
        {
            DO.Station station = DataSource.ListStation.Find(s => s.Code == Code);
            if (station != null)
                return station.Clone();
            else
                throw new Exception(string.Format("bad code: {0}", Code));//create exception
        }

        public IEnumerable<Station> GetAllStations()
        {
            return ((IDL)instance).GetAllStations();
        }

        public IEnumerable<Station> GetAllStationsBy(Predicate<Station> predicate)
        {
            return ((IDL)instance).GetAllStationsBy(predicate);
        }

       


        public void AddStation(Station station)
        {
            ((IDL)instance).AddStation(station);
        }

      
        public void DeleteStation(int Code)
        {
            ((IDL)instance).DeleteStation(Code);
        }

       


        public void UpdateStation(Station sation)
        {
            ((IDL)instance).UpdateStation(sation);
        }

        public void UpdateStation(int Code, Action<Station> update)
        {
            ((IDL)instance).UpdateStation(Code, update);
        }

        #endregion

        #region BusOnTrip

        public BusOnTrip GetBusOnTrip(int Id)
        {
            DO.BusOnTrip bus = DataSource.ListBusOnTrip.Find(b => b.Id == Id);
            if (bus != null)
                return bus.Clone();
            else
                throw new Exception(string.Format("bad bus ID: {0}", Id));//create exception
        }


        public IEnumerable<BusOnTrip> GetAllBusOnTrips()
        {
            return ((IDL)instance).GetAllBusOnTrips();
        }

        public IEnumerable<BusOnTrip> GetAllBusOnTripsBy(Predicate<BusOnTrip> predicate)
        {
            return from sic in DataSource.ListBusOnTrip
                   where predicate(sic)
                   select sic.Clone();
        }

        public BusOnTrip GetBusOnTrip(int LicenseNum, int LineId, TimeSpan PlannedTakeOff)
        {
            return ((IDL)instance).GetBusOnTrip(LicenseNum, LineId, PlannedTakeOff);
        }




        public void AddBusOnTrip(BusOnTrip bus_on_trip)
        {
            ((IDL)instance).AddBusOnTrip(bus_on_trip);
        }

      
        public void DeleteBusOnTrip(int LicenseNum, int LineId, TimeSpan PlannedTakeOff)
        {
            ((IDL)instance).DeleteBusOnTrip(LicenseNum, LineId, PlannedTakeOff);
        }

        public void DeleteBusOnTrip(int Id)
        {
            ((IDL)instance).DeleteBusOnTrip(Id);
        }

        

        public void UpdateBusOnTrip(BusOnTrip bus_on_trip)
        {
            ((IDL)instance).UpdateBusOnTrip(bus_on_trip);
        }

        public void UpdateBusOnTrip(int LicenseNum, int LineId, TimeSpan PlannedTakeOff, Action<BusOnTrip> update)
        {
            ((IDL)instance).UpdateBusOnTrip(LicenseNum, LineId, PlannedTakeOff, update);
        }

        public void UpdateBusOnTrip(int Id, Action<BusOnTrip> update)
        {
            ((IDL)instance).UpdateBusOnTrip(Id, update);
        }

        #endregion

        #region AdjacentStations

        public void AddAdjacentStations(AdjacentStations Adjacent_Stations)
        {
            ((IDL)Instance).AddAdjacentStations(Adjacent_Stations);
        }

        public void DeleteAdjacentStations(int CodeStation1, int CodeStation2)
        {
            ((IDL)instance).DeleteAdjacentStations(CodeStation1, CodeStation2);
        }

        

        public AdjacentStations GetAdjacentStations(int CodeStation1, int CodeStation2)
        {
            return ((IDL)instance).GetAdjacentStations(CodeStation1, CodeStation2);
        }

        public IEnumerable<AdjacentStations> GetAllAdjacentStationss()
        {
            return ((IDL)instance).GetAllAdjacentStationss();
        }

        public IEnumerable<AdjacentStations> GetAllAdjacentStationssBy(Predicate<AdjacentStations> predicate)
        {
            return ((IDL)instance).GetAllAdjacentStationssBy(predicate);
        }

       

        public void UpdateAdjacentStations(AdjacentStations Adjacent_Stations)
        {
            ((IDL)instance).UpdateAdjacentStations(Adjacent_Stations);
        }

        public void UpdateAdjacentStations(int CodeStation1, int CodeStation2, Action<AdjacentStations> update)
        {
            ((IDL)instance).UpdateAdjacentStations(CodeStation1, CodeStation2, update);
        }



        #endregion

        #region Line

        public void AddLine(Line line)
        {
            ((IDL)Instance).AddLine(line);
        }
        public void DeleteLine(int Id)
        {
            ((IDL)instance).DeleteLine(Id);
        }

        public IEnumerable<Line> GetAllLines()
        {
            return from Line in DataSource.ListLine
                   select Line.Clone();
        }

        public IEnumerable<Line> GetAllLinesBy(Predicate<Line> predicate)
        {
            return ((IDL)instance).GetAllLinesBy(predicate);
        }

       

        public Line GetLine(int code)
        {
            return DataSource.ListLine.Find(c => c.Code == code).Clone();
        }

       

        public void UpdateLine(Line line)
        {
            ((IDL)instance).UpdateLine(line);
        }

        public void UpdateLine(int Id, Action<Line> update)
        {
            ((IDL)instance).UpdateLine(Id, update);
        }

        IEnumerable<LineStation> IDL.GeLineStationsInLine(int lineId)
        {
            throw new NotImplementedException();
        }


        #endregion























    }
}
