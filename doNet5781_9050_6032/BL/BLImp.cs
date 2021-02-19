using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Reflection;
using BLAPI;
using BO;
using DO;
using DLAPI;

namespace BL
{
    class BLImp : IBL
    {
        IDL dl = DLFactory.GetDL();

        #region BasicLine

        /// <summary>
        /// adapt a Do line To a BO line
        /// </summary>
        /// <param name="lineDO"></param>
        /// <returns></returns>
        BO.BasicLine LineDoBoAdapter(DO.Line lineDO)
        {
            BO.BasicLine lineBO = new BO.BasicLine();

            int id = lineDO.Id;

            lineDO = dl.GetLine(id);

            lineDO.CopyPropertiesTo(lineBO);

            lineBO.Destination = GetStation(lineBO.LastStation).Name;

            return lineBO;
        }

        /// <summary>
        /// gets a Line based on the line Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BO.BasicLine GetLine(int id)
        {
            DO.Line lineDO;
            try
            {
                lineDO = dl.GetLine(id);
            }
            catch (DO.BadLineIdException ex)
            {
                throw new BO.BadLineIdException("line does not exist", ex);
            }
            return LineDoBoAdapter(lineDO);

        }

        /// <summary>
        /// get all lines
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BO.BasicLine> GetAllLines()
        {
            return (from item in dl.GetAllLines()
                    select LineDoBoAdapter(item)).OrderBy(line => line.Code);
        }

        /// <summary>
        /// delete a line and all it's stations and trips
        /// </summary>
        /// <param name="id"></param>
        public void DeleteLine(int id)
        {
            try
            {
                dl.DeleteLine(id);
                dl.DeleteLineFromAllStations(id);
                dl.DeleteLineFromAllTrips(id);
            }
            catch (DO.BadLineIdException ex)
            {
                throw new BO.BadLineIdException("line does not exist", ex);
            }
        }
        #endregion

        #region LineAndStations
        /// <summary>
        /// get a line with a list of it's stations by the given ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public BO.LineAndStations GetLineAndStations(int Id)
        {
            BO.LineAndStations lineBO = new BO.LineAndStations();
            DO.Line lineDO;
            try
            {
                lineDO = dl.GetLine(Id);
            }
            catch (Exception ex)
            {
                throw new BO.BadLineIdException("line does not exist", ex);
            }
            lineDO.CopyPropertiesTo(lineBO);
            lineBO.ListOfStation = GetStationCodeNameDistanceTimeInLine(Id).ToList();

            lineBO.totalDistance = lineBO.ListOfStation.Sum(s => s.Distance);
            lineBO.totalTime = TimeSpan.FromTicks(lineBO.ListOfStation.Sum(s => s.Time.Ticks));

            return lineBO;

        }


        /*
        public void UpdateLineArea(int lineId, BO.Areas BOarea)
        {
            Enum.TryParse(BOarea.ToString(), out DO.Areas DOarea);
            try
            {
                dl.UpdateLineArea(lineId, DOarea);
            }
            catch (DO.BadLineIdException ex)
            {
                throw new BO.BadLineIdException("Line ID does not exist", ex);
            }
        }

        */
        #endregion

        #region Station


        /// <summary>
        /// Add a new station
        /// </summary>
        /// <param name="station"></param>
        public void AddStation(BO.Station station)
        {
            DO.Station stationDO = new DO.Station();
            station.CopyPropertiesTo(stationDO);
            try
            {
                dl.AddStation(stationDO);
            }
            catch (DO.BadStationCodeException ex)
            {

                throw new BO.BadStationCodeException(ex.Message, ex);
            }

        }


