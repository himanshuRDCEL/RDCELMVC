using GraspCorn.Common.Constant;
using GraspCorn.Common.Enums;
using GraspCorn.Common.Helper;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RDCEL.DocUpload.BAL.ABBRegistration;
using RDCEL.DocUpload.BAL.Common;
using RDCEL.DocUpload.BAL.Manager;
using RDCEL.DocUpload.BAL.ServiceCall;
using RDCEL.DocUpload.BAL.SponsorsApiCall;
using RDCEL.DocUpload.BAL.ZohoCreatorCall;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DAL.Helper;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DataContract.ABBRedemption;
using RDCEL.DocUpload.DataContract.BillCloud;
using RDCEL.DocUpload.DataContract.Bizlog;
using RDCEL.DocUpload.DataContract.Common;
using RDCEL.DocUpload.DataContract.ExchangeOrderDetails;
using RDCEL.DocUpload.DataContract.MasterModel;
using RDCEL.DocUpload.DataContract.SponsorModel;
using RDCEL.DocUpload.DataContract.WhatsappTemplates;
using RDCEL.DocUpload.DataContract.ZohoModel;
using Data = RDCEL.DocUpload.DataContract.ZohoModel.Data;
using static RDCEL.DocUpload.BAL.Common.WhatsappNotificationManager;

namespace RDCEL.DocUpload.BAL.ProcessAPI
{
    public class SponsrOrderSyncManager
    {
        #region Variable Declaration

        DateTime currentDatetime = DateTime.Now.TrimMilliseconds();
        BusinessUnitRepository _businessUnitRepository;
        ExchangeOrderRepository _exchangeOrderRepository;
        NotificationManager _notificationManager;
        ProductTypeRepository _productTypeRepository;
        BrandRepository _brandRepository;
        ProductCategoryRepository _productCategoryRepository;
        WhatsappMessageRepository _whatsAppMessageRepository;
        ExchangeOrderStatusRepository _exchangeOrderStatusRepository1;
        OrderTransactionRepository _OrderTransactionRepository;
        OrderLGC_Repository _OrderLGC_Repository;
        OrderQC_Repository _orderQC_Repository;
        Logging logging;
        VoucherStatusRepository _voucherStatusRepository;
        ABBRegistrationRepository _abbRegistrationRepository;
        AbbRedemptionRepository _abbRedemptionRepository;

        #endregion

