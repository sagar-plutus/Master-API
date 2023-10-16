using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using ODLMWebAPI.Models;
using System.Threading.Tasks;
using System;
using ODLMWebAPI.BL;
using System.Linq;
using ODLMWebAPI.BL.Interfaces;
using System.Data;
using ODLMWebAPI.StaticStuff;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using simpliMASTERSAPI.TO;
using ODLMWebAPI.DAL.Interfaces;
using simpliMASTERSAPI.BL.Interfaces;

namespace ODLMWebAPI.Controllers
{
    
    [Route("api/[controller]")]
    public class DynamicApprovalController : Controller
    {
        private readonly IDynamicApprovalCYcleBL iDynamicApprovalCYcleBL;
        private readonly ITblPersonBL _iTblPersonBL;
        private readonly INotification notify;
        private readonly ITblVehicleInOutDetailsBL _iTblVehicleInOutDetailsBL;
        private readonly ICommon _iCommonDAO;
        private readonly ITblAlertInstanceBL _iTblAlertInstanceBL;
        private readonly ITblUserBL _iTblUserBL;


        public DynamicApprovalController(ITblUserBL iTblUserBL, IDynamicApprovalCYcleBL iDynamicApprovalCYcleBL, ITblVehicleInOutDetailsBL iTblVehicleInOutDetailsBL, ITblPersonBL iTblPersonBL, ICommon icommondao, ITblAlertInstanceBL iTblAlertInstanceBL,
            INotification inotify)
        {
            this.iDynamicApprovalCYcleBL=iDynamicApprovalCYcleBL;
            _iTblPersonBL = iTblPersonBL;
            notify = inotify;
            _iCommonDAO = icommondao;
            _iTblVehicleInOutDetailsBL = iTblVehicleInOutDetailsBL;
            _iTblAlertInstanceBL = iTblAlertInstanceBL;
            _iTblUserBL = iTblUserBL;


        }
        [Route("GetAllApprovalList")]
        [HttpGet]
        public List<DimDynamicApprovalTO> GetAllApprovalList(int idModule=0,int area=0,int userId=0)
        {
           
         return iDynamicApprovalCYcleBL.SelectAllApprovalList(idModule , area, userId);
        
        }


        //[Route("GetListBySequenceNo")]
        //[HttpGet]
        //public DataTable GetListBySequenceNo(int seqNo)
        //{
        //    return iDynamicApprovalCYcleBL.SelectAllList(seqNo);
        //}

        [Route("GetListBySequenceNo")]
        [HttpGet]
        public DataTable GetListBySequenceNo(int seqNo, int userId)
        {
            //AmolG[2020-Dec-22] Added for UserHirachy wise request display
            return iDynamicApprovalCYcleBL.SelectAllList(seqNo, userId);
        }

        [Route("GetDetailsById")]
        [HttpGet]
        public DataTable GetDetailsById(int idDetails,int idApprovalActions)
        {
            return iDynamicApprovalCYcleBL.GetDetailsById(idDetails, idApprovalActions);
        }


        [Route("GetActionIconList")]
        [HttpGet]
        public List<DimApprovalActionsTO> GetActionIconList(int idApproval)
        {
            return iDynamicApprovalCYcleBL.GetActionIconList(idApproval);
        }

