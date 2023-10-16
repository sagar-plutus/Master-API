using ODLMWebAPI.Models;
using simpliMASTERSAPI.DAL.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.BL.Interfaces;

namespace simpliMASTERSAPI.DAL
{
    public class TblLoadingDAO : ITblLoadingDAO
    {
        private readonly ITblModuleBL _tblModuleBL;
        public TblLoadingDAO(ITblModuleBL tblModuleBL) 
        {
            _tblModuleBL = tblModuleBL;
        }
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT loading.* , fromOrgNameTbl.firmName as fromOrgName ,org.digitalSign, org.firmName as cnfOrgName, org.isInternalCnf ,transOrg.firmName as transporterOrgName ,dimStat.statusName ,ISNULL(person.firstName,'') + ' ' + ISNULL(person.lastName,'') AS superwisorName    " +

                                  " ,createdUser.userDisplayName ,tblUserCallFlag.userDisplayName AS notifyByName" +
                                  " , tblGate.portNumber, tblGate.IoTUrl, tblGate.machineIP " +
                                  " FROM tempLoading loading " +

                                  " LEFT JOIN tblOrganization org ON org.idOrganization = loading.cnfOrgId " +
                                  " LEFT JOIN dimStatus dimStat ON dimStat.idStatus = loading.statusId " +
                                  " LEFT JOIN tblSupervisor superwisor ON superwisor.idSupervisor=loading.superwisorId " +
                                  " LEFT JOIN tblPerson person ON superwisor.personId = person.idPerson" +
                                  " LEFT JOIN tblOrganization transOrg ON transOrg.idOrganization = loading.transporterOrgId " +
                                  " LEFT JOIN tblUser tblUserCallFlag ON tblUserCallFlag.idUser=loading.callFlagBy " +

                                  " LEFT JOIN tblUser createdUser ON createdUser.idUser=loading.createdBy" +
                                  " LEFT JOIN tblGate tblGate ON tblGate.idGate=loading.gateId " +
                                  //Prajakta [2021-06-29] Added to show orgName on loading slip
                                  " LEFT JOIN tblOrganization fromOrgNameTbl on fromOrgNameTbl.idOrganization = loading.fromOrgId " +
                                // Vaibhav [20-Nov-2017] added to select from finalLoading
                                " UNION ALL " +

                                 " SELECT loading.*, fromOrgNameTbl.firmName as fromOrgName ,org.digitalSign, org.firmName as cnfOrgName,org.isInternalCnf,transOrg.firmName as transporterOrgName ,dimStat.statusName ,ISNULL(person.firstName,'') + ' ' + ISNULL(person.lastName,'') AS superwisorName    " +
                                 " ,createdUser.userDisplayName ,tblUserCallFlag.userDisplayName AS notifyByName" +
                                 " , tblGate.portNumber, tblGate.IoTUrl, tblGate.machineIP " +
                                 " FROM finalLoading loading " +
                                 " LEFT JOIN tblOrganization org ON org.idOrganization = loading.cnfOrgId " +
                                 " LEFT JOIN dimStatus dimStat ON dimStat.idStatus = loading.statusId " +
                                 " LEFT JOIN tblSupervisor superwisor ON superwisor.idSupervisor=loading.superwisorId " +
                                 " LEFT JOIN tblPerson person ON superwisor.personId = person.idPerson" +
                                 " LEFT JOIN tblOrganization transOrg ON transOrg.idOrganization = loading.transporterOrgId " +
                                 " LEFT JOIN tblUser tblUserCallFlag ON tblUserCallFlag.idUser=loading.callFlagBy " +
                                 " LEFT JOIN tblUser createdUser ON createdUser.idUser=loading.createdBy" +
                                 " LEFT JOIN tblGate tblGate ON tblGate.idGate=loading.gateId " +
                                  //Prajakta [2021-06-29] Added to show orgName on loading slip
                                  " LEFT JOIN tblOrganization fromOrgNameTbl on fromOrgNameTbl.idOrganization = loading.fromOrgId ";

            return sqlSelectQry;
        }
        public  List<TblLoadingTO> SelectAllLoadingListByStatus(string statusId, SqlConnection conn, SqlTransaction tran, int gateId = 0)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                int modeId = getModeIdConfigTO();
                //Vaibhav [21-Nov-2017] Changed for data separation ativity
                if (gateId == 0)
                    cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE sq1.statusId IN(" + statusId + ")";
                else
                    cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE sq1.statusId IN(" + statusId + ") AND sq1.gateId = " + gateId;

