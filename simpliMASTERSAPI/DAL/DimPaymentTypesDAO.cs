using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.Models;
using ODLMWebAPI.BL.Interfaces;
using simpliMASTERSAPI.DAL.Interfaces;

namespace ODLMWebAPI.DAL
{
    public class DimPaymentTypesDAO: IDimPaymentTypesDAO
    {
        #region Methods
        private readonly IConnectionString _iConnectionString;
        public DimPaymentTypesDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [dimPaymentTypes]";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public DataTable SelectAllDimPaymentTypes()
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

                //cmdSelect.Parameters.Add("@idPayType", System.Data.SqlDbType.Int).Value = dimPaymentTypesTO.IdPayType;
                SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public DataTable SelectDimPaymentTypes(Int32 idPayType)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idPayType = " + idPayType + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPayType", System.Data.SqlDbType.Int).Value = dimPaymentTypesTO.IdPayType;
                SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public DataTable SelectAllDimPaymentTypes(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPayType", System.Data.SqlDbType.Int).Value = dimPaymentTypesTO.IdPayType;
                SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
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
        public int InsertDimPaymentTypes(DimPaymentTypesTO dimPaymentTypesTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(dimPaymentTypesTO, cmdInsert);
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

        public int InsertDimPaymentTypes(DimPaymentTypesTO dimPaymentTypesTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(dimPaymentTypesTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public int ExecuteInsertionCommand(DimPaymentTypesTO dimPaymentTypesTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [dimPaymentTypes]( " +
            "  [idPayType]" +
            " ,[permissionId]" +
            " ,[isActive]" +
            " ,[payTypeName]" +
            " ,[payTypeDec]" +
            " )" +
" VALUES (" +
            "  @IdPayType " +
            " ,@PermissionId " +
            " ,@IsActive " +
            " ,@PayTypeName " +
            " ,@PayTypeDec " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdPayType", System.Data.SqlDbType.Int).Value = dimPaymentTypesTO.IdPayType;
            cmdInsert.Parameters.Add("@PermissionId", System.Data.SqlDbType.Int).Value = dimPaymentTypesTO.PermissionId;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Bit).Value = dimPaymentTypesTO.IsActive;
            cmdInsert.Parameters.Add("@PayTypeName", System.Data.SqlDbType.VarChar).Value = dimPaymentTypesTO.PayTypeName;
            cmdInsert.Parameters.Add("@PayTypeDec", System.Data.SqlDbType.VarChar).Value = dimPaymentTypesTO.PayTypeDec;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion

        #region Updation
        public int UpdateDimPaymentTypes(DimPaymentTypesTO dimPaymentTypesTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(dimPaymentTypesTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public int UpdateDimPaymentTypes(DimPaymentTypesTO dimPaymentTypesTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(dimPaymentTypesTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public int ExecuteUpdationCommand(DimPaymentTypesTO dimPaymentTypesTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [dimPaymentTypes] SET " +
            "  [idPayType] = @IdPayType" +
            " ,[permissionId]= @PermissionId" +
            " ,[isActive]= @IsActive" +
            " ,[payTypeName]= @PayTypeName" +
            " ,[payTypeDec] = @PayTypeDec" +
            " WHERE 1 = 2 ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdPayType", System.Data.SqlDbType.Int).Value = dimPaymentTypesTO.IdPayType;
            cmdUpdate.Parameters.Add("@PermissionId", System.Data.SqlDbType.Int).Value = dimPaymentTypesTO.PermissionId;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Bit).Value = dimPaymentTypesTO.IsActive;
            cmdUpdate.Parameters.Add("@PayTypeName", System.Data.SqlDbType.VarChar).Value = dimPaymentTypesTO.PayTypeName;
            cmdUpdate.Parameters.Add("@PayTypeDec", System.Data.SqlDbType.VarChar).Value = dimPaymentTypesTO.PayTypeDec;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public int DeleteDimPaymentTypes(Int32 idPayType)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idPayType, cmdDelete);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public int DeleteDimPaymentTypes(Int32 idPayType, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idPayType, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idPayType, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [dimPaymentTypes] " +
            " WHERE idPayType = " + idPayType + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idPayType", System.Data.SqlDbType.Int).Value = dimPaymentTypesTO.IdPayType;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
    }
}
