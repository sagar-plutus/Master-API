using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{  
    public interface ITblProductItemBL
    {
        List<TblProductItemTO> SelectAllTblProductItemList(Int32 specificationId = 0);
        TblProductItemTO SelectTblProductItemTO(Int32 idProdItem);
        List<TblProductItemTO> GetProductItemDetailsForPurchaseItem(string idProdItems);
        TblProductItemTO SelectTblProductItemTO(Int32 idProdItem, SqlConnection conn, SqlTransaction tran);
        List<TblProductItemTO> SelectProductItemListStockUpdateRequire(int isStockRequire);
        List<TblProductItemTO> SelectProductItemListStockTOList(int activeYn, int PageNumber, int RowsPerPage, string strsearchtxt);
        List<TblProductItemTO> SelectProductItemList(Int32 idProdClass);
        List<DropDownTO> SelectProductItemListIsParity(Int32 isParity);
        ResultMessage InsertTblProductItemByName(TblProductItemTO tblProductItemTO);
        ResultMessage InsertTblProductItem(TblProductItemTO tblProductItemTO, List<TblPurchaseItemMasterTO> tblPurchaseItemMasterTO, List<WareHouseWiseItemDtlsTO> wareHouseWiseItemDtlsTOList, List<StoresLocationTO> StoresLocationTOList, List<TblProdItemMakeBrandExtTO> tblProdItemMakeBrandExtTOList = null);
        List<DropDownTO> GetGradeDropDownList(Int32 specificationId);
        //Pandurang[2021-02-26] commented due to no defination found with help of Amol sir
        // ResultMessage InsertTblProductItem(TblProductItemTO tblProductItemTO, List<TblPurchaseItemMasterTO> tblPurchaseItemMasterTO);
        int InsertTblProductItem(TblProductItemTO tblProductItemTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage UpdateTblProductItem(TblProductItemTO tblProductItemTO, List<TblPurchaseItemMasterTO> tblPurchaseItemMasterTOList, List<WareHouseWiseItemDtlsTO> wareHouseWiseItemDtlsTOList, List<StoresLocationTO> StoresLocationTOList,  List<TblProdItemMakeBrandExtTO> tblProdItemMakeBrandExtTOList= null);
        int UpdateTblProductItem(TblProductItemTO tblProductItemTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblProductItemTaxType(String idClassStr, Int32 codeTypeId, SqlConnection conn, SqlTransaction tran);
        int DeleteTblProductItem(Int32 idProdItem);
        int DeleteTblProductItem(Int32 idProdItem, SqlConnection conn, SqlTransaction tran);
        TblPurchaseItemMasterTO SelectTblPurchaseItemMasterTO(Int32 prodItemId);
        List<TblPurchaseItemMasterTO> SelectProductItemPurchaseDataAllList(Int32 prodItemId);
        List<StoresLocationTO> SelectItemStoreLocList(Int32 prodItemId);

        int InsertTblPurchaseItemMaster(TblPurchaseItemMasterTO tblPurchaseItemMasterTO);
        int UpdateTblPurchaseItemMasterTO(TblPurchaseItemMasterTO tblPurchaseItemMasterTO);
        int UpdateProductItemSequenceNo(List<TblProductItemTO> tblProductItemTO);
        List<TblPurchaseItemMasterTO> SelectAllTblPurchaseItemMasterTOList(Int32 prodItemId, Int32 purchaseItemMasterId);
        // int UpdateIsPriorityOfPurchaseItem(TblPurchaseItemMasterTO tblPurchaseItemMasterTO, SqlConnection conn, SqlTransaction tran);
        List<DropDownTO> GetProductItemDropDownList();
        List<TblProductItemTO> SelectAllProductItemListWrtSubGroupOrBaseItem(Int32 prodClassId = 0, int baseItemId = 0, int NonListedType=1);
        List<TblProductItemTO> SelectProductItemList(Int32 idProdClass, Int32 subCategoryId, Int32 itemID, Int32 procurementId = 0, Int32 ProductTypeId = 0, int isShowListed = 0, string warehouseIds = "");
        List<TblProductItemTO> SelectProductItemFromScrapCategoryList(Int32 idProdClass, Int32 subCategoryId, Int32 itemID, Int32 procurementId = 0);
        List<TblProductItemTO> GetSearchProductItemForStockView(string itemName = "", Int32 itemNo = 0, Int32 categoryNo = 0, Int32 subCategoryId = 0, Int32 subSubCat = 0, string warehouseIds = "", Int32 procurementId = 0);
        List<TblProductItemTO> SearchProductItemList(string itemName, Int32 itemNo, Int32 categoryNo, Int32 subCategoryId, Int32 itemID, string warehouseIds, Int32 procurementId = 0, Int32 ProductTypeId = 0, int isShowListed = 0, Int32 isShowMinLevelItem = 1);
        //Priyanka [11-07-2019]
        ResultMessage DeactivateBaseOrMakeItem(Int32 idProdItem,Int32 baseProdItemId, Int32 loginUserId);
        ResultMessage PostNewItemSupplier(List<TblPurchaseItemMasterTO> tblPurchaseItemMasterTOList);
        List<DropDownTO> GetMaxPriorityItemSupplier(string idProdItems);
        
        ResultMessage UpdateInvalidItems(Int32 itemProdCatId);
        List<TblProductItemTO> SelectListOfProductItemTOOnSearchString(string searchString, Int32 ProductTypeId = 0); 
        TblProductItemTO SelectTblProductItemFromOrignalItem(Int32 idProdItem);
        TblProductItemTO SelectTblProductItemFromOrignalItem(Int32 idProdItem, SqlConnection conn, SqlTransaction tran);
        ResultMessage InsertTblProductItem(TblProductItemTO tblProductItemTO, List<TblPurchaseItemMasterTO> tblPurchaseItemMasterTOList, SqlConnection conn, SqlTransaction tran);
        ResultMessage CopyPastItemScrapItem(int prodItemId,int userId);
        ResultMessage CopyPastMakeItem(int prodItemId, long baseProdItemId,int userId);
        List<TblProductItemTO> GetMakeItemList(Int32 BOMTypeId, String bomStatusIdStr,Int32 idProdItem=0, Int32 parentProdItemId = 0);
        
        ResultMessage InsertTblProductItemBom(List<TblProductItemBomTO> tblProductItemBomTOList);
        ResultMessage UpdateTblProductItemBom(List<TblProductItemBomTO> tblProductItemBomTOList);
        ResultMessage DeleteTblProductItemBom(TblProductItemBomTO tblProductItemBomTO);
        ResultMessage FinalizedProductItemBOM(TblProductItemTO finalizeItemBomTO);
        ResultMessage RevisedProductItemBOM(TblProductItemTO tblProductItemTO);
        ResultMessage RevisedProductItemBOMV2(TblProductItemTO tblProductItemTO);
        ResultMessage SaveBOMItemList(int parentBomId);
        ResultMessage CopyPastMakeItem(int prodItemId, long baseProdItemId, int userId, SqlConnection conn, SqlTransaction tran,ref int newProdItemId);
        ResultMessage AddUOMGroupV2FromMaster(DimUomGroupTO dimUomGroupTO);
        List<TblProductItemBomTO > GetBOMData(Int32 parentItemId);
        ResultMessage PostDeactivateNonListedItems();
        List<DropDownTO> getCountOfListedAndNonListedItems();
        ResultMessage SetMissingFieldsInSap();
        ResultMessage SelectAllItemListForExportToExcel(int materialListedType);
        ResultMessage UpdateItemWithIsPurchaseItem();//Dhananjay[2021-06-22] Added for Update SAP Item With IsPurchaseItem
        ResultMessage IsItemProperlyConfigured(Int32 idProdItem); //Dhananjay [2021-07-28] added for check item is properly configured
    }
}