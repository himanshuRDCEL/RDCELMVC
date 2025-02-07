using GraspCorn.Common.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RDCEL.DocUpload.BAL.ABBRegistration;
using RDCEL.DocUpload.BAL.SponsorsApiCall;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DataContract.ABBRedemption;
using RDCEL.DocUpload.DataContract.ABBRegistration;
using RDCEL.DocUpload.DataContract.ProductTaxonomy;
using RDCEL.DocUpload.DataContract.ZohoModel;

namespace RDCEL.DocUpload.BAL.SponsorsApiCall
{
    public class ABBOrderMaanger
    {
        BusinessPartnerRepository _businessPartnerRepository;
        BusinessUnitRepository _businessUnitRepository;
        ABBRegistrationRepository _ABBRegistrationRepository;
        AbbRedemptionRepository _abbredemptionRepository;


        /// <summary>
        /// Method to mvoe data from UTC Bridge to Zoho Creator for ABB
        /// </summary>
        /// <param name="month">month number</param>
        /// <returns>bool</returns>
        public bool MoveUTCBridgeToZoho(int month)
        {
            _businessUnitRepository = new BusinessUnitRepository();
            _businessPartnerRepository = new BusinessPartnerRepository();
            _ABBRegistrationRepository = new ABBRegistrationRepository();
            ABBRegManager ABBRegInfo = new ABBRegManager();
            ABBRegistrationDataContract ABBRegistrationDC = null;
            ABBRegistrationFormResponseDataContract ABBRegistrationResponseDC = null;
            ModelNumberRepository _modelNumberRepository = new ModelNumberRepository();
            tblModelNumber modelNoObj = null;
            int ABBRegId = 0;
            DateTime _dateTime = DateTime.Now;
            string msg = string.Empty;
            string fileName = string.Empty;
            string ModelName = string.Empty;
            bool flag = false;
            try
            {
                List<tblABBRegistration> aBBRegistrations = _ABBRegistrationRepository.GetList(x => string.IsNullOrEmpty(x.ZohoABBRegistrationId)
                && (x.CreatedDate != null && (Convert.ToDateTime(x.CreatedDate).Month == month && Convert.ToDateTime(x.CreatedDate).Year == DateTime.Now.Year))
                ).ToList();

                foreach (tblABBRegistration aBBRegistration in aBBRegistrations)
                {
                    if (aBBRegistration != null && aBBRegistration.ABBRegistrationId != 0)
                    {
                        if (string.IsNullOrEmpty(aBBRegistration.RegdNo))
                            aBBRegistration.RegdNo = "A" + UniqueString.RandomNumberByLength(5);
                    }

                    if (aBBRegistration != null)
                    {
                        if (aBBRegistration.ModelNumberId > 0)
                        {
                            modelNoObj = _modelNumberRepository.GetSingle(x => x.ModelNumberId == aBBRegistration.ModelNumberId);
                            if (modelNoObj != null)
                                ModelName = modelNoObj.ModelName;
                        }

                        #region Code to add ABB Reg. details in database
                        ABBRegId = aBBRegistration.ABBRegistrationId;
                        #endregion

                        #region Code to add ABB REg in zoho creator
                        if (ABBRegId > 0)
                        {
                            ABBRegistrationDC = SetZohoABBRegObjectFromUTCBridgeDBObj(aBBRegistration, ModelName);
                            ABBRegistrationResponseDC = ABBRegInfo.AddZohoABBReg(ABBRegistrationDC);
                        }
                        #endregion

                        #region Code to Update Zoho ABB Reg Id in database
                        if (ABBRegistrationResponseDC != null)
                        {
                            if (ABBRegistrationResponseDC.data != null)
                            {
                                if (ABBRegId != 0 && ABBRegistrationResponseDC.data.ID != null)
                                {
                                    //ABBSingleDC = ABBRegInfo.GetABBById(ABBRegistrationResponseDC.data.ID);
                                    if (!string.IsNullOrEmpty(ABBRegistrationDC.Regd_No) && !string.IsNullOrEmpty(ABBRegistrationResponseDC.data.ID))
                                    {
                                        ABBRegId = ABBRegInfo.UpdateABBReg(ABBRegistrationResponseDC.data.ID, ABBRegId, ABBRegistrationDC.Regd_No);

                                    }

                                }
                            }
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBOrderMaanger", "MoveUTCBridgeToZoho", ex);

            }
            return flag;
        }

        #region set Zoho ABB Reg obj
        /// <summary>
        /// Method to set Zoho ABB Reg info 
        /// </summary>
        /// <param name="productOrderDataContract">productOrderDataContract</param>     
        public ABBRegistrationDataContract SetZohoABBRegObjectFromUTCBridgeDBObj(tblABBRegistration aBBRegistration, string ModelName)
        {
            ABBRegistrationDataContract ABBRegistrationObj = null;
            SponserSubCategoryListDataContract SponserSubCategoryListDC = null;
            SponsorCategoryListDataContract SponserCategoryListDC = null;
            BrandMasterListDataContract brandMasterListDC = null;
            ProductSizeListDataContract ProductSizeListDC = null;
            StoreCodeListDataContract storeCodeListDC = null;
            RDCEL.DocUpload.BAL.ZohoCreatorCall.MasterManager masterManager = new RDCEL.DocUpload.BAL.ZohoCreatorCall.MasterManager();
            BusinessPartnerRepository businessPartnerRepository = new BusinessPartnerRepository();
            BrandRepository sponserRepository = new BrandRepository();
            ProductTypeRepository productTypeRepository = new ProductTypeRepository();
            ProductCategoryRepository categoryRepository = new ProductCategoryRepository();
            PriceMasterRepository priceMasterRepository = new PriceMasterRepository();

            try
            {
                if (aBBRegistration != null)
                {
                    ABBRegistrationObj = new ABBRegistrationDataContract();
                    //ABBRegistrationObj.Sponsor_Name = "4186686000000737017";               

                    // Sponsor and Store code 
                    if (aBBRegistration.BusinessPartnerId > 0)
                    {
                        tblBusinessPartner BPObj = businessPartnerRepository.GetSingle(x => x.IsActive == true && x.BusinessPartnerId.Equals(aBBRegistration.BusinessPartnerId));
                        if (BPObj != null)
                        {
                            storeCodeListDC = masterManager.GetAllStoreCodeByCode(BPObj.StoreCode);//masterManager.GetAllStoreCode();
                            if (storeCodeListDC != null)
                            {
                                if (storeCodeListDC.data != null && storeCodeListDC.data.Count > 0)
                                {
                                    ABBRegistrationObj.Sponsor_Name = storeCodeListDC.data[0].Sponsor_Name.ID;
                                    ABBRegistrationObj.Store_Code = storeCodeListDC.data[0].ID;

                                }
                            }

                        }
                    }

                    if (aBBRegistration.FollowupCommunication == true)
                    {
                        ABBRegistrationObj.Mar_Com = "Yes";
                    }

                    ABBRegistrationObj.Regd_No = aBBRegistration.RegdNo;
                    ABBRegistrationObj.Sponsor_Order_No = aBBRegistration.SponsorOrderNo;
                    ABBRegistrationObj.Cust_Name = new CustName();
                    ABBRegistrationObj.Cust_Name.first_name = aBBRegistration.CustFirstName;
                    ABBRegistrationObj.Cust_Name.last_name = aBBRegistration.CustLastName;
                    ABBRegistrationObj.Cust_Mobile = aBBRegistration.CustMobile;
                    ABBRegistrationObj.Cust_E_mail = aBBRegistration.CustEmail;
                    ABBRegistrationObj.Cust_Add_1 = aBBRegistration.CustAddress1;
                    ABBRegistrationObj.Cust_Add_2 = aBBRegistration.CustAddress2;
                    ABBRegistrationObj.Customer_Location = aBBRegistration.Location;
                    ABBRegistrationObj.Cust_Pin_Code = aBBRegistration.CustPinCode;
                    ABBRegistrationObj.Cust_City = aBBRegistration.CustCity;
                    ABBRegistrationObj.Cust_State = aBBRegistration.CustState;

                    // Product category & type
                    if (aBBRegistration.NewProductCategoryTypeId != 0)
                    {
                        tblProductType productTypeObj = productTypeRepository.GetSingle(x => x.Id.Equals(aBBRegistration.NewProductCategoryTypeId));
                        if (productTypeObj != null)
                        {
                            tblProductCategory productCatObj = categoryRepository.GetSingle(x => x.Id.Equals(productTypeObj.ProductCatId));
                            if (productCatObj != null)
                            {
                                // fill Product Category
                                SponserCategoryListDC = masterManager.GetAllCategory();
                                if (SponserCategoryListDC != null)
                                {
                                    if (SponserCategoryListDC.data != null && SponserCategoryListDC.data.Count > 0)
                                    {
                                        CategoryData CategoryData = SponserCategoryListDC.data.Find(x => x.Product_Technology.ToLower().Equals(productCatObj.Code.ToLower()));
                                        if (CategoryData != null)
                                        {
                                            ABBRegistrationObj.New_Prod_Group = CategoryData.ID;

                                        }
                                    }
                                }
                                // fill Product type
                                SponserSubCategoryListDC = masterManager.GetAllSubCategory();
                                if (SponserSubCategoryListDC != null)
                                {
                                    if (SponserSubCategoryListDC.data != null && SponserSubCategoryListDC.data.Count > 0)
                                    {
                                        string category = null;

                                        if (productTypeObj.Code != "RF2" && productTypeObj.Code != "RF3")
                                        {
                                            category = Regex.Replace(productTypeObj.Code, @"[\d]", string.Empty);
                                        }
                                        else
                                        {
                                            category = productTypeObj.Code;
                                        }

                                        SubCategoryData subCategoryData = SponserSubCategoryListDC.data.Find(x => x.Sub_Product_Technology.ToLower().Equals(category.ToLower()));
                                        if (subCategoryData != null)
                                        {
                                            ABBRegistrationObj.New_Prod_Type = subCategoryData.ID;
                                        }
                                    }
                                }

                                // fill Product size
                                ProductSizeListDC = masterManager.GetAllProductSize();
                                if (ProductSizeListDC != null)
                                {
                                    if (ProductSizeListDC.data != null && ProductSizeListDC.data.Count > 0)
                                    {
                                        string size = null;
                                        if (!String.IsNullOrEmpty(productTypeObj.Size))
                                        {
                                            size = Regex.Replace(productTypeObj.Code, "[^0-9.]", "");
                                            //size = Regex.Replace(productTypeObj.Code, @"[\d]", string.Empty);

                                            ProductSize productSize = ProductSizeListDC.data.Find(x => x.Size.ToLower().Equals(size.ToLower()));
                                            if (productSize != null)
                                            {
                                                ABBRegistrationObj.New_Size = productSize.ID;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // Brand
                    if (aBBRegistration.NewBrandId > 0)
                    {
                        tblBrand brandObj = sponserRepository.GetSingle(x => x.Id.Equals(aBBRegistration.NewBrandId));
                        if (brandObj != null)
                        {
                            brandMasterListDC = masterManager.GetAllBrand();
                            if (brandMasterListDC != null)
                            {
                                if (brandMasterListDC.data != null && brandMasterListDC.data.Count > 0)
                                {
                                    BrandMaster brandData = brandMasterListDC.data.Find(x => x.Brand_Name.ToLower().Equals(brandObj.Name.ToLower()));
                                    if (brandData != null)
                                    {
                                        ABBRegistrationObj.New_Brand = brandData.ID;
                                    }
                                }
                            }

                        }
                    }

                    ABBRegistrationObj.Prod_Sr_No = aBBRegistration.ProductSrNo;
                    ABBRegistrationObj.Model_No = ModelName;
                    ABBRegistrationObj.ABB_Plan_Name = aBBRegistration.ABBPlanName;
                    ABBRegistrationObj.Sponsor_Prog_code = aBBRegistration.ABBPlanName;
                    ABBRegistrationObj.HSN_Code_For_ABB_Fees = aBBRegistration.HSNCode;

                    if (aBBRegistration.InvoiceDate != null)
                    {
                        DateTime InvoiceDate = (DateTime)aBBRegistration.InvoiceDate;
                        ABBRegistrationObj.Invoice_Date = InvoiceDate.ToString("dd-MMM-yyyy");
                    }
                    ABBRegistrationObj.Invoice_No = aBBRegistration.InvoiceNo;
                    ABBRegistrationObj.New_Price = aBBRegistration.NewPrice != null ? aBBRegistration.NewPrice.ToString() : "";
                    ABBRegistrationObj.ABB_Fees = aBBRegistration.ABBFees != null ? aBBRegistration.ABBFees.ToString() : "";
                    ABBRegistrationObj.Order_Type = "ABB";
                    //ABBRegistrationObj.Order_Type = aBBRegistration.OrderType;
                    ABBRegistrationObj.Sponsor_Prog_code = aBBRegistration.SponsorProdCode;

                    ABBRegistrationObj.ABB_Price_Id = aBBRegistration.ABBPriceId != null ? aBBRegistration.ABBPriceId.ToString() : "";

                    if (aBBRegistration.UploadDateTime != null)
                    {
                        DateTime UploadDateTime = (DateTime)aBBRegistration.UploadDateTime;
                        ABBRegistrationObj.Upload_Date = UploadDateTime.ToString("dd-MMM-yyyy");
                    }

                    //ABBRegistrationObj.Invoice_Image = aBBRegistration.InvoiceImage;
                    ABBRegistrationObj.ABB_Plan_Period_Months = aBBRegistration.ABBPlanPeriod;
                    ABBRegistrationObj.No_Of_Claim_Period_Months = aBBRegistration.NoOfClaimPeriod;
                    ABBRegistrationObj.Product_Net_Price_Incl_Of_GST = aBBRegistration.ProductNetPrice != null ? aBBRegistration.ProductNetPrice.ToString() : "";
                    if (aBBRegistration.InvoiceImage != null)
                    {
                        string imageUrl = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Content/DB_Files/InvoiceImage/" + aBBRegistration.InvoiceImage;
                        ABBRegistrationObj.Invoice_Image = imageUrl;
                    }

                    ABBRegistrationObj.Sponsor_Status = "Not Imported";
                    ABBRegistrationObj.Approve_this = "No";
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBRegManager", "SetZohoABBRegObjectFromUTCBridgeDBObj", ex);
            }
            return ABBRegistrationObj;
        }
        #endregion


        #region GetVoucherDataForRedemption
        public RedemptionDataContract GetOrderData(int Id)
        {
            _businessUnitRepository = new BusinessUnitRepository();
            _ABBRegistrationRepository = new ABBRegistrationRepository();
            _abbredemptionRepository = new AbbRedemptionRepository();
            RedemptionDataContract RedemptionDataDC = new RedemptionDataContract();
            tblABBRedemption redemptionObj = null;
            tblABBRegistration registrationObj = null;
            tblBusinessUnit businessUnitObj = null;
            try
            {
                if (Id > 0)
                {
                    redemptionObj = _abbredemptionRepository.GetSingle(x => x.IsActive == true && x.RedemptionId == Id);
                    if (redemptionObj != null)
                    {
                        RedemptionDataDC.VoucherCode = redemptionObj.VoucherCode;
                        RedemptionDataDC.VoucherCodeExpDate = redemptionObj.VoucherCodeExpDate;
                        RedemptionDataDC.RedemptionValue = redemptionObj.RedemptionValue;
                        registrationObj = _ABBRegistrationRepository.GetSingle(x => x.IsActive == true && x.ABBRegistrationId == redemptionObj.ABBRegistrationId);
                        if (registrationObj != null)
                        {
                            RedemptionDataDC.BusinessUnitId = registrationObj.BusinessUnitId;
                            businessUnitObj = _businessUnitRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == RedemptionDataDC.BusinessUnitId);
                            if(businessUnitObj!=null && businessUnitObj.LogoName!=null)
                            {
                                RedemptionDataDC.BULogoName = businessUnitObj.LogoName;
                            }
                            else
                            {
                                RedemptionDataDC.ErrorMessage = "BusinessUnit Not found Please check if busiess unit is active and has logo name"; 
                            }
                        }
                        else
                        {
                            RedemptionDataDC.ErrorMessage = "ABB registration data not found";
                        }
                    }
                    else
                    {
                        RedemptionDataDC.ErrorMessage = "ABB redemption data not found";
                    }
                }
                else
                {
                    RedemptionDataDC.ErrorMessage = "order id is not provided";
                }
            }
            catch(Exception ex)
            {
                RedemptionDataDC.ErrorMessage =ex.Message;
                LibLogging.WriteErrorToDB("ABBOrderMaanger", "GetOrderData", ex);
            }
            return RedemptionDataDC;
        }
        #endregion
    }
}
