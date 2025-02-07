using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.Lodha_Group
{
    public class OrderPushback
    {
        public string pwaOrderId { get; set; }
        public string mobileNo { get; set; }
        public string address { get; set; }
        public string bookingStatus { get; set; }
        public string deviceType { get; set; }
        public int finalAmount { get; set; }
        public string orderDetailURL { get; set; }
        public PwaOrderSummary pwaOrderSummary { get; set; }
        public string remarks { get; set; }
    }
    public class PwaOrderSummary
    {
        public string serviceName { get; set; }
        public List<int> ratecards { get; set; }
        public string ProductCategory { get; set; }
        public string ProductType { get; set; } 
        public string Condition { get; set; }
        public string RegdNo { get; set; }
        public int OrderId { get; set; }
    }

    public class Data
    {
        public string bookingId { get; set; }
    }

    public class OrderPushBackResponse
    {
        public int statusCode { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
    }

}
