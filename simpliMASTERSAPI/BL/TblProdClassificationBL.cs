using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using System.Linq;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;
using simpliMASTERSAPI;

namespace ODLMWebAPI.BL
{
    public class TblProdClassificationBL : ITblProdClassificationBL
    {
        private readonly ITblProdClassificationDAO _iTblProdClassificationDAO;
        private readonly ITblProductItemDAO _iTblProductItemDAO;
        private readonly IConnectionString _iConnectionString;
        private readonly ITblConfigParamsDAO _iTblConfigParamsDAO;
        private readonly IDimensionDAO _iDimensionDAO;
        private readonly ICommon _iCommon;
        private readonly IDimGstCodeTypeBL _iDimGstCodeTypeBL;

        public TblProdClassificationBL(ICommon iCommon,IDimensionDAO iDimensionDAO, ITblConfigParamsDAO iTblConfigParamsDAO, ITblProdClassificationDAO iTblProdClassificationDAO, ITblProductItemDAO iTblProductItemDAO, IConnectionString iConnectionString
            , IDimGstCodeTypeBL iDimGstCodeTypeBL)
        {
            _iTblProdClassificationDAO = iTblProdClassificationDAO;
            _iTblProductItemDAO = iTblProductItemDAO;
            _iConnectionString = iConnectionString;
            _iTblConfigParamsDAO = iTblConfigParamsDAO;
            _iDimensionDAO = iDimensionDAO;
            _iCommon = iCommon;
            _iDimGstCodeTypeBL = iDimGstCodeTypeBL;
        }
        #region Selection

        public List<TblProdClassificationTO> SelectAllTblProdClassificationList(string prodClassType = "")
        {
            return _iTblProdClassificationDAO.SelectAllTblProdClassification(prodClassType);
        }
        //Priyanka [17-06-2019]
        public List<TblProdClassificationTO> SelectAllTblProdClassificationListByParentId(Int32 prodClassId, Int32 itemProdCatId)
        {
            return _iTblProdClassificationDAO.SelectAllTblProdClassificationByParentId(prodClassId, itemProdCatId);
        }
        public List<TblProdClassificationTO> checkSubGroupAlreadyExists(TblProdClassificationTO tblProdClassificationTO)
        {
            return _iTblProdClassificationDAO.checkSubGroupAlreadyExists(tblProdClassificationTO);
        }
        public List<TblProdClassificationTO> SelectAllTblProdClassificationList(SqlConnection conn, SqlTransaction tran, string prodClassType = "")
        {
            return _iTblProdClassificationDAO.SelectAllTblProdClassification(conn, tran, prodClassType);
        }
        public List<DropDownTO> SelectAllProdClassificationForDropDown(Int32 parentClassId)
        {
            return _iTblProdClassificationDAO.SelectAllProdClassificationForDropDown(parentClassId);

        }
        public TblProdClassificationTO SelectTblProdClassificationTO(Int32 idProdClass)
        {
            return _iTblProdClassificationDAO.SelectTblProdClassification(idProdClass);
        }
        //Reshma Added
        public TblProdClassificationTO SelectTblProdClassificationTOV2(Int32 idProdClass, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblProdClassificationDAO.SelectTblProdClassificationTOV2(idProdClass, conn, tran);
        }
        public TblProdClassificationTO SelectTblProdClassification(bool isScrapProdItem, Int32 parentProdClassId)
        {
            return _iTblProdClassificationDAO.SelectTblProdClassification(isScrapProdItem,parentProdClassId);
        }

        public List<TblProdClassificationTO> SelectAllProdClassificationListyByItemProdCatgE(Constants.ItemProdCategoryE itemProdCategoryE)
        {
            return _iTblProdClassificationDAO.SelectAllProdClassificationListyByItemProdCatgE(itemProdCategoryE);
        }
        public TblProdClassificationTO SelectTblProdClassification(string prodClassDesc, Int32 parentProdClassId, int prodCatId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblProdClassificationDAO.SelectTblProdClassification(prodClassDesc, parentProdClassId,prodCatId, conn, tran);
        }

        public List<DropDownTO> SelectMaterialListForDropDown(Constants.ItemProdCategoryE itemProdCategoryE)
        {
            return _iTblProdClassificationDAO.SelectMaterialListForDropDown(itemProdCategoryE);
        }

        #endregion

