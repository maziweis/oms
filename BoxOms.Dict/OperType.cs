using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxOms.Dict
{
    public class OperType
    {
        private static Dictionary<int, string> d = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        static OperType()
        {
            d = new Dictionary<int, string>();
            d.Add(500, "教材预览");
            d.Add(501, "编辑教材");
            d.Add(6, "动画预览");
            d.Add(27, "互动课件");
            d.Add(555, "PPT使用");//没有
            d.Add(28, "图片");
            d.Add(5, "音频");
            d.Add(41, "微课");
            d.Add(12, "教案");
            d.Add(2, "文本");
            d.Add(666, "点读");//没有
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> Get()
        {
            return d;
        }

        /// <summary>
        /// 获取Value
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static string GetVal(int key)
        {
            return d.Keys.Contains(key) ? d[key] : null;
        }
    }
}
