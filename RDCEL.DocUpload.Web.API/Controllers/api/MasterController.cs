using RDCEL.DocUpload.BAL.SponsorsApiCall;
using RDCEL.DocUpload.DataContract.Base;
using RDCEL.DocUpload.DataContract.ProductTaxonomy;
using RDCEL.DocUpload.DataContract.SponsorModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using RDCEL.DocUpload.DataContract.ProductsPrices;
using RDCEL.DocUpload.DataContract.MasterModel;
using GraspCorn.Common.Enums;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DAL;
using GraspCorn.Common.Helper;
using RDCEL.DocUpload.DataContract.UniversalPricemasterDetails;
using RDCEL.DocUpload.BAL.OldProductDetailsManager;

namespace RDCEL.DocUpload.Web.API.Controllers.api
{
    [Authorize]
    public class MasterController : BaseApiController
    {
        #region Variable declaration
        MasterManager _masterManager;
        IsDtoCController _isDtoCController;
        Controllers.ABBController _abbcontroller;
        ExchangeController _exchangeController;
        BusinessUnitRepository businessUnitRepository;
        LoginDetailsUTCRepository _loginRepository;
        OldProductDetailsManager _oldproductdetailsmanager;
        BusinessPartnerRepository _businessPartnerrepository;
        BrandRepository _brandRepository;
        #endregion

        [HttpGet]
        public HttpResponseMessage GetTest()
        {
            HttpResponseMessage response = null;
            StatusDataContract structObj = new StatusDataContract(true, "API Working..", null);
            response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
            };
            return response;
        }

