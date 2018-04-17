using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxOms.Dict
{
    public class Edition
    {
        private static Dictionary<int, string> d = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        static Edition()
        {
            d = new Dictionary<int, string>();
            d.Add(1, "人教PEP版");
            d.Add(3, "北京版");
            d.Add(4, "牛津上海本地版");
            d.Add(5, "牛津上海全国版");
            d.Add(9, "外研新标准(一起)");
            d.Add(10, "人教版新起点");
            d.Add(14, "湘少版");
            d.Add(16, "人教版精通");
            d.Add(21, "牛津深圳版");
            d.Add(22, "江苏译林");
            d.Add(24, "广州版");
            d.Add(27, "人教版");
            d.Add(30, "山东版");
            d.Add(31, "闽教版");
            d.Add(33, "人教新目标");
            d.Add(39, "广东版");
            d.Add(61, "外研新标准(三起)");
            d.Add(64, "北师大版");
            d.Add(66, "部编本");
            d.Add(999, "电影课");

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

        /// <summary>
        /// 得到英语版本
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> GetEnglish()
        {
            return d.Where(_ => _.Key != 66 && _.Key != 27 && _.Key != 64).ToDictionary(_ => _.Key, _ => _.Value);
        }

        /// <summary>
        /// 得到数学版本
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> GetMath()
        {
            return d.Where(_ => _.Key == 3 || _.Key == 27 || _.Key == 64).ToDictionary(_ => _.Key, _ => _.Value);
        }

        /// <summary>
        /// 得到语文版本
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> GetChinese()
        {
            return d.Where(_ => _.Key == 66 || _.Key == 27).ToDictionary(_ => _.Key, _ => _.Value);
        }
    }
}
