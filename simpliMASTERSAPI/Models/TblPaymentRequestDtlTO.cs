using System;
using System.Collections.Generic;
using System.Text;

namespace ODLMWebAPI.Models
{
    public class TblPaymentRequestDtlTO
    {
        #region Declarations
        Int32 idPayReqDtl;
        Int32 payReqId;
        Int32 payTypeId;
        Int32 refNo;
        Int32 txnTypeId;
        Int32 statusId;
        Int32 supplierId;
        Int32 userId;
        Int32 expenseId;
        Int32 advanceId;
        Int32 departmentId;
        Int32 payBankId;
        Int32 paymentTypeId;
        Int32 payById;
        DateTime paymentDate;
        DateTime payOn;
        Double amount;
        Int64 grnId;
        String paymentNarration;
        #endregion

        #region Constructor
        public TblPaymentRequestDtlTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdPayReqDtl
        {
            get { return idPayReqDtl; }
            set { idPayReqDtl = value; }
        }
        public Int32 PayReqId
        {
            get { return payReqId; }
            set { payReqId = value; }
        }
        public Int32 PayTypeId
        {
            get { return payTypeId; }
            set { payTypeId = value; }
        }
        public Int32 RefNo
        {
            get { return refNo; }
            set { refNo = value; }
        }
        public Int32 TxnTypeId
        {
            get { return txnTypeId; }
            set { txnTypeId = value; }
        }
        public Int32 StatusId
        {
            get { return statusId; }
            set { statusId = value; }
        }
        public Int32 SupplierId
        {
            get { return supplierId; }
            set { supplierId = value; }
        }
        public Int32 UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        public Int32 ExpenseId
        {
            get { return expenseId; }
            set { expenseId = value; }
        }
        public Int32 AdvanceId
        {
            get { return advanceId; }
            set { advanceId = value; }
        }
        public Int32 DepartmentId
        {
            get { return departmentId; }
            set { departmentId = value; }
        }
        public Int32 PayBankId
        {
            get { return payBankId; }
            set { payBankId = value; }
        }
        public Int32 PaymentTypeId
        {
            get { return paymentTypeId; }
            set { paymentTypeId = value; }
        }
        public Int32 PayById
        {
            get { return payById; }
            set { payById = value; }
        }
        public DateTime PaymentDate
        {
            get { return paymentDate; }
            set { paymentDate = value; }
        }
        public DateTime PayOn
        {
            get { return payOn; }
            set { payOn = value; }
        }
        public Double Amount
        {
            get { return amount; }
            set { amount = value; }
        }
        public Int64 GrnId
        {
            get { return grnId; }
            set { grnId = value; }
        }
        public String PaymentNarration
        {
            get { return paymentNarration; }
            set { paymentNarration = value; }
        }
        #endregion
    }
}
