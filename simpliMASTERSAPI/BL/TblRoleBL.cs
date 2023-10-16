using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using ODLMWebAPI.DAL;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;
using simpliMASTERSAPI.MessageQueuePayloads;
using AutoMapper;
using rabbitMessaging;
using Newtonsoft.Json;
using simpliMASTERSAPI.Models;

namespace ODLMWebAPI.BL
{
    public class TblRoleBL : ITblRoleBL
    {
        private readonly ITblConfigParamsDAO _iTblConfigParamsDAO;
        private readonly ITblRoleDAO _iTblRoleDAO;
        private readonly IConnectionString _iConnectionString;
        private readonly IMessagePublisher _iMessagePublisher;
        private readonly ICommon _iCommon;
        public TblRoleBL(ICommon iCommon, ITblRoleDAO iTblRoleDAO, IConnectionString iConnectionString, ITblConfigParamsDAO iTblConfigParamsDAO, IMessagePublisher iMessagePublisher)
        {
            _iConnectionString = iConnectionString;
            _iTblConfigParamsDAO = iTblConfigParamsDAO;
            _iMessagePublisher = iMessagePublisher;
            _iTblRoleDAO = iTblRoleDAO;
            _iCommon = iCommon;
        }
        #region Selection
        public TblRoleTO SelectAllTblRole()
        {
            return _iTblRoleDAO.SelectAllTblRole();
        }
        public List<TblRoleTO> GetAllRoleList()
        {
            return _iTblRoleDAO.GetAllRoleList();
        }
        public List<TblRoleTO> SelectAllTblRoleList()
        {
            TblRoleTO tblRoleTODT = _iTblRoleDAO.SelectAllTblRole();
            return ConvertDTToList(tblRoleTODT);
        }

        #region rabbitMigration
        public ResultMessage MigrateAllRoles()
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            try
            {
                RolePayload roleTO = null;
                List<RolePayload> allRoleList = _iTblRoleDAO.selectRolePayLoadList();
                //Parallel.For(0, allRoleList.Count,
                //   index => {
                //       try
                //       {
                //           roleTO = allRoleList[index];
                //           //add tenentId
                //           roleTO.TenantId = _iConnectionString.GetConnectionString(Constants.TENANT_ID);

                //           _iMessagePublisher.PublishMessageAsync(Constants.RABBIT_ROLE_ADD, roleTO, "");
                //       }
                //       catch (Exception e)
                //       {

                //           throw new Exception("Error at  RoleId" + roleTO.IdRole);
                //       }

                //   });
                for (int index = 0; index < allRoleList.Count; index++)
                {
                    try
                    {
                        roleTO = allRoleList[index];
                        //add tenentId
                        TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.RABBIT_MQ_CONFIGURATION_DETAILS);
                        TenantTO tenantTO = new TenantTO();
                        if (tblConfigParamsTO != null)
                        {
                            tenantTO = JsonConvert.DeserializeObject<TenantTO>(tblConfigParamsTO.ConfigParamVal.ToString());
                        }
                        roleTO.TenantId = tenantTO.TenantId;
                        roleTO.AuthKey = tenantTO.AuthKey;
                        _iMessagePublisher.PublishMessageAsync(Constants.RABBIT_ROLE_ADD, roleTO, "");
                    }
                    catch (Exception e)
                    {

                        throw new Exception("Error at  RoleId" + roleTO.IdRole);
                    }
                }



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



        public TblRoleTO SelectTblRoleTO(Int32 idRole)
        {
            return _iTblRoleDAO.SelectTblRole(idRole);
           
        }

        public List<TblRoleTO> ConvertDTToList(TblRoleTO tblRoleTODT)
        {
            List<TblRoleTO> tblRoleTOList = new List<TblRoleTO>();
            if (tblRoleTODT != null)
            {
            }
            return tblRoleTOList;
        }

        public TblRoleTO SelectTblRoleOnOrgStructureId(Int32 orgStructutreId)
        {
            TblRoleTO tblRoleTODT = _iTblRoleDAO.SelectTblRoleOnOrgStructureId(orgStructutreId);
            if (tblRoleTODT != null)
                return tblRoleTODT;
            else
                return null;
        }


        /// <summary>
        /// Sudhir[22-AUG-2018] Added Connection ,Transaction
        /// </summary>
        /// <param name="orgStructutreId"></param>
        /// <returns></returns>
        public TblRoleTO SelectTblRoleOnOrgStructureId(Int32 orgStructutreId, SqlConnection conn, SqlTransaction tran)
        {
            TblRoleTO tblRoleTODT = _iTblRoleDAO.SelectTblRoleOnOrgStructureId(orgStructutreId, conn, tran);
            if (tblRoleTODT != null)
                return tblRoleTODT;
            else
                return null;
        }

