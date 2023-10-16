using System;
using System.Collections.Generic;
using System.Text;

namespace ODLMWebAPI
{
    public class TblProdItemMakeBrandTO
    {
        #region Declarations
        Int32 idProdMakeBrand;
        Int32 prodItemId;
        Int32 brandId;
        Int32 isDefaultMake;
        Int32 createdBy;
        DateTime createdOn;
        String itemBrandDesc;
        #endregion

        #region Constructor
        public TblProdItemMakeBrandTO()
        {
        }

        #endregion

        #region GetSet

        public String ItemBrandDesc
        {
            get { return itemBrandDesc; }
            set { itemBrandDesc = value; }
        }
        public Int32 IdProdMakeBrand
        {
            get { return idProdMakeBrand; }
            set { idProdMakeBrand = value; }
        }
        public Int32 ProdItemId
        {
            get { return prodItemId; }
            set { prodItemId = value; }
        }
        public Int32 BrandId
        {
            get { return brandId; }
            set { brandId = value; }
        }
        public Int32 IsDefaultMake
        {
            get { return isDefaultMake; }
            set { isDefaultMake = value; }
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
        #endregion
    }
}
