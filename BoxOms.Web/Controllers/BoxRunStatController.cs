using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BoxOms.Web.Models;
using BoxOms.Database;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace BoxOms.Web.Controllers
{
    public class BoxRunStatController : Controller
    {
        // GET: BoxRunStat
        #region 列表页
        public ActionResult Index(VM_BoxRunStat_Index m)
        {
            if (m.Grid == null)
            {
                m.Grid = new OMS.Common.Model.PList<VM_BoxRunStat_Index_Grid>();
                m.Grid.Pager = new OMS.Common.Model.Pager();
            }

            using (var db = new box_omsEntities())
            {
                var query = db.box_good.OrderByDescending(o => o.BoxId).AsQueryable();

                if (!string.IsNullOrWhiteSpace(m.key))
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(m.key, @"^[0-9]+$"))
                    {
                        int _id = Convert.ToInt32(m.key);
                        query = query.Where(w => w.BoxId == _id);
                    }
                    else
                        query = query.Where(w => w.SchoolName.Contains(m.key) || w.UseUserName.Contains(m.key));
                }

                if (!string.IsNullOrWhiteSpace(m.State))
                {
                    int _state = Convert.ToInt32(m.State);
                    query = query.Where(w => w.State == _state);
                }
                m.Grid.Pager = new OMS.Common.Model.Pager(m.Grid.Pager.PageIndex, m.Grid.Pager.PageSize, query.Count());
                m.Grid.Data = query.Skip((m.Grid.Pager.PageIndex - 1) * m.Grid.Pager.PageSize).Take(m.Grid.Pager.PageSize).Select(s => new VM_BoxRunStat_Index_Grid
                {
                    BoxId = s.BoxId,
                    SchoolName = s.SchoolName,
                    FirstRunTime = s.FirstRunTime,
                    UseUserName = s.UseUserName,
                    State = s.State
                }).ToList();

                if (m.Grid.Data.Count > 0)
                {
                    foreach (var item in m.Grid.Data)
                    {
                        item.Prov = BoxOms.Web.BLL.BoxGoodBLL.BackPrName(item.BoxId);
                        item.Chinese = BoxOms.Web.BLL.BoxGoodBLL.BackEditionName(item.BoxId, 1);
                        item.Math = BoxOms.Web.BLL.BoxGoodBLL.BackEditionName(item.BoxId, 2);
                        item.English = BoxOms.Web.BLL.BoxGoodBLL.BackEditionName(item.BoxId, 3);
                    }
                }
            }

            return View(m);
        }
        #endregion

        public ActionResult Index2(VM_BoxRunStat_Index2 m)
        {
            using (var db = new box_omsEntities())
            {
                var query = db.box_resource_statist_day.OrderBy(o => o.Subject).ThenBy(t => t.Edition).AsQueryable();
                if (m.SDate != null)
                {
                    query = query.Where(w => System.Data.Entity.DbFunctions.TruncateTime(w.SDate) >= m.SDate).AsQueryable();
                }
                if (m.EDate != null)
                {
                    query = query.Where(w => System.Data.Entity.DbFunctions.TruncateTime(w.SDate) <= m.EDate).AsQueryable();
                }

                m.Grid = query.GroupBy(g => new { g.Subject, g.Edition }).Select(s => new VM_BoxRunStat_Index2_Grid
                {
                    Subject = s.Key.Subject,
                    Edition = s.Key.Edition,
                    f0 = s.Sum(item => item.f0),
                    f1 = s.Sum(item => item.f1),
                    f2 = s.Sum(item => item.f2),
                    f3 = s.Sum(item => item.f3),
                    f4 = s.Sum(item => item.f4),
                    f5 = s.Sum(item => item.f5),
                    f6 = s.Sum(item => item.f6),
                    f7 = s.Sum(item => item.f7),
                    f8 = s.Sum(item => item.f8),
                    f9 = s.Sum(item => item.f9),
                    f10 = s.Sum(item => item.f10),
                    f11 = s.Sum(item => item.f11),
                    f12 = s.Sum(item => item.f12),
                    SubjectTotal = s.Sum(item => item.SubjectTotal),
                    EditionTotal = s.Sum(item => item.EditionTotal),
                }).ToList();
            }

            return View(m);
        }

        #region 编辑
        public ActionResult Edit(int id)
        {
            VM_BoxGood_Form1 m = new VM_BoxGood_Form1();

            using (var db = new box_omsEntities())
            {
                var dbm = db.box_good.Find(id);
                m.id = dbm.BoxId;
                m.State = dbm.State;
                m.Prov = Convert.ToInt32(dbm.Prov);
                m.City = Convert.ToInt32(dbm.City);
                m.Area = Convert.ToInt32(dbm.Area);
                m.UseUserName = dbm.UseUserName;
                m.SchoolName = dbm.SchoolName;
                m.Chinese = BoxOms.Web.BLL.BoxGoodBLL.BackEditionid(dbm.BoxId, 1);
                m.Math = BoxOms.Web.BLL.BoxGoodBLL.BackEditionid(dbm.BoxId, 2);
                m.English = BoxOms.Web.BLL.BoxGoodBLL.BackEditionid(dbm.BoxId, 3);
            }

            return View(m);
        }

        [HttpPost]
        public ActionResult Edit(VM_BoxGood_Form1 m)
        {
            if (ModelState.IsValid)
            {
                if (m.UseUserName.Length > 10)
                {
                    ModelState.AddModelError("UseUserName", "输入的字符不能超过10个！");
                    return View(m);
                }
                if (m.SchoolName.Length > 20)
                {
                    ModelState.AddModelError("SchoolName", "输入的字符不能超过20个！");
                    return View(m);
                }
                using (var db = new box_omsEntities())
                {
                    box_good dbm = db.box_good.Find(m.id);
                    dbm.UpdateTime = DateTime.Now;
                    dbm.UpdateUserId = System.Web.HttpContext.Current.Session["UserInfo"] == null ? 0 : (System.Web.HttpContext.Current.Session["UserInfo"] as VM_SyPassport_UserInfo).Id;
                    dbm.State = m.State;
                    dbm.Prov = m.Prov.ToString();
                    dbm.City = m.City.ToString();
                    dbm.Area = m.Area.ToString();
                    dbm.UseUserName = m.UseUserName;
                    dbm.SchoolName = m.SchoolName;
                    //先删除然后重新添加
                    db.box_subject_edition.RemoveRange(db.box_subject_edition.Where(_ => _.BoxId == m.id));
                    Bind(m.English, db, dbm.BoxId, 3);
                    Bind(m.Math, db, dbm.BoxId, 2);
                    Bind(m.Chinese, db, dbm.BoxId, 1);
                    db.SaveChanges();

                    return Json(new { success = true });

                }
            }

            return View(m);
        }
        #endregion

        #region 绑定盒子与教材、版本的关系
        /// <summary>
        /// 绑定盒子与教材、版本的关系
        /// </summary>
        /// <param name="val"></param>
        /// <param name="db"></param>
        /// <param name="BoxId"></param>
        /// <param name="Subject"></param>
        private static void Bind(string val, box_omsEntities db, int BoxId, int Subject)
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
                    db.box_subject_edition.Add(new box_subject_edition()
                    {
                        Id = Guid.NewGuid().ToString(),
                        BoxId = BoxId,
                        Subject = Subject,
                        Edition = Convert.ToInt32(_ids[i])
                    });
                }
            }
        }
        #endregion

        #region 导出数据
        public FileResult ExportData(string state, string key)
        {
            string _title = "运行管理.xls";
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            ICellStyle cellstyle = book.CreateCellStyle();
            cellstyle.VerticalAlignment = VerticalAlignment.Center;
            cellstyle.Alignment = HorizontalAlignment.Center;
            SetCellRangeAddress(sheet1, 0, 1, 0, 0);
            SetCellRangeAddress(sheet1, 0, 1, 1, 1);
            SetCellRangeAddress(sheet1, 0, 1, 2, 2);
            SetCellRangeAddress(sheet1, 0, 1, 3, 3);
            SetCellRangeAddress(sheet1, 0, 1, 4, 4);
            SetCellRangeAddress(sheet1, 0, 1, 5, 5);
            SetCellRangeAddress(sheet1, 0, 0, 6, 8);
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            NPOI.SS.UserModel.IRow row2 = sheet1.CreateRow(1);
            SetStyle(cellstyle, row1, 0, "盒子编号");
            SetStyle(cellstyle, row1, 1, "状态");
            SetStyle(cellstyle, row1, 2, "省\\直辖市");
            SetStyle(cellstyle, row1, 3, "领用人");
            SetStyle(cellstyle, row1, 4, "学校");
            SetStyle(cellstyle, row1, 5, "首次回执");
            SetStyle(cellstyle, row1, 6, "版本");
            SetStyle(cellstyle, row2, 6, "英语");
            SetStyle(cellstyle, row2, 7, "数学");
            SetStyle(cellstyle, row2, 8, "语文");

            List<VM_BoxRunStat_Index_Grid> _list = new List<VM_BoxRunStat_Index_Grid>();

            using (var db = new box_omsEntities())
            {
                var query = db.box_good.OrderBy(o => o.Code).AsQueryable();

                if (!string.IsNullOrWhiteSpace(key))
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(key, @"^[0-9]+$"))
                    {
                        int _id = Convert.ToInt32(key);
                        query = query.Where(w => w.BoxId == _id);
                    }
                    else
                        query = query.Where(w => w.SchoolName.Contains(key) || w.UseUserName.Contains(key));
                }

                if (!string.IsNullOrWhiteSpace(state))
                {
                    int _state = Convert.ToInt32(state);
                    query = query.Where(w => w.State == _state);
                }
                _list = query.Select(s => new VM_BoxRunStat_Index_Grid
                {
                    BoxId = s.BoxId,
                    SchoolName = s.SchoolName,
                    FirstRunTime = s.FirstRunTime,
                    UseUserName = s.UseUserName,
                    State = s.State
                }).ToList();
            }
            if (_list.Count > 0)
            {
                for (int i = 0; i < _list.Count; i++)
                {
                    NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 2);
                    string _state = string.Empty;
                    switch (_list[i].State)
                    {
                        case 1:
                            _state = "库存";
                            break;
                        case 2:
                            _state = "已售";
                            break;
                        case 3:
                            _state = "领用";
                            break;
                    }
                    string _FirstRunTime = _list[i].FirstRunTime == null ? "" : ((DateTime)_list[i].FirstRunTime).ToString("yyyy-MM-dd HH:mm:ss");
                    rowtemp.CreateCell(0).SetCellValue(_list[i].BoxId);
                    rowtemp.CreateCell(1).SetCellValue(_state);
                    rowtemp.CreateCell(2).SetCellValue(BoxOms.Web.BLL.BoxGoodBLL.BackPrName(_list[i].BoxId));
                    rowtemp.CreateCell(3).SetCellValue(_list[i].UseUserName);
                    rowtemp.CreateCell(4).SetCellValue(_list[i].SchoolName);
                    rowtemp.CreateCell(5).SetCellValue(_FirstRunTime);
                    rowtemp.CreateCell(6).SetCellValue(BoxOms.Web.BLL.BoxGoodBLL.BackEditionName(_list[i].BoxId, 3));
                    rowtemp.CreateCell(7).SetCellValue(BoxOms.Web.BLL.BoxGoodBLL.BackEditionName(_list[i].BoxId, 2));
                    rowtemp.CreateCell(8).SetCellValue(BoxOms.Web.BLL.BoxGoodBLL.BackEditionName(_list[i].BoxId, 1));
                }
            }

            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", _title);
        }

        #endregion

        #region 合并单元格
        /// <summary>
        /// 合并单元格
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rowstart">开始行</param>
        /// <param name="rowend">结束行</param>
        /// <param name="colstart">开始列</param>
        /// <param name="colend">结束列</param>
        public static void SetCellRangeAddress(ISheet sheet, int rowstart, int rowend, int colstart, int colend)
        {
            CellRangeAddress cellRangeAddress = new CellRangeAddress(rowstart, rowend, colstart, colend);
            sheet.AddMergedRegion(cellRangeAddress);
        }
        #endregion

        #region 给单元格设置样式
        /// <summary>
        /// 给单元格设置样式
        /// </summary>
        /// <param name="cellstyle"></param>
        /// <param name="row1">第几行</param>
        /// <param name="num">第几列</param>
        /// <param name="title">所要显示的内容</param>
        private static void SetStyle(ICellStyle cellstyle, IRow row1, int num, string title)
        {
            ICell cell = row1.CreateCell(num);
            cell.SetCellValue(title);
            cell.CellStyle = cellstyle;
        }
        #endregion

        #region 省市区校级联
        /// <summary>
        /// 根据省份ID获取城市
        /// </summary>
        /// <param name="pid">省份ID</param>
        /// <returns></returns>
        public JsonResult GetCitys(int? pid)
        {
            using (var db = new box_omsEntities())
            {
                List<SelectListItem> items = db.oms_district.Where(w => w.ParentID == pid && w.Deleted == 0).Select(s => new SelectListItem { Text = s.CodeName, Value = s.ID.ToString() }).ToList();
                return Json(items, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 根据城市ID获取区
        /// </summary>
        /// <param name="cid">城市ID</param>
        /// <returns></returns>
        public JsonResult GetAreas(int? cid)
        {
            using (var db = new box_omsEntities())
            {
                List<SelectListItem> items = db.oms_district.Where(w => w.ParentID == cid && w.Deleted == 0).Select(s => new SelectListItem { Text = s.CodeName, Value = s.ID.ToString() }).ToList();
                return Json(items, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}