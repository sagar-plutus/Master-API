using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using simpliMASTERSAPI.MessageQueuePayloads;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{ 
    public interface ITblUserBL
    {

        ResultMessage MigrateAllUsers();
        //Added by minal 03/03/2021 for get all active user from crm reports
        List<DropDownTO> GetAllActiveUsersForDropDown();
        //List<DropDownTO> SelectAllTblModuleList();
        List<TblUserTO> SelectAllTblUserList(Boolean onlyActiveYn , String deptId);
        TblUserTO SelectTblUserTO(Int32 idUser);
        int SelectUserByImeiNumber(string idDevice);
        TblUserTO SelectTblUserTO(Int32 idUser, SqlConnection conn, SqlTransaction tran);
        TblUserTO SelectTblUserTO(String userID, String password);
        Boolean IsThisUserExists(String userId);
        Boolean IsThisUserExists(String userId, SqlConnection conn, SqlTransaction tran);
        Dictionary<int, string> SelectUserMobileNoDCTByUserIdOrRole(String userOrRoleIds, Boolean isUser, SqlConnection conn, SqlTransaction tran);
        Dictionary<int, string> SelectUserMobileNoDCTByUserIdOrRoleForDeliver(String userOrRoleIds, Boolean isUser, SqlConnection conn, SqlTransaction tran);
        Dictionary<int, string> SelectUserEmailDCTByUserIdOrRole(String userOrRoleIds, Boolean isUser, SqlConnection conn, SqlTransaction tran);
        Dictionary<int, List<string>> SelectUserMobileNoAndAlterMobileDCTByUserIdOrRole(String userOrRoleIds, Boolean isUser, SqlConnection conn, SqlTransaction tran);
        Dictionary<int, string> SelectUserDeviceRegNoDCTByUserIdOrRole(String userOrRoleIds, Boolean isUser, SqlConnection conn, SqlTransaction tran);
        Dictionary<int, string> SelectUserUsingRole(String userOrRoleIds, Boolean isUser, SqlConnection conn, SqlTransaction tran);
        List<TblUserTO> SelectAllTblUserList(Int32 orgId, SqlConnection conn, SqlTransaction tran);
        List<DropDownTO> SelectAllActiveUsersForDropDown(int moduleId = 0);
        List<DropDownTO> SelectUsersOnUserIds(string userIds);
        List<TblUserTO> SelectAllTblUserByRoleType(Int32 roleTypeId);
        int InsertTblUser(TblUserTO tblUserTO);
        int InsertTblUser(TblUserTO tblUserTO, SqlConnection conn, SqlTransaction tran);
        String CreateUserName(string firstName, string lastName, SqlConnection conn, SqlTransaction tran);
        ResultMessage SaveNewUser(TblUserTO tblUserTO, Int32 loginUserId);
        int UpdateTblUser(TblUserTO tblUserTO, ResultMessage rMessage, int loginUserId);
        ResultMessage UpdateTblUser(TblUserTO tblUserTO, TblUserPwdHistoryTO tblUserPwdHistoryTO, TblUserTO userTOForRabbitMessage);
        int UpdateTblUser(TblUserTO tblUserTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage UpdateUser(TblUserTO tblUserTO, Int32 loginUserId);
        int DeleteTblUser(Int32 idUser);
        int DeleteTblUser(Int32 idUser, SqlConnection conn, SqlTransaction tran);
        Boolean GetUsersQuata(SqlConnection conn, SqlTransaction tran);
        List<DropDownTO> SelectUserListForIssue();
        int ActivateOrDeactivateUser(TblUserTO tblUserTO);
        ResultMessage PostProFlowUser(TblUserTO tblUserTO, Int32 TransactionId = 0);
    }
}