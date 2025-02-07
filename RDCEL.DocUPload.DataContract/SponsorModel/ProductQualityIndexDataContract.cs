using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.SponsorModel
{
    public class ProductQualityIndexDataContract
    {
        public int ProductQualityIndexId { get; set; }
        public string Name { get; set; }
        public Nullable<int> ProductCategoryId { get; set; }
        public string CategoryName { get; set; }
        public string ExcellentDesc { get; set; }
        public string GoodDesc { get; set; }
        public string AverageDesc { get; set; }
        public string NonWorkingDesc { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}
