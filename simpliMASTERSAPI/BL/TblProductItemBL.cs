using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System.Linq;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;
using simpliMASTERSAPI.DAL.Interfaces;
using simpliMASTERSAPI;
using System.IO;
using System.Drawing;
using System.Net;
using SAPbobsCOM;
using simpliMASTERSAPI.BL.Interfaces;
using ODLMWebAPI.DAL;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.SS.Formula.Functions;

namespace ODLMWebAPI.BL
{
    public class TblProductItemBL : ITblProductItemBL
    {
        #region Declaration & Constructor

        private readonly ITblProductItemDAO _iTblProductItemDAO;
        private readonly ITblProdItemMakeBrandExtDAO _iTblProdItemMakeBrandExtDAO;
        private readonly IConnectionString _iConnectionString;
        private readonly ITblProdClassificationBL _iTblProdClassificationBL;
        private readonly ITblConfigParamsDAO _iTblConfigParamsDAO;
        private readonly IDimUnitMeasuresDAO _iDimUnitMeasuresDAO;
        private readonly IDimensionDAO _iDimensionDAO;
        private readonly IDimUomGroupDAO _iDimUomGroupDAO;
        private readonly ICommon _iCommon;
        private readonly IDimUomGroupConversionDAO _iDimUomGroupConversionDAO;
        private readonly ITblProdGstCodeDtlsBL _iTblProdGstCodeDtlsBL;
        private readonly ITblGstCodeDtlsBL _iTblGstCodeDtlsBL;
        private readonly ITblLocationBL _iTblLocationBL;
        private readonly IDimProdCatBL _iDimProdCatBL;
        private readonly ITblModelBL _iTblModelBL;
        private readonly ITblProdItemMakeBrandBL _iITblProdItemMakeBrandBL;
        private readonly IRunReport _iRunReport;
        private readonly IDimReportTemplateBL _iDimReportTemplateBL;
        private readonly ITblOrganizationBL _iTblOrganizationBL;
        public TblProductItemBL(ITblProdGstCodeDtlsBL iTblProdGstCodeDtlsBL, IDimUomGroupConversionDAO iDimUomGroupConversionDAO, ICommon iCommon, IDimUomGroupDAO iDimUomGroupDAO, IDimensionDAO iDimensionDAO, IDimUnitMeasuresDAO iDimUnitMeasuresDAO, ITblProdClassificationBL iTblProdClassificationBL, IConnectionString iConnectionString, ITblProductItemDAO iTblProductItemDAO, ITblConfigParamsDAO iTblConfigParamsDAO
                                ,ITblLocationBL iTblLocationBL, IDimProdCatBL iDimProdCatBL, ITblModelBL iTblModelBL,
            ITblProdItemMakeBrandBL iITblProdItemMakeBrandBL,ITblProdItemMakeBrandExtDAO iTblProdItemMakeBrandExtDAO, IRunReport iRunReport, IDimReportTemplateBL iDimReportTemplateBL, ITblOrganizationBL iTblOrganizationBL, ITblGstCodeDtlsBL iTblGstCodeDtlsBL)
        {
            _iTblProdItemMakeBrandExtDAO = iTblProdItemMakeBrandExtDAO;
            _iTblModelBL = iTblModelBL;
            _iTblProductItemDAO = iTblProductItemDAO;
            _iConnectionString = iConnectionString;
            _iTblProdClassificationBL = iTblProdClassificationBL;
            _iTblConfigParamsDAO = iTblConfigParamsDAO;
            _iDimUnitMeasuresDAO = iDimUnitMeasuresDAO;
            _iDimensionDAO = iDimensionDAO;
            _iDimUomGroupDAO = iDimUomGroupDAO;
            _iCommon = iCommon;
            _iDimUomGroupConversionDAO = iDimUomGroupConversionDAO;
            _iTblProdGstCodeDtlsBL = iTblProdGstCodeDtlsBL;
            _iTblLocationBL = iTblLocationBL;
            _iDimProdCatBL = iDimProdCatBL;
            _iITblProdItemMakeBrandBL = iITblProdItemMakeBrandBL;
            _iRunReport = iRunReport;
            _iDimReportTemplateBL = iDimReportTemplateBL;
            _iTblOrganizationBL = iTblOrganizationBL;
            _iTblGstCodeDtlsBL = iTblGstCodeDtlsBL;
        }

        #endregion

        #region Selection
        public List<TblPurchaseItemMasterTO> SelectAllTblPurchaseItemMasterTOList(Int32 prodItemId, Int32 purchaseItemMasterId)
        {
            return _iTblProductItemDAO.SelectAllTblPurchaseItemMasterTOList(prodItemId, purchaseItemMasterId);
        }
        public List<TblProductItemTO> SelectAllTblProductItemList(Int32 specificationId = 0)
        {
            return _iTblProductItemDAO.SelectAllTblProductItem(specificationId);
        }
        public List<TblProductItemTO> GetMakeItemList(Int32 BOMTypeId, String bomStatusIdStr, Int32 idProdItem = 0, Int32 parentProdItemId = 0) 
        {
            List<TblProductItemTO> tblProductItemTOList = new List<TblProductItemTO>();
            if (parentProdItemId <= 0)
            {
                tblProductItemTOList = _iTblProductItemDAO.GetMakeItemList(BOMTypeId, bomStatusIdStr, idProdItem);

                if (idProdItem > 0 && tblProductItemTOList != null && tblProductItemTOList.Count>0)//Reshma Added 
                {
                    tblProductItemTOList = tblProductItemTOList.Where(w => w.IdProdItem == idProdItem).ToList();
                }


                if (tblProductItemTOList != null && tblProductItemTOList.Count > 0)
                {                   
                    for (int i = 0; i < tblProductItemTOList.Count; i++)
                    {
                        List<TblProductItemTO> TblProductItemBomTOListTemp = _iTblProductItemDAO.GetMakeItemBOMList(tblProductItemTOList[i].IdProdItem.ToString());
                        for (int j = 0; j < TblProductItemBomTOListTemp.Count; j++)
                        {
                            tblProductItemTOList[i].TotalNLC += TblProductItemBomTOListTemp[j].ItemCost * Convert.ToDouble(TblProductItemBomTOListTemp[j].Qty);
                        }
                    }
                }
                        //Deepali [18-03-2021] To get only parents list 
                        //if (tblProductItemTOList != null && tblProductItemTOList.Count > 0)
                        //{
                        //    List<List<TblProductItemTO>> enumerationsList = new List<List<TblProductItemTO>>();
                        //    enumerationsList = ListExtensions.ChunkBy(tblProductItemTOList, 75);
                        //    for (int i = 0; i < enumerationsList.Count; i++)
                        //    {
                        //        string IdProdItemStr = string.Join(",", enumerationsList[i].Select(n => n.IdProdItem.ToString()).ToArray());
                        //        List<TblProductItemTO> TblProductItemBomTOList = _iTblProductItemDAO.GetMakeItemBOMList(IdProdItemStr);
                        //        if (TblProductItemBomTOList != null && TblProductItemBomTOList.Count > 0)
                        //        {
                        //            TblProductItemBomTOList.ForEach(element =>
                        //            {
                        //                tblProductItemTOList.Add(element);
                        //            });
                        //        }
                        //    }
                        //}

                    }
                    if (parentProdItemId > 0)
            {
                List<TblProductItemTO> TblProductItemBomTOList = _iTblProductItemDAO.GetMakeItemBOMList(parentProdItemId.ToString());
                if (TblProductItemBomTOList != null && TblProductItemBomTOList.Count > 0)
                {
                    if (tblProductItemTOList == null)
                    {
                        tblProductItemTOList = new List<TblProductItemTO>();
                    }
                    TblProductItemBomTOList.ForEach(element =>
                    {
                        tblProductItemTOList.Add(element);
                    });
                }
            }
            return tblProductItemTOList;
        }
        //Reshma
        public ResultMessage SaveBOMItemList(int parentBomId)
        {

            ResultMessage resultMessage = new ResultMessage();
            int result = 1;
            string prodItemIdStr = string.Empty;
            List<TblProductItemBomTO> TblProductItemBomTOList = _iTblProductItemDAO.GetItemBOMList(parentBomId);
            if (TblProductItemBomTOList != null && TblProductItemBomTOList.Count > 0)
            {
                for (int i = 0; i < TblProductItemBomTOList.Count; i++)
                {
                    TblProductItemBomTO TblProductItemBomTOLocal = TblProductItemBomTOList[i];
                    if (string.IsNullOrEmpty(prodItemIdStr))
                        prodItemIdStr =  TblProductItemBomTOLocal.ChildProdItemId.ToString();
                    else
                        prodItemIdStr = prodItemIdStr + " ," + TblProductItemBomTOLocal.ChildProdItemId;
                }
                prodItemIdStr = prodItemIdStr.TrimEnd(',');
            }
            if (!string.IsNullOrEmpty(prodItemIdStr))
            {
                List<TblProductItemTO> tblProductItemTOList = _iTblProductItemDAO.GetMakeItemBOMList(parentBomId.ToString());

                List<TblProductItemTO> tblProductItemTONonListedList = tblProductItemTOList.Where(w => w.IsNonListed == true).ToList();
                if (tblProductItemTONonListedList != null && tblProductItemTONonListedList.Count > 0)
                {

                    string NonListetItemId = "";
                    for (int i = 0; i < tblProductItemTONonListedList.Count; i++)
                    {
                        TblProductItemTO TblProductItemTOLocal = tblProductItemTONonListedList[i];
                        NonListetItemId = TblProductItemTOLocal + "," + NonListetItemId + "";
                    }
                    resultMessage.DefaultBehaviour();
                    resultMessage.DisplayMessage = "Following Items are non Listed Item Please convert it into Listed ;" + NonListetItemId + "";
                    return resultMessage;
                }
                TblProductItemTO TblParentProductItemTO = SelectTblProductItemTO(parentBomId);

                //Deepali commented for BOM update in SAP
                //resultMessage = GetBOM(TblParentProductItemTO.IdProdItem);
                //if (resultMessage.Result == 1)//BOM Exist
                //{
                //    //TblProductItemBomTOLocal.IsBOMExistsInSAP = true;
                //    //int updateResult = _iTblProductItemDAO.UpdateTblProductItemBomIsExisteInSAP(TblProductItemBomTOLocal);
                //    resultMessage.MessageType = ResultMessageE.Information;
                //    resultMessage.DisplayMessage = "BOM is already Exists";
                //    resultMessage.Result = 1;
                //    return resultMessage;
                //}
                if (tblProductItemTOList != null && tblProductItemTOList.Count > 0)
                {
                    for (int i = 0; i < tblProductItemTOList.Count; i++)
                    {
                        TblProductItemBomTO TblProductItemBomTOLocal = TblProductItemBomTOList[i];
                        if (TblProductItemBomTOLocal.IsBOMExistsInSAP)
                        {
                            resultMessage.MessageType = ResultMessageE.Information;
                            resultMessage.DisplayMessage = "BOM is already Exists";
                            resultMessage.Result = 1;
                            return resultMessage;
                        }
                    }
                }
                resultMessage = SaveBOMInSAP(parentBomId, prodItemIdStr);
                if (resultMessage.Result == 1)//BOM Exist
                {
                    //TblProductItemBomTOLocal.IsBOMExistsInSAP = true;
                    //int updateResult = _iTblProductItemDAO.UpdateTblProductItemBomIsExisteInSAP(TblProductItemBomTOLocal);
                    resultMessage.MessageType = ResultMessageE.Information;
                    resultMessage.DisplayMessage = "BOM is created successfully ";
                    resultMessage.Result = 1;
                }
                
            }
            //resultMessage.DefaultSuccessBehaviour();
            return resultMessage;
        }
        public ResultMessage SaveBOMInSAP(int parentBomId,string prodItemIdStr)
        {
            int result = 1;
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                SAPbobsCOM.Recordset oRsGI = null;
                SAPbobsCOM.Recordset oRsBatch = null;

                oRsBatch = (SAPbobsCOM.Recordset)Startup.CompanyObject.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                oRsGI = (SAPbobsCOM.Recordset)Startup.CompanyObject.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                string Warehouse = string.Empty;
                TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_DAFAULT_SAP_MAPPED_LOCATION_ID_FOR_PURCHASE_REQUEST);
                if (tblConfigParamsTO != null)
                {
                    Warehouse = tblConfigParamsTO.ConfigParamVal;
                }
                int isAlreadyAdded = 0;
                SAPbobsCOM.ProductTrees IssueToProd = null;
                if (!string.IsNullOrEmpty(prodItemIdStr))
                {
                    
                    TblProductItemTO TblParentProductItemTO = SelectTblProductItemTO(parentBomId);
                    List<TblProductItemTO> tblProductItemTOList = _iTblProductItemDAO.GetMakeItemBOMList(parentBomId.ToString());
                    IssueToProd = (SAPbobsCOM.ProductTrees)(Startup.CompanyObject.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oProductTrees));
                                                 
                    if (IssueToProd.GetByKey(parentBomId.ToString()))
                    {
                        isAlreadyAdded = 1;
                    }

                    IssueToProd.Quantity = 1;
                    IssueToProd.ProductDescription = TblParentProductItemTO.ItemName;// Item Name
                    IssueToProd.TreeType = BoItemTreeTypes.iProductionTree;
                    //IssueToProd.TreeCode = "3609";
                    IssueToProd.TreeCode = TblParentProductItemTO.IdProdItem.ToString();// Model Item Code
                    //IssueToProd.Warehouse = Warehouse; //TblParentProductItemTO.MappedLocationId.ToString();//.LocationId.ToString();// Warehouse Id //Reshma[24-12-2020] commented For assign only item mappedSaplocation
                    IssueToProd.Warehouse = TblParentProductItemTO.MappedLocationId.ToString();
                    //IssueToProd.Project = "2";//Here link the Project Name if the Project Has Defined (WO) "20-4101"
                    //Items
                    List<TblProductItemTO> tblProductItemBomItemTOList = tblProductItemTOList.Where(w => w.IdProdItem != parentBomId).ToList();
                    if (tblProductItemBomItemTOList != null && tblProductItemBomItemTOList.Count > 0)
                    {
                        for (var i = 0; i < tblProductItemBomItemTOList.Count; i++)
                        {
                            IssueToProd.Items.SetCurrentLine(i);
                            IssueToProd.Items.ItemCode = tblProductItemBomItemTOList[i].IdProdItem.ToString();
                            IssueToProd.Items.ItemType = ProductionItemType.pit_Item;
                            IssueToProd.Items.IssueMethod = Constants.GetSAPIssueMethodEnum(tblProductItemBomItemTOList[i].IssueId);
                            //IssueToProd.Items.AdditionalQuantity = 2;
                            //IssueToProd.Items.Warehouse = Warehouse; // tblProductItemBomItemTOList[i].MappedLocationId.ToString();//Reshma[24-12-2020] commented For assign only item mappedSaplocation
                            IssueToProd.Items.Warehouse = tblProductItemBomItemTOList[i].MappedLocationId.ToString();
                            IssueToProd.Items.Quantity = (double)tblProductItemBomItemTOList[i].Qty;
                            IssueToProd.Items.Add();
                            
                        }
                    }


