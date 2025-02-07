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
    public class CouponRepository : AbstractRepository<tblCoupon>
    {
        public virtual DataTable CouponVerification(int BusinessUnitId, int BusinessPartnerId, string CouponCode)
        {
            DataTable dt = new DataTable();
        
            int resultStatus = 0; // Initialize the output parameter

            try
            {
                DBHelper obj = new DBHelper();
                    SqlParameter[] sqlParam =
                    {
                        new SqlParameter("@BusinessUnitId", BusinessUnitId),
                        new SqlParameter("@BusinessPartnerId", BusinessPartnerId),
                        new SqlParameter("@CouponCode", CouponCode),
                        new SqlParameter
                        {
                            ParameterName = "@StatusResult",
                            SqlDbType = SqlDbType.Int,
                            Direction = ParameterDirection.Output // Set the parameter as output
                        }
                    };

                dt = obj.ExecuteDataTable("sp_CouponVerification", sqlParam);

                // Retrieve the output parameter value after executing the stored procedure
                resultStatus = Convert.ToInt32(sqlParam[3].Value);
                if (dt == null)
                {
                    dt = new DataTable();
                    dt.Columns.Add("StatusValue", typeof(int));
                    // Add a new row to the DataTable
                    DataRow newRow = dt.NewRow();
                    newRow["StatusValue"] = resultStatus;
                    dt.Rows.Add(resultStatus); // Example integer value
                }
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
                // Handle exceptions as needed
            }

            return dt;
        }

    }
}
