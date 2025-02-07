using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.UniversalPricemasterDetails
{
    public class ProductDetailsForOldDataContract
    {
        public int? OldProductcategoryId { get; set; }
        public int? OldProductTypeId { get; set; }
        public int? PriceMasterNameId { get; set; }
        
    }

    public class PriceMasterMappingDataContract
    {
        public int? BusinessunitId { get; set; }
        public int? BusinessPartnerId { get; set; }
        public int? NewBrandId { get; set; }
    }

    public class PriceMasterNameDataContract
    {
        public int? PriceNameId { get; set; }
        public string ErrorMessage { get; set; }
    }
}
