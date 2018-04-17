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
    public class BoxSysUpdatePathController : ApiController
    {
        /// <summary>
        /// 根据网卡MAC地址获取该商品的更新包列表
        /// </summary>
        /// <param name="id">MAC地址</param>
        /// <returns>返回：更新包列表</returns>
        public JsonData Get(string id)
        {
            List<string> list = new List<string>();
            using (var db = new box_omsEntities())
            {
                var good = db.box_good.Where(w => w.Mac == id).FirstOrDefault();
                var pack = db.box_update_sys.OrderBy(o => o.VNumber).Where(w => w.VNumber > good.SysVersion && w.IsPublish == true).FirstOrDefault();
                list.Add(pack.PackUrl);
                list.Add(pack.VNumber.ToString());
                db.box_update_sys_log.Add(new box_update_sys_log
                {
                    BoxId = good.BoxId,
                    UpdateId = pack.Id,
                    State = 1,
                    StartTime = DateTime.Now
                });

                db.SaveChanges();
            }
            return new JsonData { flag = JsonDataFlag.Succeed, data = list };
        }
    }
}