        [Route("UpdateStatus")]
        [HttpPost]
        public ResultMessage UpdateStatus([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                var tableData = JsonConvert.DeserializeObject<Dictionary<string, string>>(data["data"].ToString());
                var Status = data["status"].ToString();
                var seqNo = data["seqNo"].ToString();
                var userId = data["userId"].ToString();
                Int32 isApprovByDir = -1;
                if (data["isApprovByDir"] != null)
                {
                    isApprovByDir = Convert.ToInt32(data["isApprovByDir"].ToString());
                }
                string txtCommentMsg = "";
                int OrganizationId = 0;
                int withinCriteria = 0;
                string TransactionNo ="";

                int result = iDynamicApprovalCYcleBL.UpdateStatus(tableData, Convert.ToInt32(Status), Convert.ToInt32(userId), Convert.ToInt32(seqNo),ref txtCommentMsg, ref OrganizationId, isApprovByDir, ref TransactionNo,ref withinCriteria);
                // Add By samadhan 7 feb 2023 Update Ismigration flag Zero when PO status changes
                int mresult = iDynamicApprovalCYcleBL.UpdateIsMigrationFlag(Convert.ToInt64(tableData["Id"]));
                // Add By samadhan 5 May 2023 Update Ismigration flag Zero when PO status changes
                int iresult = iDynamicApprovalCYcleBL.UpdateIsOnePageSummaryMigrationFlag(Convert.ToInt64(tableData["Id"]));
                // Add By samadhan 24 March 2023

                result = iDynamicApprovalCYcleBL.InsertTblPurchaseRequestStatusHistory(tableData,Convert.ToInt32(Status), Convert.ToInt32(userId));
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error While Inserting  Purchase Request Status History Record - InsertTblPurchaseRequestStatusHistory");
                    return resultMessage;
                }

                result = iDynamicApprovalCYcleBL.InsertCommercialDocPaymentStatusHistory(tableData, Convert.ToInt32(Status), Convert.ToInt32(userId));
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error While Inserting PO Advance History Record - InsertCommercialDocPaymentStatusHistory");
                    return resultMessage;
                }


