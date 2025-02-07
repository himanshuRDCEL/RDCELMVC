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
using RDCEL.DocUpload.DataContract.MahindraLogistics;

namespace RDCEL.DocUpload.BAL.MahindraApicall
{
    public class MahindraInformationCall
    {
        #region SubmitRequest in 247Around 
        /// <summary>
        /// Method to  PlaceSingleOrder in Mahindra(Whizzard)  DB
        /// </summary>       
        /// <returns></returns>   
        public MahindraLogisticsResponseDataContract PlaceSingleOrder(MahindraLogisticsDataContract mahindraDataContract)
        {
            MahindraLogisticsResponseDataContract mahindraResponseDC = null;
            string responseString = string.Empty;
            string url = ConfigurationManager.AppSettings["apiURl"].ToString();
            try
            {
                if (mahindraDataContract != null)
                {

                    //IRestResponse response = BizlogServiceCall.Rest_InvokeBizlogSeviceFormData(url, Method.POST, ticketDataContract);
                    responseString = MahindraServiceCall.Rest_InvokeMahindraServiceFormData(url, Method.POST, mahindraDataContract);
                    if (responseString != null)
                    {

                        mahindraResponseDC = JsonConvert.DeserializeObject<MahindraLogisticsResponseDataContract>(responseString);
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("MahindraInformationCall", "PlaceSingleOrder", ex);
            }

            return mahindraResponseDC;
        }
        #endregion
    }
}
