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
        public LineTotal GetLineNew(int Id)
        {
           
            IDL dl = DLFactory.GetDL();
            LineTotal line = new LineTotal();
            Line do_line = dl.GetLine(Id);
            line.Area = (BO.Areas) do_line.Area;
            line.Code = do_line.Code;
            line.Id = do_line.Id;

            


        }

        #endregion


        #region StationWithLines
        public BO.StationWithLines GetStationWithLines(int code)
        {
            BO.StationWithLines stationWith = new StationWithLines();
            BO.Station station = stationDoBoAdapter(dl.GetStation(code));
            station.CopyPropertiesTo(stationWith);
           
           List<DO.LineStation> lineStationList = dl.GetAllLineStationBy(sta => sta.Station == code).ToList();
            //IEnumerable<DO.LineStation> lineStationList = dl.GetAllLineStationBy(sta => sta.Station == code);
            
            stationWith.ListOfLines = new List<BO.BasicLine>();
            List<BO.BasicLine> lineList= new List<BasicLine>();
            foreach (DO.LineStation lineStation in lineStationList)//change to LINQ////////////////
                lineList.Add(GetLine(lineStation.LineId));
            stationWith.ListOfLines = lineList;
            return stationWith;
        }
        #endregion








    }
}
