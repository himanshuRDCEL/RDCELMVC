using GraspCorn.Common.Helper;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using RDCEL.DocUpload.DataContract.OCRDataContract;

namespace RDCEL.DocUpload.BAL.ServiceCall
{
    public class OCR_Services
    {
        public static string Ocr_InvoiveAuthorization(string url, object content = null)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            IRestResponse getResponse = null;
            string jsonString = string.Empty;
            string responseString = string.Empty;
            // string ApiKey = ConfigurationManager.AppSettings["AuthorizationKey"].ToString();
            try
            {

                var client = new RestClient(url);
                var request = new RestRequest();
                request.Method = Method.POST;
                // request.AddHeader()
                request.AddHeader("Authorization", "JWT");
                request.AddHeader("content-type", "application/json");

                if (content != null)
                {
                    jsonString = JsonConvert.SerializeObject(content);
                    request.AddJsonBody(jsonString);
                }
                getResponse = client.Execute(request);
                responseString = getResponse.Content;

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ServiceCall URL: " + url, "Ocr_InvoiveAuthorization", ex);
            }
            return responseString;

        }
        public static string Ocr_InvoiceUpload(OcrFileUploadRequestDataContract content)
        {
            OcrFileUploadResponseDataContract fileUploadResponse = new OcrFileUploadResponseDataContract();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            IRestResponse getResponse = null;
            string jsonString = string.Empty;
            string responseString = string.Empty;
            string ApiKey = content.token;
            string Uploadurl = string.Empty;
            string fullFileName = content.InvoicePath;
            string file = ConfigurationManager.AppSettings["BaseURL"].ToString();

            string filenamewithpath = file + "Content/DB_Files/InvoiceImage/";
            try
            {

                Uploadurl = ConfigurationManager.AppSettings["requestOcrUploadUrl"].ToString();
                var client = new RestClient(Uploadurl);
                var request = new RestRequest("api/document", Method.POST);
                // request.Method = Method.POST;
                // request.AddHeader()
                request.AddHeader("Authorization", ApiKey);
                request.AddHeader("Subid", "435");
                request.AddHeader("Productid", "arap");
                request.AddHeader("content-type", "multipart/form-data");
                //var request = new RestRequest("api/document", Method.POST);


                request.AddFile("file_url", Path.GetFileNameWithoutExtension(filenamewithpath + fullFileName));
                // request.AddParameter("ReferenceType", ReferenceType.ToString());
                //request.AddParameter("RefId", StudioEventEntryId.ToString());

                request.AlwaysMultipartFormData = true;


                getResponse = client.Execute(request);
                responseString = getResponse.Content;

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ServiceCall URL: " + Uploadurl, "Ocr_InvoiveAuthorization", ex);
            }
            return responseString;

        }

    }
}