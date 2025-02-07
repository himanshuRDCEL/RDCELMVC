using GraspCorn.Common.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using RDCEL.DocUpload.BAL.ABBRegistration;
using RDCEL.DocUpload.BAL.SponsorsApiCall;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DataContract.ABBRedemption;
using RDCEL.DocUpload.DataContract.ABBRegistration;
using RDCEL.DocUpload.DataContract.Base;
using RDCEL.DocUpload.DataContract.MasterModel;
using RDCEL.DocUpload.DataContract.ProductTaxonomy;
using RDCEL.DocUpload.DataContract.SponsorModel;

namespace RDCEL.DocUpload.Web.API.Controllers.api
{
    [Authorize]
    public class ABBController : BaseApiController
    {

        #region Variable declaration
        LoginDetailsUTCRepository _loginRepository;
        ABBOrderManager _aBBOrderManager;
        ABBRegistrationRepository _aBBRegistrationRepository;
        ABBPlanMasterRepository _abbPlanMaster;
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

        [HttpGet]
        public HttpResponseMessage GetProductCategory()
        {
            _aBBOrderManager = new ABBOrderManager();
            HttpResponseMessage response = null;
            ProductCategoryDataContract productCategoryDC = null;

            string username = string.Empty;
            try
            {
                if (HttpContext.Current != null && HttpContext.Current.User != null
                            && HttpContext.Current.User.Identity.Name != null)
                {
                    username = HttpContext.Current.User.Identity.Name;
                    if (!string.IsNullOrEmpty(username))
                    {

                        List<ProductCategory> productCategories = _aBBOrderManager.GetCategoryListByBUId(username);
                        if (productCategories != null && productCategories.Count > 0)
                        {
                            productCategoryDC = new ProductCategoryDataContract();
                            productCategoryDC.ProductsCategory = productCategories;
                        }
                    }
                }
                else
                {
                    StatusDataContract structObj = new StatusDataContract(false, "Token Not Verified");
                    response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                    return response;
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
                    StatusDataContract structObj = new StatusDataContract(false, "No data found");
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

        [HttpGet]
        public HttpResponseMessage GetProductType(int catid)
        {
            _abbPlanMaster = new ABBPlanMasterRepository();
            _aBBOrderManager = new ABBOrderManager();
            HttpResponseMessage response = null;
            ProductTypeDataContract productTypeDataContract = null;
            string username = string.Empty;
            _loginRepository = new LoginDetailsUTCRepository();
            Login tblLoginObj = null;
            List<tblABBPlanMaster> planmasterObj = new List<tblABBPlanMaster>();
            int Buid = 0;
            try
            {
                if (HttpContext.Current != null && HttpContext.Current.User != null
                            && HttpContext.Current.User.Identity.Name != null)
                {
                    username = HttpContext.Current.User.Identity.Name;
                    if (!string.IsNullOrEmpty(username) && catid > 0)
                    {
                        tblLoginObj = _loginRepository.GetSingle(x => x.username == username && x.SponsorId != null);
                        if(tblLoginObj!=null)
                        {
                            Buid = Convert.ToInt32(tblLoginObj.SponsorId);
                           
                        }
                        List<ProductType> productCategories = _aBBOrderManager.GetProductTypeListByBUId(catid, Buid);
                        if (productCategories != null && productCategories.Count > 0)
                        {
                            productTypeDataContract = new ProductTypeDataContract();
                            productTypeDataContract.ProductsType = productCategories;
                        }
                    }
                }
                else
                {
                    StatusDataContract structObj = new StatusDataContract(false, "Token Not Verified");
                    response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                    return response;
                }
                if (productTypeDataContract != null)
                {
                    StatusDataContract structObj = new StatusDataContract(true, "Success", productTypeDataContract);
                    response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                }
                else
                {
                    StatusDataContract structObj = new StatusDataContract(false, "No data found");
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

        [HttpGet]
        public HttpResponseMessage GetPlanDetails(int productCatId, int productSubCatId, string productPrice)
        {
            _aBBOrderManager = new ABBOrderManager();
            HttpResponseMessage response = null;

            string username = string.Empty;
            try
            {
                List<abbplanmaster> abbplanmasterDCList = null;
                if (HttpContext.Current != null && HttpContext.Current.User != null
                            && HttpContext.Current.User.Identity.Name != null)
                {
                    username = HttpContext.Current.User.Identity.Name;
                    if (!string.IsNullOrEmpty(username) && productCatId > 0 && productSubCatId > 0)
                    {

                        abbplanmasterDCList = _aBBOrderManager.Getabbplandetails(productCatId, productSubCatId, productPrice, username);

                    }
                }
                else
                {
                    StatusDataContract structObj = new StatusDataContract(false, "Token Not Verified");
                    response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                    return response;
                }
                if (abbplanmasterDCList != null && abbplanmasterDCList.Count > 0)
                {
                    StatusDataContract structObj = new StatusDataContract(true, "Success", abbplanmasterDCList);
                    response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                }
                else
                {
                    StatusDataContract structObj = new StatusDataContract(false, "No data found");
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

        [HttpGet]
        public HttpResponseMessage GetPlanPriceDetails(int productCatId, int productSubCatId, string productPrice)
        {
            _aBBOrderManager = new ABBOrderManager();
            HttpResponseMessage response = null;
            plandetail plan = null;
            string username = string.Empty;
            try
            {
                //List<abbplanmaster> abbplanmasterDCList = null;
                if (HttpContext.Current != null && HttpContext.Current.User != null
                            && HttpContext.Current.User.Identity.Name != null)
                {
                    username = HttpContext.Current.User.Identity.Name;
                    if (!string.IsNullOrEmpty(username) && productCatId > 0 && productSubCatId > 0)
                    {

                        plan = _aBBOrderManager.GetabbPlanPrice(productCatId, productSubCatId, productPrice, username);

                    }
                }
                else
                {
                    StatusDataContract structObj = new StatusDataContract(false, "Token Not Verified");
                    response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                    return response;
                }
                if (plan != null)
                {
                    StatusDataContract structObj = new StatusDataContract(true, "Success", plan);
                    response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                }
                else
                {
                    StatusDataContract structObj = new StatusDataContract(false, "No data found");
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

        [HttpGet]
        public HttpResponseMessage GetYourBrandDetails()
        {
            _aBBOrderManager = new ABBOrderManager();
            HttpResponseMessage response = null;

            string username = string.Empty;
            try
            {
                BrandName brandName = null;

                if (HttpContext.Current != null && HttpContext.Current.User != null
                            && HttpContext.Current.User.Identity.Name != null)
                {
                    username = HttpContext.Current.User.Identity.Name;
                    if (!string.IsNullOrEmpty(username))
                    {
                        brandName = _aBBOrderManager.GetYourBrandName(username);
                        //abbplanmasterDCList = _aBBOrderManager.Getabbplandetails(productCatId, productSubCatId, productPrice, username);

                    }
                }
                else
                {
                    StatusDataContract structObj = new StatusDataContract(false, "Token Not Verified");
                    response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                    return response;
                }
                if (brandName != null)
                {
                    StatusDataContract structObj = new StatusDataContract(true, "Success", brandName);
                    response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                }
                else
                {
                    StatusDataContract structObj = new StatusDataContract(false, "No data found");
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

        [HttpGet]
        public HttpResponseMessage GetModelNumbers(int productCatId, int productSubCatId)
        {
            _aBBOrderManager = new ABBOrderManager();

            HttpResponseMessage response = null;
            ModelNumberDataContract modelNumberDataContract = null;

            string username = string.Empty;
            try
            {


                if (HttpContext.Current != null && HttpContext.Current.User != null
                            && HttpContext.Current.User.Identity.Name != null)
                {
                    username = HttpContext.Current.User.Identity.Name;
                    if (!string.IsNullOrEmpty(username) && productCatId > 0 && productSubCatId > 0)
                    {
                        List<ModelNumberModel> modelNumberModel = _aBBOrderManager.GetListOfModelNumbers(username, productCatId, productSubCatId);
                        if (  modelNumberModel != null && modelNumberModel.Count > 0)
                        {
                            modelNumberDataContract = new ModelNumberDataContract();
                            modelNumberDataContract.modelNumberModels = modelNumberModel;
                        }
                    }
                }
                else
                {
                    StatusDataContract structObj = new StatusDataContract(false, "Token Not Verified");
                    response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                    return response;
                }
                if (modelNumberDataContract != null)
                {
                    StatusDataContract structObj = new StatusDataContract(true, "Success", modelNumberDataContract);
                    response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                }
                else
                {
                    StatusDataContract structObj = new StatusDataContract(false, "No data found");
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

        [HttpGet]
        public HttpResponseMessage GetAllBrandDetails(int ProdCatId)
        {
            _aBBOrderManager = new ABBOrderManager();
            _loginRepository = new LoginDetailsUTCRepository();
            HttpResponseMessage response = null;

            string username = string.Empty;
            try
            {
                if (ProdCatId > 0)
                {
                    List<BrandName> brandName = null;

                    if (HttpContext.Current != null && HttpContext.Current.User != null
                                && HttpContext.Current.User.Identity.Name != null)
                    {
                        username = HttpContext.Current.User.Identity.Name;
                        if (!string.IsNullOrEmpty(username))
                        {
                            brandName = _aBBOrderManager.GetAllBrandName(username, ProdCatId);
                            //abbplanmasterDCList = _aBBOrderManager.Getabbplandetails(productCatId, productSubCatId, productPrice, username);
                        }
                    }

                    else
                    {
                        StatusDataContract structObj = new StatusDataContract(false, "Token Not Verified");
                        response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                        {
                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                        };
                        return response;
                    }
                    if (brandName != null)
                    {
                        StatusDataContract structObj = new StatusDataContract(true, "Success", brandName);
                        response = new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                        };
                    }
                    else
                    {
                        StatusDataContract structObj = new StatusDataContract(false, "No data found");
                        response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                        {
                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                        };

                    }
                }
                else
                {
                    StatusDataContract structObj = new StatusDataContract(false, "Product category id missing");
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

        [HttpPost]

        public HttpResponseMessage AbbOrderPlace(ABBOrderRequestDataContract aBBOrderRequestDataContract)
        {
            HttpResponseMessage response = null;
            ABBPlanTransactionResponse TransactioResponseforPlan = new ABBPlanTransactionResponse();
            ABBRegManager ABBRegInfo = new ABBRegManager();
            if (!ModelState.IsValid)
            {
                var result = new List<Error>();
                var erroneousFields = ModelState.Where(ms => ms.Value.Errors.Any())
                                                .Select(x => new { x.Key, x.Value.Errors });

                foreach (var erroneousField in erroneousFields)
                {
                    var fieldKey = erroneousField.Key;
                    //aBB Order Request Data Contract 27
                    //String s = "ABCDEF";
                    int start = 0;
                    int length = 28;
                    fieldKey = fieldKey.Remove(start, length);
                    var fieldErrors = erroneousField.Errors.Select(error => new Error(fieldKey, error.ErrorMessage));
                    result.AddRange(fieldErrors);
                }
                //string errormsg = ModelState.;
                StatusDataContract structObj = new StatusDataContract(false, "Invalid Request Object", result);
                response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                };
                return response;
            }
            if(aBBOrderRequestDataContract is null)
            {
                StatusDataContract structObj = new StatusDataContract(false, "Null object requested");
                response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                };
                return response;
            }

            _aBBOrderManager = new ABBOrderManager();

            ABBOrderResponseDataContract abbOrderResponseDC = null;

            _aBBRegistrationRepository = new ABBRegistrationRepository();
            string username = string.Empty;
            try
            {
                if (HttpContext.Current != null && HttpContext.Current.User != null
                            && HttpContext.Current.User.Identity.Name != null)
                {
                    username = HttpContext.Current.User.Identity.Name;
                    if (!string.IsNullOrEmpty(username))
                    {
                        abbOrderResponseDC = _aBBOrderManager.CreateAbbOrder(aBBOrderRequestDataContract, username);
                    }
                }
                else
                {
                    StatusDataContract structObj = new StatusDataContract(false, "Token Not Verified");
                    response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                    return response;
                }
                if (abbOrderResponseDC != null)
                {
                    if (abbOrderResponseDC.RequestErrorMessage != string.Empty )
                    {
                        StatusDataContract structObj = new StatusDataContract(false, "Invalid Input", abbOrderResponseDC.RequestErrorMessage);
                        response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                        {
                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                        };
                    }
                    else
                    {

                        StatusDataContract structObj = new StatusDataContract(true, "Success", abbOrderResponseDC);
                        response = new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                        };
                    }

                }
                else
                {
                    StatusDataContract structObj = new StatusDataContract(false, "No data found");
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


        [HttpGet]
        public HttpResponseMessage GetRedemptionData(string RegdNo)
        {
            _aBBOrderManager = new ABBOrderManager();
            HttpResponseMessage response = null;
            ABBRedemptionDataContract abbredemptionDC = new ABBRedemptionDataContract();
            try
            {
                if (!string.IsNullOrEmpty(RegdNo))
                {
                    abbredemptionDC = _aBBOrderManager.GetRedemptionData(RegdNo);
                    if (abbredemptionDC != null)
                    {
                        if (!string.IsNullOrEmpty(abbredemptionDC.ErrorMessage))
                        {
                            StatusDataContract structObj = new StatusDataContract(false, "Invalid Input", abbredemptionDC.ErrorMessage);
                            response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                            {
                                Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                            };
                        }
                        else
                        {

                            StatusDataContract structObj = new StatusDataContract(true, "Success", abbredemptionDC);
                            response = new HttpResponseMessage(HttpStatusCode.OK)
                            {
                                Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                            };
                        }

                    }
                    else
                    {
                        StatusDataContract structObj = new StatusDataContract(false, "No data found");
                        response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                        {
                            Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                        };

                    }
                }
                else
                {
                    StatusDataContract structObj = new StatusDataContract(false, "Please provide registration number");
                    response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                }
            }
            catch(Exception ex)
            {
                StatusDataContract structObj = new StatusDataContract(false, ex.Message);
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json")) //new StringContent("error"),                    
                };
            }
            return response;
        }
        public class Error
        {
            public Error(string key, string message)
            {
                Key = key;
                Message = message;
            }

            public string Key { get; set; }
            public string Message { get; set; }
        }


    }
}