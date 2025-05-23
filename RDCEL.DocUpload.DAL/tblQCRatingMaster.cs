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
    
    public partial class tblQCRatingMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblQCRatingMaster()
        {
            this.tblOrderQCRatings = new HashSet<tblOrderQCRating>();
            this.tblQCRatingMasterMappings = new HashSet<tblQCRatingMasterMapping>();
        }
    
        public int QCRatingId { get; set; }
        public Nullable<int> ProductCatId { get; set; }
        public string QCQuestion { get; set; }
        public Nullable<int> RatingWeightage { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> QuestionerLOVId { get; set; }
        public string QuestionsImage { get; set; }
        public Nullable<bool> IsAgeingQues { get; set; }
        public Nullable<bool> IsDecidingQues { get; set; }
        public Nullable<bool> IsDiagnoseV2 { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblOrderQCRating> tblOrderQCRatings { get; set; }
        public virtual tblUser tblUser { get; set; }
        public virtual tblUser tblUser1 { get; set; }
        public virtual tblUser tblUser2 { get; set; }
        public virtual tblUser tblUser3 { get; set; }
        public virtual TblQuestionerLOV TblQuestionerLOV { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblQCRatingMasterMapping> tblQCRatingMasterMappings { get; set; }
    }
}
