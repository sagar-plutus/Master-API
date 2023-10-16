using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
//using System.Windows.Forms;
using System.Configuration;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL.Interfaces;

namespace ODLMWebAPI.DAL
{
    public class DimOtherChargesTypeDAO: IDimOtherChargesTypeDAO
    {
        private readonly IConnectionString _iConnectionString;
        public DimOtherChargesTypeDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }

        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [dimOtherChargesType]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  DataTable SelectAllDimOtherChargesType()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idOtherChargesType", System.Data.SqlDbType.Int).Value = dimOtherChargesTypeTO.IdOtherChargesType;
                SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public DataTable SelectDimOtherChargesType(Int32 idOtherChargesType)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idOtherChargesType = " + idOtherChargesType +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idOtherChargesType", System.Data.SqlDbType.Int).Value = dimOtherChargesTypeTO.IdOtherChargesType;
                SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public DataTable SelectAllDimOtherChargesType(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idOtherChargesType", System.Data.SqlDbType.Int).Value = dimOtherChargesTypeTO.IdOtherChargesType;
                SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
            }
        }

        #endregion
        
        #region Insertion
        public int InsertDimOtherChargesType(DimOtherChargesTypeTO dimOtherChargesTypeTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(dimOtherChargesTypeTO, cmdInsert);
            }
            catch(Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public int InsertDimOtherChargesType(DimOtherChargesTypeTO dimOtherChargesTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(dimOtherChargesTypeTO, cmdInsert);
            }
            catch(Exception ex)
            { 
                return 0;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public int ExecuteInsertionCommand(DimOtherChargesTypeTO dimOtherChargesTypeTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [dimOtherChargesType]( " + 
            "  [idOtherChargesType]" +
            " ,[isActive]" +
            " ,[otherChargesName]" +
            " )" +
" VALUES (" +
            "  @IdOtherChargesType " +
            " ,@IsActive " +
            " ,@OtherChargesName " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdOtherChargesType", System.Data.SqlDbType.Int).Value = dimOtherChargesTypeTO.IdOtherChargesType;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Bit).Value = dimOtherChargesTypeTO.IsActive;
            cmdInsert.Parameters.Add("@OtherChargesName", System.Data.SqlDbType.VarChar).Value = dimOtherChargesTypeTO.OtherChargesName;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public  int UpdateDimOtherChargesType(DimOtherChargesTypeTO dimOtherChargesTypeTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(dimOtherChargesTypeTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public int UpdateDimOtherChargesType(DimOtherChargesTypeTO dimOtherChargesTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(dimOtherChargesTypeTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public static int ExecuteUpdationCommand(DimOtherChargesTypeTO dimOtherChargesTypeTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [dimOtherChargesType] SET " + 
            "  [idOtherChargesType] = @IdOtherChargesType" +
            " ,[isActive]= @IsActive" +
            " ,[otherChargesName] = @OtherChargesName" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdOtherChargesType", System.Data.SqlDbType.Int).Value = dimOtherChargesTypeTO.IdOtherChargesType;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Bit).Value = dimOtherChargesTypeTO.IsActive;
            cmdUpdate.Parameters.Add("@OtherChargesName", System.Data.SqlDbType.VarChar).Value = dimOtherChargesTypeTO.OtherChargesName;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public int DeleteDimOtherChargesType(Int32 idOtherChargesType)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idOtherChargesType, cmdDelete);
            }
            catch(Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public  int DeleteDimOtherChargesType(Int32 idOtherChargesType, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idOtherChargesType, cmdDelete);
            }
            catch(Exception ex)
            {          
                return 0;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public static int ExecuteDeletionCommand(Int32 idOtherChargesType, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [dimOtherChargesType] " +
            " WHERE idOtherChargesType = " + idOtherChargesType + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idOtherChargesType", System.Data.SqlDbType.Int).Value = dimOtherChargesTypeTO.IdOtherChargesType;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
