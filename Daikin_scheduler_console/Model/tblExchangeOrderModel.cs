﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCEL.DocUpload.DAL;

namespace Daikin_scheduler_console.Model
{
    public class tblExchangeOrderModel
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string ZohoSponsorOrderId { get; set; }
        public string OrderStatus { get; set; }
        public Nullable<int> CustomerDetailsId { get; set; }
        public Nullable<int> ProductTypeId { get; set; }
        public Nullable<int> BrandId { get; set; }
        public string Bonus { get; set; }
        public string SponsorOrderNumber { get; set; }
        public string EstimatedDeliveryDate { get; set; }
        public string ProductCondition { get; set; }
        public Nullable<int> LoginID { get; set; }
        public string ExchPriceCode { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<bool> IsDtoC { get; set; }
        public Nullable<int> SocietyId { get; set; }
        public string RegdNo { get; set; }
        public Nullable<int> BusinessPartnerId { get; set; }
        public string SaleAssociateName { get; set; }
        public string SaleAssociateCode { get; set; }
        public string PurchasedProductCategory { get; set; }
        public string StoreCode { get; set; }
        public Nullable<bool> IsDelivered { get; set; }
        public string VoucherCode { get; set; }
        public Nullable<bool> IsVoucherused { get; set; }
        public string SalesAssociateEmail { get; set; }
        public string SalesAssociatePhone { get; set; }
        public string InvoiceImageName { get; set; }
        public Nullable<System.DateTime> VoucherCodeExpDate { get; set; }
        public Nullable<decimal> ExchangePrice { get; set; }
        public string ProductNumber { get; set; }
        public Nullable<int> NewProductCategoryId { get; set; }
        public Nullable<int> NewProductTypeId { get; set; }
        public Nullable<int> NewBrandId { get; set; }
        public Nullable<int> ModelNumberId { get; set; }
        public string InvoiceNumber { get; set; }
        public string QCDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public Nullable<int> StatusId { get; set; }
        public string Comment1 { get; set; }
        public string Comment2 { get; set; }
        public string Comment3 { get; set; }
        public string IsUnInstallationRequired { get; set; }
        public Nullable<decimal> UnInstallationPrice { get; set; }
        public Nullable<int> VoucherStatusId { get; set; }
        public Nullable<decimal> Sweetener { get; set; }
        public Nullable<bool> OtherCommunications { get; set; }
        public Nullable<bool> OtherCommunications1 { get; set; }
        public Nullable<bool> FollowupCommunication { get; set; }
        public Nullable<bool> FollowupCommunication1 { get; set; }
        public string SerialNumber { get; set; }
        public Nullable<decimal> FinalExchangePrice { get; set; }
        public Nullable<bool> IsDefferedSettlement { get; set; }
        public string SponsorServiceRefId { get; set; }
      
    }
}
