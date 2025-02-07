using RDCEL.DocUpload.DataContract.Base;
using RDCEL.DocUpload.DataContract.MasterModel;
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
using RDCEL.DocUpload.BAL.ZohoCreatorCall;
using RDCEL.DocUpload.DataContract.ZohoModel;
using RDCEL.DocUpload.BAL.SponsorsApiCall;
using RDCEL.DocUpload.DataContract.ProductsPrices;
using RDCEL.DocUpload.BAL.ProcessAPI;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DAL.Repository;
using GraspCorn.Common.Helper;
using Microsoft.AspNet.Identity;
using System.Data;
using RDCEL.DocUpload.DataContract.ExchangeOrderDetails;
using RDCEL.DocUpload.DataContract.Voucher;
using Newtonsoft.Json;
using RDCEL.DocUpload.DAL.Helper;
using RDCEL.DocUpload.DataContract.Bizlog;
using GraspCorn.Common.Enums;
using RDCEL.DocUpload.DataContract.Common;
using RDCEL.DocUpload.BAL.SweetenerManager;
using RDCEL.DocUpload.BAL.ExchangePriceMaster;
using RDCEL.DocUpload.DataContract.UniversalPricemasterDetails;
using RDCEL.DocUpload.BAL.OldProductDetailsManager;
using RDCEL.DocUpload.DataContract.OCRDataContract;

namespace RDCEL.DocUpload.Web.API.Controllers.api
{
    [Authorize]
    public class SponsorController : BaseApiController
    {
        #region Variable declaration
        ExchangeOrderManager _exchangeOrderManager;
        SponserManager _sponserManager;
        ExchangeOrderRepository _sponsorRepository;
        BusinessUnitRepository businessUnitRepository;
        BusinessPartnerRepository _businessPartnerRepository;
        LoginDetailsUTCRepository _loginRepository;
        ExchangeOrderRepository _exchangeOrderRepository;
        CustomerDetailsRepository _customerDetailsRepository;
        ProductCategoryRepository productCategoryRepository;
        ProductTypeRepository productTypeRepository;
        BrandRepository _brandRepository;
        DateTime currentDatetime = DateTime.Now.TrimMilliseconds();
        Logging logging;
        AbbRedemptionRepository _abbRedemptionRepository;
        ABBRegistrationRepository _abbRegistrationRepository;
        PinCodeRepository _picodeRepository;
        ManageSweetener _sweetenerManager;
        BAL.ExchangePriceMaster.PriceMasterManager priceMasterManager;
        OldProductDetailsManager _oldproductdetailsmanager;
        OrderBasedConfigurationRepository _orderBasedRepository;
        ProductConditionLabelRepository _productConditionLabel;
        BPBURedemptionMappingRepository _BPBURedemptionMappingRepository;
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

        #region Get all PinCode detail
        [HttpGet]
        public HttpResponseMessage GetZipCodes()
        {
            _exchangeOrderManager = new ExchangeOrderManager();
            HttpResponseMessage response = null;
            PinCodeDataContract pinCodeDC = null;
            string userName = string.Empty;
            Login loginObj = null;
            int buid = 0;
            _loginRepository = new LoginDetailsUTCRepository();
            System.Web.Mvc.JsonResult _jsonResult = null;
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
                        pinCodeDC = _exchangeOrderManager.GetZipCodes(buid);

                        if (pinCodeDC != null)
                        {
                            StatusDataContract structObj = new StatusDataContract(true, "Success", pinCodeDC);
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
                LibLogging.WriteErrorToDB("SponserController", "GetZipCodes", ex);
                StatusDataContract structObj = new StatusDataContract(false, ex.Message);
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json")) //new StringContent("error"),                    
                };
            }


            return response;

        }

        #endregion

