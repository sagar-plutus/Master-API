using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using ODLMWebAPI.StaticStuff;

namespace ODLMWebAPI.Models
{
    public class TblProductItemTO
    {
        #region Declarations
        Int32 itemClassId;
        Int32 idProdItem;
        Int32 prodClassId;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        String itemName;
        String itemDesc;
        String remark;
        Int32 isActive;
        Int32 weightMeasureUnitId;
        Int32 mappedWeightMeasureUnitId;
        Int32 conversionUnitOfMeasure;
        Int32 mappedConversionUnitOfMeasure;
        Double conversionFactor;
        Int32 isStockRequire;
        Int32 totalCount;
        Int32 searchAllCount;
        Int32 rowNumber;
        String prodClassDisplayName;
        Int32 isParity;
        Double basePrice;
        Int32 codeTypeId;                   //Priyanka [16-05-2018] : Added for service tax type.
        Int32 isBaseItemForRate;
        Int32 isNonCommercialItem;
        Int32 isDisplaySequenceNo;
        String category;
        String subCategory;
        String specification;
        String weightMeasurUnitDesc;
        Int32 categoryID;
        Int32 subCategoryID;
        Int32 specificationID;
        Int32 itemProdCatID;
        Boolean isConsumable;         //Harshala added
        Boolean isFixedAsset;         //Harshala added
        Int32 bomTypeId;   //Aditee added
        Int32 refProdItemId;   //Aditee added
        Int32 revisionNo;   //Aditee added
        Int32 isHavingNewRev;   //Aditee added
        Int32 status;   //Aditee added

        Boolean isNonListed; //Reshma For Non Listed Item
        Boolean isConvertNonListedToListed; //Reshma For NonListed Item
        Double scrapValuation;//Reshma For Scrap 
        int scrapStoreLocationId;//Reshma For Scrap
        Boolean isDailyScrapReq;//Reshma For Scrap
        Int32 conversionUnitOfMeasureId;// Aditee
        //General 
        Int32 priceListId;                  //Priyanka [24-04-2019]
        Int32 uOMGroupId;
        Int32 itemCategoryId;
        Int32 manageItemById;
        String additionalIdent;
        Int32 isGSTApplicable;
        Int32 manufacturerId;
        Int32 shippingTypeId;
        Int32 materialTypeId;


        Int32 isTestCertificateCompulsary;

        Int32 isAllocationApplicable;
        string uOMGroupName;
        Double hSNCode;
        Int32 mappedSalesUOMId;
        Double sapHSNCode;
        Int32 isInventoryItem;
        Int32 isSalesItem;
        Int32 isPurchaseItem;
        Int32 warrantyTemplateId;
        Int32 mgmtMethodId;

        String mappedUOMGroupId;
        Int32 taxCategoryId;
        //Priyanka [20-06-2019]
        Double sACCode;
        Double sapSACCode;
        //Sales
        Int32 salesUOMId;
        Int32 itemPerSalesUnit;
        Double salesQtyPerPkg;
        Double salesLength;
        Double salesWidth;
        Double salesHeight;
        Int32 salesVolumeId;
        Double salesWeight;

        Int32 parentProdItemId;
        Int32 idBomTree;
        Decimal qty;

        //Inventory
        Int32 gLAccId;
        Int32 inventUOMId;
        Double inventWeight;
        Int32 reqPurchaseUOMId;
        Double inventMinimum;
        Int32 valuationId;
        Double itemCost;

        //Planning data
        Int32 planningId;
        Int32 procurementId;
        Int32 compWareHouseId;
        Double minOrderQty;
        String leadTime;
        String toleranceDays;
        Double orderMultiple;

        //Production
        Int32 issueId;

        //SAP Item Group ID
        string sapItemGroupId;
        int mappedShippingTypeId;
        int mappedManufacturerId;
        string warrantyTemplateName;
        int mappedLocationId;

        Int32 locationId;
        String locationDesc;

        long baseProdItemId;

