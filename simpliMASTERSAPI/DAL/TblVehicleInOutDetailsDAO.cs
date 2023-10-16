using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
//using System.Windows.Forms;
using ODLMWebAPI.StaticStuff;
using System.Configuration;
using simpliMASTERSAPI.TO;
using ODLMWebAPI.BL.Interfaces;
using simpliMASTERSAPI.DAL.Interfaces;
using ODLMWebAPI.Models;

namespace simpliMASTERSAPI.DAL
{
    public class TblVehicleInOutDetailsDAO : ITblVehicleInOutDetailsDAO
    {
        #region Methods
        private readonly IConnectionString _iConnectionString;
        public TblVehicleInOutDetailsDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = "select tblVehicleInOutDetails.*,orgTechInspector.userDisplayName AS technicalInspectorName,dimstatusTranStatus.statusName as TransactionStatusName,dimstatusNext.statusName as NextStatusName,orgTransporter.firmName as TransporterName,orgSupplier.firmname as partyName, "
                                   + " orgSupervisor.userDisplayName as supervisorName from tblVehicleInOutDetails tblVehicleInOutDetails "
                                   + " left join dimstatus dimstatusTranStatus on dimstatusTranStatus.idStatus = tblVehicleInOutDetails.transactionStatusid "
                                   +" left join dimstatus dimstatusNext on dimstatusNext.idStatus = tblVehicleInOutDetails.NextStatusId "
                                   +" left  join tblOrganization orgTransporter on orgTransporter.idOrganization = tblVehicleInOutDetails.transporterId "
                                   +" left join tblOrganization orgSupplier on orgSupplier.idOrganization = tblVehicleInOutDetails.partyId "
                                   +" left join tblUser orgSupervisor on orgSupervisor.idUser = tblVehicleInOutDetails.supervisorId " 
                                   +" left join tblUser orgTechInspector on orgTechInspector.idUser = tblVehicleInOutDetails.technicalInspectorId"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblVehicleInOutDetailsTO> SelectAllTblVehicleInOutDetails(Int32 moduleId,Int32 showVehicleInOut, string fromDate, string toDate, bool skipDatetime,string statusStr)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            string dateConditionStr = "";
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " where isnull(tblVehicleInOutDetails.isActive ,0)= 1 and tblVehicleInOutDetails.vehicleNo is not null and TRIM(tblVehicleInOutDetails.vehicleNo) <> ''"+//" and transporterId > 0" +//Reshma Commented For Showing Spot Entry without transporter name.
                    " and moduleId =" + moduleId; //[2021-02-02] Dhananjay added and TRIM(tblVehicleInOutDetails.vehicleNo) <> ''
                //if(showVehicleInOut == 1)
                //{
                //    //cmdSelect.CommandText += " and transactionStatusId not in ( " + Convert.ToInt32(Constants.TranStatusE.Vehicle_Out_Common) + "," + Convert.ToInt32(Constants.TranStatusE.Sent_in) + ","+ Convert.ToInt32(Constants.TranStatusE.Vehicle_Rejected_and_Out)+ "," + Convert.ToInt32(Constants.TranStatusE.Unloading_is_in_progress) + "," + Convert.ToInt32(Constants.TranStatusE.Unloading_Completed) + "," + Convert.ToInt32(Constants.TranStatusE.Vehicle_Rejected_while_GRN) + ")";
                //    cmdSelect.CommandText += " and transactionStatusId not in ( "+ statusStr + ")";
                //}
                //else if(showVehicleInOut == 2)
                //{
                //    //cmdSelect.CommandText += " and transactionStatusId in ( " + Convert.ToInt32(Constants.TranStatusE.Vehicle_Rejected_while_GRN) + "," + Convert.ToInt32(Constants.TranStatusE.Unloading_Completed) + "," + Convert.ToInt32(Constants.TranStatusE.Unloading_is_in_progress) + ")";
                //    cmdSelect.CommandText += " and transactionStatusId not in ( " + statusStr + ")";
                //}
                if (showVehicleInOut==3)
                {
                    cmdSelect.CommandText += " and transactionStatusId in ( " + statusStr + ")";
                }
                else if (statusStr != null && statusStr !="") {
                    cmdSelect.CommandText += " and transactionStatusId not in ( " + statusStr + ")";
                }
                if (!skipDatetime)
                {
                    cmdSelect.CommandText += " and  cast(tblVehicleInOutDetails.transactionDate as date) BETWEEN @fromDate and @toDate ";
                }

