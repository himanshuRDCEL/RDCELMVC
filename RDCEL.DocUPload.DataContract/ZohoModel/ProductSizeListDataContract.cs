using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.ZohoModel
{
    public class ProductSizeListDataContract
    {
        [DataMember]
        public int code { get; set; }
        [DataMember]
        public List<ProductSize> data { get; set; }
    }
    public class ProductSize
    {
        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public string Size { get; set; }
        [DataMember]
        public string Size_Description { get; set; }
    }
}