        /// <summary>
        /// Update a station
        /// </summary>
        /// <param name="station"></param>
        public void UpdateStation(BO.Station station)
        {
            DO.Station stationDO = new DO.Station();
            station.CopyPropertiesTo(stationDO);
            try
            {
                dl.UpdateStation(stationDO);
            }
            catch (DO.BadStationCodeException ex)
            {

                throw new BO.BadStationCodeException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Adapt a DO staion to a BO station
        /// </summary>
        /// <param name="stationDO"></param>
        /// <returns></returns>
        BO.Station stationDoBoAdapter(DO.Station stationDO)
        {
            BO.Station stationBO = new BO.Station();
            int code = stationDO.Code;
            stationDO = dl.GetStation(code);
            stationDO.CopyPropertiesTo(stationBO);

            return stationBO;
        }


        /// <summary>
        /// Get a station by the code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public BO.Station GetStation(int code)
        {
            return stationDoBoAdapter(dl.GetStation(code));
        }
      /*  public IEnumerable<BO.Station> GetAllOtherStations(int prevCode, int NextCode)
        {
            return from station in dl.GetAllStationsBy(s => s.Code != prevCode && s.Code != NextCode)
                   select stationDoBoAdapter(station);
        }
      */

        /// <summary>
        /// Get all Stations
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BO.Station> GetAllStations()
        {
            return (from station in dl.GetAllStations()
                    select stationDoBoAdapter(station)).OrderBy(station => station.Code);
        }
/*
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public IEnumerable<BO.Station> GetAllOtherStations(int code)
        {
            return from station in dl.GetAllStationsBy(s => s.Code != code)
                   select stationDoBoAdapter(station);
        }
*/

        /// <summary>
        /// Delete a station
        /// </summary>
        /// <param name="code"></param>
        public void DeleteStation(int code)
        {
            IEnumerable<DO.LineStation> lineStations = dl.GetAllLineStationBy(s => s.Station == code);
            if (lineStations.Count() != 0)
                throw new BO.BadStationCodeException(code, "station has lines");
            try
            {
                dl.DeleteStation(code);
            }
            catch (DO.BadStationCodeException ex)
            {

                throw new BO.BadStationCodeException(ex.Message, ex);
            }

        }
        #endregion

        #region AdjacentStations

        /// <summary>
        /// test if the adjacent station exist in the data base
        /// </summary>
        /// <param name="station1"></param>
        /// <param name="station2"></param>
        /// <returns></returns>
        bool GoodAdjacentStations(int station1, int station2)
        {
            try
            {
                dl.GetAdjacentStations(station1, station2);
                return true;
            }
            catch (DO.BadAdjacentStationsException)
            {
                return false;
            }


        }

        /// <summary>
        /// Get Adjacent Stations
        /// </summary>
        /// <param name="station1"></param>
        /// <param name="station2"></param>
        /// <returns></returns>
        public BO.AdjacentStations GetAdjacentStations(int station1, int station2)
        {
            try
            {
                BO.AdjacentStations adjBO = new BO.AdjacentStations();
                DO.AdjacentStations adjDO = dl.GetAdjacentStations(station1, station2);
                adjDO.CopyPropertiesTo(adjBO);
                return adjBO;
            }
            catch (DO.BadAdjacentStationsException ex)
            {

                throw new BO.BadAdjacentStationsException(ex.Message, ex);
            }

        }

        /// <summary>
        /// Update Adjacent Stations
        /// </summary>
        /// <param name="adjBO"></param>
        public void UpdateAdjacentStations(BO.AdjacentStations adjBO)
        {
            DO.AdjacentStations adjDO = new DO.AdjacentStations();

            adjBO.CopyPropertiesTo(adjDO);
            try
            {
                dl.UpdateAdjacentStations(adjDO);
            }
            catch (DO.BadAdjacentStationsException ex)
            {

                throw new BO.BadAdjacentStationsException(ex.Message, ex);
            }

        }

        /// <summary>
        /// Add AdjacentStations
        /// </summary>
        /// <param name="adjBO"></param>
        public void AddAdjacentStations(BO.AdjacentStations adjBO)
        {
            DO.AdjacentStations adjDO = new DO.AdjacentStations();

            adjBO.CopyPropertiesTo(adjDO);
            try
            {
                dl.AddAdjacentStations(adjDO);
            }
            catch (DO.BadAdjacentStationsException ex)
            {

                throw new BO.BadAdjacentStationsException(ex.Message, ex);
            }

        }
        #endregion

        #region newLine
        /// <summary>
        /// tests if the Time a and distance betwwen the line stations exist
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        public bool HasTimeAndDistance(int Code, NewLine line)
        {
           return GoodAdjacentStations(line.ListOfStation.ElementAt(line.ListOfStation.Count() - 1).Code, Code);
        }

        /// <summary>
        /// add a station to the end of the line
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="line"></param>
        public void AddLastStation(int Code, NewLine line)
        {
            BO.AdjacentStations adjacent;
            BO.Station basicStation = GetStation(Code);
            BO.ListedLineStation station = new ListedLineStation();
            basicStation.CopyPropertiesTo(station);

            //create a new station, and mark this station as last
            station.index = line.ListOfStation.Count() + 1;
            station.Distance = -1;
            station.Time = TimeSpan.Zero;
            station.ThereIsTimeAndDistance = false;

            //fill in the time and distance from the previous station
            if (line.ListOfStation.Count() > 0)
            {
                BO.ListedLineStation prevStation = line.ListOfStation.ElementAt(line.ListOfStation.Count() - 1);
                if (HasTimeAndDistance(Code, line))
                {
                    adjacent = GetAdjacentStations(line.ListOfStation.ElementAt(line.ListOfStation.Count() - 1).Code, Code);
                    prevStation.Distance = adjacent.Distance;
                    prevStation.Time = adjacent.Time;
                    prevStation.ThereIsTimeAndDistance = true;
                }
                else
                {
                    BO.ListedLineStation find = line.ListOfStation.FirstOrDefault(s => s.Code == prevStation.Code
                                                      && s.index < line.ListOfStation.Count()
                                                      && line.ListOfStation.ElementAt(s.index).Code == station.Code);
                    if (find != null)
                    {
                        prevStation.Distance = find.Distance;
                        prevStation.Time = find.Time;
                        prevStation.ThereIsTimeAndDistance = true;
                    }
                    else
                        prevStation.Distance = 0;
                }
            }

            //add station to line
            line.ListOfStation = line.ListOfStation.Append(station);

        }

        /// <summary>
        /// delete the last station from the line
        /// </summary>
        /// <param name="line"></param>
        public void DelLastStation(NewLine line)
        {
            line.ListOfStation = line.ListOfStation.Take(line.ListOfStation.Count() - 1);
            if (line.ListOfStation.Count() != 0)
            {
                BO.ListedLineStation lastStation = line.ListOfStation.Last();
                lastStation.Distance = -1;
                lastStation.Time = TimeSpan.Zero;
                lastStation.ThereIsTimeAndDistance = false;

            }
        }


        /// <summary>
        /// Add a new trip to the given Line
        /// </summary>
        /// <param Name="tripTime"></param>
        /// <param Name="line"></param>
        public void AddTripToLine(TimeSpan tripTime, NewLine line)
        {
            line.ListOfTrips = line.ListOfTrips.Append(new ListedLineTrip() { Id = 0, StartAt = tripTime, Valid = true }).OrderBy(trip => trip.StartAt);
        }

        /// <summary>
        /// Deletes the trip from the Line.
        /// </summary>
        /// <param Name="trip"></param>
        /// <param Name="line"></param>
        public void DelTripFromLine(ListedLineTrip trip, NewLine line)
        {
            List<ListedLineTrip> tripLine = line.ListOfTrips.ToList();
            tripLine.Remove(tripLine.FirstOrDefault(t => t.Id == trip.Id && t.StartAt == trip.StartAt));
            line.ListOfTrips = tripLine;
        }

        /// <summary>
        /// save  the new line
        /// </summary>
        /// <param name="line"></param>
        public void SaveLine(NewLine line)
        {
            //adapt line to LineTotal and save it
            BO.LineTotal lineTotal = new LineTotal();
            lineTotal.ListOfStation = new List<ListedLineStation>();
            line.CopyPropertiesTo(lineTotal);
            lineTotal.ListOfStation = line.ListOfStation.ToList();
            lineTotal.ListOfTrips = line.ListOfTrips.ToList();
            lineTotal.ListOfStation.Last().Distance = 0;
            lineTotal.Id = 0;


            SaveLine(lineTotal);
        }


        /// <summary>
        /// add new time and distance to the line
        /// </summary>
        /// <param name="adj"></param>
        /// <param name="line"></param>
        public void AddTimeAndDistance(BO.AdjacentStations adj, NewLine line)
        {

           IEnumerable<BO.ListedLineStation> stationsToUpdateList = line.ListOfStation.Where(station => station.Code == adj.Station1
                                                                                             && station.index < line.ListOfStation.Count()
                                                                                              && line.ListOfStation.ElementAt(station.index).Code == adj.Station2);

            foreach (BO.ListedLineStation station in stationsToUpdateList)
            {
                station.Distance = adj.Distance;
                station.Time = adj.Time;
                station.ThereIsTimeAndDistance = true;
            }

        }
        #endregion

        #region ListedStation
        /// <summary>
        /// Get List of stations with time and distance for the given line
        /// </summary>
        /// <param name="LineId"></param>
        /// <returns></returns>
        public IEnumerable<ListedLineStation> GetStationCodeNameDistanceTimeInLine(int LineId)
        {
            List<ListedLineStation> list = new List<ListedLineStation>();
            int i = 1;
            foreach (LineStation item in DLFactory.GetDL().GeLineStationsInLine(LineId))
            {
                if (item.NextStation != 0)
                {
                    list.Add(new ListedLineStation()
                    {
                        Code = item.Station,
                        Name = DLFactory.GetDL().GetStation(item.Station).Name,
                        Distance = DLFactory.GetDL().GetAdjacentStations(item.Station, item.NextStation).Distance,
                        Time = DLFactory.GetDL().GetAdjacentStations(item.Station, item.NextStation).Time,
                        ThereIsTimeAndDistance = true,
                        index = i

                    }); ;
                }
                else
                {
                    list.Add(new ListedLineStation()
                    {
                        Code = item.Station,
                        Name = DLFactory.GetDL().GetStation(item.Station).Name,
                        Distance = 0,
                        Time = TimeSpan.Zero,
                        index = i

                    });

                }
                i++;
            }
            return list;
        }

        #endregion

        #region ListedLineTrip
        /// <summary>
        /// Get all trips in the line
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns></returns>
        public IEnumerable<ListedLineTrip> GetTripsInLine(int lineId)
        {
            IEnumerable<LineTrip> tripsDO = dl.GetAllLineTripsBy(trip => trip.LineId == lineId);
            IEnumerable<ListedLineTrip> tripsBO = new List<ListedLineTrip>();
            
            tripsBO = tripsDO.Select(t => new ListedLineTrip() { Id = t.Id, StartAt = t.StartAt, Valid = true });
            return tripsBO;
        }

        /// <summary>
        /// Add a trip to the Line
        /// </summary>
        /// <param name="trip"></param>
        /// <param name="lineID"></param>
        public void AddTrip(ListedLineTrip trip, int lineID)
        {
            DO.LineTrip newTrip = new LineTrip()
            {
                Id = trip.Id,
                LineId = lineID,
                StartAt = trip.StartAt
            };

            dl.AddLineTrip(newTrip);

        }
        #endregion

        #region line Total
        //public LineTotal GetLineNew(int Id)
        //{

        //    IDL dl = DLFactory.GetDL();
        //    LineTotal line = new LineTotal();
        //    Line do_line = dl.GetLine(Id);
        //    line.Area = (BO.Areas) do_line.Area;
        //    line.Code = do_line.Code;
        //    line.Id = do_line.Id;




        //}
        //void AddStatToLine(int station_id, int index, LineTotal line)
        //{
        //    IDL idl = DLFactory.GetDL();
        //    TimeSpan time;
        //    double distance;
        //    DO.Station station;
        //    try
        //    {
        //        station = idl.GetStation(station_id);
        //    }
        //    catch (Exception)// if station don't exikst
        //    {

        //        throw;// to_do
        //    }
        //    if (index != 0 && line.ListOfStation.Count()>0)// if There is a station before our station
        //    {

        //        try// If time and distance already exist 
        //        {
        //            DO.AdjacentStations adjacent_stations = idl.GetAdjacentStations(line.ListOfStation.First().Code, station_id);
        //            time = adjacent_stations.Time;
        //            distance = adjacent_stations.Distance;
        //        }
        //        catch (DO.BadAdjacentStationsException)
        //        {
        //            time = TimeSpan.Zero;
        //            distance = 0;
        //        }
        //    }
        //    line.ListOfStation.ToList().Insert
        //}


        /// <summary>
        /// Add a station to the line
        /// </summary>
        /// <param name="station_id"></param>
        /// <param name="index"></param>
        /// <param name="line"></param>
        public void AddStatToLine(int station_id, int index, LineTotal line)
        {
            IDL idl = DLFactory.GetDL();
            DO.Station station;
            try
            {
                station = idl.GetStation(station_id);
            }
            catch (Exception)// if station don't exikst
            {

                throw;// to_do
            }


            List<ListedLineStation> list = line.ListOfStation.ToList();
            list.Insert(index, new ListedLineStation
            {
                Code = station.Code,
                Name = station.Name,
                Distance = 0,
                Time = TimeSpan.Zero
            });
            line.ListOfStation = list;
            update_time_and_distance_of_station(line, index);
            update_time_and_distance_of_station(line, index - 1);
            int i = 1;
            foreach (var item in line.ListOfStation)
                item.index = i++;
        }

        /// <summary>
        /// Add a new trip to the given Line
        /// </summary>
        /// <param Name="tripTime"></param>
        /// <param Name="line"></param>
        public void AddTripToLine(TimeSpan tripTime, LineTotal line)
        {
            line.ListOfTrips = line.ListOfTrips.Append(new ListedLineTrip() { Id = 0, StartAt = tripTime, Valid = true }).OrderBy(trip => trip.StartAt);
        }

        /// <summary>
        /// Deletes the trip from the Line. by marking the valid property as "false"
        /// </summary>
        /// <param Name="trip"></param>
        /// <param Name="line"></param>
        public void DelTripFromLine(ListedLineTrip trip, LineTotal line)
        {

            List<ListedLineTrip> tripLine = line.ListOfTrips.ToList();
            tripLine.Remove(tripLine.FirstOrDefault(t => t.Id == trip.Id && t.StartAt == trip.StartAt && t.Valid == true));
            line.ListOfTrips = tripLine.Append(new ListedLineTrip() { Id = trip.Id, StartAt = trip.StartAt, Valid = false }).OrderBy(t => t.StartAt);
        }

        /// <summary>
        /// Removes a station from the line
        /// </summary>
        /// <param name="index"></param>
        /// <param name="line"></param>
        public void DelStatFromLine(int index, LineTotal line)
        {
            List<ListedLineStation> list = line.ListOfStation.ToList();
            list.RemoveAt(index);
            line.ListOfStation = list;
            update_time_and_distance_of_station(line, index - 1);
            int i = 1;
            foreach (var item in line.ListOfStation)
                item.index = i++;
        }

        /// <summary>
        /// Fill in time and distance. If the dont exist mark as not valid
        /// </summary>
        /// <param name="line"></param>
        /// <param name="index"></param>
        void update_time_and_distance_of_station(LineTotal line, int index)
        {
            IDL idl = DLFactory.GetDL();
            if (index >= 0 && line.ListOfStation.Count() - 2 >= index)
            {
                try
                {
                    DO.AdjacentStations adjacent_stations = idl.GetAdjacentStations(line.ListOfStation.ElementAt(index).Code, line.ListOfStation.ElementAt(index + 1).Code);
                    line.ListOfStation.ElementAt(index).Distance = adjacent_stations.Distance;
                    line.ListOfStation.ElementAt(index).Time = adjacent_stations.Time;
                    line.ListOfStation.ElementAt(index).ThereIsTimeAndDistance = true;
                }
                catch (DO.BadAdjacentStationsException)
                {
                    line.ListOfStation.ElementAt(index).Distance = 0;
                    line.ListOfStation.ElementAt(index).Time = TimeSpan.Zero;
                    line.ListOfStation.ElementAt(index).ThereIsTimeAndDistance = false;
                }
            }
        }

        /// <summary>
        /// Test if line can be saved
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        bool line_can_save(LineTotal line)
        {
            var a = from item in line.ListOfStation
                    where item.Distance == 0 && item.Time == TimeSpan.Zero
                    select item;
            if (a.FirstOrDefault() == line.ListOfStation.LastOrDefault() && line.ListOfStation.Count() >= 2)
            {
                return true; // to do -עוד בדיקות של כל הקלט
            }
            return false;
        }
        /// <summary>
        /// take a line of BO and save it in idl
        /// </summary>
        /// <param Name="line">line of BO</param>
        public void SaveLine(LineTotal line)
        {
            if (!line_can_save(line))
                throw new BO.BadSaveLineException();
            IDL idl = DLFactory.GetDL();
            int line_id = line.Id;

            //add do.line
            if (line.Id == 0)
                line_id = idl.AddLine(new DO.Line { Area = (DO.Areas)line.Area, Code = line.Code, Id = 0, FirstStation = line.ListOfStation.First().Code, LastStation = line.ListOfStation.Last().Code });
            else
                idl.UpdateLine(new DO.Line { Area = (DO.Areas)line.Area, Code = line.Code, Id = line_id, FirstStation = line.ListOfStation.First().Code, LastStation = line.ListOfStation.Last().Code });

            IEnumerator<ListedLineStation> iter = line.ListOfStation.GetEnumerator();
            iter.MoveNext();
            ListedLineStation first = iter.Current;
            int IDPrev = 0, index = 1;
            if (line.Id != 0)// if line don't new
                idl.DeleteLineFromAllStations(line.Id);

            // add all line-station without last, and add all AdjacentStations
            while (iter.MoveNext())
            {
                idl.AddLineStation(new DO.LineStation { LineId = line_id, PrevStation = IDPrev, NextStation = iter.Current.Code, LineStationIndex = index, Station = first.Code });
                try
                {
                    idl.AddAdjacentStations(new DO.AdjacentStations { Station1 = first.Code, Station2 = iter.Current.Code, Time = first.Time, Distance = first.Distance });
                }
                catch (DO.BadAdjacentStationsException)
                {

                    idl.UpdateAdjacentStations(new DO.AdjacentStations { Station1 = first.Code, Station2 = iter.Current.Code, Time = first.Time, Distance = first.Distance });
                }
                IDPrev = first.Code;
                first = iter.Current;
                index++;
            }


            //save the last line- station
            idl.AddLineStation(new DO.LineStation { LineId = line_id, PrevStation = IDPrev, NextStation = 0, LineStationIndex = index, Station = first.Code });

            foreach (ListedLineTrip trip in line.ListOfTrips)
            {
                try
                {
                    if (!trip.Valid && trip.Id != 0)
                        dl.DeleteLineTrip(trip.Id);
                    else if (trip.Valid && trip.Id == 0)
                        AddTrip(trip, line_id);
                }
                catch (DO.BadLineTripIdException)
                {
                    //trips have been modified before while working but the changes have been saved
                    //in any case, there is no reason to stop the saving
                }

            }

        }
        /// <summary>
        /// take id and build the bo of line
        /// </summary>
        /// <param Name="Id">id of line, if=0 new line</param>
        /// <returns>bo line</returns>
        public LineTotal GetLineNew(int Id = 0)
        {
            if (Id == 0)
                return new LineTotal() { Id = 0, totalDistance = 0, totalTime = TimeSpan.Zero, ListOfStation = new List<ListedLineStation>() };
            IDL idl = DLFactory.GetDL();
            DO.Line line;
            try
            {
                line = idl.GetLine(Id);
            }
            catch (DO.BadLineIdException)/// 
            {

                throw; // to do
            }
            LineTotal new_line = new LineTotal { Area = (BO.Areas)line.Area, Id = line.Id, Code = line.Code };
            new_line.ListOfStation = GetStationCodeNameDistanceTimeInLine(Id);
            new_line.totalDistance = new_line.ListOfStation.Sum(s => s.Distance);
            new_line.totalTime = TimeSpan.FromTicks(new_line.ListOfStation.Sum(s => s.Time.Ticks));

            new_line.ListOfTrips = GetTripsInLine(Id).OrderBy(trip => trip.StartAt);

            return new_line;
        }

        #endregion

        #region StationWithLines
        /// <summary>
        /// Get a station with the list of Lines which go through it
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public BO.StationWithLines GetStationWithLines(int code)
        {
            BO.StationWithLines stationWith = new StationWithLines();
            BO.Station station = stationDoBoAdapter(dl.GetStation(code));
            station.CopyPropertiesTo(stationWith);

            stationWith.ListOfLines = dl.GetAllLineStationBy(sta => sta.Station == code).Select(st => GetLine(st.LineId)).Distinct().OrderBy(line => line.Code);
            return stationWith;
        }

        #endregion

        #region Trip and Stations
        /// <summary>
        /// Get a trip with the List of stations with there times
        /// </summary>
        /// <param name="tripId"></param>
        /// <returns></returns>
        public BO.TripAndStations GetTripAndStations(int tripId)
        {
            DO.LineTrip tripDO = dl.GetLineTrip(tripId);
            BO.TripAndStations tripAndStations = new TripAndStations { LineId = tripDO.LineId, startTime = tripDO.StartAt, TripId=tripDO.Id};
            IEnumerable<ListedLineStation> lineTotalStations = GetLineNew(tripDO.LineId).ListOfStation;
            tripAndStations.ListOfStationTime = lineTotalStations.Select(st =>
            {
                if (st.index == 1)
                {
                    return new StationTime { station = st.Code, index = st.index, timeToNextStop = st.Time, timeAtStop = tripDO.StartAt, Name = st.Name };
                }
                else
                {
                    return new StationTime
                    {
                        station = st.Code,
                        Name = st.Name,
                        index = st.index,
                        timeToNextStop = st.Time,
                        timeAtStop = tripAndStations.ListOfStationTime.ElementAt(st.index - 2).timeAtStop + tripAndStations.ListOfStationTime.ElementAt(st.index - 2).timeToNextStop
                    };
                }

            });
            tripAndStations.Destination = tripAndStations.ListOfStationTime.Last().Name;
            return tripAndStations;
        }

        /// <summary>
        /// get all trips lists which pass through this station
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        public IEnumerable<BO.TripAndStations> GetTripListByStation(int station)
        {
            IEnumerable<DO.LineStation> lineStations = dl.GetAllLineStationBy(st => st.Station == station);
            IEnumerable<int> tripList = new List<int>();
            foreach (DO.LineStation line in lineStations)
            {
                tripList = tripList.Concat(((dl.GetAllLineTripsBy(trip => trip.LineId == line.LineId)).Select(tr => tr.Id)));
            }
            IEnumerable<BO.TripAndStations> fullTripList = tripList.Select(tr => GetTripAndStations(tr)).OrderBy(tr => tr.startTime);

            return fullTripList;
        }


        /// <summary>
        /// update a Tip List with a new timing
        /// </summary>
        /// <param name="fullTimingList"></param>
        /// <param name="newTiming"></param>
        /// <returns></returns>
        public IEnumerable<BO.TripAndStations> UpdateNewTimingInList(IEnumerable<BO.TripAndStations> fullTimingList, BO.LineTiming newTiming)
        {
            BO.TripAndStations tripList = fullTimingList.ToList().Find(trip => trip.TripId==newTiming.TripId);
            BO.StationTime stationTime = tripList.ListOfStationTime.First(st => st.station == newTiming.Code);
            tripList.ListOfStationTime = tripList.ListOfStationTime.Select(t =>
            {
                if (t.index < stationTime.index)
                {
                    return t;
                }
                else if (t.index == stationTime.index)
                    return new BO.StationTime { index = t.index, station = t.station, timeAtStop = newTiming.TimeAtStop, timeToNextStop = t.timeToNextStop, Name = t.Name };
                else
                    return new BO.StationTime { index = t.index, station = t.station, timeAtStop = t.timeAtStop + newTiming.TimeAtStop - stationTime.timeAtStop, timeToNextStop = t.timeToNextStop, Name = t.Name };
            });

            fullTimingList = fullTimingList.Where(trip => trip.LineId != newTiming.LineId || trip.startTime != newTiming.StartTime).Append(tripList).OrderBy(t => t.startTime);
            return fullTimingList;
        }

        #endregion

        #region Line Timing
        /// <summary>
        /// Get all line timings for a station
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        public IEnumerable<LineTiming> GetLineTimingsByStation(int station)
        {
            return GetTripListByStation(station).Select(trip => new LineTiming
            {
                TripId = trip.TripId,
                LineId = trip.LineId,
                Code = dl.GetLine(trip.LineId).Code,
                Destination = trip.ListOfStationTime.Last().Name,
                StartTime = trip.ListOfStationTime.First().timeAtStop,
                TimeAtStop = trip.ListOfStationTime.First(t => t.station == station).timeAtStop
            });
        }

        /// <summary>
        /// Get all line timings for a station from a list with all stations
        /// </summary>
        /// <param name="station"></param>
        /// <param name="tripAndStations"></param>
        /// <returns></returns>
        public IEnumerable<LineTiming> GetLineTimingsFromFullList(int station, IEnumerable<BO.TripAndStations> tripAndStations)
        {
            return tripAndStations.Select(trip => new LineTiming
            {
                TripId = trip.TripId,
                LineId = trip.LineId,
                Code = dl.GetLine(trip.LineId).Code,
                Destination = trip.ListOfStationTime.Last().Name,
                StartTime = trip.ListOfStationTime.First().timeAtStop,
                TimeAtStop = TimeSpan.FromSeconds(Math.Round(trip.ListOfStationTime.First(t => t.station == station).timeAtStop.TotalSeconds))
            });
        }

        /// <summary>
        /// Get a Line Timing list of the first trip of each line
        /// </summary>
        /// <param name="timingList"></param>
        /// <returns></returns>
        public IEnumerable<LineTiming> GetFirstTimingForEachLine(IEnumerable<LineTiming> timingList)
        {
            TimeSpan timeNow = GetTime();
            return timingList.
                Where(t => t.TimeAtStop > timeNow).
                GroupBy(t => t.LineId).
                Select(group => group.FirstOrDefault(t => t.TimeAtStop == group.Min(tr => tr.TimeAtStop))).
                OrderBy(t => t.StartTime);
        }

        /// <summary>
        /// returns last Line timing which has past 
        /// </summary>
        /// <param name="timingList"></param>
        /// <returns></returns>
        public LineTiming LastLineTiming(IEnumerable<LineTiming> timingList)
        {
            TimeSpan timeNow = GetTime();
            if (timingList.Where(t => t.TimeAtStop < timeNow).Count() > 0)
            {
                return timingList.
                Where(t => t.TimeAtStop < timeNow).
                OrderByDescending(t => t.TimeAtStop).
                First();

            }
            else if (timingList.Count() > 0)
            {
                return timingList.
                OrderByDescending(t => t.TimeAtStop).
                First();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// updates the property TimeFromNow based on the given current time
        /// </summary>
        /// <param name="timingList"></param>
        /// <param name="curentTime"></param>
        /// <returns></returns>
        public IEnumerable<LineTiming> UpdateTimeNow(IEnumerable<LineTiming> timingList, TimeSpan curentTime)
        {
            return timingList.Select(lt => new LineTiming
            {
                Code = lt.Code,
                Destination = lt.Destination,
                LineId = lt.LineId,
                StartTime = lt.StartTime,
                TimeAtStop = lt.TimeAtStop,
                TripId = lt.TripId,
                TimeFromNow = TimeSpan.FromSeconds(Math.Round((curentTime -lt.TimeAtStop).TotalSeconds))
            });

        }
        #endregion

        #region Simulation Timer
        /// <summary>
        /// Get the time on the timer
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetTime()
        {
            SimulationTimer simulation = SimulationTimer.Instance;
            if (!simulation.stopwatch.IsRunning)
                return TimeSpan.Zero;
            else
                return simulation.SimulationTime;
        }

        /// <summary>
        /// Get the timer rate
        /// </summary>
        /// <returns></returns>
        public int GetRate()
        {
            SimulationTimer simulation = SimulationTimer.Instance;
            if (!simulation.stopwatch.IsRunning)
            {
                throw new NotImplementedException();//to create
            }

            return simulation.TimerRate;
        }

        /// <summary>
        /// Start timer simulator
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="Rate"></param>
        /// <param name="updateTime"></param>
        public void StartSimulator(TimeSpan startTime, int Rate, Action<TimeSpan> updateTime)
        {
            SimulationTimer simulation = SimulationTimer.Instance;

            simulation.ValueChanged += (x, y) => updateTime(((ValueChangedEventArgs)y).NewValue);
            simulation.run(startTime, Rate);

        }

        /// <summary>
        /// Stop timer Simulator
        /// </summary>
        public void StopSimulator()
        {
            SimulationTimer simulation = SimulationTimer.Instance;
            simulation.stopwatch.Stop();
        }
        #endregion

        #region simulation Driver
        /// <summary>
        /// start driver and set station panel
        /// </summary>
        /// <param name="station"></param>
        /// <param name="updateBus"></param>
        public void SetStationPanel(int station, Action<BO.LineTiming> updateBus)
        {
            SimulationDriver driver = SimulationDriver.Instance;
            if (station == -1)
            {
                driver.UpdatedTiming -= updateBus;
                
                driver.isDriveRun = false;
                foreach (Thread thread in driver.threads)
                {
                    thread.Interrupt();
                }              
                
            }
            else
            {
                driver.UpdatedTiming += updateBus;

                driver.isDriveRun = true;
                IEnumerable<BO.TripAndStations> fullTripList = GetTripListByStation(station);
                //driver.run(station);
                driver.run(station, fullTripList);
            }
        }
        
        #endregion






    }
}
