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
    
    public partial class sp_GetPincodeListForABBD2C_Result
    {
        public int Id { get; set; }
        public string ZohoPinCodeId { get; set; }
        public Nullable<int> ZipCode { get; set; }
        public string Location { get; set; }
        public string HubControl { get; set; }
        public string State { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<bool> IsABB { get; set; }
        public Nullable<bool> IsExchange { get; set; }
        public Nullable<int> CityId { get; set; }
        public string AreaLocality { get; set; }
    }
}
