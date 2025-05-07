using GraspCorn.Common.Constant;
using GraspCorn.Common.Enums;
using GraspCorn.Common.Helper;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection.Emit;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Windows.Interop;
using RDCEL.DocUpload.BAL.ABBRegistration;
using RDCEL.DocUpload.BAL.Common;
using RDCEL.DocUpload.BAL.OldProductDetailsManager;
using RDCEL.DocUpload.BAL.ProcessAPI;
using RDCEL.DocUpload.BAL.QRCode;
using RDCEL.DocUpload.BAL.SponsorsApiCall;
using RDCEL.DocUpload.BAL.SweetenerManager;
using RDCEL.DocUpload.BAL.ZohoCreatorCall;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DataContract.ABBRegistration;
using RDCEL.DocUpload.Web.API.Models;
using RDCEL.DocUpload.DataContract;
using RDCEL.DocUpload.DataContract.ABBRegistration;
using RDCEL.DocUpload.DataContract.Bizlog;
using RDCEL.DocUpload.DataContract.BlowHorn;
using RDCEL.DocUpload.DataContract.ExchangeOrderDetails;
using RDCEL.DocUpload.DataContract.MasterModel;
using RDCEL.DocUpload.DataContract.ProductTaxonomy;
using RDCEL.DocUpload.DataContract.SponsorModel;
using RDCEL.DocUpload.DataContract.UniversalPricemasterDetails;
using RDCEL.DocUpload.DataContract.ZohoModel;

namespace RDCEL.DocUpload.Web.API.Controllers
{
    public class ExchangeController : Controller
    {

        BusinessPartnerRepository _businessPartnerRepository;
        BusinessUnitRepository _businessUnitRepository;
        BAL.SponsorsApiCall.ExchangeOrderManager _exchangeOrderManager;
        ExchangeOrderRepository _exchangeOrderRepository;
        ProductCategoryRepository _productCategoryRepository;
        ProductTypeRepository _productTypeRepository;
        NotificationManager _notificationManager;
        LoginDetailsUTCRepository _loginRepository;
        CustomerDetailsRepository _customerDetailsRepository;
        RDCEL.DocUpload.BAL.SponsorsApiCall.MasterManager _masterManager;
        ExchangeOrderStatusRepository _exchangeOrderStatusRepository;
        ProductCategoryMappingRepository _productCategoryMappingRepository;
        PinCodeRepository pinCodeRepository;
        BrandRepository _brandRepository;
        PriceMasterRepository _priceMasterRepository;
        ProductConditionLabelRepository _productConditionLabelRepository;
        OrderBasedConfigurationRepository _OrderBasedConfigurationRepository;
        BPPincodeMappingRepository _bPPincodeMappingRepository;
        BPBUAssociationRepository _bPBUAssociationRepository;
        UninstallationPriceRepository _uninstallationPriceRepository;
        CouponRepository _couponRepository;

