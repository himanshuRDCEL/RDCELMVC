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
    
    public partial class tblDriverDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblDriverDetail()
        {
            this.tblExchangeABBStatusHistories = new HashSet<tblExchangeABBStatusHistory>();
            this.tblLogistics = new HashSet<tblLogistic>();
            this.tblOrderLGCs = new HashSet<tblOrderLGC>();
            this.TblVehicleJourneyTrackings = new HashSet<TblVehicleJourneyTracking>();
            this.TblVehicleJourneyTrackingDetails = new HashSet<TblVehicleJourneyTrackingDetail>();
        }
    
        public int DriverDetailsId { get; set; }
        public string DriverName { get; set; }
        public string DriverPhoneNumber { get; set; }
        public string VehicleNumber { get; set; }
        public string city { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> Modifiedby { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string Vehicle_RC_Number { get; set; }
        public string Vehicle_RC_Certificate { get; set; }
        public string VehiclefitnessCertificate { get; set; }
        public string DriverlicenseNumber { get; set; }
        public string DriverlicenseImage { get; set; }
        public Nullable<bool> is_approved { get; set; }
        public Nullable<int> approvedBy { get; set; }
        public string ProfilePicture { get; set; }
        public string VehicleInsuranceCertificate { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> CityId { get; set; }
        public Nullable<int> ServicePartnerId { get; set; }
        public Nullable<int> DriverId { get; set; }
        public Nullable<System.DateTime> JourneyPlanDate { get; set; }
        public Nullable<int> VehicleId { get; set; }
    
        public virtual tblCity tblCity { get; set; }
        public virtual tblUser tblUser { get; set; }
        public virtual tblUser tblUser1 { get; set; }
        public virtual tblDriverList tblDriverList { get; set; }
        public virtual tblDriverList tblDriverList1 { get; set; }
        public virtual tblUser tblUser2 { get; set; }
        public virtual tblUser tblUser3 { get; set; }
        public virtual tblVehicleList tblVehicleList { get; set; }
        public virtual tblVehicleList tblVehicleList1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblExchangeABBStatusHistory> tblExchangeABBStatusHistories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblLogistic> tblLogistics { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblOrderLGC> tblOrderLGCs { get; set; }
        public virtual tblServicePartner tblServicePartner { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblVehicleJourneyTracking> TblVehicleJourneyTrackings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TblVehicleJourneyTrackingDetail> TblVehicleJourneyTrackingDetails { get; set; }
    }
}
