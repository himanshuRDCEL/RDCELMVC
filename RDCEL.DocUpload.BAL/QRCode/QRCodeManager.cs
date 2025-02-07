using GraspCorn.Common.Constant;
using GraspCorn.Common.Helper;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RDCEL.DocUpload.BAL.Common;
using RDCEL.DocUpload.BAL.ServiceCall;
using RDCEL.DocUpload.BAL.ZohoCreatorCall;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DAL.Helper;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DataContract.ABBRegistration;
using RDCEL.DocUpload.DataContract.ZohoModel;

namespace RDCEL.DocUpload.BAL.QRCode
{
    public class QRCodeManager
    {

        #region Variable Declaration
        BusinessPartnerRepository businessPartnerRepository;
     
        DateTime currentDatetime = DateTime.Now.TrimMilliseconds();
      
        #endregion

        #region Get Sponsor Order by Id from database
        /// <summary>
        /// Method to Get Sponsor Order by Id
        /// </summary>       
        /// <returns></returns>   
        //public string GetOrderById(int orderId)
        //{
        //    exchangeOrderRepository = new ExchangeOrderRepository();
        //    string result = null;
        //    try
        //    {
        //        tblExchangeOrder tempexchangeOrderInfo = exchangeOrderRepository.GetSingle(x => x.Id.Equals(orderId));
        //        if (tempexchangeOrderInfo != null)
        //        {
        //            result = tempexchangeOrderInfo.ZohoSponsorOrderId;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        LibLogging.WriteErrorToDB("ProductManager", "AddOrder", ex);
        //    }

        //    return result;
        //}
        #endregion

        #region Add QR Code Info in database
        /// <summary>
        /// Method to add the QR Code Info
        /// </summary>       
        /// <returns></returns>   
        public int AddBusinessPartner(BusinessPartnerViewModel BusinessPartnerVM)
        {
            businessPartnerRepository = new BusinessPartnerRepository();
            int result = 0;
            try
            {
                tblBusinessPartner businessPartnerInfo = SetBusinessPartnerObjectJson(BusinessPartnerVM);
                {
                    businessPartnerRepository.Add(businessPartnerInfo);

                    businessPartnerRepository.SaveChanges();
                    result = businessPartnerInfo.BusinessPartnerId;
                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBRegManager", "AddABBReg", ex);
            }

            return result;
        }
        #endregion

        #region Update ABB Reg Info in database
        /// <summary>
        /// Method to Update ABB Reg Info
        /// </summary>       
        /// <returns></returns>   
        public int UpdateBusinessPartner(string url, int BusinessPartnerId)
        {
            businessPartnerRepository = new BusinessPartnerRepository();

            int result = 0;
            try
            {

                if (url != null && BusinessPartnerId > 0)
                {
                    tblBusinessPartner tempBusinessPartnerInfo = businessPartnerRepository.GetSingle(x => x.BusinessPartnerId.Equals(BusinessPartnerId));
                    if (tempBusinessPartnerInfo != null)
                    {
                        tempBusinessPartnerInfo.QRCodeURL = url;
                        tempBusinessPartnerInfo.ModifiedDate = currentDatetime;

                        businessPartnerRepository.Update(tempBusinessPartnerInfo);

                        businessPartnerRepository.SaveChanges();
                        result = tempBusinessPartnerInfo.BusinessPartnerId;
                    }

                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ProductManager", "AddOrder", ex);
            }

            return result;
        }
        #endregion

        #region set ABB Order obj
        /// <summary>
        /// Method to set Order info to table
        /// </summary>
        /// <param name="productOrderDataContract">productOrderDataContract</param>     
        public tblBusinessPartner SetBusinessPartnerObjectJson(BusinessPartnerViewModel BusinessPartnerVM)
        {
            tblBusinessPartner BusinessPartnerObj = null;
            try
            {
                if (BusinessPartnerVM != null)
                {
                    BusinessPartnerObj = new tblBusinessPartner();
                    BusinessPartnerObj.BusinessUnitId = BusinessPartnerVM.BusinessUnitId;
                   // BusinessPartnerObj.BusinessPartnerId = BusinessPartnerVM.BusinessPartnerId;
                    BusinessPartnerObj.Name = BusinessPartnerVM.Name;
                    BusinessPartnerObj.StoreCode = BusinessPartnerVM.StoreCode;
                    BusinessPartnerObj.Description = BusinessPartnerVM.Description;
                    BusinessPartnerObj.ContactPersonFirstName = BusinessPartnerVM.ContactPersonFirstName;
                    BusinessPartnerObj.ContactPersonLastName = BusinessPartnerVM.ContactPersonLastName;
                    BusinessPartnerObj.PhoneNumber = BusinessPartnerVM.PhoneNumber;
                    BusinessPartnerObj.Email = BusinessPartnerVM.Email;
                    BusinessPartnerObj.AddressLine1 = BusinessPartnerVM.AddressLine1;
                    BusinessPartnerObj.AddressLine2 = BusinessPartnerVM.AddressLine2;
                    BusinessPartnerObj.Pincode = BusinessPartnerVM.Pincode;
                    BusinessPartnerObj.City = BusinessPartnerVM.City;
                    BusinessPartnerObj.State = BusinessPartnerVM.State;                                     
                    BusinessPartnerObj.IsActive = true;
                    BusinessPartnerObj.CreatedDate = currentDatetime;

                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ABBRegManager", "SetABBRegObjectJson", ex);
            }
            return BusinessPartnerObj;
        }
        #endregion

        
    }
}
