using GraspCorn.Common.Constant;
using GraspCorn.Common.Helper;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RDCEL.DocUpload.BAL.ServiceCall;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DAL.Helper;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DataContract.Bizlog;
using RDCEL.DocUpload.DataContract.SponsorModel;
using RDCEL.DocUpload.DataContract.ZohoModel;
using GraspCorn.Common.Enums;
using RDCEL.DocUpload.DataContract.Voucher;

namespace RDCEL.DocUpload.BAL.ZohoCreatorCall
{
    public class SponserManager
    {
        Logging logging;
        SponserManager sponserManager;
        DateTime currentDatetime = DateTime.Now.TrimMilliseconds();
        MasterManager masterManager;


        #region Get all Sponser detail
        /// <summary>
        /// Method to get all Sponser information from ZOho creator
        /// </summary>
        /// <returns></returns>
        public List<SponserData> GetAllSponser()
        {
            SponserListDataContract SponserDC = null;
            List<SponserData> finalSponserList = new List<SponserData>();
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

                    response = ZohoServiceCalls.Rest_InvokeZohoInvoiceServiceForPlainText(
                                                   ZohoCreatorAPICallURL.GetURLFor(ZohoCreatorAPICallURL.GetUrlWithFilter,
                                                                                  ReportLinkNameConstant.Sponser_report,
                                                                                  finalQryString
                                                                                      ), Method.GET, null);
                    if (response != null)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            SponserDC = JsonConvert.DeserializeObject<SponserListDataContract>(response.Content);
                        }
                    }
                    if (SponserDC != null && SponserDC != null && SponserDC.data.Count > 0 && response != null && response.StatusCode == HttpStatusCode.OK)

                        finalSponserList.AddRange(SponserDC.data);
                    else
                        break;

