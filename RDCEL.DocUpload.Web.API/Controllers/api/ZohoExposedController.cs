using GraspCorn.Common.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using RDCEL.DocUpload.BAL.ProcessAPI;
using RDCEL.DocUpload.BAL.SponsorsApiCall;
using RDCEL.DocUpload.BAL.ZohoCreatorCall;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DAL.Helper;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DataContract.Base;
using RDCEL.DocUpload.DataContract.Bizlog;
using RDCEL.DocUpload.DataContract.BlowHorn;
using RDCEL.DocUpload.DataContract.LogisticsDetails;
using RDCEL.DocUpload.DataContract.MahindraLogistics;
using RDCEL.DocUpload.DataContract.ZohoModel;

namespace RDCEL.DocUpload.Web.API.Controllers.api
{
    public class ZohoExposedController : ApiController
    {
        #region Variable Declaration
        BizlogTicketRepository _bizlogTicketRepository;
        MahindraLogisticsRepository _mahindraLogisticsRepository;
        ExchangeOrderRepository _exchangeOrderRepository;
        ABBRegistrationRepository aBBRegistrationRepository;
        EVCApprovedRepository _evcApprovedREpository;
        CustomerDetailsRepository _customerDetailsRepository;
        OrderTransactionRepository orderTransactionRepository;
        LogisticsRepository _logisticsRepository;
        EVCWalletRepository _walletRepository;
        EVCRegistrationRepository _evcRegistrationRepository;
        AbbRedemptionRepository _abbRedemptionRepository;

