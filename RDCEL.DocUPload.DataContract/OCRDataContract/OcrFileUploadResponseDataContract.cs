using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DataContract.OCRDataContract
{

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class BuyerAddress
    {
        public string value { get; set; }
        public double accuracy { get; set; }
    }

    public class BuyerName
    {
        public string value { get; set; }
        public double accuracy { get; set; }
    }

    public class Datum
    {
        public InvoiceNumber invoice_number { get; set; }                                      
        public PoNumber po_number { get; set; }
        public InvoiceDate invoice_date { get; set; }
        public InvoiceDueDate invoice_due_date { get; set; }
        public string payment_terms { get; set; }
        public string irn { get; set; }
        public string ewaybill_number { get; set; }
        public string supplier_gstin { get; set; }
        [JsonProperty(PropertyName = "supplier_gstin1")]
        public SupplierGstin supplier_gst { get; set; }
        public SupplierName supplier_name { get; set; }
        public string buyer_gstin { get; set; }
        public BuyerName buyer_name { get; set; }
        public string ship_to_gstin { get; set; }
        public TotalTaxable total_taxable { get; set; }
        public string total_igst { get; set; }
        public TotalCgst total_cgst { get; set; }
        public TotalSgst total_sgst { get; set; }
        public string total_cess { get; set; }
        public TotalTaxAmount total_tax_amount { get; set; }
        public InvoiceAmount invoice_amount { get; set; }
        public string other_charges { get; set; }
        public Discount discount { get; set; }
        public SupplierAddress supplier_address { get; set; }
        public BuyerAddress buyer_address { get; set; }
        public ShippingAddress shipping_address { get; set; }
        public string supplier_email { get; set; }   
        ////
    }

    public class Discount
    {
        public string value { get; set; }
        public double accuracy { get; set; }
    }

    public class InvoiceAmount
    {
        public double value { get; set; }
        public double accuracy { get; set; }
    }

    public class InvoiceDate
    {
        public string value { get; set; }
        public double accuracy { get; set; }
    }

    public class InvoiceDueDate
    {
        public string value { get; set; }
        public double accuracy { get; set; }
    }

    public class InvoiceNumber
    {
        public string value { get; set; }
        public double accuracy { get; set; }
    }

    public class PoNumber
    {
        public string value { get; set; }
        public double accuracy { get; set; }
    }

    public class OcrFileUploadResponseDataContract
    {
        public bool status { get; set; }
        public string message { get; set; }
        public List<Datum> data { get; set; }
        //public List<List<List<string>>> table_data { get; set; }
        public List<List<object>> table_data { get; set; }
    }

    public class ShippingAddress
    {
        public string value { get; set; }
        public double accuracy { get; set; }
    }

    public class SupplierAddress
    {
        public string value { get; set; }
        public double accuracy { get; set; }
    }

    public class SupplierName
    {
        public string value { get; set; }
        public double accuracy { get; set; }
    }

    public class TotalCgst
    {
        public double value { get; set; }
        public double accuracy { get; set; }
    }

    public class TotalSgst
    {
        public double value { get; set; }
        public double accuracy { get; set; }
    }

    public class TotalTaxable
    {
        public double value { get; set; }
        public double accuracy { get; set; }
    }

    public class TotalTaxAmount
    {
        public double value { get; set; }
        public int accuracy { get; set; }
    }


    ///////////////////////////////////////////
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
   

   

    public class SupplierGstin
    {
        public string value { get; set; }
        public double accuracy { get; set; }
    }
}
