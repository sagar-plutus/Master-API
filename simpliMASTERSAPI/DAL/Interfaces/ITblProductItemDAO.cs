using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
using ODLMWebAPI.StaticStuff;

namespace ODLMWebAPI.DAL.Interfaces
{    
    public interface ITblProductItemDAO 
    {
        String SqlSelectQuery();
        List<TblProductItemTO> SelectAllTblProductItem(Int32 specificationId = 0);
        List<TblProductItemTO> GetProductItemDetailsForPurchaseItem(string idProdItem, SqlConnection conn = null, SqlTransaction tran = null);
        TblProductItemTO SelectTblProductItem(Int32 idProdItem, SqlConnection conn = null, SqlTransaction tran = null);
        List<TblProductItemTO> ConvertDTToList(SqlDataReader tblProductItemTODT);
        List<TblProductItemTO> ConvertDTToListForUpdate(SqlDataReader tblProductItemTODT);
        List<TblProductItemTO> SelectProductItemListStockUpdateRequire(int isStockRequire);
        #region add paggination parameters updated by binal
        List<TblProductItemTO> SelectProductItemListStockTOList(int activeYn, int PageNumber, int RowsPerPage, string strsearchtxt);
        #endregion
        List<TblProductItemTO> SelectListOfProductItemTOOnprdClassId(string prodClassIds);
        List<DropDownTO> SelectProductItemListIsParity(Int32 isParity);
        List<DropDownTO> GetGradeDropDownList(Int32 specificationId = 0);
        int InsertTblProductItem(TblProductItemTO tblProductItemTO, TblPurchaseItemMasterTO tblPurchaseItemMasterTO);
        int InsertTblProductItem(TblProductItemTO tblProductItemTO, SqlConnection conn, SqlTransaction tran);
        
