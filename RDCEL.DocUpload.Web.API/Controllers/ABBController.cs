
using GraspCorn.Common.Enums;
using GraspCorn.Common.Helper;
using Microsoft.Owin.Security.Provider;
using Newtonsoft.Json;
using Razorpay.Api;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Windows.Interop;
using RDCEL.DocUpload.BAL.ABBRegistration;
using RDCEL.DocUpload.BAL.Common;
using RDCEL.DocUpload.BAL.OCR_Invoice_Validator;
using RDCEL.DocUpload.BAL.OcrImageReaderCall;
using RDCEL.DocUpload.BAL.SponsorsApiCall;
using RDCEL.DocUpload.BAL.zaakpay;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.Web.API.Models;
using RDCEL.DocUpload.DataContract;
using RDCEL.DocUpload.DataContract.ABBRegistration;
using RDCEL.DocUpload.DataContract.BlowHorn;
using RDCEL.DocUpload.DataContract.OCRDataContract;
using RDCEL.DocUpload.DataContract.PluralGateway;
using RDCEL.DocUpload.DataContract.ProductTaxonomy;
using RDCEL.DocUpload.DataContract.ZohoModel;
//using static RDCEL.DocUpload.Web.API.Controllers.api.ABBController;

namespace RDCEL.DocUpload.Web.API.Controllers
{
    public class ABBController : Controller
    {
        #region Variable declaration
        BusinessPartnerRepository _businessPartnerRepository;
        BusinessUnitRepository _businessUnitRepository;
        ABBRegistrationRepository _ABBRegistrationRepository;
        BrandRepository _brandRepository;
        ProductCategoryRepository _productCategoryRepository;
        ProductTypeRepository _productTypeRepository;
        MasterManager _masterManager;
        MessageDetailRepository _messageDetailRepository;
        ModelNumberRepository _modelNumberRepository;
        private static Random random = new Random();
        DateTime _currentDatetime = DateTime.Now;
        ProductCategoryMappingRepository _productCategoryMappingRepository;
        ABBPaymentRepository _paymentRepository;
        ABBPlanMasterRepository _abbPlanMaster;
        PinCodeRepository pinCodeRepository;
        ABBPriceMasterRepository _abbPriceMasterRepository;
        BrandSmartSellRepository _brandMappingRepository;
        CustomerDetailsRepository _customerDetailsRepository;
        #endregion

        // GET: ABB
        public ActionResult Index()
        {
            return View();
        }

        // GET: ABB/Details/5
        public ActionResult Details(string message)
        {
            string msg = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(message))
                    msg = message;
                else
                    msg = "Some error occurred, please connect with the Administrator.";

