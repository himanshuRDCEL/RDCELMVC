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
using RDCEL.DocUpload.DAL.Helper;
using RDCEL.DocUpload.DataContract.ZohoModel;

namespace RDCEL.DocUpload.BAL.SponsorsApiCall
{
  public   class EVCZohoRegistraionManager
    {

        Logging logging;

        #region Post zoho EVC detail

        /// <summary>
        /// Method to add EVC to Zoho EVC form
        /// </summary>
        /// <param name="EVCZohoRegistrationDC"></param>
        /// <returns></returns>
        public EVCRegistrationResponse AddEVC(EVCZohoRegistrationDataContract EVCZohoRegistrationDC)
        {

            logging = new Logging();
            EVCRegistrationResponse evcRegistrationResponse = null;
            try
            {
                if (EVCZohoRegistrationDC != null)
                {
                    IRestResponse response = ZohoServiceCalls.Rest_InvokeZohoInvoiceServiceForPlainText(ZohoCreatorAPICallURL.GetURLFor(ZohoCreatorAPICallURL.AddDetails,
                                                                                   FormLinkNameConstant.EVC_Master_form,
                                                                                    null
                                                                                       ), Method.POST, EVCZohoRegistrationDC);


                    if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK)
                    {
                        evcRegistrationResponse = JsonConvert.DeserializeObject<EVCRegistrationResponse>(response.Content);
                        if (evcRegistrationResponse.code != 3000)
                        {
                            logging.WriteErrorToDB("EVCZohoRegistraionManager", "AddEVC", evcRegistrationResponse.data.ID, response);
                        }
                    }
                    else
                    {
                        logging.WriteErrorToDB("EVCZohoRegistraionManager", "AddEVC", evcRegistrationResponse.data.ID, response);
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("EVCZohoRegistraionManager", "AddEVC", ex);
            }
            return evcRegistrationResponse;
        }



        #endregion
    }
}
