using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace BoxOms.Web
{
    public static class HtmlHelpers
    {
        private static List<SelectListItem> PageSizeList = new List<SelectListItem>()
        {
            new SelectListItem{  Value="10",  Text="10条"},
            new SelectListItem{  Value="20",  Text="20条"},
            new SelectListItem{  Value="50",  Text="50条"},
            new SelectListItem{  Value="100", Text="100条"}
        };

        public static MvcHtmlString Pager(this HtmlHelper helper, OMS.Common.Model.Pager pager)
        {
            StringBuilder sbHtmlText = new StringBuilder();
            sbHtmlText.Append("<div style=\"min-height:50px;\"><div style=\"float:right;\"><ul  class=\"pagination\">");
            if (pager.PageIndex <= 1)
            {
                sbHtmlText.Append("<li class=\"disabled\"><a href=\"javascript:void()\" aria-label=\"Previous\"><span aria-hidden=\"true\">首页</span></a></li>");
                sbHtmlText.Append("<li class=\"disabled\"><a href=\"javascript:void()\" aria-label=\"Previous\"><span aria-hidden=\"true\">上页</span></a></li>");
            }
            else
            {
                sbHtmlText.Append("<li><a href=\"javascript:changePageIndex(1)\" aria-label=\"1\"><span aria-hidden=\"true\">首页</span></a></li>");
                sbHtmlText.AppendFormat("<li><a href=\"javascript:changePageIndex({0})\" aria-label=\"1\"><span aria-hidden=\"true\">上页</span></a></li>", pager.PageIndex - 1);
            }



            int[] pags = new int[5];
            for (int i = 0; i < 5; i++)
            {
                //判读是否要变动页码，如果不大于3就为初始页码
                if (pager.PageIndex > 3)
                {
                    pags[i] = pager.PageIndex - 2 + i;
                    //超出最大页码，直接从后往前赋值
                    if (pags[i] > pager.PageCount)
                    {
                        int reduce = 4;
                        for (int j = 0; j < 5; j++)
                        {
                            pags[j] = pager.PageCount - reduce;
                            reduce--;
                        }
                    }
                }
                else
                {
                    pags[i] = i + 1;
                }
            }

            foreach (int pag in pags)
            {
                if (pag > pager.PageCount || pag < 1)
                {
                    continue;
                }

                if (pag == pager.PageIndex)
                {
                    sbHtmlText.AppendFormat("<li class=\"active\"><a href=\"javascript:void()\">{0}<span class=\"sr-only\">(current)</span></a></li>", pag);
                }
                else
                {
                    sbHtmlText.AppendFormat("<li><a href=\"javascript:changePageIndex({0})\">{0}</a></li>", pag);
                }
            }





            if (pager.PageIndex >= pager.PageCount)
            {
                sbHtmlText.Append("<li class=\"disabled\"><a href=\"javascript:void()\" aria-label=\"Next\"><span aria-hidden=\"true\">下页</span></a></li>");
                sbHtmlText.Append("<li class=\"disabled\"><a href=\"javascript:void()\" aria-label=\"Next\"><span aria-hidden=\"true\">尾页</span></a></li>");
            }
            else
            {
                sbHtmlText.AppendFormat("<li><a href=\"javascript:changePageIndex({0})\" aria-label=\"Next\"><span aria-hidden=\"true\">下页</span></a></li>", pager.PageIndex + 1);
                sbHtmlText.AppendFormat("<li><a href=\"javascript:changePageIndex({0})\" aria-label=\"Next\"><span aria-hidden=\"true\">尾页</span></a></li>", pager.PageCount);
            }
            sbHtmlText.Append("</ul></div>");
            sbHtmlText.AppendFormat("<div style=\"float:left;margin-top:5px;\"><span>共<strong>{0}</strong>条，<strong>{1}</strong>/<strong>{2}</strong>页，每页显示<select id=\"pSize\" onchange=\"changePageSize(this)\">", pager.TotalCount, pager.PageIndex, pager.PageCount);
            foreach (SelectListItem item in PageSizeList)
            {
                if (item.Value != pager.PageSize.ToString())
                {
                    sbHtmlText.AppendFormat("<option value=\"{0}\">{1}</option>", item.Value, item.Text);
                }
                else
                {
                    sbHtmlText.AppendFormat("<option selected=\"selected\" value=\"{0}\">{1}</option>", item.Value, item.Text);
                }

            }
            sbHtmlText.Append("</select></span></div></div>");
            return MvcHtmlString.Create(sbHtmlText.ToString());
        }
    }
}
