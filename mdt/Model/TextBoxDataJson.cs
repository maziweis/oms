using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mdt.Model
{
    public class TextBoxDataJson
    {
        /// <summary>
        /// 书名
        /// </summary>
        public string book { get; set; }
        /// <summary>
        /// 页内容
        /// </summary>
        public List<TextBoxPageDataJson> pageSource { get; set; }
    }

    public class TextBoxPageDataJson
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int pageId { get; set; }
        /// <summary>
        /// 底图的路径
        /// </summary>
        public string pageImg { get; set; }
        /// <summary>
        /// 每页上面的点读信息
        /// </summary>
        public List<TextBoxPageButtonDataJson> buttons { get; set; }
    }
    public class TextBoxPageButtonDataJson
    {
        public int id { get; set; }
        /// <summary>
        /// 按钮类型(5)
        /// </summary>
        public int eventtype { get; set; }
        public double height { get; set; }
        public double width { get; set; }
        public double x { get; set; }
        public double y { get; set; }
        public string soundsrc { get; set; }
    }
}