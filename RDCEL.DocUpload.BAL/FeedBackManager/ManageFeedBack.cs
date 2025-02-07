using GraspCorn.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DataContract.FeedBack;

namespace RDCEL.DocUpload.BAL.FeedBackManager
{
    public class ManageFeedBack
    {
        #region Declare Variable
        CustomerDetailsRepository _customerDetailsRepository;
        FeedBackAnswerRepository _answerRepository;
        FeedBackQuestionsRepository _quesRepository;
        FeedBackRepository _feedBackRepository;
        DateTime currentDatetime = DateTime.Now.TrimMilliseconds();
        #endregion

        #region send FeedBack To DataBase
        public int AddfeedbackToDB(FeedBackDataContract feedbackDC)
        {
            _answerRepository = new FeedBackAnswerRepository();
            _feedBackRepository = new FeedBackRepository();
            _quesRepository = new FeedBackQuestionsRepository();
            _customerDetailsRepository = new CustomerDetailsRepository();
            int result = 0;
            try
            {
                tblFeedBack feedBackObject = setFeedBackObjectJson(feedbackDC);
                {
                    _feedBackRepository.Add(feedBackObject);
                    _feedBackRepository.SaveChanges();
                    result = feedBackObject.Id;
                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ManageFeedBack", "AddfeedbackToDB", ex);
            }

            return result;
        }
        #endregion
        #region set feedBack obj
        /// <summary>
        /// Method to set Order info to table
        /// </summary>
        /// <param name="productOrderDataContract">productOrderDataContract</param>     
        public tblFeedBack setFeedBackObjectJson(FeedBackDataContract feedBackDC)
        {
            tblFeedBack feedBAckObj = null;
            try
            {
                if (feedBackDC != null)
                {
                    feedBAckObj = new tblFeedBack();
                    feedBAckObj.RatingNo = feedBackDC.RatingNo;
                    feedBAckObj.QuestionId = feedBackDC.QuestionId;
                    feedBAckObj.AnswerId = feedBackDC.AnswerId;
                    feedBAckObj.CustomerId = feedBackDC.CustomerId;
                    feedBAckObj.Comment = feedBackDC.Comment;
                    feedBAckObj.CreatedDate = currentDatetime;
                    feedBAckObj.ExchangeOrderId = feedBackDC.ExchangeOrderId;
                    feedBAckObj.CreatedBy = feedBackDC.CustomerId.ToString();
                    feedBAckObj.IsActive = true;
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ManageFeedBack", "setFeedBackObjectJson", ex);
            }
            return feedBAckObj;
        }
        #endregion
    }
}
