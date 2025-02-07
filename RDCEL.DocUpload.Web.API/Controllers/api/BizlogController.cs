using RDCEL.DocUpload.DataContract.Base;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using RDCEL.DocUpload.BAL.ProcessAPI;
using RDCEL.DocUpload.DataContract.Bizlog;

namespace RDCEL.DocUpload.Web.API.Controllers.api
{
    [Authorize]
    public class BizlogController : BaseApiController
    {
        #region Variable declaration

        #endregion

        [HttpGet]
        public HttpResponseMessage GetTest()
        {
            HttpResponseMessage response = null;
            StatusDataContract structObj = new StatusDataContract(true, "API Working..", null);
            response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
            };
            return response;
        }


        #region Create Ticket detail

        [HttpPost]
        public HttpResponseMessage CreateTicket(TicketDataContract ticketDataContract)
        {
            TicketSyncManager ticketSyncManager = new TicketSyncManager();
            HttpResponseMessage response = null;
            TicketResponceDataContract TicketResponceDC = null;
            try
            {
                
                TicketResponceDC = ticketSyncManager.ProcessTicketInfo(ticketDataContract);
                
                if (TicketResponceDC != null && TicketResponceDC.success == true)
                {
                    
                    StatusDataContract structObj = new StatusDataContract(true, "Success", TicketResponceDC);
                    response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                }
                else
                {
                    StatusDataContract structObj = new StatusDataContract(false, "Unsuccess", TicketResponceDC);
                    response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };

                }
            }
            catch (Exception ex)
            {
                StatusDataContract structObj = new StatusDataContract(false, ex.Message);
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json")) //new StringContent("error"),                    
                };
            }


            return response;

        }

        #endregion

        #region Cancel Ticket 

        [HttpPost]
        public HttpResponseMessage CancelTicket(string ticketNo)
        {
            TicketSyncManager ticketSyncManager = new TicketSyncManager();
            HttpResponseMessage response = null;
            TicketCancelResponseDataContract TicketCancelResponceDC = null;
            try
            {

                TicketCancelResponceDC = ticketSyncManager.ProcessCancelTicketInfo(ticketNo);

                if (TicketCancelResponceDC != null && TicketCancelResponceDC.success == true)
                {

                    StatusDataContract structObj = new StatusDataContract(true, "Success", TicketCancelResponceDC);
                    response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                }
                else
                {
                    StatusDataContract structObj = new StatusDataContract(false, "No data found.");
                    response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };

                }
            }
            catch (Exception ex)
            {
                StatusDataContract structObj = new StatusDataContract(false, ex.Message);
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json")) //new StringContent("error"),                    
                };
            }


            return response;

        }

        #endregion

        #region Post Ticket Status

        [HttpPost]
        public HttpResponseMessage UpdateTicketStatus(TicketStatusDataContract ticketStatusDataContract)
        {
            TicketSyncManager ticketSyncManager = new TicketSyncManager();
            HttpResponseMessage response = null;            
            string tickeNo = null;

            try
            {

                tickeNo = ticketSyncManager.ProcessTicketStatusInfo(ticketStatusDataContract);

                //Add method which will update the ticket stats in Zoho
                if(tickeNo != null)
                {

                }

                if (tickeNo != null )
                {
                    StatusDataContract structObj = new StatusDataContract(true, "Status Changed Successfully", tickeNo);
                    response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                }
                else
                {
                    StatusDataContract structObj = new StatusDataContract(false, "No data found.");
                    response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };

                }
            }
            catch (Exception ex)
            {
                StatusDataContract structObj = new StatusDataContract(false, ex.Message);
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json")) //new StringContent("error"),                    
                };
            }


            return response;

        }

        #endregion

    }
}
