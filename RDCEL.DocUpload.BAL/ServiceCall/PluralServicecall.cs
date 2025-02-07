using GraspCorn.Common.Helper;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.BAL.ServiceCall
{
    public class PluralServicecall
    {
        #region Plural  service Call

        /// <summary>
        /// Method to POST form-data
        /// </summary>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static IRestResponse Rest_InvokePluralServiceCall(string url, Method methodType,string Hasstring, object content = null)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            IRestResponse getResponse = null;
            string jsonString = string.Empty;
            string responseString = string.Empty;
            string secretKey = ConfigurationManager.AppSettings["SecretKeyPlural"].ToString();
            try
            {
                var client = new RestClient(url);
                var request = new RestRequest();
                request.Method = methodType;
                request.AddHeader("x-verify", Hasstring);
                request.AddHeader("content-type", "application/json");
                if (content != null)
                {
                    jsonString = JsonConvert.SerializeObject(content);
                    request.AddJsonBody(jsonString);
                }
                getResponse = client.Execute(request);
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("PluralServicecall", "Rest_InvokePluralServiceCall", ex);
            }
            return getResponse;

        }
        #endregion

        #region GetOrderStatus for payment
        public static IRestResponse Rest_InvokePluralServiceCallGetPaymentStatus(string url, Method methodType,string Encryption, object content = null)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            IRestResponse getResponse = null;
            string jsonString = string.Empty;
            string responseString = string.Empty;
            string secretKey = ConfigurationManager.AppSettings["SecretKeyPlural"].ToString();
            try
            {
                var client = new RestClient(url);
                var request = new RestRequest();
                request.Method = methodType;
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Authorization", Encryption);
               
                if (content != null)
                {
                    //jsonString = JsonConvert.SerializeObject(content);
                    //request.AddJsonBody(jsonString);
                }
                getResponse = client.Execute(request);
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("PluralServicecall", "Rest_InvokePluralServiceCallGetPaymentStatus", ex);
            }
            return getResponse;

        }
        #endregion

        #region 
        public static async Task<string> MakeRequest(string url, string username, string password)
        {
            var handler = new HttpClientHandler
            {
                Credentials = new NetworkCredential(username, password)
            };

            var client = new HttpClient(handler);

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var response = await client.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();

            return content;
        }
        #endregion 
    }

}
