using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RDCEL.DocUpload.DataContract.ExchangeOrderDetails
{
    public class ExchangeOrderDetail
    {
        public Exchangedetail exchangedetail { get; set; }
        public CustomerDetail customerdetail { get; set; }
        public OTP otp { get; set; }
    }

    public class Exchangedetail
    {
        public int OrderId { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public string Bonus { get; set; }
        public string ProductCondition { get; set; }
        public string RegdNo { get; set; }
        public string VoucherCode { get; set; }
        public string VoucherDiscountPrice { get; set; }
        public int ProductTypeId { get; set; }
        public string ProductTypeDescription { get; set; }
        public int ProductCategoryId { get; set; }
        public string ProductCategoryName { get; set; }
        public string ProductCategoryDescription { get; set; }

    }

    public class CustomerDetail
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ZipCode { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
    }
    public class OTP
    {
        public string Otp { get; set; }
    }


    public class ProductDetailsToExchange
    {
        [Required(ErrorMessage = "Please select appliance category")]
        public int OldProductCatId { get; set; }
        [Required(ErrorMessage = "Please select appliance type")]
        public int OldTypeId { get; set; }
        [Required(ErrorMessage = "Please select appliance brand")]
        public int BrandId { get; set; }
        public int NewBrandId { get; set; }
        [Required(ErrorMessage = "Please select appliance condition")]
        public int QualityCheck { get; set; }
        [Required(ErrorMessage = "Please select new appliance type")]
        public int NewProductTypeId { get; set; }
        [Required(ErrorMessage = "Please select new appliance category")]
        public int NewProductCategoryId { get; set; }
        public int BusinessUnitId { get; set; }
        public int BusinessPartnerId { get; set; }
        public int ProductAge { get; set; }
        public string BusinessUnitLogo { get; set; }
        public string BULogoName { get; set; }
        public string BusinesUnitName { get; set; }
        public string ExchangePrice { get; set; }
        public string BasePrice { get; set; }
        public string FormatName { get; set; }
        public string url { get; set; }
        public string priceCode { get; set; }
        public bool IsSweetnerModelBased { get; set; }
        public bool IsSweetnerBasedonModal { get; set; }

        public bool IsOldProductBaseSweetener { get; set; }
        public bool IsQualityRequiredOnUI { get; set; }
        public bool IsNewProductDetailsRequired { get; set; }
        public bool IsBuMultiBrand { get; set; }
        public bool IsOrc { get; set; }
        public bool IsDeffered { get; set; }
        public bool IsD2C { get; set; }
        public bool IsVoucher { get; set; }
        public bool IsNewBrandRequired { get; set; }
        public int VoucherType { get; set; }
        public int ProductConditionCount { get; set; }
        public int BusinessUnitForHidingQualityCheck { get; set; }
        public List<SelectListItem> QualityCheckList { get; set; }
        public List<SelectListItem> OldProductTypeList { get; set; }
        public List<SelectListItem> NewProductTypeList { get; set; }
        public List<SelectListItem> BrandList { get; set; }
        public List<SelectListItem> NewBrandList { get; set; }
        public List<ProductDetailsList> productDetailsList { get; set; }
        //Added by VK
        public bool IsValidationBasedSweetner { get; set; }
        public List<BUBasedSweetnerValidation> buBasedSweetnerValidationsList { get; set; }
        public int voucherCash { get; set; }
        public bool IsModelNumberRequired { get; set; }
        [Required(ErrorMessage = "Please select model number")]
        public int ModelNumberId { get; set; }
        public List<SelectListItem> ProductModelList { get; set; }
        //Added by VK
       
        public int ProductModelIdNew { get; set; }
        public string ProductModelNew { get; set; }

        public int priceMasterNameID { get; set; }
        public decimal? SweetenerBu { get; set; }
        public decimal? SweetenerBP { get; set; }
        public decimal? SweetenerDigi2L { get; set; }
        public decimal? SweetenerTotal { get; set; }
        public bool? IsCouponAplied { get; set; }
        public int? CouponId { get; set; }
        public string UsedCouponCode { get; set; }
        public decimal? CouponValue { get; set; }
        public bool? IsCouponsAvailable { get; set; }
    }

    public class LodhaGroupHaders
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string mobileNo { get; set; }
        public string email { get; set; }

    }

    public class HeaderValuesLodha
    {
        public string Beleviee { get; set; }
        public string bellevieData { get; set; }
        public string apiKey { get; set; }
    }

    public class HeaderVariables
    {
        public string thirdParty { get; set; }
        public string bellevieVI { get; set; }
        public string belleviedata { get; set; }
    }

    public class ProductDetailsList
    {
        public int ProductCatId { get; set; }
        public int ProductTypeId { get; set; }
        public int BrandId { get; set; }
        public int ProductQualityId { get; set; }
        public int NewProductcatId { get; set; }
        public int NewProducttypeId { get; set; }
        public string ProductPrice { get; set; }
    }
    public class BUBasedSweetnerValidation 
    {
        public int Id { get; set; }
        public int BusinessUnitId { get; set; }
        public int QuestionId { get; set; }
        public string Question { get; set; }
        public bool IsDisplay { get; set; }
        public bool IsRequired { get; set; }
    }
}
