using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.MasterModel
{
    public class ModelNumberDataContract
    {
        public List<ModelNumberModel> modelNumberModels { get; set; }
    }
    public class ModelNumberModel
    {
        public int ModelNumberId { get; set; }
        public string ModelName { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public Nullable<int> BrandId { get; set; }
        public Nullable<int> ProductCategoryId { get; set; }
        public Nullable<int> ProductTypeId { get; set; }
        public Nullable<decimal> SweetnerForDTD { get; set; }
        public Nullable<decimal> SweetnerForDTC { get; set; }
        public Nullable<bool> IsDefaultProduct { get; set; }
        public Nullable<int> BusinessUnitId { get; set; }
    }


    #region getmodel details
    public class ModelDetailsDataContract
    {
        public string ErrorMessage { get; set; }
        public List<ModalListdataDataContract> ModelList { get; set; }
    }

    public class ModalListdataDataContract
    {
        public int ModelNumberId { get; set; }
        public string ModelName { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public int? BrandId { get; set; }
        public int? ProductCatgoryId { get; set; }
        public int? ProductTypeId { get; set; }
    }
    #endregion
}
