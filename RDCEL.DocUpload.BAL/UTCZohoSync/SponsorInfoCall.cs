using GraspCorn.Common.Constant;
using GraspCorn.Common.Helper;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RDCEL.DocUpload.BAL.ServiceCall;
using RDCEL.DocUpload.BAL.ZohoCreatorCall;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DataContract.ZohoModel;

namespace RDCEL.DocUpload.BAL.UTCZohoSync
{
    public class SponsorInfoCall
    {
        #region Variable Declaration   
        MasterManager _masterManager;
        ExchangeOrderRepository sponserRepository;
        CustomerDetailsRepository customerDetailsRepository;
        ProductTypeRepository productTypeRepository;
        BrandRepository brandRepository;
        DateTime currentDatetime = DateTime.Now.TrimMilliseconds();
        #endregion


        #region Add Sponsor details into DB
        /// <summary>
        /// Method to Add Sponsor
        /// </summary>
        /// <param></param>
        /// <returns>model</returns>

        public int AddSponsorInfotoDB(SponserData sponserDataObj, int custId)
        {
            sponserRepository = new ExchangeOrderRepository();
            int result = 0;
            try
            {
                tblExchangeOrder sponsorInfo = SetSponsorInfoObject(sponserDataObj, custId);

                if (sponsorInfo != null)
                {
                    sponserRepository.Add(sponsorInfo);
                    sponserRepository.SaveChanges();
                    result = sponsorInfo.Id;
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SponsorInfoCall", "AddSponsorInfotoDB", ex);
            }
            return result;

        }


        public tblExchangeOrder SetSponsorInfoObject(SponserData sponserDataObj, int custId)
        {
            _masterManager = new MasterManager();
            tblExchangeOrder sponsorInfo = null;
            productTypeRepository = new ProductTypeRepository();
            brandRepository = new BrandRepository();
            try
            {
                if (sponserDataObj != null)
                {
                    sponsorInfo = new tblExchangeOrder();

                    sponsorInfo.ZohoSponsorOrderId = sponserDataObj.ID;
                    sponsorInfo.CustomerDetailsId = custId;
                    sponsorInfo.OrderStatus = sponserDataObj.Exchange_Status;
                    sponsorInfo.SponsorOrderNumber = sponserDataObj.Sp_Order_No;
                    sponsorInfo.Sweetener = !string.IsNullOrEmpty(sponserDataObj.Sweetener_Bonus_Amount_By_Sponsor)?  Convert.ToDecimal(sponserDataObj.Sweetener_Bonus_Amount_By_Sponsor): 0;
                    if (sponserDataObj.New_Product_Technology != null)
                    {
                        if (sponserDataObj.New_Product_Technology.ID != null)
                        {
                            SubCategoryData catType = _masterManager.GetAllSubCategory().data.FirstOrDefault(x => x.ID.Equals(sponserDataObj.New_Product_Technology.ID));
                            if (catType != null)
                            {
                                string zohoProductSubCategoryCode = catType.Sub_Product_Technology;
                                tblProductType productType = productTypeRepository.GetSingle(x => x.Code.ToLower().Equals(zohoProductSubCategoryCode.ToLower()));

                                if (productType != null)
                                    sponsorInfo.ProductTypeId = productType.Id;
                            }
                        }

                    }
                    if (sponserDataObj.Prexo_Brand != null)
                    {
                        tblBrand brand = brandRepository.GetSingle(x => x.Name.ToLower().Equals(sponserDataObj.Prexo_Brand.ToLower()));
                        if (brand != null)
                            sponsorInfo.BrandId = brand.Id;
                    }


                    //sponsorInfo.ProductCondition = sponserDataObj.;
                    //sponsorInfo.CompanyName = sponserDataObj.;
                    //sponsorInfo.EstimatedDeliveryDate = sponserDataObj.;
                    //sponsorInfo.ExchPriceCode = sponserDataObj.;
                    //sponsorInfo. = sponserDataObj.;
                    //sponsorInfo. = sponserDataObj.;

                    sponsorInfo.IsActive = true;
                    sponsorInfo.CreatedDate = currentDatetime;
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SponsorInfoCall", "SetSponsorInfoObject", ex);
            }
            return sponsorInfo;
        }

        #endregion

        #region Add Customer details into DB
        /// <summary>
        /// Method to Add Customer detail
        /// </summary>
        /// <param></param>
        /// <returns>model</returns>

        public int AddCustomerInfotoDB(SponserData sponserDataObj)
        {
            customerDetailsRepository = new CustomerDetailsRepository();
            int result = 0;
            try
            {
                tblCustomerDetail customerDetailInfo = SetCustomerInfoObject(sponserDataObj);

                if (customerDetailInfo != null)
                {
                    customerDetailsRepository.Add(customerDetailInfo);
                    customerDetailsRepository.SaveChanges();
                    result = customerDetailInfo.Id;
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SponsorInfoCall", "AddCustomerInfotoDB", ex);
            }
            return result;

        }


        public tblCustomerDetail SetCustomerInfoObject(SponserData sponserDataObj)
        {
            tblCustomerDetail customerDetailInfo = null;
            try
            {
                if (sponserDataObj != null)
                {
                    customerDetailInfo = new tblCustomerDetail();

                    customerDetailInfo.FirstName = sponserDataObj.Customer_Name.first_name;
                    customerDetailInfo.LastName = sponserDataObj.Customer_Name.last_name;
                    customerDetailInfo.Address1 = sponserDataObj.Customer_Address_1;
                    customerDetailInfo.Address2 = sponserDataObj.Customer_Address_2;
                    customerDetailInfo.PhoneNumber = sponserDataObj.Customer_Mobile;
                    customerDetailInfo.ZipCode = sponserDataObj.Customer_Pincode;
                    customerDetailInfo.City = sponserDataObj.Customer_City;
                    customerDetailInfo.State = sponserDataObj.Customer_State_Name;
                    customerDetailInfo.Email = sponserDataObj.Customer_Email_Address;
                    customerDetailInfo.IsActive = true;
                    customerDetailInfo.CreatedDate = currentDatetime;
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SponsorInfoCall", "SetCustomerInfoObject", ex);
            }
            return customerDetailInfo;
        }

        #endregion












    }
}
