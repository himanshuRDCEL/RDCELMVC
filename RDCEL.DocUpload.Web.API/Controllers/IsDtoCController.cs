using GraspCorn.Common.Enums;
using GraspCorn.Common.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using RDCEL.DocUpload.BAL.Common;
using RDCEL.DocUpload.BAL.Manager;
using RDCEL.DocUpload.BAL.OldProductDetailsManager;
using RDCEL.DocUpload.BAL.ProcessAPI;
using RDCEL.DocUpload.BAL.SponsorsApiCall;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DAL.Helper;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DataContract.ABBRegistration;
using RDCEL.DocUpload.Web.API.Helpers;
using RDCEL.DocUpload.DataContract;
using RDCEL.DocUpload.DataContract.Bizlog;
using RDCEL.DocUpload.DataContract.BlowHorn;
using RDCEL.DocUpload.DataContract.ExchangeOrderDetails;
using RDCEL.DocUpload.DataContract.SponsorModel;
using RDCEL.DocUpload.DataContract.UniversalPricemasterDetails;

namespace RDCEL.DocUpload.Web.API.Controllers
{
    public class IsDtoCController : Controller
    {
        // GET: IsDtoC
        #region Variable Declaration
        ProductCategoryRepository _productCategoryRepository;
        ProductTypeRepository _productTypeRepository;
        BAL.SponsorsApiCall.ExchangeOrderManager _exchangeOrderManager;
        BusinessPartnerRepository _businessPartnerRepository;
        BusinessUnitRepository _businessUnitRepository;
        ExchangeOrderRepository _exchangeOrderRepository;
        BusinessPartnerManager _businessPartnerManager;
        LoginDetailsUTCRepository _loginRepository;
        PinCodeRepository _pincodeRepository;
        PriceMasterRepository _priceMasterRepository;
        ProductConditionLabelRepository _productConditionLabelRepository;
        Logging logging;
        RDCEL.DocUpload.BAL.SponsorsApiCall.MasterManager _masterManager;

        #endregion
        #region Old Product Detail and Price
        public ActionResult ProductDetails(int BUId)
        {
            MyGateViewModel myGateDataContract = new MyGateViewModel();

            _exchangeOrderManager = new BAL.SponsorsApiCall.ExchangeOrderManager();
            _businessUnitRepository = new BusinessUnitRepository();
            _businessPartnerRepository = new BusinessPartnerRepository();
            _productCategoryRepository = new ProductCategoryRepository();
            _loginRepository = new LoginDetailsUTCRepository();
            _priceMasterRepository = new PriceMasterRepository();
            _productConditionLabelRepository = new ProductConditionLabelRepository();
            tblBusinessUnit BusinessUnitObj = null;
            //List<tblPriceMaster> pricemaster = null;
            List<tblProductCategory> prodGroupListForExchange = new List<tblProductCategory>();
            List<tblProductConditionLabel> ConditionLabelObj = new List<tblProductConditionLabel>();
            //tblProductCategory categoryObj = null;
            string storeCode = "MYG-APP-01";
            string Message = "";
            PriceMasterMappingDataContract priceMasterMappingDataContract = new PriceMasterMappingDataContract();

            try
            {
                BUId = 9;
                myGateDataContract.url = ConfigurationManager.AppSettings["Close"].ToString();
                //Product Category List
                //List<tblProductCategory> prodGroupListForAliance = _productCategoryRepository.GetList(x => x.IsActive == true && x.IsAllowedForOld == true).ToList();
                //if (prodGroupListForAliance != null && prodGroupListForAliance.Count > 0)
                //{
                //    ViewBag.ProductCategoryList = new SelectList(prodGroupListForAliance, "Id", "Description");
                //}
                myGateDataContract.ProductAge = Convert.ToInt32(ProductAgeEnum.ProductAge);
                myGateDataContract.BUId = BUId;
                myGateDataContract.BusinessUnitId = Convert.ToInt32(BusinessUnitEnum.Alliance);
                myGateDataContract.FormatName = ExchangeOrderManager.GetEnumDescription((FormatTypeEnum.Home));
                myGateDataContract.ProductTypeList = new List<SelectListItem>();
                myGateDataContract.ProductModelList = new List<SelectListItem>();
                myGateDataContract.BrandList = new List<SelectListItem>();

                BusinessUnitObj = _businessUnitRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == BUId);
                if (BusinessUnitObj != null)
                {
                    if (BusinessUnitObj.LogoName != null)
                    {
                       // myGateDataContract.BULogoName = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Content/DB_Files/SponsorLogo/" + BusinessUnitObj.LogoName;
                    }
                    priceMasterMappingDataContract.BusinessunitId = BusinessUnitObj.BusinessUnitId;
                    myGateDataContract.IsSweetnerModelBased = (bool)BusinessUnitObj.IsSweetnerModelBased;
                    // Code  to get Product condition labels By vishal choudhary date[15/06/2023]

                    //call businesspartner
                    tblBusinessPartner tblBusinessPartner = _businessPartnerRepository.GetSingle(x => x.IsActive == true && x.StoreCode == storeCode);
                    if (tblBusinessPartner != null)
                    {
                        myGateDataContract.BULogoName = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Content/DB_Files/SponsorLogo/" + tblBusinessPartner.LogoImage;
                        priceMasterMappingDataContract.BusinessPartnerId = tblBusinessPartner.BusinessPartnerId;
                        myGateDataContract.BusinessPartnerId= tblBusinessPartner.BusinessPartnerId;
                        ConditionLabelObj = _productConditionLabelRepository.GetList(x => x.BusinessUnitId == BusinessUnitObj.BusinessUnitId && x.BusinessPartnerId== tblBusinessPartner.BusinessPartnerId && x.IsActive == true).ToList();
                    }

                    if (ConditionLabelObj.Count > 0)
                    {
                        myGateDataContract.ProductConditionCount = ConditionLabelObj.Count;
                        myGateDataContract.QualityCheckList = ConditionLabelObj.Select(x => new SelectListItem
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
                    myGateDataContract.QualityCheckList = myGateDataContract.QualityCheckList.OrderByDescending(o => o.Value).ToList();
                }
                Login loginObj = _loginRepository.GetSingle(x => x.SponsorId == myGateDataContract.BusinessUnitId);
                if (loginObj != null)
                {
                    myGateDataContract.PriceCode = loginObj.PriceCode;
                }

                //Old product category for exchange
                //if (myGateDataContract.PriceCode != null)
                //{
                //    DataTable dtProductCat = _priceMasterRepository.GetProductCategoryByPriceCode(myGateDataContract.PriceCode);

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

                // old category list by universal pricemaster
                if (priceMasterMappingDataContract.BusinessunitId > 0 && priceMasterMappingDataContract.BusinessPartnerId>0)
                {
                    OldProductDetailsManager oldProductDetailsManager = new OldProductDetailsManager();
                    //priceMasterMappingDataContract.BusinessunitId = BusinessUnitObj.BusinessUnitId;
                    //priceMasterMappingDataContract.BusinessPartnerId = BusinessUnitObj.BusinessUnitId;
                    PriceMasterNameDataContract priceMasterNameDataContract = new PriceMasterNameDataContract();
                    priceMasterNameDataContract = oldProductDetailsManager.GetPriceNameId(priceMasterMappingDataContract);

                    if (priceMasterNameDataContract != null && priceMasterNameDataContract.PriceNameId > 0)
                    {
                        priceMasterNameDataContract.PriceNameId = priceMasterNameDataContract.PriceNameId;
                        myGateDataContract.priceMasterNameID = Convert.ToInt32(priceMasterNameDataContract.PriceNameId);

                        prodGroupListForExchange = oldProductDetailsManager.GetProductCatListByPriceMasterNameId(Convert.ToInt32(priceMasterNameDataContract.PriceNameId));
                    }
                    prodGroupListForExchange = prodGroupListForExchange.OrderBy(o => o.Description_For_ABB).ToList();

                    if (prodGroupListForExchange != null && prodGroupListForExchange.Count > 0)
                    {
                        ViewBag.ProductCategoryList = new SelectList(prodGroupListForExchange, "Id", "Description");
                    }
                    else
                    {
                        Message = "No product category found for old product exchange";
                        return RedirectToAction("DetailsFailedResponse", "Exchange", new { Message = Message });
                    }
                }
                else
                {
                    TempData["Msg"] = "BusinessUnit and Businespartner not available";
                    return RedirectToAction("Details");
                }


            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("IsDtoC", "ProductDetails", ex);
            }

            return View(myGateDataContract);
        }

        [HttpPost]
        public ActionResult ProductDetails(MyGateViewModel ExchangeObj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ExchangeObj.BUId = 9;
                    if (ExchangeObj.BUId == 9)
                        return RedirectToAction("SelectQCTime", "IsDtoC", new { BUId = ExchangeObj.BUId, BrandId = ExchangeObj.BrandId, ProductAge = ExchangeObj.ProductAge, FormatName = ExchangeObj.FormatName, ProductCategoryId = ExchangeObj.ProductCategoryId, ProductTypeId = ExchangeObj.ProductTypeId, ExchangePriceString = ExchangeObj.ExchangePriceString, QualityCheck = ExchangeObj.QualityCheck, BULogoName = ExchangeObj.BULogoName,BusinessPartnerId = ExchangeObj.BusinessPartnerId,priceMasterNameId= ExchangeObj.priceMasterNameID,BasePrice = ExchangeObj.BasePrice, SweetenerTotal = ExchangeObj.SweetenerTotal, SweetenerBu = ExchangeObj.SweetenerBu , SweetenerBP = ExchangeObj.SweetenerBP, SweetenerDigi2L = ExchangeObj.SweetenerDigi2L });
                    else
                        return RedirectToAction("SelectQCTime", "IsDtoC", new { BUId = ExchangeObj.BUId, BrandId = ExchangeObj.BrandId, ProductAge = ExchangeObj.ProductAge, FormatName = ExchangeObj.FormatName, ProductCategoryId = ExchangeObj.ProductCategoryId, ProductTypeId = ExchangeObj.ProductTypeId, QualityCheck = ExchangeObj.QualityCheck });
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("IsDtoCController", "SelectBP", ex);
            }

            return View(ExchangeObj);
        }

