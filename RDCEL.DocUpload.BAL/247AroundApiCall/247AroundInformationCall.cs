using GraspCorn.Common.Helper;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCEL.DocUpload.BAL.ServiceCall;
using RDCEL.DocUpload.DataContract._247aroundService;

namespace RDCEL.DocUpload.BAL._247AroundApiCall
{
    public class _247AroundInformationCall
    {
        #region SubmitRequest in 247Around 
        /// <summary>
        /// Method to  SubmitRequest in 247Around  DB
        /// </summary>       
        /// <returns></returns>   
        public _247ResponseDataContract SubmitRequest(_247AroundDataContract aroundDataContract)
        {
            _247ResponseDataContract aroundResponseDC = null;
            Response response = new Response();
            string responseString = string.Empty;
            string url = ConfigurationManager.AppSettings["ServiceRequest"].ToString();
            try
            {
                if (aroundDataContract != null)
                {

                    //IRestResponse response = BizlogServiceCall.Rest_InvokeBizlogSeviceFormData(url, Method.POST, ticketDataContract);
                    responseString = _247AroundServiceCall.Rest_Invoke247AroundServiceFormData(url, Method.POST, aroundDataContract);
                    if (responseString != null)
                    {

                        aroundResponseDC = JsonConvert.DeserializeObject<_247ResponseDataContract>(responseString);
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("_247AroundApiCall", "SubmitRequest", ex);
            }

            return aroundResponseDC;
        }
        #endregion
    }
}
