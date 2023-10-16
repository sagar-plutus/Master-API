using System;
using System.Collections.Generic;
using System.Text;

namespace ODLMWebAPI.Models
{
    public class TblReceiptChargesTO
    {
        #region Declarations
        Int32 idReceiptCharges;
        Int32 brsBankStatementDtlId;
        Int32 otherChargesTypeId;
        Double amount;
        #endregion

        #region Constructor
        public TblReceiptChargesTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdReceiptCharges
        {
            get { return idReceiptCharges; }
            set { idReceiptCharges = value; }
        }
        public Int32 BrsBankStatementDtlId
        {
            get { return brsBankStatementDtlId; }
            set { brsBankStatementDtlId = value; }
        }
        public Int32 OtherChargesTypeId
        {
            get { return otherChargesTypeId; }
            set { otherChargesTypeId = value; }
        }
        public Double Amount
        {
            get { return amount; }
            set { amount = value; }
        }
        #endregion
    }
}
