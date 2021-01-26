using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    [Serializable]
    public class BadLineIdException : Exception
    {
        public int ID;
        public BadLineIdException(string message, Exception innerException) :
            base(message, innerException) => ID = ((DO.BadLineIdException)innerException).ID;
        public override string ToString() => base.ToString() + $", bad line id: {ID}";
    }

    public class BadStationCodeException : Exception
    {
        public int Code;
        public BadStationCodeException(string message, Exception innerException) :
            base(message, innerException) => Code = ((DO.BadStationCodeException)innerException).Code;
        public override string ToString() => base.ToString() + $", bad station code: {Code}";
    }

    public class BadAdjacentStationsException : Exception
    {
        public int Code1, Code2;
        public BadAdjacentStationsException(string message, Exception innerException) :
            base(message, innerException) 
            {Code1 = ((DO.BadAdjacentStationsException) innerException).Code1;
            Code2 = ((DO.BadAdjacentStationsException)innerException).Code2;
        }
        public override string ToString() => base.ToString() + $",bad adjacent stations: {Code1} and {Code2}";
    }
      public class BadSaveLineException : Exception
    {

    }
}
