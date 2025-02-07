using GraspCorn.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCEL.DocUpload.BAL.Common;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DataContract.ProductTaxonomy;
using RDCEL.DocUpload.DataContract.SponsorModel;
using RDCEL.DocUpload.DataContract.ProductsPrices;
using RDCEL.DocUpload.DataContract.MasterModel;
using System.Data;
using GraspCorn.Common.Enums;
using RDCEL.DocUpload.DataContract.ExchangeOrderDetails;
using RDCEL.DocUpload.DataContract.ABBRegistration;

namespace RDCEL.DocUpload.BAL.SponsorsApiCall
{
    public class MasterManager
    {
        #region Variable Declaration
        ProductCategoryRepository _productCategoryRepository;
        ProductTypeRepository _productTypeRepository;
        BrandRepository _brandRepository;
        BusinessUnitRepository _buisnessUnitRepository;
        ProductQualityIndexRepository _productQualityIndexRepository;
        DateTime currentDatetime = DateTime.Now.TrimMilliseconds();
        PriceMasterRepository _productPriceRepository;
        LoginDetailsUTCRepository _loginDetailsUTCRepository;
        UnInstallationRepository _unInstallationRepository;
        ExchangeOrderRepository exchangeOrderRepository;
        ModelNumberRepository modelNumberRepository;
        ABBPriceMasterRepository _priceMasterRepository;
        BusinessPartnerRepository _businessPartnerReposirory;
        QuestionsForSweetnerRepository _questionsForSweetnerRepository;
        BUBasedSweetnerValidationRepository _bUBasedSweetnerValidationRepository;
        #endregion

