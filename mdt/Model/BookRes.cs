using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mdt.Model
{
    public class BookRes
    {
        public int BookId { get; set; }
        public int PageIndex { get; set; }
        public string Content { get; set; }
        public int IsSys { get; set; }
    }
}
