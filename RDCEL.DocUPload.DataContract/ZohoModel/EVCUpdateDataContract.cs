using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.ZohoModel
{
    public class EVCUpdateDataContract
    {
        [DataMember]
        public string EVC_Wallet_Amount { get; set; }
        [DataMember]
        public string ID { get; set; }
        
    }

    public class EVCUpdateFormResponseDataContract
    {
        [DataMember]
        public int code { get; set; }
        [DataMember]
        public Data data { get; set; }
        [DataMember]
        public string message { get; set; }

    }

    public class EVCUpdateFormRequestDataContract
    {
        public EVCUpdateFormRequestDataContract()
        {
            result = new ResultInRequestDataContract();
        }
        [DataMember]
        public EVCUpdateDataContract data { get; set; }
        [DataMember]
        public ResultInRequestDataContract result { get; set; }

    }
}
