using GraspCorn.Common.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RDCEL.DocUpload.BAL.FeedBackManager;
using RDCEL.DocUpload.BAL.SponsorsApiCall;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DataContract.FeedBack;

namespace RDCEL.DocUpload.Web.API.Controllers
{
    public class FeedBackController : Controller
    {
        #region Variable Declaration
        CustomerDetailsRepository _customerDetailsRepository;
        FeedBackAnswerRepository _answerRepository;
        FeedBackQuestionsRepository _quesRepository;
        FeedBackRepository _feedBackRepository;
        ManageFeedBack _managefeedback;
        ExchangeOrderRepository _exchangeOrderRepository;
        #endregion
        // GET: FeedBack
        public ActionResult GiveFB(string regno)
        {
            _exchangeOrderRepository = new ExchangeOrderRepository();
            _answerRepository = new FeedBackAnswerRepository();
            _feedBackRepository = new FeedBackRepository();
            _quesRepository = new FeedBackQuestionsRepository();
            _customerDetailsRepository = new CustomerDetailsRepository();
            string message = string.Empty;

            FeedBackView feedbackViewDataContract = new FeedBackView();
            try
            {
                if (!string.IsNullOrEmpty(regno))
                {
                    tblExchangeOrder exchangeOrder = _exchangeOrderRepository.GetSingle(x => !string.IsNullOrEmpty(x.RegdNo) && x.RegdNo.ToLower().Equals(regno.ToLower()));
                    tblCustomerDetail custObj = _customerDetailsRepository.GetSingle(x => x.Id == exchangeOrder.CustomerDetailsId);
                    if (custObj != null)
                    {
                        feedbackViewDataContract.CustId = custObj.Id;
                        feedbackViewDataContract.ExchangeOrderId = exchangeOrder.Id;
                        List<tblFeedBackQuestion> quesObj = _quesRepository.GetList(x => x.IsActive.Equals(true)).ToList();
                        if (quesObj != null && quesObj.Count > 0)
                        {
                            foreach (var Quest in quesObj)
                            {
                                if (Quest.Id.Equals(1))
                                {
                                    feedbackViewDataContract.Question = Quest.Question;
                                }
                                else if (Quest.Id.Equals(2))
                                {
                                    feedbackViewDataContract.Question2 = Quest.Question;
                                }

                            }
                        }
                        else
                        {
                            message = "Sorry Can Not Share FeedBack You are Not Registered as User of Our Service";
                            if (!string.IsNullOrEmpty(message))
                                TempData["Msg"] = message;
                            else
                                TempData["Msg"] = "Some error occurred, please connect with the Administrator.";
                            return RedirectToAction("Details");
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("FeedBackController", "FeedBack", ex);
            }
            return View(feedbackViewDataContract);
        }
        [HttpPost]
        public ActionResult GiveFB(FeedBackView feedBackView)
        {
            _answerRepository = new FeedBackAnswerRepository();
            _feedBackRepository = new FeedBackRepository();
            _quesRepository = new FeedBackQuestionsRepository();
            _customerDetailsRepository = new CustomerDetailsRepository();
            _managefeedback = new ManageFeedBack();
            string message = string.Empty;
            try
            {
                FeedBackDataContract feedBackDataContract = new FeedBackDataContract();
                feedBackDataContract.CustomerId = feedBackView.CustId;
                feedBackDataContract.Comment = feedBackView.Comment;
                feedBackDataContract.AnswerId = feedBackView.AnsId;
                feedBackDataContract.QuestionId = feedBackView.QesId;
                feedBackDataContract.RatingNo = feedBackView.ratingId;
                feedBackDataContract.ExchangeOrderId = feedBackView.ExchangeOrderId;
                var result = _managefeedback.AddfeedbackToDB(feedBackDataContract);
                if (result >0)
                    message = "Thank you.Your feedback has been recieved successfully.";
                else
                    message = "Order not Created";

                if (!string.IsNullOrEmpty(message))
                    TempData["Msg"] = message;
                else
                    TempData["Msg"] = "Some error occurred, please connect with the Administrator.";
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("FeedBackController", "GiveFB", ex);
                return View();
            }

            return RedirectToAction("Details");
        }

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
    }
}