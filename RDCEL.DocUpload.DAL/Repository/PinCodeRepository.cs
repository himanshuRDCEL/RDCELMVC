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
   public class PinCodeRepository : AbstractRepository<tblPinCode>
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

                dt = obj.ExecuteDataTableWithoutParam("sp_getPincodeforPineLabs");
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
        public virtual DataTable GetStateList(int pincode)
        {
            DataTable dt = new DataTable();
            try
            {
                DBHelper obj = new DBHelper();
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@pincode",pincode)
                };
                dt = obj.ExecuteDataTable("sp_getStateAndCityByPincode", sqlParam);
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
            }
            return dt;
        }

        /// <summary>
        /// Method to get the list of Pincode customer form
        /// </summary>
        /// <returns></returns>
        public virtual DataTable GetPincodeListABB()
        {
            DataTable dt = new DataTable();
            try
            {
                DBHelper obj = new DBHelper();

                dt = obj.ExecuteDataTableWithoutParam("sp_GetPincodeForABB");
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
            }
            return dt;
        }

        /// <summary>
        /// Method to get the list of States and city for customer 
        /// </summary>
        /// <returns></returns>
        public virtual DataTable GetABBSateAndCityABB(int pincode)
        {
            DataTable dt = new DataTable();
            try
            {
                DBHelper obj = new DBHelper();
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@pincode",pincode)
                };
                dt = obj.ExecuteDataTable("sp_GetStateCityForABB", sqlParam);
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
            }
            return dt;
        }


        /// <summary>
        /// Method to get the list of Pincode
        /// </summary>
        /// <returns></returns>
        public virtual DataTable GetPincodeListForALlianceD2C()
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
        public virtual DataTable GetCityListForAllianceD2C(string pincode)
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


        /// <summary>
        /// Method to get the list of Pincode
        /// </summary>
        /// <returns></returns>
        public virtual DataTable GetPincodeListForABBD2C()
        {
            DataTable dt = new DataTable();
            try
            {
                DBHelper obj = new DBHelper();

                dt = obj.ExecuteDataTableWithoutParam("sp_GetPincodeListForABBD2C");
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
            }
            return dt;
        }


        //create by priyanshi
        public virtual DataTable GetPincodeListbybuidforex(string pincode,int? buid)
        {
            DataTable dt = new DataTable();
            try
            {
                DBHelper obj = new DBHelper();
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@pincode",pincode),
                     new SqlParameter("@buid", buid)
                };
                dt = obj.ExecuteDataTable("sp_GetExchangePincodeListByBuidAndPincodeText", sqlParam);

                //DBHelper obj = new DBHelper();

                //dt = obj.ExecuteDataTableWithoutParam("sp_getPincodeforPineLabs");
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
            }
            return dt;
        }

        //create by priyanshi
        public virtual DataTable GetPincodeListbybuidforABB(string pincode, int? buid)
        {
            DataTable dt = new DataTable();
            try
            {
                DBHelper obj = new DBHelper();
                SqlParameter[] sqlParam =
                {
                    new SqlParameter("@pincode",pincode),
                     new SqlParameter("@buid", buid)
                };
                dt = obj.ExecuteDataTable("sp_GetABBPincodeListByBuidAndPincodeText", sqlParam);



                //DBHelper obj = new DBHelper();

                //dt = obj.ExecuteDataTableWithoutParam("sp_getPincodeforPineLabs");
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
            }
            return dt;
        }

        //create by priyanshi
        public virtual DataTable GetABBPincodeListbybuid(int? buid)
        {
            DataTable dt = new DataTable();
            try
            {
                DBHelper obj = new DBHelper();
                SqlParameter[] sqlParam =
                {                   
                     new SqlParameter("@buid", buid)
                };
                dt = obj.ExecuteDataTable("sp_GetABBPincodeListByBuid", sqlParam);              
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
            }
            return dt;
        }


        //create by priyanshi
        public virtual DataTable GetEXchangePincodeListbybuid(int? buid)
        {
            DataTable dt = new DataTable();
            try
            {
                DBHelper obj = new DBHelper();
                SqlParameter[] sqlParam =
                {
                     new SqlParameter("@buid", buid)
                };
                dt = obj.ExecuteDataTable("sp_GetExchangePincodeListByBuid", sqlParam);
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
            }
            return dt;
        }
    }
}
