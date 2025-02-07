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
   public class MasterManager
    {

        #region Get all Sponser Category detail
        /// <summary>
        /// Method to get all Sponser Category from ZOho creator
        /// </summary>
        /// <returns></returns>
        public SponsorCategoryListDataContract GetAllCategory()
        {
            SponsorCategoryListDataContract SponserSubCategoryListDC = null;
            IRestResponse response = null;

            try
            {

                response = ZohoServiceCalls.Rest_InvokeZohoInvoiceServiceForPlainText(
                                               ZohoCreatorAPICallURL.GetURLFor(ZohoCreatorAPICallURL.GetReportWithoutFilter,
                                                                              ReportLinkNameConstant.All_Category_sponsor_report, null
                                                                                  ), Method.GET, null);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    SponserSubCategoryListDC = JsonConvert.DeserializeObject<SponsorCategoryListDataContract>(response.Content);
                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SponserManager", "GetAllSubCategory", ex);
            }
            return SponserSubCategoryListDC;
        }

        #endregion

        #region Get all Sponser Sub Category detail
        /// <summary>
        /// Method to get all Sponser Sub Category from ZOho creator
        /// </summary>
        /// <returns></returns>
        public SponserSubCategoryListDataContract GetAllSubCategory()
        {
            SponserSubCategoryListDataContract SponserSubCategoryListDC = null;
            IRestResponse response = null;

            try
            {

                response = ZohoServiceCalls.Rest_InvokeZohoInvoiceServiceForPlainText(
                                               ZohoCreatorAPICallURL.GetURLFor(ZohoCreatorAPICallURL.GetReportWithoutFilter,
                                                                              ReportLinkNameConstant.All_Sub_Category_sponsor_report, null
                                                                                  ), Method.GET, null);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    SponserSubCategoryListDC = JsonConvert.DeserializeObject<SponserSubCategoryListDataContract>(response.Content);
                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SponserManager", "GetAllSubCategory", ex);
            }
            return SponserSubCategoryListDC;
        }

        #endregion

        #region Get all Brand detail
        /// <summary>
        /// Method to get all Brand from ZOho creator
        /// </summary>
        /// <returns></returns>
        public BrandMasterListDataContract GetAllBrand()
        {
            BrandMasterListDataContract brandMasterListDC = null;
            IRestResponse response = null;

            try
            {

                response = ZohoServiceCalls.Rest_InvokeZohoInvoiceServiceForPlainText(
                                               ZohoCreatorAPICallURL.GetURLFor(ZohoCreatorAPICallURL.GetReportWithoutFilter,
                                                                              ReportLinkNameConstant.All_Brand_Master_Report, null
                                                                                  ), Method.GET, null);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    brandMasterListDC = JsonConvert.DeserializeObject<BrandMasterListDataContract>(response.Content);
                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("MasterManager", "GetAllBrand", ex);
            }
            return brandMasterListDC;
        }

        #endregion

        #region Get all Product Size
        /// <summary>
        /// Method to get all Product Size from ZOho creator
        /// </summary>
        /// <returns></returns>
        public ProductSizeListDataContract GetAllProductSize()
        {
            ProductSizeListDataContract productSizeListDC = null;
            IRestResponse response = null;

            try
            {

                response = ZohoServiceCalls.Rest_InvokeZohoInvoiceServiceForPlainText(
                                               ZohoCreatorAPICallURL.GetURLFor(ZohoCreatorAPICallURL.GetReportWithoutFilter,
                                                                              ReportLinkNameConstant.Product_Size_Report, null
                                                                                  ), Method.GET, null);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    productSizeListDC = JsonConvert.DeserializeObject< ProductSizeListDataContract > (response.Content);
                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("MasterManager", "GetAllProductSize", ex);
            }
            return productSizeListDC;
        }

        #endregion

        #region Get all Store Code
        /// <summary>
        /// Method to get all Store Code from ZOho creator
        /// </summary>
        /// <returns></returns>
        public StoreCodeListDataContract GetAllStoreCode()
        {
            StoreCodeListDataContract storeCodeListDC = null;
            IRestResponse response = null;

            try
            {

                response = ZohoServiceCalls.Rest_InvokeZohoInvoiceServiceForPlainText(
                                               ZohoCreatorAPICallURL.GetURLFor(ZohoCreatorAPICallURL.GetReportWithoutFilter,
                                                                              ReportLinkNameConstant.Store_Code_Master_Report, null
                                                                                  ), Method.GET, null);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    storeCodeListDC = JsonConvert.DeserializeObject<StoreCodeListDataContract>(response.Content);
                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("MasterManager", "GetAllProductSize", ex);
            }
            return storeCodeListDC;
        }

        /// <summary>
        /// Method to get all Store Detail from ZOho creator by code
        /// </summary>
        /// <returns>StoreCodeListDataContract</returns>
        public StoreCodeListDataContract GetAllStoreCodeByCode(string storeCode)
        {
            StoreCodeListDataContract storeCodeListDC = null;
            IRestResponse response = null;

            try
            {

                response = ZohoServiceCalls.Rest_InvokeZohoInvoiceServiceForPlainText(
                                               ZohoCreatorAPICallURL.GetURLFor(ZohoCreatorAPICallURL.GetUrlWithFilter,
                                                                              ReportLinkNameConstant.Store_Code_Master_Report, FilterConstant.Store_Filter_By_Code.Replace("[StoreCode]", storeCode)
                                                                                  ), Method.GET, null);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    storeCodeListDC = JsonConvert.DeserializeObject<StoreCodeListDataContract>(response.Content);
                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("MasterManager", "GetAllStoreCodeByCode", ex);
            }
            return storeCodeListDC;
        }
        #endregion
    }
}
