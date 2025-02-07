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
using RDCEL.DocUpload.DataContract.ZohoSyncModel;

namespace RDCEL.DocUpload.BAL.UTCZohoSync
{
    public class PinCodeMasterInfoCall
    {
        #region Variable Declaration   
        PinCodeRepository pinCodeRepository;
        DateTime currentDatetime = DateTime.Now.TrimMilliseconds();
        #endregion


        #region Add PinCode details into DB
        /// <summary>
        /// Method to Add PinCode
        /// </summary>
        /// <param></param>
        /// <returns>model</returns>

        public int AddPincodeInfotoDB(PinCodeMaster pinCodeMasterObj, tblPinCode pinCodeDataObj)
        {
            pinCodeRepository = new PinCodeRepository();
            int result = 0;
            try
            {
                tblPinCode pinCodeInfo = SetPinCodeInfoObject(pinCodeMasterObj);

                if (pinCodeInfo != null)
                {
                    tblPinCode tempPinCode = pinCodeRepository.GetSingle(x => x.ZipCode.Equals(pinCodeInfo.ZipCode));
                    //tblPinCode tempPinCode = null;

                    if (tempPinCode != null)
                    {
                        pinCodeInfo.Id = tempPinCode.Id;
                        pinCodeInfo.ModifiedDate = currentDatetime;
                        pinCodeRepository.Update(pinCodeInfo);
                    }
                    else
                    {
                        pinCodeInfo.CreatedDate = currentDatetime;
                        pinCodeRepository.Add(pinCodeInfo);
                    }

                    pinCodeRepository.SaveChanges();
                    result = pinCodeInfo.Id;
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("PinCodeInfoCall", "AddPincodeInfotoDB", ex);
            }
            return result;

        }


        public tblPinCode SetPinCodeInfoObject(PinCodeMaster pinCodeMasterObj)
        {
            tblPinCode pinCodeInfo = null;
            try
            {
                if (pinCodeMasterObj != null)
                {
                    pinCodeInfo = new tblPinCode();

                    pinCodeInfo.ZohoPinCodeId = pinCodeMasterObj.ID;
                    pinCodeInfo.ZipCode = Convert.ToInt32(pinCodeMasterObj.Pin_Code);
                    pinCodeInfo.Location = pinCodeMasterObj.City_Code;
                    pinCodeInfo.IsActive = true;
                    
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("PinCodeInfoCall", "SetPinCodeInfoObject", ex);
            }
            return pinCodeInfo;
        }

        #endregion












    }
}
