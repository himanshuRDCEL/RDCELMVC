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
    
    public partial class tblEVCPODDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblEVCPODDetail()
        {
            this.tblOrderLGCs = new HashSet<tblOrderLGC>();
        }
    
        public int Id { get; set; }
        public string RegdNo { get; set; }
        public Nullable<int> ExchangeId { get; set; }
        public Nullable<int> EVCId { get; set; }
        public string PODURL { get; set; }
        public Nullable<int> ABBRedemptionId { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string InvoicePdfName { get; set; }
        public string DebitNotePdfName { get; set; }
        public Nullable<int> InvSrNum { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public Nullable<decimal> InvoiceAmount { get; set; }
        public Nullable<System.DateTime> DebitNoteDate { get; set; }
        public Nullable<decimal> DebitNoteAmount { get; set; }
        public Nullable<int> DNSrNum { get; set; }
        public Nullable<int> OrderTransId { get; set; }
        public string FinancialYear { get; set; }
        public Nullable<int> EVCPartnerId { get; set; }
    
        public virtual tblUser tblUser { get; set; }
        public virtual tblUser tblUser1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblOrderLGC> tblOrderLGCs { get; set; }
        public virtual tblEVCRegistration tblEVCRegistration { get; set; }
        public virtual tblOrderTran tblOrderTran { get; set; }
    }
}
