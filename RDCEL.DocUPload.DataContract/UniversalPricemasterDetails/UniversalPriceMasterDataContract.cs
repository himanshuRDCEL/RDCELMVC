using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.UniversalPricemasterDetails
{
   public class UniversalPriceMasterDataContract
    {
        public decimal? BaseValue { get; set; }
        public string ErrorMessage { get; set; }
        public int? PricemasternameId { get; set; }
    }

    public  class ProductPriceDetailsDataContract
    {
        public int? BrandId { get; set; }
        public int? NewBrandId { get; set; }
        public int? ProductCatId { get; set; }
        public int? ProductTypeId { get; set; }
        public int? BusinessUnitId { get; set; }
        public int? BusinessPartnerId { get; set; }
        public int? conditionId { get; set; }


        public int? PriceNameId { get; set; }

    }

    public class ProductConditionList
    {
       
        public string Price { get; set; }
        public string Mid_Price { get; set; }
        public string Min_Price { get; set; }
        public string Scrap_Price { get; set; }
    }

}
