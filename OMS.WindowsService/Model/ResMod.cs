using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.WindowsService.Model
{
    public class ResMod
    {
        /// <summary>
        /// 激活码
        /// </summary>
        public string ActiveCode { get; set; }
        /// <summary>
        /// 学科编号
        /// </summary>
        public string SubjectId { get; set; }
        /// <summary>
        /// 版本编号
        /// </summary>
        public string EditionId { get; set; }
        /// <summary>
        /// 版本名称
        /// </summary>
        public string EditionName { get; set; }
        /// <summary>
        /// 大的资源类型(1：电子教材；2：电影课；3：云课堂)
        /// </summary>
        public string ResourceType { get; set; }

        public string Account { get; set; }

        public string RoleName { get; set; }

        public string SchoolName { get; set; }
    }
}