        #region Get all Product Category detail
        /// <summary>
        /// Method to get the list of Product Category data contract
        /// </summary>       
        /// <returns>List ProductCategoryDataContract</returns>   
        public ProductCategoryDataContract GetProductCategory(int Buid, string pricecode)
        {
            _productCategoryRepository = new ProductCategoryRepository();
            _loginDetailsUTCRepository = new LoginDetailsUTCRepository();
            _productPriceRepository = new PriceMasterRepository();
            List<ProductCategory> productCategoryList = null;
            List<tblProductCategory> tblProductCategoryObjList = new List<tblProductCategory>();
            ProductCategoryDataContract productCategoryNew = new ProductCategoryDataContract();
            List<tblPriceMaster> pricemaster = null;
            List<tblProductCategory> prodGroupListForExchange = new List<tblProductCategory>();
            tblProductCategory categoryObj = null;
            try
            {

                //Old product category for exchange
                if (pricecode != null)
                {
                    DataTable dtProductCat = _productPriceRepository.GetProductCategoryByPriceCode(pricecode);

                    if (dtProductCat != null && dtProductCat.Rows.Count > 0)
                    {
                        pricemaster = GenericConversionHelper.DataTableToList<tblPriceMaster>(dtProductCat);
                        foreach (var productCat in pricemaster)
                        {
                            categoryObj = _productCategoryRepository.GetSingle(x => x.IsActive == true && x.IsAllowedForOld == true && x.Id == productCat.ProductCategoryId);
                            if (categoryObj != null)
                            {
                                prodGroupListForExchange.Add(categoryObj);
                            }
                        }
                    }


                }
                if (prodGroupListForExchange != null && prodGroupListForExchange.Count > 0)
                {

                    productCategoryList = GenericMapper<tblProductCategory, ProductCategory>.MapList(prodGroupListForExchange);

                    productCategoryNew.ProductsCategory = productCategoryList;

                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SamsungManager", "GetZipCodes", ex);
            }
            return productCategoryNew;
        }
        #endregion

        #region Get all Product Type detail
        /// <summary>
        /// Method to get the list of Product Type data contract
        /// </summary>       
        /// <returns>List ProductTypeDataContract</returns>   
        public ProductTypeDataContract GetProductType(string username, int catId = 0)
        {
            _productTypeRepository = new ProductTypeRepository();
            ExchangeOrderManager exchangeOrderManager = new ExchangeOrderManager();
            List<tblProductType> tblProductTypeObjList = new List<tblProductType>();
            ProductTypeDataContract ProductTypeNew = new ProductTypeDataContract();
            ProductTypeNew.ProductsType = new List<ProductType>();
            _productPriceRepository = new PriceMasterRepository();
            try
            {
                //tblProductTypeObjList = _productTypeRepository.GetAll().ToList();
                tblProductTypeObjList = _productTypeRepository.GetList(x => x.IsActive == true).ToList();
                //tblProductTypeObjList.RemoveAll(x => x.ProductCatId == Convert.ToInt32(ProductCategoryEnum.Refrigerator) && string.IsNullOrEmpty(x.Size)); //ADDED LINE TO REMOVE REF SIZE TYPES
                if (catId > 0)
                {
                    tblProductTypeObjList = tblProductTypeObjList.Where(x => x.ProductCatId == catId).ToList();
                }
                if (tblProductTypeObjList != null && tblProductTypeObjList.Count > 0)
                {

                    //productTypeList = GenericMapper<tblProductType, ProductType>.MapList(tblProductTypeObjList);

                    //ProductTypeNew.ProductsType = productTypeList;


                    string priceCode = string.Empty;
                    if (!string.IsNullOrEmpty(username))
                    {
                        priceCode = exchangeOrderManager.GetPriceCodeByUserName(username);
                    }
                    priceCode = !string.IsNullOrEmpty(priceCode) ? priceCode : "SSG-PRICE-001";

                    List<tblPriceMaster> tblProductPriceObjList = _productPriceRepository.GetList(x => x.IsActive == true
                                                                   && (x.ExchPriceCode.ToLower().Equals(priceCode.ToLower()))).ToList();


                    //start new code
                    foreach (var item in tblProductTypeObjList)
                    {
                        ProductType productType = new ProductType();
                        if (String.IsNullOrEmpty(item.Size))
                        {
                            productType.Description = item.Description.Replace(System.Environment.NewLine, string.Empty);
                            //productType.Description = item.Description.Replace(@"\r\n", "");
                        }
                        else
                        {
                            productType.Description = item.Description.Replace(System.Environment.NewLine, string.Empty) + " " + "(" + item.Size + ")";
                            //productType.Description = item.Description.Replace(@"\r\n", "") + " " + "(" + item.Size + ")";
                        }


                        productType.Id = item.Id;
                        productType.Code = item.Code;
                        productType.Name = item.Name;
                        productType.ProductCatId = (int)item.ProductCatId;

                        if (tblProductPriceObjList != null && tblProductPriceObjList.Count > 0 && tblProductPriceObjList.Exists(x => x.ProductTypeId == productType.Id))
                        {
                            ProductTypeNew.ProductsType.Add(productType);
                        }


                    }
                    // end new code

                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SamsungManager", "GetZipCodes", ex);
            }
            return ProductTypeNew;
        }
        #endregion


        #region Get all Brand detail
        /// <summary>
        /// Method to get the list of Brand data contract
        /// </summary>       
        /// <returns>List BrandDataContract</returns>   
        public BrandDataContract GetBrand()
        {
            _brandRepository = new BrandRepository();
            List<BrandName> brandList = null;
            List<tblBrand> tblbrandObjList = new List<tblBrand>();
            BrandDataContract brandNew = new BrandDataContract();
            try
            {
                //tblbrandObjList = _brandRepository.GetAll().ToList();
                tblbrandObjList = _brandRepository.GetList(x => x.IsActive == true).ToList();
                //DataTable dt = _brandRepository.GetBrabdList();
                //if (dt != null && dt.Rows.Count > 0)
                //{
                //    tblbrandObjList = GenericConversionHelper.DataTableToList<tblBrand>(dt);
                //}
                //if (tblbrandObjList != null && tblbrandObjList.Count > 0)
                //{
                //    brandList = GenericMapper<tblBrand, BrandName>.MapList(tblbrandObjList);
                //    //if (zipCodesList != null)
                //    brandNew.Brand = brandList;

                //}

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SamsungManager", "GetZipCodes", ex);
            }
            return brandNew;
        }

        /// <summary>
        /// Method to get the Brand for Exchange
        /// </summary>
        /// <returns>BrandDataContract</returns>
        public BrandDataContract GetBrandForExchange()
        {
            _brandRepository = new BrandRepository();
            List<BrandName> brandList = null;
            List<tblBrand> tblbrandObjList = new List<tblBrand>();
            BrandDataContract brandNew = new BrandDataContract();
            try
            {
                DataTable dt = _brandRepository.GetBrabdListForExchange();
                if (dt != null && dt.Rows.Count > 0)
                {
                    tblbrandObjList = GenericConversionHelper.DataTableToList<tblBrand>(dt);
                }
                if (tblbrandObjList != null && tblbrandObjList.Count > 0)
                {
                    brandList = GenericMapper<tblBrand, BrandName>.MapList(tblbrandObjList);
                    //if (zipCodesList != null)
                    brandNew.Brand = brandList;
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("MasterManager", "GetBrandForExchange", ex);
            }
            return brandNew;
        }


        /// <summary>
        ///Method to get the list of Brands for exchange by product cat id
        /// </summary>
        /// <returns>BrandDataContract</returns>
        public BrandDataContract GetBrandForExchangeByCategoryId(int catId, int buid, int typeId)
        {
            _brandRepository = new BrandRepository();
            _loginDetailsUTCRepository = new LoginDetailsUTCRepository();
            List<BrandName> brandList = null;
            List<tblBrand> tblbrandObjList = new List<tblBrand>();
            BrandDataContract brandNew = new BrandDataContract();
            try
            {
                Login login = _loginDetailsUTCRepository.GetSingle(x => x.SponsorId != null && x.SponsorId == buid);
                if (typeId > 0)
                {
                    DataTable dt = _brandRepository.GetBrabdListForExchangeByCatId(catId, login.PriceCode, typeId);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        tblbrandObjList = GenericConversionHelper.DataTableToList<tblBrand>(dt);
                    }
                    if (tblbrandObjList != null && tblbrandObjList.Count > 0)
                    {
                        brandList = GenericMapper<tblBrand, BrandName>.MapList(tblbrandObjList);
                        brandNew.Brand = brandList;
                    }
                }
                else
                {
                    DataTable dt = _brandRepository.GetBrabdListForExchangeByCategoryId(catId, login.PriceCode);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        tblbrandObjList = GenericConversionHelper.DataTableToList<tblBrand>(dt);
                    }
                    if (tblbrandObjList != null && tblbrandObjList.Count > 0)
                    {
                        brandList = GenericMapper<tblBrand, BrandName>.MapList(tblbrandObjList);
                        //if (zipCodesList != null)
                        brandNew.Brand = brandList;
                    }
                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("MasterManager", "GetBrandForExchange", ex);
            }
            return brandNew;
        }

        #endregion

        #region Get the Quality Index Data

        /// <summary>
        /// Method to get the ProductQualityIndex
        /// </summary>
        /// <param name="catId"></param>
        /// <returns>ProductQualityIndexDataContract</returns>
        public ProductQualityIndexDataContract GetProductQualityIndexByCategory(int catId)
        {
            _productQualityIndexRepository = new ProductQualityIndexRepository();
            ProductQualityIndexDataContract productQualityIndexDC = null;
            try
            {
                tblProductQualityIndex productQualityIndex = _productQualityIndexRepository.GetSingle(x => x.IsActive == true && x.ProductCategoryId == catId);
                productQualityIndexDC = GenericMapper<tblProductQualityIndex, ProductQualityIndexDataContract>.MapObject(productQualityIndex);
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("MasterManager", "GetProductQualityIndexByCategory", ex);
            }
            return productQualityIndexDC;
        }

        #endregion

        #region Get Product Price by Detail 

        /// <summary>
        /// Get product price customer selection
        /// </summary>
        /// <param name="catid">catid</param>
        /// <param name="subcateid">subcateid</param>
        /// <param name="brandId">brandId</param>
        /// <param name="qualityId">qualityId</param>
        /// <returns>string</returns>
        public string GetProductPrice(int catid, int subcateid, int brandId, int qualityId, int businessUnitId, string formatType = "")
        {
            _productPriceRepository = new PriceMasterRepository();
            _brandRepository = new BrandRepository();
            _productTypeRepository = new ProductTypeRepository();
            _buisnessUnitRepository = new BusinessUnitRepository();
            _productCategoryRepository = new ProductCategoryRepository();
            _loginDetailsUTCRepository = new LoginDetailsUTCRepository();
            List<tblPriceMaster> tblProductPriceObjList = new List<tblPriceMaster>();
            List<ProductsPricesDataContract> productsPricesList = new List<ProductsPricesDataContract>();
            List<ProductsFromTypePriceList> productsTyepeList = new List<ProductsFromTypePriceList>();
            List<BrandPriceList> brandList = null;
            string productPrice = string.Empty;
            string priceCode = string.Empty;
            double SweetnerAmmount = 0;
            double price = 0;

            try
            {
                Login loginObj = _loginDetailsUTCRepository.GetSingle(x => x.SponsorId == businessUnitId);
                if (loginObj != null)
                {
                    priceCode = loginObj.PriceCode;
                }
                //Get the brand list 
                List<tblBrand> masterbrandList = _brandRepository.GetList(x => x.IsActive == true).ToList();
                List<tblProductType> masterCategoryTypeList = _productTypeRepository.GetList(x => x.IsActive == true).ToList();

                //Get Sweetner Amount For Perticular Brad
                if (!string.IsNullOrEmpty(formatType))
                {
                    tblBusinessUnit buisnessobj = _buisnessUnitRepository.GetSingle(x => x.BusinessUnitId == businessUnitId);
                    if (buisnessobj != null && buisnessobj.SweetnerForDTC != null || buisnessobj.SweetnerForDTD != null)
                    {
                        if (formatType.Equals("Home"))
                        {
                            SweetnerAmmount = (double)buisnessobj.SweetnerForDTC;
                        }
                        else
                        {
                            SweetnerAmmount = (double)buisnessobj.SweetnerForDTD;
                        }
                    }
                }

                //Get the Product price list
                if (catid > 0)
                {
                    #region For Category Name

                    tblProductPriceObjList = _productPriceRepository.GetList(x => x.IsActive == true
                    && (x.ProductCategoryId != null && x.ProductCategoryId == catid)
                    && (x.ExchPriceCode != null && x.ExchPriceCode.ToLower().Equals(priceCode.ToLower()))
                    && (x.ProductTypeId != null && x.ProductTypeId == subcateid)).ToList();
                    //Add the condition for Exch Price Code
                    //&& (x.ExchPriceCode != null && x.ExchPriceCode.ToLower().Contains("rel"))

                    if (tblProductPriceObjList != null && tblProductPriceObjList.Count > 0)
                    {

                        foreach (var item in tblProductPriceObjList)
                        {
                            //ProductsPricesDataContract ProductsPrices = new ProductsPricesDataContract();


                            ProductsFromTypePriceList productsType = new ProductsFromTypePriceList();

                            productsType.ProducttypeId = (int)item.ProductTypeId;
                            productsType.id = item.Id;

                            #region code to fill brand list

                            BrandPriceList brandPL = null;
                            brandList = new List<BrandPriceList>();
                            //For Brand 1
                            string BrandDescription = ExchangeOrderManager.GetEnumDescription(PriceMasterBrandEnum.Others);
                            if (!string.IsNullOrEmpty(item.BrandName_1))
                            {
                                if (item.BrandName_1 == BrandDescription)
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_1.ToLower()) && x.IsActive == true).Id;
                                    brandPL.Name = item.BrandName_1;
                                    brandPL.Price = item.Quote_P;
                                    brandPL.Mid_Price = item.Quote_Q;
                                    brandPL.Min_Price = item.Quote_R;
                                    brandPL.Scrap_Price = item.Quote_S;
                                    brandList.Add(brandPL);
                                }
                                else
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_1.ToLower()) && x.IsActive == true).Id;
                                    brandPL.Name = item.BrandName_1;
                                    brandPL.Price = item.Quote_P_High;
                                    brandPL.Mid_Price = item.Quote_Q_High;
                                    brandPL.Min_Price = item.Quote_R_High;
                                    brandPL.Scrap_Price = item.Quote_S_High;
                                    brandList.Add(brandPL);
                                }
                            }

                            //if (!string.IsNullOrEmpty(item.BrandName_1))
                            //{
                            //    brandPL = new BrandPriceList();
                            //    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_1.ToLower())).Id;
                            //    brandPL.Name = item.BrandName_1;
                            //    brandPL.Price = item.Quote_P_High;
                            //    brandPL.Mid_Price = item.Quote_Q_High;
                            //    brandPL.Min_Price = item.Quote_R_High;
                            //    brandPL.Scrap_Price = item.Quote_S_High;
                            //    brandList.Add(brandPL);
                            //}

                            //For Brand 2
                            if (!string.IsNullOrEmpty(item.BrandName_2))
                            {
                                if (item.BrandName_2 == BrandDescription)
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_2.ToLower()) && x.IsActive == true).Id;
                                    brandPL.Name = item.BrandName_2;
                                    brandPL.Price = item.Quote_P;
                                    brandPL.Mid_Price = item.Quote_Q;
                                    brandPL.Min_Price = item.Quote_R;
                                    brandPL.Scrap_Price = item.Quote_S;
                                    brandList.Add(brandPL);
                                }
                                else
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_2.ToLower()) && x.IsActive == true).Id;
                                    brandPL.Name = item.BrandName_2;
                                    brandPL.Price = item.Quote_P_High;
                                    brandPL.Mid_Price = item.Quote_Q_High;
                                    brandPL.Min_Price = item.Quote_R_High;
                                    brandPL.Scrap_Price = item.Quote_S_High;
                                    brandList.Add(brandPL);

                                }

                            }

                            //For Brand 3
                            if (!string.IsNullOrEmpty(item.BrandName_3))
                            {
                                if (item.BrandName_3 == BrandDescription)
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_3.ToLower()) && x.IsActive == true).Id;
                                    brandPL.Name = item.BrandName_3;
                                    brandPL.Price = item.Quote_P;
                                    brandPL.Mid_Price = item.Quote_Q;
                                    brandPL.Min_Price = item.Quote_R;
                                    brandPL.Scrap_Price = item.Quote_S;
                                    brandList.Add(brandPL);
                                }
                                else
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_3.ToLower()) && x.IsActive == true).Id;
                                    brandPL.Name = item.BrandName_3;
                                    brandPL.Price = item.Quote_P_High;
                                    brandPL.Mid_Price = item.Quote_Q_High;
                                    brandPL.Min_Price = item.Quote_R_High;
                                    brandPL.Scrap_Price = item.Quote_S_High;
                                    brandList.Add(brandPL);

                                }
                            }

                            //For Brand 4
                            if (!string.IsNullOrEmpty(item.BrandName_4))
                            {
                                if (item.BrandName_4 == BrandDescription)
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_4.ToLower()) && x.IsActive == true).Id;
                                    brandPL.Name = item.BrandName_4;
                                    brandPL.Price = item.Quote_P;
                                    brandPL.Mid_Price = item.Quote_Q;
                                    brandPL.Min_Price = item.Quote_R;
                                    brandPL.Scrap_Price = item.Quote_S;
                                    brandList.Add(brandPL);
                                }
                                else
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_4.ToLower()) && x.IsActive == true).Id;
                                    brandPL.Name = item.BrandName_4;
                                    brandPL.Price = item.Quote_P_High;
                                    brandPL.Mid_Price = item.Quote_Q_High;
                                    brandPL.Min_Price = item.Quote_R_High;
                                    brandPL.Scrap_Price = item.Quote_S_High;
                                    brandList.Add(brandPL);

                                }
                            }

                            //For Other
                            brandPL = new BrandPriceList();
                            brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals("Others".ToLower()) && x.IsActive == true).Id;
                            brandPL.Name = "Others";
                            brandPL.Price = item.Quote_P;
                            brandPL.Mid_Price = item.Quote_Q;
                            brandPL.Min_Price = item.Quote_R;
                            brandPL.Scrap_Price = item.Quote_S;
                            brandList.Add(brandPL);
                            //productsPriceList.Add(productsPrice);
                            #endregion

                            productsType.Brand = brandList;
                            productsTyepeList.Add(productsType);


                        }
                        if (productsTyepeList != null && productsTyepeList.Count > 0)
                        {

                            ProductsFromTypePriceList productsType = productsTyepeList.FirstOrDefault(x => x.ProducttypeId == subcateid);
                            BrandPriceList brandPriceList = productsType.Brand.FirstOrDefault(x => x.BrandId == brandId);
                            switch (qualityId)
                            {
                                case 1:
                                    productPrice = brandPriceList.Price;
                                    break;
                                case 2:
                                    productPrice = brandPriceList.Mid_Price;
                                    break;
                                case 3:
                                    productPrice = brandPriceList.Min_Price;
                                    break;
                                case 4:
                                    productPrice = brandPriceList.Scrap_Price;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("MasterManager", "GetProductPrice", ex);
            }
            price = Convert.ToDouble(productPrice);
            productPrice = (price + SweetnerAmmount).ToString();
            return productPrice;
        }
        #endregion

        #region Get UnInstallationPrice
        public string GetUnInstallationAmount(int catid, int subcateid, string IsunInstallation, string type)
        {
            _productTypeRepository = new ProductTypeRepository();
            _productCategoryRepository = new ProductCategoryRepository();
            _unInstallationRepository = new UnInstallationRepository();
            MyGateUnInstallation priceObj = new MyGateUnInstallation();
            string unInstallationPrice = string.Empty;
            string TypeOfTv = string.Empty;
            List<tblUnInstallationPriceMaster> unInstallatioObj = new List<tblUnInstallationPriceMaster>();
            try
            {
                if (IsunInstallation != null && IsunInstallation == "Yes")
                {
                    tblProductCategory CategoryObj = _productCategoryRepository.GetSingle(x => x.Id == catid);

                    if (CategoryObj != null)
                    {
                        tblProductType ProductTypeObj = _productTypeRepository.GetSingle(x => x.Id == subcateid && x.ProductCatId == catid);
                        if (ProductTypeObj != null)
                        {

                            if (CategoryObj.Description == "Television")
                            {
                                if (type == "1")
                                {
                                    TypeOfTv = "Table Mount";
                                }
                                else
                                {
                                    TypeOfTv = "Wall Mount";
                                }
                                tblUnInstallationPriceMaster uninstallPrice = _unInstallationRepository.GetSingle(x => x.ProductType == ProductTypeObj.Description && x.Product == CategoryObj.Description && x.Type == TypeOfTv);

                                if (uninstallPrice != null)
                                {
                                    unInstallationPrice = uninstallPrice.UninstallationPrice.ToString();
                                }

                            }
                            else
                            {

                                tblUnInstallationPriceMaster uninstallPrice = _unInstallationRepository.GetSingle(x => x.ProductType == ProductTypeObj.Description && x.Product == CategoryObj.Description);
                                if (uninstallPrice != null)
                                {
                                    unInstallationPrice = uninstallPrice.UninstallationPrice.ToString();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("MasterManager", "GetUnInstallationAmount", ex);
            }

            return unInstallationPrice;
        }
        #endregion

        /// <summary>
        /// Product type by Category Id
        /// </summary>
        /// <param name="productCatId">productCatId</param>
        /// <returns>ProductTypeDataContract</returns>
        public ProductTypeDataContract GetProductTypeByProductCatId(int productCatId)
        {
            _productTypeRepository = new ProductTypeRepository();
            List<tblProductType> tblProductTypeObjList = new List<tblProductType>();
            ProductTypeDataContract ProductTypeNew = new ProductTypeDataContract();
            ProductTypeNew.ProductsType = new List<ProductType>();
            try
            {
                //tblProductTypeObjList = _productTypeRepository.GetAll().ToList();
                tblProductTypeObjList = _productTypeRepository.GetList(x => x.IsActive == true && x.ProductCatId == productCatId).ToList();
                tblProductTypeObjList.RemoveAll(x => x.ProductCatId == Convert.ToInt32(ProductCategoryEnum.Refrigerator) && string.IsNullOrEmpty(x.Size)); //ADDED LINE TO REMOVE REF SIZE TYPES
                if (tblProductTypeObjList != null && tblProductTypeObjList.Count > 0)
                {
                    //start new code
                    foreach (var item in tblProductTypeObjList)
                    {
                        ProductType productType = new ProductType();
                        if (String.IsNullOrEmpty(item.Size))
                        {
                            productType.Description = item.Description.Replace(System.Environment.NewLine, string.Empty);
                            //productType.Description = item.Description.Replace(@"\r\n", "");
                        }
                        else
                        {
                            productType.Description = item.Description.Replace(System.Environment.NewLine, string.Empty) + " " + "(" + item.Size + ")";
                            //productType.Description = item.Description.Replace(@"\r\n", "") + " " + "(" + item.Size + ")";
                        }

                        productType.Id = item.Id;
                        productType.Code = item.Code;
                        productType.Name = item.Name;
                        productType.ProductCatId = (int)item.ProductCatId;
                        ProductTypeNew.ProductsType.Add(productType);
                    }
                    // end new code
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("MasterManager", "GetProductTypeByProductCatId", ex);
            }
            return ProductTypeNew;
        }
        #region GetProductPriceWithModelBasedSweetner
        public string GetProductPriceWithModelBasedSweetner(int newcatid, int newsubcatid, int catid, int subcateid, int brandId, int qualityId, int businessUnitId, int modelnoId, string formatType = "")
        {
            _productPriceRepository = new PriceMasterRepository();
            _brandRepository = new BrandRepository();
            _productTypeRepository = new ProductTypeRepository();
            _buisnessUnitRepository = new BusinessUnitRepository();
            _productCategoryRepository = new ProductCategoryRepository();
            _loginDetailsUTCRepository = new LoginDetailsUTCRepository();
            modelNumberRepository = new ModelNumberRepository();
            List<tblPriceMaster> tblProductPriceObjList = new List<tblPriceMaster>();
            List<ProductsPricesDataContract> productsPricesList = new List<ProductsPricesDataContract>();
            List<ProductsFromTypePriceList> productsTyepeList = new List<ProductsFromTypePriceList>();
            List<BrandPriceList> brandList = null;
            string productPrice = string.Empty;
            string priceCode = string.Empty;
            double SweetnerAmmount = 0;
            double price = 0;
            try
            {
                Login loginObj = _loginDetailsUTCRepository.GetSingle(x => x.SponsorId == businessUnitId);
                if (loginObj != null)
                {
                    priceCode = loginObj.PriceCode;
                }
                //Get the brand list 
                List<tblBrand> masterbrandList = _brandRepository.GetList(x => x.IsActive == true).ToList();
                List<tblProductType> masterCategoryTypeList = _productTypeRepository.GetList(x => x.IsActive == true).ToList();

                //Get Sweetner Amount For Perticular Brad
                if (!string.IsNullOrEmpty(formatType))
                {
                    tblBusinessUnit buisnessobj = _buisnessUnitRepository.GetSingle(x => x.BusinessUnitId == businessUnitId);
                    if (buisnessobj != null && buisnessobj.IsSweetnerModelBased == true)
                    {
                        if (modelnoId != 0)
                        {
                            tblModelNumber modelObj = modelNumberRepository.GetSingle(x => x.ProductCategoryId == newcatid && x.ProductTypeId == newsubcatid && x.IsDefaultProduct == false && x.ModelNumberId.Equals(modelnoId) && x.BusinessUnitId == businessUnitId);
                            if (modelObj != null && modelObj.SweetnerForDTC != null || modelObj.SweetnerForDTD != null && modelObj.IsDefaultProduct == false)
                            {
                                string Format = ExchangeOrderManager.GetEnumDescription((FormatTypeEnum.Dealer));
                                if (formatType == Format)
                                {
                                    SweetnerAmmount = (double)modelObj.SweetnerForDTD;
                                }
                                else
                                {
                                    SweetnerAmmount = (double)modelObj.SweetnerForDTC;
                                }
                            }
                            else
                            {
                                SweetnerAmmount = 0;
                            }
                        }
                        else
                        {
                            tblModelNumber modelObj = modelNumberRepository.GetSingle(x => x.ProductCategoryId == newcatid && x.ProductTypeId == newsubcatid && x.IsDefaultProduct == true && x.BusinessUnitId == businessUnitId);
                            if (modelObj != null && (modelObj.SweetnerForDTC != null || modelObj.SweetnerForDTD != null) && modelObj.IsDefaultProduct == true)
                            {
                                string Format = ExchangeOrderManager.GetEnumDescription((FormatTypeEnum.Dealer));
                                if (formatType == Format)
                                {
                                    SweetnerAmmount = (double)modelObj.SweetnerForDTD;
                                }
                                else
                                {
                                    SweetnerAmmount = (double)modelObj.SweetnerForDTC;
                                }
                            }
                            else
                            {
                                SweetnerAmmount = 0;
                            }
                        }
                    }
                }

                //Get the Product price list
                if (catid > 0)
                {
                    #region For Category Name

                    tblProductPriceObjList = _productPriceRepository.GetList(x => x.IsActive == true
                    && (x.ProductCategoryId != null && x.ProductCategoryId == catid)
                    && (x.ExchPriceCode != null && x.ExchPriceCode.ToLower().Equals(priceCode.ToLower()))
                    && (x.ProductTypeId != null && x.ProductTypeId == subcateid)).ToList();
                    //Add the condition for Exch Price Code
                    //&& (x.ExchPriceCode != null && x.ExchPriceCode.ToLower().Contains("rel"))

                    if (tblProductPriceObjList != null && tblProductPriceObjList.Count > 0)
                    {

                        foreach (var item in tblProductPriceObjList)
                        {
                            //ProductsPricesDataContract ProductsPrices = new ProductsPricesDataContract();


                            ProductsFromTypePriceList productsType = new ProductsFromTypePriceList();

                            productsType.ProducttypeId = (int)item.ProductTypeId;
                            productsType.id = item.Id;

                            #region code to fill brand list

                            BrandPriceList brandPL = null;
                            brandList = new List<BrandPriceList>();
                            string BrandDescription = ExchangeOrderManager.GetEnumDescription(PriceMasterBrandEnum.Others);
                            //For Brand 1
                            if (!string.IsNullOrEmpty(item.BrandName_1))
                            {
                                if (item.BrandName_1 == BrandDescription)
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_1.ToLower()) && x.IsActive == true).Id;
                                    brandPL.Name = item.BrandName_1;
                                    brandPL.Price = item.Quote_P;
                                    brandPL.Mid_Price = item.Quote_Q;
                                    brandPL.Min_Price = item.Quote_R;
                                    brandPL.Scrap_Price = item.Quote_S;
                                    brandList.Add(brandPL);
                                }
                                else
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_1.ToLower()) && x.IsActive == true).Id;
                                    brandPL.Name = item.BrandName_1;
                                    brandPL.Price = item.Quote_P_High;
                                    brandPL.Mid_Price = item.Quote_Q_High;
                                    brandPL.Min_Price = item.Quote_R_High;
                                    brandPL.Scrap_Price = item.Quote_S_High;
                                    brandList.Add(brandPL);
                                }

                            }

                            //For Brand 2
                            if (!string.IsNullOrEmpty(item.BrandName_2))
                            {
                                if (item.BrandName_2 == BrandDescription)
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_2.ToLower()) && x.IsActive == true).Id;
                                    brandPL.Name = item.BrandName_2;
                                    brandPL.Price = item.Quote_P;
                                    brandPL.Mid_Price = item.Quote_Q;
                                    brandPL.Min_Price = item.Quote_R;
                                    brandPL.Scrap_Price = item.Quote_S;
                                    brandList.Add(brandPL);
                                }
                                else
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_2.ToLower()) && x.IsActive == true).Id;
                                    brandPL.Name = item.BrandName_2;
                                    brandPL.Price = item.Quote_P_High;
                                    brandPL.Mid_Price = item.Quote_Q_High;
                                    brandPL.Min_Price = item.Quote_R_High;
                                    brandPL.Scrap_Price = item.Quote_S_High;
                                    brandList.Add(brandPL);
                                }
                            }

                            //For Brand 3
                            if (!string.IsNullOrEmpty(item.BrandName_3))
                            {
                                if (item.BrandName_3 == BrandDescription)
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_3.ToLower()) && x.IsActive == true).Id;
                                    brandPL.Name = item.BrandName_3;
                                    brandPL.Price = item.Quote_P;
                                    brandPL.Mid_Price = item.Quote_Q;
                                    brandPL.Min_Price = item.Quote_R;
                                    brandPL.Scrap_Price = item.Quote_S;
                                    brandList.Add(brandPL);
                                }
                                else
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_3.ToLower()) && x.IsActive == true).Id;
                                    brandPL.Name = item.BrandName_3;
                                    brandPL.Price = item.Quote_P_High;
                                    brandPL.Mid_Price = item.Quote_Q_High;
                                    brandPL.Min_Price = item.Quote_R_High;
                                    brandPL.Scrap_Price = item.Quote_S_High;
                                    brandList.Add(brandPL);
                                }
                            }

                            //For Brand 4
                            if (!string.IsNullOrEmpty(item.BrandName_4))
                            {
                                if (item.BrandName_4 == BrandDescription)
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_4.ToLower()) && x.IsActive == true).Id;
                                    brandPL.Name = item.BrandName_4;
                                    brandPL.Price = item.Quote_P;
                                    brandPL.Mid_Price = item.Quote_Q;
                                    brandPL.Min_Price = item.Quote_R;
                                    brandPL.Scrap_Price = item.Quote_S;
                                    brandList.Add(brandPL);
                                }
                                else
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_4.ToLower()) && x.IsActive == true).Id;
                                    brandPL.Name = item.BrandName_4;
                                    brandPL.Price = item.Quote_P_High;
                                    brandPL.Mid_Price = item.Quote_Q_High;
                                    brandPL.Min_Price = item.Quote_R_High;
                                    brandPL.Scrap_Price = item.Quote_S_High;
                                    brandList.Add(brandPL);
                                }
                            }

                            //For Other
                            brandPL = new BrandPriceList();
                            brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals("Others".ToLower()) && x.IsActive == true).Id;
                            brandPL.Name = "Others";
                            brandPL.Price = item.Quote_P;
                            brandPL.Mid_Price = item.Quote_Q;
                            brandPL.Min_Price = item.Quote_R;
                            brandPL.Scrap_Price = item.Quote_S;
                            brandList.Add(brandPL);
                            //productsPriceList.Add(productsPrice);
                            #endregion

                            productsType.Brand = brandList;
                            productsTyepeList.Add(productsType);
                        }
                        if (productsTyepeList != null && productsTyepeList.Count > 0)
                        {
                            ProductsFromTypePriceList productsType = productsTyepeList.FirstOrDefault(x => x.ProducttypeId == subcateid);
                            BrandPriceList brandPriceList = productsType.Brand.FirstOrDefault(x => x.BrandId == brandId);
                            if (brandPriceList != null)
                            {
                                switch (qualityId)
                                {
                                    case 1:
                                        productPrice = brandPriceList.Price;
                                        break;
                                    case 2:
                                        productPrice = brandPriceList.Mid_Price;
                                        break;
                                    case 3:
                                        productPrice = brandPriceList.Min_Price;
                                        break;
                                    case 4:
                                        productPrice = brandPriceList.Scrap_Price;
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("MasterManager", "GetProductPriceWithModelBasedSweetner", ex);
            }
            price = Convert.ToDouble(productPrice);
            productPrice = (price + SweetnerAmmount).ToString();
            return productPrice;
        }
        #endregion

        #region GetProductPriceforAbsoluteSweetner
        public string GetProductPriceforAbsoluteSweetner(int newcatid, int newsubcatid, int catid, int subcateid, int brandId, int qualityId, int businessUnitId, int modelnoId, int ExchangeOrderId, string formatType = "")
        {
            _productPriceRepository = new PriceMasterRepository();
            _brandRepository = new BrandRepository();
            _productTypeRepository = new ProductTypeRepository();
            _buisnessUnitRepository = new BusinessUnitRepository();
            _productCategoryRepository = new ProductCategoryRepository();
            _loginDetailsUTCRepository = new LoginDetailsUTCRepository();
            exchangeOrderRepository = new ExchangeOrderRepository();
            modelNumberRepository = new ModelNumberRepository();
            List<tblPriceMaster> tblProductPriceObjList = new List<tblPriceMaster>();
            List<ProductsPricesDataContract> productsPricesList = new List<ProductsPricesDataContract>();
            List<ProductsFromTypePriceList> productsTyepeList = new List<ProductsFromTypePriceList>();
            List<BrandPriceList> brandList = null;
            string productPrice = string.Empty;
            string priceCode = string.Empty;
            double SweetnerAmmount = 0;
            double price = 0;
            try
            {
                Login loginObj = _loginDetailsUTCRepository.GetSingle(x => x.SponsorId == businessUnitId);
                if (loginObj != null)
                {
                    priceCode = loginObj.PriceCode;
                }
                //Get the brand list 
                List<tblBrand> masterbrandList = _brandRepository.GetList(x => x.IsActive == true).ToList();
                List<tblProductType> masterCategoryTypeList = _productTypeRepository.GetList(x => x.IsActive == true).ToList();

                //Get Sweetner Amount For Perticular Brad
                if (ExchangeOrderId > 0)
                {
                    tblExchangeOrder ExchangeObj = exchangeOrderRepository.GetSingle(x => x.Id == ExchangeOrderId && x.IsActive == true);
                    if (ExchangeObj != null)
                    {
                        tblBusinessUnit bussinessUnitObj = _buisnessUnitRepository.GetSingle(x => x.BusinessUnitId == businessUnitId && x.IsActive == true);
                        if (bussinessUnitObj != null && bussinessUnitObj.IsSweetnerModelBased == true)
                        {
                            SweetnerAmmount = Convert.ToDouble(ExchangeObj.Sweetener);

                            //Commented code if in case Model Based sweetner gets active
                            tblModelNumber modelObj = modelNumberRepository.GetSingle(x => x.ModelNumberId == modelnoId && x.IsDefaultProduct == false);
                            if (modelObj != null && modelObj.SweetnerForDTC != null || modelObj.SweetnerForDTD != null)
                            {
                                if (ExchangeObj.IsDtoC == true)
                                {
                                    SweetnerAmmount = (double)modelObj.SweetnerForDTC;
                                }
                                else
                                {
                                    SweetnerAmmount = (double)modelObj.SweetnerForDTD;
                                }
                            }
                        }
                    }
                }

                //Get the Product price list
                if (catid > 0)
                {
                    #region For Category Name

                    tblProductPriceObjList = _productPriceRepository.GetList(x => x.IsActive == true
                    && (x.ProductCategoryId != null && x.ProductCategoryId == catid)
                    && (x.ExchPriceCode != null && x.ExchPriceCode.ToLower().Contains(priceCode.ToLower()))
                    && (x.ProductTypeId != null && x.ProductTypeId == subcateid)).ToList();
                    //Add the condition for Exch Price Code
                    //&& (x.ExchPriceCode != null && x.ExchPriceCode.ToLower().Contains("rel"))

                    if (tblProductPriceObjList != null && tblProductPriceObjList.Count > 0)
                    {

                        foreach (var item in tblProductPriceObjList)
                        {
                            //ProductsPricesDataContract ProductsPrices = new ProductsPricesDataContract();


                            ProductsFromTypePriceList productsType = new ProductsFromTypePriceList();

                            productsType.ProducttypeId = (int)item.ProductTypeId;
                            productsType.id = item.Id;

                            #region code to fill brand list

                            BrandPriceList brandPL = null;
                            brandList = new List<BrandPriceList>();
                            string BrandDescription = ExchangeOrderManager.GetEnumDescription(PriceMasterBrandEnum.Others);
                            //For Brand 1
                            if (!string.IsNullOrEmpty(item.BrandName_1))
                            {
                                if (item.BrandName_1 == BrandDescription)
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_1.ToLower()) && x.IsActive == true).Id;
                                    brandPL.Name = item.BrandName_1;
                                    brandPL.Price = item.Quote_P;
                                    brandPL.Mid_Price = item.Quote_Q;
                                    brandPL.Min_Price = item.Quote_R;
                                    brandPL.Scrap_Price = item.Quote_S;
                                    brandList.Add(brandPL);
                                }
                                else
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_1.ToLower()) && x.IsActive == true).Id;
                                    brandPL.Name = item.BrandName_1;
                                    brandPL.Price = item.Quote_P_High;
                                    brandPL.Mid_Price = item.Quote_Q_High;
                                    brandPL.Min_Price = item.Quote_R_High;
                                    brandPL.Scrap_Price = item.Quote_S_High;
                                    brandList.Add(brandPL);
                                }

                            }

                            //For Brand 2
                            if (!string.IsNullOrEmpty(item.BrandName_2))
                            {
                                if (item.BrandName_2 == BrandDescription)
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_2.ToLower()) && x.IsActive == true).Id;
                                    brandPL.Name = item.BrandName_2;
                                    brandPL.Price = item.Quote_P;
                                    brandPL.Mid_Price = item.Quote_Q;
                                    brandPL.Min_Price = item.Quote_R;
                                    brandPL.Scrap_Price = item.Quote_S;
                                    brandList.Add(brandPL);
                                }
                                else
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_2.ToLower()) && x.IsActive == true).Id;
                                    brandPL.Name = item.BrandName_2;
                                    brandPL.Price = item.Quote_P_High;
                                    brandPL.Mid_Price = item.Quote_Q_High;
                                    brandPL.Min_Price = item.Quote_R_High;
                                    brandPL.Scrap_Price = item.Quote_S_High;
                                    brandList.Add(brandPL);
                                }
                            }

                            //For Brand 3
                            if (!string.IsNullOrEmpty(item.BrandName_3))
                            {
                                if (item.BrandName_3 == BrandDescription)
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_3.ToLower()) && x.IsActive == true).Id;
                                    brandPL.Name = item.BrandName_3;
                                    brandPL.Price = item.Quote_P;
                                    brandPL.Mid_Price = item.Quote_Q;
                                    brandPL.Min_Price = item.Quote_R;
                                    brandPL.Scrap_Price = item.Quote_S;
                                    brandList.Add(brandPL);
                                }
                                else
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_3.ToLower()) && x.IsActive == true).Id;
                                    brandPL.Name = item.BrandName_3;
                                    brandPL.Price = item.Quote_P_High;
                                    brandPL.Mid_Price = item.Quote_Q_High;
                                    brandPL.Min_Price = item.Quote_R_High;
                                    brandPL.Scrap_Price = item.Quote_S_High;
                                    brandList.Add(brandPL);
                                }
                            }

                            //For Brand 4
                            if (!string.IsNullOrEmpty(item.BrandName_4))
                            {
                                if (item.BrandName_4 == BrandDescription)
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_4.ToLower()) && x.IsActive == true).Id;
                                    brandPL.Name = item.BrandName_4;
                                    brandPL.Price = item.Quote_P;
                                    brandPL.Mid_Price = item.Quote_Q;
                                    brandPL.Min_Price = item.Quote_R;
                                    brandPL.Scrap_Price = item.Quote_S;
                                    brandList.Add(brandPL);
                                }
                                else
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_4.ToLower()) && x.IsActive == true).Id;
                                    brandPL.Name = item.BrandName_4;
                                    brandPL.Price = item.Quote_P_High;
                                    brandPL.Mid_Price = item.Quote_Q_High;
                                    brandPL.Min_Price = item.Quote_R_High;
                                    brandPL.Scrap_Price = item.Quote_S_High;
                                    brandList.Add(brandPL);
                                }
                            }

                            //For Other
                            brandPL = new BrandPriceList();
                            brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals("Others".ToLower()) && x.IsActive == true).Id;
                            brandPL.Name = "Others";
                            brandPL.Price = item.Quote_P;
                            brandPL.Mid_Price = item.Quote_Q;
                            brandPL.Min_Price = item.Quote_R;
                            brandPL.Scrap_Price = item.Quote_S;
                            brandList.Add(brandPL);
                            //productsPriceList.Add(productsPrice);
                            #endregion

                            productsType.Brand = brandList;
                            productsTyepeList.Add(productsType);
                        }
                        if (productsTyepeList != null && productsTyepeList.Count > 0)
                        {

                            ProductsFromTypePriceList productsType = productsTyepeList.FirstOrDefault(x => x.ProducttypeId == subcateid);
                            BrandPriceList brandPriceList = productsType.Brand.FirstOrDefault(x => x.BrandId == brandId);
                            switch (qualityId)
                            {
                                case 1:
                                    productPrice = brandPriceList.Price;
                                    break;
                                case 2:
                                    productPrice = brandPriceList.Mid_Price;
                                    break;
                                case 3:
                                    productPrice = brandPriceList.Min_Price;
                                    break;
                                case 4:
                                    productPrice = brandPriceList.Scrap_Price;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("MasterManager", "GetProductPriceWithModelBasedSweetner", ex);
            }
            price = Convert.ToDouble(productPrice);
            productPrice = (price + SweetnerAmmount).ToString();
            return productPrice;
        }
        #endregion

        #region Plan Price For ABB
        public plandetail GetABBPlanPrice(int productCatId, int producttypeId, int buid, string productValue, int GstType)
        {
            _priceMasterRepository = new ABBPriceMasterRepository();
            ABBPlanMasterRepository _planMasterepository = new ABBPlanMasterRepository();
            List<tblABBPriceMaster> abbplanpriceObj = new List<tblABBPriceMaster>();
            _buisnessUnitRepository = new BusinessUnitRepository();
            string planPrice = string.Empty;
            plandetail plan = new plandetail();
            try
            {
                if (productCatId > 0 && producttypeId > 0 && buid > 0 && productValue != string.Empty || productValue != null)
                {

                    abbplanpriceObj = _priceMasterRepository.GetList(x => x.ProductCatId == productCatId && x.ProductTypeId == producttypeId && x.BusinessUnitId == buid && x.IsActive == true).ToList();
                    if (abbplanpriceObj != null && abbplanpriceObj.Count > 0)
                    {
                        tblABBPlanMaster planmasterObj = _planMasterepository.GetSingle(x => x.ProductCatId == productCatId && x.ProductTypeId == producttypeId && x.BusinessUnitId == buid && x.IsActive == true);
                        if (planmasterObj != null)
                        {
                            //code to calculate Gst amount
                            ///formula to calculate gst <>GSt value=GST Inclusive Price * GST Rate /(100 + GST Rate Percentage)</>
                            ///formula to calculate original cost <>original cost=GST Inclusive Price * 100/(100 + GST Rate Percentage)</>
                            int productPrice = Convert.ToInt32(productValue);
                            foreach (var item in abbplanpriceObj)
                            {
                                if (item.Fees_Applicable_Amt > 0)
                                {
                                    if (productPrice >= item.Price_Start_Range && productPrice <= item.Price_End_Range)
                                    {
                                        plan.planprice = item.Fees_Applicable_Amt.ToString();
                                        plan.planduration = item.Plan_Period_in_Months.ToString();
                                        plan.NoClaimPeriod = planmasterObj.NoClaimPeriod;
                                        plan.planName = planmasterObj.ABBPlanName;
                                        if (GstType == Convert.ToInt32(ABBPlanEnum.GstExclusive) && item.GSTExclusive > 0)
                                        {

                                            double cgst = 0;
                                            double gstExclusive = Convert.ToDouble(item.GSTExclusive);
                                            double planamount = 0;
                                            if (!string.IsNullOrEmpty(plan.planprice))
                                            {
                                                planamount = Convert.ToDouble(plan.planprice);
                                                plan.BaseValue = planamount.ToString();
                                                cgst = gstExclusive / 2;
                                                plan.Cgst = cgst.ToString();
                                                plan.Sgst = cgst.ToString();
                                                double gstvalue = (planamount * gstExclusive) / 100;
                                                var finalamount = planamount + gstvalue;
                                                plan.planprice = finalamount.ToString();
                                            }
                                            else
                                            {
                                                plan.Message = "Plan data is not available";
                                            }
                                        }
                                        else if (GstType == Convert.ToInt32(ABBPlanEnum.GstInclusive) && item.GSTInclusive > 0)
                                        {
                                            double gstvalue = 0;
                                            double gstInclusive = Convert.ToDouble(item.GSTInclusive);
                                            double planamount = 0;
                                            double BaseValue = 0;
                                            if (!string.IsNullOrEmpty(plan.planprice))
                                            {
                                                planamount = Convert.ToDouble(plan.planprice);
                                                gstvalue = gstInclusive / 2;
                                                plan.Cgst = gstvalue.ToString();
                                                plan.Sgst = gstvalue.ToString();
                                                BaseValue = planamount * 100 / (gstInclusive + 100);
                                                plan.planprice = plan.planprice;
                                                plan.BaseValue = BaseValue.ToString();

                                            }
                                            else
                                            {
                                                plan.Message = "Plan data is not available";
                                            }
                                        }
                                        else
                                        {
                                            double planamount = 0;
                                            double BaseValue = 0;
                                            if (!string.IsNullOrEmpty(plan.planprice))
                                            {
                                                planamount = Convert.ToDouble(plan.planprice);
                                                plan.Cgst = "0";
                                                plan.Sgst = "0";
                                                BaseValue = planamount;
                                                plan.planprice = plan.planprice;
                                                plan.BaseValue = BaseValue.ToString();

                                            }
                                            else
                                            {
                                                plan.Message = "Plan data is not available";
                                            }
                                        }
                                    }
                                }
                                else if (item.Fees_Applicable_Percentage > 0)
                                {
                                    if (productPrice >= item.Price_Start_Range && productPrice <= item.Price_End_Range)
                                    {
                                        decimal fees_percentage = Convert.ToDecimal(item.Fees_Applicable_Percentage);
                                        decimal productprice = Convert.ToDecimal(productValue);
                                        var planprice = (productprice * fees_percentage) / 100;
                                        plan.planprice = planprice.ToString();
                                        plan.planduration = item.Plan_Period_in_Months.ToString();
                                        plan.NoClaimPeriod = planmasterObj.NoClaimPeriod;
                                        plan.planName = planmasterObj.ABBPlanName;
                                        if (GstType == Convert.ToInt32(ABBPlanEnum.GstExclusive) && item.GSTExclusive > 0)
                                        {
                                            decimal gstamount = Convert.ToDecimal(item.GSTExclusive);
                                            decimal planamount = 0;
                                            if (!string.IsNullOrEmpty(plan.planprice))
                                            {
                                                plan.BaseValue = plan.planprice;
                                                decimal cgst = 0;
                                                cgst = gstamount / 2;
                                                plan.Cgst = cgst.ToString();
                                                plan.Sgst = cgst.ToString();
                                                planamount = Convert.ToDecimal(plan.planprice);
                                                var finalamount = (planamount * gstamount) / 100;
                                                finalamount = planprice + finalamount;
                                                plan.planprice = finalamount.ToString();
                                            }
                                            else
                                            {
                                                plan.Message = "Plan data is not available";
                                            }


                                        }
                                        else if (GstType == Convert.ToInt32(ABBPlanEnum.GstInclusive) && item.GSTInclusive > 0)
                                        {
                                            decimal gstpercentage = Convert.ToDecimal(item.GSTInclusive);
                                            decimal planamount = 0;
                                            if (!string.IsNullOrEmpty(plan.planprice))
                                            {
                                                decimal BaseValue = 0;
                                                decimal cndsgstvalue = 0;
                                                cndsgstvalue = gstpercentage / 2;
                                                plan.Cgst = cndsgstvalue.ToString();
                                                plan.Sgst = cndsgstvalue.ToString();
                                                planamount = Convert.ToDecimal(plan.planprice);
                                                BaseValue = planamount * 100 / (100 + gstpercentage);
                                                plan.BaseValue = BaseValue.ToString();
                                                plan.planprice = planamount.ToString();
                                            }
                                            else
                                            {
                                                plan.Message = "Plan data is not available";
                                            }
                                        }
                                        else
                                        {
                                         
                                            decimal planamount = 0;
                                            if (!string.IsNullOrEmpty(plan.planprice))
                                            {
                                                decimal BaseValue = 0;
                                               
                                                plan.Cgst = "0";
                                                plan.Sgst = "0";
                                                planamount = Convert.ToDecimal(plan.planprice);
                                                BaseValue = planamount;
                                                plan.BaseValue = BaseValue.ToString();
                                                plan.planprice = planamount.ToString();
                                            }
                                            else
                                            {
                                                plan.Message = "Plan data is not available";
                                            }
                                        }
                                    }
                                }

                            }

                            if (plan.planprice == null && plan.planduration == null && plan.NoClaimPeriod == null)
                            {
                                plan.Message = "Plan data is not available";
                            }
                            else
                            {
                                double amount = Convert.ToDouble(plan.planprice);
                                string amountnew = String.Format("{0:0.00}", amount);
                                plan.planprice = amountnew;
                                plan.Message = "Success";
                            }
                        }
                        else
                        {
                            plan.Message = "Plan data is not available";
                        }

                    }
                    else
                    {
                        plan.Message = "Plan price is not available";
                    }


                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("MasterManager", "GetABBPlanPrice", ex);
            }
            return plan;
        }
        #endregion
        #region Get dealer info for is deffered abb
        public bool GetdealerIsAbb(int businesspartnerid)
        {
            _businessPartnerReposirory = new BusinessPartnerRepository();
            bool flag = false;
            try
            {
                tblBusinessPartner businesspartnerObj = _businessPartnerReposirory.GetSingle(x => x.BusinessPartnerId == businesspartnerid && x.IsActive == true);
                if (businesspartnerObj != null)
                {
                    if (businesspartnerObj.IsDefferedAbb == true)
                    {
                        flag = true;
                    }
                    else
                    {
                        flag = false;
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("MasterManager", "GetdealerIsAbb", ex);
            }

            return flag;
        }
        #endregion

        #region getabbplandetails
        public List<abbplanmaster> getabbplandetails(int productCatId, int productSubCatId, int buid, string productPrice)
        {

            List<abbplanmaster> abbdatalist = new List<abbplanmaster>();
            ABBPlanMasterRepository abbplanmasterRepository = new ABBPlanMasterRepository();
            List<tblABBPlanMaster> abbplanlist = new List<tblABBPlanMaster>();
            try
            {
                if (productCatId > 0 && productSubCatId > 0 && buid > 0 && productPrice != null || productPrice != string.Empty)
                {
                    abbplanlist = abbplanmasterRepository.GetList(x => x.ProductCatId == productCatId && x.ProductTypeId == productSubCatId && x.BusinessUnitId == buid).ToList();
                    if (abbplanlist.Count > 0)
                    {
                        foreach (var item in abbplanlist)
                        {
                            abbplanmaster abbplan = new abbplanmaster();
                            abbplan.Assured_BuyBack_Percentage = item.Assured_BuyBack_Percentage.ToString();
                            abbplan.From_month = item.From_Month.ToString();
                            abbplan.To_month = item.To_Month.ToString();
                            abbdatalist.Add(abbplan);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("MasterManager", "getabbplandetails", ex);
            }
            return abbdatalist;
        }
        #endregion

        #region GetProductPriceWithoutSweetner Add by VK Date 19-July, For BU Based Validation on Sweetner
        public string GetProductPriceWithoutSweetner(int catid, int subcateid, int brandId, int qualityId, int businessUnitId, int modelnoId, string formatType = "")
        {
            #region Variable Declaration 
            _productPriceRepository = new PriceMasterRepository();
            _brandRepository = new BrandRepository();
            _productTypeRepository = new ProductTypeRepository();
            _buisnessUnitRepository = new BusinessUnitRepository();
            _productCategoryRepository = new ProductCategoryRepository();
            _loginDetailsUTCRepository = new LoginDetailsUTCRepository();
            modelNumberRepository = new ModelNumberRepository();
            List<tblPriceMaster> tblProductPriceObjList = new List<tblPriceMaster>();
            List<ProductsPricesDataContract> productsPricesList = new List<ProductsPricesDataContract>();
            List<ProductsFromTypePriceList> productsTyepeList = new List<ProductsFromTypePriceList>();
            List<BrandPriceList> brandList = null;
            string productPrice = string.Empty;
            string priceCode = string.Empty;
            #endregion

            try
            {
                Login loginObj = _loginDetailsUTCRepository.GetSingle(x => x.SponsorId == businessUnitId);
                if (loginObj != null)
                {
                    priceCode = loginObj.PriceCode;
                }
                //Get the brand list 
                List<tblBrand> masterbrandList = _brandRepository.GetList(x => x.IsActive == true).ToList();
                List<tblProductType> masterCategoryTypeList = _productTypeRepository.GetList(x => x.IsActive == true).ToList();

                //Get the Product price list
                if (catid > 0)
                {
                    #region For Category Name

                    tblProductPriceObjList = _productPriceRepository.GetList(x => x.IsActive == true
                    && (x.ProductCategoryId != null && x.ProductCategoryId == catid)
                    && (x.ExchPriceCode != null && x.ExchPriceCode.ToLower().Contains(priceCode.ToLower()))
                    && (x.ProductTypeId != null && x.ProductTypeId == subcateid)).ToList();
                    //Add the condition for Exch Price Code
                    //&& (x.ExchPriceCode != null && x.ExchPriceCode.ToLower().Contains("rel"))

                    if (tblProductPriceObjList != null && tblProductPriceObjList.Count > 0)
                    {
                        foreach (var item in tblProductPriceObjList)
                        {
                            //ProductsPricesDataContract ProductsPrices = new ProductsPricesDataContract();

                            ProductsFromTypePriceList productsType = new ProductsFromTypePriceList();

                            productsType.ProducttypeId = (int)item.ProductTypeId;
                            productsType.id = item.Id;

                            #region code to fill brand list

                            BrandPriceList brandPL = null;
                            brandList = new List<BrandPriceList>();
                            string BrandDescription = ExchangeOrderManager.GetEnumDescription(PriceMasterBrandEnum.Others);
                            //For Brand 1
                            if (!string.IsNullOrEmpty(item.BrandName_1))
                            {
                                if (item.BrandName_1 == BrandDescription)
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_1.ToLower()) && x.IsActive == true).Id;
                                    brandPL.Name = item.BrandName_1;
                                    brandPL.Price = item.Quote_P;
                                    brandPL.Mid_Price = item.Quote_Q;
                                    brandPL.Min_Price = item.Quote_R;
                                    brandPL.Scrap_Price = item.Quote_S;
                                    brandList.Add(brandPL);
                                }
                                else
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_1.ToLower()) && x.IsActive == true).Id;
                                    brandPL.Name = item.BrandName_1;
                                    brandPL.Price = item.Quote_P_High;
                                    brandPL.Mid_Price = item.Quote_Q_High;
                                    brandPL.Min_Price = item.Quote_R_High;
                                    brandPL.Scrap_Price = item.Quote_S_High;
                                    brandList.Add(brandPL);
                                }

                            }

                            //For Brand 2
                            if (!string.IsNullOrEmpty(item.BrandName_2))
                            {
                                if (item.BrandName_2 == BrandDescription)
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_2.ToLower()) && x.IsActive == true).Id;
                                    brandPL.Name = item.BrandName_2;
                                    brandPL.Price = item.Quote_P;
                                    brandPL.Mid_Price = item.Quote_Q;
                                    brandPL.Min_Price = item.Quote_R;
                                    brandPL.Scrap_Price = item.Quote_S;
                                    brandList.Add(brandPL);
                                }
                                else
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_2.ToLower()) && x.IsActive == true).Id;
                                    brandPL.Name = item.BrandName_2;
                                    brandPL.Price = item.Quote_P_High;
                                    brandPL.Mid_Price = item.Quote_Q_High;
                                    brandPL.Min_Price = item.Quote_R_High;
                                    brandPL.Scrap_Price = item.Quote_S_High;
                                    brandList.Add(brandPL);
                                }
                            }

                            //For Brand 3
                            if (!string.IsNullOrEmpty(item.BrandName_3))
                            {
                                if (item.BrandName_3 == BrandDescription)
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_3.ToLower()) && x.IsActive == true).Id;
                                    brandPL.Name = item.BrandName_3;
                                    brandPL.Price = item.Quote_P;
                                    brandPL.Mid_Price = item.Quote_Q;
                                    brandPL.Min_Price = item.Quote_R;
                                    brandPL.Scrap_Price = item.Quote_S;
                                    brandList.Add(brandPL);
                                }
                                else
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_3.ToLower()) && x.IsActive == true).Id;
                                    brandPL.Name = item.BrandName_3;
                                    brandPL.Price = item.Quote_P_High;
                                    brandPL.Mid_Price = item.Quote_Q_High;
                                    brandPL.Min_Price = item.Quote_R_High;
                                    brandPL.Scrap_Price = item.Quote_S_High;
                                    brandList.Add(brandPL);
                                }
                            }

                            //For Brand 4
                            if (!string.IsNullOrEmpty(item.BrandName_4))
                            {
                                if (item.BrandName_4 == BrandDescription)
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_4.ToLower()) && x.IsActive == true).Id;
                                    brandPL.Name = item.BrandName_4;
                                    brandPL.Price = item.Quote_P;
                                    brandPL.Mid_Price = item.Quote_Q;
                                    brandPL.Min_Price = item.Quote_R;
                                    brandPL.Scrap_Price = item.Quote_S;
                                    brandList.Add(brandPL);
                                }
                                else
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_4.ToLower()) && x.IsActive == true).Id;
                                    brandPL.Name = item.BrandName_4;
                                    brandPL.Price = item.Quote_P_High;
                                    brandPL.Mid_Price = item.Quote_Q_High;
                                    brandPL.Min_Price = item.Quote_R_High;
                                    brandPL.Scrap_Price = item.Quote_S_High;
                                    brandList.Add(brandPL);
                                }
                            }

                            //For Other
                            brandPL = new BrandPriceList();
                            brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals("Others".ToLower()) && x.IsActive == true).Id;
                            brandPL.Name = "Others";
                            brandPL.Price = item.Quote_P;
                            brandPL.Mid_Price = item.Quote_Q;
                            brandPL.Min_Price = item.Quote_R;
                            brandPL.Scrap_Price = item.Quote_S;
                            brandList.Add(brandPL);
                            //productsPriceList.Add(productsPrice);
                            #endregion

                            productsType.Brand = brandList;
                            productsTyepeList.Add(productsType);
                        }
                        if (productsTyepeList != null && productsTyepeList.Count > 0)
                        {
                            ProductsFromTypePriceList productsType = productsTyepeList.FirstOrDefault(x => x.ProducttypeId == subcateid);
                            BrandPriceList brandPriceList = productsType.Brand.FirstOrDefault(x => x.BrandId == brandId);
                            if (brandPriceList != null)
                            {
                                switch (qualityId)
                                {
                                    case 1:
                                        productPrice = brandPriceList.Price;
                                        break;
                                    case 2:
                                        productPrice = brandPriceList.Mid_Price;
                                        break;
                                    case 3:
                                        productPrice = brandPriceList.Min_Price;
                                        break;
                                    case 4:
                                        productPrice = brandPriceList.Scrap_Price;
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("MasterManager", "GetProductPriceWithModelBasedSweetner", ex);
            }
            return productPrice;
        }
        #endregion

        #region GetSweetnerPrice Add by VK Date 19-July, For BU Based Validation on Sweetner
        public string GetSweetnerPrice(int newcatid, int newsubcatid, int businessUnitId, int modelnoId, string formatType = "")
        {
            _buisnessUnitRepository = new BusinessUnitRepository();
            _loginDetailsUTCRepository = new LoginDetailsUTCRepository();
            modelNumberRepository = new ModelNumberRepository();
            /*string priceCode = string.Empty;*/
            double SweetnerAmmount = 0;
            string SweetnerAmmountString = "0";
            try
            {
                /*Login loginObj = _loginDetailsUTCRepository.GetSingle(x => x.SponsorId == businessUnitId);
                if (loginObj != null)
                {
                    priceCode = loginObj.PriceCode;
                }*/
                //Get Sweetner Amount For Perticular Brand
                if (!string.IsNullOrEmpty(formatType))
                {
                    tblBusinessUnit buisnessobj = _buisnessUnitRepository.GetSingle(x => x.BusinessUnitId == businessUnitId);
                    if (buisnessobj != null && buisnessobj.IsSweetnerModelBased == true)
                    {
                        if (modelnoId != 0)
                        {
                            tblModelNumber modelObj = modelNumberRepository.GetSingle(x => x.ProductCategoryId == newcatid && x.ProductTypeId == newsubcatid && x.IsDefaultProduct == false && x.ModelNumberId.Equals(modelnoId) && x.BusinessUnitId == businessUnitId);
                            if (modelObj != null && modelObj.SweetnerForDTC != null || modelObj.SweetnerForDTD != null && modelObj.IsDefaultProduct == false)
                            {
                                string Format = ExchangeOrderManager.GetEnumDescription((FormatTypeEnum.Dealer));
                                if (formatType == Format)
                                {
                                    SweetnerAmmount = (double)modelObj.SweetnerForDTD;
                                }
                                else
                                {
                                    SweetnerAmmount = (double)modelObj.SweetnerForDTC;
                                }
                            }
                            else
                            {
                                SweetnerAmmount = 0;
                            }
                        }
                        else
                        {
                            tblModelNumber modelObj = modelNumberRepository.GetSingle(x => x.ProductCategoryId == newcatid && x.ProductTypeId == newsubcatid && x.IsDefaultProduct == true && x.BusinessUnitId == businessUnitId);
                            if (modelObj != null && (modelObj.SweetnerForDTC != null || modelObj.SweetnerForDTD != null) && modelObj.IsDefaultProduct == true)
                            {
                                string Format = ExchangeOrderManager.GetEnumDescription((FormatTypeEnum.Dealer));
                                if (formatType == Format)
                                {
                                    SweetnerAmmount = (double)modelObj.SweetnerForDTD;
                                }
                                else
                                {
                                    SweetnerAmmount = (double)modelObj.SweetnerForDTC;
                                }
                            }
                            else
                            {
                                SweetnerAmmount = 0;
                            }
                        }
                    }
                    else if (buisnessobj != null && buisnessobj.SweetnerForDTC != null || buisnessobj.SweetnerForDTD != null)
                    {
                        if (formatType.Equals("Home"))
                        {
                            SweetnerAmmount = (double)buisnessobj.SweetnerForDTC;
                        }
                        else
                        {
                            SweetnerAmmount = (double)buisnessobj.SweetnerForDTD;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("MasterManager", "GetProductPriceWithModelBasedSweetner", ex);
            }
            SweetnerAmmountString = SweetnerAmmount.ToString();
            return SweetnerAmmountString;
        }
        #endregion

        #region GetSweetnerPrice Add by VK Date 21-July, For BU Based Validation on Sweetner
        public List<BUBasedSweetnerValidation> GetBUSweetnerValidationQuestions(int businessUnitId)
        {
            _buisnessUnitRepository = new BusinessUnitRepository();
            _questionsForSweetnerRepository = new QuestionsForSweetnerRepository();
            _bUBasedSweetnerValidationRepository = new BUBasedSweetnerValidationRepository();
            List<BUBasedSweetnerValidation> buBasedSweetnerValidationsList = new List<BUBasedSweetnerValidation>();
            BUBasedSweetnerValidation bUBasedSweetnerValidation = null;
            tblQuestionsForSweetner tblQuestionsForSweetner = null;
            List<tblBUBasedSweetnerValidation> tblBUBasedSweetnerValidationList = null;
            try
            {
                if (businessUnitId > 0)
                {

                    tblBUBasedSweetnerValidationList = _bUBasedSweetnerValidationRepository.GetList(x => x.IsActive == true && x.IsDisplay == true && x.BusinessUnitId == businessUnitId).ToList();
                    if (tblBUBasedSweetnerValidationList != null && tblBUBasedSweetnerValidationList.Count > 0)
                    {
                        foreach (tblBUBasedSweetnerValidation item in tblBUBasedSweetnerValidationList)
                        {
                            if (item != null)
                            {
                                bUBasedSweetnerValidation = new BUBasedSweetnerValidation();
                                tblQuestionsForSweetner = _questionsForSweetnerRepository.GetSingle(x => x.IsActive == true && x.QuestionId == item.QuestionId);
                                if (tblQuestionsForSweetner != null)
                                {
                                    bUBasedSweetnerValidation = GenericMapper<tblBUBasedSweetnerValidation, BUBasedSweetnerValidation>.MapObject(item);
                                    bUBasedSweetnerValidation.Question = tblQuestionsForSweetner.Question;
                                    buBasedSweetnerValidationsList.Add(bUBasedSweetnerValidation);
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("MasterManager", "GetBUSweetnerValidationQuestions", ex);
            }
            return buBasedSweetnerValidationsList;
        }
        #endregion

        #region Get Model details for new productm
        public ModelDetailsDataContract GetModelList(int BussinessUnitId, int BusinessPartnerId, int NewCatId, int NewrtypeId, int NewBrandId)
        {
            modelNumberRepository = new ModelNumberRepository();
            ModelDetailsDataContract modeldetailsdc = new ModelDetailsDataContract();
            List<tblModelNumber> modelNumbersList = new List<tblModelNumber>();
            List<ModalListdataDataContract> modelList = new List<ModalListdataDataContract>();

            try
            {
                DataTable dt = modelNumberRepository.GetModellist(NewCatId, NewrtypeId, BussinessUnitId, BusinessPartnerId, NewBrandId);
                if (dt != null && dt.Rows.Count > 0)
                {
                    modelNumbersList = GenericConversionHelper.DataTableToList<tblModelNumber>(dt);
                }
                else
                {
                    modeldetailsdc.ErrorMessage = "No model found";
                }
                if (modelNumbersList != null && modelNumbersList.Count > 0)
                {
                    modelList = GenericMapper<tblModelNumber, ModalListdataDataContract>.MapList(modelNumbersList);
                    modeldetailsdc.ModelList = modelList;
                }
                else
                {
                    modeldetailsdc.ErrorMessage = "No model found";
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("MasterManager", "GetModelList", ex);
            }
            return modeldetailsdc;
        }
        #endregion
    }
}