        Logging logging;
        #endregion
        #region Update Ticket Status methods
        [HttpPost]
        public HttpResponseMessage UpdateTicketStatus(TicketStatusDataContract ticketStatusDataContract)
        {
            TicketSyncManager ticketSyncManager = new TicketSyncManager();
            SponserManager sponserManager = new SponserManager();
            HttpResponseMessage response = null;
            SponserListDataContract sponserListDC = null;
            SponserData sponserObj = null;
            sponserObj = new SponserData();
            SponserFormResponseDataContract sponserResponseDC = null;

            logging = new Logging();
            try
            {
                int addLog = Convert.ToInt32(ConfigurationManager.AppSettings["EnableStatusUpdateAPI"]);
                //int addLog = 1;
                if (addLog == 1)
                {
                    if (ticketStatusDataContract != null)
                    {
                        if (ticketStatusDataContract.ticketNo != null)
                        {
                            //point 1 >> get zoho sponsor order form by id (zohoSalesOrderid)
                            sponserListDC = sponserManager.GetSponserOrderByBizlogTicketNo(ticketStatusDataContract.ticketNo);
                            if (sponserListDC != null && sponserListDC.data.Count > 0)
                            {
                                sponserObj = sponserListDC.data[0];
                                if (sponserObj != null)
                                {
                                    #region Code to store the Request body
                                    string jsonstring = JsonConvert.SerializeObject(ticketStatusDataContract);
                                    logging.WriteAPIRequestToDB("ZohoExposedController", "UpdateTicketStatus", sponserObj.Sp_Order_No, jsonstring);
                                    #endregion
                                    //point 2 >> Cancel the tciket in bizlog and local DB (already happening) 
                                    string ticketNo = ticketSyncManager.ProcessTicketStatusInfo(ticketStatusDataContract);

                                    if (ticketNo != null && sponserObj.ID != null)
                                    {
                                        LogisticStatusDataContract sponserUpdateObj = sponserManager.SetUpdateLogisticStatusObject(ticketStatusDataContract, sponserObj.ID);
                                        if (sponserUpdateObj != null)
                                            sponserResponseDC = sponserManager.UpdateLGCStatusDetail(sponserUpdateObj);

                                        if (sponserResponseDC != null)
                                        {
                                            if (sponserResponseDC.data != null && sponserResponseDC.data.ID != null)
                                            {
                                                try
                                                {
                                                    ExchangeOrderManager exchangeOrderManager = new ExchangeOrderManager();
                                                    exchangeOrderManager.UpdateOrderStatus(sponserObj.ID);
                                                }
                                                catch (Exception ex1)
                                                {
                                                    LibLogging.WriteErrorToDB("", "", ex1);
                                                }
                                                StatusDataContract structObj = new StatusDataContract(true, "Success", sponserResponseDC);
                                                response = new HttpResponseMessage(HttpStatusCode.OK)
                                                {
                                                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                                };
                                            }
                                            else
                                            {
                                                StatusDataContract structObj = new StatusDataContract(false, sponserResponseDC.message);
                                                response = new HttpResponseMessage(HttpStatusCode.OK)
                                                {
                                                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                                };

                                            }
                                        }

                                    }
                                    else
                                    {
                                        StatusDataContract structObj = new StatusDataContract(false, "Ticket Not Updated");
                                        response = new HttpResponseMessage(HttpStatusCode.OK)
                                        {
                                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                        };

                                    }

                                }
                            }
                            else
                            {
                                StatusDataContract structObj = new StatusDataContract(false, "Ticket No. not found in Sponsor");
                                response = new HttpResponseMessage(HttpStatusCode.OK)
                                {
                                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                };

                            }

                        }
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

        #region Other methods
        public ValidationResponse Validate(object model)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model);

            var isValid = Validator.TryValidateObject(model, context, results, true);

            return new ValidationResponse()
            {
                IsValid = isValid,
                Results = results
            };
        }

        #endregion


        #region Create Ticket With Bizlog
        [HttpGet]
        public HttpResponseMessage CreateTicketWithBizlog(string RegdNo, bool GenerateTicketWithoutBalanceCheck, string priority, int servicePartnerId)
        {
            TicketSyncManager ticketSyncManager = new TicketSyncManager();
            SponserManager sponserManager = new SponserManager();
            EVCManager eVCManager = new EVCManager();
            HttpResponseMessage response = null;
            SponserData sponserObj = new SponserData();
            EvcApprovedData evcApprovedObj = null;
            evcApprovedObj = new EvcApprovedData();
            evcApprovedObj = new EvcApprovedData();
            UpdatedTicketDataContract updatedTicketDataContract = null;
            UpdatedTicketResponceDataContract updatedTicketResponseDC = null;
            _bizlogTicketRepository = new BizlogTicketRepository();
            _exchangeOrderRepository = new ExchangeOrderRepository();
            _evcApprovedREpository = new EVCApprovedRepository();
            _customerDetailsRepository = new CustomerDetailsRepository();
            orderTransactionRepository = new OrderTransactionRepository();
            _logisticsRepository = new LogisticsRepository();
            _walletRepository = new EVCWalletRepository();
            aBBRegistrationRepository = new ABBRegistrationRepository();
            _evcRegistrationRepository = new EVCRegistrationRepository();
            _abbRedemptionRepository = new AbbRedemptionRepository();
            RunningBalance runingbalancecalculation = new RunningBalance();
            EVCdetailsDataContract evcdetailsDC = new EVCdetailsDataContract();
            CustomerandOrderDetailsDataContract customerDetails = new CustomerandOrderDetailsDataContract();
            decimal TotalofInprogress = 0;
            decimal TotalofDeliverd = 0;
            decimal EvcWallletAmount = 0;
            try
            {
                
                //point 1 >> get zoho sponsor order form by id (zohoSalesOrderid)
                tblOrderTran transactionObj = orderTransactionRepository.GetSingle(x => !string.IsNullOrEmpty(x.RegdNo) && x.RegdNo.ToLower() == RegdNo.ToLower() && x.IsActive == true);
                //point 2 >> Fill the TicketDataContract object using the response from point 1 
                if (transactionObj != null)
                {
                    tblLogistic lgcRecordObj = _logisticsRepository.GetSingle(x => x.RegdNo.ToLower() == RegdNo.ToLower() && x.IsActive == true);
                    //Ticket should not be generated already
                    if (lgcRecordObj == null)
                    {

                        if (transactionObj.OrderType == Convert.ToInt32(OrderTypeEnum.Exchange))
                        {
                            tblExchangeOrder exchangeOrder = _exchangeOrderRepository.GetSingle(x => x.Id == transactionObj.ExchangeId);
                            if (exchangeOrder != null)
                            {
                                customerDetails = ticketSyncManager.SetOrderDetailsObject(exchangeOrder);
                            }
                        }
                        else
                        {
                            tblABBRedemption abbredemptionObj = _abbRedemptionRepository.GetSingle(x => x.RedemptionId == transactionObj.ABBRedemptionId);
                            if (abbredemptionObj != null)
                            {
                                customerDetails = ticketSyncManager.SetOrderDetailsObjectForABB(abbredemptionObj);
                            }
                        }
                        if (customerDetails.Message == null || customerDetails.Message == "")
                        {
                            // get EVC Call allocation report by sponserID for Drop customer details
                            tblWalletTransaction EVCassigned = _walletRepository.GetSingle(x => x.OrderTransId == transactionObj.OrderTransId);
                            if (EVCassigned != null)
                            {
                                //Get evc data from tblevcRegistration
                                tblEVCRegistration evcRegistrartion = _evcRegistrationRepository.GetSingle(x => x.EVCRegistrationId == EVCassigned.EVCRegistrationId && x.ISEVCApprovrd == true);

                                if (evcRegistrartion != null)
                                {
                                    evcdetailsDC = ticketSyncManager.SetEvcDetailsDataContract(evcRegistrartion);
                                    List<tblWalletTransaction> walletsummary = _walletRepository.GetList(x => x.EVCRegistrationId == EVCassigned.EVCRegistrationId).ToList();

                                    ///
                                    foreach (var items in walletsummary)
                                    {
                                        if (items.OrderOfInprogressDate != null && items.OrderOfDeliverdDate == null && items.OrderOfCompleteDate == null)
                                        {
                                            if (items.OrderAmount != null)
                                            {
                                                TotalofInprogress = TotalofInprogress + Convert.ToDecimal(items.OrderAmount);

                                            }
                                            // tblWalletTransaction.OrderAmount += items.OrderAmount;
                                        }
                                        if (items.OrderOfInprogressDate != null && items.OrderOfDeliverdDate != null && items.OrderOfCompleteDate == null)
                                        {
                                            if (items.OrderAmount != null)
                                            {
                                                TotalofDeliverd = TotalofDeliverd + Convert.ToDecimal(items.OrderAmount);
                                            }
                                        }
                                    }
                                    runingbalancecalculation.TotalofInprogress = TotalofInprogress;
                                    runingbalancecalculation.TotalofDeliverd = TotalofDeliverd;
                                    EvcWallletAmount = Convert.ToDecimal(evcRegistrartion.EVCWalletAmount);
                                    runingbalancecalculation.runningBalance = EvcWallletAmount - (runingbalancecalculation.TotalofInprogress + runingbalancecalculation.TotalofDeliverd);
                                    ///
                                    bool isBlanaceTrue = true;
                                    if (!GenerateTicketWithoutBalanceCheck)
                                    {
                                        if (Convert.ToInt32(runingbalancecalculation.runningBalance) > 0)
                                            isBlanaceTrue = true;
                                        else
                                            isBlanaceTrue = false;
                                    }

                                    if (evcRegistrartion != null && isBlanaceTrue)
                                    {
                                        // set value in bizlog ticket object
                                        customerDetails.pickupPriority = priority;
                                        updatedTicketDataContract = ticketSyncManager.UpdatedSetTicketObjInfo(customerDetails, evcdetailsDC);

                                        if (updatedTicketDataContract != null)
                                        {
                                            //point 3 >> Create the tciket in bizlog and local DB (already happening)              
                                            updatedTicketResponseDC = ticketSyncManager.UpdatedProcessTicketInfo(updatedTicketDataContract);
                                            //point To Add Ticket data into logistics table and update status in wallet as well as status history table and exchange order table
                                            int ticketid = ticketSyncManager.AddTicketToTblLogistics(updatedTicketResponseDC.data.ticketNo, transactionObj.OrderTransId, servicePartnerId, transactionObj.RegdNo, customerDetails);
                                            //point 4 >> update the zoho sponsor order form by ticket id received from bizlog response
                                            if (updatedTicketResponseDC != null)
                                            {
                                                if (updatedTicketResponseDC.success == true)
                                                {
                                                    if (updatedTicketResponseDC.data.ticketNo != null)
                                                    {
                                                        StatusDataContract structObj = new StatusDataContract(true, "Success", updatedTicketResponseDC);
                                                        response = new HttpResponseMessage(HttpStatusCode.OK)
                                                        {
                                                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                                        };
                                                    }
                                                }
                                                else
                                                {
                                                    StatusDataContract structObj = new StatusDataContract(false, "Error Requesting Bizlog Unable to Create ticket", updatedTicketResponseDC);
                                                    response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                                    {
                                                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                                    };
                                                }
                                            }
                                            else
                                            {
                                                StatusDataContract structObj = new StatusDataContract(false, "Error Requesting Bizlog Unable to Create ticket", updatedTicketResponseDC);
                                                response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                                {
                                                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                                };
                                            }
                                        }
                                        //}
                                        else
                                        {
                                            StatusDataContract structObj = new StatusDataContract(false, "Invalid model request", null);
                                            response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                            {
                                                Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                            };
                                        }

                                    }
                                    else
                                    {
                                        StatusDataContract structObj = new StatusDataContract(false, evcApprovedObj.Evc_Name + ": does not have sufficient running balance i.e.", null);
                                        response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                        {
                                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                        };
                                    }
                                }
                                else
                                {
                                    StatusDataContract structObj = new StatusDataContract(false, "EVC Data Not Foud");
                                    response = new HttpResponseMessage(HttpStatusCode.OK)
                                    {
                                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                    };
                                }
                            }
                            else
                            {
                                StatusDataContract structObj = new StatusDataContract(false, "Evc not allocated to this order");
                                response = new HttpResponseMessage(HttpStatusCode.OK)
                                {
                                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                };
                            }
                        }
                        else
                        {
                            StatusDataContract structObj = new StatusDataContract(false,customerDetails.Message);
                            response = new HttpResponseMessage(HttpStatusCode.OK)
                            {
                                Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                            };
                        }
                    }
                    else
                    {
                        StatusDataContract structObj = new StatusDataContract(false, "Logistics ticket already generated for this order "+ lgcRecordObj.TicketNumber);
                        response = new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                        };
                    }
                }
                else
                {
                    StatusDataContract structObj = new StatusDataContract(false, "Order not found.");
                    response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ZohoExposedController", "CreateTicketWithBizlog", ex);
                StatusDataContract structObj = new StatusDataContract(false, ex.Message);
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json")) //new StringContent("error"),                    
                };
            }

            return response;

        }
        #endregion
        #region  Mahindra Logistics
        [HttpGet]
        public HttpResponseMessage RequestMahindraLGC(string RegdNo, bool GenerateTicketWithoutBalanceCheck, int servicePartnerId)
        {

            TicketSyncManager ticketSyncManager = new TicketSyncManager();
            MahindraLogisticsSyncManager _mahindraSyncManager = new MahindraLogisticsSyncManager();
            SponserManager sponserManager = new SponserManager();
            EVCManager eVCManager = new EVCManager();
            HttpResponseMessage response = null;
            SponserData sponserObj = null;
            sponserObj = new SponserData();
            EvcApprovedData evcApprovedObj = null;
            evcApprovedObj = new EvcApprovedData();
            evcApprovedObj = new EvcApprovedData();
            MahindraLogisticsDataContract mahindraLogisticsDataContract = null;
            MahindraLogisticsResponseDataContract mahindraResponseDC = null;
            _mahindraLogisticsRepository = new MahindraLogisticsRepository();
            _exchangeOrderRepository = new ExchangeOrderRepository();
            _evcApprovedREpository = new EVCApprovedRepository();
            _customerDetailsRepository = new CustomerDetailsRepository();
            orderTransactionRepository = new OrderTransactionRepository();
            _logisticsRepository = new LogisticsRepository();
            _walletRepository = new EVCWalletRepository();
            aBBRegistrationRepository = new ABBRegistrationRepository();
            _evcRegistrationRepository = new EVCRegistrationRepository();
            _abbRedemptionRepository = new AbbRedemptionRepository();
            RunningBalance runingbalancecalculation = new RunningBalance();
            EVCdetailsDataContract evcdetailsDC = new EVCdetailsDataContract();
            CustomerandOrderDetailsDataContract customerDetails = new CustomerandOrderDetailsDataContract();
            decimal TotalofInprogress = 0;
            decimal TotalofDeliverd = 0;
            decimal EvcWallletAmount = 0;
            try
            {
                //point 1 >> get zoho sponsor order form by id (zohoSalesOrderid)
                tblOrderTran transactionObj = orderTransactionRepository.GetSingle(x => !string.IsNullOrEmpty(x.RegdNo) && x.RegdNo.ToLower() == RegdNo.ToLower() && x.IsActive == true);
                //point 2 >> Fill the TicketDataContract object using the response from point 1 
                if (transactionObj != null)
                {

                    tblLogistic lgcRecordObj = _logisticsRepository.GetSingle(x => x.RegdNo.ToLower() == RegdNo.ToLower() && x.IsActive == true);
                    //Ticket should not be generated already
                    if (lgcRecordObj==null)
                    {

                        if (transactionObj.OrderType == Convert.ToInt32(OrderTypeEnum.Exchange))
                        {
                            tblExchangeOrder exchangeOrder = _exchangeOrderRepository.GetSingle(x => x.Id == transactionObj.ExchangeId);
                            if (exchangeOrder != null)
                            {
                                customerDetails = ticketSyncManager.SetOrderDetailsObject(exchangeOrder);
                            }
                        }
                        else
                        {
                            tblABBRedemption abbredemptionObj = _abbRedemptionRepository.GetSingle(x => x.RedemptionId == transactionObj.ABBRedemptionId);
                            if (abbredemptionObj != null)
                            {
                                customerDetails = ticketSyncManager.SetOrderDetailsObjectForABB(abbredemptionObj);
                            }
                        }
                        if (customerDetails.Message == null || customerDetails.Message == "")
                        {
                            // get EVC Call allocation report by sponserID for Drop customer details
                            tblWalletTransaction EVCassigned = _walletRepository.GetSingle(x => x.OrderTransId == transactionObj.OrderTransId);
                            if (EVCassigned != null)
                            {
                                //Get evc data from tblevcRegistration
                                tblEVCRegistration evcRegistrartion = _evcRegistrationRepository.GetSingle(x => x.EVCRegistrationId == EVCassigned.EVCRegistrationId && x.ISEVCApprovrd == true);

                                if (evcRegistrartion != null)
                                {
                                    evcdetailsDC = ticketSyncManager.SetEvcDetailsDataContract(evcRegistrartion);
                                    List<tblWalletTransaction> walletsummary = _walletRepository.GetList(x => x.EVCRegistrationId == EVCassigned.EVCRegistrationId).ToList();

                                    ///
                                    foreach (var items in walletsummary)
                                    {
                                        if (items.OrderOfInprogressDate != null && items.OrderOfDeliverdDate == null && items.OrderOfCompleteDate == null)
                                        {
                                            if (items.OrderAmount != null)
                                            {
                                                TotalofInprogress = TotalofInprogress + Convert.ToDecimal(items.OrderAmount);

                                            }
                                            // tblWalletTransaction.OrderAmount += items.OrderAmount;
                                        }
                                        if (items.OrderOfInprogressDate != null && items.OrderOfDeliverdDate != null && items.OrderOfCompleteDate == null)
                                        {
                                            if (items.OrderAmount != null)
                                            {
                                                TotalofDeliverd = TotalofDeliverd + Convert.ToDecimal(items.OrderAmount);
                                            }
                                        }
                                    }
                                    runingbalancecalculation.TotalofInprogress = TotalofInprogress;
                                    runingbalancecalculation.TotalofDeliverd = TotalofDeliverd;
                                    EvcWallletAmount = Convert.ToDecimal(evcRegistrartion.EVCWalletAmount);
                                    runingbalancecalculation.runningBalance = EvcWallletAmount - (runingbalancecalculation.TotalofInprogress + runingbalancecalculation.TotalofDeliverd);
                                    ///
                                    bool isBlanaceTrue = true;
                                    if (!GenerateTicketWithoutBalanceCheck)
                                    {
                                        if (Convert.ToInt32(runingbalancecalculation.runningBalance) > 0)
                                            isBlanaceTrue = true;
                                        else
                                            isBlanaceTrue = false;
                                    }

                                    if (evcRegistrartion != null && isBlanaceTrue)
                                    {
                                        // set value in bizlog ticket object
                                        mahindraLogisticsDataContract = _mahindraSyncManager.SetMahindraObj(customerDetails, evcdetailsDC);

                                        if (mahindraLogisticsDataContract != null)
                                        {
                                            //point 3 >> Create the tciket in bizlog and local DB (already happening)              
                                            mahindraResponseDC = _mahindraSyncManager.ProcessLogisticsRequest(mahindraLogisticsDataContract);
                                            //point To Add Ticket data into logistics table and update status in wallet as well as status history table and exchange order table
                                            int ticketid = ticketSyncManager.AddTicketToTblLogistics(mahindraResponseDC.awbNumber.ToString(), transactionObj.OrderTransId, servicePartnerId, transactionObj.RegdNo,customerDetails);


                                            //point 4 >> update the zoho sponsor order form by ticket id received from bizlog response
                                            if (mahindraResponseDC != null)
                                            {
                                                if (mahindraResponseDC.status/*.Equals(true)*/== null)
                                                {
                                                    if (mahindraResponseDC.awbNumber >0)
                                                    {

                                                        StatusDataContract structObj = new StatusDataContract(true, "Success", mahindraResponseDC);
                                                        response = new HttpResponseMessage(HttpStatusCode.OK)
                                                        {
                                                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                                        };
                                                    }
                                                }

                                                else
                                                {
                                                    StatusDataContract structObj = new StatusDataContract(false, "Invalid model request", mahindraResponseDC);
                                                    response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                                    {
                                                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                                    };
                                                }
                                            }
                                        }
                                        else
                                        {
                                            StatusDataContract structObj = new StatusDataContract(false, "please check the data once something is invalid", mahindraResponseDC);
                                            response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                            {
                                                Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                            };
                                        }
                                    }
                                    else
                                    {
                                        StatusDataContract structObj = new StatusDataContract(false, evcApprovedObj.Evc_Name + ": does not have sufficient running balance i.e.", null);
                                        response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                        {
                                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                        };
                                    }

                                }
                                else
                                {
                                    StatusDataContract structObj = new StatusDataContract(false, "EVC Data Not Foud please check if evc is not approved");
                                    response = new HttpResponseMessage(HttpStatusCode.OK)
                                    {
                                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                    };
                                }
                            }
                            else
                            {
                                StatusDataContract structObj = new StatusDataContract(false, "EVC is not alocated");
                                response = new HttpResponseMessage(HttpStatusCode.OK)
                                {
                                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                };
                            }
                        }
                        else
                        {
                            StatusDataContract structObj = new StatusDataContract(false, customerDetails.Message);
                            response = new HttpResponseMessage(HttpStatusCode.OK)
                            {
                                Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                            };
                        }
                       
                    }
                    else
                    {
                        StatusDataContract structObj = new StatusDataContract(false, "Ticket is alredy generated for this order");
                        response = new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                        };
                    }
                }
                else
                {
                    StatusDataContract structObj = new StatusDataContract(false, "order not found");
                    response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                }

            }

            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ZohoExposedController", "RequestMahindraLGC", ex);
                StatusDataContract structObj = new StatusDataContract(false, ex.Message);
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json")) //new StringContent("error"),                    
                };
            }

