using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mdt.Model
{
    public class Books
    {
        public int BookId { get; set; }
        public string BookName { get; set; }
        public string DirName { get; set; }
        public string Cover { get; set; }
        public int Subject { get; set; }
        public int Edition { get; set; }
        public int Grade { get; set; }
        public int Booklet { get; set; }
        public string AgeMin { get; set; }
        public string AgeMax { get; set; }
        public string Press { get; set; }
        public string Remark { get; set; }

        public string BookType { get; set; }

        public string AgeRange { get; set; }
    }
}
