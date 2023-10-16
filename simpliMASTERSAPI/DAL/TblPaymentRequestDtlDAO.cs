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
    public class TblPaymentRequestDtlDAO: ITblPaymentRequestDtlDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblPaymentRequestDtlDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblPaymentRequestDtl]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  DataTable SelectAllTblPaymentRequestDtl()
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

                //cmdSelect.Parameters.Add("@idPayReqDtl", System.Data.SqlDbType.Int).Value = tblPaymentRequestDtlTO.IdPayReqDtl;
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

        public  DataTable SelectTblPaymentRequestDtl(Int32 idPayReqDtl)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idPayReqDtl = " + idPayReqDtl +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPayReqDtl", System.Data.SqlDbType.Int).Value = tblPaymentRequestDtlTO.IdPayReqDtl;
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

        public DataTable SelectAllTblPaymentRequestDtl(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPayReqDtl", System.Data.SqlDbType.Int).Value = tblPaymentRequestDtlTO.IdPayReqDtl;
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
        public int InsertTblPaymentRequestDtl(TblPaymentRequestDtlTO tblPaymentRequestDtlTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblPaymentRequestDtlTO, cmdInsert);
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

        public  int InsertTblPaymentRequestDtl(TblPaymentRequestDtlTO tblPaymentRequestDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblPaymentRequestDtlTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblPaymentRequestDtlTO tblPaymentRequestDtlTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblPaymentRequestDtl]( " + 
            "  [idPayReqDtl]" +
            " ,[payReqId]" +
            " ,[payTypeId]" +
            " ,[refNo]" +
            " ,[txnTypeId]" +
            " ,[statusId]" +
            " ,[supplierId]" +
            " ,[userId]" +
            " ,[expenseId]" +
            " ,[advanceId]" +
            " ,[departmentId]" +
            " ,[payBankId]" +
            " ,[paymentTypeId]" +
            " ,[payById]" +
            " ,[paymentDate]" +
            " ,[payOn]" +
            " ,[amount]" +
            " ,[grnId]" +
            " ,[paymentNarration]" +
            " )" +
" VALUES (" +
            "  @IdPayReqDtl " +
            " ,@PayReqId " +
            " ,@PayTypeId " +
            " ,@RefNo " +
            " ,@TxnTypeId " +
            " ,@StatusId " +
            " ,@SupplierId " +
            " ,@UserId " +
            " ,@ExpenseId " +
            " ,@AdvanceId " +
            " ,@DepartmentId " +
            " ,@PayBankId " +
            " ,@PaymentTypeId " +
            " ,@PayById " +
            " ,@PaymentDate " +
            " ,@PayOn " +
            " ,@Amount " +
            " ,@GrnId " +
            " ,@PaymentNarration " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdPayReqDtl", System.Data.SqlDbType.Int).Value = tblPaymentRequestDtlTO.IdPayReqDtl;
            cmdInsert.Parameters.Add("@PayReqId", System.Data.SqlDbType.Int).Value = tblPaymentRequestDtlTO.PayReqId;
            cmdInsert.Parameters.Add("@PayTypeId", System.Data.SqlDbType.Int).Value = tblPaymentRequestDtlTO.PayTypeId;
            cmdInsert.Parameters.Add("@RefNo", System.Data.SqlDbType.Int).Value = tblPaymentRequestDtlTO.RefNo;
            cmdInsert.Parameters.Add("@TxnTypeId", System.Data.SqlDbType.Int).Value = tblPaymentRequestDtlTO.TxnTypeId;
            cmdInsert.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblPaymentRequestDtlTO.StatusId;
            cmdInsert.Parameters.Add("@SupplierId", System.Data.SqlDbType.Int).Value = tblPaymentRequestDtlTO.SupplierId;
            cmdInsert.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = tblPaymentRequestDtlTO.UserId;
            cmdInsert.Parameters.Add("@ExpenseId", System.Data.SqlDbType.Int).Value = tblPaymentRequestDtlTO.ExpenseId;
            cmdInsert.Parameters.Add("@AdvanceId", System.Data.SqlDbType.Int).Value = tblPaymentRequestDtlTO.AdvanceId;
            cmdInsert.Parameters.Add("@DepartmentId", System.Data.SqlDbType.Int).Value = tblPaymentRequestDtlTO.DepartmentId;
            cmdInsert.Parameters.Add("@PayBankId", System.Data.SqlDbType.Int).Value = tblPaymentRequestDtlTO.PayBankId;
            cmdInsert.Parameters.Add("@PaymentTypeId", System.Data.SqlDbType.Int).Value = tblPaymentRequestDtlTO.PaymentTypeId;
            cmdInsert.Parameters.Add("@PayById", System.Data.SqlDbType.Int).Value = tblPaymentRequestDtlTO.PayById;
            cmdInsert.Parameters.Add("@PaymentDate", System.Data.SqlDbType.DateTime).Value = tblPaymentRequestDtlTO.PaymentDate;
            cmdInsert.Parameters.Add("@PayOn", System.Data.SqlDbType.DateTime).Value = tblPaymentRequestDtlTO.PayOn;
            cmdInsert.Parameters.Add("@Amount", System.Data.SqlDbType.Decimal).Value = tblPaymentRequestDtlTO.Amount;
            cmdInsert.Parameters.Add("@GrnId", System.Data.SqlDbType.BigInt).Value = tblPaymentRequestDtlTO.GrnId;
            cmdInsert.Parameters.Add("@PaymentNarration", System.Data.SqlDbType.VarChar).Value = tblPaymentRequestDtlTO.PaymentNarration;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public  int UpdateTblPaymentRequestDtl(TblPaymentRequestDtlTO tblPaymentRequestDtlTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblPaymentRequestDtlTO, cmdUpdate);
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

        public  int UpdateTblPaymentRequestDtl(TblPaymentRequestDtlTO tblPaymentRequestDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblPaymentRequestDtlTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblPaymentRequestDtlTO tblPaymentRequestDtlTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblPaymentRequestDtl] SET " + 
            "  [idPayReqDtl] = @IdPayReqDtl" +
            " ,[payReqId]= @PayReqId" +
            " ,[payTypeId]= @PayTypeId" +
            " ,[refNo]= @RefNo" +
            " ,[txnTypeId]= @TxnTypeId" +
            " ,[statusId]= @StatusId" +
            " ,[supplierId]= @SupplierId" +
            " ,[userId]= @UserId" +
            " ,[expenseId]= @ExpenseId" +
            " ,[advanceId]= @AdvanceId" +
            " ,[departmentId]= @DepartmentId" +
            " ,[payBankId]= @PayBankId" +
            " ,[paymentTypeId]= @PaymentTypeId" +
            " ,[payById]= @PayById" +
            " ,[paymentDate]= @PaymentDate" +
            " ,[payOn]= @PayOn" +
            " ,[amount]= @Amount" +
            " ,[grnId]= @GrnId" +
            " ,[paymentNarration] = @PaymentNarration" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdPayReqDtl", System.Data.SqlDbType.Int).Value = tblPaymentRequestDtlTO.IdPayReqDtl;
            cmdUpdate.Parameters.Add("@PayReqId", System.Data.SqlDbType.Int).Value = tblPaymentRequestDtlTO.PayReqId;
            cmdUpdate.Parameters.Add("@PayTypeId", System.Data.SqlDbType.Int).Value = tblPaymentRequestDtlTO.PayTypeId;
            cmdUpdate.Parameters.Add("@RefNo", System.Data.SqlDbType.Int).Value = tblPaymentRequestDtlTO.RefNo;
            cmdUpdate.Parameters.Add("@TxnTypeId", System.Data.SqlDbType.Int).Value = tblPaymentRequestDtlTO.TxnTypeId;
            cmdUpdate.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblPaymentRequestDtlTO.StatusId;
            cmdUpdate.Parameters.Add("@SupplierId", System.Data.SqlDbType.Int).Value = tblPaymentRequestDtlTO.SupplierId;
            cmdUpdate.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = tblPaymentRequestDtlTO.UserId;
            cmdUpdate.Parameters.Add("@ExpenseId", System.Data.SqlDbType.Int).Value = tblPaymentRequestDtlTO.ExpenseId;
            cmdUpdate.Parameters.Add("@AdvanceId", System.Data.SqlDbType.Int).Value = tblPaymentRequestDtlTO.AdvanceId;
            cmdUpdate.Parameters.Add("@DepartmentId", System.Data.SqlDbType.Int).Value = tblPaymentRequestDtlTO.DepartmentId;
            cmdUpdate.Parameters.Add("@PayBankId", System.Data.SqlDbType.Int).Value = tblPaymentRequestDtlTO.PayBankId;
            cmdUpdate.Parameters.Add("@PaymentTypeId", System.Data.SqlDbType.Int).Value = tblPaymentRequestDtlTO.PaymentTypeId;
            cmdUpdate.Parameters.Add("@PayById", System.Data.SqlDbType.Int).Value = tblPaymentRequestDtlTO.PayById;
            cmdUpdate.Parameters.Add("@PaymentDate", System.Data.SqlDbType.DateTime).Value = tblPaymentRequestDtlTO.PaymentDate;
            cmdUpdate.Parameters.Add("@PayOn", System.Data.SqlDbType.DateTime).Value = tblPaymentRequestDtlTO.PayOn;
            cmdUpdate.Parameters.Add("@Amount", System.Data.SqlDbType.Decimal).Value = tblPaymentRequestDtlTO.Amount;
            cmdUpdate.Parameters.Add("@GrnId", System.Data.SqlDbType.BigInt).Value = tblPaymentRequestDtlTO.GrnId;
            cmdUpdate.Parameters.Add("@PaymentNarration", System.Data.SqlDbType.VarChar).Value = tblPaymentRequestDtlTO.PaymentNarration;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblPaymentRequestDtl(Int32 idPayReqDtl)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idPayReqDtl, cmdDelete);
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

        public  int DeleteTblPaymentRequestDtl(Int32 idPayReqDtl, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idPayReqDtl, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int32 idPayReqDtl, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblPaymentRequestDtl] " +
            " WHERE idPayReqDtl = " + idPayReqDtl +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idPayReqDtl", System.Data.SqlDbType.Int).Value = tblPaymentRequestDtlTO.IdPayReqDtl;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
