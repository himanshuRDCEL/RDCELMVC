//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RDCEL.DocUpload.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblExchangeOrder
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblExchangeOrder()
        {
            this.tblEVCDisputes = new HashSet<tblEVCDispute>();
            this.tblFeedBacks = new HashSet<tblFeedBack>();
            this.tblHistories = new HashSet<tblHistory>();
            this.tblImages = new HashSet<tblImage>();
            this.tblSelfQCs = new HashSet<tblSelfQC>();
            this.tblOrderTrans = new HashSet<tblOrderTran>();
            this.tblVoucherVerfications = new HashSet<tblVoucherVerfication>();
        }
    
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
        public Nullable<int> ProductTechnologyId { get; set; }
        public Nullable<bool> IsExchangePriceWithoutSweetner { get; set; }
        public Nullable<bool> IsFinalExchangePriceWithoutSweetner { get; set; }
        public Nullable<decimal> BaseExchangePrice { get; set; }
        public string EmployeeId { get; set; }
        public Nullable<decimal> SweetenerBU { get; set; }
        public Nullable<decimal> SweetenerBP { get; set; }
        public Nullable<decimal> SweetenerDigi2l { get; set; }
        public Nullable<int> BusinessUnitId { get; set; }
        public Nullable<int> PriceMasterNameId { get; set; }
        public Nullable<bool> IsDiagnoseV2 { get; set; }
        public Nullable<bool> IsSrNumValid { get; set; }
        public string NewSerialNumber { get; set; }
        public Nullable<int> CouponId { get; set; }
        public Nullable<bool> IsCouponAplied { get; set; }
        public Nullable<decimal> CouponValue { get; set; }
    
        public virtual tblBrand tblBrand { get; set; }
        public virtual tblBusinessPartner tblBusinessPartner { get; set; }
        public virtual tblBusinessUnit tblBusinessUnit { get; set; }
        public virtual tblCustomerDetail tblCustomerDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblEVCDispute> tblEVCDisputes { get; set; }
        public virtual TblProductTechnology TblProductTechnology { get; set; }
        public virtual tblExchangeOrderStatu tblExchangeOrderStatu { get; set; }
        public virtual tblVoucherStatu tblVoucherStatu { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblFeedBack> tblFeedBacks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblHistory> tblHistories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblImage> tblImages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblSelfQC> tblSelfQCs { get; set; }
        public virtual tblProductType tblProductType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblOrderTran> tblOrderTrans { get; set; }
        public virtual tblPriceMasterName tblPriceMasterName { get; set; }
        public virtual tblUser tblUser { get; set; }
        public virtual tblUser tblUser1 { get; set; }
        public virtual tblSociety tblSociety { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblVoucherVerfication> tblVoucherVerfications { get; set; }
    }
}