        Int32 itemMakeId;
        Int32 itemBrandId;
        String catLogNo;
        String supItemCode;
        Int32 isDefaultMake;
        Int32 isImportedItem;
        String makeSeries;

        Int32 gstCodeId;

        TblPurchaseItemMasterTO tblPurchaseItemMasterTO;
        Int32 isProperSAPItem;                              //Priyanka [02-08-2019]
        String baseItemName;

        //Priyanka H [12/09/2019] 
        Int32 gstCodeTypeId;
        Double gstTaxPct;
        String gstCodeNumber;
        String gstCodeDesc;
        String gstSapHsnCode;
        Boolean isUpdateTblProdItemData;        //Harshala added to update only when IsConsumable checkbox is checked/unchecked

        string manufacturer;
        string priceListName;
        string uOM;
        string cUOM;
        string gUOM;
        string warrenty;
        string sUOM;
        string iUOM;
        string shippingType;
        string storeLocation;
        //chetan[2020-10-01] added 
        int orignalProdItemId;
        Boolean isHaveScrapProdItem;
        Int64 finYearExpLedgerId;
        string finYearExpLedgerName;
        string finYearExpLedgerCode;

        //Fixed Asset  //Reshma
        Int32 assetClassId;
        Int32 assetStoreLocationId;
        string mapSapAssetClassId;
        string mappedSapAssetLocationId;
        Boolean isScrapItem;
        Boolean isManageInventory;

        String rackNo;
        String xBinLocation;
        String yBinLocation;
        String prodClassType;
        int itemProdCatId;
        int isAddItemfrmGrpSubGrp;
        double itemGrnNlcAmt;
        //List<TblProdItemMakeBrandTO> prodItemMakeBrandTOList;

        #endregion

        #region Constructor
        public TblProductItemTO()
        {
        }

        #endregion

        #region GetSet

        //public List<TblProdItemMakeBrandTO> ProdItemMakeBrandTOList
        //{
        //    get
        //    {
        //        return prodItemMakeBrandTOList;
        //    }

        //    set
        //    {
        //        prodItemMakeBrandTOList = value;
        //    }
        //}
        public int IsAddItemfrmGrpSubGrp
        {
            get { return isAddItemfrmGrpSubGrp; }
            set { isAddItemfrmGrpSubGrp = value; }
        }

        public Int32 ItemClassId
        {
            get { return itemClassId; }
            set { itemClassId = value; }
        }

        public string ProdClassType
        {
            get { return prodClassType; }
            set { prodClassType = value; }
        }
        public int ItemProdCatId
        {
            get { return itemProdCatId; }
            set { itemProdCatId = value; }
        }
        public Boolean IsScrapItem
        {
            get { return isScrapItem; }
            set { isScrapItem = value; }
        }
        public string MappedSapAssetLocationId
        {
            get { return mappedSapAssetLocationId; }
            set { mappedSapAssetLocationId = value; }
        }
        public string MapSapAssetClassId
        {
            get { return mapSapAssetClassId; }
            set { mapSapAssetClassId = value; }
        }
        public Int32 AssetClassId
        {
            get { return assetClassId; }
            set { assetClassId = value; }
        }
        public Int32 AssetStoreLocationId
        {
            get { return assetStoreLocationId; }
            set { assetStoreLocationId = value; }
        }

        public Double ScrapValuation
        {
            get { return scrapValuation; }
            set { scrapValuation = value; }
        }
        public Boolean IsDailyScrapReq
        {
            get { return isDailyScrapReq; }
            set { isDailyScrapReq = value; }
        }
        public Boolean IsHaveScrapProdItem
        {
            get { return isHaveScrapProdItem; }
            set { isHaveScrapProdItem = value; }
        }

