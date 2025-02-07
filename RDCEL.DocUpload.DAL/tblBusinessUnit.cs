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
    
    public partial class tblBusinessUnit
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblBusinessUnit()
        {
            this.tblABBPlanMasters = new HashSet<tblABBPlanMaster>();
            this.tblABBPriceMasters = new HashSet<tblABBPriceMaster>();
            this.tblABBRegistrations = new HashSet<tblABBRegistration>();
            this.tblBrands = new HashSet<tblBrand>();
            this.tblBrandSmartBuys = new HashSet<tblBrandSmartBuy>();
            this.tblBUBasedSweetnerValidations = new HashSet<tblBUBasedSweetnerValidation>();
            this.tblBUProductCategoryMappings = new HashSet<tblBUProductCategoryMapping>();
            this.tblBusinessPartners = new HashSet<tblBusinessPartner>();
            this.tblCompanies = new HashSet<tblCompany>();
            this.tblExchangeOrders = new HashSet<tblExchangeOrder>();
            this.tblModelMappings = new HashSet<tblModelMapping>();
            this.tblModelNumbers = new HashSet<tblModelNumber>();
            this.tblOrderBasedConfigs = new HashSet<tblOrderBasedConfig>();
            this.tblPriceMasterMappings = new HashSet<tblPriceMasterMapping>();
            this.tblProductConditionLabels = new HashSet<tblProductConditionLabel>();
            this.tblTransMasterABBPlanMasters = new HashSet<tblTransMasterABBPlanMaster>();
            this.tblVoucherTermsAndConditions = new HashSet<tblVoucherTermsAndCondition>();
            this.tblSocieties = new HashSet<tblSociety>();
            this.tblBPBURedemptionMappings = new HashSet<tblBPBURedemptionMapping>();
            this.tblSponsorCategoryMappings = new HashSet<tblSponsorCategoryMapping>();
            this.tblBPBUAssociations = new HashSet<tblBPBUAssociation>();
            this.TblBPPincodeMappings = new HashSet<TblBPPincodeMapping>();
            this.tblUninstallationPrices = new HashSet<tblUninstallationPrice>();
            this.tblCouponMasters = new HashSet<tblCouponMaster>();
        }
    
        public int BusinessUnitId { get; set; }
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
        public Nullable<int> LoginId { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ZohoSponsorId { get; set; }
        public Nullable<int> ExpectedDeliveryHours { get; set; }
        public Nullable<int> VoucherExpiryTime { get; set; }
        public Nullable<decimal> SweetnerForDTD { get; set; }
        public Nullable<decimal> SweetnerForDTC { get; set; }
        public Nullable<bool> IsSweetnerModelBased { get; set; }
        public Nullable<bool> IsABB { get; set; }
        public Nullable<bool> IsExchange { get; set; }
        public Nullable<bool> IsBUMultiBrand { get; set; }
        public Nullable<bool> IsBUD2C { get; set; }
        public Nullable<bool> ShowAbbPlan { get; set; }
        public Nullable<bool> IsInvoiceDetailsRequired { get; set; }
        public Nullable<bool> IsNewProductDetailsRequired { get; set; }
        public Nullable<int> GSTType { get; set; }
        public Nullable<int> MarginType { get; set; }
        public Nullable<bool> IsSponsorNumberRequiredOnUI { get; set; }
        public Nullable<bool> IsUpiIdRequired { get; set; }
        public Nullable<bool> IsPaymentThirdParty { get; set; }
        public Nullable<bool> IsModelDetailRequired { get; set; }
        public Nullable<bool> IsNewBrandRequired { get; set; }
        public Nullable<bool> IsAreaLocality { get; set; }
        public Nullable<bool> IsValidationBasedSweetner { get; set; }
        public Nullable<bool> IsQualityRequiredOnUI { get; set; }
        public Nullable<bool> IsQualityWorkingNonWorking { get; set; }
        public Nullable<bool> IsStandardPriceMaster { get; set; }
        public Nullable<bool> ShowEmplyeeCode { get; set; }
        public Nullable<bool> IsQCDateTimeRequiredOnD2C { get; set; }
        public Nullable<bool> IsBulkOrder { get; set; }
        public Nullable<bool> IsCertificateAvailable { get; set; }
        public Nullable<bool> IsSweetenerIndependent { get; set; }
        public Nullable<bool> IsInvoiceAvailable { get; set; }
        public Nullable<bool> IsReportingOn { get; set; }
        public Nullable<int> OrderPendingTimeH { get; set; }
        public string ReportEmails { get; set; }
        public Nullable<bool> IsAbbDayConfig { get; set; }
        public Nullable<int> AbbDayDiff { get; set; }
        public Nullable<bool> IsBUCatIdOn { get; set; }
        public Nullable<bool> IsBPAssociated { get; set; }
        public Nullable<bool> IsProductSerialNumberRequired { get; set; }
        public Nullable<bool> IsSFIDRequired { get; set; }
    
        public virtual Login Login { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblABBPlanMaster> tblABBPlanMasters { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblABBPriceMaster> tblABBPriceMasters { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblABBRegistration> tblABBRegistrations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBrand> tblBrands { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBrandSmartBuy> tblBrandSmartBuys { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBUBasedSweetnerValidation> tblBUBasedSweetnerValidations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBUProductCategoryMapping> tblBUProductCategoryMappings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBusinessPartner> tblBusinessPartners { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblCompany> tblCompanies { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblExchangeOrder> tblExchangeOrders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblModelMapping> tblModelMappings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblModelNumber> tblModelNumbers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblOrderBasedConfig> tblOrderBasedConfigs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblPriceMasterMapping> tblPriceMasterMappings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblProductConditionLabel> tblProductConditionLabels { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblTransMasterABBPlanMaster> tblTransMasterABBPlanMasters { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblVoucherTermsAndCondition> tblVoucherTermsAndConditions { get; set; }
        public virtual tblLoV tblLoV { get; set; }
        public virtual tblLoV tblLoV1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblSociety> tblSocieties { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBPBURedemptionMapping> tblBPBURedemptionMappings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblSponsorCategoryMapping> tblSponsorCategoryMappings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBPBUAssociation> tblBPBUAssociations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblBPPincodeMapping> TblBPPincodeMappings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblUninstallationPrice> tblUninstallationPrices { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblCouponMaster> tblCouponMasters { get; set; }
    }
}
