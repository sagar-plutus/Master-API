using System;
using System.Collections.Generic;
using System.Text;

namespace ODLMWebAPI.Models
{
    public class TblReceiptLinkingTO
    {
        #region Declarations
        Int32 idReceiptLinking;
        Int32 brsBankStatementDtlId;
        Int32 bookingId;
        Int32 paySchId;
        Int32 refReceiptLinkingId;
        Int32 createdBy;
        Int32 returnBy;
        Int32 lastSplitedBy;
        DateTime createdOn;
        DateTime returnOn;
        DateTime lastSplitedOn;
        Boolean isSplited;
        Boolean isReturn;
        Boolean isActive;
        Double linkedAmt;
        Double actualLinkedAmt;
        Double splitedAmt;
        #endregion

        #region Constructor
        public TblReceiptLinkingTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdReceiptLinking
        {
            get { return idReceiptLinking; }
            set { idReceiptLinking = value; }
        }
        public Int32 BrsBankStatementDtlId
        {
            get { return brsBankStatementDtlId; }
            set { brsBankStatementDtlId = value; }
        }
        public Int32 BookingId
        {
            get { return bookingId; }
            set { bookingId = value; }
        }
        public Int32 PaySchId
        {
            get { return paySchId; }
            set { paySchId = value; }
        }
        public Int32 RefReceiptLinkingId
        {
            get { return refReceiptLinkingId; }
            set { refReceiptLinkingId = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public Int32 ReturnBy
        {
            get { return returnBy; }
            set { returnBy = value; }
        }
        public Int32 LastSplitedBy
        {
            get { return lastSplitedBy; }
            set { lastSplitedBy = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public DateTime ReturnOn
        {
            get { return returnOn; }
            set { returnOn = value; }
        }
        public DateTime LastSplitedOn
        {
            get { return lastSplitedOn; }
            set { lastSplitedOn = value; }
        }
        public Boolean IsSplited
        {
            get { return isSplited; }
            set { isSplited = value; }
        }
        public Boolean IsReturn
        {
            get { return isReturn; }
            set { isReturn = value; }
        }
        public Boolean IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public Double LinkedAmt
        {
            get { return linkedAmt; }
            set { linkedAmt = value; }
        }
        public Double ActualLinkedAmt
        {
            get { return actualLinkedAmt; }
            set { actualLinkedAmt = value; }
        }
        public Double SplitedAmt
        {
            get { return splitedAmt; }
            set { splitedAmt = value; }
        }
        #endregion
    }
}
