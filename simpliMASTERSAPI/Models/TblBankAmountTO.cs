using System;
using System.Collections.Generic;
using System.Text;

namespace ODLMWebAPI.Models
{
    public class TblBankAmountTO
    {
        #region Declarations
        Int32 idBankAmount;
        Int32 bankLedgerId;
        Int32 amountTakenBy;
        Int32 paymentAllocationId;
        Int32 createdBy;
        DateTime amountTakenOn;
        DateTime createdOn;
        Boolean isLatestEntry;
        Double bankAmt;
        string bankName;
        double enterAmt;
        double totalAmt;

        #endregion

        #region Constructor
        public TblBankAmountTO()
        {
        }

        #endregion

        #region GetSet
        public string BankName
        {
            get { return bankName; }
            set { bankName = value; }
        }
        public Double EnterAmt
        {
            get { return enterAmt; }
            set { enterAmt = value; }
        }
        public Double TotalAmt
        {
            get { return totalAmt; }
            set { totalAmt = value; }
        }
        public Int32 IdBankAmount
        {
            get { return idBankAmount; }
            set { idBankAmount = value; }
        }
        public Int32 BankLedgerId
        {
            get { return bankLedgerId; }
            set { bankLedgerId = value; }
        }
        public Int32 AmountTakenBy
        {
            get { return amountTakenBy; }
            set { amountTakenBy = value; }
        }
        public Int32 PaymentAllocationId
        {
            get { return paymentAllocationId; }
            set { paymentAllocationId = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public DateTime AmountTakenOn
        {
            get { return amountTakenOn; }
            set { amountTakenOn = value; }
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
        #endregion
    }
}
