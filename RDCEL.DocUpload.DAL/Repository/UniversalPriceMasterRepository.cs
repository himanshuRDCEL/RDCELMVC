using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCEL.DocUpload.DAL.AbstractRepository;
using RDCEL.DocUpload.DAL.Helper;

namespace RDCEL.DocUpload.DAL.Repository
{
   public class UniversalPriceMasterRepository:AbstractRepository<tblUniversalPriceMaster>
    {
        #region for new universal pricemaster mapping
        //Mapping details 
        //input - BUID , BPID
        //output - tblPriceMasterMapping
        public virtual DataTable GetPriceMasterMappingDetails(int BUId, int? BPId)
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
        public virtual DataTable GetProductTypeByPriceMasterNameId(int? PriceMasterNameId, int catid)
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
        public virtual DataTable GetBrandsByPriceMasterNameId(int?catid,int? PriceMasterNameId,int? typeId)
        {
            DataTable dt = new DataTable();
            try
            {
                DBHelper obj = new DBHelper();
                SqlParameter[] sqlParam =  {
                      new SqlParameter("@PriceMasterNameId",PriceMasterNameId),
                      new SqlParameter("@catid",catid),
                      new SqlParameter("@typeId",typeId)
              };
                dt = obj.ExecuteDataTable("sp_GetBrandsByPriceMasterNameId", sqlParam);
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
            }
            return dt;
        }
       
        // Get Product Category
        public virtual DataTable GetProductCategoryByPriceMasterId(int? PriceMasterNameId)
        {
            DataTable dt = new DataTable();
            try
            {
                DBHelper obj = new DBHelper();
                SqlParameter[] sqlParam =  {
                      new SqlParameter("@PriceMasterNameId",PriceMasterNameId),
              };
                dt = obj.ExecuteDataTable("sp_GetProductCategoryByPriceMasterNameId", sqlParam);
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
            }
            return dt;
        }
        #endregion

        #region get all product type by price master id
        public virtual DataTable GetProductTypeByPriceMasterNameIdOnly(int? PriceMasterNameId)
        {
            DataTable dt = new DataTable();
            try
            {
                DBHelper obj = new DBHelper();
                SqlParameter[] sqlParam =  {
                      new SqlParameter("@PriceMasterNameId",PriceMasterNameId)
                      
              };
                dt = obj.ExecuteDataTable("sp_GetProductTypeByPriceMasterNameIdOnly", sqlParam);
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
