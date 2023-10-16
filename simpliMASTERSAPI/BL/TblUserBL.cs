using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;
using AutoMapper;
using simpliMASTERSAPI.MessageQueuePayloads;
using rabbitMessaging;
using simpliMASTERSAPI.DAL.Interfaces;
using System.Linq;
using Newtonsoft.Json;
using simpliMASTERSAPI.Models;

namespace ODLMWebAPI.BL
{
    public class TblUserBL : ITblUserBL
    {
        private readonly ITblConfigParamsDAO _iTblConfigParamsDAO;
        private readonly IMessagePublisher _iMessagePublisher;
       
        private readonly IDimMstDeptDAO _iDimMstDeptDAO;
        private readonly IDimUserTypeDAO _iDimUserTypeDAO;
        private readonly IDimensionDAO _iDimensionDAO;
        private readonly ITblUserDAO _iTblUserDAO;
        private readonly ITblPersonDAO _iTblPersonDAO;
        private readonly ITblAddressDAO _iTblAddressDAO;
        private readonly ITblUserExtDAO _iTblUserExtDAO;
        private readonly ITblOrgStructureBL _iTblOrgStructureBL;
        private readonly ITblUserRoleBL _iTblUserRoleBL;
        private readonly ITblUserPwdHistoryDAO _iTblUserPwdHistoryDAO;
        private readonly IConnectionString _iConnectionString;
        private readonly ITblLoginDAO _iTblLoginDAO;
            private readonly ITblModuleBL _iTblModuleBL;
        private readonly ICommon _iCommon;
        private readonly ITblConfigParamsBL _iTblConfigParamsBL;
        private readonly ITblRoleBL _iTblRoleBL;
        private readonly IRabbitMessagingHistoryDAO _iRabbitMessagingHistoryDAO;
        private static readonly object proFlowUserLock = new object();
        public TblUserBL(ITblModuleBL _iTblModuleBL, ITblConfigParamsDAO iTblConfigParamsDAO, IRabbitMessagingHistoryDAO iRabbitMessagingHistoryDAO, IMessagePublisher iMessagePublisher, IDimMstDeptDAO iDimMstDeptDAO, IDimUserTypeDAO iDimUserTypeDAO, ITblLoginDAO iTblLoginDAO, IDimensionDAO iDimensionDAO, ITblRoleBL iTblRoleBL, ITblUserRoleBL iTblUserRoleBL, ITblOrgStructureBL iTblOrgStructureBL, ICommon iCommon, IConnectionString iConnectionString, ITblUserPwdHistoryDAO iTblUserPwdHistoryDAO, ITblUserExtDAO iTblUserExtDAO, ITblAddressDAO iTblAddressDAO, ITblPersonDAO iTblPersonDAO, ITblUserDAO iTblUserDAO, ITblConfigParamsBL iTblConfigParamsBL)
        {
            _iTblConfigParamsDAO = iTblConfigParamsDAO;
            _iRabbitMessagingHistoryDAO = iRabbitMessagingHistoryDAO;
            _iMessagePublisher = iMessagePublisher;        
            _iDimMstDeptDAO = iDimMstDeptDAO;
            _iDimUserTypeDAO = iDimUserTypeDAO;
            _iDimensionDAO = iDimensionDAO;
            _iTblRoleBL = iTblRoleBL;
            _iTblUserDAO = iTblUserDAO;
            _iTblPersonDAO = iTblPersonDAO;
            _iTblAddressDAO = iTblAddressDAO;
            _iTblUserExtDAO = iTblUserExtDAO;
            _iTblOrgStructureBL = iTblOrgStructureBL;
            _iTblUserRoleBL = iTblUserRoleBL;
            _iTblUserPwdHistoryDAO = iTblUserPwdHistoryDAO;
            _iConnectionString = iConnectionString;
            _iTblLoginDAO = iTblLoginDAO;
            _iCommon = iCommon;
            this._iTblModuleBL=_iTblModuleBL;
            _iTblConfigParamsBL = iTblConfigParamsBL;
        }
        #region Selection
        //public List<DropDownTO> SelectAllTblModuleList()
        //{
        //    return _iTblModuleDAO.SelectAllTblModule();
        //}
        public List<TblUserTO> SelectAllTblUserList(Boolean onlyActiveYn ,String deptId)
        {
            return _iTblUserDAO.SelectAllTblUser(onlyActiveYn , deptId);
        }

