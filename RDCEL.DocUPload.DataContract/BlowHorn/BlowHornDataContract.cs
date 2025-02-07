using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.BlowHorn
{
    public class ItemDetail
    {
        public string item_name { get; set; }
        public int item_quantity { get; set; }
        public int item_price_per_each { get; set; }
        public int total_item_price { get; set; }
        public string item_category { get; set; }
        public string brand { get; set; }
        public string weight { get; set; }
        public string volume { get; set; }
        public string cgst { get; set; }
        public string sgst { get; set; }
        public string igst { get; set; }
    }

    public class BlowHornDataContract
    {
        public string awb_number { get; set; }
        public string delivery_hub { get; set; }
        public string pickup_hub { get; set; }
        public string is_hyperlocal { get; set; }
        public string customer_name { get; set; }
        public string customer_mobile { get; set; }
        public string pin_number { get; set; }
        public string alternate_customer_mobile { get; set; }
        public string customer_email { get; set; }
        public string delivery_address { get; set; }
        public string delivery_postal_code { get; set; }
        public string what3words { get; set; }
        public string reference_number { get; set; }
        public string customer_reference_number { get; set; }
        public string delivery_lat { get; set; }
        public string delivery_lon { get; set; }
        public string pickup_address { get; set; }
        public string pickup_postal_code { get; set; }
        public string pickup_lat { get; set; }
        public string pickup_lon { get; set; }
        public string pickup_customer_name { get; set; }
        public string pickup_customer_mobile { get; set; }
        public bool is_return_order { get; set; }
        public string commercial_class { get; set; }
        public string priority { get; set; }
        public string weight { get; set; }
        public string volume { get; set; }
        public string length { get; set; }
        public string breadth { get; set; }
        public string height { get; set; }
        public bool is_commercial_address { get; set; }
        public DateTime pickup_datetime { get; set; }
        public string division { get; set; }
        public DateTime expected_delivery_time { get; set; }
        public bool is_cod { get; set; }
        public string cash_on_delivery { get; set; }
        //public string SponsrorOrderNo { get; set; }
        public List<ItemDetail> item_details { get; set; }
    }
    public class Item
    {
        public string item_name { get; set; }
        public int item_quantity { get; set; }
        public int item_price_per_each { get; set; }
        public int total_item_price { get; set; }
    }

    public class ItemDetails
    {
        public Item _item { get; set; }
    }

    public class Message
    {
        public string awb_number { get; set; }
        public ItemDetails item_details { get; set; }
    }

    public class BlowHornRreponseDataContract
    {
        public string status { get; set; }
        public Message message { get; set; }
    }

}
