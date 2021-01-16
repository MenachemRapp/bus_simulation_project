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
        BO.Station stationDoBoAdapter(DO.Station stationDO)
        {
            BO.Station stationBO = new BO.Station();
            int code = stationDO.Code;
            stationDO = dl.GetStation(code);
            stationDO.CopyPropertiesTo(stationBO);

            return stationBO;
        }
        public IEnumerable<BO.Station> GetAllOtherStations(int prevCode, int NextCode)
        {
            return from station in dl.GetAllStationsBy(s=> s.Code!=prevCode && s.Code != NextCode)
                   select stationDoBoAdapter(station);
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

       
    }
}
