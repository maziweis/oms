//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace BoxOms.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class box_good_online
    {
        public int BoxId { get; set; }
        public Nullable<System.DateTime> LastTime { get; set; }
        public string LastIp { get; set; }
    
        public virtual box_good box_good { get; set; }
    }
}
