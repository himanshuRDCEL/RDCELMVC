using RDCEL.DocUpload.BAL.SponsorsApiCall;
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
using RDCEL.DocUpload.DataContract.SponsorModel;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.BAL.ZohoCreatorCall;
using RDCEL.DocUpload.DAL.Repository;

namespace RDCEL.DocUpload.Web.API.Controllers.api
{
   
    public class OrderStatusController : BaseApiController
    {
        #region Variable declaration       
        SponserManager _sponserManager;
        ExchangeOrderRepository _exchangeOrderRepository;

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

        #region Get sponsor order status from UTC database
        [HttpGet]
        public HttpResponseMessage GetSponsorOrderStatusbyOrderNumberfromUTC(string OrderNo)
        {
            _sponserManager = new SponserManager();
            HttpResponseMessage response = null;
            _exchangeOrderRepository = new ExchangeOrderRepository();
            tblExchangeOrder SponserObj = null;
            OrderStatusDetailsFromUTCDataContract OrderStatusDetailsFromUTCDC = new OrderStatusDetailsFromUTCDataContract();
            try
            {
                if (!string.IsNullOrEmpty(OrderNo))
                {
                    SponserObj = _exchangeOrderRepository.GetSingle(x => x.SponsorOrderNumber.Equals(OrderNo));
                    if (SponserObj != null)
                    {
                        OrderStatusDetailsFromUTCDC.OrderNumber = SponserObj.SponsorOrderNumber;
                        OrderStatusDetailsFromUTCDC.OrderStatus = SponserObj.OrderStatus;
                        OrderStatusDetailsFromUTCDC.ExpectedDelivyDate = SponserObj.EstimatedDeliveryDate;
                        OrderStatusDetailsFromUTCDC.OrderCreatedDate = SponserObj.CreatedDate;
                        OrderStatusDetailsFromUTCDC.OrderModifiedDate = SponserObj.ModifiedDate;

                        StatusDataContract structObj = new StatusDataContract(true, "Success", OrderStatusDetailsFromUTCDC);
                        response = new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                        };
                    }
                    else
                    {
                        StatusDataContract structObj = new StatusDataContract(false, "No data found.");
                        response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                        {
                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                        };

                    }
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

        #region Get sponsor product exchange order Status detail

        [HttpGet]
        public HttpResponseMessage GetOrderStatus(string OrderNo)
        {
            SponsrOrderSyncManager sponsrOrderSyncManager = new SponsrOrderSyncManager();
            HttpResponseMessage response = null;
            OrderStatusDetailsDataContract orderStatusDetailsDC = null;
            ExchangeOrderRepository ExchangeOrderRepository = new ExchangeOrderRepository();
            try
            {
                tblExchangeOrder sponsorObj = ExchangeOrderRepository.GetSingle(x => x.SponsorOrderNumber.Equals(OrderNo));
                if (sponsorObj != null && sponsorObj.Id != 0)
                {
                    orderStatusDetailsDC = sponsrOrderSyncManager.ProcessGetOrderStatusInfo(sponsorObj.Id);

                    if (orderStatusDetailsDC != null)
                    {
                        StatusDataContract structObj = new StatusDataContract(true, "success", orderStatusDetailsDC);
                        response = new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                        };
                    }
                    else
                    {
                        StatusDataContract structObj = new StatusDataContract(false, "Data not found.");
                        response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                        {
                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                        };

                    }
                }
                else
                {
                    StatusDataContract structObj = new StatusDataContract(false, "order number not found in UTC database.");
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

        #endregion  
    }
}
