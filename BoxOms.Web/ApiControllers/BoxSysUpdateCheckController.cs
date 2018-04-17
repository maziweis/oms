using OMS.Common.Model;
using BoxOms.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BoxOms.Web.ApiControllers
{
    public class BoxSysUpdateCheckController : ApiController
    {
        /// <summary>
        /// 根据网卡MAC地址获取该商品是否需要更新
        /// </summary>
        /// <param name="id">MAC地址</param>
        /// <returns>返回：True-需要更新，False-不需要更新</returns>
        public JsonData Get(string id)
        {
            using (var db = new box_omsEntities())
            {
                //1.判断当前盒子是否有权限更新
                var good = db.box_good.Where(w => w.Mac == id).FirstOrDefault();
                if (good == null || good.IsCanUpdate == false)
                {
                    return new JsonData { flag = JsonDataFlag.Succeed, data = false, msg = "无自动更新权限" };
                }

                //2.判断当前盒子是否在更新中或者版本更新失败
                if (db.box_update_sys_log.Where(w => w.BoxId == good.BoxId && w.State != 2).Count() > 0)
                {
                    return new JsonData { flag = JsonDataFlag.Succeed, data = false, msg = string.Format("【{0}】版正在更新中，或者已经更新失败", good.SysVersion) };
                }

                //3.判断当前已发布的版本更新包中是否有更新版本。
                if (db.box_update_sys.OrderBy(o => o.VNumber).Where(w => w.VNumber > good.SysVersion && w.IsPublish == true).Count() <= 0)
                {
                    return new JsonData { flag = JsonDataFlag.Succeed, data = false, msg = "目前没有需要更新的版本" };
                }

                return new JsonData { flag = JsonDataFlag.Succeed, data = true };
            }

        }
    }
}