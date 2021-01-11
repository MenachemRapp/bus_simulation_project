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

        #region line
        BO.Line GetLine(int id);
        IEnumerable<BO.Line> GetAllLines();
        #endregion


    }
}
