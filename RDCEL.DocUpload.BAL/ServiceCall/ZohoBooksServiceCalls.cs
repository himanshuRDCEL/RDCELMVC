using GraspCorn.Common.Helper;
using Newtonsoft.Json;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DAL.Repository;
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

namespace RDCEL.DocUpload.BAL.ServiceCall
{
    public class ZohoBooksServiceCalls
    {
        /// <summary>
        /// URL to generate the access token using refresh token
        /// </summary>
        //private const string GenerateAccessTokenURL = "https://accounts.zoho.com/oauth/v2/token?refresh_token=[referesh-token]&client_id=[client-id]&client_secret=[client-secret]&redirect_uri=[redirect-uri]&grant_type=refresh_token";
        private const string GenerateAccessTokenURL = "https://accounts.zoho.com/oauth/v2/token?refresh_token=[referesh-token]&client_id=[client-id]&client_secret=[client-secret]&grant_type=refresh_token";

        /// <summary>
        /// Key name for organizatin id
        /// </summary>
        private const string Zoho_OrganizationId = "X-com-zoho-invoice-organizationid";
        /// <summary>
        /// Key name for access key
        /// </summary>
        private const string Zoho_Authorization = "Authorization";

        #region Zoho Invoice Rest Call

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
        /// Method to get the Zoho Invoice Aouth Token
        /// </summary>
        /// <returns></returns>
        public static AccessAouthToken GenerateAccessTokenUsingRefreshToken()
        {
            HttpWebResponse httpWebResp = null;
            AccessAouthToken aouthObj = null;
            try
            {
                //Code to add auth token
                string url = GenerateAccessTokenURL;
                url = url.Replace("[referesh-token]", ConfigurationManager.AppSettings["referesh-token-books"].ToString());
                url = url.Replace("[client-id]", ConfigurationManager.AppSettings["client-id"].ToString());
                url = url.Replace("[client-secret]", ConfigurationManager.AppSettings["client-secret"].ToString());
                // url = url.Replace("[redirect-uri]", ConfigurationManager.AppSettings["redirect-uri"].ToString());
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

}
