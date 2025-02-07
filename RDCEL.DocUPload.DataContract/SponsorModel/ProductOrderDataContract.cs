using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DataContract.ExchangeOrderDetails;

namespace RDCEL.DocUpload.DataContract.SponsorModel
{
    enum ProductConditionCode
    {
        Excellent = 1,
        Good = 2,
        Average = 3,

    }
    public class SocietyDataContract
    {
        public int SocietyId { get; set; }
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
        public Nullable<int> BusinessUnitId { get; set; }

    }
    public class ExchangeOrderDataContract
    {
        [Required(ErrorMessage = "Please provide first name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please provide last name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Please provide email")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please provide city")]
        public string City { get; set; }
        [Required(ErrorMessage = "Please provide pincode")]
        [RegularExpression(@"^\d{6}(-\d{5})?$", ErrorMessage = "Invalid Pincode")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "Please provide pincode")]
        [RegularExpression(@"^\d{6}(-\d{5})?$", ErrorMessage = "Invalid Pincode")]
        public string PinCode { get; set; }
        [Required(ErrorMessage = "Please provide address")]
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Mobile Number:")]
        [Required(ErrorMessage = "Please provide mobile number")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid 10 Digit Mobile Number.")]
        public string PhoneNumber { get; set; }
        public string Bonus { get; set; }
        public string EstimatedDeliveryDate { get; set; }
        public string ProductCondition { get; set; }
        public string CompanyName { get; set; }
        public string BULogoName { get; set; }
        [Display(Name = "Invoice Number")]
        public string SponsorOrderNumber { get; set; }
        public string ZohoSponsorNumber { get; set; }
        public string BrandName { get; set; }
        [Required(ErrorMessage = "Please select appliance brand")]
        public int BrandId { get; set; }
        public int Id { get; set; }
        public int CustomerDetailsId { get; set; }
        [Required(ErrorMessage = "Please select appliance category")]
        public int ProductCategoryId { get; set; }
        [Required(ErrorMessage = "Please select appliance type")]
        public int ProductTypeId { get; set; }
        public int? LoginID { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public int BusinessUnitSponser { get; set; }
        public bool OtherCommunications { get; set; }
        public bool OtherCommunications1 { get; set; }
        public bool FollowupCommunication { get; set; }
        public bool FollowupCommunication1 { get; set; }
        [RegularExpression(@"^([0-9]{4})$", ErrorMessage = "Use only 4 Digit")]
        [Range(2002, 2025, ErrorMessage = "Year value should not be greater then 2002")]
        public int ProductAge { get; set; }
        [Required]
        public int QualityCheck { get; set; }
        [Required]
        public int QualityCheckValue { get; set; }
        public Nullable<bool> IsDtoC { get; set; }
        public int? SocietyId { get; set; }
        public SocietyDataContract SocietyDataContract { get; set; }
        public BusinessUnitDataContract BusinessUnitDataContract { get; set; }
        public List<SelectListItem> ProductTypeList { get; set; }
        public List<SelectListItem> SocietyDataContractList { get; set; }
        public List<SelectListItem> ProductModelList { get; set; }
        public List<SelectListItem> ProductAgeList { get; set; }
        public List<SelectListItem> QualityCheckList { get; set; }
        public List<SelectListItem> StoreList { get; set; }
        public List<SelectListItem> CityList { get; set; }
        public List<SelectListItem> AreaLocalityList { get; set; }
        public List<SelectListItem> BrandList { get; set; }
        public string RegdNo { get; set; }
        [Required(ErrorMessage = "Please select store")]
        public Nullable<int> BusinessPartnerId { get; set; }

        [Display(Name = "Sales Associate Name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string SaleAssociateName { get; set; }
        [Display(Name = "Sales Associate Code")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
        public string SaleAssociateCode { get; set; }
        public string BUName { get; set; }
        [Required]
        public int? BusinessUnitId { get; set; }
        public int? ExpectedDeliveryHours { get; set; }
        public string StoreCode { get; set; }
        public List<SelectListItem> PurchasedProductCategoryList { get; set; }
        public string PurchasedProductCategory { get; set; }
        public string Name { get; set; }
        public Nullable<bool> IsDelivered { get; set; }
        public List<SelectListItem> PincodeList { get; set; }
        public string ZohoSponsorOrderId { get; set; }

        public string AssociateName { get; set; }
        public string AssociateEmail { get; set; }
       
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid 10 Digit Mobile Number.")]
        public string StorePhoneNumber { get; set; }
        public string FormatName { get; set; }
        public decimal ExchangePrice { get; set; }
        public decimal Sweetner { get; set; }
        public string ExchangePriceString { get; set; }
        public string BasePrice { get; set; }
        //public string Purchased_Product_Category { get; set; }
        public string VoucherCode { get; set; }
        public Nullable<System.DateTime> VoucherCodeExpDate { get; set; }
        public string VoucherCodeExpDateString { get; set; }

        public string StateName { get; set; }
        public int? VoucherStatusId { get; set; }
        public string CityName { get; set; }
        public int Month { get; set; }
        public string Month1 { get; set; }
        public int Year { get; set; }
        public string Year1 { get; set; }
        public string AssociateCode { get; set; }
        public List<SelectList> YearList { get; set; }
        public List<SelectList> MonthList { get; set; }
        public string AmountStatus { get; set; }
        public int NewProductCategoryId { get; set; }
        public int NewProductCategoryTypeId { get; set; }
        public int NewBrandId { get; set; }
        public string NewBrandName { get; set; }
        public int ModelNumberId { get; set; }
        [Required(ErrorMessage = "Please provide invoice number")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Use letters only please")]
        public string InvoiceNumber { get; set; }
        public string InvoiceImageName { get; set; }
        public string Base64StringValue { get; set; }
        public tblBusinessPartner businessPartner { get; set; }
        public string Comment1 { get; set; }
        public string Comment2 { get; set; }
        public string Comment3 { get; set; }
        public string ProductNumber { get; set; }
        public string ProductName { get; set; }
        public int productBrandID { get; set; }
        public string IsUnInstallationRequired { get; set; }
        public Nullable<decimal> UnInstallationPrice { get; set; }
        public string stringUnInstallationPrice { get; set; }

        //Time Slot
        public string StartTime { get; set; }
        public List<SelectListItem> StartTimeList { get; set; }
        public List<SelectListItem> TermsandCondition { get; set; }
        public string OrderStatus { get; set; }
        public string ProductSize { get; set; }
        public string EndTime { get; set; }
        public string QCDate { get; set; }
        public string ProductCategory { get; set; }
        public string ProductType { get; set; }
        public string OldBrand { get; set; }
        public string NewProductType { get; set; }
        public string NewProductCategory { get; set; }
        public string NewBrand { get; set; }
        public string OrderCreatedOn { get; set; }
        public int StatusId { get; set; }
        public string PickupStatus { get; set; }
        public decimal? Sweetener { get; set; }
        public string Condition1 { get; set; }
        public string Condition2 { get; set; }
        public string Condition3 { get; set; }
        public string Condition4 { get; set; }
        public string City1 { get; set; }
        public string State1 { get; set; }
     [Required(ErrorMessage = "Please provide state")]
        public string State { get; set; }
        public string PriceCode { get; set; }
        public bool IsOtpRequired { get; set; }
        public bool IsOrc { get; set; }
        public bool IsD2C { get; set; }
        public string PhoneNo { get; set; }
        public bool IsDifferedSettlement { get; set; }
        public bool IsSweetnerModelBased { get; set; }
        public bool IsCustomerAcceptenceRequired { get; set; }
        public bool IsCustomerEmailRequired { get; set; }
        public bool IsQualityRequiredOnUi{ get; set; }
        public bool IsInvoiceDetailsReqiured { get; set; }
        public bool IsNewProductDetailsReqiured { get; set; }
        public bool IsModelNumberRequired { get; set; }
        public bool IsVoucher { get; set; }
        public bool IsNewBrandRequired { get; set; }
        public int VoucherType { get; set; }
        public int voucherCash { get; set; }
        public int voucherDiscount { get; set; }
        public string ImageName { get; set; }
        public string Response { get; set; }
        public string StoreType { get; set; }
        public int  businessUnitForHidingModelNumberAndInvoiceData { get; set; }
        public string mobileNoWithCountryCode { get; set; }
        public int ProductConditionCount { get; set; }
        public int NewBrandIdDefault { get; set; }

        [Required(ErrorMessage = "Please provide Area Locality")]
        //public int AreaLocality { get; set; }
        public string AreaLocality { get; set; }
        public string AreaLocalityName { get; set; }
        //Added by VK
        public bool IsValidationBasedSweetner { get; set; }
        public List<BUBasedSweetnerValidation> buBasedSweetnerValidationsList { get; set; }
        public int ProductModelIdNew { get; set; }
        public string ProductModelNew { get; set; }
        [Required]
        public string EmployeeId { get; set; }
        public bool? showEmployeeId { get; set; }

        public string ModelNumber { get; set; }
        public int priceMasterNameID { get; set; }
        public decimal? SweetenerBu { get; set; }
        public decimal? SweetenerBP { get; set; }
        public decimal? SweetenerDigi2L { get; set; }
        public decimal? SweetenerTotal { get; set; }
        public bool? IsSponsorLogorequiredOnUI { get; set; }
        public bool? IsProductSerialNumberRequired { get; set; }
        [Required(ErrorMessage = "Please provide product serial number")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Use letters only please")]
        public string ProductSerialNumber { get; set; }
        public bool? IsSFIDRequired { get; set; }
        public string SponsorServiceRefId { get; set; }
        public bool? IsUnInstallation { get; set; }
        public bool? IsDefaultPickupAddress { get; set; }
        public bool? IsCouponAplied { get; set; }
        public int? CouponId { get; set; }
        public string UsedCouponCode { get; set; }
        public decimal? CouponValue { get; set; }
    }
    public class GenerateVoucherCode
    {
        public string VoucherCode { get; set; }
        public bool IsVoucherused { get; set; }
        public DateTime? VoucherCodeExpDate { get; set; }
    }

    public class ExchagneViewModel
    {
        [Required(ErrorMessage = "Please select state")]
        [Display(Name = "State")]
        public string StateName { get; set; }
        [Required(ErrorMessage = "Please select city")]
        [Display(Name = "City")]
        public string CityName { get; set; }
        [Required(ErrorMessage = " Format Required!")]
        [Display(Name = "Format")]
        public string FormatName { get; set; }
        [Required]
        public int BUId { get; set; }
        public string BULogoName { get; set; }
        [Required(ErrorMessage = "Please provide valid pincode of store location")]
        [RegularExpression(@"^\d{6}(-\d{5})?$", ErrorMessage = "Please provide valid pincode of store location")]
        public string ZipCode { get; set; }
        [Required(ErrorMessage = "Please select store")]
        public Nullable<int> BusinessPartnerId { get; set; }
        public int OldProductCategoryId { get; set; }
        public int oldProductypeId { get; set; }
        public int OldBrandId { get; set; }
        public int NewCategoryId { get; set; }
        public int NewTypeId { get; set; }
        public int NewBrandId { get; set; }
        public int ProductAge { get; set; }
        public int QualityCheckValue { get; set; }
        public bool IsSweetnerModelBased { get; set; }
        public bool IsD2C { get; set; }
        public bool IsDeffered { get; set; }
        public bool IsVoucher { get; set; }
        public int VoucherType { get; set; }
        public string BUsinessUnitLogoName { get; set; }
        public string ExchangePrice { get; set; }
        public string BasePrice { get; set; }
        public List<SelectListItem> StateList { get; set; }
        public List<SelectListItem> CityList { get; set; }
        public List<SelectListItem> PincodeList { get; set; }
        public List<SelectListItem> StoreList { get; set; }
        public List<CityViewModalforMetroCities> metroCities { get; set; }
        //Added by VK
        public int ProductModelIdNew { get; set; }
        public string ProductModelNew { get; set; }
        [Required(ErrorMessage ="Please provide employee id")]
        public string EmployeeId { get; set; }
        public bool? showEmployeeId { get; set; }
        public bool IsValidationBasedSweetner { get; set; }

        public int priceMasterNameID { get; set; }
        public int ModelNumberId { get; set; }
        public decimal? SweetenerBu { get; set; }
        public decimal? SweetenerBP { get; set; }
        public decimal? SweetenerDigi2L { get; set; }
        public decimal? SweetenerTotal { get; set; }

        public int? CityId { get; set; }
        public bool? IsBPAssociated { get; set; }
        public bool? IsCouponAplied { get; set; }
        public int? CouponId { get; set; }
        public string UsedCouponCode { get; set; }
        public decimal? CouponValue { get; set; }
        public bool? IsCouponsAvailable { get; set; }
    }

    public class ProductOrderDataContract
    {
        
        public string FirstName { get; set; }
      
        public string LastName { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string State { get; set; }
     
        public string ZipCode { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        [Required]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid 10 Digit Mobile Number.")]
        public string PhoneNumber { get; set; }
        //public string ProductTypeId { get; set; }
        [Required]
        [Range(1, 10000, ErrorMessage = "BrandId is required.")]
        public int BrandId { get; set; }
        public string Bonus { get; set; }
        [Required(ErrorMessage ="Estimate Delivery Date Required (Preferred Format: dd-MMM-yyyy)")]
        public string EstimatedDeliveryDate { get; set; }
        [Required]
        public string ProductCondition { get; set; }
        public int Id { get; set; }
        [Required]
        public string CompanyName { get; set; }
        public int CustomerDetailsId { get; set; }
        [Required]
        [Range(1, 10000, ErrorMessage = "ProductTypeId  is required.")]
        public int ProductTypeId { get; set; }
        public string SponsorOrderNumber { get; set; }
        public int LoginID { get; set; }
        public string RegdNo { get; set; }
        public string UploadDateTime { get; set; }
        public string StoreCode { get; set; }
        public int BusinessPartnerId { get; set; }
        public int BUId { get; set; }
        public bool IsDefferedSettlement { get; set; }
        public bool? IsRedemptionInstant { get; set; }
        public bool? IsSweetenerModelBased { get; set; }
        public int voucherType { get; set; }
        public bool IsVoucher { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string QCDate { get; set; }        
        public string VoucherCode { get; set; }        
        public string AreaLocality { get; set; }
        public int OrderType { get; set; }
        public int ModelId { get; set; }
        public int NewCatId { get; set; }
        public int NewTypeId { get; set; }
        public int NewBrandId { get; set; }
        public decimal? ExchangePrice { get; set; }
        public decimal? Sweetener { get; set; }
        public decimal? SweetenerBU { get; set; }
        public decimal? SweetenerBp { get; set; }
        public decimal? SweetenerDigi2l { get; set; }
        public decimal? BasePrice { get; set; }
        public int priceMasterNameID { get; set; }
    }

    public class ProductOrderResponseDataContract
    {
        public int OrderId { get; set; }
        public int? BusinessUnitId { get; set; }
        public int? BusinessPartnerId { get; set; }
        public string RegdNo { get; set; }
        public string ZohoSponsorId { get; set; }
    }
    public class MyGateViewModel
    {
        public string url { get; set; }
        public BusinessUnitDataContract BusinessUnitDataContract { get; set; }

        [Required(ErrorMessage = "Please enter your appliance category")]
        public int ProductCategoryId { get; set; }
        [Required(ErrorMessage = "Please enter the appliance type")]
        public int ProductTypeId { get; set; }
        [Required(ErrorMessage = "Please enter the product brand ")]
        public int BrandId { get; set; }
        [Required(ErrorMessage = "Please select quality of your appliance ")]
        public int QualityCheck { get; set; }
        public List<SelectListItem> ProductTypeList { get; set; }
        public int BUId { get; set; }
        public int BusinessUnitId { get; set; }
        public string FormatName { get; set; }
        public List<SelectListItem> ProductModelList { get; set; }
        public List<SelectListItem> ProductAgeList { get; set; }

        public List<SelectListItem> QualityCheckList { get; set; }
        public int ProductAge { get; set; }
        public string IsUnInstallationRequired { get; set; }
        public string EndTime { get; set; }
        public string QCDate { get; set; }
        public string StartTime { get; set; }
        public string UnInstallationAmount { get; set; }
        public List<SelectListItem> BrandList { get; set; }
        public string BULogoName { get; set; }
        public string ExchangePriceString { get; set; }
        public int BusinessPartnerId { get; set; }
        public int ProductConditionCount { get; set; }
       
        public string PriceCode { get; set; }
        public bool IsSweetnerModelBased { get; set; }
        public bool IsQualityConditionWorkingNOnWorking { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string mobileNo { get; set; }
        public string mobileNoWithCountryCode { get; set; }
        public string email { get; set; }
        public bool? ShowQCTimeandDatepage { get; set; }

        public int priceMasterNameID { get; set; }
        public string BasePrice { get; set; }  
        public decimal? SweetenerBu { get; set; }
        public decimal? SweetenerBP { get; set; }
        public decimal? SweetenerDigi2L { get; set; }
        public decimal? SweetenerTotal { get; set; }

    }
    public class MyGateQCViewModel
    {
        public string url { get; set; }
        public BusinessUnitDataContract BusinessUnitDataContract { get; set; }
        public int ProductCategoryId { get; set; }
        public int ProductTypeId { get; set; }
        public int SamsungBU { get; set; }
        public int WhirlPoolBU { get; set; }
        public int BrandId { get; set; }
        public int QualityCheck { get; set; }
        public int BUId { get; set; }
        public string FormatName { get; set; }
        public int ProductAge { get; set; }
        public string BULogoName { get; set; }
        public string ExchangePriceString { get; set; }
        [Required(ErrorMessage = "Please Select Time Of Availability")]
        public string StartTime { get; set; }
        public string QCTime { get; set; }
        public IEnumerable<SelectListItem> StartTimeList { get; set; }
        public List<SelectListItem> TypeOfTV { get; set; }
        [Required(ErrorMessage = "Please Select Mount Position Of TV")]
        public string Type { get; set; }
        public string IsUnInstallationRequired { get; set; }

        public string UnInstallationAmount { get; set; }
        public string EndTime { get; set; }
        [Required(ErrorMessage = "Please Select Date Of Availability")]
        public string QCDate { get; set; }
        public string QCDate1 { get; set; }
        public int BusinessPartnerId { get; set; }
        public string PriceCode { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string mobileNo { get; set; }
        public string email { get; set; }
        public string mobileNoWithCountryCode { get; set; }
        public int priceMasterNameID { get; set; }
        public string BasePrice { get; set; }
        public decimal? SweetenerBu { get; set; }
        public decimal? SweetenerBP { get; set; }
        public decimal? SweetenerDigi2L { get; set; }
        public decimal? SweetenerTotal { get; set; }

    }
    public class MyGateCityState
    {
        public string StateName { get; set; }
        public string CityName { get; set; }        
        public string AreaLocalityName { get; set; }        
    }
    public class MyGateUnInstallation
    {
        public string Type { get; set; }
        public string Price1 { get; set; }
    }
    #region To Confirm the Delivery
    public class ExchangeorderConfirmation
    {
        public int BUId { get; set; }
        public string BULogoName { get; set; }
        public int ExchangeOrderid { get; set; }
        public string ZohoOrderId { get; set; }
        public bool IsDelivered { get; set; }
    }
    #endregion


    #region Exchange Dashboard
    public class ExchangeDashBoardViewModel
    {
        public string BULogoName { get; set; }
        public string BuName { get; set; }
        //[DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        //[DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public List<tblBusinessUnit> BusinessPartenerList { get; set; }
    }
    #endregion

    #region Model for Daikin Soap service request Exchange Order 

    public class ProductOrderSoapServiceRequest{
        public string TicketCategory { get; set; }
        public string SubType { get; set; }
        public string Status { get; set; }
        public string CustomerId { get; set; }
        public string Branch { get; set; }
        public string InstalledBaseId { get; set; }
        public string TypeCode { get; set; }
        public string Content { get; set; }
        public string WarrantyStatus { get; set; }
        public string Product_Type { get; set; }
        public string SourceOfCall { get; set; }
        public string MaterialID { get; set; }
        public string ProductSerialID { get; set; }
        public string EmployeeId { get; set; }
    }
    #endregion



    #region Dealer Dashboard
    public class AllStoresOfOnedealer
    {
        public string VoucherCode { get; set; }
        public string Description { get; set; }
        public string ExchangePrice { get; set; }
        public string SweetnerForDTD { get; set; }

    }

    #endregion



    public class CityViewModalforMetroCities
    {
        public int CityId { get; set; }
        public string Name { get; set; }
        public Nullable<bool> isMetro { get; set; }
        public string cityLogo { get; set; }
    }

    public class NewBrandDataContract
    {
        public string Errormessage { get; set; }
        public List<NewBrandList> brandlist { get; set; }
    }

    public class NewBrandList
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }

    public  class GetNewbrandDC
    {
        public int? BusinessPartnerId { get; set; }
        public int? BusinessUnitId { get; set; }
    }
}
