using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract._247aroundService
{
    public class DeliveryDate
    {
        public string minute { get; set; }
        public string month { get; set; }
        public string hour { get; set; }
        public string day { get; set; }
        public string year { get; set; }
    }

    public class _247AroundDataContract
    {
        public string address { get; set; }
        public string partnerName { get; set; }
        public DeliveryDate deliveryDate { get; set; }
        public string productType { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public string product { get; set; }
        public string category { get; set; }
        public string subCategory { get; set; }
        public string orderID { get; set; }
        public string partnerSource { get; set; }
        public string requestType { get; set; }
        public string name { get; set; }
        public string city { get; set; }
        public string brand { get; set; }
        public int pincode { get; set; }
        public string itemID { get; set; }
    }




}
