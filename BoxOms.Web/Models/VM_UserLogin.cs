using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoxOms.Web.Models
{
    public class VM_UserLogin_Index
    {
        public int? UseProv { get; set; }

        public int? UseCity { get; set; }

        public int? UseDist { get; set; }

        public string keyid { get; set; }
        public OMS.Common.Model.PList<VM_UserLogin_Index_Grid> Grid { get; set; }
    }

    public class VM_UserLogin_Index_Grid
    {
        public string Id { get; set; }
        public string LoginAccount { get; set; }
        public string LoginUserName { get; set; }
        public string LoginSchool { get; set; }
        public string LoginRoleName { get; set; }
        public string LoginEnte { get; set; }
        public string LoginDistName { get; set; }
        public string LoginKey { get; set; }
        public DateTime? LoginTime { get; set; }
        public string LoginIP { get; set; }

        public int? count { get; set; }
    }

    public class VM_UserOper_Index
    {
        public OMS.Common.Model.PList<VM_UserOper_Index_Grid> Grid { get; set; }
    }

    public class VM_UserOper_Index_Grid
    {
        public string UserName { get; set; }
    }

    public class UserOperdata
    {
        public string RoleName { get; set; }

        public string SchoolName { get; set; }

        public int BookNum { get; set; }

        public int YktNum { get; set; }

        public int DykNum { get; set; }
    }

    public class UserGroup
    {
        public string UserName { get; set; }
    }
}