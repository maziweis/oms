using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoxOms.Web.Models
{
    public class VM_BoxRunStat_Index
    {
        public string key { get; set; }

        public string State { get; set; }
        public OMS.Common.Model.PList<VM_BoxRunStat_Index_Grid> Grid { get; set; }
    }

    public class VM_BoxRunStat_Index_Grid
    {
        public int BoxId { get; set; }

        public int State { get; set; }

        public string Prov { get; set; }

        public string SchoolName { get; set; }

        public DateTime? FirstRunTime { get; set; }

        public string English { get; set; }

        public string Math { get; set; }

        public string Chinese { get; set; }

        public string UseUserName { get; set; }
    }


    public class VM_BoxRunStat_Index2
    {
        public DateTime? SDate { get; set; }
        public DateTime? EDate { get; set; }
        public List<VM_BoxRunStat_Index2_Grid> Grid { get; set; }
    }

    public class VM_BoxRunStat_Index2_Grid
    {
        public int? Subject { get; set; }

        public int? Edition { get; set; }

        public int f0 { get; set; }

        public int f1 { get; set; }

        public int f2 { get; set; }

        public int f3 { get; set; }

        public int f4 { get; set; }

        public int f5 { get; set; }

        public int f6 { get; set; }

        public int f7 { get; set; }

        public int f8 { get; set; }

        public int f9 { get; set; }

        public int f10 { get; set; }

        public int f11 { get; set; }

        public int f12 { get; set; }

        public int EditionTotal { get; set; }

        public int SubjectTotal { get; set; }
    }
}