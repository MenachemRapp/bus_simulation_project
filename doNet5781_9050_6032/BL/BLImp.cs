using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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


       public  IEnumerable<ListedLineStation> GetStationCodeNameDistanceTimeInLine(int LineId)
        {
            IDL idl = DLFactory.GetDL();
            //var x = idl.GetSortLineStationsInLine(LineId);
            //DO.LineStation last = x.Last();
            //from item in x
            //where item != last
            //let AdjacentStations = idl.GetAdjacentStations(item.Station, item.NextStation)
            //select (new ListedLineStation()
            //{
            //    Code = item.Station,
            //    Name = idl.GetStation(item.Station).Name,
            //    Distance = AdjacentStations.Distance,
            //    Time = AdjacentStations.Time
            //}



            List<ListedLineStation> list = new List<ListedLineStation>();
            foreach (LineStation item in idl.GetSortLineStationsInLine(LineId))
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
