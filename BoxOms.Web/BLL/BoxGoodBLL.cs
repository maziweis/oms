using BoxOms.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoxOms.Web.BLL
{
    public class BoxGoodBLL
    {
        /// <summary>
        /// 根据MAC地址获取激活码
        /// </summary>
        /// <param name="mac">MAC地址</param>
        /// <returns></returns>
        public static string GetKey(string mac, DateTime? _time)
        {
            return OMS.Common.SecurityHelper.Encrypt(string.Format("{0}|{1}", mac, ((DateTime)_time).ToString("yyyy-MM-dd")), "a%k8h5.o");
            //OMS.Common.SecurityHelper.Encrypt(OMS.Common.SecurityHelper.Encrypt(mac, "Z2*7e43_"), "V_)5<?%g");
        }

        /// <summary>
        /// 根据盒子编号返回所属省市区
        /// </summary>
        /// <param name="BoxId"></param>
        /// <returns></returns>
        public static string BackPrName(int BoxId)
        {
            string _name = string.Empty;
            using (var db = new box_omsEntities())
            {
                var _m = db.box_good.Find(BoxId);
                if (_m == null)
                    return _name;
                _name += db.oms_district.Where(_ => _.ID.ToString() == _m.Prov).FirstOrDefault() == null ? "" : db.oms_district.Where(_ => _.ID.ToString() == _m.Prov).FirstOrDefault().CodeName;
                _name += db.oms_district.Where(_ => _.ID.ToString() == _m.City).FirstOrDefault() == null ? "" : db.oms_district.Where(_ => _.ID.ToString() == _m.City).FirstOrDefault().CodeName;
                _name += db.oms_district.Where(_ => _.ID.ToString() == _m.Area).FirstOrDefault() == null ? "" : db.oms_district.Where(_ => _.ID.ToString() == _m.Area).FirstOrDefault().CodeName;
            }
            return _name;
        }
        /// <summary>
        /// 根据盒子与学科返回对应的版本
        /// </summary>
        /// <param name="BoxId"></param>
        /// <param name="Subject"></param>
        /// <returns></returns>
        public static string BackEditionName(int BoxId, int Subject)
        {
            string _name = string.Empty;
            using (var db = new box_omsEntities())
            {
                var _list = db.box_subject_edition.Where(_ => _.BoxId == BoxId && _.Subject == Subject).ToList();
                if (_list.Count == 0)
                    return _name;
                foreach (var item in _list)
                {
                    _name += BoxOms.Dict.Edition.GetVal(item.Edition) + ",";
                }
            }
            return _name.TrimEnd(',');
        }
        /// <summary>
        /// 根据盒子与学科返回对应的版本id
        /// </summary>
        /// <param name="BoxId"></param>
        /// <param name="Subject"></param>
        /// <returns></returns>
        public static string BackEditionid(int BoxId, int Subject)
        {
            string _name = string.Empty;
            using (var db = new box_omsEntities())
            {
                var _list = db.box_subject_edition.Where(_ => _.BoxId == BoxId && _.Subject == Subject).ToList();
                if (_list.Count == 0)
                    return _name;
                foreach (var item in _list)
                {
                    _name += item.Edition + ",";
                }
            }
            return _name;
        }


        #region 根据id得到区域名称
        /// <summary>
        /// 根据id得到区域名称
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public static string GetName(int pid)
        {
            using (var db = new box_omsEntities())
            {
                var _m = db.oms_district.Where(_ => _.ID == pid && _.Deleted == 0).FirstOrDefault();
                if (_m != null)
                    return _m.CodeName;
                return "";
            }
        }
        #endregion


        #region 得到省市数据
        /// <summary>
        /// 得到省市数据
        /// </summary>
        /// <param name="pid">如果是省就不需要传</param>
        /// <returns></returns>
        public static Dictionary<int, string> GetData(int pid = 0)
        {
            Dictionary<int, string> _dic = new Dictionary<int, string>();
            using (var db = new box_omsEntities())
            {
                var _list = db.oms_district.Where(_ => _.ParentID == pid && _.Deleted == 0).ToList();
                if (_list.Count > 0)
                {
                    foreach (var item in _list)
                    {
                        _dic.Add(item.ID, item.CodeName);
                    }
                }
            }
            return _dic;
        }
        #endregion

        #region 根据区域编号筛选出该区域下所有的学校
        /// <summary>
        /// 根据区域编号筛选出该区域下所有的学校
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public static Dictionary<int, string> GetSchoolData(string pid)
        {
            Dictionary<int, string> _dic = new Dictionary<int, string>();
            using (var db = new box_omsEntities())
            {
                var _list = db.box_good.Where(_ => _.Area.Contains(pid)).ToList();
                if (_list.Count > 0)
                {
                    foreach (var item in _list)
                    {
                        _dic.Add(item.BoxId, item.SchoolName);
                    }
                }
            }
            return _dic;
        }
        #endregion


    }
}