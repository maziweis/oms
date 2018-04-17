using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxOms.Dict
{
    public class DYK
    {
        private static Dictionary<int, string> d = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        static DYK()
        {
            d = new Dictionary<int, string>();
            d.Add(1, "电影听说分级教程一年级");
            d.Add(2, "电影听说分级教程二年级");
            d.Add(3, "电影听说分级教程三年级");
            d.Add(4, "电影听说分级教程四年级");
            d.Add(5, "电影听说分级教程五年级");
            d.Add(6, "电影听说分级教程六年级");
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
