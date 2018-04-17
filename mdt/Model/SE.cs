using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mdt.Model
{
    public class SE
    {
        public int Subject { get; set; }

        public int Edition { get; set; }
    }

    public class C_E
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }

    public class Edition
    {
        public string SubjectID { get; set; }
        public string EditionID { get; set; }
    }
}
