using GraspCorn.Common.Helper;
using System;
using System.Configuration;
using System.Web;
using System.Drawing;

using RDCEL.DocUpload.BAL.ServiceCall;
using RDCEL.DocUpload.DataContract.OCRDataContract;
using System.Web.UI.WebControls;

namespace RDCEL.DocUpload.BAL.OcrImageReaderCall
{
    public class OcrServiceCall
    {
        //OCR_Services ocrauthCall;
        public string OCRauthorization()
        {

            OCRRequestDataContract authrequest = new OCRRequestDataContract();
            OcrFileUploadRequestDataContract ocrFileUploadRequestDataContract = new OcrFileUploadRequestDataContract();
            string authToken = string.Empty;
            //IRestResponse response = null;
            string url = string.Empty;
            string Uploadurl = string.Empty;
            string file = string.Empty;
            string filenamewithpath = string.Empty;
      

            // IRestResponse getResponse = null;
            try
            {
                authrequest.username = ConfigurationManager.AppSettings["ocrUserName"].ToString();
                authrequest.password = ConfigurationManager.AppSettings["ocrpassword"].ToString();
                url = ConfigurationManager.AppSettings["requestAuthUrl"].ToString();

                authToken = OCR_Services.Ocr_InvoiveAuthorization(url, authrequest);

            }

            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("OcrServiceCall", "OCRauthorization", ex);
            }
            return authToken;


        }
       
    }
}
