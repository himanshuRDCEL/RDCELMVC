using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCEL.DocUpload.DataContract.Base;

namespace RDCEL.DocUpload.DataContract.ExchangeOrderDetails
{
    public class ExchangeABBStatusHistoryDataContract : BaseDataContract
    {
        public int StatusHistoryId { get; set; }
        public int OrderType { get; set; }
        public Nullable<int> ExchangeId { get; set; }
        public Nullable<int> ABBRedemptionId { get; set; }
        public string SponsorOrderNumber { get; set; }
        public string RegdNo { get; set; }
        public string ZohoSponsorId { get; set; }
        public int CustId { get; set; }
        public Nullable<int> StatusId { get; set; }
        public Nullable<int> OrderTransId { get; set; }
    }
}
