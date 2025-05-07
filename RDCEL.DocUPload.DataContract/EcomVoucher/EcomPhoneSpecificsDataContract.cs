using Newtonsoft.Json;
using RDCEL.DocUpload.DataContract.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUPload.DataContract.EcomVoucher
{
    public class EcomPhoneSpecificsDataContract :BaseDataContract
    {

        public int EcomPhoneSpecificId { get; set; }
        [JsonProperty("VoucherCode")]
        public string VoucherCode { get; set; }
        [JsonProperty("Phoneno")]
        public string Phoneno { get; set; }
        public int EcomVoucherId { get; set; }
        public string Voucherstatus { get; set; }
        public bool IsUsed { get; set; }
    }
}