        public Int32 ScrapStoreLocationId
        {
            get { return scrapStoreLocationId; }
            set { scrapStoreLocationId = value; }
        }
        public Int32 OrignalProdItemId
        {
            get { return orignalProdItemId; }
            set { orignalProdItemId = value; }
        }
        public String gstDisplayCode { get; set; }//Reshma[10-09-2020]
        public Int32 IdProdItem
        {
            get { return idProdItem; }
            set { idProdItem = value; }
        }
        public Int32 ProdClassId
        {
            get { return prodClassId; }
            set { prodClassId = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }


        public Int32 IsTestCertificateCompulsary
        {
            get { return isTestCertificateCompulsary; }
            set { isTestCertificateCompulsary = value; }
        }


        public Int32 UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public DateTime UpdatedOn
        {
            get { return updatedOn; }
            set { updatedOn = value; }
        }
        public String ItemName
        {
            get { return itemName; }
            set { itemName = value; }
        }
        public String ItemDesc
        {
            get { return itemDesc; }
            set { itemDesc = value; }
        }
        public String Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        public Int32 WeightMeasureUnitId
        {
            get { return weightMeasureUnitId; }
            set { weightMeasureUnitId = value; }
        }

        public Int32 ConversionUnitOfMeasure
        {
            get { return conversionUnitOfMeasure; }
            set { conversionUnitOfMeasure = value; }
        }

        public Double ConversionFactor
        {
            get { return conversionFactor; }
            set { conversionFactor = value; }
        }

        public int IsActive { get => isActive; set => isActive = value; }

        public Int32 IsStockRequire
        {
            get { return isStockRequire; }
            set { isStockRequire = value; }
        }
        public Int32 IsBaseItemForRate
        {
            get { return isBaseItemForRate; }
            set { isBaseItemForRate = value; }
        }
        public Int32 IsNonCommercialItem
        {
            get { return isNonCommercialItem; }
            set { isNonCommercialItem = value; }
        }
        public Int32 IsDisplaySequenceNo
        {
            get { return isDisplaySequenceNo; }
            set { isDisplaySequenceNo = value; }
        }
        public Int32 TotalCount
        {
            get { return totalCount; }
            set { totalCount = value; }
        }
        public Int32 SearchAllCount
        {
            get { return searchAllCount; }
            set { searchAllCount = value; }
        }

        public Int32 RowNumber
        {
            get { return rowNumber; }
            set { rowNumber = value; }
        }
        public string ProdClassDisplayName { get => prodClassDisplayName; set => prodClassDisplayName = value; }
        public int IsParity { get => isParity; set => isParity = value; }
        public int CodeTypeId { get => codeTypeId; set => codeTypeId = value; }
        public double BasePrice { get => basePrice; set => basePrice = value; }
        public string Category { get => category; set => category = value; }
        public string SubCategory { get => subCategory; set => subCategory = value; }
        public string Specification { get => specification; set => specification = value; }
        public string WeightMeasurUnitDesc { get => weightMeasurUnitDesc; set => weightMeasurUnitDesc = value; }


        public int PriceListId { get => priceListId; set => priceListId = value; }
        public int UOMGroupId { get => uOMGroupId; set => uOMGroupId = value; }
        public int ItemCategoryId { get => itemCategoryId; set => itemCategoryId = value; }
        public int ManageItemById { get => manageItemById; set => manageItemById = value; }
        public string AdditionalIdent { get => additionalIdent; set => additionalIdent = value; }
        public int IsGSTApplicable { get => isGSTApplicable; set => isGSTApplicable = value; }
        public int ManufacturerId { get => manufacturerId; set => manufacturerId = value; }
        public int ShippingTypeId { get => shippingTypeId; set => shippingTypeId = value; }
        public int MaterialTypeId { get => materialTypeId; set => materialTypeId = value; }
        public double HSNCode { get => hSNCode; set => hSNCode = value; }
        public int SalesUOMId { get => salesUOMId; set => salesUOMId = value; }
        public int ItemPerSalesUnit { get => itemPerSalesUnit; set => itemPerSalesUnit = value; }
        public double SalesQtyPerPkg { get => salesQtyPerPkg; set => salesQtyPerPkg = value; }
        public double SalesLength { get => salesLength; set => salesLength = value; }
        public double SalesWidth { get => salesWidth; set => salesWidth = value; }
        public double SalesHeight { get => salesHeight; set => salesHeight = value; }
        public int SalesVolumeId { get => salesVolumeId; set => salesVolumeId = value; }
        public double SalesWeight { get => salesWeight; set => salesWeight = value; }
        public int GLAccId { get => gLAccId; set => gLAccId = value; }
        public int InventUOMId { get => inventUOMId; set => inventUOMId = value; }
        public double InventWeight { get => inventWeight; set => inventWeight = value; }
        public int ReqPurchaseUOMId { get => reqPurchaseUOMId; set => reqPurchaseUOMId = value; }
        public double InventMinimum { get => inventMinimum; set => inventMinimum = value; }
        public int ValuationId { get => valuationId; set => valuationId = value; }
        public double ItemCost { get => itemCost; set => itemCost = value; }
        public int PlanningId { get => planningId; set => planningId = value; }
        public int ProcurementId { get => procurementId; set => procurementId = value; }
        public int CompWareHouseId { get => compWareHouseId; set => compWareHouseId = value; }
        public double MinOrderQty { get => minOrderQty; set => minOrderQty = value; }
        public string LeadTime { get => leadTime; set => leadTime = value; }
        public string ToleranceDays { get => toleranceDays; set => toleranceDays = value; }
        public int IssueId { get => issueId; set => issueId = value; }
        public int IsInventoryItem { get => isInventoryItem; set => isInventoryItem = value; }
        public int IsSalesItem { get => isSalesItem; set => isSalesItem = value; }
        public int IsPurchaseItem { get => isPurchaseItem; set => isPurchaseItem = value; }
        public int WarrantyTemplateId { get => warrantyTemplateId; set => warrantyTemplateId = value; }
        public int MgmtMethodId { get => mgmtMethodId; set => mgmtMethodId = value; }
        public string SapItemGroupId { get => sapItemGroupId; set => sapItemGroupId = value; }
        public int MappedShippingTypeId { get => mappedShippingTypeId; set => mappedShippingTypeId = value; }
        public int MappedManufacturerId { get => mappedManufacturerId; set => mappedManufacturerId = value; }
        public string WarrantyTemplateName { get => warrantyTemplateName; set => warrantyTemplateName = value; }
        public string MappedUOMGroupId { get => mappedUOMGroupId; set => mappedUOMGroupId = value; }
        public int MappedLocationId { get => mappedLocationId; set => mappedLocationId = value; }
        public int LocationId { get => locationId; set => locationId = value; }
        public string LocationDesc { get => locationDesc; set => locationDesc = value; }


        /// <summary>
        /// Sanjay [13-JUNE-2019] Make Item Concept is introduced
        /// </summary>
        public long BaseProdItemId { get => baseProdItemId; set => baseProdItemId = value; }
        public int ItemMakeId { get => itemMakeId; set => itemMakeId = value; }
        public int ItemBrandId { get => itemBrandId; set => itemBrandId = value; }
        public string CatLogNo { get => catLogNo; set => catLogNo = value; }
        public string SupItemCode { get => supItemCode; set => supItemCode = value; }
        public int IsDefaultMake { get => isDefaultMake; set => isDefaultMake = value; }
        public int IsImportedItem { get => isImportedItem; set => isImportedItem = value; }
        public string MakeSeries { get => makeSeries; set => makeSeries = value; }
        public double SapHSNCode { get => sapHSNCode; set => sapHSNCode = value; }

        public double SapSACCode { get => sapSACCode; set => sapSACCode = value; }
        public int TaxCategoryId { get => taxCategoryId; set => taxCategoryId = value; }
        public double SACCode { get => sACCode; set => sACCode = value; }
        public int MappedWeightMeasureUnitId { get => mappedWeightMeasureUnitId; set => mappedWeightMeasureUnitId = value; }
        public int MappedConversionUnitOfMeasure { get => mappedConversionUnitOfMeasure; set => mappedConversionUnitOfMeasure = value; }
        public int GstCodeId { get => gstCodeId; set => gstCodeId = value; }
        public TblPurchaseItemMasterTO TblPurchaseItemMasterTO { get => tblPurchaseItemMasterTO; set => tblPurchaseItemMasterTO = value; }
        public int IsProperSAPItem { get => isProperSAPItem; set => isProperSAPItem = value; }
        public string BaseItemName { get => baseItemName; set => baseItemName = value; }
        public double OrderMultiple { get => orderMultiple; set => orderMultiple = value; }
        public int GstCodeTypeId { get => gstCodeTypeId; set => gstCodeTypeId = value; }
        public double GstTaxPct { get => gstTaxPct; set => gstTaxPct = value; }
        public string GstCodeNumber { get => gstCodeNumber; set => gstCodeNumber = value; }
        public string GstCodeDesc { get => gstCodeDesc; set => gstCodeDesc = value; }
        public string GstSapHsnCode { get => gstSapHsnCode; set => gstSapHsnCode = value; }
        public int CategoryID { get => categoryID; set => categoryID = value; }
        public int SubCategoryID { get => subCategoryID; set => subCategoryID = value; }
        public int SpecificationID { get => specificationID; set => specificationID = value; }
        public int ItemProdCatID { get => itemProdCatID; set => itemProdCatID = value; }
        public string Warrenty { get => warrenty; set => warrenty = value; }
        public string SUOM { get => sUOM; set => sUOM = value; }
        public string IUOM { get => iUOM; set => iUOM = value; }
        public string ShippingType { get => shippingType; set => shippingType = value; }
        public string StoreLocation { get => storeLocation; set => storeLocation = value; }
        public string Manufacturer { get => manufacturer; set => manufacturer = value; }
        public string PriceListName { get => priceListName; set => priceListName = value; }
        public string UOM { get => uOM; set => uOM = value; }
        public string CUOM { get => cUOM; set => cUOM = value; }
        public string GUOM { get => gUOM; set => gUOM = value; }
        public int IsAllocationApplicable { get => isAllocationApplicable; set => isAllocationApplicable = value; }
        public Boolean IsConsumable { get => isConsumable; set => isConsumable = value; }
        public Boolean IsFixedAsset { get => isFixedAsset; set => isFixedAsset = value; }
        public Boolean IsNonListed { get => isNonListed; set => isNonListed = value; }
        public Boolean IsConvertNonListedToListed { get => isConvertNonListedToListed; set => isConvertNonListedToListed = value; }
        public bool IsUpdateTblProdItemData { get => isUpdateTblProdItemData; set => isUpdateTblProdItemData = value; }
        public int BomTypeId { get => bomTypeId; set => bomTypeId = value; }
        public int RefProdItemId { get => refProdItemId; set => refProdItemId = value; }
        public int RevisionNo { get => revisionNo; set => revisionNo = value; }
        public int IsHavingNewRev { get => isHavingNewRev; set => isHavingNewRev = value; }
        public int Status { get => status; set => status = value; }
        public int ParentProdItemId { get => parentProdItemId; set => parentProdItemId = value; }
        public int IdBomTree { get => idBomTree; set => idBomTree = value; }
        public decimal Qty { get => qty; set => qty = value; }
        public Int64 FinYearExpLedgerId { get => finYearExpLedgerId; set => finYearExpLedgerId = value; }
        public string FinYearExpLedgerName { get => finYearExpLedgerName; set => finYearExpLedgerName = value; }
        public string FinYearExpLedgerCode { get => finYearExpLedgerCode; set => finYearExpLedgerCode = value; }
        public Int32 MappedSalesUOMId { get => mappedSalesUOMId; set => mappedSalesUOMId = value; }
        public string UOMGroupName { get => uOMGroupName; set => uOMGroupName = value; }

        /// <summary>
        /// AmolG[2020-Dec-16] For Warehouse wise inventory
        /// </summary>
        public bool IsManageInventory { get => isManageInventory; set => isManageInventory = value; }

        public String YBinLocation { get => yBinLocation; set => yBinLocation = value; }
        public string RackNo { get => rackNo; set => rackNo = value; }
        public int ConversionUnitOfMeasureId { get => conversionUnitOfMeasureId; set => conversionUnitOfMeasureId = value; }
        public string XBinLocation { get => xBinLocation; set => xBinLocation = value; }
        public Int32 ProdQty { get; set; }
        public int IsOptional { get; set; }
        public int IsHavingChild { get; set; }
        public int MappedSapItemClassId { get; set; }
        public int EnquiryTypeId { get; set; }
        public double NlcCost { get; set; }
        public string ItemClassName { get; set; }
        public double TotalNLC { get; set; }
        public double ItemGrnNlcAmt { get => itemGrnNlcAmt; set => itemGrnNlcAmt = value; }
        public double DeliveryPeriodInDays { get; set; }
        public string ItemMakeDesc { get; set; }
        public string ItemBrandDesc { get; set; }
        public string  MfgCatlogNo { get; set; }
        public string Supplier { get; set; }
        public double BasicRate { get; set; }
        public double Freight { get; set; }
        public double PF { get; set; }
        public double Discount { get; set; }
        public double CurrencyRate { get; set; }
        public int CurrencyId { get; set; }
        public string IsTestCertificateCompulsaryStr { get; set; }
        public string Currency { get; set; }
        public string IsImportedItemStr { get; set; }
        public string IsDefaultMakeStr { get; set; }
        public string ManageItemByStr { get; set; }
        public string MaterialType { get; set; }
        public string ItemCategory { get; set; }
        public string TaxCategory { get; set; }
        public int IdProcess { get; set; }

        #endregion

        public TblProductItemTO DeepCopy()
        {
            TblProductItemTO other = (TblProductItemTO)this.MemberwiseClone();
            return other;
        }
    }

