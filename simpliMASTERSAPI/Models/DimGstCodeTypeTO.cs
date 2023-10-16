using Newtonsoft.Json;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using ODLMWebAPI.StaticStuff;

namespace ODLMWebAPI.Models
{
    public class DimGstCodeTypeTO
    {
        #region Declarations
        Int32 idCodeType;
        DateTime createdOn;
        String codeDesc;
        string inventoryAcctFinLedgerId;
        string costAcctFinLedgerId;
        string transfersAcctFinLedgerId;
        string varianceAcctFinLedgerId;
        string priceDifferenceAcctFinLedgerId;

        #endregion

        #region Constructor
        public DimGstCodeTypeTO()
        {
        }

        #endregion

        #region GetSet
        public string InventoryAcctFinLedgerId
        {
            get { return inventoryAcctFinLedgerId; }
            set { inventoryAcctFinLedgerId = value; }
        }
        public string CostAcctFinLedgerId
        {
            get { return costAcctFinLedgerId; }
            set { costAcctFinLedgerId = value; }
        }
        public string TransfersAcctFinLedgerId
        {
            get { return transfersAcctFinLedgerId; }
            set { transfersAcctFinLedgerId = value; }
        }
        public string VarianceAcctFinLedgerId
        {
            get { return varianceAcctFinLedgerId; }
            set { varianceAcctFinLedgerId = value; }
        }
        public string PriceDifferenceAcctFinLedgerId
        {
            get { return priceDifferenceAcctFinLedgerId; }
            set { priceDifferenceAcctFinLedgerId = value; }
        }

        public Int32 IdCodeType
        {
            get { return idCodeType; }
            set { idCodeType = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public String CodeDesc
        {
            get { return codeDesc; }
            set { codeDesc = value; }
        }
        #endregion
    }
}