                cmdSelect.CommandText += " And transactionStatusId !=NextStatusId  order by tblVehicleInOutDetails.idTblVehicleInOutDetails desc";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = fromDate;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = toDate;

                //cmdSelect.Parameters.Add("@idTblVehicleInOutDetails", System.Data.SqlDbType.Int).Value = tblVehicleInOutDetailsTO.IdTblVehicleInOutDetails;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblVehicleInOutDetailsTO> list = ConvertDTToList(reader);
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

       public TblVehicleInOutDetailsTO SelectAllTblVehicleInOutDetailsById(int moduleId, int idVehicleInOut)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " where isnull(tblVehicleInOutDetails.isActive ,0)= 1 and moduleId =" + moduleId;
                if (idVehicleInOut >0 )
                {
                    cmdSelect.CommandText += " and idTblVehicleInOutDetails = " + idVehicleInOut;
                }
               
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idTblVehicleInOutDetails", System.Data.SqlDbType.Int).Value = tblVehicleInOutDetailsTO.IdTblVehicleInOutDetails;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblVehicleInOutDetailsTO> list = ConvertDTToList(reader);
                if (list != null && list.Count > 0)
                {
                    return list[0];
                }
                return null;
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
        public List<TblVehicleInOutDetailsTO> SelectAllTblVehicleInOutDetails()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " where isnull(tblVehicleInOutDetails.isActive ,0)= 1 " ;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idTblVehicleInOutDetails", System.Data.SqlDbType.Int).Value = tblVehicleInOutDetailsTO.IdTblVehicleInOutDetails;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblVehicleInOutDetailsTO> list = ConvertDTToList(reader);
                return list;
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

        public TblVehicleInOutDetailsTO SelectTblVehicleInOutDetails(Int32 idTblVehicleInOutDetails)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idTblVehicleInOutDetails = " + idTblVehicleInOutDetails +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idTblVehicleInOutDetails", System.Data.SqlDbType.Int).Value = tblVehicleInOutDetailsTO.IdTblVehicleInOutDetails;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblVehicleInOutDetailsTO> list = ConvertDTToList(reader);
                if (list != null && list.Count > 0)
                {
                    return list[0];
                }
                return null;

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

