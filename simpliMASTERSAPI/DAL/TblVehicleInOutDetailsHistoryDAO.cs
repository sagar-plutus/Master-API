using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using simpliMASTERSAPI.TO;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.BL.Interfaces;
using simpliMASTERSAPI.DAL.Interfaces;

namespace simpliMASTERSAPI.DAL
{
    public class TblVehicleInOutDetailsHistoryDAO : ITblVehicleInOutDetailsHistoryDAO
    {
        #region Methods

        private readonly IConnectionString _iConnectionString;
        public TblVehicleInOutDetailsHistoryDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblVehicleInOutDetailsHistory]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblVehicleInOutDetailsHistoryTO> SelectAllTblVehicleInOutDetailsHistory()
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

                //cmdSelect.Parameters.Add("@idTblVehicleInOutDetailsHistory", System.Data.SqlDbType.Int).Value = tblVehicleInOutDetailsHistoryTO.IdTblVehicleInOutDetailsHistory;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblVehicleInOutDetailsHistoryTO> list = ConvertDTToList(reader);
                return list;
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

        public TblVehicleInOutDetailsHistoryTO SelectTblVehicleInOutDetailsHistory(Int32 idTblVehicleInOutDetailsHistory)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idTblVehicleInOutDetailsHistory = " + idTblVehicleInOutDetailsHistory +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idTblVehicleInOutDetailsHistory", System.Data.SqlDbType.Int).Value = tblVehicleInOutDetailsHistoryTO.IdTblVehicleInOutDetailsHistory;
                //cmdSelect.Parameters.Add("@idTblVehicleInOutDetailsHistory", System.Data.SqlDbType.Int).Value = tblVehicleInOutDetailsHistoryTO.IdTblVehicleInOutDetailsHistory;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblVehicleInOutDetailsHistoryTO> list = ConvertDTToList(reader);
                if (list != null && list.Count > 0)
                {
                    return list[0];
                }
                return null;
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

        public  List<TblVehicleInOutDetailsHistoryTO> SelectAllTblVehicleInOutDetailsHistory(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idTblVehicleInOutDetailsHistory", System.Data.SqlDbType.Int).Value = tblVehicleInOutDetailsHistoryTO.IdTblVehicleInOutDetailsHistory;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblVehicleInOutDetailsHistoryTO> list = ConvertDTToList(reader);                
                return list;
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

        public List<TblVehicleInOutDetailsHistoryTO> ConvertDTToList(SqlDataReader tblVehicleInOutDetailsHistoryTODT)
        {
            List<TblVehicleInOutDetailsHistoryTO> tblVehicleInOutDetailsHistoryTOList = new List<TblVehicleInOutDetailsHistoryTO>();
            if (tblVehicleInOutDetailsHistoryTODT != null)
            {
                while (tblVehicleInOutDetailsHistoryTODT.Read())
                { 
                    TblVehicleInOutDetailsHistoryTO tblVehicleInOutDetailsHistoryTONew = new TblVehicleInOutDetailsHistoryTO();
                    if (tblVehicleInOutDetailsHistoryTODT["idTblVehicleInOutDetailsHistory"] != DBNull.Value)
                        tblVehicleInOutDetailsHistoryTONew.IdTblVehicleInOutDetailsHistory = Convert.ToInt32(tblVehicleInOutDetailsHistoryTODT["idTblVehicleInOutDetailsHistory"].ToString());
                    if (tblVehicleInOutDetailsHistoryTODT["tblVehicleInOutDetailsId"] != DBNull.Value)
                        tblVehicleInOutDetailsHistoryTONew.TblVehicleInOutDetailsId = Convert.ToInt32(tblVehicleInOutDetailsHistoryTODT["tblVehicleInOutDetailsId"].ToString());
                    if (tblVehicleInOutDetailsHistoryTODT["moduleId"] != DBNull.Value)
                        tblVehicleInOutDetailsHistoryTONew.ModuleId = Convert.ToInt32(tblVehicleInOutDetailsHistoryTODT["moduleId"].ToString());
                    if (tblVehicleInOutDetailsHistoryTODT["transactionTypeId"] != DBNull.Value)
                        tblVehicleInOutDetailsHistoryTONew.TransactionTypeId = Convert.ToInt32(tblVehicleInOutDetailsHistoryTODT["transactionTypeId"].ToString());
                    if (tblVehicleInOutDetailsHistoryTODT["transactionStatusId"] != DBNull.Value)
                        tblVehicleInOutDetailsHistoryTONew.TransactionStatusId = Convert.ToInt32(tblVehicleInOutDetailsHistoryTODT["transactionStatusId"].ToString());
                    if (tblVehicleInOutDetailsHistoryTODT["NextStatusId"] != DBNull.Value)
                        tblVehicleInOutDetailsHistoryTONew.NextStatusId = Convert.ToInt32(tblVehicleInOutDetailsHistoryTODT["NextStatusId"].ToString());
                    if (tblVehicleInOutDetailsHistoryTODT["partyId"] != DBNull.Value)
                        tblVehicleInOutDetailsHistoryTONew.PartyId = Convert.ToInt32(tblVehicleInOutDetailsHistoryTODT["partyId"].ToString());
                    if (tblVehicleInOutDetailsHistoryTODT["transporterId"] != DBNull.Value)
                        tblVehicleInOutDetailsHistoryTONew.TransporterId = Convert.ToInt32(tblVehicleInOutDetailsHistoryTODT["transporterId"].ToString());
                    if (tblVehicleInOutDetailsHistoryTODT["supervisorId"] != DBNull.Value)
                        tblVehicleInOutDetailsHistoryTONew.SupervisorId = Convert.ToInt32(tblVehicleInOutDetailsHistoryTODT["supervisorId"].ToString());
                    if (tblVehicleInOutDetailsHistoryTODT["nextExpectedActionId"] != DBNull.Value)
                        tblVehicleInOutDetailsHistoryTONew.NextExpectedActionId = Convert.ToInt32(tblVehicleInOutDetailsHistoryTODT["nextExpectedActionId"].ToString());
                    if (tblVehicleInOutDetailsHistoryTODT["isActive"] != DBNull.Value)
                        tblVehicleInOutDetailsHistoryTONew.IsActive = Convert.ToInt32(tblVehicleInOutDetailsHistoryTODT["isActive"].ToString());
                    if (tblVehicleInOutDetailsHistoryTODT["createdBy"] != DBNull.Value)
                        tblVehicleInOutDetailsHistoryTONew.CreatedBy = Convert.ToInt32(tblVehicleInOutDetailsHistoryTODT["createdBy"].ToString());
                    if (tblVehicleInOutDetailsHistoryTODT["updatedBy"] != DBNull.Value)
                        tblVehicleInOutDetailsHistoryTONew.UpdatedBy = Convert.ToInt32(tblVehicleInOutDetailsHistoryTODT["updatedBy"].ToString());
                    if (tblVehicleInOutDetailsHistoryTODT["transactionDate"] != DBNull.Value)
                        tblVehicleInOutDetailsHistoryTONew.TransactionDate = Convert.ToDateTime(tblVehicleInOutDetailsHistoryTODT["transactionDate"].ToString());
                    if (tblVehicleInOutDetailsHistoryTODT["transactionStatusDate"] != DBNull.Value)
                        tblVehicleInOutDetailsHistoryTONew.TransactionStatusDate = Convert.ToDateTime(tblVehicleInOutDetailsHistoryTODT["transactionStatusDate"].ToString());
                    if (tblVehicleInOutDetailsHistoryTODT["createdOn"] != DBNull.Value)
                        tblVehicleInOutDetailsHistoryTONew.CreatedOn = Convert.ToDateTime(tblVehicleInOutDetailsHistoryTODT["createdOn"].ToString());
                    if (tblVehicleInOutDetailsHistoryTODT["updatedOn"] != DBNull.Value)
                        tblVehicleInOutDetailsHistoryTONew.UpdatedOn = Convert.ToDateTime(tblVehicleInOutDetailsHistoryTODT["updatedOn"].ToString());
                    if (tblVehicleInOutDetailsHistoryTODT["vehicleNo"] != DBNull.Value)
                        tblVehicleInOutDetailsHistoryTONew.VehicleNo = Convert.ToString(tblVehicleInOutDetailsHistoryTODT["vehicleNo"].ToString());
                    if (tblVehicleInOutDetailsHistoryTODT["transactionNo"] != DBNull.Value)
                        tblVehicleInOutDetailsHistoryTONew.TransactionNo = Convert.ToString(tblVehicleInOutDetailsHistoryTODT["transactionNo"].ToString());
                    if (tblVehicleInOutDetailsHistoryTODT["transactionDisplayNo"] != DBNull.Value)
                        tblVehicleInOutDetailsHistoryTONew.TransactionDisplayNo = Convert.ToString(tblVehicleInOutDetailsHistoryTODT["transactionDisplayNo"].ToString());
                    if (tblVehicleInOutDetailsHistoryTODT["applicationRouting"] != DBNull.Value)
                        tblVehicleInOutDetailsHistoryTONew.ApplicationRouting = Convert.ToString(tblVehicleInOutDetailsHistoryTODT["applicationRouting"].ToString());
                    if (tblVehicleInOutDetailsHistoryTODT["scheduleRefNo"] != DBNull.Value)
                        tblVehicleInOutDetailsHistoryTONew.ScheduleRefNo = Convert.ToString(tblVehicleInOutDetailsHistoryTODT["scheduleRefNo"].ToString());
                    tblVehicleInOutDetailsHistoryTOList.Add(tblVehicleInOutDetailsHistoryTONew);
                }
            }
            return tblVehicleInOutDetailsHistoryTOList;
        }



        #region Insertion
        public  int InsertTblVehicleInOutDetailsHistory(TblVehicleInOutDetailsHistoryTO tblVehicleInOutDetailsHistoryTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblVehicleInOutDetailsHistoryTO, cmdInsert);
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

        public  int InsertTblVehicleInOutDetailsHistory(TblVehicleInOutDetailsHistoryTO tblVehicleInOutDetailsHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblVehicleInOutDetailsHistoryTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblVehicleInOutDetailsHistoryTO tblVehicleInOutDetailsHistoryTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblVehicleInOutDetailsHistory]( " + 
            "  [tblVehicleInOutDetailsId]" +
            " ,[moduleId]" +
            " ,[transactionTypeId]" +
            " ,[transactionStatusId]" +
            " ,[NextStatusId]" +
            " ,[partyId]" +
            " ,[transporterId]" +
            " ,[supervisorId]" +
            " ,[nextExpectedActionId]" +
            " ,[isActive]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[transactionDate]" +
            " ,[transactionStatusDate]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[vehicleNo]" +
            " ,[transactionNo]" +
            " ,[transactionDisplayNo]" +
            " ,[applicationRouting]" +
            " ,[scheduleRefNo]" +
            " )" +
" VALUES (" +            
            "  @TblVehicleInOutDetailsId " +
            " ,@ModuleId " +
            " ,@TransactionTypeId " +
            " ,@TransactionStatusId " +
            " ,@NextStatusId " +
            " ,@PartyId " +
            " ,@TransporterId " +
            " ,@SupervisorId " +
            " ,@NextExpectedActionId " +
            " ,@IsActive " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@TransactionDate " +
            " ,@TransactionStatusDate " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@VehicleNo " +
            " ,@TransactionNo " +
            " ,@TransactionDisplayNo " +
            " ,@ApplicationRouting " +
            " ,@ScheduleRefNo " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdTblVehicleInOutDetailsHistory", System.Data.SqlDbType.Int).Value =Constants.GetSqlDataValueNullForBaseValue( tblVehicleInOutDetailsHistoryTO.IdTblVehicleInOutDetailsHistory);
            cmdInsert.Parameters.Add("@TblVehicleInOutDetailsId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsHistoryTO.TblVehicleInOutDetailsId);
            cmdInsert.Parameters.Add("@ModuleId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsHistoryTO.ModuleId);
            cmdInsert.Parameters.Add("@TransactionTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsHistoryTO.TransactionTypeId);
            cmdInsert.Parameters.Add("@TransactionStatusId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsHistoryTO.TransactionStatusId);
            cmdInsert.Parameters.Add("@NextStatusId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsHistoryTO.NextStatusId);
            cmdInsert.Parameters.Add("@PartyId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsHistoryTO.PartyId);
            cmdInsert.Parameters.Add("@TransporterId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsHistoryTO.TransporterId);
            cmdInsert.Parameters.Add("@SupervisorId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsHistoryTO.SupervisorId);
            cmdInsert.Parameters.Add("@NextExpectedActionId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsHistoryTO.NextExpectedActionId);
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsHistoryTO.IsActive);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsHistoryTO.CreatedBy);
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsHistoryTO.UpdatedBy);
            cmdInsert.Parameters.Add("@TransactionDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsHistoryTO.TransactionDate);
            cmdInsert.Parameters.Add("@TransactionStatusDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsHistoryTO.TransactionStatusDate);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsHistoryTO.CreatedOn);
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsHistoryTO.UpdatedOn);
            cmdInsert.Parameters.Add("@VehicleNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsHistoryTO.VehicleNo);
            cmdInsert.Parameters.Add("@TransactionNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsHistoryTO.TransactionNo);
            cmdInsert.Parameters.Add("@TransactionDisplayNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsHistoryTO.TransactionDisplayNo);
            cmdInsert.Parameters.Add("@ApplicationRouting", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsHistoryTO.ApplicationRouting);
            cmdInsert.Parameters.Add("@ScheduleRefNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsHistoryTO.ScheduleRefNo);
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public  int UpdateTblVehicleInOutDetailsHistory(TblVehicleInOutDetailsHistoryTO tblVehicleInOutDetailsHistoryTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblVehicleInOutDetailsHistoryTO, cmdUpdate);
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

        public  int UpdateTblVehicleInOutDetailsHistory(TblVehicleInOutDetailsHistoryTO tblVehicleInOutDetailsHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblVehicleInOutDetailsHistoryTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblVehicleInOutDetailsHistoryTO tblVehicleInOutDetailsHistoryTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblVehicleInOutDetailsHistory] SET " + 
            "  [tblVehicleInOutDetailsId]= @TblVehicleInOutDetailsId" +
            " ,[moduleId]= @ModuleId" +
            " ,[transactionTypeId]= @TransactionTypeId" +
            " ,[transactionStatusId]= @TransactionStatusId" +
            " ,[NextStatusId]= @NextStatusId" +
            " ,[partyId]= @PartyId" +
            " ,[transporterId]= @TransporterId" +
            " ,[supervisorId]= @SupervisorId" +
            " ,[nextExpectedActionId]= @NextExpectedActionId" +
            " ,[isActive]= @IsActive" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[transactionDate]= @TransactionDate" +
            " ,[transactionStatusDate]= @TransactionStatusDate" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[vehicleNo]= @VehicleNo" +
            " ,[transactionNo]= @TransactionNo" +
            " ,[transactionDisplayNo]= @TransactionDisplayNo" +
            " ,[applicationRouting]= @ApplicationRouting" +
            " ,[scheduleRefNo] = @ScheduleRefNo" +
            " WHERE 1 = 1 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdTblVehicleInOutDetailsHistory", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsHistoryTO.IdTblVehicleInOutDetailsHistory);
            cmdUpdate.Parameters.Add("@TblVehicleInOutDetailsId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsHistoryTO.TblVehicleInOutDetailsId);
            cmdUpdate.Parameters.Add("@ModuleId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsHistoryTO.ModuleId);
            cmdUpdate.Parameters.Add("@TransactionTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsHistoryTO.TransactionTypeId);
            cmdUpdate.Parameters.Add("@TransactionStatusId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsHistoryTO.TransactionStatusId);
            cmdUpdate.Parameters.Add("@NextStatusId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsHistoryTO.NextStatusId);
            cmdUpdate.Parameters.Add("@PartyId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsHistoryTO.PartyId);
            cmdUpdate.Parameters.Add("@TransporterId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsHistoryTO.TransporterId);
            cmdUpdate.Parameters.Add("@SupervisorId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsHistoryTO.SupervisorId);
            cmdUpdate.Parameters.Add("@NextExpectedActionId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsHistoryTO.NextExpectedActionId);
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsHistoryTO.IsActive);
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsHistoryTO.CreatedBy);
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsHistoryTO.UpdatedBy);
            cmdUpdate.Parameters.Add("@TransactionDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsHistoryTO.TransactionDate);
            cmdUpdate.Parameters.Add("@TransactionStatusDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsHistoryTO.TransactionStatusDate);
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsHistoryTO.CreatedOn);
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsHistoryTO.UpdatedOn);
            cmdUpdate.Parameters.Add("@VehicleNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsHistoryTO.VehicleNo);
            cmdUpdate.Parameters.Add("@TransactionNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsHistoryTO.TransactionNo);
            cmdUpdate.Parameters.Add("@TransactionDisplayNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsHistoryTO.TransactionDisplayNo);
            cmdUpdate.Parameters.Add("@ApplicationRouting", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsHistoryTO.ApplicationRouting);
            cmdUpdate.Parameters.Add("@ScheduleRefNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsHistoryTO.ScheduleRefNo);
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblVehicleInOutDetailsHistory(Int32 idTblVehicleInOutDetailsHistory)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idTblVehicleInOutDetailsHistory, cmdDelete);
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

        public  int DeleteTblVehicleInOutDetailsHistory(Int32 idTblVehicleInOutDetailsHistory, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idTblVehicleInOutDetailsHistory, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int32 idTblVehicleInOutDetailsHistory, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblVehicleInOutDetailsHistory] " +
            " WHERE idTblVehicleInOutDetailsHistory = " + idTblVehicleInOutDetailsHistory +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idTblVehicleInOutDetailsHistory", System.Data.SqlDbType.Int).Value = tblVehicleInOutDetailsHistoryTO.IdTblVehicleInOutDetailsHistory;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
