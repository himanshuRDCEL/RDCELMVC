using ConsoleApp.Model;
using GraspCorn.Common.Enums;
using GraspCorn.Common.Helper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using RDCEL.DocUpload.DAL.Repository;
using RDCEL.DocUpload.DAL;
using RDCEL.DocUpload.DataContract.SponsorModel;
using RDCEL.DocUpload.BAL.Common;
using RDCEL.DocUpload.DAL.Helper;
using Daikin_scheduler_console.Model;
using System.Linq;

namespace Daikin_scheduler_console.DAL
{
    public class Daikin_Scheduler_Dal
    {
        #region Variable Declaration
        DateTime currentDatetime = DateTime.Now.TrimMilliseconds();
        #endregion

        private string _connectionString;
        public string Url;
        public string username;

        public Daikin_Scheduler_Dal()
        {
            _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Digi2l_DBEntitiesSQL"].ToString();
            Url = System.Configuration.ConfigurationManager.AppSettings["GetServiceRequest"].ToString();
        }

        public List<ExchangeOrderViewModel> GetExchangeOrderViewList()
        {
            var listExchangeOrderViewModel = new List<ExchangeOrderViewModel>();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[SPDaikin_Scheduler]", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        listExchangeOrderViewModel.Add(new ExchangeOrderViewModel
                        {
                            ExchangeOrderID = Convert.ToInt32(rdr[0]),
                            RegdNo = rdr[1].ToString(),
                            SponsorServiceRefId = rdr[2].ToString()
                        });
                    }
                    rdr.Close();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("ServiceRequest_Daikin_ExhangeOrder", "Update_Order_Status", ex);
                Console.WriteLine("Exception : " + ex.Message);
                throw ex;
            }
            return listExchangeOrderViewModel;
        }

        public string GetUserPassword()
        {
            string Password = null;
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("[GetPassowrdForDaikin]", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    Password = cmd.ExecuteScalar().ToString();
                    //SqlDataReader rdr = cmd.ExecuteReader();
                    //while (rdr.Read())
                    //{
                    //    Password = rdr["Password"].ToString();
                    //}

                    con.Close();
                }
            }
            catch(Exception ex)
            {
                LibLogging.WriteErrorToDB("ServiceRequest_Daikin_ExhangeOrder", "GetUserPassword", ex);
                Console.WriteLine("Exception : " + ex.Message);
                throw ex;
            }
            return Password;
        }

        public string UrlForGetRequest()
        {
            string url = null;
            try
            {
                url = Url;
            }
            catch(Exception ex)
            {
                LibLogging.WriteErrorToDB("", "", ex);
                Console.WriteLine("Exception : " + ex.Message);
                throw ex;
            }
            return url;
        }
    }
}
