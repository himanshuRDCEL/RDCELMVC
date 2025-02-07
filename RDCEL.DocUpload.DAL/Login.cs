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
    
    public partial class Login
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Login()
        {
            this.tblSocieties = new HashSet<tblSociety>();
            this.tblBusinessUnits = new HashSet<tblBusinessUnit>();
        }
    
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string ZohoSponsorId { get; set; }
        public Nullable<int> SponsorId { get; set; }
        public string PriceCode { get; set; }
        public Nullable<int> BusinessPartnerId { get; set; }
        public Nullable<int> PriceMasterNameId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblSociety> tblSocieties { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblBusinessUnit> tblBusinessUnits { get; set; }
        public virtual tblPriceMasterName tblPriceMasterName { get; set; }
    }
}
