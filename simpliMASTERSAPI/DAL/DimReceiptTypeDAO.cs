using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.TO;
using simpliMASTERSAPI.DAL.Interfaces;

namespace ODLMWebAPI.DAL
{
    public class DimReceiptTypeDAO : IDimReceiptTypeDAO
    {
        private readonly IConnectionString _iConnectionString;
        public DimReceiptTypeDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
           
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [dimReceiptType] where isActive=1"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public DataTable SelectAllDimReceiptType()
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

                //cmdSelect.Parameters.Add("@idReceiptType", System.Data.SqlDbType.Int).Value = dimReceiptTypeTO.IdReceiptType;
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

        public  DataTable SelectDimReceiptType(Int32 idReceiptType)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idReceiptType = " + idReceiptType +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idReceiptType", System.Data.SqlDbType.Int).Value = dimReceiptTypeTO.IdReceiptType;
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

        public DataTable SelectAllDimReceiptType(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idReceiptType", System.Data.SqlDbType.Int).Value = dimReceiptTypeTO.IdReceiptType;
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
        public int InsertDimReceiptType(DimReceiptTypeTO dimReceiptTypeTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(dimReceiptTypeTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public  int InsertDimReceiptType(DimReceiptTypeTO dimReceiptTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(dimReceiptTypeTO, cmdInsert);
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

        public int ExecuteInsertionCommand(DimReceiptTypeTO dimReceiptTypeTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [dimReceiptType]( " + 
            "  [idReceiptType]" +
            " ,[sysElementId]" +
            " ,[isActive]" +
            " ,[receiptTypeName]" +
            " ,[receiptTypeDesc]" +
            " )" +
" VALUES (" +
            "  @IdReceiptType " +
            " ,@SysElementId " +
            " ,@IsActive " +
            " ,@ReceiptTypeName " +
            " ,@ReceiptTypeDesc " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdReceiptType", System.Data.SqlDbType.Int).Value = dimReceiptTypeTO.IdReceiptType;
            cmdInsert.Parameters.Add("@SysElementId", System.Data.SqlDbType.Int).Value = dimReceiptTypeTO.SysElementId;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Bit).Value = dimReceiptTypeTO.IsActive;
            cmdInsert.Parameters.Add("@ReceiptTypeName", System.Data.SqlDbType.VarChar).Value = dimReceiptTypeTO.ReceiptTypeName;
            cmdInsert.Parameters.Add("@ReceiptTypeDesc", System.Data.SqlDbType.VarChar).Value = dimReceiptTypeTO.ReceiptTypeDesc;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public  int UpdateDimReceiptType(DimReceiptTypeTO dimReceiptTypeTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(dimReceiptTypeTO, cmdUpdate);
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

        public int UpdateDimReceiptType(DimReceiptTypeTO dimReceiptTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(dimReceiptTypeTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(DimReceiptTypeTO dimReceiptTypeTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [dimReceiptType] SET " + 
            "  [idReceiptType] = @IdReceiptType" +
            " ,[sysElementId]= @SysElementId" +
            " ,[isActive]= @IsActive" +
            " ,[receiptTypeName]= @ReceiptTypeName" +
            " ,[receiptTypeDesc] = @ReceiptTypeDesc" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdReceiptType", System.Data.SqlDbType.Int).Value = dimReceiptTypeTO.IdReceiptType;
            cmdUpdate.Parameters.Add("@SysElementId", System.Data.SqlDbType.Int).Value = dimReceiptTypeTO.SysElementId;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Bit).Value = dimReceiptTypeTO.IsActive;
            cmdUpdate.Parameters.Add("@ReceiptTypeName", System.Data.SqlDbType.VarChar).Value = dimReceiptTypeTO.ReceiptTypeName;
            cmdUpdate.Parameters.Add("@ReceiptTypeDesc", System.Data.SqlDbType.VarChar).Value = dimReceiptTypeTO.ReceiptTypeDesc;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteDimReceiptType(Int32 idReceiptType)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idReceiptType, cmdDelete);
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

        public int DeleteDimReceiptType(Int32 idReceiptType, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idReceiptType, cmdDelete);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }
        public int ExecuteDeletionCommand(Int32 idReceiptType, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [dimReceiptType] " +
            " WHERE idReceiptType = " + idReceiptType + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idReceiptType", System.Data.SqlDbType.Int).Value = dimReceiptTypeTO.IdReceiptType;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
