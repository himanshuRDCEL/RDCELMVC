using GraspCorn.Common.Constant;
using GraspCorn.Common.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using RDCEL.DocUpload.BAL.Common;
using RDCEL.DocUpload.BAL.ProcessAPI;
using RDCEL.DocUpload.BAL.SponsorsApiCall;
using RDCEL.DocUpload.BAL.ZohoCreatorCall;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DataContract.Base;
using RDCEL.DocUpload.DataContract.BillCloud;
using RDCEL.DocUpload.DataContract.SponsorModel;
using RDCEL.DocUpload.DataContract.ZohoModel;

namespace RDCEL.DocUpload.Web.API.Controllers.api
{
    public class VoucherStatusController : BaseApiController
    {

        #region Variable Declarations
        SponserManager _sponserManager;
        ExchangeOrderRepository _exchangeOrderRepository;
        ExchangeOrderStatusRepository _exchangeOrderStatusRepository;
        CustomerDetailsRepository _customerDetailsRepository;
        NotificationManager _notificationManager;
        ExchangeOrderManager exchangeOrderManager;
        #endregion

        #region Update Voucher status from Capture to Redeem by regno

        [HttpGet]
        public HttpResponseMessage UpdateVoucherStatus(string RegNo)
        {
            SponsrOrderSyncManager sponsrOrderSyncManager = new SponsrOrderSyncManager();
            HttpResponseMessage response = null;
            VoucherVerificationResponseViewModel sucessObj = null;
            try
            {
                sucessObj = sponsrOrderSyncManager.UpdateVoucherstatusToRedeemed(RegNo);

                if (sucessObj != null)
                {
                    StatusDataContract structObj = new StatusDataContract(true, "success", "The voucher redeemed successfully.");
                    response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                }
                else
                {
                    StatusDataContract structObj = new StatusDataContract(false, "Status not updated.");
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

        #region Get sponsor exchange order detail

        [HttpGet]
        public HttpResponseMessage GetOrderStatus(string zohoOrderId)
        {
            _exchangeOrderRepository = new ExchangeOrderRepository();
            exchangeOrderManager = new ExchangeOrderManager();
            _exchangeOrderStatusRepository = new ExchangeOrderStatusRepository();
            _notificationManager = new NotificationManager();
            _customerDetailsRepository = new CustomerDetailsRepository();
            _sponserManager = new SponserManager();
            HttpResponseMessage response = null;
            SponserListDataContract sponserDC = new SponserListDataContract();
            SponserData sponser = new SponserData();
            try
            {

                bool flag = exchangeOrderManager.UpdateOrderStatus(zohoOrderId);


                if (flag)
                {
                    StatusDataContract structObj = new StatusDataContract(true, "Status Updated Successfully.");
                    response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                }
                else
                {
                    StatusDataContract structObj = new StatusDataContract(false, "Status not updated.");
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

        [HttpGet]
        public HttpResponseMessage GetOrderStatusFoCommentSection(string regno)
        {
            _exchangeOrderRepository = new ExchangeOrderRepository();
            exchangeOrderManager = new ExchangeOrderManager();
            _exchangeOrderStatusRepository = new ExchangeOrderStatusRepository();
            _notificationManager = new NotificationManager();
            _customerDetailsRepository = new CustomerDetailsRepository();
            _sponserManager = new SponserManager();
            HttpResponseMessage response = null;
            SponserListDataContract sponserDC = new SponserListDataContract();
            SponserData sponser = new SponserData();
            try
            {
                bool flag = false;
                if (!string.IsNullOrEmpty(regno))
                {
                    tblExchangeOrder exchangeOrder = _exchangeOrderRepository.GetSingle(x => x.RegdNo == regno);
                    if (exchangeOrder != null && !string.IsNullOrEmpty(exchangeOrder.ZohoSponsorOrderId))
                        flag = exchangeOrderManager.UpdateOrderStatus(exchangeOrder.ZohoSponsorOrderId);
                }

                if (flag)
                {
                    StatusDataContract structObj = new StatusDataContract(true, "Status Updated Successfully.");
                    response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                }
                else
                {
                    StatusDataContract structObj = new StatusDataContract(false, "Status not updated.");
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


        [HttpGet]
        public HttpResponseMessage UpdateMissing()
        {
            SponsrOrderSyncManager sponsrOrderSyncManager = new SponsrOrderSyncManager();
            HttpResponseMessage response = null;
            VoucherVerificationResponseViewModel sucessObj = null;
            VoucherVerificationRepository voucherVerificationRepository = new VoucherVerificationRepository();
            _exchangeOrderRepository = new ExchangeOrderRepository();
            try
            {
                List<tblVoucherVerfication> voucherVerfications = voucherVerificationRepository.GetList(x => !string.IsNullOrEmpty(x.VoucherCode) && x.IsVoucherused == false).ToList();
                foreach (tblVoucherVerfication item in voucherVerfications)
                {
                    tblExchangeOrder tblExchangeOrder = _exchangeOrderRepository.GetSingle(x => x.Id == item.ExchangeOrderId);
                    sponsrOrderSyncManager.UpdateVoucherstatusToRedeemedTemp(tblExchangeOrder.RegdNo);
                }

                if (sucessObj != null)
                {
                    StatusDataContract structObj = new StatusDataContract(true, "success", "The voucher redeemed successfully.");
                    response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                }
                else
                {
                    StatusDataContract structObj = new StatusDataContract(false, "Status not updated.");
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

        //#region Diakin POst call
        //[HttpPost]
        //public HttpResponseMessage PushDiakinCustomer(ProductOrderDataContract productOrderDataContract)
        //{
        //    SponsrOrderSyncManager sponsrOrderSyncManager = new SponsrOrderSyncManager();
        //    HttpResponseMessage response = null;
        //    ProductOrderResponseDataContract productOrderResponseDC = null;

        //    try
        //    {
        //                string  responseXML = sponsrOrderSyncManager.PushDiakinCustomer(productOrderDataContract);

        //                if (!string.IsNullOrEmpty(responseXML))
        //                {
        //                    StatusDataContract structObj = new StatusDataContract(true, "Customer Created Successfully", responseXML);
        //                    response = new HttpResponseMessage(HttpStatusCode.OK)
        //                    {
        //                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
        //                    };
        //                }
        //                else
        //                {
        //                    StatusDataContract structObj = new StatusDataContract(false, "Order not Created", productOrderResponseDC);
        //                    response = new HttpResponseMessage(HttpStatusCode.OK)
        //                    {
        //                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
        //                    };
        //                }
        //    }
        //    catch (Exception ex)
        //    {
        //        LibLogging.WriteErrorToDB("VoucherStatusController", "PushDiakinCustomer", ex);
        //        StatusDataContract structObj = new StatusDataContract(false, ex.Message);
        //        response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
        //        {
        //            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json")) //new StringContent("error"),                    
        //        };
        //    }


        //    return response;

        //}
        
        //[HttpPost]
        //public HttpResponseMessage PushDiakinServiceRequest(ProductOrderSoapServiceRequest productOrderDataContract)
        //{
        //    SponsrOrderSyncManager sponsrOrderSyncManager = new SponsrOrderSyncManager();
        //    HttpResponseMessage response = null;
        //    ProductOrderResponseDataContract productOrderResponseDC = null;

        //    try
        //    {
        //        string responseXML = sponsrOrderSyncManager.PushDiakinServiceRequest(productOrderDataContract);

        //        if (!string.IsNullOrEmpty(responseXML))
        //        {
        //            StatusDataContract structObj = new StatusDataContract(true, "Order Save Successfully", responseXML);
        //            response = new HttpResponseMessage(HttpStatusCode.OK)
        //            {
        //                Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
        //            };
        //        }
        //        else
        //        {
        //            StatusDataContract structObj = new StatusDataContract(false, "Order not Saved", productOrderResponseDC);
        //            response = new HttpResponseMessage(HttpStatusCode.OK)
        //            {
        //                Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
        //            };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LibLogging.WriteErrorToDB("VoucherStatusController", "PushDiakinServiceRequest", ex);
        //        StatusDataContract structObj = new StatusDataContract(false, ex.Message);
        //        response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
        //        {
        //            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json")) //new StringContent("error"),                    
        //        };
        //    }


        //    return response;

        //}


        //#endregion


    }
}