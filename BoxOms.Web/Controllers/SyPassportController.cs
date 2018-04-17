using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BoxOms.Web.Models;
using BoxOms.Database;

namespace BoxOms.Web.Controllers
{
    [AllowAnonymous]
    public class SyPassportController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(VM_SyPassport_Login m)
        {
            if (ModelState.IsValid)
            {
                using (var db = new box_omsEntities())
                {
                    var dbm = db.sys_user.Where(w => w.Account == m.Account).FirstOrDefault();

                    if (dbm == null)
                    {
                        ModelState.AddModelError("Account", "你输入的帐号不存在！");
                    }
                    else if (dbm.Password.Trim() != OMS.Common.Function.MD5Encrypt(m.Password.Trim()))
                    {
                        ModelState.AddModelError("Account", "密码错误！");
                    }
                    else if (dbm.State == 1)
                    {
                        ModelState.AddModelError("Account", "帐号已停用！");
                    }
                    else
                    {
                        VM_SyPassport_UserInfo info = new VM_SyPassport_UserInfo();
                        info.Id = dbm.UserId;
                        info.Name = dbm.Name;
                        System.Web.HttpContext.Current.Session["UserInfo"] = info;

                        return RedirectToAction("Index", "BoxHome");
                    }
                }
            }

            return View(m);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(VM_SyPassport_ChangePassword m)
        {
            if (ModelState.IsValid)
            {
                using (var db = new box_omsEntities())
                {
                    if (m.NewPwd != m.NewPwd2)
                    {
                        ModelState.AddModelError("NewPwd2", "两次密码不一样！");
                        return View(m);
                    }
                    VM_SyPassport_UserInfo info = (VM_SyPassport_UserInfo)Session["UserInfo"];
                    sys_user _user = db.sys_user.Find(info.Id);
                    if (_user.Password.Trim() != OMS.Common.Function.MD5Encrypt(m.OldPwd.Trim()))
                    {
                        ModelState.AddModelError("OldPwd", "原始密码不正确！");
                        return View(m);
                    }
                    _user.Password = OMS.Common.Function.MD5Encrypt(m.NewPwd.Trim());
                    db.SaveChanges();
                }

                return Json(new { success = true });
            }
            return View(m);

        }

        [HttpPost]
        public JsonResult Exit()
        {
            System.Web.HttpContext.Current.Session["UserInfo"] = null;
            return Json(new OMS.Common.Model.JsonData { flag = OMS.Common.Model.JsonDataFlag.Succeed });
        }
    }
}