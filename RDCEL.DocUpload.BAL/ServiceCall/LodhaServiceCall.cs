using GraspCorn.Common.Helper;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.BAL.ServiceCall
{
    public class LodhaServiceCall
    {
        #region Lodha  service Call

        /// <summary>
        /// Method to POST form-data
        /// </summary>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string Rest_InvokeLodhaServiceFormData(string url, Method methodType, object content = null)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            IRestResponse getResponse = null;
            string jsonString = string.Empty;
            string responseString = string.Empty;
            string ApiKey = ConfigurationManager.AppSettings["LodhaApiKey"].ToString();
            string Vendor = ConfigurationManager.AppSettings["LodhaVendorId"].ToString();
            try
            {
                var client = new RestClient(url);
                var request = new RestRequest();
                request.Method = methodType;
                request.AddHeader("vendorId", Vendor);
                request.AddHeader("apiKey", ApiKey);
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
                LibLogging.WriteErrorToDB("ServiceCall URL: " + url, "Rest_InvokeLodhaServiceFormData", ex);
            }
            return responseString;

        }
        #endregion
    }
}
