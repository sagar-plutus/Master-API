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
    public class TblBankAmountDAO: ITblBankAmountDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblBankAmountDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblBankAmount]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  DataTable SelectAllTblBankAmount()
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

                //cmdSelect.Parameters.Add("@idBankAmount", System.Data.SqlDbType.Int).Value = tblBankAmountTO.IdBankAmount;
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

        public  DataTable SelectTblBankAmount(Int32 idBankAmount)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idBankAmount = " + idBankAmount +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idBankAmount", System.Data.SqlDbType.Int).Value = tblBankAmountTO.IdBankAmount;
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

        public  DataTable SelectAllTblBankAmount(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idBankAmount", System.Data.SqlDbType.Int).Value = tblBankAmountTO.IdBankAmount;
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
        public  int InsertTblBankAmount(TblBankAmountTO tblBankAmountTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblBankAmountTO, cmdInsert);
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

        public int InsertTblBankAmount(TblBankAmountTO tblBankAmountTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblBankAmountTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblBankAmountTO tblBankAmountTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblBankAmount]( " + 
            "  [idBankAmount]" +
            " ,[bankLedgerId]" +
            " ,[amountTakenBy]" +
            " ,[paymentAllocationId]" +
            " ,[createdBy]" +
            " ,[amountTakenOn]" +
            " ,[createdOn]" +
            " ,[isLatestEntry]" +
            " ,[bankAmt]" +
            " )" +
" VALUES (" +
            "  @IdBankAmount " +
            " ,@BankLedgerId " +
            " ,@AmountTakenBy " +
            " ,@PaymentAllocationId " +
            " ,@CreatedBy " +
            " ,@AmountTakenOn " +
            " ,@CreatedOn " +
            " ,@IsLatestEntry " +
            " ,@BankAmt " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdBankAmount", System.Data.SqlDbType.Int).Value = tblBankAmountTO.IdBankAmount;
            cmdInsert.Parameters.Add("@BankLedgerId", System.Data.SqlDbType.Int).Value = tblBankAmountTO.BankLedgerId;
            cmdInsert.Parameters.Add("@AmountTakenBy", System.Data.SqlDbType.Int).Value = tblBankAmountTO.AmountTakenBy;
            cmdInsert.Parameters.Add("@PaymentAllocationId", System.Data.SqlDbType.Int).Value = tblBankAmountTO.PaymentAllocationId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblBankAmountTO.CreatedBy;
            cmdInsert.Parameters.Add("@AmountTakenOn", System.Data.SqlDbType.DateTime).Value = tblBankAmountTO.AmountTakenOn;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblBankAmountTO.CreatedOn;
            cmdInsert.Parameters.Add("@IsLatestEntry", System.Data.SqlDbType.Bit).Value = tblBankAmountTO.IsLatestEntry;
            cmdInsert.Parameters.Add("@BankAmt", System.Data.SqlDbType.Decimal).Value = tblBankAmountTO.BankAmt;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public  int UpdateTblBankAmount(TblBankAmountTO tblBankAmountTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblBankAmountTO, cmdUpdate);
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

        public int UpdateTblBankAmount(TblBankAmountTO tblBankAmountTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblBankAmountTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(TblBankAmountTO tblBankAmountTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblBankAmount] SET " + 
            "  [idBankAmount] = @IdBankAmount" +
            " ,[bankLedgerId]= @BankLedgerId" +
            " ,[amountTakenBy]= @AmountTakenBy" +
            " ,[paymentAllocationId]= @PaymentAllocationId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[amountTakenOn]= @AmountTakenOn" +
            " ,[createdOn]= @CreatedOn" +
            " ,[isLatestEntry]= @IsLatestEntry" +
            " ,[bankAmt] = @BankAmt" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdBankAmount", System.Data.SqlDbType.Int).Value = tblBankAmountTO.IdBankAmount;
            cmdUpdate.Parameters.Add("@BankLedgerId", System.Data.SqlDbType.Int).Value = tblBankAmountTO.BankLedgerId;
            cmdUpdate.Parameters.Add("@AmountTakenBy", System.Data.SqlDbType.Int).Value = tblBankAmountTO.AmountTakenBy;
            cmdUpdate.Parameters.Add("@PaymentAllocationId", System.Data.SqlDbType.Int).Value = tblBankAmountTO.PaymentAllocationId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblBankAmountTO.CreatedBy;
            cmdUpdate.Parameters.Add("@AmountTakenOn", System.Data.SqlDbType.DateTime).Value = tblBankAmountTO.AmountTakenOn;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblBankAmountTO.CreatedOn;
            cmdUpdate.Parameters.Add("@IsLatestEntry", System.Data.SqlDbType.Bit).Value = tblBankAmountTO.IsLatestEntry;
            cmdUpdate.Parameters.Add("@BankAmt", System.Data.SqlDbType.Decimal).Value = tblBankAmountTO.BankAmt;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblBankAmount(Int32 idBankAmount)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idBankAmount, cmdDelete);
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

        public int DeleteTblBankAmount(Int32 idBankAmount, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idBankAmount, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int32 idBankAmount, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblBankAmount] " +
            " WHERE idBankAmount = " + idBankAmount +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idBankAmount", System.Data.SqlDbType.Int).Value = tblBankAmountTO.IdBankAmount;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
