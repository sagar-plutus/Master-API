using ODLMWebAPI.Models;
using simpliMASTERSAPI.TO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface IDynamicApprovalCYcleBL
    {
              List<DimDynamicApprovalTO> SelectAllApprovalList(int idModule = 0, int area = 0, int userId = 0);

              DataTable SelectAllList(int seqNo);

        DataTable SelectAllList(int seqNo, int userId);

        List<DimApprovalActionsTO> GetActionIconList(int idApproval);

        int UpdateStatus(Dictionary<string, string> tableData, int status, int userId, int seqNo, ref string txtCommentMsg, ref Int32 OrganizationId,Int32 isApprovByDir, ref string TransactionNo, ref int withinCriteria);
        DataTable GetDetailsById(int idDetails,int idApprovalActions);
        int updatePurchaseRequestComments(DropDownTO purchaseTO, int userId);
        int updateCommercialDocComments(DropDownTO commDocTO, int v);
        int UpdateIsMigrationFlag(Int64 commercialDocumentId);
        int InsertTblPurchaseRequestStatusHistory(Dictionary<string, string> tableData, int status, int userId);
        int InsertCommercialDocPaymentStatusHistory(Dictionary<string, string> tableData, int Status, Int32 userId);
        string GetLatestStatus(Int64 commercialDocumentId); //[2023-04-10] Samadhan added
        int UpdateIsOnePageSummaryMigrationFlag(Int64 commercialDocumentId); //[2023-05-05] Samadhan added

    }
}
