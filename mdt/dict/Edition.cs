using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mdt.dict
{
    public class Edition
    {
        private static Dictionary<int, string> d = null;

        static Edition()
        {
            d = new Dictionary<int, string>();
            d.Add(25, "牛津上海本地版");
            d.Add(61, "外研新标准(三起)");
            d.Add(9, "外研新标准(一起)");
            d.Add(30, "山东版");
            d.Add(66, "部编本");
            d.Add(27, "人教版");
            d.Add(64, "北师大版");
        }

        public static Dictionary<int, string> Get()
        {
            return d;
        }

        public static string GetVal(int key)
        {
            return d.ContainsKey(key) ? d[key] : "";
        }
    }
}
