using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DLAPI;
using DO;

namespace DL
{
    sealed class DLXML : IDL    //internal
    {
        #region singelton
        static readonly DLXML instance = new DLXML();
        static DLXML() { }// static ctor to ensure instance init is done just before first usage
        DLXML() { } // default => private
        public static DLXML Instance { get => instance; }// The public Instance property to use
        #endregion
        #region DS XML Files
        string stationsPath = @"StationXml.xml"; //XMLSerializer
        string lineStationPath= @"LineStationXml.xml"; //XMLSerializer
        string linePath = @"LineXml.xml"; //XMLSerializer

        string adjacentStationsPath = @"AdjacentStationsXml.xml"; //XElement
        #endregion

        #region Station

        public DO.Station GetStation(int code)
        {
            List<Station> ListStations = XMLTools.LoadListFromXMLSerializer<Station>(stationsPath);

            DO.Station station = ListStations.Find(s => s.Code == code);
            if (station != null)
                return station;
            else
                throw new DO.BadStationCodeException(code, $"bad station code id: {code}");
        }



        public IEnumerable<Station> GetAllStations()
        {
            List<Station> ListStations = XMLTools.LoadListFromXMLSerializer<Station>(stationsPath);
            return from station in ListStations
                   select station;
        }


        public void AddStation(DO.Station station)
        {
            List<Station> ListStations = XMLTools.LoadListFromXMLSerializer<Station>(stationsPath);

            if (ListStations.FirstOrDefault(s => s.Code == station.Code) != null)
                throw new DO.BadStationCodeException(station.Code, "Duplicate station code");

            ListStations.Add(station);
            
            XMLTools.SaveListToXMLSerializer(ListStations, stationsPath);

        }

        public void DeleteStation(int Code)
        {
            List<Station> ListStations = XMLTools.LoadListFromXMLSerializer<Station>(stationsPath);

            DO.Station station = ListStations.Find(s => s.Code == Code);

            if (station == null)
                throw new BadStationCodeException(Code, "Bad Station Code");
            
            
            
            ListStations.Remove(station);

            XMLTools.SaveListToXMLSerializer(ListStations, stationsPath);

        }


        public void UpdateStation(Station myStation)
        {

            List<Station> ListStations = XMLTools.LoadListFromXMLSerializer<Station>(stationsPath);

            DO.Station station = ListStations.Find(s => s.Code == myStation.Code);

            if (station == null)
                throw new BadStationCodeException(myStation.Code, "Bad Station Code");

            ListStations.Remove(station);
            ListStations.Add(myStation);

            XMLTools.SaveListToXMLSerializer(ListStations, stationsPath);
                     
        }
        #endregion

        #region LineStation
        public IEnumerable<LineStation> GetAllLineStationBy(Predicate<DO.LineStation> predicate)
        {
            List<LineStation> ListLineStations = XMLTools.LoadListFromXMLSerializer<LineStation>(lineStationPath);

            return from LineStation in ListLineStations
                   where predicate(LineStation)
                   select LineStation;
        }

        #endregion

        #region AdjacentStations
        public DO.AdjacentStations GetAdjacentStations(int CodeStation1, int CodeStation2)
        {
            XElement adjacentStationsRootElem = XMLTools.LoadListFromXMLElement(adjacentStationsPath);

            AdjacentStations a = (from adj in adjacentStationsRootElem.Elements()
                                    where (int.Parse(adj.Element("Station1").Value) == CodeStation1 && int.Parse(adj.Element("Station2").Value) == CodeStation2)
                                    select new AdjacentStations()
                                    {
                                        Station1 = Int32.Parse(adj.Element("Station1").Value),
                                        Station2 = Int32.Parse(adj.Element("Station2").Value),
                                        Distance = double.Parse(adj.Element("Distance").Value),
                                        Time = TimeSpan.ParseExact(adj.Element("Time").Value, "hh\\:mm\\:ss", CultureInfo.InvariantCulture)
                                    }
                                    ).FirstOrDefault();

            if (a == null)
                throw new BadAdjacentStationsException(CodeStation1, CodeStation2);

            return a;
        }

