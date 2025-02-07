using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace RDCEL.DocUpload.DataContract.ABBRegistration
{
    public class ABBProductOrderDataContract
    {

        public int ABBRegistrationId { get; set; }

        public int? BusinessUnitId { get; set; }

        public string RegdNo { get; set; }

        public string SponsorOrderNo { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First Name Required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string CustFirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last Name Required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string CustLastName { get; set; }

        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Mobile Number required.")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid 10 Digit Mobile Number.")]
        public string CustMobile { get; set; }
        [Required]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        public string CustEmail { get; set; }

        [Display(Name = "Address Line 1")]
        [Required]
        public string CustAddress1 { get; set; }

        public string CustAddress2 { get; set; }

        public string Customer_Location { get; set; }

        //[RegularExpression("^[0-9]{1,6}$", ErrorMessage = "Pincode must be 6 digit only")]
        [Display(Name = "PinCode")]
        [Required]
        [RegularExpression(@"^\d{6}(-\d{5})?$", ErrorMessage = "Invalid Pincode")]
        public string CustPinCode { get; set; }

        public string CustCity { get; set; }

        public string CustState { get; set; }

        [Display(Name = "Product Group")]
        [Required]
        public int? NewProductCategoryId { get; set; }

        [Display(Name = "Product Type")]
        [Required]
        public int? NewProductCategoryTypeId { get; set; }

        public int? NewBrandId { get; set; }

        public string NewSize { get; set; }

        public string ProductSrNo { get; set; }

        [Display(Name = "Model Number")]
        [Required]
        public int ModelNumberId { get; set; }

        public string ABBPlanName { get; set; }

        public string HSNCode { get; set; }

        [Display(Name = "Invoice Date")]
        [Required]
        public DateTime? InvoiceDate { get; set; }

        public string InvoiceNo { get; set; }

        public decimal? NewPrice { get; set; }

        public decimal? ABBFees { get; set; }

        public string OrderType { get; set; }

        public string SponsorProdCode { get; set; }

        public int? ABBPriceId { get; set; }

        public DateTime? UploadDateTime { get; set; }

        public int? BusinessPartnerId { get; set; }



        public string YourRegistrationNo { get; set; }

        public string InvoiceImage { get; set; }

        public string ABBPlanPeriod { get; set; }

        [Display(Name = "No Claim Period (Months)")]
        public string NoOfClaimPeriod { get; set; }

        [Display(Name = "Net Product Price")]
        [Required]
        public decimal? ProductNetPrice { get; set; }

        public bool OtherCommunications { get; set; }

        public bool FollowupCommunication { get; set; }

        public bool? IsActive { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }



        public string BULogoName { get; set; }

        public string BUName { get; set; }

        public string StoreName { get; set; }

        public string BrandName { get; set; }

        public string ModelName { get; set; }

        public string Base64StringValue { get; set; }

        public bool PurchaseStatus { get; set; }
        public bool IsPayNow { get; set; }
        public bool? IsdefferedAbb { get; set; }
        public string PlanPrice { get; set; }
        public string transactionId { get; set; }
        public string OrderId { get; set; }
        public string PaymentRemark { get; set; }
        public string Is_ABB_Payment_Done { get; set; }
        public string city1 { get; set; }
        public string state1 { get; set; }
        public bool IsBUD2C { get; set; }
        public bool IsBuMultibrand { get; set; }
        public int bpId { get; set; }
        //public List<SelectListItem> StoreList { get; set; }
        // public List<SelectListItem> ProductTypeList { get; set; }
        //public List<SelectListItem> ProductModelList { get; set; }
        public string imageName { get; set; }
        public string StoreCode { get; set; }
        public decimal BusinessUnitMargin { get; set; }
        public decimal DealerMargin { get; set; }
    }
    public class ABBOrderRequestDataContract
    {
        [Display(Name = "First Name")]
        //[Required(ErrorMessage = "First Name Required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string CustFirstName { get; set; }

        [Display(Name = "Last Name")]
        //[Required(ErrorMessage = "Last Name Required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string CustLastName { get; set; }

        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Mobile Number required.")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid 10 Digit Mobile Number.")]
        public string CustMobile { get; set; }
        //[Required]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        public string CustEmail { get; set; }

        [Display(Name = "Address Line 1")]
        //[Required]
        public string CustAddress1 { get; set; }

        public string CustAddress2 { get; set; }

        public string Customer_Location { get; set; }

        //[RegularExpression("^[0-9]{1,6}$", ErrorMessage = "Pincode must be 6 digit only")]
        [Display(Name = "PinCode")]
        //[Required]
        [RegularExpression(@"^\d{6}(-\d{5})?$", ErrorMessage = "Invalid Pincode")]
        public string CustPinCode { get; set; }

        public string CustCity { get; set; }

        public string CustState { get; set; }

        [Display(Name = "Product Group")]
        [Range(1, int.MaxValue, ErrorMessage = "ProductCategoryId Must be greater than zero")]
        [Required]
        public int? NewProductCategoryId { get; set; }

        [Display(Name = "Product Type")]
        [Range(1, int.MaxValue, ErrorMessage = "ProductTypeId Must be greater than zero")]
        [Required]
        public int? NewProductCategoryTypeId { get; set; }
        //[Required]
        [Display(Name ="Brand")]
        [Range(1,int.MaxValue,ErrorMessage ="Brand Id must be greater than zero")]
        public int? NewBrandId { get; set; }

        public string NewSize { get; set; }

        public string ProductSrNo { get; set; }

        [Display(Name = "Model Number")]
        //[Required]
        public int ModelNumberId { get; set; }

        public string ABBPlanName { get; set; }


        [Display(Name = "Invoice Date")]
       // [Required]
        public DateTime? InvoiceDate { get; set; }
        [Display(Name ="Invoice Number")]
      //  [Required]
        public string InvoiceNo { get; set; }
        //[Required]
        public string ABBPlanPeriod { get; set; }
       // [Required]
        [Display(Name = "No Claim Period (Months)")]
        public string NoOfClaimPeriod { get; set; }
        public string PlanPrice { get; set; }
        [Display(Name = "Net Product Price")]
        [Range(1, double.MaxValue, ErrorMessage = "ProductNetPrice Must be greater than zero")]
        [Required]
        public decimal? ProductNetPrice { get; set; }

        public string BrandName { get; set; }

        public string ModelName { get; set; }
       // [Required]
        public string Base64StringValue { get; set; }
        public string StoreCode { get; set; }

    }

    public class ABBOrderErrorDataContract
    {
        public string errorMessage { get; set; }
    }
    public class ABBOrderResponseDataContract
    {
        public int OrderId { get; set; }
        public string Orderdate { get; set; }
        public string orderRegdNo { get; set; }
        public string RequestErrorMessage { get; set; }
    }
}