        #region Get all Product Price
        [HttpGet]
        public HttpResponseMessage GetProductPrice(string categoryname = "", string catid = "", string typeid = "", string brandid = "", int? NewBrandId = 0, int? NewTypeId = 0, int? NewCatId = 0, int? ModelId = 0, string StoreCode = null)
        {
            _sweetenerManager = new ManageSweetener();
            priceMasterManager = new BAL.ExchangePriceMaster.PriceMasterManager();
            _exchangeOrderManager = new ExchangeOrderManager();
            _businessPartnerRepository = new BusinessPartnerRepository();
            businessUnitRepository = new BusinessUnitRepository();
            HttpResponseMessage response = null;
            _loginRepository = new LoginDetailsUTCRepository();
            _oldproductdetailsmanager = new OldProductDetailsManager();
            _orderBasedRepository = new OrderBasedConfigurationRepository();
            List<ProductsPricesDataContract> productsPricesList = new List<ProductsPricesDataContract>();
            List<ProductsPricesDataContractSamsung> productsPricesListsamsung = new List<ProductsPricesDataContractSamsung>();
            PriceMasterNameDataContract pricemastermappingobj = new PriceMasterNameDataContract();
            PriceMasterMappingDataContract pricemasterdetailsobj = new PriceMasterMappingDataContract();
            Login loginObj = null;
            tblBusinessPartner businessPartnerObj = null;
            tblOrderBasedConfig orderbasedConfig = null;
            tblBusinessUnit businessunitObj = null;
            string username = string.Empty;
            bool? IsSweetenerModelBased = false;
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
                        pricemasterdetailsobj.BusinessunitId = loginObj.SponsorId > 0 ? loginObj.SponsorId : 0;
                        pricemasterdetailsobj.BusinessPartnerId = loginObj.BusinessPartnerId > 0 ? loginObj.BusinessPartnerId : 0;
                        pricemasterdetailsobj.NewBrandId = NewBrandId > 0 ? NewBrandId : 0;
                        pricemastermappingobj = _oldproductdetailsmanager.GetPriceNameId(pricemasterdetailsobj);
                        if (pricemastermappingobj.PriceNameId > 0)
                        {
                            businessunitObj = businessUnitRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == loginObj.SponsorId);
                            if (businessunitObj != null)
                            {
                                orderbasedConfig = _orderBasedRepository.GetSingle(x => x.IsActive == true && x.BusinessPartnerId == pricemasterdetailsobj.BusinessPartnerId && x.BusinessUnitId == pricemasterdetailsobj.BusinessunitId);
                                if (orderbasedConfig != null)
                                {
                                    IsSweetenerModelBased = orderbasedConfig.IsSweetenerModalBased;
                                    if (businessunitObj.IsSweetenerIndependent == true)
                                    {
                                        productsPricesListsamsung = _exchangeOrderManager.GetProductPriceSamsung(username, IsSweetenerModelBased, pricemastermappingobj.PriceNameId, categoryname, catid, typeid, brandid, NewBrandId, NewCatId, NewTypeId, pricemasterdetailsobj.BusinessunitId, pricemasterdetailsobj.BusinessPartnerId, ModelId);
                                        //productsPriceList = _productTaxonomyManager.GetProductPrice();

                                        if (productsPricesListsamsung != null)
                                        {
                                            StatusDataContract structObj = new StatusDataContract(true, "Success", productsPricesListsamsung);
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
                                        productsPricesList = _exchangeOrderManager.GetProductPrice(username, IsSweetenerModelBased, pricemastermappingobj.PriceNameId, categoryname, catid, typeid, brandid, NewBrandId, NewCatId, NewTypeId, pricemasterdetailsobj.BusinessunitId, pricemasterdetailsobj.BusinessPartnerId, ModelId);
                                        //productsPriceList = _productTaxonomyManager.GetProductPrice();

                                        if (productsPricesList != null)
                                        {
                                            StatusDataContract structObj = new StatusDataContract(true, "Success", productsPricesList);
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
                                else
                                {
                                    StatusDataContract structObj = new StatusDataContract(true, "No data in order configuartion");
                                    response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                    {
                                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                    };
                                }
                            }
                            else
                            {
                                StatusDataContract structObj = new StatusDataContract(true, "Sponsor details not found");
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
                        businessPartnerObj = _businessPartnerRepository.GetSingle(x => x.IsActive == true && x.StoreCode == StoreCode);
                        if (businessPartnerObj != null)
                        {
                            pricemasterdetailsobj.BusinessunitId = loginObj.SponsorId != null ? (loginObj.SponsorId) : 0;
                            pricemasterdetailsobj.BusinessPartnerId = businessPartnerObj.BusinessPartnerId > 0 ? (businessPartnerObj.BusinessPartnerId) : 0;
                            pricemasterdetailsobj.NewBrandId = NewBrandId > 0 ? (NewBrandId) : 0;
                            pricemastermappingobj = _oldproductdetailsmanager.GetPriceNameId(pricemasterdetailsobj);
                            if (pricemastermappingobj.PriceNameId > 0)
                            {
                                businessunitObj = businessUnitRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == loginObj.SponsorId);
                                if (businessunitObj != null)
                                {

                                    orderbasedConfig = _orderBasedRepository.GetSingle(x => x.IsActive == true && x.BusinessPartnerId == pricemasterdetailsobj.BusinessPartnerId && x.BusinessUnitId == pricemasterdetailsobj.BusinessunitId);
                                    if (orderbasedConfig != null)
                                    {
                                        IsSweetenerModelBased = orderbasedConfig.IsSweetenerModalBased;
                                        if (businessunitObj.IsSweetenerIndependent == true)
                                        {
                                            productsPricesListsamsung = _exchangeOrderManager.GetProductPriceSamsung(username, IsSweetenerModelBased, pricemastermappingobj.PriceNameId, categoryname, catid, typeid, brandid, NewBrandId, NewCatId, NewTypeId, pricemasterdetailsobj.BusinessunitId, pricemasterdetailsobj.BusinessPartnerId, ModelId);
                                            //productsPriceList = _productTaxonomyManager.GetProductPrice();

                                            if (productsPricesListsamsung != null)
                                            {
                                                StatusDataContract structObj = new StatusDataContract(true, "Success", productsPricesListsamsung);
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
                                            productsPricesList = _exchangeOrderManager.GetProductPrice(username, IsSweetenerModelBased, pricemastermappingobj.PriceNameId, categoryname, catid, typeid, brandid, NewBrandId, NewCatId, NewTypeId, pricemasterdetailsobj.BusinessunitId, pricemasterdetailsobj.BusinessPartnerId, ModelId);
                                            //productsPriceList = _productTaxonomyManager.GetProductPrice();

                                            if (productsPricesList != null)
                                            {
                                                StatusDataContract structObj = new StatusDataContract(true, "Success", productsPricesList);
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
                                    else
                                    {
                                        StatusDataContract structObj = new StatusDataContract(true, "No data in order configuartion");
                                        response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                        {
                                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                        };
                                    }
                                }

                                else
                                {
                                    StatusDataContract structObj = new StatusDataContract(true, "Sponsor details not found");
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
                            StatusDataContract structObj = new StatusDataContract(true, "No store found with this storecode");
                            response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                            {
                                Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                            };
                        }
                    }
                }
                else
                {
                    StatusDataContract structObj = new StatusDataContract(true, "Login details are not proper");
                    response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                }


            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SponserController", "GetProductPrice", ex);
                StatusDataContract structObj = new StatusDataContract(false, ex.Message);
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json")) //new StringContent("error"),                    
                };
            }
            return response;
        }
        #endregion

        #region Place sponsor product exchange order detail

        [HttpPost]
        public HttpResponseMessage ProductOrderPlace(ProductOrderDataContract productOrderDataContract)
        {
            SponsrOrderSyncManager sponsrOrderSyncManager = new SponsrOrderSyncManager();
            HttpResponseMessage response = null;
            ProductOrderResponseDataContract productOrderResponseDC = null;
            priceMasterManager = new BAL.ExchangePriceMaster.PriceMasterManager();
            _sweetenerManager = new ManageSweetener();
            _sponsorRepository = new ExchangeOrderRepository();
            tblExchangeOrder SponserObj = null;
            tblBusinessUnit businessUnit = null;
            tblBusinessPartner businessPartnerObj = null;
            tblProductType tblProducttypeObj = null;
            Login loginObj = null;
            logging = new Logging();
            _businessPartnerRepository = new BusinessPartnerRepository();
            businessUnitRepository = new BusinessUnitRepository();
            _loginRepository = new LoginDetailsUTCRepository();
            _abbRedemptionRepository = new AbbRedemptionRepository();
            _picodeRepository = new PinCodeRepository();
            _orderBasedRepository = new OrderBasedConfigurationRepository();
            productTypeRepository = new ProductTypeRepository();
            _productConditionLabel = new ProductConditionLabelRepository();
            tblABBRedemption abbredemptionObj = null;
            tblPinCode pincodeObj = null;
            tblOrderBasedConfig orderBasedObj = null;
            tblProductConditionLabel productconditionobj = null;
            UniversalPriceMasterDataContract universalpricemasterDC = new UniversalPriceMasterDataContract();
            ProductPriceDetailsDataContract productPriceDetailsDataContract = new ProductPriceDetailsDataContract();
            GetSweetenerDetailsDataContract detailsforSweetenerDc = new GetSweetenerDetailsDataContract();
            SweetenerDataContract sweetener = new SweetenerDataContract();
            try
            {
                //Code to log order on every request made
                string ordrJson = JsonConvert.SerializeObject(productOrderDataContract);
                if (!string.IsNullOrEmpty(productOrderDataContract.SponsorOrderNumber))
                {
                    logging.WriteAPIRequestToDB("Sponsor", "ProductOrderPlace", productOrderDataContract.SponsorOrderNumber, ordrJson);
                }
                else
                {
                    logging.WriteAPIRequestToDB("Sponsor", "ProductOrderPlace", productOrderDataContract.PhoneNumber, ordrJson);
                }

                if (!ModelState.IsValid)
                {
                    StatusDataContract structObj = new StatusDataContract(false, "Model is not valid", ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList());
                    response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json")) //new StringContent("error"),                    
                    };
                }
                else
                {

                    string userName = string.Empty;

                    if (HttpContext.Current != null && HttpContext.Current.User != null
                            && HttpContext.Current.User.Identity.Name != null)
                    {
                        userName = HttpContext.Current.User.Identity.Name;

                        loginObj = _loginRepository.GetSingle(x => !string.IsNullOrEmpty(x.username) && x.username.ToLower().Equals(userName.ToLower()));
                        if (loginObj != null && loginObj.BusinessPartnerId != null)
                        {
                            productOrderDataContract.BusinessPartnerId = (int)loginObj.BusinessPartnerId;
                            productOrderDataContract.BUId = loginObj.SponsorId != null ? Convert.ToInt32(loginObj.SponsorId) : 0;
                        }

                        if (loginObj != null && loginObj.SponsorId != null)
                        {
                            businessUnit = businessUnitRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == loginObj.SponsorId);
                            if (businessUnit != null && productOrderDataContract.EstimatedDeliveryDate != null)
                            {
                                productOrderDataContract.EstimatedDeliveryDate = DateTime.Now.AddHours(Convert.ToInt32(businessUnit.ExpectedDeliveryHours)).ToString("dd-MMM-yyyy");
                            }
                            else if (businessUnit == null)
                            {
                                StatusDataContract structObj = new StatusDataContract(false, "Sponsor is not active", productOrderResponseDC);
                                response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                {
                                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                };

                                return response;
                            }
                        }
                    }
                    if (businessUnit != null)
                    {
                        if (string.IsNullOrEmpty(productOrderDataContract.StoreCode))
                        {
                            businessPartnerObj = _businessPartnerRepository.GetSingle(x => x.BusinessUnitId == businessUnit.BusinessUnitId
                                                        && x.IsActive.Equals(true)
                                                        && x.BusinessPartnerId.Equals(loginObj.BusinessPartnerId));

                            if (businessPartnerObj != null)
                            {
                                productOrderDataContract.BusinessPartnerId = businessPartnerObj.BusinessPartnerId;
                                productOrderDataContract.StoreCode = businessPartnerObj.StoreCode;
                                productOrderDataContract.IsRedemptionInstant = businessPartnerObj.IsRedemptionSettelemtInstant;
                                productOrderDataContract.IsVoucher = businessPartnerObj.IsVoucher == null ? false : Convert.ToBoolean(businessPartnerObj.IsVoucher);
                                productOrderDataContract.voucherType = businessPartnerObj.VoucherType == null ? 0 : Convert.ToInt32(businessPartnerObj.VoucherType);
                                productOrderDataContract.IsDefferedSettlement = businessPartnerObj.IsDefferedSettlement == null ? false : Convert.ToBoolean(businessPartnerObj.IsDefferedSettlement);
                            }
                            else
                            {
                                StatusDataContract structObj = new StatusDataContract(false, "No associated store found for this order", productOrderResponseDC);
                                response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                {
                                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                };

                                return response;
                            }


                            orderBasedObj = _orderBasedRepository.GetSingle(x => x.IsActive == true && x.BusinessPartnerId == productOrderDataContract.BusinessPartnerId && x.BusinessUnitId == productOrderDataContract.BUId);
                            if (orderBasedObj != null)
                            {
                                productOrderDataContract.IsSweetenerModelBased = orderBasedObj.IsSweetenerModalBased;
                            }
                            else
                            {
                                StatusDataContract structObj = new StatusDataContract(false, "No data found in order configuration table", productOrderResponseDC);
                                response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                {
                                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                };

                                return response;
                            }

                        }
                        else
                        {
                            businessPartnerObj = _businessPartnerRepository.GetSingle(x => x.BusinessUnitId == businessUnit.BusinessUnitId
                                                        && x.IsActive.Equals(true)
                                                        && (!string.IsNullOrEmpty(x.StoreCode) && x.StoreCode.Equals(productOrderDataContract.StoreCode)));
                            if (businessPartnerObj != null)
                            {
                                productOrderDataContract.BusinessPartnerId = businessPartnerObj.BusinessPartnerId;
                                productOrderDataContract.StoreCode = businessPartnerObj.StoreCode;
                                productOrderDataContract.IsRedemptionInstant = businessPartnerObj.IsRedemptionSettelemtInstant;
                                productOrderDataContract.IsVoucher = businessPartnerObj.IsVoucher == null ? false : Convert.ToBoolean(businessPartnerObj.IsVoucher);
                                productOrderDataContract.voucherType = businessPartnerObj.VoucherType == null ? 0 : Convert.ToInt32(businessPartnerObj.VoucherType);
                                productOrderDataContract.IsRedemptionInstant = businessPartnerObj.IsRedemptionSettelemtInstant;
                                productOrderDataContract.IsDefferedSettlement = businessPartnerObj.IsDefferedSettlement == null ? false : Convert.ToBoolean(businessPartnerObj.IsDefferedSettlement);
                            }
                            else
                            {
                                StatusDataContract structObj = new StatusDataContract(false, "No associated store found for this order", productOrderResponseDC);
                                response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                {
                                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                };

                                return response;
                            }
                            orderBasedObj = _orderBasedRepository.GetSingle(x => x.IsActive == true && x.BusinessPartnerId == productOrderDataContract.BusinessPartnerId && x.BusinessUnitId == productOrderDataContract.BUId);
                            if (orderBasedObj != null)
                            {
                                productOrderDataContract.IsSweetenerModelBased = orderBasedObj.IsSweetenerModalBased;
                            }
                            else
                            {
                                StatusDataContract structObj = new StatusDataContract(false, "No data found in order configuration table", productOrderResponseDC);
                                response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                {
                                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                };

                                return response;
                            }
                        }
                    }
                    if (productOrderDataContract.OrderType > 0)
                    {
                        if (!string.IsNullOrEmpty(productOrderDataContract.RegdNo))
                        {
                            abbredemptionObj = _abbRedemptionRepository.GetSingle(x => x.IsActive == true && x.RegdNo == productOrderDataContract.RegdNo);
                            if (abbredemptionObj == null)
                            {
                                productOrderResponseDC = sponsrOrderSyncManager.ProcessOrderInfo(productOrderDataContract, businessUnit);
                                if (productOrderResponseDC != null)
                                {
                                    StatusDataContract structObj = new StatusDataContract(true, "Order Created Successfully", productOrderResponseDC);
                                    response = new HttpResponseMessage(HttpStatusCode.OK)
                                    {
                                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                    };
                                }
                                else
                                {
                                    StatusDataContract structObj = new StatusDataContract(false, "Order not Created", productOrderResponseDC);
                                    response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                    {
                                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                    };

                                }
                            }
                            else
                            {
                                StatusDataContract structObj = new StatusDataContract(false, "Redemption request already registered", productOrderResponseDC);
                                response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                {
                                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                };
                            }

                        }
                        else
                        {
                            StatusDataContract structObj = new StatusDataContract(false, "RegdNo should not be null", productOrderResponseDC);
                            response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                            {
                                Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                            };
                        }

                    }
                    else
                    {
                        if (string.IsNullOrEmpty(productOrderDataContract.SponsorOrderNumber))
                            productOrderDataContract.SponsorOrderNumber = "API" + UniqueString.RandomNumberByLength(9);

                        else if (businessUnit.BusinessUnitId == Convert.ToInt32(BusinessUnitEnum.D2C))
                        {
                            if (string.IsNullOrEmpty(productOrderDataContract.SponsorOrderNumber))
                                productOrderDataContract.SponsorOrderNumber = "D2C" + UniqueString.RandomNumberByLength(9);
                        }
                        //Changes for Add QC Date,Start time, End time
                        if (productOrderDataContract.QCDate != null && productOrderDataContract.StartTime != null)
                        {
                            if (productOrderDataContract.StartTime.Equals("1"))
                            {
                                productOrderDataContract.StartTime = "10:00AM";
                                productOrderDataContract.EndTime = "12:00PM";
                            }
                            else if (productOrderDataContract.StartTime.Equals("2"))
                            {
                                productOrderDataContract.StartTime = "12:00PM";
                                productOrderDataContract.EndTime = "2:00PM";
                            }
                            else if (productOrderDataContract.StartTime.Equals("3"))
                            {
                                productOrderDataContract.StartTime = "2:00PM";
                                productOrderDataContract.EndTime = "4:00PM";
                            }
                            else if (productOrderDataContract.StartTime.Equals("4"))
                            {
                                productOrderDataContract.StartTime = "4:00PM";
                                productOrderDataContract.EndTime = "6:00PM";
                            }
                            else if (productOrderDataContract.StartTime.Equals("5"))
                            {
                                productOrderDataContract.StartTime = "6:00PM";
                                productOrderDataContract.EndTime = "8:00PM";
                            }
                        }
                        string sponsorOrderNo = productOrderDataContract.SponsorOrderNumber;
                        if (!string.IsNullOrEmpty(sponsorOrderNo))
                        {
                            SponserObj = _sponsorRepository.GetSingle(x => x.SponsorOrderNumber != null && x.SponsorOrderNumber == sponsorOrderNo);
                            if (SponserObj != null)
                            {
                                StatusDataContract structObj = new StatusDataContract(false, "This SponsorOrderNumber : " + productOrderDataContract.SponsorOrderNumber + " is already exist", productOrderResponseDC);
                                response = new HttpResponseMessage(HttpStatusCode.OK)
                                {
                                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                };
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(productOrderDataContract.ZipCode))
                                {
                                    int? pincode = Convert.ToInt32(productOrderDataContract.ZipCode);
                                    pincodeObj = _picodeRepository.GetSingle(x => x.IsActive.Equals(true) && x.isExchange.Equals(true) && x.ZipCode.Equals(pincode));
                                    if (pincodeObj != null)
                                    {
                                        productOrderDataContract.City = !string.IsNullOrEmpty(pincodeObj.Location) ? pincodeObj.Location : null;
                                        productOrderDataContract.State = !string.IsNullOrEmpty(pincodeObj.State) ? pincodeObj.State : null;
                                    }
                                    else
                                    {
                                        StatusDataContract structObj = new StatusDataContract(false, "Provided zipcode is not active", productOrderResponseDC);
                                        response = new HttpResponseMessage(HttpStatusCode.OK)
                                        {
                                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                        };
                                        return response;
                                    }
                                }
                                if (productOrderDataContract.StoreCode != null)
                                {
                                    productOrderDataContract.RegdNo = "E" + UniqueString.RandomNumberByLength(7);
                                    productOrderDataContract.UploadDateTime = DateTime.Now.ToString("dd-MMMM-yyyy HH:mm");
                                    tblProducttypeObj = productTypeRepository.GetSingle(x => x.IsActive == true && x.IsAllowedForOld == true && x.Id == productOrderDataContract.ProductTypeId);
                                    if (tblProducttypeObj != null)
                                    {
                                        productPriceDetailsDataContract.BrandId = productOrderDataContract.BrandId;
                                        productPriceDetailsDataContract.NewBrandId = productOrderDataContract.NewBrandId;
                                        productPriceDetailsDataContract.BusinessPartnerId = productOrderDataContract.BusinessPartnerId;
                                        productPriceDetailsDataContract.BusinessUnitId = productOrderDataContract.BUId;
                                        productPriceDetailsDataContract.ProductCatId = tblProducttypeObj.ProductCatId;
                                        productPriceDetailsDataContract.ProductTypeId = productOrderDataContract.ProductTypeId;
                                        productPriceDetailsDataContract.conditionId = Convert.ToInt32(productOrderDataContract.ProductCondition);
                                        universalpricemasterDC = priceMasterManager.GetProductPrice(productPriceDetailsDataContract);
                                        if (universalpricemasterDC.BaseValue != null)
                                        {

                                            detailsforSweetenerDc.BrandId = productOrderDataContract.NewBrandId;
                                            detailsforSweetenerDc.NewProdCatId = productOrderDataContract.NewCatId;
                                            detailsforSweetenerDc.NewProdTypeId = productOrderDataContract.NewTypeId;
                                            detailsforSweetenerDc.BusinessPartnerId = productOrderDataContract.BusinessPartnerId;
                                            detailsforSweetenerDc.BusinessUnitId = productOrderDataContract.BUId;
                                            detailsforSweetenerDc.ModalId = productOrderDataContract.ModelId;
                                            detailsforSweetenerDc.IsSweetenerModalBased = productOrderDataContract.IsSweetenerModelBased;

                                            productOrderDataContract.priceMasterNameID = (int)universalpricemasterDC.PricemasternameId;

                                            if (businessUnit.IsSweetenerIndependent == true)
                                            {
                                                decimal Bonus = 0;
                                                if (!string.IsNullOrEmpty(productOrderDataContract.Bonus))
                                                {
                                                    Bonus = Convert.ToDecimal(productOrderDataContract.Bonus);
                                                }
                                                productOrderDataContract.ExchangePrice = Bonus + universalpricemasterDC.BaseValue;
                                                productOrderDataContract.SweetenerBU = Bonus;
                                                productOrderDataContract.SweetenerBp = 0;
                                                productOrderDataContract.SweetenerDigi2l = 0;
                                                productOrderDataContract.BasePrice = universalpricemasterDC.BaseValue;
                                                productOrderDataContract.Sweetener = Bonus;
                                            }
                                            else
                                            {
                                                productconditionobj = _productConditionLabel.GetSingle(x => x.IsActive == true && x.BusinessUnitId == productOrderDataContract.BUId && x.BusinessPartnerId == productOrderDataContract.BusinessPartnerId && x.OrderSequence == Convert.ToInt32(productOrderDataContract.ProductCondition));
                                                if (productconditionobj != null)
                                                {
                                                    if (productconditionobj.IsSweetenerApplicable == true)
                                                    {
                                                        sweetener = _sweetenerManager.GetSweetenerAmtExchange(detailsforSweetenerDc);
                                                        if (sweetener != null && sweetener.SweetenerTotal != null)
                                                        {
                                                            productOrderDataContract.ExchangePrice = sweetener.SweetenerTotal + universalpricemasterDC.BaseValue;
                                                            productOrderDataContract.BasePrice = universalpricemasterDC.BaseValue;
                                                            productOrderDataContract.SweetenerBU = sweetener.SweetenerBu;
                                                            productOrderDataContract.SweetenerBp = sweetener.SweetenerBP;
                                                            productOrderDataContract.SweetenerDigi2l = sweetener.SweetenerDigi2L;
                                                            productOrderDataContract.Sweetener = sweetener.SweetenerTotal;
                                                        }
                                                        else
                                                        {
                                                            StatusDataContract structObj = new StatusDataContract(false, sweetener.ErrorMessage);
                                                            response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                                            {
                                                                Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                                            };

                                                            return response;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        sweetener.SweetenerTotal = 0;
                                                        sweetener.SweetenerBP = 0;
                                                        sweetener.SweetenerBu = 0;
                                                        sweetener.SweetenerDigi2L = 0;
                                                        productOrderDataContract.ExchangePrice = sweetener.SweetenerTotal + universalpricemasterDC.BaseValue;
                                                        productOrderDataContract.BasePrice = universalpricemasterDC.BaseValue;
                                                        productOrderDataContract.SweetenerBU = sweetener.SweetenerBu;
                                                        productOrderDataContract.SweetenerBp = sweetener.SweetenerBP;
                                                        productOrderDataContract.SweetenerDigi2l = sweetener.SweetenerDigi2L;
                                                        productOrderDataContract.Sweetener = sweetener.SweetenerTotal;
                                                    }

                                                }
                                                else
                                                {
                                                    StatusDataContract structObj = new StatusDataContract(false, "No data found in condition table");
                                                    response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                                    {
                                                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                                    };

                                                    return response;
                                                }
                                            }


                                        }
                                        else
                                        {
                                            StatusDataContract structObj = new StatusDataContract(false, universalpricemasterDC.ErrorMessage);
                                            response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                            {
                                                Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                            };

                                            return response;
                                        }
                                    }
                                    else
                                    {
                                        StatusDataContract structObj = new StatusDataContract(false, "No data found for  the product type provided", productOrderResponseDC);
                                        response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                        {
                                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                        };

                                        return response;
                                    }


                                    string jsonString = JsonConvert.SerializeObject(productOrderDataContract);
                                    logging.WriteAPIRequestToDB("SponserController_Request", "ProductOrderPlace", productOrderDataContract.SponsorOrderNumber, jsonString);

                                    productOrderResponseDC = sponsrOrderSyncManager.ProcessOrderInfo(productOrderDataContract, businessUnit);
                                    if (productOrderResponseDC != null)
                                    {
                                        StatusDataContract structObj = new StatusDataContract(true, "Order Created Successfully", productOrderResponseDC);
                                        response = new HttpResponseMessage(HttpStatusCode.OK)
                                        {
                                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                        };
                                    }
                                    else
                                    {
                                        StatusDataContract structObj = new StatusDataContract(false, "Order not Created", productOrderResponseDC);
                                        response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                        {
                                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                        };

                                    }
                                }
                                else
                                {
                                    StatusDataContract structObj = new StatusDataContract(false, "No associated store found for this order", productOrderResponseDC);
                                    response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                    {
                                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                    };
                                }
                                //Code to add Reg Code
                            }
                        }
                        else
                        {
                            StatusDataContract structObj = new StatusDataContract(false, "Sponsor Order No is required", productOrderResponseDC);
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
                LibLogging.WriteErrorToDB("SponserController", "ProductOrderPlace", ex);
                StatusDataContract structObj = new StatusDataContract(false, ex.Message);
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json")) //new StringContent("error"),                    
                };
            }


            return response;

        }

        #endregion

        #region Push Delivered/Cancelled sponsor product exchange order Status detail
        [HttpPost]
        public HttpResponseMessage ProductOrderStatusChange(ProductOrderStatusDataContract productOrderStatusDataContract)
        {
            SponsrOrderSyncManager sponsrOrderSyncManager = new SponsrOrderSyncManager();
            HttpResponseMessage response = null;
            ProductOrderStatusResponseDataContract productOrderStatusResponseDC = null;
            _sponsorRepository = new ExchangeOrderRepository();
            logging = new Logging();
            tblExchangeOrder SponserObj = null;
            try
            {
                if (productOrderStatusDataContract.OrderId != 0)
                {

                    SponserObj = _sponsorRepository.GetSingle(x => x.Id.Equals(productOrderStatusDataContract.OrderId));

                    string jsonString = JsonConvert.SerializeObject(productOrderStatusDataContract);
                    logging.WriteAPIRequestToDB("SponserController_Request", "ProductOrderStatusChange", SponserObj.SponsorOrderNumber, jsonString);

                    if (SponserObj != null)
                    {
                        productOrderStatusResponseDC = sponsrOrderSyncManager.ProcessOrderStatusInfo(productOrderStatusDataContract);

                        if (productOrderStatusResponseDC != null)
                        {
                            StatusDataContract structObj = new StatusDataContract(true, "Order" + " " + productOrderStatusDataContract.Status + " " + "by Sponsor", productOrderStatusResponseDC);
                            response = new HttpResponseMessage(HttpStatusCode.OK)
                            {
                                Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                            };
                        }
                        else
                        {
                            StatusDataContract structObj = new StatusDataContract(false, "Status not updated.");
                            response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                            {
                                Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                            };

                        }
                    }
                    else
                    {
                        StatusDataContract structObj = new StatusDataContract(false, "Data not found.");
                        response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                        {
                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SponserController", "ProductOrderStatusChange", ex);
                StatusDataContract structObj = new StatusDataContract(false, ex.Message);
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json")) //new StringContent("error"),                    
                };
            }
            return response;
        }
        #endregion

        #region Get sponsor product exchange order Status detail

        [HttpGet]
        public HttpResponseMessage GetOrderStatus(int orderId)
        {
            SponsrOrderSyncManager sponsrOrderSyncManager = new SponsrOrderSyncManager();
            HttpResponseMessage response = null;
            OrderStatusDetailsDataContract orderStatusDetailsDC = null;
            try
            {
                orderStatusDetailsDC = sponsrOrderSyncManager.ProcessGetOrderStatusInfo(orderId);

                if (orderStatusDetailsDC != null)
                {
                    StatusDataContract structObj = new StatusDataContract(true, "success", orderStatusDetailsDC);
                    response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                }
                else
                {
                    StatusDataContract structObj = new StatusDataContract(false, "Status not updated.");
                    response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };

                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SponserController", "GetOrderStatus", ex);
                StatusDataContract structObj = new StatusDataContract(false, ex.Message);
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json")) //new StringContent("error"),                    
                };
            }


            return response;

        }

        #endregion         

        #region Get all Zoho Sponser detail
        [HttpGet]
        public HttpResponseMessage GetAllSponser()
        {
            _sponserManager = new SponserManager();
            HttpResponseMessage response = null;
            List<SponserData> sponserDC = null;
            try
            {
                sponserDC = _sponserManager.GetAllSponser();

                if (sponserDC != null)
                {
                    StatusDataContract structObj = new StatusDataContract(true, "Success", sponserDC);
                    response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                }
                else
                {
                    StatusDataContract structObj = new StatusDataContract(false, "No data found.");
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

        #region Add Zoho Sponser detail

        [HttpPost]
        public HttpResponseMessage AddSponser(SponserDataContract sponserDataContract)
        {
            _sponserManager = new SponserManager();
            HttpResponseMessage response = null;
            SponserFormResponseDataContract sponserResponseDC = null;
            try
            {
                sponserResponseDC = _sponserManager.AddSponser(sponserDataContract);

                if (sponserResponseDC != null)
                {
                    StatusDataContract structObj = new StatusDataContract(true, "Success", sponserResponseDC);
                    response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                }
                else
                {
                    StatusDataContract structObj = new StatusDataContract(false, "No data found.");
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

        #region Order Deatails on basis of voucher code and customer's Mobile No
        [HttpGet]
        public HttpResponseMessage GetOrderDetails(string VoucherCode, string PhoneNo)
        {
            _exchangeOrderManager = new ExchangeOrderManager();
            _exchangeOrderRepository = new ExchangeOrderRepository();
            _customerDetailsRepository = new CustomerDetailsRepository();
            productCategoryRepository = new ProductCategoryRepository();
            productTypeRepository = new ProductTypeRepository();
            _businessPartnerRepository = new BusinessPartnerRepository();
            businessUnitRepository = new BusinessUnitRepository();
            _abbRedemptionRepository = new AbbRedemptionRepository();
            _abbRegistrationRepository = new ABBRegistrationRepository();
            _BPBURedemptionMappingRepository = new BPBURedemptionMappingRepository();
            Login loginObj = null;
            _loginRepository = new LoginDetailsUTCRepository();
            HttpResponseMessage response = null;
            ExchangeOrderDetail exchangeResponceDC = new ExchangeOrderDetail();
            ErrorMessageDataContract errorMessageDataContract = new ErrorMessageDataContract();
            tblExchangeOrder ExchangeObj = null;
            tblCustomerDetail custObj = null;
            tblABBRedemption RedemptionObj = null;
            tblABBRegistration RegistrationObj = null;
            tblBusinessPartner businesspartnerObj = null;
            bool isOtherBuMappedwithVoucherBP = false;
            try
            {
                string userName = string.Empty;
                if (HttpContext.Current != null && HttpContext.Current.User != null
                        && HttpContext.Current.User.Identity.Name != null)
                    userName = HttpContext.Current.User.Identity.Name;
                else
                {
                    errorMessageDataContract.Message = "Invalid token";
                    StatusDataContract structObj = new StatusDataContract(false, "Authorization failure", errorMessageDataContract);
                    response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                }
                if (VoucherCode != string.Empty && VoucherCode != null && PhoneNo != null && PhoneNo != string.Empty)
                {
                    loginObj = _loginRepository.GetSingle(x => !string.IsNullOrEmpty(x.username) && x.username.ToLower().Equals(userName.ToLower()));
                    if (loginObj != null)
                    {

                        ExchangeObj = _exchangeOrderRepository.GetSingle(x => !string.IsNullOrEmpty(x.VoucherCode) && x.VoucherCode.ToLower().Equals(VoucherCode.ToLower()) && x.IsDefferedSettlement == false);
                        RedemptionObj = _abbRedemptionRepository.GetSingle(x => !string.IsNullOrEmpty(x.VoucherCode) && x.VoucherCode.ToLower().Equals(VoucherCode.ToLower()) && x.IsDefferedSettelment == false);
                        if (ExchangeObj != null)
                        {
                            if (ExchangeObj.IsVoucherused == false)
                            {
                                if (ExchangeObj.VoucherCodeExpDate >= currentDatetime)
                                {
                                    businesspartnerObj = _businessPartnerRepository.GetSingle(x => x.BusinessPartnerId == ExchangeObj.BusinessPartnerId);
                                    if (businesspartnerObj != null)
                                    {
                                        if (businesspartnerObj.BusinessUnitId == loginObj.SponsorId)
                                        {
                                            custObj = _customerDetailsRepository.GetSingle(x => !string.IsNullOrEmpty(x.PhoneNumber) && x.PhoneNumber.Equals(PhoneNo) && x.Id.Equals(ExchangeObj.CustomerDetailsId));
                                            if (custObj != null)
                                            {

                                                exchangeResponceDC = _exchangeOrderManager.GetExchangeDetailForVoucherCode(ExchangeObj, custObj, RedemptionObj, RegistrationObj);
                                                if (exchangeResponceDC != null)
                                                {
                                                    StatusDataContract structObj = new StatusDataContract(true, "Success", exchangeResponceDC);
                                                    response = new HttpResponseMessage(HttpStatusCode.OK)
                                                    {
                                                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                                    };
                                                }


                                            }
                                            else
                                            {
                                                errorMessageDataContract.Message = "Mobile number entered is incorrect";
                                                StatusDataContract structObj = new StatusDataContract(false, "Invalid Input", errorMessageDataContract);
                                                response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                                {
                                                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                                };
                                            }
                                        }
                                        else
                                        {
                                            List<tblBPBURedemptionMapping> tblBPBURedemptionMappings = _BPBURedemptionMappingRepository.GetList(x => x.IsActive == true && x.BusinessPartnerId == businesspartnerObj.BusinessPartnerId).ToList();
                                            if (tblBPBURedemptionMappings != null && tblBPBURedemptionMappings.Count > 0)
                                            {
                                                foreach (tblBPBURedemptionMapping tblBPBURedemptionMapping in tblBPBURedemptionMappings)
                                                {
                                                    if (tblBPBURedemptionMapping.BusinessUnitId == loginObj.SponsorId)
                                                    {
                                                        custObj = _customerDetailsRepository.GetSingle(x => !string.IsNullOrEmpty(x.PhoneNumber) && x.PhoneNumber.Equals(PhoneNo) && x.Id.Equals(ExchangeObj.CustomerDetailsId));
                                                        if (custObj != null)
                                                        {

                                                            exchangeResponceDC = _exchangeOrderManager.GetExchangeDetailForVoucherCode(ExchangeObj, custObj, RedemptionObj, RegistrationObj);
                                                            if (exchangeResponceDC != null)
                                                            {
                                                                isOtherBuMappedwithVoucherBP = true;
                                                                StatusDataContract structObj = new StatusDataContract(true, "Success", exchangeResponceDC);
                                                                response = new HttpResponseMessage(HttpStatusCode.OK)
                                                                {
                                                                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                                                };
                                                            }

                                                        }
                                                        else
                                                        {
                                                            isOtherBuMappedwithVoucherBP = false;

                                                            continue;
                                                        }
                                                    }



                                                }
                                                if (isOtherBuMappedwithVoucherBP == false)
                                                {
                                                    errorMessageDataContract.Message = "You are not authorized to redeem voucher";
                                                    StatusDataContract structObj = new StatusDataContract(false, "NonAuthoritativeInformation", errorMessageDataContract);
                                                    response = new HttpResponseMessage(HttpStatusCode.NonAuthoritativeInformation)
                                                    {
                                                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                                    };
                                                }
                                            }
                                            else
                                            {
                                                errorMessageDataContract.Message = "You are not authorized to redeem voucher";
                                                StatusDataContract structObj = new StatusDataContract(false, "NonAuthoritativeInformation", errorMessageDataContract);
                                                response = new HttpResponseMessage(HttpStatusCode.NonAuthoritativeInformation)
                                                {
                                                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                                };
                                            }
                                        }


                                    }
                                    else
                                    {
                                        errorMessageDataContract.Message = "You are not authorized to redeem this voucher";
                                        StatusDataContract structObj = new StatusDataContract(false, "Not Valid user", errorMessageDataContract);
                                        response = new HttpResponseMessage(HttpStatusCode.NonAuthoritativeInformation)
                                        {
                                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                        };
                                    }


                                }
                                else
                                {
                                    errorMessageDataContract.Message = "Voucher is already used";
                                    StatusDataContract structObj = new StatusDataContract(false, "Voucher Expired", errorMessageDataContract);
                                    response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                    {
                                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                    };
                                }

                            }
                            else
                            {
                                errorMessageDataContract.Message = "Voucher is already used";
                                StatusDataContract structObj = new StatusDataContract(false, "Voucher Expired", errorMessageDataContract);
                                response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                {
                                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                };
                            }

                        }
                        else if (RedemptionObj != null)
                        {
                            RegistrationObj = _abbRegistrationRepository.GetSingle(x => x.IsActive == true && x.ABBRegistrationId == RedemptionObj.ABBRegistrationId);
                            if (RegistrationObj != null)
                            {
                                businesspartnerObj = _businessPartnerRepository.GetSingle(x => x.BusinessPartnerId == RegistrationObj.BusinessPartnerId);
                                if (businesspartnerObj != null && businesspartnerObj.BusinessUnitId == loginObj.SponsorId)
                                {
                                    if (RedemptionObj.IsVoucherUsed == false)
                                    {

                                        if (RedemptionObj.VoucherCodeExpDate >= currentDatetime)
                                        {
                                            custObj = _customerDetailsRepository.GetSingle(x => !string.IsNullOrEmpty(x.PhoneNumber) && x.PhoneNumber.Equals(PhoneNo) && x.Id.Equals(RedemptionObj.CustomerDetailsId));
                                            if (custObj != null)
                                            {


                                                exchangeResponceDC = _exchangeOrderManager.GetExchangeDetailForVoucherCode(ExchangeObj, custObj, RedemptionObj, RegistrationObj);
                                                if (exchangeResponceDC != null)
                                                {
                                                    StatusDataContract structObj = new StatusDataContract(true, "Success", exchangeResponceDC);
                                                    response = new HttpResponseMessage(HttpStatusCode.OK)
                                                    {
                                                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                                    };
                                                }


                                            }
                                            else
                                            {
                                                errorMessageDataContract.Message = "Mobile number entered is incorrect";
                                                StatusDataContract structObj = new StatusDataContract(false, "Invalid Input", errorMessageDataContract);
                                                response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                                {
                                                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                                };
                                            }

                                        }
                                        else
                                        {
                                            errorMessageDataContract.Message = "Voucher is already used";
                                            StatusDataContract structObj = new StatusDataContract(false, "Voucher Expired", errorMessageDataContract);
                                            response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                            {
                                                Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                            };
                                        }
                                    }
                                    else
                                    {
                                        errorMessageDataContract.Message = "Voucher is already used";
                                        StatusDataContract structObj = new StatusDataContract(false, "Voucher Expired", errorMessageDataContract);
                                        response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                        {
                                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                        };
                                    }
                                }
                                else
                                {
                                    errorMessageDataContract.Message = "You are not authorized to redeem this voucher";
                                    StatusDataContract structObj = new StatusDataContract(false, "Not Valid user", errorMessageDataContract);
                                    response = new HttpResponseMessage(HttpStatusCode.NonAuthoritativeInformation)
                                    {
                                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                    };
                                }
                            }
                            else
                            {
                                errorMessageDataContract.Message = "Abb registration data not found";
                                StatusDataContract structObj = new StatusDataContract(false, "Not Valid user", errorMessageDataContract);
                                response = new HttpResponseMessage(HttpStatusCode.NonAuthoritativeInformation)
                                {
                                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                };
                            }

                        }
                        else
                        {
                            errorMessageDataContract.Message = "Invalid token";
                            StatusDataContract structObj = new StatusDataContract(false, "Voucher is not valid");
                            response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                            {
                                Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                            };
                        }
                    }
                    else
                    {
                        errorMessageDataContract.Message = "Invalid token";
                        StatusDataContract structObj = new StatusDataContract(false, "Authorization failure", errorMessageDataContract);
                        response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                        {
                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                        };
                    }
                }
                else
                {
                    errorMessageDataContract.Message = "VoucherCode or PhoneNo should not be empty or null";

                    StatusDataContract structObj = new StatusDataContract(false, "Invalid Input", errorMessageDataContract);
                    response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                }


            }
            catch (Exception ex)
            {

                StatusDataContract structObj = new StatusDataContract(false, "Process Denied", ex.Message);
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json")) //new StringContent("error"),                    
                };
                LibLogging.WriteErrorToDB("Sponsor", "GetOrderDetails", ex);
            }
            return response;
        }
        #endregion

        #region Method to Capture Voucher
        [HttpPost]
        public HttpResponseMessage CaptureVoucher(string VoucherCode, bool IsVoucherUsed, int? NewBrandId = 0)
        {
            int result = 0;
            _exchangeOrderManager = new ExchangeOrderManager();
            _brandRepository = new BrandRepository();
            _businessPartnerRepository = new BusinessPartnerRepository();
            businessUnitRepository = new BusinessUnitRepository();
            _exchangeOrderRepository = new ExchangeOrderRepository();
            _abbRedemptionRepository = new AbbRedemptionRepository();
            _abbRegistrationRepository = new ABBRegistrationRepository();
            _BPBURedemptionMappingRepository = new BPBURedemptionMappingRepository();
            VoucherDataContract voucherDC = new VoucherDataContract();
            HttpResponseMessage response = null;
            Login loginObj = null;
            _loginRepository = new LoginDetailsUTCRepository();
            ErrorMessageDataContract errorMessageDataContract = new ErrorMessageDataContract();
            tblExchangeOrder ExchangeObj = null;
            tblABBRedemption RedemptionObj = null;
            tblABBRegistration registrationObj = null;
            bool isOtherBuMappedwithVoucherBP = false;
            try
            {
                string userName = string.Empty;
                if (HttpContext.Current != null && HttpContext.Current.User != null
                        && HttpContext.Current.User.Identity.Name != null)
                    userName = HttpContext.Current.User.Identity.Name;

                loginObj = _loginRepository.GetSingle(x => !string.IsNullOrEmpty(x.username) && x.username.ToLower().Equals(userName.ToLower()));
                if (loginObj != null)
                {
                    if (VoucherCode != null && IsVoucherUsed == true)
                    {
                        if (NewBrandId > 0)
                        {
                            ExchangeObj = _exchangeOrderRepository.GetSingle(x => !string.IsNullOrEmpty(x.VoucherCode) && x.VoucherCode.ToLower().Equals(VoucherCode.ToLower()) && x.IsDefferedSettlement == false && x.NewBrandId == NewBrandId);
                        }
                        else
                        {
                            ExchangeObj = _exchangeOrderRepository.GetSingle(x => !string.IsNullOrEmpty(x.VoucherCode) && x.VoucherCode.ToLower().Equals(VoucherCode.ToLower()) && x.IsDefferedSettlement == false);
                        }

                        RedemptionObj = _abbRedemptionRepository.GetSingle(x => !string.IsNullOrEmpty(x.VoucherCode) && x.VoucherCode.ToLower().Equals(VoucherCode.ToLower()) && x.IsDefferedSettelment == false);
                        if (ExchangeObj != null)
                        {
                            if (ExchangeObj.IsVoucherused == false)
                            {

                                if (ExchangeObj.VoucherCodeExpDate >= currentDatetime)
                                {
                                    tblBusinessPartner businessPartner = _businessPartnerRepository.GetSingle(x => x.BusinessPartnerId.Equals(ExchangeObj.BusinessPartnerId));
                                    if (businessPartner != null)
                                    {
                                        if (businessPartner.BusinessUnitId == loginObj.SponsorId)
                                        {
                                            voucherDC.BusinessPartnerId = ExchangeObj.BusinessPartnerId;
                                            voucherDC.BusinessUnitId = Convert.ToInt32(loginObj.SponsorId);
                                            voucherDC.ExchangeOrderDataContract.Id = ExchangeObj.Id;
                                            voucherDC.ExchangeOrderId = ExchangeObj.Id;
                                            voucherDC.CustomerId = ExchangeObj.CustomerDetailsId;
                                            voucherDC.VoucherCode = ExchangeObj.VoucherCode;
                                            voucherDC.IsVoucherused = IsVoucherUsed;
                                            result = _exchangeOrderManager.AddVoucherToDataBaseAPI(voucherDC, ExchangeObj, RedemptionObj, registrationObj);
                                            if (result > 0)
                                            {
                                                errorMessageDataContract.Message = "Congratulation's ! You have successfully Capture voucher code";
                                                StatusDataContract structObj = new StatusDataContract(true, "Success", errorMessageDataContract);
                                                response = new HttpResponseMessage(HttpStatusCode.OK)
                                                {
                                                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                                };
                                            }
                                        }
                                        else
                                        {
                                            List<tblBPBURedemptionMapping> BPBURedemptionMappingList = _BPBURedemptionMappingRepository.GetList(x => x.IsActive == true && x.BusinessPartnerId == businessPartner.BusinessPartnerId).ToList();
                                            if (BPBURedemptionMappingList != null && BPBURedemptionMappingList.Count > 0)
                                            {
                                                foreach (tblBPBURedemptionMapping tblBPBURedemptionMapping in BPBURedemptionMappingList)
                                                {
                                                    if (tblBPBURedemptionMapping.BusinessUnitId == loginObj.SponsorId)
                                                    {
                                                        voucherDC.BusinessPartnerId = ExchangeObj.BusinessPartnerId;
                                                        voucherDC.BusinessUnitId = Convert.ToInt32(loginObj.SponsorId);
                                                        voucherDC.ExchangeOrderDataContract.Id = ExchangeObj.Id;
                                                        voucherDC.ExchangeOrderId = ExchangeObj.Id;
                                                        voucherDC.CustomerId = ExchangeObj.CustomerDetailsId;
                                                        voucherDC.VoucherCode = ExchangeObj.VoucherCode;
                                                        voucherDC.IsVoucherused = IsVoucherUsed;
                                                        result = _exchangeOrderManager.AddVoucherToDataBaseAPI(voucherDC, ExchangeObj, RedemptionObj, registrationObj);
                                                        if (result > 0)
                                                        {
                                                            isOtherBuMappedwithVoucherBP = true;
                                                            errorMessageDataContract.Message = "Congratulation's ! You have successfully Capture voucher code";
                                                            StatusDataContract structObj = new StatusDataContract(true, "Success", errorMessageDataContract);
                                                            response = new HttpResponseMessage(HttpStatusCode.OK)
                                                            {
                                                                Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                                            };
                                                            return response;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        isOtherBuMappedwithVoucherBP = false;

                                                        continue;
                                                    }
                                                }

                                                if (isOtherBuMappedwithVoucherBP == false)
                                                {
                                                    errorMessageDataContract.Message = "You are not authorized to redeem voucher";
                                                    StatusDataContract structObj = new StatusDataContract(false, "NonAuthoritativeInformation", errorMessageDataContract);
                                                    response = new HttpResponseMessage(HttpStatusCode.NonAuthoritativeInformation)
                                                    {
                                                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                                    };
                                                }

                                            }
                                            else
                                            {
                                                errorMessageDataContract.Message = "You are not authorized to redeem voucher";
                                                StatusDataContract structObj = new StatusDataContract(false, "NonAuthoritativeInformation", errorMessageDataContract);
                                                response = new HttpResponseMessage(HttpStatusCode.NonAuthoritativeInformation)
                                                {
                                                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                                };
                                            }

                                        }

                                    }
                                    else
                                    {
                                        errorMessageDataContract.Message = "You are not authorized to redeem voucher";
                                        StatusDataContract structObj = new StatusDataContract(false, "NonAuthoritativeInformation", errorMessageDataContract);
                                        response = new HttpResponseMessage(HttpStatusCode.NonAuthoritativeInformation)
                                        {
                                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                        };
                                    }
                                }
                                else
                                {
                                    errorMessageDataContract.Message = "voucher is already used";
                                    StatusDataContract structObj = new StatusDataContract(false, "Invalid Input", errorMessageDataContract);
                                    response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                    {
                                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                    };
                                }
                            }
                            else
                            {
                                errorMessageDataContract.Message = "voucher is already used";
                                StatusDataContract structObj = new StatusDataContract(false, "Invalid Input", errorMessageDataContract);
                                response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                {
                                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                };
                            }
                        }
                        else if (RedemptionObj != null)
                        {
                            if (RedemptionObj.IsVoucherUsed == false)
                            {

                                if (RedemptionObj.VoucherCodeExpDate >= currentDatetime)
                                {
                                    registrationObj = _abbRegistrationRepository.GetSingle(x => x.ABBRegistrationId == RedemptionObj.ABBRegistrationId && x.IsActive == true);
                                    if (registrationObj != null)
                                    {
                                        tblBusinessPartner businessPartner = _businessPartnerRepository.GetSingle(x => x.BusinessPartnerId.Equals(registrationObj.BusinessPartnerId) && x.IsActive == true);
                                        if (businessPartner != null)
                                        {
                                            if (businessPartner.BusinessUnitId == loginObj.SponsorId)
                                            {
                                                voucherDC.BusinessPartnerId = registrationObj.BusinessPartnerId;
                                                voucherDC.BusinessUnitId = Convert.ToInt32(loginObj.SponsorId);
                                                voucherDC.RedemptionId = RedemptionObj.RedemptionId;
                                                voucherDC.CustomerId = RedemptionObj.CustomerDetailsId;
                                                voucherDC.VoucherCode = RedemptionObj.VoucherCode;
                                                voucherDC.IsVoucherused = IsVoucherUsed;
                                                result = _exchangeOrderManager.AddVoucherToDataBaseAPI(voucherDC, ExchangeObj, RedemptionObj, registrationObj);
                                                if (result > 0)
                                                {
                                                    errorMessageDataContract.Message = "Congratulation's ! You have successfully Capture voucher code";
                                                    StatusDataContract structObj = new StatusDataContract(true, "Success", errorMessageDataContract);
                                                    response = new HttpResponseMessage(HttpStatusCode.OK)
                                                    {
                                                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                                    };
                                                }
                                            }
                                            else
                                            {
                                                List<tblBPBURedemptionMapping> BPBURedemptionMappingList = _BPBURedemptionMappingRepository.GetList(x => x.IsActive == true && x.BusinessPartnerId == businessPartner.BusinessPartnerId).ToList();
                                                if (BPBURedemptionMappingList != null && BPBURedemptionMappingList.Count > 0)
                                                {
                                                    foreach (tblBPBURedemptionMapping tblBPBURedemptionMapping in BPBURedemptionMappingList)
                                                    {
                                                        if (tblBPBURedemptionMapping.BusinessUnitId == loginObj.SponsorId)
                                                        {
                                                            voucherDC.BusinessPartnerId = ExchangeObj.BusinessPartnerId;
                                                            voucherDC.BusinessUnitId = Convert.ToInt32(loginObj.SponsorId);
                                                            voucherDC.ExchangeOrderDataContract.Id = ExchangeObj.Id;
                                                            voucherDC.ExchangeOrderId = ExchangeObj.Id;
                                                            voucherDC.CustomerId = ExchangeObj.CustomerDetailsId;
                                                            voucherDC.VoucherCode = ExchangeObj.VoucherCode;
                                                            voucherDC.IsVoucherused = IsVoucherUsed;
                                                            result = _exchangeOrderManager.AddVoucherToDataBaseAPI(voucherDC, ExchangeObj, RedemptionObj, registrationObj);
                                                            if (result > 0)
                                                            {
                                                                isOtherBuMappedwithVoucherBP = true;
                                                                errorMessageDataContract.Message = "Congratulation's ! You have successfully Capture voucher code";
                                                                StatusDataContract structObj = new StatusDataContract(true, "Success", errorMessageDataContract);
                                                                response = new HttpResponseMessage(HttpStatusCode.OK)
                                                                {
                                                                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                                                };
                                                                return response;

                                                            }
                                                        }
                                                        else
                                                        {
                                                            isOtherBuMappedwithVoucherBP = false;

                                                            continue;
                                                        }
                                                    }

                                                    if (isOtherBuMappedwithVoucherBP == false)
                                                    {
                                                        errorMessageDataContract.Message = "You are not authorized to redeem voucher";
                                                        StatusDataContract structObj = new StatusDataContract(false, "NonAuthoritativeInformation", errorMessageDataContract);
                                                        response = new HttpResponseMessage(HttpStatusCode.NonAuthoritativeInformation)
                                                        {
                                                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                                        };
                                                    }

                                                }
                                                else
                                                {
                                                    errorMessageDataContract.Message = "You are not authorized to redeem voucher";
                                                    StatusDataContract structObj = new StatusDataContract(false, "NonAuthoritativeInformation", errorMessageDataContract);
                                                    response = new HttpResponseMessage(HttpStatusCode.NonAuthoritativeInformation)
                                                    {
                                                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                                    };
                                                }
                                            }

                                        }
                                        else
                                        {
                                            errorMessageDataContract.Message = "You are not authorized to redeem voucher";
                                            StatusDataContract structObj = new StatusDataContract(false, "NonAuthoritativeInformation", errorMessageDataContract);
                                            response = new HttpResponseMessage(HttpStatusCode.NonAuthoritativeInformation)
                                            {
                                                Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                            };
                                        }
                                    }
                                    else
                                    {
                                        errorMessageDataContract.Message = "Registration details does not have dealer data";
                                        StatusDataContract structObj = new StatusDataContract(false, "NonAuthoritativeInformation", errorMessageDataContract);
                                        response = new HttpResponseMessage(HttpStatusCode.NonAuthoritativeInformation)
                                        {
                                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                        };
                                    }

                                }
                                else
                                {
                                    errorMessageDataContract.Message = "voucher is already used";
                                    StatusDataContract structObj = new StatusDataContract(false, "Invalid Input", errorMessageDataContract);
                                    response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                    {
                                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                    };
                                }
                            }
                            else
                            {
                                errorMessageDataContract.Message = "voucher is already used";
                                StatusDataContract structObj = new StatusDataContract(false, "Invalid Input", errorMessageDataContract);
                                response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                {
                                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                };
                            }
                        }
                        else
                        {
                            errorMessageDataContract.Message = "The voucher code you have entered is incorrect";
                            StatusDataContract structObj = new StatusDataContract(false, "Invalid Input", errorMessageDataContract);
                            response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                            {
                                Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                            };
                        }

                    }
                    else
                    {
                        errorMessageDataContract.Message = "Please check voucher code should not be empty and isvoucherused should be true";
                        StatusDataContract structObj = new StatusDataContract(false, "Invalid Input", errorMessageDataContract);
                        response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                        {
                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                        };
                    }
                }
                else
                {
                    errorMessageDataContract.Message = "Invalid token";
                    StatusDataContract structObj = new StatusDataContract(false, "Authorization failure", errorMessageDataContract);
                    response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                }
            }
            catch (Exception ex)
            {
                StatusDataContract structObj = new StatusDataContract(false, "InternalServerErrror", ex.Message);
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json")) //new StringContent("error"),                    
                };
            }
            return response;
        }
        #endregion

        [HttpGet]
        public HttpResponseMessage getRegNoExchange(int count)
        {
            HttpResponseMessage response = null;
            RegnioList regdNo = new RegnioList();
            List<string> reg = new List<string>();
            regdNo.regNo = new List<string>();
            string regno = null;

            int i = count;
            for (i = 1; i <= count; i++)
            {
                regno = "E" + UniqueString.RandomNumberByLength(8);
                regdNo.regNo.Add(regno);
            }

            response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent<RegnioList>(regdNo, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
            };

            return response;

        }


        #region Get Sponser exchange order Status detail 
        /// <summary>
        /// get orderdetails with help of orderid - created by ashwin 02-03-2023
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetOrderDetails(int orderId)
        {
            SponsrOrderSyncManager sponsrOrderSyncManager = new SponsrOrderSyncManager();
            HttpResponseMessage response = null;
            OrderDetailsViewModel orderDetailsViewModel = null;
            try
            {
                orderDetailsViewModel = sponsrOrderSyncManager.GetOrderStatusInfo(orderId);

                if (orderDetailsViewModel != null)
                {
                    StatusDataContract structObj = new StatusDataContract(true, "success", orderDetailsViewModel);
                    response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                }
                else
                {
                    StatusDataContract structObj = new StatusDataContract(false, "Status not updated.");
                    response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };

                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SponserController", "GetOrderStatus", ex);
                StatusDataContract structObj = new StatusDataContract(false, ex.Message);
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json")) //new StringContent("error"),                    
                };
            }


            return response;

        }
        #endregion

        #region Get all PinCode detail
        [HttpGet]
        public HttpResponseMessage GetZipCodesABB()
        {
            _exchangeOrderManager = new ExchangeOrderManager();
            HttpResponseMessage response = null;
            PinCodeDataContract pinCodeDC = null;        
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
                    if (loginObj != null && loginObj.BusinessPartnerId != null)
                    {
                        buid = loginObj.SponsorId != null ? Convert.ToInt32(loginObj.SponsorId) : 0;                      
                        pinCodeDC = _exchangeOrderManager.GetZipCodesABB(buid);

                        if (pinCodeDC != null)
                        {
                            StatusDataContract structObj = new StatusDataContract(true, "Success", pinCodeDC);
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
                LibLogging.WriteErrorToDB("SponserController", "GetZipCodes", ex);
                StatusDataContract structObj = new StatusDataContract(false, ex.Message);
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json")) //new StringContent("error"),                    
                };
            }


            return response;

        }

        #endregion

        [HttpGet]
        public HttpResponseMessage getRegNoABB(int count)
        {
            HttpResponseMessage response = null;
            RegnioList regdNo = new RegnioList();
            List<string> reg = new List<string>();
            regdNo.regNo = new List<string>();
            string regno = null;

            int i = count;
            for (i = 1; i <= count; i++)
            {
                regno = "A" + UniqueString.RandomNumberByLength(8);
                regdNo.regNo.Add(regno);
            }

            response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent<RegnioList>(regdNo, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
            };

            return response;

        }
    }
}
