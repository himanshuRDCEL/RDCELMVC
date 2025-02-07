using GraspCorn.Common.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDCEL.DocUpload.DAL.Helper
{
    public class DBHelper
    {
        #region Declare Variables 

        SqlConnection connection = null;
        SqlCommand command = null;
        public string connectionstring = ConfigurationManager.ConnectionStrings["Digi2l_DBEntitiesSQL"].ToString();

        #endregion

        /// <summary>
        /// Execute procedure 
        /// </summary>
        /// <param name="commandName">procedure name</param>
        /// <param name="paramCollection">collatin of paramter</param>
        /// <returns></returns>
        public DataSet ExecuteProcedure(string commandName, List<SqlParameter> paramCollection)
        {
            DataSet ds = new DataSet();
            try
            {
                connection = new SqlConnection(connectionstring);
                command = new SqlCommand(commandName, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                foreach (SqlParameter item in paramCollection)
                {
                    command.Parameters.Add(item);
                }
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("DBHelper", "ExecuteProcedure", ex);

            }
            finally
            {
                connection.Close();
            }
            return ds;
        }
        internal DataSet ExecuteDataSet1(string v, SqlParameter[] parameters)
        {
            throw new NotImplementedException();
        }
        public DataSet ExecuteDataSet(string oSql, params SqlParameter[] sqlParam)
        {
            DataSet oDS = null;
            try
            {
                connection = new SqlConnection(connectionstring);


                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = oSql;
                if ((sqlParam != null))
                {
                    foreach (SqlParameter param in sqlParam)
                    {
                        cmd.Parameters.Add(param);
                    }
                }
                SqlDataAdapter oDA = new SqlDataAdapter(cmd);
                oDS = new DataSet();
                oDA.Fill(oDS);


            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("DBHelper", "ExecuteDataSet", ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
            return oDS;
        }

        /// <summary>
        /// Method to get the DataTable from the procedure.
        /// </summary>
        /// <param name="oSql"></param>
        /// <param name="sqlParam"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string oSql, params SqlParameter[] sqlParam)
        {
            DataSet oDS = null;
            DataTable oDT = null;
            try
            {
                connection = new SqlConnection(connectionstring);


                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = oSql;
                if ((sqlParam != null))
                {
                    foreach (SqlParameter param in sqlParam)
                    {
                        cmd.Parameters.Add(param);
                    }
                }
                SqlDataAdapter oDA = new SqlDataAdapter(cmd);
                oDS = new DataSet();
                oDA.Fill(oDS);


                if (oDS != null && oDS.Tables.Count > 0)
                {
                    oDT = new DataTable();
                    oDT = oDS.Tables[0];
                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("DBHelper", "ExecuteDataTable", ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
            return oDT;
        }

        /// <summary>
        /// Method to get the DataTable from the procedure.
        /// </summary>
        /// <param name="oSql"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTableWithoutParam(string oSql)
        {
            DataSet oDS = null;
            DataTable oDT = null;
            try
            {
                connection = new SqlConnection(connectionstring);


                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = oSql;
                //if ((sqlParam != null))
                //{
                //    foreach (SqlParameter param in sqlParam)
                //    {
                //        cmd.Parameters.Add(param);
                //    }
                //}
                SqlDataAdapter oDA = new SqlDataAdapter(cmd);
                oDS = new DataSet();
                oDA.Fill(oDS);


                if (oDS != null && oDS.Tables.Count > 0)
                {
                    oDT = new DataTable();
                    oDT = oDS.Tables[0];
                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("DBHelper", "ExecuteDataTable", ex);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
            return oDT;
        }

        public int ExecuteNonQuery(string commandName, List<SqlParameter> paramCollection)
        {
            int rowAffected = 0;
            try
            {
                connection = new SqlConnection(connectionstring);
                connection.Open();
                command = new SqlCommand(commandName, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                foreach (SqlParameter item in paramCollection)
                {
                    command.Parameters.Add(item);
                }
                rowAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("DBHelper", "ExecuteNonQuery", ex);
            }
            finally
            {
                connection.Close();
            }
            return rowAffected;
        }

    }
}
