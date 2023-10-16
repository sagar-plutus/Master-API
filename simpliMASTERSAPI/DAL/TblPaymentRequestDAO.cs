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
    public class TblPaymentRequestDAO: ITblPaymentRequestDAO
    {

        private readonly IConnectionString _iConnectionString;
        public TblPaymentRequestDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblPaymentRequest]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  DataTable SelectAllTblPaymentRequest()
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

                //cmdSelect.Parameters.Add("@idPayReq", System.Data.SqlDbType.Int).Value = tblPaymentRequestTO.IdPayReq;
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

        public  DataTable SelectTblPaymentRequest(Int32 idPayReq)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idPayReq = " + idPayReq +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPayReq", System.Data.SqlDbType.Int).Value = tblPaymentRequestTO.IdPayReq;
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

        public  DataTable SelectAllTblPaymentRequest(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPayReq", System.Data.SqlDbType.Int).Value = tblPaymentRequestTO.IdPayReq;
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
        public  int InsertTblPaymentRequest(TblPaymentRequestTO tblPaymentRequestTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblPaymentRequestTO, cmdInsert);
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

        public  int InsertTblPaymentRequest(TblPaymentRequestTO tblPaymentRequestTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblPaymentRequestTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblPaymentRequestTO tblPaymentRequestTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblPaymentRequest]( " + 
            "  [idPayReq]" +
            " ,[paymentAllocationId]" +
            " ,[payTypeId]" +
            " ,[refNo]" +
            " ,[txnTypeId]" +
            " ,[createdBy]" +
            " ,[createdOn]" +
            " ,[amount]" +
            " )" +
" VALUES (" +
            "  @IdPayReq " +
            " ,@PaymentAllocationId " +
            " ,@PayTypeId " +
            " ,@RefNo " +
            " ,@TxnTypeId " +
            " ,@CreatedBy " +
            " ,@CreatedOn " +
            " ,@Amount " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdPayReq", System.Data.SqlDbType.Int).Value = tblPaymentRequestTO.IdPayReq;
            cmdInsert.Parameters.Add("@PaymentAllocationId", System.Data.SqlDbType.Int).Value = tblPaymentRequestTO.PaymentAllocationId;
            cmdInsert.Parameters.Add("@PayTypeId", System.Data.SqlDbType.Int).Value = tblPaymentRequestTO.PayTypeId;
            cmdInsert.Parameters.Add("@RefNo", System.Data.SqlDbType.Int).Value = tblPaymentRequestTO.RefNo;
            cmdInsert.Parameters.Add("@TxnTypeId", System.Data.SqlDbType.Int).Value = tblPaymentRequestTO.TxnTypeId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPaymentRequestTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPaymentRequestTO.CreatedOn;
            cmdInsert.Parameters.Add("@Amount", System.Data.SqlDbType.Decimal).Value = tblPaymentRequestTO.Amount;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public  int UpdateTblPaymentRequest(TblPaymentRequestTO tblPaymentRequestTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblPaymentRequestTO, cmdUpdate);
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

        public  int UpdateTblPaymentRequest(TblPaymentRequestTO tblPaymentRequestTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblPaymentRequestTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(TblPaymentRequestTO tblPaymentRequestTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblPaymentRequest] SET " + 
            "  [idPayReq] = @IdPayReq" +
            " ,[paymentAllocationId]= @PaymentAllocationId" +
            " ,[payTypeId]= @PayTypeId" +
            " ,[refNo]= @RefNo" +
            " ,[txnTypeId]= @TxnTypeId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[amount] = @Amount" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdPayReq", System.Data.SqlDbType.Int).Value = tblPaymentRequestTO.IdPayReq;
            cmdUpdate.Parameters.Add("@PaymentAllocationId", System.Data.SqlDbType.Int).Value = tblPaymentRequestTO.PaymentAllocationId;
            cmdUpdate.Parameters.Add("@PayTypeId", System.Data.SqlDbType.Int).Value = tblPaymentRequestTO.PayTypeId;
            cmdUpdate.Parameters.Add("@RefNo", System.Data.SqlDbType.Int).Value = tblPaymentRequestTO.RefNo;
            cmdUpdate.Parameters.Add("@TxnTypeId", System.Data.SqlDbType.Int).Value = tblPaymentRequestTO.TxnTypeId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPaymentRequestTO.CreatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPaymentRequestTO.CreatedOn;
            cmdUpdate.Parameters.Add("@Amount", System.Data.SqlDbType.Decimal).Value = tblPaymentRequestTO.Amount;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblPaymentRequest(Int32 idPayReq)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idPayReq, cmdDelete);
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

        public int DeleteTblPaymentRequest(Int32 idPayReq, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idPayReq, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idPayReq, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblPaymentRequest] " +
            " WHERE idPayReq = " + idPayReq +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idPayReq", System.Data.SqlDbType.Int).Value = tblPaymentRequestTO.IdPayReq;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
