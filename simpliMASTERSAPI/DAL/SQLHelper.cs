using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using ODLMWebAPI.DAL.Interfaces;
using System.Reflection;

using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.StaticStuff;
namespace ODLMWebAPI.DAL
{ 
    public class SQLHelper : ISQLHelper
    {
        private readonly IConnectionString _iConnectionString;
        public SQLHelper(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        private string connectionStringLocalTransaction= "";
        public SQLHelper()
        {
           
        }
        public IConfiguration Configuration { get; }

        //public readonly string ConnectionStringLocalTransaction = connectionString;
        public int ExecuteNonQuery(CommandType cmdType, string cmdText, params SqlParameter[] commandParmeter)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            if (conn.State != ConnectionState.Open)
                conn.Open();
            PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParmeter);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            conn.Close();
            return val;

        }

        public int ExecuteNonQuery(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        public object ExecuteScalar(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();

            SqlConnection connection = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            connection.Close();
            return val;
        }

        private  void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = cmdType;
            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                {
                    if (parm.Value == null)
                    {
                        parm.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parm);
                }
            }
        }


        /// <summary>
        /// This method is used to get Array of given class object from db by using reflection method
        /// </summary>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public List<T> ExecuteReader<T>(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {

            using (SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING)))
            {
                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                SqlDataReader sdr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(sdr);
                List<T> data = new List<T>();
                data = ConvertDataTable<T>(dt);
                sdr.Close();
                conn.Close();
                return data;
            }
        }


        private  List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private  T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                    {
                        if (dr[column.ColumnName] != DBNull.Value)
                            pro.SetValue(obj, dr[column.ColumnName], null);
                    }
                    else
                        continue;
                }
            }
            return obj;
        }
    

    public SqlDataReader ExecuteReader(CommandType cmdType, string cmdText, CommandBehavior behavior, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));

            // we use a try/catch here because if the method throws an exception we want to 
            // close the connection throw code, because no datareader will exist, hence the 
            // commandBehaviour.CloseConnection will not work
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                SqlDataReader rdr = cmd.ExecuteReader(behavior);
                //cmd.Parameters.Clear();
                return rdr;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }

        public DataTable ExecuteReaderDataTable<T>(CommandType cmdType, string cmdText, string connectionString, params SqlParameter[] commandParameters)
        {

            // using (SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING)))
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                SqlDataReader sdr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(sdr);

                sdr.Close();
                conn.Close();
                return dt;
            }
        }
        


        //Deepali Added to get Serial no column in DT for Search filter[30-07-2021]
        public DataTable ExecuteReaderDataTableWithSrNoColumnAdded<T>(CommandType cmdType, string cmdText, string connectionString, params SqlParameter[] commandParameters)
        {

            // using (SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING)))
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                SqlDataReader sdr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(sdr);

                DataTable dtIncremented = new DataTable(dt.TableName);
                DataColumn dc = new DataColumn("Sr No");
                dc.AutoIncrement = true;
                dc.AutoIncrementSeed = 1;
                dc.AutoIncrementStep = 1;
                dc.DataType = typeof(Int32);
                dtIncremented.Columns.Add(dc);

                dtIncremented.BeginLoadData();

                DataTableReader dtReader = new DataTableReader(dt);
                dtIncremented.Load(dtReader);
                dtIncremented.EndLoadData();

                sdr.Close();
                conn.Close();
                return dtIncremented;
            }
        }

    }
}
