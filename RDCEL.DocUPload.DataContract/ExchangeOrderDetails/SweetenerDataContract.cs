using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.ExchangeOrderDetails
{
   public class SweetenerDataContract
   {
        public decimal? SweetenerBu { get; set; }
        public decimal? SweetenerBP { get; set; }
        public decimal? SweetenerDigi2L { get; set; }
        public decimal? SweetenerTotal { get; set; }
        public OldSweetenerDataContract OldSweetenerDC { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorMessageForProductPrice { get; set; }
        public decimal? BaseValue { get; set; }
        public decimal? ExchangePrice { get; set; }
      
   }

    public class OldSweetenerDataContract
    {
      
        public decimal? OldSweetenerBu { get; set; }
        public decimal? OldSweetenerBP { get; set; }
        public decimal? OldSweetenerOwn { get; set; }
        public decimal? OldSweetenerTotal { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorMessageForProductPrice { get; set; }
        public decimal? BaseValue { get; set; }
        public decimal? ExchangePrice { get; set; }

    }
    public class GetSweetenerDetailsDataContract
    {
        public int? BusinessUnitId { get; set; }
        public int? BusinessPartnerId { get; set; }
        public int? BrandId { get; set; }
        public bool? IsSweetenerModalBased { get; set; }

        public bool ? IsOldProductBaseSweetener { get; set; }
        public int? ModalId { get; set; }
        public int? OldBrandId { get; set; }

        public int? NewProdCatId { get; set; }
        public int? OlProdCatId { get; set; }
        public  int? NewProdTypeId { get; set; }
        public  int? OldProdTypeId { get; set; }
    }
}
