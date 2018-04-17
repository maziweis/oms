using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BoxOms.Web.Models
{
    public class VM_BoxUpdateSys_Index
    {
        public OMS.Common.Model.PList<VM_BoxUpdateSys_Index_Grid> Grid { get; set; }
    }

    public class VM_BoxUpdateSys_Index_Grid
    {
        public int Id { get; set; }

        public double VNumber { get; set; }

        public string Name { get; set; }

        public string Principal { get; set; }

        public string Cause { get; set; }

        public int UpdateCount { get; set; }

        public bool IsPublish { get; set; }
    }

    public class VM_BoxUpdateSys_Form
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "版本号")]
        public double? VNumber { get; set; }

        [Display(Name = "导入包")]
        public HttpPostedFileBase File { get; set; }
        public string Name { get; set; }

        [Required]
        [MaxLength(25)]
        [Display(Name = "测试责任人")]
        public string Principal { get; set; }

        [Required]
        [MaxLength(1500)]
        [Display(Name = "更新原因")]
        public string Cause { get; set; }
    }

    public class VM_BoxUpdateSys_Detail
    {
        public DateTime UTime { get; set; }

        public string Mac { get; set; }
    }
}