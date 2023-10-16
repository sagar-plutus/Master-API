using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL.Interfaces;

namespace ODLMWebAPI.DAL
{
    public class TblProductItemDAO : ITblProductItemDAO
    {
        private readonly IConnectionString _iConnectionString;
        private readonly ITblConfigParamsDAO _iTblConfigParamsDAO;
        private readonly ICommon _iCommon;
        public TblProductItemDAO(ICommon iCommon, IConnectionString iConnectionString, ITblConfigParamsDAO iTblConfigParamsDAO)
        {
            _iConnectionString = iConnectionString;
            _iTblConfigParamsDAO = iTblConfigParamsDAO;
            _iCommon = iCommon;

        }
        #region Methods
        public String SqlSelectQuery()
        {
            // String sqlSelectQry = " SELECT * FROM [tblProductItem]"; 
            String sqlSelectQry = "select baseItem.itemName as baseItemName,dimMasterValue.masterValueDesc as ItemClassName,case when item.idProdItem in (select parentProdItemId from tblProductItemBom) then 1 else 0 end as isHavingChild ," +
                                    //tblProductItemBom.qty as prodQty," +//Reshma[16-03-21] Commented for showing item in serch popup
                                    "dimUomGup.uomGroupName,dimUomGroupConversion.altQty as conversionFactor,tblProductItemPurchaseExt.nlcCost,tblProductItemPurchaseExt.purchaseUOMId as conversionUnitOfMeasure, p.prodClassDesc as category, c.prodClassDesc as subCategory,sc.prodClassDesc as specification,p.itemProdCatId as itemProdCatID,p.idProdClass as categoryID, c.idProdClass as subCategoryID,sc.idProdClass as specificationID,um.weightMeasurUnitDesc " +
                                  ",dimGenericMaster.value as manufacturer,dimGenericMasterP.value as priceListName,dimGenericMasterShip.value as shippingType ,tblLocation.locationDesc as storeLocation,dimGenericMasterUOM.weightMeasurUnitDesc AS UOM,dimGenericMasterCUOM.weightMeasurUnitDesc as CUOM,dimGenericMasterGroupUOM.weightMeasurUnitDesc as GUOM," +
                                  "dimGenericMasterSUOM.weightMeasurUnitDesc as SUOM,dimGenericMasterIUOM.weightMeasurUnitDesc AS IUOM,dimGenericMasterWarrenty.value as warrenty," +
                                   "gstCodeTbl.sapHsnCode as gstSapHsnCode , gstCodeTbl.codeDesc as gstCodeDesc, gstCodeTbl.codeNumber as gstCodeNumber, gstCodeTbl.taxPct as gstTaxPct, gstCodeTbl.codeTypeId as gstCodeTypeId ,PM.idProcess  as IdProcess," +
                                    "tblLocation.mappedTxnId as MappedLocationId ,ledger.ledgerName as finYearExpLedgerName, ledger.ledgerCode as finYearExpLedgerCode, " +
                                   SelectText() + " from tblProductItem item" +
                                   " LEFT JOIN tblProductItem baseItem ON baseItem.idProdItem = item.baseProdItemId " +
                                   " LEFT JOIN tblProdClassification sc ON item.prodClassId = sc.idProdClass " +
                                   " LEFT JOIN tblProdClassification c ON sc.parentProdClassId = c.idProdClass " +
                                   " LEFT JOIN tblProdClassification p ON c.parentProdClassId = p.idProdClass " +
                                   " LEFT JOIN dimUnitMeasures um ON item.weightMeasureUnitId = um.idWeightMeasurUnit " +
                                   " LEFT JOIN dimUomGroup dimUomGup on dimUomGup.idUomGroup = item.uOMGroupId " +
                                   " LEFT JOIN dimGenericMaster dimGenericMaster on dimGenericMaster.idGenericMaster = item.manufacturerId                                  " +
                                   " Left join dimGenericMaster dimGenericMasterP on dimGenericMasterP.idGenericMaster = item.priceListId                                   " +
                                   " Left join dimGenericMaster dimGenericMasterShip on dimGenericMasterShip.idGenericMaster = item.shippingTypeId                          " +
                                   " Left join dimGenericMaster dimGenericMasterWarrenty on dimGenericMasterWarrenty.idGenericMaster = item.warrantyTemplateId              " +
                                   " Left join dimUnitMeasures dimGenericMasterUOM on dimGenericMasterUOM.idWeightMeasurUnit = item.weightMeasureUnitId                     " +
                                   " left join tblProductItemPurchaseExt tblProductItemPurchaseExt on tblProductItemPurchaseExt.prodItemId = item.idProdItem  and tblProductItemPurchaseExt.isActive = 1 and isnull(tblProductItemPurchaseExt.priority,0) = 1" +
                                   " Left join dimUnitMeasures dimGenericMasterGroupUOM on dimGenericMasterGroupUOM.idWeightMeasurUnit = tblProductItemPurchaseExt.purchaseUOMId                    " +
                                   " Left join dimUnitMeasures dimGenericMasterCUOM on dimGenericMasterCUOM.idWeightMeasurUnit = tblProductItemPurchaseExt.purchaseUOMId               " +
                                   " Left join dimUnitMeasures dimGenericMasterSUOM on dimGenericMasterSUOM.idWeightMeasurUnit = item.salesUOMId                            " +
                                   " Left join dimUnitMeasures dimGenericMasterIUOM on dimGenericMasterIUOM.idWeightMeasurUnit = item.inventUOMId                           " +
                                   " left join tblLocation tblLocation on tblLocation.idLocation = item.locationId                                                         " +
                                   " Left join dimUomGroupConversion dimUomGroupConversion on dimUomGroupConversion.uomGroupId = item.uOMGroupId and dimUomGroupConversion.uomId = tblProductItemPurchaseExt.purchaseUOMId " +
                                   " LEFT JOIN tblGstCodeDtls gstCodeTbl ON item.gstCodeId = gstCodeTbl.idGstCode " +
                                   " LEFT JOIN tblLocation ParentTblLocation on ParentTblLocation.idLocation =item.assetStoreLocationId " + //Reshma Added
                                   " left join tblFinLedger ledger on ledger.idFinLedger = item.finYearExpLedgerId " +
                                   " Left Join tblAssetClass asset on item.assetClassId =asset.idAssetClass " +
                                   " Left Join dimMasterValue dimMasterValue on item.ItemClassId =dimMasterValue.idMasterValue" +
                                   // Samadhan Added 28 Nov 2022
                                   " Left join dimProcessType  PT on item.idProdItem=PT.prodItemId " +
                                  " Left join  dimProcessMaster PM on PT.processTypeName = PM.ProcessName ";
            //" LEFT JOIN tblProductItemBom tblProductItemBom ON tblProductItemBom.childProdItemId = item.idProdItem";
            return sqlSelectQry;
        }

        public String SqlSelectQueryForUOMAndConvUOM()
        {
            // String sqlSelectQry = " SELECT * FROM [tblProductItem]"; 
            String sqlSelectQry = "select baseItem.itemName as baseItemName,dimMasterValue.masterValueDesc as ItemClassName,case when item.idProdItem in (select parentProdItemId from tblProductItemBom) then 1 else 0 end as isHavingChild ," +
                                    //tblProductItemBom.qty as prodQty," +//Reshma[16-03-21] Commented for showing item in serch popup
                                    "dimUomGup.uomGroupName,item.conversionFactor,tblProductItemPurchaseExt.nlcCost,item.conversionUnitOfMeasure, p.prodClassDesc as category, c.prodClassDesc as subCategory,sc.prodClassDesc as specification,p.itemProdCatId as itemProdCatID,p.idProdClass as categoryID, c.idProdClass as subCategoryID,sc.idProdClass as specificationID,um.weightMeasurUnitDesc " +
                                  ",dimGenericMaster.value as manufacturer,dimGenericMasterP.value as priceListName,dimGenericMasterShip.value as shippingType ,tblLocation.locationDesc as storeLocation,dimGenericMasterUOM.weightMeasurUnitDesc AS UOM,dimGenericMasterCUOM.weightMeasurUnitDesc as CUOM,dimGenericMasterGroupUOM.weightMeasurUnitDesc as GUOM," +
                                  "dimGenericMasterSUOM.weightMeasurUnitDesc as SUOM,dimGenericMasterIUOM.weightMeasurUnitDesc AS IUOM,dimGenericMasterWarrenty.value as warrenty," +
                                   "gstCodeTbl.sapHsnCode as gstSapHsnCode , gstCodeTbl.codeDesc as gstCodeDesc, gstCodeTbl.codeNumber as gstCodeNumber, gstCodeTbl.taxPct as gstTaxPct, gstCodeTbl.codeTypeId as gstCodeTypeId ," +
                                    "tblLocation.mappedTxnId as MappedLocationId ,ledger.ledgerName as finYearExpLedgerName,PM.idProcess as IdProcess, ledger.ledgerCode as finYearExpLedgerCode,PM.idProcess  as IdProcess, " +
                                   SelectText() + " from tblProductItem item" +
                                   " LEFT JOIN tblProductItem baseItem ON baseItem.idProdItem = item.baseProdItemId " +
                                   " LEFT JOIN tblProdClassification sc ON item.prodClassId = sc.idProdClass " +
                                   " LEFT JOIN tblProdClassification c ON sc.parentProdClassId = c.idProdClass " +
                                   " LEFT JOIN tblProdClassification p ON c.parentProdClassId = p.idProdClass " +
                                   " LEFT JOIN dimUnitMeasures um ON item.weightMeasureUnitId = um.idWeightMeasurUnit " +
                                   " LEFT JOIN dimUomGroup dimUomGup on dimUomGup.idUomGroup = item.uOMGroupId " +
                                   " LEFT JOIN dimGenericMaster dimGenericMaster on dimGenericMaster.idGenericMaster = item.manufacturerId                                  " +
                                   " Left join dimGenericMaster dimGenericMasterP on dimGenericMasterP.idGenericMaster = item.priceListId                                   " +
                                   " Left join dimGenericMaster dimGenericMasterShip on dimGenericMasterShip.idGenericMaster = item.shippingTypeId                          " +
                                   " Left join dimGenericMaster dimGenericMasterWarrenty on dimGenericMasterWarrenty.idGenericMaster = item.warrantyTemplateId              " +
                                   " Left join dimUnitMeasures dimGenericMasterUOM on dimGenericMasterUOM.idWeightMeasurUnit = item.weightMeasureUnitId                     " +
                                   " left join tblProductItemPurchaseExt tblProductItemPurchaseExt on tblProductItemPurchaseExt.prodItemId = item.idProdItem  and tblProductItemPurchaseExt.isActive = 1 and isnull(tblProductItemPurchaseExt.priority,0) = 1" +
                                   " Left join dimUnitMeasures dimGenericMasterGroupUOM on dimGenericMasterGroupUOM.idWeightMeasurUnit = tblProductItemPurchaseExt.purchaseUOMId                    " +
                                   " Left join dimUnitMeasures dimGenericMasterCUOM on dimGenericMasterCUOM.idWeightMeasurUnit = tblProductItemPurchaseExt.purchaseUOMId               " +
                                   " Left join dimUnitMeasures dimGenericMasterSUOM on dimGenericMasterSUOM.idWeightMeasurUnit = item.salesUOMId                            " +
                                   " Left join dimUnitMeasures dimGenericMasterIUOM on dimGenericMasterIUOM.idWeightMeasurUnit = item.inventUOMId                           " +
                                   " left join tblLocation tblLocation on tblLocation.idLocation = item.locationId                                                         " +
                                   " Left join dimUomGroupConversion dimUomGroupConversion on dimUomGroupConversion.uomGroupId = item.uOMGroupId and dimUomGroupConversion.uomId = tblProductItemPurchaseExt.purchaseUOMId " +
                                   " LEFT JOIN tblGstCodeDtls gstCodeTbl ON item.gstCodeId = gstCodeTbl.idGstCode " +
                                   " LEFT JOIN tblLocation ParentTblLocation on ParentTblLocation.idLocation =item.assetStoreLocationId " + //Reshma Added
                                   " left join tblFinLedger ledger on ledger.idFinLedger = item.finYearExpLedgerId " +
                                   " Left Join tblAssetClass asset on item.assetClassId =asset.idAssetClass " +
                                   " Left Join dimMasterValue dimMasterValue on item.ItemClassId =dimMasterValue.idMasterValue" +
                                   // Samadhan Added 28 Nov 2022
                                   " Left join dimProcessType  PT on item.idProdItem=PT.prodItemId " +
                                  " Left join  dimProcessMaster PM on PT.processTypeName = PM.ProcessName ";
            //" LEFT JOIN tblProductItemBom tblProductItemBom ON tblProductItemBom.childProdItemId = item.idProdItem";
            return sqlSelectQry;
        }
        public String SqlSelectQueryFortblProductItemPurchaseExt()
        {
            //String sqlSelectQry = " SELECT * FROM [tblProductItemPurchaseExt]";
            String sqlSelectQry = "select tblOrg.firmName as supplierName ," +
                " tblprodItemPurchaseExt.priority,tblprodItemPurchaseExt.basicRate,tblprodItemPurchaseExt.purchaseUOMId , " +
                " tblAddr.areaName,tblAddr.idAddr, dimTaluka.talukaName,dimDistrict.districtName ,tblprodItemPurchaseExt.supplierOrgId,tblprodItemPurchaseExt.idPurchaseItemMaster," +
                " tblprodItemPurchaseExt.isPrioritySupplier,tblprodItemPurchaseExt.prodItemId from tblProductItemPurchaseExt tblprodItemPurchaseExt" +
                " LEFT JOIN tblOrganization tblOrg ON tblOrg.idOrganization = tblprodItemPurchaseExt.supplierOrgId " +
                " LEFT JOIN tblOrgAddress tblOrgAddr ON tblOrgAddr.organizationId = tblOrg.idOrganization " +
                " LEFT JOIN tblAddress tblAddr ON tblAddr.idAddr = tblOrgAddr.addressId " +
                " LEFT JOIN dimTaluka dimTaluka ON dimTaluka.idTaluka = tblAddr.talukaId " +
                " LEFT JOIN dimDistrict dimDistrict  ON dimDistrict.idDistrict = tblAddr.districtId ";

            return sqlSelectQry;
        }


        public string SqlQueryForExportToExcel()
        {
            return "select item.idProdItem,p.prodClassDesc as category, c.prodClassDesc as subCategory, item.materialTypeId, "
                  + "sc.prodClassDesc as specification,baseItem.itemName,item.itemName,item.itemDesc,item.weightMeasureUnitId, "
                  + "dimUnitMeasuresUOM.weightMeasurUnitDesc as UOM , "
                  + "case when tblProductItemPurchaseExt.purchaseUOMId > 0  then dimUnitMeasuresCUOM.weightMeasurUnitDesc "
                  + "else dimUnitMeasuresUOMC.weightMeasurUnitDesc end as CUOM , "
                  + "case when dimUomGroupConversion.altQty  > 0  then dimUomGroupConversion.altQty "
                  + "else item.conversionFactor end as conversionFactor, gstCodeTbl.codeNumber,dimUnitMeasuresSUOM.weightMeasurUnitDesc as SUOM, "
                  + "dimUnitMeasuresIUOM.weightMeasurUnitDesc AS IUOM,dimItemMake.itemMakeDesc,dimItemBrand.itemBrandDesc,item.catLogNo, "
                  + "item.manageItemById,tblProductItemPurchaseExt.mfgCatlogNo,item.salesWeight,item.salesHeight,item.salesWidth,item.salesLength, "
                  + "item.isDefaultMake,item.isImportedItem,item.basePrice,tblLocation.locationDesc,tblOrganization.firmName "
                  + ",tblProductItemPurchaseExt.basicRate,tblProductItemPurchaseExt.pf,tblProductItemPurchaseExt.discount,tblProductItemPurchaseExt.freight, "
                  + "tblProductItemPurchaseExt.currencyId,tblProductItemPurchaseExt.currencyRate,item.minOrderQty,item.orderMultiple,item.inventMinimum,"
                  + "item.valuationId,item.shippingTypeId,tblProductItemPurchaseExt.deliveryPeriodInDays,item.leadTime,item.toleranceDays,item.isTestCertificateCompulsary, "
                  + "item.additionalIdent,item.makeSeries,item.supItemCode,item.taxCategoryId ,dimGenericMasterManuf.value as manufacturer,dimGstTaxCategoryType.gstTaxCategoryTypeName, item.itemCategoryId"
                  + " from tblProductItem item "
                  + "LEFT JOIN tblProductItem baseItem ON baseItem.idProdItem = item.baseProdItemId "
                  + "LEFT JOIN tblProdClassification sc ON item.prodClassId = sc.idProdClass "
                  + "LEFT JOIN tblProdClassification c ON sc.parentProdClassId = c.idProdClass "
                  + "LEFT JOIN tblProdClassification p ON c.parentProdClassId = p.idProdClass "
                  + "LEFT JOIN dimUomGroup dimUomGup on dimUomGup.idUomGroup = item.uOMGroupId "
                  + "left join tblLocation tblLocation on tblLocation.idLocation = item.locationId "
                  + "Left join dimUnitMeasures dimUnitMeasuresUOM on dimUnitMeasuresUOM.idWeightMeasurUnit = item.weightMeasureUnitId "
                  + "Left join dimUnitMeasures dimUnitMeasuresUOMC on dimUnitMeasuresUOMC.idWeightMeasurUnit = item.conversionUnitOfMeasure "
                  + "left join tblProductItemPurchaseExt tblProductItemPurchaseExt on tblProductItemPurchaseExt.prodItemId = item.idProdItem "
                  + "and tblProductItemPurchaseExt.isActive = 1 and isnull(tblProductItemPurchaseExt.priority,0) = 1 "
                  + "Left join tblOrganization tblOrganization on tblOrganization.idOrganization = tblProductItemPurchaseExt.supplierOrgId "
                  + "Left join dimUnitMeasures dimUnitMeasuresCUOM on dimUnitMeasuresCUOM.idWeightMeasurUnit = tblProductItemPurchaseExt.purchaseUOMId "
                  + "Left join dimUomGroupConversion dimUomGroupConversion on dimUomGroupConversion.uomGroupId = item.uOMGroupId "
                  + "and dimUomGroupConversion.uomId = tblProductItemPurchaseExt.purchaseUOMId "
                  + "LEFT JOIN tblGstCodeDtls gstCodeTbl ON item.gstCodeId = gstCodeTbl.idGstCode "
                  + "Left join dimUnitMeasures dimUnitMeasuresSUOM on dimUnitMeasuresSUOM.idWeightMeasurUnit = item.salesUOMId "
                  + "Left join dimUnitMeasures dimUnitMeasuresIUOM on dimUnitMeasuresIUOM.idWeightMeasurUnit = item.inventUOMId "
                  + "Left join dimItemMake dimItemMake on dimItemMake.idItemMake = item.itemMakeId "
                  + "Left join dimItemBrand dimItemBrand on dimItemBrand.idItemBrand = item.itemBrandId "
                  + " Left join dimGenericMaster dimGenericMasterManuf on dimGenericMasterManuf.idGenericMaster = item.manufacturerId " +
                  " Left join dimGstTaxCategoryType dimGstTaxCategoryType on dimGstTaxCategoryType.gstTaxCategoryTypeId = item.taxCategoryId  and dimGstTaxCategoryType.isActive=1 ";
        }
        #endregion

        #region Selection


