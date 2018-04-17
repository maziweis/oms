using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OMS.Common.Model
{
    /// <summary>
    /// 数据分页模型
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public class PList<T> where T : class, new()
    {
        public List<T> Data { get; set; }

        public Pager Pager { get; set; }
    }
}
