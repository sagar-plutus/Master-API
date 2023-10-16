using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;
using SAPbobsCOM;
using simpliMASTERSAPI;
namespace ODLMWebAPI.BL
{  
    public class DimStateBL : IDimStateBL
    {
        private readonly IDimStateDAO _iDimStateDAO;
        private readonly ITblConfigParamsDAO _iTblConfigParamsDAO;
        private readonly IConnectionString _iConnectionString;
        private readonly IDimensionDAO _iDimensionDAO;
        public DimStateBL(IDimensionDAO iDimensionDAO, IDimStateDAO iDimStateDAO, ITblConfigParamsDAO iTblConfigParamsDAO, IConnectionString iConnectionString)
        {
            _iDimStateDAO = iDimStateDAO;
            _iTblConfigParamsDAO = iTblConfigParamsDAO;
            _iConnectionString = iConnectionString;
            _iDimensionDAO = iDimensionDAO;
        }
        #region Selection
        public List<DimStateTO> SelectAllDimState()
        {
            return _iDimStateDAO.SelectAllDimState();
        }

        //public List<DimStateTO> SelectAllDimStateList()
        //{
        //    List<DimStateTO> dimStateTODT = _iDimStateDAO.SelectAllDimState();
        //    return dimStateTODT;
        //}

        public DimStateTO SelectDimStateTO(Int32 idState)
        {
            DimStateTO dimStateTO = _iDimStateDAO.SelectDimState(idState);
            if (dimStateTO != null)
                return dimStateTO;
            else
                return null;
        }

        public List<DimStateTO> ConvertDTToList(DataTable dimStateTODT)
        {
            List<DimStateTO> dimStateTOList = new List<DimStateTO>();
            if (dimStateTODT != null)
            {
             
            }
            return dimStateTOList;
        }

        #endregion

        #region Insertion
        public int InsertDimState(DimStateTO dimStateTO)
        {
            return _iDimStateDAO.InsertDimState(dimStateTO);
        }

        public int InsertDimState(DimStateTO dimStateTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimStateDAO.InsertDimState(dimStateTO, conn, tran);
        }

        //Sudhir[09-12-2017] Added for SaveNewState.
        public ResultMessage SaveNewState(DimStateTO dimStateTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            conn.Open();
            SqlTransaction tran = conn.BeginTransaction();

            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            try
            {
                result = InsertDimState(dimStateTO, conn, tran); // result = lastInsertId
                if (result == 0)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("SaveNewState");
                    return resultMessage;
                }
                TblConfigParamsTO tblConfigParamsTOSAPService = _iTblConfigParamsDAO.SelectTblConfigParams(Constants.SAPB1_SERVICES_ENABLE);
                if (tblConfigParamsTOSAPService != null)
                {
                    if (Convert.ToInt32(tblConfigParamsTOSAPService.ConfigParamVal) == 1)
                    {
                        #region Add State Integration In SAP
                        CompanyService oCompanyService = Startup.CompanyObject.GetCompanyService();
                        StatesService oStatesService = (StatesService)oCompanyService.GetBusinessService(ServiceTypes.StatesService);
                        SAPbobsCOM.State oState = (SAPbobsCOM.State)oStatesService.GetDataInterface(StatesServiceDataInterfaces.ssState);
                        oState.Code = dimStateTO.StateCode;
                        oState.Country = dimStateTO.CountryCode;
                        oState.Name = dimStateTO.StateName;
                        StateParams stateParams = oStatesService.AddState(oState);
                        if (!String.IsNullOrEmpty(stateParams.Code))
                        {
                            string queryString = "UPDATE dimState SET mappedTxnId = '" + stateParams.Code + "' where idState = " + result;
                            result = _iDimensionDAO.InsertdimentionalData(queryString, false, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour("SaveNewState");
                                return resultMessage;
                            }
                        }
                        else
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("SaveNewState");
                            return resultMessage;
                        }
                        #endregion
                    }
                }
                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "SaveNewState");
                return resultMessage;
            }
            finally
            {

            }
        }
        public ResultMessage UpdateState(DimStateTO dimStateTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            conn.Open();
            SqlTransaction tran = conn.BeginTransaction();

            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            try
            {
                result = UpdateDimState(dimStateTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("UpdateState");
                    return resultMessage;
                }
                TblConfigParamsTO tblConfigParamsTOSAPService = _iTblConfigParamsDAO.SelectTblConfigParams(Constants.SAPB1_SERVICES_ENABLE);
                if (tblConfigParamsTOSAPService != null)
                {
                    if (Convert.ToInt32(tblConfigParamsTOSAPService.ConfigParamVal) == 1)
                    {
                        #region Add State Integration In SAP
                      

                        CompanyService oCompanyService = Startup.CompanyObject.GetCompanyService();
                        StatesService oStatesService = (StatesService)oCompanyService.GetBusinessService(ServiceTypes.StatesService);
                        SAPbobsCOM.State oState = (SAPbobsCOM.State)oStatesService.GetDataInterface(StatesServiceDataInterfaces.ssState);
                        SAPbobsCOM.StateParams paramss = (SAPbobsCOM.StateParams)oStatesService.GetDataInterface(StatesServiceDataInterfaces.ssStateParams);
                        paramss.Code = dimStateTO.MappedTxnId;
                         paramss.Country = dimStateTO.CountryCode;
                        oState =oStatesService.GetState(paramss);
                        oState.Code = dimStateTO.StateCode;
                  
                        oState.Name = dimStateTO.StateName;
                        oStatesService.UpdateState(oState);
                        
                        #endregion
                    }
                }

                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateState");
                return resultMessage;
            }
            finally
            {

            }
        }
        #endregion

        #region Updation
        public int UpdateDimState(DimStateTO dimStateTO)
        {
            return _iDimStateDAO.UpdateDimState(dimStateTO);
        }

        public int UpdateDimState(DimStateTO dimStateTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimStateDAO.UpdateDimState(dimStateTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteDimState(Int32 idState)
        {
            return _iDimStateDAO.DeleteDimState(idState);
        }

        public int DeleteDimState(Int32 idState, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimStateDAO.DeleteDimState(idState, conn, tran);
        }

        #endregion

    }
}

