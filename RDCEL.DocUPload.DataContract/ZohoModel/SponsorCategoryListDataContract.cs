using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.ZohoModel
{
    public class SponsorCategoryListDataContract
    {
        [DataMember]
        public int code { get; set; }
        [DataMember]
        public List<CategoryData> data { get; set; }
    }

    public class CategoryData
    {
        [DataMember]
        public string Long_Name { get; set; }
        
        [DataMember]
        public string Product_Technology { get; set; }
        [DataMember]
        public string ID { get; set; }      
        
    }

    
}


