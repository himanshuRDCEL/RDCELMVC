using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using RDCEL.DocUpload.DataContract.SponsorModel;

namespace RDCEL.DocUpload.DataContract.Voucher
{
    public class VoucherDataContract
    {
        public VoucherDataContract()
        {
            ExchangeOrderDataContract = new ExchangeOrderDataContract();
        }

        public int VoucherVerficationId { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public Nullable<int> ExchangeOrderId { get; set; }
        public Nullable<int> RedemptionId { get; set; }
        public string InvoiceName { get; set; }
        public Nullable<bool> IsDealerConfirm { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string VoucherCode { get; set; }
        public Nullable<bool> IsVoucherused { get; set; }
        public string InvoiceImageName { get; set; }

        public ExchangeOrderDataContract ExchangeOrderDataContract { get; set; }
        public string RNumber { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Mobile Number:")]
        [Required(ErrorMessage = "Mobile Number is required.")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid 10 Digit Mobile Number.")]
        public string PhoneNumber { get; set; }

        public string StateName { get; set; }
        public string CityName { get; set; }
        public List<SelectListItem> CityList { get; set; }
        public List<SelectListItem> StoreList { get; set; }
        public int? BusinessPartnerId { get; set; }
        public int BusinessUnitId { get; set; }


        [DataMember]
        [Display(Name = "Product Group")]
        [Required]
        public int? NewProductCategoryId { get; set; }
        [DataMember]
        [Display(Name = "Product Type")]
        [Required]
        public int? NewProductCategoryTypeId { get; set; }
        [DataMember]
        public string BrandName { get; set; }
        [DataMember]
        public int? NewBrandId { get; set; }
        [DataMember]
        public string ModelName { get; set; }
        [DataMember]
        [Display(Name = "Model Number")]
        [Required]
        public int? ModelNumberId { get; set; }
        public List<SelectListItem> ProductTypeList { get; set; }
        public List<SelectListItem> BrandList{ get; set; }
        public List<SelectListItem> ProductModelList { get; set; }
        public string Base64StringValue { get; set; }
        public decimal ExchangePrice { get; set; }
        public decimal Sweetner { get; set; }

        [Required (ErrorMessage = "Invoice Number is required.")]
        public string InvoiceNumber { get; set; }
        public string InvoiceNumberv { get; set; }

        [Required(ErrorMessage = "Serial Number is required.")]
        public string SerialNumber { get; set; }

        public int ExchangePriceOld { get; set; }
        public int ProductTypeIdf { get; set; }
        public string  DiffrenceAmount { get; set; }
        public bool isDealer { get; set; }
        public bool IsBuMultiBrand { get; set; }
        public string ImageName { get; set; }
        public string BULogoName { get; set; }

    }
}