using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;
using simpliMASTERSAPI.MessageQueuePayloads;
using rabbitMessaging;
using AutoMapper;
using System.Threading.Tasks;
using simpliMASTERSAPI.Models;
using Newtonsoft.Json;

namespace ODLMWebAPI.BL
{
    public class DimMstDesignationBL : IDimMstDesignationBL
    {
        private readonly IMessagePublisher _iIMessagePublisher;
        private readonly IDimMstDesignationDAO _iDimMstDesignationDAO;
        private readonly ITblConfigParamsDAO _iTblConfigParamsDAO;
        private readonly IConnectionString _iConnectionString;
        public DimMstDesignationBL(IDimMstDesignationDAO iDimMstDesignationDAO, IConnectionString iConnectionString, ITblConfigParamsDAO iTblConfigParamsDAO, IMessagePublisher imessagePublisher)
        {
            _iConnectionString = iConnectionString;
            _iTblConfigParamsDAO = iTblConfigParamsDAO;
            _iIMessagePublisher = imessagePublisher;
            _iDimMstDesignationDAO = iDimMstDesignationDAO;
        }

        #region Selection

        public List<DimMstDesignationTO> SelectAllDimMstDesignationList()
        {
            return _iDimMstDesignationDAO.SelectAllDimMstDesignation();
        }

        public DimMstDesignationTO SelectDimMstDesignationTO(Int32 idDesignation)
        {
            return _iDimMstDesignationDAO.SelectDimMstDesignation(idDesignation);
        
        }

        public List<DropDownTO> SelectAllDesignationForDropDownList()
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                List<DropDownTO> dropDownTO = _iDimMstDesignationDAO.SelectAllDesignationForDropDownList();
                if (dropDownTO != null)
                    return dropDownTO;
                else
                    return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllDesignationForDropDown");
                return null;
            }
        }



        #endregion


        #region rabbitMigration

        public ResultMessage MigrateAllDesignations()
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            try
            {
                DimMstDesignationTO destTO = null;
                List<DimMstDesignationTO> AlldesignationList = _iDimMstDesignationDAO.SelectAllDimMstDesignation();
                Parallel.For(0, AlldesignationList.Count,
                   index => {
                       try
                       {
                           destTO = AlldesignationList[index];
                           DesignationPayload designationPayload = Mapper.Map<DesignationPayload>(destTO);
                           //add TenantId
                           TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.RABBIT_MQ_CONFIGURATION_DETAILS);
                           TenantTO tenantTO = new TenantTO();
                           if (tblConfigParamsTO != null)
                           {
                               tenantTO = JsonConvert.DeserializeObject<TenantTO>(tblConfigParamsTO.ConfigParamVal.ToString());
                           }
                           designationPayload.TenantId = tenantTO.TenantId;
                           designationPayload.AuthKey = tenantTO.AuthKey;
                           _iIMessagePublisher.PublishMessageAsync(Constants.RABBIT_DESIGNATION_ADD, designationPayload, "");
                       }
                       catch (Exception e)
                       {

                           throw new Exception("Error at  DesignationId" + destTO.IdDesignation);
                       }

                   });

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "Migration Failed");
                return resultMessage;
            }
        }


        #endregion


        #region Insertion
        public int InsertDimMstDesignation(DimMstDesignationTO dimMstDesignationTO)
        {

            Boolean isDuplicateFound = _iDimMstDesignationDAO.IsDuplicateDesignationFound(dimMstDesignationTO.DesignationDesc.Trim());

            if(isDuplicateFound)
            {
                return 5;//considered 5 as a duplicate record found
            } 
            int result= _iDimMstDesignationDAO.InsertDimMstDesignation(dimMstDesignationTO);

            //Hrushikesh added to send Model to hrGrid
            if (result == 1)
            {
                TblConfigParamsTO msgConfigTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_ISRABBITMQ_ENABLED);
                if (msgConfigTO != null && (Convert.ToInt32(msgConfigTO.ConfigParamVal)) == 1)
                {
                    DesignationPayload designationPayload = Mapper.Map<DesignationPayload>(dimMstDesignationTO);
                    //add TenantId
                    TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.RABBIT_MQ_CONFIGURATION_DETAILS);
                    TenantTO tenantTO = new TenantTO();
                    if (tblConfigParamsTO != null)
                    {
                        tenantTO = JsonConvert.DeserializeObject<TenantTO>(tblConfigParamsTO.ConfigParamVal.ToString());
                    }
                    designationPayload.TenantId = tenantTO.TenantId;
                    designationPayload.AuthKey = tenantTO.AuthKey;
                    _iIMessagePublisher.PublishMessageAsync(Constants.RABBIT_DESIGNATION_ADD, designationPayload, "");
                }
            }
            return result;
        }

        public int InsertDimMstDesignation(DimMstDesignationTO dimMstDesignationTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimMstDesignationDAO.InsertDimMstDesignation(dimMstDesignationTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateDimMstDesignation(DimMstDesignationTO dimMstDesignationTO)
        {
            int result = _iDimMstDesignationDAO.UpdateDimMstDesignation(dimMstDesignationTO);
            //Hrushikesh added to send Model to hrGrid
            if (result == 1)
            {
                TblConfigParamsTO msgConfigTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_ISRABBITMQ_ENABLED);
                if (msgConfigTO != null && (Convert.ToInt32(msgConfigTO.ConfigParamVal)) == 1)
                {
                    DesignationPayload designationPayload = Mapper.Map<DesignationPayload>(dimMstDesignationTO);
                    //add TenantId
                    TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.RABBIT_MQ_CONFIGURATION_DETAILS);
                    TenantTO tenantTO = new TenantTO();
                    if (tblConfigParamsTO != null)
                    {
                        tenantTO = JsonConvert.DeserializeObject<TenantTO>(tblConfigParamsTO.ConfigParamVal.ToString());
                    }
                    designationPayload.TenantId = tenantTO.TenantId;
                    designationPayload.AuthKey = tenantTO.AuthKey;
                    if (designationPayload.IsVisible == 0)
                        _iIMessagePublisher.PublishMessageAsync(Constants.RABBIT_DESIGNATION__DEACTIVATED, designationPayload, "");
                    else
                        _iIMessagePublisher.PublishMessageAsync(Constants.RABBIT_DESIGNATION_UPDATE, designationPayload, "");
                }
            }
            return result;
        }

        public int UpdateDimMstDesignation(DimMstDesignationTO dimMstDesignationTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimMstDesignationDAO.UpdateDimMstDesignation(dimMstDesignationTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteDimMstDesignation(Int32 idDesignation)
        {
            return _iDimMstDesignationDAO.DeleteDimMstDesignation(idDesignation);
        }

        public int DeleteDimMstDesignation(Int32 idDesignation, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimMstDesignationDAO.DeleteDimMstDesignation(idDesignation, conn, tran);
        }

        #endregion
        
    }
}
