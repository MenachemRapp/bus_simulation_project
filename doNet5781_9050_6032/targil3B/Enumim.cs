using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Targil1
{
    public enum CHOICE
    {
        ADD_BUS, PICK_BUS, REFUEL_OR_MAINTENANCE, KILOMETER_MAINTANENCE, EXIT = -1
    }

    public enum FUEL_MAINTAIN
    {
        REFUEL_BUS, MAINTENANCE_BUS
    }
    
    public enum BUS_STATUS
    {
        DRIVING, REFULING, FIXING, AVAILABLE
    }
}
