using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Mvc;

namespace RDCEL.DocUpload.DataContract.ABBRegistration
{
    public class ABBRegistrationViewModel
    {
        [DataMember]
        public int ABBRegistrationId { get; set; }
        [DataMember]
        public int? BusinessUnitId { get; set; }
        [DataMember]
        public string RegdNo { get; set; }
        [DataMember]
        [Required(ErrorMessage = "Please enter sponsor order number")]
        public string SponsorOrderNo { get; set; }
        [DataMember]
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Please enter first name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string CustFirstName { get; set; }
        [DataMember]
        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Please enter last name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string CustLastName { get; set; }
        [DataMember]
        [Display(Name = "Phone Number")] 
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Please enter mobile number")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid 10 Digit Mobile Number.")]
        public string CustMobile { get; set; }
        [Required(ErrorMessage = "Please enter email")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        public string CustEmail { get; set; }
        [DataMember]
        [Display(Name = "Address Line 1")]
        [Required(ErrorMessage = "Please enter address")]
        public string CustAddress1 { get; set; }
        [DataMember]
        public string CustAddress2 { get; set; }
        [DataMember]
        public string Customer_Location { get; set; }
        [DataMember]
        //[RegularExpression("^[0-9]{1,6}$", ErrorMessage = "Pincode must be 6 digit only")]
        [Display(Name = "PinCode")]
        [Required(ErrorMessage = "Please enter pincode")]
        [RegularExpression(@"^\d{6}(-\d{5})?$", ErrorMessage = "Invalid Pincode")]
        public string CustPinCode { get; set; }
        [DataMember]
       
        public string CustCity { get; set; }
        [DataMember]
        
        public string CustState { get; set; }
        [DataMember]
        [Display(Name = "Product Group")]
        [Required]
        public int? NewProductCategoryId { get; set; }
        [DataMember]
        [Display(Name = "Product Type")]
        [Required]
        public int? NewProductCategoryTypeId { get; set; }
        [DataMember]
        [Required (ErrorMessage ="Brand Is required")]
        public int? NewBrandId { get; set; }
        [DataMember]
        public string NewSize { get; set; }
        [DataMember]
        [Required(ErrorMessage ="Please enter product serial number")]
        public string ProductSrNo { get; set; }
        [DataMember]
        [Display(Name = "Model Number")]
        [Required(ErrorMessage = "Please enter model number")]
        public int ModelNumberId { get; set; }
        [DataMember]
        public string ABBPlanName { get; set; }
        [DataMember]
        public string HSNCode { get; set; }
        [DataMember]
        [Display(Name = "Invoice Date")]
        [Required(ErrorMessage = "Please enter invoice date")]
        public DateTime? InvoiceDate { get; set; }
        [DataMember]
        [Required(ErrorMessage = "Please enter invoice number")]
        public string InvoiceNo { get; set; }
        [DataMember]
        public decimal? NewPrice { get; set; }
        [DataMember]
        public decimal? ABBFees { get; set; }
        [DataMember]
        public string OrderType { get; set; }
        [DataMember]
        public string SponsorProdCode { get; set; }
        [DataMember]
        public int? ABBPriceId { get; set; }
        [DataMember]
        public DateTime? UploadDateTime { get; set; }
        [DataMember]
        public int? BusinessPartnerId { get; set; }
        [DataMember]
        public string ZohoABBRegistrationId { get; set; }
        [DataMember]
        public string YourRegistrationNo { get; set; }
        [DataMember]
        public string InvoiceImage { get; set; }
        [DataMember]
        public string ABBPlanPeriod { get; set; }
        [DataMember]
        [Display(Name = "No Claim Period (Months)")]
        public string NoOfClaimPeriod { get; set; }
        [DataMember]
        [Display(Name = "Net Product Price")]
        [Required]
        public decimal? ProductNetPrice { get; set; }
        [DataMember]
        public bool OtherCommunications { get; set; }
        [DataMember]
        public bool FollowupCommunication { get; set; }
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
        public string BULogoName { get; set; }
        [DataMember]
        public string BUName { get; set; }
        [DataMember]
        public string StoreName { get; set; }
        [DataMember]
        public string BrandName { get; set; }
        [DataMember]
        public string ModelName { get; set; }
        [DataMember]
    
        public string Base64StringValue { get; set; }
        [DataMember]
        public bool PurchaseStatus { get; set; }
        public bool IsPayNow { get; set; }
        public bool IsdefferedAbb { get; set; }
        public string PlanPrice { get; set; }
        public string transactionId { get; set; }
        public string OrderId { get; set; }
        public string PaymentRemark { get; set; }
        public string Is_ABB_Payment_Done { get; set; }
        public string city1{ get; set; }
        public string state1 { get; set; }
        public bool IsBUD2C { get; set; }
        public bool IsBuMultibrand { get; set; }
        public int bpId { get; set; }
        public int CustomerDetailsId { get; set; }
        public bool IsD2C { get; set; }
        public bool IsSponsorOrderNumberRequired { get; set; }
        public string StoreCode { get; set; }
        public int model { get; set; }
        public int GstType { get; set; }
        public int Margintype { get; set; }
        public decimal BusinessUnitMargin { get; set; }
        public decimal BusinessPartnerMargin { get; set; }
        public List<SelectListItem> StoreList { get; set; }
        public List<SelectListItem> ProductTypeList { get; set; }
        public List<SelectListItem> ProductModelList { get; set; }
        public string imageName { get; set; }
        public string EmployeeId { get; set; }
        public string BaseValue { get; set; }
        public string Cgst { get; set; }
        public string Sgst { get; set; }

        //Added by VK
        public string BillNumber { get; set; }
        public string BillCounterNum { get; set; }
        public string FinancialYear { get; set; }

        //Added by Priyanshi
        public int AbbDayDiff { get; set; }
        public bool IsAbbDayConfig { get; set; }

    }

    public class OrderModel
    {
      public string theme { get; set; }
      public string orderToken { get; set; }
      public string channelId { get; set; }
      public string paymentMode { get; set; }
      public string countryCode { get; set; }
      public string mobileNumber { get; set; }
      public string emailId { get; set; }
      public string OrderId { get; set; }
      public string PaymentId { get; set; }
      public string errorcode { get; set; }
      public string errrorResponse { get; set; }
      public decimal amount { get; set; }
      public bool showSavedCardsFeature { get; set; }
      public string BULogoName { get; set; }
    }

    public class PaymentInitiateModel
    {
        public string Custname { get; set; }
        public string Custemail { get; set; }
        public string CustcontactNumber { get; set; }
        public string Custaddress { get; set; }
        public decimal planamount { get; set; }
        public string planPrice { get; set; }
        public string orderRegdNo { get; set; }
        public string description { get; set; }
        public int productcategoryId { get; set; }
        public int productTypeId { get; set; }
        public string newproductCategory { get; set; }
        public string newProductType { get; set; }
        public string custfirstname { get; set; }
        public string custlastname { get; set; }
        public string Custaddress1 { get; set; }
        public string Custaddress2 { get; set; }
        public string address3 { get; set; }
        public string custpin_code { get; set; }
        public string custcity { get; set; }
        public string custstate { get; set; }
        public string custcountry { get; set; }
        public string ModuleType { get; set; }
        public string ModuleTypeEnum { get; set; }
        [Required(ErrorMessage= "Please provide invoice number")]
        public string InvoiceNo { get; set; }
        public string InvoiceImage { get; set; }
        public string Base64StringValue { get; set; }

    }
    public class paymentInitialViewModel
    {
        public string name { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string email { get; set; }
        public string contactNumber { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string address { get; set; }
        public decimal amount { get; set; }
        public string planPrice { get; set; }
        public string RegdNo { get; set; }
        public int productcategoryId { get; set; }
        public int productTypeId { get; set; }
        public string productCategory { get; set; }
        public string ProductType { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string pincode { get; set; }
        public string ModuleName { get; set; }
    }
    public class PaymentResponseModel
    {
        public string transactionId { get; set; }
        public string OrderId { get; set; }
        public string status { get; set; }
        public string RegdNo { get; set; }
        public decimal amount { get; set; }
        public int responseCode { get; set; }
        public string responseDescription { get; set; }
        public string checksum { get; set; }
        public string paymentMode { get; set; }
        public string cardId { get; set; }
        public string cardScheme { get; set; }
        public string cardToken { get; set; }
        public string bank { get; set; }
        public string bankid { get; set; }
        public string paymentmethod { get; set; }
        public string cardhashId { get; set; }
        public string productDescription { get; set; }
        public string pgTransId { get; set; }
        public string gatewayTransactionId { get; set; }
        public string pgTransTime { get; set; }
        public string orderStatus { get; set; }
        

    }
    public class pluralgatewayKey
    {
        public string merchantId { get; set; }
        public string secretKey { get; set; }
        public string accessCode { get; set; }
    }
    public class ZaakpayKey
    {
        public string merchantId { get; set; }
        public string secretKey { get; set; }
    }
    public class ZaakPayResponseModel
    {
        public string orderId { get; set; }
        public int responseCode { get; set; }
        public string responseDescription { get; set; }
        public string checksum { get; set; }
        public string amount { get; set; }
        public string paymentMode { get; set; }
        public string cardId { get; set; }
        public string cardScheme { get; set; }
        public string cardToken { get; set; }
        public string bank { get; set; }
        public string bankid { get; set; }
        public string paymentmethod { get; set; }
        public string cardhashId { get; set; }
        public string productDescription { get; set; }
        public string pgTransId { get; set; }
        public string pgTransTime { get; set; }
        public string RegdNo { get; set; }
    }
    public class ABBRegistrationResponseModel
    {
        public int RegistrationId { get; set; }
        public string  Messsage { get; set; }
    }

    public class ABBPlanMargin
    {
        public decimal? BusinessPartnerMargin { get; set; }
        public decimal? BusinessUnitMargin { get; set; }
        public decimal? abbFees { get; set; }
        public decimal? BaseValue { get; set; }
        public decimal? Cgst { get; set; }
        public decimal? Sgst { get; set; }
    }
    
}