using BoxOms.Database;
using BoxOms.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BoxOms.Web.Controllers
{
    public class BoxUpdateController : Controller
    {
        public ActionResult Index(VM_BoxUpdateSys_Index m)
        {
            if (m.Grid == null)
            {
                m.Grid = new OMS.Common.Model.PList<VM_BoxUpdateSys_Index_Grid>();
                m.Grid.Pager = new OMS.Common.Model.Pager();
            }

            using (var db = new box_omsEntities())
            {
                var query = db.box_update_sys.OrderBy(o => o.Id).AsQueryable();

                m.Grid.Pager = new OMS.Common.Model.Pager(m.Grid.Pager.PageIndex, m.Grid.Pager.PageSize, query.Count());
                m.Grid.Data = query.Skip((m.Grid.Pager.PageIndex - 1) * m.Grid.Pager.PageSize).Take(m.Grid.Pager.PageSize).Select(s => new VM_BoxUpdateSys_Index_Grid
                {
                    Id = s.Id,
                    VNumber = s.VNumber,
                    Name = s.PackUrl,
                    Principal = s.Principal,
                    Cause = s.Cause,
                    UpdateCount = s.box_update_sys_log.Count,
                    IsPublish = s.IsPublish
                }).ToList();
            }

            return View(m);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(VM_BoxUpdateSys_Form m)
        {
            if (ModelState.IsValid)
            {
                using (var db = new box_omsEntities())
                {
                    if (db.box_update_sys.Where(w => w.VNumber == m.VNumber).Count() > 0)
                    {
                        ModelState.AddModelError("VNumber", "版本号已存在。");
                        return View(m);
                    }
                    if (m.File == null)
                    {
                        ModelState.AddModelError("File", "请选择要导入的zip包");
                        return View(m);
                    }
                    if (Path.GetExtension(m.File.FileName).ToLower() != ".zip")
                    {
                        ModelState.AddModelError("File", "只能上传zip格式的包");
                        return View(m);
                    }
                    box_update_sys dbm = new box_update_sys
                    {
                        VNumber = (double)m.VNumber,
                        Principal = m.Principal,
                        Cause = m.Cause,
                        IsPublish = false
                    };
                    dbm.PackUrl = UploadZip(m.File, ((double)m.VNumber).ToString("f2").Replace(".", "_"));
                    db.box_update_sys.Add(dbm);
                    db.SaveChanges();

                    return RedirectToAction("Index", "BoxUpdate");

                }
            }

            return View(m);
        }

        public ActionResult Edit(int id)
        {
            using (var db = new box_omsEntities())
            {
                box_update_sys dbm = db.box_update_sys.Find(id);

                return View(new VM_BoxUpdateSys_Form
                {
                    Id = dbm.Id,
                    VNumber = dbm.VNumber,
                    Name = dbm.PackUrl,
                    Principal = dbm.Principal,
                    Cause = dbm.Cause
                });
            }
        }

        [HttpPost]
        public ActionResult Edit(VM_BoxUpdateSys_Form m)
        {
            if (ModelState.IsValid)
            {
                using (var db = new box_omsEntities())
                {
                    box_update_sys dbm = db.box_update_sys.Find(m.Id);

                    if (dbm.VNumber != m.VNumber && db.box_update_sys.Where(w => w.VNumber == m.VNumber).Count() > 0)
                    {
                        ModelState.AddModelError("VNumber", "版本号已存在。");
                        return View(m);
                    }
                    if (m.File != null)
                    {
                        if (Path.GetExtension(m.File.FileName).ToLower() != ".zip")
                        {
                            ModelState.AddModelError("File", "只能上传zip格式的包");
                            return View(m);
                        }
                        dbm.PackUrl = UploadZip(m.File, ((double)dbm.VNumber).ToString("f2").Replace(".", "_"));
                    }

                    dbm.VNumber = (double)m.VNumber;

                    dbm.Principal = m.Principal;
                    dbm.Cause = m.Cause;

                    db.SaveChanges();

                    return RedirectToAction("Index", "BoxUpdate");

                }
            }

            return View(m);
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            using (var db = new box_omsEntities())
            {
                db.box_update_sys.Remove(db.box_update_sys.Find(id));
                db.SaveChanges();
            }

            return Json(new OMS.Common.Model.JsonData { flag = OMS.Common.Model.JsonDataFlag.Succeed });
        }

        public ActionResult Detail(int id)
        {
            using (var db = new box_omsEntities())
            {
                var query = db.box_update_sys_log.Where(w => w.UpdateId == id);

                return View(query.Select(s => new VM_BoxUpdateSys_Detail
                {
                    UTime = s.StartTime,
                    Mac = ""
                }).ToList());
            }
        }

        [HttpPost]
        public JsonResult Publish(int id)
        {
            using (var db = new box_omsEntities())
            {
                box_update_sys dbm = db.box_update_sys.Find(id);
                dbm.IsPublish = true;
                db.SaveChanges();
            }

            return Json(new OMS.Common.Model.JsonData { flag = OMS.Common.Model.JsonDataFlag.Succeed });
        }

        [HttpPost]
        public JsonResult PublishCancel(int id)
        {
            using (var db = new box_omsEntities())
            {
                box_update_sys dbm = db.box_update_sys.Find(id);
                dbm.IsPublish = false;
                db.SaveChanges();
            }

            return Json(new OMS.Common.Model.JsonData { flag = OMS.Common.Model.JsonDataFlag.Succeed });
        }


        #region 保存zip包
        /// <summary>
        /// 保存zip包
        /// </summary>
        /// <returns></returns>
        public string UploadZip(HttpPostedFileBase file, string id)
        {
            string _box = ConfigurationManager.AppSettings["BoxOMS"];
            string upFilePath = Server.MapPath("/Files/Box/Update/");
            string _url = string.Empty;
            if (!Directory.Exists(upFilePath))
            {
                Directory.CreateDirectory(upFilePath);
            }
            string strPath = file.FileName;
            string type = strPath.Substring(strPath.LastIndexOf(".") + 1).ToLower();

            if (!Directory.Exists(upFilePath))
            {
                Directory.CreateDirectory(upFilePath);
            }

            file.SaveAs(upFilePath += id + "." + type);
            _url = _box + "/Files/Box/Update/" + id + "." + type;
            return _url;
        }
        #endregion
    }
}