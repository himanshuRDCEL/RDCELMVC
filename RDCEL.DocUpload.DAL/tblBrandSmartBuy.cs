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
    
    public partial class tblBrandSmartBuy
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string BrandLogoURL { get; set; }
        public Nullable<int> BusinessUnitId { get; set; }
        public Nullable<int> ProductCategoryId { get; set; }
        public Nullable<int> BrandId { get; set; }
    
        public virtual tblBrand tblBrand { get; set; }
        public virtual tblBusinessUnit tblBusinessUnit { get; set; }
        public virtual tblProductCategory tblProductCategory { get; set; }
    }
}
