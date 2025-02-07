using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using RDCEL.DocUpload.Web.API.Models;
using RDCEL.DocUpload.Web.API.UserAuth;
using GraspCorn.Common.Helper;
using RDCEL.DocUpload.DAL.Helper;
using RDCEL.DocUpload.DAL.Repository;

namespace RDCEL.DocUpload.Web.API.UserAuth
{
    public class LoginDetails
    {
        #region Variable Declaration
        LoginDetailsUTCRepository _loginDetailsUTCRepository;
        DateTime currentDatetime = DateTime.Now.TrimMilliseconds();
        #endregion

        #region Get client credentials detail
        /// <summary>
        /// Method to get the list of Client credentials
        /// </summary>       
        /// <returns ClientCredentialsModel</returns>   
        public LoginModel GetClientCredential(string email, string pass)
        {
            _loginDetailsUTCRepository = new LoginDetailsUTCRepository();
            LoginModel login = null;
           // tblClientCredential tblClientCredential = new tblClientCredential();

            try
            {
                DataSet ds = _loginDetailsUTCRepository.GetLoginDetails(email, pass);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    login = GenericConversionHelper.DataTableToList<LoginModel>(ds.Tables[0]).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ClientCredentialInfo", "GetClientCredential", ex);
            }
            return login;
        }

        #endregion
    }
}