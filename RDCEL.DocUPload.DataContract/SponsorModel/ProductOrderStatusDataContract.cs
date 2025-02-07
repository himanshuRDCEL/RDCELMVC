using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.SponsorModel
{
   
    public class ProductOrderStatusDataContract
    {
        public int OrderId { get; set; }
        public string Status { get; set; }
        
    }
    public class ProductOrderStatusResponseDataContract
    {
        public int OrderId { get; set; }
        public string Expected_Pickup_Date { get; set; }
    }
}
