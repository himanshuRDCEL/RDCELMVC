using GraspCorn.Common.Constant;
using GraspCorn.Common.Enums;
using GraspCorn.Common.Helper;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.UI.WebControls;
using RDCEL.DocUpload.BAL.Common;
using RDCEL.DocUpload.BAL.SponsorsApiCall;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DAL.Helper;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DataContract.ABBRedemption;
using RDCEL.DocUpload.DataContract.ABBRegistration;
using RDCEL.DocUpload.DataContract.MasterModel;
using RDCEL.DocUpload.DataContract.ProductTaxonomy;
using RDCEL.DocUpload.DataContract.WhatsappTemplates;
using Login = RDCEL.DocUpload.DAL.Login;

namespace RDCEL.DocUpload.BAL.ABBRegistration
{

    public class ABBOrderManager
    {
        ProductCategoryMappingRepository _productCategoryMappingRepository;
        LoginDetailsUTCRepository _loginDetailsUTCRepository;
        ModelNumberRepository _modelNumberRepository;
        BusinessUnitRepository _businessUnitRepository;
        ABBPriceMasterRepository _priceMasterRepository;
        ABBRegistrationRepository _ABBRegistrationRepository;
        ProductTypeRepository _productTypeRepository;
        BrandRepository _brandRepository;
        ProductCategoryRepository _productCategoryRepository;
        BusinessPartnerRepository _BusinessPartnerRepository;
        BrandSmartSellRepository _brandMappingRepository;
        ABBPlanMasterRepository _abbPlanMaster;
        CustomerDetailsRepository _customerDetailsRepository;
        WhatsappMessageRepository _whatsAppMessageRepository;
        Logging logging;
        TransactionABBPlanMasterRepository _abbplantransRepository;
        SponsorCategoryMappingRepository _sponsorCategoryMappingRepository;
        /// <summary>
        /// Method to get the list of Category by BUID
        /// </summary>
        /// <param name="buid">buid</param>
        /// <returns>List of ProductCategoryDataContract</returns>
        public List<ProductCategory> GetCategoryListByBUId(string username)
        {
            List<ProductCategory> ProductCategoryDCList = new List<ProductCategory>();
            List<tblProductCategory> tblProductCategories = new List<tblProductCategory>();
            _productCategoryMappingRepository = new ProductCategoryMappingRepository();
            _businessUnitRepository = new BusinessUnitRepository();
            _productCategoryRepository = new ProductCategoryRepository();
            try
            {

                List<tblBUProductCategoryMapping> productCategoryForNew = new List<tblBUProductCategoryMapping>();
                tblBusinessUnit businessUnit = _businessUnitRepository.GetSingle(x => !string.IsNullOrEmpty(x.Email) && x.Email.ToLower().Equals(username.ToLower()));
                if (businessUnit != null)
                {
                    DataTable dt = _productCategoryMappingRepository.GetNewProductCategoryForABB(businessUnit.BusinessUnitId);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        productCategoryForNew = GenericConversionHelper.DataTableToList<tblBUProductCategoryMapping>(dt);
                    }
                }

                if (productCategoryForNew.Count > 0)
                {
                    foreach (var productCategory in productCategoryForNew)
                    {
                        tblProductCategory productObj = _productCategoryRepository.GetSingle(x => x.Id == productCategory.ProductCatId && x.IsActive == true);
                        if (productObj != null)
                        {
                            ProductCategory productcat = new ProductCategory();
                            productcat.Id = productObj.Id;
                            productcat.Description = productObj.Description;
                            productcat.Name = productObj.Name;
                            productcat.Code = productObj.Code;
                            ProductCategoryDCList.Add(productcat);
                        }
                    }
                }
                else
                {
                    ProductCategoryDCList = null;
                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBOrderManager", "GetCategoryListByBUId", ex);
            }
            return ProductCategoryDCList;


        }