            return response;

        }
        #endregion
        #region Update Ticket Status methods
        [HttpPost]
        public HttpResponseMessage UpdateStatusMahindraLogistics(TicketStatusDataContract ticketStatusDataContract)
        {
            TicketSyncManager ticketSyncManager = new TicketSyncManager();
            SponserManager sponserManager = new SponserManager();
            HttpResponseMessage response = null;
            SponserListDataContract sponserListDC = null;
            SponserData sponserObj = null;
            sponserObj = new SponserData();
            SponserFormResponseDataContract sponserResponseDC = null;

            try
            {
                int addLog = Convert.ToInt32(ConfigurationManager.AppSettings["EnableStatusUpdateAPI"]);
                //int addLog = 1;
                if (addLog == 1)
                {
                    if (ticketStatusDataContract != null)
                    {
                        if (ticketStatusDataContract.ticketNo != null)
                        {
                            //point 1 >> get zoho sponsor order form by id (zohoSalesOrderid)
                            sponserListDC = sponserManager.GetSponserOrderByBizlogTicketNo(ticketStatusDataContract.ticketNo);

                            if (sponserListDC != null && sponserListDC.data.Count > 0)
                            {
                                sponserObj = sponserListDC.data[0];
                                if (sponserObj != null)
                                {
                                    #region Code to store the Request body
                                    string jsonstring = JsonConvert.SerializeObject(ticketStatusDataContract);
                                    logging.WriteAPIRequestToDB("ZohoExposedController", "UpdateStatusMahindraLogistics", sponserObj.Sp_Order_No, jsonstring);
                                    #endregion
                                    //point 2 >> Cancel the tciket in bizlog and local DB (already happening) 
                                    string ticketNo = ticketSyncManager.ProcessawbNoMahindraLogistics(ticketStatusDataContract);

                                    if (ticketNo != null && sponserObj.ID != null)
                                    {
                                        LogisticStatusDataContract sponserUpdateObj = sponserManager.SetUpdateLogisticStatusObject(ticketStatusDataContract, sponserObj.ID);
                                        if (sponserUpdateObj != null)
                                            sponserResponseDC = sponserManager.UpdateLGCStatusDetail(sponserUpdateObj);

                                        if (sponserResponseDC != null)
                                        {
                                            if (sponserResponseDC.data != null && sponserResponseDC.data.ID != null)
                                            {
                                                try
                                                {
                                                    ExchangeOrderManager exchangeOrderManager = new ExchangeOrderManager();
                                                    exchangeOrderManager.UpdateOrderStatus(sponserObj.ID);
                                                }
                                                catch (Exception ex1)
                                                {
                                                    LibLogging.WriteErrorToDB("ZohoExposed", "UpdateStatusMahindraLogistics", ex1);
                                                }
                                                StatusDataContract structObj = new StatusDataContract(true, "Success", sponserResponseDC);
                                                response = new HttpResponseMessage(HttpStatusCode.OK)
                                                {
                                                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                                };
                                            }
                                            else
                                            {
                                                StatusDataContract structObj = new StatusDataContract(false, sponserResponseDC.message);
                                                response = new HttpResponseMessage(HttpStatusCode.OK)
                                                {
                                                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                                };

                                            }
                                        }

                                    }
                                    else
                                    {
                                        StatusDataContract structObj = new StatusDataContract(false, "Ticket Not Updated");
                                        response = new HttpResponseMessage(HttpStatusCode.OK)
                                        {
                                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                        };

                                    }

                                }
                            }
                            else
                            {
                                StatusDataContract structObj = new StatusDataContract(false, "Ticket No. not found in Sponsor");
                                response = new HttpResponseMessage(HttpStatusCode.OK)
                                {
                                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                };

                            }

                        }
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
        #region
        [HttpGet]
        public HttpResponseMessage GenerateTicketForLocalLgcPartner(string RegdNo, bool GenerateTicketWithoutBalanceCheck, int servicePartnerId)
        {
            TicketSyncManager ticketSyncManager = new TicketSyncManager();
            HttpResponseMessage response = null;
            _exchangeOrderRepository = new ExchangeOrderRepository();
            _evcApprovedREpository = new EVCApprovedRepository();
            _customerDetailsRepository = new CustomerDetailsRepository();
            orderTransactionRepository = new OrderTransactionRepository();
            _logisticsRepository = new LogisticsRepository();
            _walletRepository = new EVCWalletRepository();
            aBBRegistrationRepository = new ABBRegistrationRepository();
            _evcRegistrationRepository = new EVCRegistrationRepository();
            _abbRedemptionRepository = new AbbRedemptionRepository();
            RunningBalance runingbalancecalculation = new RunningBalance();
            EVCdetailsDataContract evcdetailsDC = new EVCdetailsDataContract();
            CustomerandOrderDetailsDataContract customerDetails = new CustomerandOrderDetailsDataContract();
            decimal TotalofInprogress = 0;
            decimal TotalofDeliverd = 0;
            decimal EvcWallletAmount = 0;
            try
            {
                //point 1 >> get zoho sponsor order form by id (zohoSalesOrderid)
                tblOrderTran transactionObj = orderTransactionRepository.GetSingle(x => !string.IsNullOrEmpty(x.RegdNo) &&x.RegdNo.ToLower() == RegdNo.ToLower() && x.IsActive == true);
                //point 2 >> Fill the TicketDataContract object using the response from point 1 
                if (transactionObj != null)
                {

                    tblLogistic lgcRecordObj = _logisticsRepository.GetSingle(x => x.RegdNo.ToLower() == RegdNo.ToLower() && x.IsActive == true);
                    //Ticket should not be generated already
                    if (lgcRecordObj==null)
                    {

                        if (transactionObj.OrderType == Convert.ToInt32(OrderTypeEnum.Exchange))
                        {
                            tblExchangeOrder exchangeOrder = _exchangeOrderRepository.GetSingle(x => x.Id == transactionObj.ExchangeId && x.IsActive==true);
                            if (exchangeOrder != null)
                            {
                                customerDetails = ticketSyncManager.SetOrderDetailsObject(exchangeOrder);
                            }
                        }
                        else
                        {
                            tblABBRedemption abbredemptionObj = _abbRedemptionRepository.GetSingle(x => x.RedemptionId == transactionObj.ABBRedemptionId && x.IsActive==true);
                            if (abbredemptionObj != null)
                            {
                                customerDetails = ticketSyncManager.SetOrderDetailsObjectForABB(abbredemptionObj);
                            }
                        }
                        if(customerDetails.Message==null || customerDetails.Message == "")
                        {
                            // get EVC Call allocation report by sponserID for Drop customer details
                            tblWalletTransaction EVCassigned = _walletRepository.GetSingle(x => x.OrderTransId == transactionObj.OrderTransId);
                            if (EVCassigned != null)
                            {
                                //Get evc data from tblevcRegistration
                                tblEVCRegistration evcRegistrartion = _evcRegistrationRepository.GetSingle(x => x.EVCRegistrationId == EVCassigned.EVCRegistrationId && x.ISEVCApprovrd == true);

                                if (evcRegistrartion != null)
                                {
                                    evcdetailsDC = ticketSyncManager.SetEvcDetailsDataContract(evcRegistrartion);
                                    List<tblWalletTransaction> walletsummary = _walletRepository.GetList(x => x.EVCRegistrationId == EVCassigned.EVCRegistrationId).ToList();

                                    ///
                                    foreach (var items in walletsummary)
                                    {
                                        if (items.OrderOfInprogressDate != null && items.OrderOfDeliverdDate == null && items.OrderOfCompleteDate == null)
                                        {
                                            if (items.OrderAmount != null)
                                            {
                                                TotalofInprogress = TotalofInprogress + Convert.ToDecimal(items.OrderAmount);

                                            }
                                            // tblWalletTransaction.OrderAmount += items.OrderAmount;
                                        }
                                        if (items.OrderOfInprogressDate != null && items.OrderOfDeliverdDate != null && items.OrderOfCompleteDate == null)
                                        {
                                            if (items.OrderAmount != null)
                                            {
                                                TotalofDeliverd = TotalofDeliverd + Convert.ToDecimal(items.OrderAmount);
                                            }
                                        }
                                    }
                                    runingbalancecalculation.TotalofInprogress = TotalofInprogress;
                                    runingbalancecalculation.TotalofDeliverd = TotalofDeliverd;
                                    EvcWallletAmount = Convert.ToDecimal(evcRegistrartion.EVCWalletAmount);
                                    runingbalancecalculation.runningBalance = EvcWallletAmount - (runingbalancecalculation.TotalofInprogress + runingbalancecalculation.TotalofDeliverd);
                                    ///
                                    bool isBlanaceTrue = true;
                                    if (!GenerateTicketWithoutBalanceCheck)
                                    {
                                        if (Convert.ToInt32(runingbalancecalculation.runningBalance) > 0)
                                            isBlanaceTrue = true;
                                        else
                                            isBlanaceTrue = false;
                                    }

                                    if (evcRegistrartion != null && isBlanaceTrue)
                                    {
                                        //Raise ticket for  local logistics partner
                                        string LgcTicket = UniqueString.RandomNumberByLength(10);
                                        //point To Add Ticket data into logistics table and update status in wallet as well as status history table and exchange order table
                                        int ticketid = ticketSyncManager.AddTicketToTblLogistics(LgcTicket, transactionObj.OrderTransId, servicePartnerId, transactionObj.RegdNo, customerDetails);

                                        if (LgcTicket != null && ticketid > 0)
                                        {
                                            StatusDataContract structObj = new StatusDataContract(true, "Success", LgcTicket);
                                            response = new HttpResponseMessage(HttpStatusCode.OK)
                                            {
                                                Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                            };
                                        }

                                    }
                                    else
                                    {
                                        StatusDataContract structObj = new StatusDataContract(false, evcRegistrartion.BussinessName + ": does not have sufficient running balance i.e.", null);
                                        response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                        {
                                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                        };
                                    }
                                }
                                else
                                {
                                    StatusDataContract structObj = new StatusDataContract(false, "EVC Data Not Foud");
                                    response = new HttpResponseMessage(HttpStatusCode.OK)
                                    {
                                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                    };
                                }
                            }
                            else
                            {
                                StatusDataContract structObj = new StatusDataContract(false, "EVC is not alocated");
                                response = new HttpResponseMessage(HttpStatusCode.OK)
                                {
                                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                };
                            }
                        }
                        else
                        {
                            StatusDataContract structObj = new StatusDataContract(false, customerDetails.Message);
                            response = new HttpResponseMessage(HttpStatusCode.OK)
                            {
                                Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                            };
                        }
                      
                    }
                    else
                    {
                        StatusDataContract structObj = new StatusDataContract(false, "Ticket is alredy generated for this order");
                        response = new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                        };
                    }
                }
                else
                {
                    StatusDataContract structObj = new StatusDataContract(false, "order not found");
                    response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                }

            }

            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ZohoExposedController", "RequestMahindraLGC", ex);
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

    public class ValidationResponse
    {
        public List<ValidationResult> Results { get; set; }
        public bool IsValid { get; set; }

        public ValidationResponse()
        {
            Results = new List<ValidationResult>();
            IsValid = false;
        }
    }

}