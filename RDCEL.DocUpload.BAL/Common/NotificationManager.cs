using GraspCorn.Common.Helper;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DataContract.ABBRegistration;
using RDCEL.DocUpload.DataContract.WhatsappTemplates;

namespace RDCEL.DocUpload.BAL.Common
{
    public class NotificationManager
    {
        /// <summary>
        /// Method to send notification message to client
        /// </summary>
        /// <param name="phoneNumber">phone Number</param>
        /// <param name="message">v</param>
        /// <returns>bool</returns>
        public bool SendNotificationSMS(string phoneNumber, string messages,string OTPCode = "")
        {
            String result;
            bool flag = false;
            string sender = null;
            TextLocalResponseViewModel TextLocalResponseVM = null;
            string apiKey = ConfigurationManager.AppSettings["SMSKey"].ToString();
            string numbers = phoneNumber; //Code to trim number , remove blanks

            string message = "Dear Customer - OTP for voucher generation for the AAA is 34343 by ROCKINGDEALS.";
           
            try
            {
                // message = message.Replace(" [OTP]", OTPValue);
                if (OTPCode != string.Empty && OTPCode!=null)
                {
                     sender = ConfigurationManager.AppSettings["SenderNameNew"].ToString();
                  
                }
                else
                {
                     sender = ConfigurationManager.AppSettings["SenderName"].ToString();
                }
                String url = "https://api.textlocal.in/send/?apikey=" + apiKey + "&numbers=" + numbers + "&message=" + message + "&sender=" + sender;
                //refer to parameters to complete correct url string

                StreamWriter myWriter = null;
                HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);

                objRequest.Method = "POST";
                objRequest.ContentLength = Encoding.UTF8.GetByteCount(url);
                objRequest.ContentType = "application/x-www-form-urlencoded";
                try
                {
                    myWriter = new StreamWriter(objRequest.GetRequestStream());
                    myWriter.Write(url);
                }
                catch (Exception e)
                {
                    LibLogging.WriteErrorToDB("NotoficationManager", "SendNotificationSMS", e);
                }
                finally
                {
                    myWriter.Close();
                }

                HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
                using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    TextLocalResponseVM = JsonConvert.DeserializeObject<TextLocalResponseViewModel>(result);
                    if (TextLocalResponseVM != null)
                        if (TextLocalResponseVM.status == "success")
                            flag = true;

                    // Close and clean up the StreamReader
                    sr.Close();
                }

                if (flag == true)
                {
                    using (MessageDetailRepository messageDetailRepository = new MessageDetailRepository())
                    {
                        tblMessageDetail messageDetailObj = new tblMessageDetail();
                        messageDetailObj.ResponseJSON = string.Empty;
                        messageDetailObj.Code = OTPCode;
                        messageDetailObj.PhoneNumber = phoneNumber;
                        messageDetailObj.SendDate = DateTime.Now; ;
                        messageDetailObj.Message = message;
                        messageDetailObj.IsUsed = false;
                        messageDetailRepository.Add(messageDetailObj);
                        messageDetailRepository.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("NotoficationManager", "SendNotificationSMS", ex);
            }
            return flag;
        }

        /// <summary>
        /// Method to validate OTP
        /// </summary>
        /// <param name="phoneNumber">phone Number</param>
        /// <param name="OTP">OTP</param>
        /// <returns>bool</returns>
        public bool ValidateOTP(string phoneNumber, string OTP)
        {
            MessageDetailRepository _messageDetailRepository = new MessageDetailRepository();
            string response = string.Empty;
            string Message = string.Empty;
            bool flag = false;
            tblMessageDetail messageDetail = null;


            try
            {
                messageDetail = _messageDetailRepository.GetSingle(x => x.PhoneNumber.Equals(phoneNumber) && (x.Code != null && x.Code.Equals(OTP)) && (x.IsUsed == false));

                DateTime start = DateTime.Now;
                if (messageDetail != null)
                {
                    DateTime oldDate = Convert.ToDateTime(messageDetail.SendDate);

                    int minDiff = Convert.ToInt32(ConfigurationManager.AppSettings["OTPActivatedMin"].ToString());
                    if (start.Subtract(oldDate) <= TimeSpan.FromMinutes(minDiff))
                    flag = true;
                    messageDetail.IsUsed = true;
                    _messageDetailRepository.Update(messageDetail);
                    _messageDetailRepository.SaveChanges();
                }
               
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("NotoficationManager", "ValidateOTP", ex);
            }
            return flag;
        }


        public bool SendNotificationSMSNewSender(string phoneNumber, string message, string OTPCode = "")
        {
            String result;
            bool flag = false;
            string sender = null;
            TextLocalResponseViewModel TextLocalResponseVM = null;
            string apiKey = ConfigurationManager.AppSettings["SMSKey"].ToString();
            string numbers = phoneNumber; //Code to trim number , remove blanks
            try
            {
                //string message = "Dear Customer - OTP for registration for the UTC Assured Buyback Program is " + OTPValue + " by TUTC";
                // message = message.Replace(" [OTP]", OTPValue);
                
                    sender = ConfigurationManager.AppSettings["SenderNameNew"].ToString();

                
                String url = "https://api.textlocal.in/send/?apikey=" + apiKey + "&numbers=" + numbers + "&message=" + message + "&sender=" + sender;
                //refer to parameters to complete correct url string

                StreamWriter myWriter = null;
                HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);

                objRequest.Method = "POST";
                objRequest.ContentLength = Encoding.UTF8.GetByteCount(url);
                objRequest.ContentType = "application/x-www-form-urlencoded";
                try
                {
                    myWriter = new StreamWriter(objRequest.GetRequestStream());
                    myWriter.Write(url);
                }
                catch (Exception e)
                {
                    LibLogging.WriteErrorToDB("NotoficationManager", "SendNotificationSMS", e);
                }
                finally
                {
                    myWriter.Close();
                }

                HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
                using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    TextLocalResponseVM = JsonConvert.DeserializeObject<TextLocalResponseViewModel>(result);
                    if (TextLocalResponseVM != null)
                        if (TextLocalResponseVM.status == "success")
                            flag = true;

                    // Close and clean up the StreamReader
                    sr.Close();
                }

                if (flag == true)
                {
                    using (MessageDetailRepository messageDetailRepository = new MessageDetailRepository())
                    {
                        tblMessageDetail messageDetailObj = new tblMessageDetail();
                        messageDetailObj.ResponseJSON = string.Empty;
                        messageDetailObj.Code = OTPCode;
                        messageDetailObj.PhoneNumber = phoneNumber;
                        messageDetailObj.SendDate = DateTime.Now; ;
                        messageDetailObj.Message = message;
                        messageDetailObj.IsUsed = false;
                        messageDetailRepository.Add(messageDetailObj);
                        messageDetailRepository.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("NotoficationManager", "SendNotificationSMS", ex);
            }
            return flag;
        }

    }
}
