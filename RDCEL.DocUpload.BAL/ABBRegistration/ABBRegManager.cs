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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Hosting;
using RDCEL.DocUpload.BAL.Common;
using RDCEL.DocUpload.BAL.Manager;
using RDCEL.DocUpload.BAL.ServiceCall;
using RDCEL.DocUpload.BAL.zaakpay;
using RDCEL.DocUpload.BAL.ZohoCreatorCall;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DAL.Helper;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DataContract;
using RDCEL.DocUpload.DataContract.ABBRegistration;
using RDCEL.DocUpload.DataContract.Common;
using RDCEL.DocUpload.DataContract.ExchangeOrderDetails;
using RDCEL.DocUpload.DataContract.PluralGateway;
using RDCEL.DocUpload.DataContract.Templates;
using RDCEL.DocUpload.DataContract.WhatsappTemplates;
using RDCEL.DocUpload.DataContract.ZohoModel;

namespace RDCEL.DocUpload.BAL.ABBRegistration
{
    public class ABBRegManager
    {

        #region Variable Declaration
        ABBRegistrationRepository aBBRegistrationRepository;
        ABBPaymentRepository aBBPaymentRepository;
        Logging logging;
        DateTime currentDatetime = DateTime.Now.TrimMilliseconds();
        SponserManager sponserManager;
        MasterManager masterManager;
        ModelNumberRepository _modelNumberRepository;
        BrandRepository _brandRepository;
        ProductTypeRepository _productTypeRepository;
        ProductCategoryRepository _productCategoryRepository;
        BusinessUnitRepository _businessUnitRepository;
        BusinessPartnerRepository businessPartnerRepository;
        WhatsappMessageRepository _whatsAppMessageRepository;
        BrandSmartSellRepository _brandsmartsellRepository;
        TransactionABBPlanMasterRepository _transactionRepository;
        ABBPlanMasterRepository _abbPlanMasterRepository;
        CustomerDetailsRepository _customerDetailsrepository;
        ABBPaymentRepository _abbPaymentRepository;
        ABBPriceMasterRepository _abbPriceMasterRepository;
        MailManager _mailManager;
        CustomerFilesRepository _customerFilesRepository;
        ConfigurationRepository _configurationRepository;
        #endregion
        #region Add ABB Reg Info in database
        /// <summary>
        /// Method to add the ABB Reg Info
        /// </summary>       
        /// <returns></returns>   
        public ABBRegistrationResponseModel AddABBReg(ABBRegistrationViewModel ABBRegistrationVM)
        {
            businessPartnerRepository = new BusinessPartnerRepository();
            _businessUnitRepository = new BusinessUnitRepository();
            _productCategoryRepository = new ProductCategoryRepository();
            _productTypeRepository = new ProductTypeRepository();
            _brandRepository = new BrandRepository();
            _modelNumberRepository = new ModelNumberRepository();
            string responseforWhatasapp = string.Empty;
            _whatsAppMessageRepository = new WhatsappMessageRepository();
            WhatasappResponse whatssappresponseDC = null;
            MailJetViewModel mailJet = new MailJetViewModel();
            MailJetMessage jetmessage = new MailJetMessage();
            MailJetFrom from = new MailJetFrom();
            MailjetTo to = new MailjetTo();
            logging = new Logging();
            string WhatssAppStatusEnum = string.Empty;
            string ResponseCode = string.Empty;
            _customerDetailsrepository = new CustomerDetailsRepository();
            tblCustomerDetail customerDetailsObj = new tblCustomerDetail();
            tblBrand brandObjE = new tblBrand();
            tblBrandSmartBuy brandObjBS = new tblBrandSmartBuy();
            aBBRegistrationRepository = new ABBRegistrationRepository();
            _abbPlanMasterRepository = new ABBPlanMasterRepository();
            _abbPaymentRepository = new ABBPaymentRepository();
            List<tblABBPlanMaster> planMasterObj = new List<tblABBPlanMaster>();
            ABBRegistrationResponseModel abbResponse = new ABBRegistrationResponseModel();     
            string BrandForABB = null;

            ABBPlanMargin marginDC = new ABBPlanMargin();
            try
            {
                if (string.IsNullOrEmpty(ABBRegistrationVM.CustCity)&& !string.IsNullOrEmpty(ABBRegistrationVM.city1))
                {
                    ABBRegistrationVM.CustCity = ABBRegistrationVM.city1;
                }

                if (string.IsNullOrEmpty(ABBRegistrationVM.CustState) && !string.IsNullOrEmpty(ABBRegistrationVM.state1))
                {
                    ABBRegistrationVM.CustState = ABBRegistrationVM.state1;
                }
                customerDetailsObj = SetcustomerDetailsObj(ABBRegistrationVM);
                {
                    _customerDetailsrepository.Add(customerDetailsObj);
                    _customerDetailsrepository.SaveChanges();
                    ABBRegistrationVM.CustomerDetailsId = customerDetailsObj.Id;
                }
                marginDC = GetPlanMargin(ABBRegistrationVM);
                tblABBRegistration exchangeOrderInfo = SetABBRegObjectJson(ABBRegistrationVM);
                {
                    if (exchangeOrderInfo.UploadDateTime == null)
                    {
                        DateTime dt = DateTime.Now;
                        string date = dt.ToString("yyyy-MM-dd");
                        exchangeOrderInfo.UploadDateTime =Convert.ToDateTime(date);
                    }
                    exchangeOrderInfo.BusinessUnitMargin = marginDC.BusinessUnitMargin;
                    exchangeOrderInfo.DealerMargin = marginDC.BusinessPartnerMargin;
                    exchangeOrderInfo.BaseValue = marginDC.BaseValue;
                    exchangeOrderInfo.Cgst = marginDC.Cgst;
                    exchangeOrderInfo.Sgst = marginDC.Sgst;
                    exchangeOrderInfo.ABBFees = marginDC.abbFees;
                    aBBRegistrationRepository.Add(exchangeOrderInfo);
                    aBBRegistrationRepository.SaveChanges();
                    abbResponse.RegistrationId = exchangeOrderInfo.ABBRegistrationId;
                }

                if (ABBRegistrationVM.IsBuMultibrand == true)
                {
                    brandObjBS = _brandsmartsellRepository.GetSingle(x => x.IsActive == true && x.Id == ABBRegistrationVM.NewBrandId);
                    if (brandObjBS != null)
                    {
                        BrandForABB = brandObjBS.Name;
                    }
                }
                else
                {
                    brandObjE = _brandRepository.GetSingle(x => x.Id == ABBRegistrationVM.NewBrandId && x.IsActive == true);
                    if (brandObjE != null)
                    {
                        BrandForABB = brandObjE.Name;
                    }
                }

                if (ABBRegistrationVM.BusinessUnitId == Convert.ToInt32(BusinessUnitEnum.Bosch))
                {
                    #region code to send whatsappNotification For abb order confirmation
                    ABBOrderConfirmation whatsappObj = new ABBOrderConfirmation();
                    whatsappObj.userDetails = new UserDetails();
                    whatsappObj.notification = new NotificationForABB();
                    whatsappObj.notification.@params = new SendSmsABBWhatsapp();
                    whatsappObj.userDetails.number = ABBRegistrationVM.CustMobile;
                    whatsappObj.notification.sender = ConfigurationManager.AppSettings["Yello.aiSenderNumber"].ToString();
                    whatsappObj.notification.type = ConfigurationManager.AppSettings["Yellow.aiMesssaheType"].ToString();
                    whatsappObj.notification.templateId = NotificationConstants.abb_order_confirmation;
                    whatsappObj.notification.@params.RegdNo = ABBRegistrationVM.RegdNo.ToString();
                    string url = ConfigurationManager.AppSettings["Yellow.AiUrl"].ToString();
                    IRestResponse response = WhatsappNotificationManager.Rest_InvokeWhatsappserviceCall(url, Method.POST, whatsappObj);
                    ResponseCode = response.StatusCode.ToString();
                    WhatssAppStatusEnum = SponsorsApiCall.ExchangeOrderManager.GetEnumDescription(WhatssAppEnum.SuccessCode);
                    if (ResponseCode == WhatssAppStatusEnum)
                    {
                        responseforWhatasapp = response.Content;
                        if (responseforWhatasapp != null)
                        {
                            whatssappresponseDC = JsonConvert.DeserializeObject<WhatasappResponse>(responseforWhatasapp);
                            tblWhatsAppMessage whatsapObj = new tblWhatsAppMessage();
                            whatsapObj.TemplateName = NotificationConstants.Test_Template;
                            whatsapObj.IsActive = true;
                            whatsapObj.PhoneNumber = ABBRegistrationVM.CustMobile;
                            whatsapObj.SendDate = DateTime.Now;
                            whatsapObj.msgId = whatssappresponseDC.msgId;
                            _whatsAppMessageRepository.Add(whatsapObj);
                            _whatsAppMessageRepository.SaveChanges();
                        }
                        else
                        {
                            string ExchOrderObj = JsonConvert.SerializeObject(ABBRegistrationVM);
                            logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", ABBRegistrationVM.RegdNo, ExchOrderObj);
                        }
                    }
                    else
                    {
                        string ExchOrderObj = JsonConvert.SerializeObject(ABBRegistrationVM);
                        logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", ABBRegistrationVM.RegdNo, ExchOrderObj);
                    }
                    #endregion

                    #region code to disable email connfirmation false before order approval may  be changes later
                    //tblProductCategory productCategory = _productCategoryRepository.GetSingle(x => x.Id == ABBRegistrationVM.NewProductCategoryId && x.IsActive == true && x.IsAllowedForNew == true);
                    //if (productCategory != null)
                    //{
                    //    tblProductType producttypeObj = _productTypeRepository.GetSingle(x => x.Id == ABBRegistrationVM.NewProductCategoryTypeId && x.IsActive == true && x.IsAllowedForNew == true);
                    //    if (producttypeObj != null)
                    //    {
                    //        tblModelNumber modelObj = _modelNumberRepository.GetSingle(x => x.ModelNumberId == ABBRegistrationVM.ModelNumberId && x.IsActive == true && x.BusinessUnitId == ABBRegistrationVM.BusinessUnitId);
                    //        if (modelObj != null)
                    //        {
                    //            planMasterObj = _abbPlanMasterRepository.GetList(x => x.IsActive == true && x.BusinessUnitId == ABBRegistrationVM.BusinessUnitId && x.ProductCatId == ABBRegistrationVM.NewProductCategoryId && x.ProductTypeId == ABBRegistrationVM.NewProductCategoryTypeId).ToList();
                    //            if (planMasterObj.Count > 0)
                    //            {
                    //                foreach (var item in planMasterObj)
                    //                {
                    //                    string FromandEndMonth = item.From_Month + "-" + item.To_Month;
                    //                    string NoClaimPeriod = item.NoClaimPeriod;
                    //                    if (FromandEndMonth == NoClaimPeriod + "-" + 12)
                    //                    {
                    //                        int percentage = Convert.ToInt32(item.Assured_BuyBack_Percentage);
                    //                        double NetValue = Convert.ToInt32(ABBPlanEnum.BoschBasicValue);
                    //                        double price = (NetValue * percentage) / 100;
                    //                        FirstYear = price.ToString();
                    //                        FirstYearPercentage = percentage.ToString();

                    //                    }
                    //                    else if (FromandEndMonth == "13-24")
                    //                    {
                    //                        int percentage = Convert.ToInt32(item.Assured_BuyBack_Percentage);
                    //                        double NetValue = Convert.ToInt32(ABBPlanEnum.BoschBasicValue);
                    //                        double price = (NetValue * percentage) / 100;
                    //                        secondYear = price.ToString();
                    //                        secondYearPercentage = percentage.ToString();
                    //                    }
                    //                    else if (FromandEndMonth == "25-36")
                    //                    {
                    //                        int percentage = Convert.ToInt32(item.Assured_BuyBack_Percentage);
                    //                        double NetValue = Convert.ToInt32(ABBPlanEnum.BoschBasicValue);
                    //                        double price = (NetValue * percentage) / 100;
                    //                        thirdYear = price.ToString();
                    //                        thirdYearPercentage = percentage.ToString();
                    //                    }
                    //                    else if (FromandEndMonth == "37-48")
                    //                    {
                    //                        int percentage = Convert.ToInt32(item.Assured_BuyBack_Percentage);
                    //                        double NetValue = Convert.ToInt32(ABBPlanEnum.BoschBasicValue);
                    //                        double price = (NetValue * percentage) / 100;
                    //                        forthYear = price.ToString();
                    //                        forthYearPercentage = percentage.ToString();
                    //                    }
                    //                    else if (FromandEndMonth == "49-60")
                    //                    {
                    //                        int percentage = Convert.ToInt32(item.Assured_BuyBack_Percentage);
                    //                        double NetValue = Convert.ToInt32(ABBPlanEnum.BoschBasicValue);
                    //                        double price = (NetValue * percentage) / 100;
                    //                        FifthYear = price.ToString();
                    //                        FifthYearPercentage = percentage.ToString();
                    //                    }
                    //                    else if (FromandEndMonth == "61-72")
                    //                    {
                    //                        int percentage = Convert.ToInt32(item.Assured_BuyBack_Percentage);
                    //                        double NetValue = Convert.ToInt32(ABBPlanEnum.BoschBasicValue);
                    //                        double price = (NetValue * percentage) / 100;
                    //                        SixthYear = price.ToString();
                    //                        SixthYearPercentage = percentage.ToString();
                    //                    }
                    //                    else if (FromandEndMonth == "73-84")
                    //                    {
                    //                        int percentage = Convert.ToInt32(item.Assured_BuyBack_Percentage);
                    //                        double NetValue = Convert.ToInt32(ABBPlanEnum.BoschBasicValue);
                    //                        double price = (NetValue * percentage) / 100;
                    //                        SeventhYear = price.ToString();
                    //                        SeventhYearPercentage = percentage.ToString();
                    //                    }
                    //                }
                    //            }

                    //            //#region Code to send mail to customer for exchange details
                    //            //jetmessage.From = new MailJetFrom() { Email = "customercare@rdcel.com", Name = "UTC - Customer  Care" };
                    //            //jetmessage.To = new List<MailjetTo>();
                    //            //jetmessage.To.Add(new MailjetTo() { Email = ABBRegistrationVM.CustEmail.Trim(), Name = ABBRegistrationVM.CustFirstName });
                    //            //jetmessage.Subject = ABBRegistrationVM.BUName + ": Assured Buy Back Detail";
                    //            //string TemplaTePath = ConfigurationManager.AppSettings["BoschABB"].ToString();
                    //            //string FilePath = TemplaTePath;
                    //            //StreamReader str = new StreamReader(FilePath);
                    //            //string MailText = str.ReadToEnd();
                    //            //str.Close();
                    //            //MailText = MailText.Replace("[BrandName]", BrandForABB).Replace("[RegdNo]", ABBRegistrationVM.RegdNo.ToString()).Replace("[StoreName]", ABBRegistrationVM.BUName).Replace("[firstname]", ABBRegistrationVM.CustFirstName).Replace("[Email]", ABBRegistrationVM.CustEmail)
                    //            //    .Replace("[Address1]", ABBRegistrationVM.CustAddress1).Replace("[Address2]", ABBRegistrationVM.CustAddress2).Replace("[pincode]", ABBRegistrationVM.CustPinCode).Replace("[city]", ABBRegistrationVM.CustCity)
                    //            //    .Replace("[productcategory]", productCategory.Description).Replace("[ptoductType]", producttypeObj.Description).Replace("[productserialno]", ABBRegistrationVM.ProductSrNo).Replace("[Modelname]", modelObj.ModelName).Replace("[invoicedate]", Convert.ToDateTime(ABBRegistrationVM.InvoiceDate).ToString("dd/MMM/yyyy"))
                    //            //    .Replace("[invoiceNumber]", ABBRegistrationVM.InvoiceNo).Replace("[Netvalue]", ABBRegistrationVM.ProductNetPrice.ToString()).Replace("[PlanPeriod]", ABBRegistrationVM.ABBPlanPeriod).Replace("[NoClaimPEriod]", ABBRegistrationVM.NoOfClaimPeriod.ToString()).Replace("[ABBPlanName]", ABBRegistrationVM.ABBPlanName).Replace("[uploaddate]", Convert.ToDateTime(ABBRegistrationVM.UploadDateTime).ToString("dd/MMM/yyyy"))
                    //            //    .Replace("[bbvalue1]", FirstYearPercentage).Replace("[bbvalue2]", secondYearPercentage).Replace("[bbvalue3]", thirdYearPercentage).Replace("[bbvalue4]", forthYearPercentage).Replace("[bbvalue5]", FifthYearPercentage).Replace("[bbvalue6]", SixthYearPercentage).Replace("[bbvalue7]", SeventhYearPercentage).Replace("[ibv1]", FirstYear).Replace("[ibv2]", secondYear)
                    //            //    .Replace("[ibv3]", thirdYear).Replace("[ibv4]", forthYear).Replace("[ibv5]", FifthYear).Replace("[ibv6]", SixthYear).Replace("[ibv7]", SeventhYear);
                    //            //jetmessage.HTMLPart = MailText;
                    //            //mailJet.Messages = new List<MailJetMessage>();
                    //            //mailJet.Messages.Add(jetmessage);
                    //            //BillCloudServiceCall.MailJetSendMailService(mailJet);
                    //            //#endregion


                    //        }
                    //        else
                    //        {
                    //            abbResponse.Messsage = "Model details not found";
                    //        }

                    //    }
                    //    else
                    //    {
                    //        abbResponse.Messsage = "Product type details not found";
                    //    }
                    //}
                    //else
                    //{
                    //    abbResponse.Messsage = "Product category details not found";
                    //}
                    #endregion  

                }

                else if (ABBRegistrationVM.IsdefferedAbb == true && ABBRegistrationVM.BusinessUnitId != Convert.ToInt32(BusinessUnitEnum.Bosch))
                {
                    tblProductCategory productCategory = _productCategoryRepository.GetSingle(x => x.Id == ABBRegistrationVM.NewProductCategoryId && x.IsActive == true && x.IsAllowedForNew == true);
                    if (productCategory != null)
                    {
                        tblProductType producttypeObj = _productTypeRepository.GetSingle(x => x.Id == ABBRegistrationVM.NewProductCategoryTypeId && x.IsActive == true && x.IsAllowedForNew == true);
                        if (producttypeObj != null)
                        {

                            #region Code to send mail to customer for exchange details currently disabled before approval of order
                            //jetmessage.From = new MailJetFrom() { Email = "customercare@rdcel.com", Name = "UTC - Customer  Care" };
                            //jetmessage.To = new List<MailjetTo>();
                            //jetmessage.To.Add(new MailjetTo() { Email = ABBRegistrationVM.CustEmail.Trim(), Name = ABBRegistrationVM.CustFirstName });
                            //jetmessage.Subject = ABBRegistrationVM.BUName + ": Assured Buy Back Detail";
                            //string TemplaTePath = ConfigurationManager.AppSettings["ABBEmail"].ToString();
                            //string FilePath = TemplaTePath;
                            //StreamReader str = new StreamReader(FilePath);
                            //string MailText = str.ReadToEnd();
                            //str.Close();
                            //MailText = MailText.Replace("[BrandName]", BrandForABB).Replace("[StoreName]", ABBRegistrationVM.BUName).Replace("[RegdNo]", ABBRegistrationVM.RegdNo).Replace("[firstname]", ABBRegistrationVM.CustFirstName)
                            //    .Replace("[Email]", ABBRegistrationVM.CustEmail).Replace("[Address1]", ABBRegistrationVM.CustAddress1).Replace("[Address2]", ABBRegistrationVM.CustAddress2).Replace("[pincode]", ABBRegistrationVM.CustPinCode)
                            //    .Replace("[city]", ABBRegistrationVM.CustCity).Replace("[productcategory]", productCategory.Description).Replace("[ptoductType]", producttypeObj.Description).Replace("[product serial no]", ABBRegistrationVM.ProductSrNo).Replace("[Modelname]", "")
                            //    .Replace("[invoicedate]", Convert.ToDateTime(ABBRegistrationVM.InvoiceDate).ToString("dd/MM/yyyy")).Replace("[invoiceNumber]", ABBRegistrationVM.InvoiceNo).Replace("[Netvalue]", ABBRegistrationVM.ProductNetPrice.ToString()).Replace("[PlanPeriod]", ABBRegistrationVM.ABBPlanPeriod).Replace("[NoClaimPEriod]", ABBRegistrationVM.NoOfClaimPeriod).Replace("[ABB PlanName]", ABBRegistrationVM.ABBPlanName)
                            //    .Replace("[upload date]", Convert.ToDateTime(ABBRegistrationVM.UploadDateTime).ToString("dd/MM/yyyy"));
                            //jetmessage.HTMLPart = MailText;
                            //mailJet.Messages = new List<MailJetMessage>();
                            //mailJet.Messages.Add(jetmessage);
                            //BillCloudServiceCall.MailJetSendMailService(mailJet);
                            #endregion

                            #region code to send whatsappNotification For abb order confirmation
                            ABBOrderConfirmation whatsappObj = new ABBOrderConfirmation();
                            whatsappObj.userDetails = new UserDetails();
                            whatsappObj.notification = new NotificationForABB();
                            whatsappObj.notification.@params = new SendSmsABBWhatsapp();
                            whatsappObj.userDetails.number = ABBRegistrationVM.CustMobile;
                            whatsappObj.notification.sender = ConfigurationManager.AppSettings["Yello.aiSenderNumber"].ToString();
                            whatsappObj.notification.type = ConfigurationManager.AppSettings["Yellow.aiMesssaheType"].ToString();
                            whatsappObj.notification.templateId = NotificationConstants.abb_order_confirmation_old;
                            whatsappObj.notification.@params.RegdNo = ABBRegistrationVM.RegdNo.ToString();
                            string url = ConfigurationManager.AppSettings["Yellow.AiUrl"].ToString();
                            IRestResponse response = WhatsappNotificationManager.Rest_InvokeWhatsappserviceCall(url, Method.POST, whatsappObj);
                            ResponseCode = response.StatusCode.ToString();
                            WhatssAppStatusEnum = SponsorsApiCall.ExchangeOrderManager.GetEnumDescription(WhatssAppEnum.SuccessCode);
                            if (ResponseCode == WhatssAppStatusEnum)
                            {
                                responseforWhatasapp = response.Content;
                                if (responseforWhatasapp != null)
                                {
                                    whatssappresponseDC = JsonConvert.DeserializeObject<WhatasappResponse>(responseforWhatasapp);
                                    tblWhatsAppMessage whatsapObj = new tblWhatsAppMessage();
                                    whatsapObj.TemplateName = NotificationConstants.Test_Template;
                                    whatsapObj.IsActive = true;
                                    whatsapObj.PhoneNumber = ABBRegistrationVM.CustMobile;
                                    whatsapObj.SendDate = DateTime.Now;
                                    whatsapObj.msgId = whatssappresponseDC.msgId;
                                    _whatsAppMessageRepository.Add(whatsapObj);
                                    _whatsAppMessageRepository.SaveChanges();
                                }
                                else
                                {
                                    string ExchOrderObj = JsonConvert.SerializeObject(ABBRegistrationVM);
                                    logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", ABBRegistrationVM.RegdNo, ExchOrderObj);
                                }
                            }
                            else
                            {
                                string ExchOrderObj = JsonConvert.SerializeObject(ABBRegistrationVM);
                                logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", ABBRegistrationVM.RegdNo, ExchOrderObj);
                            }
                            #endregion
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBRegManager", "AddABBReg", ex);
            }
            return abbResponse;
        }

        public bool ValidateDuplicateABBCall(string customerMobileNumber, string invoiceNumber, int modelNumber, DateTime PurchaseDate)
        {
            aBBRegistrationRepository = new ABBRegistrationRepository();
            bool flag = false;
            try
            {
                tblABBRegistration abbRegistration = aBBRegistrationRepository.GetSingle(x => x.CustMobile == customerMobileNumber
               && x.ModelNumberId == modelNumber
               && x.InvoiceDate == PurchaseDate
               && x.InvoiceNo == invoiceNumber);
                if (abbRegistration == null)
                    flag = true;
                else
                    flag = false;

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBRegManager", "ValidateDuplicateABBCall", ex);
            }

            return flag;
        }

        public bool ValidateDuplicateABBCall(ABBRegistrationViewModel ABBRegistrationVM)
        {
            aBBRegistrationRepository = new ABBRegistrationRepository();
            bool flag = false;
            try
            {
                tblABBRegistration abbRegistration = aBBRegistrationRepository.GetSingle(x =>
                x.BusinessUnitId == ABBRegistrationVM.BusinessUnitId
               && x.BusinessPartnerId == ABBRegistrationVM.BusinessPartnerId
               && x.CustFirstName == ABBRegistrationVM.CustFirstName
               && x.CustLastName == ABBRegistrationVM.CustLastName
               && x.CustMobile == ABBRegistrationVM.CustMobile
               && x.CustEmail == ABBRegistrationVM.CustEmail
               && x.CustAddress1 == ABBRegistrationVM.CustAddress1
               && x.CustPinCode == ABBRegistrationVM.CustPinCode
               && x.NewProductCategoryId == ABBRegistrationVM.NewProductCategoryId
               && x.NewProductCategoryTypeId == ABBRegistrationVM.NewProductCategoryTypeId
               && x.NewBrandId == ABBRegistrationVM.NewBrandId
               && x.ModelNumberId == ABBRegistrationVM.ModelNumberId
               && x.ABBPlanName == ABBRegistrationVM.ABBPlanName
               && x.InvoiceDate == ABBRegistrationVM.InvoiceDate
               && x.InvoiceNo == ABBRegistrationVM.InvoiceNo
               && x.NewPrice == ABBRegistrationVM.NewPrice
               && x.ABBPlanPeriod == ABBRegistrationVM.ABBPlanPeriod
               && x.NoOfClaimPeriod == ABBRegistrationVM.NoOfClaimPeriod
               && x.ProductNetPrice == ABBRegistrationVM.ProductNetPrice
               && x.IsActive == true
               && Convert.ToDateTime(x.CreatedDate).ToShortDateString() == Convert.ToDateTime(currentDatetime).ToShortDateString());
                if (abbRegistration == null)
                    flag = true;
                else
                    flag = false;

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBRegManager", "ValidateDuplicateABBCall", ex);
            }

            return flag;
        }
        #endregion


        #region Update ABB Reg Info in database
        /// <summary>
        /// Method to Update ABB Reg Info
        /// </summary>       
        /// <returns></returns>   
        public int UpdateABBReg(string zohoABBRegId, int ABBRegId, string regdNo)
        {
            aBBRegistrationRepository = new ABBRegistrationRepository();

            int result = 0;
            try
            {

                if (zohoABBRegId != null && ABBRegId > 0 && regdNo != null)
                {
                    tblABBRegistration tempABBRegInfo = aBBRegistrationRepository.GetSingle(x => x.ABBRegistrationId.Equals(ABBRegId));
                    if (tempABBRegInfo != null)
                    {
                        tempABBRegInfo.ZohoABBRegistrationId = zohoABBRegId;
                        //tempABBRegInfo.RegdNo = regdNo;
                        tempABBRegInfo.ModifiedDate = currentDatetime;

                        aBBRegistrationRepository.Update(tempABBRegInfo);

                        aBBRegistrationRepository.SaveChanges();
                        result = tempABBRegInfo.ABBRegistrationId;
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
        public tblABBRegistration SetABBRegObjectJson(ABBRegistrationViewModel ABBRegistrationVM)
        {
            tblABBRegistration ABBRegistrationObj = null;
            try
            {
                if (ABBRegistrationVM != null)
                {
                    ABBRegistrationObj = new tblABBRegistration();
                    ABBRegistrationObj.BusinessUnitId = ABBRegistrationVM.BusinessUnitId;
                    ABBRegistrationObj.StoreCode = ABBRegistrationVM.StoreCode;
                    ABBRegistrationObj.StoreName = ABBRegistrationVM.StoreName;
                    ABBRegistrationObj.BusinessPartnerId = ABBRegistrationVM.BusinessPartnerId;
                    ABBRegistrationObj.RegdNo = ABBRegistrationVM.RegdNo;
                    ABBRegistrationObj.SponsorOrderNo = ABBRegistrationVM.SponsorOrderNo;
                    ABBRegistrationObj.CustFirstName = ABBRegistrationVM.CustFirstName;
                    ABBRegistrationObj.CustLastName = ABBRegistrationVM.CustLastName;
                    ABBRegistrationObj.CustMobile = ABBRegistrationVM.CustMobile;
                    ABBRegistrationObj.CustEmail = ABBRegistrationVM.CustEmail;
                    ABBRegistrationObj.CustAddress1 = ABBRegistrationVM.CustAddress1;
                    ABBRegistrationObj.CustAddress2 = ABBRegistrationVM.CustAddress2;
                    ABBRegistrationObj.Location = ABBRegistrationVM.Customer_Location;
                    ABBRegistrationObj.CustPinCode = ABBRegistrationVM.CustPinCode;
                    ABBRegistrationObj.CustCity = ABBRegistrationVM.CustCity;
                    ABBRegistrationObj.CustState = ABBRegistrationVM.CustState;
                    ABBRegistrationObj.NewProductCategoryId = ABBRegistrationVM.NewProductCategoryId;
                    ABBRegistrationObj.NewProductCategoryTypeId = ABBRegistrationVM.NewProductCategoryTypeId;
                    ABBRegistrationObj.NewBrandId = ABBRegistrationVM.NewBrandId;
                    ABBRegistrationObj.NewSize = ABBRegistrationVM.NewSize;
                    ABBRegistrationObj.ProductSrNo = ABBRegistrationVM.ProductSrNo;
                    if (ABBRegistrationVM.ModelNumberId > 0)
                    {
                        ABBRegistrationObj.ModelNumberId = ABBRegistrationVM.ModelNumberId;
                    }
                    
                    
                    ABBRegistrationObj.ABBPlanName = ABBRegistrationVM.ABBPlanName;
                    ABBRegistrationObj.HSNCode = ABBRegistrationVM.HSNCode;
                    ABBRegistrationObj.InvoiceDate = ABBRegistrationVM.InvoiceDate;
                    ABBRegistrationObj.InvoiceNo = ABBRegistrationVM.InvoiceNo;
                    ABBRegistrationObj.NewPrice = ABBRegistrationVM.NewPrice;
                    ABBRegistrationObj.ABBFees = ABBRegistrationVM.ABBFees;
                    ABBRegistrationObj.OrderType = ABBRegistrationVM.OrderType;
                    ABBRegistrationObj.SponsorProdCode = ABBRegistrationVM.SponsorProdCode;
                    ABBRegistrationObj.ABBPriceId = ABBRegistrationVM.ABBPriceId;
                    ABBRegistrationObj.UploadDateTime = ABBRegistrationVM.UploadDateTime;
                    ABBRegistrationObj.YourRegistrationNo = ABBRegistrationVM.YourRegistrationNo;
                    ABBRegistrationObj.InvoiceImage = ABBRegistrationVM.InvoiceImage;
                    ABBRegistrationObj.ABBPlanPeriod = ABBRegistrationVM.ABBPlanPeriod;
                    ABBRegistrationObj.NoOfClaimPeriod = ABBRegistrationVM.NoOfClaimPeriod;
                    ABBRegistrationObj.ProductNetPrice = ABBRegistrationVM.ProductNetPrice;
                    ABBRegistrationObj.OtherCommunications = ABBRegistrationVM.OtherCommunications;
                    ABBRegistrationObj.ABBPlanName = ABBRegistrationVM.ABBPlanName;
                    ABBRegistrationObj.FollowupCommunication = ABBRegistrationVM.FollowupCommunication;
                    ABBRegistrationObj.IsActive = true;
                    ABBRegistrationObj.CreatedDate = DateTime.Now;
                    ABBRegistrationObj.ModifiedDate = DateTime.Now;
                    ABBRegistrationObj.PaymentStatus = false;
                    ABBRegistrationObj.AbbApprove = false;
                    ABBRegistrationObj.CustomerId = ABBRegistrationVM.CustomerDetailsId;
                    ABBRegistrationObj.StatusId = Convert.ToInt32(StatusEnum.PaymentUnsuccessfull);
                    ABBRegistrationObj.EmployeeId = ABBRegistrationVM.EmployeeId;
                    ABBRegistrationObj.BaseValue = Convert.ToDecimal(ABBRegistrationVM.BaseValue);
                    ABBRegistrationObj.Cgst = Convert.ToDecimal(ABBRegistrationVM.Cgst);
                    ABBRegistrationObj.Sgst = Convert.ToDecimal(ABBRegistrationVM.Sgst);
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBRegManager", "SetABBRegObjectJson", ex);
            }
            return ABBRegistrationObj;
        }
        #endregion

        #region Add ABB Reg Info in Zoho
        /// <summary>
        /// Method to add the ABB Reg Info
        /// </summary>       
        /// <returns></returns>   
        public ABBRegistrationFormResponseDataContract AddZohoABBReg(ABBRegistrationDataContract ABBRegistrationDC)
        {
            logging = new Logging();
            ABBRegistrationFormResponseDataContract ABBRegistrationResponseDC = null;
            ABBRegistrationFormRequestDataContract ABBRegistrationRequestDC = new ABBRegistrationFormRequestDataContract();
            try
            {
                if (ABBRegistrationDC != null)
                {
                    ABBRegistrationRequestDC.data = ABBRegistrationDC;

                    //IRestResponse response = ZohoServiceCalls.Rest_InvokeZohoInvoiceServiceForPlainText("https://creatorapp.zoho.com/api/v2/accountsperthsecurityservices/mobileapp/form/D_Runsheets", patrolRequestDC);
                    IRestResponse response = ZohoServiceCalls.Rest_InvokeZohoInvoiceServiceForPlainText(ZohoCreatorAPICallURL.GetURLFor(ZohoCreatorAPICallURL.AddDetails,
                                                                                   FormLinkNameConstant.ABBReg_form,
                                                                                    null
                                                                                       ), Method.POST, ABBRegistrationRequestDC);


                    if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK)
                    {
                        ABBRegistrationResponseDC = JsonConvert.DeserializeObject<ABBRegistrationFormResponseDataContract>(response.Content);
                        if (ABBRegistrationResponseDC.code != 3000)
                        {
                            logging.WriteErrorToDB("ABBRegManager", "AddZohoABBReg", ABBRegistrationDC.Sponsor_Order_No, response);
                        }
                    }
                    else
                    {
                        logging.WriteErrorToDB("ABBRegManager", "AddZohoABBReg", ABBRegistrationDC.Sponsor_Order_No, response);
                    }
                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBRegManager", "AddABBReg", ex);
            }

            return ABBRegistrationResponseDC;
        }
        #endregion

        #region set Zoho ABB Reg obj
        /// <summary>
        /// Method to set Zoho ABB Reg info 
        /// </summary>
        /// <param name="productOrderDataContract">productOrderDataContract</param>     
        public ABBRegistrationDataContract SetZohoABBRegObject(ABBRegistrationViewModel ABBRegistrationVM)
        {
            ABBRegistrationDataContract ABBRegistrationObj = null;
            SponserSubCategoryListDataContract SponserSubCategoryListDC = null;
            SponsorCategoryListDataContract SponserCategoryListDC = null;
            BrandMasterListDataContract brandMasterListDC = null;
            ProductSizeListDataContract ProductSizeListDC = null;
            StoreCodeListDataContract storeCodeListDC = null;
            sponserManager = new SponserManager();
            masterManager = new MasterManager();
            BusinessPartnerRepository businessPartnerRepository = new BusinessPartnerRepository();
            BrandRepository sponserRepository = new BrandRepository();
            ProductTypeRepository productTypeRepository = new ProductTypeRepository();
            ProductCategoryRepository categoryRepository = new ProductCategoryRepository();
            PriceMasterRepository priceMasterRepository = new PriceMasterRepository();

            try
            {
                if (ABBRegistrationVM != null)
                {
                    ABBRegistrationObj = new ABBRegistrationDataContract();

                    // Sponsor and Store code 
                    if (ABBRegistrationVM.BusinessPartnerId > 0)
                    {
                        tblBusinessPartner BPObj = businessPartnerRepository.GetSingle(x => x.IsActive == true && x.BusinessPartnerId.Equals(ABBRegistrationVM.BusinessPartnerId));
                        if (BPObj != null)
                        {
                            storeCodeListDC = masterManager.GetAllStoreCodeByCode(BPObj.StoreCode);//masterManager.GetAllStoreCode();
                            if (storeCodeListDC != null)
                            {
                                if (storeCodeListDC.data != null && storeCodeListDC.data.Count > 0)
                                {
                                    ABBRegistrationObj.Sponsor_Name = storeCodeListDC.data[0].Sponsor_Name.ID;
                                    ABBRegistrationObj.Store_Code = storeCodeListDC.data[0].ID;
                                }
                            }

                        }
                    }

                    if (ABBRegistrationVM.FollowupCommunication == true)
                    {
                        ABBRegistrationObj.Mar_Com = "Yes";
                    }

                    ABBRegistrationObj.Regd_No = ABBRegistrationVM.RegdNo;
                    ABBRegistrationObj.Sponsor_Order_No = ABBRegistrationVM.SponsorOrderNo;
                    ABBRegistrationObj.Cust_Name = new CustName();
                    ABBRegistrationObj.Cust_Name.first_name = ABBRegistrationVM.CustFirstName;
                    ABBRegistrationObj.Cust_Name.last_name = ABBRegistrationVM.CustLastName;
                    ABBRegistrationObj.Cust_Mobile = ABBRegistrationVM.CustMobile;
                    ABBRegistrationObj.Cust_E_mail = ABBRegistrationVM.CustEmail;
                    ABBRegistrationObj.Cust_Add_1 = ABBRegistrationVM.CustAddress1;
                    ABBRegistrationObj.Cust_Add_2 = ABBRegistrationVM.CustAddress2;
                    ABBRegistrationObj.Customer_Location = ABBRegistrationVM.Customer_Location;
                    ABBRegistrationObj.Cust_Pin_Code = ABBRegistrationVM.CustPinCode;
                    ABBRegistrationObj.Cust_City = ABBRegistrationVM.CustCity;
                    ABBRegistrationObj.Cust_State = ABBRegistrationVM.CustState;
                    ABBRegistrationObj.Is_ABB_Payment_Done = ABBRegistrationVM.Is_ABB_Payment_Done;
                    ABBRegistrationObj.Transaction_Id = ABBRegistrationVM.transactionId;
                    ABBRegistrationObj.Order_Id = ABBRegistrationVM.OrderId;
                    ABBRegistrationObj.Payment_Remark = ABBRegistrationVM.PaymentRemark;

                    // Product category & type
                    if (ABBRegistrationVM.NewProductCategoryTypeId != 0)
                    {
                        tblProductType productTypeObj = productTypeRepository.GetSingle(x => x.Id.Equals(ABBRegistrationVM.NewProductCategoryTypeId));
                        if (productTypeObj != null)
                        {
                            tblProductCategory productCatObj = categoryRepository.GetSingle(x => x.Id.Equals(productTypeObj.ProductCatId));
                            if (productCatObj != null)
                            {
                                // fill Product Category
                                SponserCategoryListDC = masterManager.GetAllCategory();
                                if (SponserCategoryListDC != null)
                                {
                                    if (SponserCategoryListDC.data != null && SponserCategoryListDC.data.Count > 0)
                                    {
                                        CategoryData CategoryData = SponserCategoryListDC.data.Find(x => x.Product_Technology.ToLower().Equals(productCatObj.Code.ToLower()));
                                        if (CategoryData != null)
                                        {
                                            ABBRegistrationObj.New_Prod_Group = CategoryData.ID;

                                        }
                                    }
                                }
                                // fill Product type
                                SponserSubCategoryListDC = masterManager.GetAllSubCategory();
                                if (SponserSubCategoryListDC != null)
                                {
                                    if (SponserSubCategoryListDC.data != null && SponserSubCategoryListDC.data.Count > 0)
                                    {
                                        string category = null;

                                        if (productTypeObj.Code != "RF2" && productTypeObj.Code != "RF3")
                                        {
                                            category = Regex.Replace(productTypeObj.Code, @"[\d]", string.Empty);
                                        }
                                        else
                                        {
                                            category = productTypeObj.Code;
                                        }

                                        SubCategoryData subCategoryData = SponserSubCategoryListDC.data.Find(x => x.Sub_Product_Technology.ToLower().Equals(category.ToLower()));
                                        if (subCategoryData != null)
                                        {
                                            ABBRegistrationObj.New_Prod_Type = subCategoryData.ID;
                                        }
                                    }
                                }

                                // fill Product size
                                ProductSizeListDC = masterManager.GetAllProductSize();
                                if (ProductSizeListDC != null)
                                {
                                    if (ProductSizeListDC.data != null && ProductSizeListDC.data.Count > 0)
                                    {
                                        string size = null;
                                        if (!String.IsNullOrEmpty(productTypeObj.Size))
                                        {
                                            size = Regex.Replace(productTypeObj.Code, "[^0-9.]", "");
                                            //size = Regex.Replace(productTypeObj.Code, @"[\d]", string.Empty);

                                            ProductSize productSize = ProductSizeListDC.data.Find(x => x.Size.ToLower().Equals(size.ToLower()));
                                            if (productSize != null)
                                            {
                                                ABBRegistrationObj.New_Size = productSize.ID;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // Brand
                    if (ABBRegistrationVM.NewBrandId > 0)
                    {
                        tblBrand brandObj = sponserRepository.GetSingle(x => x.Id.Equals(ABBRegistrationVM.NewBrandId));
                        if (brandObj != null)
                        {
                            brandMasterListDC = masterManager.GetAllBrand();
                            if (brandMasterListDC != null)
                            {
                                if (brandMasterListDC.data != null && brandMasterListDC.data.Count > 0)
                                {
                                    BrandMaster brandData = brandMasterListDC.data.Find(x => x.Brand_Name.ToLower().Equals(brandObj.Name.ToLower()));
                                    if (brandData != null)
                                    {
                                        ABBRegistrationObj.New_Brand = brandData.ID;
                                    }
                                }
                            }

                        }
                    }



                    ABBRegistrationObj.Prod_Sr_No = ABBRegistrationVM.ProductSrNo;
                    ABBRegistrationObj.Model_No = ABBRegistrationVM.ModelName;
                    ABBRegistrationObj.ABB_Plan_Name = ABBRegistrationVM.ABBPlanName;
                    ABBRegistrationObj.Sponsor_Prog_code = ABBRegistrationVM.ABBPlanName;
                    ABBRegistrationObj.HSN_Code_For_ABB_Fees = ABBRegistrationVM.HSNCode;

                    if (ABBRegistrationVM.InvoiceDate != null)
                    {
                        DateTime InvoiceDate = (DateTime)ABBRegistrationVM.InvoiceDate;
                        ABBRegistrationObj.Invoice_Date = InvoiceDate.ToString("dd-MMM-yyyy");
                    }
                    ABBRegistrationObj.Invoice_No = ABBRegistrationVM.InvoiceNo;
                    ABBRegistrationObj.New_Price = ABBRegistrationVM.NewPrice != null ? ABBRegistrationVM.NewPrice.ToString() : "";
                    ABBRegistrationObj.ABB_Fees = ABBRegistrationVM.ABBFees != null ? ABBRegistrationVM.ABBFees.ToString() : "";
                    ABBRegistrationObj.Order_Type = "ABB";
                    //ABBRegistrationObj.Order_Type = ABBRegistrationVM.OrderType;
                    ABBRegistrationObj.Sponsor_Prog_code = ABBRegistrationVM.SponsorProdCode;

                    ABBRegistrationObj.ABB_Price_Id = ABBRegistrationVM.ABBPriceId != null ? ABBRegistrationVM.ABBPriceId.ToString() : "";

                    if (ABBRegistrationVM.UploadDateTime != null)
                    {
                        DateTime UploadDateTime = (DateTime)ABBRegistrationVM.UploadDateTime;
                        ABBRegistrationObj.Upload_Date = UploadDateTime.ToString("dd-MMM-yyyy");
                    }

                    //ABBRegistrationObj.Invoice_Image = ABBRegistrationVM.InvoiceImage;
                    ABBRegistrationObj.ABB_Plan_Period_Months = ABBRegistrationVM.ABBPlanPeriod;
                    ABBRegistrationObj.No_Of_Claim_Period_Months = ABBRegistrationVM.NoOfClaimPeriod;
                    ABBRegistrationObj.Product_Net_Price_Incl_Of_GST = ABBRegistrationVM.ProductNetPrice != null ? ABBRegistrationVM.ProductNetPrice.ToString() : "";
                    if (ABBRegistrationVM.InvoiceImage != null)
                    {
                        string imageUrl = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Content/DB_Files/InvoiceImage/" + ABBRegistrationVM.InvoiceImage;
                        ABBRegistrationObj.Invoice_Image = imageUrl;
                    }

                    ABBRegistrationObj.Sponsor_Status = "Not Imported";
                    ABBRegistrationObj.Approve_this = "No";
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBRegManager", "SetABBRegObjectJson", ex);
            }
            return ABBRegistrationObj;
        }
        #endregion

        #region Get zoho ABB details  by ABBId
        /// <summary>
        /// Method to get zohoABB by ABBId
        /// </summary>
        /// <param></param>
        /// <returns>model</returns>
        public ABBSingleDataContract GetABBById(string ABBId)
        {
            ABBSingleDataContract ABBSingleDC = null;
            IRestResponse response = null;
            try
            {
                if (ABBId != null)
                {

                    response = ZohoServiceCalls.Rest_InvokeZohoInvoiceServiceForPlainText(ZohoCreatorAPICallURL.GetURLFor(ZohoCreatorAPICallURL.GetUrlWithFilter,
                                                                                    ReportLinkNameConstant.BSH_Stores_Abb_Table,
                                                                                     FilterConstant.ABB_Id_filter.Replace("[ABBID]", ABBId)
                                                                                        ), Method.GET, null);

                    if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK)
                    {
                        ABBSingleDC = JsonConvert.DeserializeObject<ABBSingleDataContract>(response.Content);

                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SponserManager", "GetEvcMasterById", ex);
            }
            return ABBSingleDC;
        }

        #endregion


        #region Get Abb Orders      
        /// <summary>
        /// Method to Get ABB orders which are not moved to ZOho
        /// </summary>       
        /// <returns></returns>   
        public string GetUnMovedZohoOrders()
        {
            aBBRegistrationRepository = new ABBRegistrationRepository();
            string result = null;
            try
            {
                List<tblABBRegistration> abbRegList = aBBRegistrationRepository.GetList(x => string.IsNullOrEmpty(x.ZohoABBRegistrationId)).ToList();
                if (abbRegList != null && abbRegList.Count > 0)
                {
                    foreach (tblABBRegistration item in abbRegList)
                    {

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

        #region Payment update in abb
        public string PaymentstatusUpdate(PaymentResponseModel paymentReponse)
        {
            businessPartnerRepository = new BusinessPartnerRepository();
            _businessUnitRepository = new BusinessUnitRepository();
            _brandsmartsellRepository = new BrandSmartSellRepository();
            _productCategoryRepository = new ProductCategoryRepository();
            _productTypeRepository = new ProductTypeRepository();
            _brandRepository = new BrandRepository();
            _modelNumberRepository = new ModelNumberRepository();
            aBBRegistrationRepository = new ABBRegistrationRepository();
            aBBPaymentRepository = new ABBPaymentRepository();
            string WhatssAppStatusEnum = string.Empty;
            string ResponseCode = string.Empty;
            string responseforWhatasapp = string.Empty;
            _whatsAppMessageRepository = new WhatsappMessageRepository();
            WhatasappResponse whatssappresponseDC = null;
            ABBRegManager ABBRegInfo = new ABBRegManager();
            tblPaymentLeaser planPaymentObj = new tblPaymentLeaser();
            SponserFormResponseDataContract zohoresponse = new SponserFormResponseDataContract();
            MailJetViewModel mailJet = new MailJetViewModel();
            MailJetMessage jetmessage = new MailJetMessage();
            MailJetFrom from = new MailJetFrom();
            MailjetTo to = new MailjetTo();
            string response = string.Empty;
            string ZohoPushFlag = string.Empty;
            string SuccessResponse = null;
            string OrderStatus = null;
            tblBrand brandObjE = new tblBrand();
            tblBrandSmartBuy brandObjBS = new tblBrandSmartBuy();
            string BrandForABB = null;

            try
            {
                if (paymentReponse != null)
                {
                    tblABBRegistration abbregistrationObj = aBBRegistrationRepository.GetSingle(x => x.RegdNo == paymentReponse.RegdNo);
                    if (abbregistrationObj != null)
                    {

                        SuccessResponse = SponsorsApiCall.ExchangeOrderManager.GetEnumDescription(PluralEnum.PaymentStatus);
                        OrderStatus = SponsorsApiCall.ExchangeOrderManager.GetEnumDescription(PluralEnum.OrderStatus);

                        if (paymentReponse.status == SuccessResponse && paymentReponse.orderStatus == OrderStatus)
                        {
                            abbregistrationObj.PaymentStatus = true;
                            abbregistrationObj.AbbApprove = true;
                            planPaymentObj.PaymentStatus = true;
                            abbregistrationObj.StatusId = Convert.ToInt32(StatusEnum.Paymentsuccessfull);
                            aBBRegistrationRepository.Update(abbregistrationObj);
                            aBBRegistrationRepository.SaveChanges();

                            #region Code to send Email to customer on abb approvel

                            tblBusinessUnit businessUnit = _businessUnitRepository.GetSingle(x => x.BusinessUnitId == abbregistrationObj.BusinessUnitId && x.IsActive == true);
                            if (businessUnit != null)
                            {
                                if (businessUnit.IsBUMultiBrand == true)
                                {
                                    brandObjBS = _brandsmartsellRepository.GetSingle(x => x.IsActive == true && x.Id == abbregistrationObj.NewBrandId);
                                    if (brandObjBS != null)
                                    {
                                        BrandForABB = brandObjBS.Name;
                                    }
                                }
                                else
                                {
                                    brandObjE = _brandRepository.GetSingle(x => x.Id == abbregistrationObj.NewBrandId && x.IsActive == true);
                                    if (brandObjE != null)
                                    {
                                        BrandForABB = brandObjE.Name;
                                    }
                                }



                                if (BrandForABB != null)
                                {
                                    tblProductCategory productCategoryObj = _productCategoryRepository.GetSingle(x => x.Id == abbregistrationObj.NewProductCategoryId && x.IsActive == true);
                                    if (productCategoryObj != null)
                                    {
                                        tblProductType productTypeObj = _productTypeRepository.GetSingle(x => x.Id == abbregistrationObj.NewProductCategoryTypeId && x.IsActive == true);
                                        if (productTypeObj != null)
                                        {
                                            #region Code to send mail to customer for exchange details
                                            TemplateDataContract templateDC = new TemplateDataContract();
                                            Task<bool> IsMailSent = null;
                                            _mailManager = new MailManager();
                                            bool? flag = false;
                                            
                                            #region Code to Generate ABB Invoice for Customer
                                            if (businessUnit.IsInvoiceAvailable == true)
                                            {
                                                List<tblConfiguration> tblConfigurationList = null;
                                                string FinancialYear = null;
                                                string BccList = null;
                                                int? customerFilesId = 0;
                                                ABBRegistrationViewModel aBBRegistrationViewModel = null;
                                                aBBRegistrationViewModel = GenericMapper<tblABBRegistration, ABBRegistrationViewModel>.MapObject(abbregistrationObj);

                                                #region Code for Get Data from TblConfiguration
                                                tblConfigurationList = GetConfigurationList();
                                                if (tblConfigurationList != null && tblConfigurationList.Count > 0)
                                                {
                                                    var financialYear = tblConfigurationList.Where(x => x.Name == ConfigurationEnum.FinancialYear.ToString()).FirstOrDefault();
                                                    if (financialYear != null && financialYear.Value != null)
                                                    {
                                                        FinancialYear = financialYear.Value.Trim();
                                                    }
                                                    var bccList = tblConfigurationList.Where(x => x.Name == ConfigurationEnum.ABB_Bcc.ToString()).FirstOrDefault();
                                                    if (bccList != null && bccList.Value != null)
                                                    {
                                                        BccList = bccList.Value.Trim();
                                                    }
                                                }
                                                #endregion

                                                #region Code for get Max InvSrNum from tblCustomerFiles
                                                int? MaxSrNum = GetAbbMaxInvSrNum(FinancialYear);
                                                if (MaxSrNum == null || MaxSrNum == 0)
                                                {
                                                    MaxSrNum = 1;
                                                }
                                                else
                                                {
                                                    MaxSrNum++;
                                                }
                                                #endregion

                                                #region Set Counter Sr. Number 
                                                FinancialYear = FinancialYear ?? "";
                                                string BillCounterNum = String.Format("{0:D6}", MaxSrNum);
                                                #endregion

                                                aBBRegistrationViewModel.FinancialYear = FinancialYear;
                                                aBBRegistrationViewModel.BillCounterNum = BillCounterNum;
                                                aBBRegistrationViewModel.BillNumber = "ABB-Inv-" + aBBRegistrationViewModel.FinancialYear + "-" + aBBRegistrationViewModel.BillCounterNum;
                                                templateDC.InvAttachFileName = "ABB-Inv-"+ aBBRegistrationViewModel.FinancialYear.Replace("/", "-") + "-" + aBBRegistrationViewModel.BillCounterNum + ".pdf";
                                                templateDC.InvAttachFilePath = ConfigurationManager.AppSettings["ABB_Invoice_Pdf"].ToString();
                                                bool IsInvoiceGenerated = GenerateInvoicePDF(aBBRegistrationViewModel, templateDC);
                                                if (IsInvoiceGenerated)
                                                {
                                                    templateDC.IsInvoiceGenerated = IsInvoiceGenerated;
                                                    templateDC.Cc = businessUnit.Email;
                                                    templateDC.Bcc = BccList;
                                                    //Save Invoice Details into DB
                                                    customerFilesId = ManageABBCustomerFiles(aBBRegistrationViewModel, templateDC);
                                                }
                                            }
                                            #endregion

                                            #region Code to Create Abb Welcome mail
                                            string TemplaTePath = ConfigurationManager.AppSettings["ABBGeneric"].ToString();
                                            string FilePath = TemplaTePath;
                                            StreamReader str = new StreamReader(FilePath);
                                            string MailText = str.ReadToEnd();
                                            str.Close();
                                            MailText = MailText.Replace("[RegdNo]", abbregistrationObj.RegdNo).Replace("[Brand]", BrandForABB).Replace("[Customer]", abbregistrationObj.CustFirstName + " " + abbregistrationObj.CustLastName)
                                                .Replace("[Email]", abbregistrationObj.CustEmail).Replace("[Address1]", abbregistrationObj.CustAddress1).Replace("[Address2]", abbregistrationObj.CustAddress2).Replace("[pincode]", abbregistrationObj.CustPinCode)
                                                .Replace("[city]", abbregistrationObj.CustCity).Replace("[productcategory]", productCategoryObj.Description).Replace("[ptoductType]", productTypeObj.Description).Replace("[product serial no]", abbregistrationObj.ProductSrNo).Replace("[invoicedate]", Convert.ToDateTime(abbregistrationObj.InvoiceDate).ToString("dd/MM/yyyy")).Replace("[invoiceNumber]", abbregistrationObj.InvoiceNo).Replace("[Netvalue]", abbregistrationObj.ProductNetPrice.ToString()).Replace("[PlanPeriod]", abbregistrationObj.ABBPlanPeriod).Replace("[NoClaimPEriod]", abbregistrationObj.NoOfClaimPeriod).Replace("[ABB PlanName]", abbregistrationObj.ABBPlanName)
                                                .Replace("[upload date]", Convert.ToDateTime(currentDatetime).ToString("dd/MM/yyyy"));
                                            #endregion

                                            #region JetMail mail Configuration 
                                            // VK
                                            templateDC.To = abbregistrationObj.CustEmail.Trim();
                                            templateDC.Subject = businessUnit.Name + ": Assured Buy Back Detail";
                                            templateDC.HtmlBody = MailText;
                                            IsMailSent = _mailManager.JetMailSendWithAttachment(templateDC);
                                            if (IsMailSent.Result)
                                            {
                                                flag = IsMailSent.Result;
                                            }
                                            #endregion

                                            #endregion

                                            #region code to send whatsappNotification For abb order confirmation
                                            ABBOrderConfirmation whatsappObj = new ABBOrderConfirmation();
                                            whatsappObj.userDetails = new UserDetails();
                                            whatsappObj.notification = new NotificationForABB();
                                            whatsappObj.notification.@params = new SendSmsABBWhatsapp();
                                            whatsappObj.userDetails.number = abbregistrationObj.CustMobile;
                                            whatsappObj.notification.sender = ConfigurationManager.AppSettings["Yello.aiSenderNumber"].ToString();
                                            whatsappObj.notification.type = ConfigurationManager.AppSettings["Yellow.aiMesssaheType"].ToString();
                                            whatsappObj.notification.templateId = NotificationConstants.abb_order_confirmation;
                                            whatsappObj.notification.@params.RegdNo = abbregistrationObj.RegdNo.ToString();
                                            string url = ConfigurationManager.AppSettings["Yellow.AiUrl"].ToString();
                                            IRestResponse responseNew = WhatsappNotificationManager.Rest_InvokeWhatsappserviceCall(url, Method.POST, whatsappObj);
                                            ResponseCode = responseNew.StatusCode.ToString();
                                            WhatssAppStatusEnum = SponsorsApiCall.ExchangeOrderManager.GetEnumDescription(WhatssAppEnum.SuccessCode);
                                            if (ResponseCode == WhatssAppStatusEnum)
                                            {
                                                responseforWhatasapp = responseNew.Content;
                                                if (responseforWhatasapp != null)
                                                {
                                                    whatssappresponseDC = JsonConvert.DeserializeObject<WhatasappResponse>(responseforWhatasapp);
                                                    tblWhatsAppMessage whatsapObj = new tblWhatsAppMessage();
                                                    whatsapObj.TemplateName = NotificationConstants.Test_Template;
                                                    whatsapObj.IsActive = true;
                                                    whatsapObj.PhoneNumber = abbregistrationObj.CustMobile;
                                                    whatsapObj.SendDate = DateTime.Now;
                                                    whatsapObj.msgId = whatssappresponseDC.msgId;
                                                    _whatsAppMessageRepository.Add(whatsapObj);
                                                    _whatsAppMessageRepository.SaveChanges();
                                                }
                                                else
                                                {
                                                    string ExchOrderObj = JsonConvert.SerializeObject(abbregistrationObj);
                                                    logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", abbregistrationObj.RegdNo, ExchOrderObj);
                                                }
                                            }
                                            else
                                            {
                                                string ExchOrderObj = JsonConvert.SerializeObject(abbregistrationObj);
                                                logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", abbregistrationObj.RegdNo, ExchOrderObj);
                                            }
                                            #endregion

                                        }
                                    }

                                }

                            }
                            #endregion
                        }
                        else
                        {
                            abbregistrationObj.PaymentStatus = false;
                            abbregistrationObj.StatusId = Convert.ToInt32(StatusEnum.PaymentFailed);
                            planPaymentObj.PaymentStatus = false;
                            aBBRegistrationRepository.Update(abbregistrationObj);
                            aBBRegistrationRepository.SaveChanges();

                            #region code to send whatsappNotification For abb order confirmation
                            ABBOrderConfirmation whatsappObj = new ABBOrderConfirmation();
                            whatsappObj.userDetails = new UserDetails();
                            whatsappObj.notification = new NotificationForABB();
                            whatsappObj.notification.@params = new SendSmsABBWhatsapp();
                            whatsappObj.userDetails.number = abbregistrationObj.CustMobile;
                            whatsappObj.notification.sender = ConfigurationManager.AppSettings["Yello.aiSenderNumber"].ToString();
                            whatsappObj.notification.type = ConfigurationManager.AppSettings["Yellow.aiMesssaheType"].ToString();
                            whatsappObj.notification.templateId = NotificationConstants.abb_order_confirmation_old;
                            whatsappObj.notification.@params.RegdNo = abbregistrationObj.RegdNo.ToString();
                            string url = ConfigurationManager.AppSettings["Yellow.AiUrl"].ToString();
                            IRestResponse responseNew = WhatsappNotificationManager.Rest_InvokeWhatsappserviceCall(url, Method.POST, whatsappObj);
                            ResponseCode = responseNew.StatusCode.ToString();
                            WhatssAppStatusEnum = SponsorsApiCall.ExchangeOrderManager.GetEnumDescription(WhatssAppEnum.SuccessCode);
                            if (ResponseCode == WhatssAppStatusEnum)
                            {
                                responseforWhatasapp = responseNew.Content;
                                if (responseforWhatasapp != null)
                                {
                                    whatssappresponseDC = JsonConvert.DeserializeObject<WhatasappResponse>(responseforWhatasapp);
                                    tblWhatsAppMessage whatsapObj = new tblWhatsAppMessage();
                                    whatsapObj.TemplateName = NotificationConstants.Test_Template;
                                    whatsapObj.IsActive = true;
                                    whatsapObj.PhoneNumber = abbregistrationObj.CustMobile;
                                    whatsapObj.SendDate = DateTime.Now;
                                    whatsapObj.msgId = whatssappresponseDC.msgId;
                                    _whatsAppMessageRepository.Add(whatsapObj);
                                    _whatsAppMessageRepository.SaveChanges();
                                }
                                else
                                {
                                    string ExchOrderObj = JsonConvert.SerializeObject(abbregistrationObj);
                                    logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", abbregistrationObj.RegdNo, ExchOrderObj);
                                }
                            }
                            else
                            {
                                string ExchOrderObj = JsonConvert.SerializeObject(abbregistrationObj);
                                logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", abbregistrationObj.RegdNo, ExchOrderObj);
                            }
                            #endregion
                        }
                        planPaymentObj.RegdNo = abbregistrationObj.RegdNo;
                        planPaymentObj.OrderId = paymentReponse.OrderId;
                        if (paymentReponse.pgTransTime != null)
                        {
                            DateTime dt = Convert.ToDateTime(paymentReponse.pgTransTime);
                            planPaymentObj.PaymentDate = dt;
                        }
                        else
                        {
                            planPaymentObj.PaymentDate = DateTime.Now;
                        }

                        planPaymentObj.IsActive = true;
                        planPaymentObj.transactionId = paymentReponse.transactionId;
                        planPaymentObj.ResponseDescription = paymentReponse.responseDescription;
                        planPaymentObj.ResponseCode = paymentReponse.responseCode.ToString();
                        planPaymentObj.CardId = paymentReponse.cardId;
                        planPaymentObj.CardHashId = paymentReponse.cardhashId;
                        planPaymentObj.CardScheme = paymentReponse.cardScheme;
                        planPaymentObj.CardToken = paymentReponse.cardToken;
                        planPaymentObj.Bank = paymentReponse.bank;
                        planPaymentObj.BankId = paymentReponse.bankid;
                        planPaymentObj.amount = paymentReponse.amount;
                        planPaymentObj.CheckSum = paymentReponse.checksum;
                        planPaymentObj.PaymentMode = paymentReponse.paymentMode;
                        planPaymentObj.ModuleType = "ABB";
                        planPaymentObj.ModuleReferenceId = abbregistrationObj.ABBRegistrationId;
                        planPaymentObj.TransactionType = "Cr";
                        planPaymentObj.CreatedDate = DateTime.Now;
                        planPaymentObj.GatewayTransactioId = paymentReponse.gatewayTransactionId;
                        planPaymentObj.CreatedBy = abbregistrationObj.ABBRegistrationId;
                        planPaymentObj.OrderStatus = paymentReponse.orderStatus;
                        if (paymentReponse.status == SuccessResponse)
                        {
                            planPaymentObj.PaymentStatus = true;
                        }
                        else
                        {
                            planPaymentObj.PaymentStatus = false;
                        }

                        aBBPaymentRepository.Add(planPaymentObj);
                        aBBPaymentRepository.SaveChanges();

                        response = "success";
                    }
                    else
                    {
                        planPaymentObj.RegdNo = paymentReponse.RegdNo;
                        planPaymentObj.OrderId = paymentReponse.OrderId;
                        planPaymentObj.PaymentDate = DateTime.Now;
                        planPaymentObj.IsActive = true;
                        if (paymentReponse.status == SuccessResponse)
                        {
                            planPaymentObj.PaymentStatus = true;
                        }
                        else
                        {
                            planPaymentObj.PaymentStatus = false;
                        }
                        planPaymentObj.transactionId = paymentReponse.transactionId;
                        planPaymentObj.ResponseDescription = paymentReponse.responseDescription;
                        planPaymentObj.ResponseCode = paymentReponse.responseCode.ToString();
                        planPaymentObj.CardId = paymentReponse.cardId;
                        planPaymentObj.CardHashId = paymentReponse.cardhashId;
                        planPaymentObj.CardScheme = paymentReponse.cardScheme;
                        planPaymentObj.CardToken = paymentReponse.cardToken;
                        planPaymentObj.Bank = paymentReponse.bank;
                        planPaymentObj.BankId = paymentReponse.bankid;
                        planPaymentObj.amount = paymentReponse.amount;
                        planPaymentObj.CheckSum = paymentReponse.checksum;
                        planPaymentObj.PaymentMode = paymentReponse.paymentMode;
                        planPaymentObj.OrderStatus = paymentReponse.orderStatus;
                        planPaymentObj.GatewayTransactioId = paymentReponse.gatewayTransactionId;
                        aBBPaymentRepository.Add(planPaymentObj);
                        aBBPaymentRepository.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBRegManager", "PaymentstatusUpdate", ex);
            }
            return response;
        }

        public pluralgatewayKey getPaymentKey()
        {
            pluralgatewayKey pluralkey = new pluralgatewayKey();
            pluralkey.merchantId = ConfigurationManager.AppSettings["MerchantIdPlural"].ToString();
            pluralkey.secretKey = ConfigurationManager.AppSettings["SecretKeyPlural"].ToString();
            pluralkey.accessCode = ConfigurationManager.AppSettings["AccessCodePlural"].ToString();
            return pluralkey;
        }
        #endregion


        #region create order service manager
        public IRestResponse createOrderManager(PluralCreateOrder pluralCreateOrder, string SecretKey)
        {
            CreateOrderResponse orderResponse = new CreateOrderResponse();
            CreateOrderEncryption orderEncryption = new CreateOrderEncryption();
            IRestResponse Response = null;
            string URl = null;
            logging = new Logging();
            try
            {
                URl = ConfigurationManager.AppSettings["CreateOrderUrl"].ToString();
                string Jsonrequest = JsonConvert.SerializeObject(pluralCreateOrder);
                byte[] jsonbytes = Encoding.UTF8.GetBytes(Jsonrequest);
                string newbase64 = Convert.ToBase64String(jsonbytes);
                string hashset = ChecksumCalculator.GetSHAGenerated(newbase64, SecretKey);
                orderEncryption.request = newbase64;
                Response = PluralServicecall.Rest_InvokePluralServiceCall(URl, Method.POST, hashset, orderEncryption);
                string responsecode = Response.StatusCode.ToString();
                if (responsecode == "OK")
                {
                    string response = Response.Content;
                    orderResponse = JsonConvert.DeserializeObject<CreateOrderResponse>(response);
                }
                else
                {
                    logging.WriteAPIRequestToDB("ABBRegManager", "createOrderManager", pluralCreateOrder.merchant_data.merchant_order_id, Response.Content);
                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBRegManager", "createOrderManager", ex);
            }
            return Response;
        }
        #endregion

        #region Get Order status
        public GetpaymentStatusResponse GetpaymentStatus(string PaymentId, string OrderId)
        {
            GetpaymentStatusResponse getsorderResponse = new GetpaymentStatusResponse();
            string Url = null;
            IRestResponse Response = null;
            string MerchantId = null;
            string AccessCode = null;
            logging = new Logging();
            try
            {
                Url = ConfigurationManager.AppSettings["GetOrderStatus"].ToString() + "/order/" + OrderId + "/payment/" + PaymentId;
                if (PaymentId != null && OrderId != null)
                {
                    MerchantId = ConfigurationManager.AppSettings["MerchantIdPlural"].ToString();
                    AccessCode = ConfigurationManager.AppSettings["AccessCodePlural"].ToString();
                    string Encoded = MerchantId + ":" + AccessCode;
                    byte[] encrypted = Encoding.UTF8.GetBytes(Encoded);
                    string result = PluralServicecall.MakeRequest(Url, MerchantId, AccessCode).ToString();
                    Encoded = Convert.ToBase64String(encrypted);
                    Encoded = "Basic " + Encoded;
                    Response = PluralServicecall.Rest_InvokePluralServiceCallGetPaymentStatus(Url, Method.GET, Encoded);
                    string responsecode = Response.StatusCode.ToString();
                    if (responsecode == "OK")
                    {
                        logging.WriteAPIRequestToDB("ABBRegManager", "GetpaymentStatus", OrderId, Response.Content);
                        getsorderResponse = JsonConvert.DeserializeObject<GetpaymentStatusResponse>(Response.Content);
                    }
                    else
                    {
                        logging.WriteAPIRequestToDB("ABBRegManager", "GetpaymentStatus", OrderId, Response.Content);
                    }
                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBRegManager", "GetpaymentStatus", ex);
            }
            return getsorderResponse;
        }
        #endregion

        #region Get Plan Details for abb 
        public ABBPlanTransactionResponse SaveABBPlanDetails(ABBRegistrationViewModel abbviewModel)
        {
            _abbPlanMasterRepository = new ABBPlanMasterRepository();
            _transactionRepository = new TransactionABBPlanMasterRepository();
            List<tblABBPlanMaster> planMasterObj = null;
            ABBPlanTransactionResponse abbtransactionReaponseObj = new ABBPlanTransactionResponse();
            try
            {
                planMasterObj = _abbPlanMasterRepository.GetList(x => x.BusinessUnitId == abbviewModel.BusinessUnitId && x.ProductTypeId == abbviewModel.NewProductCategoryTypeId && x.IsActive == true && x.ProductCatId == abbviewModel.NewProductCategoryId).ToList();
                if (planMasterObj != null && planMasterObj.Count > 0)
                {
                    foreach (var item in planMasterObj)
                    {
                        tblTransMasterABBPlanMaster tblTransABBplanMasterObj = new tblTransMasterABBPlanMaster();
                        tblTransABBplanMasterObj.BusinessUnitId = item.BusinessUnitId;
                        tblTransABBplanMasterObj.ABBRegistrationId = abbviewModel.ABBRegistrationId;
                        tblTransABBplanMasterObj.Assured_BuyBack_Percentage = item.Assured_BuyBack_Percentage;
                        tblTransABBplanMasterObj.From_Month = item.From_Month;
                        tblTransABBplanMasterObj.To_Month = item.To_Month;
                        tblTransABBplanMasterObj.Sponsor = item.Sponsor;
                        tblTransABBplanMasterObj.ProductTypeId = item.ProductTypeId;
                        tblTransABBplanMasterObj.IsActive = item.IsActive;
                        tblTransABBplanMasterObj.CreatedBy = Convert.ToInt32(UserEnum.Admin);
                        tblTransABBplanMasterObj.CreatedDate = DateTime.Now;
                        _transactionRepository.Add(tblTransABBplanMasterObj);
                    }
                    abbtransactionReaponseObj.TransactionId = _transactionRepository.SaveChanges();
                    abbtransactionReaponseObj.Message = "Transaction Successfull ";
                    abbtransactionReaponseObj.Response = "success";
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBRegManager", "GetABBPlanDetails", ex);
            }
            return abbtransactionReaponseObj;
        }
        #endregion

        #region set custommer details obj
        public tblCustomerDetail SetcustomerDetailsObj(ABBRegistrationViewModel abbviewModel)
        {
            _customerDetailsrepository = new CustomerDetailsRepository();
            tblCustomerDetail customerDetailsObj = new tblCustomerDetail();

            try
            {
                customerDetailsObj.FirstName = abbviewModel.CustFirstName;
                customerDetailsObj.LastName = abbviewModel.CustLastName;
                customerDetailsObj.Email = abbviewModel.CustEmail;
                customerDetailsObj.City = abbviewModel.CustCity;
                customerDetailsObj.ZipCode = abbviewModel.CustPinCode;
                customerDetailsObj.Address1 = abbviewModel.CustAddress1;
                if (abbviewModel.CustAddress2 == null)
                {
                    abbviewModel.CustAddress2 = abbviewModel.CustAddress1;
                    customerDetailsObj.Address2 = abbviewModel.CustAddress2;
                }
                else
                {
                    customerDetailsObj.Address2 = abbviewModel.CustAddress2;
                }
                customerDetailsObj.PhoneNumber = abbviewModel.CustMobile;
                customerDetailsObj.State = abbviewModel.CustState;
                customerDetailsObj.IsActive = true;
                customerDetailsObj.CreatedBy = Convert.ToInt32(UserEnum.Admin);
                customerDetailsObj.CreatedDate = DateTime.Now;
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBRegManager", "SetcustomerDetailsObj", ex);
            }
            return customerDetailsObj;
        }
        #endregion

        #region set custommer details obj
        public tblCustomerDetail SetcustomerDetailsObjAPI(ABBOrderRequestDataContract abbviewModel)
        {
            _customerDetailsrepository = new CustomerDetailsRepository();
            tblCustomerDetail customerDetailsObj = new tblCustomerDetail();

            try
            {
                customerDetailsObj.FirstName = abbviewModel.CustFirstName;
                customerDetailsObj.LastName = abbviewModel.CustLastName;
                customerDetailsObj.Email = abbviewModel.CustEmail;
                customerDetailsObj.City = abbviewModel.CustCity;
                customerDetailsObj.ZipCode = abbviewModel.CustPinCode;
                customerDetailsObj.Address1 = abbviewModel.CustAddress1;
                if (abbviewModel.CustAddress2 == null)
                {
                    abbviewModel.CustAddress2 = abbviewModel.CustAddress1;
                    customerDetailsObj.Address2 = abbviewModel.CustAddress2;
                }
                else
                {
                    customerDetailsObj.Address2 = abbviewModel.CustAddress2;
                }
                customerDetailsObj.PhoneNumber = abbviewModel.CustMobile;
                customerDetailsObj.State = abbviewModel.CustState;
                customerDetailsObj.IsActive = true;
                customerDetailsObj.CreatedBy = Convert.ToInt32(UserEnum.Admin);
                customerDetailsObj.CreatedDate = DateTime.Now;
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBRegManager", "SetcustomerDetailsObj", ex);
            }
            return customerDetailsObj;
        }
        #endregion
        #region Get Plan Details for abb 
        public ABBPlanTransactionResponse GetABBPlanDetailsAPI(ABBProductOrderDataContract abbviewModel)
        {
            _abbPlanMasterRepository = new ABBPlanMasterRepository();
            _transactionRepository = new TransactionABBPlanMasterRepository();
            List<tblABBPlanMaster> planMasterObj = null;
            ABBPlanTransactionResponse abbtransactionReaponseObj = new ABBPlanTransactionResponse();
            try
            {
                planMasterObj = _abbPlanMasterRepository.GetList(x => x.BusinessUnitId == abbviewModel.BusinessUnitId && x.ProductTypeId == abbviewModel.NewProductCategoryTypeId && x.IsActive == true && x.ProductCatId == abbviewModel.NewProductCategoryId).ToList();
                if (planMasterObj != null && planMasterObj.Count > 0)
                {
                    foreach (var item in planMasterObj)
                    {
                        tblTransMasterABBPlanMaster tblTransABBplanMasterObj = new tblTransMasterABBPlanMaster();
                        tblTransABBplanMasterObj.BusinessUnitId = item.BusinessUnitId;
                        tblTransABBplanMasterObj.ABBRegistrationId = abbviewModel.ABBRegistrationId;
                        tblTransABBplanMasterObj.Assured_BuyBack_Percentage = item.Assured_BuyBack_Percentage;
                        tblTransABBplanMasterObj.From_Month = item.From_Month;
                        tblTransABBplanMasterObj.To_Month = item.To_Month;
                        tblTransABBplanMasterObj.Sponsor = item.Sponsor;
                        tblTransABBplanMasterObj.ProductTypeId = item.ProductTypeId;
                        tblTransABBplanMasterObj.IsActive = item.IsActive;
                        tblTransABBplanMasterObj.CreatedBy = Convert.ToInt32(UserEnum.Admin);
                        tblTransABBplanMasterObj.CreatedDate = DateTime.Now;
                        _transactionRepository.Add(tblTransABBplanMasterObj);
                    }
                    abbtransactionReaponseObj.TransactionId = _transactionRepository.SaveChanges();
                    abbtransactionReaponseObj.Message = "Transaction Successfull ";
                    abbtransactionReaponseObj.Response = "success";
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBRegManager", "GetABBPlanDetails", ex);
            }
            return abbtransactionReaponseObj;
        }
        #endregion

        #region Payment Status Capture for Zaak pay
        public string PaymentstatusUpdateZaakPay(PaymentResponseModel paymentReponse)
        {
            businessPartnerRepository = new BusinessPartnerRepository();
            _businessUnitRepository = new BusinessUnitRepository();
            _productCategoryRepository = new ProductCategoryRepository();
            _productTypeRepository = new ProductTypeRepository();
            _brandRepository = new BrandRepository();
            _brandsmartsellRepository = new BrandSmartSellRepository();
            _modelNumberRepository = new ModelNumberRepository();
            aBBRegistrationRepository = new ABBRegistrationRepository();
            aBBPaymentRepository = new ABBPaymentRepository();
            ABBRegManager ABBRegInfo = new ABBRegManager();
            tblPaymentLeaser planPaymentObj = new tblPaymentLeaser();
            SponserFormResponseDataContract zohoresponse = new SponserFormResponseDataContract();
            MailJetViewModel mailJet = new MailJetViewModel();
            MailJetMessage jetmessage = new MailJetMessage();
            MailJetFrom from = new MailJetFrom();
            MailjetTo to = new MailjetTo();
            string response = string.Empty;
            string ZohoPushFlag = string.Empty;
            string WhatssAppStatusEnum = string.Empty;
            string ResponseCode = string.Empty;
            string responseforWhatasapp = string.Empty;
            _whatsAppMessageRepository = new WhatsappMessageRepository();
            WhatasappResponse whatssappresponseDC = null;
            tblBrand brandObjE = new tblBrand();
            tblBrandSmartBuy brandObjBS = new tblBrandSmartBuy();
            string BrandForABB = null;
            try
            {
                if (paymentReponse != null)
                {
                    tblABBRegistration abbregistrationObj = aBBRegistrationRepository.GetSingle(x => x.RegdNo == paymentReponse.RegdNo && x.IsActive == true);
                    if (abbregistrationObj != null)
                    {
                        if (paymentReponse.responseCode == Convert.ToInt32(ZaakPayPaymentStatus.successfull))
                        {
                            abbregistrationObj.PaymentStatus = true;
                            abbregistrationObj.AbbApprove = true;
                            planPaymentObj.PaymentStatus = true;
                            abbregistrationObj.StatusId = Convert.ToInt32(StatusEnum.Paymentsuccessfull);
                            aBBRegistrationRepository.Update(abbregistrationObj);
                            aBBRegistrationRepository.SaveChanges();
                            #region Code to send Email to customer on abb approvel

                            tblBusinessUnit businessUnit = _businessUnitRepository.GetSingle(x => x.BusinessUnitId == abbregistrationObj.BusinessUnitId && x.IsActive == true);
                            if (businessUnit != null)
                            {
                                if (businessUnit.IsBUMultiBrand == true)
                                {
                                    brandObjBS = _brandsmartsellRepository.GetSingle(x => x.IsActive == true && x.Id == abbregistrationObj.NewBrandId);
                                    if (brandObjBS != null)
                                    {
                                        BrandForABB = brandObjBS.Name;
                                    }
                                }
                                else
                                {
                                    brandObjE = _brandRepository.GetSingle(x => x.Id == abbregistrationObj.NewBrandId && x.IsActive == true);
                                    if (brandObjE != null)
                                    {
                                        BrandForABB = brandObjE.Name;
                                    }
                                }

                                if (BrandForABB != null)
                                {
                                    tblProductCategory productCategoryObj = _productCategoryRepository.GetSingle(x => x.Id == abbregistrationObj.NewProductCategoryId && x.IsActive == true);
                                    if (productCategoryObj != null)
                                    {
                                        tblProductType productTypeObj = _productTypeRepository.GetSingle(x => x.Id == abbregistrationObj.NewProductCategoryTypeId && x.IsActive == true);
                                        if (productTypeObj != null)
                                        {
                                            #region Code to send mail to customer for exchange details
                                            TemplateDataContract templateDC = new TemplateDataContract();
                                            Task<bool> IsMailSent = null;
                                            _mailManager = new MailManager();
                                           

                                            #region Code to Generate ABB Invoice for Customer
                                            if (businessUnit.IsInvoiceAvailable == true)
                                            {
                                                List<tblConfiguration> tblConfigurationList = null;
                                                string FinancialYear = null;
                                                string BccList = null;
                                                int? customerFilesId = 0;
                                                ABBRegistrationViewModel aBBRegistrationViewModel = null;
                                                aBBRegistrationViewModel = GenericMapper<tblABBRegistration, ABBRegistrationViewModel>.MapObject(abbregistrationObj);

                                                #region Code for Get Data from TblConfiguration
                                                tblConfigurationList = GetConfigurationList();
                                                if (tblConfigurationList != null && tblConfigurationList.Count > 0)
                                                {
                                                    var financialYear = tblConfigurationList.Where(x => x.Name == ConfigurationEnum.FinancialYear.ToString()).FirstOrDefault();
                                                    if (financialYear != null && financialYear.Value != null)
                                                    {
                                                        FinancialYear = financialYear.Value.Trim();
                                                    }
                                                    var bccList = tblConfigurationList.Where(x => x.Name == ConfigurationEnum.ABB_Bcc.ToString()).FirstOrDefault();
                                                    if (bccList != null && bccList.Value != null)
                                                    {
                                                        BccList = bccList.Value.Trim();
                                                    }
                                                }
                                                #endregion

                                                #region Code for get Max InvSrNum from tblCustomerFiles
                                                int? MaxSrNum = GetAbbMaxInvSrNum(FinancialYear);
                                                if (MaxSrNum == null || MaxSrNum == 0)
                                                {
                                                    MaxSrNum = 1;
                                                }
                                                else
                                                {
                                                    MaxSrNum++;
                                                }
                                                #endregion

                                                #region Set Counter Sr. Number 
                                                FinancialYear = FinancialYear ?? "";
                                                string BillCounterNum = String.Format("{0:D6}", MaxSrNum);
                                                #endregion

                                                aBBRegistrationViewModel.FinancialYear = FinancialYear;
                                                aBBRegistrationViewModel.BillCounterNum = BillCounterNum;
                                                aBBRegistrationViewModel.BillNumber = "ABB-Inv-" + aBBRegistrationViewModel.FinancialYear + "-" + aBBRegistrationViewModel.BillCounterNum;
                                                templateDC.InvAttachFileName = "ABB-Inv-" + aBBRegistrationViewModel.FinancialYear.Replace("/", "-") + "-" + aBBRegistrationViewModel.BillCounterNum + ".pdf";
                                                templateDC.InvAttachFilePath = ConfigurationManager.AppSettings["ABB_Invoice_Pdf"].ToString();
                                                bool IsInvoiceGenerated = GenerateInvoicePDF(aBBRegistrationViewModel, templateDC);
                                                if (IsInvoiceGenerated)
                                                {
                                                    templateDC.IsInvoiceGenerated = IsInvoiceGenerated;
                                                    templateDC.Cc = businessUnit.Email;
                                                    templateDC.Bcc = BccList;
                                                    //Save Invoice Details into DB
                                                    customerFilesId = ManageABBCustomerFiles(aBBRegistrationViewModel, templateDC);
                                                }
                                            }
                                            #endregion

                                            #region Code to Create Abb Welcome mail
                                            string TemplaTePath = ConfigurationManager.AppSettings["ABBGeneric"].ToString();
                                            string FilePath = TemplaTePath;
                                            StreamReader str = new StreamReader(FilePath);
                                            string MailText = str.ReadToEnd();
                                            str.Close();
                                            MailText = MailText.Replace("[RegdNo]", abbregistrationObj.RegdNo).Replace("[Brand]", BrandForABB).Replace("[Customer]", abbregistrationObj.CustFirstName + " " + abbregistrationObj.CustLastName)
                                                .Replace("[Email]", abbregistrationObj.CustEmail).Replace("[Address1]", abbregistrationObj.CustAddress1).Replace("[Address2]", abbregistrationObj.CustAddress2).Replace("[pincode]", abbregistrationObj.CustPinCode)
                                                .Replace("[city]", abbregistrationObj.CustCity).Replace("[productcategory]", productCategoryObj.Description).Replace("[ptoductType]", productTypeObj.Description).Replace("[product serial no]", abbregistrationObj.ProductSrNo).Replace("[invoicedate]", Convert.ToDateTime(abbregistrationObj.InvoiceDate).ToString("dd/MM/yyyy")).Replace("[invoiceNumber]", abbregistrationObj.InvoiceNo).Replace("[Netvalue]", abbregistrationObj.ProductNetPrice.ToString()).Replace("[PlanPeriod]", abbregistrationObj.ABBPlanPeriod).Replace("[NoClaimPEriod]", abbregistrationObj.NoOfClaimPeriod).Replace("[ABB PlanName]", abbregistrationObj.ABBPlanName)
                                                .Replace("[upload date]", Convert.ToDateTime(currentDatetime).ToString("dd/MM/yyyy"));
                                            #endregion

                                            #region JetMail mail Configuration 
                                            // VK
                                            templateDC.To = abbregistrationObj.CustEmail.Trim();
                                            templateDC.Subject = businessUnit.Name + ": Assured Buy Back Detail";
                                            templateDC.HtmlBody = MailText;
                                            IsMailSent = _mailManager.JetMailSendWithAttachment(templateDC);
                                            #endregion

                                            #endregion
                                        }
                                    }
                                }
                            }


                            #endregion
                        }
                        else
                        {
                            abbregistrationObj.PaymentStatus = false;
                            abbregistrationObj.StatusId = Convert.ToInt32(StatusEnum.PaymentFailed);
                            planPaymentObj.PaymentStatus = false;
                            aBBRegistrationRepository.Update(abbregistrationObj);
                            aBBRegistrationRepository.SaveChanges();
                        }
                        planPaymentObj.RegdNo = abbregistrationObj.RegdNo;
                        planPaymentObj.OrderId = paymentReponse.OrderId;
                        planPaymentObj.PaymentDate = DateTime.Now;
                        planPaymentObj.IsActive = true;
                        planPaymentObj.transactionId = paymentReponse.transactionId;
                        planPaymentObj.ResponseDescription = paymentReponse.responseDescription;
                        planPaymentObj.ResponseCode = paymentReponse.responseCode.ToString();
                        planPaymentObj.CardId = paymentReponse.cardId;
                        planPaymentObj.CardHashId = paymentReponse.cardhashId;
                        planPaymentObj.CardScheme = paymentReponse.cardScheme;
                        planPaymentObj.CardToken = paymentReponse.cardToken;
                        planPaymentObj.Bank = paymentReponse.bank;
                        planPaymentObj.BankId = paymentReponse.bankid;
                        planPaymentObj.amount = paymentReponse.amount;
                        planPaymentObj.CheckSum = paymentReponse.checksum;
                        planPaymentObj.PaymentMode = paymentReponse.paymentMode;
                        planPaymentObj.ModuleType = "ABB";
                        planPaymentObj.ModuleReferenceId = abbregistrationObj.ABBRegistrationId;
                        planPaymentObj.TransactionType = "Cr";
                        planPaymentObj.CreatedDate = DateTime.Now;
                        planPaymentObj.CreatedBy = abbregistrationObj.ABBRegistrationId;
                        if (paymentReponse.responseCode == 100)
                        {
                            planPaymentObj.PaymentStatus = true;
                        }
                        else
                        {
                            planPaymentObj.PaymentStatus = false;
                        }

                        aBBPaymentRepository.Add(planPaymentObj);
                        aBBPaymentRepository.SaveChanges();

                        response = "success";

                        #region code to send whatsappNotification For abb order confirmation
                        ABBOrderConfirmation whatsappObj = new ABBOrderConfirmation();
                        whatsappObj.userDetails = new UserDetails();
                        whatsappObj.notification = new NotificationForABB();
                        whatsappObj.notification.@params = new SendSmsABBWhatsapp();
                        whatsappObj.userDetails.number = abbregistrationObj.CustMobile;
                        whatsappObj.notification.sender = ConfigurationManager.AppSettings["Yello.aiSenderNumber"].ToString();
                        whatsappObj.notification.type = ConfigurationManager.AppSettings["Yellow.aiMesssaheType"].ToString();
                        whatsappObj.notification.templateId = NotificationConstants.abb_order_confirmation;
                        whatsappObj.notification.@params.RegdNo = abbregistrationObj.RegdNo.ToString();
                        string url = ConfigurationManager.AppSettings["Yellow.AiUrl"].ToString();
                        IRestResponse responseNew = WhatsappNotificationManager.Rest_InvokeWhatsappserviceCall(url, Method.POST, whatsappObj);
                        ResponseCode = responseNew.StatusCode.ToString();
                        WhatssAppStatusEnum = SponsorsApiCall.ExchangeOrderManager.GetEnumDescription(WhatssAppEnum.SuccessCode);
                        if (ResponseCode == WhatssAppStatusEnum)
                        {
                            responseforWhatasapp = responseNew.Content;
                            if (responseforWhatasapp != null)
                            {
                                whatssappresponseDC = JsonConvert.DeserializeObject<WhatasappResponse>(responseforWhatasapp);
                                tblWhatsAppMessage whatsapObj = new tblWhatsAppMessage();
                                whatsapObj.TemplateName = NotificationConstants.Test_Template;
                                whatsapObj.IsActive = true;
                                whatsapObj.PhoneNumber = abbregistrationObj.CustMobile;
                                whatsapObj.SendDate = DateTime.Now;
                                whatsapObj.msgId = whatssappresponseDC.msgId;
                                _whatsAppMessageRepository.Add(whatsapObj);
                                _whatsAppMessageRepository.SaveChanges();
                            }
                            else
                            {
                                string ExchOrderObj = JsonConvert.SerializeObject(abbregistrationObj);
                                logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", abbregistrationObj.RegdNo, ExchOrderObj);
                            }
                        }
                        else
                        {
                            string ExchOrderObj = JsonConvert.SerializeObject(abbregistrationObj);
                            logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", abbregistrationObj.RegdNo, ExchOrderObj);
                        }
                        #endregion
                    }
                    else
                    {
                        planPaymentObj.RegdNo = paymentReponse.RegdNo;
                        planPaymentObj.OrderId = paymentReponse.OrderId;
                        planPaymentObj.PaymentDate = DateTime.Now;
                        planPaymentObj.IsActive = true;
                        if (paymentReponse.responseCode == 100)
                        {
                            planPaymentObj.PaymentStatus = true;
                        }
                        else
                        {
                            planPaymentObj.PaymentStatus = false;
                        }
                        planPaymentObj.transactionId = paymentReponse.transactionId;
                        planPaymentObj.ResponseDescription = paymentReponse.responseDescription;
                        planPaymentObj.ResponseCode = paymentReponse.responseCode.ToString();
                        planPaymentObj.CardId = paymentReponse.cardId;
                        planPaymentObj.CardHashId = paymentReponse.cardhashId;
                        planPaymentObj.CardScheme = paymentReponse.cardScheme;
                        planPaymentObj.CardToken = paymentReponse.cardToken;
                        planPaymentObj.Bank = paymentReponse.bank;
                        planPaymentObj.BankId = paymentReponse.bankid;
                        planPaymentObj.amount = paymentReponse.amount;
                        planPaymentObj.CheckSum = paymentReponse.checksum;
                        planPaymentObj.PaymentMode = paymentReponse.paymentMode;
                        aBBPaymentRepository.Add(planPaymentObj);
                        aBBPaymentRepository.SaveChanges();

                        #region code to send whatsappNotification For abb order confirmation
                        ABBOrderConfirmation whatsappObj = new ABBOrderConfirmation();
                        whatsappObj.userDetails = new UserDetails();
                        whatsappObj.notification = new NotificationForABB();
                        whatsappObj.notification.@params = new SendSmsABBWhatsapp();
                        whatsappObj.userDetails.number = abbregistrationObj.CustMobile;
                        whatsappObj.notification.sender = ConfigurationManager.AppSettings["Yello.aiSenderNumber"].ToString();
                        whatsappObj.notification.type = ConfigurationManager.AppSettings["Yellow.aiMesssaheType"].ToString();
                        whatsappObj.notification.templateId = NotificationConstants.abb_order_confirmation_old;
                        whatsappObj.notification.@params.RegdNo = abbregistrationObj.RegdNo.ToString();
                        string url = ConfigurationManager.AppSettings["Yellow.AiUrl"].ToString();
                        IRestResponse responseNew = WhatsappNotificationManager.Rest_InvokeWhatsappserviceCall(url, Method.POST, whatsappObj);
                        ResponseCode = responseNew.StatusCode.ToString();
                        WhatssAppStatusEnum = SponsorsApiCall.ExchangeOrderManager.GetEnumDescription(WhatssAppEnum.SuccessCode);
                        if (ResponseCode == WhatssAppStatusEnum)
                        {
                            responseforWhatasapp = responseNew.Content;
                            if (responseforWhatasapp != null)
                            {
                                whatssappresponseDC = JsonConvert.DeserializeObject<WhatasappResponse>(responseforWhatasapp);
                                tblWhatsAppMessage whatsapObj = new tblWhatsAppMessage();
                                whatsapObj.TemplateName = NotificationConstants.Test_Template;
                                whatsapObj.IsActive = true;
                                whatsapObj.PhoneNumber = abbregistrationObj.CustMobile;
                                whatsapObj.SendDate = DateTime.Now;
                                whatsapObj.msgId = whatssappresponseDC.msgId;
                                _whatsAppMessageRepository.Add(whatsapObj);
                                _whatsAppMessageRepository.SaveChanges();
                            }
                            else
                            {
                                string ExchOrderObj = JsonConvert.SerializeObject(abbregistrationObj);
                                logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", abbregistrationObj.RegdNo, ExchOrderObj);
                            }
                        }
                        else
                        {
                            string ExchOrderObj = JsonConvert.SerializeObject(abbregistrationObj);
                            logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", abbregistrationObj.RegdNo, ExchOrderObj);
                        }
                        #endregion
                    }
                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBRegManager", "PaymentstatusUpdate", ex);
            }
            return response;
        }
        #endregion

        #region GetKeyFor ZaakPay

        public ZaakpayKey getPaymentKeyzaakpay()
        {
            ZaakpayKey zaakpay = new ZaakpayKey();
            zaakpay.merchantId = ConfigurationManager.AppSettings["MerchentId"].ToString();
            zaakpay.secretKey = ConfigurationManager.AppSettings["ZaakPaysecretKey"].ToString();
            return zaakpay;
        }
        #endregion


        #region Get BusinessPartner and BusinessUnit Margin for abb plan
        public ABBPlanMargin GetPlanMargin(ABBRegistrationViewModel abbvm)
        {
            _abbPriceMasterRepository = new ABBPriceMasterRepository();
            ABBPlanMargin marginDC = new ABBPlanMargin();
            tblABBPriceMaster abbPriceMasterrObj = new tblABBPriceMaster();
            decimal? GSTvalue = 0;
            try
            {
                abbPriceMasterrObj = _abbPriceMasterRepository.GetSingle(x => x.BusinessUnitId == abbvm.BusinessUnitId && (x.IsActive == true && x.Price_Start_Range <= abbvm.ProductNetPrice && x.Price_End_Range >= abbvm.ProductNetPrice || x.Fees_Applicable_Percentage > 0) && x.ProductCatId == abbvm.NewProductCategoryId && x.ProductTypeId == abbvm.NewProductCategoryTypeId);
                if (abbPriceMasterrObj != null)
                {
                    //code to calculate Gst amount
                    ///formula to calculate gst <>GSt value=GST Inclusive Price * GST Rate /(100 + GST Rate Percentage)</>
                    ///formula to calculate original cost <>original cost=GST Inclusive Price * 100/(100 + GST Rate Percentage)</>
                    if (abbPriceMasterrObj.Fees_Applicable_Amt > 0)
                    {
                        if (abbvm.GstType == Convert.ToInt32(ABBPlanEnum.GstInclusive))
                        {
                        
                            marginDC.BaseValue = (abbPriceMasterrObj.Fees_Applicable_Amt * 100) / (100 + abbPriceMasterrObj.GSTInclusive);
                            marginDC.abbFees = abbPriceMasterrObj.Fees_Applicable_Amt;
                            GSTvalue = marginDC.abbFees - marginDC.BaseValue;
                            marginDC.Cgst = GSTvalue / 2;
                            marginDC.Sgst = GSTvalue / 2;
                        }
                        else if (abbvm.GstType == Convert.ToInt32(ABBPlanEnum.GstExclusive))
                        {
                            marginDC.BaseValue = abbPriceMasterrObj.Fees_Applicable_Amt;
                            marginDC.abbFees = (abbPriceMasterrObj.Fees_Applicable_Amt * abbPriceMasterrObj.GSTExclusive) / 100;
                            marginDC.abbFees = marginDC.abbFees + abbPriceMasterrObj.Fees_Applicable_Amt;
                            GSTvalue = marginDC.abbFees - marginDC.BaseValue;
                            marginDC.Cgst = GSTvalue / 2;
                            marginDC.Sgst = GSTvalue / 2;
                        }
                        else
                        {
                            marginDC.BaseValue = abbPriceMasterrObj.Fees_Applicable_Amt;
                            marginDC.Cgst = 0;
                            marginDC.Sgst = 0;
                            marginDC.abbFees = abbPriceMasterrObj.Fees_Applicable_Amt;
                        }
                    }
                    else if (abbPriceMasterrObj.Fees_Applicable_Percentage > 0)
                    {
                        if (abbvm.GstType == Convert.ToInt32(ABBPlanEnum.GstInclusive))
                        {
                            decimal? abbfees = (abbvm.ProductNetPrice * abbPriceMasterrObj.Fees_Applicable_Percentage) / 100;
                            marginDC.BaseValue = (abbfees * 100) / (100 + abbPriceMasterrObj.GSTInclusive);
                            marginDC.abbFees = abbfees;
                            GSTvalue = marginDC.abbFees - marginDC.BaseValue;
                            marginDC.Cgst = GSTvalue / 2;
                            marginDC.Sgst = GSTvalue / 2;
                        }
                        else if (abbvm.GstType == Convert.ToInt32(ABBPlanEnum.GstExclusive))
                        {
                            decimal? abbfees = (abbvm.ProductNetPrice * abbPriceMasterrObj.Fees_Applicable_Percentage) / 100;
                            marginDC.BaseValue = abbfees;
                            marginDC.abbFees = (abbfees * abbPriceMasterrObj.GSTExclusive) / 100;
                            marginDC.abbFees = marginDC.abbFees + abbfees;
                            GSTvalue = marginDC.abbFees - marginDC.BaseValue;
                            marginDC.Cgst = GSTvalue / 2;
                            marginDC.Sgst = GSTvalue / 2;
                        }
                        else
                        {
                            decimal? abbfees = (abbvm.ProductNetPrice * abbPriceMasterrObj.Fees_Applicable_Percentage) / 100;
                            marginDC.BaseValue = abbfees;
                            marginDC.Cgst = 0;
                            marginDC.Sgst = 0;
                            marginDC.abbFees = abbfees;
                        }
                    }
                    else
                    {
                        marginDC.abbFees = 0;
                        marginDC.Cgst = 0;
                        marginDC.Sgst = 0;
                        marginDC.BaseValue = 0;
                    }

                    if (abbvm.Margintype == Convert.ToInt32(ABBPlanEnum.MarginTypeFixed))
                    {
                        marginDC.BusinessPartnerMargin = abbPriceMasterrObj.BusinessPartnerMarginAmount;
                        marginDC.BusinessUnitMargin = abbPriceMasterrObj.BusinessUnitMarginAmount;
                    }
                    else if (abbvm.Margintype == Convert.ToInt32(ABBPlanEnum.MarginTypePerc))
                    {
                        marginDC.BusinessPartnerMargin = (marginDC.BaseValue * abbPriceMasterrObj.BusinessPartnerMarginPerc) / 100;
                        marginDC.BusinessUnitMargin = (marginDC.BaseValue * abbPriceMasterrObj.BusinessUnitMarginPerc) / 100;
                    }
                    else
                    {
                        marginDC.BusinessPartnerMargin = 0;
                        marginDC.BusinessUnitMargin = 0;
                    }
                }
                else
                {
                    marginDC.abbFees = abbvm.ABBFees;
                    marginDC.Cgst = 0;
                    marginDC.Sgst = 0;
                    marginDC.BaseValue = abbvm.ABBFees;
                }

                if (marginDC != null)
                {
                    double amount = Convert.ToDouble(marginDC.BaseValue);
                    string amountnew = String.Format("{0:0.00}", amount);
                    double gst = Convert.ToDouble(marginDC.Cgst);
                    string gstamount= String.Format("{0:0.00}", gst);
                    marginDC.BaseValue =Convert.ToDecimal(amountnew);
                    marginDC.Cgst = Convert.ToDecimal(gstamount);
                    marginDC.Sgst = Convert.ToDecimal(gstamount);
                    double amountabbfees = Convert.ToDouble(marginDC.abbFees);
                    string amountnewabbfees = String.Format("{0:0.00}", amountabbfees);
                    marginDC.abbFees = Convert.ToDecimal(amountnewabbfees);
                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBRegManager", "GetPlanMargin", ex);
            }
            return marginDC;
        }
        #endregion

        #region Update ABBRegistration with Invoice data
        public string UpdateInvoiceDetailsABB(PaymentInitiateModel requestDC)
        {
            aBBRegistrationRepository = new ABBRegistrationRepository();
            string status = null;
            try
            {
                tblABBRegistration abbRegistrationObj = aBBRegistrationRepository.GetSingle(x => x.RegdNo == requestDC.orderRegdNo && x.IsActive == true);
                if (abbRegistrationObj != null)
                {
                    abbRegistrationObj.InvoiceNo = requestDC.InvoiceNo;
                    abbRegistrationObj.InvoiceImage = requestDC.InvoiceImage;
                    abbRegistrationObj.InvoiceDate = DateTime.Now;
                    aBBRegistrationRepository.Update(abbRegistrationObj);
                    aBBRegistrationRepository.SaveChanges();
                    status = "success";
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBRegManager", "UpdateInvoiceDetailsABB", ex);
            }
            return status;
        }
        #endregion

        #region
        public OrderConfirmationDataContract UpdateABBOrderData(ABBRegistrationUpdateDataContract abbRegistrationDC)
        {
            aBBRegistrationRepository = new ABBRegistrationRepository();
            _customerDetailsrepository = new CustomerDetailsRepository();
            DateTime _dateTime = DateTime.Now;
            string fileName = string.Empty;
            tblABBRegistration abbRegistrationObj = new tblABBRegistration();
            tblCustomerDetail customerDetailObj = null;
            OrderConfirmationDataContract orderconfirmation = new OrderConfirmationDataContract();
            string WhatssAppStatusEnum = string.Empty;
            string ResponseCode = string.Empty;
            string responseforWhatasapp = string.Empty;
            _whatsAppMessageRepository = new WhatsappMessageRepository();
            WhatasappResponse whatssappresponseDC = null;
            try
            {
                if (abbRegistrationDC.Base64StringValue != null)
                {
                    byte[] bytes = System.Convert.FromBase64String(abbRegistrationDC.Base64StringValue);
                    fileName = abbRegistrationDC.RegdNo + _dateTime.ToString("yyyyMMddHHmmssFFF") + Path.GetExtension("image.jpeg");
                    string rootPath = @HostingEnvironment.ApplicationPhysicalPath;
                    string filePath = ConfigurationManager.AppSettings["InvoiceImage"].ToString() + fileName;
                    System.IO.File.WriteAllBytes(rootPath + filePath, bytes);
                    abbRegistrationDC.InvoiceImage = fileName;
                }
                abbRegistrationObj = aBBRegistrationRepository.GetSingle(x => x.IsActive == true && x.RegdNo == abbRegistrationDC.RegdNo);
                if (abbRegistrationObj != null)
                {
                    customerDetailObj = _customerDetailsrepository.GetSingle(x => x.IsActive == true && x.Id == abbRegistrationObj.CustomerId);
                    if (customerDetailObj != null)
                    {
                        orderconfirmation.RegdNo = abbRegistrationObj.RegdNo;
                        orderconfirmation.ABBRegistrationId = abbRegistrationObj.ABBRegistrationId;
                        orderconfirmation.CustomerDetailId = Convert.ToInt32(abbRegistrationObj.CustomerId);
                        orderconfirmation.Message = "Success";
                        #region Code to update customer details and invoice details in abb registration table
                        abbRegistrationObj.CustFirstName = abbRegistrationDC.FirstName;
                        abbRegistrationObj.CustLastName = abbRegistrationDC.LastName;
                        abbRegistrationObj.CustEmail = abbRegistrationDC.Email;
                        abbRegistrationObj.CustState = abbRegistrationDC.State;
                        abbRegistrationObj.CustCity = abbRegistrationDC.City;
                        abbRegistrationObj.CustAddress1 = abbRegistrationDC.Address1;
                        abbRegistrationObj.CustAddress2 = abbRegistrationDC.Address2;
                        abbRegistrationObj.CustPinCode = abbRegistrationDC.PinCode;
                        abbRegistrationObj.InvoiceImage = abbRegistrationDC.InvoiceImage;
                        abbRegistrationObj.InvoiceDate = abbRegistrationDC.InvoiceDate;
                        abbRegistrationObj.InvoiceNo = abbRegistrationDC.InvoiceNo;
                        abbRegistrationObj.ProductSrNo = abbRegistrationDC.ProductSerialNumber;
                        abbRegistrationObj.UploadDateTime = DateTime.Now;
                        aBBRegistrationRepository.Update(abbRegistrationObj);
                        aBBRegistrationRepository.SaveChanges();
                        #endregion
                        #region Code to update customer details in customer details table
                        customerDetailObj.FirstName = abbRegistrationDC.FirstName;
                        customerDetailObj.LastName = abbRegistrationDC.LastName;
                        customerDetailObj.Email = abbRegistrationDC.Email;
                        customerDetailObj.State = abbRegistrationDC.State;
                        customerDetailObj.City = abbRegistrationDC.City;
                        customerDetailObj.ZipCode = abbRegistrationDC.PinCode;
                        customerDetailObj.Address1 = abbRegistrationDC.Address1;
                        customerDetailObj.Address2 = abbRegistrationDC.Address2;
                        customerDetailObj.IsActive = true;
                        customerDetailObj.ModifiedDate = DateTime.Now;
                        customerDetailObj.ModifiedBy = Convert.ToInt32(UserEnum.Admin);
                        _customerDetailsrepository.Update(customerDetailObj);
                        _customerDetailsrepository.SaveChanges();
                        #endregion
                    }

                    #region code to send whatsappNotification For abb order confirmation
                    ABBOrderConfirmation whatsappObj = new ABBOrderConfirmation();
                    whatsappObj.userDetails = new UserDetails();
                    whatsappObj.notification = new NotificationForABB();
                    whatsappObj.notification.@params = new SendSmsABBWhatsapp();
                    whatsappObj.userDetails.number = abbRegistrationDC.PhoneNumber;
                    whatsappObj.notification.sender = ConfigurationManager.AppSettings["Yello.aiSenderNumber"].ToString();
                    whatsappObj.notification.type = ConfigurationManager.AppSettings["Yellow.aiMesssaheType"].ToString();
                    whatsappObj.notification.templateId = NotificationConstants.abb_order_confirmation_old;
                    whatsappObj.notification.@params.RegdNo = abbRegistrationDC.RegdNo.ToString();
                    string url = ConfigurationManager.AppSettings["Yellow.AiUrl"].ToString();
                    IRestResponse response = WhatsappNotificationManager.Rest_InvokeWhatsappserviceCall(url, Method.POST, whatsappObj);
                    ResponseCode = response.StatusCode.ToString();
                    WhatssAppStatusEnum = SponsorsApiCall.ExchangeOrderManager.GetEnumDescription(WhatssAppEnum.SuccessCode);
                    if (ResponseCode == WhatssAppStatusEnum)
                    {
                        responseforWhatasapp = response.Content;
                        if (responseforWhatasapp != null)
                        {
                            whatssappresponseDC = JsonConvert.DeserializeObject<WhatasappResponse>(responseforWhatasapp);
                            tblWhatsAppMessage whatsapObj = new tblWhatsAppMessage();
                            whatsapObj.TemplateName = NotificationConstants.Test_Template;
                            whatsapObj.IsActive = true;
                            whatsapObj.PhoneNumber = abbRegistrationDC.PhoneNumber;
                            whatsapObj.SendDate = DateTime.Now;
                            whatsapObj.msgId = whatssappresponseDC.msgId;
                            _whatsAppMessageRepository.Add(whatsapObj);
                            _whatsAppMessageRepository.SaveChanges();
                        }
                        else
                        {
                            string ExchOrderObj = JsonConvert.SerializeObject(abbRegistrationDC);
                            logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", abbRegistrationDC.RegdNo, ExchOrderObj);
                        }
                    }
                    else
                    {
                        string ExchOrderObj = JsonConvert.SerializeObject(abbRegistrationDC);
                        logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", abbRegistrationDC.RegdNo, ExchOrderObj);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                orderconfirmation.Message = "" + ex.Message;
                LibLogging.WriteErrorToDB("ABBRegManager", "UpdateABBOrderData", ex);
            }
            return orderconfirmation;
        }
        #endregion

        #region Create Html string for Customer ABB Invoice Added by VK
        /// <summary>
        /// Create Html string for Customer ABB Invoice Added by VK
        /// </summary>
        /// <param name="abbRegVM"></param>
        /// <param name="HtmlTemplateNameOnly"></param>
        /// <returns></returns>
        public string GetInvoiceHtmlString(ABBRegistrationViewModel abbRegVM, string HtmlTemplateNameOnly)
        {
            #region Variable Declaration
            var DateTime = System.DateTime.Now;
            string FinalDate = DateTime.Date.ToShortDateString();
            string htmlString = "";
            string fileName = HtmlTemplateNameOnly + ".html";
            string fileNameWithPath = "";
            string baseUrl = ConfigurationManager.AppSettings["BaseURL"].ToString();
            decimal? basePriceInv = 0;
            decimal? gstInv = 0;
            #endregion

            try
            {
                if (abbRegVM != null && HtmlTemplateNameOnly != null)
                {
                    #region Get Html String Dynamically
                    fileNameWithPath = ConfigurationManager.AppSettings["ABBInvoice"].ToString();
                    htmlString = File.ReadAllText(fileNameWithPath);
                    #endregion

                    #region Configuration setup for Invoice
                    //var UtcSeel_INV = baseUrl + EnumHelper.DescriptionAttr(FileAddressEnum.UTCACSeel);
                    var UtcSeel_INV = ConfigurationManager.AppSettings["UTC_Seel_Inv"].ToString();
                    var CurrentDate = Convert.ToDateTime(DateTime).ToString("dd/MM/yyyy");
                    decimal? finalPriceInv = abbRegVM.ABBFees ?? 0;
                    string baseVal = abbRegVM.BaseValue != null ? abbRegVM.BaseValue : "0";
                    basePriceInv = Convert.ToDecimal(baseVal);
                    gstInv = Convert.ToDecimal(abbRegVM.Cgst != null ? abbRegVM.Cgst : "0");

                    #region Invoice GST Calculation
                    //string? GSTInclusive = EnumHelper.DescriptionAttr(LoVEnum.GSTInclusive);// Get this value from BU table. For now its is kept as true;
                    //string? GSTExclusive = EnumHelper.DescriptionAttr(LoVEnum.GSTExclusive);
                    //string? GSTNotApplicable = EnumHelper.DescriptionAttr(LoVEnum.GSTNotApplicable);
                    //if (abbRegVM.GSTType == GSTInclusive)
                    //{
                    //    basePriceInv = finalPriceInv / Convert.ToDecimal(GeneralConstant.GSTPercentage);
                    //    basePriceInv = Math.Round((basePriceInv ?? 0), 2);
                    //    gstInv = basePriceInv * Convert.ToDecimal(GeneralConstant.CGST);
                    //    gstInv = Math.Round((gstInv ?? 0), 2);
                    //}
                    //else if (abbRegVM.GSTType == GSTExclusive)
                    //{
                    //    basePriceInv = finalPriceInv;
                    //    basePriceInv = Math.Round((basePriceInv ?? 0), 2);
                    //    gstInv = basePriceInv * Convert.ToDecimal(GeneralConstant.CGST);
                    //    gstInv = Math.Round((gstInv ?? 0), 2);
                    //}
                    //else if (abbRegVM.GSTType == GSTNotApplicable)
                    //{
                    //    basePriceInv = finalPriceInv;
                    //    basePriceInv = Math.Round((basePriceInv ?? 0), 2);
                    //    gstInv = 0;
                    //}
                    #endregion

                    decimal? finalPriceWithGSTInv = finalPriceInv;
                    string finalPiceInWordsInv = NumberToWordsConverterHelper.ConvertAmount(Convert.ToDecimal(finalPriceWithGSTInv));
                    #endregion

                    #region Replace Dynamic part from the template
                    if (HtmlTemplateNameOnly == "ABB_Invoice")
                    {
                        htmlString = htmlString.Replace("[BillNumber]", abbRegVM.BillNumber)
                            .Replace("[PlaceOfSupply]", abbRegVM.CustState)
                            .Replace("[CurrentDate]", CurrentDate)
                            .Replace("[CustomerName]", abbRegVM.CustFirstName + " " + abbRegVM.CustLastName)
                            .Replace("[Address]", abbRegVM.CustAddress1)
                            .Replace("[City]", abbRegVM.CustCity)
                            .Replace("[Pincode]", abbRegVM.CustPinCode)
                            .Replace("[State]", abbRegVM.CustState)
                            .Replace("[RegdNo]", abbRegVM.RegdNo)
                            .Replace("[BasePrice]", basePriceInv.ToString())
                            .Replace("[GST]", gstInv.ToString())
                            .Replace("[FinalPrice]", finalPriceWithGSTInv.ToString())
                            .Replace("[FinalAmtInWords]", finalPiceInWordsInv)
                            .Replace("[UtcSeel_INV]", UtcSeel_INV);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBRegManager", "GetInvoiceHtmlString", ex);
            }
            return htmlString;
        }
        #endregion

        #region Generate PDF
        public bool GenerateInvoicePDF(ABBRegistrationViewModel abbRegVM, TemplateDataContract templateDC)
        {
            bool flag = false;
            string HtmlString = null;
            try
            {
                HtmlString = GetInvoiceHtmlString(abbRegVM, "ABB_Invoice");
                flag = HtmlToPDFConverterHelper.GeneratePDF(HtmlString, templateDC.InvAttachFilePath, templateDC.InvAttachFileName);
            }
            catch(Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBRegManager", "GetInvoiceHtmlString", ex);
            }
            return flag;
        }
        #endregion

        #region Store Customer Files in database
        /// <summary>
        /// Store Customer Files in database
        /// </summary>       
        /// <returns></returns>   
        public int ManageABBCustomerFiles(ABBRegistrationViewModel abbRegVM, TemplateDataContract templateDC)
        {
            tblCustomerFile tblCustomerFile = null;
            string fileName = null;
            int result = 0;
            int customerFilesId = 0;
            DateTime _currentDatetime = DateTime.Now;
            try
            {
                if (abbRegVM != null && templateDC != null && templateDC.IsInvoiceGenerated == true)
                {
                    tblCustomerFile = GetCustomerFilesByRegdNo(abbRegVM.RegdNo);
                    if (tblCustomerFile == null)
                    {
                        tblCustomerFile = new tblCustomerFile();
                        //tblEVCPODDetailObj = _mapper.Map<PODViewModel, TblEvcpoddetail>(podVM);
                    }

                    #region Common Data mapping
                    tblCustomerFile.InvoicePdfName = templateDC.InvAttachFileName;
                    tblCustomerFile.InvSrNum = Convert.ToInt32(abbRegVM.BillCounterNum);
                    tblCustomerFile.InvoiceDate = _currentDatetime;
                    tblCustomerFile.InvoiceAmount = abbRegVM.ABBFees;
                    tblCustomerFile.FinancialYear = abbRegVM.FinancialYear;
                    #endregion

                    if (tblCustomerFile.Id > 0)
                    {
                        tblCustomerFile.ABBRegistrationId = abbRegVM.ABBRegistrationId;
                        tblCustomerFile.ModifiedDate = _currentDatetime;
                        _customerFilesRepository.Update(tblCustomerFile);
                    }
                    else
                    {
                        tblCustomerFile.ABBRegistrationId = abbRegVM.ABBRegistrationId;
                        tblCustomerFile.RegdNo = abbRegVM.RegdNo;
                        tblCustomerFile.IsActive = true;
                        tblCustomerFile.CreatedDate = _currentDatetime;
                        _customerFilesRepository.Add(tblCustomerFile);
                    }
                    result = _customerFilesRepository.SaveChanges();
                    if (result > 0)
                    {
                        customerFilesId = tblCustomerFile.Id;
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBRegManager", "ManageABBCustomerFiles", ex);
            }
            return customerFilesId;
        }
        #endregion

        #region Get ABB Invoice Max Sr. Num
        public int? GetAbbMaxInvSrNum(string FinancialYear)
        {
            _customerFilesRepository = new CustomerFilesRepository();
            int? maxInvNum = 0;
            try
            {
                if (!string.IsNullOrEmpty(FinancialYear))
                {
                    maxInvNum = _customerFilesRepository.GetList(x => x.IsActive == true && (x.FinancialYear ?? "").Trim().ToLower() == FinancialYear).Max(x => x.InvSrNum);
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBRegManager", "GetAbbMaxInvSrNum", ex);
            }
            return maxInvNum;
        }
        #endregion

        #region Get Customer Files by RegdNo
        public tblCustomerFile GetCustomerFilesByRegdNo(string regdNo)
        {
            _customerFilesRepository = new CustomerFilesRepository();
            tblCustomerFile tblCustomerFile = null;
            try
            {
                if (!string.IsNullOrEmpty(regdNo))
                {
                    regdNo = regdNo.Trim().ToLower();
                    tblCustomerFile = _customerFilesRepository.GetSingle(x => x.IsActive == true && (x.RegdNo ?? "").Trim().ToLower() == regdNo);
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBRegManager", "GetCustomerFilesByRegdNo", ex);
            }
            return tblCustomerFile;
        }
        #endregion

        #region Get List of Configurations added by VK
        public List<tblConfiguration> GetConfigurationList()
        {
            _configurationRepository = new ConfigurationRepository();
            List<tblConfiguration> TblConfigurationList = null;
            try
            {
                TblConfigurationList = _configurationRepository.GetList(x => x.IsActive == true).ToList();
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBRegManager", "GetConfigurationList", ex);
            }
            return TblConfigurationList;
        }
        #endregion


        #region delete trans data 
        public bool DeleteOldDataFromTrans(int prodtypeId,int BusinessUnitId,int AbbRegistrationId)
        {
            _transactionRepository = new TransactionABBPlanMasterRepository();
            List<tblTransMasterABBPlanMaster> transmaster = null;
            try
            {
                transmaster = _transactionRepository.GetList(x => x.ABBRegistrationId == AbbRegistrationId && x.BusinessUnitId == BusinessUnitId && x.ProductTypeId == prodtypeId).ToList();
                if (transmaster.Count > 0)
                {
                    foreach (var item in transmaster)
                    {
                        _transactionRepository.Remove(item);
                        _transactionRepository.SaveChanges();
                    }
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBRegManager", "DeleteOldDataFromTrans", ex);
            }
            return true;
        }

        #endregion
        #region set view model to update transaction table
        public ABBRegistrationViewModel setTransactionObject(ABBRegistrationUpdateDataContract updateddata)
        {
            ABBRegistrationViewModel model = new ABBRegistrationViewModel();
            try
            {
                model.NewProductCategoryId = updateddata.ProductcategoryId;
                model.NewProductCategoryTypeId = updateddata.ProductTypeID;
                model.BusinessUnitId = updateddata.ProductTypeID;
            }
            catch(Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBRegManager", "setTransactionObject", ex);
            }
            return model;
        }
        #endregion

    }
}
