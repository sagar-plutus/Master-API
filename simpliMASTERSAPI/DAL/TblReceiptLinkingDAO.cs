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
    public class TblReceiptLinkingDAO: ITblReceiptLinkingDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblReceiptLinkingDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblReceiptLinking]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public DataTable SelectAllTblReceiptLinking()
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

                //cmdSelect.Parameters.Add("@idReceiptLinking", System.Data.SqlDbType.Int).Value = tblReceiptLinkingTO.IdReceiptLinking;
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

        public  DataTable SelectTblReceiptLinking(Int32 idReceiptLinking)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idReceiptLinking = " + idReceiptLinking +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idReceiptLinking", System.Data.SqlDbType.Int).Value = tblReceiptLinkingTO.IdReceiptLinking;
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

        public  DataTable SelectAllTblReceiptLinking(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idReceiptLinking", System.Data.SqlDbType.Int).Value = tblReceiptLinkingTO.IdReceiptLinking;
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
        public  int InsertTblReceiptLinking(TblReceiptLinkingTO tblReceiptLinkingTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblReceiptLinkingTO, cmdInsert);
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

        public  int InsertTblReceiptLinking(TblReceiptLinkingTO tblReceiptLinkingTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblReceiptLinkingTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblReceiptLinkingTO tblReceiptLinkingTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblReceiptLinking]( " + 
            //"  [idReceiptLinking]" +
            " [brsBankStatementDtlId]" +
            " ,[bookingId]" +
            " ,[paySchId]" +
            " ,[refReceiptLinkingId]" +
            " ,[createdBy]" +
            " ,[returnBy]" +
            " ,[lastSplitedBy]" +
            " ,[createdOn]" +
            " ,[returnOn]" +
            " ,[lastSplitedOn]" +
            " ,[isSplited]" +
            " ,[isReturn]" +
            " ,[isActive]" +
            " ,[linkedAmt]" +
            " ,[actualLinkedAmt]" +
            " ,[splitedAmt]" +
            " )" +
" VALUES (" +
            //"  @IdReceiptLinking " +
            " @BrsBankStatementDtlId " +
            " ,@BookingId " +
            " ,@PaySchId " +
            " ,@RefReceiptLinkingId " +
            " ,@CreatedBy " +
            " ,@ReturnBy " +
            " ,@LastSplitedBy " +
            " ,@CreatedOn " +
            " ,@ReturnOn " +
            " ,@LastSplitedOn " +
            " ,@IsSplited " +
            " ,@IsReturn " +
            " ,@IsActive " +
            " ,@LinkedAmt " +
            " ,@ActualLinkedAmt " +
            " ,@SplitedAmt " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdReceiptLinking", System.Data.SqlDbType.Int).Value = tblReceiptLinkingTO.IdReceiptLinking;
            cmdInsert.Parameters.Add("@BrsBankStatementDtlId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblReceiptLinkingTO.BrsBankStatementDtlId);
            cmdInsert.Parameters.Add("@BookingId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblReceiptLinkingTO.BookingId);
            cmdInsert.Parameters.Add("@PaySchId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblReceiptLinkingTO.PaySchId);
            cmdInsert.Parameters.Add("@RefReceiptLinkingId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblReceiptLinkingTO.RefReceiptLinkingId);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblReceiptLinkingTO.CreatedBy);
            cmdInsert.Parameters.Add("@ReturnBy", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblReceiptLinkingTO.ReturnBy);
            cmdInsert.Parameters.Add("@LastSplitedBy", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblReceiptLinkingTO.LastSplitedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblReceiptLinkingTO.CreatedOn);
            cmdInsert.Parameters.Add("@ReturnOn", System.Data.SqlDbType.DateTime).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblReceiptLinkingTO.ReturnOn);
            cmdInsert.Parameters.Add("@LastSplitedOn", System.Data.SqlDbType.DateTime).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblReceiptLinkingTO.LastSplitedOn);
            cmdInsert.Parameters.Add("@IsSplited", System.Data.SqlDbType.Bit).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblReceiptLinkingTO.IsSplited);
            cmdInsert.Parameters.Add("@IsReturn", System.Data.SqlDbType.Bit).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblReceiptLinkingTO.IsReturn);
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Bit).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblReceiptLinkingTO.IsActive);
            cmdInsert.Parameters.Add("@LinkedAmt", System.Data.SqlDbType.Decimal).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblReceiptLinkingTO.LinkedAmt);
            cmdInsert.Parameters.Add("@ActualLinkedAmt", System.Data.SqlDbType.Decimal).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblReceiptLinkingTO.ActualLinkedAmt);
            cmdInsert.Parameters.Add("@SplitedAmt", System.Data.SqlDbType.Decimal).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblReceiptLinkingTO.SplitedAmt);
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public int UpdateTblReceiptLinking(TblReceiptLinkingTO tblReceiptLinkingTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblReceiptLinkingTO, cmdUpdate);
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

        public int UpdateTblReceiptLinking(TblReceiptLinkingTO tblReceiptLinkingTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblReceiptLinkingTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(TblReceiptLinkingTO tblReceiptLinkingTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblReceiptLinking] SET " + 
            "  [idReceiptLinking] = @IdReceiptLinking" +
            " ,[brsBankStatementDtlId]= @BrsBankStatementDtlId" +
            " ,[bookingId]= @BookingId" +
            " ,[paySchId]= @PaySchId" +
            " ,[refReceiptLinkingId]= @RefReceiptLinkingId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[returnBy]= @ReturnBy" +
            " ,[lastSplitedBy]= @LastSplitedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[returnOn]= @ReturnOn" +
            " ,[lastSplitedOn]= @LastSplitedOn" +
            " ,[isSplited]= @IsSplited" +
            " ,[isReturn]= @IsReturn" +
            " ,[isActive]= @IsActive" +
            " ,[linkedAmt]= @LinkedAmt" +
            " ,[actualLinkedAmt]= @ActualLinkedAmt" +
            " ,[splitedAmt] = @SplitedAmt" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdReceiptLinking", System.Data.SqlDbType.Int).Value = tblReceiptLinkingTO.IdReceiptLinking;
            cmdUpdate.Parameters.Add("@BrsBankStatementDtlId", System.Data.SqlDbType.Int).Value = tblReceiptLinkingTO.BrsBankStatementDtlId;
            cmdUpdate.Parameters.Add("@BookingId", System.Data.SqlDbType.Int).Value = tblReceiptLinkingTO.BookingId;
            cmdUpdate.Parameters.Add("@PaySchId", System.Data.SqlDbType.Int).Value = tblReceiptLinkingTO.PaySchId;
            cmdUpdate.Parameters.Add("@RefReceiptLinkingId", System.Data.SqlDbType.Int).Value = tblReceiptLinkingTO.RefReceiptLinkingId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblReceiptLinkingTO.CreatedBy;
            cmdUpdate.Parameters.Add("@ReturnBy", System.Data.SqlDbType.Int).Value = tblReceiptLinkingTO.ReturnBy;
            cmdUpdate.Parameters.Add("@LastSplitedBy", System.Data.SqlDbType.Int).Value = tblReceiptLinkingTO.LastSplitedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblReceiptLinkingTO.CreatedOn;
            cmdUpdate.Parameters.Add("@ReturnOn", System.Data.SqlDbType.DateTime).Value = tblReceiptLinkingTO.ReturnOn;
            cmdUpdate.Parameters.Add("@LastSplitedOn", System.Data.SqlDbType.DateTime).Value = tblReceiptLinkingTO.LastSplitedOn;
            cmdUpdate.Parameters.Add("@IsSplited", System.Data.SqlDbType.Bit).Value = tblReceiptLinkingTO.IsSplited;
            cmdUpdate.Parameters.Add("@IsReturn", System.Data.SqlDbType.Bit).Value = tblReceiptLinkingTO.IsReturn;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Bit).Value = tblReceiptLinkingTO.IsActive;
            cmdUpdate.Parameters.Add("@LinkedAmt", System.Data.SqlDbType.Decimal).Value = tblReceiptLinkingTO.LinkedAmt;
            cmdUpdate.Parameters.Add("@ActualLinkedAmt", System.Data.SqlDbType.Decimal).Value = tblReceiptLinkingTO.ActualLinkedAmt;
            cmdUpdate.Parameters.Add("@SplitedAmt", System.Data.SqlDbType.Decimal).Value = tblReceiptLinkingTO.SplitedAmt;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblReceiptLinking(Int32 idReceiptLinking)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idReceiptLinking, cmdDelete);
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

        public int DeleteTblReceiptLinking(Int32 idReceiptLinking, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idReceiptLinking, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idReceiptLinking, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblReceiptLinking] " +
            " WHERE idReceiptLinking = " + idReceiptLinking +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idReceiptLinking", System.Data.SqlDbType.Int).Value = tblReceiptLinkingTO.IdReceiptLinking;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion


        #region call tbl brs bank statement dtl

        public int UpdateReceiptStatementDtlStatus(TblBrsBankStatementDtlTO tblBrsBankStatementDtlTO,SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                cmdUpdate.CommandText = " UPDATE tblBrsBankStatementDtl set statusId=@statusId, " +
                                        " receiptTypeId=@receiptTypeId, " +
                                        " updatedBy =@updatedBy, " +
                                        " updatedOn =@updatedOn" +
                                        " WHERE idBrsBankStatementDtl=@idBrsBankStatementDtl ";
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@statusId", System.Data.SqlDbType.Int).Value = tblBrsBankStatementDtlTO.StatusId;
                cmdUpdate.Parameters.Add("@idBrsBankStatementDtl", System.Data.SqlDbType.Int).Value = tblBrsBankStatementDtlTO.IdBrsBankStatementDtl;

                cmdUpdate.Parameters.Add("@receiptTypeId", System.Data.SqlDbType.Int).Value = tblBrsBankStatementDtlTO.ReceiptTypeId;
                cmdUpdate.Parameters.Add("@updatedBy", System.Data.SqlDbType.Int).Value = tblBrsBankStatementDtlTO.UpdatedBy;
                cmdUpdate.Parameters.Add("@updatedOn", System.Data.SqlDbType.DateTime).Value = tblBrsBankStatementDtlTO.UpdatedOn;

                return cmdUpdate.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }


        public DataTable SelectAllReceiptStatementDtl()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = @"   select * from tblBrsBankStatementDtl brsBankStatementDtl
                                             LEFT JOIN tblBrsBankStatement BrsBankStatement on BrsBankStatement.idBrsBankStatement=brsBankStatementDtl.brsBankStatementId
                                             LEFT JOIN tblBrsTemplateDtl brsTemplateDtl
                                             ON brsTemplateDtl.brsTemplateId = BrsBankStatement.templateId and brsBankStatementDtl.brsTemplateDtlId=brsTemplateDtl.idBrsTemplateDtl
                                            LEFT JOIN tblUser tblUser on tblUser.idUser = BrsBankStatement.createdBy
                                            LEFT JOIN tblFinLedger FinLedger on FinLedger.idFinLedger = BrsBankStatement.finLedgerId  where isnull(accImportVoucherEntryId,0)=0  ";

                //if(fromDate!=DateTime.MinValue && toDate != DateTime.MinValue)
                //{
                //    cmdSelect.CommandText += " AND brsBankStatementRecordDate between  @fromDate AND @toDate ";
                //    cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = fromDate;
                //    cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = toDate;
                //}

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                

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

        #endregion
    }
}
