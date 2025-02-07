using GraspCorn.Common.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Mailjet.Client.TransactionalEmails.Response;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Mailjet.Client.TransactionalEmails;
using Newtonsoft.Json.Linq;
using System.IO;
using RDCEL.DocUpload.DataContract.Templates;

namespace RDCEL.DocUpload.BAL.Common
{
    public class MailManager
    {
        //Configuration configuration = new Configuration();
        /// <summary>
        /// Send Email Locally
        /// </summary>
        /// <param name="to">To</param>
        /// <param name="body">Body</param>
        /// <param name="subject">Subject</param>
        public async Task SendEmailLocally(string to, string body, string subject)
        {

            try
            {
                int EnableMail = Convert.ToInt32(ConfigurationManager.AppSettings["EnableMail"]);
                TransactionalEmailResponse task = await JetMailSend(to, body, subject);
                if (task != null)
                {

                }
                if (EnableMail == 1)
                {
                    string bccEmailAddress = ConfigurationManager.AppSettings["BCCEmail"].ToString();
                    string SupportDisplayName = ConfigurationManager.AppSettings["SupportDisplayName"].ToString();
                    string fromEmail = ConfigurationManager.AppSettings["FromEmail"].ToString();
                    string userName = ConfigurationManager.AppSettings["UserName"].ToString();
                    string password = ConfigurationManager.AppSettings["Password"].ToString();
                    string hostName = ConfigurationManager.AppSettings["HostName"].ToString();
                    int portNumber = Convert.ToInt32(ConfigurationManager.AppSettings["PortNumber"]);
                    bool isSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["IsSSL"]);
                    bool UseDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings["UseDefaultCredentials"]);
                    MailMessage msg = new MailMessage();
                    msg.To.Add(new MailAddress(to));
                    msg.Bcc.Add(new MailAddress(bccEmailAddress));
                    msg.From = new MailAddress(fromEmail, SupportDisplayName);
                    msg.Subject = subject;
                    msg.Body = body;
                    msg.IsBodyHtml = true;
                    SmtpClient client = new SmtpClient();
                    client.Host = hostName;
                    client.UseDefaultCredentials = UseDefaultCredentials;
                    client.Credentials = new System.Net.NetworkCredential(userName, password);
                    client.Port = portNumber;
                    client.EnableSsl = isSSL;

                    client.Send(msg);
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("MailManager", "SendEmailLocally", ex);
            }

        }

        /// <summary>
        /// Send mail to multiple address
        /// </summary>
        /// <param name="to">To Email</param>
        /// <param name="body">Message Body</param>
        /// <param name="subject">Subject</param>
        public void SendEmailLocallyToMultipleAddress(string to, string body, string subject)
        {
            try
            {
                int EnableMail = Convert.ToInt32(ConfigurationManager.AppSettings["EnableMail"]);

                if (EnableMail == 1)
                {
                    string bccEmailAddress = ConfigurationManager.AppSettings["BCCEmail"].ToString();
                    string SupportDisplayName = ConfigurationManager.AppSettings["SupportDisplayName"].ToString();
                    string userName = ConfigurationManager.AppSettings["UserName"].ToString();
                    string password = ConfigurationManager.AppSettings["Password"].ToString();
                    string hostName = ConfigurationManager.AppSettings["HostName"].ToString();
                    int portNumber = Convert.ToInt32(ConfigurationManager.AppSettings["PortNumber"]);
                    bool isSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["IsSSL"]);
                    MailMessage msg = new MailMessage();

                    List<string> toList = to.Split(',').ToList();
                    if (toList != null && toList.Count > 0)
                    {
                        foreach (string item in toList)
                            msg.To.Add(new MailAddress(item));
                    }

                    msg.Bcc.Add(new MailAddress(bccEmailAddress));
                    msg.From = new MailAddress(userName, SupportDisplayName);
                    msg.Subject = subject;
                    msg.Body = body;
                    msg.IsBodyHtml = true;
                    SmtpClient client = new SmtpClient();
                    client.Host = hostName;
                    client.Credentials = new System.Net.NetworkCredential(userName, password);
                    client.Port = portNumber;
                    client.EnableSsl = isSSL;
                    client.Send(msg);
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("MailManager", "SendEmailLocallyToMultipleAddress", ex);
            }

        }

