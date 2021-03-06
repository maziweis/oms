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
    
    public partial class box_good
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public box_good()
        {
            this.box_subject_edition = new HashSet<box_subject_edition>();
            this.box_update_sys_log = new HashSet<box_update_sys_log>();
        }
    
        public int BoxId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Mac { get; set; }
        public string ActivNumber { get; set; }
        public Nullable<int> Edition { get; set; }
        public double SysVersion { get; set; }
        public string Principal { get; set; }
        public Nullable<System.DateTime> UpTime { get; set; }
        public int State { get; set; }
        public bool IsCanUpdate { get; set; }
        public bool IsActiv { get; set; }
        public int Total { get; set; }
        public string Prov { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string SchoolName { get; set; }
        public string IP { get; set; }
        public Nullable<System.DateTime> FirstRunTime { get; set; }
        public Nullable<System.DateTime> RunTime { get; set; }
        public string UseUserName { get; set; }
        public string Remark { get; set; }
        public System.DateTime CreateTime { get; set; }
        public int CreateUserId { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
        public Nullable<int> UpdateUserId { get; set; }
        public Nullable<System.DateTime> ExpirTime { get; set; }
        public Nullable<int> Validity { get; set; }
    
        public virtual box_good_online box_good_online { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<box_subject_edition> box_subject_edition { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<box_update_sys_log> box_update_sys_log { get; set; }
    }
}
