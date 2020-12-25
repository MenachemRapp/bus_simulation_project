using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace DALAPI
{
    public interface IDL
    {
        #region Bus
        
        IEnumerable<DO.Bus> GetAllBuss();
        IEnumerable<DO.Bus> GetAllBussBy(Predicate<DO.Bus> predicate);
        DO.Bus GetBus(int id);
        void AddBus(DO.Bus bus);
        void UpdateBus(DO.Bus bus);
        void UpdateBus(int id, Action<DO.Bus> update); //method that knows to updt specific fields in Bus
        void DeleteBus(int id);
        #endregion
    }
}
