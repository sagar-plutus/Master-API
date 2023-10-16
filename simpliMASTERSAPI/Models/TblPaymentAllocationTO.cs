using System;
using System.Collections.Generic;
using System.Text;

namespace ODLMWebAPI.Models
{
    public class TblPaymentAllocationTO
    {
        #region Declarations
        Int32 idPaymentAllocation;
        Int32 createdBy;
        DateTime createdOn;
        Boolean isLatestEntry;
        Double bankAmt;
        Double fundAllocationAmt;
        #endregion

        #region Constructor
        public TblPaymentAllocationTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdPaymentAllocation
        {
            get { return idPaymentAllocation; }
            set { idPaymentAllocation = value; }
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
        public Boolean IsLatestEntry
        {
            get { return isLatestEntry; }
            set { isLatestEntry = value; }
        }
        public Double BankAmt
        {
            get { return bankAmt; }
            set { bankAmt = value; }
        }
        public Double FundAllocationAmt
        {
            get { return fundAllocationAmt; }
            set { fundAllocationAmt = value; }
        }
        #endregion
    }
}
