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
   public class AreaLocalityRepository : AbstractRepository<tblAreaLocality>
    {

        /// <summary>
        ///  Method to get AreaLocality by ID 
        /// </summary>
        /// <param name="stateName"></param>
        /// <param name="buid"></param>
        /// <returns></returns>
        public virtual DataTable GetAreaLocalitybyID(string AreaLocalityId)
        {
            DataTable dt = new DataTable();
            try
            {
                DBHelper obj = new DBHelper();
                SqlParameter[] sqlParam =  {
                        new SqlParameter("@AreaLocalityId",AreaLocalityId)
                        };
                dt = obj.ExecuteDataTable("sp_GetAreaLocalitybyAreaId", sqlParam);
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("BusinessPartnerRepository", "GetAreaLocalitybyID", ex);
            }
            return dt;
        }

    }
}
