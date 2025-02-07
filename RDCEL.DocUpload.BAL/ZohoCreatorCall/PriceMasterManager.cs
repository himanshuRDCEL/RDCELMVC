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

namespace RDCEL.DocUpload.BAL.ZohoCreatorCall
{
    public class PriceMasterManager
    {

        #region Get zoho Price master details 
        /// <summary>
        /// Method to get zoho Price master
        /// </summary>
        /// <param></param>
        /// <returns>model</returns>
        public List<PriceMasterData> GetAllPriceMaster()
        {
            PriceMasterListDataContract priceMasterDC = null;
            List<PriceMasterData> finalPriceMasterList = new List<PriceMasterData>();
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
                                                                                ReportLinkNameConstant.All_Price_Master_Report,
                                                                                finalQryString
                                                                                 ), Method.GET, null);

                    if (response != null)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            priceMasterDC = JsonConvert.DeserializeObject<PriceMasterListDataContract>(response.Content);
                        }
                    }
                    if (priceMasterDC != null && priceMasterDC != null && priceMasterDC.data.Count > 0 && response != null && response.StatusCode == HttpStatusCode.OK)
                        finalPriceMasterList.AddRange(priceMasterDC.data);
                    else
                        break;

                    frm = (i * 200) + 1;
                }



                //response = ZohoServiceCalls.Rest_InvokeZohoInvoiceServiceForPlainText(ZohoCreatorAPICallURL.GetURLFor(ZohoCreatorAPICallURL.GetReportWithoutFilter,
                //                                                                ReportLinkNameConstant.All_Price_Master_Report, null                                                                               
                //                                                                 ), Method.GET, null);

                //        if (response.StatusCode == HttpStatusCode.OK)
                //        {
                //          priceMasterDC = JsonConvert.DeserializeObject<PriceMasterListDataContract>(response.Content);

                //        }

                //    if (priceMasterDC != null && priceMasterDC != null && priceMasterDC.data.Count > 0 && response != null && response.StatusCode == HttpStatusCode.OK)

                //     finalPriceMasterList.AddRange(priceMasterDC.data);

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("EVCManager", "GetAllEVCApproved", ex);
            }
            return finalPriceMasterList;
        }

        #endregion
















    }
}
