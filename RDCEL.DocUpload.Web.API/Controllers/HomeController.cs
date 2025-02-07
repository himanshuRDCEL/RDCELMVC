using GraspCorn.Common.Enums;
using GraspCorn.Common.Helper;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using RDCEL.DocUpload.BAL.Common;
using RDCEL.DocUpload.BAL.SponsorsApiCall;
using RDCEL.DocUpload.BAL.Utility;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DataContract.BillCloud;
using RDCEL.DocUpload.DataContract.Bizlog;
using RDCEL.DocUpload.DataContract.LogisticsDetails;

namespace RDCEL.DocUpload.Web.API.Controllers
{
    public class HomeController : Controller
    {
        #region Variable Declaration
        EVCPODDetailsRepository _evcPODDetailsRepository;
        ExchangeOrderRepository _exchangeOrderRepository;
        ServicePartnerRepository _servicepartnerRepository;
        UtilityManager _utilityManager = null;
        #endregion
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            LibLogging.WriteErrorToDB("ExchangeController", "SelectBP", null);
            return View();
        }

        //Scheduler for sync Zoho book to zoho creater EVC balance
        public ActionResult Schedular()
        {
            bool flag = false;

            try
            {
                Process p = new Process();
                p.StartInfo.FileName = ConfigurationManager.AppSettings["ExePath"].ToString() + ConfigurationManager.AppSettings["FileName"].ToString();
                p.Start();
                flag = true;
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("HomeController", "Schedular", ex);
                flag = false;
            }
            finally
            {
                if (flag == true)
                {
                    ViewBag.Flag = "Wallet Updated Successfully";
                }
                else
                {
                    ViewBag.Flag = "Wallet Not Updated Successfully";
                }

            }

            return View();
        }

        // For create ticket from bizlog
        [HttpGet]
        public ActionResult CreateTicket()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateTicket(string SponsorOrderNo)
        {
            // string url = "https://utcbridge.com/UTCAPI/api/ZohoExposed/CreateTicket?zohoSponsorOrderNo="+SponsorOrderNo;
            // return Redirect(url);
            // string url = "~/api/ZohoExposed/CreateTicket?zohoSponsorOrderNo="+SponsorOrderNo;

            return Redirect("~/api/ZohoExposed/CreateTicket?zohoSponsorOrderNo=" + SponsorOrderNo);

        }

        // For check order status
        [HttpGet]
        public ActionResult SponsorOrderStatus()
        {

            return View();

        }

        [HttpPost]
        public ActionResult SponsorOrderStatus(string SponsorOrderNo)
        {

            // return Redirect("~/api/OrderStatus/GetOrderStatus?OrderNo=" + SponsorOrderNo);
            return Redirect("~/api/OrderStatus/GetSponsorOrderStatusbyOrderNumberfromUTC?OrderNo=" + SponsorOrderNo);
        }

        public ActionResult ALLIntegrationScheduler()
        {
            bool flag = false;

            try
            {
                Process p = new Process();
                p.StartInfo.FileName = ConfigurationManager.AppSettings["AllIntegrationExePath"].ToString() + ConfigurationManager.AppSettings["AllIntegrationFileName"].ToString();
                p.Start();
                flag = true;
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("HomeController", "ALLIntegrationScheduler", ex);
                flag = false;
            }
            finally
            {
                if (flag == true)
                {
                    ViewBag.Flag = "Price master and Pincode master Table Updated Successfully";
                }
                else
                {
                    ViewBag.Flag = "Price master and Pincode master Table Not Updated Successfully";
                }

            }

            return View();
        }

        [HttpGet]
        public ActionResult V(int id)
        {
            return Redirect("~/Voucher/getvoucher?id=" + id);
        }

