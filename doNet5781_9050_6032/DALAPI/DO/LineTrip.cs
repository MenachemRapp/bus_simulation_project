using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class LineTrip
    {
        public int Id { get; set; }
        public int LineId { get; set; }
        public TimeSpan StartAt { get; set; }//to chack if time spane
        public TimeSpan Frequency { get; set; }
        public TimeSpan FinishAt { get; set; }
        public override string ToString()
        {
            return this.ToStringProperty();
        }

    }
}
