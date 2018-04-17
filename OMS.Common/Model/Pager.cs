using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OMS.Common.Model
{
    public class Pager
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int PageCount { get; private set; }

        public int TotalCount { get; set; }

        public Pager()
        {
            PageIndex = 1;
            PageSize = 10;
            TotalCount = 0;
            PageCount = 0;
        }

        public Pager(int totalCount)
        {
            PageIndex = 1;
            PageSize = 10;
            TotalCount = totalCount;
            PageCount = Convert.ToInt32(Math.Ceiling(TotalCount * 1.0 / PageSize));
        }

        public Pager(int pageSize, int totalCount)
        {
            PageIndex = 1;
            PageSize = pageSize;
            TotalCount = totalCount;
            PageCount = Convert.ToInt32(Math.Ceiling(TotalCount * 1.0 / PageSize));
        }

        public Pager(int pageIndex, int pageSize, int totalCount)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            PageCount = Convert.ToInt32(Math.Ceiling(TotalCount * 1.0 / PageSize));
        }
    }
}
