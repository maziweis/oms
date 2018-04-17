using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mdt.Model
{

    public class BookCatalogs
    {
        public int CatalogId { get; set; }

        public int BookId { get; set; }

        public int PId { get; set; }

        public string CatalogName { get; set; }

        public string PageStart { get; set; }

        public string PageEnd { get; set; }

        public int Sort { get; set; }
    }
}
