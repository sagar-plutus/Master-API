using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using simpliMASTERSAPI.TO;
using simpliMASTERSAPI.DAL;
using simpliMASTERSAPI.DAL.Interfaces;
using simpliMASTERSAPI.BL.Interfaces;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.Models;
using ODLMWebAPI.DAL.Interfaces;
using System.Linq;

namespace simpliMASTERSAPI.BL
{
    public class TblVehicleInOutDetailsBL : ITblVehicleInOutDetailsBL
    {
        private readonly IConnectionString _iConnectionString;
        private readonly ITblUnLoadingBL _iTblUnLoadingBL;
        private readonly ITblAlertInstanceBL _iTblAlertInstanceBL;
        private readonly ITblUserBL _iTblUserBL;
        private readonly INotification _iNotification;
        private readonly ITblCommercialDocScheduleDetailsDAO _iTblCommercialDocScheduleDetailsDAO; 

        private readonly ITblVehicleInOutDetailsDAO _iTblVehicleInOutDetailsDAO;
        private readonly ITblVehicleInOutDetailsHistoryDAO _iTblVehicleInOutDetailsHistoryDAO;
        private readonly ITblPersonDAO _iTblPersonDAO;
        public TblVehicleInOutDetailsBL(ITblVehicleInOutDetailsDAO iTblVehicleInOutDetailsDAO, ITblPersonDAO iTblPersonDAO, ITblUserBL iTblUserBL, ITblAlertInstanceBL iTblAlertInstanceBL, INotification iNotification, ITblCommercialDocScheduleDetailsDAO iTblCommercialDocScheduleDetailsDAO, ITblUnLoadingBL iTblUnLoadingBL, IConnectionString iConnectionString, ITblVehicleInOutDetailsHistoryDAO iTblVehicleInOutDetailsHistoryDAO)
       {
            _iTblPersonDAO = iTblPersonDAO;
            _iTblVehicleInOutDetailsHistoryDAO = iTblVehicleInOutDetailsHistoryDAO;
            _iConnectionString = iConnectionString;
            _iTblVehicleInOutDetailsDAO = iTblVehicleInOutDetailsDAO;
            _iTblUnLoadingBL = iTblUnLoadingBL;
            _iTblUserBL = iTblUserBL;
            _iNotification = iNotification;
            _iTblAlertInstanceBL = iTblAlertInstanceBL;
            _iTblCommercialDocScheduleDetailsDAO = iTblCommercialDocScheduleDetailsDAO;
       }
        #region Selection
        public List<TblVehicleInOutDetailsTO> SelectAllTblVehicleInOutDetails()
        {
            return _iTblVehicleInOutDetailsDAO.SelectAllTblVehicleInOutDetails();
        }

        public List<TblVehicleInOutDetailsTO> SelectAllTblVehicleInOutDetailsList(Int32 moduleId,Int32 showVehicleInOut, string fromDate, string toDate, bool skipDatetime,string statusStr)
        {
            return _iTblVehicleInOutDetailsDAO.SelectAllTblVehicleInOutDetails(moduleId, showVehicleInOut, fromDate, toDate, skipDatetime, statusStr);
        }

        public TblVehicleInOutDetailsTO SelectAllTblVehicleInOutDetailsById(int moduleId, int idVehicleInOut)
        {
            return _iTblVehicleInOutDetailsDAO.SelectAllTblVehicleInOutDetailsById(moduleId, idVehicleInOut);
        }

        public TblVehicleInOutDetailsTO SelectTblVehicleInOutDetailsTO(Int32 idTblVehicleInOutDetails)
        {
           return _iTblVehicleInOutDetailsDAO.SelectTblVehicleInOutDetails(idTblVehicleInOutDetails);          
        }

      
        #endregion
        ///
        #region Insertion
        public  int InsertTblVehicleInOutDetails(TblVehicleInOutDetailsTO tblVehicleInOutDetailsTO)
        {
            return _iTblVehicleInOutDetailsDAO.InsertTblVehicleInOutDetails(tblVehicleInOutDetailsTO);
        }

        public  int InsertTblVehicleInOutDetails(TblVehicleInOutDetailsTO tblVehicleInOutDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVehicleInOutDetailsDAO.InsertTblVehicleInOutDetails(tblVehicleInOutDetailsTO, conn, tran);
        }


      public  int InsertTblCommericalDocStatusHistory(TblCommericalDocStatusHistoryTO tblCommericalDocStatusHistoryTO)
      {
            return _iTblVehicleInOutDetailsDAO.InsertTblCommericalDocStatusHistory(tblCommericalDocStatusHistoryTO);
      }
    #endregion

