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
    public class PincodeMasterDtoCRepository : AbstractRepository<tblPincodeMasterDtoC>
    {


        /// <summary>
        /// Method to get the list of Pincode
        /// </summary>
        /// <returns></returns>
        public virtual DataTable GetPincodeList()
        {
            DataTable dt = new DataTable();
            try
            {
                DBHelper obj = new DBHelper();

                dt = obj.ExecuteDataTableWithoutParam("sp_GetPincodeListForMYG");
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
            }
            return dt;
        }


        /// <summary>
        /// Method to get the list of Cities
        /// </summary>
        /// <returns></returns>
        public virtual DataTable GetCityList(string pincode)
        {
            DataTable dt = new DataTable();
            try
            {
                DBHelper obj = new DBHelper();
                SqlParameter[] sqlParam =  {
                        new SqlParameter("@pincode",pincode)

                        };
                dt = obj.ExecuteDataTable("GetCityForMyGate", sqlParam);
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
            }
            return dt;
        }

        /// <summary>
        /// Method to get the list of States
        /// </summary>
        /// <returns></returns>
        public virtual DataTable GetStateList(string pincode)
        {
            DataTable dt = new DataTable();
            try
            {
                DBHelper obj = new DBHelper();
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@pincode",pincode)
                };
                dt = obj.ExecuteDataTable("sp_GetStateByPincode", sqlParam);
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
            }
            return dt;
        }
    }
}