        #region sync order details
        /// <summary>
        /// 
        /// </summary>       
        /// <returns></returns>   
        public ProductOrderResponseDataContract ProcessOrderInfo(ProductOrderDataContract productOrderDataContract, tblBusinessUnit businessUnit)
        {
            int orderId = 0;
            CustomerManager customerInfo = new CustomerManager();
            ProductManager productOrderInfo = new ProductManager();
            SponserManager sponserManager = new SponserManager();
            _businessUnitRepository = new BusinessUnitRepository();
            _exchangeOrderRepository = new ExchangeOrderRepository();
            _notificationManager = new NotificationManager();
            _productCategoryRepository = new ProductCategoryRepository();
            _productTypeRepository = new ProductTypeRepository();
            _brandRepository = new BrandRepository();
            _voucherStatusRepository = new VoucherStatusRepository();
            WhatasappResponse whatssappresponseDC = null;
            string responseforWhatasapp = string.Empty;
            _whatsAppMessageRepository = new WhatsappMessageRepository();
            ProductOrderResponseDataContract productOrderResponseDC = null;
            productOrderResponseDC = new ProductOrderResponseDataContract();
            string message = null;
            string ZohoPushFlag = string.Empty;
            logging = new Logging();
            string BrandName = null;
            string ProductType = null;
            string ProductCategory = null;
            string SelfQCLink = null;
            string WhatssAppStatusEnum = string.Empty;
            string SendSMSFlag = null;
            string ResponseCode = string.Empty;
            ABBOrderMaanger orderManager = new ABBOrderMaanger();
            ABBRedemptionDataContract abbredemptiondataobj = new ABBRedemptionDataContract();
            ABBOrderManager ordermanagerObj = new ABBOrderManager();
            tblABBRedemption redemptionObj = null;

            try
            {
                SendSMSFlag = ConfigurationManager.AppSettings["sendsmsflag"].ToString();
                if (productOrderDataContract != null)
                {
                    if (productOrderDataContract.OrderType == Convert.ToInt32(GraspCorn.Common.Enums.OrderTypeEnum.ABB))
                    {
                        #region create redemption request
                        abbredemptiondataobj = ordermanagerObj.GetRedemptionData(productOrderDataContract.RegdNo);
                        redemptionObj = AddRedemptioRequest(abbredemptiondataobj, productOrderDataContract, businessUnit);

                        #region code to manage order trans and abbexchange History
                        OrderTransactionManager orderTransactionManager = new OrderTransactionManager();
                        ExchangeABBStatusHistoryManager exchangeABBStatusHistoryManager = new ExchangeABBStatusHistoryManager();
                        tblExchangeOrder exchangeOrder = _exchangeOrderRepository.GetSingle(x => x.Id.Equals(orderId));
                        if (productOrderResponseDC != null && redemptionObj != null)
                        {
                            //Code for Order tran
                            OrderTransactionDataContract orderTransactionDC = new OrderTransactionDataContract();
                            orderTransactionDC.OrderType = Convert.ToInt32(GraspCorn.Common.Enums.OrderTypeEnum.ABB);
                            orderTransactionDC.ABBRedemptionId = redemptionObj.RedemptionId;
                            //orderTransactionDC.SponsorOrderNumber = redemptionObj.;
                            orderTransactionDC.RegdNo = redemptionObj.RegdNo;
                            orderTransactionDC.ExchangePrice = redemptionObj.RedemptionValue;
                            orderTransactionDC.Sweetner = 0;
                            int tranid = orderTransactionManager.MangeOrderTransaction(orderTransactionDC);
                            //Code for Order history
                            if (tranid > 0)
                            {
                                ExchangeABBStatusHistoryDataContract exchangeABBStatusHistoryDC = new ExchangeABBStatusHistoryDataContract();
                                exchangeABBStatusHistoryDC.OrderType = Convert.ToInt32(GraspCorn.Common.Enums.OrderTypeEnum.ABB);
                                exchangeABBStatusHistoryDC.OrderTransId = tranid;
                                exchangeABBStatusHistoryDC.ABBRedemptionId = redemptionObj.RedemptionId;
                                //exchangeABBStatusHistoryDC.SponsorOrderNumber = exchangeOrder.SponsorOrderNumber;
                                exchangeABBStatusHistoryDC.RegdNo = redemptionObj.RegdNo;
                                exchangeABBStatusHistoryDC.CustId = Convert.ToInt32(redemptionObj.CustomerDetailsId);
                                exchangeABBStatusHistoryDC.StatusId = 5;
                                exchangeABBStatusHistoryManager.MangeOrderHisotry(exchangeABBStatusHistoryDC);
                            }
                            productOrderResponseDC.RegdNo = redemptionObj.RegdNo;
                            productOrderResponseDC.OrderId = redemptionObj.RedemptionId;
                            productOrderResponseDC.BusinessPartnerId = productOrderDataContract.BusinessPartnerId;
                            productOrderResponseDC.BusinessUnitId = businessUnit.BusinessUnitId;
                        }
                        #endregion
                        #endregion
                        #region send self QC link on redemption
                        string ERPBaseURL = ConfigurationManager.AppSettings["ERPBaseURL"].ToString();
                        string selfQC = ConfigurationManager.AppSettings["SelfQCUrl"].ToString();
                        string selfqcurl = ERPBaseURL + "" + selfQC + "" + productOrderDataContract.RegdNo;
                        ABBRedemptionSelfQCLink whatsappObj = new ABBRedemptionSelfQCLink();
                        whatsappObj.userDetails = new UserDetails();
                        whatsappObj.notification = new ABBRedemptionSelfQCLinkNotification();
                        whatsappObj.notification.@params = new ABBRedemptionParameters();
                        whatsappObj.userDetails.number = abbredemptiondataobj.PhoneNumber;
                        whatsappObj.notification.sender = ConfigurationManager.AppSettings["Yello.aiSenderNumber"].ToString();
                        whatsappObj.notification.type = ConfigurationManager.AppSettings["Yellow.aiMesssaheType"].ToString();
                        whatsappObj.notification.templateId = NotificationConstants.ABBRedemptionSelfQC;
                        whatsappObj.notification.@params.Link = selfqcurl;
                        whatsappObj.notification.@params.CustomerName = abbredemptiondataobj.FirstName + " " + abbredemptiondataobj.LastName;
                        string url = ConfigurationManager.AppSettings["Yellow.AiUrl"].ToString();
                        IRestResponse response = WhatsappNotificationManager.Rest_InvokeWhatsappserviceCall(url, Method.POST, whatsappObj);
                        ResponseCode = response.StatusCode.ToString();
                        WhatssAppStatusEnum = ExchangeOrderManager.GetEnumDescription(WhatssAppEnum.SuccessCode);
                        if (ResponseCode == WhatssAppStatusEnum)
                        {
                            responseforWhatasapp = response.Content;
                            if (responseforWhatasapp != null)
                            {
                                whatssappresponseDC = JsonConvert.DeserializeObject<WhatasappResponse>(responseforWhatasapp);
                                tblWhatsAppMessage whatsapObj = new tblWhatsAppMessage();
                                whatsapObj.TemplateName = NotificationConstants.ABBRedemptionSelfQC;
                                whatsapObj.IsActive = true;
                                whatsapObj.PhoneNumber = abbredemptiondataobj.PhoneNumber;
                                whatsapObj.SendDate = DateTime.Now;
                                whatsapObj.msgId = whatssappresponseDC.msgId;
                                _whatsAppMessageRepository.Add(whatsapObj);
                                _whatsAppMessageRepository.SaveChanges();
                            }
                            else
                            {
                                string ExchOrderObj = JsonConvert.SerializeObject(productOrderDataContract);
                                logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", productOrderDataContract.SponsorOrderNumber, ExchOrderObj);
                            }
                        }
                        else
                        {
                            string ExchOrderObj = JsonConvert.SerializeObject(productOrderDataContract);
                            logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", productOrderDataContract.SponsorOrderNumber, ExchOrderObj);
                        }
                        #endregion
                    }
                    else
                    {
                        #region Code to add Customer details in database
                        int custId = customerInfo.AddCustomer(productOrderDataContract);
                        #endregion

                        #region Code to add product order in database
                        if (custId != 0)
                        {
                            productOrderDataContract.CustomerDetailsId = custId;
                            orderId = productOrderInfo.AddOrder(productOrderDataContract, businessUnit);
                            productOrderResponseDC.OrderId = orderId;
                            productOrderResponseDC.RegdNo = productOrderDataContract.RegdNo;
                        }
                        #endregion

                        ProductTypeRepository productTypeRepository = new ProductTypeRepository();
                        tblProductType tblProductType = productTypeRepository.GetSingle(x => x.Id == productOrderDataContract.ProductTypeId);
                        if (tblProductType != null)
                        {
                            try
                            {
                                if (productOrderDataContract.BUId == Convert.ToInt32(BusinessUnitEnum.PineLabs))
                                {
                                    #region Code to send notification to customer
                                    tblBusinessUnit Obj = _businessUnitRepository.GetSingle(x => x.BusinessUnitId == productOrderDataContract.BUId);
                                    tblExchangeOrder ExchangeObj = _exchangeOrderRepository.GetSingle(x => !string.IsNullOrEmpty(x.RegdNo) && x.RegdNo == productOrderDataContract.RegdNo);
                                    string baseUrl = ConfigurationManager.AppSettings["BaseUrl"].ToString();
                                    string PineLabsPage = ConfigurationManager.AppSettings["PineLabsLink"].ToString();
                                    string CustomerDetailsLink = baseUrl + "" + PineLabsPage + "" + ExchangeObj.RegdNo;

                                    message = NotificationConstants.SMS_PineLabs_CustomerDetails.Replace("[REGNO]", ExchangeObj.RegdNo).Replace("[PLink]", CustomerDetailsLink);
                                    _notificationManager.SendNotificationSMS(productOrderDataContract.PhoneNumber, message, null);
                                    #endregion

                                    #region TO send WhatsappNotificatio for CustomerDetails link Settelment
                                    PersonalDdetailsLinkWhatsappTemplate whatsappObj = new PersonalDdetailsLinkWhatsappTemplate();
                                    whatsappObj.userDetails = new UserDetails();
                                    whatsappObj.notification = new PersonalDetailsNOtification();
                                    whatsappObj.notification.@params = new PersonalDetailsLinkOnWhatsapp();
                                    whatsappObj.userDetails.number = productOrderDataContract.PhoneNumber;
                                    whatsappObj.notification.sender = ConfigurationManager.AppSettings["Yello.aiSenderNumber"].ToString();
                                    whatsappObj.notification.type = ConfigurationManager.AppSettings["Yellow.aiMesssaheType"].ToString();
                                    whatsappObj.notification.templateId = NotificationConstants.send_link_for_personal_details;
                                    //whatsappObj.notification.@params.RegdNo = productOrderDataContract.RegdNo.ToString();
                                    whatsappObj.notification.@params.Link = CustomerDetailsLink;
                                    string url = ConfigurationManager.AppSettings["Yellow.AiUrl"].ToString();
                                    IRestResponse response = WhatsappNotificationManager.Rest_InvokeWhatsappserviceCall(url, Method.POST, whatsappObj);
                                    ResponseCode = response.StatusCode.ToString();
                                    WhatssAppStatusEnum = ExchangeOrderManager.GetEnumDescription(WhatssAppEnum.SuccessCode);
                                    if (ResponseCode == WhatssAppStatusEnum)
                                    {
                                        responseforWhatasapp = response.Content;
                                        if (responseforWhatasapp != null)
                                        {
                                            whatssappresponseDC = JsonConvert.DeserializeObject<WhatasappResponse>(responseforWhatasapp);
                                            tblWhatsAppMessage whatsapObj = new tblWhatsAppMessage();
                                            whatsapObj.TemplateName = NotificationConstants.send_link_for_personal_details;
                                            whatsapObj.IsActive = true;
                                            whatsapObj.PhoneNumber = productOrderDataContract.PhoneNumber;
                                            whatsapObj.SendDate = DateTime.Now;
                                            whatsapObj.msgId = whatssappresponseDC.msgId;
                                            _whatsAppMessageRepository.Add(whatsapObj);
                                            _whatsAppMessageRepository.SaveChanges();
                                        }
                                        else
                                        {
                                            string ExchOrderObj = JsonConvert.SerializeObject(productOrderDataContract);
                                            logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", productOrderDataContract.SponsorOrderNumber, ExchOrderObj);
                                        }
                                    }
                                    else
                                    {
                                        string ExchOrderObj = JsonConvert.SerializeObject(productOrderDataContract);
                                        logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", productOrderDataContract.SponsorOrderNumber, ExchOrderObj);
                                    }
                                    #endregion
                                }

                                #region Code to add order in transaction and history
                                OrderTransactionManager orderTransactionManager = new OrderTransactionManager();
                                ExchangeABBStatusHistoryManager exchangeABBStatusHistoryManager = new ExchangeABBStatusHistoryManager();
                                tblExchangeOrder exchangeOrder = _exchangeOrderRepository.GetSingle(x => x.Id.Equals(orderId));
                                if (productOrderResponseDC != null && exchangeOrder != null)
                                {
                                    //Code for Order tran
                                    OrderTransactionDataContract orderTransactionDC = new OrderTransactionDataContract();
                                    orderTransactionDC.OrderType = 17;
                                    orderTransactionDC.ExchangeId = exchangeOrder.Id;
                                    orderTransactionDC.SponsorOrderNumber = exchangeOrder.SponsorOrderNumber;
                                    orderTransactionDC.RegdNo = exchangeOrder.RegdNo;
                                    orderTransactionDC.ExchangePrice = exchangeOrder.ExchangePrice;
                                    orderTransactionDC.Sweetner = exchangeOrder.Sweetener;
                                    int tranid = orderTransactionManager.MangeOrderTransaction(orderTransactionDC);
                                    //Code for Order history
                                    if (tranid > 0)
                                    {
                                        ExchangeABBStatusHistoryDataContract exchangeABBStatusHistoryDC = new ExchangeABBStatusHistoryDataContract();
                                        exchangeABBStatusHistoryDC.OrderType = 17;
                                        exchangeABBStatusHistoryDC.OrderTransId = tranid;
                                        exchangeABBStatusHistoryDC.ExchangeId = exchangeOrder.Id;
                                        exchangeABBStatusHistoryDC.SponsorOrderNumber = exchangeOrder.SponsorOrderNumber;
                                        exchangeABBStatusHistoryDC.RegdNo = exchangeOrder.RegdNo;
                                        exchangeABBStatusHistoryDC.CustId = Convert.ToInt32(exchangeOrder.CustomerDetailsId);
                                        exchangeABBStatusHistoryDC.StatusId = 5;
                                        exchangeABBStatusHistoryManager.MangeOrderHisotry(exchangeABBStatusHistoryDC);
                                    }

                                }
                                #endregion

                                if (orderId > 0 && !string.IsNullOrEmpty(productOrderDataContract.FirstName) && !string.IsNullOrEmpty(productOrderDataContract.Email) && businessUnit.BusinessUnitId != Convert.ToInt32(BusinessUnitEnum.D2C) && businessUnit.BusinessUnitId != Convert.ToInt32(BusinessUnitEnum.PineLabs) && productOrderDataContract.IsDefferedSettlement==true)
                                {
                                    #region Code to send Mail To Customer for exchange 
                                    string ERPBaseURL = ConfigurationManager.AppSettings["ERPBaseURL"].ToString();
                                    string selfQC = ConfigurationManager.AppSettings["SelfQCUrl"].ToString();
                                    string selfqcurl = ERPBaseURL + "" + selfQC + "" + productOrderDataContract.RegdNo;
                                    SelfQCLink = selfqcurl;
                                    tblProductType productType = _productTypeRepository.GetSingle(x => x.Id == productOrderDataContract.ProductTypeId);
                                    if (productType != null)
                                    {
                                        tblProductCategory productCategory = _productCategoryRepository.GetSingle(x => x.Id == productType.ProductCatId);
                                        if (productCategory != null)
                                        {
                                            tblBrand brandobj = _brandRepository.GetSingle(x => x.Id == productOrderDataContract.BrandId);
                                            if (brandobj != null)
                                            {
                                                BrandName = brandobj.Name;
                                                ProductCategory = productCategory.Description;
                                                ProductType = productType.Description;
                                                #region Code to send mail to customer for exchange details

                                                MailJetViewModel mailJet = new MailJetViewModel();
                                                MailJetMessage jetmessage = new MailJetMessage();
                                                MailJetFrom from = new MailJetFrom();
                                                MailjetTo to = new MailjetTo();
                                                jetmessage.From = new MailJetFrom() { Email = "hp@rdcel.com", Name = "Rocking Deals - Customer  Care" };
                                                jetmessage.To = new List<MailjetTo>();
                                                jetmessage.To.Add(new MailjetTo() { Email = productOrderDataContract.Email.Trim(), Name = productOrderDataContract.FirstName });
                                                jetmessage.Subject = businessUnit.Name + ": Exchange Detail";
                                                string TemplaTePath = ConfigurationManager.AppSettings["DefferedEmail"].ToString();
                                                string FilePath = TemplaTePath;
                                                StreamReader str = new StreamReader(FilePath);
                                                string MailText = str.ReadToEnd();
                                                str.Close();
                                                MailText = MailText.Replace("[CustomerName]", productOrderDataContract.FirstName).Replace("[BusinessUnitName]", businessUnit.Name).Replace("[SponserOrderNumber]", productOrderDataContract.SponsorOrderNumber).Replace("[CreatedDate]", Convert.ToDateTime(currentDatetime).ToString("dd/MM/yyyy")).Replace("[CustName]", productOrderDataContract.FirstName).Replace("[CustMobile]", productOrderDataContract.PhoneNumber).Replace("[CustAdd1]", productOrderDataContract.Address1)
                                                    .Replace("[CustAdd2]", productOrderDataContract.Address2).Replace("[State]", productOrderDataContract.State).Replace("[PinCode]", productOrderDataContract.ZipCode).Replace("[CustCity]", productOrderDataContract.City).Replace("[ProductCategory]", productCategory.Description)
                                                    .Replace("[OldProdType]", productType.Description).Replace("[OldBrand]", brandobj.Name).Replace("[Size]", productType.Size).Replace("[ExchangePrice]", exchangeOrder.ExchangePrice.ToString()).Replace("[EstimatedDeliveryDate]", productOrderDataContract.EstimatedDeliveryDate).Replace("[SelfQCLink]", selfqcurl);
                                                jetmessage.HTMLPart = MailText;
                                                mailJet.Messages = new List<MailJetMessage>();
                                                mailJet.Messages.Add(jetmessage);
                                                BillCloudServiceCall.MailJetSendMailService(mailJet);
                                                #endregion
                                            }
                                        }
                                    }
                                    #endregion
                                }
                                else if (businessUnit.BusinessUnitId == Convert.ToInt32(BusinessUnitEnum.D2C))
                                {
                                    string ERPBaseURL = ConfigurationManager.AppSettings["ERPBaseURL"].ToString();
                                    string selfQC = ConfigurationManager.AppSettings["SelfQCUrl"].ToString();
                                    string selfqcurl = ERPBaseURL + "" + selfQC + "" + productOrderDataContract.RegdNo;
                                    SelfQCLink = selfqcurl;
                                    #region Code to send Mail To Customer for exchange 
                                    tblProductType productType = _productTypeRepository.GetSingle(x => x.Id == productOrderDataContract.ProductTypeId);
                                    if (productType != null)
                                    {
                                        tblProductCategory productCategory = _productCategoryRepository.GetSingle(x => x.Id == productType.ProductCatId);
                                        if (productCategory != null)
                                        {
                                            tblBrand brandobj = _brandRepository.GetSingle(x => x.Id == productOrderDataContract.BrandId);
                                            if (brandobj != null)
                                            {
                                                BrandName = brandobj.Name;
                                                ProductCategory = productCategory.Description;
                                                ProductType = productType.Description;
                                                #region Code to send mail to customer for exchange details
                                                MailJetViewModel mailJet = new MailJetViewModel();
                                                MailJetMessage jetmessage = new MailJetMessage();
                                                MailJetFrom from = new MailJetFrom();
                                                MailjetTo to = new MailjetTo();
                                                jetmessage.From = new MailJetFrom() { Email = "hp@rdcel.com", Name = "RockingDeals - Customer  Care" };
                                                jetmessage.To = new List<MailjetTo>();
                                                jetmessage.To.Add(new MailjetTo() { Email = productOrderDataContract.Email.Trim(), Name = productOrderDataContract.FirstName });
                                                jetmessage.Subject = businessUnit.Name + ": Exchange Detail";
                                                string TemplaTePath = ConfigurationManager.AppSettings["SmartSellEmail"].ToString();
                                                string FilePath = TemplaTePath;
                                                StreamReader str = new StreamReader(FilePath);
                                                string MailText = str.ReadToEnd();
                                                str.Close();
                                                MailText = MailText.Replace("[customerName]", productOrderDataContract.FirstName.ToString())
                                                    .Replace("[ProductCategory]", productCategory.Description).Replace("[SelfQCUrl]", selfqcurl).Replace("[productCatDescription]", productCategory.Description);
                                                jetmessage.HTMLPart = MailText;
                                                mailJet.Messages = new List<MailJetMessage>();
                                                mailJet.Messages.Add(jetmessage);
                                                BillCloudServiceCall.MailJetSendMailService(mailJet);
                                                #endregion
                                            }
                                        }
                                    }

                                    #endregion

                                }
                                #region code to send Order confirmation Except Pine Labs
                                if (productOrderDataContract.IsDefferedSettlement==true)
                                {
                                    OrderConfirmationTemplateExchange whatsappObjforOrderConfirmation = new OrderConfirmationTemplateExchange();
                                    whatsappObjforOrderConfirmation.userDetails = new UserDetails();
                                    whatsappObjforOrderConfirmation.notification = new OrderConfiirmationNotification();
                                    WhatsappNotificationManager whatsappNotificationManager = new WhatsappNotificationManager();
                                    whatsappObjforOrderConfirmation.notification.@params = new SendWhatssappForExcahangeConfirmation();
                                    whatsappObjforOrderConfirmation.notification.@params.CustName = productOrderDataContract.FirstName + " " + productOrderDataContract.LastName;
                                   whatsappObjforOrderConfirmation.notification.@params.Link = SelfQCLink;
                                    whatsappObjforOrderConfirmation.notification.@params.ProductBrand = BrandName;
                                    whatsappObjforOrderConfirmation.notification.@params.ProdCategory = ProductCategory;
                                    whatsappObjforOrderConfirmation.notification.@params.ProdType = ProductType;
                                   whatsappObjforOrderConfirmation.notification.@params.RegdNO = productOrderDataContract.RegdNo.ToString();
                                   
                                    #region sa


                                    // Step 2: Convert WhatsappTemplate data into templateParams List
                                    whatsappObjforOrderConfirmation.userDetails.number = productOrderDataContract.PhoneNumber;
                                    whatsappObjforOrderConfirmation.notification.templateId = NotificationConstants.orderConfirmationForExchange;
                                    whatsappObjforOrderConfirmation.notification.@params.CustName = productOrderDataContract.FirstName + " " + productOrderDataContract.LastName; ;
                                    whatsappObjforOrderConfirmation.notification.@params.RegdNO = productOrderDataContract.RegdNo.ToString();
                                    whatsappObjforOrderConfirmation.notification.@params.ProdCategory = ProductCategory;
                                   
                                    whatsappObjforOrderConfirmation.notification.@params.CustName = productOrderDataContract.FirstName + " " + productOrderDataContract.LastName;
                                    whatsappObjforOrderConfirmation.notification.@params.Number = productOrderDataContract.PhoneNumber;
                                    whatsappObjforOrderConfirmation.notification.@params.Email = productOrderDataContract.Email;
                                    whatsappObjforOrderConfirmation.notification.@params.Link = SelfQCLink;


                                    // Step 2: Convert WhatsappTemplate data into templateParams List
                                    List<string> templateParams = new List<string>
                                   {
                                       whatsappObjforOrderConfirmation.notification.@params.CustName,  // name
                                       whatsappObjforOrderConfirmation.notification.@params.RegdNO,      //RegdNO
                                       whatsappObjforOrderConfirmation.notification.@params.ProdCategory,    // Code
                                       whatsappObjforOrderConfirmation.notification.@params.ProdType,  // prodtype
                                       whatsappObjforOrderConfirmation.notification.@params.CustName,    
                                       whatsappObjforOrderConfirmation.notification.@params.Number,   
                                       whatsappObjforOrderConfirmation.notification.@params.Email,     
                                       whatsappObjforOrderConfirmation.notification.@params.Link,    // oclink
                                   };
                                    HttpResponseDetails response = whatsappNotificationManager.SendWhatsAppMessageAsync(
                                                        whatsappObjforOrderConfirmation.notification.templateId,
                                                        whatsappObjforOrderConfirmation.userDetails.number,
                                                        templateParams
                                                    ).GetAwaiter().GetResult();

                                    #endregion
                                    ResponseCode = response.Response.StatusCode.ToString();
                                    WhatssAppStatusEnum = ExchangeOrderManager.GetEnumDescription(WhatssAppEnum.SuccessCode);
                                    if (ResponseCode == WhatssAppStatusEnum)
                                    {
                                        string responseContent = response.Content;
                                        if (responseContent != null)
                                        {
                                            whatssappresponseDC = JsonConvert.DeserializeObject<WhatasappResponse>(responseContent);
                                            tblWhatsAppMessage whatsapObj = new tblWhatsAppMessage();
                                            whatsapObj.TemplateName = NotificationConstants.orderConfirmationForExchange;
                                            whatsapObj.IsActive = true;
                                            whatsapObj.PhoneNumber = productOrderDataContract.PhoneNumber;
                                            whatsapObj.SendDate = DateTime.Now;
                                            whatsapObj.msgId = whatssappresponseDC.msgId;
                                            _whatsAppMessageRepository.Add(whatsapObj);
                                            _whatsAppMessageRepository.SaveChanges();
                                        }
                                        else
                                        {
                                            string JsonObjectForExchangeOrder = JsonConvert.SerializeObject(productOrderDataContract);
                                            logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", productOrderDataContract.SponsorOrderNumber, JsonObjectForExchangeOrder);
                                        }
                                    }
                                    else
                                    {
                                        string JsonObjectForExchangeOrder = JsonConvert.SerializeObject(productOrderDataContract);
                                        logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", productOrderDataContract.SponsorOrderNumber, JsonObjectForExchangeOrder);
                                    }
                                }
                                #endregion


                            }
                            catch (Exception ex)
                            {
                                LibLogging.WriteErrorToDB("SponserOrderSync", "ProcessSponserInfo", ex);
                            }

                        }
                    }

                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SponserOrderSync", "ProcessSponserInfo", ex);
            }

            return productOrderResponseDC;
        }
        #endregion

        #region sync order Status details
        /// <summary>
        /// 
        /// </summary>       
        /// <returns></returns>   
        public ProductOrderStatusResponseDataContract ProcessOrderStatusInfo(ProductOrderStatusDataContract productOrderStatusDataContract)
        {
            //int orderId = 0;           
            ProductManager productOrderInfo = new ProductManager();
            SponserFormResponseDataContract sponserStatusResponseDC = null;
            SponserManager sponserManager = new SponserManager();
            UpdateSponserOrderStatusDataContract sponserStatusDC = null;
            ProductOrderStatusResponseDataContract productOrderStatusResponseDC = null;
            productOrderStatusResponseDC = new ProductOrderStatusResponseDataContract();
            SponserListDataContract sponserListDC = null;
            string ZohoPushFlag = null;
            try
            {
                if (productOrderStatusDataContract != null)
                {

                    #region Code to add product order in database
                    productOrderStatusResponseDC.OrderId = productOrderInfo.UpdateOrderStatus(productOrderStatusDataContract);
                    #endregion

                    #region Code to push sponser order status in zoho creator
                    ZohoPushFlag = ConfigurationManager.AppSettings["ZohoPush"].ToString();
                    if (productOrderStatusResponseDC.OrderId != 0 && ZohoPushFlag == "true")
                    {
                        // get Sponser order Id 
                        string zohoSponsorOrderId = productOrderInfo.GetOrderById(productOrderStatusResponseDC.OrderId);
                        if (zohoSponsorOrderId != null)
                            // get sponsor order detail by id
                            sponserListDC = sponserManager.GetSponserOrderById(zohoSponsorOrderId);
                        if (sponserListDC != null)
                        {
                            if (sponserListDC.data != null && sponserListDC.data.Count > 0)
                            {
                                //Set Sponsor order status object
                                sponserStatusDC = sponserManager.SetSponsorOrderStatusObject(productOrderStatusDataContract, sponserListDC);
                                //Add Sponsor order status 
                                sponserStatusResponseDC = sponserManager.UpdateSponserOrderStatus(sponserStatusDC);
                                if (productOrderStatusDataContract.Status != null)
                                {
                                    if (productOrderStatusDataContract.Status != "Cancelled" && productOrderStatusDataContract.Status != "cancelled")
                                        productOrderStatusResponseDC.Expected_Pickup_Date = sponserListDC.data[0].Expected_Pickup_Date;
                                }
                            }

                        }

                    }
                    #endregion

                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SponserOrderSync", "ProcessSponserInfo", ex);
            }

            return productOrderStatusResponseDC;
        }
        #endregion

        #region get order Status details
        /// <summary>
        /// 
        /// </summary>       
        /// <returns></returns>   
        public OrderStatusDetailsDataContract ProcessGetOrderStatusInfo(int orderId)
        {
            _exchangeOrderStatusRepository1 = new ExchangeOrderStatusRepository();
            _OrderTransactionRepository = new OrderTransactionRepository();
            _OrderLGC_Repository = new OrderLGC_Repository();
            ProductManager productOrderInfo = new ProductManager();
            SponserManager sponserManager = new SponserManager();
            OrderStatusDetailsDataContract orderStatusDetailsDC = null;
            orderStatusDetailsDC = new OrderStatusDetailsDataContract();
            ExchangeOrderRepository sponserRepository = new ExchangeOrderRepository();
            //ExchangeOrderStatusRepository exchangeOrderStatusRepository;
            try
            {
                if (orderId != 0)
                {

                    // sponsorOrderID = 
                    #region 
                    tblExchangeOrder sponsorObj = sponserRepository.GetSingle(x => x.Id.Equals(orderId)&&x.IsActive==true);
                    if (sponsorObj != null)
                    {
                        tblExchangeOrderStatu tblExchangeOrderStatus = _exchangeOrderStatusRepository1.GetSingle(x => x.Id.Equals(sponsorObj.StatusId));

                        if (tblExchangeOrderStatus != null)
                        {

                            orderStatusDetailsDC.OrderId = orderId;

                            //orderStatusDetailsDC.Mode_of_Payment = sponserListDC.data[0].Mode;


                            // orderStatusDetailsDC.Payment_Received = sponserListDC.data[0].Actual_Amount_Paid;

                            //orderStatusDetailsDC.Reason_for_Rejection = sponserListDC.data[0].Status_Reason;
                            orderStatusDetailsDC.Reason_for_Rejection = tblExchangeOrderStatus.StatusDescription;
                            orderStatusDetailsDC.Pickup = sponsorObj.OrderStatus;
                            #region pickupdate
                            tblOrderTran tblOrderTran = _OrderTransactionRepository.GetSingle(x => x.ExchangeId.Equals(sponsorObj.Id));
                            if (tblOrderTran != null && tblOrderTran.OrderTransId > 0)
                            {
                                tblOrderLGC tblOrderLGC = _OrderLGC_Repository.GetSingle(x => x.OrderTransId == tblOrderTran.OrderTransId);
                                if (tblOrderLGC != null && tblOrderLGC.ActualPickupDate != null)
                                {
                                    orderStatusDetailsDC.Date_of_Pickup = tblOrderLGC.ActualPickupDate.ToString();
                                    orderStatusDetailsDC.Payment_Received = sponsorObj.FinalExchangePrice.ToString();

                                    if (tblOrderLGC.ProposedPickDate != null)
                                    {
                                        DateTime complaintDate = Convert.ToDateTime(tblOrderLGC.ProposedPickDate);
                                        string cDate = complaintDate.ToString("dd-MMM-yyyy");
                                        //orderStatusDetailsDC.Expected_Pickup_Date = sponserListDC.data[0].Expected_Pickup_Date;
                                        orderStatusDetailsDC.Expected_Pickup_Date = cDate;

                                    }
                                }
                                else
                                {
                                    orderStatusDetailsDC.Date_of_Pickup = string.Empty;
                                }
                            }

                            #endregion

                            // orderStatusDetailsDC.Date_of_Pickup = tblExchangeOrderStatus.


                        }


                    }
                    #endregion


                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SponserOrderSync", "ProcessGetOrderStatusInfo", ex);
            }

            return orderStatusDetailsDC;
        }
        #endregion

        #region Update Voucher status to redeem
        /// <summary>
        /// 
        /// </summary>       
        /// <returns></returns>   
        public VoucherVerificationResponseViewModel UpdateVoucherstatusToRedeemed(string RegNo)
        {
            ProductManager productOrderInfo = new ProductManager();
            SponserManager sponserManager = new SponserManager();
            OrderStatusDetailsDataContract orderStatusDetailsDC = null;
            orderStatusDetailsDC = new OrderStatusDetailsDataContract();
            ExchangeOrderRepository sponserRepository = new ExchangeOrderRepository();
            ExchangeOrderRepository _exchangeOrderRepository = new ExchangeOrderRepository();
            VoucherVerificationRepository _voucherVerificationRepository = new VoucherVerificationRepository();
            BusinessUnitRepository _businessUnitRepository = new BusinessUnitRepository();
            BusinessPartnerRepository _businessPartnerRepository = new BusinessPartnerRepository();
            VoucherVerificationResponseViewModel sucessObj = null;
            VoucherStatusRepository _voucherStatusRepository = new VoucherStatusRepository();
            SponserManager _sponserManager = new SponserManager();
            string result = "_R";
            string voucherStatusName = null;
            try
            {
                if (RegNo != null)
                {
                    #region Code to update the Bill Cloud API  (Voucher Reedeemed)
                    tblExchangeOrder exchangeOrder = _exchangeOrderRepository.GetSingle(x => x.RegdNo == RegNo);
                    if (exchangeOrder != null)
                    {
                        tblVoucherVerfication voucherVerfication = _voucherVerificationRepository.GetSingle(x => x.ExchangeOrderId == exchangeOrder.Id);
                        if (voucherVerfication != null && voucherVerfication.BusinessPartnerId != null)
                        {
                            tblBusinessPartner businessPartner = _businessPartnerRepository.GetSingle(x => x.BusinessPartnerId == voucherVerfication.BusinessPartnerId);
                            tblBusinessUnit businessUnit = _businessUnitRepository.GetSingle(x => x.BusinessUnitId == businessPartner.BusinessUnitId);

                            VoucherVerificationViewModel voucherverificationvm = new VoucherVerificationViewModel();
                            voucherverificationvm.data = new VoucherVerificationData();
                            voucherverificationvm.data.event_id = "REDEEM_VOUCHER";
                            voucherverificationvm.data.rrn = exchangeOrder.SponsorOrderNumber + result;
                            voucherverificationvm.data.dao_name = businessUnit != null ? businessUnit.Name.Trim() : string.Empty;
                            voucherverificationvm.data.payload = new VoucherVerificationPayload();
                            voucherverificationvm.data.payload.service_id = "EXCHANGE";
                            voucherverificationvm.data.payload.amount = exchangeOrder.FinalExchangePrice != null && exchangeOrder.FinalExchangePrice > 0 ? exchangeOrder.FinalExchangePrice.ToString() : exchangeOrder.ExchangePrice.ToString();
                            Convert.ToInt32(exchangeOrder.ExchangePrice).ToString();
                            decimal sweetner = 0;
                            if (exchangeOrder.IsDtoC == true)
                                sweetner = businessUnit.SweetnerForDTC != null ? Convert.ToDecimal(businessUnit.SweetnerForDTC) : 0;
                            else
                                sweetner = businessUnit.SweetnerForDTD != null ? Convert.ToDecimal(businessUnit.SweetnerForDTD) : 0;

                            voucherverificationvm.data.payload.sweetener = Convert.ToInt32(sweetner).ToString();

                            voucherverificationvm.data.payload.expiry = exchangeOrder.VoucherCodeExpDate != null ? Convert.ToDateTime(exchangeOrder.VoucherCodeExpDate).ToString("MM/dd/yyyy hh:mm:ss") : string.Empty;
                            voucherverificationvm.data.payload.voucher_id = exchangeOrder.VoucherCode.ToString();
                            voucherverificationvm.data.payload.dealer_ref_id = !string.IsNullOrEmpty(businessPartner.AssociateCode) ? businessPartner.AssociateCode.Trim() : string.Empty;
                            voucherverificationvm.data.payload.acquirer_ref_id = !string.IsNullOrEmpty(businessPartner.AssociateCode) ? businessPartner.AssociateCode.Trim() : string.Empty;
                            voucherverificationvm.data.payload.beneficiary_ref_id = exchangeOrder.CustomerDetailsId > 0 ? exchangeOrder.CustomerDetailsId.ToString() : string.Empty;
                            voucherverificationvm.data.payload.consumer_ref_id = exchangeOrder.CustomerDetailsId > 0 ? exchangeOrder.CustomerDetailsId.ToString() : string.Empty;
                            voucherverificationvm.data.payload.issuer_ref_id = businessUnit.BusinessUnitId.ToString();
                            voucherverificationvm.data.payload.abrand_ref_id = businessUnit.BusinessUnitId.ToString();
                            voucherverificationvm.data.payload.merchant_ref_id = "UTCDIGITAL";

                            bool isVoucherProccessed = false;
                            IRestResponse response = BillCloudServiceCall.Rest_InvokeZohoInvoiceServiceForPlainText(ConfigurationManager.AppSettings["VoucherProcess"].ToString(), Method.POST, voucherverificationvm);
                            if (response != null)
                            {
                                Logging logging = new Logging();
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    sucessObj = JsonConvert.DeserializeObject<VoucherVerificationResponseViewModel>(response.Content);
                                    if (sucessObj != null && sucessObj.data != null && sucessObj.data.status.ToLower().Equals("success"))
                                    {
                                        isVoucherProccessed = true;
                                    }
                                    else
                                    {
                                        logging.WriteErrorToDB("SponsrOrderSyncManager", "UpdateVoucherstatusToRedeemed", exchangeOrder.SponsorOrderNumber, response);
                                    }
                                }
                                else
                                {
                                    logging.WriteErrorToDB("SponsrOrderSyncManager", "UpdateVoucherstatusToRedeemed", exchangeOrder.SponsorOrderNumber, response);
                                }
                            }
                            if (isVoucherProccessed)
                            {
                                voucherStatusName = "Redeemed";
                                tblVoucherStatu tblVoucherStatu = _voucherStatusRepository.GetSingle(x => x.VoucherStatusName == voucherStatusName);
                                //voucherVerfication = new tblVoucherVerfication();
                                //_voucherVerificationRepository = new VoucherVerificationRepository();
                                voucherVerfication.IsVoucherused = true;
                                voucherVerfication.VoucherStatusId = tblVoucherStatu.VoucherStatusId;
                                _voucherVerificationRepository.Update(voucherVerfication);
                                _voucherVerificationRepository.SaveChanges();
                                if (exchangeOrder != null)
                                {
                                    exchangeOrder.IsVoucherused = true;
                                    exchangeOrder.VoucherStatusId = tblVoucherStatu.VoucherStatusId;
                                    _exchangeOrderRepository.Update(exchangeOrder);
                                    _exchangeOrderRepository.SaveChanges();
                                }
                            }
                            #endregion

                            #region Update Voucher Detail in Zoho
                            tblVoucherStatu voucherStatus = _voucherStatusRepository.GetSingle(x => x.VoucherStatusId == voucherVerfication.VoucherStatusId);
                            if (voucherStatus != null)
                            {
                                ExchageOrderVoucherUpdateDataContract exchOrderObj = _sponserManager.SetUpdateExchangeVoucherDetail(exchangeOrder.ZohoSponsorOrderId, Convert.ToInt32(exchangeOrder.ExchangePrice).ToString(), voucherVerfication, businessPartner, voucherStatus.VoucherStatusName);
                                _sponserManager.UpdateVoucherDetailinExchangeOrder(exchOrderObj);
                            }

                            #endregion
                        }
                    }

                }
                return sucessObj;
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SponserOrderSync", "ProcessGetOrderStatusInfo", ex);
            }

            return sucessObj;
        }


        public VoucherVerificationResponseViewModel UpdateVoucherstatusToRedeemedTemp(string RegNo)
        {
            ProductManager productOrderInfo = new ProductManager();
            SponserManager sponserManager = new SponserManager();
            OrderStatusDetailsDataContract orderStatusDetailsDC = null;
            orderStatusDetailsDC = new OrderStatusDetailsDataContract();
            ExchangeOrderRepository sponserRepository = new ExchangeOrderRepository();
            ExchangeOrderRepository _exchangeOrderRepository = new ExchangeOrderRepository();
            VoucherVerificationRepository _voucherVerificationRepository = new VoucherVerificationRepository();
            BusinessUnitRepository _businessUnitRepository = new BusinessUnitRepository();
            BusinessPartnerRepository _businessPartnerRepository = new BusinessPartnerRepository();
            VoucherVerificationResponseViewModel sucessObj = null;
            VoucherStatusRepository _voucherStatusRepository = new VoucherStatusRepository();
            SponserManager _sponserManager = new SponserManager();
            string voucherStatusName = null;
            try
            {
                if (RegNo != null)
                {
                    #region Code to update the Bill Cloud API  (Voucher Reedeemed)
                    tblExchangeOrder exchangeOrder = _exchangeOrderRepository.GetSingle(x => x.RegdNo == RegNo);
                    if (exchangeOrder != null)
                    {
                        tblVoucherVerfication voucherVerfication = _voucherVerificationRepository.GetSingle(x => x.ExchangeOrderId == exchangeOrder.Id);
                        if (voucherVerfication != null && voucherVerfication.BusinessPartnerId != null)
                        {
                            tblBusinessPartner businessPartner = _businessPartnerRepository.GetSingle(x => x.BusinessPartnerId == voucherVerfication.BusinessPartnerId);
                            tblBusinessUnit businessUnit = _businessUnitRepository.GetSingle(x => x.BusinessUnitId == businessPartner.BusinessUnitId);
                            bool isVoucherProccessed = true;
                            if (isVoucherProccessed)
                            {
                                voucherStatusName = "Captured";
                                tblVoucherStatu tblVoucherStatu = _voucherStatusRepository.GetSingle(x => x.VoucherStatusName == voucherStatusName);
                                voucherVerfication.IsVoucherused = true;
                                voucherVerfication.VoucherStatusId = tblVoucherStatu.VoucherStatusId;
                                _voucherVerificationRepository.Update(voucherVerfication);
                                _voucherVerificationRepository.SaveChanges();
                                if (exchangeOrder != null)
                                {
                                    exchangeOrder.IsVoucherused = true;
                                    exchangeOrder.VoucherStatusId = tblVoucherStatu.VoucherStatusId;
                                    _exchangeOrderRepository.Update(exchangeOrder);
                                    _exchangeOrderRepository.SaveChanges();
                                }
                            }
                            #endregion

                            #region Update Voucher Detail in Zoho
                            tblVoucherStatu voucherStatus = _voucherStatusRepository.GetSingle(x => x.VoucherStatusId == voucherVerfication.VoucherStatusId);
                            if (voucherStatus != null)
                            {
                                ExchageOrderVoucherUpdateDataContract exchOrderObj = _sponserManager.SetUpdateExchangeVoucherDetail(exchangeOrder.ZohoSponsorOrderId, Convert.ToInt32(exchangeOrder.ExchangePrice).ToString(), voucherVerfication, businessPartner, voucherStatus.VoucherStatusName);
                                _sponserManager.UpdateVoucherDetailinExchangeOrder(exchOrderObj);
                            }

                            #endregion
                        }
                    }

                }
                return sucessObj;
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SponserOrderSync", "ProcessGetOrderStatusInfo", ex);
            }

            return sucessObj;
        }
        #endregion


        #region Diakin API Calls

        public string GetDiakinCustomerDetails(string custMobile, string Password)
        {
            string XMLCustomerBody = string.Empty;

            try
            {
                if (custMobile != null)
                {
                    string MessageBody = SetupDaikinForGetCustomerDetails(custMobile);
                    string envelop = SOAPConstant.Diakin_Envelop;
                    envelop = envelop.Replace("[MessageBody]", MessageBody);
                    string URl = ConfigurationManager.AppSettings["GetCustomerIndividual"].ToString();

                    XMLCustomerBody = SOAPServiceCall.SOAP_InvokeServiceForDaikin(envelop, URl, Password);
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SponserOrderSync", "GetDiakinCustomerDetails", ex);
            }
            return XMLCustomerBody;
        }

        public string SetupDaikinForGetCustomerDetails(string custMobile)
        {
            string XMLCustomerBody = string.Empty;
            try
            {
                if (custMobile != null && custMobile != "")
                {
                    XMLCustomerBody = SOAPConstant.Diakin_GetCustomer;
                    XMLCustomerBody = XMLCustomerBody.Replace("[CustMobile]", custMobile);
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SponserOrderSync", "SetupDaikinForGetCustomerDetails", ex);
            }

            return XMLCustomerBody;
        }

        public string PushDiakinCustomer(ProductOrderDataContract productOrderDC, string Password)
        {
            _exchangeOrderRepository = new ExchangeOrderRepository();
            string XMLCustomerBody = string.Empty;
            try
            {
                if (productOrderDC != null)
                {
                    string MessageBody = SetupDaikinCustomerObject(productOrderDC);
                    string envelop = SOAPConstant.Diakin_Envelop;
                    envelop = envelop.Replace("[MessageBody]", MessageBody);
                    string url = ConfigurationManager.AppSettings["CreateCustomer"].ToString();
                    XMLCustomerBody = SOAPServiceCall.SOAP_InvokeServiceForDaikin(envelop, url, Password);
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SponserOrderSync", "PushDiakinCustomer", ex);
            }
            return XMLCustomerBody;
        }

        public string SetupDaikinCustomerObject(ProductOrderDataContract productOrderDC)
        {
            _exchangeOrderRepository = new ExchangeOrderRepository();
            string XMLCustomerBody = string.Empty;
            try
            {
                if (productOrderDC != null)
                {
                    XMLCustomerBody = SOAPConstant.Diakin_Customer;
                    XMLCustomerBody = XMLCustomerBody.Replace("[FirstName]", productOrderDC.FirstName);
                    XMLCustomerBody = XMLCustomerBody.Replace("[LastName]", productOrderDC.LastName);
                    XMLCustomerBody = XMLCustomerBody.Replace("[Email]", productOrderDC.Email);
                    XMLCustomerBody = XMLCustomerBody.Replace("[RegionCode]", "30");
                    XMLCustomerBody = XMLCustomerBody.Replace("[City]", productOrderDC.City);
                    XMLCustomerBody = XMLCustomerBody.Replace("[Pincode]", productOrderDC.ZipCode);
                    //XMLCustomerBody = XMLCustomerBody.Replace("[Address]", productOrderDC.Address1);
                    XMLCustomerBody = XMLCustomerBody.Replace("[Mobile]", productOrderDC.PhoneNumber);
                    XMLCustomerBody = XMLCustomerBody.Replace("[AreaLocality]", productOrderDC.AreaLocality);
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SponserOrderSync", "SetupDaikinCustomerObject", ex);
            }

            return XMLCustomerBody;
        }

        public string PushDiakinServiceRequest(ProductOrderSoapServiceRequest productOrderDC, string Password)
        {
            _exchangeOrderRepository = new ExchangeOrderRepository();
            string XMLServiceBody = string.Empty;
            try
            {
                if (productOrderDC != null)
                {
                    string MessageBody = SetupDaikinServiceRequestObject(productOrderDC);
                    string envelop = SOAPConstant.Diakin_Envelop;
                    envelop = envelop.Replace("[MessageBody]", MessageBody);
                    string Url = ConfigurationManager.AppSettings["ServiceRequest"].ToString();
                    XMLServiceBody = SOAPServiceCall.SOAP_InvokeServiceForDaikin(envelop, Url, Password);
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SponserOrderSync", "PushDiakinCustomer", ex);
            }
            return XMLServiceBody;
        }

        public string SetupDaikinServiceRequestObject(ProductOrderSoapServiceRequest productOrderDC)
        {
            _exchangeOrderRepository = new ExchangeOrderRepository();
            string XMLCustomerBody = string.Empty;
            try
            {
                if (productOrderDC != null)
                {
                    XMLCustomerBody = SOAPConstant.Diakin_ServiceRequest;
                    XMLCustomerBody = XMLCustomerBody.Replace("[SubType]", productOrderDC.SubType);
                    XMLCustomerBody = XMLCustomerBody.Replace("[CustomerId]", productOrderDC.CustomerId);
                    XMLCustomerBody = XMLCustomerBody.Replace("[Branch]", productOrderDC.Branch);
                    //XMLCustomerBody = XMLCustomerBody.Replace("[InstalledBaseId]", productOrderDC.InstalledBaseId);
                    XMLCustomerBody = XMLCustomerBody.Replace("[TypeCode]", productOrderDC.TypeCode);
                    // XMLCustomerBody = XMLCustomerBody.Replace("[Content]", productOrderDC.Content);
                    XMLCustomerBody = XMLCustomerBody.Replace("[WarrantyStatus]", productOrderDC.WarrantyStatus);
                    XMLCustomerBody = XMLCustomerBody.Replace("[Product_Type]", productOrderDC.Product_Type);
                    if (!string.IsNullOrEmpty(productOrderDC.EmployeeId))
                    {
                        XMLCustomerBody = XMLCustomerBody.Replace("[EmployeeId]", productOrderDC.EmployeeId);
                    }
                    else
                    {
                        XMLCustomerBody = XMLCustomerBody.Replace("<EmployeeID>[EmployeeId]</EmployeeID>", "");
                    }
                    //XMLCustomerBody = XMLCustomerBody.Replace("[EmployeeId]", productOrderDC.EmployeeId);
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SponserOrderSync", "SetupDaikinServiceRequestObject", ex);
            }

            return XMLCustomerBody;
        }

        #endregion


        #region Get Sponser exchange order Status detail 
        /// <summary>
        /// For Getting sponser order details - Created by Ashwin
        /// </summary>       
        /// <returns>orderStatusDetailsDC</returns>   
        public OrderDetailsViewModel GetOrderStatusInfo(int orderId)
        {
            OrderDetailsViewModel orderDetailsViewModel = null;
            _exchangeOrderStatusRepository1 = new ExchangeOrderStatusRepository();
            _OrderTransactionRepository = new OrderTransactionRepository();
            _OrderLGC_Repository = new OrderLGC_Repository();
            ProductManager productOrderInfo = new ProductManager();
            SponserManager sponserManager = new SponserManager();
            OrderStatusDetailsDataContract orderStatusDetailsDC = null;
            orderStatusDetailsDC = new OrderStatusDetailsDataContract();
            ExchangeOrderRepository sponserRepository = new ExchangeOrderRepository();
            _orderQC_Repository = new OrderQC_Repository();
            //ExchangeOrderStatusRepository exchangeOrderStatusRepository;
            try
            {
                if (orderId != 0)
                {
                    // sponsorOrderID = 
                    orderDetailsViewModel = new OrderDetailsViewModel();
                    #region 
                    tblExchangeOrder sponsorObj = sponserRepository.GetSingle(x => x.Id.Equals(orderId));
                    if (sponsorObj != null)
                    {
                        tblExchangeOrderStatu tblExchangeOrderStatus = _exchangeOrderStatusRepository1.GetSingle(x => x.Id.Equals(sponsorObj.StatusId));
                        orderDetailsViewModel.OrderId = orderId;
                        orderDetailsViewModel.Quality_declared_by_customer = sponsorObj.ProductCondition;

                        if (tblExchangeOrderStatus != null)
                        {
                            orderDetailsViewModel.Status_Code = tblExchangeOrderStatus.StatusCode;
                            orderDetailsViewModel.Status_description = tblExchangeOrderStatus.StatusDescription;

                            #region Update  QC Details
                            tblOrderTran tblOrderTran = _OrderTransactionRepository.GetSingle(x => x.ExchangeId.Equals(sponsorObj.Id));

                            if (tblOrderTran != null && tblOrderTran.OrderTransId > 0)
                            {
                                tblOrderQC tblOrderQC = _orderQC_Repository.GetSingle(x => x.OrderTransId == tblOrderTran.OrderTransId);
                                if (tblOrderQC != null && tblOrderQC.OrderQCId > 0)
                                {
                                    orderDetailsViewModel.Quality_as_per_qc = tblOrderQC.QualityAfterQC;
                                    orderDetailsViewModel.Price_after_QC = tblOrderQC.PriceAfterQC.ToString();
                                }
                            }
                            else
                            {
                                orderDetailsViewModel.Quality_as_per_qc = string.Empty;
                                orderDetailsViewModel.Price_after_QC = string.Empty;
                            }
                            #endregion


                            #region pickup details
                            tblOrderLGC tblOrderLGC = _OrderLGC_Repository.GetSingle(x => x.OrderTransId == tblOrderTran.OrderTransId);
                            if (tblOrderLGC != null && tblOrderLGC.ActualPickupDate != null)
                            {
                                //pickuporderlgc
                                orderDetailsViewModel.DateOfPayment = tblOrderLGC.ActualPickupDate.ToString();
                                orderDetailsViewModel.Mode_of_Payment = "CASH";
                            }
                            else
                            {
                                orderDetailsViewModel.DateOfPayment = string.Empty;
                                orderDetailsViewModel.Mode_of_Payment = string.Empty;
                            }
                            #endregion
                        }
                        else
                        {
                            orderDetailsViewModel.Status_description = string.Empty;
                        }
                    }
                    else
                    {
                        return orderDetailsViewModel;
                    }
                    #endregion


                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SponserOrderSync", "ProcessGetOrderStatusInfo", ex);
            }

            return orderDetailsViewModel;
        }
        #endregion

        public tblABBRedemption AddRedemptioRequest(ABBRedemptionDataContract redemptionDataDC, ProductOrderDataContract productorderDC, tblBusinessUnit businessUnit)
        {
            _abbRegistrationRepository = new ABBRegistrationRepository();
            _abbRedemptionRepository = new AbbRedemptionRepository();
            _voucherStatusRepository = new VoucherStatusRepository();
            tblABBRedemption redemptionObj = new tblABBRedemption();
            ExchangeOrderManager exchangeOrderManager = new ExchangeOrderManager();
            tblABBRegistration registrationObj = null;
            string OrderStatusDiscription = null;
            string voucherName = null;
            WhatasappResponse whatssappresponseDC = null;
            string responseforWhatasapp = string.Empty;
            _whatsAppMessageRepository = new WhatsappMessageRepository();
            string WhatssAppStatusEnum = string.Empty;
            string SendSMSFlag = null;
            string ResponseCode = string.Empty;
            string message = null;
            MailJetViewModel mailJet = new MailJetViewModel();
            MailJetMessage jetmessage = new MailJetMessage();
            MailJetFrom from = new MailJetFrom();
            MailjetTo to = new MailjetTo();
            logging = new Logging();
            try
            {
                SendSMSFlag = ConfigurationManager.AppSettings["sendsmsflag"].ToString();
                if (redemptionDataDC != null)
                {
                    OrderStatusDiscription = ExchangeOrderManager.GetEnumDescription(StatusEnum.OrderCreated);
                    registrationObj = _abbRegistrationRepository.GetSingle(x => x.RegdNo == productorderDC.RegdNo && x.IsActive == true);
                    if (redemptionObj != null)
                    {
                        redemptionObj.ABBRegistrationId = registrationObj.ABBRegistrationId;
                        redemptionObj.CustomerDetailsId = registrationObj.CustomerId;
                        redemptionObj.RegdNo = registrationObj.RegdNo;
                        redemptionObj.InvoiceDate = registrationObj.InvoiceDate;
                        redemptionObj.InvoiceNo = registrationObj.InvoiceNo;
                        redemptionObj.ABBRedemptionStatus = OrderStatusDiscription;
                        redemptionObj.RedemptionPeriod = redemptionDataDC.RedemptionPeriod;
                        redemptionObj.BusinessPartnerId = productorderDC.BusinessPartnerId;
                        redemptionObj.RedemptionPercentage = Convert.ToInt32(redemptionDataDC.RedemptionPercentage);
                        redemptionObj.RedemptionDate = DateTime.Now;
                        redemptionObj.RedemptionValue = redemptionDataDC.RedemptionValue;
                        redemptionObj.IsActive = true;
                        redemptionObj.CreatedBy = Convert.ToInt32(UserEnum.Admin);
                        redemptionObj.CreatedDate = DateTime.Now;
                        redemptionObj.ModifiedDate = DateTime.Now;
                        if (productorderDC.IsRedemptionInstant == true)
                        {
                            redemptionObj.IsDefferedSettelment = false;
                        }
                        else
                        {
                            redemptionObj.IsDefferedSettelment = true;
                        }
                        redemptionObj.StatusId = Convert.ToInt32(StatusEnum.OrderCreated);
                        
                        #region For Insatant voucher
                        if (productorderDC.IsRedemptionInstant == true && productorderDC.IsVoucher == true && productorderDC.voucherType == Convert.ToInt32(VoucherTypeEnum.Discount))
                        {
                            voucherName = "Generated";
                            tblVoucherStatu voucherStatu = _voucherStatusRepository.GetSingle(x => x.VoucherStatusName == voucherName);
                            redemptionObj.VoucherCodeExpDate = DateTime.Now.AddHours(Convert.ToDouble(businessUnit.VoucherExpiryTime));
                            redemptionObj.VoucherCode = exchangeOrderManager.GenerateVoucher();
                            redemptionObj.IsVoucherUsed = false;
                            redemptionObj.VoucherStatusId = voucherStatu.VoucherStatusId;
                        }
                        #endregion
                        #region For Cash voucher
                        else if (productorderDC.IsRedemptionInstant == false && productorderDC.IsVoucher == true && productorderDC.voucherType == Convert.ToInt32(VoucherTypeEnum.Cash))
                        {
                            voucherName = "Generated";
                            tblVoucherStatu voucherStatu = _voucherStatusRepository.GetSingle(x => x.VoucherStatusName == voucherName);
                            redemptionObj.VoucherCodeExpDate = DateTime.Now.AddHours(Convert.ToDouble(businessUnit.VoucherExpiryTime));
                            redemptionObj.VoucherCode = exchangeOrderManager.GenerateVoucher();
                            redemptionObj.IsVoucherUsed = false;
                            redemptionObj.VoucherStatusId = voucherStatu.VoucherStatusId;
                        }
                        #endregion
                        _abbRedemptionRepository.Add(redemptionObj);
                        _abbRedemptionRepository.SaveChanges();

                        #region Send Voucher Message to customer
                        if (productorderDC.IsRedemptionInstant == true && productorderDC.IsVoucher == true && productorderDC.voucherType == Convert.ToInt32(VoucherTypeEnum.Discount))
                        {
                            WhatsappTemplate whatsappObj = new WhatsappTemplate();
                            whatsappObj.userDetails = new UserDetails();
                            whatsappObj.notification = new Notification();
                            whatsappObj.notification.@params = new SendVoucherOnWhatssapp();
                            whatsappObj.userDetails.number = redemptionDataDC.PhoneNumber;
                            whatsappObj.notification.sender = ConfigurationManager.AppSettings["Yello.aiSenderNumber"].ToString();
                            whatsappObj.notification.type = ConfigurationManager.AppSettings["Yellow.aiMesssaheType"].ToString();
                            whatsappObj.notification.templateId = NotificationConstants.Send_Voucher_Code_Template;
                            whatsappObj.notification.@params.voucherAmount = redemptionDataDC.RedemptionValue.ToString();
                            whatsappObj.notification.@params.VoucherExpiry = Convert.ToDateTime(redemptionObj.VoucherCodeExpDate).ToString("dd/MM/yyyy");
                            whatsappObj.notification.@params.voucherCode = redemptionObj.VoucherCode.ToString();
                            whatsappObj.notification.@params.BrandName = businessUnit.Name.ToString();
                            whatsappObj.notification.@params.BrandName2 = businessUnit.Name.ToString();
                            whatsappObj.notification.@params.VoucherLink = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Home/RV/" + redemptionObj.RedemptionId;
                            string url = ConfigurationManager.AppSettings["Yellow.AiUrl"].ToString();
                            IRestResponse response = WhatsappNotificationManager.Rest_InvokeWhatsappserviceCall(url, Method.POST, whatsappObj);
                            ResponseCode = response.StatusCode.ToString();
                            WhatssAppStatusEnum = ExchangeOrderManager.GetEnumDescription(WhatssAppEnum.SuccessCode);
                            if (ResponseCode == WhatssAppStatusEnum)
                            {
                                responseforWhatasapp = response.Content;
                                if (responseforWhatasapp != null)
                                {
                                    whatssappresponseDC = JsonConvert.DeserializeObject<WhatasappResponse>(responseforWhatasapp);
                                    tblWhatsAppMessage whatsapObj = new tblWhatsAppMessage();
                                    whatsapObj.TemplateName = NotificationConstants.Send_Voucher_Code_Template;
                                    whatsapObj.IsActive = true;
                                    whatsapObj.PhoneNumber = redemptionDataDC.PhoneNumber;
                                    whatsapObj.SendDate = DateTime.Now;
                                    whatsapObj.msgId = whatssappresponseDC.msgId;
                                    _whatsAppMessageRepository.Add(whatsapObj);
                                    _whatsAppMessageRepository.SaveChanges();
                                }
                                else
                                {
                                    string JsonObjectForExchangeOrder = JsonConvert.SerializeObject(productorderDC);
                                    logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", productorderDC.RegdNo, JsonObjectForExchangeOrder);
                                }
                            }
                            else
                            {
                                string JsonObjectForExchangeOrder = JsonConvert.SerializeObject(productorderDC);
                                logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", productorderDC.RegdNo, JsonObjectForExchangeOrder);
                            }


                            string voucherUrl = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Home/RV/" + redemptionObj.RedemptionId;
                            if (SendSMSFlag == "true")
                            {
                                message = NotificationConstants.SMS_VoucherRedemption_Confirmation.Replace("[ExchPrice]", redemptionObj.RedemptionValue.ToString()).Replace("[VCODE]", redemptionObj.VoucherCode)
                                .Replace("[VLink]", "( " + voucherUrl + " )").Replace("[STORENAME]", businessUnit.Name).Replace("[COMPANY]", businessUnit.Name).Replace("[VALIDTILLDATE]", Convert.ToDateTime(redemptionObj.VoucherCodeExpDate).ToString("dd/MM/yyyy"));
                                _notificationManager.SendNotificationSMS(redemptionDataDC.PhoneNumber, message, null);
                            }

                            jetmessage.From = new MailJetFrom() { Email = "customercare@rdcel.com", Name = "UTC - Customer  Care" };
                            jetmessage.To = new List<MailjetTo>();
                            jetmessage.To.Add(new MailjetTo() { Email = redemptionDataDC.Email.Trim(), Name = redemptionDataDC.FirstName });
                            jetmessage.Subject = businessUnit.Name + ": ABBRedemption Voucher Detail";
                            string TemplaTePath = ConfigurationManager.AppSettings["VoucherGenerationInstant"].ToString();
                            string FilePath = TemplaTePath;
                            StreamReader str = new StreamReader(FilePath);
                            string MailText = str.ReadToEnd();
                            str.Close();
                            MailText = MailText.Replace("[ExchPrice]", redemptionDataDC.RedemptionValue.ToString()).Replace("[VCode]", redemptionObj.VoucherCode).Replace("[FirstName]", redemptionDataDC.FirstName)
                            .Replace("[VLink]", voucherUrl).Replace("[STORENAME]", businessUnit.Name).Replace("[VALIDTILLDATE]", Convert.ToDateTime(redemptionObj.VoucherCodeExpDate).ToString("dd/MM/yyyy"));
                            jetmessage.HTMLPart = MailText;
                            mailJet.Messages = new List<MailJetMessage>();
                            mailJet.Messages.Add(jetmessage);
                            BillCloudServiceCall.MailJetSendMailService(mailJet);
                        }
                        else if (productorderDC.IsRedemptionInstant == false && productorderDC.IsVoucher == true && productorderDC.voucherType == Convert.ToInt32(VoucherTypeEnum.Cash))
                        {
                            #region code to send whatssapp notification for cash voucher

                            string voucherUrl = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Home/RVC/" + redemptionObj.RedemptionId;

                            if (SendSMSFlag == "true")
                            {
                                message = NotificationConstants.SMS_VOUCHER_GENERATIONCash.Replace("[ExchPrice]", redemptionObj.RedemptionValue.ToString()).Replace("[VCODE]", redemptionObj.VoucherCode)
                                 .Replace("[VLink]", "( " + voucherUrl + " )").Replace("[STORENAME]", businessUnit.Name);
                                _notificationManager.SendNotificationSMS(redemptionDataDC.PhoneNumber, message, null);
                            }
                            jetmessage.From = new MailJetFrom() { Email = "customercare@rdcel.com", Name = "UTC - Customer  Care" };
                            jetmessage.To = new List<MailjetTo>();
                            jetmessage.To.Add(new MailjetTo() { Email = redemptionDataDC.Email.Trim(), Name = redemptionDataDC.FirstName });
                            jetmessage.Subject = businessUnit.Name + ": Redemption Voucher Detail";
                            string TemplaTePath = ConfigurationManager.AppSettings["VoucherGenerationCash"].ToString();
                            string FilePath = TemplaTePath;
                            StreamReader str = new StreamReader(FilePath);
                            string MailText = str.ReadToEnd();
                            str.Close();
                            MailText = MailText.Replace("[ExchPrice]", redemptionObj.RedemptionValue.ToString()).Replace("[VCode]", redemptionObj.VoucherCode).Replace("[FirstName]", redemptionDataDC.FirstName)
                                .Replace("[VLink]", voucherUrl).Replace("[STORENAME]", businessUnit.Name).Replace("[VALIDTILLDATE]", "7 days from quality check of your appliance");
                            jetmessage.HTMLPart = MailText;
                            mailJet.Messages = new List<MailJetMessage>();
                            mailJet.Messages.Add(jetmessage);
                            BillCloudServiceCall.MailJetSendMailService(mailJet);
                            #endregion

                            #region code to send whatsappNotification For Voucher Generation for csh voucher
                            WhatsappTemplatecashvoucher whatsappObj = new WhatsappTemplatecashvoucher();
                            whatsappObj.userDetails = new UserDetails();
                            whatsappObj.notification = new NotificationForCash();
                            whatsappObj.notification.@params = new SendCashVoucherOnWhatssapp();
                            whatsappObj.userDetails.number = redemptionDataDC.PhoneNumber;
                            whatsappObj.notification.sender = ConfigurationManager.AppSettings["Yello.aiSenderNumber"].ToString();
                            whatsappObj.notification.type = ConfigurationManager.AppSettings["Yellow.aiMesssaheType"].ToString();
                            whatsappObj.notification.templateId = NotificationConstants.cash_voucher;
                            whatsappObj.notification.@params.voucherAmount = redemptionObj.RedemptionValue.ToString();
                            whatsappObj.notification.@params.VoucherExpiry = 7.ToString();
                            whatsappObj.notification.@params.voucherCode = redemptionObj.VoucherCode.ToString();
                            whatsappObj.notification.@params.BrandName = businessUnit.Name.ToString();
                            whatsappObj.notification.@params.VoucherLink = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Home/RVC/" + redemptionObj.RedemptionId;
                            string url = ConfigurationManager.AppSettings["Yellow.AiUrl"].ToString();
                            IRestResponse response = WhatsappNotificationManager.Rest_InvokeWhatsappserviceCall(url, Method.POST, whatsappObj);
                            ResponseCode = response.StatusCode.ToString();
                            WhatssAppStatusEnum = ExchangeOrderManager.GetEnumDescription(WhatssAppEnum.SuccessCode);
                            if (ResponseCode == WhatssAppStatusEnum)
                            {
                                responseforWhatasapp = response.Content;
                                if (responseforWhatasapp != null)
                                {
                                    whatssappresponseDC = JsonConvert.DeserializeObject<WhatasappResponse>(responseforWhatasapp);
                                    tblWhatsAppMessage whatsapObj = new tblWhatsAppMessage();
                                    whatsapObj.TemplateName = NotificationConstants.cash_voucher;
                                    whatsapObj.IsActive = true;
                                    whatsapObj.PhoneNumber = redemptionDataDC.PhoneNumber;
                                    whatsapObj.SendDate = DateTime.Now;
                                    whatsapObj.msgId = whatssappresponseDC.msgId;
                                    _whatsAppMessageRepository.Add(whatsapObj);
                                    _whatsAppMessageRepository.SaveChanges();
                                }
                                else
                                {
                                    string JsonObjectForExchangeOrder = JsonConvert.SerializeObject(productorderDC);
                                    logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", productorderDC.RegdNo, JsonObjectForExchangeOrder);
                                }
                            }
                            else
                            {
                                string JsonObjectForExchangeOrder = JsonConvert.SerializeObject(productorderDC);
                                logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", productorderDC.RegdNo, JsonObjectForExchangeOrder);
                            }

                            #endregion
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SponsorOrderSyncManager", "SetRedemptionObj", ex);
            }
            return redemptionObj;

        }

    }
}
