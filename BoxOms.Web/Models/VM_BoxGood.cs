using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BoxOms.Web.Models
{
    public class VM_BoxGood_Index
    {
        public int? State { get; set; }
        public string Key { get; set; }
        public OMS.Common.Model.PList<VM_BoxGood_Index_Grid> Grid { get; set; }
    }

    public class VM_BoxGood_Index_Grid
    {
        public int id { get; set; }
        public string Code { get; set; }

        public string Mac { get; set; }

        public string ActivNumber { get; set; }

        public bool IsActiv { get; set; }

        public double SysVersion { get; set; }

        public string Remark { get; set; }

        public string IP { get; set; }

        public DateTime? ActivTime { get; set; }

        public DateTime? ExpirTime { get; set; }
    }

    public class VM_BoxGood_Form
    {
        public int? id { get; set; }

        [Required]
        [RegularExpression(@"(?i)^([A-Fa-f0-9]{2}-){5}[A-Fa-f0-9]{2}$", ErrorMessage = "MAC地址格式不正确！")]
        [MaxLength(30)]
        [Display(Name = "MAC地址")]
        public string MAC { get; set; }
        [Display(Name = "备注")]
        [MaxLength(50)]
        public String Remark { get; set; }

        [Display(Name = "IP地址")]
        public String IP { get; set; }

        [Required]
        [Display(Name = "版本号")]
        [Range(typeof(double), "1.00", "99999999.99", ErrorMessage = "版本号格式不正确！")]
        [RegularExpression(@"^([1-9][0-9]*)+(.[0-9]{1,2})?$", ErrorMessage = "请输入正整数或保留两位小数的数字！")]
        public double? SysVersion { get; set; }

        [Display(Name = "有效期")]
        public string Validity { get; set; }
    }

    public class VM_BoxGood_Form1
    {
        [Display(Name = "盒子编号")]
        public int? id { get; set; }

        [Required]
        [Display(Name = "所属省")]
        public int? Prov { get; set; }

        [Required]
        [Display(Name = "所属市")]
        public int? City { get; set; }

        [Required]
        [Display(Name = "所属区")]
        public int? Area { get; set; }

        [Display(Name = "产品状态")]
        public int State { get; set; }

        [Required]
        [Display(Name = "领用人员")]
        public string UseUserName { get; set; }

        [Required]
        [Display(Name = "学校名称")]
        public string SchoolName { get; set; }

        public string English { get; set; }

        public string Math { get; set; }

        public string Chinese { get; set; }
    }


    public class VM_BoxGood_UpdateVer
    {
        public string mac { get; set; }

        public string ver { get; set; }
    }

    public class dataInfo
    {
        public string TypeName { get; set; }
    }
}