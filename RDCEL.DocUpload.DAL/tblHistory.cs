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
    
    public partial class tblHistory
    {
        public int Id { get; set; }
        public string RegdNo { get; set; }
        public string VoucherCode { get; set; }
        public Nullable<int> ProductTypeId { get; set; }
        public Nullable<decimal> Sweetner { get; set; }
        public Nullable<decimal> ExchangeAmount { get; set; }
        public Nullable<int> ExchangeOrderId { get; set; }
        public Nullable<int> CustId { get; set; }
        public Nullable<System.DateTime> createdate { get; set; }
        public Nullable<System.DateTime> modifieddate { get; set; }
        public Nullable<bool> IsActive { get; set; }
    
        public virtual tblCustomerDetail tblCustomerDetail { get; set; }
        public virtual tblExchangeOrder tblExchangeOrder { get; set; }
    }
}
