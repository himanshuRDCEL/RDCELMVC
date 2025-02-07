using GraspCorn.Common.Helper;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCEL.DocUpload.DAL.Repository;

namespace RDCEL.DocUpload.DAL.Helper
{
    public class Logging
    {
        #region Variable Declaration
        ErrorLogRepository errorLogRepository;
        //TicketStatusRepository ticketStatusRepository;
        // MailManager _mailManager;
        DateTime currentDatetime = DateTime.Now.TrimMilliseconds();

        #endregion

        /// <summary>
        /// Method to write error to DB
        /// </summary>
        /// <param name="Source">Source</param>
        /// <param name="Code">Code</param>
        /// <param name="ex">ex</param>
        public void WriteErrorToDB(string Source, string Code, string sponsorOrderNo, IRestResponse response = null)
        {
            errorLogRepository = new ErrorLogRepository();
            tblErrorLog errorLog = null;

            try
            {
                int addLog = Convert.ToInt32(ConfigurationManager.AppSettings["AddLog"]);

                if (addLog == 1)
                {
                    //string message = ex != null ? ex.Message : string.Empty;
                    //string stackTrace = ex != null ? ex.StackTrace : string.Empty;

                    string message = response != null ? response.ErrorMessage : string.Empty;
                    string content = response != null ? response.Content : string.Empty;

                    message = message + Environment.NewLine + "Content :" + content;

                    errorLog = new tblErrorLog();
                    errorLog.ClassName = Source;
                    errorLog.MethodName = Code;
                    errorLog.SponsorOrderNo = sponsorOrderNo;
                    errorLog.ErrorMessage = message;
                    errorLog.CreatedDate = DateTime.Now;
                    errorLogRepository.Add(errorLog);
                    errorLogRepository.SaveChanges();
                }

            }
            catch (Exception ex1)
            {
                //string message = ex != null ? ex1.Message : string.Empty;
                //string stackTrace = ex != null ? ex1.StackTrace : string.Empty;
                string ex= ex1.Message;
                string content = response != null ? response.Content : string.Empty;

                string message = "Content :" + content;

                errorLog = new tblErrorLog();
                errorLog.ClassName = Source;
                errorLog.MethodName = Code;
                errorLog.ErrorMessage = message;
                errorLog.CreatedDate = DateTime.Now;
                errorLogRepository.Add(errorLog);
                errorLogRepository.SaveChanges();
            }
        }

        /// <summary>
        /// Method to write error to DB
        /// </summary>
        /// <param name="Source">Source</param>
        /// <param name="Code">Code</param>
        /// <param name="ex">ex</param>
        public void WriteAPIRequestToDB(string Source, string Code, string sponsorOrderNo, string jsonString)
        {
            errorLogRepository = new ErrorLogRepository();
            tblErrorLog errorLog = null;

            try
            {
                

                    errorLog = new tblErrorLog();
                    errorLog.ClassName = Source;
                    errorLog.MethodName = Code;
                    errorLog.SponsorOrderNo = sponsorOrderNo;
                    errorLog.ErrorMessage = jsonString;
                    errorLog.CreatedDate = DateTime.Now;
                    errorLogRepository.Add(errorLog);
                    errorLogRepository.SaveChanges();
                

            }
            catch (Exception ex1)
            {
                string ex = ex1.Message;
                errorLog = new tblErrorLog();
                errorLog.ClassName = Source;
                errorLog.MethodName = Code;
                errorLog.ErrorMessage = jsonString;
                errorLog.CreatedDate = DateTime.Now;
                errorLogRepository.Add(errorLog);
                errorLogRepository.SaveChanges();
            }
        }
    }
}
