using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLAPI
{
    public interface IBL
    {
        IEnumerable<BO.ListedLineStation> GetStationCodeNameDistanceTimeInLine(int LineId);

        #region BasicLine
        BO.BasicLine GetLine(int id);
        IEnumerable<BO.BasicLine> GetAllLines();

        void DeleteLine(int id);

        void UpdateLineArea(int lineId, BO.Areas area);

        #endregion

        #region LineAndStations
        BO.LineAndStations GetLineAndStations(int id);

        #endregion

        #region Station
        IEnumerable<BO.Station> GetAllOtherStations(int prevCode, int NextCode);
        IEnumerable<BO.Station> GetAllOtherStations(int code);
        #endregion
    }
}
