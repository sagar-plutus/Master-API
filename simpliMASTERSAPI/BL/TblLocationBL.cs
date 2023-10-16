using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.StaticStuff;
using SAPbobsCOM;
using simpliMASTERSAPI;

namespace ODLMWebAPI.BL
{
    public class TblLocationBL : ITblLocationBL
    {
        private readonly ITblLocationDAO _iTblLocationDAO;
        private readonly IConnectionString _iConnectionString;
        private readonly ICommon _iCommon;
        private readonly ITblConfigParamsDAO _iTblConfigParamsDAO;
        public TblLocationBL(ITblConfigParamsDAO iTblConfigParamsDAO, ICommon iCommon, IConnectionString iConnectionString, ITblLocationDAO iTblLocationDAO)
        {
            _iTblLocationDAO = iTblLocationDAO;
            _iConnectionString = iConnectionString;
            _iCommon = iCommon;
            _iTblConfigParamsDAO = iTblConfigParamsDAO;
        }
        #region Selection

        public List<TblLocationTO> SelectAllTblLocationList()
        {
            //return _iTblLocationDAO.SelectAllTblLocation();
            //List<TblLocationTO> locationReturnList = new List<TblLocationTO>();
            List<TblLocationTO> locationList = _iTblLocationDAO.SelectAllTblLocation();
            if (locationList != null && locationList.Count > 0)
            {
                for (int i = 0; i < locationList.Count; i++)
                {
                    if (locationList[i].ParentLocId > 0)
                    {
                        locationList[i].DisplayLocationDesc = locationList[i].ParentLocationDesc + "-" + locationList[i].LocationDesc;
                    }
                }
            }
            return locationList;
        }

        public List<TblLocationTO> SelectAllCompartmentLocationList(Int32 parentLocationId)
        {
            return _iTblLocationDAO.SelectAllTblLocation(parentLocationId);
        }
        //Reshma Added For Fiding location from warehouse
        public List<DropDownTO> SelectLocationFromWarehouse(Int32 warehouseId)
        {
            return _iTblLocationDAO.SelectLocationFromWarehouse(warehouseId );
        }
        public List<DropDownTO> SelectAllLocationAndCompartmentsListForDropDown(Boolean onlyCompartments = true)
        {
            return _iTblLocationDAO.SelectAllLocationAndCompartmentsListForDropDown(onlyCompartments);
        }

        public List<TblLocationTO> SelectAllParentLocation()
        {
            return _iTblLocationDAO.SelectAllParentLocation();
        }
      
        public TblLocationTO SelectTblLocationTO(Int32 idLocation)
        {
            return  _iTblLocationDAO.SelectTblLocation(idLocation);
        }

        /// <summary>
        /// Sanjay [2017-05-03] To Get All the compartment whose stock for the given date is not taken
        /// </summary>
        /// <param name="stockDate"></param>
        /// <returns></returns>
        public List<TblLocationTO> SelectStkNotTakenCompartmentList(DateTime stockDate)
        {
            return _iTblLocationDAO.SelectStkNotTakenCompartmentList(stockDate);

        }
        public List<TblLocationTO> SelectAllTblLocationList(TblLocationTO tblLocationTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLocationDAO.SelectAllParentLocationWithConnTran(tblLocationTO, conn, tran);
        }
        public ResultMessage SaveAndUpdateLocationMaster(TblLocationTO tblLocationTO, Int32 loginUserId)
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
                List<TblLocationTO> list = _iTblLocationDAO.SelectAllParentLocationWithConnTran(tblLocationTO, conn, tran);
                if (list != null && list.Count > 0)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Location Already Exist";
                    resultMessage.DisplayMessage = "Location Already Exist";
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                tblLocationTO.IsActive = 1;

