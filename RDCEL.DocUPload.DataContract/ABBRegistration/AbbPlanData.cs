using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.ABBRegistration
{
    public class AbbPlanData
    {
        public string abbplanname{ get; set; }
        public string noclaimperiod{ get; set; }
        public string abbplanperiod{ get; set; }
        public decimal abbplanfees{ get; set; }
        public decimal BPMargin{ get; set; }
        public decimal BUMargin{ get; set; }
        public decimal BaseValue{ get; set; }
        public decimal Cgst{ get; set; }
        public decimal Sgst{ get; set; }
   }

    public class ABBRegistrationUpdateDataContract
    {
        public string PhoneNumber { get; set;}
        public string ABBPlanName { get; set; }
        public string NoClaimPeriod { get; set; }
        public decimal? abbfees { get; set; }
        public string PlanDuration { get; set; }
        public string ProductNetValue { get; set; }
        [Required(ErrorMessage ="Invoice image required")]
        public string InvoiceImageName { get; set; }
        [Required(ErrorMessage = "Product serial number required")]
        public string ProductSerialNumber { get; set; }
        [Required(ErrorMessage = "Invoice date required")]
        public DateTime InvoiceDate { get; set; }
        [Required(ErrorMessage = "Invoice number  required")]
        public string InvoiceNo { get; set; }
       
        public string InvoiceImage { get; set; }
        [Required]
        public string UploadDateTime { get; set; }
        [Required]
        public bool FollowupCommunication { get; set; }
        [Required]
        public bool OtherCommunications { get; set; }
        [Required(ErrorMessage = "Please enter first name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please enter last name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email  required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        public string Email { get; set; }
        [Required(ErrorMessage = "PinCode  required")]
        [RegularExpression(@"^\d{6}(-\d{5})?$", ErrorMessage = "Invalid Pincode")]
        public string PinCode { get; set; }
        [Required(ErrorMessage = "State  required")]
        public string State { get; set; }
        [Required(ErrorMessage = "City  required")]
        public string City { get; set; }
        [Required(ErrorMessage = "Address1  required")]
        public string Address1 { get; set; }
   
        public string Address2 { get; set; }
        [Required]
        public string Base64StringValue { get; set; }
        public string Productcategory { get; set; }
        public string ProductType { get; set; }
        public string Brand { get; set; }
        public string LogoImage { get; set; }
        public bool paynow { get; set; }
        public bool IsdefferedABB { get; set; }

        public string RegdNo { get; set; }
        public string BUNAme { get; set; }
        public int ProductcategoryId { get; set; }
        public int ProductTypeID { get; set; }
        public int BusinessUnitId { get; set; }
        public string BULogoName { get; set; }

    }

    public class OrderConfirmationDataContract
    {
        public string RegdNo { get; set; }
        public int ABBRegistrationId { get; set; }
        public int  CustomerDetailId { get; set; }
        public string Message { get; set; }
    }
}
