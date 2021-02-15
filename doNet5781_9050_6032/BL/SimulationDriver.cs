using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    sealed class SimulationDriver
    {

        #region singleton
        static readonly SimulationDriver instance = new SimulationDriver();
        static SimulationDriver() { }// static ctor to ensure instance init is done just before first usage
        SimulationDriver() { } // default => private
        public static SimulationDriver Instance { get => instance; }// The public Instance property to use

        #endregion

    }
}
