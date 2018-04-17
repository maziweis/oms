using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoxOms.Web.Models
{
    public class VM_BoxSta_Index
    {
        public DateTime? SDate { get; set; }
        public DateTime? EDate { get; set; }
        public VM_BoxSta_Index_Grid grid { get; set; }
        public List<VM_BoxRunStat_Index_Grid> list { get; set; }

        public string RunTime { get; set; }

        public int total { get; set; }

        public int? Prov { get; set; }

        public int? City { get; set; }

        public int? Area { get; set; }

        public int? BoxID { get; set; }
    }

    public class VM_BoxSta_TotalIndex
    {
        public DateTime? SDate { get; set; }
        public DateTime? EDate { get; set; }
        public VM_BoxSta_Index_Grid grid { get; set; }
        public List<VM_BoxRunStat_Index_Grid> list { get; set; }

    }

    public class VM_BoxSta_Index_Grid
    {
        /// <summary>
        /// 英语的版本
        /// </summary>
        public ArrayList English { get; set; }
        /// <summary>
        /// 数学的版本
        /// </summary>
        public ArrayList Math { get; set; }
        /// <summary>
        /// 语文的版本
        /// </summary>
        public ArrayList Chinese { get; set; }
    }
}