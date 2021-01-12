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

        #region line
        BO.Line LineDoBoAdapter(DO.Line lineDO)
        {
            BO.Line lineBO = new BO.Line();
            
            int id = lineDO.Id;
            
            lineDO = dl.GetLine(id);
                        
            lineDO.CopyPropertiesTo(lineBO);
                     

            return lineBO;
        }

        public BO.Line GetLine(int id)
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

        public IEnumerable<BO.Line> GetAllLines()
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




        IEnumerable<ListedLineStation> IBL.GetStationCodeNameDistanceTimeInLine(int LineId)
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
