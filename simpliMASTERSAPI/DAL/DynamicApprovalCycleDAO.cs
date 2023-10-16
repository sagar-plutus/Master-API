using System;
using System.Collections;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System.Data;
using System.Collections.Generic;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL.Interfaces;
using simpliMASTERSAPI.TO;
using Newtonsoft.Json;
using System.Linq;

namespace ODLMWebAPI.DAL
{
    public class DynamicApprovalCycleDAO : IDynamicApprovalCycleDAO
    {
        private readonly IConnectionString _iConnectionString;
        private readonly ISQLHelper _sqlHelper;
        private readonly ICommon _icommon;
        private readonly ITblOrgStructureBL _iTblOrgStructureBL;
        public DynamicApprovalCycleDAO(IConnectionString iConnectionString, ISQLHelper _sqlHelper, ICommon _icommon, ITblOrgStructureBL iTblOrgStructureBL)
        {
            this._sqlHelper = _sqlHelper;
            this._icommon = _icommon;
            _iConnectionString = iConnectionString;
            _iTblOrgStructureBL = iTblOrgStructureBL;
        }
         public List<DimDynamicApprovalTO> SelectAllApprovalList(int idModule = 0, int area = 0, int userId = 0)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText ="select * from dimApproval where isActive=1 and moduleId = " +idModule +" and approvalId = "+ area;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idState", System.Data.SqlDbType.Int).Value = dimStateTO.IdState;
                SqlDataReader allApproval = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimDynamicApprovalTO> ApprovalList = ConvertDTToList(allApproval);
                if (ApprovalList != null)
                    return ApprovalList;
                else
                    return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllApprovalList");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }



        public DataTable SelectAllList(int seqNo)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select * from dimApproval where isActive=1 and sequenceNo=" + seqNo;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idState", System.Data.SqlDbType.Int).Value = dimStateTO.IdState;
                SqlDataReader allApprovalList = cmdSelect.ExecuteReader(CommandBehavior.Default);
                DataTable dt = new DataTable();
                dt.Load(allApprovalList);

