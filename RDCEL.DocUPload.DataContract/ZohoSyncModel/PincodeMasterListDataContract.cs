using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.ZohoSyncModel
{
    public class PincodeMasterListDataContract
    {
        [DataMember]
        public int code { get; set; }
        [DataMember]
        public List<PinCodeMaster> data { get; set; }
    }
    public class PinCodeMaster
    {
        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public string Pin_Code { get; set; }
        [DataMember]
        public string Exch_Active { get; set; }
        [DataMember]
        public string City_Code { get; set; }
        [DataMember]
        public string ABB_Active { get; set; }
        //public string Temp_Hold_On_Exg { get; set; }
      
    }
}
