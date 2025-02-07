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
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DataContract.ZohoModel;

namespace RDCEL.DocUpload.BAL.UTCZohoSync
{
    public class PriceMasterInfoCall
    {
        #region Variable Declaration   
        PriceMasterRepository productPriceInformationRepository;
        DateTime currentDatetime = DateTime.Now.TrimMilliseconds();
        #endregion

        #region Update Price Master details into DB
        /// <summary>
        /// Method to update Price Master
        /// </summary>
        /// <param></param>
        /// <returns>model</returns>

        public int UpdatePriceMasterInfotoDB(PriceMasterData priceMasterDataObj, tblPriceMaster productPriceInfo)
        {
            productPriceInformationRepository = new PriceMasterRepository();
            int result = 0;
            try
            {
                tblPriceMaster priceMasterInfo = SetPriceMasterInfoObject(priceMasterDataObj, productPriceInfo);

                if (priceMasterInfo != null && priceMasterInfo.Id > 0)
                {
                    priceMasterInfo.ModifiedDate = DateTime.Now.TrimMilliseconds();
                    productPriceInformationRepository.Update(priceMasterInfo);
                    productPriceInformationRepository.SaveChanges();
                    result = priceMasterInfo.Id;
                }
                else
                {
                    priceMasterInfo.IsActive = true;
                    priceMasterInfo.CreatedDate = DateTime.Now.TrimMilliseconds();
                    productPriceInformationRepository.Add(priceMasterInfo);
                    productPriceInformationRepository.SaveChanges();
                    result = priceMasterInfo.Id;
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("PriceMasterInfoCall", "AddPriceMasterInfotoDB", ex);
            }
            return result;

        }


        public tblPriceMaster SetPriceMasterInfoObject(PriceMasterData priceMasterDataObj, tblPriceMaster productPriceInfo)
        {
            tblPriceMaster priceMasterInfo = null;
            try
            {
                if (priceMasterDataObj != null && productPriceInfo != null)
                {
                    priceMasterInfo = new tblPriceMaster();

                    priceMasterInfo.Id = productPriceInfo.Id;
                    priceMasterInfo.ZohoPriceMasterId = priceMasterDataObj.ID;
                    priceMasterInfo.ProductCategoryId = productPriceInfo.ProductCategoryId;
                    priceMasterInfo.ProductCat = productPriceInfo.ProductCat;
                    priceMasterInfo.ProductTypeId = productPriceInfo.ProductTypeId;
                    priceMasterInfo.ProductType = productPriceInfo.ProductType;
                    priceMasterInfo.ProductTypeCode = productPriceInfo.ProductTypeCode;
                    priceMasterInfo.ExchPriceCode = priceMasterDataObj.Exch_price_ID;
                 
                    if(priceMasterDataObj.Brand_1 != null)
                    {
                        priceMasterInfo.BrandName_1 = priceMasterDataObj.Brand_1.display_value;
                    }
                    if (priceMasterDataObj.Brand_2 != null)
                    {
                        priceMasterInfo.BrandName_2 = priceMasterDataObj.Brand_2.display_value;
                    }
                    if (priceMasterDataObj.Brand_3 != null)
                    {
                        priceMasterInfo.BrandName_3 = priceMasterDataObj.Brand_3.display_value;
                    }
                    if (priceMasterDataObj.Brand_4 != null)
                    {
                        priceMasterInfo.BrandName_4 = priceMasterDataObj.Brand_4.display_value;
                    }

                    priceMasterInfo.Quote_P_High = priceMasterDataObj.Quote_P_H;
                    priceMasterInfo.Quote_Q_High = priceMasterDataObj.Quote_Q_H;
                    priceMasterInfo.Quote_R_High = priceMasterDataObj.Quote_R_H;
                    priceMasterInfo.Quote_S_High = priceMasterDataObj.Quote_S_H;
                    priceMasterInfo.Quote_P = priceMasterDataObj.Quote_P;
                    priceMasterInfo.Quote_Q = priceMasterDataObj.Quote_Q;
                    priceMasterInfo.Quote_R = priceMasterDataObj.Quote_R;
                    priceMasterInfo.Quote_S = priceMasterDataObj.Quote_S;
                    priceMasterInfo.PriceStartDate = priceMasterDataObj.Price_Start_Date;
                    priceMasterInfo.PriceEndDate = priceMasterDataObj.Price_End_Date;                  
                    priceMasterInfo.IsActive = true;
                    priceMasterInfo.ModifiedDate = currentDatetime;
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("PriceMasterInfoCall", "SetPriceMasterInfoObject", ex);
            }
            return priceMasterInfo;
        }

        #endregion












    }
}
