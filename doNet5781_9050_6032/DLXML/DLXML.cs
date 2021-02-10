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
        string linesPath = @"LineXml.xml"; //XMLSerializer
        string lineTripPath = @"LineTripXml.xml"; //XMLSerializer

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

        public void DeleteLineFromAllStations(int Id)
        {
            List<LineStation> ListLineStations = XMLTools.LoadListFromXMLSerializer<LineStation>(lineStationPath);

            ListLineStations.RemoveAll(s => s.LineId == Id);


            XMLTools.SaveListToXMLSerializer(ListLineStations, lineStationPath);

        }

        public void AddLineStation(DO.LineStation line_station)
        {
            List<LineStation> ListLineStations = XMLTools.LoadListFromXMLSerializer<LineStation>(lineStationPath);
            if (ListLineStations.Exists(sta => sta.LineId == line_station.LineId && sta.Station == line_station.Station))
                throw new BadLineStationException();// to do.. already exikt
            ListLineStations.Add(line_station);

            XMLTools.SaveListToXMLSerializer(ListLineStations, lineStationPath);
        }


        IEnumerable<LineStation> IDL.GeLineStationsInLine(int lineId)
        {
            List<LineStation> ListLineStations = XMLTools.LoadListFromXMLSerializer<LineStation>(lineStationPath);

            return from sil in ListLineStations
                   where sil.LineId == lineId//change to predicate
                   select sil;
        }
        #endregion

        #region AdjacentStations
        public void AddAdjacentStations(DO.AdjacentStations adjacentStations)
        {
            XElement adjacentStationsRootElem = XMLTools.LoadListFromXMLElement(adjacentStationsPath);

            XElement adj1 = (from adj in adjacentStationsRootElem.Elements()
                                  where (int.Parse(adj.Element("Station1").Value) == adjacentStations.Station1
                                         && int.Parse(adj.Element("Station2").Value) == adjacentStations.Station2)
                                  select adj).FirstOrDefault();

            if (adj1 != null)
                throw new DO.BadAdjacentStationsException(adjacentStations.Station1, adjacentStations.Station2, "Duplicate adjacenct stations");

            XElement adjacentStationsElem = new XElement("AdjacentStations",
                                  new XElement("Station1", adjacentStations.Station1.ToString()),
                                  new XElement("Station2", adjacentStations.Station2.ToString()),
                                  new XElement("Distance", adjacentStations.Distance.ToString()),
                                  new XElement("Time", adjacentStations.Time.ToString()));

            adjacentStationsRootElem.Add(adjacentStationsElem);

            XMLTools.SaveListToXMLElement(adjacentStationsRootElem, adjacentStationsPath);
        }


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
        public void UpdateAdjacentStations(AdjacentStations adjacentStations)
        {

            XElement adjacentStationsRootElem = XMLTools.LoadListFromXMLElement(adjacentStationsPath);

            XElement adj = (from a in adjacentStationsRootElem.Elements()
                             where (int.Parse(a.Element("Station1").Value) == adjacentStations.Station1
                                    && int.Parse(a.Element("Station2").Value) == adjacentStations.Station2)
                             select a).FirstOrDefault();

            if (adj == null)
                throw new DO.BadAdjacentStationsException(adjacentStations.Station1, adjacentStations.Station2, "Adjacent stastion don't exist.");

            adj.Element("Distance").Value = adjacentStations.Distance.ToString();
            adj.Element("Time").Value= adjacentStations.Time.ToString();
                        
            XMLTools.SaveListToXMLElement(adjacentStationsRootElem, adjacentStationsPath);

        }


        #endregion

        #region line
        public Line GetLine(int Id)
        {
            List<Line> ListLines = XMLTools.LoadListFromXMLSerializer<Line>(linesPath);
            if (!ListLines.Exists(c => c.Id == Id))
                throw new DO.BadLineIdException(Id);
            return ListLines.Find(c => c.Id == Id);
        }


        public IEnumerable<Line> GetAllLines()
        {
            List<Line> ListLines = XMLTools.LoadListFromXMLSerializer<Line>(linesPath);
            return from line in ListLines
                   select line;
        }

        public int AddLine(Line line)
        {
            List<Line> ListLines = XMLTools.LoadListFromXMLSerializer<Line>(linesPath);
            if (ListLines.Count() != 0)
                line.Id = ListLines.Max(l=>l.Id) + 1;// didn't use runnerNumber
            else
                line.Id = 1;
            ListLines.Add(line);

            XMLTools.SaveListToXMLSerializer(ListLines, linesPath);
            return line.Id; //why to return this ??????????????
        }

        public IEnumerable<Line> GetAllLinesBy(Predicate<Line> predicate)
        {
            List<Line> ListLines = XMLTools.LoadListFromXMLSerializer<Line>(linesPath);
            return from line in ListLines
                   where predicate(line)
                   select line;
        }

        public void UpdateLineArea(int lineId, Areas area)
        {
            List<Line> ListLines = XMLTools.LoadListFromXMLSerializer<Line>(linesPath);
            DO.Line line = ListLines.Find(li => li.Id == lineId);

            if (line != null)
            {
                line.Area = area;
            }
            else
                throw new DO.BadLineIdException(lineId);

            XMLTools.SaveListToXMLSerializer(ListLines, linesPath);
        }

        public void UpdateLine(Line line)
        {
            List<Line> ListLines = XMLTools.LoadListFromXMLSerializer<Line>(linesPath);
            DO.Line myLine = ListLines.Find(l => l.Id == line.Id);
            if (myLine != null)
            {
                ListLines.Remove(myLine);
                ListLines.Add(line);
            }
            else
                throw new DO.BadLineIdException(line.Id);
            XMLTools.SaveListToXMLSerializer(ListLines, linesPath);
        }

        public void DeleteLine(int Id)
        {
            List<Line> ListLines = XMLTools.LoadListFromXMLSerializer<Line>(linesPath);
            DO.Line line = ListLines.Find(l => l.Id == Id);

            if (line != null)
            {
                ListLines.Remove(line);
            }
            else
                throw new BadLineIdException(Id);
            XMLTools.SaveListToXMLSerializer(ListLines, linesPath);
        }

        #endregion

        #region LineTrip

        public void AddLineTrip(LineTrip line_trip)
        {
            List<LineTrip> ListLineTrips = XMLTools.LoadListFromXMLSerializer<LineTrip>(lineTripPath);
            if (ListLineTrips.Count() != 0)
                line_trip.Id = ListLineTrips.Max(l => l.Id) + 1;
            else
                line_trip.Id = 1;
            ListLineTrips.Add(line_trip);

            XMLTools.SaveListToXMLSerializer(ListLineTrips, lineTripPath);
        }


        public IEnumerable<LineTrip> GetAllLineTripsBy(Predicate<LineTrip> predicate)
        {
            List<LineTrip> ListLineTrips = XMLTools.LoadListFromXMLSerializer<LineTrip>(lineTripPath);
            
            return from lineTrip in ListLineTrips
                   where predicate(lineTrip)
                   select lineTrip;
        }

        public LineTrip GetLineTrip(int tripId)
        {
            List<LineTrip> ListLineTrips = XMLTools.LoadListFromXMLSerializer<LineTrip>(lineTripPath);
            if (!ListLineTrips.Exists(t => t.Id == tripId))
                throw new DO.BadLineTripIdException(tripId);
            return ListLineTrips.Find(t => t.Id == tripId);
        }

        

        public void UpdateLineTrip(LineTrip line_trip)
        {
            List<LineTrip> ListLineTrips = XMLTools.LoadListFromXMLSerializer<LineTrip>(lineTripPath);

            DO.LineTrip trip = ListLineTrips.Find(t => t.Id == line_trip.Id);

            if (trip == null)
                throw new BadLineTripIdException(trip.Id);

            ListLineTrips.Remove(trip);
            ListLineTrips.Add(line_trip);

            XMLTools.SaveListToXMLSerializer(ListLineTrips, lineTripPath); ;
        }

       
        public void DeleteLineTrip(int tripId)
        {
            List<LineTrip> ListLineTrips = XMLTools.LoadListFromXMLSerializer<LineTrip>(lineTripPath);

            DO.LineTrip trip = ListLineTrips.Find(t => t.Id == tripId);

            if (trip == null)
                throw new BadLineTripIdException(trip.Id);

            ListLineTrips.Remove(trip);
            
            XMLTools.SaveListToXMLSerializer(ListLineTrips, lineTripPath); ;

        }
        #endregion
        
        #region temp




        public void AddBus(Bus bus)
        {
            throw new NotImplementedException();
        }

        public void AddBusOnTrip(BusOnTrip bus_on_trip)
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


        public void UpdateLine(int Id, Action<Line> update)
        {
            throw new NotImplementedException();
        }

      

        public void UpdateStation(int Code, Action<Station> update)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LineTrip> GetAllLineTrips()
        {
            throw new NotImplementedException();
        }

        

        public void UpdateLineTrip(int LicenseNum, Action<LineTrip> update)
        {
            throw new NotImplementedException();
        }

        #endregion



    }
}
