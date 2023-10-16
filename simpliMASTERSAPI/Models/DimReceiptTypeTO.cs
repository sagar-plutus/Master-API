using System;
using System.Collections.Generic;
using System.Text;

namespace ODLMWebAPI.TO
{
    public class DimReceiptTypeTO
    {
        #region Declarations
        Int32 idReceiptType;
        Int32 sysElementId;
        Boolean isActive;
        String receiptTypeName;
        String receiptTypeDesc;
        #endregion

        #region Constructor
        public DimReceiptTypeTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdReceiptType
        {
            get { return idReceiptType; }
            set { idReceiptType = value; }
        }
        public Int32 SysElementId
        {
            get { return sysElementId; }
            set { sysElementId = value; }
        }
        public Boolean IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public String ReceiptTypeName
        {
            get { return receiptTypeName; }
            set { receiptTypeName = value; }
        }
        public String ReceiptTypeDesc
        {
            get { return receiptTypeDesc; }
            set { receiptTypeDesc = value; }
        }
        #endregion
    }
}