    /// <summary>
    /// AmolG[2020-Dec-16] For Warehouse wise inventory
    /// </summary>
    public class WareHouseWiseItemDtlsTO
    {
        #region Declarations
        String itemCode;
        String itemName;
        String whsCode;
        String whsName;
        Double inStock;
        Double commited;
        Double ordered;
        Double itemCost;
        Double totalInStock;
        Double totalCommited;
        Double totalOrdered;
        Double available;
        Double totalAvailable;
        Double minLevel;
        Double minOrdrQty;
        Double ordrMulti;
        string manSerNum;
        string invntryUom;
        Double itemNLC;
        Double actualCommitedStock;
        Boolean isManageBySerialNo;
        Double virtualStock; //chetan[2020-11-05] added for display virtual Stock
        Double minInventory;
        Double maxInventory;
        Double minOrder;
        String rack;
        String row;
        String column;
        #endregion

        #region Constructor
        public WareHouseWiseItemDtlsTO()
        {
        }
        #endregion

        #region GetSet
        public string ItemCode { get => itemCode; set => itemCode = value; }
        public string ManSerNum { get => manSerNum; set => manSerNum = value; }
        public string ItemName { get => itemName; set => itemName = value; }
        public string WhsCode { get => whsCode; set => whsCode = value; }
        public string WhsName { get => whsName; set => whsName = value; }
        public double InStock { get => inStock; set => inStock = value; }
        public double Commited { get => commited; set => commited = value; }
        public double Ordered { get => ordered; set => ordered = value; }
        public double ItemCost { get => itemCost; set => itemCost = value; }
        public double TotalInStock { get => totalInStock; set => totalInStock = value; }
        public double TotalCommited { get => totalCommited; set => totalCommited = value; }
        public double TotalOrdered { get => totalOrdered; set => totalOrdered = value; }
        public double Available { get => available; set => available = value; }
        public double TotalAvailable { get => totalAvailable; set => totalAvailable = value; }
        public double MinLevel { get => minLevel; set => minLevel = value; }
        public double MinOrdrQty { get => minOrdrQty; set => minOrdrQty = value; }
        public double OrdrMulti { get => ordrMulti; set => ordrMulti = value; }
        public string InvntryUom { get => invntryUom; set => invntryUom = value; }
        public double ItemNLC { get => itemNLC; set => itemNLC = value; }
        public double ActualCommitedStock { get => actualCommitedStock; set => actualCommitedStock = value; }
        public double VirtualStock { get => virtualStock; set => virtualStock = value; }