                    Startup.CompanyObject.StartTransaction();
                    if (isAlreadyAdded == 1)
                    {
                        result = IssueToProd.Update();
                    }
                    else
                    {
                        result = IssueToProd.Add();
                    }
                    if (result != 0)
                    {
                        string s = Startup.CompanyObject.GetLastErrorDescription();
                        if (Startup.CompanyObject.InTransaction)
                            Startup.CompanyObject.EndTransaction(BoWfTransOpt.wf_RollBack);
                        resultMessage.DefaultBehaviour();
                        resultMessage.DisplayMessage =s;
                        resultMessage.Result = 0;
                        return resultMessage;
                    }
                    if (result == 0)
                    {
                        string TxnId = Startup.CompanyObject.GetNewObjectKey();
                        if (Startup.CompanyObject.InTransaction)
                            Startup.CompanyObject.EndTransaction(BoWfTransOpt.wf_Commit);

                        resultMessage.Tag = TxnId;
                        resultMessage.DefaultSuccessBehaviour();

                        resultMessage.MessageType = ResultMessageE.Information;
                        resultMessage.DisplayMessage = "BOM Created successfully";
                        resultMessage.Result = 1;
                    }
                }
                
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
                

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "BOM");
                return resultMessage;
            }
        }

        public ResultMessage GetBOM(int prodItemId)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                SAPbobsCOM.ProductTrees oPO;
                oPO = (SAPbobsCOM.ProductTrees)(Startup.CompanyObject.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oProductTrees));
              
                Boolean result = oPO.GetByKey(prodItemId.ToString());
                string itemId = oPO.TreeCode;
                if (result == true)
                {
                    resultMessage.MessageType = ResultMessageE.Information;
                    resultMessage.DisplayMessage = "BOM already Exist";
                    resultMessage.Result = 1;
                }
                else
                {
                    resultMessage.MessageType = ResultMessageE.Information;
                    resultMessage.DisplayMessage = "BOM Not Exist";
                    resultMessage.Result = 0;
                }
                return resultMessage;
            }
            catch (Exception ex)
            {
                string errorMsg = Startup.CompanyObject.GetLastErrorDescription();
                resultMessage.DefaultExceptionBehaviour(ex, "SAP EX");
                return resultMessage;

            }


        }
        public List<TblProductItemTO> SelectAllProductItemListWrtSubGroupOrBaseItem(Int32 prodClassId = 0, int baseItemId = 0,int NonListedType=1)
        {

            TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.IS_SHOW_UOM_AND_COUM_DDL);
            List<TblProductItemTO> TblProductItemTOListFinal = new List<TblProductItemTO>();
            int isShowConvUOM = 0;
            if(tblConfigParamsTO != null)
            {
                isShowConvUOM =Convert.ToInt16(tblConfigParamsTO.ConfigParamVal);
            }
            List <TblProductItemTO> TblProductItemTOList= _iTblProductItemDAO.SelectAllProductItemListWrtSubGroupOrBaseItem(prodClassId, baseItemId, NonListedType, isShowConvUOM);
            if (NonListedType == 1)
                TblProductItemTOListFinal= TblProductItemTOList;
            else if(NonListedType ==2  && TblProductItemTOList[0].BaseProdItemId ==0)
            {
                TblProductItemTOListFinal= TblProductItemTOList;
            }
            else if(NonListedType == 2 && TblProductItemTOList[0].BaseProdItemId !=0)
            {
                TblProductItemTOListFinal = TblProductItemTOList.Where(w => w.IsNonListed == false).ToList();
                //TblProductItemTOListFinal = TblProductItemTOList;
            }
            else if (NonListedType == 3 && TblProductItemTOList[0].BaseProdItemId == 0)
            {
                return TblProductItemTOListFinal=TblProductItemTOList;
            }
            else if (NonListedType == 3 && TblProductItemTOList[0].BaseProdItemId != 0)
            {
                TblProductItemTOListFinal = TblProductItemTOList.Where(w => w.IsNonListed == true).ToList();
                //TblProductItemTOListFinal = TblProductItemTOList;
            }
            return TblProductItemTOListFinal;
            //return _iTblProductItemDAO.SelectAllProductItemListWrtSubGroupOrBaseItem(prodClassId, baseItemId, NonListedType);
        }
        //
        public TblProductItemTO SelectTblProductItemFromOrignalItem(Int32 idProdItem,SqlConnection conn,SqlTransaction tran)
        {
            return _iTblProductItemDAO.SelectTblProductItemFromOrignalItem(idProdItem, conn, tran);
        }

        public TblProductItemTO SelectTblProductItemFromOrignalItem(Int32 idProdItem)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return _iTblProductItemDAO.SelectTblProductItemFromOrignalItem(idProdItem, conn, tran);

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }
        public TblProductItemTO SelectTblProductItemTO(Int32 idProdItem)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return _iTblProductItemDAO.SelectTblProductItem(idProdItem, conn, tran);

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public TblProductItemTO SelectTblProductItemTO(Int32 idProdItem, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblProductItemDAO.SelectTblProductItem(idProdItem, conn, tran);
        }
        public List<TblProductItemTO> GetProductItemDetailsForPurchaseItem(string idProdItems)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return _iTblProductItemDAO.GetProductItemDetailsForPurchaseItem(idProdItems, conn, tran);

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }
        public List<DropDownTO> GetMaxPriorityItemSupplier(string idProdItems)
        {
            return _iTblProductItemDAO.GetMaxPriorityItemSupplier(idProdItems);
        }
        //public List<DropDownTO> GetPriorityOneItemSupplier(string idProdItems)
        //{
        //    return _iTblProductItemDAO.GetPriorityOneItemSupplier(idProdItems);
        //}


        /// <summary>
        /// Sudhir[11-Jan-2017] Added for Get List of Items Based on isStockRequire Flag
        /// </summary>
        /// <param name="isStockRequire"></param>
        /// <returns></returns>
        public List<TblProductItemTO> SelectProductItemListStockUpdateRequire(int isStockRequire)
        {
            return _iTblProductItemDAO.SelectProductItemListStockUpdateRequire(isStockRequire);
        }

        public List<TblProductItemTO> SelectProductItemListStockTOList(int activeYn, int PageNumber, int RowsPerPage, string strsearchtxt)
        {
            return _iTblProductItemDAO.SelectProductItemListStockTOList(activeYn,  PageNumber,  RowsPerPage,  strsearchtxt);
        }

        /// <summary>
        /// Sudhir[15-MAR-2018]
        /// Get List of ProductItemTO Based On Category/Subcategory or Specification
        /// </summary>
        /// <param name="tblProductItemTO"></param>
        /// <returns></returns>
        /// 
        public List<TblProductItemTO> SelectProductItemList(Int32 idProdClass)
        {
            string prodClassIds = _iTblProdClassificationBL.SelectProdtClassificationListOnType(idProdClass);
            if (prodClassIds != string.Empty)
            {
                return _iTblProductItemDAO.SelectListOfProductItemTOOnprdClassId(prodClassIds);
            }
            else
                return null;
        }



        /// <summary>
        /// Priyanka H [09-07-2019]
        /// Get List of ProductItemTO Based On Category/Subcategory or Specification Or prodItemId
        /// </summary>
        /// <param name="tblProductItemTO"></param>
        /// <returns></returns>
        /// 
        public List<TblProductItemTO> SelectProductItemList(Int32 idProdClass, Int32 subCategoryId, Int32 itemID, Int32 procurementId = 0, Int32 ProductTypeId = 0, int isShowListed = 0, string warehouseIds = "")
        {
            string prodClassIds = _iTblProdClassificationBL.SelectProdtClassificationListOnType(idProdClass, subCategoryId, itemID);
            if (prodClassIds != string.Empty)
            {
                return _iTblProductItemDAO.SelectListOfProductItemTOOnprdClassIds(prodClassIds,procurementId, ProductTypeId, isShowListed,warehouseIds);
            }
            else
                return null;
        }
        //Reshma[12-03-21] Added For Stock view report
        public List<TblProductItemTO> SelectProductItemFromScrapCategoryList(Int32 idProdClass, Int32 subCategoryId, Int32 itemID, Int32 procurementId = 0)
        {
            List<DropDownTO> DropDownTOList = _iDimensionDAO.GetItemProductCategoryListForDropDown();
             DropDownTOList = DropDownTOList.Where(w => w.isScrapProdItem == true).ToList();
            int scrapCateId = 0; string prodClassIds = string.Empty;
            if (DropDownTOList != null)
            {
                scrapCateId = DropDownTOList[0].Value;
            }
            List<DropDownTO> tblProdClassificationTOList = _iTblProdClassificationBL.getProdClassIdsByItemProdCat(scrapCateId, "C");
            if (tblProdClassificationTOList != null && tblProdClassificationTOList.Count >0)
            {
                for(int i=0;i< tblProdClassificationTOList.Count;i++)
                {
                    DropDownTO DropDownTO = tblProdClassificationTOList[i];
                    prodClassIds += _iTblProdClassificationBL.SelectProdtClassificationListOnType(DropDownTO.Value, 0, 0);

                }
            }
            //string prodClassIds = _iTblProdClassificationBL.SelectProdtClassificationListOnType(idProdClass, subCategoryId, itemID);
            if (prodClassIds != string.Empty)
            {
                return _iTblProductItemDAO.SelectListOfProductItemTOOnprdClassIds(prodClassIds, procurementId);
            }
            else
                return null;
        }

        /// <summary>
        /// Sudhir[20-MAR-2018]  Added for Get ProductItem List which has Parity Yes.
        /// </summary>
        /// <param name="isParity"></param>
        /// <returns></returns>
        /// 
        public List<DropDownTO> SelectProductItemListIsParity(Int32 isParity)
        {
            List<DropDownTO> list = _iTblProductItemDAO.SelectProductItemListIsParity(isParity);
            if (list != null)
            {
                return list;
            }
            else
                return new List<DropDownTO>();
        }
        public List<DropDownTO> GetProductItemDropDownList()
        {
            return _iTblProductItemDAO.GetProductItemDropDownList();
        }

        public List<DropDownTO> GetGradeDropDownList(Int32 specificationId)
        {
            return _iTblProductItemDAO.GetGradeDropDownList(specificationId);
        }

        /// <summary>
        /// Hudekar Priyanka [04-march-19] - To Get Purchase Tab Related Information for given item
        /// </summary>
        /// <param name="prodItemId"></param>
        /// <returns></returns>
        public TblPurchaseItemMasterTO SelectTblPurchaseItemMasterTO(Int32 prodItemId)
        {
            // return TblProductItemPurchaseDAO.SelectTblPurchaseItemMaster(prodItemId);
            return _iTblProductItemDAO.SelectTblPurchaseItemMaster(prodItemId);
        }
        public List<TblPurchaseItemMasterTO> SelectProductItemPurchaseDataAllList(Int32 prodItemId)
        {
            // return TblProductItemPurchaseDAO.SelectTblPurchaseItemMaster(prodItemId);
            return _iTblProductItemDAO.SelectProductItemPurchaseDataAllList(prodItemId);
        }

        // Add By  Samadhan 13 May 2022
        public List<StoresLocationTO> SelectItemStoreLocList(Int32 prodItemId)
        {            
            return _iTblProductItemDAO.SelectItemStoreLocList(prodItemId);
        }

        /// <summary>
        /// Priyank H [15-03-2019] To Search In Item Master based on given criteria
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="itemNo"></param>
        /// <param name="categoryNo"></param>
        /// <returns></returns>
        public List<TblProductItemTO> SearchProductItemListOld(string itemName = "", Int32 itemNo = 0, Int32 categoryNo = 0, Int32 subCategoryId = 0, Int32 itemID = 0)
        {
            if (categoryNo != 0 && (itemNo != 0 || itemName != "undefined"))
            {
                // return TblProductItemBL.SelectProductItemList(categoryNo);
                if (categoryNo != 0 && !String.IsNullOrEmpty(itemName))
                {
                    //List<TblProductItemTO> list = TblProductItemBL.SelectProductItemList(categoryNo);
                    List<TblProductItemTO> list = SelectProductItemList(categoryNo, subCategoryId, itemID);
                    if (list != null && list.Count > 0)
                    {
                        // return TblProductItemDAO.SearchTblProductItem(itemName, itemNo, categoryNo);//working line
                        //List<TblProductItemTO> listOfItemfilter = TblProductItemDAO.SearchTblProductItem(itemName, itemNo, categoryNo);
                        List<TblProductItemTO> listOfItemfilter = _iTblProductItemDAO.SearchTblProductItem(itemName, itemNo, categoryNo, null);

                        listOfItemfilter.AddRange(list);
                        // return listOfItemfilter;
                        // listOfItemfilter.Select(s => s.IdProdItem).Distinct();
                        List<TblProductItemTO> distinctList = listOfItemfilter.GroupBy(g => g.IdProdItem).Select(s => s.FirstOrDefault()).ToList();

                        return distinctList;

                    }
                }
            }
            if (categoryNo != 0 && itemNo == 0 && itemName == "undefined")
            {
                return SelectProductItemList(categoryNo, subCategoryId, itemID);
            }

            if (categoryNo != 0 && itemNo == 0 && String.IsNullOrEmpty(itemName))
            {
                return SelectProductItemList(categoryNo, subCategoryId, itemID);
            }


            // return TblProductItemDAO.SearchTblProductItem(itemName,itemNo,categoryNo);
            return _iTblProductItemDAO.SearchTblProductItem(itemName, itemNo, categoryNo,null);



        }

        public List<TblProductItemTO> SearchProductItemList(string itemName = "", Int32 itemNo = 0, Int32 categoryNo = 0, Int32 subCategoryId = 0, Int32 subSubCat = 0, string warehouseIds = "", Int32 procurementId = 0, Int32 ProductTypeId = 0, int isShowListed = 0, Int32 isShowMinLevelItem = 1)
        {
            if (itemName != null && itemName == "undefined")
            {
                itemName = String.Empty;
            }

            List<TblProductItemTO> itemList = new List<TblProductItemTO>();
            if (categoryNo > 0)
            {
                List<TblProductItemTO> list = SelectProductItemList(categoryNo, subCategoryId, subSubCat,procurementId, ProductTypeId, isShowListed,warehouseIds);
                if (list != null && list.Count > 0)
                {
                    if (!String.IsNullOrEmpty(itemName))
                        list = list.Where(w => w.ItemName.Contains(itemName)).ToList();

                    itemList = list;
                }
            }
            else
            {
                itemList =  _iTblProductItemDAO.SearchTblProductItem(itemName, itemNo, categoryNo,warehouseIds,procurementId, ProductTypeId, isShowListed);
                //List<TblProdItemMakeBrandTO> TblProdItemMakeBrandTOList = _iITblProdItemMakeBrandBL.SelectedProdItemMakeBrand(itemNo);
                //itemList[0].ProdItemMakeBrandTOList = TblProdItemMakeBrandTOList;
                //List<TblProductItemTO> itemList = TblProdItemMakeBrandTOList;
            }

            if (itemList != null && isShowMinLevelItem == 0)
            {
                for (int i = 0; i < itemList.Count; i++)
                {
                    TblProductItemTO tblProductItemTO = itemList[i];
                    if (tblProductItemTO.IdProdItem == 75870)
                    {
                    }
                    if (tblProductItemTO.IsManageInventory == false)
                    {
                        if (tblProductItemTO.InventMinimum > 0)
                        {
                            itemList.Remove(tblProductItemTO);
                        }
                    }
                }
                //itemList = itemList.Where(e => e.InventMinimum == 0 && e.IsManageInventory).ToList();
            }
            return itemList;
        }

        //Reshma[12-03-21] For Stock view report
        public List<TblProductItemTO> GetSearchProductItemForStockView(string itemName = "", Int32 itemNo = 0, Int32 categoryNo = 0, Int32 subCategoryId = 0, Int32 subSubCat = 0, string warehouseIds = "", Int32 procurementId = 0)
        {
            if (itemName != null && itemName == "undefined")
            {
                itemName = String.Empty;
            }

            List<TblProductItemTO> itemList = new List<TblProductItemTO>();
            if (categoryNo > 0)
            {
                List<TblProductItemTO> list = SelectProductItemList(categoryNo, subCategoryId, subSubCat, procurementId);
                if (list != null && list.Count > 0)
                {
                    if (!String.IsNullOrEmpty(itemName))
                        list = list.Where(w => w.ItemName.Contains(itemName)).ToList();

                    itemList = list;
                }
            }
            else
            {
                itemList = _iTblProductItemDAO.SearchTblProductItem(itemName, itemNo, categoryNo, warehouseIds, procurementId);
            }
            if(itemList !=null && itemList.Count>0)
            {
                List<TblProductItemTO> ScrapItemList =SelectProductItemFromScrapCategoryList(categoryNo, subCategoryId, subSubCat, procurementId);
                if(ScrapItemList !=null && ScrapItemList.Count >0)
                {
                    for(int i=0;i< ScrapItemList.Count;i++)
                    {
                        TblProductItemTO tblProductItemTO = ScrapItemList[i];
                        if(tblProductItemTO !=null && tblProductItemTO.OrignalProdItemId >0)
                        {
                            List<TblProductItemTO> originalItemList = itemList.Where(w => w.IdProdItem == tblProductItemTO.OrignalProdItemId).ToList();
                            if (originalItemList.Count > 0 && originalItemList !=null)
                            {
                                itemList.Add(tblProductItemTO);
                            }
                        }
                    }
                }
            }
            return itemList;
        }

        /*public List<TblProductItemTO> SearchProductItemList(string itemName = "", Int32 itemNo = 0, Int32 categoryNo = 0)
        {
            if (categoryNo != 0 && (itemNo != 0 || itemName != "undefined"))
            {
                // return TblProductItemBL.SelectProductItemList(categoryNo);
                if (categoryNo != 0 && !String.IsNullOrEmpty(itemName))
                {
                    //List<TblProductItemTO> list = TblProductItemBL.SelectProductItemList(categoryNo);
                    List<TblProductItemTO> list = SelectProductItemList(categoryNo);
                    if (list != null && list.Count > 0)
                    {
                        // return TblProductItemDAO.SearchTblProductItem(itemName, itemNo, categoryNo);//working line
                        //List<TblProductItemTO> listOfItemfilter = TblProductItemDAO.SearchTblProductItem(itemName, itemNo, categoryNo);
                        List<TblProductItemTO> listOfItemfilter = _iTblProductItemDAO.SearchTblProductItem(itemName, itemNo, categoryNo);

                        listOfItemfilter.AddRange(list);
                        // return listOfItemfilter;
                        // listOfItemfilter.Select(s => s.IdProdItem).Distinct();
                        List<TblProductItemTO> distinctList = listOfItemfilter.GroupBy(g => g.IdProdItem).Select(s => s.FirstOrDefault()).ToList();

                        return distinctList;

                    }
                }
            }
            if (categoryNo != 0 && itemNo == 0 && itemName == "undefined")
            {
                return SelectProductItemList(categoryNo);
            }

            if (categoryNo != 0 && itemNo == 0 && String.IsNullOrEmpty(itemName))
            {
                return SelectProductItemList(categoryNo);
            }


            // return TblProductItemDAO.SearchTblProductItem(itemName,itemNo,categoryNo);
            return _iTblProductItemDAO.SearchTblProductItem(itemName, itemNo, categoryNo);



        }

        */
        public List<TblProductItemTO> SelectListOfProductItemTOOnSearchString(string searchString, Int32 ProductTypeId = 0)
        {
            //  TblConfigParamsTO tblConfigParamsTOSAPService = _iTblConfigParamsDAO.SelectTblConfigParams(Constants.SAPB1_SERVICES_ENABLE);
           return _iTblProductItemDAO.SelectListOfProductItemTOOnSearchString(searchString, ProductTypeId);
        }

        //Dhananjay [2021-07-28] added for check item is properly configured
        public ResultMessage IsItemProperlyConfigured(Int32 idProdItem)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                string sFieldsNotConfigured = "";
                string sFieldsNotConfiguredInSAP = "";
                string sErrMsgForPrioritySupplier = "";

                TblProductItemTO tblProductItemTO = _iTblProductItemDAO.SelectTblProductItem(idProdItem);
                if (tblProductItemTO == null)
                {
                    resultMessage.DefaultBehaviour("Item " + idProdItem + " not found");
                    return resultMessage;
                }
                else
                {
                    if (tblProductItemTO.IsProperSAPItem != 1)
                    {
                        sFieldsNotConfigured += "IsProperSAPItem,";
                    }
                    if (tblProductItemTO.UOM == "")
                    {
                        sFieldsNotConfigured += "UOM,";
                    }
                    if (tblProductItemTO.UOMGroupId == 0)
                    {
                        sFieldsNotConfigured += "UOM Group,";
                    }
                    if (tblProductItemTO.ConversionFactor == 0)
                    {
                        sFieldsNotConfigured += "Conversion Factor,";
                    }
                    if (tblProductItemTO.ConversionUnitOfMeasureId == 0)
                    {
                        sFieldsNotConfigured += "Conversion Unit Of Measure,";
                    }
                    if (tblProductItemTO.IsPurchaseItem == 0)
                    {
                        sFieldsNotConfigured += "Is Purchase Item,";
                    }
                    if (tblProductItemTO.GstCodeId == 0)
                    {
                        sFieldsNotConfigured += "Gst Code,";
                    }
                    if (tblProductItemTO.GstSapHsnCode == "")
                    {
                        sFieldsNotConfigured += "Gst Sap HSN Code,";
                    }

                    if (tblProductItemTO.CodeTypeId == 1)
                    {
                        if (tblProductItemTO.HSNCode == 0)
                        {
                            sFieldsNotConfigured += "HSN Code,";
                        }
                        //if (tblProductItemTO.GstTaxPct == 0)
                        //{
                        //    sFieldsNotConfigured += "Gst Tax Percentage,";
                        //}
                    }
                    else if (tblProductItemTO.CodeTypeId == 2)
                    {
                        if (tblProductItemTO.SACCode == 0)
                        {
                            sFieldsNotConfigured += "SAC Code,";
                        }
                    }

                    TblPurchaseItemMasterTO tblPurchaseItemMasterTO = _iTblProductItemDAO.SelectTblPurchaseItemMaster(idProdItem);
                    if (tblPurchaseItemMasterTO == null)
                    {
                        sFieldsNotConfigured += "Priority Supplier,";
                    }
                    else
                    {
                        if (tblPurchaseItemMasterTO.SupplierOrgId == 0)
                        {
                            sFieldsNotConfigured += "Priority Supplier,";
                        }
                        else
                        {
                            resultMessage = _iTblOrganizationBL.CheckIfOrgIsAvailableInSAP(tblPurchaseItemMasterTO.SupplierOrgId);
                            if (resultMessage.MessageType == ResultMessageE.Error)
                            {
                                sErrMsgForPrioritySupplier = resultMessage.DisplayMessage;
                            }
                            if (tblPurchaseItemMasterTO.PurchaseUOMId == 0)
                            {
                                sFieldsNotConfigured += "Purchase UOM,";
                            }
                            if (tblPurchaseItemMasterTO.BasicRate == 0)
                            {
                                sFieldsNotConfigured += "Basic Rate,";
                            }
                        }
                    }

                    Boolean SAPServiceEnable = false;
                    TblConfigParamsTO tblConfigParamsTOSAPService = _iTblConfigParamsDAO.SelectTblConfigParams(Constants.SAPB1_SERVICES_ENABLE);
                    if (tblConfigParamsTOSAPService != null)
                    {
                        if (Convert.ToInt32(tblConfigParamsTOSAPService.ConfigParamVal) == 1)
                        {
                            SAPServiceEnable = true;
                        }
                    }
                    if (SAPServiceEnable == true)
                    {
                        SAPItemTO sAPItemTO = _iTblProductItemDAO.GetItemFromSap(idProdItem.ToString());
                        if (sAPItemTO == null)
                        {
                            resultMessage.DefaultBehaviour("Item " + idProdItem + " not found in SAP");
                            resultMessage.DisplayMessage ="Item " + idProdItem + " not found in SAP";
                            return resultMessage;
                        }
                        else
                        {
                            if (sAPItemTO.CardCode == "")
                            {
                                sFieldsNotConfiguredInSAP += "Priority Supplier in SAP,";
                            }
                            if (sAPItemTO.PrchseItem != "Y")
                            {
                                sFieldsNotConfiguredInSAP += "Is Purchase Item in SAP,";
                            }
                            if (sAPItemTO.UgpEntry == 0)
                            {
                                sFieldsNotConfiguredInSAP += "UOM Group in SAP,";
                            }
                            if (sAPItemTO.IUoMEntry == 0)
                            {
                                sFieldsNotConfiguredInSAP += "UOM in SAP,";
                            }
                            if (sAPItemTO.PUoMEntry == 0)
                            {
                                sFieldsNotConfiguredInSAP += "Purchase UOM in SAP,";
                            }
                            if (sAPItemTO.GSTRelevnt == "Y")
                            {
                                if (sAPItemTO.GstTaxCtg == "N")
                                {
                                    sFieldsNotConfiguredInSAP += "GST Tax Category in SAP,";
                                }
                                if (sAPItemTO.ItemClass == "1")
                                {
                                    if (sAPItemTO.SACEntry == -1)
                                    {
                                        sFieldsNotConfiguredInSAP += "SAC in SAP,";
                                    }
                                }
                                else if (sAPItemTO.ItemClass == "2")
                                {
                                    if (sAPItemTO.ChapterID == -1)
                                    {
                                        sFieldsNotConfiguredInSAP += "HSN in SAP,";
                                    }
                                }
                            }
                        }
                    }
                }

                sFieldsNotConfigured = sFieldsNotConfigured.TrimEnd(',');
                sFieldsNotConfiguredInSAP = sFieldsNotConfiguredInSAP.TrimEnd(',');

                string sErrMsg = "";
                if(!string.IsNullOrEmpty(sFieldsNotConfigured))
                {
                    sErrMsg = "Item " + idProdItem + " is not properly configured. Following fields are missing: \n" + sFieldsNotConfigured;
                }
                if (!string.IsNullOrEmpty(sErrMsgForPrioritySupplier)) //errMsg from CheckIfOrgIsAvailableInSAP function
                {
                    sErrMsg += "\n\n" + sErrMsgForPrioritySupplier;
                }
                if (!string.IsNullOrEmpty(sFieldsNotConfiguredInSAP))
                {
                    string sErrMsgForSAP = "Item " + idProdItem + " is not properly configured in SAP. Following fields are missing: \n" + sFieldsNotConfiguredInSAP;
                    if (sErrMsg == "")
                    {
                        sErrMsg = sErrMsgForSAP;
                    }
                    else
                    {
                        sErrMsg += "\n\n" + sErrMsgForSAP;
                    }
                }

                if (sErrMsg != "")
                {
                    sErrMsg += "\n\n" + "Please update the item and try again.";

                    resultMessage.DefaultBehaviour();
                    resultMessage.DisplayMessage = sErrMsg;
                    resultMessage.Text = sErrMsg;
                }
                else
                {
                    resultMessage.DefaultSuccessBehaviour();
                }
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "");
                return resultMessage;
            }
        }
        #endregion

        #region Insertion
        //chetan[2020-10-01] added for copy Paste Item
        public ResultMessage CopyPastMakeItem(int prodItemId, long baseProdItemId ,int userId)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                string prodStartName = "SCRAP ";
                TblProductItemTO tblProductMakeItemTO = SelectTblProductItemTO(prodItemId);
                TblProductItemTO tblProductBaseItemTO = SelectTblProductItemTO(prodItemId);
                tblProductMakeItemTO.BaseProdItemId = baseProdItemId;
                TblProductItemTO tblProductItemTO = SelectTblProductItemFromOrignalItem(prodItemId);
                List<TblProdClassificationTO> tblProdClassificationTOList = new List<TblProdClassificationTO>();
                TblProdClassificationTO tblProdClassificationTO = _iTblProdClassificationBL.SelectTblProdClassificationTO(tblProductItemTO.ProdClassId);

                GetProdClassificationHierarchy(tblProdClassificationTOList, tblProdClassificationTO);
                List<TblProdClassificationTO> tblProdClassificationTOTempList = tblProdClassificationTOList
                                                    .Where(w => w.ParentProdClassId == 0).ToList();

                tblProductMakeItemTO.SapItemGroupId = tblProdClassificationTOTempList[0].MappedTxnId;
                tblProductMakeItemTO.CreatedBy = userId;
                tblProductMakeItemTO.CreatedOn = _iCommon.ServerDateTime;
                tblProductMakeItemTO.ItemName = prodStartName + tblProductMakeItemTO.ItemName;
                tblProductMakeItemTO.ItemDesc = prodStartName + tblProductMakeItemTO.ItemDesc;
                tblProductMakeItemTO.RefProdItemId = prodItemId;
                tblProductMakeItemTO.RevisionNo = 1;
                tblProductMakeItemTO.IsHavingNewRev = 1;
                tblProductMakeItemTO.IdProdItem = 0;
                TblLocationTO TblLocationTO = _iTblLocationBL.SelectTblLocationTO(tblProductMakeItemTO.LocationId);
                tblProductMakeItemTO.MappedLocationId = Convert.ToInt32(TblLocationTO.MappedTxnId);
                List<TblPurchaseItemMasterTO> tblPurchaseItemMasterTOList = new List<TblPurchaseItemMasterTO>();
                resultMessage = InsertTblProductItem(tblProductMakeItemTO, tblPurchaseItemMasterTOList, null,null);
                if (resultMessage.MessageType != ResultMessageE.Information)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.DisplayMessage = "Error in insert InsertTblProductItem";
                    return resultMessage;
                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "CopyPastMakeItem");
                return resultMessage;
            }
        }
        //Reshma Added 
        public ResultMessage  CopyPastMakeItem(int prodItemId, long baseProdItemId, int userId,SqlConnection  conn ,SqlTransaction tran,ref int newProdItemId)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                string prodStartName = "";
                TblProductItemTO tblProductMakeItemTO = SelectTblProductItemTO(prodItemId,conn,tran);
                TblProductItemTO tblProductBaseItemTO = SelectTblProductItemTO(prodItemId,conn,tran);
                tblProductMakeItemTO.BaseProdItemId = baseProdItemId;
                //TblProductItemTO tblProductItemTO = SelectTblProductItemFromOrignalItem(prodItemId);
                List<TblProdClassificationTO> tblProdClassificationTOList = new List<TblProdClassificationTO>();
                TblProdClassificationTO tblProdClassificationTO = _iTblProdClassificationBL.SelectTblProdClassificationTOV2(tblProductMakeItemTO.ProdClassId,conn,tran);

                GetProdClassificationHierarchy(tblProdClassificationTOList, tblProdClassificationTO,conn,tran);
                List<TblProdClassificationTO> tblProdClassificationTOTempList = tblProdClassificationTOList
                                                    .Where(w => w.ParentProdClassId == 0).ToList();

                tblProductMakeItemTO.SapItemGroupId = tblProdClassificationTOTempList[0].MappedTxnId;
                tblProductMakeItemTO.CreatedBy = userId;
                tblProductMakeItemTO.CreatedOn = _iCommon.ServerDateTime;
                tblProductMakeItemTO.ItemName = prodStartName + tblProductMakeItemTO.ItemName;
                tblProductMakeItemTO.ItemDesc = prodStartName + tblProductMakeItemTO.ItemDesc;
                tblProductMakeItemTO.RefProdItemId = prodItemId;
                tblProductMakeItemTO.RevisionNo = 1;
                tblProductMakeItemTO.IsHavingNewRev = 1;
                tblProductMakeItemTO.IdProdItem = 0;
                tblProductMakeItemTO.IsActive = 1;
                tblProductMakeItemTO.CreatedBy = tblProductMakeItemTO.UpdatedBy;
               
                TblLocationTO TblLocationTO = _iTblLocationBL.SelectTblLocationTO(tblProductMakeItemTO.LocationId);
                tblProductMakeItemTO.MappedLocationId = Convert.ToInt32(TblLocationTO.MappedTxnId);
                List<TblPurchaseItemMasterTO> tblPurchaseItemMasterTOList = new List<TblPurchaseItemMasterTO>();
                resultMessage = InsertTblProductItem(tblProductMakeItemTO, tblPurchaseItemMasterTOList,conn,tran);
                if (resultMessage.MessageType != ResultMessageE.Information)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.DisplayMessage = "Error in insert InsertTblProductItem";
                    return resultMessage;
                }
                if (tblProductMakeItemTO.IdProdItem > 0)
                    newProdItemId = tblProductMakeItemTO.IdProdItem;
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "CopyPastMakeItem");
                return resultMessage;
            }
        }
        public ResultMessage CopyPastItemScrapItem(int prodItemId, int userId)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                TblProductItemTO tblProductItemTO = SelectTblProductItemTO(prodItemId);
                if (tblProductItemTO == null)
                {
                    resultMessage.DefaultBehaviour();
                    return resultMessage;
                }
                if (tblProductItemTO.IsHaveScrapProdItem)
                {
                    tblProductItemTO = SelectTblProductItemFromOrignalItem(tblProductItemTO.IdProdItem);
                    resultMessage.DefaultSuccessBehaviour();
                    resultMessage.Tag = tblProductItemTO;
                    return resultMessage;
                }
                //get base item 
                List<TblProdClassificationTO> tblProdClassificationTOList = new List<TblProdClassificationTO>();
                List<TblProductItemTO> tblProductItemTOList = new List<TblProductItemTO>();
                TblProdClassificationTO tblProdClassificationTO = _iTblProdClassificationBL.SelectTblProdClassificationTO(tblProductItemTO.ProdClassId);
                ///TblProdClassificationTO tblProdClassificationScrapTO = _iTblProdClassificationBL.SelectTblProdClassification(true, 0);
                DimProdCatTO dimProdCatTO = _iDimProdCatBL.SelectDimProdCatTO(true);
                TblProdClassificationTO tblProdClassificationScrapTO = new TblProdClassificationTO();
                if (dimProdCatTO == null)
                {
                    resultMessage.DefaultSuccessBehaviour();
                    resultMessage.DisplayMessage = "Scrap category not define.";
                    return resultMessage;
                }
                int prodCatId = dimProdCatTO.IdProdCat;
                GetProdClassificationHierarchy(tblProdClassificationTOList, tblProdClassificationTO);
                GetProductionItemHierarchy(tblProductItemTOList, tblProductItemTO);

                String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
                SqlConnection conn = new SqlConnection(sqlConnStr);
                SqlTransaction tran = null;
                try
                {
                    conn.Open();
                    tran = conn.BeginTransaction();
                    Startup.CompanyObject.StartTransaction();
                    string baseItemDisplayName = tblProdClassificationScrapTO.ProdClassDesc;
                    string prodStartName = "SCRAP ";
                    string sapItemGroupId = string.Empty;
                    if (tblProdClassificationTOList != null && tblProdClassificationTOList.Count > 0)
                    {
                        for (int i = 0; i < tblProdClassificationTOList.Count; i++)
                        {
                            TblProdClassificationTO tblProdClassificationExitTO = tblProdClassificationTOList[i];
                            string productDesc = prodStartName + tblProdClassificationExitTO.ProdClassDesc.Trim();
                            if (productDesc.Length > 20)
                                productDesc = productDesc.Substring(0, 20);
                            TblProdClassificationTO tblProdClassificationScrapExitTO = _iTblProdClassificationBL.SelectTblProdClassification(productDesc,tblProdClassificationScrapTO.IdProdClass,prodCatId,conn,tran);
                            if (tblProdClassificationScrapExitTO == null)
                            {
                                tblProdClassificationScrapExitTO = tblProdClassificationExitTO.DeepCopy();
                                tblProdClassificationScrapExitTO.IdProdClass = 0;
                                tblProdClassificationScrapExitTO.ItemProdCatId = prodCatId;
                                tblProdClassificationScrapExitTO.CreatedOn = _iCommon.ServerDateTime;
                                tblProdClassificationScrapExitTO.CreatedBy = userId;
                                if(tblProdClassificationScrapTO.IdProdClass>0)
                                   tblProdClassificationScrapExitTO.ParentProdClassId = tblProdClassificationScrapTO.IdProdClass;
                                tblProdClassificationScrapExitTO.ProdClassDesc = productDesc;// prodStartName + tblProdClassificationScrapExitTO.ProdClassDesc;
                                //if (tblProdClassificationScrapExitTO.ProdClassDesc.Length > 20)
                                //    tblProdClassificationScrapExitTO.ProdClassDesc = tblProdClassificationScrapExitTO.ProdClassDesc.Substring(0,20);
                                baseItemDisplayName += "/" + tblProdClassificationScrapExitTO.ProdClassDesc;
                                tblProdClassificationScrapExitTO.DisplayName = baseItemDisplayName;
                                tblProdClassificationScrapExitTO.ProdClassType = tblProdClassificationScrapExitTO.ProdClassType.Trim();
                                resultMessage = _iTblProdClassificationBL.InsertProdClassification(tblProdClassificationScrapExitTO, conn, tran);
                                if (resultMessage.MessageType != ResultMessageE.Information)
                                {
                                    tran.Rollback();
                                    if (Startup.CompanyObject.InTransaction)
                                        Startup.CompanyObject.EndTransaction(BoWfTransOpt.wf_RollBack);
                                    resultMessage.DefaultBehaviour();
                                    resultMessage.DisplayMessage = "Error in insert InsertProdClassification";
                                    return resultMessage;
                                }
                            }
                            else
                            {
                                baseItemDisplayName += "/" + tblProdClassificationScrapExitTO.ProdClassDesc;
                                tblProdClassificationScrapExitTO.DisplayName = baseItemDisplayName;
                            }
                            if(!string.IsNullOrEmpty(tblProdClassificationScrapExitTO.MappedTxnId))
                            {
                                sapItemGroupId = tblProdClassificationScrapExitTO.MappedTxnId;
                            }
                            tblProdClassificationScrapTO = tblProdClassificationScrapExitTO;
                        }
                        if (tblProductItemTOList != null && tblProductItemTOList.Count > 0)
                        {
                            TblProductItemTO TblProductItemScrapTO = new TblProductItemTO();
                            for (int i = 0; i < tblProductItemTOList.Count; i++)
                            {
                                TblProductItemTO tblProductItemExistTO = tblProductItemTOList[i];
                                TblProductItemTO TblProductItemExistScrapTO = SelectTblProductItemFromOrignalItem(tblProductItemExistTO.IdProdItem, conn, tran);
                                if (TblProductItemExistScrapTO == null)
                                {
                                    TblProductItemExistScrapTO = tblProductItemExistTO.DeepCopy();
                                    TblProductItemExistScrapTO.ValuationId = (int)SAPbobsCOM.BoInventorySystem.bis_Standard;
                                    TblProductItemExistScrapTO.ItemName = prodStartName + TblProductItemExistScrapTO.ItemName;
                                    TblProductItemExistScrapTO.ItemDesc = prodStartName + TblProductItemExistScrapTO.ItemDesc;
                                    TblProductItemExistScrapTO.ProdClassId = tblProdClassificationScrapTO.IdProdClass;
                                    TblProductItemExistScrapTO.CreatedOn = _iCommon.ServerDateTime;
                                    TblProductItemExistScrapTO.CreatedBy = userId;
                                    TblProductItemExistScrapTO.UpdatedBy = userId;
                                    TblProductItemExistScrapTO.UpdatedOn = _iCommon.ServerDateTime;
                                    TblProductItemExistScrapTO.BaseProdItemId = TblProductItemScrapTO.IdProdItem;
                                    TblProductItemExistScrapTO.OrignalProdItemId = tblProductItemExistTO.IdProdItem;
                                    TblProductItemExistScrapTO.IdProdItem = 0;
                                    TblProductItemExistScrapTO.SapItemGroupId = "106";
                                    if (!string.IsNullOrEmpty(sapItemGroupId))
                                        TblProductItemExistScrapTO.SapItemGroupId = sapItemGroupId;

                                    if (tblProductItemExistTO.BaseProdItemId > 0)
                                    {
                                        if (tblProductItemExistTO.ScrapStoreLocationId < 1)//Chetan
                                        {
                                            tran.Rollback();
                                            if (Startup.CompanyObject.InTransaction)
                                                Startup.CompanyObject.EndTransaction(BoWfTransOpt.wf_RollBack);
                                            resultMessage.DefaultBehaviour();
                                            resultMessage.DisplayMessage = "Add Scrap Store Location";
                                            resultMessage.Text = "Add Scrap Store Location";
                                            return resultMessage;
                                        }
                                        TblProductItemExistScrapTO.LocationId = tblProductItemExistTO.ScrapStoreLocationId;
                                        TblLocationTO TblLocationTO = _iTblLocationBL.SelectTblLocationTO(TblProductItemExistScrapTO.LocationId);
                                        TblProductItemExistScrapTO.MappedLocationId = Convert.ToInt32(TblLocationTO.MappedTxnId);
                                        //TblProductItemExistScrapTO.RefProdItemId = tblProductItemExistTO.IdProdItem;
                                    }
                                    
                                    List<TblPurchaseItemMasterTO> tblPurchaseItemMasterTOList = new List<TblPurchaseItemMasterTO>();
                                    TblProductItemExistScrapTO.IsScrapItem = true;
                                    resultMessage = InsertTblProductItem(TblProductItemExistScrapTO, tblPurchaseItemMasterTOList, conn, tran);
                                    if (resultMessage.MessageType != ResultMessageE.Information)
                                    {
                                        tran.Rollback();
                                        if (Startup.CompanyObject.InTransaction)
                                            Startup.CompanyObject.EndTransaction(BoWfTransOpt.wf_RollBack);
                                        resultMessage.DefaultBehaviour();
                                        resultMessage.DisplayMessage = "Error in insert InsertTblProductItem";
                                        return resultMessage;
                                    }
                                    tblProductItemExistTO.IsHaveScrapProdItem = true;
                                    int result = _iTblProductItemDAO.UpdateisHaveScrapProdItem(tblProductItemExistTO, conn, tran);
                                    if (result < 1)
                                    {
                                        tran.Rollback();
                                        if (Startup.CompanyObject.InTransaction)
                                            Startup.CompanyObject.EndTransaction(BoWfTransOpt.wf_RollBack);
                                        resultMessage.DefaultBehaviour();
                                        resultMessage.DisplayMessage = "Error in Update InsertTblProductItem";
                                        return resultMessage;
                                    }
                                }
                                TblProductItemScrapTO = TblProductItemExistScrapTO;
                            }
                            if (TblProductItemScrapTO.BaseProdItemId > 0)
                            {
                                resultMessage = AddItemValuation(TblProductItemScrapTO);
                                if (resultMessage.MessageType != ResultMessageE.Information)
                                {
                                    tran.Rollback();
                                    if (Startup.CompanyObject.InTransaction)
                                        Startup.CompanyObject.EndTransaction(BoWfTransOpt.wf_RollBack);
                                    resultMessage.DefaultBehaviour();
                                    resultMessage.DisplayMessage = "Error in insert AddItemValuation";
                                    resultMessage.Text = "Error in insert AddItemValuation";
                                    return resultMessage;
                                }
                            }
                            resultMessage.Tag = TblProductItemScrapTO;
                            tran.Commit();
                            if (Startup.CompanyObject.InTransaction)
                                Startup.CompanyObject.EndTransaction(BoWfTransOpt.wf_Commit);
                            return resultMessage;
                        }
                    }
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    if (Startup.CompanyObject.InTransaction)
                        Startup.CompanyObject.EndTransaction(BoWfTransOpt.wf_RollBack);
                    resultMessage.DefaultExceptionBehaviour(ex, "CopyPastItem");
                    return resultMessage;
                }
                finally
                {
                    conn.Close();
                    if (Startup.CompanyObject.InTransaction)
                        Startup.CompanyObject.EndTransaction(BoWfTransOpt.wf_RollBack);
                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                if (Startup.CompanyObject.InTransaction)
                    Startup.CompanyObject.EndTransaction(BoWfTransOpt.wf_RollBack);
                resultMessage.DefaultExceptionBehaviour(ex, "CopyPastItem");
                return resultMessage;
            }
        }


        public ResultMessage SetMissingFieldsInSap()
        {
            ResultMessage resultMessage = new ResultMessage();
            int count = 0;
            try
            {

                if (Startup.CompanyObject == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "SAP CompanyObject Found NULL. Connectivity Error. " + Startup.SapConnectivityErrorCode;
                    resultMessage.DisplayMessage = "Error while creating item in SAP with Exception";
                    return resultMessage;
                }

                List<DropDownTO> listSAP = _iTblProductItemDAO.GetMissingFieldsItemFromSap();
                List<DropDownTO> listItemInSW = _iTblProductItemDAO.GetMissingFieldsItemHavingConversionFactorOtherThan1();
                if (listSAP != null && listSAP.Count > 0)
                {
                    for (int i = 0; i < listSAP.Count; i++)
                    {
                        DropDownTO toTemp = listItemInSW.Where(w => w.Value == listSAP[i].Value).FirstOrDefault();
                        if (toTemp != null) { }
                        else
                        {
                            SAPbobsCOM.Items oitm;
                            oitm = Startup.CompanyObject.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems);
                            oitm.GetByKey(listSAP[i].Value.ToString());
                            oitm.GSTTaxCategory = SAPbobsCOM.GSTTaxCategoryEnum.gtc_Regular;
                            oitm.UoMGroupEntry = 1;
                            int result = oitm.Update();
                            if (result == 0)
                            {
                                count ++;           
                            }
                            else
                            {
                                resultMessage.DefaultBehaviour();
                                string sapErrorMsg = Startup.CompanyObject.GetLastErrorDescription();
                                resultMessage.Text = sapErrorMsg;
                                resultMessage.Result = i;
                                resultMessage.DisplayMessage = "Error while updating item in SAP";
                            }
                        }
                    }
                }
                resultMessage.DefaultSuccessBehaviour();
                resultMessage.Result = count;
                return resultMessage;
            }
            catch (Exception ex) {
                resultMessage.DisplayMessage = ex.Message;
                resultMessage.Result = count;
                return resultMessage;
            }
        }

        public ResultMessage AddItemValuation(TblProductItemTO TblProductItemScrapTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                SAPbobsCOM.MaterialRevaluation materialRevaluation;
                materialRevaluation = (SAPbobsCOM.MaterialRevaluation)Startup.CompanyObject.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMaterialRevaluation);
                materialRevaluation.DocDate = DateTime.Now;
                //materialRevaluation.Warehouse = "100000";
                materialRevaluation.RevalType = "P";
                materialRevaluation.Lines.SetCurrentLine(0);
                materialRevaluation.Lines.ItemCode = TblProductItemScrapTO.IdProdItem.ToString();
                materialRevaluation.Lines.WarehouseCode = TblProductItemScrapTO.MappedLocationId.ToString();//"100000";
                materialRevaluation.Lines.Price = 1;
               ////materialRevaluation.Lines.RevaluationDecrementAccount = "130000";
                ////materialRevaluation.Lines.RevaluationIncrementAccount = "130000";
                //TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_Scrap_Revaluation_Decrement_Account);
                //if (tblConfigParamsTO != null)
                //materialRevaluation.Lines.RevaluationDecrementAccount = tblConfigParamsTO.ConfigParamVal;
                //tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_Scrap_Revaluation_Increment_Account);
                //if (tblConfigParamsTO != null)
                //    materialRevaluation.Lines.RevaluationIncrementAccount = tblConfigParamsTO.ConfigParamVal;
                materialRevaluation.Lines.Add();
                int result = materialRevaluation.Add();
                if (result != 0)
                {
                    string errorMsg = Startup.CompanyObject.GetLastErrorDescription();
                    resultMessage.DefaultBehaviour(errorMsg);
                }
                else
                {
                    resultMessage.DefaultSuccessBehaviour("Stock Transfer Completed Successfully");
                    string TxnId = Startup.CompanyObject.GetNewObjectKey();
                    resultMessage.Tag = TxnId;

                }
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "AddItemValuation");
                return resultMessage;
            }
        }
        public void GetProdClassificationHierarchy(List<TblProdClassificationTO> tblProdClassificationTOList, TblProdClassificationTO tblProdClassificationTO)
        {
            TblProdClassificationTO TblProdClassificationTOTemp = _iTblProdClassificationBL.SelectTblProdClassificationTO(tblProdClassificationTO.ParentProdClassId);
            if (tblProdClassificationTO.ParentProdClassId > 0)
                GetProdClassificationHierarchy(tblProdClassificationTOList, TblProdClassificationTOTemp);
            tblProdClassificationTOList.Add(tblProdClassificationTO);
        }
        //Reshma
        void GetProdClassificationHierarchy(List<TblProdClassificationTO> tblProdClassificationTOList, TblProdClassificationTO tblProdClassificationTO, SqlConnection conn, SqlTransaction tran)
        {
            TblProdClassificationTO TblProdClassificationTOTemp = _iTblProdClassificationBL.SelectTblProdClassificationTOV2(tblProdClassificationTO.ParentProdClassId, conn, tran);
            if (tblProdClassificationTO.ParentProdClassId > 0)
                GetProdClassificationHierarchy(tblProdClassificationTOList, TblProdClassificationTOTemp,conn,tran);
            tblProdClassificationTOList.Add(tblProdClassificationTO);
        }
        public void GetProductionItemHierarchy(List<TblProductItemTO> tblProductItemTOList, TblProductItemTO tblProductBaseItemTO)
        {
            if(tblProductBaseItemTO.BaseProdItemId>0)
            {
                int baseProductItemId = Convert.ToInt32(tblProductBaseItemTO.BaseProdItemId);
                TblProductItemTO TblProductItemTOTemp = SelectTblProductItemTO(baseProductItemId);
                GetProductionItemHierarchy(tblProductItemTOList, TblProductItemTOTemp);
            }
            tblProductItemTOList.Add(tblProductBaseItemTO);
        }

        public TblProductItemTO CreateDummyItem(TblProductItemTO tblProductItemTO)
        {
            TblProductItemTO tblProductItemTOtemp = new TblProductItemTO();
            TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParams(Constants.PROJECT_SUBGROUP_ID);
            if (tblConfigParamsTO == null || String.IsNullOrEmpty(tblConfigParamsTO.ConfigParamVal))
            {
                return null;
            }
            TblConfigParamsTO tblConfigParamsTOItem = new TblConfigParamsTO();
            TblConfigParamsTO tblConfigParamsTOGstCodeId = new TblConfigParamsTO();
            if (tblProductItemTO.EnquiryTypeId ==(int)Constants.dimEnquiryTypeEnum.Project)
            {
                tblConfigParamsTOItem = _iTblConfigParamsDAO.SelectTblConfigParams(Constants.CP_JSON_FOR_ORDER_ITEM_CREATION_PROJECT_WISE);
            }
            else if (tblProductItemTO.EnquiryTypeId == (int)Constants.dimEnquiryTypeEnum.Services)
            {
                tblConfigParamsTOItem = _iTblConfigParamsDAO.SelectTblConfigParams(Constants.CP_JSON_FOR_ORDER_ITEM_CREATION_SERVICE_WISE);
            }
            else if (tblProductItemTO.EnquiryTypeId == (int)Constants.dimEnquiryTypeEnum.Spares)
            {
                tblConfigParamsTOItem = _iTblConfigParamsDAO.SelectTblConfigParams(Constants.CP_JSON_FOR_ORDER_ITEM_CREATION_SPARE_WISE);
            }

            if (tblConfigParamsTOItem == null || String.IsNullOrEmpty(tblConfigParamsTOItem.ConfigParamVal))
            {
                tblProductItemTO.ProdClassId = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);

                TblProdClassificationTO temp = _iTblProdClassificationBL.SelectTblProdClassificationTO(tblProductItemTO.ProdClassId);
                TblProdClassificationTO temp1 = null;
                TblProdClassificationTO temp2 = null;
                if (temp != null)
                {
                    temp1 = _iTblProdClassificationBL.SelectTblProdClassificationTO(temp.ParentProdClassId);
                    if (temp1 != null)
                    {
                        temp2 = _iTblProdClassificationBL.SelectTblProdClassificationTO(temp1.ParentProdClassId);
                        if (temp2 != null)
                        {

                        }
                    }
                }

                if (temp2 == null)
                {
                    return null;
                }

               
                

                tblProductItemTO.HSNCode = 85044010;
                tblProductItemTO.GstCodeId = 39441;
                tblProductItemTO.SapHSNCode = 10752;

                // Add By Samadhan 9 Feb 2023
                tblConfigParamsTOGstCodeId = _iTblConfigParamsDAO.SelectTblConfigParams(Constants.DEFAULT_GST_CODEID);
                if (tblConfigParamsTOGstCodeId != null && !String.IsNullOrEmpty(tblConfigParamsTOGstCodeId.ConfigParamVal))
                {
                    List<TblGstCodeDtlsTO> tblGstCodeDtlsTO = _iTblGstCodeDtlsBL.SelectAllTblGstCodeDtlsList(Convert.ToInt32(tblConfigParamsTOGstCodeId.ConfigParamVal));

                   if (tblGstCodeDtlsTO != null && tblGstCodeDtlsTO.Count > 0)
                    {                     
                        tblProductItemTO.GstCodeId = tblGstCodeDtlsTO[0].IdGstCode;
                        tblProductItemTO.SapHSNCode = Convert.ToDouble(tblGstCodeDtlsTO[0].SapHsnCode);
                    }
                }

                tblProductItemTO.ItemDesc = tblProductItemTO.ItemName;
                tblProductItemTO.IsAddItemfrmGrpSubGrp = 1;
                tblProductItemTO.SapItemGroupId = temp2.MappedTxnId;
                tblProductItemTO.LocationId = 4;
                tblProductItemTO.MappedLocationId = 1001;
                tblProductItemTO.CodeTypeId = 1;

                //oitm.PurchaseItem = Constants.GetYesNoEnum(tblProductItemTO.IsPurchaseItem);
                tblProductItemTO.IsInventoryItem = 1;
                tblProductItemTO.IsSalesItem = 1;

                tblProductItemTO.TaxCategoryId = 0;
                tblProductItemTO.IssueId = 1;
                tblProductItemTO.IsPurchaseItem = 1;
                tblProductItemTO.ProcurementId = 0;

                tblProductItemTO.IsProperSAPItem = 1;
                if (tblProductItemTO.WeightMeasureUnitId >0)
                {
                    DimUnitMeasuresTO dimUnitMeasuresTO = _iDimUnitMeasuresDAO.SelectDimUnitMeasures(tblProductItemTO.WeightMeasureUnitId);
                    tblProductItemTO.WeightMeasureUnitId = dimUnitMeasuresTO.IdWeightMeasurUnit;
                    tblProductItemTO.MappedWeightMeasureUnitId = dimUnitMeasuresTO.MappedTxnId;

                    tblProductItemTO.ConversionUnitOfMeasureId = dimUnitMeasuresTO.IdWeightMeasurUnit;
                    tblProductItemTO.MappedConversionUnitOfMeasure = dimUnitMeasuresTO.MappedTxnId;
                    tblProductItemTO.IsNonListed = true;
                }
                else
                {
                    tblProductItemTO.WeightMeasureUnitId = 23;
                    tblProductItemTO.MappedWeightMeasureUnitId = 14;

                    tblProductItemTO.ConversionUnitOfMeasureId = 23;
                    tblProductItemTO.MappedConversionUnitOfMeasure = 14;
                }
                tblProductItemTO.ConversionFactor = 1;

                tblProductItemTO.SalesUOMId = 23;
                tblProductItemTO.MappedSalesUOMId = 14;

                tblProductItemTO.UOMGroupId = 47;
                tblProductItemTO.MappedUOMGroupId = 1.ToString();

                tblProductItemTO.MaterialTypeId = 3;
                tblProductItemTO.IsGSTApplicable = 1;
            }
            else
            {
                JObject data = JObject.Parse(tblConfigParamsTOItem.ConfigParamVal);
                tblProductItemTOtemp = JsonConvert.DeserializeObject<TblProductItemTO>(data["productItemTO"].ToString());
                tblProductItemTOtemp.ItemName = tblProductItemTO.ItemName;
                tblProductItemTOtemp.ItemDesc = tblProductItemTO.ItemName;
                tblProductItemTO = tblProductItemTOtemp.DeepCopy();
                tblProductItemTO.CreatedOn= _iCommon.ServerDateTime;
                tblProductItemTO.CreatedBy = Convert.ToInt32(1);
                tblProductItemTO.IsActive = 1;
            }

            return tblProductItemTO;
        }


        public ResultMessage InsertTblProductItemByName(TblProductItemTO tblProductItemTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            tblProductItemTO = CreateDummyItem(tblProductItemTO);

            if (tblProductItemTO == null)
            {
                throw new Exception("Category Not found");
            }

            resultMessage= InsertTblProductItem(tblProductItemTO, new List<TblPurchaseItemMasterTO>(), new List<WareHouseWiseItemDtlsTO>(),new List<StoresLocationTO>());
            resultMessage.Tag = tblProductItemTO;
            return resultMessage;
        }


        public ResultMessage InsertTblProductItem(TblProductItemTO tblProductItemTO, List<TblPurchaseItemMasterTO> tblPurchaseItemMasterTOList, List<WareHouseWiseItemDtlsTO> wareHouseWiseItemDtlsTOList, List<StoresLocationTO> StoresLocationTOList, List<TblProdItemMakeBrandExtTO> tblProdItemMakeBrandExtTOList=null)
        {
            ResultMessage resultMessage = new ResultMessage();
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            SqlConnection sapConn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.SAP_CONNECTION_STRING));
            SqlTransaction saptTran = null;
            int result = 0;
            int res = 0;
            try
            {
                
                conn.Open();
                tran = conn.BeginTransaction();
                #region Check Make Item Alredy Added Or Not Of Same itemMakeId & itemBrandId
                if (tblProductItemTO.BaseProdItemId != 0)
                {
                    List<TblProductItemTO> TblProductItemTOList = _iTblProductItemDAO.checkMakeItemAlreadyExists(Convert.ToInt32(tblProductItemTO.BaseProdItemId), tblProductItemTO.ItemMakeId, tblProductItemTO.ItemBrandId, tblProductItemTO.IdProdItem, conn, tran);
                    if (TblProductItemTOList != null && TblProductItemTOList.Count > 0)
                    {
                        resultMessage.DefaultBehaviour();
                        resultMessage.DefaultBehaviour("Make Item is already added with Make Item Id " + TblProductItemTOList[0].IdProdItem);
                        resultMessage.DisplayMessage = "Make Item is already added with Make Item Id " + TblProductItemTOList[0].IdProdItem;
                        resultMessage.Result = 2;
                        return resultMessage;
                    }
                    
                }
                #endregion
                if(tblProductItemTO.BaseProdItemId != 0 && tblProductItemTO.IsFixedAsset ==true)
                {
                    if(tblProductItemTO.AssetClassId ==0)
                    {
                        resultMessage.DefaultBehaviour();
                        resultMessage.DefaultBehaviour("Please Select Proper Asset Class");
                        resultMessage.DisplayMessage = "Please Select Proper Asset Class";
                        
                        return resultMessage;
                    }
                    else if(tblProductItemTO.AssetStoreLocationId ==0 || tblProductItemTO.AssetStoreLocationId ==83)
                    {
                        resultMessage.DefaultBehaviour();
                        resultMessage.DefaultBehaviour("Please Select Proper Asset Location");
                        resultMessage.DisplayMessage = "Please Select Proper Asset Location";
                       
                        return resultMessage;
                    }
                }
                #region Check Base Item Alredy Added Or Not 
                if (tblProductItemTO.BaseProdItemId == 0)
                {
                    List<TblProductItemTO> TblProductItemTOList = _iTblProductItemDAO.checkBaseItemAlreadyExists(tblProductItemTO.IdProdItem, tblProductItemTO.ProdClassId, tblProductItemTO.ItemName, conn, tran);
                    if (TblProductItemTOList != null && TblProductItemTOList.Count > 0)
                    {
                        resultMessage.DefaultBehaviour();
                        resultMessage.DefaultBehaviour("Base Item is already added with Base Item Id " + TblProductItemTOList[0].IdProdItem);
                        resultMessage.DisplayMessage = "Base Item is already added with Base Item Id " + TblProductItemTOList[0].IdProdItem;
                        resultMessage.Result = 2;
                        return resultMessage;
                    }
                }
                #endregion
                Boolean SAPServiceEnable = false;
                TblConfigParamsTO tblConfigParamsTOSAPService = _iTblConfigParamsDAO.SelectTblConfigParams(Constants.SAPB1_SERVICES_ENABLE);
                if (tblConfigParamsTOSAPService != null)
                {
                    if (Convert.ToInt32(tblConfigParamsTOSAPService.ConfigParamVal) == 1)
                    {
                        SAPServiceEnable = true;
                        sapConn.Open();
                        saptTran = sapConn.BeginTransaction();
                    }
                }
                if (tblProductItemTO.IsBaseItemForRate > 0)
                {
                    result = _iTblProductItemDAO.updatePreviousBase(conn, tran);
                    if (result == -1)
                    {
                        tran.Rollback();
                        saptTran.Rollback();
                        resultMessage.DefaultBehaviour();
                        resultMessage.DisplayMessage = "Error while updatePreviousBase";
                        return resultMessage;
                    }
                }
                //resultMessage = AddUOMGroupV2(tblProductItemTO, SAPServiceEnable, conn, sapConn, tran, saptTran);

                //Deepali Added[26-04-2021] task no 1030
                TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.IS_SHOW_UOM_AND_COUM_DDL);
                int isShowConvUOM = 0;
                if (tblConfigParamsTO != null)
                {
                    isShowConvUOM = Convert.ToInt16(tblConfigParamsTO.ConfigParamVal);
                }
                if (isShowConvUOM == 1)
                {
                    DimUomGroupTO dimUomGroupTO = new DimUomGroupTO();

                    dimUomGroupTO = getUOMGroupIdFromUOMAndConversionUOM(tblProductItemTO.WeightMeasureUnitId, tblProductItemTO.ConversionUnitOfMeasure, tblProductItemTO.ConversionFactor,conn,tran);
                    if (dimUomGroupTO != null)
                    {
                        tblProductItemTO.MappedUOMGroupId = dimUomGroupTO.MappedUomGroupId;
                        tblProductItemTO.UOMGroupId = dimUomGroupTO.IdUomGroup;
                    }
                    else
                    {
                        dimUomGroupTO = getUOMGroupIdFromUOMAndConversionUOM(tblProductItemTO.WeightMeasureUnitId,0,0,conn,tran);
                        if (dimUomGroupTO != null)
                        {
                            dimUomGroupTO.ConversionUnitOfMeasure = tblProductItemTO.ConversionUnitOfMeasure;
                            dimUomGroupTO.ConversionFactor = tblProductItemTO.ConversionFactor;
                        }
                        else
                        {
                            dimUomGroupTO = new DimUomGroupTO();
                            dimUomGroupTO.BaseUomId = tblProductItemTO.WeightMeasureUnitId;
                            dimUomGroupTO.ConversionUnitOfMeasure = tblProductItemTO.ConversionUnitOfMeasure;
                            dimUomGroupTO.ConversionFactor = tblProductItemTO.ConversionFactor;
                            List<DropDownTO> UnitMeasureList = _iDimUnitMeasuresDAO.SelectAllUnitMeasuresForDropDown();
                            if (UnitMeasureList != null && UnitMeasureList.Count > 0)
                            {
                                DropDownTO dropDownTOUom = UnitMeasureList.Where(w => w.Value == tblProductItemTO.WeightMeasureUnitId).FirstOrDefault();
                                DropDownTO dropDownTOCUom = UnitMeasureList.Where(w => w.Value == tblProductItemTO.ConversionUnitOfMeasure).FirstOrDefault();

                                if (dropDownTOUom != null && dropDownTOCUom != null)
                                {
                                    dimUomGroupTO.UomGroupName = dropDownTOUom.Text + "_" + dropDownTOCUom.Text + "_" + tblProductItemTO.ConversionFactor;
                                    dimUomGroupTO.UomGroupCode = dropDownTOUom.Text + "_" + dropDownTOCUom.Text + "_" + tblProductItemTO.ConversionFactor;
                                }
                            }
                            dimUomGroupTO.CreatedBy = tblProductItemTO.CreatedBy;
                            dimUomGroupTO.CreatedOn = tblProductItemTO.CreatedOn;
                        }

                        resultMessage = AddUOMGroupV2FromMaster(dimUomGroupTO, SAPServiceEnable, conn, sapConn, tran, saptTran);
                        if (resultMessage.MessageType != ResultMessageE.Information)
                        {
                            tran.Rollback();
                            if (SAPServiceEnable)
                                saptTran.Rollback();
                            return resultMessage;
                        }
                        else
                        {
                            if (SAPServiceEnable)
                            {
                                saptTran.Commit();
                                sapConn.Close();
                            }
                        }
                    }
                   
                    dimUomGroupTO = getUOMGroupIdFromUOMAndConversionUOM(tblProductItemTO.WeightMeasureUnitId, tblProductItemTO.ConversionUnitOfMeasure, tblProductItemTO.ConversionFactor, conn, tran);
                    if (dimUomGroupTO != null)
                    {
                        tblProductItemTO.MappedUOMGroupId = dimUomGroupTO.MappedUomGroupId;
                        tblProductItemTO.UOMGroupId = dimUomGroupTO.IdUomGroup;
                    }
                }

                if (tblProductItemTO.IdProdItem == 0)
                {
                    //AmolG[2020-Dec-18] If the values not comes from UI then do sumation here
                    if (tblProductItemTO.IsManageInventory)
                    {
                        tblProductItemTO.InventMinimum = 0;
                        //tblProductItemTO.MinOrderQty = 0;
                        if (wareHouseWiseItemDtlsTOList != null && wareHouseWiseItemDtlsTOList.Count > 0)
                        {
                            for (int iCnt = 0; iCnt < wareHouseWiseItemDtlsTOList.Count; iCnt++)
                            {
                                if (!String.IsNullOrEmpty(wareHouseWiseItemDtlsTOList[iCnt].WhsCode))
                                {
                                    tblProductItemTO.InventMinimum += wareHouseWiseItemDtlsTOList[iCnt].MinInventory;
                                    //tblProductItemTO.MinOrderQty += wareHouseWiseItemDtlsTOList[iCnt].MinOrder;
                                }
                            }
                        }
                    }
                    //Added by minal 12 march 2021
                    if (tblProductItemTO.IsAddItemfrmGrpSubGrp == 1)
                    {

                        TblProductItemTO baseItemTo = tblProductItemTO.DeepCopy();

                        resultMessage = AddItemfrmGrpSubGrp(baseItemTo, conn, tran);
                        if (resultMessage.MessageType != ResultMessageE.Information)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour(resultMessage.Text);
                            return resultMessage;
                        }

                        tblProductItemTO.BaseProdItemId = baseItemTo.IdProdItem;

                    }
                    if(true)
                    //else
                    {
                        if(tblProductItemTO.BaseProdItemId >0 && tblProductItemTO.ConversionUnitOfMeasureId >0 && tblProductItemTO.WeightMeasureUnitId > 0)
                        {
                            tblProductItemTO.ConversionUnitOfMeasure = tblProductItemTO.ConversionUnitOfMeasureId;
                        }
                        if (tblProductItemTO.CodeTypeId == 2)
                            tblProductItemTO.IsInventoryItem = 0;
                        res = _iTblProductItemDAO.InsertTblProductItem(tblProductItemTO, conn, tran);
                        if (res != 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour();
                            resultMessage.DisplayMessage = "Error while insertTblProductItem";
                            return resultMessage;
                        }

                        //if (tblProductItemTO.IdProdItem > 0)
                        //{
                        //    resultMessage = AddProdItemMakeBrand(tblProductItemTO, conn, tran);
                        //    if (resultMessage.MessageType != ResultMessageE.Information)
                        //    {
                        //        tran.Rollback();
                        //        resultMessage.DefaultBehaviour(resultMessage.Text);
                        //        return resultMessage;
                        //    }
                        //}
                    }
                    if (tblPurchaseItemMasterTOList.Count > 0)
                    {
                        for (int i = 0; i < tblPurchaseItemMasterTOList.Count; i++)
                        {
                            tblPurchaseItemMasterTOList[i].ProdItemId = tblProductItemTO.IdProdItem;
                            tblPurchaseItemMasterTOList[i].CreatedBy = tblProductItemTO.CreatedBy;
                            tblPurchaseItemMasterTOList[i].CreatedOn = tblProductItemTO.CreatedOn;
                            tblPurchaseItemMasterTOList[i].IsActive = tblProductItemTO.IsActive;
                            result = _iTblProductItemDAO.InsertTblPurchaseItemMaster(tblPurchaseItemMasterTOList[i], conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour();
                                resultMessage.DisplayMessage = "Error while InsertTblPurchaseItemMaster";
                                return resultMessage;
                            }
                        }
                    }

                    // Samadhan Added 11 May 2022

                    if (StoresLocationTOList != null && StoresLocationTOList.Count > 0)
                    {
                        for (int i = 0; i < StoresLocationTOList.Count; i++)
                        {
                            StoresLocationTOList[i].ProdItemId = tblProductItemTO.IdProdItem;
                            StoresLocationTOList[i].IsActive = 1;
                            result = _iTblProductItemDAO.InsertTblItemLinkedStoreLoc(StoresLocationTOList[i], conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour();
                                resultMessage.DisplayMessage = "Error while InsertTblItemLinkedStoreLoc";
                                return resultMessage;
                            }
                        }
                    }
                    //

                    #region Priyanka [26-06-2019] : Added to link the product under GST taxation code.
                    if (tblProductItemTO.BaseProdItemId > 0 && tblProductItemTO.GstCodeId > 0)
                    {
                        List<TblProdGstCodeDtlsTO> tblProdGstCodeDtlsTOList = new List<TblProdGstCodeDtlsTO>();
                        TblProdGstCodeDtlsTO tblProdGstCodeDtlsTO = new TblProdGstCodeDtlsTO();
                        tblProdGstCodeDtlsTO.ProdItemId = tblProductItemTO.IdProdItem;
                        tblProdGstCodeDtlsTO.GstCodeId = tblProductItemTO.GstCodeId;
                        tblProdGstCodeDtlsTO.IsActive = 1;
                        tblProdGstCodeDtlsTOList.Add(tblProdGstCodeDtlsTO);

                        resultMessage = _iTblProdGstCodeDtlsBL.UpdateProductGstCodeAgainstNewItem(tblProdGstCodeDtlsTOList, tblProductItemTO, tblProductItemTO.CreatedBy, conn, tran);
                        if (resultMessage.Result != 1)
                        {

                            tran.Rollback();
                            resultMessage.DefaultBehaviour();
                            return resultMessage;
                        }
                    }
                    #endregion


                    if (tblProdItemMakeBrandExtTOList != null && tblProdItemMakeBrandExtTOList.Count > 0)
                    {
                        for (int i = 0; i < tblProdItemMakeBrandExtTOList.Count; i++)
                        {
                            tblProdItemMakeBrandExtTOList[i].ProdItemId = tblProductItemTO.IdProdItem;
                            tblProdItemMakeBrandExtTOList[i].CreatedBy = tblProductItemTO.CreatedBy;
                            tblProdItemMakeBrandExtTOList[i].CreatedOn = tblProductItemTO.CreatedOn;
                            //tblProdItemMakeBrandExtTOList[i].IsActive = tblProductItemTO.IsActive;
                            result = _iTblProdItemMakeBrandExtDAO.InsertTblProdItemMakeBrandExt(tblProdItemMakeBrandExtTOList[i], conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour();
                                resultMessage.DisplayMessage = "Error while tblProdItemMakeBrandExtTOList";
                                return resultMessage;
                            }
                        }
                    }
                    if (SAPServiceEnable == true && tblProductItemTO.BaseProdItemId > 0)
                    {
                        if (!tblProductItemTO.IsNonListed)//Reshma For Non Listed
                        {
                            ResultMessage sapResult = SaveItemInSAP(tblProductItemTO, tblPurchaseItemMasterTOList, wareHouseWiseItemDtlsTOList);
                            if (sapResult.Result != 1)
                            {
                                tran.Rollback();
                                return sapResult;
                            }
                        }
                    }


                    // Add By samadhan 28 Nov 2022
                    if (tblProductItemTO.IdProcess > 0)
                    {
                        List<DimProcessTO> TblProcessNameTOList = _iTblProductItemDAO.GetProcessName(tblProductItemTO.IdProcess, conn, tran);

                        if (TblProcessNameTOList != null && TblProcessNameTOList.Count > 0)
                        {
                            string ProcessTypeName = TblProcessNameTOList[0].ProcessName;
                            List<dimProcessType> TblProcessTypeTOList = _iTblProductItemDAO.checkProcessTpyeAlreadyExists(tblProductItemTO.IdProdItem, conn, tran);
                            if (TblProcessTypeTOList != null && TblProcessTypeTOList.Count > 0)
                            {
                                result = _iTblProductItemDAO.UpdateTblProcessType(tblProductItemTO.IdProdItem, ProcessTypeName, conn, tran);
                                if (result != 1)
                                {
                                    tran.Rollback();
                                    resultMessage.DefaultBehaviour();
                                    resultMessage.DisplayMessage = "Error while InsertTblProcessType";
                                    return resultMessage;
                                }
                            }
                            else
                            {
                                int ProcessTypeId = 0;

                                List<dimProcessType> TblProcessTypeTOIdList = _iTblProductItemDAO.GetNewProcessTypeId(conn, tran);
                                if (TblProcessTypeTOIdList != null && TblProcessTypeTOIdList.Count > 0)
                                {
                                    ProcessTypeId = TblProcessTypeTOIdList[0].IdProcessType;
                                }
                                result = _iTblProductItemDAO.InsertTblProcessType(ProcessTypeId, tblProductItemTO.IdProdItem, ProcessTypeName, conn, tran);
                                if (result != 1)
                                {
                                    tran.Rollback();
                                    resultMessage.DefaultBehaviour();
                                    resultMessage.DisplayMessage = "Error while InsertTblProcessType";
                                    return resultMessage;
                                }
                            }

                        }



                    }
                }
               
                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "InsertTblProductItem at BL");
                return resultMessage;
            }
            finally
            {
                conn.Close();
                sapConn.Close();
            }
        }

        public DimUomGroupTO getUOMGroupIdFromUOMAndConversionUOM(int weightMeasureUnitId, int conversionUnitOfMeasure, double conversionFactor, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimUomGroupDAO.SelectDimUomGroup(weightMeasureUnitId,  conversionUnitOfMeasure, conversionFactor,conn,tran);          
        }

        //public ResultMessage AddProdItemMakeBrand(TblProductItemTO tblProductItemTO, SqlConnection conn, SqlTransaction tran)
        //{
        //    ResultMessage resultMessage = new ResultMessage();
        //    int res = 0;
        //    try
        //    {
        //        if (tblProductItemTO.ProdItemMakeBrandTOList == null || tblProductItemTO.ProdItemMakeBrandTOList.Count == 0)
        //        {
        //            return null;
        //        }
        //        for (int i = 0; i < tblProductItemTO.ProdItemMakeBrandTOList.Count; i++)
        //        {
        //            TblProdItemMakeBrandTO tblProdItemMakeBrandTO = new TblProdItemMakeBrandTO();
        //            tblProdItemMakeBrandTO.ProdItemId = tblProductItemTO.IdProdItem;
        //            tblProdItemMakeBrandTO.BrandId = tblProductItemTO.ProdItemMakeBrandTOList[i].BrandId;
        //            tblProdItemMakeBrandTO.IsDefaultMake = tblProductItemTO.ProdItemMakeBrandTOList[i].IsDefaultMake;
        //            tblProdItemMakeBrandTO.CreatedBy = tblProductItemTO.CreatedBy;
        //            tblProdItemMakeBrandTO.CreatedOn = tblProductItemTO.CreatedOn;

        //            res = _iITblProdItemMakeBrandBL.InsertTblProdItemMakeBrand(tblProdItemMakeBrandTO,conn,tran);                    

        //            if (res != 1)
        //            {
        //                tran.Rollback();
        //                resultMessage.DefaultBehaviour();
        //                resultMessage.DisplayMessage = "Error while AddProdItemMakeBrand";
        //                return resultMessage;
        //            }
        //        }

        //        resultMessage.DefaultSuccessBehaviour();
        //        return resultMessage;
        //    }
        //    catch (Exception ex)
        //    {
        //        tran.Rollback();
        //        resultMessage.DefaultExceptionBehaviour(ex, "InsertTblProductItem at BL");
        //        return resultMessage;
        //    }
        //    finally
        //    {
        //        //conn.Close();
        //        //sapConn.Close();
        //    }

        //}

        public ResultMessage AddItemfrmGrpSubGrp(TblProductItemTO tblProductItemTO,SqlConnection conn,SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage(); 
            int res = 0;
            try
            {
                TblProdClassificationTO tblProdClassificationTO = new TblProdClassificationTO();
                tblProdClassificationTO.CreatedOn = tblProductItemTO.CreatedOn;
                tblProdClassificationTO.CreatedBy = tblProductItemTO.CreatedBy;
                tblProdClassificationTO.UpdatedOn = tblProductItemTO.UpdatedOn;
                tblProdClassificationTO.UpdatedBy = tblProductItemTO.UpdatedBy;
                tblProdClassificationTO.IsActive = tblProductItemTO.IsActive;
                tblProdClassificationTO.ItemProdCatId = tblProductItemTO.ItemProdCatId;
                tblProdClassificationTO.CodeTypeId = tblProductItemTO.CodeTypeId;
                if (tblProductItemTO.ProdClassType == "C")
                {
                    tblProdClassificationTO.ProdClassDesc = tblProductItemTO.ItemName;
                    tblProdClassificationTO.DisplayName = tblProductItemTO.ItemDesc;
                    tblProdClassificationTO.ParentProdClassId = tblProductItemTO.CategoryID;
                    tblProdClassificationTO.Remark = tblProductItemTO.ItemName;                    
                    
                    tblProdClassificationTO.ProdClassType = "SC";
                    List<TblProdClassificationTO> tblProdClassificationTOList = _iTblProdClassificationBL.checkSubGroupAlreadyExists(tblProdClassificationTO);
                    if (tblProdClassificationTOList != null && tblProdClassificationTOList.Count > 0)
                    {
                        resultMessage.DefaultBehaviour();
                        resultMessage.DefaultBehaviour("Sub Group is already added with Sub Group Id " + tblProdClassificationTOList[0].IdProdClass);
                        resultMessage.DisplayMessage = "Sub Group is already added with Sub Group Id " + tblProdClassificationTOList[0].IdProdClass;
                        resultMessage.Result = 2;
                        return resultMessage;
                    }

                    tblProdClassificationTO.DisplayName = tblProductItemTO.ItemDesc;
                    Int32 finalResult1 = _iTblProdClassificationBL.InsertTblProdClassification(tblProdClassificationTO, conn, tran);
                    if (finalResult1 != 1)
                    {
                        tran.Rollback();
                        //saptTran.Rollback();
                        resultMessage.DefaultBehaviour();
                        resultMessage.DisplayMessage = "Error while InsertTblProdClassification For SC";
                        return resultMessage;
                    }
                    tblProdClassificationTO.ParentProdClassId = tblProdClassificationTO.IdProdClass;
                    tblProdClassificationTO.ProdClassType = "S";
                    tblProdClassificationTO.DisplayName = tblProductItemTO.ItemDesc;
                    Int32 finalResult2 = _iTblProdClassificationBL.InsertTblProdClassification(tblProdClassificationTO, conn, tran);
                    if (finalResult2 != 1)
                    {
                        tran.Rollback();
                        //saptTran.Rollback();
                        resultMessage.DefaultBehaviour();
                        resultMessage.DisplayMessage = "Error while InsertTblProdClassification For SC";
                        return resultMessage;
                    }

                    tblProductItemTO.ProdClassId = tblProdClassificationTO.IdProdClass;
                }
                else if (tblProductItemTO.ProdClassType == "SC")
                {
                    tblProdClassificationTO.Remark = tblProductItemTO.ItemName;
                    tblProdClassificationTO.ProdClassDesc = tblProductItemTO.ItemName;
                    tblProdClassificationTO.ParentProdClassId = tblProductItemTO.SubCategoryID;
                    tblProdClassificationTO.ProdClassType = "S";
                    List<TblProdClassificationTO> tblProdClassificationTOList = _iTblProdClassificationBL.checkSubGroupAlreadyExists(tblProdClassificationTO);
                    if (tblProdClassificationTOList != null && tblProdClassificationTOList.Count > 0)
                    {
                        resultMessage.DefaultBehaviour();
                        resultMessage.DefaultBehaviour("Product is already added with Product Id " + tblProdClassificationTOList[0].IdProdClass);
                        resultMessage.DisplayMessage = "Product is already added with Product Id " + tblProdClassificationTOList[0].IdProdClass;
                        resultMessage.Result = 2;
                        return resultMessage;
                    }
                    tblProdClassificationTO.DisplayName = tblProductItemTO.ItemDesc;
                    Int32 finalResult2 = _iTblProdClassificationBL.InsertTblProdClassification(tblProdClassificationTO, conn, tran);
                    if (finalResult2 != 1)
                    {
                        tran.Rollback();
                        //saptTran.Rollback();
                        resultMessage.DefaultBehaviour();
                        resultMessage.DisplayMessage = "Error while InsertTblProdClassification For S";
                        return resultMessage;
                    }
                    tblProductItemTO.ProdClassId = tblProdClassificationTO.IdProdClass;
                }
                if (tblProductItemTO.CodeTypeId == 2)
                    tblProductItemTO.IsInventoryItem = 0;
                res = _iTblProductItemDAO.InsertTblProductItem(tblProductItemTO, conn, tran);
                if (res != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour();
                    resultMessage.DisplayMessage = "Error while insertTblProductItem in add sc or s";
                    return resultMessage;
                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "InsertTblProductItem at BL");
                return resultMessage;
            }
            finally
            {
                //conn.Close();
                //sapConn.Close();
            }

        }
        //chetan[2020-20-01] added connection Tran method
        public ResultMessage InsertTblProductItem(TblProductItemTO tblProductItemTO, List<TblPurchaseItemMasterTO> tblPurchaseItemMasterTOList, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            SqlConnection sapConn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.SAP_CONNECTION_STRING));
            SqlTransaction saptTran = null;
            int result = 0;
            int res = 0;
            try
            {
                #region Check Make Item Alredy Added Or Not Of Same itemMakeId & itemBrandId
                if (tblProductItemTO.BaseProdItemId != 0)
                {
                    List<TblProductItemTO> TblProductItemTOList = _iTblProductItemDAO.checkMakeItemAlreadyExists(Convert.ToInt32(tblProductItemTO.BaseProdItemId), tblProductItemTO.ItemMakeId, tblProductItemTO.ItemBrandId, tblProductItemTO.IdProdItem, conn, tran);
                    if (TblProductItemTOList != null && TblProductItemTOList.Count > 0)
                    {
                        resultMessage.DefaultBehaviour();
                        resultMessage.DefaultBehaviour("Make Item is already added with Make Item Id " + TblProductItemTOList[0].IdProdItem);
                        resultMessage.DisplayMessage = "Make Item is already added with Make Item Id " + TblProductItemTOList[0].IdProdItem;
                        resultMessage.Result = 2;
                        return resultMessage;
                    }
                }
                #endregion

                #region Check Base Item Alredy Added Or Not 
                if (tblProductItemTO.BaseProdItemId == 0)
                {
                    List<TblProductItemTO> TblProductItemTOList = _iTblProductItemDAO.checkBaseItemAlreadyExists(tblProductItemTO.IdProdItem, tblProductItemTO.ProdClassId, tblProductItemTO.ItemName, conn, tran);
                    if (TblProductItemTOList != null && TblProductItemTOList.Count > 0)
                    {
                        resultMessage.DefaultBehaviour();
                        resultMessage.DefaultBehaviour("Base Item is already added with Base Item Id " + TblProductItemTOList[0].IdProdItem);
                        resultMessage.DisplayMessage = "Base Item is already added with Base Item Id " + TblProductItemTOList[0].IdProdItem;
                        resultMessage.Result = 2;
                        return resultMessage;
                    }
                }
                #endregion
                Boolean SAPServiceEnable = false;
                TblConfigParamsTO tblConfigParamsTOSAPService = _iTblConfigParamsDAO.SelectTblConfigParams(Constants.SAPB1_SERVICES_ENABLE);
                if (tblConfigParamsTOSAPService != null)
                {
                    if (Convert.ToInt32(tblConfigParamsTOSAPService.ConfigParamVal) == 1)
                    {
                        SAPServiceEnable = true;
                        sapConn.Open();
                        saptTran = sapConn.BeginTransaction();
                    }
                }
                if (tblProductItemTO.IsBaseItemForRate > 0)
                {
                    result = _iTblProductItemDAO.updatePreviousBase(conn, tran);
                    if (result == -1)
                    {
                        saptTran.Rollback();
                        resultMessage.DefaultBehaviour();
                        resultMessage.DisplayMessage = "Error while updatePreviousBase";
                        return resultMessage;
                    }
                }
                //resultMessage = AddUOMGroupV2(tblProductItemTO, SAPServiceEnable, conn, sapConn, tran, saptTran);
                //if (resultMessage.MessageType != ResultMessageE.Information)
                //{
                //    if (SAPServiceEnable)
                //        saptTran.Rollback();
                //    return resultMessage;
                //}
                //else
                //{
                //    if (SAPServiceEnable)
                //    {
                //        saptTran.Commit();
                //        sapConn.Close();
                //    }
                //}
                if (tblProductItemTO.CodeTypeId == 2)
                    tblProductItemTO.IsInventoryItem = 0;
                if (tblProductItemTO.IdProdItem == 0)
                {
                    res = _iTblProductItemDAO.InsertTblProductItem(tblProductItemTO, conn, tran);

                    if (res != 1)
                    {
                        resultMessage.DefaultBehaviour();
                        resultMessage.DisplayMessage = "Error while insertTblProductItem";
                        return resultMessage;
                    }


                    if (tblPurchaseItemMasterTOList.Count > 0)
                    {
                        for (int i = 0; i < tblPurchaseItemMasterTOList.Count; i++)
                        {
                            tblPurchaseItemMasterTOList[i].ProdItemId = tblProductItemTO.IdProdItem;
                            tblPurchaseItemMasterTOList[i].CreatedBy = tblProductItemTO.CreatedBy;
                            tblPurchaseItemMasterTOList[i].CreatedOn = tblProductItemTO.CreatedOn;
                            tblPurchaseItemMasterTOList[i].IsActive = tblProductItemTO.IsActive;
                            result = _iTblProductItemDAO.InsertTblPurchaseItemMaster(tblPurchaseItemMasterTOList[i], conn, tran);
                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour();
                                resultMessage.DisplayMessage = "Error while InsertTblPurchaseItemMaster";
                                return resultMessage;
                            }
                        }
                    }

                    #region Priyanka [26-06-2019] : Added to link the product under GST taxation code.
                    if (tblProductItemTO.BaseProdItemId > 0 && tblProductItemTO.GstCodeId > 0)
                    {
                        List<TblProdGstCodeDtlsTO> tblProdGstCodeDtlsTOList = new List<TblProdGstCodeDtlsTO>();
                        TblProdGstCodeDtlsTO tblProdGstCodeDtlsTO = new TblProdGstCodeDtlsTO();
                        tblProdGstCodeDtlsTO.ProdItemId = tblProductItemTO.IdProdItem;
                        tblProdGstCodeDtlsTO.GstCodeId = tblProductItemTO.GstCodeId;
                        tblProdGstCodeDtlsTO.IsActive = 1;
                        tblProdGstCodeDtlsTOList.Add(tblProdGstCodeDtlsTO);

                        resultMessage = _iTblProdGstCodeDtlsBL.UpdateProductGstCodeAgainstNewItem(tblProdGstCodeDtlsTOList, tblProductItemTO, tblProductItemTO.CreatedBy, conn, tran);
                        if (resultMessage.Result != 1)
                        {
                            resultMessage.DefaultBehaviour();
                            return resultMessage;
                        }
                    }
                    #endregion

                    if (SAPServiceEnable == true && tblProductItemTO.BaseProdItemId > 0)
                    {
                        ResultMessage sapResult = SaveItemInSAP(tblProductItemTO, tblPurchaseItemMasterTOList, null);
                        if (sapResult.Result != 1)
                        {
                            tran.Rollback();
                            return sapResult;
                        }
                    }


                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "InsertTblProductItem at BL");
                return resultMessage;
            }
            finally
            {
                sapConn.Close();
            }
        }
        public ResultMessage AddUOMGroup(TblProductItemTO tblProductItemTO, Boolean SAPServiceEnable, SqlConnection conn, SqlConnection sapConn, SqlTransaction tran, SqlTransaction saptTran)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            try
            {
                #region Add UOM Group In SAP
                if (tblProductItemTO.UOMGroupId == 0)
                {
                    List<DimUomGroupTO> UOMGroupList = _iDimUomGroupDAO.SelectAllDimUomGroup();
                    List<DropDownTO> UOMGroupDropDownList = new List<DropDownTO>();
                    if (UOMGroupList != null)
                    {
                        for (int i = 0; i < UOMGroupList.Count; i++)
                        {
                            DropDownTO dropDownTO = new DropDownTO();
                            dropDownTO.Text = UOMGroupList[i].UomGroupName;
                            dropDownTO.Value = UOMGroupList[i].IdUomGroup;
                            dropDownTO.Tag = UOMGroupList[i].UomGroupConversionTO;
                            dropDownTO.Code = UOMGroupList[i].UomGroupCode;
                            dropDownTO.MappedTxnId = UOMGroupList[i].MappedUomGroupId;
                            UOMGroupDropDownList.Add(dropDownTO);
                        }
                    }
                    List<DropDownTO> UnitMeasureList = _iDimUnitMeasuresDAO.SelectAllUnitMeasuresForDropDown();
                    if (UnitMeasureList == null || UnitMeasureList.Count == 0)
                    {
                        resultMessage.DefaultBehaviour("Error while SelectAllUnitMeasuresForDropDown");
                        return resultMessage;
                    }

                    var matchingWeightMeasureUnitIdTO = UnitMeasureList.Where(x => x.Value == tblProductItemTO.WeightMeasureUnitId).FirstOrDefault();
                    if (matchingWeightMeasureUnitIdTO == null)
                    {
                        resultMessage.DefaultBehaviour("Error while matchingWeightMeasureUnitIdTO Filter");
                        return resultMessage;
                    }
                    var matchingUOMTO = UnitMeasureList.Where(x => x.Value == tblProductItemTO.ConversionUnitOfMeasure).FirstOrDefault();
                    if (matchingUOMTO == null)
                    {
                        resultMessage.DefaultBehaviour("Error while UnitMeasureList Filter");
                        return resultMessage;
                    }
                    List<DropDownTO> UOMGroupConversionTO = new List<DropDownTO>();
                    if (SAPServiceEnable == true)
                        UOMGroupConversionTO = _iDimensionDAO.getUomGropConversionFromSAP(Convert.ToInt32(matchingWeightMeasureUnitIdTO.MappedTxnId), Convert.ToInt32(matchingUOMTO.MappedTxnId), tblProductItemTO.ConversionFactor, sapConn, saptTran);
                    else
                        UOMGroupConversionTO = _iDimensionDAO.getUomGropConversionFromSAP(Convert.ToInt32(matchingWeightMeasureUnitIdTO.Value), Convert.ToInt32(matchingUOMTO.Value), tblProductItemTO.ConversionFactor, sapConn, saptTran);
                    if (UOMGroupConversionTO != null && UOMGroupConversionTO.Count > 0)
                    {
                        tblProductItemTO.MappedUOMGroupId = Convert.ToString(UOMGroupConversionTO[0].Value);
                        var matchingUOMGrpTO = UOMGroupDropDownList.Where(x => x.MappedTxnId == tblProductItemTO.MappedUOMGroupId).FirstOrDefault();
                        if (matchingUOMGrpTO != null)
                        {
                            tblProductItemTO.UOMGroupId = matchingUOMGrpTO.Value;
                        }
                    }
                    else
                    {
                        DimUomGroupTO dimUomGroupTO = new DimUomGroupTO();
                        dimUomGroupTO.UomGroupCode = matchingWeightMeasureUnitIdTO.Text + "_" + matchingUOMTO.Text + "_" + tblProductItemTO.ConversionFactor;
                        dimUomGroupTO.UomGroupName = matchingWeightMeasureUnitIdTO.Text + "_" + matchingUOMTO.Text + "_" + tblProductItemTO.ConversionFactor;
                        dimUomGroupTO.BaseUomId = matchingWeightMeasureUnitIdTO.Value;
                        dimUomGroupTO.CreatedBy = tblProductItemTO.CreatedBy;
                        dimUomGroupTO.CreatedOn = _iCommon.ServerDateTime;
                        dimUomGroupTO.IsActive = 1;
                        dimUomGroupTO.UomGroupConversionTO = new DimUomGroupConversionTO();
                        dimUomGroupTO.UomGroupConversionTO.AltQty = tblProductItemTO.ConversionFactor;
                        dimUomGroupTO.UomGroupConversionTO.BaseQty = 1;
                        dimUomGroupTO.UomGroupConversionTO.UomId = matchingUOMTO.Value;
                        result = _iDimUomGroupDAO.InsertDimUomGroup(dimUomGroupTO, conn, tran);
                        if (result != 1)
                        {
                            resultMessage.DefaultBehaviour("Error while InsertDimUomGroup");
                            return resultMessage;
                        }
                        dimUomGroupTO.UpdatedBy = tblProductItemTO.CreatedBy;
                        dimUomGroupTO.UpdatedOn = dimUomGroupTO.CreatedOn;
                        dimUomGroupTO.UomGroupConversionTO.UomGroupId = dimUomGroupTO.IdUomGroup;
                        tblProductItemTO.UOMGroupId = dimUomGroupTO.IdUomGroup;
                        result = _iDimUomGroupConversionDAO.InsertDimUomGroupConversion(dimUomGroupTO.UomGroupConversionTO, conn, tran);
                        if (result != 1)
                        {
                            resultMessage.DefaultBehaviour("Error while InsertDimUomGroupConversion");
                            return resultMessage;
                        }
                        Int32 UgpEntryFromOUGP = 0;
                        if (SAPServiceEnable == true)
                        {
                            UgpEntryFromOUGP = _iDimensionDAO.getMaxCountOfSAPTable("UgpEntry", "OUGP", sapConn, saptTran) + 1;
                            Int32 UOMEntryId = _iDimUnitMeasuresDAO.InsertUOMGroupInSAP(dimUomGroupTO.UomGroupCode, Convert.ToInt32(matchingWeightMeasureUnitIdTO.MappedTxnId), UgpEntryFromOUGP, sapConn, saptTran);
                            if (UOMEntryId == 0)
                            {
                                resultMessage.DefaultBehaviour("Error while InsertUOMGroupInSAP");
                                return resultMessage;
                            }
                        }
                        tblProductItemTO.MappedUOMGroupId = Convert.ToString(UgpEntryFromOUGP);
                        dimUomGroupTO.MappedUomGroupId = Convert.ToString(UgpEntryFromOUGP);
                        if (SAPServiceEnable == true)
                        {
                            Int32 UOMConversionId = _iDimUnitMeasuresDAO.InsertUOMGroupConversionInSAP(Convert.ToInt32(matchingWeightMeasureUnitIdTO.MappedTxnId), 1, UgpEntryFromOUGP, 1, sapConn, saptTran);
                            if (UOMConversionId == 0)
                            {
                                resultMessage.DefaultBehaviour("Error while InsertUOMGroupConversionInSAP");
                                return resultMessage;
                            }
                            UOMConversionId = _iDimUnitMeasuresDAO.InsertUOMGroupConversionInSAP(Convert.ToInt32(matchingUOMTO.MappedTxnId), tblProductItemTO.ConversionFactor, UgpEntryFromOUGP, 2, sapConn, saptTran);
                            if (UOMConversionId == 0)
                            {
                                resultMessage.DefaultBehaviour("Error while InsertUOMGroupConversionInSAP");
                                return resultMessage;
                            }
                            result = _iDimUomGroupDAO.UpdateDimUomGroup(dimUomGroupTO, conn, tran);
                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour("Error while UpdateDimUomGroup");
                                return resultMessage;
                            }
                        }
                    }
                }

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
                #endregion
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "AddUOMGroup");
                return resultMessage;

            }
        }

        public ResultMessage AddUOMGroupV2(TblProductItemTO tblProductItemTO, Boolean SAPServiceEnable, SqlConnection conn, SqlConnection sapConn, SqlTransaction tran, SqlTransaction saptTran)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            try
            {
                #region Add UOM Group In SAP
                if (tblProductItemTO.UOMGroupId == 0 || tblProductItemTO.BaseProdItemId == 0)
                {
                    List<DimUomGroupTO> UOMGroupList = _iDimUomGroupDAO.SelectAllDimUomGroup();
                    List<DropDownTO> UOMGroupDropDownList = new List<DropDownTO>();
                    if (UOMGroupList != null)
                    {
                        for (int i = 0; i < UOMGroupList.Count; i++)
                        {
                            DropDownTO dropDownTO = new DropDownTO();
                            dropDownTO.Text = UOMGroupList[i].UomGroupName;
                            dropDownTO.Value = UOMGroupList[i].IdUomGroup;
                            dropDownTO.Tag = UOMGroupList[i].UomGroupConversionTO;
                            dropDownTO.Code = UOMGroupList[i].UomGroupCode;
                            dropDownTO.MappedTxnId = UOMGroupList[i].MappedUomGroupId;
                            UOMGroupDropDownList.Add(dropDownTO);
                        }
                    }
                    List<DropDownTO> UnitMeasureList = _iDimUnitMeasuresDAO.SelectAllUnitMeasuresForDropDown();
                    if (UnitMeasureList == null || UnitMeasureList.Count == 0)
                    {
                        resultMessage.DefaultBehaviour("Error while SelectAllUnitMeasuresForDropDown");
                        return resultMessage;
                    }

                    var matchingWeightMeasureUnitIdTO = UnitMeasureList.Where(x => x.Value == tblProductItemTO.WeightMeasureUnitId).FirstOrDefault();
                    if (matchingWeightMeasureUnitIdTO == null)
                    {
                        resultMessage.DefaultBehaviour("Error while matchingWeightMeasureUnitIdTO Filter");
                        return resultMessage;
                    }
                    var matchingUOMTO = UnitMeasureList.Where(x => x.Value == tblProductItemTO.ConversionUnitOfMeasure).FirstOrDefault();
                    if (matchingUOMTO == null)
                    {
                        resultMessage.DefaultBehaviour("Error while UnitMeasureList Filter");
                        return resultMessage;
                    }
                    List<DropDownTO> UOMGroupConversionTO = new List<DropDownTO>();
                    if (SAPServiceEnable == true)
                        UOMGroupConversionTO = _iDimensionDAO.getUomGropConversionFromSAP(Convert.ToInt32(matchingWeightMeasureUnitIdTO.MappedTxnId), Convert.ToInt32(matchingUOMTO.MappedTxnId), tblProductItemTO.ConversionFactor, sapConn, saptTran);
                    else
                        UOMGroupConversionTO = _iDimensionDAO.getUomGropConversionFromSAP(Convert.ToInt32(matchingWeightMeasureUnitIdTO.Value), Convert.ToInt32(matchingUOMTO.Value), tblProductItemTO.ConversionFactor, sapConn, saptTran);
                    if (UOMGroupConversionTO != null && UOMGroupConversionTO.Count > 0)
                    {
                        tblProductItemTO.MappedUOMGroupId = Convert.ToString(UOMGroupConversionTO[0].Value);
                        var matchingUOMGrpTO = UOMGroupDropDownList.Where(x => x.MappedTxnId == tblProductItemTO.MappedUOMGroupId).FirstOrDefault();
                        if (matchingUOMGrpTO != null)
                        {
                            tblProductItemTO.UOMGroupId = matchingUOMGrpTO.Value;
                        }
                    }
                    else
                    {
                        DimUomGroupTO dimUomGroupTO = new DimUomGroupTO();
                        dimUomGroupTO.UomGroupCode = matchingWeightMeasureUnitIdTO.Text + "_" + matchingUOMTO.Text + "_" + tblProductItemTO.ConversionFactor;
                        dimUomGroupTO.UomGroupName = matchingWeightMeasureUnitIdTO.Text + "_" + matchingUOMTO.Text + "_" + tblProductItemTO.ConversionFactor;
                        dimUomGroupTO.BaseUomId = matchingWeightMeasureUnitIdTO.Value;
                        dimUomGroupTO.CreatedBy = tblProductItemTO.CreatedBy;
                        dimUomGroupTO.CreatedOn = _iCommon.ServerDateTime;
                        dimUomGroupTO.IsActive = 1;
                        dimUomGroupTO.UomGroupConversionTO = new DimUomGroupConversionTO();
                        dimUomGroupTO.UomGroupConversionTO.AltQty = tblProductItemTO.ConversionFactor;
                        dimUomGroupTO.UomGroupConversionTO.BaseQty = 1;
                        dimUomGroupTO.UomGroupConversionTO.UomId = matchingUOMTO.Value;
                        result = _iDimUomGroupDAO.InsertDimUomGroup(dimUomGroupTO, conn, tran);
                        if (result != 1)
                        {
                            resultMessage.DefaultBehaviour("Error while InsertDimUomGroup");
                            return resultMessage;
                        }
                        dimUomGroupTO.UpdatedBy = tblProductItemTO.CreatedBy;
                        dimUomGroupTO.UpdatedOn = dimUomGroupTO.CreatedOn;
                        dimUomGroupTO.UomGroupConversionTO.UomGroupId = dimUomGroupTO.IdUomGroup;
                        tblProductItemTO.UOMGroupId = dimUomGroupTO.IdUomGroup;
                        result = _iDimUomGroupConversionDAO.InsertDimUomGroupConversion(dimUomGroupTO.UomGroupConversionTO, conn, tran);
                        if (result != 1)
                        {
                            resultMessage.DefaultBehaviour("Error while InsertDimUomGroupConversion");
                            return resultMessage;
                        }
                        Int32 UgpEntryFromOUGP = 0;
                        if (SAPServiceEnable == true)
                        {
                            //UgpEntryFromOUGP = _iDimensionDAO.getMaxCountOfSAPTable("UgpEntry", "OUGP", sapConn, saptTran) + 1;
                            //Int32 UOMEntryId = _iDimUnitMeasuresDAO.InsertUOMGroupInSAP(dimUomGroupTO.UomGroupCode, Convert.ToInt32(matchingWeightMeasureUnitIdTO.MappedTxnId), UgpEntryFromOUGP, sapConn, saptTran);
                            //if (UOMEntryId == 0)
                            //{
                            //    resultMessage.DefaultBehaviour("Error while InsertUOMGroupInSAP");
                            //    return resultMessage;
                            //}
                            try
                            {
                                //Code for adding UOM group //using API
                                CompanyService oCompanyService = Startup.CompanyObject.GetCompanyService();
                                UnitOfMeasurementGroupsService ouomgrp = (UnitOfMeasurementGroupsService)oCompanyService.GetBusinessService(ServiceTypes.UnitOfMeasurementGroupsService);
                                SAPbobsCOM.UnitOfMeasurementGroup uomgrp = (SAPbobsCOM.UnitOfMeasurementGroup)ouomgrp.GetDataInterface
                                (UnitOfMeasurementGroupsServiceDataInterfaces.uomgsUnitOfMeasurementGroup);
                                uomgrp.Code = dimUomGroupTO.UomGroupCode;
                                uomgrp.Name = dimUomGroupTO.UomGroupCode;
                                uomgrp.BaseUoM = Convert.ToInt32(matchingWeightMeasureUnitIdTO.MappedTxnId);
                                ouomgrp.Add(uomgrp);
                            }
                            catch (Exception ex)
                            {
                                resultMessage.DefaultBehaviour("Error while UOM Group Creation in SAP Ex:" + ex.ToString());
                                return resultMessage;
                            }

                            UgpEntryFromOUGP = _iDimensionDAO.GetSAPUOMGroupUGPEntry(dimUomGroupTO.UomGroupCode, sapConn, saptTran);
                        }
                        tblProductItemTO.MappedUOMGroupId = Convert.ToString(UgpEntryFromOUGP);
                        dimUomGroupTO.MappedUomGroupId = Convert.ToString(UgpEntryFromOUGP);
                        if (SAPServiceEnable == true)
                        {
                            Int32 UOMConversionId = 0;
                            //UOMConversionId = _iDimUnitMeasuresDAO.InsertUOMGroupConversionInSAP(Convert.ToInt32(matchingWeightMeasureUnitIdTO.MappedTxnId), 1, UgpEntryFromOUGP, 1, sapConn, saptTran);
                            //if (UOMConversionId == 0)
                            //{
                            //    resultMessage.DefaultBehaviour("Error while InsertUOMGroupConversionInSAP");
                            //    return resultMessage;
                            //}

                            if (matchingWeightMeasureUnitIdTO.Value != matchingUOMTO.Value)
                            {
                                UOMConversionId = _iDimUnitMeasuresDAO.InsertUOMGroupConversionInSAP(Convert.ToInt32(matchingUOMTO.MappedTxnId), tblProductItemTO.ConversionFactor, UgpEntryFromOUGP, 2, sapConn, saptTran);
                                if (UOMConversionId == 0)
                                {
                                    resultMessage.DefaultBehaviour("Error while InsertUOMGroupConversionInSAP");
                                    return resultMessage;
                                }
                            }
                            result = _iDimUomGroupDAO.UpdateDimUomGroup(dimUomGroupTO, conn, tran);
                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour("Error while UpdateDimUomGroup");
                                return resultMessage;
                            }
                        }
                    }
                }

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
                #endregion
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "AddUOMGroup");
                return resultMessage;

            }
        }

        public ResultMessage AddUOMGroupV2FromMaster(DimUomGroupTO dimUomGroupTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            SqlConnection sapConn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.SAP_CONNECTION_STRING));
            SqlTransaction saptTran = null;
            int result = 0;
            int res = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                Boolean SAPServiceEnable = false;
                TblConfigParamsTO tblConfigParamsTOSAPService = _iTblConfigParamsDAO.SelectTblConfigParams(Constants.SAPB1_SERVICES_ENABLE);
                if (tblConfigParamsTOSAPService != null)
                {
                    if (Convert.ToInt32(tblConfigParamsTOSAPService.ConfigParamVal) == 1)
                    {
                        SAPServiceEnable = true;
                        sapConn.Open();
                        saptTran = sapConn.BeginTransaction();
                        resultMessage = AddUOMGroupV2FromMaster(dimUomGroupTO, SAPServiceEnable, conn, sapConn, tran, saptTran);
                        if(resultMessage.Result <=0 )
                        {
                            tran.Rollback();
                            saptTran.Rollback();
                        }

                        tran.Commit();
                        saptTran.Commit();
                        return resultMessage;
                    }
                }

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "AddUOMGroup");
                return resultMessage;
            }
            finally
            {
                conn.Close();
                sapConn.Close();
            }
        }
        public ResultMessage AddUOMGroupV2FromMaster(DimUomGroupTO uomGroupTO, Boolean SAPServiceEnable, SqlConnection conn, SqlConnection sapConn, SqlTransaction tran, SqlTransaction saptTran)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            try
            {
                #region Add UOM Group In SAP

               List<DimUomGroupTO> UOMGroupList = _iDimUomGroupDAO.SelectAllDimUomGroup();
               List<DropDownTO> UOMGroupDropDownList = new List<DropDownTO>();
               if (UOMGroupList != null)
                    {
                        for (int i = 0; i < UOMGroupList.Count; i++)
                        {
                            DropDownTO dropDownTO = new DropDownTO();
                            dropDownTO.Text = UOMGroupList[i].UomGroupName;
                            dropDownTO.Value = UOMGroupList[i].IdUomGroup;
                            dropDownTO.Tag = UOMGroupList[i].UomGroupConversionTO;
                            dropDownTO.Code = UOMGroupList[i].UomGroupCode;
                            dropDownTO.MappedTxnId = UOMGroupList[i].MappedUomGroupId;
                            UOMGroupDropDownList.Add(dropDownTO);
                        }
                    }
               List<DropDownTO> UnitMeasureList = _iDimUnitMeasuresDAO.SelectAllUnitMeasuresForDropDown();
               if (UnitMeasureList == null || UnitMeasureList.Count == 0)
                    {
                        resultMessage.DefaultBehaviour("Error while SelectAllUnitMeasuresForDropDown");
                        return resultMessage;
                    }

               var matchingWeightMeasureUnitIdTO = UnitMeasureList.Where(x => x.Value == uomGroupTO.BaseUomId).FirstOrDefault();
               if (matchingWeightMeasureUnitIdTO == null)
                    {
                        resultMessage.DefaultBehaviour("Error while matchingWeightMeasureUnitIdTO Filter");
                        return resultMessage;
                    }
               var matchingUOMTO = UnitMeasureList.Where(x => x.Value == uomGroupTO.ConversionUnitOfMeasure).FirstOrDefault();
               if (matchingUOMTO == null)
                    {
                        resultMessage.DefaultBehaviour("Error while UnitMeasureList Filter");
                        return resultMessage;
                    }
               List<DropDownTO> UOMGroupConversionTO = new List<DropDownTO>();
               if (SAPServiceEnable == true)
                   UOMGroupConversionTO = _iDimensionDAO.getUomGropConversionFromSAP(Convert.ToInt32(matchingWeightMeasureUnitIdTO.MappedTxnId), Convert.ToInt32(matchingUOMTO.MappedTxnId), uomGroupTO.ConversionFactor, sapConn, saptTran);
               else
                   UOMGroupConversionTO = _iDimensionDAO.getUomGropConversionFromSAP(Convert.ToInt32(matchingWeightMeasureUnitIdTO.Value), Convert.ToInt32(matchingUOMTO.Value), uomGroupTO.ConversionFactor, sapConn, saptTran);
               if (UOMGroupConversionTO != null && UOMGroupConversionTO.Count > 0)
                {
                    uomGroupTO.MappedUomGroupId = Convert.ToString(UOMGroupConversionTO[0].Value);
                    var matchingUOMGrpTO = UOMGroupDropDownList.Where(x => x.MappedTxnId == uomGroupTO.MappedUomGroupId).FirstOrDefault();
                    if (matchingUOMGrpTO != null)
                    {
                        if (uomGroupTO.IdUomGroup > 0)
                        {
                            result = _iDimUomGroupConversionDAO.UpdateDimUomGroupConversionForConversion(uomGroupTO.IdUomGroup, uomGroupTO.ConversionFactor, uomGroupTO.ConversionUnitOfMeasure, conn, tran);
                            if (result > 0)
                            {
                                if (SAPServiceEnable == true)
                                {
                                    DimUnitMeasuresTO dimUnitMeasuresTO = _iDimUnitMeasuresDAO.SelectDimUnitMeasures(uomGroupTO.ConversionUnitOfMeasure);
                                    result = _iDimensionDAO.UpdateSAPUOMGroupUGPEntry(Convert.ToInt32(uomGroupTO.MappedUomGroupId), uomGroupTO.ConversionFactor, dimUnitMeasuresTO.MappedTxnId, sapConn, saptTran);
                                    if (result > 0)
                                    {
                                        uomGroupTO.IdUomGroup = matchingUOMGrpTO.Value;
                                        resultMessage.MessageType = ResultMessageE.Information;
                                        resultMessage.Text = "Group updated Successfully";
                                        resultMessage.Result = 1;
                                        resultMessage.DisplayMessage = "Group updated Successfully";
                                        return resultMessage;
                                    }
                                }
                                else
                                {
                                    uomGroupTO.IdUomGroup = matchingUOMGrpTO.Value;
                                    resultMessage.MessageType = ResultMessageE.Information;
                                    resultMessage.Text = "Group updated Successfully";
                                    resultMessage.Result = 1;
                                    resultMessage.DisplayMessage = "Group updated Successfully";
                                    return resultMessage;
                                }

                            }
                            else
                            {
                                uomGroupTO.IdUomGroup = matchingUOMGrpTO.Value;
                                resultMessage.MessageType = ResultMessageE.Information;
                                resultMessage.Text = "Error in Updation of UOM Group";
                                resultMessage.Result = 1;
                                resultMessage.DisplayMessage = "Error in Updation of UOM Group";
                                return resultMessage;
                            }
                        }
                        else
                        {
                            uomGroupTO.IdUomGroup = matchingUOMGrpTO.Value;
                            resultMessage.MessageType = ResultMessageE.Information;
                            resultMessage.Text = "Group is already available in SAP";
                            resultMessage.Result = 1;
                            resultMessage.DisplayMessage = "Group is already available in SAP";
                            return resultMessage;
                        }
                    }
                    else
                    {
                        resultMessage.MessageType = ResultMessageE.Information;
                        resultMessage.Text = "Group is already available in SAP";
                        resultMessage.Result = 1;
                        resultMessage.DisplayMessage = "Group is already available in SAP";
                        return resultMessage;
                    }
                }
               else
               {
                   DimUomGroupTO dimUomGroupTO = new DimUomGroupTO();
                   dimUomGroupTO.UomGroupCode = uomGroupTO.UomGroupName;
                   dimUomGroupTO.UomGroupName = uomGroupTO.UomGroupCode;
                   dimUomGroupTO.BaseUomId = matchingWeightMeasureUnitIdTO.Value;
                   dimUomGroupTO.CreatedBy = uomGroupTO.CreatedBy;
                   dimUomGroupTO.CreatedOn = _iCommon.ServerDateTime;
                   dimUomGroupTO.UpdatedOn = _iCommon.ServerDateTime;
                   dimUomGroupTO.IsActive = 1;
                   dimUomGroupTO.UomGroupConversionTO = new DimUomGroupConversionTO();
                   dimUomGroupTO.ConversionFactor = uomGroupTO.ConversionFactor;
                   dimUomGroupTO.UomGroupConversionTO.AltQty = uomGroupTO.ConversionFactor;
                   dimUomGroupTO.UomGroupConversionTO.BaseQty = 1;
                   dimUomGroupTO.UomGroupConversionTO.UomId = matchingUOMTO.Value;
                   dimUomGroupTO.UomGroupConversionTO.UomGroupId = uomGroupTO.IdUomGroup;
                   dimUomGroupTO.IdUomGroup = uomGroupTO.IdUomGroup;
                    var matchingGroupTo = new DimUomGroupTO();
                    List<DropDownTO> UOMGroupConversionTOtemp = _iDimensionDAO.getUomGropConversionFromSAP(Convert.ToInt32(matchingWeightMeasureUnitIdTO.MappedTxnId), Convert.ToInt32(matchingUOMTO.MappedTxnId),0, sapConn, saptTran);
                    if (UOMGroupConversionTOtemp != null && UOMGroupConversionTOtemp.Count > 0)
                    {
                        uomGroupTO.IdUomGroup = 0;
                        uomGroupTO.UomGroupCode = matchingWeightMeasureUnitIdTO.Text + '_' + matchingUOMTO.Text + '_' + uomGroupTO.ConversionFactor;
                        dimUomGroupTO.UomGroupCode = uomGroupTO.UomGroupCode;
                        dimUomGroupTO.UomGroupName = uomGroupTO.UomGroupCode;
                    }
                    if (uomGroupTO.IdUomGroup > 0)
                    {
                        dimUomGroupTO.MappedUomGroupId = uomGroupTO.MappedUomGroupId;
                        matchingGroupTo = UOMGroupList.Where(x => x.IdUomGroup == uomGroupTO.IdUomGroup).FirstOrDefault();
                    }
                    else
                    {
                        matchingGroupTo = UOMGroupList.Where(x => x.BaseUomId == uomGroupTO.BaseUomId).FirstOrDefault();
                        if (matchingGroupTo != null)
                        {
                            List<DimUomGroupConversionTO> dimUomGroupConversionTOList = _iDimUomGroupConversionDAO.GetAllUOMGroupConversionListByGroupId(matchingGroupTo.IdUomGroup);
                            dimUomGroupConversionTOList = dimUomGroupConversionTOList.Where(w => w.UomId == dimUomGroupTO.UomGroupConversionTO.UomId).ToList();
                            if (dimUomGroupConversionTOList != null && dimUomGroupConversionTOList.Count > 0)
                            {
                                dimUomGroupConversionTOList = dimUomGroupConversionTOList.Where(w => w.AltQty == dimUomGroupTO.UomGroupConversionTO.AltQty).ToList();
                                if (dimUomGroupConversionTOList != null && dimUomGroupConversionTOList.Count == 0)
                                    matchingGroupTo = null;
                            }
                            else
                            {
                                matchingGroupTo = null;

                            }
                        }

                    }
                   if (matchingGroupTo == null)
                   {
                       result = _iDimUomGroupDAO.InsertDimUomGroup(dimUomGroupTO, conn, tran);
                       if (result != 1)
                       {
                           resultMessage.DefaultBehaviour("Error while InsertDimUomGroup");
                           return resultMessage;
                       }
                        dimUomGroupTO.UpdatedBy = uomGroupTO.CreatedBy;
                        dimUomGroupTO.UpdatedOn = dimUomGroupTO.CreatedOn;
                        dimUomGroupTO.UomGroupConversionTO.UomGroupId = dimUomGroupTO.IdUomGroup;
                    }
                    else if(uomGroupTO.IdUomGroup == 0) //Reshma[30-12-2020] For allow only same basegroup again
                    {
                        resultMessage.MessageType = ResultMessageE.Information;
                        resultMessage.Text = "Same base Group Name already exist";
                        resultMessage.Result = 1;
                        resultMessage.DisplayMessage = "Same base Group Name already exist";
                        return resultMessage;
                    }
                    result = _iDimUomGroupConversionDAO.InsertDimUomGroupConversion(dimUomGroupTO.UomGroupConversionTO, conn, tran);
                   if (result != 1)
                   {
                       resultMessage.DefaultBehaviour("Error while InsertDimUomGroupConversion");
                       return resultMessage;
                   }
                   Int32 UgpEntryFromOUGP = 0;
                    UgpEntryFromOUGP =Convert.ToInt32(dimUomGroupTO.MappedUomGroupId);
                    if (dimUomGroupTO.MappedUomGroupId == null || dimUomGroupTO.MappedUomGroupId == "" || dimUomGroupTO.MappedUomGroupId == "0")
                    {
                        if (SAPServiceEnable == true)
                        {
                            try
                            {
                                //Code for adding UOM group //using API
                                CompanyService oCompanyService = Startup.CompanyObject.GetCompanyService();
                                UnitOfMeasurementGroupsService ouomgrp = (UnitOfMeasurementGroupsService)oCompanyService.GetBusinessService(ServiceTypes.UnitOfMeasurementGroupsService);
                                SAPbobsCOM.UnitOfMeasurementGroup uomgrp = (SAPbobsCOM.UnitOfMeasurementGroup)ouomgrp.GetDataInterface
                                (UnitOfMeasurementGroupsServiceDataInterfaces.uomgsUnitOfMeasurementGroup);
                                uomgrp.Code = dimUomGroupTO.UomGroupCode;
                                uomgrp.Name = dimUomGroupTO.UomGroupCode;
                                uomgrp.BaseUoM = Convert.ToInt32(matchingWeightMeasureUnitIdTO.MappedTxnId);
                                ouomgrp.Add(uomgrp);
                            }
                            catch (Exception ex)
                            {
                                resultMessage.DefaultBehaviour("Error while UOM Group Creation in SAP Ex:" + ex.ToString());
                                return resultMessage;
                            }

                            UgpEntryFromOUGP = _iDimensionDAO.GetSAPUOMGroupUGPEntry(dimUomGroupTO.UomGroupCode, sapConn, saptTran);
                        }
                        dimUomGroupTO.MappedUomGroupId = Convert.ToString(UgpEntryFromOUGP);
                    }
                   if (SAPServiceEnable == true)
                   {
                       Int32 UOMConversionId = 0;                         
                       if (matchingWeightMeasureUnitIdTO.Value != matchingUOMTO.Value)
                       {
                           UOMConversionId = _iDimUnitMeasuresDAO.InsertUOMGroupConversionInSAP(Convert.ToInt32(matchingUOMTO.MappedTxnId), dimUomGroupTO.ConversionFactor, UgpEntryFromOUGP, 2, sapConn, saptTran);
                           if (UOMConversionId == 0)
                           {
                               resultMessage.DefaultBehaviour("Error while InsertUOMGroupConversionInSAP");
                               return resultMessage;
                           }
                       }
                       result = _iDimUomGroupDAO.UpdateDimUomGroup(dimUomGroupTO, conn, tran);
                       if (result != 1)
                       {
                           resultMessage.DefaultBehaviour("Error while UpdateDimUomGroup");
                           return resultMessage;
                       }
                   }                    
                }

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
                #endregion
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "AddUOMGroup");
                return resultMessage;

            }
        }
        public ResultMessage SaveItemInSAP(TblProductItemTO tblProductItemTO, List<TblPurchaseItemMasterTO> tblPurchaseItemMasterTOList, List<WareHouseWiseItemDtlsTO> wareHouseWiseItemDtlsTOList)
        {
            ResultMessage resultMessage = new ResultMessage();
            SAPbobsCOM.Items oitm = null;
            TblPurchaseItemMasterTO priooneSuppTO = null;
            try
            {
                if (Startup.CompanyObject == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "SAP CompanyObject Found NULL. Connectivity Error. " + Startup.SapConnectivityErrorCode;
                    resultMessage.DisplayMessage = "Error while creating item in SAP with Exception";
                    return resultMessage;
                }

                oitm = Startup.CompanyObject.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems);
                oitm.ItemCode = tblProductItemTO.IdProdItem.ToString();


                //oitm.ItemName = tblProductItemTO.ItemName;
                //oitm.ForeignName = tblProductItemTO.ItemDesc;
                //Samadhan[15/05/2023] added 
                if (tblProductItemTO.ItemName.Length > 100 && tblProductItemTO.ItemDesc.Length >0)
                {
                    oitm.ItemName = Convert.ToString(tblProductItemTO.ItemName).Substring(0, 100);
                    oitm.ForeignName = Convert.ToString(tblProductItemTO.ItemDesc).Substring(0, 100);
                }
                else
                {
                    oitm.ItemName = tblProductItemTO.ItemName;
                    oitm.ForeignName = tblProductItemTO.ItemDesc;
                }

                //Harshala[17/09/2020] added to set Fixed Asset in SAP
                if (tblProductItemTO.IsFixedAsset)
                    oitm.ItemType = ItemTypeEnum.itFixedAssets;
                //
                oitm.ItemsGroupCode = Convert.ToInt32(tblProductItemTO.SapItemGroupId);
                //oitm.UserFields.Fields.Item("U_Is_Certi_Comp").Value = "Y";

                //oitm.DefaultWarehouse = "01";
                oitm.DefaultWarehouse = tblProductItemTO.MappedLocationId.ToString();

                #region General Data
                if (tblProductItemTO.CodeTypeId == 2)
                    tblProductItemTO.IsInventoryItem = 0;
                oitm.PurchaseItem = Constants.GetYesNoEnum(tblProductItemTO.IsPurchaseItem);
                oitm.InventoryItem = Constants.GetYesNoEnum(tblProductItemTO.IsInventoryItem);
               
                oitm.SalesItem = Constants.GetYesNoEnum(tblProductItemTO.IsSalesItem);
                //Harshala added to Set Service/Material category and created new table instead of using hardcoded value.
                if (tblProductItemTO.IsInventoryItem == 1)
                    oitm.GSTTaxCategory = Constants.GetGstTaxCtg(tblProductItemTO.TaxCategoryId);
                else
                    oitm.ServiceCategoryEntry = tblProductItemTO.TaxCategoryId;//Reshma[16-09-2020]
                //
                //From Generic Master it should map column mapped txn id to pass to SAP
                if (tblProductItemTO.MappedShippingTypeId > 0)
                    oitm.ShipType = tblProductItemTO.MappedShippingTypeId;

                if (tblProductItemTO.MappedManufacturerId > 0)
                    oitm.Manufacturer = tblProductItemTO.MappedManufacturerId;
                //oitm.Location = tblProductItemTO.MappedLocationId;

                oitm.UoMGroupEntry = Convert.ToInt32(tblProductItemTO.MappedUOMGroupId);
                oitm.PricingUnit = Convert.ToInt32(tblProductItemTO.MappedConversionUnitOfMeasure);

                oitm.MaterialType = Constants.GetMaterialTypesEnum(tblProductItemTO.MaterialTypeId);
                oitm.GSTRelevnt = Constants.GetYesNoEnum(tblProductItemTO.IsGSTApplicable);

                if (tblProductItemTO.ManageItemById == (Int32)Constants.ManageItemByIdE.SERIAL_NO)
                {
                    oitm.ManageSerialNumbers = SAPbobsCOM.BoYesNoEnum.tYES;
                    //AmolG[2020-Jul-28] In case of Serial No set oitm.SRIAndBatchManageMethod enum to Every Transaction
                    //oitm.SRIAndBatchManageMethod = Constant.GetManageMethodEnum(tblProductItemTO.MgmtMethodId);          //MgmtMethodId
                    oitm.SRIAndBatchManageMethod = Constants.GetManageMethodEnum(0);          //MgmtMethodId
                    oitm.WarrantyTemplate = tblProductItemTO.WarrantyTemplateName;
                    tblProductItemTO.ValuationId = 3;

                    //Reshma[14-09-2020] Commented for Valuation method issue
                    //oitm.ManageSerialNumbers = SAPbobsCOM.BoYesNoEnum.tYES;
                    //oitm.SRIAndBatchManageMethod = Constants.GetManageMethodEnum(tblProductItemTO.MgmtMethodId);          //MgmtMethodId
                    //oitm.WarrantyTemplate = tblProductItemTO.WarrantyTemplateName;
                }
                else if (tblProductItemTO.ManageItemById == (Int32)Constants.ManageItemByIdE.BATCHES)
                {
                    oitm.ManageBatchNumbers = SAPbobsCOM.BoYesNoEnum.tYES;
                    oitm.SRIAndBatchManageMethod = Constants.GetManageMethodEnum(tblProductItemTO.MgmtMethodId);          //MgmtMethodId
                    oitm.WarrantyTemplate = tblProductItemTO.WarrantyTemplateName;
                }

                oitm.PurchaseVolumeUnit = 1;
                if (Convert.ToInt32(tblProductItemTO.SapHSNCode) > 0)
                    oitm.ChapterID = Convert.ToInt32(tblProductItemTO.SapHSNCode);
                if(tblProductItemTO.CodeTypeId == 2 && tblProductItemTO.SapSACCode != 0)
                        oitm.SACEntry = Convert.ToInt32(tblProductItemTO.SapSACCode);
              
                oitm.SWW = tblProductItemTO.AdditionalIdent;



                #endregion

                // oitm.PriceList.. tblProductItemTO.PriceListId;
                //Valuation method, ManageItemById, VolumeType

                #region Purchase Data
                if (tblPurchaseItemMasterTOList != null)
                {
                    if (tblPurchaseItemMasterTOList.Count == 1)
                        priooneSuppTO = tblPurchaseItemMasterTOList[0];
                    else
                        priooneSuppTO = tblPurchaseItemMasterTOList.Where(x => x.Priority == 1).FirstOrDefault();
                }

                if (priooneSuppTO != null)
                {
                    if(priooneSuppTO.SupplierOrgId.ToString() != "0")
                    {
                        oitm.Mainsupplier = priooneSuppTO.SupplierOrgId.ToString();
                        oitm.SupplierCatalogNo = priooneSuppTO.MfgCatlogNo;
                        //oitm.OrderMultiple = Convert.ToDouble(priooneSuppTO.MinimumOrderQty);
                    }
                    //if (priooneSuppTO.PurchaseUOMId == tblProductItemTO.WeightMeasureUnitId)
                    //    oitm.DefaultPurchasingUoMEntry = tblProductItemTO.MappedWeightMeasureUnitId;
                    //else if (priooneSuppTO.PurchaseUOMId == tblProductItemTO.ConversionUnitOfMeasure)
                    //    oitm.DefaultPurchasingUoMEntry = tblProductItemTO.MappedConversionUnitOfMeasure;
                    if (priooneSuppTO.PurchaseUOMId == priooneSuppTO.PurchaseUOMId)
                        oitm.DefaultPurchasingUoMEntry = priooneSuppTO.MappedPurchaseUOMId;

                }
                oitm.OrderMultiple = Convert.ToDouble(tblProductItemTO.OrderMultiple);
                
                for (int pv = 0; pv < tblPurchaseItemMasterTOList.Count; pv++)
                {
                    if (tblPurchaseItemMasterTOList[pv].SupplierOrgId.ToString() != "0")
                    {
                        oitm.PreferredVendors.SetCurrentLine(pv);
                        oitm.PreferredVendors.BPCode = tblPurchaseItemMasterTOList[pv].SupplierOrgId.ToString();
                        oitm.PreferredVendors.Add();
                    }
                    //if (tblPurchaseItemMasterTOList[pv].PurchaseUOMId == tblProductItemTO.WeightMeasureUnitId)
                    //    oitm.DefaultPurchasingUoMEntry = tblProductItemTO.MappedWeightMeasureUnitId;
                    //else if (tblPurchaseItemMasterTOList[pv].PurchaseUOMId == tblProductItemTO.ConversionUnitOfMeasure)
                    //    oitm.DefaultPurchasingUoMEntry = tblProductItemTO.MappedConversionUnitOfMeasure;
                }

                //AmolG[2020-Dec-16] For Warehouse wise inventory management.
                oitm.ManageStockByWarehouse = BoYesNoEnum.tNO;
                if (tblProductItemTO.IsManageInventory)
                {
                    oitm.ManageStockByWarehouse = BoYesNoEnum.tYES;

                    if (wareHouseWiseItemDtlsTOList != null && wareHouseWiseItemDtlsTOList.Count > 0)
                    {
                        Double minInv = 0;
                        Double maxInv = 0;
                        Double minOrd = 0;
                        Boolean isUpdateminOrderQty = false;//Reshma[1-3-22] For warehouse wise order Qty
                        List<WareHouseWiseItemDtlsTO> wareHouseWiseItemDtlsTOListNew = wareHouseWiseItemDtlsTOList.Where(w => w.MinOrder > 0).ToList();
                        if (wareHouseWiseItemDtlsTOListNew != null && wareHouseWiseItemDtlsTOListNew.Count > 0)
                        {
                            isUpdateminOrderQty = true;
                        }
                        for (int iCnt = 0; iCnt < wareHouseWiseItemDtlsTOList.Count; iCnt++)
                        {
                            if (!String.IsNullOrEmpty(wareHouseWiseItemDtlsTOList[iCnt].WhsCode))
                            {
                                oitm.WhsInfo.SetCurrentLine(iCnt);
                                oitm.WhsInfo.WarehouseCode = wareHouseWiseItemDtlsTOList[iCnt].WhsCode;
                                oitm.WhsInfo.MaximalStock = wareHouseWiseItemDtlsTOList[iCnt].MaxInventory;
                                oitm.WhsInfo.MinimalStock = wareHouseWiseItemDtlsTOList[iCnt].MinInventory;
                                oitm.WhsInfo.MinimalOrder = wareHouseWiseItemDtlsTOList[iCnt].MinOrder;
                                //Reshma Added FOr Rack Location.
                                if(!string .IsNullOrEmpty(wareHouseWiseItemDtlsTOList[iCnt].Rack))
                                    oitm.WhsInfo.UserFields.Fields.Item("U_Rack").Value = wareHouseWiseItemDtlsTOList[iCnt].Rack;
                                if (!string.IsNullOrEmpty(wareHouseWiseItemDtlsTOList[iCnt].Row))
                                    oitm.WhsInfo.UserFields.Fields.Item("U_Row").Value = wareHouseWiseItemDtlsTOList[iCnt].Row;
                                if (!string.IsNullOrEmpty(wareHouseWiseItemDtlsTOList[iCnt].Column))
                                    oitm.WhsInfo.UserFields.Fields.Item("U_Column").Value = wareHouseWiseItemDtlsTOList[iCnt].Column;
                                if (isUpdateminOrderQty && wareHouseWiseItemDtlsTOList[iCnt].MinOrder == 0 && wareHouseWiseItemDtlsTOList[iCnt].MinInventory >0)
                                    oitm.WhsInfo.MinimalOrder = wareHouseWiseItemDtlsTOList[iCnt].MinInventory + 1;
                                else
                                    oitm.WhsInfo.MinimalOrder = wareHouseWiseItemDtlsTOList[iCnt].MinOrder;
                                minInv += wareHouseWiseItemDtlsTOList[iCnt].MinInventory;
                                maxInv += wareHouseWiseItemDtlsTOList[iCnt].MaxInventory;
                                minOrd += wareHouseWiseItemDtlsTOList[iCnt].MinOrder;
                                oitm.WhsInfo.Add();
                            }
                        }

                        oitm.DefaultWarehouse = tblProductItemTO.MappedLocationId.ToString();
                        oitm.MinInventory = minInv;
                        oitm.MaxInventory = maxInv;
                        //oitm.MinOrderQuantity = minOrd;
                        //oitm.MinOrderQuantity = minOrd;
                        //oitm.OrderMultiple = minOrd;
                    }
                }
                else
                    oitm.MinInventory = tblProductItemTO.InventMinimum;

                oitm.MinOrderQuantity = tblProductItemTO.MinOrderQty;
                
                #endregion

                #region Sales Data

                //if (tblProductItemTO.SalesUOMId == tblProductItemTO.WeightMeasureUnitId)
                //    oitm.DefaultSalesUoMEntry = tblProductItemTO.MappedWeightMeasureUnitId;
                //else if (tblProductItemTO.SalesUOMId == tblProductItemTO.ConversionUnitOfMeasure)
                //    oitm.DefaultSalesUoMEntry = tblProductItemTO.MappedConversionUnitOfMeasure;

                oitm.DefaultSalesUoMEntry = tblProductItemTO.MappedSalesUOMId;

                if (tblProductItemTO.ItemPerSalesUnit != 0)
                    oitm.SalesItemsPerUnit = tblProductItemTO.ItemPerSalesUnit;

                if (tblProductItemTO.SalesQtyPerPkg != 0)
                    oitm.SalesQtyPerPackUnit = tblProductItemTO.SalesQtyPerPkg;

                if (tblProductItemTO.SalesHeight > 0)
                    oitm.SalesUnitHeight = tblProductItemTO.SalesHeight;
                if (tblProductItemTO.SalesWeight > 0)
                    oitm.SalesUnitWeight = tblProductItemTO.SalesWeight;
                if (tblProductItemTO.SalesWidth > 0)
                    oitm.SalesUnitWidth = tblProductItemTO.SalesWidth;
                if (tblProductItemTO.SalesLength > 0)
                    oitm.SalesUnitLength = tblProductItemTO.SalesLength;
                if (tblProductItemTO.SalesVolumeId > 0)
                    oitm.SalesVolumeUnit = tblProductItemTO.SalesVolumeId;

                #endregion

                #region Inventory Data

                oitm.GLMethod = Constants.GetGLMethodsEnum(tblProductItemTO.GLAccId);                                    //G/L method
                                                                                                                         //oitm.InventoryUOM = tblProductItemTO.InventUOMId.ToString();
                oitm.InventoryWeight = tblProductItemTO.InventWeight;

                
                //AmolG[2020-Dec-18]Shift This code Above
                //oitm.MinInventory = tblProductItemTO.InventMinimum;

                //oitm.InventoryUoMEntry = tblProductItemTO.ReqPurchaseUOMId;
                oitm.CostAccountingMethod = Constants.GetBoInventorySystemEnum(tblProductItemTO.ValuationId);
                #endregion

                #region Planning & Production Data

                oitm.PlanningSystem = Constants.GetPlanningSystemEnum(tblProductItemTO.PlanningId);
                oitm.ProcurementMethod = Constants.GetProcurementMethodEnum(tblProductItemTO.ProcurementId);
                if (oitm.ProcurementMethod == SAPbobsCOM.BoProcurementMethod.bom_Make)
                    oitm.ComponentWarehouse = Constants.GetMRPComponentWarehouseEnum(tblProductItemTO.CompWareHouseId);

                //AmolG[2020-Dec-18]Shift This code Above
                //oitm.MinOrderQuantity = tblProductItemTO.MinOrderQty;
                oitm.LeadTime = Convert.ToInt32(tblProductItemTO.LeadTime);
                oitm.ToleranceDays = Convert.ToInt32(tblProductItemTO.ToleranceDays);

                oitm.IssueMethod = Constants.GetSAPIssueMethodEnum(tblProductItemTO.IssueId);

                #endregion

                if (tblProductItemTO.MappedSapItemClassId > 0)
                {
                    oitm.set_Properties(tblProductItemTO.MappedSapItemClassId, SAPbobsCOM.BoYesNoEnum.tYES);
                }

                //reshma for fixed asset
                if (tblProductItemTO.IsFixedAsset)
                {
                    oitm.AssetClass = tblProductItemTO.MapSapAssetClassId;
                    oitm.Location = Convert.ToInt32(tblProductItemTO.MappedSapAssetLocationId.ToString());
                }


                int result = oitm.Add();
                if(result != 0)
                {
                    resultMessage.DefaultBehaviour();
                    string sapErrorMsg = Startup.CompanyObject.GetLastErrorDescription();
                    resultMessage.Text = sapErrorMsg;
                    resultMessage.DisplayMessage = sapErrorMsg;
                    return resultMessage;
                }
                SAPbobsCOM.Items oaddedItm;
                oaddedItm = Startup.CompanyObject.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems);
                oaddedItm.GetByKey(tblProductItemTO.IdProdItem.ToString());
                oaddedItm.PriceList.SetCurrentLine(0);
                oaddedItm.PriceList.Currency = "INR";
                if(priooneSuppTO != null && priooneSuppTO.SupplierOrgId.ToString() != "0")
                    oaddedItm.PriceList.Price = Convert.ToDouble(priooneSuppTO.BasicRate);
                else
                    oaddedItm.PriceList.Price = Convert.ToDouble(tblProductItemTO.BasePrice);
                result = oaddedItm.Update();
                if(result != 0)
                {
                    resultMessage.DefaultBehaviour();
                    string sapErrorMsg = Startup.CompanyObject.GetLastErrorDescription();
                    resultMessage.Text = sapErrorMsg;
                    resultMessage.DisplayMessage = sapErrorMsg;
                    return resultMessage;
                }
                //if (result == 0)
                //{
                //    //Update Unit Price
                //    Boolean updateUnitPrice = true;
                //    for (int pv = 0; pv < tblPurchaseItemMasterTOList.Count; pv++)
                //    {
                //        if (tblPurchaseItemMasterTOList[pv].SupplierOrgId.ToString() != "0")
                //        {
                //            if (oitm.Mainsupplier == tblPurchaseItemMasterTOList[pv].SupplierOrgId.ToString())
                //            {
                //                updateUnitPrice = false;
                //                SAPbobsCOM.Items oaddedItm;
                //                oaddedItm = Startup.CompanyObject.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems);
                //                oaddedItm.GetByKey(tblProductItemTO.IdProdItem.ToString());

                //                oaddedItm.PriceList.SetCurrentLine(0);
                //                oaddedItm.PriceList.Currency = "INR";
                //                oaddedItm.PriceList.Price = Convert.ToDouble(tblProductItemTO.BasePrice);
                //                result = oaddedItm.Update();
                //                if (result == 0)
                //                {

                //                }
                //                else
                //                {
                //                    resultMessage.DefaultBehaviour();
                //                    string sapErrorMsg = Startup.CompanyObject.GetLastErrorDescription();
                //                    resultMessage.Text = sapErrorMsg;
                //                    resultMessage.DisplayMessage = "Error while updating item price in SAP";
                //                }
                //            }
                //        }
                //    }
                //    if (updateUnitPrice)
                //    {
                //        SAPbobsCOM.Items oaddedItm;
                //        oaddedItm = Startup.CompanyObject.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems);
                //        oaddedItm.GetByKey(tblProductItemTO.IdProdItem.ToString());
                //        oaddedItm.PriceList.SetCurrentLine(0);
                //        oaddedItm.PriceList.Currency = "INR";
                //        oaddedItm.PriceList.Price = Convert.ToDouble(tblProductItemTO.BasePrice);
                //        result = oaddedItm.Update();

                //        if (result == 0)
                //        {

                //        }
                //        else
                //        {
                //            resultMessage.DefaultBehaviour();
                //            string sapErrorMsg = Startup.CompanyObject.GetLastErrorDescription();
                //            resultMessage.Text = sapErrorMsg;
                //            resultMessage.DisplayMessage = "Error while updating item price in SAP";
                //        }
                //    }

                //    resultMessage.DefaultSuccessBehaviour();
                //}
                //else
                //{
                //    resultMessage.DefaultBehaviour();
                //    string sapErrorMsg = Startup.CompanyObject.GetLastErrorDescription();
                //    resultMessage.Text = sapErrorMsg;
                //    //resultMessage.DisplayMessage = "Error while creating item in SAP";
                //    resultMessage.DisplayMessage = sapErrorMsg;
                //}
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.DefaultBehaviour();
                string sapErrorMsg = Startup.CompanyObject.GetLastErrorDescription();
                resultMessage.Text = sapErrorMsg + " " + ex.ToString();
                resultMessage.DisplayMessage = "Error while creating item in SAP with Exception";
                return resultMessage;

            }
            finally
            {
                //  oitm.Close();
            }
        }

        public int InsertTblProductItem(TblProductItemTO tblProductItemTO, SqlConnection conn, SqlTransaction tran)
        {
            int result = 0;
            if (tblProductItemTO.IsBaseItemForRate > 0)
            {
                result = _iTblProductItemDAO.updatePreviousBase(conn, tran);
            }
            if (result != 1)
            {
                return result;
            }
            if (tblProductItemTO.CodeTypeId == 2)
                tblProductItemTO.IsInventoryItem = 0;
            return _iTblProductItemDAO.InsertTblProductItem(tblProductItemTO, conn, tran);
        }

        //@  Hudekar Priyanka [01-march-2019]
        public int InsertTblPurchaseItemMaster(TblPurchaseItemMasterTO tblPurchaseItemMasterTO)
        {
            //   tblPurchaseItemMasterTO.prodItemId = _ProdItemId;
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                result = _iTblProductItemDAO.InsertTblPurchaseItemMaster(tblPurchaseItemMasterTO, conn, tran);
                //   result = TblProductItemPurchaseDAO.InsertTblPurchaseItemMasters(tblPurchaseItemMasterTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    return result;
                }

                // var res = _iTblProductItemDAO.InsertTblPurchaseItemMaster(tblPurchaseItemMasterTO, conn, tran);
                tran.Commit();
                return result;

            }
            catch (Exception e)
            {
                tran.Rollback();
                return result;
            }
        }
        public ResultMessage PostNewItemSupplier(List<TblPurchaseItemMasterTO> tblPurchaseItemMasterTOList)
        {
            ResultMessage resultMessage = new ResultMessage();
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;          
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                TblPurchaseItemMasterTO tblPurchaseItemMasterTO = new TblPurchaseItemMasterTO();
                TblProductItemTO tblProductItemTO = new TblProductItemTO();
                DropDownTO maxPrioritySupplierTO = new DropDownTO();
                Int32 result = 0;
                String ItemIdStr = string.Join(",", tblPurchaseItemMasterTOList.Select(d => d.ProdItemId.ToString()).ToArray());
                List<TblProductItemTO> tblProductItemTOList = _iTblProductItemDAO.GetProductItemDetailsForPurchaseItem(ItemIdStr, conn, tran);
                if(tblProductItemTOList == null || tblProductItemTOList.Count == 0)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour();
                    resultMessage.DisplayMessage = "Failed To Get Product Item Details - GetProductItemDetailsForPurchaseItem";
                    return resultMessage;
                }
                List<DropDownTO> maxPrioritySupplierTOList = _iTblProductItemDAO.GetMaxPriorityItemSupplier(ItemIdStr, conn, tran);
                if (maxPrioritySupplierTOList == null || maxPrioritySupplierTOList.Count == 0)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour();
                    resultMessage.DisplayMessage = "Failed To Get Max Priority Item Supplier List - GetMaxPriorityItemSupplier";
                    return resultMessage;
                }
                for (int i = 0; i < tblPurchaseItemMasterTOList.Count; i++)
                {
                    tblPurchaseItemMasterTO = null; result = 0; tblProductItemTO = null; maxPrioritySupplierTO = null;
                    tblPurchaseItemMasterTO = tblPurchaseItemMasterTOList[i];
                    tblProductItemTO = tblProductItemTOList.Where(x => x.IdProdItem == tblPurchaseItemMasterTOList[i].ProdItemId).FirstOrDefault();
                    if (tblProductItemTO == null)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour();
                        resultMessage.DisplayMessage = "Failed To Get Product Item Details - GetProductItemDetailsForPurchaseItem";
                        return resultMessage;
                    }
                    tblPurchaseItemMasterTO.PurchaseUOMId = tblProductItemTO.WeightMeasureUnitId;
                    maxPrioritySupplierTO = maxPrioritySupplierTOList.Where(x => x.Value == tblPurchaseItemMasterTOList[i].ProdItemId).FirstOrDefault();
                    if (maxPrioritySupplierTO == null)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour();
                        resultMessage.DisplayMessage = "Failed To Get Max Priority Item Supplier List - GetMaxPriorityItemSupplier";
                        return resultMessage;
                    }
                    tblPurchaseItemMasterTO.Priority = Convert.ToDecimal(maxPrioritySupplierTO.Text) + 1;
                    result = _iTblProductItemDAO.InsertTblPurchaseItemMaster(tblPurchaseItemMasterTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour();
                        resultMessage.DisplayMessage = "Error while InsertTblPurchaseItemMaster";
                        return resultMessage;
                    }
                    TblConfigParamsTO tblConfigParamsTOSAPService = _iTblConfigParamsDAO.SelectTblConfigParams(Constants.SAPB1_SERVICES_ENABLE);
                    if (tblConfigParamsTOSAPService != null)
                    {
                        if (Convert.ToInt32(tblConfigParamsTOSAPService.ConfigParamVal) == 1)
                        {
                            SAPbobsCOM.Items oaddedItm;
                            oaddedItm = Startup.CompanyObject.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems);
                            oaddedItm.GetByKey(tblProductItemTO.IdProdItem.ToString());

                            oaddedItm.PreferredVendors.SetCurrentLine(oaddedItm.PreferredVendors.Count);
                            oaddedItm.PreferredVendors.BPCode = tblPurchaseItemMasterTO.SupplierOrgId.ToString();
                            oaddedItm.PreferredVendors.Add();
                            result = oaddedItm.Update();
                            if (result != 0)
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour();
                                resultMessage.DisplayMessage = Startup.CompanyObject.GetLastErrorDescription();
                                return resultMessage;
                            }
                        }
                    }
                }
                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "PostNewItemSupplier at BL");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }
        public TblProductItemTO ValidateItemData(TblProductItemTO tblProductItemTO)
        {
            if (tblProductItemTO.LocationId != 0 && tblProductItemTO.ItemBrandId != 0 && tblProductItemTO.ItemMakeId != 0
                && tblProductItemTO.MaterialTypeId != 0 && tblProductItemTO.InventUOMId != 0
                && tblProductItemTO.IssueId != 0 && tblProductItemTO.SalesUOMId != 0
            )
            {
                if(tblProductItemTO.ManageItemById == (Int32)Constants.ManageItemByIdE.SERIAL_NO)
                {
                    if(tblProductItemTO.WarrantyTemplateId != 0 && tblProductItemTO.MgmtMethodId != 0)
                    {
                        tblProductItemTO.IsProperSAPItem = 1;
                    }
                }
                else
                {
                    tblProductItemTO.IsProperSAPItem = 1;
                }
            }
            return tblProductItemTO;
        }
        public ResultMessage UpdateInvalidItems(Int32 itemProdCatId)
        {
            ResultMessage resultMessage = new ResultMessage();
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            Boolean SAPServiceEnable = false;
            try
            {
                List<DropDownTO> prodClassificationIdList = _iTblProdClassificationBL.getProdClassIdsByItemProdCat(itemProdCatId);
                if (prodClassificationIdList == null || prodClassificationIdList.Count == 0)
                {
                    resultMessage.DefaultBehaviour("Invalid itemProdCatId");
                }
                TblConfigParamsTO tblConfigParamsTOSAPService = _iTblConfigParamsDAO.SelectTblConfigParams(Constants.SAPB1_SERVICES_ENABLE);
                if (tblConfigParamsTOSAPService != null)
                {
                    if (Convert.ToInt32(tblConfigParamsTOSAPService.ConfigParamVal) == 1)
                    {
                        SAPServiceEnable = true;
                    }
                }
                String prodClassIds = string.Join(",", prodClassificationIdList.Select(s => s.Value.ToString()).ToArray());
                List<TblProductItemTO> TblProductItemTOList = new List<TblProductItemTO>();
                TblProductItemTOList = _iTblProductItemDAO.SelectListOfProductItemTOOnprdClassIds(prodClassIds);
                if (TblProductItemTOList == null || prodClassificationIdList.Count == 0)
                {
                    resultMessage.DefaultBehaviour("Item Not Found");
                }
                TblProductItemTOList = TblProductItemTOList.Where(w => w.IsProperSAPItem != 1).ToList();
                for (int i = 0; i < TblProductItemTOList.Count; i++)
                {
                    conn.Open();
                    tran = conn.BeginTransaction();
                    TblProductItemTO tblProductItemTO = new TblProductItemTO();
                    tblProductItemTO = TblProductItemTOList[i];
                    tblProductItemTO = ValidateItemData(tblProductItemTO);
                    if(tblProductItemTO.GstCodeId == 0)
                    {
                        switch (tblProductItemTO.SpecificationID)
                        {
                            case 2133:
                                tblProductItemTO.GstCodeId = 8830;
                                tblProductItemTO.HSNCode = 72029911;
                                tblProductItemTO.SapHSNCode = 8822;
                                break;
                            case 95:
                                tblProductItemTO.GstCodeId = 11214;
                                tblProductItemTO.HSNCode = 84821020;
                                tblProductItemTO.SapHSNCode = 11195;
                                break;
                            case 2132:
                                tblProductItemTO.GstCodeId = 11068;
                                tblProductItemTO.HSNCode = 84713090;
                                tblProductItemTO.SapHSNCode = 11049;
                                break;
                            case 94:
                                tblProductItemTO.GstCodeId = 11860;
                                tblProductItemTO.HSNCode = 85362030;
                                tblProductItemTO.SapHSNCode = 11840;
                                break;
                            case 93:
                                tblProductItemTO.GstCodeId = 10111;
                                tblProductItemTO.HSNCode = 83026000;
                                tblProductItemTO.SapHSNCode = 10096;
                                break;
                            case 2126:
                                tblProductItemTO.GstCodeId = 11208;
                                tblProductItemTO.HSNCode = 84818090;
                                tblProductItemTO.SapHSNCode = 11189;
                                break;
                            case 2127:
                                tblProductItemTO.GstCodeId = 8816;
                                tblProductItemTO.HSNCode = 72021100;
                                tblProductItemTO.SapHSNCode = 8809;
                                break;
                            default:
                                tblProductItemTO.GstCodeId = 766;
                                tblProductItemTO.HSNCode = 03031300;
                                tblProductItemTO.SapHSNCode = 763;
                                break;
                        }
                    }
                    List<DropDownTO> maxPrioritySupplierTOList = _iTblProductItemDAO.GetMaxPriorityItemSupplier(tblProductItemTO.IdProdItem.ToString(), conn, tran);
                    if (maxPrioritySupplierTOList == null || maxPrioritySupplierTOList.Count == 0)
                    {
                        tblProductItemTO.IsProperSAPItem = 0;
                    }
                    tblProductItemTO.UpdatedBy = 1;
                    tblProductItemTO.UpdatedOn = _iCommon.ServerDateTime;
                    result = _iTblProductItemDAO.UpdateTblProductItem(tblProductItemTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour();
                        resultMessage.DisplayMessage = "Error while UpdateTblProductItem";
                        return resultMessage;
                    }
                    if (SAPServiceEnable == true)
                    {
                        SAPbobsCOM.Items oaddedItm;
                        oaddedItm = Startup.CompanyObject.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems);
                        oaddedItm.GetByKey(tblProductItemTO.IdProdItem.ToString());
                        if(tblProductItemTO.SapHSNCode > 0 && tblProductItemTO.SapHSNCode != null)
                        {
                            oaddedItm.ChapterID = Convert.ToInt32(tblProductItemTO.SapHSNCode);
                        }
                        result = oaddedItm.Update();
                        if (result != 0)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour();
                            string sapErrorMsg = Startup.CompanyObject.GetLastErrorDescription();
                            resultMessage.Text = sapErrorMsg;
                            resultMessage.DisplayMessage = sapErrorMsg;
                            return resultMessage;
                        }
                    }
                    tran.Commit();
                    conn.Close();
                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "InsertTblProductItem at BL");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        
        public ResultMessage InsertTblProductItemBom(List<TblProductItemBomTO> tblProductItemBomTOList)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                //[18-03-2021] Deepali added for revision and History purpose 
                TblModelTO tblModelTO = new TblModelTO();  
                List<TblModelTO> tblModelTOList = new List<TblModelTO>();
                tblModelTOList = _iTblModelBL.SelectAllTblModelList(tblProductItemBomTOList[0].ParentProdItemId, conn, tran);               
                if (tblModelTOList != null && tblModelTOList.Count > 0)
                {
                    tblModelTO = tblModelTOList.Where(w => w.VersionNo == -1).FirstOrDefault();
                }
                else
                {
                    tblModelTO.CreatedBy = tblProductItemBomTOList[0].CreatedBy;
                    tblModelTO.FinalizedBy = tblProductItemBomTOList[0].CreatedBy;
                    tblModelTO.CreatedOn = tblProductItemBomTOList[0].UpdatedOn;
                    tblModelTO.FinalizedOn = tblProductItemBomTOList[0].UpdatedOn;
                    tblModelTO.ProdItemId = tblProductItemBomTOList[0].ParentProdItemId;
                    tblModelTO.VersionNo = -1;
                    result = _iTblModelBL.InsertTblModel(tblModelTO, conn, tran);
                    if (result <= 0)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Record Saved Failed - InsertTblModel");
                        return resultMessage;
                    }
                }
                
                for (var i = 0; i < tblProductItemBomTOList.Count; i++)
                {
                    tblProductItemBomTOList[i].CreatedOn = _iCommon.ServerDateTime;
                    tblProductItemBomTOList[i].ModelId = tblModelTO.IdModel;
                    //Reshma Added Existing BOM List OF parent Item Id
                    List<TblProductItemBomTO> tblProductItemBomTOTempList = _iTblProductItemDAO.GetItemBOMList(tblProductItemBomTOList[i].ParentProdItemId , conn, tran);
                    if(tblProductItemBomTOTempList !=null && tblProductItemBomTOTempList.Count >0)
                    {
                        List<TblProductItemBomTO> tblProductItemBomTOLocalList = tblProductItemBomTOTempList.Where(w => w.ChildProdItemId == tblProductItemBomTOList[i].ChildProdItemId).ToList();
                        if(tblProductItemBomTOLocalList !=null && tblProductItemBomTOLocalList.Count >0)
                        {
                            tran.Rollback();
                            resultMessage.Result = 0;
                            resultMessage.DisplayMessage = "Duplicate child Item "+ tblProductItemBomTOLocalList[0].ChildProdItemId + " can not be added into BOM" + tblProductItemBomTOLocalList[0].ParentProdItemId;
                            resultMessage.DefaultBehaviour("Duplicate Item can not be added into BOM");
                            resultMessage.DisplayMessage= "Duplicate child Item " + tblProductItemBomTOLocalList[0].ChildProdItemId + "  can not be added into BOM  " + tblProductItemBomTOLocalList[0].ParentProdItemId;
                            return resultMessage;
                        }
                    }
                    //Deepali Added to check if items are interlinked
                    List<TblProductItemBomTO> tblProductItemBomTOTempListChild = _iTblProductItemDAO.GetItemBOMList(tblProductItemBomTOList[i].ChildProdItemId, conn, tran);
                    if (tblProductItemBomTOTempListChild != null && tblProductItemBomTOTempListChild.Count > 0)
                    {
                        List<TblProductItemBomTO> tblProductItemBomTOLocalList = tblProductItemBomTOTempListChild.Where(w => w.ChildProdItemId == tblProductItemBomTOList[i].ParentProdItemId).ToList();
                        if (tblProductItemBomTOLocalList != null && tblProductItemBomTOLocalList.Count > 0)
                        {
                            tran.Rollback();
                            resultMessage.Result = 0;
                            resultMessage.DisplayMessage = "Child Item " + tblProductItemBomTOLocalList[0].ChildProdItemId + " can not be added into BOM" + tblProductItemBomTOLocalList[0].ParentProdItemId;
                            resultMessage.DefaultBehaviour("Child Item can not be added into BOM");
                            resultMessage.DisplayMessage = "Parent Item " + tblProductItemBomTOLocalList[0].ChildProdItemId + "  already added  as Child Item into BOM  " + tblProductItemBomTOLocalList[0].ParentProdItemId;
                            return resultMessage;
                        }
                    }
                    result = _iTblProductItemDAO.InsertTblProductItemBom(tblProductItemBomTOList[i], conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Record Saved Failed - InsertTblProductItemBom");
                        return resultMessage;
                    }

                }
                tblProductItemBomTOList[0].UpdatedOn = _iCommon.ServerDateTime;
          
                result = _iTblProductItemDAO.UpdateStatusTblProductItem(tblProductItemBomTOList[0], conn, tran);
                if (result == -1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Record Saved Failed - UpdateStatusTblProductItem");
                    return resultMessage;
                }                           

                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "Exception Error While Record Save : AddFavouriteOrder");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }
        
        public ResultMessage UpdateTblProductItemBom(List<TblProductItemBomTO> tblProductItemBomTOList)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                for (var i = 0; i < tblProductItemBomTOList.Count; i++)
                {
                    tblProductItemBomTOList[i].UpdatedOn = _iCommon.ServerDateTime;

                    result = _iTblProductItemDAO.UpdateTblProductItemBom(tblProductItemBomTOList[i], conn, tran);
                    if (result == -1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Record Saved Failed - UpdateTblProductItemBom");
                        return resultMessage;
                    }
                }
                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "Exception Error While Record Save : AddFavouriteOrder");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        public ResultMessage DeleteTblProductItemBom(TblProductItemBomTO tblProductItemBomTO)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                result = _iTblProductItemDAO.DeleteTblProductItemBom(tblProductItemBomTO, conn, tran);
                if (result == -1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Record Saved Failed - DeleteTblProductItemBom");
                    return resultMessage;
                }

                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "Exception Error While Record Save : AddFavouriteOrder");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }

        }

        public List<TblProductItemBomTO > GetBOMData (Int32 parentItemId)
        {
            return _iTblProductItemDAO.GetItemBOMList(parentItemId);
        }
        public ResultMessage FinalizedProductItemBOM(TblProductItemTO finalizeItemBomTO)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                finalizeItemBomTO.UpdatedOn = _iCommon.ServerDateTime;

                result = _iTblProductItemDAO.FinalizeTblProductItem(finalizeItemBomTO, conn, tran);
                if (result == -1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Record Saved Failed - FinalizeTblProductItem");
                    return resultMessage;
                }              

                string IdProdItemStr = finalizeItemBomTO.IdProdItem.ToString();
                List<TblProductItemTO> TblProductItemBomTOList = _iTblProductItemDAO.GetMakeItemBOMList(IdProdItemStr);
                
                //Reshma
                if (finalizeItemBomTO.BomTypeId == (int)Constants.dimBOMTypeE.Standard_BOM || finalizeItemBomTO.BomTypeId == (int)Constants.dimBOMTypeE.Dismantal_BOM)
                {
                    resultMessage = SaveBOMItemList(finalizeItemBomTO.ParentProdItemId);
                    if (resultMessage.Result != 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Record Saved Failed - FinalizeTblProductItem");
                        resultMessage.Text = "Record Not Saved";
                        return resultMessage;
                    }
                }
                #region old code
                //if (finalizeItemBomTO.BomTypeId == (int)Constants.dimBOMTypeE.Dismantal_BOM)
                //{
                //    SAPbobsCOM.Recordset oRsGI = null;
                //    SAPbobsCOM.Recordset oRsBatch = null;

                //    oRsBatch = (SAPbobsCOM.Recordset)Startup.CompanyObject.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                //    oRsGI = (SAPbobsCOM.Recordset)Startup.CompanyObject.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                //    SAPbobsCOM.ProductTrees IssueToProd = null;
                //    IssueToProd = (SAPbobsCOM.ProductTrees)(Startup.CompanyObject.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oProductTrees));

                //    IssueToProd.Quantity = 1;
                //    IssueToProd.ProductDescription = finalizeItemBomTO.ItemName;// Item Name
                //    IssueToProd.TreeType = BoItemTreeTypes.iProductionTree; 
                //    //IssueToProd.TreeCode = "3609";
                //    IssueToProd.TreeCode = finalizeItemBomTO.IdProdItem.ToString();// Model Item Code
                //    IssueToProd.Warehouse = finalizeItemBomTO.LocationId.ToString();// Warehouse Id
                //    //IssueToProd.Project = "2";//Here link the Project Name if the Project Has Defined (WO) "20-4101"
                //    if (TblProductItemBomTOList != null && TblProductItemBomTOList.Count > 0)
                //    {
                //        for(var i = 0; i< TblProductItemBomTOList.Count; i++)
                //        {
                //            IssueToProd.Items.SetCurrentLine(i);
                //            IssueToProd.Items.ItemCode = TblProductItemBomTOList[i].IdProdItem.ToString();
                //            IssueToProd.Items.ItemType = ProductionItemType.pit_Item;
                //            IssueToProd.Items.IssueMethod = Constants.GetSAPIssueMethodEnum(TblProductItemBomTOList[i].IssueId);
                //            //IssueToProd.Items.AdditionalQuantity = 2;
                //            IssueToProd.Items.Warehouse = TblProductItemBomTOList[i].LocationId.ToString();
                //            IssueToProd.Items.Quantity = (double)TblProductItemBomTOList[i].Qty;
                //            IssueToProd.Items.Add();
                //        }
                //    }
                //    result = IssueToProd.Add();
                //    if (result != 0)
                //    {
                //        tran.Rollback();
                //        resultMessage.DefaultBehaviour("Record Saved Failed - FinalizeTblProductItem");
                //        return resultMessage;
                //    }
                //}
                #endregion
                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
               
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "Exception Error While Record Save : FinalizeTblProductItem");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }
        
        public ResultMessage RevisedProductItemBOM(TblProductItemTO tblProductItemTO)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            Int32 newProdItemId = 0;
            try
            {
                List<TblPurchaseItemMasterTO> tblPurchaseItemMasterTOList = _iTblProductItemDAO.SelectAllTblPurchaseItemMasterTOList(tblProductItemTO.IdProdItem);
                conn.Open();
                tran = conn.BeginTransaction();
                TblProductItemTO tblProductItemTOLocal = SelectTblProductItemTO(tblProductItemTO.IdProdItem, conn, tran);
                tblProductItemTO.BaseProdItemId = tblProductItemTOLocal.BaseProdItemId;
                tblProductItemTO.IsActive = 0;
                result = _iTblProductItemDAO.UpdateIsHavingNewRevTblProductItem(tblProductItemTO.IdProdItem, tblProductItemTO.UpdatedBy, conn, tran);
                resultMessage = CopyPastMakeItem(tblProductItemTO.IdProdItem, tblProductItemTO.BaseProdItemId, tblProductItemTO.CreatedBy,conn,tran, ref newProdItemId);
                List<TblProductItemBomTO> TblProductItemBomTOList = _iTblProductItemDAO.GetItemBOMList(tblProductItemTO.IdProdItem);
                if(newProdItemId >0)
                {
                    TblProductItemTO tblProductItemTOTemp = SelectTblProductItemTO(newProdItemId, conn, tran);
                    tblProductItemTOTemp.Status = tblProductItemTO.Status;
                    tblProductItemTOTemp.UpdatedOn = _iCommon.ServerDateTime;
                    tblProductItemTOTemp.UpdatedBy = tblProductItemTO.UpdatedBy;
                    result = _iTblProductItemDAO.UpdateStatusTblProductItem(tblProductItemTOTemp,conn,tran);
                    if (result == -1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Record BOM Status Update Failed - RevisedProductItemBOM");
                        return resultMessage;
                    }
                    if (tblPurchaseItemMasterTOList.Count > 0)
                    {
                        for (int i = 0; i < tblPurchaseItemMasterTOList.Count; i++)
                        {
                            tblPurchaseItemMasterTOList[i].ProdItemId = newProdItemId;
                            tblPurchaseItemMasterTOList[i].CreatedBy = tblProductItemTO.UpdatedBy;
                            tblPurchaseItemMasterTOList[i].CreatedOn = _iCommon.ServerDateTime;
                            tblPurchaseItemMasterTOList[i].IsActive =1;
                            result = _iTblProductItemDAO.InsertTblPurchaseItemMaster(tblPurchaseItemMasterTOList[i], conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour();
                                resultMessage.DisplayMessage = "Error while InsertTblPurchaseItemMaster - RevisedProductItemBOM";
                                return resultMessage;
                            }
                        }
                    }
                }
                if (TblProductItemBomTOList != null && TblProductItemBomTOList.Count > 0)
                {
                    for (var i = 0; i < TblProductItemBomTOList.Count; i++)
                    {
                        if (newProdItemId > 0)
                        {
                            TblProductItemBomTOList[i].ParentProdItemId = newProdItemId;
                            TblProductItemBomTOList[i].CreatedBy = tblProductItemTO.UpdatedBy;
                            TblProductItemBomTOList[i].CreatedOn = _iCommon.ServerDateTime;
                            result = _iTblProductItemDAO.InsertTblProductItemBom(TblProductItemBomTOList[i], conn, tran);

                            if (result == -1)
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour("Record Insert BOM Saved Failed - RevisedProductItemBOM");
                                return resultMessage;
                            }
                        }
                    }
                }
                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "Exception Error While Record Save : FinalizeTblProductItem");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        public ResultMessage RevisedProductItemBOMV2(TblProductItemTO tblProductItemTO)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                tblProductItemTO.Status = (int)Constants.bomStatusEnum.Pending;
                tblProductItemTO.UpdatedOn = _iCommon.ServerDateTime;
                result = _iTblProductItemDAO.UpdateStatusTblProductItem(tblProductItemTO, conn, tran);
                if (result <= 0)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Record BOM Status Update Failed - RevisedProductItemBOM");
                    return resultMessage;
                }
                                
                //Deepali
                List<TblModelTO> tblModelTOList =_iTblModelBL.SelectAllTblModelList(tblProductItemTO.IdProdItem, conn, tran);
                if (tblModelTOList != null && tblModelTOList.Count > 0)
                {
                    for (int model = 0; model < tblModelTOList.Count; model++)
                    {
                        TblModelTO tblModelTOtemp = tblModelTOList[model];
                        tblModelTOtemp.VersionNo = (tblModelTOtemp.VersionNo + 1);
                        result = _iTblModelBL.UpdateTblModel(tblModelTOtemp, conn, tran);
                        if (result <= 0)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Record Saved Failed - FinalizeTblProductItem");
                            return resultMessage;
                        }

                    }
                }

                TblModelTO tblModelTO = new TblModelTO();
                tblModelTO.CreatedBy = tblProductItemTO.CreatedBy;
                tblModelTO.FinalizedBy = tblProductItemTO.CreatedBy;
                tblModelTO.CreatedOn = tblProductItemTO.UpdatedOn;
                tblModelTO.FinalizedOn = tblProductItemTO.UpdatedOn;
                tblModelTO.ProdItemId = tblProductItemTO.IdProdItem;
                tblModelTO.VersionNo = -1;

                result = _iTblModelBL.InsertTblModel(tblModelTO, conn, tran);
                if (result <= 0)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Record Saved Failed - FinalizeTblProductItem");
                    return resultMessage;
                }
                List<TblProductItemBomTO> TblProductItemBomTOList = _iTblProductItemDAO.GetItemBOMList(tblProductItemTO.IdProdItem);

                if (TblProductItemBomTOList != null && TblProductItemBomTOList.Count > 0)
                {
                    for (var i = 0; i < TblProductItemBomTOList.Count; i++)
                    {
                        TblProductItemBomTOList[i].CreatedBy = tblProductItemTO.UpdatedBy;
                        TblProductItemBomTOList[i].CreatedOn = _iCommon.ServerDateTime;
                        TblProductItemBomTOList[i].ModelId = tblModelTO.IdModel;
                        result = _iTblProductItemDAO.InsertTblProductItemBom(TblProductItemBomTOList[i], conn, tran);
                        if (result == -1)
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour("Record Insert BOM Saved Failed - RevisedProductItemBOM");
                                return resultMessage;
                            }
                       
                    }
                }
                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "Exception Error While Record Save : FinalizeTblProductItem");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        public ResultMessage PostDeactivateNonListedItems()
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                int deactivationTime = 0;
                TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.DEFAULT_TIME_TO_DEACTIVATE_NON_LISTED_ITEMS);
                if (tblConfigParamsTO != null)
                {
                    deactivationTime =Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);
                }
                if(deactivationTime > 0)
                {
                    List<TblProductItemTO> tblProductItemTOList = SelectAllTblProductNonListedItemList(deactivationTime, conn, tran);
                    if(tblProductItemTOList != null && tblProductItemTOList.Count>0)
                    {
                        for (int i = 0; i < tblProductItemTOList.Count; i++)
                        {
                            result = _iTblProductItemDAO.DeactivateTblProdctItemMaster(tblProductItemTOList[i].IdProdItem, conn, tran);
                            if(result <=0)
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour("Record Item update Failed - DeactivateTblProdctItemMaster");
                                return resultMessage;
                            }
                        }
                    }
                }

                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "Exception Error While Record Save : PostDeactivateNonListedItems");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        public List<DropDownTO> getCountOfListedAndNonListedItems()
        {
            List<DropDownTO> list = new List<DropDownTO>();

            DropDownTO dropDownTO = _iTblProductItemDAO.getCountOfListedAndNonListedItems(1);
            if(dropDownTO != null)
            {
                dropDownTO.Text = "Non Listed Items";
                list.Add(dropDownTO);
            }
            dropDownTO = _iTblProductItemDAO.getCountOfListedAndNonListedItems(0);
            if (dropDownTO != null)
            {
                dropDownTO.Text = "Listed Items";
                list.Add(dropDownTO);
            }
            return list;
        }
        public List<TblProductItemTO> SelectAllTblProductNonListedItemList(int deactivationTime, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblProductItemDAO.SelectAllTblProductNonListedItemList(deactivationTime,conn,tran);            
        }



        #endregion

        #region Updation

        public ResultMessage UpdateTblProductItem(TblProductItemTO tblProductItemTO, List<TblPurchaseItemMasterTO> tblPurchaseItemMasterTOList, List<WareHouseWiseItemDtlsTO> wareHouseWiseItemDtlsTOList, List<StoresLocationTO> StoresLocationTOList, List<TblProdItemMakeBrandExtTO> tblProdItemMakeBrandExtTOList=null)
        {
            ResultMessage resultMessage = new ResultMessage();
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            SqlConnection sapConn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.SAP_CONNECTION_STRING));
            SqlTransaction saptTran = null;
            int result = 0;
            int res = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                Boolean SAPServiceEnable = false;
                TblConfigParamsTO tblConfigParamsTOSAPService = _iTblConfigParamsDAO.SelectTblConfigParams(Constants.SAPB1_SERVICES_ENABLE);
                if (tblConfigParamsTOSAPService != null)
                {
                    if (Convert.ToInt32(tblConfigParamsTOSAPService.ConfigParamVal) == 1)
                    {
                        SAPServiceEnable = true;
                        sapConn.Open();
                        saptTran = sapConn.BeginTransaction();
                    }
                }
                #region Check Make Item Alredy Added Or Not Of Same itemMakeId & itemBrandId
                if (tblProductItemTO.BaseProdItemId != 0)
                {
                    List<TblProductItemTO> TblProductItemTOList = _iTblProductItemDAO.checkMakeItemAlreadyExists(Convert.ToInt32(tblProductItemTO.BaseProdItemId), tblProductItemTO.ItemMakeId, tblProductItemTO.ItemBrandId, tblProductItemTO.IdProdItem, conn, tran);
                    if (TblProductItemTOList != null && TblProductItemTOList.Count > 0)
                    {
                        resultMessage.DefaultBehaviour();
                        resultMessage.DisplayMessage = "Make Item already added with make item id " + TblProductItemTOList[0].IdProdItem;
                        resultMessage.Result = 2;
                        return resultMessage;
                    }
                }
                #endregion
                #region Check Base Item Alredy Added Or Not 
                if (tblProductItemTO.BaseProdItemId == 0)
                {
                    List<TblProductItemTO> TblProductItemTOList = _iTblProductItemDAO.checkBaseItemAlreadyExists(tblProductItemTO.IdProdItem,tblProductItemTO.ProdClassId, tblProductItemTO.ItemName, conn, tran);
                    if (TblProductItemTOList != null && TblProductItemTOList.Count > 0)
                    {
                        resultMessage.DefaultBehaviour();
                        resultMessage.DisplayMessage = "Base Item already added with base item id" + TblProductItemTOList[0].IdProdItem;
                        resultMessage.Result = 2;
                        return resultMessage;
                    }
                }
                #endregion
                if (tblProductItemTO.IsBaseItemForRate > 0)
                {
                    result = _iTblProductItemDAO.updatePreviousBase(conn, tran);
                    if (result == -1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour();
                        resultMessage.DisplayMessage = "Error while updatePreviousBase";
                        return resultMessage;
                    }
                }

                if (tblProductItemTO.IdProdItem != 0)
                {
                    //Deepali Added[26-04-2021] task no 1030
                    TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.IS_SHOW_UOM_AND_COUM_DDL);
                    int isShowConvUOM = 0;
                    if (tblConfigParamsTO != null)
                    {
                        isShowConvUOM = Convert.ToInt16(tblConfigParamsTO.ConfigParamVal);
                    }
                    if (isShowConvUOM == 1)
                    {
                        DimUomGroupTO dimUomGroupTO = new DimUomGroupTO();

                        dimUomGroupTO = _iDimUomGroupDAO.SelectDimUomGroup(tblProductItemTO.UOMGroupId);
                        if (dimUomGroupTO == null || dimUomGroupTO.BaseUomId != tblProductItemTO.WeightMeasureUnitId || dimUomGroupTO.ConversionUnitOfMeasure != tblProductItemTO.ConversionUnitOfMeasure || dimUomGroupTO.ConversionFactor != tblProductItemTO.ConversionFactor)
                        {
                            dimUomGroupTO = getUOMGroupIdFromUOMAndConversionUOM(tblProductItemTO.WeightMeasureUnitId, tblProductItemTO.ConversionUnitOfMeasure, tblProductItemTO.ConversionFactor, conn, tran);
                            if (dimUomGroupTO != null)
                            {
                                tblProductItemTO.MappedUOMGroupId = dimUomGroupTO.MappedUomGroupId;
                                tblProductItemTO.UOMGroupId = dimUomGroupTO.IdUomGroup;
                            }
                            else
                            {
                                dimUomGroupTO = getUOMGroupIdFromUOMAndConversionUOM(tblProductItemTO.WeightMeasureUnitId, 0, 0, conn, tran);
                                if (dimUomGroupTO != null)
                                {
                                    dimUomGroupTO.ConversionUnitOfMeasure = tblProductItemTO.ConversionUnitOfMeasure;
                                    dimUomGroupTO.ConversionFactor = tblProductItemTO.ConversionFactor;
                                }
                                else
                                {
                                    dimUomGroupTO = new DimUomGroupTO();
                                    dimUomGroupTO.BaseUomId = tblProductItemTO.WeightMeasureUnitId;
                                    dimUomGroupTO.ConversionUnitOfMeasure = tblProductItemTO.ConversionUnitOfMeasure;
                                    dimUomGroupTO.ConversionFactor = tblProductItemTO.ConversionFactor;
                                    List<DropDownTO> UnitMeasureList = _iDimUnitMeasuresDAO.SelectAllUnitMeasuresForDropDown();
                                    if (UnitMeasureList != null && UnitMeasureList.Count > 0)
                                    {
                                        DropDownTO dropDownTOUom = UnitMeasureList.Where(w => w.Value == tblProductItemTO.WeightMeasureUnitId).FirstOrDefault();
                                        DropDownTO dropDownTOCUom = UnitMeasureList.Where(w => w.Value == tblProductItemTO.ConversionUnitOfMeasure).FirstOrDefault();

                                        if (dropDownTOUom != null && dropDownTOCUom != null)
                                        {
                                            dimUomGroupTO.UomGroupName = dropDownTOUom.Text + "_" + dropDownTOCUom.Text + "_" + tblProductItemTO.ConversionFactor;
                                            dimUomGroupTO.UomGroupCode = dropDownTOUom.Text + "_" + dropDownTOCUom.Text + "_" + tblProductItemTO.ConversionFactor;
                                        }
                                    }
                                    dimUomGroupTO.CreatedBy = tblProductItemTO.CreatedBy;
                                    dimUomGroupTO.CreatedOn = tblProductItemTO.CreatedOn;
                                }

                                resultMessage = AddUOMGroupV2FromMaster(dimUomGroupTO, SAPServiceEnable, conn, sapConn, tran, saptTran);
                                if (resultMessage.MessageType != ResultMessageE.Information)
                                {
                                    tran.Rollback();
                                    if (SAPServiceEnable)
                                        saptTran.Rollback();
                                    return resultMessage;
                                }
                                else
                                {
                                    if (SAPServiceEnable)
                                    {
                                        saptTran.Commit();
                                        sapConn.Close();
                                    }
                                }
                            }
                            dimUomGroupTO = getUOMGroupIdFromUOMAndConversionUOM(tblProductItemTO.WeightMeasureUnitId, tblProductItemTO.ConversionUnitOfMeasure, tblProductItemTO.ConversionFactor, conn, tran);
                            if (dimUomGroupTO != null)
                            {
                                tblProductItemTO.MappedUOMGroupId = dimUomGroupTO.MappedUomGroupId;
                                tblProductItemTO.UOMGroupId = dimUomGroupTO.IdUomGroup;
                            }
                        }
                    }

                    //resultMessage = AddUOMGroupV2(tblProductItemTO, SAPServiceEnable, conn, sapConn, tran, saptTran);
                    //if (resultMessage.MessageType != ResultMessageE.Information)
                    //{
                    //    tran.Rollback();
                    //    if (SAPServiceEnable)
                    //        saptTran.Rollback();
                    //    return resultMessage;
                    //}
                    //else
                    //{
                    //    if (SAPServiceEnable)
                    //    {
                    //        saptTran.Commit();
                    //        sapConn.Close();
                    //    }
                    //}
                    //Reshma Added For BOM Updation
                    TblProductItemTO tblProductItemTOLocal = _iTblProductItemDAO.SelectTblProductItem(tblProductItemTO.IdProdItem, conn, tran);
                    if (tblProductItemTOLocal.BomTypeId != tblProductItemTO.BomTypeId)
                    {
                        List<TblProductItemBomTO> tblProductItemBomTOList = _iTblProductItemDAO.GetItemBOMList(tblProductItemTO.IdProdItem, conn, tran);
                        if (tblProductItemBomTOList != null && tblProductItemBomTOList.Count > 0)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour();
                            resultMessage.DisplayMessage = "Error while UpdateTblProductItem";
                            resultMessage.Text = "Item No " + tblProductItemTO.IdProdItem + " has already define BOM so we can not update BOM type";
                            return resultMessage;
                        }
                    }
                    if(tblProductItemTO.IsConvertNonListedToListed || tblProductItemTOLocal.IsNonListed ==false)
                    {
                        if(tblProductItemTOLocal.IsNonListed ==false && tblProductItemTO.IsConvertNonListedToListed==true)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour();
                            resultMessage.DisplayMessage = "Error while UpdateTblProductItem";
                            resultMessage.Text = "Item No " + tblProductItemTO.IdProdItem + " is already converted from non listed to listed.";
                            return resultMessage;
                        }
                        else
                        {
                            tblProductItemTO.IsNonListed = false;
                        }
                    }
                    
                    //AmolG[2020-Dec-18] If the values not comes from UI then do sumation here
                    if (tblProductItemTO.IsManageInventory)
                    {
                        if (wareHouseWiseItemDtlsTOList != null && wareHouseWiseItemDtlsTOList.Count > 0)
                        {
                            tblProductItemTO.InventMinimum = 0;
                            for (int iCnt = 0; iCnt < wareHouseWiseItemDtlsTOList.Count; iCnt++)
                            {
                                if (!String.IsNullOrEmpty(wareHouseWiseItemDtlsTOList[iCnt].WhsCode))
                                {
                                    tblProductItemTO.InventMinimum += wareHouseWiseItemDtlsTOList[iCnt].MinInventory;
                                    //tblProductItemTO.MinOrderQty += wareHouseWiseItemDtlsTOList[iCnt].MinOrder;
                                }
                            }
                        }
                    }
                    if (tblProductItemTO.CodeTypeId == 2)
                        tblProductItemTO.IsInventoryItem = 0;
                    res = _iTblProductItemDAO.UpdateTblProductItem(tblProductItemTO, conn, tran);
                    result = res;
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour();
                        resultMessage.DisplayMessage = "Error while UpdateTblProductItem";
                        return resultMessage;
                    }


                    // Add By Samadhan 11 May 2022
                    if (result == 1)
                    {
                        //StoresLocationTOList
                        if (StoresLocationTOList != null && StoresLocationTOList.Count > 0)
                        {
                          
                            res = _iTblProductItemDAO.UpdateTblItemLinkedStoreLoc(tblProductItemTO.IdProdItem, conn, tran);
                            result = res;
                           

                            // Insert
                            for (int i = 0; i < StoresLocationTOList.Count; i++)
                            {

                                StoresLocationTOList[i].ProdItemId = tblProductItemTO.IdProdItem;
                                StoresLocationTOList[i].IsActive = 1;
                                result = _iTblProductItemDAO.InsertTblItemLinkedStoreLoc(StoresLocationTOList[i], conn, tran);
                                if (result != 1)
                                {
                                    tran.Rollback();
                                    resultMessage.DefaultBehaviour();
                                    resultMessage.DisplayMessage = "Error while InsertTblItemLinkedStoreLoc";
                                    return resultMessage;
                                }
                            }
                        }
                        else
                        {
                            res = _iTblProductItemDAO.UpdateTblItemLinkedStoreLoc(tblProductItemTO.IdProdItem, conn, tran);
                           
                           
                        }
                    }

                    //


                    //if (tblProductItemTO.IdProdItem > 0)
                    //{
                    //    resultMessage = UpdateProdItemMakeBrand(tblProductItemTO, conn, tran);
                    //    if (resultMessage.MessageType != ResultMessageE.Information)
                    //    {
                    //        tran.Rollback();
                    //        resultMessage.DefaultBehaviour(resultMessage.Text);
                    //        return resultMessage;
                    //    }
                    //}
                    else if (tblProductItemTO.IsUpdateTblProdItemData)
                    {
                        result = SetConsumableOrFixedAssetByTypeForMakeItem(Constants.ConsumableOrFixedAssetE.CONSUMABLE, tblProductItemTO.IdProdItem, tblProductItemTO.IsConsumable, conn, tran);
                        if (result == -1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour();
                            resultMessage.DisplayMessage = "Error while UpdateTblProductItem";
                            return resultMessage;
                        }
                    }

                   


                    //List<TblProductItemTO> tblitemList = _iTblProductItemDAO.findItemOfBaseItem(tblProductItemTO.IdProdItem);
                    //if (tblitemList != null || tblitemList.Count > 0)
                    //{
                    //    for (int i = 0; i < tblitemList.Count; i++)
                    //    {
                    //        result = _iTblProductItemDAO.UpdateHSNCode(tblProductItemTO.HSNCode, tblitemList[i].IdProdItem, conn, tran);
                    //        if (result == -1)
                    //        {
                    //            tran.Rollback();
                    //            resultMessage.DefaultBehaviour();
                    //            resultMessage.DisplayMessage = "Error while UpdateTblProductItem";
                    //            return resultMessage;
                    //        }
                    //        if (SAPServiceEnable && tblProductItemTO.BaseProdItemId > 0)
                    //        {
                    //            ResultMessage sapResult1 = UpdateItemHSNCodeInSAP(tblProductItemTO, tblitemList[i].IdProdItem);
                    //            if (sapResult1.Result != 1)
                    //            {
                    //                tran.Rollback();
                    //                return sapResult1;
                    //            }
                    //        }
                    //    }
                    //}
                    
                    List<TblPurchaseItemMasterTO> tblPurchaseItemMasterTOListOld = _iTblProductItemDAO.SelectAllTblPurchaseItemMasterTOListOfData(tblProductItemTO.IdProdItem, 0);

                    if (tblPurchaseItemMasterTOListOld != null || tblPurchaseItemMasterTOListOld.Count > 0)
                    {

                        for (int i = 0; i < tblPurchaseItemMasterTOListOld.Count; i++)
                        {
                            Int32 id = tblProductItemTO.IdProdItem;
                            result = _iTblProductItemDAO.DeactivateTblPurchaseItemMaster(id, conn, tran);
                            if (result == -1)
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour();
                                resultMessage.DisplayMessage = "Error while UpdateTblProductItem";
                                return resultMessage;
                            }
                        }
                    }

                    List<TblProdItemMakeBrandExtTO> tblProdItemMakeBrandExtTOListOld = _iTblProdItemMakeBrandExtDAO.SelectAllTblProdItemMakeBrandExtByProdItem(tblProductItemTO.IdProdItem);
                    if(tblProdItemMakeBrandExtTOListOld != null && tblProdItemMakeBrandExtTOListOld.Count>0)
                    {
                        for(int i=0;i< tblProdItemMakeBrandExtTOListOld.Count;i++)
                        {
                            result = _iTblProdItemMakeBrandExtDAO.DeleteTblProdItemMakeBrandExt(tblProdItemMakeBrandExtTOListOld[i].IdProdItemMakeBrandExt, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour();
                                resultMessage.DisplayMessage = "Error while tblProdItemMakeBrandExtTOList";
                                return resultMessage;
                            }
                            
                        }
                    }
                    
                }
                //Deepali Added to Add multiple make brand [22-04-2021]
                if (tblProdItemMakeBrandExtTOList != null && tblProdItemMakeBrandExtTOList.Count > 0)
                {
                    for (int i = 0; i < tblProdItemMakeBrandExtTOList.Count; i++)
                    {
                        tblProdItemMakeBrandExtTOList[i].ProdItemId = tblProductItemTO.IdProdItem;
                        tblProdItemMakeBrandExtTOList[i].CreatedBy = tblProductItemTO.CreatedBy;
                        tblProdItemMakeBrandExtTOList[i].CreatedOn = tblProductItemTO.CreatedOn;
                        //tblProdItemMakeBrandExtTOList[i].IsActive = tblProductItemTO.IsActive;
                        result = _iTblProdItemMakeBrandExtDAO.InsertTblProdItemMakeBrandExt(tblProdItemMakeBrandExtTOList[i], conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour();
                            resultMessage.DisplayMessage = "Error while tblProdItemMakeBrandExtTOList";
                            return resultMessage;
                        }
                    }
                }

                //if (tblProductItemTO.TblPurchaseItemMasterTO.SupplierOrgId > 0)
                //{
                //    tblPurchaseItemMasterTOList.Add(tblProductItemTO.TblPurchaseItemMasterTO);
                //}
                if (tblPurchaseItemMasterTOList != null || tblPurchaseItemMasterTOList.Count > 0)
                {
                    for (int i = 0; i < tblPurchaseItemMasterTOList.Count; i++)
                    {
                        tblPurchaseItemMasterTOList[i].ProdItemId = tblProductItemTO.IdProdItem;
                        tblPurchaseItemMasterTOList[i].CreatedBy = tblProductItemTO.UpdatedBy;
                        tblPurchaseItemMasterTOList[i].CreatedOn = tblProductItemTO.UpdatedOn;
                        tblPurchaseItemMasterTOList[i].IsActive = 1;

                        result = _iTblProductItemDAO.InsertTblPurchaseItemMaster(tblPurchaseItemMasterTOList[i], conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour();
                            resultMessage.DisplayMessage = "Error while InsertTblPurchaseItemMaster";
                            return resultMessage;
                        }
                    }
                }

                #region Priyanka [26-06-2019] : Added to update the gst code details against product item.
                if (tblProductItemTO.BaseProdItemId > 0 && tblProductItemTO.GstCodeId > 0)
                {
                    List<TblProdGstCodeDtlsTO> tblProdGstCodeDtlsTOList = new List<TblProdGstCodeDtlsTO>();
                    TblProdGstCodeDtlsTO tblProdGstCodeDtlsTO = new TblProdGstCodeDtlsTO();
                    tblProdGstCodeDtlsTO.ProdItemId = tblProductItemTO.IdProdItem;
                    tblProdGstCodeDtlsTO.GstCodeId = tblProductItemTO.GstCodeId;
                    tblProdGstCodeDtlsTO.IsActive = 1;
                    tblProdGstCodeDtlsTOList.Add(tblProdGstCodeDtlsTO);
                    resultMessage = _iTblProdGstCodeDtlsBL.UpdateProductGstCodeAgainstNewItem(tblProdGstCodeDtlsTOList, tblProductItemTO, tblProductItemTO.CreatedBy, conn, tran);
                    
                    if (resultMessage.Result != 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour();
                        return resultMessage;
                    }
                }
                #endregion

                if (SAPServiceEnable && tblProductItemTO.BaseProdItemId > 0)
                {
                    TblPurchaseItemMasterTO priooneSuppTO = null;
                    if (tblPurchaseItemMasterTOList != null && tblPurchaseItemMasterTOList.Count == 1)
                        priooneSuppTO = tblPurchaseItemMasterTOList[0];
                    else
                        priooneSuppTO = tblPurchaseItemMasterTOList.Where(x => x.Priority == 1).FirstOrDefault();

                    if (tblProductItemTO.IsConvertNonListedToListed)
                    {
                        ResultMessage sapResult = SaveItemInSAP(tblProductItemTO, tblPurchaseItemMasterTOList, wareHouseWiseItemDtlsTOList);
                        if (sapResult.Result != 1)
                        {
                            tran.Rollback();
                            return sapResult;
                        }
                    }
                    else if (!tblProductItemTO.IsNonListed)//Reshma
                    {
                        ResultMessage sapResult = UpdateItemInSAP(tblProductItemTO, tblPurchaseItemMasterTOList, wareHouseWiseItemDtlsTOList);
                        if (sapResult.Result != 1)
                        {
                            tran.Rollback();
                            return sapResult;
                        }
                    }
                }

                // Add By Samadhan 25 Nov 2022

                if (tblProductItemTO.IdProcess > 0)
                {
                    
                   List<DimProcessTO> TblProcessNameTOList = _iTblProductItemDAO.GetProcessName(tblProductItemTO.IdProcess, conn, tran);
                    
                    if (TblProcessNameTOList != null && TblProcessNameTOList.Count > 0)
                    {
                        string ProcessTypeName = TblProcessNameTOList[0].ProcessName;
                        List<dimProcessType> TblProcessTypeTOList = _iTblProductItemDAO.checkProcessTpyeAlreadyExists(tblProductItemTO.IdProdItem, conn, tran);
                        if (TblProcessTypeTOList != null && TblProcessTypeTOList.Count > 0)
                        {
                            result = _iTblProductItemDAO.UpdateTblProcessType(tblProductItemTO.IdProdItem, ProcessTypeName, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour();
                                resultMessage.DisplayMessage = "Error while InsertTblProcessType";
                                return resultMessage;
                            }
                        }
                        else
                        {
                            int ProcessTypeId = 0;

                            List<dimProcessType> TblProcessTypeTOIdList = _iTblProductItemDAO.GetNewProcessTypeId( conn, tran);
                            if (TblProcessTypeTOIdList != null && TblProcessTypeTOIdList.Count > 0)
                            {
                                ProcessTypeId = TblProcessTypeTOIdList[0].IdProcessType;
                            }
                            result = _iTblProductItemDAO.InsertTblProcessType(ProcessTypeId,tblProductItemTO.IdProdItem, ProcessTypeName, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour();
                                resultMessage.DisplayMessage = "Error while InsertTblProcessType";
                                return resultMessage;
                            }
                        }

                    }
                   


                }


                    //

                    tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                if (tblProductItemTO.IsConvertNonListedToListed)
                    resultMessage.DisplayMessage = tblProductItemTO.IdProdItem + " Converted Non listed to listed Successfully ";

                 return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                return resultMessage;
            }
            finally
            {
                conn.Close();
                sapConn.Close();
            }
        }

        //public ResultMessage UpdateProdItemMakeBrand(TblProductItemTO tblProductItemTO, SqlConnection conn, SqlTransaction tran)
        //{
        //    ResultMessage resultMessage = new ResultMessage();
        //    int res = 0;
        //    try
        //    {
        //        if (tblProductItemTO.ProdItemMakeBrandTOList == null || tblProductItemTO.ProdItemMakeBrandTOList.Count == 0)
        //        {
        //            return null;
        //        }
        //        for (int i = 0; i < tblProductItemTO.ProdItemMakeBrandTOList.Count; i++)
        //        {
        //            TblProdItemMakeBrandTO tblProdItemMakeBrandTO = new TblProdItemMakeBrandTO();
        //            tblProdItemMakeBrandTO.ProdItemId = tblProductItemTO.IdProdItem;
        //            tblProdItemMakeBrandTO.BrandId = tblProductItemTO.ProdItemMakeBrandTOList[i].BrandId;
        //            tblProdItemMakeBrandTO.IsDefaultMake = tblProductItemTO.ProdItemMakeBrandTOList[i].IsDefaultMake;
        //            tblProdItemMakeBrandTO.CreatedBy = tblProductItemTO.CreatedBy;
        //            tblProdItemMakeBrandTO.CreatedOn = tblProductItemTO.CreatedOn;

        //            List<TblProdItemMakeBrandTO> TblProdItemMakeBrandTOList = _iITblProdItemMakeBrandBL.SelectProdItemMakeBrandByProdItemBrandId(tblProdItemMakeBrandTO);
        //            if (TblProdItemMakeBrandTOList == null || TblProdItemMakeBrandTOList.Count == 0)
        //            {
        //                res = _iITblProdItemMakeBrandBL.InsertTblProdItemMakeBrand(tblProdItemMakeBrandTO, conn, tran);
        //                if (res != 1)
        //                {
        //                    tran.Rollback();
        //                    resultMessage.DefaultBehaviour();
        //                    resultMessage.DisplayMessage = "Error while InsertTblProdItemMakeBrand";
        //                    return resultMessage;
        //                }
        //            }
        //            else
        //            {
        //                res = _iITblProdItemMakeBrandBL.UpdateTblProdItemMakeBrand(tblProdItemMakeBrandTO, conn, tran);
        //                if (res != 1)
        //                {
        //                    tran.Rollback();
        //                    resultMessage.DefaultBehaviour();
        //                    resultMessage.DisplayMessage = "Error while UpdateTblProdItemMakeBrand";
        //                    return resultMessage;
        //                }
        //            }                    
        //        }

        //        resultMessage.DefaultSuccessBehaviour();
        //        return resultMessage;
        //    }
        //    catch (Exception ex)
        //    {
        //        tran.Rollback();
        //        resultMessage.DefaultExceptionBehaviour(ex, "InsertTblProductItem at BL");
        //        return resultMessage;
        //    }
        //    finally
        //    {
        //        //conn.Close();
        //        //sapConn.Close();
        //    }

        //}

        //Dhananjay[2021-06-22] Added for Update SAP Item With IsPurchaseItem
        public ResultMessage UpdateItemWithIsPurchaseItem()
        {
            ResultMessage resultMessage = new ResultMessage();
            SqlConnection sapConn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.SAP_CONNECTION_STRING));
            int result = _iTblProductItemDAO.UpdateItemWithIsPurchaseItem();
            if (result == -1)
            {
                resultMessage.DefaultBehaviour();
                resultMessage.DisplayMessage = "Error while UpdateItemWithIsPurchaseItem";
                return resultMessage;
            }
            else
            {
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
        }

        public int SetConsumableOrFixedAssetByTypeForMakeItem(Constants.ConsumableOrFixedAssetE consumableOrFixedAssetType, Int32 idProdItem, Boolean updateValue, SqlConnection conn, SqlTransaction tran)
        {
            int result = 0;
            result = _iTblProductItemDAO.UpdateTblProductItemIsConsumableForMakeItem(consumableOrFixedAssetType, idProdItem, updateValue, conn, tran); 
            return result;
        }
        public int UpdateTblProductItem(TblProductItemTO tblProductItemTO, SqlConnection conn, SqlTransaction tran)
        {
            int result = 0;
            if (tblProductItemTO.IsBaseItemForRate > 0)
            {
                result = _iTblProductItemDAO.updatePreviousBase(conn, tran);
            }
            if (result != 1)
            {
                return result;
            }
            return _iTblProductItemDAO.UpdateTblProductItem(tblProductItemTO, conn, tran);
        }

        //Priyanka [22-05-2018]: Added to change the product item tax type(HSN/SSN)
        public int UpdateTblProductItemTaxType(String idClassStr, Int32 codeTypeId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblProductItemDAO.UpdateTblProductItemTaxType(idClassStr, codeTypeId, conn, tran);
        }

        //@  Hudekar Priyanka [04-march-2019]

        public int UpdateTblPurchaseItemMasterTO(TblPurchaseItemMasterTO tblPurchaseItemMasterTO)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();


                result = _iTblProductItemDAO.UpdateTblPurchaseItemMasterTO(tblPurchaseItemMasterTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    return result;
                }
                tran.Commit();
                return result;
            }
            catch (Exception e)
            {
                tran.Rollback();
                return result;
            }
        }

        /// <summary>
        /// Priyanka H [14-03-2019] Update Product Item Sequence Number
        /// </summary>
        /// <param name="tblProductItemTOList"></param>
        /// <returns></returns>
        public int UpdateProductItemSequenceNo(List<TblProductItemTO> tblProductItemTOList)
        {

            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                int flag = 0;
                for (int i = 0; i < tblProductItemTOList.Count; i++)
                {
                    //  result = UpdateTblProductItem(tblProductItemTOList[i], conn, tran);
                    // result = TblProductItemDAO.UpdateTblProductItem(tblProductItemTOList[i], conn, tran);
                    result = _iTblProductItemDAO.UpdateTblProductItemSequenceNo(tblProductItemTOList[i].IsDisplaySequenceNo, tblProductItemTOList[i].IdProdItem, conn, tran);
                    if (result == 1)
                    {
                        flag = 1;
                    }
                    else
                    {
                        flag = 0;
                        break;
                    }

                }
                if (flag == 1)
                {
                    tran.Commit();
                    resultMessage.MessageType = ResultMessageE.Information;
                    resultMessage.Text = "Record Saved Sucessfully";
                    resultMessage.Result = 1;
                    return result;
                    //tran.Rollback();
                    //resultMessage.MessageType = ResultMessageE.Error;
                    //resultMessage.Text = "Error While UpdateTblProductItemTOList";
                    //resultMessage.Result = 0;
                    //return resultMessage;
                }
                else
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "tblProductItemTOList Found Null";
                    resultMessage.Result = 0;
                    return result;
                }

            }
            catch (Exception ex)
            {
                resultMessage.Text = "Exception Error While Record Save : UpdateDailyStock";
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                return result;
            }
            finally
            {
                conn.Close();
            }
        }

        //public int UpdateIsPriorityOfPurchaseItem(TblPurchaseItemMasterTO tblPurchaseItemMasterTO, SqlConnection conn, SqlTransaction tran)
        //{
        //    //SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
        //    // SqlTransaction tran = null;
        //    int result = 0;
        //    try
        //    {
        //        // conn.Open();
        //        //tran = conn.BeginTransaction();
        //        List<TblPurchaseItemMasterTO> tblPurchaseItemMasterToList = _iTblProductItemDAO.SelectAllTblPurchaseItemMasterTOListOfData(tblPurchaseItemMasterTO.ProdItemId, tblPurchaseItemMasterTO.IdPurchaseItemMaster);
        //        for (int i = 0; i < tblPurchaseItemMasterToList.Count; i++)
        //        {
        //            if (tblPurchaseItemMasterToList[i].IdPurchaseItemMaster != 0)
        //            {
        //                //  tblPurchaseItemMasterToList[i].IsPrioritySupplier = 0;
        //                result = _iTblProductItemDAO.UpdateAllIsPriorityOfPurchaseItem(tblPurchaseItemMasterToList[i], conn, tran);
        //                // return result;
        //                if (result != 1)
        //                {
        //                    //  tran.Rollback();
        //                    return result;
        //                }

        //            }
        //        }

        //        result = _iTblProductItemDAO.UpdateIsPriorityOfPurchaseItem(tblPurchaseItemMasterTO.IdPurchaseItemMaster, tblPurchaseItemMasterTO.ProdItemId, conn, tran);
        //        if (result != 1)
        //        {
        //            // tran.Rollback();
        //            return result;
        //        }
        //        //tran.Commit();
        //        return result;
        //    }
        //    catch (Exception e)
        //    {
        //        tran.Rollback();
        //        return result;
        //    }
        //    finally
        //    {
        //        //conn.Close();
        //    }
        //}

        /// <summary>
        /// Priyanka [27-04-2019] : Added to update the product item data to SAP.
        /// </summary>
        /// <param name="tblProductItemTO"></param>
        /// <param name="tblPurchaseItemMasterTOList"></param>
        /// <returns></returns>
        public ResultMessage UpdateItemInSAP(TblProductItemTO tblProductItemTO, List<TblPurchaseItemMasterTO> tblPurchaseItemMasterTOList, List<WareHouseWiseItemDtlsTO> wareHouseWiseItemDtlsTOList)
        {
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                if (Startup.CompanyObject == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "SAP CompanyObject Found NULL. Connectivity Error. " + Startup.SapConnectivityErrorCode;
                    resultMessage.DisplayMessage = "Error while creating item in SAP with Exception";
                    return resultMessage;
                }

                SAPbobsCOM.Items oitm;
                oitm = Startup.CompanyObject.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems);
                oitm.GetByKey(tblProductItemTO.IdProdItem.ToString());

                //oitm.ItemName = tblProductItemTO.ItemName;
                //oitm.ForeignName = tblProductItemTO.ItemDesc;
                if (tblProductItemTO.ItemName.Length > 100 && tblProductItemTO.ItemDesc.Length > 0)
                {
                    oitm.ItemName = Convert.ToString(tblProductItemTO.ItemName).Substring(0, 100);
                    oitm.ForeignName = Convert.ToString(tblProductItemTO.ItemDesc).Substring(0, 100);
                }
                else
                {
                    oitm.ItemName = tblProductItemTO.ItemName;
                    oitm.ForeignName = tblProductItemTO.ItemDesc;
                }

                double availStock = oitm.QuantityOnStock;

                SAPbobsCOM.ItemWarehouseInfo itemWarehouseInfo = oitm.WhsInfo;



                //oitm.PurchaseUnit = "NO";

                // tblProductItemTO.SapItemGroupId = "106";
                oitm.ItemsGroupCode = Convert.ToInt32(tblProductItemTO.SapItemGroupId);
                //oitm.UserFields.Fields.Item("U_Is_Certi_Comp").Value = "Y";

                //oitm.DefaultWarehouse = "01";
                oitm.DefaultWarehouse = tblProductItemTO.MappedLocationId.ToString();
                //if (tblPurchaseItemMasterTOList != null && tblPurchaseItemMasterTOList.Count > 0)
                //{
                //    oitm.Mainsupplier = tblPurchaseItemMasterTOList[0].SupplierOrgId.ToString();
                //}



                #region General Data
                if (tblProductItemTO.CodeTypeId == 2)
                    tblProductItemTO.IsInventoryItem = 0;
                oitm.PurchaseItem = Constants.GetYesNoEnum(tblProductItemTO.IsPurchaseItem);
                oitm.InventoryItem = Constants.GetYesNoEnum(tblProductItemTO.IsInventoryItem);
                oitm.SalesItem = Constants.GetYesNoEnum(tblProductItemTO.IsSalesItem);
                oitm.PricingUnit = Convert.ToInt32(tblProductItemTO.MappedConversionUnitOfMeasure);
                //From Generic Master it should map column mapped txn id to pass to SAP
                if (tblProductItemTO.MappedShippingTypeId > 0)
                    oitm.ShipType = tblProductItemTO.MappedShippingTypeId;

                if (tblProductItemTO.MappedManufacturerId > 0)
                    oitm.Manufacturer = tblProductItemTO.MappedManufacturerId;
                else
                    oitm.Manufacturer = -1;


                // Deepali [12-10-2020] ---- Added for BRM Item update issue.
                if (tblProductItemTO.MappedUOMGroupId != null && Convert.ToInt32(tblProductItemTO.MappedUOMGroupId) > 0)
                {
                    oitm.UoMGroupEntry = Convert.ToInt32(tblProductItemTO.MappedUOMGroupId);
                }

                oitm.MaterialType = Constants.GetMaterialTypesEnum(tblProductItemTO.MaterialTypeId);
                oitm.GSTRelevnt = Constants.GetYesNoEnum(tblProductItemTO.IsGSTApplicable);

                if (tblProductItemTO.ManageItemById == (Int32)Constants.ManageItemByIdE.SERIAL_NO)
                {
                    oitm.ManageSerialNumbers = SAPbobsCOM.BoYesNoEnum.tYES;
                    oitm.SRIAndBatchManageMethod = Constants.GetManageMethodEnum(tblProductItemTO.MgmtMethodId);          //MgmtMethodId
                    oitm.WarrantyTemplate = tblProductItemTO.WarrantyTemplateName;
                }
                else if (tblProductItemTO.ManageItemById == (Int32)Constants.ManageItemByIdE.BATCHES)
                {
                    oitm.ManageBatchNumbers = SAPbobsCOM.BoYesNoEnum.tYES;
                    oitm.SRIAndBatchManageMethod = Constants.GetManageMethodEnum(tblProductItemTO.MgmtMethodId);          //MgmtMethodId
                    oitm.WarrantyTemplate = tblProductItemTO.WarrantyTemplateName;
                }
                else
                {
                    oitm.ManageSerialNumbers = SAPbobsCOM.BoYesNoEnum.tNO;
                    oitm.ManageBatchNumbers = SAPbobsCOM.BoYesNoEnum.tNO;
                }


                oitm.PurchaseVolumeUnit = 1;
                if (Convert.ToInt32(tblProductItemTO.SapHSNCode) > 0)
                    oitm.ChapterID = Convert.ToInt32(tblProductItemTO.SapHSNCode);
                oitm.SWW = tblProductItemTO.AdditionalIdent;



                #endregion


                // oitm.PriceList.. tblProductItemTO.PriceListId;
                //Valuation method, ManageItemById, VolumeType

                #region Purchase Data
                //oitm.DefaultPurchasingUoMEntry = priooneSuppTO.
                TblPurchaseItemMasterTO priooneSuppTO = null;
                if (tblPurchaseItemMasterTOList != null && tblPurchaseItemMasterTOList.Count == 1)
                    priooneSuppTO = tblPurchaseItemMasterTOList[0];
                else
                    priooneSuppTO = tblPurchaseItemMasterTOList.Where(x => x.Priority == 1).FirstOrDefault();

                if (priooneSuppTO != null)
                {
                    if(priooneSuppTO.SupplierOrgId.ToString() != "0")
                    {
                        oitm.Mainsupplier = priooneSuppTO.SupplierOrgId.ToString();
                        oitm.SupplierCatalogNo = priooneSuppTO.MfgCatlogNo;
                        //oitm.OrderMultiple = Convert.ToDouble(priooneSuppTO.MinimumOrderQty);
                    }

                    oitm.DefaultPurchasingUoMEntry = priooneSuppTO.MappedPurchaseUOMId;

                    //if (priooneSuppTO.PurchaseUOMId == tblProductItemTO.WeightMeasureUnitId)
                    //    oitm.DefaultPurchasingUoMEntry = tblProductItemTO.MappedWeightMeasureUnitId;
                    //else if (priooneSuppTO.PurchaseUOMId == tblProductItemTO.ConversionUnitOfMeasure)
                    //    oitm.DefaultPurchasingUoMEntry = tblProductItemTO.MappedConversionUnitOfMeasure;
                }
                oitm.OrderMultiple = Convert.ToDouble(tblProductItemTO.OrderMultiple);

                for (int pv = 0; pv < tblPurchaseItemMasterTOList.Count; pv++)
                {
                    if (tblPurchaseItemMasterTOList[pv].SupplierOrgId.ToString() != "0")
                    {
                        oitm.PreferredVendors.SetCurrentLine(pv);
                        oitm.PreferredVendors.BPCode = tblPurchaseItemMasterTOList[pv].SupplierOrgId.ToString();
                        oitm.PreferredVendors.Add();
                        if (oitm.Mainsupplier == tblPurchaseItemMasterTOList[pv].SupplierOrgId.ToString())
                        {
                            oitm.PriceList.SetCurrentLine(0);
                            oitm.PriceList.Currency = "INR";
                            oitm.PriceList.Price = Convert.ToDouble(tblPurchaseItemMasterTOList[pv].BasicRate);
                        }
                    }
                    //if (tblPurchaseItemMasterTOList[pv].PurchaseUOMId == tblProductItemTO.WeightMeasureUnitId)
                    //    oitm.DefaultPurchasingUoMEntry = tblProductItemTO.MappedWeightMeasureUnitId;
                    //else if (tblPurchaseItemMasterTOList[pv].PurchaseUOMId == tblProductItemTO.ConversionUnitOfMeasure)
                    //    oitm.DefaultPurchasingUoMEntry = tblProductItemTO.MappedConversionUnitOfMeasure;
                }

                //AmolG[2020-Dec-16] For Warehouse wise inventory management.
                oitm.ManageStockByWarehouse = BoYesNoEnum.tNO;
                if (tblProductItemTO.IsManageInventory)
                {
                    oitm.ManageStockByWarehouse = BoYesNoEnum.tYES;

                    if (wareHouseWiseItemDtlsTOList != null && wareHouseWiseItemDtlsTOList.Count > 0)
                    {
                        Double minInv = 0;
                        Double maxInv = 0;
                        Double minOrd = 0;
                        Boolean isUpdateminOrderQty = false;//Reshma[1-3-22] For warehouse wise order Qty
                        List<WareHouseWiseItemDtlsTO> wareHouseWiseItemDtlsTOListNew = wareHouseWiseItemDtlsTOList.Where(w => w.MinOrder > 0).ToList();
                        if (wareHouseWiseItemDtlsTOListNew != null && wareHouseWiseItemDtlsTOListNew.Count > 0)
                        {
                            isUpdateminOrderQty = true;
                        }
                        for (int iCnt = 0; iCnt < oitm.WhsInfo.Count; iCnt++)
                        {
                            if (!String.IsNullOrEmpty(wareHouseWiseItemDtlsTOList[iCnt].WhsCode))
                            {
                                oitm.WhsInfo.SetCurrentLine(iCnt);
                                oitm.WhsInfo.WarehouseCode = wareHouseWiseItemDtlsTOList[iCnt].WhsCode;
                                oitm.WhsInfo.MaximalStock = wareHouseWiseItemDtlsTOList[iCnt].MaxInventory;
                                oitm.WhsInfo.MinimalStock = wareHouseWiseItemDtlsTOList[iCnt].MinInventory;
                                //Reshma Added FOr Rack Location.
                                if (!string.IsNullOrEmpty(wareHouseWiseItemDtlsTOList[iCnt].Rack))
                                    oitm.WhsInfo.UserFields.Fields.Item("U_Rack").Value = wareHouseWiseItemDtlsTOList[iCnt].Rack;
                                if (!string.IsNullOrEmpty(wareHouseWiseItemDtlsTOList[iCnt].Row))
                                    oitm.WhsInfo.UserFields.Fields.Item("U_Row").Value = wareHouseWiseItemDtlsTOList[iCnt].Row;
                                if (!string.IsNullOrEmpty(wareHouseWiseItemDtlsTOList[iCnt].Column))
                                    oitm.WhsInfo.UserFields.Fields.Item("U_Column").Value = wareHouseWiseItemDtlsTOList[iCnt].Column;

                                if (isUpdateminOrderQty && wareHouseWiseItemDtlsTOList[iCnt].MinOrder == 0 && wareHouseWiseItemDtlsTOList[iCnt].MinInventory >0)
                                    oitm.WhsInfo.MinimalOrder = wareHouseWiseItemDtlsTOList[iCnt].MinInventory + 1;
                                else
                                    oitm.WhsInfo.MinimalOrder = wareHouseWiseItemDtlsTOList[iCnt].MinOrder;
                                minInv += wareHouseWiseItemDtlsTOList[iCnt].MinInventory;
                                maxInv += wareHouseWiseItemDtlsTOList[iCnt].MaxInventory;
                                minOrd += wareHouseWiseItemDtlsTOList[iCnt].MinOrder;
                            }
                        }
                        oitm.DefaultWarehouse = tblProductItemTO.MappedLocationId.ToString();
                        oitm.MinInventory = minInv;
                        oitm.MaxInventory = maxInv;
                        //oitm.OrderMultiple = minOrd;
                        //oitm.MinOrderQuantity = minOrd;
                    }
                }
                else
                    oitm.MinInventory = tblProductItemTO.InventMinimum;

                oitm.MinOrderQuantity = tblProductItemTO.MinOrderQty;

                #endregion

                #region Sales Data
                //if (tblProductItemTO.SalesUOMId == tblProductItemTO.WeightMeasureUnitId)
                //    oitm.DefaultSalesUoMEntry = tblProductItemTO.MappedWeightMeasureUnitId;
                //else if (tblProductItemTO.SalesUOMId == tblProductItemTO.ConversionUnitOfMeasure)
                //    oitm.DefaultSalesUoMEntry = tblProductItemTO.MappedConversionUnitOfMeasure;

                oitm.DefaultSalesUoMEntry = tblProductItemTO.MappedSalesUOMId;

                //oitm.DefaultSalesUoMEntry = tblProductItemTO.SalesUOMId;
                if (tblProductItemTO.ItemPerSalesUnit != 0)
                    oitm.SalesItemsPerUnit = tblProductItemTO.ItemPerSalesUnit;

                if (tblProductItemTO.SalesQtyPerPkg != 0)
                    oitm.SalesQtyPerPackUnit = tblProductItemTO.SalesQtyPerPkg;

                if (tblProductItemTO.SalesHeight > 0)
                    oitm.SalesUnitHeight = tblProductItemTO.SalesHeight;
                if (tblProductItemTO.SalesWeight > 0)
                    oitm.SalesUnitWeight = tblProductItemTO.SalesWeight;
                if (tblProductItemTO.SalesWidth > 0)
                    oitm.SalesUnitWidth = tblProductItemTO.SalesWidth;
                if (tblProductItemTO.SalesLength > 0)
                    oitm.SalesUnitLength = tblProductItemTO.SalesLength;
                if (tblProductItemTO.SalesVolumeId > 0)
                    oitm.SalesVolumeUnit = tblProductItemTO.SalesVolumeId;

                #endregion

                #region Inventory Data

                oitm.GLMethod = Constants.GetGLMethodsEnum(tblProductItemTO.GLAccId);                                    //G/L method
                                                                                                                         //oitm.InventoryUOM = tblProductItemTO.InventUOMId.ToString();
                oitm.InventoryWeight = tblProductItemTO.InventWeight;
                //AmolG[2020-Dec-18] Shift this code to above
                //oitm.MinInventory = tblProductItemTO.InventMinimum;
                oitm.InventoryUoMEntry = tblProductItemTO.MappedWeightMeasureUnitId;
                oitm.CostAccountingMethod = Constants.GetBoInventorySystemEnum(tblProductItemTO.ValuationId);
                #endregion


                #region Planning & Production Data

                oitm.PlanningSystem = Constants.GetPlanningSystemEnum(tblProductItemTO.PlanningId);
                oitm.ProcurementMethod = Constants.GetProcurementMethodEnum(tblProductItemTO.ProcurementId);
                if (oitm.ProcurementMethod == SAPbobsCOM.BoProcurementMethod.bom_Make)
                    oitm.ComponentWarehouse = Constants.GetMRPComponentWarehouseEnum(tblProductItemTO.CompWareHouseId);

                //AmolG[2020-Dec-18] Shift this code to above
                //oitm.MinOrderQuantity = tblProductItemTO.MinOrderQty;
                oitm.LeadTime = Convert.ToInt32(tblProductItemTO.LeadTime);
                oitm.ToleranceDays = Convert.ToInt32(tblProductItemTO.ToleranceDays);

                oitm.IssueMethod = Constants.GetSAPIssueMethodEnum(tblProductItemTO.IssueId);
                if (tblProductItemTO.IsInventoryItem == 1)
                    oitm.GSTTaxCategory = Constants.GetGstTaxCtg(tblProductItemTO.TaxCategoryId);
                else
                    oitm.ServiceCategoryEntry = tblProductItemTO.TaxCategoryId;//Reshma[16-09-2020]
               
                //Yogesh - Commented because of we can not update gsttaxcategory in service type of item 
                // oitm.GSTTaxCategory = Constants.GetGstTaxCtg(tblProductItemTO.TaxCategoryId);
                #endregion
                                

               if(tblProductItemTO.MappedSapItemClassId == (int)Constants.ItemClassPropertiesTypeE.Class_A)
                {
                    oitm.set_Properties(tblProductItemTO.MappedSapItemClassId, SAPbobsCOM.BoYesNoEnum.tYES);
                    oitm.set_Properties((int)Constants.ItemClassPropertiesTypeE.Class_B, SAPbobsCOM.BoYesNoEnum.tNO);
                    oitm.set_Properties((int)Constants.ItemClassPropertiesTypeE.Class_C, SAPbobsCOM.BoYesNoEnum.tNO);
                }
                if (tblProductItemTO.MappedSapItemClassId == (int)Constants.ItemClassPropertiesTypeE.Class_B)
                {
                    oitm.set_Properties(tblProductItemTO.MappedSapItemClassId, SAPbobsCOM.BoYesNoEnum.tYES);
                    oitm.set_Properties((int)Constants.ItemClassPropertiesTypeE.Class_A, SAPbobsCOM.BoYesNoEnum.tNO);
                    oitm.set_Properties((int)Constants.ItemClassPropertiesTypeE.Class_C, SAPbobsCOM.BoYesNoEnum.tNO);
                }
                if (tblProductItemTO.MappedSapItemClassId == (int)Constants.ItemClassPropertiesTypeE.Class_C)
                {
                    oitm.set_Properties(tblProductItemTO.MappedSapItemClassId, SAPbobsCOM.BoYesNoEnum.tYES);
                    oitm.set_Properties((int)Constants.ItemClassPropertiesTypeE.Class_B, SAPbobsCOM.BoYesNoEnum.tNO);
                    oitm.set_Properties((int)Constants.ItemClassPropertiesTypeE.Class_A, SAPbobsCOM.BoYesNoEnum.tNO);
                }
                //reshma for fixed asset
                oitm.AssetClass = tblProductItemTO.MapSapAssetClassId;
                oitm.Location = Convert.ToInt32(tblProductItemTO.MappedSapAssetLocationId.ToString());
                int result = oitm.Update();
                if (result == 0)
                {
                    //Update Unit Price
                    //for (int pv = 0; pv < tblPurchaseItemMasterTOList.Count; pv++)
                    //{
                    //    if (tblPurchaseItemMasterTOList[pv].SupplierOrgId.ToString() != "0")
                    //    {
                    //        if (oitm.Mainsupplier == tblPurchaseItemMasterTOList[pv].SupplierOrgId.ToString())
                    //        {
                    //            oitm.PriceList.SetCurrentLine(0);
                    //            oitm.PriceList.Currency = "INR";
                    //            oitm.PriceList.Price = Convert.ToDouble(tblPurchaseItemMasterTOList[pv].BasicRate);
                    //            result= oitm.Update();

                    //            if(result==0)
                    //            {

                    //            }
                    //            else
                    //            {

                    //            }
                    //        }
                    //    }
                    //}

                    resultMessage.DefaultSuccessBehaviour();
                }
                else
                {
                    resultMessage.DefaultBehaviour();
                    string sapErrorMsg = Startup.CompanyObject.GetLastErrorDescription();
                    resultMessage.Text = sapErrorMsg;
                    resultMessage.DisplayMessage = "Error while creating item in SAP";
                }

                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.DefaultBehaviour();
                string sapErrorMsg = Startup.CompanyObject.GetLastErrorDescription();
                resultMessage.Text = sapErrorMsg + " " + ex.ToString();
                resultMessage.DisplayMessage = "    Error while creating item in SAP with Exception";
                return resultMessage;

            }
        }

        public ResultMessage UpdateItemHSNCodeInSAP(TblProductItemTO tblProductItemTO , Int32 IdProdItem)
        {
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                if (Startup.CompanyObject == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "SAP CompanyObject Found NULL. Connectivity Error. " + Startup.SapConnectivityErrorCode;
                    resultMessage.DisplayMessage = "Error while creating item in SAP with Exception";
                    return resultMessage;
                }

                SAPbobsCOM.Items oitm;
                oitm = Startup.CompanyObject.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems);
                oitm.GetByKey(IdProdItem.ToString());
                
                if (Convert.ToInt32(tblProductItemTO.SapHSNCode) > 0)
                    oitm.ChapterID = Convert.ToInt32(tblProductItemTO.SapHSNCode);
                oitm.SWW = tblProductItemTO.AdditionalIdent;


                int result = oitm.Update();
                if (result == 0)
                {
                    resultMessage.DefaultSuccessBehaviour();
                }
                else
                {
                    resultMessage.DefaultBehaviour();
                    string sapErrorMsg = Startup.CompanyObject.GetLastErrorDescription();
                    resultMessage.Text = sapErrorMsg;
                    resultMessage.DisplayMessage = "Error while creating item in SAP";
                }

                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.DefaultBehaviour();
                string sapErrorMsg = Startup.CompanyObject.GetLastErrorDescription();
                resultMessage.Text = sapErrorMsg + " " + ex.ToString();
                resultMessage.DisplayMessage = "Error while creating item in SAP with Exception";
                return resultMessage;

            }
        }


        #endregion

        #region Deletion
        public int DeleteTblProductItem(Int32 idProdItem)
        {
            return _iTblProductItemDAO.DeleteTblProductItem(idProdItem);
        }
       
        public int DeleteTblProductItem(Int32 idProdItem, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblProductItemDAO.DeleteTblProductItem(idProdItem, conn, tran);
        }
        //Priyanka [11-07-2019] : Added to delete the base item or make item.
        public ResultMessage DeactivateBaseOrMakeItem(Int32 idProdItem, Int32 baseProdItemId, Int32 loginUserId)
        {
            ResultMessage resultMessage = new ResultMessage();
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                TblProductItemTO tblProductItemTOBase = new TblProductItemTO();
                tblProductItemTOBase = _iTblProductItemDAO.SelectTblProductItem(idProdItem, conn, tran);
                if (tblProductItemTOBase != null)
                {
                    tblProductItemTOBase.UpdatedBy = Convert.ToInt32(loginUserId);
                    tblProductItemTOBase.UpdatedOn = _iCommon.ServerDateTime;
                    tblProductItemTOBase.IsActive = 0;
                    result = _iTblProductItemDAO.UpdateTblProductItem(tblProductItemTOBase, conn, tran);
                    if (result == 0)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour();
                        return resultMessage;
                    }
                }

                if (baseProdItemId == 0)
                {
                    List<TblProductItemTO> tblProductItemTOExtList = new List<TblProductItemTO>();
                    tblProductItemTOExtList = _iTblProductItemDAO.SelectTblProductItemListByBaseProdItemId(idProdItem, conn, tran);
                    if (tblProductItemTOExtList != null && tblProductItemTOExtList.Count > 0)
                    {
                        for (int i = 0; i < tblProductItemTOExtList.Count; i++)
                        {
                            TblProductItemTO tblProductItemTOMake = new TblProductItemTO();
                            tblProductItemTOMake = tblProductItemTOExtList[i];
                            tblProductItemTOMake.UpdatedBy = Convert.ToInt32(loginUserId);
                            tblProductItemTOMake.UpdatedOn = _iCommon.ServerDateTime;
                            tblProductItemTOMake.IsActive = 0;
                            result = _iTblProductItemDAO.UpdateTblProductItem(tblProductItemTOMake, conn, tran);
                            if (result == 0)
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour();
                                return resultMessage;
                            }
                        }
                    }
                }

                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "Error while DeactivateBaseOrMakeItem");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }

        }

        public ResultMessage SelectAllItemListForExportToExcel(int materialListedType)
        {
            ResultMessage resultMessage = new ResultMessage();
            List<Object> resultMessageMulti = new List<Object>();
            List<TblProductItemTO> tblProductItemTOList = _iTblProductItemDAO.SelectAllItemListForExportToExcel(materialListedType);
            
            for (int i = 0; i < tblProductItemTOList.Count; i += 10000)
            {
                List<TblProductItemTO> tblProductItemTOLis = tblProductItemTOList.GetRange(i, Math.Min(10000, tblProductItemTOList.Count - i));

                DataSet printDataSet = new DataSet();
                DataTable tblProductItemTOListDT = new DataTable();
                if (tblProductItemTOLis != null && tblProductItemTOLis.Count > 0)
            {
                tblProductItemTOListDT = Common.ToDataTable(tblProductItemTOLis);
            }
            tblProductItemTOListDT.TableName = "tblProductItemTOListDT";

            printDataSet.Tables.Add(tblProductItemTOListDT);
            String ReportTemplateName = "ItemMasterRpt";
            String templateFilePath = _iDimReportTemplateBL.SelectReportFullName(ReportTemplateName);
            String fileName = "Doc-" + DateTime.Now.Ticks;
            String saveLocation = AppDomain.CurrentDomain.BaseDirectory + fileName + ".xls";
            Boolean IsProduction = true;

            TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName("IS_PRODUCTION_ENVIRONMENT_ACTIVE");
            if (tblConfigParamsTO != null)
            {
                if (Convert.ToInt32(tblConfigParamsTO.ConfigParamVal) == 0)
                {
                    IsProduction = false;
                }
            }
            resultMessage = _iRunReport.GenrateMktgInvoiceReport(printDataSet, templateFilePath, saveLocation, Constants.ReportE.EXCEL_DONT_OPEN, IsProduction);
            if (resultMessage.MessageType == ResultMessageE.Information)
            {
                String filePath = String.Empty;
                if (resultMessage.Tag != null && resultMessage.Tag.GetType() == typeof(String))
                {
                    filePath = resultMessage.Tag.ToString();
                }
                //driveName + path;
                int returnPath = 0;
                if (returnPath != 1)
                {
                    String fileName1 = Path.GetFileName(saveLocation);
                    Byte[] bytes = File.ReadAllBytes(filePath);
                    if (bytes != null && bytes.Length > 0)
                    {
                        resultMessage.Tag = bytes;

                        string resFname = Path.GetFileNameWithoutExtension(saveLocation);
                        string directoryName;
                        directoryName = Path.GetDirectoryName(saveLocation);
                        string[] fileEntries = Directory.GetFiles(directoryName, "*Doc*");
                        string[] filesList = Directory.GetFiles(directoryName, "*Doc*");

                        foreach (string file in filesList)
                        {
                            //if (file.ToUpper().Contains(resFname.ToUpper()))
                            {
                                File.Delete(file);
                            }
                        }
                    }

                    if (resultMessage.MessageType == ResultMessageE.Information)
                    {
                        resultMessageMulti.Add(resultMessage.Tag);
                            //return resultMessage;
                        }
                }

            }
                else
                {
                resultMessage.Text = "Something wents wrong please try again";
                resultMessage.DisplayMessage = "Something wents wrong please try again";
                resultMessage.Result = 0;
                }
            }
            resultMessage.DefaultSuccessBehaviour();
            resultMessage.TagList = resultMessageMulti;
            return resultMessage;
        }

       
        #endregion
    }
}