        /// <summary>
        /// Method to get the list of Product type by id 
        /// </summary>
        /// <param name="username">username</param>
        /// <returns></returns>
        public List<ProductType> GetProductTypeListByBUId(int catid, int buid)
        {
            _abbPlanMaster = new ABBPlanMasterRepository();
            List<tblABBPlanMaster> planmasterObj = new List<tblABBPlanMaster>();
            List<ProductType> ProductTypeDCList = null;
            List<tblProductType> prodTypeListForABB = new List<tblProductType>();
            _productCategoryMappingRepository = new ProductCategoryMappingRepository();
            _productTypeRepository = new ProductTypeRepository();
            tblProductType productTypeObj = null;
            try
            {

                DataTable dt = new DataTable();
                dt = _abbPlanMaster.GetNewProductCategoryForABB(buid, catid);
                if (dt != null && dt.Rows.Count > 0)
                {
                    planmasterObj = GenericConversionHelper.DataTableToList<tblABBPlanMaster>(dt);

                    if (planmasterObj.Count > 0)
                    {
                        foreach (var item in planmasterObj)
                        {
                            productTypeObj = _productTypeRepository.GetSingle(x => x.Id == item.ProductTypeId && x.IsAllowedForNew == true && x.IsActive == true && x.Size == null);
                            if (productTypeObj != null)
                            {
                                prodTypeListForABB.Add(productTypeObj);
                            }
                        }
                    }
                }
                // List<tblProductType> prodTypeListForABB = _productTypeRepository.GetList(x => x.IsActive == true).ToList();
                if (prodTypeListForABB != null && prodTypeListForABB.Count > 0)
                {
                    // prodTypeListForABB.RemoveAt(0);
                    prodTypeListForABB = prodTypeListForABB.Where(x => x.ProductCatId == catid).ToList();
                    prodTypeListForABB.RemoveAll(x => !string.IsNullOrEmpty(x.Size)); //ADDED LINE TO REMOVE REF SIZE TYPES
                    if (prodTypeListForABB != null && prodTypeListForABB.Count > 0)
                    {
                        ProductTypeDCList = GenericMapper<tblProductType, ProductType>.MapList(prodTypeListForABB);
                    }
                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBOrderManager", "GetProductTypeListByBUId", ex);
            }
            return ProductTypeDCList;


        }

        /// <summary>
        /// list of abbplandetails 
        /// </summary>
        /// <param name="catid"></param>
        /// <param name="subcatid"></param>
        /// <param name="productprice"></param>
        /// <param name="username"></param>
        /// <returns>abbplanmasterDCList</returns>
        public List<abbplanmaster> Getabbplandetails(int productCatId, int productSubCatId, string productPrice, string username)
        {

            List<abbplanmaster> abbplanmasterDCList = new List<abbplanmaster>();
            _businessUnitRepository = new BusinessUnitRepository();
            ABBPlanMasterRepository abbplanmasterRepository = new ABBPlanMasterRepository();
            List<tblABBPlanMaster> abbplanlist = new List<tblABBPlanMaster>();
            _sponsorCategoryMappingRepository = new SponsorCategoryMappingRepository();
            try
            {
                tblBusinessUnit businessUnit = _businessUnitRepository.GetSingle(x => !string.IsNullOrEmpty(x.Email) && x.Email.ToLower().Equals(username.ToLower()) && x.IsActive == true);
                tblSponsorCategoryMapping tblSponsorCategory = _sponsorCategoryMappingRepository.GetSingle(x => x.BusinessUnitId == businessUnit.BusinessUnitId && x.BUCategoryId == productCatId && x.IsActive == true);
                if (businessUnit.IsBUCatIdOn == true && tblSponsorCategory != null)
                {
                    productCatId = (int)tblSponsorCategory.CategoryId;
                }
                if (productCatId > 0 && productSubCatId > 0 && businessUnit.BusinessUnitId > 0 && productPrice != null || productPrice != string.Empty)
                {
                    abbplanlist = abbplanmasterRepository.GetList(x => x.ProductCatId == productCatId && x.ProductTypeId == productSubCatId && x.BusinessUnitId == businessUnit.BusinessUnitId && x.IsActive == true).ToList();
                    if (abbplanlist.Count > 0)
                    {
                        foreach (var item in abbplanlist)
                        {
                            abbplanmaster abbplan = new abbplanmaster();
                            abbplan.Assured_BuyBack_Percentage = item.Assured_BuyBack_Percentage.ToString();
                            abbplan.From_month = item.From_Month.ToString();
                            abbplan.To_month = item.To_Month.ToString();
                            abbplanmasterDCList.Add(abbplan);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBOrderManager", "Getabbplandetails", ex);
            }
            return abbplanmasterDCList;
        }
        /// <summary>
        /// abb plan price details 
        /// </summary>
        /// <param name="productCatId"></param>
        /// <param name="producttypeId"></param>
        /// <param name="productValue"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public plandetail GetabbPlanPrice(int productCatId, int producttypeId, string productValue, string username)
        {
            _priceMasterRepository = new ABBPriceMasterRepository();
            ABBPlanMasterRepository _planMasterepository = new ABBPlanMasterRepository();
            List<tblABBPriceMaster> abbplanpriceObj = new List<tblABBPriceMaster>();
            string planPrice = string.Empty;
            _businessUnitRepository = new BusinessUnitRepository();
            plandetail plan = new plandetail();
            _sponsorCategoryMappingRepository = new SponsorCategoryMappingRepository();
            try
            {
                tblBusinessUnit businessUnit = _businessUnitRepository.GetSingle(x => !string.IsNullOrEmpty(x.Email) && x.Email.ToLower().Equals(username.ToLower()) && x.IsActive == true);
                if (businessUnit != null)
                {                    
                    var IsAbbDayConfig = businessUnit.IsAbbDayConfig == null ? false : ((bool)businessUnit.IsAbbDayConfig == true ? true : false);
                    if (IsAbbDayConfig == true)
                    {
                        plan.AbbDayDiff = (int)businessUnit.AbbDayDiff > 0 ? (int)businessUnit.AbbDayDiff : 0;
                    }
                    else
                    {
                        var VarAbbDayDiff = ConfigurationManager.AppSettings["DefaultABBDayDiff"];
                        plan.AbbDayDiff = Convert.ToInt32(VarAbbDayDiff);
                    }
                }

                tblSponsorCategoryMapping tblSponsorCategory = _sponsorCategoryMappingRepository.GetSingle(x => x.BusinessUnitId == businessUnit.BusinessUnitId && x.BUCategoryId == productCatId && x.IsActive == true);
                if (businessUnit.IsBUCatIdOn == true && tblSponsorCategory != null)
                {
                    productCatId = (int)tblSponsorCategory.CategoryId;
                }

                if (productCatId > 0 && producttypeId > 0 && businessUnit.BusinessUnitId > 0 && productValue != string.Empty || productValue != null)
                {
                    abbplanpriceObj = _priceMasterRepository.GetList(x => x.ProductCatId == productCatId && x.ProductTypeId == producttypeId
                    && x.BusinessUnitId == businessUnit.BusinessUnitId && x.Price_Start_Range <= Convert.ToInt32(productValue) && x.Price_End_Range >= Convert.ToInt32(productValue) && x.IsActive == true).ToList();
                    if (abbplanpriceObj != null && abbplanpriceObj.Count > 0)
                    {
                        tblABBPlanMaster planmasterObj = _planMasterepository.GetSingle(x => x.ProductCatId == productCatId && x.ProductTypeId == producttypeId && x.BusinessUnitId == businessUnit.BusinessUnitId && x.IsActive == true);
                        if (planmasterObj != null)
                        {
                            int productPrice = Convert.ToInt32(productValue);
                            foreach (var item in abbplanpriceObj)
                            {
                                if (item.Fees_Applicable_Amt > 0)
                                {
                                    if (productPrice >= item.Price_Start_Range && productPrice <= item.Price_End_Range)
                                    {
                                        //code to calculate Gst amount
                                        ///formula to calculate gst <>GSt value=GST Inclusive Price * GST Rate /(100 + GST Rate Percentage)</>
                                        ///formula to calculate original cost <>original cost=GST Inclusive Price * 100/(100 + GST Rate Percentage)</>

                                        if (businessUnit.GSTType == Convert.ToInt32(ABBPlanEnum.GstInclusive))
                                        {
                                            decimal basicvalue = Convert.ToDecimal(item.Fees_Applicable_Amt);
                                            basicvalue = basicvalue * 100 / (100 + Convert.ToDecimal(item.GSTInclusive));
                                            plan.BaseValue = basicvalue.ToString();
                                            decimal gstvalue = (Convert.ToDecimal(item.Fees_Applicable_Amt) - basicvalue);
                                            plan.Cgst =(gstvalue / 2).ToString();
                                            plan.Sgst =(gstvalue / 2).ToString();
                                            plan.planprice =item.Fees_Applicable_Amt.ToString();
                                            plan.planduration =item.Plan_Period_in_Months.ToString();
                                            plan.NoClaimPeriod = planmasterObj.NoClaimPeriod;
                                            plan.planName = planmasterObj.ABBPlanName;
                                        }
                                        else if (businessUnit.GSTType == Convert.ToInt32(ABBPlanEnum.GstExclusive))
                                        {
                                            decimal abbplanfees = Convert.ToDecimal(item.Fees_Applicable_Amt);
                                            decimal gstvalue = Convert.ToDecimal(item.GSTExclusive);
                                            decimal originalcost = (abbplanfees * gstvalue) / 100;
                                            originalcost = originalcost + abbplanfees;
                                            decimal gstvaluessndc = (originalcost- abbplanfees)/2;
                                            plan.BaseValue = abbplanfees.ToString();
                                            plan.Sgst = gstvaluessndc.ToString();
                                            plan.Cgst = gstvaluessndc.ToString();
                                            plan.planprice = originalcost.ToString();
                                            plan.planduration = item.Plan_Period_in_Months.ToString();
                                            plan.NoClaimPeriod = planmasterObj.NoClaimPeriod;
                                            plan.planName = planmasterObj.ABBPlanName;
                                        }
                                        else
                                        {
                                            decimal abbplanfees = Convert.ToDecimal(item.Fees_Applicable_Amt);
                                            plan.BaseValue = abbplanfees.ToString();
                                            plan.Sgst = "0";
                                            plan.Cgst = "0";
                                            plan.planprice = abbplanfees.ToString();
                                            plan.planduration = item.Plan_Period_in_Months.ToString();
                                            plan.NoClaimPeriod = planmasterObj.NoClaimPeriod;
                                            plan.planName = planmasterObj.ABBPlanName;
                                        }

                                    }
                                }
                                else if (item.Fees_Applicable_Percentage > 0)
                                {
                                    if (productPrice >= item.Price_Start_Range && productPrice <= item.Price_End_Range)
                                    {
                                        if (businessUnit.GSTType == Convert.ToInt32(ABBPlanEnum.GstInclusive))
                                        {
                                            decimal netproductvalue = Convert.ToDecimal(productValue);
                                            decimal Fees_percentage = Convert.ToDecimal(item.Fees_Applicable_Percentage);
                                            var planprice = (netproductvalue * Fees_percentage) / 100;
                                            decimal basevalue = planprice * 100 / (100 + Convert.ToDecimal(item.GSTInclusive));
                                            decimal gstvalue = planprice-basevalue;
                                            gstvalue = gstvalue / 2;
                                            plan.Cgst = gstvalue.ToString();
                                            plan.Sgst = gstvalue.ToString();
                                            plan.BaseValue = basevalue.ToString();
                                            plan.planprice = planprice.ToString();
                                            plan.planduration = item.Plan_Period_in_Months.ToString();
                                            plan.NoClaimPeriod = planmasterObj.NoClaimPeriod;
                                            plan.planName = planmasterObj.ABBPlanName;
                                        }
                                        else if (businessUnit.GSTType == Convert.ToInt32(ABBPlanEnum.GstExclusive))
                                        {
                                            decimal netproductvalue = Convert.ToDecimal(productValue);
                                            decimal FeesPercentage = Convert.ToDecimal(item.Fees_Applicable_Percentage);
                                            decimal gstvalue = Convert.ToDecimal(item.GSTExclusive);
                                            var planprice = (netproductvalue * FeesPercentage) / 100;
                                            decimal originalcost = (planprice * gstvalue) / 100;
                                            originalcost = originalcost + planprice;
                                            gstvalue = (originalcost- planprice) / 2;
                                            plan.Cgst = gstvalue.ToString();
                                            plan.Sgst = gstvalue.ToString();
                                            plan.BaseValue = planprice.ToString();
                                            plan.planprice = originalcost.ToString();
                                            plan.planduration = item.Plan_Period_in_Months.ToString();
                                            plan.NoClaimPeriod = planmasterObj.NoClaimPeriod;
                                            plan.planName = planmasterObj.ABBPlanName;
                                        }
                                        else
                                        {
                                            decimal netproductvalue = Convert.ToDecimal(productValue);
                                            decimal FeesPercentage = Convert.ToDecimal(item.Fees_Applicable_Percentage);
                                            var planprice = (netproductvalue * FeesPercentage) / 100;
                                            plan.Cgst = "0";
                                            plan.Sgst = "0";
                                            plan.BaseValue = planprice.ToString();
                                            plan.planprice = planprice.ToString();
                                            plan.planduration = item.Plan_Period_in_Months.ToString();
                                            plan.NoClaimPeriod = planmasterObj.NoClaimPeriod;
                                            plan.planName = planmasterObj.ABBPlanName;
                                        }
                                    }
                                }

                            }
                        }

                    }
                    else
                    {
                        plan = null;
                    }

                }

                if (plan != null)
                {
                    double amount = Convert.ToDouble(plan.BaseValue);
                    string amountnew = String.Format("{0:0.00}", amount);
                    double gst = Convert.ToDouble(plan.Cgst);
                    string gstamount = String.Format("{0:0.00}", gst);
                    plan.BaseValue = amountnew;
                    plan.Cgst = gstamount;
                    plan.Sgst = gstamount;
                    double amountabbfees = Convert.ToDouble(plan.planprice);
                    string amountnewabbfees = String.Format("{0:0.00}", amountabbfees);
                    plan.planprice =amountnewabbfees;
                    
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBOrderManager", "GetabbPlanPrice", ex);
            }
            return plan;
        }
        /// <summary>
        /// object of Brand which return brand id with respect to login user 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public BrandName GetYourBrandName(string username)
        {
            BrandName brandName = null;
            //tblBrand tblBrand = null;
            Login login = null;
            _loginDetailsUTCRepository = new LoginDetailsUTCRepository();
            _brandRepository = new BrandRepository();
            _businessUnitRepository = new BusinessUnitRepository();
            try
            {
                login = _loginDetailsUTCRepository.GetSingle(x => !string.IsNullOrEmpty(x.username) && x.username.ToLower().Equals(username.ToLower()));
                //tblBusinessUnit businessUnit = _businessUnitRepository.GetSingle(x => !string.IsNullOrEmpty(x.Email) && x.Email.ToLower().Equals(username.ToLower()));
                if (login != null)
                {
                    tblBrand tblBrandName = _brandRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == login.SponsorId);
                    if (tblBrandName != null)
                    {
                        brandName = GenericMapper<tblBrand, BrandName>.MapObject(tblBrandName);
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBOrderManager", "GetBrandName", ex);
            }
            return brandName;
        }

        /// <summary>
        /// list of model number by login user  
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public List<ModelNumberModel> GetListOfModelNumbers(string username, int productCatId, int productSubCatId)
        {
            List<ModelNumberModel> modelNumberModels = null;
            _modelNumberRepository = new ModelNumberRepository();
            _businessUnitRepository = new BusinessUnitRepository();
            try
            {
                tblBusinessUnit businessUnit = _businessUnitRepository.GetSingle(x => !string.IsNullOrEmpty(x.Email) && x.Email.ToLower().Equals(username.ToLower()));
                if (businessUnit != null)
                {
                    List<tblModelNumber> tblModelNumbers = _modelNumberRepository.GetList(x => x.IsActive == true && x.BusinessUnitId == businessUnit.BusinessUnitId && x.ProductCategoryId == productCatId && x.ProductTypeId == productSubCatId).ToList();
                    if (tblModelNumbers != null && tblModelNumbers.Count > 0)
                    {
                        modelNumberModels = GenericMapper<tblModelNumber, ModelNumberModel>.MapList(tblModelNumbers);

                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBOrderManager", "GetListOfModelNumbers", ex);
            }
            return modelNumberModels;
        }


        /// <summary>
        /// Get all brand details 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public List<BrandName> GetAllBrandName(string username, int catid)
        {
            List<BrandName> brandName = new List<BrandName>();

            Login login = null;
            _loginDetailsUTCRepository = new LoginDetailsUTCRepository();
            _brandRepository = new BrandRepository();
            _brandMappingRepository = new BrandSmartSellRepository();
            List<tblBrandSmartBuy> brandDetails = null;

            try
            {
                login = _loginDetailsUTCRepository.GetSingle(x => !string.IsNullOrEmpty(x.username) && x.username.ToLower().Equals(username.ToLower()));
                //tblBusinessUnit businessUnit = _businessUnitRepository.GetSingle(x => !string.IsNullOrEmpty(x.Email) && x.Email.ToLower().Equals(username.ToLower()));
                if (login != null)
                {
                    DataTable dt = _brandMappingRepository.GetBrandistbyBU(catid, Convert.ToInt32(login.SponsorId));
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        brandDetails = GenericConversionHelper.DataTableToList<tblBrandSmartBuy>(dt);
                    }
                    if (brandDetails.Count > 0)
                    {
                        foreach (var productCategory in brandDetails)
                        {
                            BrandName tblBrand = new BrandName();
                            if (productCategory != null)
                            {
                                tblBrand.Name = productCategory.Name;
                                tblBrand.Id = productCategory.Id;
                                brandName.Add(tblBrand);
                            }
                        }
                    }

                    // brandName = GenericMapper<tblBrandSmartBuy, BrandName>.MapList(brandName);

                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBOrderManager", "GetBrandName", ex);
            }
            return brandName;
        }
        /// <summary>
        /// Create Abb Order
        /// </summary>
        /// <param name="aBBProductOrderDataContract"></param>
        /// <returns>aBBOrderResponse</returns>
        public ABBOrderResponseDataContract CreateAbbOrder(ABBOrderRequestDataContract aBBOrderRequestDataContract, string username)
        {
            ABBRegManager ABBRegInfo = new ABBRegManager();
            _customerDetailsRepository = new CustomerDetailsRepository();
            ABBProductOrderDataContract aBBProductOrderDataContract = null;
            ABBOrderErrorDataContract error = new ABBOrderErrorDataContract();
            ABBOrderResponseDataContract aBBOrderResponse = new ABBOrderResponseDataContract();
            AbbPlanData abbPlanData = new AbbPlanData();
            aBBOrderResponse.RequestErrorMessage = string.Empty;
            _BusinessPartnerRepository = new BusinessPartnerRepository();
            _businessUnitRepository = new BusinessUnitRepository();
            //tblBrand tblBrand = null;
            int result = 0;
            int customerid = 0;
            decimal NetProductValue = 0;
            DateTime _dateTime = DateTime.Now;
            Login login = null;
            _loginDetailsUTCRepository = new LoginDetailsUTCRepository();
            _brandRepository = new BrandRepository();
            _ABBRegistrationRepository = new ABBRegistrationRepository();
            tblCustomerDetail customerDetailsObj = new tblCustomerDetail();
            ABBPlanTransactionResponse abbplantransactionnResposne = new ABBPlanTransactionResponse();
            WhatasappResponse whatssappresponseDC = null;
            string responseforWhatasapp = string.Empty;
            _whatsAppMessageRepository = new WhatsappMessageRepository();
            string WhatssAppStatusEnum = string.Empty;
            string ResponseCode = string.Empty;
            logging = new Logging();
            _sponsorCategoryMappingRepository = new SponsorCategoryMappingRepository();
            _productTypeRepository = new ProductTypeRepository();
            try
            {
                customerDetailsObj = ABBRegInfo.SetcustomerDetailsObjAPI(aBBOrderRequestDataContract);
                {
                    _customerDetailsRepository.Add(customerDetailsObj);
                    _customerDetailsRepository.SaveChanges();
                    customerid = customerDetailsObj.Id;
                }
                tblABBRegistration tblABBRegistration = new tblABBRegistration();
                login = _loginDetailsUTCRepository.GetSingle(x => !string.IsNullOrEmpty(x.username) && x.username.ToLower().Equals(username.ToLower()));
                tblBusinessUnit businessUnit = _businessUnitRepository.GetSingle(x => !string.IsNullOrEmpty(x.Email) && x.Email.ToLower().Equals(username.ToLower()) && x.IsActive == true);
                if (login != null && businessUnit.BusinessUnitId > 0)
                {

                    tblSponsorCategoryMapping tblSponsorCategory = _sponsorCategoryMappingRepository.GetSingle(x => x.BusinessUnitId == businessUnit.BusinessUnitId && x.BUCategoryId == aBBOrderRequestDataContract.NewProductCategoryId && x.IsActive == true);
                    if (businessUnit.IsBUCatIdOn == true && tblSponsorCategory != null)
                    {
                        aBBOrderRequestDataContract.NewProductCategoryId = tblSponsorCategory.CategoryId;
                    }

                    NetProductValue = Convert.ToDecimal(aBBOrderRequestDataContract.ProductNetPrice);
                    abbPlanData = GetAbbPlanData(businessUnit.BusinessUnitId, Convert.ToInt32(aBBOrderRequestDataContract.NewProductCategoryId), Convert.ToInt32(aBBOrderRequestDataContract.NewProductCategoryTypeId), Convert.ToInt32(businessUnit.GSTType), Convert.ToInt32(businessUnit.MarginType), NetProductValue);
                    aBBProductOrderDataContract = new ABBProductOrderDataContract();
                    aBBProductOrderDataContract = GenericMapper<ABBOrderRequestDataContract, ABBProductOrderDataContract>.MapObject(aBBOrderRequestDataContract);

                   
                    tblProductType productType = _productTypeRepository.GetSingle(x => x.Id == aBBProductOrderDataContract.NewProductCategoryTypeId && x.ProductCatId == aBBProductOrderDataContract.NewProductCategoryId);
                    if(productType == null)
                    {
                        aBBOrderResponse.RequestErrorMessage = "Product Category Id and Product Type Id should be according to the system";
                        return aBBOrderResponse;
                    }
                  
                    aBBProductOrderDataContract.BusinessUnitId = login.SponsorId;
                    aBBProductOrderDataContract.BUName = businessUnit.Name;
                    aBBProductOrderDataContract.RegdNo = "ABB" + UniqueString.RandomNumberByLength(7);
                    aBBProductOrderDataContract.UploadDateTime = DateTime.Now;
                    aBBProductOrderDataContract.BusinessPartnerId = login.BusinessPartnerId;
                    aBBProductOrderDataContract.CreatedDate = DateTime.Now;
                    aBBProductOrderDataContract.ModifiedDate = DateTime.Now;
                    aBBProductOrderDataContract.IsActive = true;
                    aBBProductOrderDataContract.ABBFees = abbPlanData.abbplanfees;
                    aBBProductOrderDataContract.ABBPlanName = abbPlanData.abbplanname;
                    aBBProductOrderDataContract.ABBPlanPeriod = abbPlanData.abbplanperiod;
                    aBBProductOrderDataContract.NoOfClaimPeriod = abbPlanData.noclaimperiod;
                    aBBProductOrderDataContract.BusinessUnitMargin = abbPlanData.BUMargin;
                    aBBProductOrderDataContract.DealerMargin = abbPlanData.BPMargin;
                    aBBProductOrderDataContract.PlanPrice = (abbPlanData.abbplanfees).ToString();
                    if (aBBOrderRequestDataContract.PlanPrice != null)
                    {
                        aBBProductOrderDataContract.ABBFees = Convert.ToDecimal(aBBOrderRequestDataContract.PlanPrice);
                    }
                    if (!string.IsNullOrEmpty(aBBProductOrderDataContract.Base64StringValue))
                    {
                        byte[] bytes = System.Convert.FromBase64String(aBBProductOrderDataContract.Base64StringValue);
                        if (bytes != null)
                        {
                            aBBProductOrderDataContract.imageName = aBBProductOrderDataContract.RegdNo + _dateTime.ToString("yyyyMMddHHmmssFFF") + Path.GetExtension("image.jpeg");
                            string rootPath = @HostingEnvironment.ApplicationPhysicalPath;
                            string filePath = ConfigurationManager.AppSettings["InvoiceImage"].ToString() + aBBProductOrderDataContract.imageName;
                            System.IO.File.WriteAllBytes(rootPath + filePath, bytes);
                            aBBProductOrderDataContract.InvoiceImage = aBBProductOrderDataContract.imageName;
                        }
                    }
                    if (login.id > 0 && login != null)
                    {
                        if (!string.IsNullOrEmpty(aBBOrderRequestDataContract.StoreCode))
                        {
                            tblBusinessPartner tblBusinessPartner = _BusinessPartnerRepository.GetSingle(x => x.IsActive == true && x.StoreCode == aBBOrderRequestDataContract.StoreCode);
                            if (tblBusinessPartner != null)
                            {
                                aBBProductOrderDataContract.IsdefferedAbb = tblBusinessPartner.IsDefferedAbb;
                                aBBProductOrderDataContract.StoreName = tblBusinessPartner.Description;
                                aBBProductOrderDataContract.StoreCode = tblBusinessPartner.StoreCode;
                                aBBProductOrderDataContract.BusinessPartnerId = tblBusinessPartner.BusinessPartnerId;
                            }
                        }
                        else
                        {
                            tblBusinessPartner tblBusinessPartner = _BusinessPartnerRepository.GetSingle(x => x.IsActive == true && x.BusinessPartnerId == login.BusinessPartnerId);
                            if (tblBusinessPartner != null)
                            {
                                aBBProductOrderDataContract.IsdefferedAbb = tblBusinessPartner.IsDefferedAbb;
                                aBBProductOrderDataContract.StoreName = tblBusinessPartner.Description;
                                aBBProductOrderDataContract.StoreCode = tblBusinessPartner.StoreCode;
                            }
                        }
                    }
                    tblABBRegistration = GenericMapper<ABBProductOrderDataContract, tblABBRegistration>.MapObject(aBBProductOrderDataContract);
                    tblABBRegistration.CustomerId = customerid;
                    tblABBRegistration.AbbApprove = false;
                    tblABBRegistration.StatusId = Convert.ToInt32(StatusEnum.PaymentUnsuccessfull);
                    tblABBRegistration.PaymentStatus = false;
                    tblABBRegistration.BaseValue = abbPlanData.BaseValue;
                    tblABBRegistration.Sgst = abbPlanData.Sgst;
                    tblABBRegistration.Cgst = abbPlanData.Cgst;
                    if(tblABBRegistration.ModelNumberId == 0)
                    {
                        tblABBRegistration.ModelNumberId = null;
                    }

                    if (tblABBRegistration.NewBrandId > 0)
                    {
                        tblABBRegistration.NewBrandId = tblABBRegistration.NewBrandId;
                    }
                    else
                    {
                        tblABBRegistration.NewBrandId = 2008;
                    }

                    _ABBRegistrationRepository.Add(tblABBRegistration);
                    result = _ABBRegistrationRepository.SaveChanges();
                    aBBProductOrderDataContract.ABBRegistrationId = tblABBRegistration.ABBRegistrationId;
                    abbplantransactionnResposne = ABBRegInfo.GetABBPlanDetailsAPI(aBBProductOrderDataContract);

                    if (result != 0)
                    {
                        aBBOrderResponse.OrderId = tblABBRegistration.ABBRegistrationId;
                        aBBOrderResponse.orderRegdNo = tblABBRegistration.RegdNo;
                    }
                    var finaldate = (DateTime)_dateTime;
                    aBBOrderResponse.Orderdate = finaldate.Date.ToShortDateString();
                    if (businessUnit.BusinessUnitId == Convert.ToInt32(BusinessUnitEnum.PineLabs))
                    {
                        string baseUrl = ConfigurationManager.AppSettings["BaseUrl"].ToString();
                        string PineLabsPage = ConfigurationManager.AppSettings["PineLabsLinkABB"].ToString();
                        string CustomerDetailsLink = baseUrl + "" + PineLabsPage + "" + aBBProductOrderDataContract.RegdNo;
                        #region TO send WhatsappNotificatio for CustomerDetails link Settelment
                        PersonalDdetailsLinkWhatsappTemplate whatsappObj = new PersonalDdetailsLinkWhatsappTemplate();
                        whatsappObj.userDetails = new UserDetails();
                        whatsappObj.notification = new PersonalDetailsNOtification();
                        whatsappObj.notification.@params = new PersonalDetailsLinkOnWhatsapp();
                        whatsappObj.userDetails.number = aBBOrderRequestDataContract.CustMobile;
                        whatsappObj.notification.sender = ConfigurationManager.AppSettings["Yello.aiSenderNumber"].ToString();
                        whatsappObj.notification.type = ConfigurationManager.AppSettings["Yellow.aiMesssaheType"].ToString();
                        whatsappObj.notification.templateId = NotificationConstants.PineLabsCustomerDetails;
                        //whatsappObj.notification.@params.RegdNo = productOrderDataContract.RegdNo.ToString();
                        whatsappObj.notification.@params.Link = CustomerDetailsLink;
                        string url = ConfigurationManager.AppSettings["Yellow.AiUrl"].ToString();
                        IRestResponse response = WhatsappNotificationManager.Rest_InvokeWhatsappserviceCall(url, Method.POST, whatsappObj);
                        ResponseCode = response.StatusCode.ToString();
                        WhatssAppStatusEnum = ExchangeOrderManager.GetEnumDescription(WhatssAppEnum.SuccessCode);
                        if (ResponseCode == WhatssAppStatusEnum)
                        {
                            responseforWhatasapp = response.Content;
                            if (responseforWhatasapp != null)
                            {
                                whatssappresponseDC = JsonConvert.DeserializeObject<WhatasappResponse>(responseforWhatasapp);
                                tblWhatsAppMessage whatsapObj = new tblWhatsAppMessage();
                                whatsapObj.TemplateName = NotificationConstants.PineLabsCustomerDetails;
                                whatsapObj.IsActive = true;
                                whatsapObj.PhoneNumber = aBBOrderRequestDataContract.CustMobile;
                                whatsapObj.SendDate = DateTime.Now;
                                whatsapObj.msgId = whatssappresponseDC.msgId;
                                _whatsAppMessageRepository.Add(whatsapObj);
                                _whatsAppMessageRepository.SaveChanges();
                            }
                            else
                            {
                                string ExchOrderObj = JsonConvert.SerializeObject(aBBOrderRequestDataContract);
                                logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", aBBProductOrderDataContract.RegdNo, ExchOrderObj);
                            }
                        }
                        else
                        {
                            string ExchOrderObj = JsonConvert.SerializeObject(aBBOrderRequestDataContract);
                            logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", aBBProductOrderDataContract.RegdNo, ExchOrderObj);
                        }
                        #endregion
                    }
                }

            }
            catch (Exception ex)
            {
                aBBOrderResponse.RequestErrorMessage = "Exception come while uploading the data : " + ex.Message;
                LibLogging.WriteErrorToDB("ABBOrderManager", "CreateAbbOrder", ex);
            }

            return aBBOrderResponse;
        }

        public static bool TryGetFromBase64String(string input, out byte[] output)
        {
            output = null;
            try
            {
                output = Convert.FromBase64String(input);
                return true;
            }
            catch (FormatException ex)
            {
                LibLogging.WriteErrorToDB("ABBOrderManager", "TryGetFromBase64String", ex);
                return false;
            }
        }
        /// <summary>
        /// </summary>
        /// <param name="BusinessUnitId"></param>
        /// <param name="NewProdutCatId"></param>
        /// <param name="NewProductTypeId"></param>
        /// <returns></returns>
        #region  Code to get plan details and plan price on basis of new productcatid,typeid and business unit id
        public AbbPlanData GetAbbPlanData(int BusinessUnitId, int NewProdutCatId, int NewProductTypeId, int GstType, int Margintype, decimal netProductprice)
        {
            _abbPlanMaster = new ABBPlanMasterRepository();
            _priceMasterRepository = new ABBPriceMasterRepository();
            AbbPlanData plandata = new AbbPlanData();
            tblABBPlanMaster planmasterObj = new tblABBPlanMaster();
            tblABBPriceMaster priceMasterObj = new tblABBPriceMaster();
            decimal basevalue = 0;
            try
            {
                if (BusinessUnitId > 0 && NewProdutCatId > 0 && NewProductTypeId > 0)
                {
                    planmasterObj = _abbPlanMaster.GetSingle(x => x.BusinessUnitId == BusinessUnitId && x.ProductCatId == NewProdutCatId && x.ProductTypeId == NewProductTypeId && x.IsActive == true);
                    if (planmasterObj != null)
                    {
                        priceMasterObj = _priceMasterRepository.GetSingle(x => x.IsActive == true && x.ProductCatId == NewProdutCatId && x.ProductTypeId == NewProductTypeId && x.BusinessUnitId == BusinessUnitId && x.Price_Start_Range <= netProductprice && x.Price_End_Range >= netProductprice);
                        if (priceMasterObj != null)
                        {

                            if (priceMasterObj.Fees_Applicable_Amt > 0)
                            {
                                //code to calculate Gst amount
                                ///formula to calculate gst <>GSt value=GST Inclusive Price * GST Rate /(100 + GST Rate Percentage)</>
                                ///formula to calculate original cost <>original cost=GST Inclusive Price * 100/(100 + GST Rate Percentage)</>
                                ///
                                if (GstType == Convert.ToInt32(ABBPlanEnum.GstExclusive))
                                {

                                    if (netProductprice >= priceMasterObj.Price_Start_Range && netProductprice <= priceMasterObj.Price_End_Range)
                                    {
                                        decimal abbplanAmount = Convert.ToDecimal(priceMasterObj.Fees_Applicable_Amt);
                                        basevalue = abbplanAmount;
                                        decimal exclusivegstvalue = Convert.ToDecimal(priceMasterObj.GSTExclusive);
                                        decimal gstvalue = (abbplanAmount * exclusivegstvalue) / 100;
                                        abbplanAmount = abbplanAmount + gstvalue;
                                        exclusivegstvalue = abbplanAmount - basevalue;
                                        plandata.abbplanfees = abbplanAmount;
                                        plandata.Cgst = exclusivegstvalue / 2;
                                        plandata.Sgst = exclusivegstvalue / 2;
                                    }

                                }
                                else if(GstType==Convert.ToInt32(ABBPlanEnum.GstInclusive))
                                {
                                    if (netProductprice >= priceMasterObj.Price_Start_Range && netProductprice <= priceMasterObj.Price_End_Range)
                                    {
                                        plandata.abbplanfees = Convert.ToDecimal(priceMasterObj.Fees_Applicable_Amt);
                                        decimal originalcost = Convert.ToDecimal((priceMasterObj.Fees_Applicable_Amt * 100) / (100 + priceMasterObj.GSTInclusive));
                                        decimal gstValue = Convert.ToDecimal(priceMasterObj.GSTInclusive);
                                        basevalue = originalcost;
                                        gstValue = plandata.abbplanfees - basevalue;
                                        plandata.Cgst = gstValue / 2;
                                        plandata.Sgst = gstValue / 2;
                                    }
                                }
                                else
                                {
                                    if (netProductprice >= priceMasterObj.Price_Start_Range && netProductprice <= priceMasterObj.Price_End_Range)
                                    {
                                        plandata.abbplanfees = Convert.ToDecimal(priceMasterObj.Fees_Applicable_Amt);
                                        plandata.BaseValue = plandata.abbplanfees;
                                        plandata.Cgst = 0;
                                        plandata.Sgst = 0;
                                    }
                                }
                            }
                            else if (priceMasterObj.Fees_Applicable_Percentage > 0)
                            {
                                //code to calculate Gst amount
                                ///formula to calculate gst <>GSt value=GST Inclusive Price * GST Rate /(100 + GST Rate Percentage)</>
                                ///formula to calculate original cost <>original cost=GST Inclusive Price * 100/(100 + GST Rate Percentage)</>
                                ///

                                if (GstType == Convert.ToInt32(ABBPlanEnum.GstExclusive))
                                {
                                    if (priceMasterObj.Price_Start_Range > 0 && priceMasterObj.Price_End_Range > 0)
                                    {
                                        if (netProductprice >= priceMasterObj.Price_Start_Range && netProductprice <= priceMasterObj.Price_End_Range)
                                        {
                                            decimal exclusivegstvalue = Convert.ToDecimal(priceMasterObj.GSTExclusive);
                                            decimal abbplanAmount = Convert.ToDecimal(priceMasterObj.Fees_Applicable_Percentage);
                                            abbplanAmount = (netProductprice * abbplanAmount) / 100;
                                            basevalue = abbplanAmount;
                                            decimal gstvalue = (abbplanAmount * exclusivegstvalue) / 100;
                                            abbplanAmount = abbplanAmount + gstvalue;
                                            exclusivegstvalue = abbplanAmount - basevalue;
                                            plandata.abbplanfees = abbplanAmount;
                                            plandata.Cgst = exclusivegstvalue / 2;
                                            plandata.Sgst = exclusivegstvalue / 2;
                                        }
                                    }
                                }
                                else if(GstType==Convert.ToInt32(ABBPlanEnum.GstInclusive))
                                {
                                    if (netProductprice >= priceMasterObj.Price_Start_Range && netProductprice <= priceMasterObj.Price_End_Range)
                                    {

                                        decimal abbplanAmount = Convert.ToDecimal(priceMasterObj.Fees_Applicable_Percentage);
                                        decimal gstinclusivevalue = Convert.ToDecimal(priceMasterObj.GSTInclusive);
                                        abbplanAmount = (netProductprice * abbplanAmount) / 100;
                                        plandata.abbplanfees = abbplanAmount;
                                        basevalue = abbplanAmount * 100 / (100 + gstinclusivevalue);
                                        gstinclusivevalue = abbplanAmount - basevalue;
                                        plandata.Cgst = gstinclusivevalue / 2;
                                        plandata.Sgst = gstinclusivevalue / 2;
                                    }
                                }
                                else
                                {
                                    if (netProductprice >= priceMasterObj.Price_Start_Range && netProductprice <= priceMasterObj.Price_End_Range)
                                    {
                                        plandata.abbplanfees = (netProductprice * Convert.ToDecimal(priceMasterObj.Fees_Applicable_Percentage)) / 100;
                                        basevalue = plandata.abbplanfees;
                                        plandata.Cgst = 0;
                                        plandata.Sgst =0;
                                    }
                                }
                            }


                        }
                        if (basevalue > 0)
                        {
                            plandata.BaseValue = basevalue;
                            if (Margintype == Convert.ToInt32(ABBPlanEnum.MarginTypeFixed))
                            {
                                plandata.BPMargin =Convert.ToDecimal(priceMasterObj.BusinessPartnerMarginAmount);
                                plandata.BUMargin =Convert.ToDecimal(priceMasterObj.BusinessUnitMarginAmount);
                            }
                            else if(Margintype == Convert.ToInt32(ABBPlanEnum.MarginTypePerc))
                            {
                                plandata.BPMargin =(basevalue* Convert.ToDecimal(priceMasterObj.BusinessPartnerMarginPerc))/100;
                                plandata.BUMargin =(basevalue* Convert.ToDecimal(priceMasterObj.BusinessUnitMarginPerc))/100;
                            }
                            else
                            {
                                plandata.BUMargin = 0;
                                plandata.BPMargin = 0;
                            }
                        }
                        plandata.abbplanname = planmasterObj.ABBPlanName;
                        plandata.noclaimperiod = planmasterObj.NoClaimPeriod;
                        plandata.abbplanperiod = planmasterObj.PlanPeriodInMonth.ToString();

                    }
                }
                if (plandata != null)
                {
                    double amount = Convert.ToDouble(plandata.BaseValue);
                    string amountnew = String.Format("{0:0.00}", amount);
                    double gst = Convert.ToDouble(plandata.Cgst);
                    string gstamount = String.Format("{0:0.00}", gst);
                    plandata.BaseValue = Convert.ToDecimal(amountnew);
                    plandata.Cgst = Convert.ToDecimal(gstamount);
                    plandata.Sgst = Convert.ToDecimal(gstamount);
                    double amountabbfees = Convert.ToDouble(plandata.abbplanfees);
                    string amountnewabbfees = String.Format("{0:0.00}", amountabbfees);
                    plandata.abbplanfees = Convert.ToDecimal(amountnewabbfees);
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBOrderManager", "GetAbbPlanData", ex);
            }
            return plandata;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RegdNo"></param>
        /// <returns></returns>
        public ABBRedemptionDataContract GetRedemptionData(string RegdNo)
        {
            ABBRedemptionDataContract redemptionDataDC = new ABBRedemptionDataContract();
            _abbplantransRepository = new TransactionABBPlanMasterRepository();
            _ABBRegistrationRepository = new ABBRegistrationRepository();
            _productCategoryRepository = new ProductCategoryRepository();
            _productTypeRepository = new ProductTypeRepository();
            _brandMappingRepository = new BrandSmartSellRepository();
            _brandRepository = new BrandRepository();
            tblABBRegistration abbregistrationobj = null;
            List<tblTransMasterABBPlanMaster> transdetails = new List<tblTransMasterABBPlanMaster>();
            tblProductCategory categoryObj = null;
            tblProductType producttypeObj = null;
            tblBrandSmartBuy brandsmartbuyObj = null;
            tblBrand brandObj = null;
            DateTime currentdate = DateTime.Now;
            try
            {
                if (!string.IsNullOrEmpty(RegdNo))
                {
                    abbregistrationobj = _ABBRegistrationRepository.GetSingle(x => x.RegdNo == RegdNo && x.IsActive == true);
                    if (abbregistrationobj != null)
                    {
                        categoryObj = _productCategoryRepository.GetSingle(x => x.IsActive == true && x.Id == abbregistrationobj.NewProductCategoryId);
                        if (categoryObj != null)
                        {
                            producttypeObj = _productTypeRepository.GetSingle(x => x.IsActive == true && x.Id == abbregistrationobj.NewProductCategoryTypeId);
                            if (producttypeObj != null)
                            {
                                brandsmartbuyObj = _brandMappingRepository.GetSingle(x => x.IsActive == true && x.Id == abbregistrationobj.NewBrandId);
                                if (brandsmartbuyObj != null)
                                {
                                    redemptionDataDC.Brand = brandsmartbuyObj.Name;
                                    redemptionDataDC.BrandId = brandsmartbuyObj.Id;
                                }
                                else
                                {
                                    brandObj = _brandRepository.GetSingle(x => x.Id == abbregistrationobj.NewBrandId && x.IsActive == true);
                                    if (brandObj != null)
                                    {
                                        redemptionDataDC.Brand = brandObj.Name;
                                        redemptionDataDC.BrandId = brandObj.Id;
                                    }
                                    else
                                    {
                                        redemptionDataDC.ErrorMessage = "Brand details not found";
                                    }
                                }

                                transdetails = _abbplantransRepository.GetList(x => x.ABBRegistrationId == abbregistrationobj.ABBRegistrationId && x.IsActive == true).ToList();
                                if (transdetails.Count > 0)
                                {
                                    DateTime InvoiceDate = Convert.ToDateTime(abbregistrationobj.InvoiceDate);
                                    int Noclaimoeriod = Convert.ToInt32(abbregistrationobj.NoOfClaimPeriod);
                                    int month = CalculateMonthsDifference(InvoiceDate,currentdate);
                                    if (month > Noclaimoeriod)
                                    {
                                        foreach (var item in transdetails)
                                        {
                                            if (month >= item.From_Month && month <= item.To_Month)
                                            {
                                                decimal? redemptionPercentageValue = Convert.ToDecimal(item.Assured_BuyBack_Percentage);
                                                decimal? redemptionvalue = (abbregistrationobj.ProductNetPrice * redemptionPercentageValue) / 100;
                                                redemptionDataDC.RedemptionDate = DateTime.Now;
                                                redemptionDataDC.RedemptionPercentage = redemptionPercentageValue;
                                                redemptionDataDC.RedemptionValue = redemptionvalue;
                                                redemptionDataDC.ProductNetPrice = abbregistrationobj.ProductNetPrice;
                                                redemptionDataDC.NoClaimPeriod = abbregistrationobj.NoOfClaimPeriod;
                                                redemptionDataDC.FirstName = abbregistrationobj.CustFirstName;
                                                redemptionDataDC.LastName = abbregistrationobj.CustLastName;
                                                redemptionDataDC.Email = abbregistrationobj.CustEmail;
                                                redemptionDataDC.PhoneNumber = abbregistrationobj.CustMobile;
                                                redemptionDataDC.Pincode = abbregistrationobj.CustPinCode;
                                                redemptionDataDC.state = abbregistrationobj.CustState;
                                                redemptionDataDC.city = abbregistrationobj.CustCity;
                                                redemptionDataDC.Address1 = abbregistrationobj.CustAddress1;
                                                redemptionDataDC.Address2 = abbregistrationobj.CustAddress2;
                                                redemptionDataDC.ProductCategory = categoryObj.Description;
                                                redemptionDataDC.ProductType = producttypeObj.Description;
                                                redemptionDataDC.ProductTypeId = abbregistrationobj.NewProductCategoryTypeId;
                                                redemptionDataDC.ProductCatId = abbregistrationobj.NewProductCategoryId;
                                                redemptionDataDC.RedemptionPeriod = month;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        redemptionDataDC.ErrorMessage = "Can not redeem plan lies in no claim period";
                                    }
                                }
                                else
                                {
                                    redemptionDataDC.ErrorMessage = "Product type details not found";
                                }
                            }
                            else
                            {
                                redemptionDataDC.ErrorMessage = "Product type details not found";
                            }
                        }
                        else
                        {
                            redemptionDataDC.ErrorMessage = "Product category details not found";
                        }
                    }
                    else
                    {
                        redemptionDataDC.ErrorMessage = "data not found for this order";
                    }
                }
                else
                {
                    redemptionDataDC.ErrorMessage = "Registration number is not provided";
                }
            }
            catch (Exception ex)
            {
                redemptionDataDC.ErrorMessage = ex.Message;
                LibLogging.WriteErrorToDB("ABBOrderManager", "GetRedemptionData", ex);
            }
            return redemptionDataDC;
        }


        #region Calculate Date Difference
        public int CalculateMonthsDifference(DateTime startDate, DateTime endDate)
        {
            // Calculate the difference in years
            int yearsDiff = endDate.Year - startDate.Year;


            // Calculate the difference in months accounted for by full years
            int monthsDiff = yearsDiff * 12;



            // Calculate the remaining months within the same year
            monthsDiff += endDate.Month - startDate.Month;



            // Adjust the months difference if endDate day is before startDate day
            if (endDate.Day < startDate.Day)
            {
                monthsDiff--;
            }



            return monthsDiff;
        }
        #endregion
    }
}