    #region Updation
    public int UpdateTblVehicleInOutDetails(TblVehicleInOutDetailsTO tblVehicleInOutDetailsTO)
        {
            return _iTblVehicleInOutDetailsDAO.UpdateTblVehicleInOutDetails(tblVehicleInOutDetailsTO);
        }
        public ResultMessage UpdateTblVehicleInOutDetailsStatusOnly(TblVehicleInOutDetailsTO tblVehicleInOutDetailsTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                TblVehicleInOutDetailsTO tblVehicleInOutDetailsTOold = new TblVehicleInOutDetailsTO();
                tblVehicleInOutDetailsTOold = SelectTblVehicleInOutDetailsTO(tblVehicleInOutDetailsTO.IdTblVehicleInOutDetails);                
                if (tblVehicleInOutDetailsTOold!= null)
                {
                    TblVehicleInOutDetailsHistoryTO tblVehicleInOutDetailsHistoryTO = new TblVehicleInOutDetailsHistoryTO();
                    tblVehicleInOutDetailsHistoryTO.TblVehicleInOutDetailsId = tblVehicleInOutDetailsTOold.IdTblVehicleInOutDetails;
                    tblVehicleInOutDetailsHistoryTO.VehicleNo = tblVehicleInOutDetailsTOold.VehicleNo;
                    tblVehicleInOutDetailsHistoryTO.ApplicationRouting = tblVehicleInOutDetailsTOold.ApplicationRouting;
                    tblVehicleInOutDetailsHistoryTO.SupervisorId = tblVehicleInOutDetailsTOold.SupervisorId;
                    tblVehicleInOutDetailsHistoryTO.TransporterId = tblVehicleInOutDetailsTOold.TransporterId;
                    tblVehicleInOutDetailsHistoryTO.PartyId = tblVehicleInOutDetailsTOold.PartyId;
                    tblVehicleInOutDetailsHistoryTO.TransactionStatusId = tblVehicleInOutDetailsTOold.TransactionStatusId;
                    tblVehicleInOutDetailsHistoryTO.NextStatusId = tblVehicleInOutDetailsTOold.NextStatusId;
                    tblVehicleInOutDetailsHistoryTO.NextExpectedActionId = tblVehicleInOutDetailsTOold.NextExpectedActionId;
                    tblVehicleInOutDetailsHistoryTO.IsActive = tblVehicleInOutDetailsTOold.IsActive;
                    tblVehicleInOutDetailsHistoryTO.ModuleId = tblVehicleInOutDetailsTOold.ModuleId;
                    tblVehicleInOutDetailsHistoryTO.TransactionTypeId = tblVehicleInOutDetailsTOold.TransactionTypeId;
                    tblVehicleInOutDetailsHistoryTO.CreatedBy = tblVehicleInOutDetailsTOold.CreatedBy;
                    tblVehicleInOutDetailsHistoryTO.TransactionDate = tblVehicleInOutDetailsTOold.TransactionDate;
                    tblVehicleInOutDetailsHistoryTO.CreatedOn = tblVehicleInOutDetailsTOold.CreatedOn;
                    tblVehicleInOutDetailsHistoryTO.TransactionStatusDate = tblVehicleInOutDetailsTOold.TransactionStatusDate;
                    tblVehicleInOutDetailsHistoryTO.ScheduleRefNo = tblVehicleInOutDetailsTOold.ScheduleRefNo;
                    tblVehicleInOutDetailsHistoryTO.TransactionNo = tblVehicleInOutDetailsTOold.TransactionNo;
                    tblVehicleInOutDetailsHistoryTO.TransactionDisplayNo = tblVehicleInOutDetailsTOold.TransactionDisplayNo;

                    resultMessage.Result = _iTblVehicleInOutDetailsHistoryDAO.InsertTblVehicleInOutDetailsHistory(tblVehicleInOutDetailsHistoryTO, conn, tran);
                }

                if (resultMessage.Result > 0)
                {
                    if(tblVehicleInOutDetailsTO.TransactionStatusId > 0 )
                    {
                        if(tblVehicleInOutDetailsTO.TransactionStatusId == Convert.ToInt32(Constants.TranStatusE.New_Schedule))
                        {
                            tblVehicleInOutDetailsTO.NextStatusId = Convert.ToInt32(Constants.TranStatusE.Reported);
                            tblVehicleInOutDetailsTO.NextExpectedActionId = Convert.ToInt32(Constants.TranStatusE.Reported);
                        }
                        else if (tblVehicleInOutDetailsTO.TransactionStatusId == Convert.ToInt32(Constants.TranStatusE.Reported))
                        {
                            tblVehicleInOutDetailsTO.NextStatusId = Convert.ToInt32(Constants.TranStatusE.Clearance_for_sent_in);
                            tblVehicleInOutDetailsTO.NextExpectedActionId = Convert.ToInt32(Constants.TranStatusE.Clearance_for_sent_in);
                        }
                        else if (tblVehicleInOutDetailsTO.TransactionStatusId == Convert.ToInt32(Constants.TranStatusE.Clearance_for_sent_in))
                        {
                            tblVehicleInOutDetailsTO.NextStatusId = Convert.ToInt32(Constants.TranStatusE.Sent_in);
                            tblVehicleInOutDetailsTO.NextExpectedActionId = Convert.ToInt32(Constants.TranStatusE.Sent_in);

                        }
                        else if (tblVehicleInOutDetailsTO.TransactionStatusId == Convert.ToInt32(Constants.TranStatusE.Sent_in))
                        {
                            tblVehicleInOutDetailsTO.NextStatusId = Convert.ToInt32(Constants.TranStatusE.Vehicle_Out_Common);
                            tblVehicleInOutDetailsTO.NextExpectedActionId = Convert.ToInt32(Constants.TranStatusE.Vehicle_Out_Common);
                        }
                        else if (tblVehicleInOutDetailsTO.TransactionStatusId == Convert.ToInt32(Constants.TranStatusE.Unloading_is_in_progress) || tblVehicleInOutDetailsTO.TransactionStatusId == Convert.ToInt32(Constants.TranStatusE.UNLOADING_COMPLETED))
                        {
                            tblVehicleInOutDetailsTO.NextStatusId = Convert.ToInt32(Constants.TranStatusE.Vehicle_Out_Common);
                            tblVehicleInOutDetailsTO.NextExpectedActionId = Convert.ToInt32(Constants.TranStatusE.Vehicle_Out_Common);
                        }
                        else if (tblVehicleInOutDetailsTO.TransactionStatusId == Convert.ToInt32(Constants.TranStatusE.Vehicle_Rejected_while_GRN))
                        {
                            tblVehicleInOutDetailsTO.NextStatusId = Convert.ToInt32(Constants.TranStatusE.Vehicle_Rejected_and_Out);
                            tblVehicleInOutDetailsTO.NextExpectedActionId = Convert.ToInt32(Constants.TranStatusE.Vehicle_Rejected_and_Out);
                        }

                    }
                    resultMessage.Result = _iTblVehicleInOutDetailsDAO.UpdateTblVehicleInOutDetailsStatusOnly(tblVehicleInOutDetailsTO,conn,tran);
                }

                if (resultMessage.Result > 0 && tblVehicleInOutDetailsTO.ModuleId == 10)
                {
                    resultMessage.Result = _iTblVehicleInOutDetailsDAO.UpdateTblScheduleStatusOnly(tblVehicleInOutDetailsTO, conn, tran);
                    if(resultMessage.Result >=0)
                    {
                        resultMessage.Result = 1;
                    }
                }
                if (resultMessage.Result > 0 && tblVehicleInOutDetailsTO.ModuleId == 10 && tblVehicleInOutDetailsTO.TransactionStatusId == Convert.ToInt32(Constants.TranStatusE.Sent_in))
                {
                    TblUnLoadingTO tblUnLoadingTO = new TblUnLoadingTO();
                    tblUnLoadingTO.SupplierOrgId = tblVehicleInOutDetailsTO.PartyId;
                    tblUnLoadingTO.CreatedBy = tblVehicleInOutDetailsTO.CreatedBy;
                    tblUnLoadingTO.VehicleNo = tblVehicleInOutDetailsTO.VehicleNo;
                    tblUnLoadingTO.CreatedOn = tblVehicleInOutDetailsTO.CreatedOn;
                    tblUnLoadingTO.StatusId = Convert.ToInt32(Constants.TranStatusE.UNLOADING_NEW);
                    tblUnLoadingTO.ModuleId = tblVehicleInOutDetailsTO.ModuleId;
                    tblUnLoadingTO.TranRefId =Convert.ToInt32 (tblVehicleInOutDetailsTO.IdTblVehicleInOutDetails);
                    tblUnLoadingTO.Status = "UNLOADING_NEW";

                    List<TblCommercialDocScheduleDetailsTO> detailsList = new List<TblCommercialDocScheduleDetailsTO>();
                    detailsList = _iTblCommercialDocScheduleDetailsDAO.SelectAllTblCommercialDocScheduleDetails(Convert.ToInt32(tblVehicleInOutDetailsTO.ScheduleRefNo));
                    if (detailsList != null && detailsList.Count > 0)
                    {
                        tblUnLoadingTO.UnLoadingItemDetTOList = new List<TblUnLoadingItemDetTO>();
                        for (int u = 0; u < detailsList.Count; u++)
                        {
                            TblUnLoadingItemDetTO tblUnLoDetails = new TblUnLoadingItemDetTO();
                            tblUnLoDetails.ProductId = detailsList[u].ProductItemId;
                            tblUnLoDetails.CreatedBy = tblUnLoadingTO.CreatedBy;
                            tblUnLoDetails.UnLoadingQty = detailsList[u].ScheduledQty;
                            tblUnLoDetails.CreatedOn = tblUnLoadingTO.CreatedOn;

                            tblUnLoadingTO.UnLoadingItemDetTOList.Add(tblUnLoDetails);
                        }
                    }
                  resultMessage = _iTblUnLoadingBL.SaveNewUnLoadingSlipDetails(tblUnLoadingTO);
                }
                if (resultMessage.Result <= 0)
                {
                    tran.Rollback();
                    resultMessage.Text = "Record could not be saved";
                    resultMessage.Result = -1;
                    return resultMessage;

                }
                if(tblVehicleInOutDetailsTO.ModuleId == 10 && tblVehicleInOutDetailsTO.TransactionStatusId == Convert.ToInt32(Constants.TranStatusE.Reported))
                {

                    #region  Send Notifications & SMSs 

                    TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                    List<TblAlertUsersTO> tblAlertUsersTOList = new List<TblAlertUsersTO>();

                    List<DropDownTO> storeManagerList = new List<DropDownTO>();
                    
                    Int32 roleTypeId = Convert.ToInt32(Constants.SystemRoleTypeE.STORE_INCHARGE);

                    storeManagerList = _iTblVehicleInOutDetailsDAO.SelectAllSystemUsersFromRoleType(roleTypeId);
                    if (storeManagerList != null && storeManagerList.Count > 0)
                    {
                        for (int k = 0; k < storeManagerList.Count; k++)
                        {
                            TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
                            tblAlertUsersTO.UserId = storeManagerList[k].Value;
                            tblAlertUsersTO.SnoozeDate = tblVehicleInOutDetailsTO.CreatedOn;
                            tblAlertUsersTO.RaisedOn = tblVehicleInOutDetailsTO.CreatedOn;
                            tblAlertUsersTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.VEHICLE_REPORTED;
                            tblAlertUsersTOList.Add(tblAlertUsersTO);
                        }
                    }

                    TblUserTO userTO = _iTblUserBL.SelectTblUserTO(tblVehicleInOutDetailsTO.UpdatedBy);

                    tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.VEHICLE_REPORTED;
                    tblAlertInstanceTO.AlertAction = "VEHICLE_REPORTED";
                    tblAlertInstanceTO.AlertComment = "Vehicle No. "+ tblVehicleInOutDetailsTO.VehicleNo + " Against Schedule "+ tblVehicleInOutDetailsTO.ScheduleRefNo + " has been reported successfully by " + userTO.UserDisplayName;
                    tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;
                    tblAlertInstanceTO.EffectiveFromDate = tblVehicleInOutDetailsTO.CreatedOn;
                    tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
                    tblAlertInstanceTO.IsActive = 1;
                    tblAlertInstanceTO.SourceDisplayId = "VEHICLE_REPORTED";
                    tblAlertInstanceTO.SourceEntityId = tblVehicleInOutDetailsTO.IdTblVehicleInOutDetails;
                    tblAlertInstanceTO.RaisedBy = tblVehicleInOutDetailsTO.UpdatedBy;
                    tblAlertInstanceTO.EscalationOn = tblVehicleInOutDetailsTO.CreatedOn;
                    tblAlertInstanceTO.RaisedOn = tblVehicleInOutDetailsTO.CreatedOn;
                    tblAlertInstanceTO.IsAutoReset = 1;

                    _iTblAlertInstanceBL.SaveAlertInstance(tblAlertInstanceTO);

                    //_iNotification.SendNotificationToUsers(tblAlertInstanceTO);

                    #endregion


                }

                if (tblVehicleInOutDetailsTO.ModuleId == 10 && tblVehicleInOutDetailsTO.TransactionStatusId == Convert.ToInt32(Constants.TranStatusE.Clearance_for_sent_in))
                {

                    #region Send Notifications & SMSs 

                    TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                    List<TblAlertUsersTO> tblAlertUsersTOList = new List<TblAlertUsersTO>();                    
                   
                    List<DropDownTO> unloadingmanagerList = new List<DropDownTO>();

                    Int32 roleTypeId = Convert.ToInt32(Constants.SystemRoleTypeE.UNLOADING_MANAGER);

                    unloadingmanagerList = _iTblVehicleInOutDetailsDAO.SelectAllSystemUsersFromRoleType(roleTypeId);
                    if (unloadingmanagerList != null && unloadingmanagerList.Count > 0)
                    {
                        for (int k = 0; k < unloadingmanagerList.Count; k++)
                        {
                            TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
                            tblAlertUsersTO.UserId = unloadingmanagerList[k].Value;
                            tblAlertUsersTO.RaisedOn = tblVehicleInOutDetailsTO.CreatedOn;
                            tblAlertUsersTO.SnoozeDate = tblVehicleInOutDetailsTO.CreatedOn;
                            tblAlertUsersTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.VEHICLE_REPORTED;
                            tblAlertUsersTOList.Add(tblAlertUsersTO);
                        }
                    }

                    TblAlertUsersTO tblAlertUsersT = new TblAlertUsersTO();
                    tblAlertUsersT.UserId = tblVehicleInOutDetailsTO.SupervisorId;
                    tblAlertUsersT.SnoozeDate = tblVehicleInOutDetailsTO.CreatedOn;
                    tblAlertUsersT.RaisedOn = tblVehicleInOutDetailsTO.CreatedOn;
                    tblAlertUsersT.AlertDefinitionId = (int)NotificationConstants.NotificationsE.VEHICLE_CLEARED_FOR_SENT_IN;
                    tblAlertUsersTOList.Add(tblAlertUsersT);


                    TblUserTO userTO = _iTblUserBL.SelectTblUserTO(tblVehicleInOutDetailsTO.UpdatedBy);

                    tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.VEHICLE_CLEARED_FOR_SENT_IN;
                    tblAlertInstanceTO.AlertAction = "VEHICLE_CLEARED_FOR_SENT_IN";
                    tblAlertInstanceTO.AlertComment = "Vehicle no. "+ tblVehicleInOutDetailsTO.VehicleNo + " against Schedule "+ tblVehicleInOutDetailsTO.ScheduleRefNo + " has cleared for sent in successfully by "+ userTO.UserDisplayName;
                    tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;
                    tblAlertInstanceTO.EffectiveFromDate = tblVehicleInOutDetailsTO.CreatedOn;
                    tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
                    tblAlertInstanceTO.IsActive = 1;
                    tblAlertInstanceTO.SourceDisplayId = "VEHICLE_CLEARED_FOR_SENT_IN";
                    tblAlertInstanceTO.SourceEntityId = tblVehicleInOutDetailsTO.IdTblVehicleInOutDetails;
                    tblAlertInstanceTO.RaisedBy = tblVehicleInOutDetailsTO.UpdatedBy;
                    tblAlertInstanceTO.RaisedOn = tblVehicleInOutDetailsTO.CreatedOn;
                    tblAlertInstanceTO.EscalationOn = tblVehicleInOutDetailsTO.CreatedOn;
                    tblAlertInstanceTO.IsAutoReset = 1;


                    _iTblAlertInstanceBL.SaveAlertInstance(tblAlertInstanceTO);
                    //_iNotification.SendNotificationToUsers(tblAlertInstanceTO);

                    #endregion
                }

                if (tblVehicleInOutDetailsTO.ModuleId == 10 && tblVehicleInOutDetailsTO.TransactionStatusId == Convert.ToInt32(Constants.TranStatusE.Sent_in))
                {

                    #region Send Notifications & SMSs 

                    TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                    List<TblAlertUsersTO> tblAlertUsersTOList = new List<TblAlertUsersTO>();

                    List<DropDownTO> securityOfficerList = new List<DropDownTO>();
                    List<DropDownTO> weighingOfficerList = new List<DropDownTO>();
                    List<DropDownTO> unloadingmanagerList = new List<DropDownTO>();

                    Int32 roleTypeId = Convert.ToInt32(Constants.SystemRoleTypeE.SECURITY_OFFICER);

                    securityOfficerList = _iTblVehicleInOutDetailsDAO.SelectAllSystemUsersFromRoleType(roleTypeId);

                    Int32 roleTypeIdWeighing = Convert.ToInt32(Constants.SystemRoleTypeE.WEIGHING_OFFICER);

                    weighingOfficerList = _iTblVehicleInOutDetailsDAO.SelectAllSystemUsersFromRoleType(roleTypeIdWeighing);

                    Int32 roleTypeIdMan = Convert.ToInt32(Constants.SystemRoleTypeE.UNLOADING_MANAGER);

                    unloadingmanagerList = _iTblVehicleInOutDetailsDAO.SelectAllSystemUsersFromRoleType(roleTypeIdMan);

                    if (unloadingmanagerList != null && unloadingmanagerList.Count > 0)
                    {
                        for (int k = 0; k < unloadingmanagerList.Count; k++)
                        {
                            TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
                            tblAlertUsersTO.UserId = unloadingmanagerList[k].Value;
                            tblAlertUsersTO.RaisedOn = tblVehicleInOutDetailsTO.CreatedOn;
                            tblAlertUsersTO.SnoozeDate = tblVehicleInOutDetailsTO.CreatedOn;
                            tblAlertUsersTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.VEHICLE_REPORTED;
                            tblAlertUsersTOList.Add(tblAlertUsersTO);
                        }
                    }


                    if (securityOfficerList != null && securityOfficerList.Count > 0)
                    {
                        for (int k = 0; k < securityOfficerList.Count; k++)
                        {
                            TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
                            tblAlertUsersTO.UserId = securityOfficerList[k].Value;
                            tblAlertUsersTO.SnoozeDate = tblVehicleInOutDetailsTO.CreatedOn;
                            tblAlertUsersTO.RaisedOn = tblVehicleInOutDetailsTO.CreatedOn;
                            tblAlertUsersTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.VEHICLE_IS_SENT_IN;
                            tblAlertUsersTOList.Add(tblAlertUsersTO);
                        }
                    }


                    if (weighingOfficerList != null && weighingOfficerList.Count > 0)
                    {
                        for (int k = 0; k < weighingOfficerList.Count; k++)
                        {
                            TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
                            tblAlertUsersTO.UserId = weighingOfficerList[k].Value;
                            tblAlertUsersTO.SnoozeDate = tblVehicleInOutDetailsTO.CreatedOn;
                            tblAlertUsersTO.RaisedOn = tblVehicleInOutDetailsTO.CreatedOn;
                            tblAlertUsersTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.VEHICLE_IS_SENT_IN;
                            tblAlertUsersTOList.Add(tblAlertUsersTO);
                        }
                    }
                    TblUserTO userTO = _iTblUserBL.SelectTblUserTO(tblVehicleInOutDetailsTO.UpdatedBy);

                    tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.VEHICLE_IS_SENT_IN;
                    tblAlertInstanceTO.AlertAction = "VEHICLE_IS_SENT_IN";
                    tblAlertInstanceTO.AlertComment = "Vehicle no. " + tblVehicleInOutDetailsTO.VehicleNo + " against Schedule " + tblVehicleInOutDetailsTO.ScheduleRefNo + " has sent in successfully by "+ userTO.UserDisplayName;
                    tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;
                    tblAlertInstanceTO.EffectiveFromDate = tblVehicleInOutDetailsTO.CreatedOn;
                    tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
                    tblAlertInstanceTO.IsActive = 1;
                    tblAlertInstanceTO.SourceDisplayId = "VEHICLE_IS_SENT_IN";
                    tblAlertInstanceTO.SourceEntityId = tblVehicleInOutDetailsTO.IdTblVehicleInOutDetails;
                    tblAlertInstanceTO.RaisedBy = tblVehicleInOutDetailsTO.UpdatedBy;
                    tblAlertInstanceTO.EscalationOn = tblVehicleInOutDetailsTO.CreatedOn;
                    tblAlertInstanceTO.RaisedOn = tblVehicleInOutDetailsTO.CreatedOn;
                    tblAlertInstanceTO.IsAutoReset = 1;

                    _iTblAlertInstanceBL.SaveAlertInstance(tblAlertInstanceTO);
                    //_iNotification.SendNotificationToUsers(tblAlertInstanceTO);

                    #endregion
                }

                if (tblVehicleInOutDetailsTO.ModuleId == 10 && tblVehicleInOutDetailsTO.TransactionStatusId == Convert.ToInt32(Constants.TranStatusE.Unloading_Completed))
                {

                    #region Send Notifications & SMSs 

                    TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                    List<TblAlertUsersTO> tblAlertUsersTOList = new List<TblAlertUsersTO>();

                    TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
                    tblAlertUsersTO.UserId = tblVehicleInOutDetailsTO.SupervisorId;
                    tblAlertUsersTO.SnoozeDate = tblVehicleInOutDetailsTO.CreatedOn;
                    tblAlertUsersTO.RaisedOn = tblVehicleInOutDetailsTO.CreatedOn;
                    tblAlertUsersTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.UNLOADING_WEIGHING_COMPLETED;
                    tblAlertUsersTOList.Add(tblAlertUsersTO);

                    TblUserTO userTO = _iTblUserBL.SelectTblUserTO(tblVehicleInOutDetailsTO.UpdatedBy);


                    tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.UNLOADING_WEIGHING_COMPLETED;
                    tblAlertInstanceTO.AlertAction = "UNLOADING_WEIGHING_COMPLETED";
                    tblAlertInstanceTO.AlertComment = "Weighing against vehicle no. " + tblVehicleInOutDetailsTO.VehicleNo + " has completed by "+userTO.UserDisplayName;
                    tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;
                    tblAlertInstanceTO.EffectiveFromDate = tblVehicleInOutDetailsTO.CreatedOn;
                    tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
                    tblAlertInstanceTO.IsActive = 1;
                    tblAlertInstanceTO.SourceDisplayId = "UNLOADING_WEIGHING_COMPLETED";
                    tblAlertInstanceTO.SourceEntityId = tblVehicleInOutDetailsTO.IdTblVehicleInOutDetails;
                    tblAlertInstanceTO.RaisedBy = tblVehicleInOutDetailsTO.UpdatedBy;
                    tblAlertInstanceTO.EscalationOn = tblVehicleInOutDetailsTO.CreatedOn;
                    tblAlertInstanceTO.RaisedOn = tblVehicleInOutDetailsTO.CreatedOn;
                    tblAlertInstanceTO.IsAutoReset = 1;

                    //_iNotification.SendNotificationToUsers(tblAlertInstanceTO);
                    _iTblAlertInstanceBL.SaveAlertInstance(tblAlertInstanceTO);


                    #endregion
                }


                if (tblVehicleInOutDetailsTO.ModuleId == 10 && tblVehicleInOutDetailsTO.TransactionStatusId == Convert.ToInt32(Constants.TranStatusE.Vehicle_Out_Common))
                {
                    #region Send Notifications & SMSs 

                    TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                    List<TblAlertUsersTO> tblAlertUsersTOList = new List<TblAlertUsersTO>();

                    List<DropDownTO> storeInchargeList = new List<DropDownTO>();                    
                    storeInchargeList = _iTblVehicleInOutDetailsDAO.SelectAllSystemUsersFromRoleType(Convert.ToInt32(Constants.SystemRoleTypeE.STORE_INCHARGE));
                    if (storeInchargeList != null && storeInchargeList.Count > 0)
                    {
                        for (int k = 0; k < storeInchargeList.Count; k++)
                        {
                            TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
                            tblAlertUsersTO.UserId = storeInchargeList[k].Value;
                            tblAlertUsersTO.RaisedOn = tblVehicleInOutDetailsTO.CreatedOn;
                            tblAlertUsersTO.SnoozeDate = tblVehicleInOutDetailsTO.CreatedOn;
                            tblAlertUsersTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.VEHICLE_OUT;
                            tblAlertUsersTOList.Add(tblAlertUsersTO);
                        }
                    }

                    List<DropDownTO> concernPerList = new List<DropDownTO>();
                    concernPerList = _iTblPersonDAO.GetUserIdFromOrgIdDetails(tblVehicleInOutDetailsTO.PartyId);
                    if (storeInchargeList != null && concernPerList.Count > 0)
                    {
                        for (int k = 0; k < concernPerList.Count; k++)
                        {
                            TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
                            tblAlertUsersTO.UserId = concernPerList[k].Value;
                            tblAlertUsersTO.RaisedOn = tblVehicleInOutDetailsTO.CreatedOn;
                            tblAlertUsersTO.SnoozeDate = tblVehicleInOutDetailsTO.CreatedOn;
                            tblAlertUsersTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.VEHICLE_OUT;
                            tblAlertUsersTOList.Add(tblAlertUsersTO);
                        }
                    }



                    TblUserTO userTO = _iTblUserBL.SelectTblUserTO(tblVehicleInOutDetailsTO.UpdatedBy);

                    tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.VEHICLE_OUT;
                    tblAlertInstanceTO.AlertAction = "VEHICLE_OUT";
                    tblAlertInstanceTO.AlertComment = "Vehicle no. " + tblVehicleInOutDetailsTO.VehicleNo + " against Schedule " + tblVehicleInOutDetailsTO.ScheduleRefNo + " has out successfully by " + userTO.UserDisplayName;
                    tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;
                    tblAlertInstanceTO.EffectiveFromDate = tblVehicleInOutDetailsTO.CreatedOn;
                    tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
                    tblAlertInstanceTO.IsActive = 1;
                    tblAlertInstanceTO.SourceDisplayId = "VEHICLE_OUT";
                    tblAlertInstanceTO.SourceEntityId = tblVehicleInOutDetailsTO.IdTblVehicleInOutDetails;
                    tblAlertInstanceTO.RaisedBy = tblVehicleInOutDetailsTO.UpdatedBy;
                    tblAlertInstanceTO.RaisedOn = tblVehicleInOutDetailsTO.CreatedOn;
                    tblAlertInstanceTO.EscalationOn = tblVehicleInOutDetailsTO.CreatedOn;
                    tblAlertInstanceTO.IsAutoReset = 1;


                    _iTblAlertInstanceBL.SaveAlertInstance(tblAlertInstanceTO);
                    //_iNotification.SendNotificationToUsers(tblAlertInstanceTO);

                    #endregion
                }

                if (tblVehicleInOutDetailsTO.ModuleId == 10 && tblVehicleInOutDetailsTO.IsTareWeightTaken > 0)
                {

                    #region Send Notifications & SMSs 

                    TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                    List<TblAlertUsersTO> tblAlertUsersTOList = new List<TblAlertUsersTO>();

                    TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
                    tblAlertUsersTO.UserId = tblVehicleInOutDetailsTO.SupervisorId;
                    tblAlertUsersTO.SnoozeDate = tblVehicleInOutDetailsTO.CreatedOn;
                    tblAlertUsersTO.RaisedOn = tblVehicleInOutDetailsTO.CreatedOn;
                    tblAlertUsersTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.UNLOADING_WEIGHING_COMPLETED;
                    tblAlertUsersTOList.Add(tblAlertUsersTO);

                    TblUserTO userTO = _iTblUserBL.SelectTblUserTO(tblVehicleInOutDetailsTO.UpdatedBy);


                    tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.UNLOADING_WEIGHING_COMPLETED;
                    tblAlertInstanceTO.AlertAction = "TARE_WEIGHT_TAKEN";
                    tblAlertInstanceTO.AlertComment = "Tare Weight taken against vehicle no. " + tblVehicleInOutDetailsTO.VehicleNo + " by " + userTO.UserDisplayName;
                    tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;
                    tblAlertInstanceTO.EffectiveFromDate = tblVehicleInOutDetailsTO.CreatedOn;
                    tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
                    tblAlertInstanceTO.IsActive = 1;
                    tblAlertInstanceTO.SourceDisplayId = "TARE_WEIGHT_TAKEN";
                    tblAlertInstanceTO.SourceEntityId = tblVehicleInOutDetailsTO.IdTblVehicleInOutDetails;
                    tblAlertInstanceTO.RaisedBy = tblVehicleInOutDetailsTO.UpdatedBy;
                    tblAlertInstanceTO.EscalationOn = tblVehicleInOutDetailsTO.CreatedOn;
                    tblAlertInstanceTO.RaisedOn = tblVehicleInOutDetailsTO.CreatedOn;
                    tblAlertInstanceTO.IsAutoReset = 1;

                    //_iNotification.SendNotificationToUsers(tblAlertInstanceTO);
                    _iTblAlertInstanceBL.SaveAlertInstance(tblAlertInstanceTO);


                    #endregion
                }



                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, " UpdateTblVehicleInOutDetailsStatusOnly");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        public  int UpdateTblVehicleInOutDetails(TblVehicleInOutDetailsTO tblVehicleInOutDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVehicleInOutDetailsDAO.UpdateTblVehicleInOutDetails(tblVehicleInOutDetailsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblVehicleInOutDetails(Int32 idTblVehicleInOutDetails)
        {
            return _iTblVehicleInOutDetailsDAO.DeleteTblVehicleInOutDetails(idTblVehicleInOutDetails);
        }

        public  int DeleteTblVehicleInOutDetails(Int32 idTblVehicleInOutDetails, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVehicleInOutDetailsDAO.DeleteTblVehicleInOutDetails(idTblVehicleInOutDetails, conn, tran);
        }


        public List<VehicleNumber> SelectAllVehicles()
        {
            return _iTblVehicleInOutDetailsDAO.SelectAllVehicles();
        }
        public List<DropDownTO> GetPONoListAgainstSupplier(Int64 supplierId)
        {
            return _iTblVehicleInOutDetailsDAO.GetPONoListAgainstSupplier(supplierId);
        }
        public List<DropDownTO> SelectAllSystemUsersFromRoleType(Int32 roleTypeId)
        {
            return _iTblVehicleInOutDetailsDAO.SelectAllSystemUsersFromRoleType(roleTypeId);
        }


        public List<DropDownTO> SelectAllSystemUsersFromRoleTypeWithVehAllocation(Int32 roleTypeId, Int32 nameWithCount)
        {

            List<DropDownTO> userListWithVehicleCount = new List<DropDownTO>();
            //DateTime serverDate = serverDate;

            List<DropDownTO> roleUserList = _iTblVehicleInOutDetailsDAO.SelectAllSystemUsersFromRoleType(roleTypeId);

            //if (roleUserList != null && roleUserList.Count > 0)
            //{
            //    Boolean getAllAlocatedVehicles = false;
            //    Dictionary<Int32, Int32> DCT = _iTblPurchaseScheduleSummaryDAO.GetAllocatedVehiclesAgainstRole(roleTypeId, getAllAlocatedVehicles);

            //    getAllAlocatedVehicles = true;
            //    //Dictionary<Int32, Int32> allAllocatedVehDCT = _iTblPurchaseScheduleSummaryDAO.GetAllocatedVehiclesAgainstRole(roleTypeId, getAllAlocatedVehicles);

            //    Dictionary<Int32, Int32> allAllocatedVehDCT = _iTblPurchaseScheduleSummaryDAO.GetTodaysAllocatedVehiclesCnt(roleTypeId, serverDate);



            //    for (int i = 0; i < roleUserList.Count; i++)
            //    {
            //        DropDownTO dropDownTO = roleUserList[i];
            //        dropDownTO.Tag = 0;

            //        String countToAppend = string.Empty;
            //        String totalCountToAppend = string.Empty;

            //        Int32 cnt = 0;
            //        Int32 totalCnt = 0;

            //        if (DCT != null && DCT.ContainsKey(roleUserList[i].Value))
            //        {
            //            dropDownTO.Tag = DCT[roleUserList[i].Value];

            //            countToAppend = " (" + DCT[roleUserList[i].Value] + ")";

            //            cnt = DCT[roleUserList[i].Value];
            //            //totalCountToAppend = countToAppend;
            //        }

            //        if (allAllocatedVehDCT != null && allAllocatedVehDCT.ContainsKey(roleUserList[i].Value))
            //        {
            //            Int32 tempVal = allAllocatedVehDCT[roleUserList[i].Value];
            //            totalCnt = tempVal;
            //            totalCountToAppend = tempVal.ToString();
            //        }

            //        if (String.IsNullOrEmpty(totalCountToAppend))
            //        {
            //            totalCountToAppend = "0";
            //        }

            //        if (String.IsNullOrEmpty(countToAppend))
            //        {
            //            countToAppend = " 0";
            //        }

            //        dropDownTO.Text = (roleUserList[i].Text);
            //        Int32 totalVehCnt = cnt + totalCnt;
            //        if (nameWithCount == 1)
            //        {
            //            dropDownTO.Text += countToAppend + "/" + totalVehCnt;
            //        }
            //        else
            //        {
            //            dropDownTO.Tag += "/" + totalVehCnt;
            //        }
            //        userListWithVehicleCount.Add(dropDownTO);
            //    }

            //}

            //if (userListWithVehicleCount != null && userListWithVehicleCount.Count > 0)
            //{
            //    userListWithVehicleCount = userListWithVehicleCount.OrderBy(o => o.Tag).ThenBy(t => t.Text).ToList();
            //}

            return roleUserList;

        }
        
        public List<DropDownTO> SelectAllSystemUsersforUnloadingSuperwisorVehicleIn()
        {

            List<DropDownTO> userListWithVehicleCount = new List<DropDownTO>();
           

            List<DropDownTO> roleUserList = _iTblVehicleInOutDetailsDAO.SelectAllSystemUsersforUnloadingSuperwisorVehicleIn();

          
            return roleUserList;

        }



        #endregion

    }
}
