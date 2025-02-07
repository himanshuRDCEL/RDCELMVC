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

        public static async Task<IRestResponse> SendWhatsAppMessage()
        {
            string url = "https://backend.aisensy.com/campaign/t1/api/v2"; // API URL

            // Prepare the content for the API call
            var content = new
            {
                apiKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjY3YTM0MGFlMWJlYTVhMjhlMDQ4ZGU5NiIsIm5hbWUiOiJST0NLSU5HREVBTFMgQ0lSQ1VMQVIgRUNPTk9NWSBMSU1JVEVEIiwiYXBwTmFtZSI6IkFpU2Vuc3kiLCJjbGllbnRJZCI6IjY3YTM0MGFlMWJlYTVhMjhlMDQ4ZGU5MCIsImFjdGl2ZVBsYW4iOiJGUkVFX0ZPUkVWRVIiLCJpYXQiOjE3Mzg3NTIxNzR9.7OUECmQ57i5Yvus_AtNjPQiEGY9VxpXwkSB_Bhdve-M", // Your API Key
                campaignName = "Utility Test Campaign",
                destination = "8962537774", // Ensure this is the correct phone number with country code
                userName = "Sakshi",
                templateParams = new string[] { "Sakshi" } // Parameters for your template
            };

            try
            {
                var client = new RestClient(url);  // Create the RestClient
                var request = new RestRequest();   // Create a new RestRequest

                request.Method = Method.POST;      // Set the request method to POST
                request.AddHeader("content-type", "application/json");  // Add Content-Type header
                request.AddHeader("x-api-key", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjY3YTM0MGFlMWJlYTVhMjhlMDQ4ZGU5NiIsIm5hbWUiOiJST0NLSU5HREVBTFMgQ0lSQ1VMQVIgRUNPTk9NWSBMSU1JVEVEIiwiYXBwTmFtZSI6IkFpU2Vuc3kiLCJjbGllbnRJZCI6IjY3YTM0MGFlMWJlYTVhMjhlMDQ4ZGU5MCIsImFjdGl2ZVBsYW4iOiJGUkVFX0ZPUkVWRVIiLCJpYXQiOjE3Mzg3NTIxNzR9.7OUECmQ57i5Yvus_AtNjPQiEGY9VxpXwkSB_Bhdve-M");  // Add API key header

                // Serialize content to JSON and add it to the request body
                var jsonString = JsonConvert.SerializeObject(content);
                request.AddJsonBody(jsonString);

                // Send the request asynchronously
                var response = await client.ExecuteAsync(request);

                // Return the response directly
                return response;
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

    }
}