        public List<TblPurchaseItemMasterTO> SelectAllTblPurchaseItemMasterTOListOfData(Int32 prodItemId, Int32 purchaseItemMasterId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                if (prodItemId != 0)
                    cmdSelect.CommandText = SqlSelectQueryFortblProductItemPurchaseExt() + " WHERE tblprodItemPurchaseExt.prodItemId = " + prodItemId + " AND tblprodItemPurchaseExt.idPurchaseItemMaster NOT IN " + "(" + purchaseItemMasterId + ") AND tblprodItemPurchaseExt.isActive =1 ";


                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseItemMasterTO> list = ConvertDTToListForViewTblPurchaseItemMaster(reader);
                //List<TblPurchaseItemMasterTO> list = ConvertDTToListForTblPurchaseItemMaster(reader);


                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                // conn.Close();
                cmdSelect.Dispose();
            }
        }
        //Reshma Added
        public List<TblPurchaseItemMasterTO> SelectAllTblPurchaseItemMasterTOList(Int32 prodItemId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                if (prodItemId != 0)
                    cmdSelect.CommandText = " select * from tblProductItemPurchaseExt where prodItemId =" + prodItemId + " AND  isActive =1 ";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseItemMasterTO> list = ConvertDTToListForTblPurchaseItemMasterNew(reader);
                //List<TblPurchaseItemMasterTO> list = ConvertDTToListForTblPurchaseItemMaster(reader);


                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                // conn.Close();
                cmdSelect.Dispose();
            }
        }


        public List<TblPurchaseItemMasterTO> SelectAllTblPurchaseItemMasterTOList(Int32 prodItemId, Int32 purchaseItemMasterId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                if (prodItemId != 0)
                    cmdSelect.CommandText = SqlSelectQueryFortblProductItemPurchaseExt() + " WHERE tblprodItemPurchaseExt.prodItemId = " + prodItemId + " AND tblprodItemPurchaseExt.idPurchaseItemMaster NOT IN " + "(" + purchaseItemMasterId + ") AND tblprodItemPurchaseExt.isActive =1";


                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseItemMasterTO> list = ConvertDTToListForTblPurchaseItemMaster(reader);
                //List<TblPurchaseItemMasterTO> list = ConvertDTToListForViewTblPurchaseItemMaster(reader);

                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public List<TblPurchaseItemMasterTO> ConvertDTToListForViewTblPurchaseItemMaster(SqlDataReader tblPurchaseItemMasterTODT)
        {
            List<TblPurchaseItemMasterTO> tblPurchaseItemMasterTORow = new List<TblPurchaseItemMasterTO>();
            if (tblPurchaseItemMasterTODT != null)
            {
                while (tblPurchaseItemMasterTODT.Read())
                {
                    TblPurchaseItemMasterTO tblPurchaseItemMasterTONew = new TblPurchaseItemMasterTO();
                    if (tblPurchaseItemMasterTODT["idPurchaseItemMaster"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.IdPurchaseItemMaster = Convert.ToInt32(tblPurchaseItemMasterTODT["idPurchaseItemMaster"].ToString());
                    if (tblPurchaseItemMasterTODT["priority"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.Priority = Convert.ToDecimal(tblPurchaseItemMasterTODT["priority"].ToString());

                    //if (tblPurchaseItemMasterTODT["createdBy"] != DBNull.Value)
                    //    tblPurchaseItemMasterTONew.CreatedBy = Convert.ToInt32(tblPurchaseItemMasterTODT["createdBy"].ToString());
                    //if (tblPurchaseItemMasterTODT["updatedBy"] != DBNull.Value)
                    //    tblPurchaseItemMasterTONew.UpdatedBy = Convert.ToInt32(tblPurchaseItemMasterTODT["updatedBy"].ToString());
                    //if (tblPurchaseItemMasterTODT["createdOn"] != DBNull.Value)
                    //    tblPurchaseItemMasterTONew.CreatedOn = Convert.ToDateTime(tblPurchaseItemMasterTODT["createdOn"].ToString());
                    //if (tblPurchaseItemMasterTODT["updatedOn"] != DBNull.Value)
                    //    tblPurchaseItemMasterTONew.UpdatedOn = Convert.ToDateTime(tblPurchaseItemMasterTODT["updatedOn"].ToString());
                    //if (tblPurchaseItemMasterTODT["isActive"] != DBNull.Value)
                    //    tblPurchaseItemMasterTONew.IsActive = Convert.ToInt32(tblPurchaseItemMasterTODT["isActive"].ToString());

                    //if (tblPurchaseItemMasterTODT["currencyId"] != DBNull.Value)
                    //    tblPurchaseItemMasterTONew.CurrencyId = Convert.ToInt32(tblPurchaseItemMasterTODT["currencyId"].ToString());

                    if (tblPurchaseItemMasterTODT["supplierOrgId"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.SupplierOrgId = Convert.ToInt32(tblPurchaseItemMasterTODT["supplierOrgId"].ToString());

                    //if (tblPurchaseItemMasterTODT["gstCodeTypeId"] != DBNull.Value)
                    //    tblPurchaseItemMasterTONew.GstCodeTypeId = Convert.ToInt32(tblPurchaseItemMasterTODT["gstCodeTypeId"].ToString());

                    //if (tblPurchaseItemMasterTODT["currencyRate"] != DBNull.Value)
                    //    tblPurchaseItemMasterTONew.CurrencyRate = Convert.ToDecimal(tblPurchaseItemMasterTODT["currencyRate"].ToString());

                    if (tblPurchaseItemMasterTODT["basicRate"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.BasicRate = Convert.ToDecimal(tblPurchaseItemMasterTODT["basicRate"].ToString());

                    //if (tblPurchaseItemMasterTODT["gstItemCode"] != DBNull.Value)
                    //    tblPurchaseItemMasterTONew.GSTItemCode = Convert.ToString(tblPurchaseItemMasterTODT["gstItemCode"].ToString());
                    //if (tblPurchaseItemMasterTODT["discount"] != DBNull.Value)
                    //    tblPurchaseItemMasterTONew.Discount = Convert.ToDecimal(tblPurchaseItemMasterTODT["discount"].ToString());

                    //if (tblPurchaseItemMasterTODT["pf"] != DBNull.Value)
                    //    tblPurchaseItemMasterTONew.PF = Convert.ToDecimal(tblPurchaseItemMasterTODT["pf"].ToString());

                    //if (tblPurchaseItemMasterTODT["freight"] != DBNull.Value)
                    //    tblPurchaseItemMasterTONew.Freight = Convert.ToDecimal(tblPurchaseItemMasterTODT["freight"].ToString());
                    //if (tblPurchaseItemMasterTODT["deliveryPeriodInDays"] != DBNull.Value)
                    //    tblPurchaseItemMasterTONew.DeliveryPeriodInDays = Convert.ToDecimal(tblPurchaseItemMasterTODT["deliveryPeriodInDays"].ToString());

                    //if (tblPurchaseItemMasterTODT["multiplicationFactor"] != DBNull.Value)
                    //    tblPurchaseItemMasterTONew.MultiplicationFactor = Convert.ToDecimal(tblPurchaseItemMasterTODT["multiplicationFactor"].ToString());
                    //if (tblPurchaseItemMasterTODT["minimumOrderQty"] != DBNull.Value)
                    //    tblPurchaseItemMasterTONew.MinimumOrderQty = Convert.ToDecimal(tblPurchaseItemMasterTODT["minimumOrderQty"].ToString());

                    //if (tblPurchaseItemMasterTODT["supplierAddress"] != DBNull.Value)
                    //    tblPurchaseItemMasterTONew.SupplierAddress = Convert.ToInt32(tblPurchaseItemMasterTODT["supplierAddress"].ToString());

                    //if (tblPurchaseItemMasterTODT["mfgCatlogNo"] != DBNull.Value)
                    //    //tblPurchaseItemMasterTONew.mfgCatlogNo = Convert.ToString(tblPurchaseItemMasterTODT["mfgCatlogNo"]).ToString());
                    //    tblPurchaseItemMasterTONew.MfgCatlogNo = Convert.ToString(tblPurchaseItemMasterTODT["mfgCatlogNo"].ToString());
                    //if (tblPurchaseItemMasterTODT["weightMeasurUnitId"] != DBNull.Value)
                    //    tblPurchaseItemMasterTONew.WeightMeasurUnitId = Convert.ToInt32(tblPurchaseItemMasterTODT["weightMeasurUnitId"].ToString());
                    if (tblPurchaseItemMasterTODT["prodItemId"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.ProdItemId = Convert.ToInt32(tblPurchaseItemMasterTODT["prodItemId"].ToString());

                    //if (tblPurchaseItemMasterTODT["itemPerPurchaseUnit"] != DBNull.Value)
                    //    tblPurchaseItemMasterTONew.ItemPerPurchaseUnit = Convert.ToDecimal(tblPurchaseItemMasterTODT["itemPerPurchaseUnit"].ToString());
                    ////   if (tblPurchaseItemMasterTODT["lengthmm"] != DBNull.Value)
                    ////      tblPurchaseItemMasterTONew.length_mm = Convert.ToDecimal(tblPurchaseItemMasterTODT["lengthmm"].ToString());

                    //if (tblPurchaseItemMasterTODT["lengthmm"] != DBNull.Value)
                    //    tblPurchaseItemMasterTONew.Length_mm = Convert.ToDecimal(tblPurchaseItemMasterTODT["lengthmm"].ToString());
                    //if (tblPurchaseItemMasterTODT["widthmm"] != DBNull.Value)
                    //    tblPurchaseItemMasterTONew.Width_mm = Convert.ToDecimal(tblPurchaseItemMasterTODT["widthmm"].ToString());
                    //if (tblPurchaseItemMasterTODT["heightmm"] != DBNull.Value)
                    //    tblPurchaseItemMasterTONew.Height_mm = Convert.ToDecimal(tblPurchaseItemMasterTODT["heightmm"].ToString());
                    //if (tblPurchaseItemMasterTODT["volumeccm"] != DBNull.Value)
                    //    tblPurchaseItemMasterTONew.Volume_ccm = Convert.ToDecimal(tblPurchaseItemMasterTODT["volumeccm"].ToString());
                    //if (tblPurchaseItemMasterTODT["weightkg"] != DBNull.Value)
                    //    tblPurchaseItemMasterTONew.Weight_kg = Convert.ToDecimal(tblPurchaseItemMasterTODT["weightkg"].ToString());


                    //if (tblPurchaseItemMasterTODT["codeTypeId"] != DBNull.Value)
                    //    tblPurchaseItemMasterTONew.CodeTypeId = Convert.ToInt32(tblPurchaseItemMasterTODT["codeTypeId"].ToString());

                    ////Priyanka [25-04-2019]
                    if (tblPurchaseItemMasterTODT["purchaseUOMId"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.PurchaseUOMId = Convert.ToInt32(tblPurchaseItemMasterTODT["purchaseUOMId"].ToString());
                    //if (tblPurchaseItemMasterTODT["volumeId"] != DBNull.Value)
                    //    tblPurchaseItemMasterTONew.VolumeId = Convert.ToInt32(tblPurchaseItemMasterTODT["volumeId"].ToString());
                    //if (tblPurchaseItemMasterTODT["qtyPerPkg"] != DBNull.Value)
                    //    tblPurchaseItemMasterTONew.QtyPerPkg = Convert.ToDouble(tblPurchaseItemMasterTODT["qtyPerPkg"].ToString());
                    if (tblPurchaseItemMasterTODT["isPrioritySupplier"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.IsPrioritySupplier = Convert.ToInt32(tblPurchaseItemMasterTODT["isPrioritySupplier"].ToString());

                    if (tblPurchaseItemMasterTODT["supplierName"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.SupplierName = Convert.ToString(tblPurchaseItemMasterTODT["supplierName"].ToString());
                    if (tblPurchaseItemMasterTODT["areaName"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.AreaName = Convert.ToString(tblPurchaseItemMasterTODT["areaName"].ToString());
                    if (tblPurchaseItemMasterTODT["talukaName"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.TalukaName = Convert.ToString(tblPurchaseItemMasterTODT["talukaName"].ToString());
                    if (tblPurchaseItemMasterTODT["districtName"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.DistrictName = Convert.ToString(tblPurchaseItemMasterTODT["districtName"].ToString());
                    if (tblPurchaseItemMasterTODT["idAddr"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.IdAddr = Convert.ToInt32(tblPurchaseItemMasterTODT["idAddr"].ToString());


                    tblPurchaseItemMasterTORow.Add(tblPurchaseItemMasterTONew);
                    //tblPurchaseItemMasterTORow = tblPurchaseItemMasterTONew;
                }
                //else
                //{
                //    return null;
                //}
            }

            return tblPurchaseItemMasterTORow;
        }
        public List<TblProductItemTO> SelectAllTblProductItem(Int32 specificationId = 0)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String orderby = " order by item.displaySequanceNo asc ";
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                if (specificationId == 0)
                    // cmdSelect.CommandText = SqlSelectQuery() + " WHERE isActive=1";
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE item.isActive=1";
                else
                    // cmdSelect.CommandText = SqlSelectQuery() + " WHERE isActive=1 AND prodClassId=" + specificationId;
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE item.isActive=1 AND item.prodClassId=" + specificationId;


                cmdSelect.CommandText += " group by " + GroupByText() + orderby;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProductItemTO> list = ConvertDTToList(reader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public List<TblProductItemTO> GetMakeItemList(Int32 BOMTypeId, String bomStatusIdStr, int idProdItem = 0)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = "SELECT tblProductItem.idProdItem,tblProductItem.isProperSAPItem,tblModel.revisionNo, tblProductItem.itemName, tblProductItem.itemDesc,tblProductItem.procurementId, tblProductItem.bomTypeId," +
                    " tblProductItem.status,weightUnitMeasures.weightMeasurUnitDesc as UOM,tblProductItemPurchaseExt.nlcCost, tblProductItem.locationId FROM tblProductItem tblProductItem " + //[2021-01-21] Dhananjay Added , tblProductItem.locationId
                    " LEFT JOIN dimUnitMeasures weightUnitMeasures ON weightUnitMeasures.idWeightMeasurUnit = tblProductItem.weightMeasureUnitId " +
                    " LEFT JOIN dimUnitMeasures conversionUnitMeasures ON conversionUnitMeasures.idWeightMeasurUnit = tblProductItem.conversionUnitOfMeasure " +
                    " LEFT JOIN tblProductItemPurchaseExt tblProductItemPurchaseExt ON tblProductItemPurchaseExt.prodItemId = tblProductItem.idProdItem " +
                    " AND ISNULL(tblProductItemPurchaseExt.priority,0) = 1 AND ISNULL(tblProductItemPurchaseExt.isActive,0) = 1 " +
                    " LEFT JOIN tblModel tblModel on tblModel.prodItemId =  tblProductItem.idProdItem and versionNo=-1 " +
                    " WHERE tblProductItem.baseProdItemId IS NOT NULL AND tblProductItem.isActive = 1";

                if (idProdItem > 0)
                {
                    cmdSelect.CommandText += " AND ISNULL(tblProductItem.idProdItem, 0) = " + idProdItem;
                }
                else
                {
                    if (BOMTypeId == (Int32)Constants.dimBOMTypeE.Standard_BOM)
                    {
                        cmdSelect.CommandText += "and procurementId =1 AND  ISNULL(tblProductItem.bomTypeId ,0) =0 ";
                        if (!string.IsNullOrEmpty(bomStatusIdStr) && string.Compare(bomStatusIdStr, "1101") == 0)
                        {
                            cmdSelect.CommandText += " AND (tblProductItem.status IN(" + bomStatusIdStr + ")  or ( Isnull(tblProductItem.status,0)=0) )";
                        }
                        else if (!string.IsNullOrEmpty(bomStatusIdStr))
                        {
                            cmdSelect.CommandText += " AND tblProductItem.status IN(" + bomStatusIdStr + ") ";
                        }
                        else
                        {
                            cmdSelect.CommandText += "or ISNULL(tblProductItem.status,0) =0)";
                        }
                    }
                    else
                    {
                        cmdSelect.CommandText += " AND tblProductItem.bomTypeId = " + BOMTypeId + "";
                    }
                    if (!String.IsNullOrEmpty(bomStatusIdStr) && (BOMTypeId != (Int32)Constants.dimBOMTypeE.Standard_BOM))
                    {
                        if (string.Compare(bomStatusIdStr, "1101") == 0)
                        {
                            cmdSelect.CommandText += " AND (tblProductItem.status IN(" + bomStatusIdStr + ")  or ( Isnull(tblProductItem.status,0)=0) )";
                        }
                        else
                        {
                            cmdSelect.CommandText += " AND tblProductItem.status IN(" + bomStatusIdStr + ")";
                        }
                    }
                }
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProductItemTO> list = ConvertDTOFMakeItemList(reader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public List<TblProductItemTO> GetMakeItemBOMList(String IdProdItemStr)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = "SELECT tblProductItemBom.parentProdItemId, tblProductItemBom.idBomTree,weightUnitMeasures.weightMeasurUnitDesc as UOM,tblProductItemPurchaseExt.nlcCost, tblProductItemBom.isOptional, tblProductItemBom.qty, tblProductItem.idProdItem,"
                    + " tblProductItem.itemName, tblProductItem.itemDesc,tblProductItem.procurementId, tblProductItem.bomTypeId  ,"
                    + " tblLocation.mappedTxnId as MappedLocationId ,tblProductItem.issueId ,tblProductItem.locationId ,"
                    + " case when tblProductItemBom.childProdItemId in (select parentProdItemId from tblProductItemBom) then 1 else 0 end as isHavingChild  "
                    + " FROM tblProductItemBom tblProductItemBom "
                    + " JOIN tblProductItem tblProductItem on tblProductItem.idProdItem = tblProductItemBom.childProdItemId "
                    + "left join tblLocation tblLocation on tblLocation.idLocation = tblProductItem.locationId "
                    + " LEFT JOIN dimUnitMeasures weightUnitMeasures ON weightUnitMeasures.idWeightMeasurUnit = tblProductItem.weightMeasureUnitId "
                    + " LEFT JOIN tblProductItemPurchaseExt tblProductItemPurchaseExt ON tblProductItemPurchaseExt.prodItemId = tblProductItem.idProdItem "
                    + " and ISNULL(tblProductItemPurchaseExt.priority,0) = 1 AND ISNULL(tblProductItemPurchaseExt.isActive,0) = 1  "
                    + " LEFT JOIN tblModel tblModel on tblModel.idModel = tblProductItemBom.modelId and isnull(tblModel.versionNo,0) = (-1) "
                    + " WHERE tblProductItem.isActive = 1  and isnull(tblModel.versionNo,0) = (-1) and tblProductItemBom.parentProdItemId IN(" + IdProdItemStr + ")";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProductItemTO> list = ConvertDTOFMakeItemBOMList(reader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public List<TblProductItemTO> ConvertDTOFMakeItemBOMList(SqlDataReader tblProductItemTODT)
        {
            List<TblProductItemTO> tblProductItemTOList = new List<TblProductItemTO>();
            if (tblProductItemTODT != null)
            {
                while (tblProductItemTODT.Read())
                {
                    TblProductItemTO tblProductItemTONew = new TblProductItemTO();
                    if (tblProductItemTODT["idProdItem"] != DBNull.Value)
                        tblProductItemTONew.IdProdItem = Convert.ToInt32(tblProductItemTODT["idProdItem"].ToString());
                    if (tblProductItemTODT["itemName"] != DBNull.Value)
                        tblProductItemTONew.ItemName = Convert.ToString(tblProductItemTODT["itemName"].ToString());
                    if (tblProductItemTODT["itemDesc"] != DBNull.Value)
                        tblProductItemTONew.ItemDesc = Convert.ToString(tblProductItemTODT["itemDesc"].ToString());
                    //if (tblProductItemTODT["weightMeasureUnitId"] != DBNull.Value)
                    //    tblProductItemTONew.WeightMeasureUnitId = Convert.ToInt32(tblProductItemTODT["weightMeasureUnitId"]);
                    //if (tblProductItemTODT["MappedLocationId"] != DBNull.Value)
                    //    tblProductItemTONew.ConversionUnitOfMeasure = Convert.ToInt32(tblProductItemTODT["conversionUnitOfMeasure"]);
                    if (tblProductItemTODT["procurementId"] != DBNull.Value)
                        tblProductItemTONew.ProcurementId = Convert.ToInt32(tblProductItemTODT["procurementId"].ToString());
                    if (tblProductItemTODT["bomTypeId"] != DBNull.Value)
                        tblProductItemTONew.BomTypeId = Convert.ToInt32(tblProductItemTODT["bomTypeId"]);
                    if (tblProductItemTODT["parentProdItemId"] != DBNull.Value)
                        tblProductItemTONew.ParentProdItemId = Convert.ToInt32(tblProductItemTODT["parentProdItemId"]);
                    if (tblProductItemTODT["idBomTree"] != DBNull.Value)
                        tblProductItemTONew.IdBomTree = Convert.ToInt32(tblProductItemTODT["idBomTree"]);
                    if (tblProductItemTODT["qty"] != DBNull.Value)
                        tblProductItemTONew.Qty = Convert.ToDecimal(tblProductItemTODT["qty"]);
                    if (tblProductItemTODT["locationId"] != DBNull.Value)
                        tblProductItemTONew.LocationId = Convert.ToInt32(tblProductItemTODT["locationId"]);
                    if (tblProductItemTODT["issueId"] != DBNull.Value)
                        tblProductItemTONew.IssueId = Convert.ToInt32(tblProductItemTODT["issueId"]);
                    if (tblProductItemTODT["isOptional"] != DBNull.Value)
                        tblProductItemTONew.IsOptional = Convert.ToInt32(tblProductItemTODT["isOptional"]);
                    if (tblProductItemTODT["UOM"] != DBNull.Value)
                        tblProductItemTONew.UOM = (tblProductItemTODT["UOM"]).ToString();
                    if (tblProductItemTODT["nlcCost"] != DBNull.Value)
                        tblProductItemTONew.ItemCost = Convert.ToDouble(tblProductItemTODT["nlcCost"]);
                    if (tblProductItemTODT["isHavingChild"] != DBNull.Value)
                        tblProductItemTONew.IsHavingChild = Convert.ToInt32(tblProductItemTODT["isHavingChild"]);


                    if (tblProductItemTODT["MappedLocationId"] != DBNull.Value)
                        tblProductItemTONew.MappedLocationId = Convert.ToInt32(tblProductItemTODT["MappedLocationId"]);
                    tblProductItemTOList.Add(tblProductItemTONew);
                }
            }
            return tblProductItemTOList;
        }
        public List<TblProductItemBomTO> GetItemBOMList(Int32 prodItemId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM tblProductItemBom tblProductItemBom " +
                                        " LEFT JOIN tblModel tblModel on tblModel.idModel = tblProductItemBom.modelId and isnull(tblModel.versionNo,0) = (-1) " +
                                        " WHERE isnull(tblModel.versionNo,0) = (-1) AND tblProductItemBom.parentProdItemId = " + prodItemId;


                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProductItemBomTO> list = ConvertDTOFItemBOMList(reader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        //Reshma Added 
        public List<TblProductItemBomTO> GetItemBOMList(Int32 prodItemId, SqlConnection conn, SqlTransaction tran)
        {
            //String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            //SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {

                cmdSelect.CommandText = "SELECT * FROM tblProductItemBom tblProductItemBom "
                    + " LEFT JOIN tblModel tblModel on tblModel.idModel = tblProductItemBom.modelId and ISNULL(tblModel.versionNo,0) = (-1) "
                    + " WHERE ISNULL(tblModel.versionNo,0) = (-1) AND tblProductItemBom.parentProdItemId = " + prodItemId;

                cmdSelect.Transaction = tran;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProductItemBomTO> list = ConvertDTOFItemBOMList(reader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }
        public List<TblProductItemBomTO> ConvertDTOFItemBOMList(SqlDataReader tblProductItemTODT)
        {
            List<TblProductItemBomTO> tblProductItemTOList = new List<TblProductItemBomTO>();
            if (tblProductItemTODT != null)
            {
                while (tblProductItemTODT.Read())
                {
                    TblProductItemBomTO tblProductItemTONew = new TblProductItemBomTO();
                    if (tblProductItemTODT["idBomTree"] != DBNull.Value)
                        tblProductItemTONew.IdBomTree = Convert.ToInt32(tblProductItemTODT["idBomTree"]);
                    if (tblProductItemTODT["parentProdItemId"] != DBNull.Value)
                        tblProductItemTONew.ParentProdItemId = Convert.ToInt32(tblProductItemTODT["parentProdItemId"].ToString());
                    if (tblProductItemTODT["childProdItemId"] != DBNull.Value)
                        tblProductItemTONew.ChildProdItemId = Convert.ToInt32(tblProductItemTODT["childProdItemId"].ToString());
                    if (tblProductItemTODT["qty"] != DBNull.Value)
                        tblProductItemTONew.Qty = Convert.ToDecimal(tblProductItemTODT["qty"]);
                    if (tblProductItemTODT["isOptional"] != DBNull.Value)
                        tblProductItemTONew.IsOptional = Convert.ToInt32(tblProductItemTODT["isOptional"].ToString());
                    if (tblProductItemTODT["modelId"] != DBNull.Value)
                        tblProductItemTONew.ModelId = Convert.ToInt32(tblProductItemTODT["modelId"].ToString());

                    tblProductItemTOList.Add(tblProductItemTONew);
                }
            }
            return tblProductItemTOList;
        }
        public List<TblProductItemTO> ConvertDTOFMakeItemList(SqlDataReader tblProductItemTODT)
        {
            List<TblProductItemTO> tblProductItemTOList = new List<TblProductItemTO>();
            if (tblProductItemTODT != null)
            {
                while (tblProductItemTODT.Read())
                {
                    TblProductItemTO tblProductItemTONew = new TblProductItemTO();
                    if (tblProductItemTODT["idProdItem"] != DBNull.Value)
                        tblProductItemTONew.IdProdItem = Convert.ToInt32(tblProductItemTODT["idProdItem"].ToString());
                    if (tblProductItemTODT["itemName"] != DBNull.Value)
                        tblProductItemTONew.ItemName = Convert.ToString(tblProductItemTODT["itemName"].ToString());
                    if (tblProductItemTODT["itemDesc"] != DBNull.Value)
                        tblProductItemTONew.ItemDesc = Convert.ToString(tblProductItemTODT["itemDesc"].ToString());
                    //if (tblProductItemTODT["weightMeasureUnitId"] != DBNull.Value)
                    //    tblProductItemTONew.WeightMeasureUnitId = Convert.ToInt32(tblProductItemTODT["weightMeasureUnitId"]);
                    //if (tblProductItemTODT["conversionUnitOfMeasure"] != DBNull.Value)
                    //    tblProductItemTONew.ConversionUnitOfMeasure = Convert.ToInt32(tblProductItemTODT["conversionUnitOfMeasure"]);
                    if (tblProductItemTODT["procurementId"] != DBNull.Value)
                        tblProductItemTONew.ProcurementId = Convert.ToInt32(tblProductItemTODT["procurementId"].ToString());
                    if (tblProductItemTODT["bomTypeId"] != DBNull.Value)
                        tblProductItemTONew.BomTypeId = Convert.ToInt32(tblProductItemTODT["bomTypeId"]);
                    if (tblProductItemTODT["status"] != DBNull.Value)
                        tblProductItemTONew.Status = Convert.ToInt32(tblProductItemTODT["status"]);
                    if (tblProductItemTODT["locationId"] != DBNull.Value) //[2021-01-21] Dhananjay added
                        tblProductItemTONew.LocationId = Convert.ToInt32(tblProductItemTODT["locationId"]); //[2021-01-21] Dhananjay added
                    if (tblProductItemTODT["UOM"] != DBNull.Value) // Deepali added
                        tblProductItemTONew.UOM = (tblProductItemTODT["UOM"]).ToString(); // Deepali added
                    if (tblProductItemTODT["nlcCost"] != DBNull.Value)
                        tblProductItemTONew.ItemCost = Convert.ToDouble(tblProductItemTODT["nlcCost"]);
                    if (tblProductItemTODT["revisionNo"] != DBNull.Value)
                        tblProductItemTONew.RevisionNo = Convert.ToInt32(tblProductItemTODT["revisionNo"]);

                    if (tblProductItemTODT["isProperSAPItem"] != DBNull.Value)
                        tblProductItemTONew.IsProperSAPItem = Convert.ToInt32(tblProductItemTODT["isProperSAPItem"]);



                    tblProductItemTONew.Qty = 1;
                    tblProductItemTONew.ParentProdItemId = 0;
                    tblProductItemTOList.Add(tblProductItemTONew);

                }
            }
            return tblProductItemTOList;
        }
        public List<TblProductItemTO> checkMakeItemAlreadyExists(Int32 baseItemId, Int32 itemMakeId, Int32 itemBrandId, Int32 idProdItem, SqlConnection conn = null, SqlTransaction tran = null)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader notifyReader = null;
            try
            {
                if (conn != null)
                {
                    cmdSelect.Connection = conn;
                    cmdSelect.Transaction = tran;
                }
                else
                {
                    String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
                    conn = new SqlConnection(sqlConnStr);
                    cmdSelect.Connection = conn;
                    conn.Open();
                }
                if (idProdItem == 0)
                {
                    cmdSelect.CommandText = "select * from tblProductItem where baseProdItemId = @BaseItemId and itemMakeId = @ItemMakeId and itemBrandId = @ItemBrandId  AND isActive =1";
                }
                else
                {
                    cmdSelect.CommandText = "select * from tblProductItem where baseProdItemId = @BaseItemId and itemMakeId = @ItemMakeId and itemBrandId = @ItemBrandId and idProdItem != @IdProdItem AND isActive =1";
                }

                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@BaseItemId", System.Data.SqlDbType.Int).Value = baseItemId;
                cmdSelect.Parameters.Add("@ItemMakeId", System.Data.SqlDbType.Int).Value = itemMakeId;
                cmdSelect.Parameters.Add("@ItemBrandId", System.Data.SqlDbType.Int).Value = itemBrandId;
                cmdSelect.Parameters.Add("@IdProdItem", System.Data.SqlDbType.Int).Value = idProdItem;
                notifyReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConverDTForMakeItemList(notifyReader);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (notifyReader != null) notifyReader.Dispose();
                if (tran == null)
                {
                    conn.Close();
                }
                cmdSelect.Dispose();
            }
        }

        public List<TblProductItemTO> checkBaseItemAlreadyExists(Int32 idProdItem, Int32 prodClassId, string itemName, SqlConnection conn = null, SqlTransaction tran = null)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader notifyReader = null;
            try
            {
                if (conn != null)
                {
                    cmdSelect.Connection = conn;
                    cmdSelect.Transaction = tran;
                }
                else
                {
                    String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
                    conn = new SqlConnection(sqlConnStr);
                    cmdSelect.Connection = conn;
                    conn.Open();
                }
                if (idProdItem == 0)
                {
                    cmdSelect.CommandText = "select * from tblProductItem where prodClassId = @ProdClassId AND itemName =@ItemName AND isActive =1";
                }
                else
                {
                    cmdSelect.CommandText = "select * from tblProductItem where prodClassId = @ProdClassId AND itemName =@ItemName AND idProdItem != @IdProdItem AND isActive =1";

                }
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@ProdClassId", System.Data.SqlDbType.Int).Value = prodClassId;
                cmdSelect.Parameters.Add("@ItemName", System.Data.SqlDbType.NVarChar).Value = itemName;
                cmdSelect.Parameters.Add("@IdProdItem", System.Data.SqlDbType.Int).Value = idProdItem;

                notifyReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConverDTForMakeItemList(notifyReader);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (notifyReader != null) notifyReader.Dispose();
                if (tran == null)
                {
                    conn.Close();
                }
                cmdSelect.Dispose();
            }
        }
        public List<TblProductItemTO> ConverDTForMakeItemList(SqlDataReader tblProductItemTODT)
        {
            List<TblProductItemTO> tblProductItemTOList = new List<TblProductItemTO>();
            if (tblProductItemTODT != null)
            {
                while (tblProductItemTODT.Read())
                {
                    TblProductItemTO tblProductItemTONew = new TblProductItemTO();
                    if (tblProductItemTODT["idProdItem"] != DBNull.Value)
                        tblProductItemTONew.IdProdItem = Convert.ToInt32(tblProductItemTODT["idProdItem"].ToString());
                    if (tblProductItemTODT["itemName"] != DBNull.Value)
                        tblProductItemTONew.ItemName = Convert.ToString(tblProductItemTODT["itemName"].ToString());
                    if (tblProductItemTODT["itemDesc"] != DBNull.Value)
                        tblProductItemTONew.ItemDesc = Convert.ToString(tblProductItemTODT["itemDesc"].ToString());
                    tblProductItemTOList.Add(tblProductItemTONew);
                }
            }
            return tblProductItemTOList;
        }

        public List<TblProductItemTO> SelectAllProductItemListWrtSubGroupOrBaseItem(Int32 prodClassId = 0, int baseItemId = 0, int NonListedType = 1, int isShowConvUOM = 0)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            string nonListed = "";
            try
            {
                conn.Open();

                //cmdSelect.CommandText = " SELECT * FROM tblProdClassification tblProdClass WHERE  ISNULL(tblProdClass.itemProdCatId,0)= "+ itemProdCatId +
                //                        " AND isActive = 1";
                //if (baseItemId > 0)
                //    cmdSelect.CommandText = SqlSelectQuery() + " WHERE item.isActive=1 AND ISNULL(baseProdItemId,0)=" + baseItemId + " " + "ORDER BY displaySequanceNo ASC";
                //else
                //{
                //    if (specificationId == 0)
                //    {
                //        cmdSelect.CommandText = SqlSelectQuery() + " WHERE item.isActive=1 AND ISNULL(baseProdItemId,0)=0" + "ORDER BY displaySequanceNo ASC";

                //    }
                //    else
                //    {
                //        cmdSelect.CommandText = SqlSelectQuery() + " WHERE item.isActive=1 AND ISNULL(baseProdItemId,0)=0 AND item.prodClassId=" + specificationId + "ORDER BY displaySequanceNo ASC";
                //    }
                //}

                //if(prodClassId > 0 && baseItemId > 0)
                //{
                //if (NonListedType == 1)//Reshma
                //    nonListed = " ";
                //else if (NonListedType == 2)
                //    nonListed = " And item.isNonListed=0";
                //else if(NonListedType ==3)
                //    nonListed = " And item.isNonListed=1";
                if (isShowConvUOM == 1)
                {
                    cmdSelect.CommandText = SqlSelectQueryForUOMAndConvUOM() + " WHERE item.isActive=1 AND ISNULL(item.prodClassId,0)= " + prodClassId + " AND ISNULL(item.baseProdItemId,0) =" + baseItemId +
                                             //" " + nonListed + 
                                             " ORDER BY item.itemName ASC ";
                }
                else
                {
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE item.isActive=1 AND ISNULL(item.prodClassId,0)= " + prodClassId + " AND ISNULL(item.baseProdItemId,0) =" + baseItemId +
                                               //" " + nonListed + 
                                               " ORDER BY item.itemName ASC ";
                }
                // }

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProductItemTO> list = ConvertDTToList(reader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public TblProductItemTO SelectTblProductItem(Int32 idProdItem, SqlConnection conn = null, SqlTransaction tran = null)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                if (conn != null)
                {
                    cmdSelect.Connection = conn;
                    cmdSelect.Transaction = tran;
                }
                else
                {
                    String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
                    conn = new SqlConnection(sqlConnStr);
                    cmdSelect.Connection = conn;
                    conn.Open();
                }

                cmdSelect.CommandText = SqlSelectQuery() + " WHERE item.idProdItem = " + idProdItem + " ";
                cmdSelect.CommandText += " group by " + GroupByText();

                //cmdSelect.Connection = conn;
                //cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProductItemTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1)
                    return list[0];

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null)
                    reader.Dispose();
                cmdSelect.Dispose();
                if (tran == null)
                    conn.Close();
            }
        }

        //Reshma Added
        private List<TblProductItemTO> SelectTblProductItemWithProdClassIdList(Int32 prodClassId, SqlConnection conn = null, SqlTransaction tran = null)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
                conn = new SqlConnection(sqlConnStr);
                cmdSelect.Connection = conn;
                conn.Open();
                // cmdSelect.CommandText = "select  * from tblProductItem where prodClassId = " + prodClassId + " and ISNULL(baseProdItemId,0) =0    ";
                // Add By Samadhan 13 Feb 2023
                cmdSelect.CommandText = "select  item.*,PM.idProcess as IdProcess from tblProductItem item " +
                    " Left join dimProcessType  PT on item.idProdItem=PT.prodItemId " +
                    " Left join  dimProcessMaster PM on PT.processTypeName = PM.ProcessName " +

                    "where item.prodClassId = " + prodClassId + " and ISNULL(item.baseProdItemId,0) =0    ";

                //cmdSelect.CommandText += " group by " + GroupByText();

                //cmdSelect.Connection = conn;
                //cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProductItemTO> list = ConvertDTToList(reader);

                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null)
                    reader.Dispose();
                cmdSelect.Dispose();
                if (tran == null)
                    conn.Close();
            }
        }
        //Priyanka [11-07-2019]
        public List<TblProductItemTO> SelectTblProductItemListByBaseProdItemId(Int64 idProdItem, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                if (conn != null)
                {
                    cmdSelect.Connection = conn;
                    cmdSelect.Transaction = tran;
                }
                else
                {
                    String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
                    conn = new SqlConnection(sqlConnStr);
                    cmdSelect.Connection = conn;
                    conn.Open();
                }

                cmdSelect.CommandText = SqlSelectQuery() + " WHERE item.baseProdItemId = " + idProdItem + " AND item.isActive =1";
                cmdSelect.CommandText += " group by " + GroupByText();

                //cmdSelect.Connection = conn;
                //cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProductItemTO> list = ConvertDTToList(reader);
                if (list != null)
                    return list;

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
                if (tran == null)
                    conn.Close();
            }
        }
        public List<TblProductItemTO> GetProductItemDetailsForPurchaseItem(string idProdItems, SqlConnection conn = null, SqlTransaction tran = null)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                if (conn != null)
                {
                    cmdSelect.Connection = conn;
                    cmdSelect.Transaction = tran;
                }
                else
                {
                    String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
                    conn = new SqlConnection(sqlConnStr);
                    cmdSelect.Connection = conn;
                    conn.Open();
                }

                cmdSelect.CommandText = SqlSelectQuery() + " WHERE item.idProdItem IN (" + idProdItems + ")";
                cmdSelect.CommandText += "GROUP BY" + GroupByText();

                //cmdSelect.Connection = conn;
                //cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProductItemTO> list = ConvertDTToList(reader);
                if (list != null && list.Count > 0)
                    return list;

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
                if (tran == null)
                    conn.Close();
            }
        }

        public List<DropDownTO> GetMaxPriorityItemSupplier(string prodItemIdStr, SqlConnection conn = null, SqlTransaction tran = null)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                if (conn != null)
                {
                    cmdSelect.Connection = conn;
                    cmdSelect.Transaction = tran;
                }
                else
                {
                    String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
                    conn = new SqlConnection(sqlConnStr);
                    cmdSelect.Connection = conn;
                    conn.Open();
                }

                string sqlQuery = " select prodItemId, MAX(priority) as priority from tblProductItemPurchaseExt where isActive = 1 and prodItemId IN (" + prodItemIdStr + ") group by prodItemId";
                cmdSelect.CommandText = sqlQuery;

                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader tblProductItemPurchaseExtDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> itemSupplierList = new List<DropDownTO>();
                while (tblProductItemPurchaseExtDT.Read())
                {
                    DropDownTO dropDownTO = new DropDownTO();
                    if (tblProductItemPurchaseExtDT["prodItemId"] != DBNull.Value)
                        dropDownTO.Value = Convert.ToInt32(tblProductItemPurchaseExtDT["prodItemId"].ToString());

                    if (tblProductItemPurchaseExtDT["priority"] != DBNull.Value)
                        dropDownTO.Text = Convert.ToString(tblProductItemPurchaseExtDT["priority"].ToString());
                    itemSupplierList.Add(dropDownTO);
                }
                if (tblProductItemPurchaseExtDT != null)
                    tblProductItemPurchaseExtDT.Dispose();
                return itemSupplierList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
                if (tran == null)
                    conn.Close();
            }
        }

        public List<DropDownTO> GetPriorityOneItemSupplier(string prodItemIdStr, SqlConnection conn = null, SqlTransaction tran = null)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                if (conn != null)
                {
                    cmdSelect.Connection = conn;
                    cmdSelect.Transaction = tran;
                }
                else
                {
                    String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
                    conn = new SqlConnection(sqlConnStr);
                    cmdSelect.Connection = conn;
                    conn.Open();
                }

                string sqlQuery = " select * from tblProductItemPurchaseExt where isActive = 1 and prodItemId IN (" + prodItemIdStr + ") group by prodItemId";
                cmdSelect.CommandText = sqlQuery;

                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader tblProductItemPurchaseExtDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> itemSupplierList = new List<DropDownTO>();
                while (tblProductItemPurchaseExtDT.Read())
                {
                    DropDownTO dropDownTO = new DropDownTO();
                    if (tblProductItemPurchaseExtDT["prodItemId"] != DBNull.Value)
                        dropDownTO.Value = Convert.ToInt32(tblProductItemPurchaseExtDT["prodItemId"].ToString());

                    if (tblProductItemPurchaseExtDT["priority"] != DBNull.Value)
                        dropDownTO.Text = Convert.ToString(tblProductItemPurchaseExtDT["priority"].ToString());
                    itemSupplierList.Add(dropDownTO);
                }
                if (tblProductItemPurchaseExtDT != null)
                    tblProductItemPurchaseExtDT.Dispose();
                return itemSupplierList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
                if (tran == null)
                    conn.Close();
            }
        }
        public List<TblProductItemTO> ConvertDTToListForUpdateHsn(SqlDataReader tblProductItemTODT)
        {
            List<TblProductItemTO> tblProductItemTOListForHSNCode = new List<TblProductItemTO>();
            if (tblProductItemTODT != null)
            {
                while (tblProductItemTODT.Read())
                {
                    TblProductItemTO tblProductItemTONew = new TblProductItemTO();
                    if (tblProductItemTODT["idProdItem"] != DBNull.Value)
                        tblProductItemTONew.IdProdItem = Convert.ToInt32(tblProductItemTODT["idProdItem"].ToString());
                    if (tblProductItemTODT["hSNCode"] != DBNull.Value)
                        tblProductItemTONew.HSNCode = Convert.ToDouble(tblProductItemTODT["hSNCode"].ToString());

                    tblProductItemTOListForHSNCode.Add(tblProductItemTONew);
                }
            }
            return tblProductItemTOListForHSNCode;
        }
        //chetan[2020-10-01] added
        public TblProductItemTO SelectTblProductItemFromOrignalItem(Int32 idProdItem, SqlConnection conn = null, SqlTransaction tran = null)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                if (conn != null)
                {
                    cmdSelect.Connection = conn;
                    cmdSelect.Transaction = tran;
                }
                else
                {
                    String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
                    conn = new SqlConnection(sqlConnStr);
                    cmdSelect.Connection = conn;
                    conn.Open();
                }
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE item.orignalProdItemId = " + idProdItem + " ";
                cmdSelect.CommandText += "GROUP BY" + GroupByText();

                //cmdSelect.Connection = conn;
                //cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProductItemTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1)
                    return list[0];

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null)
                    reader.Dispose();
                cmdSelect.Dispose();
                if (tran == null)
                    conn.Close();
            }
        }

        public List<TblProductItemTO> SelectAllTblProductNonListedItemList(int deactivationTime, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;

                cmdSelect.CommandText = SqlSelectQuery() + " where ISNULL(item.IsNonListed,0)=1 and DATEDIFF(DAY,item.createdOn,GETDATE()) > " + deactivationTime;
                cmdSelect.CommandText += "GROUP BY" + GroupByText();

                //cmdSelect.Connection = conn;
                //cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProductItemTO> list = ConvertDTToList(reader);
                if (list != null && list.Count > 0)
                    return list;
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null)
                    reader.Dispose();
                cmdSelect.Dispose();
            }
        }
        public List<TblProductItemTO> ConvertDTToList(SqlDataReader tblProductItemTODT)
        {
            List<TblProductItemTO> tblProductItemTOList = new List<TblProductItemTO>();
            if (tblProductItemTODT != null)
            {
                while (tblProductItemTODT.Read())
                {
                    TblProductItemTO tblProductItemTONew = new TblProductItemTO();
                    if (tblProductItemTODT["idProdItem"] != DBNull.Value)
                        tblProductItemTONew.IdProdItem = Convert.ToInt32(tblProductItemTODT["idProdItem"].ToString());
                    if (tblProductItemTODT["prodClassId"] != DBNull.Value)
                        tblProductItemTONew.ProdClassId = Convert.ToInt32(tblProductItemTODT["prodClassId"].ToString());
                    if (tblProductItemTODT["createdBy"] != DBNull.Value)
                        tblProductItemTONew.CreatedBy = Convert.ToInt32(tblProductItemTODT["createdBy"].ToString());
                    if (tblProductItemTODT["updatedBy"] != DBNull.Value)
                        tblProductItemTONew.UpdatedBy = Convert.ToInt32(tblProductItemTODT["updatedBy"].ToString());
                    if (tblProductItemTODT["createdOn"] != DBNull.Value)
                        tblProductItemTONew.CreatedOn = Convert.ToDateTime(tblProductItemTODT["createdOn"].ToString());
                    if (tblProductItemTODT["updatedOn"] != DBNull.Value)
                        tblProductItemTONew.UpdatedOn = Convert.ToDateTime(tblProductItemTODT["updatedOn"].ToString());
                    if (tblProductItemTODT["itemName"] != DBNull.Value)
                        tblProductItemTONew.ItemName = Convert.ToString(tblProductItemTODT["itemName"].ToString());
                    if (tblProductItemTODT["itemDesc"] != DBNull.Value)
                        tblProductItemTONew.ItemDesc = Convert.ToString(tblProductItemTODT["itemDesc"].ToString());
                    if (tblProductItemTODT["remark"] != DBNull.Value)
                        tblProductItemTONew.Remark = Convert.ToString(tblProductItemTODT["remark"].ToString());
                    if (tblProductItemTODT["isActive"] != DBNull.Value)
                        tblProductItemTONew.IsActive = Convert.ToInt32(tblProductItemTODT["isActive"].ToString());
                    if (tblProductItemTODT["weightMeasureUnitId"] != DBNull.Value)
                        tblProductItemTONew.WeightMeasureUnitId = Convert.ToInt32(tblProductItemTODT["weightMeasureUnitId"]);
                    if (tblProductItemTODT["conversionUnitOfMeasure"] != DBNull.Value)
                        tblProductItemTONew.ConversionUnitOfMeasure = Convert.ToInt32(tblProductItemTODT["conversionUnitOfMeasure"]);
                    if (tblProductItemTODT["conversionFactor"] != DBNull.Value)
                        tblProductItemTONew.ConversionFactor = Convert.ToDouble(tblProductItemTODT["conversionFactor"]);
                    if (tblProductItemTODT["isStockRequire"] != DBNull.Value)
                        tblProductItemTONew.IsStockRequire = Convert.ToInt32(tblProductItemTODT["isStockRequire"].ToString());
                    if (tblProductItemTODT["isParity"] != DBNull.Value)
                        tblProductItemTONew.IsParity = Convert.ToInt32(tblProductItemTODT["isParity"].ToString());
                    if (tblProductItemTODT["basePrice"] != DBNull.Value)
                        tblProductItemTONew.BasePrice = Convert.ToDouble(tblProductItemTODT["basePrice"].ToString());
                    if (tblProductItemTODT["isBaseItemForRate"] != DBNull.Value)
                        tblProductItemTONew.IsBaseItemForRate = Convert.ToInt32(tblProductItemTODT["isBaseItemForRate"].ToString());
                    if (tblProductItemTODT["isNonCommercialItem"] != DBNull.Value)
                        tblProductItemTONew.IsNonCommercialItem = Convert.ToInt32(tblProductItemTODT["isNonCommercialItem"].ToString());
                    //Priyanka H [14-03-2019]
                    if (tblProductItemTODT["displaySequanceNo"] != DBNull.Value)
                        tblProductItemTONew.IsDisplaySequenceNo = Convert.ToInt32(tblProductItemTODT["displaySequanceNo"].ToString());
                    //Priyanka [16-05-2018]
                    if (tblProductItemTODT["codeTypeId"] != DBNull.Value)
                        tblProductItemTONew.CodeTypeId = Convert.ToInt32(tblProductItemTODT["codeTypeId"].ToString());
                    //Priyanka H [19-03-2019]
                    if (tblProductItemTODT["category"] != DBNull.Value)
                        tblProductItemTONew.Category = Convert.ToString(tblProductItemTODT["category"].ToString());
                    if (tblProductItemTODT["subCategory"] != DBNull.Value)
                        tblProductItemTONew.SubCategory = Convert.ToString(tblProductItemTODT["subCategory"].ToString());
                    if (tblProductItemTODT["specification"] != DBNull.Value)
                        tblProductItemTONew.Specification = Convert.ToString(tblProductItemTODT["specification"].ToString());
                    if (tblProductItemTODT["categoryID"] != DBNull.Value)
                        tblProductItemTONew.CategoryID = Convert.ToInt32(tblProductItemTODT["categoryID"].ToString());
                    if (tblProductItemTODT["subCategoryID"] != DBNull.Value)
                        tblProductItemTONew.SubCategoryID = Convert.ToInt32(tblProductItemTODT["subCategoryID"].ToString());
                    if (tblProductItemTODT["specificationID"] != DBNull.Value)
                        tblProductItemTONew.SpecificationID = Convert.ToInt32(tblProductItemTODT["specificationID"].ToString());
                    if (tblProductItemTODT["itemProdCatID"] != DBNull.Value)
                        tblProductItemTONew.ItemProdCatID = Convert.ToInt32(tblProductItemTODT["itemProdCatID"].ToString());


                    if (tblProductItemTODT["weightMeasurUnitDesc"] != DBNull.Value)
                        tblProductItemTONew.WeightMeasurUnitDesc = Convert.ToString(tblProductItemTODT["weightMeasurUnitDesc"].ToString());
                    if (tblProductItemTODT["conversionUnitOfMeasureId"] != DBNull.Value)
                        tblProductItemTONew.ConversionUnitOfMeasureId = Convert.ToInt32(tblProductItemTODT["conversionUnitOfMeasureId"]);


                    if (tblProductItemTODT["nlcCost"] != DBNull.Value)
                        tblProductItemTONew.NlcCost = Convert.ToDouble(tblProductItemTODT["nlcCost"]);

                    if (tblProductItemTODT["ItemClassName"] != DBNull.Value)
                        tblProductItemTONew.ItemClassName = (tblProductItemTODT["ItemClassName"]).ToString();



                    //Priyanka [24-04-2019]
                    //General 
                    if (tblProductItemTODT["priceListId"] != DBNull.Value)
                        tblProductItemTONew.PriceListId = Convert.ToInt32(tblProductItemTODT["priceListId"].ToString());
                    if (tblProductItemTODT["locationId"] != DBNull.Value)
                        tblProductItemTONew.LocationId = Convert.ToInt32(tblProductItemTODT["locationId"].ToString());

                    if (tblProductItemTODT["uOMGroupId"] != DBNull.Value)
                        tblProductItemTONew.UOMGroupId = Convert.ToInt32(tblProductItemTODT["uOMGroupId"].ToString());

                    if (tblProductItemTODT["uomGroupName"] != DBNull.Value)
                        tblProductItemTONew.UOMGroupName = (tblProductItemTODT["uomGroupName"].ToString());

                    if (tblProductItemTODT["itemCategoryId"] != DBNull.Value)
                        tblProductItemTONew.ItemCategoryId = Convert.ToInt32(tblProductItemTODT["itemCategoryId"].ToString());
                    if (tblProductItemTODT["manageItemById"] != DBNull.Value)
                        tblProductItemTONew.ManageItemById = Convert.ToInt32(tblProductItemTODT["manageItemById"].ToString());
                    if (tblProductItemTODT["isGSTApplicable"] != DBNull.Value)
                        tblProductItemTONew.IsGSTApplicable = Convert.ToInt32(tblProductItemTODT["isGSTApplicable"].ToString());
                    if (tblProductItemTODT["manufacturerId"] != DBNull.Value)
                        tblProductItemTONew.ManufacturerId = Convert.ToInt32(tblProductItemTODT["manufacturerId"].ToString());
                    if (tblProductItemTODT["shippingTypeId"] != DBNull.Value)
                        tblProductItemTONew.ShippingTypeId = Convert.ToInt32(tblProductItemTODT["shippingTypeId"].ToString());
                    if (tblProductItemTODT["materialTypeId"] != DBNull.Value)
                        tblProductItemTONew.MaterialTypeId = Convert.ToInt32(tblProductItemTODT["materialTypeId"].ToString());
                    if (tblProductItemTODT["isInventoryItem"] != DBNull.Value)
                        tblProductItemTONew.IsInventoryItem = Convert.ToInt32(tblProductItemTODT["isInventoryItem"].ToString());
                    if (tblProductItemTODT["isSalesItem"] != DBNull.Value)
                        tblProductItemTONew.IsSalesItem = Convert.ToInt32(tblProductItemTODT["isSalesItem"].ToString());
                    if (tblProductItemTODT["isPurchaseItem"] != DBNull.Value)
                        tblProductItemTONew.IsPurchaseItem = Convert.ToInt32(tblProductItemTODT["isPurchaseItem"].ToString());
                    if (tblProductItemTODT["warrantyTemplateId"] != DBNull.Value)
                        tblProductItemTONew.WarrantyTemplateId = Convert.ToInt32(tblProductItemTODT["warrantyTemplateId"].ToString());
                    if (tblProductItemTODT["mgmtMethodId"] != DBNull.Value)
                        tblProductItemTONew.MgmtMethodId = Convert.ToInt32(tblProductItemTODT["mgmtMethodId"].ToString());
                    if (tblProductItemTODT["additionalIdent"] != DBNull.Value)
                        tblProductItemTONew.AdditionalIdent = Convert.ToString(tblProductItemTODT["additionalIdent"].ToString());
                    if (tblProductItemTODT["hSNCode"] != DBNull.Value)
                        tblProductItemTONew.HSNCode = Convert.ToDouble(tblProductItemTODT["hSNCode"].ToString());
                    if (tblProductItemTODT["sACCode"] != DBNull.Value)
                        tblProductItemTONew.SACCode = Convert.ToDouble(tblProductItemTODT["sACCode"].ToString());
                    if (tblProductItemTODT["taxCategoryId"] != DBNull.Value)
                        tblProductItemTONew.TaxCategoryId = Convert.ToInt32(tblProductItemTODT["taxCategoryId"].ToString());
                    // Description added to query // 31-10-2019 Deepali   

                    if (tblProductItemTODT["warrenty"] != DBNull.Value)
                        tblProductItemTONew.Warrenty = Convert.ToString(tblProductItemTODT["warrenty"].ToString());
                    if (tblProductItemTODT["SUOM"] != DBNull.Value)
                        tblProductItemTONew.SUOM = Convert.ToString(tblProductItemTODT["SUOM"].ToString());
                    if (tblProductItemTODT["IUOM"] != DBNull.Value)
                        tblProductItemTONew.IUOM = Convert.ToString(tblProductItemTODT["IUOM"].ToString());
                    if (tblProductItemTODT["shippingType"] != DBNull.Value)
                        tblProductItemTONew.ShippingType = Convert.ToString(tblProductItemTODT["shippingType"].ToString());
                    if (tblProductItemTODT["storeLocation"] != DBNull.Value)
                        tblProductItemTONew.StoreLocation = Convert.ToString(tblProductItemTODT["storeLocation"].ToString());
                    if (tblProductItemTODT["manufacturer"] != DBNull.Value)
                        tblProductItemTONew.Manufacturer = Convert.ToString(tblProductItemTODT["manufacturer"].ToString());
                    if (tblProductItemTODT["priceListName"] != DBNull.Value)
                        tblProductItemTONew.PriceListName = Convert.ToString(tblProductItemTODT["priceListName"].ToString());
                    if (tblProductItemTODT["UOM"] != DBNull.Value)
                        tblProductItemTONew.UOM = Convert.ToString(tblProductItemTODT["UOM"].ToString());
                    if (tblProductItemTODT["CUOM"] != DBNull.Value)
                        tblProductItemTONew.CUOM = Convert.ToString(tblProductItemTODT["CUOM"].ToString());
                    if (tblProductItemTODT["GUOM"] != DBNull.Value)
                        tblProductItemTONew.GUOM = Convert.ToString(tblProductItemTODT["GUOM"].ToString());


                    //Sales

                    if (tblProductItemTODT["salesUOMId"] != DBNull.Value)
                        tblProductItemTONew.SalesUOMId = Convert.ToInt32(tblProductItemTODT["salesUOMId"].ToString());
                    if (tblProductItemTODT["itemPerSalesUnit"] != DBNull.Value)
                        tblProductItemTONew.ItemPerSalesUnit = Convert.ToInt32(tblProductItemTODT["itemPerSalesUnit"].ToString());
                    if (tblProductItemTODT["salesVolumeId"] != DBNull.Value)
                        tblProductItemTONew.SalesVolumeId = Convert.ToInt32(tblProductItemTODT["salesVolumeId"].ToString());
                    if (tblProductItemTODT["salesQtyPerPkg"] != DBNull.Value)
                        tblProductItemTONew.SalesQtyPerPkg = Convert.ToDouble(tblProductItemTODT["salesQtyPerPkg"].ToString());
                    if (tblProductItemTODT["salesLength"] != DBNull.Value)
                        tblProductItemTONew.SalesLength = Convert.ToDouble(tblProductItemTODT["salesLength"].ToString());
                    if (tblProductItemTODT["salesWidth"] != DBNull.Value)
                        tblProductItemTONew.SalesWidth = Convert.ToDouble(tblProductItemTODT["salesWidth"].ToString());
                    if (tblProductItemTODT["salesHeight"] != DBNull.Value)
                        tblProductItemTONew.SalesHeight = Convert.ToDouble(tblProductItemTODT["salesHeight"].ToString());
                    if (tblProductItemTODT["salesWeight"] != DBNull.Value)
                        tblProductItemTONew.SalesWeight = Convert.ToDouble(tblProductItemTODT["salesWeight"].ToString());

                    //Inventory
                    if (tblProductItemTODT["gLAccId"] != DBNull.Value)
                        tblProductItemTONew.GLAccId = Convert.ToInt32(tblProductItemTODT["gLAccId"].ToString());
                    if (tblProductItemTODT["inventUOMId"] != DBNull.Value)
                        tblProductItemTONew.InventUOMId = Convert.ToInt32(tblProductItemTODT["inventUOMId"].ToString());
                    if (tblProductItemTODT["reqPurchaseUOMId"] != DBNull.Value)
                        tblProductItemTONew.ReqPurchaseUOMId = Convert.ToInt32(tblProductItemTODT["reqPurchaseUOMId"].ToString());
                    if (tblProductItemTODT["valuationId"] != DBNull.Value)
                        tblProductItemTONew.ValuationId = Convert.ToInt32(tblProductItemTODT["valuationId"].ToString());
                    if (tblProductItemTODT["inventWeight"] != DBNull.Value)
                        tblProductItemTONew.InventWeight = Convert.ToDouble(tblProductItemTODT["inventWeight"].ToString());
                    if (tblProductItemTODT["inventMinimum"] != DBNull.Value)
                        tblProductItemTONew.InventMinimum = Convert.ToDouble(tblProductItemTODT["inventMinimum"].ToString());
                    if (tblProductItemTODT["itemCost"] != DBNull.Value)
                        tblProductItemTONew.ItemCost = Convert.ToDouble(tblProductItemTODT["itemCost"].ToString());
                    //Planning Data
                    if (tblProductItemTODT["planningId"] != DBNull.Value)
                        tblProductItemTONew.PlanningId = Convert.ToInt32(tblProductItemTODT["planningId"].ToString());
                    if (tblProductItemTODT["procurementId"] != DBNull.Value)
                        tblProductItemTONew.ProcurementId = Convert.ToInt32(tblProductItemTODT["procurementId"].ToString());
                    if (tblProductItemTODT["compWareHouseId"] != DBNull.Value)
                        tblProductItemTONew.CompWareHouseId = Convert.ToInt32(tblProductItemTODT["compWareHouseId"].ToString());
                    if (tblProductItemTODT["leadTime"] != DBNull.Value)
                        tblProductItemTONew.LeadTime = Convert.ToString(tblProductItemTODT["LeadTime"].ToString());
                    if (tblProductItemTODT["toleranceDays"] != DBNull.Value)
                        tblProductItemTONew.ToleranceDays = Convert.ToString(tblProductItemTODT["toleranceDays"].ToString());
                    if (tblProductItemTODT["minOrderQty"] != DBNull.Value)
                        tblProductItemTONew.MinOrderQty = Convert.ToDouble(tblProductItemTODT["minOrderQty"].ToString());
                    if (tblProductItemTODT["orderMultiple"] != DBNull.Value)
                        tblProductItemTONew.OrderMultiple = Convert.ToDouble(tblProductItemTODT["orderMultiple"].ToString());
                    if (tblProductItemTODT["gstCodeId"] != DBNull.Value)
                        tblProductItemTONew.GstCodeId = Convert.ToInt32(tblProductItemTODT["gstCodeId"].ToString());


                    //Production 
                    if (tblProductItemTODT["issueId"] != DBNull.Value)
                        tblProductItemTONew.IssueId = Convert.ToInt32(tblProductItemTODT["issueId"].ToString());

                    //For Make Item and base item identification 
                    if (tblProductItemTODT["baseProdItemId"] != DBNull.Value)
                        tblProductItemTONew.BaseProdItemId = Convert.ToInt64(tblProductItemTODT["baseProdItemId"].ToString());

                    if (tblProductItemTODT["itemMakeId"] != DBNull.Value)
                        tblProductItemTONew.ItemMakeId = Convert.ToInt32(tblProductItemTODT["itemMakeId"].ToString());
                    if (tblProductItemTODT["itemBrandId"] != DBNull.Value)
                        tblProductItemTONew.ItemBrandId = Convert.ToInt32(tblProductItemTODT["itemBrandId"].ToString());
                    if (tblProductItemTODT["catLogNo"] != DBNull.Value)
                        tblProductItemTONew.CatLogNo = Convert.ToString(tblProductItemTODT["catLogNo"].ToString());
                    if (tblProductItemTODT["supItemCode"] != DBNull.Value)
                        tblProductItemTONew.SupItemCode = Convert.ToString(tblProductItemTODT["supItemCode"].ToString());
                    if (tblProductItemTODT["isDefaultMake"] != DBNull.Value)
                        tblProductItemTONew.IsDefaultMake = Convert.ToInt32(tblProductItemTODT["isDefaultMake"].ToString());
                    if (tblProductItemTODT["isImportedItem"] != DBNull.Value)
                        tblProductItemTONew.IsImportedItem = Convert.ToInt32(tblProductItemTODT["isImportedItem"].ToString());
                    if (tblProductItemTODT["makeSeries"] != DBNull.Value)
                        tblProductItemTONew.MakeSeries = Convert.ToString(tblProductItemTODT["makeSeries"].ToString());
                    //Priyanka [02-08-2019]
                    if (tblProductItemTODT["isProperSAPItem"] != DBNull.Value)
                        tblProductItemTONew.IsProperSAPItem = Convert.ToInt32(tblProductItemTODT["isProperSAPItem"].ToString());
                    if (tblProductItemTODT["baseItemName"] != DBNull.Value)
                        tblProductItemTONew.BaseItemName = Convert.ToString(tblProductItemTODT["baseItemName"].ToString());

                    //Priyanka H [12/09/2019]
                    if (tblProductItemTODT["gstCodeTypeId"] != DBNull.Value)
                        tblProductItemTONew.GstCodeTypeId = Convert.ToInt32(tblProductItemTODT["gstCodeTypeId"].ToString());
                    if (tblProductItemTODT["gstTaxPct"] != DBNull.Value)
                        tblProductItemTONew.GstTaxPct = Convert.ToDouble(tblProductItemTODT["gstTaxPct"].ToString());
                    if (tblProductItemTODT["gstCodeNumber"] != DBNull.Value)
                        tblProductItemTONew.GstCodeNumber = Convert.ToString(tblProductItemTODT["gstCodeNumber"].ToString());
                    if (tblProductItemTODT["gstCodeDesc"] != DBNull.Value)
                        tblProductItemTONew.GstCodeDesc = Convert.ToString(tblProductItemTODT["gstCodeDesc"].ToString());
                    if (tblProductItemTODT["gstSapHsnCode"] != DBNull.Value)
                        tblProductItemTONew.GstSapHsnCode = Convert.ToString(tblProductItemTODT["GstSapHsnCode"].ToString());
                    if (tblProductItemTODT["isTestCertificateCompulsary"] != DBNull.Value)
                        tblProductItemTONew.IsTestCertificateCompulsary = Convert.ToInt32(tblProductItemTODT["isTestCertificateCompulsary"]);

                    if (tblProductItemTODT["isAllocationApplicable"] != DBNull.Value)
                        tblProductItemTONew.IsAllocationApplicable = Convert.ToInt32(tblProductItemTODT["isAllocationApplicable"]);
                    ////Reshma[13-09-2020] For Service Item
                    if (tblProductItemTONew.CodeTypeId == 1)
                    {
                        if (!string.IsNullOrEmpty(tblProductItemTONew.GstCodeNumber))
                        {
                            tblProductItemTONew.gstDisplayCode = tblProductItemTONew.GstCodeNumber.ToString();
                        }
                        else
                            tblProductItemTONew.gstDisplayCode = tblProductItemTONew.HSNCode.ToString();
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(tblProductItemTONew.GstCodeNumber))
                        {
                            tblProductItemTONew.gstDisplayCode = tblProductItemTONew.GstCodeNumber.ToString();
                        }
                        else
                            tblProductItemTONew.gstDisplayCode = tblProductItemTONew.SACCode.ToString();
                    }
                    if (tblProductItemTODT["isConsumable"] != DBNull.Value)
                        tblProductItemTONew.IsConsumable = Convert.ToBoolean(tblProductItemTODT["isConsumable"]);
                    if (tblProductItemTODT["isFixedAsset"] != DBNull.Value)
                        tblProductItemTONew.IsFixedAsset = Convert.ToBoolean(tblProductItemTODT["isFixedAsset"]);
                    ////Reshma
                    if (tblProductItemTODT["isNonListed"] != DBNull.Value)
                        tblProductItemTONew.IsNonListed = Convert.ToBoolean(tblProductItemTODT["isNonListed"]);
                    //For Scrap
                    if (tblProductItemTODT["isDailyScrapReq"] != DBNull.Value)
                        tblProductItemTONew.IsDailyScrapReq = Convert.ToBoolean(tblProductItemTODT["isDailyScrapReq"]);

                    if (tblProductItemTODT["scrapValuation"] != DBNull.Value)
                        tblProductItemTONew.ScrapValuation = Convert.ToDouble(tblProductItemTODT["scrapValuation"]);

                    //chetan[2020-20-01] added 
                    if (tblProductItemTODT["orignalProdItemId"] != DBNull.Value)
                        tblProductItemTONew.OrignalProdItemId = Convert.ToInt32(tblProductItemTODT["orignalProdItemId"]);

                    if (tblProductItemTODT["isHaveScrapProdItem"] != DBNull.Value)
                        tblProductItemTONew.IsHaveScrapProdItem = Convert.ToBoolean(tblProductItemTODT["isHaveScrapProdItem"]);
                    if (tblProductItemTODT["scrapStoreLocationId"] != DBNull.Value)
                        tblProductItemTONew.ScrapStoreLocationId = Convert.ToInt32(tblProductItemTODT["scrapStoreLocationId"]);

                    if (tblProductItemTODT["bomTypeId"] != DBNull.Value)
                        tblProductItemTONew.BomTypeId = Convert.ToInt32(tblProductItemTODT["bomTypeId"]);
                    if (tblProductItemTODT["status"] != DBNull.Value)//Reshma Added For BOM
                        tblProductItemTONew.Status = Convert.ToInt32(tblProductItemTODT["status"]);

                    //Fixed Asset
                    if (tblProductItemTODT["assetClassId"] != DBNull.Value)
                        tblProductItemTONew.AssetClassId = Convert.ToInt32(tblProductItemTODT["assetClassId"]);
                    if (tblProductItemTODT["assetStoreLocationId"] != DBNull.Value)
                        tblProductItemTONew.AssetStoreLocationId = Convert.ToInt32(tblProductItemTODT["assetStoreLocationId"]);
                    if (tblProductItemTODT["MapSapAssetClassId"] != DBNull.Value)
                        tblProductItemTONew.MapSapAssetClassId = Convert.ToString(tblProductItemTODT["MapSapAssetClassId"].ToString());
                    if (tblProductItemTODT["mappedSapAssetLocationId"] != DBNull.Value)
                        tblProductItemTONew.MappedSapAssetLocationId = Convert.ToString(tblProductItemTODT["mappedSapAssetLocationId"].ToString());
                    //
                    if (tblProductItemTODT["MappedLocationId"] != DBNull.Value)
                        tblProductItemTONew.MappedLocationId = Convert.ToInt32(tblProductItemTODT["MappedLocationId"].ToString());


                    if (tblProductItemTODT["finYearExpLedgerId"] != DBNull.Value)
                        tblProductItemTONew.FinYearExpLedgerId = Convert.ToInt64(tblProductItemTODT["finYearExpLedgerId"]);

                    if (tblProductItemTODT["finYearExpLedgerCode"] != DBNull.Value)
                        tblProductItemTONew.FinYearExpLedgerCode = Convert.ToString(tblProductItemTODT["finYearExpLedgerCode"]);

                    if (tblProductItemTODT["finYearExpLedgerName"] != DBNull.Value)
                        tblProductItemTONew.FinYearExpLedgerName = Convert.ToString(tblProductItemTODT["finYearExpLedgerName"]);
                    if (tblProductItemTODT["isScrapItem"] != DBNull.Value)
                        tblProductItemTONew.IsScrapItem = Convert.ToBoolean(tblProductItemTODT["isScrapItem"]);
                    if (tblProductItemTODT["isManageInventory"] != DBNull.Value) //AmolG[2020-Dec-18]
                        tblProductItemTONew.IsManageInventory = Convert.ToBoolean(tblProductItemTODT["isManageInventory"]);
                    if (tblProductItemTODT["rackNo"] != DBNull.Value) //AmolG[2021-Jan-14]
                        tblProductItemTONew.RackNo = Convert.ToString(tblProductItemTODT["rackNo"]);
                    if (tblProductItemTODT["xBinLocation"] != DBNull.Value) //AmolG[2021-Jan-14]
                        tblProductItemTONew.XBinLocation = Convert.ToString(tblProductItemTODT["xBinLocation"]);
                    if (tblProductItemTODT["yBinLocation"] != DBNull.Value) //AmolG[2021-Jan-14]
                        tblProductItemTONew.YBinLocation = Convert.ToString(tblProductItemTODT["yBinLocation"]);


                    if (tblProductItemTODT["itemClassId"] != DBNull.Value) //AmolG[2021-Jan-14]
                        tblProductItemTONew.ItemClassId = Convert.ToInt32(tblProductItemTODT["itemClassId"]);

                    if (tblProductItemTODT["isHavingChild"] != DBNull.Value) //AmolG[2021-Jan-14]
                        tblProductItemTONew.IsHavingChild = Convert.ToInt32(tblProductItemTODT["isHavingChild"]);

                    if (tblProductItemTODT["itemGrnNlcAmt"] != DBNull.Value) //AmolG[2021-Jan-14]
                        tblProductItemTONew.ItemGrnNlcAmt = Convert.ToInt32(tblProductItemTODT["itemGrnNlcAmt"]);
                    if (tblProductItemTODT["IdProcess"] != DBNull.Value) //Samadhan[2022-Nov-28]
                        tblProductItemTONew.IdProcess = Convert.ToInt32(tblProductItemTODT["IdProcess"]);

                    //if (tblProductItemTODT["prodQty"] != DBNull.Value) //AmolG[2021-Jan-14]
                    //    tblProductItemTONew.ProdQty = Convert.ToInt32(tblProductItemTODT["prodQty"]);



                    tblProductItemTOList.Add(tblProductItemTONew);
                }
            }
            return tblProductItemTOList;
        }

        public List<TblProductItemTO> ConvertDTToListForExport(SqlDataReader tblProductItemTODT)
        {
            List<TblProductItemTO> tblProductItemTOList = new List<TblProductItemTO>();
            if (tblProductItemTODT != null)
            {
                int i = 0;
                while (tblProductItemTODT.Read())
                {

                    TblProductItemTO tblProductItemTONew = new TblProductItemTO();
                    if (tblProductItemTODT["idProdItem"] != DBNull.Value)
                        tblProductItemTONew.IdProdItem = Convert.ToInt32(tblProductItemTODT["idProdItem"].ToString());
                    if (tblProductItemTODT["itemName"] != DBNull.Value)
                        tblProductItemTONew.ItemName = Convert.ToString(tblProductItemTODT["itemName"].ToString());
                    if (tblProductItemTODT["itemDesc"] != DBNull.Value)
                        tblProductItemTONew.ItemDesc = Convert.ToString(tblProductItemTODT["itemDesc"].ToString());

                    if (tblProductItemTODT["category"] != DBNull.Value)
                        tblProductItemTONew.Category = Convert.ToString(tblProductItemTODT["category"].ToString());
                    if (tblProductItemTODT["subCategory"] != DBNull.Value)
                        tblProductItemTONew.SubCategory = Convert.ToString(tblProductItemTODT["subCategory"].ToString());
                    if (tblProductItemTODT["specification"] != DBNull.Value)
                        tblProductItemTONew.Specification = Convert.ToString(tblProductItemTODT["specification"].ToString());


                    if (tblProductItemTODT["manufacturer"] != DBNull.Value)
                        tblProductItemTONew.Manufacturer = Convert.ToString(tblProductItemTODT["manufacturer"].ToString());

                    if (tblProductItemTODT["UOM"] != DBNull.Value)
                        tblProductItemTONew.UOM = Convert.ToString(tblProductItemTODT["UOM"].ToString());

                    if (tblProductItemTODT["SUOM"] != DBNull.Value)
                        tblProductItemTONew.SUOM = Convert.ToString(tblProductItemTODT["SUOM"].ToString());

                    if (tblProductItemTODT["IUOM"] != DBNull.Value)
                        tblProductItemTONew.IUOM = Convert.ToString(tblProductItemTODT["IUOM"].ToString());

                    if (tblProductItemTODT["CUOM"] != DBNull.Value)
                        tblProductItemTONew.CUOM = Convert.ToString(tblProductItemTODT["CUOM"].ToString());

                    if (tblProductItemTODT["conversionFactor"] != DBNull.Value)
                        tblProductItemTONew.ConversionFactor = Convert.ToDouble(tblProductItemTODT["conversionFactor"]);

                    if (tblProductItemTODT["basePrice"] != DBNull.Value)
                        tblProductItemTONew.BasePrice = Convert.ToDouble(tblProductItemTODT["basePrice"].ToString());

                    if (tblProductItemTODT["deliveryPeriodInDays"] != DBNull.Value)
                        tblProductItemTONew.DeliveryPeriodInDays = Convert.ToDouble(tblProductItemTODT["deliveryPeriodInDays"].ToString());

                    if (tblProductItemTODT["itemMakeDesc"] != DBNull.Value)
                        tblProductItemTONew.ItemMakeDesc = Convert.ToString(tblProductItemTODT["itemMakeDesc"].ToString());

                    if (tblProductItemTODT["itemBrandDesc"] != DBNull.Value)
                        tblProductItemTONew.ItemBrandDesc = Convert.ToString(tblProductItemTODT["itemBrandDesc"].ToString());


                    if (tblProductItemTODT["codeNumber"] != DBNull.Value)
                        tblProductItemTONew.gstDisplayCode = Convert.ToString(tblProductItemTODT["codeNumber"].ToString());


                    if (tblProductItemTODT["gstTaxCategoryTypeName"] != DBNull.Value)
                        tblProductItemTONew.TaxCategory = Convert.ToString(tblProductItemTODT["gstTaxCategoryTypeName"].ToString());

                    if (tblProductItemTODT["catLogNo"] != DBNull.Value)
                        tblProductItemTONew.CatLogNo = Convert.ToString(tblProductItemTODT["catLogNo"].ToString());

                    if (tblProductItemTODT["manageItemById"] != DBNull.Value)
                        tblProductItemTONew.ManageItemById = Convert.ToInt32(tblProductItemTODT["manageItemById"].ToString());

                    if (tblProductItemTONew.ManageItemById > 0)
                    {
                        tblProductItemTONew.ManageItemByStr = "YES";
                    }
                    else
                    {
                        tblProductItemTONew.ManageItemByStr = "NO";

                    }
                    if (tblProductItemTODT["mfgCatlogNo"] != DBNull.Value)
                        tblProductItemTONew.MfgCatlogNo = Convert.ToString(tblProductItemTODT["mfgCatlogNo"].ToString());

                    if (tblProductItemTODT["salesWeight"] != DBNull.Value)
                        tblProductItemTONew.SalesWeight = Convert.ToDouble(tblProductItemTODT["salesWeight"].ToString());

                    if (tblProductItemTODT["salesHeight"] != DBNull.Value)
                        tblProductItemTONew.SalesHeight = Convert.ToDouble(tblProductItemTODT["salesHeight"].ToString());
                    if (tblProductItemTODT["salesLength"] != DBNull.Value)
                        tblProductItemTONew.SalesLength = Convert.ToDouble(tblProductItemTODT["salesLength"].ToString());
                    if (tblProductItemTODT["salesWidth"] != DBNull.Value)
                        tblProductItemTONew.SalesWidth = Convert.ToDouble(tblProductItemTODT["salesWidth"].ToString());

                    if (tblProductItemTODT["materialTypeId"] != DBNull.Value)
                        tblProductItemTONew.MaterialTypeId = Convert.ToInt32(tblProductItemTODT["materialTypeId"].ToString());
                    if (tblProductItemTONew.MaterialTypeId == 3)
                    {
                        tblProductItemTONew.MaterialType = "Finished Goods";
                    }
                    else if (tblProductItemTONew.MaterialTypeId == 1)
                    {
                        tblProductItemTONew.MaterialType = "Raw Material";
                    }
                    else if (tblProductItemTONew.MaterialTypeId == 9)
                    {
                        tblProductItemTONew.MaterialType = "Service";
                    }

                    if (tblProductItemTODT["isDefaultMake"] != DBNull.Value)
                        tblProductItemTONew.IsDefaultMake = Convert.ToInt32(tblProductItemTODT["isDefaultMake"].ToString());

                    if (tblProductItemTONew.IsDefaultMake > 0)
                    {
                        tblProductItemTONew.IsDefaultMakeStr = "YES";
                    }
                    if (tblProductItemTODT["isImportedItem"] != DBNull.Value)
                        tblProductItemTONew.IsImportedItem = Convert.ToInt32(tblProductItemTODT["isImportedItem"].ToString());
                    tblProductItemTONew.IsImportedItemStr = "NO";
                    if (tblProductItemTONew.IsImportedItem > 0)
                    {
                        tblProductItemTONew.IsImportedItemStr = "YES";
                    }

                    if (tblProductItemTODT["itemCategoryId"] != DBNull.Value)
                        tblProductItemTONew.ItemCategoryId = Convert.ToInt32(tblProductItemTODT["itemCategoryId"].ToString());
                    tblProductItemTONew.ItemCategory = "Material";
                    if (tblProductItemTONew.IsImportedItem == 2)
                    {
                        tblProductItemTONew.ItemCategory = "Service";
                    }
                    if (tblProductItemTODT["basePrice"] != DBNull.Value)
                        tblProductItemTONew.BasePrice = Convert.ToDouble(tblProductItemTODT["basePrice"].ToString());

                    if (tblProductItemTODT["locationDesc"] != DBNull.Value)
                        tblProductItemTONew.LocationDesc = Convert.ToString(tblProductItemTODT["locationDesc"].ToString());

                    if (tblProductItemTODT["firmName"] != DBNull.Value)
                        tblProductItemTONew.Supplier = Convert.ToString(tblProductItemTODT["firmName"].ToString());

                    if (tblProductItemTODT["basicRate"] != DBNull.Value)
                        tblProductItemTONew.BasicRate = Convert.ToDouble(tblProductItemTODT["basicRate"].ToString());

                    if (tblProductItemTODT["pf"] != DBNull.Value)
                        tblProductItemTONew.PF = Convert.ToDouble(tblProductItemTODT["pf"].ToString());

                    if (tblProductItemTODT["freight"] != DBNull.Value)
                        tblProductItemTONew.Freight = Convert.ToDouble(tblProductItemTODT["freight"].ToString());

                    if (tblProductItemTODT["discount"] != DBNull.Value)
                        tblProductItemTONew.Discount = Convert.ToDouble(tblProductItemTODT["discount"].ToString());

                    if (tblProductItemTODT["currencyId"] != DBNull.Value)
                        tblProductItemTONew.CurrencyId = Convert.ToInt32(tblProductItemTODT["currencyId"].ToString());

                    tblProductItemTONew.Currency = "INR";

                    if (tblProductItemTODT["currencyRate"] != DBNull.Value)
                        tblProductItemTONew.CurrencyRate = Convert.ToDouble(tblProductItemTODT["currencyRate"].ToString());

                    if (tblProductItemTODT["minOrderQty"] != DBNull.Value)
                        tblProductItemTONew.MinOrderQty = Convert.ToDouble(tblProductItemTODT["minOrderQty"].ToString());

                    if (tblProductItemTODT["orderMultiple"] != DBNull.Value)
                        tblProductItemTONew.OrderMultiple = Convert.ToDouble(tblProductItemTODT["orderMultiple"].ToString());


                    if (tblProductItemTODT["inventMinimum"] != DBNull.Value)
                        tblProductItemTONew.InventMinimum = Convert.ToDouble(tblProductItemTODT["inventMinimum"].ToString());


                    if (tblProductItemTODT["valuationId"] != DBNull.Value)
                        tblProductItemTONew.ValuationId = Convert.ToInt32(tblProductItemTODT["valuationId"].ToString());

                    if (tblProductItemTODT["shippingTypeId"] != DBNull.Value)
                        tblProductItemTONew.ShippingTypeId = Convert.ToInt32(tblProductItemTODT["shippingTypeId"].ToString());

                    if (tblProductItemTODT["leadTime"] != DBNull.Value)
                        tblProductItemTONew.LeadTime = (tblProductItemTODT["leadTime"].ToString());

                    if (tblProductItemTODT["additionalIdent"] != DBNull.Value)
                        tblProductItemTONew.AdditionalIdent = (tblProductItemTODT["additionalIdent"].ToString());

                    if (tblProductItemTODT["toleranceDays"] != DBNull.Value)
                        tblProductItemTONew.ToleranceDays = (tblProductItemTODT["toleranceDays"].ToString());

                    if (tblProductItemTODT["makeSeries"] != DBNull.Value)
                        tblProductItemTONew.MakeSeries = (tblProductItemTODT["makeSeries"].ToString());

                    if (tblProductItemTODT["isTestCertificateCompulsary"] != DBNull.Value)
                        tblProductItemTONew.IsTestCertificateCompulsary = Convert.ToInt32(tblProductItemTODT["isTestCertificateCompulsary"].ToString());

                    tblProductItemTONew.IsTestCertificateCompulsaryStr = "NO";

                    if (tblProductItemTONew.IsTestCertificateCompulsary > 0)
                    {
                        tblProductItemTONew.IsTestCertificateCompulsaryStr = "YES";

                    }

                    if (tblProductItemTODT["supItemCode"] != DBNull.Value)
                        tblProductItemTONew.SupItemCode = Convert.ToString(tblProductItemTODT["supItemCode"].ToString());

                    if (tblProductItemTODT["taxCategoryId"] != DBNull.Value)
                        tblProductItemTONew.TaxCategoryId = Convert.ToInt32(tblProductItemTODT["taxCategoryId"]);



                    tblProductItemTOList.Add(tblProductItemTONew);
                }
            }
            return tblProductItemTOList;
        }


        public List<TblProductItemTO> ConvertDTToListForUpdate(SqlDataReader tblProductItemTODT)
        {
            List<TblProductItemTO> tblProductItemTOList = new List<TblProductItemTO>();
            if (tblProductItemTODT != null)
            {
                while (tblProductItemTODT.Read())
                {
                    TblProductItemTO tblProductItemTONew = new TblProductItemTO();
                    if (tblProductItemTODT["idProdItem"] != DBNull.Value)
                        tblProductItemTONew.IdProdItem = Convert.ToInt32(tblProductItemTODT["idProdItem"].ToString());
                    if (tblProductItemTODT["prodClassId"] != DBNull.Value)
                        tblProductItemTONew.ProdClassId = Convert.ToInt32(tblProductItemTODT["prodClassId"].ToString());
                    if (tblProductItemTODT["createdBy"] != DBNull.Value)
                        tblProductItemTONew.CreatedBy = Convert.ToInt32(tblProductItemTODT["createdBy"].ToString());
                    if (tblProductItemTODT["updatedBy"] != DBNull.Value)
                        tblProductItemTONew.UpdatedBy = Convert.ToInt32(tblProductItemTODT["updatedBy"].ToString());
                    if (tblProductItemTODT["createdOn"] != DBNull.Value)
                        tblProductItemTONew.CreatedOn = Convert.ToDateTime(tblProductItemTODT["createdOn"].ToString());
                    if (tblProductItemTODT["updatedOn"] != DBNull.Value)
                        tblProductItemTONew.UpdatedOn = Convert.ToDateTime(tblProductItemTODT["updatedOn"].ToString());
                    if (tblProductItemTODT["itemName"] != DBNull.Value)
                        tblProductItemTONew.ItemName = Convert.ToString(tblProductItemTODT["itemName"].ToString());
                    if (tblProductItemTODT["itemDesc"] != DBNull.Value)
                        tblProductItemTONew.ItemDesc = Convert.ToString(tblProductItemTODT["itemDesc"].ToString());
                    if (tblProductItemTODT["remark"] != DBNull.Value)
                        tblProductItemTONew.Remark = Convert.ToString(tblProductItemTODT["remark"].ToString());
                    if (tblProductItemTODT["isActive"] != DBNull.Value)
                        tblProductItemTONew.IsActive = Convert.ToInt32(tblProductItemTODT["isActive"].ToString());
                    if (tblProductItemTODT["weightMeasureUnitId"] != DBNull.Value)
                        tblProductItemTONew.WeightMeasureUnitId = Convert.ToInt32(tblProductItemTODT["weightMeasureUnitId"]);
                    if (tblProductItemTODT["conversionUnitOfMeasure"] != DBNull.Value)
                        tblProductItemTONew.ConversionUnitOfMeasure = Convert.ToInt32(tblProductItemTODT["conversionUnitOfMeasure"]);
                    if (tblProductItemTODT["conversionFactor"] != DBNull.Value)
                        tblProductItemTONew.ConversionFactor = Convert.ToDouble(tblProductItemTODT["conversionFactor"]);
                    if (tblProductItemTODT["isStockRequire"] != DBNull.Value)
                        tblProductItemTONew.IsStockRequire = Convert.ToInt32(tblProductItemTODT["isStockRequire"].ToString());
                    if (tblProductItemTODT["displayName"] != DBNull.Value)
                        tblProductItemTONew.ProdClassDisplayName = tblProductItemTODT["displayName"].ToString();
                    if (tblProductItemTODT["isBaseItemForRate"] != DBNull.Value)
                        tblProductItemTONew.IsBaseItemForRate = Convert.ToInt32(tblProductItemTODT["isBaseItemForRate"].ToString());
                    if (tblProductItemTODT["isNonCommercialItem"] != DBNull.Value)
                        tblProductItemTONew.IsNonCommercialItem = Convert.ToInt32(tblProductItemTODT["isNonCommercialItem"].ToString());
                    //Priyanka H [14-03-2019]
                    if (tblProductItemTODT["displaySequanceNo"] != DBNull.Value)
                        tblProductItemTONew.IsDisplaySequenceNo = Convert.ToInt32(tblProductItemTODT["displaySequanceNo"].ToString());
                    if (tblProductItemTODT["isTestCertificateCompulsary"] != DBNull.Value)
                        tblProductItemTONew.IsTestCertificateCompulsary = Convert.ToInt32(tblProductItemTODT["isTestCertificateCompulsary"]);
                    
                    DataTable dt = tblProductItemTODT.GetSchemaTable();

                    foreach (DataRow item in dt.Rows)
                    {
                        string ColumnName = item.ItemArray[0].ToString();
                        if (ColumnName == "TotalCount")
                        {
                            if (tblProductItemTODT["totalCount"] != DBNull.Value)
                                tblProductItemTONew.TotalCount = Convert.ToInt32(tblProductItemTODT["TotalCount"].ToString());
                        }
                        if (ColumnName == "SearchAllCount")
                        {
                            if (tblProductItemTODT["searchAllCount"] != DBNull.Value)
                                tblProductItemTONew.SearchAllCount = Convert.ToInt32(tblProductItemTODT["searchAllCount"].ToString());
                        }
                        if (ColumnName == "RowNumber")
                        {
                            if (tblProductItemTODT["rowNumber"] != DBNull.Value)
                                tblProductItemTONew.RowNumber = Convert.ToInt32(tblProductItemTODT["rowNumber"].ToString());
                        }
                    }


                    //if (tblProductItemTODT["isParity"] != DBNull.Value)
                    //    tblProductItemTONew.IsParity = Convert.ToInt32(tblProductItemTODT["isParity"].ToString());

                    tblProductItemTOList.Add(tblProductItemTONew);
                }
            }
            return tblProductItemTOList;
        }

        //sudhir[12-jan-2018] added for getlistof items whose stockupdate is require.
        public List<TblProductItemTO> SelectProductItemListStockUpdateRequire(int isStockRequire)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                if (isStockRequire == 0)
                    cmdSelect.CommandText = " SELECT ProdClassification.displayName,* FROM [tblProductItem] ProductItem " +
                                            " INNER JOIN tblProdClassification ProdClassification ON" +
                                            " ProductItem.prodClassId = ProdClassification.idProdClass WHERE ProductItem.isActive=1 AND isStockRequire=0";
                else
                    cmdSelect.CommandText = " SELECT ProdClassification.displayName,* FROM [tblProductItem] ProductItem " +
                                            " INNER JOIN tblProdClassification ProdClassification ON" +
                                            " ProductItem.prodClassId = ProdClassification.idProdClass WHERE ProductItem.isActive=1 AND ProductItem.isStockRequire=1";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProductItemTO> list = ConvertDTToListForUpdate(reader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblProductItemTO> SelectProductItemListStockTOList(int activeYn, int PageNumber, int RowsPerPage, string strsearchtxt)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select * from (select  COUNT(*) OVER () as SearchAllCount, ROW_NUMBER() over (order by displayName) as RowNumber, * from ( " +
                                    " SELECT COUNT(*) OVER () as TotalCount,ProdClassification.displayName," +
                                    " Isnull(ProdClassification.displayName+ '/','') +''+ Isnull(ProductItem.itemDesc,'') AS FullitemName," +
                                    " ProductItem.* FROM [tblProductItem] ProductItem " +
                                    " INNER JOIN tblProdClassification ProdClassification ON" +
                                    " ProductItem.prodClassId = ProdClassification.idProdClass " +
                                    " WHERE ISNULL(ProductItem.isActive, 0) = " + activeYn +

                                    " )as tbl1 where(( " + strsearchtxt + " = '') or (tbl1.displayName like '%' + " + strsearchtxt + " + '%'" +
                                    " or tbl1.idProdItem like '%' +  " + strsearchtxt + "  + '%' or tbl1.itemName like '%' +  " + strsearchtxt + "  + '%'" +
                                    " or tbl1.itemDesc  like '%' +  " + strsearchtxt + " + '%'" +
                                    " or tbl1.FullitemName  like '%' +  " + strsearchtxt + " + '%'" +
                                    " or tbl1.remark like '%' + " + strsearchtxt + " + '%')))as tbl2 where (" + RowsPerPage + " = 0 " +
                                    " or tbl2.RowNumber between ((" + PageNumber + " - 1) * " + RowsPerPage + " + 1) and (" + PageNumber + " * " + RowsPerPage + ")) " +
                                    " ORDER BY tbl2.displayName";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProductItemTO> list = ConvertDTToListForUpdate(reader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        //Sudhir[15-MAR-2018] Added for get List of ProductItem based on Category/SubCategory/Specification
        public List<TblProductItemTO> SelectListOfProductItemTOOnprdClassId(string prodClassIds)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();

                cmdSelect.CommandText = SqlSelectQuery() + " WHERE item.prodClassId IN (" + prodClassIds + ") AND item.isActive=1";
                cmdSelect.CommandText += "GROUP BY" + GroupByText();

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProductItemTO> list = ConvertDTToList(reader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
                conn.Close();
            }
        }

        /// <summary>
        /// Priyanka H [09/07/2019]  Added for get List of ProductItem based on Category/SubCategory/Specification
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public List<TblProductItemTO> SelectListOfProductItemTOOnprdClassIds(string prodClassIds, Int32 procurementId = 0, Int32 ProductTypeId = 0, int isShowListed = 0, string warehouseIds = "")
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;

            //Add By Samadhan 25 May 2022
            int ShowStorLocWiseItem = 0;
            TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_SHOW_STORLOCEWISE_ITEM_REQUEST);
            if (tblConfigParamsTO != null)
            {
                if (tblConfigParamsTO.ConfigParamVal == "1")
                {
                    ShowStorLocWiseItem = 1;
                }
            }

            try
            {
                conn.Open();

                cmdSelect.CommandText = SqlSelectQuery() + " WHERE item.prodClassId IN (" + prodClassIds + ") AND " +
                                                           "item.baseProdItemId > 0 AND item.baseProdItemId IS NOT NULL AND item.isActive=1 ";
                if (ProductTypeId > 0)
                {
                    cmdSelect.CommandText += " AND item.codeTypeId = " + ProductTypeId;
                }
                if (procurementId == (int)Constants.ProcurementTypeE.MAKE)
                {
                    cmdSelect.CommandText += " AND ISNULL(item.procurementId,0) =" + procurementId + " ";
                }
                if (procurementId == (int)Constants.ProcurementTypeE.BUY)
                {
                    cmdSelect.CommandText += " AND ISNULL(item.procurementId,0) =0  ";
                }
                if (isShowListed != (int)Constants.ItemMasterTypeE.ALL)
                {
                    //cmdSelect.CommandText += " AND ISNULL(item.IsNonListed,0) = "+ isShowListed;
                }
                if (!String.IsNullOrEmpty(warehouseIds))
                {
                    if (ShowStorLocWiseItem == 1)
                    {
                        cmdSelect.CommandText += " and item.idProdItem in (select ProductItemId from TblItemLinkedStoreLoc where StoreLocId = " + warehouseIds + " and isnull(IsActive,0)= 1)";
                    }
                }

                cmdSelect.CommandText += "GROUP BY " + GroupByText();
                cmdSelect.CommandText += " order by item.itemName";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProductItemTO> list = ConvertDTToList(reader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
                conn.Close();
            }
        }

        //chetan[04-April-2020] added for searching item
        public List<TblProductItemTO> SelectListOfProductItemTOOnSearchString(string searchString, Int32 ProductTypeId = 0)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();

                string whereCondStr = " where item.baseProdItemId > 0  AND item.baseProdItemId IS NOT NULL AND item.isActive=1 ";
                if (ProductTypeId > 0)
                {
                    whereCondStr += " AND item.codeTypeId = " + ProductTypeId;
                }
                TblConfigParamsTO tblConfigParamsTOItemSearch = _iTblConfigParamsDAO.SelectTblConfigParams(Constants.IS_SEARCH_ITEM_FILITER_WHERE_CONDITIONS);
                if (tblConfigParamsTOItemSearch != null)
                {
                    whereCondStr += tblConfigParamsTOItemSearch.ConfigParamVal;
                }
                // whereCondStr += " AND item.itemName like @ItemSearchStr or c.displayName like @ItemSearchStr or sc.displayName like @ItemSearchStr or p.displayName like @ItemSearchStr ";
                cmdSelect.CommandText = SqlSelectQuery() + whereCondStr;
                cmdSelect.CommandText += " GROUP BY " + GroupByText();
                cmdSelect.CommandText += " order by item.itemName";
                cmdSelect.Connection = conn;
                cmdSelect.Parameters.Add("@ItemSearchStr", System.Data.SqlDbType.NVarChar).Value = '%' + searchString + '%';

                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProductItemTO> list = ConvertDTToList(reader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
                conn.Close();
            }
        }

        //Sudhir[20-MAR-2018] Added for Get ProductItem List which has Parity Yes.
        public List<DropDownTO> SelectProductItemListIsParity(Int32 isParity)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE item.isParity=" + isParity;
                cmdSelect.CommandText += " GROUP BY " + GroupByText();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> list = new List<DropDownTO>();
                while (reader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (reader["idProdItem"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(reader["idProdItem"].ToString());
                    if (reader["itemName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(reader["itemName"].ToString());

                    list.Add(dropDownTONew);
                }
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
                conn.Close();
            }
        }
        public List<DropDownTO> GetProductItemDropDownList()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = " select idProdItem, itemName from tblProductItem where isActive = 1";
                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idProdItem"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idProdItem"].ToString());
                    if (dateReader["itemName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["itemName"].ToString());
                    dropDownTOList.Add(dropDownTONew);
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<DropDownTO> GetGradeDropDownList(Int32 specificationId = 0)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = " select idProdItem, itemName from tblProductItem WHERE isActive=1 AND prodClassId=" + specificationId;
                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idProdItem"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idProdItem"].ToString());
                    if (dateReader["itemName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["itemName"].ToString());
                    dropDownTOList.Add(dropDownTONew);
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public DropDownTO getCountOfListedAndNonListedItems(int nonlisted)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String aqlQuery = " SELECT count(*) as ItemCount FROM tblProductItem WHERE isActive = 1 AND baseProdItemId > 0 AND ISNULL(IsNonListed,0)= " + nonlisted;
                cmdSelect = new SqlCommand(aqlQuery, conn);
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<Models.DropDownTO>();
                while (dateReader.Read())
                {
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["ItemCount"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["ItemCount"].ToString());
                    dropDownTOList.Add(dropDownTONew);
                }
                return dropDownTOList[0];
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblProductItemTO> SelectAllItemListForExportToExcel(int materialListedType)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();

                string whereCondStr = " where item.baseProdItemId > 0  AND item.baseProdItemId IS NOT NULL AND item.isActive=1 ";

                if (materialListedType == 2)
                {
                    whereCondStr += " AND ISNULL(item.IsNonListed, 0) = 0 ";
                }
                if (materialListedType == 3)
                {
                    whereCondStr += " AND ISNULL(item.IsNonListed, 0) = 1 ";
                }
                cmdSelect.CommandText = SqlQueryForExportToExcel() + whereCondStr;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProductItemTO> list = ConvertDTToListForExport(reader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
                conn.Close();
            }
        }

        #endregion

        #region Insertion
        public int InsertTblProductItem(TblProductItemTO tblProductItemTO, TblPurchaseItemMasterTO tblPurchaseItemMasterTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblProductItemTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public int InsertTblProductItem(TblProductItemTO tblProductItemTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblProductItemTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public int ExecuteInsertionCommand(TblProductItemTO tblProductItemTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblProductItem]( " +
                            "  [prodClassId]" +
                            " ,[createdBy]" +
                            " ,[updatedBy]" +
                            " ,[createdOn]" +
                            " ,[updatedOn]" +
                            " ,[itemName]" +
                            " ,[itemDesc]" +
                            " ,[remark]" +
                            " ,[isActive]" +
                            " ,[weightMeasureUnitId]" +
                            " ,[conversionUnitOfMeasure]" +
                            " ,[conversionFactor]" +
                            ", [isStockRequire]" +
                            ", [isParity]" +
                            ", [basePrice]" +
                            ", [codeTypeId]" +
                             ",[isBaseItemForRate]" +
                             ",[IsNonCommercialItem]" +
                             ",[displaySequanceNo]" +
                             ",[priceListId]" +                  //Priyanka [24-04-2019]
                             ",[locationId]" +
                             ",[uOMGroupId]" +

                             ",[itemCategoryId]" +
                             ",[manageItemById]" +
                             ",[isGSTApplicable]" +
                             ",[manufacturerId]" +
                             ",[shippingTypeId]" +
                             ",[materialTypeId]" +
                             ",[isInventoryItem]" +
                             ",[isSalesItem]" +
                             ",[isPurchaseItem]" +
                             ",[warrantyTemplateId]" +
                             ",[mgmtMethodId]" +
                             ",[additionalIdent]" +
                             ",[hSNCode]" +
                             ",[sACCode]" +
                             ",[taxCategoryId]" +

                             ",[salesUOMId]" +
                             ",[itemPerSalesUnit]" +
                             ",[salesQtyPerPkg]" +
                             ",[salesLength]" +
                             ",[salesWidth]" +
                             ",[salesHeight]" +
                             ",[salesVolumeId]" +
                             ",[salesWeight]" +

                             ",[gLAccId]" +
                             ",[inventUOMId]" +
                             ",[inventWeight]" +
                             ",[reqPurchaseUOMId]" +
                             ",[inventMinimum]" +
                             ",[valuationId]" +
                             ",[itemCost]" +

                             ",[planningId]" +
                             ",[procurementId]" +
                             ",[compWareHouseId]" +
                             ",[minOrderQty]" +
                             ",[leadTime]" +
                             ",[toleranceDays]" +

                             ",[issueId]" +
                             ",[baseProdItemId]" +
                             ",[itemMakeId]" +
                             ",[itemBrandId]" +
                             ",[catLogNo]" +
                             ",[supItemCode]" +
                             ",[isDefaultMake]" +
                             ",[isImportedItem]" +
                             ",[makeSeries]" +
                             ",[isProperSAPItem]" +
                             ",[orderMultiple]" +
                             ",[gstCodeId]" +
                             ",[isTestCertificateCompulsary]" +
                             ",[isAllocationApplicable]" +
                              ",[isConsumable]" +
                               ",[isFixedAsset]" +
                               ",[bomTypeId]" +
                             ",[orignalProdItemId]" +
                               ", [IsNonListed]" +//Reshma

                                ", [finYearExpLedgerId]" +
                                ", refProdItemId " +
                                ", isScrapItem " +
                                 " ,scrapValuation " +
                               " ,scrapStoreLocationId" +
                               ",isDailyScrapReq " +
                               ", assetStoreLocationId" +
                               ", assetClassId " +
                               ", isManageInventory" +
                               ", rackNo" +
                               ", xBinLocation" +
                               ", yBinLocation" +
                               ", itemClassId" +

            " )" +
                " VALUES (" +
                            "  @ProdClassId " +
                            " ,@CreatedBy " +
                            " ,@UpdatedBy " +
                            " ,@CreatedOn " +
                            " ,@UpdatedOn " +
                            " ,@ItemName " +
                            " ,@ItemDesc " +
                            " ,@Remark " +
                            " ,@isActive " +
                            " ,@weightMeasureUnitId " +
                            " ,@conversionUnitOfMeasure " +
                            " ,@conversionFactor " +
                            " ,@isStockRequire" +
                            " ,@IsParity" +
                            ", @BasePrice" +
                            ", @CodeTypeId" +
                            ", @isBase" +
                            ", @nonCommertial" +
                            ", @DisplaySequanceNo" +
                            ", @PriceListId" +                      //Priyanka [24-04-2019]
                            ", @LocationId" +
                            ", @UOMGroupId" +


                            ", @ItemCategoryId" +
                            ", @ManageItemById" +
                            ", @IsGSTApplicable" +
                            ", @ManufacturerId" +
                            ", @ShippingTypeId" +
                            ", @MaterialTypeId" +
                            ", @IsInventoryItem" +
                            ", @IsSalesItem" +
                            ", @IsPurchaseItem" +
                            ", @WarrantyTemplateId" +
                            ", @MgmtMethodId" +
                            ", @AdditionalIdent" +
                            ", @HSNCode" +
                            ", @SACCode" +
                            ", @TaxCategoryId" +

                             ", @SalesUOMId" +
                            ", @ItemPerSalesUnit" +
                            ", @SalesQtyPerPkg" +
                            ", @SalesLength" +
                            ", @SalesWidth" +
                            ", @SalesHeight" +
                            ", @SalesVolumeId" +
                            ", @SalesWeight" +

                            ", @GLAccId" +
                            ", @InventUOMId" +
                            ", @InventWeight" +
                            ", @ReqPurchaseUOMId" +
                            ", @InventMinimum" +
                            ", @ValuationId" +
                            ", @ItemCost" +

                            ", @PlanningId" +
                            ", @ProcurementId" +
                            ", @CompWareHouseId" +
                            ", @MinOrderQty" +
                            ", @LeadTime" +
                            ", @ToleranceDays" +

                            ", @IssueId" +
                            ", @baseProdItemId" +
                            ", @itemMakeId" +
                            ", @itemBrandId" +
                            ", @catLogNo" +
                            ", @supItemCode" +
                            ", @isDefaultMake" +
                            ", @isImportedItem" +
                            ", @makeSeries" +
                            ", @IsProperSAPItem" +
                            ", @OrderMultiple" +
                            ", @GstCodeId " +
                            ", @IsTestCertificateCompulsary " +
                            ", @IsAllocationApplicable " +
                             ", @isConsumable " +
                              ", @isFixedAsset " +
                              ", @bomTypeId" +

                            ", @orignalProdItemId " +//orignalProdItemId

                              ", @isNonListed" +
                               ", @finYearExpLedgerId" +

                               " ,@refProdItemId " +
                               " ,@isScrapItem " +
                               " ,@scrapValuation " +
                               " ,@scrapStoreLocationId" +
                               ",@isDailyScrapReq " +
                               ", @assetStoreLocationId" +
                               ", @assetClassId " +
                               ", @IsManageInventory " +
                               ", @RackNo " +
                               ", @XBinLocation " +
                               ", @YBinLocation " +
                               ", @ItemClassId " +

            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdProdItem", System.Data.SqlDbType.Int).Value = tblProductItemTO.IdProdItem;
            cmdInsert.Parameters.Add("@ProdClassId", System.Data.SqlDbType.Int).Value = tblProductItemTO.ProdClassId;
            cmdInsert.Parameters.Add("@ItemClassId", System.Data.SqlDbType.Int).Value = tblProductItemTO.ItemClassId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblProductItemTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.UpdatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblProductItemTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.UpdatedOn);
            cmdInsert.Parameters.Add("@ItemName", System.Data.SqlDbType.NVarChar).Value = tblProductItemTO.ItemName;
            cmdInsert.Parameters.Add("@ItemDesc", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.ItemDesc);
            cmdInsert.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.Remark);
            cmdInsert.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value = tblProductItemTO.IsActive;
            cmdInsert.Parameters.Add("@weightMeasureUnitId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.WeightMeasureUnitId);
            cmdInsert.Parameters.Add("@conversionUnitOfMeasure", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.ConversionUnitOfMeasure);
            cmdInsert.Parameters.Add("@conversionFactor", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.ConversionFactor);
            cmdInsert.Parameters.Add("@isStockRequire", System.Data.SqlDbType.Int).Value = tblProductItemTO.IsStockRequire;
            cmdInsert.Parameters.Add("@isParity", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.IsParity);
            cmdInsert.Parameters.Add("@BasePrice", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.BasePrice);
            cmdInsert.Parameters.Add("@CodeTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.CodeTypeId);
            cmdInsert.Parameters.Add("@isBase", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.IsBaseItemForRate);
            cmdInsert.Parameters.Add("@nonCommertial", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.IsNonCommercialItem);
            cmdInsert.Parameters.Add("@DisplaySequanceNo", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.IsDisplaySequenceNo);
            cmdInsert.Parameters.Add("@PriceListId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.PriceListId);
            cmdInsert.Parameters.Add("@UOMGroupId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.UOMGroupId);
            cmdInsert.Parameters.Add("@LocationId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.LocationId);

            //General
            cmdInsert.Parameters.Add("@ItemCategoryId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.ItemCategoryId);
            cmdInsert.Parameters.Add("@ManageItemById", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.ManageItemById);
            cmdInsert.Parameters.Add("@IsGSTApplicable", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.IsGSTApplicable);
            cmdInsert.Parameters.Add("@ManufacturerId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.ManufacturerId);
            cmdInsert.Parameters.Add("@ShippingTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.ShippingTypeId);
            cmdInsert.Parameters.Add("@MaterialTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.MaterialTypeId);
            cmdInsert.Parameters.Add("@IsInventoryItem", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.IsInventoryItem);
            cmdInsert.Parameters.Add("@IsSalesItem", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.IsSalesItem);
            cmdInsert.Parameters.Add("@WarrantyTemplateId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.WarrantyTemplateId);
            cmdInsert.Parameters.Add("@MgmtMethodId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.MgmtMethodId);
            cmdInsert.Parameters.Add("@IsPurchaseItem", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.IsPurchaseItem);
            cmdInsert.Parameters.Add("@AdditionalIdent", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.AdditionalIdent);
            cmdInsert.Parameters.Add("@HSNCode", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.HSNCode);
            cmdInsert.Parameters.Add("@SACCode", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.SACCode);
            cmdInsert.Parameters.Add("@TaxCategoryId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.TaxCategoryId);
            cmdInsert.Parameters.Add("@IsTestCertificateCompulsary", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.IsTestCertificateCompulsary);
            cmdInsert.Parameters.Add("@IsAllocationApplicable", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.IsAllocationApplicable);

            //Sales
            cmdInsert.Parameters.Add("@SalesVolumeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.SalesVolumeId);
            cmdInsert.Parameters.Add("@SalesUOMId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.SalesUOMId);
            cmdInsert.Parameters.Add("@ItemPerSalesUnit", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.ItemPerSalesUnit);
            cmdInsert.Parameters.Add("@SalesLength", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.SalesLength);
            cmdInsert.Parameters.Add("@SalesWidth", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.SalesWidth);
            cmdInsert.Parameters.Add("@SalesHeight", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.SalesHeight);
            cmdInsert.Parameters.Add("@SalesWeight", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.SalesWeight);
            cmdInsert.Parameters.Add("@SalesQtyPerPkg", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.SalesQtyPerPkg);

            //Inventory
            cmdInsert.Parameters.Add("@GLAccId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.GLAccId);
            cmdInsert.Parameters.Add("@InventUOMId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.InventUOMId);
            cmdInsert.Parameters.Add("@InventWeight", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.InventWeight);
            cmdInsert.Parameters.Add("@ReqPurchaseUOMId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.ReqPurchaseUOMId);
            cmdInsert.Parameters.Add("@InventMinimum", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.InventMinimum);
            cmdInsert.Parameters.Add("@ValuationId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.ValuationId);
            cmdInsert.Parameters.Add("@ItemCost", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.ItemCost);

            //Planning data
            cmdInsert.Parameters.Add("@PlanningId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.PlanningId);
            cmdInsert.Parameters.Add("@ProcurementId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.ProcurementId);
            cmdInsert.Parameters.Add("@CompWareHouseId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.CompWareHouseId);
            cmdInsert.Parameters.Add("@MinOrderQty", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.MinOrderQty);
            cmdInsert.Parameters.Add("@LeadTime", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.LeadTime);
            cmdInsert.Parameters.Add("@ToleranceDays", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.ToleranceDays);

            //Production
            cmdInsert.Parameters.Add("@IssueId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.IssueId);

            //Sanjay [13-June-2019] Base and Make item identification
            cmdInsert.Parameters.Add("@baseProdItemId", System.Data.SqlDbType.BigInt).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.BaseProdItemId);

            cmdInsert.Parameters.Add("@itemMakeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.ItemMakeId);
            cmdInsert.Parameters.Add("@itemBrandId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.ItemBrandId);
            cmdInsert.Parameters.Add("@catLogNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.CatLogNo);
            cmdInsert.Parameters.Add("@supItemCode", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.SupItemCode);
            cmdInsert.Parameters.Add("@isDefaultMake", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.IsDefaultMake);
            cmdInsert.Parameters.Add("@isImportedItem", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.IsImportedItem);
            cmdInsert.Parameters.Add("@makeSeries", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.MakeSeries);
            cmdInsert.Parameters.Add("@IsProperSAPItem", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.IsProperSAPItem);
            cmdInsert.Parameters.Add("@OrderMultiple", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.OrderMultiple);
            cmdInsert.Parameters.Add("@GstCodeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.GstCodeId);
            cmdInsert.Parameters.Add("@orignalProdItemId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.OrignalProdItemId);


            cmdInsert.Parameters.Add("@isConsumable", System.Data.SqlDbType.Bit).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.IsConsumable);
            cmdInsert.Parameters.Add("@isFixedAsset", System.Data.SqlDbType.Bit).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.IsFixedAsset);

            cmdInsert.Parameters.Add("@bomTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.BomTypeId);

            cmdInsert.Parameters.Add("@isNonListed", System.Data.SqlDbType.Bit).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.IsNonListed);


            cmdInsert.Parameters.Add("@finYearExpLedgerId", System.Data.SqlDbType.BigInt).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.FinYearExpLedgerId);

            cmdInsert.Parameters.Add("@refProdItemId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.RefProdItemId);
            cmdInsert.Parameters.Add("@isScrapItem", System.Data.SqlDbType.Bit).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.IsScrapItem);

            cmdInsert.Parameters.Add("@scrapValuation", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.ScrapValuation);
            cmdInsert.Parameters.Add("@scrapStoreLocationId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.ScrapStoreLocationId);
            cmdInsert.Parameters.Add("@isDailyScrapReq", System.Data.SqlDbType.Bit).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.IsDailyScrapReq);

            //Fixed Asset
            cmdInsert.Parameters.Add("@assetStoreLocationId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.AssetStoreLocationId);
            cmdInsert.Parameters.Add("@assetClassId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.AssetClassId);

            //AmolG[2020-Dec-16] For Warehouse wise inventory management.
            cmdInsert.Parameters.Add("@IsManageInventory", System.Data.SqlDbType.Bit).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.IsManageInventory);//AmolG
            cmdInsert.Parameters.Add("@RackNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.RackNo);//AmolG
            cmdInsert.Parameters.Add("@XBinLocation", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.XBinLocation);//AmolG
            cmdInsert.Parameters.Add("@YBinLocation", System.Data.SqlDbType.VarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.YBinLocation);//AmolG

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblProductItemTO.IdProdItem = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }

            return 0;
        }
        #endregion
        public int InsertTblPurchaseItemMaster(TblPurchaseItemMasterTO tblPurchaseItemMasterTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommandForPurchaseItemMasterTO(tblPurchaseItemMasterTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }
        public int ExecuteInsertionCommandForPurchaseItemMasterTO(TblPurchaseItemMasterTO tblPurchaseItemMasterTO, SqlCommand cmdInsert)
        {

            String sqlQuery = @" INSERT INTO [tblProductItemPurchaseExt]( " +
                         "  [gstCodeTypeId]" +
                         " ,[createdBy]" +
                         " ,[updatedBy]" +
                         " ,[createdOn]" +
                         " ,[updatedOn]" +
                         " ,[isActive]" +
                         " ,[priority]" +
                         " ,[currencyId]" +
                         " ,[supplierOrgId]" +
                         " ,[currencyRate]" +
                         " ,[basicRate]" +
                         ", [gstItemCode]" +
                         ", [discount]" +
                         ", [codeTypeId]" +
                          ",[pf]" +
                          ",[freight]" +
                          ",[deliveryPeriodInDays]" +
                          ",[multiplicationFactor]" +
                          ",[minimumOrderQty]" +
                          ",[supplierAddress]" +
                          ",[mfgCatlogNo]" +
                          ",[weightMeasurUnitId]" +
                          ",[prodItemId]" +
                          ",[itemPerPurchaseUnit]" +
                          ",[lengthmm]" +
                          ",[widthmm]" +
                          ",[heightmm]" +
                          ",[volumeccm]" +
                          ",[weightkg]" +
                         //Priyanka [25-04-2019]
                         ",[purchaseUOMId]" +
                         ",[qtyPerPkg]" +
                         ",[volumeId]" +
                         ",[isPrioritySupplier]" +
                         //",[supplierAddr]" + 
                         ",[nlcCost] " +
                         ",[orderMultiple] " +
                         ",[supItemCode] " +
                         ",[priceListId] " +
                          " )" +
             " VALUES (" +
                         "  @GstCodeTypeId " +
                         " ,@CreatedBy " +
                         " ,@UpdatedBy " +
                         " ,@CreatedOn " +
                         " ,@UpdatedOn " +
                         " ,@isActive " +
                         " ,@Priority " +
                         " ,@CurrencyId " +
                         " ,@SupplierOrgId " +
                         " ,@Currency_Rate " +
                         " ,@Basic_Rate " +
                         " ,@Gst_Item_Code" +
                         ", @Discount" +
                         ", @CodeTypeId" +
                         ", @Pf" +
                         ", @Freight" +
                         ", @Delivery_Period_In_Days" +
                         ", @Multiplication_Factor" +
                         ", @Minimum_Order_Qty" +
                         ", @Supplier_Address" +
                         ", @Mfg_Catlog_No" +
                         ", @WeightMeasurUnitId" +
                         ", @ProdItemId" +
                         ", @Item_Per_Purchase_Unit" +
                         ", @length_mm" +
                         ", @width_mm" +
                         ", @height_mm" +
                         ", @volume_ccm" +
                         ", @weight_kg" +
                         ", @PurchaseUOMId" +
                         ", @QtyPerPkg" +
                         ", @VolumeId" +
                         ", @IsPrioritySupplier" +
                         //", @SupplierAddr" + //Saket [2019-09-10] Get address from master.
                         ", @NlcCost" +
                         ", @OrderMultiple" +
                         ", @SupItemCode" +
                         ", @PriceListId" +
                         " )";
            //Hudeakr Priyanka [22-feb-19]
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@GstCodeTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.GstCodeTypeId);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblPurchaseItemMasterTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.UpdatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblPurchaseItemMasterTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.UpdatedOn);
            cmdInsert.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.IsActive);

            cmdInsert.Parameters.Add("@OrderMultiple", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.OrderMultiple);
            cmdInsert.Parameters.Add("@SupItemCode", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.SupItemCode);
            cmdInsert.Parameters.Add("@PriceListId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.PriceListId);
            cmdInsert.Parameters.Add("@Priority", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.Priority);
            cmdInsert.Parameters.Add("@CurrencyId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.CurrencyId);

            cmdInsert.Parameters.Add("@SupplierOrgId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.SupplierOrgId);
            cmdInsert.Parameters.Add("@Currency_Rate", System.Data.SqlDbType.Decimal).Value = tblPurchaseItemMasterTO.CurrencyRate;
            cmdInsert.Parameters.Add("@Basic_Rate", System.Data.SqlDbType.Decimal).Value = tblPurchaseItemMasterTO.BasicRate;

            cmdInsert.Parameters.Add("@Gst_Item_Code", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.GSTItemCode);
            cmdInsert.Parameters.Add("@Discount", System.Data.SqlDbType.Decimal).Value = tblPurchaseItemMasterTO.Discount;
            cmdInsert.Parameters.Add("@CodeTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.CodeTypeId);
            cmdInsert.Parameters.Add("@Pf", System.Data.SqlDbType.Decimal).Value = tblPurchaseItemMasterTO.PF;
            cmdInsert.Parameters.Add("@Freight", System.Data.SqlDbType.Decimal).Value = tblPurchaseItemMasterTO.Freight;
            cmdInsert.Parameters.Add("@Delivery_Period_In_Days", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.DeliveryPeriodInDays);
            cmdInsert.Parameters.Add("@Multiplication_Factor", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.MultiplicationFactor);
            cmdInsert.Parameters.Add("@Minimum_Order_Qty", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.MinimumOrderQty);
            cmdInsert.Parameters.Add("@Supplier_Address", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.SupplierAddress);
            cmdInsert.Parameters.Add("@Mfg_Catlog_No", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.MfgCatlogNo);
            cmdInsert.Parameters.Add("@WeightMeasurUnitId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.WeightMeasurUnitId);
            cmdInsert.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.ProdItemId);
            cmdInsert.Parameters.Add("@Item_Per_Purchase_Unit", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.ItemPerPurchaseUnit);
            cmdInsert.Parameters.Add("@length_mm", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.Length_mm);
            cmdInsert.Parameters.Add("@width_mm", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.Width_mm);
            cmdInsert.Parameters.Add("@height_mm", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.Height_mm);
            cmdInsert.Parameters.Add("@volume_ccm", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.Volume_ccm);
            cmdInsert.Parameters.Add("@weight_kg", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.Weight_kg);
            cmdInsert.Parameters.Add("@purchaseUOMId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.PurchaseUOMId);
            cmdInsert.Parameters.Add("@volumeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.VolumeId);
            cmdInsert.Parameters.Add("@qtyPerPkg", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.QtyPerPkg);
            cmdInsert.Parameters.Add("@isPrioritySupplier", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.IsPrioritySupplier);
            //cmdInsert.Parameters.Add("@supplierAddr", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.SupplierAddr);  //Saket [2019-09-10] Get address from master.
            cmdInsert.Parameters.Add("@NlcCost", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.NlcCost);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                return 1;
            }

            return 0;
        }


        public int InsertTblProductItemBom(TblProductItemBomTO tblProductItemBomTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommandtblProductItemBom(tblProductItemBomTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public int ExecuteInsertionCommandtblProductItemBom(TblProductItemBomTO tblProductItemBomTO, SqlCommand cmdInsert)
        {

            String sqlQuery = @" INSERT INTO [tblProductItemBom]( " +
                "[parentProdItemId]" +
                ",[childProdItemId]" +
                ",[modelId]" +
                ",[qty]" +
                ",[createdBy]" +
                ",[isOptional]" +
                ",[createdOn]" +

                " )" +
            " VALUES (" +
                " @ParentProdItemId" +
                ", @ChildProdItemId" +
                ", @ModelId" +
                ", @Qty" +
                ", @CreatedBy" +
                ", @IsOptional" +
                ", @CreatedOn" +

            " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            String sqlSelectIdentityQry = "Select @@Identity";
            cmdInsert.Parameters.Add("@ParentProdItemId", System.Data.SqlDbType.Int).Value = tblProductItemBomTO.ParentProdItemId;
            cmdInsert.Parameters.Add("@ChildProdItemId", System.Data.SqlDbType.Int).Value = tblProductItemBomTO.ChildProdItemId;
            cmdInsert.Parameters.Add("@ModelId", System.Data.SqlDbType.Int).Value = tblProductItemBomTO.ModelId;
            cmdInsert.Parameters.Add("@Qty", System.Data.SqlDbType.Decimal).Value = tblProductItemBomTO.Qty;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblProductItemBomTO.CreatedBy;
            cmdInsert.Parameters.Add("@IsOptional", System.Data.SqlDbType.Int).Value = tblProductItemBomTO.IsOptional;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblProductItemBomTO.CreatedOn;

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = sqlSelectIdentityQry;
                tblProductItemBomTO.IdBomTree = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }


        #region Updation

        //remove previous Base Items while adding or updating new
        public int updatePreviousBase(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                String sqlQuery = @" UPDATE [tblProductItem] SET " +
                                " [isBaseItemForRate] = " + 0 +                //Priyanka [16-05-2018] 
                                " WHERE [isBaseItemForRate] = " + 1;

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                return cmdUpdate.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public int UpdateTblProductItem(TblProductItemTO tblProductItemTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblProductItemTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public int UpdateTblProductItem(TblProductItemTO tblProductItemTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblProductItemTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }


        public int UpdateHSNCode(Double HSNCode, Int32 idProdItem, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationHSNCodeCommand(HSNCode, idProdItem, cmdUpdate);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public int ExecuteUpdationHSNCodeCommand(Double HSNCode, Int32 idProdItem, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblProductItem] SET " +
                                " [hSNCode] = " + HSNCode +                //Priyanka [16-05-2018] 
                                " WHERE [idProdItem] IN (" + idProdItem + ")";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@HSNCode", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(HSNCode);
            return cmdUpdate.ExecuteNonQuery();
        }
        public List<TblProductItemTO> findItemOfBaseItem(Int32 prodItemId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                if (prodItemId != 0)
                    cmdSelect.CommandText = "select idProdItem,hSNCode from tblProductItem where baseProdItemId = " + prodItemId + " and isActive = 1";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProductItemTO> itemlist = ConvertDTToListForUpdateHsn(reader);

                return itemlist;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                // conn.Close();
                cmdSelect.Dispose();
            }
        }
        //Priyanka [22-05-2018] : Added to update  product item tax type.
        public int UpdateTblProductItemTaxType(String idClassStr, Int32 codeTypeId, SqlConnection conn, SqlTransaction tran)
        {

            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdateTblProductItemTaxType(idClassStr, codeTypeId, cmdUpdate);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public int ExecuteUpdationCommand(TblProductItemTO tblProductItemTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblProductItem] SET " +
                                "  [prodClassId]= @ProdClassId" +
                                " ,[updatedBy]= @UpdatedBy" +
                                " ,[updatedOn]= @UpdatedOn" +
                                " ,[itemName]= @ItemName" +
                                " ,[itemDesc]= @ItemDesc" +
                                " ,[remark] = @Remark" +
                                " ,[isActive] = @isActive" +
                                " ,[weightMeasureUnitId] = @weightMeasureUnitId " +
                                " ,[conversionUnitOfMeasure] = @conversionUnitOfMeasure " +
                                " ,[conversionFactor] = @conversionFactor " +
                                " ,[isStockRequire] = @isStockRequire " +
                                " ,[isParity] = @IsParity " +
                                " ,[basePrice] = @BasePrice " +
                                " ,[codeTypeId] = @CodeTypeId" +                //Priyanka [16-05-2018] 
                                " ,[isBaseItemForRate] = @isBase" +
                                " ,[isNonCommercialItem] = @nonCommertial" +
                                " ,[displaySequanceNo] = @DisplaySequanceNo" +
                                " ,[priceListId] = @PriceListId" +               //PRiyanka [24-04-2019]
                                " ,[uOMGroupId] = @UOMGroupId" +
                                " ,[locationId] = @LocationId" +

                                " ,[itemCategoryId]= @ItemCategoryId" +
                                " ,[manageItemById] =@ManageItemById" +
                                " ,[additionalIdent] =@AdditionalIdent" +
                                " ,[isGSTApplicable] =@IsGSTApplicable" +
                                " ,[manufacturerId] =@ManufacturerId" +
                                " ,[shippingTypeId] =@ShippingTypeId" +
                                " ,[materialTypeId] =@MaterialTypeId" +
                                " ,[hSNCode] =@HSNCode" +
                                " ,[sACCode] =@SACCode" +
                                " ,[taxCategoryId] =@TaxCategoryId" +

                                " ,[isInventoryItem] =@IsInventoryItem" +
                                " ,[isSalesItem] =@IsSalesItem" +
                                " ,[isPurchaseItem]= @IsPurchaseItem" +
                                " ,[warrantyTemplateId]= @WarrantyTemplateId" +
                                " ,[mgmtMethodId] = @MgmtMethodId" +

                                " ,[salesUOMId] = @SalesUOMId" +
                                " ,[itemPerSalesUnit] = @ItemPerSalesUnit" +
                                " ,[salesQtyPerPkg] = @SalesQtyPerPkg" +
                                " ,[salesLength] = @SalesLength" +
                                " ,[salesWidth] = @SalesWidth" +
                                " ,[salesHeight] = @SalesHeight " +
                                " ,[salesVolumeId] =@SalesVolumeId" +
                                " ,[salesWeight] = @SalesWeight" +

                                " ,[gLAccId] =@GLAccId" +
                                " ,[inventUOMId] =@InventUOMId" +
                                " ,[inventWeight] =@InventWeight" +
                                " ,[reqPurchaseUOMId] =@ReqPurchaseUOMId" +
                                " ,[inventMinimum] =@InventMinimum" +
                                " ,[valuationId]= @ValuationId" +
                                " ,[itemCost] =@ItemCost" +

                                " ,[planningId] =@PlanningId" +
                                " ,[procurementId] =@ProcurementId" +
                                " ,[compWareHouseId] =@CompWareHouseId" +
                                " ,[minOrderQty]= @MinOrderQty" +
                                " ,[leadTime] =@LeadTime" +
                                " ,[toleranceDays] =@ToleranceDays" +

                                " ,[issueId] =@IssueId" +
                                " ,[baseProdItemId] =@baseProdItemId" +
                                " ,[itemMakeId] =@itemMakeId" +
                                " ,[itemBrandId] =@itemBrandId" +
                                " ,[catLogNo] =@catLogNo" +
                                " ,[supItemCode] =@supItemCode" +
                                " ,[isDefaultMake] =@isDefaultMake" +
                                " ,[isImportedItem] =@isImportedItem" +
                                " ,[makeSeries] =@makeSeries" +
                                " ,[isProperSAPItem] = @IsProperSAPItem" +
                                " ,[orderMultiple] = @OrderMultiple" +
                                " ,[gstCodeId] = @GstCodeId" +
                                " ,[isTestCertificateCompulsary] = @IsTestCertificateCompulsary" +
                                " ,[isAllocationApplicable] = @IsAllocationApplicable" +
                                " ,[isConsumable] = @isConsumable" +
                                " ,[isFixedAsset] = @isFixedAsset" +
                                " ,[isNonListed] =@isNonListed" +
                                 " ,[finYearExpLedgerId] =@finYearExpLedgerId" +



                                ",isDailyScrapReq=@isDailyScrapReq" +//Reshma added
                                ",scrapValuation=@scrapValuation" +
                                ",scrapStoreLocationId=@scrapStoreLocationId" +
                                ",assetStoreLocationId =@assetStoreLocationId " +
                                ",assetClassId=@assetClassId " +
                                " ,refProdItemId=@refProdItemId" +
                                " ,bomTypeId =@bomTypeId " +
                                " ,isManageInventory =@IsManageInventory" +//AmolG  
                                " ,rackNo = @RackNo" +//AmolG  
                                " ,xBinLocation = @XBinLocation" +//AmolG 
                                " ,yBinLocation = @YBinLocation" +//AmolG  
                                " ,itemClassId = @ItemClassId" +//Deepali  
                                 " ,isMigration = 0" +//Deepali  

            " WHERE [idProdItem] = @IdProdItem ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdProdItem", System.Data.SqlDbType.Int).Value = tblProductItemTO.IdProdItem;
            cmdUpdate.Parameters.Add("@ProdClassId", System.Data.SqlDbType.Int).Value = tblProductItemTO.ProdClassId;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblProductItemTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblProductItemTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@ItemName", System.Data.SqlDbType.NVarChar).Value = tblProductItemTO.ItemName;
            cmdUpdate.Parameters.Add("@ItemDesc", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.ItemDesc);
            cmdUpdate.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.Remark);
            cmdUpdate.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value = tblProductItemTO.IsActive;
            cmdUpdate.Parameters.Add("@weightMeasureUnitId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.WeightMeasureUnitId);
            cmdUpdate.Parameters.Add("@conversionUnitOfMeasure", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.ConversionUnitOfMeasure);
            cmdUpdate.Parameters.Add("@conversionFactor", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.ConversionFactor);
            cmdUpdate.Parameters.Add("@isStockRequire", System.Data.SqlDbType.Int).Value = tblProductItemTO.IsStockRequire;
            cmdUpdate.Parameters.Add("@IsParity", System.Data.SqlDbType.Int).Value = tblProductItemTO.IsParity;
            cmdUpdate.Parameters.Add("@BasePrice", System.Data.SqlDbType.Decimal).Value = tblProductItemTO.BasePrice;
            cmdUpdate.Parameters.Add("@CodeTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.CodeTypeId);             //Priyanka [16-05-2018]
            cmdUpdate.Parameters.Add("@isBase", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.IsBaseItemForRate);
            cmdUpdate.Parameters.Add("@nonCommertial", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.IsNonCommercialItem);
            cmdUpdate.Parameters.Add("@DisplaySequanceNo", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.IsDisplaySequenceNo);
            cmdUpdate.Parameters.Add("@PriceListId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.PriceListId);            //Priyanka [24-04-2019]
            cmdUpdate.Parameters.Add("@UOMGroupId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.UOMGroupId);
            cmdUpdate.Parameters.Add("@LocationId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.LocationId);

            //General
            cmdUpdate.Parameters.Add("@ItemCategoryId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.ItemCategoryId);
            cmdUpdate.Parameters.Add("@ManageItemById", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.ManageItemById);
            cmdUpdate.Parameters.Add("@IsGSTApplicable", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.IsGSTApplicable);
            cmdUpdate.Parameters.Add("@ManufacturerId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.ManufacturerId);
            cmdUpdate.Parameters.Add("@ShippingTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.ShippingTypeId);
            cmdUpdate.Parameters.Add("@MaterialTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.MaterialTypeId);
            cmdUpdate.Parameters.Add("@IsInventoryItem", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.IsInventoryItem);
            cmdUpdate.Parameters.Add("@IsSalesItem", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.IsSalesItem);
            cmdUpdate.Parameters.Add("@WarrantyTemplateId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.WarrantyTemplateId);
            cmdUpdate.Parameters.Add("@MgmtMethodId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.MgmtMethodId);
            cmdUpdate.Parameters.Add("@IsPurchaseItem", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.IsPurchaseItem);
            cmdUpdate.Parameters.Add("@AdditionalIdent", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.AdditionalIdent);
            cmdUpdate.Parameters.Add("@HSNCode", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.HSNCode);
            cmdUpdate.Parameters.Add("@SACCode", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.SACCode);
            cmdUpdate.Parameters.Add("@TaxCategoryId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.TaxCategoryId);             //Priyanka [21-06-2019]
            cmdUpdate.Parameters.Add("@IsTestCertificateCompulsary", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.IsTestCertificateCompulsary);             //Priyanka [21-06-2019]
            cmdUpdate.Parameters.Add("@IsAllocationApplicable", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.IsAllocationApplicable);             //Priyanka [21-06-2019]

            //Sales
            cmdUpdate.Parameters.Add("@SalesVolumeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.SalesVolumeId);
            cmdUpdate.Parameters.Add("@SalesUOMId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.SalesUOMId);
            cmdUpdate.Parameters.Add("@ItemPerSalesUnit", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.ItemPerSalesUnit);
            cmdUpdate.Parameters.Add("@SalesLength", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.SalesLength);
            cmdUpdate.Parameters.Add("@SalesWidth", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.SalesWidth);
            cmdUpdate.Parameters.Add("@SalesHeight", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.SalesHeight);
            cmdUpdate.Parameters.Add("@SalesWeight", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.SalesWeight);
            cmdUpdate.Parameters.Add("@SalesQtyPerPkg", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.SalesQtyPerPkg);

            //Inventory
            cmdUpdate.Parameters.Add("@GLAccId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.GLAccId);
            cmdUpdate.Parameters.Add("@InventUOMId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.InventUOMId);
            cmdUpdate.Parameters.Add("@InventWeight", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.InventWeight);
            cmdUpdate.Parameters.Add("@ReqPurchaseUOMId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.ReqPurchaseUOMId);
            cmdUpdate.Parameters.Add("@InventMinimum", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.InventMinimum);
            cmdUpdate.Parameters.Add("@ValuationId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.ValuationId);
            cmdUpdate.Parameters.Add("@ItemCost", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.ItemCost);

            //Planning data
            cmdUpdate.Parameters.Add("@PlanningId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.PlanningId);
            cmdUpdate.Parameters.Add("@ProcurementId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.ProcurementId);
            cmdUpdate.Parameters.Add("@CompWareHouseId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.CompWareHouseId);
            cmdUpdate.Parameters.Add("@MinOrderQty", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.MinOrderQty);
            cmdUpdate.Parameters.Add("@LeadTime", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.LeadTime);
            cmdUpdate.Parameters.Add("@ToleranceDays", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.ToleranceDays);

            //Production
            cmdUpdate.Parameters.Add("@IssueId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.IssueId);

            cmdUpdate.Parameters.Add("@baseProdItemId", System.Data.SqlDbType.BigInt).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.BaseProdItemId);

            cmdUpdate.Parameters.Add("@itemMakeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.ItemMakeId);
            cmdUpdate.Parameters.Add("@itemBrandId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.ItemBrandId);
            cmdUpdate.Parameters.Add("@catLogNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.CatLogNo);
            cmdUpdate.Parameters.Add("@supItemCode", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.SupItemCode);
            cmdUpdate.Parameters.Add("@isDefaultMake", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.IsDefaultMake);
            cmdUpdate.Parameters.Add("@isImportedItem", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.IsImportedItem);
            cmdUpdate.Parameters.Add("@makeSeries", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.MakeSeries);
            cmdUpdate.Parameters.Add("@IsProperSAPItem", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.IsProperSAPItem);
            cmdUpdate.Parameters.Add("@OrderMultiple", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.OrderMultiple);
            cmdUpdate.Parameters.Add("@GstCodeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.GstCodeId);
            cmdUpdate.Parameters.Add("@isConsumable", System.Data.SqlDbType.Bit).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.IsConsumable);
            cmdUpdate.Parameters.Add("@isFixedAsset", System.Data.SqlDbType.Bit).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.IsFixedAsset);
            cmdUpdate.Parameters.Add("@isNonListed", System.Data.SqlDbType.Bit).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.IsNonListed);
            cmdUpdate.Parameters.Add("@finYearExpLedgerId", System.Data.SqlDbType.BigInt).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.FinYearExpLedgerId);



            cmdUpdate.Parameters.Add("@isDailyScrapReq", System.Data.SqlDbType.Bit).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.IsDailyScrapReq);
            cmdUpdate.Parameters.Add("@scrapValuation", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.ScrapValuation);
            cmdUpdate.Parameters.Add("@scrapStoreLocationId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.ScrapStoreLocationId);

            //Fixed Asset
            cmdUpdate.Parameters.Add("@assetStoreLocationId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.AssetStoreLocationId);
            cmdUpdate.Parameters.Add("@assetClassId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.AssetClassId);
            cmdUpdate.Parameters.Add("@refProdItemId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.RefProdItemId);

            cmdUpdate.Parameters.Add("@bomTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.BomTypeId);
            cmdUpdate.Parameters.Add("@IsManageInventory", System.Data.SqlDbType.Bit).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.IsManageInventory);

            cmdUpdate.Parameters.Add("@RackNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.RackNo);//AmolG
            cmdUpdate.Parameters.Add("@XBinLocation", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.XBinLocation);//AmolG
            cmdUpdate.Parameters.Add("@YBinLocation", System.Data.SqlDbType.VarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.YBinLocation);//AmolG
            cmdUpdate.Parameters.Add("@ItemClassId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductItemTO.ItemClassId);//Deepali

            return cmdUpdate.ExecuteNonQuery();
        }
        public int ExecuteUpdationItemLinkedStoreLoc(Int32 idProdItem, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [TblItemLinkedStoreLoc] SET " +
                                "  [IsActive]= 0" +
            " WHERE [ProductItemId] = @IdProdItem ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdProdItem", System.Data.SqlDbType.Int).Value = idProdItem;


            return cmdUpdate.ExecuteNonQuery();
        }

        public int ExecuteUpdateTblProductItemTaxType(String idClassStr, Int32 codeTypeId, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblProductItem] SET " +
                                " [codeTypeId] = " + codeTypeId +                //Priyanka [16-05-2018] 
                                " WHERE [prodClassId] IN (" + idClassStr + ")";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@CodeTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(codeTypeId);             //Priyanka [16-05-2018]
            return cmdUpdate.ExecuteNonQuery();
        }

        //@ Hudekar Priyanka [04-march-19]
        public int UpdateTblPurchaseItemMasterTO(TblPurchaseItemMasterTO tblPurchaseItemMasterTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                //if(tblPurchaseItemMasterTO.IdPurchaseItemMaster==0 && tblPurchaseItemMasterTO.IsAddNewPurchase==0)
                //{
                //    return ExecuteDeactivateCommandForTblPurchaseItemMasterTO(tblPurchaseItemMasterTO, cmdUpdate);
                //}
                return ExecuteUpdationCommandForTblPurchaseItemMasterTO(tblPurchaseItemMasterTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }
        //public int ExecuteDeactivateCommandForTblPurchaseItemMasterTO(TblPurchaseItemMasterTO tblPurchaseItemMasterTO, SqlCommand cmdDeactivate)
        //{
        //    String sqlQuery = @" UPDATE [tblProductItemPurchaseExt] SET " +
        //                      " [prodItemId] = @prodItemId" +
        //                      " WHERE [idPurchaseItemMaster] = @idPurchaseItemMaster ";

        //    cmdDeactivate.CommandText = sqlQuery;

        //    cmdDeactivate.CommandType = System.Data.CommandType.Text;

        //    return cmdDeactivate.ExecuteNonQuery();
        //}

        //Reshma For Deactive supplier taging
        public int UpdateTblPurchaseItemMasterTOAfterRevision(TblPurchaseItemMasterTO tblPurchaseItemMasterTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                String sqlQuery = @" UPDATE [tblProductItemPurchaseExt] SET" +
                                " [UpdatedBy]= @UpdatedBy " +
                                " ,[updatedOn]= @UpdatedOn " +
                                " ,[isActive] = @isActive " +
                                " WHERE [idPurchaseItemMaster]=@idPurchaseItemMaster ";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@idPurchaseItemMaster", System.Data.SqlDbType.Int).Value = tblPurchaseItemMasterTO.IdPurchaseItemMaster;
                cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblPurchaseItemMasterTO.UpdatedBy;
                cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblPurchaseItemMasterTO.UpdatedOn;
                cmdUpdate.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value = tblPurchaseItemMasterTO.IsActive;


                return cmdUpdate.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }
        public int DeactivateTblPurchaseItemMaster(int prodItemId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;

                String sqlQuery = @" UPDATE [tblProductItemPurchaseExt] SET " +
                                " [isActive] = 0" +
                                " WHERE [prodItemId] = @prodItemId ";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;
                cmdUpdate.Parameters.AddWithValue("@prodItemId", DbType.Int32).Value = prodItemId;
                //  cmdUpdate.Parameters.AddWithValue("@idPurchaseItemMaster", DbType.Int32).Value = _idPurchaseItemMaster;

                return cmdUpdate.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }
        public int DeactivateTblProdctItemMaster(int prodItemId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;

                String sqlQuery = @" UPDATE [tblProductItem] SET " +
                                " [isActive] = 0" +
                                " WHERE [idProdItem] = @idProdItem ";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;
                cmdUpdate.Parameters.AddWithValue("@idProdItem", DbType.Int32).Value = prodItemId;
                return cmdUpdate.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        //@  Hudekar Priyanka [04-march-2019]
        public int ExecuteUpdationCommandForTblPurchaseItemMasterTO(TblPurchaseItemMasterTO tblPurchaseItemMasterTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblProductItemPurchaseExt] SET " +
                                "  [gstCodeTypeId]= @GstCodeTypeId" +
                                " ,[createdBy]= @CreatedBy " +
                                " ,[updatedBy]= @UpdatedBy " +
                                " ,[createdOn]= @CreatedOn " +
                                " ,[updatedOn]= @UpdatedOn " +
                                " ,[isActive] = @isActive " +
                                " ,[priority] = @Priority " +
                                " ,[currencyId] = @CurrencyId  " +
                                " ,[supplierOrgId] = @SupplierOrgId  " +
                                " ,[currencyRate] = @Currency_Rate" +
                                " ,[basicRate] = @Basic_Rate" +
                                " ,[gstItemCode] = @Gst_Item_Code" +
                                " ,[discount] = @Discount " +
                                " ,[codeTypeId] = @CodeTypeId" +
                                " ,[pf] = @Pf" +
                                " ,[freight] = @Freight" +
                                " ,[deliveryPeriodInDays] = @Delivery_Period_In_Days" +
                                " ,[multiplicationFactor] = @Multiplication_Factor " +
                                " ,[minimumOrderQty] = @Minimum_Order_Qty" +
                                " ,[supplierAddress] = @Supplier_Address" +
                                " ,[mfgCatlogNo] = @Mfg_Catlog_No" +
                                " ,[weightMeasurUnitId] = @WeightMeasurUnitId" +
                                " ,[prodItemId] = @ProdItemId " +
                                " ,[itemPerPurchaseUnit] = @Item_Per_Purchase_Unit" +
                                " ,[lengthmm] = @length_mm" +
                                " ,[widthmm] = @width_mm" +
                                " ,[heightmm] = @height_mm" +
                                " ,[volumeccm] = @volume_ccm" +
                                " ,[weightkg] = @weight_kg" +
                                ",[purchaseUOMId] =@PurchaseUOMId" +
                                ",[qtyPerPkg] = @QtyPerPkg" +
                                ",[volumeId] =@VolumeId" +
                                ",[isPrioritySupplier] = @IsPrioritySupplier" +
                                //",[supplierAddr] = @SupplierAddr" +
                                ",[nlcCost] = @NlcCost" +
                                ",[orderMultiple] = @OrderMultiple" +
                                ",[SupItemCode] = @SupItemCode" +
                                ",[PriceListId] = @PriceListId" +

                                " WHERE [idPurchaseItemMaster] = @IdPurchaseItemMaster";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;
            cmdUpdate.Parameters.Add("@OrderMultiple", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.OrderMultiple);
            cmdUpdate.Parameters.Add("@SupItemCode", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.SupItemCode);
            cmdUpdate.Parameters.Add("@PriceListId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.PriceListId);

            cmdUpdate.Parameters.Add("@IdPurchaseItemMaster", System.Data.SqlDbType.Int).Value = tblPurchaseItemMasterTO.IdPurchaseItemMaster;

            cmdUpdate.Parameters.Add("@GstCodeTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.GstCodeTypeId);
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.CreatedBy);
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblPurchaseItemMasterTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.CreatedOn);
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblPurchaseItemMasterTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value = tblPurchaseItemMasterTO.IsActive;

            cmdUpdate.Parameters.Add("@Priority", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.Priority);
            cmdUpdate.Parameters.Add("@CurrencyId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.CurrencyId);

            cmdUpdate.Parameters.Add("@SupplierOrgId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.SupplierOrgId);
            cmdUpdate.Parameters.Add("@Currency_Rate", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.CurrencyRate);
            cmdUpdate.Parameters.Add("@Basic_Rate", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.BasicRate);

            cmdUpdate.Parameters.Add("@Gst_Item_Code", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.GSTItemCode);
            cmdUpdate.Parameters.Add("@Discount", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.Discount);
            cmdUpdate.Parameters.Add("@CodeTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.CodeTypeId);
            cmdUpdate.Parameters.Add("@Pf", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.PF);
            cmdUpdate.Parameters.Add("@Freight", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.Freight);
            cmdUpdate.Parameters.Add("@Delivery_Period_In_Days", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.DeliveryPeriodInDays);
            cmdUpdate.Parameters.Add("@Multiplication_Factor", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.MultiplicationFactor);
            cmdUpdate.Parameters.Add("@Minimum_Order_Qty", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.MinimumOrderQty);
            cmdUpdate.Parameters.Add("@Supplier_Address", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.SupplierAddress);
            cmdUpdate.Parameters.Add("@Mfg_Catlog_No", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.MfgCatlogNo);
            cmdUpdate.Parameters.Add("@WeightMeasurUnitId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.WeightMeasurUnitId);
            cmdUpdate.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.ProdItemId);
            cmdUpdate.Parameters.Add("@Item_Per_Purchase_Unit", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.ItemPerPurchaseUnit);
            cmdUpdate.Parameters.Add("@length_mm", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.Length_mm);
            cmdUpdate.Parameters.Add("@width_mm", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.Width_mm);
            cmdUpdate.Parameters.Add("@height_mm", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.Height_mm);
            cmdUpdate.Parameters.Add("@volume_ccm", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.Volume_ccm);
            cmdUpdate.Parameters.Add("@weight_kg", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.Weight_kg);
            cmdUpdate.Parameters.Add("@PurchaseUOMId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.PurchaseUOMId);
            cmdUpdate.Parameters.Add("@VolumeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.VolumeId);
            cmdUpdate.Parameters.Add("@QtyPerPkg", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.QtyPerPkg);
            cmdUpdate.Parameters.Add("@IsPrioritySupplier", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.IsPrioritySupplier);
            //cmdUpdate.Parameters.Add("@SupplierAddr", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.SupplierAddr);
            cmdUpdate.Parameters.Add("@NlcCost", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.NlcCost);

            return cmdUpdate.ExecuteNonQuery();
        }
        //@Hudekar Priyanka [04-march-19]
        public TblPurchaseItemMasterTO SelectTblPurchaseItemMaster(Int32 prodItemId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQueryForTblPurchaseItemMaster() + " WHERE tblprodItemPurchaseExt.prodItemId =" + prodItemId + " AND tblprodItemPurchaseExt.priority = 1 AND tblprodItemPurchaseExt.isActive =1";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@prodItemId", SqlDbType.Int).Value = prodItemId;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseItemMasterTO> tblPurchaseItemMasterData = ConvertDTToListForTblPurchaseItemMaster(reader);
                if (tblPurchaseItemMasterData != null && tblPurchaseItemMasterData.Count != 0)
                    return tblPurchaseItemMasterData[0];

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public List<TblPurchaseItemMasterTO> SelectProductItemPurchaseDataAllList(Int32 prodItemId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQueryForTblPurchaseItemMaster() + " WHERE tblprodItemPurchaseExt.prodItemId =" + prodItemId + " AND tblprodItemPurchaseExt.isActive =1  ORDER BY tblprodItemPurchaseExt.priority ASC";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@prodItemId", SqlDbType.Int).Value = prodItemId;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblPurchaseItemMasterTO> tblPurchaseItemMasterData = ConvertDTToListForTblPurchaseItemMaster(reader);
                if (tblPurchaseItemMasterData != null && tblPurchaseItemMasterData.Count != 0)
                    return tblPurchaseItemMasterData;

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public List<StoresLocationTO> SelectItemStoreLocList(Int32 prodItemId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select ILSL.StoreLocId as IdLocation ,Loc.locationDesc as LocationDesc from tblItemLinkedStoreLoc ILSL inner join tblLocation Loc on Loc.idLocation =ILSL.StoreLocId " +
                                        " inner join tblProductItem TPI on ILSL.ProductItemId = TPI.idProdItem " +
                                        " where ILSL.ProductItemId =" + prodItemId + " and isnull(ILSL.isActive,0)= 1 ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@prodItemId", SqlDbType.Int).Value = prodItemId;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<StoresLocationTO> tblLinkStoreLocData = ConvertDTToListForTblItemLinkedStoreLoc(reader);
                if (tblLinkStoreLocData != null && tblLinkStoreLocData.Count != 0)
                    return tblLinkStoreLocData;

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public String SqlSelectQueryForTblPurchaseItemMaster()
        {
            //String sqlSelectQry = " SELECT * FROM [tblProductItemPurchaseExt] prodItemPurchaseClass " +
            //                       " LEFT JOIN tblProductItem itemProdCatg ON itemProdCatg.idProdItem = prodItemPurchaseClass.ProdItemId" +
            //                      ""
            String sqlSelectQry = "SELECT tblprodItemPurchaseExt.*, tblOrg.firmName as supplierName ,tblAddr.*, dimTaluka.talukaName,dimDistrict.districtName, dimState.stateName " +
                " FROM tblProductItemPurchaseExt tblprodItemPurchaseExt" +
                " LEFT JOIN tblOrganization tblOrg ON tblOrg.idOrganization = tblprodItemPurchaseExt.supplierOrgId " +
                //" LEFT JOIN tblOrgAddress tblOrgAddr ON tblOrgAddr.organizationId = tblOrg.idOrganization " +
                " LEFT JOIN tblAddress tblAddr ON tblAddr.idAddr = tblOrg.addrId " +
                " LEFT JOIN dimTaluka dimTaluka ON dimTaluka.idTaluka = tblAddr.talukaId " +
                " LEFT JOIN dimDistrict dimDistrict  ON dimDistrict.idDistrict = tblAddr.districtId " +
                " LEFT JOIN dimState dimState  ON dimState.idState = tblAddr.stateId ";

            return sqlSelectQry;
        }
        public List<TblPurchaseItemMasterTO> ConvertDTToListForTblPurchaseItemMaster(SqlDataReader tblPurchaseItemMasterTODT)
        {
            List<TblPurchaseItemMasterTO> tblPurchaseItemMasterTORow = new List<TblPurchaseItemMasterTO>();
            if (tblPurchaseItemMasterTODT != null)
            {
                while (tblPurchaseItemMasterTODT.Read())
                {
                    TblPurchaseItemMasterTO tblPurchaseItemMasterTONew = new TblPurchaseItemMasterTO();
                    if (tblPurchaseItemMasterTODT["idPurchaseItemMaster"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.IdPurchaseItemMaster = Convert.ToInt32(tblPurchaseItemMasterTODT["idPurchaseItemMaster"].ToString());
                    if (tblPurchaseItemMasterTODT["priority"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.Priority = Convert.ToDecimal(tblPurchaseItemMasterTODT["priority"].ToString());

                    if (tblPurchaseItemMasterTODT["createdBy"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.CreatedBy = Convert.ToInt32(tblPurchaseItemMasterTODT["createdBy"].ToString());
                    if (tblPurchaseItemMasterTODT["updatedBy"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.UpdatedBy = Convert.ToInt32(tblPurchaseItemMasterTODT["updatedBy"].ToString());
                    if (tblPurchaseItemMasterTODT["createdOn"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.CreatedOn = Convert.ToDateTime(tblPurchaseItemMasterTODT["createdOn"].ToString());
                    if (tblPurchaseItemMasterTODT["updatedOn"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.UpdatedOn = Convert.ToDateTime(tblPurchaseItemMasterTODT["updatedOn"].ToString());
                    if (tblPurchaseItemMasterTODT["isActive"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.IsActive = Convert.ToInt32(tblPurchaseItemMasterTODT["isActive"].ToString());

                    if (tblPurchaseItemMasterTODT["currencyId"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.CurrencyId = Convert.ToInt32(tblPurchaseItemMasterTODT["currencyId"].ToString());

                    if (tblPurchaseItemMasterTODT["supplierOrgId"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.SupplierOrgId = Convert.ToInt32(tblPurchaseItemMasterTODT["supplierOrgId"].ToString());

                    if (tblPurchaseItemMasterTODT["gstCodeTypeId"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.GstCodeTypeId = Convert.ToInt32(tblPurchaseItemMasterTODT["gstCodeTypeId"].ToString());

                    if (tblPurchaseItemMasterTODT["currencyRate"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.CurrencyRate = Convert.ToDecimal(tblPurchaseItemMasterTODT["currencyRate"].ToString());

                    if (tblPurchaseItemMasterTODT["basicRate"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.BasicRate = Convert.ToDecimal(tblPurchaseItemMasterTODT["basicRate"].ToString());

                    if (tblPurchaseItemMasterTODT["gstItemCode"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.GSTItemCode = Convert.ToString(tblPurchaseItemMasterTODT["gstItemCode"].ToString());
                    if (tblPurchaseItemMasterTODT["discount"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.Discount = Convert.ToDecimal(tblPurchaseItemMasterTODT["discount"].ToString());

                    if (tblPurchaseItemMasterTODT["pf"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.PF = Convert.ToDecimal(tblPurchaseItemMasterTODT["pf"].ToString());

                    if (tblPurchaseItemMasterTODT["freight"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.Freight = Convert.ToDecimal(tblPurchaseItemMasterTODT["freight"].ToString());
                    if (tblPurchaseItemMasterTODT["deliveryPeriodInDays"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.DeliveryPeriodInDays = Convert.ToDecimal(tblPurchaseItemMasterTODT["deliveryPeriodInDays"].ToString());

                    if (tblPurchaseItemMasterTODT["multiplicationFactor"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.MultiplicationFactor = Convert.ToDecimal(tblPurchaseItemMasterTODT["multiplicationFactor"].ToString());
                    if (tblPurchaseItemMasterTODT["minimumOrderQty"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.MinimumOrderQty = Convert.ToDecimal(tblPurchaseItemMasterTODT["minimumOrderQty"].ToString());

                    if (tblPurchaseItemMasterTODT["supplierAddress"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.SupplierAddress = Convert.ToInt32(tblPurchaseItemMasterTODT["supplierAddress"].ToString());

                    if (tblPurchaseItemMasterTODT["mfgCatlogNo"] != DBNull.Value)
                        //tblPurchaseItemMasterTONew.mfgCatlogNo = Convert.ToString(tblPurchaseItemMasterTODT["mfgCatlogNo"]).ToString());
                        tblPurchaseItemMasterTONew.MfgCatlogNo = Convert.ToString(tblPurchaseItemMasterTODT["mfgCatlogNo"].ToString());
                    if (tblPurchaseItemMasterTODT["weightMeasurUnitId"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.WeightMeasurUnitId = Convert.ToInt32(tblPurchaseItemMasterTODT["weightMeasurUnitId"].ToString());
                    if (tblPurchaseItemMasterTODT["prodItemId"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.ProdItemId = Convert.ToInt32(tblPurchaseItemMasterTODT["prodItemId"].ToString());

                    if (tblPurchaseItemMasterTODT["itemPerPurchaseUnit"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.ItemPerPurchaseUnit = Convert.ToDecimal(tblPurchaseItemMasterTODT["itemPerPurchaseUnit"].ToString());
                    //   if (tblPurchaseItemMasterTODT["lengthmm"] != DBNull.Value)
                    //      tblPurchaseItemMasterTONew.length_mm = Convert.ToDecimal(tblPurchaseItemMasterTODT["lengthmm"].ToString());

                    if (tblPurchaseItemMasterTODT["lengthmm"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.Length_mm = Convert.ToDecimal(tblPurchaseItemMasterTODT["lengthmm"].ToString());
                    if (tblPurchaseItemMasterTODT["widthmm"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.Width_mm = Convert.ToDecimal(tblPurchaseItemMasterTODT["widthmm"].ToString());
                    if (tblPurchaseItemMasterTODT["heightmm"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.Height_mm = Convert.ToDecimal(tblPurchaseItemMasterTODT["heightmm"].ToString());
                    if (tblPurchaseItemMasterTODT["volumeccm"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.Volume_ccm = Convert.ToDecimal(tblPurchaseItemMasterTODT["volumeccm"].ToString());
                    if (tblPurchaseItemMasterTODT["weightkg"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.Weight_kg = Convert.ToDecimal(tblPurchaseItemMasterTODT["weightkg"].ToString());


                    if (tblPurchaseItemMasterTODT["codeTypeId"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.CodeTypeId = Convert.ToInt32(tblPurchaseItemMasterTODT["codeTypeId"].ToString());

                    //Priyanka [25-04-2019]
                    if (tblPurchaseItemMasterTODT["purchaseUOMId"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.PurchaseUOMId = Convert.ToInt32(tblPurchaseItemMasterTODT["purchaseUOMId"].ToString());
                    if (tblPurchaseItemMasterTODT["volumeId"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.VolumeId = Convert.ToInt32(tblPurchaseItemMasterTODT["volumeId"].ToString());
                    if (tblPurchaseItemMasterTODT["qtyPerPkg"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.QtyPerPkg = Convert.ToDouble(tblPurchaseItemMasterTODT["qtyPerPkg"].ToString());
                    if (tblPurchaseItemMasterTODT["isPrioritySupplier"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.IsPrioritySupplier = Convert.ToInt32(tblPurchaseItemMasterTODT["isPrioritySupplier"].ToString());

                    if (tblPurchaseItemMasterTODT["supplierName"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.SupplierName = Convert.ToString(tblPurchaseItemMasterTODT["supplierName"].ToString());

                    if (tblPurchaseItemMasterTODT["orderMultiple"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.OrderMultiple = Convert.ToDouble(tblPurchaseItemMasterTODT["orderMultiple"].ToString());
                    if (tblPurchaseItemMasterTODT["supItemCode"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.SupItemCode = Convert.ToString(tblPurchaseItemMasterTODT["supItemCode"].ToString());
                    if (tblPurchaseItemMasterTODT["priceListId"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.PriceListId = Convert.ToInt32(tblPurchaseItemMasterTODT["priceListId"].ToString());


                    //if (tblPurchaseItemMasterTODT["areaName"] != DBNull.Value)
                    //    tblPurchaseItemMasterTONew.SupplierTblAddressTO.AreaName = Convert.ToString(tblPurchaseItemMasterTODT["areaName"].ToString());
                    //if (tblPurchaseItemMasterTODT["talukaName"] != DBNull.Value)
                    //    tblPurchaseItemMasterTONew.SupplierTblAddressTO.TalukaName = Convert.ToString(tblPurchaseItemMasterTODT["talukaName"].ToString());
                    //if (tblPurchaseItemMasterTODT["districtName"] != DBNull.Value)
                    //    tblPurchaseItemMasterTONew.SupplierTblAddressTO.DistrictName = Convert.ToString(tblPurchaseItemMasterTODT["districtName"].ToString());
                    //if (tblPurchaseItemMasterTODT["idAddr"] != DBNull.Value)
                    //    tblPurchaseItemMasterTONew.SupplierTblAddressTO.IdAddr = Convert.ToInt32(tblPurchaseItemMasterTODT["idAddr"].ToString());

                    if (tblPurchaseItemMasterTODT["idAddr"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.SupplierTblAddressTO.IdAddr = Convert.ToInt32(tblPurchaseItemMasterTODT["idAddr"].ToString());
                    if (tblPurchaseItemMasterTODT["talukaId"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.SupplierTblAddressTO.TalukaId = Convert.ToInt32(tblPurchaseItemMasterTODT["talukaId"].ToString());
                    if (tblPurchaseItemMasterTODT["districtId"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.SupplierTblAddressTO.DistrictId = Convert.ToInt32(tblPurchaseItemMasterTODT["districtId"].ToString());
                    if (tblPurchaseItemMasterTODT["stateId"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.SupplierTblAddressTO.StateId = Convert.ToInt32(tblPurchaseItemMasterTODT["stateId"].ToString());
                    if (tblPurchaseItemMasterTODT["countryId"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.SupplierTblAddressTO.CountryId = Convert.ToInt32(tblPurchaseItemMasterTODT["countryId"].ToString());
                    if (tblPurchaseItemMasterTODT["pincode"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.SupplierTblAddressTO.Pincode = Convert.ToInt32(tblPurchaseItemMasterTODT["pincode"].ToString());
                    if (tblPurchaseItemMasterTODT["createdBy"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.SupplierTblAddressTO.CreatedBy = Convert.ToInt32(tblPurchaseItemMasterTODT["createdBy"].ToString());
                    if (tblPurchaseItemMasterTODT["createdOn"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.SupplierTblAddressTO.CreatedOn = Convert.ToDateTime(tblPurchaseItemMasterTODT["createdOn"].ToString());
                    if (tblPurchaseItemMasterTODT["plotNo"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.SupplierTblAddressTO.PlotNo = Convert.ToString(tblPurchaseItemMasterTODT["plotNo"].ToString());
                    if (tblPurchaseItemMasterTODT["streetName"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.SupplierTblAddressTO.StreetName = Convert.ToString(tblPurchaseItemMasterTODT["streetName"].ToString());
                    if (tblPurchaseItemMasterTODT["areaName"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.SupplierTblAddressTO.AreaName = Convert.ToString(tblPurchaseItemMasterTODT["areaName"].ToString());
                    if (tblPurchaseItemMasterTODT["villageName"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.SupplierTblAddressTO.VillageName = Convert.ToString(tblPurchaseItemMasterTODT["villageName"].ToString());
                    if (tblPurchaseItemMasterTODT["comments"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.SupplierTblAddressTO.Comments = Convert.ToString(tblPurchaseItemMasterTODT["comments"].ToString());
                    if (tblPurchaseItemMasterTODT["talukaName"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.SupplierTblAddressTO.TalukaName = Convert.ToString(tblPurchaseItemMasterTODT["talukaName"].ToString());
                    if (tblPurchaseItemMasterTODT["districtName"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.SupplierTblAddressTO.DistrictName = Convert.ToString(tblPurchaseItemMasterTODT["districtName"].ToString());
                    if (tblPurchaseItemMasterTODT["stateName"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.SupplierTblAddressTO.StateName = Convert.ToString(tblPurchaseItemMasterTODT["stateName"].ToString());
                    //if (tblPurchaseItemMasterTODT["stateCode"] != DBNull.Value)
                    //    tblPurchaseItemMasterTONew.SupplierTblAddressTO.StateCode = Convert.ToString(tblPurchaseItemMasterTODT["stateCode"].ToString());

                    tblPurchaseItemMasterTONew.SupplierAddr = MergeSupplierAddres(tblPurchaseItemMasterTONew);
                    //if (tblPurchaseItemMasterTODT["supplierAddr"] != DBNull.Value)
                    //     tblPurchaseItemMasterTONew.SupplierAddr = Convert.ToString(tblPurchaseItemMasterTODT["supplierAddr"].ToString());


                    if (tblPurchaseItemMasterTODT["nlcCost"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.NlcCost = Convert.ToDecimal(tblPurchaseItemMasterTODT["nlcCost"].ToString());

                    tblPurchaseItemMasterTORow.Add(tblPurchaseItemMasterTONew);
                    //tblPurchaseItemMasterTORow = tblPurchaseItemMasterTONew;
                }
                //else
                //{
                //    return null;
                //}
            }

            return tblPurchaseItemMasterTORow;
        }
        public List<StoresLocationTO> ConvertDTToListForTblItemLinkedStoreLoc(SqlDataReader tblStoresLocationTODT)
        {
            List<StoresLocationTO> tblStoresLocationTORow = new List<StoresLocationTO>();
            if (tblStoresLocationTODT != null)
            {
                while (tblStoresLocationTODT.Read())
                {
                    StoresLocationTO StoresLocationTONew = new StoresLocationTO();
                    if (tblStoresLocationTODT["IdLocation"] != DBNull.Value)
                        StoresLocationTONew.IdLocation = Convert.ToInt32(tblStoresLocationTODT["IdLocation"].ToString());
                    if (tblStoresLocationTODT["LocationDesc"] != DBNull.Value)
                        StoresLocationTONew.LocationDesc = tblStoresLocationTODT["LocationDesc"].ToString();

                    tblStoresLocationTORow.Add(StoresLocationTONew);

                }

            }

            return tblStoresLocationTORow;
        }
        private List<TblPurchaseItemMasterTO> ConvertDTToListForTblPurchaseItemMasterNew(SqlDataReader tblPurchaseItemMasterTODT)
        {
            List<TblPurchaseItemMasterTO> tblPurchaseItemMasterTORow = new List<TblPurchaseItemMasterTO>();
            if (tblPurchaseItemMasterTODT != null)
            {
                while (tblPurchaseItemMasterTODT.Read())
                {
                    TblPurchaseItemMasterTO tblPurchaseItemMasterTONew = new TblPurchaseItemMasterTO();
                    if (tblPurchaseItemMasterTODT["idPurchaseItemMaster"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.IdPurchaseItemMaster = Convert.ToInt32(tblPurchaseItemMasterTODT["idPurchaseItemMaster"].ToString());
                    if (tblPurchaseItemMasterTODT["priority"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.Priority = Convert.ToDecimal(tblPurchaseItemMasterTODT["priority"].ToString());

                    if (tblPurchaseItemMasterTODT["createdBy"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.CreatedBy = Convert.ToInt32(tblPurchaseItemMasterTODT["createdBy"].ToString());
                    if (tblPurchaseItemMasterTODT["updatedBy"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.UpdatedBy = Convert.ToInt32(tblPurchaseItemMasterTODT["updatedBy"].ToString());
                    if (tblPurchaseItemMasterTODT["createdOn"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.CreatedOn = Convert.ToDateTime(tblPurchaseItemMasterTODT["createdOn"].ToString());
                    if (tblPurchaseItemMasterTODT["updatedOn"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.UpdatedOn = Convert.ToDateTime(tblPurchaseItemMasterTODT["updatedOn"].ToString());
                    if (tblPurchaseItemMasterTODT["isActive"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.IsActive = Convert.ToInt32(tblPurchaseItemMasterTODT["isActive"].ToString());

                    if (tblPurchaseItemMasterTODT["currencyId"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.CurrencyId = Convert.ToInt32(tblPurchaseItemMasterTODT["currencyId"].ToString());

                    if (tblPurchaseItemMasterTODT["supplierOrgId"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.SupplierOrgId = Convert.ToInt32(tblPurchaseItemMasterTODT["supplierOrgId"].ToString());

                    if (tblPurchaseItemMasterTODT["gstCodeTypeId"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.GstCodeTypeId = Convert.ToInt32(tblPurchaseItemMasterTODT["gstCodeTypeId"].ToString());

                    if (tblPurchaseItemMasterTODT["currencyRate"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.CurrencyRate = Convert.ToDecimal(tblPurchaseItemMasterTODT["currencyRate"].ToString());

                    if (tblPurchaseItemMasterTODT["basicRate"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.BasicRate = Convert.ToDecimal(tblPurchaseItemMasterTODT["basicRate"].ToString());

                    if (tblPurchaseItemMasterTODT["gstItemCode"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.GSTItemCode = Convert.ToString(tblPurchaseItemMasterTODT["gstItemCode"].ToString());
                    if (tblPurchaseItemMasterTODT["discount"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.Discount = Convert.ToDecimal(tblPurchaseItemMasterTODT["discount"].ToString());

                    if (tblPurchaseItemMasterTODT["pf"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.PF = Convert.ToDecimal(tblPurchaseItemMasterTODT["pf"].ToString());

                    if (tblPurchaseItemMasterTODT["freight"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.Freight = Convert.ToDecimal(tblPurchaseItemMasterTODT["freight"].ToString());
                    if (tblPurchaseItemMasterTODT["deliveryPeriodInDays"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.DeliveryPeriodInDays = Convert.ToDecimal(tblPurchaseItemMasterTODT["deliveryPeriodInDays"].ToString());

                    if (tblPurchaseItemMasterTODT["multiplicationFactor"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.MultiplicationFactor = Convert.ToDecimal(tblPurchaseItemMasterTODT["multiplicationFactor"].ToString());
                    if (tblPurchaseItemMasterTODT["minimumOrderQty"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.MinimumOrderQty = Convert.ToDecimal(tblPurchaseItemMasterTODT["minimumOrderQty"].ToString());

                    if (tblPurchaseItemMasterTODT["supplierAddress"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.SupplierAddress = Convert.ToInt32(tblPurchaseItemMasterTODT["supplierAddress"].ToString());

                    if (tblPurchaseItemMasterTODT["mfgCatlogNo"] != DBNull.Value)
                        //tblPurchaseItemMasterTONew.mfgCatlogNo = Convert.ToString(tblPurchaseItemMasterTODT["mfgCatlogNo"]).ToString());
                        tblPurchaseItemMasterTONew.MfgCatlogNo = Convert.ToString(tblPurchaseItemMasterTODT["mfgCatlogNo"].ToString());
                    if (tblPurchaseItemMasterTODT["weightMeasurUnitId"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.WeightMeasurUnitId = Convert.ToInt32(tblPurchaseItemMasterTODT["weightMeasurUnitId"].ToString());
                    if (tblPurchaseItemMasterTODT["prodItemId"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.ProdItemId = Convert.ToInt32(tblPurchaseItemMasterTODT["prodItemId"].ToString());

                    if (tblPurchaseItemMasterTODT["itemPerPurchaseUnit"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.ItemPerPurchaseUnit = Convert.ToDecimal(tblPurchaseItemMasterTODT["itemPerPurchaseUnit"].ToString());
                    //   if (tblPurchaseItemMasterTODT["lengthmm"] != DBNull.Value)
                    //      tblPurchaseItemMasterTONew.length_mm = Convert.ToDecimal(tblPurchaseItemMasterTODT["lengthmm"].ToString());

                    if (tblPurchaseItemMasterTODT["lengthmm"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.Length_mm = Convert.ToDecimal(tblPurchaseItemMasterTODT["lengthmm"].ToString());
                    if (tblPurchaseItemMasterTODT["widthmm"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.Width_mm = Convert.ToDecimal(tblPurchaseItemMasterTODT["widthmm"].ToString());
                    if (tblPurchaseItemMasterTODT["heightmm"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.Height_mm = Convert.ToDecimal(tblPurchaseItemMasterTODT["heightmm"].ToString());
                    if (tblPurchaseItemMasterTODT["volumeccm"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.Volume_ccm = Convert.ToDecimal(tblPurchaseItemMasterTODT["volumeccm"].ToString());
                    if (tblPurchaseItemMasterTODT["weightkg"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.Weight_kg = Convert.ToDecimal(tblPurchaseItemMasterTODT["weightkg"].ToString());


                    if (tblPurchaseItemMasterTODT["codeTypeId"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.CodeTypeId = Convert.ToInt32(tblPurchaseItemMasterTODT["codeTypeId"].ToString());

                    //Priyanka [25-04-2019]
                    if (tblPurchaseItemMasterTODT["purchaseUOMId"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.PurchaseUOMId = Convert.ToInt32(tblPurchaseItemMasterTODT["purchaseUOMId"].ToString());
                    if (tblPurchaseItemMasterTODT["volumeId"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.VolumeId = Convert.ToInt32(tblPurchaseItemMasterTODT["volumeId"].ToString());
                    if (tblPurchaseItemMasterTODT["qtyPerPkg"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.QtyPerPkg = Convert.ToDouble(tblPurchaseItemMasterTODT["qtyPerPkg"].ToString());
                    if (tblPurchaseItemMasterTODT["isPrioritySupplier"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.IsPrioritySupplier = Convert.ToInt32(tblPurchaseItemMasterTODT["isPrioritySupplier"].ToString());

                    //if (tblPurchaseItemMasterTODT["supplierName"] != DBNull.Value)
                    //    tblPurchaseItemMasterTONew.SupplierName = Convert.ToString(tblPurchaseItemMasterTODT["supplierName"].ToString());

                    if (tblPurchaseItemMasterTODT["createdBy"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.SupplierTblAddressTO.CreatedBy = Convert.ToInt32(tblPurchaseItemMasterTODT["createdBy"].ToString());
                    if (tblPurchaseItemMasterTODT["createdOn"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.SupplierTblAddressTO.CreatedOn = Convert.ToDateTime(tblPurchaseItemMasterTODT["createdOn"].ToString());

                    if (tblPurchaseItemMasterTODT["nlcCost"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.NlcCost = Convert.ToDecimal(tblPurchaseItemMasterTODT["nlcCost"].ToString());

                    if (tblPurchaseItemMasterTODT["itemGrnNlcAmt"] != DBNull.Value)
                        tblPurchaseItemMasterTONew.ItemGrnNlcAmt = Convert.ToDouble(tblPurchaseItemMasterTODT["itemGrnNlcAmt"].ToString());

                    tblPurchaseItemMasterTORow.Add(tblPurchaseItemMasterTONew);
                    //tblPurchaseItemMasterTORow = tblPurchaseItemMasterTONew;
                }
                //else
                //{
                //    return null;
                //}
            }

            return tblPurchaseItemMasterTORow;
        }
        public String MergeSupplierAddres(TblPurchaseItemMasterTO tblPurchaseItemMasterTO)
        {
            String address = String.Empty;

            if (!String.IsNullOrEmpty(tblPurchaseItemMasterTO.SupplierTblAddressTO.AreaName))
                address += tblPurchaseItemMasterTO.SupplierTblAddressTO.AreaName + ",";

            if (!String.IsNullOrEmpty(tblPurchaseItemMasterTO.SupplierTblAddressTO.PlotNo))
                address += tblPurchaseItemMasterTO.SupplierTblAddressTO.PlotNo + ",";

            if (!String.IsNullOrEmpty(tblPurchaseItemMasterTO.SupplierTblAddressTO.StreetName))
                address += tblPurchaseItemMasterTO.SupplierTblAddressTO.StreetName + ",";


            if (!String.IsNullOrEmpty(tblPurchaseItemMasterTO.SupplierTblAddressTO.VillageName))
                address += tblPurchaseItemMasterTO.SupplierTblAddressTO.VillageName + ",";

            if (!String.IsNullOrEmpty(tblPurchaseItemMasterTO.SupplierTblAddressTO.TalukaName))
                address += tblPurchaseItemMasterTO.SupplierTblAddressTO.TalukaName + ",";

            if (!String.IsNullOrEmpty(tblPurchaseItemMasterTO.SupplierTblAddressTO.DistrictName))
                address += tblPurchaseItemMasterTO.SupplierTblAddressTO.DistrictName + ",";


            if (!String.IsNullOrEmpty(address))
            {
                address = address.TrimEnd(',');
            }

            return address;
        }

        // public int UpdateTblProductItemSequenceNo(TblProductItemTO tblProductItemTO, SqlConnection conn, SqlTransaction tran)
        public int UpdateTblProductItemSequenceNo(Int32 isDisplaySeqNo, Int32 idProdItem, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                //return ExecuteUpdationCommandForSequenceNo(tblProductItemTO, cmdUpdate);
                return ExecuteUpdationCommandForSequenceNo(isDisplaySeqNo, idProdItem, cmdUpdate);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }
        // public int ExecuteUpdationCommandForSequenceNo(TblProductItemTO tblProductItemTO, SqlCommand cmdUpdate)
        public int ExecuteUpdationCommandForSequenceNo(Int32 DisplaySequanceNo, Int32 IdProdItem, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblProductItem] SET " +
                                " [displaySequanceNo] = @DisplaySequanceNo" +
                                " WHERE [idProdItem] = @IdProdItem ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;
            cmdUpdate.Parameters.Add("@IdProdItem", System.Data.SqlDbType.Int).Value = IdProdItem;
            cmdUpdate.Parameters.Add("@DisplaySequanceNo", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(DisplaySequanceNo);

            return cmdUpdate.ExecuteNonQuery();
        }



        /// <summary>
        /// //Harshala[16/9/2020] added to update isConsumable/isFixedAsset flag 
        /// </summary>
        /// <param name="prodClassId">Sub grp Id</param>
        /// <param name="updateValue"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns>0, -1 if error</returns>
        public int UpdateTblProductItemIsConsumable(Constants.ConsumableOrFixedAssetE consumableOrFixedAssetE, Int32 prodClassId, Boolean updateValue, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommandForIsConsumable(consumableOrFixedAssetE, prodClassId, updateValue, cmdUpdate);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }
        public int ExecuteUpdationCommandForIsConsumable(Constants.ConsumableOrFixedAssetE consumableOrFixedAssetE, Int32 prodClassId, Boolean updateValue, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblProductItem] SET ";

            if (consumableOrFixedAssetE == Constants.ConsumableOrFixedAssetE.CONSUMABLE)
            {
                sqlQuery += " [isConsumable] = @isConsumable";
            }
            else if (consumableOrFixedAssetE == Constants.ConsumableOrFixedAssetE.FIXED_ASSET)
            {
                sqlQuery += " [isFixedAsset] = @isFixedAsset";

            }
            sqlQuery += " WHERE [prodClassId] = @prodClassId ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;
            cmdUpdate.Parameters.Add("@prodClassId", System.Data.SqlDbType.Int).Value = prodClassId;
            if (consumableOrFixedAssetE == Constants.ConsumableOrFixedAssetE.CONSUMABLE)
                cmdUpdate.Parameters.Add("@isConsumable", System.Data.SqlDbType.Bit).Value = updateValue;
            if (consumableOrFixedAssetE == Constants.ConsumableOrFixedAssetE.FIXED_ASSET)
                cmdUpdate.Parameters.Add("@isFixedAsset", System.Data.SqlDbType.Bit).Value = updateValue;

            return cmdUpdate.ExecuteNonQuery();
        }

        /// <summary>
        /// //Harshala[1679/2020] added to update isConsumable/isFixedAsset for Make Item  
        /// </summary>
        /// <param name="prodClassId">Sub grp Id</param>
        /// <param name="updateValue"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns>0, -1 if error</returns>
        public int UpdateTblProductItemIsConsumableForMakeItem(Constants.ConsumableOrFixedAssetE consumableOrFixedAssetE, Int32 idProdItem, Boolean updateValue, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommandForIsConsumableForMakeItem(consumableOrFixedAssetE, idProdItem, updateValue, cmdUpdate);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }
        public int ExecuteUpdationCommandForIsConsumableForMakeItem(Constants.ConsumableOrFixedAssetE consumableOrFixedAssetE, Int32 idProdItem, Boolean updateValue, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblProductItem] SET ";

            if (consumableOrFixedAssetE == Constants.ConsumableOrFixedAssetE.CONSUMABLE)
            {
                sqlQuery += " [isConsumable] = @isConsumable";
            }
            else if (consumableOrFixedAssetE == Constants.ConsumableOrFixedAssetE.FIXED_ASSET)
            {
                sqlQuery += " [isFixedAsset] = @isFixedAsset";

            }
            sqlQuery += " WHERE [baseProdItemId] = @baseProdItemId ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;
            cmdUpdate.Parameters.Add("@baseProdItemId", System.Data.SqlDbType.Int).Value = idProdItem;
            if (consumableOrFixedAssetE == Constants.ConsumableOrFixedAssetE.CONSUMABLE)
                cmdUpdate.Parameters.Add("@isConsumable", System.Data.SqlDbType.Bit).Value = updateValue;
            if (consumableOrFixedAssetE == Constants.ConsumableOrFixedAssetE.FIXED_ASSET)
                cmdUpdate.Parameters.Add("@isFixedAsset", System.Data.SqlDbType.Bit).Value = updateValue;

            return cmdUpdate.ExecuteNonQuery();
        }

        //Dhananjay[2021-06-22] Added for Update SAP Item With IsPurchaseItem
        public int UpdateItemWithIsPurchaseItem()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.SAP_CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                String sqlQuery = @" UPDATE [OITM] SET PrchseItem = 'Y' WHERE PrchseItem <> 'Y' AND CardCode IS NOT NULL";

                conn.Open();
                cmdUpdate.Connection = conn;
                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;
                return cmdUpdate.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }
        #endregion

        #region Deletion
        public int DeleteTblProductItem(Int32 idProdItem)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idProdItem, cmdDelete);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public int DeleteTblProductItem(Int32 idProdItem, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idProdItem, cmdDelete);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }


        public int ExecuteDeletionCommand(Int32 idProdItem, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblProductItem] " +
            " WHERE idProdItem = " + idProdItem + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idProdItem", System.Data.SqlDbType.Int).Value = tblProductItemTO.IdProdItem;
            return cmdDelete.ExecuteNonQuery();
        }



        //@Priyanka H [15-03-2019]

        public List<TblProductItemTO> SearchTblProductItemOld(string itemName = "", Int32 itemNo = 0, Int32 categoryNo = 0)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            string queryString;
            try
            {
                conn.Open();

                //if (specificationId == 0)
                //{
                if (!String.IsNullOrEmpty(itemName) && itemName != "undefined" && itemNo == 0)
                {
                    if (categoryNo == 0)
                    {
                        //cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE itemDesc LIKE '%" + itemName + "%' ";
                        cmdSelect.CommandText = SqlSelectQuery() + " WHERE item.baseProdItemId > 0 AND item.baseProdItemId IS NOT NULL AND item.isActive=1 AND item.itemName LIKE '%" + itemName + "%' ";
                    }
                    else
                    {
                        cmdSelect.CommandText = SqlSelectQuery() + " WHERE item.baseProdItemId > 0 AND item.baseProdItemId IS NOT NULL AND item.isActive=1 AND item.itemName LIKE '%" + itemName + "%' ";
                    }
                }
                if (itemNo != 0 && String.IsNullOrEmpty(itemName))
                {
                    if (categoryNo == 0)
                    {
                        //  cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE idProdItem LIKE '%" + itemNo + "%' ";
                        cmdSelect.CommandText = SqlSelectQuery() + " WHERE item.baseProdItemId > 0 AND item.baseProdItemId IS NOT NULL AND item.isActive=1 AND item.idProdItem LIKE '%" + itemNo + "%' ";
                    }
                    else
                    { cmdSelect.CommandText = SqlSelectQuery() + " WHERE item.baseProdItemId > 0 AND item.baseProdItemId IS NOT NULL AND item.isActive=1 AND item.isActive=1 AND item.idProdItem LIKE '%" + itemNo + "%' "; }
                }
                if (itemNo != 0 && itemName == "undefined")
                {
                    if (categoryNo == 0)
                    {
                        //cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE idProdItem LIKE '%" + itemNo + "%' ";
                        cmdSelect.CommandText = SqlSelectQuery() + " WHERE item.baseProdItemId > 0 AND item.baseProdItemId IS NOT NULL AND item.isActive=1 AND item.idProdItem LIKE '%" + itemNo + "%' ";
                    }
                    else
                    { cmdSelect.CommandText = SqlSelectQuery() + " WHERE item.baseProdItemId > 0 AND item.baseProdItemId IS NOT NULL AND item.isActive=1 AND item.idProdItem LIKE '%" + itemNo + "%' "; }
                }
                // }
                //  cmdSelect.CommandText = SqlSelectQuery() + " WHERE isActive=1" + "order by displaySequanceNo asc";
                //else
                //{
                //if (specificationId != 0 || itemNo == 0 || itemName == "" || itemName == "undefined")
                //{
                //    cmdSelect.CommandText = SqlSelectQuery() + " WHERE isActive=1 AND prodClassId=" + specificationId;
                //}
                // if (specificationId != 0 && (!String.IsNullOrEmpty(itemName) && itemNo != 0))
                if ((!String.IsNullOrEmpty(itemName) && itemNo != 0 && itemName != "undefined"))
                {
                    // cmdSelect.CommandText = SqlSelectQuery() + " WHERE isActive=1 AND prodClassId=" + specificationId + " AND itemName LIKE '%" + itemName + "%' " + "AND idProdItem LIKE '%" + itemNo + "%' ";
                    // if (categoryNo == 0)
                    // {
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE item.baseProdItemId > 0 AND item.baseProdItemId IS NOT NULL AND item.isActive=1 AND ( item.itemName LIKE '%" + itemName + "%' " + "OR item.idProdItem LIKE '%" + itemNo + "%' )";
                    //}
                    //else
                    //{
                    //    cmdSelect.CommandText = SqlSelectQuery() + " WHERE item.isActive=1 AND itemName LIKE '%" + itemName + "%' " + "AND idProdItem LIKE '%" + itemNo + "%' ";
                    //}
                }

                // queryString = SqlSelectQuery() + " WHERE isActive=1 AND prodClassId=" + specificationId ;
                // cmdSelect.CommandText = " SELECT * FROM (" + queryString + ")sq1 WHERE idProdItem LIKE '%" + itemNo + "%' ";
                // }
                /* if (categoryNo != 0 && specificationId == 0)
                 {
                     //cmdSelect.CommandText = "SELECT * FROM tblProductItem where prodClassId IN(" +
                     //                         "select idProdClass from tblProdClassification where parentProdClassId IN(" +
                     //                         "select idProdClass from tblProdClassification where parentProdClassId IN(" +
                     //                         "select idProdClass from tblProdClassification where idProdClass = " + categoryNo + ")))";

                     cmdSelect.CommandText = "select item.*,p.prodClassDesc as Product, c.prodClassDesc as Category, sc.prodClassDesc as discriSpecification,item.itemDesc,item.idProdItem " +
                                              "from tblProductItem item " +
                                              "LEFT JOIN tblProdClassification sc ON item.prodClassId = sc.idProdClass " +
                                              "LEFT JOIN tblProdClassification c ON sc.parentProdClassId = c.idProdClass " +
                                              "LEFT JOIN tblProdClassification p ON c.parentProdClassId = p.idProdClass " +
                                              "where p.idProdClass =" + categoryNo;
                 }*/

                cmdSelect.CommandText += " group by " + GroupByText();

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProductItemTO> list = ConvertDTToList(reader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        //public int UpdateIsPriorityOfPurchaseItem(Int32 idPurchaseItemMaster, Int32 prodItemId, SqlConnection conn, SqlTransaction tran)
        //{
        //    SqlCommand cmdUpdate = new SqlCommand();
        //    try
        //    {
        //        cmdUpdate.Connection = conn;
        //        cmdUpdate.Transaction = tran;
        //        //return ExecuteUpdationCommandForSequenceNo(tblProductItemTO, cmdUpdate);
        //        return ExecuteUpdationCommandForIsPriorityOfPurchaseItem(idPurchaseItemMaster, prodItemId, cmdUpdate);
        //    }
        //    catch (Exception ex)
        //    {
        //        return -1;
        //    }
        //    finally
        //    {
        //        cmdUpdate.Dispose();
        //    }
        //}



        public List<TblProductItemTO> SearchTblProductItem(string itemName = "", Int32 itemNo = 0, Int32 categoryNo = 0, string warehouseIds = "", int procurementId = 0, Int32 ProductTypeId = 0, int isShowListed = 0)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            string queryString;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE item.baseProdItemId > 0 AND item.baseProdItemId IS NOT NULL AND item.isActive = 1";
                if (ProductTypeId > 0)
                {
                    cmdSelect.CommandText += " AND item.codeTypeId = " + ProductTypeId;
                }
                //if (procurementId != 0)
                //{
                //    cmdSelect.CommandText += " AND item.procurementId =" + procurementId + " ";
                //}


                if (procurementId == (int)Constants.ProcurementTypeE.MAKE)
                {
                    cmdSelect.CommandText += " AND ISNULL(item.procurementId,0) =" + procurementId + " ";
                }
                if (procurementId == (int)Constants.ProcurementTypeE.BUY)
                {
                    cmdSelect.CommandText += " AND ISNULL(item.procurementId,0) =0  ";
                }

                if (isShowListed != (int)Constants.ItemMasterTypeE.ALL)
                {
                    //cmdSelect.CommandText += " AND ISNULL(item.IsNonListed,0) =" + isShowListed + " ";
                }
                if (itemNo != 0)
                {
                    cmdSelect.CommandText += " AND item.idProdItem LIKE '" + itemNo + "' ";
                }
                if (!String.IsNullOrEmpty(itemName))
                {
                    cmdSelect.CommandText += " AND item.itemName LIKE '%" + itemName + "%' ";
                }

                if (!String.IsNullOrEmpty(warehouseIds))
                {
                    if (warehouseIds != "null")
                    {
                        // cmdSelect.CommandText += " AND item.locationId in (" + warehouseIds + ") ";//Reshma commented for release.
                    }
                }

                if (categoryNo != 0)
                {

                }

                cmdSelect.CommandText += " group by " + GroupByText();
                cmdSelect.CommandText += " order by item.itemName";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProductItemTO> list = ConvertDTToList(reader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        private string GroupByText()
        {
            return " item.isFixedAsset,item.isConsumable,item.idProdItem,item.itemName,baseItem.itemName,item.itemDesc,item.prodClassId,item.remark,item.createdOn,item.createdBy                    "
             + ",item.updatedOn,item.updatedBy,item.isActive,item.weightMeasureUnitId,item.conversionUnitOfMeasure,item.conversionFactor,item.isStockRequire,             "
             + "item.isParity,item.basePrice,item.codeTypeId,item.isBaseItemForRate,item.isNonCommercialItem,item.displaySequanceNo,item.priceListId,                     "
             + "item.uOMGroupId,item.manageItemById,item.itemCategoryId,item.isGSTApplicable,item.manufacturerId,item.shippingTypeId,item.materialTypeId,                 "
             + "item.isInventoryItem,item.isSalesItem,item.isPurchaseItem,item.warrantyTemplateId,item.mgmtMethodId  ,dimUomGroupConversion.altQty,tblProductItemPurchaseExt.purchaseUOMId "
             + ",item.additionalIdent,item.hSNCode,item.salesUOMId,item.itemPerSalesUnit,item.salesVolumeId,item.salesQtyPerPkg,item.salesLength,item.salesWidth,         "
             + "item.salesHeight,item.salesWeight,item.gLAccId,item.inventUOMId,item.reqPurchaseUOMId,item.valuationId,item.inventWeight,item.inventMinimum,              "
             + "item.itemCost,item.planningId,item.procurementId,item.compWareHouseId,item.issueId,item.minOrderQty,item.leadTime,item.toleranceDays,item.locationId,     "
             + "item.baseProdItemId,item.itemMakeId,item.itemBrandId,item.catLogNo,item.supItemCode,item.isDefaultMake,item.isImportedItem,item.makeSeries,               "
             + "item.taxCategoryId,item.sACCode,item.isProperSAPItem,item.orderMultiple,item.gstCodeId,item.itemGrnNlcAmt,item.isTestCertificateCompulsary,item.isAllocationApplicable,               "
             + "sc.prodClassDesc,p.prodClassDesc,c.prodClassDesc,p.itemProdCatId,p.idProdClass,sc.idProdClass,c.idProdClass,um.weightMeasurUnitDesc,                      "
             + "dimGenericMaster.value,dimGenericMasterP.value,dimGenericMasterShip.value,tblLocation.locationDesc ,dimGenericMasterUOM.weightMeasurUnitDesc,             "
             + "dimGenericMasterCUOM.weightMeasurUnitDesc ,dimGenericMasterGroupUOM.weightMeasurUnitDesc,dimGenericMasterSUOM.weightMeasurUnitDesc,                       "
             + "dimGenericMasterIUOM.weightMeasurUnitDesc,dimGenericMasterWarrenty.value,gstCodeTbl.sapHsnCode, gstCodeTbl.codeDesc, gstCodeTbl.codeNumber ,              "
             + "gstCodeTbl.taxPct , gstCodeTbl.codeTypeId,item.isAllocationApplicable,item.isConsumable,item.isFixedAsset ,"
             + " item.bomTypeId, item.refProdItemId, item.revisionNo, item.isHavingNewRev, item.status, item.orignalProdItemId, "
             + " gstCodeTbl.taxPct , gstCodeTbl.codeTypeId,item.isAllocationApplicable,item.isConsumable,item.isFixedAsset "
            + " ,tblLocation.mappedTxnId,item.IsNonListed,item.scrapValuation ,item.isDailyScrapReq ,item.scrapStoreLocationId,item.isHaveScrapProdItem,item.finYearExpLedgerId"
            + ",item.assetClassId,item.assetStoreLocationId ,asset.mapSapId,ParentTblLocation.mappedTxnId,ledger.ledgerName,ledger.ledgerCode,dimUomGup.uomGroupName,item.isScrapItem,item.isManageInventory" +
            " , item.rackNo, item.xBinLocation, item.yBinLocation,item.itemClassId,tblProductItemPurchaseExt.nlcCost,dimMasterValue.masterValueDesc,IdProcess";//,tblProductItemBom.qty ";   //AmolG[2021-Jan-14]
        }

        private string SelectText()
        {
            return " item.isDailyScrapReq ,item.scrapStoreLocationId,item.isHaveScrapProdItem,item.orignalProdItemId,item.isFixedAsset,item.isConsumable,item.idProdItem,item.itemName,baseItem.itemName,item.itemDesc,item.prodClassId,item.remark,item.createdOn,item.createdBy                    "
                + ",item.updatedOn,item.updatedBy,item.isActive,item.weightMeasureUnitId,item.conversionUnitOfMeasure as conversionUnitOfMeasureId,item.conversionFactor,item.isStockRequire,             "
                + "item.isParity,item.basePrice,item.codeTypeId,item.isBaseItemForRate,item.isNonCommercialItem,item.displaySequanceNo,item.priceListId,                     "
                + "item.uOMGroupId,item.manageItemById,item.itemCategoryId,item.isGSTApplicable,item.manufacturerId,item.shippingTypeId,item.materialTypeId,                 "
                + "item.isInventoryItem,item.isSalesItem,item.isPurchaseItem,item.warrantyTemplateId,item.mgmtMethodId                                                       "
                + ",item.additionalIdent,item.hSNCode,item.salesUOMId,item.itemPerSalesUnit,item.salesVolumeId,item.salesQtyPerPkg,item.salesLength,item.salesWidth,         "
                + "item.salesHeight,item.salesWeight,item.gLAccId,item.inventUOMId,item.reqPurchaseUOMId,item.valuationId,item.inventWeight,item.inventMinimum,              "
                + "item.itemCost,item.planningId,item.procurementId,item.compWareHouseId,item.issueId,item.minOrderQty,item.leadTime,item.toleranceDays,item.locationId,     "
                + "item.baseProdItemId,item.itemMakeId,item.itemBrandId,item.catLogNo,item.supItemCode,item.isDefaultMake,item.isImportedItem,item.makeSeries,               "
                + "item.taxCategoryId,item.sACCode,item.isProperSAPItem,item.orderMultiple,item.gstCodeId,item.itemGrnNlcAmt,item.isTestCertificateCompulsary,item.isAllocationApplicable," +
                " item.isAllocationApplicable,item.isConsumable,item.isFixedAsset, item.bomTypeId," +
                " item.refProdItemId, item.revisionNo, item.isHavingNewRev, item.status ," +
                " item.IsNonListed,item.scrapValuation ,item.isDailyScrapReq ,item.scrapStoreLocationId,item.isHaveScrapProdItem,item.assetClassId,item.assetStoreLocationId ," +
                " tblLocation.mappedTxnId as MappedLocationId,asset.mapSapId as MapSapAssetClassId ,ParentTblLocation.mappedTxnId as 'mappedSapAssetLocationId',item.finYearExpLedgerId ," +//Reshma Added
               " gstCodeTbl.taxPct , gstCodeTbl.codeTypeId,item.isAllocationApplicable,item.isConsumable,item.isFixedAsset,item.isScrapItem,item.isManageInventory " +
               " , item.rackNo, item.xBinLocation, item.yBinLocation,item.itemClassId ";   //AmolG[2021-Jan-14]
        }

        public int ExecuteUpdationCommandForIsPriorityOfPurchaseItem(Int32 idPurchaseItemMaster, Int32 prodItemId, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblProductItemPurchaseExt] SET " +
                                " [isPrioritySupplier] = 1" +
                                " WHERE [idPurchaseItemMaster] = " + idPurchaseItemMaster + " AND [prodItemId] = " + prodItemId;

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;
            cmdUpdate.Parameters.Add("@IdPurchaseItemMaster", System.Data.SqlDbType.Int).Value = idPurchaseItemMaster;
            cmdUpdate.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(prodItemId);

            return cmdUpdate.ExecuteNonQuery();
        }

        public int UpdateAllIsPriorityOfPurchaseItem(TblPurchaseItemMasterTO tblPurchaseItemMasterTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                //return ExecuteUpdationCommandForSequenceNo(tblProductItemTO, cmdUpdate);
                return ExecuteUpdationCommandForAllIsPriorityOfPurchaseItem(tblPurchaseItemMasterTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }
        public int ExecuteUpdationCommandForAllIsPriorityOfPurchaseItem(TblPurchaseItemMasterTO tblPurchaseItemMasterTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblProductItemPurchaseExt] SET " +
                                " [isPrioritySupplier] = 0" +
                                " WHERE [idPurchaseItemMaster] = " + tblPurchaseItemMasterTO.IdPurchaseItemMaster + " AND [prodItemId] = " + tblPurchaseItemMasterTO.ProdItemId;

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;
            //cmdUpdate.Parameters.Add("@IdPurchaseItemMaster", System.Data.SqlDbType.Int).Value = idPurchaseItemMaster;
            cmdUpdate.Parameters.Add("@IsPrioritySupplier", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblPurchaseItemMasterTO.IsPrioritySupplier);

            return cmdUpdate.ExecuteNonQuery();
        }
        //Reshma Added
        public int DeleteTblProductItemBom(TblProductItemBomTO tblProductItemBomTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                String sqlQuery = @"Delete from tblProductItemBom  " +
                               " WHERE [idBomTree] = @idBomTree";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@idBomTree", System.Data.SqlDbType.Int).Value = tblProductItemBomTO.IdBomTree;
                return cmdUpdate.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }
        public int UpdateTblProductItemBom(TblProductItemBomTO tblProductItemBomTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                String sqlQuery = @" UPDATE [tblproductitembom] SET " +
                               "  [qty]= @qty " +
                               ",  [isOptional]= @IsOptional " +
                               ", [updatedOn] = @updateOn" +
                               ", [updatedBy] = @updateBy" +
                               " WHERE [idBomTree] = @idBomTree";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@idBomTree", System.Data.SqlDbType.Int).Value = tblProductItemBomTO.IdBomTree;
                cmdUpdate.Parameters.Add("@qty", System.Data.SqlDbType.Decimal).Value = tblProductItemBomTO.Qty;
                cmdUpdate.Parameters.Add("@IsOptional", System.Data.SqlDbType.Decimal).Value = tblProductItemBomTO.IsOptional;
                cmdUpdate.Parameters.Add("@updateOn", System.Data.SqlDbType.DateTime).Value = tblProductItemBomTO.UpdatedOn;
                cmdUpdate.Parameters.Add("@updateBy", System.Data.SqlDbType.Int).Value = tblProductItemBomTO.UpdatedBy;
                return cmdUpdate.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }
        //Reshma
        public int UpdateTblProductItemBomIsExisteInSAP(TblProductItemBomTO tblProductItemBomTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                String sqlQuery = @" UPDATE [tblproductitembom] SET " +
                               "  [isBOMExistsInSAP]= @isBOMExistsInSAP " +
                               ", [updatedOn] = @updateOn" +
                               ", [updatedBy] = @updateBy" +
                               " WHERE [idBomTree] = @idBomTree";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@idBomTree", System.Data.SqlDbType.Int).Value = tblProductItemBomTO.IdBomTree;
                cmdUpdate.Parameters.Add("@isBOMExistsInSAP", System.Data.SqlDbType.Decimal).Value = tblProductItemBomTO.IsBOMExistsInSAP;
                cmdUpdate.Parameters.Add("@updateOn", System.Data.SqlDbType.DateTime).Value = tblProductItemBomTO.UpdatedOn;
                cmdUpdate.Parameters.Add("@updateBy", System.Data.SqlDbType.Int).Value = tblProductItemBomTO.UpdatedBy;
                return cmdUpdate.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }
        public int UpdateTblProductItemBomModelId(TblProductItemBomTO tblProductItemBomTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                String sqlQuery = @" UPDATE [tblproductitembom] SET " +
                               "  [modelId]= @ModelId " +
                               ", [updatedOn] = @updateOn" +
                               ", [updatedBy] = @updateBy" +
                               " WHERE [idBomTree] = @idBomTree";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@idBomTree", System.Data.SqlDbType.Int).Value = tblProductItemBomTO.IdBomTree;
                cmdUpdate.Parameters.Add("@ModelId", System.Data.SqlDbType.Decimal).Value = tblProductItemBomTO.ModelId;
                cmdUpdate.Parameters.Add("@updateOn", System.Data.SqlDbType.DateTime).Value = tblProductItemBomTO.UpdatedOn;
                cmdUpdate.Parameters.Add("@updateBy", System.Data.SqlDbType.Int).Value = tblProductItemBomTO.UpdatedBy;
                return cmdUpdate.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public int UpdateTblProductItemBomIsExisteInSAP(TblProductItemBomTO tblProductItemBomTO)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            try
            {
                conn.Open();
                String sqlQuery = @" UPDATE [tblproductitembom] SET " +
                               "  [isBOMExistsInSAP]= @isBOMExistsInSAP " +
                               ", [updatedOn] = @updateOn" +
                               ", [updatedBy] = @updateBy" +
                               " WHERE [idBomTree] = @idBomTree";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@idBomTree", System.Data.SqlDbType.Int).Value = tblProductItemBomTO.IdBomTree;
                cmdUpdate.Parameters.Add("@isBOMExistsInSAP", System.Data.SqlDbType.Decimal).Value = tblProductItemBomTO.IsBOMExistsInSAP;
                cmdUpdate.Parameters.Add("@updateOn", System.Data.SqlDbType.DateTime).Value = tblProductItemBomTO.UpdatedOn;
                cmdUpdate.Parameters.Add("@updateBy", System.Data.SqlDbType.Int).Value = tblProductItemBomTO.UpdatedBy;
                return cmdUpdate.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
                conn.Close();
            }
        }

        public int UpdateStatusTblProductItem(TblProductItemBomTO tblProductItemBomTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                String sqlQuery = @" UPDATE [tblproductitem] SET " +
                               "  [status]= @status " +
                               ", [updatedOn] = @updateOn" +
                               ", [updatedBy] = @updateBy" +
                               " WHERE [idProdItem] = @idProdItem";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@idProdItem", System.Data.SqlDbType.Int).Value = tblProductItemBomTO.ParentProdItemId;
                cmdUpdate.Parameters.Add("@status", System.Data.SqlDbType.Decimal).Value = tblProductItemBomTO.Status;
                cmdUpdate.Parameters.Add("@updateOn", System.Data.SqlDbType.DateTime).Value = tblProductItemBomTO.UpdatedOn;
                cmdUpdate.Parameters.Add("@updateBy", System.Data.SqlDbType.Int).Value = tblProductItemBomTO.CreatedBy;
                return cmdUpdate.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public int UpdateStatusTblProductItem(TblProductItemTO tblProductItemTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                String sqlQuery = @" UPDATE [tblproductitem] SET " +
                               "  [status]= @status " +
                               ", [updatedOn] = @updateOn" +
                               ", [updatedBy] = @updateBy" +
                               " WHERE [idProdItem] = @idProdItem";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@idProdItem", System.Data.SqlDbType.Int).Value = tblProductItemTO.IdProdItem;
                cmdUpdate.Parameters.Add("@status", System.Data.SqlDbType.Decimal).Value = tblProductItemTO.Status;
                cmdUpdate.Parameters.Add("@updateOn", System.Data.SqlDbType.DateTime).Value = tblProductItemTO.UpdatedOn;
                cmdUpdate.Parameters.Add("@updateBy", System.Data.SqlDbType.Int).Value = tblProductItemTO.UpdatedBy;
                return cmdUpdate.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }
        public int FinalizeTblProductItem(TblProductItemTO finalizeItemBomTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                String sqlQuery = @" UPDATE [tblproductitem] SET " +
                               "  [status]= @status " +
                               ", [updatedOn] = @updateOn" +
                               ", [updatedBy] = @updateBy" +
                               " WHERE [idProdItem] = @idProdItem";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@idProdItem", System.Data.SqlDbType.Int).Value = finalizeItemBomTO.ParentProdItemId;
                cmdUpdate.Parameters.Add("@status", System.Data.SqlDbType.Decimal).Value = finalizeItemBomTO.Status;
                cmdUpdate.Parameters.Add("@updateOn", System.Data.SqlDbType.DateTime).Value = finalizeItemBomTO.UpdatedOn;
                cmdUpdate.Parameters.Add("@updateBy", System.Data.SqlDbType.Int).Value = finalizeItemBomTO.UpdatedBy;
                return cmdUpdate.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public int UpdateIsHavingNewRevTblProductItem(Int32 prodItemId, Int32 UpdatedBy, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                var UpdatedOn = _iCommon.ServerDateTime;

                String sqlQuery = @" UPDATE [tblproductitem] SET " +
                               "  [isHavingNewRev]= 0 " +
                               ", [updatedOn] = @updateOn" +
                               ", [updatedBy] = @updateBy" +
                               ", [isActive]=@isActive " +//Reshma Added
                               " WHERE [idProdItem] = @idProdItem";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@idProdItem", System.Data.SqlDbType.Int).Value = prodItemId;
                cmdUpdate.Parameters.Add("@updateOn", System.Data.SqlDbType.DateTime).Value = UpdatedOn;
                cmdUpdate.Parameters.Add("@updateBy", System.Data.SqlDbType.Int).Value = UpdatedBy;
                cmdUpdate.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value = 0;
                return cmdUpdate.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }
        //chetan[2020-10-01] added
        public int UpdateisHaveScrapProdItem(TblProductItemTO tblProductItemTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                int haveScrapProdItemId = 0;
                if (tblProductItemTO.IsHaveScrapProdItem)
                    haveScrapProdItemId = 1;
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                cmdUpdate.CommandText = "UPDATE tblProductItem set isHaveScrapProdItem= " + haveScrapProdItemId + " WHERE idProdItem= " + tblProductItemTO.IdProdItem;
                cmdUpdate.CommandType = System.Data.CommandType.Text;
                return cmdUpdate.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public int UpdateAssetStoreLocation(TblProductItemTO tblProductItemTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                cmdUpdate.CommandText = "UPDATE tblProductItem set assetStoreLocationId= " + tblProductItemTO.AssetStoreLocationId + " WHERE idProdItem= " + tblProductItemTO.IdProdItem;
                cmdUpdate.CommandType = System.Data.CommandType.Text;
                return cmdUpdate.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        //Dhananjay[2021-July-28] added
        public SAPItemTO GetItemFromSap(string ItemCode)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.SAP_CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "SELECT * FROM OITM WHERE ItemCode ='" + ItemCode + "' AND validFor='Y'";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader notifyReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<SAPItemTO> sAPItemTOList = new List<SAPItemTO>();
                while ((notifyReader).Read())
                {
                    SAPItemTO sAPItemTO = new SAPItemTO();

                    if ((notifyReader)["CardCode"] != DBNull.Value)
                        sAPItemTO.CardCode = Convert.ToString((notifyReader)["CardCode"].ToString());
                    if ((notifyReader)["PrchseItem"] != DBNull.Value)
                        sAPItemTO.PrchseItem = (notifyReader)["PrchseItem"].ToString();
                    if ((notifyReader)["UgpEntry"] != DBNull.Value)
                        sAPItemTO.UgpEntry = Convert.ToInt32((notifyReader)["UgpEntry"].ToString());
                    if ((notifyReader)["IUoMEntry"] != DBNull.Value)
                        sAPItemTO.IUoMEntry = Convert.ToInt32((notifyReader)["IUoMEntry"].ToString());
                    if ((notifyReader)["PUoMEntry"] != DBNull.Value)
                        sAPItemTO.PUoMEntry = Convert.ToInt32((notifyReader)["PUoMEntry"].ToString());
                    if ((notifyReader)["GSTRelevnt"] != DBNull.Value)
                        sAPItemTO.GSTRelevnt = Convert.ToString((notifyReader)["GSTRelevnt"].ToString());
                    if ((notifyReader)["GstTaxCtg"] != DBNull.Value)
                        sAPItemTO.GstTaxCtg = Convert.ToString((notifyReader)["GstTaxCtg"].ToString());
                    if ((notifyReader)["ChapterID"] != DBNull.Value)
                        sAPItemTO.ChapterID = Convert.ToInt32((notifyReader)["ChapterID"].ToString());
                    if ((notifyReader)["ItemClass"] != DBNull.Value)
                        sAPItemTO.ItemClass = (notifyReader)["ItemClass"].ToString();
                    if ((notifyReader)["SACEntry"] != DBNull.Value)
                        sAPItemTO.SACEntry = Convert.ToInt32((notifyReader)["SACEntry"].ToString());
                    sAPItemTOList.Add(sAPItemTO);
                }
                if (notifyReader != null) notifyReader.Dispose();
                if (sAPItemTOList != null && sAPItemTOList.Count == 1)
                    return sAPItemTOList[0];
                else return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmdSelect.Dispose();
            }
        }

        public List<DropDownTO> GetMissingFieldsItemFromSap()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.SAP_CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select ItemCode from OITM where UgpEntry = -1 or GstTaxCtg ='N'";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader notifyReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> convItemTOList = new List<DropDownTO>();
                while ((notifyReader).Read())
                {
                    DropDownTO convItemTO = new DropDownTO();

                    if ((notifyReader)["ItemCode"] != DBNull.Value)
                        convItemTO.Text = Convert.ToString((notifyReader)["ItemCode"].ToString());
                    if ((notifyReader)["ItemCode"] != DBNull.Value)
                        convItemTO.Value = Convert.ToInt32((notifyReader)["ItemCode"].ToString());
                    convItemTOList.Add(convItemTO);
                }
                if (notifyReader != null) notifyReader.Dispose();
                return convItemTOList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmdSelect.Dispose();
            }
        }

        public List<DropDownTO> GetMissingFieldsItemHavingConversionFactorOtherThan1()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select idProdItem from tblProductItem where isActive =1 and baseProdItemId > 0 and isnull(conversionFactor,0) not in (1,0)";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader notifyReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> convItemTOList = new List<DropDownTO>();
                while ((notifyReader).Read())
                {
                    DropDownTO convItemTO = new DropDownTO();

                    if ((notifyReader)["idProdItem"] != DBNull.Value)
                        convItemTO.Text = Convert.ToString((notifyReader)["idProdItem"].ToString());
                    if ((notifyReader)["idProdItem"] != DBNull.Value)
                        convItemTO.Value = Convert.ToInt32((notifyReader)["idProdItem"].ToString());
                    convItemTOList.Add(convItemTO);
                }
                if (notifyReader != null) notifyReader.Dispose();
                return convItemTOList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmdSelect.Dispose();
            }
        }
        public int InsertTblItemLinkedStoreLoc(StoresLocationTO StoresLocationTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommandForItemLinkedStoreLoc(StoresLocationTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public int ExecuteInsertionCommandForItemLinkedStoreLoc(StoresLocationTO StoresLocationTO, SqlCommand cmdInsert)
        {

            String sqlQuery = @" INSERT INTO [tblItemLinkedStoreLoc]( " +
                         "  [ProductItemId]" +
                         " ,[StoreLocId]" +
                         ",[IsActive] " +
                          " )" +
             " VALUES (" +
                         "  @ProductItemId " +
                         " ,@StoreLocId " +
                         " ,@IsActive " +

                         " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@ProductItemId", System.Data.SqlDbType.Int).Value = StoresLocationTO.ProdItemId;
            cmdInsert.Parameters.Add("@StoreLocId", System.Data.SqlDbType.Int).Value = StoresLocationTO.IdLocation;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = StoresLocationTO.IsActive;

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                return 1;
            }

            return 0;
        }
        public int UpdateTblItemLinkedStoreLoc(Int32 idProdItem, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationItemLinkedStoreLoc(idProdItem, cmdUpdate);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public int InsertTblProcessType(int ProcessTypeId, int ProdItemId, string ProcessTypeName, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommandForProcessType(ProcessTypeId, ProdItemId, ProcessTypeName, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }
        public int ExecuteInsertionCommandForProcessType(int ProcessTypeId, Int32 ProdItemId, string ProcessTypeName, SqlCommand cmdInsert)
        {

            String sqlQuery = @" INSERT INTO [dimProcessType]( " +
                         "[idProcessType]" +
                         "  ,[processTypeName]" +
                         " ,[processTypeDesc]" +
                         " ,[prodItemId]" +
                         " ,[isActive]" +
                         " ,[isDefualt]" +
                         " ,[isForReparing]" +

                          " )" +
             " VALUES (" +
                         "  @idProcessType " +
                         "  ,@processTypeName " +
                         " ,@processTypeDesc " +
                         " ,@prodItemId " +
                         " ,@isActive " +
                         " ,@isDefualt " +
                         " ,@isForReparing " +

                         " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            cmdInsert.Parameters.Add("@idProcessType", System.Data.SqlDbType.Int).Value = ProcessTypeId;
            cmdInsert.Parameters.Add("@processTypeName", System.Data.SqlDbType.NVarChar).Value = Convert.ToString(ProcessTypeName);
            cmdInsert.Parameters.Add("@processTypeDesc", System.Data.SqlDbType.NVarChar).Value = Convert.ToString(ProcessTypeName);
            cmdInsert.Parameters.Add("@prodItemId", System.Data.SqlDbType.Int).Value = ProdItemId;
            cmdInsert.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value = 1;
            cmdInsert.Parameters.Add("@isDefualt", System.Data.SqlDbType.Int).Value = 0;
            cmdInsert.Parameters.Add("@isForReparing", System.Data.SqlDbType.Int).Value = 0;


            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                return 1;
            }

            return 0;
        }

        public int UpdateTblProcessType(int ProdItemId, string ProcessTypeName, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteUpdateCommandForProcessType(ProdItemId, ProcessTypeName, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }
        public int ExecuteUpdateCommandForProcessType(Int32 ProdItemId, string ProcessTypeName, SqlCommand cmdInsert)
        {


            String sqlQuery = @" UPDATE [dimProcessType] SET " +
                                " [processTypeName] = @processTypeName " +
                                 " ,[processTypeDesc] = @processTypeName  " +
                                " WHERE [prodItemId] = @prodItemId ";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@prodItemId", System.Data.SqlDbType.Int).Value = ProdItemId;
            cmdInsert.Parameters.Add("@processTypeName", System.Data.SqlDbType.NVarChar).Value = Convert.ToString(ProcessTypeName);



            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                return 1;
            }

            return 0;
        }

        public List<dimProcessType> GetNewProcessTypeId(SqlConnection conn = null, SqlTransaction tran = null)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader notifyReader = null;
            try
            {
                if (conn != null)
                {
                    cmdSelect.Connection = conn;
                    cmdSelect.Transaction = tran;
                }
                else
                {
                    String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
                    conn = new SqlConnection(sqlConnStr);
                    cmdSelect.Connection = conn;
                    conn.Open();
                }

                cmdSelect.CommandText = "select max(isnull(idProcessType,0)) + 1 as idProcessType from dimProcessType";



                cmdSelect.CommandType = System.Data.CommandType.Text;
                notifyReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConverDTForProcessTpyeIDList(notifyReader);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (notifyReader != null) notifyReader.Dispose();
                if (tran == null)
                {
                    conn.Close();
                }
                cmdSelect.Dispose();
            }
        }

        public List<dimProcessType> checkProcessTpyeAlreadyExists(Int32 idProdItem, SqlConnection conn = null, SqlTransaction tran = null)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader notifyReader = null;
            try
            {
                if (conn != null)
                {
                    cmdSelect.Connection = conn;
                    cmdSelect.Transaction = tran;
                }
                else
                {
                    String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
                    conn = new SqlConnection(sqlConnStr);
                    cmdSelect.Connection = conn;
                    conn.Open();
                }
                if (idProdItem > 0)
                {
                    cmdSelect.CommandText = "select * from dimProcessType where prodItemId = @IdProdItem   AND isActive =1";
                }


                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@IdProdItem", System.Data.SqlDbType.Int).Value = idProdItem;
                notifyReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConverDTForProcessTpyeList(notifyReader);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (notifyReader != null) notifyReader.Dispose();
                if (tran == null)
                {
                    conn.Close();
                }
                cmdSelect.Dispose();
            }
        }
        public List<dimProcessType> ConverDTForProcessTpyeList(SqlDataReader tblProcessTypeTODT)
        {
            List<dimProcessType> tblProcessTypeTOList = new List<dimProcessType>();
            if (tblProcessTypeTODT != null)
            {
                while (tblProcessTypeTODT.Read())
                {
                    dimProcessType tblProcessTypeTONew = new dimProcessType();
                    if (tblProcessTypeTODT["prodItemId"] != DBNull.Value)
                        tblProcessTypeTONew.ProdItemId = Convert.ToInt32(tblProcessTypeTODT["prodItemId"].ToString());
                    tblProcessTypeTOList.Add(tblProcessTypeTONew);
                }
            }
            return tblProcessTypeTOList;
        }

        public List<dimProcessType> ConverDTForProcessTpyeIDList(SqlDataReader tblProcessTypeTODT)
        {
            List<dimProcessType> tblProcessTypeTOList = new List<dimProcessType>();
            if (tblProcessTypeTODT != null)
            {
                while (tblProcessTypeTODT.Read())
                {
                    dimProcessType tblProcessTypeTONew = new dimProcessType();
                    if (tblProcessTypeTODT["idProcessType"] != DBNull.Value)
                        tblProcessTypeTONew.IdProcessType = Convert.ToInt32(tblProcessTypeTODT["idProcessType"].ToString());
                    tblProcessTypeTOList.Add(tblProcessTypeTONew);
                }
            }
            return tblProcessTypeTOList;
        }
        public List<DimProcessTO> GetProcessName(Int32 idProcess, SqlConnection conn = null, SqlTransaction tran = null)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader notifyReader = null;
            try
            {
                if (conn != null)
                {
                    cmdSelect.Connection = conn;
                    cmdSelect.Transaction = tran;
                }
                else
                {
                    String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
                    conn = new SqlConnection(sqlConnStr);
                    cmdSelect.Connection = conn;
                    conn.Open();
                }
                if (idProcess > 0)
                {
                    cmdSelect.CommandText = "select * from dimProcessMaster where idProcess = @idProcess   AND isActive =1";
                }


                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@idProcess", System.Data.SqlDbType.Int).Value = idProcess;
                notifyReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConverDTForProcessNameList(notifyReader);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (notifyReader != null) notifyReader.Dispose();
                if (tran == null)
                {
                    conn.Close();
                }
                cmdSelect.Dispose();
            }
        }
        public List<DimProcessTO> ConverDTForProcessNameList(SqlDataReader tblProcessNameTODT)
        {
            List<DimProcessTO> tblProcessNameTOList = new List<DimProcessTO>();
            if (tblProcessNameTODT != null)
            {
                while (tblProcessNameTODT.Read())
                {
                    DimProcessTO tblProcessNameTONew = new DimProcessTO();
                    if (tblProcessNameTODT["ProcessName"] != DBNull.Value)
                        tblProcessNameTONew.ProcessName = Convert.ToString(tblProcessNameTODT["ProcessName"].ToString());
                    tblProcessNameTOList.Add(tblProcessNameTONew);
                }
            }
            return tblProcessNameTOList;
        }
        #endregion

    }
}
