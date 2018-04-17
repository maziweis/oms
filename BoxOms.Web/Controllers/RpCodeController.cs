using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BoxOms.Web.Models;
using BoxOms.Database;

namespace BoxOms.Web.Controllers
{
    public class RpCodeController : BaseController
    {
        #region 首页
        public ActionResult Index(VM_RpCode_Index m)
        {
            if (m.Grid == null)
            {
                m.Grid = new OMS.Common.Model.PList<VM_RpCode_Index_Grid>();
                m.Grid.Pager = new OMS.Common.Model.Pager();
            }

            using (var db = new box_omsEntities())
            {
                var query1 = new List<rp_cdkey>();
                var query = db.rp_cdkey.OrderBy(o => o.Id).ToList();

                if (m.Prov != null)
                    query = query.Where(_ => _.UseProv == m.Prov).ToList();
                if (m.City != null)
                    query = query.Where(_ => _.UseCity == m.City).ToList();
                if (m.Area != null)
                    query = query.Where(_ => _.UseDist == m.Area).ToList();

                if (!string.IsNullOrWhiteSpace(m.Key))
                {
                    var _listRP = db.rp_enterprise.ToList();
                    if (_listRP.Count > 0)
                    {
                        var _mrp = _listRP.Where(_ => _.EntName.Contains(m.Key)).ToList();
                        if (_mrp.Count > 0)
                        {
                            query1 = (from t in query
                                      from r in _mrp
                                      where t.EntId == r.EntId
                                      select t).ToList();
                        }
                    }
                    if (query1.Count > 0)
                        query = query1;
                }

                m.Grid.Pager = new OMS.Common.Model.Pager(m.Grid.Pager.PageIndex, m.Grid.Pager.PageSize, query.Count());
                m.Grid.Data = query.Skip((m.Grid.Pager.PageIndex - 1) * m.Grid.Pager.PageSize).Take(m.Grid.Pager.PageSize).Select(s => new VM_RpCode_Index_Grid
                {
                    Id = s.Id,
                    EnteName = s.rp_enterprise.EntName,
                    EntId = s.EntId,
                    UseSchool = s.UseSchool,
                    Validity = s.Validity,
                    ActiveTime = s.ActiveTime,
                    Remark = s.Remark,
                    ActiveCode = s.ActiveCode,
                    AuthUserCount = s.AuthUserCount,
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
        public ActionResult Add(VM_RpCode_Form m)
        {
            if (ModelState.IsValid)
            {
                using (var db = new box_omsEntities())
                {
                    int? _Validity = null;
                    int? _AuthUserCount = null;
                    if (m.Validity != null)
                    {
                        if (!System.Text.RegularExpressions.Regex.IsMatch(m.Validity.ToString(), @"^(([1-9]\d*))$"))
                        {
                            ModelState.AddModelError("Validity", "请输入大于0的正整数！");
                            return View(m);
                        }
                        _Validity = Convert.ToInt32(m.Validity);
                    }
                    if (m.AuthUserCount != null)
                    {
                        if (!System.Text.RegularExpressions.Regex.IsMatch(m.AuthUserCount.ToString(), @"^(([1-9]\d*))$"))
                        {
                            ModelState.AddModelError("AuthUserCount", "请输入大于0的正整数！");
                            return View(m);
                        }
                        _AuthUserCount = Convert.ToInt32(m.AuthUserCount);
                    }
                    string _id = Guid.NewGuid().ToString();
                    db.rp_cdkey.Add(new rp_cdkey
                    {
                        EntId = m.EnteId,
                        Id = _id,
                        Validity = _Validity,
                        AuthUserCount = _AuthUserCount,
                        UseProv = m.Provinces,
                        UseCity = m.Cityss,
                        UseDist = m.Areass,
                        Remark = m.Remark,
                        UseSchool = m.UseSchool
                    });
                    Bind(m.English, db, _id, 3);
                    Bind(m.Math, db, _id, 2);
                    Bind(m.Chinese, db, _id, 1);
                    db.SaveChanges();

                    return Json(new { success = true });
                }
            }

            return View(m);
        }
        #endregion

        #region 编辑
        public ActionResult Edit(string id)
        {
            VM_RpCode_Form m = new VM_RpCode_Form();

            using (var db = new box_omsEntities())
            {
                var dbm = db.rp_cdkey.Where(_ => _.Id == id).FirstOrDefault();
                m.Id = dbm.Id;
                m.EnteId = dbm.EntId;
                m.Validity = dbm.Validity == null ? "" : dbm.Validity.ToString();
                m.AuthUserCount = dbm.AuthUserCount == null ? "" : dbm.AuthUserCount.ToString();
                m.Provinces = dbm.UseProv;
                m.Cityss = dbm.UseCity;
                m.Areass = dbm.UseDist;
                m.Remark = dbm.Remark;
                m.UseSchool = dbm.UseSchool;
                m.Chinese = BoxOms.Web.BLL.Rp_Code.BackEditionid(m.Id, 1);
                m.Math = BoxOms.Web.BLL.Rp_Code.BackEditionid(m.Id, 2);
                m.English = BoxOms.Web.BLL.Rp_Code.BackEditionid(m.Id, 3);
            }

            return View(m);
        }

        [HttpPost]
        public ActionResult Edit(VM_RpCode_Form m)
        {
            if (ModelState.IsValid)
            {
                using (var db = new box_omsEntities())
                {
                    int? _Validity = null;
                    int? _AuthUserCount = null;
                    if (m.Validity != null)
                    {
                        if (!System.Text.RegularExpressions.Regex.IsMatch(m.Validity.ToString(), @"^(([1-9]\d*))$"))
                        {
                            ModelState.AddModelError("Validity", "请输入大于0的正整数！");
                            return View(m);
                        }
                        _Validity = Convert.ToInt32(m.Validity);
                    }
                    if (m.AuthUserCount != null)
                    {
                        if (!System.Text.RegularExpressions.Regex.IsMatch(m.AuthUserCount.ToString(), @"^(([1-9]\d*))$"))
                        {
                            ModelState.AddModelError("AuthUserCount", "请输入大于0的正整数！");
                            return View(m);
                        }
                        _AuthUserCount = Convert.ToInt32(m.AuthUserCount);
                    }
                    rp_cdkey rpc = db.rp_cdkey.Where(_ => _.Id == m.Id).FirstOrDefault();

                    rpc.EntId = m.EnteId;
                    rpc.Validity = _Validity;
                    rpc.AuthUserCount = _AuthUserCount;
                    rpc.UseProv = m.Provinces;
                    rpc.UseCity = m.Cityss;
                    rpc.UseDist = m.Areass;
                    rpc.Remark = m.Remark;
                    rpc.UseSchool = m.UseSchool;
                    //先删除然后重新添加
                    db.rp_cdkey_and_edition.RemoveRange(db.rp_cdkey_and_edition.Where(_ => _.CdKey == m.Id));
                    Bind(m.English, db, m.Id, 3);
                    Bind(m.Math, db, m.Id, 2);
                    Bind(m.Chinese, db, m.Id, 1);
                    db.SaveChanges();

                    return Json(new { success = true });
                }
            }

            return View(m);
        }
        #endregion

        #region 删除
        [HttpPost]
        public JsonResult Delete(string id)
        {
            using (var db = new box_omsEntities())
            {
                db.rp_cdkey_and_edition.RemoveRange(db.rp_cdkey_and_edition.Where(_ => _.CdKey == id));
                db.rp_cdkey.Remove(db.rp_cdkey.Where(_ => _.Id == id).FirstOrDefault());
                db.SaveChanges();
            }

            return Json(new OMS.Common.Model.JsonData { flag = OMS.Common.Model.JsonDataFlag.Succeed });
        }
        #endregion

        #region 查看
        public ActionResult Look(string id)
        {
            VM_RpCode_Index_Grid _m = new VM_RpCode_Index_Grid();
            using (var db = new box_omsEntities())
            {
                var dbm = db.rp_cdkey.Where(_ => _.Id == id).FirstOrDefault();
                _m.Id = dbm.Id;
                _m.EnteName = dbm.rp_enterprise.EntName;
                _m.EntId = dbm.EntId;
                _m.UseSchool = dbm.UseSchool;
                _m.Validity = dbm.Validity;
                _m.ActiveTime = dbm.ActiveTime;
                _m.Remark = dbm.Remark;
                _m.ActiveCode = dbm.ActiveCode;
                _m.AuthUserCount = dbm.AuthUserCount;
                _m.ActiveMac = dbm.ActiveMac;
                _m.ActiveIp = dbm.ActiveIp;
                _m.ActiveDisk = dbm.ActiveDisk;
                ViewBag.Chinese = BoxOms.Web.BLL.Rp_Code.BackEditionName(_m.Id, 1);
                ViewBag.Math = BoxOms.Web.BLL.Rp_Code.BackEditionName(_m.Id, 2);
                ViewBag.English = BoxOms.Web.BLL.Rp_Code.BackEditionName(_m.Id, 3);
            }

            return View(_m);
        }
        #endregion

        #region 绑定教材、版本的关系
        /// <summary>
        /// 绑定教材、版本的关系
        /// </summary>
        /// <param name="val"></param>
        /// <param name="db"></param>
        /// <param name="BoxId"></param>
        /// <param name="Subject"></param>
        private static void Bind(string val, box_omsEntities db, string cdId, int Subject)
        {
            if (!string.IsNullOrWhiteSpace(val))
            {
                string[] _ids = val.Split(',');
                for (int i = 0; i < _ids.Length; i++)
                {
                    if (string.IsNullOrEmpty(_ids[i]))
                        continue;
                    if (string.IsNullOrWhiteSpace(BoxOms.Dict.Edition.GetVal(Convert.ToInt32(_ids[i]))))
                        continue;
                    db.rp_cdkey_and_edition.Add(new rp_cdkey_and_edition()
                    {
                        Id = Guid.NewGuid().ToString(),
                        CdKey = cdId,
                        Subject = Subject,
                        Edition = Convert.ToInt32(_ids[i])
                    });
                }
            }
        }
        #endregion
    }
}