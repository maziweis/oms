using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mdt.Model
{
    public class ResourceTypes
    {
        public int ID { get; set; }
        public string CodeName { get; set; }
        public int ParentID { get; set; }
        public string Sort { get; set; }
    }
}