        #region Product Classification DisplayName
        public void SetProductClassificationDisplayName(TblProdClassificationTO tblProdClassificationTO, List<TblProdClassificationTO> allProdClassificationList)
        {
            String DisplayName = String.Empty;
            List<TblProdClassificationTO> DisplayNameList = new List<TblProdClassificationTO>();
            if (tblProdClassificationTO != null)
            {
                //List<TblProdClassificationTO> allProdClassificationList = SelectAllTblProdClassificationList("");
                GetDisplayName(allProdClassificationList, tblProdClassificationTO.ParentProdClassId, DisplayNameList);
                DisplayNameList = DisplayNameList.OrderBy(x => x.IdProdClass).ToList();
                if (DisplayNameList != null && DisplayNameList.Count > 0)
                {
                    for (int ele = 0; ele < DisplayNameList.Count; ele++)
                    {
                        TblProdClassificationTO tempTo = DisplayNameList[ele];
                        DisplayName += tempTo.ProdClassDesc + "/";
                    }
                }
                else if (DisplayNameList.Count == 0)
                {

                }
                else
                {
                    DisplayName += DisplayNameList[0].ProdClassDesc + "/";
                }
                tblProdClassificationTO.DisplayName = DisplayName + tblProdClassificationTO.ProdClassDesc;
            }
        }
        public void GetDisplayName(List<TblProdClassificationTO> allProdClassificationList, int parentId, List<TblProdClassificationTO> DisplayNameList)
        {

            if (allProdClassificationList != null && allProdClassificationList.Count > 0)
            {
                List<TblProdClassificationTO> tempList = allProdClassificationList.Where(ele => ele.IdProdClass == parentId).ToList();
                if (tempList != null && tempList.Count > 0)
                {
                    if (tempList[0].ParentProdClassId == 0)
                    {
                        TblProdClassificationTO ProdClassificationTO = tempList[0];
                        DisplayNameList.Add(tempList[0]);
                    }
                    else
                    {
                        TblProdClassificationTO ProdClassificationTO = tempList[0];
                        DisplayNameList.Add(tempList[0]);
                        GetDisplayName(allProdClassificationList, tempList[0].ParentProdClassId, DisplayNameList);
                    }
                }
            }
        }
        //Priyanka [11-07-2019] : 
        public ResultMessage DeactivateCategoryAndRespItem(Int32 idProdClass, Int32 loginUserId)
        {
            ResultMessage resultMessage = new ResultMessage();
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                List<TblProductItemTO> productItemList = new List<TblProductItemTO>();
                string prodClassIds = SelectProdtClassificationListOnType(idProdClass);
                if (prodClassIds != string.Empty)
                {
                    List<string> list = prodClassIds.Split(',').ToList<string>();
                    for (int i = 0; i < list.Count; i++)
                    {
                        TblProdClassificationTO tblProdClassificationTO = new TblProdClassificationTO();
                        tblProdClassificationTO = _iTblProdClassificationDAO.SelectTblProdClassification(Convert.ToInt32(list[i]));
                        if (tblProdClassificationTO != null)
                        {
                            tblProdClassificationTO.UpdatedBy = loginUserId;
                            tblProdClassificationTO.UpdatedOn = _iCommon.ServerDateTime;
                            tblProdClassificationTO.IsActive = 0;
                            result = _iTblProdClassificationDAO.UpdateTblProdClassification(tblProdClassificationTO);
                            if (result == -1)
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour();
                                return resultMessage;
                            }
                        }
                    }

                    productItemList = _iTblProductItemDAO.SelectListOfProductItemTOOnprdClassId(prodClassIds);
                    if (productItemList != null || productItemList.Count > 0)
                    {
                        for (int j = 0; j < productItemList.Count; j++)
                        {
                            TblProductItemTO tblProductItemTO = new TblProductItemTO();
                            tblProductItemTO = productItemList[j];
                            tblProductItemTO.UpdatedBy = loginUserId;
                            tblProductItemTO.UpdatedOn = _iCommon.ServerDateTime;
                            tblProductItemTO.IsActive = 0;
                            
                            result = _iTblProductItemDAO.UpdateTblProductItem(tblProductItemTO, conn, tran);
                            if (result == -1)
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
                resultMessage.DefaultExceptionBehaviour(ex, "Error while DeactivateCategoryAndRespItem");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }

        }

        //Sudhir[15-March-2018] Added for getList classification  based on  Categeory or Subcategory or specification.
        public string SelectProdtClassificationListOnType(Int32 idProdClass)
        {
            try
            {
                List<TblProdClassificationTO> allProdClassificationList = SelectAllTblProdClassificationList("");
                TblProdClassificationTO tblProdClassificationTO = allProdClassificationList.Where(ele => ele.IdProdClass == idProdClass).FirstOrDefault();
                String tempids = String.Empty;
                String idsProdClass = String.Empty;
                if (allProdClassificationList != null && tblProdClassificationTO != null)
                {
                    GetIdsofProductClassification(allProdClassificationList, idProdClass, ref tempids);
                }
                idsProdClass = tempids.TrimEnd(',');
                return idsProdClass;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Sudhir[15-March-2018] Added for getIds of productClassification.
        public void GetIdsofProductClassification(List<TblProdClassificationTO> allList, int parentId, ref String ids)
        {
            ids += parentId + ",";
            List<TblProdClassificationTO> childList = allList.Where(ele => ele.ParentProdClassId == parentId).ToList();
            if (childList != null && childList.Count > 0)
            {
                foreach (TblProdClassificationTO item in childList)
                {
                    GetIdsofProductClassification(allList, item.IdProdClass, ref ids);
                }
            }
        }


        /// <summary>
        ///  Priyanka H [09-07-2019] Added for getList classification  based on  Categeory or Subcategory or specification.
        /// </summary>
        /// <param name="idProdClass"></param>
        /// <param name="subCategoryId"></param>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public string SelectProdtClassificationListOnType(Int32 idProdClass, Int32 subCategoryId, Int32 itemID)
        {
            try
            {
                List<TblProdClassificationTO> allProdClassificationList = SelectAllTblProdClassificationList("");
                //  TblProdClassificationTO tblProdClassificationTO = new TblProdClassificationTO();
                //if (idProdClass > 0 && subCategoryId ==0)
                //{
                //   tblProdClassificationTO = allProdClassificationList.Where(ele => ele.IdProdClass == idProdClass).FirstOrDefault();
                //}
                //if (subCategoryId > 0)
                //{
                //    tblProdClassificationTO = allProdClassificationList.Where(ele => ele.ParentProdClassId == idProdClass).FirstOrDefault();
                //}
                TblProdClassificationTO tblProdClassificationTO = allProdClassificationList.Where(ele => ele.IdProdClass == idProdClass).FirstOrDefault();
                String tempids = String.Empty;
                String idsProdClass = String.Empty;
                if (allProdClassificationList != null && tblProdClassificationTO != null)
                {
                    GetIdsofProductClassification(allProdClassificationList, idProdClass, subCategoryId, itemID, ref tempids);
                }
                idsProdClass = tempids.TrimEnd(',');
                return idsProdClass;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Priyanka H [09-07-2019] Added for getIds of productClassification.
        /// </summary>
        /// <param name="allList"></param>
        /// <param name="parentId"></param>
        /// <param name="subCategoryId"></param>
        /// <param name="itemID"></param>
        /// <param name="ids"></param>

        public void GetIdsofProductClassification(List<TblProdClassificationTO> allList, int parentId, int subCategoryId, int itemID, ref String ids)
        {
            List<TblProdClassificationTO> childList = new List<TblProdClassificationTO>();
            //ids += parentId + ",";
            if (parentId > 0 && subCategoryId > 0 && (itemID == 0 || itemID == null))
            {
                ids += subCategoryId + ",";

                childList = allList.Where(ele => ele.IdProdClass == subCategoryId).ToList();
            }
            else if (itemID > 0 && parentId > 0 && subCategoryId > 0)
            {
                ids += itemID + ",";

                childList = allList.Where(ele => ele.IdProdClass == itemID).ToList();
            }
            else
            {
                ids += parentId + ",";

                childList = allList.Where(ele => ele.ParentProdClassId == parentId).ToList();
            }
            // childList = allList.Where(ele => ele.ParentProdClassId == parentId).ToList();
            if (childList != null && childList.Count > 0)
            {
                foreach (TblProdClassificationTO item in childList)
                {
                    if (subCategoryId > 0 && (itemID == 0 || itemID == null))
                    {
                        GetIdsofProductClassification(allList, subCategoryId, 0, 0, ref ids);
                    }
                    else if (itemID > 0 && subCategoryId > 0 && parentId > 0)
                    {
                        GetIdsofProductClassification(allList, itemID, 0, 0, ref ids);
                    }

                    else
                    {
                        GetIdsofProductClassification(allList, item.IdProdClass, 0, 0, ref ids);

                        // GetIdsofProductClassification(allList,parentId, item.IdProdClass, 0, ref ids);
                    }
                }

            }
        }




        //[05-09-2018]Vijaymala added to get product classification data
        public List<TblProdClassificationTO> SelectProductClassificationListByProductItemId(Int32 prodItemId)
        {
            TblProductItemTO tblProductItemTO = _iTblProductItemDAO.SelectTblProductItem(prodItemId);
            List<TblProdClassificationTO> tblProdClassificationTOlist = new List<TblProdClassificationTO>();
            if (tblProductItemTO != null)
            {


                TblProdClassificationTO tblProdClassificationTO = SelectTblProdClassificationTO(tblProductItemTO.ProdClassId);
                if (tblProdClassificationTO != null)
                {
                    tblProdClassificationTO.ProdClassType = tblProdClassificationTO.ProdClassType.Trim();
                    tblProdClassificationTOlist.Add(tblProdClassificationTO);
                    tblProdClassificationTOlist = SelectProductChildList(tblProdClassificationTOlist, tblProdClassificationTO.ParentProdClassId);
                }
            }
            return tblProdClassificationTOlist;
        }

        public List<TblProdClassificationTO> SelectProductChildList(List<TblProdClassificationTO> tblProdClassificationTOlist, Int32 parentId)
        {
            TblProdClassificationTO tblProdClassificationTO = SelectTblProdClassificationTO(parentId);
            if (tblProdClassificationTO != null)
            {
                tblProdClassificationTO.ProdClassType = tblProdClassificationTO.ProdClassType.Trim();
                tblProdClassificationTOlist.Add(tblProdClassificationTO);
                SelectProductChildList(tblProdClassificationTOlist, tblProdClassificationTO.ParentProdClassId);
            }
            return tblProdClassificationTOlist;
        }
        #endregion


        #region Insertion

        //Sudhir[12-Jan-2018] Added for Set the DisplayName of Product Classification.
        public ResultMessage InsertProdClassification(TblProdClassificationTO tblProdClassificationTO)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                #region Check Category Exist Or Not 
                
                //Saket [2019-09-06] Checked only category commented and checck all the tree.
                //if (tblProdClassificationTO.ProdClassType == "C")
                if (true)
                {
                    tblProdClassificationTO.ProdClassDesc = tblProdClassificationTO.ProdClassDesc.TrimStart();
                    tblProdClassificationTO.ProdClassDesc = tblProdClassificationTO.ProdClassDesc.TrimEnd();
                    List<TblProdClassificationTO> TblProdClassificationTOList = _iTblProdClassificationDAO.checkCategoryAlreadyExists(tblProdClassificationTO.IdProdClass, tblProdClassificationTO.ProdClassType, tblProdClassificationTO.ProdClassDesc, tblProdClassificationTO.ParentProdClassId, tblProdClassificationTO.ItemProdCatId, conn, tran);
                    if (TblProdClassificationTOList != null && TblProdClassificationTOList.Count > 0)
                    {
                        if (tblProdClassificationTO.ProdClassType == "C")
                        {
                            resultMessage.DefaultBehaviour("Product is already added with Product Id " + TblProdClassificationTOList[0].IdProdClass);
                            resultMessage.DisplayMessage = "Product is already added with Product Id " + TblProdClassificationTOList[0].IdProdClass;
                        }
                        else if (tblProdClassificationTO.ProdClassType == "SC")
                        {
                            resultMessage.DefaultBehaviour("Group is already added with Group Id " + TblProdClassificationTOList[0].IdProdClass);
                            resultMessage.DisplayMessage = "Group is already added with Group Id " + TblProdClassificationTOList[0].IdProdClass;
                        }
                        else if (tblProdClassificationTO.ProdClassType == "S")
                        {
                            resultMessage.DefaultBehaviour("Sub-Group is already added with Sub-Group  Id " + TblProdClassificationTOList[0].IdProdClass);
                            resultMessage.DisplayMessage = "Sub-Group is already added with Sub-Group  Id " + TblProdClassificationTOList[0].IdProdClass;
                        }           
                        else
                        {
                            resultMessage.DefaultBehaviour("already added");
                            resultMessage.DisplayMessage = "already added ";
                        }
                        return resultMessage;
                    }
                }
              
               
                #endregion

                List<TblProdClassificationTO> allProdClassificationList = SelectAllTblProdClassificationList("");
                SetProductClassificationDisplayName(tblProdClassificationTO, allProdClassificationList);
                if (tblProdClassificationTO.IsSetDefault == 1)
                {
                    result = _iTblProdClassificationDAO.SetIsDefaultByClassificationType(tblProdClassificationTO);
                    if (result == -1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour();
                        return resultMessage;
                    }
                }
                Int32 finalResult = _iTblProdClassificationDAO.InsertTblProdClassification(tblProdClassificationTO, conn, tran);
                if (finalResult != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour();
                    return resultMessage;
                }
                #region Mapp Category to item Group

                if (tblProdClassificationTO.ProdClassType == "C")
                {
                    TblConfigParamsTO tblConfigParamsTOSAPService = _iTblConfigParamsDAO.SelectTblConfigParams(Constants.SAPB1_SERVICES_ENABLE);
                    if (tblConfigParamsTOSAPService != null)

                    {
                        string mappedTxnId = "";
                        if (Convert.ToInt32(tblConfigParamsTOSAPService.ConfigParamVal) == 1)
                        {
                            resultMessage = SaveItemCategoryInSAP(tblProdClassificationTO);

                            if (resultMessage.MessageType != ResultMessageE.Information)
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour();
                                return resultMessage;
                            }
                            mappedTxnId = resultMessage.Tag.ToString();
                            if (String.IsNullOrEmpty(mappedTxnId))
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour();
                                return resultMessage;
                            }
                            string queryString = "UPDATE tblProdClassification SET mappedTxnId = '" + mappedTxnId + "' WHERE idProdClass = " + tblProdClassificationTO.IdProdClass;
                            finalResult = _iDimensionDAO.InsertdimentionalData(queryString, false, conn, tran);
                            if (finalResult != 1)
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour();
                                return resultMessage;
                            }
                        }
                    }
                }
                #endregion
                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                // resultMessage.Text = "Error While InsertTblAlertActionDtl";
                //  resultMessage.MessageType = ResultMessageE.Error;
                //  resultMessage.Exception = ex;
                // resultMessage.Result = -1;
                resultMessage.DefaultExceptionBehaviour(ex, "InsertProdClassification");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        //chetan[2020-10-01] added with connection transcation Method
        public ResultMessage InsertProdClassification(TblProdClassificationTO tblProdClassificationTO,SqlConnection conn,SqlTransaction tran)
        {
           ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            try
            {
                #region Check Category Exist Or Not 

                //Saket [2019-09-06] Checked only category commented and checck all the tree.
                //if (tblProdClassificationTO.ProdClassType == "C")
                if (true)
                {
                    tblProdClassificationTO.ProdClassDesc = tblProdClassificationTO.ProdClassDesc.TrimStart();
                    tblProdClassificationTO.ProdClassDesc = tblProdClassificationTO.ProdClassDesc.TrimEnd();
                    List<TblProdClassificationTO> TblProdClassificationTOList = _iTblProdClassificationDAO.checkCategoryAlreadyExists(tblProdClassificationTO.IdProdClass, tblProdClassificationTO.ProdClassType, tblProdClassificationTO.ProdClassDesc, tblProdClassificationTO.ParentProdClassId, tblProdClassificationTO.ItemProdCatId, conn, tran);
                    if (TblProdClassificationTOList != null && TblProdClassificationTOList.Count > 0)
                    {
                        if (tblProdClassificationTO.ProdClassType == "C")
                        {
                            resultMessage.DefaultBehaviour("Group is already added with Group Id " + TblProdClassificationTOList[0].IdProdClass);
                            resultMessage.DisplayMessage = "Group is already added with Group Id " + TblProdClassificationTOList[0].IdProdClass;
                        }
                        else if (tblProdClassificationTO.ProdClassType == "SC")
                        {
                            resultMessage.DefaultBehaviour("Sub-Group is already added with Sub-Group Id " + TblProdClassificationTOList[0].IdProdClass);
                            resultMessage.DisplayMessage = "Sub-Group is already added with Sub-Group Id " + TblProdClassificationTOList[0].IdProdClass;
                        }
                        else if (tblProdClassificationTO.ProdClassType == "S")
                        {
                            resultMessage.DefaultBehaviour("Product is already added with Product Id " + TblProdClassificationTOList[0].IdProdClass);
                            resultMessage.DisplayMessage = "Product is already added with Product Id " + TblProdClassificationTOList[0].IdProdClass;
                        }
                        else
                        {
                            resultMessage.DefaultBehaviour("already added");
                            resultMessage.DisplayMessage = "already added ";
                        }
                        return resultMessage;
                    }
                }


                #endregion

                List<TblProdClassificationTO> allProdClassificationList = SelectAllTblProdClassificationList("");
                SetProductClassificationDisplayName(tblProdClassificationTO, allProdClassificationList);
                if (tblProdClassificationTO.IsSetDefault == 1)
                {
                    result = _iTblProdClassificationDAO.SetIsDefaultByClassificationType(tblProdClassificationTO);
                    if (result == -1)
                    {
                        resultMessage.DefaultBehaviour();
                        return resultMessage;
                    }
                }
                Int32 finalResult = _iTblProdClassificationDAO.InsertTblProdClassification(tblProdClassificationTO, conn, tran);
                if (finalResult != 1)
                {
                    resultMessage.DefaultBehaviour();
                    return resultMessage;
                }
                #region Mapp Category to item Group

                if (tblProdClassificationTO.ProdClassType == "C")
                {
                    TblConfigParamsTO tblConfigParamsTOSAPService = _iTblConfigParamsDAO.SelectTblConfigParams(Constants.SAPB1_SERVICES_ENABLE);
                    if (tblConfigParamsTOSAPService != null)

                    {
                        string mappedTxnId = "";
                        if (Convert.ToInt32(tblConfigParamsTOSAPService.ConfigParamVal) == 1)
                        {
                            resultMessage = SaveItemCategoryInSAP(tblProdClassificationTO);

                            if (resultMessage.MessageType != ResultMessageE.Information)
                            {
                                resultMessage.DefaultBehaviour();
                                return resultMessage;
                            }
                            mappedTxnId = resultMessage.Tag.ToString();
                            tblProdClassificationTO.MappedTxnId = mappedTxnId;
                            if (String.IsNullOrEmpty(mappedTxnId))
                            {
                                resultMessage.DefaultBehaviour();
                                return resultMessage;
                            }
                          
                            string queryString = "UPDATE tblProdClassification SET mappedTxnId = '" + mappedTxnId + "' WHERE idProdClass = " + tblProdClassificationTO.IdProdClass;
                            finalResult = _iDimensionDAO.InsertdimentionalData(queryString, false, conn, tran);
                            if (finalResult != 1)
                            {
                                resultMessage.DefaultBehaviour();
                                return resultMessage;
                            }
                        }
                    }
                }
                #endregion
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "InsertProdClassification");
                return resultMessage;
            }
            finally
            {
            }
        }

        public ResultMessage SaveItemCategoryInSAP(TblProdClassificationTO tblProdClassificationTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            if (Startup.CompanyObject == null)
            {
                // return string.Empty;
                resultMessage.DefaultBehaviour("SAP CompanyObject Found NULL. Connectivity Error.");
                return resultMessage;
            }

            SAPbobsCOM.ItemGroups oitmGrp;
            oitmGrp = Startup.CompanyObject.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItemGroups);
            oitmGrp.GroupName = tblProdClassificationTO.ProdClassDesc;
            //oitmGrp.Number = LastInserId;
            if ((Int32)SAPbobsCOM.ItemClassEnum.itcService == tblProdClassificationTO.CodeTypeId)
                oitmGrp.ItemClass = SAPbobsCOM.ItemClassEnum.itcMaterial;
            else
                oitmGrp.ItemClass = SAPbobsCOM.ItemClassEnum.itcService;

            //Reshma[21-09-2020] For Account type
            DimGstCodeTypeTO dimGstCodeTypeTO = _iDimGstCodeTypeBL.SelectDimGstCodeTypeTO(tblProdClassificationTO.CodeTypeId);
            if ( !string.IsNullOrEmpty( dimGstCodeTypeTO.InventoryAcctFinLedgerId ) && !string.IsNullOrEmpty( dimGstCodeTypeTO.CostAcctFinLedgerId)
                && !string.IsNullOrEmpty(dimGstCodeTypeTO.TransfersAcctFinLedgerId )
                && !string.IsNullOrEmpty(dimGstCodeTypeTO.VarianceAcctFinLedgerId) && !string.IsNullOrEmpty(dimGstCodeTypeTO.PriceDifferenceAcctFinLedgerId))
            {
                oitmGrp.InventoryAccount = dimGstCodeTypeTO.InventoryAcctFinLedgerId.ToString();   /// "_SYS00000000114";  // Inventory
                oitmGrp.CostAccount = dimGstCodeTypeTO.CostAcctFinLedgerId.ToString();   //"_SYS00000000262"; // Goods and Sold
                oitmGrp.TransfersAccount = dimGstCodeTypeTO.TransfersAcctFinLedgerId.ToString(); //"_SYS00000000376";// Allocation
                oitmGrp.VarianceAccount = dimGstCodeTypeTO.VarianceAcctFinLedgerId.ToString(); ///"_SYS00000000268"; // Variance Account
                oitmGrp.PriceDifferencesAccount = dimGstCodeTypeTO.PriceDifferenceAcctFinLedgerId.ToString();   ///"_SYS00000000269";  // Price Diff Account
            }
            int result = oitmGrp.Add();

            string TxnId = "";
            if (result != 0)
            {
                string errorMsg = Startup.CompanyObject.GetLastErrorDescription();
                resultMessage.DefaultBehaviour(errorMsg);
            }
            else
            {
                TxnId = Startup.CompanyObject.GetNewObjectKey();
                resultMessage.Tag = TxnId;
            }
            if (resultMessage.MessageType == ResultMessageE.None)
            {
                resultMessage.MessageType = ResultMessageE.Information;
            }
            return resultMessage;
        }
        //@Kiran To Get GetDefaultProductClassification List
        public List<Int32> GetDefaultProductClassification()
        {
            List<Int32> defualtList = new List<Int32>();
            List<TblProdClassificationTO> allProductClassificationList = SelectAllTblProdClassificationList("");
            if (allProductClassificationList.Count > 0 && allProductClassificationList != null)
            {
                TblProdClassificationTO ProdutCategoryTo = allProductClassificationList.Where(w => w.ProdClassType.Trim().Equals("C") && w.IsSetDefault == 1).FirstOrDefault();
                if (ProdutCategoryTo != null)
                {
                    defualtList.Add(ProdutCategoryTo.IdProdClass);
                }
                else
                {
                    defualtList.Add(0);
                    return defualtList;
                }
                TblProdClassificationTO ProdSubCategoryTo = allProductClassificationList.Where(w => w.ProdClassType.Trim().Equals("SC") && w.IsSetDefault == 1 && w.ParentProdClassId == ProdutCategoryTo.IdProdClass).FirstOrDefault();
                if (ProdSubCategoryTo != null)
                {
                    defualtList.Add(ProdSubCategoryTo.IdProdClass);
                }
                else
                {
                    defualtList.Add(0);
                    return defualtList;
                }
                TblProdClassificationTO ProdSpecificationTo = allProductClassificationList.Where(w => w.ProdClassType.Trim().Equals("S") && w.IsSetDefault == 1 && w.ParentProdClassId == ProdSubCategoryTo.IdProdClass).FirstOrDefault();
                if (ProdSpecificationTo != null)
                {
                    defualtList.Add(ProdSpecificationTo.IdProdClass);
                }
                else
                    defualtList.Add(0);
            }
            return defualtList;
        }
        public int InsertTblProdClassification(TblProdClassificationTO tblProdClassificationTO)
        {
            return _iTblProdClassificationDAO.InsertTblProdClassification(tblProdClassificationTO);
        }

