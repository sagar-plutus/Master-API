using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.Models;
using ODLMWebAPI.DAL.Interfaces;
namespace ODLMWebAPI.DAL
{
    public class TblReceiptChargesDAO: ITblReceiptChargesDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblReceiptChargesDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblReceiptCharges]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public DataTable SelectAllTblReceiptCharges()
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

                //cmdSelect.Parameters.Add("@idReceiptCharges", System.Data.SqlDbType.Int).Value = tblReceiptChargesTO.IdReceiptCharges;
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

        public  DataTable SelectTblReceiptCharges(Int32 idReceiptCharges)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idReceiptCharges = " + idReceiptCharges +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idReceiptCharges", System.Data.SqlDbType.Int).Value = tblReceiptChargesTO.IdReceiptCharges;
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

        public  DataTable SelectAllTblReceiptCharges(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idReceiptCharges", System.Data.SqlDbType.Int).Value = tblReceiptChargesTO.IdReceiptCharges;
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
        public  int InsertTblReceiptCharges(TblReceiptChargesTO tblReceiptChargesTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblReceiptChargesTO, cmdInsert);
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

        public int InsertTblReceiptCharges(TblReceiptChargesTO tblReceiptChargesTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblReceiptChargesTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblReceiptChargesTO tblReceiptChargesTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblReceiptCharges]( " + 
            "  [idReceiptCharges]" +
            " ,[brsBankStatementDtlId]" +
            " ,[otherChargesTypeId]" +
            " ,[amount]" +
            " )" +
" VALUES (" +
            "  @IdReceiptCharges " +
            " ,@BrsBankStatementDtlId " +
            " ,@OtherChargesTypeId " +
            " ,@Amount " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdReceiptCharges", System.Data.SqlDbType.Int).Value = tblReceiptChargesTO.IdReceiptCharges;
            cmdInsert.Parameters.Add("@BrsBankStatementDtlId", System.Data.SqlDbType.Int).Value = tblReceiptChargesTO.BrsBankStatementDtlId;
            cmdInsert.Parameters.Add("@OtherChargesTypeId", System.Data.SqlDbType.Int).Value = tblReceiptChargesTO.OtherChargesTypeId;
            cmdInsert.Parameters.Add("@Amount", System.Data.SqlDbType.Decimal).Value = tblReceiptChargesTO.Amount;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public  int UpdateTblReceiptCharges(TblReceiptChargesTO tblReceiptChargesTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblReceiptChargesTO, cmdUpdate);
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

        public  int UpdateTblReceiptCharges(TblReceiptChargesTO tblReceiptChargesTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblReceiptChargesTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblReceiptChargesTO tblReceiptChargesTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblReceiptCharges] SET " + 
            "  [idReceiptCharges] = @IdReceiptCharges" +
            " ,[brsBankStatementDtlId]= @BrsBankStatementDtlId" +
            " ,[otherChargesTypeId]= @OtherChargesTypeId" +
            " ,[amount] = @Amount" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdReceiptCharges", System.Data.SqlDbType.Int).Value = tblReceiptChargesTO.IdReceiptCharges;
            cmdUpdate.Parameters.Add("@BrsBankStatementDtlId", System.Data.SqlDbType.Int).Value = tblReceiptChargesTO.BrsBankStatementDtlId;
            cmdUpdate.Parameters.Add("@OtherChargesTypeId", System.Data.SqlDbType.Int).Value = tblReceiptChargesTO.OtherChargesTypeId;
            cmdUpdate.Parameters.Add("@Amount", System.Data.SqlDbType.Decimal).Value = tblReceiptChargesTO.Amount;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblReceiptCharges(Int32 idReceiptCharges)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idReceiptCharges, cmdDelete);
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

        public  int DeleteTblReceiptCharges(Int32 idReceiptCharges, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idReceiptCharges, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idReceiptCharges, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblReceiptCharges] " +
            " WHERE idReceiptCharges = " + idReceiptCharges +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idReceiptCharges", System.Data.SqlDbType.Int).Value = tblReceiptChargesTO.IdReceiptCharges;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
