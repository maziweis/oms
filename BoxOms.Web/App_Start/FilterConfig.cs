using BoxOms.Web.App_Start;
using System.Web;
using System.Web.Mvc;

namespace BoxOms.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new AuthAttribute());
            filters.Add(new HandleErrorAttribute());
        }
    }
}
