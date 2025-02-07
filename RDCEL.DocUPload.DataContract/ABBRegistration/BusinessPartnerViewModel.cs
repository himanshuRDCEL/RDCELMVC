using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace RDCEL.DocUpload.DataContract.ABBRegistration
{
    public class BusinessPartnerViewModel
    {
        [DataMember]
        public int BusinessPartnerId { get; set; }
        [DataMember]
        [Required]
        [DisplayName("Store Name *")]
        public string Name { get; set; }
        [DataMember]
        [DisplayName("Description")]
        public string Description { get; set; }
        [DataMember]
        public string QRCodeURL { get; set; }
        [DataMember]
        [Required]
        [DisplayName("Contact Person First Name *")]
        public string ContactPersonFirstName { get; set; }
        [DataMember]
        [Required]
        [DisplayName("Contact Person Last Name *")]
        public string ContactPersonLastName { get; set; }
        [DataMember]
        [Required]
        [DisplayName("Phone Number *")]
        public string PhoneNumber { get; set; }
        [DataMember]
        [Required]
        [DisplayName("Email *")]
        public string Email { get; set; }
        [DataMember]
        [Required]
        [DisplayName("Address Line1 *")]
        public string AddressLine1 { get; set; }
        [DataMember]
        [DisplayName("Address Line2")]
        public string AddressLine2 { get; set; }
        [DataMember]
        [Required]
        [DisplayName("Pincode *")]
        public string Pincode { get; set; }
        [DataMember]
        [DisplayName("City")]
        public string City { get; set; }
        [DataMember]
        [DisplayName("State")]
        public string State { get; set; }
        [DataMember]
        [Required]
        public int? BusinessUnitId { get; set; }
        [DataMember]
        public string QRImage { get; set; }
        [DataMember]
        [Required]
        [DisplayName("Store Code *")]
        public string StoreCode { get; set; }
        [DataMember]
        public bool? IsActive { get; set; }
        [DataMember]
        public int? CreatedBy { get; set; }
        [DataMember]
        public DateTime? CreatedDate { get; set; }
        [DataMember]
        public int? ModifiedBy { get; set; }
        [DataMember]
        public DateTime? ModifiedDate { get; set; }
        [DataMember]
        public Nullable<bool> IsABBBP { get; set; }
        [DataMember]
        public Nullable<bool> IsExchangeBP { get; set; }
        [DataMember]
        public string FormatName { get; set; }
        [DataMember]
        public string BPPassword { get; set; }
        [DataMember]
        public Nullable<bool> IsDealer { get; set; }
        [DataMember]
        public string AssociateCode { get; set; }
        [DataMember]
        public Nullable<bool> IsORC { get; set; }
        [DataMember]
        public Nullable<bool> IsDefferedSettlement { get; set; }


    }


    public class OrderCounViewModel
    {
        public int OrderCount { get; set; }
    }
}