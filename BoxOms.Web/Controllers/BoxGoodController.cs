using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BoxOms.Web.Models;
using BoxOms.Database;

namespace BoxOms.Web.Controllers
{
    public class BoxGoodController : BaseController
    {
        #region 首页
        public ActionResult Index(VM_BoxGood_Index m)
        {
            if (m.Grid == null)
            {
                m.Grid = new OMS.Common.Model.PList<VM_BoxGood_Index_Grid>();
                m.Grid.Pager = new OMS.Common.Model.Pager();
            }

            using (var db = new box_omsEntities())
            {
                var query = db.box_good.OrderByDescending(o => o.BoxId).AsQueryable();

                if (!string.IsNullOrWhiteSpace(m.Key))
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(m.Key, @"^[0-9]+$"))
                    {
                        int _id = Convert.ToInt32(m.Key);
                        query = query.Where(w => w.BoxId == _id);
                    }
                    else
                        query = query.Where(w => w.Mac.Contains(m.Key));
                }

                m.Grid.Pager = new OMS.Common.Model.Pager(m.Grid.Pager.PageIndex, m.Grid.Pager.PageSize, query.Count());
                m.Grid.Data = query.Skip((m.Grid.Pager.PageIndex - 1) * m.Grid.Pager.PageSize).Take(m.Grid.Pager.PageSize).Select(s => new VM_BoxGood_Index_Grid
                {
                    Code = s.Code,
                    Mac = s.Mac,
                    ActivNumber = s.ActivNumber,
                    IsActiv = s.IsActiv,
                    SysVersion = s.SysVersion,
                    id = s.BoxId,
                    Remark = s.Remark,
                    IP = s.IP,
                    ActivTime = s.FirstRunTime,
                    ExpirTime = s.ExpirTime
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
        public ActionResult Add(VM_BoxGood_Form m)
        {
            if (ModelState.IsValid)
            {
                using (var db = new box_omsEntities())
                {
                    if (db.box_good.Where(w => w.Mac == m.MAC).Count() > 0)
                    {
                        ModelState.AddModelError("MAC", "商品MAC地址已存在。");
                    }
                    else
                    {
                        DateTime? _ExpirTime = null;
                        int? _Validity = null;
                        if (m.Validity != null)
                        {
                            if (!System.Text.RegularExpressions.Regex.IsMatch(m.Validity.ToString(), @"^(([1-9]\d*))$"))
                            {
                                ModelState.AddModelError("Validity", "请输入大于0的正整数！");
                                return View(m);
                            }
                            _Validity = Convert.ToInt32(m.Validity);
                        }
                        if (_Validity == null)
                            _ExpirTime = Convert.ToDateTime("2099-12-12");
                        else
                            _ExpirTime = DateTime.Now.AddMonths(Convert.ToInt32(_Validity));
                        box_good dbm = new box_good
                        {
                            Mac = m.MAC,
                            ActivNumber = m.MAC == null ? "" : BLL.BoxGoodBLL.GetKey(m.MAC, _ExpirTime),
                            SysVersion = Convert.ToDouble(m.SysVersion),
                            State = 1,
                            IsCanUpdate = true,
                            IsActiv = false,
                            Total = 0,
                            Remark = m.Remark,
                            CreateTime = DateTime.Now,
                            CreateUserId = GetUserId(),
                            IP = m.IP,
                            ExpirTime = _ExpirTime,
                            Validity = _Validity
                        };

                        box_good _m = db.box_good.Add(dbm);
                        db.SaveChanges();
                        db.box_good_online.Add(new box_good_online
                        {
                            BoxId = _m.BoxId
                        });
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
            VM_BoxGood_Form m = new VM_BoxGood_Form();

            using (var db = new box_omsEntities())
            {
                var dbm = db.box_good.Find(id);
                m.MAC = dbm.Mac;
                m.SysVersion = dbm.SysVersion;
                m.Remark = dbm.Remark;
                m.id = dbm.BoxId;
                m.IP = dbm.IP;
                m.Validity = dbm.Validity == null ? "" : dbm.Validity.ToString();
            }

            return View(m);
        }

        [HttpPost]
        public ActionResult Edit(VM_BoxGood_Form m)
        {
            if (ModelState.IsValid)
            {
                using (var db = new box_omsEntities())
                {
                    box_good dbm = db.box_good.Find(m.id);

                    if (m.MAC.ToUpper() != dbm.Mac.ToUpper() && db.box_good.Where(w => w.Mac == m.MAC).Count() > 0)
                    {
                        ModelState.AddModelError("MAC", "商品MAC地址已存在。");
                    }
                    else
                    {
                        DateTime? _ExpirTime = null;
                        int? _Validity = null;
                        if (m.Validity != null)
                        {
                            if (!System.Text.RegularExpressions.Regex.IsMatch(m.Validity.ToString(), @"^(([1-9]\d*))$"))
                            {
                                ModelState.AddModelError("Validity", "请输入大于0的正整数！");
                                return View(m);
                            }
                            _Validity = Convert.ToInt32(m.Validity);
                        }
                        if (_Validity == null)
                            _ExpirTime = Convert.ToDateTime("2099-12-12");
                        else
                            _ExpirTime = DateTime.Now.AddMonths(Convert.ToInt32(_Validity));
                        dbm.Validity = _Validity;
                        dbm.IP = m.IP;
                        dbm.SysVersion = Convert.ToDouble(m.SysVersion);
                        dbm.Mac = m.MAC;
                        dbm.ActivNumber = m.MAC == null ? "" : BLL.BoxGoodBLL.GetKey(m.MAC, _ExpirTime);
                        dbm.Remark = m.Remark;
                        dbm.UpdateTime = DateTime.Now;
                        dbm.ExpirTime = _ExpirTime;
                        dbm.UpdateUserId = System.Web.HttpContext.Current.Session["UserInfo"] == null ? 0 : (System.Web.HttpContext.Current.Session["UserInfo"] as VM_SyPassport_UserInfo).Id;
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
                db.box_good_online.RemoveRange(db.box_good_online.Where(_ => _.BoxId == id));
                db.box_update_sys_log.RemoveRange(db.box_update_sys_log.Where(_ => _.BoxId == id));
                db.box_subject_edition.RemoveRange(db.box_subject_edition.Where(_ => _.BoxId == id));
                db.box_resource_statist.RemoveRange(db.box_resource_statist.Where(_ => _.BoxId == id));
                db.box_resource_statist_day.RemoveRange(db.box_resource_statist_day.Where(_ => _.BoxId == id));
                db.box_good.Remove(db.box_good.Find(id));
                db.SaveChanges();
            }

            return Json(new OMS.Common.Model.JsonData { flag = OMS.Common.Model.JsonDataFlag.Succeed });
        }
        #endregion

        #region 查看
        public ActionResult Look(int id)
        {
            VM_BoxGood_Index_Grid m = new VM_BoxGood_Index_Grid();

            using (var db = new box_omsEntities())
            {
                var dbm = db.box_good.Find(id);
                m.id = dbm.BoxId;
                m.Code = dbm.Code;
                m.Mac = dbm.Mac;
                m.SysVersion = dbm.SysVersion;
                m.Remark = dbm.Remark;
                m.IsActiv = dbm.IsActiv;
                m.ActivTime = dbm.FirstRunTime;
                m.IP = dbm.IP;
                m.ActivNumber = dbm.ActivNumber;
                m.ExpirTime = dbm.ExpirTime;
                ViewBag.Chinese = BoxOms.Web.BLL.BoxGoodBLL.BackEditionName(m.id, 1);
                ViewBag.Math = BoxOms.Web.BLL.BoxGoodBLL.BackEditionName(m.id, 2);
                ViewBag.English = BoxOms.Web.BLL.BoxGoodBLL.BackEditionName(m.id, 3);
            }

            return View(m);
        }
        #endregion
    }
}