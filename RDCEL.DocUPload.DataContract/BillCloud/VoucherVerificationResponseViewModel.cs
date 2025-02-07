using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.BillCloud
{

    public class VoucherVerificationResponseViewModel
    {
        public VoucherVerificationResponseData data { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class VoucherVerificationContext
    {
        public VoucherVerificationEVENT EVENT { get; set; }
        public VoucherVerificationMERCHANT MERCHANT { get; set; }
        public VoucherVerificationDEALER DEALER { get; set; }
        public int D1 { get; set; }
        public int C1 { get; set; }
        public int D2 { get; set; }
        public int C2 { get; set; }
    }

    public class VoucherVerificationResponseData
    {
        public string status { get; set; }
        public string reason { get; set; }
        public string event_qrn { get; set; }
        public string event_rrn { get; set; }
        public VoucherVerificationContext context { get; set; }
        public string voucher_id { get; set; }
    }

    public class VoucherVerificationDEALER
    {
        public int DEALER_WALLET { get; set; }
    }

    public class VoucherVerificationEVENT
    {
        public string service_id { get; set; }
        public int amount { get; set; }
        public string expiry { get; set; }
        public string voucher_id { get; set; }
        public string dealer_ref_id { get; set; }
        public string acquirer_ref_id { get; set; }
        public int beneficiary_ref_id { get; set; }
        public int consumer_ref_id { get; set; }
        public int issuer_ref_id { get; set; }
        public int abrand_ref_id { get; set; }
        public string merchant_ref_id { get; set; }
    }

    public class VoucherVerificationMERCHANT
    {
        public int CONSUMER_WALLET { get; set; }
    }

  


}
