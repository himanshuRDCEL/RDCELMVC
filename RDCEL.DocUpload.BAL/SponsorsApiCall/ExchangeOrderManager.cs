using GraspCorn.Common.Constant;
using GraspCorn.Common.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RDCEL.DocUpload.BAL.Common;
using RDCEL.DocUpload.BAL.ZohoCreatorCall;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DataContract.MasterModel;
using RDCEL.DocUpload.DataContract.ProductsPrices;
using RDCEL.DocUpload.DataContract.ProductTaxonomy;
using RDCEL.DocUpload.DataContract.SponsorModel;
using RDCEL.DocUpload.DataContract.ZohoModel;
using GraspCorn.Common.Enums;
using GraspCorn.Common;
using RDCEL.DocUpload.DataContract.Voucher;
using RDCEL.DocUpload.DataContract.BillCloud;
using RDCEL.DocUpload.BAL.ServiceCall;
using RestSharp;
using System.Net;
using Newtonsoft.Json;
using RDCEL.DocUpload.DataContract.Common;
using RDCEL.DocUpload.DAL.Helper;
using System.Data.SqlClient;
using System.IO;
using RDCEL.DocUpload.BAL.ProcessAPI;
using RDCEL.DocUpload.DataContract.ExchangeOrderDetails;
using System.Xml.Linq;
using RDCEL.DocUpload.DataContract.Daikin;
using System.Web.Mvc;
using RDCEL.DocUpload.DataContract.WhatsappTemplates;
using System.Web.Hosting;
using RDCEL.DocUpload.BAL.Manager;
using System.Reflection;
using System.ComponentModel;
using Data = RDCEL.DocUpload.DataContract.ZohoModel.Data;
using RDCEL.DocUpload.DataContract.TimeSlot;
using RDCEL.DocUpload.DataContract.Lodha_Group;

using RDCEL.DocUpload.BAL.LodhaGroupManager;
using System.Web.Mail;
using RDCEL.DocUpload.DataContract.DaikinModel;
using RDCEL.DocUpload.BAL.SweetenerManager;
using System.Runtime.Remoting.Contexts;
using static RDCEL.DocUpload.BAL.Common.WhatsappNotificationManager;
using System.Web.UI.WebControls;

namespace RDCEL.DocUpload.BAL.SponsorsApiCall
{
    public class ExchangeOrderManager
    {
        #region Variable Declaration
        PinCodeRepository _pinCodeRepository;
        PriceMasterRepository _productPriceRepository;
        BrandRepository _brandRepository;
        ProductCategoryRepository _productCategoryRepository;
        NotificationManager _notificationManager;
        ProductTypeRepository _productTypeRepository;
        DateTime currentDatetime = DateTime.Now.TrimMilliseconds();
        BusinessUnitRepository _businessUnitRepository;
        ExchangeOrderRepository _exchangeOrderRepository;
        CustomerDetailsRepository _customerDetailsRepository;
        LoginDetailsUTCRepository _loginDetailsUTCRepository;
        SponserManager _sponserManager;
        SocietyRepository _societyRepository;
        RDCEL.DocUpload.BAL.ZohoCreatorCall.MasterManager _masterManager;
        VoucherVerificationRepository _voucherVerificationRepository;
        BusinessPartnerRepository _businessPartnerRepository;
        ExchangeOrderStatusRepository _exchangeOrderStatusRepository;
        VoucherStatusRepository _voucherStatusRepository;
        ModelNumberRepository _modelNumberrepository;
        HistoryRepository _historyRepository;
        WhatsappNotificationManager _whatsappManager;
        WhatsappMessageRepository _whatsAppMessageRepository;
        ConfigurationRepository _configurationRepository;
        TimeSlotMasterRepository _timeSlotMasterRepository;
        SponsrOrderSyncManager _sponserOrderSyncManager;
        OrderTransactionRepository _orderTransactionRepository;
        Logging logging;
        AreaLocalityRepository _areaLocalityRepository;
        AbbRedemptionRepository abbRedemptionRepository;
        ABBRegistrationRepository aBBRegistrationRepository;
        BrandSmartSellRepository brandSmartSellRepository;
        UniversalPriceMasterRepository _universalPriceMasterRepository;
        ManageSweetener _managerSweetener;
        OrderBasedConfigurationRepository _orderbasedRepository;
        CouponRepository _couponRepository;

        #endregion

        #region Get all PinCode detail
        /// <summary>
        /// Method to get the list of PinCode data contract
        /// </summary>       
        /// <returns>List PinCodeDataContract</returns>   
        public PinCodeDataContract GetZipCodes(int buid)
        {
            _pinCodeRepository = new PinCodeRepository();
            PinCodeDataContract pinCodeDC = new PinCodeDataContract();
            try
            {
                DataTable dt = _pinCodeRepository.GetEXchangePincodeListbybuid(buid);

                List<tblPinCode> pincodeMasterList = GenericConversionHelper.DataTableToList<tblPinCode>(dt);
                if (pincodeMasterList != null && pincodeMasterList.Count > 0)
                {
                    List<ZipCodes> zipCodesList = pincodeMasterList.Select(x => new ZipCodes
                    {
                        ZipCode = x.ZipCode.ToString(),
                        Id = x.Id  // Assuming x.Id is the property representing the Id of the PinCode
                    }).ToList();
                    pinCodeDC.ZipCodes = zipCodesList.OrderBy(o => o.ZipCode).ToList();
                }
                else
                {
                    pinCodeDC.ZipCodes = new List<ZipCodes> { new ZipCodes { ZipCode = "No pincode available on this location" } };
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SamsungManager", "GetZipCodes", ex);
                // You might want to handle the exception accordingly
            }
            return pinCodeDC;
        }

        #endregion

        #region Get all Product Price detail
        /// <summary>
        /// Method to get the list of Product Price data contract
        /// </summary>       
        /// <returns>List ProductsPricesDataContract</returns>   
        public List<ProductsPricesDataContract> GetProductPrice(string uname, bool? IsSweetenerModelBased, int? PriceMasterNameId, string categoryname = "", string catid = "", string typeid = "", string brandid = "", int? NewBrandId = 0, int? NewCatId = 0, int? NewTypeId = 0, int? BusinessUnitId = 0, int? BusinessPartnerId = 0, int? NewModelId = 0)
        {
            _productPriceRepository = new PriceMasterRepository();
            _universalPriceMasterRepository = new UniversalPriceMasterRepository();
            _brandRepository = new BrandRepository();
            _productTypeRepository = new ProductTypeRepository();
            _productCategoryRepository = new ProductCategoryRepository();
            _managerSweetener = new ManageSweetener();
            List<tblPriceMaster> tblProductPriceObjList = new List<tblPriceMaster>();
            List<tblUniversalPriceMaster> tblUniversalPriceMasterObjList = new List<tblUniversalPriceMaster>();
            ProductsPricesDataContract productsPricesDC = null;
            List<ProductsPricesDataContract> productsPricesList = new List<ProductsPricesDataContract>();
            List<ProductsFromTypePriceList> productsTyepeList = new List<ProductsFromTypePriceList>();
            List<BrandPriceList> brandList = null;
            GetSweetenerDetailsDataContract detailsforSweetenerDc = new GetSweetenerDetailsDataContract();
            SweetenerDataContract sweetener = new SweetenerDataContract();
            string priceCode = string.Empty;
            try
            {


                //Get the brand list 
                List<tblBrand> masterbrandList = _brandRepository.GetList(x => x.IsActive == true).ToList();
                List<tblProductType> masterCategoryTypeList = _productTypeRepository.GetList(x => x.IsActive == true).ToList();

                detailsforSweetenerDc.BrandId = NewBrandId;
                detailsforSweetenerDc.BusinessPartnerId = BusinessPartnerId;
                detailsforSweetenerDc.BusinessUnitId = BusinessUnitId;
                detailsforSweetenerDc.NewProdCatId = NewCatId;
                detailsforSweetenerDc.NewProdTypeId = NewTypeId;
                detailsforSweetenerDc.IsSweetenerModalBased = IsSweetenerModelBased;
                detailsforSweetenerDc.ModalId = NewModelId;
                sweetener = _managerSweetener.GetSweetenerAmtExchange(detailsforSweetenerDc);
                //Get the Product price list
                if (!string.IsNullOrEmpty(categoryname))
                {
                    #region For Category Name

                    tblUniversalPriceMasterObjList = _universalPriceMasterRepository.GetList(x => x.IsActive == true
                    && (x.PriceMasterNameId == PriceMasterNameId)
                    && (x.ProductCategoryName != null && x.ProductCategoryName.ToLower().Equals(categoryname.ToLower()))).ToList();
                    if (tblUniversalPriceMasterObjList != null && tblUniversalPriceMasterObjList.Count > 0)
                    {

                        foreach (var item in tblUniversalPriceMasterObjList)
                        {
                            //ProductsPricesDataContract ProductsPrices = new ProductsPricesDataContract();


                            ProductsFromTypePriceList productsType = new ProductsFromTypePriceList();
                            //Code to check type

                            productsType.name = item.ProductTypeName.Replace(System.Environment.NewLine, string.Empty);
                            //if (productsType.name != null)
                            //{
                            //string size = masterCategoryTypeList.FirstOrDefault(x => x.Description.ToLower().Equals(productsType.name.ToLower())).Size;
                            //new code
                            string size = masterCategoryTypeList.FirstOrDefault(x => x.Id.Equals(item.ProductTypeId)).Size;

                            if (size != null)
                                productsType.name = productsType.name + " " + "(" + size + ")";

                            //}
                            productsType.ProducttypeId = (int)item.ProductTypeId;
                            productsType.id = item.PriceMasterUniversalId;

                            #region code to fill brand list

                            BrandPriceList brandPL = null;
                            brandList = new List<BrandPriceList>();
                            //For Brand 1
                            if (!string.IsNullOrEmpty(item.BrandName_1))
                            {

                                string BrandDescription = ExchangeOrderManager.GetEnumDescription(PriceMasterBrandEnum.Others);
                                if (item.BrandName_1 == BrandDescription)
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_1.ToLower())).Id;
                                    brandPL.Name = item.BrandName_1;
                                    brandPL.Price = item.Quote_P;
                                    brandPL.Mid_Price = item.Quote_Q;
                                    brandPL.Min_Price = item.Quote_R;
                                    brandPL.Scrap_Price = item.Quote_S;
                                    brandList.Add(brandPL);
                                }
                                else
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_1.ToLower())).Id;
                                    brandPL.Name = item.BrandName_1;
                                    brandPL.Price = item.Quote_P_High;
                                    brandPL.Mid_Price = item.Quote_Q_High;
                                    brandPL.Min_Price = item.Quote_R_High;
                                    brandPL.Scrap_Price = item.Quote_S_High;
                                    brandList.Add(brandPL);
                                }
                                //Old code for price f brand 2 by V.C
                                //brandPL = new BrandPriceList();
                                //brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_1.ToLower())).Id;
                                //brandPL.Name = item.BrandName_1;
                                //brandPL.Price = item.Quote_P_High;
                                //brandPL.Mid_Price = item.Quote_Q_High;
                                //brandPL.Min_Price = item.Quote_R_High;
                                //brandPL.Scrap_Price = item.Quote_S_High;
                                //brandList.Add(brandPL);
                            }

                            //For Brand 2
                            if (!string.IsNullOrEmpty(item.BrandName_2))
                            {
                                string BrandDescription = ExchangeOrderManager.GetEnumDescription(PriceMasterBrandEnum.Others);
                                if (item.BrandName_2 == BrandDescription)
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_2.ToLower())).Id;
                                    brandPL.Name = item.BrandName_2;
                                    brandPL.Price = item.Quote_P;
                                    brandPL.Mid_Price = item.Quote_Q;
                                    brandPL.Min_Price = item.Quote_R;
                                    brandPL.Scrap_Price = item.Quote_S;
                                    brandList.Add(brandPL);
                                }
                                else
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_2.ToLower())).Id;
                                    brandPL.Name = item.BrandName_2;
                                    brandPL.Price = item.Quote_P_High;
                                    brandPL.Mid_Price = item.Quote_Q_High;
                                    brandPL.Min_Price = item.Quote_R_High;
                                    brandPL.Scrap_Price = item.Quote_S_High;
                                    brandList.Add(brandPL);
                                }

                                //Old code for price f brand 2 by V.C
                                //brandPL = new BrandPriceList();
                                //brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_2.ToLower())).Id;
                                //brandPL.Name = item.BrandName_2;
                                //brandPL.Price = item.Quote_P_High;
                                //brandPL.Mid_Price = item.Quote_Q_High;
                                //brandPL.Min_Price = item.Quote_R_High;
                                //brandPL.Scrap_Price = item.Quote_S_High;
                                //brandList.Add(brandPL);
                            }

                            //For Brand 3
                            if (!string.IsNullOrEmpty(item.BrandName_3))
                            {
                                string BrandDescription = ExchangeOrderManager.GetEnumDescription(PriceMasterBrandEnum.Others);
                                if (item.BrandName_3 == BrandDescription)
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_3.ToLower())).Id;
                                    brandPL.Name = item.BrandName_3;
                                    brandPL.Price = item.Quote_P;
                                    brandPL.Mid_Price = item.Quote_Q;
                                    brandPL.Min_Price = item.Quote_R;
                                    brandPL.Scrap_Price = item.Quote_S;
                                    brandList.Add(brandPL);
                                }
                                else
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_3.ToLower())).Id;
                                    brandPL.Name = item.BrandName_3;
                                    brandPL.Price = item.Quote_P_High;
                                    brandPL.Mid_Price = item.Quote_Q_High;
                                    brandPL.Min_Price = item.Quote_R_High;
                                    brandPL.Scrap_Price = item.Quote_S_High;
                                    brandList.Add(brandPL);
                                }

                            }

                            //For Brand 4
                            if (!string.IsNullOrEmpty(item.BrandName_4))
                            {
                                string BrandDescription = ExchangeOrderManager.GetEnumDescription(PriceMasterBrandEnum.Others);
                                if (item.BrandName_4 == BrandDescription)
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_4.ToLower())).Id;
                                    brandPL.Name = item.BrandName_4;
                                    brandPL.Price = item.Quote_P;
                                    brandPL.Mid_Price = item.Quote_Q;
                                    brandPL.Min_Price = item.Quote_R;
                                    brandPL.Scrap_Price = item.Quote_S;
                                    brandList.Add(brandPL);
                                }
                                else
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_4.ToLower())).Id;
                                    brandPL.Name = item.BrandName_4;
                                    brandPL.Price = item.Quote_P_High;
                                    brandPL.Mid_Price = item.Quote_Q_High;
                                    brandPL.Min_Price = item.Quote_R_High;
                                    brandPL.Scrap_Price = item.Quote_S_High;
                                    brandList.Add(brandPL);
                                }
                                //brandPL = new BrandPriceList();
                                //brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_4.ToLower())).Id;
                                //brandPL.Name = item.BrandName_4;
                                //brandPL.Price = item.Quote_P_High;
                                //brandPL.Mid_Price = item.Quote_Q_High;
                                //brandPL.Min_Price = item.Quote_R_High;
                                //brandPL.Scrap_Price = item.Quote_S_High;
                                //brandList.Add(brandPL);
                            }

                            //For Other
                            //brandPL = new BrandPriceList();
                            //brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals("Others".ToLower())).Id;
                            //brandPL.Name = "Others";
                            //brandPL.Price = item.Quote_P;
                            //brandPL.Mid_Price = item.Quote_Q;
                            //brandPL.Min_Price = item.Quote_R;
                            //brandPL.Scrap_Price = item.Quote_S;
                            //brandList.Add(brandPL);
                            //productsPriceList.Add(productsPrice);
                            #endregion

                            productsType.Brand = brandList;
                            productsTyepeList.Add(productsType);


                        }
                        productsPricesDC = new ProductsPricesDataContract();
                        productsPricesDC.ProductsFromType = productsTyepeList;

                        productsPricesDC.CategoryId = (int)tblUniversalPriceMasterObjList[0].ProductCategoryId;
                        productsPricesDC.categoryName = tblUniversalPriceMasterObjList[0].ProductCategoryName;
                        productsPricesDC.SweetenerAmount = sweetener.SweetenerTotal.ToString();

                    }
                    productsPricesList = new List<ProductsPricesDataContract>();
                    productsPricesList.Add(productsPricesDC);
                    #endregion
                }
                else if (!string.IsNullOrEmpty(catid) || !string.IsNullOrEmpty(typeid) || !string.IsNullOrEmpty(brandid))
                {
                    #region For Category id type id
                    tblUniversalPriceMasterObjList = _universalPriceMasterRepository.GetList(x => x.IsActive == true
                    && (x.PriceMasterNameId != null && x.PriceMasterNameId.Equals(PriceMasterNameId))
                    && (x.ProductCategoryId != null && (string.IsNullOrEmpty(catid) || x.ProductCategoryId == Convert.ToInt32(catid)))
                    && (x.ProductTypeId != null && (string.IsNullOrEmpty(typeid) || x.ProductTypeId == Convert.ToInt32(typeid)))
                    ).ToList();
                    if (tblUniversalPriceMasterObjList != null && tblUniversalPriceMasterObjList.Count > 0)
                    {

                        foreach (var item in tblUniversalPriceMasterObjList)
                        {
                            //ProductsPricesDataContract ProductsPrices = new ProductsPricesDataContract();


                            ProductsFromTypePriceList productsType = new ProductsFromTypePriceList();
                            //Code to check type

                            productsType.name = item.ProductTypeName.Replace(System.Environment.NewLine, string.Empty);
                            //if (productsType.name != null)
                            //{
                            //string size = masterCategoryTypeList.FirstOrDefault(x => x.Description.ToLower().Equals(productsType.name.ToLower())).Size;
                            //new code
                            tblProductType producttypeObj = new tblProductType();
                            string size = masterCategoryTypeList.FirstOrDefault(x => x.Id.Equals(item.ProductTypeId) && x.IsActive == true).Size;

                            if (size != null)
                                productsType.name = productsType.name + " " + "(" + size + ")";

                            //}
                            productsType.ProducttypeId = (int)item.ProductTypeId;
                            productsType.id = item.PriceMasterUniversalId;

                            #region code to fill brand list

                            BrandPriceList brandPL = null;
                            brandList = new List<BrandPriceList>();
                            //For Brand 1
                            if (!string.IsNullOrEmpty(item.BrandName_1))
                            {

                                string BrandDescription = ExchangeOrderManager.GetEnumDescription(PriceMasterBrandEnum.Others);
                                if (item.BrandName_1 == BrandDescription)
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_1.ToLower())).Id;
                                    brandPL.Name = item.BrandName_1;
                                    brandPL.Price = item.Quote_P;
                                    brandPL.Mid_Price = item.Quote_Q;
                                    brandPL.Min_Price = item.Quote_R;
                                    brandPL.Scrap_Price = item.Quote_S;
                                    brandList.Add(brandPL);
                                }
                                else
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_1.ToLower())).Id;
                                    brandPL.Name = item.BrandName_1;
                                    brandPL.Price = item.Quote_P_High;
                                    brandPL.Mid_Price = item.Quote_Q_High;
                                    brandPL.Min_Price = item.Quote_R_High;
                                    brandPL.Scrap_Price = item.Quote_S_High;
                                    brandList.Add(brandPL);
                                }
                                //Old code for price f brand 2 by V.C
                                //brandPL = new BrandPriceList();
                                //brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_1.ToLower())).Id;
                                //brandPL.Name = item.BrandName_1;
                                //brandPL.Price = item.Quote_P_High;
                                //brandPL.Mid_Price = item.Quote_Q_High;
                                //brandPL.Min_Price = item.Quote_R_High;
                                //brandPL.Scrap_Price = item.Quote_S_High;
                                //brandList.Add(brandPL);
                            }

                            //For Brand 2
                            if (!string.IsNullOrEmpty(item.BrandName_2))
                            {


                                string BrandDescription = ExchangeOrderManager.GetEnumDescription(PriceMasterBrandEnum.Others);
                                if (item.BrandName_2 == BrandDescription)
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_2.ToLower())).Id;
                                    brandPL.Name = item.BrandName_2;
                                    brandPL.Price = item.Quote_P;
                                    brandPL.Mid_Price = item.Quote_Q;
                                    brandPL.Min_Price = item.Quote_R;
                                    brandPL.Scrap_Price = item.Quote_S;
                                    brandList.Add(brandPL);
                                }
                                else
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_2.ToLower())).Id;
                                    brandPL.Name = item.BrandName_2;
                                    brandPL.Price = item.Quote_P_High;
                                    brandPL.Mid_Price = item.Quote_Q_High;
                                    brandPL.Min_Price = item.Quote_R_High;
                                    brandPL.Scrap_Price = item.Quote_S_High;
                                    brandList.Add(brandPL);
                                }

                                //Old code for price f brand 2 by V.C
                                //brandPL = new BrandPriceList();
                                //brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_2.ToLower())).Id;
                                //brandPL.Name = item.BrandName_2;
                                //brandPL.Price = item.Quote_P_High;
                                //brandPL.Mid_Price = item.Quote_Q_High;
                                //brandPL.Min_Price = item.Quote_R_High;
                                //brandPL.Scrap_Price = item.Quote_S_High;
                                //brandList.Add(brandPL);
                            }

                            //For Brand 3
                            if (!string.IsNullOrEmpty(item.BrandName_3))
                            {
                                string BrandDescription = ExchangeOrderManager.GetEnumDescription(PriceMasterBrandEnum.Others);
                                if (item.BrandName_3 == BrandDescription)
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_3.ToLower())).Id;
                                    brandPL.Name = item.BrandName_3;
                                    brandPL.Price = item.Quote_P;
                                    brandPL.Mid_Price = item.Quote_Q;
                                    brandPL.Min_Price = item.Quote_R;
                                    brandPL.Scrap_Price = item.Quote_S;
                                    brandList.Add(brandPL);
                                }
                                else
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_3.ToLower())).Id;
                                    brandPL.Name = item.BrandName_3;
                                    brandPL.Price = item.Quote_P_High;
                                    brandPL.Mid_Price = item.Quote_Q_High;
                                    brandPL.Min_Price = item.Quote_R_High;
                                    brandPL.Scrap_Price = item.Quote_S_High;
                                    brandList.Add(brandPL);
                                }

                            }

                            //For Brand 4
                            if (!string.IsNullOrEmpty(item.BrandName_4))
                            {
                                string BrandDescription = ExchangeOrderManager.GetEnumDescription(PriceMasterBrandEnum.Others);
                                if (item.BrandName_4 == BrandDescription)
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_4.ToLower())).Id;
                                    brandPL.Name = item.BrandName_4;
                                    brandPL.Price = item.Quote_P;
                                    brandPL.Mid_Price = item.Quote_Q;
                                    brandPL.Min_Price = item.Quote_R;
                                    brandPL.Scrap_Price = item.Quote_S;
                                    brandList.Add(brandPL);
                                }
                                else
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_4.ToLower())).Id;
                                    brandPL.Name = item.BrandName_4;
                                    brandPL.Price = item.Quote_P_High;
                                    brandPL.Mid_Price = item.Quote_Q_High;
                                    brandPL.Min_Price = item.Quote_R_High;
                                    brandPL.Scrap_Price = item.Quote_S_High;
                                    brandList.Add(brandPL);
                                }
                                //brandPL = new BrandPriceList();
                                //brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_4.ToLower())).Id;
                                //brandPL.Name = item.BrandName_4;
                                //brandPL.Price = item.Quote_P_High;
                                //brandPL.Mid_Price = item.Quote_Q_High;
                                //brandPL.Min_Price = item.Quote_R_High;
                                //brandPL.Scrap_Price = item.Quote_S_High;
                                //brandList.Add(brandPL);
                            }
                            //productsPriceList.Add(productsPrice);
                            #endregion
                            if (!string.IsNullOrEmpty(brandid))
                            {
                                brandList = brandList.Where(x => x.BrandId == Convert.ToInt32(brandid)).ToList();
                            }
                            productsType.Brand = brandList;
                            productsTyepeList.Add(productsType);


                        }
                        productsPricesDC = new ProductsPricesDataContract();
                        productsPricesDC.ProductsFromType = productsTyepeList;

                        productsPricesDC.CategoryId = (int)tblUniversalPriceMasterObjList[0].ProductCategoryId;
                        productsPricesDC.categoryName = tblUniversalPriceMasterObjList[0].ProductCategoryName;
                        productsPricesDC.SweetenerAmount = sweetener.SweetenerTotal.ToString();

                    }
                    productsPricesList = new List<ProductsPricesDataContract>();
                    productsPricesList.Add(productsPricesDC);
                    #endregion
                }
                else
                {
                    tblUniversalPriceMasterObjList = _universalPriceMasterRepository.GetList(x => x.IsActive == true
                    && (x.PriceMasterNameId != null && x.PriceMasterNameId.Equals(PriceMasterNameId))).ToList();
                    List<tblProductCategory> categoryList = new List<tblProductCategory>();

                    List<int?> categoryforfilter = tblUniversalPriceMasterObjList.OrderBy(x => x.ProductCategoryId).Select(x => x.ProductCategoryId).Distinct().ToList();
                    foreach (var cat in categoryforfilter)
                    {
                        tblProductCategory categoryObj = _productCategoryRepository.GetSingle(x => x.IsActive == true && x.IsAllowedForOld == true && x.Id == cat);
                        if (categoryObj != null)
                        {

                            categoryList.Add(categoryObj);
                        }
                    }
                    productsPricesList = new List<ProductsPricesDataContract>();
                    //  ProductsFromTypePriceList productsType = new ProductsFromTypePriceList();
                    foreach (tblProductCategory catObj in categoryList)
                    {
                        productsPricesDC = new ProductsPricesDataContract();
                        productsPricesDC.CategoryId = catObj.Id;
                        productsPricesDC.categoryName = catObj.Description;
                        productsPricesDC.SweetenerAmount = sweetener.SweetenerTotal.ToString();
                        List<tblUniversalPriceMaster> temppriceMasterList = tblUniversalPriceMasterObjList.Where(x => x.ProductCategoryId == catObj.Id && x.IsActive == true).ToList();
                        productsTyepeList = new List<ProductsFromTypePriceList>();
                        foreach (var item in temppriceMasterList)
                        {
                            ProductsFromTypePriceList productsType = new ProductsFromTypePriceList();
                            //Code to check type

                            productsType.name = item.ProductTypeName.Replace(System.Environment.NewLine, string.Empty);

                            string size = masterCategoryTypeList.FirstOrDefault(x => x.Id.Equals(item.ProductTypeId) && x.IsActive == true).Size;

                            if (size != null)
                            {
                                productsType.name = productsType.name + " " + "(" + size + ")";
                            }

                            productsType.ProducttypeId = (int)item.ProductTypeId;
                            productsType.id = item.PriceMasterUniversalId;

                            #region code to fill brand list

                            BrandPriceList brandPL = null;
                            brandList = new List<BrandPriceList>();
                            //For Brand 1
                            //For Brand 1
                            if (!string.IsNullOrEmpty(item.BrandName_1))
                            {

                                string BrandDescription = ExchangeOrderManager.GetEnumDescription(PriceMasterBrandEnum.Others);
                                if (item.BrandName_1 == BrandDescription)
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_1.ToLower())).Id;
                                    brandPL.Name = item.BrandName_1;
                                    brandPL.Price = item.Quote_P;
                                    brandPL.Mid_Price = item.Quote_Q;
                                    brandPL.Min_Price = item.Quote_R;
                                    brandPL.Scrap_Price = item.Quote_S;
                                    brandList.Add(brandPL);
                                }
                                else
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_1.ToLower())).Id;
                                    brandPL.Name = item.BrandName_1;
                                    brandPL.Price = item.Quote_P_High;
                                    brandPL.Mid_Price = item.Quote_Q_High;
                                    brandPL.Min_Price = item.Quote_R_High;
                                    brandPL.Scrap_Price = item.Quote_S_High;
                                    brandList.Add(brandPL);
                                }
                                //Old code for price f brand 2 by V.C
                                //brandPL = new BrandPriceList();
                                //brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_1.ToLower())).Id;
                                //brandPL.Name = item.BrandName_1;
                                //brandPL.Price = item.Quote_P_High;
                                //brandPL.Mid_Price = item.Quote_Q_High;
                                //brandPL.Min_Price = item.Quote_R_High;
                                //brandPL.Scrap_Price = item.Quote_S_High;
                                //brandList.Add(brandPL);
                            }

                            //For Brand 2
                            if (!string.IsNullOrEmpty(item.BrandName_2))
                            {


                                string BrandDescription = ExchangeOrderManager.GetEnumDescription(PriceMasterBrandEnum.Others);
                                if (item.BrandName_2 == BrandDescription)
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_2.ToLower())).Id;
                                    brandPL.Name = item.BrandName_2;
                                    brandPL.Price = item.Quote_P;
                                    brandPL.Mid_Price = item.Quote_Q;
                                    brandPL.Min_Price = item.Quote_R;
                                    brandPL.Scrap_Price = item.Quote_S;
                                    brandList.Add(brandPL);
                                }
                                else
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_2.ToLower())).Id;
                                    brandPL.Name = item.BrandName_2;
                                    brandPL.Price = item.Quote_P_High;
                                    brandPL.Mid_Price = item.Quote_Q_High;
                                    brandPL.Min_Price = item.Quote_R_High;
                                    brandPL.Scrap_Price = item.Quote_S_High;
                                    brandList.Add(brandPL);
                                }

                                //Old code for price f brand 2 by V.C
                                //brandPL = new BrandPriceList();
                                //brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_2.ToLower())).Id;
                                //brandPL.Name = item.BrandName_2;
                                //brandPL.Price = item.Quote_P_High;
                                //brandPL.Mid_Price = item.Quote_Q_High;
                                //brandPL.Min_Price = item.Quote_R_High;
                                //brandPL.Scrap_Price = item.Quote_S_High;
                                //brandList.Add(brandPL);
                            }

                            //For Brand 3
                            if (!string.IsNullOrEmpty(item.BrandName_3))
                            {
                                string BrandDescription = ExchangeOrderManager.GetEnumDescription(PriceMasterBrandEnum.Others);
                                if (item.BrandName_3 == BrandDescription)
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_3.ToLower())).Id;
                                    brandPL.Name = item.BrandName_3;
                                    brandPL.Price = item.Quote_P;
                                    brandPL.Mid_Price = item.Quote_Q;
                                    brandPL.Min_Price = item.Quote_R;
                                    brandPL.Scrap_Price = item.Quote_S;
                                    brandList.Add(brandPL);
                                }
                                else
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_3.ToLower())).Id;
                                    brandPL.Name = item.BrandName_3;
                                    brandPL.Price = item.Quote_P_High;
                                    brandPL.Mid_Price = item.Quote_Q_High;
                                    brandPL.Min_Price = item.Quote_R_High;
                                    brandPL.Scrap_Price = item.Quote_S_High;
                                    brandList.Add(brandPL);
                                }

                            }

                            //For Brand 4
                            if (!string.IsNullOrEmpty(item.BrandName_4))
                            {
                                string BrandDescription = ExchangeOrderManager.GetEnumDescription(PriceMasterBrandEnum.Others);
                                if (item.BrandName_4 == BrandDescription)
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_4.ToLower())).Id;
                                    brandPL.Name = item.BrandName_4;
                                    brandPL.Price = item.Quote_P;
                                    brandPL.Mid_Price = item.Quote_Q;
                                    brandPL.Min_Price = item.Quote_R;
                                    brandPL.Scrap_Price = item.Quote_S;
                                    brandList.Add(brandPL);
                                }
                                else
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_4.ToLower())).Id;
                                    brandPL.Name = item.BrandName_4;
                                    brandPL.Price = item.Quote_P_High;
                                    brandPL.Mid_Price = item.Quote_Q_High;
                                    brandPL.Min_Price = item.Quote_R_High;
                                    brandPL.Scrap_Price = item.Quote_S_High;
                                    brandList.Add(brandPL);
                                }
                                //brandPL = new BrandPriceList();
                                //brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_4.ToLower())).Id;
                                //brandPL.Name = item.BrandName_4;
                                //brandPL.Price = item.Quote_P_High;
                                //brandPL.Mid_Price = item.Quote_Q_High;
                                //brandPL.Min_Price = item.Quote_R_High;
                                //brandPL.Scrap_Price = item.Quote_S_High;
                                //brandList.Add(brandPL);
                            }

                            // For Other
                            //brandPL = new BrandPriceList();
                            //brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals("Others".ToLower())).Id;
                            //brandPL.Name = "Others";
                            //brandPL.Price = item.Quote_P;
                            //brandPL.Mid_Price = item.Quote_Q;
                            //brandPL.Min_Price = item.Quote_R;
                            //brandPL.Scrap_Price = item.Quote_S;
                            //brandList.Add(brandPL);
                            //productsPriceList.Add(productsPrice);
                            #endregion

                            productsType.Brand = brandList;
                            productsTyepeList.Add(productsType);

                        }

                        productsPricesDC.ProductsFromType = productsTyepeList;
                        productsPricesList.Add(productsPricesDC);

                    }

                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeOrderManager", "GetProductPrice", ex);
            }
            return productsPricesList;
        }

        /// <summary>
        /// Method to get the pricing code by user id
        /// </summary>
        /// <param name="uname">user Name</param>
        /// <returns>string</returns>
        public string GetPriceCodeByUserName(string uname)
        {
            _loginDetailsUTCRepository = new LoginDetailsUTCRepository();
            string priceCode = string.Empty;
            try
            {
                priceCode = _loginDetailsUTCRepository.GetSingle(x => x.username.Equals(uname)).PriceCode;
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeOrderManager", "GetPriceCodeByUserName", ex);
            }
            return priceCode;
        }

        #endregion

        #region Get Business Unit detail


        /// <summary>
        /// Method to Get  BU  by Id
        /// </summary>               
        /// <returns>BusinessUnitDataContract</returns>   
        public BusinessUnitDataContract GetBUById(int? id)
        {
            _businessUnitRepository = new BusinessUnitRepository();
            tblBusinessUnit businessUnit = null;
            BusinessUnitDataContract BUDC = null;
            try
            {
                businessUnit = _businessUnitRepository.GetSingle(x => x.BusinessUnitId == Convert.ToInt32(id));
                if (businessUnit != null)
                {
                    BUDC = GenericMapper<tblBusinessUnit, BusinessUnitDataContract>.MapObject(businessUnit);
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeOrderManager", "GetBUById", ex);
            }

            return BUDC;
        }


        #endregion

        #region Method to Get  Society  by Id
        /// <summary>
        /// Method to Get  Society  by Id
        /// </summary>               
        /// <returns>SocietyDataContract</returns>   
        public SocietyDataContract GetSocietyById(int id)
        {
            _societyRepository = new SocietyRepository();
            tblSociety societyObj = null;
            SocietyDataContract societyDC = null;
            try
            {
                societyObj = _societyRepository.GetSingle(x => x.SocietyId == id);
                if (societyObj != null)
                {
                    societyDC = GenericMapper<tblSociety, SocietyDataContract>.MapObject(societyObj);
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SocietyManager", "GetSocietyById", ex);
            }

            return societyDC;
        }
        #endregion

        #region manage exchange order

        /// <summary>
        /// manage exchange order
        /// </summary>
        /// <param name="exchangeOrderDC">Exchange Order Data Contract</param>
        /// <returns>ProductOrderResponseDataContract</returns>
        public ProductOrderResponseDataContract ManageExchangeOrder(ExchangeOrderDataContract exchangeOrderDC)
        {
            #region Variable Declaration
            int orderId = 0;
            CustomerManager customerInfo = new CustomerManager();
            ProductManager productOrderInfo = new ProductManager();
            _customerDetailsRepository = new CustomerDetailsRepository();
            _notificationManager = new NotificationManager();
            _businessUnitRepository = new BusinessUnitRepository();
            _productCategoryRepository = new ProductCategoryRepository();
            _productTypeRepository = new ProductTypeRepository();
            _voucherStatusRepository = new VoucherStatusRepository();
            _businessPartnerRepository = new BusinessPartnerRepository();
            _brandRepository = new BrandRepository();
            _modelNumberrepository = new ModelNumberRepository();
            _whatsAppMessageRepository = new WhatsappMessageRepository();

            SponserManager sponserManager = new SponserManager();
            logging = new Logging();
            ProductOrderResponseDataContract productOrderResponseDC = null;
            productOrderResponseDC = new ProductOrderResponseDataContract();
            _exchangeOrderRepository = new ExchangeOrderRepository();
            string sponsercode = string.Empty;
            string fileName = null;
            string voucherName = null;
            string BrandName = null;
            string ProductTypeName = null;
            string ProductCategoryName = null;
            string SelfQClink = null;
            string message = null;
            string responseforWhatasapp = string.Empty;
            string WhatssAppStatusEnum = string.Empty;
            string ResponseCode = string.Empty;
            string SendMessageFlag = null;
            string DaikinPassword = null;
            DateTime _dateTime = DateTime.Now.TrimMilliseconds();
            int resultAddCustormer = 0;
            int resultSaveExchOrder = 0;
            tblCustomerDetail customerObjForSoap = null;
            WhatasappResponse whatssappresponseDC = null;
            MailJetViewModel mailJet = new MailJetViewModel();
            MailJetMessage jetmessage = new MailJetMessage();
            MailJetFrom from = new MailJetFrom();
            MailjetTo to = new MailjetTo();
            string ZohoPushFlag = string.Empty;
            _configurationRepository = new ConfigurationRepository();
            _areaLocalityRepository = new AreaLocalityRepository();
            DaikinCustomerDataContract daikinCustomerDC = new DaikinCustomerDataContract();
            SingleCustomerDataContract daikinCustomerOldDC = new SingleCustomerDataContract();
            tblExchangeOrder exchangeOrderObj = new tblExchangeOrder();
            tblBusinessUnit businessUnit = null;
            #endregion

            try
            {
                if (exchangeOrderDC != null)
                {
                    SendMessageFlag = ConfigurationManager.AppSettings["sendsmsflag"].ToString();

                    if (string.IsNullOrEmpty(exchangeOrderDC.Address2))
                    {
                        exchangeOrderDC.Address2 = exchangeOrderDC.Address1;
                    }

                    if (string.IsNullOrEmpty(exchangeOrderDC.City))
                    {
                        exchangeOrderDC.City = exchangeOrderDC.City1;
                    }
                    #region Code to add Customer details in database
                    tblCustomerDetail customerObj = new tblCustomerDetail();
                    tblBusinessPartner tblBusinessPartner = new tblBusinessPartner();
                    tblBusinessPartner = _businessPartnerRepository.GetSingle(x => x.IsActive == true && x.BusinessPartnerId == exchangeOrderDC.BusinessPartnerId);
                    if (tblBusinessPartner != null && tblBusinessPartner.IsDefaultPickupAddress == true)
                    {
                        //customerObj.LastName = exchangeOrderDC.LastName;
                        customerObj.ZipCode = tblBusinessPartner.Pincode;
                        customerObj.Address1 = tblBusinessPartner.AddressLine1;
                        customerObj.Address2 = tblBusinessPartner.AddressLine2;
                        if (customerObj.Address2 == null)
                        {
                            customerObj.Address2 = tblBusinessPartner.AddressLine1;
                        }
                        customerObj.City = tblBusinessPartner.City;
                        customerObj.State = tblBusinessPartner.State;
                        customerObj.Email = exchangeOrderDC.Email;
                        customerObj.PhoneNumber = exchangeOrderDC.PhoneNumber;
                        customerObj.FirstName = exchangeOrderDC.FirstName;
                        customerObj.LastName = exchangeOrderDC.LastName;

                    }
                    else
                    {
                        customerObj.FirstName = exchangeOrderDC.FirstName;
                        customerObj.LastName = exchangeOrderDC.LastName;
                        customerObj.ZipCode = exchangeOrderDC.ZipCode;
                        customerObj.Address1 = exchangeOrderDC.Address1;
                        customerObj.Address2 = exchangeOrderDC.Address2;
                        customerObj.City = exchangeOrderDC.City;
                        customerObj.State = exchangeOrderDC.StateName;
                        if (exchangeOrderDC.BusinessUnitDataContract != null)
                        {
                            if (exchangeOrderDC.BusinessUnitDataContract.IsAreaLocality == true)
                            {
                                if (exchangeOrderDC.AreaLocality != "null")
                                {
                                    customerObj.AreaLocalityId = Convert.ToInt32(exchangeOrderDC.AreaLocality);
                                }
                            }
                        }
                        if (exchangeOrderDC.BusinessUnitId == Convert.ToInt32(BusinessUnitEnum.Lg) && exchangeOrderDC.Email == null)
                        {
                            customerObj.Email = "contacts@rdcel.com";
                        }
                        else
                        {
                            customerObj.Email = exchangeOrderDC.Email;
                        }
                        customerObj.PhoneNumber = exchangeOrderDC.PhoneNumber;
                    }

                    customerObj.IsActive = true;
                    customerObj.CreatedDate = currentDatetime;
                    _customerDetailsRepository.Add(customerObj);
                    resultAddCustormer = _customerDetailsRepository.SaveChanges();
                    #endregion

                    #region Code to add product order in database
                    if (customerObj != null && customerObj.Id > 0)
                    {

                        exchangeOrderObj.RegdNo = exchangeOrderDC.RegdNo;
                        exchangeOrderObj.ProductTypeId = exchangeOrderDC.ProductTypeId;
                        exchangeOrderObj.BrandId = exchangeOrderDC.BrandId;
                        exchangeOrderObj.Bonus = exchangeOrderDC.Bonus;
                        //exchangeOrderObj.ProductCondition = exchangeOrderDC.ProductCondition;
                        exchangeOrderObj.SponsorOrderNumber = exchangeOrderDC.SponsorOrderNumber;
                        exchangeOrderObj.LoginID = 1;
                        exchangeOrderObj.CustomerDetailsId = exchangeOrderDC.CustomerDetailsId;
                        exchangeOrderObj.CompanyName = exchangeOrderDC.CompanyName;
                        exchangeOrderObj.EstimatedDeliveryDate = exchangeOrderDC.EstimatedDeliveryDate;
                        if (exchangeOrderDC.BusinessUnitId == Convert.ToInt32(BusinessUnitEnum.Lg))
                        {
                            exchangeOrderObj.OtherCommunications = Convert.ToBoolean(exchangeOrderDC.Condition1);
                            exchangeOrderObj.OtherCommunications1 = Convert.ToBoolean(exchangeOrderDC.Condition2);
                            exchangeOrderObj.FollowupCommunication = Convert.ToBoolean(exchangeOrderDC.Condition3);
                            exchangeOrderObj.FollowupCommunication1 = Convert.ToBoolean(exchangeOrderDC.Condition4);
                        }

                        if (string.IsNullOrEmpty(exchangeOrderDC.StoreCode))
                        {
                            exchangeOrderObj.SaleAssociateName = exchangeOrderDC.AssociateName;
                            exchangeOrderObj.SalesAssociateEmail = exchangeOrderDC.AssociateEmail;
                            exchangeOrderObj.SalesAssociatePhone = exchangeOrderDC.StorePhoneNumber;
                        }
                        if (exchangeOrderDC.QualityCheck == 1)
                        {
                            exchangeOrderObj.ProductCondition = "Excellent";
                        }
                        if (exchangeOrderDC.QualityCheck == 2)
                        {
                            exchangeOrderObj.ProductCondition = "Good";
                        }
                        if (exchangeOrderDC.QualityCheck == 3)
                        {
                            exchangeOrderObj.ProductCondition = "Average";
                        }
                        if (exchangeOrderDC.QualityCheck == 4)
                        {
                            exchangeOrderObj.ProductCondition = "Not Working";
                        }

                        exchangeOrderObj.ExchangePrice = Convert.ToDecimal(exchangeOrderDC.ExchangePriceString);
                        exchangeOrderObj.BaseExchangePrice = Convert.ToDecimal(exchangeOrderDC.BasePrice);
                        exchangeOrderObj.OrderStatus = "Order Created";
                        exchangeOrderObj.StatusId = Convert.ToInt32(StatusEnum.OrderCreated);
                        exchangeOrderObj.IsActive = true;
                        exchangeOrderObj.CreatedDate = currentDatetime;
                        exchangeOrderObj.ModifiedDate = currentDatetime;

                        exchangeOrderObj.CustomerDetailsId = customerObj.Id;
                        exchangeOrderDC.CustomerDetailsId = customerObj.Id;
                        exchangeOrderObj.SweetenerBP = exchangeOrderDC.SweetenerBP;
                        exchangeOrderObj.SweetenerBU = exchangeOrderDC.SweetenerBu;
                        exchangeOrderObj.SweetenerDigi2l = exchangeOrderDC.SweetenerDigi2L;
                        exchangeOrderObj.Sweetener = exchangeOrderDC.SweetenerTotal;
                        exchangeOrderObj.BusinessPartnerId = exchangeOrderDC.BusinessPartnerId == 0 || exchangeOrderDC.BusinessPartnerId == 999999 ? null : exchangeOrderDC.BusinessPartnerId;
                        exchangeOrderObj.PurchasedProductCategory = exchangeOrderDC.PurchasedProductCategory;
                        exchangeOrderObj.StoreCode = exchangeOrderDC.StoreCode;
                        exchangeOrderObj.BusinessUnitId = exchangeOrderDC.BusinessUnitId;
                        exchangeOrderObj.ModelNumberId = exchangeOrderDC.ModelNumberId > 0 ? exchangeOrderDC.ModelNumberId : 0;
                        exchangeOrderObj.PriceMasterNameId = exchangeOrderDC.priceMasterNameID;
                        if (exchangeOrderDC.IsUnInstallation == true)
                        {
                            exchangeOrderObj.IsUnInstallationRequired = Convert.ToString(exchangeOrderDC.IsUnInstallation);
                            exchangeOrderObj.UnInstallationPrice = exchangeOrderDC.UnInstallationPrice;
                        }

                        if (exchangeOrderDC.BusinessUnitId == Convert.ToInt32(BusinessUnitEnum.Alliance))
                        {
                            exchangeOrderObj.StoreCode = exchangeOrderDC.StoreCode;
                            exchangeOrderObj.QCDate = exchangeOrderDC.QCDate;
                            exchangeOrderObj.StartTime = exchangeOrderDC.StartTime;
                            exchangeOrderObj.EndTime = exchangeOrderDC.EndTime;
                            exchangeOrderObj.IsDtoC = true;
                            exchangeOrderObj.UnInstallationPrice = Convert.ToDecimal(exchangeOrderDC.stringUnInstallationPrice);
                            exchangeOrderObj.IsUnInstallationRequired = exchangeOrderDC.IsUnInstallationRequired;
                            exchangeOrderObj.QCDate = exchangeOrderDC.QCDate;
                            exchangeOrderObj.StartTime = exchangeOrderDC.StartTime;
                            exchangeOrderObj.EndTime = exchangeOrderDC.EndTime;
                            exchangeOrderObj.SweetenerBP = exchangeOrderDC.SweetenerBP;
                            exchangeOrderObj.SweetenerBU = exchangeOrderDC.SweetenerBu;
                            exchangeOrderObj.SweetenerDigi2l = exchangeOrderDC.SweetenerDigi2L;

                        }

                        businessUnit = _businessUnitRepository.GetSingle(x => x.BusinessUnitId == exchangeOrderDC.BusinessUnitId && x.IsActive == true);


                        #region Changes for BU Validation Based Sweetner By VK Date 25-July-2023

                        if (exchangeOrderDC.IsValidationBasedSweetner == true)
                        {
                            exchangeOrderObj.IsExchangePriceWithoutSweetner = true;
                            //exchangeOrderObj.BaseExchangePrice = exchangeOrderObj.BaseExchangePrice;
                            //exchangeOrderObj.ExchangePrice = exchangeOrderObj.BaseExchangePrice;
                        }
                        else
                        {
                            exchangeOrderObj.IsExchangePriceWithoutSweetner = false;
                            //exchangeOrderObj.BaseExchangePrice = exchangeOrderObj.BaseExchangePrice;
                        }

                        #endregion

                        if (exchangeOrderDC.BusinessUnitId == Convert.ToInt32(BusinessUnitEnum.Alliance))
                        {
                            exchangeOrderDC.IsDtoC = true;
                        }
                        bool flag = false;


                        if (exchangeOrderDC.BusinessPartnerId > 0)
                        {


                            if (exchangeOrderDC.IsDifferedSettlement == true)
                            {
                                exchangeOrderObj.IsDefferedSettlement = true;
                            }
                            else
                            {
                                exchangeOrderObj.IsDefferedSettlement = false;
                            }


                        }
                        else
                        {
                            if (exchangeOrderDC.IsDifferedSettlement == false)
                            {
                                exchangeOrderObj.IsDefferedSettlement = exchangeOrderDC.IsDifferedSettlement;
                            }
                        }
                        if (exchangeOrderDC.NewProductCategoryId > 0)
                        {
                            exchangeOrderObj.NewProductCategoryId = exchangeOrderDC.NewProductCategoryId;
                        }
                        if (exchangeOrderDC.NewProductCategoryTypeId > 0)
                        {
                            exchangeOrderObj.NewProductTypeId = exchangeOrderDC.NewProductCategoryTypeId;
                        }
                        if (exchangeOrderDC.NewBrandId > 0)
                        {
                            exchangeOrderObj.NewBrandId = exchangeOrderDC.NewBrandId;
                        }
                        exchangeOrderObj.InvoiceNumber = exchangeOrderDC.InvoiceNumber;
                        exchangeOrderObj.ProductNumber = exchangeOrderDC.ProductSerialNumber;
                        if (exchangeOrderDC.Base64StringValue != null)
                        {
                            byte[] bytes = System.Convert.FromBase64String(exchangeOrderDC.Base64StringValue);
                            fileName = _dateTime.ToString("yyyyMMddHHmmssFFF") + Path.GetExtension("image.jpeg");
                            string rootPath = @HostingEnvironment.ApplicationPhysicalPath;
                            string filePath = ConfigurationManager.AppSettings["ExchangeInvoiceImage"].ToString() + fileName;
                            System.IO.File.WriteAllBytes(rootPath + filePath, bytes);
                            exchangeOrderObj.InvoiceImageName = fileName;
                        }
                        exchangeOrderObj.EmployeeId = exchangeOrderDC.EmployeeId;
                        exchangeOrderObj.PriceMasterNameId = exchangeOrderDC.priceMasterNameID != 0 ? exchangeOrderDC.priceMasterNameID : 1;
                        exchangeOrderObj.BusinessUnitId = Convert.ToInt32(exchangeOrderDC.BusinessUnitId);
                        exchangeOrderObj.IsCouponAplied = exchangeOrderDC.IsCouponAplied;
                        exchangeOrderObj.CouponId = exchangeOrderDC.CouponId;
                        exchangeOrderObj.CouponValue = exchangeOrderDC.CouponValue;
                        _exchangeOrderRepository.Add(exchangeOrderObj);
                        resultSaveExchOrder = _exchangeOrderRepository.SaveChanges();


                        flag = true;
                        if (flag)
                        {
                            productOrderResponseDC.OrderId = exchangeOrderObj.Id;
                            productOrderResponseDC.BusinessPartnerId = exchangeOrderDC.BusinessPartnerId;
                            productOrderResponseDC.BusinessUnitId = exchangeOrderDC.BusinessUnitId;
                            productOrderResponseDC.RegdNo = exchangeOrderDC.RegdNo;
                        }

                        #region Code to add order in transaction and history
                        OrderTransactionManager orderTransactionManager = new OrderTransactionManager();
                        ExchangeABBStatusHistoryManager exchangeABBStatusHistoryManager = new ExchangeABBStatusHistoryManager();
                        tblExchangeOrder exchangeOrder = _exchangeOrderRepository.GetSingle(x => x.Id.Equals(exchangeOrderObj.Id));
                        if (productOrderResponseDC != null && exchangeOrder != null)
                        {
                            //Code for Order tran
                            OrderTransactionDataContract orderTransactionDC = new OrderTransactionDataContract();
                            orderTransactionDC.OrderType = Convert.ToInt32(GraspCorn.Common.Enums.OrderTypeEnum.Exchange); ;
                            orderTransactionDC.ExchangeId = exchangeOrder.Id;
                            orderTransactionDC.SponsorOrderNumber = exchangeOrder.SponsorOrderNumber;
                            orderTransactionDC.RegdNo = exchangeOrder.RegdNo;
                            orderTransactionDC.ExchangePrice = exchangeOrder.ExchangePrice;
                            orderTransactionDC.Sweetner = exchangeOrder.Sweetener;

                            int tranid = orderTransactionManager.MangeOrderTransaction(orderTransactionDC);

                            //Code for Order history
                            if (tranid > 0)
                            {
                                ExchangeABBStatusHistoryDataContract exchangeABBStatusHistoryDC = new ExchangeABBStatusHistoryDataContract();
                                exchangeABBStatusHistoryDC.OrderType = Convert.ToInt32(GraspCorn.Common.Enums.OrderTypeEnum.Exchange);
                                exchangeABBStatusHistoryDC.OrderTransId = tranid;
                                exchangeABBStatusHistoryDC.ExchangeId = exchangeOrder.Id;
                                exchangeABBStatusHistoryDC.SponsorOrderNumber = exchangeOrder.SponsorOrderNumber;
                                exchangeABBStatusHistoryDC.RegdNo = exchangeOrder.RegdNo;
                                exchangeABBStatusHistoryDC.CustId = Convert.ToInt32(exchangeOrder.CustomerDetailsId);
                                exchangeABBStatusHistoryDC.StatusId = Convert.ToInt32(StatusEnum.OrderCreated);
                                exchangeABBStatusHistoryManager.MangeOrderHisotry(exchangeABBStatusHistoryDC);
                            }
                            #region Update Coupon table if coupon is Applied
                            if (exchangeOrderDC.IsCouponAplied == true)
                            {
                                _couponRepository = new CouponRepository();
                                tblCoupon tblcoupon = _couponRepository.GetSingle(coupon =>
                                  coupon.CouponId == exchangeOrderDC.CouponId &&
                                  (coupon.IsActive ?? false) &&
                                  !(coupon.IsUsed ?? true));
                                if (tblcoupon != null)
                                {
                                    tblcoupon.UsedCoupon = exchangeOrderDC.UsedCouponCode;
                                    tblcoupon.UsedCouponValue = exchangeOrderDC.CouponValue;
                                    tblcoupon.IsUsed = true;
                                    tblcoupon.OrderTransId = tranid;
                                    tblcoupon.ModifiedDate = currentDatetime;
                                    tblcoupon.ModifiedBy = exchangeOrderDC.ModifiedBy;
                                    _couponRepository.Update(tblcoupon);
                                    _couponRepository.SaveChanges();

                                }
                            }
                            #endregion
                        }
                        #endregion
                        if (exchangeOrderDC.BusinessUnitId != Convert.ToInt32(BusinessUnitEnum.Alliance))
                        {
                            if (!exchangeOrderDC.FormatName.Equals("Home"))
                            {
                                if (exchangeOrderDC.IsDifferedSettlement == true && exchangeOrderDC.IsVoucher == true && exchangeOrderDC.VoucherType == Convert.ToInt32(VoucherTypeEnum.Cash))
                                {
                                    #region For other Store
                                    voucherName = "Generated";
                                    tblVoucherStatu voucherStatu = _voucherStatusRepository.GetSingle(x => x.VoucherStatusName == voucherName);
                                    exchangeOrderObj.VoucherCodeExpDate = DateTime.Now.AddHours(Convert.ToDouble(businessUnit.VoucherExpiryTime));
                                    exchangeOrderObj.VoucherCode = GenerateVoucher();
                                    exchangeOrderObj.IsVoucherused = false;
                                    exchangeOrderObj.VoucherStatusId = voucherStatu.VoucherStatusId;
                                    exchangeOrderObj.IsActive = true;
                                    _exchangeOrderRepository.Update(exchangeOrderObj);
                                    _exchangeOrderRepository.SaveChanges();
                                    if (exchangeOrderObj.VoucherCode != null)
                                    {
                                        #region Code to send notification to customer for voucher generation
                                        //if(exchangeOrderObj.IsDefferedSettlement ==true &&)
                                        string voucherUrl = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Home/VC/" + exchangeOrderObj.Id;

                                        if (SendMessageFlag == "true")
                                        {
                                            message = NotificationConstants.SMS_VOUCHER_GENERATIONCash.Replace("[ExchPrice]", exchangeOrderDC.ExchangePriceString).Replace("[VCODE]", exchangeOrderObj.VoucherCode)
                                             .Replace("[VLink]", "( " + voucherUrl + " )").Replace("[STORENAME]", businessUnit.Name);
                                            _notificationManager.SendNotificationSMS(exchangeOrderDC.PhoneNumber, message, null);
                                        }
                                        jetmessage.From = new MailJetFrom() { Email = "hp@rdcel.com", Name = "ROCKINGDEALS - Customer  Care" };
                                        jetmessage.To = new List<MailjetTo>();
                                        jetmessage.To.Add(new MailjetTo() { Email = customerObj.Email.Trim(), Name = exchangeOrderDC.FirstName });
                                        jetmessage.Subject = businessUnit.Name + ": Exchange Voucher Detail";
                                        string TemplaTePath = ConfigurationManager.AppSettings["VoucherGenerationCash"].ToString();
                                        string FilePath = TemplaTePath;
                                        StreamReader str = new StreamReader(FilePath);
                                        string MailText = str.ReadToEnd();
                                        str.Close();
                                        MailText = MailText.Replace("[ExchPrice]", exchangeOrderObj.ExchangePrice.ToString()).Replace("[VCode]", exchangeOrderObj.VoucherCode).Replace("[FirstName]", exchangeOrderDC.FirstName)
                                            .Replace("[VLink]", voucherUrl).Replace("[STORENAME]", businessUnit.Name).Replace("[VALIDTILLDATE]", "7 days from quality check of your appliance");
                                        jetmessage.HTMLPart = MailText;
                                        mailJet.Messages = new List<MailJetMessage>();
                                        mailJet.Messages.Add(jetmessage);
                                        BillCloudServiceCall.MailJetSendMailService(mailJet);
                                        #endregion

                                        #region  sa code to send whatsappNotification For Voucher Generation for csh voucher
                                        WhatsappTemplatecashvoucher whatsappObj = new WhatsappTemplatecashvoucher();
                                        whatsappObj.userDetails = new UserDetails();
                                        whatsappObj.notification = new NotificationForCash();
                                        WhatsappNotificationManager whatsappNotificationManager = new WhatsappNotificationManager(); whatsappObj.notification.@params = new SendCashVoucherOnWhatssapp();
                                        whatsappObj.userDetails.number = exchangeOrderDC.PhoneNumber;
                                        //cash_voucher_
                                        whatsappObj.notification.templateId = NotificationConstants.Send_Voucher_Code_Template;
                                        whatsappObj.notification.@params.voucherAmount = exchangeOrderObj.ExchangePrice.ToString();
                                        whatsappObj.notification.@params.VoucherExpiry = Convert.ToDateTime(exchangeOrderObj.VoucherCodeExpDate).ToString("dd/MM/yyyy");
                                        whatsappObj.notification.@params.voucherCode = exchangeOrderObj.VoucherCode.ToString();
                                        whatsappObj.notification.@params.BrandName = businessUnit.Name.ToString();
                                        whatsappObj.notification.@params.VoucherLink = ConfigurationManager.AppSettings["BaseURL"] + "Home/V/" + exchangeOrderObj.Id;

                                        // Step 2: Convert WhatsappTemplate data into templateParams List
                                        List<string> templateParams = new List<string>
{
    whatsappObj.notification.@params.voucherAmount,   // Price
    whatsappObj.notification.@params.BrandName,       // Brand Name
    whatsappObj.notification.@params.voucherCode,     // Code
    whatsappObj.notification.@params.VoucherExpiry,   // Validity
    whatsappObj.notification.@params.VoucherLink      // Download URL
};

                                        // Send WhatsApp message
                                        HttpResponseDetails response = whatsappNotificationManager.SendWhatsAppMessageAsync(
                                 whatsappObj.notification.templateId,
                                                            whatsappObj.userDetails.number,
                                                    templateParams
                                        ).GetAwaiter().GetResult();

                                        ResponseCode = response.Response.StatusCode.ToString();
                                        WhatssAppStatusEnum = ExchangeOrderManager.GetEnumDescription(WhatssAppEnum.SuccessCode);


                                        if (ResponseCode == WhatssAppStatusEnum)
                                        {
                                            responseforWhatasapp = response.Content;
                                            if (responseforWhatasapp != null)
                                            {
                                                whatssappresponseDC = JsonConvert.DeserializeObject<WhatasappResponse>(responseforWhatasapp);
                                                tblWhatsAppMessage whatsapObj = new tblWhatsAppMessage();
                                                whatsapObj.TemplateName = NotificationConstants.send_voucher_generationcashe;
                                                whatsapObj.IsActive = true;
                                                whatsapObj.PhoneNumber = exchangeOrderDC.PhoneNumber;
                                                whatsapObj.SendDate = DateTime.Now;
                                                whatsapObj.msgId = whatssappresponseDC.msgId;
                                                _whatsAppMessageRepository.Add(whatsapObj);
                                                _whatsAppMessageRepository.SaveChanges();
                                            }
                                            else
                                            {
                                                string JsonObjectForExchangeOrder = JsonConvert.SerializeObject(exchangeOrderDC);
                                                logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", exchangeOrderDC.SponsorOrderNumber, JsonObjectForExchangeOrder);
                                            }
                                        }
                                        else
                                        {
                                            string JsonObjectForExchangeOrder = JsonConvert.SerializeObject(exchangeOrderDC);
                                            logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", exchangeOrderDC.SponsorOrderNumber, JsonObjectForExchangeOrder);
                                        }

                                        #endregion

                                    }
                                    //send sms

                                    #endregion

                                }
                                else if (exchangeOrderDC.IsDifferedSettlement == false && exchangeOrderDC.IsVoucher == true && exchangeOrderDC.VoucherType == Convert.ToInt32(VoucherTypeEnum.Discount))
                                {

                                    tblBusinessUnit tblBusinessUnit = _businessUnitRepository.GetSingle(x => x.BusinessUnitId == exchangeOrderDC.BusinessUnitId && x.IsActive == true && x.IsVoucherAfterQC == true);
                                    if (tblBusinessUnit != null)
                                    {
                                        bool flags = GenerateQCLInkBeforeSendVoucher(exchangeOrderDC);
                                        if (flags == true)
                                        {
                                            exchangeOrderDC.IsVoucherAfterQC = true;
                                           
                                        }
                                    }
                                     else   { 
                                    #region For other Store
                                    voucherName = "Generated";
                                    tblVoucherStatu voucherStatu = _voucherStatusRepository.GetSingle(x => x.VoucherStatusName == voucherName);
                                    exchangeOrderObj.VoucherCodeExpDate = DateTime.Now.AddHours(Convert.ToDouble(businessUnit.VoucherExpiryTime));
                                    exchangeOrderObj.VoucherCode = GenerateVoucher();
                                    exchangeOrderObj.IsVoucherused = false;
                                    exchangeOrderObj.VoucherStatusId = voucherStatu.VoucherStatusId;
                                    exchangeOrderObj.IsActive = true;
                                    _exchangeOrderRepository.Update(exchangeOrderObj);
                                    _exchangeOrderRepository.SaveChanges();

                                    //send sms
                                    #region code to send whatsappNotification For Voucher Generation
                                    WhatsappTemplate whatsappObj = new WhatsappTemplate();
                                    whatsappObj.userDetails = new UserDetails();
                                    whatsappObj.notification = new Notification();
                                    whatsappObj.notification.@params = new SendVoucherOnWhatssapp();
                                    WhatsappNotificationManager whatsappNotificationManager = new WhatsappNotificationManager();

                                    #region sa
                                    // Assign values
                                    whatsappObj.userDetails.number = exchangeOrderDC.PhoneNumber;
                                    whatsappObj.notification.templateId = NotificationConstants.Send_Voucher_Code_Template;
                                    whatsappObj.notification.@params.voucherAmount = exchangeOrderObj.ExchangePrice.ToString();
                                    whatsappObj.notification.@params.VoucherExpiry = Convert.ToDateTime(exchangeOrderObj.VoucherCodeExpDate).ToString("dd/MM/yyyy");
                                    whatsappObj.notification.@params.voucherCode = exchangeOrderObj.VoucherCode.ToString();
                                    whatsappObj.notification.@params.BrandName = businessUnit.Name.ToString();
                                    whatsappObj.notification.@params.VoucherLink =
ConfigurationManager.AppSettings["BaseURL"].ToString() + "Home/V/" + exchangeOrderObj.Id;

                                    // Step 2: Convert WhatsappTemplate data into templateParams List
                                    List<string> templateParams = new List<string>
                                   {
                                       whatsappObj.notification.@params.voucherAmount,  // Price
                                       whatsappObj.notification.@params.BrandName,      // Brand Name
                                       whatsappObj.notification.@params.voucherCode,    // Code
                                       whatsappObj.notification.@params.VoucherExpiry,  // Validity
                                       whatsappObj.notification.@params.VoucherLink     // Download URL
                                   };
                                    HttpResponseDetails response = whatsappNotificationManager.SendWhatsAppMessageAsync(
                                                        whatsappObj.notification.templateId,
                                                        whatsappObj.userDetails.number,
                                                        templateParams
                                                    ).GetAwaiter().GetResult();

                                    #endregion
                                    ResponseCode = response.Response.StatusCode.ToString();
                                    WhatssAppStatusEnum = ExchangeOrderManager.GetEnumDescription(WhatssAppEnum.SuccessCode);
                                    if (ResponseCode == WhatssAppStatusEnum)
                                    {
                                        // responseforWhatasapp = response.Content.ToString();
                                        string responseContent = response.Content;

                                        if (responseContent != null)
                                        {
                                            whatssappresponseDC = JsonConvert.DeserializeObject<WhatasappResponse>(responseContent);
                                            tblWhatsAppMessage whatsapObj = new tblWhatsAppMessage();
                                            whatsapObj.TemplateName = NotificationConstants.Send_Voucher_Code_Template;
                                            whatsapObj.IsActive = true;
                                            whatsapObj.PhoneNumber = exchangeOrderDC.PhoneNumber;
                                            whatsapObj.SendDate = DateTime.Now;
                                            whatsapObj.msgId = whatssappresponseDC.msgId;
                                            _whatsAppMessageRepository.Add(whatsapObj);
                                            _whatsAppMessageRepository.SaveChanges();
                                        }
                                        else
                                        {
                                            string JsonObjectForExchangeOrder = JsonConvert.SerializeObject(exchangeOrderDC);
                                            logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", exchangeOrderDC.SponsorOrderNumber, JsonObjectForExchangeOrder);
                                        }
                                    }
                                    else
                                    {
                                        string JsonObjectForExchangeOrder = JsonConvert.SerializeObject(exchangeOrderDC);
                                        logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", exchangeOrderDC.SponsorOrderNumber, JsonObjectForExchangeOrder);
                                    }
                                    #endregion



                                    #region Code to send notification to customer for voucher generation
                                    string voucherUrl = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Home/V/" + exchangeOrderObj.Id;
                                    if (SendMessageFlag == "true")
                                    {
                                        message = NotificationConstants.SMS_VoucherRedemption_Confirmation.Replace("[ExchPrice]", exchangeOrderDC.ExchangePriceString).Replace("[VCODE]", exchangeOrderObj.VoucherCode)
                                        .Replace("[VLink]", "( " + voucherUrl + " )").Replace("[STORENAME]", businessUnit.Name).Replace("[COMPANY]", businessUnit.Name).Replace("[VALIDTILLDATE]", Convert.ToDateTime(exchangeOrderObj.VoucherCodeExpDate).ToString("dd/MM/yyyy"));
                                        _notificationManager.SendNotificationSMS(exchangeOrderDC.PhoneNumber, message, null);
                                    }

                                    jetmessage.From = new MailJetFrom() { Email = "hp@rdcel.com", Name = "ROCKINGDEALS - Customer  Care" };
                                    jetmessage.To = new List<MailjetTo>();
                                    jetmessage.To.Add(new MailjetTo() { Email = customerObj.Email.Trim(), Name = exchangeOrderDC.FirstName });
                                    jetmessage.Subject = businessUnit.Name + ": Exchange Voucher Detail";
                                    string TemplaTePath = ConfigurationManager.AppSettings["VoucherGenerationInstant"].ToString();
                                    string FilePath = TemplaTePath;
                                    StreamReader str = new StreamReader(FilePath);
                                    string MailText = str.ReadToEnd();
                                    str.Close();
                                    MailText = MailText.Replace("[ExchPrice]", exchangeOrderObj.ExchangePrice.ToString()).Replace("[VCode]", exchangeOrderObj.VoucherCode).Replace("[FirstName]", exchangeOrderDC.FirstName)
                                    .Replace("[VLink]", voucherUrl).Replace("[STORENAME]", businessUnit.Name).Replace("[VALIDTILLDATE]", Convert.ToDateTime(exchangeOrderObj.VoucherCodeExpDate).ToString("dd/MM/yyyy"));
                                    jetmessage.HTMLPart = MailText;
                                    mailJet.Messages = new List<MailJetMessage>();
                                    mailJet.Messages.Add(jetmessage);
                                    BillCloudServiceCall.MailJetSendMailService(mailJet);
                                    #endregion
                                    #endregion
                                }
                                }
                            }
                        }
                        orderId = exchangeOrderObj.Id;
                        exchangeOrderDC.Id = orderId;
                        #region Daikin Customer Service Call
                        // Method to add customer details in Daikin DB with using Daikin Soap Create customer api. 
                        if (resultAddCustormer > 0 && exchangeOrderDC.BusinessUnitId == Convert.ToInt32(BusinessUnitEnum.Diakin))
                        {
                            ProductOrderDataContract productOrderDataContract = new ProductOrderDataContract();
                            _sponserOrderSyncManager = new SponsrOrderSyncManager();
                            string responseXML = null, responseGetCustomer = null;
                            string sponsorRefId = null;
                            List<tblAreaLocality> tblAreaLocalityList = null;
                            List<SelectListItem> AreaLocalityList = new List<SelectListItem>();
                            string ArealocalityName = null;
                            tblConfiguration configurationObj = _configurationRepository.GetSingle(x => x.Name == exchangeOrderDC.CompanyName && x.IsActive == true);
                            if (configurationObj != null)
                            {
                                DaikinPassword = configurationObj.Value;
                                if (customerObj.PhoneNumber != null)
                                {
                                    if (customerObj.AreaLocalityId != null && customerObj.AreaLocalityId > 0)
                                    {
                                        DataTable dt = _businessPartnerRepository.GetAreaLocalityById(Convert.ToInt32(customerObj.AreaLocalityId));
                                        if (dt != null && dt.Rows.Count > 0)
                                        {
                                            tblAreaLocalityList = GenericConversionHelper.DataTableToList<tblAreaLocality>(dt);
                                            if (tblAreaLocalityList != null)
                                            {
                                                foreach (var value in tblAreaLocalityList)
                                                {
                                                    ArealocalityName = value.AreaLocality;
                                                }
                                            }
                                        }

                                    }
                                    responseGetCustomer = _sponserOrderSyncManager.GetDiakinCustomerDetails(customerObj.PhoneNumber, DaikinPassword);
                                    if (responseGetCustomer != null)
                                    {
                                        XDocument XmlDoc = null;
                                        string Convertedjson = null;
                                        try
                                        {
                                            XmlDoc = XDocument.Parse(responseGetCustomer);
                                            Convertedjson = JsonConvert.SerializeXNode(XmlDoc);
                                        }
                                        catch (Exception exda)
                                        {
                                            logging.WriteAPIRequestToDB("ExchangeOrderManager", "ManageExchangeOrder", exchangeOrderDC.SponsorOrderNumber, XmlDoc.ToString());
                                            logging.WriteAPIRequestToDB("ExchangeOrderManager", "ManageExchangeOrder", exchangeOrderDC.SponsorOrderNumber, Convertedjson);
                                            LibLogging.WriteErrorToDB("ExchangeOrderManager", "ManageExchangeOrder", exda);
                                        }

                                        logging.WriteAPIRequestToDB("ExchangeOrderManager", "ManageExchangeOrder", exchangeOrderDC.RegdNo, Convertedjson);

                                        try
                                        {
                                            daikinCustomerDC = JsonConvert.DeserializeObject<DaikinCustomerDataContract>(Convertedjson);
                                        }
                                        catch (Exception exd)
                                        {
                                            daikinCustomerOldDC = JsonConvert.DeserializeObject<SingleCustomerDataContract>(Convertedjson);
                                            logging.WriteAPIRequestToDB("ExchangeOrderManager", "ManageExchangeOrder", exchangeOrderDC.SponsorOrderNumber, Convertedjson);
                                            LibLogging.WriteErrorToDB("ExchangeOrderManager", "ManageExchangeOrder", exd);
                                        }

                                        if (daikinCustomerDC.soapEnvelope != null)
                                        {
                                            if (daikinCustomerDC.soapEnvelope.soapBody.IndividualCustomerCollection != null)
                                            {
                                                sponsorRefId = daikinCustomerDC.soapEnvelope.soapBody.IndividualCustomerCollection.IndividualCustomer[0].CustomerID;
                                            }
                                            else
                                            {
                                                productOrderDataContract.PhoneNumber = customerObj.PhoneNumber;
                                                productOrderDataContract.FirstName = customerObj.FirstName;
                                                productOrderDataContract.LastName = customerObj.LastName;
                                                productOrderDataContract.Email = customerObj.Email;
                                                productOrderDataContract.City = customerObj.City;
                                                productOrderDataContract.ZipCode = customerObj.ZipCode;
                                                productOrderDataContract.Address1 = customerObj.Address1;
                                                productOrderDataContract.Address2 = customerObj.Address2;
                                                productOrderDataContract.AreaLocality = ArealocalityName;

                                                responseXML = _sponserOrderSyncManager.PushDiakinCustomer(productOrderDataContract, DaikinPassword);
                                                string Json = null;
                                                XDocument doc = null;
                                                try
                                                {
                                                    doc = XDocument.Parse(responseXML);
                                                    Json = JsonConvert.SerializeXNode(doc);
                                                }
                                                catch (Exception exdq)
                                                {
                                                    logging.WriteAPIRequestToDB("ExchangeOrderManager", "ManageExchangeOrder", exchangeOrderDC.RegdNo, doc.ToString());
                                                    logging.WriteAPIRequestToDB("ExchangeOrderManager", "ManageExchangeOrder", exchangeOrderDC.RegdNo, Json);
                                                    LibLogging.WriteErrorToDB("ExchangeOrderManager", "ManageExchangeOrder", exdq);
                                                }
                                                logging.WriteAPIRequestToDB("ExchangeOrderManager", "ManageExchangeOrder", exchangeOrderDC.RegdNo, Json);
                                                DaikinCustomerDataContract daikinCustomerNewDC = new DaikinCustomerDataContract();
                                                DaikinNewCustomerDataContractNew daikinNewCustomerObj = new DaikinNewCustomerDataContractNew();
                                                //daikinCustomerNewDC = JsonConvert.DeserializeObject<DaikinCustomerDataContract[]>(Json)[0];
                                                try
                                                {
                                                    daikinCustomerNewDC = JsonConvert.DeserializeObject<DaikinCustomerDataContract>(Json);
                                                    sponsorRefId = daikinCustomerNewDC.soapEnvelope.soapBody.ns2CustomerBundleMaintainConfirmation_sync_V1.Customer.InternalID;
                                                }
                                                catch (Exception exdsi)
                                                {
                                                    try
                                                    {
                                                        daikinNewCustomerObj = JsonConvert.DeserializeObject<DaikinNewCustomerDataContractNew>(Json);
                                                        sponsorRefId = daikinNewCustomerObj.soapEnvelope.soapBody.ns2CustomerBundleMaintainConfirmation_sync_V1.Customer.InternalID;
                                                    }
                                                    catch (Exception ex12)
                                                    {
                                                        LibLogging.WriteErrorToDB("ExchangeOrderManager", "ManageExchangeOrder", ex12);
                                                    }

                                                    logging.WriteAPIRequestToDB("ExchangeOrderManager", "ManageExchangeOrder", exchangeOrderDC.RegdNo, doc.ToString());
                                                    logging.WriteAPIRequestToDB("ExchangeOrderManager", "ManageExchangeOrder", exchangeOrderDC.RegdNo, Json);
                                                    LibLogging.WriteErrorToDB("ExchangeOrderManager", "ManageExchangeOrder", exdsi);
                                                }
                                            }
                                        }
                                        else if (daikinCustomerOldDC.soapEnvelope != null)
                                        {
                                            if (daikinCustomerOldDC.soapEnvelope.soapBody.IndividualCustomerCollection.IndividualCustomer != null)
                                            {
                                                sponsorRefId = daikinCustomerOldDC.soapEnvelope.soapBody.IndividualCustomerCollection.IndividualCustomer.CustomerID;
                                            }
                                            else
                                            {

                                                productOrderDataContract.PhoneNumber = customerObj.PhoneNumber;
                                                productOrderDataContract.FirstName = customerObj.FirstName;
                                                productOrderDataContract.LastName = customerObj.LastName;
                                                productOrderDataContract.Email = customerObj.Email;
                                                productOrderDataContract.City = customerObj.City;
                                                productOrderDataContract.ZipCode = customerObj.ZipCode;
                                                productOrderDataContract.Address1 = customerObj.Address1;
                                                productOrderDataContract.Address2 = customerObj.Address2;
                                                //productOrderDataContract.AreaLocality = exchangeOrderDC.AreaLocalityName;
                                                productOrderDataContract.AreaLocality = ArealocalityName;

                                                responseXML = _sponserOrderSyncManager.PushDiakinCustomer(productOrderDataContract, DaikinPassword);
                                                string Json = null;
                                                XDocument doc = null;
                                                try
                                                {
                                                    doc = XDocument.Parse(responseXML);
                                                    Json = JsonConvert.SerializeXNode(doc);
                                                }
                                                catch (Exception exdq)
                                                {
                                                    logging.WriteAPIRequestToDB("ExchangeOrderManager", "ManageExchangeOrder", exchangeOrderDC.RegdNo, doc.ToString());
                                                    logging.WriteAPIRequestToDB("ExchangeOrderManager", "ManageExchangeOrder", exchangeOrderDC.RegdNo, Json);
                                                    LibLogging.WriteErrorToDB("ExchangeOrderManager", "ManageExchangeOrder", exdq);
                                                }

                                                logging.WriteAPIRequestToDB("ExchangeOrderManager", "ManageExchangeOrder", exchangeOrderDC.RegdNo, Json);
                                                DaikinCustomerDataContract daikinCustomerNewDC = new DaikinCustomerDataContract();
                                                daikinCustomerNewDC = JsonConvert.DeserializeObject<DaikinCustomerDataContract>(Json);
                                                sponsorRefId = daikinCustomerNewDC.soapEnvelope.soapBody.ns2CustomerBundleMaintainConfirmation_sync_V1.Customer.InternalID;
                                            }
                                        }

                                    }
                                    else
                                    {
                                        productOrderDataContract.PhoneNumber = customerObj.PhoneNumber;
                                        productOrderDataContract.FirstName = customerObj.FirstName;
                                        productOrderDataContract.LastName = customerObj.LastName;
                                        productOrderDataContract.Email = customerObj.Email;
                                        productOrderDataContract.City = customerObj.City;
                                        productOrderDataContract.ZipCode = customerObj.ZipCode;
                                        productOrderDataContract.Address1 = customerObj.Address1;
                                        productOrderDataContract.Address2 = customerObj.Address2;
                                        //productOrderDataContract.AreaLocality = exchangeOrderDC.AreaLocalityName;
                                        productOrderDataContract.AreaLocality = ArealocalityName;

                                        responseXML = _sponserOrderSyncManager.PushDiakinCustomer(productOrderDataContract, DaikinPassword);
                                        string Json = null;
                                        XDocument doc = null;
                                        try
                                        {
                                            doc = XDocument.Parse(responseXML);
                                            Json = JsonConvert.SerializeXNode(doc);
                                        }
                                        catch (Exception exdq)
                                        {
                                            logging.WriteAPIRequestToDB("ExchangeOrderManager", "ManageExchangeOrder", exchangeOrderDC.RegdNo, doc.ToString());
                                            logging.WriteAPIRequestToDB("ExchangeOrderManager", "ManageExchangeOrder", exchangeOrderDC.RegdNo, Json);
                                            LibLogging.WriteErrorToDB("ExchangeOrderManager", "ManageExchangeOrder", exdq);
                                        }

                                        logging.WriteAPIRequestToDB("ExchangeOrderManager", "ManageExchangeOrder", exchangeOrderDC.RegdNo, Json);

                                        DaikinCustomerDataContract daikinCustomerNewDC = new DaikinCustomerDataContract();
                                        daikinCustomerNewDC = JsonConvert.DeserializeObject<DaikinCustomerDataContract>(Json);
                                        sponsorRefId = daikinCustomerNewDC.soapEnvelope.soapBody.ns2CustomerBundleMaintainConfirmation_sync_V1.Customer.InternalID;
                                    }
                                }
                            }
                            if (sponsorRefId != null && sponsorRefId != "")
                            {
                                _customerDetailsRepository = new CustomerDetailsRepository();
                                customerObjForSoap = _customerDetailsRepository.GetSingle(x => x.IsActive == true && x.Id == customerObj.Id);
                                if (customerObjForSoap != null)
                                {
                                    customerObjForSoap.SponsorRefId = sponsorRefId;
                                    _customerDetailsRepository.Update(customerObjForSoap);
                                    _customerDetailsRepository.SaveChanges();
                                }
                            }
                        }
                        #endregion


                        #region daikin serviceRequest creation 
                        // Create for store exchange order details on Daikin soap service 
                        if (resultSaveExchOrder > 0 && businessUnit.BusinessUnitId == Convert.ToInt32(BusinessUnitEnum.Diakin))
                        {
                            ProductOrderSoapServiceRequest productOrderDataContract = new ProductOrderSoapServiceRequest();
                            _sponserOrderSyncManager = new SponsrOrderSyncManager();
                            string responseXML = null;
                            _productTypeRepository = new ProductTypeRepository();
                            productOrderDataContract.TicketCategory = EnumHelper.DescriptionAttr(TicketCategoryEnum.ServiceRequest).ToString();
                            productOrderDataContract.SubType = EnumHelper.DescriptionAttr(SubTypeEnum.ThirtyTwoNewProduct).ToString();
                            productOrderDataContract.Status = EnumHelper.DescriptionAttr(StatusEnum.Open).ToString();
                            productOrderDataContract.CustomerId = customerObjForSoap.SponsorRefId;
                            // Set Product Type Dynamic
                            productOrderDataContract.Branch = exchangeOrderDC.StoreType;

                            string sendEmplpoyeeCode = null;
                            sendEmplpoyeeCode = ConfigurationManager.AppSettings["SendEmployeeCode"].ToString();
                            if (sendEmplpoyeeCode == "true")
                            {
                                //exchangeOrderDC.AssociateCode = "E7000617";
                                if (exchangeOrderDC.AssociateCode.Contains("E"))
                                {
                                    string SetAssociatecode = exchangeOrderDC.AssociateCode.Trim('E');
                                    productOrderDataContract.EmployeeId = SetAssociatecode;
                                }
                                else
                                {
                                    productOrderDataContract.EmployeeId = "";
                                }
                            }
                            else
                            {
                                productOrderDataContract.EmployeeId = "";
                            }

                            List<SelectListItem> Product_TypeList = EnumHelper.GetEnumForDropDown(ProductTypeEnum.AirPurifier);
                            if (Product_TypeList != null && Product_TypeList.Count > 0)
                            {
                                tblProductType tblProductType = _productTypeRepository.GetSingle(x => x.IsActive == true && x.Id == exchangeOrderObj.ProductTypeId);
                                if (tblProductType != null)
                                {
                                    foreach (var item in Product_TypeList)
                                    {
                                        if (item != null && item.Text != null && item.Text.Contains(tblProductType.Description))
                                        {
                                            int valuefortype = Convert.ToInt32(item.Value);
                                            if (valuefortype < 10)
                                            {
                                                productOrderDataContract.Product_Type = "0" + item.Value;
                                                break;
                                            }
                                            else
                                            {
                                                productOrderDataContract.Product_Type = item.Value;
                                                break;
                                            }

                                        }
                                    }
                                }
                            }
                            if (productOrderDataContract.Product_Type == null || productOrderDataContract.Product_Type == "")
                            {
                                productOrderDataContract.Product_Type = "01";
                            }
                            // Set Static Containt
                            productOrderDataContract.InstalledBaseId = "3934002";
                            productOrderDataContract.TypeCode = "10004";
                            productOrderDataContract.Content = "Demo and Installation for Buyback Scheme";
                            productOrderDataContract.WarrantyStatus = Convert.ToInt32(CoverageEnum.InWarranty).ToString();

                            responseXML = _sponserOrderSyncManager.PushDiakinServiceRequest(productOrderDataContract, DaikinPassword);
                            if (responseXML != null && responseXML != "")
                            {
                                XDocument doc = null;
                                string json = null;
                                try
                                {

                                    doc = XDocument.Parse(responseXML);
                                    json = JsonConvert.SerializeXNode(doc);
                                }
                                catch (Exception exd12)
                                {
                                    logging.WriteAPIRequestToDB("ExchangeOrderManager", "ManageExchangeOrder", exchangeOrderDC.RegdNo, json);
                                    logging.WriteAPIRequestToDB("ExchangeOrderManager", "ManageExchangeOrder", exchangeOrderDC.RegdNo, doc.ToString());
                                    LibLogging.WriteErrorToDB("ExchangeOrderManager", "ManageExchangeOrder", exd12);
                                }
                                logging.WriteAPIRequestToDB("ExchangeOrderManager", "ManageExchangeOrder", exchangeOrderDC.RegdNo, json);
                                RequestSerciceresponse myDeserializedClass = JsonConvert.DeserializeObject<RequestSerciceresponse>(json);
                                if (myDeserializedClass.soapEnvelope.soapBody.ns2ServiceRequestBundleMaintainConfirmation2_sync.ServiceRequest != null)
                                {
                                    exchangeOrderObj.SponsorServiceRefId = myDeserializedClass.soapEnvelope.soapBody.ns2ServiceRequestBundleMaintainConfirmation2_sync.ServiceRequest.ID;
                                    _exchangeOrderRepository.Update(exchangeOrderObj);
                                    _exchangeOrderRepository.SaveChanges();
                                }
                            }
                        }
                        #endregion

                        #region Code To send Email For ExchangeOrder
                        if (exchangeOrderDC.IsDifferedSettlement == true)
                        {


                            tblProductCategory prroductCategory = _productCategoryRepository.GetSingle(x => x.Id == exchangeOrderDC.ProductCategoryId && x.IsActive == true);
                            if (prroductCategory != null)
                            {
                                tblProductType productType = _productTypeRepository.GetSingle(x => x.Id == exchangeOrderDC.ProductTypeId);
                                if (productType != null)
                                {
                                    tblBrand brand = _brandRepository.GetSingle(x => x.Id == exchangeOrderDC.BrandId);
                                    if (brand != null)
                                    {
                                        BrandName = brand.Name;
                                        ProductTypeName = productType.Description;
                                        ProductCategoryName = prroductCategory.Description;
                                        //Add self QC Link
                                        string ErpBaseUrl = ConfigurationManager.AppSettings["ERPBaseURL"].ToString();
                                        string selfQC = ConfigurationManager.AppSettings["SelfQCUrl"].ToString();
                                        string url = ErpBaseUrl + "" + selfQC + "" + exchangeOrderDC.RegdNo;
                                        SelfQClink = url;
                                        jetmessage.From = new MailJetFrom() { Email = "hp@rdcel.com", Name = "ROCKINGDEALS - Customer  Care" };
                                        jetmessage.To = new List<MailjetTo>();
                                        jetmessage.To.Add(new MailjetTo() { Email = customerObj.Email.Trim(), Name = exchangeOrderDC.FirstName });
                                        jetmessage.Subject = businessUnit.Name + ": Exchange Detail";
                                        string TemplaTePath = ConfigurationManager.AppSettings["DefferedEmail"].ToString();
                                        string FilePath = TemplaTePath;
                                        StreamReader str = new StreamReader(FilePath);
                                        string MailText = str.ReadToEnd();
                                        str.Close();
                                        MailText = MailText.Replace("[CustomerName]", exchangeOrderDC.FirstName).Replace("[BusinessUnitName]", exchangeOrderDC.CompanyName).Replace("[SponserOrderNumber]", exchangeOrderObj.SponsorOrderNumber).Replace("[CreatedDate]", Convert.ToDateTime(_dateTime).ToString("dd/MM/yyyy")).Replace("[CustName]", exchangeOrderDC.FirstName).Replace("[CustMobile]", customerObj.PhoneNumber).Replace("[CustAdd1]", exchangeOrderDC.Address1)
                                            .Replace("[CustAdd2]", exchangeOrderDC.Address2).Replace("[State]", exchangeOrderDC.StateName).Replace("[PinCode]", exchangeOrderDC.ZipCode).Replace("[CustCity]", exchangeOrderDC.City).Replace("[ProductCategory]", prroductCategory.Description)
                                            .Replace("[OldProdType]", productType.Description).Replace("[OldBrand]", brand.Name).Replace("[Size]", productType.Size).Replace("[ExchangePrice]", exchangeOrderDC.ExchangePriceString).Replace("[EstimatedDeliveryDate]", exchangeOrderObj.EstimatedDeliveryDate).Replace("[SelfQCLink]", url);
                                        jetmessage.HTMLPart = MailText;
                                        mailJet.Messages = new List<MailJetMessage>();
                                        mailJet.Messages.Add(jetmessage);
                                        BillCloudServiceCall.MailJetSendMailService(mailJet);
                                    }
                                }

                            }
                            #region  sa Code Send Notification Via Whatssapp For Order Confirmation
                            OrderConfirmationTemplateExchangeUpdated whatsappObjforOrderConfirmation = new OrderConfirmationTemplateExchangeUpdated();
                            whatsappObjforOrderConfirmation.userDetails = new UserDetails();
                            whatsappObjforOrderConfirmation.notification = new OrderConfiirmationNotificationUpdated();

                            WhatsappNotificationManager whatsappNotificationManager = new WhatsappNotificationManager();
                            //whatsappObjforOrderConfirmation.notification.@params = new SendWhatssappForExcahangeConfirmationUpdated();
                            //whatsappObjforOrderConfirmation.userDetails.number = exchangeOrderDC.PhoneNumber;
                            //whatsappObjforOrderConfirmation.notification.sender = ConfigurationManager.AppSettings["Yello.aiSenderNumber"].ToString();
                            //whatsappObjforOrderConfirmation.notification.type = ConfigurationManager.AppSettings["Yellow.aiMesssaheType"].ToString();
                            //whatsappObjforOrderConfirmation.notification.templateId = NotificationConstants.orderConfirmationForExchangeUpdated;
                            //whatsappObjforOrderConfirmation.notification.@params.CustName = exchangeOrderDC.FirstName;
                            //whatsappObjforOrderConfirmation.notification.@params.Link = SelfQClink;
                            //whatsappObjforOrderConfirmation.notification.@params.Address = exchangeOrderDC.Address1 + " " + exchangeOrderDC.Address2;
                            //whatsappObjforOrderConfirmation.notification.@params.ProdCategory = ProductCategoryName;
                            //whatsappObjforOrderConfirmation.notification.@params.ProdType = ProductTypeName;
                            //whatsappObjforOrderConfirmation.notification.@params.RegdNO = exchangeOrderDC.RegdNo.ToString();
                            //whatsappObjforOrderConfirmation.notification.@params.Email = exchangeOrderDC.Email.ToString();
                            //whatsappObjforOrderConfirmation.notification.@params.PhoneNumber = exchangeOrderDC.PhoneNumber.ToString();
                            //whatsappObjforOrderConfirmation.notification.@params.CustomerName = exchangeOrderDC.FirstName + " " + exchangeOrderDC.LastName.ToString();
                            //string urlforwhatsapp = ConfigurationManager.AppSettings["Yellow.AiUrl"].ToString();
                            //IRestResponse responseConfirmation = WhatsappNotificationManager.Rest_InvokeWhatsappserviceCall(urlforwhatsapp, Method.POST, whatsappObjforOrderConfirmation);
                            //ResponseCode = responseConfirmation.StatusCode.ToString();
                            //WhatssAppStatusEnum = ExchangeOrderManager.GetEnumDescription(WhatssAppEnum.SuccessCode);
                            //if (ResponseCode == WhatssAppStatusEnum)
                            //{string orderConfirmationTemplateId = NotificationConstants.orderConfirmationForExchangeUpdated;
                            whatsappObjforOrderConfirmation.notification.templateId = NotificationConstants.orderConfirmationForExchange;
                            string phoneNumber = exchangeOrderDC.PhoneNumber;
                            string customerName = exchangeOrderDC.FirstName + " " + exchangeOrderDC.LastName;
                            string address = exchangeOrderDC.Address1 + " " + exchangeOrderDC.Address2;
                            string regNo = exchangeOrderDC.RegdNo.ToString();
                            string email = exchangeOrderDC.Email;
                            string selfQcLink = SelfQClink;
                            string productCategory = ProductCategoryName;
                            string producttype = ProductTypeName;

                            // Step 2: Prepare templateParams in the correct order expected by the WhatsApp template
                            List<string> templateParams = new List<string>
{
    exchangeOrderDC.FirstName,     // CustName
        regNo,                         // RegdNO
    productCategory,               // ProdCategory
    producttype,                   // ProdType
        customerName    ,               // CustomerName

    email,                         // Email
    phoneNumber,                   // PhoneNumber
        selfQcLink                  // Link

};

                            // Step 3: Call WhatsApp send method
                            HttpResponseDetails response = whatsappNotificationManager.SendWhatsAppMessageAsync(
                                                                whatsappObjforOrderConfirmation.notification.templateId,
                                                                phoneNumber,
                                                                templateParams
                                                            ).GetAwaiter().GetResult();

                            // Step 4: Handle response
                            ResponseCode = response.Response.StatusCode.ToString();
                            WhatssAppStatusEnum = ExchangeOrderManager.GetEnumDescription(WhatssAppEnum.SuccessCode);

                            if (ResponseCode == WhatssAppStatusEnum)
                            {

                                responseforWhatasapp = response.Content;
                                if (responseforWhatasapp != null)
                                {
                                    whatssappresponseDC = JsonConvert.DeserializeObject<WhatasappResponse>(responseforWhatasapp);
                                    tblWhatsAppMessage whatsapObj = new tblWhatsAppMessage();
                                    whatsapObj.TemplateName = NotificationConstants.orderConfirmationForExchange;
                                    whatsapObj.IsActive = true;
                                    whatsapObj.PhoneNumber = exchangeOrderDC.PhoneNumber;
                                    whatsapObj.SendDate = DateTime.Now;
                                    whatsapObj.msgId = whatssappresponseDC.msgId;
                                    _whatsAppMessageRepository.Add(whatsapObj);
                                    _whatsAppMessageRepository.SaveChanges();
                                }
                                else
                                {
                                    string JsonObjectForExchangeOrder = JsonConvert.SerializeObject(exchangeOrderDC);
                                    logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", exchangeOrderDC.SponsorOrderNumber, JsonObjectForExchangeOrder);
                                }
                            }
                            else
                            {
                                string JsonObjectForExchangeOrder = JsonConvert.SerializeObject(exchangeOrderDC);
                                logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", exchangeOrderDC.SponsorOrderNumber, JsonObjectForExchangeOrder);
                            }
                            #endregion

                        }
                        else
                        {

                        }
                        #endregion



                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                string Exch = JsonConvert.SerializeObject(exchangeOrderDC);
                logging.WriteAPIRequestToDB("ExchangeOrderManager", "ManageExchangeOrder", exchangeOrderDC.RegdNo, Exch);
                LibLogging.WriteErrorToDB("ExchangeOrderManager", "ManageExchangeOrder", ex);
            }

            return productOrderResponseDC;
        }




        #endregion

        #region 99999manage Bulk exchange order
        /// <summary>
        /// manage exchange order
        /// </summary>       
        /// <returns></returns>   
        public ProductOrderResponseDataContract ManageBulkExchangeOrder(ExchangeOrderDataContract exchangeOrderDC)
        {
            int orderId = 0;
            CustomerManager customerInfo = new CustomerManager();
            ProductManager productOrderInfo = new ProductManager();
            _customerDetailsRepository = new CustomerDetailsRepository();
            _notificationManager = new NotificationManager();
            _businessUnitRepository = new BusinessUnitRepository();
            _productCategoryRepository = new ProductCategoryRepository();
            _productTypeRepository = new ProductTypeRepository();
            _brandRepository = new BrandRepository();
            _whatsAppMessageRepository = new WhatsappMessageRepository();
            SponserManager sponserManager = new SponserManager();
            logging = new Logging();
            string responseforWhatasapp = string.Empty;
            WhatasappResponse whatssappresponseDC = null;
            string WhatssAppStatusEnum = string.Empty;
            string ResponseCode = string.Empty;
            ProductOrderResponseDataContract productOrderResponseDC = null;
            productOrderResponseDC = new ProductOrderResponseDataContract();
            _exchangeOrderRepository = new ExchangeOrderRepository();
            DateTime _dateTime = DateTime.Now.TrimMilliseconds();
            string sponsercode = string.Empty;
            string ZohoPushFlag = string.Empty;
            int PinCode = 0;
            try
            {
                if (exchangeOrderDC != null)
                {
                    #region Code to add Customer details in database
                    tblCustomerDetail customerObj = new tblCustomerDetail();
                    customerObj.FirstName = exchangeOrderDC.FirstName;
                    customerObj.LastName = exchangeOrderDC.LastName;
                    customerObj.ZipCode = exchangeOrderDC.ZipCode;
                    customerObj.Address1 = exchangeOrderDC.Address1;
                    customerObj.Address2 = exchangeOrderDC.Address2;
                    customerObj.City = exchangeOrderDC.City;

                    if (!string.IsNullOrEmpty(exchangeOrderDC.ZipCode))
                    {
                        PinCode = Convert.ToInt32(exchangeOrderDC.ZipCode);
                        PinCodeRepository pinCodeRepository = new PinCodeRepository();
                        tblPinCode pinCode = pinCodeRepository.GetSingle(x => x.ZipCode.Equals(PinCode));
                        customerObj.State = pinCode != null && !string.IsNullOrEmpty(pinCode.State) ? pinCode.State : null;
                    }
                    customerObj.Email = exchangeOrderDC.Email;
                    customerObj.PhoneNumber = exchangeOrderDC.PhoneNumber;
                    customerObj.IsActive = true;
                    customerObj.CreatedDate = currentDatetime;
                    _customerDetailsRepository.Add(customerObj);

                    _customerDetailsRepository.SaveChanges();

                    #endregion

                    #region Code to add product order in database
                    if (customerObj != null && customerObj.Id > 0)
                    {
                        tblExchangeOrder exchangeOrderObj = new tblExchangeOrder();
                        exchangeOrderObj.RegdNo = exchangeOrderDC.RegdNo;
                        exchangeOrderObj.ProductTypeId = exchangeOrderDC.ProductTypeId;
                        exchangeOrderObj.BrandId = exchangeOrderDC.BrandId;
                        exchangeOrderObj.Bonus = exchangeOrderDC.Bonus;
                        //exchangeOrderObj.ProductCondition = exchangeOrderDC.ProductCondition;
                        exchangeOrderObj.SponsorOrderNumber = exchangeOrderDC.SponsorOrderNumber;
                        exchangeOrderObj.LoginID = 1;
                        exchangeOrderObj.CustomerDetailsId = customerObj.Id;
                        exchangeOrderObj.CompanyName = exchangeOrderDC.CompanyName;
                        exchangeOrderObj.EstimatedDeliveryDate = exchangeOrderDC.EstimatedDeliveryDate;
                        exchangeOrderObj.ProductNumber = exchangeOrderDC.ProductNumber;
                        exchangeOrderObj.IsDefferedSettlement = true;
                        if (string.IsNullOrEmpty(exchangeOrderDC.StoreCode))
                        {
                            exchangeOrderObj.SaleAssociateName = exchangeOrderDC.AssociateName;
                            exchangeOrderObj.SalesAssociateEmail = exchangeOrderDC.AssociateEmail;
                            exchangeOrderObj.SalesAssociatePhone = exchangeOrderDC.StorePhoneNumber;
                        }
                        if (exchangeOrderDC.QualityCheck == 1)
                        {
                            exchangeOrderObj.ProductCondition = "Excellent";
                        }
                        if (exchangeOrderDC.QualityCheck == 2)
                        {
                            exchangeOrderObj.ProductCondition = "Good";
                        }
                        if (exchangeOrderDC.QualityCheck == 3)
                        {
                            exchangeOrderObj.ProductCondition = "Average";
                        }
                        if (exchangeOrderDC.QualityCheck == 4)
                        {
                            exchangeOrderObj.ProductCondition = "Not Working";
                        }

                        exchangeOrderObj.ExchangePrice = Convert.ToDecimal(exchangeOrderDC.ExchangePriceString);
                        exchangeOrderObj.BaseExchangePrice = Convert.ToDecimal(exchangeOrderDC.BasePrice);
                        exchangeOrderObj.SweetenerBP = Convert.ToDecimal(exchangeOrderDC.SweetenerBP);
                        exchangeOrderObj.SweetenerBU = Convert.ToDecimal(exchangeOrderDC.SweetenerBu);
                        exchangeOrderObj.SweetenerDigi2l = Convert.ToDecimal(exchangeOrderDC.SweetenerDigi2L);
                        exchangeOrderObj.Sweetener = Convert.ToDecimal(exchangeOrderDC.SweetenerTotal);
                        exchangeOrderObj.OrderStatus = "Order Created";
                        exchangeOrderObj.StatusId = 5;
                        exchangeOrderObj.IsActive = true;
                        exchangeOrderObj.CreatedDate = currentDatetime;
                        exchangeOrderObj.ModifiedDate = currentDatetime;

                        exchangeOrderObj.CustomerDetailsId = customerObj.Id;
                        exchangeOrderDC.CustomerDetailsId = customerObj.Id;

                        exchangeOrderObj.BusinessPartnerId = exchangeOrderDC.BusinessPartnerId == 0 ? null : exchangeOrderDC.BusinessPartnerId;
                        exchangeOrderObj.BusinessUnitId = exchangeOrderDC.BusinessUnitId != null ? exchangeOrderDC.BusinessUnitId : 0;
                        exchangeOrderObj.StoreCode = exchangeOrderDC.StoreCode;
                        exchangeOrderObj.Comment1 = exchangeOrderDC.Comment1;
                        exchangeOrderObj.Comment2 = exchangeOrderDC.Comment2;
                        exchangeOrderObj.Comment3 = exchangeOrderDC.Comment3;
                        exchangeOrderObj.PriceMasterNameId = exchangeOrderDC.priceMasterNameID;
                        tblBusinessUnit businessUnit = _businessUnitRepository.GetSingle(x => x.BusinessUnitId == exchangeOrderDC.BusinessUnitId);
                        if (businessUnit.BusinessUnitId == Convert.ToInt32(BusinessUnitEnum.D2C) || businessUnit.BusinessUnitId == Convert.ToInt32(BusinessUnitEnum.Onsitego))
                        {
                            exchangeOrderObj.IsDtoC = true;
                            exchangeOrderDC.IsDtoC = true;
                        }
                        else
                            exchangeOrderDC.IsDtoC = false;

                        bool flag = false;
                        _exchangeOrderRepository.Add(exchangeOrderObj);
                        _exchangeOrderRepository.SaveChanges();
                        flag = true;
                        if (flag)
                        {
                            productOrderResponseDC.OrderId = exchangeOrderObj.Id;
                            productOrderResponseDC.RegdNo = exchangeOrderDC.RegdNo;
                        }
                        orderId = exchangeOrderObj.Id;
                        exchangeOrderDC.Id = orderId;
                    }
                    #endregion



                    #region Code to add order in transaction and history
                    OrderTransactionManager orderTransactionManager = new OrderTransactionManager();
                    ExchangeABBStatusHistoryManager exchangeABBStatusHistoryManager = new ExchangeABBStatusHistoryManager();
                    tblExchangeOrder exchangeOrder = _exchangeOrderRepository.GetSingle(x => x.Id.Equals(orderId));
                    if (productOrderResponseDC != null && exchangeOrder != null)
                    {
                        //Code for Order tran
                        OrderTransactionDataContract orderTransactionDC = new OrderTransactionDataContract();
                        orderTransactionDC.OrderType = Convert.ToInt32(GraspCorn.Common.Enums.OrderTypeEnum.Exchange);
                        orderTransactionDC.ExchangeId = exchangeOrder.Id;
                        orderTransactionDC.SponsorOrderNumber = exchangeOrder.SponsorOrderNumber;
                        orderTransactionDC.RegdNo = exchangeOrder.RegdNo;
                        orderTransactionDC.ExchangePrice = exchangeOrder.ExchangePrice;
                        orderTransactionDC.Sweetner = exchangeOrder.Sweetener;
                        int tranid = orderTransactionManager.MangeOrderTransaction(orderTransactionDC);

                        //Code for Order history
                        if (tranid > 0)
                        {
                            ExchangeABBStatusHistoryDataContract exchangeABBStatusHistoryDC = new ExchangeABBStatusHistoryDataContract();
                            exchangeABBStatusHistoryDC.OrderType = Convert.ToInt32(GraspCorn.Common.Enums.OrderTypeEnum.Exchange);
                            exchangeABBStatusHistoryDC.OrderTransId = tranid;
                            exchangeABBStatusHistoryDC.ExchangeId = exchangeOrder.Id;
                            exchangeABBStatusHistoryDC.SponsorOrderNumber = exchangeOrder.SponsorOrderNumber;
                            exchangeABBStatusHistoryDC.RegdNo = exchangeOrder.RegdNo;
                            exchangeABBStatusHistoryDC.CustId = Convert.ToInt32(exchangeOrder.CustomerDetailsId);
                            exchangeABBStatusHistoryDC.StatusId = Convert.ToInt32(StatusEnum.OrderCreated); ;
                            exchangeABBStatusHistoryManager.MangeOrderHisotry(exchangeABBStatusHistoryDC);
                        }

                    }
                    #endregion
                    #region To send Notification for Order confirmation
                    tblBrand brandObj = _brandRepository.GetSingle(x => x.Id == exchangeOrderDC.BrandId && x.IsActive == true);
                    if (brandObj != null)
                    {
                        tblProductCategory prodCat = _productCategoryRepository.GetSingle(x => x.Id == exchangeOrderDC.ProductCategoryId && x.IsActive == true);
                        if (prodCat != null)
                        {
                            tblProductType productTypeObj = _productTypeRepository.GetSingle(x => x.Id == exchangeOrderDC.ProductTypeId && x.IsActive == true);
                            if (productTypeObj != null)
                            {
                                //WhatsappTemplateIntegration for Order Confirmation
                                string ErpBaseUrl = ConfigurationManager.AppSettings["ERPBaseURL"].ToString();
                                string selfQC = ConfigurationManager.AppSettings["SelfQCUrl"].ToString();
                                string selfqcurl = ErpBaseUrl + "" + selfQC + "" + exchangeOrderDC.RegdNo;
                                #region Code Send Notification Via Whatssapp For Order Confirmation
                                OrderConfirmationTemplateExchange whatsappObjforOrderConfirmation = new OrderConfirmationTemplateExchange();
                                whatsappObjforOrderConfirmation.userDetails = new UserDetails();
                                whatsappObjforOrderConfirmation.notification = new OrderConfiirmationNotification();
                                whatsappObjforOrderConfirmation.notification.@params = new SendWhatssappForExcahangeConfirmation();
                                whatsappObjforOrderConfirmation.userDetails.number = exchangeOrderDC.PhoneNumber;
                                whatsappObjforOrderConfirmation.notification.sender = ConfigurationManager.AppSettings["Yello.aiSenderNumber"].ToString();
                                whatsappObjforOrderConfirmation.notification.type = ConfigurationManager.AppSettings["Yellow.aiMesssaheType"].ToString();
                                whatsappObjforOrderConfirmation.notification.templateId = NotificationConstants.orderConfirmationForExchange;
                                whatsappObjforOrderConfirmation.notification.@params.CustName = exchangeOrderDC.FirstName + " " + exchangeOrderDC.LastName;
                                whatsappObjforOrderConfirmation.notification.@params.Link = selfqcurl;
                                whatsappObjforOrderConfirmation.notification.@params.ProductBrand = brandObj.Name;
                                whatsappObjforOrderConfirmation.notification.@params.ProdCategory = prodCat.Description;
                                whatsappObjforOrderConfirmation.notification.@params.ProdType = productTypeObj.Description;
                                whatsappObjforOrderConfirmation.notification.@params.RegdNO = exchangeOrderDC.RegdNo.ToString();
                                string urlforwhatsapp = ConfigurationManager.AppSettings["Yellow.AiUrl"].ToString();
                                IRestResponse responseConfirmation = WhatsappNotificationManager.Rest_InvokeWhatsappserviceCall(urlforwhatsapp, Method.POST, whatsappObjforOrderConfirmation);
                                ResponseCode = responseConfirmation.StatusCode.ToString();
                                WhatssAppStatusEnum = ExchangeOrderManager.GetEnumDescription(WhatssAppEnum.SuccessCode);
                                if (ResponseCode == WhatssAppStatusEnum)
                                {
                                    responseforWhatasapp = responseConfirmation.Content;
                                    if (responseforWhatasapp != null)
                                    {
                                        whatssappresponseDC = JsonConvert.DeserializeObject<WhatasappResponse>(responseforWhatasapp);
                                        tblWhatsAppMessage whatsapObj = new tblWhatsAppMessage();
                                        whatsapObj.TemplateName = NotificationConstants.orderConfirmationForExchange;
                                        whatsapObj.IsActive = true;
                                        whatsapObj.PhoneNumber = exchangeOrderDC.PhoneNumber;
                                        whatsapObj.SendDate = DateTime.Now;
                                        whatsapObj.msgId = whatssappresponseDC.msgId;
                                        _whatsAppMessageRepository.Add(whatsapObj);
                                        _whatsAppMessageRepository.SaveChanges();
                                    }
                                    else
                                    {
                                        string JsonObjectForExchangeOrder = JsonConvert.SerializeObject(exchangeOrderDC);
                                        logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", exchangeOrderDC.SponsorOrderNumber, JsonObjectForExchangeOrder);
                                    }
                                }
                                else
                                {
                                    string JsonObjectForExchangeOrder = JsonConvert.SerializeObject(exchangeOrderDC);
                                    logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", exchangeOrderDC.SponsorOrderNumber, JsonObjectForExchangeOrder);
                                }
                            }
                        }
                    }
                    #endregion
                    #endregion
                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeOrderManager", "ManageBulkExchangeOrder", ex);
            }

            return productOrderResponseDC;
        }
        #endregion

        #region add exchange order to DB
        /// <summary>
        /// Method to add the exchange Order
        /// </summary>       
        /// <returns></returns>   
        public int AddExchangeOrdertoDB(ProductOrderDataContract productOrderDataContract)
        {
            _exchangeOrderRepository = new ExchangeOrderRepository();
            int result = 0;
            try
            {
                tblExchangeOrder exchangeOrderInfo = SetOrderObjectJson(productOrderDataContract);
                {
                    _exchangeOrderRepository.Add(exchangeOrderInfo);

                    _exchangeOrderRepository.SaveChanges();
                    result = exchangeOrderInfo.Id;
                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeOrderManager", "AddExchangeOrdertoDB", ex);
            }

            return result;
        }
        #endregion

        #region Method to set Order info to table
        /// <summary>
        /// Method to set Order info to table
        /// </summary>
        /// <param name="productOrderDataContract">productOrderDataContract</param>     
        public tblExchangeOrder SetOrderObjectJson(ProductOrderDataContract productOrderDataContract)
        {
            tblExchangeOrder sponsorObj = null;
            try
            {
                if (productOrderDataContract != null)
                {
                    sponsorObj = new tblExchangeOrder();
                    sponsorObj.ProductTypeId = productOrderDataContract.ProductTypeId;
                    sponsorObj.BrandId = productOrderDataContract.BrandId;
                    sponsorObj.Bonus = productOrderDataContract.Bonus;
                    //sponsorObj.ProductCondition = productOrderDataContract.ProductCondition;
                    sponsorObj.SponsorOrderNumber = productOrderDataContract.SponsorOrderNumber;
                    sponsorObj.LoginID = 1;
                    sponsorObj.CustomerDetailsId = productOrderDataContract.CustomerDetailsId;
                    sponsorObj.CompanyName = productOrderDataContract.CompanyName;
                    sponsorObj.EstimatedDeliveryDate = productOrderDataContract.EstimatedDeliveryDate;
                    if (productOrderDataContract.ProductCondition == "1")
                    {
                        sponsorObj.ProductCondition = "Excellent";
                    }
                    if (productOrderDataContract.ProductCondition == "2")
                    {
                        sponsorObj.ProductCondition = "Good";
                    }
                    if (productOrderDataContract.ProductCondition == "3")
                    {
                        sponsorObj.ProductCondition = "Average";
                    }
                    if (productOrderDataContract.ProductCondition == "4")
                    {
                        sponsorObj.ProductCondition = "Not Working";
                    }
                    //sponsorObj.ExchPriceCode = productOrderDataContract.ExchPriceCode;
                    sponsorObj.OrderStatus = "Order Created";
                    sponsorObj.IsActive = true;
                    sponsorObj.CreatedDate = currentDatetime;

                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ProductManager", "SetOrderObjectJson", ex);
            }
            return sponsorObj;
        }
        #endregion

        #region set zoho Sponser detail
        /// <summary>
        /// Method to set details ('Sponser' form) ExchangeOrderDataContract exchangeOrderDC
        /// </summary>
        /// <param></param>
        /// <returns>model</returns>
        public SponserDataContract SetExchangeOrderObject(ExchangeOrderDataContract exchangeOrderDC)
        {
            _masterManager = new RDCEL.DocUpload.BAL.ZohoCreatorCall.MasterManager();
            SponserDataContract sponserObj = null;
            SponserSubCategoryListDataContract SponserSubCategoryListDC = null;
            SponsorCategoryListDataContract SponserCategoryListDC = null;
            BrandMasterListDataContract brandMasterListDC = null;
            ProductSizeListDataContract productSizeListDC = null;
            _sponserManager = new SponserManager();
            BrandRepository sponserRepository = new BrandRepository();
            ProductTypeRepository productTypeRepository = new ProductTypeRepository();
            ProductCategoryRepository categoryRepository = new ProductCategoryRepository();
            PriceMasterRepository priceMasterRepository = new PriceMasterRepository();
            BusinessPartnerRepository _businessPartnerRepository = new BusinessPartnerRepository();
            try
            {
                if (exchangeOrderDC != null)
                {
                    sponserObj = new SponserDataContract();
                    sponserObj.Sp_Order_No = exchangeOrderDC.SponsorOrderNumber;
                    sponserObj.Sponsor_Name = exchangeOrderDC.ZohoSponsorNumber;
                    sponserObj.Regd_No = exchangeOrderDC.RegdNo;
                    //sponserObj.Customer_Name = new CustomerName();
                    sponserObj.First_Name = exchangeOrderDC.FirstName;
                    sponserObj.Last_Name = exchangeOrderDC.LastName;
                    sponserObj.Customer_Pincode = exchangeOrderDC.ZipCode;
                    sponserObj.Customer_Address_1 = exchangeOrderDC.Address1;
                    sponserObj.Customer_Address_2 = exchangeOrderDC.Address2;
                    sponserObj.Customer_City = exchangeOrderDC.City;
                    sponserObj.Customer_State_Name = !string.IsNullOrEmpty(exchangeOrderDC.StateName) ? exchangeOrderDC.StateName : null;
                    sponserObj.Customer_Email_Address = exchangeOrderDC.Email;
                    sponserObj.Customer_Mobile = exchangeOrderDC.PhoneNumber;
                    sponserObj.Sweetener_Bonus_Amount_By_Sponsor = exchangeOrderDC.Sweetener != null ? exchangeOrderDC.Sweetener.ToString() : "0";
                    if (exchangeOrderDC.QCDate != null && exchangeOrderDC.StartTime != null && exchangeOrderDC.EndTime != null)
                    {
                        sponserObj.Preferred_QC_DateTime = exchangeOrderDC.QCDate + "  At " + exchangeOrderDC.StartTime + "-" + exchangeOrderDC.EndTime;
                        sponserObj.Store_Code = exchangeOrderDC.StoreCode;
                    }

                    tblBusinessPartner businessPartner = _businessPartnerRepository.GetSingle(x => x.IsActive == true && x.BusinessPartnerId == exchangeOrderDC.BusinessPartnerId);
                    if (exchangeOrderDC.IsDtoC == true)//!string.IsNullOrEmpty(exchangeOrderDC.FormatName) && exchangeOrderDC.FormatName.Equals("Home"))
                        sponserObj.Is_DtoC = "Yes";
                    else
                        sponserObj.Is_DtoC = "No";


                    if (businessPartner != null && businessPartner.IsORC == true && businessPartner.IsDefferedSettlement == true)
                    {
                        sponserObj.Is_Deferred = "Yes";
                    }
                    else if (exchangeOrderDC.BusinessUnitId.Equals(4) || exchangeOrderDC.BusinessUnitId.Equals(6) || exchangeOrderDC.BusinessUnitId.Equals(9))
                    {
                        sponserObj.Is_Deferred = "Yes";
                    }
                    else
                    {
                        sponserObj.Is_Deferred = "No";
                    }
                    sponserObj.Store_Code = exchangeOrderDC.StoreCode;
                    sponserObj.Associate_Name = exchangeOrderDC.AssociateName;
                    sponserObj.Store_Phone_Number = exchangeOrderDC.StorePhoneNumber;
                    sponserObj.Associate_Email = exchangeOrderDC.AssociateEmail;

                    sponserObj.Associate_Code = "";// exchangeOrderDC.SaleAssociateCode;

                    sponserObj.Purchased_Product_Category = exchangeOrderDC.PurchasedProductCategory;

                    if (exchangeOrderDC.QualityCheck == 1)
                    {
                        sponserObj.Cust_Declared_Qlty = "P";
                    }
                    else if (exchangeOrderDC.QualityCheck == 2)
                    {
                        sponserObj.Cust_Declared_Qlty = "Q";
                    }
                    else if (exchangeOrderDC.QualityCheck == 3)
                    {
                        sponserObj.Cust_Declared_Qlty = "R";
                    }
                    else if (exchangeOrderDC.QualityCheck == 4)
                    {
                        sponserObj.Cust_Declared_Qlty = "S";
                    }


                    if (exchangeOrderDC.BrandId != 0)
                    {
                        tblBrand brandObj = sponserRepository.GetSingle(x => x.Id.Equals(exchangeOrderDC.BrandId));
                        if (brandObj != null)
                        {

                            brandMasterListDC = _masterManager.GetAllBrand();
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
                    if (exchangeOrderDC.ProductTypeId != 0)
                    {
                        tblProductType productTypeObj = productTypeRepository.GetSingle(x => x.Id.Equals(exchangeOrderDC.ProductTypeId));
                        if (productTypeObj != null)
                        {
                            tblProductCategory productCatObj = categoryRepository.GetSingle(x => x.Id.Equals(productTypeObj.ProductCatId));
                            if (productCatObj != null)
                            {
                                // fill Product Category
                                SponserCategoryListDC = _masterManager.GetAllCategory();
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
                                SponserSubCategoryListDC = _masterManager.GetAllSubCategory();
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

                                productSizeListDC = _masterManager.GetAllProductSize();
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
                    DateTime EstimateDeldate = DateTime.ParseExact(exchangeOrderDC.EstimatedDeliveryDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    sponserObj.Estimate_Delivery_Date = EstimateDeldate.ToString("dd-MMM-yyyy");
                    if (exchangeOrderDC.EstimatedDeliveryDate != null)
                    {
                        DateTime date = DateTime.ParseExact(exchangeOrderDC.EstimatedDeliveryDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture).AddDays(1);
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
                    sponserObj.Alternate_Email = ConfigurationManager.AppSettings["AlternateEmail"].ToString(); // "logitics@digimart.co.in";
                    sponserObj.Problem_Description = "Exchange";
                    sponserObj.Is_Under_Warranty = "No";
                    sponserObj.Bulk_Mode = "No";

                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeOrderManager", "SetExchangeOrderObject", ex);
            }
            return sponserObj;
        }

        #endregion

        #region Method to update the expected pickup date by zoho order id
        /// <summary>
        /// Method to update the expected pickup date by zoho order id
        /// </summary>
        /// <param name="zohoExchangeOrderid">zohoExchangeOrderid</param>
        /// <returns></returns>
        public bool UpdateDeliveryStatusAndPickupDate(ProductOrderStatusDataContract productOrderStatusDataContract)
        {
            ProductManager productOrderInfo = new ProductManager();
            SponserManager sponserManager = new SponserManager();
            bool flag = false;
            string SchedulePickupDate = string.Empty;
            _exchangeOrderRepository = new ExchangeOrderRepository();
            string zohoExchangeOrderid = string.Empty;
            tblExchangeOrder tempexchangeOrderInfo = null;
            try
            {
                if (!string.IsNullOrEmpty(zohoExchangeOrderid))
                {

                    #region Code to add product order in database

                    tempexchangeOrderInfo = _exchangeOrderRepository.GetSingle(x => x.Id.Equals(productOrderStatusDataContract.OrderId));
                    if (tempexchangeOrderInfo != null)
                    {

                        tempexchangeOrderInfo.OrderStatus = productOrderStatusDataContract.Status;
                        tempexchangeOrderInfo.ModifiedDate = currentDatetime;
                        _exchangeOrderRepository.Update(tempexchangeOrderInfo);

                        _exchangeOrderRepository.SaveChanges();
                        flag = true;
                    }


                    #endregion

                    #region Update Zoho Status
                    zohoExchangeOrderid = tempexchangeOrderInfo.ZohoSponsorOrderId;

                    SponserListDataContract sponserListDC = sponserManager.GetSponserOrderById(zohoExchangeOrderid);
                    if (sponserListDC != null && sponserListDC.data != null && sponserListDC.data.Count > 0)
                    {

                        UpdateExchangeOrderPickupDateDataContract sponserStatusObj = null;
                        if (sponserListDC.data[0].Estimate_Delivery_Date != null)
                        {
                            DateTime date = DateTime.ParseExact(sponserListDC.data[0].Estimate_Delivery_Date, "dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture).AddDays(1);
                            SchedulePickupDate = date.ToString("dd-MMM-yyyy");
                        }

                        if (sponserListDC != null)
                        {
                            sponserStatusObj = new UpdateExchangeOrderPickupDateDataContract();
                            sponserStatusObj.ID = sponserListDC.data[0].ID;
                            sponserStatusObj.Latest_Status = "6";
                            sponserStatusObj.Order = sponserListDC.data[0].Order;
                            sponserStatusObj.Installation = "6";
                            sponserStatusObj.Expected_Pickup_Date = SchedulePickupDate;

                            SponserFormResponseDataContract sponserPickupdateResponseDC = sponserManager.UpdateSponserOrderEstimatePickupdate(sponserStatusObj);
                            if (sponserPickupdateResponseDC.code == 3000)
                                flag = true;
                        }
                    }
                    #endregion

                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeOrderManager", "UpdatePickupDate", ex);
            }
            return flag;
        }
        #endregion

        #region Code to manage voucher

        /// <summary>
        /// Method to save voucher code detail in table
        /// </summary>
        /// <param name="voucherData">voucherData</param>
        /// <returns>int</returns>
        public int AddVouchertoDB(VoucherDataContract voucherData)
        {
            _sponserManager = new SponserManager();
            _voucherVerificationRepository = new VoucherVerificationRepository();
            _exchangeOrderRepository = new ExchangeOrderRepository();
            tblExchangeOrder exchangeObj = new tblExchangeOrder();
            _businessUnitRepository = new BusinessUnitRepository();
            _businessPartnerRepository = new BusinessPartnerRepository();
            _customerDetailsRepository = new CustomerDetailsRepository();
            _voucherStatusRepository = new VoucherStatusRepository();
            _modelNumberrepository = new ModelNumberRepository();
            _productCategoryRepository = new ProductCategoryRepository();
            _productTypeRepository = new ProductTypeRepository();
            logging = new Logging();
            string WhatssAppStatusEnum = string.Empty;
            string ResponseCode = string.Empty;
            VoucherVerificationResponseViewModel sucessObj = null;
            tblBusinessPartner businessPartnerObj = null;
            _whatsAppMessageRepository = new WhatsappMessageRepository();
            WhatasappResponse whatssappresponseDC = null;
            int result = 0;
            bool isVoucherProccessed = false;
            string responseforWhatasapp = string.Empty;
            decimal sweetner = 0;
            string voucherStatusName = null;
            string unique = "_C";
            try
            {
                tblVoucherVerfication voucherVerfication = _voucherVerificationRepository.GetSingle(x => x.ExchangeOrderId == voucherData.ExchangeOrderDataContract.Id && x.NewProductCategoryId == voucherData.ExchangeOrderDataContract.ProductCategoryId);
                tblBusinessUnit businessUnit = _businessUnitRepository.GetSingle(x => x.BusinessUnitId == voucherData.ExchangeOrderDataContract.BusinessUnitDataContract.BusinessUnitId);
                tblCustomerDetail customerDetail = _customerDetailsRepository.GetSingle(x => x.Id == voucherData.ExchangeOrderDataContract.CustomerDetailsId);

                if (voucherVerfication == null)
                {
                    voucherVerfication = GenericMapper<VoucherDataContract, tblVoucherVerfication>.MapObject(voucherData);
                    voucherVerfication.IsVoucherused = false;
                    voucherVerfication.IsActive = true;
                    voucherVerfication.CreatedDate = DateTime.Now.TrimMilliseconds();
                    voucherVerfication.NewProductTypeId = voucherData.NewProductCategoryTypeId;
                    voucherVerfication.InvoiceNumber = voucherData.InvoiceNumber;
                    //voucherVerfication.SerialNumber = voucherData.SerialNumber;
                    _voucherVerificationRepository.Add(voucherVerfication);
                    _voucherVerificationRepository.SaveChanges();
                    result = voucherVerfication.VoucherVerficationId;

                    #region Code to update the Bill Cloud API  (Voucher Reedeemed)
                    tblExchangeOrder exchangeOrder = _exchangeOrderRepository.GetSingle(x => x.Id == voucherData.ExchangeOrderId);
                    string billcloudcall = ConfigurationManager.AppSettings["BillcloudCallActive"].ToString();
                    if (billcloudcall == "true")
                    {
                        VoucherVerificationViewModel voucherverificationvm = new VoucherVerificationViewModel();
                        voucherverificationvm.data = new VoucherVerificationData();
                        voucherverificationvm.data.event_id = "CAPTURE_VOUCHER"; // "CAPTURE_VOUCHER"; /
                        voucherverificationvm.data.rrn = exchangeOrder.SponsorOrderNumber + unique;
                        voucherverificationvm.data.dao_name = businessUnit != null ? businessUnit.Name.Trim() : string.Empty;
                        voucherverificationvm.data.payload = new VoucherVerificationPayload();
                        voucherverificationvm.data.payload.service_id = "EXCHANGE";
                        voucherverificationvm.data.payload.amount = Convert.ToInt32(exchangeOrder.ExchangePrice).ToString();

                        if (exchangeOrder.IsDtoC == true)
                            sweetner = businessUnit.SweetnerForDTC != null ? Convert.ToDecimal(exchangeOrder.Sweetener) : 0;
                        else
                            sweetner = businessUnit.SweetnerForDTD != null ? Convert.ToDecimal(exchangeOrder.Sweetener) : 0;
                        voucherverificationvm.data.payload.sweetener = Convert.ToInt32(sweetner).ToString();
                        voucherverificationvm.data.payload.expiry = exchangeOrder.VoucherCodeExpDate != null ? Convert.ToDateTime(exchangeOrder.VoucherCodeExpDate).ToString("MM/dd/yyyy hh:mm:ss") : string.Empty;
                        voucherverificationvm.data.payload.voucher_id = exchangeOrder.VoucherCode.ToString();
                        string buid = voucherData.ExchangeOrderDataContract.BusinessUnitDataContract.BusinessUnitId.ToString();
                        businessPartnerObj = _businessPartnerRepository.GetSingle(x => x.BusinessPartnerId == voucherData.BusinessPartnerId);
                        voucherverificationvm.data.payload.dealer_ref_id = !string.IsNullOrEmpty(businessPartnerObj.AssociateCode) ? businessPartnerObj.AssociateCode.Trim() : string.Empty;
                        voucherverificationvm.data.payload.acquirer_ref_id = !string.IsNullOrEmpty(businessPartnerObj.AssociateCode) ? businessPartnerObj.AssociateCode.Trim() : string.Empty;
                        voucherverificationvm.data.payload.beneficiary_ref_id = exchangeOrder.CustomerDetailsId > 0 ? exchangeOrder.CustomerDetailsId.ToString() : string.Empty;
                        voucherverificationvm.data.payload.consumer_ref_id = exchangeOrder.CustomerDetailsId > 0 ? exchangeOrder.CustomerDetailsId.ToString() : string.Empty;
                        voucherverificationvm.data.payload.issuer_ref_id = buid;
                        voucherverificationvm.data.payload.abrand_ref_id = buid;
                        voucherverificationvm.data.payload.merchant_ref_id = "UTCDIGITAL";


                        IRestResponse response = BillCloudServiceCall.Rest_InvokeZohoInvoiceServiceForPlainText(ConfigurationManager.AppSettings["VoucherProcess"].ToString(), Method.POST, voucherverificationvm);
                        if (response != null)
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                sucessObj = JsonConvert.DeserializeObject<VoucherVerificationResponseViewModel>(response.Content);
                                if (sucessObj != null && sucessObj.data != null && sucessObj.data.status.ToLower().Equals("success"))
                                {
                                    isVoucherProccessed = true;
                                }
                                else
                                {
                                    Logging logging = new Logging();
                                    logging.WriteErrorToDB("ExchangeOrderManager", "AddVouchertoDB", exchangeOrder.SponsorOrderNumber, response);
                                }
                            }
                        }
                    }
                    else
                    {
                        isVoucherProccessed = true;
                    }

                    if (isVoucherProccessed)
                    {
                        voucherStatusName = "Captured";
                        tblVoucherStatu tblVoucherStatu = _voucherStatusRepository.GetSingle(x => x.VoucherStatusName == voucherStatusName);
                        voucherVerfication = new tblVoucherVerfication();
                        _voucherVerificationRepository = new VoucherVerificationRepository();
                        voucherVerfication = _voucherVerificationRepository.GetSingle(x => x.VoucherVerficationId == result);
                        voucherVerfication.ExchangePrice = Convert.ToDecimal(exchangeOrder.ExchangePrice);
                        if (exchangeOrder.IsDtoC == true)
                        {
                            voucherVerfication.Sweetneer = Convert.ToDecimal(exchangeOrder.Sweetener);
                        }
                        else
                        {
                            voucherVerfication.Sweetneer = Convert.ToDecimal(exchangeOrder.Sweetener);
                        }
                        voucherVerfication.IsVoucherused = true;
                        voucherVerfication.VoucherStatusId = tblVoucherStatu.VoucherStatusId;
                        _voucherVerificationRepository.Update(voucherVerfication);
                        _voucherVerificationRepository.SaveChanges();
                        result = voucherVerfication.VoucherVerficationId;

                        if (exchangeOrder != null)
                        {
                            exchangeOrder.IsVoucherused = true;
                            exchangeOrder.VoucherStatusId = tblVoucherStatu.VoucherStatusId;
                            exchangeOrder.BusinessPartnerId = voucherVerfication.BusinessPartnerId;
                            exchangeObj.SaleAssociateName = voucherData.ExchangeOrderDataContract.SaleAssociateName;
                            exchangeObj.SalesAssociateEmail = voucherData.ExchangeOrderDataContract.AssociateEmail;
                            exchangeObj.SalesAssociatePhone = voucherData.ExchangeOrderDataContract.StorePhoneNumber;
                            exchangeOrder.SerialNumber = voucherData.SerialNumber;
                            _exchangeOrderRepository.Update(exchangeOrder);
                            _exchangeOrderRepository.SaveChanges();
                        }
                    }
                    #endregion

                    #region Update Voucher Detail in Zoho
                    tblVoucherStatu voucherStatus = _voucherStatusRepository.GetSingle(x => x.VoucherStatusId == voucherVerfication.VoucherStatusId);
                    string ZohoPushFlag = ConfigurationManager.AppSettings["ZohoPush"].ToString();
                    if (ZohoPushFlag == "true")
                    {
                        ExchageOrderVoucherUpdateDataContract exchOrderObj = _sponserManager.SetUpdateExchangeVoucherDetail(exchangeOrder.ZohoSponsorOrderId, Convert.ToInt32(exchangeOrder.ExchangePrice).ToString(), voucherVerfication, businessPartnerObj, voucherStatus.VoucherStatusName);
                        exchOrderObj.Associate_Email = voucherData.ExchangeOrderDataContract.AssociateEmail;
                        exchOrderObj.Associate_Name = voucherData.ExchangeOrderDataContract.AssociateName;
                        exchOrderObj.Retailer_Phone_Number = voucherData.ExchangeOrderDataContract.StorePhoneNumber;
                        _sponserManager.UpdateVoucherDetailinExchangeOrder(exchOrderObj);
                    }

                    #endregion
                    #region Code To send Notification for working Product
                    tblProductType producttype = _productTypeRepository.GetSingle(x => x.Id == exchangeOrder.ProductTypeId);
                    if (producttype != null)
                    {
                        tblProductCategory productCategoryobj = _productCategoryRepository.GetSingle(x => x.Id == producttype.ProductCatId);
                        if (productCategoryobj != null)
                        {
                            string productCondition = ExchangeOrderManager.GetEnumDescription((ProductConditionEnum.Excellent));
                            if (exchangeOrder.ProductCondition == productCondition)
                            {
                                #region code to send whatsappNotification For Voucher verification
                                voucherCapture whatsappObj = new voucherCapture();
                                whatsappObj.userDetails = new UserDetails();
                                whatsappObj.notification = new vaucherCaptureNotification();
                                whatsappObj.notification.@params = new vouchercaptureProperties();
                                whatsappObj.userDetails.number = customerDetail.PhoneNumber;
                                whatsappObj.notification.sender = ConfigurationManager.AppSettings["Yello.aiSenderNumber"].ToString();
                                whatsappObj.notification.type = ConfigurationManager.AppSettings["Yellow.aiMesssaheType"].ToString();
                                whatsappObj.notification.templateId = NotificationConstants.voucher_capture_working;
                                whatsappObj.notification.@params.companyName = exchangeOrder.CompanyName.ToString();
                                whatsappObj.notification.@params.customerName = customerDetail.FirstName;
                                whatsappObj.notification.@params.vouchercode = exchangeOrder.VoucherCode;
                                whatsappObj.notification.@params.oldProductcategory = productCategoryobj.Description;
                                whatsappObj.notification.@params.olsProductCategory = productCategoryobj.Description;
                                whatsappObj.notification.@params.workingQualities = productCategoryobj.CommentForWorking;
                                string url = ConfigurationManager.AppSettings["Yellow.AiUrl"].ToString();
                                IRestResponse responsewhatsapp = WhatsappNotificationManager.Rest_InvokeWhatsappserviceCall(url, Method.POST, whatsappObj);
                                ResponseCode = responsewhatsapp.StatusCode.ToString();
                                WhatssAppStatusEnum = ExchangeOrderManager.GetEnumDescription(WhatssAppEnum.SuccessCode);
                                if (ResponseCode == WhatssAppStatusEnum)
                                {
                                    responseforWhatasapp = responsewhatsapp.Content;
                                    if (responseforWhatasapp != null)
                                    {
                                        whatssappresponseDC = JsonConvert.DeserializeObject<WhatasappResponse>(responseforWhatasapp);
                                        tblWhatsAppMessage whatsapObj = new tblWhatsAppMessage();
                                        whatsapObj.TemplateName = NotificationConstants.Test_Template;
                                        whatsapObj.IsActive = true;
                                        whatsapObj.PhoneNumber = customerDetail.PhoneNumber;
                                        whatsapObj.SendDate = DateTime.Now;
                                        whatsapObj.msgId = whatssappresponseDC.msgId;
                                        _whatsAppMessageRepository.Add(whatsapObj);
                                        _whatsAppMessageRepository.SaveChanges();
                                    }
                                    else
                                    {
                                        string ExchOrderObj = JsonConvert.SerializeObject(voucherData);
                                        logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", voucherData.ExchangeOrderDataContract.SponsorOrderNumber, ExchOrderObj);
                                    }
                                }
                                else
                                {
                                    string ExchOrderObj = JsonConvert.SerializeObject(voucherData);
                                    logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", voucherData.ExchangeOrderDataContract.SponsorOrderNumber, ExchOrderObj);
                                }
                            }
                        }

                        #endregion
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeOrderManager", "AddVouchertoDB", ex);
            }

            return result;
        }
        #endregion

        #region Add vouchercode to ExchangeOrder Table
        public int AddVouchertoExchangeOrderTable(VoucherDataContract voucherData)
        {
            _voucherVerificationRepository = new VoucherVerificationRepository();
            _exchangeOrderRepository = new ExchangeOrderRepository();
            _businessUnitRepository = new BusinessUnitRepository();
            _voucherStatusRepository = new VoucherStatusRepository();
            _customerDetailsRepository = new CustomerDetailsRepository();
            logging = new Logging();
            string WhatssAppStatusEnum = string.Empty;
            string ResponseCode = string.Empty;
            _modelNumberrepository = new ModelNumberRepository();
            tblCustomerDetail customerDetail = null;
            string responseforWhatasapp = string.Empty;
            _whatsAppMessageRepository = new WhatsappMessageRepository();
            WhatasappResponse whatssappresponseDC = null;
            int result = 0;
            string voucherStatusName = "Generated";
            string SMSFlag = null;
            _brandRepository = new BrandRepository();
            _productTypeRepository = new ProductTypeRepository();
            _productCategoryRepository = new ProductCategoryRepository();
            try
            {
                SMSFlag = ConfigurationManager.AppSettings["sendsmsflag"].ToString();
                if (voucherData != null && voucherData.ExchangeOrderDataContract != null)
                {
                    tblExchangeOrder exchangeOrder = _exchangeOrderRepository.GetSingle(x => x.Id == voucherData.ExchangeOrderDataContract.Id);
                    if (exchangeOrder != null && exchangeOrder.CustomerDetailsId > 0)
                        customerDetail = _customerDetailsRepository.GetSingle(x => x.Id == exchangeOrder.CustomerDetailsId);
                    #region Code to update Exchange Price for Voucher
                    if (!string.IsNullOrEmpty(voucherData.ExchangeOrderDataContract.ExchangePriceString))
                    {
                        if (voucherData.ExchangeOrderDataContract.QualityCheck == 1)
                        {
                            exchangeOrder.ProductCondition = "Excellent";
                        }
                        if (voucherData.ExchangeOrderDataContract.QualityCheck == 2)
                        {
                            exchangeOrder.ProductCondition = "Good";
                        }
                        if (voucherData.ExchangeOrderDataContract.QualityCheck == 3)
                        {
                            exchangeOrder.ProductCondition = "Average";
                        }
                        if (voucherData.ExchangeOrderDataContract.QualityCheck == 4)
                        {
                            exchangeOrder.ProductCondition = "Not Working";
                        }
                        if (voucherData.BusinessUnitId == 1)
                        {
                            exchangeOrder.ModelNumberId = voucherData.ExchangeOrderDataContract.ModelNumberId;
                            exchangeOrder.NewProductCategoryId = voucherData.ExchangeOrderDataContract.NewProductCategoryId;
                            exchangeOrder.NewProductTypeId = voucherData.ExchangeOrderDataContract.NewProductCategoryTypeId;
                            tblBusinessUnit bussinessunitObj = _businessUnitRepository.GetSingle(x => x.BusinessUnitId == voucherData.BusinessUnitId && x.IsActive == true);
                            if (bussinessunitObj != null)
                            {
                                if (bussinessunitObj.IsSweetnerModelBased == true)
                                {
                                    if (voucherData.ExchangeOrderDataContract.VoucherType == Convert.ToInt32(VoucherTypeEnum.Cash))
                                    {
                                        tblModelNumber modelObj = _modelNumberrepository.GetSingle(x => x.ProductCategoryId == voucherData.ExchangeOrderDataContract.NewProductCategoryId && x.ProductTypeId == voucherData.ExchangeOrderDataContract.NewProductCategoryTypeId && x.IsActive == true && x.ModelNumberId == voucherData.ExchangeOrderDataContract.ModelNumberId && x.IsDefaultProduct == false);
                                        if (modelObj != null && modelObj.SweetnerForDTC != null)
                                        {
                                            exchangeOrder.Sweetener = modelObj.SweetnerForDTC;
                                        }
                                        else
                                        {
                                            exchangeOrder.Sweetener = 0;
                                        }
                                    }
                                    else
                                    {
                                        tblModelNumber modelObj = _modelNumberrepository.GetSingle(x => x.ProductCategoryId == voucherData.ExchangeOrderDataContract.NewProductCategoryId && x.ProductTypeId == voucherData.ExchangeOrderDataContract.NewProductCategoryTypeId && x.IsActive == true && x.IsDefaultProduct == true);
                                        if (modelObj != null && modelObj.SweetnerForDTC != null)
                                        {
                                            exchangeOrder.Sweetener = modelObj.SweetnerForDTC;
                                        }
                                        else
                                        {
                                            exchangeOrder.Sweetener = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    exchangeOrder.Sweetener = bussinessunitObj.SweetnerForDTC;
                                }
                            }
                        }
                        exchangeOrder.InvoiceImageName = voucherData.InvoiceImageName;
                        exchangeOrder.InvoiceNumber = voucherData.InvoiceNumberv;
                        exchangeOrder.ExchangePrice = Convert.ToDecimal(voucherData.ExchangeOrderDataContract.ExchangePriceString);
                        _exchangeOrderRepository.Update(exchangeOrder);
                        _exchangeOrderRepository.SaveChanges();
                    }

                    #endregion

                    tblBusinessUnit businessUnit = _businessUnitRepository.GetSingle(x => x.BusinessUnitId == voucherData.ExchangeOrderDataContract.BusinessUnitId);
                    if (voucherData != null)
                    {
                        tblVoucherStatu voucherStatu = _voucherStatusRepository.GetSingle(x => x.VoucherStatusName == voucherStatusName);
                        exchangeOrder.VoucherCodeExpDate = DateTime.Now.AddHours(Convert.ToDouble(businessUnit.VoucherExpiryTime));
                        exchangeOrder.VoucherCode = GenerateVoucher();
                        exchangeOrder.VoucherStatusId = voucherStatu.VoucherStatusId;
                        exchangeOrder.IsVoucherused = false;
                        exchangeOrder.IsActive = true;
                        _exchangeOrderRepository.Update(exchangeOrder);
                        _exchangeOrderRepository.SaveChanges();

                        //Code send SMS to cusrtomer about the Voucher code and expire time
                        result = exchangeOrder.Id;
                        if (voucherData.ExchangeOrderDataContract.IsDifferedSettlement == true && voucherData.ExchangeOrderDataContract.IsVoucher == true && voucherData.ExchangeOrderDataContract.VoucherType == Convert.ToInt32(VoucherTypeEnum.Cash))
                        {
                            if (exchangeOrder.VoucherCode != null)
                            {
                                #region code to send whatsappNotification For Voucher Generation for csh voucher
                                WhatsappTemplatecashvoucher whatsappObj = new WhatsappTemplatecashvoucher();
                                whatsappObj.userDetails = new UserDetails();
                                whatsappObj.notification = new NotificationForCash();
                                whatsappObj.notification.@params = new SendCashVoucherOnWhatssapp();
                                whatsappObj.userDetails.number = customerDetail.PhoneNumber;
                                whatsappObj.notification.sender = ConfigurationManager.AppSettings["Yello.aiSenderNumber"].ToString();
                                whatsappObj.notification.type = ConfigurationManager.AppSettings["Yellow.aiMesssaheType"].ToString();
                                whatsappObj.notification.templateId = NotificationConstants.cash_voucher;
                                whatsappObj.notification.@params.voucherAmount = exchangeOrder.ExchangePrice.ToString();
                                whatsappObj.notification.@params.VoucherExpiry = 7.ToString();
                                whatsappObj.notification.@params.voucherCode = exchangeOrder.VoucherCode.ToString();
                                whatsappObj.notification.@params.BrandName = businessUnit.Name.ToString();
                                whatsappObj.notification.@params.VoucherLink = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Home/VC/" + exchangeOrder.Id;
                                string url = ConfigurationManager.AppSettings["Yellow.AiUrl"].ToString();
                                IRestResponse response = WhatsappNotificationManager.Rest_InvokeWhatsappserviceCall(url, Method.POST, whatsappObj);
                                ResponseCode = response.StatusCode.ToString();
                                WhatssAppStatusEnum = ExchangeOrderManager.GetEnumDescription(WhatssAppEnum.SuccessCode);
                                if (ResponseCode == WhatssAppStatusEnum)
                                {
                                    responseforWhatasapp = response.Content;
                                    if (responseforWhatasapp != null)
                                    {
                                        whatssappresponseDC = JsonConvert.DeserializeObject<WhatasappResponse>(responseforWhatasapp);
                                        tblWhatsAppMessage whatsapObj = new tblWhatsAppMessage();
                                        whatsapObj.TemplateName = NotificationConstants.Test_Template;
                                        whatsapObj.IsActive = true;
                                        whatsapObj.PhoneNumber = customerDetail.PhoneNumber;
                                        whatsapObj.SendDate = DateTime.Now;
                                        whatsapObj.msgId = whatssappresponseDC.msgId;
                                        _whatsAppMessageRepository.Add(whatsapObj);
                                        _whatsAppMessageRepository.SaveChanges();
                                    }
                                    else
                                    {
                                        string ExchOrderObj = JsonConvert.SerializeObject(voucherData);
                                        logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", voucherData.ExchangeOrderDataContract.SponsorOrderNumber, ExchOrderObj);
                                    }
                                }
                                else
                                {
                                    string ExchOrderObj = JsonConvert.SerializeObject(voucherData);
                                    logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", voucherData.ExchangeOrderDataContract.SponsorOrderNumber, ExchOrderObj);
                                }
                                #endregion
                                #region Code to send notification to customer for voucher generation
                                string voucherUrl = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Home/VC/" + exchangeOrder.Id;
                                MailJetViewModel mailJet = new MailJetViewModel();
                                MailJetMessage jetmessage = new MailJetMessage();
                                MailJetFrom from = new MailJetFrom();
                                MailjetTo to = new MailjetTo();
                                jetmessage.From = new MailJetFrom() { Email = "customercare@utcdigital.com", Name = "UTC - Customer  Care" };
                                jetmessage.To = new List<MailjetTo>();
                                jetmessage.To.Add(new MailjetTo() { Email = customerDetail.Email.Trim(), Name = customerDetail.FirstName });
                                jetmessage.Subject = businessUnit.Name + ": Exchange Voucher Detail";
                                string TemplaTePath = ConfigurationManager.AppSettings["VoucherGenerationCash"].ToString();
                                string FilePath = TemplaTePath;
                                StreamReader str = new StreamReader(FilePath);
                                string MailText = str.ReadToEnd();
                                str.Close();
                                MailText = MailText.Replace("[ExchPrice]", exchangeOrder.ExchangePrice.ToString()).Replace("[VCode]", exchangeOrder.VoucherCode).Replace("[FirstName]", customerDetail.FirstName)
                                    .Replace("[VLink]", voucherUrl).Replace("[STORENAME]", businessUnit.Name).Replace("[VALIDTILLDATE]", "7 days from quality check of your appliance");
                                jetmessage.HTMLPart = MailText;
                                mailJet.Messages = new List<MailJetMessage>();
                                mailJet.Messages.Add(jetmessage);
                                BillCloudServiceCall.MailJetSendMailService(mailJet);
                                #endregion
                            }
                            //send sms
                            //WhatsappTemplateIntegration for OrderConfirmation
                            tblProductType productType = _productTypeRepository.GetSingle(x => x.Id == exchangeOrder.ProductTypeId && x.IsActive == true);
                            if (productType != null)
                            {
                                tblProductCategory productCategory = _productCategoryRepository.GetSingle(x => x.Id == productType.ProductCatId && x.IsActive == true);
                                if (productCategory != null)
                                {
                                    tblBrand brand = _brandRepository.GetSingle(x => x.Id == exchangeOrder.BrandId && x.IsActive == true);
                                    if (brand != null)
                                    {
                                        string ERPBaseURL = ConfigurationManager.AppSettings["ERPBaseURL"].ToString();
                                        string selfQC = ConfigurationManager.AppSettings["SelfQCUrl"].ToString();
                                        string selfqcurl = ERPBaseURL + "" + selfQC + "" + exchangeOrder.RegdNo;
                                        #region TO send WhatsappNotificatio for Deffreed Settelment
                                        OrderConfirmationTemplateExchange whatsappObjforOrderConfirmation = new OrderConfirmationTemplateExchange();
                                        whatsappObjforOrderConfirmation.userDetails = new UserDetails();
                                        whatsappObjforOrderConfirmation.notification = new OrderConfiirmationNotification();
                                        whatsappObjforOrderConfirmation.notification.@params = new SendWhatssappForExcahangeConfirmation();
                                        whatsappObjforOrderConfirmation.userDetails.number = customerDetail.PhoneNumber;
                                        whatsappObjforOrderConfirmation.notification.sender = ConfigurationManager.AppSettings["Yello.aiSenderNumber"].ToString();
                                        whatsappObjforOrderConfirmation.notification.type = ConfigurationManager.AppSettings["Yellow.aiMesssaheType"].ToString();
                                        whatsappObjforOrderConfirmation.notification.templateId = NotificationConstants.orderConfirmationForExchange;
                                        whatsappObjforOrderConfirmation.notification.@params.CustName = customerDetail.FirstName + " " + customerDetail.LastName;
                                        whatsappObjforOrderConfirmation.notification.@params.Link = selfqcurl;
                                        whatsappObjforOrderConfirmation.notification.@params.ProductBrand = brand.Name;
                                        whatsappObjforOrderConfirmation.notification.@params.ProdCategory = productCategory.Description;
                                        whatsappObjforOrderConfirmation.notification.@params.ProdType = productType.Description;
                                        whatsappObjforOrderConfirmation.notification.@params.RegdNO = exchangeOrder.RegdNo.ToString();
                                        string urlforwhatsapp = ConfigurationManager.AppSettings["Yellow.AiUrl"].ToString();
                                        IRestResponse responseConfirmation = WhatsappNotificationManager.Rest_InvokeWhatsappserviceCall(urlforwhatsapp, Method.POST, whatsappObjforOrderConfirmation);
                                        ResponseCode = responseConfirmation.StatusCode.ToString();
                                        WhatssAppStatusEnum = ExchangeOrderManager.GetEnumDescription(WhatssAppEnum.SuccessCode);
                                        if (ResponseCode == WhatssAppStatusEnum)
                                        {
                                            responseforWhatasapp = responseConfirmation.Content;
                                            if (responseforWhatasapp != null)
                                            {
                                                whatssappresponseDC = JsonConvert.DeserializeObject<WhatasappResponse>(responseforWhatasapp);
                                                tblWhatsAppMessage whatsapObj = new tblWhatsAppMessage();
                                                whatsapObj.TemplateName = NotificationConstants.orderConfirmationForExchange;
                                                whatsapObj.IsActive = true;
                                                whatsapObj.PhoneNumber = customerDetail.PhoneNumber;
                                                whatsapObj.SendDate = DateTime.Now;
                                                whatsapObj.msgId = whatssappresponseDC.msgId;
                                                _whatsAppMessageRepository.Add(whatsapObj);
                                                _whatsAppMessageRepository.SaveChanges();
                                            }
                                            else
                                            {
                                                string JsonObjectForExchangeOrder = JsonConvert.SerializeObject(voucherData);
                                                logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", exchangeOrder.SponsorOrderNumber, JsonObjectForExchangeOrder);
                                            }
                                        }
                                        else
                                        {
                                            string JsonObjectForExchangeOrder = JsonConvert.SerializeObject(voucherData);
                                            logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", exchangeOrder.SponsorOrderNumber, JsonObjectForExchangeOrder);
                                        }
                                    }
                                }
                            }


                            #endregion
                        }
                        else
                        {
                            #region Code to send notification to customer for voucher generation
                            _notificationManager = new NotificationManager();
                            string voucherUrl = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Home/V/" + exchangeOrder.Id;
                            if (SMSFlag == "true")
                            {
                                string message = NotificationConstants.SMS_VoucherRedemption_Confirmation.Replace("[ExchPrice]", Convert.ToInt32(exchangeOrder.ExchangePrice).ToString()).Replace("[VCODE]", exchangeOrder.VoucherCode)
                               .Replace("[VLink]", "( " + voucherUrl + " )").Replace("[STORENAME]", businessUnit.Name).Replace("[COMPANY]", businessUnit.Name).Replace("[VALIDTILLDATE]", Convert.ToDateTime(exchangeOrder.VoucherCodeExpDate).ToString("dd/MM/yyyy"));
                                _notificationManager.SendNotificationSMS(voucherData.ExchangeOrderDataContract.PhoneNumber, message, null);
                            }
                            MailJetViewModel mailJet = new MailJetViewModel();
                            MailJetMessage jetmessage = new MailJetMessage();
                            MailJetFrom from = new MailJetFrom();
                            MailjetTo to = new MailjetTo();
                            jetmessage.From = new MailJetFrom() { Email = "customercare@utcdigital.com", Name = "UTC - Customer  Care" };
                            jetmessage.To = new List<MailjetTo>();
                            jetmessage.To.Add(new MailjetTo() { Email = customerDetail.Email.Trim(), Name = voucherData.ExchangeOrderDataContract.FirstName });
                            jetmessage.Subject = businessUnit.Name + ": Exchange Voucher Detail";
                            string TemplaTePath = ConfigurationManager.AppSettings["VoucherGenerationInstant"].ToString();
                            string FilePath = TemplaTePath;
                            StreamReader str = new StreamReader(FilePath);
                            string MailText = str.ReadToEnd();
                            str.Close();
                            MailText = MailText.Replace("[ExchPrice]", exchangeOrder.ExchangePrice.ToString()).Replace("[VCode]", exchangeOrder.VoucherCode).Replace("[FirstName]", voucherData.ExchangeOrderDataContract.FirstName)
                                .Replace("[VLink]", voucherUrl).Replace("[STORENAME]", businessUnit.Name).Replace("[VALIDTILLDATE]", Convert.ToDateTime(exchangeOrder.VoucherCodeExpDate).ToString("dd/MM/yyyy"));
                            jetmessage.HTMLPart = MailText;
                            mailJet.Messages = new List<MailJetMessage>();
                            mailJet.Messages.Add(jetmessage);
                            BillCloudServiceCall.MailJetSendMailService(mailJet);
                            #endregion

                            //send sms
                            #region code to send whatsappNotification For Voucher Generation
                            WhatsappTemplate whatsappObj = new WhatsappTemplate();
                            whatsappObj.userDetails = new UserDetails();
                            whatsappObj.notification = new Notification();
                            whatsappObj.notification.@params = new SendVoucherOnWhatssapp();
                            whatsappObj.userDetails.number = customerDetail.PhoneNumber;
                            whatsappObj.notification.sender = ConfigurationManager.AppSettings["Yello.aiSenderNumber"].ToString();
                            whatsappObj.notification.type = ConfigurationManager.AppSettings["Yellow.aiMesssaheType"].ToString();
                            whatsappObj.notification.templateId = NotificationConstants.Send_Voucher_Code_Template;
                            whatsappObj.notification.@params.voucherAmount = exchangeOrder.ExchangePrice.ToString();
                            whatsappObj.notification.@params.VoucherExpiry = Convert.ToDateTime(exchangeOrder.VoucherCodeExpDate).ToString("dd/MM/yyyy");
                            whatsappObj.notification.@params.voucherCode = exchangeOrder.VoucherCode.ToString();
                            whatsappObj.notification.@params.BrandName = businessUnit.Name.ToString();
                            whatsappObj.notification.@params.BrandName2 = businessUnit.Name.ToString();
                            whatsappObj.notification.@params.VoucherLink = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Home/V/" + exchangeOrder.Id;
                            string url = ConfigurationManager.AppSettings["Yellow.AiUrl"].ToString();
                            IRestResponse response = WhatsappNotificationManager.Rest_InvokeWhatsappserviceCall(url, Method.POST, whatsappObj);
                            ResponseCode = response.StatusCode.ToString();
                            WhatssAppStatusEnum = ExchangeOrderManager.GetEnumDescription(WhatssAppEnum.SuccessCode);
                            if (ResponseCode == WhatssAppStatusEnum)
                            {
                                responseforWhatasapp = response.Content;
                                if (responseforWhatasapp != null)
                                {
                                    whatssappresponseDC = JsonConvert.DeserializeObject<WhatasappResponse>(responseforWhatasapp);
                                    tblWhatsAppMessage whatsapObj = new tblWhatsAppMessage();
                                    whatsapObj.TemplateName = NotificationConstants.Send_Voucher_Code_Template;
                                    whatsapObj.IsActive = true;
                                    whatsapObj.PhoneNumber = customerDetail.PhoneNumber;
                                    whatsapObj.SendDate = DateTime.Now;
                                    whatsapObj.msgId = whatssappresponseDC.msgId;
                                    _whatsAppMessageRepository.Add(whatsapObj);
                                    _whatsAppMessageRepository.SaveChanges();
                                }
                                else
                                {
                                    string ExchOrderObj = JsonConvert.SerializeObject(voucherData);
                                    logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", voucherData.ExchangeOrderDataContract.SponsorOrderNumber, ExchOrderObj);
                                }
                            }
                            else
                            {
                                string ExchOrderObj = JsonConvert.SerializeObject(voucherData);
                                logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", voucherData.ExchangeOrderDataContract.SponsorOrderNumber, ExchOrderObj);
                            }
                            #endregion
                            //WhatsappTemplateIntegration for OrderConfirmation
                            tblProductType productType = _productTypeRepository.GetSingle(x => x.Id == exchangeOrder.ProductTypeId && x.IsActive == true);
                            if (productType != null)
                            {
                                tblProductCategory productCategory = _productCategoryRepository.GetSingle(x => x.Id == productType.ProductCatId && x.IsActive == true);
                                if (productCategory != null)
                                {
                                    tblBrand brand = _brandRepository.GetSingle(x => x.Id == exchangeOrder.BrandId && x.IsActive == true);
                                    if (brand != null)
                                    {
                                        string ERPBaseURL = ConfigurationManager.AppSettings["ERPBaseURL"].ToString();
                                        string selfQC = ConfigurationManager.AppSettings["SelfQCUrl"].ToString();
                                        string selfqcurl = ERPBaseURL + "" + selfQC + "" + exchangeOrder.RegdNo;
                                        #region TO send WhatsappNotificatio for Deffreed Settelment
                                        OrderConfirmationTemplateExchange whatsappObjforOrderConfirmation = new OrderConfirmationTemplateExchange();
                                        whatsappObjforOrderConfirmation.userDetails = new UserDetails();
                                        whatsappObjforOrderConfirmation.notification = new OrderConfiirmationNotification();
                                        whatsappObjforOrderConfirmation.notification.@params = new SendWhatssappForExcahangeConfirmation();
                                        whatsappObjforOrderConfirmation.userDetails.number = customerDetail.PhoneNumber;
                                        whatsappObjforOrderConfirmation.notification.sender = ConfigurationManager.AppSettings["Yello.aiSenderNumber"].ToString();
                                        whatsappObjforOrderConfirmation.notification.type = ConfigurationManager.AppSettings["Yellow.aiMesssaheType"].ToString();
                                        whatsappObjforOrderConfirmation.notification.templateId = NotificationConstants.orderConfirmationForExchange;
                                        whatsappObjforOrderConfirmation.notification.@params.CustName = customerDetail.FirstName + " " + customerDetail.LastName;
                                        whatsappObjforOrderConfirmation.notification.@params.Link = selfqcurl;
                                        whatsappObjforOrderConfirmation.notification.@params.ProductBrand = brand.Name;
                                        whatsappObjforOrderConfirmation.notification.@params.ProdCategory = productCategory.Description;
                                        whatsappObjforOrderConfirmation.notification.@params.ProdType = productType.Description;
                                        whatsappObjforOrderConfirmation.notification.@params.RegdNO = exchangeOrder.RegdNo.ToString();
                                        string urlforwhatsapp = ConfigurationManager.AppSettings["Yellow.AiUrl"].ToString();
                                        IRestResponse responseConfirmation = WhatsappNotificationManager.Rest_InvokeWhatsappserviceCall(urlforwhatsapp, Method.POST, whatsappObjforOrderConfirmation);
                                        ResponseCode = responseConfirmation.StatusCode.ToString();
                                        WhatssAppStatusEnum = ExchangeOrderManager.GetEnumDescription(WhatssAppEnum.SuccessCode);
                                        if (ResponseCode == WhatssAppStatusEnum)
                                        {
                                            responseforWhatasapp = responseConfirmation.Content;
                                            if (responseforWhatasapp != null)
                                            {
                                                whatssappresponseDC = JsonConvert.DeserializeObject<WhatasappResponse>(responseforWhatasapp);
                                                tblWhatsAppMessage whatsapObj = new tblWhatsAppMessage();
                                                whatsapObj.TemplateName = NotificationConstants.orderConfirmationForExchange;
                                                whatsapObj.IsActive = true;
                                                whatsapObj.PhoneNumber = customerDetail.PhoneNumber;
                                                whatsapObj.SendDate = DateTime.Now;
                                                whatsapObj.msgId = whatssappresponseDC.msgId;
                                                _whatsAppMessageRepository.Add(whatsapObj);
                                                _whatsAppMessageRepository.SaveChanges();
                                            }
                                            else
                                            {
                                                string JsonObjectForExchangeOrder = JsonConvert.SerializeObject(voucherData);
                                                logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", exchangeOrder.SponsorOrderNumber, JsonObjectForExchangeOrder);
                                            }
                                        }
                                        else
                                        {
                                            string JsonObjectForExchangeOrder = JsonConvert.SerializeObject(voucherData);
                                            logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", exchangeOrder.SponsorOrderNumber, JsonObjectForExchangeOrder);
                                        }
                                    }
                                }
                            }


                            #endregion
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeOrderManager", "AddVouchertoDB", ex);
            }

            return result;
        }

        public static string DealerEmailBody()
        {
            string htmlString = @"<p>Hi [FirstName],</p>
                            <p>&nbsp;</p>
                            <p>Greeting for the day!!!</p>
                            <p>&nbsp;</p>
                            <p>Please refer following access details and steps to access the Voucher Redemption:</p>
                            <p>&nbsp;</p>
                            <p><b>Access Detail:</b></p>
                            <p>&nbsp;</p>
                            <ul>
	                            <li>URL: [VoucherURL]</li>
	                            <li>User Name: [UserName]</li>
	                            <li>Password: [Password]</li>
                            </ul>
                            <p>&nbsp;</p>
                            <p><b>Steps to Access Redeem Voucher Page:</b></p>
                            <p>&nbsp;</p>
                            <ul>
	                            <li>Click on the above URL OR copy and paste the URL into your browser</li>
	                            <li>Enter user name, password and click on Login, you will get redirected to the Voucher redemption screen.</li>
                            </ul>
                            <p>&nbsp;</p>
                            <p><b><i>Note: Please save the above URL, username and password in your notes to access the voucher redemption page quickly.</i></b></p>
                            <p>&nbsp;</p>
                            <p>In case of any query, please drop mail at&nbsp;<a href='mailto:exchange@digimart.co.in' target='_blank'>exchange@digimart.co.in</a>&nbsp;</p>
                            <p>&nbsp;</p>
                            <p>Thank you.</p>";
            return htmlString;
        }



        #endregion

        #region Method to get the exchange order id
        /// <summary>
        /// Method to get the exchange order id
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public ExchangeOrderDataContract GetExchangeOrderDCByVoucherCode(string vcode, string custphone)
        {
            ExchangeOrderDataContract exchangeOrderDC = null;
            _exchangeOrderRepository = new ExchangeOrderRepository();
            _productTypeRepository = new ProductTypeRepository();
            _customerDetailsRepository = new CustomerDetailsRepository();
            _productCategoryRepository = new ProductCategoryRepository();
            try
            {
                tblExchangeOrder exchangeObj = null;
                DataTable dt = _exchangeOrderRepository.GetExchangeOrderDCByVoucherCode(vcode, custphone);
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<tblExchangeOrder> exchangeOrders = GenericConversionHelper.DataTableToList<tblExchangeOrder>(dt);
                    exchangeObj = exchangeOrders != null && exchangeOrders.Count > 0 ? exchangeOrders[0] : null;
                }

                if (exchangeObj != null)
                {
                    exchangeOrderDC = GenericMapper<tblExchangeOrder, ExchangeOrderDataContract>.MapObject(exchangeObj);
                    tblCustomerDetail custObj = _customerDetailsRepository.GetSingle(x => x.Id == exchangeObj.CustomerDetailsId);
                    if (exchangeOrderDC != null && custObj != null)
                    {
                        exchangeOrderDC.CustomerDetailsId = custObj.Id;
                        exchangeOrderDC.FirstName = custObj.FirstName;
                        exchangeOrderDC.LastName = custObj.LastName;
                        exchangeOrderDC.ZipCode = custObj.ZipCode;
                        exchangeOrderDC.Address1 = custObj.Address1;
                        exchangeOrderDC.Address2 = custObj.Address2;
                        exchangeOrderDC.City = custObj.City;
                        exchangeOrderDC.Email = custObj.Email;
                        exchangeOrderDC.PhoneNumber = custObj.PhoneNumber;


                        // new Order details
                        if (exchangeObj.NewProductCategoryId != null)
                        {
                            exchangeOrderDC.NewProductCategoryTypeId = (int)exchangeObj.NewProductTypeId;
                            if (exchangeObj.ModelNumberId != null)
                            {
                                exchangeOrderDC.ModelNumberId = (int)exchangeObj.ModelNumberId;
                            }
                        }


                    }
                    if (exchangeObj.ProductCondition != null)
                    {
                        if (exchangeObj.ProductCondition == "Excellent")
                        {
                            exchangeOrderDC.QualityCheckValue = 1;
                        }
                        else if (exchangeObj.ProductCondition == "Good")
                        {
                            exchangeOrderDC.QualityCheckValue = 2;
                        }
                        else if (exchangeObj.ProductCondition == "Average")
                        {
                            exchangeOrderDC.QualityCheckValue = 3;
                        }
                        else if (exchangeObj.ProductCondition == "Not Working")
                        {
                            exchangeOrderDC.QualityCheckValue = 4;
                        }

                    }
                    //Code to fill prod cat
                    tblProductType prodTypeObj = _productTypeRepository.GetSingle(x => x.Id == exchangeObj.ProductTypeId);

                    tblProductCategory prodCatObj = prodTypeObj != null && prodTypeObj.ProductCatId > 0 ? _productCategoryRepository.GetSingle(x => x.Id == prodTypeObj.ProductCatId) : null;
                    exchangeOrderDC.ProductCategoryId = prodCatObj.Id;
                    exchangeOrderDC.Response = "Success";
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeOrderManager", "GetExchangeOrderDCById", ex);
            }
            return exchangeOrderDC;
        }
        #endregion

        #region Method to get the Exchange order object by r number 
        /// <summary>
        /// Method to get the Exchange order object by r number 
        /// </summary>
        /// <param name="rnumber"></param>
        /// <returns>ExchangeOrderDataContract</returns>
        public ExchangeOrderDataContract GetExchangeOrderDCByRnumber(string rnumber)
        {
            ExchangeOrderDataContract exchangeOrderDC = null;
            _exchangeOrderRepository = new ExchangeOrderRepository();
            _productTypeRepository = new ProductTypeRepository();
            _customerDetailsRepository = new CustomerDetailsRepository();
            _productCategoryRepository = new ProductCategoryRepository();
            _businessPartnerRepository = new BusinessPartnerRepository();
            try
            {
                tblExchangeOrder exchangeObj = _exchangeOrderRepository.GetSingle(x => x.VoucherCode == null && (x.RegdNo != null && x.RegdNo.Equals(rnumber)));
                exchangeOrderDC = GenericMapper<tblExchangeOrder, ExchangeOrderDataContract>.MapObject(exchangeObj);
                if (exchangeObj != null)
                {
                    tblBusinessPartner businessPartner = _businessPartnerRepository.GetSingle(x => x.BusinessPartnerId == exchangeObj.BusinessPartnerId);
                    if (businessPartner != null)
                    {
                        exchangeOrderDC.ExchangePriceString = exchangeObj.ExchangePrice.ToString();
                        exchangeOrderDC.IsVoucher = Convert.ToBoolean(businessPartner.IsVoucher);
                        exchangeOrderDC.VoucherType = Convert.ToInt32(businessPartner.VoucherType);
                        exchangeOrderDC.voucherCash = Convert.ToInt32(VoucherTypeEnum.Cash);
                        exchangeOrderDC.voucherDiscount = Convert.ToInt32(VoucherTypeEnum.Discount);
                        exchangeOrderDC.IsDifferedSettlement = Convert.ToBoolean(businessPartner.IsDefferedSettlement);
                        tblCustomerDetail custObj = _customerDetailsRepository.GetSingle(x => x.Id == exchangeObj.CustomerDetailsId);
                        if (exchangeOrderDC != null && custObj != null)
                        {
                            exchangeOrderDC.CustomerDetailsId = custObj.Id;
                            exchangeOrderDC.FirstName = custObj.FirstName;
                            exchangeOrderDC.LastName = custObj.LastName;
                            exchangeOrderDC.ZipCode = custObj.ZipCode;
                            exchangeOrderDC.Address1 = custObj.Address1;
                            exchangeOrderDC.Address2 = custObj.Address2;
                            exchangeOrderDC.City = custObj.City;
                            exchangeOrderDC.Email = custObj.Email;
                            exchangeOrderDC.PhoneNumber = custObj.PhoneNumber;

                            //new product details 
                            exchangeOrderDC.NewProductCategoryTypeId = exchangeObj.NewProductTypeId != null && exchangeObj.NewProductTypeId > 0 ? (int)exchangeObj.NewProductTypeId : 0;
                            if (exchangeObj.ProductCondition == "Excellent")
                            {
                                exchangeOrderDC.QualityCheck = 1;
                            }
                            if (exchangeObj.ProductCondition == "Good")
                            {
                                exchangeOrderDC.QualityCheck = 2;
                            }
                            if (exchangeObj.ProductCondition == "Average")
                            {
                                exchangeOrderDC.QualityCheck = 3;
                            }
                            if (exchangeObj.ProductCondition == "Not Working")
                            {
                                exchangeOrderDC.QualityCheck = 4;
                            }
                        }
                    }




                    //Code to fill prod cat
                    tblProductType prodTypeObj = _productTypeRepository.GetSingle(x => x.Id == exchangeObj.ProductTypeId);

                    tblProductCategory prodCatObj = prodTypeObj != null && prodTypeObj.ProductCatId > 0 ? _productCategoryRepository.GetSingle(x => x.Id == prodTypeObj.ProductCatId) : null;
                    exchangeOrderDC.ProductCategoryId = prodCatObj.Id;
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeOrderManager", "GetExchangeOrderDCByRnumber", ex);
            }
            return exchangeOrderDC;
        }
        #endregion

        #region Genereate Voucher
        /// <summary>
        /// Method to generate the voucher code
        /// </summary>
        /// <param name="buCode">business unit code</param>
        /// <returns>string</returns>
        public string GenerateVoucher()
        {
            string code = null;
            _businessUnitRepository = new BusinessUnitRepository();
            try
            {
                code = "V" + UniqueString.RandomNumberByLength(8);
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeOrderManager", "GenerateVoucher", ex);
            }

            return code;
        }
        #endregion

        #region Get Exchange Order Detail From DB

        /// <summary>
        /// Method to get the exchange order id
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public ExchangeOrderDataContract GetExchangeOrderDCById(int orderid)
        {
            ExchangeOrderDataContract exchangeOrderDC = null;
            _exchangeOrderRepository = new ExchangeOrderRepository();
            _businessUnitRepository = new BusinessUnitRepository();
            try
            {
                tblExchangeOrder exchangeObj = _exchangeOrderRepository.GetSingle(x => x.Id == orderid);
                tblBusinessUnit businessUnit = _businessUnitRepository.GetSingle(x => x.Name.ToLower().Equals(exchangeObj.CompanyName.Trim().ToLower()));
                exchangeOrderDC = GenericMapper<tblExchangeOrder, ExchangeOrderDataContract>.MapObject(exchangeObj);
                if (exchangeOrderDC != null && businessUnit != null)
                {
                    exchangeOrderDC.BusinessUnitDataContract = GetBUById(businessUnit.BusinessUnitId);
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeOrderManager", "GetExchangeOrderDCById", ex);
            }
            return exchangeOrderDC;
        }

        #endregion

        #region Push Delivery Confirmation Message

        /// <summary>
        /// Push Delivery Confirmation Message
        /// </summary>
        /// <param name="buid">buid</param>
        /// <param name="companyName">companyName</param>
        /// <returns>companyName</returns>
        public bool PushMessgeForConfirmDelivery(int buid)
        {
            _exchangeOrderRepository = new ExchangeOrderRepository();
            _customerDetailsRepository = new CustomerDetailsRepository();
            _businessUnitRepository = new BusinessUnitRepository();
            _sponserManager = new SponserManager();
            NotificationManager _notificationManager = new NotificationManager();
            string fromDate = string.Empty;
            string message = string.Empty;
            string link = string.Empty;
            bool flag = false;
            try
            {
                fromDate = DateTime.Now.AddDays(-5).ToString("dd-MM-yyy");
                tblBusinessUnit businessUnitDetail = _businessUnitRepository.GetSingle(x => x.BusinessUnitId == buid);
                DataTable dt = _exchangeOrderRepository.GetNotDeliveredOrderForBU(businessUnitDetail.Name, fromDate);
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<tblExchangeOrder> exchangeOrders = GenericConversionHelper.DataTableToList<tblExchangeOrder>(dt);
                    foreach (tblExchangeOrder exchangeOrder in exchangeOrders)
                    {
                        flag = true;
                        SponserListDataContract sponserListDC = _sponserManager.GetSponserOrderById(exchangeOrder.ZohoSponsorOrderId);
                        if (sponserListDC != null && sponserListDC.data != null && sponserListDC.data.Count > 0)
                        {
                            if (!sponserListDC.data[0].QC.ToLower().Equals("5x") && !sponserListDC.data[0].QC.ToLower().Equals("3y"))
                                flag = false;
                        }

                        if (flag)
                        {
                            //code to get the customer detail by id 
                            tblCustomerDetail custDetailObj = _customerDetailsRepository.GetSingle(x => x.Id == exchangeOrder.CustomerDetailsId);

                            link = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Exchange/ConfirmDeliveryBP?BUId=" + buid + "&orderid=" + exchangeOrder.Id;
                            message = NotificationConstants.SMS_Exchange_DeliveryConfirm.Replace("[Link]", link);
                            _notificationManager.SendNotificationSMS(custDetailObj.PhoneNumber, message);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeOrderManager", "PushMessgeForConfirmDelivery", ex);
            }
            return true;
        }
        #endregion
        
        #region Method to get the exchange order detail list by business partner id

        /// <summary>
        /// Method to get the exchange order detail list by business partner id
        /// </summary>
        /// <param name="businessPartnerId">businessPartnerId</param>
        /// <returns>List ExchangeOrderDataContract</returns>
        public List<ExchangeOrderDataContract> GetExchangeOrderDetailbyBPId(ExchangeOrderDataContract exchangeOrderDataDC)
        {
            List<ExchangeOrderDataContract> exchangeOrderListDC = null;
            _exchangeOrderRepository = new ExchangeOrderRepository();
            _voucherVerificationRepository = new VoucherVerificationRepository();
            try
            {
                _exchangeOrderRepository = new ExchangeOrderRepository();
                List<tblExchangeOrder> exchangeOrderObj = _exchangeOrderRepository.GetList(x => x.BusinessPartnerId == exchangeOrderDataDC.BusinessPartnerId && x.IsVoucherused == true
                && (x.CreatedDate != null && (Convert.ToDateTime(x.CreatedDate).Month == exchangeOrderDataDC.Month) && Convert.ToDateTime(x.CreatedDate).Year == exchangeOrderDataDC.Year)).ToList();
                exchangeOrderListDC = GenericMapper<tblExchangeOrder, ExchangeOrderDataContract>.MapList(exchangeOrderObj);
                if (exchangeOrderListDC != null && exchangeOrderListDC.Count > 0)
                {
                    foreach (ExchangeOrderDataContract exchangeOrderDC in exchangeOrderListDC)
                    {
                        tblVoucherVerfication tblVoucherVerfication = _voucherVerificationRepository.GetSingle(x => x.IsActive == true && x.ExchangeOrderId == exchangeOrderDC.Id);
                        exchangeOrderDC.PurchasedProductCategory = tblVoucherVerfication.tblProductCategory != null ? tblVoucherVerfication.tblProductCategory.Description : string.Empty;
                    }
                }
                #region Bill Cloud API call to get the ledger
                string billcloudcall = ConfigurationManager.AppSettings["BillcloudCallActive"].ToString();
                if (billcloudcall == "true")
                {
                    LedgerRequestModel ledgerRequestObj = new LedgerRequestModel();
                    LedgerResponseModel ledgerResponseObj = new LedgerResponseModel();
                    ledgerRequestObj.data = new LedgerData();
                    ledgerRequestObj.data.member_ref_id = exchangeOrderDataDC.AssociateCode;
                    ledgerRequestObj.data.month = exchangeOrderDataDC.Month;
                    ledgerRequestObj.data.year = exchangeOrderDataDC.Year;
                    ledgerRequestObj.data.dao_name = exchangeOrderDataDC.CompanyName;
                    ledgerRequestObj.data.account_type_name = GetEnumDescription((AcountTypeEnum.AccountTypeForLedger));
                    IRestResponse response = BillCloudServiceCall.Rest_InvokeZohoInvoiceServiceForPlainText(ConfigurationManager.AppSettings["GetLedger"].ToString(), Method.POST, ledgerRequestObj);
                    if (response != null)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            ledgerResponseObj = JsonConvert.DeserializeObject<LedgerResponseModel>(response.Content);
                            if (ledgerResponseObj != null && ledgerResponseObj.data.Count > 0)
                            {
                                if (exchangeOrderListDC != null && exchangeOrderListDC.Count > 0)
                                {
                                    foreach (ExchangeOrderDataContract exchangeOrderDC in exchangeOrderListDC)
                                    {
                                        Ledger ledger = ledgerResponseObj.data.FirstOrDefault(x => x.voucher != null && (!string.IsNullOrEmpty(exchangeOrderDC.VoucherCode) && x.voucher.voucher_id == exchangeOrderDC.VoucherCode));
                                        if (ledger != null && ledger.debit_amount > 0)
                                        {
                                            exchangeOrderDC.AmountStatus = "Paid";
                                        }
                                        else
                                        {
                                            exchangeOrderDC.AmountStatus = "Not Paid";
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
                else
                {
                    foreach (ExchangeOrderDataContract exchangeOrderDC in exchangeOrderListDC)
                    {
                        if (exchangeOrderDC.VoucherStatusId != null)
                        {
                            if (exchangeOrderDC.VoucherStatusId == Convert.ToInt32(VoucherStatus.Capture))
                            {
                                exchangeOrderDC.AmountStatus = "Not Paid";
                            }
                            if (exchangeOrderDC.VoucherStatusId == Convert.ToInt32(VoucherStatus.Reedem))
                            {
                                exchangeOrderDC.AmountStatus = "Paid";
                            }
                        }

                    }

                }

                #endregion
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeOrderManager", "GetExchangeOrderDetailbyBPId", ex);
            }
            return exchangeOrderListDC;
        }

        #endregion

        #region Method to get the sweetnerDTC by business partner id

        /// <summary>
        /// Method to get the sweetnerDTC by business partner id
        /// </summary>
        /// <param name="businessPartnerId">businessPartnerId</param>
        /// <returns>List ExchangeOrderDataContract</returns>
        public decimal GetSweetnerDTCAmtByBuid(ExchangeOrderDataContract exchangeOrderDataDC)
        {
            _businessPartnerRepository = new BusinessPartnerRepository();
            _businessUnitRepository = new BusinessUnitRepository();
            decimal sweetner = 0;
            try
            {
                tblBusinessPartner businessPartnerObj = _businessPartnerRepository.GetSingle(x => x.BusinessPartnerId == exchangeOrderDataDC.BusinessPartnerId);
                tblBusinessUnit businessUnitObj = _businessUnitRepository.GetSingle(x => x.BusinessUnitId == businessPartnerObj.BusinessUnitId);
                sweetner = (decimal)businessUnitObj.SweetnerForDTC;
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeOrderManager", "GetSweetnerDTDAmtByBuid", ex);
            }
            return sweetner;
        }

        #endregion

        #region Method to get the sweetnerDTD by business partner id

        /// <summary>
        /// Method to get the sweetnerDTC by business partner id
        /// </summary>
        /// <param name="businessPartnerId">businessPartnerId</param>
        /// <returns>List ExchangeOrderDataContract</returns>
        public decimal GetSweetnerDTDAmtByBuid(ExchangeOrderDataContract exchangeOrderDataDC)
        {
            _businessPartnerRepository = new BusinessPartnerRepository();
            _businessUnitRepository = new BusinessUnitRepository();
            decimal sweetner = 0;
            try
            {
                tblBusinessPartner businessPartnerObj = _businessPartnerRepository.GetSingle(x => x.BusinessPartnerId == exchangeOrderDataDC.BusinessPartnerId);
                tblBusinessUnit businessUnitObj = _businessUnitRepository.GetSingle(x => x.BusinessUnitId == businessPartnerObj.BusinessUnitId);
                if (businessUnitObj.IsSweetnerModelBased == true)
                {

                }
                else
                {
                    sweetner = (decimal)businessUnitObj.SweetnerForDTD;
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeOrderManager", "GetSweetnerDTDAmtByBuid", ex);
            }
            return sweetner;
        }
        #endregion

        #region Method to verify duplicate exchange orders

        /// <summary>
        /// Method to verify duplicate exchange orders
        /// </summary>
        /// <param name=""></param>
        /// <returns>Bool</returns>
        public bool ValidateExchangeProductExists(int? NewProductId, int OldProductTypeId, string customerEmail, string customerPhone)
        {
            if (NewProductId == 0)
            {
                NewProductId = null;
            }
            DataTable dt = new DataTable();
            try
            {
                DBHelper obj = new DBHelper();
                SqlParameter[] sqlParam =  {
                        new SqlParameter("@NewProductId",NewProductId),
                        new SqlParameter("@OldProductTypeId", OldProductTypeId),
                        new SqlParameter("@customerEmail", customerEmail),
                        new SqlParameter("@customerPhone", customerPhone)
                        };
                dt = obj.ExecuteDataTable("sp_ValidateExchangeProductExists", sqlParam);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return false;
                }
                else
                    return true;
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeOrderManager", "ValidateExchangeProductExists", ex);
            }
            return true;

        }
        #endregion

        #region manage exchange order
        /// <summary>
        /// manage exchange order
        /// </summary>       
        /// <returns></returns>   
        public bool MoveUTCbridgeToZoho(string companyName, int month)
        {
            bool flag = false;

            CustomerManager customerInfo = new CustomerManager();
            ProductManager productOrderInfo = new ProductManager();
            _customerDetailsRepository = new CustomerDetailsRepository();
            _notificationManager = new NotificationManager();
            _businessUnitRepository = new BusinessUnitRepository();
            SponserFormResponseDataContract sponserResponseDC = null;
            SponserManager sponserManager = new SponserManager();
            SponserDataContract sponserDC = null;
            ProductOrderResponseDataContract productOrderResponseDC = null;
            productOrderResponseDC = new ProductOrderResponseDataContract();
            _exchangeOrderRepository = new ExchangeOrderRepository();
            string sponsercode = string.Empty;
            int OrderId = 0;
            try
            {

                List<tblExchangeOrder> exchangeOrders = _exchangeOrderRepository.GetList(x => x.ZohoSponsorOrderId == null && (!string.IsNullOrEmpty(x.CompanyName)
                && x.CompanyName.Equals(companyName))
                  && (x.CreatedDate != null && (Convert.ToDateTime(x.CreatedDate).Month == month && Convert.ToDateTime(x.CreatedDate).Year == DateTime.Now.Year))
                //&& string.IsNullOrEmpty(x.RegdNo)
                ).ToList();

                List<tblBusinessUnit> businessUnits = _businessUnitRepository.GetList(x => x.IsActive == true && !string.IsNullOrEmpty(x.ZohoSponsorId)).ToList();

                string rNumber = string.Empty;
                foreach (tblExchangeOrder exchangeOrder in exchangeOrders)
                {
                    #region Code to add exchange order in zoho creator
                    if (exchangeOrder != null && exchangeOrder.Id != 0)
                    {
                        if (string.IsNullOrEmpty(exchangeOrder.RegdNo))
                            rNumber = "E" + UniqueString.RandomNumberByLength(5);

                        exchangeOrder.RegdNo = rNumber;
                        tblCustomerDetail customerDetail = exchangeOrder.tblCustomerDetail != null ? exchangeOrder.tblCustomerDetail : null;
                        tblBusinessPartner businessPartner = exchangeOrder.tblBusinessPartner != null ? exchangeOrder.tblBusinessPartner : null;
                        tblBusinessUnit businessUnit = businessUnits.FirstOrDefault(x => x.Name.ToLower().Equals(exchangeOrder.CompanyName.ToLower()));
                        OrderId = exchangeOrder.Id;
                        if (businessUnit != null && customerDetail != null)
                        {
                            sponserDC = SetExchangeOrderObjectToMoveFromUTCtoZoho(exchangeOrder, customerDetail, businessPartner, businessUnit);
                            if (sponserDC != null)
                                sponserResponseDC = sponserManager.AddSponser(sponserDC);

                            #region Code to Update Zoho Sponsor Id in database
                            if (sponserResponseDC != null)
                            {
                                if (sponserResponseDC.data != null)
                                {
                                    _exchangeOrderRepository = new ExchangeOrderRepository();
                                    if (sponserResponseDC.data.ID != null && exchangeOrder.Id != 0)
                                    {
                                        tblExchangeOrder tempexchangeOrderInfo = _exchangeOrderRepository.GetSingle(x => x.Id.Equals(exchangeOrder.Id));
                                        if (tempexchangeOrderInfo != null)
                                        {
                                            if (string.IsNullOrEmpty(tempexchangeOrderInfo.RegdNo))
                                                tempexchangeOrderInfo.RegdNo = rNumber;
                                            tempexchangeOrderInfo.ZohoSponsorOrderId = sponserResponseDC.data.ID;
                                            tempexchangeOrderInfo.ModifiedDate = currentDatetime;
                                            _exchangeOrderRepository.Update(tempexchangeOrderInfo);
                                            _exchangeOrderRepository.SaveChanges();

                                            #region region to update the voucher detail
                                            if (companyName.Contains("Bosch"))
                                            {
                                                _voucherVerificationRepository = new VoucherVerificationRepository();
                                                _sponserManager = new SponserManager();
                                                tblVoucherVerfication voucherVerfication = _voucherVerificationRepository.GetSingle(x => x.ExchangeOrderId == exchangeOrder.Id && x.IsVoucherused == true);
                                                ExchageOrderVoucherUpdateDataContract exchOrderObj = _sponserManager.SetUpdateExchangeVoucherDetail(exchangeOrder.ZohoSponsorOrderId, Convert.ToInt32(exchangeOrder.ExchangePrice).ToString(), voucherVerfication, businessPartner, "Yes");
                                                _sponserManager.UpdateVoucherDetailinExchangeOrder(exchOrderObj);
                                            }
                                            #endregion
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                    }


                    #endregion
                }
                flag = true;
            }
            catch (Exception ex)
            {
                if (OrderId > 0)
                    LibLogging.WriteErrorToDB("ExchangeOrderManager", "MoveUTCbridgeToZoho OrderId: " + OrderId, ex);
                else
                    LibLogging.WriteErrorToDB("ExchangeOrderManager", "MoveUTCbridgeToZoho", ex);
            }

            return flag;
        }

        /// <summary>
        /// Method to set details ('Sponser' form) ExchangeOrderDataContract exchangeOrderDC
        /// </summary>
        /// <param></param>
        /// <returns>model</returns>
        public SponserDataContract SetExchangeOrderObjectToMoveFromUTCtoZoho(tblExchangeOrder exchangeOrder, tblCustomerDetail customerDetail, tblBusinessPartner businessPartner, tblBusinessUnit businessUnit)
        {
            _masterManager = new RDCEL.DocUpload.BAL.ZohoCreatorCall.MasterManager();
            SponserDataContract sponserObj = null;
            SponserSubCategoryListDataContract SponserSubCategoryListDC = null;
            SponsorCategoryListDataContract SponserCategoryListDC = null;
            BrandMasterListDataContract brandMasterListDC = null;
            ProductSizeListDataContract productSizeListDC = null;
            _sponserManager = new SponserManager();
            BrandRepository sponserRepository = new BrandRepository();
            ProductTypeRepository productTypeRepository = new ProductTypeRepository();
            ProductCategoryRepository categoryRepository = new ProductCategoryRepository();
            PriceMasterRepository priceMasterRepository = new PriceMasterRepository();
            try
            {
                if (exchangeOrder != null)
                {
                    sponserObj = new SponserDataContract();
                    sponserObj.Sp_Order_No = exchangeOrder.SponsorOrderNumber;
                    sponserObj.Sponsor_Name = businessUnit.ZohoSponsorId;
                    sponserObj.Regd_No = exchangeOrder.RegdNo;
                    //sponserObj.Customer_Name = new CustomerName();
                    sponserObj.First_Name = customerDetail.FirstName;
                    sponserObj.Last_Name = customerDetail.LastName;
                    sponserObj.Customer_Pincode = customerDetail.ZipCode;
                    sponserObj.Customer_Address_1 = customerDetail.Address1;
                    sponserObj.Customer_Address_2 = customerDetail.Address2;
                    sponserObj.Customer_City = customerDetail.City;
                    if (!string.IsNullOrEmpty(customerDetail.ZipCode))
                    {
                        PinCodeRepository pinCodeRepository = new PinCodeRepository();
                        tblPinCode pinCode = pinCodeRepository.GetSingle(x => x.ZipCode.Equals(customerDetail.ZipCode));
                        sponserObj.Customer_State_Name = pinCode != null && !string.IsNullOrEmpty(pinCode.State) ? pinCode.State : null;
                    }
                    sponserObj.Customer_Email_Address = customerDetail.Email;
                    sponserObj.Customer_Mobile = customerDetail.PhoneNumber;

                    if (businessUnit.BusinessUnitId.Equals(1))
                    {
                        if (exchangeOrder.IsDtoC == true)
                        {
                            exchangeOrder.Sweetener = businessUnit.SweetnerForDTC != null ? (decimal)businessUnit.SweetnerForDTC : 0;
                        }
                        else
                        {
                            exchangeOrder.Sweetener = businessUnit.SweetnerForDTD != null ? (decimal)businessUnit.SweetnerForDTD : 0;
                        }
                    }

                    sponserObj.Sweetener_Bonus_Amount_By_Sponsor = exchangeOrder.Sweetener != null && exchangeOrder.Sweetener > 0 ? exchangeOrder.Sweetener.ToString() : "0";


                    if (businessUnit.BusinessUnitId == 5)
                    {
                        sponserObj.Associate_Name = exchangeOrder.SaleAssociateName;
                        sponserObj.Store_Code = exchangeOrder.StoreCode;
                    }
                    else if (businessUnit.BusinessUnitId == 1)
                    {
                        if (string.IsNullOrEmpty(exchangeOrder.StoreCode))
                        {
                            sponserObj.Associate_Name = exchangeOrder.SaleAssociateName;
                            sponserObj.Store_Phone_Number = exchangeOrder.SalesAssociatePhone;
                            sponserObj.Associate_Email = exchangeOrder.SalesAssociateEmail;
                        }
                        else
                            sponserObj.Store_Code = exchangeOrder.StoreCode;

                    }
                    sponserObj.Associate_Code = businessPartner != null && !string.IsNullOrEmpty(businessPartner.AssociateCode) ? businessPartner.AssociateCode : string.Empty;

                    sponserObj.Purchased_Product_Category = exchangeOrder.PurchasedProductCategory;


                    if (exchangeOrder.ProductCondition.Equals("Excellent"))
                    {
                        sponserObj.Cust_Declared_Qlty = "P";
                    }
                    else if (exchangeOrder.ProductCondition.Equals("Good"))
                    {
                        sponserObj.Cust_Declared_Qlty = "Q";
                    }
                    else if (exchangeOrder.ProductCondition.Equals("Average"))
                    {
                        sponserObj.Cust_Declared_Qlty = "R";
                    }
                    else if (exchangeOrder.ProductCondition.Equals("Not Working"))
                    {
                        sponserObj.Cust_Declared_Qlty = "S";
                    }

                    #region Set From Zoho LOVs


                    if (exchangeOrder.BrandId != 0)
                    {
                        tblBrand brandObj = sponserRepository.GetSingle(x => x.Id.Equals(exchangeOrder.BrandId));
                        if (brandObj != null)
                        {

                            brandMasterListDC = _masterManager.GetAllBrand();
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
                    if (exchangeOrder.ProductTypeId != 0)
                    {
                        tblProductType productTypeObj = productTypeRepository.GetSingle(x => x.Id.Equals(exchangeOrder.ProductTypeId));
                        if (productTypeObj != null)
                        {
                            tblProductCategory productCatObj = categoryRepository.GetSingle(x => x.Id.Equals(productTypeObj.ProductCatId));
                            if (productCatObj != null)
                            {
                                // fill Product Category
                                SponserCategoryListDC = _masterManager.GetAllCategory();
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
                                SponserSubCategoryListDC = _masterManager.GetAllSubCategory();
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

                                productSizeListDC = _masterManager.GetAllProductSize();
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
                            }
                        }
                    }

                    #endregion

                    string orderDate = Convert.ToDateTime(exchangeOrder.CreatedDate).ToString("dd-MMMM-yyyy");
                    sponserObj.Order_Date = orderDate;
                    sponserObj.EVC_Status = "Not Allocated";
                    sponserObj.Order = "0";
                    sponserObj.Order_Type = "Exchange";


                    // new fields  Added
                    if (exchangeOrder.EstimatedDeliveryDate != null)
                    {
                        //DateTime EstimateDeldate = DateTime.ParseExact(exchangeOrder.EstimatedDeliveryDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        DateTime EstimateDeldate = Convert.ToDateTime(exchangeOrder.EstimatedDeliveryDate);
                        sponserObj.Estimate_Delivery_Date = EstimateDeldate.ToString("dd-MMM-yyyy");
                        if (exchangeOrder.EstimatedDeliveryDate != null)
                        {
                            // DateTime date = DateTime.ParseExact(exchangeOrder.EstimatedDeliveryDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture).AddDays(1);
                            DateTime date = Convert.ToDateTime(exchangeOrder.EstimatedDeliveryDate).AddDays(1);
                            string SchedulePickupDate = date.ToString("dd-MMM-yyyy");
                            sponserObj.Expected_Pickup_Date = SchedulePickupDate;
                        }
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
                    sponserObj.Alternate_Email = ConfigurationManager.AppSettings["AlternateEmail"].ToString(); // "logitics@digimart.co.in";
                    sponserObj.Problem_Description = "Exchange";
                    sponserObj.Is_Under_Warranty = "No";
                    sponserObj.Bulk_Mode = "No";

                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeOrderManager", "SetExchangeOrderObjectToMoveFromUTCtoZoho", ex);
            }
            return sponserObj;
        }
        #endregion

        #region Method to get BP is IsOrc and IsDeffered

        /// <summary>
        /// Method to get BP is IsOrc and IsDeffered
        /// </summary>
        /// <param name="">buid</param>
        /// <returns>Bool</returns>
        public bool VerifyIsOrcByBPId(int businessPartnerId)
        {
            _businessPartnerRepository = new BusinessPartnerRepository();
            tblBusinessPartner businessPartner = null;
            //bool flag = false;
            try
            {
                businessPartner = _businessPartnerRepository.GetSingle(x => x.BusinessPartnerId == businessPartnerId && x.IsORC == true && x.IsDefferedSettlement == true);
                if (businessPartner == null)
                {
                    //return flag = false;
                }
                else
                {
                    return true;
                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeOrderManager", "VerifyIsOrcByBPId", ex);
            }
            return false;
        }
        #endregion

        #region Method to get the exchange order detail list by business partner id

        /// <summary>
        /// Method to get the Bulk exchange order detail list by business partner id
        /// </summary>
        /// <param name="businessPartnerId">businessPartnerId</param>
        /// <returns>List ExchangeOrderDataContract</returns>
        public List<ExchangeOrderDataContract> GetBulkExchangeOrderDetailbyBPId(int businessPartnerId)
        {
            List<ExchangeOrderDataContract> exchangeOrderListDC = null;
            ExchangeOrderDataContract exchangeOrderDataObj;
            _exchangeOrderRepository = new ExchangeOrderRepository();
            try
            {
                _exchangeOrderRepository = new ExchangeOrderRepository();
                List<tblExchangeOrder> exchangeOrderObj = _exchangeOrderRepository.GetList(x => x.BusinessPartnerId == businessPartnerId).ToList();
                //exchangeOrderListDC = GenericMapper<tblExchangeOrder, ExchangeOrderDataContract>.MapList(exchangeOrderObj);
                exchangeOrderListDC = new List<ExchangeOrderDataContract>();
                foreach (var item in exchangeOrderObj)
                {
                    exchangeOrderDataObj = new ExchangeOrderDataContract();
                    exchangeOrderDataObj.RegdNo = item.RegdNo;
                    exchangeOrderDataObj.ProductName = item.tblProductType.Description_For_ABB;
                    exchangeOrderDataObj.BrandName = item.tblBrand.Name;
                    exchangeOrderDataObj.ExchangePrice = Convert.ToDecimal(item.ExchangePrice);
                    exchangeOrderListDC.Add(exchangeOrderDataObj);
                }

                return exchangeOrderListDC;
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeOrderManager", "GetBulkExchangeOrderDetailbyBPId", ex);
            }
            return exchangeOrderListDC;
        }

        #endregion

        #region Method to get the exchange order detail list by business partner id for MYGate application

        /// <summary>
        /// Method to get the exchange order detail list by business partner id
        /// </summary>
        /// <param name="businessPartnerId">businessPartnerId</param>
        /// <returns>List ExchangeOrderDataContract</returns>
        public List<ExchangeOrderDataContract> GetMyGateExchangeOrderByBPId(ExchangeOrderDataContract exchangeOrderDataDC)
        {
            _exchangeOrderRepository = new ExchangeOrderRepository();
            _productCategoryRepository = new ProductCategoryRepository();
            _productTypeRepository = new ProductTypeRepository();
            _exchangeOrderStatusRepository = new ExchangeOrderStatusRepository();
            _customerDetailsRepository = new CustomerDetailsRepository();
            List<ExchangeOrderDataContract> exchangeList = new List<ExchangeOrderDataContract>();
            try
            {
                List<tblExchangeOrder> exchangeOrderObj = _exchangeOrderRepository.GetList(x => x.BusinessPartnerId == exchangeOrderDataDC.BusinessPartnerId).ToList();
                //exchangeOrderListDC = GenericMapper<tblExchangeOrder, ExchangeOrderDataContract>.MapList(exchangeOrderObj);
                foreach (var item in exchangeOrderObj)
                {
                    ExchangeOrderDataContract exchangeDc = new ExchangeOrderDataContract();
                    if (item.StatusId != null)
                    {
                        tblExchangeOrderStatu exchangeStatusObject = _exchangeOrderStatusRepository.GetSingle(x => x.Id == item.StatusId);
                        if (exchangeStatusObject != null)
                        {
                            if (exchangeStatusObject.Id == 5)
                            {
                                exchangeDc.OrderStatus = "order created";
                            }
                            else if (exchangeStatusObject.Id == 2 || exchangeStatusObject.Id == 12 || exchangeStatusObject.Id == 14 || exchangeStatusObject.Id == 15 || exchangeStatusObject.Id == 16)
                            {
                                exchangeDc.OrderStatus = exchangeStatusObject.StatusDescription;
                            }
                            else if (exchangeStatusObject.Id == 18 || exchangeStatusObject.Id == 21 || exchangeStatusObject.Id == 23 || exchangeStatusObject.Id == 23 || exchangeStatusObject.Id == 26 || exchangeStatusObject.Id == 30)
                            {
                                exchangeDc.PickupStatus = exchangeStatusObject.StatusDescription;
                            }
                        }
                    }
                    exchangeDc.RegdNo = item.RegdNo;
                    exchangeDc.AssociateName = item.SaleAssociateName;
                    if (item.CustomerDetailsId != null)
                    {
                        tblCustomerDetail CustObj = _customerDetailsRepository.GetSingle(x => x.Id == item.CustomerDetailsId);
                        if (CustObj != null)
                        {
                            exchangeDc.StateName = CustObj.State;
                            exchangeDc.City = CustObj.City;
                        }
                    }
                    if (item.ProductTypeId != null && item.ProductTypeId > 0)
                    {
                        tblProductType productTypeObj = _productTypeRepository.GetSingle(x => x.Id == item.ProductTypeId);
                        exchangeDc.ProductType = productTypeObj.Description;

                        if (productTypeObj.ProductCatId != null && productTypeObj.ProductCatId > 0)
                        {
                            tblProductCategory productCategoryObj = _productCategoryRepository.GetSingle(x => x.Id == productTypeObj.ProductCatId);
                            exchangeDc.ProductCategory = productCategoryObj.Description;
                        }
                    }
                    exchangeList.Add(exchangeDc);
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeOrderManager", "GetMyGateExchangeOrderByBPId", ex);
            }
            return exchangeList;
        }
        #endregion

        #region Update the Order status
        public bool UpdateOrderStatus(string zohoOrderId)
        {
            _exchangeOrderRepository = new ExchangeOrderRepository();
            _exchangeOrderStatusRepository = new ExchangeOrderStatusRepository();
            _sponserManager = new SponserManager();
            _notificationManager = new NotificationManager();
            _customerDetailsRepository = new CustomerDetailsRepository();
            SponsorOrderDetailForExchangeDataContract sponserDC = new SponsorOrderDetailForExchangeDataContract();
            SponserData sponser = new SponserData();
            bool flag = false;
            try
            {
                sponserDC = _sponserManager.GetSponserOrderByIdDetailed(zohoOrderId);
                if (sponserDC != null && sponserDC.data != null)
                {
                    sponser.Latest_Status = sponserDC.data.Latest_Status;

                    tblExchangeOrderStatu exchangeOrderStatu = _exchangeOrderStatusRepository.GetSingle(x => x.StatusCode == sponser.Latest_Status);
                    tblExchangeOrder exchangeOrder = _exchangeOrderRepository.GetSingle(x => x.ZohoSponsorOrderId == zohoOrderId);
                    tblCustomerDetail customerObj = _customerDetailsRepository.GetSingle(x => x.Id == exchangeOrder.CustomerDetailsId);
                    if (exchangeOrderStatu != null && exchangeOrder != null && customerObj != null)
                    {
                        exchangeOrder.StatusId = exchangeOrderStatu.Id;
                        _exchangeOrderRepository.Update(exchangeOrder);
                        _exchangeOrderRepository.SaveChanges();

                        try
                        {
                            if (exchangeOrderStatu != null && exchangeOrderStatu.StatusCode.Equals("8"))
                            {
                                SponsrOrderSyncManager sponsrOrderSyncManager = new SponsrOrderSyncManager();
                                VoucherVerificationResponseViewModel sucessObj = null;
                                sucessObj = sponsrOrderSyncManager.UpdateVoucherstatusToRedeemed(exchangeOrder.RegdNo);
                            }
                            else if (exchangeOrderStatu != null && exchangeOrderStatu.StatusCode.Equals("5") || exchangeOrderStatu.StatusCode.Equals("5X"))
                            {
                                string feedbacklink = ConfigurationManager.AppSettings["BaseURL"].ToString() + "home/fb?rn=" + exchangeOrder.RegdNo;
                                string message = NotificationConstants.SMS_FeedBack.Replace("[FBLink]", feedbacklink);
                                _notificationManager.SendNotificationSMS(customerObj.PhoneNumber, message, null);


                            }

                            if (sponserDC.data != null)
                            {
                                if (!string.IsNullOrEmpty(sponserDC.data.Actual_Total_Amount_as_per_QC))
                                    exchangeOrder.FinalExchangePrice = Convert.ToDecimal(sponserDC.data.Actual_Total_Amount_as_per_QC);

                                //if (exchangeOrder.FinalExchangePrice > 0 && exchangeOrder.Sweetener != null && exchangeOrder.Sweetener > 0)
                                //{
                                //    exchangeOrder.FinalExchangePrice = exchangeOrder.FinalExchangePrice + exchangeOrder.Sweetener;
                                //}
                                _exchangeOrderRepository.Update(exchangeOrder);
                                _exchangeOrderRepository.SaveChanges();

                            }
                        }
                        catch (Exception ex)
                        {
                            LibLogging.WriteErrorToDB("ExchangeOrderManager", "UpdateOrderStatus-In", ex);
                        }
                    }
                }
                flag = true;
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeOrderManager", "UpdateOrderStatus", ex);

            }
            return flag;
        }

        #endregion

        #region ManageD2COrders
        /// <summary>
        /// manage exchange order For D2C
        /// </summary>       
        /// <returns></returns>   
        public ProductOrderResponseDataContract ManageOrderForD2C(ExchangeOrderDataContract exchangeOrderDC)
        {
            int orderId = 0;
            CustomerManager customerInfo = new CustomerManager();
            ProductManager productOrderInfo = new ProductManager();
            _customerDetailsRepository = new CustomerDetailsRepository();
            _notificationManager = new NotificationManager();
            _businessUnitRepository = new BusinessUnitRepository();
            _voucherStatusRepository = new VoucherStatusRepository();
            _productCategoryRepository = new ProductCategoryRepository();
            _productTypeRepository = new ProductTypeRepository();
            _brandRepository = new BrandRepository();
            _whatsAppMessageRepository = new WhatsappMessageRepository();
            SponserManager sponserManager = new SponserManager();
            logging = new Logging();
            string WhatssAppStatusEnum = string.Empty;
            string ResponseCode = string.Empty;
            ProductOrderResponseDataContract productOrderResponseDC = null;
            productOrderResponseDC = new ProductOrderResponseDataContract();
            _exchangeOrderRepository = new ExchangeOrderRepository();
            WhatasappResponse whatssappresponseDC = null;
            OrderPushBackResponse orderPushBackResponse = null;
            string sponsercode = string.Empty;
            string responseforWhatasapp = string.Empty;
            DateTime _dateTime = DateTime.Now.TrimMilliseconds();
            string message = null;
            string ZohoPushFlag = string.Empty;
            string SendSmsFlag = null;
            string BrandName = null;
            string ProductType = null;
            string ProductCategory = null;
            string ProductSize = null;
            try
            {
                SendSmsFlag = ConfigurationManager.AppSettings["sendsmsflag"].ToString();
                if (exchangeOrderDC != null)
                {
                    if (string.IsNullOrEmpty(exchangeOrderDC.Address2))
                    {
                        exchangeOrderDC.Address2 = exchangeOrderDC.Address1;
                    }
                    #region Code to add Customer details in database
                    tblCustomerDetail customerObj = new tblCustomerDetail();
                    customerObj.FirstName = exchangeOrderDC.FirstName;
                    customerObj.LastName = exchangeOrderDC.LastName;
                    customerObj.ZipCode = exchangeOrderDC.ZipCode;
                    customerObj.Address1 = exchangeOrderDC.Address1;
                    customerObj.Address2 = exchangeOrderDC.Address2;
                    customerObj.City = exchangeOrderDC.City;
                    customerObj.State = exchangeOrderDC.StateName;
                    customerObj.Email = exchangeOrderDC.Email;
                    customerObj.PhoneNumber = exchangeOrderDC.PhoneNumber;
                    customerObj.IsActive = true;
                    customerObj.CreatedDate = currentDatetime;
                    _customerDetailsRepository.Add(customerObj);
                    _customerDetailsRepository.SaveChanges();
                    #endregion

                    #region Code to add product order in database
                    if (customerObj != null && customerObj.Id > 0)
                    {
                        tblExchangeOrder exchangeOrderObj = new tblExchangeOrder();
                        exchangeOrderObj.RegdNo = exchangeOrderDC.RegdNo;
                        exchangeOrderObj.ProductTypeId = exchangeOrderDC.ProductTypeId;
                        exchangeOrderObj.BrandId = exchangeOrderDC.BrandId;
                        exchangeOrderObj.Bonus = exchangeOrderDC.Bonus;
                        //exchangeOrderObj.ProductCondition = exchangeOrderDC.ProductCondition;
                        exchangeOrderObj.SponsorOrderNumber = exchangeOrderDC.SponsorOrderNumber;
                        exchangeOrderObj.LoginID = 1;
                        exchangeOrderObj.CustomerDetailsId = exchangeOrderDC.CustomerDetailsId;
                        exchangeOrderObj.CompanyName = exchangeOrderDC.CompanyName;
                        exchangeOrderObj.EstimatedDeliveryDate = exchangeOrderDC.EstimatedDeliveryDate;
                        exchangeOrderObj.IsDefferedSettlement = true;

                        if (string.IsNullOrEmpty(exchangeOrderDC.StoreCode))
                        {
                            exchangeOrderObj.SaleAssociateName = exchangeOrderDC.AssociateName;
                            exchangeOrderObj.SalesAssociateEmail = exchangeOrderDC.AssociateEmail;
                            exchangeOrderObj.SalesAssociatePhone = exchangeOrderDC.StorePhoneNumber;
                        }
                        if (exchangeOrderDC.QualityCheck == 1)
                        {
                            exchangeOrderObj.ProductCondition = "Excellent";
                            exchangeOrderDC.ProductCondition = "Excellent";
                        }
                        if (exchangeOrderDC.QualityCheck == 2)
                        {
                            exchangeOrderObj.ProductCondition = "Good";
                            exchangeOrderDC.ProductCondition = "Good";
                        }
                        if (exchangeOrderDC.QualityCheck == 3)
                        {
                            exchangeOrderObj.ProductCondition = "Average";
                            exchangeOrderDC.ProductCondition = "Average";
                        }
                        if (exchangeOrderDC.QualityCheck == 4)
                        {
                            exchangeOrderObj.ProductCondition = "Not Working";
                            exchangeOrderDC.ProductCondition = "Not Working";
                        }

                        exchangeOrderObj.ExchangePrice = Convert.ToDecimal(exchangeOrderDC.ExchangePriceString);
                        exchangeOrderObj.BaseExchangePrice = Convert.ToDecimal(exchangeOrderDC.BasePrice);
                        exchangeOrderObj.SweetenerBU = exchangeOrderDC.SweetenerBu;
                        exchangeOrderObj.SweetenerBP = exchangeOrderDC.SweetenerBP;
                        exchangeOrderObj.SweetenerDigi2l = exchangeOrderDC.SweetenerDigi2L;
                        exchangeOrderObj.OrderStatus = "Order Created";
                        exchangeOrderObj.StatusId = Convert.ToInt32(StatusEnum.OrderCreated);
                        exchangeOrderObj.IsActive = true;
                        exchangeOrderObj.CreatedDate = currentDatetime;
                        exchangeOrderObj.ModifiedDate = currentDatetime;

                        exchangeOrderObj.CustomerDetailsId = customerObj.Id;
                        exchangeOrderDC.CustomerDetailsId = customerObj.Id;
                        exchangeOrderObj.BusinessPartnerId = exchangeOrderDC.BusinessPartnerId == 0 || exchangeOrderDC.BusinessPartnerId == 999999 ? null : exchangeOrderDC.BusinessPartnerId;
                        exchangeOrderObj.PurchasedProductCategory = exchangeOrderDC.PurchasedProductCategory;
                        exchangeOrderObj.StoreCode = exchangeOrderDC.StoreCode;
                        exchangeOrderObj.SaleAssociateCode = exchangeOrderDC.SaleAssociateCode;
                        exchangeOrderObj.SaleAssociateName = exchangeOrderDC.SaleAssociateName;
                        exchangeOrderObj.SalesAssociateEmail = exchangeOrderDC.AssociateEmail;
                        exchangeOrderObj.QCDate = exchangeOrderDC.QCDate;
                        exchangeOrderObj.StartTime = exchangeOrderDC.StartTime;
                        exchangeOrderObj.EndTime = exchangeOrderDC.EndTime;
                        exchangeOrderObj.IsDtoC = exchangeOrderDC.IsD2C;
                        exchangeOrderObj.EmployeeId = exchangeOrderDC.EmployeeId;
                        exchangeOrderObj.UnInstallationPrice = Convert.ToDecimal(exchangeOrderDC.stringUnInstallationPrice);
                        exchangeOrderObj.IsUnInstallationRequired = exchangeOrderDC.IsUnInstallationRequired;
                        exchangeOrderObj.QCDate = exchangeOrderDC.QCDate;
                        exchangeOrderObj.StartTime = exchangeOrderDC.StartTime;
                        exchangeOrderObj.EndTime = exchangeOrderDC.EndTime;
                        exchangeOrderObj.BusinessUnitId = exchangeOrderDC.BusinessUnitId;
                        exchangeOrderObj.PriceMasterNameId = exchangeOrderDC.priceMasterNameID;
                        exchangeOrderObj.SponsorServiceRefId = exchangeOrderDC.SponsorServiceRefId;

                        bool flag = false;
                        tblBusinessUnit businessUnit = _businessUnitRepository.GetSingle(x => x.BusinessUnitId == exchangeOrderDC.BusinessUnitId);
                        if (businessUnit != null)
                        {
                            exchangeOrderDC.Sweetener = businessUnit.SweetnerForDTD;
                            exchangeOrderObj.Sweetener = businessUnit.SweetnerForDTD;
                        }

                        exchangeOrderObj.BaseExchangePrice = exchangeOrderObj.ExchangePrice - (exchangeOrderObj.Sweetener ?? 0);
                        _exchangeOrderRepository.Add(exchangeOrderObj);
                        _exchangeOrderRepository.SaveChanges();
                        flag = true;
                        if (flag)
                        {
                            productOrderResponseDC.OrderId = exchangeOrderObj.Id;
                            productOrderResponseDC.RegdNo = exchangeOrderDC.RegdNo;
                        }
                        orderId = exchangeOrderObj.Id;
                        exchangeOrderDC.Id = orderId;
                        #region Code to add order in transaction and history
                        int tranid = 0;
                        OrderTransactionManager orderTransactionManager = new OrderTransactionManager();
                        ExchangeABBStatusHistoryManager exchangeABBStatusHistoryManager = new ExchangeABBStatusHistoryManager();
                        tblExchangeOrder exchangeOrder = _exchangeOrderRepository.GetSingle(x => x.Id.Equals(orderId));
                        if (productOrderResponseDC != null && exchangeOrder != null)
                        {
                            //Code for Order tran
                            OrderTransactionDataContract orderTransactionDC = new OrderTransactionDataContract();
                            orderTransactionDC.OrderType = Convert.ToInt32(GraspCorn.Common.Enums.OrderTypeEnum.Exchange);
                            orderTransactionDC.ExchangeId = exchangeOrder.Id;
                            orderTransactionDC.SponsorOrderNumber = exchangeOrder.SponsorOrderNumber;
                            orderTransactionDC.RegdNo = exchangeOrder.RegdNo;
                            orderTransactionDC.ExchangePrice = exchangeOrder.ExchangePrice;
                            orderTransactionDC.Sweetner = exchangeOrder.Sweetener;
                            tranid = orderTransactionManager.MangeOrderTransaction(orderTransactionDC);

                            //Code for Order history
                            if (tranid > 0)
                            {
                                ExchangeABBStatusHistoryDataContract exchangeABBStatusHistoryDC = new ExchangeABBStatusHistoryDataContract();
                                exchangeABBStatusHistoryDC.OrderType = Convert.ToInt32(GraspCorn.Common.Enums.OrderTypeEnum.Exchange);
                                exchangeABBStatusHistoryDC.OrderTransId = tranid;
                                exchangeABBStatusHistoryDC.ExchangeId = exchangeOrder.Id;
                                exchangeABBStatusHistoryDC.SponsorOrderNumber = exchangeOrder.SponsorOrderNumber;
                                exchangeABBStatusHistoryDC.RegdNo = exchangeOrder.RegdNo;
                                exchangeABBStatusHistoryDC.CustId = Convert.ToInt32(exchangeOrder.CustomerDetailsId);
                                exchangeABBStatusHistoryDC.StatusId = Convert.ToInt32(StatusEnum.OrderCreated); ;
                                exchangeABBStatusHistoryManager.MangeOrderHisotry(exchangeABBStatusHistoryDC);
                            }

                        }
                        #endregion
                        if (orderId > 0)
                        {
                            #region Code to send Mail To Customer for exchange 
                            tblProductType productType = _productTypeRepository.GetSingle(x => x.Id == exchangeOrderObj.ProductTypeId);
                            if (productType != null)
                            {
                                tblProductCategory productCategory = _productCategoryRepository.GetSingle(x => x.Id == productType.ProductCatId);
                                if (productCategory != null)
                                {
                                    tblBrand brandobj = _brandRepository.GetSingle(x => x.Id == exchangeOrderObj.BrandId);
                                    if (brandobj != null)
                                    {
                                        BrandName = brandobj.Name;
                                        ProductType = productType.Description;
                                        ProductCategory = productCategory.Description;
                                        ProductSize = productType.Size;
                                        string storeCodeLodha = ExchangeOrderManager.GetEnumDescription(StoreCodeEnum.LodhaGroup);
                                        if (exchangeOrderDC.StoreCode == storeCodeLodha)
                                        {
                                            PushBackOrder pushBackOrder = new PushBackOrder();
                                            #region Order Push Back api for
                                            exchangeOrderDC.ProductType = productType.Description;
                                            exchangeOrderDC.ProductCategory = productCategory.Description;
                                            orderPushBackResponse = pushBackOrder.PushOrderBackForTracking(exchangeOrderDC, tranid);
                                            if (orderPushBackResponse != null)
                                            {
                                                if (orderPushBackResponse.statusCode == Convert.ToInt32(lodhaStatusenum.successfull))
                                                {
                                                    if (orderPushBackResponse.data.bookingId != null)
                                                    {
                                                        exchangeOrderObj.SponsorServiceRefId = orderPushBackResponse.data.bookingId;
                                                        _exchangeOrderRepository.Update(exchangeOrderObj);
                                                        _exchangeOrderRepository.SaveChanges();
                                                    }

                                                }
                                                else
                                                {
                                                    string serializedResponseLodhGroup = JsonConvert.SerializeObject(orderPushBackResponse);
                                                    logging.WriteAPIRequestToDB("orderPushBackResponse", "ManageExchangeOrderD2C", exchangeOrderDC.SponsorOrderNumber, serializedResponseLodhGroup);
                                                }

                                            }
                                            #endregion
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                    #endregion


                    #region Order confirmation whatsapp
                    string ErpBaseUrl = ConfigurationManager.AppSettings["ERPBaseURL"].ToString();
                    string selfQC = ConfigurationManager.AppSettings["SelfQCUrl"].ToString();
                    string url = ErpBaseUrl + "" + selfQC + "" + exchangeOrderDC.RegdNo;
                    OrderConfirmationTemplateExchange whatsappObjforOrderConfirmation = new OrderConfirmationTemplateExchange();
                    whatsappObjforOrderConfirmation.userDetails = new UserDetails();
                    whatsappObjforOrderConfirmation.notification = new OrderConfiirmationNotification();
                    whatsappObjforOrderConfirmation.notification.@params = new SendWhatssappForExcahangeConfirmation();
                    whatsappObjforOrderConfirmation.userDetails.number = exchangeOrderDC.PhoneNumber;
                    whatsappObjforOrderConfirmation.notification.sender = ConfigurationManager.AppSettings["Yello.aiSenderNumber"].ToString();
                    whatsappObjforOrderConfirmation.notification.type = ConfigurationManager.AppSettings["Yellow.aiMesssaheType"].ToString();
                    whatsappObjforOrderConfirmation.notification.templateId = NotificationConstants.orderConfirmationForExchange;
                    whatsappObjforOrderConfirmation.notification.@params.CustName = exchangeOrderDC.FirstName + " " + exchangeOrderDC.LastName;
                    whatsappObjforOrderConfirmation.notification.@params.Link = url;
                    whatsappObjforOrderConfirmation.notification.@params.ProductBrand = BrandName;
                    whatsappObjforOrderConfirmation.notification.@params.ProdCategory = ProductCategory;
                    whatsappObjforOrderConfirmation.notification.@params.ProdType = ProductType;
                    whatsappObjforOrderConfirmation.notification.@params.RegdNO = exchangeOrderDC.RegdNo.ToString();
                    string urlforwhatsapp = ConfigurationManager.AppSettings["Yellow.AiUrl"].ToString();
                    IRestResponse responseConfirmation = WhatsappNotificationManager.Rest_InvokeWhatsappserviceCall(urlforwhatsapp, Method.POST, whatsappObjforOrderConfirmation);
                    ResponseCode = responseConfirmation.StatusCode.ToString();
                    WhatssAppStatusEnum = ExchangeOrderManager.GetEnumDescription(WhatssAppEnum.SuccessCode);
                    if (ResponseCode == WhatssAppStatusEnum)
                    {
                        responseforWhatasapp = responseConfirmation.Content;
                        if (responseforWhatasapp != null)
                        {
                            whatssappresponseDC = JsonConvert.DeserializeObject<WhatasappResponse>(responseforWhatasapp);
                            tblWhatsAppMessage whatsapObj = new tblWhatsAppMessage();
                            whatsapObj.TemplateName = NotificationConstants.orderConfirmationForExchange;
                            whatsapObj.IsActive = true;
                            whatsapObj.PhoneNumber = exchangeOrderDC.PhoneNumber;
                            whatsapObj.SendDate = DateTime.Now;
                            whatsapObj.msgId = whatssappresponseDC.msgId;
                            _whatsAppMessageRepository.Add(whatsapObj);
                            _whatsAppMessageRepository.SaveChanges();
                        }
                        else
                        {
                            string JsonObjectForExchangeOrder = JsonConvert.SerializeObject(exchangeOrderDC);
                            logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", exchangeOrderDC.SponsorOrderNumber, JsonObjectForExchangeOrder);
                        }
                    }
                    else
                    {
                        string JsonObjectForExchangeOrder = JsonConvert.SerializeObject(exchangeOrderDC);
                        logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", exchangeOrderDC.SponsorOrderNumber, JsonObjectForExchangeOrder);
                    }
                    #endregion

                    #region
                    MailJetViewModel mailJet = new MailJetViewModel();
                    MailJetMessage jetmessage = new MailJetMessage();
                    MailJetFrom from = new MailJetFrom();
                    MailjetTo to = new MailjetTo();
                    jetmessage.From = new MailJetFrom() { Email = "customercare@utcdigital.com", Name = "UTC - Customer  Care" };
                    jetmessage.To = new List<MailjetTo>();
                    jetmessage.To.Add(new MailjetTo() { Email = customerObj.Email.Trim(), Name = exchangeOrderDC.FirstName });
                    jetmessage.Subject = exchangeOrderDC.CompanyName + ": Exchange Detail";
                    message = EmailTemplateConstants.ExchangeMailBody();
                    string TemplaTePath = ConfigurationManager.AppSettings["DefferedEmail"].ToString();
                    string FilePath = TemplaTePath;
                    StreamReader str = new StreamReader(FilePath);
                    string MailText = str.ReadToEnd();
                    str.Close();
                    MailText = MailText.Replace("[CustomerName]", exchangeOrderDC.FirstName).Replace("[BusinessUnitName]", exchangeOrderDC.CompanyName).Replace("[SponserOrderNumber]", exchangeOrderDC.SponsorOrderNumber).Replace("[CreatedDate]", Convert.ToDateTime(_dateTime).ToString("dd/MM/yyyy")).Replace("[CustName]", exchangeOrderDC.FirstName).Replace("[CustMobile]", customerObj.PhoneNumber).Replace("[CustAdd1]", exchangeOrderDC.Address1)
                        .Replace("[CustAdd2]", exchangeOrderDC.Address2).Replace("[State]", exchangeOrderDC.StateName).Replace("[PinCode]", exchangeOrderDC.ZipCode).Replace("[CustCity]", exchangeOrderDC.City).Replace("[ProductCategory]", ProductCategory)
                        .Replace("[OldProdType]", ProductType).Replace("[OldBrand]", BrandName).Replace("[Size]", ProductSize).Replace("[ExchangePrice]", exchangeOrderDC.ExchangePriceString).Replace("[EstimatedDeliveryDate]", exchangeOrderDC.EstimatedDeliveryDate).Replace("[SelfQCLink]", url);
                    jetmessage.HTMLPart = MailText;
                    mailJet.Messages = new List<MailJetMessage>();
                    mailJet.Messages.Add(jetmessage);
                    BillCloudServiceCall.MailJetSendMailService(mailJet);
                    #endregion
                }

            }
            catch (Exception ex)
            {

                LibLogging.WriteErrorToDB("ExchangeOrderManager", "ManageOrderForD2C", ex);
            }

            return productOrderResponseDC;
        }
        #endregion

        #region Update exchange order
        /// <summary>
        /// manage exchange order
        /// </summary>       
        /// <returns></returns>   
        public ProductOrderResponseDataContract UpdateExchangeOrder(ExchangeOrderDataContract exchangeOrderDC)
        {
            CustomerManager customerInfo = new CustomerManager();
            ProductManager productOrderInfo = new ProductManager();
            _customerDetailsRepository = new CustomerDetailsRepository();
            _notificationManager = new NotificationManager();
            _whatsappManager = new WhatsappNotificationManager();
            _businessUnitRepository = new BusinessUnitRepository();
            _voucherStatusRepository = new VoucherStatusRepository();
            _businessPartnerRepository = new BusinessPartnerRepository();
            _whatsAppMessageRepository = new WhatsappMessageRepository();
            _productCategoryRepository = new ProductCategoryRepository();
            _productTypeRepository = new ProductTypeRepository();
            _brandRepository = new BrandRepository();
            logging = new Logging();
            _orderTransactionRepository = new OrderTransactionRepository();
            SponserManager sponserManager = new SponserManager();
            WhatasappResponse whatssappresponseDC = null;
            string WhatssAppStatusEnum = string.Empty;
            string ResponseCode = string.Empty;
            ProductOrderResponseDataContract productOrderResponseDC = null;
            productOrderResponseDC = new ProductOrderResponseDataContract();
            _exchangeOrderRepository = new ExchangeOrderRepository();
            MailJetViewModel mailJet = new MailJetViewModel();
            MailJetMessage jetmessage = new MailJetMessage();
            MailJetFrom from = new MailJetFrom();
            MailjetTo to = new MailjetTo();
            string sponsercode = string.Empty;
            string responseforWhatasapp = string.Empty;
            string SendMessageFlag = null;
            int price = 0;
            string voucherName = null;
            DateTime _dateTime = DateTime.Now.TrimMilliseconds();
            string ZohoPushFlag = string.Empty;
            //string BrandName = null;
            //string ProductCategory = null;
            //string ProductType = null;
            tblExchangeOrder exchangeOrderObj = null;
            try
            {
                SendMessageFlag = ConfigurationManager.AppSettings["sendsmsflag"].ToString();
                exchangeOrderObj = _exchangeOrderRepository.GetSingle(x => !string.IsNullOrEmpty(x.RegdNo) && x.RegdNo.ToLower().Equals(exchangeOrderDC.RegdNo.ToLower()));
                if (exchangeOrderObj != null)
                {
                    if (string.IsNullOrEmpty(exchangeOrderDC.Address2))
                    {
                        exchangeOrderDC.Address2 = exchangeOrderDC.Address1;
                    }
                    #region code to Update customer details
                    tblCustomerDetail customerObj = _customerDetailsRepository.GetSingle(x => x.Id.Equals(exchangeOrderObj.CustomerDetailsId));
                    if (exchangeOrderDC != null)
                    {
                        customerObj.FirstName = exchangeOrderDC.FirstName;
                        customerObj.LastName = exchangeOrderDC.LastName;
                        customerObj.ZipCode = exchangeOrderDC.ZipCode;
                        customerObj.Address1 = exchangeOrderDC.Address1;
                        customerObj.Address2 = exchangeOrderDC.Address2;
                        customerObj.City = exchangeOrderDC.City;
                        customerObj.PhoneNumber = exchangeOrderDC.PhoneNumber;
                        customerObj.State = exchangeOrderDC.StateName;
                        customerObj.Email = exchangeOrderDC.Email;
                        customerObj.IsActive = true;
                        customerObj.CreatedDate = currentDatetime;
                        _customerDetailsRepository.Update(customerObj);
                        _customerDetailsRepository.SaveChanges();
                    }
                    #endregion
                    if (exchangeOrderObj != null)
                    {
                        tblBusinessUnit businessUnit = _businessUnitRepository.GetSingle(x => x.BusinessUnitId == exchangeOrderDC.BusinessUnitId);
                        if (businessUnit != null)
                        {
                            if (exchangeOrderDC.QualityCheck == 1)
                            {
                                exchangeOrderObj.ProductCondition = "Excellent";
                            }
                            if (exchangeOrderDC.QualityCheck == 2)
                            {
                                exchangeOrderObj.ProductCondition = "Good";
                            }
                            if (exchangeOrderDC.QualityCheck == 3)
                            {
                                exchangeOrderObj.ProductCondition = "Average";
                            }
                            if (exchangeOrderDC.QualityCheck == 4)
                            {
                                exchangeOrderObj.ProductCondition = "Not Working";
                            }
                            #region For pine labs Store
                            voucherName = "Generated";
                            tblVoucherStatu voucherStatu = _voucherStatusRepository.GetSingle(x => x.VoucherStatusName == voucherName);
                            exchangeOrderObj.VoucherCodeExpDate = DateTime.Now.AddHours(Convert.ToDouble(businessUnit.VoucherExpiryTime));
                            exchangeOrderObj.VoucherCode = GenerateVoucher();
                            exchangeOrderObj.IsVoucherused = false;
                            exchangeOrderObj.VoucherStatusId = voucherStatu.VoucherStatusId;
                            exchangeOrderObj.IsActive = true;
                            exchangeOrderObj.IsDefferedSettlement = false;
                            //exchangeOrderObj.Sweetener = businessUnit.SweetnerForDTD;
                            exchangeOrderObj.ModifiedDate = DateTime.Now;
                            exchangeOrderObj.ExchangePrice = Convert.ToDecimal(exchangeOrderDC.ExchangePrice);
                            //exchangeOrderObj.BaseExchangePrice = exchangeOrderObj.ExchangePrice - (exchangeOrderObj.Sweetener ?? 0);
                            exchangeOrderObj.BaseExchangePrice = Convert.ToDecimal(exchangeOrderDC.BasePrice);
                            exchangeOrderObj.Sweetener = exchangeOrderDC.SweetenerTotal;
                            exchangeOrderObj.SweetenerBP = exchangeOrderDC.SweetenerBP;
                            exchangeOrderObj.SweetenerBU = exchangeOrderDC.SweetenerBu;
                            exchangeOrderObj.SweetenerDigi2l = exchangeOrderDC.SweetenerDigi2L;
                            exchangeOrderObj.PriceMasterNameId = exchangeOrderDC.priceMasterNameID;
                            _exchangeOrderRepository.Update(exchangeOrderObj);
                            _exchangeOrderRepository.SaveChanges();
                            price = (int)exchangeOrderObj.ExchangePrice;
                            exchangeOrderDC.ExchangePriceString = price.ToString();
                            exchangeOrderDC.Sweetener = exchangeOrderDC.SweetenerTotal;
                            #region Code to add order in transaction and history
                            tblOrderTran ordertransObj = _orderTransactionRepository.GetSingle(x => x.ExchangeId == exchangeOrderObj.Id && x.IsActive == true);
                            if (ordertransObj != null)
                            {
                                ordertransObj.ExchangePrice = exchangeOrderObj.ExchangePrice;
                                ordertransObj.Sweetner = exchangeOrderObj.Sweetener;
                                _orderTransactionRepository.Update(ordertransObj);
                                _orderTransactionRepository.SaveChanges();
                            }
                            #endregion
                            string message = null;
                            //send sms
                            #region Code to send notification to customer for voucher generation
                            string voucherUrl = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Home/V/" + exchangeOrderObj.Id;
                            if (SendMessageFlag == "true")
                            {
                                message = NotificationConstants.SMS_VoucherRedemption_Confirmation.Replace("[ExchPrice]", exchangeOrderDC.ExchangePriceString).Replace("[VCODE]", exchangeOrderObj.VoucherCode)
                               .Replace("[VLink]", "( " + voucherUrl + " )").Replace("[STORENAME]", businessUnit.Name).Replace("[COMPANY]", businessUnit.Name).Replace("[VALIDTILLDATE]", Convert.ToDateTime(exchangeOrderObj.VoucherCodeExpDate).ToString("dd/MM/yyyy"));
                                _notificationManager.SendNotificationSMS(exchangeOrderDC.PhoneNumber, message, null);
                            }
                            jetmessage.From = new MailJetFrom() { Email = "customercare@utcdigital.com", Name = "UTC - Customer  Care" };
                            jetmessage.To = new List<MailjetTo>();
                            jetmessage.To.Add(new MailjetTo() { Email = customerObj.Email.Trim(), Name = exchangeOrderDC.FirstName });
                            jetmessage.Subject = businessUnit.Name + ": Exchange Voucher Detail";
                            string TemplaTePathforvoucher = ConfigurationManager.AppSettings["VoucherGenerationInstant"].ToString();
                            string FilePathvc = TemplaTePathforvoucher;
                            StreamReader stri = new StreamReader(FilePathvc);
                            string MailTextt = stri.ReadToEnd();
                            stri.Close();
                            MailTextt = MailTextt.Replace("[ExchPrice]", exchangeOrderDC.ExchangePriceString).Replace("[VCode]", exchangeOrderObj.VoucherCode).Replace("[FirstName]", exchangeOrderDC.FirstName)
                                .Replace("[VLink]", voucherUrl).Replace("[STORENAME]", businessUnit.Name).Replace("[VALIDTILLDATE]", Convert.ToDateTime(exchangeOrderObj.VoucherCodeExpDate).ToString("dd/MM/yyyy"));
                            jetmessage.HTMLPart = MailTextt;
                            mailJet.Messages = new List<MailJetMessage>();
                            mailJet.Messages.Add(jetmessage);
                            BillCloudServiceCall.MailJetSendMailService(mailJet);
                            #endregion
                            #endregion
                            //tblProductType productType = _productTypeRepository.GetSingle(x => x.Id == exchangeOrderObj.ProductTypeId);
                            //if (productType != null)
                            //{
                            //    tblProductCategory productCategory = _productCategoryRepository.GetSingle(x => x.Id == productType.ProductCatId);
                            //    if (productCategory != null)
                            //    {
                            //        //tblBrand brandobj = _brandRepository.GetSingle(x => x.Id == exchangeOrderObj.BrandId);
                            //        //if (brandobj != null)
                            //        //{
                            //        //    BrandName = brandobj.Name;
                            //        //    ProductCategory = productCategory.Description;
                            //        //    ProductType = productType.Description;
                            //        //    string ErpBaseUrl = ConfigurationManager.AppSettings["ERPBaseURL"].ToString();
                            //        //    string selfQC = ConfigurationManager.AppSettings["SelfQCUrl"].ToString();
                            //        //    string url = ErpBaseUrl + "" + selfQC + "" + exchangeOrderDC.RegdNo;
                            //        //    #region Code to send mail to customer for exchange details
                            //        //    jetmessage.From = new MailJetFrom() { Email = "customercare@utcdigital.com", Name = "UTC - Customer  Care" };
                            //        //    jetmessage.To = new List<MailjetTo>();
                            //        //    jetmessage.To.Add(new MailjetTo() { Email = customerObj.Email.Trim(), Name = exchangeOrderDC.FirstName });
                            //        //    jetmessage.Subject = businessUnit.Name + ": Exchange Detail";
                            //        //    message = EmailTemplateConstants.EmailForInstant();
                            //        //    string TemplaTePath = ConfigurationManager.AppSettings["InstantEmail"].ToString();
                            //        //    string FilePath = TemplaTePath;
                            //        //    StreamReader str = new StreamReader(FilePath);
                            //        //    string MailText = str.ReadToEnd();
                            //        //    str.Close();
                            //        //    MailText = MailText.Replace("[CustomerName]", exchangeOrderDC.FirstName).Replace("[BusinessUnitName]", exchangeOrderDC.CompanyName).Replace("[SponserOrderNumber]", exchangeOrderObj.SponsorOrderNumber).Replace("[CreatedDate]", Convert.ToDateTime(_dateTime).ToString("dd/MM/yyyy")).Replace("[CustName]", exchangeOrderDC.FirstName).Replace("[CustMobile]", customerObj.PhoneNumber).Replace("[CustAdd1]", exchangeOrderDC.Address1)
                            //        //        .Replace("[CustAdd2]", exchangeOrderDC.Address2).Replace("[State]", exchangeOrderDC.StateName).Replace("[PinCode]", exchangeOrderDC.ZipCode).Replace("[CustCity]", exchangeOrderDC.City).Replace("[ProductCategory]", productCategory.Description)
                            //        //        .Replace("[OldProdType]", productType.Description).Replace("[OldBrand]", brandobj.Name).Replace("[Size]", productType.Size).Replace("[ExchangePrice]", exchangeOrderDC.ExchangePriceString).Replace("[EstimatedDeliveryDate]", exchangeOrderObj.EstimatedDeliveryDate).Replace("[SelfQCLink]", url);
                            //        //    jetmessage.HTMLPart = MailText;
                            //        //    mailJet.Messages = new List<MailJetMessage>();
                            //        //    mailJet.Messages.Add(jetmessage);
                            //        //    BillCloudServiceCall.MailJetSendMailService(mailJet);
                            //        //    #endregion
                            //        //}
                            //    }
                            //}

                        }
                        if (exchangeOrderDC.BusinessPartnerId > 0)
                        {
                            tblBusinessPartner businessPartner = _businessPartnerRepository.GetSingle(x => x.IsActive == true && x.BusinessPartnerId == exchangeOrderDC.BusinessPartnerId);

                            if (businessPartner != null)
                            {
                                #region Code to send notification to customer
                                string message = null;
                                if (SendMessageFlag == "true")
                                {
                                    message = NotificationConstants.SMS_Order_Recive_Confirmation.Replace("[REGNO]", exchangeOrderObj.RegdNo).Replace("[STORENAME]", businessUnit.Name.Trim());
                                    _notificationManager.SendNotificationSMS(exchangeOrderDC.PhoneNumber, message, null);
                                }
                                //Template For Voucher Code
                                #region Whatsapp Template for voucher code
                                WhatsappTemplate whatsappObj = new WhatsappTemplate();
                                whatsappObj.userDetails = new UserDetails();
                                whatsappObj.notification = new Notification();
                                whatsappObj.notification.@params = new SendVoucherOnWhatssapp();
                                whatsappObj.userDetails.number = exchangeOrderDC.PhoneNumber;
                                whatsappObj.notification.sender = ConfigurationManager.AppSettings["Yello.aiSenderNumber"].ToString();
                                whatsappObj.notification.type = ConfigurationManager.AppSettings["Yellow.aiMesssaheType"].ToString();
                                whatsappObj.notification.templateId = NotificationConstants.Send_Voucher_Code_Template;
                                whatsappObj.notification.@params.voucherAmount = exchangeOrderObj.ExchangePrice.ToString();
                                whatsappObj.notification.@params.VoucherExpiry = Convert.ToDateTime(exchangeOrderObj.VoucherCodeExpDate).ToString("dd/MM/yyyy");
                                whatsappObj.notification.@params.voucherCode = exchangeOrderObj.VoucherCode.ToString();
                                whatsappObj.notification.@params.BrandName = businessUnit.Name.ToString();
                                whatsappObj.notification.@params.BrandName2 = businessUnit.Name.ToString();
                                whatsappObj.notification.@params.VoucherLink = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Home/V/" + exchangeOrderObj.Id;
                                string url = ConfigurationManager.AppSettings["Yellow.AiUrl"].ToString();
                                IRestResponse response = WhatsappNotificationManager.Rest_InvokeWhatsappserviceCall(url, Method.POST, whatsappObj);
                                ResponseCode = response.StatusCode.ToString();
                                WhatssAppStatusEnum = ExchangeOrderManager.GetEnumDescription(WhatssAppEnum.SuccessCode);
                                if (ResponseCode == WhatssAppStatusEnum)
                                {
                                    responseforWhatasapp = response.Content;
                                    if (responseforWhatasapp != null)
                                    {
                                        whatssappresponseDC = JsonConvert.DeserializeObject<WhatasappResponse>(responseforWhatasapp);
                                        tblWhatsAppMessage whatsapObj = new tblWhatsAppMessage();
                                        whatsapObj.TemplateName = NotificationConstants.Send_Voucher_Code_Template;
                                        whatsapObj.IsActive = true;
                                        whatsapObj.PhoneNumber = exchangeOrderDC.PhoneNumber;
                                        whatsapObj.SendDate = DateTime.Now;
                                        whatsapObj.msgId = whatssappresponseDC.msgId;
                                        _whatsAppMessageRepository.Add(whatsapObj);
                                        _whatsAppMessageRepository.SaveChanges();
                                    }
                                    else
                                    {
                                        string ExchOrderObj = JsonConvert.SerializeObject(exchangeOrderDC);
                                        logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", exchangeOrderDC.SponsorOrderNumber, ExchOrderObj);
                                    }
                                }
                                else
                                {
                                    string ExchOrderObj = JsonConvert.SerializeObject(exchangeOrderDC);
                                    logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", exchangeOrderDC.SponsorOrderNumber, ExchOrderObj);
                                }
                                #endregion
                                #endregion

                                //WhatsappTemplateIntegration for Order Confirmation
                                string ErpBaseUrl = ConfigurationManager.AppSettings["ERPBaseURL"].ToString();
                                string selfQC = ConfigurationManager.AppSettings["SelfQCUrl"].ToString();
                                string selfqcurl = ErpBaseUrl + "" + selfQC + "" + exchangeOrderDC.RegdNo;
                                //#region TO send WhatsappNotificatio for Order Confirmation
                                //OrderConfirmationTemplateExchange whatsappObjforOrderConfirmation = new OrderConfirmationTemplateExchange();
                                //whatsappObjforOrderConfirmation.userDetails = new UserDetails();
                                //whatsappObjforOrderConfirmation.notification = new OrderConfiirmationNotification();
                                //whatsappObjforOrderConfirmation.notification.@params = new SendWhatssappForExcahangeConfirmation();
                                //whatsappObjforOrderConfirmation.userDetails.number = exchangeOrderDC.PhoneNumber;
                                //whatsappObjforOrderConfirmation.notification.sender = ConfigurationManager.AppSettings["Yello.aiSenderNumber"].ToString();
                                //whatsappObjforOrderConfirmation.notification.type = ConfigurationManager.AppSettings["Yellow.aiMesssaheType"].ToString();
                                //whatsappObjforOrderConfirmation.notification.templateId = NotificationConstants.orderConfirmationForExchange;
                                //whatsappObjforOrderConfirmation.notification.@params.CustName = exchangeOrderDC.FirstName + " " + exchangeOrderDC.LastName;
                                //whatsappObjforOrderConfirmation.notification.@params.Link = selfqcurl;
                                //whatsappObjforOrderConfirmation.notification.@params.ProductBrand = BrandName;
                                //whatsappObjforOrderConfirmation.notification.@params.ProdCategory = ProductCategory;
                                //whatsappObjforOrderConfirmation.notification.@params.ProdType = ProductType;
                                //whatsappObjforOrderConfirmation.notification.@params.RegdNO = exchangeOrderDC.RegdNo.ToString();
                                //string urlforwhatsapp = ConfigurationManager.AppSettings["Yellow.AiUrl"].ToString();
                                //IRestResponse responseConfirmation = WhatsappNotificationManager.Rest_InvokeWhatsappserviceCall(urlforwhatsapp, Method.POST, whatsappObjforOrderConfirmation);
                                //ResponseCode = responseConfirmation.StatusCode.ToString();
                                //WhatssAppStatusEnum = ExchangeOrderManager.GetEnumDescription(WhatssAppEnum.SuccessCode);
                                //if (ResponseCode == WhatssAppStatusEnum)
                                //{
                                //    responseforWhatasapp = responseConfirmation.Content;
                                //    if (responseforWhatasapp != null)
                                //    {
                                //        whatssappresponseDC = JsonConvert.DeserializeObject<WhatasappResponse>(responseforWhatasapp);
                                //        tblWhatsAppMessage whatsapObj = new tblWhatsAppMessage();
                                //        whatsapObj.TemplateName = NotificationConstants.orderConfirmationForExchange;
                                //        whatsapObj.IsActive = true;
                                //        whatsapObj.PhoneNumber = exchangeOrderDC.PhoneNumber;
                                //        whatsapObj.SendDate = DateTime.Now;
                                //        whatsapObj.msgId = whatssappresponseDC.msgId;
                                //        _whatsAppMessageRepository.Add(whatsapObj);
                                //        _whatsAppMessageRepository.SaveChanges();
                                //    }
                                //    else
                                //    {
                                //        string JsonObjectForExchangeOrder = JsonConvert.SerializeObject(exchangeOrderDC);
                                //        logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", exchangeOrderDC.SponsorOrderNumber, JsonObjectForExchangeOrder);
                                //    }
                                //}
                                //else
                                //{
                                //    string JsonObjectForExchangeOrder = JsonConvert.SerializeObject(exchangeOrderDC);
                                //    logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", exchangeOrderDC.SponsorOrderNumber, JsonObjectForExchangeOrder);
                                //}
                                //#endregion
                            }
                        }
                    }

                }
                if (exchangeOrderObj != null)
                {
                    productOrderResponseDC.RegdNo = exchangeOrderDC.RegdNo;
                    productOrderResponseDC.OrderId = exchangeOrderObj.Id;
                }


            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeOrderManager", "UpdateExchangeOrder", ex);
            }

            return productOrderResponseDC;
        }
        #endregion

        #region update zoho Customer detail
        /// <summary>
        /// Method to Update details ('Sponser' form) ExchangeOrderDataContract exchangeOrderDC
        /// </summary>
        /// <param></param>
        /// <returns>model</returns>
        public CustomerDetailsUpdateDatacontract SetCustomerUpdateObject(ExchangeOrderDataContract exchangeOrderDC)
        {
            _masterManager = new RDCEL.DocUpload.BAL.ZohoCreatorCall.MasterManager();
            _sponserManager = new SponserManager();
            CustomerDetailsUpdateDatacontract zohoCustomerObj = new CustomerDetailsUpdateDatacontract();
            zohoCustomerObj.Customer_Name = new CustomerName();
            BrandRepository sponserRepository = new BrandRepository();
            ProductTypeRepository productTypeRepository = new ProductTypeRepository();
            ProductCategoryRepository categoryRepository = new ProductCategoryRepository();
            PriceMasterRepository priceMasterRepository = new PriceMasterRepository();
            BusinessPartnerRepository _businessPartnerRepository = new BusinessPartnerRepository();
            try
            {
                if (exchangeOrderDC != null)
                {
                    if (string.IsNullOrEmpty(exchangeOrderDC.Address2))
                    {
                        exchangeOrderDC.Address2 = exchangeOrderDC.Address1;
                    }
                    zohoCustomerObj.Id = exchangeOrderDC.ZohoSponsorOrderId;
                    zohoCustomerObj.Customer_Name.first_name = exchangeOrderDC.FirstName;
                    zohoCustomerObj.Customer_Name.last_name = exchangeOrderDC.LastName;
                    zohoCustomerObj.Customer_State_Name = exchangeOrderDC.StateName;
                    zohoCustomerObj.Customer_City = exchangeOrderDC.City;
                    zohoCustomerObj.Customer_Pincode = exchangeOrderDC.ZipCode;
                    zohoCustomerObj.Customer_Mobile = exchangeOrderDC.PhoneNumber;
                    zohoCustomerObj.Customer_Email_Address = exchangeOrderDC.Email;
                    zohoCustomerObj.Customer_Address_1 = exchangeOrderDC.Address1;
                    zohoCustomerObj.Customer_Address_2 = exchangeOrderDC.Address2;
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeOrderManager", "SetExchangeOrderObject", ex);
            }
            return zohoCustomerObj;
        }
        #endregion

        #region Method To Set Response Object For Exchange Order On Basis of Voucher code and Phone no
        public ExchangeOrderDetail GetExchangeDetailForVoucherCode(tblExchangeOrder exchangeOrderDC, tblCustomerDetail customerdetailDC, tblABBRedemption RedemptionObj, tblABBRegistration RegistrationObj)
        {
            _notificationManager = new NotificationManager();
            _businessPartnerRepository = new BusinessPartnerRepository();
            _businessUnitRepository = new BusinessUnitRepository();
            _exchangeOrderRepository = new ExchangeOrderRepository();
            _customerDetailsRepository = new CustomerDetailsRepository();
            _productCategoryRepository = new ProductCategoryRepository();
            _productTypeRepository = new ProductTypeRepository();
            _brandRepository = new BrandRepository();
            abbRedemptionRepository = new AbbRedemptionRepository();
            aBBRegistrationRepository = new ABBRegistrationRepository();
            brandSmartSellRepository = new BrandSmartSellRepository();
            ExchangeOrderDetail exchangeDetail = new ExchangeOrderDetail();
            List<ExchangeOrderDetail> exchangelist = new List<ExchangeOrderDetail>();
            exchangeDetail.exchangedetail = new Exchangedetail();
            exchangeDetail.customerdetail = new CustomerDetail();
            exchangeDetail.otp = new OTP();
            try
            {

                if (exchangeOrderDC != null && customerdetailDC != null)
                {
                    //GetBrand Details
                    tblBrand brandObj = _brandRepository.GetSingle(x => x.Id.Equals(exchangeOrderDC.BrandId));
                    {

                        tblProductType productType = _productTypeRepository.GetSingle(x => x.Id.Equals(exchangeOrderDC.ProductTypeId));

                        {
                            tblProductCategory productCategoryObj = _productCategoryRepository.GetSingle(x => x.Id.Equals(productType.ProductCatId));

                            {
                                //Old Product Details 
                                exchangeDetail.exchangedetail.OrderId = exchangeOrderDC.Id;
                                exchangeDetail.exchangedetail.BrandId = brandObj.Id;
                                exchangeDetail.exchangedetail.BrandName = brandObj.Name;
                                exchangeDetail.exchangedetail.Bonus = exchangeOrderDC.Bonus;
                                exchangeDetail.exchangedetail.ProductCondition = exchangeOrderDC.ProductCondition;
                                exchangeDetail.exchangedetail.RegdNo = exchangeOrderDC.RegdNo;

                                //Voucher Details
                                exchangeDetail.exchangedetail.VoucherCode = exchangeOrderDC.VoucherCode;
                                exchangeDetail.exchangedetail.VoucherDiscountPrice = exchangeOrderDC.ExchangePrice.ToString();
                                //product Type
                                exchangeDetail.exchangedetail.ProductTypeId = (int)exchangeOrderDC.ProductTypeId;
                                exchangeDetail.exchangedetail.ProductTypeDescription = productType.Description;
                                //product Category
                                exchangeDetail.exchangedetail.ProductCategoryId = productCategoryObj.Id;
                                exchangeDetail.exchangedetail.ProductCategoryName = productCategoryObj.Name;
                                exchangeDetail.exchangedetail.ProductCategoryDescription = productCategoryObj.Description;

                                //customer Details
                                exchangeDetail.customerdetail.FirstName = customerdetailDC.FirstName;
                                exchangeDetail.customerdetail.LastName = customerdetailDC.LastName;
                                exchangeDetail.customerdetail.Email = customerdetailDC.Email;
                                exchangeDetail.customerdetail.ZipCode = customerdetailDC.ZipCode;
                                exchangeDetail.customerdetail.State = customerdetailDC.State;
                                exchangeDetail.customerdetail.City = customerdetailDC.City;
                                exchangeDetail.customerdetail.PhoneNumber = customerdetailDC.PhoneNumber;
                                exchangeDetail.customerdetail.Address1 = customerdetailDC.Address1;
                                exchangeDetail.customerdetail.Address2 = customerdetailDC.Address2;


                                tblBusinessPartner businessPartner = _businessPartnerRepository.GetSingle(x => x.BusinessPartnerId.Equals(exchangeOrderDC.BusinessPartnerId));
                                {

                                    tblBusinessUnit businessUnit = _businessUnitRepository.GetSingle(x => x.BusinessUnitId.Equals(businessPartner.BusinessUnitId));
                                    if (businessUnit.BusinessUnitId > 0)
                                    {
                                        string OTPValue = UniqueString.RandomNumber();
                                        exchangeDetail.otp.Otp = OTPValue;
                                        string message = NotificationConstants.SMS_ExchangeOtp.Replace("[OTP]", OTPValue).Replace("[BrandName]", businessUnit.Name);
                                        _notificationManager.SendNotificationSMS(customerdetailDC.PhoneNumber, message, OTPValue);

                                    }


                                }

                            }
                        }
                    }
                }
                else if (RedemptionObj != null && RegistrationObj != null && customerdetailDC != null)
                {
                    //Code to get brand details
                    tblBrandSmartBuy brandsamrtBuyObj = brandSmartSellRepository.GetSingle(x => x.IsActive == true && x.Id == RegistrationObj.NewBrandId);
                    tblBrand brandObj = _brandRepository.GetSingle(x => x.IsActive == true && x.Id == RegistrationObj.NewBrandId);
                    if (brandsamrtBuyObj != null)
                    {
                        exchangeDetail.exchangedetail.BrandId = brandsamrtBuyObj.Id;
                        exchangeDetail.exchangedetail.BrandName = brandsamrtBuyObj.Name;
                    }
                    else if (brandObj != null)
                    {
                        exchangeDetail.exchangedetail.BrandId = brandObj.Id;
                        exchangeDetail.exchangedetail.BrandName = brandObj.Name;
                    }

                    //Code to get Order Details
                    tblProductCategory productCategoryObj = _productCategoryRepository.GetSingle(x => x.IsActive == true && x.Id == RegistrationObj.NewProductCategoryId);
                    if (productCategoryObj != null)
                    {
                        tblProductType producttypeObj = _productTypeRepository.GetSingle(x => x.IsActive == true && x.Id == RegistrationObj.NewProductCategoryTypeId);
                        if (producttypeObj != null)
                        {
                            //Old Product Details 
                            exchangeDetail.exchangedetail.OrderId = RedemptionObj.RedemptionId;
                            exchangeDetail.exchangedetail.Bonus = 0.ToString();
                            exchangeDetail.exchangedetail.ProductCondition = null;
                            exchangeDetail.exchangedetail.RegdNo = RedemptionObj.RegdNo;

                            //Voucher Details
                            exchangeDetail.exchangedetail.VoucherCode = RedemptionObj.VoucherCode;
                            exchangeDetail.exchangedetail.VoucherDiscountPrice = RedemptionObj.RedemptionValue.ToString();
                            //product Type
                            exchangeDetail.exchangedetail.ProductTypeId = producttypeObj.Id;
                            exchangeDetail.exchangedetail.ProductTypeDescription = producttypeObj.Description;
                            //product Category
                            exchangeDetail.exchangedetail.ProductCategoryId = productCategoryObj.Id;
                            exchangeDetail.exchangedetail.ProductCategoryName = productCategoryObj.Name;
                            exchangeDetail.exchangedetail.ProductCategoryDescription = productCategoryObj.Description;

                            //customer Details
                            exchangeDetail.customerdetail.FirstName = customerdetailDC.FirstName;
                            exchangeDetail.customerdetail.LastName = customerdetailDC.LastName;
                            exchangeDetail.customerdetail.Email = customerdetailDC.Email;
                            exchangeDetail.customerdetail.ZipCode = customerdetailDC.ZipCode;
                            exchangeDetail.customerdetail.State = customerdetailDC.State;
                            exchangeDetail.customerdetail.City = customerdetailDC.City;
                            exchangeDetail.customerdetail.PhoneNumber = customerdetailDC.PhoneNumber;
                            exchangeDetail.customerdetail.Address1 = customerdetailDC.Address1;
                            exchangeDetail.customerdetail.Address2 = customerdetailDC.Address2;


                            tblBusinessPartner businessPartner = _businessPartnerRepository.GetSingle(x => x.BusinessPartnerId.Equals(RegistrationObj.BusinessPartnerId));
                            {

                                tblBusinessUnit businessUnit = _businessUnitRepository.GetSingle(x => x.BusinessUnitId.Equals(businessPartner.BusinessUnitId));
                                if (businessUnit.BusinessUnitId > 0)
                                {
                                    string OTPValue = UniqueString.RandomNumber();
                                    exchangeDetail.otp.Otp = OTPValue;
                                    string message = NotificationConstants.SMS_ExchangeOtp.Replace("[OTP]", OTPValue).Replace("[BrandName]", businessUnit.Name);
                                    _notificationManager.SendNotificationSMS(customerdetailDC.PhoneNumber, message, OTPValue);

                                }


                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeOrderManager", "GetExchangeDetailForVoucherCode", ex);
            }
            return exchangeDetail;
        }
        #endregion

        #region Code to manage voucher Test for pine labs

        /// <summary>
        /// Method to save voucher code detail in table
        /// </summary>
        /// <param name="voucherData">voucherData</param>
        /// <returns>int</returns>
        public int AddVoucherToDataBaseAPI(VoucherDataContract voucherData, tblExchangeOrder exchangeOrder, tblABBRedemption RedemptionOrder, tblABBRegistration registrationObj)
        {
            _sponserManager = new SponserManager();
            _voucherVerificationRepository = new VoucherVerificationRepository();
            _exchangeOrderRepository = new ExchangeOrderRepository();
            tblExchangeOrder exchangeObj = new tblExchangeOrder();
            _businessUnitRepository = new BusinessUnitRepository();
            _businessPartnerRepository = new BusinessPartnerRepository();
            _customerDetailsRepository = new CustomerDetailsRepository();
            _voucherStatusRepository = new VoucherStatusRepository();
            abbRedemptionRepository = new AbbRedemptionRepository();
            aBBRegistrationRepository = new ABBRegistrationRepository();
            _productTypeRepository = new ProductTypeRepository();
            _brandRepository = new BrandRepository();
            _productCategoryRepository = new ProductCategoryRepository();
            WhatasappResponse whatssappresponseDC = null;
            string responseforWhatasapp = string.Empty;
            _whatsAppMessageRepository = new WhatsappMessageRepository();
            int result = 0;
            string voucherStatusName = null;
            string WhatssAppStatusEnum = string.Empty;
            string ResponseCode = string.Empty;
            MailJetViewModel mailJet = new MailJetViewModel();
            MailJetMessage jetmessage = new MailJetMessage();
            MailJetFrom from = new MailJetFrom();
            MailjetTo to = new MailjetTo();
            string message = null;

            try
            {
                tblVoucherVerfication voucherVerfication = _voucherVerificationRepository.GetSingle(x => x.ExchangeOrderId == voucherData.ExchangeOrderId || x.RedemptionId == voucherData.RedemptionId && x.VoucherCode == voucherData.VoucherCode && x.IsActive == true);
                tblBusinessUnit businessUnit = _businessUnitRepository.GetSingle(x => x.BusinessUnitId == voucherData.BusinessUnitId);
                tblCustomerDetail customerDetail = _customerDetailsRepository.GetSingle(x => x.Id == voucherData.CustomerId);

                if (voucherVerfication == null)
                {
                    voucherVerfication = GenericMapper<VoucherDataContract, tblVoucherVerfication>.MapObject(voucherData);
                    voucherVerfication.IsVoucherused = false;
                    voucherVerfication.IsActive = true;
                    voucherVerfication.CreatedDate = DateTime.Now.TrimMilliseconds();
                    voucherVerfication.NewProductTypeId = voucherData.NewProductCategoryTypeId;
                    voucherVerfication.InvoiceNumber = voucherData.InvoiceNumber;
                    //voucherVerfication.SerialNumber = voucherData.SerialNumber;
                    _voucherVerificationRepository.Add(voucherVerfication);
                    _voucherVerificationRepository.SaveChanges();
                    result = voucherVerfication.VoucherVerficationId;

                    voucherStatusName = "Captured";
                    tblVoucherStatu tblVoucherStatu = _voucherStatusRepository.GetSingle(x => x.VoucherStatusName == voucherStatusName);
                    voucherVerfication = new tblVoucherVerfication();
                    _voucherVerificationRepository = new VoucherVerificationRepository();
                    voucherVerfication = _voucherVerificationRepository.GetSingle(x => x.VoucherVerficationId == result);
                    if (exchangeOrder != null)
                    {
                        voucherVerfication.ExchangePrice = Convert.ToDecimal(exchangeOrder.ExchangePrice);
                        if (exchangeOrder.IsDtoC == true)
                        {
                            voucherVerfication.Sweetneer = Convert.ToDecimal(businessUnit.SweetnerForDTC);
                        }
                        else
                        {
                            voucherVerfication.Sweetneer = Convert.ToDecimal(businessUnit.SweetnerForDTC);
                        }
                    }
                    else if (RedemptionOrder != null)
                    {
                        voucherVerfication.ExchangePrice = RedemptionOrder.RedemptionValue;
                        voucherVerfication.Sweetneer = 0;
                    }

                    voucherVerfication.IsVoucherused = true;
                    voucherVerfication.VoucherStatusId = tblVoucherStatu.VoucherStatusId;
                    _voucherVerificationRepository.Update(voucherVerfication);
                    _voucherVerificationRepository.SaveChanges();
                    result = voucherVerfication.VoucherVerficationId;

                    if (exchangeOrder != null)
                    {
                        exchangeOrder.IsVoucherused = true;
                        exchangeOrder.VoucherStatusId = tblVoucherStatu.VoucherStatusId;
                        exchangeOrder.BusinessPartnerId = voucherVerfication.BusinessPartnerId;
                        exchangeOrder.SerialNumber = voucherData.SerialNumber;
                        _exchangeOrderRepository.Update(exchangeOrder);
                        _exchangeOrderRepository.SaveChanges();

                        #region Send Order Confirmation on redemption
                        string ERPBaseURL = ConfigurationManager.AppSettings["ERPBaseURL"].ToString();
                        string selfQC = ConfigurationManager.AppSettings["SelfQCUrl"].ToString();
                        string selfqcurl = ERPBaseURL + "" + selfQC + "" + exchangeOrder.RegdNo;
                        tblProductType productType = _productTypeRepository.GetSingle(x => x.Id == exchangeOrder.ProductTypeId);
                        if (productType != null)
                        {
                            tblProductCategory productCategory = _productCategoryRepository.GetSingle(x => x.Id == productType.ProductCatId);
                            if (productCategory != null)
                            {
                                tblBrand brandobj = _brandRepository.GetSingle(x => x.Id == exchangeOrder.BrandId);
                                if (brandobj != null)
                                {
                                    OrderConfirmationTemplateExchange whatsappObjforOrderConfirmation = new OrderConfirmationTemplateExchange();
                                    whatsappObjforOrderConfirmation.userDetails = new UserDetails();
                                    whatsappObjforOrderConfirmation.notification = new OrderConfiirmationNotification();
                                    whatsappObjforOrderConfirmation.notification.@params = new SendWhatssappForExcahangeConfirmation();
                                    whatsappObjforOrderConfirmation.userDetails.number = customerDetail.PhoneNumber;
                                    whatsappObjforOrderConfirmation.notification.sender = ConfigurationManager.AppSettings["Yello.aiSenderNumber"].ToString();
                                    whatsappObjforOrderConfirmation.notification.type = ConfigurationManager.AppSettings["Yellow.aiMesssaheType"].ToString();
                                    whatsappObjforOrderConfirmation.notification.templateId = NotificationConstants.orderConfirmationForExchange;
                                    whatsappObjforOrderConfirmation.notification.@params.CustName = customerDetail.FirstName + " " + customerDetail.LastName;
                                    whatsappObjforOrderConfirmation.notification.@params.Link = selfqcurl;
                                    whatsappObjforOrderConfirmation.notification.@params.ProductBrand = brandobj.Name;
                                    whatsappObjforOrderConfirmation.notification.@params.ProdCategory = productCategory.Description;
                                    whatsappObjforOrderConfirmation.notification.@params.ProdType = productType.Description;
                                    whatsappObjforOrderConfirmation.notification.@params.RegdNO = exchangeOrder.RegdNo.ToString();
                                    string urlforwhatsapp = ConfigurationManager.AppSettings["Yellow.AiUrl"].ToString();
                                    IRestResponse responseConfirmation = WhatsappNotificationManager.Rest_InvokeWhatsappserviceCall(urlforwhatsapp, Method.POST, whatsappObjforOrderConfirmation);
                                    ResponseCode = responseConfirmation.StatusCode.ToString();
                                    WhatssAppStatusEnum = ExchangeOrderManager.GetEnumDescription(WhatssAppEnum.SuccessCode);
                                    if (ResponseCode == WhatssAppStatusEnum)
                                    {
                                        responseforWhatasapp = responseConfirmation.Content;
                                        if (responseforWhatasapp != null)
                                        {
                                            whatssappresponseDC = JsonConvert.DeserializeObject<WhatasappResponse>(responseforWhatasapp);
                                            tblWhatsAppMessage whatsapObj = new tblWhatsAppMessage();
                                            whatsapObj.TemplateName = NotificationConstants.orderConfirmationForExchange;
                                            whatsapObj.IsActive = true;
                                            whatsapObj.PhoneNumber = customerDetail.PhoneNumber;
                                            whatsapObj.SendDate = DateTime.Now;
                                            whatsapObj.msgId = whatssappresponseDC.msgId;
                                            _whatsAppMessageRepository.Add(whatsapObj);
                                            _whatsAppMessageRepository.SaveChanges();
                                        }
                                        else
                                        {
                                            string JsonObjectForExchangeOrder = JsonConvert.SerializeObject(exchangeOrder);
                                            logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", exchangeOrder.SponsorOrderNumber, JsonObjectForExchangeOrder);
                                        }
                                    }
                                    else
                                    {
                                        string JsonObjectForExchangeOrder = JsonConvert.SerializeObject(exchangeOrder);
                                        logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", exchangeOrder.SponsorOrderNumber, JsonObjectForExchangeOrder);
                                    }

                                    #region Code to send mail to customer for exchange details
                                    jetmessage.From = new MailJetFrom() { Email = "customercare@utcdigital.com", Name = "UTC - Customer  Care" };
                                    jetmessage.To = new List<MailjetTo>();
                                    jetmessage.To.Add(new MailjetTo() { Email = customerDetail.Email.Trim(), Name = customerDetail.FirstName });
                                    jetmessage.Subject = businessUnit.Name + ": Exchange Detail";
                                    message = EmailTemplateConstants.EmailForInstant();
                                    string TemplaTePath = ConfigurationManager.AppSettings["InstantEmail"].ToString();
                                    string FilePath = TemplaTePath;
                                    StreamReader str = new StreamReader(FilePath);
                                    string MailText = str.ReadToEnd();
                                    str.Close();
                                    MailText = MailText.Replace("[CustomerName]", customerDetail.FirstName).Replace("[BusinessUnitName]", exchangeOrder.CompanyName).Replace("[SponserOrderNumber]", exchangeOrder.SponsorOrderNumber).Replace("[CreatedDate]", Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy")).Replace("[CustName]", customerDetail.FirstName).Replace("[CustMobile]", customerDetail.PhoneNumber).Replace("[CustAdd1]", customerDetail.Address1)
                                        .Replace("[CustAdd2]", customerDetail.Address2).Replace("[State]", customerDetail.State).Replace("[PinCode]", customerDetail.ZipCode).Replace("[CustCity]", customerDetail.City).Replace("[ProductCategory]", productCategory.Description)
                                        .Replace("[OldProdType]", productType.Description).Replace("[OldBrand]", brandobj.Name).Replace("[Size]", productType.Size).Replace("[ExchangePrice]", exchangeOrder.ExchangePrice.ToString()).Replace("[EstimatedDeliveryDate]", exchangeOrder.EstimatedDeliveryDate).Replace("[SelfQCLink]", selfqcurl);
                                    jetmessage.HTMLPart = MailText;
                                    mailJet.Messages = new List<MailJetMessage>();
                                    mailJet.Messages.Add(jetmessage);
                                    BillCloudServiceCall.MailJetSendMailService(mailJet);
                                    #endregion
                                }
                            }
                        }

                        #endregion
                    }
                    else if (RedemptionOrder != null)
                    {
                        RedemptionOrder.IsVoucherUsed = true;
                        RedemptionOrder.VoucherStatusId = tblVoucherStatu.VoucherStatusId;
                        abbRedemptionRepository.Update(RedemptionOrder);
                        abbRedemptionRepository.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeOrderManager", "AddVouchertoDB", ex);
            }

            return result;
        }
        #endregion

        #region
        public bool NewProductCategoryDetails(int newcatid, int ExchangeOrderId)
        {
            _exchangeOrderRepository = new ExchangeOrderRepository();
            bool flag = false;
            try
            {

                if (newcatid > 0 && ExchangeOrderId > 0)
                {
                    tblExchangeOrder ExchangeObj = _exchangeOrderRepository.GetSingle(x => x.Id == ExchangeOrderId && x.IsActive == true);
                    if (ExchangeObj != null && ExchangeObj.NewProductCategoryId == newcatid)
                    {
                        flag = true;
                    }
                    else
                    {
                        flag = false;
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeOrderManager", "NewProductCategoryDetails", ex);
            }
            return flag;
        }
        #endregion


        #region Genereate Voucher
        /// <summary>
        /// Method to generate the voucher code
        /// </summary>
        /// <param name="buCode">business unit code</param>
        /// <returns>string</returns>
        public ExchangeOrderDataContract UpdateVoucher(ExchangeOrderDataContract exchangeOrderDC)
        {
            ExchangeOrderDataContract ExchangeObj = new ExchangeOrderDataContract();
            _exchangeOrderRepository = new ExchangeOrderRepository();
            _modelNumberrepository = new ModelNumberRepository();
            _customerDetailsRepository = new CustomerDetailsRepository();
            _businessUnitRepository = new BusinessUnitRepository();
            _historyRepository = new HistoryRepository();
            _voucherStatusRepository = new VoucherStatusRepository();
            DateTime currentDatetime = DateTime.Now.TrimMilliseconds();
            string voucherStatusName = "Generated";
            string SMSFlag = null;
            string code = null;

            try
            {
                SMSFlag = ConfigurationManager.AppSettings["sendsmsflag"].ToString();
                //if (!string.IsNullOrEmpty(buCode))
                //{
                //    code = "vcr-" + buCode + "-" + UniqueString.RandomNumberByLength(8);
                //}
                if (exchangeOrderDC != null)
                {
                    tblExchangeOrder exchangeOrder = _exchangeOrderRepository.GetSingle(x => x.Id == exchangeOrderDC.Id && x.IsActive == true);
                    if (exchangeOrder != null)
                    {
                        if (exchangeOrderDC.BusinessUnitId > 0)
                        {
                            tblBusinessUnit businessUnitObj = _businessUnitRepository.GetSingle(x => x.BusinessUnitId == exchangeOrderDC.BusinessUnitId && x.IsActive == true);
                            if (businessUnitObj != null)
                            {
                                tblHistory historyObj = new tblHistory();
                                historyObj.CustId = exchangeOrder.CustomerDetailsId;
                                historyObj.ExchangeOrderId = exchangeOrder.Id;
                                historyObj.ExchangeAmount = exchangeOrder.ExchangePrice;
                                historyObj.RegdNo = exchangeOrder.RegdNo;
                                historyObj.VoucherCode = exchangeOrder.VoucherCode;
                                historyObj.Sweetner = exchangeOrder.Sweetener;
                                historyObj.createdate = currentDatetime;
                                historyObj.IsActive = true;
                                _historyRepository.Add(historyObj);
                                _historyRepository.SaveChanges();


                                exchangeOrder.ExchangePrice = exchangeOrderDC.ExchangePrice;
                                exchangeOrder.NewProductCategoryId = exchangeOrderDC.NewProductCategoryId;
                                exchangeOrder.NewProductTypeId = exchangeOrderDC.NewProductCategoryTypeId;
                                exchangeOrder.ModelNumberId = exchangeOrderDC.ModelNumberId;
                                tblVoucherStatu voucherStatu = _voucherStatusRepository.GetSingle(x => x.VoucherStatusName == voucherStatusName);
                                exchangeOrder.VoucherCodeExpDate = DateTime.Now.AddHours(Convert.ToDouble(businessUnitObj.VoucherExpiryTime));
                                exchangeOrder.IsVoucherused = false;
                                exchangeOrder.VoucherStatusId = voucherStatu.VoucherStatusId;
                                exchangeOrder.IsActive = true;

                                // ExchangeOrder data
                                ExchangeObj.ExchangePrice = (decimal)exchangeOrder.ExchangePrice;


                                string unique = "_" + exchangeOrderDC.NewProductCategoryId + "_" + exchangeOrderDC.NewProductCategoryTypeId + "_" + currentDatetime;
                                #region Bill Cloud API call for generating the voucher code
                                string billcloudcall = ConfigurationManager.AppSettings["BillcloudCallActive"].ToString();
                                if (billcloudcall == "true")
                                {
                                    VoucherSucessResponseModel sucessObj = new VoucherSucessResponseModel();
                                    GenerateVoucherViewModel generateVoucherVM = new GenerateVoucherViewModel();
                                    generateVoucherVM.data = new RDCEL.DocUpload.DataContract.BillCloud.GenerateVoucherData();
                                    generateVoucherVM.data.event_id = "ISSUE_VOUCHER";
                                    generateVoucherVM.data.rrn = exchangeOrder.SponsorOrderNumber + unique;
                                    generateVoucherVM.data.dao_name = businessUnitObj != null ? businessUnitObj.Name.Trim() : string.Empty;
                                    generateVoucherVM.data.payload = new GenerateVoucherPayload();
                                    generateVoucherVM.data.payload.service_id = "EXCHANGE";
                                    generateVoucherVM.data.payload.amount = exchangeOrder.ExchangePrice.ToString();
                                    generateVoucherVM.data.payload.expiry = exchangeOrder.VoucherCodeExpDate != null ? Convert.ToDateTime(exchangeOrder.VoucherCodeExpDate).ToString("MM/dd/yyyy hh:mm:ss") : string.Empty;
                                    generateVoucherVM.data.payload.beneficiary_ref_id = exchangeOrder.CustomerDetailsId.ToString();
                                    generateVoucherVM.data.payload.consumer_ref_id = exchangeOrder.CustomerDetailsId.ToString();
                                    generateVoucherVM.data.payload.issuer_ref_id = businessUnitObj.BusinessUnitId.ToString();
                                    generateVoucherVM.data.payload.brand_ref_id = businessUnitObj.BusinessUnitId.ToString();
                                    generateVoucherVM.data.payload.merchant_ref_id = "UTCDIGITAL";
                                    IRestResponse response = BillCloudServiceCall.Rest_InvokeZohoInvoiceServiceForPlainText(ConfigurationManager.AppSettings["VoucherProcess"].ToString(), Method.POST, generateVoucherVM);

                                    if (response != null)
                                    {
                                        if (response.StatusCode == HttpStatusCode.OK)
                                        {
                                            sucessObj = JsonConvert.DeserializeObject<VoucherSucessResponseModel>(response.Content);
                                            if (sucessObj != null && sucessObj.data != null && sucessObj.data.status.ToLower().Equals("success"))
                                            {
                                                if (sucessObj.data.context != null && sucessObj.data.context.EVENT != null && sucessObj.data.voucher_id != null)
                                                {
                                                    code = sucessObj.data.voucher_id;
                                                    exchangeOrder.VoucherCode = code;
                                                    ExchangeObj.VoucherCode = code;
                                                    _exchangeOrderRepository.Update(exchangeOrder);
                                                    _exchangeOrderRepository.SaveChanges();
                                                }
                                                //Code send SMS to cusrtomer about the Voucher code and expire time
                                                tblCustomerDetail customerObj = _customerDetailsRepository.GetSingle(x => x.Id == exchangeOrder.CustomerDetailsId && x.IsActive == true);
                                                if (customerObj != null)
                                                {
                                                    string voucherUrl = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Home/V/" + exchangeOrder.Id;
                                                    if (SMSFlag == "true")
                                                    {
                                                        _notificationManager = new NotificationManager();
                                                        string message = NotificationConstants.SMS_VoucherRedemption_Confirmation.Replace("[ExchPrice]", Convert.ToInt32(exchangeOrder.ExchangePrice).ToString()).Replace("[VCODE]", exchangeOrder.VoucherCode)
                                                            .Replace("[VLink]", "( " + voucherUrl + " )").Replace("[STORENAME]", businessUnitObj.Name).Replace("[COMPANY]", businessUnitObj.Name).Replace("[VALIDTILLDATE]", Convert.ToDateTime(exchangeOrder.VoucherCodeExpDate).ToString("dd/MM/yyyy"));
                                                        _notificationManager.SendNotificationSMS(customerObj.PhoneNumber, message, null);
                                                    }
                                                    #region Code to send notification to customer for voucher generation


                                                    MailJetViewModel mailJet = new MailJetViewModel();
                                                    MailJetMessage jetmessage = new MailJetMessage();
                                                    MailJetFrom from = new MailJetFrom();
                                                    MailjetTo to = new MailjetTo();
                                                    jetmessage.From = new MailJetFrom() { Email = "customercare@utcdigital.com", Name = "UTC - Customer  Care" };
                                                    jetmessage.To = new List<MailjetTo>();
                                                    jetmessage.To.Add(new MailjetTo() { Email = customerObj.Email.Trim(), Name = customerObj.FirstName });
                                                    jetmessage.Subject = businessUnitObj.Name + ": Exchange Voucher Detail";
                                                    string TemplaTePath = ConfigurationManager.AppSettings["VoucherGenerationInstant"].ToString();
                                                    string FilePath = TemplaTePath;
                                                    StreamReader str = new StreamReader(FilePath);
                                                    string MailText = str.ReadToEnd();
                                                    str.Close();
                                                    MailText = MailText.Replace("[ExchPrice]", exchangeOrder.ExchangePrice.ToString()).Replace("[VCode]", exchangeOrder.VoucherCode).Replace("[FirstName]", customerObj.FirstName)
                                                        .Replace("[VLink]", voucherUrl).Replace("[STORENAME]", businessUnitObj.Name).Replace("[VALIDTILLDATE]", "7 days from quality check of your appliance");
                                                    jetmessage.HTMLPart = MailText;
                                                    mailJet.Messages = new List<MailJetMessage>();
                                                    mailJet.Messages.Add(jetmessage);
                                                    BillCloudServiceCall.MailJetSendMailService(mailJet);
                                                    #endregion
                                                }

                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    code = "V" + UniqueString.RandomNumberByLength(8);

                                    exchangeOrder.VoucherCode = code;
                                    ExchangeObj.VoucherCode = code;
                                    _exchangeOrderRepository.Update(exchangeOrder);
                                    _exchangeOrderRepository.SaveChanges();


                                    //Code send SMS to cusrtomer about the Voucher code and expire time
                                    tblCustomerDetail customerObj = _customerDetailsRepository.GetSingle(x => x.Id == exchangeOrder.CustomerDetailsId && x.IsActive == true);
                                    if (customerObj != null)
                                    {
                                        #region Code to send notification to customer for voucher generation
                                        _notificationManager = new NotificationManager();
                                        string voucherUrl = ConfigurationManager.AppSettings["BaseURL"].ToString() + "Home/V/" + exchangeOrder.Id;
                                        if (SMSFlag == "true")
                                        {
                                            string message = NotificationConstants.SMS_VoucherRedemption_Confirmation.Replace("[ExchPrice]", Convert.ToInt32(exchangeOrder.ExchangePrice).ToString()).Replace("[VCODE]", exchangeOrder.VoucherCode)
                                            .Replace("[VLink]", "( " + voucherUrl + " )").Replace("[STORENAME]", businessUnitObj.Name).Replace("[COMPANY]", businessUnitObj.Name).Replace("[VALIDTILLDATE]", Convert.ToDateTime(exchangeOrder.VoucherCodeExpDate).ToString("dd/MM/yyyy"));
                                            _notificationManager.SendNotificationSMS(customerObj.PhoneNumber, message, null);
                                        }
                                        MailJetViewModel mailJet = new MailJetViewModel();
                                        MailJetMessage jetmessage = new MailJetMessage();
                                        MailJetFrom from = new MailJetFrom();
                                        MailjetTo to = new MailjetTo();
                                        jetmessage.From = new MailJetFrom() { Email = "customercare@utcdigital.com", Name = "UTC - Customer  Care" };
                                        jetmessage.To = new List<MailjetTo>();
                                        jetmessage.To.Add(new MailjetTo() { Email = customerObj.Email.Trim(), Name = customerObj.FirstName });
                                        jetmessage.Subject = businessUnitObj.Name + ": Exchange Voucher Detail";
                                        string TemplaTePath = ConfigurationManager.AppSettings["VoucherGenerationInstant"].ToString();
                                        string FilePath = TemplaTePath;
                                        StreamReader str = new StreamReader(FilePath);
                                        string MailText = str.ReadToEnd();
                                        str.Close();
                                        MailText = MailText.Replace("[ExchPrice]", exchangeOrder.ExchangePrice.ToString()).Replace("[VCode]", exchangeOrder.VoucherCode).Replace("[FirstName]", customerObj.FirstName)
                                            .Replace("[VLink]", voucherUrl).Replace("[STORENAME]", businessUnitObj.Name).Replace("[VALIDTILLDATE]", "7 days from quality check of your appliance");
                                        jetmessage.HTMLPart = MailText;
                                        mailJet.Messages = new List<MailJetMessage>();
                                        mailJet.Messages.Add(jetmessage);
                                        BillCloudServiceCall.MailJetSendMailService(mailJet);
                                        #endregion
                                    }

                                    #endregion
                                }
                            }
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeOrderManager", "GenerateVoucher", ex);
            }

            return ExchangeObj;
        }
        #endregion

        #region
        public bool CancelOrder(int ExchangeOrderId)
        {
            _exchangeOrderRepository = new ExchangeOrderRepository();
            bool success = false;
            try
            {
                if (ExchangeOrderId > 0)
                {
                    tblExchangeOrder exchangeorder = _exchangeOrderRepository.GetSingle(x => x.Id == ExchangeOrderId && x.IsActive == true);
                    if (exchangeorder != null)
                    {
                        exchangeorder.IsActive = false;
                        exchangeorder.VoucherCode = null;
                        exchangeorder.StatusId = 6;
                        success = true;
                        _exchangeOrderRepository.Update(exchangeorder);
                        _exchangeOrderRepository.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeOrderManager", "GenerateVoucher", ex);
            }
            return success;
        }
        #endregion

        #region to get String Value Of Enum
        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attributes != null && attributes.Any())
            {
                return attributes.First().Description;
            }

            return value.ToString();
        }
        #endregion

        #region Method to get the AvailableTimeSLot
        /// <summary>
        /// Method to get AvailableTimeSLot as per selecteddate
        /// </summary>
        /// <param name="dateToday"></param>
        /// <returns></returns>
        public List<timeSlotDataContract> AvailableTimeSLot(string selectedDate)
        {
            List<timeSlotDataContract> timeSlotDataContracts = new List<timeSlotDataContract>();
            //List<timeSlotDataContract> AllSlot = new List<timeSlotDataContract>();
            List<timeSlotDataContract> AllSlot = null;
            List<timeSlotDataContract> timeSlotDataContracts1 = new List<timeSlotDataContract>();
            List<ExchangeOrderDataContract> exchangeOrderDC = null;
            _configurationRepository = new ConfigurationRepository();
            _exchangeOrderRepository = new ExchangeOrderRepository();
            _productTypeRepository = new ProductTypeRepository();
            _customerDetailsRepository = new CustomerDetailsRepository();
            _productCategoryRepository = new ProductCategoryRepository();
            _timeSlotMasterRepository = new TimeSlotMasterRepository();
            try
            {
                //tblExchangeOrder exchangeObj = null;
                List<tblExchangeOrder> tblExchangeOrderslist = _exchangeOrderRepository.GetList(x => x.IsActive == true && x.QCDate != null && x.QCDate == selectedDate && x.StartTime != null && x.EndTime != null).ToList();
                if (tblExchangeOrderslist != null)
                {
                    exchangeOrderDC = GenericMapper<tblExchangeOrder, ExchangeOrderDataContract>.MapList(tblExchangeOrderslist);

                }
                if (exchangeOrderDC != null)
                {
                    var groupBycount = exchangeOrderDC
                        .GroupBy(x => x.StartTime).Select(g => new
                        {
                            StartTime = g.Select(x => x.StartTime).FirstOrDefault(),
                            Endtime = g.Select(x => x.EndTime).FirstOrDefault(),
                            OrderList = g.ToList()
                        });
                    List<TimeSlotMaster> timeSlotMaster = _timeSlotMasterRepository.GetList(x => x.IsActive == true).ToList();
                    tblConfiguration configAgents = _configurationRepository.GetSingle(x => x.IsActive == true && x.ConfigId == Convert.ToInt32(TimeSlotConfigEnum.TotalAgents));
                    tblConfiguration configCalls = _configurationRepository.GetSingle(x => x.IsActive == true && x.ConfigId == Convert.ToInt32(TimeSlotConfigEnum.CallPerHour));
                    int availableagents = Convert.ToInt32(configAgents.Value);
                    int callperhour = Convert.ToInt32(configCalls.Value);
                    int slotHour = Convert.ToInt32(timeSlotMaster[0].SlotHour);
                    int totatlcall = (availableagents * callperhour) * slotHour;
                    if (groupBycount != null && timeSlotMaster != null && timeSlotMaster.Count > 0)
                    {
                        AllSlot = new List<timeSlotDataContract>();
                        foreach (var item in timeSlotMaster)
                        {
                            timeSlotDataContract timeSlotDataContract = new timeSlotDataContract();
                            timeSlotDataContract.startTime = item.StartTime.ToString();
                            timeSlotDataContract.endTime = item.EndTime.ToString();
                            AllSlot.Add(timeSlotDataContract);
                        }
                        foreach (var item in groupBycount)
                        {
                            foreach (var slot in timeSlotMaster)
                            {
                                DateTime a = Convert.ToDateTime(item.StartTime);
                                timeSlotDataContract timeSlotDataContract = new timeSlotDataContract();
                                if (slot.StartTime == a.TimeOfDay)
                                {
                                    timeSlotDataContract.startTime = slot.StartTime.ToString();
                                    timeSlotDataContract.endTime = slot.EndTime.ToString();
                                    if (Convert.ToInt32(item.OrderList.Count) > totatlcall)
                                    {
                                        AllSlot.RemoveAll(x => x.startTime == slot.StartTime.ToString());

                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeOrderManager", "AvailableTimeSLot", ex);
            }
            return AllSlot;
        }
        #endregion

        #region Get all PinCode detail for abb
        /// <summary>
        /// Method to get the list of PinCode data contract
        /// </summary>       
        /// <returns>List PinCodeDataContract</returns>   
        public PinCodeDataContract GetZipCodesABB(int buid)
        {
            _pinCodeRepository = new PinCodeRepository();
            PinCodeDataContract pinCodeDC = new PinCodeDataContract();
            try
            {
                DataTable dt = _pinCodeRepository.GetABBPincodeListbybuid(buid);

                List<tblPinCode> pincodeMasterList = GenericConversionHelper.DataTableToList<tblPinCode>(dt);
                if (pincodeMasterList != null && pincodeMasterList.Count > 0)
                {
                    List<ZipCodes> zipCodesList = pincodeMasterList.Select(x => new ZipCodes
                    {
                        ZipCode = x.ZipCode.ToString(),
                        Id = x.Id  // Assuming x.Id is the property representing the Id of the PinCode
                    }).ToList();
                    pinCodeDC.ZipCodes = zipCodesList.OrderBy(o => o.ZipCode).ToList();
                }
                else
                {
                    pinCodeDC.ZipCodes = new List<ZipCodes> { new ZipCodes { ZipCode = "No pincode available on this location" } };
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("SamsungManager", "GetZipCodes", ex);
                // You might want to handle the exception accordingly
            }
            return pinCodeDC;
        }

        #endregion

        #region  get new brand
        public NewBrandDataContract getNewBrandsForexchnge(GetNewbrandDC details)
        {
            _orderbasedRepository = new OrderBasedConfigurationRepository();
            _businessPartnerRepository = new BusinessPartnerRepository();
            _brandRepository = new BrandRepository();
            NewBrandDataContract dataDC = new NewBrandDataContract();
            tblOrderBasedConfig orderbasedObj = null;
            List<tblBrand> BrandObj = new List<tblBrand>();
            List<NewBrandList> newBrandlist = new List<NewBrandList>();
            try
            {
                orderbasedObj = _orderbasedRepository.GetSingle(x => x.IsActive == true && x.BusinessUnitId == details.BusinessUnitId && x.BusinessPartnerId == details.BusinessPartnerId);
                if (orderbasedObj != null)
                {
                    if (orderbasedObj.IsBPMultiBrand == true)
                    {
                        BrandObj = _brandRepository.GetList(x => x.IsActive == true).ToList();
                        if (BrandObj != null && BrandObj.Count > 0)
                        {
                            newBrandlist = GenericMapper<tblBrand, NewBrandList>.MapList(BrandObj);
                            dataDC.brandlist = newBrandlist;
                        }
                        else
                        {
                            dataDC.Errormessage = "No Brand found";
                        }
                    }
                    else
                    {
                        BrandObj = _brandRepository.GetList(x => x.IsActive == true && x.BusinessUnitId == details.BusinessUnitId).ToList();
                        if (BrandObj != null && BrandObj.Count > 0)
                        {
                            newBrandlist = GenericMapper<tblBrand, NewBrandList>.MapList(BrandObj);
                            dataDC.brandlist = newBrandlist;
                        }
                        else
                        {
                            dataDC.Errormessage = "No Brand found";
                        }
                    }
                }
                else
                {
                    dataDC.Errormessage = "No data in order configuration table";
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeOrderManager", "getNewBrandsForexchnge", ex);
            }
            return dataDC;
        }
        #endregion

        #region Get Price for independent sweetner BusinessUnit
        public List<ProductsPricesDataContractSamsung> GetProductPriceSamsung(string uname, bool? IsSweetenerModelBased, int? PriceMasterNameId, string categoryname = "", string catid = "", string typeid = "", string brandid = "", int? NewBrandId = 0, int? NewCatId = 0, int? NewTypeId = 0, int? BusinessUnitId = 0, int? BusinessPartnerId = 0, int? NewModelId = 0)
        {
            _productPriceRepository = new PriceMasterRepository();
            _universalPriceMasterRepository = new UniversalPriceMasterRepository();
            _brandRepository = new BrandRepository();
            _productTypeRepository = new ProductTypeRepository();
            _productCategoryRepository = new ProductCategoryRepository();
            _managerSweetener = new ManageSweetener();
            List<tblPriceMaster> tblProductPriceObjList = new List<tblPriceMaster>();
            List<tblUniversalPriceMaster> tblUniversalPriceMasterObjList = new List<tblUniversalPriceMaster>();
            ProductsPricesDataContractSamsung productsPricesDC = null;
            List<ProductsPricesDataContractSamsung> productsPricesList = new List<ProductsPricesDataContractSamsung>();
            List<ProductsFromTypePriceList> productsTyepeList = new List<ProductsFromTypePriceList>();
            List<BrandPriceList> brandList = null;
            GetSweetenerDetailsDataContract detailsforSweetenerDc = new GetSweetenerDetailsDataContract();
            SweetenerDataContract sweetener = new SweetenerDataContract();
            string priceCode = string.Empty;
            try
            {


                //Get the brand list 
                List<tblBrand> masterbrandList = _brandRepository.GetList(x => x.IsActive == true).ToList();
                List<tblProductType> masterCategoryTypeList = _productTypeRepository.GetList(x => x.IsActive == true).ToList();

                detailsforSweetenerDc.BrandId = NewBrandId;
                detailsforSweetenerDc.BusinessPartnerId = BusinessPartnerId;
                detailsforSweetenerDc.BusinessUnitId = BusinessUnitId;
                detailsforSweetenerDc.NewProdCatId = NewCatId;
                detailsforSweetenerDc.NewProdTypeId = NewTypeId;
                detailsforSweetenerDc.IsSweetenerModalBased = IsSweetenerModelBased;
                detailsforSweetenerDc.ModalId = NewModelId;
                sweetener = _managerSweetener.GetSweetenerAmtExchange(detailsforSweetenerDc);
                //Get the Product price list
                if (!string.IsNullOrEmpty(categoryname))
                {
                    #region For Category Name

                    tblUniversalPriceMasterObjList = _universalPriceMasterRepository.GetList(x => x.IsActive == true
                    && (x.PriceMasterNameId == PriceMasterNameId)
                    && (x.ProductCategoryName != null && x.ProductCategoryName.ToLower().Equals(categoryname.ToLower()))).ToList();
                    if (tblUniversalPriceMasterObjList != null && tblUniversalPriceMasterObjList.Count > 0)
                    {

                        foreach (var item in tblUniversalPriceMasterObjList)
                        {
                            ProductsFromTypePriceList productsType = new ProductsFromTypePriceList();
                            //Code to check type

                            productsType.name = item.ProductTypeName.Replace(System.Environment.NewLine, string.Empty);
                            string size = masterCategoryTypeList.FirstOrDefault(x => x.Id.Equals(item.ProductTypeId)).Size;

                            if (size != null)
                                productsType.name = productsType.name + " " + "(" + size + ")";

                            productsType.ProducttypeId = (int)item.ProductTypeId;
                            productsType.id = item.PriceMasterUniversalId;

                            #region code to fill brand list

                            BrandPriceList brandPL = null;
                            brandList = new List<BrandPriceList>();
                            //For Brand 1
                            if (!string.IsNullOrEmpty(item.BrandName_1))
                            {

                                string BrandDescription = ExchangeOrderManager.GetEnumDescription(PriceMasterBrandEnum.Others);
                                if (item.BrandName_1 == BrandDescription)
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_1.ToLower())).Id;
                                    brandPL.Name = item.BrandName_1;
                                    brandPL.Price = item.Quote_P;
                                    brandPL.Mid_Price = item.Quote_Q;
                                    brandPL.Min_Price = item.Quote_R;
                                    brandPL.Scrap_Price = item.Quote_S;
                                    brandList.Add(brandPL);
                                }
                                else
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_1.ToLower())).Id;
                                    brandPL.Name = item.BrandName_1;
                                    brandPL.Price = item.Quote_P_High;
                                    brandPL.Mid_Price = item.Quote_Q_High;
                                    brandPL.Min_Price = item.Quote_R_High;
                                    brandPL.Scrap_Price = item.Quote_S_High;
                                    brandList.Add(brandPL);
                                }
                            }

                            //For Brand 2
                            if (!string.IsNullOrEmpty(item.BrandName_2))
                            {
                                string BrandDescription = ExchangeOrderManager.GetEnumDescription(PriceMasterBrandEnum.Others);
                                if (item.BrandName_2 == BrandDescription)
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_2.ToLower())).Id;
                                    brandPL.Name = item.BrandName_2;
                                    brandPL.Price = item.Quote_P;
                                    brandPL.Mid_Price = item.Quote_Q;
                                    brandPL.Min_Price = item.Quote_R;
                                    brandPL.Scrap_Price = item.Quote_S;
                                    brandList.Add(brandPL);
                                }
                                else
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_2.ToLower())).Id;
                                    brandPL.Name = item.BrandName_2;
                                    brandPL.Price = item.Quote_P_High;
                                    brandPL.Mid_Price = item.Quote_Q_High;
                                    brandPL.Min_Price = item.Quote_R_High;
                                    brandPL.Scrap_Price = item.Quote_S_High;
                                    brandList.Add(brandPL);
                                }

                            }

                            //For Brand 3
                            if (!string.IsNullOrEmpty(item.BrandName_3))
                            {
                                string BrandDescription = ExchangeOrderManager.GetEnumDescription(PriceMasterBrandEnum.Others);
                                if (item.BrandName_3 == BrandDescription)
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_3.ToLower())).Id;
                                    brandPL.Name = item.BrandName_3;
                                    brandPL.Price = item.Quote_P;
                                    brandPL.Mid_Price = item.Quote_Q;
                                    brandPL.Min_Price = item.Quote_R;
                                    brandPL.Scrap_Price = item.Quote_S;
                                    brandList.Add(brandPL);
                                }
                                else
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_3.ToLower())).Id;
                                    brandPL.Name = item.BrandName_3;
                                    brandPL.Price = item.Quote_P_High;
                                    brandPL.Mid_Price = item.Quote_Q_High;
                                    brandPL.Min_Price = item.Quote_R_High;
                                    brandPL.Scrap_Price = item.Quote_S_High;
                                    brandList.Add(brandPL);
                                }

                            }

                            //For Brand 4
                            if (!string.IsNullOrEmpty(item.BrandName_4))
                            {
                                string BrandDescription = ExchangeOrderManager.GetEnumDescription(PriceMasterBrandEnum.Others);
                                if (item.BrandName_4 == BrandDescription)
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_4.ToLower())).Id;
                                    brandPL.Name = item.BrandName_4;
                                    brandPL.Price = item.Quote_P;
                                    brandPL.Mid_Price = item.Quote_Q;
                                    brandPL.Min_Price = item.Quote_R;
                                    brandPL.Scrap_Price = item.Quote_S;
                                    brandList.Add(brandPL);
                                }
                                else
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_4.ToLower())).Id;
                                    brandPL.Name = item.BrandName_4;
                                    brandPL.Price = item.Quote_P_High;
                                    brandPL.Mid_Price = item.Quote_Q_High;
                                    brandPL.Min_Price = item.Quote_R_High;
                                    brandPL.Scrap_Price = item.Quote_S_High;
                                    brandList.Add(brandPL);
                                }
                            }

                            #endregion

                            productsType.Brand = brandList;
                            productsTyepeList.Add(productsType);


                        }
                        productsPricesDC = new ProductsPricesDataContractSamsung();
                        productsPricesDC.ProductsFromType = productsTyepeList;

                        productsPricesDC.CategoryId = (int)tblUniversalPriceMasterObjList[0].ProductCategoryId;
                        productsPricesDC.categoryName = tblUniversalPriceMasterObjList[0].ProductCategoryName;

                    }
                    productsPricesList = new List<ProductsPricesDataContractSamsung>();
                    productsPricesList.Add(productsPricesDC);
                    #endregion
                }
                else if (!string.IsNullOrEmpty(catid) || !string.IsNullOrEmpty(typeid) || !string.IsNullOrEmpty(brandid))
                {
                    #region For Category id type id
                    tblUniversalPriceMasterObjList = _universalPriceMasterRepository.GetList(x => x.IsActive == true
                    && (x.PriceMasterNameId != null && x.PriceMasterNameId.Equals(PriceMasterNameId))
                    && (x.ProductCategoryId != null && (string.IsNullOrEmpty(catid) || x.ProductCategoryId == Convert.ToInt32(catid)))
                    && (x.ProductTypeId != null && (string.IsNullOrEmpty(typeid) || x.ProductTypeId == Convert.ToInt32(typeid)))
                    ).ToList();
                    if (tblUniversalPriceMasterObjList != null && tblUniversalPriceMasterObjList.Count > 0)
                    {

                        foreach (var item in tblUniversalPriceMasterObjList)
                        {

                            ProductsFromTypePriceList productsType = new ProductsFromTypePriceList();
                            //Code to check type

                            productsType.name = item.ProductTypeName.Replace(System.Environment.NewLine, string.Empty);

                            //new code
                            tblProductType producttypeObj = new tblProductType();
                            string size = masterCategoryTypeList.FirstOrDefault(x => x.Id.Equals(item.ProductTypeId) && x.IsActive == true).Size;

                            if (size != null)
                                productsType.name = productsType.name + " " + "(" + size + ")";


                            productsType.ProducttypeId = (int)item.ProductTypeId;
                            productsType.id = item.PriceMasterUniversalId;

                            #region code to fill brand list

                            BrandPriceList brandPL = null;
                            brandList = new List<BrandPriceList>();
                            //For Brand 1
                            if (!string.IsNullOrEmpty(item.BrandName_1))
                            {

                                string BrandDescription = ExchangeOrderManager.GetEnumDescription(PriceMasterBrandEnum.Others);
                                if (item.BrandName_1 == BrandDescription)
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_1.ToLower())).Id;
                                    brandPL.Name = item.BrandName_1;
                                    brandPL.Price = item.Quote_P;
                                    brandPL.Mid_Price = item.Quote_Q;
                                    brandPL.Min_Price = item.Quote_R;
                                    brandPL.Scrap_Price = item.Quote_S;
                                    brandList.Add(brandPL);
                                }
                                else
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_1.ToLower())).Id;
                                    brandPL.Name = item.BrandName_1;
                                    brandPL.Price = item.Quote_P_High;
                                    brandPL.Mid_Price = item.Quote_Q_High;
                                    brandPL.Min_Price = item.Quote_R_High;
                                    brandPL.Scrap_Price = item.Quote_S_High;
                                    brandList.Add(brandPL);
                                }

                            }

                            //For Brand 2
                            if (!string.IsNullOrEmpty(item.BrandName_2))
                            {


                                string BrandDescription = ExchangeOrderManager.GetEnumDescription(PriceMasterBrandEnum.Others);
                                if (item.BrandName_2 == BrandDescription)
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_2.ToLower())).Id;
                                    brandPL.Name = item.BrandName_2;
                                    brandPL.Price = item.Quote_P;
                                    brandPL.Mid_Price = item.Quote_Q;
                                    brandPL.Min_Price = item.Quote_R;
                                    brandPL.Scrap_Price = item.Quote_S;
                                    brandList.Add(brandPL);
                                }
                                else
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_2.ToLower())).Id;
                                    brandPL.Name = item.BrandName_2;
                                    brandPL.Price = item.Quote_P_High;
                                    brandPL.Mid_Price = item.Quote_Q_High;
                                    brandPL.Min_Price = item.Quote_R_High;
                                    brandPL.Scrap_Price = item.Quote_S_High;
                                    brandList.Add(brandPL);
                                }


                            }

                            //For Brand 3
                            if (!string.IsNullOrEmpty(item.BrandName_3))
                            {
                                string BrandDescription = ExchangeOrderManager.GetEnumDescription(PriceMasterBrandEnum.Others);
                                if (item.BrandName_3 == BrandDescription)
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_3.ToLower())).Id;
                                    brandPL.Name = item.BrandName_3;
                                    brandPL.Price = item.Quote_P;
                                    brandPL.Mid_Price = item.Quote_Q;
                                    brandPL.Min_Price = item.Quote_R;
                                    brandPL.Scrap_Price = item.Quote_S;
                                    brandList.Add(brandPL);
                                }
                                else
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_3.ToLower())).Id;
                                    brandPL.Name = item.BrandName_3;
                                    brandPL.Price = item.Quote_P_High;
                                    brandPL.Mid_Price = item.Quote_Q_High;
                                    brandPL.Min_Price = item.Quote_R_High;
                                    brandPL.Scrap_Price = item.Quote_S_High;
                                    brandList.Add(brandPL);
                                }

                            }

                            //For Brand 4
                            if (!string.IsNullOrEmpty(item.BrandName_4))
                            {
                                string BrandDescription = ExchangeOrderManager.GetEnumDescription(PriceMasterBrandEnum.Others);
                                if (item.BrandName_4 == BrandDescription)
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_4.ToLower())).Id;
                                    brandPL.Name = item.BrandName_4;
                                    brandPL.Price = item.Quote_P;
                                    brandPL.Mid_Price = item.Quote_Q;
                                    brandPL.Min_Price = item.Quote_R;
                                    brandPL.Scrap_Price = item.Quote_S;
                                    brandList.Add(brandPL);
                                }
                                else
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_4.ToLower())).Id;
                                    brandPL.Name = item.BrandName_4;
                                    brandPL.Price = item.Quote_P_High;
                                    brandPL.Mid_Price = item.Quote_Q_High;
                                    brandPL.Min_Price = item.Quote_R_High;
                                    brandPL.Scrap_Price = item.Quote_S_High;
                                    brandList.Add(brandPL);
                                }

                            }
                            //productsPriceList.Add(productsPrice);
                            #endregion
                            if (!string.IsNullOrEmpty(brandid))
                            {
                                brandList = brandList.Where(x => x.BrandId == Convert.ToInt32(brandid)).ToList();
                            }
                            productsType.Brand = brandList;
                            productsTyepeList.Add(productsType);


                        }
                        productsPricesDC = new ProductsPricesDataContractSamsung();
                        productsPricesDC.ProductsFromType = productsTyepeList;

                        productsPricesDC.CategoryId = (int)tblUniversalPriceMasterObjList[0].ProductCategoryId;
                        productsPricesDC.categoryName = tblUniversalPriceMasterObjList[0].ProductCategoryName;

                    }
                    productsPricesList = new List<ProductsPricesDataContractSamsung>();
                    productsPricesList.Add(productsPricesDC);
                    #endregion
                }
                else
                {
                    tblUniversalPriceMasterObjList = _universalPriceMasterRepository.GetList(x => x.IsActive == true
                    && (x.PriceMasterNameId != null && x.PriceMasterNameId.Equals(PriceMasterNameId))).ToList();
                    List<tblProductCategory> categoryList = new List<tblProductCategory>();

                    List<int?> categoryforfilter = tblUniversalPriceMasterObjList.OrderBy(x => x.ProductCategoryId).Select(x => x.ProductCategoryId).Distinct().ToList();
                    foreach (var cat in categoryforfilter)
                    {
                        tblProductCategory categoryObj = _productCategoryRepository.GetSingle(x => x.IsActive == true && x.IsAllowedForOld == true && x.Id == cat);
                        if (categoryObj != null)
                        {

                            categoryList.Add(categoryObj);
                        }
                    }
                    productsPricesList = new List<ProductsPricesDataContractSamsung>();
                    //  ProductsFromTypePriceList productsType = new ProductsFromTypePriceList();
                    foreach (tblProductCategory catObj in categoryList)
                    {
                        productsPricesDC = new ProductsPricesDataContractSamsung();
                        productsPricesDC.CategoryId = catObj.Id;
                        productsPricesDC.categoryName = catObj.Description;
                        List<tblUniversalPriceMaster> temppriceMasterList = tblUniversalPriceMasterObjList.Where(x => x.ProductCategoryId == catObj.Id && x.IsActive == true).ToList();
                        productsTyepeList = new List<ProductsFromTypePriceList>();
                        foreach (var item in temppriceMasterList)
                        {
                            ProductsFromTypePriceList productsType = new ProductsFromTypePriceList();
                            //Code to check type

                            productsType.name = item.ProductTypeName.Replace(System.Environment.NewLine, string.Empty);

                            string size = masterCategoryTypeList.FirstOrDefault(x => x.Id.Equals(item.ProductTypeId) && x.IsActive == true).Size;

                            if (size != null)
                            {
                                productsType.name = productsType.name + " " + "(" + size + ")";
                            }

                            productsType.ProducttypeId = (int)item.ProductTypeId;
                            productsType.id = item.PriceMasterUniversalId;

                            #region code to fill brand list

                            BrandPriceList brandPL = null;
                            brandList = new List<BrandPriceList>();
                            //For Brand 1
                            //For Brand 1
                            if (!string.IsNullOrEmpty(item.BrandName_1))
                            {

                                string BrandDescription = ExchangeOrderManager.GetEnumDescription(PriceMasterBrandEnum.Others);
                                if (item.BrandName_1 == BrandDescription)
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_1.ToLower())).Id;
                                    brandPL.Name = item.BrandName_1;
                                    brandPL.Price = item.Quote_P;
                                    brandPL.Mid_Price = item.Quote_Q;
                                    brandPL.Min_Price = item.Quote_R;
                                    brandPL.Scrap_Price = item.Quote_S;
                                    brandList.Add(brandPL);
                                }
                                else
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_1.ToLower())).Id;
                                    brandPL.Name = item.BrandName_1;
                                    brandPL.Price = item.Quote_P_High;
                                    brandPL.Mid_Price = item.Quote_Q_High;
                                    brandPL.Min_Price = item.Quote_R_High;
                                    brandPL.Scrap_Price = item.Quote_S_High;
                                    brandList.Add(brandPL);
                                }
                            }

                            //For Brand 2
                            if (!string.IsNullOrEmpty(item.BrandName_2))
                            {


                                string BrandDescription = ExchangeOrderManager.GetEnumDescription(PriceMasterBrandEnum.Others);
                                if (item.BrandName_2 == BrandDescription)
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_2.ToLower())).Id;
                                    brandPL.Name = item.BrandName_2;
                                    brandPL.Price = item.Quote_P;
                                    brandPL.Mid_Price = item.Quote_Q;
                                    brandPL.Min_Price = item.Quote_R;
                                    brandPL.Scrap_Price = item.Quote_S;
                                    brandList.Add(brandPL);
                                }
                                else
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_2.ToLower())).Id;
                                    brandPL.Name = item.BrandName_2;
                                    brandPL.Price = item.Quote_P_High;
                                    brandPL.Mid_Price = item.Quote_Q_High;
                                    brandPL.Min_Price = item.Quote_R_High;
                                    brandPL.Scrap_Price = item.Quote_S_High;
                                    brandList.Add(brandPL);
                                }

                            }

                            //For Brand 3
                            if (!string.IsNullOrEmpty(item.BrandName_3))
                            {
                                string BrandDescription = ExchangeOrderManager.GetEnumDescription(PriceMasterBrandEnum.Others);
                                if (item.BrandName_3 == BrandDescription)
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_3.ToLower())).Id;
                                    brandPL.Name = item.BrandName_3;
                                    brandPL.Price = item.Quote_P;
                                    brandPL.Mid_Price = item.Quote_Q;
                                    brandPL.Min_Price = item.Quote_R;
                                    brandPL.Scrap_Price = item.Quote_S;
                                    brandList.Add(brandPL);
                                }
                                else
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_3.ToLower())).Id;
                                    brandPL.Name = item.BrandName_3;
                                    brandPL.Price = item.Quote_P_High;
                                    brandPL.Mid_Price = item.Quote_Q_High;
                                    brandPL.Min_Price = item.Quote_R_High;
                                    brandPL.Scrap_Price = item.Quote_S_High;
                                    brandList.Add(brandPL);
                                }

                            }

                            //For Brand 4
                            if (!string.IsNullOrEmpty(item.BrandName_4))
                            {
                                string BrandDescription = ExchangeOrderManager.GetEnumDescription(PriceMasterBrandEnum.Others);
                                if (item.BrandName_4 == BrandDescription)
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_4.ToLower())).Id;
                                    brandPL.Name = item.BrandName_4;
                                    brandPL.Price = item.Quote_P;
                                    brandPL.Mid_Price = item.Quote_Q;
                                    brandPL.Min_Price = item.Quote_R;
                                    brandPL.Scrap_Price = item.Quote_S;
                                    brandList.Add(brandPL);
                                }
                                else
                                {
                                    brandPL = new BrandPriceList();
                                    brandPL.BrandId = masterbrandList.FirstOrDefault(x => x.Name.ToLower().Equals(item.BrandName_4.ToLower())).Id;
                                    brandPL.Name = item.BrandName_4;
                                    brandPL.Price = item.Quote_P_High;
                                    brandPL.Mid_Price = item.Quote_Q_High;
                                    brandPL.Min_Price = item.Quote_R_High;
                                    brandPL.Scrap_Price = item.Quote_S_High;
                                    brandList.Add(brandPL);
                                }

                            }


                            #endregion

                            productsType.Brand = brandList;
                            productsTyepeList.Add(productsType);

                        }

                        productsPricesDC.ProductsFromType = productsTyepeList;
                        productsPricesList.Add(productsPricesDC);

                    }

                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ExchangeOrderManager", "GetProductPrice", ex);
            }
            return productsPricesList;
        }

        #endregion


        #region sa
        public bool GenerateQCLInkBeforeSendVoucher(ExchangeOrderDataContract exchangeOrderDataContract)
        {
            bool flag = false;
            string BrandName = string.Empty;
            string ProductCategory = string.Empty;
            string ProductType = string.Empty;
            string ResponseCode = string.Empty;
            WhatasappResponse whatssappresponseDC = null;
            string responseforWhatasapp = string.Empty;
            _whatsAppMessageRepository = new WhatsappMessageRepository();
            string ERPBaseURL = ConfigurationManager.AppSettings["ERPBaseURL"].ToString();
        string selfQC = ConfigurationManager.AppSettings["SelfQCUrl"].ToString();
        string SelfQCLink = ERPBaseURL + "" + selfQC + "" + exchangeOrderDataContract.RegdNo;
            tblProductType productType = _productTypeRepository.GetSingle(x => x.Id == exchangeOrderDataContract.ProductTypeId);
            if (productType != null)
            {
                tblProductCategory productCategory = _productCategoryRepository.GetSingle(x => x.Id == productType.ProductCatId);
                if (productCategory != null)
                {
                    tblBrand brandobj = _brandRepository.GetSingle(x => x.Id == exchangeOrderDataContract.BrandId);
                    if (brandobj != null)
                    {
                        BrandName = brandobj.Name;
                        ProductCategory = productCategory.Description;
                        ProductType = productType.Description;
                        OrderConfirmationTemplateExchange whatsappObjforOrderConfirmation = new OrderConfirmationTemplateExchange();
                        whatsappObjforOrderConfirmation.userDetails = new UserDetails();
                        whatsappObjforOrderConfirmation.notification = new OrderConfiirmationNotification();
                        WhatsappNotificationManager whatsappNotificationManager = new WhatsappNotificationManager();
                        whatsappObjforOrderConfirmation.notification.@params = new SendWhatssappForExcahangeConfirmation();
                        whatsappObjforOrderConfirmation.notification.@params.CustName = exchangeOrderDataContract.FirstName + " " + exchangeOrderDataContract.LastName;
                        whatsappObjforOrderConfirmation.notification.@params.Link = SelfQCLink;
                        whatsappObjforOrderConfirmation.notification.@params.ProductBrand = BrandName;
                        whatsappObjforOrderConfirmation.notification.@params.ProdCategory = ProductCategory;
                        whatsappObjforOrderConfirmation.notification.@params.ProdType = ProductType;
                        whatsappObjforOrderConfirmation.notification.@params.RegdNO = exchangeOrderDataContract.RegdNo.ToString();


                        whatsappObjforOrderConfirmation.userDetails.number = exchangeOrderDataContract.PhoneNumber;
                        whatsappObjforOrderConfirmation.notification.templateId = NotificationConstants.orderConfirmationForExchange;
                        whatsappObjforOrderConfirmation.notification.@params.CustName = exchangeOrderDataContract.FirstName + " " + exchangeOrderDataContract.LastName; ;
                        whatsappObjforOrderConfirmation.notification.@params.RegdNO = exchangeOrderDataContract.RegdNo.ToString();
                        whatsappObjforOrderConfirmation.notification.@params.ProdCategory = ProductCategory;

                        whatsappObjforOrderConfirmation.notification.@params.CustName = exchangeOrderDataContract.FirstName + " " + exchangeOrderDataContract.LastName;
                        whatsappObjforOrderConfirmation.notification.@params.Number = exchangeOrderDataContract.PhoneNumber;
                        whatsappObjforOrderConfirmation.notification.@params.Email = exchangeOrderDataContract.Email;
                        whatsappObjforOrderConfirmation.notification.@params.Link = SelfQCLink;

                        List<string> templateParams = new List<string>
                                   {
                                       whatsappObjforOrderConfirmation.notification.@params.CustName, whatsappObjforOrderConfirmation.notification.@params.RegdNO,     whatsappObjforOrderConfirmation.notification.@params.ProdCategory,  
                                       whatsappObjforOrderConfirmation.notification.@params.ProdType, 
                                       whatsappObjforOrderConfirmation.notification.@params.CustName,
                                       whatsappObjforOrderConfirmation.notification.@params.Number,
                                       whatsappObjforOrderConfirmation.notification.@params.Email,
                                       whatsappObjforOrderConfirmation.notification.@params.Link,
                                   };
                        HttpResponseDetails response = whatsappNotificationManager.SendWhatsAppMessageAsync(
                                            whatsappObjforOrderConfirmation.notification.templateId,
                                            whatsappObjforOrderConfirmation.userDetails.number,
                                            templateParams
                                        ).GetAwaiter().GetResult();

                        ResponseCode = response.Response.StatusCode.ToString();
                        string WhatssAppStatusEnum = ExchangeOrderManager.GetEnumDescription(WhatssAppEnum.AiSuccessCode);
                        if (ResponseCode == WhatssAppStatusEnum)
                        {
                            responseforWhatasapp = response.Content;
                            if (responseforWhatasapp != null)
                            {
                                whatssappresponseDC = JsonConvert.DeserializeObject<WhatasappResponse>(responseforWhatasapp);
                                tblWhatsAppMessage whatsapObj = new tblWhatsAppMessage();
                                whatsapObj.TemplateName = NotificationConstants.ABBRedemptionSelfQC;
                                whatsapObj.IsActive = true;
                                whatsapObj.PhoneNumber = exchangeOrderDataContract.PhoneNumber;
                                whatsapObj.SendDate = DateTime.Now;
                                whatsapObj.msgId = whatssappresponseDC.msgId;
                                _whatsAppMessageRepository.Add(whatsapObj);
                                _whatsAppMessageRepository.SaveChanges();
                                flag = true;
                            }
                            else
                            {
                                string ExchOrderObj = JsonConvert.SerializeObject(exchangeOrderDataContract);
                                logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", exchangeOrderDataContract.SponsorOrderNumber, ExchOrderObj);
                            }
                        }
                        else
                        {
                            string ExchOrderObj = JsonConvert.SerializeObject(exchangeOrderDataContract);
                            logging.WriteAPIRequestToDB("WhatsappNotificationManager", "Rest_InvokeWhatsappserviceCall", exchangeOrderDataContract.SponsorOrderNumber, ExchOrderObj);
                        }
                    }
                }
            }

            return flag;
                       }
        #endregion
    }
}