        public bool IsManageBySerialNo { get => isManageBySerialNo; set => isManageBySerialNo = value; }

        /// <summary>
        /// AmolG[2020-Dec-16] For Warehouse wise inventory
        /// </summary>
        public double MinInventory { get => minInventory; set => minInventory = value; }

        /// <summary>
        /// AmolG[2020-Dec-16] For Warehouse wise inventory
        /// </summary>
        public double MaxInventory { get => maxInventory; set => maxInventory = value; }

        /// <summary>
        /// AmolG[2020-Dec-16] For Warehouse wise inventory
        /// </summary>
        public double MinOrder { get => minOrder; set => minOrder = value; }
        public string Rack { get => rack; set => rack = value; }
        public string Row { get => row; set => row = value; }
        public string Column { get => column; set => column = value; }
        #endregion
    }
    public class StoresLocationTO
    {
        #region Declarations
        
       
        #endregion

        #region Constructor
        public StoresLocationTO()
        {
        }
        #endregion

        #region GetSet
       
        public int IdLocation { get; set; }
        public string LocationDesc { get; set; }
        public int ProdItemId { get; set; }
        public int IsActive { get; set; }
        #endregion
    }
    /// <summary>
    /// Dhananjay[2021-July-28] For SAP Item
    /// </summary>
    public class SAPItemTO
    {
        #region Declarations

