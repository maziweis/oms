using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BoxOms.Web.Controllers
{
    public class BoxHomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}