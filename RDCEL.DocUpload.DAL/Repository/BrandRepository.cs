using RDCEL.DocUpload.DAL.AbstractRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RDCEL.DocUpload.DAL.Helper;
using System.Data;
using System.Data.SqlClient;

namespace RDCEL.DocUpload.DAL.Repository
{
    public class BrandRepository : AbstractRepository<tblBrand>
    {
        /// <summary>
        /// Method to get the list of Brands
        /// </summary>
        /// <returns></returns>
        public virtual DataTable GetBrandList(int? priceMasternameid)
        {
            DataTable dt = new DataTable();
            try
            {
              
                DBHelper obj = new DBHelper();
                SqlParameter[] sqlParam =  {
                        new SqlParameter("@PriceMasterNameId",priceMasternameid)
                };
                dt = obj.ExecuteDataTable("sp_GetBrabdList", sqlParam);
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
            }
            return dt;
        }
        /// <summary>
        /// Method to get the list of Brands for exchange
        /// </summary>
        /// <returns></returns>
        public virtual DataTable GetBrabdListForExchange()
        {
            DataTable dt = new DataTable();
            try
            {

                DBHelper obj = new DBHelper();
                dt = obj.ExecuteDataTableWithoutParam("sp_GetBrabdListForExchange");
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
            }
            return dt;
        }

        #region Get Brand on basis of category and  type id
        /// <summary>
        /// Method to get the list of Brands for exchange by product cat id
        /// </summary>
        /// <returns>DataTable</returns>
        public virtual DataTable GetBrabdListForExchangeByCatId(int catId,string priceCode,int typeId)
        {
            DataTable dt = new DataTable();
            try
            {

                DBHelper obj = new DBHelper();
                SqlParameter[] sqlParam =  {
                        new SqlParameter("@carId",catId),
                        new SqlParameter("@priceCode",priceCode),
                        new SqlParameter("@typeId",typeId)
                };
                dt = obj.ExecuteDataTable("sp_GetBrabdListForExchangeByCatId", sqlParam);
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
            }
            return dt;
        }
        #endregion

        #region Get brand on basis of CatId
        /// <summary>
        /// Method to get the list of Brands for exchange by product cat id
        /// </summary>
        /// <returns>DataTable</returns>
        public virtual DataTable GetBrabdListForExchangeByCategoryId(int catId, string priceCode)
        {
            DataTable dt = new DataTable();
            try
            {

                DBHelper obj = new DBHelper();
                SqlParameter[] sqlParam =  {
                        new SqlParameter("@carId",catId),
                        new SqlParameter("@priceCode",priceCode),
                };
                dt = obj.ExecuteDataTable("sp_GetBrabdListForExchangeByCategoryId", sqlParam);
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
