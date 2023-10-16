using System;
using System.Collections.Generic;
using System.Text;

namespace ODLMWebAPI.Models
{
    public class TblPaymentRequestTO
    {
        #region Declarations
        Int32 idPayReq;
        Int32 paymentAllocationId;
        Int32 payTypeId;
        Int32 refNo;
        Int32 txnTypeId;
        Int32 createdBy;
        DateTime createdOn;
        Double amount;
        #endregion

        #region Constructor
        public TblPaymentRequestTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdPayReq
        {
            get { return idPayReq; }
            set { idPayReq = value; }
        }
        public Int32 PaymentAllocationId
        {
            get { return paymentAllocationId; }
            set { paymentAllocationId = value; }
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
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public Double Amount
        {
            get { return amount; }
            set { amount = value; }
        }
        #endregion
    }
}
