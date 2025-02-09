using GraspCorn.Common.Helper;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DataContract.WhatsappTemplates;
using System.Net.Http;

namespace RDCEL.DocUpload.BAL.Common
{
    public class WhatsappNotificationManager
    {

        public static IRestResponse Rest_InvokeWhatsappserviceCall(string url, Method methodType, object content = null)
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
                request.AddHeader("x-api-key", ConfigurationManager.AppSettings["Yellow.Ai_ApiKey"].ToString()); //Add Header tocken
                if (content != null)
                {
                    jsonString = JsonConvert.SerializeObject(content);
                    request.AddJsonBody(jsonString);
                }
                getResponse = client.Execute(request);

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", ex);
            }
            return getResponse;

        }

        public async Task<HttpResponseDetails> SendWhatsAppMessageAsync(string templateId, string recipientNumber, List<string> templateParams)
        {
            string apiURL = ConfigurationManager.AppSettings["AiSensy_ApiURL"]?.ToString();
            string apiKey = ConfigurationManager.AppSettings["AiSensy_ApiKey"]?.ToString();

            if (string.IsNullOrEmpty(apiURL) || string.IsNullOrEmpty(apiKey))
            {
                throw new Exception("API URL/API Key missing");
            }

            var content = new
            {
                apiKey = apiKey,
                campaignName = templateId,
                destination = recipientNumber,
                templateParams = templateParams
            };

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var jsonContent = JsonConvert.SerializeObject(content);
                    var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync(apiURL, httpContent).ConfigureAwait(false);
                    Console.WriteLine($"Response Status: {response.StatusCode}");
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Response Content: {responseContent}");

                    return new HttpResponseDetails { Response = response, Content = responseContent };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"WhatsApp: {ex.Message}");
                throw;
            }

        }

        public class HttpResponseDetails
        {
            public HttpResponseMessage Response { get; set; }
            public string Content { get; set; }
        }

    }
}
