using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.Models
{
    public class TblBrsBankStatementDtlTO
    {
        #region Declarations
        Int32 idBrsBankStatementDtl;
        Int32 brsBankStatementId;
        Int32 brsTemplateDtlId;
        DateTime brsBankStatementRecordDate;
        Boolean isReconciled;
        String brsBankStatementRecordValue;
        String referenceNo;
        string receiptNo;
        DateTime receiptDate;
        double amount;
        string narration;
        string bankName;
        string receiptByName;
        string location;
        int receiptTypeId;
        bool checkForMackingYn;
        bool checkForValueDate;
        bool checkForChequeNo;
        bool checkForBankNar;
        bool checkForCredit;
        bool checkForDebit;
        bool checkForTransLoc;
        DateTime updatedOn;
        Int32 updatedBy;
        Int32 statusId;
        Int32 rowNO;
        Int32 dealerId;
        Int32 bankLedgerId;

        #endregion

        #region Constructor
        public TblBrsBankStatementDtlTO()
        {
        }
        #endregion

        #region GetSet
        public Int32 DealerId
        {
            get { return dealerId; }
            set { dealerId = value; }
        }
        public Int32 BankLedgerId
        {
            get { return bankLedgerId; }
            set { bankLedgerId = value; }
        }
        public Int32 RowNO
        {
            get { return rowNO; }
            set { rowNO = value; }
        }
        public Int32 UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; }
        }
        public Int32 StatusId
        {
            get { return statusId; }
            set { statusId = value; }
        }
        public DateTime UpdatedOn
        {
            get { return updatedOn; }
            set { updatedOn = value;}
        }
        public bool CheckForMackingYn
        {
            get { return checkForMackingYn; }
            set { checkForMackingYn = value; }
        }
        public bool CheckForValueDate
        {
            get { return checkForValueDate; }
            set { checkForValueDate = value; }
        }
        public bool CheckForChequeNo
        {
            get { return checkForChequeNo; }
            set { checkForChequeNo = value; }
        }
        public bool CheckForBankNar
        {
            get { return checkForBankNar; }
            set { checkForBankNar = value; }
        }
        public bool CheckForCredit
        {
            get { return checkForCredit; }
            set { checkForCredit = value; }
        }
        public bool CheckForDebit
        {
            get { return checkForDebit; }
            set { checkForDebit = value; }
        }
        public bool CheckForTransLoc
        {
            get { return checkForTransLoc; }
            set { checkForTransLoc = value; }
        }
        public string ReceiptNo
        {
            get { return receiptNo; }
            set { receiptNo = value; }
        }
        public double Amount
        {
            get { return amount; }
            set { amount = value; }
        }
        public DateTime ReceiptDate
        {
            get { return receiptDate; }
            set { receiptDate = value; }
        }
        public string Narration
        {
            get { return narration; }
            set { narration = value; }
        }
        public string BankName
        {
            get { return bankName; }
            set { bankName = value; }
        }
        public string ReceiptByName
        {
            get { return receiptByName; }
            set { receiptByName = value; }
        }
        public string Location
        {
            get { return location; }
            set { location = value; }
        }
        public int ReceiptTypeId
        {
            get { return receiptTypeId; }
            set { receiptTypeId = value; }
        }
        public Int32 IdBrsBankStatementDtl
        {
            get { return idBrsBankStatementDtl; }
            set { idBrsBankStatementDtl = value; }
        }
        public Int32 BrsBankStatementId
        {
            get { return brsBankStatementId; }
            set { brsBankStatementId = value; }
        }
        public Int32 BrsTemplateDtlId
        {
            get { return brsTemplateDtlId; }
            set { brsTemplateDtlId = value; }
        }
        public DateTime BrsBankStatementRecordDate
        {
            get { return brsBankStatementRecordDate; }
            set { brsBankStatementRecordDate = value; }
        }
        public Boolean IsReconciled
        {
            get { return isReconciled; }
            set { isReconciled = value; }
        }
        public String BrsBankStatementRecordValue
        {
            get { return brsBankStatementRecordValue; }
            set { brsBankStatementRecordValue = value; }
        }
        public String ReferenceNo
        {
            get { return referenceNo; }
            set { referenceNo = value; }
        }
        #endregion
    }
}
