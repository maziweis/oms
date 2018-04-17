using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BoxOms.Web.Models
{
    public class VM_SyRole_Index
    {
    }

    public class VM_SyRole_Index_Grid
    {
        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public string Remark { get; set; }
    }

    public class VM_SyRole_Form
    {
        [Required]
        [MaxLength(50)]
        [Display(Name = "角色名称")]
        public string RoleName { get; set; }

        [Display(Name = "备注")]
        public String Remark { get; set; }

        public string ids { get; set; }

        public int? RoleId { get; set; }
    }
}