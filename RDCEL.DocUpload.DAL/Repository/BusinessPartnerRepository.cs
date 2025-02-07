using RDCEL.DocUpload.DAL.AbstractRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using RDCEL.DocUpload.DAL.Helper;
using System.Data.SqlClient;
using GraspCorn.Common.Helper;

namespace RDCEL.DocUpload.DAL.Repository
{
    public class BusinessPartnerRepository : AbstractRepository<tblBusinessPartner>
    {
        /// <summary>
        /// Method to get the list of State
        /// </summary>
        /// <returns></returns>
        public virtual DataTable GetStateList()
        {
            DataTable dt = new DataTable();
            try
            {

                DBHelper obj = new DBHelper();
                dt = obj.ExecuteDataTableWithoutParam("sp_GetStateList");
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
            }
            return dt;
        }

        /// <summary>
        /// Method to get the list of City
        /// </summary>
        /// <returns></returns>
        public virtual DataTable GetCityList()
        {
            DataTable dt = new DataTable();
            try
            {
                DBHelper obj = new DBHelper();
                dt = obj.ExecuteDataTableWithoutParam("sp_GetCityList");
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
            }
            return dt;
        }

        /// <summary>
        ///  Method to get the list of City by BU and state name
        /// </summary>
        /// <param name="stateName"></param>
        /// <param name="buid"></param>
        /// <returns></returns>
        public virtual DataTable GetCityListbyBU(string stateName, int buid, string email = "")
        {
            DataTable dt = new DataTable();
            try
            {
                DBHelper obj = new DBHelper();
                SqlParameter[] sqlParam =  {
                        new SqlParameter("@state",stateName),
                        new SqlParameter("@buid", buid),
                        new SqlParameter("@email", email)
                        };
                dt = obj.ExecuteDataTable("sp_GetCityListForBU", sqlParam);


            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("BusinessPartnerRepository", "GetCityListbyBU", ex);
            }
            return dt;
        }

        /// <summary>
        ///  Method to get the list of Pincode by BU and state name
        /// </summary>
        /// <param name="stateName"></param>
        /// <param name="cityName"></param>
        /// <param name="buid"></param>
        /// <returns></returns>
        public virtual DataTable GetPincodeListForBU(string stateName, string cityName, int buid)
        {
            DataTable dt = new DataTable();
            try
            {
                DBHelper obj = new DBHelper();
                SqlParameter[] sqlParam =  {
                        new SqlParameter("@state",stateName),
                        new SqlParameter("@city",cityName),
                        new SqlParameter("@buid", buid)
                        };
                dt = obj.ExecuteDataTable("sp_GetPincodeListForBU", sqlParam);
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("BusinessPartnerRepository", "GetPincodeListForBU", ex);
            }
            return dt;
        }
        /// <summary>
        ///  Method to get the list of City by  state name
        /// </summary>
        /// <param name="stateName"></param>

        /// <returns></returns>
        public virtual DataTable GetCityByStateName(string stateName)
        {
            DataTable dt = new DataTable();
            try
            {
                DBHelper obj = new DBHelper();
                SqlParameter[] sqlParam =  {
                        new SqlParameter("@state",stateName)
                        };
                dt = obj.ExecuteDataTable("GetCityByStateName", sqlParam);
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("BusinessPartnerRepository", "GetCityByStateName", ex);
            }
            return dt;
        }
        /// <summary>
        ///  Method to get the list of Pincode by BU and state name
        /// </summary>
        /// <param name="cityName"></param>

        /// <returns></returns>
        public virtual DataTable GetPinCodeListforMYGate(string cityName, string stateName)
        {
            DataTable dt = new DataTable();
            try
            {
                DBHelper obj = new DBHelper();
                SqlParameter[] sqlParam =  {
                        new SqlParameter("@city",cityName),
                        new SqlParameter("@state",stateName)
                        };
                dt = obj.ExecuteDataTable("sp_GetPincodeListForMYG", sqlParam);
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("BusinessPartnerRepository", "GetPinCodeListforMYGate", ex);
            }
            return dt;
        }
        public virtual DataTable GetAllStoresData(string associatecode)
        {
            DataTable dt = new DataTable();
            try
            {
                DBHelper obj = new DBHelper();
                SqlParameter[] sqlParam =  {
                        new SqlParameter("@Associatecode",associatecode)
                        };
                dt = obj.ExecuteDataTable("sp_GetAllStoreDataOfSingleDealer", sqlParam);
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("BusinessPartnerRepository", "GetAllStoresData", ex);
            }
            return dt;
        }