        /// <summary>
        /// Send Email Locally
        /// </summary>
        /// <param name="to">To</param>
        /// <param name="body">Body</param>
        /// <param name="subject">Subject</param>
        public bool SendEmailLocallyWithConfirmation(string to, string body, string subject)
        {
            bool flag = false;
            try
            {
                int EnableMail = Convert.ToInt32(ConfigurationManager.AppSettings["EnableMail"]);
                if (EnableMail == 1)
                {
                    string bccEmailAddress = ConfigurationManager.AppSettings["BCCEmail"].ToString();
                    string SupportDisplayName = ConfigurationManager.AppSettings["SupportDisplayName"].ToString();
                    string fromEmail = ConfigurationManager.AppSettings["FromEmail"].ToString();
                    string userName = ConfigurationManager.AppSettings["UserName"].ToString();
                    string password = ConfigurationManager.AppSettings["Password"].ToString();
                    string hostName = ConfigurationManager.AppSettings["HostName"].ToString();
                    int portNumber = Convert.ToInt32(ConfigurationManager.AppSettings["PortNumber"]);
                    bool isSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["IsSSL"]);
                    bool UseDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings["UseDefaultCredentials"]);

                    MailMessage msg = new MailMessage();
                    msg.To.Add(new MailAddress(to));
                    msg.Bcc.Add(new MailAddress(bccEmailAddress));
                    msg.From = new MailAddress(fromEmail, SupportDisplayName);
                    msg.Subject = subject;
                    msg.Body = body;
                    msg.IsBodyHtml = true;
                    SmtpClient client = new SmtpClient();
                    client.Host = hostName;
                    client.UseDefaultCredentials = UseDefaultCredentials;
                    client.Credentials = new System.Net.NetworkCredential(userName, password);
                    //client.Port = portNumber;
                    //client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.EnableSsl = isSSL;
                    //client.UseDefaultCredentials = false;
                    client.Send(msg);
                    flag = true;

                }
            }
            catch (Exception ex)
            {
                flag = false;
                LibLogging.WriteErrorToDB("MailManager", "SendEmailLocally", ex);
            }
            return flag;
        }


        public async Task<TransactionalEmailResponse> JetMailSend(string to, string body, string subject)
        {
            
            TransactionalEmailResponse response = null;
            string MailjetAPIKey = ConfigurationManager.AppSettings["MailjetAPIKey"];
            string MailjetAPISecret = ConfigurationManager.AppSettings["MailjetAPISecret"];

            if (to.Length > 0)
            {
                bool flag=false;
                MailjetClient client = new MailjetClient(MailjetAPIKey, MailjetAPISecret);

                MailjetRequest request = new MailjetRequest
                {
                    Resource = Send.Resource
                };

                List<SendContact> contactList = new List<SendContact>();
                to = to.Replace(",", ";");
                String[] recipient = to.Split(';');
                for (int n = 0; n < recipient.Length; n++)
                    contactList.Add(new SendContact(recipient[n].ToString()));


                List<SendContact> bccContactList = new List<SendContact>();
                string BccEmailAddress = ConfigurationManager.AppSettings["BCCEmail"].ToString();
                if (BccEmailAddress != null)
                {
                    BccEmailAddress = BccEmailAddress.Replace(",", ";");
                    String[] BccEmailAddresses = BccEmailAddress.Split(';');
                    for (int n = 0; n < BccEmailAddresses.Length; n++)
                        bccContactList.Add(new SendContact(BccEmailAddresses[n].ToString()));
                }

                // construct your email with builder
                //.WithReplyTo(new SendContact(this.SenderEmail, this.DisplayName))
                var email = new TransactionalEmail();
                string FromEmail = ConfigurationManager.AppSettings["FromEmail"].ToString();
                email.From = new SendContact(FromEmail, ConfigurationManager.AppSettings["SupportDisplayName"].ToString());
                email.ReplyTo = new SendContact(FromEmail, ConfigurationManager.AppSettings["SupportDisplayName"].ToString());
                email.To = contactList;

                if (bccContactList != null && bccContactList.Count > 0)
                    email.Bcc = bccContactList;

                email.Subject = subject;
                email.HTMLPart = body;

                string dflag = flag.ToString();
                try
                {
                    // invoke API to send email
                    response = await client.SendTransactionalEmailAsync(email);//SendTransactionalEmailAsync(email);
                    if (response != null && response.Messages != null && response.Messages.Length > 0 && !string.IsNullOrEmpty(response.Messages[0].Status) && response.Messages[0].Status.ToLower().Equals("success"))
                        flag = true;
                }
                catch (Exception ex)
                {
                    LibLogging.WriteErrorToDB("MailManager", "JetMailSend", ex);
                    flag = false;
                }

            }
            return response;
        }

       public  static async Task RunAsync()
        {
            string MailjetAPIKey = ConfigurationManager.AppSettings["MailjetAPIKey"];
            string MailjetAPISecret = ConfigurationManager.AppSettings["MailjetAPISecret"];
            MailjetClient client = new MailjetClient(MailjetAPIKey, MailjetAPISecret);

            //{
            //    BaseAdress = "ApiVersion.V3_1",
            //};
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
             .Property(Send.Messages, new JArray {
     new JObject {
      {
       "From",
       new JObject {
        {"Email", "himanshu@utcdigital.com"},
        {"Name", "Himanshu"}
       }
      }, {
       "To",
       new JArray {
        new JObject {
         {
          "Email",
          "himanshu@utcdigital.com"
         }, {
          "Name",
          "Himanshu"
         }
        }
       }
      }, {
       "Subject",
       "Greetings from Mailjet."
      }, {
       "TextPart",
       "My first Mailjet email"
      }, {
       "HTMLPart",
       "<h3>Dear passenger 1, welcome to <a href='https://www.mailjet.com/'>Mailjet</a>!</h3><br />May the delivery force be with you!"
      }, {
       "CustomID",
       "AppGettingStartedTest"
      }
     }
             });
            MailjetResponse response = await client.PostAsync(request);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine(string.Format("Total: {0}, Count: {1}\n", response.GetTotal(), response.GetCount()));
                Console.WriteLine(response.GetData());
            }
            else
            {
                Console.WriteLine(string.Format("StatusCode: {0}\n", response.StatusCode));
                Console.WriteLine(string.Format("ErrorInfo: {0}\n", response.GetErrorInfo()));
                Console.WriteLine(response.GetData());
                Console.WriteLine(string.Format("ErrorMessage: {0}\n", response.GetErrorMessage()));
            }
        }

        #region Create send mail async
        public void SendEmailLocallyAsync(string to, string body, string subject)
        {
            try
            {
                int EnableMail = Convert.ToInt32(ConfigurationManager.AppSettings["EnableMail"]);
                if (EnableMail == 1)
                {
                    string bccEmailAddress = ConfigurationManager.AppSettings["BCCEmail"].ToString();
                    string SupportDisplayName = ConfigurationManager.AppSettings["SupportDisplayName"].ToString();
                    string fromEmail = ConfigurationManager.AppSettings["FromEmail"].ToString();
                    string userName = ConfigurationManager.AppSettings["UserName"].ToString();
                    string password = ConfigurationManager.AppSettings["Password"].ToString();
                    string hostName = ConfigurationManager.AppSettings["HostName"].ToString();
                    int portNumber = Convert.ToInt32(ConfigurationManager.AppSettings["PortNumber"]);
                    bool isSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["IsSSL"]);
                    bool UseDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings["UseDefaultCredentials"]);
                    MailMessage msg = new MailMessage();
                    msg.To.Add(new MailAddress(to));
                    msg.Bcc.Add(new MailAddress(bccEmailAddress));
                    msg.From = new MailAddress(fromEmail, SupportDisplayName);
                    msg.Subject = subject;
                    msg.Body = body;
                    msg.IsBodyHtml = true;
                    SmtpClient client = new SmtpClient();
                    client.Host = hostName;
                    client.UseDefaultCredentials = UseDefaultCredentials;
                    client.Credentials = new System.Net.NetworkCredential(userName, password);
                    client.Port = portNumber;
                    client.EnableSsl = isSSL;
                    client.SendMailAsync(msg);

                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("MailManager", "SendEmailLocallyAsync", ex);
            }
        }
        #endregion

        #region Mail Send with Attatchment using JetMail Added By VK
        public async Task<bool> JetMailSendWithAttachment(TemplateDataContract templateDC)
        {
            bool flag = false;
            TransactionalEmailResponse response = new TransactionalEmailResponse();

            if (templateDC != null && templateDC.To.Length > 0)
            {
                try
                {
                    string MailjetAPIKey = ConfigurationManager.AppSettings["MailjetAPIKey"].ToString();
                    string MailjetAPISecret = ConfigurationManager.AppSettings["MailjetAPISecret"].ToString();
                    string FromEmail = ConfigurationManager.AppSettings["FromEmail"].ToString();
                    string FromDisplayName = ConfigurationManager.AppSettings["FromDisplayName"].ToString();
                    MailjetClient client = new MailjetClient(MailjetAPIKey, MailjetAPISecret);
                    MailjetRequest request = new MailjetRequest
                    {
                        Resource = Send.Resource
                    };

                    // Mail Send To
                    List<SendContact> contactList = new List<SendContact>();
                    templateDC.To = templateDC.To.Replace(",", ";");
                    String[] recipient = templateDC.To.Split(';');
                    for (int n = 0; n < recipient.Length; n++)
                        contactList.Add(new SendContact(recipient[n].Trim().ToString()));

                    // Add CC
                    List<SendContact> ccContactList = new List<SendContact>();
                    if (templateDC.Cc != null && templateDC.Cc.Length > 0)
                    {
                        templateDC.Cc = templateDC.Cc.Replace(",", ";");
                        String[] ccEmailAddresses = templateDC.Cc.Split(';');
                        for (int n = 0; n < ccEmailAddresses.Length; n++)
                            ccContactList.Add(new SendContact(ccEmailAddresses[n].Trim().ToString()));
                    }

                    //Add Bcc
                    List<SendContact> bccContactList = new List<SendContact>();
                    if (templateDC.Bcc != null && templateDC.Bcc.Length > 0)
                    {
                        templateDC.Bcc = templateDC.Bcc.Replace(",", ";");
                        String[] BccEmailAddresses = templateDC.Bcc.Split(';');
                        for (int n = 0; n < BccEmailAddresses.Length; n++)
                            bccContactList.Add(new SendContact(BccEmailAddresses[n].Trim().ToString()));
                    }
                    var email = new TransactionalEmail();
                    email.From = new SendContact(FromEmail, FromDisplayName);
                    email.ReplyTo = new SendContact(FromEmail, FromDisplayName);
                    email.To = contactList;

                    List<Mailjet.Client.TransactionalEmails.Attachment> AttachmentsList = new List<Mailjet.Client.TransactionalEmails.Attachment>();
                    // Add Attachment ABB Customer Invoice
                    if (templateDC.IsInvoiceGenerated == true)
                    {
                        var attachmentsT_CPath = string.Concat(templateDC.InvAttachFilePath);
                        if (Directory.Exists(attachmentsT_CPath))
                        {
                            string[] filesTC = Directory.GetFiles(attachmentsT_CPath, templateDC.InvAttachFileName);
                            foreach (string fileName in filesTC)
                            {
                                Byte[] bytes = File.ReadAllBytes(fileName);
                                String file = Convert.ToBase64String(bytes);
                                AttachmentsList.Add(new Mailjet.Client.TransactionalEmails.Attachment(templateDC.InvAttachFileName, ".pdf", file));
                            }
                        }
                    }
                    if (AttachmentsList != null && AttachmentsList.Count > 0)
                    {
                        email.Attachments = AttachmentsList;
                    }

                    if (ccContactList != null && ccContactList.Count > 0)
                        email.Cc = ccContactList;

                    if (bccContactList != null && bccContactList.Count > 0)
                        email.Bcc = bccContactList;

                    email.Subject = templateDC.Subject;
                    email.HTMLPart = templateDC.HtmlBody;

                    // invoke API to send email
                    response = await client.SendTransactionalEmailAsync(email);
                    if (response != null && response.Messages != null && response.Messages.Length > 0 && !string.IsNullOrEmpty(response.Messages[0].Status) && response.Messages[0].Status.ToLower().Equals("success"))
                        flag = true;
                }
                catch (Exception ex)
                {
                    flag = false;
                    LibLogging.WriteErrorToDB("MailManager", "JetMailSendWithAttachment", ex);
                }
            }
            return flag;
        }
        #endregion
    }
}
