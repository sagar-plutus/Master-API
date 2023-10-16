using System;
using System.Collections.Generic;
using System.Text;

namespace ODLMWebAPI.Models
{
    public class TblFundDisbursedTO
    {
        #region Declarations
        Int32 idFundDisbursed;
        Int32 paymentAllocationId;
        Int32 payTypeId;
        Int32 createdBy;
        DateTime createdOn;
        Double disbursedAmt;
        Double balanceDisbursedAmt;
        Double balanceAmt;
        #endregion

        #region Constructor
        public TblFundDisbursedTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdFundDisbursed
        {
            get { return idFundDisbursed; }
            set { idFundDisbursed = value; }
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
        public Double DisbursedAmt
        {
            get { return disbursedAmt; }
            set { disbursedAmt = value; }
        }
        public Double BalanceDisbursedAmt
        {
            get { return balanceDisbursedAmt; }
            set { balanceDisbursedAmt = value; }
        }
        public Double BalanceAmt
        {
            get { return balanceAmt; }
            set { balanceAmt = value; }
        }
        #endregion
    }
}
