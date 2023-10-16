using Newtonsoft.Json;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using static ODLMWebAPI.StaticStuff.Constants;
namespace ODLMWebAPI.Models
{
    public class DimItemProdCategTO
    {
        #region Declarations
        Int32 idItemProdCat;
        Int32 isSystem;
        Int32 isActive;
        int isScrapProdItem;
        int isFixedAsset;
        String itemProdCategory;
        String itemProdCategoryDesc;
        #endregion

        #region Constructor
        public DimItemProdCategTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdItemProdCat
        {
            get { return idItemProdCat; }
            set { idItemProdCat = value; }
        }
        public Int32 IsSystem
        {
            get { return isSystem; }
            set { isSystem = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public int IsScrapProdItem
        {
            get { return isScrapProdItem; }
            set { isScrapProdItem = value; }
        }
        public int IsFixedAsset
        {
            get { return isFixedAsset; }
            set { isFixedAsset = value; }
        }
        public String ItemProdCategory
        {
            get { return itemProdCategory; }
            set { itemProdCategory = value; }
        }
        public String ItemProdCategoryDesc
        {
            get { return itemProdCategoryDesc; }
            set { itemProdCategoryDesc = value; }
        }

        public bool IsScrapProdItemb { get; set; }
        public bool IsFixedAssetb { get;  set; }
        #endregion
    }
}
