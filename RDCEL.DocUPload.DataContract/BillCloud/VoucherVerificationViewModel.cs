using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.BillCloud
{
    public class VoucherVerificationViewModel
    {
        public VoucherVerificationData data { get; set; }
    }

    public class VoucherVerificationData
    {
        public string event_id { get; set; }
        public string rrn { get; set; }
        public string dao_name { get; set; }
        public VoucherVerificationPayload payload { get; set; }
    }

    public class VoucherVerificationPayload
    {
        public string service_id { get; set; }
        public string amount { get; set; }
        public string sweetener { get; set; }
        public string expiry { get; set; }
        public string voucher_id { get; set; }
        public string dealer_ref_id { get; set; }
        public string acquirer_ref_id { get; set; }
        public string beneficiary_ref_id { get; set; }
        public string consumer_ref_id { get; set; }
        public string issuer_ref_id { get; set; }
        public string abrand_ref_id { get; set; }
        public string merchant_ref_id { get; set; }
    }
}
