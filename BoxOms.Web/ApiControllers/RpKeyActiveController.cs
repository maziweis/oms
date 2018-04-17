using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using OMS.Common.Model;
using BoxOms.Web.ApiModels;
using BoxOms.Database;

namespace BoxOms.Web.ApiControllers
{
    public class RpKeyActiveController : ApiController
    {
        /// <summary>
        /// 激活
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonData Post([FromBody]AM_RpKeyActivePost m)
        {
            string ip = ((System.Web.HttpContextWrapper)Request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            string key = "";
            using (var db = new box_omsEntities())
            {
                rp_cdkey dbm = db.rp_cdkey.Where(_ => _.Id == m.id).FirstOrDefault();
                if (dbm == null)
                {
                    return new JsonData { flag = JsonDataFlag.ValidFailed, msg = "无效的激活码！" };
                }
                if (dbm.ActiveTime != null && dbm.ActiveMac != m.mac)//同一个激活码在同一台电脑上可以被无限激活(防止重装系统后原激活码无法使用)
                {
                    return new JsonData { flag = JsonDataFlag.ValidFailed, msg = "激活码已被使用！" };
                }

                if (dbm.Validity == null)
                    dbm.ExpirTime = Convert.ToDateTime("2099-12-12");
                else
                    dbm.ExpirTime = DateTime.Now.AddMonths(Convert.ToInt32(dbm.Validity));
                key = OMS.Common.SecurityHelper.Encrypt(string.Format("{0}|{1}|{2}", m.id, ((DateTime)dbm.ExpirTime).ToString("yyyy-MM-dd"), m.mac), "a%k8h5.o");
                dbm.ActiveTime = DateTime.Now;
                dbm.ActiveMac = m.mac;
                dbm.ActiveDisk = m.disk;
                dbm.ActiveIp = ip;
                dbm.ActiveCode = key;
                db.SaveChanges();
            }

            return new JsonData { flag = JsonDataFlag.Succeed, data = key };
        }
    }
}
