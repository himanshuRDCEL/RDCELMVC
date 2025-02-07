using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using RDCEL.DocUpload.BAL.SponsorsApiCall;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DataContract.Base;
using RDCEL.DocUpload.DataContract.TimeSlot;

namespace RDCEL.DocUpload.Web.API.Controllers.api
{
    [Authorize]
    public class TimeSlotController : BaseApiController
    {
        #region Variable declaration

        ExchangeOrderRepository _exchangeOrderRepository;
        ExchangeOrderManager _exchangeOrderManager;
        #endregion
        // GET: TimeSlot
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


        [HttpGet]
        public HttpResponseMessage GetTimeSlot(string selectedDate)
        {
            _exchangeOrderRepository = new ExchangeOrderRepository();
            _exchangeOrderManager = new ExchangeOrderManager();
            HttpResponseMessage response = null;

            string username = string.Empty;
            try
            {
                List<timeSlotDataContract> timeSlotDataContracts = null;
                if (HttpContext.Current != null && HttpContext.Current.User != null
                            && HttpContext.Current.User.Identity.Name != null)
                {
                    username = HttpContext.Current.User.Identity.Name;
                    if (!string.IsNullOrEmpty(username))
                    {
                        DateTime d;
                        bool chValidity = false;
                        chValidity = DateTime.TryParseExact(selectedDate, "MM/dd/yyyy",
                            CultureInfo.InvariantCulture, DateTimeStyles.None, out d);

                        if (selectedDate == string.Empty || selectedDate == null || selectedDate == "0" || chValidity is false)
                        {
                            StatusDataContract structObj = new StatusDataContract(false, "Invalid Parameter");
                            response = new HttpResponseMessage(HttpStatusCode.OK)
                            {
                                Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                            };
                            return response;
                        }
                        if(chValidity is true)
                        {
                            timeSlotDataContracts = _exchangeOrderManager.AvailableTimeSLot(selectedDate);
                        }   
                    }
                }
                else
                {
                    StatusDataContract structObj = new StatusDataContract(false, "token invalid");
                    response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                    return response;
                }
                if (timeSlotDataContracts != null)
                {
                    StatusDataContract structObj = new StatusDataContract(true, "Success", timeSlotDataContracts);
                    response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                }
                else
                {
                    StatusDataContract structObj = new StatusDataContract(false, "No data found");
                    response = new HttpResponseMessage(HttpStatusCode.BadRequest)
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
    }
}