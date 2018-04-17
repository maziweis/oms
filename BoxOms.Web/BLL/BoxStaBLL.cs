using BoxOms.Database;
using BoxOms.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoxOms.Web.BLL
{
    public class BoxStaBLL
    {
        /// <summary>
        /// 根据科目、版本、资源类型返回使用次数
        /// </summary>
        /// <param name="list"></param>
        /// <param name="Subject"></param>
        /// <param name="Edition"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public static int BackNum(List<VM_BoxRunStat_Index_Grid> list, int Subject, int Edition, int Type)
        {
            int num = 0;
            using (var db = new box_omsEntities())
            {
                if (list.Count == 0)
                    return num;
                var _list = db.box_resource_statist.Where(_ => _.Subject == Subject && _.Edition == Edition && _.Type == Type).ToList();
                var _list_result = from t in _list
                                   from r in list
                                   where t.BoxId == r.BoxId
                                   select t;
                num = _list_result.Count();
            }
            return num;
        }

        /// <summary>
        /// 根据科目、版本、资源类型的名称返回使用次数
        /// </summary>
        /// <param name="list"></param>
        /// <param name="Subject"></param>
        /// <param name="Edition"></param>
        /// <param name="TypeName"></param>
        /// <returns></returns>
        public static int BackNum(List<VM_BoxRunStat_Index_Grid> list, int Subject, int Edition, string TypeName)
        {
            int num = 0;
            using (var db = new box_omsEntities())
            {
                if (list.Count == 0 || string.IsNullOrEmpty(TypeName))
                    return num;
                var _list = db.box_resource_statist.Where(_ => _.Subject == Subject && _.Edition == Edition && _.TypeName == TypeName).ToList();
                var _list_result = from t in _list
                                   from r in list
                                   where t.BoxId == r.BoxId
                                   select t;
                num = _list_result.Count();
            }
            return num;
        }

        /// <summary>
        /// 根据科目、版本返回资源类型名称
        /// </summary>
        /// <param name="Subject"></param>
        /// <param name="Edition"></param>
        public static List<string> BackTypeName(int Subject, int Edition)
        {
            List<string> _list = new List<string>();
            using (var db = new box_omsEntities())
            {
                var _query = db.Database.SqlQuery<dataInfo>(string.Format(@"SELECT TypeName FROM box_resource_statist
                WHERE Subject={0} AND Edition={1} AND TypeName IS NOT NULL GROUP BY TypeName", Subject, Edition)).ToList();
                if (_query.Count != 0)
                {
                    _query.ForEach(_ =>
                    {
                        _list.Add(_.TypeName);
                    });
                }
                int _count = _list.Count;
                if (_count != 13)
                {
                    for (int i = 0; i < 13 - _count; i++)
                    {
                        _list.Add("");
                    }
                }
            }
            return _list;
        }


        /// <summary>
        /// 根据科目、版本返回总的使用次数
        /// </summary>
        /// <param name="list"></param>
        /// <param name="Subject"></param>
        /// <param name="Edition"></param>
        /// <returns></returns>
        public static int BackNumTotal(List<VM_BoxRunStat_Index_Grid> list, int Subject, int Edition)
        {
            int num = 0;
            using (var db = new box_omsEntities())
            {
                if (list.Count == 0)
                    return num;
                var _list = db.box_resource_statist.Where(_ => _.Subject == Subject && _.Edition == Edition && !string.IsNullOrEmpty(_.TypeName)).ToList();
                var _list_result = from t in _list
                                   from r in list
                                   where t.BoxId == r.BoxId
                                   select t;
                num = _list_result.Count();
            }
            return num;
        }

        /// <summary>
        /// 根据科目返回总的使用次数
        /// </summary>
        /// <param name="list"></param>
        /// <param name="Subject"></param>
        /// <returns></returns>
        public static int BackNumTotal(List<VM_BoxRunStat_Index_Grid> list, int Subject)
        {
            int num = 0;
            using (var db = new box_omsEntities())
            {
                if (list.Count == 0)
                    return num;
                var _list = db.box_resource_statist.Where(_ => _.Subject == Subject).ToList();
                var _list_result = from t in _list
                                   from r in list
                                   where t.BoxId == r.BoxId
                                   select t;
                num = _list_result.Count();
            }
            return num;
        }

    }
}