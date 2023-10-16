using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ODLMWebAPI.StaticStuff.Constants;

namespace ODLMWebAPI.Models
{
    public class TblPurchaseUnloadingDtlTO
    {
        #region Declarations
        Int32 idPurchaseUnloadingDtl;
        Int32 purchaseWeighingStageId;
         Int32 weighingStageId;
        Int32 prodItemId;
        Int32 createdBy;
        DateTime createdOn;
        Double qtyMT;
        string itemName;
        Int32 purchaseScheduleSummaryId;

        Int32 isConfirmUnloading;
        #endregion

        #region Constructor
        public TblPurchaseUnloadingDtlTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdPurchaseUnloadingDtl
        {
            get { return idPurchaseUnloadingDtl; }
            set { idPurchaseUnloadingDtl = value; }
        }
        public Int32 PurchaseWeighingStageId
        {
            get { return purchaseWeighingStageId; }
            set { purchaseWeighingStageId = value; }
        }

        public Int32 WeighingStageId
        {
            get { return weighingStageId; }
            set { weighingStageId = value; }
        }
        public Int32 ProdItemId
        {
            get { return prodItemId; }
            set { prodItemId = value; }
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
        public Double QtyMT
        {
            get { return qtyMT; }
            set { qtyMT = value; }
        }

        public string ItemName
        {
            get { return itemName; }
            set { itemName = value; }
        }
        
        public Int32 IsConfirmUnloading
        {
            get { return isConfirmUnloading; }
            set { isConfirmUnloading = value; }
        }
        
          public Int32 PurchaseScheduleSummaryId
        {
            get { return purchaseScheduleSummaryId; }
            set { purchaseScheduleSummaryId = value; }
        }

        #endregion
    }
}
