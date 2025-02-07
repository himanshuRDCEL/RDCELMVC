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
    public class LoginDetailsUTCRepository : AbstractRepository<Login>
    {
        /// <summary>
        /// Method to get the login details
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="password">password</param>
        /// <returns>DataSet</returns>
        public virtual DataSet GetLoginDetails(string username,string password)
        {
            DataSet objDS = new DataSet();
            DBHelper obj = new DBHelper();
            try
            {
                SqlParameter[] parameters = new SqlParameter[] {
                    new SqlParameter("@username", username  ),
                    new SqlParameter("@password", password)
                };
                objDS = obj.ExecuteDataSet("LoginByUsernamePassword", parameters);
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
            }
            return objDS;
        }
    }
}
