using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxOms.Dict
{
    /// <summary>
    /// 资源
    /// </summary>
    public class Course
    {
        private static Dictionary<int, string> d = null;
        /// <summary>
        /// 构造函数
        /// </summary>
        static Course()
        {
            d = new Dictionary<int, string>();
            d.Add(0, "数字教材");
            d.Add(1, "巩固练习");
            d.Add(2, "快乐天地");
            d.Add(3, "趣味游戏");
            d.Add(4, "同步练习");
            d.Add(5, "快乐阅读");
            d.Add(6, "日常英语");
            d.Add(7, "随堂小练");
            d.Add(8, "备课资源");
            d.Add(9, "练习册");
            d.Add(10, "跟我唱");
            d.Add(11, "故事园");
            d.Add(12, "互动课堂");
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
