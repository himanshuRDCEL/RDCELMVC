
using GraspCorn.Common.Constant;
using GraspCorn.Common.Enums;
using GraspCorn.Common.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Security;
using RDCEL.DocUpload.BAL.Common;
using RDCEL.DocUpload.BAL.Manager;
using RDCEL.DocUpload.BAL.ServiceCall;
using RDCEL.DocUpload.BAL.SponsorsApiCall;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DataContract.ABBRegistration;
using RDCEL.DocUpload.Web.API.Helpers;
using RDCEL.DocUpload.DataContract.ABBRedemption;
using RDCEL.DocUpload.DataContract.Common;
using RDCEL.DocUpload.DataContract.SponsorModel;
using RDCEL.DocUpload.DataContract.Voucher;

namespace RDCEL.DocUpload.Web.API.Controllers
{
    public class VoucherController : Controller
    {
        private ExchangeOrderRepository _exchangeOrderRepository;
        private CustomerDetailsRepository _customerDetailRepository;
        private ProductCategoryRepository _productCategoryRepository;
        ExchangeOrderManager _exchangeOrderManager;
        BusinessPartnerRepository _businessPartnerRepository;
        BusinessUnitRepository _businessUnitRepository;
        TermsAndConditionsForVoucherRepository _tandcRepository;
        BAL.SponsorsApiCall.MasterManager _masterManager;
        ProductCategoryMappingRepository _productCategoryMappingRepository;
        NotificationManager _notificationManager;
        BusinessPartnerManager _businessPartnerManager;
        BrandRepository _brandRepository;
        ABBOrderMaanger _abbOrderManager;
        ProductConditionLabelRepository _productConditionLabelRepository;
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
                LibLogging.WriteErrorToDB("VoucherController", "Details", ex);
            }
            return View();
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
                    return RedirectToAction("VoucherVerification", "Voucher", new { buid = businessPartnerVM.BusinessUnitId });
                }
                else
                {
                    TempData["Auth"] = false;
                    return RedirectToAction("Login", "Voucher", new { Area = "" });
                }
            }
            else
            {
                TempData["Auth"] = false;
                return RedirectToAction("Login", "Voucher", new { Area = "" });
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

        #endregion

        #region Voucher Verification

        public ActionResult VoucherVerification(int buid)
        {
            _productCategoryRepository = new ProductCategoryRepository();
            _exchangeOrderManager = new BAL.SponsorsApiCall.ExchangeOrderManager();
            _masterManager = new BAL.SponsorsApiCall.MasterManager();
            _businessPartnerRepository = new BusinessPartnerRepository();
            _businessUnitRepository = new BusinessUnitRepository();
            _exchangeOrderManager = new ExchangeOrderManager();
            _productCategoryMappingRepository = new ProductCategoryMappingRepository();
            List<tblBusinessPartner> businessPartnerList = null;
            VoucherDataContract voucherDC = new VoucherDataContract();
            ExchangeOrderDataContract exchangeDataContract = new ExchangeOrderDataContract();
            List<SelectListItem> StoreList = null;
            List<SelectListItem> categoryList = null;
            tblBusinessUnit BusinessUnitObj = null;
            _brandRepository = new BrandRepository();
            try
            {
                if (ValidateBPLogin())
                {
                    SessionHelper _sessionHelper = (SessionHelper)Session["User"];
                    ViewBag.LoginUser = _sessionHelper.LoggedUserInfo;
                    ViewBag.IsDealer = _sessionHelper.LoggedUserInfo.IsDealer;
                    ViewBag.LoginEmail = _sessionHelper.LoggedUserInfo.Email;

                    #region Set Voucher Data Contact
                    voucherDC.isDealer =Convert.ToBoolean(_sessionHelper.LoggedUserInfo.IsDealer);
                    //Product Category List
                    List<tblProductCategory> prodGroupListForExchnage = _productCategoryRepository.GetList(x => x.IsActive == true).ToList();
                    prodGroupListForExchnage = prodGroupListForExchnage.OrderBy(o => o.Description_For_ABB).ToList();
                    if (prodGroupListForExchnage != null && prodGroupListForExchnage.Count > 0)
                    {
                        ViewBag.ProductCategoryList = new SelectList(prodGroupListForExchnage, "Id", "Description");
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


                    exchangeDataContract.BusinessUnitDataContract = _exchangeOrderManager.GetBUById(buid);

                    if (exchangeDataContract.BusinessUnitDataContract != null)
                    {
                        if (exchangeDataContract.BusinessUnitDataContract.LogoName != null)
                        {
                            exchangeDataContract.BULogoName = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Content/DB_Files/SponsorLogo/" + exchangeDataContract.BusinessUnitDataContract.LogoName;
                        }
                        exchangeDataContract.ExpectedDeliveryHours = exchangeDataContract.BusinessUnitDataContract.ExpectedDeliveryHours;
                    }
                    //exchangeDataContract.BrandList =  new SelectList(_masterManager.GetBrandForExchange().Brand, "Id", "Name");

                    if (buid > 0)
                    {
                        //store details

                        businessPartnerList = _businessPartnerRepository.GetList(x => x.IsActive == true
                            && x.BusinessUnitId == buid).ToList();
                        tblBusinessPartner businessPartner = businessPartnerList.FirstOrDefault(x => x.BusinessUnitId == buid
                                  && (x.Email != null && x.Email.ToLower().Equals(_sessionHelper.LoggedUserInfo.Email.ToLower())));
                        ViewBag.BusinesPartnerCode = businessPartner != null ? businessPartner.StoreCode.Trim() : string.Empty;

                        StoreList = businessPartnerList.Where(x => x.BusinessPartnerId == _sessionHelper.LoggedUserInfo.BusinessPartnerId).Select(x => new SelectListItem
                        {
                            Text = x.Description,
                            Value = x.BusinessPartnerId.ToString()
                        }).ToList();

                        exchangeDataContract.AssociateCode = !string.IsNullOrWhiteSpace(businessPartner.AssociateCode) ? businessPartner.AssociateCode : string.Empty;
                        exchangeDataContract.StoreList = StoreList.OrderBy(o => o.Text).ToList();

                        //state city
                        exchangeDataContract.CityList = new List<SelectListItem>();
                        DataTable dtb = _businessPartnerRepository.GetStateList();
                        if (dtb != null && dtb.Rows.Count > 0)
                        {
                            businessPartnerList = GenericConversionHelper.DataTableToList<tblBusinessPartner>(dtb);
                            businessPartnerList = businessPartnerList.Where(x => x.BusinessUnitId == buid
                            && (x.IsExchangeBP != null && x.IsExchangeBP == true)
                            && (x.Email != null && x.Email.ToLower().Equals(_sessionHelper.LoggedUserInfo.Email.ToLower()))).ToList();

                            List<string> states = businessPartnerList.OrderBy(o => o.City)
                                                                    .Select(x => x.State.Trim())
                                                                    .Distinct().ToList();
                            List<SelectListItem> stateListItems = states.Select(x => new SelectListItem
                            {
                                Text = x,
                                Value = x
                            }).ToList();
                            ViewBag.StateList = new SelectList(stateListItems, "Text", "Text");


                        }


                        ////BU details
                        BusinessUnitObj = _businessUnitRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == buid);
                        if (BusinessUnitObj != null)
                        {
                            if (BusinessUnitObj.LogoName != null)
                            {
                                exchangeDataContract.BULogoName = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Content/DB_Files/SponsorLogo/" + BusinessUnitObj.LogoName;
                            }
                            exchangeDataContract.IsSweetnerModelBased = Convert.ToBoolean(BusinessUnitObj.IsSweetnerModelBased);
                            exchangeDataContract.CompanyName = BusinessUnitObj.Name;
                            exchangeDataContract.BUName = BusinessUnitObj.Name;
                            exchangeDataContract.BusinessUnitId = buid;
                            exchangeDataContract.ZohoSponsorNumber = BusinessUnitObj.ZohoSponsorId;
                            exchangeDataContract.BusinessUnitDataContract.IsBUMultiBrand =Convert.ToBoolean(BusinessUnitObj.IsBUMultiBrand);
                            voucherDC.IsBuMultiBrand= Convert.ToBoolean(BusinessUnitObj.IsBUMultiBrand);
                        }
                    }

                    voucherDC.ExchangeOrderDataContract = exchangeDataContract;

                    //new product categorgy list

                    List<tblProductCategory> prodGroupListForBosch = new List<tblProductCategory>();
                    List<tblBUProductCategoryMapping> productCategoryForNew = new List<tblBUProductCategoryMapping>();

                    DataTable dt = _productCategoryMappingRepository.GetNewProductCategory(buid);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        productCategoryForNew = GenericConversionHelper.DataTableToList<tblBUProductCategoryMapping>(dt);
                    }
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
                    else
                    {
                        categoryList = new List<SelectListItem> { new SelectListItem { Text = "No Model Available", Value = "0" } };

                    }
                    if (prodGroupListForBosch != null && prodGroupListForBosch.Count > 0)
                    {
                        ViewBag.ProductCategoryListNew = new SelectList(prodGroupListForBosch, "Id", "Description_For_ABB");
                    }
                    else
                    {
                        ViewBag.ProductCategoryListNew = new SelectList(categoryList, "Id", "Description_For_ABB");
                    }
                    voucherDC.ProductTypeList = new List<SelectListItem>();
                    voucherDC.ProductModelList = new List<SelectListItem>();
                    voucherDC.BrandList = new List<SelectListItem>();
                    voucherDC.BusinessUnitId = BusinessUnitObj.BusinessUnitId;
                    if (BusinessUnitObj.IsBUMultiBrand == false)
                    {
                        tblBrand brandObj = _brandRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId==BusinessUnitObj.BusinessUnitId);
                        if (brandObj != null)
                        {
                            if (brandObj.Name != null)
                            {
                                voucherDC.BrandName = brandObj.Name;
                                voucherDC.NewBrandId = brandObj.Id;
                            }
                        }
                    }
                    else
                    {
                        List<tblBrand> brandsList = _brandRepository.GetList(x => x.IsActive == true).ToList();
                        brandsList = brandsList.OrderBy(o => o.Name).ToList();
                        if (brandsList != null && brandsList.Count > 0)
                        {
                            ViewBag.Brand = new SelectList(brandsList, "Id", "Name");
                        }
                    }
                    
                    #endregion
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("Voucher", "VoucherVerification", ex);
                return View(voucherDC);
            }

            return View(voucherDC);
        }


        [HttpPost]
        public ActionResult VoucherVerification(VoucherDataContract voucherData)
        {
            string msg = string.Empty;
            int result = 0;
            DateTime _dateTime = DateTime.Now;
            _exchangeOrderManager = new ExchangeOrderManager();
            _brandRepository = new BrandRepository();
            _businessPartnerRepository = new BusinessPartnerRepository();
            tblVoucherVerfication tblVoucher = new tblVoucherVerfication();
            VoucherVerificationRepository _verificationRepository = new VoucherVerificationRepository();
            string fileName = null;
            try
            {
                SessionHelper _sessionHelper = (SessionHelper)Session["User"];
                var isBPDealer = _sessionHelper.LoggedUserInfo.IsDealer;
                var email = _sessionHelper.LoggedUserInfo.Email;
                var bpid=_sessionHelper.LoggedUserInfo.BusinessPartnerId;
                if (isBPDealer == false && voucherData.BusinessPartnerId==Convert.ToInt32(BusinessPartnerDiscriptionEnum.OtherBP))
                {
                    tblBusinessPartner businessPartnerobj = _businessPartnerRepository.GetSingle(x => x.BusinessPartnerId== bpid&& x.AssociateCode == voucherData.ExchangeOrderDataContract.AssociateCode);
                    if (businessPartnerobj != null)
                    {
                        voucherData.BusinessPartnerId = businessPartnerobj.BusinessPartnerId;
                    }
                }
                if (voucherData.Base64StringValue != null)
                {
                    byte[] bytes = System.Convert.FromBase64String(voucherData.Base64StringValue);
                    fileName = _dateTime.ToString("yyyyMMddHHmmssFFF") + Path.GetExtension("image.jpeg");
                    string rootPath = @HostingEnvironment.ApplicationPhysicalPath;
                    string filePath = ConfigurationManager.AppSettings["ExchangeInvoiceImage"].ToString() + fileName;
                    System.IO.File.WriteAllBytes(rootPath + filePath, bytes);
                }
                else
                {
                    msg = "Invoice image is not proper process again.";
                    TempData["Msg"] = msg;
                    return RedirectToAction("Details");
                }

                if (voucherData != null && voucherData.ModelNumberId != null)
                {
                    if (voucherData.ExchangeOrderDataContract.BusinessUnitDataContract.IsBUMultiBrand == true || voucherData.ExchangeOrderDataContract.BusinessUnitDataContract.BusinessUnitId == Convert.ToInt32(BusinessUnitEnum.Diakin))
                    {
                        voucherData.ModelNumberId = null;
                        //Set Brand Id
                        if (!string.IsNullOrEmpty(voucherData.BrandName))
                        {
                            tblBrand brand = _brandRepository.GetSingle(x => x.IsActive == true && x.Name == voucherData.BrandName);
                            if (brand != null)
                                voucherData.NewBrandId = brand.Id;
                        }
                    }
                }

                //if (InvoiceImage != null)
                //{
                //    fileName = _dateTime.ToString("yyyyMMddHHmmssFFF") + Path.GetExtension(InvoiceImage.FileName);
                //    string filePath = ConfigurationManager.AppSettings["ExchangeInvoiceImage"].ToString() + fileName;
                //    InvoiceImage.SaveAs(filePath);
                //}

                if (voucherData != null && voucherData.ExchangeOrderDataContract != null)
                {
                    voucherData.ExchangeOrderId = voucherData.ExchangeOrderDataContract.Id;
                    voucherData.CustomerId = voucherData.ExchangeOrderDataContract.CustomerDetailsId;
                    voucherData.VoucherCode = voucherData.VoucherCode;
                    voucherData.IsVoucherused = true;
                    voucherData.InvoiceImageName = fileName;
                    result = _exchangeOrderManager.AddVouchertoDB(voucherData);
                    if (result > 0)
                    {
                        msg = "Thank You, your voucher has been redeemed successfully.";
                    }
                    else
                    {
                        msg = "Some error occurred, please connect with the Administrator.";
                    }
                    TempData["Msg"] = msg;
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("Voucher", "VoucherVerification", ex);
                return RedirectToAction("Details");
            }
            return RedirectToAction("Details");
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

        #region On Page Ajax Call Methods

        public JsonResult GetExchangeOrderByVoucherCode(string voucherCode, string phoneNumber)
        {
            _businessUnitRepository = new BusinessUnitRepository();
            _exchangeOrderManager = new ExchangeOrderManager();
            _customerDetailRepository = new CustomerDetailsRepository();
            _businessPartnerRepository = new BusinessPartnerRepository();
            ExchangeOrderDataContract exchangeOrderData = new ExchangeOrderDataContract();
            _exchangeOrderRepository = new ExchangeOrderRepository();
            DateTime _dateTime = DateTime.Now;
            try
            {
                SessionHelper _sessionHelper = (SessionHelper)Session["User"];
                var businessunitId = _sessionHelper.LoggedUserInfo.BusinessUnitId;

                tblExchangeOrder exchangeOrder = _exchangeOrderRepository.GetSingle(x => x.VoucherCode == voucherCode);
                if (exchangeOrder != null)
                {
                    tblBusinessPartner businessPartnerObj = _businessPartnerRepository.GetSingle(x=>x.BusinessPartnerId==exchangeOrder.BusinessPartnerId);
                    if(businessPartnerObj != null)
                    {
                        if(businessPartnerObj.VoucherType!=null )
                        {
                            if (businessPartnerObj.VoucherType == Convert.ToInt32(VoucherTypeEnum.Discount))
                            {
                                if (exchangeOrder.IsVoucherused == false)
                                {
                                    if (exchangeOrder.VoucherCodeExpDate >= _dateTime)
                                    {
                                        tblCustomerDetail customerObj = _customerDetailRepository.GetSingle(x => x.PhoneNumber == phoneNumber && x.Id == exchangeOrder.CustomerDetailsId);
                                        if (customerObj != null)
                                        {
                                            tblBusinessUnit businessUniObj = _businessUnitRepository.GetSingle(x => x.BusinessUnitId == businessunitId);
                                            if (businessUniObj != null && businessUniObj.Name == exchangeOrder.CompanyName)
                                            {
                                                exchangeOrderData = _exchangeOrderManager.GetExchangeOrderDCByVoucherCode(voucherCode, phoneNumber);
                                            }
                                            else
                                            {
                                                exchangeOrderData.Response = "you are not eligible to validate this voucher";
                                            }
                                        }
                                        else
                                        {
                                            exchangeOrderData.Response = "Phone number is incorrect";
                                        }
                                    }
                                    else
                                    {
                                        exchangeOrderData.Response = "Voucher already expired ";
                                    }
                                }
                                else
                                {
                                    exchangeOrderData.Response = "Voucher is already used";
                                }
                            }
                            else
                            {
                                exchangeOrderData.Response = "Voucher is not discount type cannot redeem voucher";
                            }
                           
                        }
                        else
                        {
                            exchangeOrderData.Response = "Voucher type not defined";
                        }
                    }
                    else
                    {
                        exchangeOrderData.Response = "Store Details are not found";
                    }
                   
                    
                }
                else
                {
                    exchangeOrderData.Response = "Voucher code is incorrect";
                }
            }
            catch (Exception x)
            {
                LibLogging.WriteErrorToDB("VoucherController", "GetExchangeOrder", x);
            }

            return Json(exchangeOrderData, JsonRequestBehavior.AllowGet);
        }
        #endregion       
        #region Voucher generation

        public ActionResult VoucherGeneration(int buid)
        {
            _productCategoryRepository = new ProductCategoryRepository();
            _exchangeOrderManager = new BAL.SponsorsApiCall.ExchangeOrderManager();
            _masterManager = new BAL.SponsorsApiCall.MasterManager();
            _businessPartnerRepository = new BusinessPartnerRepository();
            _businessUnitRepository = new BusinessUnitRepository();
            _productCategoryMappingRepository = new ProductCategoryMappingRepository();
            _exchangeOrderManager = new ExchangeOrderManager();
            _productConditionLabelRepository = new ProductConditionLabelRepository();
            List<tblBusinessPartner> businessPartnerList = null;
            VoucherDataContract voucherDC = new VoucherDataContract();
            ExchangeOrderDataContract exchangeDataContract = new ExchangeOrderDataContract();
            List<tblProductConditionLabel> ConditionLabelObj = new List<tblProductConditionLabel>();
            List<SelectListItem> StoreList = null;
            tblBusinessUnit BusinessUnitObj = null;
            try
            {
                //exchangeDataContract.SocietyDataContract = _exchangeOrderManager.GetSocietyById(id);

                //Product Category List
                List<tblProductCategory> prodGroupListForABB = _productCategoryRepository.GetList(x => x.IsActive == true).ToList();
                prodGroupListForABB = prodGroupListForABB.OrderBy(o => o.Description_For_ABB).ToList();
                if (prodGroupListForABB != null && prodGroupListForABB.Count > 0)
                {
                    ViewBag.ProductCategoryList = new SelectList(prodGroupListForABB, "Id", "Description");
                }
                //new product categorgy list
                List<tblProductCategory> prodGroupListForBosch = new List<tblProductCategory>();
                List<tblBUProductCategoryMapping> productCategoryForNew = new List<tblBUProductCategoryMapping>();

                DataTable dt = _productCategoryMappingRepository.GetNewProductCategory(buid);
                if (dt != null && dt.Rows.Count > 0)
                {
                    productCategoryForNew = GenericConversionHelper.DataTableToList<tblBUProductCategoryMapping>(dt);
                }
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

                exchangeDataContract.BusinessUnitDataContract = _exchangeOrderManager.GetBUById(buid);

                if (exchangeDataContract.BusinessUnitDataContract != null)
                {
                    if (exchangeDataContract.BusinessUnitDataContract.LogoName != null)
                    {
                        exchangeDataContract.BULogoName = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Content/DB_Files/SponsorLogo/" + exchangeDataContract.BusinessUnitDataContract.LogoName;
                    }
                    exchangeDataContract.ExpectedDeliveryHours = exchangeDataContract.BusinessUnitDataContract.ExpectedDeliveryHours;
                }
                //exchangeDataContract.BrandList =  new SelectList(_masterManager.GetBrandForExchange().Brand, "Id", "Name");
                if (buid > 0)
                {
                    //store details
                    voucherDC.BusinessUnitId = buid;
                    businessPartnerList = _businessPartnerRepository.GetList(x => x.IsActive == true
                        && x.BusinessUnitId == buid).ToList();

                    StoreList = businessPartnerList.Select(x => new SelectListItem
                    {
                        Text = x.Description,
                        Value = x.BusinessPartnerId.ToString()
                    }).ToList();

                    exchangeDataContract.StoreList = StoreList.OrderBy(o => o.Text).ToList();

                    //state city
                    //exchangeDataContract.City = ExchangeObj.CityName;


                    ////BU details
                    BusinessUnitObj = _businessUnitRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == buid);
                    if (BusinessUnitObj != null)
                    {
                       
                        if (BusinessUnitObj.LogoName != null)
                        {
                            exchangeDataContract.BULogoName = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Content/DB_Files/SponsorLogo/" + BusinessUnitObj.LogoName;
                        }
                        exchangeDataContract.IsSweetnerModelBased = (bool)BusinessUnitObj.IsSweetnerModelBased;
                        exchangeDataContract.CompanyName = BusinessUnitObj.Name;
                        exchangeDataContract.BUName = BusinessUnitObj.Name;
                        exchangeDataContract.BusinessUnitId = buid;
                        exchangeDataContract.ZohoSponsorNumber = BusinessUnitObj.ZohoSponsorId;
                        // Code  to get Product condition labels By vishal choudhary date[15/06/2023]
                        ConditionLabelObj = _productConditionLabelRepository.GetList(x => x.BusinessUnitId == BusinessUnitObj.BusinessUnitId && x.IsActive == true).ToList();
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

                    }
                }
                voucherDC.ExchangeOrderDataContract = exchangeDataContract;
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("voucher", "VoucherGeneration", ex);
                return View(voucherDC);
            }

            return View(voucherDC);
        }

        [HttpPost]
        public ActionResult VoucherGeneration(VoucherDataContract voucherData)
        {
            string msg = string.Empty;
            int result = 0;
            DateTime _dateTime = DateTime.Now;
            _exchangeOrderManager = new ExchangeOrderManager();
            tblVoucherVerfication tblVoucher = new tblVoucherVerfication();
            VoucherVerificationRepository _verificationRepository = new VoucherVerificationRepository();
            string fileName = null;
            try
            {

                if (voucherData != null && voucherData.ExchangeOrderDataContract != null)
                {
                    if (voucherData.ExchangeOrderDataContract.QualityCheckValue > 0)
                    {
                        voucherData.ExchangeOrderDataContract.QualityCheck = voucherData.ExchangeOrderDataContract.QualityCheckValue;
                    }
                    if (voucherData.ExchangeOrderDataContract.VoucherType == Convert.ToInt32(VoucherTypeEnum.Cash))
                    {
                        if (voucherData.Base64StringValue != null)
                        {
                            byte[] bytes = System.Convert.FromBase64String(voucherData.Base64StringValue);
                            fileName = _dateTime.ToString("yyyyMMddHHmmssFFF") + Path.GetExtension("image.jpeg");
                            string rootPath = @HostingEnvironment.ApplicationPhysicalPath;
                            string filePath = ConfigurationManager.AppSettings["ExchangeInvoiceImage"].ToString() + fileName;
                            System.IO.File.WriteAllBytes(rootPath + filePath, bytes);
                        }
                        else
                        {
                            msg = "Invoice image is not proper process again.";
                            TempData["Msg"] = msg;
                            return RedirectToAction("Details");
                        }
                    }
                   

                    voucherData.ExchangeOrderId = voucherData.ExchangeOrderDataContract.Id;
                    voucherData.CustomerId = voucherData.ExchangeOrderDataContract.CustomerDetailsId;
                    voucherData.VoucherCode = voucherData.VoucherCode;
                    voucherData.IsVoucherused = true;
                    voucherData.InvoiceImageName = fileName;

                    result = _exchangeOrderManager.AddVouchertoExchangeOrderTable(voucherData);
                    if (result > 0)
                    {
                        msg = "Thank You, voucher code has been generated and sent to customers registered mobile number and email.";
                    }
                    else
                    {
                        msg = "Some error occurred, please connect with the Administrator."; ;
                    }
                    TempData["Msg"] = msg;
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("voucher", "VoucherGeneration", ex);
                return View();
            }
            return RedirectToAction("Details");
        }

        public JsonResult GetExchangeOrderByRNumber(string rnumber)
        {
            _exchangeOrderManager = new ExchangeOrderManager();
            ExchangeOrderDataContract exchangeOrderData = null;

            try
            {
                exchangeOrderData = _exchangeOrderManager.GetExchangeOrderDCByRnumber(rnumber);
                if (exchangeOrderData != null)
                {
                    exchangeOrderData.QualityCheckValue = exchangeOrderData.QualityCheck;
                    if (exchangeOrderData.IsDtoC == true)
                        exchangeOrderData.FormatName = "Home";
                    else
                        exchangeOrderData.FormatName = "Dealer";
                }
            }
            catch (Exception x)
            {
                LibLogging.WriteErrorToDB("VoucherController", "GetExchangeOrderByRNumber", x);
            }

            return Json(exchangeOrderData, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region On Page Ajax Call Methods for OTP send and varification
        public JsonResult SendOTP(string mobnumber, int buid, string tempaltename)
        {
            _notificationManager = new NotificationManager();
            _businessUnitRepository = new BusinessUnitRepository();
            bool flag = false;
            try
            {
                tblBusinessUnit businessUnit = _businessUnitRepository.GetSingle(x => x.BusinessUnitId == buid);
                string message = string.Empty;
                string OTPValue = UniqueString.RandomNumber();
                if (tempaltename.Equals("SMS_VoucherGeneration_OTP"))
                    message = NotificationConstants.SMS_VoucherGeneration_OTP.Replace("[OTP]", OTPValue).Replace("[STORENAME]", businessUnit.Name);
                else if (tempaltename.Equals("SMS_VoucherVerification_OTP"))
                    message = NotificationConstants.SMS_VoucherVerification_OTP.Replace("[OTP]", OTPValue).Replace("[STORENAME]", businessUnit.Name);

                flag = _notificationManager.SendNotificationSMS(mobnumber, message, OTPValue);
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("VoucherController", "sendByTextLocalSMS", ex);
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
                LibLogging.WriteErrorToDB("VoucherController", "VerifyOTP", ex);
            }

            return Json(flag, JsonRequestBehavior.AllowGet);

        }

        #endregion


        #region City name and BP
        [HttpGet]
        public JsonResult GetCityByStateName(string stateName, int buid, string email)
        {
            _businessPartnerRepository = new BusinessPartnerRepository();
            IEnumerable<SelectListItem> cityList = null;
            List<tblBusinessPartner> businessPartnerList = null;
            try
            {
                DataTable dt = _businessPartnerRepository.GetCityListbyBU(stateName, buid, email);
                if (dt != null && dt.Rows.Count > 0)
                {
                    businessPartnerList = GenericConversionHelper.DataTableToList<tblBusinessPartner>(dt);
                }
                cityList = (businessPartnerList).AsEnumerable().Select(prodt => new SelectListItem() { Text = prodt.City, Value = prodt.City });
                cityList = cityList.OrderBy(o => o.Text).ToList();
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("VoucherController", "GetCityByStateName", ex);
            }
            var result = new SelectList(cityList, "Value", "Text");
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetBPByCity(string city, int buid, string associateCode)
        {
            _businessPartnerRepository = new BusinessPartnerRepository();
            List<SelectListItem> cityList = null;
            List<tblBusinessPartner> businessPartnerList = null;
            try
            {
                tblBusinessPartner businesspartnerobj = _businessPartnerRepository.GetSingle(x => (!string.IsNullOrEmpty(x.AssociateCode)&& x.AssociateCode.ToLower().Equals(associateCode.ToLower())) && (!string.IsNullOrEmpty(x.City)&& x.City.ToLower().Equals(city.ToLower())) && (x.BusinessUnitId != null && x.BusinessUnitId == buid));
                if (businesspartnerobj.IsDealer == false)
                {
                    
                        businessPartnerList = _businessPartnerRepository.GetList(x => x.IsActive == true && (x.BusinessUnitId != null && x.BusinessUnitId == buid)
                        && (!string.IsNullOrEmpty(x.AssociateCode) && x.AssociateCode.ToLower().Equals(associateCode.ToLower()))
                        && (!string.IsNullOrEmpty(x.City) && x.City.ToLower().Equals(city.ToLower()))).ToList();

                        cityList = businessPartnerList.Select(x => new SelectListItem
                        {
                            Text = x.Name + ", " + x.AddressLine1,
                            Value = x.BusinessPartnerId.ToString()
                        }).ToList();

                        cityList.Add(new SelectListItem { Text = "Other", Value = "999999" });
                    

                }
                else
                {
                    businessPartnerList = _businessPartnerRepository.GetList(x => x.IsActive == true && (x.BusinessUnitId != null && x.BusinessUnitId == buid)
                    && (!string.IsNullOrEmpty(x.AssociateCode) && x.AssociateCode.ToLower().Equals(associateCode.ToLower()))
                    && (!string.IsNullOrEmpty(x.City) && x.City.ToLower().Equals(city.ToLower()))).ToList();

                    cityList = businessPartnerList.Select(x => new SelectListItem
                    {
                        Text = x.Name + ", " + x.AddressLine1,
                        Value = x.BusinessPartnerId.ToString()
                    }).ToList();

                
                }

               
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("VoucherController", "GetCityByStateName", ex);
            }
            var result = new SelectList(cityList, "Value", "Text");
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion
        
        #region Convert Html Veiw To Image

        public ActionResult GetVoucher(int id)
        {
            _exchangeOrderManager = new BAL.SponsorsApiCall.ExchangeOrderManager();
            _tandcRepository = new TermsAndConditionsForVoucherRepository();
            VoucherDataContract voucherDC = new VoucherDataContract();
            ExchangeOrderDataContract exchangeDC = null;
            string response = string.Empty;
           List<tblVoucherTermsAndCondition> termsandconditionList = null;
           
            try
            {
                exchangeDC = _exchangeOrderManager.GetExchangeOrderDCById(id);

                if (exchangeDC.BusinessUnitDataContract != null && exchangeDC.BusinessUnitDataContract.LogoName != null)
                {
                    exchangeDC.BULogoName = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Content/DB_Files/SponsorLogo/" + exchangeDC.BusinessUnitDataContract.LogoName;
                }
               
                exchangeDC.VoucherCodeExpDateString = exchangeDC.VoucherCodeExpDate != null ? Convert.ToDateTime(exchangeDC.VoucherCodeExpDate).ToString("dd/MM/yyyy") : string.Empty;
                termsandconditionList = _tandcRepository.GetList(x => x.BusinessUnitId == exchangeDC.BusinessUnitDataContract.BusinessUnitId && x.IsDeffered==false).ToList();
                exchangeDC.TermsandCondition = new List<SelectListItem>();
                if (termsandconditionList.Count > 0)
                {
                    exchangeDC.TermsandCondition = termsandconditionList.Select(x => new SelectListItem
                    {
                        Text = x.TermsandCondition,
                        Value = x.Id.ToString()
                    }).ToList();
                }
            }
            catch (Exception ex) 
            {
                LibLogging.WriteErrorToDB("VoucherController", "GetVoucher", ex);
            }
            return View(exchangeDC);
        }


        #endregion

        #region Dealer Dashboard

        [HttpGet]
        public ActionResult DealerDashboard()
        {
            string urlerp = ConfigurationManager.AppSettings["ERPBaseURL"].ToString(); 
            return Redirect(urlerp);
            //string msg = string.Empty;
            //BusinessPartnerViewModel businessPartnerViewObj;
            //_exchangeOrderManager = new ExchangeOrderManager();
            //_exchangeOrderRepository = new ExchangeOrderRepository();
            //_businessPartnerRepository = new BusinessPartnerRepository();
            //_businessUnitRepository = new BusinessUnitRepository();
            //List<tblBusinessPartner> businessPartnersList = new List<tblBusinessPartner>();
            //List<tblExchangeOrder> exchangeOrdersList = new List<tblExchangeOrder>();
            //ExchangeOrderDataContract ExchangeObj = new ExchangeOrderDataContract();
            //tblBusinessUnit BusinessUnitObj = new tblBusinessUnit();
            //ExchangeObj.CityList = new List<SelectListItem>();
            //List<SelectListItem> StoreList = null;

            //if (ValidateBPLogin())
            //{
            //    SessionHelper _sessionHelper = (SessionHelper)Session["User"];
            //    ViewBag.LoginUser = _sessionHelper.LoggedUserInfo;
            //    ViewBag.IsDealer = _sessionHelper.LoggedUserInfo.IsDealer;
            //    ExchangeObj.Email = _sessionHelper.LoggedUserInfo.Email;
            //    ExchangeObj.BusinessUnitId = (int)_sessionHelper.LoggedUserInfo.BusinessUnitId;
            //    ExchangeObj.AssociateCode = _sessionHelper.LoggedUserInfo.AssociateCode;
            //    ExchangeObj.City = _sessionHelper.LoggedUserInfo.City;
            //    int count = 0;
            //    int OrderCount = 0;

            //    BusinessUnitObj = _businessUnitRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == (int)_sessionHelper.LoggedUserInfo.BusinessUnitId);
            //    if (BusinessUnitObj != null)
            //    {
            //        if (BusinessUnitObj.LogoName != null)
            //        {
            //            ExchangeObj.BULogoName = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Content/DB_Files/SponsorLogo/" + BusinessUnitObj.LogoName;
            //        }
            //        ExchangeObj.CompanyName = BusinessUnitObj.Name;
            //    }
            //    DataTable bporderCount = _businessPartnerRepository.GetAllStoreOrderCount(ExchangeObj.AssociateCode);
            //   List<OrderCounViewModel> ordercount = new List<OrderCounViewModel>();
            //    if (bporderCount!=null && bporderCount.Rows.Count > 0)
            //    {
            //        ordercount = GenericConversionHelper.DataTableToList<OrderCounViewModel>(bporderCount);
            //        foreach(var item in ordercount)
            //        {
            //            ViewBag.NoOfOrders =item.OrderCount;
            //        }
            //    }
            //    else
            //    {
            //        msg = "No Records Found";
            //        TempData["Msg"] = msg;
            //        return RedirectToAction("Details");
            //    }
            //    //Old Code To Get Order Count.
            //    //foreach (var item in businessPartnersList)
            //    //{
            //    //    businessPartnerViewObj = new BusinessPartnerViewModel();
            //    //    businessPartnerViewObj.BusinessPartnerId = item.BusinessPartnerId;
            //    //    if (businessPartnerViewObj != null)
            //    //    {
            //    //        exchangeOrdersList = _exchangeOrderRepository.GetList(x => x.BusinessPartnerId == businessPartnerViewObj.BusinessPartnerId).ToList();
            //    //        count = exchangeOrdersList.Where(x => x.BusinessPartnerId == businessPartnerViewObj.BusinessPartnerId && x.IsVoucherused == true).Count();
            //    //    }
            //    //    else
            //    //    {
            //    //        msg = "No Records Found";
            //    //        TempData["Msg"] = msg;
            //    //        return RedirectToAction("Details");
            //    //    }

            //    //    orderCount += count;
            //    //}
                

            //    //get state list
            //    DataTable dt = _businessPartnerRepository.GetStateList();
            //    if (dt != null && dt.Rows.Count > 0)
            //    {
            //        businessPartnersList = GenericConversionHelper.DataTableToList<tblBusinessPartner>(dt);
            //        List<string> states = businessPartnersList.Where(x => x.BusinessUnitId == _sessionHelper.LoggedUserInfo.BusinessUnitId 
            //        && (x.AssociateCode != null && x.AssociateCode.ToLower().Equals(_sessionHelper.LoggedUserInfo.AssociateCode.ToLower()))
            //        && (x.IsExchangeBP != null && x.IsExchangeBP == true)).OrderBy(o => o.State).Select(x => x.State).Distinct().ToList();
            //        List<SelectListItem> stateListItems = states.Select(x => new SelectListItem
            //        {
            //            Text = x,
            //            Value = x
            //        }).ToList();
            //        ViewBag.StateList = new SelectList(stateListItems, "Text", "Text");
            //    }

            //    //store List
            //    if (ExchangeObj.City != null)
            //    {
            //        businessPartnersList = businessPartnersList.Where(x => (x.IsExchangeBP != null && x.IsExchangeBP == true)
            //            && (x.BusinessUnitId != null && x.BusinessUnitId == ExchangeObj.BusinessUnitId)
            //            && (x.City != null && x.City.ToLower().Equals(ExchangeObj.City.ToLower()))).ToList();

            //        StoreList = businessPartnersList.Select(x => new SelectListItem
            //        {
            //            Text = x.Description + ", " + x.AddressLine1,
            //            Value = x.BusinessPartnerId.ToString()
            //        }).ToList();
            //        ExchangeObj.StoreList = StoreList;
            //    }
            //    return View(ExchangeObj);
            //}
            //else
            //{
            //    return RedirectToAction("Login", "Voucher", new { Area = "" });
            //}

        }
        #endregion

        #region get order count of individual BP
        public JsonResult GetOrderCount(ExchangeOrderDataContract exchangeOrderData)
        {
            _exchangeOrderManager = new ExchangeOrderManager();
            DateTime Month = Convert.ToDateTime(exchangeOrderData.Year1);
            exchangeOrderData.Month1 = Month.ToString("MM");
            exchangeOrderData.Month = Convert.ToInt32(exchangeOrderData.Month1);
            DateTime Year = Convert.ToDateTime(exchangeOrderData.Year1);
            exchangeOrderData.Year1 = Year.ToString("yyyy");
            exchangeOrderData.Year = Convert.ToInt32(exchangeOrderData.Year1);
            var exchangeOrderObj = _exchangeOrderManager.GetExchangeOrderDetailbyBPId(exchangeOrderData);
            foreach (var item in exchangeOrderObj)
            {
                item.Sweetner = item.Sweetener != null ? Convert.ToDecimal(item.Sweetener) : 0;
            }
            return Json(new { data = exchangeOrderObj }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Logout
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Login", "Voucher");
        }
        #endregion

        #region VerifyDuplicateExchangeOrders
        public JsonResult VerifyDuplicateExchangeOrders(int? NewProductId, int OldProductTypeId, string customerEmail, string customerPhone)
        {
            _exchangeOrderManager = new ExchangeOrderManager();
            bool flag = false;
            try
            {
                flag = _exchangeOrderManager.ValidateExchangeProductExists(NewProductId, OldProductTypeId, customerEmail, customerPhone);
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("VoucherController", "VerifyDuplicateExchangeOrders", ex);
            }

            return Json(flag, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region 
        [HttpGet]
        public JsonResult GetNewProdcutCategorydetails(int newCatId, int ExchangeOrderId)
        {
            _exchangeOrderManager = new ExchangeOrderManager();
            bool result = false;
            try
            {
                result = _exchangeOrderManager.NewProductCategoryDetails(newCatId, ExchangeOrderId);
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeController", "GetIsOrcAndIsDefferedSettelmentByBPId", ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 
        [HttpGet]
        public JsonResult GetNewVoucher(int ExchangeOrderId, int newCatId, int newsubCatId, int buid, int ExchangePriceNew, int modelno)
        {
            _exchangeOrderManager = new ExchangeOrderManager();
            ExchangeOrderDataContract exchangeorderDC = new ExchangeOrderDataContract();
            ExchangeOrderRepository exchangerepository = new ExchangeOrderRepository();

            try
            {
                tblExchangeOrder exchangeOrder = exchangerepository.GetSingle(x => x.Id == ExchangeOrderId && x.IsActive == true);
                if (exchangeOrder != null)
                {
                    if (exchangeOrder.NewProductCategoryId == newCatId)
                    {
                        exchangeorderDC.Id = ExchangeOrderId;
                        exchangeorderDC.NewProductCategoryId = newCatId;
                        exchangeorderDC.NewProductCategoryTypeId = newsubCatId;
                        exchangeorderDC.ExchangePrice = ExchangePriceNew;
                        exchangeorderDC.ModelNumberId = modelno;
                        exchangeorderDC.BusinessUnitId = buid;

                        exchangeorderDC = _exchangeOrderManager.UpdateVoucher(exchangeorderDC);
                    }
                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("VoucherController", "GetNewVoucher", ex);
            }

            return Json(exchangeorderDC, JsonRequestBehavior.AllowGet);
        }
        #endregion


        [HttpGet]
        public JsonResult GetProdPrice(int productCatId, int productSubCatId, int brandId, int conditionId, int buid, int ExchangeOrderId, int newcatid = 0, int newsubcatid = 0, int modelno = 0, bool IsSweetnerModelBased = false, string formatType = "")
        {
            _masterManager = new BAL.SponsorsApiCall.MasterManager();
            string price = null;
            try
            {
                if (IsSweetnerModelBased == true)
                {
                    if (newcatid > 0 && newsubcatid > 0 && modelno > 0 && ExchangeOrderId > 0)
                    {
                        price = _masterManager.GetProductPriceforAbsoluteSweetner(newcatid, newsubcatid, productCatId, productSubCatId, brandId, conditionId, buid, modelno, ExchangeOrderId, formatType);
                    }
                }
                else
                {
                    price = _masterManager.GetProductPrice(productCatId, productSubCatId, brandId, conditionId, buid, formatType);
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("VoucherController", "GetProdPrice", ex);
            }

            return Json(price, JsonRequestBehavior.AllowGet);
        }

        #region 
        [HttpGet]
        public JsonResult CancelOrder(int ExchangeOrderId, bool IsCanceled)
        {
            _exchangeOrderManager = new ExchangeOrderManager();
            bool success = false;
            string message = string.Empty;
            try
            {
                if (ExchangeOrderId > 0 && IsCanceled == true)
                {
                    success = _exchangeOrderManager.CancelOrder(ExchangeOrderId);
                    if (success == true)
                    {
                        message = "Your Order has Been Canceled";
                        if (!string.IsNullOrEmpty(message))
                            TempData["Msg"] = message;
                        else
                            TempData["Msg"] = "Some error occurred, please connect with the Administrator.";
                    }
                    else
                    {
                        TempData["Msg"] = "Some error occurred, please connect with the Administrator.";
                    }


                }
            }
            catch (Exception ex)
            {

                LibLogging.WriteErrorToDB("VoucherController", "CancelOrder", ex);

            }
            return Json(new { redirectToUrl = Url.Action("Details", "Voucher") }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region
        public ActionResult GetVoucherCash(int id)
        {
            _exchangeOrderManager = new BAL.SponsorsApiCall.ExchangeOrderManager();
            _tandcRepository = new TermsAndConditionsForVoucherRepository();
            VoucherDataContract voucherDC = new VoucherDataContract();
            ExchangeOrderDataContract exchangeDC = null;
            string response = string.Empty;
            List<tblVoucherTermsAndCondition> termsandconditionList = null;

            try
            {
                exchangeDC = _exchangeOrderManager.GetExchangeOrderDCById(id);
                if (exchangeDC.BusinessUnitDataContract != null && exchangeDC.BusinessUnitDataContract.LogoName != null)
                {
                    exchangeDC.BULogoName = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Content/DB_Files/SponsorLogo/" + exchangeDC.BusinessUnitDataContract.LogoName;
                }
                exchangeDC.VoucherCodeExpDateString = exchangeDC.VoucherCodeExpDate != null ? Convert.ToDateTime(exchangeDC.VoucherCodeExpDate).ToString("dd/MM/yyyy") : string.Empty;
                termsandconditionList = _tandcRepository.GetList(x => x.BusinessUnitId == exchangeDC.BusinessUnitDataContract.BusinessUnitId && x.IsDeffered == true).ToList();
                exchangeDC.TermsandCondition = new List<SelectListItem>();
                if (termsandconditionList.Count > 0)
                {
                    exchangeDC.TermsandCondition = termsandconditionList.Select(x => new SelectListItem
                    {
                        Text = x.TermsandCondition,
                        Value = x.Id.ToString()
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("VoucherController", "GetVoucher", ex);
            }
            return View(exchangeDC);
        }
        #endregion

        #region Get vouocher Instant for ABB Redemption
        public ActionResult GetVoucherInstant(int id)
        {
            _abbOrderManager = new ABBOrderMaanger() ;
            _tandcRepository = new TermsAndConditionsForVoucherRepository();
            VoucherDataContract voucherDC = new VoucherDataContract();
            RedemptionDataContract abbredemptionDc = null;
            string response = string.Empty;
            List<tblVoucherTermsAndCondition> termsandconditionList = null;

            try
            {
                abbredemptionDc = _abbOrderManager.GetOrderData(id);

                if (abbredemptionDc.BusinessUnitId >0 && abbredemptionDc.BULogoName!=null)
                {
                    abbredemptionDc.BULogoName = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Content/DB_Files/SponsorLogo/" + abbredemptionDc.BULogoName;
                }

                abbredemptionDc.VoucherCodeExpDateString = abbredemptionDc.VoucherCodeExpDate != null ? Convert.ToDateTime(abbredemptionDc.VoucherCodeExpDate).ToString("dd/MM/yyyy") : string.Empty;
                termsandconditionList = _tandcRepository.GetList(x => x.BusinessUnitId == abbredemptionDc.BusinessUnitId && x.IsDeffered == false).ToList();
                abbredemptionDc.TermsandCondition = new List<SelectListItem>();
                if (termsandconditionList.Count > 0)
                {
                    abbredemptionDc.TermsandCondition = termsandconditionList.Select(x => new SelectListItem
                    {
                        Text = x.TermsandCondition,
                        Value = x.Id.ToString()
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("VoucherController", "GetVoucher", ex);
            }
            return View(abbredemptionDc);
        }
        #endregion

        #region to generate cash voucher for  abb redemption
        public ActionResult GetVoucherRedemptionCash(int id)
        {
            _abbOrderManager = new ABBOrderMaanger();
            _tandcRepository = new TermsAndConditionsForVoucherRepository();
            VoucherDataContract voucherDC = new VoucherDataContract();
            RedemptionDataContract abbredemptionDc = null;
            string response = string.Empty;
            List<tblVoucherTermsAndCondition> termsandconditionList = null;

            try
            {
                abbredemptionDc = _abbOrderManager.GetOrderData(id);

                if (abbredemptionDc.BusinessUnitId > 0 && abbredemptionDc.BULogoName != null)
                {
                    abbredemptionDc.BULogoName = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Content/DB_Files/SponsorLogo/" + abbredemptionDc.BULogoName;
                }

                abbredemptionDc.VoucherCodeExpDateString = abbredemptionDc.VoucherCodeExpDate != null ? Convert.ToDateTime(abbredemptionDc.VoucherCodeExpDate).ToString("dd/MM/yyyy") : string.Empty;
                termsandconditionList = _tandcRepository.GetList(x => x.BusinessUnitId == abbredemptionDc.BusinessUnitId && x.IsDeffered == false).ToList();
                abbredemptionDc.TermsandCondition = new List<SelectListItem>();
                if (termsandconditionList.Count > 0)
                {
                    abbredemptionDc.TermsandCondition = termsandconditionList.Select(x => new SelectListItem
                    {
                        Text = x.TermsandCondition,
                        Value = x.Id.ToString()
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("VoucherController", "GetVoucher", ex);
            }
            return View(abbredemptionDc);
        }
        #endregion
    }
}