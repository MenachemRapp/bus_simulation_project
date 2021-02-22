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
    internal class DLObject : IDL
    {
        #region singelton
        static readonly DLObject instance = new DLObject();
        static DLObject() { }// static ctor to ensure instance init is done just before first usage
        DLObject() { } // default => private
        public static DLObject Instance { get => instance; }// The public Instance property to use
        #endregion

        //Implement IDL methods, CRUD
    
        #region Station
        public Station GetStation(int Code)
        {
            DO.Station station = DataSource.ListStation.Find(s => s.Code == Code);
            if (station != null)
                return station.Clone();
            else
                throw new BadStationCodeException(Code);
        }

        public IEnumerable<Station> GetAllStations()
        {
            return from station in DataSource.ListStation
                   select station.Clone();
        }

        public IEnumerable<Station> GetAllStationsBy(Predicate<Station> predicate)
        {
            return from sta in DataSource.ListStation
                   where predicate(sta)
                   select sta.Clone();
        }




        public void AddStation(Station station)
        {
            if (DataSource.ListStation.FirstOrDefault(s => s.Code == station.Code) != null)
                throw new DO.BadStationCodeException(station.Code, "Duplicate station Code");

            DataSource.ListStation.Add(station.Clone());
        }


        public void DeleteStation(int Code)
        {
            DO.Station station = DataSource.ListStation.Find(s => s.Code == Code);
            if (station == null) 
                throw new BadStationCodeException(Code, "Bad Station Code");
            DO.LineStation lineStation = DataSource.ListLineStation.Find(s => s.Station == Code);
            if (lineStation != null)
                throw new BadStationCodeException(Code, "Station has lines");
            else
            DataSource.ListStation.Remove(station);
            
        }




        public void UpdateStation(Station station)
        {
            DO.Station myStation = DataSource.ListStation.Find(s => s.Code == station.Code);
            if (myStation != null)
            {
                DataSource.ListStation.Remove(myStation);
                DataSource.ListStation.Add(station);
            }
            else
                throw new BadStationCodeException(station.Code, "Bad Station Code");
        }

       

        #endregion
              
        #region AdjacentStations

        public void AddAdjacentStations(AdjacentStations Adjacent_Stations)
        {
            DO.AdjacentStations adjStations = DataSource.ListAdjacentStations.Find(sta => sta.Station1 == Adjacent_Stations.Station1 && sta.Station2 == Adjacent_Stations.Station2);
            if (adjStations == null)
            {
                DataSource.ListAdjacentStations.Add(Adjacent_Stations);
            }
            else
                throw new BadAdjacentStationsException(Adjacent_Stations.Station1, Adjacent_Stations.Station2, "Duplicate adjacent stastion");
        }

      
        public AdjacentStations GetAdjacentStations(int CodeStation1, int CodeStation2)
        {
            DO.AdjacentStations stations = DataSource.ListAdjacentStations.Find(sta => sta.Station1 == CodeStation1 && sta.Station2 == CodeStation2);
            if (stations != null)
                return stations.Clone();
            else
                throw new BadAdjacentStationsException(CodeStation1, CodeStation2);
        }

             

        public void UpdateAdjacentStations(AdjacentStations Adjacent_Stations)
        {
            DO.AdjacentStations adjStations = DataSource.ListAdjacentStations.Find(sta => sta.Station1==Adjacent_Stations.Station1 && sta.Station2==Adjacent_Stations.Station2);
            if (adjStations != null)
            {
                DataSource.ListAdjacentStations.Remove(adjStations);
                DataSource.ListAdjacentStations.Add(Adjacent_Stations);
            }
            else
                throw new BadAdjacentStationsException(Adjacent_Stations.Station1, Adjacent_Stations.Station2, "Adjacent stastion don't exist.");
        }

      

        #endregion

        #region Line

        public Line GetLine(int Id)
        {
            if (!DataSource.ListLine.Exists(c => c.Id == Id))
                throw new DO.BadLineIdException(Id);
            return DataSource.ListLine.Find(c => c.Id == Id).Clone();
        }

        public IEnumerable<Line> GetAllLines()
        {
            return from Line in DataSource.ListLine
                   select Line.Clone();
        }

        public int AddLine(Line line)
        {
            Line new_line = line.Clone();
            new_line.Id = DS.RunnerNumber.GetIdLine();
            DataSource.ListLine.Add(new_line);
            return new_line.Id;
        }
        public void DeleteLine(int Id)
        {
            DO.Line line = DataSource.ListLine.Find(l => l.Id == Id);

            if (line != null)
            {
                DataSource.ListLine.Remove(line);
            }
            else
                throw new BadLineIdException(Id);
        }



        public IEnumerable<Line> GetAllLinesBy(Predicate<Line> predicate)
        {
            return from line in DataSource.ListLine
                   where predicate(line)
                   select line.Clone();
        }


        public void UpdateLineArea(int lineId, Areas area)
        {
            DO.Line line = DataSource.ListLine.Find(li => li.Id == lineId);

            if (line != null)
            {
                line.Area = area;
            }
            else
                throw new DO.BadLineIdException(lineId);
        }


        public void UpdateLine(Line line)
        {
            DO.Line myLine = DataSource.ListLine.Find(l => l.Id == line.Id);
            if (myLine != null)
            {
                DataSource.ListLine.Remove(myLine);
                DataSource.ListLine.Add(line);
            }
            else
                throw new DO.BadLineIdException(line.Id);
        }

        #endregion

        #region LineStation
        public IEnumerable<LineStation> GetAllLineStationBy(Predicate<DO.LineStation> predicate)
        {
            return from sil in DataSource.ListLineStation
                   where predicate(sil)
                   select sil.Clone();
        }

       
        public void DeleteLineFromAllStations(int Id)
        {
            DataSource.ListLineStation.RemoveAll(s => s.LineId == Id);
        }

        public void AddLineStation(DO.LineStation line_station)
        {
            if(DataSource.ListLineStation.Exists(sta=> sta.LineId== line_station.LineId && sta.Station ==line_station.Station))
                throw new BadLineStationException(line_station.LineId, line_station.Station, "already exist");
            DataSource.ListLineStation.Add(line_station.Clone());

        }

        IEnumerable<LineStation> IDL.GeLineStationsInLine(int lineId)
        {
            return from sil in DataSource.ListLineStation
                   where sil.LineId == lineId//change to predicate
                   select sil.Clone();
        }



        #endregion

        #region LineTrip
       
        public IEnumerable<LineTrip> GetAllLineTripsBy(Predicate<LineTrip> predicate)
        {
            throw new NotImplementedException();
        }

        public LineTrip GetLineTrip(int LicenseNum)
        {
            throw new NotImplementedException();
        }

        public void AddLineTrip(LineTrip line_trip)
        {
            throw new NotImplementedException();
        }

        public void DeleteLineTrip(int LicenseNum)
        {
            throw new NotImplementedException();
        }

        public void DeleteLineFromAllTrips(int Id)
        {
            throw new NotImplementedException();
        }
        #endregion





















    }
}
