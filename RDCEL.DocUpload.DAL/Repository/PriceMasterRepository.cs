using RDCEL.DocUpload.DAL.AbstractRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using RDCEL.DocUpload.DAL.Helper;
using System.Data.SqlClient;

namespace RDCEL.DocUpload.DAL.Repository
{
    public class PriceMasterRepository : AbstractRepository<tblPriceMaster>
    {
        /// <summary>
        /// Method to get the list of product category for exchange by price code
        /// </summary>
        /// <returns>DataTable</returns>
        public virtual DataTable GetProductCategoryByPriceCode(string priceCode)
        {
            DataTable dt = new DataTable();
            try
            {

                DBHelper obj = new DBHelper();
                SqlParameter[] sqlParam =  {
                        new SqlParameter("@pricecode",priceCode)
                };
                dt = obj.ExecuteDataTable("sp_GetProductCategoryByPriceCode", sqlParam);
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
            }
            return dt;
        }

        /// <summary>
        /// Method to get the list of product category for exchange by price code
        /// </summary>
        /// <returns>DataTable</returns>
        public virtual DataTable GetProducttypeByPriceCode(string priceCode, int catid)
        {
            DataTable dt = new DataTable();
            try
            {
                DBHelper obj = new DBHelper();
                SqlParameter[] sqlParam =  {
                        new SqlParameter("@pricecode",priceCode),
                        new SqlParameter("@catid",catid)
                };
                dt = obj.ExecuteDataTable("sp_GetProductTypeByPriceCode", sqlParam);
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
            }
            return dt;
        }


        #region for new universal pricemaster mapping
        //Mapping details 
        //input - BUID , BPID
        //output - tblPriceMasterMapping
        public virtual DataTable GetPriceMasterMappingDetails(int BUId,int BPId)
        {
            DataTable dt = new DataTable();
            try
            {
                DBHelper obj = new DBHelper();
                SqlParameter[] sqlParam =  {
                        new SqlParameter("@buid",BUId),
                        new SqlParameter("@bpid",BPId)
                };
                dt = obj.ExecuteDataTable("sp_GetPriceMasterMappingDetails", sqlParam);
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
            }
            return dt;
        }
        //product type
        //input - priceMasterNameId , catid
        //output - list of distinct productType

        public virtual DataTable GetProductTypeByPriceMasterNameId(int PriceMasterNameId, int catid)
        {
            DataTable dt = new DataTable();
            try
            {
                DBHelper obj = new DBHelper();
                SqlParameter[] sqlParam =  {
                        new SqlParameter("@PriceMasterNameId",PriceMasterNameId),
                        new SqlParameter("@catid",catid)
                };
                dt = obj.ExecuteDataTable("sp_GetProductTypeByPriceMasterNameId", sqlParam);
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
            }
            return dt;
        }

        // Get brands
        public virtual DataTable GetBrandsByPriceMasterNameId(int PriceMasterNameId, int catid)
        {
            DataTable dt = new DataTable();
            try
            {
                DBHelper obj = new DBHelper();
                SqlParameter[] sqlParam =  {
                        new SqlParameter("@PriceMasterNameId",PriceMasterNameId),
                        new SqlParameter("@catid",catid)
                };
                dt = obj.ExecuteDataTable("sp_GetProductTypeByPriceMasterNameId", sqlParam);
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
            }
            return dt;
        }

        // Get Product Category
        public virtual DataTable GetProductCategoryByPriceMasterId(int PriceMasterNameId)
        {
            DataTable dt = new DataTable();
            try
            {
                DBHelper obj = new DBHelper();
                SqlParameter[] sqlParam =  {
                        new SqlParameter("@PriceMasterNameId",PriceMasterNameId),
                };
                dt = obj.ExecuteDataTable("sp_GetProductCategoryByPriceMasterId", sqlParam);
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
            }
            return dt;
        }


        #endregion

    }
}