                if (tblLocationTO.IdLocation > 0)
                {
                    tblLocationTO.UpdatedOn = _iCommon.ServerDateTime;
                    tblLocationTO.UpdatedBy = Convert.ToInt32(loginUserId);
                    tblLocationTO.UpdatedBy = Convert.ToInt32(loginUserId);
                    result = _iTblLocationDAO.UpdateTblLocation(tblLocationTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Text = "Error While UpdateTblLocation";
                        resultMessage.DisplayMessage = "Record Cound Not Saved";
                        return resultMessage;

                    }

                    TblConfigParamsTO tblConfigParamsTOSAPService = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.SAPB1_SERVICES_ENABLE);
                    if (tblConfigParamsTOSAPService != null)
                    {
                        if (Convert.ToInt32(tblConfigParamsTOSAPService.ConfigParamVal) == 1)
                        {
                            ResultMessage sapResult = UpdateLocationMasterInSAP(tblLocationTO);
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
                     
                    }
                    tran.Commit();
                    resultMessage.DefaultSuccessBehaviour();
                 
                    return resultMessage;

                }
                else
                {
                    tblLocationTO.CreatedOn = _iCommon.ServerDateTime;
                    tblLocationTO.CreatedBy = Convert.ToInt32(loginUserId);
                    result = _iTblLocationDAO.InsertTblLocation(tblLocationTO, conn, tran);
                    if (result != 1)
                    {

                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Text = "Error While InsertTblLocation";
                        resultMessage.DisplayMessage = "Record Cound Not Saved";
                        return resultMessage;

                    }

                    TblConfigParamsTO tblConfigParamsTOSAPService = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.SAPB1_SERVICES_ENABLE);
                    if (tblConfigParamsTOSAPService != null)

                    {

                        if (Convert.ToInt32(tblConfigParamsTOSAPService.ConfigParamVal) == 1)
                        {
                            ResultMessage sapResult = SaveLocationMasterInSAP(tblLocationTO);
                            if (sapResult.Result == 1)
                            {
                                tblLocationTO.MappedTxnId = sapResult.Tag.ToString();
                                tblLocationTO.UpdatedOn = _iCommon.ServerDateTime;
                                tblLocationTO.UpdatedBy = Convert.ToInt32(loginUserId);
                                result = _iTblLocationDAO.UpdateTblLocation(tblLocationTO, conn, tran);
                                if (result != 1)
                                {
                                    tran.Rollback();
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    resultMessage.Text = "Error While UpdateTblLocation";
                                    resultMessage.DisplayMessage = "Record Cound Not Saved";
                                    return resultMessage;

                                }

                                tran.Commit();
                                return sapResult;
                            }
                            else
                            {
                                tran.Rollback();
                                return sapResult;
                            }
                        }                       
                    }

                    tran.Commit();
                    resultMessage.MessageType = ResultMessageE.Information;
                    resultMessage.Result = 1;
                    resultMessage.Text = "Success... Location Saved";
                    resultMessage.DisplayMessage = "Success... Location Saved";


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
        /// Priyanka [01-05-2019] : Added to save Location into SAP.
        /// </summary>
        /// <param name="tblLocationTO"></param>
        /// <returns></returns>
        public ResultMessage SaveLocationMasterInSAP(TblLocationTO tblLocationTO)
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

               
                if (tblLocationTO.ParentLocId != 0)
                {
                    //Add Warehouse
                    SAPbobsCOM.Warehouses owarehouse;
                    owarehouse = Startup.CompanyObject.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oWarehouses);
                    owarehouse.WarehouseName = tblLocationTO.LocationDesc;
                    owarehouse.WarehouseCode = tblLocationTO.IdLocation.ToString();
                    owarehouse.Location = Convert.ToInt32(tblLocationTO.ParentMappedTxnId);
                   
