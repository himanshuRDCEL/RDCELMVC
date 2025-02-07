using GraspCorn.Common.Constant;
using GraspCorn.Common.Enums;
using GraspCorn.Common.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using RDCEL.DocUpload.BAL.Common;
using RDCEL.DocUpload.BAL.Manager;
using RDCEL.DocUpload.BAL.OldProductDetailsManager;
using RDCEL.DocUpload.BAL.ProcessAPI;
using RDCEL.DocUpload.BAL.SponsorsApiCall;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DataContract.ABBRegistration;
using RDCEL.DocUpload.Web.API.Helpers;
using RDCEL.DocUpload.DataContract.BlowHorn;
using RDCEL.DocUpload.DataContract.SponsorModel;
using RDCEL.DocUpload.DataContract.UniversalPricemasterDetails;

namespace RDCEL.DocUpload.Web.API.Controllers
{
    public class BulkExchnageController : Controller
    {
        BusinessPartnerManager _businessPartnerManager;
        BusinessPartnerRepository _businessPartnerRepository;
        BusinessUnitRepository _businessUnitRepository;
        ExchangeOrderManager _exchangeOrderManager;
        ExchangeOrderRepository _exchangeOrderRepository;
        ProductCategoryRepository _productCategoryRepository;
        NotificationManager _notificationManager;
        PriceMasterRepository _priceMasterRepository;
        LoginDetailsUTCRepository _loginRepository;
        ProductConditionLabelRepository _productConditionLabelRepository;
        OrderBasedConfigurationRepository _OrderBasedConfigurationRepository;
        public ActionResult Index()
        {
            return View();
        }

        #region Login

        public ActionResult Login()
        {
            if (TempData["Auth"] != null && Convert.ToBoolean(TempData["Auth"]) == false)
            {
                ShowMessage("Invalid Credential", MessageTypeEnum.error);
            }

            return View();
        }


        [HttpPost]
        public ActionResult Login(FormCollection formCollection)
        {
            _businessPartnerManager = new BusinessPartnerManager();

            if (formCollection != null)
            {
                string username = formCollection["username"] != null ? formCollection["username"].ToString() : "";
                string password = formCollection["password"] != null ? formCollection["password"].ToString() : "";


                BusinessPartnerViewModel businessPartnerVM = _businessPartnerManager.GetUsersByIdPassword(username, password);
                if (businessPartnerVM != null && businessPartnerVM.BusinessPartnerId != 0)
                {
                    Session["User"] = FillSession(businessPartnerVM);
                    Session["UserId"] = businessPartnerVM.BusinessPartnerId;
                    return RedirectToAction("BulkExchangeRegistration", "BulkExchnage", new { BPId = businessPartnerVM.BusinessPartnerId });
                }
                else
                {
                    TempData["Auth"] = false;
                    return RedirectToAction("Login", "BulkExchnage", new { Area = "" });
                }
            }
            else
            {
                TempData["Auth"] = false;
                return RedirectToAction("Login", "BulkExchnage", new { Area = "" });
            }

        }


        /// <summary>
        /// Method to fill session
        /// </summary>
        /// <param name="agentVM">User view model</param>
        /// <returns>SessionHelper</returns>
        public SessionHelper FillSession(BusinessPartnerViewModel businessPartnerVM)
        {
            SessionHelper sessionHelper = new SessionHelper();
            sessionHelper.LoggedUserInfo = businessPartnerVM;
            return sessionHelper;
        }

        public void ShowMessage(string message, MessageTypeEnum messageType)
        {
            ViewBag.MessageType = messageType;
            ModelState.AddModelError(string.Empty, message);
        }

        /// <summary>
        /// method to validate the login 
        /// </summary>
        /// <returns>bool</returns>
        public bool ValidateBPLogin()
        {
            bool flag = false;
            SessionHelper _sessionHelper = (SessionHelper)Session["User"];
            if (_sessionHelper != null && _sessionHelper.LoggedUserInfo != null && _sessionHelper.LoggedUserInfo.BusinessPartnerId > 0)
            {
                flag = true;
            }
            return flag;
        }

