using BoxOms.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoxOms.Dict
{
    public class NavTop
    {
        private static List<VM_NavTop> d = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        static NavTop()
        {
            d = new List<VM_NavTop>();
            d.Add(new VM_NavTop { Id = 3, Name = "首页", Url = "/BoxHome/Index", Icon = "glyphicon glyphicon-home", Sort = 10, IsEnabled = true });
            d.Add(new VM_NavTop { Id = 1, Name = "金太阳盒子", Url = "/BoxGood/Index", Icon = "glyphicon glyphicon-cd", Sort = 30, IsEnabled = true });
            d.Add(new VM_NavTop { Id = 4, Name = "资源开放平台", Url = "/RpEnte/Index", Icon = "glyphicon glyphicon-leaf", Sort = 35, IsEnabled = true });
            d.Add(new VM_NavTop { Id = 2, Name = "系统管理", Url = "/SyUser/Index", Icon = "glyphicon glyphicon-cog", Sort = 100, IsEnabled = true });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<VM_NavTop> Get()
        {
            return d.Where(w => w.IsEnabled == true).OrderBy(o => o.Sort).ToList();
        }

        public static List<VM_NavTop> GetHome()
        {
            return d.Where(w => w.IsEnabled == true && w.Id == 3).OrderBy(o => o.Sort).ToList();
        }

        public static List<VM_NavTop> Get(int userid)
        {
            //首先找到该用户是否选择了角色
            using (var db = new box_omsEntities())
            {
                var _listRole = db.sys_user_and_role.Where(_ => _.UserId == userid).Select(w => w.RoleId);
                if (_listRole.Count() == 0)
                    return GetHome();
                var _listAuth = db.sys_role_and_auth.Where(_ => _listRole.Contains(_.RoleId)).ToList();
                if (_listAuth.Count == 0)
                    return GetHome();
                var _list = NavLeft.Get().Where(_ => _listAuth.Select(w => w.LeftId).Contains(_.Id)).ToList();
                var _listRe = d.Where(w => _list.Select(_ => _.NavTopId).Contains(w.Id) && w.IsEnabled == true).OrderBy(o => o.Sort).ToList();
                _listRe.Insert(0, GetHome().FirstOrDefault());
                return _listRe;
            }
            //return d.Where(w => w.IsEnabled == true).OrderBy(o => o.Sort).ToList();
        }
    }

    public class VM_NavTop
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public int? Sort { get; set; }
        public bool IsEnabled { get; set; }
    }
}