        public int InsertTblProdClassification(TblProdClassificationTO tblProdClassificationTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblProdClassificationDAO.InsertTblProdClassification(tblProdClassificationTO, conn, tran);
        }

        #endregion

        //Sudhir[12-Jan-2018] Added for Updating DisplayName Recursively.
        public int UpdateDisplayName(List<TblProdClassificationTO> allProdClassificationList, TblProdClassificationTO ProdClassificationTO, ref String idClassStr, SqlConnection conn, SqlTransaction tran)
        {
            int result = 0;
            List<TblProdClassificationTO> childList = allProdClassificationList.Where(ele => ele.ParentProdClassId == ProdClassificationTO.IdProdClass).ToList();
            if (childList != null && childList.Count > 0)
            {
                for (int i = 0; i < childList.Count; i++)
                {
                    TblProdClassificationTO tempTo = childList[i];
                    tempTo.UpdatedOn = childList[i].CreatedOn;
                    tempTo.UpdatedBy = childList[i].CreatedBy;
                    tempTo.CodeTypeId = ProdClassificationTO.CodeTypeId;                        //Priyanka [21-05-18]
                    SetProductClassificationDisplayName(tempTo, allProdClassificationList);
                    result = UpdateTblProdClassification(tempTo, conn, tran);

                    idClassStr += tempTo.IdProdClass + ",";

                    if (result >= 0)
                    {
                        result = UpdateDisplayName(allProdClassificationList, tempTo, ref idClassStr, conn, tran);
                    }
                    else
                        return -1;
                }
                //if (idClassStr != String.Empty)
                //{
                //    idClassStr = idClassStr.TrimEnd(',');
                //     result = _iTblProductItemDAO.UpdateTblProductItemTaxType(idClassStr, ProdClassificationTO.CodeTypeId, conn, tran);
                //}
            }
            else
                result = 1;
            return result;
        }

