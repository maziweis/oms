using BoxOms.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoxOms.Web.BLL
{
    public class SyRoleBLL
    {
        /// <summary>
        /// 得到所有的角色
        /// </summary>
        /// <returns></returns>
        public static List<sys_role> GetAllRole()
        {
            using (var db = new box_omsEntities())
            {
                return db.sys_role.OrderBy(o => o.RoleId).ToList();
            }
        }

        /// <summary>
        /// 根据用户id找到所对应的角色
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static string BackRoleName(int userid)
        {
            string _roleName = string.Empty;
            using (var db = new box_omsEntities())
            {
                var _list = db.sys_user_and_role.Where(_ => _.UserId == userid).ToList();
                if (_list.Count == 0)
                    return _roleName;
                foreach (var item in _list)
                {
                    _roleName += db.sys_role.Where(_ => _.RoleId == item.RoleId).FirstOrDefault().RoleName + ",";
                }
            }
            return _roleName;
        }
    }
}