                if (dt.Rows.Count > 0)
                {
                    string SelectQuery = Convert.ToString(dt.Rows[0]["selectQuery"]);
                    cmdSelect.CommandText = SelectQuery;

                    return _sqlHelper.ExecuteReaderDataTable<DataTable>(System.Data.CommandType.Text, SelectQuery, sqlConnStr, null);
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllList");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        /// <summary>
        /// AmolG[2020-Dec-22] Added UserId for Showing hirachy wise data. If the setting is applied.
        /// </summary>
        /// <param name="seqNo"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataTable SelectAllList(int seqNo, int userId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select * from dimApproval where isActive=1 and sequenceNo=" + seqNo;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idState", System.Data.SqlDbType.Int).Value = dimStateTO.IdState;
                SqlDataReader allApprovalList = cmdSelect.ExecuteReader(CommandBehavior.Default);
                DataTable dt = new DataTable();
                dt.Load(allApprovalList);

                if (dt.Rows.Count > 0)
                {
                    string toFind = "order by";
                    string SelectQuery = Convert.ToString(dt.Rows[0]["selectQuery"]);
                    string OrderBy = SelectQuery.Substring(SelectQuery.ToLower().IndexOf(toFind));
                    if(OrderBy == "")
                    {
                        OrderBy = "ORDER BY Id DESC";
                    }
                    else
                    {
                        SelectQuery = SelectQuery.Replace(OrderBy, "");
                    }
                    //AmolG[2020-Dec-22] User Hirarchy wise show list
                    if (!String.IsNullOrEmpty(dt.Rows[0]["isSelectDataByUserHirachy"].ToString()))
                    {
                        Boolean isUseUserHirarchy = false;
                        String userIds = "";
                        try
                        {
                            isUseUserHirarchy = Convert.ToBoolean(dt.Rows[0]["isSelectDataByUserHirachy"]);
                            //For Admin User Has to see all Requests
                            if (userId == 1)
                            {
                                isUseUserHirarchy = false;
                            }

                            if (isUseUserHirarchy)
                            {
                                String userColName = dt.Rows[0]["userColName"].ToString();
                                if (!String.IsNullOrEmpty(userColName))
                                {
                                    OrderBy = userColName.Substring(userColName.ToLower().IndexOf(toFind));
                                    if (OrderBy == "")
                                    {
                                        OrderBy = "ORDER BY Id DESC";
                                    }
                                    else
                                    {
                                        userColName = userColName.Replace(OrderBy, "");
                                    }
                                    object userList = _iTblOrgStructureBL.ChildUserListOnUserId(userId, 1, 1);
                                    if (userList != null && userList.GetType() == typeof(List<int>))
                                    {
                                        List<int> users = (List<int>)userList;
                                        if (users != null && users.Count > 0)
                                        {
                                            //Reshma Added For showing Administrative user request.
                                            List<TblUserReportingDetailsTO> allUserReportingDetailsList = _iTblOrgStructureBL.SelectAllUserReportingDetails();
                                            List<TblUserReportingDetailsTO> usersonUserIdList = allUserReportingDetailsList.Where(ele => ele.UserId == userId).ToList();
                                            if (usersonUserIdList != null && usersonUserIdList.Count > 0)
                                            {
                                                if (usersonUserIdList.Count == 1 && usersonUserIdList[0].ReportingTo ==0 )
                                                {
                                                    TblUserReportingDetailsTO TblUserReportingDetailsTOTemp = usersonUserIdList[0];
                                                    users.Add(TblUserReportingDetailsTOTemp.UserId);
                                                }
                                            }
                                            userIds = String.Join(',', users);
                                            //userIds += "," + userId;

                                            userIds = userIds.TrimStart(',');

                                            String condition = " In ( " + userIds + ")";

                                            userColName = userColName.Replace("#ReplaceCondition", condition);

                                            SelectQuery += userColName;
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    SelectQuery = SelectQuery + " " + OrderBy;
                    cmdSelect.CommandText = SelectQuery;

                    return _sqlHelper.ExecuteReaderDataTable<DataTable>(System.Data.CommandType.Text, SelectQuery, sqlConnStr, null);
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllList");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        //Pandurang[2021-01-14] as duplicate functions found
        ///// <summary>
        ///// AmolG[2020-Dec-22] Added UserId for Showing hirachy wise data. If the setting is applied.
        ///// </summary>
        ///// <param name="seqNo"></param>
        ///// <param name="userId"></param>
        ///// <returns></returns>
        //public DataTable SelectAllList(int seqNo, int userId)
        //{
        //    String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
        //    SqlConnection conn = new SqlConnection(sqlConnStr);
        //    SqlCommand cmdSelect = new SqlCommand();
        //    ResultMessage resultMessage = new ResultMessage();
        //    try
        //    {
        //        conn.Open();
        //        cmdSelect.CommandText = "select * from dimApproval where isActive=1 and sequenceNo=" + seqNo;
        //        cmdSelect.Connection = conn;
        //        cmdSelect.CommandType = System.Data.CommandType.Text;

        //        //cmdSelect.Parameters.Add("@idState", System.Data.SqlDbType.Int).Value = dimStateTO.IdState;
        //        SqlDataReader allApprovalList = cmdSelect.ExecuteReader(CommandBehavior.Default);
        //        DataTable dt = new DataTable();
        //        dt.Load(allApprovalList);

        //        if (dt.Rows.Count > 0)
        //        {
        //            string SelectQuery = Convert.ToString(dt.Rows[0]["selectQuery"]);

        //            //AmolG[2020-Dec-22] User Hirarchy wise show list
        //            if (!String.IsNullOrEmpty(dt.Rows[0]["isSelectDataByUserHirachy"].ToString()))
        //            {
        //                Boolean isUseUserHirarchy = false;
        //                String userIds = "";
        //                try
        //                {
        //                    isUseUserHirarchy = Convert.ToBoolean(dt.Rows[0]["isSelectDataByUserHirachy"]);
        //                    //For Admin User Has to see all Requests
        //                    if (userId == 1)
        //                    {
        //                        isUseUserHirarchy = false;
        //                    }

        //                    if (isUseUserHirarchy)
        //                    {
        //                        String userColName = dt.Rows[0]["userColName"].ToString();
        //                        if (!String.IsNullOrEmpty(userColName))
        //                        {
        //                            object userList = _iTblOrgStructureBL.ChildUserListOnUserId(userId, 1, 1);
        //                            if (userList != null && userList.GetType() == typeof(List<int>))
        //                            {
        //                                List<int> users = (List<int>)userList;
        //                                if (users != null && users.Count > 0)
        //                                {
        //                                    userIds = String.Join(',', users);
        //                                    userIds += "," + userId;

        //                                    userIds = userIds.TrimStart(',');

        //                                    String condition = " In ( " + userIds + ")";

        //                                    userColName = userColName.Replace("#ReplaceCondition", condition);

        //                                    SelectQuery += userColName;
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                catch (Exception ex)
        //                {

        //                }
        //            }
        //            cmdSelect.CommandText = SelectQuery;

        //            return _sqlHelper.ExecuteReaderDataTable<DataTable>(System.Data.CommandType.Text, SelectQuery, sqlConnStr, null);
        //        }
        //        else
        //            return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        resultMessage.DefaultExceptionBehaviour(ex, "SelectAllList");
        //        return null;
        //    }
        //    finally
        //    {
        //        conn.Close();
        //        cmdSelect.Dispose();
        //    }
        //}

        public DataTable GetDetailsById(int idDetails,int idApprovalActions)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select * from dimApprovalActions where idApprovalActions = " + idApprovalActions;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader allApprovalList = cmdSelect.ExecuteReader(CommandBehavior.Default);
                DataTable dt = new DataTable();
                dt.Load(allApprovalList);

                if (dt.Rows.Count > 0)
                {
                    string SelectQuery = Convert.ToString(dt.Rows[0]["actionQuery"]);
                    cmdSelect.CommandText = SelectQuery;
                    SqlParameter[] parameters = {new SqlParameter() };
                    parameters[0].Value = idDetails;
                    parameters[0].ParameterName = "@idDetails";

                    return _sqlHelper.ExecuteReaderDataTable<DataTable>(System.Data.CommandType.Text, SelectQuery, sqlConnStr, parameters);
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllList");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        public List<DimApprovalActionsTO> GetActionIconList(int idApproval)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select * from dimApprovalActions where isActive=1 and approvalId =" + idApproval + " order by sequanceNo asc";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idState", System.Data.SqlDbType.Int).Value = dimStateTO.IdState;
                SqlDataReader allApproval = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimApprovalActionsTO> ApprovalAtionList = ConvertDTToListActions(allApproval);
                if (ApprovalAtionList != null)
                    return ApprovalAtionList;
                else
                    return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllList");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        public int UpdateStatus(Dictionary<string, string> tableData, int status, int userId, int seqNo, ref string txtCommentMsg, ref Int32 OrganizationId,Int32 isApprovByDir,ref string TransactionNo,ref int withinCriteria)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlTransaction tran = null;
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                DateTime currentdate = _icommon.ServerDateTime;
                Int32 areaId;
                string moduleId;
                conn.Open();
                cmdSelect.CommandText = "select * from dimApproval where isActive=1 and sequenceNo=" + seqNo;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idState", System.Data.SqlDbType.Int).Value = dimStateTO.IdState;
                SqlDataReader allApprovalList = cmdSelect.ExecuteReader(CommandBehavior.Default);
                DataTable dt = new DataTable();
                dt.Load(allApprovalList);
                conn.Close();
                if (dt.Rows.Count > 0)
                {
                    string updateCommandType= "Text";
                    string SelectQuery = Convert.ToString(dt.Rows[0]["updateQuery"]);
                    string authorisedStatusId = Convert.ToString(dt.Rows[0]["authorisedStatusId"]);
                    double approvalCriteria = Convert.ToDouble(dt.Rows[0]["approvalCriteria"]);
                    Int32 approvalCriteriaStatus = Convert.ToInt32(dt.Rows[0]["approvalCriteriaStatus"]);
                    Int32 Idapproval = Convert.ToInt32(dt.Rows[0]["idapproval"]);
                    string rejectStatusId = Convert.ToString(dt.Rows[0]["rejectStatusId"]);
                    areaId = Convert.ToInt32(dt.Rows[0]["approvalId"]);
                    moduleId = Convert.ToString(dt.Rows[0]["moduleId"]);
                    updateCommandType = Convert.ToString(dt.Rows[0]["updateCommandType"]);
                    txtCommentMsg = Convert.ToString(dt.Rows[0]["notificationText"]);
                    string notifyQuery = "";
                    notifyQuery = Convert.ToString(dt.Rows[0]["notificationQuery"]);

                    if (areaId == (Int32)Constants.dimApprovalAreaE.PURCHASE_REQUEST && (status == 1 || status == 6))
                    {
                        TblPurchaseRequestTo tblPurchaseRequestTo = new TblPurchaseRequestTo();
                        tblPurchaseRequestTo.IdPurchaseRequest = Convert.ToInt32(tableData["Id"]);
                        tblPurchaseRequestTo.CreatedBy = userId;
                        tblPurchaseRequestTo.UpdatedBy = userId;
                        tblPurchaseRequestTo.ApprovalActionValue = status;
                        resultMessage = JsonConvert.DeserializeObject<ResultMessage>(_icommon.AddConsolidationByPurchaseRequest(tblPurchaseRequestTo));
                        if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                        {
                            return 0;
                        }
                        if (status == 1)
                        {
                            resultMessage = JsonConvert.DeserializeObject<ResultMessage>(_icommon.StockTransfer(tblPurchaseRequestTo));
                            if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                            {
                                return 0;
                            }
                        }
                    }

                    if (notifyQuery != null && notifyQuery != "")
                    {
                        conn.Open();
                        cmdSelect.CommandText = notifyQuery;
                        cmdSelect.Connection = conn;
                        cmdSelect.CommandType = System.Data.CommandType.Text;

                        cmdSelect.Parameters.Add("@idCommercialDocument", System.Data.SqlDbType.Int).Value = tableData["Id"];
                        SqlDataReader rdrUser = cmdSelect.ExecuteReader(CommandBehavior.Default);
                        DataTable dtUser = new DataTable();
                        dtUser.Load(rdrUser);
                        conn.Close();
                        if (dtUser.Rows.Count > 0)
                        {
                            OrganizationId = Convert.ToInt32(dtUser.Rows[0]["UserId"]);
                            TransactionNo = Convert.ToString(dtUser.Rows[0]["transactionNo"]);
                        }
                    }




                    conn.Open();
                    tran = conn.BeginTransaction();
                    SqlCommand cmdUpdate = new SqlCommand();
                    cmdUpdate.CommandText = SelectQuery;
                    if (updateCommandType == "Text")
                    {
                        cmdUpdate.CommandType = System.Data.CommandType.Text;
                    }
                   else {
                        cmdUpdate.CommandType = System.Data.CommandType.StoredProcedure;
                    }
                    cmdUpdate.Connection = conn;
                    cmdUpdate.Transaction = tran;
                    String StatusValue = "";
                    if (status == 1)
                    {
                        StatusValue = authorisedStatusId;
                        if (approvalCriteria != null && approvalCriteria > 0)
                        {
                            double val = Convert.ToDouble(tableData["Grand Total"]);
                            if(val > approvalCriteria)
                            {
                                StatusValue = approvalCriteriaStatus.ToString();
                                withinCriteria = 1;
                            }
                        }
                        //cmdUpdate.Parameters.AddWithValue("@status", authorisedStatusId);
                    }
                    else
                    {
                        StatusValue = rejectStatusId;
                        //cmdUpdate.Parameters.AddWithValue("@status", rejectStatusId);
                    }
                    if (status > 1 && status != 6)// 6 is for rejection which was 0 previously Deepali[11-11-2020]
                    {
                        cmdSelect.CommandText = "select * from dimApprovalActions where approvalId = " + Idapproval + " and actionVal = " + status + "";
                        cmdSelect.Connection = conn;
                        cmdSelect.Transaction = tran;
                        cmdSelect.CommandType = System.Data.CommandType.Text;
                        SqlDataReader ApprovalActionList = cmdSelect.ExecuteReader(CommandBehavior.Default);
                        DataTable dt1 = new DataTable();
                        dt1.Load(ApprovalActionList);
                        if (dt1.Rows.Count > 0)
                        {
                            Int32 ActionWiseStatus = Convert.ToInt32(dt1.Rows[0]["StatusId"]);
                            if(ActionWiseStatus != 0)
                            {
                                StatusValue = ActionWiseStatus.ToString();
                                //cmdUpdate.Parameters.AddWithValue("@status", ActionWiseStatus);
                            }
                        }
                    }
                    cmdUpdate.Parameters.AddWithValue("@status", StatusValue);
                    if (isApprovByDir != -1)
                    {
                        cmdUpdate.Parameters.AddWithValue("@isApprovedByDirector", isApprovByDir);
                    }

                    cmdUpdate.Parameters.AddWithValue("@updatedBy", userId);
                    cmdUpdate.Parameters.AddWithValue("@updatedOn", currentdate);
                    cmdUpdate.Parameters.AddWithValue("@id", tableData["Id"]);



                    int result = cmdUpdate.ExecuteNonQuery();
                    if (result <= 0)
                    {
                        tran.Rollback();
                        return 0;
                    }
                    SqlCommand cmdInsert = new SqlCommand();
                    cmdInsert.CommandText = "insert into dynamicApprovalHistory values (@transactionId,@statusId,@areaId,@moduleId,@createdOn,@createdBy)";

                    cmdInsert.Connection = conn;
                    cmdInsert.Transaction = tran;
                    cmdInsert.CommandType = System.Data.CommandType.Text;
                    cmdInsert.Parameters.AddWithValue("@transactionId", tableData["Id"]);
                    if (status == 1)
                    {
                        cmdInsert.Parameters.AddWithValue("@statusId", authorisedStatusId);
                    }
                    else
                    {
                        cmdInsert.Parameters.AddWithValue("@statusId", rejectStatusId);
                    }
                    cmdInsert.Parameters.AddWithValue("@areaId", areaId);
                    cmdInsert.Parameters.AddWithValue("@moduleId", moduleId);
                    cmdInsert.Parameters.AddWithValue("@createdOn", currentdate);
                    cmdInsert.Parameters.AddWithValue("@createdBy", userId);
                    result = cmdInsert.ExecuteNonQuery();
                    if (result != 1)
                    {
                        tran.Rollback();
                        return 0;

                    }
                   
                    tran.Commit();
                    return 1;



                }
                return 0;

            }
            catch (Exception ex)
            {
                tran.Rollback();

                resultMessage.DefaultExceptionBehaviour(ex, "UpdateStatus");
                return 0;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }

        }

       public int updatePurchaseRequestComments(DropDownTO purchaseTO, int userId)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
                SqlConnection conn = new SqlConnection(sqlConnStr);
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(purchaseTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(DropDownTO purchaseTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = "";
            if (purchaseTO.Tag.ToString() == "1")
            {
                sqlQuery = @" update tblPurchaseRequest set rejectionComment =@RejectionComment where idPurchaseRequest = @IdPurchaseRequest";
            }
            else if(purchaseTO.Tag.ToString() == "2")
            {
                sqlQuery = @" update tblPurchaseRequest set dirComment =@RejectionComment where idPurchaseRequest = @IdPurchaseRequest";
            }
            else if (purchaseTO.Tag.ToString() == "3")
            {
                sqlQuery = @" update tblPurchaseRequest set dirCommentAuthLevel2 =@RejectionComment where idPurchaseRequest = @IdPurchaseRequest";
            }

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdPurchaseRequest", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(purchaseTO.Value);
            cmdUpdate.Parameters.Add("@RejectionComment", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(purchaseTO.Text);
            return cmdUpdate.ExecuteNonQuery();
        }

        public int updateCommercialDocComments(DropDownTO CommDocTO, int userId)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
                SqlConnection conn = new SqlConnection(sqlConnStr);
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommandCommDoc(CommDocTO, cmdUpdate);
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

        public int ExecuteUpdationCommandCommDoc(DropDownTO CommDocTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = "";
            if (CommDocTO.Tag.ToString() == "1")
            {
                sqlQuery = @" update tblCommercialDocument set rejectionReason =@rejectionReason where idCommercialDocument = @idCommercialDocument";
            }
            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@idCommercialDocument", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(CommDocTO.Value);
            cmdUpdate.Parameters.Add("@rejectionReason", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(CommDocTO.Text);
            return cmdUpdate.ExecuteNonQuery();
        }
        public int UpdateIsMigrationFlag(Int64 commercialDocumentId)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
                SqlConnection conn = new SqlConnection(sqlConnStr);
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommandUpdateIsMigrationFlag(commercialDocumentId, cmdUpdate);
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
        // Add By Samadhan 5 May 2023
        public int UpdateIsOnePageSummaryMigrationFlag(Int64 commercialDocumentId)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
                SqlConnection conn = new SqlConnection(sqlConnStr);
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommandUpdateIsOnePageSummaryMigrationFlag(commercialDocumentId, cmdUpdate);
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
        
        public int ExecuteUpdationCommandUpdateIsMigrationFlag(Int64 commercialDocumentId, SqlCommand cmdUpdate)
        {
            String sqlQuery = "";            
                sqlQuery = @" update tblCommericalDocItemDtls set isMigration =0 where commercialDocumentId = @idCommercialDocument";
            
            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@idCommercialDocument", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(commercialDocumentId);

            return cmdUpdate.ExecuteNonQuery();
        }

        // Add By Samadhan 5 May 2023
        public int ExecuteUpdationCommandUpdateIsOnePageSummaryMigrationFlag(Int64 commercialDocumentId, SqlCommand cmdUpdate)
        {
            String sqlQuery = "";
            sqlQuery = @" update tblCommercialDocument set isPOOnePageSummaryMigration =0,isGRNOnePageSummaryMigration=0 where idCommercialDocument = @idCommercialDocument";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@idCommercialDocument", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(commercialDocumentId);

            return cmdUpdate.ExecuteNonQuery();
        }

        


        public int InsertTblPurchaseRequestStatusHistory(Dictionary<string, string> tableData, int Status, Int32 userId)
        {
            SqlCommand cmdInsert = new SqlCommand();
            SqlCommand cmdSelect = new SqlCommand();
            int StatusId = 0;
            try
            {
                String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
                SqlConnection conn = new SqlConnection(sqlConnStr);
                conn.Open();

                // Get Latest Status
                cmdSelect.CommandText = "select  * from tblPurchaseRequest where  idPurchaseRequest=" + Convert.ToString(tableData["Id"]);
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader PurchaseRequestData = cmdSelect.ExecuteReader(CommandBehavior.Default);
                DataTable dt = new DataTable();
                dt.Load(PurchaseRequestData);
                conn.Close();
                if (dt.Rows.Count > 0)
                {
                    StatusId = Convert.ToInt32(dt.Rows[0]["statusId"]);
                }

                //
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommandforPurchaseRequestStatusHistory(tableData, StatusId, userId, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }
        public int ExecuteInsertionCommandforPurchaseRequestStatusHistory(Dictionary<string, string> tableData, int Status, Int32 userId, SqlCommand cmdInsert)
        {
            DateTime currentdate = _icommon.ServerDateTime;
            String sqlQuery = @" INSERT INTO [tblPurchaseRequestStatusHistory]( " +
            "  [statusId]" +
            " ,[createdBy]" +
            " ,[statusDate]" +
            " ,[createdOn]" +
            " ,[PurchaseRequestId]" +
            " ,[IsComment]" +
            " )" +
    " VALUES (" +
            "  @StatusId " +
            " ,@CreatedBy " +
            " ,@StatusDate " +
            " ,@CreatedOn " +
            " ,@PurchaseRequestId " +
             " ,@IsComment " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = Status;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = userId;
            cmdInsert.Parameters.Add("@StatusDate", System.Data.SqlDbType.DateTime).Value = currentdate;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = currentdate;
            cmdInsert.Parameters.Add("@PurchaseRequestId", System.Data.SqlDbType.BigInt).Value = Convert.ToInt32(tableData["Id"]);
            cmdInsert.Parameters.Add("@IsComment", System.Data.SqlDbType.Int).Value = 0;
            return cmdInsert.ExecuteNonQuery();
        }
        public int InsertCommercialDocPaymentStatusHistory(Dictionary<string, string> tableData, int Status, Int32 userId)
        {
            SqlCommand cmdInsert = new SqlCommand();
            SqlCommand cmdSelect = new SqlCommand();
            int StatusId = 0;
            try
            {
                String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
                SqlConnection conn = new SqlConnection(sqlConnStr);
                conn.Open();

                // Get Latest Status
                cmdSelect.CommandText = "select  * from tblCommercialDocument where  idCommercialDocument=" + Convert.ToString(tableData["Id"]);
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader PurchaseRequestData = cmdSelect.ExecuteReader(CommandBehavior.Default);
                DataTable dt = new DataTable();
                dt.Load(PurchaseRequestData);
                conn.Close();
                if (dt.Rows.Count > 0)
                {
                    StatusId = Convert.ToInt32(dt.Rows[0]["transactionStatusId"]);
                }
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommandforCommercialStatusHistory(tableData, StatusId, userId, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }

        }
        public int ExecuteInsertionCommandforCommercialStatusHistory(Dictionary<string, string> tableData, int Status, Int32 userId, SqlCommand cmdInsert)
        {
            DateTime currentdate = _icommon.ServerDateTime;
            String sqlQuery = @" INSERT INTO [tblCommercialDocPaymentStatusHistory]( " +
            "  [statusId]" +
            " ,[createdBy]" +
            " ,[createdOn]" +
            " ,[commercialDocumentId]" +
            " )" +
    " VALUES (" +
            "  @StatusId " +
            " ,@CreatedBy " +
            " ,@CreatedOn " +
            " ,@commercialDocumentId " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = Status;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = userId;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = currentdate;
            cmdInsert.Parameters.Add("@commercialDocumentId", System.Data.SqlDbType.BigInt).Value = Convert.ToInt32(tableData["Id"]);
            return cmdInsert.ExecuteNonQuery();
        }
        public List<DimDynamicApprovalTO> ConvertDTToList(SqlDataReader dimVehicleTypeTODT)
        {
            List<DimDynamicApprovalTO> dimVehicleTypeTOList = new List<DimDynamicApprovalTO>();
            if (dimVehicleTypeTODT != null)
            {
                while (dimVehicleTypeTODT.Read())
                {
                    DimDynamicApprovalTO dimDynamciAoproval = new DimDynamicApprovalTO();
                    if (dimVehicleTypeTODT["approvalName"] != DBNull.Value)
                        dimDynamciAoproval.ApprovalName =(dimVehicleTypeTODT["approvalName"].ToString());
                    if (dimVehicleTypeTODT["sequenceNo"] != DBNull.Value)
                        dimDynamciAoproval.SequenceNo = Convert.ToInt32(dimVehicleTypeTODT["sequenceNo"].ToString());
                    if (dimVehicleTypeTODT["idapproval"] != DBNull.Value)
                        dimDynamciAoproval.IdApproval = Convert.ToInt32(dimVehicleTypeTODT["idapproval"].ToString());

                    if (dimVehicleTypeTODT["approvalTypeId"] != DBNull.Value)
                        dimDynamciAoproval.ApprovalTypeId = Convert.ToInt32(dimVehicleTypeTODT["approvalTypeId"].ToString());
                       
                          if (dimVehicleTypeTODT["currentStatusId"] != DBNull.Value)
                        dimDynamciAoproval.CurrentStatusId = Convert.ToInt32(dimVehicleTypeTODT["currentStatusId"].ToString());
                          if (dimVehicleTypeTODT["authorisedStatusId"] != DBNull.Value)
                        dimDynamciAoproval.AuthorisedStatusId = Convert.ToInt32(dimVehicleTypeTODT["authorisedStatusId"].ToString());
                        
                          if (dimVehicleTypeTODT["rejectStatusId"] != DBNull.Value)
                        dimDynamciAoproval.RejectStatusId = Convert.ToInt32(dimVehicleTypeTODT["rejectStatusId"].ToString());
                     if (dimVehicleTypeTODT["isActive"] != DBNull.Value)
                        dimDynamciAoproval.IsActive = Convert.ToInt32(dimVehicleTypeTODT["isActive"].ToString());
                         if (dimVehicleTypeTODT["bootstrapIconName"] != DBNull.Value)
                        dimDynamciAoproval.BootstrapIconName = (dimVehicleTypeTODT["bootstrapIconName"].ToString()); 
                          if (dimVehicleTypeTODT["sysElementId"] != DBNull.Value)
                        dimDynamciAoproval.SysElementId = Convert.ToInt32(dimVehicleTypeTODT["sysElementId"].ToString());
                         if (dimVehicleTypeTODT["moduleId"] != DBNull.Value)
                        dimDynamciAoproval.ModuleId = Convert.ToInt32(dimVehicleTypeTODT["moduleId"].ToString());
                          if (dimVehicleTypeTODT["selectQuery"] != DBNull.Value)
                        dimDynamciAoproval.SelectQuery = (dimVehicleTypeTODT["selectQuery"].ToString()); 
                          if (dimVehicleTypeTODT["updateQuery"] != DBNull.Value)
                        dimDynamciAoproval.UpdateQuery = (dimVehicleTypeTODT["updateQuery"].ToString());

                    if (dimVehicleTypeTODT["approvalCriteria"] != DBNull.Value)
                        dimDynamciAoproval.ApprovalCriteria = Convert.ToDouble(dimVehicleTypeTODT["approvalCriteria"].ToString());

                    if (dimVehicleTypeTODT["approvalCriteriaStatus"] != DBNull.Value)
                        dimDynamciAoproval.ApprovalCriteriaStatus = Convert.ToInt32(dimVehicleTypeTODT["approvalCriteriaStatus"].ToString());

                    dimVehicleTypeTOList.Add(dimDynamciAoproval);
                }
            }
            return dimVehicleTypeTOList;
        }

        public List<DimApprovalActionsTO> ConvertDTToListActions(SqlDataReader dimApprovalActionsTODT)
        {
            List<DimApprovalActionsTO> dimApprovalActionsTOList = new List<DimApprovalActionsTO>();
            if (dimApprovalActionsTODT != null)
            {
                while (dimApprovalActionsTODT.Read())
                {
                    DimApprovalActionsTO dimApprovalActionsTONew = new DimApprovalActionsTO();
                    if (dimApprovalActionsTODT["idApprovalActions"] != DBNull.Value)
                        dimApprovalActionsTONew.IdApprovalActions = Convert.ToInt32(dimApprovalActionsTODT["idApprovalActions"].ToString());
                    if (dimApprovalActionsTODT["approvalId"] != DBNull.Value)
                        dimApprovalActionsTONew.ApprovalId = Convert.ToInt32(dimApprovalActionsTODT["approvalId"].ToString());
                    if (dimApprovalActionsTODT["sysElementId"] != DBNull.Value)
                        dimApprovalActionsTONew.SysElementId = Convert.ToInt32(dimApprovalActionsTODT["sysElementId"].ToString());
                    if (dimApprovalActionsTODT["isActive"] != DBNull.Value)
                        dimApprovalActionsTONew.IsActive = Convert.ToInt32(dimApprovalActionsTODT["isActive"].ToString());
                    if (dimApprovalActionsTODT["sequanceNo"] != DBNull.Value)
                        dimApprovalActionsTONew.SequanceNo = Convert.ToInt32(dimApprovalActionsTODT["sequanceNo"].ToString());
                    if (dimApprovalActionsTODT["actionVal"] != DBNull.Value)
                        dimApprovalActionsTONew.ActionVal = Convert.ToInt32(dimApprovalActionsTODT["actionVal"].ToString());
                    if (dimApprovalActionsTODT["bootstrapIconName"] != DBNull.Value)
                        dimApprovalActionsTONew.BootstrapIconName = Convert.ToString(dimApprovalActionsTODT["bootstrapIconName"].ToString());
                    if (dimApprovalActionsTODT["toottip"] != DBNull.Value)
                        dimApprovalActionsTONew.Toottip = Convert.ToString(dimApprovalActionsTODT["toottip"].ToString());
                    if (dimApprovalActionsTODT["actionHeading"] != DBNull.Value)
                        dimApprovalActionsTONew.ActionHeading = Convert.ToString(dimApprovalActionsTODT["actionHeading"].ToString());
                    if (dimApprovalActionsTODT["actionQuery"] != DBNull.Value)
                        dimApprovalActionsTONew.ActionQuery = Convert.ToString(dimApprovalActionsTODT["actionQuery"].ToString());
                    if (dimApprovalActionsTODT["actionId"] != DBNull.Value)
                        dimApprovalActionsTONew.ActionId = Convert.ToString(dimApprovalActionsTODT["actionId"].ToString());
                    if (dimApprovalActionsTODT["dimApprovalActionsTypeId"] != DBNull.Value)
                        dimApprovalActionsTONew.DimApprovalActionsTypeId = Convert.ToInt32(dimApprovalActionsTODT["dimApprovalActionsTypeId"].ToString());
                    if (dimApprovalActionsTODT["baseAPIUrlForAction"] != DBNull.Value)
                        dimApprovalActionsTONew.BaseAPIUrlForAction = Convert.ToString(dimApprovalActionsTODT["baseAPIUrlForAction"].ToString());
                    if (dimApprovalActionsTODT["apiMethodForAction"] != DBNull.Value)
                        dimApprovalActionsTONew.ApiMethodForAction = Convert.ToString(dimApprovalActionsTODT["apiMethodForAction"].ToString());
                    dimApprovalActionsTOList.Add(dimApprovalActionsTONew);
                }
            }
            return dimApprovalActionsTOList;
        }

        public string GetLatestStatus(Int64 commercialDocumentId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlTransaction tran = null;
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                String sqlQuery = "SELECT transactionStatusId from tblCommercialDocument WHERE idCommercialDocument = '" + commercialDocumentId + "' ";

                cmdSelect = new SqlCommand(sqlQuery, conn, tran);
                object obj = cmdSelect.ExecuteScalar();
                return Convert.ToString(obj);
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
    }
  
}