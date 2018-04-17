using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BoxOms.Web.Models
{
    public class VM_RpCode_Index
    {
        public string Key { get; set; }
        public int? Prov { get; set; }

        public int? City { get; set; }

        public int? Area { get; set; }
        public OMS.Common.Model.PList<VM_RpCode_Index_Grid> Grid { get; set; }
    }

    public class VM_RpCode_Index_Grid
    {
        public string Id { get; set; }
        public int? EntId { get; set; }

        public string UseSchool { get; set; }
        public string Addr { get; set; }
        public string EnteName { get; set; }

        public int? Validity { get; set; }

        public DateTime? ActiveTime { get; set; }

        public string Remark { get; set; }

        public string ActiveCode { get; set; }

        public int? AuthUserCount { get; set; }

        public string ActiveMac { get; set; }
        public string ActiveDisk { get; set; }
        public string ActiveIp { get; set; }

        public DateTime? ExpirTime { get; set; }
    }

    public class VM_RpCode_Form
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "所属企业")]
        public int? EnteId { get; set; }

        [Display(Name = "有效期")]
        public string Validity { get; set; }

        [Display(Name = "授权人数")]
        public string AuthUserCount { get; set; }

        [Required]
        [Display(Name = "所属省")]
        public int? Provinces { get; set; }

        [Required]
        [Display(Name = "所属市")]
        public int? Cityss { get; set; }

        [Required]
        [Display(Name = "所属区")]
        public int? Areass { get; set; }

        [Display(Name = "备注")]
        [MaxLength(50, ErrorMessage = "输入的字符不能超过50个！")]
        public string Remark { get; set; }

        [Required]
        [Display(Name = "学校名称")]
        [MaxLength(20, ErrorMessage = "输入的字符不能超过20个！")]
        public string UseSchool { get; set; }

        public string English { get; set; }

        public string Math { get; set; }

        public string Chinese { get; set; }
    }
}