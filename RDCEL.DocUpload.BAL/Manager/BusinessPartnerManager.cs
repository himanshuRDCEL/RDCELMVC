using GraspCorn.Common.Enums;
using GraspCorn.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DataContract.ABBRegistration;

namespace RDCEL.DocUpload.BAL.Manager
{
    public class BusinessPartnerManager
    {
        BusinessPartnerRepository _businessPartnerRepository;


        /// <summary>
        /// Method to Update ABB Reg Info
        /// </summary>       
        /// <returns></returns>   
        public BusinessPartnerViewModel GetUsersByIdPassword(string username, string password)
        {
            _businessPartnerRepository = new BusinessPartnerRepository();
            BusinessPartnerViewModel bpViewModel = new BusinessPartnerViewModel();
            try
            {
                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    tblBusinessPartner tempBusinessPartnerInfo = _businessPartnerRepository.GetSingle(x => x.IsActive == true && x.IsExchangeBP == true
                    && x.AssociateCode != null
                    && (x.Email != null && x.Email.Equals(username.Trim()))
                    && (x.BPPassword != null && x.BPPassword.Equals(password)));
                    if (tempBusinessPartnerInfo != null &&tempBusinessPartnerInfo.BusinessUnitId!=Convert.ToInt32(BusinessUnitEnum.Bosch))
                    {
                        bpViewModel = GenericMapper<tblBusinessPartner, BusinessPartnerViewModel>.MapObject(tempBusinessPartnerInfo);
                    }

                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("BusinessPartnerManager", "GetUsersByIdPassword", ex);
            }

            return bpViewModel;
        }
    }
}
