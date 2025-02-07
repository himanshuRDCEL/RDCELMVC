using GraspCorn.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using RDCEL.DocUpload.BAL.ProcessAPI;
using RDCEL.DocUpload.BAL.ZohoCreatorCall;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DataContract._247aroundService;
using RDCEL.DocUpload.DataContract.Base;
using RDCEL.DocUpload.DataContract.ZohoModel;

namespace RDCEL.DocUpload.Web.API.Controllers.api
{
    public class QCServiceController : ApiController
    {
        #region Variable declaration
        ExchangeOrderRepository exchangeOrderRepository;
        CustomerDetailsRepository customerDetailsRepository;
        BusinessPartnerRepository businessPartnerRepository;
        BrandRepository brandRepository;
        ProductCategoryRepository productCatgRepository;
        ProductTypeRepository productTypeRepository;
        #endregion
        #region Submit Request For QC

        [System.Web.Http.HttpGet]
        public HttpResponseMessage SubmitRequest(string RegdNo)
        {
            AroundSyncManager aroundSyncManager = new AroundSyncManager();
            _247AroundDataContract aroundDataContract = null;
            _247ResponseDataContract AroundResponseDC = null;
            HttpResponseMessage response = null;
            exchangeOrderRepository = new ExchangeOrderRepository();
            customerDetailsRepository = new CustomerDetailsRepository();
            businessPartnerRepository = new BusinessPartnerRepository();
            brandRepository = new BrandRepository();
            productCatgRepository = new ProductCategoryRepository();
            productTypeRepository = new ProductTypeRepository();
            SponserData sponserObj = null;
            sponserObj = new SponserData();
            SponserManager sponserManager = new SponserManager();
            try
            {
                if (RegdNo != null)
                {
                    tblExchangeOrder exchObj = exchangeOrderRepository.GetSingle(x => x.RegdNo == RegdNo);

                    if (exchObj != null)
                    {
                        tblCustomerDetail custObj = customerDetailsRepository.GetSingle(x => x.Id == exchObj.CustomerDetailsId);
                        if (custObj != null && exchObj.ProductTypeId != null && exchObj.BrandId != null)
                        {
                            tblProductType productTypeObj = productTypeRepository.GetSingle(x => x.Id == exchObj.ProductTypeId);
                            if (productTypeObj != null && exchObj != null && custObj != null)
                            {
                                tblBrand brandObj = brandRepository.GetSingle(x => x.Id == exchObj.BrandId);
                                if (brandObj != null && productTypeObj != null && exchObj != null && custObj != null)
                                {
                                    tblProductCategory productCatObj = productCatgRepository.GetSingle(x => x.Id == productTypeObj.ProductCatId);
                                    if (productCatObj != null && brandObj != null && productTypeObj != null && exchObj != null && custObj != null)
                                    {
                                        aroundDataContract = aroundSyncManager.Set247AroundObj(exchObj, custObj, brandObj, productCatObj, productTypeObj);
                                        if (aroundDataContract != null)
                                        {
                                            AroundResponseDC = aroundSyncManager.ProcessServiceRequest(aroundDataContract, exchObj);
                                        }
                                        if (AroundResponseDC != null)
                                        {
                                            if (AroundResponseDC.data.result == "Success" && AroundResponseDC.data.response._247aroundBookingID != null)
                                            {

                                                StatusDataContract structObj = new StatusDataContract(true, "Success", AroundResponseDC);
                                                response = new HttpResponseMessage(HttpStatusCode.OK)
                                                {
                                                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                                };

                                            }
                                            else if (AroundResponseDC.data.result.Equals("Order ID Exists"))
                                            {
                                                StatusDataContract structObj = new StatusDataContract(false, "Order Already Exist", AroundResponseDC);
                                                response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                                {
                                                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                                };
                                            }

                                            else
                                            {
                                                StatusDataContract structObj = new StatusDataContract(false, "Invalid model request", AroundResponseDC);
                                                response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                                                {
                                                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                                };
                                            }
                                        }
                                    }

                                }
                                else
                                {
                                    StatusDataContract structObj = new StatusDataContract(false, "Brand Data not found.");
                                    response = new HttpResponseMessage(HttpStatusCode.NoContent)
                                    {
                                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                    };

                                }
                            }
                            else
                            {
                                StatusDataContract structObj = new StatusDataContract(false, "Product Data not found.");
                                response = new HttpResponseMessage(HttpStatusCode.NoContent)
                                {
                                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                                };

                            }
                        }
                        else
                        {
                            StatusDataContract structObj = new StatusDataContract(false, "Customer Data not found.");
                            response = new HttpResponseMessage(HttpStatusCode.NoContent)
                            {
                                Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                            };

                        }
                    }
                    else
                    {
                        StatusDataContract structObj = new StatusDataContract(false, "sponser Data not found.");
                        response = new HttpResponseMessage(HttpStatusCode.NoContent)
                        {
                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                        };

                    }
                }
                else
                {
                    StatusDataContract structObj = new StatusDataContract(false, "Regd id Not Given.");
                    response = new HttpResponseMessage(HttpStatusCode.NoContent)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };

                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("QCServiceController", "SubmitRequest", ex);
            }
            return response;
        }
        #endregion
    }
}