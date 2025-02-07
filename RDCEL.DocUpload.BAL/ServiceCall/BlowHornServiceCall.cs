using GraspCorn.Common.Helper;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.BAL.ServiceCall
{
    public class BlowHornServiceCall
    {
        #region Blow Horn service Call

        /// <summary>
        /// Method to POST form-data
        /// </summary>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string Rest_InvokeBlowHornServiceFormData(string url, Method methodType, object content = null, string ticketNo = null)
        {
            IRestResponse getResponse = null;
            string jsonString = string.Empty;
            string responseString = string.Empty;
            string ApiKey = ConfigurationManager.AppSettings["API_KEY"].ToString();
            try
            {

                var client = new RestClient(url);
                var request = new RestRequest();
                request.Method = methodType;
                request.AddHeader("content-type", "application/json");
                request.AddHeader("API_KEY", ApiKey);
                if (content != null)
                {
                    jsonString = JsonConvert.SerializeObject(content);
                    request.AddJsonBody(jsonString);
                }
                getResponse = client.Execute(request);
                responseString = getResponse.Content;

                //HttpClient _httpclient = new HttpClient();
                //MultipartFormDataContent multiPartStream = new MultipartFormDataContent();
                ////multiPartStream.Add(new StringContent("31b26b73e64f56d6354eb632dafd7fe2"), "apiToken");
                ////multiPartStream.Add(new StringContent("858c0120-7f10-4c46-b16f-9a3cb560561c"), "retailerId");
                //multiPartStream.Add(new StringContent(ApiKey), "apiKey");
                //if (ticketNo != null)
                //{
                //    multiPartStream.Add(new StringContent(ticketNo), "ticketNo");
                //}
                //if (content != null)
                //{
                //    jsonString = JsonConvert.SerializeObject(content);
                //}

                //multiPartStream.Add(new StringContent(jsonString), "fields");
                //HttpRequestMessage requesttest = new HttpRequestMessage(HttpMethod.Post, url);
                //requesttest.Content = multiPartStream;

                //HttpCompletionOption option = HttpCompletionOption.ResponseContentRead;
                //System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
                //getResponse = _httpclient.SendAsync(requesttest, option).Result;
                //if (getResponse.Content != null && getResponse.Content.ReadAsStringAsync().Result != null)
                //{
                //    responseString = getResponse.Content.ReadAsStringAsync().Result;
                //}
                //getResponse = client.Execute(request);

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ServiceCall URL: " + url, "Rest_InvokeBlowHornSeviceFormData", ex);
            }
            return responseString;

        }
        #endregion
    }
}