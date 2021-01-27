using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class NewLine
    {
        public int Code { get; set; }
        public Areas Area { get; set; }
                
        public IEnumerable<Station> ListOfStation { get; set; }
    }
}
