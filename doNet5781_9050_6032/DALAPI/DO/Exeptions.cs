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

        public BadAdjacentStationsException(int code1, int code2) : base() { Code1 = code1; Code2 = code2; }
        public BadAdjacentStationsException(int code1, int code2, string message) :
            base(message)
        { Code1 = code1; Code2 = code2; }
        public BadAdjacentStationsException(int code1, int code2, string message, Exception innerException) :
            base(message, innerException)
        { Code1 = code1; Code2 = code2; }

        public override string ToString() => base.ToString() + $",bad adjacent stations: {Code1} and {Code2}";
    }

    public class BadLineTripIdException : Exception
    {
        public int ID;

        public BadLineTripIdException(int Id) : base() => ID = Id;
        public BadLineTripIdException(int Id, string message) :
            base(message) => ID = Id;
        public BadLineTripIdException(int Id, string message, Exception innerException) :
            base(message, innerException) => ID = Id;

        public override string ToString() => base.ToString() + $", bad line trip ID : {ID}";
    }

    public class BadLineStationException : Exception
    {
        public int LineId;
        public int StationCode;
        public BadLineStationException(int LineID, int StationCODE) : base() { LineId = LineID; StationCode = StationCODE; }
        public BadLineStationException(int LineID, int StationCODE, string message) : base(message) { LineId = LineID; StationCode = StationCODE; }
        public BadLineStationException(int LineID, int StationCODE, string message, Exception innerException) : base(message, innerException) { LineId = LineID; StationCode = StationCODE; }
        public override string ToString() => base.ToString() + $", bad line station, line= : {LineId} station = {StationCode}";

    }


    public class XMLFileLoadCreateException : Exception
    {
        public string xmlFilePath;
        public XMLFileLoadCreateException(string xmlPath) : base() { xmlFilePath = xmlPath; }
        public XMLFileLoadCreateException(string xmlPath, string message) :
            base(message)
        { xmlFilePath = xmlPath; }
        public XMLFileLoadCreateException(string xmlPath, string message, Exception innerException) :
            base(message, innerException)
        { xmlFilePath = xmlPath; }

        public override string ToString() => base.ToString() + $", fail to load or create xml file: {xmlFilePath}";
    }
}
