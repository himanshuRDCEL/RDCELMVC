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
    
    public partial class tblABBRedemption
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblABBRedemption()
        {
            this.tblSelfQCs = new HashSet<tblSelfQC>();
            this.tblVoucherVerfications = new HashSet<tblVoucherVerfication>();
            this.tblOrderTrans = new HashSet<tblOrderTran>();
        }
    
        public int RedemptionId { get; set; }
        public Nullable<int> ABBRegistrationId { get; set; }
        public Nullable<int> ZohoABBRedemptionId { get; set; }
        public Nullable<int> CustomerDetailsId { get; set; }
        public string RegdNo { get; set; }
        public string ABBRedemptionStatus { get; set; }
        public string StoreOrderNo { get; set; }
        public string Sponsor { get; set; }
        public string LogisticsComments { get; set; }
        public string QCComments { get; set; }
        public string InvoiceNo { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public string InvoiceImage { get; set; }
        public Nullable<int> RedemptionPeriod { get; set; }
        public Nullable<int> RedemptionPercentage { get; set; }
        public Nullable<System.DateTime> RedemptionDate { get; set; }
        public Nullable<decimal> RedemptionValue { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> StatusId { get; set; }
        public Nullable<decimal> FinalRedemptionValue { get; set; }
        public string ReferenceId { get; set; }
        public Nullable<bool> IsVoucherUsed { get; set; }
        public string VoucherCode { get; set; }
        public Nullable<System.DateTime> VoucherCodeExpDate { get; set; }
        public Nullable<int> VoucherStatusId { get; set; }
        public Nullable<bool> IsDefferedSettelment { get; set; }
        public Nullable<int> BusinessPartnerId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblSelfQC> tblSelfQCs { get; set; }
        public virtual tblBusinessPartner tblBusinessPartner { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblVoucherVerfication> tblVoucherVerfications { get; set; }
        public virtual tblABBRedemption tblABBRedemption1 { get; set; }
        public virtual tblABBRedemption tblABBRedemption2 { get; set; }
        public virtual tblABBRegistration tblABBRegistration { get; set; }
        public virtual tblCustomerDetail tblCustomerDetail { get; set; }
        public virtual tblExchangeOrderStatu tblExchangeOrderStatu { get; set; }
        public virtual tblUser tblUser { get; set; }
        public virtual tblUser tblUser1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblOrderTran> tblOrderTrans { get; set; }
        public virtual tblVoucherStatu tblVoucherStatu { get; set; }
    }
}
