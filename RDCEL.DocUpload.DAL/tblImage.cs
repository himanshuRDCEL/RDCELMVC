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
    
    public partial class tblImage
    {
        public int Id { get; set; }
        public Nullable<int> BizlogTicketId { get; set; }
        public Nullable<int> SponsorId { get; set; }
        public string ImageURL { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    
        public virtual tblBizlogTicket tblBizlogTicket { get; set; }
        public virtual tblExchangeOrder tblExchangeOrder { get; set; }
    }
}