                if (modeId > 0)
                {
                    cmdSelect.CommandText += " AND ISNULL(sq1.modeId,1) =" + modeId;
                }
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {

                return null;
            }
            finally
            {
                sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public int getModeIdConfigTO()
        {
            Int32 modeId = 1;
            TblModuleTO tblModuleTo = _tblModuleBL.SelectTblModuleTO(Constants.DefaultModuleID);
            if (tblModuleTo != null)
            {
                modeId = Convert.ToInt32(tblModuleTo.ModeId);
            }
            return modeId;
        }
        public static List<TblLoadingTO> ConvertDTToList(SqlDataReader tblLoadingTODT)
        {
            List<TblLoadingTO> tblLoadingTOList = new List<TblLoadingTO>();
            if (tblLoadingTODT != null)
            {
                while (tblLoadingTODT.Read())
                {
                    TblLoadingTO tblLoadingTONew = new TblLoadingTO();
                    if (tblLoadingTODT["idLoading"] != DBNull.Value)
                        tblLoadingTONew.IdLoading = Convert.ToInt32(tblLoadingTODT["idLoading"].ToString());
                    if (tblLoadingTODT["isJointDelivery"] != DBNull.Value)
                        tblLoadingTONew.IsJointDelivery = Convert.ToInt32(tblLoadingTODT["isJointDelivery"].ToString());
                    if (tblLoadingTODT["noOfDeliveries"] != DBNull.Value)
                        tblLoadingTONew.NoOfDeliveries = Convert.ToInt32(tblLoadingTODT["noOfDeliveries"].ToString());
                    if (tblLoadingTODT["statusId"] != DBNull.Value)
                        tblLoadingTONew.StatusId = Convert.ToInt32(tblLoadingTODT["statusId"].ToString());
                    if (tblLoadingTODT["createdBy"] != DBNull.Value)
                        tblLoadingTONew.CreatedBy = Convert.ToInt32(tblLoadingTODT["createdBy"].ToString());
                    if (tblLoadingTODT["updatedBy"] != DBNull.Value)
                        tblLoadingTONew.UpdatedBy = Convert.ToInt32(tblLoadingTODT["updatedBy"].ToString());
                    if (tblLoadingTODT["statusDate"] != DBNull.Value)
                        tblLoadingTONew.StatusDate = Convert.ToDateTime(tblLoadingTODT["statusDate"].ToString());
                    if (tblLoadingTODT["loadingDatetime"] != DBNull.Value)
                        tblLoadingTONew.LoadingDatetime = Convert.ToDateTime(tblLoadingTODT["loadingDatetime"].ToString());
                    if (tblLoadingTODT["createdOn"] != DBNull.Value)
                        tblLoadingTONew.CreatedOn = Convert.ToDateTime(tblLoadingTODT["createdOn"].ToString());
                    if (tblLoadingTODT["updatedOn"] != DBNull.Value)
                        tblLoadingTONew.UpdatedOn = Convert.ToDateTime(tblLoadingTODT["updatedOn"].ToString());
                    if (tblLoadingTODT["loadingSlipNo"] != DBNull.Value)
                        tblLoadingTONew.LoadingSlipNo = Convert.ToString(tblLoadingTODT["loadingSlipNo"].ToString());
                    if (tblLoadingTODT["vehicleNo"] != DBNull.Value)
                        tblLoadingTONew.VehicleNo = Convert.ToString(tblLoadingTODT["vehicleNo"].ToString().ToUpper());
                    if (tblLoadingTODT["statusReason"] != DBNull.Value)
                        tblLoadingTONew.StatusReason = Convert.ToString(tblLoadingTODT["statusReason"].ToString());

                    if (tblLoadingTODT["cnfOrgId"] != DBNull.Value)
                        tblLoadingTONew.CnfOrgId = Convert.ToInt32(tblLoadingTODT["cnfOrgId"].ToString());
                    if (tblLoadingTODT["cnfOrgName"] != DBNull.Value)
                        tblLoadingTONew.CnfOrgName = Convert.ToString(tblLoadingTODT["cnfOrgName"].ToString());
                    if (tblLoadingTODT["totalLoadingQty"] != DBNull.Value)
                        tblLoadingTONew.TotalLoadingQty = Convert.ToDouble(tblLoadingTODT["totalLoadingQty"].ToString());

                    if (tblLoadingTODT["statusName"] != DBNull.Value)
                        tblLoadingTONew.StatusDesc = Convert.ToString(tblLoadingTODT["statusName"].ToString());
                    if (tblLoadingTODT["statusReasonId"] != DBNull.Value)
                        tblLoadingTONew.StatusReasonId = Convert.ToInt32(tblLoadingTODT["statusReasonId"].ToString());
                    if (tblLoadingTODT["transporterOrgId"] != DBNull.Value)
                        tblLoadingTONew.TransporterOrgId = Convert.ToInt32(tblLoadingTODT["transporterOrgId"].ToString());
                    if (tblLoadingTODT["freightAmt"] != DBNull.Value)
                        tblLoadingTONew.FreightAmt = Convert.ToDouble(tblLoadingTODT["freightAmt"].ToString());

                    if (tblLoadingTODT["transporterOrgName"] != DBNull.Value)
                        tblLoadingTONew.TransporterOrgName = Convert.ToString(tblLoadingTODT["transporterOrgName"].ToString());

                    if (tblLoadingTODT["superwisorId"] != DBNull.Value)
                        tblLoadingTONew.SuperwisorId = Convert.ToInt32(tblLoadingTODT["superwisorId"].ToString());
                    if (tblLoadingTODT["superwisorName"] != DBNull.Value)
                        tblLoadingTONew.SuperwisorName = Convert.ToString(tblLoadingTODT["superwisorName"].ToString());
                    if (tblLoadingTODT["isFreightIncluded"] != DBNull.Value)
                        tblLoadingTONew.IsFreightIncluded = Convert.ToInt32(tblLoadingTODT["isFreightIncluded"].ToString());

                    if (tblLoadingTODT["contactNo"] != DBNull.Value)
                        tblLoadingTONew.ContactNo = Convert.ToString(tblLoadingTODT["contactNo"].ToString());
                    if (tblLoadingTODT["driverName"] != DBNull.Value)
                        tblLoadingTONew.DriverName = Convert.ToString(tblLoadingTODT["driverName"].ToString());

                    if (tblLoadingTODT["digitalSign"] != DBNull.Value)
                        tblLoadingTONew.DigitalSign = Convert.ToString(tblLoadingTODT["digitalSign"].ToString());
                    if (tblLoadingTODT["userDisplayName"] != DBNull.Value)
                        tblLoadingTONew.CreatedByUserName = Convert.ToString(tblLoadingTODT["userDisplayName"].ToString());
                    if (tblLoadingTODT["parentLoadingId"] != DBNull.Value)
                        tblLoadingTONew.ParentLoadingId = Convert.ToInt32(tblLoadingTODT["parentLoadingId"].ToString());
                    if (tblLoadingTODT["callFlag"] != DBNull.Value)
                        tblLoadingTONew.CallFlag = Convert.ToInt32(tblLoadingTODT["callFlag"].ToString());
                    if (tblLoadingTODT["flagUpdatedOn"] != DBNull.Value)
                        tblLoadingTONew.FlagUpdatedOn = Convert.ToDateTime(tblLoadingTODT["flagUpdatedOn"].ToString());
                    if (tblLoadingTODT["isAllowNxtLoading"] != DBNull.Value)
                        tblLoadingTONew.IsAllowNxtLoading = Convert.ToInt32(tblLoadingTODT["isAllowNxtLoading"].ToString());
                    if (tblLoadingTODT["loadingType"] != DBNull.Value)
                        tblLoadingTONew.LoadingType = Convert.ToInt32(tblLoadingTODT["loadingType"]);
                    if (tblLoadingTODT["currencyId"] != DBNull.Value)
                        tblLoadingTONew.CurrencyId = Convert.ToInt32(tblLoadingTODT["currencyId"]);
                    if (tblLoadingTODT["currencyRate"] != DBNull.Value)
                        tblLoadingTONew.CurrencyRate = Convert.ToDouble(tblLoadingTODT["currencyRate"]);
                    if (tblLoadingTODT["callFlagBy"] != DBNull.Value)
                        tblLoadingTONew.CallFlagBy = Convert.ToInt32(tblLoadingTODT["callFlagBy"]);
                    if (tblLoadingTODT["notifyByName"] != DBNull.Value)
                        tblLoadingTONew.NotifyfiedUserName = Convert.ToString(tblLoadingTODT["notifyByName"].ToString());
                    if (tblLoadingTODT["modbusRefId"] != DBNull.Value)
                        tblLoadingTONew.ModbusRefId = Convert.ToInt32(tblLoadingTODT["modbusRefId"]);

                    if (tblLoadingTODT["gateId"] != DBNull.Value)
                        tblLoadingTONew.GateId = Convert.ToInt32(tblLoadingTODT["gateId"]);
                    if (tblLoadingTODT["portNumber"] != DBNull.Value)
                        tblLoadingTONew.PortNumber = Convert.ToString(tblLoadingTODT["portNumber"]);
                    if (tblLoadingTODT["ioTUrl"] != DBNull.Value)
                        tblLoadingTONew.IoTUrl = Convert.ToString(tblLoadingTODT["ioTUrl"]);
                    if (tblLoadingTODT["machineIP"] != DBNull.Value)
                        tblLoadingTONew.MachineIP = Convert.ToString(tblLoadingTODT["machineIP"]);
                    if (tblLoadingTODT["isDBup"] != DBNull.Value)
                        tblLoadingTONew.IsDBup = Convert.ToInt32(tblLoadingTODT["isDBup"]);
                    if (tblLoadingTODT["fromOrgId"] != DBNull.Value)
                        tblLoadingTONew.FromOrgId = Convert.ToInt32(tblLoadingTODT["fromOrgId"]);
                    if (tblLoadingTODT["isInternalCnf"] != DBNull.Value)
                        tblLoadingTONew.IsInternalCnf = Convert.ToInt32(tblLoadingTODT["isInternalCnf"]);
                    if (tblLoadingTODT["fromOrgName"] != DBNull.Value)
                        tblLoadingTONew.FromOrgName = Convert.ToString(tblLoadingTODT["fromOrgName"]);

                    tblLoadingTOList.Add(tblLoadingTONew);
                }
            }
            return tblLoadingTOList;
        }
    }
}
