using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mdt.dict
{
    public class Course
    {
        private static Dictionary<int, string> d = null;

        static Course()
        {
            d = new Dictionary<int, string>();
            d.Add(211, "SHBD1A");//牛津上海版
            d.Add(215, "SHBD1B");
            d.Add(220, "SHBD2A");
            d.Add(216, "SHBD2B");
            d.Add(212, "SHBD3A");
            d.Add(217, "SHBD3B");
            d.Add(213, "SHBD4A");
            d.Add(218, "SHBD4B");
            d.Add(214, "SHBD5A");
            d.Add(219, "SHBD5B");
            d.Add(357, "WY(1)3A");
            d.Add(363, "WY(1)3B");
            d.Add(358, "WY(1)4A");
            d.Add(364, "WY(1)4B");
            d.Add(359, "WY(1)5A");
            d.Add(365, "WY(1)5B");
            d.Add(360, "WY(1)6A");
            d.Add(366, "WY(1)6B");
            d.Add(395, "WY(3)3A");
            d.Add(399, "WY(3)3B");
            d.Add(396, "WY(3)4A");
            d.Add(400, "WY(3)4B");
            d.Add(397, "WY(3)5A");
            d.Add(401, "WY(3)5B");
            d.Add(398, "WY(3)6A");
            d.Add(402, "WY(3)6B");
            d.Add(208, "SD3AT");
            d.Add(287, "SD3BT");
            d.Add(209, "SD4AT");
            d.Add(288, "SD4BT");
            d.Add(210, "SD5AT");
            d.Add(289, "SD5BT");
            //d.Add(436, "BSDSX1A");
            //d.Add(438, "BSDSX2A");
            //d.Add(439, "BSDSX3A");
            //d.Add(440, "BSDSX4A");
            //d.Add(441, "BSDSX5A");
            d.Add(306, "RJYW1A");
            d.Add(270, "RJYW1B");
            d.Add(307, "RJYW2A");
            d.Add(271, "RJYW2B");
            d.Add(331, "RJYW3A");
            d.Add(351, "RJYW3B");
            d.Add(332, "RJYW4A");
            d.Add(352, "RJYW4B");
            d.Add(333, "RJYW5A");
            d.Add(353, "RJYW5B");
            d.Add(334, "RJYW6A");
            d.Add(354, "RJYW6B");
            d.Add(415, "RJYW1A_2016");
            d.Add(413, "RJYW1B_2016");
            d.Add(436, "RJYW2A_2017");
        }

        public static Dictionary<int, string> Get()
        {
            return d;
        }

        public static string GetVal(int key)
        {
            return d.ContainsKey(key) ? d[key] : "";
        }

        public static int GetId(string val)
        {
            return d.ContainsValue(val) ? d.Where(_ => _.Value == val).FirstOrDefault().Key : 0;
        }
    }
}
