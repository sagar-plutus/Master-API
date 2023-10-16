using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.Models
{
    public class TblProductItemBomTO
    {
        #region declations 
        Int32 idBomTree;
        Int32 parentProdItemId;
        Int32 childProdItemId;
        Decimal qty;
        Int32 createdBy;
        DateTime createdOn;
        Int32 updatedBy;
        DateTime updatedOn;
        String sapMappedId;
        Int32 status;
        Boolean isBOMExistsInSAP;
        int isOptional;
        int modelId;
        #endregion
        #region Constructor
        public TblProductItemBomTO()
        {

        }
        #endregion
        #region get
        public int ModelId { get => modelId; set => modelId = value; }
        public int IsOptional { get => isOptional; set => isOptional = value; }
        public int IdBomTree { get => idBomTree; set => idBomTree = value; }
        public int ParentProdItemId { get => parentProdItemId; set => parentProdItemId = value; }
        public int ChildProdItemId { get => childProdItemId; set => childProdItemId = value; }
        public decimal Qty { get => qty; set => qty = value; }
        public int CreatedBy { get => createdBy; set => createdBy = value; }
        public DateTime CreatedOn { get => createdOn; set => createdOn = value; }
        public int UpdatedBy { get => updatedBy; set => updatedBy = value; }
        public DateTime UpdatedOn { get => updatedOn; set => updatedOn = value; }
        public string SapMappedId { get => sapMappedId; set => sapMappedId = value; }
        public int Status { get => status; set => status = value; }
        public Boolean IsBOMExistsInSAP { get => isBOMExistsInSAP; set => isBOMExistsInSAP = value; }
        #endregion

    }
}