        #endregion

        #region Details
        public ActionResult Details()
        {
            string msg = string.Empty;
            try
            {
                if (TempData["Msg"] != null && !string.IsNullOrEmpty(TempData["Msg"].ToString()))
                    msg = TempData["Msg"].ToString();
                else
                    msg = "Some error occurred, please connect with the Administrator.";

                ViewBag.MSG = msg;

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("BulkExchnageController", "Details", ex);
            }
            return View();
        }
        #endregion

        #region Bulk Exchange Order Registration
        public ActionResult BulkExchangeRegistration(int BPId)
        {
            ExchangeOrderDataContract exchangeDataContract = new ExchangeOrderDataContract();
            tblBusinessUnit BusinessUnitObj = new tblBusinessUnit();
            List<tblBusinessPartner> businessPartnersList = new List<tblBusinessPartner>();
            _businessUnitRepository = new BusinessUnitRepository();
            _productCategoryRepository = new ProductCategoryRepository();
            _OrderBasedConfigurationRepository = new OrderBasedConfigurationRepository();
            _loginRepository = new LoginDetailsUTCRepository();
            _priceMasterRepository = new PriceMasterRepository();
            _productConditionLabelRepository = new ProductConditionLabelRepository();
            List<tblPriceMaster> pricemaster = null;
            List<tblProductCategory> prodGroupListForExchange = new List<tblProductCategory>();
            List<tblProductConditionLabel> ConditionLabelObj = new List<tblProductConditionLabel>();
            tblProductCategory categoryObj = null;
            Login login = null;

            try
            {
                if (ValidateBPLogin())
                {
                    SessionHelper _sessionHelper = (SessionHelper)Session["User"];
                    ViewBag.LoginUser = _sessionHelper.LoggedUserInfo;
                    ViewBag.IsDealer = _sessionHelper.LoggedUserInfo.IsDealer;
                    ViewBag.BPId = _sessionHelper.LoggedUserInfo.BusinessPartnerId;
                    exchangeDataContract.BusinessUnitId = (int)_sessionHelper.LoggedUserInfo.BusinessUnitId;
                    exchangeDataContract.BusinessPartnerId = BPId;

                    BusinessUnitObj = _businessUnitRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == (int)_sessionHelper.LoggedUserInfo.BusinessUnitId);
                    if (BusinessUnitObj != null)
                    {
                        exchangeDataContract.BusinessUnitId = BusinessUnitObj.BusinessUnitId;
                        if (BusinessUnitObj.LogoName != null)
                        {
                            exchangeDataContract.BULogoName = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Content/DB_Files/SponsorLogo/" + BusinessUnitObj.LogoName;
                        }


                        //code to check quality Condition
                        if (BusinessUnitObj.IsQualityRequiredOnUI != null)
                        {
                            exchangeDataContract.IsQualityRequiredOnUi = Convert.ToBoolean(BusinessUnitObj.IsQualityRequiredOnUI);
                        }
                        else
                        {
                            exchangeDataContract.IsQualityRequiredOnUi = false;
                        }

                        //login = _loginRepository.GetSingle(x => x.SponsorId == exchangeDataContract.BusinessUnitId);
                        //if (login != null)
                        //{
                        //    exchangeDataContract.PriceCode = login.PriceCode;
                        //}
                        // Code  to get Product condition labels By vishal choudhary date[15/06/2023]
                        ConditionLabelObj = _productConditionLabelRepository.GetList(x => x.BusinessUnitId == BusinessUnitObj.BusinessUnitId && x.IsActive == true && x.BusinessPartnerId== BPId).ToList();
                        if (ConditionLabelObj.Count > 0)
                        {
                            exchangeDataContract.ProductConditionCount = ConditionLabelObj.Count;
                            exchangeDataContract.QualityCheckList = ConditionLabelObj.Select(x => new SelectListItem
                            {
                                Text = x.PCLabelName,
                                Value = x.OrderSequence.ToString()
                            }).ToList();
                        }
                        else
                        {
                            TempData["Msg"] = "Product Condition not available";
                            return RedirectToAction("Details");
                        }
                        exchangeDataContract.QualityCheckList = exchangeDataContract.QualityCheckList.OrderByDescending(o => o.Value).ToList();
                        //Old product category for exchange
                        //if (exchangeDataContract.PriceCode != null)
                        //{
                        //    DataTable dtProductCat = _priceMasterRepository.GetProductCategoryByPriceCode(exchangeDataContract.PriceCode);

                        //    if (dtProductCat != null && dtProductCat.Rows.Count > 0)
                        //    {
                        //        pricemaster = GenericConversionHelper.DataTableToList<tblPriceMaster>(dtProductCat);
                        //        foreach (var productCat in pricemaster)
                        //        {
                        //            categoryObj = _productCategoryRepository.GetSingle(x => x.IsActive == true && x.IsAllowedForOld == true && x.Id == productCat.ProductCategoryId);
                        //            if (categoryObj != null)
                        //            {
                        //                prodGroupListForExchange.Add(categoryObj);
                        //            }
                        //        }
                        //    }

                        //    prodGroupListForExchange = prodGroupListForExchange.OrderBy(o => o.Description_For_ABB).ToList();
                        //    if (prodGroupListForExchange != null && prodGroupListForExchange.Count > 0)
                        //    {
                        //        ViewBag.ProductCategoryList = new SelectList(prodGroupListForExchange, "Id", "Description");
                        //    }
                        //}
                    }
                   // exchangeDataContract.IsSweetnerModelBased = (bool)BusinessUnitObj.IsSweetnerModelBased;
                    exchangeDataContract.CompanyName = BusinessUnitObj.Name;
                    exchangeDataContract.BUName = BusinessUnitObj.Name;
                    exchangeDataContract.BusinessUnitId = _sessionHelper.LoggedUserInfo.BusinessUnitId;
                    exchangeDataContract.BusinessPartnerId = _sessionHelper.LoggedUserInfo.BusinessPartnerId;
                    exchangeDataContract.ZohoSponsorNumber = BusinessUnitObj.ZohoSponsorId;

                    //orderbasedconfig calling

                    tblOrderBasedConfig orderBasedConfig = new tblOrderBasedConfig();

                    orderBasedConfig = _OrderBasedConfigurationRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == Convert.ToInt32(BusinessUnitObj.BusinessUnitId) && x.BusinessPartnerId == Convert.ToInt32(exchangeDataContract.BusinessPartnerId));

                    if (orderBasedConfig != null)

                    {

                        exchangeDataContract.IsValidationBasedSweetner = Convert.ToBoolean(orderBasedConfig.IsValidationBasedSweetener != null ? orderBasedConfig.IsValidationBasedSweetener : false);

                        exchangeDataContract.IsSweetnerModelBased = Convert.ToBoolean(orderBasedConfig.IsSweetenerModalBased != null ? orderBasedConfig.IsSweetenerModalBased : false);


                    }


                    //old productcategory list from pricemasternameid
                    if (exchangeDataContract.BusinessUnitId > 0 && exchangeDataContract.BusinessPartnerId > 0)
                    {
                        OldProductDetailsManager oldProductDetailsManager = new OldProductDetailsManager();
                        PriceMasterMappingDataContract priceMasterMappingDataContract = new PriceMasterMappingDataContract();

                        priceMasterMappingDataContract.BusinessunitId = exchangeDataContract.BusinessUnitId;
                        priceMasterMappingDataContract.BusinessPartnerId = exchangeDataContract.BusinessPartnerId;
                        PriceMasterNameDataContract priceMasterNameDataContract = new PriceMasterNameDataContract();
                        priceMasterNameDataContract = oldProductDetailsManager.GetPriceNameId(priceMasterMappingDataContract);

                        if (priceMasterNameDataContract != null && priceMasterNameDataContract.PriceNameId > 0)
                        {
                            priceMasterNameDataContract.PriceNameId = priceMasterNameDataContract.PriceNameId;
                            exchangeDataContract.priceMasterNameID = Convert.ToInt32(priceMasterNameDataContract.PriceNameId);

                            prodGroupListForExchange = oldProductDetailsManager.GetProductCatListByPriceMasterNameId(Convert.ToInt32(priceMasterNameDataContract.PriceNameId));
                        }
                        prodGroupListForExchange = prodGroupListForExchange.OrderBy(o => o.Description_For_ABB).ToList();

                        if (prodGroupListForExchange != null && prodGroupListForExchange.Count > 0)
                        {
                            ViewBag.ProductCategoryList = new SelectList(prodGroupListForExchange, "Id", "Description");
                        }
                        else
                        {
                            TempData["Msg"] = "No product category found for old product exchange";
                            return RedirectToAction("Details");
                        }
                    }
                    else
                    {
                        TempData["Msg"] = "BusinessUnit and Businespartner not available";
                        return RedirectToAction("Details");
                    }

                    //List<tblProductCategory> prodGroupListForABB = _productCategoryRepository.GetList(x => x.IsActive == true && x.IsAllowedForOld == true && !x.Description.ToLower().Contains("dishwasher")).ToList();
                    //prodGroupListForABB = prodGroupListForABB.OrderBy(o => o.Description_For_ABB).ToList();

                    //if (prodGroupListForABB != null && prodGroupListForABB.Count > 0)
                    //{
                    //    ViewBag.ProductCategoryList = new SelectList(prodGroupListForABB, "Id", "Description");
                    //}
                    exchangeDataContract.ProductAge = 2002;
                    exchangeDataContract.ProductTypeList = new List<SelectListItem>();
                    exchangeDataContract.BrandList = new List<SelectListItem>();
                    //exchangeDataContract.QualityCheckList = new List<SelectListItem> { new SelectListItem { Text = "Working", Value = "1" }
                    //                                                                , new SelectListItem { Text = "Not Working", Value = "4" }};
                    exchangeDataContract.QualityCheck = 1; //By Default excelent working

                    return View(exchangeDataContract);
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("BulkExchange", "BulkExchangeRegistration", ex);
                return View(exchangeDataContract);
            }

