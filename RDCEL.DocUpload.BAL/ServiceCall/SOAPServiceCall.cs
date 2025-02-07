using GraspCorn.Common.Constant;
using GraspCorn.Common.Helper;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using RDCEL.DocUpload.DAL.Repository;

namespace RDCEL.DocUpload.BAL.ServiceCall
{
    
    public class SOAPServiceCall
    {
        
        /// <summary>
        /// Method to invoke SOP service
        /// </summary>
        /// <param name="strMessage"></param>
        /// <returns>HttpWebResponse</returns>
        public static string SOAP_InvokeServiceForDaikin(string xmlMessage, string sopURL,string sopPassword)
        {
          
            string ResultString = string.Empty;
            string UserName = null;
            try
            {
                UserName = ConfigurationManager.AppSettings["DaikinUserName"].ToString();
               // CallWebService(xmlMessage, sopURL, sopPassword);
                //xmlMessage = !string.IsNullOrEmpty(xmlMessage) ? XMLSerializer.ConvertXMLtoReqString(xmlMessage) : string.Empty;

                string XMlString = SOAPConstant.Diakin_Envelop;
                XMlString = XMlString.Replace("[MessageBody]", xmlMessage);


                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(sopURL);
                string usernamePassword = UserName + ":" + sopPassword;
                usernamePassword = Convert.ToBase64String(new ASCIIEncoding().GetBytes(usernamePassword)); // <--- here.
                //CredentialCache mycache = new CredentialCache();
                //myReq.Credentials = mycache;
                req.Headers.Add("Authorization", "Basic " + usernamePassword);
                req.ContentType = "text/xml";
                req.Accept = "*/*";
                req.Method = "POST";

                using (Stream stm = req.GetRequestStream())
                {
                    using (StreamWriter stmw = new StreamWriter(stm))
                    {
                        stmw.Write(XMlString);
                    }
                }

                using (StreamReader responseReader = new StreamReader(req.GetResponse().GetResponseStream()))
                {
                    HttpStatusCode statusCode = ((HttpWebResponse)req.GetResponse()).StatusCode;
                    if(statusCode==System.Net.HttpStatusCode.OK)
                    {
                        ResultString = responseReader.ReadToEnd();
                    }
                    //XMLDoc = new XmlDocument();
                    //XMLDoc.LoadXml(result);
                    //ResultXML = XDocument.Parse(result);
                    //ResultString = result;
                }


            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("PastelServiceCall", "SOP_InvokeService", ex);
            }
            return ResultString;
        }

        public static void CallWebService(string xmlMessage, string sopURL,string soapPassword)
        {
            XmlDocument soapEnvelopeXml = CreateSoapEnvelope(xmlMessage);
            HttpWebRequest webRequest = CreateWebRequest(sopURL,soapPassword);
            InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

            // begin async call to web request.
            IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);

            // suspend this thread until call is complete. You might want to
            // do something usefull here like update your UI.
            asyncResult.AsyncWaitHandle.WaitOne();

            // get the response from the completed web request.
            string soapResult;
            using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
            {
                using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                {
                    soapResult = rd.ReadToEnd();
                }
                Console.Write(soapResult);
            }
        }


        private static XmlDocument CreateSoapEnvelope(string xmlBody)
        {
            XmlDocument soapEnvelopeDocument = new XmlDocument();
            soapEnvelopeDocument.LoadXml(xmlBody);
            return soapEnvelopeDocument;
        }

        private static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
        }
        private static HttpWebRequest CreateWebRequest(string url,string Password)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            string UserName = ConfigurationManager.AppSettings["DaikinUserName"].ToString();
            string usernamePassword = UserName + ":" + Password;
            usernamePassword = Convert.ToBase64String(new ASCIIEncoding().GetBytes(usernamePassword));
            webRequest.Headers.Add("Authorization", "Basic " + usernamePassword);
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }

    }
}
