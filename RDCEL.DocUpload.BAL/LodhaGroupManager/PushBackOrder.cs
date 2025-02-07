using GraspCorn.Common.Enums;
using GraspCorn.Common.Helper;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using RDCEL.DocUpload.BAL.ServiceCall;
using RDCEL.DocUpload.BAL.SponsorsApiCall;
using RDCEL.DocUpload.DAL.Helper;
using RDCEL.DocUpload.DataContract.Lodha_Group;
using RDCEL.DocUpload.DataContract.SponsorModel;

namespace RDCEL.DocUpload.BAL.LodhaGroupManager
{
    public class PushBackOrder
    {
        #region Variable Declaration
        Logging logging;
        #endregion
        public OrderPushBackResponse PushOrderBackForTracking(ExchangeOrderDataContract exchangeOrderDC,int transId)
        {
            OrderPushBackResponse orderpushBack = null;
            string Status=string.Empty;
            logging = new Logging();
            try
            {
        
                OrderPushback orderPushback= new OrderPushback();
                orderPushback.pwaOrderId = ConfigurationManager.AppSettings["LodhaVendorId"].ToString()+"-"+ exchangeOrderDC.RegdNo;
                orderPushback.mobileNo = exchangeOrderDC.mobileNoWithCountryCode;
                orderPushback.address = exchangeOrderDC.Address1+" "+ exchangeOrderDC.Address2;
                orderPushback.orderDetailURL = ConfigurationManager.AppSettings["ERPBaseURL"].ToString()+ "TimeLineIndependent?orderTransId=" + transId;
                string bookingstatusenum = ExchangeOrderManager.GetEnumDescription(LodhaGroupEnum.INPROGRESS);
                orderPushback.bookingStatus = bookingstatusenum;
                string StatusDescription = ExchangeOrderManager.GetEnumDescription(StatusEnum.OrderCreated);
                orderPushback.remarks = StatusDescription;
                orderPushback.finalAmount = Convert.ToInt32(exchangeOrderDC.ExchangePriceString);
                orderPushback.pwaOrderSummary=new PwaOrderSummary();
                orderPushback.pwaOrderSummary.ProductType = exchangeOrderDC.ProductType;
                orderPushback.pwaOrderSummary.ProductCategory= exchangeOrderDC.ProductCategory;
                orderPushback.pwaOrderSummary.OrderId = exchangeOrderDC.Id;
                orderPushback.pwaOrderSummary.RegdNo= exchangeOrderDC.RegdNo;
                orderPushback.pwaOrderSummary.Condition = exchangeOrderDC.ProductCondition;
                
                string url = ConfigurationManager.AppSettings["LodhaOrderPushBackUrl"].ToString();
                string Response = LodhaServiceCall.Rest_InvokeLodhaServiceFormData(url,Method.POST, orderPushback);

                if(Response != null)
                {
                    logging.WriteAPIRequestToDB("PushBackOrder", "LodhaServiceCall", exchangeOrderDC.SponsorOrderNumber, Response);
                    orderpushBack = JsonConvert.DeserializeObject<OrderPushBackResponse>(Response);
                }
               
            }
            catch (Exception ex) 
            {
                LibLogging.WriteErrorToDB("PushBackOrder", "PushOrderBackForTracking", ex);
            }
            return orderpushBack;
        }
    }
}
