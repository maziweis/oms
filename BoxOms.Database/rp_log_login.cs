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
    
    public partial class rp_log_login
    {
        public string Id { get; set; }
        public string LoginAccount { get; set; }
        public string LoginUserName { get; set; }
        public string LoginSchool { get; set; }
        public string LoginRoleName { get; set; }
        public string LoginEnte { get; set; }
        public string LoginDistName { get; set; }
        public string LoginKey { get; set; }
        public Nullable<System.DateTime> LoginTime { get; set; }
        public string LoginIP { get; set; }
    
        public virtual rp_cdkey rp_cdkey { get; set; }
    }
}
