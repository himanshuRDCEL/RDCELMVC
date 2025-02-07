using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using SFTPFileUpload.Model;


namespace SFTPFileUpload.DAL
{
    public class SFTP_Day_End_Report_Dal
    {
        #region variabels
        private string _connectionString;
        public string Url;
        #endregion


        public SFTP_Day_End_Report_Dal()
        {
  
            _connectionString =ConfigurationManager.ConnectionStrings["Digi2l_DBEntitiesSQL"].ToString();
           // Url = System.Configuration.ConfigurationManager.AppSettings["GetServiceRequest"].ToString();
        }

        public SFTP_DAY_End_Report_Model GetOrderDetails(string VoucherCode)
        {

            SFTP_DAY_End_Report_Model orderdetails = new SFTP_DAY_End_Report_Model();
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_Pinelabs_DayEndReportData", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Add parameters to the command
                    cmd.Parameters.AddWithValue("@VoucherCode", VoucherCode);
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        string CheckDate = (rdr[0]).ToString();
                        if (!string.IsNullOrEmpty(CheckDate))
                        {
                            DateTime dtCreateddate = Convert.ToDateTime(rdr[0]);
                            orderdetails.DateOfTRX = dtCreateddate.ToString("dd-MMM-yyyy");
                        }
                        else
                        {
                            orderdetails.DateOfTRX = string.Empty;
                        }
                     
                        orderdetails.PickUpStatus = rdr[1].ToString();
                        if (rdr[2] != null)
                        {
                            string rdr2 = rdr[2].ToString();
                            if (!string.IsNullOrEmpty(rdr2))
                            {
                               DateTime pickDate = Convert.ToDateTime(rdr[2]);
                                orderdetails.DateOfDevicePickUp = pickDate.ToString("dd-MMM-yyyy");
                            }
                            else
                            {
                                orderdetails.DateOfDevicePickUp = string.Empty;
                            }
                            
                        }
                        orderdetails.VoucherCode = rdr[5].ToString();
                        string amount = (rdr[6]).ToString();
                        if (!string.IsNullOrEmpty(amount))
                        {
                            orderdetails.Amount = Convert.ToDouble(rdr[6]);
                        }
                        string bonus = (rdr[7]).ToString();
                        if (!string.IsNullOrEmpty(bonus))
                        {
                            orderdetails.OEMBonusAmount = Convert.ToDouble(rdr[7]);
                        }
                        orderdetails.UTRNumber = rdr[9].ToString();
                       
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
            return orderdetails;
        }
      
    }
}