                if (OrganizationId > 0)
                {
                    var LatestStatus = iDynamicApprovalCYcleBL.GetLatestStatus(Convert.ToInt64(tableData["Id"]));

                    //if (Convert.ToInt32(Status) == Convert.ToInt32(Constants.TranStatusE.PURCHASE_ORDER_STATUS_AUTHORIZATION_1) || Convert.ToInt32(Status) == Convert.ToInt32(Constants.TranStatusE.PURCHASE_ORDER_STATUS_AUTHORIZATION_1) || Convert.ToInt32(Status) == Convert.ToInt32(Constants.TranStatusE.PURCHASE_ORDER_STATUS_AUTHORIZATION_2) || Convert.ToInt32(Status) == Convert.ToInt32(Constants.TranStatusE.PURCHASE_ORDER_STATUS_AUTHORIZATION_3)) {
                    #region 1. Update Document History Status
                    TblCommericalDocStatusHistoryTO tblCommericalDocStatusHistoryTO = new TblCommericalDocStatusHistoryTO();
                        tblCommericalDocStatusHistoryTO.CommercialDocumentId = Convert.ToInt32(tableData["Id"]);
                        tblCommericalDocStatusHistoryTO.CreatedBy = Convert.ToInt32(userId);
                        tblCommericalDocStatusHistoryTO.CreatedOn = _iCommonDAO.ServerDateTime;
                        tblCommericalDocStatusHistoryTO.StatusDate = _iCommonDAO.ServerDateTime;
                    // tblCommericalDocStatusHistoryTO.StatusId = Convert.ToInt32(Constants.TranStatusE.PURCHASE_ORDER_STATUS_AUTHORIZED);
                        tblCommericalDocStatusHistoryTO.StatusId=Convert.ToInt32(LatestStatus); // Add By Samadhan 10 Apr 2023
                        tblCommericalDocStatusHistoryTO.IsComment = 0;

                        result = _iTblVehicleInOutDetailsBL.InsertTblCommericalDocStatusHistory(tblCommericalDocStatusHistoryTO);
                        #endregion
                    //}

                    #region 2. Send Notifications & SMSs to Purchase Managers Defined or undefined

                    TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                    List<TblAlertUsersTO> tblAlertUsersTOList = new List<TblAlertUsersTO>();
                    List<TblAlertUsersTO> tblAlertUsersTOListfllup = new List<TblAlertUsersTO>();

                    //get purchase manager of supplier
                    List<DropDownTO> UserList = new List<DropDownTO>();
                    UserList = _iTblPersonBL.GetUserIdFromOrgIdDetails(OrganizationId);                              
                                       
                   
                    if (UserList != null && UserList.Count > 0)
                    {
                        for (int k = 0; k < UserList.Count; k++)
                        {
                            TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
                            tblAlertUsersTO.UserId = UserList[k].Value;
                            tblAlertUsersTO.RaisedOn = _iCommonDAO.ServerDateTime;
                            tblAlertUsersTO.SnoozeDate = _iCommonDAO.ServerDateTime;
                            tblAlertUsersTO.AlertDefinitionId = 1511;
                            tblAlertUsersTOList.Add(tblAlertUsersTO);

                            TblAlertUsersTO tblAlertUsersTOfp = new TblAlertUsersTO();
                            tblAlertUsersTOfp.UserId = UserList[k].Value;
                            tblAlertUsersTOfp.RaisedOn = _iCommonDAO.ServerDateTime;
                            tblAlertUsersTOfp.SnoozeDate = _iCommonDAO.ServerDateTime;
                            tblAlertUsersTOfp.AlertDefinitionId = 1519;
                            tblAlertUsersTOListfllup.Add(tblAlertUsersTOfp);
                        }
                    }
                    List<DropDownTO> tbldirectorsList = _iTblVehicleInOutDetailsBL.SelectAllSystemUsersFromRoleType(Convert.ToInt32(Constants.SystemRoleTypeE.PURCHASE_MANAGER));
                    List<DropDownTO> tblStoreHODList = _iTblVehicleInOutDetailsBL.SelectAllSystemUsersFromRoleType(Convert.ToInt32(Constants.SystemRoleTypeE.Store_HOD));


                    if (tbldirectorsList != null && tbldirectorsList.Count > 0)
                    {
                        for (int i = 0; i < tbldirectorsList.Count; i++)
                        {
                            TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
                            tblAlertUsersTO.UserId = tbldirectorsList[i].Value;
                            tblAlertUsersTO.RaisedOn = _iCommonDAO.ServerDateTime;
                            tblAlertUsersTO.SnoozeDate = _iCommonDAO.ServerDateTime;
                            tblAlertUsersTO.AlertDefinitionId = 1511;
                            tblAlertUsersTOList.Add(tblAlertUsersTO);
                        }
                    }
                    if (tblStoreHODList != null && tblStoreHODList.Count > 0)
                    {
                        for (int i = 0; i < tblStoreHODList.Count; i++)
                        {
                            TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
                            tblAlertUsersTO.UserId = tblStoreHODList[i].Value;
                            tblAlertUsersTO.RaisedOn = _iCommonDAO.ServerDateTime;
                            tblAlertUsersTO.SnoozeDate = _iCommonDAO.ServerDateTime;
                            tblAlertUsersTO.AlertDefinitionId = 1511;
                            tblAlertUsersTOList.Add(tblAlertUsersTO);
                        }
                    }

                    tblAlertInstanceTO.AlertDefinitionId = 1511;
                    tblAlertInstanceTO.AlertAction = "Authorization";
                    txtCommentMsg = txtCommentMsg.Replace("@DISPLAYNO", TransactionNo);
                    tblAlertInstanceTO.AlertComment = txtCommentMsg;
                    TblUserTO userTO = _iTblUserBL.SelectTblUserTO(Convert.ToInt32(userId));
                    if(userTO != null)
                    {
                        tblAlertInstanceTO.AlertComment = txtCommentMsg+" "+ userTO.UserDisplayName;
                    }
                    tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;
                    tblAlertInstanceTO.EffectiveFromDate = _iCommonDAO.ServerDateTime;
                    tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
                    tblAlertInstanceTO.IsActive = 1;
                    tblAlertInstanceTO.SourceDisplayId = "Authorization";
                    tblAlertInstanceTO.SourceEntityId =Convert.ToInt32(tableData["Id"]);
                    tblAlertInstanceTO.RaisedBy = Convert.ToInt32(userId);
                    tblAlertInstanceTO.RaisedOn = _iCommonDAO.ServerDateTime;
                    tblAlertInstanceTO.EscalationOn = _iCommonDAO.ServerDateTime;
                    tblAlertInstanceTO.IsAutoReset = 1;

                    //notify.SendNotificationToUsers(tblAlertInstanceTO);
                    resultMessage= _iTblAlertInstanceBL.SaveAlertInstance(tblAlertInstanceTO);


                    tblAlertInstanceTO.AlertDefinitionId = 1519;
                    tblAlertInstanceTO.AlertAction = "FollowUp Added";
                    tblAlertInstanceTO.SourceDisplayId = "FollowUp";
                    
                    if (userTO != null)
                    {
                        tblAlertInstanceTO.AlertComment = "Follow Up added against Purchase Order #" + TransactionNo + " by " + userTO.UserDisplayName;
                    }
                    tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOListfllup;
                    if (tblAlertInstanceTO.AlertUsersTOList.Count > 0)
                        resultMessage = _iTblAlertInstanceBL.SaveAlertInstance(tblAlertInstanceTO);
                    

                    #endregion

                }
                if (result == 1)
                {
                    resultMessage.Result = 1;
                    if (Convert.ToInt32(Status) > 1 && Convert.ToInt32(Status) != 6 && Convert.ToInt32(Status) != 8)
                    {
                        resultMessage.DisplayMessage = "Send For Director Approval Successfully - Id = " + tableData["Id"];
                        return resultMessage;
                    }else if (Status == "1")
                    {
                        if (withinCriteria == 1)
                        {
                            resultMessage.DisplayMessage = "Sent For Next Level Authorization - Id =" + tableData["Id"];
                        }
                        else
                        {
                            resultMessage.DisplayMessage = "Status Approved Successfully for Id=" + tableData["Id"];
                        }
                        return resultMessage;
                    }
                    else
                    {
                        resultMessage.DisplayMessage = "Status Reject Successfully for Id=" + tableData["Id"];
                        return resultMessage;
                    }


                }
                else
                {
                    resultMessage.DisplayMessage = "Error... Record could not be saved";
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                resultMessage.Text = "Exception Error IN API Call UpdateStatus :" + ex;
                resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                return resultMessage;
            }
            return resultMessage;


        }


