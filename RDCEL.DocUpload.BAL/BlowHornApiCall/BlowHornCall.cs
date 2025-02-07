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
using RDCEL.DocUpload.DataContract.BlowHorn;

namespace RDCEL.DocUpload.BAL.BlowHornApiCall
{
   public class BlowHornCall
    {
        #region Create ShipMent in Blow Horn
        /// <summary>
        /// Method to  Create ShipMent in Blow Horn DB
        /// </summary>       
        /// <returns></returns>   
        public BlowHornRreponseDataContract CreateShipMent(BlowHornDataContract blowDataContract)
        {
            BlowHornRreponseDataContract blowHornResponse = null;
            string responseString = string.Empty;
            string url = ConfigurationManager.AppSettings["CreateShipment"].ToString();
            try
            {
                if (blowDataContract != null)
                {

                    //IRestResponse response = BizlogServiceCall.Rest_InvokeBizlogSeviceFormData(url, Method.POST, ticketDataContract);
                    responseString = BlowHornServiceCall.Rest_InvokeBlowHornServiceFormData(url, Method.POST, blowDataContract);

                    if (responseString != null)
                    {
                        blowHornResponse = JsonConvert.DeserializeObject<BlowHornRreponseDataContract>(responseString);

                    }
                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("BlowHornCall", "CreateShipMent", ex);
            }

            return blowHornResponse;
        }
        #endregion

        //#region Cancel ShipMent in Blow Horn
        ///// <summary>
        ///// Method to Cancel the ShipMent in Blow Horn DB
        ///// </summary>       
        ///// <returns></returns>   
        //public TicketCancelResponseDataContract CancelTicketToBizlog(string ticketNo)
        //{
        //    TicketCancelResponseDataContract ticketCancelResponceDC = null;
        //    string responseString = string.Empty;
        //    string url = ConfigurationManager.AppSettings["CancelTicketUrl"].ToString();
        //    try
        //    {
        //        if (ticketNo != null)
        //        {
        //            responseString = BizlogServiceCall.Rest_InvokeBizlogSeviceFormData(url, Method.POST, null, ticketNo);

        //            if (responseString != null)
        //            {
        //                ticketCancelResponceDC = JsonConvert.DeserializeObject<TicketCancelResponseDataContract>(responseString);

        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        LibLogging.WriteErrorToDB("TicketManager", "AddTicketToBizlog", ex);
        //    }

        //    return ticketCancelResponceDC;
        //}
        //#endregion



    }
}