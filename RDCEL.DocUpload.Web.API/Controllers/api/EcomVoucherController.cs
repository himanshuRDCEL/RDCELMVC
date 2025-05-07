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

using RDCEL.DocUpload.DataContract.ProductsPrices;
using RDCEL.DocUpload.DataContract.MasterModel;
using GraspCorn.Common.Enums;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DAL;
using GraspCorn.Common.Helper;
using RDCEL.DocUpload.DataContract.UniversalPricemasterDetails;
using RDCEL.DocUpload.BAL.OldProductDetailsManager;
using RDCEL.DocUpload.BAL.EcomVoucher;
using RDCEL.DocUpload.BAL.ProcessAPI;
using RDCEL.DocUpload.DataContract.Bizlog;
using RDCEL.DocUPload.DataContract.EcomVoucher;

using System.Web;
using System.Web.Http;

namespace RDCEL.DocUpload.Web.API.Controllers.api
{
    [Authorize]
    public class EcomVoucherController : BaseApiController
    {
        #region Variable declaration

        BusinessUnitRepository businessUnitRepository;
        LoginDetailsUTCRepository _loginRepository;
        BrandManager _brandManager;
        EcomVoucherManager _ecomManager;
        CompanyRepository _companyRepository;
        #endregion  
        #region Get all Brand detail
        [HttpGet]
        public HttpResponseMessage GetBrand(string StoreCode = null)
        {

            _brandManager = new BrandManager();
            HttpResponseMessage response = null;
            BrandDataContract brandDC = new BrandDataContract();
            businessUnitRepository = new BusinessUnitRepository();
            _loginRepository = new LoginDetailsUTCRepository();

            string username = string.Empty;
            Login loginObj = null;

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
                        if (loginObj.SponsorId != null)
                        {

                            BrandDC = _brandManager.GetBrandListByBUId(loginObj.SponsorId);
                            brandDC.Brand = BrandDC;
                        }
                    }
                    if (brandDC != null && brandDC.Brand.Count > 0)
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