        [Route("updatePurchaseRequestComments")]
        [HttpPost]
        public ResultMessage updatePurchaseRequestComments([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                DropDownTO purchaseTO = JsonConvert.DeserializeObject<DropDownTO>(data["purchaseReqTO"].ToString());
                var userId = data["userId"].ToString();
               
                int result = iDynamicApprovalCYcleBL.updatePurchaseRequestComments(purchaseTO,Convert.ToInt32(userId));
                if(result >0)
                {
                    resultMessage.DefaultSuccessBehaviour();
                    return resultMessage;
                }

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                resultMessage.Text = "Exception Error IN API Call updatePurchaseRequestComments :" + ex;
                resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                return resultMessage;
            }
            return resultMessage;


        }


        [Route("updateCommercialDocComments")]
        [HttpPost]
        public ResultMessage updateCommercialDocComments([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                DropDownTO CommDocTO = JsonConvert.DeserializeObject<DropDownTO>(data["CommDocTO"].ToString());
                var userId = data["userId"].ToString();

                int result = iDynamicApprovalCYcleBL.updateCommercialDocComments(CommDocTO, Convert.ToInt32(userId));
                if (result > 0)
                {
                    resultMessage.DefaultSuccessBehaviour();
                    return resultMessage;
                }

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                resultMessage.Text = "Exception Error IN API Call updatePurchaseRequestComments :" + ex;
                resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                return resultMessage;
            }
            return resultMessage;


        }


    }
}

