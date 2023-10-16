using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;
using  SAPbobsCOM;
using simpliMASTERSAPI;

namespace ODLMWebAPI.BL
{
    public class DimUomGroupBL : IDimUomGroupBL
    {
        private readonly IDimUomGroupDAO _iDimUomGroupDAO;
        private readonly ICommon _iCommon;
        private readonly IDimUomGroupConversionDAO _iDimUomGroupConversionDAO;
        private readonly ITblConfigParamsDAO _iTblConfigParamsDAO;
        private readonly IConnectionString _iConnectionString;

        public DimUomGroupBL(ITblConfigParamsDAO iTblConfigParamsDAO, IDimUomGroupDAO iDimUomGroupDAO, ICommon iCommon, IConnectionString iConnectionString, IDimUomGroupConversionDAO iDimUomGroupConversionDAO)
        {
            _iDimUomGroupDAO = iDimUomGroupDAO;
            _iCommon = iCommon;
            _iConnectionString = iConnectionString;
            _iDimUomGroupConversionDAO = iDimUomGroupConversionDAO;
            _iTblConfigParamsDAO = iTblConfigParamsDAO;
        }
        //public DimUomGroupBL(IDimUomGroupDAO iDimUomGroupDAO, ITblConfigParamsBL iTblConfigParamsBL, IDimUomGroupBL iDimUomGroupBL, IConnectionString iConnectionString)
        //{
        //    _iDimUomGroupDAO = iDimUomGroupDAO;
        //    _iTblConfigParamsBL = iTblConfigParamsBL;
        //   // _iDimUomGroupBL = iDimUomGroupBL;
        //    _iConnectionString = iConnectionString;
        //}
        #region Selection
        public List<DimUomGroupTO> SelectAllDimUomGroup()
        {
            return _iDimUomGroupDAO.SelectAllDimUomGroup();
        }
        public List<DimUomGroupTO> SelectAllDimUomGroupList()
        {
            return _iDimUomGroupDAO.SelectAllDimUomGroup();
        }
        public DimUomGroupTO SelectDimUomGroupTO(Int32 idUomGroup)
        {
            return _iDimUomGroupDAO.SelectDimUomGroup(idUomGroup);
        }
        public List<DimUomGroupTO> SelectAllUomGroupList(DimUomGroupTO dimUomGroupTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimUomGroupDAO.SelectAllUomGroupList(dimUomGroupTO, conn, tran);
        }

