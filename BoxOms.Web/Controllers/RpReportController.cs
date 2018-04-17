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
    public class RpReportController : Controller
    {
        /// <summary>
        /// 报表首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        #region 用户登录
        /// <summary>
        /// 用户登录情况分析表
        /// </summary>
        /// <returns></returns>
        public ActionResult UserLoginInfo(VM_UserLogin_Index m)
        {
            UserLoginData(m);

            return View(m);
        }

        #region 用户登录数据
        /// <summary>
        /// 用户登录数据
        /// </summary>
        /// <param name="m"></param>
        private static void UserLoginData(VM_UserLogin_Index m)
        {

            List<rp_log_login> _lrll = new List<rp_log_login>();

            if (m.Grid == null)
            {
                m.Grid = new OMS.Common.Model.PList<VM_UserLogin_Index_Grid>();
                m.Grid.Pager = new OMS.Common.Model.Pager();
            }

            using (var db = new box_omsEntities())
            {
                ArrayList _arrl = new ArrayList();
                var query = db.rp_log_login.OrderByDescending(o => o.LoginTime).AsQueryable().ToList();

                if (m.UseProv != null)
                    query = db.rp_log_login.Where(_ => _.rp_cdkey.UseProv == m.UseProv).ToList();
                if (m.UseCity != null)
                    query = db.rp_log_login.Where(_ => _.rp_cdkey.UseCity == m.UseCity).ToList();
                if (m.UseDist != null)
                    query = db.rp_log_login.Where(_ => _.rp_cdkey.UseDist == m.UseDist).ToList();
                if (m.keyid != null)
                    query = db.rp_log_login.Where(_ => _.rp_cdkey.Id == m.keyid).ToList();


                query.ForEach(_ =>
                {
                    if (!_arrl.Contains(_.LoginAccount))
                    {
                        _lrll.Add(new rp_log_login
                        {
                            Id = _.Id,
                            LoginAccount = _.LoginAccount,
                            LoginEnte = _.LoginEnte,
                            LoginSchool = _.LoginSchool,
                            LoginUserName = _.LoginUserName,
                            LoginRoleName = _.LoginRoleName,
                        });
                        _arrl.Add(_.LoginAccount);
                    }
                });
                query = _lrll;

                m.Grid.Pager = new OMS.Common.Model.Pager(m.Grid.Pager.PageIndex, m.Grid.Pager.PageSize, query.Count());
                m.Grid.Data = query.Skip((m.Grid.Pager.PageIndex - 1) * m.Grid.Pager.PageSize).Take(m.Grid.Pager.PageSize).Select(s => new VM_UserLogin_Index_Grid
                {
                    Id = s.Id,
                    LoginAccount = s.LoginAccount,
                    LoginTime = s.LoginTime,
                    LoginDistName = s.LoginDistName,
                    LoginEnte = s.LoginEnte,
                    LoginIP = s.LoginIP,
                    LoginKey = s.LoginKey,
                    LoginRoleName = s.LoginRoleName,
                    LoginSchool = s.LoginSchool,
                    LoginUserName = s.LoginUserName,
                    count = db.rp_log_login.Where(_ => _.LoginAccount == s.LoginAccount).ToList().Count
                }).ToList();
            }
        }
        #endregion

        #region 导出用户登录数据
        public FileResult ExportUserLoginData(VM_UserLogin_Index m)
        {
            string _title = "用户登录情况.xls";
            UserLoginData(m);
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");

            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("账号");
            row1.CreateCell(1).SetCellValue("所属企业");
            row1.CreateCell(2).SetCellValue("所属学校");
            row1.CreateCell(3).SetCellValue("姓名");
            row1.CreateCell(4).SetCellValue("用户角色");
            row1.CreateCell(5).SetCellValue("登录IP");
            row1.CreateCell(6).SetCellValue("IP归属地");
            row1.CreateCell(7).SetCellValue("登录次数");
            if (m.Grid.Data.Count > 0)
            {
                for (int i = 0; i < m.Grid.Data.Count; i++)
                {
                    NPOI.SS.UserModel.IRow row2 = sheet1.CreateRow(i + 1);
                    row2.CreateCell(0).SetCellValue(m.Grid.Data[i].LoginAccount);
                    row2.CreateCell(1).SetCellValue(m.Grid.Data[i].LoginEnte);
                    row2.CreateCell(2).SetCellValue(m.Grid.Data[i].LoginSchool);
                    row2.CreateCell(3).SetCellValue(m.Grid.Data[i].LoginUserName);
                    row2.CreateCell(4).SetCellValue(m.Grid.Data[i].LoginRoleName);
                    row2.CreateCell(5).SetCellValue(m.Grid.Data[i].LoginIP);
                    row2.CreateCell(6).SetCellValue(m.Grid.Data[i].LoginDistName);
                    row2.CreateCell(7).SetCellValue(m.Grid.Data[i].count.ToString());
                }
            }
            //// 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", _title);
        }

        #endregion 
        #endregion

        /// <summary>
        /// 用户登录异常统计表
        /// </summary>
        /// <returns></returns>
        public ActionResult UserLoginError()
        {
            return View();
        }

        #region 用户操作
        public ActionResult UserOperation(VM_UserOper_Index m)
        {
            UserOpe(m);

            return View(m);
        }

        #region 导出用户操作数据
        public FileResult ExportUserOperData(VM_UserOper_Index m)
        {
            string _title = "用户使用情况分析.xls";
            UserOpe(m);
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");

            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("账号");
            row1.CreateCell(1).SetCellValue("角色");
            row1.CreateCell(2).SetCellValue("所属学校");
            row1.CreateCell(3).SetCellValue("电子教材预览次数");
            row1.CreateCell(4).SetCellValue("电影课预览次数");
            row1.CreateCell(5).SetCellValue("云课堂预览次数");
            if (m.Grid.Data.Count > 0)
            {
                for (int i = 0; i < m.Grid.Data.Count; i++)
                {
                    UserOperdata _model = BoxOms.Web.BLL.BookUseBLL.BackUserData(m.Grid.Data[i].UserName);
                    NPOI.SS.UserModel.IRow row2 = sheet1.CreateRow(i + 1);
                    row2.CreateCell(0).SetCellValue(m.Grid.Data[i].UserName);
                    row2.CreateCell(1).SetCellValue(_model.RoleName);
                    row2.CreateCell(2).SetCellValue(_model.SchoolName);
                    row2.CreateCell(3).SetCellValue(_model.BookNum);
                    row2.CreateCell(4).SetCellValue(_model.DykNum);
                    row2.CreateCell(5).SetCellValue(_model.YktNum);
                }
            }
            //// 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", _title);
        }

        #endregion

        #region 用户操作数据
        /// <summary>
        /// 用户操作数据
        /// </summary>
        /// <param name="m"></param>
        private static void UserOpe(VM_UserOper_Index m)
        {

            if (m.Grid == null)
            {
                m.Grid = new OMS.Common.Model.PList<VM_UserOper_Index_Grid>();
                m.Grid.Pager = new OMS.Common.Model.Pager();
            }

            using (var db = new box_omsEntities())
            {
                List<UserGroup> _list = db.Database.SqlQuery<UserGroup>(@"  SELECT UserName FROM [box_oms].[dbo].[rp_resource_statist]
                WHERE UserName IS NOT NULL
                GROUP BY UserName").ToList(); ;

                m.Grid.Pager = new OMS.Common.Model.Pager(m.Grid.Pager.PageIndex, m.Grid.Pager.PageSize, _list.Count());
                m.Grid.Data = _list.Skip((m.Grid.Pager.PageIndex - 1) * m.Grid.Pager.PageSize).Take(m.Grid.Pager.PageSize).Select(s => new VM_UserOper_Index_Grid
                {
                    UserName = s.UserName
                }).ToList();
            }
        }
        #endregion 
        #endregion

        #region 电子书
        public ActionResult BookUse(VM_BookUse_Index m)
        {
            BookUseData(m);
            return View(m);
        }

        public ActionResult AreaBookUse(VM_AreaBookUse_Index m)
        {
            AreaData(m);
            return View(m);
        }
        #region 电子书（区域查看）
        private static void AreaData(VM_AreaBookUse_Index m)
        {
            if (m.grid == null)
            {
                m.grid = new VM_Book_Edition();
            }
            m.AreaList = new ArrayList();
            m.grid.Chinese = new ArrayList();
            m.grid.English = new ArrayList();
            m.grid.Math = new ArrayList();
            ArrayList _arrKey = new ArrayList();
            using (var db = new box_omsEntities())
            {
                var _listRes = db.rp_resource_statist.Where(_ => _.ResourceType == 1).ToList();
                if (_listRes.Count > 0)
                {
                    _listRes.ForEach(_ =>
                    {
                        if (!_arrKey.Contains(_.Cd_key))
                            _arrKey.Add(_.Cd_key);
                    });
                    string where = string.Empty;
                    foreach (var item in _arrKey)
                    {
                        where += string.Format("'{0}'", item.ToString()) + ",";
                    }
                    var _listKey = db.rp_cdkey.SqlQuery(string.Format("SELECT * FROM [dbo].[rp_cdkey] WHERE Id IN({0})", where.TrimEnd(','))).ToList();
                    if (_listKey.Count > 0)
                    {
                        if (m.UseProv != null)
                            _listKey = _listKey.Where(_ => _.UseProv == m.UseProv).ToList();
                        if (m.UseCity != null)
                            _listKey = _listKey.Where(_ => _.UseCity == m.UseCity).ToList();
                        if (m.UseDist != null)
                            _listKey = _listKey.Where(_ => _.UseDist == m.UseDist).ToList();
                        if (!string.IsNullOrWhiteSpace(m.keyid))
                            _listKey = _listKey.Where(_ => _.Id == m.keyid).ToList();
                        _listKey.ForEach(_ =>
                        {
                            if (!m.AreaList.Contains(_.UseDist))
                                m.AreaList.Add(_.UseDist);
                        });
                    }
                }
            }
        }
        #endregion

        #region 电子书（查看全部）
        private static void BookUseData(VM_BookUse_Index m)
        {
            if (m.grid == null)
            {
                m.grid = new VM_Book_Edition();
            }
            m.grid.Chinese = new ArrayList();
            m.grid.English = new ArrayList();
            m.grid.Math = new ArrayList();
            using (var db = new box_omsEntities())
            {
                var _listRes = db.rp_resource_statist.Where(_ => _.ResourceType == 1).ToList();
                if (_listRes.Count > 0)
                {
                    if (m.SDate != null && m.EDate != null)
                    {
                        _listRes = _listRes.Where(_ => _.CreateTime > m.SDate && _.CreateTime < m.EDate).ToList();
                    }

                    foreach (var item in _listRes)
                    {
                        if (item.SubjectId == 1 && !m.grid.Chinese.Contains(item.EditionId))
                            m.grid.Chinese.Add(item.EditionId);
                        if (item.SubjectId == 2 && !m.grid.Math.Contains(item.EditionId))
                            m.grid.Math.Add(item.EditionId);
                        if (item.SubjectId == 3 && !m.grid.English.Contains(item.EditionId))
                            m.grid.English.Add(item.EditionId);
                    }
                }
            }
        }
        #endregion

        #region 导出电子书数据(全部)
        public FileResult ExportBookUseData(VM_BookUse_Index m)
        {
            string _title = "电子教材使用数据(全部).xls";
            BookUseData(m);
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
                Dictionary<int, string> dict = BoxOms.Dict.OperType.Get();
                int k = 0;
                foreach (var item in dict)
                {
                    row1.CreateCell(2 + k).SetCellValue(item.Value);
                    k++;
                }

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
                        }
                        row2.CreateCell(1).SetCellValue(BoxOms.Web.BLL.BookUseBLL.BackEditionName(Convert.ToInt32(m.grid.English[i])));
                        int v = 0;
                        foreach (var item in dict)
                        {
                            row2.CreateCell(v + 2).SetCellValue(BoxOms.Web.BLL.BookUseBLL.BackNum(3, Convert.ToInt32(m.grid.English[i]), item.Key));
                            v++;
                        }
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
                        }
                        row2.CreateCell(1).SetCellValue(BoxOms.Dict.Edition.GetVal(Convert.ToInt32(m.grid.Math[i])));
                        int v = 0;
                        foreach (var item in dict)
                        {
                            row2.CreateCell(v + 2).SetCellValue(BoxOms.Web.BLL.BookUseBLL.BackNum(2, Convert.ToInt32(m.grid.Math[i]), item.Key));
                            v++;
                        }
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
                        }
                        row2.CreateCell(1).SetCellValue(BoxOms.Dict.Edition.GetVal(Convert.ToInt32(m.grid.Chinese[i])));
                        int v = 0;
                        foreach (var item in dict)
                        {
                            row2.CreateCell(v + 2).SetCellValue(BoxOms.Web.BLL.BookUseBLL.BackNum(1, Convert.ToInt32(m.grid.Chinese[i]), item.Key));
                            v++;
                        }
                    }
                }
            }
            else
            {
                //给sheet1添加第一行的头部标题
                NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
                row1.CreateCell(0).SetCellValue("学科");
                row1.CreateCell(1).SetCellValue("版本");
                Dictionary<int, string> dict = BoxOms.Dict.OperType.Get();
                int v = 0;
                foreach (var item in dict)
                {
                    row1.CreateCell(2 + v).SetCellValue(item.Value);
                    v++;
                }
            }

            //// 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", _title);
        }

        #endregion

        #region 导出电子书数据(区域)
        public FileResult ExportBookUseByAreaData(VM_AreaBookUse_Index m)
        {
            string _title = "电子教材使用数据(区域).xls";
            AreaData(m);
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            ICellStyle cellstyle = book.CreateCellStyle();
            cellstyle.VerticalAlignment = VerticalAlignment.Center;
            cellstyle.Alignment = HorizontalAlignment.Center;
            if (m.AreaList.Count > 0)
            {
                //给sheet1添加第一行的头部标题
                NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
                row1.CreateCell(0).SetCellValue("区域");
                row1.CreateCell(1).SetCellValue("学校");
                row1.CreateCell(2).SetCellValue("学科");
                row1.CreateCell(3).SetCellValue("版本");
                Dictionary<int, string> dict = BoxOms.Dict.OperType.Get();
                int k = 0;
                foreach (var item in dict)
                {
                    row1.CreateCell(4 + k).SetCellValue(item.Value);
                    k++;
                }
                //用于记录各个科目应有的版本的个数
                int _allengNum = 0;
                int _allmatNum = 0;
                int _allchiNum = 0;
                foreach (var it in m.AreaList)
                {
                    List<BoxOms.Database.rp_cdkey> _listKey = BoxOms.Web.BLL.BookUseBLL.BackSchoolName(Convert.ToInt32(it.ToString()));
                    int z = 0;
                    foreach (var key in _listKey)
                    {
                        List<BoxOms.Database.rp_resource_statist> _lrrs = BoxOms.Web.BLL.BookUseBLL.BackList(key.Id, 1);
                        m.grid.Chinese = new System.Collections.ArrayList();
                        m.grid.Math = new System.Collections.ArrayList();
                        m.grid.English = new System.Collections.ArrayList();
                        if (_lrrs.Count > 0)
                        {
                            foreach (var item in _lrrs)
                            {
                                if (item.SubjectId == 1 && !m.grid.Chinese.Contains(item.EditionId))
                                {
                                    m.grid.Chinese.Add(item.EditionId);
                                }
                                if (item.SubjectId == 2 && !m.grid.Math.Contains(item.EditionId))
                                {
                                    m.grid.Math.Add(item.EditionId);
                                }
                                if (item.SubjectId == 3 && !m.grid.English.Contains(item.EditionId))
                                {
                                    m.grid.English.Add(item.EditionId);
                                }
                            }
                            int _totalNum = _allengNum + _allchiNum + _allmatNum;
                            int _engNum = 0;
                            int _matNum = 0;
                            int _chiNum = 0;
                            if (m.grid.English.Count > 0)
                            {
                                _engNum = m.grid.English.Count;
                            }
                            if (m.grid.Math.Count > 0)
                            {
                                _matNum = m.grid.Math.Count;
                            }
                            if (m.grid.Chinese.Count > 0)
                            {
                                _chiNum = m.grid.Chinese.Count;
                            }
                            if (m.grid.English.Count > 0)
                            {
                                for (int i = 0; i < m.grid.English.Count; i++)
                                {
                                    NPOI.SS.UserModel.IRow row2 = sheet1.CreateRow(i + 1 + _totalNum);
                                    if (i == 0)
                                    {
                                        if (z == 0)
                                        {
                                            SetCellRangeAddress(sheet1, _totalNum + 1, _totalNum + (_engNum + _matNum + _chiNum) * _listKey.Count, 0, 0);
                                            SetStyle(cellstyle, row2, 0, BoxOms.Web.BLL.BookUseBLL.BackAreaName(Convert.ToInt32(it.ToString())));
                                        }
                                        SetCellRangeAddress(sheet1, _totalNum + 1, _totalNum + _engNum + _matNum + _chiNum, 1, 1);
                                        SetStyle(cellstyle, row2, 1, key.UseSchool);
                                        SetCellRangeAddress(sheet1, _totalNum + 1, _totalNum + _engNum, 2, 2);
                                        SetStyle(cellstyle, row2, 2, "英语");
                                    }
                                    row2.CreateCell(3).SetCellValue(BoxOms.Web.BLL.BookUseBLL.BackEditionName(Convert.ToInt32(m.grid.English[i])));
                                    int v = 0;
                                    foreach (var item in dict)
                                    {
                                        row2.CreateCell(v + 4).SetCellValue(BoxOms.Web.BLL.BookUseBLL.BackNum(3, Convert.ToInt32(m.grid.English[i]), item.Key, key.Id));
                                        v++;
                                    }
                                }
                            }

                            if (m.grid.Math.Count > 0)
                            {
                                for (int i = 0; i < m.grid.Math.Count; i++)
                                {
                                    NPOI.SS.UserModel.IRow row2 = sheet1.CreateRow(i + 1 + _engNum + _totalNum);
                                    if (i == 0)
                                    {
                                        if (m.grid.English.Count == 0)
                                        {
                                            if (z == 0)
                                            {
                                                SetCellRangeAddress(sheet1, _totalNum + 1, _totalNum + (_engNum + _matNum + _chiNum) * _listKey.Count, 0, 0);
                                                SetStyle(cellstyle, row2, 0, BoxOms.Web.BLL.BookUseBLL.BackAreaName(Convert.ToInt32(it.ToString())));
                                            }

                                            SetCellRangeAddress(sheet1, _totalNum + 1, _totalNum + _engNum + _matNum + _chiNum, 1, 1);
                                            SetStyle(cellstyle, row2, 1, key.UseSchool);
                                        }
                                        SetCellRangeAddress(sheet1, _totalNum + _engNum + 1, _totalNum + _engNum + _matNum, 2, 2);
                                        SetStyle(cellstyle, row2, 2, "数学");
                                    }
                                    row2.CreateCell(3).SetCellValue(BoxOms.Web.BLL.BookUseBLL.BackEditionName(Convert.ToInt32(m.grid.Math[i])));
                                    int v = 0;
                                    foreach (var item in dict)
                                    {
                                        row2.CreateCell(v + 4).SetCellValue(BoxOms.Web.BLL.BookUseBLL.BackNum(2, Convert.ToInt32(m.grid.Math[i]), item.Key, key.Id));
                                        v++;
                                    }
                                }
                            }

                            if (m.grid.Chinese.Count > 0)
                            {
                                for (int i = 0; i < m.grid.Chinese.Count; i++)
                                {
                                    NPOI.SS.UserModel.IRow row2 = sheet1.CreateRow(i + 1 + _engNum + _matNum + _totalNum);
                                    if (i == 0)
                                    {
                                        if (m.grid.English.Count == 0 && m.grid.Math.Count == 0)
                                        {
                                            if (z == 0)
                                            {
                                                SetCellRangeAddress(sheet1, _totalNum + 1, _totalNum + (_engNum + _matNum + _chiNum) * _listKey.Count, 0, 0);
                                                SetStyle(cellstyle, row2, 0, BoxOms.Web.BLL.BookUseBLL.BackAreaName(Convert.ToInt32(it.ToString())));
                                            }

                                            SetCellRangeAddress(sheet1, _totalNum + 1, _totalNum + _engNum + _matNum + _chiNum, 1, 1);
                                            SetStyle(cellstyle, row2, 1, key.UseSchool);
                                        }

                                        SetCellRangeAddress(sheet1, _totalNum + _engNum + _matNum + 1, _totalNum + _engNum + _matNum + _chiNum, 2, 2);
                                        SetStyle(cellstyle, row2, 2, "语文");
                                    }
                                    row2.CreateCell(3).SetCellValue(BoxOms.Web.BLL.BookUseBLL.BackEditionName(Convert.ToInt32(m.grid.Chinese[i])));
                                    int v = 0;
                                    foreach (var item in dict)
                                    {
                                        row2.CreateCell(v + 4).SetCellValue(BoxOms.Web.BLL.BookUseBLL.BackNum(1, Convert.ToInt32(m.grid.Chinese[i]), item.Key, key.Id));
                                        v++;
                                    }
                                }
                            }

                            z++;
                            _allchiNum += _chiNum;
                            _allmatNum += _matNum;
                            _allengNum += _engNum;
                        }
                    }
                }
            }
            else
            {
                //给sheet1添加第一行的头部标题
                NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
                row1.CreateCell(0).SetCellValue("区域");
                row1.CreateCell(1).SetCellValue("学校");
                row1.CreateCell(2).SetCellValue("学科");
                row1.CreateCell(3).SetCellValue("版本");
                Dictionary<int, string> dict = BoxOms.Dict.OperType.Get();
                int v = 0;
                foreach (var item in dict)
                {
                    row1.CreateCell(4 + v).SetCellValue(item.Value);
                    v++;
                }
            }
            //// 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", _title);
        }

        #endregion

        #endregion

        #region 电影课
        public ActionResult DykUse()
        {
            return View();
        }

        public ActionResult DykArea(VM_AreaDyk_Index m)
        {
            DykAreaData(m);
            return View(m);
        }

        #region 电影课（区域查看）
        private static void DykAreaData(VM_AreaDyk_Index m)
        {

            m.AreaList = new ArrayList();
            ArrayList _arrKey = new ArrayList();
            using (var db = new box_omsEntities())
            {
                var _listRes = db.rp_resource_statist.Where(_ => _.ResourceType == 2).ToList();
                if (_listRes.Count > 0)
                {
                    _listRes.ForEach(_ =>
                    {
                        if (!_arrKey.Contains(_.Cd_key))
                            _arrKey.Add(_.Cd_key);
                    });
                    string where = string.Empty;
                    foreach (var item in _arrKey)
                    {
                        where += string.Format("'{0}'", item.ToString()) + ",";
                    }
                    var _listKey = db.rp_cdkey.SqlQuery(string.Format("SELECT * FROM [dbo].[rp_cdkey] WHERE Id IN({0})", where.TrimEnd(','))).ToList();
                    if (_listKey.Count > 0)
                    {
                        if (m.UseProv != null)
                            _listKey = _listKey.Where(_ => _.UseProv == m.UseProv).ToList();
                        if (m.UseCity != null)
                            _listKey = _listKey.Where(_ => _.UseCity == m.UseCity).ToList();
                        if (m.UseDist != null)
                            _listKey = _listKey.Where(_ => _.UseDist == m.UseDist).ToList();
                        if (!string.IsNullOrWhiteSpace(m.keyid))
                            _listKey = _listKey.Where(_ => _.Id == m.keyid).ToList();
                        _listKey.ForEach(_ =>
                        {
                            if (!m.AreaList.Contains(_.UseDist))
                                m.AreaList.Add(_.UseDist);
                        });
                    }
                }
            }
        }
        #endregion

        #region 导出电影课数据(全部)
        public FileResult ExportDykData()
        {
            string _title = "电影课使用数据(全部).xls";
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            ICellStyle cellstyle = book.CreateCellStyle();
            cellstyle.VerticalAlignment = VerticalAlignment.Center;
            cellstyle.Alignment = HorizontalAlignment.Center;

            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("教材");
            row1.CreateCell(1).SetCellValue("浏览次数");
            Dictionary<int, string> dict = BoxOms.Dict.DYK.Get();
            int v = 0;
            foreach (var item in dict)
            {
                NPOI.SS.UserModel.IRow row2 = sheet1.CreateRow(v + 1);
                row2.CreateCell(0).SetCellValue(item.Value);
                row2.CreateCell(1).SetCellValue(BoxOms.Web.BLL.BookUseBLL.BackDykNum(item.Key));
                v++;
            }


            //// 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", _title);
        }

        #endregion

        #region 导出电影课数据(区域)
        public FileResult ExportDykAreaData(VM_AreaDyk_Index m)
        {
            DykAreaData(m);
            string _title = "电影课使用数据(区域).xls";
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            ICellStyle cellstyle = book.CreateCellStyle();
            cellstyle.VerticalAlignment = VerticalAlignment.Center;
            cellstyle.Alignment = HorizontalAlignment.Center;
            if (m.AreaList.Count > 0)
            {
                //给sheet1添加第一行的头部标题
                NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
                row1.CreateCell(0).SetCellValue("区域");
                row1.CreateCell(1).SetCellValue("学校");
                row1.CreateCell(2).SetCellValue("教材");
                row1.CreateCell(3).SetCellValue("浏览次数");
                Dictionary<int, string> dict = BoxOms.Dict.DYK.Get();
                int _count = 0;
                foreach (var it in m.AreaList)//区域
                {
                    List<BoxOms.Database.rp_cdkey> _listKey = BoxOms.Web.BLL.BookUseBLL.BackSchoolName(Convert.ToInt32(it.ToString()));
                    int z = 0;
                    foreach (var key in _listKey)//学校
                    {
                        //精确到学校
                        List<BoxOms.Database.rp_resource_statist> _lrrs = BoxOms.Web.BLL.BookUseBLL.BackList(key.Id, 2);
                        if (_lrrs.Count > 0)
                        {
                            int _totalNum = dict.Count * _count;
                            int k = 0;
                            foreach (var item in dict)
                            {
                                NPOI.SS.UserModel.IRow row2 = sheet1.CreateRow(_totalNum + k + 1);
                                if (k == 0)
                                {
                                    if (z == 0)
                                    {
                                        SetCellRangeAddress(sheet1, _totalNum + 1, _totalNum + dict.Count * _listKey.Count, 0, 0);
                                        SetStyle(cellstyle, row2, 0, BoxOms.Web.BLL.BookUseBLL.BackAreaName(Convert.ToInt32(it.ToString())));
                                    }
                                    SetCellRangeAddress(sheet1, _totalNum + 1, _totalNum + dict.Count, 1, 1);
                                    SetStyle(cellstyle, row2, 1, key.UseSchool);
                                }
                                SetStyle(cellstyle, row2, 2, item.Value);
                                SetStyle(cellstyle, row2, 3, BoxOms.Web.BLL.BookUseBLL.BackDykNum(item.Key, key.Id).ToString());
                                k++;
                            }
                            z++;
                            _count++;
                        }
                    }
                }
            }
            else
            {
                //给sheet1添加第一行的头部标题
                NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
                row1.CreateCell(0).SetCellValue("区域");
                row1.CreateCell(1).SetCellValue("学校");
                row1.CreateCell(2).SetCellValue("教材");
                row1.CreateCell(3).SetCellValue("浏览次数");
            }
            //// 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", _title);
        }

        #endregion
        #endregion

        #region 云课堂
        public ActionResult YktUse(VM_YktAll_Index m)
        {
            OperationAllData(m);
            return View(m);
        }

        public ActionResult YktArea(VM_AreaYkt_Index m)
        {
            AreaYktData(m);
            return View(m);
        }


        #region 云课堂查看全部
        /// <summary>
        /// 操作所有数据
        /// </summary>
        /// <param name="m"></param>
        private static void OperationAllData(VM_YktAll_Index m)
        {
            if (m.grid == null)
            {
                m.grid = new VM_Book_Edition();
            }
            m.grid.Chinese = new ArrayList();
            m.grid.English = new ArrayList();
            m.grid.Math = new ArrayList();
            //查询出所有的盒子
            using (var db = new box_omsEntities())
            {
                var _li = db.rp_resource_statist.Where(_ => _.ResourceType == 3).ToList();
                if (_li.Count > 0)
                {
                    if (m.SDate != null && m.EDate != null)
                        _li = _li.Where(_ => _.CreateTime >= m.SDate && _.CreateTime <= m.EDate).ToList();
                    else
                    {
                        m.SDate = null;
                        m.EDate = null;
                    }
                    foreach (var item in _li)
                    {
                        if (item.SubjectId == 1 && !m.grid.Chinese.Contains(item.EditionId))
                            m.grid.Chinese.Add(item.EditionId);
                        if (item.SubjectId == 2 && !m.grid.Math.Contains(item.EditionId))
                            m.grid.Math.Add(item.EditionId);
                        if (item.SubjectId == 3 && !m.grid.English.Contains(item.EditionId))
                            m.grid.English.Add(item.EditionId);
                    }
                }
            }
        }
        #endregion

        #region 导出云课堂数据(全部)
        public FileResult ExportAllData(VM_YktAll_Index m)
        {
            string _title = "云课堂使用情况统计表(全部).xls";
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
                            SetStyle(cellstyle, row2, 3 + dict.Count, BoxOms.Web.BLL.BookUseBLL.BackNumTotalByYkt(3).ToString());
                        }
                        row2.CreateCell(1).SetCellValue(BoxOms.Dict.Edition.GetVal(Convert.ToInt32(m.grid.English[i])));
                        for (int j = 0; j < dict.Count; j++)
                        {
                            row2.CreateCell(j + 2).SetCellValue(BoxOms.Web.BLL.BookUseBLL.BackNumByYkt(3, Convert.ToInt32(m.grid.English[i]), j));
                        }
                        row2.CreateCell(2 + dict.Count).SetCellValue(BoxOms.Web.BLL.BookUseBLL.BackNumTotalByYkt(3, Convert.ToInt32(m.grid.English[i])));
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
                            SetStyle(cellstyle, row2, 3 + dict.Count, BoxOms.Web.BLL.BookUseBLL.BackNumTotalByYkt(2).ToString());
                        }
                        row2.CreateCell(1).SetCellValue(BoxOms.Dict.Edition.GetVal(Convert.ToInt32(m.grid.Math[i])));
                        for (int j = 0; j < dict.Count; j++)
                        {
                            row2.CreateCell(j + 2).SetCellValue(BoxOms.Web.BLL.BookUseBLL.BackNumByYkt(2, Convert.ToInt32(m.grid.Math[i]), j));
                        }
                        row2.CreateCell(2 + dict.Count).SetCellValue(BoxOms.Web.BLL.BookUseBLL.BackNumTotalByYkt(2, Convert.ToInt32(m.grid.Math[i])));
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
                            SetStyle(cellstyle, row2, 3 + dict.Count, BoxOms.Web.BLL.BookUseBLL.BackNumTotalByYkt(1).ToString());
                        }
                        row2.CreateCell(1).SetCellValue(BoxOms.Dict.Edition.GetVal(Convert.ToInt32(m.grid.Chinese[i])));
                        for (int j = 0; j < dict.Count; j++)
                        {
                            row2.CreateCell(j + 2).SetCellValue(BoxOms.Web.BLL.BookUseBLL.BackNumByYkt(1, Convert.ToInt32(m.grid.Chinese[i]), j));
                        }
                        row2.CreateCell(2 + dict.Count).SetCellValue(BoxOms.Web.BLL.BookUseBLL.BackNumTotalByYkt(1, Convert.ToInt32(m.grid.Chinese[i])));
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

        #region 云课堂（区域查看）
        private static void AreaYktData(VM_AreaYkt_Index m)
        {
            if (m.grid == null)
            {
                m.grid = new VM_Book_Edition();
            }
            m.AreaList = new ArrayList();
            m.grid.Chinese = new ArrayList();
            m.grid.English = new ArrayList();
            m.grid.Math = new ArrayList();
            ArrayList _arrKey = new ArrayList();
            using (var db = new box_omsEntities())
            {
                var _listRes = db.rp_resource_statist.Where(_ => _.ResourceType == 3).ToList();
                if (_listRes.Count > 0)
                {
                    _listRes.ForEach(_ =>
                    {
                        if (!_arrKey.Contains(_.Cd_key))
                            _arrKey.Add(_.Cd_key);
                    });
                    string where = string.Empty;
                    foreach (var item in _arrKey)
                    {
                        where += string.Format("'{0}'", item.ToString()) + ",";
                    }
                    var _listKey = db.rp_cdkey.SqlQuery(string.Format("SELECT * FROM [dbo].[rp_cdkey] WHERE Id IN({0})", where.TrimEnd(','))).ToList();
                    if (_listKey.Count > 0)
                    {
                        if (m.UseProv != null)
                            _listKey = _listKey.Where(_ => _.UseProv == m.UseProv).ToList();
                        if (m.UseCity != null)
                            _listKey = _listKey.Where(_ => _.UseCity == m.UseCity).ToList();
                        if (m.UseDist != null)
                            _listKey = _listKey.Where(_ => _.UseDist == m.UseDist).ToList();
                        if (!string.IsNullOrWhiteSpace(m.keyid))
                            _listKey = _listKey.Where(_ => _.Id == m.keyid).ToList();
                        _listKey.ForEach(_ =>
                        {
                            if (!m.AreaList.Contains(_.UseDist))
                                m.AreaList.Add(_.UseDist);
                        });
                    }
                }
            }
        }
        #endregion

        #region 导出云课堂数据(区域)
        public FileResult ExportYktByAreaData(VM_AreaYkt_Index m)
        {
            string _title = "云课堂使用数据(区域).xls";
            AreaYktData(m);
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            ICellStyle cellstyle = book.CreateCellStyle();
            cellstyle.VerticalAlignment = VerticalAlignment.Center;
            cellstyle.Alignment = HorizontalAlignment.Center;
            Dictionary<int, string> dict = BoxOms.Dict.Course.Get();
            Dictionary<int, string> dict1 = BoxOms.Dict.Edition.Get();
            if (m.AreaList.Count > 0)
            {
                //给sheet1添加第一行的头部标题
                NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
                row1.CreateCell(0).SetCellValue("区域");
                row1.CreateCell(1).SetCellValue("学校");
                row1.CreateCell(2).SetCellValue("学科");
                row1.CreateCell(3).SetCellValue("版本");
                int k = 0;
                foreach (var item in dict)
                {
                    row1.CreateCell(4 + k).SetCellValue(item.Value);
                    k++;
                }
                row1.CreateCell(4 + dict.Count).SetCellValue("版本小计");
                row1.CreateCell(5 + dict.Count).SetCellValue("学科小计");
                //用于记录各个科目应有的版本的个数
                int _allengNum = 0;
                int _allmatNum = 0;
                int _allchiNum = 0;
                foreach (var it in m.AreaList)
                {
                    List<BoxOms.Database.rp_cdkey> _listKey = BoxOms.Web.BLL.BookUseBLL.BackSchoolName(Convert.ToInt32(it.ToString()));
                    int z = 0;
                    foreach (var key in _listKey)
                    {
                        List<BoxOms.Database.rp_resource_statist> _lrrs = BoxOms.Web.BLL.BookUseBLL.BackList(key.Id, 3);
                        m.grid.Chinese = new System.Collections.ArrayList();
                        m.grid.Math = new System.Collections.ArrayList();
                        m.grid.English = new System.Collections.ArrayList();
                        if (_lrrs.Count > 0)
                        {
                            foreach (var item in _lrrs)
                            {
                                if (item.SubjectId == 1 && !m.grid.Chinese.Contains(item.EditionId))
                                {
                                    m.grid.Chinese.Add(item.EditionId);
                                }
                                if (item.SubjectId == 2 && !m.grid.Math.Contains(item.EditionId))
                                {
                                    m.grid.Math.Add(item.EditionId);
                                }
                                if (item.SubjectId == 3 && !m.grid.English.Contains(item.EditionId))
                                {
                                    m.grid.English.Add(item.EditionId);
                                }
                            }
                            int _totalNum = _allengNum + _allchiNum + _allmatNum;
                            int _engNum = 0;
                            int _matNum = 0;
                            int _chiNum = 0;
                            if (m.grid.English.Count > 0)
                            {
                                _engNum = m.grid.English.Count;
                            }
                            if (m.grid.Math.Count > 0)
                            {
                                _matNum = m.grid.Math.Count;
                            }
                            if (m.grid.Chinese.Count > 0)
                            {
                                _chiNum = m.grid.Chinese.Count;
                            }
                            if (m.grid.English.Count > 0)
                            {
                                for (int i = 0; i < m.grid.English.Count; i++)
                                {
                                    NPOI.SS.UserModel.IRow row2 = sheet1.CreateRow(i + 1 + _totalNum);
                                    if (i == 0)
                                    {
                                        if (z == 0)
                                        {
                                            SetCellRangeAddress(sheet1, _totalNum + 1, _totalNum + (_engNum + _matNum + _chiNum) * _listKey.Count, 0, 0);
                                            SetStyle(cellstyle, row2, 0, BoxOms.Web.BLL.BookUseBLL.BackAreaName(Convert.ToInt32(it.ToString())));
                                        }
                                        SetCellRangeAddress(sheet1, _totalNum + 1, _totalNum + _engNum + _matNum + _chiNum, 1, 1);
                                        SetStyle(cellstyle, row2, 1, key.UseSchool);
                                        SetCellRangeAddress(sheet1, _totalNum + 1, _totalNum + _engNum, 2, 2);
                                        SetCellRangeAddress(sheet1, _totalNum + 1, _totalNum + _engNum, 5 + dict.Count, 5 + dict.Count);
                                        SetStyle(cellstyle, row2, 2, "英语");
                                        SetStyle(cellstyle, row2, 5 + dict.Count, BoxOms.Web.BLL.BookUseBLL.BackNumTotalByYkt(3, key.Id).ToString());
                                    }
                                    row2.CreateCell(3).SetCellValue(BoxOms.Dict.Edition.GetVal(Convert.ToInt32(m.grid.English[i])));

                                    for (int j = 0; j < dict.Count; j++)
                                    {
                                        row2.CreateCell(j + 4).SetCellValue(BoxOms.Web.BLL.BookUseBLL.BackNumByYkt(3, Convert.ToInt32(m.grid.English[i]), j, key.Id));
                                    }
                                    row2.CreateCell(4 + dict.Count).SetCellValue(BoxOms.Web.BLL.BookUseBLL.BackNumTotalByYkt(3, Convert.ToInt32(m.grid.English[i]), key.Id));
                                }
                            }

                            if (m.grid.Math.Count > 0)
                            {
                                for (int i = 0; i < m.grid.Math.Count; i++)
                                {
                                    NPOI.SS.UserModel.IRow row2 = sheet1.CreateRow(i + 1 + _engNum + _totalNum);
                                    if (i == 0)
                                    {
                                        if (m.grid.English.Count == 0)
                                        {
                                            if (z == 0)
                                            {
                                                SetCellRangeAddress(sheet1, _totalNum + 1, _totalNum + (_engNum + _matNum + _chiNum) * _listKey.Count, 0, 0);
                                                SetStyle(cellstyle, row2, 0, BoxOms.Web.BLL.BookUseBLL.BackAreaName(Convert.ToInt32(it.ToString())));
                                            }

                                            SetCellRangeAddress(sheet1, _totalNum + 1, _totalNum + _engNum + _matNum + _chiNum, 1, 1);
                                            SetStyle(cellstyle, row2, 1, key.UseSchool);
                                        }

                                        SetCellRangeAddress(sheet1, _totalNum + _engNum + 1, _totalNum + _engNum + _matNum, 2, 2);
                                        SetCellRangeAddress(sheet1, _totalNum + _engNum + 1, _totalNum + _engNum + _matNum, 5 + dict.Count, 5 + dict.Count);
                                        SetStyle(cellstyle, row2, 2, "数学");
                                        SetStyle(cellstyle, row2, 5 + dict.Count, BoxOms.Web.BLL.BookUseBLL.BackNumTotalByYkt(2, key.Id).ToString());
                                    }
                                    row2.CreateCell(3).SetCellValue(BoxOms.Dict.Edition.GetVal(Convert.ToInt32(m.grid.Math[i])));
                                    for (int j = 0; j < dict.Count; j++)
                                    {
                                        row2.CreateCell(j + 4).SetCellValue(BoxOms.Web.BLL.BookUseBLL.BackNumByYkt(2, Convert.ToInt32(m.grid.Math[i]), j, key.Id));
                                    }
                                    row2.CreateCell(4 + dict.Count).SetCellValue(BoxOms.Web.BLL.BookUseBLL.BackNumTotalByYkt(2, Convert.ToInt32(m.grid.Math[i]), key.Id));
                                }
                            }

                            if (m.grid.Chinese.Count > 0)
                            {
                                for (int i = 0; i < m.grid.Chinese.Count; i++)
                                {
                                    NPOI.SS.UserModel.IRow row2 = sheet1.CreateRow(i + 1 + _engNum + _matNum + _totalNum);
                                    if (i == 0)
                                    {
                                        if (m.grid.English.Count == 0 && m.grid.Math.Count == 0)
                                        {
                                            if (z == 0)
                                            {
                                                SetCellRangeAddress(sheet1, _totalNum + 1, _totalNum + (_engNum + _matNum + _chiNum) * _listKey.Count, 0, 0);
                                                SetStyle(cellstyle, row2, 0, BoxOms.Web.BLL.BookUseBLL.BackAreaName(Convert.ToInt32(it.ToString())));
                                            }

                                            SetCellRangeAddress(sheet1, _totalNum + 1, _totalNum + _engNum + _matNum + _chiNum, 1, 1);
                                            SetStyle(cellstyle, row2, 1, key.UseSchool);
                                        }

                                        SetCellRangeAddress(sheet1, _totalNum + _engNum + _matNum + 1, _totalNum + _engNum + _matNum + _chiNum, 2, 2);
                                        SetCellRangeAddress(sheet1, _totalNum + _engNum + _matNum + 1, _totalNum + _engNum + _matNum + _chiNum, 5 + dict.Count, 5 + dict.Count);
                                        SetStyle(cellstyle, row2, 2, "语文");
                                        SetStyle(cellstyle, row2, 5 + dict.Count, BoxOms.Web.BLL.BookUseBLL.BackNumTotalByYkt(1, key.Id).ToString());
                                    }
                                    row2.CreateCell(3).SetCellValue(BoxOms.Dict.Edition.GetVal(Convert.ToInt32(m.grid.Chinese[i])));

                                    for (int j = 0; j < dict.Count; j++)
                                    {
                                        row2.CreateCell(j + 4).SetCellValue(BoxOms.Web.BLL.BookUseBLL.BackNumByYkt(2, Convert.ToInt32(m.grid.Chinese[i]), j, key.Id));
                                    }
                                    row2.CreateCell(4 + dict.Count).SetCellValue(BoxOms.Web.BLL.BookUseBLL.BackNumTotalByYkt(1, Convert.ToInt32(m.grid.Chinese[i]), key.Id));
                                }
                            }

                            z++;
                            _allchiNum += _chiNum;
                            _allmatNum += _matNum;
                            _allengNum += _engNum;
                        }
                    }
                }
            }
            else
            {
                //给sheet1添加第一行的头部标题
                NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
                row1.CreateCell(0).SetCellValue("区域");
                row1.CreateCell(1).SetCellValue("学校");
                row1.CreateCell(2).SetCellValue("学科");
                row1.CreateCell(3).SetCellValue("版本");
                int v = 0;
                foreach (var item in dict)
                {
                    row1.CreateCell(4 + v).SetCellValue(item.Value);
                    v++;
                }
                row1.CreateCell(4 + dict.Count).SetCellValue("版本小计");
                row1.CreateCell(5 + dict.Count).SetCellValue("学科小计");
            }
            //// 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", _title);
        }

        #endregion 
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

        #region 根据区域ID获取学校
        /// <summary>
        /// 根据区域ID获取学校
        /// </summary>
        /// <param name="aid">区域ID</param>
        /// <returns></returns>
        public JsonResult GetSchool(int? aid)
        {
            using (var db = new box_omsEntities())
            {
                List<SelectListItem> items = db.rp_cdkey.Where(w => w.UseDist == aid).Select(s => new SelectListItem { Text = s.UseSchool, Value = s.Id }).ToList();
                return Json(items, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}