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
    
    public partial class tblServicePartner
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblServicePartner()
        {
            this.tblLogistics = new HashSet<tblLogistic>();
            this.tblExchangeABBStatusHistories = new HashSet<tblExchangeABBStatusHistory>();
        }
    
        public int ServicePartnerId { get; set; }
        public string ServicePartnerName { get; set; }
        public string ServicePartnerDescription { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> Modifiedby { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<bool> IsServicePartnerLocal { get; set; }
        public Nullable<int> UserId { get; set; }
        public string ServicePartnerRegdNo { get; set; }
        public string ServicePartnerMobileNumber { get; set; }
        public string ServicePartnerAlternateMobileNumber { get; set; }
        public string ServicePartnerEmailID { get; set; }
        public string ServicePartnerAddressLine1 { get; set; }
        public string ServicePartnerAddressLine2 { get; set; }
        public string ServicePartnerPinCode { get; set; }
        public Nullable<int> ServicePartnerCityId { get; set; }
        public Nullable<int> ServicePartnerStateId { get; set; }
        public string ServicePartnerGSTNo { get; set; }
        public string ServicePartnerGSTRegisteration { get; set; }
        public string ServicePartnerBankName { get; set; }
        public string ServicePartnerIFSCCODE { get; set; }
        public string ServicePartnerBankAccountNo { get; set; }
        public Nullable<int> ServicePartnerInsertOTP { get; set; }
        public Nullable<int> ServicePartnerLoginId { get; set; }
        public Nullable<bool> ServicePartnerIsApprovrd { get; set; }
        public string ServicePartnerCancelledCheque { get; set; }
        public Nullable<bool> IConfirmTermsCondition { get; set; }
        public string ServicePartnerAadharfrontImage { get; set; }
        public string ServicePartnerAadharBackImage { get; set; }
        public string ServicePartnerProfilePic { get; set; }
        public string ServicePartnerFirstName { get; set; }
        public string ServicePartnerLastName { get; set; }
        public string ServicePartnerBusinessName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblLogistic> tblLogistics { get; set; }
        public virtual tblUser tblUser { get; set; }
        public virtual tblUser tblUser1 { get; set; }
        public virtual tblUser tblUser2 { get; set; }
        public virtual tblCity tblCity { get; set; }
        public virtual tblState tblState { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblExchangeABBStatusHistory> tblExchangeABBStatusHistories { get; set; }
    }
}
