using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.ZohoModel
{
    public class AllCallAllocationListDataContract
    {
        [DataMember]
        public int code { get; set; }
        [DataMember]
        public List<AllCallAllocationData> data { get; set; }
    }

    public class AllCallAllocationData
    {
        [DataMember]
        public string Product_Group { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string Total_Exchange_Price { get; set; }
        [DataMember]
        public RegdNo Regd_No { get; set; }
        [DataMember]
        public string Cust_Pin_Code { get; set; }
        [DataMember]
        public EVCName EVC_Name { get; set; }
        [DataMember]
        public string Prexo_Technology { get; set; }
        [DataMember]
        public string EVC_PIN_Code { get; set; }
        [DataMember]
        public string Bonus { get; set; }
        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string Exchange_Price_Displayed_Max { get; set; }
    }

    public class RegdNo
    {
        [DataMember]
        public string display_value { get; set; }
        [DataMember]
        public string ID { get; set; }
    }

    public class EVCName
    {
        [DataMember]
        public string display_value { get; set; }
        [DataMember]
        public string ID { get; set; }
    }

}
