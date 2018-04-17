using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BoxOms.Web.Models;
using BoxOms.Database;

namespace BoxOms.Web.Controllers
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public class SyUserController : Controller
    {
        public ActionResult Index(VM_SyUser_Index m)
        {
            if (m.Grid == null)
            {
                m.Grid = new OMS.Common.Model.PList<VM_SyUser_Index_Grid>();
                m.Grid.Pager = new OMS.Common.Model.Pager();
            }

            using (var db = new box_omsEntities())
            {
                var query = db.sys_user.OrderBy(o => o.UserId).AsQueryable();

                if (m.State != null)
                {
                    query = query.Where(w => w.State == m.State);
                }
                if (!string.IsNullOrWhiteSpace(m.Key))
                {
                    query = query.Where(w => w.Account.Contains(m.Key) || w.Name.Contains(m.Key));
                }

                m.Grid.Pager = new OMS.Common.Model.Pager(m.Grid.Pager.PageIndex, m.Grid.Pager.PageSize, query.Count());
                m.Grid.Data = query.Skip((m.Grid.Pager.PageIndex - 1) * m.Grid.Pager.PageSize).Take(m.Grid.Pager.PageSize).Select(s => new VM_SyUser_Index_Grid
                {
                    Id = s.UserId,
                    Account = s.Account,
                    Name = s.Name,
                    State = s.State
                }).ToList();
            }

            return View(m);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(VM_SyUser_Form m)
        {
            if (ModelState.IsValid)
            {
                using (var db = new box_omsEntities())
                {
                    if (db.sys_user.Where(w => w.Account == m.Account).Count() > 0)
                    {
                        ModelState.AddModelError("Account", "帐号已存在。");//帐号已存在
                    }
                    else
                    {
                        sys_user dbm = db.sys_user.Add(new sys_user
                        {
                            Account = m.Account,
                            Password = OMS.Common.Function.MD5Encrypt("123456"),
                            Name = m.Name,
                            State = m.State
                        });
                        if (!string.IsNullOrWhiteSpace(m.ids))
                        {
                            string[] _ids = m.ids.Split(',');
                            for (int i = 0; i < _ids.Length; i++)
                            {
                                if (string.IsNullOrWhiteSpace(_ids[i]))
                                    continue;
                                db.sys_user_and_role.Add(new sys_user_and_role()
                                {
                                    UserId = dbm.UserId,
                                    RoleId = Convert.ToInt32(_ids[i]),
                                    CreateTime = DateTime.Now
                                });
                            }
                        }
                        db.SaveChanges();

                        return Json(new { success = true });
                    }
                }
            }

            return View(m);
        }

        public ActionResult Edit(int id)
        {
            using (var db = new box_omsEntities())
            {
                VM_SyUser_Form _m = new VM_SyUser_Form();
                sys_user dbm = db.sys_user.Find(id);
                _m.Id = dbm.UserId;
                _m.Account = dbm.Account;
                _m.Name = dbm.Name;
                _m.State = dbm.State;
                var _list = db.sys_user_and_role.Where(_ => _.UserId == dbm.UserId).ToList();
                if (_list.Count > 0)
                {
                    foreach (var item in _list)
                    {
                        _m.ids += item.RoleId + ",";
                    }
                }
                return View(_m);
            }
        }

        [HttpPost]
        public ActionResult Edit(VM_SyUser_Form m)
        {
            if (ModelState.IsValid)
            {
                using (var db = new box_omsEntities())
                {
                    sys_user dbm = db.sys_user.Find(m.Id);

                    if (dbm.Account != m.Account && db.sys_user.Where(w => w.Account == m.Account).Count() > 0)
                    {
                        ModelState.AddModelError("Account", "帐号已存在。");//帐号已存在
                    }
                    else
                    {
                        dbm.Account = m.Account;
                        dbm.Name = m.Name;
                        dbm.State = m.State;
                        //先删除然后重新添加
                        db.sys_user_and_role.RemoveRange(db.sys_user_and_role.Where(_ => _.UserId == dbm.UserId));
                        if (!string.IsNullOrWhiteSpace(m.ids))
                        {
                            string[] _ids = m.ids.Split(',');
                            for (int i = 0; i < _ids.Length; i++)
                            {
                                if (string.IsNullOrEmpty(_ids[i]))
                                    continue;
                                db.sys_user_and_role.Add(new sys_user_and_role()
                                {
                                    UserId = dbm.UserId,
                                    RoleId = Convert.ToInt32(_ids[i]),
                                    CreateTime = DateTime.Now
                                });
                            }
                        }
                        db.SaveChanges();

                        return Json(new { success = true });
                    }
                }
            }

            return View(m);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            using (var db = new box_omsEntities())
            {
                sys_user dbm = db.sys_user.Find(id);
                db.sys_user_and_role.RemoveRange(db.sys_user_and_role.Where(_ => _.UserId == dbm.UserId));
                db.sys_user.Remove(dbm);
                db.SaveChanges();
            }

            return Json(new OMS.Common.Model.JsonData { flag = OMS.Common.Model.JsonDataFlag.Succeed });
        }

        [HttpPost]
        public JsonResult ResetPwd(int id)
        {
            using (var db = new box_omsEntities())
            {
                sys_user dbm = db.sys_user.Find(id);
                dbm.Password = OMS.Common.Function.MD5Encrypt("123456");
                db.SaveChanges();
            }

            return Json(new OMS.Common.Model.JsonData { flag = OMS.Common.Model.JsonDataFlag.Succeed });
        }
    }
}