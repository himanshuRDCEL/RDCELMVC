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
using RDCEL.DocUpload.BAL.Common;
using RDCEL.DocUpload.BAL.ProcessAPI;
using RDCEL.DocUpload.BAL.SponsorsApiCall;
using RDCEL.DocUpload.BAL.ZohoCreatorCall;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DataContract.SponsorModel;

namespace RDCEL.DocUpload.Web.API.Controllers
{
    public class CustomerController : Controller
    {
        BusinessPartnerRepository _businessPartnerRepository;
        BusinessUnitRepository _businessUnitRepository;
        BAL.SponsorsApiCall.ExchangeOrderManager _exchangeOrderManager;
      
        ExchangeOrderRepository _exchangeOrderRepository;
        ProductCategoryRepository _productCategoryRepository;
    
        NotificationManager _notificationManager;
        ProductCategoryMappingRepository _productCategoryMappingRepository;
        RDCEL.DocUpload.BAL.SponsorsApiCall.MasterManager _masterManager;

        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }

        #region OTP 
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
                LibLogging.WriteErrorToDB("CustomerController", "SendOTP", ex);
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
                LibLogging.WriteErrorToDB("CustomerController", "VerifyOTP", ex);
            }

            return Json(flag, JsonRequestBehavior.AllowGet);

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
                LibLogging.WriteErrorToDB("CustomerController", "Details", ex);
            }
            return View();
        }
        #endregion

        #region Select Customer
        public ActionResult SelectCust(int BUId)
        {
            List<tblBusinessPartner> businessPartnerList = null;
            tblBusinessPartner businessPartnerObj = new tblBusinessPartner();
            _businessPartnerRepository = new BusinessPartnerRepository();
            _businessUnitRepository = new BusinessUnitRepository();
            ExchagneViewModel ExchangeObj = new ExchagneViewModel();
            tblBusinessUnit BusinessUnitObj = new tblBusinessUnit();
            try
            {
                ExchangeObj.CityList = new List<SelectListItem>();
                ExchangeObj.PincodeList = new List<SelectListItem>();
                ExchangeObj.BUId = BUId;
                ExchangeObj.FormatName = "Home";
                BusinessUnitObj = _businessUnitRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == BUId);
                if (BusinessUnitObj != null)
                {
                    if (BusinessUnitObj.LogoName != null)
                    {
                        ExchangeObj.BULogoName = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Content/DB_Files/SponsorLogo/" + BusinessUnitObj.LogoName;
                    }
                }

                //get state list
                DataTable dt = _businessPartnerRepository.GetStateList();
                if (dt != null && dt.Rows.Count > 0)
                {
                    businessPartnerList = GenericConversionHelper.DataTableToList<tblBusinessPartner>(dt);
                    List<string> states = businessPartnerList.Where(x => x.BusinessUnitId == BUId && (x.IsExchangeBP != null && x.IsExchangeBP == true)).OrderBy(o => o.State).Select(x => x.State).Distinct().ToList();
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
                LibLogging.WriteErrorToDB("CustomerController", "SelectCust", ex);
            }

            return View(ExchangeObj);
        }

        [HttpPost]
        public ActionResult SelectCust(ExchagneViewModel ExchangeObj)
        {
            try
            {
                    return RedirectToAction("CustomerExchangeRegistration", "Customer", new { BUId = ExchangeObj.BUId, StateName = ExchangeObj.StateName, CityName = ExchangeObj.CityName, FormatName = ExchangeObj.FormatName, ZipCode = ExchangeObj.ZipCode });
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("CustomerController", "SelectCust", ex);
            }

            return View(ExchangeObj);
        }
        #endregion

        #region Customer Exchange REgistration
        public ActionResult CustomerExchangeRegistration(ExchagneViewModel ExchangeObj)
        {
            _productCategoryRepository = new ProductCategoryRepository();
            _exchangeOrderManager = new BAL.SponsorsApiCall.ExchangeOrderManager();
            _masterManager = new BAL.SponsorsApiCall.MasterManager();
            _businessPartnerRepository = new BusinessPartnerRepository();
            _businessUnitRepository = new BusinessUnitRepository();
            _exchangeOrderManager = new ExchangeOrderManager();
            _productCategoryMappingRepository = new ProductCategoryMappingRepository();
            List<tblBusinessPartner> businessPartnerList = null;
            ExchangeOrderDataContract exchangeDataContract = new ExchangeOrderDataContract();
            List<SelectListItem> StoreList = null;
            tblBusinessUnit BusinessUnitObj = null;
            try
            {
                //Product Category List
                List<tblProductCategory> prodGroupListForABB = _productCategoryRepository.GetList(x => x.IsActive == true && x.IsAllowedForOld == true).ToList();
                prodGroupListForABB = prodGroupListForABB.OrderBy(o => o.Description_For_ABB).ToList();

                if (prodGroupListForABB != null && prodGroupListForABB.Count > 0)
                {
                    ViewBag.ProductCategoryList = new SelectList(prodGroupListForABB, "Id", "Description");
                }
                //new product categorgy list
               
                List<tblProductCategory> prodGroupListForBosch = new List<tblProductCategory>();
                List<tblBUProductCategoryMapping> productCategoryForNew = _productCategoryMappingRepository.GetList(x => x.BusinessUnitId == ExchangeObj.BUId).ToList();
                if (productCategoryForNew.Count > 0)
                {
                    foreach (var productCategory in productCategoryForNew)
                    {
                        tblProductCategory productObj = _productCategoryRepository.GetSingle(x => x.Id == productCategory.ProductCatId && x.IsActive == true);
                        if (productObj != null)
                        {
                            prodGroupListForBosch.Add(productObj);
                        }
                    }
                }
                if (prodGroupListForBosch != null && prodGroupListForBosch.Count > 0)
                {
                    ViewBag.NewProductCategoryList = new SelectList(prodGroupListForBosch, "Id", "Description_For_ABB");
                }

                exchangeDataContract.ProductAge = 2002;
                exchangeDataContract.ProductTypeList = new List<SelectListItem>();
                exchangeDataContract.ProductModelList = new List<SelectListItem>();
                exchangeDataContract.BrandList = new List<SelectListItem>();
                exchangeDataContract.PincodeList = new List<SelectListItem>();
                exchangeDataContract.QualityCheckList = new List<SelectListItem> { new SelectListItem { Text = "Working", Value = "1" }
                                                                                    , new SelectListItem { Text = "Not Working", Value = "4" }};
                exchangeDataContract.QualityCheckList = exchangeDataContract.QualityCheckList.OrderByDescending(o => o.Value).ToList();
                exchangeDataContract.PurchasedProductCategoryList = new List<SelectListItem> { new SelectListItem { Text = "Dishwasher", Value = "Dishwasher" }
                                                                                        , new SelectListItem { Text = "Dryer", Value = "Dryer" }
                                                                                        , new SelectListItem { Text = "Refrigerator Frost Free" , Value = "Refrigerator Frost Free" }
                                                                                        , new SelectListItem { Text = "Refrigerator Side by Side", Value = "Refrigerator Side by Side" }
                                                                                        , new SelectListItem { Text = "Washing Machine" , Value = "Washing Machine" }
                                                                                        , new SelectListItem { Text = "Others" , Value = "Others" }
                                                                                    }.OrderBy(o => o.Text).ToList();

                exchangeDataContract.BusinessUnitDataContract = _exchangeOrderManager.GetBUById(ExchangeObj.BUId);

                if (exchangeDataContract.BusinessUnitDataContract != null)
                {
                    if (exchangeDataContract.BusinessUnitDataContract.LogoName != null)
                    {
                        exchangeDataContract.BULogoName = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Content/DB_Files/SponsorLogo/" + exchangeDataContract.BusinessUnitDataContract.LogoName;
                    }
                    exchangeDataContract.ExpectedDeliveryHours = exchangeDataContract.BusinessUnitDataContract.ExpectedDeliveryHours;
                }

                if (ExchangeObj.BUId > 0)
                {
                    //store details
                    if (ExchangeObj.CityName != null)
                    {
                        businessPartnerList = _businessPartnerRepository.GetList(x => x.IsActive == true
                        && x.IsExchangeBP == true
                        && !string.IsNullOrEmpty(x.AssociateCode)
                            && x.BusinessUnitId == ExchangeObj.BUId
                            && x.City.ToLower().Equals(ExchangeObj.CityName.ToLower())
                            && x.Pincode.ToLower().Equals(ExchangeObj.ZipCode.ToLower())).ToList();

                        StoreList = businessPartnerList.Select(x => new SelectListItem
                        {
                            Text = x.Description + ", " + x.AddressLine1,
                            Value = x.BusinessPartnerId.ToString()
                        }).ToList();
                      
                        //state city
                        exchangeDataContract.City = ExchangeObj.CityName;
                        exchangeDataContract.StateName = ExchangeObj.StateName;
                        //Pincode
                        exchangeDataContract.ZipCode = ExchangeObj.ZipCode;

                        //Quality Check 
                        exchangeDataContract.QualityCheck = 4; //By Default excelent working
                        exchangeDataContract.QualityCheckValue = 1;
                    }

                    ////BU details
                    BusinessUnitObj = _businessUnitRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == ExchangeObj.BUId);
                    if (BusinessUnitObj != null)
                    {
                        if (BusinessUnitObj.LogoName != null)
                        {
                            exchangeDataContract.BULogoName = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Content/DB_Files/SponsorLogo/" + BusinessUnitObj.LogoName;
                        }
                        exchangeDataContract.IsSweetnerModelBased = (bool)BusinessUnitObj.IsSweetnerModelBased;
                        exchangeDataContract.CompanyName = BusinessUnitObj.Name;
                        exchangeDataContract.BUName = BusinessUnitObj.Name;
                        exchangeDataContract.BusinessUnitId = ExchangeObj.BUId;
                        exchangeDataContract.ZohoSponsorNumber = BusinessUnitObj.ZohoSponsorId;
                    }
                    exchangeDataContract.FormatName = ExchangeObj.FormatName;
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("CustomerController", "CustomerExchangeRegistration", ex);
                return View(exchangeDataContract);
            }

            return View(exchangeDataContract);
        }

        [HttpPost]
        public ActionResult CustomerExchangeRegistration(ExchangeOrderDataContract exchangeDataContract)
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

                if (exchangeDataContract.QualityCheckValue > 0)
                {
                    exchangeDataContract.QualityCheck = exchangeDataContract.QualityCheckValue;
                }
                exchangeDataContract.RegdNo = "E" + UniqueString.RandomNumberByLength(7);
                if (string.IsNullOrEmpty(exchangeDataContract.SponsorOrderNumber))
                    exchangeDataContract.SponsorOrderNumber = "ECH" + UniqueString.RandomNumberByLength(9);
                else
                    exchangeDataContract.SponsorOrderNumber = exchangeDataContract.SponsorOrderNumber.Trim() + exchangeDataContract.RegdNo;
                string sponsorOrderNo = exchangeDataContract.SponsorOrderNumber;
                if (String.IsNullOrEmpty(sponsorOrderNo) == false)
                {
                    SponserObj = _exchangeOrderRepository.GetSingle(x => !string.IsNullOrEmpty(x.SponsorOrderNumber) &&  x.SponsorOrderNumber.ToLower().Equals(sponsorOrderNo.ToLower()));


                    if (SponserObj != null)
                    {
                        message = "This Exchange Order Number : " + exchangeDataContract.SponsorOrderNumber + " is already exist";
                    }
                    else
                    {

                        //exchangeDataContract.SponsorOrderNumber = "DTC" + UniqueString.RandomNumberByLength(9);



                        exchangeDataContract.CompanyName = exchangeDataContract.CompanyName;
                        exchangeDataContract.EstimatedDeliveryDate = DateTime.Now.AddHours(Convert.ToInt32(exchangeDataContract.ExpectedDeliveryHours)).ToString("dd-MM-yyyy");
                        exchangeDataContract.Bonus = "0";
                        if (exchangeDataContract.BusinessPartnerId > 0)
                        {
                            exchangeDataContract.StoreCode = _businessPartnerRepository.GetSingle(x => x.IsActive == true
                            && x.BusinessPartnerId == exchangeDataContract.BusinessPartnerId).StoreCode.Trim();
                        }
                        else if (exchangeDataContract.BusinessPartnerId == null || exchangeDataContract.BusinessPartnerId == 0 && exchangeDataContract.BusinessUnitId == Convert.ToInt32(BusinessUnitEnum.Bosch))
                        {
                            exchangeDataContract.IsDifferedSettlement = false;
                        }
                        productOrderResponseDC = _exchangeOrderManager.ManageExchangeOrder(exchangeDataContract);

                        if (productOrderResponseDC != null && productOrderResponseDC.OrderId > 0 && !string.IsNullOrEmpty(productOrderResponseDC.RegdNo))
                        {
                            if (exchangeDataContract.BusinessUnitId.Equals(1))
                            {
                                if (exchangeDataContract.FormatName.Equals("Home"))
                                {
                                    message = "Thankyou. Your Exchange details have been received at UTC. Our product registration referance no. " + exchangeDataContract.RegdNo + ". Our quality check team will connect you soon.";
                                }
                                else
                                {
                                    tblBusinessPartner businessPartner = null;
                                    if (exchangeDataContract.BusinessPartnerId > 0)
                                    {
                                        businessPartner = _businessPartnerRepository.GetSingle(x => x.IsActive == true && x.BusinessPartnerId == exchangeDataContract.BusinessPartnerId);
                                    }
                                    if (businessPartner != null && businessPartner.IsORC == true && businessPartner.IsDefferedSettlement == true)
                                    {
                                        message = "Thankyou. Your Exchange details have been received at UTC. Our quality check team will connect with you soon.";
                                    }
                                    else
                                    {
                                        message = "Thank you. Your Exchange details have been received at UTC. Our product registration referance no. " + exchangeDataContract.RegdNo + ". Our quality check team will connect with you soon." +
                                           " Congratulations!!! Please check your voucher detail at registered SMS/Email.";
                                    }

                                }
                            }
                            else
                                message = "Thankyou. Your Exchange details have been received at UTC. Our product registration referance no. " + exchangeDataContract.RegdNo + ". Our quality check team will connect with you soon.";
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
                LibLogging.WriteErrorToDB("CustomerController", "CustomerExchangeRegistration", ex);
                return View();
            }

            return RedirectToAction("Details");
        }

        #endregion


    }
}