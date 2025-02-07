using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.ZohoModel
{
    public class StoreCodeListDataContract
    {
        [DataMember]
        public int code { get; set; }
        [DataMember]
        public List<StoreCode> data { get; set; }
        
    }
    public class StoreCode
    {
        [DataMember]
        public string Store_Name1 { get; set; }
        [DataMember]
        public string Store_Code { get; set; }
        [DataMember]
        public string Store_Manager_Email { get; set; }
        [DataMember]
        public SponsorName Sponsor_Name { get; set; }
        [DataMember]
        public string location { get; set; }
        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public string Pin_Code { get; set; }
        [DataMember]
        public string City { get; set; }
    }
    //public class SponsorName
    //{
    //    public string display_value { get; set; }
    //    public string ID { get; set; }
    //}
}