        #endregion

        #region temp


        public void AddAdjacentStations(AdjacentStations Adjacent_Stations)
        {
            throw new NotImplementedException();
        }

        public void AddBus(Bus bus)
        {
            throw new NotImplementedException();
        }

        public void AddBusOnTrip(BusOnTrip bus_on_trip)
        {
            throw new NotImplementedException();
        }

        public int AddLine(Line line)
        {
            throw new NotImplementedException();
        }

        public void AddLineStation(LineStation line_station)
        {
            throw new NotImplementedException();
        }

        

        public void DeleteAdjacentStations(int CodeStation1, int CodeStation2)
        {
            throw new NotImplementedException();
        }

        public void DeleteBus(int LicenseNum)
        {
            throw new NotImplementedException();
        }

        public void DeleteBusOnTrip(int LicenseNum, int LineId, TimeSpan PlannedTakeOff)
        {
            throw new NotImplementedException();
        }

        public void DeleteBusOnTrip(int Id)
        {
            throw new NotImplementedException();
        }

        public void DeleteLine(int Id)
        {
            throw new NotImplementedException();
        }

        public void DeleteLineFromAllStations(int Id)
        {
            throw new NotImplementedException();
        }

        public void DeleteLineStation(int LineId, int Station)
        {
            throw new NotImplementedException();
        }

        
        public IEnumerable<LineStation> GeLineStationsInLine(int lineId)
        {
            throw new NotImplementedException();
        }

        

        public IEnumerable<AdjacentStations> GetAllAdjacentStationss()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AdjacentStations> GetAllAdjacentStationssBy(Predicate<AdjacentStations> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BusOnTrip> GetAllBusOnTrips()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BusOnTrip> GetAllBusOnTripsBy(Predicate<BusOnTrip> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Bus> GetAllBuss()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Bus> GetAllBussBy(Predicate<Bus> predicate)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Line> GetAllLines()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Line> GetAllLinesBy(Predicate<Line> predicate)
        {
            throw new NotImplementedException();
        }

       

       
        public IEnumerable<Station> GetAllStationsBy(Predicate<Station> predicate)
        {
            throw new NotImplementedException();
        }

        public Bus GetBus(int LicenseNum)
        {
            throw new NotImplementedException();
        }

        public BusOnTrip GetBusOnTrip(int LicenseNum, int LineId, TimeSpan PlannedTakeOff)
        {
            throw new NotImplementedException();
        }

        public BusOnTrip GetBusOnTrip(int Id)
        {
            throw new NotImplementedException();
        }

        public Line GetLine(int Id)
        {
            throw new NotImplementedException();
        }

        

        public void UpdateAdjacentStations(AdjacentStations Adjacent_Stations)
        {
            throw new NotImplementedException();
        }

        public void UpdateAdjacentStations(int CodeStation1, int CodeStation2, Action<AdjacentStations> update)
        {
            throw new NotImplementedException();
        }

        public void UpdateBus(Bus bus)
        {
            throw new NotImplementedException();
        }

        public void UpdateBus(int LicenseNum, Action<Bus> update)
        {
            throw new NotImplementedException();
        }

        public void UpdateBusOnTrip(BusOnTrip bus_on_trip)
        {
            throw new NotImplementedException();
        }

        public void UpdateBusOnTrip(int LicenseNum, int LineId, TimeSpan PlannedTakeOff, Action<BusOnTrip> update)
        {
            throw new NotImplementedException();
        }

        public void UpdateBusOnTrip(int Id, Action<BusOnTrip> update)
        {
            throw new NotImplementedException();
        }

        public void UpdateLine(Line line)
        {
            throw new NotImplementedException();
        }

        public void UpdateLine(int Id, Action<Line> update)
        {
            throw new NotImplementedException();
        }

        public void UpdateLineArea(int lineId, Areas area)
        {
            throw new NotImplementedException();
        }

      

        public void UpdateStation(int Code, Action<Station> update)
        {
            throw new NotImplementedException();
        }
        #endregion


       
    }
}