        public ResultMessage SaveAndUpdateUOMGroup(DimUomGroupTO dimUomGroupTO, Int32 loginUserId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            int res = 0;

            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                List<DimUomGroupTO> list = SelectAllUomGroupList(dimUomGroupTO, conn, tran);
                if (list != null && list.Count > 0)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Group Name Already Exist";
                    resultMessage.DisplayMessage = "Uom Group Name Already Exist";
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                if (dimUomGroupTO.IdUomGroup > 0)
                {
                    dimUomGroupTO.UpdatedOn = _iCommon.ServerDateTime;
                    dimUomGroupTO.UpdatedBy = Convert.ToInt32(loginUserId);
                    result = UpdateDimUomGroup(dimUomGroupTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Text = "Error While UpdateDimUomGroup";
                        resultMessage.DisplayMessage = "Record Cound Not Saved";
                        return resultMessage;

                    }
                    //if (dimUomGroupTO.IsActive != 0)
                    //{
                    //    dimUomGroupTO.UomGroupConversionTO.UomGroupId = dimUomGroupTO.IdUomGroup;
                    //    result = _iDimUomGroupConversionDAO.UpdateDimUomGroupConversion(dimUomGroupTO.UomGroupConversionTO, conn, tran);
                    //    if (result != 1)
                    //    {
                    //        tran.Rollback();
                    //        resultMessage.MessageType = ResultMessageE.Error;
                    //        resultMessage.Text = "Error While UpdateDimUomGroupConversion";
                    //        resultMessage.DisplayMessage = "Record Cound Not Saved";
                    //        return resultMessage;

                    //    }
                    //}
                    TblConfigParamsTO tblConfigParamsTOSAPService = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.SAPB1_SERVICES_ENABLE);
                    if (tblConfigParamsTOSAPService != null)
                    {
                        if (Convert.ToInt32(tblConfigParamsTOSAPService.ConfigParamVal) == 1)
                        {
                            ResultMessage sapResult = UpdateUOMMasterInSAP(dimUomGroupTO);
                            if (sapResult.Result == 1)
                            {
                                tran.Commit();
                                return sapResult;
                            }
                            else
                            {
                                tran.Rollback();
                                return sapResult;
                            }
                        }
                        else
                        {
                            tran.Commit();
                            resultMessage.DefaultSuccessBehaviour();
                            return resultMessage;
                        }
                    }
                    tran.Commit();

                    resultMessage.MessageType = ResultMessageE.Information;
                    resultMessage.Result = 1;
                    resultMessage.Text = "Success... Group Saved";
                    resultMessage.DisplayMessage = "UOM Group updated Successfully.";
                    return resultMessage;

                }
                else
                {
                    dimUomGroupTO.CreatedOn = _iCommon.ServerDateTime;
                    dimUomGroupTO.CreatedBy = Convert.ToInt32(loginUserId);
                    result = InsertDimUomGroup(dimUomGroupTO, conn, tran);
                    if (result != 1)
                    {

                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Text = "Error While InsertDimUomGroup";
                        resultMessage.DisplayMessage = "Record Cound Not Saved";
                        return resultMessage;

                    }
                    //dimUomGroupTO.UomGroupConversionTO.UomGroupId = dimUomGroupTO.IdUomGroup;
                    //result = _iDimUomGroupConversionDAO.InsertDimUomGroupConversion(dimUomGroupTO.UomGroupConversionTO, conn, tran);
                    //if (result != 1)
                    //{

                    //    resultMessage.MessageType = ResultMessageE.Error;
                    //    resultMessage.Text = "Error While InsertDimUomGroupConversion";
                    //    resultMessage.DisplayMessage = "Record Cound Not Saved";
                    //    return resultMessage;

                    //}
                    TblConfigParamsTO tblConfigParamsTOSAPService = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.SAPB1_SERVICES_ENABLE);
                    if (tblConfigParamsTOSAPService != null)
                    {
                        if (Convert.ToInt32(tblConfigParamsTOSAPService.ConfigParamVal) == 1)
                        {
                            ResultMessage sapResult = SaveUOMMasterInSAP(dimUomGroupTO);
                            if (sapResult.Result == 1)
                            {
                                tran.Commit();
                                return sapResult;
                            }
                            else
                            {
                                tran.Rollback();
                                return sapResult;
                            }
                        }
                        else
                        {
                            tran.Commit();
                            resultMessage.DefaultSuccessBehaviour();
                            return resultMessage;
                        }
                    }
                    tran.Commit();
                    resultMessage.MessageType = ResultMessageE.Information;
                    resultMessage.Result = 1;
                    resultMessage.Text = "Success... Group Saved";
                    resultMessage.DisplayMessage = "Success... Group Saved";


                }
              
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "API : Exception In Method PostUomGroupMaster";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return resultMessage;
            }
            finally{
                conn.Close();
            }
        }

        public ResultMessage SaveAndUpdateUOM(DimUomGroupConversionTO UomGroupConversionTO, Int32 loginUserId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            int res = 0;

            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                //List<DimUomGroupTO> list = SelectAllUomGroupList(dimUomGroupTO, conn, tran);
                //if (list != null && list.Count > 0)
                //{
                //    resultMessage.MessageType = ResultMessageE.Error;
                //    resultMessage.Text = "Group Name Already Exist";
                //    resultMessage.DisplayMessage = "Uom Group Name Already Exist";
                //    resultMessage.Result = 0;
                //    return resultMessage;
                //}

                if (UomGroupConversionTO.IdUomConversion > 0)
                {
                    //UomGroupConversionTO.UpdatedOn = _iCommon.ServerDateTime;
                    //UomGroupConversionTO.UpdatedBy = Convert.ToInt32(loginUserId);
                    //result = UpdateDimUomGroup(dimUomGroupTO, conn, tran);
                    //if (result != 1)
                    //{
                    //    tran.Rollback();
                    //    resultMessage.MessageType = ResultMessageE.Error;
                    //    resultMessage.Text = "Error While UpdateDimUomGroup";
                    //    resultMessage.DisplayMessage = "Record Cound Not Saved";
                    //    return resultMessage;

                    //}
                    //if (dimUomGroupTO.IsActive != 0)
                    //{
                        //dimUomGroupTO.UomGroupConversionTO.UomGroupId = dimUomGroupTO.IdUomGroup;
                        result = _iDimUomGroupConversionDAO.UpdateDimUomGroupConversion(UomGroupConversionTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error While UpdateDimUomGroupConversion";
                            resultMessage.DisplayMessage = "Record Cound Not Saved";
                            return resultMessage;

                        }
                    //}
                    TblConfigParamsTO tblConfigParamsTOSAPService = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.SAPB1_SERVICES_ENABLE);
                    if (tblConfigParamsTOSAPService != null)
                    {
                        if (Convert.ToInt32(tblConfigParamsTOSAPService.ConfigParamVal) == 1)
                        {
                            ResultMessage sapResult = new ResultMessage();// UpdateUOMMasterInSAP(dimUomGroupTO);
                            if (sapResult.Result == 1)
                            {
                                tran.Commit();
                                return sapResult;
                            }
                            else
                            {
                                tran.Rollback();
                                return sapResult;
                            }
                        }
                        else
                        {
                            tran.Commit();
                            resultMessage.DefaultSuccessBehaviour();
                            return resultMessage;
                        }
                    }
                    tran.Commit();

                    resultMessage.MessageType = ResultMessageE.Information;
                    resultMessage.Result = 1;
                    resultMessage.Text = "Success... Group Saved";
                    resultMessage.DisplayMessage = "UOM Group updated Successfully.";
                    return resultMessage;

                }
                else
                {
                    //dimUomGroupTO.CreatedOn = _iCommon.ServerDateTime;
                    //dimUomGroupTO.CreatedBy = Convert.ToInt32(loginUserId);
                    //result = InsertDimUomGroup(dimUomGroupTO, conn, tran);
                    //if (result != 1)
                    //{

                    //    resultMessage.MessageType = ResultMessageE.Error;
                    //    resultMessage.Text = "Error While InsertDimUomGroup";
                    //    resultMessage.DisplayMessage = "Record Cound Not Saved";
                    //    return resultMessage;

                    //}
                    //dimUomGroupTO.UomGroupConversionTO.UomGroupId = dimUomGroupTO.IdUomGroup;
                    result = _iDimUomGroupConversionDAO.InsertDimUomGroupConversion(UomGroupConversionTO, conn, tran);
                    if (result != 1)
                    {

                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Text = "Error While InsertDimUomGroupConversion";
                        resultMessage.DisplayMessage = "Record Cound Not Saved";
                        return resultMessage;

                    }
                    TblConfigParamsTO tblConfigParamsTOSAPService = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.SAPB1_SERVICES_ENABLE);
                    if (tblConfigParamsTOSAPService != null)
                    {
                        if (Convert.ToInt32(tblConfigParamsTOSAPService.ConfigParamVal) == 1)
                        {
                            ResultMessage sapResult = new ResultMessage();// SaveUOMMasterInSAP(dimUomGroupTO);
                            if (sapResult.Result == 1)
                            {
                                tran.Commit();
                                return sapResult;
                            }
                            else
                            {
                                tran.Rollback();
                                return sapResult;
                            }
                        }
                        else
                        {
                            tran.Commit();
                            resultMessage.DefaultSuccessBehaviour();
                            return resultMessage;
                        }
                    }
                    tran.Commit();
                    resultMessage.MessageType = ResultMessageE.Information;
                    resultMessage.Result = 1;
                    resultMessage.Text = "Success... Group Saved";
                    resultMessage.DisplayMessage = "Success... Group Saved";


                }

                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "API : Exception In Method PostUomGroupMaster";
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }
        /// <summary>
        /// Priyanka [30-04-2019] : Added to save and update UOM group into SAP.
        /// </summary>
        /// <param name="dimUomGroupTO"></param>
        /// <returns></returns>
        public ResultMessage SaveUOMMasterInSAP(DimUomGroupTO dimUomGroupTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            try
            {
                if (Startup.CompanyObject == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "SAP CompanyObject Found NULL. Connectivity Error. " + Startup.SapConnectivityErrorCode;
                    resultMessage.DisplayMessage = "Error while creating item in SAP with Exception";
                    return resultMessage;
                }

                CompanyService oCompanyService = Startup.CompanyObject.GetCompanyService();
                UnitOfMeasurementGroupsService ouom = (UnitOfMeasurementGroupsService)oCompanyService.GetBusinessService(ServiceTypes.UnitOfMeasurementGroupsService);

                //// Add a UOM Group
                SAPbobsCOM.UnitOfMeasurementGroup uom = (SAPbobsCOM.UnitOfMeasurementGroup)ouom.GetDataInterface
                                 (UnitOfMeasurementGroupsServiceDataInterfaces.uomgsUnitOfMeasurementGroup);

                uom.Code = dimUomGroupTO.UomGroupCode;
                uom.Name = dimUomGroupTO.UomGroupName;
                uom.BaseUoM = dimUomGroupTO.BaseUomId;

                UoMGroupDefinitionCollection collection = ouom.GetDataInterface(SAPbobsCOM.UnitOfMeasurementGroupsServiceDataInterfaces.uomgsUnitOfMeasurementGroup);
                    // uom.GroupDefinitions.Item(uom).AlternateUoM = dimUomGroupTO.UomGroupConversionTO.UomId;
                //uom.GroupDefinitions.Item().AlternateUoM = 2;
                //uom.GroupDefinitions.Item(uom).BaseQuantity = dimUomGroupTO.UomGroupConversionTO.BaseQty;
                //uom.GroupDefinitions.Item(uom).AlternateQuantity = dimUomGroupTO.UomGroupConversionTO.AltQty;

                //uom.GroupDefinitions.Add();

                //uomg.GroupDefinitions.Item(collection).AlternateUoM = 2;
                //uomg.GroupDefinitions.Item(collection).BaseQuantity = 20;
                //uomg.GroupDefinitions.Item(collection).AlternateQuantity = 20;

                ouom.Add(uom);

                uom.GroupDefinitions.Add();
                UnitOfMeasurementGroupParams unitOfMeasurementGroupParams = ouom.Add(uom);

                if (unitOfMeasurementGroupParams.Code != null)
                {
                    int x = 0;
                }

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
        public ResultMessage UpdateUOMMasterInSAP(DimUomGroupTO dimUomGroupTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            try
            {
                if (Startup.CompanyObject == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "SAP CompanyObject Found NULL. Connectivity Error. " + Startup.SapConnectivityErrorCode;
                    resultMessage.DisplayMessage = "Error while creating item in SAP with Exception";
                    return resultMessage;
                }

                CompanyService oCompanyService = Startup.CompanyObject.GetCompanyService();
                UnitOfMeasurementGroupsService ouom = (UnitOfMeasurementGroupsService)oCompanyService.GetBusinessService(ServiceTypes.UnitOfMeasurementGroupsService);

                //// Update a UOM Group
                SAPbobsCOM.UnitOfMeasurementGroup uom = (SAPbobsCOM.UnitOfMeasurementGroup)ouom.GetDataInterface
                                 (UnitOfMeasurementGroupsServiceDataInterfaces.uomgsUnitOfMeasurementGroup);

                uom.Code = dimUomGroupTO.UomGroupCode;
                uom.Name = dimUomGroupTO.UomGroupName;
                uom.BaseUoM = dimUomGroupTO.BaseUomId;
                //UnitOfMeasurementGroupParams unitOfMeasurementGroupParams = ouom.Update(uom);

                // if (unitOfMeasurementGroupParams != null)
                //{
                //    int x = 0;
                //}

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

        #region Insertion
        public int InsertDimUomGroup(DimUomGroupTO dimUomGroupTO)
        {
            return _iDimUomGroupDAO.InsertDimUomGroup(dimUomGroupTO);
        }
        public int InsertDimUomGroup(DimUomGroupTO dimUomGroupTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimUomGroupDAO.InsertDimUomGroup(dimUomGroupTO, conn, tran);
        }

        #endregion

        #region Updation
        public int UpdateDimUomGroup(DimUomGroupTO dimUomGroupTO)
        {
            return _iDimUomGroupDAO.UpdateDimUomGroup(dimUomGroupTO);
        }
        public int UpdateDimUomGroup(DimUomGroupTO dimUomGroupTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimUomGroupDAO.UpdateDimUomGroup(dimUomGroupTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteDimUomGroup(Int32 idUomGroup)
        {
            return _iDimUomGroupDAO.DeleteDimUomGroup(idUomGroup);
        }
        public int DeleteDimUomGroup(Int32 idUomGroup, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimUomGroupDAO.DeleteDimUomGroup(idUomGroup, conn, tran);
        }

        public DimUomGroupTO SelectDimUomGroup(int idUomGroup)
        {
            throw new NotImplementedException();
        }

        public DimUomGroupTO SelectDimUomGroupTO(int idUomGroup, SqlConnection conn, SqlTransaction tran)
        {
            throw new NotImplementedException();
        }

        public List<DimUomGroupTO> SelectAllDimUomGroup(DimUomGroupTO dimUomGroupTO)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
