using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.BillCloud
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class LedgerData
    {
        public string member_ref_id { get; set; }
        public int month { get; set; }
        public int year { get; set; }
        public string dao_name { get; set; }
        public string account_type_name { get; set; }
    }

    public class LedgerRequestModel
    {
        public LedgerData data { get; set; }
    }
}
