using BoxOms.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BoxOms.Web.Controllers
{
    /// <summary>
    /// 普通基类
    /// </summary>
    public class BaseController : Controller
    {
        VM_SyPassport_UserInfo userInfo = new VM_SyPassport_UserInfo();

        /// <summary>
        /// 构造函数
        /// </summary>
        public BaseController()
        {
            userInfo = System.Web.HttpContext.Current.Session["UserInfo"] as VM_SyPassport_UserInfo;
        }

        protected int GetUserId()
        {
            return userInfo == null ? 0 : userInfo.Id;
        }

        protected string GetUserName()
        {
            return userInfo == null ? "" : userInfo.Name;
        }
    }
}