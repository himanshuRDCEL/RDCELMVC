//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RDCEL.DocUpload.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblExchangeOrderStatu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblExchangeOrderStatu()
        {
            this.tblExchangeABBStatusHistories = new HashSet<tblExchangeABBStatusHistory>();
            this.tblExchangeOrders = new HashSet<tblExchangeOrder>();
            this.tblLogistics = new HashSet<tblLogistic>();
            this.tblOrderTrans = new HashSet<tblOrderTran>();
            this.tblTimelineStatusMappings = new HashSet<tblTimelineStatusMapping>();
            this.tblABBRegistrations = new HashSet<tblABBRegistration>();
            this.tblABBRedemptions = new HashSet<tblABBRedemption>();
        }
    
        public int Id { get; set; }
        public string StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public string StatusName { get; set; }
        public Nullable<bool> IsActive { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblExchangeABBStatusHistory> tblExchangeABBStatusHistories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblExchangeOrder> tblExchangeOrders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblLogistic> tblLogistics { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblOrderTran> tblOrderTrans { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblTimelineStatusMapping> tblTimelineStatusMappings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblABBRegistration> tblABBRegistrations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblABBRedemption> tblABBRedemptions { get; set; }
    }
}
