using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RDCEL.DocUpload.DataContract.ABBRegistration;

namespace RDCEL.DocUpload.Web.API.Helpers
{
    public class SessionHelper
    {
        public BusinessPartnerViewModel LoggedUserInfo { get; set; }

        /// <summary>
        /// intializes session helper
        /// </summary>
        public SessionHelper()
        {
            LoggedUserInfo = new BusinessPartnerViewModel();
        }

       

    }
}