using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BoxOms.Web.Models
{
    public class VM_SyUser_Index
    {
        public int? State { get; set; }
        public string Key { get; set; }
        public OMS.Common.Model.PList<VM_SyUser_Index_Grid> Grid { get; set; }
    }

    public class VM_SyUser_Index_Grid
    {
        public int Id { get; set; }

        public string Account { get; set; }

        public string Name { get; set; }

        public int State { get; set; }
    }

    public class VM_SyUser_Form
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(25, ErrorMessage = "输入内容不能大于25个字")]
        [Display(Name = "帐号")]
        public string Account { get; set; }

        [Required]
        [MaxLength(25, ErrorMessage = "输入内容不能大于25个字")]
        [Display(Name = "姓名")]
        public string Name { get; set; }

        [Display(Name = "状态")]
        public int State { get; set; }

        [Display(Name = "角色")]
        public string ids { get; set; }
    }
}