        #region Updation

        //Sudhir[12-Jan-2018] Added for updating productclassificaiton and its Displayname where its refrences.
        public ResultMessage UpdateProdClassification(TblProdClassificationTO tblProdClassificationTO)
        {

            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            try
            {

              

                List<TblProdClassificationTO> allProdClassificationList = SelectAllTblProdClassificationList("");
                SetProductClassificationDisplayName(tblProdClassificationTO, allProdClassificationList);
                conn.Open();
                tran = conn.BeginTransaction();

                #region Check Category Exist Or Not 
                //Saket [2019-09-06] Commented and added new function.
                //if (tblProdClassificationTO.ProdClassType == "C")
                if(true)
                {
                    tblProdClassificationTO.ProdClassDesc = tblProdClassificationTO.ProdClassDesc.TrimStart();
                    tblProdClassificationTO.ProdClassDesc = tblProdClassificationTO.ProdClassDesc.TrimEnd();
                    List<TblProdClassificationTO> TblProdClassificationTOList = _iTblProdClassificationDAO.checkCategoryAlreadyExists(tblProdClassificationTO.IdProdClass, tblProdClassificationTO.ProdClassType, tblProdClassificationTO.ProdClassDesc, tblProdClassificationTO.ParentProdClassId, tblProdClassificationTO.ItemProdCatId, conn, tran);
                    if (TblProdClassificationTOList != null && TblProdClassificationTOList.Count > 0)
                    {
                        if (tblProdClassificationTO.ProdClassType == "C")
                        {
                            resultMessage.DefaultBehaviour("Group is already added with Group Id " + TblProdClassificationTOList[0].IdProdClass);
                            resultMessage.DisplayMessage = "Group is already added with Group Id " + TblProdClassificationTOList[0].IdProdClass;
                        }
                        else if (tblProdClassificationTO.ProdClassType == "SC")
                        {
                            resultMessage.DefaultBehaviour("Sub-Group is already added with Sub-Group Id " + TblProdClassificationTOList[0].IdProdClass);
                            resultMessage.DisplayMessage = "Sub-Group is already added with Sub-Group Id " + TblProdClassificationTOList[0].IdProdClass;
                        }
                        else if (tblProdClassificationTO.ProdClassType == "S")
                        {
                            resultMessage.DefaultBehaviour("Product is already added with Product Id " + TblProdClassificationTOList[0].IdProdClass);
                            resultMessage.DisplayMessage = "Product is already added with Product Id " + TblProdClassificationTOList[0].IdProdClass;
                        }

                        else
                        {
                            resultMessage.DefaultBehaviour("already added");
                            resultMessage.DisplayMessage = "already added ";
                        }
                        return resultMessage;
                    }

                }
                #endregion

                if (tblProdClassificationTO.IsSetDefault == 1)
                {
                    resultMessage.Result = _iTblProdClassificationDAO.SetIsDefaultByClassificationType(tblProdClassificationTO);
                    if (result == -1)
                    {
                        tran.Rollback();
                        return resultMessage;
                    }
                }
                result = _iTblProdClassificationDAO.UpdateTblProdClassification(tblProdClassificationTO, conn, tran);
                if (result == -1)
                {
                    tran.Rollback();
                    return resultMessage;
                }
                //Harshala added
                else if(tblProdClassificationTO.IsUpdateTblProdItemData)
                {
                    result = SetConsumableOrFixedAssetByType(Constants.ConsumableOrFixedAssetE.CONSUMABLE, tblProdClassificationTO.IdProdClass, tblProdClassificationTO.IsConsumable, conn, tran);
                    if (result == -1)
                    {
                        tran.Rollback();
                        return resultMessage;
                    }
                }
                //
                allProdClassificationList = SelectAllTblProdClassificationList(conn, tran, "");

                String updatedIds = String.Empty;

                result = UpdateDisplayName(allProdClassificationList, tblProdClassificationTO, ref updatedIds, conn, tran);
                if (result == -1)
                {
                    tran.Rollback();
                    return resultMessage;
                }


                if (updatedIds != String.Empty)
                {
                    updatedIds = updatedIds.TrimEnd(',');
                    result = _iTblProductItemDAO.UpdateTblProductItemTaxType(updatedIds, tblProdClassificationTO.CodeTypeId, conn, tran);
                }

                #region Update Item Group In SAP
                if (tblProdClassificationTO.ProdClassType == "C")
                {
                    TblConfigParamsTO tblConfigParamsTOSAPService = _iTblConfigParamsDAO.SelectTblConfigParams(Constants.SAPB1_SERVICES_ENABLE);
                    if (tblConfigParamsTOSAPService != null)
                    {
                        if (Convert.ToInt32(tblConfigParamsTOSAPService.ConfigParamVal) == 1)
                        {
                            SAPbobsCOM.ItemGroups oitmGrp;
                            oitmGrp = Startup.CompanyObject.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItemGroups);
                            oitmGrp.GetByKey(Convert.ToInt32(tblProdClassificationTO.MappedTxnId));
                            int finalResult;
                            if (tblProdClassificationTO.IsActive == 1)
                            {
                                oitmGrp.GroupName = tblProdClassificationTO.ProdClassDesc;
                                if ((Int32)SAPbobsCOM.ItemClassEnum.itcService == tblProdClassificationTO.CodeTypeId)
                                    oitmGrp.ItemClass = SAPbobsCOM.ItemClassEnum.itcMaterial;
                                else
                                    oitmGrp.ItemClass = SAPbobsCOM.ItemClassEnum.itcService;
                                finalResult = oitmGrp.Update();
                            }
                            else
                            {
                                finalResult = oitmGrp.Remove();
                            }
                            if (finalResult != 0)
                            {
                                string errorMsg = Startup.CompanyObject.GetLastErrorDescription();
                                resultMessage.Result = -1;
                                tran.Rollback();
                                return resultMessage;
                            }
                            if(finalResult == 0)
                            {
                                resultMessage.DefaultSuccessBehaviour();
                            }
                        }
                    }
                }

                #endregion

                tran.Commit();
                return resultMessage;
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

        
        /// <summary>
        /// Harshala added to update IsConsumable/IsFixedAsset according to type
        /// </summary>
        /// <param name="ConsumableOrFixedAssetType"></param>
        /// <param name="idProdClass"></param>
        /// <param name="updateValue"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public int SetConsumableOrFixedAssetByType(Constants.ConsumableOrFixedAssetE consumableOrFixedAssetType, Int32 idProdClass,Boolean updateValue, SqlConnection conn, SqlTransaction tran)
        {
            int result = 0;

             result = _iTblProductItemDAO.UpdateTblProductItemIsConsumable(consumableOrFixedAssetType, idProdClass, updateValue, conn, tran);

            return result;
        }

        public int UpdateTblProdClassification(TblProdClassificationTO tblProdClassificationTO)
        {
            return _iTblProdClassificationDAO.UpdateTblProdClassification(tblProdClassificationTO);
        }

        public int UpdateTblProdClassification(TblProdClassificationTO tblProdClassificationTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblProdClassificationDAO.UpdateTblProdClassification(tblProdClassificationTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblProdClassification(Int32 idProdClass)
        {
            return _iTblProdClassificationDAO.DeleteTblProdClassification(idProdClass);
        }

        public int DeleteTblProdClassification(Int32 idProdClass, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblProdClassificationDAO.DeleteTblProdClassification(idProdClass, conn, tran);
        }

        #endregion

        public List<DropDownTO> getProdClassIdsByItemProdCat(Int32 itemProdCatId, string prodClassType = "S")
        {
            return _iTblProdClassificationDAO.getProdClassIdsByItemProdCat(itemProdCatId, prodClassType);
        }



    }
}
