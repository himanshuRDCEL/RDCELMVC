using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.SponsorModel
{
   
    public class OrderStatusDetailsFromUTCDataContract
    {
        public string OrderNumber { get; set; }
        public string OrderStatus { get; set; }
         public string ExpectedDelivyDate { get; set; }
         public DateTime? OrderCreatedDate { get; set; }
        public DateTime? OrderModifiedDate { get; set; }
       

    }
}
