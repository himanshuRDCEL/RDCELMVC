using GraspCorn.Common.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DataContract.MasterModel;
using RDCEL.DocUpload.DataContract.ProductTaxonomy;
using RDCEL.DocUpload.DataContract.UniversalPricemasterDetails;

namespace RDCEL.DocUpload.BAL.OldProductDetailsManager
{
    public class OldProductDetailsManager
    {
        #region Variable declaration
        PriceMasterMappingRepository _priceMasterMapping;
        UniversalPriceMasterRepository _universalPriceMasterMapping;
        ProductCategoryRepository _productCategoryRepository;
        ProductTypeRepository _productTypeRepository;
        BrandRepository _brandRepository;
        BUProductCategoryMappingRepository _bUProductCategoryMappingRepository;
        ModelNumberRepository _modelNumberRepository;
        ModalMappingRepository _modelMappingRepository;
        #endregion

        #region getProdPriceName
        public PriceMasterNameDataContract GetPriceNameId(PriceMasterMappingDataContract detials)
        {
            _universalPriceMasterMapping = new UniversalPriceMasterRepository();
            _priceMasterMapping = new PriceMasterMappingRepository();
            PriceMasterNameDataContract pricemsterData = new PriceMasterNameDataContract();
            tblPriceMasterMapping mappingObj = new tblPriceMasterMapping();
            try
            {
                if (detials !=null && detials.NewBrandId > 0)
                {
                    if (detials.BusinessPartnerId > 0)
                    {
                        if (detials.BusinessunitId > 0)
                        {
                            mappingObj = _priceMasterMapping.GetSingle(x => x.IsActive == true && x.BusinessUnitId == detials.BusinessunitId && x.BusinessPartnerId == detials.BusinessPartnerId && x.BrandId == detials.NewBrandId);
                            if (mappingObj != null)
                            {
                                pricemsterData.PriceNameId = mappingObj.PriceMasterNameId;
                            }
                            else
                            {
                                mappingObj = _priceMasterMapping.GetSingle(x => x.IsActive == true && x.BusinessUnitId == detials.BusinessunitId && x.BusinessPartnerId == detials.BusinessPartnerId && x.BrandId == null);
                                if (mappingObj != null)
                                {
                                    pricemsterData.PriceNameId = mappingObj.PriceMasterNameId;
                                }
                                else
                                {
                                    mappingObj = _priceMasterMapping.GetSingle(x => x.IsActive == true && x.BusinessUnitId == detials.BusinessunitId && x.BusinessPartnerId == null && x.BrandId == null);
                                    if (mappingObj != null)
                                    {
                                        pricemsterData.PriceNameId = mappingObj.PriceMasterNameId;
                                    }
                                    else
                                    {
                                        pricemsterData.ErrorMessage = "Price master not found for this order";
                                    }
                                }
                            }
                            
                        }
                        else
                        {
                            pricemsterData.ErrorMessage = "Please provide business unit id";
                        }
                    }
                    else
                    {
                        pricemsterData.ErrorMessage = "Please provide business partner id";
                    }

                }
                else
                {
                    mappingObj = _priceMasterMapping.GetSingle(x => x.IsActive == true && x.BusinessUnitId == detials.BusinessunitId && x.BusinessPartnerId == detials.BusinessPartnerId && x.BrandId == null);
                    if (mappingObj != null)
                    {
                        pricemsterData.PriceNameId = mappingObj.PriceMasterNameId;
                    }
                    else
                    {
                        mappingObj = _priceMasterMapping.GetSingle(x => x.IsActive == true && x.BusinessUnitId == detials.BusinessunitId && x.BusinessPartnerId == null && x.BrandId == null);
                        if (mappingObj != null)
                        {
                            pricemsterData.PriceNameId = mappingObj.PriceMasterNameId;
                        }
                        else
                        {
                            pricemsterData.ErrorMessage = "Price master not found for this order";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("OldProductDetailsManager", "GetPriceNameId", ex);
            }
            return pricemsterData;
        }
        #endregion
        #region OldBrand catid
        public List<BrandName> GetBrandOldByPriceMasterId(ProductDetailsForOldDataContract details)
        {
            _universalPriceMasterMapping = new UniversalPriceMasterRepository();
            _brandRepository = new BrandRepository();
            List<BrandName> brandList = null;
            List<tblBrand> tblbrandObjList = new List<tblBrand>();
            BrandDataContract brandNew = new BrandDataContract();
            try
            {
                if (details.OldProductTypeId > 0)
                {
                    DataTable dt = _universalPriceMasterMapping.GetBrandsByPriceMasterNameId(details.OldProductcategoryId, details.PriceMasterNameId, details.OldProductTypeId);
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
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("OldProductDetailsManager", "GetBrandOldByPriceMasterId", ex);
            }
            return brandList;
        }
        #endregion
        #region Product Category
        public List<tblProductCategory> GetProductCatListByPriceMasterNameId(int? priceMasterNameID)
        {
            #region variable's declaration
            _productCategoryRepository = new ProductCategoryRepository();
            _universalPriceMasterMapping = new UniversalPriceMasterRepository();
            List<tblProductCategory> productCategoriesList = new List<tblProductCategory>();
            List<tblUniversalPriceMaster> universalPriceMasters = new List<tblUniversalPriceMaster>();
            tblProductCategory categoryObj = null;
            #endregion
            try
            {
                if (priceMasterNameID > 0)
                {
                    DataTable dt = _universalPriceMasterMapping.GetProductCategoryByPriceMasterId(priceMasterNameID);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        universalPriceMasters = GenericConversionHelper.DataTableToList<tblUniversalPriceMaster>(dt);
                        productCategoriesList = new List<tblProductCategory>();
                        foreach (var productCat in universalPriceMasters)
                        {
                            categoryObj = _productCategoryRepository.GetSingle(x => x.IsActive == true && x.IsAllowedForOld == true && x.Id == productCat.ProductCategoryId);
                            if (categoryObj != null)
                            {
                                productCategoriesList.Add(categoryObj);
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                    else
                    {
                        productCategoriesList = null;
                    }
                }
                else
                {
                    productCategoriesList = null;
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("OldProductDetailsManager", "GetProductCatListByPriceMasterNameId", ex);
            }
            return productCategoriesList;
        }
        #endregion

        #region GetProductType
        public List<tblProductType> GetProTypeListByPriceMasterNameId(int? priceMasterNameID, int proCatId)
        {
            #region variable's declaration
            _universalPriceMasterMapping = new UniversalPriceMasterRepository();
            _productTypeRepository = new ProductTypeRepository();
            List<tblProductType> tblProductTypeList = new List<tblProductType>();
            List<tblUniversalPriceMaster> universalPriceMasters = new List<tblUniversalPriceMaster>();
            tblProductType proTypeObj = null;
            #endregion
            try
            {
                if (priceMasterNameID > 0 && proCatId > 0)
                {
                    DataTable dt = _universalPriceMasterMapping.GetProductTypeByPriceMasterNameId(priceMasterNameID, proCatId);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        universalPriceMasters = GenericConversionHelper.DataTableToList<tblUniversalPriceMaster>(dt);
                        tblProductTypeList = new List<tblProductType>();
                        foreach (var productType in universalPriceMasters)
                        {
                            proTypeObj = _productTypeRepository.GetSingle(x => x.IsActive == true && x.IsAllowedForOld == true && x.Id == productType.ProductTypeId);
                            if (proTypeObj != null)
                            {
                                tblProductTypeList.Add(proTypeObj);
                            }
                        }
                    }
                    else
                    {
                        tblProductTypeList = null;
                    }
                }
                else if(priceMasterNameID>0)
                {
                    DataTable dt = _universalPriceMasterMapping.GetProductTypeByPriceMasterNameIdOnly (priceMasterNameID);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        universalPriceMasters = GenericConversionHelper.DataTableToList<tblUniversalPriceMaster>(dt);

                        foreach (var productType in universalPriceMasters)
                        {
                            proTypeObj = _productTypeRepository.GetSingle(x => x.IsActive == true && x.IsAllowedForOld == true && x.Id == productType.ProductTypeId);
                            if (proTypeObj != null)
                            {
                                tblProductTypeList.Add(proTypeObj);
                            }
                        }
                    }
                    else
                    {
                        tblProductTypeList = null;
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("PriceMasterManager", "GetProductCatListByPriceMasterNameId", ex);
            }
            return tblProductTypeList;
        }
        #endregion

        #region OldBrand all
        public List<BrandName> GetAllBrandOldByPriceMasterId(PriceMasterNameDataContract details)
        {
            _universalPriceMasterMapping = new UniversalPriceMasterRepository();
            _brandRepository = new BrandRepository();
            List<BrandName> brandList = null;
            List<tblBrand> tblbrandObjList = new List<tblBrand>();
            BrandDataContract brandNew = new BrandDataContract();
            try
            {
                DataTable dt = _brandRepository.GetBrandList(details.PriceNameId);
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
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("OldProductDetailsManager", "GetBrandOldByPriceMasterId", ex);
            }
            return brandList;
        }
        #endregion

        #region Get Product Category List by BU Id
        public List<tblProductCategory> GetProductCatListByBUId(int BuId)
        {
            _productCategoryRepository = new ProductCategoryRepository();
            List<tblProductCategory> productCategoriesList = new List<tblProductCategory>();
            tblProductCategory categoryObj = null;
            _bUProductCategoryMappingRepository = new BUProductCategoryMappingRepository();
            List<tblBUProductCategoryMapping> productCategoryMappings = new List<tblBUProductCategoryMapping>();
            try
            {
                if(BuId > 0)
                {
                    DataTable dt = _bUProductCategoryMappingRepository.GetProductCategoryByBUId(BuId);
                    if(dt != null && dt.Rows.Count > 0)
                    {
                        productCategoryMappings = GenericConversionHelper.DataTableToList<tblBUProductCategoryMapping>(dt);
                        foreach(var productCat in productCategoryMappings)
                        {
                            categoryObj = _productCategoryRepository.GetSingle(x => x.IsActive == true && x.IsAllowedForNew == true && x.Id == productCat.ProductCatId);
                            if(categoryObj != null)
                            {
                                productCategoriesList.Add(categoryObj);
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                    else
                    {
                        productCategoriesList = null;
                    }
                }
                else
                {
                    productCategoriesList = null;
                }
            }
            catch(Exception ex)
            {
                LibLogging.WriteErrorToDB("OldProductDetailsManager", "GetProductCatListByBUId", ex);
            }
            return productCategoriesList;
        }
        #endregion

        #region Get Product Type List By BU Id
        public List<tblProductType> GetProductTypeListByBUId(int BUId, int productCatId)
        {
            _bUProductCategoryMappingRepository = new BUProductCategoryMappingRepository();
            _productTypeRepository = new ProductTypeRepository();
            List<tblProductType> productTypeList = new List<tblProductType>();
            tblProductType productTypeObj = null;
            List<tblBUProductCategoryMapping> productCategoryMappings = new List<tblBUProductCategoryMapping>();

            try
            {
                if(BUId > 0 && productCatId > 0)
                {
                    DataTable dt = _bUProductCategoryMappingRepository.GetProductTypeByBUIdandCatId(BUId, productCatId);
                    if(dt != null && dt.Rows.Count > 0)
                    {
                        productCategoryMappings = GenericConversionHelper.DataTableToList<tblBUProductCategoryMapping>(dt);
                        foreach(var productType in productCategoryMappings)
                        {
                            productTypeObj = _productTypeRepository.GetSingle(x => x.IsActive == true && x.IsAllowedForNew == true && x.Id == productType.ProductTypeId);
                            if(productTypeObj != null)
                            {
                                productTypeList.Add(productTypeObj);
                            }
                        }
                    }
                    else
                    {
                        productTypeList = null;
                    }
                }
                else if(BUId > 0)
                {
                    DataTable dt = _bUProductCategoryMappingRepository.GetProductTypeByBUIdOnly(BUId);
                    if(dt != null && dt.Rows.Count > 0)
                    {
                        productCategoryMappings = GenericConversionHelper.DataTableToList<tblBUProductCategoryMapping>(dt);
                        foreach(var productType in productCategoryMappings)
                        {
                            productTypeObj = _productTypeRepository.GetSingle(x => x.IsActive == true && x.IsAllowedForNew == true && x.Id == productType.ProductTypeId);
                            if(productTypeObj != null)
                            {
                                productTypeList.Add(productTypeObj);
                            }
                        }
                    }
                    else
                    {
                        productTypeList = null;
                    }
                }
            }
            catch(Exception ex)
            {
                LibLogging.WriteErrorToDB("OldProductDetailsManager", "GetProductTypeListByBUId", ex);
            }
            return productTypeList;
        }

        #endregion

        #region Get Model Numbers by BU Id
        //public List<ModelNumberModel> GetModelNumbersByModelId(int? ModelId)
        //{
        //    List<ModelNumberModel> modelNumberModels = null;
        //    _modelNumberRepository = new ModelNumberRepository();
        //    try
        //    {
        //        if(ModelId > 0)
        //        {
        //            List<tblModelNumber> tblModelNumbers = _modelNumberRepository.GetList(x => x.IsActive == true && x.BusinessUnitId == BUId && x.ProductCategoryId == catId && x.ProductTypeId == typeId).ToList();
        //            if (tblModelNumbers != null && tblModelNumbers.Count > 0)
        //            {
        //                modelNumberModels = GenericMapper<tblModelNumber, ModelNumberModel>.MapList(tblModelNumbers);
        //            }
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        LibLogging.WriteErrorToDB("OldProductDetailsManager", "GetListOfModelNumbers", ex);
        //    }
        //    return modelNumberModels;
        //}
        #endregion

    }
}
