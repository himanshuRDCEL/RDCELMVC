using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UTC_Bridge.DocUPload.DataContract.ZohoModel
{
    public class SponserUpdateDataContract
    {
        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public string LGC_Tkt_No { get; set; }      
        [DataMember]
        public string EVC_Drop { get; set; }  
        [DataMember]
        public string Pickup { get; set; }
    }

    public class SponserUpdateFormRequestDataContract
    {
        public SponserUpdateFormRequestDataContract()
        {
            result = new ResultInRequestDataContract();
        }
        [DataMember]
        public SponserUpdateDataContract data { get; set; }
        [DataMember]
        public ResultInRequestDataContract result { get; set; }

    }
}
