using GraspCorn.Common.Constant;
using GraspCorn.Common.Helper;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RDCEL.DocUpload.BAL.ServiceCall;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DataContract.ZohoModel;

namespace RDCEL.DocUpload.BAL.UTCZohoSync
{
    public class EVCApprovedInfoCall
    {
        #region Variable Declaration   
        EVCApprovedRepository eVCApprovedRepository;
        DateTime currentDatetime = DateTime.Now.TrimMilliseconds();
        #endregion


        #region Add EVC Approved details into DB
        /// <summary>
        /// Method to Add EVC Approved
        /// </summary>
        /// <param></param>
        /// <returns>model</returns>

        public int AddEVCApprovedInfotoDB(EvcApprovedData evcApprovedDataObj)
        {
            eVCApprovedRepository = new EVCApprovedRepository();
            int result = 0;
            try
            {
                tblEVCApproved eVCApprovedInfo = SetEVCApprovedInfoObject(evcApprovedDataObj);

                if (eVCApprovedInfo != null)
                {
                    tblEVCApproved tempEVCApproved = eVCApprovedRepository.GetSingle(x => x.ZohoEVCApprovedId.Equals(eVCApprovedInfo.ZohoEVCApprovedId));
                    if (tempEVCApproved != null)
                    {
                        eVCApprovedInfo.Id = tempEVCApproved.Id;
                        eVCApprovedInfo.ModifiedDate = currentDatetime;
                        eVCApprovedRepository.Update(eVCApprovedInfo);
                    }
                    else
                    {
                        eVCApprovedInfo.CreatedDate = currentDatetime;
                        eVCApprovedRepository.Add(eVCApprovedInfo);
                    }

                    eVCApprovedRepository.SaveChanges();
                    result = eVCApprovedInfo.Id;
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("EVCApprovedInfoCall", "AddEVCApprovedInfotoDB", ex);
            }
            return result;

        }


        public tblEVCApproved SetEVCApprovedInfoObject(EvcApprovedData evcApprovedDataObj)
        {
            tblEVCApproved eVCApprovedInfo = null;
            try
            {
                if (evcApprovedDataObj != null)
                {
                    eVCApprovedInfo = new tblEVCApproved();

                    eVCApprovedInfo.ZohoEVCApprovedId = evcApprovedDataObj.ID;
                    eVCApprovedInfo.BussinessName = evcApprovedDataObj.Bussiness_Name;
                    eVCApprovedInfo.EVCRegdNo = evcApprovedDataObj.EVC_Regd_No;
                    eVCApprovedInfo.ContactPerson = evcApprovedDataObj.EVC_Name;
                    eVCApprovedInfo.EVCMobileNumber = evcApprovedDataObj.EVC_Mobile_Number;
                    eVCApprovedInfo.EmailID = evcApprovedDataObj.E_mail_ID;
                    eVCApprovedInfo.RegdAddressLine1 = evcApprovedDataObj.Regd_Address_Line_1;
                    eVCApprovedInfo.RegdAddressLine2 = evcApprovedDataObj.Regd_Address_Line_2;
                    eVCApprovedInfo.PinCode = evcApprovedDataObj.PIN_Code;
                    eVCApprovedInfo.City = evcApprovedDataObj.City;
                    eVCApprovedInfo.State = evcApprovedDataObj.State;
                    eVCApprovedInfo.EVCWalletAmount = evcApprovedDataObj.EVC_Wallet_Amount;
                    eVCApprovedInfo.ContactPersonAddress = evcApprovedDataObj.Contact_Person_Address;
                    eVCApprovedInfo.UploadGSTRegistration = evcApprovedDataObj.Upload_GST_Registration;
                    eVCApprovedInfo.CopyofCancelledCheque = evcApprovedDataObj.Copy_of_Cancelled_Cheque;
                    eVCApprovedInfo.IsActive = true;
                    
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("EVCApprovedInfoCall", "SetEVCApprovedInfoObject", ex);
            }
            return eVCApprovedInfo;
        }

        #endregion












    }
}
