using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.SponsorModel
{
   
    public class OrderStatusDetailsDataContract
    {
        public int OrderId { get; set; }
        public string Mode_of_Payment { get; set; }
         public string Pickup { get; set; }
         public string Reason_for_Rejection { get; set; }
        public string Payment_Received { get; set; }
        public string Date_of_Pickup { get; set; }
        public string Expected_Pickup_Date { get; set; }

    }

    public class OrderDetailsViewModel
    {
        public int OrderId { get; set; }
        public string Quality_declared_by_customer { get; set; }
        
        public string Quality_as_per_qc { get; set; }
        public string Status_Code { get; set; }
        public string Status_description { get; set; }
        public string Price_after_QC { get; set; }
        public string DateOfPayment { get; set; }
        public string Mode_of_Payment { get; set; } 

    }
}
