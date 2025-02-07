using GraspCorn.Common.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RDCEL.DocUpload.BAL.Common;
using RDCEL.DocUpload.BAL.Manager;
using RDCEL.DocUpload.BAL.ProcessAPI;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DataContract.SponsorModel;

namespace RDCEL.DocUpload.Web.API.Controllers
{
    public class PineLabsController : Controller
    {
        #region Variable ddeclaration 
        ProductCategoryRepository _productCategoryRepository;
        BAL.SponsorsApiCall.ExchangeOrderManager _exchangeOrderManager;
        BusinessPartnerRepository _businessPartnerRepository;
        BusinessUnitRepository _businessUnitRepository;
        ExchangeOrderRepository _exchangeOrderRepository;
        LoginDetailsUTCRepository _loginRepository;
        PinCodeRepository pinCodeRepository;
        CustomerDetailsRepository _customerDetailsRepository;
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
            tblExchangeOrder ExchangeObj = null;
            tblCustomerDetail CustomerObj = null;
            ExchangeOrderDataContract exchangeOrderDC = new ExchangeOrderDataContract();
            try
            {

                if (RegNo != null)
                {
                    string reg = RegNo.Replace(".", "");
                    ExchangeObj = _exchangeOrderRepository.GetSingle(x =>!string.IsNullOrEmpty( x.RegdNo)&& x.RegdNo.ToLower().Equals(reg.ToLower()));
                    if (ExchangeObj != null)
                    {
                        CustomerObj = _customerDetailsRepository.GetSingle(x => x.Id == ExchangeObj.CustomerDetailsId);
                        if (CustomerObj != null)
                        {
                            exchangeOrderDC.PhoneNumber = CustomerObj.PhoneNumber;
                            exchangeOrderDC.PhoneNo = CustomerObj.PhoneNumber;
                        }
                        tblBusinessPartner BuisnessPrtnerObj = _businessPartnerRepository.GetSingle(x => x.BusinessPartnerId == ExchangeObj.BusinessPartnerId && x.IsActive == true);
                        if (BuisnessPrtnerObj != null)
                        {
                            exchangeOrderDC.BULogoName = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Content/DB_Files/SponsorLogo/" + BuisnessPrtnerObj.LogoImage;
                        }
                        exchangeOrderDC.BusinessUnitId = BuisnessPrtnerObj.BusinessUnitId;
                        exchangeOrderDC.BusinessPartnerId = BuisnessPrtnerObj.BusinessPartnerId;
                        exchangeOrderDC.RegdNo = ExchangeObj.RegdNo;
                        exchangeOrderDC.SponsorOrderNumber = ExchangeObj.SponsorOrderNumber;

                    }
                    return View(exchangeOrderDC);
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("PineLabs", "CustomerDetails", ex);
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
                    productOrderResponseDC = _exchangeOrderManager.UpdateExchangeOrder(ExchangeObj);
                    if (productOrderResponseDC != null)
                        message = "Thank you. Your product details have been received at  Digi2L. Our quality check team will soon knock at your door.";
                    else
                        message = "Order not Created";
                }
                if (!string.IsNullOrEmpty(message))
                    TempData["Msg"] = message;
                else
                    TempData["Msg"] = "Some error occurred, please connect with the Administrator.";
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("PineLabs", "CustomerDetails", ex);
                return View(exchangeOrderDC);
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

        #region City And pincode
        [HttpPost]
        public JsonResult GetPincodeforPineLabs(string pintext, int? buid)
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
                foreach (var item in pincodeListForPineLabs)
                {
                    mygateCityState.StateName = item.State;
                    mygateCityState.CityName = item.Location;
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("PineLabs", "GetState", ex);
            }

            return Json(mygateCityState, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}