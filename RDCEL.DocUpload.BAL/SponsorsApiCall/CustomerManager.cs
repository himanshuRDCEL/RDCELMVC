using GraspCorn.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCEL.DocUpload.BAL.Common;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DataContract.SponsorModel;

namespace RDCEL.DocUpload.BAL.SponsorsApiCall
{
    public class CustomerManager
    {
        #region Variable Declaration
        CustomerDetailsRepository customerDetailsRepository;
        DateTime currentDatetime = DateTime.Now.TrimMilliseconds();

        #endregion

        #region Add Customer detail in database
        /// <summary>
        /// Method to add the customer
        /// </summary>       
        /// <returns></returns>   
        public int AddCustomer(ProductOrderDataContract productOrderDataContract)
        {
            customerDetailsRepository = new CustomerDetailsRepository();
            int result = 0;
            try
            {
                tblCustomerDetail customerInfo = SetCustomerObjectJson(productOrderDataContract);
                {
                    customerDetailsRepository.Add(customerInfo);

                    customerDetailsRepository.SaveChanges();
                    result = customerInfo.Id;
                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SamsungManager", "AddCustomer", ex);
            }
            return result;
        }
        #endregion

        #region set Customer detail obj
        /// <summary>
        /// Method to set Custome info to table
        /// </summary>
        /// <param name="productOrderDataContract">productOrderDataContract</param>     
        public tblCustomerDetail SetCustomerObjectJson(ProductOrderDataContract productOrderDataContract)
        {
            tblCustomerDetail customerObj = null;
            try
            {
                
                if (productOrderDataContract != null)
                {
                    customerObj = new tblCustomerDetail();
                    customerObj.FirstName = productOrderDataContract.FirstName;
                    customerObj.LastName = productOrderDataContract.LastName;
                    customerObj.ZipCode = productOrderDataContract.ZipCode;
                    customerObj.Address1 = productOrderDataContract.Address1;
                    customerObj.Address2 = productOrderDataContract.Address2;
                    customerObj.City = productOrderDataContract.City;
                    customerObj.Email = productOrderDataContract.Email;
                    customerObj.PhoneNumber = productOrderDataContract.PhoneNumber;
                    customerObj.IsActive = true;
                    customerObj.CreatedDate = currentDatetime;
                    customerObj.State = productOrderDataContract.State;
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("CustomerManager", "SetCstomerObjectJson", ex);
            }
            return customerObj;
        }
        #endregion
    }
}
