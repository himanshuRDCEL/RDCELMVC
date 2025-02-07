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
    public class BrandSmartSellRepository:AbstractRepository<tblBrandSmartBuy>
    {

        /// <summary>
        ///  Method to get the list of City by BU and state name
        /// </summary>
        /// <param name="catid"></param>
        /// <param name="buid"></param>
        /// <returns></returns>
        public virtual DataTable GetBrandistbyBU(int catid, int buid)
        {
            DataTable dt = new DataTable();
            try
            {
                DBHelper obj = new DBHelper();
                SqlParameter[] sqlParam =  {
                        new SqlParameter("@catid",catid),
                        new SqlParameter("@buid", buid)
                        
                        };
                dt = obj.ExecuteDataTable("sp_GetBrandListAsperBusinessUnitId", sqlParam);


            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("BrandSmartSellRepository", "GetBrandistbyBU", ex);
            }
            return dt;
        }

    }
}
