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
using RDCEL.DocUpload.DataContract.EVCDataDB;
using RDCEL.DocUpload.DataContract.ZohoModel;

namespace RDCEL.DocUpload.BAL.ZohoCreatorCall
{
    public class EVCManager
    {


        #region Get zoho Evc Call Allocation details  by R.No 
        /// <summary>
        /// Method to get zoho Call Allocation by R.No 
        /// </summary>
        /// <param></param>
        /// <returns>model</returns>
        public EvcAllocationReportListDataContract GetEvcAllocationReportByR_No(string sponserID)
        {
           // EvcApprovedListDataContract EvcApprovedDC = null;
            EvcAllocationReportListDataContract EvcAllocationReportDC = null;
            IRestResponse response = null;
            try
            {
                if (sponserID != null)
                {
                    response = ZohoServiceCalls.Rest_InvokeZohoInvoiceServiceForPlainText(ZohoCreatorAPICallURL.GetURLFor(ZohoCreatorAPICallURL.GetUrlWithFilter,
                                                                                    ReportLinkNameConstant.Evc_Allocation_report,
                                                                                     FilterConstant.Evc_Allocation_RNo_filter.Replace("[sponserID]", sponserID)
                                                                                        ), Method.GET, null);

                    if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK)
                    {
                        EvcAllocationReportDC = JsonConvert.DeserializeObject<EvcAllocationReportListDataContract>(response.Content);

                    }
                    //if (EvcAllocationReportDC != null)
                    //{
                    //    if (EvcAllocationReportDC.data.Count > 0 && EvcAllocationReportDC.data[0] != null)
                    //    {
                    //        string EVCId = null;
                    //        EVCId = EvcAllocationReportDC.data[0].EVC_Name.ID;
                    //        if (EVCId != null)
                    //        {

                    //            response = ZohoServiceCalls.Rest_InvokeZohoInvoiceServiceForPlainText(ZohoCreatorAPICallURL.GetURLFor(ZohoCreatorAPICallURL.GetUrlWithFilter,
                    //                                                                            ReportLinkNameConstant.All_Evc_Masters_report,
                    //                                                                             FilterConstant.Evc_Masters_Id_filter.Replace("[evcID]", EVCId)
                    //                                                                                ), Method.GET, null);

                    //            if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK)
                    //            {
                    //                EvcApprovedDC = JsonConvert.DeserializeObject<EvcApprovedListDataContract>(response.Content);

                    //            }
                    //        }
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SponserManager", "GetSponserOrderByOrderNo", ex);
            }
            return EvcAllocationReportDC;
        }

        #endregion

