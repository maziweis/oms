using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BoxOms.Web.Models;
using BoxOms.Database;

namespace BoxOms.Web.Controllers
{
    public class RpEnteController : BaseController
    {
        #region 首页
        public ActionResult Index(VM_RpEnte_Index m)
        {
            if (m.Grid == null)
            {
                m.Grid = new OMS.Common.Model.PList<VM_RpEnte_Index_Grid>();
                m.Grid.Pager = new OMS.Common.Model.Pager();
            }

            using (var db = new box_omsEntities())
            {
                var query = db.rp_enterprise.OrderBy(o => o.EntId).AsQueryable();

                if (!string.IsNullOrWhiteSpace(m.Key))
                {
                    query = query.Where(w => w.EntName.Contains(m.Key));
                }

                m.Grid.Pager = new OMS.Common.Model.Pager(m.Grid.Pager.PageIndex, m.Grid.Pager.PageSize, query.Count());
                m.Grid.Data = query.Skip((m.Grid.Pager.PageIndex - 1) * m.Grid.Pager.PageSize).Take(m.Grid.Pager.PageSize).Select(s => new VM_RpEnte_Index_Grid
                {
                    Id = s.EntId,
                    Name = s.EntName,
                    ActiveNum = s.rp_cdkey.Count(),
                    AlreadyActiveNum = s.rp_cdkey.Where(_ => _.ActiveTime != null).Count(),
                    Remark = s.Remark
                }).ToList();
            }

            return View(m);
        }
        #endregion

        #region 新增
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(VM_RpEnte_Form m)
        {
            if (ModelState.IsValid)
            {
                using (var db = new box_omsEntities())
                {
                    if (db.rp_enterprise.Where(w => w.EntName == m.Name).Count() > 0)
                    {
                        ModelState.AddModelError("Name", "企业名称已存在");
                    }
                    else
                    {
                        rp_enterprise dbm = new rp_enterprise
                        {
                            EntName = m.Name,
                            Remark = m.Remark
                        };

                        db.rp_enterprise.Add(dbm);
                        db.SaveChanges();

                        return Json(new { success = true });
                    }
                }
            }

            return View(m);
        }
        #endregion

        #region 编辑
        public ActionResult Edit(int id)
        {
            VM_RpEnte_Form m = new VM_RpEnte_Form();

            using (var db = new box_omsEntities())
            {
                var dbm = db.rp_enterprise.Find(id);
                m.Id = dbm.EntId;
                m.Name = dbm.EntName;
                m.Remark = dbm.Remark;
            }

            return View(m);
        }

        [HttpPost]
        public ActionResult Edit(VM_RpEnte_Form m)
        {
            if (ModelState.IsValid)
            {
                using (var db = new box_omsEntities())
                {
                    rp_enterprise dbm = db.rp_enterprise.Find(m.Id);

                    if (m.Name.ToUpper() != dbm.EntName.ToUpper() && db.rp_enterprise.Where(w => w.EntName == m.Name).Count() > 0)
                    {
                        ModelState.AddModelError("Name", "企业名称已存在");
                    }
                    else
                    {
                        dbm.EntName = m.Name;
                        dbm.Remark = m.Remark;
                        db.SaveChanges();

                        return Json(new { success = true });
                    }
                }
            }

            return View(m);
        }
        #endregion

        #region 删除
        [HttpPost]
        public JsonResult Delete(int id)
        {
            using (var db = new box_omsEntities())
            {
                db.rp_cdkey_and_edition.RemoveRange(db.rp_cdkey_and_edition.Where(_ => _.rp_cdkey.rp_enterprise.EntId == id));
                db.rp_cdkey.RemoveRange(db.rp_cdkey.Where(_ => _.EntId == id));
                db.rp_enterprise.Remove(db.rp_enterprise.Find(id));
                db.SaveChanges();
            }

            return Json(new OMS.Common.Model.JsonData { flag = OMS.Common.Model.JsonDataFlag.Succeed });
        }
        #endregion
    }
}