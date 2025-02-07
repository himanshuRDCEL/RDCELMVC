using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.ZohoModel
{
    public class BrandMasterListDataContract
    {
        [DataMember]
        public int code { get; set; }
        [DataMember]
        public List<BrandMaster> data { get; set; }
    }
    public class BrandMaster
    {
        public string ID { get; set; }
        public string Brand_Name { get; set; }
    }
}