        #endregion

        #region Select QC And Uninstalation Price
        public ActionResult SelectQCTime(MyGateViewModel ExchangeObj)
        {
            _businessUnitRepository = new BusinessUnitRepository();
            _businessPartnerRepository = new BusinessPartnerRepository();
            tblBusinessUnit BusinessUnitObj = null;
            MyGateQCViewModel myGateDataContract = new MyGateQCViewModel();
            try
            {
                myGateDataContract.url = ConfigurationManager.AppSettings["Close"].ToString();
                myGateDataContract.BrandId = ExchangeObj.BrandId;
                myGateDataContract.BusinessPartnerId = ExchangeObj.BusinessPartnerId;
                myGateDataContract.priceMasterNameID = ExchangeObj.priceMasterNameID;
                myGateDataContract.BUId = ExchangeObj.BUId;
                myGateDataContract.ExchangePriceString = ExchangeObj.ExchangePriceString;
                myGateDataContract.ProductAge = ExchangeObj.ProductAge;
                myGateDataContract.ProductCategoryId = ExchangeObj.ProductCategoryId;
                myGateDataContract.ProductTypeId = ExchangeObj.ProductTypeId;
                myGateDataContract.StartTimeList = new List<SelectListItem>{ new SelectListItem { Text = "10AM-12PM", Value = "1" }
                                                                                    , new SelectListItem { Text = "12PM-2PM", Value = "2" }
                                                                                    , new SelectListItem { Text = "2PM-4PM", Value = "3" }
                                                                                    , new SelectListItem { Text = "4PM-6PM", Value = "4" }
                                                                                    , new SelectListItem { Text = "6PM-8PM", Value = "5" }};
                myGateDataContract.StartTimeList = myGateDataContract.StartTimeList.OrderBy(o => o.Value).ToList();
                myGateDataContract.TypeOfTV = new List<SelectListItem> { new SelectListItem { Text = "Table Mounted", Value = "1" }, new SelectListItem { Text = "Wall Mounted", Value = "2" } };
                myGateDataContract.TypeOfTV = myGateDataContract.TypeOfTV.OrderBy(o => o.Value).ToList();
               // BusinessUnitObj = _businessUnitRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == myGateDataContract.BUId);
                tblBusinessPartner tblBusinessPartner = _businessPartnerRepository.GetSingle(x => x.IsActive == true && x.BusinessPartnerId == ExchangeObj.BusinessPartnerId);
                if (tblBusinessPartner != null)
                {
                    if (tblBusinessPartner.LogoImage != null)
                    {
                        myGateDataContract.BULogoName = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Content/DB_Files/SponsorLogo/" + tblBusinessPartner.LogoImage;
                    }
                }
            }
            catch (Exception ex)
            {

                LibLogging.WriteErrorToDB("IsDtoCController", "SelectQCTime", ex);
            }
            return View(myGateDataContract);
        }
        [HttpPost]
        public ActionResult SelectQCTime(MyGateQCViewModel ExchangeObj)
        {

            try
            {
                if (ExchangeObj.QCDate != null && ExchangeObj.StartTime != null)
                {
                    if (ExchangeObj.BUId == 9)
                        return RedirectToAction("MyGateExchangeRegistration", "IsDtoC", new { BUId = ExchangeObj.BUId, BrandId = ExchangeObj.BrandId, ProductAge = ExchangeObj.ProductAge, FormatName = ExchangeObj.FormatName, ProductCategoryId = ExchangeObj.ProductCategoryId, ProductTypeId = ExchangeObj.ProductTypeId, ExchangePriceString = ExchangeObj.ExchangePriceString, QualityCheck = ExchangeObj.QualityCheck, StartTime = ExchangeObj.StartTime, QCDate = ExchangeObj.QCDate, IsUnInstallationRequired = ExchangeObj.IsUnInstallationRequired, UnInstallationAmount = ExchangeObj.UnInstallationAmount, Type = ExchangeObj.Type,priceMasterNameId= ExchangeObj.priceMasterNameID,BusinessParnterId = ExchangeObj.BusinessPartnerId, BasePrice = ExchangeObj.BasePrice, SweetenerTotal = ExchangeObj.SweetenerTotal, SweetenerBu = ExchangeObj.SweetenerBu, SweetenerBP = ExchangeObj.SweetenerBP, SweetenerDigi2L = ExchangeObj.SweetenerDigi2L });
                    else
                        return RedirectToAction("MyGateExchangeRegistration", "IsDtoC", new { BUId = ExchangeObj.BUId, BrandId = ExchangeObj.BrandId, ProductAge = ExchangeObj.ProductAge, FormatName = ExchangeObj.FormatName, ProductCategoryId = ExchangeObj.ProductCategoryId, ProductTypeId = ExchangeObj.ProductTypeId, QualityCheck = ExchangeObj.QualityCheck });
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("IsDtoCController", "SelectBP", ex);
            }

            return View(ExchangeObj);
        }
        #endregion

        #region Enter Customer Details
        public ActionResult MyGateExchangeRegistration(MyGateQCViewModel ExchangeObj)
        {
            List<tblBusinessPartner> businessPartnerList = null;
            tblBusinessPartner businessPartnerObj = new tblBusinessPartner();
            _businessPartnerRepository = new BusinessPartnerRepository();
            _businessUnitRepository = new BusinessUnitRepository();
            _exchangeOrderManager = new ExchangeOrderManager();
            _masterManager = new BAL.SponsorsApiCall.MasterManager();

            ExchangeOrderDataContract exchangeDataContract = new ExchangeOrderDataContract();
            tblBusinessUnit BusinessUnitObj = new tblBusinessUnit();
            try
            {
                exchangeDataContract.CityList = new List<SelectListItem>();
                exchangeDataContract.PincodeList = new List<SelectListItem>();
                exchangeDataContract.BusinessPartnerId = ExchangeObj.BusinessPartnerId;
                exchangeDataContract.priceMasterNameID = ExchangeObj.priceMasterNameID;
                exchangeDataContract.BusinessUnitId = ExchangeObj.BUId;
                BusinessUnitObj = _businessUnitRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == ExchangeObj.BUId);
                exchangeDataContract.BusinessUnitDataContract = _exchangeOrderManager.GetBUById(ExchangeObj.BUId);
                exchangeDataContract.CompanyName = BusinessUnitObj.Name;
                
                if (BusinessUnitObj != null)
                {
                    if (BusinessUnitObj.LogoName != null)
                    {
                        //exchangeDataContract.BULogoName = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Content/DB_Files/SponsorLogo/" + BusinessUnitObj.LogoName;
                    }
                    exchangeDataContract.ExpectedDeliveryHours = exchangeDataContract.BusinessUnitDataContract.ExpectedDeliveryHours;

                    exchangeDataContract.ZohoSponsorNumber = BusinessUnitObj.ZohoSponsorId;
                }

                tblBusinessPartner tblBusinessPartner = _businessPartnerRepository.GetSingle(x => x.IsActive == true && x.BusinessPartnerId == ExchangeObj.BusinessPartnerId);
                if (tblBusinessPartner != null)
                {
                    if (tblBusinessPartner.LogoImage != null)
                    {
                        exchangeDataContract.BULogoName = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Content/DB_Files/SponsorLogo/" + tblBusinessPartner.LogoImage;

                    }
                }

                        //get state list
                        DataTable dt = _businessPartnerRepository.GetStateList();
                if (dt != null && dt.Rows.Count > 0)
                {
                    businessPartnerList = GenericConversionHelper.DataTableToList<tblBusinessPartner>(dt);
                    List<string> states = businessPartnerList.Where(x => x.IsExchangeBP != null && x.IsExchangeBP == true).OrderBy(o => o.State).Select(x => x.State).Distinct().ToList();
                    List<SelectListItem> stateListItems = states.Select(x => new SelectListItem
                    {
                        Text = x,
                        Value = x
                    }).ToList();
                    ViewBag.StateList = new SelectList(stateListItems, "Text", "Text");
                }
                // Slot For Qc


                // ViewBag.TimeSlot = exchangeDataContract.StartTimeList;
                exchangeDataContract.QCDate = ExchangeObj.QCDate;
                exchangeDataContract.StartTime = ExchangeObj.StartTime;
                exchangeDataContract.BusinessUnitId = ExchangeObj.BUId;
                exchangeDataContract.BrandId = ExchangeObj.BrandId;
                exchangeDataContract.ProductTypeId = ExchangeObj.ProductTypeId;
                exchangeDataContract.ProductCategoryId = ExchangeObj.ProductCategoryId;
                exchangeDataContract.ProductAge = ExchangeObj.ProductAge;
                exchangeDataContract.FormatName = ExchangeObj.FormatName;
                exchangeDataContract.ExchangePriceString = ExchangeObj.ExchangePriceString;
                exchangeDataContract.stringUnInstallationPrice = ExchangeObj.UnInstallationAmount;
                exchangeDataContract.IsUnInstallationRequired = ExchangeObj.IsUnInstallationRequired;
                exchangeDataContract.QualityCheck = ExchangeObj.QualityCheck;

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("IsDtoC", "MyGateExchangeRegistration", ex);
            }
            return View(exchangeDataContract);
        }
        [HttpPost]
        public ActionResult MyGateExchangeRegistration(ExchangeOrderDataContract exchangeDataContract)
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
                    exchangeDataContract.SponsorOrderNumber = "MYG" + UniqueString.RandomNumberByLength(9);
                else
                    exchangeDataContract.SponsorOrderNumber = exchangeDataContract.SponsorOrderNumber.Trim();
                string sponsorOrderNo = exchangeDataContract.SponsorOrderNumber;
                if (String.IsNullOrEmpty(sponsorOrderNo) == false)
                {
                    SponserObj = _exchangeOrderRepository.GetSingle(x => !string.IsNullOrEmpty(x.SponsorOrderNumber) && x.SponsorOrderNumber == sponsorOrderNo);


                    if (SponserObj != null)
                    {
                        message = "This Exchange Order Number : " + exchangeDataContract.SponsorOrderNumber + " is already exist";
                    }
                    else
                    {
                        exchangeDataContract.CompanyName = exchangeDataContract.CompanyName;
                        exchangeDataContract.City = exchangeDataContract.City1;
                        exchangeDataContract.StateName = exchangeDataContract.State1;
                        exchangeDataContract.EstimatedDeliveryDate = DateTime.Now.AddHours(Convert.ToInt32(exchangeDataContract.ExpectedDeliveryHours)).ToString("dd-MM-yyyy");
                        exchangeDataContract.Bonus = "0";
                        if (exchangeDataContract.StartTime != null)
                        {
                            if (exchangeDataContract.StartTime.Equals("1"))
                            {
                                exchangeDataContract.StartTime = "10:00AM";
                                exchangeDataContract.EndTime = "12:00PM";
                            }
                            else if (exchangeDataContract.StartTime.Equals("2"))
                            {
                                exchangeDataContract.StartTime = "12:00PM";
                                exchangeDataContract.EndTime = "2:00PM";
                            }
                            else if (exchangeDataContract.StartTime.Equals("3"))
                            {
                                exchangeDataContract.StartTime = "2:00PM";
                                exchangeDataContract.EndTime = "4:00PM";
                            }
                            else if (exchangeDataContract.StartTime.Equals("4"))
                            {
                                exchangeDataContract.StartTime = "4:00PM";
                                exchangeDataContract.EndTime = "6:00PM";
                            }
                            else if (exchangeDataContract.StartTime.Equals("5"))
                            {
                                exchangeDataContract.StartTime = "6:00PM";
                                exchangeDataContract.EndTime = "8:00PM";
                            }

                        }
                        string StoreCode = ExchangeOrderManager.GetEnumDescription(StoreCodeEnum.Mygate);
                        tblBusinessPartner buisnessPartnerObj = _businessPartnerRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == exchangeDataContract.BusinessUnitId && x.StoreCode == StoreCode);
                        if (buisnessPartnerObj != null)
                        {
                            exchangeDataContract.BusinessPartnerId = buisnessPartnerObj.BusinessPartnerId;
                            exchangeDataContract.StoreCode = buisnessPartnerObj.StoreCode;
                            exchangeDataContract.SaleAssociateName = "My Gate Application";
                            exchangeDataContract.SaleAssociateCode = "12345678";
                            exchangeDataContract.IsD2C = Convert.ToBoolean(buisnessPartnerObj.IsD2C);
                            exchangeDataContract.IsDifferedSettlement = Convert.ToBoolean(buisnessPartnerObj.IsDefferedSettlement);
                        }
                        productOrderResponseDC = _exchangeOrderManager.ManageExchangeOrder(exchangeDataContract);

                        if (productOrderResponseDC != null)
                            message = "Thank you. Your product details have been received at  Digi2L.Your registration no. " +exchangeDataContract.RegdNo+ " . Our quality check team will soon knock at your door.";
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
                LibLogging.WriteErrorToDB("IsDtoC", "MyGateExchangeRegistration", ex);
            }

