using GraspCorn.Common.Helper;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.BAL.ServiceCall
{
    public class BillCloudServiceCall
    {
        public static IRestResponse Rest_InvokeZohoInvoiceServiceForPlainText(string url, Method methodType, object content = null)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            IRestResponse getResponse = null;
            string aouthToken = string.Empty;
            string jsonString = string.Empty;
            try
            {
             
                    var client = new RestClient(url);
                    var request = new RestRequest();
                    request.Method = methodType;
                    request.AddHeader("content-type", "application/json");
                    request.AddHeader("apikey", ConfigurationManager.AppSettings["BillCloudKey"].ToString()); //Add Header tocken
                    if (content != null)
                    {
                        jsonString = JsonConvert.SerializeObject(content);
                        request.AddJsonBody(jsonString);
                    }
                    getResponse = client.Execute(request);
             
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("BillCloudServiceCall", "Rest_InvokeZohoInvoiceServiceForPlainText", ex);
            }
            return getResponse;

        }


        /// <summary>
        /// Method to send mail using mail jet 
        /// </summary>
        /// <param name="content">content</param>
        /// <returns>IRestResponse</returns>
        public static IRestResponse MailJetSendMailService(object content)
        {
            string jsonString = string.Empty;
            IRestResponse response = null;
            var request = new RestRequest();
            var client = new RestClient(ConfigurationManager.AppSettings["MailjetURL"].ToString());
            client.Authenticator = new HttpBasicAuthenticator(ConfigurationManager.AppSettings["MailjetAPIKey"].ToString(), ConfigurationManager.AppSettings["MailjetAPISecret"].ToString());
            request.Method = Method.POST;
            request.AddHeader("content-type", "application/json");
            if (content != null)
            {
                jsonString = JsonConvert.SerializeObject(content);
                request.AddJsonBody(jsonString);
            }

            response =  client.Execute(request);
            return response;
        }
    }
}
