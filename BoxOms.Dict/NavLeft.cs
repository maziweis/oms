using BoxOms.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoxOms.Dict
{
    public class NavLeft
    {
        private static List<VM_NavLeft> d = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        static NavLeft()
        {
            d = new List<VM_NavLeft>();

            //系统管理
            d.Add(new VM_NavLeft { Id = 3, NavTopId = 2, Name = "用户管理", Url = "/SyUser/Index", Icon = "", Sort = 1 });
            d.Add(new VM_NavLeft { Id = 4, NavTopId = 2, Name = "角色管理", Url = "/SyRole/Index", Icon = "", Sort = 2 });

            //盒子
            d.Add(new VM_NavLeft { Id = 4010, NavTopId = 1, Name = "系统管理", Url = "/BoxGood/Index", Icon = "", Sort = 1 });
            d.Add(new VM_NavLeft { Id = 4011, NavTopId = 1, Name = "运行管理", Url = "/BoxRunStat/Index", Icon = "", Sort = 2 });
            d.Add(new VM_NavLeft { Id = 4013, NavTopId = 1, Name = "数据统计", Url = "/BoxStatistical/Index", Icon = "", Sort = 3 });
            d.Add(new VM_NavLeft { Id = 4012, NavTopId = 1, Name = "版本更新", Url = "/BoxUpdate/Index", Icon = "", Sort = 4 });

            //资源开放平台
            d.Add(new VM_NavLeft { Id = 5010, NavTopId = 4, Name = "企业管理", Url = "/RpEnte/Index", Icon = "", Sort = 1 });
            d.Add(new VM_NavLeft { Id = 5011, NavTopId = 4, Name = "激活码管理", Url = "/RpCode/Index", Icon = "", Sort = 2 });
            d.Add(new VM_NavLeft { Id = 5012, NavTopId = 4, Name = "报表管理", Url = "/RpReport/Index", Icon = "", Sort = 3 });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static List<VM_NavLeft> Get(int? NavTopId)
        {
            return d.Where(w => w.NavTopId == NavTopId).ToList();
        }

        public static List<VM_NavLeft> Get(string controller, string action, int userid)
        {
            int _topid = 0;
            //VM_NavLeft _m = d.Where(w => w.Url == "/" + controller + "/" + action).FirstOrDefault();
            //if (_m != null)
            //    _topid = _m.NavTopId;
            var _mpage = Pages.Get().Where(_ => _.Controller == controller && _.Action == action).FirstOrDefault();
            if (_mpage != null)
                _topid = Convert.ToInt32(_mpage.NavTopId);
            if (_topid == 0)
                return new List<VM_NavLeft>();
            //首先找到该用户是否选择了角色
            using (var db = new box_omsEntities())
            {
                var _listRole = db.sys_user_and_role.Where(_ => _.UserId == userid).Select(w => w.RoleId);
                if (_listRole.Count() == 0)
                    return new List<VM_NavLeft>();
                var _listAuth = db.sys_role_and_auth.Where(_ => _listRole.Contains(_.RoleId)).ToList();
                if (_listAuth.Count == 0)
                    return new List<VM_NavLeft>();
                return d.Where(w => _listAuth.Select(_ => _.LeftId).Contains(w.Id) && w.NavTopId == _topid).OrderBy(o => o.Sort).ToList();
            }
        }


        public static List<VM_NavLeft> Get()
        {
            return d.OrderBy(o => o.Sort).ToList();
        }
    }

    public class VM_NavLeft
    {
        public int Id { get; set; }
        public int NavTopId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public int? Sort { get; set; }
    }
}