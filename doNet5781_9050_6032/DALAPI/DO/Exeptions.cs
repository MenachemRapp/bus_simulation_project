using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    [Serializable]
    public class BadStationCodeException : Exception
    {
        public int Code;

        public BadStationCodeException(int code) : base() => Code = code;
        public BadStationCodeException(int code, string message) :
            base(message) => Code = code;
        public BadStationCodeException(int code, string message, Exception innerException) :
            base(message, innerException) => Code = code;

        public override string ToString() => base.ToString() + $", bad station code: {Code}";
    }


    public class BadLineIdException : Exception
    {
        public int ID;

        public BadLineIdException(int id) : base() => ID = id;
        public BadLineIdException(int id, string message) :
            base(message) => ID = id;
        public BadLineIdException(int id, string message, Exception innerException) :
            base(message, innerException) => ID = id;

        public override string ToString() => base.ToString() + $", bad line id: {ID}";
    }

    public class BadBusLicenseException : Exception
    {
        public int License;

        public BadBusLicenseException(int licence) : base() => License = licence;
        public BadBusLicenseException(int licence, string message) :
            base(message) => License = licence;
        public BadBusLicenseException(int licence, string message, Exception innerException) :
            base(message, innerException) => License = licence;

        public override string ToString() => base.ToString() + $", bad licence number: {License}";
    }

    public class BadAdjacentStationsException : Exception
    {
        public int Code1, Code2;

        public BadAdjacentStationsException(int code1, int code2) : base() { Code1 = code1; Code2 = code2;}
        public BadAdjacentStationsException(int code1, int code2, string message) :
            base(message) { Code1 = code1; Code2 = code2; }
        public BadAdjacentStationsException(int code1, int code2, string message, Exception innerException) :
            base(message, innerException) { Code1 = code1; Code2 = code2; }

    public override string ToString() => base.ToString() + $",no adjacent stations: {Code1} and {Code2}";
    }
}