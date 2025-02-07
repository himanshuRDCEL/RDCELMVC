using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GraspCorn.Common.Helper
{
    public class ZohoServiceCalls
    {
        /// <summary>
        /// URL to generate the access token using refresh token
        /// </summary>
        private const string GenerateAccessTokenURL = "https://accounts.zoho.com/oauth/v2/token?refresh_token=[referesh-token]&client_id=[client-id]&client_secret=[client-secret]&redirect_uri=[redirect-uri]&grant_type=refresh_token";
        /// <summary>
        /// Key name for organizatin id
        /// </summary>
        private const string Zoho_OrganizationId = "X-com-zoho-invoice-organizationid";
        /// <summary>
        /// Key name for access key
        /// </summary>
        private const string Zoho_Authorization = "Authorization";
        /// <summary>
        /// URL for APIs
        /// </summary>
        private const string GenerateApiURL = "https://creatorapp.zoho.com/api/v2/accountsperthsecurityservices/mobileapp/form/<form_link_name>";

        #region Zoho Invoice Rest Call

        /// <summary>
        /// Method to invoke zoho invoice get calls
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="methodType">methodType</param>
        /// <param name="content">content</param>
        /// <returns>response string</returns>
        public static string Rest_InvokeZohoInvoiceServiceForMultiPart(string url, object content)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpResponseMessage getResponse = null;
            string aouthToken = string.Empty;
            string jsonString = string.Empty;
            string responseString = string.Empty;
            try
            {
                #region  Code to generate the auth tocken by refresh token                
                AccessAouthToken aouthObj = GenerateAccessTokenUsingRefreshToken();
                if (aouthObj != null)
                    aouthToken = aouthObj.access_token;
                #endregion

                if (!string.IsNullOrEmpty(aouthToken))
                {
                    HttpClient _httpclient = new HttpClient();
                    using (var multiPartStream = new MultipartFormDataContent())
                    {
                        if (content != null)
                        {
                            jsonString = JsonConvert.SerializeObject(content);

                            multiPartStream.Add(new StringContent(jsonString), "JSONString");
                            HttpRequestMessage requesttest = new HttpRequestMessage(HttpMethod.Post, url);
                            requesttest.Content = multiPartStream;
                            requesttest.Headers.Add(Zoho_Authorization, "Zoho-oauthtoken " + aouthToken);

                            HttpCompletionOption option = HttpCompletionOption.ResponseContentRead;
                            System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
                            getResponse = _httpclient.SendAsync(requesttest, option).Result;
                            if (getResponse.Content != null && getResponse.Content.ReadAsStringAsync().Result != null)
                            {
                                responseString = getResponse.Content.ReadAsStringAsync().Result;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ServiceCall URL: " + url, "Rest_InvokeZohoInvoiceServiceForMultiPart", ex);
            }
            return responseString;
        }

        /// <summary>
        /// Method to POST plain text
        /// </summary>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static IRestResponse Rest_InvokeZohoInvoiceServiceForPlainText(string url, Method methodType, object content = null)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            IRestResponse getResponse = null;
            string aouthToken = string.Empty;
            string jsonString = string.Empty;
            try
            {
                #region  Code to generate the auth tocken by refresh token                
                AccessAouthToken aouthObj = GenerateAccessTokenUsingRefreshToken();
                if (aouthObj != null)
                    aouthToken = aouthObj.access_token;
                #endregion

                if (!string.IsNullOrEmpty(aouthToken))
                {
                    var client = new RestClient(url);
                    var request = new RestRequest();
                    request.Method = methodType;
                    request.AddHeader("content-type", "application/json");
                    request.AddHeader(Zoho_Authorization, "Zoho-oauthtoken " + aouthToken);
                    if (content != null)
                    {
                        jsonString = JsonConvert.SerializeObject(content);
                        request.AddJsonBody(jsonString);
                    }
                    getResponse = client.Execute(request);
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ServiceCall URL: " + url, "Rest_InvokeZohoInvoiceServiceForMultiPart", ex);
            }
            return getResponse;

        }

        /// <summary>
        /// Method to rest API POST call for zoho 
        /// </summary>
        /// <param name="url">request URL</param>
        /// <returns>Plain text</returns>

        //public static IRestResponse Rest_InvokeZohoInvoiceServiceForPlainText(string url, object content)
        //{
        //    ServicePointManager.Expect100Continue = true;
        //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        //    IRestResponse getResponse = null;
        //    string aouthToken = string.Empty;
        //    string jsonString = string.Empty;
        //    try
        //    {
        //        #region  Code to generate the auth tocken by refresh token                
        //        AccessAouthToken aouthObj = GenerateAccessTokenUsingRefreshToken();
        //        if (aouthObj != null)
        //            aouthToken = aouthObj.access_token;
        //        #endregion

        //        if (!string.IsNullOrEmpty(aouthToken))
        //        {
        //            var client = new RestClient(url);
        //            var request = new RestRequest(Method.POST);
        //            request.AddHeader("content-type", "text/plain");
        //            request.AddHeader(Zoho_Authorization, "Zoho-oauthtoken " + aouthToken);


        //            if (content != null)
        //            {
        //                jsonString = JsonConvert.SerializeObject(content);
        //               // jsonString = @"{\r\n  \'data\': {\r\n    \'Day_due\': \'1-Tuesday\',\r\n    \'Job_description\': \'Hello, this is a new job\',\r\n    \'Job_date\': \'16\/03\/2021\',\r\n    \'Start_date_time\': \'16\/03\/2021 16:42:00\',\r\n    \'End_date_time\': \'17\/03\/2021 04:30:00\',\r\n    \'Allocated_to\': \'3354762000000189003\',\r\n    \'Active\': true,\r\n    \'Job_closed\': false\r\n    },\r\n \'result\': {\r\n    \'fields\': [\r\n      \'Single_Line\',\r\n      \'Email\'\r\n    ],\r\n    \'message\': true,\r\n    \'tasks\': true\r\n  }\r\n}";
        //                request.AddParameter("text/plain", jsonString);
        //            }
        //            getResponse = client.Execute(request);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LibLogging.WriteErrorToDB("ServiceCall URL: " + url, "Rest_InvokeZohoInvoiceServiceForMultiPart", ex);
        //    }
        //    return getResponse;

        //}


        /// <summary>
        /// Method to rest API GET call for zoho 
        /// </summary>
        /// <param name="url">request URL</param>
        /// <returns>json string</returns>
        public static async Task<string> InvokeZohoServiceForFormDataGet(string url)
        {
            string strResponse = string.Empty;
            string aouthToken = string.Empty;
            try
            {
                #region  Code to generate the auth tocken by refresh token                
                AccessAouthToken aouthObj = GenerateAccessTokenUsingRefreshToken();
                if (aouthObj != null)
                    aouthToken = aouthObj.access_token;
                #endregion

                if (!string.IsNullOrEmpty(aouthToken))
                {
                    HttpClient httpClient = new HttpClient();

                    httpClient.DefaultRequestHeaders.Add(Zoho_OrganizationId, ConfigurationManager.AppSettings["orgId"].ToString());
                    httpClient.DefaultRequestHeaders.Add(Zoho_Authorization, "Zoho-oauthtoken " + aouthToken);

                    HttpResponseMessage response = await httpClient.GetAsync(url);

                    response.EnsureSuccessStatusCode();
                    httpClient.Dispose();
                    strResponse = response.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ZohoServiceCalls", "InvokeZohoServiceForFormDataGet", ex);
            }
            return strResponse;
        }


        /// <summary>
        /// Method to post zoho data post call
        /// </summary>
        /// <param name="url">request url</param>
        /// <param name="content">json content</param>
        /// <returns>json string </returns>
        public static async Task<string> InvokeZohoServiceForFormDataPost(string url, string content = "")
        {
            string strResponse = string.Empty;
            string aouthToken = string.Empty;
            try
            {
                #region  Code to generate the auth tocken by refresh token                
                AccessAouthToken aouthObj = GenerateAccessTokenUsingRefreshToken();
                if (aouthObj != null)
                    aouthToken = aouthObj.access_token;
                #endregion

                if (!string.IsNullOrEmpty(aouthToken))
                {
                    HttpClient httpClient = new HttpClient();

                    httpClient.DefaultRequestHeaders.Add(Zoho_OrganizationId, ConfigurationManager.AppSettings["orgId"].ToString());
                    httpClient.DefaultRequestHeaders.Add(Zoho_Authorization, "Zoho-oauthtoken " + aouthToken);


                    MultipartFormDataContent form = new MultipartFormDataContent();
                    form.Add(new StringContent(content), "JSONString");

                    HttpResponseMessage response = await httpClient.PostAsync(url, form);

                    response.EnsureSuccessStatusCode();
                    httpClient.Dispose();
                    strResponse = response.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ZohoServiceCalls", "InvokeZohoServiceForFormDataPost", ex);
            }
            return strResponse;
        }


        /// <summary>
        /// Method to get the Zoho Invoice Aouth Token
        /// </summary>
        /// <returns></returns>
        public static AccessAouthToken GenerateAccessTokenUsingRefreshToken()
        {
            HttpWebResponse httpWebResp = null;
            AccessAouthToken aouthObj = null;
            try
            {
                string url = GenerateAccessTokenURL;
                url = url.Replace("[referesh-token]", ConfigurationManager.AppSettings["referesh-token"].ToString());
                url = url.Replace("[client-id]", ConfigurationManager.AppSettings["client-id"].ToString());
                url = url.Replace("[client-secret]", ConfigurationManager.AppSettings["client-secret"].ToString());
                url = url.Replace("[redirect-uri]", ConfigurationManager.AppSettings["redirect-uri"].ToString());
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";


                httpWebResp = (HttpWebResponse)request.GetResponse();
                if (httpWebResp != null)
                {
                    Stream stream = httpWebResp.GetResponseStream();
                    if (stream != null)
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string result = reader.ReadToEnd();
                            aouthObj = JsonConvert.DeserializeObject<AccessAouthToken>(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ZohoServiceCalls", "GenerateAccessTokenUsingRefreshToken", ex);
            }
            return aouthObj;
        }


        #endregion
    }

    public class AccessAouthToken
    {
        public string access_token { get; set; }
        public string api_domain { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }
    }

}
