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
    
    public partial class PineLabsPriceMaster
    {
        public short PriceMasterUniversalId { get; set; }
        public byte PriceMasterNameId { get; set; }
        public string PriceMasterName { get; set; }
        public byte ProductCategoryId { get; set; }
        public string ProductCategoryName { get; set; }
        public byte ProductTypeId { get; set; }
        public string ProductTypeName { get; set; }
        public string ProductTypeCode { get; set; }
        public string BrandName_1 { get; set; }
        public string BrandName_2 { get; set; }
        public string BrandName_3 { get; set; }
        public string BrandName_4 { get; set; }
        public short Quote_P_High { get; set; }
        public short Quote_Q_High { get; set; }
        public short Quote_R_High { get; set; }
        public short Quote_S_High { get; set; }
        public short Quote_P { get; set; }
        public short Quote_Q { get; set; }
        public short Quote_R { get; set; }
        public short Quote_S { get; set; }
        public string OtherBrand { get; set; }
        public System.DateTime PriceStartDate { get; set; }
        public System.DateTime PriceEndDate { get; set; }
        public byte IsActive { get; set; }
        public byte CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
    }
}
