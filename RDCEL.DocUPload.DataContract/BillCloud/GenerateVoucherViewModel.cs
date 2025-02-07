using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.BillCloud
{
   public class GenerateVoucherViewModel
    {
        public GenerateVoucherData data { get; set; }
    }

    public class GenerateVoucherData
    {
        public string event_id { get; set; }
        public string rrn { get; set; }
        public string dao_name { get; set; }
        public GenerateVoucherPayload payload { get; set; }
    }

    public class GenerateVoucherPayload
    {
        public string service_id { get; set; }
        public string amount { get; set; }
        public string expiry { get; set; }
        public string beneficiary_ref_id { get; set; }
        public string consumer_ref_id { get; set; }
        public string issuer_ref_id { get; set; }
        public string brand_ref_id { get; set; }
        public string merchant_ref_id { get; set; }
       
    }

    public class Decryptdatacontract
    {
        [Required(ErrorMessage ="Please provide encrypted data only from ERP portal")]
        public string Encryptstring { get; set; }
        public string EncryptionKey { get; set; }
        public string Decryptedstring { get; set; }
        public string BULogoName { get; set; }
    }
}
