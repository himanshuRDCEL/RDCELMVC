using GraspCorn.Common.Enums;
using GraspCorn.Common.Helper;
using System;
using System.Web.Mvc;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DataContract.ABBRegistration;
using RDCEL.DocUpload.BAL;
using System.Configuration;

namespace RDCEL.DocUpload.Web.API.Controllers
{
    public class ERPController : Controller
    {

        #region Variable declaration
        EVCRegistrationRepository _EVCRegistrationRepository;
        ABBPaymentRepository _EVCPaymentRepository;
        ERPManager _ERPManager;
        #endregion
        // GET: ERP
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult EVCCompletePayment(ZaakPayResponseModel zaakPayResponseModel, int UserId)
        {
            try
            {
                _EVCRegistrationRepository = new EVCRegistrationRepository();
                _ERPManager = new ERPManager();

                string dbresponse = string.Empty;
                _EVCPaymentRepository = new ABBPaymentRepository();
                string msg = string.Empty;
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
                string[] orderIdParts = response.RegdNo.Split('_');
                string EVCregdNo = orderIdParts[1];
                response.RegdNo = EVCregdNo;
                dbresponse = _ERPManager.EVCPaymentstatusUpdate(response, UserId);
                //// Check payment made successfully
                #region sms Send
                if (zaakPayResponseModel.responseCode == Convert.ToInt32(ZaakPayPaymentStatus.successfull) && dbresponse == "success")
                {
                    tblEVCRegistration registrationObj = _EVCRegistrationRepository.GetSingle(x => x.EVCRegdNo == response.RegdNo);
                    if (registrationObj != null)
                    {

                        if (dbresponse == "success" && registrationObj != null)
                        {

                            msg = "Thank You " + registrationObj.EVCRegdNo + " " + registrationObj.BussinessName
                            + " Wallet has been recharged Successfully Transaction Id "
                            + response.transactionId + " for Wallet Amount " + response.amount + 
                            ". Your Current Wallet Balance is " + registrationObj.EVCWalletAmount;

                            //if (registrationObj.CustMobile != null && !string.IsNullOrEmpty(registrationObj.RegdNo))
                            //    SendSucessSMS(registrationObj.CustMobile, registrationObj.RegdNo);

                            if (!string.IsNullOrEmpty(msg))
                                TempData["Msg"] = msg;
                            // Create these action method

                        }
                    }
                }
                else
                {
                    msg = "Payment Is Failed  transactionId  " + response.transactionId + " EVC Reg Id  -" + response.RegdNo;
                    if (!string.IsNullOrEmpty(msg))
                        TempData["Msg"] = msg;
                }
                #endregion
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ERPController", "EVCCompletePayment", ex);
            }
            return RedirectToAction("Details");
        }
        // GET: ABB/Details/5
        public ActionResult Details()
        {
            string msg = string.Empty;
            string ERPEVCDashborad = ConfigurationManager.AppSettings["ERPEVCDashborad"].ToString() + "/EVC_Portal/EVC_Dashboard";
        

            try
            {
                if (TempData["Msg"] != null && !string.IsNullOrEmpty(TempData["Msg"].ToString()))
                    msg = TempData["Msg"].ToString();
                else
                    msg = "Some error occurred, please connect with the Administrator.";

                ViewBag.MSG = msg;
                ViewBag.ERPEVCDashborad = ERPEVCDashborad;

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ERPController", "Details", ex);
            }
            return View();
        }

        // GET: ABB/Details/5
        public ActionResult DetailsFailedOrder()
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
                LibLogging.WriteErrorToDB("ABBController", "Details", ex);
            }
            return View();
        }
    }
}