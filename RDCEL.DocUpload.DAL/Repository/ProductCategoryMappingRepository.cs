using GraspCorn.Common.Helper;
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
     public class ProductCategoryMappingRepository:AbstractRepository<tblBUProductCategoryMapping>
    {
        /// <summary>
        ///  Method to get the list of new productCategory by BU 
        /// </summary>
     
        /// <param name="buid"></param>
        /// <returns></returns>
        public virtual DataTable GetNewProductCategory(int buid)
        {
            DataTable dt = new DataTable();
            try
            {
                DBHelper obj = new DBHelper();
                SqlParameter[] sqlParam =  {
                        
                        new SqlParameter("@buid", buid)
                     
                        };
                dt = obj.ExecuteDataTable("sp_getProductCategoryListForNew", sqlParam);
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ProductCategoryMappingRepository", "GetNewProductCategory", ex);
            }
            return dt;
        }


        /// <summary>
        ///  Method to get the list of new productCategory by BU 
        /// </summary>

        /// <param name="buid"></param>
        /// <returns></returns>
        public virtual DataTable GetNewProductCategoryForABB(int buid)
        {
            DataTable dt = new DataTable();
            try
            {
                DBHelper obj = new DBHelper();
                SqlParameter[] sqlParam =  {

                        new SqlParameter("@buid", buid)

                        };
                dt = obj.ExecuteDataTable("sp_getProductCategoryListForNewABB", sqlParam);
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ProductCategoryMappingRepository", "GetNewProductCategory", ex);
            }
            return dt;
        }
    }

    
   

}
