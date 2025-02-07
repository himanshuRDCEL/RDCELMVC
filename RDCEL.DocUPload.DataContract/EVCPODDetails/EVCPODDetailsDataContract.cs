using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.EVCPODDetails
{
    public class EVCPODDetailsDataContract
    {
        public int Id { get; set; }
        public Nullable<int> RegdNo { get; set; }
        public Nullable<int> ExchangeId { get; set; }
        public Nullable<int> EVCId { get; set; }
        public string PODURL { get; set; }
        public Nullable<int> ABBRedemptionId { get; set; }
     
    }
}
