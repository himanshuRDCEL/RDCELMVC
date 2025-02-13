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
    
    public partial class tblBizlogTicket
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblBizlogTicket()
        {
            this.tblImages = new HashSet<tblImage>();
        }
    
        public int Id { get; set; }
        public string BizlogTicketNo { get; set; }
        public string SponsrorOrderNo { get; set; }
        public string ConsumerName { get; set; }
        public string ConsumerComplaintNumber { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }
        public string TelephoneNumber { get; set; }
        public string RetailerPhoneNo { get; set; }
        public string AlternateTelephoneNumber { get; set; }
        public string EmailId { get; set; }
        public string DateOfPurchase { get; set; }
        public string DateOfComplaint { get; set; }
        public string NatureOfComplaint { get; set; }
        public string IsUnderWarranty { get; set; }
        public string Brand { get; set; }
        public string ProductCategory { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string Model { get; set; }
        public string IdentificationNo { get; set; }
        public string DropLocation { get; set; }
        public string DropLocAddress1 { get; set; }
        public string DropLocAddress2 { get; set; }
        public string DropLocCity { get; set; }
        public string DropLocState { get; set; }
        public string DropLocPincode { get; set; }
        public string DropLocContactPerson { get; set; }
        public string DropLocContactNo { get; set; }
        public string DropLocAlternateNo { get; set; }
        public string PhysicalEvaluation { get; set; }
        public string TechEvalRequired { get; set; }
        public string Value { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string TicketPriority { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblImage> tblImages { get; set; }
    }
}