        [HttpGet]
        public HttpResponseMessage GetCategoryListByBrandId(int? brandId)
        {

            _brandManager = new BrandManager();
            HttpResponseMessage response = null;
           
            businessUnitRepository = new BusinessUnitRepository();
            _loginRepository = new LoginDetailsUTCRepository();

            string username = string.Empty;
            Login loginObj = null;

            List<System.Web.Mvc.SelectListItem> CategoryList = new List<System.Web.Mvc.SelectListItem>();
            try
            {
                if (brandId == null || brandId == 0)
                {
                    brandId = null;
                }
                username = RequestContext.Principal.Identity.Name;
                loginObj = _loginRepository.GetSingle(x => !string.IsNullOrEmpty(x.username) && x.username.ToLower().Equals(username.ToLower()));
                if (loginObj != null)
                {
                   
                        if (loginObj.SponsorId != null)
                        {

                            CategoryList = _brandManager.GetCategoryListByBrandId(Convert.ToInt32(brandId));
                           
                        }
                    
                    if (CategoryList != null && CategoryList.Count > 0)
                    {
                        StatusDataContract structObj = new StatusDataContract(true, "Success", CategoryList);
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
        [System.Web.Http.HttpPost]
      
        public HttpResponseMessage ManageEcomVoucher([FromBody] EcomVoucherDataContract ecomVoucherDC)
        {

        EcomVoucherManager ecomVoucherManager = new EcomVoucherManager();
            HttpResponseMessage response = null;
            EcomVoucherDataContract respecomvoucherDC = null;
            _loginRepository = new LoginDetailsUTCRepository();
            _companyRepository = new CompanyRepository();
            Login loginObj = null;

            try
            {
                if (!ModelState.IsValid)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                string userName = string.Empty;

                if (HttpContext.Current != null && HttpContext.Current.User != null
                        && HttpContext.Current.User.Identity.Name != null)
                {
                    userName = HttpContext.Current.User.Identity.Name;

                    loginObj = _loginRepository.GetSingle(x => !string.IsNullOrEmpty(x.username) && x.username.ToLower().Equals(userName.ToLower()));
                   
                    if (loginObj != null && loginObj.SponsorId != null)
                    {


                      tblCompany tblCompany=_companyRepository.GetSingle(x => x.BusinessUnitId== loginObj.SponsorId);
                        if (tblCompany != null)
                        {
                            ecomVoucherDC.CompanyId =tblCompany.CompanyId;
                        }
                        respecomvoucherDC = ecomVoucherManager.ManageEcomVoucher(ecomVoucherDC);

                        if (respecomvoucherDC != null && respecomvoucherDC.success == true)
                        {

                            StatusDataContract structObj = new StatusDataContract(true, "Success", respecomvoucherDC);
                            response = new HttpResponseMessage(HttpStatusCode.OK)
                            {
                                Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                            };
                        }

                        else
                        {
                            StatusDataContract structObj = new StatusDataContract(false, "Unsuccess", respecomvoucherDC);
                            response = new HttpResponseMessage(HttpStatusCode.OK)
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

        [HttpPost]
        public HttpResponseMessage GetEcomVoucherPrice([FromBody] ResquestEcomVoucherPriceDC resquestDC)
        {
            EcomVoucherManager ecomVoucherManager = new EcomVoucherManager();
            HttpResponseMessage response = null;
            ResponseEcomVoucherPriceDC respecomvoucherDC = null;
            _loginRepository = new LoginDetailsUTCRepository();
            _companyRepository = new CompanyRepository();
            Login loginObj = null;
            int? companyId = 0;

            try
            {
                if (!ModelState.IsValid)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                string userName = string.Empty;

                if (HttpContext.Current != null && HttpContext.Current.User != null
                        && HttpContext.Current.User.Identity.Name != null)
                {
                    userName = HttpContext.Current.User.Identity.Name;

                    loginObj = _loginRepository.GetSingle(x => !string.IsNullOrEmpty(x.username) && x.username.ToLower().Equals(userName.ToLower()));

                    if (loginObj != null && loginObj.SponsorId != null)
                    {
                        tblCompany tblCompany = _companyRepository.GetSingle(x => x.BusinessUnitId == loginObj.SponsorId);
                        if (tblCompany != null)
                        {
                            companyId = tblCompany.CompanyId;
                        }
                        respecomvoucherDC = ecomVoucherManager.GetEcomVoucherPriceDetail(resquestDC,companyId);

                        if (respecomvoucherDC != null && respecomvoucherDC.Message == "Success")
                        {

                            StatusDataContract structObj = new StatusDataContract(true, "Success", respecomvoucherDC);
                            response = new HttpResponseMessage(HttpStatusCode.OK)
                            {
                                Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                            };
                        }

                        else
                        {
                            StatusDataContract structObj = new StatusDataContract(false, "Unsuccess", respecomvoucherDC);
                            response = new HttpResponseMessage(HttpStatusCode.OK)
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

        [HttpPost]
        public HttpResponseMessage EcomVoucherRedemption([FromBody] EcomVoucherRedemptionDataContract resquestDC)
        {
            EcomVoucherManager ecomVoucherManager = new EcomVoucherManager();
            HttpResponseMessage response = null;
            EcomVoucherRedemptionDataContract respecomvoucherDC = null;
            _loginRepository = new LoginDetailsUTCRepository();
            _companyRepository = new CompanyRepository();
            Login loginObj = null;
            int? companyId = 0;

            try
            {
                if (!ModelState.IsValid)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                string userName = string.Empty;

                if (HttpContext.Current != null && HttpContext.Current.User != null
                        && HttpContext.Current.User.Identity.Name != null)
                {
                    userName = HttpContext.Current.User.Identity.Name;

                    loginObj = _loginRepository.GetSingle(x => !string.IsNullOrEmpty(x.username) && x.username.ToLower().Equals(userName.ToLower()));

                    if (loginObj != null && loginObj.SponsorId != null)
                    {
                        tblCompany tblCompany = _companyRepository.GetSingle(x => x.BusinessUnitId == loginObj.SponsorId);
                        if (tblCompany != null)
                        {
                            companyId = tblCompany.CompanyId;
                        }
                        respecomvoucherDC = ecomVoucherManager.ManageEcomVoucherRedemption(resquestDC, companyId);

                        if (respecomvoucherDC != null && respecomvoucherDC.Sucess == true)
                        {

                            StatusDataContract structObj = new StatusDataContract(true, "Success", respecomvoucherDC);
                            response = new HttpResponseMessage(HttpStatusCode.OK)
                            {
                                Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                            };
                        }

                        else
                        {
                            StatusDataContract structObj = new StatusDataContract(false, "Unsuccess", respecomvoucherDC);
                            response = new HttpResponseMessage(HttpStatusCode.OK)
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

    }
}