            return RedirectToAction("Details");
        }
        #endregion

        #region details
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
                LibLogging.WriteErrorToDB("IsDtoCController", "Details", ex);
            }
            return View();
        }
        #endregion

        #region Dealer Dashboard

        [HttpGet]
        public ActionResult MyGateDashBoard()
        {
            string msg = string.Empty;
            BusinessPartnerViewModel businessPartnerViewObj;
            _exchangeOrderManager = new ExchangeOrderManager();
            _exchangeOrderRepository = new ExchangeOrderRepository();
            _businessPartnerRepository = new BusinessPartnerRepository();
            _businessUnitRepository = new BusinessUnitRepository();
            List<tblBusinessPartner> businessPartnersList = new List<tblBusinessPartner>();
            List<tblExchangeOrder> exchangeOrdersList = new List<tblExchangeOrder>();
            ExchangeOrderDataContract ExchangeObj = new ExchangeOrderDataContract();
            tblBusinessUnit BusinessUnitObj = new tblBusinessUnit();
            ExchangeObj.CityList = new List<SelectListItem>();

            if (ValidateBPLogin())
            {
                SessionHelper _sessionHelper = (SessionHelper)Session["User"];
                ViewBag.LoginUser = _sessionHelper.LoggedUserInfo;
                ViewBag.IsDealer = _sessionHelper.LoggedUserInfo.IsDealer;
                ExchangeObj.Email = _sessionHelper.LoggedUserInfo.Email;
                ExchangeObj.BusinessUnitId = (int)_sessionHelper.LoggedUserInfo.BusinessUnitId;
                ExchangeObj.SaleAssociateCode = _sessionHelper.LoggedUserInfo.AssociateCode;
                ExchangeObj.City = _sessionHelper.LoggedUserInfo.City;
                int count = 0;
                int orderCount = 0;
                int DuplicateOrderCount = 0;
                int QCDeclinedCount = 0;
                int PriceAcceptedCount = 0;
                int PriceRejectedCount = 0;
                int QCRejectedCount = 0;
                int OrderCompletionCount = 0;
                int AppointmentDeclinedCount = 0;
                int PickupDeclinedCount = 0;
                int PickupAcceptedCount = 0;
                int TotalQCCount = 0;

                BusinessUnitObj = _businessUnitRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == (int)_sessionHelper.LoggedUserInfo.BusinessUnitId);
                if (BusinessUnitObj != null)
                {
                    tblBusinessPartner businessPartnerObj = _businessPartnerRepository.GetSingle(x => x.BusinessUnitId.Equals(BusinessUnitObj.BusinessUnitId) && x.Email.Equals(_sessionHelper.LoggedUserInfo.Email));
                    if (businessPartnerObj.DashBoardImage != null)
                    {

                        ExchangeObj.BULogoName = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Content/DB_Files/SponsorLogo/" + businessPartnerObj.DashBoardImage;
                    }
                }
                businessPartnersList = _businessPartnerRepository.GetList(x => x.Email == _sessionHelper.LoggedUserInfo.Email).ToList();
                foreach (var item in businessPartnersList)
                {
                    businessPartnerViewObj = new BusinessPartnerViewModel();
                    businessPartnerViewObj.BusinessPartnerId = item.BusinessPartnerId;
                    ExchangeObj.BusinessPartnerId = item.BusinessPartnerId;
                    ExchangeObj.AssociateName = item.Name;
                    if (businessPartnerViewObj != null)
                    {
                        exchangeOrdersList = _exchangeOrderRepository.GetList(x => x.BusinessPartnerId == businessPartnerViewObj.BusinessPartnerId).ToList();
                        count = exchangeOrdersList.Where(x => x.BusinessPartnerId == businessPartnerViewObj.BusinessPartnerId).Count();
                        DuplicateOrderCount = exchangeOrdersList.Where(x => x.BusinessPartnerId == businessPartnerViewObj.BusinessPartnerId && x.StatusId == 2).Count();
                        TotalQCCount = exchangeOrdersList.Where(x => x.BusinessPartnerId == businessPartnerViewObj.BusinessPartnerId && x.StatusId == 5).Count();
                        QCDeclinedCount = exchangeOrdersList.Where(x => x.BusinessPartnerId == businessPartnerViewObj.BusinessPartnerId && x.StatusId == 12).Count();
                        PriceAcceptedCount = exchangeOrdersList.Where(x => x.BusinessPartnerId == businessPartnerViewObj.BusinessPartnerId && x.StatusId == 15).Count();
                        PriceRejectedCount = exchangeOrdersList.Where(x => x.BusinessPartnerId == businessPartnerViewObj.BusinessPartnerId && x.StatusId == 16).Count();
                        OrderCompletionCount = exchangeOrdersList.Where(x => x.BusinessPartnerId == businessPartnerViewObj.BusinessPartnerId && x.StatusId == 30).Count();
                        QCRejectedCount = exchangeOrdersList.Where(x => x.BusinessPartnerId == businessPartnerViewObj.BusinessPartnerId && x.StatusId == 14).Count();
                        AppointmentDeclinedCount = exchangeOrdersList.Where(x => x.BusinessPartnerId == businessPartnerViewObj.BusinessPartnerId && x.StatusId == 26).Count();
                        PickupDeclinedCount = exchangeOrdersList.Where(x => x.BusinessPartnerId == businessPartnerViewObj.BusinessPartnerId && x.StatusId == 21).Count();
                        PickupAcceptedCount = exchangeOrdersList.Where(x => x.BusinessPartnerId == businessPartnerViewObj.BusinessPartnerId && x.StatusId == 23).Count();
                    }
                    else
                    {
                        msg = "No Records Found";
                        TempData["Msg"] = msg;
                        return RedirectToAction("Details");
                    }

                    orderCount += count;
                }
                ViewBag.NoOfOrders = orderCount;
                ViewBag.DuplicateOrder = DuplicateOrderCount;
                ViewBag.TotalQcStatus = TotalQCCount;
                ViewBag.QCDeclined = QCDeclinedCount;
                ViewBag.PriceAcccepted = PriceAcceptedCount;
                ViewBag.PriceRejected = PriceRejectedCount;
                ViewBag.OrderCompleted = OrderCompletionCount;
                ViewBag.QCRejected = QCRejectedCount;
                ViewBag.AppointmentDeclined = AppointmentDeclinedCount;
                ViewBag.PickupDeclined = PickupDeclinedCount;
                ViewBag.PickupAccepted = PickupAcceptedCount;

                return View(ExchangeObj);
            }
            else
            {
                return RedirectToAction("Login", "IsDtoC", new { Area = "" });
            }

        }
        #endregion

        #region get order count of individual BP
        public JsonResult GetMyGateOrder(ExchangeOrderDataContract exchangeOrderData)
        {
            _exchangeOrderManager = new ExchangeOrderManager();
            var exchangeOrderObj = _exchangeOrderManager.GetMyGateExchangeOrderByBPId(exchangeOrderData);
            return Json(new { data = exchangeOrderObj }, JsonRequestBehavior.AllowGet);
        }
        #endregion
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
                    return RedirectToAction("MyGateDashBoard", "IsDtoC", new { buid = businessPartnerVM.BusinessUnitId });
                }
                else
                {
                    TempData["Auth"] = false;
                    return RedirectToAction("Login", "IsDtoC", new { Area = "" });
                }
            }
            else
            {
                TempData["Auth"] = false;
                return RedirectToAction("Login", "IsDtoC", new { Area = "" });
            }

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

        #endregion
        #region Logout
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Login", "IsDtoC");
        }
        #endregion

        #region Product details With status
        [HttpGet]
        public ActionResult ProductDiscription()
        {
            string msg = string.Empty;
            BusinessPartnerViewModel businessPartnerViewObj;
            _exchangeOrderManager = new ExchangeOrderManager();
            _exchangeOrderRepository = new ExchangeOrderRepository();
            _businessPartnerRepository = new BusinessPartnerRepository();
            _businessUnitRepository = new BusinessUnitRepository();
            List<tblBusinessPartner> businessPartnersList = new List<tblBusinessPartner>();
            List<tblExchangeOrder> exchangeOrdersList = new List<tblExchangeOrder>();
            ExchangeOrderDataContract ExchangeObj = new ExchangeOrderDataContract();
            tblBusinessUnit BusinessUnitObj = new tblBusinessUnit();
            ExchangeObj.CityList = new List<SelectListItem>();

            if (ValidateBPLogin())
            {
                SessionHelper _sessionHelper = (SessionHelper)Session["User"];
                ViewBag.LoginUser = _sessionHelper.LoggedUserInfo;
                ViewBag.IsDealer = _sessionHelper.LoggedUserInfo.IsDealer;
                ExchangeObj.Email = _sessionHelper.LoggedUserInfo.Email;
                ExchangeObj.BusinessUnitId = (int)_sessionHelper.LoggedUserInfo.BusinessUnitId;
                ExchangeObj.SaleAssociateCode = _sessionHelper.LoggedUserInfo.AssociateCode;
                ExchangeObj.City = _sessionHelper.LoggedUserInfo.City;
                int count = 0;
                int orderCount = 0;
                BusinessUnitObj = _businessUnitRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == (int)_sessionHelper.LoggedUserInfo.BusinessUnitId);
                if (BusinessUnitObj != null)
                {
                    tblBusinessPartner businessPartnerObj = _businessPartnerRepository.GetSingle(x => x.BusinessUnitId.Equals(BusinessUnitObj.BusinessUnitId) && x.Email.Equals(_sessionHelper.LoggedUserInfo.Email));
                    if (businessPartnerObj.DashBoardImage != null)
                    {

                        ExchangeObj.BULogoName = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Content/DB_Files/SponsorLogo/" + businessPartnerObj.DashBoardImage;
                    }
                }
                businessPartnersList = _businessPartnerRepository.GetList(x => x.Email == _sessionHelper.LoggedUserInfo.Email).ToList();
                foreach (var item in businessPartnersList)
                {
                    businessPartnerViewObj = new BusinessPartnerViewModel();
                    businessPartnerViewObj.BusinessPartnerId = item.BusinessPartnerId;
                    ExchangeObj.BusinessPartnerId = item.BusinessPartnerId;

                    if (businessPartnerViewObj != null)
                    {
                        exchangeOrdersList = _exchangeOrderRepository.GetList(x => x.BusinessPartnerId == businessPartnerViewObj.BusinessPartnerId).ToList();
                        count = exchangeOrdersList.Where(x => x.BusinessPartnerId == businessPartnerViewObj.BusinessPartnerId).Count();
                    }
                    else
                    {
                        msg = "No Records Found";
                        TempData["Msg"] = msg;
                        return RedirectToAction("Details");
                    }

                    orderCount += count;
                }
                ViewBag.NoOfOrders = orderCount;
                return View(ExchangeObj);
            }
            else
            {
                return RedirectToAction("Login", "IsDtoC", new { Area = "" });
            }
        }
        #endregion

        #region City And pincode
        [HttpPost]
        public JsonResult GetPincodeForMyGate(string pintext,int? buid)
        {
            _pincodeRepository = new PinCodeRepository();
            IEnumerable<SelectListItem> pincodeList = null;
            List<tblPinCode> pincodeMasterList = null;
            try
            {

                DataTable dt = _pincodeRepository.GetPincodeListbybuidforex(pintext, buid);

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
                //DataTable dt = _pincodeRepository.GetPincodeListForALlianceD2C();
                //if (dt != null && dt.Rows.Count > 0)
                //{
                //    pincodeMasterList = GenericConversionHelper.DataTableToList<tblPinCode>(dt);
                //}
                //pincodeList = (pincodeMasterList).AsEnumerable().Select(prodt => new SelectListItem() { Text = prodt.ZipCode.ToString(), Value = prodt.ZipCode.ToString() });
                //pincodeList = pincodeList.OrderBy(o => o.Text).ToList();
                //pincodeList = pincodeList.Where(x => x.Text.Contains(pintext)).ToList();
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("IsDtoC", "GetPincodeForMyGate", ex);
            }
            var result = new SelectList(pincodeList, "Value", "Text");
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetState(string pincode)
        {
            _pincodeRepository = new PinCodeRepository();
            List<tblPinCode> pincodeMasterList = null;
            MyGateCityState mygateCityState = new MyGateCityState();
            try
            {
                DataTable dt = _pincodeRepository.GetStateList(pincode);
                if (dt != null && dt.Rows.Count > 0)
                {
                    pincodeMasterList = GenericConversionHelper.DataTableToList<tblPinCode>(dt);
                }
                if (pincodeMasterList != null)
                {
                    foreach (var item in pincodeMasterList)
                    {
                        mygateCityState.StateName = item.State;
                        mygateCityState.CityName = item.Location;
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("IsDtoC", "GetState", ex);
            }

            return Json(mygateCityState, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region unInstallation Price
        [HttpGet]
        public JsonResult GetUnInstallationPriceTV(int productCatId, int productType, string IsUnInstallationrequired, string Type)
        {
            _masterManager = new BAL.SponsorsApiCall.MasterManager();
            string price = null;
            try
            {
                price = _masterManager.GetUnInstallationAmount(productCatId, productType, IsUnInstallationrequired, Type);
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeController", "GetProdPrice", ex);
            }

            return Json(price, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetUnInstallationPriceAll(int productCatId, int productType, string IsUnInstallationrequired)
        {
            _masterManager = new BAL.SponsorsApiCall.MasterManager();
            string price = null;
            string type = string.Empty;
            try
            {
                price = _masterManager.GetUnInstallationAmount(productCatId, productType, IsUnInstallationrequired, type);
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeController", "GetProdPrice", ex);
            }

            return Json(price, JsonRequestBehavior.AllowGet);
        }
        #endregion


        // GET: Category type by Category Id
        [HttpGet]
        public JsonResult GetProdTypeByProdGroupId(int productCatId, string pricecode)
        {
            _productTypeRepository = new ProductTypeRepository();
            List<SelectListItem> prodType = null;
            _priceMasterRepository = new PriceMasterRepository();
            SelectList result = null;
            List<tblPriceMaster> pricemaster = null;
            List<tblProductType> prodsubGroupListForExchange = new List<tblProductType>();
            tblProductType typeObj = null;
            try
            {

                if (pricecode != null)
                {
                    prodType = new List<SelectListItem>();
                    DataTable dtProductType = _priceMasterRepository.GetProducttypeByPriceCode(pricecode, productCatId);
                    if (dtProductType != null && dtProductType.Rows.Count > 0)
                    {
                        pricemaster = GenericConversionHelper.DataTableToList<tblPriceMaster>(dtProductType);
                        foreach (var productType in pricemaster)
                        {
                            typeObj = _productTypeRepository.GetSingle(x => x.IsActive == true && x.IsAllowedForOld == true && x.Id == productType.ProductTypeId);
                            if (typeObj != null)
                            {
                                prodsubGroupListForExchange.Add(typeObj);
                                if (typeObj.Size != null)
                                {
                                    prodType.Add(new SelectListItem() { Text = typeObj.Description + "(" + typeObj.Size + ")", Value = typeObj.Id.ToString() });
                                }
                                else
                                {
                                    prodType.Add(new SelectListItem() { Text = typeObj.Description, Value = typeObj.Id.ToString() });
                                }
                            }
                        }
                    }
                    {
                        prodType = prodType.OrderBy(o => o.Text).ToList();
                        result = new SelectList(prodType, "Value", "Text");
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeController", "GetProdTypeByProdGroupId", ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
        #region Old Product Detail and Price For Direct To Customer
        public ActionResult ProductDetailsForD2C(int BUId, int BPID)
        {
            MyGateViewModel myGateDataContract = new MyGateViewModel();
            logging = new Logging();
            _exchangeOrderManager = new BAL.SponsorsApiCall.ExchangeOrderManager();
            _businessUnitRepository = new BusinessUnitRepository();
            _businessPartnerRepository = new BusinessPartnerRepository();
            _productCategoryRepository = new ProductCategoryRepository();
            _loginRepository = new LoginDetailsUTCRepository();
            _priceMasterRepository = new PriceMasterRepository();
            _productConditionLabelRepository = new ProductConditionLabelRepository();
            LodhaGroupHaders jsondata = new LodhaGroupHaders();
            tblBusinessUnit BusinessUnitObj = null;
            tblBusinessPartner BusinessPartnerObj = null;
            //List<tblPriceMaster> pricemaster = null;
            List<tblProductCategory> prodGroupListForExchange = new List<tblProductCategory>();
            //tblProductCategory categoryObj = null;
            List<tblProductConditionLabel> ConditionLabelObj = new List<tblProductConditionLabel>();
            try
            {
                myGateDataContract.url = ConfigurationManager.AppSettings["Close"].ToString();
                myGateDataContract.ProductAge = Convert.ToInt32(ProductAgeEnum.ProductAge);
                myGateDataContract.BusinessPartnerId = BPID;
                myGateDataContract.ProductTypeList = new List<SelectListItem>();
                myGateDataContract.ProductModelList = new List<SelectListItem>();
                myGateDataContract.BrandList = new List<SelectListItem>();
                myGateDataContract.BUId = BUId;
                HeaderValuesLodha headersdata = new HeaderValuesLodha();

                BusinessUnitObj = _businessUnitRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == BUId);
                if (BusinessUnitObj != null)
                {
                    // Code  to get Product condition labels By vishal choudhary date[15/06/2023]
                    ConditionLabelObj = _productConditionLabelRepository.GetList(x => x.BusinessUnitId == BusinessUnitObj.BusinessUnitId && x.BusinessPartnerId ==myGateDataContract.BusinessPartnerId && x.IsActive == true).ToList();
                    if (ConditionLabelObj.Count > 0)
                    {
                        myGateDataContract.ProductConditionCount = ConditionLabelObj.Count;
                        myGateDataContract.QualityCheckList = ConditionLabelObj.Select(x => new SelectListItem
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
                    myGateDataContract.QualityCheckList = myGateDataContract.QualityCheckList.OrderByDescending(o => o.Value).ToList();
                    BusinessPartnerObj = _businessPartnerRepository.GetSingle(x => x.BusinessUnitId == BUId && x.BusinessPartnerId == BPID && x.IsActive == true);
                    if (BusinessPartnerObj != null)
                    {
                        myGateDataContract.BULogoName = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Content/DB_Files/SponsorLogo/" + BusinessPartnerObj.LogoImage;
                        myGateDataContract.FormatName = BusinessPartnerObj.FormatName;

                        string StoreCodeForLodhaGroup = ExchangeOrderManager.GetEnumDescription(StoreCodeEnum.LodhaGroup);
                        if (BusinessPartnerObj.StoreCode == StoreCodeForLodhaGroup)
                        {
                            try
                            {
                                //Code To Get Header values from url (By Vishal Choudhary)
                                AES256encoding aES256Encoding = new AES256encoding();
                                var headers = Request.Headers;
                                string plainText = string.Empty;
                                string thirdParty = headers.GetValues("Third-Party").FirstOrDefault();
                                string BelleVie = headers.GetValues("BelleVie-IV").FirstOrDefault();
                                //string BelleVie = "z29pujVTLzIs7h6y"; 
                                string BelivieData = headers.GetValues("BelleVie-Data").FirstOrDefault();
                                string ApiKey = ConfigurationManager.AppSettings["LodhaApiKey"].ToString();
                                headersdata.apiKey = ApiKey;
                                headersdata.bellevieData = BelivieData;
                                headersdata.Beleviee = BelleVie;

                               if (BelleVie != null && BelivieData != null)
                                {
                                    //<Summary>
                                    // <>Code Added to perform decryption using aes 256 cbc algotithm on headers passed by lodha group<by Vishal Choudhary>
                                    //string DataString = "{\"firstName\": \"Kushal\",\"lastName\": \"Panchal\",\"mobileNo\": \"+91 7666124501\",\"email\":\"kushal.panchal@lodhagroup.com\"}"; 
                                    using (AesManaged aes = new AesManaged())
                                    {
                                        // Encrypt string    
                                        //byte[] encrypted = Encrypt(raw, aes.Key, aes.IV);
                                        // Print encrypted string    
                                        //string basestring = AES256encoding.Encrypt(DataString, ApiKey, BelleVie);
                                        // Decrypt the bytes to a string.    
                                        string decrypted = AES256encoding.Decrypt(BelivieData, ApiKey, BelleVie);
                                        // Print decrypted string. It should be same as raw data.
                                        if (decrypted != null)
                                        {
                                            logging.WriteAPIRequestToDB("IsDtoCController", "ProductDetailsForD2C", BusinessPartnerObj.StoreCode, decrypted);
                                            jsondata = JsonConvert.DeserializeObject<LodhaGroupHaders>(decrypted);
                                        }
                                    }
                                    myGateDataContract.firstName = jsondata.firstName;
                                    myGateDataContract.lastName = jsondata.lastName;
                                    myGateDataContract.email = jsondata.email;
                                    string myString = jsondata.mobileNo;
                                    myGateDataContract.mobileNoWithCountryCode = myString;
                                    jsondata.mobileNo = myString.Substring(myString.Length - 10);
                                    myGateDataContract.mobileNo = jsondata.mobileNo;
                               }
                            }
                            catch(Exception ex1)
                            {
                                string HeaddersString = JsonConvert.SerializeObject(headersdata);
                                logging.WriteAPIRequestToDB("IsDtoCController", "ProductDetailsForD2C", BusinessPartnerObj.StoreCode, HeaddersString);
                                LibLogging.WriteErrorToDB("IsDtoCController", "ProductDetailsForD2C", ex1);
                            }
                        }
                    }
                    myGateDataContract.IsSweetnerModelBased = (bool)BusinessUnitObj.IsSweetnerModelBased;
                    Login loginObj = _loginRepository.GetSingle(x => x.SponsorId == myGateDataContract.BUId);
                    if (loginObj != null)
                    {
                        myGateDataContract.PriceCode = loginObj.PriceCode;
                    }
                   
                    if (myGateDataContract.BUId > 0 && myGateDataContract.BusinessPartnerId>0)
                    {
                        OldProductDetailsManager oldProductDetailsManager = new OldProductDetailsManager();
                        PriceMasterMappingDataContract priceMasterMappingDataContract = new PriceMasterMappingDataContract();
                        myGateDataContract.BusinessUnitId = myGateDataContract.BUId;
                        priceMasterMappingDataContract.BusinessunitId = myGateDataContract.BUId;
                        priceMasterMappingDataContract.BusinessPartnerId = myGateDataContract.BusinessPartnerId;
                        PriceMasterNameDataContract priceMasterNameDataContract = new PriceMasterNameDataContract();
                        priceMasterNameDataContract = oldProductDetailsManager.GetPriceNameId(priceMasterMappingDataContract);

                        if (priceMasterNameDataContract != null && priceMasterNameDataContract.PriceNameId > 0)
                        {
                            priceMasterNameDataContract.PriceNameId = priceMasterNameDataContract.PriceNameId;
                            myGateDataContract.priceMasterNameID = Convert.ToInt32(priceMasterNameDataContract.PriceNameId);

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


                    if (BusinessUnitObj.IsQCDateTimeRequiredOnD2C == true)
                    {
                        myGateDataContract.ShowQCTimeandDatepage = BusinessUnitObj.IsQCDateTimeRequiredOnD2C;
                    }
                    else
                    {
                        myGateDataContract.ShowQCTimeandDatepage = false;
                    }
                  
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("IsDtoCController", "ProductDetailsForD2C", ex);
            }
            return View(myGateDataContract);
        }
        [HttpPost]
        public ActionResult ProductDetailsForD2C(MyGateViewModel ExchangeObj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (ExchangeObj.ShowQCTimeandDatepage == true)
                    {
                        if (ExchangeObj.BUId > 0)
                            return RedirectToAction("SelectQCTimeForD2C", "IsDtoC", new { BUId = ExchangeObj.BUId, BrandId = ExchangeObj.BrandId, ProductAge = ExchangeObj.ProductAge, FormatName = ExchangeObj.FormatName, ProductCategoryId = ExchangeObj.ProductCategoryId, ProductTypeId = ExchangeObj.ProductTypeId, ExchangePriceString = ExchangeObj.ExchangePriceString, QualityCheck = ExchangeObj.QualityCheck, BULogoName = ExchangeObj.BULogoName, BusinessPartnerId = ExchangeObj.BusinessPartnerId, firstName = ExchangeObj.firstName, lastName = ExchangeObj.lastName, email = ExchangeObj.email, mobileNo = ExchangeObj.mobileNo, mobileNoWithCountryCode = ExchangeObj.mobileNoWithCountryCode, priceMasterNameID = ExchangeObj.priceMasterNameID, BasePrice = ExchangeObj.BasePrice, SweetenerTotal = ExchangeObj.SweetenerTotal, SweetenerBu = ExchangeObj.SweetenerBu, SweetenerBP = ExchangeObj.SweetenerBP, SweetenerDigi2L = ExchangeObj.SweetenerDigi2L });

                    }
                    else
                    {
                            return RedirectToAction("D2CRegistration", "IsDtoC", new { BUId = ExchangeObj.BUId, BrandId = ExchangeObj.BrandId, ProductAge = ExchangeObj.ProductAge, FormatName = ExchangeObj.FormatName, ProductCategoryId = ExchangeObj.ProductCategoryId, ProductTypeId = ExchangeObj.ProductTypeId, ExchangePriceString = ExchangeObj.ExchangePriceString, QualityCheck = ExchangeObj.QualityCheck, BULogoName = ExchangeObj.BULogoName, BusinessPartnerId = ExchangeObj.BusinessPartnerId, firstName = ExchangeObj.firstName, lastName = ExchangeObj.lastName, email = ExchangeObj.email, mobileNo = ExchangeObj.mobileNo, mobileNoWithCountryCode = ExchangeObj.mobileNoWithCountryCode, priceMasterNameID = ExchangeObj.priceMasterNameID, BasePrice = ExchangeObj.BasePrice, SweetenerTotal = ExchangeObj.SweetenerTotal, SweetenerBu = ExchangeObj.SweetenerBu, SweetenerBP = ExchangeObj.SweetenerBP, SweetenerDigi2L = ExchangeObj.SweetenerDigi2L });

                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("IsDtoCController", "ProductDetailsForD2C", ex);
            }

            return View(ExchangeObj);
        }
        #endregion

        #region Select QC And Uninstalation Price
        public ActionResult SelectQCTimeForD2C(MyGateViewModel ExchangeObj)
        {
            _businessUnitRepository = new BusinessUnitRepository();
            _businessPartnerRepository = new BusinessPartnerRepository();
            tblBusinessUnit BusinessUnitObj = null;
            tblBusinessPartner BusinessPartnerObj = null;
            MyGateQCViewModel myGateDataContract = new MyGateQCViewModel();
            try
            {
                myGateDataContract.url = ConfigurationManager.AppSettings["Close"].ToString();
                myGateDataContract.BrandId = ExchangeObj.BrandId;
                myGateDataContract.BUId = ExchangeObj.BUId;
                myGateDataContract.priceMasterNameID = ExchangeObj.priceMasterNameID;
                myGateDataContract.ExchangePriceString = ExchangeObj.ExchangePriceString;
                myGateDataContract.ProductAge = ExchangeObj.ProductAge;
                myGateDataContract.ProductCategoryId = ExchangeObj.ProductCategoryId;
                myGateDataContract.ProductTypeId = ExchangeObj.ProductTypeId;
                myGateDataContract.BusinessPartnerId = ExchangeObj.BusinessPartnerId;
                myGateDataContract.mobileNoWithCountryCode = ExchangeObj.mobileNoWithCountryCode;
                if (ExchangeObj.BUId == Convert.ToInt32(BusinessUnitEnum.Samsung) || ExchangeObj.BUId == Convert.ToInt32(BusinessUnitEnum.WhirlPool))
                {

                    myGateDataContract.SamsungBU = ExchangeObj.BUId;
                }
                myGateDataContract.firstName = ExchangeObj.firstName;
                myGateDataContract.lastName = ExchangeObj.lastName;
                myGateDataContract.email = ExchangeObj.email;
                myGateDataContract.mobileNo = ExchangeObj.mobileNo;
                myGateDataContract.StartTimeList = new List<SelectListItem>{ new SelectListItem { Text = "10AM-12PM", Value = "1" }
                                                                                    , new SelectListItem { Text = "12PM-2PM", Value = "2" }
                                                                                    , new SelectListItem { Text = "2PM-4PM", Value = "3" }
                                                                                    , new SelectListItem { Text = "4PM-6PM", Value = "4" }
                                                                                    , new SelectListItem { Text = "6PM-8PM", Value = "5" }};
                myGateDataContract.StartTimeList = myGateDataContract.StartTimeList.OrderBy(o => o.Value).ToList();
                myGateDataContract.TypeOfTV = new List<SelectListItem> { new SelectListItem { Text = "Table Mounted", Value = "1" }, new SelectListItem { Text = "Wall Mounted", Value = "2" } };
                myGateDataContract.TypeOfTV = myGateDataContract.TypeOfTV.OrderBy(o => o.Value).ToList();
                BusinessUnitObj = _businessUnitRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == myGateDataContract.BUId);
                if (BusinessUnitObj != null)
                {
                    BusinessPartnerObj = _businessPartnerRepository.GetSingle(x => x.BusinessUnitId == myGateDataContract.BUId && x.BusinessPartnerId == myGateDataContract.BusinessPartnerId);
                    if (BusinessPartnerObj != null)
                    {
                        myGateDataContract.BULogoName = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Content/DB_Files/SponsorLogo/" + BusinessPartnerObj.LogoImage;
                    }
                }
            }
            catch (Exception ex)
            {

                LibLogging.WriteErrorToDB("IsDtoCController", "SelectQCTime", ex);
            }
            return View(myGateDataContract);
        }
        [HttpPost]
        public ActionResult SelectQCTimeForD2C(MyGateQCViewModel ExchangeObj)
        {

            try
            {

                if (ExchangeObj.BUId > 0)
                    if (ExchangeObj.QCTime != null && ExchangeObj.QCDate1 != null)
                    {
                        ExchangeObj.QCDate = ExchangeObj.QCDate1;
                        ExchangeObj.StartTime = ExchangeObj.QCTime;
                    }
                return RedirectToAction("D2CRegistration", "IsDtoC", new { BUId = ExchangeObj.BUId, BrandId = ExchangeObj.BrandId, ProductAge = ExchangeObj.ProductAge, FormatName = ExchangeObj.FormatName, ProductCategoryId = ExchangeObj.ProductCategoryId, ProductTypeId = ExchangeObj.ProductTypeId, ExchangePriceString = ExchangeObj.ExchangePriceString, QualityCheck = ExchangeObj.QualityCheck, StartTime = ExchangeObj.StartTime, QCDate = ExchangeObj.QCDate, IsUnInstallationRequired = ExchangeObj.IsUnInstallationRequired, UnInstallationAmount = ExchangeObj.UnInstallationAmount, Type = ExchangeObj.Type, BusinessPartnerId = ExchangeObj.BusinessPartnerId, firstName = ExchangeObj.firstName, lastName = ExchangeObj.lastName, email = ExchangeObj.email, mobileNo = ExchangeObj.mobileNo, mobileNoWithCountryCode = ExchangeObj.mobileNoWithCountryCode, priceMasterNameID=ExchangeObj.priceMasterNameID, BasePrice = ExchangeObj.BasePrice, SweetenerTotal = ExchangeObj.SweetenerTotal, SweetenerBu = ExchangeObj.SweetenerBu, SweetenerBP = ExchangeObj.SweetenerBP, SweetenerDigi2L = ExchangeObj.SweetenerDigi2L });
                //else if (ExchangeObj.BUId == 9)
                //{
                //    return RedirectToAction("D2CRegistration", "IsDtoC", new { BUId = ExchangeObj.BUId, BrandId = ExchangeObj.BrandId, ProductAge = ExchangeObj.ProductAge, FormatName = ExchangeObj.FormatName, ProductCategoryId = ExchangeObj.ProductCategoryId, ProductTypeId = ExchangeObj.ProductTypeId, ExchangePriceString = ExchangeObj.ExchangePriceString, QualityCheck = ExchangeObj.QualityCheck, StartTime = ExchangeObj.StartTime, QCDate = ExchangeObj.QCDate, IsUnInstallationRequired = ExchangeObj.IsUnInstallationRequired, UnInstallationAmount = ExchangeObj.UnInstallationAmount, Type = ExchangeObj.Type, BusinessPartnerId = ExchangeObj.BusinessPartnerId });
                //}


            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("IsDtoCController", "SelectBP", ex);
            }

            return View(ExchangeObj);
        }
        #endregion

        #region Enter Customer Details
        public ActionResult D2CRegistration(MyGateQCViewModel ExchangeObj)
        {
            List<tblBusinessPartner> businessPartnerList = null;
            tblBusinessPartner businessPartnerObj = new tblBusinessPartner();
            _businessPartnerRepository = new BusinessPartnerRepository();
            _businessUnitRepository = new BusinessUnitRepository();
            _exchangeOrderManager = new ExchangeOrderManager();
            _masterManager = new BAL.SponsorsApiCall.MasterManager();

            ExchangeOrderDataContract exchangeDataContract = new ExchangeOrderDataContract();
            tblBusinessUnit BusinessUnitObj = new tblBusinessUnit();
            try
            {
                exchangeDataContract.CityList = new List<SelectListItem>();
                exchangeDataContract.PincodeList = new List<SelectListItem>();
                exchangeDataContract.BusinessPartnerId = ExchangeObj.BusinessPartnerId;
                exchangeDataContract.BusinessUnitId = ExchangeObj.BUId;
                exchangeDataContract.priceMasterNameID = ExchangeObj.priceMasterNameID;
                exchangeDataContract.FirstName = ExchangeObj.firstName;
                exchangeDataContract.LastName = ExchangeObj.lastName;
                exchangeDataContract.Email = ExchangeObj.email;
                exchangeDataContract.PhoneNumber = ExchangeObj.mobileNo;
                exchangeDataContract.mobileNoWithCountryCode = ExchangeObj.mobileNoWithCountryCode;
                BusinessUnitObj = _businessUnitRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == ExchangeObj.BUId);

               
                    if (BusinessUnitObj.IsSponsorNumberRequiredOnUI == true)
                    {
                        exchangeDataContract.IsSponsorLogorequiredOnUI = BusinessUnitObj.IsSponsorNumberRequiredOnUI;
                    }
                    else
                    {
                        exchangeDataContract.IsSponsorLogorequiredOnUI = false;
                    }
                    
               
                exchangeDataContract.BusinessUnitDataContract = _exchangeOrderManager.GetBUById(ExchangeObj.BUId);
                exchangeDataContract.CompanyName = BusinessUnitObj.Name;
                if (BusinessUnitObj != null)
                {
                    businessPartnerObj = _businessPartnerRepository.GetSingle(x => x.BusinessUnitId == ExchangeObj.BUId && x.BusinessPartnerId == ExchangeObj.BusinessPartnerId);
                    if (businessPartnerObj != null)
                    {
                        exchangeDataContract.BULogoName = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Content/DB_Files/SponsorLogo/" + businessPartnerObj.LogoImage;
                        
                        if (businessPartnerObj.IsOtpRequired == true)
                        {
                            exchangeDataContract.IsOtpRequired = (bool)businessPartnerObj.IsOtpRequired;
                        }
                        else
                        {
                            exchangeDataContract.IsOtpRequired = false;
                        }
                    }
                    exchangeDataContract.ExpectedDeliveryHours = exchangeDataContract.BusinessUnitDataContract.ExpectedDeliveryHours;
                    exchangeDataContract.ZohoSponsorNumber = BusinessUnitObj.ZohoSponsorId;
                    exchangeDataContract.showEmployeeId = BusinessUnitObj.ShowEmplyeeCode;
                    exchangeDataContract.IsSFIDRequired = BusinessUnitObj.IsSFIDRequired;
                }
                //get state list
                DataTable dt = _businessPartnerRepository.GetStateList();
                if (dt != null && dt.Rows.Count > 0)
                {
                    businessPartnerList = GenericConversionHelper.DataTableToList<tblBusinessPartner>(dt);
                    List<string> states = businessPartnerList.Where(x => x.IsExchangeBP != null && x.IsExchangeBP == true).OrderBy(o => o.State).Select(x => x.State).Distinct().ToList();
                    List<SelectListItem> stateListItems = states.Select(x => new SelectListItem
                    {
                        Text = x,
                        Value = x
                    }).ToList();
                    ViewBag.StateList = new SelectList(stateListItems, "Text", "Text");
                }
                // Slot For Qc
                // ViewBag.TimeSlot = exchangeDataContract.StartTimeList;
                exchangeDataContract.QCDate = ExchangeObj.QCDate;
                exchangeDataContract.StartTime = ExchangeObj.StartTime;
                exchangeDataContract.BusinessUnitId = ExchangeObj.BUId;
                exchangeDataContract.BrandId = ExchangeObj.BrandId;
                exchangeDataContract.ProductTypeId = ExchangeObj.ProductTypeId;
                exchangeDataContract.ProductCategoryId = ExchangeObj.ProductCategoryId;
                exchangeDataContract.ProductAge = ExchangeObj.ProductAge;
                exchangeDataContract.FormatName = ExchangeObj.FormatName;
                exchangeDataContract.ExchangePriceString = ExchangeObj.ExchangePriceString;
                exchangeDataContract.stringUnInstallationPrice = ExchangeObj.UnInstallationAmount;
                exchangeDataContract.IsUnInstallationRequired = ExchangeObj.IsUnInstallationRequired;
                exchangeDataContract.QualityCheck = ExchangeObj.QualityCheck;

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("IsDtoC", "D2CRegistration", ex);
            }
            return View(exchangeDataContract);
        }
        [HttpPost]
        public ActionResult D2CRegistration(ExchangeOrderDataContract exchangeDataContract)
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
                if (exchangeDataContract.BusinessUnitId != exchangeDataContract.BusinessUnitSponser)
                {
                    if (exchangeDataContract.BusinessUnitId.Equals(9))
                    {
                        if (string.IsNullOrEmpty(exchangeDataContract.SponsorOrderNumber))
                            exchangeDataContract.SponsorOrderNumber = "ALC" + UniqueString.RandomNumberByLength(9);
                        else
                            exchangeDataContract.SponsorOrderNumber = exchangeDataContract.SponsorOrderNumber.Trim();
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(exchangeDataContract.SponsorOrderNumber))
                            exchangeDataContract.SponsorOrderNumber = "D2C" + UniqueString.RandomNumberByLength(9);
                        else
                            exchangeDataContract.SponsorOrderNumber = exchangeDataContract.SponsorOrderNumber.Trim();
                    }
                }

                string sponsorOrderNo = exchangeDataContract.SponsorOrderNumber.Trim();
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
                        exchangeDataContract.City = exchangeDataContract.City1;
                        exchangeDataContract.StateName = exchangeDataContract.State1;

                        exchangeDataContract.EstimatedDeliveryDate = DateTime.Now.AddHours(Convert.ToInt32(exchangeDataContract.ExpectedDeliveryHours)).ToString("dd-MM-yyyy");
                        exchangeDataContract.Bonus = "0";
                        if (exchangeDataContract.StartTime != null)
                        {
                            if (exchangeDataContract.StartTime.Equals("1"))
                            {
                                exchangeDataContract.StartTime = "10:00AM";
                                exchangeDataContract.EndTime = "12:00PM";
                            }
                            else if (exchangeDataContract.StartTime.Equals("2"))
                            {
                                exchangeDataContract.StartTime = "12:00PM";
                                exchangeDataContract.EndTime = "2:00PM";
                            }
                            else if (exchangeDataContract.StartTime.Equals("3"))
                            {
                                exchangeDataContract.StartTime = "2:00PM";
                                exchangeDataContract.EndTime = "4:00PM";
                            }
                            else if (exchangeDataContract.StartTime.Equals("4"))
                            {
                                exchangeDataContract.StartTime = "4:00PM";
                                exchangeDataContract.EndTime = "6:00PM";
                            }
                            else if (exchangeDataContract.StartTime.Equals("5"))
                            {
                                exchangeDataContract.StartTime = "6:00PM";
                                exchangeDataContract.EndTime = "8:00PM";
                            }

                        }
                        tblBusinessPartner buisnessPartnerObj = _businessPartnerRepository.GetSingle(x => x.IsActive == true && x.BusinessPartnerId == exchangeDataContract.BusinessPartnerId);
                        if (buisnessPartnerObj != null)
                        {
                            exchangeDataContract.BusinessPartnerId = buisnessPartnerObj.BusinessPartnerId;
                            exchangeDataContract.StoreCode = buisnessPartnerObj.StoreCode;
                            exchangeDataContract.SaleAssociateCode = buisnessPartnerObj.AssociateCode;
                            exchangeDataContract.IsD2C = Convert.ToBoolean(buisnessPartnerObj.IsD2C);
                            exchangeDataContract.IsDtoC = exchangeDataContract.IsD2C;
                            //exchangeDataContract.AssociateEmail = buisnessPartnerObj.Email;
                            exchangeDataContract.AssociateName = buisnessPartnerObj.Name + " " + buisnessPartnerObj.Description;
                        }
                        else
                        {
                            message = "Order not Created";
                        }
                        productOrderResponseDC = _exchangeOrderManager.ManageOrderForD2C(exchangeDataContract);

                        if (productOrderResponseDC != null && productOrderResponseDC.OrderId > 0 && !string.IsNullOrEmpty(productOrderResponseDC.RegdNo))
                            message = "Thank you. Your product details have been received at  Digi2L.Your registration no. " + exchangeDataContract.RegdNo + ". Our quality check team will soon knock at your door.";
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
                LibLogging.WriteErrorToDB("IsDtoC", "D2CRegistration", ex);
            }

            return RedirectToAction("DetailsForD2C");
        }
        #endregion

        #region Details Page For D2C
        public ActionResult DetailsForD2C()
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
                LibLogging.WriteErrorToDB("IsDtoCController", "Details", ex);
            }
            return View();
        }
        #endregion

        #region BrandListForD2C
        [HttpGet]
        public JsonResult GetBrandByProductGroup(int productCatId, int buid,int typeId)
        {
            _masterManager = new BAL.SponsorsApiCall.MasterManager();
            _businessUnitRepository = new BusinessUnitRepository();
            List<SelectListItem> BrandList = null;
            List<SelectListItem> BrandListFinal = null;
            try
            {
                BrandList = _masterManager.GetBrandForExchangeByCategoryId(productCatId, buid, typeId).Brand.Select(prodt => new SelectListItem() { Text = prodt.Name, Value = prodt.Id.ToString() }).ToList();
                tblBusinessUnit businessUnit = _businessUnitRepository.GetSingle(x => x.BusinessUnitId == buid);
                if (businessUnit != null)
                {
                    SelectListItem Bubrand = BrandList.FirstOrDefault(o => o.Text.ToLower().Equals(businessUnit.Name.ToLower()));
                    SelectListItem Otherand = BrandList.FirstOrDefault(o => o.Text.ToLower().Equals("others"));
                    BrandList.RemoveAll(x => x.Text.ToLower().Equals(businessUnit.Name.ToLower()));
                    BrandList.RemoveAll(x => x.Text.ToLower().Equals("others"));
                    BrandList = BrandList.OrderBy(o => o.Value).ToList();
                    BrandListFinal = new List<SelectListItem>();
                    if (Bubrand != null)
                        BrandListFinal.Add(Bubrand);
                    BrandListFinal.AddRange(BrandList);
                    BrandListFinal.Add(Otherand);
                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeController", "GetBrandByProductGroup", ex);
            }
            return Json(BrandListFinal, JsonRequestBehavior.AllowGet);
            //return Json(BrandList, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HttpGet]
        public ActionResult GetHeadersFromrequest(int buid, int bpid)
        {
            AES256encoding aES256Encoding = new AES256encoding();
            var headers = Request.Headers;
            string plainText = string.Empty;
            string thirdParty = headers.GetValues("Third-Party").FirstOrDefault();
            string BelleVie = headers.GetValues("BelleVie-IV").FirstOrDefault();
            string BelivieData = headers.GetValues("BelleVie-Data").FirstOrDefault();
            string ApiKey = "E546C8DF278CD5931069B522E695D4F2";
            if (thirdParty != null && BelleVie != null && BelivieData != null)
            {
                //string DataString = "{\"firstName\":\"Sachin\",\"lastName\":\"Talekar\",\"mobileNo\":\"9664026209\",\"email\":\"sachin.talekar08@gmail.com\"}";


                byte[] IV = Convert.FromBase64String(BelleVie);
                byte[] apikey = Encoding.UTF8.GetBytes(ApiKey);
                byte[] IVa = Encoding.UTF8.GetBytes(BelleVie);
                using (AesManaged aes = new AesManaged())
                {
                    // Encrypt string    
                    //byte[] encrypted = Encrypt(raw, aes.Key, aes.IV);
                    // Print encrypted string    
                    // string basestring = AES256encoding.Encrypt(DataString, ApiKey, BelleVie);

                    // Decrypt the bytes to a string.    
                    string decrypted = AES256encoding.DecryptStringne(BelivieData, ApiKey);

                   // string decrypted = AES256encoding.DecryptStringne(BelivieData, ApiKey);
                    // Print decrypted string. It should be same as raw data
                    // 

                    LodhaGroupHaders jsondata = new LodhaGroupHaders();
                    jsondata = JsonConvert.DeserializeObject<LodhaGroupHaders>(decrypted);
                }

            }
            return View();
        }


        #region Pincode For ABB
        [HttpPost]
        public JsonResult GetPincodeForABB(string pintext)
        {
            _pincodeRepository = new PinCodeRepository();
            IEnumerable<SelectListItem> pincodeList = null;
            List<tblPinCode> pincodeMasterList = null;
            try
            {
                DataTable dt = _pincodeRepository.GetPincodeListForABBD2C();
                if (dt != null && dt.Rows.Count > 0)
                {
                    pincodeMasterList = GenericConversionHelper.DataTableToList<tblPinCode>(dt);
                }
                pincodeList = (pincodeMasterList).AsEnumerable().Select(prodt => new SelectListItem() { Text = prodt.ZipCode.ToString(), Value = prodt.ZipCode.ToString() });
                pincodeList = pincodeList.OrderBy(o => o.Text).ToList();
                pincodeList = pincodeList.Where(x => x.Text.Contains(pintext)).ToList();
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("IsDtoC", "GetPincodeForABB", ex);
            }
            var result = new SelectList(pincodeList, "Value", "Text");
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}