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

        public async Task<IRestResponse> SendWhatsAppMessage(string campaignName, string destination, string userName, string[] templateParams)
        {
            string apiURL = ConfigurationManager.AppSettings["AiSensy_ApiURL"].ToString();
            var content = new
            {
                apiKey = ConfigurationManager.AppSettings["AiSensy_ApiKey"].ToString(),
                campaignName = campaignName,
                destination = destination, // Recipient phone number with country code
                userName = userName,
                templateParams = templateParams // Template parameters
            };

            try
            {
                var client = new RestClient(apiURL);
                var request = new RestRequest(Method.POST);
                request.AddHeader("content-type", "application/json"); // Add Content-Type header
                request.AddJsonBody(JsonConvert.SerializeObject(content)); // Serialize and add request body

                var response = await client.ExecuteAsync(request);
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending WhatsApp message: {ex.Message}");
                throw;
            }
        }

        public async Task<IRestResponse> SendWhatsAppMessage(string templateId, string recipientNumber, List<string> templateParams)
        {
            string apiURL = ConfigurationManager.AppSettings["AiSensy_ApiURL"].ToString();

            var content = new
            {
                apiKey = ConfigurationManager.AppSettings["AiSensy_ApiKey"].ToString(),
                campaignName = templateId, // Template ID dynamically set ho rahi hai
                destination = recipientNumber, // Recipient phone number
                templateParams = templateParams // Dynamic template parameters
            };

            try
            {
                var client = new RestClient(apiURL);
                var request = new RestRequest(Method.POST);
                request.AddHeader("content-type", "application/json"); // Add Content-Type header
                request.AddJsonBody(JsonConvert.SerializeObject(content)); // Serialize and add request body

                var response = await client.ExecuteAsync(request);
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending WhatsApp message: {ex.Message}");
                throw;
            }
        }

    }
}
