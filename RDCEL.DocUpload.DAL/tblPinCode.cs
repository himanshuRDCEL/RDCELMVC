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
    
    public partial class tblPinCode
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblPinCode()
        {
            this.TblBPPincodeMappings = new HashSet<TblBPPincodeMapping>();
        }
    
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
        public Nullable<bool> isExchange { get; set; }
        public Nullable<int> CityId { get; set; }
        public string AreaLocality { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblBPPincodeMapping> TblBPPincodeMappings { get; set; }
        public virtual tblCity tblCity { get; set; }
    }
}
