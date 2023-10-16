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
    public class TblPaymentAllocationDAO: ITblPaymentAllocationDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblPaymentAllocationDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblPaymentAllocation]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  DataTable SelectAllTblPaymentAllocation()
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

                //cmdSelect.Parameters.Add("@idPaymentAllocation", System.Data.SqlDbType.Int).Value = tblPaymentAllocationTO.IdPaymentAllocation;
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

        public  DataTable SelectTblPaymentAllocation(Int32 idPaymentAllocation)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idPaymentAllocation = " + idPaymentAllocation +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPaymentAllocation", System.Data.SqlDbType.Int).Value = tblPaymentAllocationTO.IdPaymentAllocation;
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

        public DataTable SelectAllTblPaymentAllocation(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPaymentAllocation", System.Data.SqlDbType.Int).Value = tblPaymentAllocationTO.IdPaymentAllocation;
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
        public  int InsertTblPaymentAllocation(TblPaymentAllocationTO tblPaymentAllocationTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblPaymentAllocationTO, cmdInsert);
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

        public  int InsertTblPaymentAllocation(TblPaymentAllocationTO tblPaymentAllocationTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblPaymentAllocationTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblPaymentAllocationTO tblPaymentAllocationTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblPaymentAllocation]( " + 
            "  [idPaymentAllocation]" +
            " ,[createdBy]" +
            " ,[createdOn]" +
            " ,[isLatestEntry]" +
            " ,[bankAmt]" +
            " ,[fundAllocationAmt]" +
            " )" +
" VALUES (" +
            "  @IdPaymentAllocation " +
            " ,@CreatedBy " +
            " ,@CreatedOn " +
            " ,@IsLatestEntry " +
            " ,@BankAmt " +
            " ,@FundAllocationAmt " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdPaymentAllocation", System.Data.SqlDbType.Int).Value = tblPaymentAllocationTO.IdPaymentAllocation;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPaymentAllocationTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPaymentAllocationTO.CreatedOn;
            cmdInsert.Parameters.Add("@IsLatestEntry", System.Data.SqlDbType.Bit).Value = tblPaymentAllocationTO.IsLatestEntry;
            cmdInsert.Parameters.Add("@BankAmt", System.Data.SqlDbType.Decimal).Value = tblPaymentAllocationTO.BankAmt;
            cmdInsert.Parameters.Add("@FundAllocationAmt", System.Data.SqlDbType.Decimal).Value = tblPaymentAllocationTO.FundAllocationAmt;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public  int UpdateTblPaymentAllocation(TblPaymentAllocationTO tblPaymentAllocationTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblPaymentAllocationTO, cmdUpdate);
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

        public  int UpdateTblPaymentAllocation(TblPaymentAllocationTO tblPaymentAllocationTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblPaymentAllocationTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblPaymentAllocationTO tblPaymentAllocationTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblPaymentAllocation] SET " + 
            "  [idPaymentAllocation] = @IdPaymentAllocation" +
            " ,[createdBy]= @CreatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[isLatestEntry]= @IsLatestEntry" +
            " ,[bankAmt]= @BankAmt" +
            " ,[fundAllocationAmt] = @FundAllocationAmt" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdPaymentAllocation", System.Data.SqlDbType.Int).Value = tblPaymentAllocationTO.IdPaymentAllocation;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPaymentAllocationTO.CreatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPaymentAllocationTO.CreatedOn;
            cmdUpdate.Parameters.Add("@IsLatestEntry", System.Data.SqlDbType.Bit).Value = tblPaymentAllocationTO.IsLatestEntry;
            cmdUpdate.Parameters.Add("@BankAmt", System.Data.SqlDbType.Decimal).Value = tblPaymentAllocationTO.BankAmt;
            cmdUpdate.Parameters.Add("@FundAllocationAmt", System.Data.SqlDbType.Decimal).Value = tblPaymentAllocationTO.FundAllocationAmt;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblPaymentAllocation(Int32 idPaymentAllocation)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idPaymentAllocation, cmdDelete);
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

        public  int DeleteTblPaymentAllocation(Int32 idPaymentAllocation, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idPaymentAllocation, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idPaymentAllocation, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblPaymentAllocation] " +
            " WHERE idPaymentAllocation = " + idPaymentAllocation +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idPaymentAllocation", System.Data.SqlDbType.Int).Value = tblPaymentAllocationTO.IdPaymentAllocation;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
