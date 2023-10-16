using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL.Interfaces;

namespace ODLMWebAPI.DAL
{
    public class TblAlertSubscriptSettingsDAO : ITblAlertSubscriptSettingsDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblAlertSubscriptSettingsDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT tblAlertSubscriptSettings.*, notificationType.* FROM [tblAlertSubscriptSettings]" +
                                  " LEFT JOIN dimNotificationType notificationType ON tblAlertSubscriptSettings.notificationTypeId =notificationType.idNotificationType";
                                  
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblAlertSubscriptSettingsTO> SelectAllTblAlertSubscriptSettings()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(sqlReader);
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose(); conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblAlertSubscriptSettingsTO> SelectAllTblAlertSubscriptSettings(int subscriptionId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE subscriptionId=" + subscriptionId + " AND isActive=1";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(sqlReader);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }

        /// <summary>
        /// Priyanka [21-09-2018] : Added to get the alert subscription setting list
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <returns></returns>
        public List<TblAlertSubscriptSettingsTO> SelectAllTblAlertSubscriptSettings(int subscriptionId)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open(); 
                cmdSelect.CommandText = " SELECT * from dimNotificationType " +
                                        " LEFT JOIN tblAlertSubscriptSettings ON dimNotificationType.idNotificationType = tblAlertSubscriptSettings.notificationTypeId " +
                                        " AND tblAlertSubscriptSettings.isActive = 1 AND tblAlertSubscriptSettings.subscriptionId = "+ subscriptionId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(sqlReader);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose(); conn.Close();
                cmdSelect.Dispose();
            }
        }
        /// <summary>
        /// Priyanka [25-09-2018] : Added to get the alert subscription setting list from alert defination Id.
        /// </summary>
        /// <param name="alertDefId"></param>
        /// <returns></returns>
        public List<TblAlertSubscriptSettingsTO> SelectAllTblAlertSubscriptSettingsByAlertDefId(int alertDefId)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * from dimNotificationType " +
                                        " LEFT JOIN tblAlertSubscriptSettings ON dimNotificationType.idNotificationType = tblAlertSubscriptSettings.notificationTypeId " +
                                        " AND tblAlertSubscriptSettings.isActive = 1 AND tblAlertSubscriptSettings.alertDefId = " + alertDefId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(sqlReader);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose(); conn.Close();
                cmdSelect.Dispose();
            }
        }
        public List<TblAlertSubscriptSettingsTO> SelectAllTblAlertSubscriptSettingsByAlertDefId(int alertDefId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE alertDefId=" + alertDefId + " AND isActive=1";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(sqlReader);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }


        public TblAlertSubscriptSettingsTO SelectTblAlertSubscriptSettings(Int32 idSubscriSettings)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idSubscriSettings = " + idSubscriSettings +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblAlertSubscriptSettingsTO> list = ConvertDTToList(sqlReader);
                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose(); conn.Close();
                cmdSelect.Dispose();
            }
        }

        //Priyanka [24-09-2018]
        public TblAlertSubscriptSettingsTO SelectTblAlertSubscriptSettingsFromNotifyId(Int32 NotificationTypeId, Int32 SubscriptionId, Int32 AlertDefId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE notificationTypeId = " + NotificationTypeId +
                                        " AND ISNULL(subscriptionId ,0) = " + SubscriptionId + 
                                        " AND ISNULL(alertDefId ,0) = " + AlertDefId + " AND isActive = 1";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblAlertSubscriptSettingsTO> list = ConvertDTToList(sqlReader);
                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose(); conn.Close();
                cmdSelect.Dispose();
            }
        }



        public List<TblAlertSubscriptSettingsTO> ConvertDTToList(SqlDataReader tblAlertSubscriptSettingsTODT)
        {
            List<TblAlertSubscriptSettingsTO> tblAlertSubscriptSettingsTOList = new List<TblAlertSubscriptSettingsTO>();
            if (tblAlertSubscriptSettingsTODT != null)
            {
                while (tblAlertSubscriptSettingsTODT.Read())
                {
                    TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTONew = new TblAlertSubscriptSettingsTO();
                    if (tblAlertSubscriptSettingsTODT["idSubscriSettings"] != DBNull.Value)
                        tblAlertSubscriptSettingsTONew.IdSubscriSettings = Convert.ToInt32(tblAlertSubscriptSettingsTODT["idSubscriSettings"].ToString());
                    if (tblAlertSubscriptSettingsTODT["subscriptionId"] != DBNull.Value)
                        tblAlertSubscriptSettingsTONew.SubscriptionId = Convert.ToInt32(tblAlertSubscriptSettingsTODT["subscriptionId"].ToString());
                    if (tblAlertSubscriptSettingsTODT["escalationSettingId"] != DBNull.Value)
                        tblAlertSubscriptSettingsTONew.EscalationSettingId = Convert.ToInt32(tblAlertSubscriptSettingsTODT["escalationSettingId"].ToString());
                    if (tblAlertSubscriptSettingsTODT["notificationTypeId"] != DBNull.Value)
                        tblAlertSubscriptSettingsTONew.NotificationTypeId = Convert.ToInt32(tblAlertSubscriptSettingsTODT["notificationTypeId"].ToString());
                    if (tblAlertSubscriptSettingsTODT["isActive"] != DBNull.Value)
                        tblAlertSubscriptSettingsTONew.IsActive = Convert.ToInt32(tblAlertSubscriptSettingsTODT["isActive"].ToString());
                    if (tblAlertSubscriptSettingsTODT["createdOn"] != DBNull.Value)
                        tblAlertSubscriptSettingsTONew.CreatedOn = Convert.ToDateTime(tblAlertSubscriptSettingsTODT["createdOn"].ToString());

                    //Priyanka [20-09-18] Added
                    if (tblAlertSubscriptSettingsTODT["updatedBy"] != DBNull.Value)
                        tblAlertSubscriptSettingsTONew.UpdatedBy = Convert.ToInt32(tblAlertSubscriptSettingsTODT["updatedBy"].ToString());

                    if (tblAlertSubscriptSettingsTODT["updatedOn"] != DBNull.Value)
                        tblAlertSubscriptSettingsTONew.UpdatedOn = Convert.ToDateTime(tblAlertSubscriptSettingsTODT["updatedOn"].ToString());

                    if (tblAlertSubscriptSettingsTODT["idNotificationType"] != DBNull.Value)
                        tblAlertSubscriptSettingsTONew.IdNotificationType = Convert.ToInt32(tblAlertSubscriptSettingsTODT["idNotificationType"].ToString());
                        tblAlertSubscriptSettingsTONew.NotificationTypeId = tblAlertSubscriptSettingsTONew.IdNotificationType;

                    if (tblAlertSubscriptSettingsTODT["notificationTypeDesc"] != DBNull.Value)
                        tblAlertSubscriptSettingsTONew.NotificationTypeDesc = Convert.ToString(tblAlertSubscriptSettingsTODT["notificationTypeDesc"].ToString());

                    if (tblAlertSubscriptSettingsTODT["alertDefId"] != DBNull.Value)
                        tblAlertSubscriptSettingsTONew.AlertDefId = Convert.ToInt32(tblAlertSubscriptSettingsTODT["alertDefId"].ToString());
                    tblAlertSubscriptSettingsTOList.Add(tblAlertSubscriptSettingsTONew);
                }
            }
            return tblAlertSubscriptSettingsTOList;
        }
        
        #endregion

        #region Insertion
        public int InsertTblAlertSubscriptSettings(TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblAlertSubscriptSettingsTO, cmdInsert);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public int InsertTblAlertSubscriptSettings(TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblAlertSubscriptSettingsTO, cmdInsert);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public int ExecuteInsertionCommand(TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblAlertSubscriptSettings]( " + 
                                "  [subscriptionId]" +
                                " ,[escalationSettingId]" +
                                " ,[notificationTypeId]" +
                                " ,[isActive]" +
                                " ,[createdOn]" +
                                " ,[updatedBy]" +
                                " ,[updatedOn]" +
                                " ,[alertDefId]" +
                                " )" +
                    " VALUES (" +
                                "  @SubscriptionId " +
                                " ,@EscalationSettingId " +
                                " ,@NotificationTypeId " +
                                " ,@IsActive " +
                                " ,@CreatedOn " + 
                                " ,@UpdatedBy "+
                                " ,@UpdatedOn "+
                                " ,@AlertDefId "+
                                " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdSubscriSettings", System.Data.SqlDbType.Int).Value = tblAlertSubscriptSettingsTO.IdSubscriSettings;
            cmdInsert.Parameters.Add("@SubscriptionId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblAlertSubscriptSettingsTO.SubscriptionId);
            cmdInsert.Parameters.Add("@EscalationSettingId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblAlertSubscriptSettingsTO.EscalationSettingId);
            cmdInsert.Parameters.Add("@NotificationTypeId", System.Data.SqlDbType.Int).Value = tblAlertSubscriptSettingsTO.NotificationTypeId;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblAlertSubscriptSettingsTO.IsActive;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblAlertSubscriptSettingsTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblAlertSubscriptSettingsTO.UpdatedBy);
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblAlertSubscriptSettingsTO.UpdatedOn);
            cmdInsert.Parameters.Add("@AlertDefId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblAlertSubscriptSettingsTO.AlertDefId);
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblAlertSubscriptSettingsTO.IdSubscriSettings = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public int UpdateTblAlertSubscriptSettings(TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblAlertSubscriptSettingsTO, cmdUpdate);
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

        public int UpdateTblAlertSubscriptSettings(TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblAlertSubscriptSettingsTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblAlertSubscriptSettings] SET " + 
           // "  [idSubscriSettings] = @IdSubscriSettings" +
            "  [subscriptionId]= @SubscriptionId" +
            " ,[escalationSettingId]= @EscalationSettingId" +
            " ,[notificationTypeId]= @NotificationTypeId" +
            " ,[isActive]= @IsActive" +
            " ,[createdOn] = @CreatedOn" +
            " ,[updatedOn] = @UpdatedOn" +
            " ,[updatedBy] = @UpdatedBy" +
            " ,[alertDefId] = @AlertDefId " +
            // " ,[idNotificationType]= @IdNotificationType" +
            // " ,[notificationTypeDesc] = @NotificationTypeDesc" +

            " WHERE idSubscriSettings = @IdSubscriSettings "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdSubscriSettings", System.Data.SqlDbType.Int).Value = tblAlertSubscriptSettingsTO.IdSubscriSettings;
            cmdUpdate.Parameters.Add("@SubscriptionId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblAlertSubscriptSettingsTO.SubscriptionId);
            cmdUpdate.Parameters.Add("@EscalationSettingId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblAlertSubscriptSettingsTO.EscalationSettingId);
            cmdUpdate.Parameters.Add("@NotificationTypeId", System.Data.SqlDbType.Int).Value = tblAlertSubscriptSettingsTO.NotificationTypeId;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblAlertSubscriptSettingsTO.IsActive;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblAlertSubscriptSettingsTO.CreatedOn);
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblAlertSubscriptSettingsTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblAlertSubscriptSettingsTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@AlertDefId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblAlertSubscriptSettingsTO.AlertDefId);
            // cmdUpdate.Parameters.Add("@IdNotificationType", System.Data.SqlDbType.Int).Value = tblAlertSubscriptSettingsTO.IdNotificationType;
            // cmdUpdate.Parameters.Add("@NotificationTypeDesc", System.Data.SqlDbType.NVarChar).Value = tblAlertSubscriptSettingsTO.NotificationTypeDesc;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public int DeleteTblAlertSubscriptSettings(Int32 idSubscriSettings)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idSubscriSettings, cmdDelete);
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

        public int DeleteTblAlertSubscriptSettings(Int32 idSubscriSettings, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idSubscriSettings, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idSubscriSettings, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblAlertSubscriptSettings] " +
            " WHERE idSubscriSettings = " + idSubscriSettings +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idSubscriSettings", System.Data.SqlDbType.Int).Value = tblAlertSubscriptSettingsTO.IdSubscriSettings;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
