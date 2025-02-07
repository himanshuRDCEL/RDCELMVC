using GraspCorn.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCEL.DocUpload.BAL.Common;
using RDCEL.DocUpload.BAL.ZohoCreatorCall;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DataContract.SponsorModel;
using RDCEL.DocUpload.DataContract.ZohoModel;

namespace RDCEL.DocUpload.BAL.Utility
{
    public class UtilityManager
    {
        #region Variable Declaration
        EVCPODDetailsRepository _evcPodDetailsRepository;
        ExchangeOrderRepository _exchangeOrderRepository;
        DateTime currentDatetime = DateTime.Now.TrimMilliseconds();
        SponserManager _sponsorManager;

        #endregion

        #region Store Exchange POD Details in database
        /// <summary>
        ///Exchange POD Details in database
        /// </summary>       
        /// <returns></returns>   
        public int ManageExchangePOD(string regdNum, string podUrl)
        {
            _evcPodDetailsRepository = new EVCPODDetailsRepository();
            _exchangeOrderRepository = new ExchangeOrderRepository();
            _sponsorManager = new SponserManager();
            tblEVCPODDetail tblEVCPODDetailObj = new tblEVCPODDetail();
            tblExchangeOrder tblExchangeOrderObj = new tblExchangeOrder();
            EVCPODDetailsDataContract exchangePODObj = new EVCPODDetailsDataContract();

            string fileName = podUrl.Split('/').LastOrDefault();
            int result = 0;
            try
            {
                if (regdNum!=null && podUrl != null)
                {
                    tblExchangeOrderObj = _exchangeOrderRepository.GetSingle(x=>x.IsActive == true && x.RegdNo == regdNum);
                    if (tblExchangeOrderObj != null)
                    {
                        tblEVCPODDetailObj = _evcPodDetailsRepository.GetSingle(x => x.IsActive == true && x.RegdNo == regdNum);
                        if (tblEVCPODDetailObj != null)
                        {
                            tblEVCPODDetailObj.PODURL = fileName;
                            tblEVCPODDetailObj.ModifiedDate = currentDatetime;
                            _evcPodDetailsRepository.Update(tblEVCPODDetailObj);
                        }
                        else
                        {
                            tblEVCPODDetailObj = new tblEVCPODDetail();
                            tblEVCPODDetailObj.RegdNo = regdNum;
                            tblEVCPODDetailObj.PODURL = fileName;
                            tblEVCPODDetailObj.IsActive = true;
                            tblEVCPODDetailObj.CreatedDate = currentDatetime;
                            _evcPodDetailsRepository.Add(tblEVCPODDetailObj);
                        }
                        result = _evcPodDetailsRepository.SaveChanges();
                        if (result > 0)
                        {
                            exchangePODObj.ID = tblExchangeOrderObj.ZohoSponsorOrderId;
                            exchangePODObj.Proof_Of_Delivery = new ProofOfDelivery();
                            exchangePODObj.Proof_Of_Delivery.value = fileName;
                            exchangePODObj.Proof_Of_Delivery.url = podUrl;
                           // sponserResponseDC = _sponsorManager.UpdateExchangePODUrlOnZoho(exchangePODObj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("UtilityManager", "ManageExchangePOD", ex);
            }

            return result;
         }
        #endregion
             
        #region set Customer detail obj
        /// <summary>
        /// Method to set Custome info to table
        /// </summary>
        /// <param name="productOrderDataContract">productOrderDataContract</param>     
        public tblCustomerDetail SetCustomerObjectJson(ProductOrderDataContract productOrderDataContract)
        {
            tblCustomerDetail customerObj = null;
            try
            {
                if (productOrderDataContract != null)
                {
                    customerObj = new tblCustomerDetail();
                    customerObj.FirstName = productOrderDataContract.FirstName;                    
                    customerObj.LastName = productOrderDataContract.LastName;
                    customerObj.ZipCode = productOrderDataContract.ZipCode;
                    customerObj.Address1 = productOrderDataContract.Address1;
                    customerObj.Address2 = productOrderDataContract.Address2;
                    customerObj.City = productOrderDataContract.City;
                    customerObj.Email = productOrderDataContract.Email;
                    customerObj.PhoneNumber = productOrderDataContract.PhoneNumber;
                    customerObj.IsActive = true;
                    customerObj.CreatedDate = currentDatetime;
                    //customerObj.State = productOrderDataContract.state;

                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("CustomerManager", "SetCstomerObjectJson", ex);
            }
            return customerObj;
        }
        #endregion
    }
}