        int ExecuteInsertionCommand(TblProductItemTO tblProductItemTO, SqlCommand cmdInsert);
        int UpdateTblProductItem(TblProductItemTO tblProductItemTO);
        int UpdateTblProductItem(TblProductItemTO tblProductItemTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblItemLinkedStoreLoc(Int32 idProdItem, SqlConnection conn, SqlTransaction tran);
        int UpdateTblProductItemTaxType(String idClassStr, Int32 codeTypeId, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblProductItemTO tblProductItemTO, SqlCommand cmdUpdate);
        int ExecuteUpdateTblProductItemTaxType(String idClassStr, Int32 codeTypeId, SqlCommand cmdUpdate);
        int DeleteTblProductItem(Int32 idProdItem);
        int DeleteTblProductItem(Int32 idProdItem, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idProdItem, SqlCommand cmdDelete);
        int updatePreviousBase(SqlConnection conn, SqlTransaction tran);
        int InsertTblPurchaseItemMaster(TblPurchaseItemMasterTO tblPurchaseItemMasterTO, SqlConnection conn, SqlTransaction tran);
        int DeactivateTblPurchaseItemMaster(int prodItemId, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPurchaseItemMasterTO(TblPurchaseItemMasterTO tblPurchaseItemMasterTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPurchaseItemMasterTOAfterRevision(TblPurchaseItemMasterTO tblPurchaseItemMasterTO, SqlConnection conn, SqlTransaction tran);
        TblPurchaseItemMasterTO SelectTblPurchaseItemMaster(Int32 prodItemId);
        //int UpdateTblProductItemSequenceNo(TblProductItemTO tblProductItemTO, SqlConnection conn, SqlTransaction tran);
        List<TblPurchaseItemMasterTO> SelectProductItemPurchaseDataAllList(Int32 prodItemId);
        List<StoresLocationTO> SelectItemStoreLocList(Int32 prodItemId);
        int UpdateTblProductItemSequenceNo(Int32 isDisplaySequenceNo, Int32 idProdItem, SqlConnection conn, SqlTransaction tran);
        List<TblProductItemTO> SearchTblProductItem(string itemName, Int32 itemNo, Int32 categoryNo, string warehouseIds, int procurementId = 0, Int32 ProductTypeId = 0, int isShowListed = 0);

        List<TblPurchaseItemMasterTO> SelectAllTblPurchaseItemMasterTOList(Int32 prodItemId, Int32 purchaseItemMasterId);
        List<TblPurchaseItemMasterTO> SelectAllTblPurchaseItemMasterTOListOfData(Int32 prodItemId, Int32 purchaseItemMasterId);
        List<TblPurchaseItemMasterTO> SelectAllTblPurchaseItemMasterTOList(Int32 prodItemId);
        //int UpdateIsPriorityOfPurchaseItem(Int32 idPurchaseItemMaster,Int32 prodItemId, SqlConnection conn, SqlTransaction tran);
        int UpdateAllIsPriorityOfPurchaseItem(TblPurchaseItemMasterTO tblPurchaseItemMasterTO, SqlConnection conn, SqlTransaction tran);
        List<DropDownTO> GetProductItemDropDownList();
        List<TblProductItemTO> SelectAllProductItemListWrtSubGroupOrBaseItem(Int32 prodClassId = 0, int baseItemId = 0, int NonListedType=1,int isShowConvUOM=0);
        List<TblProductItemTO> checkMakeItemAlreadyExists(Int32 baseItemId, Int32 itemMakeId, Int32 itemBrandId, Int32 idProdItem, SqlConnection conn = null, SqlTransaction tran = null);
        //Priyanka [30-07-2019]
        List<TblProductItemTO> checkBaseItemAlreadyExists(Int32 idProdItem, Int32 prodClassId, string itemName, SqlConnection conn = null, SqlTransaction tran = null);
        List<TblProductItemTO> SelectListOfProductItemTOOnprdClassIds(string prodClassIds, Int32 procurementId = 0, Int32 ProductTypeId = 0, int isShowListed = 0, string warehouseIds = "");
        //Priyanka [11-07-2019] 
        List<TblProductItemTO> SelectTblProductItemListByBaseProdItemId(Int64 baseProdItemId, SqlConnection conn, SqlTransaction tran);
        List<DropDownTO> GetMaxPriorityItemSupplier(string prodItemIdStr, SqlConnection conn = null, SqlTransaction tran = null);
        List<TblProductItemTO> SelectListOfProductItemTOOnSearchString(string searchString, Int32 ProductTypeId = 0); 
        List<TblProductItemTO> findItemOfBaseItem(Int32 prodItemId);
        int UpdateHSNCode(Double HSNCode, Int32 idProdItem, SqlConnection conn, SqlTransaction tran);
       
        /// <summary>
        /// //Harshala[16/9/2020] added to update isConsumable flag 
        /// </summary>
        /// <param name="prodClassId">Sub Gup Id</param>
        /// <param name="isConsumable"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns>0, -1 if error</returns>
        int UpdateTblProductItemIsConsumable(Constants.ConsumableOrFixedAssetE consumableOrFixedAssetE, Int32 prodClassId, Boolean updateValue, SqlConnection conn, SqlTransaction tran);
        int UpdateTblProductItemIsConsumableForMakeItem(Constants.ConsumableOrFixedAssetE consumableOrFixedAssetE, Int32 idProdItem, Boolean updateValue, SqlConnection conn, SqlTransaction tran);
        TblProductItemTO SelectTblProductItemFromOrignalItem(Int32 idProdItem, SqlConnection conn = null, SqlTransaction tran = null);
        int UpdateisHaveScrapProdItem(TblProductItemTO tblProductItemTO, SqlConnection conn, SqlTransaction tran);
        List<TblProductItemTO> GetMakeItemList(Int32 BOMTypeId, String bomStatusIdStr,int idProdItem=0);
        int InsertTblProductItemBom(TblProductItemBomTO tblProductItemBomTO, SqlConnection conn, SqlTransaction tran);
        List<TblProductItemTO> GetMakeItemBOMList(String IdProdItemStr);
        int UpdateTblProductItemBom(TblProductItemBomTO tblProductItemBomTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblProductItemBom(TblProductItemBomTO tblProductItemBomTO, SqlConnection conn, SqlTransaction tran);
        int UpdateStatusTblProductItem(TblProductItemBomTO tblProductItemBomTO, SqlConnection conn, SqlTransaction tran);
        int UpdateStatusTblProductItem(TblProductItemTO tblProductItemTO, SqlConnection conn, SqlTransaction tran);
        int FinalizeTblProductItem(TblProductItemTO finalizeItemBomTO, SqlConnection conn, SqlTransaction tran);
        List<TblProductItemBomTO> GetItemBOMList(Int32 prodItemId);
        List<TblProductItemBomTO> GetItemBOMList(Int32 prodItemId, SqlConnection conn, SqlTransaction tran);
        int UpdateIsHavingNewRevTblProductItem(Int32 prodItemId, Int32 UpdatedBy, SqlConnection conn, SqlTransaction tran);
        int UpdateTblProductItemBomIsExisteInSAP(TblProductItemBomTO tblProductItemBomTO);
        int UpdateTblProductItemBomIsExisteInSAP(TblProductItemBomTO tblProductItemBomTO, SqlConnection conn, SqlTransaction tran);
        int UpdateAssetStoreLocation(TblProductItemTO tblProductItemTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblProductItemBomModelId(TblProductItemBomTO tblProductItemBomTO, SqlConnection conn, SqlTransaction tran);
        List<TblProductItemTO> SelectAllTblProductNonListedItemList(int deactivationTime, SqlConnection conn, SqlTransaction tran);
        int DeactivateTblProdctItemMaster(int idProdItem, SqlConnection conn, SqlTransaction tran);
        DropDownTO getCountOfListedAndNonListedItems(int nonlisted);
        List<DropDownTO> GetMissingFieldsItemFromSap();
        List<DropDownTO> GetMissingFieldsItemHavingConversionFactorOtherThan1();
        List<TblProductItemTO> SelectAllItemListForExportToExcel(int materialListedType);
        int UpdateItemWithIsPurchaseItem();//Dhananjay[2021-06-22] Added for Update SAP Item With IsPurchaseItem
        SAPItemTO GetItemFromSap(string ItemCode);//Dhananjay[2021-July-28] added
        int InsertTblItemLinkedStoreLoc(StoresLocationTO StoresLocationTOList, SqlConnection conn, SqlTransaction tran);

        int InsertTblProcessType(int ProcessTypeId,Int32 IdProdItem,string ProcessTypeName, SqlConnection conn, SqlTransaction tran);
        List<dimProcessType> checkProcessTpyeAlreadyExists(Int32 idProdItem, SqlConnection conn, SqlTransaction tran);
        List<DimProcessTO> GetProcessName(Int32 idProcess, SqlConnection conn, SqlTransaction tran);
        int UpdateTblProcessType(Int32 IdProdItem, string ProcessTypeName, SqlConnection conn, SqlTransaction tran);
        List<dimProcessType> GetNewProcessTypeId( SqlConnection conn, SqlTransaction tran);



    }
}