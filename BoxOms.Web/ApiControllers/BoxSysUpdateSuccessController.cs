using BoxOms.Database;
using BoxOms.Web.ApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BoxOms.Web.ApiControllers
{
    public class BoxSysUpdateSuccessController : ApiController
    {
        /// <summary>
        /// 更新成功
        /// </summary>
        /// <param name="m"></param>
        [HttpPost]
        public void Post([FromBody]AM_BoxSysUpdateSuccessPost m)
        {
            using (var db = new box_omsEntities())
            {
                var good = db.box_good.Where(w => w.Mac == m.mac).FirstOrDefault();

                var update_log = db.box_update_sys_log.Where(w => w.BoxId == good.BoxId && w.State == 1).FirstOrDefault();//获取该盒子当前正在更新的更新包
                update_log.State = 2;
                update_log.FinishTime = DateTime.Now;

                good.SysVersion = db.box_update_sys.Find(update_log.UpdateId).VNumber;

                db.SaveChanges();
            }
        }
    }
}
