using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoxOms.Web.Models
{
    public class VM_BookUse_Index
    {
        public DateTime? SDate { get; set; }
        public DateTime? EDate { get; set; }
        public VM_Book_Edition grid { get; set; }
    }


    public class VM_Book_Edition
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



    public class VM_AreaBookUse_Index
    {

        public int? UseProv { get; set; }

        public int? UseCity { get; set; }

        public int? UseDist { get; set; }

        public string keyid { get; set; }
        public VM_Book_Edition grid { get; set; }

        public ArrayList AreaList { get; set; }
    }

    public class VM_AreaDyk_Index
    {

        public int? UseProv { get; set; }

        public int? UseCity { get; set; }

        public int? UseDist { get; set; }

        public string keyid { get; set; }

        public ArrayList AreaList { get; set; }
    }

    public class VM_AreaYkt_Index
    {

        public int? UseProv { get; set; }

        public int? UseCity { get; set; }

        public int? UseDist { get; set; }

        public string keyid { get; set; }

        public VM_Book_Edition grid { get; set; }
        public ArrayList AreaList { get; set; }
    }

    public class VM_YktAll_Index
    {
        public DateTime? SDate { get; set; }
        public DateTime? EDate { get; set; }
        public VM_Book_Edition grid { get; set; }

    }

}