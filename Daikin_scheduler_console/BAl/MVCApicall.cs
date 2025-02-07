using Daikin_scheduler_console.Model;
using GraspCorn.Common.Helper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using RDCEL.DocUpload.DataContract.SponsorModel;

namespace Daikin_scheduler_console.BAl
{
    public class MVCApicall
    {
        #region Exchange Order Update Status  service Call

        /// <summary>
        /// Method to POST form-data
        /// </summary>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string Rest_TokenAPI(ProductOrderStatusDataContract productOrderStatusDataContract)
        {
            TokenResponse token = new TokenResponse();
            IRestResponse getResponse = null;
            string jsonString = string.Empty;
            string responseString = string.Empty;
            string url = "http://103.127.146.29/QA/token";
            string urlPostStatus = "http://103.127.146.29/QA/api/Sponsor/ProductOrderStatusChange";
            try
            {

                var client = new RestClient(url);
                var request = new RestRequest();
                request.Method = Method.POST;
                request.AddHeader("content-type", "application/json");
                request.AddParameter("application/x-www-form-urlencoded", $"grant_type=password&username=siel@digi2l.in&password=S@msung!EL22", ParameterType.RequestBody);
                getResponse = client.Execute(request);
                responseString = getResponse.Content;
                if (!string.IsNullOrEmpty(responseString))
                {
                    token=JsonConvert.DeserializeObject<TokenResponse>(responseString);
                    if (token != null)
                    {
                        string BearerToken = token.token_type + " " + token.access_token;
                        var clientnew = new RestClient(urlPostStatus);
                        var requestnew = new RestRequest();
                        request.Method = Method.POST;
                        request.AddHeader("Authorization", BearerToken);
                        request.AddHeader("content-type", "application/json");
                        if (productOrderStatusDataContract != null)
                        {
                            jsonString = JsonConvert.SerializeObject(productOrderStatusDataContract);
                            request.AddJsonBody(jsonString);
                        }
                        getResponse = client.Execute(request);
                        responseString = getResponse.Content;
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ServiceCall URL: " + url, "Rest_Invoke247AroundServiceFormData", ex);
            }
            return responseString;

        }
        #endregion
    }
}
