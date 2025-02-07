using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCEL.DocUpload.DataContract.Base;

namespace RDCEL.DocUpload.DataContract.ExchangeOrderDetails
{
    public class OrderTransactionDataContract : BaseDataContract
    {

        public int OrderTransId { get; set; }
        public int OrderType { get; set; }
        public Nullable<int> ExchangeId { get; set; }
        public Nullable<int> ABBRedemptionId { get; set; }
        public string SponsorOrderNumber { get; set; }
        public string RegdNo { get; set; }
        public Nullable<decimal> ExchangePrice { get; set; }
        public Nullable<decimal> QuotedPrice { get; set; }
        public Nullable<decimal> Sweetner { get; set; }
        public Nullable<decimal> FinalPriceAfterQC { get; set; }
        public Nullable<int> EVCPriceMasterId { get; set; }
        public Nullable<decimal> EVCPrice { get; set; }
    }
}
