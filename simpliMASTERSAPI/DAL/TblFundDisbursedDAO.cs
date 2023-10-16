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
    public class TblFundDisbursedDAO: ITblFundDisbursedDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblFundDisbursedDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblFundDisbursed]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  DataTable SelectAllTblFundDisbursed()
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

                //cmdSelect.Parameters.Add("@idFundDisbursed", System.Data.SqlDbType.Int).Value = tblFundDisbursedTO.IdFundDisbursed;
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

        public  DataTable SelectTblFundDisbursed(Int32 idFundDisbursed)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idFundDisbursed = " + idFundDisbursed +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idFundDisbursed", System.Data.SqlDbType.Int).Value = tblFundDisbursedTO.IdFundDisbursed;
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

        public  DataTable SelectAllTblFundDisbursed(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idFundDisbursed", System.Data.SqlDbType.Int).Value = tblFundDisbursedTO.IdFundDisbursed;
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
        public  int InsertTblFundDisbursed(TblFundDisbursedTO tblFundDisbursedTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblFundDisbursedTO, cmdInsert);
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

        public  int InsertTblFundDisbursed(TblFundDisbursedTO tblFundDisbursedTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblFundDisbursedTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblFundDisbursedTO tblFundDisbursedTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblFundDisbursed]( " + 
            "  [idFundDisbursed]" +
            " ,[paymentAllocationId]" +
            " ,[payTypeId]" +
            " ,[createdBy]" +
            " ,[createdOn]" +
            " ,[disbursedAmt]" +
            " ,[balanceDisbursedAmt]" +
            " ,[balanceAmt]" +
            " )" +
" VALUES (" +
            "  @IdFundDisbursed " +
            " ,@PaymentAllocationId " +
            " ,@PayTypeId " +
            " ,@CreatedBy " +
            " ,@CreatedOn " +
            " ,@DisbursedAmt " +
            " ,@BalanceDisbursedAmt " +
            " ,@BalanceAmt " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdFundDisbursed", System.Data.SqlDbType.Int).Value = tblFundDisbursedTO.IdFundDisbursed;
            cmdInsert.Parameters.Add("@PaymentAllocationId", System.Data.SqlDbType.Int).Value = tblFundDisbursedTO.PaymentAllocationId;
            cmdInsert.Parameters.Add("@PayTypeId", System.Data.SqlDbType.Int).Value = tblFundDisbursedTO.PayTypeId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblFundDisbursedTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblFundDisbursedTO.CreatedOn;
            cmdInsert.Parameters.Add("@DisbursedAmt", System.Data.SqlDbType.Decimal).Value = tblFundDisbursedTO.DisbursedAmt;
            cmdInsert.Parameters.Add("@BalanceDisbursedAmt", System.Data.SqlDbType.Decimal).Value = tblFundDisbursedTO.BalanceDisbursedAmt;
            cmdInsert.Parameters.Add("@BalanceAmt", System.Data.SqlDbType.Decimal).Value = tblFundDisbursedTO.BalanceAmt;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public  int UpdateTblFundDisbursed(TblFundDisbursedTO tblFundDisbursedTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblFundDisbursedTO, cmdUpdate);
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

        public  int UpdateTblFundDisbursed(TblFundDisbursedTO tblFundDisbursedTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblFundDisbursedTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblFundDisbursedTO tblFundDisbursedTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblFundDisbursed] SET " + 
            "  [idFundDisbursed] = @IdFundDisbursed" +
            " ,[paymentAllocationId]= @PaymentAllocationId" +
            " ,[payTypeId]= @PayTypeId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[disbursedAmt]= @DisbursedAmt" +
            " ,[balanceDisbursedAmt]= @BalanceDisbursedAmt" +
            " ,[balanceAmt] = @BalanceAmt" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdFundDisbursed", System.Data.SqlDbType.Int).Value = tblFundDisbursedTO.IdFundDisbursed;
            cmdUpdate.Parameters.Add("@PaymentAllocationId", System.Data.SqlDbType.Int).Value = tblFundDisbursedTO.PaymentAllocationId;
            cmdUpdate.Parameters.Add("@PayTypeId", System.Data.SqlDbType.Int).Value = tblFundDisbursedTO.PayTypeId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblFundDisbursedTO.CreatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblFundDisbursedTO.CreatedOn;
            cmdUpdate.Parameters.Add("@DisbursedAmt", System.Data.SqlDbType.Decimal).Value = tblFundDisbursedTO.DisbursedAmt;
            cmdUpdate.Parameters.Add("@BalanceDisbursedAmt", System.Data.SqlDbType.Decimal).Value = tblFundDisbursedTO.BalanceDisbursedAmt;
            cmdUpdate.Parameters.Add("@BalanceAmt", System.Data.SqlDbType.Decimal).Value = tblFundDisbursedTO.BalanceAmt;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblFundDisbursed(Int32 idFundDisbursed)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idFundDisbursed, cmdDelete);
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

        public  int DeleteTblFundDisbursed(Int32 idFundDisbursed, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idFundDisbursed, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idFundDisbursed, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblFundDisbursed] " +
            " WHERE idFundDisbursed = " + idFundDisbursed +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idFundDisbursed", System.Data.SqlDbType.Int).Value = tblFundDisbursedTO.IdFundDisbursed;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