                    result = owarehouse.Add();
                    if(result!=0)
                    {
                        string sapErrorMsg = Startup.CompanyObject.GetLastErrorDescription();
                        resultMessage.DefaultBehaviour();
                        resultMessage.Text = sapErrorMsg;
                        resultMessage.DisplayMessage = "Error while creating Warehouse in SAP";
                        return resultMessage;
                    }
                    else
                    {
                        resultMessage.DefaultSuccessBehaviour();
                        resultMessage.Tag = owarehouse.WarehouseCode;
                        return resultMessage;
                    }
                }
                else
                {
                    //Add Location
                    SAPbobsCOM.WarehouseLocations oLocation;
                    oLocation = Startup.CompanyObject.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oWarehouseLocations);
                    oLocation.Name = tblLocationTO.LocationDesc;
                    oLocation.State = tblLocationTO.StateName;
                    oLocation.Country = tblLocationTO.CountryName;
                    result = oLocation.Add();
                    if (result != 0)
                    {
                        string sapErrorMsg = Startup.CompanyObject.GetLastErrorDescription();
                        resultMessage.DefaultBehaviour();
                        resultMessage.Text = sapErrorMsg;
                        resultMessage.DisplayMessage = "Error while creating Location in SAP";
                        return resultMessage;
                    }
                    else
                    {
                        resultMessage.DefaultSuccessBehaviour();
                        resultMessage.Tag = Startup.CompanyObject.GetNewObjectKey();
                        return resultMessage;
                    }
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
        /// <summary>
        /// Priyanka [01-05-2019] : Added to save Location into SAP.
        /// </summary>
        /// <param name="tblLocationTO"></param>
        /// <returns></returns>
        public ResultMessage UpdateLocationMasterInSAP(TblLocationTO tblLocationTO)
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


                if (tblLocationTO.ParentLocId != 0)
                {
                    //Add Warehouse
                    SAPbobsCOM.Warehouses owarehouse;
                    owarehouse = Startup.CompanyObject.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oWarehouses);
                    if (owarehouse.GetByKey(tblLocationTO.MappedTxnId))
                    {
                        owarehouse.WarehouseName = tblLocationTO.LocationDesc;
                        owarehouse.WarehouseCode = tblLocationTO.IdLocation.ToString();
                        owarehouse.Location = Convert.ToInt32(tblLocationTO.ParentMappedTxnId);
                        result = owarehouse.Update();
                    }
                    else
                    {
                        resultMessage.DefaultBehaviour();
                        string sapErrorMsg = "Error while retriving SAP Data by Key Before Update";
                        resultMessage.Text = sapErrorMsg;
                        resultMessage.DisplayMessage = "Error while Updating item in SAP";
                    }
                }
                else
                {
                    //update Location
                    SAPbobsCOM.WarehouseLocations oLocation;
                    oLocation = Startup.CompanyObject.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oWarehouseLocations);

                    if (oLocation.GetByKey(Convert.ToInt32(tblLocationTO.MappedTxnId)))
                    {
                        oLocation.Name = tblLocationTO.LocationDesc;
                        oLocation.State = tblLocationTO.StateName;
                        oLocation.Country = tblLocationTO.CountryName;

                        result = oLocation.Update();
                    }
                    else
                    {
                        resultMessage.DefaultBehaviour();
                        string sapErrorMsg = "Error while retriving SAP Data by Key Before Update";
                        resultMessage.Text = sapErrorMsg;
                        resultMessage.DisplayMessage = "Error while Updating item in SAP";
                    }
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

        #endregion

        #region Insertion
        public int InsertTblLocation(TblLocationTO tblLocationTO)
        {
            return _iTblLocationDAO.InsertTblLocation(tblLocationTO);
        }

        public int InsertTblLocation(TblLocationTO tblLocationTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLocationDAO.InsertTblLocation(tblLocationTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblLocation(TblLocationTO tblLocationTO)
        {
            return _iTblLocationDAO.UpdateTblLocation(tblLocationTO);
        }

        public int UpdateTblLocation(TblLocationTO tblLocationTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLocationDAO.UpdateTblLocation(tblLocationTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblLocation(Int32 idLocation)
        {
            return _iTblLocationDAO.DeleteTblLocation(idLocation);
        }

        public int DeleteTblLocation(Int32 idLocation, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLocationDAO.DeleteTblLocation(idLocation, conn, tran);
        }

        #endregion
        
    }
}
