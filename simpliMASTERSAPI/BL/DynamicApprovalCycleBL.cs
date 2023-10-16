using System;
using System.Collections.Generic;
using ODLMWebAPI.Models;
using ODLMWebAPI.DAL;
using System.Linq;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;
using System.Data;
using simpliMASTERSAPI.TO;

namespace ODLMWebAPI.BL
{ 
    public class DynamicApprovalCycleBL : IDynamicApprovalCYcleBL
    {
       private readonly IDynamicApprovalCycleDAO iDynamicApprovalCYcleDAO;
        public DynamicApprovalCycleBL(IDynamicApprovalCycleDAO iDynamicApprovalCYcleDAO)
        {
        this.iDynamicApprovalCYcleDAO=iDynamicApprovalCYcleDAO;
        }

        #region selection
       public List<DimDynamicApprovalTO> SelectAllApprovalList(int idModule = 0, int area = 0, int userId = 0)
       {
           return iDynamicApprovalCYcleDAO.SelectAllApprovalList(idModule , area, userId);
       }

        public DataTable SelectAllList(int seqNo)
        {
            return iDynamicApprovalCYcleDAO.SelectAllList(seqNo);
        }


        /// <summary>
        /// AmolG[2020-Dec-22] 
        /// </summary>
        /// <param name="seqNo"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public DataTable SelectAllList(int seqNo, int userId)
        {
            return iDynamicApprovalCYcleDAO.SelectAllList(seqNo, userId);
        }


        public DataTable GetDetailsById(int idDetails,int idApprovalActions) {
            return iDynamicApprovalCYcleDAO.GetDetailsById(idDetails, idApprovalActions);

        }

        public List<DimApprovalActionsTO> GetActionIconList(int idApproval)
        {
            return iDynamicApprovalCYcleDAO.GetActionIconList(idApproval);
        }
        public int UpdateStatus(Dictionary<string, string> tableData, int status, int userId, int seqNo, ref string txtCommentMsg, ref Int32 OrganizationId,Int32 isApprovByDir, ref string TransactionNo,ref int withinCriteria)
        {
            return iDynamicApprovalCYcleDAO.UpdateStatus(tableData, status, userId, seqNo,ref txtCommentMsg,ref OrganizationId, isApprovByDir,ref TransactionNo,ref withinCriteria);
        }
        public int updatePurchaseRequestComments(DropDownTO purchaseTO, int userId)
        {
            return iDynamicApprovalCYcleDAO.updatePurchaseRequestComments(purchaseTO, userId);
        }
        public int updateCommercialDocComments(DropDownTO CommDocTO, int userId)
        {
            return iDynamicApprovalCYcleDAO.updateCommercialDocComments(CommDocTO, userId);
        }
        public int UpdateIsMigrationFlag(Int64 commercialDocumentId)
        {
            return iDynamicApprovalCYcleDAO.UpdateIsMigrationFlag(commercialDocumentId);
        }

        public int UpdateIsOnePageSummaryMigrationFlag(Int64 commercialDocumentId)
        {
            return iDynamicApprovalCYcleDAO.UpdateIsOnePageSummaryMigrationFlag(commercialDocumentId);
        }
        public int InsertTblPurchaseRequestStatusHistory(Dictionary<string, string> tableData, int Status, Int32 userId)
        {
            return iDynamicApprovalCYcleDAO.InsertTblPurchaseRequestStatusHistory(tableData,Status, userId);
        }
        public int InsertCommercialDocPaymentStatusHistory(Dictionary<string, string> tableData, int Status, Int32 userId)
        {
            return iDynamicApprovalCYcleDAO.InsertCommercialDocPaymentStatusHistory(tableData, Status, userId);
        }
        public string GetLatestStatus(Int64 commercialDocumentId)
        {
            return iDynamicApprovalCYcleDAO.GetLatestStatus(commercialDocumentId);
        }

        #endregion
    }
}