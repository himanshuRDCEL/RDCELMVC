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
using System.Text;
using System.Threading.Tasks;
using RDCEL.DocUpload.BAL.Common;
using RDCEL.DocUpload.BAL.ServiceCall;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DAL.Helper;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DataContract.Common;
using RDCEL.DocUpload.DataContract.SponsorModel;
using RDCEL.DocUpload.DataContract.WhatsappTemplates;
using static RDCEL.DocUpload.BAL.Common.WhatsappNotificationManager;

namespace RDCEL.DocUpload.BAL.SponsorsApiCall
{
    public class ProductManager
    {
        #region Variable Declaration
        ExchangeOrderRepository exchangeOrderRepository;
        DateTime currentDatetime = DateTime.Now.TrimMilliseconds();
        MasterManager masterManager;
       
        OrderTransactionRepository _orderTransactionRepository;
        ExchangeABBStatusHistoryRepository _ExchangeHistory;
        LOVRepository _LOVRepository;
        VoucherStatusRepository _voucherStatusRepository;
        Logging logging;
        WhatsappMessageRepository _whatsAppMessageRepository;
        NotificationManager _notificationManager;
        #endregion

        #region Get Sponsor Order by Id from database
        /// <summary>
        /// Method to Get Sponsor Order by Id
        /// </summary>       
        /// <returns></returns>   
        public string GetOrderById(int orderId)
        {
            exchangeOrderRepository = new ExchangeOrderRepository();
            string result = null;
            try
            {
                tblExchangeOrder tempexchangeOrderInfo = exchangeOrderRepository.GetSingle(x => x.Id.Equals(orderId));
                if (tempexchangeOrderInfo != null)
                {
                    result = tempexchangeOrderInfo.ZohoSponsorOrderId;
                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ProductManager", "AddOrder", ex);
            }

            return result;
        }
        #endregion

        #region Add Sponsor Order in database
        /// <summary>
        /// Method to add the Order
        /// </summary>       
        /// <returns></returns>   
        public int AddOrder(ProductOrderDataContract productOrderDataContract, tblBusinessUnit businessUnit)
        {
            exchangeOrderRepository = new ExchangeOrderRepository();
            _voucherStatusRepository = new VoucherStatusRepository();
            _whatsAppMessageRepository = new WhatsappMessageRepository();
            _notificationManager = new NotificationManager();
            string voucherName = null;
            ExchangeOrderManager exchangeOrderManager = new ExchangeOrderManager();
            WhatasappResponse whatssappresponseDC = null;
            MailJetViewModel mailJet = new MailJetViewModel();
            MailJetMessage jetmessage = new MailJetMessage();
            MailJetFrom from = new MailJetFrom();
            MailjetTo to = new MailjetTo();
            logging = new Logging();
            string message = null;
            string responseforWhatasapp = string.Empty;
            string WhatssAppStatusEnum = string.Empty;
            string SendMessageFlag = null;
            int result = 0;
            string ResponseCode = string.Empty;
            try
            {
                SendMessageFlag = ConfigurationManager.AppSettings["sendsmsflag"].ToString();
                tblExchangeOrder exchangeOrderInfo = SetOrderObjectJson(productOrderDataContract);
                if (exchangeOrderInfo != null && businessUnit != null)
                {
                    exchangeOrderInfo.CompanyName = businessUnit.Name;
                    exchangeOrderInfo.LoginID = businessUnit.LoginId;
                }
                #region Voucher generation for order by V.C date 07/08/2023
                if (productOrderDataContract.IsDefferedSettlement == false && productOrderDataContract.IsVoucher == true && productOrderDataContract.voucherType == Convert.ToInt32(VoucherTypeEnum.Discount) && !string.IsNullOrEmpty(productOrderDataContract.FirstName) && !string.IsNullOrEmpty(productOrderDataContract.Email) && !string.IsNullOrEmpty(productOrderDataContract.PhoneNumber))
                {
                    #region For Insatant voucher
                    voucherName = "Generated";
                    tblVoucherStatu voucherStatu = _voucherStatusRepository.GetSingle(x => x.VoucherStatusName == voucherName);
                    exchangeOrderInfo.VoucherCodeExpDate = DateTime.Now.AddHours(Convert.ToDouble(businessUnit.VoucherExpiryTime));
                    exchangeOrderInfo.VoucherCode = exchangeOrderManager.GenerateVoucher();
                    exchangeOrderInfo.IsVoucherused = false;
                    exchangeOrderInfo.VoucherStatusId = voucherStatu.VoucherStatusId;
                    exchangeOrderInfo.IsActive = true;
                    #endregion

                }
                else if (productOrderDataContract.IsDefferedSettlement == true && productOrderDataContract.IsVoucher == true && productOrderDataContract.voucherType == Convert.ToInt32(VoucherTypeEnum.Cash) && !string.IsNullOrEmpty(productOrderDataContract.FirstName) && !string.IsNullOrEmpty(productOrderDataContract.Email) && !string.IsNullOrEmpty(productOrderDataContract.PhoneNumber))
                {
                    #region For Cash voucher 
                    voucherName = "Generated";
                    tblVoucherStatu voucherStatu = _voucherStatusRepository.GetSingle(x => x.VoucherStatusName == voucherName);
                    exchangeOrderInfo.VoucherCodeExpDate = DateTime.Now.AddHours(Convert.ToDouble(businessUnit.VoucherExpiryTime));
                    exchangeOrderInfo.VoucherCode = exchangeOrderManager.GenerateVoucher();
                    exchangeOrderInfo.IsVoucherused = false;
                    exchangeOrderInfo.VoucherStatusId = voucherStatu.VoucherStatusId;
                    exchangeOrderInfo.IsActive = true;
                    #endregion
                }
                #endregion
                exchangeOrderRepository.Add(exchangeOrderInfo);
                exchangeOrderRepository.SaveChanges();
                result = exchangeOrderInfo.Id;


                #region code to send voucher notification to customer
                //send sms
                if (productOrderDataContract.IsDefferedSettlement == false && productOrderDataContract.IsVoucher == true && productOrderDataContract.voucherType == Convert.ToInt32(VoucherTypeEnum.Discount) && !string.IsNullOrEmpty(productOrderDataContract.FirstName) && !string.IsNullOrEmpty(productOrderDataContract.Email) && !string.IsNullOrEmpty(productOrderDataContract.PhoneNumber))
                {
                    WhatsappTemplate whatsappObj = new WhatsappTemplate();
                    whatsappObj.userDetails = new UserDetails();
                    whatsappObj.notification = new Notification();
                    whatsappObj.notification.@params = new SendVoucherOnWhatssapp();
                    WhatsappNotificationManager whatsappNotificationManager = new WhatsappNotificationManager();
                    //whatsappObj.userDetails.number = productOrderDataContract.PhoneNumber;
                    //whatsappObj.notification.sender = ConfigurationManager.AppSettings["Yello.aiSenderNumber"].ToString();
                    //whatsappObj.notification.type = ConfigurationManager.AppSettings["Yellow.aiMesssaheType"].ToString();
                    //whatsappObj.notification.templateId = NotificationConstants.Send_Voucher_Code_Template;
                    //whatsappObj.notification.@params.voucherAmount = exchangeOrderInfo.ExchangePrice.ToString();
                    //whatsappObj.notification.@params.VoucherExpiry = Convert.ToDateTime(exchangeOrderInfo.VoucherCodeExpDate).ToString("dd/MM/yyyy");
                    //whatsappObj.notification.@params.voucherCode = exchangeOrderInfo.VoucherCode.ToString();
                    //whatsappObj.notification.@params.BrandName = businessUnit.Name.ToString();
                    //whatsappObj.notification.@params.BrandName2 = businessUnit.Name.ToString();
                    //whatsappObj.notification.@params.VoucherLink = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Home/V/" + exchangeOrderInfo.Id;
                    //string url = ConfigurationManager.AppSettings["Yellow.AiUrl"].ToString();
                    //IRestResponse response = WhatsappNotificationManager.Rest_InvokeWhatsappserviceCall(url, Method.POST, whatsappObj);
                    #region sa
                    // Assign values
                    whatsappObj.userDetails.number = productOrderDataContract.PhoneNumber;
                    whatsappObj.notification.templateId = NotificationConstants.Send_Voucher_Code_Template;
                    whatsappObj.notification.@params.voucherAmount = exchangeOrderInfo.ExchangePrice.ToString();
                    whatsappObj.notification.@params.VoucherExpiry = Convert.ToDateTime(exchangeOrderInfo.VoucherCodeExpDate).ToString("dd/MM/yyyy");
                    whatsappObj.notification.@params.voucherCode = exchangeOrderInfo.VoucherCode.ToString();
                    whatsappObj.notification.@params.BrandName = businessUnit.Name.ToString();
                    whatsappObj.notification.@params.VoucherLink = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Home/V/" + exchangeOrderInfo.Id;

                    // Step 2: Convert WhatsappTemplate data into templateParams List
                    List<string> templateParams = new List<string>
                                   {
                                                      whatsappObj.notification.@params.voucherAmount,  // Price
                                                       whatsappObj.notification.@params.BrandName,      // Brand Name
                                                       whatsappObj.notification.@params.voucherCode,    // Code
                                                       whatsappObj.notification.@params.VoucherExpiry,  // Validity
                                                       whatsappObj.notification.@params.VoucherLink     // Download URL
                                                   };
                                    HttpResponseDetails response = whatsappNotificationManager.SendWhatsAppMessageAsync(
                                                        whatsappObj.notification.templateId,
                                                    whatsappObj.userDetails.number,
                                                    templateParams
                                                ).GetAwaiter().GetResult();

                    #endregion
                    ResponseCode = response.Response.StatusCode.ToString();
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


                    string voucherUrl = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Home/V/" + exchangeOrderInfo.Id;
                    if (SendMessageFlag == "true")
                    {
                        message = NotificationConstants.SMS_VoucherRedemption_Confirmation.Replace("[ExchPrice]", exchangeOrderInfo.ExchangePrice.ToString()).Replace("[VCODE]", exchangeOrderInfo.VoucherCode)
                        .Replace("[VLink]", "( " + voucherUrl + " )").Replace("[STORENAME]", businessUnit.Name).Replace("[COMPANY]", businessUnit.Name).Replace("[VALIDTILLDATE]", Convert.ToDateTime(exchangeOrderInfo.VoucherCodeExpDate).ToString("dd/MM/yyyy"));
                        _notificationManager.SendNotificationSMS(productOrderDataContract.PhoneNumber, message, null);
                    }

                    jetmessage.From = new MailJetFrom() { Email = "hp@rdcel.com", Name = "Rocking Deals - Customer  Care" };
                    jetmessage.To = new List<MailjetTo>();
                    jetmessage.To.Add(new MailjetTo() { Email = productOrderDataContract.Email.Trim(), Name = productOrderDataContract.FirstName });
                    jetmessage.Subject = businessUnit.Name + ": Exchange Voucher Detail";
                    string TemplaTePath = ConfigurationManager.AppSettings["VoucherGenerationInstant"].ToString();
                    string FilePath = TemplaTePath;
                    StreamReader str = new StreamReader(FilePath);
                    string MailText = str.ReadToEnd();
                    str.Close();
                    MailText = MailText.Replace("[ExchPrice]", exchangeOrderInfo.ExchangePrice.ToString()).Replace("[VCode]", exchangeOrderInfo.VoucherCode).Replace("[FirstName]", productOrderDataContract.FirstName)
                    .Replace("[VLink]", voucherUrl).Replace("[STORENAME]", businessUnit.Name).Replace("[VALIDTILLDATE]", Convert.ToDateTime(exchangeOrderInfo.VoucherCodeExpDate).ToString("dd/MM/yyyy"));
                    jetmessage.HTMLPart = MailText;
                    mailJet.Messages = new List<MailJetMessage>();
                    mailJet.Messages.Add(jetmessage);
                    BillCloudServiceCall.MailJetSendMailService(mailJet);
                }
                else if (productOrderDataContract.IsDefferedSettlement == true && productOrderDataContract.IsVoucher == true && productOrderDataContract.voucherType == Convert.ToInt32(VoucherTypeEnum.Cash) && !string.IsNullOrEmpty(productOrderDataContract.FirstName) && !string.IsNullOrEmpty(productOrderDataContract.Email) && !string.IsNullOrEmpty(productOrderDataContract.PhoneNumber))
                {
                    #region code to send whatssapp notification for cash voucher
                   
                    string voucherUrl = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Home/VC/" + exchangeOrderInfo.Id;

                    if (SendMessageFlag == "true")
                    {
                        message = NotificationConstants.SMS_VOUCHER_GENERATIONCash.Replace("[ExchPrice]", exchangeOrderInfo.ExchangePrice.ToString()).Replace("[VCODE]", exchangeOrderInfo.VoucherCode)
                         .Replace("[VLink]", "( " + voucherUrl + " )").Replace("[STORENAME]", businessUnit.Name);
                        _notificationManager.SendNotificationSMS(productOrderDataContract.PhoneNumber, message, null);
                    }
                    jetmessage.From = new MailJetFrom() { Email = "hp@rdcel.com", Name = "Rocking Deals - Customer  Care" };
                    jetmessage.To = new List<MailjetTo>();
                    jetmessage.To.Add(new MailjetTo() { Email = productOrderDataContract.Email.Trim(), Name = productOrderDataContract.FirstName });
                    jetmessage.Subject = businessUnit.Name + ": Exchange Voucher Detail";
                    string TemplaTePath = ConfigurationManager.AppSettings["VoucherGenerationCash"].ToString();
                    string FilePath = TemplaTePath;
                    StreamReader str = new StreamReader(FilePath);
                    string MailText = str.ReadToEnd();
                    str.Close();
                    MailText = MailText.Replace("[ExchPrice]", exchangeOrderInfo.ExchangePrice.ToString()).Replace("[VCode]", exchangeOrderInfo.VoucherCode).Replace("[FirstName]", productOrderDataContract.FirstName)
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
                    whatsappObj.userDetails.number = productOrderDataContract.PhoneNumber;
                    whatsappObj.notification.sender = ConfigurationManager.AppSettings["Yello.aiSenderNumber"].ToString();
                    whatsappObj.notification.type = ConfigurationManager.AppSettings["Yellow.aiMesssaheType"].ToString();
                    whatsappObj.notification.templateId = NotificationConstants.cash_voucher;
                    whatsappObj.notification.@params.voucherAmount = exchangeOrderInfo.ExchangePrice.ToString();
                    whatsappObj.notification.@params.VoucherExpiry = 7.ToString();
                    whatsappObj.notification.@params.voucherCode = exchangeOrderInfo.VoucherCode.ToString();
                    whatsappObj.notification.@params.BrandName = businessUnit.Name.ToString();
                    whatsappObj.notification.@params.VoucherLink = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Home/VC/" + exchangeOrderInfo.Id;
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

                    #endregion
                    

                }
                #endregion

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ProductManager", "AddOrder", ex);
            }

            return result;
        }
        #endregion

        #region Add Sponsor Order in database
        /// <summary>
        /// Method to add the Order
        /// </summary>       
        /// <returns></returns>   
        public int UpdateOrder(ProductOrderDataContract productOrderDataContract, string zohoSponsorOrderId, int tblSponserId)
        {
            exchangeOrderRepository = new ExchangeOrderRepository();

            int result = 0;
            try
            {

                if (zohoSponsorOrderId != null && tblSponserId != 0)
                {
                    tblExchangeOrder tempexchangeOrderInfo = exchangeOrderRepository.GetSingle(x => x.Id.Equals(tblSponserId));
                    if (tempexchangeOrderInfo != null)
                    {
                        tempexchangeOrderInfo.ZohoSponsorOrderId = zohoSponsorOrderId;
                        tempexchangeOrderInfo.ModifiedDate = currentDatetime;

                        exchangeOrderRepository.Update(tempexchangeOrderInfo);

                        exchangeOrderRepository.SaveChanges();
                        result = tempexchangeOrderInfo.Id;
                    }

                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ProductManager", "AddOrder", ex);
            }

            return result;
        }
        #endregion

        #region set Sponsor Order obj
        /// <summary>
        /// Method to set Order info to table
        /// </summary>
        /// <param name="productOrderDataContract">productOrderDataContract</param>     
        public tblExchangeOrder SetOrderObjectJson(ProductOrderDataContract productOrderDataContract)
        {
            masterManager = new MasterManager();
            tblExchangeOrder sponsorObj = null;
            try
            {
                if (productOrderDataContract != null)
                {
                    sponsorObj = new tblExchangeOrder();
                    sponsorObj.RegdNo = productOrderDataContract.RegdNo;
                    sponsorObj.ProductTypeId = productOrderDataContract.ProductTypeId;
                    sponsorObj.BrandId = productOrderDataContract.BrandId;
                    sponsorObj.Bonus = productOrderDataContract.Bonus;
                    sponsorObj.Sweetener = !string.IsNullOrEmpty(productOrderDataContract.Bonus) ? Convert.ToDecimal(productOrderDataContract.Bonus) : 0;
                    //sponsorObj.ProductCondition = productOrderDataContract.ProductCondition;
                    sponsorObj.SponsorOrderNumber = productOrderDataContract.SponsorOrderNumber;
                    sponsorObj.LoginID = 1;
                    sponsorObj.StatusId = Convert.ToInt32(StatusEnum.OrderCreated);
                    sponsorObj.CustomerDetailsId = productOrderDataContract.CustomerDetailsId;
                    sponsorObj.CompanyName = productOrderDataContract.CompanyName;
                    sponsorObj.EstimatedDeliveryDate = productOrderDataContract.EstimatedDeliveryDate;
                    sponsorObj.BusinessPartnerId = productOrderDataContract.BusinessPartnerId;
                    sponsorObj.StoreCode = productOrderDataContract.StoreCode;
                    if (productOrderDataContract.NewBrandId > 0)
                    {
                        sponsorObj.NewBrandId = productOrderDataContract.NewBrandId;
                    }
                    else
                    {
                        sponsorObj.NewBrandId = null;
                    }
                    if (productOrderDataContract.NewCatId > 0)
                    {
                        sponsorObj.NewProductCategoryId = productOrderDataContract.NewCatId;
                    }
                    else
                    {
                        sponsorObj.NewProductCategoryId = null;
                    }
                    if (productOrderDataContract.NewTypeId > 0)
                    {
                        sponsorObj.NewProductTypeId = productOrderDataContract.NewTypeId;
                    }
                    else
                    {
                        sponsorObj.NewProductTypeId = null;
                    }
                    if (productOrderDataContract.ModelId > 0)
                    {
                        sponsorObj.ModelNumberId = productOrderDataContract.ModelId;
                    }
                    else
                    {
                        sponsorObj.ModelNumberId = null;

                    }
                    
                    
                    sponsorObj.IsDefferedSettlement = productOrderDataContract.IsDefferedSettlement;
                    if (sponsorObj.IsDefferedSettlement == false)
                    {
                        sponsorObj.IsDtoC = false;
                    }
                    else if (sponsorObj.IsDefferedSettlement == true)
                    {
                        sponsorObj.IsDtoC = true;
                    }
                    sponsorObj.QCDate = productOrderDataContract.QCDate;
                    sponsorObj.StartTime = productOrderDataContract.StartTime;
                    sponsorObj.EndTime = productOrderDataContract.EndTime;
                    sponsorObj.OrderStatus = "Order Created";
                    sponsorObj.IsActive = true;
                    sponsorObj.CreatedDate = currentDatetime;
                    sponsorObj.ModifiedDate = currentDatetime;
                    sponsorObj.Sweetener = productOrderDataContract.Sweetener;
                    sponsorObj.SweetenerBU = productOrderDataContract.SweetenerBU;
                    sponsorObj.SweetenerBP = productOrderDataContract.SweetenerBp;
                    sponsorObj.SweetenerDigi2l = productOrderDataContract.SweetenerDigi2l;
                    sponsorObj.ExchangePrice = productOrderDataContract.ExchangePrice;
                    sponsorObj.BaseExchangePrice = productOrderDataContract.BasePrice;
                    sponsorObj.BusinessUnitId = productOrderDataContract.BUId;
                    sponsorObj.PriceMasterNameId = productOrderDataContract.priceMasterNameID;

                    if (productOrderDataContract.ProductCondition == "1")
                    {
                        sponsorObj.ProductCondition = "Excellent";
                    }
                    if (productOrderDataContract.ProductCondition == "2")
                    {
                        sponsorObj.ProductCondition = "Good";
                    }
                    if (productOrderDataContract.ProductCondition == "3")
                    {
                        sponsorObj.ProductCondition = "Average";
                    }
                    if (productOrderDataContract.ProductCondition == "4")
                    {
                        sponsorObj.ProductCondition = "Not Working";
                    }
                    ///<summary> code commented for price calculation done on controller level only by v.c (18/09/2023)
                    //productTypeRepository = new ProductTypeRepository();
                    //tblProductType tblProductType = productTypeRepository.GetSingle(x => x.Id == productOrderDataContract.ProductTypeId);
                    //if (tblProductType != null)
                    //{
                    //    //Remove BUID Hardcoading                        
                    //    string prodprice = masterManager.GetProductPrice(Convert.ToInt32(tblProductType.ProductCatId), tblProductType.Id, productOrderDataContract.BrandId, Convert.ToInt32(productOrderDataContract.ProductCondition), productOrderDataContract.BUId);
                    //    //Add Code Bonus in price (Add Sweetner
                    //    sponsorObj.ExchangePrice = !string.IsNullOrEmpty(prodprice) ? Convert.ToDecimal(prodprice) : 0;
                    //    decimal BonusAdd = 0;
                    //    if (!string.IsNullOrEmpty(productOrderDataContract.Bonus))
                    //    {
                    //         BonusAdd = Convert.ToDecimal(productOrderDataContract.Bonus);
                    //    }
                        
                    //    sponsorObj.ExchangePrice = sponsorObj.ExchangePrice + BonusAdd;
                    //    sponsorObj.BaseExchangePrice= sponsorObj.ExchangePrice - (sponsorObj.Sweetener ?? 0);
                    //}
                    ////sponsorObj.ExchPriceCode = productOrderDataContract.ExchPriceCode;
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ProductManager", "SetOrderObjectJson", ex);
            }
            return sponsorObj;
        }
        #endregion

        #region Update Sponsor Order Status in database
        /// <summary>
        /// Method to Update Order Status
        /// </summary>       
        /// <returns></returns>   
        public int UpdateOrderStatus(ProductOrderStatusDataContract productOrderStatusDataContract)
        {
            _LOVRepository = new LOVRepository();
            exchangeOrderRepository = new ExchangeOrderRepository();
            _orderTransactionRepository = new OrderTransactionRepository();
            _ExchangeHistory = new ExchangeABBStatusHistoryRepository();
            tblOrderTran orderTrans = null;
            tblExchangeABBStatusHistory exchangeHistory = null;
            int result = 0;
            int ExchangeHistoryId = 0;
            try
            {
                //tblExchangeOrder exchangeOrderInfo = SetCancelOrderObjectJson(productOrderCancelDataContract);
                if (productOrderStatusDataContract != null)
                {
                    if (productOrderStatusDataContract.OrderId != 0)
                    {
                        tblExchangeOrder tempexchangeOrderInfo = exchangeOrderRepository.GetSingle(x => x.Id.Equals(productOrderStatusDataContract.OrderId));
                        if (tempexchangeOrderInfo != null)
                        {
                            #region Commented code
                            //if (tempexchangeOrderInfo.StatusId == 9 && tempexchangeOrderInfo.StatusId == 10 && tempexchangeOrderInfo.StatusId == 11 && tempexchangeOrderInfo.StatusId == 12 && tempexchangeOrderInfo.StatusId == 13 && tempexchangeOrderInfo.StatusId == 14 && tempexchangeOrderInfo.StatusId == 16 && tempexchangeOrderInfo.StatusId == 33 && tempexchangeOrderInfo.StatusId == 41 && tempexchangeOrderInfo.StatusId == 53 && tempexchangeOrderInfo.StatusId == 57 && tempexchangeOrderInfo.StatusId == 58 && tempexchangeOrderInfo.StatusId == 60)
                            //if (tempexchangeOrderInfo.StatusId != 18 && tempexchangeOrderInfo.StatusId != 19 && tempexchangeOrderInfo.StatusId != 20 && tempexchangeOrderInfo.StatusId != 21 && tempexchangeOrderInfo.StatusId != 22 && tempexchangeOrderInfo.StatusId != 23 && tempexchangeOrderInfo.StatusId != 24 && tempexchangeOrderInfo.StatusId != 25 && tempexchangeOrderInfo.StatusId != 26 && tempexchangeOrderInfo.StatusId != 27 && tempexchangeOrderInfo.StatusId != 30 && tempexchangeOrderInfo.StatusId != 31 && tempexchangeOrderInfo.StatusId != 32 && tempexchangeOrderInfo.StatusId != 34 && tempexchangeOrderInfo.StatusId != 35 && tempexchangeOrderInfo.StatusId != 43 && tempexchangeOrderInfo.StatusId != 44)
                            //{
                            //    if (productOrderStatusDataContract.Status == "Cancelled" || productOrderStatusDataContract.Status == "cancelled")
                            //    {
                            //        tempexchangeOrderInfo.StatusId = Convert.ToInt32(StatusEnum.OrderCancell);
                            //    }

                            //    else if (productOrderStatusDataContract.Status == "Delivered" || productOrderStatusDataContract.Status == "delivered")
                            //    {
                            //        tempexchangeOrderInfo.StatusId = Convert.ToInt32(StatusEnum.InstallationOfnewProduct);
                            //    }

                            //    tempexchangeOrderInfo.OrderStatus = productOrderStatusDataContract.Status;
                            //    tempexchangeOrderInfo.ModifiedDate = currentDatetime;
                            //    exchangeOrderRepository.Update(tempexchangeOrderInfo);

                            //    exchangeOrderRepository.SaveChanges();
                            //    result = tempexchangeOrderInfo.Id;


                            //    orderTrans = _orderTransactionRepository.GetSingle(x => x.ExchangeId == tempexchangeOrderInfo.Id && x.IsActive == true);
                            //    if (orderTrans != null)
                            //    {
                            //        orderTrans.StatusId = tempexchangeOrderInfo.StatusId;
                            //        orderTrans.ModifiedDate = DateTime.Now;
                            //        _orderTransactionRepository.Update(orderTrans);
                            //        _orderTransactionRepository.SaveChanges();

                            //    }
                            //    exchangeHistory = new tblExchangeABBStatusHistory();
                            //    exchangeHistory.RegdNo = tempexchangeOrderInfo.RegdNo;
                            //    exchangeHistory.SponsorOrderNumber = tempexchangeOrderInfo.SponsorOrderNumber;
                            //    exchangeHistory.OrderType = Convert.ToInt32(GraspCorn.Common.Enums.OrderTypeEnum.Exchange);
                            //    exchangeHistory.CustId = tempexchangeOrderInfo.CustomerDetailsId;
                            //    exchangeHistory.StatusId = tempexchangeOrderInfo.StatusId;
                            //    exchangeHistory.IsActive = true;
                            //    exchangeHistory.CreatedBy = 3;
                            //    exchangeHistory.CreatedDate = DateTime.Now;
                            //    exchangeHistory.OrderTransId = orderTrans.OrderTransId;
                            //    string jsonResponse = JsonConvert.SerializeObject(productOrderStatusDataContract);
                            //    exchangeHistory.JsonObjectString = jsonResponse;
                            //    _ExchangeHistory.Add(exchangeHistory);
                            //    _ExchangeHistory.SaveChanges();
                            //}
                            #endregion
                            if (productOrderStatusDataContract.Status == "Cancelled" || productOrderStatusDataContract.Status == "cancelled")
                            {
                                orderTrans = _orderTransactionRepository.GetSingle(x => x.ExchangeId == tempexchangeOrderInfo.Id && x.IsActive == true);
                                var AfterPickupComplete = _LOVRepository.GetList(x => x.IsActive == true && x.ParentId == Convert.ToInt32(LOVEnum.PickupComplete)).ToList();
                                foreach (var item in AfterPickupComplete)
                                {
                                    if (tempexchangeOrderInfo.StatusId == Convert.ToInt32(item.LoVName))
                                    {
                                        #region Insert into tblExchangeABBStatusHistory
                                        exchangeHistory = new tblExchangeABBStatusHistory();
                                        exchangeHistory.RegdNo = tempexchangeOrderInfo.RegdNo;
                                        exchangeHistory.SponsorOrderNumber = tempexchangeOrderInfo.SponsorOrderNumber;
                                        exchangeHistory.OrderType = Convert.ToInt32(GraspCorn.Common.Enums.OrderTypeEnum.Exchange);
                                        exchangeHistory.CustId = tempexchangeOrderInfo.CustomerDetailsId;
                                        exchangeHistory.StatusId = Convert.ToInt32(StatusEnum.OrderCancell);
                                        exchangeHistory.OrderTransId = orderTrans != null ? orderTrans.OrderTransId : 0;
                                        exchangeHistory.IsActive = true;
                                        exchangeHistory.CreatedBy = 3;
                                        exchangeHistory.CreatedDate = DateTime.Now;
                                        string jsonResponseExch = JsonConvert.SerializeObject(productOrderStatusDataContract);
                                        exchangeHistory.JsonObjectString = jsonResponseExch;
                                        _ExchangeHistory.Add(exchangeHistory);
                                        _ExchangeHistory.SaveChanges();
                                        ExchangeHistoryId = exchangeHistory.StatusHistoryId;
                                        #endregion
                                    }
                                }

                                if(ExchangeHistoryId == 0)
                                {
                                    tempexchangeOrderInfo.StatusId = Convert.ToInt32(StatusEnum.OrderCancell);
                                    tempexchangeOrderInfo.OrderStatus = productOrderStatusDataContract.Status;
                                    tempexchangeOrderInfo.ModifiedDate = currentDatetime;
                                    exchangeOrderRepository.Update(tempexchangeOrderInfo);
                                    exchangeOrderRepository.SaveChanges();
                                    result = tempexchangeOrderInfo.Id;

                                    #region Insert into tblExchangeABBStatusHistory
                                    exchangeHistory = new tblExchangeABBStatusHistory();
                                    exchangeHistory.RegdNo = tempexchangeOrderInfo.RegdNo;
                                    exchangeHistory.SponsorOrderNumber = tempexchangeOrderInfo.SponsorOrderNumber;
                                    exchangeHistory.OrderType = Convert.ToInt32(GraspCorn.Common.Enums.OrderTypeEnum.Exchange);
                                    exchangeHistory.CustId = tempexchangeOrderInfo.CustomerDetailsId;
                                    exchangeHistory.StatusId = tempexchangeOrderInfo.StatusId;
                                    exchangeHistory.OrderTransId = orderTrans != null ? orderTrans.OrderTransId : 0;
                                    exchangeHistory.IsActive = true;
                                    exchangeHistory.CreatedBy = 3;
                                    exchangeHistory.CreatedDate = DateTime.Now;
                                    string jsonResponse = JsonConvert.SerializeObject(productOrderStatusDataContract);
                                    exchangeHistory.JsonObjectString = jsonResponse;
                                    _ExchangeHistory.Add(exchangeHistory);
                                    _ExchangeHistory.SaveChanges();
                                    ExchangeHistoryId = exchangeHistory.StatusHistoryId;
                                    #endregion
                                }
                            }
                            var statusidlist = _LOVRepository.GetList(x => x.IsActive == true && x.ParentId == Convert.ToInt32(LOVEnum.Installation)).ToList();
                            if(statusidlist != null)
                            {
                                #region Insert into tblExchangeABBStatusHistory & update in tblExchangeOrder and tblordertrans where status lay into QC status
                                foreach (var item in statusidlist)
                                {
                                    if(tempexchangeOrderInfo.StatusId == Convert.ToInt32(item.LoVName))
                                    {
                                         if (productOrderStatusDataContract.Status == "Delivered" || productOrderStatusDataContract.Status == "delivered")
                                        {
                                            tempexchangeOrderInfo.StatusId = Convert.ToInt32(StatusEnum.InstallationOfnewProduct);
                                        }

                                        tempexchangeOrderInfo.OrderStatus = productOrderStatusDataContract.Status;
                                        tempexchangeOrderInfo.ModifiedDate = currentDatetime;
                                        exchangeOrderRepository.Update(tempexchangeOrderInfo);
                                        exchangeOrderRepository.SaveChanges();
                                        result = tempexchangeOrderInfo.Id;

                                        orderTrans = _orderTransactionRepository.GetSingle(x => x.ExchangeId == tempexchangeOrderInfo.Id && x.IsActive == true);
                                        if (orderTrans != null)
                                        {
                                            orderTrans.StatusId = tempexchangeOrderInfo.StatusId;
                                            orderTrans.ModifiedDate = DateTime.Now;
                                            _orderTransactionRepository.Update(orderTrans);
                                            _orderTransactionRepository.SaveChanges();
                                        }

                                        #region Insert into tblExchangeABBStatusHistory
                                        exchangeHistory = new tblExchangeABBStatusHistory();
                                        exchangeHistory.RegdNo = tempexchangeOrderInfo.RegdNo;
                                        exchangeHistory.SponsorOrderNumber = tempexchangeOrderInfo.SponsorOrderNumber;
                                        exchangeHistory.OrderType = Convert.ToInt32(GraspCorn.Common.Enums.OrderTypeEnum.Exchange);
                                        exchangeHistory.CustId = tempexchangeOrderInfo.CustomerDetailsId;
                                        exchangeHistory.StatusId = tempexchangeOrderInfo.StatusId;
                                        exchangeHistory.OrderTransId = orderTrans.OrderTransId;
                                        exchangeHistory.IsActive = true;
                                        exchangeHistory.CreatedBy = 3;
                                        exchangeHistory.CreatedDate = DateTime.Now;
                                        string jsonResponse = JsonConvert.SerializeObject(productOrderStatusDataContract);
                                        exchangeHistory.JsonObjectString = jsonResponse;
                                        _ExchangeHistory.Add(exchangeHistory);
                                        _ExchangeHistory.SaveChanges();
                                        ExchangeHistoryId = exchangeHistory.StatusHistoryId;
                                        #endregion
                                    }                                   
                                }
                                #endregion

                                #region Insert into tblExchangeABBStatusHistory & update in tblExchangeOrder where status is order created
                                if (ExchangeHistoryId == 0 && tempexchangeOrderInfo.StatusId == 5)
                                {
                                    orderTrans = _orderTransactionRepository.GetSingle(x => x.ExchangeId == tempexchangeOrderInfo.Id && x.IsActive == true);
                                    if (orderTrans != null)
                                    {
                                        tempexchangeOrderInfo.StatusId = Convert.ToInt32(StatusEnum.InstallationOfnewProduct);
                                        tempexchangeOrderInfo.OrderStatus = productOrderStatusDataContract.Status;
                                        tempexchangeOrderInfo.ModifiedDate = currentDatetime;
                                        exchangeOrderRepository.Update(tempexchangeOrderInfo);
                                        exchangeOrderRepository.SaveChanges();
                                        result = tempexchangeOrderInfo.Id;

                                        exchangeHistory = new tblExchangeABBStatusHistory();
                                        exchangeHistory.RegdNo = tempexchangeOrderInfo.RegdNo;
                                        exchangeHistory.SponsorOrderNumber = tempexchangeOrderInfo.SponsorOrderNumber;
                                        exchangeHistory.OrderType = Convert.ToInt32(GraspCorn.Common.Enums.OrderTypeEnum.Exchange);
                                        exchangeHistory.CustId = tempexchangeOrderInfo.CustomerDetailsId;
                                        exchangeHistory.StatusId = Convert.ToInt32(StatusEnum.InstallationOfnewProduct);
                                        exchangeHistory.OrderTransId = orderTrans.OrderTransId;
                                        exchangeHistory.IsActive = true;
                                        exchangeHistory.CreatedBy = 3;
                                        exchangeHistory.CreatedDate = DateTime.Now;
                                        string jsonResponse1 = JsonConvert.SerializeObject(productOrderStatusDataContract);
                                        exchangeHistory.JsonObjectString = jsonResponse1;
                                        _ExchangeHistory.Add(exchangeHistory);
                                        _ExchangeHistory.SaveChanges();
                                        ExchangeHistoryId = exchangeHistory.StatusHistoryId;
                                    }

                                        
                                }
                                #endregion

                                #region Insert into tblExchangeABBStatusHistory where status is QC completed
                                if (ExchangeHistoryId == 0 )
                                {
                                    orderTrans = _orderTransactionRepository.GetSingle(x => x.ExchangeId == tempexchangeOrderInfo.Id && x.IsActive == true);
                                    if (orderTrans != null)
                                    {
                                        exchangeHistory = new tblExchangeABBStatusHistory();
                                        exchangeHistory.RegdNo = tempexchangeOrderInfo.RegdNo;
                                        exchangeHistory.SponsorOrderNumber = tempexchangeOrderInfo.SponsorOrderNumber;
                                        exchangeHistory.OrderType = Convert.ToInt32(GraspCorn.Common.Enums.OrderTypeEnum.Exchange);
                                        exchangeHistory.CustId = tempexchangeOrderInfo.CustomerDetailsId;
                                        exchangeHistory.OrderTransId = orderTrans.OrderTransId;
                                        exchangeHistory.StatusId = Convert.ToInt32(StatusEnum.InstallationOfnewProduct);
                                        exchangeHistory.IsActive = true;
                                        exchangeHistory.CreatedBy = 3;
                                        exchangeHistory.CreatedDate = DateTime.Now;
                                        string jsonResponse1 = JsonConvert.SerializeObject(productOrderStatusDataContract);
                                        exchangeHistory.JsonObjectString = jsonResponse1;
                                        _ExchangeHistory.Add(exchangeHistory);
                                        _ExchangeHistory.SaveChanges();
                                    }
                                }
                                #endregion
                            }

                            result = tempexchangeOrderInfo.Id;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ProductManager", "Status", ex);
            }

            return result;
        }
        #endregion


    }
}
