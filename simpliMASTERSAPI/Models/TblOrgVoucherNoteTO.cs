using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simpliMASTERSAPI.Models
{
    public class TblOrgVoucherNoteTO
    {
        #region Declarations
        long idIOrgVoucherNote;
        Int32 organizationId;
        long voucherTypeId;
        Int32 transactionTypeId;
        long transactionId;
        Double voucherNoteAmt;
        Double gstTaxPercentage;
        Double gstTaxAmt;
        Double tdsTaxPercentage;
        Double tdsTaxAmt;
        Double totalVoucherNoteAmt;
        long voucherId;
        Int32 statusId;
        String entityStr;
        Int32 createdBy;
        DateTime createdOn;
        Int32 updatedBy;
        DateTime updatedOn;
        String finVoucherType;
        String statusDesc;
        String firmName;
        String createdByName;
        String updatedByName;
        String transactionName;
        Int32 moduleId;
        String remark;
        String narration;
        Int32 voucherNoteReasonId;
        String moduleName;
        String reasonDesc;
        Int32 type;
        Int32 orgTypeId;
        #endregion

        #region Constructor
        public TblOrgVoucherNoteTO()
        {
        }
        #endregion

        #region GetSet

        public long IdIOrgVoucherNote { get => idIOrgVoucherNote; set => idIOrgVoucherNote = value; }
        public int OrganizationId { get => organizationId; set => organizationId = value; }
        public long VoucherTypeId { get => voucherTypeId; set => voucherTypeId = value; }
        public int TransactionTypeId { get => transactionTypeId; set => transactionTypeId = value; }
        public long TransactionId { get => transactionId; set => transactionId = value; }
        public double VoucherNoteAmt { get => voucherNoteAmt; set => voucherNoteAmt = value; }
        public long VoucherId { get => voucherId; set => voucherId = value; }
        public int StatusId { get => statusId; set => statusId = value; }
        public string EntityStr { get => entityStr; set => entityStr = value; }
        public int CreatedBy { get => createdBy; set => createdBy = value; }
        public DateTime CreatedOn { get => createdOn; set => createdOn = value; }
        public int UpdatedBy { get => updatedBy; set => updatedBy = value; }
        public DateTime UpdatedOn { get => updatedOn; set => updatedOn = value; }
        public double GstTaxPercentage { get => gstTaxPercentage; set => gstTaxPercentage = value; }
        public double GstTaxAmt { get => gstTaxAmt; set => gstTaxAmt = value; }
        public double TdsTaxPercentage { get => tdsTaxPercentage; set => tdsTaxPercentage = value; }
        public double TdsTaxAmt { get => tdsTaxAmt; set => tdsTaxAmt = value; }
        public double TotalVoucherNoteAmt { get => totalVoucherNoteAmt; set => totalVoucherNoteAmt = value; }
        public string FirmName { get => firmName; set => firmName = value; }
        public string StatusDesc { get => statusDesc; set => statusDesc = value; }
        public string FinVoucherType { get => finVoucherType; set => finVoucherType = value; }
        public string CreatedByName { get => createdByName; set => createdByName = value; }
        public string UpdatedByName { get => updatedByName; set => updatedByName = value; }
        public string TransactionName { get => transactionName; set => transactionName = value; }
        public int ModuleId { get => moduleId; set => moduleId = value; }
        public string Narration { get => narration; set => narration = value; }
        public string Remark { get => remark; set => remark = value; }
        public int VoucherNoteReasonId { get => voucherNoteReasonId; set => voucherNoteReasonId = value; }
        public string ModuleName { get => moduleName; set => moduleName = value; }
        public string ReasonDesc { get => reasonDesc; set => reasonDesc = value; }
        public int Type { get => type; set => type = value; }
        public int OrgTypeId { get => orgTypeId; set => orgTypeId = value; }
        #endregion
    }
    public class TblOrgVoucherNoteFilterTO
    {
        #region Declarations
        DateTime fromDate;
        DateTime toDate;
        String statusIdStr;
        String voucherTypeIdStr;
        Boolean skipDateFilter;
        Boolean viewPending;
        #endregion

        #region Constructor
        public TblOrgVoucherNoteFilterTO()
        {
        }
        #endregion

        #region GetSet
        public DateTime FromDate { get => fromDate; set => fromDate = value; }
        public DateTime ToDate { get => toDate; set => toDate = value; }
        public string StatusIdStr { get => statusIdStr; set => statusIdStr = value; }
        public string VoucherTypeIdStr { get => voucherTypeIdStr; set => voucherTypeIdStr = value; }
        public bool SkipDateFilter { get => skipDateFilter; set => skipDateFilter = value; }
        public bool ViewPending { get => viewPending; set => viewPending = value; }
        #endregion
    }
}
