using BoxOms.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoxOms.Dict
{
    public class Pages
    {
        private static List<VM_Page> d = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        static Pages()
        {
            d = new List<VM_Page>();
            d.Add(new VM_Page { Controller = "BoxHome", Action = "Index", Name = "首页", NavTopId = 3 });

            d.Add(new VM_Page { Controller = "SyUser", Action = "Index", Name = "用户管理", NavTopId = 2, NavLeftId = 3 });
            d.Add(new VM_Page { Controller = "SyRole", Action = "Index", Name = "角色管理", NavTopId = 2, NavLeftId = 4 });
            d.Add(new VM_Page { Controller = "SyRole", Action = "Add", Name = "新增角色", NavTopId = 2, NavLeftId = 4 });
            d.Add(new VM_Page { Controller = "SyRole", Action = "Edit", Name = "编辑角色", NavTopId = 2, NavLeftId = 4 });

            //资源盒子
            d.Add(new VM_Page { Controller = "BoxGood", Action = "Index", Name = "系统管理", NavTopId = 1, NavLeftId = 4010 });
            d.Add(new VM_Page { Controller = "BoxRunStat", Action = "Index", Name = "运行管理", NavTopId = 1, NavLeftId = 4011 });
            d.Add(new VM_Page { Controller = "BoxRunStat", Action = "Index2", Name = "访问统计", NavTopId = 1, NavLeftId = 4011 });
            d.Add(new VM_Page { Controller = "BoxUpdate", Action = "Index", Name = "版本更新", NavTopId = 1, NavLeftId = 4012 });
            d.Add(new VM_Page { Controller = "BoxUpdate", Action = "Add", Name = "新增版本", NavTopId = 1, NavLeftId = 4012 });
            d.Add(new VM_Page { Controller = "BoxUpdate", Action = "Edit", Name = "编辑版本", NavTopId = 1, NavLeftId = 4012 });
            d.Add(new VM_Page { Controller = "BoxStatistical", Action = "Index", Name = "数据统计", NavTopId = 1, NavLeftId = 4013 });
            d.Add(new VM_Page { Controller = "BoxStatistical", Action = "AllTotal", Name = "总体统计", NavTopId = 1, NavLeftId = 4013 });

            //资源开放平台
            d.Add(new VM_Page { Controller = "RpEnte", Action = "Index", Name = "企业管理", NavTopId = 4, NavLeftId = 5010 });
            d.Add(new VM_Page { Controller = "RpCode", Action = "Index", Name = "激活码管理", NavTopId = 4, NavLeftId = 5011 });
            d.Add(new VM_Page { Controller = "RpReport", Action = "Index", Name = "报表管理", NavTopId = 4, NavLeftId = 5012 });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static VM_Page Get(string controller, string action)
        {
            return d.Where(w => w.Controller == controller && w.Action == action).FirstOrDefault();
        }

        public static List<VM_Page> Get()
        {
            return d.OrderBy(o => o.NavLeftId).ToList();
        }

        public static List<VM_Page> GetHome()
        {
            return d.Where(w => w.NavTopId == 3).ToList();
        }

        public static List<VM_Page> Get(int id, int userid)
        {
            List<VM_Page> _list = new List<VM_Page>();
            //首先找到该用户是否选择了角色
            using (var db = new box_omsEntities())
            {
                var _listRole = db.sys_user_and_role.Where(_ => _.UserId == userid).Select(w => w.RoleId);
                if (_listRole.Count() == 0)
                    return GetHome();
                var _listAuth = db.sys_role_and_auth.Where(_ => _listRole.Contains(_.RoleId)).ToList();
                if (_listAuth.Count == 0)
                    return GetHome();
                //foreach (var item in _listAuth)
                //{
                //    _list = Get().Where(w => item.LeftId == w.NavLeftId && w.NavTopId == id).OrderBy(_ => _.NavLeftId).ToList();
                //    if (_list.Count > 0)
                //        break;
                //}
                var _list_result = from t in Get()
                                   from r in _listAuth
                                   where t.NavLeftId == r.LeftId
                                   select t;
                var _li = _list_result.Where(w => w.NavTopId == id).OrderBy(_ => _.NavLeftId).ToList();
                _li.Insert(0, GetHome().FirstOrDefault());
                return _li;
            }

        }
    }

    public class VM_Page
    {
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Name { get; set; }
        public int? NavTopId { get; set; }
        public int? NavLeftId { get; set; }
    }
}