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
   public class ExchangeOrderRepository : AbstractRepository<tblExchangeOrder>
    {

        /// <summary>
        ///  Method to get the list of City by BU and state name
        /// </summary>
        /// <param name="stateName"></param>
        /// <param name="buid"></param>
        /// <returns></returns>
        public virtual DataTable GetNotDeliveredOrderForBU(string companyName, string fromDate)
        {
            DataTable dt = new DataTable();
            try
            {
                DBHelper obj = new DBHelper();
                SqlParameter[] sqlParam =  {
                        new SqlParameter("@CompanyName",companyName),
                        new SqlParameter("@FromDate", fromDate)
                        };
                dt = obj.ExecuteDataTable("sp_GetNotDeliveredOrderForBU", sqlParam);
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("BusinessPartnerRepository", "GetCityListbyBU", ex);
            }
            return dt;
        }

        /// <summary>
        /// Method to get the exchange order detail by voucher code and customer phone number
        /// </summary>
        /// <param name="vcode">Voucher Cdode</param>
        /// <param name="custphone">Customer Phone number</param>
        /// <returns>DataTable</returns>
        public virtual DataTable GetExchangeOrderDCByVoucherCode(string vcode, string custphone)
        {
            DataTable dt = new DataTable();
            try
            {
                DBHelper obj = new DBHelper();
                SqlParameter[] sqlParam =  {
                        new SqlParameter("@VoucherCode",vcode),
                        new SqlParameter("@CustPhoneNumber", custphone)
                        };
                dt = obj.ExecuteDataTable("sp_GetExchangeOrderDCByVoucherCode", sqlParam);
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
            }
            return dt;
        }

        /// <summary>
        /// Method to get the exchange order detail by voucher code and customer phone number
        /// </summary>
        /// <param name="vcode">Voucher Cdode</param>
        /// <param name="custphone">Customer Phone number</param>
        /// <returns>DataTable</returns>
        public virtual DataSet GetOrderSummaryForBU(int buid, string companyName)
        {
            DataSet ds = new DataSet();
            try
            {
                DBHelper obj = new DBHelper();
                SqlParameter[] sqlParam =  {
                        new SqlParameter("@buid",buid),
                        new SqlParameter("@companyName", companyName)
                        };
                ds = obj.ExecuteDataSet("sp_GetOrderSummaryForBU", sqlParam);
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
            }
            return ds;
        }

        /// <summary>
        /// Method to get the exchange order detail by voucher code and customer phone number
        /// </summary>
        /// <param name="vcode">Voucher Cdode</param>
        /// <param name="custphone">Customer Phone number</param>
        /// <returns>DataTable</returns>
        public virtual DataSet GetOrderSummaryForBUWithDateRange(string date1, string date2, string companyName, int? statusCode)
        {
            DataSet ds = new DataSet();
            try
            {
                DBHelper obj = new DBHelper();
                SqlParameter[] sqlParam =  {
                        new SqlParameter("@startdate",date1),
                        new SqlParameter("@enddate",date2),
                        new SqlParameter("@companyName", companyName),
                        new SqlParameter("@statusCode", statusCode)
                        };
                ds = obj.ExecuteDataSet("sp_GetOrderSummaryForBUWithDateRange", sqlParam);
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
            }
            return ds;
        }

        /// <summary>
        /// Method to get the exchange order status with status code
        /// </summary>
        /// <returns>DataTable</returns>
        public virtual DataSet GetOrderStatusSummaryForAllBU()
        {
            DataSet ds = new DataSet();
            try
            {
                DBHelper obj = new DBHelper();
                SqlParameter[] sqlParam =  {
                //        new SqlParameter("@startdate",date1),
                //        new SqlParameter("@enddate",date2),
                //        new SqlParameter("@companyName", companyName),
                //        new SqlParameter("@statusCode", statusCode)
                      };
                ds = obj.ExecuteDataSet("sp_GetOrderStatusSummaryForAllBU", sqlParam);
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
            }
            return ds;
        }

        #region Method To get Order Details By Voucher code and Phone Number
        public virtual DataTable GetExchangeOrderDetails(string voucherCode,string PhoneNo)
        {
            DataTable ds = new DataTable();
            try
            {
                DBHelper obj = new DBHelper();
                SqlParameter[] sqlParam={
                    new SqlParameter("@voucher", voucherCode),
                        new SqlParameter("@PhoneNo", PhoneNo)
                };
                ds = obj.ExecuteDataTable("sp_GetOrderDetailsByVoucherCodeandPhoneNo", sqlParam);
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
            }
            return ds;
        }
        #endregion
    }
}