        public List<TblVehicleInOutDetailsTO> SelectAllTblVehicleInOutDetails(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idTblVehicleInOutDetails", System.Data.SqlDbType.Int).Value = tblVehicleInOutDetailsTO.IdTblVehicleInOutDetails;
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblVehicleInOutDetailsTO> list = ConvertDTToList(reader);
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

        public List<TblVehicleInOutDetailsTO> ConvertDTToList(SqlDataReader tblVehicleInOutDetailsTODT)
        {
            List<TblVehicleInOutDetailsTO> tblVehicleInOutDetailsTOList = new List<TblVehicleInOutDetailsTO>();
            if (tblVehicleInOutDetailsTODT != null)
            {
                while (tblVehicleInOutDetailsTODT.Read())
                {
                    TblVehicleInOutDetailsTO tblVehicleInOutDetailsTONew = new TblVehicleInOutDetailsTO();
                    if (tblVehicleInOutDetailsTODT["idTblVehicleInOutDetails"] != DBNull.Value)
                        tblVehicleInOutDetailsTONew.IdTblVehicleInOutDetails = Convert.ToInt32(tblVehicleInOutDetailsTODT["idTblVehicleInOutDetails"].ToString());
                    if (tblVehicleInOutDetailsTODT["moduleId"] != DBNull.Value)
                        tblVehicleInOutDetailsTONew.ModuleId = Convert.ToInt32(tblVehicleInOutDetailsTODT["moduleId"].ToString());
                    if (tblVehicleInOutDetailsTODT["transactionTypeId"] != DBNull.Value)
                        tblVehicleInOutDetailsTONew.TransactionTypeId = Convert.ToInt32(tblVehicleInOutDetailsTODT["transactionTypeId"].ToString());
                    if (tblVehicleInOutDetailsTODT["transactionStatusId"] != DBNull.Value)
                        tblVehicleInOutDetailsTONew.TransactionStatusId = Convert.ToInt32(tblVehicleInOutDetailsTODT["transactionStatusId"].ToString());
                    if (tblVehicleInOutDetailsTODT["NextStatusId"] != DBNull.Value)
                        tblVehicleInOutDetailsTONew.NextStatusId = Convert.ToInt32(tblVehicleInOutDetailsTODT["NextStatusId"].ToString());
                    if (tblVehicleInOutDetailsTODT["partyId"] != DBNull.Value)
                        tblVehicleInOutDetailsTONew.PartyId = Convert.ToInt32(tblVehicleInOutDetailsTODT["partyId"].ToString());
                    if (tblVehicleInOutDetailsTODT["transporterId"] != DBNull.Value)
                        tblVehicleInOutDetailsTONew.TransporterId = Convert.ToInt32(tblVehicleInOutDetailsTODT["transporterId"].ToString());
                    if (tblVehicleInOutDetailsTODT["supervisorId"] != DBNull.Value)
                        tblVehicleInOutDetailsTONew.SupervisorId = Convert.ToInt32(tblVehicleInOutDetailsTODT["supervisorId"].ToString());
                    if (tblVehicleInOutDetailsTODT["nextExpectedActionId"] != DBNull.Value)
                        tblVehicleInOutDetailsTONew.NextExpectedActionId = Convert.ToInt32(tblVehicleInOutDetailsTODT["nextExpectedActionId"].ToString());
                    if (tblVehicleInOutDetailsTODT["isActive"] != DBNull.Value)
                        tblVehicleInOutDetailsTONew.IsActive = Convert.ToInt32(tblVehicleInOutDetailsTODT["isActive"].ToString());
                    if (tblVehicleInOutDetailsTODT["createdBy"] != DBNull.Value)
                        tblVehicleInOutDetailsTONew.CreatedBy = Convert.ToInt32(tblVehicleInOutDetailsTODT["createdBy"].ToString());
                    if (tblVehicleInOutDetailsTODT["updatedBy"] != DBNull.Value)
                        tblVehicleInOutDetailsTONew.UpdatedBy = Convert.ToInt32(tblVehicleInOutDetailsTODT["updatedBy"].ToString());
                    if (tblVehicleInOutDetailsTODT["transactionDate"] != DBNull.Value)
                        tblVehicleInOutDetailsTONew.TransactionDate = Convert.ToDateTime(tblVehicleInOutDetailsTODT["transactionDate"].ToString());
                    if (tblVehicleInOutDetailsTODT["transactionStatusDate"] != DBNull.Value)
                        tblVehicleInOutDetailsTONew.TransactionStatusDate = Convert.ToDateTime(tblVehicleInOutDetailsTODT["transactionStatusDate"].ToString());
                    if (tblVehicleInOutDetailsTODT["createdOn"] != DBNull.Value)
                        tblVehicleInOutDetailsTONew.CreatedOn = Convert.ToDateTime(tblVehicleInOutDetailsTODT["createdOn"].ToString());
                    if (tblVehicleInOutDetailsTODT["updatedOn"] != DBNull.Value)
                        tblVehicleInOutDetailsTONew.UpdatedOn = Convert.ToDateTime(tblVehicleInOutDetailsTODT["updatedOn"].ToString());
                    if (tblVehicleInOutDetailsTODT["vehicleNo"] != DBNull.Value)
                        tblVehicleInOutDetailsTONew.VehicleNo = Convert.ToString(tblVehicleInOutDetailsTODT["vehicleNo"].ToString());
                    if (tblVehicleInOutDetailsTODT["transactionNo"] != DBNull.Value)
                        tblVehicleInOutDetailsTONew.TransactionNo = Convert.ToString(tblVehicleInOutDetailsTODT["transactionNo"].ToString());
                    if (tblVehicleInOutDetailsTODT["transactionDisplayNo"] != DBNull.Value)
                        tblVehicleInOutDetailsTONew.TransactionDisplayNo = Convert.ToString(tblVehicleInOutDetailsTODT["transactionDisplayNo"].ToString());
                    if (tblVehicleInOutDetailsTODT["applicationRouting"] != DBNull.Value)
                        tblVehicleInOutDetailsTONew.ApplicationRouting = Convert.ToString(tblVehicleInOutDetailsTODT["applicationRouting"].ToString());
                    if (tblVehicleInOutDetailsTODT["scheduleRefNo"] != DBNull.Value)
                        tblVehicleInOutDetailsTONew.ScheduleRefNo = Convert.ToString(tblVehicleInOutDetailsTODT["scheduleRefNo"].ToString());
                    if (tblVehicleInOutDetailsTODT["NextStatusName"] != DBNull.Value)
                        tblVehicleInOutDetailsTONew.NextStatusName = Convert.ToString(tblVehicleInOutDetailsTODT["NextStatusName"].ToString());
                    if (tblVehicleInOutDetailsTODT["PartyName"] != DBNull.Value)
                        tblVehicleInOutDetailsTONew.PartyName = Convert.ToString(tblVehicleInOutDetailsTODT["PartyName"].ToString());
                    if (tblVehicleInOutDetailsTODT["TransporterName"] != DBNull.Value)
                        tblVehicleInOutDetailsTONew.TransporterName = Convert.ToString(tblVehicleInOutDetailsTODT["TransporterName"].ToString());
                    if (tblVehicleInOutDetailsTODT["TransactionStatusName"] != DBNull.Value)
                        tblVehicleInOutDetailsTONew.TransactionStatusName = Convert.ToString(tblVehicleInOutDetailsTODT["TransactionStatusName"].ToString());
                    if (tblVehicleInOutDetailsTODT["supervisorName"] != DBNull.Value)
                        tblVehicleInOutDetailsTONew.SupervisorName = Convert.ToString(tblVehicleInOutDetailsTODT["supervisorName"].ToString());
                    if (tblVehicleInOutDetailsTODT["technicalInspectorId"] != DBNull.Value)
                        tblVehicleInOutDetailsTONew.TechnicalInspectorId = Convert.ToInt32(tblVehicleInOutDetailsTODT["technicalInspectorId"].ToString());
                    if (tblVehicleInOutDetailsTODT["technicalInspectorName"] != DBNull.Value)
                        tblVehicleInOutDetailsTONew.TechnicalInspectorName = Convert.ToString(tblVehicleInOutDetailsTODT["technicalInspectorName"].ToString());

                    tblVehicleInOutDetailsTOList.Add(tblVehicleInOutDetailsTONew);
                }
            }
            return tblVehicleInOutDetailsTOList;
        }



        #endregion

        #region Insertion
        public int InsertTblVehicleInOutDetails(TblVehicleInOutDetailsTO tblVehicleInOutDetailsTO)
        {
            String sqlConnStr = "";
                //Masters.GlobalConnectionString.ActiveConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblVehicleInOutDetailsTO, cmdInsert);
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

        public  int InsertTblVehicleInOutDetails(TblVehicleInOutDetailsTO tblVehicleInOutDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblVehicleInOutDetailsTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblVehicleInOutDetailsTO tblVehicleInOutDetailsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblVehicleInOutDetails]( " + 
            "  [moduleId]" +
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
            " ,[technicalInspectorId]" +
            " )" +
" VALUES (" +
            "  @ModuleId " +
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
            " ,@TechnicalInspectorId " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdTblVehicleInOutDetails", System.Data.SqlDbType.Int).Value = tblVehicleInOutDetailsTO.IdTblVehicleInOutDetails;
            cmdInsert.Parameters.Add("@ModuleId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.ModuleId);
            cmdInsert.Parameters.Add("@TechnicalInspectorId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.TechnicalInspectorId);
            cmdInsert.Parameters.Add("@TransactionTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.TransactionTypeId);
            cmdInsert.Parameters.Add("@TransactionStatusId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.TransactionStatusId);
            cmdInsert.Parameters.Add("@NextStatusId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.NextStatusId);
            cmdInsert.Parameters.Add("@PartyId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.PartyId);
            cmdInsert.Parameters.Add("@TransporterId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.TransporterId);
            cmdInsert.Parameters.Add("@SupervisorId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.SupervisorId);
            cmdInsert.Parameters.Add("@NextExpectedActionId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.NextExpectedActionId);
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.IsActive);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.CreatedBy);
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.UpdatedBy);
            cmdInsert.Parameters.Add("@TransactionDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.TransactionDate);
            cmdInsert.Parameters.Add("@TransactionStatusDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.TransactionStatusDate);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.CreatedOn);
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.UpdatedOn);
            cmdInsert.Parameters.Add("@VehicleNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.VehicleNo);
            cmdInsert.Parameters.Add("@TransactionNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.TransactionNo);
            cmdInsert.Parameters.Add("@TransactionDisplayNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.TransactionDisplayNo);
            cmdInsert.Parameters.Add("@ApplicationRouting", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.ApplicationRouting);
            cmdInsert.Parameters.Add("@ScheduleRefNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.ScheduleRefNo);
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public  int UpdateTblVehicleInOutDetails(TblVehicleInOutDetailsTO tblVehicleInOutDetailsTO)
        {
            String sqlConnStr = "";
                //Masters.GlobalConnectionString.ActiveConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblVehicleInOutDetailsTO, cmdUpdate);
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

        public  int UpdateTblVehicleInOutDetails(TblVehicleInOutDetailsTO tblVehicleInOutDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblVehicleInOutDetailsTO, cmdUpdate);
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

        public int UpdateTblVehicleInOutDetailsStatusOnly(TblVehicleInOutDetailsTO tblVehicleInOutDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommandStatusonly(tblVehicleInOutDetailsTO, cmdUpdate);
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

        public List<DropDownTO> SelectAllSystemUsersFromRoleType(int roleTypeId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                //Prajakta[2019-05-28]Added tblUserRole.isActive=1 condition
                String aqlQuery = "select distinct userId,userDisplayName from tblUser userm " +
                                 " left join tblUserRole on userm.iduser = tblUserRole.userId" +
                                 " left join tblRole on   tblUserRole.roleId = tblRole.idRole where roleTypeId = " + roleTypeId + " and tblUserRole.isActive=1 and userm.isActive=1";


                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["userId"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["userId"].ToString());

                    if (dateReader["userDisplayName"] != DBNull.Value)
                        dropDownTONew.Text = dateReader["userDisplayName"].ToString();

                    dropDownTOList.Add(dropDownTONew);
                }


                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }

        }

        public List<DropDownTO> SelectAllSystemUsersforUnloadingSuperwisorVehicleIn()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
               
                String aqlQuery = "select distinct idUser as userId,userDisplayName from tblUser  where isActive=1 ";            


                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["userId"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["userId"].ToString());

                    if (dateReader["userDisplayName"] != DBNull.Value)
                        dropDownTONew.Text = dateReader["userDisplayName"].ToString();

                    dropDownTOList.Add(dropDownTONew);
                }


                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }

        }

        public int UpdateTblScheduleStatusOnly(TblVehicleInOutDetailsTO tblVehicleInOutDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommandScheduleStatus(tblVehicleInOutDetailsTO, cmdUpdate);
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


        public int ExecuteUpdationCommandScheduleStatus(TblVehicleInOutDetailsTO tblVehicleInOutDetailsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblCommercialDocSchedule] SET " +
            "  [statusId]= @TransactionStatusId" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[updatedOn]= @UpdatedOn" +
            " WHERE 1 = 1 ";

            cmdUpdate.CommandText = sqlQuery + " and idCommercialDocSchedule = @idCommercialDocSchedule and isnull(isActive,0) = 1";
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@idCommercialDocSchedule", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.TransactionNo);
            cmdUpdate.Parameters.Add("@TransactionStatusId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.TransactionStatusId);
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.UpdatedBy);
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.UpdatedOn);
            return cmdUpdate.ExecuteNonQuery();
        }

        public int ExecuteUpdationCommandStatusonly(TblVehicleInOutDetailsTO tblVehicleInOutDetailsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblVehicleInOutDetails] SET " +
            "  [transactionStatusId]= @TransactionStatusId" +
            " ,[NextStatusId]= @NextStatusId" +
            " ,[nextExpectedActionId]= @NextExpectedActionId" +
            " ,[supervisorId]= @SupervisorId" +
            " ,[technicalInspectorId]= @TechnicalInspectorId" +
            " ,[isActive]= @IsActive" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[transactionStatusDate]= @TransactionStatusDate" +
            " ,[updatedOn]= @UpdatedOn" +
            " WHERE 1 = 1 ";

            cmdUpdate.CommandText = sqlQuery + " and idTblVehicleInOutDetails = @IdTblVehicleInOutDetails and moduleId =@ModuleId";
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdTblVehicleInOutDetails", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.IdTblVehicleInOutDetails);
            cmdUpdate.Parameters.Add("@SupervisorId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.SupervisorId);
            cmdUpdate.Parameters.Add("@TechnicalInspectorId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.TechnicalInspectorId);
            cmdUpdate.Parameters.Add("@TransactionStatusId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.TransactionStatusId);
            cmdUpdate.Parameters.Add("@NextStatusId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.NextStatusId);
            cmdUpdate.Parameters.Add("@NextExpectedActionId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.NextExpectedActionId);
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.IsActive);
            cmdUpdate.Parameters.Add("@ModuleId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.ModuleId);
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.UpdatedBy);
            cmdUpdate.Parameters.Add("@TransactionStatusDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.TransactionStatusDate);
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.UpdatedOn);
            return cmdUpdate.ExecuteNonQuery();
        }


        public int ExecuteUpdationCommand(TblVehicleInOutDetailsTO tblVehicleInOutDetailsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblVehicleInOutDetails] SET " + 
            "  [moduleId]= @ModuleId" +
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
            " ,[technicalInspectorId] = @TechnicalInspectorId" +
            " WHERE 1 = 1 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@TechnicalInspectorId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.TechnicalInspectorId);
            cmdUpdate.Parameters.Add("@IdTblVehicleInOutDetails", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.IdTblVehicleInOutDetails);
            cmdUpdate.Parameters.Add("@ModuleId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.ModuleId);
            cmdUpdate.Parameters.Add("@TransactionTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.TransactionTypeId);
            cmdUpdate.Parameters.Add("@TransactionStatusId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.TransactionStatusId);
            cmdUpdate.Parameters.Add("@NextStatusId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.NextStatusId);
            cmdUpdate.Parameters.Add("@PartyId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.PartyId);
            cmdUpdate.Parameters.Add("@TransporterId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.TransporterId);
            cmdUpdate.Parameters.Add("@SupervisorId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.SupervisorId);
            cmdUpdate.Parameters.Add("@NextExpectedActionId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.NextExpectedActionId);
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.IsActive);
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.CreatedBy);
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.UpdatedBy);
            cmdUpdate.Parameters.Add("@TransactionDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.TransactionDate);
            cmdUpdate.Parameters.Add("@TransactionStatusDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.TransactionStatusDate);
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.CreatedOn);
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.UpdatedOn);
            cmdUpdate.Parameters.Add("@VehicleNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.VehicleNo);
            cmdUpdate.Parameters.Add("@TransactionNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.TransactionNo);
            cmdUpdate.Parameters.Add("@TransactionDisplayNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.TransactionDisplayNo);
            cmdUpdate.Parameters.Add("@ApplicationRouting", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.ApplicationRouting);
            cmdUpdate.Parameters.Add("@ScheduleRefNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblVehicleInOutDetailsTO.ScheduleRefNo);
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblVehicleInOutDetails(Int32 idTblVehicleInOutDetails)
        {
            String sqlConnStr = "";
                //Masters.GlobalConnectionString.ActiveConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idTblVehicleInOutDetails, cmdDelete);
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

        public  int DeleteTblVehicleInOutDetails(Int32 idTblVehicleInOutDetails, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idTblVehicleInOutDetails, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int32 idTblVehicleInOutDetails, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblVehicleInOutDetails] " +
            " WHERE idTblVehicleInOutDetails = " + idTblVehicleInOutDetails +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idTblVehicleInOutDetails", System.Data.SqlDbType.Int).Value = tblVehicleInOutDetailsTO.IdTblVehicleInOutDetails;
            return cmdDelete.ExecuteNonQuery();
        }

        public List<VehicleNumber> SelectAllVehicles()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlDataReader sqlReader = null;
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "SELECT distinct vehicleNo FROM tempLoading";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<VehicleNumber> list = new List<VehicleNumber>();
                if (sqlReader != null)
                {
                    while (sqlReader.Read())
                    {
                        String vehicleNo = string.Empty;
                        if (sqlReader["vehicleNo"] != DBNull.Value)
                            vehicleNo = Convert.ToString(sqlReader["vehicleNo"].ToString());

                        if (!string.IsNullOrEmpty(vehicleNo))
                        {
                            String[] vehNoPart = vehicleNo.Split(' ');
                            if (vehNoPart.Length == 4)
                            {
                                VehicleNumber vehicleNumber = new VehicleNumber();
                                for (int i = 0; i < vehNoPart.Length; i++)
                                {
                                    if (i == 0)
                                    {
                                        vehicleNumber.StateCode = vehNoPart[i].ToUpper();
                                    }
                                    if (i == 1)
                                    {
                                        vehicleNumber.DistrictCode = vehNoPart[i].ToUpper();
                                    }
                                    if (i == 2)
                                    {
                                        vehicleNumber.UniqueLetters = vehNoPart[i];
                                        if (vehicleNumber.UniqueLetters == "undefined")
                                            vehicleNumber.UniqueLetters = "";
                                        else
                                            vehicleNumber.UniqueLetters = vehicleNumber.UniqueLetters.ToUpper();
                                    }
                                    if (i == 3)
                                    {
                                        if (Constants.IsInteger(vehNoPart[i]))
                                        {
                                            vehicleNumber.VehicleNo = Convert.ToInt32(vehNoPart[i]);
                                        }
                                        else break;
                                    }
                                }

                                if (vehicleNumber.VehicleNo > 0)
                                    list.Add(vehicleNumber);
                            }
                        }
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public List<DropDownTO> GetPONoListAgainstSupplier(Int64 supplierId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlDataReader sqlReader = null;
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select * from tblCommercialDocument where transactionTypeId = "+ Convert.ToInt32(Constants.TransactionTypeE.PURCHASE_ORDER) 
                   + " and isActive =1 and transactionStatusId in ( "+Convert.ToInt32(Constants.TranStatusE.PURCHASE_ORDER_STATUS_AUTHORIZED)+","+
                   +Convert.ToInt32(Constants.TranStatusE.PURCHASE_ORDER_STATUS_PARTIALY)+")";
                if(supplierId > 0)
                {
                    cmdSelect.CommandText += " and organizationId = " + supplierId;
                }
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> list = new List<DropDownTO>();
                if (sqlReader != null)
                {
                    while (sqlReader.Read())
                    {
                        DropDownTO TO = new DropDownTO();
                        if (sqlReader["idCommercialDocument"] != DBNull.Value)
                            TO.Value = Convert.ToInt32(sqlReader["idCommercialDocument"].ToString());
                        if (sqlReader["transactionNo"] != DBNull.Value)
                            TO.Text = Convert.ToString(sqlReader["transactionNo"].ToString());
                        list.Add(TO);
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        #endregion


        public int InsertTblCommericalDocStatusHistory(TblCommericalDocStatusHistoryTO tblCommericalDocStatusHistoryTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommandStatusHistory(tblCommericalDocStatusHistoryTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        private int ExecuteInsertionCommandStatusHistory(TblCommericalDocStatusHistoryTO tblCommericalDocStatusHistoryTO, SqlCommand cmdInsert)
        {
            
                String sqlQuery = @" INSERT INTO [tblCommericalDocStatusHistory]( " +
                "  [statusId]" +
                " ,[isComment]" +
                " ,[createdBy]" +
                " ,[statusDate]" +
                " ,[createdOn]" +
                //" ,[idCommerDocStatusHistory]" +
                " ,[commercialDocumentId]" +
                " ,[statusRemark]" +
                " )" +
    " VALUES (" +
                "  @StatusId " +
                " ,@IsComment " +
                " ,@CreatedBy " +
                " ,@StatusDate " +
                " ,@CreatedOn " +
                //" ,@IdCommerDocStatusHistory " +
                " ,@CommercialDocumentId " +
                " ,@StatusRemark " +
                " )";
                cmdInsert.CommandText = sqlQuery;
                cmdInsert.CommandType = System.Data.CommandType.Text;

                cmdInsert.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblCommericalDocStatusHistoryTO.StatusId;
                cmdInsert.Parameters.Add("@IsComment", System.Data.SqlDbType.Int).Value = tblCommericalDocStatusHistoryTO.IsComment;
                cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblCommericalDocStatusHistoryTO.CreatedBy;
                cmdInsert.Parameters.Add("@StatusDate", System.Data.SqlDbType.DateTime).Value = tblCommericalDocStatusHistoryTO.StatusDate;
                cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblCommericalDocStatusHistoryTO.CreatedOn;
                // cmdInsert.Parameters.Add("@IdCommerDocStatusHistory", System.Data.SqlDbType.BigInt).Value = tblCommericalDocStatusHistoryTO.IdCommerDocStatusHistory;
                cmdInsert.Parameters.Add("@CommercialDocumentId", System.Data.SqlDbType.BigInt).Value = tblCommericalDocStatusHistoryTO.CommercialDocumentId;
                cmdInsert.Parameters.Add("@StatusRemark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblCommericalDocStatusHistoryTO.StatusRemark);
                return cmdInsert.ExecuteNonQuery();
            
        }
    }
}
