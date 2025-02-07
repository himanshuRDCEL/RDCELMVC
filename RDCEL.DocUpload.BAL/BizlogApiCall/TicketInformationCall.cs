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
using RDCEL.DocUpload.BAL.Common;
using RDCEL.DocUpload.BAL.ServiceCall;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DataContract.Bizlog;

namespace RDCEL.DocUpload.BAL.BizlogApiCall
{
    public class TicketInformationCall
    {
        #region Variable Declaration
        //TicketRepository ticketRepository;
        //TicketStatusRepository ticketStatusRepository;
       // MailManager _mailManager;
        DateTime currentDatetime = DateTime.Now.TrimMilliseconds();

        #endregion

        #region Add Ticket in Bizlog
        /// <summary>
        /// Method to add the Ticket in Bizlog DB
        /// </summary>       
        /// <returns></returns>   
        public TicketResponceDataContract AddTicketToBizlog(TicketDataContract ticketDataContract)
        {
            TicketResponceDataContract TicketResponceDC = null;
            string responseString = string.Empty;
            string url = ConfigurationManager.AppSettings["CreateTicketUrl"].ToString();
            try
            {
                if (ticketDataContract != null)
                {

                    //IRestResponse response = BizlogServiceCall.Rest_InvokeBizlogSeviceFormData(url, Method.POST, ticketDataContract);
                    responseString = BizlogServiceCall.Rest_InvokeBizlogSeviceFormData(url, Method.POST, ticketDataContract);

                    if (responseString != null)
                    {
                        TicketResponceDC = JsonConvert.DeserializeObject<TicketResponceDataContract>(responseString);

                    }
                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("TicketManager", "AddTicketToBizlog", ex);
            }

            return TicketResponceDC;
        }
        #endregion

        #region Cancel Ticket in Bizlog
        /// <summary>
        /// Method to Cancel the Ticket in Bizlog DB
        /// </summary>       
        /// <returns></returns>   
        public TicketCancelResponseDataContract CancelTicketToBizlog(string ticketNo)
        {
            TicketCancelResponseDataContract ticketCancelResponceDC = null;
            string responseString = string.Empty;
            string url = ConfigurationManager.AppSettings["CancelTicketUrl"].ToString();
            try
            {
                if (ticketNo != null)
                {
                    responseString = BizlogServiceCall.Rest_InvokeBizlogSeviceFormData(url, Method.POST, null, ticketNo);

                    if (responseString != null)
                    {
                        ticketCancelResponceDC = JsonConvert.DeserializeObject<TicketCancelResponseDataContract>(responseString);

                    }
                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("TicketManager", "AddTicketToBizlog", ex);
            }

            return ticketCancelResponceDC;
        }
        #endregion


        #region Add Updated Ticket in Bizlog
        /// <summary>
        /// Method to add the Ticket in Bizlog DB
        /// </summary>       
        /// <returns></returns>   
        public UpdatedTicketResponceDataContract AddUpdatedTicketToBizlog(UpdatedTicketDataContract UpdatedticketDataContract)
        {
            UpdatedTicketResponceDataContract TicketResponceDC = null;
            string responseString = string.Empty;
            string url = ConfigurationManager.AppSettings["UpdatedCreateTicketUrl"].ToString();
            try
            {
                if (UpdatedticketDataContract != null)
                {
                    //IRestResponse response = BizlogServiceCall.Rest_InvokeBizlogSeviceFormData(url, Method.POST, ticketDataContract);
                    responseString = BizlogServiceCall.Rest_InvokeUPdatedBlowHornServiceFormData(url, Method.PUT, UpdatedticketDataContract);

                    if (responseString != null )
                    {
                        TicketResponceDC = JsonConvert.DeserializeObject<UpdatedTicketResponceDataContract>(responseString);

                    }
                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("TicketManager", "AddTicketToBizlog", ex);
            }

            return TicketResponceDC;
        }
        #endregion

    }
}
