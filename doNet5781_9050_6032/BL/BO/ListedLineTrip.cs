using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class ListedLineTrip
    {        
        public int Id { get; set; }
        public TimeSpan StartAt { get; set; }

        public bool Valid { get; set; }
        
    }
}