            return View(exchangeDataContract);
        }

        [HttpPost]
        public ActionResult BulkExchangeRegistration(ExchangeOrderDataContract exchangeDataContract)
        {
            SponsrOrderSyncManager sponsrOrderSyncManager = new SponsrOrderSyncManager();
            ProductOrderResponseDataContract productOrderResponseDC = null;
            _exchangeOrderRepository = new ExchangeOrderRepository();
            _businessPartnerRepository = new BusinessPartnerRepository();
            _exchangeOrderManager = new ExchangeOrderManager();
            tblExchangeOrder SponserObj = null;
            string message = string.Empty;
            try
            {


                exchangeDataContract.RegdNo = "E" + UniqueString.RandomNumberByLength(7);
                if (string.IsNullOrEmpty(exchangeDataContract.SponsorOrderNumber))
                    exchangeDataContract.SponsorOrderNumber = "DTC" + UniqueString.RandomNumberByLength(9);
                else
                    exchangeDataContract.SponsorOrderNumber = exchangeDataContract.SponsorOrderNumber.Trim() + exchangeDataContract.RegdNo;
                string sponsorOrderNo = exchangeDataContract.SponsorOrderNumber;
                if (String.IsNullOrEmpty(sponsorOrderNo) == false)
                {
                    SponserObj = _exchangeOrderRepository.GetSingle(x => !string.IsNullOrEmpty(x.SponsorOrderNumber) && x.SponsorOrderNumber.ToLower().Equals(sponsorOrderNo.ToLower()));


                    if (SponserObj != null)
                    {
                        message = "This Exchange Order Number : " + exchangeDataContract.SponsorOrderNumber + " is already exist";
                    }
                    else
                    {
                        exchangeDataContract.CompanyName = exchangeDataContract.CompanyName;
                        exchangeDataContract.EstimatedDeliveryDate = DateTime.Now.AddHours(Convert.ToInt32(exchangeDataContract.ExpectedDeliveryHours)).ToString("dd-MM-yyyy");
                        exchangeDataContract.Bonus = "0";
                        if (exchangeDataContract.BusinessPartnerId > 0)
                        {
                            exchangeDataContract.StoreCode = _businessPartnerRepository.GetSingle(x => x.IsActive == true
                            && x.BusinessPartnerId == exchangeDataContract.BusinessPartnerId).StoreCode.Trim();
                        }
                        productOrderResponseDC = _exchangeOrderManager.ManageBulkExchangeOrder(exchangeDataContract);

                        if (productOrderResponseDC != null && productOrderResponseDC.OrderId > 0 && !string.IsNullOrEmpty(productOrderResponseDC.RegdNo))
                        {
                            message = "Thankyou. Your Exchange details have been received at UTC. Our quality check team will connect you soon.";
                        }
                        else
                            message = "Order not Created";
                    }
                }

                if (!string.IsNullOrEmpty(message))
                    TempData["Msg"] = message;
                else
                    TempData["Msg"] = "Some error occurred, please connect with the Administrator.";
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("BulkExchange", "BulkExchangeRegistration", ex);
                return View();
            }

            return RedirectToAction("Details");
        }
        #endregion

