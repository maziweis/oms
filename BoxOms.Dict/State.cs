using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BoxOms.Dict
{
    /// <summary>
    /// 状态
    /// </summary>
    public class State
    {
        private static Dictionary<int, string> d = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        static State()
        {
            d = new Dictionary<int, string>();
            d.Add(0, "正常");
            d.Add(1, "停用");
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