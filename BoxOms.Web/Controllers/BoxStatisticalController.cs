using BoxOms.Database;
using BoxOms.Web.Models;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BoxOms.Web.Controllers
{
    public class BoxStatisticalController : Controller
    {
        // GET: BoxStatistical
        public ActionResult Index(VM_BoxSta_Index m)
        {
            OperationData(m);
            return View(m);
        }

        public ActionResult AllTotal(VM_BoxSta_TotalIndex m)
        {
            OperationAllData(m);
            return View(m);
        }

        #region 操作数据
        /// <summary>
        /// 操作数据
        /// </summary>
        /// <param name="m"></param>
        private static void OperationData(VM_BoxSta_Index m)
        {
            if (m.grid == null)
            {
                m.grid = new VM_BoxSta_Index_Grid();
            }
            m.grid.Chinese = new ArrayList();
            m.grid.English = new ArrayList();
            m.grid.Math = new ArrayList();
            //查询出所有的盒子
            using (var db = new box_omsEntities())
            {
                var query = db.box_good.OrderBy(o => o.Code).AsQueryable();
                if (m.Prov != null)
                    query = query.Where(_ => _.Prov.Contains(m.Prov.ToString()));
                if (m.City != null)
                    query = query.Where(_ => _.City.Contains(m.City.ToString()));
                if (m.Area != null)
                    query = query.Where(_ => _.Area.Contains(m.Area.ToString()));
                if (m.BoxID != null)
                    query = query.Where(_ => _.BoxId == m.BoxID);
                var _li = db.box_resource_statist.OrderBy(_ => _.CreateDate).ToList();
                if (m.SDate != null && m.EDate != null)
                    _li = _li.Where(_ => _.CreateDate >= m.SDate && _.CreateDate <= m.EDate).ToList();
                else
                {
                    m.SDate = null;
                    m.EDate = null;
                }
                m.list = query.Select(s => new VM_BoxRunStat_Index_Grid
                {
                    BoxId = s.BoxId,
                    SchoolName = s.SchoolName,
                    FirstRunTime = s.FirstRunTime,
                    UseUserName = s.UseUserName,
                    State = s.State
                }).ToList();
                //找出每个学科所对应的版本
                if (m.list.Count > 0 && _li.Count > 0)
                {
                    foreach (var item in m.list)
                    {
                        var _listE = db.box_subject_edition.Where(_ => _.BoxId == item.BoxId).ToList();
                        if (_listE.Count == 0)
                            continue;
                        foreach (var it in _listE)
                        {
                            if (it.Subject == 1 && !m.grid.Chinese.Contains(it.Edition))
                                m.grid.Chinese.Add(it.Edition);
                            if (it.Subject == 2 && !m.grid.Math.Contains(it.Edition))
                                m.grid.Math.Add(it.Edition);
                            if (it.Subject == 3 && !m.grid.English.Contains(it.Edition))
                                m.grid.English.Add(it.Edition);
                        }
                    }

                    var _list_result = from t in _li
                                       from r in m.list
                                       where t.BoxId == r.BoxId
                                       select t;
                    ArrayList _arr = new ArrayList();
                    if (_list_result.Count() > 0)
                    {
                        m.RunTime = _list_result.Max(_ => _.CreateDate).ToString("yyyy-MM-dd");

                        foreach (var item in _list_result)
                        {
                            if (!_arr.Contains(item.CreateDate.ToString("yyyy-MM-dd")))
                                _arr.Add(item.CreateDate.ToString("yyyy-MM-dd"));
                        }

                    }
                    m.total = _arr.Count;
                }
                else
                    m.list = new List<VM_BoxRunStat_Index_Grid>();
            }
        }
        #endregion

        #region 操作所有数据
        /// <summary>
        /// 操作所有数据
        /// </summary>
        /// <param name="m"></param>
        private static void OperationAllData(VM_BoxSta_TotalIndex m)
        {
            if (m.grid == null)
            {
                m.grid = new VM_BoxSta_Index_Grid();
            }
            m.grid.Chinese = new ArrayList();
            m.grid.English = new ArrayList();
            m.grid.Math = new ArrayList();
            //查询出所有的盒子
            using (var db = new box_omsEntities())
            {
                var query = db.box_good.OrderBy(o => o.Code).AsQueryable();
                var _li = db.box_resource_statist.OrderBy(_ => _.CreateDate).ToList();
                if (m.SDate != null && m.EDate != null)
                    _li = _li.Where(_ => _.CreateDate >= m.SDate && _.CreateDate <= m.EDate).ToList();
                else
                {
                    m.SDate = null;
                    m.EDate = null;
                }
                m.list = query.Select(s => new VM_BoxRunStat_Index_Grid
                {
                    BoxId = s.BoxId,
                    SchoolName = s.SchoolName,
                    FirstRunTime = s.FirstRunTime,
                    UseUserName = s.UseUserName,
                    State = s.State
                }).ToList();
                //找出每个学科所对应的版本
                if (m.list.Count > 0 && _li.Count > 0)
                {
                    foreach (var item in m.list)
                    {
                        var _listE = db.box_subject_edition.Where(_ => _.BoxId == item.BoxId).ToList();
                        if (_listE.Count == 0)
                            continue;
                        foreach (var it in _listE)
                        {
                            if (it.Subject == 1 && !m.grid.Chinese.Contains(it.Edition))
                                m.grid.Chinese.Add(it.Edition);
                            if (it.Subject == 2 && !m.grid.Math.Contains(it.Edition))
                                m.grid.Math.Add(it.Edition);
                            if (it.Subject == 3 && !m.grid.English.Contains(it.Edition))
                                m.grid.English.Add(it.Edition);
                        }
                    }
                }
                else
                    m.list = new List<VM_BoxRunStat_Index_Grid>();
            }
        }
        #endregion

        #region 导出数据
        public FileResult ExportData(VM_BoxSta_Index m)
        {
            string _title = "学校数据统计.xls";
            OperationData(m);
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            ICellStyle cellstyle = book.CreateCellStyle();
            cellstyle.VerticalAlignment = VerticalAlignment.Center;
            cellstyle.Alignment = HorizontalAlignment.Center;
            if (m.grid.Chinese.Count != 0 || m.grid.Math.Count != 0 || m.grid.English.Count != 0)
            {
                int _engNum = 0;
                int _matNum = 0;
                int _chiNum = 0;
                if (m.grid.English.Count > 0)
                    _engNum = m.grid.English.Count * 2;
                if (m.grid.Math.Count > 0)
                    _matNum = m.grid.Math.Count * 2;
                if (m.grid.Chinese.Count > 0)
                    _chiNum = m.grid.Chinese.Count * 2;
                //给sheet1添加第一行的头部标题
                NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
                SetCellRangeAddress(sheet1, 0, 0, 0, 0);
                SetStyle(cellstyle, row1, 0, "学科");
                if (_engNum > 0)
                {
                    SetCellRangeAddress(sheet1, 0, 0, 1, _engNum);
                    SetStyle(cellstyle, row1, 1, "英语");
                }
                if (_matNum > 0)
                {
                    SetCellRangeAddress(sheet1, 0, 0, _engNum + 1, _engNum + _matNum);
                    SetStyle(cellstyle, row1, _engNum + 1, "数学");
                }
                if (_chiNum > 0)
                {
                    SetCellRangeAddress(sheet1, 0, 0, _engNum + _matNum + 1, _engNum + _matNum + _chiNum);
                    SetStyle(cellstyle, row1, _engNum + _matNum + 1, "语文");
                }

                NPOI.SS.UserModel.IRow row2 = sheet1.CreateRow(1);
                SetCellRangeAddress(sheet1, 1, 1, 0, 0);
                SetStyle(cellstyle, row2, 0, "版本");
                if (m.grid.English.Count > 0)
                {
                    for (int i = 0; i < m.grid.English.Count; i++)
                    {
                        SetCellRangeAddress(sheet1, 1, 1, i * 2 + 1, i * 2 + 2);
                        SetStyle(cellstyle, row2, i * 2 + 1, BoxOms.Dict.Edition.GetVal(Convert.ToInt32(m.grid.English[i].ToString())));
                    }
                }
                if (m.grid.Math.Count > 0)
                {
                    for (int i = 0; i < m.grid.Math.Count; i++)
                    {
                        SetCellRangeAddress(sheet1, 1, 1, _engNum + i * 2 + 1, _engNum + i * 2 + 2);
                        SetStyle(cellstyle, row2, _engNum + i * 2 + 1, BoxOms.Dict.Edition.GetVal(Convert.ToInt32(m.grid.Math[i].ToString())));
                    }
                }
                if (m.grid.Chinese.Count > 0)
                {
                    for (int i = 0; i < m.grid.Chinese.Count; i++)
                    {
                        SetCellRangeAddress(sheet1, 1, 1, _engNum + _matNum + i * 2 + 1, _engNum + _matNum + i * 2 + 2);
                        SetStyle(cellstyle, row2, _engNum + _matNum + i * 2 + 1, BoxOms.Dict.Edition.GetVal(Convert.ToInt32(m.grid.Chinese[i].ToString())));
                    }
                }
                // Dictionary<int, string> dict = BoxOms.Dict.Course.Get();
                for (int i = 0; i < 13; i++)
                {
                    NPOI.SS.UserModel.IRow row3 = sheet1.CreateRow(i + 2);
                    if (i == 0)
                    {
                        SetCellRangeAddress(sheet1, 2, 1 + 13, 0, 0);
                        SetStyle(cellstyle, row3, 0, "模块");
                    }
                    if (m.grid.English.Count > 0)
                    {
                        for (int j = 0; j < m.grid.English.Count; j++)
                        {
                            string _typeName = BoxOms.Web.BLL.BoxStaBLL.BackTypeName(3, Convert.ToInt32(m.grid.English[j]))[i];
                            row3.CreateCell(j * 2 + 1).SetCellValue(_typeName);
                            row3.CreateCell(j * 2 + 2).SetCellValue(string.IsNullOrEmpty(_typeName) ? "" : BoxOms.Web.BLL.BoxStaBLL.BackNum(m.list, 3, Convert.ToInt32(m.grid.English[j]), _typeName).ToString());
                        }
                    }
                    if (m.grid.Math.Count > 0)
                    {
                        for (int j = 0; j < m.grid.Math.Count; j++)
                        {
                            string _typeName = BoxOms.Web.BLL.BoxStaBLL.BackTypeName(2, Convert.ToInt32(m.grid.Math[j]))[i];
                            row3.CreateCell(j * 2 + _engNum + 1).SetCellValue(_typeName);
                            row3.CreateCell(j * 2 + _engNum + 2).SetCellValue(string.IsNullOrEmpty(_typeName) ? "" : BoxOms.Web.BLL.BoxStaBLL.BackNum(m.list, 2, Convert.ToInt32(m.grid.Math[j]), i).ToString());
                        }

                    }
                    if (m.grid.Chinese.Count > 0)
                    {
                        for (int j = 0; j < m.grid.Chinese.Count; j++)
                        {
                            string _typeName = BoxOms.Web.BLL.BoxStaBLL.BackTypeName(1, Convert.ToInt32(m.grid.Chinese[j]))[i];
                            row3.CreateCell(j * 2 + _engNum + _matNum + 1).SetCellValue(_typeName);
                            row3.CreateCell(j * 2 + _engNum + _matNum + 2).SetCellValue(string.IsNullOrEmpty(_typeName) ? "" : BoxOms.Web.BLL.BoxStaBLL.BackNum(m.list, 1, Convert.ToInt32(m.grid.Chinese[j]), i).ToString());
                        }

                    }
                }
                NPOI.SS.UserModel.IRow row4 = sheet1.CreateRow(2 + 13);
                row4.CreateCell(0).SetCellValue("版本小计");
                if (m.grid.English.Count > 0)
                {
                    for (int j = 0; j < m.grid.English.Count; j++)
                    {
                        row4.CreateCell(j * 2 + 1).SetCellValue("");
                        row4.CreateCell(j * 2 + 2).SetCellValue(BoxOms.Web.BLL.BoxStaBLL.BackNumTotal(m.list, 3, Convert.ToInt32(m.grid.English[j])));
                    }
                }
                if (m.grid.Math.Count > 0)
                {
                    for (int j = 0; j < m.grid.Math.Count; j++)
                    {
                        row4.CreateCell(j * 2 + _engNum + 1).SetCellValue("");
                        row4.CreateCell(j * 2 + _engNum + 2).SetCellValue(BoxOms.Web.BLL.BoxStaBLL.BackNumTotal(m.list, 2, Convert.ToInt32(m.grid.Math[j])));
                    }

                }
                if (m.grid.Chinese.Count > 0)
                {
                    for (int j = 0; j < m.grid.Chinese.Count; j++)
                    {
                        row4.CreateCell(j * 2 + _engNum + _matNum + 1).SetCellValue("");
                        row4.CreateCell(j * 2 + _engNum + _matNum + 2).SetCellValue(BoxOms.Web.BLL.BoxStaBLL.BackNumTotal(m.list, 1, Convert.ToInt32(m.grid.Chinese[j])));
                    }

                }
                NPOI.SS.UserModel.IRow row5 = sheet1.CreateRow(3 + 13);
                row5.CreateCell(0).SetCellValue("最近回执");
                SetCellRangeAddress(sheet1, 3 + 13, 3 + 13, 1, _engNum + _matNum + _chiNum);
                SetStyle(cellstyle, row5, 1, m.RunTime);
                NPOI.SS.UserModel.IRow row6 = sheet1.CreateRow(4 + 13);
                row6.CreateCell(0).SetCellValue("回执次数");
                SetCellRangeAddress(sheet1, 4 + 13, 4 + 13, 1, _engNum + _matNum + _chiNum);
                SetStyle(cellstyle, row6, 1, m.total.ToString());
            }
            else
            {
                //给sheet1添加第一行的头部标题
                NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
                row1.CreateCell(0).SetCellValue("学科");
                row1.CreateCell(1).SetCellValue("英语");
                row1.CreateCell(2).SetCellValue("数学");
                row1.CreateCell(3).SetCellValue("语文");
            }

            //// 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", _title);
        }

        #endregion

        #region 导出所有数据
        public FileResult ExportAllData(VM_BoxSta_TotalIndex m)
        {
            string _title = "总体数据统计.xls";
            OperationAllData(m);
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            ICellStyle cellstyle = book.CreateCellStyle();
            cellstyle.VerticalAlignment = VerticalAlignment.Center;
            cellstyle.Alignment = HorizontalAlignment.Center;
            if (m.grid.Chinese.Count != 0 || m.grid.Math.Count != 0 || m.grid.English.Count != 0)
            {
                int _engNum = 0;
                int _matNum = 0;
                int _chiNum = 0;
                if (m.grid.English.Count > 0)
                    _engNum = m.grid.English.Count;
                if (m.grid.Math.Count > 0)
                    _matNum = m.grid.Math.Count;
                if (m.grid.Chinese.Count > 0)
                    _chiNum = m.grid.Chinese.Count;
                //给sheet1添加第一行的头部标题
                NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
                row1.CreateCell(0).SetCellValue("学科");
                row1.CreateCell(1).SetCellValue("版本");
                Dictionary<int, string> dict = BoxOms.Dict.Course.Get();
                for (int i = 0; i < dict.Count; i++)
                {
                    row1.CreateCell(2 + i).SetCellValue(dict[i].ToString());
                }
                row1.CreateCell(2 + dict.Count).SetCellValue("版本小计");
                row1.CreateCell(3 + dict.Count).SetCellValue("学科小计");

                if (m.grid.English.Count > 0)
                {
                    for (int i = 0; i < m.grid.English.Count; i++)
                    {
                        NPOI.SS.UserModel.IRow row2 = sheet1.CreateRow(i + 1);
                        if (i == 0)
                        {
                            SetCellRangeAddress(sheet1, 1, _engNum, 0, 0);
                            SetCellRangeAddress(sheet1, 1, _engNum, 3 + dict.Count, 3 + dict.Count);
                            SetStyle(cellstyle, row2, 0, "英语");
                            SetStyle(cellstyle, row2, 3 + dict.Count, BoxOms.Web.BLL.BoxStaBLL.BackNumTotal(m.list, 3).ToString());
                        }
                        row2.CreateCell(1).SetCellValue(BoxOms.Dict.Edition.GetVal(Convert.ToInt32(m.grid.English[i])));
                        for (int j = 0; j < dict.Count; j++)
                        {
                            row2.CreateCell(j + 2).SetCellValue(BoxOms.Web.BLL.BoxStaBLL.BackNum(m.list, 3, Convert.ToInt32(m.grid.English[i]), j));
                        }
                        row2.CreateCell(2 + dict.Count).SetCellValue(BoxOms.Web.BLL.BoxStaBLL.BackNumTotal(m.list, 3, Convert.ToInt32(m.grid.English[i])));
                    }
                }
                if (m.grid.Math.Count > 0)
                {
                    for (int i = 0; i < m.grid.Math.Count; i++)
                    {
                        NPOI.SS.UserModel.IRow row2 = sheet1.CreateRow(i + 1 + _engNum);
                        if (i == 0)
                        {
                            SetCellRangeAddress(sheet1, _engNum + 1, _engNum + _matNum, 0, 0);
                            SetCellRangeAddress(sheet1, _engNum + 1, _engNum + _matNum, 3 + dict.Count, 3 + dict.Count);
                            SetStyle(cellstyle, row2, 0, "数学");
                            SetStyle(cellstyle, row2, 3 + dict.Count, BoxOms.Web.BLL.BoxStaBLL.BackNumTotal(m.list, 2).ToString());
                        }
                        row2.CreateCell(1).SetCellValue(BoxOms.Dict.Edition.GetVal(Convert.ToInt32(m.grid.Math[i])));
                        for (int j = 0; j < dict.Count; j++)
                        {
                            row2.CreateCell(j + 2).SetCellValue(BoxOms.Web.BLL.BoxStaBLL.BackNum(m.list, 2, Convert.ToInt32(m.grid.Math[i]), j));
                        }
                        row2.CreateCell(2 + dict.Count).SetCellValue(BoxOms.Web.BLL.BoxStaBLL.BackNumTotal(m.list, 2, Convert.ToInt32(m.grid.Math[i])));
                    }
                }
                if (m.grid.Chinese.Count > 0)
                {
                    for (int i = 0; i < m.grid.Chinese.Count; i++)
                    {
                        NPOI.SS.UserModel.IRow row2 = sheet1.CreateRow(i + 1 + _engNum + _matNum);
                        if (i == 0)
                        {
                            SetCellRangeAddress(sheet1, _engNum + _matNum + 1, _engNum + _matNum + _chiNum, 0, 0);
                            SetCellRangeAddress(sheet1, _engNum + _matNum + 1, _engNum + _matNum + _chiNum, 3 + dict.Count, 3 + dict.Count);
                            SetStyle(cellstyle, row2, 0, "语文");
                            SetStyle(cellstyle, row2, 3 + dict.Count, BoxOms.Web.BLL.BoxStaBLL.BackNumTotal(m.list, 1).ToString());
                        }
                        row2.CreateCell(1).SetCellValue(BoxOms.Dict.Edition.GetVal(Convert.ToInt32(m.grid.Chinese[i])));
                        for (int j = 0; j < dict.Count; j++)
                        {
                            row2.CreateCell(j + 2).SetCellValue(BoxOms.Web.BLL.BoxStaBLL.BackNum(m.list, 1, Convert.ToInt32(m.grid.Chinese[i]), j));
                        }
                        row2.CreateCell(2 + dict.Count).SetCellValue(BoxOms.Web.BLL.BoxStaBLL.BackNumTotal(m.list, 1, Convert.ToInt32(m.grid.Chinese[i])));
                    }
                }
            }
            else
            {
                //给sheet1添加第一行的头部标题
                NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
                row1.CreateCell(0).SetCellValue("学科");
                row1.CreateCell(1).SetCellValue("版本");
                Dictionary<int, string> dict = BoxOms.Dict.Course.Get();
                for (int i = 0; i < dict.Count; i++)
                {
                    row1.CreateCell(2 + i).SetCellValue(dict[i].ToString());
                }
                row1.CreateCell(2 + dict.Count).SetCellValue("版本小计");
                row1.CreateCell(3 + dict.Count).SetCellValue("学科小计");
            }

            //// 写入到客户端 
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

    }
}