        #region Dealer Dashboard

        [HttpGet]
        public ActionResult Dashboard()
        {
            string msg = string.Empty;
            _businessUnitRepository = new BusinessUnitRepository();
            ExchangeOrderDataContract ExchangeObj = new ExchangeOrderDataContract();
            tblBusinessUnit BusinessUnitObj = new tblBusinessUnit();

            try
            {
                if (ValidateBPLogin())
                {
                    SessionHelper _sessionHelper = (SessionHelper)Session["User"];
                    ViewBag.BPId = _sessionHelper.LoggedUserInfo.BusinessPartnerId;
                    ExchangeObj.BusinessPartnerId = _sessionHelper.LoggedUserInfo.BusinessPartnerId;
                    BusinessUnitObj = _businessUnitRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == (int)_sessionHelper.LoggedUserInfo.BusinessUnitId);
                    if (BusinessUnitObj != null)
                    {
                        if (BusinessUnitObj.LogoName != null)
                        {
                            ExchangeObj.BULogoName = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Content/DB_Files/SponsorLogo/" + BusinessUnitObj.LogoName;
                        }
                    }
                    return View(ExchangeObj);
                }
                else
                {
                    return RedirectToAction("Login", "Voucher", new { Area = "" });
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("BulkExchange", "Dashboard", ex);
                return View();
            }
        }
        #endregion

