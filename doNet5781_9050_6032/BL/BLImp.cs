using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        BO.BasicLine LineDoBoAdapter(DO.Line lineDO)
        {
            BO.BasicLine lineBO = new BO.BasicLine();

            int id = lineDO.Id;

            lineDO = dl.GetLine(id);

            lineDO.CopyPropertiesTo(lineBO);


            return lineBO;
        }

        public BO.BasicLine GetLine(int id)
        {
            DO.Line lineDO;
            try
            {
                lineDO = dl.GetLine(id);
            }
            catch (Exception ex)
            {
                throw new Exception("line does not exist", ex);
            }
            return LineDoBoAdapter(lineDO);

        }

        public IEnumerable<BO.BasicLine> GetAllLines()
        {
            return (from item in dl.GetAllLines()
                    select LineDoBoAdapter(item)).OrderBy(line => line.Code);
        }

        public void DeleteLine(int id)
        {
            try
            {
                dl.DeleteLine(id);
                dl.DeleteLineFromAllStations(id);
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }
        #endregion

        #region LineAndStations
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
        #endregion

        #region Station



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
        BO.Station stationDoBoAdapter(DO.Station stationDO)
        {
            BO.Station stationBO = new BO.Station();
            int code = stationDO.Code;
            stationDO = dl.GetStation(code);
            stationDO.CopyPropertiesTo(stationBO);

            return stationBO;
        }

        public BO.Station GetStation(int code)
        {
            return stationDoBoAdapter(dl.GetStation(code));
        }
        public IEnumerable<BO.Station> GetAllOtherStations(int prevCode, int NextCode)
        {
            return from station in dl.GetAllStationsBy(s => s.Code != prevCode && s.Code != NextCode)
                   select stationDoBoAdapter(station);
        }

        public IEnumerable<BO.Station> GetAllStations()
        {
            return (from station in dl.GetAllStations()
                    select stationDoBoAdapter(station)).OrderBy(station => station.Code);
        }

        public IEnumerable<BO.Station> GetAllOtherStations(int code)
        {
            return from station in dl.GetAllStationsBy(s => s.Code != code)
                   select stationDoBoAdapter(station);
        }

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

        public bool HasTimeAndDistance(int Code, NewLine line)
        {
            // if (line.ListOfStation.Count() == 0)
            //   return true;
            return GoodAdjacentStations(line.ListOfStation.ElementAt(line.ListOfStation.Count() - 1).Code, Code);
        }
        public void AddLastStation(int Code, NewLine line)
        {
            BO.AdjacentStations adjacent;
            BO.Station basicStation = GetStation(Code);
            BO.ListedLineStation station = new ListedLineStation();
            basicStation.CopyPropertiesTo(station);

            station.index = line.ListOfStation.Count() + 1;
            station.Distance = -1;
            station.Time = TimeSpan.Zero;
            station.ThereIsTimeAndDistance = false;

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
            line.ListOfStation = line.ListOfStation.Append(station);

        }
        public void DelLastStation(NewLine line)
        {
            line.ListOfStation = line.ListOfStation.Take(line.ListOfStation.Count() - 1);
            if (line.ListOfStation.Count()!=0)
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
        /// <param name="tripTime"></param>
        /// <param name="line"></param>
        public void AddTripToLine(TimeSpan tripTime, NewLine line)
        {
            line.ListOfTrips = line.ListOfTrips.Append(new ListedLineTrip() { Id = 0, StartAt = tripTime, Valid = true }).OrderBy(trip => trip.StartAt);
        }

        /// <summary>
        /// Deletes the trip from the Line.
        /// </summary>
        /// <param name="trip"></param>
        /// <param name="line"></param>
        public void DelTripFromLine(ListedLineTrip trip, NewLine line)
        {
            List<ListedLineTrip> tripLine = line.ListOfTrips.ToList();
            tripLine.Remove(tripLine.FirstOrDefault(t => t.Id == trip.Id && t.StartAt == trip.StartAt));
            line.ListOfTrips = tripLine;
        }

        public void SaveLine(NewLine line)
        {
            BO.LineTotal lineTotal = new LineTotal();
            lineTotal.ListOfStation = new List<ListedLineStation>();
            line.CopyPropertiesTo(lineTotal);
            lineTotal.ListOfStation = line.ListOfStation.ToList();
            lineTotal.ListOfTrips = line.ListOfTrips.ToList();
            lineTotal.ListOfStation.Last().Distance = 0;
            lineTotal.Id = 0;


            SaveLine(lineTotal);
        }


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
        public IEnumerable<ListedLineTrip> GetTripsInLine(int lineId)
        {
            IEnumerable<LineTrip> tripsDO = dl.GetAllLineTripsBy(trip => trip.LineId == lineId);
            IEnumerable<ListedLineTrip> tripsBO = new List<ListedLineTrip>();
            /*tripsDO.CopyPropertiesTo(tripsBO);
            foreach (var item in tripsBO)
            {
                item.Valid = true;
            }*/
            tripsBO = tripsDO.Select(t => new ListedLineTrip() { Id = t.Id, StartAt = t.StartAt, Valid = true });
            return tripsBO;
        }

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
        /// <param name="tripTime"></param>
        /// <param name="line"></param>
        public void AddTripToLine(TimeSpan tripTime, LineTotal line)
        {
            line.ListOfTrips = line.ListOfTrips.Append(new ListedLineTrip() { Id = 0, StartAt = tripTime, Valid = true }).OrderBy(trip => trip.StartAt);
        }

        /// <summary>
        /// Deletes the trip from the Line. by marking the valid property as "false"
        /// </summary>
        /// <param name="trip"></param>
        /// <param name="line"></param>
        public void DelTripFromLine(ListedLineTrip trip, LineTotal line)
        {

            List<ListedLineTrip> tripLine = line.ListOfTrips.ToList();
            tripLine.Remove(tripLine.FirstOrDefault(t => t.Id == trip.Id && t.StartAt == trip.StartAt && t.Valid==true));
            line.ListOfTrips = tripLine.Append(new ListedLineTrip() { Id = trip.Id, StartAt = trip.StartAt, Valid = false }).OrderBy(t => t.StartAt);
        }
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
        /// <param name="line">line of BO</param>
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
        /// <param name="Id">id of line, if=0 new line</param>
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
        public BO.StationWithLines GetStationWithLines(int code)
        {
            BO.StationWithLines stationWith = new StationWithLines();
            BO.Station station = stationDoBoAdapter(dl.GetStation(code));
            station.CopyPropertiesTo(stationWith);

            stationWith.ListOfLines = dl.GetAllLineStationBy(sta => sta.Station == code).Select(st => GetLine(st.LineId)).Distinct().OrderBy(line => line.Code);
            return stationWith;
        }




        #endregion

        #region Simulation Timer


        public void StartSimulator(TimeSpan startTime, int Rate, Action<TimeSpan> updateTime)
        {
            SimulationTimer simulation=  SimulationTimer.Instance;
           
            simulation.ValueChanged += (x,y) => updateTime(((ValueChangedEventArgs)y).NewValue) ;
            simulation.run(startTime, Rate);
                       
        }

        public void StopSimulator()
        {
            SimulationTimer simulation = SimulationTimer.Instance;
            simulation.stopwatch.Stop();
        }
        #endregion

        #region simulation Driver
        public void SetStationPanel(int station, Action<LineTiming> updateBus)
        {
            //IEnumerable<>
            var b = dl.GetAllLineStationBy(st => st.Station == station);
            var a = b.Select(st => dl.GetLineTrip(st.LineId)).OrderBy(tr=>tr.StartAt);
            throw new NotImplementedException();
        }
        #endregion




    }
}
