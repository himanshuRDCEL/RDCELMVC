using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Mvc;

namespace RDCEL.DocUpload.DataContract.ABBRegistration
{
    public class ABBViewModel
    {
        [DataMember]
        [Display(Name = "State")]
        public string StateName { get; set; }
        [DataMember]
        [Display(Name = "City")]
        public string CityName { get; set; }
        //[DataMember]        
        //public int StoreId { get; set; }
        //[DataMember]
        //[Display(Name = "Store Name")]
        //public string StoreName { get; set; }
        [DataMember]
        public int BUId { get; set; }
        [DataMember]
        public string BULogoName { get; set; }

        public List<SelectListItem> ProductCategory { get; set; }
        public List<SelectListItem> ProductType { get; set; }
        [Required(ErrorMessage = "Please provide net product value")]

        public decimal? NetProductPrice { get; set; }
        [Required(ErrorMessage = "Please provide product group")]
        public int productCategoryId { get; set; }
        [Required(ErrorMessage = "Please provide product type")]
        public int ProductTypeId { get; set; }
        [Required(ErrorMessage = "Please provide store name")]
        public string StoreName { get; set; }
        public int BusinessUnitId { get; set; }
        public string BusinessLogo { get; set; }
        public string Buname { get; set; }
        [Required(ErrorMessage = "Please select store")]
        public int BusinessPartnerId { get; set; }
        public string planprice { get; set; }
        public string planduration { get; set; }
        public string planName { get; set; }

        public string BaseValue { get; set; }

        public string Cgst { get; set; }
        public string Sgst { get; set; }
       
        public int  bussinessUnitenum { get; set; }
        public bool IsBUD2C { get; set; }
        public bool IsdeferredABB { get; set; }
        public bool ShowAbbPlan { get; set; }
        public int MarginType { get; set; }
        public int GstType { get; set; }

        public string NoClaimPeriod { get; set; }
        [Required(ErrorMessage = "Please provide EmployeeId")]
        public string EmployeeId { get; set; }
        public List<SelectListItem> StoreList { get; set; }
        public List<SelectListItem> CityList { get; set; }
        //public List<SelectListItem> StoreList { get; set; }
    }
    public class ProductDetailsViewModel
    {
        public List<SelectListItem> ProductCategory { get; set; }
        public List<SelectListItem> ProductType { get; set; }
        [Required(ErrorMessage = "Please provide net product value")]

        public decimal? NetProductPrice { get; set; }
        [Required(ErrorMessage = "Please provide product category")]
        public int productCategoryId { get; set; }
        [Required(ErrorMessage = "Please provide product type")]
        public int ProductTypeId { get; set; }
        public int BusinessUnitId { get; set; }
        public string BusinessLogo { get; set; }
        public string Buname { get; set; }
        public int BusinessPartnerId { get; set; }
        public string planprice { get; set; }
        public string planduration { get; set; }
        public string planName { get; set; }
        public string NoClaimPeriod { get; set; }
    }
    public class plandetail
    {
        public string planprice { get; set; }
        public string planduration { get; set; }
        public string planName { get; set; }
        public string NoClaimPeriod { get; set; }
        public string BaseValue { get; set; }
        public string Cgst  { get; set; }
        public string Sgst { get; set; }
        public string Message { get; set; }
        public int AbbDayDiff { get; set; }
    }
    public class abbplanmaster
    {
        public string From_month { get; set; }
        public string To_month { get; set; }
        public string Assured_BuyBack_Percentage { get; set; }
    }
    //class to get state and city name for customer info
    public class abbCustAddress
    {
        public string StateName { get; set; }
        public string CityName { get; set; }
    }
    public class RedirectModel
    {
        public int BusinessUnitId { get; set; }
        public int BusinessPartnerId { get; set; }
        public int BUId { get; set; }
        public string BULogoName { get; set; }
        public bool IsABB { get; set; }
        public bool IsBUABB { get; set; }
        public bool IsBUExchange { get; set; }
        public bool IsExchange { get; set; }
        public bool IsBUD2C { get; set; }
        public bool IsSweetnerModelBased{ get; set;}
        public int ProductModelIdNew { get; set; }
    }
}