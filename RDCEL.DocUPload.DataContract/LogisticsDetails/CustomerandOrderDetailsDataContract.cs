using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RDCEL.DocUpload.DataContract.LogisticsDetails
{
    public class CustomerandOrderDetailsDataContract
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PhoneNumber { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string Pincode { get; set; }
        public string RegdNo { get; set; }
        public string SponserOrdrNumber { get; set; }
        public string ProductCategory{ get; set; }
        public string ProductType { get; set; }
        public string Brandname { get; set; }
        public string productCost { get; set; }
        public bool IsDtoC { get; set; }
        public bool  IsDeffered { get; set; }
        public string Message { get; set; }
        public string pickupPriority { get; set; }

        public string OrderDate { get; set; }
       
    }

    public class ServicePartnerLogin
    {
        public string RegdNo { get; set; }
        public string priority { get; set; }
        public bool GenerateticketWithutCheckingBalance { get; set; }
        public int ServicePartnerId { get; set; }
        public List<SelectListItem> priorityList { get; set; }
        public bool IsServicePartnerLocal { get; set; }
        public string BULogoName { get; set; }
    }
}