        #region get list of Bulk Exchange order on basis of BPId
        public JsonResult GetBulkExchangeOrder(int businessPartnerId)
        {
            _exchangeOrderManager = new ExchangeOrderManager();
            var exchangeOrderObj = _exchangeOrderManager.GetBulkExchangeOrderDetailbyBPId(businessPartnerId);
            return Json(new { data = exchangeOrderObj }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region On Page Ajax Call Methods

        public JsonResult SendOTP(string mobnumber, int buid)
        {
            _notificationManager = new NotificationManager();
            _businessUnitRepository = new BusinessUnitRepository();
            bool flag = false;
            try
            {
                if (buid > 0)
                {
                    tblBusinessUnit businessUnit = _businessUnitRepository.GetSingle(x => x.BusinessUnitId == buid);
                    string OTPValue = UniqueString.RandomNumber();

                    string message = NotificationConstants.SMS_Exchange_OTP_ALL.Replace("[OTP]", OTPValue).Replace("[STORENAME]", businessUnit.Name);
                    flag = _notificationManager.SendNotificationSMS(mobnumber, message, OTPValue);
                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeController", "sendByTextLocalSMS", ex);
            }

            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        public JsonResult VerifyOTP(string mobnumber, string OTP)
        {
            _notificationManager = new NotificationManager();
            bool flag = false;
            try
            {
                string message = string.Empty;
                flag = _notificationManager.ValidateOTP(mobnumber, OTP);
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeController", "VerifyOTP", ex);
            }

            return Json(flag, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region Logout
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Login", "BulkExchnage");
        }
        #endregion
    }
}