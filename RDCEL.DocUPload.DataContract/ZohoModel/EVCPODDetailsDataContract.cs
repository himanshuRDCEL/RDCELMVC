using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.ZohoModel
{
    public class EVCPODDetailsDataContract
    {
        public string ID { get; set; }
        [DataMember]
        public ProofOfDelivery Proof_Of_Delivery { get; set; }
    }

    public class ProofOfDelivery
    {
        public string value { get; set; }
        public string url { get; set; }
    }

    public class ExchageOrderPODDataContract
    {
        public ExchageOrderPODDataContract()
        {
            result = new ResultInRequestDataContract();
        }
        [DataMember]
        public EVCPODDetailsDataContract data { get; set; }
        [DataMember]
        public ResultInRequestDataContract result { get; set; }

    }
}
