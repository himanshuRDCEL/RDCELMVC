using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Daikin_scheduler_console.DAL;
using RDCEL.DocUpload.BAL.ServiceCall;
using RDCEL.DocUpload.DataContract.BillCloud;
using GraspCorn.Common.Constant;
using GraspCorn.Common.Helper;
using System.Xml.Linq;
using Newtonsoft.Json;
using ConsoleApp.Model;
using RDCEL.DocUpload.DataContract.SponsorModel;
using RDCEL.DocUpload.BAL.SponsorsApiCall;
using Root = ConsoleApp.Model.Root;
using Daikin_scheduler_console.BAl;
using System.Configuration;

namespace Daikin_scheduler_console
{
    public class Program
    {
        
        static void Main(string[] args)
        {
            //string sValue = System.Configuration.ConfigurationManager.AppSettings["BatchFile"];
            //GetAppSettingsFile();
            GetDaikinExchangeOrderReport();
            Console.ReadLine();
        }

        //static void GetAppSettingsFile()
        //{
        //    try
        //    {
        //        var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
        //        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        //        _iconfiguration = builder.Build();
        //        Console.WriteLine("Data connection successfully!");                
        //    }
        //    catch (Exception ex)
        //    {
        //        LibLogging.WriteErrorToDB("ServiceRequest_Daikin_ExhangeOrder", "Update_Order_Status", ex);
        //        Console.WriteLine("Exception : " + ex.Message);
        //        Console.ReadLine();
        //    }
        //}

        static void GetDaikinExchangeOrderReport()
        {
            string responseGetOrder = null, serviceRequestLifeCycleStatusCode = null;
            var daikinSchedukerDAL = new Daikin_Scheduler_Dal();

            Console.WriteLine("Get Exhange Order List Start!");
            var listExchangeOrder = daikinSchedukerDAL.GetExchangeOrderViewList();
            Console.WriteLine("Get Exhange Order List END With Count : " + listExchangeOrder.Count);
            string Password = daikinSchedukerDAL.GetUserPassword();
            foreach (var exchange in listExchangeOrder)
            {
                ProductOrderStatusDataContract productOrderStatusDataContract = new ProductOrderStatusDataContract();

                Console.WriteLine("Get Daikin Order Details With Respect To Sponsor Service Refrence ID Start!");
                responseGetOrder = GetDiakinOrderDetails(exchange.SponsorServiceRefId, Password);
                Console.WriteLine("Get Daikin Order Details With Respect To Sponsor Service Refrence ID End !");
                
                if (responseGetOrder != string.Empty)
                {
                    Console.WriteLine("Parse XML response start!");
                    var XmlDoc = XDocument.Parse(responseGetOrder);
                    Console.WriteLine("Serialize X Node start!");
                    var Convertedjson = JsonConvert.SerializeXNode(XmlDoc);
                    Console.WriteLine("Deserialize into Object and convertinf into JSon start!");
                    Root myDeserializedClass1 = JsonConvert.DeserializeObject<Root>(Convertedjson);
                    Console.WriteLine("Finding serviceRequestLifeCycleStatusCode from JSon");
                    serviceRequestLifeCycleStatusCode = myDeserializedClass1.soapEnvelope.soapBody.ServiceRequestPartyCollection.ServiceRequestParty.ServiceRequestUserLifeCycleStatusCode;

                    Console.WriteLine("Checking Status serviceRequestLifeCycleStatusCode");
                    if (serviceRequestLifeCycleStatusCode.Trim().ToLower() == "5" || serviceRequestLifeCycleStatusCode.Trim().ToLower() == "Z4")
                    {
                        ProductManager productOrderInfo = new ProductManager();
                        Console.WriteLine("Update Exchange Order Status as Delivered Strat !");
                        productOrderStatusDataContract.OrderId = exchange.ExchangeOrderID;
                        productOrderStatusDataContract.Status = "Delivered";
                        productOrderInfo.UpdateOrderStatus(productOrderStatusDataContract);
                        Console.Write("Order number " + exchange.ExchangeOrderID + " is updated ");


                        //IRestResponse response = BizlogServiceCall.Rest_InvokeBizlogSeviceFormData(url, Method.POST, ticketDataContract);
                        //string responseString = MVCApicall.Rest_TokenAPI(productOrderStatusDataContract);

                    }
                    else
                    {
                        Console.WriteLine("ServiceRequestLifeCycleStatusCode Not 5 OR Z4, Status For ServiceRequestLifeCycleStatusCode Is : " + serviceRequestLifeCycleStatusCode);
                    }
                }
                else
                {
                    Console.WriteLine("Daikin Order Details Not Found With Respect To Sponsor Service Refrence ID :  " + exchange.SponsorServiceRefId);
                    //LibLogging.WriteErrorToDB("SponserOrderSync", "GetDiakinCustomerDetails", ex);
                }
            }
        }

        static string GetDiakinOrderDetails(string sponsorServiceRefId,string SoapPassword)
        {
            string XMLCustomerBody = string.Empty;
            var daikinSchedukerDAL = new Daikin_Scheduler_Dal();
            try
            {
                if (sponsorServiceRefId != null)
                {
                    string url = daikinSchedukerDAL.UrlForGetRequest();
                    Console.WriteLine("Set up Daikin For Get Order Details Start!");
                    string MessageBody = SetupDaikinForGetOderDetails(sponsorServiceRefId);
                    Console.WriteLine("Set up Daikin For Get Order Details End!");
                    string envelop = SOAPConstant.Diakin_Envelop;
                    envelop = envelop.Replace("[MessageBody]", MessageBody);
                    Console.WriteLine("SOAP API Call For Daikin Get Order Details Start!");

                     XMLCustomerBody = SOAPServiceCall.SOAP_InvokeServiceForDaikin(envelop, url, SoapPassword);
                    Console.WriteLine("SOAP API Call For Daikin Get Order Details End!");
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ServiceRequest_Daikin_ExhangeOrder", "Update_Order_Status", ex);
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
            return XMLCustomerBody;
        }

        static string SetupDaikinForGetOderDetails(string sponsorServiceRefId)
        {
            string XMLCustomerBody = string.Empty;
            try
            {
                if (sponsorServiceRefId != null && sponsorServiceRefId != "")
                {
                    XMLCustomerBody = SOAPConstant.Diakin_GetOrder;
                    XMLCustomerBody = XMLCustomerBody.Replace("[Order]", sponsorServiceRefId);
                }
                else
                {
                    Console.WriteLine("Sponsor Service Ref Id is Null/Empty");
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ServiceRequest_Daikin_ExhangeOrder", "Update_Order_Status", ex);
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }

            return XMLCustomerBody;
        }
    }
}