        public TblRoleTO getDepartmentIdFromUserId(Int32 userId)
        {
            TblRoleTO tblRoleTODT = _iTblRoleDAO.getDepartmentIdFromUserId(userId);
            if (tblRoleTODT != null)
                return tblRoleTODT;
            else
                return null;
        }


        #endregion

        #region Insertion
        public int InsertTblRole(TblRoleTO tblRoleTO)
        {
            return _iTblRoleDAO.InsertTblRole(tblRoleTO);
        }

        public int InsertTblRole(TblRoleTO tblRoleTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblRoleDAO.InsertTblRole(tblRoleTO, conn, tran);
        }

        #endregion

        #region Updation
        public int UpdateTblRole(TblRoleTO tblRoleTO)
        {
            return _iTblRoleDAO.UpdateTblRole(tblRoleTO);
        }

        public int UpdateTblRole(TblRoleTO tblRoleTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblRoleDAO.UpdateTblRole(tblRoleTO, conn, tran);
        }


        public ResultMessage UpdateRoleType(TblOrgStructureTO tblOrgStructureTO)
        {
            Int32 result = 0;
            ResultMessage resultMessage = new ResultMessage();

            TblRoleTO tblRoleTO = SelectTblRoleOnOrgStructureId(tblOrgStructureTO.IdOrgStructure);
            if (tblRoleTO != null)
            {
                tblRoleTO.RoleTypeId = tblOrgStructureTO.RoleTypeId;
                result = UpdateTblRole(tblRoleTO);
                if (result < 1)
                {
                    //tran.Rollback();
                    resultMessage.DefaultBehaviour("Error While Updating Role Type");
                   // return resultMessage;
                }

                #region rabbitMessaging
                TblConfigParamsTO msgConfigTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_ISRABBITMQ_ENABLED);
                if (msgConfigTO != null && (Convert.ToInt32(msgConfigTO.ConfigParamVal)) == 1)
                {
                    TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.RABBIT_MQ_CONFIGURATION_DETAILS);
                TenantTO tenantTO = new TenantTO();
                if (tblConfigParamsTO != null)
                {
                    tenantTO = JsonConvert.DeserializeObject<TenantTO>(tblConfigParamsTO.ConfigParamVal.ToString());
                }
                
                    RolePayload rolePayload = Mapper.Map<RolePayload>(tblRoleTO);
                    rolePayload.DeptId = tblOrgStructureTO.DeptId;
                    rolePayload.DesignationId = tblOrgStructureTO.DesignationId;
                    //add tenentId
                    rolePayload.TenantId = tenantTO.TenantId;
                    rolePayload.AuthKey = tenantTO.AuthKey;
                    _iMessagePublisher.PublishMessageAsync(Constants.RABBIT_ROLE_UPDATE,
                        rolePayload, "");
                }

                #endregion
                resultMessage.DefaultSuccessBehaviour();

               // return resultMessage;
            }
            return resultMessage;
        }
        #endregion
        #region Update 
        public int UpdateRoleSettings(TblRoleTO tblRoleTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblRoleDAO.UpdateRoleSettings(tblRoleTO, conn, tran);
        }
        public ResultMessage UpdateRoleSettings(List<TblRoleTO> tblRoleTOList)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlTransaction tran = null;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                DateTime ServerDateTime = _iCommon.ServerDateTime;
                Int32 result = 0;
                #region Update UserSetting
                if (tblRoleTOList == null || tblRoleTOList.Count == 0)
                {
                    resultMessage.DefaultBehaviour("Invalid Role Setting List");
                    return resultMessage;
                }
                conn.Open();
                tran = conn.BeginTransaction();
                for (int i = 0; i < tblRoleTOList.Count; i++)
                {
                    TblRoleTO tblRoleTO = tblRoleTOList[i];
                    tblRoleTO.UpdatedOn = ServerDateTime;
                    result = UpdateRoleSettings(tblRoleTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Failed to updated Role setting Details.Role Id - " + tblRoleTO.IdRole);
                        return resultMessage;
                    }
                }
                #endregion
                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "");
                return resultMessage;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }
        #endregion
        #region Deletion
        public int DeleteTblRole(Int32 idRole)
        {
            return _iTblRoleDAO.DeleteTblRole(idRole);
        }

        public int DeleteTblRole(Int32 idRole, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblRoleDAO.DeleteTblRole(idRole, conn, tran);
        }

        #endregion

    }
}
