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
    public class BUProductCategoryMappingRepository : AbstractRepository<tblBUProductCategoryMapping>
    {
        #region Get Product Category By BU Id
        public virtual DataTable GetProductCategoryByBUId(int BuId)
        {
            DataTable dt = new DataTable();
            try
            {
                DBHelper obj = new DBHelper();
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@BuId", BuId),
                };
                dt = obj.ExecuteDataTable("sp_GetProductCategoryByBUId", sqlParam);
            }
            catch(Exception ex)
            {
                string message = ex.Message;
            }
            return dt;
        }
        #endregion

        #region Get Product Type By BU Id and Category Id
        public virtual DataTable GetProductTypeByBUIdandCatId(int BUId, int catId)
        {
            DataTable dt = new DataTable();
            try
            {
                DBHelper obj = new DBHelper();
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@BUId", BUId),
                    new SqlParameter("@catId", catId)
                };
                dt = obj.ExecuteDataTable("sp_GetProductTypeByBUIdandCatId", sqlParam);
            }
            catch(Exception ex)
            {
                string message = ex.Message;
            }
            return dt;
        }
        #endregion

        #region Get Product Type By BU Id only
        public virtual DataTable GetProductTypeByBUIdOnly(int BUId)
        {
            DataTable dt = new DataTable();
            try
            {
                DBHelper obj = new DBHelper();
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@BUId", BUId)
                };
                dt = obj.ExecuteDataTable("sp_GetProductTypeByBUIdOnly", sqlParam);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return dt;
        }
        #endregion
    }
}
