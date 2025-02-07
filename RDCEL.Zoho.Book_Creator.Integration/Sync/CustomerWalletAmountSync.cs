using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCEL.DocUpload.BAL;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DataContract;
using GraspCorn.Common.Helper;
using RDCEL.DocUpload.DataContract.ZohoModel;
using RDCEL.DocUpload.BAL.UTCZohoSync;
using RDCEL.DocUpload.BAL.ZohoCreatorCall;
using RDCEL.DocUpload.DataContract.ZohoBooksModel;
using RDCEL.DocUpload.BAL.ZohoBooksSync;
using RDCEL.DocUpload.DataContract.EVCDataDB;

namespace RDCEL.Zoho.Book_Creator.Integration
{
    public class CustomerWalletAmountSync
    {
        #region Variable Declaration

        CustomerInfoCall customerInfoCall;
        EVCManager eVCManager;
        EVCRegistrationRepository eVCRegistrationRepository;
        #endregion

        #region Contact

        /// <summary>
        /// Method will fetch all the Contact infromation 
        /// from Zoho Books and Update into Zoho Creator EVC 
        /// </summary>
        /// <returns>bool</returns>
        public bool ProcessContactInfo()
        {
            eVCRegistrationRepository = new EVCRegistrationRepository();
            customerInfoCall = new CustomerInfoCall();
            eVCManager = new EVCManager();
            bool flag = false;
            List<ContactData> contactList = null;
            List<tblEVCRegistration> EvcApprovedListDB = null;
            
            try
            {
                // Evc All Approved list from zoho creator
                 // EvcApprovedList = eVCManager.GetAllEVCApproved();
                 EvcApprovedListDB= eVCManager.EvcAprrovedListFromDB();
                //#region Update wallet for THE UNITED TRADING COMPANY - Organization ID: 758714876

                ////Contact list from zoho books
                //contactList = customerInfoCall.GetAllContact("758714876");              

                //if (contactList != null && contactList.Count > 0 && EvcApprovedListDB != null && EvcApprovedListDB.Count > 0)
                //{
                //    foreach( tblEVCRegistration evcDetails in EvcApprovedListDB)
                //    {
                //        string custName = evcDetails.EVCRegdNo+"-"+ evcDetails.BussinessName;
                //        //string custName = evcApprovedInfo.EVC_Regd_No + "-" + evcApprovedInfo.Bussiness_Name;
                //        ContactData contactDataObj = contactList.Find(x => x.customer_name.ToLower().Equals(custName.ToLower()));

                //        if (contactDataObj != null)
                //        {
                //            UPdateEvcWalletAmount(evcDetails, contactDataObj);
                //        }

                //    }

                //    //foreach (ContactData contactDataObj in contactList)
                //    //{
                //    //    //EvcApprovedData evcMasterInfo = EvcApprovedList.Find(x => x.EVC_Regd_No.ToLower().Equals(contactDataObj.contact_name.ToLower()));

                //    //    if (evcMasterInfo != null)
                //    //    {
                //    //        UpdateEVCWalletJson(evcMasterInfo, contactDataObj);
                //    //    }

                //    //}

                //}
                //#endregion

                #region Update wallet for UTC DIGITAL TECHNOLOGIES PRIVATE LIMITED - Organization ID: 758715049

                //Contact list from zoho books
                contactList = customerInfoCall.GetAllContact("758715049");
                if (contactList != null && contactList.Count > 0 && EvcApprovedListDB != null && EvcApprovedListDB.Count > 0)
                {
                    foreach (tblEVCRegistration evcDetails in EvcApprovedListDB)
                    {
                        string custName = evcDetails.EVCZohoBookName;
                        char[] separator = { '-' };

                        string[] splitString = custName.Split(separator);

                    
                        if (custName != null )
                        {
                            ContactData contactDataObj = contactList.Find(x => x.customer_name.ToLower().Equals(custName.ToLower()));

                            if (contactDataObj != null)
                            {
                                UPdateEvcWalletAmount(evcDetails, contactDataObj);
                            }
                        }
                        
                    }
                }

                #endregion

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("CustomerWalletAmountSync", "ProcessContactInfo", ex);

            }
            return flag;
        }

        public void UpdateEVCWalletJson(EvcApprovedData evcMasterInfo,ContactData contactDataObj)
        {
            //programMasterInfoCall = new ProgramMasterInfoCall();
            EVCUpdateDataContract eVCUpdateDC = null;
            EVCUpdateFormResponseDataContract evcUpdateResponseDC = null;
            eVCManager = new EVCManager();
            string message = string.Empty;
            try
            {
                if (evcMasterInfo != null && contactDataObj != null)
                {
                    // update EVC wallet amount details
                    double amount = contactDataObj.outstanding_payable_amount - contactDataObj.unused_credits_receivable_amount;

                    if (amount != 0)
                    {
                        string finalamount = amount.ToString();
                        bool result = finalamount.Contains("-");
                        if (result == true)
                        {
                            double eVCWalletAmount = amount * (-1);
                            eVCUpdateDC = eVCManager.SetUpdateEVCObject(evcMasterInfo.ID, eVCWalletAmount);
                            if (eVCUpdateDC != null)
                            {
                                evcUpdateResponseDC = eVCManager.UpdateEVCWallet(eVCUpdateDC);
                            }
                        }
                        else
                        {
                            eVCUpdateDC = eVCManager.SetUpdateEVCObject(evcMasterInfo.ID, amount);
                            if (eVCUpdateDC != null)
                            {
                                evcUpdateResponseDC = eVCManager.UpdateEVCWallet(eVCUpdateDC);
                            }
                        }

                    }
                    else
                    {
                        eVCUpdateDC = eVCManager.SetUpdateEVCObject(evcMasterInfo.ID, amount);
                        if (eVCUpdateDC != null)
                        {
                            evcUpdateResponseDC = eVCManager.UpdateEVCWallet(eVCUpdateDC);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("CustomerWalletAmountSync", "UpdateEVCWalletJson", ex);
            }
        }

        #endregion

        #region Update wallet amount to db
        public void UPdateEvcWalletAmount(tblEVCRegistration evcRegistration, ContactData contactDataObj)
        {
            eVCManager = new EVCManager();
            string message = string.Empty;
            try
            {
                if (evcRegistration != null && contactDataObj != null)
                {
                    // update EVC wallet amount details
                    double amount = contactDataObj.outstanding_payable_amount - contactDataObj.unused_credits_receivable_amount;

                    if (amount != 0)
                    {
                        string finalamount = amount.ToString();
                        bool result = finalamount.Contains("-");
                        if (result == true)
                        {
                            double eVCWalletAmount = amount * (-1);
                            evcRegistration = eVCManager.setEVCWalletObj(evcRegistration, eVCWalletAmount);
                            if (evcRegistration != null)
                            {
                                eVCManager.updateEvcWalletTODB(evcRegistration);
                            }
                        }
                        else
                        {
                            evcRegistration = eVCManager.setEVCWalletObj(evcRegistration, amount);
                            if (evcRegistration != null)
                            {
                                eVCManager.updateEvcWalletTODB(evcRegistration);
                            }
                        }

                    }
                    else
                    {
                        evcRegistration = eVCManager.setEVCWalletObj(evcRegistration, amount);
                        if (evcRegistration != null)
                        {
                            eVCManager.updateEvcWalletTODB(evcRegistration);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                LibLogging.WriteErrorToDB("CustomerWalletAmountSync", "UPdateEvcWalletAmount", ex);
            }
        }
        #endregion




    }
}