        // GET: Exchange
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(string Message)
        {
            string msg = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(Message))
                {
                    msg = Message;
                }
                else
                {
                    msg = "Some error occurred, please connect with the Administrator.";
                }
                ViewBag.MSG = msg;

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeController", "Details", ex);
            }
            return View();
        }

        #region Failed details response
        public ActionResult DetailsFailedResponse(string Message)
        {
            string msg = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(Message))
                {
                    msg = Message;
                }
                else
                {
                    msg = "Some error occurred, please connect with the Administrator.";
                }
                ViewBag.MSG = msg;

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeController", "Details", ex);
            }
            return View();
        }
        #endregion
        #region Select BP and Registration Process

        public ActionResult SelectSociety(int BUId)
        {
            List<tblBusinessPartner> businessPartnerList = null;
            tblBusinessPartner businessPartnerObj = new tblBusinessPartner();
            _businessPartnerRepository = new BusinessPartnerRepository();
            _businessUnitRepository = new BusinessUnitRepository();
            ABBViewModel ABBObj = new ABBViewModel();
            tblBusinessUnit BusinessUnitObj = new tblBusinessUnit();
            try
            {
                ABBObj.CityList = new List<SelectListItem>();
                ABBObj.BUId = BUId;
                BusinessUnitObj = _businessUnitRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == BUId);
                if (BusinessUnitObj != null)
                {
                    if (BusinessUnitObj.LogoName != null)
                    {
                        ABBObj.BULogoName = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Content/DB_Files/SponsorLogo/" + BusinessUnitObj.LogoName;
                    }
                }
                //get state list
                DataTable dt = _businessPartnerRepository.GetStateList();
                if (dt != null && dt.Rows.Count > 0)
                {
                    businessPartnerList = GenericConversionHelper.DataTableToList<tblBusinessPartner>(dt);
                    businessPartnerList = businessPartnerList.Where(x => x.BusinessUnitId == BUId).ToList();
                    ViewBag.StateList = new SelectList(businessPartnerList, "State", "State");
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("Exchange", "SelectSociety", ex);
            }

            return View(ABBObj);
        }

        public ActionResult SelectBP(int BUId, int bpid = 0)
        {
            _masterManager = new BAL.SponsorsApiCall.MasterManager();
            tblBusinessPartner businessPartnerObj = new tblBusinessPartner();
            _businessPartnerRepository = new BusinessPartnerRepository();
            _businessUnitRepository = new BusinessUnitRepository();
            RedirectModel RedirectObj = new RedirectModel();
            tblBusinessUnit BusinessUnitObj = new tblBusinessUnit();
            string message = string.Empty;
            try
            {
                RedirectObj.BUId = BUId;
                RedirectObj.BusinessUnitId = BUId;
                BusinessUnitObj = _businessUnitRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == BUId);
                if (BusinessUnitObj != null)
                {
                    if (BUId == Convert.ToInt32(BusinessUnitEnum.Havells))
                    {
                        return Redirect("https://utcbridge.com/havells.html?buid=0");
                    }
                    if (BusinessUnitObj.LogoName != null)
                    {
                        RedirectObj.BULogoName = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Content/DB_Files/SponsorLogo/" + BusinessUnitObj.LogoName;
                    }
                    RedirectObj.BusinessPartnerId = bpid;
                    RedirectObj.IsBUD2C = Convert.ToBoolean(BusinessUnitObj.IsBUD2C);
                    RedirectObj.IsBUABB = Convert.ToBoolean(BusinessUnitObj.IsABB);
                    RedirectObj.IsBUExchange = Convert.ToBoolean(BusinessUnitObj.IsExchange);
                    RedirectObj.IsSweetnerModelBased = Convert.ToBoolean(BusinessUnitObj.IsSweetnerModelBased);
                }
                else
                {
                    message = "This page is no more active";
                    return RedirectToAction("DetailsFailedResponse", "Exchange", new { Message = message });
                }


            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeController", "SelectBP", ex);
                message = ex.Message;
                return RedirectToAction("DetailsFailedResponse", "Exchange", new { Message = message });
            }
            return View(RedirectObj);
        }

        [HttpPost]
        public ActionResult SelectBP(RedirectModel ExchangeObj)
        {
            string message = string.Empty;
            try
            {
                if (ExchangeObj.IsABB == true)
                {
                    ExchangeObj.IsABB = false;
                    return RedirectToAction("ProductDetails", "ABB", new { BUId = ExchangeObj.BUId, BusinessPartnerId = ExchangeObj.BusinessPartnerId, BusinessUnitId = ExchangeObj.BusinessUnitId });
                }
                else if (ExchangeObj.IsExchange == true)
                {
                    if (ExchangeObj.IsBUD2C == true)
                    {
                        return Redirect("~/IsDtoC/ProductDetailsForD2C?BUId=" + ExchangeObj.BUId + "&&BPID=" + ExchangeObj.BusinessPartnerId);
                    }
                    else if (ExchangeObj.BusinessPartnerId > 0)
                    {
                        return RedirectToAction("ProductDetailsToExchange", "Exchange", new { BUId = ExchangeObj.BUId, oldProductypeId = ExchangeObj, IsSweetnerModelBased = ExchangeObj.IsSweetnerModelBased, BusinessPartnerId = ExchangeObj.BusinessPartnerId, ProductModelIdNew = ExchangeObj.ProductModelIdNew });
                    }
                    else
                    {
                        return RedirectToAction("SelectBusinessPartner", "Exchange", new { BUId = ExchangeObj.BUId, BusinessPartnerId = ExchangeObj.BusinessPartnerId, BusinessUnitId = ExchangeObj.BusinessUnitId, BULogoName = ExchangeObj.BULogoName, IsSweetnerModelBased = ExchangeObj.IsSweetnerModelBased });
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeController", "SelectBP", ex);
                message = ex.Message;
                return RedirectToAction("DetailsFailedResponse", "Exchange", new { Message = message });
            }
            return View(ExchangeObj);
        }

        public ActionResult ExchangeRegistration(ExchagneViewModel ExchangeObj)
        {
            _productCategoryRepository = new ProductCategoryRepository();
            _exchangeOrderManager = new BAL.SponsorsApiCall.ExchangeOrderManager();
            _masterManager = new BAL.SponsorsApiCall.MasterManager();
            _businessPartnerRepository = new BusinessPartnerRepository();
            _businessUnitRepository = new BusinessUnitRepository();
            _exchangeOrderManager = new ExchangeOrderManager();
            List<tblBusinessPartner> businessPartnerList = null;
            ExchangeOrderDataContract exchangeDataContract = new ExchangeOrderDataContract();
            List<SelectListItem> StoreList = null;
            tblBusinessUnit BusinessUnitObj = null;
            try
            {
                //exchangeDataContract.SocietyDataContract = _exchangeOrderManager.GetSocietyById(id);

                //Product Category List
                List<tblProductCategory> prodGroupListForABB = _productCategoryRepository.GetList(x => x.IsActive == true && !x.Description.ToLower().Contains("dishwasher")).ToList();
                prodGroupListForABB = prodGroupListForABB.OrderBy(o => o.Description_For_ABB).ToList();
                if (prodGroupListForABB != null && prodGroupListForABB.Count > 0)
                {
                    ViewBag.ProductCategoryList = new SelectList(prodGroupListForABB, "Id", "Description");
                }
                exchangeDataContract.ProductAge = 2002;
                exchangeDataContract.ProductTypeList = new List<SelectListItem>();
                exchangeDataContract.ProductModelList = new List<SelectListItem>();
                exchangeDataContract.BrandList = new List<SelectListItem>();
                exchangeDataContract.QualityCheckList = new List<SelectListItem> { new SelectListItem { Text = "Excellent", Value = "1" }
                                                                                    , new SelectListItem { Text = "Good", Value = "2" }
                                                                                    , new SelectListItem { Text = "Average", Value = "3" }
                                                                                    , new SelectListItem { Text = "Not Working", Value = "4" }};
                exchangeDataContract.QualityCheckList = exchangeDataContract.QualityCheckList.OrderByDescending(o => o.Value).ToList();
                exchangeDataContract.PurchasedProductCategoryList = new List<SelectListItem> { new SelectListItem { Text = "Refrigerator", Value = "Refrigerator" }
                                                                                        , new SelectListItem { Text = "Washing Machine", Value = "Washing Machine" }
                                                                                        , new SelectListItem { Text = "Television" , Value = "Television" }
                                                                                        , new SelectListItem { Text = "Air Conditioner", Value = "Air Conditioner" }
                                                                                        , new SelectListItem { Text = "Dishwasher" , Value = "Dishwasher" }
                                                                                        , new SelectListItem { Text = "Dryer" , Value = "Dryer" }
                                                                                        , new SelectListItem { Text = "Microwave Oven" , Value = "Microwave Oven" }
                                                                                        , new SelectListItem { Text = "Audio Player" , Value = "Audio Player" }
                                                                                        , new SelectListItem { Text = "Air Purifier" , Value = "Air Purifier" }
                                                                                        , new SelectListItem { Text = "Vacuum Cleaner" , Value = "Vacuum Cleaner" }
                                                                                        , new SelectListItem { Text = "Kitchen Appliance" , Value = "Kitchen Appliance" }
                                                                                        , new SelectListItem { Text = "Cooktop" , Value = "Cooktop" }
                                                                                        , new SelectListItem { Text = "Chimney" , Value = "Chimney" }
                                                                                        , new SelectListItem { Text = "Air Cooler" , Value = "Air Cooler" }
                                                                                        , new SelectListItem { Text = "Gaming Device" , Value = "Gaming Device" }
                                                                                        , new SelectListItem { Text = "Mobile Phones" , Value = "Mobile Phones" }
                                                                                        , new SelectListItem { Text = "Laptops" , Value = "Laptops" }
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
                //exchangeDataContract.BrandList =  new SelectList(_masterManager.GetBrandForExchange().Brand, "Id", "Name");

                if (ExchangeObj.BUId > 0)
                {
                    //store details
                    if (ExchangeObj.CityName != null)
                    {
                        businessPartnerList = _businessPartnerRepository.GetList(x => x.IsActive == true
                            && x.BusinessUnitId == ExchangeObj.BUId
                            && x.City.ToLower().Equals(ExchangeObj.CityName.ToLower())
                            && x.FormatName.ToLower().Equals(ExchangeObj.FormatName.ToLower())).ToList();

                        StoreList = businessPartnerList.Select(x => new SelectListItem
                        {
                            Text = x.Description,
                            Value = x.BusinessPartnerId.ToString()
                        }).ToList();

                        exchangeDataContract.StoreList = StoreList.OrderBy(o => o.Text).ToList();

                        //state city
                        exchangeDataContract.City = ExchangeObj.CityName;
                        exchangeDataContract.State = ExchangeObj.StateName;
                        exchangeDataContract.City1 = ExchangeObj.CityName;
                    }

                    ////BU details
                    BusinessUnitObj = _businessUnitRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == ExchangeObj.BUId);
                    if (BusinessUnitObj != null)
                    {
                        if (BusinessUnitObj.LogoName != null)
                        {
                            exchangeDataContract.BULogoName = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Content/DB_Files/SponsorLogo/" + BusinessUnitObj.LogoName;
                        }

                        exchangeDataContract.CompanyName = BusinessUnitObj.Name;
                        exchangeDataContract.BUName = BusinessUnitObj.Name;
                        exchangeDataContract.BusinessUnitId = ExchangeObj.BUId;
                        exchangeDataContract.ZohoSponsorNumber = BusinessUnitObj.ZohoSponsorId;
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("Exchange", "ExchangeRegistration", ex);
                return View(exchangeDataContract);
            }

            return View(exchangeDataContract);
        }

        [HttpPost]
        public ActionResult ExchangeRegistration(ExchangeOrderDataContract exchangeDataContract)
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
                exchangeDataContract.RegdNo = "E" + UniqueString.RandomNumberByLength(5);
                if (string.IsNullOrEmpty(exchangeDataContract.SponsorOrderNumber))
                    exchangeDataContract.SponsorOrderNumber = "REL" + UniqueString.RandomNumberByLength(9);
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
                        exchangeDataContract.StoreCode = _businessPartnerRepository.GetSingle(x => x.IsActive == true
                            && x.BusinessPartnerId == exchangeDataContract.BusinessPartnerId).StoreCode.Trim();
                        productOrderResponseDC = _exchangeOrderManager.ManageExchangeOrder(exchangeDataContract);

                        if (productOrderResponseDC != null)
                            message = "Thankyou. Your Exchange details have been received at UTC. Our quality check team will connect you soon.";
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
                LibLogging.WriteErrorToDB("Exchange", "ExchangeRegistration", ex);
                return View();
            }

            return RedirectToAction("Details", "Exchange", new { Message = message });
        }

        // Term & Condition
        public ActionResult TermAndCondition()
        {
            return View();
        }
        #endregion

        #region Bosch Exchange Registration
        public ActionResult ExchReg(ExchagneViewModel ExchangeObj)
        {
            _productCategoryRepository = new ProductCategoryRepository();
            _exchangeOrderManager = new BAL.SponsorsApiCall.ExchangeOrderManager();
            _masterManager = new BAL.SponsorsApiCall.MasterManager();
          _businessPartnerRepository = new BusinessPartnerRepository();
            _businessUnitRepository = new BusinessUnitRepository();
            _productCategoryMappingRepository = new ProductCategoryMappingRepository();
            _exchangeOrderManager = new ExchangeOrderManager();
            _productTypeRepository = new ProductTypeRepository();
            _brandRepository = new BrandRepository();
            ModelNumberRepository _modelNumberRepository = new ModelNumberRepository();
            ExchangeOrderDataContract exchangeDataContract = new ExchangeOrderDataContract();
            tblBusinessUnit BusinessUnitObj = null;
            tblBusinessPartner BusinessPartnerObj = null;
            List<tblProductType> produucttypeList = null;
            List<tblProductCategory> productCategoryList = null;
            List<tblBrand> BrandList = null;
            exchangeDataContract.AreaLocalityList = new List<SelectListItem>();
            string message = string.Empty;
            _uninstallationPriceRepository = new UninstallationPriceRepository();
            tblUninstallationPrice uninstallationPriceObj = null;
            try
            {
                productCategoryList = _productCategoryRepository.GetList(x => x.IsActive == true).ToList();
                if (productCategoryList.Count > 0)
                {
                    foreach (var item in productCategoryList)
                    {
                        if (item.Id == ExchangeObj.OldProductCategoryId)
                        {
                            exchangeDataContract.ProductCategory = item.Description;

                        }
                        if (item.Id == ExchangeObj.NewCategoryId)
                        {
                            exchangeDataContract.NewProductCategory = item.Description;
                        }
                    }
                }
                produucttypeList = _productTypeRepository.GetList(x => x.IsActive == true).ToList();
                if (produucttypeList.Count > 0)
                {
                    foreach (var item in produucttypeList)
                    {
                        if (item.Id == ExchangeObj.oldProductypeId)
                        {
                            exchangeDataContract.ProductType = item.Description;
                        }
                        if (item.Id == ExchangeObj.NewTypeId)
                        {
                            exchangeDataContract.NewProductType = item.Description;
                        }
                    }
                }

                BrandList = _brandRepository.GetList(x => x.IsActive == true).ToList();
                if (BrandList.Count > 0)
                {
                    foreach (var item in BrandList)
                    {
                        if (item.Id == ExchangeObj.OldBrandId)
                        {
                            exchangeDataContract.BrandName = item.Name;
                        }
                        if (ExchangeObj.NewBrandId > 0)
                        {
                            if (item.Id == ExchangeObj.NewBrandId)
                            {
                                exchangeDataContract.NewBrandName = item.Name;
                                exchangeDataContract.NewBrandId = ExchangeObj.NewBrandId;
                            }
                        }
                        else if (item.BusinessUnitId == ExchangeObj.BUId)
                        {
                            exchangeDataContract.NewBrandId = item.Id;
                            exchangeDataContract.NewBrandIdDefault = item.Id;
                        }

                    }
                }

                BusinessPartnerObj = _businessPartnerRepository.GetSingle(x => x.BusinessPartnerId == ExchangeObj.BusinessPartnerId);
                if (BusinessPartnerObj != null)
                {
                    exchangeDataContract.AssociateCode = BusinessPartnerObj.AssociateCode;
                    exchangeDataContract.StoreType = BusinessPartnerObj.StoreType;
                    exchangeDataContract.BusinessPartnerId = BusinessPartnerObj.BusinessPartnerId;
                    exchangeDataContract.IsUnInstallation = BusinessPartnerObj.IsUnInstallationRequired;
                    if (BusinessPartnerObj.IsDefaultPickupAddress != null)
                    {
                        exchangeDataContract.IsDefaultPickupAddress = Convert.ToBoolean(BusinessPartnerObj.IsDefaultPickupAddress);
                        if (exchangeDataContract.IsDefaultPickupAddress == true)
                        {
                            exchangeDataContract.Address1 = BusinessPartnerObj.AddressLine1;
                            exchangeDataContract.Address2 = BusinessPartnerObj.AddressLine2;
                            exchangeDataContract.City = BusinessPartnerObj.City;
                            exchangeDataContract.State = BusinessPartnerObj.State;
                            exchangeDataContract.ZipCode = BusinessPartnerObj.Pincode;

                        }
                    }
                    else
                    {
                        exchangeDataContract.IsDefaultPickupAddress = false;
                    }
                    if (BusinessPartnerObj.IsORC != null)
                    {
                        exchangeDataContract.IsOrc = Convert.ToBoolean(BusinessPartnerObj.IsORC);
                    }
                    else
                    {
                        exchangeDataContract.IsOrc = false;
                    }

                    if (BusinessPartnerObj.IsD2C != null)
                    {
                        exchangeDataContract.IsD2C = Convert.ToBoolean(BusinessPartnerObj.IsD2C);
                    }
                    else
                    {
                        exchangeDataContract.IsD2C = false;
                    }
                    exchangeDataContract.StoreCode = BusinessPartnerObj.StoreCode;
                    //Differed settelement 
                    if (BusinessPartnerObj.IsDefferedSettlement == true)
                    {
                        exchangeDataContract.IsDifferedSettlement = Convert.ToBoolean(BusinessPartnerObj.IsDefferedSettlement);
                    }
                    else
                    {
                        exchangeDataContract.IsDifferedSettlement = Convert.ToBoolean(BusinessPartnerObj.IsDefferedSettlement);
                    }

                    exchangeDataContract.VoucherType = Convert.ToInt32(BusinessPartnerObj.VoucherType);
                    exchangeDataContract.IsVoucher = Convert.ToBoolean(BusinessPartnerObj.IsVoucher);
                    exchangeDataContract.voucherCash = Convert.ToInt32(VoucherTypeEnum.Cash);
                    exchangeDataContract.voucherDiscount = Convert.ToInt32(VoucherTypeEnum.Discount);

                }
                if (ExchangeObj.ModelNumberId > 0)
                {
                    tblModelNumber tblModelNumber = new tblModelNumber();
                    tblModelNumber = _modelNumberRepository.GetSingle(x => x.IsActive == true && x.ModelNumberId == ExchangeObj.ModelNumberId);
                    if (tblModelNumber != null)
                    {
                        exchangeDataContract.ModelNumber = tblModelNumber.Code != null ? tblModelNumber.Code : "";
                    }
                }

                uninstallationPriceObj = _uninstallationPriceRepository.GetSingle(x => x.BusinessUnitId == ExchangeObj.BUId && x.BusinessPartnerId == ExchangeObj.BusinessPartnerId && x.ProductCatId == ExchangeObj.OldProductCategoryId && x.ProductTypeId == ExchangeObj.oldProductypeId);
                if(uninstallationPriceObj != null)
                {
                    exchangeDataContract.UnInstallationPrice = uninstallationPriceObj.UnInstallationPrice;
                }

                // product age
                exchangeDataContract.ProductAge = ExchangeObj.ProductAge;
                // Model number Id
                exchangeDataContract.ModelNumberId = ExchangeObj.ModelNumberId;
                // old product category id
                exchangeDataContract.ProductCategoryId = ExchangeObj.OldProductCategoryId;
                // old product type id
                exchangeDataContract.ProductTypeId = ExchangeObj.oldProductypeId;
                // new product category id
                exchangeDataContract.NewProductCategoryId = ExchangeObj.NewCategoryId;
                //new product type id
                exchangeDataContract.NewProductCategoryTypeId = ExchangeObj.NewTypeId;
                //new brand id
                exchangeDataContract.NewBrandId = ExchangeObj.NewBrandId;
                //Formatname
                exchangeDataContract.FormatName = ExchangeObj.FormatName;
                //OldBrandId
                exchangeDataContract.BrandId = ExchangeObj.OldBrandId;
                //ExchangePrice
                exchangeDataContract.ExchangePriceString = ExchangeObj.ExchangePrice;
                exchangeDataContract.BasePrice = ExchangeObj.BasePrice;
                exchangeDataContract.EmployeeId = ExchangeObj.EmployeeId;
                //Quality Check 
                exchangeDataContract.QualityCheck = ExchangeObj.QualityCheckValue;
                exchangeDataContract.ProductTypeList = new List<SelectListItem>();
                exchangeDataContract.ProductModelList = new List<SelectListItem>();
                exchangeDataContract.BrandList = new List<SelectListItem>();
                exchangeDataContract.PincodeList = new List<SelectListItem>();
                exchangeDataContract.businessUnitForHidingModelNumberAndInvoiceData = Convert.ToInt32(BusinessUnitEnum.Diakin);
                exchangeDataContract.BusinessUnitDataContract = _exchangeOrderManager.GetBUById(ExchangeObj.BUId);
                //New Product Model for Relience
                if (ExchangeObj.ProductModelIdNew > 0)
                {
                    tblModelNumber modelnumber = _modelNumberRepository.GetSingle(x => x.IsActive == true && x.ModelNumberId == ExchangeObj.ProductModelIdNew);
                    if (modelnumber != null)
                    {
                        exchangeDataContract.ProductModelIdNew = modelnumber.ModelNumberId;
                        exchangeDataContract.ProductModelNew = modelnumber.ModelName;
                    }
                }
                else
                {
                    exchangeDataContract.ProductModelIdNew = 0;
                    exchangeDataContract.ProductModelNew = "No Model Available";
                }
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
                    ////BU details
                    BusinessUnitObj = _businessUnitRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == ExchangeObj.BUId);
                    if (BusinessUnitObj != null)
                    {
                        if (BusinessUnitObj.IsModelDetailRequired == true)
                        {
                            exchangeDataContract.IsModelNumberRequired = Convert.ToBoolean(BusinessUnitObj.IsModelDetailRequired);
                        }
                        else
                        {
                            exchangeDataContract.IsModelNumberRequired = false;
                        }

                        if (BusinessUnitObj.IsNewBrandRequired == true)
                        {
                            exchangeDataContract.IsNewBrandRequired = Convert.ToBoolean(BusinessUnitObj.IsNewBrandRequired);
                        }
                        else
                        {
                            exchangeDataContract.IsNewBrandRequired = false;
                        }
                        if (BusinessUnitObj.LogoName != null)
                        {
                            exchangeDataContract.BULogoName = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Content/DB_Files/SponsorLogo/" + BusinessUnitObj.LogoName;
                        }
                        if (BusinessUnitObj.IsNewProductDetailsRequired != null)
                        {
                            exchangeDataContract.IsNewProductDetailsReqiured = Convert.ToBoolean(BusinessUnitObj.IsNewProductDetailsRequired);
                        }
                        else
                        {
                            exchangeDataContract.IsNewProductDetailsReqiured = false;
                        }
                        if (BusinessUnitObj.IsInvoiceDetailsRequired != null)
                        {
                            exchangeDataContract.IsInvoiceDetailsReqiured = Convert.ToBoolean(BusinessUnitObj.IsInvoiceDetailsRequired);
                        }
                        else
                        {
                            exchangeDataContract.IsInvoiceDetailsReqiured = false;
                        }
                        exchangeDataContract.IsSweetnerModelBased = (bool)BusinessUnitObj.IsSweetnerModelBased;
                        exchangeDataContract.CompanyName = BusinessUnitObj.Name;
                        exchangeDataContract.BUName = BusinessUnitObj.Name;
                        exchangeDataContract.BusinessUnitId = ExchangeObj.BUId;
                        exchangeDataContract.ZohoSponsorNumber = BusinessUnitObj.ZohoSponsorId;
                        //Added by Vk 


                        exchangeDataContract.IsValidationBasedSweetner = ExchangeObj.IsValidationBasedSweetner;
                        exchangeDataContract.buBasedSweetnerValidationsList = GetBUSweetnerValidationQuestions(BusinessUnitObj.BusinessUnitId);
                        //Added by Vk

                        if(BusinessUnitObj.IsProductSerialNumberRequired == true)
                        {
                            exchangeDataContract.IsProductSerialNumberRequired = Convert.ToBoolean(BusinessUnitObj.IsProductSerialNumberRequired);
                        }
                        else
                        {
                            exchangeDataContract.IsProductSerialNumberRequired = false;
                        }
                    }
                }
                if (exchangeDataContract == null)
                {
                    exchangeDataContract = new ExchangeOrderDataContract();
                }
            }
            catch (Exception ex)
            {
                exchangeDataContract = new ExchangeOrderDataContract();
                LibLogging.WriteErrorToDB("Exchange", "ExchReg", ex);
                message = ex.Message;
                return RedirectToAction("DetailsFailedResponse", "Exchange", new { Message = message });

            }
            tblBusinessPartner tblBusinessPartner = new tblBusinessPartner();
            tblBusinessPartner = _businessPartnerRepository.GetSingle(x => x.IsActive == true && x.BusinessPartnerId == exchangeDataContract.BusinessPartnerId);
            if (tblBusinessPartner != null && tblBusinessPartner.IsDefaultPickupAddress == true)
            {
                exchangeDataContract.IsDefaultPickupAddress = true;
                //return ExchRegPost(ExchangeObj);IsDefaultPickupAddress
                
            }
            exchangeDataContract.IsCouponAplied = ExchangeObj.IsCouponAplied;
            exchangeDataContract.CouponId = ExchangeObj.CouponId;
            exchangeDataContract.CouponValue = ExchangeObj.CouponValue;
            return View(exchangeDataContract);
        }

        [HttpPost]
        public ActionResult ExchReg(ExchangeOrderDataContract exchangeDataContract)
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
                    exchangeDataContract.SponsorOrderNumber = "ECH" + UniqueString.RandomNumberByLength(9);
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
                        if (exchangeDataContract.productBrandID > 0)
                        {
                            exchangeDataContract.BrandId = exchangeDataContract.productBrandID;
                        }
                        if (exchangeDataContract.NewBrandIdDefault > 0)
                        {
                            exchangeDataContract.NewBrandId = exchangeDataContract.NewBrandIdDefault;
                        }
                        //Added for Relience
                        if (exchangeDataContract.ProductModelIdNew > 0 && exchangeDataContract.ModelNumberId == 0)
                        {
                            exchangeDataContract.ModelNumberId = exchangeDataContract.ProductModelIdNew;
                        }
                        //exchangeDataContract.SponsorOrderNumber = "DTC" + UniqueString.RandomNumberByLength(9);
                        exchangeDataContract.ZipCode = exchangeDataContract.PinCode;
                        exchangeDataContract.CompanyName = exchangeDataContract.CompanyName;
                        exchangeDataContract.EstimatedDeliveryDate = DateTime.Now.AddHours(Convert.ToInt32(exchangeDataContract.ExpectedDeliveryHours)).ToString("dd-MM-yyyy");
                        exchangeDataContract.Bonus = "0";
                        productOrderResponseDC = _exchangeOrderManager.ManageExchangeOrder(exchangeDataContract);

                        if (productOrderResponseDC != null && productOrderResponseDC.OrderId > 0 && !string.IsNullOrEmpty(productOrderResponseDC.RegdNo))
                        {
                            if (exchangeDataContract.IsDifferedSettlement == false)
                            {
                                if (exchangeDataContract.FormatName.Equals("Home"))
                                {
                                    message = "Your Exchange details have been received at rockingdeals.Our product registration referance no. " + exchangeDataContract.RegdNo + ". Our quality check team will connect with you soon.";
                                }
                                else
                                {

                                    if (exchangeDataContract.IsOrc == true && exchangeDataContract.IsDifferedSettlement == true)
                                    {
                                        message = "Your Exchange details have been received at rockingdeals. Our product registration referance no. " + exchangeDataContract.RegdNo + ". Our quality check team will connect with you soon.";
                                    }
                                    else
                                    {
                                        message = "Your Exchange details have been received at rockingdeals.Our product registration referance no. " + exchangeDataContract.RegdNo + ". Our quality check team will connect with you soon." +
                                           " Congratulations!!! Please check your voucher detail at registered SMS/Email.";
                                    }
                                }
                            }
                            else
                                message = "Your Exchange details have been received at rockingdeals. Our product registration referance no. " + exchangeDataContract.RegdNo + ". Our quality check team will connect with you soon.";
                        }
                        else
                        {
                            message = "Some error occurred, please connect with the Administrator.";
                            return RedirectToAction("DetailsFailedResponse", "Exchange", new { Message = message });
                        }

                    }
                }
                else
                {
                    message = "Sponser order number can not be provided at this place";
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("Exchange", "ExchReg", ex);
                message = ex.Message;
                return RedirectToAction("DetailsFailedResponse", "Exchange", new { Message = message });
            }

            return RedirectToAction("Details", "Exchange", new { Message = message });
        }


        //for bypass customer detailspage
        [HttpGet]
        public ActionResult ExchRegBusinessPartner(ExchagneViewModel ExchangeObj)
        {
            //POST
            SponsrOrderSyncManager sponsrOrderSyncManager = new SponsrOrderSyncManager();
            ProductOrderResponseDataContract productOrderResponseDC = null;
            _exchangeOrderRepository = new ExchangeOrderRepository();
            _businessPartnerRepository = new BusinessPartnerRepository();
            _exchangeOrderManager = new ExchangeOrderManager();
            tblExchangeOrder SponserObj = null;
            string message = string.Empty;

            ///GET
            _productCategoryRepository = new ProductCategoryRepository();
            //_exchangeOrderManager = new BAL.SponsorsApiCall.ExchangeOrderManager();
            _masterManager = new BAL.SponsorsApiCall.MasterManager();
            //_businessPartnerRepository = new BusinessPartnerRepository();
            _businessUnitRepository = new BusinessUnitRepository();
            _productCategoryMappingRepository = new ProductCategoryMappingRepository();
            
            _productTypeRepository = new ProductTypeRepository();
            _brandRepository = new BrandRepository();
            ModelNumberRepository _modelNumberRepository = new ModelNumberRepository();
            ExchangeOrderDataContract exchangeDataContract = new ExchangeOrderDataContract();
            tblBusinessUnit BusinessUnitObj = null;
            tblBusinessPartner BusinessPartnerObj = null;
            List<tblProductType> produucttypeList = null;
            List<tblProductCategory> productCategoryList = null;
            List<tblBrand> BrandList = null;
            exchangeDataContract.AreaLocalityList = new List<SelectListItem>();
           // string message = string.Empty;
            _uninstallationPriceRepository = new UninstallationPriceRepository();
            tblUninstallationPrice uninstallationPriceObj = null;


            try
            {
                //MSG
                string msg = string.Empty;
                if (!string.IsNullOrEmpty(msg))
                {
                    msg = msg;
                }
                else
                {
                    msg = "Some error occurred, please connect with the Administrator.";
                }
                ViewBag.MSG = msg;
                //
                #region GET DATA
                productCategoryList = _productCategoryRepository.GetList(x => x.IsActive == true).ToList();
                if (productCategoryList.Count > 0)
                {
                    foreach (var item in productCategoryList)
                    {
                        if (item.Id == ExchangeObj.OldProductCategoryId)
                        {
                            exchangeDataContract.ProductCategory = item.Description;

                        }
                        if (item.Id == ExchangeObj.NewCategoryId)
                        {
                            exchangeDataContract.NewProductCategory = item.Description;
                        }
                    }
                }

                produucttypeList = _productTypeRepository.GetList(x => x.IsActive == true).ToList();
                if (produucttypeList.Count > 0)
                {
                    foreach (var item in produucttypeList)
                    {
                        if (item.Id == ExchangeObj.oldProductypeId)
                        {
                            exchangeDataContract.ProductType = item.Description;
                        }
                        if (item.Id == ExchangeObj.NewTypeId)
                        {
                            exchangeDataContract.NewProductType = item.Description;
                        }
                    }
                }

                BrandList = _brandRepository.GetList(x => x.IsActive == true).ToList();
                if (BrandList.Count > 0)
                {
                    foreach (var item in BrandList)
                    {
                        if (item.Id == ExchangeObj.OldBrandId)
                        {
                            exchangeDataContract.BrandName = item.Name;
                        }
                        if (ExchangeObj.NewBrandId > 0)
                        {
                            if (item.Id == ExchangeObj.NewBrandId)
                            {
                                exchangeDataContract.NewBrandName = item.Name;
                                exchangeDataContract.NewBrandId = ExchangeObj.NewBrandId;
                            }
                        }
                        else if (item.BusinessUnitId == ExchangeObj.BUId)
                        {
                            exchangeDataContract.NewBrandId = item.Id;
                            exchangeDataContract.NewBrandIdDefault = item.Id;
                        }

                    }
                }

                BusinessPartnerObj = _businessPartnerRepository.GetSingle(x => x.BusinessPartnerId == ExchangeObj.BusinessPartnerId);
                if (BusinessPartnerObj != null)
                {
                    exchangeDataContract.AssociateCode = BusinessPartnerObj.AssociateCode;
                    exchangeDataContract.StoreType = BusinessPartnerObj.StoreType;
                    exchangeDataContract.BusinessPartnerId = BusinessPartnerObj.BusinessPartnerId;
                    exchangeDataContract.IsUnInstallation = BusinessPartnerObj.IsUnInstallationRequired;
                    if (BusinessPartnerObj.IsORC != null)
                    {
                        exchangeDataContract.IsOrc = Convert.ToBoolean(BusinessPartnerObj.IsORC);
                    }
                    else
                    {
                        exchangeDataContract.IsOrc = false;
                    }

                    if (BusinessPartnerObj.IsD2C != null)
                    {
                        exchangeDataContract.IsD2C = Convert.ToBoolean(BusinessPartnerObj.IsD2C);
                    }
                    else
                    {
                        exchangeDataContract.IsD2C = false;
                    }
                    exchangeDataContract.StoreCode = BusinessPartnerObj.StoreCode;
                    //Differed settelement 
                    if (BusinessPartnerObj.IsDefferedSettlement == true)
                    {
                        exchangeDataContract.IsDifferedSettlement = Convert.ToBoolean(BusinessPartnerObj.IsDefferedSettlement);
                    }
                    else
                    {
                        exchangeDataContract.IsDifferedSettlement = Convert.ToBoolean(BusinessPartnerObj.IsDefferedSettlement);
                    }

                    exchangeDataContract.VoucherType = Convert.ToInt32(BusinessPartnerObj.VoucherType);
                    exchangeDataContract.IsVoucher = Convert.ToBoolean(BusinessPartnerObj.IsVoucher);
                    exchangeDataContract.voucherCash = Convert.ToInt32(VoucherTypeEnum.Cash);
                    exchangeDataContract.voucherDiscount = Convert.ToInt32(VoucherTypeEnum.Discount);

                }
                if (ExchangeObj.BUId == Convert.ToInt32(BusinessUnitEnum.Lg))
                {
                    exchangeDataContract.IsCustomerEmailRequired = false;
                    exchangeDataContract.IsCustomerAcceptenceRequired = true;
                }
                if (ExchangeObj.ModelNumberId > 0)
                {
                    tblModelNumber tblModelNumber = new tblModelNumber();
                    tblModelNumber = _modelNumberRepository.GetSingle(x => x.IsActive == true && x.ModelNumberId == ExchangeObj.ModelNumberId);
                    if (tblModelNumber != null)
                    {
                        exchangeDataContract.ModelNumber = tblModelNumber.Code != null ? tblModelNumber.Code : "";
                    }
                }

                uninstallationPriceObj = _uninstallationPriceRepository.GetSingle(x => x.BusinessUnitId == ExchangeObj.BUId && x.BusinessPartnerId == ExchangeObj.BusinessPartnerId && x.ProductCatId == ExchangeObj.OldProductCategoryId && x.ProductTypeId == ExchangeObj.oldProductypeId);
                if (uninstallationPriceObj != null)
                {
                    exchangeDataContract.UnInstallationPrice = uninstallationPriceObj.UnInstallationPrice;
                }

                // product age
                exchangeDataContract.ProductAge = ExchangeObj.ProductAge;
                // Model number Id
                exchangeDataContract.ModelNumberId = ExchangeObj.ModelNumberId;
                // old product category id
                exchangeDataContract.ProductCategoryId = ExchangeObj.OldProductCategoryId;
                // old product type id
                exchangeDataContract.ProductTypeId = ExchangeObj.oldProductypeId;
                // new product category id
                exchangeDataContract.NewProductCategoryId = ExchangeObj.NewCategoryId;
                //new product type id
                exchangeDataContract.NewProductCategoryTypeId = ExchangeObj.NewTypeId;
                //new brand id
                exchangeDataContract.NewBrandId = ExchangeObj.NewBrandId;
                //Formatname
                exchangeDataContract.FormatName = ExchangeObj.FormatName;
                //OldBrandId
                exchangeDataContract.BrandId = ExchangeObj.OldBrandId;
                //ExchangePrice
                exchangeDataContract.ExchangePriceString = ExchangeObj.ExchangePrice;
                exchangeDataContract.BasePrice = ExchangeObj.BasePrice;
                exchangeDataContract.EmployeeId = ExchangeObj.EmployeeId;
                //Quality Check 
                exchangeDataContract.QualityCheck = ExchangeObj.QualityCheckValue;
                exchangeDataContract.ProductTypeList = new List<SelectListItem>();
                exchangeDataContract.ProductModelList = new List<SelectListItem>();
                exchangeDataContract.BrandList = new List<SelectListItem>();
                exchangeDataContract.PincodeList = new List<SelectListItem>();
                exchangeDataContract.businessUnitForHidingModelNumberAndInvoiceData = Convert.ToInt32(BusinessUnitEnum.Diakin);
                exchangeDataContract.BusinessUnitDataContract = _exchangeOrderManager.GetBUById(ExchangeObj.BUId);
                //New Product Model for Relience
                if (ExchangeObj.ProductModelIdNew > 0)
                {
                    tblModelNumber modelnumber = _modelNumberRepository.GetSingle(x => x.IsActive == true && x.ModelNumberId == ExchangeObj.ProductModelIdNew);
                    if (modelnumber != null)
                    {
                        exchangeDataContract.ProductModelIdNew = modelnumber.ModelNumberId;
                        exchangeDataContract.ProductModelNew = modelnumber.ModelName;
                    }
                }
                else
                {
                    exchangeDataContract.ProductModelIdNew = 0;
                    exchangeDataContract.ProductModelNew = "No Model Available";
                }
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
                    ////BU details
                    BusinessUnitObj = _businessUnitRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == ExchangeObj.BUId);
                    if (BusinessUnitObj != null)
                    {
                        if (BusinessUnitObj.IsModelDetailRequired == true)
                        {
                            exchangeDataContract.IsModelNumberRequired = Convert.ToBoolean(BusinessUnitObj.IsModelDetailRequired);
                        }
                        else
                        {
                            exchangeDataContract.IsModelNumberRequired = false;
                        }

                        if (BusinessUnitObj.IsNewBrandRequired == true)
                        {
                            exchangeDataContract.IsNewBrandRequired = Convert.ToBoolean(BusinessUnitObj.IsNewBrandRequired);
                        }
                        else
                        {
                            exchangeDataContract.IsNewBrandRequired = false;
                        }
                        if (BusinessUnitObj.LogoName != null)
                        {
                            exchangeDataContract.BULogoName = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Content/DB_Files/SponsorLogo/" + BusinessUnitObj.LogoName;
                        }
                        if (BusinessUnitObj.IsNewProductDetailsRequired != null)
                        {
                            exchangeDataContract.IsNewProductDetailsReqiured = Convert.ToBoolean(BusinessUnitObj.IsNewProductDetailsRequired);
                        }
                        else
                        {
                            exchangeDataContract.IsNewProductDetailsReqiured = false;
                        }
                        if (BusinessUnitObj.IsInvoiceDetailsRequired != null)
                        {
                            exchangeDataContract.IsInvoiceDetailsReqiured = Convert.ToBoolean(BusinessUnitObj.IsInvoiceDetailsRequired);
                        }
                        else
                        {
                            exchangeDataContract.IsInvoiceDetailsReqiured = false;
                        }
                        exchangeDataContract.IsSweetnerModelBased = (bool)BusinessUnitObj.IsSweetnerModelBased;
                        exchangeDataContract.CompanyName = BusinessUnitObj.Name;
                        exchangeDataContract.BUName = BusinessUnitObj.Name;
                        exchangeDataContract.BusinessUnitId = ExchangeObj.BUId;
                        exchangeDataContract.ZohoSponsorNumber = BusinessUnitObj.ZohoSponsorId;
                        //Added by Vk 


                        exchangeDataContract.IsValidationBasedSweetner = ExchangeObj.IsValidationBasedSweetner;
                        exchangeDataContract.buBasedSweetnerValidationsList = GetBUSweetnerValidationQuestions(BusinessUnitObj.BusinessUnitId);
                        //Added by Vk

                        if (BusinessUnitObj.IsProductSerialNumberRequired == true)
                        {
                            exchangeDataContract.IsProductSerialNumberRequired = Convert.ToBoolean(BusinessUnitObj.IsProductSerialNumberRequired);
                        }
                        else
                        {
                            exchangeDataContract.IsProductSerialNumberRequired = false;
                        }
                    }
                }
                if (exchangeDataContract == null)
                {
                    exchangeDataContract = new ExchangeOrderDataContract();
                }
                #endregion


                //POST
                #region pOST 
                exchangeDataContract.RegdNo = "E" + UniqueString.RandomNumberByLength(7);

                if (string.IsNullOrEmpty(exchangeDataContract.SponsorOrderNumber))
                    exchangeDataContract.SponsorOrderNumber = "ECH" + UniqueString.RandomNumberByLength(9);
                else
                    exchangeDataContract.SponsorOrderNumber = exchangeDataContract.SponsorOrderNumber.Trim() + exchangeDataContract.RegdNo;
                string sponsorOrderNo1 = exchangeDataContract.SponsorOrderNumber;
                if (String.IsNullOrEmpty(sponsorOrderNo1) == false)
                {
                    SponserObj = _exchangeOrderRepository.GetSingle(x => !string.IsNullOrEmpty(x.SponsorOrderNumber) && x.SponsorOrderNumber.ToLower().Equals(sponsorOrderNo1.ToLower()));
                    if (SponserObj != null)
                    {
                        message = "This Exchange Order Number : " + exchangeDataContract.SponsorOrderNumber + " is already exist";
                    }
                    else
                    {
                        if (exchangeDataContract.productBrandID > 0)
                        {
                            exchangeDataContract.BrandId = exchangeDataContract.productBrandID;
                        }
                        if (exchangeDataContract.NewBrandIdDefault > 0)
                        {
                            exchangeDataContract.NewBrandId = exchangeDataContract.NewBrandIdDefault;
                        }
                        //Added for Relience
                        if (exchangeDataContract.ProductModelIdNew > 0 && exchangeDataContract.ModelNumberId == 0)
                        {
                            exchangeDataContract.ModelNumberId = exchangeDataContract.ProductModelIdNew;
                        }
                        //exchangeDataContract.SponsorOrderNumber = "DTC" + UniqueString.RandomNumberByLength(9);
                        exchangeDataContract.ZipCode = exchangeDataContract.PinCode;
                        exchangeDataContract.CompanyName = exchangeDataContract.CompanyName;
                        exchangeDataContract.EstimatedDeliveryDate = DateTime.Now.AddHours(Convert.ToInt32(exchangeDataContract.ExpectedDeliveryHours)).ToString("dd-MM-yyyy");
                        exchangeDataContract.Bonus = "0";
                        productOrderResponseDC = _exchangeOrderManager.ManageExchangeOrder(exchangeDataContract);

                        if (productOrderResponseDC != null && productOrderResponseDC.OrderId > 0 && !string.IsNullOrEmpty(productOrderResponseDC.RegdNo))
                        {
                            if (exchangeDataContract.IsDifferedSettlement == false)
                            {
                                if (exchangeDataContract.FormatName.Equals("Home"))
                                {
                                    message = "Your Exchange details have been received at rockingdeals.Our product registration referance no. " + exchangeDataContract.RegdNo + ". Our quality check team will connect with you soon.";
                                }
                                else
                                {

                                    if (exchangeDataContract.IsOrc == true && exchangeDataContract.IsDifferedSettlement == true)
                                    {
                                        message = "Your Exchange details have been received at rockingdeals. Our product registration referance no. " + exchangeDataContract.RegdNo + ". Our quality check team will connect with you soon.";
                                    }
                                    else
                                    {
                                        message = "Your Exchange details have been received at rockingdeals.Our product registration referance no. " + exchangeDataContract.RegdNo + ". Our quality check team will connect with you soon." +
                                           " Congratulations!!! Please check your voucher detail at registered SMS/Email.";
                                    }
                                }
                            }
                            else
                                message = "Your Exchange details have been received at rockingdeals. Our product registration referance no. " + exchangeDataContract.RegdNo + ". Our quality check team will connect with you soon.";
                        }
                        else
                        {
                            message = "Some error occurred, please connect with the Administrator.";
                            return RedirectToAction("DetailsFailedResponse", "Exchange", new { Message = message });
                        }

                    }
                }
                else
                {
                    message = "Sponser order number can not be provided at this place";
                }

                #endregion


            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("Exchange", "ExchReg", ex);
                message = ex.Message;
                return RedirectToAction("DetailsFailedResponse", "Exchange", new { Message = message });
            }

            return RedirectToAction("Details", "Exchange", new { Message = message });
        }

        #endregion

        #region Validate Delivery

        [HttpPost]
        public ActionResult ConfirmDeliveryBP(ExchangeorderConfirmation ExchangeObj)
        {
            _exchangeOrderManager = new ExchangeOrderManager();
            ProductOrderStatusDataContract productOrderStatusDataContract = null;
            bool flag = false;
            string message = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    if (ExchangeObj.IsDelivered == true)
                    {
                        productOrderStatusDataContract.OrderId = ExchangeObj.ExchangeOrderid;
                        productOrderStatusDataContract.Status = SponsorDeliveryStatusConstant.Delivered;
                        flag = _exchangeOrderManager.UpdateDeliveryStatusAndPickupDate(productOrderStatusDataContract);
                    }

                    if (flag)
                        message = "Thanks for sharing the update, will revert you soon.";
                    else
                        message = "Thanks for sharing the update.";

                    if (!string.IsNullOrEmpty(message))
                        TempData["Msg"] = message;
                    else
                        TempData["Msg"] = "Some error occurred, please connect with the Administrator.";

                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeController", "SelectBP", ex);
            }

            return RedirectToAction("Details", "Exchange", new { Message = message });
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

                    string message = NotificationConstants.SMS_ExchangeOtp.Replace("[OTP]", OTPValue).Replace("[BrandName]", businessUnit.Name);
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
        public JsonResult GetORCStoreByCityStatePin(string stateName, string cityName, string pintext, int buid)
        {
            _businessPartnerRepository = new BusinessPartnerRepository();
            List<SelectListItem> formateList = new List<SelectListItem>();
            try
            {
                formateList.Insert(0, new SelectListItem() { Value = "Dealer", Text = "At Dealer Store" });

                DataTable dt = _businessPartnerRepository.GetPincodeListForBU(stateName, cityName, buid);
                List<tblBusinessPartner> businessPartners = _businessPartnerRepository.GetList(x => x.IsActive == true && x.IsExchangeBP == true
               && (x.BusinessUnitId != null && x.BusinessUnitId == buid)
               && (x.IsORC != null && x.IsORC == true)
               && (x.State.ToLower().Equals(stateName.ToLower()))
               && (x.City.ToLower().Equals(cityName.ToLower()))
               && (x.Pincode.ToLower().Equals(pintext.ToLower()))
                ).ToList();
                if (businessPartners != null && businessPartners.Count > 0)
                {
                    foreach (var bpList in businessPartners)
                    {
                        formateList = new List<SelectListItem> { new SelectListItem() { Value = bpList.BusinessPartnerId.ToString(), Text = bpList.Description + " " + bpList.AddressLine1 } };
                    }
                    if (businessPartners.Exists(x => x.AssociateCode.Equals("6784001468"))) //Reliance Digital
                    {
                        formateList.Insert(0, new SelectListItem() { Value = "6784001468", Text = "At Reliance Digital" });
                    }
                    if (businessPartners.Exists(x => x.AssociateCode.Equals("6784001512"))) //Croma
                    {
                        formateList.Insert(0, new SelectListItem() { Value = "6784001512", Text = "At Croma" });
                    }
                    if (businessPartners.Exists(x => x.AssociateCode.Equals("6782007009"))) //Vijay Sales
                    {
                        formateList.Insert(0, new SelectListItem() { Value = "6782007009", Text = "At Vijay Sales" });
                    }

                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("BoschExchangeController", "GetORCByCityStatePin", ex);
            }
            var result = new SelectList(formateList, "Value", "Text");
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetCityByStateName(string stateName, int buid)
        {
            _businessPartnerRepository = new BusinessPartnerRepository();
            IEnumerable<SelectListItem> cityList = null;
            List<tblBusinessPartner> businessPartnerList = null;
            try
            {
                DataTable dt = _businessPartnerRepository.GetCityListbyBU(stateName, buid);
                if (dt != null && dt.Rows.Count > 0)
                {
                    businessPartnerList = GenericConversionHelper.DataTableToList<tblBusinessPartner>(dt);
                }
                cityList = (businessPartnerList).AsEnumerable().Select(prodt => new SelectListItem() { Text = prodt.City, Value = prodt.City });
                cityList = cityList.OrderBy(o => o.Text).ToList();
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeController", "GetCityByStateName", ex);
            }
            var result = new SelectList(cityList, "Value", "Text");
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPincode(string stateName, string cityName, string pintext, int buid)
        {
            _businessPartnerRepository = new BusinessPartnerRepository();
            IEnumerable<SelectListItem> pincodeList = null;
            List<tblBusinessPartner> businessPartnerList = null;
            try
            {
                DataTable dt = _businessPartnerRepository.GetPincodeListForBU(stateName, cityName, buid);
                if (dt != null && dt.Rows.Count > 0)
                {
                    businessPartnerList = GenericConversionHelper.DataTableToList<tblBusinessPartner>(dt);
                }
                pincodeList = (businessPartnerList).AsEnumerable().Select(prodt => new SelectListItem() { Text = prodt.Pincode, Value = prodt.Pincode });
                pincodeList = pincodeList.OrderBy(o => o.Text).ToList();
                // businessPartnerList = businessPartnerList.Where(x => x.Pincode.Contains(pintext)).ToList();
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("BoschExchangeController", "GetPincodeByStateName", ex);
            }
            var result = new SelectList(pincodeList, "Value", "Text");
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetPincodeByStateName(string stateName, string cityName, string pintext, int buid)
        {
            _businessPartnerRepository = new BusinessPartnerRepository();
            IEnumerable<SelectListItem> pincodeList = null;
            List<tblBusinessPartner> businessPartnerList = null;
            try
            {
                DataTable dt = _businessPartnerRepository.GetPincodeListForBU(stateName, cityName, buid);
                if (dt != null && dt.Rows.Count > 0)
                {
                    businessPartnerList = GenericConversionHelper.DataTableToList<tblBusinessPartner>(dt);
                }
                pincodeList = (businessPartnerList).AsEnumerable().Select(prodt => new SelectListItem() { Text = prodt.Pincode, Value = prodt.Pincode });
                pincodeList = pincodeList.OrderBy(o => o.Text).ToList();
                pincodeList = pincodeList.Where(x => x.Text.Contains(pintext)).ToList();
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("BoschExchangeController", "GetPincodeByStateName", ex);
            }
            var result = new SelectList(pincodeList, "Value", "Text");
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetBrandByProductGroup(int productCatId, int buid, int typeId)
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

        // GET: Category type by Category Id
        [HttpGet]
        public JsonResult GetProdTypeByProdGroupId(int productCatId)
        {
            _productTypeRepository = new ProductTypeRepository();
            List<SelectListItem> prodType = null;
            SelectList result = null;
            try
            {
                List<tblProductType> prodTypeListForExc = _productTypeRepository.GetList(x => x.IsActive == true && x.ProductCatId == productCatId).ToList();
                if (prodTypeListForExc != null && prodTypeListForExc.Count > 0)
                {

                    //    prodTypeListForABB = prodTypeListForABB.Where(x => x.ProductCatId == productCatId).ToList();
                    prodType = new List<SelectListItem>();
                    foreach (tblProductType prodTypeObj in prodTypeListForExc)
                    {
                        if (!string.IsNullOrEmpty(prodTypeObj.Size))
                        {
                            prodType.Add(new SelectListItem() { Text = prodTypeObj.Description + "(" + prodTypeObj.Size + ")", Value = prodTypeObj.Id.ToString() });
                        }
                        else
                        {
                            prodType.Add(new SelectListItem() { Text = prodTypeObj.Description, Value = prodTypeObj.Id.ToString() });
                        }
                    }
                }
                if (prodType != null && prodType.Count > 0)
                    prodType = prodType.OrderBy(o => o.Text).ToList();
                result = new SelectList(prodType, "Value", "Text");
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeController", "GetProdTypeByProdGroupId", ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetProdqualityIndexDetailbyCategotyId(int productCatId)
        {
            _masterManager = new BAL.SponsorsApiCall.MasterManager();
            ProductQualityIndexDataContract productQualityIndexDC = null;
            try
            {
                productQualityIndexDC = _masterManager.GetProductQualityIndexByCategory(productCatId);
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeController", "GetProdTypeByProdGroupId", ex);
            }

            return Json(productQualityIndexDC, JsonRequestBehavior.AllowGet);
        }

        #region GetProdPrice Added by Vishal.C
        /*[HttpGet]
        public JsonResult GetProdPrice(int productCatId, int productSubCatId, int brandId, int conditionId, int buid, bool IsSweetnerModelBased, int newcatid = 0, int newsubcatid = 0, int modelno = 0, string formatType = "")
        {
            _masterManager = new BAL.SponsorsApiCall.MasterManager();
            string price = null;
            try
            {
                if (IsSweetnerModelBased == true)
                {
                    if (newcatid > 0 && newsubcatid > 0 && modelno > 0)
                    {
                        price = _masterManager.GetProductPriceWithModelBasedSweetner(newcatid, newsubcatid, productCatId, productSubCatId, brandId, conditionId, buid, modelno, formatType);
                    }
                    else if (newcatid > 0 && newsubcatid > 0)
                    {
                        price = _masterManager.GetProductPriceWithModelBasedSweetner(newcatid, newsubcatid, productCatId, productSubCatId, brandId, conditionId, buid, modelno, formatType);
                    }
                }
                else
                {
                    price = _masterManager.GetProductPrice(productCatId, productSubCatId, brandId, conditionId, buid, formatType);
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeController", "GetProdPrice", ex);
            }
            return Json(price, JsonRequestBehavior.AllowGet);
        }
        */
        #endregion

        public JsonResult GetIsOrcAndIsDefferedSettelmentByBPId(int businessPartnerId)
        {
            _exchangeOrderManager = new ExchangeOrderManager();
            bool result = false;
            try
            {
                result = _exchangeOrderManager.VerifyIsOrcByBPId(businessPartnerId);
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeController", "GetIsOrcAndIsDefferedSettelmentByBPId", ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region Get Porduct Type for New Product

        public JsonResult GetNewProdTypeByNewProdGroupId(int catId)
        {
            _productTypeRepository = new ProductTypeRepository();
            IEnumerable<SelectListItem> prodType = null;
            try
            {
                List<tblProductType> prodTypeListForExchange = _productTypeRepository.GetList(x => x.IsActive == true).ToList();
                if (prodTypeListForExchange != null && prodTypeListForExchange.Count > 0)
                {
                    prodTypeListForExchange = prodTypeListForExchange.Where(x => x.ProductCatId == catId).ToList();
                    prodTypeListForExchange.RemoveAll(x => !string.IsNullOrEmpty(x.Size)); //ADDED LINE TO REMOVE REF SIZE TYPES
                }
                prodType = (prodTypeListForExchange).AsEnumerable().Select(prodt => new SelectListItem() { Text = prodt.Description_For_ABB, Value = prodt.Id.ToString() });
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeController", "GetProdTypeByProdGroupId", ex);
                //LibLogging.WriteErrorLog("UserManager", "VerifyOTP", ex);
            }
            var result = new SelectList(prodType, "Value", "Text");
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetModelNumberByProdTypeId(int ProdTypeId, int buid, int newcatid)
        {
            ModelNumberRepository _modelNumberRepository = new ModelNumberRepository();
            _productCategoryMappingRepository = new ProductCategoryMappingRepository();
            List<tblModelNumber> modelNoDC = null;
            List<SelectListItem> modelNumber = new List<SelectListItem>();
            tblBUProductCategoryMapping mappingObj = _productCategoryMappingRepository.GetSingle(x => x.BusinessUnitId == buid && x.ProductCatId == newcatid);
            if (mappingObj != null)
            {
                if (ProdTypeId > 0)
                    modelNoDC = _modelNumberRepository.GetList(x => x.ProductTypeId == ProdTypeId && x.BusinessUnitId == mappingObj.BusinessUnitId && x.IsDefaultProduct == false && x.IsActive == true).ToList();
                if (modelNoDC != null && modelNoDC.Count > 0)
                    modelNumber = modelNoDC.Select(projt => new SelectListItem() { Text = projt.ModelName, Value = projt.ModelNumberId.ToString() }).ToList();
                else
                {
                    modelNumber.Add(new SelectListItem { Text = "No Model Available", Value = "0" });
                }

            }
            else
            {
                modelNumber.Add(new SelectListItem { Text = "No Model Available", Value = "0" });
            }
            var result = new SelectList(modelNumber, "Value", "Text");

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region Sponsor Dashboard
        public ActionResult Dashboard(int buid)
        {
            string msg = string.Empty;
            _exchangeOrderManager = new ExchangeOrderManager();
            _exchangeOrderRepository = new ExchangeOrderRepository();
            _businessPartnerRepository = new BusinessPartnerRepository();
            _businessUnitRepository = new BusinessUnitRepository();
            _exchangeOrderStatusRepository = new ExchangeOrderStatusRepository();
            List<tblBusinessUnit> tblBusinessUnitList = new List<tblBusinessUnit>();
            List<tblExchangeOrder> exchangeOrdersList = new List<tblExchangeOrder>();
            List<tblExchangeOrderStatu> tblExchangeOrderStatus = new List<tblExchangeOrderStatu>();
            ExchangeDashBoardViewModel ExchangeObj = new ExchangeDashBoardViewModel();
            tblBusinessUnit BusinessUnitObj = new tblBusinessUnit();
            List<SelectListItem> exchangeOrderStatus = null;



            int totalOrdercount = 0;
            int buTotalorderCount = 0;
            int defferedOrderCount = 0;
            int dToCOrderCount = 0;
            int oRCOrtderCount = 0;
            int todayTotalOrderCount = 0;
            int totalVoucherRedeemed = 0;
            int todaybuTotalorderCount = 0;
            int todaydefferedOrderCount = 0;
            int todaydToCOrderCount = 0;
            int todayoRCOrtderCount = 0;
            int todaytotalVoucherRedeemed = 0;

            tblExchangeOrderStatus = _exchangeOrderStatusRepository.GetList(x => x.IsActive == true).ToList();
            exchangeOrderStatus = tblExchangeOrderStatus.Select(x => new SelectListItem
            {
                Text = x.StatusName + "(" + x.StatusCode + ")",
                Value = x.Id.ToString()
            }).ToList();
            ViewBag.Status = new SelectList(exchangeOrderStatus, "Value", "Text");
            tblBusinessUnitList = _businessUnitRepository.GetList(x => x.IsActive == true).ToList();
            ExchangeObj.BusinessPartenerList = tblBusinessUnitList;
            BusinessUnitObj = _businessUnitRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == buid);
            if (BusinessUnitObj != null)
            {
                if (BusinessUnitObj.LogoName != null)
                {
                    ExchangeObj.BULogoName = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Content/DB_Files/SponsorLogo/" + BusinessUnitObj.LogoName;
                    ViewBag.ImagePath = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Content/DB_Files/SponsorLogo/";
                    ExchangeObj.BuName = BusinessUnitObj.Name;
                    ViewBag.BuName = BusinessUnitObj.Name;
                }
                DataSet ds = _exchangeOrderRepository.GetOrderSummaryForBU(buid, BusinessUnitObj.Name);
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                {
                    totalOrdercount = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    buTotalorderCount = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
                    defferedOrderCount = Convert.ToInt32(ds.Tables[2].Rows[0][0]);
                    dToCOrderCount = Convert.ToInt32(ds.Tables[3].Rows[0][0]);
                    oRCOrtderCount = Convert.ToInt32(ds.Tables[4].Rows[0][0]);
                    totalVoucherRedeemed = Convert.ToInt32(ds.Tables[5].Rows[0][0]);

                    todayTotalOrderCount = Convert.ToInt32(ds.Tables[6].Rows[0][0]);
                    todaybuTotalorderCount = Convert.ToInt32(ds.Tables[7].Rows[0][0]);
                    todaydefferedOrderCount = Convert.ToInt32(ds.Tables[8].Rows[0][0]);
                    todaydToCOrderCount = Convert.ToInt32(ds.Tables[9].Rows[0][0]);
                    todayoRCOrtderCount = Convert.ToInt32(ds.Tables[10].Rows[0][0]);
                    todaytotalVoucherRedeemed = Convert.ToInt32(ds.Tables[11].Rows[0][0]);
                }
            }

            ViewBag.totalOrdercount = totalOrdercount;
            ViewBag.buTotalorderCount = buTotalorderCount;
            ViewBag.defferedOrderCount = defferedOrderCount;
            ViewBag.dToCOrderCount = dToCOrderCount;
            ViewBag.oRCOrtderCount = oRCOrtderCount;
            ViewBag.totalVoucherRedeemed = totalVoucherRedeemed;

            ViewBag.todayTotalOrderCount = todayTotalOrderCount;
            ViewBag.todaybuTotalorderCount = todaybuTotalorderCount;
            ViewBag.todaydefferedOrderCount = todaydefferedOrderCount;
            ViewBag.todaydToCOrderCount = todaydToCOrderCount;
            ViewBag.todayoRCOrtderCount = todayoRCOrtderCount;
            ViewBag.todaytotalVoucherRedeemed = todaytotalVoucherRedeemed;

            return View(ExchangeObj);


        }
        #endregion

        #region method to get order count using date filter
        [HttpGet]
        public JsonResult GetOrderCountByDate(string date1, string date2, string companyName, int? statusCode)
        {
            _exchangeOrderRepository = new ExchangeOrderRepository();
            int totalOrdercount = 0;
            int buTotalorderCount = 0;
            int defferedOrderCount = 0;
            int dToCOrderCount = 0;
            int oRCOrtderCount = 0;
            int totalVoucherRedeemed = 0;
            var result = string.Empty;
            try
            {
                DataSet ds = _exchangeOrderRepository.GetOrderSummaryForBUWithDateRange(date1, date2, companyName, statusCode);
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                {
                    totalOrdercount = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    buTotalorderCount = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
                    defferedOrderCount = Convert.ToInt32(ds.Tables[2].Rows[0][0]);
                    dToCOrderCount = Convert.ToInt32(ds.Tables[3].Rows[0][0]);
                    oRCOrtderCount = Convert.ToInt32(ds.Tables[4].Rows[0][0]);
                    totalVoucherRedeemed = Convert.ToInt32(ds.Tables[5].Rows[0][0]);
                }

                int[] arr = new int[6] { totalOrdercount, buTotalorderCount, defferedOrderCount, dToCOrderCount, oRCOrtderCount, totalVoucherRedeemed };
                result = string.Join(",", arr);
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeController", "GetOrderCountByDate", ex);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region to get all the order count by status code
        public ActionResult StatusDashboard()
        {
            string msg = string.Empty;
            _exchangeOrderRepository = new ExchangeOrderRepository();
            int pickupByDealer = 0;
            int duplicateOrder = 0;
            int customerAddressDifferentFromRegisteredAddress = 0;
            int inTransit = 0;
            int orderCreatedBySponsor = 0;
            int exchangeOrderCancelledAfterCreation = 0;
            int customerCommunicatedAboutProgram = 0;
            int customerCommunicatedAboutOrderCancellation = 0;
            int callAssignedForQCDataTransferredToSVC = 0;
            int callGoScheduledAppointmentTaken = 0;
            int appointmentRescheduled = 0;
            int QCAppointmentDeclinedByCustomerNotInterested = 0;
            int QCInProgress = 0;
            int QCFail = 0;
            int amountApprovedByCustomerAfterQC = 0;
            int QCAmountConfirmationDeclinedByCustomerAfterQC = 0;
            int newproductdeliveredInstalledBySponsor = 0;
            int callAssignedForPickup = 0;
            int pickupAppointmentIsFixed = 0;
            int ticketIsAssignedForPickup = 0;
            int pickupTicketCancellationByUTC = 0;
            int customerHasAskedforPickupReschedule = 0;
            int pickupcompleted = 0;
            int pickupProductHasReachedLogisticsHub = 0;
            int pickupProductIsAssignedForDropToEVC = 0;
            int pickupAppointmentDeclinedByCustomerToCallCenter = 0;
            int physicalEvaluationAtTheTimeOfPickupFail = 0;
            int amountPaidToCustomer = 0;
            int amountPaidToSponsor = 0;
            int productDeliveredAtEVC = 0;
            int EVCRejectionOfTheMaterial = 0;
            int posted = 0;
            try
            {
                DataSet ds = _exchangeOrderRepository.GetOrderStatusSummaryForAllBU();
                if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                {
                    pickupByDealer = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    duplicateOrder = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
                    customerAddressDifferentFromRegisteredAddress = Convert.ToInt32(ds.Tables[2].Rows[0][0]);
                    inTransit = Convert.ToInt32(ds.Tables[3].Rows[0][0]);
                    orderCreatedBySponsor = Convert.ToInt32(ds.Tables[4].Rows[0][0]);
                    exchangeOrderCancelledAfterCreation = Convert.ToInt32(ds.Tables[5].Rows[0][0]);
                    customerCommunicatedAboutProgram = Convert.ToInt32(ds.Tables[6].Rows[0][0]);
                    customerCommunicatedAboutOrderCancellation = Convert.ToInt32(ds.Tables[7].Rows[0][0]);
                    callAssignedForQCDataTransferredToSVC = Convert.ToInt32(ds.Tables[8].Rows[0][0]);
                    callGoScheduledAppointmentTaken = Convert.ToInt32(ds.Tables[9].Rows[0][0]);
                    appointmentRescheduled = Convert.ToInt32(ds.Tables[10].Rows[0][0]);
                    QCAppointmentDeclinedByCustomerNotInterested = Convert.ToInt32(ds.Tables[11].Rows[0][0]);
                    QCInProgress = Convert.ToInt32(ds.Tables[12].Rows[0][0]);
                    QCFail = Convert.ToInt32(ds.Tables[13].Rows[0][0]);
                    amountApprovedByCustomerAfterQC = Convert.ToInt32(ds.Tables[14].Rows[0][0]);
                    QCAmountConfirmationDeclinedByCustomerAfterQC = Convert.ToInt32(ds.Tables[15].Rows[0][0]);
                    newproductdeliveredInstalledBySponsor = Convert.ToInt32(ds.Tables[16].Rows[0][0]);
                    callAssignedForPickup = Convert.ToInt32(ds.Tables[17].Rows[0][0]);
                    pickupAppointmentIsFixed = Convert.ToInt32(ds.Tables[18].Rows[0][0]);
                    ticketIsAssignedForPickup = Convert.ToInt32(ds.Tables[19].Rows[0][0]);
                    pickupTicketCancellationByUTC = Convert.ToInt32(ds.Tables[20].Rows[0][0]);
                    customerHasAskedforPickupReschedule = Convert.ToInt32(ds.Tables[21].Rows[0][0]);
                    pickupcompleted = Convert.ToInt32(ds.Tables[22].Rows[0][0]);
                    pickupProductHasReachedLogisticsHub = Convert.ToInt32(ds.Tables[23].Rows[0][0]);
                    pickupProductIsAssignedForDropToEVC = Convert.ToInt32(ds.Tables[24].Rows[0][0]);
                    pickupAppointmentDeclinedByCustomerToCallCenter = Convert.ToInt32(ds.Tables[25].Rows[0][0]);
                    physicalEvaluationAtTheTimeOfPickupFail = Convert.ToInt32(ds.Tables[26].Rows[0][0]);
                    amountPaidToCustomer = Convert.ToInt32(ds.Tables[27].Rows[0][0]);
                    amountPaidToSponsor = Convert.ToInt32(ds.Tables[28].Rows[0][0]);
                    productDeliveredAtEVC = Convert.ToInt32(ds.Tables[29].Rows[0][0]);
                    EVCRejectionOfTheMaterial = Convert.ToInt32(ds.Tables[30].Rows[0][0]);
                    posted = Convert.ToInt32(ds.Tables[31].Rows[0][0]);
                }

                ViewBag.pickupByDealer = pickupByDealer;
                ViewBag.duplicateOrder = duplicateOrder;
                ViewBag.customerAddressDifferentFromRegisteredAddress = customerAddressDifferentFromRegisteredAddress;
                ViewBag.inTransit = inTransit;
                ViewBag.orderCreatedBySponsor = orderCreatedBySponsor;
                ViewBag.exchangeOrderCancelledAfterCreation = exchangeOrderCancelledAfterCreation;
                ViewBag.customerCommunicatedAboutProgram = customerCommunicatedAboutProgram;
                ViewBag.customerCommunicatedAboutOrderCancellation = customerCommunicatedAboutOrderCancellation;
                ViewBag.callAssignedForQCDataTransferredToSVC = callAssignedForQCDataTransferredToSVC;
                ViewBag.callGoScheduledAppointmentTaken = callGoScheduledAppointmentTaken;
                ViewBag.appointmentRescheduled = appointmentRescheduled;
                ViewBag.QCAppointmentDeclinedByCustomerNotInterested = QCAppointmentDeclinedByCustomerNotInterested;


                ViewBag.QCInProgress = QCInProgress;
                ViewBag.QCFail = QCFail;
                ViewBag.amountApprovedByCustomerAfterQC = amountApprovedByCustomerAfterQC;
                ViewBag.QCAmountConfirmationDeclinedByCustomerAfterQC = QCAmountConfirmationDeclinedByCustomerAfterQC;
                ViewBag.newproductdeliveredInstalledBySponsor = newproductdeliveredInstalledBySponsor;
                ViewBag.callAssignedForPickup = callAssignedForPickup;
                ViewBag.pickupAppointmentIsFixed = pickupAppointmentIsFixed;
                ViewBag.ticketIsAssignedForPickup = ticketIsAssignedForPickup;
                ViewBag.pickupTicketCancellationByUTC = pickupTicketCancellationByUTC;
                ViewBag.customerHasAskedforPickupReschedule = customerHasAskedforPickupReschedule;
                ViewBag.pickupcompleted = pickupcompleted;
                ViewBag.pickupProductHasReachedLogisticsHub = pickupProductHasReachedLogisticsHub;
                ViewBag.pickupProductIsAssignedForDropToEVC = pickupProductIsAssignedForDropToEVC;
                ViewBag.pickupAppointmentDeclinedByCustomerToCallCenter = pickupAppointmentDeclinedByCustomerToCallCenter;
                ViewBag.physicalEvaluationAtTheTimeOfPickupFail = physicalEvaluationAtTheTimeOfPickupFail;
                ViewBag.amountPaidToCustomer = amountPaidToCustomer;
                ViewBag.amountPaidToSponsor = amountPaidToSponsor;
                ViewBag.productDeliveredAtEVC = productDeliveredAtEVC;
                ViewBag.EVCRejectionOfTheMaterial = EVCRejectionOfTheMaterial;
                ViewBag.posted = posted;
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeController", "StatusDashboard", ex);
            }

            return View();
        }
        #endregion

        #region Customers Personal data pushed to database
        public ActionResult CustomerDetails(string RegNo)
        {
            SponsrOrderSyncManager sponsrOrderSyncManager = new SponsrOrderSyncManager();
            _exchangeOrderManager = new BAL.SponsorsApiCall.ExchangeOrderManager();
            _businessUnitRepository = new BusinessUnitRepository();
            _businessPartnerRepository = new BusinessPartnerRepository();
            _productCategoryRepository = new ProductCategoryRepository();
            _loginRepository = new LoginDetailsUTCRepository();
            _customerDetailsRepository = new CustomerDetailsRepository();
            _exchangeOrderRepository = new ExchangeOrderRepository();
            _productTypeRepository = new ProductTypeRepository();
            _productConditionLabelRepository = new ProductConditionLabelRepository();
            tblExchangeOrder ExchangeObj = null;
            tblCustomerDetail CustomerObj = null;
            ExchangeOrderDataContract exchangeOrderDC = new ExchangeOrderDataContract();
            List<tblProductConditionLabel> ConditionLabelObj = new List<tblProductConditionLabel>();
            string Message = string.Empty;
            try
            {
                if (RegNo != null)
                {
                    string reg = RegNo.Replace(".", "");
                    ExchangeObj = _exchangeOrderRepository.GetSingle(x => !string.IsNullOrEmpty(x.RegdNo) && x.RegdNo.ToLower().Equals(reg.ToLower()) && x.IsActive == true);
                    if (ExchangeObj != null)
                    {
                        CustomerObj = _customerDetailsRepository.GetSingle(x => x.Id == ExchangeObj.CustomerDetailsId);
                        if (CustomerObj != null)
                        {
                            exchangeOrderDC.PhoneNumber = CustomerObj.PhoneNumber;
                            exchangeOrderDC.PhoneNo = CustomerObj.PhoneNumber;
                        }

                        if (!string.IsNullOrEmpty(CustomerObj.FirstName) && !string.IsNullOrEmpty(CustomerObj.LastName) && !string.IsNullOrEmpty(CustomerObj.Email))
                        {
                            Message = "Customer details already updated";
                            return RedirectToAction("DetailsFailedResponse", "Exchange", new { Message = Message });
                        }
                        tblBusinessPartner BuisnessPrtnerObj = _businessPartnerRepository.GetSingle(x => x.BusinessPartnerId == ExchangeObj.BusinessPartnerId && x.IsActive == true);
                        if (BuisnessPrtnerObj != null)
                        {
                            exchangeOrderDC.BULogoName = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Content/DB_Files/SponsorLogo/" + BuisnessPrtnerObj.LogoImage;
                        }
                        exchangeOrderDC.BusinessUnitId = BuisnessPrtnerObj.BusinessUnitId;
                        exchangeOrderDC.BusinessPartnerId = BuisnessPrtnerObj.BusinessPartnerId;
                        //get price master name id
                        if (exchangeOrderDC.BusinessUnitId > 0 && exchangeOrderDC.BusinessPartnerId > 0)
                        {
                            OldProductDetailsManager oldProductDetailsManager = new OldProductDetailsManager();
                            PriceMasterMappingDataContract priceMasterMappingDataContract = new PriceMasterMappingDataContract();

                            priceMasterMappingDataContract.BusinessunitId = BuisnessPrtnerObj.BusinessUnitId;
                            priceMasterMappingDataContract.BusinessPartnerId = BuisnessPrtnerObj.BusinessPartnerId;
                            PriceMasterNameDataContract priceMasterNameDataContract = new PriceMasterNameDataContract();
                            priceMasterNameDataContract = oldProductDetailsManager.GetPriceNameId(priceMasterMappingDataContract);
                            exchangeOrderDC.priceMasterNameID = Convert.ToInt32(priceMasterNameDataContract.PriceNameId);
                        }

                        exchangeOrderDC.RegdNo = ExchangeObj.RegdNo;
                        exchangeOrderDC.ExchangePrice = Convert.ToDecimal(ExchangeObj.ExchangePrice != null ? ExchangeObj.ExchangePrice : 0);
                        exchangeOrderDC.BasePrice = Convert.ToString(ExchangeObj.BaseExchangePrice != null ? ExchangeObj.BaseExchangePrice : 0);
                        exchangeOrderDC.RegdNo = ExchangeObj.RegdNo;
                        exchangeOrderDC.SponsorOrderNumber = ExchangeObj.SponsorOrderNumber;
                        exchangeOrderDC.FormatName = ExchangeOrderManager.GetEnumDescription(FormatTypeEnum.Dealer);
                        if (exchangeOrderDC.BusinessUnitId != null)
                        {
                            tblBusinessUnit BusinessUnitObj = _businessUnitRepository.GetSingle(x => x.BusinessUnitId == exchangeOrderDC.BusinessUnitId && x.IsActive == true);
                            if (BusinessUnitObj != null)
                            {
                                // code to get condition labels from database

                                ConditionLabelObj = _productConditionLabelRepository.GetList(x => x.BusinessUnitId == BusinessUnitObj.BusinessUnitId && x.IsActive == true && x.BusinessPartnerId == exchangeOrderDC.BusinessPartnerId).ToList();
                                if (ConditionLabelObj.Count > 0)
                                {
                                    exchangeOrderDC.ProductConditionCount = ConditionLabelObj.Count;
                                    exchangeOrderDC.QualityCheckList = ConditionLabelObj.Select(x => new SelectListItem
                                    {
                                        Text = x.PCLabelName,
                                        Value = x.OrderSequence.ToString()
                                    }).ToList();
                                }
                                else
                                {
                                    Message = "Product Condition not available";
                                    return RedirectToAction("DetailsFailedResponse", "Exchange", new { Message = Message });
                                }
                                exchangeOrderDC.QualityCheckList = exchangeOrderDC.QualityCheckList.OrderByDescending(o => o.Value).ToList();
                            }
                        }
                        if (ExchangeObj.ProductTypeId != null)
                        {
                            tblProductType productType = _productTypeRepository.GetSingle(x => x.Id == ExchangeObj.ProductTypeId && x.IsActive == true);
                            if (productType != null)
                            {
                                exchangeOrderDC.ProductTypeId = productType.Id;

                                exchangeOrderDC.ProductType = productType.Description;
                                if (productType.ProductCatId != null)
                                {
                                    tblProductCategory productCategory = _productCategoryRepository.GetSingle(x => x.Id == productType.ProductCatId && x.IsActive == true);
                                    if (productCategory != null)
                                    {
                                        exchangeOrderDC.ProductCategoryId = productCategory.Id;
                                        exchangeOrderDC.ProductCategory = productCategory.Description;
                                    }
                                }
                            }
                        }
                        //Code to set quality as selected by customer
                        if (ExchangeObj.ProductCondition == "Excellent")
                        {
                            exchangeOrderDC.QualityCheck = Convert.ToInt32(ProductConditionEnum.Excellent);
                        }
                        if (ExchangeObj.ProductCondition == "Good")
                        {
                            exchangeOrderDC.QualityCheck = Convert.ToInt32(ProductConditionEnum.Good);
                        }
                        if (ExchangeObj.ProductCondition == "Average")
                        {
                            exchangeOrderDC.QualityCheck = Convert.ToInt32(ProductConditionEnum.Average);
                        }
                        if (ExchangeObj.ProductCondition == "Not Working")
                        {
                            exchangeOrderDC.QualityCheck = Convert.ToInt32(ProductConditionEnum.NotWorking);
                        }
                        if (ExchangeObj.BrandId != null)
                        {
                            exchangeOrderDC.BrandId = Convert.ToInt32(ExchangeObj.BrandId);
                        }



                    }
                    return View(exchangeOrderDC);
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("Exchange", "CustomerDetails", ex);
                return View(exchangeOrderDC);
            }
            return View(exchangeOrderDC);
        }

        [HttpPost]
        public ActionResult CustomerDetails(ExchangeOrderDataContract ExchangeObj)
        {
            _exchangeOrderManager = new BAL.SponsorsApiCall.ExchangeOrderManager();
            _businessUnitRepository = new BusinessUnitRepository();
            _businessPartnerRepository = new BusinessPartnerRepository();
            _productCategoryRepository = new ProductCategoryRepository();
            _loginRepository = new LoginDetailsUTCRepository();
            _customerDetailsRepository = new CustomerDetailsRepository();
            ExchangeOrderDataContract exchangeOrderDC = new ExchangeOrderDataContract();
            ProductOrderResponseDataContract productOrderResponseDC = null;
            string message = string.Empty;
            try
            {
                if (ExchangeObj != null)
                {
                    ExchangeObj.City = ExchangeObj.City1;
                    ExchangeObj.StateName = ExchangeObj.State1;
                    ExchangeObj.ExchangePrice = Convert.ToDecimal(ExchangeObj.ExchangePriceString);

                    productOrderResponseDC = _exchangeOrderManager.UpdateExchangeOrder(ExchangeObj);
                    if (productOrderResponseDC != null)
                        message = "Your product details have been received at rockingdeals for reference no is " + ExchangeObj.RegdNo + ". Our quality check team will soon knock at your door.";
                    else
                    {
                        message = "Order not Created";
                        return RedirectToAction("DetailsFailedResponse", "Exchange", new { Message = message });
                    }

                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("Exchange", "CustomerDetails", ex);
                message = ex.Message;
                return RedirectToAction("DetailsFailedResponse", "Exchange", new { Message = message });
            }
            return RedirectToAction("Details", "Exchange", new { Message = message });
        }
        #endregion


        #region City And pincode
        [HttpPost]
        public JsonResult GetPincodeList(string pintext, int? buid)
        {
            pinCodeRepository = new PinCodeRepository();
            IEnumerable<SelectListItem> pincodeList = null;
            List<tblPinCode> pincodeListForPineLabs = null;
            try
            {
                DataTable dt = pinCodeRepository.GetPincodeListbybuidforex(pintext, buid);

                pincodeListForPineLabs = GenericConversionHelper.DataTableToList<tblPinCode>(dt);
                if (pincodeListForPineLabs.Count > 0)
                {
                    pincodeList = pincodeListForPineLabs.Select(x => new SelectListItem
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
                LibLogging.WriteErrorToDB("PineLabs", "GetPincodeforPineLabs", ex);
            }
            var result = new SelectList(pincodeList, "Value", "Text");
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetState(int pincode)
        {
            pinCodeRepository = new PinCodeRepository();

            MyGateCityState mygateCityState = new MyGateCityState();
            List<tblPinCode> pincodeListForPineLabs = null;

            try
            {
                DataTable dt = pinCodeRepository.GetStateList(pincode);
                if (dt != null && dt.Rows.Count > 0)
                {
                    pincodeListForPineLabs = GenericConversionHelper.DataTableToList<tblPinCode>(dt);
                }
                if(pincodeListForPineLabs != null)
                {
                    foreach (var item in pincodeListForPineLabs)
                    {
                        mygateCityState.StateName = item.State;
                        mygateCityState.CityName = item.Location;
                        //mygateCityState.AreaLocalityName = item.AreaLocality;
                    }
                }
                
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("Exchange", "GetState", ex);
            }

            return Json(mygateCityState, JsonRequestBehavior.AllowGet);
        }
        #endregion



        //#region City And pincode
        //[HttpPost]
        //public JsonResult GetPincodeList(string pintext,int? buid)
        //{
        //    pinCodeRepository = new PinCodeRepository();
        //    IEnumerable<SelectListItem> pincodeList = null;
        //    List<tblPinCode> pincodeListForPineLabs = null;
        //    try
        //    {
        //        DataTable dt = pinCodeRepository.GetPincodeList();
        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            pincodeListForPineLabs = GenericConversionHelper.DataTableToList<tblPinCode>(dt);
        //        }
        //        pincodeList = (pincodeListForPineLabs).AsEnumerable().Select(prodt => new SelectListItem() { Text = prodt.ZipCode.ToString(), Value = prodt.ZipCode.ToString() });
        //        pincodeList = pincodeList.OrderBy(o => o.Text).ToList();
        //        pincodeList = pincodeList.Where(x => x.Text.Contains(pintext)).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        LibLogging.WriteErrorToDB("PineLabs", "GetPincodeforPineLabs", ex);
        //    }
        //    var result = new SelectList(pincodeList, "Value", "Text");
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}
        //[HttpGet]
        //public JsonResult GetState(int pincode)
        //{
        //    pinCodeRepository = new PinCodeRepository();

        //    MyGateCityState mygateCityState = new MyGateCityState();
        //    List<tblPinCode> pincodeListForPineLabs = null;

        //    try
        //    {
        //        DataTable dt = pinCodeRepository.GetStateList(pincode);
        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            pincodeListForPineLabs = GenericConversionHelper.DataTableToList<tblPinCode>(dt);
        //        }
        //        foreach (var item in pincodeListForPineLabs)
        //        {
        //            mygateCityState.StateName = item.State;
        //            mygateCityState.CityName = item.Location;
        //            //mygateCityState.AreaLocalityName = item.AreaLocality;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LibLogging.WriteErrorToDB("Exchange", "GetState", ex);
        //    }

        //    return Json(mygateCityState, JsonRequestBehavior.AllowGet);
        //}
        //#endregion

        #region Select Location For Exchange
        public ActionResult SelectLocation(ProductDetailsToExchange productDetails)
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
                ExchangeObj.BUId = productDetails.BusinessUnitId;
                if (productDetails != null)
                {
                    if (productDetails.BULogoName != null)
                    {
                        ExchangeObj.BULogoName = productDetails.BULogoName;
                    }
                    else
                    {
                        BusinessUnitObj = _businessUnitRepository.GetSingle(x => x.BusinessUnitId == productDetails.BusinessUnitId);
                        if (BusinessUnitObj != null)
                        {
                            ExchangeObj.BULogoName = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Content/DB_Files/SponsorLogo/" + BusinessUnitObj.LogoName;

                            if (BusinessUnitObj.ShowEmplyeeCode == true)
                            {
                                ExchangeObj.showEmployeeId = BusinessUnitObj.ShowEmplyeeCode;
                            }
                            else
                            {
                                ExchangeObj.EmployeeId = "3";
                            }
                        }
                    }
                    //get state list
                    DataTable dt = _businessPartnerRepository.GetStateList();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        businessPartnerList = GenericConversionHelper.DataTableToList<tblBusinessPartner>(dt);
                        List<string> states = businessPartnerList.Where(x => x.BusinessUnitId == productDetails.BusinessUnitId && (x.IsExchangeBP != null && x.IsExchangeBP == true)).OrderBy(o => o.State).Select(x => x.State).Distinct().ToList();
                        List<SelectListItem> stateListItems = states.Select(x => new SelectListItem
                        {
                            Text = x,
                            Value = x
                        }).ToList();
                        ViewBag.StateList = new SelectList(stateListItems, "Text", "Text");
                    }
                    //Setting Values from  productdetails model
                    ExchangeObj.OldProductCategoryId = productDetails.OldProductCatId;
                    ExchangeObj.oldProductypeId = productDetails.OldTypeId;
                    ExchangeObj.OldBrandId = productDetails.BrandId;
                    ExchangeObj.NewBrandId = productDetails.NewBrandId;
                    ExchangeObj.NewCategoryId = productDetails.NewProductCategoryId;
                    ExchangeObj.NewTypeId = productDetails.NewProductTypeId;
                    ExchangeObj.ProductAge = productDetails.ProductAge;
                    ExchangeObj.IsSweetnerModelBased = productDetails.IsSweetnerModelBased;
                    ExchangeObj.QualityCheckValue = productDetails.QualityCheck;
                    ExchangeObj.FormatName = productDetails.FormatName;
                    ExchangeObj.ExchangePrice = productDetails.ExchangePrice;
                    //Store List For BusinessPartner list
                    ExchangeObj.StoreList = new List<SelectListItem>();
                    ExchangeObj.ProductModelIdNew = productDetails.ProductModelIdNew;
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeController", "SelectLocation", ex);
            }

            return View(ExchangeObj);
        }

        [HttpPost]
        public ActionResult SelectLocation(ExchagneViewModel ExchangeObj)
        {
            try
            {
                //Redirecting to exchReg With Required Data
                if (ModelState.IsValid)
                {
                    return RedirectToAction("ExchReg", "Exchange", new { BUId = ExchangeObj.BUId, StateName = ExchangeObj.StateName, CityName = ExchangeObj.CityName, FormatName = ExchangeObj.FormatName, ZipCode = ExchangeObj.ZipCode, ExchangePrice = ExchangeObj.ExchangePrice, OldProductCategoryId = ExchangeObj.OldProductCategoryId, oldProductypeId = ExchangeObj.oldProductypeId, OldBrandId = ExchangeObj.OldBrandId, NewBrandId = ExchangeObj.NewBrandId, NewCategoryId = ExchangeObj.NewCategoryId, NewTypeId = ExchangeObj.NewTypeId, ProductAge = ExchangeObj.ProductAge, QualityCheckValue = ExchangeObj.QualityCheckValue, IsSweetnerModelBased = ExchangeObj.IsSweetnerModelBased, BusinessPartnerId = ExchangeObj.BusinessPartnerId, ProductModelIdNew = ExchangeObj.ProductModelIdNew, EmployeeId = ExchangeObj.EmployeeId });
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeController", "SelectLocation", ex);
            }

            return View(ExchangeObj);
        }
        #endregion

        #region  Get Product Category
        [HttpGet]
        public JsonResult GetProductCategoryForNew(int buid, int oldCatId)
        {
            _productCategoryRepository = new ProductCategoryRepository();
            _productCategoryMappingRepository = new ProductCategoryMappingRepository();
            List<SelectListItem> newProductCategory = new List<SelectListItem>();
            List<tblBUProductCategoryMapping> BUmappingcategory = new List<tblBUProductCategoryMapping>();
            try
            {
                if (buid > 0 && (buid == Convert.ToInt32(BusinessUnitEnum.Bosch) || buid == Convert.ToInt32(BusinessUnitEnum.BoschDecoline)))
                {
                    if (oldCatId == Convert.ToInt32(ProductCatEnum.CookTop))
                    {
                        tblProductCategory productObj = _productCategoryRepository.GetSingle(x => x.Id == oldCatId && x.IsAllowedForNew == true && x.IsActive == true);
                        if (productObj != null)
                        {
                            newProductCategory.Add(new SelectListItem { Text = productObj.Description, Value = productObj.Id.ToString() });
                        }
                    }
                    else
                    {
                        //new product categorgy list
                        List<tblProductCategory> prodGroupListForBosch = new List<tblProductCategory>();
                        List<tblBUProductCategoryMapping> productCategoryForNew = new List<tblBUProductCategoryMapping>();
                        DataTable dt = _productCategoryMappingRepository.GetNewProductCategory(buid);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            productCategoryForNew = GenericConversionHelper.DataTableToList<tblBUProductCategoryMapping>(dt);
                        }
                        productCategoryForNew.Remove(BUmappingcategory.FirstOrDefault(x => x.ProductCatId == Convert.ToInt32(ProductCatEnum.CookTop)));
                        if (productCategoryForNew != null && productCategoryForNew.Count > 0)
                        {
                            foreach (var item in productCategoryForNew)
                            {
                                tblProductCategory productObj = _productCategoryRepository.GetSingle(x => x.Id == item.ProductCatId && x.IsAllowedForNew == true && x.IsActive == true && x.Description.ToLower() != "cook top");
                                if (productObj != null)
                                {
                                    newProductCategory.Add(new SelectListItem { Text = productObj.Description, Value = productObj.Id.ToString() });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("Exchange", "GetProductCategoryForNew", ex);
            }

            return Json(newProductCategory, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get C Product Details
        [HttpGet]
        public JsonResult GetCProductDetails(int buid, int productCategory, bool qualityRequired)
        {

            BrandOBJ brandobj = new BrandOBJ();
            if ((buid == Convert.ToInt32(BusinessUnitEnum.Bosch) || buid == Convert.ToInt32(BusinessUnitEnum.BoschDecoline)) && productCategory == Convert.ToInt32(ProductCatEnum.CookTop))
            {
                brandobj.flag = true;
                brandobj.brandIdOld = Convert.ToInt32(BrandIdEnum.OtherBrand).ToString();
            }
            else
            {
                brandobj.flag = false;
            }

            if (qualityRequired == false)
            {
                brandobj.flag = true;
            }
            return Json(brandobj, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region  Old Product Details To Exchange
        [HttpGet]
        public ActionResult ProductDetailsToExchange(RedirectModel ExchangeObj)
        {
            _productTypeRepository = new ProductTypeRepository();
            _productCategoryRepository = new ProductCategoryRepository();
            _productCategoryMappingRepository = new ProductCategoryMappingRepository();
            _brandRepository = new BrandRepository();
            _exchangeOrderManager = new ExchangeOrderManager();
            _businessUnitRepository = new BusinessUnitRepository();
            _businessPartnerRepository = new BusinessPartnerRepository();
            _priceMasterRepository = new PriceMasterRepository();
            _loginRepository = new LoginDetailsUTCRepository();
            _productConditionLabelRepository = new ProductConditionLabelRepository();
            _OrderBasedConfigurationRepository = new OrderBasedConfigurationRepository();
            List<SelectListItem> newProductCategory = new List<SelectListItem>();
            ProductDetailsToExchange productDetails = new ProductDetailsToExchange();
            List<tblBUProductCategoryMapping> BUProductCategoryList = new List<tblBUProductCategoryMapping>();
            tblBusinessUnit BusinessUnitObj = null;
            tblBrand brandObj = null;
            tblBusinessPartner BusinessPartnerObj = null;
            //List<tblPriceMaster> pricemaster = null;
            List<tblProductCategory> prodGroupListForExchange = new List<tblProductCategory>();
            //tblProductCategory categoryObj = null;
            List<tblProductConditionLabel> ConditionLabelObj = new List<tblProductConditionLabel>();
            List<tblBrand> NewBrandList = new List<tblBrand>();
            Login login = null;
            string Message = string.Empty;
            tblOrderBasedConfig orderBasedConfig = null;
            try
            {
                if (ExchangeObj != null)
                {      // Code to det business unit logo

                    if (ExchangeObj.BUId > 0)
                    {
                        BusinessUnitObj = _businessUnitRepository.GetSingle(x => x.BusinessUnitId == ExchangeObj.BUId && x.IsActive == true);
                        if (BusinessUnitObj != null)
                        {
                            int bpid = (int)ExchangeObj.BusinessPartnerId;

                            bool? IsCouponsAvailable = _businessPartnerRepository.GetCouponAvailableStatusByBpId(bpid);
                            productDetails.IsCouponsAvailable = IsCouponsAvailable;
                            //call order based configuration table
                            orderBasedConfig = _OrderBasedConfigurationRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == Convert.ToInt32(ExchangeObj.BUId) && x.BusinessPartnerId == Convert.ToInt32(ExchangeObj.BusinessPartnerId));
                            if (orderBasedConfig != null)
                            {
                                productDetails.IsBuMultiBrand = Convert.ToBoolean(orderBasedConfig.IsBPMultiBrand != null ? orderBasedConfig.IsBPMultiBrand : false);
                                productDetails.IsValidationBasedSweetner = Convert.ToBoolean(orderBasedConfig.IsValidationBasedSweetener != null ? orderBasedConfig.IsValidationBasedSweetener : false);
                                productDetails.IsSweetnerModelBased = Convert.ToBoolean(orderBasedConfig.IsSweetenerModalBased != null ? orderBasedConfig.IsSweetenerModalBased : false);
                                productDetails.IsSweetnerBasedonModal = Convert.ToBoolean(orderBasedConfig.IsSweetenerModalBased != null ? orderBasedConfig.IsSweetenerModalBased : false);

                                //Check is product MultiBrand or not and configure new brand according to data

                                if (productDetails.IsBuMultiBrand == true)
                                {
                                    NewBrandList = _brandRepository.GetList(x => x.IsActive == true).ToList();
                                    if (NewBrandList.Count > 0)
                                    {
                                        var distinctBrands = NewBrandList.Distinct().ToList();
                                        // Transform distinct items into SelectListItem objects
                                        productDetails.NewBrandList = distinctBrands
                                            .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).ToList();

                                    }
                                    else
                                    {
                                        productDetails.NewBrandList = NewBrandList.Select(x => new SelectListItem
                                        {
                                            Text = "No Brand Available",
                                            Value = 0.ToString()
                                        }).ToList();
                                    }
                                }
                                else
                                {
                                    productDetails.IsBuMultiBrand = false;

                                    brandObj = _brandRepository.GetSingle(x => x.BusinessUnitId == ExchangeObj.BUId);
                                    if (brandObj != null)
                                    {
                                        productDetails.NewBrandId = brandObj.Id;
                                    }
                                    else
                                    {
                                        Message = "No brand found for new product";
                                        return RedirectToAction("DetailsFailedResponse", "Exchange", new { Message = Message });
                                    }
                                }

                            }
                            else
                            {
                                Message = "No data found in order based config table";
                                return RedirectToAction("DetailsFailedResponse", "Exchange", new { Message = Message });
                            }

                            // only for Relience Display Model number in new product details
                            productDetails.ProductModelList = new List<SelectListItem>();

                            //if (BusinessUnitObj.BusinessUnitId == Convert.ToInt32(BusinessUnitEnum.Relience))
                            //{
                            if (BusinessUnitObj.IsModelDetailRequired == true)
                            {
                                productDetails.IsModelNumberRequired = Convert.ToBoolean(BusinessUnitObj.IsModelDetailRequired);
                            }
                            else
                            {
                                productDetails.IsModelNumberRequired = false;
                            }
                            //}
                            // code to check if Quality needs to be shown on ui
                            if (BusinessUnitObj.IsQualityRequiredOnUI == true)
                            {
                                productDetails.IsQualityRequiredOnUI = true;
                            }
                            else
                            {
                                productDetails.IsQualityRequiredOnUI = false;
                            }
                            // only for Relience
                            if (BusinessUnitObj.IsNewProductDetailsRequired != null)
                            {
                                productDetails.IsNewProductDetailsRequired = Convert.ToBoolean(BusinessUnitObj.IsNewProductDetailsRequired);
                            }
                            else
                            {
                                productDetails.IsNewProductDetailsRequired = false;
                            }
                            if (BusinessUnitObj.IsNewBrandRequired == true)
                            {
                                productDetails.IsNewBrandRequired = Convert.ToBoolean(BusinessUnitObj.IsNewBrandRequired);
                            }
                            else
                            {
                                productDetails.IsNewBrandRequired = false;
                            }
                            productDetails.BusinessUnitLogo = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Content/DB_Files/SponsorLogo/" + BusinessUnitObj.LogoName;
                            productDetails.BusinesUnitName = BusinessUnitObj.Name;
                            //Business Unit Logo Name To Carry Forward
                            productDetails.BULogoName = ConfigurationManager.AppSettings["BaseUrl"].ToString() + "Content/DB_Files/SponsorLogo/" + BusinessUnitObj.LogoName;

                            //Added by Vk 
                            productDetails.buBasedSweetnerValidationsList = GetBUSweetnerValidationQuestions(BusinessUnitObj.BusinessUnitId);
                            //Added by Vk

                            // code to get condition labels from database
                            ConditionLabelObj = _productConditionLabelRepository.GetList(x => x.BusinessUnitId == BusinessUnitObj.BusinessUnitId && x.BusinessPartnerId == ExchangeObj.BusinessPartnerId && x.IsActive == true).ToList();

                            //ConditionLabelObj = _productConditionLabelRepository.GetList(x => x.BusinessUnitId == BusinessUnitObj.BusinessUnitId && x.IsActive == true).ToList();
                            if (ConditionLabelObj.Count > 0)
                            {
                                productDetails.ProductConditionCount = ConditionLabelObj.Count;
                                productDetails.QualityCheckList = ConditionLabelObj.Select(x => new SelectListItem
                                {
                                    Text = x.PCLabelName,
                                    Value = x.OrderSequence.ToString()
                                }).ToList();
                            }
                            else
                            {
                                Message = "Product Condition not available";
                                return RedirectToAction("DetailsFailedResponse", "Exchange", new { Message = Message });
                            }
                            productDetails.QualityCheckList = productDetails.QualityCheckList.OrderByDescending(o => o.Value).ToList();


                        }
                        else
                        {
                            Message = "Business unit not found check if Business unit is active or not";
                            return RedirectToAction("DetailsFailedResponse", "Exchange", new { Message = Message });
                        }
                        login = _loginRepository.GetSingle(x => x.SponsorId == ExchangeObj.BUId);
                        if (login != null)
                        {
                            productDetails.priceCode = login.PriceCode;
                        }

                    }
                    //Old product category for exchange
                    if (BusinessUnitObj.BusinessUnitId > 0)
                    {
                        OldProductDetailsManager oldProductDetailsManager = new OldProductDetailsManager();
                        PriceMasterMappingDataContract priceMasterMappingDataContract = new PriceMasterMappingDataContract();
                        priceMasterMappingDataContract.BusinessunitId = BusinessUnitObj.BusinessUnitId;
                        priceMasterMappingDataContract.BusinessPartnerId = ExchangeObj.BusinessPartnerId > 0 ? ExchangeObj.BusinessPartnerId : 0;
                        PriceMasterNameDataContract priceMasterNameDataContract = new PriceMasterNameDataContract();
                        priceMasterNameDataContract = oldProductDetailsManager.GetPriceNameId(priceMasterMappingDataContract);

                        if (priceMasterNameDataContract != null && priceMasterNameDataContract.PriceNameId > 0)
                        {
                            priceMasterNameDataContract.PriceNameId = priceMasterNameDataContract.PriceNameId;
                            productDetails.priceMasterNameID = Convert.ToInt32(priceMasterNameDataContract.PriceNameId);

                            prodGroupListForExchange = oldProductDetailsManager.GetProductCatListByPriceMasterNameId(Convert.ToInt32(priceMasterNameDataContract.PriceNameId));
                        }
                        else if (priceMasterNameDataContract != null && priceMasterNameDataContract.ErrorMessage != null)
                        {
                            Message = priceMasterNameDataContract.ErrorMessage;

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


                    #region New product categorgy list
                    //new product categorgy list
                    List<tblProductCategory> prodGroupListForBosch = GetProductCatForNew(ExchangeObj.BUId);
                    if (prodGroupListForBosch != null && prodGroupListForBosch.Count > 0)
                    {
                        ViewBag.NewProductCategoryList = new SelectList(prodGroupListForBosch, "Id", "Description_For_ABB");
                    }
                    else
                    {
                        Message = "No product category found for new product";
                        return RedirectToAction("DetailsFailedResponse", "Exchange", new { Message = Message });
                    }
                    #endregion


                    //Quality Check 
                    if (productDetails.IsQualityRequiredOnUI == false)
                    {
                        productDetails.QualityCheck = Convert.ToInt32(ProductConditionEnum.Working);
                    }
                    else
                    {
                        productDetails.QualityCheck = Convert.ToInt32(ProductConditionEnum.NotWorking);
                    }

                    productDetails.BusinessUnitForHidingQualityCheck = Convert.ToInt32(BusinessUnitEnum.Diakin);
                    //By Default non working
                    productDetails.NewProductTypeList = new List<SelectListItem>();
                    productDetails.BrandList = new List<SelectListItem>();
                    productDetails.OldProductTypeList = new List<SelectListItem>();
                    productDetails.BusinessUnitId = ExchangeObj.BUId;
                    productDetails.BusinessPartnerId = ExchangeObj.BusinessPartnerId;

                    //product Age
                    productDetails.ProductAge = Convert.ToInt32(ProductAgeEnum.ProductAge);
                    //Orc Store
                    productDetails.IsOrc = false;

                    //Is BusinessPartner D2C if BusinessPartner is >0
                    if (ExchangeObj.BusinessPartnerId > 0)
                    {
                        BusinessPartnerObj = _businessPartnerRepository.GetSingle(x => x.BusinessPartnerId == ExchangeObj.BusinessPartnerId && x.IsActive == true && x.IsExchangeBP == true);
                        if (BusinessPartnerObj != null)
                        {
                            if (BusinessPartnerObj.IsD2C == true)
                            {
                                productDetails.IsD2C = Convert.ToBoolean(BusinessPartnerObj.IsD2C);
                                productDetails.FormatName = BusinessPartnerObj.FormatName;
                            }
                            else
                            {
                                productDetails.FormatName = ExchangeOrderManager.GetEnumDescription((FormatTypeEnum.Dealer));
                            }
                            //Code to Check whether to issue voucher or not 
                            productDetails.IsVoucher = Convert.ToBoolean(BusinessPartnerObj.IsVoucher);
                            productDetails.VoucherType = Convert.ToInt32(BusinessPartnerObj.VoucherType);
                            productDetails.IsDeffered = Convert.ToBoolean(BusinessPartnerObj.IsDefferedSettlement);
                        }
                        else
                        {
                            //Format name 
                            productDetails.FormatName = ExchangeOrderManager.GetEnumDescription((FormatTypeEnum.Dealer));
                        }
                    }
                    else
                    {
                        //Format name 
                        productDetails.FormatName = ExchangeOrderManager.GetEnumDescription((FormatTypeEnum.Dealer));
                    }
                    //Close button url
                    productDetails.url = ConfigurationManager.AppSettings["Close"].ToString();
                }

                else
                {
                    Message = "Exchange obj is null";
                    return RedirectToAction("DetailsFailedResponse", "Exchange", new { Message = Message });
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("Exchange", "ProductDetailsToExchange", ex);
                Message = ex.Message;
                return RedirectToAction("DetailsFailedResponse", "Exchange", new { Message = Message });
            }
            return View(productDetails);
        }

        [HttpPost]
        public ActionResult ProductDetailsToExchange(ProductDetailsToExchange productDetails)
        {
            _businessPartnerRepository = new BusinessPartnerRepository();
            tblBusinessPartner tblBusinessPartner = new tblBusinessPartner();

            try
            { 
                if (productDetails != null && productDetails.BusinessPartnerId > 0)
                {
                    return RedirectToAction("ExchReg", "Exchange", new { OldBrandId = productDetails.BrandId, BUId = productDetails.BusinessUnitId, ExchangePrice = productDetails.ExchangePrice, OldProductCategoryId = productDetails.OldProductCatId, oldProductypeId = productDetails.OldTypeId, NewBrandId = productDetails.NewBrandId, NewCategoryId = productDetails.NewProductCategoryId, NewTypeId = productDetails.NewProductTypeId, ProductAge = productDetails.ProductAge, IsSweetnerModelBased = productDetails.IsSweetnerModelBased, QualityCheckValue = productDetails.QualityCheck, FormatName = productDetails.FormatName, BusinessPartnerId = productDetails.BusinessPartnerId, IsD2C = productDetails.IsD2C, IsVoucher = productDetails.IsVoucher, VoucherType = productDetails.VoucherType, IsDeffered = productDetails.IsDeffered, ProductModelIdNew = productDetails.ProductModelIdNew, priceMasterNameID = productDetails.priceMasterNameID, SweetenerBu = productDetails.SweetenerBu, SweetenerBP = productDetails.SweetenerBP, SweetenerDigi2L = productDetails.SweetenerDigi2L, SweetenerTotal = productDetails.SweetenerTotal, ModelNumberId = productDetails.ModelNumberId, BasePrice = productDetails.BasePrice, IsValidationBasedSweetner = productDetails.IsValidationBasedSweetner, IsCouponAplied= productDetails.IsCouponAplied, CouponId= productDetails.CouponId, UsedCouponCode = productDetails.UsedCouponCode, CouponValue = productDetails.CouponValue });
                }
                else
                {
                    return RedirectToAction("SelectLocation", "Exchange", new { BrandId = productDetails.BrandId, BusinessUnitId = productDetails.BusinessUnitId, ExchangePrice = productDetails.ExchangePrice, OldProductCatId = productDetails.OldProductCatId, OldTypeId = productDetails.OldTypeId, NewBrandId = productDetails.NewBrandId, NewProductCategoryId = productDetails.NewProductCategoryId, NewProductTypeId = productDetails.NewProductTypeId, ProductAge = productDetails.ProductAge, IsSweetnerModelBased = productDetails.IsSweetnerModelBased, QualityCheck = productDetails.QualityCheck, FormatName = productDetails.FormatName, BusinessPartnerId = productDetails.BusinessPartnerId, ProductModelIdNew = productDetails.ProductModelIdNew, IsCouponAplied = productDetails.IsCouponAplied, CouponId = productDetails.CouponId, UsedCouponCode= productDetails.UsedCouponCode, CouponValue = productDetails.CouponValue });
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("Exchange", "ProductDetailsToExchange", ex);
            }
            return View(productDetails);
        }
        #endregion

        #region Ajax Method for Loading Store list from City and state
        [HttpGet]
        public JsonResult GetStoreList(string city, string pincode, int buid, bool isBPAssociated)
        {
            _bPBUAssociationRepository = new BPBUAssociationRepository();
            _businessPartnerRepository = new BusinessPartnerRepository();
            pinCodeRepository = new PinCodeRepository();
            _bPPincodeMappingRepository = new BPPincodeMappingRepository();
            List<tblBusinessPartner> businessPartnerList = null;
            List<TblBPPincodeMapping> businessPartnermappingList = null;

            List<SelectListItem> StoreList = null;
            try
            {
                if (city != null && pincode != null && buid > 0)
                {
                    //get state list
                    DataTable dt = _businessPartnerRepository.GetBpListbyPincode(city, pincode, buid);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        businessPartnerList = GenericConversionHelper.DataTableToList<tblBusinessPartner>(dt);

                        if (businessPartnerList.Count > 0)
                        {
                            if (isBPAssociated)
                            {
                                List<tblBPBUAssociation> tblBPBUAssociations = _bPBUAssociationRepository
                                    .GetList(x => x.IsActive == true && x.BusinessUnitId == buid && x.AssociationCode != null)
                                    .ToList();

                                List<tblBusinessPartner> filteredList = new List<tblBusinessPartner>();

                                foreach (var businessPartner in businessPartnerList)
                                {
                                    bool found = tblBPBUAssociations.Any(b => b.BusinessPartnerId == businessPartner.BusinessPartnerId);
                                    if (found)
                                    {
                                        filteredList.Add(businessPartner);
                                    }
                                }

                                if (filteredList.Count > 0)
                                {
                                    StoreList = filteredList.Select(x => new SelectListItem
                                    {
                                        Text = x.Description + ", " + x.AddressLine1,
                                        Value = x.BusinessPartnerId.ToString()
                                    })
                                    .OrderBy(o => o.Value)
                                    .ToList();
                                }
                                else
                                {
                                    StoreList = new List<SelectListItem> { new SelectListItem { Text = "No store available on this location", Value = "0" } };
                                }
                            }
                            else
                            {
                                StoreList = businessPartnerList.Select(x => new SelectListItem
                                {
                                    Text = x.Description + ", " + x.AddressLine1,
                                    Value = x.BusinessPartnerId.ToString()
                                })
                                .OrderBy(o => o.Value)
                                .ToList();
                            }
                        }
                        else
                        {
                            StoreList = new List<SelectListItem> { new SelectListItem { Text = "No store available on this location", Value = "0" } };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("Exchange", "GetStoreList", ex);
            }
            return Json(StoreList, JsonRequestBehavior.AllowGet);
        }
        #endregion

        //#region Ajax Method for Loading Store list from City and state
        //[HttpGet]
        //public JsonResult GetStoreList(string city, string pincode, int buid)
        //{
        //    _businessPartnerRepository = new BusinessPartnerRepository();           
        //    List<tblBusinessPartner> businessPartnerList = null;        
        //    List<SelectListItem> StoreList = null;
        //    try
        //    {
        //        if (city != null && pincode != null && buid > 0)
        //        {

        //            businessPartnerList = _businessPartnerRepository.GetList(x => x.IsActive == true
        //              && x.IsExchangeBP == true
        //              && !string.IsNullOrEmpty(x.AssociateCode)
        //                  && x.BusinessUnitId == buid
        //                  && x.cityId== Convert.ToInt32((city))
        //                  && x.Pincode.ToLower().Equals(pincode.ToLower())).ToList();

        //            if (businessPartnerList.Count > 0)
        //            {
        //                StoreList = businessPartnerList.Select(x => new SelectListItem
        //                {
        //                    Text = x.Description + ", " + x.AddressLine1,
        //                    Value = x.BusinessPartnerId.ToString()
        //                }).ToList();
        //                StoreList = StoreList.OrderBy(o => o.Value).ToList();
        //            }
        //            else
        //            {
        //                StoreList = new List<SelectListItem> { new SelectListItem { Text = "No store available on this location", Value = "0" } };
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LibLogging.WriteErrorToDB("Exchange", "GetStoreList", ex);
        //    }
        //    return Json(StoreList, JsonRequestBehavior.AllowGet);
        //}
        //#endregion

        #region GetAreaLocalityList Added by PJ
        [HttpGet]
        public JsonResult GetAreaLocalityList(string PinCode)
        {
            _businessPartnerRepository = new BusinessPartnerRepository();
            List<SelectListItem> AreaLocalityList = new List<SelectListItem>();
            List<tblAreaLocality> tblAreaLocalityList = null;

            try
            {
                DataTable dt = _businessPartnerRepository.GetAreaLocalityByPincode(PinCode);
                if (dt != null && dt.Rows.Count > 0)
                {
                    tblAreaLocalityList = GenericConversionHelper.DataTableToList<tblAreaLocality>(dt);
                }
                if (tblAreaLocalityList != null)
                {
                    AreaLocalityList = tblAreaLocalityList.Select(area => new SelectListItem() { Text = area.AreaLocality, Value = area.AreaId.ToString() }).ToList();
                    AreaLocalityList = AreaLocalityList.OrderBy(o => o.Text).ToList();
                    //var result = new SelectList(AreaLocalityList, "Value", "Text");
                }
                else
                {
                    AreaLocalityList.Add(new SelectListItem { Text = "No Area Locality Available", Value = "null" });
                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeController", "GetAreaLocalityByPincode", ex);
            }
            return Json(AreaLocalityList, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GetProdPrice Added by VK Date 19-July, For BU Based Validation on Sweetner
        /* [HttpGet]
         public JsonResult GetProdPrice(int productCatId, int productSubCatId, int brandId, int conditionId, int buid, bool IsValidationBasedSweetner, int modelno = 0, string formatType = "")
         {
             _masterManager = new BAL.SponsorsApiCall.MasterManager();
             string price = null;
             try
             {
                 if (productCatId > 0 && productSubCatId > 0)
                 {
                     price = _masterManager.GetProductPriceWithoutSweetner(productCatId, productSubCatId, brandId, conditionId, buid, modelno, formatType);
                 }
             }
             catch (Exception ex)
             {
                 LibLogging.WriteErrorToDB("ExchangeController", "GetProdPrice", ex);
             }
             return Json(price, JsonRequestBehavior.AllowGet);
         }
 */
        [HttpGet]
        public JsonResult GetProdPrice(int productCatId, int productSubCatId, int brandId, int conditionId, int buid, bool IsSweetnerModelBased, int newcatid = 0, int newsubcatid = 0, int modelno = 0, string formatType = "", bool IsValidationBasedSweetner = false)
        {
            _masterManager = new BAL.SponsorsApiCall.MasterManager();
            string price = null;
            try
            {
                if (newcatid > 0 && newsubcatid > 0 && productCatId > 0 && productSubCatId > 0 && IsSweetnerModelBased && !IsValidationBasedSweetner)
                {
                    price = _masterManager.GetProductPriceWithModelBasedSweetner(newcatid, newsubcatid, productCatId, productSubCatId, brandId, conditionId, buid, modelno, formatType);
                }
                else if (productCatId > 0 && productSubCatId > 0 && !IsSweetnerModelBased && !IsValidationBasedSweetner)
                {
                    price = _masterManager.GetProductPrice(productCatId, productSubCatId, brandId, conditionId, buid, formatType);
                }
                else if (productCatId > 0 && productSubCatId > 0 && IsValidationBasedSweetner)
                {
                    price = _masterManager.GetProductPriceWithoutSweetner(productCatId, productSubCatId, brandId, conditionId, buid, modelno, formatType);
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeController", "GetProdPrice", ex);
            }
            return Json(price, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region GetSweetnerPrice Added by VK Date 20-July, For BU Based Validation on Sweetner
        [HttpGet]
        public JsonResult GetSweetnerPrice(int newcatid, int newsubcatid, int buid, int modelno = 0, string formatType = "")
        {
            _masterManager = new BAL.SponsorsApiCall.MasterManager();
            string sweetnerPrice = null;
            try
            {
                if (newcatid > 0 && newsubcatid > 0)
                {
                    sweetnerPrice = _masterManager.GetSweetnerPrice(newcatid, newsubcatid, buid, modelno, formatType);
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeController", "GetSweetnerPrice", ex);
            }
            return Json(sweetnerPrice, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GetBUSweetnerValidationQuestions Added by VK Date 20-July, For BU Based Validation on Sweetner
        [HttpGet]
        public List<BUBasedSweetnerValidation> GetBUSweetnerValidationQuestions(int businessUnitId)
        {
            _masterManager = new BAL.SponsorsApiCall.MasterManager();
            List<BUBasedSweetnerValidation> bUBasedSweetnerValidationsList = null;
            try
            {
                if (businessUnitId > 0)
                {
                    bUBasedSweetnerValidationsList = _masterManager.GetBUSweetnerValidationQuestions(businessUnitId);
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeController", "GetBUSweetnerValidationQuestions", ex);
            }
            return bUBasedSweetnerValidationsList;
        }
        #endregion

        #region Product Details Old and New for Dropdowns
        #region  Get Product Category for Old
        [HttpGet]
        public JsonResult GetProductCategoryForOld(int buid, string priceCode = null, int? prodCatIdOld = 0, string newProdTypeDesc = null)
        {
            _productCategoryRepository = new ProductCategoryRepository();
            _productCategoryMappingRepository = new ProductCategoryMappingRepository();
            _priceMasterRepository = new PriceMasterRepository();
            List<SelectListItem> newProductCategory = new List<SelectListItem>();
            tblProductCategory tblProductCat = new tblProductCategory();
            try
            {
                //Old product category for exchange
                DataTable dtProductCat = _priceMasterRepository.GetProductCategoryByPriceCode(priceCode);
                if (buid > 0 && buid == Convert.ToInt32(BusinessUnitEnum.Relience) && priceCode != null && newProdTypeDesc != null && newProdTypeDesc != "")
                {
                    bool productTypeNew = newProdTypeDesc.Contains(EnumHelper.DescriptionAttr(ProductTypeNewEnum.BPL));
                    if (productTypeNew)
                    {
                        if (dtProductCat != null && dtProductCat.Rows.Count > 0)
                        {
                            var pricemaster = GenericConversionHelper.DataTableToList<tblPriceMaster>(dtProductCat);
                            if (pricemaster != null && pricemaster.Count > 0)
                            {
                                foreach (var productCat in pricemaster)
                                {
                                    tblProductCat = _productCategoryRepository.GetSingle(x => x.IsActive == true && x.IsAllowedForOld == true && x.Id == productCat.ProductCategoryId);
                                    if (tblProductCat != null && tblProductCat.Description == "Television")
                                    {
                                        if (tblProductCat.Id != prodCatIdOld)
                                        {
                                            newProductCategory.Add(new SelectListItem { Text = tblProductCat.Description, Value = tblProductCat.Id.ToString() });
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (dtProductCat != null && dtProductCat.Rows.Count > 0)
                        {
                            var pricemaster = GenericConversionHelper.DataTableToList<tblPriceMaster>(dtProductCat);
                            if (pricemaster != null && pricemaster.Count > 0)
                            {
                                foreach (var productCat in pricemaster)
                                {
                                    tblProductCat = _productCategoryRepository.GetSingle(x => x.IsActive == true && x.IsAllowedForOld == true && x.Id == productCat.ProductCategoryId);
                                    if (tblProductCat != null)
                                    {
                                        newProductCategory.Add(new SelectListItem { Text = tblProductCat.Description, Value = tblProductCat.Id.ToString() });
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("Exchange", "GetProductCategoryForOld", ex);
            }
            return Json(newProductCategory, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region  Get Product type for New
        [HttpGet]
        public JsonResult GetProductTypeForNew(int buid, int newProdCatId)
        {
            _productCategoryRepository = new ProductCategoryRepository();
            _productTypeRepository = new ProductTypeRepository();
            _productCategoryMappingRepository = new ProductCategoryMappingRepository();
            List<SelectListItem> newProductType = new List<SelectListItem>();
            List<tblBUProductCategoryMapping> BUmappingcategory = new List<tblBUProductCategoryMapping>();
            try
            {
                if (buid > 0)
                {
                    //new product categorgy list
                    List<tblProductType> prodTypeListForBosch = new List<tblProductType>();
                    List<tblBUProductCategoryMapping> productCategoryTypeForNew = new List<tblBUProductCategoryMapping>();
                    BUmappingcategory = _productCategoryMappingRepository.GetList(x => x.IsActive == true && x.ProductCatId == newProdCatId).ToList();
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
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("Exchange", "GetProductTypeForNew", ex);
            }

            return Json(newProductType, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region  Get Product Cat For New
        [HttpGet]
        public List<tblProductCategory> GetProductCatForNew(int buid)
        {
            _productCategoryRepository = new ProductCategoryRepository();
            _productCategoryMappingRepository = new ProductCategoryMappingRepository();
            List<tblProductCategory> tblProductCategoryList = new List<tblProductCategory>();
            List<tblBUProductCategoryMapping> tblBUProdCateMapList = new List<tblBUProductCategoryMapping>();
            try
            {
                if (buid > 0)
                {
                    DataTable dt = _productCategoryMappingRepository.GetNewProductCategory(buid);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        tblBUProdCateMapList = GenericConversionHelper.DataTableToList<tblBUProductCategoryMapping>(dt);
                    }
                    if (tblBUProdCateMapList != null && tblBUProdCateMapList.Count > 0)
                    {
                        foreach (var productCategory in tblBUProdCateMapList)
                        {
                            tblProductCategory productObj = _productCategoryRepository.GetSingle(x => x.IsActive == true
                            && x.Id == productCategory.ProductCatId && x.Id != Convert.ToInt32(ProductCatEnum.CookTop));
                            if (productObj != null)
                            {
                                tblProductCategoryList.Add(productObj);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("Exchange", "GetProductCatForNew", ex);
            }

            return tblProductCategoryList;
        }
        #endregion

        /* #region  Get Product Category for Old
         [HttpGet]
         public JsonResult GetProductCategoryForOld1(int buid, string priceCode = null, string newProdTypeDesc = null)
         {
             _productCategoryRepository = new ProductCategoryRepository();
             _productCategoryMappingRepository = new ProductCategoryMappingRepository();
             _priceMasterRepository = new PriceMasterRepository();
             List<SelectListItem> newProductCategory = new List<SelectListItem>();
             tblProductCategory tblProductCat = new tblProductCategory();
             tblBUProductCategoryMapping tblBUProductCategoryMapping = new tblBUProductCategoryMapping();
             tblProductCategory tblProductCat1 = new tblProductCategory();
             tblProductType tblProductType = new tblProductType();
             try
             {
                 //Old product category for exchange
                 if (buid > 0 && priceCode != null && newProdTypeDesc != null && newProdTypeDesc != "")
                 {
                     if (buid == Convert.ToInt32(BusinessUnitEnum.Relience))
                     {
                         bool productTypeNew = newProdTypeDesc.Contains(EnumHelper.DescriptionAttr(ProductTypeNewEnum.BPL));
                         if (productTypeNew)
                         {
                             newProdTypeDesc = newProdTypeDesc.ToLower();
                             tblProductType = _productTypeRepository.GetSingle(x => x.IsActive == true && ((x.Description).ToLower().Contains(newProdTypeDesc)));
                             if (tblProductType != null)
                             {
                                 tblBUProductCategoryMapping = _productCategoryMappingRepository.GetSingle(x => x.IsActive == true && x.ProductTypeId == tblProductType.Id && x.BusinessUnitId == buid);
                                 if (tblBUProductCategoryMapping != null)
                                 {
                                     DataTable dtProductCat = _priceMasterRepository.GetProductCategoryByPriceCode(priceCode);
                                     if (dtProductCat != null && dtProductCat.Rows.Count > 0)
                                     {
                                         var pricemaster = GenericConversionHelper.DataTableToList<tblPriceMaster>(dtProductCat);
                                         if (pricemaster != null && pricemaster.Count > 0)
                                         {
                                             foreach (var productCat in pricemaster)
                                             {
                                                 tblProductCat = _productCategoryRepository.GetSingle(x => x.IsActive == true && x.IsAllowedForOld == true && x.Id == productCat.ProductCategoryId);
                                                 if (tblProductCat != null && tblProductCat.Description == "Television")
                                                 {
                                                     newProductCategory.Add(new SelectListItem { Text = tblProductCat.Description, Value = tblProductCat.Id.ToString() });
                                                 }
                                             }
                                         }
                                     }
                                 }
                             }
                         }
                     }
                 }
             }
             catch (Exception ex)
             {
                 LibLogging.WriteErrorToDB("Exchange", "GetProductCategoryForOld", ex);
             }

             return Json(newProductCategory, JsonRequestBehavior.AllowGet);
         }
         #endregion*/
        #endregion

        #region New flow for exchange order

        #region select BP
        public ActionResult SelectBusinessPartner(ProductDetailsToExchange productDetails)
        {
            List<tblBusinessPartner> businessPartnerList = null;
            IList<tblCity> tblCities = null;
            tblBusinessPartner businessPartnerObj = new tblBusinessPartner();
            _businessPartnerRepository = new BusinessPartnerRepository();
            _businessUnitRepository = new BusinessUnitRepository();
            ExchagneViewModel ExchangeObj = new ExchagneViewModel();
            tblBusinessUnit BusinessUnitObj = new tblBusinessUnit();

            try
            {
                ExchangeObj.CityList = new List<SelectListItem>();
                ExchangeObj.PincodeList = new List<SelectListItem>();
                ExchangeObj.BUId = productDetails.BusinessUnitId;
                if (productDetails != null)
                {
                    if (productDetails.BULogoName != null)
                    {
                        ExchangeObj.BULogoName = productDetails.BULogoName;
                    }
                    else
                    {
                        BusinessUnitObj = _businessUnitRepository.GetSingle(x => x.BusinessUnitId == productDetails.BusinessUnitId);
                        if (BusinessUnitObj != null)
                        {
                            ExchangeObj.IsBPAssociated = BusinessUnitObj.IsBPAssociated != null ? BusinessUnitObj.IsBPAssociated : false;
                            ExchangeObj.BULogoName = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Content/DB_Files/SponsorLogo/" + BusinessUnitObj.LogoName;
                        }
                    }
                    //get state list
                    DataTable dt = _businessPartnerRepository.GetStateList();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        businessPartnerList = GenericConversionHelper.DataTableToList<tblBusinessPartner>(dt);
                        List<string> states = businessPartnerList.Where(x => x.BusinessUnitId == productDetails.BusinessUnitId && (x.IsExchangeBP != null && x.IsExchangeBP == true)).OrderBy(o => o.State).Select(x => x.State).Distinct().ToList();
                        List<SelectListItem> stateListItems = states.Select(x => new SelectListItem
                        {
                            Text = x,
                            Value = x
                        }).ToList();
                        ViewBag.StateList = new SelectList(stateListItems, "Text", "Text");
                    }
                    // GET METRO CITY
                    DataTable citydt = _businessPartnerRepository.GetCityListforExchange(ExchangeObj.BUId);
                    if (citydt != null && citydt.Rows.Count > 0)
                    {
                        tblCities = GenericConversionHelper.DataTableToList<tblCity>(citydt);
                        foreach (var item in tblCities)
                        {
                            if (item.isMetro == true)
                            {

                                item.cityLogo = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Content/assets/img/cities/" + item.cityLogo;
                            }
                        }
                        //businessPartnerList = GenericConversionHelper.DataTableToList<tblBusinessPartner>(citydt);
                        List<SelectListItem> citilist = tblCities.Select(x => new SelectListItem
                        {
                            Text = x.ToString(),
                            Value = x.ToString()
                        }).ToList();
                        ExchangeObj.CityList = citilist;


                        ExchangeObj.metroCities = GenericMapper<tblCity, CityViewModalforMetroCities>.MapList(tblCities);
                    }

                    //Setting Values from  productdetails model
                    ExchangeObj.OldProductCategoryId = productDetails.OldProductCatId;
                    ExchangeObj.oldProductypeId = productDetails.OldTypeId;
                    ExchangeObj.OldBrandId = productDetails.BrandId;
                    ExchangeObj.NewBrandId = productDetails.NewBrandId;
                    ExchangeObj.NewCategoryId = productDetails.NewProductCategoryId;
                    ExchangeObj.NewTypeId = productDetails.NewProductTypeId;
                    ExchangeObj.ProductAge = productDetails.ProductAge;
                    ExchangeObj.IsSweetnerModelBased = productDetails.IsSweetnerModelBased;
                    ExchangeObj.QualityCheckValue = productDetails.QualityCheck;
                    ExchangeObj.FormatName = productDetails.FormatName;
                    ExchangeObj.ExchangePrice = productDetails.ExchangePrice;
                    //Store List For BusinessPartner list
                    ExchangeObj.StoreList = new List<SelectListItem>();
                    ExchangeObj.ProductModelIdNew = productDetails.ProductModelIdNew;
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeController", "SelectBusinessPartner", ex);
            }

            return View(ExchangeObj);
        }

        [HttpPost]
        public ActionResult SelectBusinessPartner(ExchagneViewModel ExchangeObj)
        {
            try
            {
                //Redirecting to exchReg With Required Data
                if (ModelState.IsValid)
                {
                    return RedirectToAction("ProductDetailsToExchange", "Exchange", new { BUId = ExchangeObj.BUId, StateName = ExchangeObj.StateName, CityName = ExchangeObj.CityName, FormatName = ExchangeObj.FormatName, ZipCode = ExchangeObj.ZipCode, ProductAge = ExchangeObj.ProductAge, BusinessPartnerId = ExchangeObj.BusinessPartnerId });
                }
                return RedirectToAction("ProductDetailsToExchange", "Exchange", new { BUId = ExchangeObj.BUId, StateName = ExchangeObj.StateName, CityName = ExchangeObj.CityName, FormatName = ExchangeObj.FormatName, ZipCode = ExchangeObj.ZipCode, ProductAge = ExchangeObj.ProductAge, BusinessPartnerId = ExchangeObj.BusinessPartnerId});

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeController", "SelectBusinessPartner", ex);
            }

            return View(ExchangeObj);
        }

        #endregion

        #region GET GetPincode By CityId for Metro CITIES CONFIGURATION
        public JsonResult PincodeByCityIdBUId(string pintext, int buid, int cityId)
        {
            _businessPartnerRepository = new BusinessPartnerRepository();
            IEnumerable<SelectListItem> pincodeList = null;
            List<tblBusinessPartner> businessPartnerList = null;
            List<tblPinCode> tblPinCodesList = null;

            try
           {
                
                DataTable dt = _businessPartnerRepository.GetPincodeByCityIdBUId(buid, cityId, pintext);
                tblPinCodesList = GenericConversionHelper.DataTableToList<tblPinCode>(dt);
                if (tblPinCodesList.Count > 0)
                {
                    pincodeList = tblPinCodesList.Select(x => new SelectListItem
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
                LibLogging.WriteErrorToDB("BoschExchangeController", "GetPincodeByCityId", ex);
            }
            var result = new SelectList(pincodeList, "Value", "Text");
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region gET AUTOCOMPLETE CITYLIST
        public JsonResult GetCityForAutoComplete(string pintext, int buid)
        {
            _businessPartnerRepository = new BusinessPartnerRepository();
            IEnumerable<SelectListItem> cityList = null;
            List<tblCity> tblCities = new List<tblCity>();
            try
            {
                pintext = pintext.ToLower();
                DataTable citydt = _businessPartnerRepository.GetCityListforExchange(buid);
                if (citydt != null && citydt.Rows.Count > 0)
                {
                    tblCities = GenericConversionHelper.DataTableToList<tblCity>(citydt);
                }
                cityList = (tblCities).AsEnumerable().Select(prodt => new SelectListItem() { Text = prodt.Name, Value = prodt.CityId.ToString() });
                cityList = cityList.OrderBy(o => o.Value).ToList();
                cityList = cityList.Where(x => x.Text.ToLower().Contains(pintext)).ToList();
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("BoschExchangeController", "GetPincodeByCityId", ex);
            }
            var result = new SelectList(cityList, "Value", "Text");
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region get product type by pricemasternameid
        [HttpGet]
        public JsonResult GetProdTypeByPriceMasterNameIdAndCatid(int proCatId, int priceMasterNameID)
        {
            #region Variable's declaration
            OldProductDetailsManager oldProductDetailsManager = new OldProductDetailsManager();
            List<tblProductType> prodsubGroupListForExchange = new List<tblProductType>();
            SelectList result = null;
            List<SelectListItem> prodType = null;
            //_productTypeRepository = new ProductTypeRepository();
            //_priceMasterRepository = new PriceMasterRepository();
            //List<tblPriceMaster> pricemaster = null;
            //tblProductType typeObj = null;
            #endregion

            try
            {
                if (priceMasterNameID > 0 && proCatId > 0)
                {
                    prodsubGroupListForExchange = oldProductDetailsManager.GetProTypeListByPriceMasterNameId(priceMasterNameID, proCatId);
                    prodType = new List<SelectListItem>();

                    foreach (tblProductType item in prodsubGroupListForExchange)
                    {
                        if (item != null)
                        {
                            if (item.Size != null)
                            {
                                prodType.Add(new SelectListItem() { Text = item.Description + "(" + item.Size + ")", Value = item.Id.ToString() });
                            }
                            else
                            {
                                prodType.Add(new SelectListItem() { Text = item.Description, Value = item.Id.ToString() });
                            }
                        }
                    }

                }
                prodType = prodType.OrderBy(o => o.Text).ToList();
                result = new SelectList(prodType, "Value", "Text");

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeController", "GetProdTypeByPriceMasterNameIdAndCatid", ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region get brands for old by pricemastername id,catid,typeid
        [HttpGet]
        public JsonResult GetBrandForOldByPriceMasterNameId(int productCatId, int buid, int typeId, int priceMasterNameId)
        {
            _masterManager = new BAL.SponsorsApiCall.MasterManager();
            OldProductDetailsManager oldProductDetailsManager = new OldProductDetailsManager();
            _businessUnitRepository = new BusinessUnitRepository();
            List<SelectListItem> BrandList = null;
            List<SelectListItem> BrandListFinal = null;
            ProductDetailsForOldDataContract productDetailsForOldDataContract = new ProductDetailsForOldDataContract();
            try
            {
                //BrandList = _masterManager.GetBrandForExchangeByCategoryId(productCatId, buid, typeId).Brand.Select(prodt => new SelectListItem() { Text = prodt.Name, Value = prodt.Id.ToString() }).ToList();
                productDetailsForOldDataContract.OldProductcategoryId = productCatId;
                productDetailsForOldDataContract.OldProductTypeId = typeId;
                productDetailsForOldDataContract.PriceMasterNameId = priceMasterNameId;
                BrandList = oldProductDetailsManager.GetBrandOldByPriceMasterId(productDetailsForOldDataContract).Select(prodt => new SelectListItem() { Text = prodt.Name, Value = prodt.Id.ToString() }).ToList();
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
                LibLogging.WriteErrorToDB("ExchangeController", "GetBrandForOldByPriceMasterNameId", ex);
            }

            return Json(BrandListFinal, JsonRequestBehavior.AllowGet);
            //return Json(BrandList, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region get model number for new 
        [HttpGet]
        public JsonResult GetModelsforNewProduct(int ProdTypeId, int buid, int newcatid, int bpid, int newBrandId)
        {
            //reference to GetModelNumberByProdTypeId
            BAL.SponsorsApiCall.MasterManager masterManager = new BAL.SponsorsApiCall.MasterManager();
            ModelNumberRepository _modelNumberRepository = new ModelNumberRepository();
            _productCategoryMappingRepository = new ProductCategoryMappingRepository();
            //List<tblModelNumber> modelNoDC = null;
            List<SelectListItem> modelNumber = new List<SelectListItem>();
            var result = new SelectList(modelNumber, "Value", "Text");
            try
            {
                ModelDetailsDataContract modelDetailsDataContract = new ModelDetailsDataContract();
                if (buid > 0 && bpid > 0 && newcatid > 0 && ProdTypeId > 0 && newBrandId > 0)
                {
                    modelDetailsDataContract = masterManager.GetModelList(buid, bpid, newcatid, ProdTypeId, newBrandId);
                    if (modelDetailsDataContract != null && modelDetailsDataContract.ModelList != null && modelDetailsDataContract.ModelList.Count > 0)
                    {
                        modelNumber = modelDetailsDataContract.ModelList.Select(projt => new SelectListItem() { Text = projt.ModelName, Value = projt.ModelNumberId.ToString() }).ToList();
                    }
                    else
                    {
                        modelNumber.Add(new SelectListItem { Text = "No Model Available", Value = "0" });
                    }
                }
                else
                {
                    modelNumber.Add(new SelectListItem { Text = "No Model Available", Value = "0" });
                }
                /*var*/
                result = new SelectList(modelNumber, "Value", "Text");

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeController", "GetModelsforNewProduct", ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region get product GetProductConditonLabels
        [HttpGet]
        public JsonResult GetProductConditonLabels(int productCatId, int buid, int bpid)
        {
            _productConditionLabelRepository = new ProductConditionLabelRepository();
            List<SelectListItem> prodType = null;

            try
            {
                if (productCatId > 0 && buid > 0 && bpid > 0)
                {
                    List<tblProductConditionLabel> ConditionLabelObj = _productConditionLabelRepository.GetList(x => x.BusinessUnitId == buid && x.BusinessPartnerId == bpid && x.ProductCatId == productCatId && x.IsActive==true).ToList();

                    if (ConditionLabelObj.Any())
                    {
                        prodType = ConditionLabelObj.Select(x => new SelectListItem
                        {
                            Text = x.PCLabelName,
                            Value = x.OrderSequence.ToString()
                        }).OrderByDescending(o => o.Value).ToList();
                    }
                    else
                    {
                        List<tblProductConditionLabel> ConditionLabelObj1 = _productConditionLabelRepository.GetList(x => x.BusinessUnitId == buid && x.BusinessPartnerId == bpid && x.ProductCatId == null && x.IsActive == true).ToList();
                        prodType = ConditionLabelObj1.Select(x => new SelectListItem
                        {
                            Text = x.PCLabelName,
                            Value = x.OrderSequence.ToString()
                        }).OrderByDescending(o => o.Value).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeController", "GetProductConditonLabels", ex);
                // Log the error, and return an appropriate response to the client-side code
                return Json(new { error = "An error occurred while processing the request." }, JsonRequestBehavior.AllowGet);
            }

            // Return the JSON result
            return Json(prodType, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region get product price with sweetner
        [HttpGet]
        public JsonResult GetPriceOnBasisofNewPriceMaster(int productCatId, int productSubCatId, int brandId, int conditionId, int buid, bool IsSweetnerModelBased, int newcatid = 0, int newsubcatid = 0, int modelno = 0, string formatType = "", bool IsValidationBasedSweetner = false, int newBrandId = 0, int priceNameId = 0, int bpid = 0)
        {
            _masterManager = new BAL.SponsorsApiCall.MasterManager();
            _productConditionLabelRepository = new ProductConditionLabelRepository();
            //string price = null;
            //decimal Convertprice = 0;
            ManageSweetener manageSweetener = new ManageSweetener();
            SweetenerDataContract sweetenerDataContract = new SweetenerDataContract();
            GetSweetenerDetailsDataContract getSweetenerDetailsDataContract = new GetSweetenerDetailsDataContract();
            BAL.ExchangePriceMaster.PriceMasterManager priceMasterManager = new BAL.ExchangePriceMaster.PriceMasterManager();
            try
            {
                #region get product price
                //request obj
                ProductPriceDetailsDataContract productPriceDetailsDataContract = new ProductPriceDetailsDataContract();
                //response obj
                UniversalPriceMasterDataContract universalPriceMasterDataContract = new UniversalPriceMasterDataContract();
                if (buid > 0 && bpid > 0)
                {
                    productPriceDetailsDataContract.BusinessUnitId = buid;
                    productPriceDetailsDataContract.BusinessPartnerId = bpid;
                    productPriceDetailsDataContract.NewBrandId = newBrandId;
                    productPriceDetailsDataContract.ProductCatId = productCatId;
                    productPriceDetailsDataContract.ProductTypeId = productSubCatId;
                    productPriceDetailsDataContract.BrandId = brandId;           //old brand id
                    productPriceDetailsDataContract.conditionId = conditionId;
                    productPriceDetailsDataContract.PriceNameId = priceNameId;
                    if (productPriceDetailsDataContract != null)
                    {
                        universalPriceMasterDataContract = priceMasterManager.GetProductPrice(productPriceDetailsDataContract);
                    }
                    else
                    {
                        universalPriceMasterDataContract.BaseValue = 0;
                    }
                }
                else
                {
                    universalPriceMasterDataContract.BaseValue = 0;
                }


                #endregion

                #region call sweetner

                //check isSweetnerApplicable or not
                if (buid > 0 && bpid > 0)
                {
                    tblProductConditionLabel tblProductConditionLabel = _productConditionLabelRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == buid && x.BusinessPartnerId == bpid && x.OrderSequence == conditionId);

                    if (tblProductConditionLabel != null)
                    {
                        getSweetenerDetailsDataContract.BusinessUnitId = buid;
                        getSweetenerDetailsDataContract.BusinessPartnerId = bpid;
                        getSweetenerDetailsDataContract.NewProdCatId = newcatid;
                        getSweetenerDetailsDataContract.NewProdTypeId = newsubcatid;
                        getSweetenerDetailsDataContract.BrandId = brandId;
                        getSweetenerDetailsDataContract.ModalId = modelno;
                        getSweetenerDetailsDataContract.IsSweetenerModalBased = IsSweetnerModelBased;

                        if (tblProductConditionLabel.IsSweetenerApplicable == true)
                        {
                            sweetenerDataContract = manageSweetener.GetSweetenerAmtExchange(getSweetenerDetailsDataContract);
                        }
                        else
                        {
                            sweetenerDataContract.SweetenerBu = 0;
                            sweetenerDataContract.SweetenerBP = 0;
                            sweetenerDataContract.SweetenerDigi2L = 0;
                            sweetenerDataContract.SweetenerTotal = 0;
                        }

                    }
                    else
                    {
                        sweetenerDataContract.SweetenerBu = 0;
                        sweetenerDataContract.SweetenerBP = 0;
                        sweetenerDataContract.SweetenerDigi2L = 0;
                        sweetenerDataContract.SweetenerTotal = 0;
                    }
                }
                else
                {
                    sweetenerDataContract.SweetenerBu = 0;
                    sweetenerDataContract.SweetenerBP = 0;
                    sweetenerDataContract.SweetenerDigi2L = 0;
                    sweetenerDataContract.SweetenerTotal = 0;
                }

                #endregion

                #region Condition for concat price with sweetner
                if (IsValidationBasedSweetner)
                {
                    sweetenerDataContract.BaseValue = universalPriceMasterDataContract.BaseValue != null ? universalPriceMasterDataContract.BaseValue : 0;
                    sweetenerDataContract.ExchangePrice = universalPriceMasterDataContract.BaseValue != null ? universalPriceMasterDataContract.BaseValue : 0;
                }
                else
                {
                    if (sweetenerDataContract.SweetenerTotal > 0)
                    {
                        sweetenerDataContract.BaseValue = universalPriceMasterDataContract.BaseValue;
                        sweetenerDataContract.ExchangePrice = universalPriceMasterDataContract.BaseValue + sweetenerDataContract.SweetenerTotal;
                        sweetenerDataContract.SweetenerTotal = sweetenerDataContract.SweetenerTotal != null ? sweetenerDataContract.SweetenerTotal : 0;
                        sweetenerDataContract.SweetenerBP = sweetenerDataContract.SweetenerBP != null ? sweetenerDataContract.SweetenerBP : 0;
                        sweetenerDataContract.SweetenerBu = sweetenerDataContract.SweetenerBu != null ? sweetenerDataContract.SweetenerBu : 0;
                        sweetenerDataContract.SweetenerDigi2L = sweetenerDataContract.SweetenerDigi2L != null ? sweetenerDataContract.SweetenerDigi2L : 0;
                    }
                    else if (universalPriceMasterDataContract != null && universalPriceMasterDataContract.BaseValue > 0)
                    {
                        sweetenerDataContract.BaseValue = universalPriceMasterDataContract.BaseValue;
                        sweetenerDataContract.ExchangePrice = universalPriceMasterDataContract.BaseValue;
                        sweetenerDataContract.SweetenerTotal = 0;
                        sweetenerDataContract.SweetenerBP = 0;
                        sweetenerDataContract.SweetenerBu = 0;
                        sweetenerDataContract.SweetenerDigi2L = 0;
                    }
                    else
                    {
                        sweetenerDataContract.BaseValue = 0;
                        sweetenerDataContract.SweetenerTotal = 0;
                        sweetenerDataContract.SweetenerBP = 0;
                        sweetenerDataContract.SweetenerBu = 0;
                        sweetenerDataContract.SweetenerDigi2L = 0;
                    }

                }
                #endregion

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeController", "GetProdPrice", ex);
            }
            return Json(sweetenerDataContract, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region get Coupon value after verify coupon
        [HttpGet]
        public JsonResult GetCouponValueWithVerification(int BusinessUnitid, int BusinessPartnerid, string CouponCode)
        {
            DataTable dataTable = new DataTable();
            _couponRepository = new CouponRepository();
            Decimal Value=0;
            string coupon="";
            int status=0;
            int CouponId = 0;
            dataTable =_couponRepository.CouponVerification(BusinessUnitid, BusinessPartnerid, CouponCode);
            if (dataTable.Columns.Count == 1)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    status = Convert.ToInt32(row["StatusValue"]); 
                }
            }
            else
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    CouponId= Convert.ToInt32(row["CouponId"]);
                    Value = Convert.ToDecimal(row["CouponValue"]); 
                    coupon = row["CouponCode"].ToString(); 
                    status = Convert.ToInt32(row["StatusValue"]);

                }
            }
            // Construct the anonymous object with the properties
            var jsonObject = new
            {
                CouponId= CouponId,
                CouponValue = Value,
                Coupon = coupon,
                Status = status
            };
            return Json(jsonObject, JsonRequestBehavior.AllowGet);
        }

        }
        #endregion
}