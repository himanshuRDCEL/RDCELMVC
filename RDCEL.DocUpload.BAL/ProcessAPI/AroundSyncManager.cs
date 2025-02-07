using GraspCorn.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCEL.DocUpload.BAL._247AroundApiCall;
using RDCEL.DocUpload.BAL._247AroundSyncCall;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DataContract._247aroundService;

namespace RDCEL.DocUpload.BAL.ProcessAPI
{
    public class AroundSyncManager
    {
        #region Variable Declaration
        _247AroundInformationCall _aroundApiCall;
        SubmitRequestSyncCall _submitRequestSyncCall;
        #endregion
        #region Submit request for Service
        /// <summary>
        /// 
        /// </summary>       
        /// <returns></returns>   
        public _247ResponseDataContract ProcessServiceRequest(_247AroundDataContract aroundDC, tblExchangeOrder exchObj)
        {
            _aroundApiCall = new _247AroundInformationCall();
            _submitRequestSyncCall = new SubmitRequestSyncCall();
            _247ResponseDataContract _247ResponseDC = null;
            try
            {
                if (aroundDC != null)
                {
                    _247ResponseDC = _aroundApiCall.SubmitRequest(aroundDC);
                    if (_247ResponseDC != null)
                    {
                        #region Code to Add Submit request to Database
                        if (_247ResponseDC.data.response._247aroundBookingID != null && _247ResponseDC.data.response._247aroundBookingStatus != null)
                        {
                            string BookingId = _247ResponseDC.data.response._247aroundBookingID;
                            _submitRequestSyncCall.AddSubmitRequestToDB(aroundDC, BookingId, exchObj);
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("AroundSyncManager", "ProcessServiceRequest", ex);
            }
            return _247ResponseDC;
        }
        #endregion

        #region Set SubmitRequest Object
        public _247AroundDataContract Set247AroundObj(tblExchangeOrder exchObj, tblCustomerDetail custObj, tblBrand brandObj, tblProductCategory productCatObj, tblProductType productTypeObj)
        {
            _aroundApiCall = new _247AroundInformationCall();
            _247AroundDataContract aroundDataContract = null;
            DeliveryDate delivery = new DeliveryDate();
            try
            {
                if (exchObj != null && custObj != null && productTypeObj != null && brandObj != null && productCatObj != null)
                {
                    aroundDataContract = new _247AroundDataContract();
                    aroundDataContract.deliveryDate = new DeliveryDate();
                    aroundDataContract.partnerName = "DIGI2L";
                    aroundDataContract.name = custObj.FirstName + " " + custObj.LastName;
                    aroundDataContract.address = custObj.Address1 + " " + custObj.Address2;
                    aroundDataContract.city = custObj.City;
                    aroundDataContract.mobile = custObj.PhoneNumber;
                    aroundDataContract.email = custObj.Email;
                    aroundDataContract.category = productTypeObj.Description;
                    aroundDataContract.brand = brandObj.Name;
                    aroundDataContract.productType = productCatObj.Description;
                    aroundDataContract.product = productCatObj.Description;
                    aroundDataContract.subCategory = productTypeObj.Size;
                    aroundDataContract.partnerSource = "Api";
                    aroundDataContract.pincode = Convert.ToInt32(custObj.ZipCode);
                    aroundDataContract.requestType = "Visit Inspection";
                    aroundDataContract.orderID = exchObj.RegdNo;
                    aroundDataContract.itemID = exchObj.SponsorOrderNumber;
                    DateTime EstimateDeldate = DateTime.ParseExact(exchObj.EstimatedDeliveryDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    aroundDataContract.deliveryDate.day = EstimateDeldate.ToString("dd");
                    aroundDataContract.deliveryDate.month = EstimateDeldate.ToString("MM");
                    aroundDataContract.deliveryDate.year = EstimateDeldate.ToString("yyyy");
                    aroundDataContract.deliveryDate.hour = EstimateDeldate.ToString("hh");
                    aroundDataContract.deliveryDate.minute = EstimateDeldate.ToString("mm");
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("AroundSyncManager", "Set247AroundObj", ex);
            }
            return aroundDataContract;
        }
        #endregion
    }
}
