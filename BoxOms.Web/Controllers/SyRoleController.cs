using BoxOms.Database;
using BoxOms.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BoxOms.Web.Controllers
{
    public class SyRoleController : Controller
    {
        // GET: SyRole
        public ActionResult Index()
        {
            using (var db = new box_omsEntities())
            {
                var query = db.sys_role.OrderBy(o => o.RoleId).ToList();
                ViewData["list"] = query;
            }
            return View();
        }

        #region 新增角色
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(VM_SyRole_Form m)
        {
            if (ModelState.IsValid)
            {
                using (var db = new box_omsEntities())
                {
                    if (db.sys_role.Where(w => w.RoleName == m.RoleName).Count() > 0)
                    {
                        ModelState.AddModelError("RoleName", "角色名已存在。");
                        return View(m);
                    }

                    sys_role _model = db.sys_role.Add(new sys_role
                    {
                        RoleName = m.RoleName,
                        Remark = m.Remark
                    });
                    if (!string.IsNullOrWhiteSpace(m.ids))
                    {
                        string[] _ids = m.ids.Split(',');
                        for (int i = 0; i < _ids.Length; i++)
                        {
                            if (string.IsNullOrWhiteSpace(_ids[i]))
                                continue;
                            db.sys_role_and_auth.Add(new sys_role_and_auth()
                            {
                                Id = Guid.NewGuid().ToString(),
                                RoleId = _model.RoleId,
                                LeftId = Convert.ToInt32(_ids[i])
                            });
                        }
                    }
                    db.SaveChanges();

                    return RedirectToAction("Index");

                }
            }

            return View(m);
        }
        #endregion

        #region 编辑
        public ActionResult Edit(int id)
        {
            VM_SyRole_Form _m = new VM_SyRole_Form();
            using (var db = new box_omsEntities())
            {
                var dbm = db.sys_role.Find(id);
                _m.RoleName = dbm.RoleName;
                _m.Remark = dbm.Remark;
                _m.RoleId = dbm.RoleId;
                var _list = db.sys_role_and_auth.Where(_ => _.RoleId == dbm.RoleId).ToList();
                if (_list.Count > 0)
                {
                    foreach (var item in _list)
                    {
                        _m.ids += item.LeftId + ",";
                    }
                }

            }
            return View(_m);
        }

        [HttpPost]
        public ActionResult Edit(VM_SyRole_Form m)
        {
            if (ModelState.IsValid)
            {
                using (var db = new box_omsEntities())
                {
                    sys_role dbm = db.sys_role.Find(m.RoleId);
                    if (dbm.RoleName != m.RoleName && db.sys_role.Where(w => w.RoleName == m.RoleName).Count() > 0)
                    {
                        ModelState.AddModelError("RoleName", "角色名已存在。");
                        return View(m);
                    }
                    dbm.Remark = m.Remark;
                    //先删除然后重新添加
                    db.sys_role_and_auth.RemoveRange(db.sys_role_and_auth.Where(_ => _.RoleId == m.RoleId));
                    if (!string.IsNullOrWhiteSpace(m.ids))
                    {
                        string[] _ids = m.ids.Split(',');
                        for (int i = 0; i < _ids.Length; i++)
                        {
                            if (string.IsNullOrEmpty(_ids[i]))
                                continue;
                            db.sys_role_and_auth.Add(new sys_role_and_auth()
                            {
                                Id = Guid.NewGuid().ToString(),
                                RoleId = dbm.RoleId,
                                LeftId = Convert.ToInt32(_ids[i])
                            });
                        }
                    }
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(m);
        }
        #endregion
    }
}