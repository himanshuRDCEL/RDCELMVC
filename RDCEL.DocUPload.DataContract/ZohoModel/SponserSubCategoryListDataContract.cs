using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.ZohoModel
{
    public class SponserSubCategoryListDataContract
    {
        [DataMember]
        public int code { get; set; }
        [DataMember]
        public List<SubCategoryData> data { get; set; }
    }

    public class SubCategoryData
    {
        [DataMember]
        public string Long_Name { get; set; }
        //public ProductTechnology Product_Technology { get; set; }
        [DataMember]
        public string Sub_Product_Technology { get; set; }
        [DataMember]
        public string ID { get; set; }      
        
    }

    public class ProductTechnology
    {
        [DataMember]
        public string display_value { get; set; }
        [DataMember]
        public string ID { get; set; }
    }
}