        #region Get all Product Category detail
        [HttpGet]
        public HttpResponseMessage GetProductCategory(string StoreCode = null, int? NewBrandId = 0)
        {
            _masterManager = new MasterManager();
            _loginRepository = new LoginDetailsUTCRepository();
            _oldproductdetailsmanager = new OldProductDetailsManager();
            _businessPartnerrepository = new BusinessPartnerRepository();
            Login loginObj = null;
            int BUId = 0;
            HttpResponseMessage response = null;
            ProductCategoryDataContract productCategoryDC = new ProductCategoryDataContract();
            PriceMasterNameDataContract pricemastermappingobj = new PriceMasterNameDataContract();
            PriceMasterMappingDataContract pricemasterdetailsobj = new PriceMasterMappingDataContract();
            tblBusinessPartner businessPartnerObj = null;
            List<tblProductCategory> productCategory = new List<tblProductCategory>();
            List<ProductCategory> productCategoryList = null;
            try
            {

                if (StoreCode == "null" || StoreCode == "Null")
                {
                    StoreCode = null;
                }
                string userName = string.Empty;
                if (HttpContext.Current != null && HttpContext.Current.User != null
                        && HttpContext.Current.User.Identity.Name != null)
                {
                    userName = HttpContext.Current.User.Identity.Name;

                    loginObj = _loginRepository.GetSingle(x => !string.IsNullOrEmpty(x.username) && x.username.ToLower().Equals(userName.ToLower()));
                    if (loginObj != null)
                    {

                        BUId = loginObj.SponsorId != null ? Convert.ToInt32(loginObj.SponsorId) : 0;
                        if (string.IsNullOrEmpty(StoreCode))
                        {
                            if (loginObj.BusinessPartnerId != null)
                            {
                                pricemasterdetailsobj.BusinessunitId = loginObj.SponsorId != null ? (loginObj.SponsorId) : 0;
                                pricemasterdetailsobj.BusinessPartnerId = loginObj.BusinessPartnerId != null ? (loginObj.BusinessPartnerId) : 0;
                                pricemasterdetailsobj.NewBrandId = NewBrandId > 0 ? (NewBrandId) : 0;


                                pricemastermappingobj = _oldproductdetailsmanager.GetPriceNameId(pricemasterdetailsobj);
                                if (pricemastermappingobj.PriceNameId > 0)
                                {
                                    productCategory = _oldproductdetailsmanager.GetProductCatListByPriceMasterNameId(pricemastermappingobj.PriceNameId);
                                    if (productCategory != null && productCategory.Count > 0)
                                    {

                                        productCategoryList = GenericMapper<tblProductCategory, ProductCategory>.MapList(productCategory);
                                        productCategoryDC.ProductsCategory = productCategoryList;

                                    }
                                    if (productCategoryDC != null)
                                    {
                                        StatusDataContract structObj = new StatusDataContract(true, "Success", productCategoryDC);
                                        response = new HttpResponseMessage(HttpStatusCode.OK)
                                        {
                                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                        };
                                    }
                                    else
                                    {
                                        StatusDataContract structObj = new StatusDataContract(true, "No data found");
                                        response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                        {
                                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                        };

                                    }
                                }
                                else
                                {
                                    StatusDataContract structObj = new StatusDataContract(true, pricemastermappingobj.ErrorMessage);
                                    response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                    {
                                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                    };
                                }
                            }
                            else
                            {
                                StatusDataContract structObj = new StatusDataContract(true, "You have to provide Store Code because it is multi store Sponsor");
                                response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                {
                                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                };
                            }

                        }
                        else
                        {
                            businessPartnerObj = _businessPartnerrepository.GetSingle(x => x.IsActive == true && x.StoreCode == StoreCode);
                            if (businessPartnerObj != null)
                            {
                                pricemasterdetailsobj.BusinessunitId = loginObj.SponsorId != null ? (loginObj.SponsorId) : 0;
                                pricemasterdetailsobj.BusinessPartnerId = businessPartnerObj.BusinessPartnerId > 0 ? (businessPartnerObj.BusinessPartnerId) : 0;
                                pricemasterdetailsobj.NewBrandId = NewBrandId > 0 ? (NewBrandId) : 0;

                                pricemastermappingobj = _oldproductdetailsmanager.GetPriceNameId(pricemasterdetailsobj);
                                if (pricemastermappingobj.PriceNameId > 0)
                                {
                                    productCategory = _oldproductdetailsmanager.GetProductCatListByPriceMasterNameId(pricemastermappingobj.PriceNameId);
                                    if (productCategory != null && productCategory.Count > 0)
                                    {

                                        productCategoryList = GenericMapper<tblProductCategory, ProductCategory>.MapList(productCategory);

                                        productCategoryDC.ProductsCategory = productCategoryList;

                                    }
                                    if (productCategoryDC != null)
                                    {
                                        StatusDataContract structObj = new StatusDataContract(true, "Success", productCategoryDC);
                                        response = new HttpResponseMessage(HttpStatusCode.OK)
                                        {
                                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                        };
                                    }
                                    else
                                    {
                                        StatusDataContract structObj = new StatusDataContract(true, "No data found");
                                        response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                        {
                                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                        };

                                    }
                                }
                                else
                                {
                                    StatusDataContract structObj = new StatusDataContract(true, pricemastermappingobj.ErrorMessage);
                                    response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                    {
                                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                    };
                                }
                            }
                            else
                            {
                                StatusDataContract structObj = new StatusDataContract(true, "No store found for the store code provided");
                                response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                {
                                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                };
                            }
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("Master", "GetProductCategory", ex);
                StatusDataContract structObj = new StatusDataContract(false, ex.Message);
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json")) //new StringContent("error"),                    
                };
            }


            return response;

        }

        #endregion

        #region Get all Product Type detail
        [HttpGet]
        public HttpResponseMessage GetProductType(int? catId = 0, int? NewBrandId = 0, string StoreCode = null)
        {
            _loginRepository = new LoginDetailsUTCRepository();
            _oldproductdetailsmanager = new OldProductDetailsManager();
            _businessPartnerrepository = new BusinessPartnerRepository();
            _masterManager = new MasterManager();
            Login loginObj = null;
            HttpResponseMessage response = null;
            ProductTypeDataContract productTypeDC = new ProductTypeDataContract();
            List<tblProductType> ProductTypeList = new List<tblProductType>();
            PriceMasterNameDataContract pricemastermappingobj = new PriceMasterNameDataContract();
            PriceMasterMappingDataContract pricemasterdetailsobj = new PriceMasterMappingDataContract();
            tblBusinessPartner businessPartnerObj = null;
            ProductTypeDataContract ProductTypeNew = new ProductTypeDataContract();
            ProductTypeNew.ProductsType = new List<ProductType>();
            try
            {
                {
                    StoreCode = null;
                }
                string username = string.Empty;
                try
                {
                    username = RequestContext.Principal.Identity.Name;
                    loginObj = _loginRepository.GetSingle(x => !string.IsNullOrEmpty(x.username) && x.username.ToLower().Equals(username.ToLower()));
                    if (loginObj != null)
                    {
                        if (string.IsNullOrEmpty(StoreCode))
                        {
                            if (loginObj.BusinessPartnerId != null)
                            {
                                pricemasterdetailsobj.BusinessunitId = loginObj.SponsorId > 0 ? loginObj.SponsorId : 0;
                                pricemasterdetailsobj.BusinessPartnerId = loginObj.BusinessPartnerId > 0 ? loginObj.BusinessPartnerId : 0;
                                pricemasterdetailsobj.NewBrandId = NewBrandId > 0 ? NewBrandId : 0;
                                pricemastermappingobj = _oldproductdetailsmanager.GetPriceNameId(pricemasterdetailsobj);
                                if (pricemastermappingobj.PriceNameId > 0)
                                {

                                    ProductTypeList = _oldproductdetailsmanager.GetProTypeListByPriceMasterNameId(pricemastermappingobj.PriceNameId, Convert.ToInt32(catId));
                                    if (ProductTypeList != null && ProductTypeList.Count > 0)
                                    {
                                        //start new code
                                        foreach (var item in ProductTypeList)
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
                                    }
                                    if (ProductTypeNew != null)
                                    {
                                        StatusDataContract structObj = new StatusDataContract(true, "Success", ProductTypeNew);
                                        response = new HttpResponseMessage(HttpStatusCode.OK)
                                        {
                                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                        };
                                    }
                                    else
                                    {
                                        StatusDataContract structObj = new StatusDataContract(true, "No data found");
                                        response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                        {
                                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                        };

                                    }
                                }
                                else
                                {
                                    StatusDataContract structObj = new StatusDataContract(true, pricemastermappingobj.ErrorMessage);
                                    response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                    {
                                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                    };
                                }
                            }
                            else
                            {
                                StatusDataContract structObj = new StatusDataContract(true, "You have to provide Store Code because it is multi store Sponsor");
                                response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                {
                                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                };
                            }
                        }
                        else
                        {
                            businessPartnerObj = _businessPartnerrepository.GetSingle(x => x.IsActive == true && x.StoreCode == StoreCode);
                            if (businessPartnerObj != null)
                            {
                                pricemasterdetailsobj.BusinessunitId = loginObj.SponsorId != null ? (loginObj.SponsorId) : 0;
                                pricemasterdetailsobj.BusinessPartnerId = businessPartnerObj.BusinessPartnerId > 0 ? (businessPartnerObj.BusinessPartnerId) : 0;
                                pricemasterdetailsobj.NewBrandId = NewBrandId > 0 ? (NewBrandId) : 0;
                                pricemastermappingobj = _oldproductdetailsmanager.GetPriceNameId(pricemasterdetailsobj);
                                if (pricemastermappingobj.PriceNameId > 0)
                                {
                                    ProductTypeList = _oldproductdetailsmanager.GetProTypeListByPriceMasterNameId(pricemastermappingobj.PriceNameId, Convert.ToInt32(catId));
                                    if (ProductTypeList.Count > 0)
                                    {
                                        //start new code
                                        foreach (var item in ProductTypeList)
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
                                    }

                                    if (ProductTypeNew != null)
                                    {
                                        StatusDataContract structObj = new StatusDataContract(true, "Success", ProductTypeNew);
                                        response = new HttpResponseMessage(HttpStatusCode.OK)
                                        {
                                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                        };
                                    }
                                    else
                                    {
                                        StatusDataContract structObj = new StatusDataContract(true, "No data found");
                                        response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                        {
                                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                        };

                                    }
                                }
                                else
                                {
                                    StatusDataContract structObj = new StatusDataContract(true, pricemastermappingobj.ErrorMessage);
                                    response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                    {
                                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                    };
                                }
                            }
                            else
                            {
                                StatusDataContract structObj = new StatusDataContract(true, "No store found for provided store code");
                                response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                {
                                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                };
                            }
                        }
                    }
                    else
                    {
                        StatusDataContract structObj = new StatusDataContract(true, "User credentials are not valid");
                        response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                        {
                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                        };
                    }
                }
                catch (Exception ex1)
                {
                    StatusDataContract structObj = new StatusDataContract(false, ex1.Message);
                    response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json")) //new StringContent("error"),                    
                    };
                }

                productTypeDC = _masterManager.GetProductType(username, Convert.ToInt32(catId));



            }
            catch (Exception ex)
            {
                StatusDataContract structObj = new StatusDataContract(false, ex.Message);
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json")) //new StringContent("error"),                    
                };
            }


            return response;

        }

