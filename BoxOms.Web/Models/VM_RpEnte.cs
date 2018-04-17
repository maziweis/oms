using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BoxOms.Web.Models
{
    public class VM_RpEnte_Index
    {
        public string Key { get; set; }
        public OMS.Common.Model.PList<VM_RpEnte_Index_Grid> Grid { get; set; }
    }

    public class VM_RpEnte_Index_Grid
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int ActiveNum { get; set; }

        public int AlreadyActiveNum { get; set; }

        public string Remark { get; set; }
    }

    public class VM_RpEnte_Form
    {
        public int? Id { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "输入的字符不能超过50个！")]
        [Display(Name = "企业名称")]
        public string Name { get; set; }

        [Display(Name = "备注")]
        [MaxLength(50, ErrorMessage = "输入的字符不能超过50个！")]
        public string Remark { get; set; }
    }
}