using BoxOms.Database;
using BoxOms.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoxOms.Web.BLL
{
    public class BookUseBLL
    {
        /// <summary>
        /// 根据版本编号得到版本名称(电子书)
        /// </summary>
        /// <param name="editionid"></param>
        /// <returns></returns>
        public static string BackEditionName(int editionid)
        {
            string _editionName = string.Empty;
            using (var db = new box_omsEntities())
            {
                var _m = db.rp_resource_statist.Where(_ => _.EditionId == editionid && _.ResourceType == 1).FirstOrDefault();
                if (_m != null)
                    _editionName = _m.EditionName;
            }
            return _editionName;
        }

        /// <summary>
        /// 根据学科、版本、类型得到数量(电子书)
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="edition"></param>
        /// <param name="type"></param>
        /// <param name="resourcetype"></param>
        /// <returns></returns>
        public static int BackNum(int subject, int edition, int type, string keyid = "")
        {
            int _num = 0;
            using (var db = new box_omsEntities())
            {
                if (!string.IsNullOrEmpty(keyid))
                    _num = db.rp_resource_statist.Where(_ => _.EditionId == edition && _.SubjectId == subject && _.Type == type && _.Cd_key == keyid && _.ResourceType == 1).ToList().Count;
                else
                    _num = db.rp_resource_statist.Where(_ => _.EditionId == edition && _.SubjectId == subject && _.Type == type && _.ResourceType == 1).ToList().Count;
            }
            return _num;
        }

        /// <summary>
        /// 根据区域返回该区域所有的学校
        /// </summary>
        /// <param name="UseDist"></param>
        /// <returns></returns>
        public static List<rp_cdkey> BackSchoolName(int UseDist)
        {
            List<rp_cdkey> _list = new List<rp_cdkey>();
            using (var db = new box_omsEntities())
            {
                _list = db.rp_cdkey.Where(_ => _.UseDist == UseDist).ToList();
            }
            return _list;
        }

        /// <summary>
        /// 返回数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<rp_resource_statist> BackList(string key, int resourcetype)
        {
            List<rp_resource_statist> _lrs = new List<rp_resource_statist>();
            using (var db = new box_omsEntities())
            {
                _lrs = db.rp_resource_statist.Where(_ => _.Cd_key == key && _.ResourceType == resourcetype).ToList();
            }
            return _lrs;
        }
        /// <summary>
        /// 根据区域id返回完整地址信息
        /// </summary>
        /// <param name="areaid"></param>
        /// <returns></returns>
        public static string BackAreaName(int areaid)
        {
            string _name = string.Empty;
            using (var db = new box_omsEntities())
            {
                var _list = db.oms_district.ToList();
                var _marea = _list.Where(_ => _.ID == areaid).FirstOrDefault();
                var _mcity = _list.Where(_ => _.ID == _marea.ParentID).FirstOrDefault();
                var _pro = _list.Where(_ => _.ID == _mcity.ParentID).FirstOrDefault();
                _name = _pro.CodeName + _mcity.CodeName + _marea.CodeName;
            }
            return _name;
        }

        /// <summary>
        /// 根据区域编号筛选出该区域下所有的学校
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetSchoolData(int pid)
        {
            Dictionary<string, string> _dic = new Dictionary<string, string>();
            using (var db = new box_omsEntities())
            {
                var _list = db.rp_cdkey.Where(_ => _.UseDist == pid).ToList();
                if (_list.Count > 0)
                {
                    foreach (var item in _list)
                    {
                        _dic.Add(item.Id, item.UseSchool);
                    }
                }
            }
            return _dic;
        }

        /// <summary>
        /// 返回电影课次数
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int BackDykNum(int type, string keyid = "")
        {
            int _num = 0;
            using (var db = new box_omsEntities())
            {
                if (!string.IsNullOrEmpty(keyid))
                    _num = db.rp_resource_statist.Where(_ => _.ResourceType == 2 && _.Type == type && _.Cd_key == keyid).ToList().Count;
                else
                    _num = db.rp_resource_statist.Where(_ => _.ResourceType == 2 && _.Type == type).ToList().Count;
            }
            return _num;
        }

        /// <summary>
        /// 根据科目、版本、资源类型返回使用次数(云课堂)
        /// </summary>
        /// <param name="list"></param>
        /// <param name="Subject"></param>
        /// <param name="Edition"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public static int BackNumByYkt(int Subject, int Edition, int Type, string keyid = "")
        {
            int num = 0;
            using (var db = new box_omsEntities())
            {
                if (!string.IsNullOrWhiteSpace(keyid))
                    num = db.rp_resource_statist.Where(_ => _.SubjectId == Subject && _.EditionId == Edition && _.Type == Type && _.ResourceType == 3 && _.Cd_key == keyid).ToList().Count;
                else
                    num = db.rp_resource_statist.Where(_ => _.SubjectId == Subject && _.EditionId == Edition && _.Type == Type && _.ResourceType == 3).ToList().Count;
            }
            return num;
        }

        /// <summary>
        /// 根据科目、版本返回总的使用次数(云课堂)
        /// </summary>
        /// <param name="list"></param>
        /// <param name="Subject"></param>
        /// <param name="Edition"></param>
        /// <returns></returns>
        public static int BackNumTotalByYkt(int Subject, int Edition, string keyid = "")
        {
            int num = 0;
            using (var db = new box_omsEntities())
            {
                if (!string.IsNullOrWhiteSpace(keyid))
                    num = db.rp_resource_statist.Where(_ => _.SubjectId == Subject && _.EditionId == Edition && _.ResourceType == 3 && _.Cd_key == keyid).ToList().Count;
                else
                    num = db.rp_resource_statist.Where(_ => _.SubjectId == Subject && _.EditionId == Edition && _.ResourceType == 3).ToList().Count;
            }
            return num;
        }

        /// <summary>
        /// 根据科目返回总的使用次数(云课堂)
        /// </summary>
        /// <param name="list"></param>
        /// <param name="Subject"></param>
        /// <returns></returns>
        public static int BackNumTotalByYkt(int Subject, string keyid = "")
        {
            int num = 0;
            using (var db = new box_omsEntities())
            {
                if (!string.IsNullOrWhiteSpace(keyid))
                    num = db.rp_resource_statist.Where(_ => _.SubjectId == Subject && _.ResourceType == 3 && _.Cd_key == keyid).ToList().Count;
                else
                    num = db.rp_resource_statist.Where(_ => _.SubjectId == Subject && _.ResourceType == 3).ToList().Count;
            }
            return num;
        }

        /// <summary>
        /// 根据用户账号返回信息（用户操作记录）
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public static UserOperdata BackUserData(string account)
        {
            UserOperdata _model = new UserOperdata();
            using (var db = new box_omsEntities())
            {
                var _list = db.rp_resource_statist.ToList();
                if (_list.Count > 0)
                {
                    var m = _list.Where(_ => _.RoleName == account && !string.IsNullOrEmpty(_.SchoolName) && !string.IsNullOrEmpty(_.RoleName)).FirstOrDefault();
                    if (m != null)
                    {
                        _model.RoleName = m.UserName;
                        _model.SchoolName = m.SchoolName;
                    }
                    _model.BookNum = _list.Where(_ => _.ResourceType == 1 && _.UserName == account).ToList().Count;
                    _model.YktNum = _list.Where(_ => _.ResourceType == 3 && _.UserName == account).ToList().Count;
                    _model.DykNum = _list.Where(_ => _.ResourceType == 2 && _.UserName == account).ToList().Count;
                }

            }
            return _model;
        }
    }
}