using System;
using System.Collections.Generic;
using System.Text;

namespace ODLMWebAPI
{
    public class TblProdItemMakeBrandExtTO
    {
        #region Declarations
        DateTime createdOn;
        int idProdItemMakeBrandExt;
        int prodItemId;
        int itemMakeId;
        int itemBrandId;
        int isDefaultMake;
        int createdBy;
        string rackNo;
        string xBinLocation;
        string yBinLocation;
        string catLogNo;
        #endregion

        #region Constructor
        public TblProdItemMakeBrandExtTO()
        {
        }

        #endregion

        #region GetSet
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public int IdProdItemMakeBrandExt
        {
            get { return idProdItemMakeBrandExt; }
            set { idProdItemMakeBrandExt = value; }
        }
        public int ProdItemId
        {
            get { return prodItemId; }
            set { prodItemId = value; }
        }
        public int ItemMakeId
        {
            get { return itemMakeId; }
            set { itemMakeId = value; }
        }
        public int ItemBrandId
        {
            get { return itemBrandId; }
            set { itemBrandId = value; }
        }
        public int IsDefaultMake
        {
            get { return isDefaultMake; }
            set { isDefaultMake = value; }
        }
        public int CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public string RackNo
        {
            get { return rackNo; }
            set { rackNo = value; }
        }
        public string XBinLocation
        {
            get { return xBinLocation; }
            set { xBinLocation = value; }
        }
        public string YBinLocation
        {
            get { return yBinLocation; }
            set { yBinLocation = value; }
        }
        public string CatLogNo
        {
            get { return catLogNo; }
            set { catLogNo = value; }
        }
        #endregion
    }
}
