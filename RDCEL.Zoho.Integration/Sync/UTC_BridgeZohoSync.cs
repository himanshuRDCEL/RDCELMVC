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
using RDCEL.DocUpload.DataContract.ZohoSyncModel;

namespace RDCEL.Zoho.Integration.Sync
{
    public class UTC_BridgeZohoSync
    {
        #region Variable Declaration

        PriceMasterManager priceMasterManager;
        PriceMasterInfoCall priceMasterInfoCall;
        PriceMasterRepository productPriceInformationRepository;
        ProductCategoryRepository _productCategoryRepository;
        CommonManager commonManager;
        PinCodeMasterInfoCall pinCodeMasterInfoCall;
        PinCodeRepository pinCodeRepository;
        ProductTypeRepository productTypeInformationRepository;
        SponserManager sponserManager;
        SponsorInfoCall sponsorInfoCall;
        ExchangeOrderRepository ExchangeOrderRepository;
        EVCManager eVCManager;
        EVCApprovedInfoCall eVCApprovedInfoCall;
        EVCApprovedRepository eVCApprovedRepository;
        DateTime currentDatetime = DateTime.Now.TrimMilliseconds();
        #endregion

        #region Price Master

        /// <summary>
        /// Method will fetch all the Price Master infromation 
        /// from Zoho and add into UTC database
        /// </summary>
        /// <returns>bool</returns>
        public bool ProcessPriceMasterInfo()
        {
            priceMasterManager = new PriceMasterManager();
            productPriceInformationRepository = new PriceMasterRepository();
            productTypeInformationRepository = new ProductTypeRepository();
            _productCategoryRepository = new ProductCategoryRepository();
            bool flag = false;
            try
            {
                // List<PriceMasterData> GetAllPriceMaster
                List<PriceMasterData> priceMasterDataList = priceMasterManager.GetAllPriceMaster();
                List<tblProductCategory> productCategories = _productCategoryRepository.GetList(x => x.IsActive == true).ToList();
                if (priceMasterDataList != null && priceMasterDataList.Count > 0)
                {
                    List<PriceMasterData> priceMasterDataObjList = priceMasterDataList.Where(x => x.Exch_price_ID.Contains("BSH")).ToList();

                    foreach (PriceMasterData priceMasterDataObj in priceMasterDataList)
                    {
                        if (priceMasterDataObj.Exch_price_ID.Contains("BSH") && (priceMasterDataObj.Prod_Group.display_value.Equals("TV")) 
                            && priceMasterDataObj.Prod_Type.display_value.Contains("TSM"))
                        {
                        }
                        if (priceMasterDataObj.Prod_Type != null)
                        {
                            string productCode = null;
                            productCode = priceMasterDataObj.Prod_Type.display_value;
                            if (priceMasterDataObj.Size != null)
                            {
                                if (priceMasterDataObj.Prod_Type.display_value.Contains("RSX") || priceMasterDataObj.Prod_Type.display_value.Contains("WDC"))
                                    productCode = priceMasterDataObj.Size.display_value;
                                else
                                {
                                    if (!priceMasterDataObj.Size.display_value.Equals("Blank"))
                                        productCode = priceMasterDataObj.Prod_Type.display_value + "" + priceMasterDataObj.Size.display_value;
                                    else
                                        productCode = priceMasterDataObj.Prod_Type.display_value ;
                                }
                            }
                            if (productCode != null)
                            {
                                tblProductType productTypeInfo = productTypeInformationRepository.GetSingle(x => x.Code.ToLower().Equals(productCode.ToLower()));
                                if (productTypeInfo != null)
                                {
                                    tblPriceMaster productPriceInfo = productPriceInformationRepository.GetSingle(x => x.ProductTypeId.Equals(productTypeInfo.Id) && x.ExchPriceCode.ToLower().Equals(priceMasterDataObj.Exch_price_ID.ToLower()));
                                    if (productPriceInfo != null)
                                    {
                                        AddPriceMasterJson(priceMasterDataObj, productPriceInfo);
                                    }
                                    else
                                    {
                                        productPriceInfo = new tblPriceMaster();
                                        productPriceInfo.ZohoPriceMasterId = priceMasterDataObj.ID;
                                        productPriceInfo.ExchPriceCode = priceMasterDataObj.Exch_price_ID;
                                        productPriceInfo.ProductCategoryId = productCategories.FirstOrDefault(x => x.Code.ToLower().Equals(priceMasterDataObj.Prod_Group.display_value.ToLower())).Id;
                                        productPriceInfo.ProductCat = productCategories.FirstOrDefault(x => x.Code.ToLower().Equals(priceMasterDataObj.Prod_Group.display_value.ToLower())).Name;
                                        productPriceInfo.ProductTypeId = productTypeInfo.Id;
                                        productPriceInfo.ProductType = productTypeInfo.Description;
                                        productPriceInfo.ProductTypeCode = productTypeInfo.Code;
                                        productPriceInfo.BrandName_1 = priceMasterDataObj.Brand_1 != null ? priceMasterDataObj.Brand_1.display_value : string.Empty;
                                        productPriceInfo.BrandName_2 = priceMasterDataObj.Brand_2 != null ? priceMasterDataObj.Brand_2.display_value : string.Empty;
                                        productPriceInfo.BrandName_3 = priceMasterDataObj.Brand_3 != null ? priceMasterDataObj.Brand_3.display_value : string.Empty;
                                        productPriceInfo.BrandName_4 = priceMasterDataObj.Brand_4 != null ? priceMasterDataObj.Brand_4.display_value : string.Empty;

                                        productPriceInfo.Quote_P_High = priceMasterDataObj.Quote_P_H;
                                        productPriceInfo.Quote_Q_High = priceMasterDataObj.Quote_Q_H;
                                        productPriceInfo.Quote_R_High = priceMasterDataObj.Quote_R_H;
                                        productPriceInfo.Quote_S_High = priceMasterDataObj.Quote_S_H;

                                        productPriceInfo.Quote_P = priceMasterDataObj.Quote_P;
                                        productPriceInfo.Quote_Q = priceMasterDataObj.Quote_Q;
                                        productPriceInfo.Quote_R = priceMasterDataObj.Quote_R;
                                        productPriceInfo.Quote_S = priceMasterDataObj.Quote_S;

                                        productPriceInfo.PriceStartDate = priceMasterDataObj.Price_End_Date;
                                        productPriceInfo.PriceEndDate = priceMasterDataObj.Price_End_Date;

                                        AddPriceMasterJson(priceMasterDataObj, productPriceInfo);
                                    }
                                }
                                //tblProductType productTypeInfo = productTypeInformationRepository.GetSingle(x => x.Code.ToLower().Equals(productCode.ToLower()));
                                //if (productTypeInfo != null)
                                //{
                                //    tblPriceMaster productPriceInfo = productPriceInformationRepository.GetSingle(x => x.ProductType.ToLower().Equals(productTypeInfo.Description.ToLower()));
                                //    if (productPriceInfo != null)
                                //    {
                                //        AddPriceMasterJson(priceMasterDataObj, productPriceInfo);
                                //    }
                                //}
                            }

                        }
                        //}

                    }

                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("UTC_BridgeZohoSync", "ProcessPriceMasterInfo", ex);

            }
            return flag;
        }

        public void AddPriceMasterJson(PriceMasterData priceMasterDataObj, tblPriceMaster productPriceInfo)
        {
            priceMasterInfoCall = new PriceMasterInfoCall();

            string message = string.Empty;
            try
            {
                if (priceMasterDataObj != null)
                {
                    // Update Price Master details to DB                                      
                    priceMasterInfoCall.UpdatePriceMasterInfotoDB(priceMasterDataObj, productPriceInfo);

                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("UTC_BridgeZohoSync", "AddProgramMasterJson", ex);

            }

        }

        #endregion

        #region Pincode Master

        /// <summary>
        /// Method will fetch all the Pincode Master infromation 
        /// from Zoho and add into UTC database
        /// </summary>
        /// <returns>bool</returns>
        public bool ProcessPincodeMasterInfo()
        {
            commonManager = new CommonManager();
            pinCodeRepository = new PinCodeRepository();
            bool flag = false;
            try
            {

                List<PinCodeMaster> pinCodeMasterList = commonManager.GetPincodeMasterList();

                if (pinCodeMasterList != null && pinCodeMasterList.Count > 0)
                {
                    foreach (PinCodeMaster pinCodeMasterObj in pinCodeMasterList)
                    {
                        if (pinCodeMasterObj != null)
                        {
                            tblPinCode pinCodeInfo = pinCodeRepository.GetSingle(x => x.ZipCode.Equals(Convert.ToInt32(pinCodeMasterObj.Pin_Code)));
                            if (pinCodeInfo == null)
                            {
                                //add new pincode
                                AddPincodeMasterJson(pinCodeMasterObj, pinCodeInfo);
                                flag = true;
                            }
                            else
                            {
                                //update existing pincode
                                bool IsExchange = false;
                                if (pinCodeMasterObj.Exch_Active == "Yes")
                                {
                                    IsExchange = true;
                                }
                                else
                                {
                                    IsExchange = false;
                                }

                                if (pinCodeInfo.IsActive != IsExchange)
                                {
                                    pinCodeInfo.IsActive = IsExchange;
                                    pinCodeInfo.ModifiedDate = currentDatetime;
                                    pinCodeRepository.Update(pinCodeInfo);
                                    pinCodeRepository.SaveChanges();
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("UTC_BridgeZohoSync", "ProcessPincodeMasterInfo", ex);

            }
            return flag;
        }

        public void AddPincodeMasterJson(PinCodeMaster pinCodeMasterObj, tblPinCode pinCodeInfo)
        {
            pinCodeMasterInfoCall = new PinCodeMasterInfoCall();
            string message = string.Empty;
            try
            {
                if (pinCodeMasterObj != null)
                {
                    // Update Price Master details to DB                                      
                    pinCodeMasterInfoCall.AddPincodeInfotoDB(pinCodeMasterObj, pinCodeInfo);

                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("UTC_BridgeZohoSync", "AddPincodeMasterJson", ex);

            }

        }

        #endregion


        #region Sponsor

        /// <summary>
        /// Method will fetch all the Sponsor infromation 
        /// from Zoho and add into UTC database
        /// </summary>
        /// <returns>bool</returns>
        public bool ProcessSponsorInfo()
        {
            sponserManager = new SponserManager();
            ExchangeOrderRepository = new ExchangeOrderRepository();
            bool flag = false;
            try
            {
                List<SponserData> sponsorList = sponserManager.GetAllSponser();
                if (sponsorList != null && sponsorList.Count > 0)
                {
                    foreach (SponserData sponserDataObj in sponsorList)
                    {
                        string sponsorID = sponserDataObj.ID;
                        tblExchangeOrder sponserInfo = ExchangeOrderRepository.GetSingle(x => x.ZohoSponsorOrderId.ToLower().Equals(sponsorID.ToLower()));
                        //tblExchangeOrder sponserInfo = ExchangeOrderRepository.GetSingle(x=>x.SponsorOrderNumber.ToLower().Equals(sponserDataObj.Sp_Order_No.ToLower()));
                        if (sponserInfo == null)
                        {
                            AddSponsorJson(sponserDataObj);
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("UTC_BridgeZohoSync", "ProcessSponsorInfo", ex);

            }
            return flag;
        }

        public void AddSponsorJson(SponserData sponserDataObj)
        {
            sponsorInfoCall = new SponsorInfoCall();
            int custId = 0;

            string message = string.Empty;
            try
            {
                if (sponserDataObj != null)
                {
                    // Add Customer details to DB                    
                    custId = sponsorInfoCall.AddCustomerInfotoDB(sponserDataObj);
                    // Add Sponsor details to DB
                    if (custId != 0)
                    {
                        sponsorInfoCall.AddSponsorInfotoDB(sponserDataObj, custId);
                    }
                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("UTC_BridgeZohoSync", "AddSponsorJson", ex);

            }

        }

        #endregion

        #region EVC Approved

        /// <summary>
        /// Method will fetch all the EVC Approved infromation 
        /// from Zoho and add into UTC database
        /// </summary>
        /// <returns>bool</returns>
        public bool ProcessEVCApprovedInfo()
        {
            eVCManager = new EVCManager();
            eVCApprovedRepository = new EVCApprovedRepository();
            bool flag = false;
            try
            {
                List<EvcApprovedData> EvcApprovedList = eVCManager.GetAllEVCApproved();
                if (EvcApprovedList != null && EvcApprovedList.Count > 0)
                {
                    foreach (EvcApprovedData evcApprovedDataObj in EvcApprovedList)
                    {
                        tblEVCApproved eVCApprovedInfo = eVCApprovedRepository.GetSingle(x => x.ZohoEVCApprovedId.ToLower().Equals(evcApprovedDataObj.ID.ToLower()));
                        if (eVCApprovedInfo == null)
                        {
                            AddEVCJson(evcApprovedDataObj);
                        }
                        else
                        {
                            AddEVCJson(evcApprovedDataObj);
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("UTC_BridgeZohoSync", "ProcessEVCApprovedInfo", ex);

            }
            return flag;
        }

        public void AddEVCJson(EvcApprovedData evcApprovedDataObj)
        {
            eVCApprovedInfoCall = new EVCApprovedInfoCall();

            string message = string.Empty;
            try
            {
                if (evcApprovedDataObj != null)
                {
                    // Add EVC approved details to DB                    
                    // priceMasterInfoCall.AddPriceMasterInfotoDB(priceMasterDataObj);
                    eVCApprovedInfoCall.AddEVCApprovedInfotoDB(evcApprovedDataObj);

                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("UTC_BridgeZohoSync", "AddEVCJson", ex);

            }

        }


        #endregion   

    }
}
