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
using RDCEL.DocUpload.DataContract.ZohoModel;
using RDCEL.DocUpload.DataContract.ZohoSyncModel;

namespace RDCEL.DocUpload.BAL.ZohoCreatorCall
{
    public class CommonManager
    {
        #region Get zoho Pincode master list 
        /// <summary>
        /// Method to get zoho Pincode master list 
        /// </summary>
        /// <param></param>
        /// <returns>model</returns>
        public List<PinCodeMaster> GetPincodeMasterList()
        {
            PincodeMasterListDataContract pincodeMasterListDC = null;          
            List<PinCodeMaster> finalPincodeMasterList = new List<PinCodeMaster>();
            IRestResponse response = null;
            int limit = 200;
            int frm = 1;
            string qryString = FilterConstant.Sponser_filter;
            string finalQryString = string.Empty;
            try
            {
                for (int i = 1; i < limit; i++)
                {
                    finalQryString = qryString.Replace("[FromCount]", frm.ToString());

                    response = ZohoServiceCalls.Rest_InvokeZohoInvoiceServiceForPlainText(ZohoCreatorAPICallURL.GetURLFor(ZohoCreatorAPICallURL.GetUrlWithFilter,
                                                                                ReportLinkNameConstant.Pin_Code_Master_Report,
                                                                                finalQryString
                                                                                 ), Method.GET, null);

                    if (response != null)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            pincodeMasterListDC = JsonConvert.DeserializeObject<PincodeMasterListDataContract>(response.Content);
                        }
                    }
                    if (pincodeMasterListDC != null && pincodeMasterListDC != null && pincodeMasterListDC.data.Count > 0 && response != null && response.StatusCode == HttpStatusCode.OK)

                        finalPincodeMasterList.AddRange(pincodeMasterListDC.data);
                    else
                        break;

                    frm = (i * 200)+1;
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ZohoCommonManager", "GetPincodeMasterList", ex);
            }

            return finalPincodeMasterList;
        }

        #endregion     

    }
}