        #endregion

        #region Get all Brand detail
        [HttpGet]
        public HttpResponseMessage GetBrand(int? catId = 0, int? typeId = 0, int? NewBrandId = 0, string StoreCode = null)
        {

            _masterManager = new MasterManager();
            HttpResponseMessage response = null;
            BrandDataContract brandDC = new BrandDataContract();
            businessUnitRepository = new BusinessUnitRepository();
            _loginRepository = new LoginDetailsUTCRepository();
            _oldproductdetailsmanager = new OldProductDetailsManager();
            _businessPartnerrepository = new BusinessPartnerRepository();
            tblBusinessUnit businessUnit = null;
            tblBusinessPartner businessPartnerObj = null;
            string username = string.Empty;
            Login loginObj = null;
            PriceMasterNameDataContract pricemastermappingobj = new PriceMasterNameDataContract();
            PriceMasterMappingDataContract pricemasterdetailsobj = new PriceMasterMappingDataContract();
            ProductDetailsForOldDataContract oldprodctDC = new ProductDetailsForOldDataContract();
            List<BrandName> BrandDC = new List<BrandName>();
            try
            {
                if (StoreCode == "null" || StoreCode == "Null")
                {
                    StoreCode = null;
                }
                username = RequestContext.Principal.Identity.Name;
                loginObj = _loginRepository.GetSingle(x => !string.IsNullOrEmpty(x.username) && x.username.ToLower().Equals(username.ToLower()));
                if (loginObj != null)
                {
                    if (string.IsNullOrEmpty(StoreCode))
                    {
                        if (loginObj.BusinessPartnerId != null)
                        {
                            pricemasterdetailsobj.BusinessunitId = loginObj.SponsorId > 0 ? loginObj.SponsorId : 0;
                            pricemasterdetailsobj.BusinessPartnerId = loginObj.BusinessPartnerId > 0 ? loginObj.BusinessPartnerId : 0;
                            pricemasterdetailsobj.NewBrandId = NewBrandId > 0 ? NewBrandId : 0;
                            pricemastermappingobj = _oldproductdetailsmanager.GetPriceNameId(pricemasterdetailsobj);
                            if (pricemastermappingobj.PriceNameId > 0)
                            {
                                if (catId == 0)
                                {

                                    BrandDC = _oldproductdetailsmanager.GetAllBrandOldByPriceMasterId(pricemastermappingobj);
                                    brandDC.Brand = BrandDC;
                                }
                                else if (catId != 0 && pricemasterdetailsobj.BusinessunitId > 0)
                                {
                                    businessUnit = businessUnitRepository.GetSingle(x => x.BusinessUnitId == Convert.ToInt32(pricemasterdetailsobj.BusinessunitId));
                                    if (businessUnit != null)
                                    {
                                        oldprodctDC.OldProductcategoryId = catId;
                                        oldprodctDC.OldProductTypeId = typeId;
                                        oldprodctDC.PriceMasterNameId = pricemastermappingobj.PriceNameId;
                                        BrandDC = _oldproductdetailsmanager.GetBrandOldByPriceMasterId(oldprodctDC);
                                        brandDC.Brand = BrandDC;
                                    }
                                }
                                if (brandDC != null)
                                {
                                    StatusDataContract structObj = new StatusDataContract(true, "Success", brandDC);
                                    response = new HttpResponseMessage(HttpStatusCode.OK)
                                    {
                                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                    };
                                }
                                else
                                {
                                    StatusDataContract structObj = new StatusDataContract(true, "No data found");
                                    response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                    {
                                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                    };
                                }
                            }
                            else
                            {
                                StatusDataContract structObj = new StatusDataContract(true, pricemastermappingobj.ErrorMessage);
                                response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                {
                                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                };
                            }
                        }
                        else
                        {
                            StatusDataContract structObj = new StatusDataContract(true, "You have to provide Store Code because it is multi store Sponsor");
                            response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                            {
                                Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                            };
                        }
                    }
                    else
                    {
                        businessPartnerObj = _businessPartnerrepository.GetSingle(x => x.IsActive == true && x.StoreCode == StoreCode);
                        if (businessPartnerObj != null)
                        {
                            pricemasterdetailsobj.BusinessunitId = loginObj.SponsorId != null ? (loginObj.SponsorId) : 0;
                            pricemasterdetailsobj.BusinessPartnerId = businessPartnerObj.BusinessPartnerId > 0 ? (businessPartnerObj.BusinessPartnerId) : 0;
                            pricemasterdetailsobj.NewBrandId = NewBrandId > 0 ? (NewBrandId) : 0;
                            pricemastermappingobj = _oldproductdetailsmanager.GetPriceNameId(pricemasterdetailsobj);
                            if (pricemastermappingobj.PriceNameId > 0)
                            {
                                if (catId == 0)
                                {

                                    BrandDC = _oldproductdetailsmanager.GetAllBrandOldByPriceMasterId(pricemastermappingobj);
                                    brandDC.Brand = BrandDC;
                                }
                                else if (catId != 0 && pricemasterdetailsobj.BusinessunitId > 0)
                                {
                                    businessUnit = businessUnitRepository.GetSingle(x => x.BusinessUnitId == Convert.ToInt32(pricemasterdetailsobj.BusinessunitId));
                                    if (businessUnit != null)
                                    {
                                        oldprodctDC.OldProductcategoryId = catId;
                                        oldprodctDC.OldProductTypeId = typeId;
                                        oldprodctDC.PriceMasterNameId = pricemastermappingobj.PriceNameId;
                                        BrandDC = _oldproductdetailsmanager.GetBrandOldByPriceMasterId(oldprodctDC);
                                        brandDC.Brand = BrandDC;
                                    }
                                }
                                if (brandDC != null)
                                {
                                    StatusDataContract structObj = new StatusDataContract(true, "Success", brandDC);
                                    response = new HttpResponseMessage(HttpStatusCode.OK)
                                    {
                                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                    };
                                }
                                else
                                {
                                    StatusDataContract structObj = new StatusDataContract(true, "No data found");
                                    response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                    {
                                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                    };
                                }
                            }
                            else
                            {
                                StatusDataContract structObj = new StatusDataContract(true, pricemastermappingobj.ErrorMessage);
                                response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                {
                                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                };
                            }
                        }
                        else
                        {
                            StatusDataContract structObj = new StatusDataContract(true, "No store found for provided store code");
                            response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                            {
                                Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                            };
                        }
                    }
                }
                else
                {
                    StatusDataContract structObj = new StatusDataContract(true, "User credentials are not valid");
                    response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                }

            }
            catch (Exception ex)
            {
                StatusDataContract structObj = new StatusDataContract(false, ex.Message);
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json")) //new StringContent("error"),                    
                };
            }
            return response;
        }
        #endregion

        #region Website enabled

        //Api for get Product Type by Product Category Id
        [HttpGet]
        public HttpResponseMessage GetProductTypeByCategoryId(int productCatId)
        {
            _masterManager = new MasterManager();
            HttpResponseMessage response = null;
            ProductTypeDataContract productTypeDC = null;
            try
            {


                productTypeDC = _masterManager.GetProductTypeByProductCatId(productCatId);

                if (productTypeDC != null)
                {
                    StatusDataContract structObj = new StatusDataContract(true, "Success", productTypeDC);
                    response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                }
                else
                {
                    StatusDataContract structObj = new StatusDataContract(true, "No data found");
                    response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };

                }

            }
            catch (Exception ex)
            {
                StatusDataContract structObj = new StatusDataContract(false, ex.Message);
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json")) //new StringContent("error"),                    
                };
            }

            return response;
        }

        // Api for Get Pincode dropdown list according to text param
        [HttpGet]
        public HttpResponseMessage GetPincode(string pintext)
        {
            _isDtoCController = new IsDtoCController();
            System.Web.Mvc.JsonResult _jsonResult = null;
            HttpResponseMessage response = null;
            string userName = string.Empty;
            Login loginObj = null;
            int buid = 0;
            _loginRepository = new LoginDetailsUTCRepository();
            try
            {
                if (HttpContext.Current != null && HttpContext.Current.User != null
                                       && HttpContext.Current.User.Identity.Name != null)
                {
                    userName = HttpContext.Current.User.Identity.Name;
                    loginObj = _loginRepository.GetSingle(x => !string.IsNullOrEmpty(x.username) && x.username.ToLower().Equals(userName.ToLower()));
                    if (loginObj != null)
                    {
                        buid = loginObj.SponsorId != null ? Convert.ToInt32(loginObj.SponsorId) : 0;
                        _jsonResult = _isDtoCController.GetPincodeForMyGate(pintext, buid);

                        if (_jsonResult != null)
                        {
                            StatusDataContract structObj = new StatusDataContract(true, "Success", _jsonResult);
                            response = new HttpResponseMessage(HttpStatusCode.OK)
                            {
                                Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                            };
                        }
                        else
                        {
                            StatusDataContract structObj = new StatusDataContract(true, "No data found");
                            response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                            {
                                Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                StatusDataContract structObj = new StatusDataContract(false, ex.Message);
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json")) //new StringContent("error"),                    
                };
            }
            return response;
        }

        // Api for Get State and city by pincode
        [HttpGet]
        public HttpResponseMessage GetStateAndCityByPincode(string pintext)
        {
            _isDtoCController = new IsDtoCController();
            System.Web.Mvc.JsonResult _jsonResult = null;
            HttpResponseMessage response = null;
            try
            {
                _jsonResult = _isDtoCController.GetState(pintext);

                if (_jsonResult != null)
                {
                    StatusDataContract structObj = new StatusDataContract(true, "Success", _jsonResult);
                    response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                }
                else
                {
                    StatusDataContract structObj = new StatusDataContract(true, "No data found");
                    response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                }
            }
            catch (Exception ex)
            {
                StatusDataContract structObj = new StatusDataContract(false, ex.Message);
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json")) //new StringContent("error"),                    
                };
            }

            return response;
        }

        #endregion

        #region Api for SendOTP and VeryfyOtp

        [HttpGet]
        public HttpResponseMessage SendOTP(string mobileNum)
        {
            HttpResponseMessage response = null;
            Login loginObj = null;
            _loginRepository = new LoginDetailsUTCRepository();
            System.Web.Mvc.JsonResult _jsonResult = null;
            _exchangeController = new ExchangeController();
            bool result = false;
            int buid = 0;
            try
            {
                string userName = string.Empty;

                if (HttpContext.Current != null && HttpContext.Current.User != null
                        && HttpContext.Current.User.Identity.Name != null)
                {
                    userName = HttpContext.Current.User.Identity.Name;

                    loginObj = _loginRepository.GetSingle(x => !string.IsNullOrEmpty(x.username) && x.username.ToLower().Equals(userName.ToLower()));
                    if (loginObj != null && loginObj.BusinessPartnerId != null)
                    {
                        buid = loginObj.SponsorId != null ? Convert.ToInt32(loginObj.SponsorId) : 0;
                    }
                }
                if (buid > 0)
                {
                    _jsonResult = _exchangeController.SendOTP(mobileNum, buid);
                    result = (bool)_jsonResult.Data;

                }
                if (result && _jsonResult != null)
                {
                    StatusDataContract structObj = new StatusDataContract(true, "OTP Send Successfully", _jsonResult);
                    response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                }
                else
                {
                    StatusDataContract structObj = new StatusDataContract(false, "OTP not Send", _jsonResult);
                    response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("MasterController", "SendOTP", ex);
                StatusDataContract structObj = new StatusDataContract(false, ex.Message);
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json")) //new StringContent("error"),                    
                };
            }


            return response;

        }

        [HttpGet]
        public HttpResponseMessage VerifyOTP(string mobileNum, string OTP)
        {
            HttpResponseMessage response = null;
            _loginRepository = new LoginDetailsUTCRepository();
            System.Web.Mvc.JsonResult _jsonResult = null;
            _exchangeController = new ExchangeController();
            bool result = false;
            try
            {
                _jsonResult = _exchangeController.VerifyOTP(mobileNum, OTP);
                result = (bool)_jsonResult.Data;

                if (result && _jsonResult != null)
                {
                    StatusDataContract structObj = new StatusDataContract(true, "OTP is valid", _jsonResult);
                    response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                }
                else
                {
                    StatusDataContract structObj = new StatusDataContract(false, "OTP not valid", _jsonResult);
                    response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("MasterController", "SendOTP", ex);
                StatusDataContract structObj = new StatusDataContract(false, ex.Message);
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json")) //new StringContent("error"),                    
                };
            }


            return response;

        }

        #endregion

        // Api for Get Pincode dropdown list according to text param for ABB
        [HttpGet]
        public HttpResponseMessage GetPincodeABB(string pintext)
        {
            _abbcontroller = new Controllers.ABBController();
            System.Web.Mvc.JsonResult _jsonResult = null;
            HttpResponseMessage response = null;
            int? buid = 0;
            string userName = string.Empty;
            Login loginObj = null;
            _loginRepository = new LoginDetailsUTCRepository();
            try
            {
                if (HttpContext.Current != null && HttpContext.Current.User != null
                                      && HttpContext.Current.User.Identity.Name != null)
                {
                    userName = HttpContext.Current.User.Identity.Name;
                    loginObj = _loginRepository.GetSingle(x => !string.IsNullOrEmpty(x.username) && x.username.ToLower().Equals(userName.ToLower()));
                    if (loginObj != null && loginObj.BusinessPartnerId != null)
                    {
                        buid = loginObj.SponsorId != null ? Convert.ToInt32(loginObj.SponsorId) : 0;
                        _jsonResult = _abbcontroller.GetPincodeForABB(pintext, buid);
                        if (_jsonResult != null)
                        {
                            StatusDataContract structObj = new StatusDataContract(true, "Success", _jsonResult);
                            response = new HttpResponseMessage(HttpStatusCode.OK)
                            {
                                Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                            };
                        }
                        else
                        {
                            StatusDataContract structObj = new StatusDataContract(true, "No data found");
                            response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                            {
                                Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                            };
                        }
                    }
                }

               
            }
            catch (Exception ex)
            {
                StatusDataContract structObj = new StatusDataContract(false, ex.Message);
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json")) //new StringContent("error"),                    
                };
            }
            return response;
        }

        #region New brand for exchange
        [HttpGet]
        public HttpResponseMessage GetNewBrand(string StoreCode = null)
        {
            _loginRepository = new LoginDetailsUTCRepository();
            _businessPartnerrepository = new BusinessPartnerRepository();
            HttpResponseMessage response = null;
            Login loginObj = null;
            tblBusinessPartner businessPartnerObj = null;
            string username = string.Empty;
            NewBrandDataContract newbrandDC = new NewBrandDataContract();
            ExchangeOrderManager exchangemanager = new ExchangeOrderManager();
            GetNewbrandDC getNewBrand = new GetNewbrandDC();
            try
            {
                if (StoreCode == "null" || StoreCode == "Null")
                {
                    StoreCode = null;
                }
                username = RequestContext.Principal.Identity.Name;
                loginObj = _loginRepository.GetSingle(x => !string.IsNullOrEmpty(x.username) && x.username.ToLower().Equals(username.ToLower()));
                if (loginObj != null && loginObj.BusinessPartnerId != null)
                {
                    if (string.IsNullOrEmpty(StoreCode))
                    {
                        getNewBrand.BusinessUnitId = loginObj.SponsorId != null ? loginObj.SponsorId : 0;
                        getNewBrand.BusinessPartnerId = loginObj.BusinessPartnerId != null ? loginObj.BusinessPartnerId : 0;
                        newbrandDC = exchangemanager.getNewBrandsForexchnge(getNewBrand);
                        if (newbrandDC != null && newbrandDC.brandlist.Count > 0)
                        {
                            StatusDataContract structObj = new StatusDataContract(true, "Success", newbrandDC);
                            response = new HttpResponseMessage(HttpStatusCode.OK)
                            {
                                Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                            };
                        }
                        else
                        {
                            StatusDataContract structObj = new StatusDataContract(true, "No store found with this store code");
                            response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                            {
                                Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                            };
                        }
                    }
                    else
                    {
                        businessPartnerObj = _businessPartnerrepository.GetSingle(x => x.IsActive == true && x.StoreCode == StoreCode && x.IsExchangeBP == true);
                        if (businessPartnerObj != null)
                        {
                            getNewBrand.BusinessUnitId = businessPartnerObj.BusinessUnitId != null ? businessPartnerObj.BusinessUnitId : 0;
                            getNewBrand.BusinessPartnerId = businessPartnerObj.BusinessPartnerId > 0 ? businessPartnerObj.BusinessPartnerId : 0;
                            newbrandDC = exchangemanager.getNewBrandsForexchnge(getNewBrand);
                            if (newbrandDC != null && newbrandDC.brandlist.Count > 0)
                            {
                                StatusDataContract structObj = new StatusDataContract(true, "Success", newbrandDC);
                                response = new HttpResponseMessage(HttpStatusCode.OK)
                                {
                                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                };
                            }
                            else
                            {
                                StatusDataContract structObj = new StatusDataContract(true, "No store found with this store code");
                                response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                {
                                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                };
                            }
                        }
                        else
                        {
                            StatusDataContract structObj = new StatusDataContract(true, "No store found with this store code");
                            response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                            {
                                Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                            };
                        }
                    }
                }
                else
                {
                    StatusDataContract structObj = new StatusDataContract(true, "User credentials are not valid");
                    response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                }
            }
            catch (Exception ex)
            {
                StatusDataContract structObj = new StatusDataContract(false, ex.Message);
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json")) //new StringContent("error"),                    
                };
            }
            return response;
        }
        #endregion

        #region New Category Id
        [HttpGet]
        public HttpResponseMessage GetNewProductCategory()
        {
            _loginRepository = new LoginDetailsUTCRepository();
            _businessPartnerrepository = new BusinessPartnerRepository();
            HttpResponseMessage response = null;
            Login loginObj = null;
            tblBusinessPartner businessPartnerObj = null;
            string username = string.Empty;
            NewBrandDataContract newbrandDC = new NewBrandDataContract();
            ExchangeOrderManager exchangemanager = new ExchangeOrderManager();
            ProductCategoryDataContract productCategoryDC = new ProductCategoryDataContract();
            int BUId = 0;
            List<tblProductCategory> productCategory = new List<tblProductCategory>();
            List<ProductCategory> productCategoryList = null;
            _oldproductdetailsmanager = new OldProductDetailsManager();
            try
            {
                username = RequestContext.Principal.Identity.Name;
                loginObj = _loginRepository.GetSingle(x => !string.IsNullOrEmpty(x.username) && x.username.ToLower().Equals(username.ToLower()));
                if (loginObj != null)
                {
                    BUId = loginObj.SponsorId != null ? Convert.ToInt32(loginObj.SponsorId) : 0;
                    if (BUId > 0)
                    {
                        productCategory = _oldproductdetailsmanager.GetProductCatListByBUId(BUId);
                        if (productCategory != null && productCategory.Count > 0)
                        {
                            productCategoryList = GenericMapper<tblProductCategory, ProductCategory>.MapList(productCategory);
                            productCategoryDC.ProductsCategory = productCategoryList;
                        }
                        if (productCategoryDC.ProductsCategory != null)
                        {
                            StatusDataContract structObj = new StatusDataContract(true, "Success", productCategoryDC);
                            response = new HttpResponseMessage(HttpStatusCode.OK)
                            {
                                Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                            };
                        }
                        else
                        {
                            StatusDataContract structObj = new StatusDataContract(true, "No data found");
                            response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                            {
                                Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                            };
                        }
                    }
                    else
                    {
                        StatusDataContract structObj = new StatusDataContract(true, "User credentials are not valid");
                        response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                        {
                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                        };
                    }
                }

                else
                {
                    StatusDataContract structObj = new StatusDataContract(true, "User credentials are not valid");
                    response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("Master", "GetNewProductCategory", ex);
                StatusDataContract structObj = new StatusDataContract(false, ex.Message);
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json")) //new StringContent("error"),                    
                };
            }
            return response;
        }
        #endregion

        #region New Product Type Id
        [HttpGet]
        public HttpResponseMessage GetNewProductType(int catId = 0)
        {
            HttpResponseMessage response = null;
            string username = string.Empty;
            Login loginObj = null;
            _loginRepository = new LoginDetailsUTCRepository();
            int BUId = 0;
            List<tblProductType> productType = new List<tblProductType>();
            List<ProductType> productTypeList = null;
            ProductTypeDataContract productTypeDC = new ProductTypeDataContract();
            _oldproductdetailsmanager = new OldProductDetailsManager();
            try
            {
                username = RequestContext.Principal.Identity.Name;
                loginObj = _loginRepository.GetSingle(x => !string.IsNullOrEmpty(x.username) && x.username.ToLower().Equals(username.ToLower()));
                if (loginObj != null)
                {
                    BUId = loginObj.SponsorId != null ? Convert.ToInt32(loginObj.SponsorId) : 0;
                    if (BUId > 0)
                    {
                        productType = _oldproductdetailsmanager.GetProductTypeListByBUId(BUId, catId);
                        if (productType != null && productType.Count > 0)
                        {
                            productTypeList = GenericMapper<tblProductType, ProductType>.MapList(productType);
                            productTypeDC.ProductsType = productTypeList;
                        }
                        if (productTypeDC.ProductsType != null)
                        {
                            StatusDataContract structObj = new StatusDataContract(true, "Success", productTypeDC);
                            response = new HttpResponseMessage(HttpStatusCode.OK)
                            {
                                Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                            };
                        }
                        else
                        {
                            StatusDataContract structObj = new StatusDataContract(true, "No data found");
                            response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                            {
                                Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                            };
                        }
                    }
                    else
                    {
                        StatusDataContract structObj = new StatusDataContract(true, "User credentials are not valid");
                        response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                        {
                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                        };
                    }
                }
                else
                {
                    StatusDataContract structObj = new StatusDataContract(true, "User credentials are not valid");
                    response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("Master", "GetNewProductType", ex);
                StatusDataContract structObj = new StatusDataContract(false, ex.Message);
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json")) //new StringContent("error"),                    
                };
            }

            return response;
        }

        #endregion

        #region New Model Id
        [HttpGet]
        
        public HttpResponseMessage GetNewModelNumbers(int catId, int typeId, string StoreCode = null, int NewBrandId = 0)
        {
            HttpResponseMessage response = null;
            string username = string.Empty;
            Login loginObj = null;
            int BUId = 0;
            int BPId = 0;
            _oldproductdetailsmanager = new OldProductDetailsManager();
            _masterManager = new MasterManager();
            _loginRepository = new LoginDetailsUTCRepository();
            List<ModelNumberModel> modelNumberModels = null;
            ModelDetailsDataContract modelDetailsDataContract = new ModelDetailsDataContract();
            _brandRepository = new BrandRepository();
            tblBrand brandObj = null;
            tblBusinessPartner businessPartnerObj = null;
            List<ModalListdataDataContract> modalListdataDataContract = null;
            _businessPartnerrepository = new BusinessPartnerRepository();

            try
            {
                if (StoreCode == "null" || StoreCode == "Null")
                {
                    StoreCode = null;
                }

                username = RequestContext.Principal.Identity.Name;
                loginObj = _loginRepository.GetSingle(x => !string.IsNullOrEmpty(x.username) && x.username.ToLower().Equals(username.ToLower()));
                if (loginObj != null)
                {
                    BUId = loginObj.SponsorId != null ? Convert.ToInt32(loginObj.SponsorId) : 0;

                    if (string.IsNullOrEmpty(StoreCode))
                    {
                        if (loginObj.BusinessPartnerId != null)
                        {
                            brandObj = _brandRepository.GetSingle(x => x.BusinessUnitId == BUId && x.IsActive == true);
                            if (NewBrandId == 0 && brandObj != null)
                            {
                                NewBrandId = brandObj.Id;
                            }
                            BPId = loginObj.BusinessPartnerId != null ? Convert.ToInt32(loginObj.BusinessPartnerId) : 0;
                            NewBrandId = NewBrandId > 0 ? (NewBrandId) : 0;

                            if (NewBrandId > 0)
                            {
                                if (BUId > 0 && BPId > 0 && catId > 0 && typeId > 0)
                                {
                                    modelDetailsDataContract = _masterManager.GetModelList(BUId, BPId, catId, typeId, NewBrandId);
                                    if (modelDetailsDataContract != null && modelDetailsDataContract.ModelList != null && modelDetailsDataContract.ModelList.Count > 0)
                                    {
                                        modalListdataDataContract = modelDetailsDataContract.ModelList;
                                    }

                                    if (modalListdataDataContract != null)
                                    {
                                        StatusDataContract structObj = new StatusDataContract(true, "Success", modalListdataDataContract);
                                        response = new HttpResponseMessage(HttpStatusCode.OK)
                                        {
                                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                        };
                                    }
                                    else
                                    {
                                        StatusDataContract structObj = new StatusDataContract(true, "No data found");
                                        response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                        {
                                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                        };
                                    }
                                }
                                else
                                {
                                    StatusDataContract structObj = new StatusDataContract(true, "No Model Available");
                                    response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                    {
                                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                    };
                                }
                            }
                            else
                            {
                                StatusDataContract structObj = new StatusDataContract(true, "Brand Id not found, please provide brand Id");
                                response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                {
                                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                };
                            }
                        }
                        else
                        {
                            StatusDataContract structObj = new StatusDataContract(true, "You have to provide Store Code because it is multi store Sponsor");
                            response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                            {
                                Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                            };
                        }

                    }
                    else
                    {
                        businessPartnerObj = _businessPartnerrepository.GetSingle(x => x.IsActive == true && x.StoreCode == StoreCode);
                        if (businessPartnerObj != null)
                        {
                            brandObj = _brandRepository.GetSingle(x => x.BusinessUnitId == BUId && x.IsActive == true);
                            if (NewBrandId == 0 && brandObj != null)
                            {
                                NewBrandId = brandObj.Id;
                            }
                            BPId = businessPartnerObj.BusinessPartnerId > 0 ? (businessPartnerObj.BusinessPartnerId) : 0;
                            NewBrandId = NewBrandId > 0 ? (NewBrandId) : 0;

                            if (NewBrandId > 0)
                            {
                                if (BUId > 0 && BPId > 0 && catId > 0 && typeId > 0)
                                {
                                    modelDetailsDataContract = _masterManager.GetModelList(BUId, BPId, catId, typeId, NewBrandId);
                                    if (modelDetailsDataContract != null && modelDetailsDataContract.ModelList != null && modelDetailsDataContract.ModelList.Count > 0)
                                    {
                                        modalListdataDataContract = modelDetailsDataContract.ModelList;
                                    }

                                    if (modalListdataDataContract != null)
                                    {
                                        StatusDataContract structObj = new StatusDataContract(true, "Success", modalListdataDataContract);
                                        response = new HttpResponseMessage(HttpStatusCode.OK)
                                        {
                                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                        };
                                    }
                                    else
                                    {
                                        StatusDataContract structObj = new StatusDataContract(true, "No data found");
                                        response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                        {
                                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                        };
                                    }
                                }
                                else
                                {
                                    StatusDataContract structObj = new StatusDataContract(true, "No Model Available");
                                    response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                    {
                                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                    };
                                }
                            }
                            else
                            {
                                StatusDataContract structObj = new StatusDataContract(true, "Brand Id not found, please provide brand Id");
                                response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                {
                                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                };
                            }
                        }
                        else
                        {
                            StatusDataContract structObj = new StatusDataContract(true, "No store found for the store code provided");
                            response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                            {
                                Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                            };
                        }

                    }
                }
                else
                {
                    StatusDataContract structObj = new StatusDataContract(true, "User credentials are not valid");
                    response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("Master", "GetNewModelNumbers", ex);
                StatusDataContract structObj = new StatusDataContract(false, ex.Message);
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json")) //new StringContent("error"),                    
                };
            }

            return response;
        }
        #endregion
    }
}
