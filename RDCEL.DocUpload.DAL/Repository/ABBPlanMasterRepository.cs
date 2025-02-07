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
   public class ABBPlanMasterRepository:AbstractRepository<tblABBPlanMaster>
    {
        /// <summary>
        ///  Method to get the list of new producttype by BU and product category id
        /// </summary>

        /// <param name="buid"></param>
        /// <returns></returns>
        public virtual DataTable GetNewProductCategoryForABB(int buid,int catid)
        {
            DataTable dt = new DataTable();
            try
            {
                DBHelper obj = new DBHelper();
                SqlParameter[] sqlParam =  {

                        new SqlParameter("@buid", buid),
                        new SqlParameter("@catid", catid)

                        };
                dt = obj.ExecuteDataTable("sp_getProductTypeForABB", sqlParam);
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ProductCategoryMappingRepository", "GetNewProductCategory", ex);
            }
            return dt;
        }
    }
}
