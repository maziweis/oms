using OMS.Common.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace BoxOms.Web.ApiControllers
{
    public class BoxUpLogController : ApiController
    {
        /// <summary>
        /// 上传商品操作日志
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonData Post()
        {
            HttpFileCollection files = HttpContext.Current.Request.Files;
            HttpPostedFile file = files[files.AllKeys[0]];
            string path = string.Format("{0}\\Files\\Box\\Logs\\", System.Web.Hosting.HostingEnvironment.MapPath("~/"));

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            file.SaveAs(string.Format("{0}{1}.xml", path, Guid.NewGuid().ToString()));

            return new JsonData { flag = JsonDataFlag.Succeed, data = true };
        }
    }
}
