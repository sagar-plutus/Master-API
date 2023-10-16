using ODLMWebAPI.Models;
using simpliMASTERSAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ICommon
    {
        System.DateTime SelectServerDateTime();
        DateTime ServerDateTime { get; }
        List<DynamicReportTO> SqlDataReaderToExpando(SqlDataReader reader);
        Boolean UserExistInCommaSeparetedStr(String commaSepatedStr, Int32 userId);
        IEnumerable<dynamic> GetDynamicSqlData(string connectionstring, string sql, params SqlParameter[] commandParmeter);
        void CalculateBookingsOpeningBalance(String RequestOriginString);
        void PostCancelNotConfirmLoadings(String RequestOriginString);
        void PostAutoResetAndDeleteAlerts(String RequestOriginString);
         int CheckLogOutEntry(int loginId);
           int IsUserDeactivate(int userId);
        string SelectApKLoginArray(int userId);
        dynamic selectUserReportingListOnUserId(int userId);
        string SelectAllLoginEntries(Int32 IdUser);
        dynamic AddConsolidationByPurchaseRequest(TblPurchaseRequestTo tblPurchaseRequestTo);
        dynamic StockTransfer(TblPurchaseRequestTo tblPurchaseRequestTo);
        //static DataTable ToDataTable<T>(List<T> items);
        dynamic GetProFlowUserList(ProFlowSettingTO proFlowSettingTO);
        dynamic CreateProFlowUser(ProFlowUserTO proFlowUserTO, ProFlowSettingTO proFlowSettingTO);
        dynamic UpdateProFlowUser(UpdateProFlowUserTO updateProFlowUserTO, ProFlowSettingTO proFlowSettingTO);
        dynamic SendWhatsAppMsg(String WhatsAppMsgRequestTOStr, String Url, String WhatsAppMsgRequestHeaderStr);
        dynamic SendWhatsAppMsgWithFile(String WhatsAppMsgRequestTOStr, String Url, String WhatsAppMsgRequestHeaderStr);
    }
}
