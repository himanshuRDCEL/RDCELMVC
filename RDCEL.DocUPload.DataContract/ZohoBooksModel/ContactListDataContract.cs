using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.ZohoBooksModel
{
    public class ContactListDataContract
    {
        public int code { get; set; }
        public string message { get; set; }
        public List<ContactData> contacts { get; set; }
        public PageContext page_context { get; set; }
    }

    public class ContactData
    {
        public string contact_id { get; set; }
        public string contact_name { get; set; }
        public string customer_name { get; set; }
        public string vendor_name { get; set; }
        public string company_name { get; set; }
        public string website { get; set; }
        public string language_code { get; set; }
        public string language_code_formatted { get; set; }
        public string contact_type { get; set; }
        public string contact_type_formatted { get; set; }
        public string status { get; set; }
        public string customer_sub_type { get; set; }
        public string source { get; set; }
        public bool is_linked_with_zohocrm { get; set; }
        public int payment_terms { get; set; }
        public string payment_terms_label { get; set; }
        public string currency_id { get; set; }
        public string twitter { get; set; }
        public string facebook { get; set; }
        public string currency_code { get; set; }
        public double outstanding_receivable_amount { get; set; }
        public double outstanding_receivable_amount_bcy { get; set; }
        public double outstanding_payable_amount { get; set; }
        public double outstanding_payable_amount_bcy { get; set; }
        public double unused_credits_receivable_amount { get; set; }
        public double unused_credits_payable_amount { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string mobile { get; set; }
        public string portal_status { get; set; }
        public DateTime created_time { get; set; }
        public string created_time_formatted { get; set; }
        public DateTime last_modified_time { get; set; }
        public string last_modified_time_formatted { get; set; }
       // public List<object> custom_fields { get; set; }
        
       
               
       
    }

    public class PageContext
    {
        public int page { get; set; }
        public int per_page { get; set; }
        public bool has_more_page { get; set; }
        public string report_name { get; set; }
        public string applied_filter { get; set; }
        public List<object> custom_fields { get; set; }
        public string sort_column { get; set; }
        public string sort_order { get; set; }
    }
   
}
