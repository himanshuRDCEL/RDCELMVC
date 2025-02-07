using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using RDCEL.DocUpload.DAL;

namespace RDCEL.DocUpload.DataContract.ABBRedemption
{
   public class ABBRedemptionDataContract
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Pincode { get; set; }
        public string PhoneNumber { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public decimal? RedemptionValue { get; set; }
        public string NoClaimPeriod { get; set; }
        public DateTime? RedemptionDate { get; set; }
        public decimal? RedemptionPercentage { get; set; }
        public int? RedemptionPeriod { get; set; }
        public decimal? ProductNetPrice { get; set; }
        public string ProductCategory { get; set; }
        public string ProductType { get; set; }
        public string Brand { get; set; }
        public int BrandId { get; set; }
        public int? ProductTypeId { get; set; }
        public int? ProductCatId { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class RedemptionDataContract
    {
        public int RedemptionId { get; set; }
        public int? BusinessUnitId { get; set; }
        public Nullable<int> ABBRegistrationId { get; set; }
        public Nullable<int> ZohoABBRedemptionId { get; set; }
        public Nullable<int> CustomerDetailsId { get; set; }
        public string RegdNo { get; set; }
        public string ABBRedemptionStatus { get; set; }
        public string BULogoName { get; set; }
        public string StoreOrderNo { get; set; }
        public string Sponsor { get; set; }
        public string LogisticsComments { get; set; }
        public string QCComments { get; set; }
        public string InvoiceNo { get; set; }
        public string VoucherCodeExpDateString { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public string InvoiceImage { get; set; }
        public Nullable<int> RedemptionPeriod { get; set; }
        public Nullable<int> RedemptionPercentage { get; set; }
        public Nullable<System.DateTime> RedemptionDate { get; set; }
        public Nullable<decimal> RedemptionValue { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> StatusId { get; set; }
        public Nullable<decimal> FinalRedemptionValue { get; set; }
        public string ReferenceId { get; set; }
        public Nullable<bool> IsVoucherUsed { get; set; }
        public string VoucherCode { get; set; }
        public Nullable<System.DateTime> VoucherCodeExpDate { get; set; }
        public Nullable<int> VoucherStatusId { get; set; }

        public string ErrorMessage { get; set; }
        public List<SelectListItem> TermsandCondition { get; set; }


    }
}
