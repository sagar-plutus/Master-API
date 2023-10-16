using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
using simpliMASTERSAPI.MessageQueuePayloads;

namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblUserDAO
    {
        List<UserPayload> SelectAllAdminUsers();
        String SqlSelectQuery();
        DropDownTO SelectTblUserOnDeviceId(String deviceId);
        //Added by minal 03/03/2021 for get all active user from crm reports
        List<DropDownTO> GetAllActiveUsersForDropDown();
        List<TblUserTO> SelectAllTblUser(Boolean onlyActiveYn , String deptId);
        TblUserTO SelectTblUser(Int32 idUser, SqlConnection conn, SqlTransaction tran);
        TblUserTO SelectTblUser(String userID, String password);
        Dictionary<int, string> SelectUserUsingRole(String userOrRoleIds, Boolean isUser, SqlConnection conn, SqlTransaction tran);
        Boolean IsThisUserExists(String userID, SqlConnection conn, SqlTransaction tran);
        Dictionary<int, string> SelectUserMobileNoDCTByUserIdOrRole(String userOrRoleIds, Boolean isUser, SqlConnection conn, SqlTransaction tran);
        Dictionary<int, string> SelectUserMobileNoDCTByUserIdOrRoleForDeliver(String userOrRoleIds, Boolean isUser, SqlConnection conn, SqlTransaction tran);
        Dictionary<int, string> SelectUserEmailDCTByUserIdOrRole(String userOrRoleIds, Boolean isUser, SqlConnection conn, SqlTransaction tran);
        Dictionary<int, List<string>> SelectUserMobileNoAndAlterMobileDCTByUserIdOrRole(String userOrRoleIds, Boolean isUser, SqlConnection conn, SqlTransaction tran);
        Dictionary<int, string> SelectUserDeviceRegNoDCTByUserIdOrRole(String userOrRoleIds, Boolean isUser, SqlConnection conn, SqlTransaction tran);
        List<TblUserTO> SelectAllTblUser(Int32 orgId, SqlConnection conn, SqlTransaction tran);
        List<DropDownTO> SelectAllActiveUsersForDropDown(string valConfig=null);
        List<TblUserTO> ConvertDTToList(SqlDataReader tblUserTODT);
        TblUserTO SelectUserByImeiNumber(string imeiNumber, SqlConnection conn, SqlTransaction tran);
        List<DropDownTO> SelectUsersOnUserIds(string userIds);
        DropDownTO SelectTblUser(Int32 idUser);
        List<TblUserTO> SelectAllTblUserByRoleType(Int32 roleTypeId);
        int InsertTblUser(TblUserTO tblUserTO);
        int InsertTblUser(TblUserTO tblUserTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblUserTO tblUserTO, SqlCommand cmdInsert);
        int UpdateTblUser(TblUserTO tblUserTO);
        int UpdateTblUser(TblUserTO tblUserTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblUserTO tblUserTO, SqlCommand cmdUpdate);
        int DeleteTblUser(Int32 idUser);
        int DeleteTblUser(Int32 idUser, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idUser, SqlCommand cmdDelete);
        List<DropDownTO> SelectUserListForIssue(string issueIds);

        int ActivateOrDeactivateTblUser(TblUserTO tblUserTO);
        List<DropDownTO> GetChildUserAndDepartmentListOnUserId(string userIds);
        #region Wishlist
        TblUserTO SelectTblWishlistUser(String userID, String password);

        #endregion
    }
}