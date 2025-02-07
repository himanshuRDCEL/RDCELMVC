using GraspCorn.Common.Enums;
using GraspCorn.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DAL.Helper;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DataContract.UniversalPricemasterDetails;



namespace RDCEL.DocUpload.BAL.ExchangePriceMaster
{
    public class PriceMasterManager
    {
        PriceMasterMappingRepository _priceMasterMappingRepository;
        UniversalPriceMasterRepository _universalPriceMasterRepository;
        BrandRepository _brandRepository;

        public UniversalPriceMasterDataContract GetProductPrice(ProductPriceDetailsDataContract details)
        {
            _priceMasterMappingRepository = new PriceMasterMappingRepository();
            UniversalPriceMasterDataContract priceDC = new UniversalPriceMasterDataContract();
            tblPriceMasterMapping priceMasterMappingObj = new tblPriceMasterMapping();
            try
            {
                if (details.NewBrandId > 0)
                {
                    priceMasterMappingObj = _priceMasterMappingRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == details.BusinessUnitId && x.BusinessPartnerId == details.BusinessPartnerId && x.BrandId == details.NewBrandId);
                    if (priceMasterMappingObj != null)
                    {
                        details.PriceNameId = priceMasterMappingObj.PriceMasterNameId;
                        priceDC = GetBasePrice(details);
                        priceDC.PricemasternameId= priceMasterMappingObj.PriceMasterNameId;
                    }
                    else
                    {
                        priceMasterMappingObj = _priceMasterMappingRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == details.BusinessUnitId && x.BusinessPartnerId == details.BusinessPartnerId && x.BrandId == null);
                        if (priceMasterMappingObj != null)
                        {
                            details.PriceNameId = priceMasterMappingObj.PriceMasterNameId;
                            priceDC = GetBasePrice(details);
                            priceDC.PricemasternameId = priceMasterMappingObj.PriceMasterNameId;

                        }
                        //Code to fetch Default price master for  that  businessunit which is mapped in price master mapping table
                        else
                        {
                            priceMasterMappingObj = _priceMasterMappingRepository.GetSingle(x => x.IsActive == true && x.BusinessPartnerId == null && x.BusinessUnitId == details.BusinessUnitId && x.BrandId == null);
                            if (priceMasterMappingObj != null)
                            {
                                details.PriceNameId = priceMasterMappingObj.PriceMasterNameId;
                                priceDC = GetBasePrice(details);
                                priceDC.PricemasternameId = priceMasterMappingObj.PriceMasterNameId;

                            }
                            else
                            {
                                priceDC.ErrorMessage = "No price master data found in mapping table for this order";
                            }
                        }
                    }
                }
                else
                {
                    priceMasterMappingObj = _priceMasterMappingRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == details.BusinessUnitId && x.BusinessPartnerId == details.BusinessPartnerId && x.BrandId == null);
                    if (priceMasterMappingObj != null)
                    {
                        details.PriceNameId = priceMasterMappingObj.PriceMasterNameId;
                        priceDC = GetBasePrice(details);
                        priceDC.PricemasternameId = priceMasterMappingObj.PriceMasterNameId;
                    }
                    //Code to fetch Default price master for  that  businessunit which is mapped in price master mapping table
                    else
                    {
                        priceMasterMappingObj = _priceMasterMappingRepository.GetSingle(x => x.IsActive == true && x.BusinessPartnerId == null && x.BusinessUnitId == details.BusinessUnitId && x.BrandId == null);
                        if (priceMasterMappingObj != null)
                        {
                            details.PriceNameId = priceMasterMappingObj.PriceMasterNameId;
                            priceDC = GetBasePrice(details);
                            priceDC.PricemasternameId = priceMasterMappingObj.PriceMasterNameId;
                        }
                        else
                        {
                            priceDC.ErrorMessage = "No price master data found in mapping table for this order";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("PriceMasterManager", "GetProductPrice", ex);
            }
            return priceDC;
        }

