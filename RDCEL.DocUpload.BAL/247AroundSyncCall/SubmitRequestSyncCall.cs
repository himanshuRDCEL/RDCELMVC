using GraspCorn.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DataContract._247aroundService;

namespace RDCEL.DocUpload.BAL._247AroundSyncCall
{
    public class SubmitRequestSyncCall
    {

        #region Variable Declaration
        _247AroundRepository aroundRepository;

        DateTime currentDatetime = DateTime.Now.TrimMilliseconds();

        #endregion

        #region Add SubmitRequest in database
        /// <summary>
        /// Method to add the Ticket
        /// </summary>       
        /// <returns> </returns>   
        public int AddSubmitRequestToDB(_247AroundDataContract aroundDataContract, string _247aroundBookingID, tblExchangeOrder exchObj)
        {
            aroundRepository = new _247AroundRepository();
            int result = 0;
            try
            {
                tbl247Around ServiceInfo = Set247AroundObjectDBJson(aroundDataContract, _247aroundBookingID, exchObj);
                {
                    aroundRepository.Add(ServiceInfo);
                    aroundRepository.SaveChanges();
                    result = ServiceInfo.Id;
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SubmitRequestSyncCall", "AddSubmitRequestToDB", ex);
            }

            return result;
        }
        #endregion

        #region set Add SubmitRequest obj
        /// <summary>
        /// Method to set Ticket info to table
        /// </summary>
        /// <param name="aroundDataContract">aroundDataContract</param>     
        public tbl247Around Set247AroundObjectDBJson(_247AroundDataContract aroundDataContract, string _247aroundBookingID, tblExchangeOrder exchObj)
        {
            tbl247Around serviceObj = null;
            try
            {
                if (aroundDataContract != null)
                {
                    serviceObj = new tbl247Around();
                    if (_247aroundBookingID != null)
                    {
                        serviceObj.TwoFourSevenAroundBooKingID = _247aroundBookingID;
                    }
                    serviceObj.name = aroundDataContract.name;
                    serviceObj.orderID = aroundDataContract.orderID;
                    serviceObj.paidByCustomer = "NO";
                    serviceObj.partnerName = aroundDataContract.partnerName;
                    serviceObj.pincode = aroundDataContract.pincode.ToString();
                    serviceObj.product = aroundDataContract.product;
                    serviceObj.productType = aroundDataContract.productType;
                    serviceObj.requestType = aroundDataContract.requestType;
                    serviceObj.subCategory = aroundDataContract.subCategory;
                    serviceObj.address = aroundDataContract.address;
                    serviceObj.brand = aroundDataContract.brand;
                    serviceObj.category = aroundDataContract.category;
                    serviceObj.city = aroundDataContract.city;
                    DateTime EstimateDeldate = DateTime.ParseExact(exchObj.EstimatedDeliveryDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    serviceObj.deliveryDate = EstimateDeldate;
                    serviceObj.email = aroundDataContract.email;
                    serviceObj.itemID = aroundDataContract.itemID;
                    serviceObj.mobile = aroundDataContract.mobile;

                    //add sponsor order id
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SubmitRequestSyncCall", "Set247AroundObjectDBJson", ex);
            }
            return serviceObj;
        }
        #endregion
    }
}
