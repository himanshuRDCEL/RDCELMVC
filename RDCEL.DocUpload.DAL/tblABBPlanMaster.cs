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
    
    public partial class tblABBPlanMaster
    {
        public int PlanMasterId { get; set; }
        public Nullable<int> BusinessUnitId { get; set; }
        public string Sponsor { get; set; }
        public Nullable<int> From_Month { get; set; }
        public Nullable<int> To_Month { get; set; }
        public Nullable<int> Assured_BuyBack_Percentage { get; set; }
        public Nullable<int> PlanPeriodInMonth { get; set; }
        public Nullable<int> ProductCatId { get; set; }
        public Nullable<int> ProductTypeId { get; set; }
        public string ABBPlanName { get; set; }
        public string NoClaimPeriod { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    
        public virtual tblProductCategory tblProductCategory { get; set; }
        public virtual tblProductType tblProductType { get; set; }
        public virtual tblBusinessUnit tblBusinessUnit { get; set; }
    }
}