        /// <summary>
        /// Method to get the list of State
        /// </summary>
        /// <returns></returns>
        public virtual DataTable GetStateListABB()
        {
            DataTable dt = new DataTable();
            try
            {

                DBHelper obj = new DBHelper();
                dt = obj.ExecuteDataTableWithoutParam("sp_GetStateForABB");
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
            }
            return dt;
        }

        /// <summary>
        /// Method to get the count of all orders of one associate
        /// </summary>
        /// <returns></returns>
        public virtual DataTable GetAllStoreOrderCount(string AssociateCode)
        {
            DataTable dt = new DataTable();
            try
            {
                DBHelper obj = new DBHelper();
                SqlParameter[] sqlParam =  {
                        new SqlParameter("@AssociateCode",AssociateCode)
                        };
                dt = obj.ExecuteDataTable("sp_GetOrderCountForAllStoresOfsingleAssociate", sqlParam);
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("BusinessPartnerRepository", "GetAllStoreOrderCount", ex);
            }
            return dt;
        }

        /// <summary>
        /// Method to get the list of Area Locality By Pincode
        /// </summary>
        /// <returns></returns>
        public virtual DataTable GetAreaLocalityByPincode(string Pincode)
        {
            DataTable dt = new DataTable();
            try
            {
                DBHelper obj = new DBHelper();
                SqlParameter[] sqlParam =  {
                        new SqlParameter("@Pincode",Pincode)
                        };
                 dt = obj.ExecuteDataTable("sp_GetAreaLocalityListByPincode", sqlParam);
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
            }
            return dt;
        }
        public virtual DataTable GetAreaLocalityById(int ArealocalityId)
        {
            DataTable dt = new DataTable();
            try
            {
                DBHelper obj = new DBHelper();
                SqlParameter[] sqlParam =  {
                        new SqlParameter("@AreaLocalityId",ArealocalityId)
                        };
                dt = obj.ExecuteDataTable("sp_GetAreaLocalitybyAreaId", sqlParam);
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
            }
            return dt;
        }

        #region get city list for metro cities
        public virtual DataTable GetCityListforExchange(int buid)
        {
            DataTable dt = new DataTable();
            try
            {
                DBHelper obj = new DBHelper();
                SqlParameter[] sqlParam =  {
                        new SqlParameter("@buid", buid),
                        };
                dt = obj.ExecuteDataTable("sp_GetCityListforExchange", sqlParam);
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("BusinessPartnerRepository", "GetCityListforExchange", ex);
            }
            return dt;
        }

        #endregion

        #region get pincode by cityId and BuId
        public virtual DataTable GetPincodeByCityIdBUId(int buid,int cityId,string pintext)
       {
            DataTable dt = new DataTable();
            try
            {
                DBHelper obj = new DBHelper();
                SqlParameter[] sqlParam =  {
                        new SqlParameter("@cityId",cityId),
                        new SqlParameter("@buid", buid),
                        new SqlParameter("@pintext", pintext)

                        };
                dt = obj.ExecuteDataTable("sp_GetPincodeByCityIdBUId", sqlParam);
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("BusinessPartnerRepository", "GetPincodeListForBU", ex);
            }
            return dt;
        }
        #endregion

        /// <summary>
        ///  Method to get the list of Bp by BU and pincode 
        /// </summary>
        /// <param name="pincode"></param>
        /// <param name="buid"></param>
        /// <param name="city"></param>
        

        /// <returns></returns>
        public virtual DataTable GetBpListbyPincode(string city, string pincode, int buid)
        {
            DataTable dt = new DataTable();
            var cityId = Convert.ToInt32(city);
            try
            {
                DBHelper obj = new DBHelper();
                SqlParameter[] sqlParam =  {
                        new SqlParameter("@city",cityId),
                        new SqlParameter("@pincode",pincode),
                        new SqlParameter("@buid",buid)
                        };
                dt = obj.ExecuteDataTable("sp_GetBPListbyPincode", sqlParam);
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("BusinessPartnerRepository", "GetPinCodeListforMYGate", ex);
            }
            return dt;
        }

        public bool? GetCouponAvailableStatusByBpId(int bpid)
        {
            bool? isCouponsAvailable = null;
            try
            {
                DBHelper obj = new DBHelper();
                SqlParameter[] sqlParam =  {
            new SqlParameter("@BusinessPartnerID", bpid)
        };

                DataTable dt = obj.ExecuteDataTable("sp_GetIsCouponsAvailable", sqlParam);

                if (dt.Rows.Count > 0)
                {
                    var value = dt.Rows[0]["IsCouponsAvailable"];
                    isCouponsAvailable = value != DBNull.Value ? (bool?)value : null;
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("BusinessPartnerRepository", "GetCouponAvailableStatusByBpId", ex);
            }
            return isCouponsAvailable;
        }



    }
}
