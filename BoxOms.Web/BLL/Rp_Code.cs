using BoxOms.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoxOms.Web.BLL
{
    public class Rp_Code
    {
        /// <summary>
        /// 得到所有的企业信息
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> GetData()
        {
            Dictionary<int, string> _dic = new Dictionary<int, string>();
            using (var db = new box_omsEntities())
            {
                var _list = db.rp_enterprise.OrderBy(o => o.EntId).ToList();
                if (_list.Count > 0)
                {
                    foreach (var item in _list)
                    {
                        _dic.Add(item.EntId, item.EntName);
                    }
                }
            }
            return _dic;
        }

        /// <summary>
        /// 根据激活码编号与学科返回对应的版本id
        /// </summary>
        /// <param name="coId"></param>
        /// <param name="Subject"></param>
        /// <returns></returns>
        public static string BackEditionid(string coId, int Subject)
        {
            string _name = string.Empty;
            using (var db = new box_omsEntities())
            {
                var _list = db.rp_cdkey_and_edition.Where(_ => _.CdKey == coId && _.Subject == Subject).ToList();
                if (_list.Count == 0)
                    return _name;
                foreach (var item in _list)
                {
                    _name += item.Edition + ",";
                }
            }
            return _name;
        }

        /// <summary>
        /// 根据激活码编号与学科返回对应的版本
        /// </summary>
        /// <param name="coId"></param>
        /// <param name="Subject"></param>
        /// <returns></returns>
        public static string BackEditionName(string coId, int Subject)
        {
            string _name = string.Empty;
            using (var db = new box_omsEntities())
            {
                var _list = db.rp_cdkey_and_edition.Where(_ => _.CdKey == coId && _.Subject == Subject).ToList();
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
        /// 根据编号返回所属区域
        /// </summary>
        /// <param name="coId"></param>
        /// <returns></returns>
        public static string BackPrName(string coId)
        {
            string _name = string.Empty;
            using (var db = new box_omsEntities())
            {
                var _m = db.rp_cdkey.Where(_ => _.Id == coId).FirstOrDefault();
                if (_m == null)
                    return _name;
                _name += db.oms_district.Where(_ => _.ID == _m.UseProv).FirstOrDefault() == null ? "" : db.oms_district.Where(_ => _.ID == _m.UseProv).FirstOrDefault().CodeName;
                _name += db.oms_district.Where(_ => _.ID == _m.UseCity).FirstOrDefault() == null ? "" : db.oms_district.Where(_ => _.ID == _m.UseCity).FirstOrDefault().CodeName;
                _name += db.oms_district.Where(_ => _.ID == _m.UseDist).FirstOrDefault() == null ? "" : db.oms_district.Where(_ => _.ID == _m.UseDist).FirstOrDefault().CodeName;
            }
            return _name;
        }
    }
}