using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BoxOms.Web.Models
{
    public class VM_SyPassport_Login
    {
        [Required]
        [MaxLength(25)]
        [Display(Name = "帐号")]
        public string Account { get; set; }

        [Required]
        [MaxLength(25)]
        [Display(Name = "密码")]
        public string Password { get; set; }
    }

    public class VM_SyPassport_ChangePassword
    {
        [Required]
        [MaxLength(25, ErrorMessage = "输入内容不能大于25个字")]
        [Display(Name = "旧密码")]
        public string OldPwd { get; set; }

        [Required]
        [MaxLength(25, ErrorMessage = "输入内容不能大于25个字")]
        [Display(Name = "新密码")]
        public string NewPwd { get; set; }

        [Required]
        [MaxLength(25, ErrorMessage = "输入内容不能大于25个字")]
        [Display(Name = "确认密码")]
        public string NewPwd2 { get; set; }
    }

    public class VM_SyPassport_UserInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}