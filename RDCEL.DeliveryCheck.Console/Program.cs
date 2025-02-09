using GraspCorn.Common.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RDCEL.DocUpload.BAL.Common;
using RDCEL.DocUpload.BAL.ServiceCall;
using RDCEL.DocUpload.BAL.SponsorsApiCall;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DataContract.Common;

namespace RDCEL.DeliveryCheck.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Program obj = new Program();
            System.Net.ServicePointManager.SecurityProtocol =
            SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;



            ExchangeOrderManager _exchangeOrderManager = new ExchangeOrderManager();
            try
            {
                //MailManager.RunAsync().Wait();
                //SendCredentials(1);
                ResizeandMove();
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("Program", "Main", ex);
            }
            //_exchangeOrderManager.PushMessgeForConfirmDelivery(5);
        }


        #region Method to move data from source location to destination location with resize
        public static void ResizeandMove()
        {
            string sourcepath = ConfigurationManager.AppSettings["SourcePath"];
            string destinationpath = ConfigurationManager.AppSettings["DestinationPath"];
            string filename = string.Empty;
            try
            {

                string[] filePaths = Directory.GetFiles(sourcepath, "*");

                for (int i = 0; i < filePaths.Length; i++)
                {
                    string sourceimagepatha = string.Concat(filePaths[i].ToString());
                    filename = Path.GetFileName(sourceimagepatha);
                    if (filePaths[i].ToLower().Contains(".png") || filePaths[i].ToLower().Contains(".jpg"))
                    {
                        try
                        {
                            System.Drawing.Image img = System.Drawing.Image.FromFile(sourceimagepatha);
                            System.Drawing.Image resizeimage = resizeImage(img, new Size(200, 200));
                            resizeimage.Save(destinationpath + filename);
                        }
                        catch (Exception)
                        {
                            if (File.Exists(sourceimagepatha))
                            {
                                string ext = Path.GetExtension(sourceimagepatha);
                                filename = filename.Replace(ext, ".pdf");
                                File.Copy(sourceimagepatha, destinationpath + filename);
                            }
                        }

                    }
                    else
                    {
                        if (File.Exists(sourceimagepatha))
                        {
                            File.Copy(sourceimagepatha, destinationpath + filename);
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("Program", "ResizeandMove", ex);
            }
        }

        private static System.Drawing.Image resizeImage(System.Drawing.Image imgToResize, Size size)
        {
            //Get the image current width  
            int sourceWidth = imgToResize.Width;
            //Get the image current height  
            int sourceHeight = imgToResize.Height;
            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;
            //Calulate  width with new desired size  
            nPercentW = ((float)size.Width / (float)sourceWidth);
            //Calculate height with new desired size  
            nPercentH = ((float)size.Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;
            //New Width  
            int destWidth = (int)(sourceWidth * nPercent);
            //New Height  
            int destHeight = (int)(sourceHeight * nPercent);
            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((System.Drawing.Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            // Draw image with new width and height  
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();
            return (System.Drawing.Image)b;
        }
        #endregion

        #region Move unmoved orders from UTC DB to Creator

        /// <summary>
        /// Method to move data from UTC DB to Creator
        /// </summary>
        /// <returns>bool</returns>
        public bool MoveOrdersFromUTCToCreator()
        {
            bool flag = false;
            try
            {
                #region Code to move ABB Orders from UTC DB to Creator 
                //1. Fetch  list of order from DB which are not moved to Creator

                //2. Move list object to Creator one by one using loop
                #endregion
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("Program", "MoveOrdersFromUTCToCreator", ex);
            }
            return flag;
        }
        #endregion

        #region Method to send email for login access detail

        public static void SendCredentials(int buid)
        {
            string subject = "Login Credentials";
            tblBusinessPartner businessPartnerObj;
            MailManager _mailManager = new MailManager();
            BusinessPartnerRepository _businessPartnerRepository = new BusinessPartnerRepository();
            List<string> emailList = new List<string>();
            List<tblBusinessPartner> businessPartnerList = _businessPartnerRepository.GetList(x => x.IsActive == true && x.BusinessUnitId == buid && x.IsExchangeBP == true).ToList();
            if (businessPartnerList != null)
            {
                int count = 0;
                foreach (var item in businessPartnerList)
                {
                    if (count == 1)
                        break;
                    if (!emailList.Contains(item.Email))
                    {
                        businessPartnerObj = new tblBusinessPartner();
                        businessPartnerObj.Email = item.Email;
                        businessPartnerObj.BPPassword = UniqueString.RandomNumberByLength(8);
                        string body = EmailBody();
                        body.Replace("[VoucherURL]", "https://utcbridge.com/UTCAPI/Voucher/Login");
                        body.Replace("[UserName]", businessPartnerObj.Email);
                        body.Replace("[Password]", businessPartnerObj.AssociateCode);
                        //_mailManager.SendEmailLocally(businessPartnerObj.Email.Trim(), EmailBody(), subject);
                        Task task = _mailManager.SendEmailLocally("himanshu@utcdigital.com", EmailBody(), subject);
                        emailList.Add(item.Email.ToLower().Trim());
                    }
                    count++;
                }
            }

        }
        #endregion

        #region HTML for mail Body

        public static void SendMail()
        {
            MailJetViewModel mailJet = new MailJetViewModel();
            MailJetMessage message = new MailJetMessage();
            MailJetFrom from = new MailJetFrom();
            MailjetTo to = new MailjetTo();
            message.From = new MailJetFrom() { Email = "customercare@rdcel.com", Name = "Customer  Care" };
            message.To = new List<MailjetTo>();
            message.To.Add(new MailjetTo() { Email = "himanshu.pathak07@gmail.com", Name = "Himanshu" });
            message.Subject = "You Voucher Detail";
            message.HTMLPart = EmailBody();
            //BillCloudServiceCall.SendMail(message);
        }
        public static string EmailBody()
        {
            string htmlString = @"<p>Hi User,</p>
                            <p>&nbsp;</p>
                            <p>Greeting for the day!!!</p>
                            <p>&nbsp;</p>
                            <p>Please refer following access details and steps to access the Voucher Redemption:</p>
                            <p>&nbsp;</p>
                            <p><b>Access Detail:</b></p>
                            <p>&nbsp;</p>
                            <ul>
	                            <li>URL: [VoucherURL]</li>
	                            <li>User Name: [UserName]</li>
	                            <li>Password: [Password]</li>
                            </ul>
                            <p>&nbsp;</p>
                            <p><b>Steps to Access Redeem Voucher Page:</b></p>
                            <p>&nbsp;</p>
                            <ul>
	                            <li>Click on the above URL OR copy and paste the URL into your browser</li>
	                            <li>Enter user name, password and click on Login, you will get redirected to the Voucher redemption screen.</li>
                            </ul>
                            <p>&nbsp;</p>
                            <p><b><i>Note: Please save the above URL, username and password in your notes to access the voucher redemption page quickly.</i></b></p>
                            <p>&nbsp;</p>
                            <p>In case of any query, please drop mail at&nbsp;<a href='mailto:exchange@digimart.co.in' target='_blank'>exchange@digimart.co.in</a>&nbsp;</p>
                            <p>&nbsp;</p>
                            <p>Thank you.</p>";
            return htmlString;
        }
        #endregion
    }
}
