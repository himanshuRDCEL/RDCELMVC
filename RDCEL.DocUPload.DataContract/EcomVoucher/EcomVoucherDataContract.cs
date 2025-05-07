using GraspCorn.Common.Enums;
using Newtonsoft.Json;
using RDCEL.DocUpload.DataContract.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUPload.DataContract.EcomVoucher
{
    public class EcomVoucherDataContract :BaseDataContract ,IValidatableObject
    {
        public int EcomVoucherId { get; set; }
        [JsonProperty("VoucherCode")]
        public string VoucherCode { get; set; }
        [JsonProperty("Phoneno")]
        public string Phoneno { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EcomVoucherType == (int)EcomVoucherTypeEnum.BrandSpecificVoucher)
            {
                if (string.IsNullOrWhiteSpace(Phoneno))
                {
                    yield return new ValidationResult("Phoneno is required for Brand Specific Vouchers.", new[] { "Phoneno" });
                }
            }
        }
            [JsonProperty("BrandId")]
        public Nullable<int> BrandId { get; set; }
        [JsonProperty("CategoryIds")]

        public string  CategoryIds { get; set; }
        [JsonProperty("StartDate")]
        public Nullable<System.DateTime> StartDate { get; set; }
        [JsonProperty("EndDate")]
        public Nullable<System.DateTime> EndDate { get; set; }
        [JsonProperty("CompanyId")]
        public Nullable<int> CompanyId { get; set; }
        [JsonProperty("EcomVoucherType")]
        public Nullable<int> EcomVoucherType { get; set; }
        [JsonProperty("VoucherCount")]
        public Nullable<int> VoucherCount { get; set; }
        [JsonProperty("voucherstatus")]
        public string voucherstatus { get; set; }
        [JsonProperty("PhoneSpecificsList")]
        public List<EcomPhoneSpecificsDataContract> EcomPhoneSpecificsListVM { get; set; }
        public List<EcomVoucher> PhoneNumbers { get; set; }
        [JsonProperty("ValueType")]
        public int? ValueType { get; set; }
        [JsonProperty("FixedValue")]
        public int? FixedValue { get; set; }
        [JsonProperty("Percentage")]
        public int? Percentage { get; set; }
        [JsonProperty("PercLimit")]
        public int? PercLimit { get; set; }
        [JsonProperty("success")]
        public bool success { get; set; }
        public bool? IsUsed { get; set; }


    }
    public class EcomVoucher
    {
        [JsonProperty("PhoneNumber")]
        public string PhoneNumber { get; set; }
        [JsonProperty("VoucherCode")]
        public string VoucherCode { get; set; }
    }

    public class ResponseEcomVoucherPriceDC
    {
        public string Message { get; set; }
        public string valueType { get; set; }
        public int? FixedValue { get; set; }
        public int? Percent { get; set; }
        public int? MaxDiscount { get; set; }
       
    }
    public class ResquestEcomVoucherPriceDC
    {
        [JsonProperty("VoucherCode")]
        public string VoucherCode { get; set; }
        [JsonProperty("PhoneNo")]
        public string PhoneNo { get; set; }
        [JsonProperty("BrandId")]
        public int? BrandId { get; set; }
        [JsonProperty("CategoryId")]
        public int? CategoryId { get; set; }

    }

    public class EcomVoucherRedemptionDataContract
    {
        [JsonProperty("VoucherCode")]
        public string VoucherCode { get; set; }
        [JsonProperty("IsPayment")]
        public bool? IsPayment { get; set; }
        public bool? Sucess { get; set; }

        public string Message { get; set; }
    }

}