        #region Get zoho Evc Master details  by evcId
        /// <summary>
        /// Method to get zoho EVC Master by evcId
        /// </summary>
        /// <param></param>
        /// <returns>model</returns>
        public EvcApprovedListDataContract GetEvcMasterById(string EVCId)
        {
            EvcApprovedListDataContract EvcApprovedDC = null;
            IRestResponse response = null;
            try
            {
                if (EVCId != null)
                {

                    response = ZohoServiceCalls.Rest_InvokeZohoInvoiceServiceForPlainText(ZohoCreatorAPICallURL.GetURLFor(ZohoCreatorAPICallURL.GetUrlWithFilter,
                                                                                    ReportLinkNameConstant.All_Evc_Masters_report,
                                                                                     FilterConstant.Evc_Masters_Id_filter.Replace("[evcID]", EVCId)
                                                                                        ), Method.GET, null);

                    if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK)
                    {
                        EvcApprovedDC = JsonConvert.DeserializeObject<EvcApprovedListDataContract>(response.Content);

                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SponserManager", "GetEvcMasterById", ex);
            }
            return EvcApprovedDC;
        }

        /// <summary>
        /// Method to get zoho EVC Master by evcId
        /// </summary>
        /// <param></param>
        /// <returns>model</returns>
        public EVCMasterDetailDataContract GetEvcMasterDetailWithBalanceById(string EVCId)
        {
            EVCMasterDetailDataContract EvcMasterDC = null;
            IRestResponse response = null;
            try
            {
                if (EVCId != null)
                {

                    response = ZohoServiceCalls.Rest_InvokeZohoInvoiceServiceForPlainText(ZohoCreatorAPICallURL.GetURLFor(ZohoCreatorAPICallURL.GetUrlWithFilter,
                                                                                    ReportLinkNameConstant.All_Evc_Masters_report,
                                                                                     FilterConstant.Evc_Id_filter.Replace("[EVCID]", EVCId)
                                                                                        ), Method.GET, null);

                    if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK)
                    {
                        EvcMasterDC = JsonConvert.DeserializeObject<EVCMasterDetailDataContract>(response.Content);

                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SponserManager", "GetEvcMasterDetailWithBalanceById", ex);
            }
            return EvcMasterDC;
        }

        #endregion

        #region Get zoho EVC Approved details 
        /// <summary>
        /// Method to get zoho EVC Approved
        /// </summary>
        /// <param></param>
        /// <returns>model</returns>
        public List<EvcApprovedData> GetAllEVCApproved()
        {
            EvcApprovedListDataContract EvcApprovedDC = null;
            List<EvcApprovedData> finalEvcApprovedList = new List<EvcApprovedData>();
            IRestResponse response = null;
            int limit = 200;
            int frm = 1;
            string qryString = FilterConstant.Sponser_filter;
            string finalQryString = string.Empty;
            try
            {
                for (int i = 1; i < limit; i++)
                {
                    finalQryString = qryString.Replace("[FromCount]", frm.ToString());
                    response = ZohoServiceCalls.Rest_InvokeZohoInvoiceServiceForPlainText(ZohoCreatorAPICallURL.GetURLFor(ZohoCreatorAPICallURL.GetUrlWithFilter,
                                                                                ReportLinkNameConstant.All_Evc_Masters_report,
                                                                                 finalQryString
                                                                                    ), Method.GET, null);
                    if (response != null)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            EvcApprovedDC = JsonConvert.DeserializeObject<EvcApprovedListDataContract>(response.Content);

                        }
                    }
                    if (EvcApprovedDC != null && EvcApprovedDC != null && EvcApprovedDC.data.Count > 0 && response != null && response.StatusCode == HttpStatusCode.OK)

                        finalEvcApprovedList.AddRange(EvcApprovedDC.data);
                    else
                        break;

                    frm = (i * 200)+1;
                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("EVCManager", "GetAllEVCApproved", ex);
            }
            return finalEvcApprovedList;
        }

        #endregion


        #region Update EVC Wallet detail
        /// <summary>
        /// Method to Update EVC Wallet detail
        /// </summary>
        /// <param></param>
        /// <returns>model</returns>
        public EVCUpdateFormResponseDataContract UpdateEVCWallet(EVCUpdateDataContract eVCUpdateDC)
        {
            EVCUpdateFormResponseDataContract evcUpdateResponseDC = null;
            EVCUpdateFormRequestDataContract evcUpdateRequestDC = new EVCUpdateFormRequestDataContract();
            IRestResponse response = null;

            try
            {
                if (eVCUpdateDC != null)
                {
                    evcUpdateRequestDC.data = eVCUpdateDC;

                    //IRestResponse response = ZohoServiceCalls.Rest_InvokeZohoInvoiceServiceForPlainText("https://creatorapp.zoho.com/api/v2/accountsperthsecurityservices/mobileapp/form/D_Runsheets", patrolRequestDC);
                    response = ZohoServiceCalls.Rest_InvokeZohoInvoiceServiceForPlainText(ZohoCreatorAPICallURL.GetURLFor(ZohoCreatorAPICallURL.GetUrlWithFilter,
                                                                                  ReportLinkNameConstant.All_Evc_Masters_report,
                                                                                   FilterConstant.Evc_Id_filter.Replace("[EVCID]", eVCUpdateDC.ID)
                                                                                      ), Method.PATCH, evcUpdateRequestDC);

                    if (response.StatusCode == HttpStatusCode.OK && response.StatusCode == HttpStatusCode.Created)
                    {
                        evcUpdateResponseDC = JsonConvert.DeserializeObject<EVCUpdateFormResponseDataContract>(response.Content);

                    }
                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("EVCManager", "UpdateEVCWallet", ex);
            }
            return evcUpdateResponseDC;
        }

        #endregion

        #region Set EVC details in EVCUpdate 
        /// <summary>
        /// Set EVC details in EVCUpdate 
        /// </summary>
        /// <param></param>
        /// <returns>model</returns>
        public EVCUpdateDataContract SetUpdateEVCObject(string id, double recievedamount)
        {
            EVCUpdateDataContract eVCUpdateInfo = null;
            try
            {
                if (id != null)
                {
                    eVCUpdateInfo = new EVCUpdateDataContract();
                    eVCUpdateInfo.ID = id;
                    eVCUpdateInfo.EVC_Wallet_Amount = recievedamount.ToString();
                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("EVCManager", "SetUpdateEVCObject", ex);
            }
            return eVCUpdateInfo;
        }

        #endregion

        #region 
        public List<tblEVCRegistration> EvcAprrovedListFromDB()
        {
            EVCRegistrationRepository eVCRegistrationRepository = new EVCRegistrationRepository();
            List<tblEVCRegistration> evcAprroved = new List<tblEVCRegistration>();

            try
            {
                evcAprroved = eVCRegistrationRepository.GetList(x => x.ISEVCApprovrd == true).ToList();
            }
            catch(Exception ex)
            {
                LibLogging.WriteErrorToDB("EVCManager", "EvcAprrovedListFromDB", ex);
            }
            return evcAprroved;
        }
        #endregion

        #region Set EVC details in EVCUpdate 
        /// <summary>
        /// Set EVC details in EVCUpdate 
        /// </summary>
        /// <param></param>
        /// <returns>model</returns>
        public tblEVCRegistration setEVCWalletObj(tblEVCRegistration  evcApprovedData, double recievedamount)
        {
            
            try
            {
                if (evcApprovedData!= null)
                {
                    evcApprovedData.EVCWalletAmount =Convert.ToDecimal(recievedamount);
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("EVCManager", "SetUpdateEVCObject", ex);
            }
            return evcApprovedData;
        }

        #endregion


        #region
        public void updateEvcWalletTODB(tblEVCRegistration evcRegistration)
        {
            EVCRegistrationRepository eVCRegistrationRepository = new EVCRegistrationRepository();
            try
            {
                if (evcRegistration!=null)
                {
                    evcRegistration.ModifiedBy = 3;
                    evcRegistration.ModifiedDate = DateTime.Now;
                    eVCRegistrationRepository.Update(evcRegistration);
                    eVCRegistrationRepository.SaveChanges();

                }
            }
            catch(Exception ex)
            {
                LibLogging.WriteErrorToDB("EVCManager","updateEvcWalletTODB", ex);
            }
        }
        #endregion


    }
}