        public TblUserTO SelectTblUserTO(Int32 idUser)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return _iTblUserDAO.SelectTblUser(idUser,conn,tran);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }            
        }

        public int SelectUserByImeiNumber(string idDevice)
        {
            TblUserTO tblUserTo = new TblUserTO();
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                tblUserTo =  _iTblUserDAO.SelectUserByImeiNumber(idDevice, conn, tran);
                if (tblUserTo != null)
                    return tblUserTo.IdUser;
                else return 0;
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
            }

        }

        public TblUserTO SelectTblUserTO(Int32 idUser,SqlConnection conn,SqlTransaction tran)
        {
            return _iTblUserDAO.SelectTblUser(idUser, conn, tran);

        }

        public TblUserTO SelectTblUserTO(String userID,String password)
        {
            return _iTblUserDAO.SelectTblUser(userID,password);
        }

        public Boolean IsThisUserExists(String userId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                Boolean result = IsThisUserExists(userId, conn, tran);
                tran.Commit();
                return result;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        public Boolean IsThisUserExists(String userId,SqlConnection conn,SqlTransaction tran)
        {
            return _iTblUserDAO.IsThisUserExists(userId, conn,tran);
        }

        public Dictionary<int, string> SelectUserMobileNoDCTByUserIdOrRole(String userOrRoleIds, Boolean isUser, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserDAO.SelectUserMobileNoDCTByUserIdOrRole(userOrRoleIds, isUser, conn, tran);

        }
        public Dictionary<int, string> SelectUserMobileNoDCTByUserIdOrRoleForDeliver(String userOrRoleIds, Boolean isUser, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserDAO.SelectUserMobileNoDCTByUserIdOrRoleForDeliver(userOrRoleIds, isUser, conn, tran);

        }
        // added by aniket for email configuration
        public Dictionary<int, string> SelectUserEmailDCTByUserIdOrRole(String userOrRoleIds, Boolean isUser, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserDAO.SelectUserEmailDCTByUserIdOrRole(userOrRoleIds, isUser, conn, tran);

        }
        public Dictionary<int, List<string>> SelectUserMobileNoAndAlterMobileDCTByUserIdOrRole(String userOrRoleIds, Boolean isUser, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserDAO.SelectUserMobileNoAndAlterMobileDCTByUserIdOrRole(userOrRoleIds, isUser, conn, tran);

        }

        public Dictionary<int, string> SelectUserDeviceRegNoDCTByUserIdOrRole(String userOrRoleIds, Boolean isUser, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserDAO.SelectUserDeviceRegNoDCTByUserIdOrRole(userOrRoleIds, isUser, conn, tran);

        }
        public Dictionary<int, string> SelectUserUsingRole(String userOrRoleIds, Boolean isUser, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserDAO.SelectUserUsingRole(userOrRoleIds, isUser, conn, tran);

        }
        public List<TblUserTO> SelectAllTblUserList(Int32 orgId,SqlConnection conn,SqlTransaction tran)
        {
            return _iTblUserDAO.SelectAllTblUser(orgId,conn,tran);
        }

        public List<DropDownTO> SelectAllActiveUsersForDropDown(int moduleId = 0)
        {

            string valConfig = "";
            if (moduleId > 0)
            {
                TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_STOCK_IGNORE_USERS_OF_ROLEIDS_AGAINST_REQUISITION);
                if (tblConfigParamsTO != null)
                {
                    valConfig = tblConfigParamsTO.ConfigParamVal;
                }
            }
            return _iTblUserDAO.SelectAllActiveUsersForDropDown(valConfig);
        }
        //Added by minal 03/03/2021 for get all active user from crm reports
        public List<DropDownTO> GetAllActiveUsersForDropDown()
        {
            return _iTblUserDAO.GetAllActiveUsersForDropDown();
        }
        //Priyanka H [30/07/2019]
        public List<DropDownTO> SelectUserListForIssue()
        {
            ResultMessage rMessage = new ResultMessage();
            TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.IGNORE_ROLEIDS_FOR_ISSUE);
            if (tblConfigParamsTO == null)
            {
                return null;
            }
            return _iTblUserDAO.SelectUserListForIssue(tblConfigParamsTO.ConfigParamVal);
        }

        //Sudhir[08-MAR-2018] Added for Get Users for Organizations Structre Based on User ID's
        public List<DropDownTO> SelectUsersOnUserIds(string userIds)
        {
            return _iTblUserDAO.SelectUsersOnUserIds(userIds);
        }


        public List<TblUserTO> SelectAllTblUserByRoleType(Int32 roleTypeId)
        {
            return _iTblUserDAO.SelectAllTblUserByRoleType(roleTypeId);
        }


        #endregion

        #region Insertion
        public int InsertTblUser(TblUserTO tblUserTO)
        {
            return _iTblUserDAO.InsertTblUser(tblUserTO);
        }

        public int InsertTblUser(TblUserTO tblUserTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserDAO.InsertTblUser(tblUserTO, conn, tran);
        }

        public String CreateUserName(string firstName,string lastName, SqlConnection conn,SqlTransaction tran)
        {
            try
            {
                String userName = string.Empty;
                userName = firstName.TrimEnd(' ') + "." + lastName.TrimEnd(' ');
                Boolean isUserExist = true;
                for (int i = 0; i < 5; i++) //Max 5 Is Considered
                {
                    if(i==0)
                    {
                        isUserExist = IsThisUserExists(userName, conn, tran);
                        if (!isUserExist)
                            return userName;
                        else continue;
                    }
                    else
                    {
                        string newUser = userName + i;
                        isUserExist = IsThisUserExists(newUser, conn, tran);
                        if (!isUserExist)
                            return newUser;
                        else continue;
                    }
                }

                userName = userName + _iCommon.ServerDateTime.ToString("ddMMyyyy");
                return userName;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public Boolean GetUsersQuata(SqlConnection conn, SqlTransaction tran)
        {
            Boolean isValid = true;
            dimUserConfigrationTO dimUserConfigrationTO = _iTblLoginDAO.GetUsersConfigration((int)Constants.UsersConfigration.USER_CONFIG, conn, tran);
            if (dimUserConfigrationTO != null)
            {
                List<TblUserTO> list = _iTblUserDAO.SelectAllTblUser(true,null);
                if (list != null && list.Count > 0)
                {
                    if (Convert.ToInt32(dimUserConfigrationTO.ConfigValue) <= list.Count)
                    {
                        isValid = false;
                    }
                }
            }
            return isValid;
        }
        public ResultMessage SaveNewUser(TblUserTO tblUserTO, Int32 loginUserId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));

            SqlTransaction tran = null;
            ResultMessage rMessage = new ResultMessage();
            DateTime serverDateTime = _iCommon.ServerDateTime;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                #region Check User Creation Limit Added By @KM 13/02/2019
                Boolean isProceedToCreate = GetUsersQuata(conn, tran);
                if(!isProceedToCreate)
                {
                    tran.Rollback();
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.DisplayMessage = "User Quota Exceeded, Please contact your administrative";
                    rMessage.Text = "User Quota Exceeded, Please contact your administrative";
                    return rMessage;
                }
                #endregion
                String userId = CreateUserName(tblUserTO.UserPersonTO.FirstName, tblUserTO.UserPersonTO.LastName, conn, tran);
                userId = userId.ToLower();
                String pwd = Constants.DefaultPassword;

                if (tblUserTO.UserPersonTO.DobDay > 0 && tblUserTO.UserPersonTO.DobMonth > 0 && tblUserTO.UserPersonTO.DobYear > 0)
                {
                    tblUserTO.UserPersonTO.DateOfBirth = new DateTime(tblUserTO.UserPersonTO.DobYear, tblUserTO.UserPersonTO.DobMonth, tblUserTO.UserPersonTO.DobDay);
                }
                else
                {
                    tblUserTO.UserPersonTO.DateOfBirth = DateTime.MinValue;
                }

                tblUserTO.UserPersonTO.CreatedBy = loginUserId;
                tblUserTO.UserPersonTO.CreatedOn = serverDateTime;
                int result = _iTblPersonDAO.InsertTblPerson(tblUserTO.UserPersonTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    rMessage.Text = "Error While InsertTblPerson for Users in Method SaveNewUser ";
                    return rMessage;
                }

                tblUserTO.UserDisplayName = tblUserTO.UserPersonTO.FirstName + " " + tblUserTO.UserPersonTO.LastName;
                tblUserTO.IsActive = 1;
                tblUserTO.UserLogin = userId;
                tblUserTO.UserPasswd = pwd;
                result =InsertTblUser(tblUserTO, conn, tran); 

                if (result != 1)
                {
                    tran.Rollback();
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    rMessage.Text = "Error While InsertTblUser for Users in Method SaveNewUser ";
                    return rMessage;
                }

                tblUserTO.UserExtTO = new TblUserExtTO();
                tblUserTO.UserExtTO.CreatedBy = loginUserId;
                tblUserTO.UserExtTO.CreatedOn = serverDateTime;
                tblUserTO.UserExtTO.PersonId = tblUserTO.UserPersonTO.IdPerson;
                tblUserTO.UserExtTO.UserId = tblUserTO.IdUser;
                tblUserTO.UserExtTO.OrganizationId = tblUserTO.OrganizationId;

                TblAddressTO addressTO = _iTblAddressDAO.SelectOrgAddressWrtAddrType(tblUserTO.OrganizationId, Constants.AddressTypeE.OFFICE_ADDRESS, conn, tran);
                if (addressTO == null)
                {
                    tran.Rollback();
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.Text = "Error..Record could not be saved. Address Details for the organization + " + tblUserTO.OrganizationName + " is not set";
                    rMessage.DisplayMessage = "Error..Record could not be saved. Address Details for the organization + " + tblUserTO.OrganizationName + " is not set";
                    return rMessage;
                }
                tblUserTO.UserExtTO.AddressId = addressTO.IdAddr;

                result = _iTblUserExtDAO.InsertTblUserExt(tblUserTO.UserExtTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.Text = "Error While InsertTblUserExt for Users in Method SaveNewUser ";
                    rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    return rMessage;
                }

                if (tblUserTO.OrgStructId != 0 && tblUserTO.OrgStructId > 0)
                {
                    TblUserReportingDetailsTO tblUserReportingDetailsTO = new TblUserReportingDetailsTO();
                    tblUserReportingDetailsTO.CreatedBy = loginUserId;
                    tblUserReportingDetailsTO.OrgStructureId = tblUserTO.OrgStructId;
                    tblUserReportingDetailsTO.ReportingTo = tblUserTO.ReportingTo;
                    tblUserReportingDetailsTO.UserId = tblUserTO.IdUser;
                    tblUserReportingDetailsTO.CreatedOn = serverDateTime;
                    tblUserReportingDetailsTO.LevelId = tblUserTO.LevelId;
                    tblUserReportingDetailsTO.IsActive = 1;
                    rMessage = _iTblOrgStructureBL.AttachNewUserToOrgStructure(tblUserReportingDetailsTO,null, conn, tran);
                    if (rMessage.MessageType != ResultMessageE.Information)
                    {
                        tran.Rollback();
                        rMessage.MessageType = ResultMessageE.Error;
                        rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        rMessage.Text = "Error While Attch New User to Organization Structure in AttachNewUserToOrgStructure Method";
                        return rMessage;
                    }
                }
                else
                {
                    tblUserTO.UserRoleList[0].UserId = tblUserTO.IdUser;
                    tblUserTO.UserRoleList[0].IsActive = 1;
                    tblUserTO.UserRoleList[0].CreatedBy = loginUserId;
                    tblUserTO.UserRoleList[0].CreatedOn = serverDateTime;

                    result = _iTblUserRoleBL.InsertTblUserRole(tblUserTO.UserRoleList[0], conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        rMessage.MessageType = ResultMessageE.Error;
                        rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        rMessage.Text = "Error While InsertTblUserRole for C&F Users in Method SaveNewUser ";
                        return rMessage;
                    }
                }

                #region RabbitMessaging
                TblConfigParamsTO msgConfigTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_ISRABBITMQ_ENABLED);
                if (msgConfigTO != null && (Convert.ToInt32(msgConfigTO.ConfigParamVal)) == 1)
                {
                    TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.RABBIT_MQ_CONFIGURATION_DETAILS);
                    TenantTO tenantTO = new TenantTO();
                    if (tblConfigParamsTO != null)
                    {
                        tenantTO = JsonConvert.DeserializeObject<TenantTO>(tblConfigParamsTO.ConfigParamVal.ToString());
                    }
                    TblConfigParamsTO userTypeConfigTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.IS_SEND_ALL_TYPE__USER_TO_HRGIRD);

                    if (userTypeConfigTO != null && (Convert.ToInt32(userTypeConfigTO.ConfigParamVal)) == 1)
                    {
                        UserPayload userPayload = _iTblOrgStructureBL.MapUserPayload(tblUserTO, loginUserId, rMessage, conn, tran, (int)Constants.UserMappingTransModeE.CREATE);
                        if (userPayload == null)
                        {
                            tran.Rollback();
                            return rMessage;
                        }

                        //adding history for message 
                        RabbitMessagingHistoryTO rabbitMessagingHistoryTO = new RabbitMessagingHistoryTO();
                        rabbitMessagingHistoryTO.SourceId = tblUserTO.IdUser;
                        rabbitMessagingHistoryTO.RabbitTransId = (int)Constants.RabbitTransactionsE.USER;
                        result = _iRabbitMessagingHistoryDAO.InsertRabbitMessagingHistory(rabbitMessagingHistoryTO);
                        if (result != 1)
                        {
                            tran.Rollback();
                            rMessage.DefaultBehaviour("Error While Insert in RabbitMessagingHistory ");
                            return rMessage;
                        }
                        //add tenentId
                        userPayload.TenantId = tenantTO.TenantId;
                        userPayload.AuthKey = tenantTO.AuthKey;

                        _iMessagePublisher.PublishMessageAsync(Constants.RABBIT_USER_ADD, userPayload, "");
                    }
                    else if ((int)Constants.UserTypeE.EMPLOYEE == tblUserTO.UserTypeId)
                    {
                        UserPayload userPayload = _iTblOrgStructureBL.MapUserPayload(tblUserTO, loginUserId, rMessage, conn, tran, (int)Constants.UserMappingTransModeE.CREATE);
                        if (userPayload == null)
                        {
                            tran.Rollback();
                            return rMessage;
                        }

                        //adding history for message 
                        RabbitMessagingHistoryTO rabbitMessagingHistoryTO = new RabbitMessagingHistoryTO();
                        rabbitMessagingHistoryTO.SourceId = tblUserTO.IdUser;
                        rabbitMessagingHistoryTO.RabbitTransId = (int)Constants.RabbitTransactionsE.USER;
                        result = _iRabbitMessagingHistoryDAO.InsertRabbitMessagingHistory(rabbitMessagingHistoryTO);
                        if (result != 1)
                        {
                            tran.Rollback();
                            rMessage.DefaultBehaviour("Error While Insert in RabbitMessagingHistory ");
                            return rMessage;
                        }
                        //add tenentId
                        userPayload.TenantId = tenantTO.TenantId;
                        userPayload.AuthKey = tenantTO.AuthKey;

                        _iMessagePublisher.PublishMessageAsync(Constants.RABBIT_USER_ADD, userPayload, "");
                    }
                }
                #endregion

                #region ProFlow Integration
                rMessage = PostProFlowUser(tblUserTO);
                if (rMessage == null || rMessage.MessageType != ResultMessageE.Information)
                {
                    tran.Rollback();
                    return rMessage;
                }
                #endregion
                tran.Commit();
                rMessage.DefaultSuccessBehaviour();
                return rMessage;

            }
            catch (Exception ex)
            {
                tran.Rollback();
                rMessage.DefaultExceptionBehaviour(ex, "SaveNewUser");
                return rMessage;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion



        #region rabbitMigration

        public ResultMessage MigrateAllUsers()
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                List<TblUserReportingDetailsTO> tblUserReportingDetailsList = _iTblOrgStructureBL.SelectAllUserReportingDetails(conn, tran);
                if (tblUserReportingDetailsList == null)
                    throw new Exception();
                TblUserTO tblUserTO = null;
                List<TblUserTO> AllUserList = _iTblUserDAO.SelectAllTblUser(true ,null);
                if (AllUserList == null)
                    throw new Exception("Exception while getting User List on User Migration");

                #region Hierarchial users
                for (int index = 0; index < tblUserReportingDetailsList.Count; index++)
                {

                    TblUserReportingDetailsTO tblUserReportingDetailsTO = tblUserReportingDetailsList[index];
                    tblUserTO = AllUserList.
                           Where(e => e.IdUser == tblUserReportingDetailsTO.UserId).FirstOrDefault();
                    TblRoleTO roleTO = _iTblRoleBL.SelectTblRoleOnOrgStructureId(tblUserReportingDetailsTO.OrgStructureId);
                    if (roleTO == null)
                        throw new Exception("exception while selecting role at userId " + tblUserReportingDetailsTO.UserId);
                    tblUserTO.OrgStructId = tblUserReportingDetailsTO.OrgStructureId;
                    TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.RABBIT_MQ_CONFIGURATION_DETAILS);
                    TenantTO tenantTO = new TenantTO();
                    if (tblConfigParamsTO != null)
                    {
                        tenantTO = JsonConvert.DeserializeObject<TenantTO>(tblConfigParamsTO.ConfigParamVal.ToString());
                    }
                    //Import all Employee
                    TblConfigParamsTO userTypeConfigTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.IS_SEND_ALL_TYPE__USER_TO_HRGIRD);
                    if(userTypeConfigTO!=null && Convert.ToInt32(userTypeConfigTO.ConfigParamVal)==1)
                    {
                        tblUserTO.UserPersonTO = _iTblPersonDAO.SelectTblPerson(tblUserTO.PersonId);
                        if (tblUserTO.UserPersonTO == null)
                            throw new Exception("exception while selecting person at userId " + tblUserReportingDetailsTO.UserId);
                        //UserMappingTransModeE is set to update as we dont want to change created and updated by
                        UserPayload userPayload = _iTblOrgStructureBL.MapUserPayload(tblUserTO, tblUserTO.UserPersonTO.CreatedBy, resultMessage, conn,
                                 tran, (int)Constants.UserMappingTransModeE.UPDATE);
                        if (userPayload == null)
                            throw new Exception("exception while selecting userPayload at userId " + tblUserReportingDetailsTO.UserId); ;
                        userPayload.ReportingToUserId = tblUserReportingDetailsTO.ReportingTo;
                        if (tblUserReportingDetailsTO.ReportingTo > 0)
                        {
                            TblUserTO tblReportingUserTO = _iTblUserDAO.SelectTblUser(tblUserReportingDetailsTO.ReportingTo, conn, tran);
                            userPayload.ReportingToUserName = tblReportingUserTO.UserDisplayName;
                        }
                        else
                        {
                            userPayload.ReportingToUserName = "self";
                        }
                        userPayload.RoleId = roleTO.IdRole;
                        userPayload.RoleDesc = roleTO.RoleDesc;
                        //add tenentId
                        RabbitMessagingHistoryTO rabbitMessagingHistoryTO = new RabbitMessagingHistoryTO();
                        rabbitMessagingHistoryTO.SourceId = tblUserTO.IdUser;
                        rabbitMessagingHistoryTO.RabbitTransId = (int)Constants.RabbitTransactionsE.USER;
                        result = _iRabbitMessagingHistoryDAO.InsertRabbitMessagingHistory(rabbitMessagingHistoryTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            throw new Exception("exception at userId " + tblUserTO.IdUser);
                        }
                        userPayload.TenantId = tenantTO.TenantId;
                        userPayload.AuthKey = tenantTO.AuthKey;
                        _iMessagePublisher.PublishMessageAsync(Constants.RABBIT_USER_ADD, userPayload, "");
                        //Adding History
                        //add tenentId
                        userPayload.TenantId = tenantTO.TenantId;
                   
                        AllUserList.Remove(tblUserTO);
                    }
                    else if (tblUserTO.UserTypeId == (int)Constants.UserTypeE.EMPLOYEE)
                    {
                        tblUserTO.UserPersonTO = _iTblPersonDAO.SelectTblPerson(tblUserTO.PersonId);
                        if (tblUserTO.UserPersonTO == null)
                            throw new Exception("exception while selecting person at userId " + tblUserReportingDetailsTO.UserId);
                        //UserMappingTransModeE is set to update as we dont want to change created and updated by
                        UserPayload userPayload = _iTblOrgStructureBL.MapUserPayload(tblUserTO, tblUserTO.UserPersonTO.CreatedBy, resultMessage, conn,
                                 tran, (int)Constants.UserMappingTransModeE.UPDATE);
                        if (userPayload == null)
                            throw new Exception("exception while selecting userPayload at userId " + tblUserReportingDetailsTO.UserId); ;
                        userPayload.ReportingToUserId = tblUserReportingDetailsTO.ReportingTo;
                        if (tblUserReportingDetailsTO.ReportingTo > 0)
                        {
                            TblUserTO tblReportingUserTO = _iTblUserDAO.SelectTblUser(tblUserReportingDetailsTO.ReportingTo, conn, tran);
                            userPayload.ReportingToUserName = tblReportingUserTO.UserDisplayName;
                        }
                        else
                        {
                            userPayload.ReportingToUserName = "self";
                        }
                        userPayload.RoleId = roleTO.IdRole;
                        userPayload.RoleDesc = roleTO.RoleDesc;
                        //add tenentId
                        RabbitMessagingHistoryTO rabbitMessagingHistoryTO = new RabbitMessagingHistoryTO();
                        rabbitMessagingHistoryTO.SourceId = tblUserTO.IdUser;
                        rabbitMessagingHistoryTO.RabbitTransId = (int)Constants.RabbitTransactionsE.USER;
                        result = _iRabbitMessagingHistoryDAO.InsertRabbitMessagingHistory(rabbitMessagingHistoryTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            throw new Exception("exception at userId " + tblUserTO.IdUser);
                        }
                        userPayload.TenantId = tenantTO.TenantId;
                        userPayload.AuthKey = tenantTO.AuthKey;
                        _iMessagePublisher.PublishMessageAsync(Constants.RABBIT_USER_ADD, userPayload, "");
                        //Adding History
                        //add tenentId
                        userPayload.TenantId = tenantTO.TenantId;
                    
                        AllUserList.Remove(tblUserTO);
                    }

                }
                #endregion

                #region Admin Users
                //Commented as Admin in provided by HrGurd itself

                //List<UserPayload> sysAdminList = _iTblUserDAO.SelectAllAdminUsers();
                //if (sysAdminList != null && sysAdminList.Count > 0)
                //{
                //    sysAdminList.ForEach(sysAdmin =>
                //    {
                //        //check if user is not already published with other role
                //        var chkList = AllUserList.Where(e => e.IdUser == sysAdmin.IdUser).ToList();
                //        if (chkList != null && chkList.Count == 1)
                //        {
                //            //add tenentId
                //            sysAdmin.TenantId = _iConnectionString.GetConnectionString(Constants.TENANT_ID);
                //            _iMessagePublisher.PublishMessageAsync(Constants.RABBIT_USER_ADD, sysAdmin, "");
                //            RabbitMessagingHistoryTO rabbitMessagingHistoryTO = new RabbitMessagingHistoryTO();
                //            rabbitMessagingHistoryTO.SourceId = sysAdmin.IdUser;
                //            rabbitMessagingHistoryTO.RabbitTransId = (int)Constants.RabbitTransactionsE.USER;
                //            result = _iRabbitMessagingHistoryDAO.InsertRabbitMessagingHistory(rabbitMessagingHistoryTO, conn, tran);
                //            if (result != 1)
                //            {
                //                tran.Rollback();
                //                throw new Exception("exception at userId " + sysAdmin.IdUser);

                //            }

                //            AllUserList = AllUserList.Where(e => e.IdUser != sysAdmin.IdUser).ToList();
                //        }
                //    });
                //}
                #endregion
                
                #region Remaining Users
                //commented by hrushikesh as this users are not active in System

                //AllUserList.ForEach(user =>
                //{

                //    user.UserPersonTO = _iTblPersonDAO.SelectTblPerson(user.PersonId);
                //    if (tblUserTO.UserPersonTO == null)
                //        throw new Exception("exception while selecting person at userId " + user.IdUser);
                //    user.UserRoleList = _iTblUserRoleBL.SelectAllActiveUserRoleList(user.IdUser);

                //    //Users which donot have roles migration
                //    if (user.UserRoleList != null && user.UserRoleList.Count == 0)
                //    {
                //        TblUserRoleTO userRole = new TblUserRoleTO();
                //        userRole.RoleId = 0;
                //        userRole.RoleDesc = "";
                //        userRole.UserId = user.IdUser;
                //        user.UserRoleList.Add(userRole);
                //    }

                //    UserPayload userPayload = _iTblOrgStructureBL.MapUserPayload(user, user.UserPersonTO.CreatedBy, resultMessage, conn,
                //         tran, (int)Constants.UserMappingTransModeE.UPDATE);
                //    if (userPayload == null)
                //        throw new Exception("exception while selecting userPayload at userId " + user.IdUser); ;

                //    //add tenentId
                //    userPayload.TenantId = _iConnectionString.GetConnectionString(Constants.TENANT_ID);
                //    _iMessagePublisher.PublishMessageAsync(Constants.RABBIT_USER_ADD, userPayload, "");
                //    RabbitMessagingHistoryTO rabbitMessagingHistoryTO = new RabbitMessagingHistoryTO();
                //    rabbitMessagingHistoryTO.SourceId = tblUserTO.IdUser;
                //    rabbitMessagingHistoryTO.RabbitTransId = (int)Constants.RabbitTransactionsE.USER;
                //    result = _iRabbitMessagingHistoryDAO.InsertRabbitMessagingHistory(rabbitMessagingHistoryTO, conn, tran);
                //    if (result != 1)
                //    {
                //        tran.Rollback();
                //        throw new Exception("exception at userId " + tblUserTO.IdUser);

                //    }
                //});
                #endregion

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, ex.Message);
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }


      
      

        #endregion


        #region Updation
        public int UpdateTblUser(TblUserTO tblUserTO,ResultMessage rMessage,int loginUserId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                int result = _iTblUserDAO.UpdateTblUser(tblUserTO,conn,tran);
                if (result == -1)
                {
                    tran.Rollback();
                    return -1;
                }
                #region RabbitMessaging
                TblConfigParamsTO msgConfigTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_ISRABBITMQ_ENABLED);
                if (msgConfigTO != null && (Convert.ToInt32(msgConfigTO.ConfigParamVal)) == 1)
                {
                    TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.RABBIT_MQ_CONFIGURATION_DETAILS);
                    TenantTO tenantTO = new TenantTO();
                    if (tblConfigParamsTO != null)
                    {
                        tenantTO = JsonConvert.DeserializeObject<TenantTO>(tblConfigParamsTO.ConfigParamVal.ToString());
                    }
                    TblConfigParamsTO userTypeConfigTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.IS_SEND_ALL_TYPE__USER_TO_HRGIRD);
                    if (userTypeConfigTO != null && Convert.ToInt32(userTypeConfigTO.ConfigParamVal) == 1)
                    {
                        UserPayload userPayload = new UserPayload();
                        userPayload.MessageId = new Guid();
                        userPayload.IdUser = tblUserTO.IdUser;
                        userPayload.DeactivatedOn = tblUserTO.DeactivatedOn;
                        userPayload.DeactivatedBy = tblUserTO.DeactivatedBy;
                        //add tenentId
                        userPayload.TenantId = tenantTO.TenantId;
                        userPayload.AuthKey = tenantTO.AuthKey;
                        _iMessagePublisher.PublishMessageAsync(Constants.RABBIT_USER_DEACTIVATED, userPayload, "");
                    }
                    else if ((int)Constants.UserTypeE.EMPLOYEE == tblUserTO.UserTypeId)
                    {
                        UserPayload userPayload = new UserPayload();
                        userPayload.MessageId = new Guid();
                        userPayload.IdUser = tblUserTO.IdUser;
                        userPayload.DeactivatedOn = tblUserTO.DeactivatedOn;
                        userPayload.DeactivatedBy = tblUserTO.DeactivatedBy;
                        //add tenentId
                        userPayload.TenantId = tenantTO.TenantId;
                        userPayload.AuthKey = tenantTO.AuthKey;
                        _iMessagePublisher.PublishMessageAsync(Constants.RABBIT_USER_DEACTIVATED, userPayload, "");

                    }
                }

                #endregion
                tran.Commit();
                return result;
            }
            catch (Exception e)
            {
                return -1;
            }
            finally
            {
                conn.Close();
            }

        }

        //Harshala added
        public int ActivateOrDeactivateUser(TblUserTO tblUserTO)
        { 
            try
            {
                return _iTblUserDAO.ActivateOrDeactivateTblUser(tblUserTO);
            }
            catch (Exception e)
            {
                return -1;
            }
            finally
            {
                
            }

        }

        /// <summary>
        /// Saket [2018-03-06] Added to save the history for PWd
        /// </summary>
        /// <param name="tblUserTO"></param>
        /// <param name="tblUserPwdHistoryTO"></param>
        /// <returns></returns>
        public ResultMessage UpdateTblUser(TblUserTO tblUserTO, TblUserPwdHistoryTO tblUserPwdHistoryTO,TblUserTO userTOForRabbitMessage)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage rMessage = new ResultMessage();
            DateTime serverDateTime = _iCommon.ServerDateTime;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                int result = UpdateTblUser(tblUserTO, conn, tran);
                if (result != 1)
                {
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.Result = 0;
                    rMessage.Text = "Error In Updating Password";
                    return rMessage;
                }

                if (tblUserPwdHistoryTO != null)
                {
                    result = _iTblUserPwdHistoryDAO.InsertTblUserPwdHistory(tblUserPwdHistoryTO, conn, tran);
                    if (result != 1)
                    {
                        rMessage.MessageType = ResultMessageE.Error;
                        rMessage.Result = 0;
                        rMessage.Text = "Error In Inserting Password history";
                        return rMessage;
                    }
                }

                //#region RabbitMessaging
                //TblConfigParamsTO msgConfigTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_ISRABBITMQ_ENABLED);
                //if (msgConfigTO != null && (Convert.ToInt32(msgConfigTO.ConfigParamVal)) == 1)
                //{
                //    TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.RABBIT_MQ_CONFIGURATION_DETAILS);
                //    TenantTO tenantTO = new TenantTO();
                //    if (tblConfigParamsTO != null)
                //    {
                //        tenantTO = JsonConvert.DeserializeObject<TenantTO>(tblConfigParamsTO.ConfigParamVal.ToString());
                //    }
                //    TblConfigParamsTO userTypeConfigTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.IS_SEND_ALL_TYPE__USER_TO_HRGIRD);
                //    if (userTypeConfigTO != null && (Convert.ToInt32(userTypeConfigTO.ConfigParamVal)) == 1)
                //    {
                //        UserPayload userPayload = _iTblOrgStructureBL.MapUserPayload(userTOForRabbitMessage, userTOForRabbitMessage.IdUser, rMessage, conn, tran, (int)Constants.UserMappingTransModeE.UPDATE);
                //        if (userPayload == null)
                //        {
                //            tran.Rollback();
                //            return rMessage;
                //        }

                //        //add tenentId
                //        userPayload.TenantId = tenantTO.TenantId;
                //        userPayload.AuthKey = tenantTO.AuthKey;
                //        _iMessagePublisher.PublishMessageAsync(Constants.RABBIT_USER_UPDATE, userPayload, "");
                //    }
                //    else if ((int)Constants.UserTypeE.EMPLOYEE == userTOForRabbitMessage.UserTypeId)
                //    {
                //        UserPayload userPayload = _iTblOrgStructureBL.MapUserPayload(userTOForRabbitMessage, userTOForRabbitMessage.IdUser, rMessage, conn, tran, (int)Constants.UserMappingTransModeE.UPDATE);
                //        if (userPayload == null)
                //        {
                //            tran.Rollback();
                //            return rMessage;
                //        }
                //        //add tenentId
                //        userPayload.TenantId = tenantTO.TenantId;
                //        userPayload.AuthKey = tenantTO.AuthKey;
                //        _iMessagePublisher.PublishMessageAsync(Constants.RABBIT_USER_UPDATE, userPayload, "");
                //    }
                //}
                //#endregion



                tran.Commit();
                rMessage.MessageType = ResultMessageE.Information;
                rMessage.Text = "Record Saved Successfully";
                rMessage.DisplayMessage = "Record Saved Successfully";
                rMessage.Result = 1;
                return rMessage;

            }
            catch (Exception ex)
            {
                tran.Rollback();
                rMessage.MessageType = ResultMessageE.Error;
                rMessage.Exception = ex;
                rMessage.Result = -1;
                rMessage.Text = "Exception Error While UpdateUser Method UpdateUser";
                rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                return rMessage;
            }
            finally
            {
                conn.Close();
            }
        }


        public int UpdateTblUser(TblUserTO tblUserTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserDAO.UpdateTblUser(tblUserTO, conn, tran);
        }

        public ResultMessage UpdateUser(TblUserTO tblUserTO, Int32 loginUserId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage rMessage = new ResultMessage();
            DateTime serverDateTime = _iCommon.ServerDateTime;
            try
            {
                  // add Logout Code For Session Out
                         string LoginIds="";
                    if(tblUserTO.IsSetDeviceId)
                    {
                         LoginIds= _iCommon.SelectApKLoginArray(tblUserTO.IdUser);
                    }
              
            // end
                conn.Open();
                tran = conn.BeginTransaction();
                int result = 0;
                if (tblUserTO.UserPersonTO != null)
                {
                    if (tblUserTO.UserPersonTO.DobDay > 0 && tblUserTO.UserPersonTO.DobMonth > 0 && tblUserTO.UserPersonTO.DobYear > 0)
                    {
                        tblUserTO.UserPersonTO.DateOfBirth = new DateTime(tblUserTO.UserPersonTO.DobYear, tblUserTO.UserPersonTO.DobMonth, tblUserTO.UserPersonTO.DobDay);
                    }
                    else
                    {
                        tblUserTO.UserPersonTO.DateOfBirth = DateTime.MinValue;
                    }

                    result = _iTblPersonDAO.UpdateTblPerson(tblUserTO.UserPersonTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        rMessage.MessageType = ResultMessageE.Error;
                        rMessage.Text = "Error While UpdateTblPerson for Users in Method UpdateUser ";
                        rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        return rMessage;
                    }

                    tblUserTO.UserDisplayName = tblUserTO.UserPersonTO.FirstName + " " + tblUserTO.UserPersonTO.LastName;

                }

                result = UpdateTblUser(tblUserTO, conn, tran);

                if (result != 1)
                {
                    tran.Rollback();
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.Text = "Error While InsertTblUser for Users in Method UpdateUser ";
                    rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    return rMessage;
                }

                tblUserTO.UserExtTO.PersonId = tblUserTO.UserPersonTO.IdPerson;
                tblUserTO.UserExtTO.UserId = tblUserTO.IdUser;
                tblUserTO.UserExtTO.OrganizationId = tblUserTO.OrganizationId;

                result = _iTblUserExtDAO.UpdateTblUserExt(tblUserTO.UserExtTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.Text = "Error While InsertTblUserExt for Users in Method UpdateUser ";
                    rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    return rMessage;
                }

                result = _iTblUserRoleBL.UpdateTblUserRole(tblUserTO.UserRoleList[0], conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.Text = "Error While UpdateTblUserRole for C&F Users in Method UpdateUser ";
                    rMessage.DisplayMessage = Constants.DefaultErrorMsg;

                    return rMessage;
                }


                //Update User Reporting Details
                if (tblUserTO.UserReportingId > 0)
                {
                    TblUserReportingDetailsTO tblUserReportingDetailsTO = _iTblOrgStructureBL.SelectUserReportingDetailsTO(tblUserTO.UserReportingId, conn, tran);
                    if (tblUserReportingDetailsTO != null)
                    {
                        tblUserReportingDetailsTO.UpdatedBy = loginUserId;
                        tblUserReportingDetailsTO.UpdatedOn = serverDateTime;
                        tblUserReportingDetailsTO.UserId = tblUserTO.IdUser;
                        tblUserReportingDetailsTO.ReportingTo = tblUserTO.ReportingTo;
                        tblUserReportingDetailsTO.OrgStructureId = tblUserTO.OrgStructId;
                        tblUserReportingDetailsTO.LevelId = tblUserTO.LevelId;
                        tblUserReportingDetailsTO.IsActive = 1;
                        result = _iTblOrgStructureBL.UpdateUserReportingDetail(tblUserReportingDetailsTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            rMessage.MessageType = ResultMessageE.Error;
                            rMessage.Text = "Error While UpdateTblUserRole for C&F Users in Method UpdateUser ";
                            rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                            return rMessage;
                        }
                    }
                }
   // add Logout Code For Session Out

              if(!string.IsNullOrEmpty(LoginIds))
              {
                     string[] x=LoginIds.Split(','); 
                      foreach(string item in x){
                      TblModuleCommHisTO TblModuleCommHis=new TblModuleCommHisTO();
                      TblModuleCommHis.LoginId=Convert.ToInt32(item);
                  result= _iTblModuleBL.UpdateTblModuleCommHisBeforeLoginForAPK(TblModuleCommHis,conn,tran);
                             if (result != 1)
                        {
                            tran.Rollback();
                            rMessage.MessageType = ResultMessageE.Error;
                            rMessage.Text = "Error While UpdateTblLogin  Method UpdateUser ";
                            rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                            return rMessage;
                        }
                          }
              }
                // end
                #region RabbitMessaging
                TblConfigParamsTO msgConfigTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_ISRABBITMQ_ENABLED);
                if (msgConfigTO != null && (Convert.ToInt32(msgConfigTO.ConfigParamVal)) == 1)
                {
                    RabbitMessagingHistoryTO rabbitMessagingOldHistoryTO = null;

                    rabbitMessagingOldHistoryTO = _iRabbitMessagingHistoryDAO.SelectRabbitMessagingHistory(tblUserTO.IdUser, (int)Constants.RabbitTransactionsE.USER);
                    TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.RABBIT_MQ_CONFIGURATION_DETAILS);
                    TenantTO tenantTO = new TenantTO();
                    if (tblConfigParamsTO != null)
                    {
                        tenantTO = JsonConvert.DeserializeObject<TenantTO>(tblConfigParamsTO.ConfigParamVal.ToString());
                    }
                    //allow if message is already sent for previous userType 

                    TblConfigParamsTO userTypeConfigTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.IS_SEND_ALL_TYPE__USER_TO_HRGIRD);

                    if ((userTypeConfigTO != null && (Convert.ToInt32(userTypeConfigTO.ConfigParamVal)) == 1) || rabbitMessagingOldHistoryTO != null)
                    {
                        UserPayload userPayload = _iTblOrgStructureBL.MapUserPayload(tblUserTO, loginUserId, rMessage, conn, tran, (int)Constants.UserMappingTransModeE.UPDATE);
                        if (userPayload == null)
                        {
                            tran.Rollback();
                            return rMessage;
                        }
                        RabbitMessagingHistoryTO rabbitMessagingHistoryTO = new RabbitMessagingHistoryTO();
                        rabbitMessagingHistoryTO.SourceId = tblUserTO.IdUser;
                        rabbitMessagingHistoryTO.RabbitTransId = (int)Constants.RabbitTransactionsE.USER;
                        result = _iRabbitMessagingHistoryDAO.InsertRabbitMessagingHistory(rabbitMessagingHistoryTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            rMessage.MessageType = ResultMessageE.Error;
                            rMessage.Text = "Error While Insert in RabbitMessagingHistory ";
                            rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                            return rMessage;
                        }
                        //add tenentId
                        userPayload.TenantId = tenantTO.TenantId;
                        userPayload.AuthKey = tenantTO.AuthKey;
                        if (rabbitMessagingOldHistoryTO != null)
                            _iMessagePublisher.PublishMessageAsync(Constants.RABBIT_USER_UPDATE, userPayload, "");
                        else
                            _iMessagePublisher.PublishMessageAsync(Constants.RABBIT_USER_ADD, userPayload, "");
                    }
                    else if ((int)Constants.UserTypeE.EMPLOYEE == tblUserTO.UserTypeId || rabbitMessagingOldHistoryTO != null)
                    {
                        UserPayload userPayload = _iTblOrgStructureBL.MapUserPayload(tblUserTO, loginUserId, rMessage, conn, tran, (int)Constants.UserMappingTransModeE.UPDATE);
                        if (userPayload == null)
                        {
                            tran.Rollback();
                            return rMessage;
                        }
                        RabbitMessagingHistoryTO rabbitMessagingHistoryTO = new RabbitMessagingHistoryTO();
                        rabbitMessagingHistoryTO.SourceId = tblUserTO.IdUser;
                        rabbitMessagingHistoryTO.RabbitTransId = (int)Constants.RabbitTransactionsE.USER;
                        result = _iRabbitMessagingHistoryDAO.InsertRabbitMessagingHistory(rabbitMessagingHistoryTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            rMessage.MessageType = ResultMessageE.Error;
                            rMessage.Text = "Error While Insert in RabbitMessagingHistory ";
                            rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                            return rMessage;
                        }
                        //add tenentId
                        userPayload.TenantId = tenantTO.TenantId;
                        userPayload.AuthKey = tenantTO.AuthKey;
                        if (rabbitMessagingOldHistoryTO != null)
                            _iMessagePublisher.PublishMessageAsync(Constants.RABBIT_USER_UPDATE, userPayload, "");
                        else
                            _iMessagePublisher.PublishMessageAsync(Constants.RABBIT_USER_ADD, userPayload, "");
                    }

                }
                #endregion
                rMessage = PostProFlowUser(tblUserTO, 1);
                if (rMessage == null || rMessage.MessageType != ResultMessageE.Information)
                {
                    tran.Rollback();
                    return rMessage;
                }
                tran.Commit();
                rMessage.MessageType = ResultMessageE.Information;
                rMessage.Text = "Record Saved Successfully";
                rMessage.DisplayMessage = "Record Saved Successfully";
                rMessage.Result = 1;
                return rMessage;

            }
            catch (Exception ex)
            {
                tran.Rollback();
                rMessage.MessageType = ResultMessageE.Error;
                rMessage.Exception = ex;
                rMessage.Result = -1;
                rMessage.Text = "Exception Error While UpdateUser Method UpdateUser ";
                rMessage.DisplayMessage = Constants.DefaultErrorMsg;
                return rMessage;
            }
            finally
            {
                conn.Close();
            }
        }
        // TransactionId 0 for Add, 1 for edit, 2 for disable user
        public ResultMessage PostProFlowUser(TblUserTO tblUserTO, Int32 TransactionId = 0)
        {
            ResultMessage rMessage = new ResultMessage();
            try
            {
                lock (proFlowUserLock)
                {
                    if(TransactionId == 0)
                    {
                        #region ProFlow Integration
                        if (tblUserTO.IsProFlowUser == 1)
                        {
                            ProFlowSettingTO proFlowSettingTO = null;
                            TblConfigParamsTO proFlowConfigurationTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_USER_PRO_FLOW_INTEGRATION_CONFIGURATION);
                            if (proFlowConfigurationTO != null)
                            {
                                proFlowSettingTO = Newtonsoft.Json.JsonConvert.DeserializeObject<ProFlowSettingTO>(proFlowConfigurationTO.ConfigParamVal);
                            }
                            if (proFlowSettingTO == null)
                            {
                                rMessage.MessageType = ResultMessageE.Error;
                                rMessage.DisplayMessage = "ProFlow Configuration Details Not Found";
                                rMessage.Text = "ProFlow Configuration Details Not Found";
                                return rMessage;
                            }
                            if (proFlowSettingTO.IS_SERVICE_ENABLE == 1)
                            {
                                List<ProFlowUserListTO> proFlowUserListTOs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ProFlowUserListTO>>(_iCommon.GetProFlowUserList(proFlowSettingTO));
                                if (proFlowUserListTOs != null && proFlowUserListTOs.Count > 0)
                                {
                                    proFlowUserListTOs = proFlowUserListTOs.Where(w => w.Status == "Yes").ToList();
                                    if (proFlowUserListTOs.Count >= proFlowSettingTO.LICENCE_CNT)
                                    {
                                        rMessage.MessageType = ResultMessageE.Error;
                                        rMessage.DisplayMessage = "ProFlow User Licence Count Exceeded";
                                        rMessage.Text = "ProFlow User Licence Exceeded";
                                        return rMessage;
                                    }
                                }
                                if (proFlowSettingTO.LICENCE_CNT > 0)
                                {
                                    #region Add user
                                    ProFlowUserTO proFlowUserTO = new ProFlowUserTO();
                                    proFlowUserTO.Client_ID = proFlowSettingTO.CLIENT_ID;
                                    proFlowUserTO.Username = proFlowSettingTO.CLIENT_USER_NAME;
                                    proFlowUserTO.Login_ID = tblUserTO.UserLogin;
                                    proFlowUserTO.Password = tblUserTO.UserPasswd;
                                    proFlowUserTO.Role = proFlowSettingTO.DEFAULT_ROLE;
                                    proFlowUserTO.Name = tblUserTO.UserDisplayName;
                                    proFlowUserTO.Email = tblUserTO.UserPersonTO.PrimaryEmail;
                                    proFlowUserTO.ActiveUser = "Yes";
                                    List<CreateProFlowUserReponseTO> CreateProFlowUserReponseTOList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CreateProFlowUserReponseTO>>(_iCommon.CreateProFlowUser(proFlowUserTO, proFlowSettingTO));
                                    if (CreateProFlowUserReponseTOList == null || CreateProFlowUserReponseTOList.Count == 0 || String.IsNullOrEmpty(CreateProFlowUserReponseTOList[0].Client_ID))
                                    {
                                        rMessage.MessageType = ResultMessageE.Error;
                                        rMessage.DisplayMessage = "Failed to Add ProFlow user";
                                        rMessage.data = CreateProFlowUserReponseTOList;
                                        rMessage.Text = CreateProFlowUserReponseTOList[0].Status;
                                        return rMessage;
                                    }
                                    #endregion
                                }
                            }
                        }
                        #endregion
                    }
                    else if(TransactionId == 1)
                    {
                        return UpdateProFlowUser(tblUserTO);
                    }
                    else if (TransactionId == 2)
                    {
                        ProFlowSettingTO proFlowSettingTO = null;
                        TblConfigParamsTO proFlowConfigurationTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_USER_PRO_FLOW_INTEGRATION_CONFIGURATION);
                        if (proFlowConfigurationTO != null)
                        {
                            proFlowSettingTO = Newtonsoft.Json.JsonConvert.DeserializeObject<ProFlowSettingTO>(proFlowConfigurationTO.ConfigParamVal);
                        }
                        if (proFlowSettingTO == null)
                        {
                            rMessage.MessageType = ResultMessageE.Error;
                            rMessage.DisplayMessage = "ProFlow Configuration Details Not Found";
                            rMessage.Text = "ProFlow Configuration Details Not Found";
                            return rMessage;
                        }
                        if (proFlowSettingTO.IS_SERVICE_ENABLE == 1)
                        {
                            List<ProFlowUserListTO> proFlowUserListTOs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ProFlowUserListTO>>(_iCommon.GetProFlowUserList(proFlowSettingTO));
                            if (proFlowUserListTOs != null && proFlowUserListTOs.Count > 0)
                            {
                                ProFlowUserListTO tempProFlowUserTO = proFlowUserListTOs.Where(w => w.Login_ID == tblUserTO.UserLogin).FirstOrDefault();
                                if (tempProFlowUserTO != null && tempProFlowUserTO.Status == "Yes")
                                {
                                    #region update user
                                    UpdateProFlowUserTO updateProFlowUserReponseTO = new UpdateProFlowUserTO();
                                    updateProFlowUserReponseTO.Client_ID = proFlowSettingTO.CLIENT_ID;
                                    updateProFlowUserReponseTO.Username = proFlowSettingTO.CLIENT_USER_NAME;
                                    updateProFlowUserReponseTO.Login_ID = tblUserTO.UserLogin;
                                    updateProFlowUserReponseTO.Fieldname = "Is_Active";
                                    updateProFlowUserReponseTO.Value = "No";
                                    List<ProFlowUserListTO> CreateProFlowUserReponseTOList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ProFlowUserListTO>>(_iCommon.UpdateProFlowUser(updateProFlowUserReponseTO, proFlowSettingTO));
                                    if (CreateProFlowUserReponseTOList == null || CreateProFlowUserReponseTOList.Count == 0 || (String.IsNullOrEmpty(CreateProFlowUserReponseTOList[0].Status) && String.IsNullOrEmpty(CreateProFlowUserReponseTOList[0].Column1)) || (CreateProFlowUserReponseTOList[0].Status != "Success" && CreateProFlowUserReponseTOList[0].Column1 != "Success"))
                                    {
                                        rMessage.MessageType = ResultMessageE.Error;
                                        rMessage.DisplayMessage = "Failed to Add ProFlow user";
                                        rMessage.data = CreateProFlowUserReponseTOList;
                                        rMessage.Text = CreateProFlowUserReponseTOList[0].Status;
                                        return rMessage;
                                    }
                                    #endregion
                                }
                            }
                        }
                    }
                }
                rMessage.DefaultSuccessBehaviour();
                return rMessage;
            }
            catch (Exception ex)
            {
                rMessage.DefaultExceptionBehaviour(ex, "PostProFlowUser");
                return rMessage;
            }
            finally
            {

            }
        }
        public ResultMessage UpdateProFlowUser(TblUserTO tblUserTO)
        {
            ResultMessage rMessage = new ResultMessage();
            try
            {
                #region ProFlow Integration
                ProFlowSettingTO proFlowSettingTO = null;
                TblConfigParamsTO proFlowConfigurationTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_USER_PRO_FLOW_INTEGRATION_CONFIGURATION);
                if (proFlowConfigurationTO != null)
                {
                    proFlowSettingTO = Newtonsoft.Json.JsonConvert.DeserializeObject<ProFlowSettingTO>(proFlowConfigurationTO.ConfigParamVal);
                }
                if (proFlowSettingTO == null)
                {
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.DisplayMessage = "ProFlow Configuration Details Not Found";
                    rMessage.Text = "ProFlow Configuration Details Not Found";
                    return rMessage;
                }
                if (proFlowSettingTO.IS_SERVICE_ENABLE == 1)
                {
                    ProFlowUserTO proFlowUserTO = new ProFlowUserTO();
                    ProFlowUserListTO proFlowUserListTO = null;
                    List<ProFlowUserListTO> proFlowUserListTOs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ProFlowUserListTO>>(_iCommon.GetProFlowUserList(proFlowSettingTO));
                    if (proFlowUserListTOs != null && proFlowUserListTOs.Count > 0)
                    {
                        ProFlowUserListTO tempProFlowUserTO = proFlowUserListTOs.Where(w => w.Login_ID == tblUserTO.UserLogin).FirstOrDefault();
                        if (tempProFlowUserTO != null)
                        {
                            if(tempProFlowUserTO.Status == "No" && tblUserTO.IsProFlowUser == 1)
                            {
                                List<ProFlowUserListTO> tempProFlowUserListTOs = proFlowUserListTOs.Where(w => w.Status == "Yes").ToList();
                                if (tempProFlowUserListTOs.Count >= proFlowSettingTO.LICENCE_CNT)
                                {
                                    rMessage.MessageType = ResultMessageE.Error;
                                    rMessage.DisplayMessage = "ProFlow User Licence Count Exceeded";
                                    rMessage.Text = "ProFlow User Licence Exceeded";
                                    return rMessage;
                                }
                            }
                            #region update user
                            proFlowUserTO.ActiveUser = "No";
                            if (tblUserTO.IsProFlowUser == 1)
                                proFlowUserTO.ActiveUser = "Yes";
                            UpdateProFlowUserTO updateProFlowUserReponseTO = new UpdateProFlowUserTO();
                            updateProFlowUserReponseTO.Client_ID = proFlowSettingTO.CLIENT_ID;
                            updateProFlowUserReponseTO.Username = proFlowSettingTO.CLIENT_USER_NAME;
                            updateProFlowUserReponseTO.Login_ID = tblUserTO.UserLogin;
                            if (!String.IsNullOrEmpty(tblUserTO.UserPersonTO.PrimaryEmail))
                            {
                                updateProFlowUserReponseTO.Fieldname = "User_Full_Name,Email_ID,Is_Active";
                                updateProFlowUserReponseTO.Value = tblUserTO.UserDisplayName;
                                updateProFlowUserReponseTO.Value += "," + tblUserTO.UserPersonTO.PrimaryEmail;
                                updateProFlowUserReponseTO.Value += "," + proFlowUserTO.ActiveUser;
                            }
                            else
                            {
                                updateProFlowUserReponseTO.Fieldname = "User_Full_Name,Is_Active";
                                updateProFlowUserReponseTO.Value = tblUserTO.UserDisplayName;
                                updateProFlowUserReponseTO.Value += "," + proFlowUserTO.ActiveUser;
                            }
                            List<ProFlowUserListTO> CreateProFlowUserReponseTOList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ProFlowUserListTO>>(_iCommon.UpdateProFlowUser(updateProFlowUserReponseTO, proFlowSettingTO));
                            if (CreateProFlowUserReponseTOList == null || CreateProFlowUserReponseTOList.Count == 0 || (String.IsNullOrEmpty(CreateProFlowUserReponseTOList[0].Status) && String.IsNullOrEmpty(CreateProFlowUserReponseTOList[0].Column1)) || (CreateProFlowUserReponseTOList[0].Status != "Success" && CreateProFlowUserReponseTOList[0].Column1 != "Success"))
                            {
                                rMessage.MessageType = ResultMessageE.Error;
                                rMessage.DisplayMessage = "Failed to Add ProFlow user";
                                rMessage.data = CreateProFlowUserReponseTOList;
                                rMessage.Text = CreateProFlowUserReponseTOList[0].Status;
                                return rMessage;
                            }
                            #endregion
                        }
                        else if(tblUserTO.IsProFlowUser == 1)
                        {
                            List<ProFlowUserListTO> tempProFlowUserListTOs = proFlowUserListTOs.Where(w => w.Status == "Yes").ToList();
                            if (tempProFlowUserListTOs.Count >= proFlowSettingTO.LICENCE_CNT)
                            {
                                rMessage.MessageType = ResultMessageE.Error;
                                rMessage.DisplayMessage = "ProFlow User Licence Count Exceeded";
                                rMessage.Text = "ProFlow User Licence Exceeded";
                                return rMessage;
                            }
                            if (proFlowSettingTO.LICENCE_CNT > 0)
                            {
                                #region Add user
                                proFlowUserTO.Client_ID = proFlowSettingTO.CLIENT_ID;
                                proFlowUserTO.Username = proFlowSettingTO.CLIENT_USER_NAME;
                                proFlowUserTO.Login_ID = tblUserTO.UserLogin;
                                proFlowUserTO.Password = tblUserTO.UserPasswd;
                                proFlowUserTO.Role = proFlowSettingTO.DEFAULT_ROLE;
                                proFlowUserTO.Name = tblUserTO.UserDisplayName;
                                proFlowUserTO.Email = tblUserTO.UserPersonTO.PrimaryEmail;
                                proFlowUserTO.ActiveUser = "Yes";
                                List<CreateProFlowUserReponseTO> CreateProFlowUserReponseTOList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CreateProFlowUserReponseTO>>(_iCommon.CreateProFlowUser(proFlowUserTO, proFlowSettingTO));
                                if (CreateProFlowUserReponseTOList == null || CreateProFlowUserReponseTOList.Count == 0 || String.IsNullOrEmpty(CreateProFlowUserReponseTOList[0].Client_ID))
                                {
                                    rMessage.MessageType = ResultMessageE.Error;
                                    rMessage.DisplayMessage = "Failed to Add ProFlow user";
                                    rMessage.data = CreateProFlowUserReponseTOList;
                                    rMessage.Text = CreateProFlowUserReponseTOList[0].Status;
                                    return rMessage;
                                }
                                #endregion
                            }
                        }
                    }
                    if((proFlowUserListTOs == null || proFlowUserListTOs.Count == 0) && tblUserTO.IsProFlowUser == 1)
                    {
                        if(proFlowSettingTO.LICENCE_CNT > 0)
                        {
                            #region Add user
                            proFlowUserTO.Client_ID = proFlowSettingTO.CLIENT_ID;
                            proFlowUserTO.Username = proFlowSettingTO.CLIENT_USER_NAME;
                            proFlowUserTO.Login_ID = tblUserTO.UserLogin;
                            proFlowUserTO.Password = tblUserTO.UserPasswd;
                            proFlowUserTO.Role = proFlowSettingTO.DEFAULT_ROLE;
                            proFlowUserTO.Name = tblUserTO.UserDisplayName;
                            proFlowUserTO.Email = tblUserTO.UserPersonTO.PrimaryEmail;
                            proFlowUserTO.ActiveUser = "Yes";
                            List<CreateProFlowUserReponseTO> CreateProFlowUserReponseTOList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CreateProFlowUserReponseTO>>(_iCommon.CreateProFlowUser(proFlowUserTO, proFlowSettingTO));
                            if (CreateProFlowUserReponseTOList == null || CreateProFlowUserReponseTOList.Count == 0 || String.IsNullOrEmpty(CreateProFlowUserReponseTOList[0].Client_ID))
                            {
                                rMessage.MessageType = ResultMessageE.Error;
                                rMessage.DisplayMessage = "Failed to Add ProFlow user";
                                rMessage.data = CreateProFlowUserReponseTOList;
                                rMessage.Text = CreateProFlowUserReponseTOList[0].Status;
                                return rMessage;
                            }
                            #endregion
                        } 
                    }
                }
                #endregion
                rMessage.DefaultSuccessBehaviour();
                return rMessage;
            }
            catch (Exception ex)
            {
                rMessage.DefaultExceptionBehaviour(ex, "UpdateProFlowUser");
                return rMessage;
            }
            finally
            {
                
            }
        }
        #endregion

        #region Deletion
        public int DeleteTblUser(Int32 idUser)
        {
            return _iTblUserDAO.DeleteTblUser(idUser);
        }

        public int DeleteTblUser(Int32 idUser, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserDAO.DeleteTblUser(idUser, conn, tran);
        }

       

      

        #endregion

    }
}
