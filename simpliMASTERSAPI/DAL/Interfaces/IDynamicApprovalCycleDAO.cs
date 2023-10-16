using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
using simpliMASTERSAPI.TO;

namespace ODLMWebAPI.DAL.Interfaces
{
    public interface IDynamicApprovalCycleDAO
    { 
       
        List<DimDynamicApprovalTO> SelectAllApprovalList(int idModule = 0, int area = 0, int userId = 0);
        DataTable SelectAllList(int seqNo);

        DataTable SelectAllList(int seqNo, int userId);


        List<DimApprovalActionsTO> GetActionIconList(int idApproval);

        int UpdateStatus(Dictionary<string, string> tableData, int status, int userId, int seqNo, ref string txtCommentMsg, ref Int32 OrganizationId, Int32 isApprovByDir,ref string TransactionNo,ref int withinCriteria);
        DataTable GetDetailsById(int idDetails,int idApprovalActions);
        int updatePurchaseRequestComments(DropDownTO purchaseTO, int userId);
        int updateCommercialDocComments(DropDownTO CommDocTO, int userId);
        int UpdateIsMigrationFlag(Int64 commercialDocumentId);
        int InsertTblPurchaseRequestStatusHistory(Dictionary<string, string> tableData, int Status, Int32 userId);
        int InsertCommercialDocPaymentStatusHistory(Dictionary<string, string> tableData, int Status, Int32 userId);
        string GetLatestStatus(Int64 commercialDocumentId);

        int UpdateIsOnePageSummaryMigrationFlag(Int64 commercialDocumentId);
        
    }
}