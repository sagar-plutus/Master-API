using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;

namespace ODLMWebAPI.Models
{
    public class TblFinLedgerTO
    {
        #region Declarations
        Int32 level;
        Int32 type;
        Int32 currencyId;
        Int32 confidential;
        Int32 finAccountTypeId;
        String finAccountTypeName;
        Int32 controlAct;
        Int32 cashAct;
        Int32 blockManul;
        Int32 reval;
        Int32 cashFlow;
        Int64 finProjectId;
        String finProjectName;
        Int32 isActive;
        Int32 createdBy;
        String createdByUserName;
        Int32 updatedBy;
        String updatedByUserName;
        DateTime createdOn;
        DateTime updatedOn;
        Int64 idFinLedger;
        Int64 parentFinLedgerId;
        String ledgerCode;
        String ledgerName;
        String parentLedgerCode;
        String currencyCode;
        String finAccountTypeCode;
        int withholdTaxId;
        Int32 statusId; //Added Dhananjay [26-12-2020]
        String segmentCode; //Added Dhananjay [23-06-2021]
        #endregion

        #region Constructor
        public TblFinLedgerTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 Level
        {
            get { return level; }
            set { level = value; }
        }
        public Int32 Type
        {
            get { return type; }
            set { type = value; }
        }
        public Int32 CurrencyId
        {
            get { return currencyId; }
            set { currencyId = value; }
        }
        public Int32 Confidential
        {
            get { return confidential; }
            set { confidential = value; }
        }
        public Int32 FinAccountTypeId
        {
            get { return finAccountTypeId; }
            set { finAccountTypeId = value; }
        }
        public Int32 ControlAct
        {
            get { return controlAct; }
            set { controlAct = value; }
        }
        public Int32 CashAct
        {
            get { return cashAct; }
            set { cashAct = value; }
        }
        public Int32 BlockManul
        {
            get { return blockManul; }
            set { blockManul = value; }
        }
        public Int32 Reval
        {
            get { return reval; }
            set { reval = value; }
        }
        public Int32 CashFlow
        {
            get { return cashFlow; }
            set { cashFlow = value; }
        }
        public Int64 FinProjectId
        {
            get { return finProjectId; }
            set { finProjectId = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public Int32 UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public DateTime UpdatedOn
        {
            get { return updatedOn; }
            set { updatedOn = value; }
        }
        public Int64 IdFinLedger
        {
            get { return idFinLedger; }
            set { idFinLedger = value; }
        }
        public Int64 ParentFinLedgerId
        {
            get { return parentFinLedgerId; }
            set { parentFinLedgerId = value; }
        }
        public String LedgerCode
        {
            get { return ledgerCode; }
            set { ledgerCode = value; }
        }
        public String LedgerName
        {
            get { return ledgerName; }
            set { ledgerName = value; }
        }
        public String ParentLedgerCode
        {
            get { return parentLedgerCode; }
            set { parentLedgerCode = value; }
        }

        public string CreatedByUserName { get => createdByUserName; set => createdByUserName = value; }
        public string UpdatedByUserName { get => updatedByUserName; set => updatedByUserName = value; }
        public string CurrencyCode { get => currencyCode; set => currencyCode = value; }
        public string FinAccountTypeName { get => finAccountTypeName; set => finAccountTypeName = value; }
        public string FinProjectName { get => finProjectName; set => finProjectName = value; }
        public string FinAccountTypeCode { get => finAccountTypeCode; set => finAccountTypeCode = value; }
        public int WithholdTaxId { get => withholdTaxId; set => withholdTaxId = value; }
        public Int32 StatusId { get => statusId; set => statusId = value; } //Added Dhananjay [26-12-2020]
        public string SegmentCode { get => segmentCode; set => segmentCode = value; } //Added Dhananjay [23-06-2021]
        #endregion
    }
}
