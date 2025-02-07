using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.SponsorModel
{
    public class BusinessUnitDataContract
    {
        public int BusinessUnitId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string RegistrationNumber { get; set; }
        public string QRCodeURL { get; set; }
        public string LogoName { get; set; }
        public string ContactPersonFirstName { get; set; }
        public string ContactPersonLastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Pincode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZohoSponsorId { get; set; }
        public bool IsBUMultiBrand { get; set; }
        public Nullable<int> ExpectedDeliveryHours { get; set; }
        public bool IsAreaLocality { get; set; }
    }
}
