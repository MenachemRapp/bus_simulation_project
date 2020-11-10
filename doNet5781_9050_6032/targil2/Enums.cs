using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace targil2
{
    public enum Area
    {
        Central,Jerusalem,North,South,Center
    }
    public enum CHOICE
    {
        ADD,REMOVE, FIND,PRINT, EXIT = -1
    }

    public enum ADD
    {
        ADD_LINE, ADD_BUS_STOP
    }

    public enum REMOVE
    {
        REMOVE_LINE, REMOVE_BUS_STOP
    }

    public enum FIND
    {
        FIND_LINES_IN_STOP, OPTIONS_BETWEEN_STOPS
    }
        public enum PRINT
    {
        PRINT_LINES, PRINT_STOPS
    }
}