                    frm = (i * 200) + 1;
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SponserManager", "GetAllSponser", ex);
            }
            return finalSponserList;
        }

        #endregion

        #region Get zoho Sponser  by Id 
        /// <summary>
        /// Method to get zoho Sponser  by Id 
        /// </summary>
        /// <param></param>
        /// <returns>model</returns>
        public SponserListDataContract GetSponserOrderById(string SponserOrderId)
        {

            SponserListDataContract sponserListDC = null;
            SponserFormRequestDataContract sponserRequestDC = new SponserFormRequestDataContract();
            try
            {
                if (SponserOrderId != null)
                {

                    IRestResponse response = ZohoServiceCalls.Rest_InvokeZohoInvoiceServiceForPlainText(ZohoCreatorAPICallURL.GetURLFor(ZohoCreatorAPICallURL.GetUrlWithFilter,
                                                                                    ReportLinkNameConstant.Sponser_report,
                                                                                     FilterConstant.SponserbyId_filter.Replace("[sponsorId]", SponserOrderId)
                                                                                        ), Method.GET, null);


                    if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK)
                    {
                        sponserListDC = JsonConvert.DeserializeObject<SponserListDataContract>(response.Content);

                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SponserManager", "GetSponserOrderById", ex);
            }
            return sponserListDC;
        }

        /// <summary>
        /// Method to get zoho Sponser  by Id 
        /// </summary>
        /// <param></param>
        /// <returns>model</returns>
        public SponsorOrderDetailForExchangeDataContract GetSponserOrderByIdDetailed(string SponserOrderId)
        {

            SponsorOrderDetailForExchangeDataContract sponserDC = null;
            SponserFormRequestDataContract sponserRequestDC = new SponserFormRequestDataContract();
            try
            {
                if (SponserOrderId != null)
                {

                    IRestResponse response = ZohoServiceCalls.Rest_InvokeZohoInvoiceServiceForPlainText(ZohoCreatorAPICallURL.GetURLFor(ZohoCreatorAPICallURL.GetUrlWithFilter,
                                                                                    ReportLinkNameConstant.Sponser_report,
                                                                                     FilterConstant.SponserbyIdForUpdate_filter.Replace("[sponsorId]", SponserOrderId)
                                                                                        ), Method.GET, null);


                    if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK)
                    {
                        sponserDC = JsonConvert.DeserializeObject<SponsorOrderDetailForExchangeDataContract>(response.Content);

                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SponserManager", "GetSponserOrderByIdDetailed", ex);
            }
            return sponserDC;
        }

        #endregion

        #region Get zoho Sponser  by SponsorOrderNo 
        /// <summary>
        /// Method to get zoho Sponser  by SponsorOrderNo 
        /// </summary>
        /// <param></param>
        /// <returns>model</returns>
        public SponserListDataContract GetSponserOrderByOrderNo(string zohoSponsorOrderNo)
        {

            SponserListDataContract sponserListDC = null;
            SponserFormRequestDataContract sponserRequestDC = new SponserFormRequestDataContract();
            try
            {
                if (zohoSponsorOrderNo != null)
                {

                    IRestResponse response = ZohoServiceCalls.Rest_InvokeZohoInvoiceServiceForPlainText(ZohoCreatorAPICallURL.GetURLFor(ZohoCreatorAPICallURL.GetUrlWithFilter,
                                                                                    ReportLinkNameConstant.Sponser_report,
                                                                                     FilterConstant.SponserbyOrderNo_filter.Replace("[sponsorOrderNo]", zohoSponsorOrderNo)
                                                                                        ), Method.GET, null);


                    if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK)
                    {
                        sponserListDC = JsonConvert.DeserializeObject<SponserListDataContract>(response.Content);

                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SponserManager", "GetSponserOrderByOrderNo", ex);
            }
            return sponserListDC;
        }

        #endregion

        #region Get zoho Sponser  by BizlogTicketNo
        /// <summary>
        /// Method to get zoho Sponser  by BizlogTicketNo
        /// </summary>
        /// <param></param>
        /// <returns>model</returns>
        public SponserListDataContract GetSponserOrderByBizlogTicketNo(string BizlogTicketNo)
        {
            SponserListDataContract sponserListDC = null;

            try
            {
                if (BizlogTicketNo != null)
                {

                    IRestResponse response = ZohoServiceCalls.Rest_InvokeZohoInvoiceServiceForPlainText(ZohoCreatorAPICallURL.GetURLFor(ZohoCreatorAPICallURL.GetUrlWithFilter,
                                                                                    ReportLinkNameConstant.Sponser_report,
                                                                                     FilterConstant.SponserbyOrderby_TicketNo_filter.Replace("[bizlogTicketNo]", BizlogTicketNo)
                                                                                        ), Method.GET, null);


                    if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK)
                    {
                        sponserListDC = JsonConvert.DeserializeObject<SponserListDataContract>(response.Content);

                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SponserManager", "GetSponserOrderByBizlogTicketNo", ex);
            }
            return sponserListDC;
        }

        #endregion

        #region Post and get zoho Sponser detail
        /// <summary>
        /// Method to Add Sponsor details ('Sponsor' form)
        /// </summary>
        /// <param></param>
        /// <returns>model</returns>
        public SponserFormResponseDataContract AddSponser(SponserDataContract sponserDataContract)
        {

            logging = new Logging();
            SponserFormResponseDataContract sponserResponseDC = null;
            SponserFormRequestDataContract sponserRequestDC = new SponserFormRequestDataContract();
            try
            {
                if (sponserDataContract != null)
                {
                    sponserRequestDC.data = sponserDataContract;

                    //IRestResponse response = ZohoServiceCalls.Rest_InvokeZohoInvoiceServiceForPlainText("https://creatorapp.zoho.com/api/v2/accountsperthsecurityservices/mobileapp/form/D_Runsheets", patrolRequestDC);
                    IRestResponse response = ZohoServiceCalls.Rest_InvokeZohoInvoiceServiceForPlainText(ZohoCreatorAPICallURL.GetURLFor(ZohoCreatorAPICallURL.AddDetails,
                                                                                   FormLinkNameConstant.Sponser_form,
                                                                                    null
                                                                                       ), Method.POST, sponserRequestDC);


                    if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK)
                    {
                        sponserResponseDC = JsonConvert.DeserializeObject<SponserFormResponseDataContract>(response.Content);
                        if (sponserResponseDC.code != 3000)
                        {
                            logging.WriteErrorToDB("SponserManager", "AddSponser", sponserDataContract.Sp_Order_No, response);
                        }
                    }
                    else
                    {
                        logging.WriteErrorToDB("SponserManager", "AddSponser", sponserDataContract.Sp_Order_No, response);
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SponserManager", "AddSponser", ex);
            }
            return sponserResponseDC;
        }



        #endregion

        #region set zoho Sponser detail
        /// <summary>
        /// Method to set details ('Sponser' form)
        /// </summary>
        /// <param></param>
        /// <returns>model</returns>
        public SponserDataContract SetSponsorObject(ProductOrderDataContract productOrderDataContract, tblBusinessUnit businessUnit)
        {
            masterManager = new MasterManager();
            SponserDataContract sponserObj = null;
            SponserSubCategoryListDataContract SponserSubCategoryListDC = null;
            SponsorCategoryListDataContract SponserCategoryListDC = null;
            BrandMasterListDataContract brandMasterListDC = null;
            ProductSizeListDataContract productSizeListDC = null;
            sponserManager = new SponserManager();
            BrandRepository sponserRepository = new BrandRepository();
            ProductTypeRepository productTypeRepository = new ProductTypeRepository();
            ProductCategoryRepository categoryRepository = new ProductCategoryRepository();
            PriceMasterRepository priceMasterRepository = new PriceMasterRepository();
            int PinCode = 0;
            try
            {
                if (productOrderDataContract != null)
                {
                    sponserObj = new SponserDataContract();
                    sponserObj.Sp_Order_No = productOrderDataContract.SponsorOrderNumber;
                    if (businessUnit != null)
                        sponserObj.Sponsor_Name = businessUnit.ZohoSponsorId; // "4186686000000687354";
                    else
                        sponserObj.Sponsor_Name = "4186686000000687354";

                    sponserObj.Regd_No = productOrderDataContract.RegdNo;
                    sponserObj.Upload_Date_Time = productOrderDataContract.UploadDateTime;
                    //sponserObj.Customer_Name = new CustomerName();
                    sponserObj.First_Name = productOrderDataContract.FirstName;
                    sponserObj.Last_Name = productOrderDataContract.LastName;
                    sponserObj.Store_Code = productOrderDataContract.StoreCode;
                    sponserObj.Customer_Pincode = productOrderDataContract.ZipCode;
                    sponserObj.Customer_Address_1 = productOrderDataContract.Address1;
                    sponserObj.Customer_Address_2 = productOrderDataContract.Address2;
                    sponserObj.Customer_City = productOrderDataContract.City;
                    if (!string.IsNullOrEmpty(productOrderDataContract.ZipCode))
                    {
                        PinCode = Convert.ToInt32(productOrderDataContract.ZipCode);
                        PinCodeRepository pinCodeRepository = new PinCodeRepository();
                        tblPinCode pinCode = pinCodeRepository.GetSingle(x => x.IsActive == true && x.ZipCode.Equals(PinCode));
                        sponserObj.Customer_State_Name = pinCode != null && !string.IsNullOrEmpty(pinCode.State) ? pinCode.State : null;
                    }
                    sponserObj.Customer_Email_Address = productOrderDataContract.Email;
                    sponserObj.Customer_Mobile = productOrderDataContract.PhoneNumber;
                    sponserObj.Sweetener_Bonus_Amount_By_Sponsor = productOrderDataContract.Bonus;
                    if (productOrderDataContract.QCDate != null && productOrderDataContract.StartTime != null && productOrderDataContract.EndTime != null)
                    {
                        sponserObj.Preferred_QC_DateTime = productOrderDataContract.QCDate + "  At " + productOrderDataContract.StartTime + "-" + productOrderDataContract.EndTime;
                    }
                    if (productOrderDataContract.IsDefferedSettlement==true)
                    {
                        sponserObj.Is_Deferred = "Yes";
                        sponserObj.Is_DtoC = "Yes";
                    }
                    else
                    {
                        sponserObj.Is_Deferred = "No";
                        sponserObj.Is_DtoC = "No";
                    }

                    ////mandatory////
                    //sponserObj.Actual_Prod_Qlty_at_time_of_QC = "P";
                    if (productOrderDataContract.ProductCondition == "1")
                    {
                        sponserObj.Cust_Declared_Qlty = "P";
                    }
                    else if (productOrderDataContract.ProductCondition == "2")
                    {
                        sponserObj.Cust_Declared_Qlty = "Q";
                    }
                    else if (productOrderDataContract.ProductCondition == "3")
                    {
                        sponserObj.Cust_Declared_Qlty = "R";
                    }
                    else
                    {
                        sponserObj.Cust_Declared_Qlty = "S";
                    }


                    if (productOrderDataContract.BrandId != 0)
                    {
                        tblBrand brandObj = sponserRepository.GetSingle(x => x.Id.Equals(productOrderDataContract.BrandId));
                        if (brandObj != null)
                        {

                            brandMasterListDC = masterManager.GetAllBrand();
                            if (brandMasterListDC != null)
                            {
                                if (brandMasterListDC.data != null && brandMasterListDC.data.Count > 0)
                                {

                                    BrandMaster brandData = brandMasterListDC.data.Find(x => x.Brand_Name.ToLower().Equals(brandObj.Name.ToLower()));
                                    if (brandData != null)
                                    {
                                        if (brandData.Brand_Name != "Others")
                                        {
                                            sponserObj.Brand_Type = "Premium";
                                        }
                                        else
                                        {
                                            sponserObj.Brand_Type = "Others";
                                        }
                                        sponserObj.Old_Brand = brandData.ID;
                                    }
                                }
                            }

                        }
                    }
                    if (productOrderDataContract.ProductTypeId != 0)
                    {
                        tblProductType productTypeObj = productTypeRepository.GetSingle(x => x.Id.Equals(productOrderDataContract.ProductTypeId));
                        if (productTypeObj != null)
                        {
                            tblProductCategory productCatObj = categoryRepository.GetSingle(x => x.Id.Equals(productTypeObj.ProductCatId));
                            if (productCatObj != null)
                            {
                                // fill Product Category
                                SponserCategoryListDC = masterManager.GetAllCategory();
                                if (SponserCategoryListDC != null)
                                {
                                    if (SponserCategoryListDC.data != null && SponserCategoryListDC.data.Count > 0)
                                    {
                                        CategoryData CategoryData = SponserCategoryListDC.data.Find(x => x.Product_Technology.ToLower().Equals(productCatObj.Code.ToLower()));
                                        if (CategoryData != null)
                                        {
                                            sponserObj.New_Prod_Group = CategoryData.ID;
                                        }
                                    }
                                }
                                // fill Product type
                                SponserSubCategoryListDC = masterManager.GetAllSubCategory();
                                string subcategory = null;
                                if (SponserSubCategoryListDC != null)
                                {
                                    if (SponserSubCategoryListDC.data != null && SponserSubCategoryListDC.data.Count > 0)
                                    {
                                        string category = null;

                                        if (productTypeObj.Code != "RF2" && productTypeObj.Code != "RF3")
                                        {
                                            if (productTypeObj.Code.Contains("RF2"))
                                                category = "RF2";
                                            else if (productTypeObj.Code.Contains("RF3"))
                                                category = "RF3";
                                            else if (productTypeObj.Code.Contains("TSM"))
                                            {

                                                category = "TSM";
                                            }
                                            else if (productTypeObj.Code.Contains("RSX"))
                                            {

                                                category = "RSX";
                                            }
                                            else if (productTypeObj.Code.Contains("RDC"))
                                            {

                                                category = "RDC";
                                            }
                                            else if (productTypeObj.Code.Contains("WDC"))
                                            {

                                                category = "WDC";
                                            }
                                            else
                                                category = Regex.Replace(productTypeObj.Code, @"[\d]", string.Empty);
                                        }
                                        else
                                        {
                                            category = productTypeObj.Code;
                                        }
                                        subcategory = category;
                                        SubCategoryData subCategoryData = SponserSubCategoryListDC.data.Find(x => x.Sub_Product_Technology.ToLower().Equals(category.ToLower()));
                                        if (subCategoryData != null)
                                        {
                                            sponserObj.New_Product_Technology = subCategoryData.ID;
                                        }
                                    }
                                }

                                // fill Product size

                                productSizeListDC = masterManager.GetAllProductSize();
                                if (productSizeListDC != null)
                                {
                                    if (productSizeListDC.data != null && productSizeListDC.data.Count > 0)
                                    {
                                        if (!String.IsNullOrEmpty(productTypeObj.Size))
                                        {
                                            string size = string.Empty;
                                            if (productTypeObj.ProductCatId == Convert.ToInt32(ProductCategoryEnum.Refrigerator))
                                            {
                                                if (productTypeObj.Code.Contains("RSX"))
                                                    size = productTypeObj.Code;
                                                else
                                                    size = productTypeObj.Code.Replace(subcategory, "");
                                            }
                                            else if (productTypeObj.ProductCatId == Convert.ToInt32(ProductCategoryEnum.Television))
                                            {
                                                size = productTypeObj.Code.Replace("TSM", "");
                                            }
                                            else if (productTypeObj.Code.Equals("WDC10+"))
                                            {
                                                size = productTypeObj.Code;
                                            }
                                            else
                                                size = Regex.Replace(productTypeObj.Code, "[^0-9.]", "");

                                            ProductSize productSize = productSizeListDC.data.Find(x => x.Size.ToLower().Equals(size.ToLower()));
                                            if (productSize != null)
                                            {
                                                sponserObj.Size = productSize.ID;
                                            }
                                            else
                                            {
                                                size = Regex.Replace(productTypeObj.Code, "[^0-9.]", "");
                                                productSize = productSizeListDC.data.Find(x => x.Size.ToLower().Equals(size.ToLower()));
                                                if (productSize != null)
                                                {
                                                    sponserObj.Size = productSize.ID;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            ProductSize productSize = productSizeListDC.data.Find(x => x.Size.ToLower().Equals("blank"));
                                            if (productSize != null)
                                            {
                                                sponserObj.Size = productSize.ID;
                                            }
                                        }
                                    }
                                }
                                //// fill Product type
                                //SponserSubCategoryListDC = masterManager.GetAllSubCategory();
                                //if (SponserSubCategoryListDC != null)
                                //{
                                //    if (SponserSubCategoryListDC.data != null && SponserSubCategoryListDC.data.Count > 0)
                                //    {
                                //        string category = null;

                                //        if (productTypeObj.Code != "RF2" && productTypeObj.Code != "RF3")
                                //        {
                                //            category = Regex.Replace(productTypeObj.Code, @"[\d]", string.Empty);
                                //        }
                                //        else
                                //        {
                                //            category = productTypeObj.Code;
                                //        }

                                //        SubCategoryData subCategoryData = SponserSubCategoryListDC.data.Find(x => x.Sub_Product_Technology.ToLower().Equals(category.ToLower()));
                                //        if (subCategoryData != null)
                                //        {
                                //            sponserObj.New_Product_Technology = subCategoryData.ID;
                                //        }
                                //    }
                                //}

                                //// fill Product size
                                //productSizeListDC = masterManager.GetAllProductSize();
                                //if (productSizeListDC != null)
                                //{
                                //    if (productSizeListDC.data != null && productSizeListDC.data.Count > 0)
                                //    {
                                //        string size = null;
                                //        if (!String.IsNullOrEmpty(productTypeObj.Size))
                                //        {
                                //            size = Regex.Replace(productTypeObj.Code, "[^0-9.]", "");
                                //            //size = Regex.Replace(productTypeObj.Code, @"[\d]", string.Empty);

                                //            ProductSize productSize = productSizeListDC.data.Find(x => x.Size.ToLower().Equals(size.ToLower()));
                                //            if (productSize != null)
                                //            {
                                //                sponserObj.Size = productSize.ID;
                                //            }
                                //        }
                                //    }
                                //}
                            }
                        }
                    }

                    string orderDate = currentDatetime.ToString("dd-MMMM-yyyy");
                    sponserObj.Order_Date = orderDate;
                    sponserObj.EVC_Status = "Not Allocated";
                    sponserObj.Order = "0";
                    sponserObj.Order_Type = "Exchange";
                    //sponserObj.Exchange = "Y";
                    //sponserObj.Exchange_Status = "Order Created";


                    // new fields  Added                 

                    sponserObj.Estimate_Delivery_Date = productOrderDataContract.EstimatedDeliveryDate;
                    if (productOrderDataContract.EstimatedDeliveryDate != null)
                    {
                        DateTime date = DateTime.ParseExact(productOrderDataContract.EstimatedDeliveryDate, "dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture).AddDays(1);
                        string SchedulePickupDate = date.ToString("dd-MMM-yyyy");

                        sponserObj.Expected_Pickup_Date = SchedulePickupDate;
                    }

                    sponserObj.Latest_Status = "0";
                    sponserObj.Order = "0";
                    sponserObj.Secondary_Order_Flag = "Not Yet Confirm";
                    sponserObj.Status_Reason = "Order created by Sponsor";

                    sponserObj.Tech_Evl_Required = "No";
                    sponserObj.Level_Of_Irritation = "1";
                    sponserObj.Nature_Of_Complaint = "Pick And Drop (One Way)";
                    sponserObj.Product_Category = "Home appliances";
                    sponserObj.Physical_Evolution = "No";
                    sponserObj.Date_Of_Complaint = orderDate;
                    sponserObj.Retailer_Phone_Number = "8652223816";
                    sponserObj.Alternate_Email = "logitics@digimart.co.in";
                    sponserObj.Problem_Description = "Exchange";
                    sponserObj.Is_Under_Warranty = "No";
                    sponserObj.Bulk_Mode = "No";

                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SponserManager", "SetSponsorObject", ex);
            }
            return sponserObj;
        }

        #endregion



        #region Post zoho Sponser Order Status Cancelled detail 
        /// <summary>
        /// Method to push status details ('Sponsor' form)
        /// </summary>
        /// <param></param>
        /// <returns>model</returns>
        public SponserFormResponseDataContract UpdateSponserOrderStatus(UpdateSponserOrderStatusDataContract sponserStatusDataContract)
        {

            SponserFormResponseDataContract sponserStatusResponseDC = null;
            UpdateSponserOrderStatusFormRequestDataContract sponserStatusRequestDC = new UpdateSponserOrderStatusFormRequestDataContract();
            try
            {
                if (sponserStatusDataContract != null)
                {
                    sponserStatusRequestDC.data = sponserStatusDataContract;

                    IRestResponse response = ZohoServiceCalls.Rest_InvokeZohoInvoiceServiceForPlainText(ZohoCreatorAPICallURL.GetURLFor(ZohoCreatorAPICallURL.GetUrlWithFilter,
                                                                                   ReportLinkNameConstant.Sponser_report,
                                                                                    FilterConstant.SponserbyIdForUpdate_filter.Replace("[sponsorId]", sponserStatusDataContract.ID)
                                                                                       ), Method.PATCH, sponserStatusRequestDC);


                    if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK)
                    {
                        sponserStatusResponseDC = JsonConvert.DeserializeObject<SponserFormResponseDataContract>(response.Content);

                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SponserManager", "AddSponserOrderStatus", ex);
            }
            return sponserStatusResponseDC;
        }


        /// <summary>
        /// Method to push pickup date details ('Sponsor' form)
        /// </summary>
        /// <param></param>
        /// <returns>model</returns>
        public SponserFormResponseDataContract UpdateSponserOrderEstimatePickupdate(UpdateExchangeOrderPickupDateDataContract sponserpickupDateDataContract)
        {

            SponserFormResponseDataContract sponserStatusResponseDC = null;
            UpdateSponserOrderPickupDateFormRequestDataContract sponserpickupRequestDC = new UpdateSponserOrderPickupDateFormRequestDataContract();
            try
            {
                if (sponserpickupDateDataContract != null)
                {
                    sponserpickupRequestDC.data = sponserpickupDateDataContract;

                    IRestResponse response = ZohoServiceCalls.Rest_InvokeZohoInvoiceServiceForPlainText(ZohoCreatorAPICallURL.GetURLFor(ZohoCreatorAPICallURL.GetUrlWithFilter,
                                                                                   ReportLinkNameConstant.Sponser_report,
                                                                                    FilterConstant.SponserbyIdForUpdate_filter.Replace("[sponsorId]", sponserpickupDateDataContract.ID)
                                                                                       ), Method.PATCH, sponserpickupRequestDC);


                    if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK)
                    {
                        sponserStatusResponseDC = JsonConvert.DeserializeObject<SponserFormResponseDataContract>(response.Content);

                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SponserManager", "UpdateSponserOrderEstimatePickupdate", ex);
            }
            return sponserStatusResponseDC;
        }

        #endregion


        #region set zoho Sponsor Order Status Cancelled detail
        /// <summary>
        /// Method to set details ('Sponsor' form)
        /// </summary>
        /// <param></param>
        /// <returns>model</returns>
        public UpdateSponserOrderStatusDataContract SetSponsorOrderStatusObject(ProductOrderStatusDataContract productOrderStatusInfo, SponserListDataContract sponserListDC)
        {
            UpdateSponserOrderStatusDataContract sponserStatusObj = null;

            try
            {
                if (productOrderStatusInfo != null && sponserListDC != null)
                {
                    if (productOrderStatusInfo.Status == "Cancelled" || productOrderStatusInfo.Status == "cancelled")
                    {
                        sponserStatusObj = new UpdateSponserOrderStatusDataContract();
                        sponserStatusObj.ID = sponserListDC.data[0].ID;
                        sponserStatusObj.Latest_Status = "0X";
                        sponserStatusObj.Order = "0X";
                        //sponserStatusObj.Status_Reason = "New product order cancelled after creation (Sponsor)";
                        //sponserStatusObj.Secondary_Order_Flag = "Cancelled";
                    }
                    else if (productOrderStatusInfo.Status == "Delivered" || productOrderStatusInfo.Status == "delivered")
                    {
                        sponserStatusObj = new UpdateSponserOrderStatusDataContract();
                        sponserStatusObj.ID = sponserListDC.data[0].ID;
                        sponserStatusObj.Latest_Status = "6";
                        sponserStatusObj.Order = sponserListDC.data[0].Order;
                        sponserStatusObj.Installation = "6";
                        //sponserStatusObj.Status_Reason = "New product delivered/ Installed by Sponsor";
                        //sponserStatusObj.Secondary_Order_Flag = "Complete";

                    }

                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SponserManager", "SetSponsorOrderStatusObject", ex);
            }
            return sponserStatusObj;
        }

        #endregion

        #region Post zoho Sponser Order Status Delivered detail 
        ///// <summary>
        ///// Method to push status details ('Sponsor' form)
        ///// </summary>
        ///// <param></param>
        ///// <returns>model</returns>
        //public SponserFormResponseDataContract UpdateSponserOrderStatusDelivered(UpdateSponserOrderStatusDataContract sponserStatusDataContract)
        //{

        //    SponserFormResponseDataContract sponserStatusResponseDC = null;
        //    UpdateSponserOrderStatusFormRequestDataContract sponserStatusRequestDC = new UpdateSponserOrderStatusFormRequestDataContract();
        //    try
        //    {
        //        if (sponserStatusDataContract != null)
        //        {
        //            sponserStatusRequestDC.data = sponserStatusDataContract;

        //            IRestResponse response = ZohoServiceCalls.Rest_InvokeZohoInvoiceServiceForPlainText(ZohoCreatorAPICallURL.GetURLFor(ZohoCreatorAPICallURL.GetUrlWithFilter,
        //                                                                           ReportLinkNameConstant.Sponser_report,
        //                                                                            FilterConstant.SponserbyIdForUpdate_filter.Replace("[sponsorId]", sponserStatusDataContract.ID)
        //                                                                               ), Method.PATCH, sponserStatusRequestDC);


        //            if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK)
        //            {
        //                sponserStatusResponseDC = JsonConvert.DeserializeObject<SponserFormResponseDataContract>(response.Content);

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LibLogging.WriteErrorToDB("SponserManager", "AddSponserOrderStatus", ex);
        //    }
        //    return sponserStatusResponseDC;
        //}

        #endregion
        #region set zoho Sponsor Order Status Delivered detail
        ///// <summary>
        ///// Method to set details ('Sponsor' form)
        ///// </summary>
        ///// <param></param>
        ///// <returns>model</returns>
        //public UpdateSponserOrderStatusDataContract SetSponsorOrderStatusDeliveredObject(ProductOrderStatusDataContract productOrderStatusInfo, SponserListDataContract sponserListDC)
        //{
        //    UpdateSponserOrderStatusDataContract sponserStatusObj = null;

        //    try
        //    {
        //        if (productOrderStatusInfo != null && sponserListDC != null)
        //        {
        //            if (productOrderStatusInfo.Status == "Cancelled" || productOrderStatusInfo.Status == "cancelled")
        //            {
        //                sponserStatusObj = new UpdateSponserOrderStatusDataContract();
        //                sponserStatusObj.ID = sponserListDC.data[0].ID;
        //                sponserStatusObj.Latest_Status = "0X";
        //                sponserStatusObj.Order = "0X";
        //                //sponserStatusObj.Status_Reason = "New product order cancelled after creation (Sponsor)";
        //                //sponserStatusObj.Secondary_Order_Flag = "Cancelled";
        //            }
        //            else if (productOrderStatusInfo.Status == "Delivered" || productOrderStatusInfo.Status == "delivered")
        //            {
        //                sponserStatusObj = new UpdateSponserOrderStatusDataContract();
        //                sponserStatusObj.ID = sponserListDC.data[0].ID;
        //                sponserStatusObj.Latest_Status = "6";
        //                sponserStatusObj.Order = sponserListDC.data[0].Order;
        //                sponserStatusObj.Installation = "6";
        //                //sponserStatusObj.Status_Reason = "New product delivered/ Installed by Sponsor";
        //                //sponserStatusObj.Secondary_Order_Flag = "Complete";

        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LibLogging.WriteErrorToDB("SponserManager", "SetSponsorOrderStatusObject", ex);
        //    }
        //    return sponserStatusObj;
        //}

        #endregion



        #region Update zoho Sponser detail
        /// <summary>
        /// Method to Update Sponsor details ('Sponsor' form)
        /// </summary>
        /// <param></param>
        /// <returns>model</returns>
        public SponserFormResponseDataContract UpdateLGCDetail(UpdateSponsorLogisticStatusDataContract sponserUpdateDataContract)
        {

            // tblDailyPatrolEntry dailyPatrolObj = new tblDailyPatrolEntry();
           
            logging = new Logging();
            SponserFormResponseDataContract sponserResponseDC = null;
            UpdateSponsorLogisticStatusFormRequestDataContract sponserUpdateRequestDC = new UpdateSponsorLogisticStatusFormRequestDataContract();
            try
            {
                if (sponserUpdateDataContract != null)
                {
                    sponserUpdateRequestDC.data = sponserUpdateDataContract;

                    //IRestResponse response = ZohoServiceCalls.Rest_InvokeZohoInvoiceServiceForPlainText("https://creatorapp.zoho.com/api/v2/accountsperthsecurityservices/mobileapp/form/D_Runsheets", patrolRequestDC);
                    IRestResponse response = ZohoServiceCalls.Rest_InvokeZohoInvoiceServiceForPlainText(ZohoCreatorAPICallURL.GetURLFor(ZohoCreatorAPICallURL.GetUrlWithFilter,
                                                                                   ReportLinkNameConstant.Sponser_report,
                                                                                    FilterConstant.SponserbyIdForUpdate_filter.Replace("[sponsorId]", sponserUpdateDataContract.ID)
                                                                                       ), Method.PATCH, sponserUpdateRequestDC);


                    //if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK)
                    //{
                    //    sponserResponseDC = JsonConvert.DeserializeObject<SponserFormResponseDataContract>(response.Content);

                    //}

                    //// new code
                    if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK)
                    {
                        sponserResponseDC = JsonConvert.DeserializeObject<SponserFormResponseDataContract>(response.Content);
                        if (sponserResponseDC.code != 3000)
                        {
                            logging.WriteErrorToDB("SponserManager", "UpdateLGCDetail", sponserUpdateDataContract.LGC_Tkt_No, response);
                        }
                    }
                    else
                    {
                        logging.WriteErrorToDB("SponserManager", "UpdateLGCDetail", sponserUpdateDataContract.LGC_Tkt_No, response);
                    }

                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SponserManager", "UpdateLGCDetail", ex);
            }
            return sponserResponseDC;
        }

        /// <summary>
        /// Method to Update Sponsor details ('Sponsor' form)
        /// </summary>
        /// <param></param>
        /// <returns>model</returns>
        public SponserFormResponseDataContract UpdateLGCStatusDetail(LogisticStatusDataContract sponserUpdateDataContract)
        {

            // tblDailyPatrolEntry dailyPatrolObj = new tblDailyPatrolEntry();
         
            logging = new Logging();
            SponserFormResponseDataContract sponserResponseDC = null;
            LogisticStatusFormRequestDataContract sponserUpdateRequestDC = new LogisticStatusFormRequestDataContract();
            try
            {
                if (sponserUpdateDataContract != null)
                {
                    sponserUpdateRequestDC.data = sponserUpdateDataContract;
                    IRestResponse response = ZohoServiceCalls.Rest_InvokeZohoInvoiceServiceForPlainText(ZohoCreatorAPICallURL.GetURLFor(ZohoCreatorAPICallURL.GetUrlWithFilter,
                                                                                                    ReportLinkNameConstant.Sponser_report,
                                                                                                     FilterConstant.SponserbyIdForUpdate_filter.Replace("[sponsorId]", sponserUpdateDataContract.ID)
                                                                                                        ), Method.PATCH, sponserUpdateRequestDC);


                    //// new code
                    if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK)
                    {
                        sponserResponseDC = JsonConvert.DeserializeObject<SponserFormResponseDataContract>(response.Content);
                        if (sponserResponseDC.code != 3000)
                        {
                            logging.WriteErrorToDB("SponserManager", "UpdateLGCDetail", sponserUpdateDataContract.LGC_Tkt_No, response);
                        }
                    }
                    else
                    {
                        logging.WriteErrorToDB("SponserManager", "UpdateLGCDetail", sponserUpdateDataContract.LGC_Tkt_No, response);
                    }

                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SponserManager", "UpdateLGCDetail", ex);
            }
            return sponserResponseDC;
        }


        #endregion

        #region set zoho Sponser detail for update
        /// <summary>
        /// Method to set details ('Sponser' form)
        /// </summary>
        /// <param></param>
        /// <returns>model</returns>
        public UpdateSponsorLogisticStatusDataContract SetUpdateSponsorObject(string SponsorId, string bizlogTicketNo)
        {
            UpdateSponsorLogisticStatusDataContract sponserUpdateObj = null;

            try
            {
                if (SponsorId != null && bizlogTicketNo != null)
                {
                    sponserUpdateObj = new UpdateSponsorLogisticStatusDataContract();
                    sponserUpdateObj.ID = SponsorId;
                    sponserUpdateObj.LGC_Tkt_No = bizlogTicketNo;
                    //sponserUpdateObj.Pickup = "7";                  
                    sponserUpdateObj.Latest_Status = "7";
                    //sponserUpdateObj.Secondary_Order_Flag = "Processing";
                    //sponserUpdateObj.Status_Reason = "Call Assigned for Logistics Activity (ticket no. Generated)";
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SponserManager", "SetUpdateSponsorObject", ex);
            }
            return sponserUpdateObj;
        }

        #endregion

        #region set zoho Cancel Sponser detail for update (Ticket is cancel by UTC by API)
        /// <summary>
        /// Method to set details ('Sponser' form)
        /// </summary>
        /// <param></param>
        /// <returns>model</returns>
        public UpdateSponsorLogisticStatusDataContract SetUpdateSponsorCancelObject(string SponsorId, string bizlogTicketNo)
        {
            UpdateSponsorLogisticStatusDataContract sponserUpdateObj = null;

            try
            {
                if (SponsorId != null && bizlogTicketNo != null)
                {
                    sponserUpdateObj = new UpdateSponsorLogisticStatusDataContract();
                    sponserUpdateObj.ID = SponsorId;
                    sponserUpdateObj.LGC_Tkt_No = bizlogTicketNo;
                    sponserUpdateObj.Pickup = "7X";
                    sponserUpdateObj.Latest_Status = "7X";
                    //sponserUpdateObj.Secondary_Order_Flag = "Processing";
                    //sponserUpdateObj.Status_Reason = "Ticket is cancel by UTC by API";
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SponserManager", "SetUpdateSponsorCancelObject", ex);
            }
            return sponserUpdateObj;
        }

        #endregion

        #region set zoho Update Logistic Status detail for update
        /// <summary>
        /// Method to set details ('Sponser' form)
        /// </summary>
        /// <param></param>
        /// <returns>model</returns>
        public LogisticStatusDataContract SetUpdateLogisticStatusObject(TicketStatusDataContract ticketStatusDataContract, string zohoId)
        {
            LogisticStatusDataContract logisticStatusUpdateObj = null;

            try
            {
                if (ticketStatusDataContract != null && zohoId != null)
                {
                    logisticStatusUpdateObj = new LogisticStatusDataContract();
                    logisticStatusUpdateObj.ID = zohoId;
                    logisticStatusUpdateObj.LGC_Tkt_No = ticketStatusDataContract.ticketNo;
                    if (ticketStatusDataContract.status == "11" || ticketStatusDataContract.status == "11X")
                    {
                        logisticStatusUpdateObj.EVC_Drop = ticketStatusDataContract.status;
                        logisticStatusUpdateObj.Latest_Status = ticketStatusDataContract.status;

                    }
                    else
                    {
                        logisticStatusUpdateObj.Pickup = ticketStatusDataContract.status;
                        logisticStatusUpdateObj.Latest_Status = ticketStatusDataContract.status;

                    }

                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SponserManager", "SetUpdateLogisticStatusObject", ex);
            }
            return logisticStatusUpdateObj;
        }

        #endregion

        #region Update Logistic status detail
        ///// <summary>
        ///// Method to Update Sponsor details ('Sponsor' form)
        ///// </summary>
        ///// <param></param>
        ///// <returns>model</returns>
        //public SponserFormResponseDataContract UpdateLogisticStatus(UpdateSponsorLogisticStatusDataContract updateLogisticStatusDataContract)
        //{

        //    // tblDailyPatrolEntry dailyPatrolObj = new tblDailyPatrolEntry();
        //    string s = null;
        //    SponserFormResponseDataContract sponserResponseDC = null;
        //    UpdateSponsorLogisticStatusFormRequestDataContract sponserUpdateRequestDC = new UpdateSponsorLogisticStatusFormRequestDataContract();
        //    try
        //    {
        //        if (updateLogisticStatusDataContract != null)
        //        {
        //            sponserUpdateRequestDC.data = updateLogisticStatusDataContract;

        //            //IRestResponse response = ZohoServiceCalls.Rest_InvokeZohoInvoiceServiceForPlainText("https://creatorapp.zoho.com/api/v2/accountsperthsecurityservices/mobileapp/form/D_Runsheets", patrolRequestDC);
        //            IRestResponse response = ZohoServiceCalls.Rest_InvokeZohoInvoiceServiceForPlainText(ZohoCreatorAPICallURL.GetURLFor(ZohoCreatorAPICallURL.GetUrlWithFilter,
        //                                                                           ReportLinkNameConstant.Sponser_report,
        //                                                                            FilterConstant.SponserbyIdForUpdate_filter.Replace("[sponsorId]", updateLogisticStatusDataContract.ID)
        //                                                                               ), Method.PATCH, sponserUpdateRequestDC);


        //            if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK)
        //            {
        //                sponserResponseDC = JsonConvert.DeserializeObject<SponserFormResponseDataContract>(response.Content);

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LibLogging.WriteErrorToDB("SponserManager", "UpdateLogisticStatus", ex);
        //    }
        //    return sponserResponseDC;
        //}

        #endregion

        #region Set zoho Sponser detail for update voucher detail

        /// <summary>
        /// Method to set voucher update object for exchange order
        /// </summary>
        /// <param name="sponsorId"></param>
        /// <param name="voucherCode"></param>
        /// <param name="storeCode"></param>
        /// <returns></returns>
        public ExchageOrderVoucherUpdateDataContract SetUpdateExchangeVoucherDetail(string sponsorId, string voucheramount, tblVoucherVerfication voucherVerfication, tblBusinessPartner businessPartnerObj, string Is_Voucher_Redeemed)
        {
            ExchageOrderVoucherUpdateDataContract sponserUpdateObj = null;
            ProductTypeRepository productTypeRepository = new ProductTypeRepository();
            ProductCategoryRepository categoryRepository = new ProductCategoryRepository();
            PriceMasterRepository priceMasterRepository = new PriceMasterRepository();
            MasterManager masterManager = new MasterManager();
            try
            {
                if (!string.IsNullOrEmpty(sponsorId) && !string.IsNullOrEmpty(voucheramount) && voucherVerfication != null)
                {
                    sponserUpdateObj = new ExchageOrderVoucherUpdateDataContract();
                    sponserUpdateObj.ID = sponsorId;
                    sponserUpdateObj.Voucher_Code = voucherVerfication.VoucherCode;
                    sponserUpdateObj.Is_Voucher_Redeemed = Is_Voucher_Redeemed;
                    
                    if (sponserUpdateObj != null)
                    {
                        sponserUpdateObj.New_Product_Code = voucherVerfication.tblModelNumber != null ? voucherVerfication.tblModelNumber.ModelName : string.Empty;
                        string prodName = string.Empty;
                        prodName = voucherVerfication.tblProductCategory != null ? voucherVerfication.tblProductCategory.Description : string.Empty;
                        prodName = !string.IsNullOrEmpty(prodName) && voucherVerfication.tblProductType != null ? prodName + " - " + voucherVerfication.tblProductType.Description : string.Empty;
                        sponserUpdateObj.New_Product_Name = prodName;
                    }
                    if (businessPartnerObj != null)
                    {
                        sponserUpdateObj.Voucher_Redeemed_By = businessPartnerObj.Name + ", " + businessPartnerObj.AddressLine1;
                        sponserUpdateObj.Store_Code = businessPartnerObj.StoreCode;
                        sponserUpdateObj.Associate_Code = businessPartnerObj.AssociateCode;
                    }
                    sponserUpdateObj.Voucher_Amount = voucheramount;
                    if (voucherVerfication != null && voucherVerfication.CreatedDate != null)
                    {
                        string voucherDate = Convert.ToDateTime(voucherVerfication.CreatedDate).ToString("dd-MMMM-yyyy");
                        sponserUpdateObj.Voucher_Redeem_Date = voucherDate;
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SponserManager", "SetUpdateExchangeVoucherDetail", ex);
            }
            return sponserUpdateObj;
        }


        /// <summary>
        /// Method to set voucher update object for exchange order deffered
        /// </summary>
        /// <param name="sponsorId"></param>
        /// <param name="voucherCode"></param>
        /// <param name="storeCode"></param>
        /// <returns></returns>
        public ExchageOrderVoucherUpdateDataContract SetUpdateExchangeForDeffered(string sponsorId, int modelNumberid, tblBusinessPartner businessPartnerObj)
        {
            ExchageOrderVoucherUpdateDataContract sponserUpdateObj = null;
            ProductTypeRepository productTypeRepository = new ProductTypeRepository();
            ProductCategoryRepository categoryRepository = new ProductCategoryRepository();
            PriceMasterRepository priceMasterRepository = new PriceMasterRepository();
            ModelNumberRepository modelNumberRepository = new ModelNumberRepository();
            MasterManager masterManager = new MasterManager();
            ExchangeOrderRepository exchangeOrderRepository = new ExchangeOrderRepository();
            try
            {
                if (!string.IsNullOrEmpty(sponsorId) && modelNumberid > 0)
                {
                    sponserUpdateObj = new ExchageOrderVoucherUpdateDataContract();
                    sponserUpdateObj.ID = sponsorId;
                    sponserUpdateObj.Voucher_Code = "";
                    sponserUpdateObj.Is_Voucher_Redeemed = "";

                    tblModelNumber modelNumber = modelNumberRepository.GetSingle(x => x.ModelNumberId == modelNumberid);
                    sponserUpdateObj.New_Product_Code = modelNumber != null && modelNumber.ModelName != null ? modelNumber.ModelName : string.Empty;
                    string prodName = string.Empty;
                    prodName = modelNumber.tblProductCategory != null ? modelNumber.tblProductCategory.Description : string.Empty;
                    prodName = !string.IsNullOrEmpty(prodName) && modelNumber.tblProductType != null ? prodName + " - " + modelNumber.tblProductType.Description : string.Empty;
                    sponserUpdateObj.New_Product_Name = prodName;

                    if (businessPartnerObj != null)
                    {
                        sponserUpdateObj.Voucher_Redeemed_By = "(Deffered) - " + businessPartnerObj.Name + ", " + businessPartnerObj.AddressLine1;
                        sponserUpdateObj.Store_Code = businessPartnerObj.StoreCode;
                        sponserUpdateObj.Associate_Code = businessPartnerObj.AssociateCode;
                    }
                    sponserUpdateObj.Voucher_Amount = "";
                    tblExchangeOrder exchangeOrder = exchangeOrderRepository.GetSingle(x => x.ZohoSponsorOrderId == sponsorId);
                    if (exchangeOrder != null)
                    {
                        sponserUpdateObj.Amount_Payable_Through_LGC = exchangeOrder.ExchangePrice.ToString();
                    }

                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SponserManager", "SetUpdateExchangeForDeffered", ex);
            }
            return sponserUpdateObj;
        }




        /// <summary>
        /// Method to update voucher detail in Zoho
        /// </summary>
        /// <param name="ExchageOrderVoucherUpdateDataContract">ExchageOrderVoucherUpdateDataContract</param>
        /// <returns>SponserFormResponseDataContract</returns>
        public SponserFormResponseDataContract UpdateVoucherDetailinExchangeOrder(ExchageOrderVoucherUpdateDataContract exchangeOrderDC)
        {
            logging = new Logging();
            SponserFormResponseDataContract sponserResponseDC = null;
            ExchageOrderVoucherUpdateFormDataContract exchangeOrderFormUpdateRequestDC = new ExchageOrderVoucherUpdateFormDataContract();
            try
            {
                if (exchangeOrderDC != null)
                {
                    exchangeOrderFormUpdateRequestDC.data = exchangeOrderDC;

                    IRestResponse response = ZohoServiceCalls.Rest_InvokeZohoInvoiceServiceForPlainText(ZohoCreatorAPICallURL.GetURLFor(ZohoCreatorAPICallURL.GetUrlWithFilter,
                                                                                   ReportLinkNameConstant.Sponser_report,
                                                                                    FilterConstant.SponserbyIdForUpdate_filter.Replace("[sponsorId]", exchangeOrderDC.ID)
                                                                                       ), Method.PATCH, exchangeOrderFormUpdateRequestDC);


                    //// new code
                    if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK)
                    {
                        sponserResponseDC = JsonConvert.DeserializeObject<SponserFormResponseDataContract>(response.Content);
                        if (sponserResponseDC.code != 3000)
                        {
                            logging.WriteErrorToDB("SponserManager", "UpdateVoucherDetail", exchangeOrderDC.Voucher_Code, response);
                        }
                    }
                    else
                    {
                        logging.WriteErrorToDB("SponserManager", "UpdateVoucherDetail", exchangeOrderDC.Voucher_Code, response);
                    }

                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SponserManager", "UpdateVoucherDetailinExchangeOrder", ex);
            }
            return sponserResponseDC;
        }


        /// <summary>
        /// Method to update voucher detail in Zoho
        /// </summary>
        /// <param name="ExchageOrderVoucherUpdateDataContract">ExchageOrderVoucherUpdateDataContract</param>
        /// <returns>SponserFormResponseDataContract</returns>
        public SponserFormResponseDataContract UpdateVoucherDetailinExchangeOrderForSamsung(ExchageOrderVoucherUpdateSamsungDataContract exchangeOrderDC)
        {
         
            logging = new Logging();
            SponserFormResponseDataContract sponserResponseDC = null;
            ExchageOrderVoucherUpdateFormSamsungDataContract exchangeOrderFormUpdateRequestDC = new ExchageOrderVoucherUpdateFormSamsungDataContract();
            try
            {
                if (exchangeOrderDC != null)
                {
                    exchangeOrderFormUpdateRequestDC.data = exchangeOrderDC;

                    IRestResponse response = ZohoServiceCalls.Rest_InvokeZohoInvoiceServiceForPlainText(ZohoCreatorAPICallURL.GetURLFor(ZohoCreatorAPICallURL.GetUrlWithFilter,
                                                                                   ReportLinkNameConstant.Sponser_report,
                                                                                    FilterConstant.SponserbyIdForUpdate_filter.Replace("[sponsorId]", exchangeOrderDC.ID)
                                                                                       ), Method.PATCH, exchangeOrderFormUpdateRequestDC);


                    //// new code
                    if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK)
                    {
                        sponserResponseDC = JsonConvert.DeserializeObject<SponserFormResponseDataContract>(response.Content);
                        if (sponserResponseDC.code != 3000)
                        {
                            logging.WriteErrorToDB("SponserManager", "UpdateVoucherDetailinExchangeOrderForSamsung", exchangeOrderDC.ID, response);
                        }
                    }
                    else
                    {
                        logging.WriteErrorToDB("SponserManager", "UpdateVoucherDetailinExchangeOrderForSamsung", exchangeOrderDC.ID, response);
                    }

                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SponserManager", "UpdateVoucherDetailinExchangeOrderForSamsung", ex);
            }
            return sponserResponseDC;
        }


        /// <summary>
        /// Method to set voucher update object for exchange order deffered
        /// </summary>
        /// <param name="sponsorId"></param>
        /// <param name="amount"></param>
        /// <returns>ExchageOrderVoucherUpdateSamsungDataContract</returns>
        public ExchageOrderVoucherUpdateSamsungDataContract SetUpdateExchangeForSamsungPayableAmount(string sponsorId, string amount)
        {
            ExchageOrderVoucherUpdateSamsungDataContract exchangeOrder = new ExchageOrderVoucherUpdateSamsungDataContract();
            ExchangeOrderRepository exchangeOrderRepository = new ExchangeOrderRepository();
            try
            {
                if (!string.IsNullOrEmpty(sponsorId))
                {

                    exchangeOrder.ID = sponsorId;
                    exchangeOrder.Amount_Payable_Through_LGC = amount;
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SponserManager", "SetUpdateExchangeForSamsungPayableAmount", ex);
            }
            return exchangeOrder;
        }

        #endregion

        #region update and get zoho Sponser detail
        /// <summary>
        /// Method to Update Sponsor details ('Sponsor' form)
        /// </summary>
        /// <param></param>
        /// <returns>model</returns>
        public SponserFormResponseDataContract UpdateSponser(CustomerDetailsUpdateDatacontract customerDetailsDC)
        {

            logging = new Logging();
         
            string jsonString = string.Empty;
            SponserFormResponseDataContract sponserResponseDC = null;
            CustomerDetailsUpdateFormDataContract customerrDetailsUpdate = new CustomerDetailsUpdateFormDataContract();
            try
            {
                if (customerDetailsDC != null)
                {
                    customerrDetailsUpdate.data = customerDetailsDC;
                    jsonString = JsonConvert.SerializeObject(customerrDetailsUpdate);
                    logging.WriteAPIRequestToDB("SponserManager", "UpdateSponser", customerrDetailsUpdate.data.Id, jsonString);

                    IRestResponse response = ZohoServiceCalls.Rest_InvokeZohoInvoiceServiceForPlainText(ZohoCreatorAPICallURL.GetURLFor(ZohoCreatorAPICallURL.GetUrlWithFilter,
                                                                                   ReportLinkNameConstant.Sponser_report,
                                                                                    FilterConstant.SponserbyIdForUpdate_filter.Replace("[sponsorId]", customerDetailsDC.Id)
                                                                                       ), Method.PATCH, customerrDetailsUpdate);


                    //// new code
                    if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK)
                    {
                        sponserResponseDC = JsonConvert.DeserializeObject<SponserFormResponseDataContract>(response.Content);
                        if (sponserResponseDC.code != 3000)
                        {
                            logging.WriteErrorToDB("SponserManager", "UpdateSponser", customerDetailsDC.Id, response);
                        }
                    }
                    else
                    {
                        logging.WriteErrorToDB("SponserManager", "UpdateSponser", customerDetailsDC.Id, response);
                    }

                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SponserManager", "UpdateSponser", ex);
            }
            return sponserResponseDC;
        }



        #endregion

        #region VK Store POD image on Sponser Report in zoho creator with using patch api
        /// <summary>
        /// Method to update voucher detail in Zoho
        /// </summary>
        /// <param name="EVCPODDetailsDataContract">EVCPODDetailsDataContract</param>
        /// <returns>SponserFormResponseDataContract</returns>
        public SponserFormResponseDataContract UpdateExchangePODUrlOnZoho(EVCPODDetailsDataContract exchangePODObj)
        {
            logging = new Logging();
            SponserFormResponseDataContract sponserResponseDC = null;
            ExchageOrderPODDataContract exchangeOrderPODDC = new ExchageOrderPODDataContract();
            try
            {
                if (exchangePODObj != null)
                {
                    exchangeOrderPODDC.data = exchangePODObj;

                    IRestResponse response = ZohoServiceCalls.Rest_InvokeZohoInvoiceServiceForPlainText(ZohoCreatorAPICallURL.GetURLFor(ZohoCreatorAPICallURL.GetUrlWithFilter,
                                                                                   ReportLinkNameConstant.Sponser_report,
                                                                                    FilterConstant.SponserbyIdForUpdate_filter.Replace("[sponsorId]", exchangePODObj.ID)
                                                                                       ), Method.PATCH, exchangeOrderPODDC);

                    //// new code
                    if (response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.OK)
                    {
                        sponserResponseDC = JsonConvert.DeserializeObject<SponserFormResponseDataContract>(response.Content);
                        if (sponserResponseDC.code != 3000)
                        {
                            logging.WriteErrorToDB("SponserManager", "UpdateExchangePODUrlOnZoho", exchangePODObj.ID, response);
                        }
                    }
                    else
                    {
                        logging.WriteErrorToDB("SponserManager", "UpdateExchangePODUrlOnZoho", exchangePODObj.ID, response);
                    }

                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SponserManager", "UpdateExchangePODUrlOnZoho", ex);
            }
            return sponserResponseDC;
        }

        #endregion
    }
}
