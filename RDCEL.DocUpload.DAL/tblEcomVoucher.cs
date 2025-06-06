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
    
    public partial class tblEcomVoucher
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblEcomVoucher()
        {
            this.tblEcomPhoneSpecifics = new HashSet<tblEcomPhoneSpecific>();
        }
    
        public int EcomVoucherId { get; set; }
        public string VoucherCode { get; set; }
        public string Phoneno { get; set; }
        public Nullable<int> BrandId { get; set; }
        public string CategoryIds { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> EcomVoucherType { get; set; }
        public Nullable<int> VoucherCount { get; set; }
        public string voucherstatus { get; set; }
        public Nullable<int> ValueType { get; set; }
        public Nullable<int> FixedValue { get; set; }
        public Nullable<int> Percentage { get; set; }
        public Nullable<int> PercLimit { get; set; }
        public Nullable<bool> IsUsed { get; set; }
    
        public virtual tblBrand tblBrand { get; set; }
        public virtual tblCompany tblCompany { get; set; }
        public virtual tblUser tblUser { get; set; }
        public virtual tblUser tblUser1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblEcomPhoneSpecific> tblEcomPhoneSpecifics { get; set; }
    }
}
