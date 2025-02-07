using RDCEL.DocUpload.DAL.AbstractRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using RDCEL.DocUpload.DAL.Helper;

namespace RDCEL.DocUpload.DAL.Repository
{
    public class ModelNumberRepository : AbstractRepository<tblModelNumber>
    {
        // Get Product Category
        public virtual DataTable GetModellist(int? catid,int? typeId,int? BusinessUnitId, int? BussinessPartnerId,int? NewBrandId)
        {
            
            DataTable dt = new DataTable();
            try
            {
                DBHelper obj = new DBHelper();
                SqlParameter[] sqlParam =  {
                      new SqlParameter("@Buid",BusinessUnitId),
                      new SqlParameter("@Bpid",BussinessPartnerId),
                      new SqlParameter("@Brandid",NewBrandId),
                      new SqlParameter("@catId",catid),
                      new SqlParameter("@typeId",typeId)
              };
                dt = obj.ExecuteDataTable("sp_GetModalDetailsByBUandBp", sqlParam);
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
            }
            return dt;
        }
    }
}
