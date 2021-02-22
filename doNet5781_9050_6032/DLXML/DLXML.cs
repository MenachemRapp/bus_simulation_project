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
        
        string lineTripPath = @"LineTripXml.xml"; //XMLElement
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
                throw new BadLineStationException(line_station.LineId, line_station.Station, "already exist");
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
           
            //selects the lowest id number which isn't used
            IEnumerable<int> idList = from t in ListLines
                                      select t.Id;
            line.Id = Enumerable.Range(1, idList.Count() + 1).Except(idList).First();

            ListLines.Add(line);
        


            XMLTools.SaveListToXMLSerializer(ListLines, linesPath);
            
            //returns the line Id in order to set the line stations and trips
            return line.Id;
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
            XElement ListLineTripsRootElem= XMLTools.LoadListFromXMLElement(lineTripPath);

            //selects the lowest id number which isn't used
            IEnumerable<int> idList = from t in ListLineTripsRootElem.Elements()
                               let t1= Int32.Parse(t.Element("Id").Value)
                               select t1;
            line_trip.Id = Enumerable.Range(1, idList.Count()+1).Except(idList).First();
                        

            XElement LineTripElem = new XElement("Linetrip",
                                  new XElement("Id", line_trip.Id.ToString()),
                                  new XElement("LineId", line_trip.LineId.ToString()),
                                  new XElement("StartAt", line_trip.StartAt.ToString()));                            ;

            ListLineTripsRootElem.Add(LineTripElem);

            XMLTools.SaveListToXMLElement(ListLineTripsRootElem, lineTripPath);
        }


        public IEnumerable<LineTrip> GetAllLineTripsBy(Predicate<LineTrip> predicate)
        {
            XElement ListLineTripsRootElem = XMLTools.LoadListFromXMLElement(lineTripPath);
            return from t in ListLineTripsRootElem.Elements()
                   let t1 = new LineTrip()
                   {
                       Id = Int32.Parse(t.Element("Id").Value),
                       LineId = Int32.Parse(t.Element("LineId").Value),
                       StartAt = TimeSpan.ParseExact(t.Element("StartAt").Value, "hh\\:mm\\:ss", CultureInfo.InvariantCulture)
                   }
                   where predicate(t1)
                   select t1;
                   

            
        }

        public LineTrip GetLineTrip(int tripId)
        {
            XElement ListLineTripsRootElem = XMLTools.LoadListFromXMLElement(lineTripPath);


            LineTrip trip = (from t in ListLineTripsRootElem.Elements()
                             where int.Parse(t.Element("Id").Value) == tripId
                             select new LineTrip()
                             {
                                 Id = Int32.Parse(t.Element("Id").Value),
                                 LineId = Int32.Parse(t.Element("LineId").Value),
                                 StartAt = TimeSpan.ParseExact(t.Element("StartAt").Value, "hh\\:mm\\:ss", CultureInfo.InvariantCulture)
                             }
                            ).FirstOrDefault();

            if (trip == null)
                throw new DO.BadLineTripIdException(tripId);

            return trip;
        }        
            

       
        public void DeleteLineTrip(int tripId)
        {
            XElement ListLineTripsRootElem = XMLTools.LoadListFromXMLElement(lineTripPath);


            XElement lineTripElem = (from t in ListLineTripsRootElem.Elements()
                                     where int.Parse(t.Element("Id").Value) == tripId
                                     select t).FirstOrDefault();

            if (lineTripElem != null)
            {
                lineTripElem.Remove();

                XMLTools.SaveListToXMLElement(ListLineTripsRootElem, lineTripPath);
            }
            else
                throw new DO.BadLineTripIdException(tripId);
          
        }


        public void DeleteLineFromAllTrips(int lineId)
        {
            XElement ListLineTripsRootElem = XMLTools.LoadListFromXMLElement(lineTripPath);
            IEnumerable<XElement> allElemnts = from t in ListLineTripsRootElem.Elements()
                    where int.Parse(t.Element("LineId").Value) != lineId
                    select t;

            ListLineTripsRootElem.ReplaceAll(allElemnts);
            XMLTools.SaveListToXMLElement(ListLineTripsRootElem, lineTripPath);

        }
        #endregion




    }
}