        [HttpGet]
        public ActionResult VC(int id)
        {
            return Redirect("~/Voucher/GetVoucherCash?id=" + id);
        }
        #region Create Ticket With update Bizlog
        // For create ticket from bizlog
        [HttpGet]
        public ActionResult CreateTicketForLogistics(string RegdNo)
        {
            _servicepartnerRepository = new ServicePartnerRepository();
            ServicePartnerLogin servicepartnerdc = new ServicePartnerLogin();
            try
            {
                if (RegdNo != null)
                {
                    servicepartnerdc.RegdNo = RegdNo;
                }
                DataTable dt = _servicepartnerRepository.GetServicePartnerList();
                if (dt.Rows.Count > 0)
                {
                    List<tblServicePartner> servicePartner = GenericConversionHelper.DataTableToList<tblServicePartner>(dt);
                    ViewBag.servicePartner = new SelectList(servicePartner, "ServicePartnerId", "ServicePartnerName");
                }
                List<SelectListItem> priorityList = new List<SelectListItem>();
                servicepartnerdc.priorityList = new List<SelectListItem>();
            }
            catch(Exception ex)
            {
                LibLogging.WriteErrorToDB("HomeController", "CreateTicketWithBizLog",ex);
            }

            
            return View(servicepartnerdc);
        }
        [HttpGet]
        public JsonResult GetOrderPriorityList(int servicepartnerId)
        {
            List<SelectListItem> priorityList = new List<SelectListItem>();
            if (servicepartnerId == Convert.ToInt32(ServicePartner.Bizlog))
            {
                priorityList.Insert(0, new SelectListItem() { Value = "high", Text = "High" });
                priorityList.Insert(0, new SelectListItem() { Value = "medium", Text = "Medium" });
                priorityList.Insert(0, new SelectListItem() { Value = "low", Text = "Low" });
            }
            var result = new SelectList(priorityList, "Value", "Text");
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPriorityNeddedforpartner(int servicepartnerId)
        {
            bool flag = false;
            _servicepartnerRepository = new ServicePartnerRepository();
            tblServicePartner srvicePartner = _servicepartnerRepository.GetSingle(x => x.ServicePartnerId == servicepartnerId);
            if (servicepartnerId == Convert.ToInt32(ServicePartner.Bizlog))
            {
                flag = true;
            }
            else
            {
                flag = false;
            }
            return Json(flag, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetServicepartnerDetails(int servicepartnerId)
        {
            bool flag = false;
            _servicepartnerRepository = new ServicePartnerRepository();
            tblServicePartner srvicePartner = _servicepartnerRepository.GetSingle(x => x.ServicePartnerId == servicepartnerId);
            if (srvicePartner.IsServicePartnerLocal==true)
            {
                flag = true;
            }
            else
            {
                flag = false;
            }
            return Json(flag, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult CreateTicketForLogistics(ServicePartnerLogin loginobj)
        {
            if (loginobj != null)
            {
                if (loginobj.ServicePartnerId == Convert.ToInt32(ServicePartner.Bizlog))
                {
                    return Redirect("~/api/ZohoExposed/CreateTicketWithBizlog?RegdNo="+loginobj.RegdNo+"&&GenerateTicketWithoutBalanceCheck=" + loginobj.GenerateticketWithutCheckingBalance.ToString() + "&&priority=" + loginobj.priority + "&&servicePartnerId=" + loginobj.ServicePartnerId);
                }
                else if (loginobj.ServicePartnerId == Convert.ToInt32(ServicePartner.Mahindra))
                {
                    return Redirect("~/api/ZohoExposed/RequestMahindraLGC?RegdNo=" + loginobj.RegdNo + "&GenerateTicketWithoutBalanceCheck=" + loginobj.GenerateticketWithutCheckingBalance.ToString() + "&servicePartnerId=" + loginobj.ServicePartnerId);
                }
                else if (loginobj.IsServicePartnerLocal==true)
                {
                    return Redirect("~/api/ZohoExposed/GenerateTicketForLocalLgcPartner?RegdNo=" + loginobj.RegdNo + "&GenerateTicketWithoutBalanceCheck=" + loginobj.GenerateticketWithutCheckingBalance.ToString() + "&servicePartnerId=" + loginobj.ServicePartnerId);
                }
               
            }

            return View();
        }
        #endregion

        #region Manage Zoho Missing Exchange Orders of UTC Bridge
        [HttpGet]
        public ActionResult MoveUTCbridgeToZoho()
        {

            if (TempData["Auth"] != null)
            {
                if (Convert.ToBoolean(TempData["Auth"]) == false)
                    ShowMessage("Some Error Occured", MessageTypeEnum.error);
                if (Convert.ToBoolean(TempData["Auth"]) == true)
                    ShowMessage("Data moved to Zoho Creator Successfully!", MessageTypeEnum.success);
            }

            BusinessUnitRepository _businessUnitRepository = new BusinessUnitRepository();
            List<string> CompanyNames = _businessUnitRepository.GetList(x => x.IsActive == true).OrderBy(o => o.Name).Select(x => x.Name.Trim()).Distinct().ToList();
            List<SelectListItem> companySelectItems = CompanyNames.Select(x => new SelectListItem
            {
                Text = x,
                Value = x
            }).ToList();
            ViewBag.companyList = new SelectList(companySelectItems, "Text", "Text");

            List<SelectListItem> monthList = new List<SelectListItem>();
            monthList.Insert(0, new SelectListItem() { Value = "1", Text = "January" });
            monthList.Insert(0, new SelectListItem() { Value = "2", Text = "February" });
            monthList.Insert(0, new SelectListItem() { Value = "3", Text = "March" });
            monthList.Insert(0, new SelectListItem() { Value = "4", Text = "April" });
            monthList.Insert(0, new SelectListItem() { Value = "5", Text = "May" });
            monthList.Insert(0, new SelectListItem() { Value = "6", Text = "June" });
            monthList.Insert(0, new SelectListItem() { Value = "7", Text = "July" });
            monthList.Insert(0, new SelectListItem() { Value = "8", Text = "August" });
            monthList.Insert(0, new SelectListItem() { Value = "9", Text = "September" });
            monthList.Insert(0, new SelectListItem() { Value = "10", Text = "October" });
            monthList.Insert(0, new SelectListItem() { Value = "11", Text = "November" });
            monthList.Insert(0, new SelectListItem() { Value = "12", Text = "December" });

            ViewBag.monthList = new SelectList(monthList, "Value", "Text");


            return View();

        }

        [HttpPost]
        public ActionResult MoveUTCbridgeToZoho(string companyName, int month)
        {
            ExchangeOrderManager exchangeOrderManager = new ExchangeOrderManager();
            bool flag = exchangeOrderManager.MoveUTCbridgeToZoho(companyName, month);
            TempData["Auth"] = flag;

            return RedirectToAction("MoveUTCbridgeToZoho", "Home", new { Area = "" });

        }

        public void ShowMessage(string message, MessageTypeEnum messageType)
        {
            ViewBag.MessageType = messageType;
            ModelState.AddModelError(string.Empty, message);
        }
        #endregion

        #region Manage Zoho Missing ABB Orders of UTC Bridge
        [HttpGet]
        public ActionResult MoveUTCbridgeABBToZoho()
        {

            if (TempData["Auth"] != null)
            {
                if (Convert.ToBoolean(TempData["Auth"]) == false)
                    ShowMessage("Some Error Occured", MessageTypeEnum.error);
                if (Convert.ToBoolean(TempData["Auth"]) == true)
                    ShowMessage("Data moved to Zoho Creator Successfully!", MessageTypeEnum.success);
            }
            List<SelectListItem> monthList = new List<SelectListItem>();
            monthList.Insert(0, new SelectListItem() { Value = "1", Text = "January" });
            monthList.Insert(0, new SelectListItem() { Value = "2", Text = "February" });
            monthList.Insert(0, new SelectListItem() { Value = "3", Text = "March" });
            monthList.Insert(0, new SelectListItem() { Value = "4", Text = "April" });
            monthList.Insert(0, new SelectListItem() { Value = "5", Text = "May" });
            monthList.Insert(0, new SelectListItem() { Value = "6", Text = "June" });
            monthList.Insert(0, new SelectListItem() { Value = "7", Text = "July" });
            monthList.Insert(0, new SelectListItem() { Value = "8", Text = "August" });
            monthList.Insert(0, new SelectListItem() { Value = "9", Text = "September" });
            monthList.Insert(0, new SelectListItem() { Value = "10", Text = "October" });
            monthList.Insert(0, new SelectListItem() { Value = "11", Text = "November" });
            monthList.Insert(0, new SelectListItem() { Value = "12", Text = "December" });

            ViewBag.monthList = new SelectList(monthList, "Value", "Text");
            return View();

        }

        [HttpPost]
        public ActionResult MoveUTCbridgeABBToZoho(int month)
        {
            ABBOrderMaanger abbOrderManager = new ABBOrderMaanger();
            bool flag = abbOrderManager.MoveUTCBridgeToZoho(month);
            TempData["Auth"] = flag;

            return RedirectToAction("MoveUTCbridgeABBToZoho", "Home", new { Area = "" });

        }
        #endregion


        #region BlowHorn Call
        [HttpGet]
        public ActionResult CreateShipment()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateShipment(string SponsorOrderNo)
        {

            return Redirect("~/api/ZohoExposed/CreateShipment?zohoSponsorOrderNo=" + SponsorOrderNo);

        }
        #endregion

        #region Make Product Delivered
        [HttpGet]
        public ActionResult MarkOrderVoucherRedeem()
        {

            if (TempData["Auth"] != null)
            {
                if (Convert.ToBoolean(TempData["Auth"]) == false)
                    ShowMessage("Some Error Occured", MessageTypeEnum.error);
                if (Convert.ToBoolean(TempData["Auth"]) == true)
                    ShowMessage("Data moved to Zoho Creator Successfully!", MessageTypeEnum.success);
            }
            
            return View();

        }

        [HttpPost]
        public ActionResult MarkOrderVoucherRedeem(string regNumber)
        {
            return Redirect("~/api/VoucherStatus/UpdateVoucherStatus?RegNo=" + regNumber );

        }
        #endregion

        #region Raise Ticket Using MahindraLogistics
        // For create ticket from bizlog
        [HttpGet]
        public ActionResult MahindraLogistics()
        {
            return View();
        }
        [HttpPost]
        public ActionResult MahindraLogistics(string SponsorOrderNo, bool GenerateTicketWithoutBalanceCheck)
        {
            return Redirect("~/api/ZohoExposed/RequestMahindraLGC?zohoSponsorOrderNo=" + SponsorOrderNo + "&GenerateTicketWithoutBalanceCheck=" + GenerateTicketWithoutBalanceCheck.ToString());
        }
        #endregion

        #region Utility Index Page
        public ActionResult UtilityIndex()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
        #endregion

        #region Store Exchange POD

        [HttpGet]
        public ActionResult ExchangePOD()
        {
            if (TempData["Auth"] != null)
            {
                if (Convert.ToBoolean(TempData["Auth"]) == false)
                    ShowMessage("Some Error Occured", MessageTypeEnum.error);
                if (Convert.ToBoolean(TempData["Auth"]) == true)
                    ShowMessage("Data moved to Zoho Creator Successfully!", MessageTypeEnum.success);
            }
            return View();
        }

        [HttpPost]
        public JsonResult GetRegdNumByInput(string regdNum)
        {
            _evcPODDetailsRepository = new EVCPODDetailsRepository();
            _exchangeOrderRepository = new ExchangeOrderRepository();
            IEnumerable<SelectListItem> regdNumList = null;
            List<tblExchangeOrder>  tblExchangeOrders = null;

            try
            {
                tblExchangeOrders =  _exchangeOrderRepository.GetList(x => x.IsActive == true && (x.RegdNo != null ? x.RegdNo.Contains(regdNum) : false)).ToList();
                regdNumList = (tblExchangeOrders).AsEnumerable().Select(prodt => new SelectListItem() { Text = prodt.RegdNo, Value = prodt.RegdNo });
                regdNumList =  regdNumList.OrderBy(o => o.Text).ToList();
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("Home", "GetRegdNumByInput", ex);
            }
            var  result  =  new  SelectList(regdNumList, "Value", "Text");
            return  Json(result, JsonRequestBehavior.AllowGet);
        }

        public string ShowImagePath(string Base64StringValue)
        {
            string fileName = null;
            string filePath = null;
            DateTime _dateTime = DateTime.Now;
            try
            {
                string baseUrl = ConfigurationManager.AppSettings["BaseURL"].ToString();
        /*        string rootPath = @HostingEnvironment.ApplicationPhysicalPath;*/
                string imagePath = ConfigurationManager.AppSettings["ExchangePODView"].ToString();
                

                if (Base64StringValue != null && Base64StringValue != "")
                {
                    byte[] bytes = System.Convert.FromBase64String(Base64StringValue);
                    fileName = _dateTime.ToString("yyyyMMddHHmmssFFF") + Path.GetExtension("image.jpeg");
                    filePath = baseUrl + imagePath + fileName;
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("HomeController", "ShowImagePath", ex);
            }
            return filePath;
        }

        [HttpPost]
        public ActionResult ExchangePOD(string regNumber, string Base64StringValue, string podUrl)
        {
            _utilityManager = new UtilityManager();
            int result = 0;
            string message = string.Empty;
            string rootPath = @HostingEnvironment.ApplicationPhysicalPath;

            string fileName = podUrl.Split('/').LastOrDefault();
            string filePathLocal = podUrl.Replace(fileName, "");
            string baseUrl = ConfigurationManager.AppSettings["BaseURL"].ToString();
            string imagePath = ConfigurationManager.AppSettings["ExchangePODView"].ToString();
            string filePathLive = baseUrl + imagePath;
            try
            {
                if (regNumber != null && regNumber != "" && Base64StringValue != null && Base64StringValue != "" && podUrl != null && podUrl != "" && fileName != null && fileName != "")
                {
                    if (filePathLocal == filePathLive)
                    {
                        result = _utilityManager.ManageExchangePOD(regNumber, podUrl);
                        if (result > 0)
                        {
                            byte[] bytes = System.Convert.FromBase64String(Base64StringValue);
                            string filePath = rootPath + ConfigurationManager.AppSettings["ExchangePOD"].ToString() + fileName;
                            System.IO.File.WriteAllBytes(filePath, bytes);
                            message = "Exchange PoD image saved successfully.";
                            TempData["Msg"] = message;
                        }
                        else
                        {
                            message = "Invalid Regd Number, PoD image is not saved";
                            TempData["Error"] = message;
                        }
                    }
                    else
                    {
                        message = "Invalid PoD URL, PoD image is not saved";
                        TempData["Error"] = message;
                    }
                }
                else
                {
                    message = "All fields are required, PoD image is not saved";
                    TempData["Error"] = message;
                }
            }
            catch (Exception ex)
            {
                message = "Some error occurred, PoD image is not saved";
                TempData["Msg"] = message;
                LibLogging.WriteErrorToDB("HomeController", "ExchangePOD", ex);
                return RedirectToAction("Details");
            }
            return RedirectToAction("ExchangePOD", "Home");
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
                LibLogging.WriteErrorToDB("HomeController", "Details", ex);
            }
            return View();
        }
        #endregion

        [HttpGet]
        public ActionResult FB(string rn)
        {
            return Redirect("~/FeedBack/GiveFB?regno=" + rn);
        }

        [HttpGet]
        public ActionResult CustInfo(string rn)
        {
            return Redirect("~/Exchange/CustomerDetails?RegNo=" + rn);
        }

        [HttpGet]
        public ActionResult LodhaGroup(int Buid,int Bpid)
        {
            // Create a new request object for the redirect URL
            string RedirectionURL = ConfigurationManager.AppSettings["BaseURL"].ToString()+ "IsDtoC/ProductDetailsForD2C?BUID="+Buid+"&&/BPID="+Bpid;
            HttpWebRequest redirectRequest = (HttpWebRequest)WebRequest.Create(RedirectionURL);

            // Set the headers for the redirect request
            redirectRequest.Headers.Add("Third-Party", "BelleVie");
            redirectRequest.Headers.Add("BelleVie-IV", "z29pujVTLzIs7h6y");
            redirectRequest.Headers.Add("BelleVie-Data", "D2E0D8AB8454902E09ED51A18A2A527E141F0EC1F887BA9EEFD758E0B182EEAE9EB382106EB10E0372384D688C586F5D9F5BD903FA77353D34921D61C22A5F5EE72E7C09BF028060BAE298F20C5B4AED59989489E96E4571D0E80C3D2CEE5229C77112ACB8B1B26BD1E72EE3CA388F58");

            // Redirect to the new URL
            Response.StatusCode = (int)HttpStatusCode.Redirect;
           //Response.Headers.Add("Location", RedirectionURL);
            Response.End();
            return new EmptyResult();
        }

        [HttpGet]
        public ActionResult RV(int id)
        {
            return Redirect("~/Voucher/GetVoucherInstant?id=" + id);
        }

        [HttpGet]
        public ActionResult RVC(int id)
        {
            return Redirect("~/Voucher/GetVoucherRedemptionCash?id=" + id);
        }

        [HttpGet]
        public ActionResult GetDecryptedValue()
        {
            Decryptdatacontract decryptdatacontract = new Decryptdatacontract();
            try
            {
                
                decryptdatacontract.EncryptionKey = ConfigurationManager.AppSettings["ERPEncryptionKey"].ToString();
            }
            catch(Exception ex)
            {
                LibLogging.WriteErrorToDB("GetDecryptedValue", "Home", ex);
            }
            return View(decryptdatacontract);
        }
        [HttpGet]
        public JsonResult GetDectypteddata(string Encrypted)
        {
            string DecryptedData = string.Empty;
            string ApiKey = null;
            try
            {
                ApiKey = ConfigurationManager.AppSettings["ERPEncryptionKey"].ToString();
                using (AesManaged aes = new AesManaged())
                {
                    // Decrypt the bytes to a string.    
                     DecryptedData = AES256encoding.DecryptStringne(Encrypted, ApiKey);
                }
            }
            catch(Exception ex)
            {
                LibLogging.WriteErrorToDB("GetDectypteddata", "Home", ex);
            }
            return Json(DecryptedData, JsonRequestBehavior.AllowGet);
        }

        
        [HttpGet]
        public JsonResult GetEncryptStringERP(string RawData)
        {
            string EncryptedData = string.Empty;
            string ApiKey = null;
            try
            {
                ApiKey = ConfigurationManager.AppSettings["ERPEncryptionKey"].ToString();
                using (AesManaged aes = new AesManaged())
                {  
                    EncryptedData = AES256encoding.EncryptStringERP(RawData, ApiKey);
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("GetEncryptStringERP", "Home", ex);
            }
            return Json(EncryptedData, JsonRequestBehavior.AllowGet);
        }
    }
}