        public UniversalPriceMasterDataContract GetBasePrice(ProductPriceDetailsDataContract details)
        {
            _priceMasterMappingRepository = new PriceMasterMappingRepository();
            _universalPriceMasterRepository = new UniversalPriceMasterRepository();
            _brandRepository = new BrandRepository();
            UniversalPriceMasterDataContract priceDC = new UniversalPriceMasterDataContract();
            tblBrand brandObj = new tblBrand();
            tblUniversalPriceMaster UniversalpricemasterObj = new tblUniversalPriceMaster();
            ProductConditionList productconditionobj = new ProductConditionList();
            int? qualityId = 0;
            string productPrice = null;
            try
            {
                brandObj = _brandRepository.GetSingle(x => x.IsActive == true && x.Id == details.BrandId);
                if (brandObj != null)
                {
                    if (details.BrandId != Convert.ToInt32(BrandIdEnum.OtherBrand))
                    {
                        UniversalpricemasterObj = _universalPriceMasterRepository.GetSingle(x => x.IsActive == true && x.ProductCategoryId == details.ProductCatId && x.ProductTypeId == details.ProductTypeId && x.PriceMasterNameId == details.PriceNameId);
                        if (UniversalpricemasterObj != null)
                        {
                            productconditionobj.Price = UniversalpricemasterObj.Quote_P_High;
                            productconditionobj.Mid_Price = UniversalpricemasterObj.Quote_Q_High;
                            productconditionobj.Min_Price = UniversalpricemasterObj.Quote_R_High;
                            productconditionobj.Scrap_Price = UniversalpricemasterObj.Quote_S_High;
                            qualityId = details.conditionId;
                            switch (qualityId)
                            {
                                case 1:
                                    productPrice = productconditionobj.Price;
                                    break;
                                case 2:
                                    productPrice = productconditionobj.Mid_Price;
                                    break;
                                case 3:
                                    productPrice = productconditionobj.Min_Price;
                                    break;
                                case 4:
                                    productPrice = productconditionobj.Scrap_Price;
                                    break;
                                default:
                                    break;
                            }
                            if (!string.IsNullOrEmpty(productPrice))
                            {
                                priceDC.BaseValue = Convert.ToDecimal(productPrice);
                            }
                            else
                            {
                                priceDC.ErrorMessage = "Price not available for this product in price master";
                            }

                        }
                        else
                        {
                            priceDC.ErrorMessage = "Price master data not found";
                        }
                    }
                    else
                    {
                        UniversalpricemasterObj = _universalPriceMasterRepository.GetSingle(x => x.IsActive == true && x.ProductCategoryId == details.ProductCatId && x.ProductTypeId == details.ProductTypeId && x.PriceMasterNameId == details.PriceNameId);
                        if (UniversalpricemasterObj != null)
                        {
                            productconditionobj.Price = UniversalpricemasterObj.Quote_P;
                            productconditionobj.Mid_Price = UniversalpricemasterObj.Quote_Q;
                            productconditionobj.Min_Price = UniversalpricemasterObj.Quote_R;
                            productconditionobj.Scrap_Price = UniversalpricemasterObj.Quote_S;
                            qualityId = details.conditionId;
                            switch (qualityId)
                            {
                                case 1:
                                    productPrice = productconditionobj.Price;
                                    break;
                                case 2:
                                    productPrice = productconditionobj.Mid_Price;
                                    break;
                                case 3:
                                    productPrice = productconditionobj.Min_Price;
                                    break;
                                case 4:
                                    productPrice = productconditionobj.Scrap_Price;
                                    break;
                                default:
                                    break;
                            }



                            if (!string.IsNullOrEmpty(productPrice))
                            {
                                priceDC.BaseValue = Convert.ToDecimal(productPrice);
                            }
                            else
                            {
                                priceDC.ErrorMessage = "Price not available for this product in price master";
                            }

                        }
                        else
                        {
                            priceDC.ErrorMessage = "Price master data not found";
                        }
                    }
                }
                else
                {
                    priceDC.ErrorMessage = "Brand id is not valid no brand found for this order";
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("PriceMasterManager", "GetBasePrice", ex);
            }
            return priceDC;
        }
    }
}