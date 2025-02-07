using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
//using System.Web.UI.WebControls;

namespace RDCEL.DocUpload.DataContract.OCRDataContract
{
    public class OcrFileUploadRequestDataContract
    {
       // public Image Invoice { get; set; }
        public string InvoicePath { get; set; }
        public string token { get; set; }
    }
}