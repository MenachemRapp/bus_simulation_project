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
            return from item in dl.GetAllLines()
                   select LineDoBoAdapter(item);
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
                throw new Exception("no such line");
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
                throw new Exception("line does not exist", ex);
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
            return from station in dl.GetAllStationsBy(s=> s.Code!=prevCode && s.Code != NextCode)
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

        public void GetAdjacentStations(int station1, int station2)
        {
            try
            {
                BO.AdjacentStations adjBO = new BO.AdjacentStations();
                DO.AdjacentStations adjDO = dl.GetAdjacentStations(station1, station2);
                adjDO.CopyPropertiesTo(adjBO);
            }
            catch (DO.BadAdjacentStationsException ex)
            {

                throw new BO.BadAdjacentStationsException(ex.Message, ex);
            }

        }

        public void UpdateAdjacentStations(BO.AdjacentStations adjBO)
        {
            DO.AdjacentStations adjDO= new DO.AdjacentStations();

            adjBO.CopyPropertiesTo(adjDO);
            try
            {
                dl.UpdateAdjacentStations(adjDO);
            }
            catch (DO.BadAdjacentStationsException ex)
            {

               throw new BO.BadAdjacentStationsException(ex.Message ,ex);
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

        public IEnumerable<ListedLineStation> GetStationCodeNameDistanceTimeInLine(int LineId)
        {
            List<ListedLineStation> list = new List<ListedLineStation>();
            foreach (LineStation item in DLFactory.GetDL().GeLineStationsInLine(LineId))
            {
                if (item.NextStation != 0)
                {
                    list.Add(new ListedLineStation()
                    {
                        Code = item.Station,
                        Name = DLFactory.GetDL().GetStation(item.Station).Name,
                        Distance = DLFactory.GetDL().GetAdjacentStations(item.Station, item.NextStation).Distance,
                        Time = DLFactory.GetDL().GetAdjacentStations(item.Station, item.NextStation).Time
                    });
                }
                else
                {
                    list.Add(new ListedLineStation()
                    {
                        Code = item.Station,
                        Name = DLFactory.GetDL().GetStation(item.Station).Name,
                        Distance = 0,
                        Time = TimeSpan.Zero

                    });

                }
            }
            return list;
        }
        
        #region line
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
            update_time_and_distance_of_station(line, index-1);
        }
        public void DelStatFromLine(int index, LineTotal line)
        {
            List<ListedLineStation> list = line.ListOfStation.ToList();
            list.RemoveAt(index);
            line.ListOfStation = list;
            update_time_and_distance_of_station(line, index - 1);

        }
        void update_time_and_distance_of_station(LineTotal line, int index)
        {
            IDL idl = DLFactory.GetDL();
            if (index>=0 &&line.ListOfStation.Count()-2 >= index)
            {
                try
                {
                    DO.AdjacentStations adjacent_stations = idl.GetAdjacentStations(line.ListOfStation.ElementAt(index).Code, line.ListOfStation.ElementAt(index+1).Code);
                    line.ListOfStation.ElementAt(index).Distance= adjacent_stations.Distance;
                    line.ListOfStation.ElementAt(index).Time = adjacent_stations.Time;
                }
                catch (DO.BadAdjacentStationsException)
                {
                    line.ListOfStation.ElementAt(index).Distance = 0;
                    line.ListOfStation.ElementAt(index).Time = TimeSpan.Zero;

                }
            }
        }

        bool line_can_save(LineTotal line)
        {
            var a = from item in line.ListOfStation
                    where item.Distance == 0 && item.Time == TimeSpan.Zero
                    select item;
            if (a.FirstOrDefault() == line.ListOfStation.LastOrDefault()&& line.ListOfStation.Count()>=2)
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
                throw new  BO.BadSaveLineException();
            IDL idl = DLFactory.GetDL();
            int line_id=line.Id;

            //add do.line
            if (line.Id == 0)
                line_id = idl.AddLine(new DO.Line { Area = (DO.Areas)line.Area, Code = line.Code, Id = 0, FirstStation = line.ListOfStation.First().Code, LastStation = line.ListOfStation.Last().Code });
            else
                idl.UpdateLine(new DO.Line { Area = (DO.Areas)line.Area, Code = line.Code, Id = line_id, FirstStation = line.ListOfStation.First().Code, LastStation = line.ListOfStation.Last().Code });

            IEnumerator<ListedLineStation> iter = line.ListOfStation.GetEnumerator();
            ListedLineStation first = iter.Current;
            int IDPrev = 0, index=1;
            if (line.Id != 0)// if line don't new
                idl.DeleteLineFromAllStations(line.Id);

            // add all line-station without last, and add all AdjacentStations
            while (iter.MoveNext())
            {
                idl.AddLineStation(new DO.LineStation { LineId =line_id, PrevStation = IDPrev, NextStation = iter.Current.Code, LineStationIndex = index, Station = first.Code });
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


        }
        #endregion

        
        #region StationWithLines
        public BO.StationWithLines GetStationWithLines(int code)
        {
            BO.StationWithLines stationWith = new StationWithLines();
            BO.Station station = stationDoBoAdapter(dl.GetStation(code));
            station.CopyPropertiesTo(stationWith);
           
            stationWith.ListOfLines = dl.GetAllLineStationBy(sta => sta.Station == code).Select(st=> GetLine(st.LineId)).Distinct().OrderBy(line=>line.Code);
            return stationWith;
        }
        #endregion








    }
}