        string prchseItem;
        string cardCode;
        Int32 ugpEntry;
        Int32 iUoMEntry;
        Int32 pUoMEntry;
        string gSTRelevnt;
        string gstTaxCtg;
        Int32 chapterID;
        string itemClass;
        Int32 sACEntry;
        #endregion


        #region Constructor
        public SAPItemTO()
        {
        }
        #endregion

        #region GetSet

        public string PrchseItem { get => prchseItem; set => prchseItem = value; }
        public string CardCode { get => cardCode; set => cardCode = value; }
        public Int32 UgpEntry { get => ugpEntry; set => ugpEntry = value; }
        public Int32 IUoMEntry { get => iUoMEntry; set => iUoMEntry = value; }
        public Int32 PUoMEntry { get => pUoMEntry; set => pUoMEntry = value; }
        public string GSTRelevnt { get => gSTRelevnt; set => gSTRelevnt = value; }
        public string GstTaxCtg { get => gstTaxCtg; set => gstTaxCtg = value; }
        public Int32 ChapterID { get => chapterID; set => chapterID = value; }
        public String ItemClass { get => itemClass; set => itemClass = value; }
        public Int32 SACEntry { get => sACEntry; set => sACEntry = value; }
        #endregion

    }

    // Add By Samadhan 25 Nov 2022
    public class DimProcessTO
    {
        #region
        int idProcess;
        string processName;
        int isActive;
        #endregion
        #region Constructor
        public DimProcessTO()
        {
        }

        #endregion

        public int IdProcess { get => idProcess; set => idProcess = value; }
        public string ProcessName { get => processName; set => processName = value; }
        public int IsActive { get => isActive; set => isActive = value; }
    }

    public class dimProcessType
    {
        #region
        int idProcessType;
        string processTypeName;
        string processTypeDesce;
        Int32 prodItemId;
        int isActive;
        #endregion
        #region Constructor
        public dimProcessType()
        {
        }

        #endregion

        public int IdProcessType { get => idProcessType; set => idProcessType = value; }
        public string ProcessTypeName { get => processTypeName; set => processTypeName = value; }
        public string ProcessTypeDesc { get => processTypeDesce; set => processTypeDesce = value; }
        public Int32 ProdItemId { get => prodItemId; set => prodItemId = value; }
        
        public int IsActive { get => isActive; set => isActive = value; }
    }
}