                ViewBag.MSG = msg;

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBController", "Details", ex);
            }
            return View();
        }

        // GET: ABB/Details/5
        public ActionResult DetailsFailedOrder(string message)
        {
            string msg = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(message))
                    msg = message;
                else
                    msg = "Some error occurred, please connect with the Administrator.";

                ViewBag.MSG = msg;

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBController", "Details", ex);
            }
            return View();
        }
        // GET: ABB/Create
        public ActionResult ABBRegistration(ABBViewModel ABBVM)
        {
            _masterManager = new MasterManager();
            _abbPlanMaster = new ABBPlanMasterRepository();
            _productCategoryRepository = new ProductCategoryRepository();
            _productCategoryMappingRepository = new ProductCategoryMappingRepository();
            _businessPartnerRepository = new BusinessPartnerRepository();
            _businessUnitRepository = new BusinessUnitRepository();
            _ABBRegistrationRepository = new ABBRegistrationRepository();
            _brandRepository = new BrandRepository();
            _abbPriceMasterRepository = new ABBPriceMasterRepository();
            List<tblBusinessUnit> businessUnitList = new List<tblBusinessUnit>();
            tblBusinessUnit BusinessUnitObj = new tblBusinessUnit();
            List<tblBusinessPartner> businessPartnerList = new List<tblBusinessPartner>();
            tblBusinessPartner businessPartnerObj = new tblBusinessPartner();
            ABBRegistrationViewModel ABBRegistrationObj = new ABBRegistrationViewModel();
            tblABBPlanMaster abbPlanObj = new tblABBPlanMaster();
            tblBrand brandObj = new tblBrand();
            _modelNumberRepository = new ModelNumberRepository();
            _brandMappingRepository = new BrandSmartSellRepository();
            List<tblBrandSmartBuy> brandDetails = null;
            List<tblABBPriceMaster> abbplanpriceObj = new List<tblABBPriceMaster>();
            try
            {
                if (ABBVM.BusinessUnitId > 0)
                {  // business partner details for deferred case 
                    if (ABBVM.BusinessPartnerId > 0)
                    {
                        businessPartnerObj = _businessPartnerRepository.GetSingle(x => x.BusinessPartnerId == ABBVM.BusinessPartnerId);
                        if (businessPartnerObj != null)
                        {
                            ABBRegistrationObj.IsdefferedAbb = Convert.ToBoolean(businessPartnerObj.IsDefferedAbb);
                            ABBRegistrationObj.IsD2C = Convert.ToBoolean(businessPartnerObj.IsD2C);
                            ABBRegistrationObj.StoreCode = businessPartnerObj.StoreCode;
                            if (ABBVM.StoreName != null)
                            {
                                ABBRegistrationObj.StoreName = ABBVM.StoreName;
                            }
                            else
                            {
                                ABBRegistrationObj.StoreName = businessPartnerObj.Description;
                            }
                        }
                        else
                        {
                            ABBRegistrationObj.StoreName = ABBVM.StoreName;
                        }
                        ABBRegistrationObj.BusinessPartnerId = ABBVM.BusinessPartnerId;
                    }
                    tblABBPlanMaster abbplanObj = _abbPlanMaster.GetSingle(x => x.BusinessUnitId == ABBVM.BusinessUnitId && x.ProductCatId == ABBVM.productCategoryId && x.ProductTypeId == ABBVM.ProductTypeId);
                    if (abbplanObj != null)
                    {
                        if (ABBVM.planName == null && ABBVM.planduration == null && ABBVM.NoClaimPeriod == null)
                        {
                            ABBRegistrationObj.ABBPlanName = abbplanObj.ABBPlanName;
                            ABBRegistrationObj.ABBPlanPeriod = abbplanObj.PlanPeriodInMonth.ToString();
                            ABBRegistrationObj.NoOfClaimPeriod = abbplanObj.NoClaimPeriod;
                        }
                        else
                        {
                            ABBRegistrationObj.ABBPlanName = ABBVM.planName;
                            ABBRegistrationObj.ABBPlanPeriod = ABBVM.planduration;
                            ABBRegistrationObj.NoOfClaimPeriod = ABBVM.NoClaimPeriod;
                        }
                    }
                    ABBRegistrationObj.PlanPrice = ABBVM.planprice;
                    ABBRegistrationObj.ABBFees = Convert.ToDecimal(ABBVM.planprice);
                    ABBRegistrationObj.BaseValue = ABBVM.BaseValue;
                    ABBRegistrationObj.Sgst = ABBVM.BaseValue;
                    ABBRegistrationObj.Cgst = ABBVM.BaseValue;
                    //BU details
                    BusinessUnitObj = _businessUnitRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == ABBVM.BusinessUnitId);
                    if (BusinessUnitObj != null)
                    {
                        if (BusinessUnitObj.LogoName != null)
                        {
                            ABBRegistrationObj.BULogoName = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Content/DB_Files/SponsorLogo/" + BusinessUnitObj.LogoName;
                        }
                        ABBRegistrationObj.BUName = BusinessUnitObj.Name;
                        ABBRegistrationObj.BusinessUnitId = ABBVM.BusinessUnitId;
                        ABBRegistrationObj.Margintype = Convert.ToInt32(BusinessUnitObj.MarginType);
                        ABBRegistrationObj.GstType = Convert.ToInt32(BusinessUnitObj.GSTType);
                        ABBRegistrationObj.IsBuMultibrand = Convert.ToBoolean(BusinessUnitObj.IsBUMultiBrand);
                        if (BusinessUnitObj.IsAbbDayConfig == null)
                        {
                            ABBRegistrationObj.IsAbbDayConfig = false;
                        }
                        else
                        {
                            ABBRegistrationObj.IsAbbDayConfig = (bool)BusinessUnitObj.IsAbbDayConfig == true ? true : false;
                        }
                        if (ABBRegistrationObj.IsAbbDayConfig == true)
                        {
                            ABBRegistrationObj.AbbDayDiff = (int)BusinessUnitObj.AbbDayDiff > 0 ? (int)BusinessUnitObj.AbbDayDiff : 0;
                        }
                        else
                        {
                            var VarAbbDayDiff = ConfigurationManager.AppSettings["DefaultABBDayDiff"];
                            ABBRegistrationObj.AbbDayDiff = Convert.ToInt32(VarAbbDayDiff);
                        }
                        if (BusinessUnitObj.IsSponsorNumberRequiredOnUI != null)
                        {
                            ABBRegistrationObj.IsSponsorOrderNumberRequired = Convert.ToBoolean(BusinessUnitObj.IsSponsorNumberRequiredOnUI);
                        }
                        else
                        {
                            ABBRegistrationObj.IsSponsorOrderNumberRequired = false;
                        }
                        
                        if (BusinessUnitObj.IsBUMultiBrand == false)
                        {
                            //get Brand Details
                            tblBrand brand = _brandRepository.GetSingle(x => x.BusinessUnitId.Equals(BusinessUnitObj.BusinessUnitId));
                            if (brand != null)
                            {
                                ABBRegistrationObj.NewBrandId = brand.Id;
                                ABBRegistrationObj.BrandName = brand.Name;
                            }
                        }
                        else
                        {
                            DataTable dt = _brandMappingRepository.GetBrandistbyBU(ABBVM.productCategoryId, ABBVM.BusinessUnitId);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                brandDetails = GenericConversionHelper.DataTableToList<tblBrandSmartBuy>(dt);
                            }
                            if (brandDetails!=null && brandDetails.Count > 0)
                            {

                                //List<tblBrand> brands = _brandRepository.GetList(x => x.IsActive == true).ToList();
                                ViewBag.BrandList = new SelectList(brandDetails, "Id", "Name");

                            }
                            else
                            {
                                return RedirectToAction("DetailsFailedOrder", "ABB", new { message = "Brand Not found" });

                            }
                        }
                    }
                    ABBRegistrationObj.NewProductCategoryId = ABBVM.productCategoryId;
                    ABBRegistrationObj.NewProductCategoryTypeId = ABBVM.ProductTypeId;
                    ABBRegistrationObj.ProductNetPrice = ABBVM.NetProductPrice;
                    ABBRegistrationObj.BusinessPartnerId = ABBVM.BusinessPartnerId;
                    ABBRegistrationObj.EmployeeId = ABBVM.EmployeeId;
                    ABBRegistrationObj.ProductModelList = new List<SelectListItem>();
                    if (ABBVM.BusinessUnitId == Convert.ToInt32(BusinessUnitEnum.Havells))
                    {
                        ABBRegistrationObj.IsdefferedAbb = false;
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABB", "ABBRegistration", ex);
                return View(ABBRegistrationObj);
            }

            return View(ABBRegistrationObj);
        }

        // POST: ABB/Create
        [HttpPost]
        public ActionResult ABBRegistration(HttpPostedFileBase InvoiceImage, ABBRegistrationViewModel model)
        {
            ABBRegManager ABBRegInfo = new ABBRegManager();
            ABBRegistrationDataContract ABBRegistrationDC = null;
            ABBRegistrationFormResponseDataContract ABBRegistrationResponseDC = null;
            ABBRegistrationResponseModel ABBresponse = new ABBRegistrationResponseModel();
            _modelNumberRepository = new ModelNumberRepository();
            ABBPlanTransactionResponse TransactioResponseforPlan = new ABBPlanTransactionResponse();
            tblModelNumber modelNoObj = null;
            int ABBRegId = 0;
            DateTime _dateTime = DateTime.Now;
            string msg = string.Empty;
            string fileName = string.Empty;
            string Email = string.Empty;
            string ZohoPushFlag = string.Empty;
            string smsFlag = null;
            try
            {
                smsFlag = ConfigurationManager.AppSettings["sendsmsflag"].ToString();
                Email = model.CustEmail.Trim();
                model.CustEmail = Email;
                model.RegdNo = "A" + UniqueString.RandomNumberByLength(8);
                if (model.Base64StringValue != null)
                {
                    byte[] bytes = System.Convert.FromBase64String(model.Base64StringValue);
                    fileName = model.RegdNo + _dateTime.ToString("yyyyMMddHHmmssFFF") + Path.GetExtension("image.jpeg");
                    string rootPath = @HostingEnvironment.ApplicationPhysicalPath;
                    string filePath = ConfigurationManager.AppSettings["InvoiceImage"].ToString() + fileName;
                    System.IO.File.WriteAllBytes(rootPath + filePath, bytes);
                    model.InvoiceImage = fileName;
                }
                else
                {
                    msg = "Invoice image is not proper plz process again ";
                    return RedirectToAction("DetailsFailedOrder","ABB",new { message = msg });
                }
                if (model != null)
                {
                    if (model.ModelNumberId > 0)
                    {
                        modelNoObj = _modelNumberRepository.GetSingle(x => x.ModelNumberId == model.ModelNumberId);
                        if (modelNoObj != null)
                            model.ModelName = modelNoObj.ModelName;
                    }
                    #region Code to add ABB Reg. details in database
                    ABBresponse = ABBRegInfo.AddABBReg(model);
                    model.ABBRegistrationId = ABBresponse.RegistrationId;
                    TransactioResponseforPlan = ABBRegInfo.SaveABBPlanDetails(model);
                    #endregion
                    #region Code to add ABB REg in zoho creator
                    ZohoPushFlag = ConfigurationManager.AppSettings["ZohoPush"].ToString();
                    if (ABBresponse.RegistrationId > 0)
                    {
                        if (model.IsdefferedAbb == true)
                        {
                            if (ZohoPushFlag == "true")
                            {
                                ABBRegistrationDC = ABBRegInfo.SetZohoABBRegObject(model);
                                ABBRegistrationResponseDC = ABBRegInfo.AddZohoABBReg(ABBRegistrationDC);
                            }
                            else
                            {
                                Data data = new Data();
                                //var ID = 0;
                                data.ID = "0";
                                ABBRegistrationResponseDC = new ABBRegistrationFormResponseDataContract();
                                ABBRegistrationResponseDC.code = 400;
                                ABBRegistrationResponseDC.message = "bypass zoho";
                                //sponserResponseDC.data.ID = ID;
                                ABBRegistrationResponseDC.data = data;
                            }
                        }
                        else
                        {
                            if (model.IsPayNow == true)
                            {
                                string IsPluralActive = ConfigurationManager.AppSettings["PluralActive"].ToString();
                                decimal planprice = Convert.ToDecimal(model.PlanPrice);
                                string PlnaPrice = planprice.ToString();
                                string KeyString = ConfigurationManager.AppSettings["EncryptionKey"].ToString();
                                string IV = ConfigurationManager.AppSettings["EncryptionIV"].ToString();
                                string FirstName = AES256encoding.EncryptString(model.CustFirstName, KeyString, IV);
                                string LastName = AES256encoding.EncryptString(model.CustLastName, KeyString, IV);
                                string Name = AES256encoding.EncryptString(model.CustFirstName + " " + model.CustLastName, KeyString, IV);
                                string EmailAddress = AES256encoding.EncryptString(model.CustEmail, KeyString, IV);
                                string planAmount = AES256encoding.EncryptString(model.PlanPrice, KeyString, IV);
                                string phonenumber = AES256encoding.EncryptString(model.CustMobile, KeyString, IV);
                                string custaddress1 = AES256encoding.EncryptString(model.CustAddress1, KeyString, IV);
                                string custaddress2 = AES256encoding.EncryptString(model.CustAddress1, KeyString, IV);
                                string custaddress = AES256encoding.EncryptString(model.CustAddress1 + " " + model.CustAddress2, KeyString, IV);
                                string orderId = AES256encoding.EncryptString(model.RegdNo, KeyString, IV);
                                string ProductCategory = AES256encoding.EncryptString(model.NewProductCategoryId.ToString(), KeyString, IV);
                                string productType = AES256encoding.EncryptString(model.NewProductCategoryTypeId.ToString(), KeyString, IV);
                                string state = AES256encoding.EncryptString(model.CustState.ToString(), KeyString, IV);
                                string city = AES256encoding.EncryptString(model.CustCity.ToString(), KeyString, IV);
                                string pincode = AES256encoding.EncryptString(model.CustPinCode.ToString(), KeyString, IV);
                                string ModuleName = AES256encoding.EncryptString("URL", KeyString, IV);
                               if (IsPluralActive == "true")
                                {
                                    return RedirectToAction("CreateOrderForPluralGateway", "ABB", new { name = Name, Firstname = FirstName, Lastname = LastName, email = EmailAddress, contactNumber = phonenumber, address = custaddress, address1 = custaddress1, address2 = custaddress2, planPrice = planAmount, RegdNo = orderId, productCategory = ProductCategory, ProductType = productType, state = state, city = city, pincode = pincode, ModuleName= ModuleName });
                                }
                                else
                                {
                                    return RedirectToAction("CreateOrder", "ABB", new { name = Name, email = EmailAddress, contactNumber = phonenumber, address = custaddress, planPrice = planAmount, RegdNo = orderId, productCategory = ProductCategory, ProductType = productType, ModuleName = ModuleName });
                                }
                            }
                        }
                    }
                    #endregion

                    #region Code to Update Zoho ABB Reg Id in database
                    if (ABBRegistrationResponseDC != null && ZohoPushFlag == "true")
                    {
                        if (ABBRegistrationResponseDC.data != null)
                        {
                            if (ABBRegId != 0 && ABBRegistrationResponseDC.data.ID != null)
                            {
                                //ABBSingleDC = ABBRegInfo.GetABBById(ABBRegistrationResponseDC.data.ID);
                                if (!string.IsNullOrEmpty(ABBRegistrationDC.Regd_No) && !string.IsNullOrEmpty(ABBRegistrationResponseDC.data.ID))
                                {
                                    ABBRegId = ABBRegInfo.UpdateABBReg(ABBRegistrationResponseDC.data.ID, ABBRegId, ABBRegistrationDC.Regd_No);
                                    ViewBag.CustomerName = ABBRegistrationDC.Cust_Name.first_name + " " + ABBRegistrationDC.Cust_Name.last_name;
                                    ViewBag.RegNo = ABBRegistrationDC.Regd_No;

                                    msg =ViewBag.CustomerName + ". Your " + model.BUName + " home appliance purchase details have been received at UTC. Your product registration referance no. " + ViewBag.RegNo;
                                    //send success sms
                                    if (smsFlag == "true")
                                    {
                                        if (model.CustMobile != null && !string.IsNullOrEmpty(ABBRegistrationDC.Regd_No))
                                            SendSucessSMS(model.CustMobile, ABBRegistrationDC.Regd_No);
                                    }

                                }
                                else
                                {
                                    msg = "Data not found in Zoho";
                                }
                            }
                            else
                            {
                                msg = "Data Not Added in Zoho";
                            }
                        }
                        else
                        {
                            msg = "Data Not Added in Zoho";
                        }
                    }
                    else
                    {
                        //send success sms
                        if (ZohoPushFlag != "true")
                        {
                            msg =model.CustFirstName + " " + model.CustLastName + ". Your " + model.BUName + " home appliance purchase details have been received at UTC. Your product registration referance no. " + model.RegdNo + ". Rest details will be e-mailed to you in a day.";
                            if (smsFlag == "true")
                            {
                                if (model.CustMobile != null && !string.IsNullOrEmpty(model.RegdNo))
                                    SendSucessSMS(model.CustMobile, model.RegdNo);
                            }
                        }

                    }

                    #endregion
                }
                return RedirectToAction("Details", "ABB", new { message = msg });
                
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBController", "ABBRegistration", ex);
                msg = ex.Message;
                return RedirectToAction("DetailsFailedOrder", "ABB", new { message = msg });
            }
        }

        public JsonResult ValidateABBOrder(string customerMobileNumber, string invoiceNumber, int modelNumber, DateTime PurchaseDate)
        {
            bool flag = false;
            ABBRegManager ABBRegInfo = new ABBRegManager();
            try
            {
                #region Code to add ABB Reg. details in database
                flag = ABBRegInfo.ValidateDuplicateABBCall(customerMobileNumber, invoiceNumber, modelNumber, PurchaseDate);
                #endregion
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBController", "ValidateABBOrder", ex);
            }
            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        public JsonResult sendByTextLocalSMS(string mobnumber)
        {
            String result;
            bool flag = false;
            TextLocalResponseViewModel TextLocalResponseVM = null;
            string apiKey = ConfigurationManager.AppSettings["SMSKey"].ToString();
            string numbers = mobnumber; //Code to trim number , remove blanks
            string OTPValue = UniqueString.RandomNumber();

            string message = "Dear Customer - OTP for registration for the UTC Assured Buyback Program is " + OTPValue + " by TUTC";
            // message = message.Replace(" [OTP]", OTPValue);
            string sender = ConfigurationManager.AppSettings["SenderName"].ToString();

            String url = "https://api.textlocal.in/send/?apikey=" + apiKey + "&numbers=" + numbers + "&message=" + message + "&sender=" + sender;
            //refer to parameters to complete correct url string

            StreamWriter myWriter = null;
            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);

            objRequest.Method = "POST";
            objRequest.ContentLength = Encoding.UTF8.GetByteCount(url);
            objRequest.ContentType = "application/x-www-form-urlencoded";
            try
            {
                myWriter = new StreamWriter(objRequest.GetRequestStream());
                myWriter.Write(url);
            }
            catch (Exception e)
            {
                LibLogging.WriteErrorToDB("ABB", "sendByTextLocalSMS", e);
                // return e.Message;
            }
            finally
            {
                myWriter.Close();
            }

            HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
            using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                TextLocalResponseVM = JsonConvert.DeserializeObject<TextLocalResponseViewModel>(result);
                if (TextLocalResponseVM != null)
                    if (TextLocalResponseVM.status == "success")
                        flag = true;

                // Close and clean up the StreamReader
                sr.Close();
            }

            if (flag == true)
            {
                _messageDetailRepository = new MessageDetailRepository();
                tblMessageDetail messageDetailObj = new tblMessageDetail();
                tblMessageDetail upmessageDetailObj = _messageDetailRepository.GetSingle(x => x.PhoneNumber == mobnumber);
                if (upmessageDetailObj != null)
                {
                    upmessageDetailObj.ResponseJSON = string.Empty;
                    upmessageDetailObj.Code = OTPValue;
                    upmessageDetailObj.PhoneNumber = mobnumber;
                    upmessageDetailObj.SendDate = DateTime.Now;
                    upmessageDetailObj.Message = message;
                    // upmessageDetailObj.MessageType = Convert.ToByte(TextMessageTypeEnum.OTP);
                    _messageDetailRepository.Update(upmessageDetailObj);
                }
                else
                {
                    messageDetailObj.ResponseJSON = string.Empty;
                    messageDetailObj.Code = OTPValue;
                    messageDetailObj.PhoneNumber = mobnumber;
                    messageDetailObj.SendDate = DateTime.Now; ;
                    messageDetailObj.Message = message;
                    // messageDetailObj.MessageType = Convert.ToByte(TextMessageTypeEnum.OTP);
                    _messageDetailRepository.Add(messageDetailObj);
                }
                _messageDetailRepository.SaveChanges();

            }
            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        public JsonResult VerifyOTP(string mobnumber, string OTP)
        {
            _messageDetailRepository = new MessageDetailRepository();
            string response = string.Empty;
            string Message = string.Empty;
            bool flag = false;

            try
            {
                tblMessageDetail messageDetail = _messageDetailRepository.GetSingle(x => x.PhoneNumber.Equals(mobnumber) && x.Code.Equals(OTP));

                DateTime start = DateTime.Now;
                if (messageDetail != null)
                {
                    DateTime oldDate = Convert.ToDateTime(messageDetail.SendDate);

                    int minDiff = Convert.ToInt32(ConfigurationManager.AppSettings["OTPActivatedMin"].ToString());
                    if (start.Subtract(oldDate) <= TimeSpan.FromMinutes(minDiff))
                        flag = true;
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("UserManager", "VerifyOTP", ex);
            }
            return Json(flag, JsonRequestBehavior.AllowGet);

        }

        public void SendSucessSMS(string mobnumber, string RegdNo)
        {
            try
            {
                String result;
                //UTCex5
                TextLocalResponseViewModel TextLocalResponseVM = null;
                string apiKey = ConfigurationManager.AppSettings["SMSKey"].ToString();
                string numbers = mobnumber; //Code to trim number , remove blanks           
                string s = " new space trim ";
                s = s.Trim();
                //string message = "Dear Customer - OTP for registration for the UTC Assured Buyback Program is " + OTPValue + " by TUTC";
                string message = "Dear Customer, You are now registered under Digi2L ABB Program with ref number "+RegdNo+". Please check your email for details. Terms and Conditions applied. Thank You, Team DIGI2L.";
                // message = message.Replace(" [OTP]", OTPValue);
                string sender = ConfigurationManager.AppSettings["SenderNameNew"].ToString();

                String url = "https://api.textlocal.in/send/?apikey=" + apiKey + "&numbers=" + numbers + "&message=" + message + "&sender=" + sender;
                //refer to parameters to complete correct url string

                StreamWriter myWriter = null;
                HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);

                objRequest.Method = "POST";
                objRequest.ContentLength = Encoding.UTF8.GetByteCount(url);
                objRequest.ContentType = "application/x-www-form-urlencoded";
                try
                {
                    myWriter = new StreamWriter(objRequest.GetRequestStream());
                    myWriter.Write(url);
                }
                catch (Exception e)
                {
                    LibLogging.WriteErrorToDB("ABB", "SendSucessSMS", e);
                    // return e.Message;
                }
                finally
                {
                    myWriter.Close();
                }

                HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
                using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    TextLocalResponseVM = JsonConvert.DeserializeObject<TextLocalResponseViewModel>(result);
                    // Close and clean up the StreamReader
                    sr.Close();
                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBController", "SendSucessSMS | Mobnumber:" + mobnumber + " | RegdNo:" + RegdNo, ex);
            }

        }

        // GET: Model Name by Prod Type Id
        [HttpGet]
        public JsonResult GetModelNumberByProdTypeId(int ProdTypeId, int buid)
        {
            _modelNumberRepository = new ModelNumberRepository(); 
            List<tblModelNumber> modelNoDC = null;
            List<SelectListItem> modelNumber = null;
            if (ProdTypeId > 0 && buid > 0)
                modelNoDC = _modelNumberRepository.GetList(x => x.ProductTypeId == ProdTypeId && x.BusinessUnitId == buid && x.IsDefaultProduct == false && x.IsActive==true).ToList();

            if (modelNoDC != null && modelNoDC.Count > 0)
            {
                modelNumber = modelNoDC.Select(projt => new SelectListItem() { Text = projt.ModelName, Value = projt.ModelNumberId.ToString() }).ToList();
            }
            else
            {
                modelNumber = new List<SelectListItem> { new SelectListItem { Text = "No model available", Value = "0" } };
            }
            var result = new SelectList(modelNumber, "Value", "Text");
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // GET: ABB/Create
        public ActionResult SelectBP(int BUId, int bpid = 0)
        {
            _masterManager = new MasterManager();
            tblBusinessPartner businessPartnerObj = new tblBusinessPartner();
            _businessPartnerRepository = new BusinessPartnerRepository();
            _businessUnitRepository = new BusinessUnitRepository();
            RedirectModel ABBObj = new RedirectModel();
            tblBusinessUnit BusinessUnitObj = new tblBusinessUnit();
            try
            {

                ABBObj.BUId = BUId;
                ABBObj.BusinessUnitId = BUId;
                BusinessUnitObj = _businessUnitRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == BUId);
                if (BusinessUnitObj != null)
                {
                    if (BusinessUnitObj.LogoName != null)
                    {
                        ABBObj.BULogoName = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Content/DB_Files/SponsorLogo/" + BusinessUnitObj.LogoName;
                    }
                    ABBObj.BusinessPartnerId = bpid;
                    ABBObj.IsBUD2C = Convert.ToBoolean(BusinessUnitObj.IsBUD2C);
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBController", "SelectBP", ex);
            }

            return View(ABBObj);
        }

        [HttpPost]
        public ActionResult SelectBP(RedirectModel ABBVM)
        {
            try
            {
                if (ABBVM.IsABB == true)
                {
                    return RedirectToAction("ProductDetails", "ABB", new { BUId = ABBVM.BUId, BusinessPartnerId = ABBVM.BusinessPartnerId, BusinessUnitId = ABBVM.BusinessUnitId });
                }
                else if (ABBVM.IsExchange == true)
                {
                    if (ABBVM.IsBUD2C == true)
                    {
                        return Redirect("~/IsDtoC/ProductDetailsForD2C?BUId=" + ABBVM.BUId + "&&BPID=" + ABBVM.BusinessPartnerId);
                    }
                    else
                    {
                        return Redirect("~/Exchange/SelectBP?BUId=" + ABBVM.BUId);
                    }

                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBController", "SelectBP", ex);
            }
            return View(ABBVM);
        }
        // get City list
        [HttpGet]
        public JsonResult GetCityByStateName(string stateName, int buid)
        {
            _businessPartnerRepository = new BusinessPartnerRepository();
            IEnumerable<SelectListItem> cityList = null;
            List<tblBusinessPartner> businessPartnerList = null;
            try
            {
                //businessPartnerObj = _businessPartnerRepository.GetSingle(x => x.IsActive == true && x.State.ToLower().Equals(stateName.ToLower()));
                DataTable dt = _businessPartnerRepository.GetCityList();
                if (dt != null && dt.Rows.Count > 0)
                {
                    businessPartnerList = GenericConversionHelper.DataTableToList<tblBusinessPartner>(dt);
                    //businessPartnerList = businessPartnerList.Where(x => x.BusinessUnitId == buid).ToList();
                    businessPartnerList = businessPartnerList.Where(x => x.BusinessUnitId == buid && x.State != null && x.State.ToLower().Equals(stateName.ToLower())).ToList();

                }

                List<string> states = businessPartnerList.Where(x => x.BusinessUnitId == buid
                && x.State.ToLower().Equals(stateName.ToLower())).OrderBy(o => o.City)
                                                                    .Select(x => x.City.Trim())
                                                                    .Distinct().ToList();
                List<SelectListItem> cityListItems = states.Select(x => new SelectListItem
                {
                    Text = x,
                    Value = x
                }).ToList();
                cityList = new SelectList(cityListItems, "Text", "Text");

                //cityList = (businessPartnerList).AsEnumerable().Select(prodt => new SelectListItem() { Text = prodt.City, Value = prodt.City });

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBController", "GetCityByStateName", ex);
                //LibLogging.WriteErrorLog("UserManager", "VerifyOTP", ex);
            }
            var result = new SelectList(cityList, "Value", "Text");
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // get City list
        [HttpGet]
        public JsonResult GetStoreNameCityName(string cityName)
        {
            _businessPartnerRepository = new BusinessPartnerRepository();
            IEnumerable<SelectListItem> StoreList = null;
            //tblBusinessPartner businessPartnerObj = null;
            List<tblBusinessPartner> businessPartnerList = null;
            // List<tblBusinessPartner> businessPartnerNewList = null;
            try
            {
                businessPartnerList = _businessPartnerRepository.GetList(x => x.IsActive == true && x.BusinessUnitId == 1 && x.City.ToLower().Equals(cityName.ToLower())).ToList();

                StoreList = (businessPartnerList).AsEnumerable().Select(prodt => new SelectListItem() { Text = prodt.Description, Value = prodt.BusinessPartnerId.ToString() });

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBController", "GetStoreNameCityName", ex);
                //LibLogging.WriteErrorLog("UserManager", "VerifyOTP", ex);
            }
            var result = new SelectList(StoreList, "Value", "Text");
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TermAndCondition()
        {
            return View();
        }

        #region RazorPay Code
        [HttpGet]
        public ActionResult CreateOrder(paymentInitialViewModel paymentModel)
        {
            PaymentInitiateModel payment = new PaymentInitiateModel();
            string msg = null;
            try
            {
                _productCategoryRepository = new ProductCategoryRepository();
                _productTypeRepository = new ProductTypeRepository();
                
                string ModuleEnum = ExchangeOrderManager.GetEnumDescription(ABBPlanEnum.ModuleERP);
                if (paymentModel != null)
                {
                    string KeyString = ConfigurationManager.AppSettings["EncryptionKey"].ToString();
                    string IV = ConfigurationManager.AppSettings["EncryptionIV"].ToString();
                    payment.Custname = AES256encoding.DecryptString(paymentModel.name, KeyString, IV);
                    payment.CustcontactNumber = AES256encoding.DecryptString(paymentModel.contactNumber, KeyString, IV);
                    payment.Custemail = AES256encoding.DecryptString(paymentModel.email, KeyString, IV);
                    payment.planamount = Convert.ToDecimal(AES256encoding.DecryptString(paymentModel.planPrice, KeyString, IV));
                    payment.Custaddress = AES256encoding.DecryptString(paymentModel.address, KeyString, IV);
                    payment.orderRegdNo = AES256encoding.DecryptString(paymentModel.RegdNo, KeyString, IV);
                    if (!string.IsNullOrEmpty(paymentModel.ModuleName))
                    {
                        payment.ModuleType = AES256encoding.DecryptString(paymentModel.ModuleName, KeyString, IV);
                        payment.ModuleTypeEnum = ModuleEnum;
                    }
                    paymentModel.productcategoryId = Convert.ToInt32(AES256encoding.DecryptString(paymentModel.productCategory, KeyString, IV));
                    paymentModel.productTypeId = Convert.ToInt32(AES256encoding.DecryptString(paymentModel.ProductType, KeyString, IV));
                    tblProductCategory productCategory = _productCategoryRepository.GetSingle(x => x.Id == paymentModel.productcategoryId);
                    if (productCategory != null)
                    {
                        tblProductType productTypeObj = _productTypeRepository.GetSingle(x => x.Id == paymentModel.productTypeId);
                        if (productTypeObj != null)
                        {
                            payment.newproductCategory = productCategory.Description;
                            payment.newProductType = productTypeObj.Description;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                LibLogging.WriteErrorToDB("ABB", "CreateOrder", ex);
                msg = ex.Message;
                return RedirectToAction("DetailsFailedOrder", "ABB", new { message = msg });
            }
            return View(payment);
        }

        [HttpPost]
        public ActionResult CreateOrder(PaymentInitiateModel _requestData)
        {
            ABBRegManager ABBRegInfo = new ABBRegManager();
            _paymentRepository = new ABBPaymentRepository();
            _ABBRegistrationRepository = new ABBRegistrationRepository();
            // Generate random receipt number for order
            OrderModel orderModel = new OrderModel();
            string fileName = string.Empty;
            DateTime _dateTime = DateTime.Now;
            string Message = null;
           
            try
            {
                if (_requestData.ModuleType == _requestData.ModuleTypeEnum)
                {
                    if (_requestData.Base64StringValue != null)
                    {
                        byte[] bytes = System.Convert.FromBase64String(_requestData.Base64StringValue);
                        fileName = _requestData.orderRegdNo + _dateTime.ToString("yyyyMMddHHmmssFFF") + Path.GetExtension("image.jpeg");
                        string rootPath = @HostingEnvironment.ApplicationPhysicalPath;
                        string filePath = ConfigurationManager.AppSettings["InvoiceImage"].ToString() + fileName;
                        System.IO.File.WriteAllBytes(rootPath + filePath, bytes);
                        _requestData.InvoiceImage = fileName;
                        string status = ABBRegInfo.UpdateInvoiceDetailsABB(_requestData);

                    }
                    else
                    {
                        Message = "Invoice image is not provided please provide invoice image";
                        return RedirectToAction("DetailsFailedOrder", "ABB", new { message = Message });
                    }
                }
                string transactionId = _requestData.orderRegdNo;
                ZaakpayKey apikey = new ZaakpayKey();
                apikey = ABBRegInfo.getPaymentKeyzaakpay();
                // Create order model for return on view
                {
                    TempData["RegNo"] = _requestData.orderRegdNo;
                }

                // Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient(apikey.razorPayKey, apikey.secretKey);
                string returnurl = ConfigurationManager.AppSettings["BaseURL"].ToString() + "ABB/Complete";
                double amount = Convert.ToDouble(_requestData.planamount);
                string amountnew = String.Format("{0:0.00}", amount);
                amount = Convert.ToDouble(amountnew);
                Dictionary<string, string> options = new Dictionary<string, string>();
                options.Add("amount", (amount * 100).ToString());  // Amount will in paise
                options.Add("buyerEmail", _requestData.Custemail);  // Amount will in paise
                options.Add("currency", "INR");
                options.Add("merchantIdentifier", apikey.merchantId);
                options.Add("orderId", _requestData.orderRegdNo);
                options.Add("returnUrl", returnurl);
                string allparameters = ChecksumCalculator.generateSignature(options);
                allparameters = allparameters + "&";
                string checksum = ChecksumCalculator.calculateChecksum(apikey.secretKey, allparameters);
                bool flag = ChecksumCalculator.verifyChecksum(apikey.secretKey, allparameters, checksum);
                string ZaakPayUrl = ConfigurationManager.AppSettings["ZaakPayUrl"].ToString().Trim() + "?" + allparameters + "checksum=" + checksum;
                return Redirect(ZaakPayUrl);
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBController", "CreateOrder", ex);
                Message = "Invoice image is not provided please provide invoice image";
                return RedirectToAction("DetailsFailedOrder", "ABB", new { message = Message });
            }
        }

        [HttpPost]
        public ActionResult Complete(ZaakPayResponseModel zaakPayResponseModel)
        { 
            string msg = string.Empty;
            try
            {
                _ABBRegistrationRepository = new ABBRegistrationRepository();
                _businessUnitRepository = new BusinessUnitRepository();
                ABBRegManager ABBRegInfo = new ABBRegManager();
                string dbresponse = string.Empty;
                _paymentRepository = new ABBPaymentRepository();
               
                string SendSMSFlag = ConfigurationManager.AppSettings["sendsmsflag"].ToString();
                //Payment Response ffrom ZaakpAy 
                PaymentResponseModel response = new PaymentResponseModel();

                response.amount = Convert.ToDecimal(zaakPayResponseModel.amount);
                response.amount = (response.amount) / 100;
                response.OrderId = zaakPayResponseModel.orderId;
                response.transactionId = zaakPayResponseModel.pgTransId;
                response.status = zaakPayResponseModel.responseDescription;
                response.responseDescription = zaakPayResponseModel.responseDescription;
                response.responseCode = zaakPayResponseModel.responseCode;
                response.paymentmethod = zaakPayResponseModel.paymentmethod;
                response.cardToken = zaakPayResponseModel.cardToken;
                response.bank = zaakPayResponseModel.bank;
                response.bankid = zaakPayResponseModel.bankid;
                response.cardId = zaakPayResponseModel.cardId;
                response.cardhashId = zaakPayResponseModel.cardhashId;
                response.paymentMode = zaakPayResponseModel.paymentMode;
                response.cardScheme = zaakPayResponseModel.cardScheme;
                response.checksum = zaakPayResponseModel.checksum;
                response.RegdNo = zaakPayResponseModel.orderId;
                dbresponse = ABBRegInfo.PaymentstatusUpdateZaakPay(response);
                //// Check payment made successfully

                if (zaakPayResponseModel.responseCode == Convert.ToInt32(ZaakPayPaymentStatus.successfull))
                {
                    tblABBRegistration registrationObj = _ABBRegistrationRepository.GetSingle(x => x.RegdNo == response.RegdNo);
                    if (registrationObj != null)
                    {
                        tblBusinessUnit bussinessUnit = _businessUnitRepository.GetSingle(x => x.BusinessUnitId == registrationObj.BusinessUnitId);
                        if (bussinessUnit != null)
                        {
                            if (dbresponse == "success" && registrationObj != null)
                            {
                                msg = "Woohoo! We're excited to let you know that your product registration reference number is " + registrationObj.RegdNo + ", and your transaction ID is " + response.transactionId + ". Your order ID is " + response.OrderId + ".We can't wait to share more details with you! Keep an eye out for an email with all the information you need within the next 24 hours. Thanks for choosing Digi2L !";
                                if (SendSMSFlag == "true")
                                {
                                    if (registrationObj.CustMobile != null && !string.IsNullOrEmpty(registrationObj.RegdNo))
                                        SendSucessSMS(registrationObj.CustMobile, registrationObj.RegdNo);
                                }
                            }
                            else
                            {
                                msg = "Woohoo! We're excited to let you know that your product registration reference number is " + registrationObj.RegdNo + ", and your transaction ID is " + response.transactionId + ". Your order ID is " + response.OrderId + ".We can't wait to share more details with you! Keep an eye out for an email with all the information you need within the next 24 hours. Thanks for choosing Digi2L !";
                                if (SendSMSFlag == "true")
                                {
                                    if (registrationObj.CustMobile != null && !string.IsNullOrEmpty(registrationObj.RegdNo))
                                        SendSucessSMS(registrationObj.CustMobile, registrationObj.RegdNo);
                                }
                            }
                        }
                    }
                }
                else
                {
                    tblABBRegistration registrationObj = _ABBRegistrationRepository.GetSingle(x => x.RegdNo == response.RegdNo);
                    if (registrationObj != null)
                    {
                        tblBusinessUnit bussinessUnit = _businessUnitRepository.GetSingle(x => x.BusinessUnitId == registrationObj.BusinessUnitId);
                        if (bussinessUnit != null)
                        {
                            if (SendSMSFlag == "true")
                            {
                                if (registrationObj.CustMobile != null && !string.IsNullOrEmpty(registrationObj.RegdNo))
                                    SendSucessSMS(registrationObj.CustMobile, registrationObj.RegdNo);
                            }
                        }
                    }
                    msg = "Payment Is Failed  transactionId  " + response.transactionId + " orderId  " + response.OrderId;
                    return RedirectToAction("DetailsFailedOrder", "ABB", new { message = msg });
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBController", "Complete", ex);
                msg = ex.Message;
                return RedirectToAction("DetailsFailedOrder", "ABB", new { message = msg });
            }
            return RedirectToAction("Details", "ABB", new { message = msg });
        }

        [HttpPost]
        public ActionResult CompletePl(OrderModel orderModel)
        {
            GetpaymentStatusResponse paymentResponse = new GetpaymentStatusResponse();
            ABBRegManager ABBRegInfo = new ABBRegManager();
            _ABBRegistrationRepository = new ABBRegistrationRepository();
            _businessUnitRepository = new BusinessUnitRepository();
            string Message = null;
            string Orderstatus = null;
            string dbresponse = string.Empty;
            int amount = 0;
            string smsFlag = null;
            try
            {
                smsFlag = ConfigurationManager.AppSettings["sendsmsflag"].ToString();
                //Code to get error message if any
                string errorCode = orderModel.errorcode;
                //Code to get error message if any
                string errorMessage = orderModel.errrorResponse;
                //Code to get Payment Id 
                string PaymentId = orderModel.PaymentId;
                //Code to get Order Id 
                string OrderId = orderModel.OrderId;
                #region success Response
                if (!string.IsNullOrEmpty(PaymentId) && !string.IsNullOrEmpty(OrderId) && PaymentId != "undefined" && OrderId != "undefined")
                {

                    paymentResponse = ABBRegInfo.GetpaymentStatus(PaymentId, OrderId);
                    PaymentResponseModel response = new PaymentResponseModel();
                    if (paymentResponse.payment_info_data.captured_amount_in_paisa != null)
                    {
                        amount = Convert.ToInt32(paymentResponse.payment_info_data.captured_amount_in_paisa);
                        amount = amount / 100;
                        response.amount = Convert.ToDecimal(amount);
                    }
                    else
                    {
                        response.amount = 0;
                    }
                    response.paymentMode = paymentResponse.payment_info_data.payment_mode;
                    response.paymentmethod = paymentResponse.payment_info_data.payment_mode;
                    response.gatewayTransactionId = paymentResponse.payment_info_data.gateway_payment_id;
                    response.OrderId = paymentResponse.merchant_data.order_id;
                    response.transactionId = paymentResponse.payment_info_data.payment_id;
                    response.responseDescription = paymentResponse.payment_info_data.payment_response_message;
                    response.responseCode = paymentResponse.payment_info_data.payment_response_code;
                    response.RegdNo = TempData["RegNo"].ToString();
                    response.pgTransTime = paymentResponse.payment_info_data.payment_completion_date_time.ToString();
                    response.status = paymentResponse.payment_info_data.payment_status;
                    response.orderStatus = paymentResponse.order_data.order_status;
                    dbresponse = ABBRegInfo.PaymentstatusUpdate(response);

                    //// Check payment made successfully
                    string ResponseForSuccessfullPayment = null;
                    Orderstatus = ExchangeOrderManager.GetEnumDescription(PluralEnum.OrderStatus);
                    ResponseForSuccessfullPayment = ExchangeOrderManager.GetEnumDescription(PluralEnum.PaymentStatus);
                    if (paymentResponse.payment_info_data.payment_status == ResponseForSuccessfullPayment && paymentResponse.order_data.order_status == Orderstatus)
                    {
                        tblABBRegistration registrationObj = _ABBRegistrationRepository.GetSingle(x => x.RegdNo == response.RegdNo);
                        if (registrationObj != null)
                        {
                            tblBusinessUnit bussinessUnit = _businessUnitRepository.GetSingle(x => x.BusinessUnitId == registrationObj.BusinessUnitId);
                            if (bussinessUnit != null)
                            {
                                if (dbresponse == "success" && registrationObj != null)
                                {

                                    Message = "Woohoo! We're excited to let you know that your product registration reference number is " + registrationObj.RegdNo + ", and your transaction ID is " + response.transactionId + ". Your order ID is " + response.OrderId + ".We can't wait to share more details with you! Keep an eye out for an email with all the information you need within the next 24 hours. Thanks for choosing Digi2L !";

                                    if (smsFlag == "true")
                                    {
                                        if (registrationObj.CustMobile != null && !string.IsNullOrEmpty(registrationObj.RegdNo))
                                            SendSucessSMS(registrationObj.CustMobile, registrationObj.RegdNo);
                                    }
                                }
                                else
                                {
                                    Message = "Woohoo! We're excited to let you know that your product registration reference number is " + registrationObj.RegdNo + ", and your transaction ID is " + response.transactionId + ". Your order ID is " + response.OrderId + ".We can't wait to share more details with you! Keep an eye out for an email with all the information you need within the next 24 hours. Thanks for choosing Digi2L !";
                                    if (smsFlag == "true")
                                    {
                                        if (registrationObj.CustMobile != null && !string.IsNullOrEmpty(registrationObj.RegdNo))
                                            SendSucessSMS(registrationObj.CustMobile, registrationObj.RegdNo);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        tblABBRegistration registrationObj = _ABBRegistrationRepository.GetSingle(x => x.RegdNo == response.RegdNo && x.IsActive==true);
                        if (registrationObj != null)
                        {
                            tblBusinessUnit bussinessUnit = _businessUnitRepository.GetSingle(x => x.BusinessUnitId == registrationObj.BusinessUnitId && x.IsActive==true);
                            if (bussinessUnit != null)
                            {
                                if (smsFlag == "true")
                                {
                                    if (registrationObj.CustMobile != null && !string.IsNullOrEmpty(registrationObj.RegdNo))
                                        SendSucessSMS(registrationObj.CustMobile, registrationObj.RegdNo);
                                }
                            }
                        }
                        Message = "Payment Failed  transactionId  " + response.transactionId + " orderId  " + response.OrderId;
                        return RedirectToAction("DetailsFailedOrder", "ABB", new { message = Message });
                    }
                }
                #endregion
                #region Failure Response
                else if ((!string.IsNullOrEmpty(OrderId) && string.IsNullOrEmpty(PaymentId)) || (!string.IsNullOrEmpty(OrderId) && !string.IsNullOrEmpty(PaymentId)))
                {
                    PaymentResponseModel response = new PaymentResponseModel();
                    response.OrderId = OrderId;
                    response.transactionId = PaymentId;
                    response.responseDescription = errorMessage;
                    response.responseCode = Convert.ToInt32(errorCode);
                    response.RegdNo = TempData["RegNo"].ToString();
                    response.status = errorMessage;
                    response.orderStatus = errorMessage;
                    response.amount = orderModel.amount;
                    dbresponse = ABBRegInfo.PaymentstatusUpdate(response);

                    // Check payment made successfully

                    tblABBRegistration registrationObj = _ABBRegistrationRepository.GetSingle(x => x.RegdNo == response.RegdNo);
                    if (registrationObj != null)
                    {
                        tblBusinessUnit bussinessUnit = _businessUnitRepository.GetSingle(x => x.BusinessUnitId == registrationObj.BusinessUnitId);
                        if (bussinessUnit != null)
                        {
                            if (dbresponse == "success" && registrationObj != null)
                            {

                                Message = "Thank you your order has been recieved but Payment is not received Regd.No" + registrationObj.RegdNo;

                                if (smsFlag == "true")
                                {
                                    if (registrationObj.CustMobile != null && !string.IsNullOrEmpty(registrationObj.RegdNo))
                                        SendSucessSMS(registrationObj.CustMobile, registrationObj.RegdNo);
                                }
                            }
                            else
                            {
                                Message = "Thank you your order has been recieved but Payment is not received Regd.No" + registrationObj.RegdNo;
                                if (smsFlag == "true")
                                {
                                    if (registrationObj.CustMobile != null && !string.IsNullOrEmpty(registrationObj.RegdNo))
                                        SendSucessSMS(registrationObj.CustMobile, registrationObj.RegdNo);
                                }
                            }
                        }
                    }
                    if (smsFlag == "true")
                    {
                        if (registrationObj.CustMobile != null && !string.IsNullOrEmpty(registrationObj.RegdNo))
                            SendSucessSMS(registrationObj.CustMobile, registrationObj.RegdNo);
                    }
                    return RedirectToAction("Details", "ABB", new { message = Message });
                }
                #endregion
                else
                {
                    Message = "Order received could not initiate payment at the moment please contact to administrator";
                    return RedirectToAction("DetailsFailedOrder", "ABB", new { message = Message });
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBController", "Complete", ex);
                Message =ex.Message;
                return RedirectToAction("DetailsFailedOrder", "ABB", new { message = Message });
            }
            return RedirectToAction("Details", "ABB", new { message = Message });
        }
        #endregion
        #region Get PLan Price
        [HttpGet]
        public JsonResult GetPlanPrice(int productCatId, int productSubCatId, int buid, string productPrice, int GstType)
        {
            _masterManager = new BAL.SponsorsApiCall.MasterManager();
            plandetail plandetails = new plandetail();
            try
            {
                if (productCatId > 0 && productSubCatId > 0 && buid > 0 && productPrice != null || productPrice != string.Empty)
                {
                    plandetails = _masterManager.GetABBPlanPrice(productCatId, productSubCatId, buid, productPrice, GstType);
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBController", "GetPlanPrice", ex);
            }
            return Json(plandetails, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region get isdefferedabb Or Not
        [HttpGet]
        public bool GetdealerInfo(int businesspartnerId)
        {
            _masterManager = new MasterManager();
            bool flag = false;
            try
            {
                if (businesspartnerId > 0)
                {
                    flag = _masterManager.GetdealerIsAbb(businesspartnerId);
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBController", "GetdealerInfo", ex);
            }
            return flag;
        }
        #endregion
        #region Product Details For ABB Plan
        [HttpGet]
        public ActionResult ProductDetails(RedirectModel businessunit)  
        {
            _businessPartnerRepository = new BusinessPartnerRepository();
            _businessUnitRepository = new BusinessUnitRepository();
            _productCategoryMappingRepository = new ProductCategoryMappingRepository();
            _productTypeRepository = new ProductTypeRepository();
            _productCategoryRepository = new ProductCategoryRepository();
            ABBViewModel productViewModel = new ABBViewModel();
            string msg = null;
            try
            {
                if (businessunit.BUId > 0)
                {
                    if (businessunit.BusinessPartnerId > 0)
                    {
                        tblBusinessPartner businessParetnerObj = _businessPartnerRepository.GetSingle(x => x.BusinessPartnerId == businessunit.BusinessPartnerId && x.IsActive == true && x.IsABBBP == true && x.BusinessUnitId == businessunit.BUId);
                        if (businessParetnerObj != null)
                        {
                            if (businessParetnerObj != null)
                            {
                                productViewModel.IsdeferredABB = Convert.ToBoolean(businessParetnerObj.IsDefferedAbb);
                            }
                            else
                            {
                                productViewModel.IsdeferredABB = true;
                            }
                        }
                        else
                        {
                            msg = "business partner details not found";
                            return RedirectToAction("DetailsFailedOrder", "ABB", new { message = msg });
                        }
                    }
                    else
                    {
                        productViewModel.IsdeferredABB = true;
                    }


                    List<tblProductCategory> prodGroupListForABB = new List<tblProductCategory>();

                    List<tblBUProductCategoryMapping> productCategoryForNew = new List<tblBUProductCategoryMapping>();

                    DataTable dt = _productCategoryMappingRepository.GetNewProductCategoryForABB(businessunit.BUId);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        productCategoryForNew = GenericConversionHelper.DataTableToList<tblBUProductCategoryMapping>(dt);
                    }
                    else
                    {
                        msg = "product category details for new product is not provided";
                        return RedirectToAction("DetailsFailedOrder", "ABB", new { message = msg });
                    }
                    if (productCategoryForNew.Count > 0)
                    {
                        foreach (var productCategory in productCategoryForNew)
                        {
                            tblProductCategory productObj = _productCategoryRepository.GetSingle(x => x.Id == productCategory.ProductCatId && x.IsActive == true && x.Description_For_ABB != null);
                            if (productObj != null)
                            {
                                prodGroupListForABB.Add(productObj);
                            }
                            else
                            {
                                msg = "product category not found for category id="+ productCategory.ProductCatId;
                                return RedirectToAction("DetailsFailedOrder", "ABB", new { message = msg });
                            }
                        }
                    }
                    else
                    {
                        msg = "product category details for new product is not provided";
                        return RedirectToAction("DetailsFailedOrder", "ABB", new { message = msg });
                    }
                    if (prodGroupListForABB != null && prodGroupListForABB.Count > 0)
                    {
                        ViewBag.ProductCategoryList = new SelectList(prodGroupListForABB, "Id", "Description_For_ABB");
                    }
                    else
                    {
                        msg = "product category details for new product is not provided";
                        return RedirectToAction("DetailsFailedOrder", "ABB", new { message = msg });
                    }
                    tblBusinessUnit businessUnitObj = _businessUnitRepository.GetSingle(x => x.BusinessUnitId == businessunit.BUId && x.IsActive==true && x.IsABB==true);
                    if (businessUnitObj != null)
                    {
                        if (businessUnitObj.LogoName != null)
                        {
                            productViewModel.BusinessLogo = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Content/DB_Files/SponsorLogo/" + businessUnitObj.LogoName;
                            productViewModel.BULogoName = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Content/DB_Files/SponsorLogo/" + businessUnitObj.LogoName;
                        }
                        else
                        {
                            productViewModel.BusinessLogo = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Content/DB_Files/SponsorLogo/Digi2L_Logo_Final.png";
                        }
                        productViewModel.Buname = businessUnitObj.Name;
                        productViewModel.IsBUD2C = Convert.ToBoolean(businessUnitObj.IsBUD2C);
                        productViewModel.ShowAbbPlan = Convert.ToBoolean(businessUnitObj.ShowAbbPlan);
                        productViewModel.MarginType = Convert.ToInt32(businessUnitObj.MarginType);
                        productViewModel.GstType = Convert.ToInt32(businessUnitObj.GSTType);
                    }
                    else
                    {
                        msg = "business unit not found check whether busiess unit is active and isabb is true ";
                        return RedirectToAction("DetailsFailedOrder", "ABB", new { message = msg });
                    }
                    productViewModel.ProductType = new List<SelectListItem>();
                    productViewModel.BusinessPartnerId = businessunit.BusinessPartnerId;
                    productViewModel.BusinessUnitId = businessunit.BUId;
                }
                else
                {
                    msg = "business unit not provided ";
                    return RedirectToAction("DetailsFailedOrder", "ABB", new { message = msg });
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBController", "ProductDetails", ex);
                msg = ex.InnerException.ToString();
                return RedirectToAction("DetailsFailedOrder", "ABB", new { message = msg });
            }
            return View(productViewModel);
        }
        [HttpPost]
        public ActionResult ProductDetails(ABBViewModel productdetails)
        {
            string msg = null;
            try
            {
                if (productdetails != null)
                {
                    if (productdetails.BusinessPartnerId > 0 && productdetails.BusinessUnitId > 0)
                    {
                        return RedirectToAction("ABBRegistration", "ABB", new { NetProductPrice = productdetails.NetProductPrice, productCategoryId = productdetails.productCategoryId, ProductTypeId = productdetails.ProductTypeId, BusinessUnitId = productdetails.BusinessUnitId, Buname = productdetails.Buname, BusinessPartnerId = productdetails.BusinessPartnerId, planprice = productdetails.planprice, planduration = productdetails.planduration, planName = productdetails.planName, NoClaimPeriod = productdetails.NoClaimPeriod, IsBUD2C = productdetails.IsBUD2C, BaseValue = productdetails.BaseValue, Cgst = productdetails.Cgst, Sgst = productdetails.Sgst });
                    }

                    else
                    {
                        return RedirectToAction("SelectLocation", "ABB", new { NetProductPrice = productdetails.NetProductPrice, productCategoryId = productdetails.productCategoryId, ProductTypeId = productdetails.ProductTypeId, BusinessUnitId = productdetails.BusinessUnitId, Buname = productdetails.Buname, BusinessPartnerId = productdetails.BusinessPartnerId, planprice = productdetails.planprice, planduration = productdetails.planduration, planName = productdetails.planName, NoClaimPeriod = productdetails.NoClaimPeriod, IsBUD2C = productdetails.IsBUD2C , BaseValue=productdetails.BaseValue, Cgst=productdetails.Cgst, Sgst=productdetails.Sgst});
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBController", "ProductDetails", ex);
                msg = ex.Message;
                return RedirectToAction("DetailsFailedOrder", "ABB", new { message = msg });
            }
            return View(productdetails);
        }
        #endregion
        #region select Location
        [HttpGet]
        public ActionResult SelectLocation(ABBViewModel productdetails)
        {
            string msg = string.Empty;
            _masterManager = new MasterManager();
            List<tblBusinessPartner> businessPartnerList = null;
            tblBusinessPartner businessPartnerObj = new tblBusinessPartner();
            pinCodeRepository = new PinCodeRepository();
            _businessPartnerRepository = new BusinessPartnerRepository();
            _businessUnitRepository = new BusinessUnitRepository();
            ABBViewModel ABBObj = new ABBViewModel();
            tblBusinessUnit BusinessUnitObj = new tblBusinessUnit();
            try
            {
                ABBObj.CityList = new List<SelectListItem>();
                ABBObj.StoreList = new List<SelectListItem>();
                ABBObj.BusinessPartnerId = productdetails.BusinessPartnerId;
                ABBObj.productCategoryId = productdetails.productCategoryId;
                ABBObj.NetProductPrice = productdetails.NetProductPrice;
                ABBObj.NoClaimPeriod = productdetails.NoClaimPeriod;
                ABBObj.planduration = productdetails.planduration;
                ABBObj.planName = productdetails.planName;
                ABBObj.planprice = productdetails.planprice;
                ABBObj.BaseValue = productdetails.BaseValue;
                ABBObj.Cgst = productdetails.Cgst;
                ABBObj.Sgst = productdetails.Sgst;
                ABBObj.ProductTypeId = productdetails.ProductTypeId;
                ABBObj.IsBUD2C = productdetails.IsBUD2C;
                ABBObj.BusinessUnitId = productdetails.BusinessUnitId;
                ABBObj.bussinessUnitenum = Convert.ToInt32(BusinessUnitEnum.Havells);
                BusinessUnitObj = _businessUnitRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == productdetails.BusinessUnitId);
                if (BusinessUnitObj != null)
                {
                    if (BusinessUnitObj.LogoName != null)
                    {
                        ABBObj.BULogoName = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Content/DB_Files/SponsorLogo/" + BusinessUnitObj.LogoName;
                    }
                }
                // state list
                DataTable dt = _businessPartnerRepository.GetStateListABB();
                if (dt != null && dt.Rows.Count > 0)
                {
                    businessPartnerList = GenericConversionHelper.DataTableToList<tblBusinessPartner>(dt);
                    List<string> states = businessPartnerList.Where(x => x.BusinessUnitId == ABBObj.BusinessUnitId && (x.IsABBBP != null && x.IsABBBP == true)).OrderBy(o => o.State).Select(x => x.State).Distinct().ToList();
                    List<SelectListItem> stateListItems = states.Select(x => new SelectListItem
                    {
                        Text = x,
                        Value = x
                    }).ToList();
                    ViewBag.StateList = new SelectList(stateListItems, "Text", "Text");

                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBController", "SelectLocation", ex);
                msg = ex.Message;
                return RedirectToAction("DetailsFailedOrder", "ABB", new { message = msg });
            }

            return View(ABBObj);
        }
        #endregion
        #region Abb PlanDetails And 
        [HttpGet]
        public JsonResult GetPlanDetails(int productCatId, int productSubCatId, int buid, string productPrice)
        {
            _masterManager = new MasterManager();
            var planObj = _masterManager.getabbplandetails(productCatId, productSubCatId, buid, productPrice);
            return Json(planObj, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region City And pincode
        [HttpPost]
        public JsonResult GetPincodeForABB(string pintext,int? buid)
        {
            pinCodeRepository = new PinCodeRepository();
            IEnumerable<SelectListItem> pincodeList = null;
            List<tblPinCode> pincodeMasterList = null;
            try
            {
                DataTable dt = pinCodeRepository.GetPincodeListbybuidforABB(pintext, buid);
                pincodeMasterList = GenericConversionHelper.DataTableToList<tblPinCode>(dt);
                if (pincodeMasterList.Count > 0)
                {
                    pincodeList = pincodeMasterList.Select(x => new SelectListItem
                    {
                        Text = x.ZipCode.ToString(),
                        Value = x.ZipCode.ToString()
                    }).ToList();
                    pincodeList = pincodeList.OrderBy(o => o.Value).ToList();
                }
                else
                {
                    pincodeList = new List<SelectListItem> { new SelectListItem { Text = "No pincode available on this location", Value = "0" } };
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABB", "GetPincodeForABB", ex);
            }
            var result = new SelectList(pincodeList, "Value", "Text");
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetStatecityabb(string pincode)
        {
            pinCodeRepository = new PinCodeRepository();
            List<tblPinCode> pincodeMasterList = null;
            abbCustAddress address = new abbCustAddress();
            try
            {
                int pin = Convert.ToInt32(pincode);
                DataTable dt = pinCodeRepository.GetABBSateAndCityABB(pin);
                if (dt != null && dt.Rows.Count > 0)
                {
                    pincodeMasterList = GenericConversionHelper.DataTableToList<tblPinCode>(dt);
                }
                if (pincodeMasterList != null)
                {
                    foreach (var item in pincodeMasterList)
                    {
                        address.StateName = item.State;
                        address.CityName = item.Location;
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABB", "GetState", ex);
            }

            return Json(address, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region
        public JsonResult ABBBusinessPartnerList(int Buid, string city)
        {
            _businessPartnerRepository = new BusinessPartnerRepository();
            List<SelectListItem> StoreList = null;
            List<tblBusinessPartner> businessPartnerList = new List<tblBusinessPartner>();
            try
            {
                if (Buid > 0 && city != "")
                {
                    businessPartnerList = _businessPartnerRepository.GetList(x => x.IsActive == true && (x.IsABBBP != null && x.IsABBBP == true) && x.BusinessUnitId == Buid && x.City != null && x.City.ToLower().Equals(city.ToLower())).ToList();
                    StoreList = businessPartnerList.Select(x => new SelectListItem
                    {
                        Text = x.Description,
                        Value = x.BusinessPartnerId.ToString()
                    }).ToList();
                    ViewBag.businessPartnerlist = new SelectList(businessPartnerList, "BusinessPartnerId", "Description");

                }
                else
                {
                    StoreList = businessPartnerList.Select(x => new SelectListItem
                    {
                        Text = "No Store Available",
                        Value = "0"
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABB", "ABBBusinessPartnerList", ex);
            }
            var result = new SelectList(StoreList, "Value", "Text");
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region
        public JsonResult ValidateImage(string imageData, string ImageName)
        {
            bool flag = false;
            OCR_Invoice_Validator ocrInvoiceValidatorCall = new OCR_Invoice_Validator();
            try
            {
                // Image image;
                if (!string.IsNullOrEmpty(imageData))
                {
                    byte[] bytes = Convert.FromBase64String(imageData);
                    flag = ocrInvoiceValidatorCall.Invoice_OCR(bytes);
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABB", "ValidateImage", ex);
            }
            return Json(flag, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region 
        public JsonResult GetProductCategoryForABB(int buid, int prodCatid)
        {
            _abbPlanMaster = new ABBPlanMasterRepository();
            _productTypeRepository = new ProductTypeRepository();
            List<tblABBPlanMaster> planmasterObj = new List<tblABBPlanMaster>();
            List<tblProductType> ProductTypeList = new List<tblProductType>();
            tblProductType ProductTypeobj = new tblProductType();
            IEnumerable<SelectListItem> prodType = null;
            List<SelectListItem> ProductTypeListForNull = new List<SelectListItem>();
            try
            {
                DataTable dt = new DataTable();
                dt = _abbPlanMaster.GetNewProductCategoryForABB(buid, prodCatid);
                if (dt != null && dt.Rows.Count > 0)
                {
                    planmasterObj = GenericConversionHelper.DataTableToList<tblABBPlanMaster>(dt);
                    if (planmasterObj.Count > 0)
                    {
                        foreach (var item in planmasterObj)
                        {
                            ProductTypeobj = _productTypeRepository.GetSingle(x => x.Id == item.ProductTypeId && x.IsAllowedForNew == true && x.IsActive == true && x.Size == null);
                            if (ProductTypeobj != null)
                            {
                                ProductTypeList.Add(ProductTypeobj);
                            }
                        }

                        if (ProductTypeList != null && ProductTypeList.Count > 0)
                        {

                            //prodTypeListForABB.RemoveAt(0);
                            prodType = (ProductTypeList).AsEnumerable().Select(prodt => new SelectListItem() { Text = prodt.Description_For_ABB, Value = prodt.Id.ToString() });
                            prodType = prodType.OrderBy(x=>x.Text);
                        }
                    }
                    else
                    {
                        prodType = new List<SelectListItem> { new SelectListItem { Text = "No product type available", Value = "0" } };
                    }
                }
                else
                {
                    prodType = new List<SelectListItem> { new SelectListItem { Text = "No product type available", Value = "0" } };
                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABB", "GetProductCategoryForABB", ex);
            }
            var result = new SelectList(prodType, "Value", "Text");
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 
        [HttpGet]
        public ActionResult CreateOrderForPluralGateway(paymentInitialViewModel paymentModel)
        {
            string msg = string.Empty;
            _productCategoryRepository = new ProductCategoryRepository();
            _productTypeRepository = new ProductTypeRepository();
            PaymentInitiateModel payment = new PaymentInitiateModel();
            try
            {
          
                string ModuleEnum = ExchangeOrderManager.GetEnumDescription(ABBPlanEnum.ModuleERP);
                if (paymentModel != null)
                {
                    string KeyString = ConfigurationManager.AppSettings["EncryptionKey"].ToString();
                    string IV = ConfigurationManager.AppSettings["EncryptionIV"].ToString();
                    payment.Custname = AES256encoding.DecryptString(paymentModel.name, KeyString, IV);
                    payment.custfirstname = AES256encoding.DecryptString(paymentModel.Firstname, KeyString, IV);
                    payment.custlastname = AES256encoding.DecryptString(paymentModel.Lastname, KeyString, IV);
                    payment.CustcontactNumber = AES256encoding.DecryptString(paymentModel.contactNumber, KeyString, IV);
                    payment.Custemail = AES256encoding.DecryptString(paymentModel.email, KeyString, IV);
                    payment.planamount = Convert.ToDecimal(AES256encoding.DecryptString(paymentModel.planPrice, KeyString, IV));
                    payment.Custaddress = AES256encoding.DecryptString(paymentModel.address, KeyString, IV);
                    payment.Custaddress1 = AES256encoding.DecryptString(paymentModel.address1, KeyString, IV);
                    payment.Custaddress2 = AES256encoding.DecryptString(paymentModel.address2, KeyString, IV);
                    payment.orderRegdNo = AES256encoding.DecryptString(paymentModel.RegdNo, KeyString, IV);
                    payment.custpin_code = AES256encoding.DecryptString(paymentModel.pincode, KeyString, IV);
                    payment.custstate = AES256encoding.DecryptString(paymentModel.state, KeyString, IV);
                    payment.custcity = AES256encoding.DecryptString(paymentModel.city, KeyString, IV);
                    paymentModel.productcategoryId = Convert.ToInt32(AES256encoding.DecryptString(paymentModel.productCategory, KeyString, IV));
                    paymentModel.productTypeId = Convert.ToInt32(AES256encoding.DecryptString(paymentModel.ProductType, KeyString, IV));
                    if (!string.IsNullOrEmpty(paymentModel.ModuleName))
                    {
                        payment.ModuleType = AES256encoding.DecryptString(paymentModel.ModuleName, KeyString, IV);
                        payment.ModuleTypeEnum = ModuleEnum;
                    }
                    tblProductCategory productCategory = _productCategoryRepository.GetSingle(x => x.Id == paymentModel.productcategoryId);
                    if (productCategory != null)
                    {
                        tblProductType productTypeObj = _productTypeRepository.GetSingle(x => x.Id == paymentModel.productTypeId);
                        if (productTypeObj != null)
                        {
                            payment.newproductCategory = productCategory.Description;
                            payment.newProductType = productTypeObj.Description;
                        }
                    }

                }
            }
            catch(Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBController", "CreateOrderForPluralGateway", ex);
                msg = ex.Message;
                return RedirectToAction("DetailsFailedOrder", "ABB", new { message = msg });
            }
           
            return View(payment);
        }
        [HttpPost]
        public ActionResult CreateOrderForPluralGateway(PaymentInitiateModel _requestData)
        {
            ABBRegManager ABBRegInfo = new ABBRegManager();
            _paymentRepository = new ABBPaymentRepository();
            _ABBRegistrationRepository = new ABBRegistrationRepository();
            // Generate random receipt number for order
            OrderModel orderModel = new OrderModel();
            PluralCreateOrder createorder = new PluralCreateOrder();
            CreateOrderResponse orderResponse = new CreateOrderResponse();
            CreateOrderErrorResponse errorResponse = new CreateOrderErrorResponse();
            string ReturnUrl = null;
            string Message = null;
            IRestResponse response = null;
            List<tblPaymentLeaser> paymenttTableObj = new List<tblPaymentLeaser>();
            string fileName = string.Empty;
            DateTime _dateTime = DateTime.Now;
            try
            {
                if (_requestData.ModuleType == _requestData.ModuleTypeEnum)
                {
                    if (_requestData.Base64StringValue != null)
                    {
                        byte[] bytes = System.Convert.FromBase64String(_requestData.Base64StringValue);
                        fileName = _requestData.orderRegdNo + _dateTime.ToString("yyyyMMddHHmmssFFF") + Path.GetExtension("image.jpeg");
                        string rootPath = @HostingEnvironment.ApplicationPhysicalPath;
                        string filePath = ConfigurationManager.AppSettings["InvoiceImage"].ToString() + fileName;
                        System.IO.File.WriteAllBytes(rootPath + filePath, bytes);
                        _requestData.InvoiceImage = fileName;
                        string status = ABBRegInfo.UpdateInvoiceDetailsABB(_requestData);

                    }
                    else
                    {
                        Message = "Invoice image is not provided please provide invoice image";
                        return RedirectToAction("DetailsFailedOrder", "ABB", new { message = Message });
                    }
                }


                paymenttTableObj = _paymentRepository.GetList(x => x.RegdNo == _requestData.orderRegdNo && x.IsActive == true && (x.PaymentStatus == false || x.PaymentStatus == null)).ToList();
                if (paymenttTableObj != null && paymenttTableObj.Count > 0)
                {
                    int i;
                    for (i = paymenttTableObj.Count; i > 0; i--)
                    {

                        _requestData.orderRegdNo = _requestData.orderRegdNo + "_" + i;
                        i = 0;
                    }
                }
                TempData["RegNo"] = _requestData.orderRegdNo;
                pluralgatewayKey apikey = new pluralgatewayKey();
                apikey = ABBRegInfo.getPaymentKey();
                ReturnUrl = ConfigurationManager.AppSettings["BaseURL"].ToString() + "ABB/CompletePl";
                //merchant data
                createorder.merchant_data = new MerchantData();
                createorder.merchant_data.merchant_id = apikey.merchantId;

                createorder.merchant_data.merchant_order_id = _requestData.orderRegdNo;
                createorder.merchant_data.merchant_return_url = ReturnUrl;
                createorder.merchant_data.merchant_access_code = apikey.accessCode;

                // Payment information
                createorder.payment_info_data = new PaymentInfoData();
                createorder.payment_info_data.amount = Convert.ToInt32(_requestData.planamount * 100);
                createorder.payment_info_data.currency_code = "INR";
                createorder.payment_info_data.order_desc = "ABB Plan amount";

                // Product information
                createorder.product_info_data = new ProductInfoData();
                createorder.product_info_data.product_details = new List<ProductDetail>();
                ProductDetail product_details = new ProductDetail();
                product_details.product_amount = Convert.ToInt32(_requestData.planamount * 100);
                product_details.product_code = _requestData.productTypeId.ToString();
                createorder.product_info_data.product_details.Add(product_details);

                // Customer information
                createorder.customer_data = new CustomerData();
                createorder.customer_data.country_code = Convert.ToInt32(PluralGatewayEnum.CountryCode).ToString();
                createorder.customer_data.mobile_number = _requestData.CustcontactNumber;
                createorder.customer_data.email_id = _requestData.Custemail;

                //ShippingAddress  information
                createorder.shipping_address_data = new ShippingAddressData();
                createorder.shipping_address_data.first_name = _requestData.custfirstname;
                createorder.shipping_address_data.last_name = _requestData.custlastname;
                createorder.shipping_address_data.address1 = _requestData.Custaddress1;
                createorder.shipping_address_data.address2 = _requestData.Custaddress2;
                createorder.shipping_address_data.address3 = _requestData.Custaddress1 + " " + _requestData.Custaddress1;
                createorder.shipping_address_data.pin_code = _requestData.custpin_code;
                createorder.shipping_address_data.city = _requestData.custcity;
                createorder.shipping_address_data.state = _requestData.custstate;
                createorder.shipping_address_data.country = "India";

                //BillingAddress information
                createorder.billing_address_data = new BillingAddressData();
                createorder.billing_address_data.first_name = _requestData.custfirstname;
                createorder.billing_address_data.last_name = _requestData.custlastname;
                createorder.billing_address_data.address1 = _requestData.Custaddress1;
                createorder.billing_address_data.address2 = _requestData.Custaddress2;
                createorder.billing_address_data.address3 = _requestData.Custaddress1 + " " + _requestData.Custaddress2;
                createorder.billing_address_data.pin_code = _requestData.custpin_code;
                createorder.billing_address_data.city = _requestData.custcity;
                createorder.billing_address_data.state = _requestData.custstate;
                createorder.billing_address_data.country = "India";

                //Additional information
                createorder.additional_info_data = new AdditionalInfoData();
                createorder.additional_info_data.rfu1 = "123456";

                response = ABBRegInfo.createOrderManager(createorder, apikey.secretKey);
                string SuccessResponse = response.StatusCode.ToString();
                if (SuccessResponse == "OK")
                {
                    orderResponse = JsonConvert.DeserializeObject<CreateOrderResponse>(response.Content);
                }
                else
                {
                    errorResponse = JsonConvert.DeserializeObject<CreateOrderErrorResponse>(response.Content);
                }

                if (orderResponse != null && orderResponse.token != null)
                {
                    orderModel.channelId = "WEB";
                    orderModel.theme = "default";
                    orderModel.orderToken = orderResponse.token;
                    orderModel.paymentMode = "CREDIT_DEBIT,NETBANKING,UPI,WALLET,EMI,DEBIT_EMI";
                    orderModel.countryCode = Convert.ToInt32(PluralGatewayEnum.CountryCode).ToString();
                    orderModel.mobileNumber = _requestData.CustcontactNumber;
                    orderModel.emailId = _requestData.Custemail;
                    orderModel.showSavedCardsFeature = false;
                    orderModel.amount = _requestData.planamount;
                }
                else
                {

                    if (errorResponse.error_message == "DUPLICATE_ORDER_RECEIVED_ON_MERCHANT")
                    {
                        Message = "Transaction already processed for this order id kindly place another order or contact with administrator";
                    }
                    else
                    {
                        Message = "Unable to process payment at the moment please contact with administrator";
                    }
                    return RedirectToAction("DetailsFailedOrder", "ABB", new { message = Message });
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBController", "CreateOrder", ex);
                Message = ex.Message;
                return RedirectToAction("DetailsFailedOrder", "ABB", new { message = Message });
            }
            return View("PaymentPage", orderModel);
        }
        #endregion

        #region abb registration customer details updation
        public ActionResult UpdateCustomerDetails(string RegdNo)
        {
            _ABBRegistrationRepository = new ABBRegistrationRepository();
            _productCategoryRepository = new ProductCategoryRepository();
            _productTypeRepository = new ProductTypeRepository();
            _brandRepository = new BrandRepository();
            _brandMappingRepository = new BrandSmartSellRepository();
            _businessUnitRepository = new BusinessUnitRepository();
            _businessPartnerRepository = new BusinessPartnerRepository();
            tblABBRegistration AbbregistrationObj = null;
            tblProductCategory CategoryObj = null;
            tblProductType productTypeobj = null;
            tblBrand brandObj = null;
            tblBrandSmartBuy brandSmartBuy= null;
            tblBusinessUnit businessunitObj = null;
            tblBusinessPartner businessPartnerobj = null;
            ABBRegistrationUpdateDataContract aBBRegistrationUpdateDataContract = new ABBRegistrationUpdateDataContract();
            tblCustomerDetail customerDetailObj = null;
            _customerDetailsRepository = new CustomerDetailsRepository();
            RegdNo = RegdNo.Trim();
            string message = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(RegdNo))
                {
                    AbbregistrationObj = _ABBRegistrationRepository.GetSingle(x => x.IsActive == true && x.RegdNo == RegdNo);
                    if (AbbregistrationObj != null)
                    {
                        customerDetailObj = _customerDetailsRepository.GetSingle(x => x.Id == AbbregistrationObj.CustomerId);
                        if (!string.IsNullOrEmpty(customerDetailObj.FirstName) && !string.IsNullOrEmpty(customerDetailObj.LastName) && !string.IsNullOrEmpty(customerDetailObj.Email))
                        {
                            message = "Customer details already updated";
                            return RedirectToAction("DetailsFailedOrder", "ABB", new { Message = message });
                        }

                        CategoryObj = _productCategoryRepository.GetSingle(x=>x.IsActive==true && x.IsAllowedForNew==true && x.Id==AbbregistrationObj.NewProductCategoryId);
                        if (CategoryObj != null)
                        {
                            productTypeobj = _productTypeRepository.GetSingle(x => x.IsActive == true && x.IsAllowedForNew == true && x.Id == AbbregistrationObj.NewProductCategoryTypeId);
                            if (productTypeobj != null)
                            {
                                businessunitObj = _businessUnitRepository.GetSingle(x => x.IsActive == true && x.IsABB == true && x.BusinessUnitId == AbbregistrationObj.BusinessUnitId);
                                if (businessunitObj != null)
                                {
                                    businessPartnerobj = _businessPartnerRepository.GetSingle(x=>x.IsActive==true && x.IsABBBP==true && x.BusinessPartnerId==AbbregistrationObj.BusinessPartnerId);
                                    if (businessPartnerobj != null)
                                    {
                                        if (businessunitObj.IsBUMultiBrand == true)
                                        {
                                            if (businessunitObj.BusinessUnitId == Convert.ToInt32(BusinessUnitEnum.PineLabs))
                                            {
                                                brandSmartBuy = _brandMappingRepository.GetSingle(x => x.BrandId == AbbregistrationObj.NewBrandId && x.IsActive == true);
                                                if (brandSmartBuy != null)
                                                {
                                                    aBBRegistrationUpdateDataContract.Brand = brandSmartBuy.Name;
                                                }
                                                else
                                                {
                                                    message = "Brand details not found in mapping table";
                                                    return RedirectToAction("DetailsFailedOrder", "ABB", new { message = message });
                                                }
                                            }
                                            else
                                            {
                                                brandSmartBuy = _brandMappingRepository.GetSingle(x => x.Id == AbbregistrationObj.NewBrandId && x.IsActive == true);
                                                if (brandSmartBuy != null)
                                                {
                                                    aBBRegistrationUpdateDataContract.Brand = brandSmartBuy.Name;
                                                }
                                                else
                                                {
                                                    message = "Brand details not found in mapping table";
                                                    return RedirectToAction("DetailsFailedOrder", "ABB", new { message = message });
                                                }
                                            }                                           
                                        }
                                        else
                                        {
                                            brandObj = _brandRepository.GetSingle(x => x.IsActive == true && x.Id == AbbregistrationObj.NewBrandId);

                                            if (brandObj != null)
                                            {
                                                aBBRegistrationUpdateDataContract.Brand = brandObj.Name;
                                            }
                                            else
                                            {
                                                message = "Brand details not found table";
                                                return RedirectToAction("DetailsFailedOrder", "ABB", new { message = message });
                                            }
                                        }
                                        if (businessPartnerobj.IsDefferedAbb == true)
                                        {
                                            aBBRegistrationUpdateDataContract.IsdefferedABB = true;
                                        }
                                        else
                                        {
                                            aBBRegistrationUpdateDataContract.IsdefferedABB = false;
                                        }
                                        if (!string.IsNullOrEmpty(businessunitObj.LogoName))
                                        {
                                            aBBRegistrationUpdateDataContract.LogoImage = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Content/DB_Files/SponsorLogo/" + businessunitObj.LogoName;
                                            aBBRegistrationUpdateDataContract.BULogoName = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Content/DB_Files/SponsorLogo/" + businessunitObj.LogoName;
                                        }
                                        else
                                        {
                                            aBBRegistrationUpdateDataContract.LogoImage=ConfigurationManager.AppSettings["BaseURL"].ToString() + "Content/DB_Files/SponsorLogo/Digi2L_Logo_Final.png";
                                        }

                                        aBBRegistrationUpdateDataContract.abbfees = AbbregistrationObj.ABBFees;
                                        aBBRegistrationUpdateDataContract.PlanDuration = AbbregistrationObj.ABBPlanPeriod;
                                        aBBRegistrationUpdateDataContract.ABBPlanName = AbbregistrationObj.ABBPlanName;
                                        aBBRegistrationUpdateDataContract.NoClaimPeriod = AbbregistrationObj.NoOfClaimPeriod;
                                        aBBRegistrationUpdateDataContract.PhoneNumber = AbbregistrationObj.CustMobile;
                                        aBBRegistrationUpdateDataContract.Productcategory = CategoryObj.Description;
                                        aBBRegistrationUpdateDataContract.ProductType = productTypeobj.Description;
                                        aBBRegistrationUpdateDataContract.RegdNo = AbbregistrationObj.RegdNo;
                                        aBBRegistrationUpdateDataContract.BUNAme = businessunitObj.Name;
                                        aBBRegistrationUpdateDataContract.ProductcategoryId =Convert.ToInt32(AbbregistrationObj.NewProductCategoryId);
                                        aBBRegistrationUpdateDataContract.ProductTypeID = Convert.ToInt32(AbbregistrationObj.NewProductCategoryTypeId);
                                        aBBRegistrationUpdateDataContract.BusinessUnitId = Convert.ToInt32(AbbregistrationObj.BusinessUnitId);

                                    }
                                    else
                                    {
                                        message = "Dealer details not found";
                                        return RedirectToAction("DetailsFailedOrder", "ABB", new { message = message });

                                    }
                                }
                                else
                                {
                                    message = "BusinessUnit not found";
                                    return RedirectToAction("DetailsFailedOrder", "ABB", new { message = message });
                                }
                            }
                            else
                            {
                                message = "Product type not found";
                                return RedirectToAction("DetailsFailedOrder", "ABB", new { message = message });
                            }
                        }
                        else
                        {
                            message = "Product category not found";
                            return RedirectToAction("DetailsFailedOrder", "ABB", new { message = message });
                        }

                    }
                    else
                    {
                        message = "No data found for this registration number";
                        return RedirectToAction("DetailsFailedOrder", "ABB", new { message = message });
                    }
                }
                else
                {
                    message = "Please provide registration number";
                    return RedirectToAction("DetailsFailedOrder", "ABB", new { message = message });
                }
            }
            catch(Exception ex)
            {
                LibLogging.WriteErrorToDB("ABB", "UpdateCustomerDetails", ex);
                message = ex.Message;
                return RedirectToAction("DetailsFailedOrder", "ABB", new { message = message });
            }
            return View(aBBRegistrationUpdateDataContract);
        }
        [HttpPost]
        public ActionResult UpdateCustomerDetails(ABBRegistrationUpdateDataContract abbRegistrationDC)
        {
            _ABBRegistrationRepository = new ABBRegistrationRepository();
            OrderConfirmationDataContract orderconfirmation = new OrderConfirmationDataContract();
            ABBRegManager aBBRegManager = new ABBRegManager();
            ABBRegistrationViewModel model = new ABBRegistrationViewModel();
            ABBPlanTransactionResponse TransactioResponseforPlan = new ABBPlanTransactionResponse();
            string msg = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                   string smsFlag = ConfigurationManager.AppSettings["sendsmsflag"].ToString();
                    if (string.IsNullOrEmpty(abbRegistrationDC.Address2))
                    {
                        abbRegistrationDC.Address2 = abbRegistrationDC.Address1;
                    }
                    orderconfirmation = aBBRegManager.UpdateABBOrderData(abbRegistrationDC);

                    TransactioResponseforPlan = aBBRegManager.SaveABBPlanDetails(model);
                    if (abbRegistrationDC.IsdefferedABB==true)
                    {
                        msg = "Thank you " + abbRegistrationDC.FirstName + " " + abbRegistrationDC.LastName + ". Your " + abbRegistrationDC.BUNAme + " home appliance purchase details have been received at UTC. Your product registration referance no. " + abbRegistrationDC.RegdNo + ". Rest details will be e-mailed to you in a day.";
                        if (smsFlag == "true")
                        {
                            if (abbRegistrationDC.PhoneNumber != null && !string.IsNullOrEmpty(abbRegistrationDC.RegdNo))
                                SendSucessSMS(abbRegistrationDC.PhoneNumber, abbRegistrationDC.RegdNo);
                        }
                       return RedirectToAction("Details", "ABB", new { message = msg });
                    }
                    else
                    {
                        
                        if (abbRegistrationDC.paynow == true)
                        {
                               
                            string IsPluralActive = ConfigurationManager.AppSettings["PluralActive"].ToString();
                            decimal planprice = Convert.ToDecimal(abbRegistrationDC.abbfees);
                            string PlnaPrice = planprice.ToString();
                            string KeyString = ConfigurationManager.AppSettings["EncryptionKey"].ToString();
                            string IV = ConfigurationManager.AppSettings["EncryptionIV"].ToString();
                            string FirstName = AES256encoding.EncryptString(abbRegistrationDC.FirstName, KeyString, IV);
                            string LastName = AES256encoding.EncryptString(abbRegistrationDC.LastName, KeyString, IV);
                            string Name = AES256encoding.EncryptString(abbRegistrationDC.FirstName + " " + abbRegistrationDC.LastName, KeyString, IV);
                            string EmailAddress = AES256encoding.EncryptString(abbRegistrationDC.Email, KeyString, IV);
                            string planAmount = AES256encoding.EncryptString(PlnaPrice, KeyString, IV);
                            string phonenumber = AES256encoding.EncryptString(abbRegistrationDC.PhoneNumber, KeyString, IV);
                            string custaddress1 = AES256encoding.EncryptString(abbRegistrationDC.Address1, KeyString, IV);
                            string custaddress2 = AES256encoding.EncryptString(abbRegistrationDC.Address2, KeyString, IV);
                            string custaddress = AES256encoding.EncryptString(abbRegistrationDC.Address1 + " " + abbRegistrationDC.Address2, KeyString, IV);
                            string orderId = AES256encoding.EncryptString(abbRegistrationDC.RegdNo, KeyString, IV);
                            string ProductCategory = AES256encoding.EncryptString(abbRegistrationDC.ProductcategoryId.ToString(), KeyString, IV);
                            string productType = AES256encoding.EncryptString(abbRegistrationDC.ProductTypeID.ToString(), KeyString, IV);
                            string state = AES256encoding.EncryptString(abbRegistrationDC.State.ToString(), KeyString, IV);
                            string city = AES256encoding.EncryptString(abbRegistrationDC.City.ToString(), KeyString, IV);
                            string pincode = AES256encoding.EncryptString(abbRegistrationDC.PinCode.ToString(), KeyString, IV);
                            string ModuleName = AES256encoding.EncryptString("URL", KeyString, IV);
                            if (IsPluralActive == "true")
                            {
                                return RedirectToAction("CreateOrderForPluralGateway", "ABB", new { name = Name, Firstname = FirstName, Lastname = LastName, email = EmailAddress, contactNumber = phonenumber, address = custaddress, address1 = custaddress1, address2 = custaddress2, planPrice = planAmount, RegdNo = orderId, productCategory = ProductCategory, ProductType = productType, state = state, city = city, pincode = pincode, ModuleName = ModuleName });
                            }
                            else
                            {
                                return RedirectToAction("CreateOrder", "ABB", new { name = Name, email = EmailAddress, contactNumber = phonenumber, address = custaddress, planPrice = planAmount, RegdNo = orderId, productCategory = ProductCategory, ProductType = productType, ModuleName = ModuleName });
                            }
                        }
                    }
                }
                else
                {
                    msg = "Some data is mising";
                    return RedirectToAction("DetailsFailedOrder", "ABB", new { message = msg });
                }
            }
            catch(Exception ex)
            {
                LibLogging.WriteErrorToDB("ABB", "UpdateCustomerDetails", ex);
                msg = ex.Message;
                return RedirectToAction("DetailsFailedOrder", "ABB", new { message = msg });
            }
            return View();
        }
        #endregion

        #region GET: Category type by Category Id Version 3 Created by VK Date 30-July
        [HttpGet]
        public JsonResult GetProdTypeByProdGroupId(int stateID, int buid, int ProductCatOld = 0, int ProductTypeOld = 0)
        {
            _productTypeRepository = new ProductTypeRepository();
            IEnumerable<SelectListItem> prodType = null;
            //Added by VK
            _productTypeRepository = new ProductTypeRepository();
            _productCategoryMappingRepository = new ProductCategoryMappingRepository();
            List<SelectListItem> newProductType = new List<SelectListItem>();
            List<tblBUProductCategoryMapping> BUmappingcategory = new List<tblBUProductCategoryMapping>();
            List<tblBUProductCategoryMapping> BUmappingcategory1 = new List<tblBUProductCategoryMapping>();
            //new product categorgy list
            List<tblProductType> prodTypeListForBosch = new List<tblProductType>();
            List<tblBUProductCategoryMapping> productCategoryTypeForNew = new List<tblBUProductCategoryMapping>();
            try
            {
                if (buid == Convert.ToInt32(BusinessUnitEnum.Diakin) && ProductTypeOld == Convert.ToInt32(ProductTypeOldEnum.SplitAC))
                {
                    tblProductType productObj = _productTypeRepository.GetSingle(x => x.Id == ProductTypeOld && x.IsAllowedForNew == true && x.IsActive == true);
                    if (productObj != null)
                    {
                        newProductType.Add(new SelectListItem { Text = productObj.Description, Value = productObj.Id.ToString() });
                    }
                    prodType = (newProductType).AsEnumerable();
                    prodType = prodType.OrderBy(x => x.Text);
                }
                else
                {
                    BUmappingcategory = _productCategoryMappingRepository.GetList(x => x.IsActive == true && x.BusinessUnitId == buid && x.ProductCatId == stateID && (x.OldProductCatId == null || x.OldProductCatId == ProductCatOld)).ToList();
                    if (BUmappingcategory != null && BUmappingcategory.Count > 0)
                    {
                        foreach (var item in BUmappingcategory)
                        {


                            tblProductType productObj = _productTypeRepository.GetSingle(x => x.Id == item.ProductTypeId && x.IsAllowedForNew == true && x.IsActive == true);
                            if (productObj != null)
                            {
                                newProductType.Add(new SelectListItem { Text = productObj.Description, Value = productObj.Id.ToString() });
                            }


                        }
                       prodType = (newProductType).AsEnumerable();
                       prodType = prodType.OrderBy(x => x.Text);
                    }
                }
               
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBController", "GetProdTypeByProdGroupId", ex);
                //LibLogging.WriteErrorLog("UserManager", "VerifyOTP", ex);
            }
            var result = new SelectList(prodType, "Value", "Text");
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
