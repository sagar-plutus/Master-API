using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using Microsoft.Extensions.Logging;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL.Interfaces;
using static ODLMWebAPI.StaticStuff.Constants;
using System.Security.Policy;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using ODLMWebAPI.BL;
using System.Data.Common;
using System.Linq;

namespace ODLMWebAPI.DAL
{
    public class TblOrganizationDAO : ITblOrganizationDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblOrganizationDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT distinct tblOrganization.*, tblPerson.firstName +' '+tblPerson.lastName as firstOwnerName, tblOrgLicenseDtl.licenseValue as licenseValue, PurchaseManagerSupplier.userId,PurchaseManagerSupplier.isDefaultPM,Users.userDisplayName, dimFirmType.firmName as firmType ,cdStructure.cdValue,dimDelPeriod.deliveryPeriod, villageName,districtId,dimStatus.statusName,dimStatus.colorCode,0 AS idQuotaDeclaration,0 AS rate,0 AS alloc_qty,0 AS rate_band ,0 AS balance_qty,0 AS validUpto,dimStatus.isBlocked,dimConsumer.consumerType FROM [tblOrganization] tblOrganization" +
                                  " LEFT JOIN " +
                                   " ( " +
                                   " SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                                   " INNER JOIN tblAddress ON idAddr = addressId " +
                                   " ) addrDtl " +
                                   " ON tblOrganization.addrId = addrDtl.idAddr" +
                                   " LEFT JOIN dimCdStructure cdStructure ON cdStructure.idCdStructure=tblOrganization.cdStructureId" +
                                   " LEFT JOIN dimDelPeriod dimDelPeriod ON dimDelPeriod.idDelPeriod=tblOrganization.delPeriodId" +
                                   " LEFT JOIN tblPurchaseManagerSupplier PurchaseManagerSupplier ON PurchaseManagerSupplier.organizationId = tblOrganization.idOrganization and PurchaseManagerSupplier.isActive =1 and isnull(PurchaseManagerSupplier.isDefaultPM,0)=1" +
                                   " LEFT JOIN tblUser Users ON Users.idUser = PurchaseManagerSupplier.userId AND Users.isActive=1" +
                                   // Deepali Added	
                                   " LEFT JOIN  tblPerson tblPerson ON tblPerson.idPerson = tblOrganization.firstOwnerPersonId  " +
                                   " LEFT JOIN dimStatus dimStatus ON dimStatus.idStatus =  tblOrganization.orgStatusId left join dimFirmType dimFirmType on tblOrganization.firmtypeId = dimFirmType.idFirmType" +
                                   //Prajakta[2020-11-26] Commented and added to get GSTIN no from default address	
                                   //" LEFT JOIN tblOrgLicenseDtl on tblOrgLicenseDtl.organizationId =tblOrganization.idOrganization and  tblOrgLicenseDtl.licenseId =7" +	
                                   " LEFT JOIN tblOrgLicenseDtl on tblOrgLicenseDtl.addressId = tblOrganization.addrId and  tblOrgLicenseDtl.licenseId =7" +
                                   " left join dimConsumerType dimConsumer on dimConsumer.idConsumer = tblOrganization.consumerTypeId";
            return sqlSelectQry;
        }
        #region SqlSelectQueryForDealerList Added By Binal	
        public String SqlSelectQueryForDealerList()
        {
            String sqlSelectQry = " SELECT COUNT(*) OVER () as TotalCount,tblOrganization.*, tblPerson.firstName +' '+tblPerson.lastName as firstOwnerName, permGstDtl.licenseValue as licenseValue, PurchaseManagerSupplier.userId,PurchaseManagerSupplier.isDefaultPM,Users.userDisplayName, dimFirmType.firmName as firmType ,cdStructure.cdValue,dimDelPeriod.deliveryPeriod, villageName,districtId,dimStatus.statusName,dimStatus.colorCode,0 AS idQuotaDeclaration,0 AS rate,0 AS alloc_qty,0 AS rate_band ,0 AS balance_qty,0 AS validUpto,dimStatus.isBlocked,dimConsumer.consumerType,districtName,talukaName,stateName,distributerDetails.distributerName FROM [tblOrganization] tblOrganization" +
                                  " LEFT JOIN  (      SELECT tblOrgAddress.organizationId, tblAddress.* , ISNULL(districtName,'') districtName, ISNULL(talukaName,'') talukaName, ISNULL(stateName,'') stateName      FROM tblOrgAddress     LEFT JOIN tblAddress ON idAddr = addressId LEFT JOIN dimDistrict On idDistrict = districtId LEFT JOIN dimTaluka On idTaluka = talukaId   LEFT JOIN dimState On idState = tblAddress.stateId      WHERE addrTypeId = 1  ) AS addrDtl ON addrDtl.organizationId = idOrganization " +
                                   " LEFT JOIN dimCdStructure cdStructure ON cdStructure.idCdStructure=tblOrganization.cdStructureId" +
                                   " LEFT JOIN dimDelPeriod dimDelPeriod ON dimDelPeriod.idDelPeriod=tblOrganization.delPeriodId" +
                                   " LEFT JOIN tblPurchaseManagerSupplier PurchaseManagerSupplier ON PurchaseManagerSupplier.organizationId = tblOrganization.idOrganization and PurchaseManagerSupplier.isActive =1 and isnull(PurchaseManagerSupplier.isDefaultPM,0)=1" +
                                   " LEFT JOIN tblUser Users ON Users.idUser = PurchaseManagerSupplier.userId AND Users.isActive=1" +
                                   " LEFT JOIN  tblPerson tblPerson ON tblPerson.idPerson = tblOrganization.firstOwnerPersonId  " +
                                   " LEFT JOIN dimStatus dimStatus ON dimStatus.idStatus =  tblOrganization.orgStatusId left join dimFirmType dimFirmType on tblOrganization.firmtypeId = dimFirmType.idFirmType" +
                                   " LEFT JOIN  (      SELECT organizationId,addressId, licenseValue FROM tblOrgLicenseDtl WHERE licenseId IN (6)  ) as provGstDtl On provGstDtl.addressId = tblOrganization.addrId   LEFT JOIN  (      SELECT organizationId,addressId, licenseValue FROM tblOrgLicenseDtl WHERE licenseId IN (7)  ) as permGstDtl On permGstDtl.addressId = tblOrganization.addrId  LEFT JOIN  (      SELECT organizationId,addressId, licenseValue FROM tblOrgLicenseDtl WHERE licenseId IN (1)  ) AS panDtl  ON panDtl.addressId = tblOrganization.addrId" +
                                   " left join dimConsumerType dimConsumer on dimConsumer.idConsumer = tblOrganization.consumerTypeId "+

                                   " LEFT JOIN ( SELECT idOrganization,(CASE WHEN villageName IS NULL then  firmName  ELSE firmName +',' + villageName END) as distributerName " +
                                   " FROM tblOrganization    LEFT JOIN  (  SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                                   " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1  ) addrDtl  ON idOrganization = organizationId " +
                                   " WHERE isActive=1 AND  orgTypeId=1 ) AS distributerDetails ON distributerDetails.idOrganization = tblOrganization.parentId";

             return sqlSelectQry;
        }
        #endregion
        #endregion	
        #region Selection
        public List<TblOrganizationTO> SelectAllTblOrganization()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(rdr);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        #region Add pagination updated by binal	
        public List<TblOrganizationTO> SelectAllTblOrganizationSearch(Int32 orgTypeId, Int32 parentId, int dealerlistType)
        {
            int PageNumber = 1;
            int RowsPerPage = 10; string strsearchtxt = "''";
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                if (parentId == 0 && dealerlistType == 0)
                    cmdSelect.CommandText =
                        SqlSelectQueryForDealerList() + " WHERE tblOrganization.isActive=1 and tblOrganization.orgTypeId=" + orgTypeId +
                                           " ORDER BY tblOrganization.firmName";
                else if (dealerlistType == 1 && parentId == 0)
                {
                    cmdSelect.CommandText =
                        SqlSelectQueryForDealerList() + " INNER JOIN tblCnfDealers ON dealerOrgId=tblOrganization.idOrganization And tblCnfDealers .isActive=1 " +
                                            " WHERE tblOrganization.isActive=1  AND tblOrganization.orgTypeId=" + orgTypeId +
                                            " ORDER BY tblOrganization.firmName";
                }
                else if (dealerlistType == 1 && parentId > 0)
                {
                    cmdSelect.CommandText =
                        SqlSelectQueryForDealerList() + " INNER JOIN tblCnfDealers ON dealerOrgId=tblOrganization.idOrganization And tblCnfDealers .isActive=1 " +
                                           " WHERE tblOrganization.isActive=1  AND tblOrganization.orgTypeId=" + orgTypeId + " AND tblCnfDealers.cnfOrgId=" + parentId + " ORDER BY tblOrganization.firmName";
                }
                else
                    cmdSelect.CommandText =
                        SqlSelectQueryForDealerList() + " INNER JOIN tblCnfDealers ON dealerOrgId=tblOrganization.idOrganization " +
                                             " WHERE tblOrganization.isActive=1 AND tblCnfDealers.isActive=1 AND tblOrganization.orgTypeId=" + orgTypeId + " AND tblCnfDealers.cnfOrgId=" + parentId + " ORDER BY tblOrganization.firmName";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToListCount(rdr);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public List<TblOrganizationTO> SelectAllTblOrganization(Int32 orgTypeId, Int32 parentId, int dealerlistType, int PageNumber, int RowsPerPage, string strsearchtxt, string dealerId, string villageName, string districtId)
        {
            //if (PageNumber == 0)	
            //{ PageNumber = 1; }	
            // if (RowsPerPage == 0)	
            //{ RowsPerPage = 10;}	
            if (strsearchtxt == null)
            { strsearchtxt = "''"; }
            if (dealerId == null)
            { dealerId = "''"; }
            if (villageName == null)
            { villageName = "''"; }
            if (districtId == null)
            { districtId = "''"; }
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                if (parentId == 0 && dealerlistType == 0)
                    cmdSelect.CommandText = "select * from (select COUNT(*) OVER () as SearchAllCount, ROW_NUMBER() over (order by idOrganization ) as RowNumber, * from ( " +
                        SqlSelectQueryForDealerList() + " WHERE tblOrganization.isActive=1 and tblOrganization.orgTypeId=" + orgTypeId +
                                             //"AND (tblOrganization.idOrganization = " + dealerId + " OR " + dealerId + " = '')"+	
                                             ")as tbl1  where( ((" + strsearchtxt + " = '') or (tbl1.firmName like '%' + " + strsearchtxt + " + '%' or tbl1.villageName like '%' + " + strsearchtxt + " + '%' or tbl1.distributerName like '%' + " + strsearchtxt + " + '%' or tbl1.districtName like '%' + " + strsearchtxt + " + '%' or tbl1.talukaName like '%' + " + strsearchtxt + " + '%' or tbl1.stateName like '%' + " + strsearchtxt + " + '%')) " +
                                             "AND ((" + villageName + " = '') or (tbl1.villageName like '%' +  " + villageName + " + '%')) " +
                                             "AND ((" + districtId + " = '') or (tbl1.districtId like '%' + " + districtId + " + '%')) " +
                                             "AND ((" + dealerId + " = '') or (tbl1.idOrganization like '%' + " + dealerId + " + '%')) " +
                                            "))as tbl2 where (" + RowsPerPage + " = 0 " +
                                             "or tbl2.RowNumber between ((" + PageNumber + " - 1) * " + RowsPerPage + " + 1) and (" + PageNumber + " * " + RowsPerPage + "))" +
                                             "ORDER BY tbl2.idOrganization";
                //"ORDER BY tblOrganization.firmName";	
                else if (dealerlistType == 1 && parentId == 0)
                {
                    cmdSelect.CommandText = "select * from (select COUNT(*) OVER () as SearchAllCount, ROW_NUMBER() over (order by idOrganization) as RowNumber, * from(" +
                         SqlSelectQueryForDealerList() + " INNER JOIN tblCnfDealers ON dealerOrgId=tblOrganization.idOrganization And tblCnfDealers .isActive=1 " +
                                            " WHERE tblOrganization.isActive=1  AND tblOrganization.orgTypeId=" + orgTypeId +
                                             //"AND (tblOrganization.idOrganization = " + dealerId + " OR " + dealerId + " = '' )" +	
                                             ")as tbl1  where( ((" + strsearchtxt + " = '') or (tbl1.firmName like '%' + " + strsearchtxt + " + '%' or tbl1.villageName like '%' + " + strsearchtxt + " + '%' or tbl1.distributerName like '%' + " + strsearchtxt + " + '%' or tbl1.districtName like '%' + " + strsearchtxt + " + '%' or tbl1.talukaName like '%' + " + strsearchtxt + " + '%' or tbl1.stateName like '%' + " + strsearchtxt + " + '%')) " +
                                              "AND ((" + villageName + " = '') or (tbl1.villageName like '%' + " + villageName + " + '%')) " +
                                              "AND ((" + districtId + " = '') or (tbl1.districtId like '%' + " + districtId + " + '%')) " +
                                             "AND ((" + dealerId + " = '') or (tbl1.idOrganization like '%' + " + dealerId + " + '%')) " +
                                            "))as tbl2 where (" + RowsPerPage + " = 0 " +
                                             "or tbl2.RowNumber between ((" + PageNumber + " - 1) * " + RowsPerPage + " + 1) and (" + PageNumber + " * " + RowsPerPage + "))" +
                                             "ORDER BY tbl2.idOrganization";
                    //" ORDER BY tblOrganization.firmName";	
                }
                else if (dealerlistType == 1 && parentId > 0)
                {
                    cmdSelect.CommandText = "select * from (select COUNT(*) OVER () as SearchAllCount, ROW_NUMBER() over (order by idOrganization ) as RowNumber, * from(" +
                        SqlSelectQueryForDealerList() +
                                            " INNER JOIN tblCnfDealers ON dealerOrgId=tblOrganization.idOrganization And tblCnfDealers .isActive=1 " +
                                            " WHERE tblOrganization.isActive=1  AND tblOrganization.orgTypeId=" + orgTypeId + " AND tblCnfDealers.cnfOrgId=" + parentId + " " +
                                            //"AND (tblOrganization.idOrganization = " + dealerId + " OR " + dealerId + " = '')" +	
                                            ")as tbl1  where( ((" + strsearchtxt + " = '') or (tbl1.firmName like '%' + " + strsearchtxt + " + '%' or tbl1.villageName like '%' + " + strsearchtxt + " + '%' or tbl1.distributerName like '%' + " + strsearchtxt + " + '%' or tbl1.districtName like '%' + " + strsearchtxt + " + '%' or tbl1.talukaName like '%' + " + strsearchtxt + " + '%' or tbl1.stateName like '%' + " + strsearchtxt + " + '%')) " +
                                             "AND ((" + villageName + " = '') or (tbl1.villageName like '%' + " + villageName + " + '%')) " +
                                             "AND ((" + districtId + " = '') or (tbl1.districtId like '%' + " + districtId + " + '%')) " +
                                             "AND ((" + dealerId + " = '') or (tbl1.idOrganization like '%' + " + dealerId + " + '%')) " +
                                            "))as tbl2 where (" + RowsPerPage + " = 0 " +
                                             "or tbl2.RowNumber between ((" + PageNumber + " - 1) * " + RowsPerPage + " + 1) and (" + PageNumber + " * " + RowsPerPage + "))" +
                                             "ORDER BY tbl2.idOrganization";
                    //"ORDER BY tblOrganization.firmName";	
                }
                else
                    cmdSelect.CommandText = "select * from (select COUNT(*) OVER () as SearchAllCount, ROW_NUMBER() over (order by idOrganization ) as RowNumber, * from(" +
                        SqlSelectQueryForDealerList()
                                            + " INNER JOIN tblCnfDealers ON dealerOrgId=tblOrganization.idOrganization " +
                                              " WHERE tblOrganization.isActive=1 AND tblCnfDealers.isActive=1 AND tblOrganization.orgTypeId=" + orgTypeId + " AND tblCnfDealers.cnfOrgId=" + parentId +
                                               //" AND (tblOrganization.idOrganization =" + dealerId + " OR " + dealerId + " = '')" +	
                                               ")as tbl1  where( ((" + strsearchtxt + " = '') or (tbl1.firmName like '%' + " + strsearchtxt + " + '%' or tbl1.villageName like '%' + " + strsearchtxt + " + '%' or tbl1.distributerName like '%' + " + strsearchtxt + " + '%' or tbl1.districtName like '%' + " + strsearchtxt + " + '%' or tbl1.talukaName like '%' + " + strsearchtxt + " + '%' or tbl1.stateName like '%' + " + strsearchtxt + " + '%')) " +
                                               "AND ((" + villageName + " = '') or (tbl1.villageName like '%' + " + villageName + " + '%')) " +
                                               "AND ((" + districtId + " = '') or (tbl1.districtId like '%' + " + districtId + " + '%')) " +
                                               "AND ((" + dealerId + " = '') or (tbl1.idOrganization like '%' + " + dealerId + " + '%')) " +
                                            "))as tbl2 where (" + RowsPerPage + " = 0 " +
                                             "or tbl2.RowNumber between ((" + PageNumber + " - 1) * " + RowsPerPage + " + 1) and (" + PageNumber + " * " + RowsPerPage + "))" +
                                             "ORDER BY tbl2.idOrganization";
                //"ORDER BY tblOrganization.firmName";	
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                //return ConvertDTToList(rdr);	
                List<TblOrganizationTO> list = ConvertDTToList(rdr);
                if (list.Count == 0)
                {
                    list = SelectAllTblOrganizationSearch(orgTypeId, parentId, dealerlistType);
                }
                rdr.Dispose();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        #endregion

        public List<TblOrganizationTO> SelectExistingAllTblOrganizationByRefIds(Int32 orgId, String overdueRefId, String enqRefId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();

                cmdSelect.CommandText = SqlSelectQuery();

                if (orgId != 0)
                    cmdSelect.CommandText += " WHERE tblOrganization.idOrganization != " + orgId;

                if (!String.IsNullOrEmpty(overdueRefId))
                {

                    if (cmdSelect.CommandText.Contains("WHERE"))
                        cmdSelect.CommandText += " AND ";
                    else
                        cmdSelect.CommandText += " WHERE ";

                    cmdSelect.CommandText += "tblOrganization.overdue_ref_id = '" + overdueRefId + "'";
                }

                if (!String.IsNullOrEmpty(enqRefId))
                {

                    if (cmdSelect.CommandText.Contains("WHERE"))
                        cmdSelect.CommandText += " AND ";
                    else
                        cmdSelect.CommandText += " WHERE ";

                    cmdSelect.CommandText += "tblOrganization.enq_ref_id = '" + enqRefId + "'";
                }

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(rdr);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblOrganizationTO> SelectSaleAgentOrganizationList()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();

                cmdSelect.CommandText = " SELECT tblOrganization.*,cdStructure.cdValue,dimDelPeriod.deliveryPeriod, villageName,districtId , 0 AS idQuotaDeclaration,0 AS rate ,0 AS alloc_qty , 0 AS rate_band,0 AS balance_qty , 0 AS validUpto  FROM [tblOrganization] tblOrganization" +
                                    " LEFT JOIN " +
                                   " ( " +
                                   " SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                                   " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                                   " ) addrDtl " +
                                   " ON idOrganization = organizationId " +
                                   " LEFT JOIN dimCdStructure cdStructure ON cdStructure.idCdStructure=tblOrganization.cdStructureId" +
                                   " LEFT JOIN dimDelPeriod dimDelPeriod ON dimDelPeriod.idDelPeriod=tblOrganization.delPeriodId" + " WHERE tblOrganization.isActive=1 and orgTypeId=" + (int)Constants.OrgTypeE.C_AND_F_AGENT + " ORDER BY tblOrganization.firmName";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(rdr);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblOrganizationTO> SelectAllTblOrganization(Constants.OrgTypeE orgTypeE, int parentId)
        {
            string countId = "";
            if (parentId >= 9999)
            {
                countId = "9999";
                int length = parentId.ToString().Length;
                if (length > 4)
                {
                    string ParentId = Convert.ToString(parentId);
                    parentId = Convert.ToInt32(ParentId.Remove(ParentId.Length - 4));
                }
                else
                {
                    parentId = 0;
                }

            }
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String sqlQuery = String.Empty;
            try
            {
                conn.Open();

                if (orgTypeE == Constants.OrgTypeE.C_AND_F_AGENT)
                    sqlQuery = " SELECT COUNT(*) OVER () as TotalCount,0 as licenseValue, PurchaseManagerSupplier.userId,PurchaseManagerSupplier.isDefaultPM,Users.userDisplayName, dimFirmType.firmName as firmType,0 as consumerType, " + selectCnfQuery() +
                              " ,dimStatus.statusName,dimStatus.colorCode,dimStatus.isBlocked ,cdStructure.cdValue,dimDelPeriod.deliveryPeriod,villageName,addrDtl.districtId, " +
                               //" existingRate.idQuotaDeclaration,rate , existingRate.alloc_qty,existingRate.rate_band ,balance_qty,validUpto" +
                               " 0 AS idQuotaDeclaration,0 AS rate , 0 AS alloc_qty,0 AS rate_band ,0 AS balance_qty,0 AS validUpto,cnFInfo.tdsPct" +
                               " FROM tblOrganization cnFInfo " +
                               //" LEFT JOIN " +
                               //" ( " +
                               //"     SELECT main.idQuotaDeclaration, rate, main.orgId, main.quotaAllocDate, alloc_qty, rate_band,balance_qty,validUpto " +
                               //"     FROM tblQuotaDeclaration main " +
                               //"    INNER JOIN " +
                               //"       (SELECT orgId, max(quotaAllocDate) quotaAllocDate " +
                               //"        FROM tblQuotaDeclaration " +
                               //"         group by orgId) RESULT " +
                               //"         ON main.orgId = RESULT.orgId and main.quotaAllocDate = RESULT.quotaAllocDate " +
                               //"    INNER JOIN tblGlobalRate ON tblGlobalRate.idGlobalRate=main.globalRateId  " +
                               //" ) AS existingRate " +
                               //" ON cnFInfo.idOrganization = existingRate.orgId " +
                               " LEFT JOIN " +
                               " ( " +
                               " SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                               " INNER JOIN tblAddress ON idAddr = addressId " +
                               " ) addrDtl " +
                               " ON cnFInfo.addrId = addrDtl.idAddr " +
                               " LEFT JOIN dimCdStructure cdStructure ON cdStructure.idCdStructure=cnFInfo.cdStructureId" +
                               " LEFT JOIN dimFirmType dimFirmType ON cnFInfo.firmtypeId = dimFirmType.idFirmType" +
                               " LEFT JOIN tblPurchaseManagerSupplier PurchaseManagerSupplier ON PurchaseManagerSupplier.organizationId = cnFInfo.idOrganization and PurchaseManagerSupplier.isActive =1 and isnull(PurchaseManagerSupplier.isDefaultPM,0)=1" +
                               " LEFT JOIN tblUser Users ON Users.idUser = PurchaseManagerSupplier.userId AND Users.isActive=1" +
                               " LEFT JOIN dimDelPeriod dimDelPeriod ON dimDelPeriod.idDelPeriod=cnFInfo.delPeriodId" +
                                " LEFT JOIN dimStatus dimStatus ON dimStatus.idStatus =  cnFInfo.orgStatusId" +
                                " LEFT JOIN tblOrgLicenseDtl on tblOrgLicenseDtl.organizationId =cnFInfo.idOrganization and  tblOrgLicenseDtl.licenseId =7 " +
                                " WHERE  cnFInfo.isActive=1 AND cnFInfo.orgTypeId=" + (int)orgTypeE;

                else if (orgTypeE == Constants.OrgTypeE.COMPETITOR)
                {
                    sqlQuery = " SELECT COUNT(*) OVER () as TotalCount,0 as licenseValue, PurchaseManagerSupplier.userId,PurchaseManagerSupplier.isDefaultPM,Users.userDisplayName, dimFirmType.firmName as firmType,0 as consumerType, * FROM tblOrganization compeInfo " +
                               " LEFT JOIN ( " +
                               "  SELECT result.* FROM( " +
                               "  SELECT competitorOrgId, max(updateDatetime) updateDatetime " +
                               "  FROM( " +
                               "  SELECT competitorOrgId , compUpdate.updateDatetime, ISNULL(LAG(price) OVER(PARTITION BY competitorExtId,competitorOrgId ORDER BY updateDatetime), 0) as lastPrice " +
                               "   FROM tblCompetitorUpdates compUpdate " +
                               "   ) as res group by competitorOrgId " +
                               "   ) AS main " +
                               "   inner join " +
                               "   ( " +
                               "   SELECT competitorExtId,competitorOrgId ,brandName, prodCapacityMT, compUpdate.updateDatetime, compUpdate.informerName, alternateInformerName ,compUpdate.price, ISNULL(LAG(price) OVER(PARTITION BY competitorExtId,competitorOrgId ORDER BY updateDatetime), 0) as lastPrice " +
                               "   FROM tblCompetitorUpdates compUpdate " +
                               "   INNER JOIN tblCompetitorExt comptExt ON comptExt.idCompetitorExt=compUpdate.competitorExtId" +
                               "   ) result " +
                               "   on main.competitorOrgId = result.competitorOrgId " +
                               "   AND main.updateDatetime = result.updateDatetime " +
                               "   ) AS compUpdate " +
                               "   ON compeInfo.idOrganization = compUpdate.competitorOrgId" +
                                " LEFT JOIN " +
                               " ( " +
                               " SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                               " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                               " ) addrDtl " +
                               " ON compeInfo.addrId = addrDtl.idAddr " +
                               " LEFT JOIN dimCdStructure cdStructure ON cdStructure.idCdStructure=compeInfo.cdStructureId" +
                               " LEFT JOIN dimFirmType dimFirmType ON compeInfo.firmtypeId = dimFirmType.idFirmType" +
                               " LEFT JOIN tblPurchaseManagerSupplier PurchaseManagerSupplier ON PurchaseManagerSupplier.organizationId = compeInfo.idOrganization and PurchaseManagerSupplier.isActive =1 and isnull(PurchaseManagerSupplier.isDefaultPM,0)=1" +
                               " LEFT JOIN tblUser Users ON Users.idUser = PurchaseManagerSupplier.userId AND Users.isActive=1" +
                               " LEFT JOIN dimDelPeriod dimDelPeriod ON dimDelPeriod.idDelPeriod=compeInfo.delPeriodId" +
                                  " LEFT JOIN dimStatus dimSta ON dimSta.idStatus =  compeInfo.orgStatusId" +
                                   " LEFT JOIN tblOrgLicenseDtl on tblOrgLicenseDtl.organizationId =compeInfo.idOrganization and  tblOrgLicenseDtl.licenseId =7 " +
                                " WHERE  compeInfo.isActive=1 AND compeInfo.orgTypeId=" + (int)orgTypeE +
                               " ORDER BY updateDatetime DESC";
                }


                //if(orgTypeE == Constants.OrgTypeE.SUPPLIER)
                //{
                //    sqlQuery = " SELECT 0 as licenseValue, PurchaseManagerSupplier.userId, dimFirmType.firmName as firmType,0 as consumerType, * FROM tblOrganization compeInfo " +
                //               " LEFT JOIN ( " +
                //               "  SELECT result.* FROM( " +
                //               "  SELECT competitorOrgId, max(updateDatetime) updateDatetime " +
                //               "  FROM( " +
                //               "  SELECT competitorOrgId , compUpdate.updateDatetime, ISNULL(LAG(price) OVER(PARTITION BY competitorExtId,competitorOrgId ORDER BY updateDatetime), 0) as lastPrice " +
                //               "   FROM tblCompetitorUpdates compUpdate " +
                //               "   ) as res group by competitorOrgId " +
                //               "   ) AS main " +
                //               "   inner join " +
                //               "   ( " +
                //               "   SELECT competitorExtId,competitorOrgId ,brandName, prodCapacityMT, compUpdate.updateDatetime, compUpdate.informerName, alternateInformerName ,compUpdate.price, ISNULL(LAG(price) OVER(PARTITION BY competitorExtId,competitorOrgId ORDER BY updateDatetime), 0) as lastPrice " +
                //               "   FROM tblCompetitorUpdates compUpdate " +
                //               "   INNER JOIN tblCompetitorExt comptExt ON comptExt.idCompetitorExt=compUpdate.competitorExtId" +
                //               "   ) result " +
                //               "   on main.competitorOrgId = result.competitorOrgId " +
                //               "   AND main.updateDatetime = result.updateDatetime " +
                //               "   ) AS compUpdate " +
                //               "   ON compeInfo.idOrganization = compUpdate.competitorOrgId" +
                //                " LEFT JOIN " +
                //               " ( " +
                //               " SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                //               " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                //               " ) addrDtl " +
                //               " ON compeInfo.addrId = addrDtl.idAddr " +
                //               " LEFT JOIN dimCdStructure cdStructure ON cdStructure.idCdStructure=compeInfo.cdStructureId" +
                //               " LEFT JOIN dimFirmType dimFirmType ON compeInfo.firmtypeId = dimFirmType.idFirmType" +
                //               "  LEFT JOIN tblPurchaseManagerSupplier as PurchaseManagerSupplier ON PurchaseManagerSupplier.organizationId = tblOrganization.idOrganization" +
                //               " LEFT JOIN dimDelPeriod dimDelPeriod ON dimDelPeriod.idDelPeriod=compeInfo.delPeriodId" +
                //                  " LEFT JOIN dimStatus dimSta ON dimSta.idStatus =  compeInfo.orgStatusId" +
                //                   " LEFT JOIN tblOrgLicenseDtl on tblOrgLicenseDtl.organizationId =compeInfo.idOrganization and  tblOrgLicenseDtl.licenseId =7 " +
                //                " WHERE  compeInfo.isActive=1 AND compeInfo.orgTypeId=" + (int)orgTypeE +
                //               " ORDER BY updateDatetime DESC";
                //}

                else if (orgTypeE == Constants.OrgTypeE.DEALER && parentId > 0)
                    sqlQuery = SqlSelectQueryForDealerList() + " INNER JOIN tblCnfDealers ON dealerOrgId=tblOrganization.idOrganization And tblCnfDealers .isActive=1 " +
                                            " WHERE tblOrganization.isActive=1  AND tblOrganization.orgTypeId=" + (int)orgTypeE + " ORDER BY tblOrganization.firmName";
                else sqlQuery = SqlSelectQueryForDealerList() + " WHERE  tblOrganization.isActive=1 AND tblOrganization.orgTypeId=" + (int)orgTypeE;

                //if (orgTypeE == Constants.OrgTypeE.C_AND_F_AGENT)
                //{
                //    sqlQuery = sqlQuery + " group by " + groupByCnF();

                //}
                //else if (orgTypeE != Constants.OrgTypeE.COMPETITOR)
                //{
                //    sqlQuery = sqlQuery + " group by " + groupBy();
                //}

                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblOrganizationTO> list = null;
                if (countId == "9999")
                {
                    list = ConvertDTToListCount(rdr);
                }
                else
                {
                    list = ConvertDTToList(rdr);
                }
                rdr.Dispose();
                return list;
            }
            catch (Exception ex)
            {
                Constants.LoggerObj.LogError(1, ex, "Error in Method SelectAllTblOrganization at DAO", orgTypeE);
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        #region Add pagination for distributer display list changed by binal		
        public List<TblOrganizationTO> SelectAllTblOrganization(Constants.OrgTypeE orgTypeE, int parentId, int PageNumber, int RowsPerPage, string strsearchtxt)
        {
            //if (PageNumber == 0)		
            //{ PageNumber = 1; }		
            //if (RowsPerPage == 0)		
            //{ RowsPerPage = 10; }		
            if (strsearchtxt == null)
            { strsearchtxt = "''"; }
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String sqlQuery = String.Empty;
            try
            {
                conn.Open();
                if (orgTypeE == Constants.OrgTypeE.C_AND_F_AGENT)
                    sqlQuery = "select * from (select COUNT(*) OVER () as SearchAllCount, ROW_NUMBER() over (order by tbl1.idOrganization desc) as RowNumber, * from(" +
                              " SELECT COUNT(*) OVER () as TotalCount,0 as licenseValue, PurchaseManagerSupplier.userId,PurchaseManagerSupplier.isDefaultPM,Users.userDisplayName, dimFirmType.firmName as firmType,0 as consumerType, " + selectCnfQuery() +
                              " ,dimStatus.statusName,dimStatus.colorCode,dimStatus.isBlocked ,cdStructure.cdValue,dimDelPeriod.deliveryPeriod,villageName,addrDtl.districtId, " +
                               //" existingRate.idQuotaDeclaration,rate , existingRate.alloc_qty,existingRate.rate_band ,balance_qty,validUpto" +		
                               " 0 AS idQuotaDeclaration,0 AS rate , 0 AS alloc_qty,0 AS rate_band ,0 AS balance_qty,0 AS validUpto,cnFInfo.tdsPct" +
                               " FROM tblOrganization cnFInfo " +
                               //" LEFT JOIN " +		
                               //" ( " +		
                               //"     SELECT main.idQuotaDeclaration, rate, main.orgId, main.quotaAllocDate, alloc_qty, rate_band,balance_qty,validUpto " +		
                               //"     FROM tblQuotaDeclaration main " +		
                               //"    INNER JOIN " +		
                               //"       (SELECT orgId, max(quotaAllocDate) quotaAllocDate " +		
                               //"        FROM tblQuotaDeclaration " +		
                               //"         group by orgId) RESULT " +		
                               //"         ON main.orgId = RESULT.orgId and main.quotaAllocDate = RESULT.quotaAllocDate " +		
                               //"    INNER JOIN tblGlobalRate ON tblGlobalRate.idGlobalRate=main.globalRateId  " +		
                               //" ) AS existingRate " +		
                               //" ON cnFInfo.idOrganization = existingRate.orgId " +		
                               " LEFT JOIN " +
                               " ( " +
                               " SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                               " INNER JOIN tblAddress ON idAddr = addressId " +
                               " ) addrDtl " +
                               " ON cnFInfo.addrId = addrDtl.idAddr " +
                               " LEFT JOIN dimCdStructure cdStructure ON cdStructure.idCdStructure=cnFInfo.cdStructureId" +
                               " LEFT JOIN dimFirmType dimFirmType ON cnFInfo.firmtypeId = dimFirmType.idFirmType" +
                               " LEFT JOIN tblPurchaseManagerSupplier PurchaseManagerSupplier ON PurchaseManagerSupplier.organizationId = cnFInfo.idOrganization and PurchaseManagerSupplier.isActive =1 and isnull(PurchaseManagerSupplier.isDefaultPM,0)=1" +
                               " LEFT JOIN tblUser Users ON Users.idUser = PurchaseManagerSupplier.userId AND Users.isActive=1" +
                               " LEFT JOIN dimDelPeriod dimDelPeriod ON dimDelPeriod.idDelPeriod=cnFInfo.delPeriodId" +
                                " LEFT JOIN dimStatus dimStatus ON dimStatus.idStatus =  cnFInfo.orgStatusId" +
                                " LEFT JOIN tblOrgLicenseDtl on tblOrgLicenseDtl.organizationId =cnFInfo.idOrganization and  tblOrgLicenseDtl.licenseId =7 " +
                                " WHERE  cnFInfo.isActive=1 AND cnFInfo.orgTypeId=" + (int)orgTypeE +
                                " )as tbl1 where(( " + strsearchtxt + " = '') or (tbl1.firmName like '%' + " + strsearchtxt + " + '%' or tbl1.villageName like '%' +  " + strsearchtxt + "  + '%')))as tbl2 where (" + RowsPerPage + " = 0 " +
                                        " or tbl2.RowNumber between ((" + PageNumber + " - 1) * " + RowsPerPage + " + 1) and (" + PageNumber + " * " + RowsPerPage + ")) ";
                else if (orgTypeE == Constants.OrgTypeE.COMPETITOR)
                {
                    sqlQuery = "SELECT * FROM (SELECT COUNT(*) OVER () as SearchAllCount, ROW_NUMBER() OVER (ORDER BY tbl1.updateDatetime desc) as RowNumber, * FROM ( " +
                               " SELECT COUNT(*) OVER () as TotalCount,0 as licenseValue, PurchaseManagerSupplier.userId,PurchaseManagerSupplier.isDefaultPM,Users.userDisplayName, dimFirmType.firmName as firmType,0 as consumerType," +
                               //" *" +	
                               "compeInfo.idOrganization,compeInfo.firmName,orgTypeId,addrId,firstOwnerPersonId,secondOwnerPersonId,parentId,compeInfo.createdBy,compeInfo.createdOn,updatedBy,updatedOn,phoneNo,faxNo,emailAddr,website,xlImportedId,registeredMobileNos,cdStructureId,compeInfo.isActive,remark,delPeriodId,isSpecialCnf,digitalSign,compeInfo.deactivatedOn,\r\norgLogo,firmTypeId,influencerTypeId,dateOfEstablishment,orgStatusId,overdue_ref_id,enq_ref_id,isOverdueExist,suppDivGroupId,isRegUnderGST,creditLimit,rebMobNoCntryCode,firmCode,branchName,branchCode,bankCustomerId,primaryAccNo,accTypeId,bgLimit,ifscCode,swiftCode,currencyId,txnId,orgGrpTypeId,consumerTypeId,enqRefId,overdueRefId,tempOrgId,isInternalCnf,isTcsApplicable,isDeclarationRec,tdsPct,compeInfo.isMigration,competitorExtId,competitorOrgId,brandName,prodCapacityMT,updateDatetime,informerName,alternateInformerName,price,lastPrice,idAddr,plotNo,streetName,areaName,villageName,talukaId,districtId,stateId,countryId,pincode,comments,lat,lng,isAddrVisible,addrDtl.gstTypeId,addrDtl.organizationId,idCdStructure,cdValue,isPercent,moduleId,idFirmType,isDefault,idPurchaseManagerSupplier,idUser,userLogin,userPasswd,registeredDeviceId,deactivatedBy,imeiNumber,doucmentId,userTypeId,isChangeModeUser,aliasName,isProFlowUser,idDelPeriod,deliveryPeriod,idStatus,transactionTypeId,statusName,prevStatusId,statusDesc,dispSeqNo,colorCode,iotStatusId,isBlocked,basicPrevStatusId,idOrgLicense,licenseId,addressId" +
                                " FROM tblOrganization compeInfo " +
                               " LEFT JOIN ( " +
                               "  SELECT result.* FROM( " +
                               "  SELECT competitorOrgId, max(updateDatetime) updateDatetime " +
                               "  FROM( " +
                               "  SELECT competitorOrgId , compUpdate.updateDatetime, ISNULL(LAG(price) OVER(PARTITION BY competitorExtId,competitorOrgId ORDER BY updateDatetime), 0) as lastPrice " +
                               "   FROM tblCompetitorUpdates compUpdate " +
                               "   ) as res group by competitorOrgId " +
                               "   ) AS main " +
                               "   inner join " +
                               "   ( " +
                               "   SELECT competitorExtId,competitorOrgId ,brandName, prodCapacityMT, compUpdate.updateDatetime, compUpdate.informerName, alternateInformerName ,compUpdate.price, ISNULL(LAG(price) OVER(PARTITION BY competitorExtId,competitorOrgId ORDER BY updateDatetime), 0) as lastPrice " +
                               "   FROM tblCompetitorUpdates compUpdate " +
                               "   INNER JOIN tblCompetitorExt comptExt ON comptExt.idCompetitorExt=compUpdate.competitorExtId" +
                               "   ) result " +
                               "   on main.competitorOrgId = result.competitorOrgId " +
                               "   AND main.updateDatetime = result.updateDatetime " +
                               "   ) AS compUpdate " +
                               "   ON compeInfo.idOrganization = compUpdate.competitorOrgId" +
                                " LEFT JOIN " +
                               " ( " +
                               " SELECT " +
                               //"tblAddress.*," +	
                               "tblAddress.idAddr,tblAddress.plotNo,tblAddress.streetName,tblAddress.areaName,tblAddress.villageName,tblAddress.talukaId,tblAddress.districtId,tblAddress.stateId ,tblAddress.countryId,tblAddress.pincode,tblAddress.comments,tblAddress.lat,tblAddress.lng,tblAddress.isAddrVisible,tblAddress.gstTypeId," +
                               " organizationId FROM tblOrgAddress " +
                               " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                               " ) addrDtl " +
                               " ON compeInfo.addrId = addrDtl.idAddr " +
                               " LEFT JOIN dimCdStructure cdStructure ON cdStructure.idCdStructure=compeInfo.cdStructureId" +
                               " LEFT JOIN dimFirmType dimFirmType ON compeInfo.firmtypeId = dimFirmType.idFirmType" +
                               " LEFT JOIN tblPurchaseManagerSupplier PurchaseManagerSupplier ON PurchaseManagerSupplier.organizationId = compeInfo.idOrganization and PurchaseManagerSupplier.isActive =1 and isnull(PurchaseManagerSupplier.isDefaultPM,0)=1" +
                               " LEFT JOIN tblUser Users ON Users.idUser = PurchaseManagerSupplier.userId AND Users.isActive=1" +
                               " LEFT JOIN dimDelPeriod dimDelPeriod ON dimDelPeriod.idDelPeriod=compeInfo.delPeriodId" +
                                  " LEFT JOIN dimStatus dimSta ON dimSta.idStatus =  compeInfo.orgStatusId" +
                                   " LEFT JOIN tblOrgLicenseDtl on tblOrgLicenseDtl.organizationId =compeInfo.idOrganization and  tblOrgLicenseDtl.licenseId =7 " +
                                " WHERE  compeInfo.isActive=1 AND compeInfo.orgTypeId=" + (int)orgTypeE +
                              " )as tbl1 where(( " + strsearchtxt + " = '') or (tbl1.firmName like '%' + " + strsearchtxt + " + '%' or tbl1.villageName like '%' +  " + strsearchtxt + "  + '%' )))as tbl2 where (" + RowsPerPage + " = 0 " +
                                        " or tbl2.RowNumber between ((" + PageNumber + " - 1) * " + RowsPerPage + " + 1) and (" + PageNumber + " * " + RowsPerPage + ")) ";
                    //" ORDER BY updateDatetime DESC";	
                }
                //if(orgTypeE == Constants.OrgTypeE.SUPPLIER)	
                //{	
                //    sqlQuery = " SELECT 0 as licenseValue, PurchaseManagerSupplier.userId, dimFirmType.firmName as firmType,0 as consumerType, * FROM tblOrganization compeInfo " +	
                //               " LEFT JOIN ( " +	
                //               "  SELECT result.* FROM( " +	
                //               "  SELECT competitorOrgId, max(updateDatetime) updateDatetime " +	
                //               "  FROM( " +	
                //               "  SELECT competitorOrgId , compUpdate.updateDatetime, ISNULL(LAG(price) OVER(PARTITION BY competitorExtId,competitorOrgId ORDER BY updateDatetime), 0) as lastPrice " +	
                //               "   FROM tblCompetitorUpdates compUpdate " +	
                //               "   ) as res group by competitorOrgId " +	
                //               "   ) AS main " +	
                //               "   inner join " +	
                //               "   ( " +	
                //               "   SELECT competitorExtId,competitorOrgId ,brandName, prodCapacityMT, compUpdate.updateDatetime, compUpdate.informerName, alternateInformerName ,compUpdate.price, ISNULL(LAG(price) OVER(PARTITION BY competitorExtId,competitorOrgId ORDER BY updateDatetime), 0) as lastPrice " +	
                //               "   FROM tblCompetitorUpdates compUpdate " +	
                //               "   INNER JOIN tblCompetitorExt comptExt ON comptExt.idCompetitorExt=compUpdate.competitorExtId" +	
                //               "   ) result " +	
                //               "   on main.competitorOrgId = result.competitorOrgId " +	
                //               "   AND main.updateDatetime = result.updateDatetime " +	
                //               "   ) AS compUpdate " +	
                //               "   ON compeInfo.idOrganization = compUpdate.competitorOrgId" +	
                //                " LEFT JOIN " +	
                //               " ( " +	
                //               " SELECT tblAddress.*, organizationId FROM tblOrgAddress " +	
                //               " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +	
                //               " ) addrDtl " +	
                //               " ON compeInfo.addrId = addrDtl.idAddr " +	
                //               " LEFT JOIN dimCdStructure cdStructure ON cdStructure.idCdStructure=compeInfo.cdStructureId" +	
                //               " LEFT JOIN dimFirmType dimFirmType ON compeInfo.firmtypeId = dimFirmType.idFirmType" +	
                //               "  LEFT JOIN tblPurchaseManagerSupplier as PurchaseManagerSupplier ON PurchaseManagerSupplier.organizationId = tblOrganization.idOrganization" +	
                //               " LEFT JOIN dimDelPeriod dimDelPeriod ON dimDelPeriod.idDelPeriod=compeInfo.delPeriodId" +	
                //                  " LEFT JOIN dimStatus dimSta ON dimSta.idStatus =  compeInfo.orgStatusId" +	
                //                   " LEFT JOIN tblOrgLicenseDtl on tblOrgLicenseDtl.organizationId =compeInfo.idOrganization and  tblOrgLicenseDtl.licenseId =7 " +	
                //                " WHERE  compeInfo.isActive=1 AND compeInfo.orgTypeId=" + (int)orgTypeE +	
                //               " ORDER BY updateDatetime DESC";	
                //}	
                else if (orgTypeE == Constants.OrgTypeE.DEALER && parentId > 0)
                    sqlQuery = "select * from (select COUNT(*) OVER () as SearchAllCount, ROW_NUMBER() over (order by tbl1.idOrganization desc) as RowNumber, * from(" +
                         SqlSelectQueryForDealerList() + " INNER JOIN tblCnfDealers ON dealerOrgId=tblOrganization.idOrganization And tblCnfDealers .isActive=1 " +
                                            " WHERE tblOrganization.isActive=1  AND tblOrganization.orgTypeId=" + (int)orgTypeE +
                                              " )as tbl1 where(( " + strsearchtxt + " = '') or (tbl1.firmName like '%' + " + strsearchtxt + " + '%' or tbl1.villageName like '%' +  " + strsearchtxt + "  + '%' )))as tbl2 where (" + RowsPerPage + " = 0 " +
                                              " or tbl2.RowNumber between ((" + PageNumber + " - 1) * " + RowsPerPage + " + 1) and (" + PageNumber + " * " + RowsPerPage + ")) " +
                                              " ORDER BY tblOrganization.firmName";
                else sqlQuery = "select * from (select COUNT(*) OVER () as SearchAllCount, ROW_NUMBER() over (order by tbl1.idOrganization desc) as RowNumber, * from(" +
                        SqlSelectQueryForDealerList() + " WHERE  tblOrganization.isActive=1 AND tblOrganization.orgTypeId=" + (int)orgTypeE +
                                       " )as tbl1 where(( " + strsearchtxt + " = '') or (tbl1.firmName like '%' + " + strsearchtxt + " + '%' or tbl1.villageName like '%' +  " + strsearchtxt + "  + '%')))as tbl2 where (" + RowsPerPage + " = 0 " +
                                        " or tbl2.RowNumber between ((" + PageNumber + " - 1) * " + RowsPerPage + " + 1) and (" + PageNumber + " * " + RowsPerPage + ")) ";
                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblOrganizationTO> list = ConvertDTToList(rdr);
                if (list.Count == 0)
                {
                    parentId = Convert.ToInt32(parentId + "9999");
                    list = SelectAllTblOrganization(orgTypeE, parentId);
                }
                rdr.Dispose();
                return list;
            }
            catch (Exception ex)
            {
                Constants.LoggerObj.LogError(1, ex, "Error in Method SelectAllTblOrganization at DAO", orgTypeE);
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        #endregion

        public ResultMessage SelectFirmName(TblOrganizationTO tblOrganizationTO, Boolean isFromEdit, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage rMessage = new ResultMessage();
            SqlConnection con = conn;
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                if (!isFromEdit)
                {
                    cmdSelect.CommandText = "SELECT * FROM tblOrganization WHERE RTRIM(LTRIM(firmName)) = RTRIM(LTRIM('" + tblOrganizationTO.FirmName + "')) AND orgTypeId=" + tblOrganizationTO.OrgTypeId + " AND isActive=1";
                }
                else
                {
                    cmdSelect.CommandText = "SELECT * FROM tblOrganization WHERE RTRIM(LTRIM(firmName)) = RTRIM(LTRIM('" + tblOrganizationTO.FirmName + "')) AND orgTypeId=" + tblOrganizationTO.OrgTypeId + " AND idOrganization !=" + tblOrganizationTO.IdOrganization + " AND isActive=1";

                }
                cmdSelect.Connection = con;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader dataReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                //List<ResultMessage> tempResultMessage = new List<ResultMessage>();
                while (dataReader.Read())
                {
                    if (dataReader["idOrganization"] != null) ;
                    rMessage.Text = Convert.ToString(dataReader["idOrganization"]);


                }
                if (dataReader.HasRows)
                {
                    dataReader.Dispose();
                    rMessage.Result = 0;
                    return rMessage;
                }
                else
                {
                    dataReader.Dispose();
                    rMessage.Result = 1;
                    return rMessage;
                }
            }
            catch (Exception ex)
            {
                rMessage.Result = 0;
                return rMessage;
            }
            finally
            {
                cmdSelect.Dispose();
            }
        }

        public string selectCnfQuery()
        {
            return " cnFInfo.idOrganization,cnFInfo.firmName,cnFInfo.isTcsApplicable,cnFInfo.isDeclarationRec,  cnFInfo.orgTypeId,cnFInfo.addrId,cnFInfo.firstOwnerPersonId,cnFInfo.secondOwnerPersonId," +
                   " cnFInfo.parentId,cnFInfo.createdBy,cnFInfo.createdOn,cnFInfo.updatedBy, cnFInfo.updatedOn,cnFInfo.phoneNo, cnFInfo.faxNo," +
                   " cnFInfo.emailAddr,cnFInfo.website,cnFInfo.xlImportedId,cnFInfo.registeredMobileNos,cnFInfo.cdStructureId,cnFInfo.isActive , " +
                   " cnFInfo.remark,cnFInfo.delPeriodId ,cnFInfo.isSpecialCnf,cnFInfo.digitalSign,cnFInfo.deactivatedOn ,cnFInfo.orgLogo,cnFInfo.firmTypeId," +
                   " cnFInfo.influencerTypeId,cnFInfo.dateOfEstablishment,cnFInfo.overdue_ref_id ,cnFInfo.enq_ref_id,cnFInfo.isOverdueExist," +
                   " cnFInfo.suppDivGroupId,cnFInfo.isRegUnderGST,cnFInfo.orgStatusId ,cnFInfo.creditLimit,cnFInfo.rebMobNoCntryCode," +
                   " cnFInfo.firmCode,cnFInfo.branchName,cnFInfo.branchCode , cnFInfo.bankCustomerId,cnFInfo.primaryAccNo,cnFInfo.accTypeId," +
                   " cnFInfo.bgLimit,cnFInfo.ifscCode ,cnFInfo.swiftCode,cnFInfo.currencyId,cnFInfo.txnId,cnFInfo.consumerTypeId, cnFInfo.orgGrpTypeId ";
        }

        public string selectOrgQuery()
        {
            return "tblOrganization.idOrganization,tblOrganization.firmName,tblOrganization.orgTypeId,  tblOrganization.addrId, " +
                " tblOrganization.firstOwnerPersonId,tblOrganization.secondOwnerPersonId, tblOrganization.parentId,tblOrganization.createdBy ," +
                " tblOrganization.createdOn,tblOrganization.updatedBy,tblOrganization.updatedOn,tblOrganization.phoneNo, tblOrganization.faxNo," +
                " tblOrganization.emailAddr ,tblOrganization.website,tblOrganization.xlImportedId,tblOrganization.registeredMobileNos " +
                " ,tblOrganization.cdStructureId,tblOrganization.isActive , tblOrganization.remark,tblOrganization.delPeriodId,tblOrganization.isSpecialCnf," +
                " tblOrganization.digitalSign,tblOrganization.deactivatedOn ,tblOrganization.orgLogo,tblOrganization.firmTypeId,tblOrganization.influencerTypeId," +
                " tblOrganization.dateOfEstablishment,tblOrganization.overdue_ref_id ,tblOrganization.enq_ref_id,tblOrganization.isOverdueExist," +
                " tblOrganization.suppDivGroupId, tblOrganization.isRegUnderGST,tblOrganization.orgStatusId ,tblOrganization.creditLimit," +
                " tblOrganization.rebMobNoCntryCode,tblOrganization.firmCode,  tblOrganization.branchName,tblOrganization.branchCode ," +
                " tblOrganization.bankCustomerId,tblOrganization.primaryAccNo,tblOrganization.accTypeId,tblOrganization.bgLimit, tblOrganization.ifscCode ," +
                " tblOrganization.swiftCode,tblOrganization.currencyId,tblOrganization.txnId,tblOrganization.consumerTypeId, tblOrganization.orgGrpTypeId ";
        }

        public string groupByCnF()
        {
            return " idOrganization,dimFirmType.firmName,cnFInfo.firmName, " +
              " cnFInfo.orgTypeId,cnFInfo.addrId,cnFInfo.firstOwnerPersonId,cnFInfo.secondOwnerPersonId,                                                  " +
              " cnFInfo.parentId,cnFInfo.createdBy,cnFInfo.createdOn,cnFInfo.updatedBy, cnFInfo.updatedOn,cnFInfo.phoneNo,                                " +
              " cnFInfo.faxNo,cnFInfo.emailAddr,cnFInfo.website,cnFInfo.xlImportedId,cnFInfo.registeredMobileNos,cnFInfo.cdStructureId,cnFInfo.isActive , " +
              " cnFInfo.remark,cnFInfo.delPeriodId ,cnFInfo.isSpecialCnf,cnFInfo.digitalSign,cnFInfo.deactivatedOn ,cnFInfo.orgLogo,cnFInfo.firmTypeId,   " +
              " cnFInfo.influencerTypeId,cnFInfo.dateOfEstablishment,cnFInfo.overdue_ref_id ,cnFInfo.enq_ref_id,cnFInfo.isOverdueExist,                   " +
              " cnFInfo.suppDivGroupId,cnFInfo.isRegUnderGST,cnFInfo.orgStatusId ,cnFInfo.creditLimit,cnFInfo.rebMobNoCntryCode,                          " +
              " cnFInfo.firmCode,cnFInfo.branchName,cnFInfo.branchCode , cnFInfo.bankCustomerId,cnFInfo.primaryAccNo,cnFInfo.accTypeId,                   " +
              " cnFInfo.bgLimit,cnFInfo.ifscCode ,cnFInfo.swiftCode,cnFInfo.currencyId,cnFInfo.txnId,cnFInfo.consumerTypeId                               " +
              " ,dimDelPeriod.deliveryPeriod,villageName,districtId,dimStatus.statusName,dimStatus.colorCode,dimStatus.isBlocked,cdStructure.cdValue,     " +
              " cnFInfo.orgGrpTypeId ";
        }

        public string groupBy()
        {
            return " idOrganization,dimFirmType.firmName,tblOrganization.firmName,tblOrganization.orgTypeId,tblOrganization.addrId, " +
              " tblOrganization.firstOwnerPersonId,tblOrganization.secondOwnerPersonId,tblOrganization.parentId,tblOrganization.createdBy , " +
              "tblOrganization.createdOn,tblOrganization.updatedBy,tblOrganization.updatedOn,tblOrganization.phoneNo,tblOrganization.faxNo," +
              "tblOrganization.emailAddr ,tblOrganization.website,tblOrganization.xlImportedId,tblOrganization.registeredMobileNos,tblOrganization.cdStructureId," +
              "tblOrganization.isActive , tblOrganization.remark,tblOrganization.delPeriodId,tblOrganization.isSpecialCnf,tblOrganization.digitalSign,tblOrganization.deactivatedOn ," +
              "tblOrganization.orgLogo,tblOrganization.firmTypeId,tblOrganization.influencerTypeId,tblOrganization.dateOfEstablishment,tblOrganization.overdue_ref_id ," +
              "tblOrganization.enq_ref_id,tblOrganization.isOverdueExist,tblOrganization.suppDivGroupId,tblOrganization.isRegUnderGST,tblOrganization.orgStatusId ," +
              "tblOrganization.creditLimit,tblOrganization.rebMobNoCntryCode,tblOrganization.firmCode,tblOrganization.branchName,tblOrganization.branchCode , " +
              "tblOrganization.bankCustomerId,tblOrganization.primaryAccNo,tblOrganization.accTypeId,tblOrganization.bgLimit,tblOrganization.ifscCode ," +
              "tblOrganization.swiftCode,tblOrganization.currencyId,tblOrganization.txnId,tblOrganization.consumerTypeId,dimDelPeriod.deliveryPeriod,  " +
              " villageName,districtId,dimStatus.statusName,dimStatus.colorCode,dimStatus.isBlocked,cdStructure.cdValue,dimConsumer.consumerType, tblOrganization.orgGrpTypeId, tblOrgLicenseDtl.licenseValue, tblOrganization.enqRefId, tblOrganization.overdueRefId " +
              " ";
        }

        public List<TblOrganizationTO> SelectAllTblOrganization(Constants.OrgTypeE orgTypeE, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            String sqlQuery = String.Empty;
            SqlDataReader rdr = null;
            try
            {

                if (orgTypeE == Constants.OrgTypeE.C_AND_F_AGENT)
                    sqlQuery = " SELECT cnFInfo.* ,cdStructure.cdValue,dimDelPeriod.deliveryPeriod,villageName,addrDtl.districtId,existingRate.idQuotaDeclaration,rate , existingRate.alloc_qty,existingRate.rate_band ,balance_qty,validUpto" +
                               " FROM tblOrganization cnFInfo " +
                               " LEFT JOIN " +
                               " ( " +
                               "     SELECT main.idQuotaDeclaration, rate, main.orgId, main.quotaAllocDate, alloc_qty, rate_band,balance_qty,validUpto " +
                               "     FROM tblQuotaDeclaration main " +
                               "    INNER JOIN " +
                               "       (SELECT orgId, max(quotaAllocDate) quotaAllocDate " +
                               "        FROM tblQuotaDeclaration " +
                               "         group by orgId) RESULT " +
                               "         ON main.orgId = RESULT.orgId and main.quotaAllocDate = RESULT.quotaAllocDate " +
                               "    INNER JOIN tblGlobalRate ON tblGlobalRate.idGlobalRate=main.globalRateId  " +
                               " ) AS existingRate " +
                               " ON cnFInfo.idOrganization = existingRate.orgId " +
                               " LEFT JOIN " +
                               " ( " +
                               " SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                               " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                               " ) addrDtl " +
                               " ON idOrganization = addrDtl.organizationId " +
                               " LEFT JOIN dimCdStructure cdStructure ON cdStructure.idCdStructure=cnFInfo.cdStructureId" +
                               " LEFT JOIN dimDelPeriod dimDelPeriod ON dimDelPeriod.idDelPeriod=cnFInfo.delPeriodId" +
                               " WHERE  cnFInfo.isActive=1 AND cnFInfo.orgTypeId=" + (int)orgTypeE;

                else if (orgTypeE == Constants.OrgTypeE.COMPETITOR)
                {
                    sqlQuery = " SELECT * FROM tblOrganization compeInfo " +
                               " LEFT JOIN ( " +
                               "  SELECT result.* FROM( " +
                               "  SELECT competitorOrgId, max(updateDatetime) updateDatetime " +
                               "  FROM( " +
                               "  SELECT competitorOrgId , compUpdate.updateDatetime, ISNULL(LAG(price) OVER(PARTITION BY competitorExtId,competitorOrgId ORDER BY updateDatetime), 0) as lastPrice " +
                               "   FROM tblCompetitorUpdates compUpdate " +
                               "   ) as res group by competitorOrgId " +
                               "   ) AS main " +
                               "   inner join " +
                               "   ( " +
                               "   SELECT competitorExtId,competitorOrgId , brandName, prodCapacityMT,compUpdate.updateDatetime, compUpdate.informerName, alternateInformerName ,compUpdate.price, ISNULL(LAG(price) OVER(PARTITION BY competitorExtId,competitorOrgId ORDER BY updateDatetime), 0) as lastPrice " +
                               "   FROM tblCompetitorUpdates compUpdate " +
                               "   INNER JOIN tblCompetitorExt comptExt ON comptExt.idCompetitorExt=compUpdate.competitorExtId" +
                               "   ) result " +
                               "   on main.competitorOrgId = result.competitorOrgId " +
                               "   AND main.updateDatetime = result.updateDatetime " +
                               "   ) AS compUpdate " +
                               "   ON compeInfo.idOrganization = compUpdate.competitorOrgId" +
                                " LEFT JOIN " +
                               " ( " +
                               " SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                               " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                               " ) addrDtl " +
                               " ON compeInfo.idOrganization = addrDtl.organizationId " +
                               " LEFT JOIN dimCdStructure cdStructure ON cdStructure.idCdStructure=compeInfo.cdStructureId" +
                               " LEFT JOIN dimDelPeriod dimDelPeriod ON dimDelPeriod.idDelPeriod=compeInfo.delPeriodId" +
                               " WHERE   compeInfo.isActive=1 AND compeInfo.orgTypeId=" + (int)orgTypeE;
                }

                else sqlQuery = SqlSelectQuery() + " WHERE  tblOrganization.isActive=1 AND tblOrganization.orgTypeId=" + (int)orgTypeE;

                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblOrganizationTO> list = ConvertDTToList(rdr);
                rdr.Dispose();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (rdr != null)
                    rdr.Dispose();
                cmdSelect.Dispose();
            }
        }

        public List<DropDownTO> SelectAllOrganizationListForDropDown(Constants.OrgTypeE orgTypeE, TblUserRoleTO userRoleTO, String DivisionIdsStr = "", Int32 orgGrpType = 0, Int32 IsAllowOrderBy = 0, string searchText = null, bool isFilter = false)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String sqlQuery = String.Empty;
            try
            {
                conn.Open();

                if (orgTypeE == Constants.OrgTypeE.C_AND_F_AGENT)
                {
                    int isConfEn = 0;
                    if (userRoleTO != null)
                        isConfEn = userRoleTO.EnableAreaAlloc;

                    if (isConfEn == 1)
                        sqlQuery = " SELECT idOrganization,firmName , isSpecialCnf FROM tblOrganization" +
                                   " WHERE  orgTypeId=" + (int)orgTypeE +
                                   " AND idOrganization IN(SELECT cnfOrgId FROM tblUserAreaAllocation WHERE userId=" + userRoleTO.UserId + " AND isActive=1 ) " +
                                   " AND isActive=1";
                    else
                        sqlQuery = " SELECT idOrganization,(CASE WHEN villageName IS NULL then  firmName  ELSE firmName +',' + villageName END) as firmName ," +
                                  " isSpecialCnf FROM tblOrganization   " +
                                  " LEFT JOIN " +
                                  " ( " +
                                  " SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                                  " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                                  " ) addrDtl " +
                                  " ON idOrganization = organizationId " +
                                    "WHERE isActive=1 AND  orgTypeId=" + (int)orgTypeE;

                }
                else if (orgTypeE == Constants.OrgTypeE.DEALER)
                {
                    int isConfEn = 0;
                    if (userRoleTO != null)
                        isConfEn = userRoleTO.EnableAreaAlloc;
                    if (isConfEn == 1)
                    {
                        sqlQuery = "select idOrganization,(CASE WHEN villageName IS NULL then  firmName  ELSE firmName +',' + villageName END) as firmName ," +
                                  " isSpecialCnf from tblOrganization tblOrganization " +
                            " JOIN tblAddress tblAddress on tblAddress.idAddr = tblOrganization.addrId " +
                            " JOIN (	" +
                            " select tblCnfDealers.dealerOrgId,tblUserAreaAllocation.districtId " +
                            " from tblCnfDealers tblCnfDealers " +
                            " JOIN tblUserAreaAllocation tblUserAreaAllocation on tblUserAreaAllocation.cnfOrgId = tblCnfDealers.cnfOrgId " +
                            " where tblCnfDealers.isActive = 1 AND tblUserAreaAllocation.isActive = 1  " +
                            " AND tblUserAreaAllocation.userId = " + userRoleTO.UserId + " AND tblUserAreaAllocation.brandId IS NULL " +
                            " group by tblCnfDealers.dealerOrgId,tblUserAreaAllocation.districtId " +
                            " ) tempOrg on tempOrg.dealerOrgId = tblOrganization.idOrganization AND tblAddress.districtId = tempOrg.districtId " +
                            "  WHERE tblOrganization.isActive = 1 and tblOrganization.orgTypeId = " + (int)orgTypeE;
                    }
                    else
                    {
                        sqlQuery = " SELECT idOrganization,(CASE WHEN villageName IS NULL then  firmName  ELSE firmName +',' + villageName END) as firmName ," +
                                  " isSpecialCnf FROM tblOrganization  " +
                                  " LEFT JOIN " +
                                  " ( " +
                                  " SELECT top(1) tblAddress.*, organizationId FROM tblOrgAddress " +
                                  " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                                  " order by createdOn asc ) addrDtl " +
                                  " ON idOrganization = organizationId " +
                                  "WHERE  isActive=1 AND orgTypeId=" + (int)orgTypeE;
                        if (!String.IsNullOrEmpty(DivisionIdsStr))
                        {
                            sqlQuery += " and (suppDivGroupId IN(" + DivisionIdsStr + ") OR suppDivGroupId IS NULL)";
                        }
                        if (orgGrpType > 0)
                        {
                            sqlQuery += " and (orgGrpTypeId = " + orgGrpType + " or isnull(orgGrpTypeId,5) =5 )";
                        }
                    }
                }
                else
                {
                    sqlQuery = " SELECT idOrganization,(CASE WHEN villageName IS NULL then  firmName  ELSE firmName +',' + villageName END) as firmName ," +
                                  " isSpecialCnf FROM tblOrganization  " +
                                  " LEFT JOIN " +
                                  " ( " +
                                  " SELECT top(1) tblAddress.*, organizationId FROM tblOrgAddress " +
                                  " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                                  " order by createdOn asc ) addrDtl " +
                                  " ON idOrganization = organizationId " +
                                  "WHERE  isActive=1 AND orgTypeId=" + (int)orgTypeE;
                    if (!String.IsNullOrEmpty(DivisionIdsStr))
                    {
                        sqlQuery += " and (suppDivGroupId IN(" + DivisionIdsStr + ") OR suppDivGroupId IS NULL)";
                    }
                    if (orgGrpType > 0)
                    {
                        sqlQuery += " and (orgGrpTypeId = " + orgGrpType + " or isnull(orgGrpTypeId,5) =5 )";
                    }
                }

                sqlQuery = "with cteOrganizationListForDropDown as ( " + sqlQuery + " ) select " + (isFilter  && string.IsNullOrWhiteSpace(searchText) ? "top 10 " : "" ) + "* from cteOrganizationListForDropDown";
                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    sqlQuery += " where firmName like '%" + searchText + "%' ";
                }
                if (IsAllowOrderBy == 1)
                {
                    sqlQuery += " ORDER BY replace(firmName,' ','') ASC";
                }
                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader tblOrgReader = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                if (tblOrgReader != null)
                {
                    while (tblOrgReader.Read())
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        if (tblOrgReader["idOrganization"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(tblOrgReader["idOrganization"].ToString());
                        if (tblOrgReader["firmName"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(tblOrgReader["firmName"].ToString());
                        if (tblOrgReader["isSpecialCnf"] != DBNull.Value)
                            dropDownTO.Tag = "isSpecialCnf : " + Convert.ToString(tblOrgReader["isSpecialCnf"].ToString());
                        dropDownTOList.Add(dropDownTO);
                    }
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                Constants.LoggerObj.LogError(1, ex, "Error in Method SelectAllOrganizationListForDropDown at DAO", orgTypeE);
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        //Priyanka [10-09-2018] : Added to get the Organization list for RM.
        public List<DropDownTO> SelectAllOrganizationListForDropDownForRM(Constants.OrgTypeE orgTypeE, Int32 RMId, TblUserRoleTO userRoleTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String sqlQuery = String.Empty;
            try
            {
                conn.Open();

                sqlQuery = " SELECT distinct idOrganization,(CASE WHEN villageName IS NULL then  firmName  ELSE firmName +',' +" +
                           " villageName END) as firmName , isSpecialCnf FROM tblOrganization org" +
                           " LEFT JOIN( SELECT tblAddress.*, organizationId FROM tblOrgAddress orgAddr " +
                           " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1  )orgAddrDtls ON org.idOrganization = orgAddrDtls.organizationId " +
                           " LEFT JOIN tblUserAreaAllocation userAreaAlloc on userAreaAlloc.cnfOrgId = org.idOrganization and userAreaAlloc.isActive = 1 " +
                           " WHERE org.isActive = 1 AND org.orgTypeId = " + (int)orgTypeE + " AND userAreaAlloc.userId = " + RMId;

                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader tblOrgReader = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                if (tblOrgReader != null)
                {
                    while (tblOrgReader.Read())
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        if (tblOrgReader["idOrganization"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(tblOrgReader["idOrganization"].ToString());
                        if (tblOrgReader["firmName"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(tblOrgReader["firmName"].ToString());
                        if (tblOrgReader["isSpecialCnf"] != DBNull.Value)
                            dropDownTO.Tag = "isSpecialCnf : " + Convert.ToString(tblOrgReader["isSpecialCnf"].ToString());
                        dropDownTOList.Add(dropDownTO);
                    }
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                Constants.LoggerObj.LogError(1, ex, "Error in Method SelectAllOrganizationListForDropDown at DAO", orgTypeE);
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        public List<DropDownTO> SelectAllSpecialCnfListForDropDown(TblUserRoleTO userRoleTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String sqlQuery = String.Empty;
            Constants.OrgTypeE orgTypeE = Constants.OrgTypeE.C_AND_F_AGENT;
            try
            {
                conn.Open();


                sqlQuery = " SELECT idOrganization,firmName,isSpecialCnf FROM tblOrganization" +
                           " WHERE  orgTypeId=" + (int)orgTypeE +
                           " AND isActive=1 AND isSpecialCnf=1";

                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader tblOrgReader = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                if (tblOrgReader != null)
                {
                    while (tblOrgReader.Read())
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        if (tblOrgReader["idOrganization"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(tblOrgReader["idOrganization"].ToString());
                        if (tblOrgReader["firmName"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(tblOrgReader["firmName"].ToString());
                        if (tblOrgReader["isSpecialCnf"] != DBNull.Value)
                            dropDownTO.Tag = "isSpecialCnf : " + Convert.ToString(tblOrgReader["isSpecialCnf"].ToString());
                        dropDownTOList.Add(dropDownTO);
                    }
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                Constants.LoggerObj.LogError(1, ex, "Error in Method SelectAllSpecialCnfListForDropDown at DAO", orgTypeE);
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<DropDownTO> SelectDealerListForDropDown(Int32 cnfId, TblUserRoleTO tblUserRoleTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String sqlQuery = String.Empty;
            //TblUserRoleTO userRoleTO=BL.TblUserRoleBL.SelectTblUserRoleTO()
            int isConfEn = 0;
            int userId = 0;
            if (tblUserRoleTO != null)
            {
                isConfEn = tblUserRoleTO.EnableAreaAlloc;
                userId = tblUserRoleTO.UserId;
            }

            try
            {
                conn.Open();

                if (cnfId > 0)
                {
                    if (isConfEn == 0)
                    {
                        sqlQuery = " SELECT DISTINCT idOrganization, CASE WHEN villageName IS NULL then  firmName " +
                                   " ELSE firmName +',' + villageName END AS dealerName " +
                                   " FROM tblOrganization " +
                                   " INNER JOIN tblCnfDealers ON dealerOrgId=idOrganization" +
                                   " INNER JOIN " +
                                   " ( " +
                                   " SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                                   " INNER JOIN tblAddress ON idAddr = addressId " +
                                   " ) addrDtl " +
                                   " ON tblOrganization.addrId=addrDtl.idAddr WHERE tblOrganization.isActive=1 AND tblCnfDealers.isActive=1 AND orgTypeId=" + (int)Constants.OrgTypeE.DEALER + " AND cnfOrgId=" + cnfId;
                    }
                    else
                    {
                        //modified by vijaymala acc to brandwise allocation or districtwise allocation[26-07-2018]

                        sqlQuery = " SELECT DISTINCT idOrganization, CASE WHEN villageName IS NULL then  firmName " +
                                   " ELSE firmName +',' + villageName END AS dealerName " +
                                   " FROM tblOrganization " +
                                   " INNER JOIN tblCnfDealers ON dealerOrgId=idOrganization" +
                                   " INNER JOIN " +
                                   " ( " +
                                   " SELECT tblAddress.*,organizationId FROM tblOrgAddress " +
                                   " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                                   " ) addrDtl " +
                                   " ON idOrganization = organizationId " +
                                   " INNER JOIN tblUserAreaAllocation areaConf ON " +
                                   " (addrDtl.districtId=areaConf.districtId  AND areaConf.cnfOrgId=tblCnfDealers.cnfOrgId and  areaConf.isActive=1 )" +
                                   " Or (areaConf.cnfOrgId=tblCnfDealers.cnfOrgId AND Isnull(areaConf.districtId,0)=0 and  areaConf.isActive=1  )" +
                                   "WHERE  tblOrganization.isActive=1 AND tblCnfDealers.isActive=1 AND orgTypeId=" + (int)Constants.OrgTypeE.DEALER + " AND areaConf.cnfOrgId=" + cnfId + " AND areaConf.userId=" + userId + " AND areaConf.isActive=1 ";

                    }
                }
                else
                {
                    if (isConfEn == 0)
                    {
                        sqlQuery = " SELECT DISTINCT idOrganization, CASE WHEN villageName IS NULL then  firmName " +
                               " ELSE firmName +',' + villageName END AS dealerName " +
                               " FROM tblOrganization " +
                               " INNER JOIN tblCnfDealers ON dealerOrgId=idOrganization" +
                               " INNER JOIN " +
                               " ( " +
                               " SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                               " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                               " ) addrDtl " +
                               " ON idOrganization = organizationId " +
                               " WHERE tblOrganization.isActive=1 AND tblCnfDealers.isActive=1 AND orgTypeId=" + (int)Constants.OrgTypeE.DEALER;
                    }
                    else
                    {
                        //modified by vijaymala acc to brandwise allocation or districtwise allocation[26-07-2018]

                        sqlQuery = " SELECT DISTINCT idOrganization, CASE WHEN villageName IS NULL then  firmName " +
                                   " ELSE firmName +',' + villageName END AS dealerName " +
                                   " FROM tblOrganization " +
                                   " INNER JOIN tblCnfDealers ON dealerOrgId=idOrganization" +
                                   " INNER JOIN " +
                                   " ( " +
                                   " SELECT tblAddress.*,organizationId FROM tblOrgAddress " +
                                   " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                                   " ) addrDtl " +
                                   " ON idOrganization = organizationId " +
                                   " INNER JOIN tblUserAreaAllocation areaConf ON " +
                                   " (addrDtl.districtId=areaConf.districtId AND areaConf.cnfOrgId=tblCnfDealers.cnfOrgId )" +
                                   " Or (areaConf.cnfOrgId=tblCnfDealers.cnfOrgId  )" +
                                   "WHERE  tblOrganization.isActive=1 AND tblCnfDealers.isActive=1 AND orgTypeId=" + (int)Constants.OrgTypeE.DEALER + " AND areaConf.userId=" + userId + " AND areaConf.isActive=1 ";

                    }
                }

                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader tblOrgReader = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                if (tblOrgReader != null)
                {
                    while (tblOrgReader.Read())
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        if (tblOrgReader["idOrganization"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(tblOrgReader["idOrganization"].ToString());
                        if (tblOrgReader["dealerName"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(tblOrgReader["dealerName"].ToString());
                        dropDownTOList.Add(dropDownTO);
                    }
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                Constants.LoggerObj.LogError(1, ex, "Error in Method SelectAllOrganizationListForDropDown at DAO", cnfId);
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<DropDownTO> GetDealerForLoadingDropDownList(Int32 cnfId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String sqlQuery = String.Empty;
            String innerQuery = string.Empty;
            try
            {
                conn.Open();

                if (cnfId > 0)
                {
                    sqlQuery = " SELECT idOrganization, CASE WHEN villageName IS NULL then  firmName " +
                               " ELSE firmName +',' + villageName END AS dealerName " +
                               " FROM tblOrganization " +
                               " INNER JOIN tblCnfDealers ON dealerOrgId=idOrganization" +
                               " LEFT JOIN " +
                               " ( " +
                               " SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                               " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                               " ) addrDtl " +
                               " ON idOrganization = organizationId WHERE  orgTypeId=" + (int)Constants.OrgTypeE.DEALER + " AND tblCnfDealers.cnfOrgId=" + cnfId +
                               " AND tblOrganization.idOrganization IN(SELECT distinct dealerOrgId FROM tblBookings WHERE cnFOrgId=" + cnfId + " AND pendingQty > 0 AND statusId IN(2,3,9,11))";
                }
                else
                    sqlQuery = " SELECT idOrganization, CASE WHEN villageName IS NULL then  firmName " +
                               " ELSE firmName +',' + villageName END AS dealerName " +
                               " FROM tblOrganization " +
                               " INNER JOIN tblCnfDealers ON dealerOrgId=idOrganization" +
                               " LEFT JOIN " +
                               " ( " +
                               " SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                               " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                               " ) addrDtl " +
                               " ON idOrganization = organizationId WHERE  orgTypeId=" + (int)Constants.OrgTypeE.DEALER +
                               " AND tblOrganization.idOrganization IN(SELECT distinct dealerOrgId FROM tblBookings WHERE pendingQty > 0 AND statusId IN(2,3,9,11))";


                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader tblOrgReader = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                if (tblOrgReader != null)
                {
                    while (tblOrgReader.Read())
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        if (tblOrgReader["idOrganization"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(tblOrgReader["idOrganization"].ToString());
                        if (tblOrgReader["dealerName"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(tblOrgReader["dealerName"].ToString());
                        dropDownTOList.Add(dropDownTO);
                    }
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public TblOrganizationTO SelectTblOrganizationTO(Int32 idOrganization)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idOrganization = " + idOrganization + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblOrganizationTO> list = ConvertDTToList(rdr);
                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectTblOrganization");
                return null;
            }
            finally
            {
                if (rdr != null)
                    rdr.Dispose();
                cmdSelect.Dispose();
            }
        }
        public TblOrganizationTO SelectTblOrganization(Int32 idOrganization, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idOrganization = " + idOrganization + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblOrganizationTO> list = ConvertDTToListV1(rdr);
                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectTblOrganization");
                return null;
            }
            finally
            {
                if (rdr != null)
                    rdr.Dispose();
                cmdSelect.Dispose();
            }
        }

        public Dictionary<int, string> SelectRegisteredMobileNoDCT(String orgIds, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            Dictionary<int, string> DCT = null;
            try
            {
                cmdSelect.CommandText = "SELECT idOrganization , registeredMobileNos FROM tblOrganization  WHERE idOrganization IN(" + orgIds + " ) ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                if (rdr != null)
                {
                    DCT = new Dictionary<int, string>();
                    while (rdr.Read())
                    {
                        Int32 orgId = 0;
                        string regMobNos = string.Empty;
                        if (rdr["idOrganization"] != DBNull.Value)
                            orgId = Convert.ToInt32(rdr["idOrganization"].ToString());
                        if (rdr["registeredMobileNos"] != DBNull.Value)
                            regMobNos = Convert.ToString(rdr["registeredMobileNos"].ToString());

                        if (orgId > 0 && !string.IsNullOrEmpty(regMobNos))
                        {
                            DCT.Add(orgId, regMobNos);
                        }
                    }

                    return DCT;
                }
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (rdr != null)
                    rdr.Dispose();
                cmdSelect.Dispose();
            }
        }

        public Dictionary<int, string> SelectRegisteredMobileNoDCTByOrgType(String orgTypeId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            Dictionary<int, string> DCT = null;
            try
            {
                cmdSelect.CommandText = "SELECT idOrganization , registeredMobileNos FROM tblOrganization  WHERE isActive=1 AND orgTypeId IN(" + orgTypeId + " ) ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                if (rdr != null)
                {
                    DCT = new Dictionary<int, string>();
                    while (rdr.Read())
                    {
                        Int32 orgId = 0;
                        string regMobNos = string.Empty;
                        if (rdr["idOrganization"] != DBNull.Value)
                            orgId = Convert.ToInt32(rdr["idOrganization"].ToString());
                        if (rdr["registeredMobileNos"] != DBNull.Value)
                            regMobNos = Convert.ToString(rdr["registeredMobileNos"].ToString());

                        if (orgId > 0 && !string.IsNullOrEmpty(regMobNos))
                        {
                            DCT.Add(orgId, regMobNos);
                        }
                    }

                    return DCT;
                }
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (rdr != null)
                    rdr.Dispose();
                cmdSelect.Dispose();
            }
        }

        public String SelectFirmNameOfOrganiationById(Int32 organizationId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "SELECT firmName FROM tblOrganization WHERE idOrganization=" + organizationId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                while (rdr.Read())
                {
                    TblOrganizationTO tblOrganizationTONew = new TblOrganizationTO();
                    if (rdr["firmName"] != DBNull.Value)
                        return Convert.ToString(rdr["firmName"].ToString());
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        public List<TblOrganizationTO> ConvertDTToList(SqlDataReader tblOrganizationTODT)
        {
            List<TblOrganizationTO> tblOrganizationTOList = new List<TblOrganizationTO>();
            if (tblOrganizationTODT != null)
            {
                var columns = tblOrganizationTODT.GetColumnSchema();
                while (tblOrganizationTODT.Read())
                {
                    TblOrganizationTO tblOrganizationTONew = new TblOrganizationTO();
                    if (tblOrganizationTODT["idOrganization"] != DBNull.Value)
                        tblOrganizationTONew.IdOrganization = Convert.ToInt32(tblOrganizationTODT["idOrganization"].ToString());
                    if (tblOrganizationTODT["orgTypeId"] != DBNull.Value)
                        tblOrganizationTONew.OrgTypeId = Convert.ToInt32(tblOrganizationTODT["orgTypeId"].ToString());
                    if (tblOrganizationTODT["addrId"] != DBNull.Value)
                        tblOrganizationTONew.AddrId = Convert.ToInt32(tblOrganizationTODT["addrId"].ToString());
                    if (tblOrganizationTODT["firstOwnerPersonId"] != DBNull.Value)
                        tblOrganizationTONew.FirstOwnerPersonId = Convert.ToInt32(tblOrganizationTODT["firstOwnerPersonId"].ToString());
                    if (tblOrganizationTODT["secondOwnerPersonId"] != DBNull.Value)
                        tblOrganizationTONew.SecondOwnerPersonId = Convert.ToInt32(tblOrganizationTODT["secondOwnerPersonId"].ToString());
                    if (tblOrganizationTODT["parentId"] != DBNull.Value)
                        tblOrganizationTONew.ParentId = Convert.ToInt32(tblOrganizationTODT["parentId"].ToString());
                    if (tblOrganizationTODT["createdBy"] != DBNull.Value)
                        tblOrganizationTONew.CreatedBy = Convert.ToInt32(tblOrganizationTODT["createdBy"].ToString());
                    if (tblOrganizationTODT["createdOn"] != DBNull.Value)
                        tblOrganizationTONew.CreatedOn = Convert.ToDateTime(tblOrganizationTODT["createdOn"].ToString());
                    if (tblOrganizationTODT["firmName"] != DBNull.Value)
                        tblOrganizationTONew.FirmName = Convert.ToString(tblOrganizationTODT["firmName"].ToString().Trim().TrimEnd(','));

                    if (tblOrganizationTODT["phoneNo"] != DBNull.Value)
                        tblOrganizationTONew.PhoneNo = Convert.ToString(tblOrganizationTODT["phoneNo"].ToString());
                    if (tblOrganizationTODT["faxNo"] != DBNull.Value)
                        tblOrganizationTONew.FaxNo = Convert.ToString(tblOrganizationTODT["faxNo"].ToString());
                    if (tblOrganizationTODT["emailAddr"] != DBNull.Value)
                        tblOrganizationTONew.EmailAddr = Convert.ToString(tblOrganizationTODT["emailAddr"].ToString());
                    if (tblOrganizationTODT["website"] != DBNull.Value)
                        tblOrganizationTONew.Website = Convert.ToString(tblOrganizationTODT["website"].ToString());

                    if (tblOrganizationTODT["registeredMobileNos"] != DBNull.Value)
                        tblOrganizationTONew.RegisteredMobileNos = Convert.ToString(tblOrganizationTODT["registeredMobileNos"].ToString());

                    if (tblOrganizationTODT["cdStructureId"] != DBNull.Value)
                        tblOrganizationTONew.CdStructureId = Convert.ToInt32(tblOrganizationTODT["cdStructureId"].ToString());
                    if (tblOrganizationTODT["cdValue"] != DBNull.Value)
                        tblOrganizationTONew.CdStructure = Convert.ToDouble(tblOrganizationTODT["cdValue"].ToString());
                    if (tblOrganizationTODT["delPeriodId"] != DBNull.Value)
                        tblOrganizationTONew.DelPeriodId = Convert.ToInt32(tblOrganizationTODT["delPeriodId"].ToString());
                    if (tblOrganizationTODT["deliveryPeriod"] != DBNull.Value)
                        tblOrganizationTONew.DeliveryPeriod = Convert.ToInt32(tblOrganizationTODT["deliveryPeriod"].ToString());

                    if (tblOrganizationTODT["isActive"] != DBNull.Value)
                        tblOrganizationTONew.IsActive = Convert.ToInt32(tblOrganizationTODT["isActive"].ToString());
                    if (tblOrganizationTODT["remark"] != DBNull.Value)
                        tblOrganizationTONew.Remark = Convert.ToString(tblOrganizationTODT["remark"].ToString());
                    if (tblOrganizationTODT["villageName"] != DBNull.Value)
                        tblOrganizationTONew.VillageName = Convert.ToString(tblOrganizationTODT["villageName"].ToString());
                    if (tblOrganizationTODT["isSpecialCnf"] != DBNull.Value)
                        tblOrganizationTONew.IsSpecialCnf = Convert.ToInt32(tblOrganizationTODT["isSpecialCnf"].ToString());

                    if (tblOrganizationTODT["digitalSign"] != DBNull.Value)
                        tblOrganizationTONew.DigitalSign = Convert.ToString(tblOrganizationTODT["digitalSign"].ToString());
                    if (tblOrganizationTODT["deactivatedOn"] != DBNull.Value)
                        tblOrganizationTONew.DeactivatedOn = Convert.ToDateTime(tblOrganizationTODT["deactivatedOn"].ToString());

                    if (tblOrganizationTODT["districtId"] != DBNull.Value)
                        tblOrganizationTONew.DistrictId = Convert.ToInt32(tblOrganizationTODT["districtId"].ToString());

                    if (tblOrganizationTODT["orgLogo"] != DBNull.Value)
                        tblOrganizationTONew.OrgLogo = Convert.ToString(tblOrganizationTODT["orgLogo"].ToString());

                    if (tblOrganizationTODT["rebMobNoCntryCode"] != DBNull.Value)
                        tblOrganizationTONew.RebMobNoCntryCode = Convert.ToString(tblOrganizationTODT["rebMobNoCntryCode"].ToString());

                    if (tblOrganizationTODT["firmCode"] != DBNull.Value)
                        tblOrganizationTONew.FirmCode = Convert.ToString(tblOrganizationTODT["firmCode"].ToString());

                    if (tblOrganizationTODT["creditLimit"] != DBNull.Value)
                        tblOrganizationTONew.CreditLimit = Convert.ToDouble(tblOrganizationTODT["creditLimit"].ToString());

                    if (tblOrganizationTODT["txnId"] != DBNull.Value)
                        tblOrganizationTONew.TxnId = Convert.ToString(tblOrganizationTODT["txnId"].ToString());

                    if (tblOrganizationTODT["consumerTypeId"] != DBNull.Value)
                        tblOrganizationTONew.ConsumerTypeId = Convert.ToInt32(tblOrganizationTODT["consumerTypeId"].ToString());
                    if (tblOrganizationTODT["consumerType"] != DBNull.Value)
                        tblOrganizationTONew.ConsumerType = Convert.ToString(tblOrganizationTODT["consumerType"].ToString());

                    if (tblOrganizationTONew.OrgTypeE == Constants.OrgTypeE.C_AND_F_AGENT)
                    {
                        if (tblOrganizationTODT["alloc_qty"] != DBNull.Value)
                            tblOrganizationTONew.LastAllocQty = Convert.ToDouble(tblOrganizationTODT["alloc_qty"].ToString());
                        if (tblOrganizationTODT["rate_band"] != DBNull.Value)
                            tblOrganizationTONew.LastRateBand = Convert.ToDouble(tblOrganizationTODT["rate_band"].ToString());
                        if (tblOrganizationTODT["balance_qty"] != DBNull.Value)
                            tblOrganizationTONew.BalanceQuota = Convert.ToDouble(tblOrganizationTODT["balance_qty"].ToString());
                        if (tblOrganizationTODT["validUpto"] != DBNull.Value)
                            tblOrganizationTONew.ValidUpto = Convert.ToInt32(tblOrganizationTODT["validUpto"].ToString());
                        if (tblOrganizationTODT["idQuotaDeclaration"] != DBNull.Value)
                            tblOrganizationTONew.QuotaDeclarationId = Convert.ToInt32(tblOrganizationTODT["idQuotaDeclaration"].ToString());
                        if (tblOrganizationTODT["rate"] != DBNull.Value)
                            tblOrganizationTONew.DeclaredRate = Convert.ToDouble(tblOrganizationTODT["rate"].ToString());

                    }

                    //Added By Gokul [08-03-21]
                    if (tblOrganizationTODT["userId"] != DBNull.Value)
                        tblOrganizationTONew.UserId = Convert.ToInt32(tblOrganizationTODT["userId"].ToString());

                    if (tblOrganizationTODT["isDefaultPM"] != DBNull.Value)
                        tblOrganizationTONew.IsDefaultPM = Convert.ToInt32(tblOrganizationTODT["isDefaultPM"].ToString());
                    if (tblOrganizationTODT["userDisplayName"] != DBNull.Value)
                        tblOrganizationTONew.UserDisplayName = Convert.ToString(tblOrganizationTODT["userDisplayName"].ToString());


                    //if (tblOrganizationTONew.OrgTypeE == Constants.OrgTypeE.COMPETITOR)
                    //{
                    //    tblOrganizationTONew.CompetitorUpdatesTO = new Models.TblCompetitorUpdatesTO();
                    //    if (tblOrganizationTODT["updateDatetime"] != DBNull.Value)
                    //        tblOrganizationTONew.CompetitorUpdatesTO.UpdateDatetime = Convert.ToDateTime(tblOrganizationTODT["updateDatetime"].ToString());
                    //    if (tblOrganizationTODT["price"] != DBNull.Value)
                    //        tblOrganizationTONew.CompetitorUpdatesTO.Price = Convert.ToDouble(tblOrganizationTODT["price"].ToString());
                    //    if (tblOrganizationTODT["lastPrice"] != DBNull.Value)
                    //        tblOrganizationTONew.CompetitorUpdatesTO.LastPrice = Convert.ToDouble(tblOrganizationTODT["lastPrice"].ToString());
                    //    if (tblOrganizationTODT["firmName"] != DBNull.Value)
                    //        tblOrganizationTONew.CompetitorUpdatesTO.FirmName = Convert.ToString(tblOrganizationTODT["firmName"].ToString());
                    //    if (tblOrganizationTODT["informerName"] != DBNull.Value)
                    //        tblOrganizationTONew.CompetitorUpdatesTO.InformerName = Convert.ToString(tblOrganizationTODT["informerName"].ToString());
                    //    if (tblOrganizationTODT["alternateInformerName"] != DBNull.Value)
                    //        tblOrganizationTONew.CompetitorUpdatesTO.AlternateInformerName = Convert.ToString(tblOrganizationTODT["alternateInformerName"].ToString());

                    //    if (tblOrganizationTODT["brandName"] != DBNull.Value)
                    //        tblOrganizationTONew.CompetitorUpdatesTO.BrandName = Convert.ToString(tblOrganizationTODT["brandName"].ToString());
                    //    if (tblOrganizationTODT["prodCapacityMT"] != DBNull.Value)
                    //        tblOrganizationTONew.CompetitorUpdatesTO.ProdCapacityMT = Convert.ToDouble(tblOrganizationTODT["prodCapacityMT"].ToString());
                    //    if (tblOrganizationTODT["competitorExtId"] != DBNull.Value)
                    //        tblOrganizationTONew.CompetitorUpdatesTO.CompetitorExtId = Convert.ToInt32(tblOrganizationTODT["competitorExtId"].ToString());
                    //    if (tblOrganizationTODT["competitorOrgId"] != DBNull.Value)
                    //        tblOrganizationTONew.CompetitorUpdatesTO.CompetitorOrgId = Convert.ToInt32(tblOrganizationTODT["competitorOrgId"].ToString());


                    //}

                    if (tblOrganizationTODT["overdue_ref_id"] != DBNull.Value)
                        tblOrganizationTONew.OverdueRefId = Convert.ToString(tblOrganizationTODT["overdue_ref_id"].ToString());

                    if (tblOrganizationTODT["enq_ref_id"] != DBNull.Value)
                        tblOrganizationTONew.EnqRefId = Convert.ToString(tblOrganizationTODT["enq_ref_id"].ToString());

                    if (tblOrganizationTODT["firmTypeId"] != DBNull.Value)
                        tblOrganizationTONew.FirmTypeId = Convert.ToInt32(tblOrganizationTODT["firmTypeId"].ToString());

                    if (tblOrganizationTODT["influencerTypeId"] != DBNull.Value)
                        tblOrganizationTONew.InfluencerTypeId = Convert.ToInt32(tblOrganizationTODT["influencerTypeId"].ToString());

                    //Priyanka [07-06-2018] : Added for SHIVANGI
                    if (tblOrganizationTODT["isOverdueExist"] != DBNull.Value)
                        tblOrganizationTONew.IsOverdueExist = Convert.ToInt32(tblOrganizationTODT["isOverdueExist"].ToString());

                    //Sudhir[22-APR-2018]
                    if (tblOrganizationTODT["dateOfEstablishment"] != DBNull.Value)
                        tblOrganizationTONew.DateOfEstablishment = Convert.ToDateTime(tblOrganizationTODT["dateOfEstablishment"].ToString());

                    //Priyanka [13-09-2019]
                    if (tblOrganizationTODT["suppDivGroupId"] != DBNull.Value)
                        tblOrganizationTONew.SuppDivGroupId = Convert.ToInt32(tblOrganizationTODT["suppDivGroupId"].ToString());
                    if (tblOrganizationTODT["isRegUnderGST"] != DBNull.Value)
                        tblOrganizationTONew.IsRegUnderGST = Convert.ToInt32(tblOrganizationTODT["isRegUnderGST"].ToString());

                    if (tblOrganizationTODT["orgStatusId"] != DBNull.Value)
                    {
                        tblOrganizationTONew.StatusId = Convert.ToInt32(tblOrganizationTODT["orgStatusId"].ToString());
                        tblOrganizationTONew.OrgStatusId = Convert.ToInt32(tblOrganizationTODT["orgStatusId"].ToString()); //[20220105] Dhananjay Added
                    }
                    if (tblOrganizationTODT["statusName"] != DBNull.Value)
                        tblOrganizationTONew.OrgStatusName = Convert.ToString(tblOrganizationTODT["statusName"].ToString());

                    if (tblOrganizationTODT["colorCode"] != DBNull.Value)
                        tblOrganizationTONew.StatusColorCode = Convert.ToString(tblOrganizationTODT["colorCode"].ToString());

                    if (tblOrganizationTODT["isBlocked"] != DBNull.Value)
                        tblOrganizationTONew.IsBlocked = Convert.ToInt32(tblOrganizationTODT["isBlocked"].ToString());

                    if (tblOrganizationTODT["firmType"] != DBNull.Value)
                        tblOrganizationTONew.FirmType = Convert.ToString(tblOrganizationTODT["firmType"].ToString());

                    if (tblOrganizationTODT["licenseValue"] != DBNull.Value)
                        tblOrganizationTONew.GstNo = Convert.ToString(tblOrganizationTODT["licenseValue"].ToString());

                    if (tblOrganizationTODT["currencyId"] != DBNull.Value)
                        tblOrganizationTONew.CurrencyId = Convert.ToInt32(tblOrganizationTODT["currencyId"].ToString());

                    if (tblOrganizationTODT["orgGrpTypeId"] != DBNull.Value)
                        tblOrganizationTONew.OrgGroupTypeId = Convert.ToInt32(tblOrganizationTODT["orgGrpTypeId"].ToString());

                    if (tblOrganizationTODT["isTcsApplicable"] != DBNull.Value)
                        tblOrganizationTONew.IsTcsApplicable = Convert.ToInt32(tblOrganizationTODT["isTcsApplicable"].ToString());
                    if (tblOrganizationTODT["isDeclarationRec"] != DBNull.Value)
                        tblOrganizationTONew.IsDeclarationRec = Convert.ToInt32(tblOrganizationTODT["isDeclarationRec"].ToString());
                    if (tblOrganizationTODT["tdsPct"] != DBNull.Value)
                        tblOrganizationTONew.TdsPct = Convert.ToDouble(tblOrganizationTODT["tdsPct"].ToString());
                    try
                    {
                        if (tblOrganizationTODT["districtName"] != DBNull.Value)
                            tblOrganizationTONew.DistrictName = Convert.ToString(tblOrganizationTODT["districtName"].ToString());
                    }
                    catch (Exception ex) { }
                    try { 
                    if (tblOrganizationTODT["talukaName"] != DBNull.Value)
                        tblOrganizationTONew.TalukaName = Convert.ToString(tblOrganizationTODT["talukaName"].ToString());
                    }
                    catch (Exception ex) { }
                    try { 
                    if (tblOrganizationTODT["stateName"] != DBNull.Value)
                        tblOrganizationTONew.StateName = Convert.ToString(tblOrganizationTODT["stateName"].ToString());
                    }
                    catch (Exception ex) { }
                    try { 
                    if (tblOrganizationTODT["distributerName"] != DBNull.Value)
                        tblOrganizationTONew.DistributerName = Convert.ToString(tblOrganizationTODT["distributerName"].ToString());
                    }
                    catch (Exception ex) { }
                    try
                    {
                        if (tblOrganizationTODT["scrapCapacity"] != DBNull.Value)
                            tblOrganizationTONew.ScrapCapacity = (decimal)tblOrganizationTODT["scrapCapacity"];
                    }
                    catch (Exception ex) { }
                    try
                    {
                        if (tblOrganizationTODT["supplierGrade"] != DBNull.Value)
                            tblOrganizationTONew.supplierGrade = (Int32)tblOrganizationTODT["supplierGrade"];
                    }
                    catch (Exception ex) { }
                    //changed by binal [17/02/2023]	 

                    if (columns.Any(x => x.ColumnName.ToLower() == "totalcount") && tblOrganizationTODT["totalCount"] != DBNull.Value)
                        tblOrganizationTONew.TotalCount = Convert.ToInt32(tblOrganizationTODT["totalCount"].ToString());
                    if (columns.Any(x => x.ColumnName.ToLower() == "searchallcount") && tblOrganizationTODT["searchAllCount"] != DBNull.Value)
                        tblOrganizationTONew.SearchAllCount = Convert.ToInt32(tblOrganizationTODT["searchAllCount"].ToString());
                    if (columns.Any(x => x.ColumnName.ToLower() == "rownumber") && tblOrganizationTODT["rowNumber"] != DBNull.Value)
                        tblOrganizationTONew.RowNumber = Convert.ToInt32(tblOrganizationTODT["rowNumber"].ToString());


                    //foreach (DataRow item in dt.Rows)
                    //{
                    //    string ColumnName = item.ItemArray[0].ToString();
                    //    if (ColumnName == "TotalCount")
                    //    {
                    //        if (tblOrganizationTODT["totalCount"] != DBNull.Value)
                    //            tblOrganizationTONew.TotalCount = Convert.ToInt32(tblOrganizationTODT["totalCount"].ToString());
                    //    }
                    //    if (ColumnName == "SearchAllCount")
                    //    {
                    //        if (tblOrganizationTODT["searchAllCount"] != DBNull.Value)
                    //            tblOrganizationTONew.SearchAllCount = Convert.ToInt32(tblOrganizationTODT["searchAllCount"].ToString());
                    //    }
                    //    if (ColumnName == "RowNumber")
                    //    {
                    //        if (tblOrganizationTODT["rowNumber"] != DBNull.Value)
                    //            tblOrganizationTONew.RowNumber = Convert.ToInt32(tblOrganizationTODT["rowNumber"].ToString());
                    //    }
                    //}
                    tblOrganizationTOList.Add(tblOrganizationTONew);
                }
            }
            return tblOrganizationTOList;
        }
        //changed by binal [31/03/2023]	
        public List<TblOrganizationTO> ConvertDTToListCount(SqlDataReader tblOrganizationTODT)
        {
            List<TblOrganizationTO> tblOrganizationTOList = new List<TblOrganizationTO>();
            if (tblOrganizationTODT != null)
            {
                while (tblOrganizationTODT.Read())
                {
                    TblOrganizationTO tblOrganizationTONew = new TblOrganizationTO();
                    DataTable dt = tblOrganizationTODT.GetSchemaTable();
                    foreach (DataRow item in dt.Rows)
                    {
                        string ColumnName = item.ItemArray[0].ToString();
                        if (ColumnName == "TotalCount")
                        {
                            if (tblOrganizationTODT["totalCount"] != DBNull.Value)
                                tblOrganizationTONew.TotalCount = Convert.ToInt32(tblOrganizationTODT["totalCount"].ToString());
                        }
                        tblOrganizationTOList.Add(tblOrganizationTONew);
                        break;
                    }
                    break;
                }
            }
            return tblOrganizationTOList;
        }

        public List<TblOrganizationTO> ConvertDTToListV1(SqlDataReader tblOrganizationTODT)
        {
            List<TblOrganizationTO> tblOrganizationTOList = new List<TblOrganizationTO>();
            if (tblOrganizationTODT != null)
            {
                while (tblOrganizationTODT.Read())
                {
                    TblOrganizationTO tblOrganizationTONew = new TblOrganizationTO();
                    if (tblOrganizationTODT["idOrganization"] != DBNull.Value)
                        tblOrganizationTONew.IdOrganization = Convert.ToInt32(tblOrganizationTODT["idOrganization"].ToString());
                    if (tblOrganizationTODT["orgTypeId"] != DBNull.Value)
                        tblOrganizationTONew.OrgTypeId = Convert.ToInt32(tblOrganizationTODT["orgTypeId"].ToString());
                    if (tblOrganizationTODT["addrId"] != DBNull.Value)
                        tblOrganizationTONew.AddrId = Convert.ToInt32(tblOrganizationTODT["addrId"].ToString());
                    if (tblOrganizationTODT["firstOwnerPersonId"] != DBNull.Value)
                        tblOrganizationTONew.FirstOwnerPersonId = Convert.ToInt32(tblOrganizationTODT["firstOwnerPersonId"].ToString());
                    if (tblOrganizationTODT["secondOwnerPersonId"] != DBNull.Value)
                        tblOrganizationTONew.SecondOwnerPersonId = Convert.ToInt32(tblOrganizationTODT["secondOwnerPersonId"].ToString());
                    if (tblOrganizationTODT["parentId"] != DBNull.Value)
                        tblOrganizationTONew.ParentId = Convert.ToInt32(tblOrganizationTODT["parentId"].ToString());
                    if (tblOrganizationTODT["createdBy"] != DBNull.Value)
                        tblOrganizationTONew.CreatedBy = Convert.ToInt32(tblOrganizationTODT["createdBy"].ToString());
                    if (tblOrganizationTODT["createdOn"] != DBNull.Value)
                        tblOrganizationTONew.CreatedOn = Convert.ToDateTime(tblOrganizationTODT["createdOn"].ToString());
                    if (tblOrganizationTODT["firmName"] != DBNull.Value)
                        tblOrganizationTONew.FirmName = Convert.ToString(tblOrganizationTODT["firmName"].ToString().Trim().TrimEnd(','));

                    if (tblOrganizationTODT["phoneNo"] != DBNull.Value)
                        tblOrganizationTONew.PhoneNo = Convert.ToString(tblOrganizationTODT["phoneNo"].ToString());
                    if (tblOrganizationTODT["faxNo"] != DBNull.Value)
                        tblOrganizationTONew.FaxNo = Convert.ToString(tblOrganizationTODT["faxNo"].ToString());
                    if (tblOrganizationTODT["emailAddr"] != DBNull.Value)
                        tblOrganizationTONew.EmailAddr = Convert.ToString(tblOrganizationTODT["emailAddr"].ToString());
                    if (tblOrganizationTODT["website"] != DBNull.Value)
                        tblOrganizationTONew.Website = Convert.ToString(tblOrganizationTODT["website"].ToString());

                    if (tblOrganizationTODT["registeredMobileNos"] != DBNull.Value)
                        tblOrganizationTONew.RegisteredMobileNos = Convert.ToString(tblOrganizationTODT["registeredMobileNos"].ToString());

                    if (tblOrganizationTODT["cdStructureId"] != DBNull.Value)
                        tblOrganizationTONew.CdStructureId = Convert.ToInt32(tblOrganizationTODT["cdStructureId"].ToString());
                    if (tblOrganizationTODT["cdValue"] != DBNull.Value)
                        tblOrganizationTONew.CdStructure = Convert.ToDouble(tblOrganizationTODT["cdValue"].ToString());
                    if (tblOrganizationTODT["delPeriodId"] != DBNull.Value)
                        tblOrganizationTONew.DelPeriodId = Convert.ToInt32(tblOrganizationTODT["delPeriodId"].ToString());
                    if (tblOrganizationTODT["deliveryPeriod"] != DBNull.Value)
                        tblOrganizationTONew.DeliveryPeriod = Convert.ToInt32(tblOrganizationTODT["deliveryPeriod"].ToString());

                    if (tblOrganizationTODT["isActive"] != DBNull.Value)
                        tblOrganizationTONew.IsActive = Convert.ToInt32(tblOrganizationTODT["isActive"].ToString());
                    if (tblOrganizationTODT["remark"] != DBNull.Value)
                        tblOrganizationTONew.Remark = Convert.ToString(tblOrganizationTODT["remark"].ToString());
                    if (tblOrganizationTODT["villageName"] != DBNull.Value)
                        tblOrganizationTONew.VillageName = Convert.ToString(tblOrganizationTODT["villageName"].ToString());
                    if (tblOrganizationTODT["isSpecialCnf"] != DBNull.Value)
                        tblOrganizationTONew.IsSpecialCnf = Convert.ToInt32(tblOrganizationTODT["isSpecialCnf"].ToString());

                    if (tblOrganizationTODT["digitalSign"] != DBNull.Value)
                        tblOrganizationTONew.DigitalSign = Convert.ToString(tblOrganizationTODT["digitalSign"].ToString());
                    if (tblOrganizationTODT["deactivatedOn"] != DBNull.Value)
                        tblOrganizationTONew.DeactivatedOn = Convert.ToDateTime(tblOrganizationTODT["deactivatedOn"].ToString());

                    if (tblOrganizationTODT["districtId"] != DBNull.Value)
                        tblOrganizationTONew.DistrictId = Convert.ToInt32(tblOrganizationTODT["districtId"].ToString());

                    if (tblOrganizationTODT["orgLogo"] != DBNull.Value)
                        tblOrganizationTONew.OrgLogo = Convert.ToString(tblOrganizationTODT["orgLogo"].ToString());

                    if (tblOrganizationTODT["rebMobNoCntryCode"] != DBNull.Value)
                        tblOrganizationTONew.RebMobNoCntryCode = Convert.ToString(tblOrganizationTODT["rebMobNoCntryCode"].ToString());

                    if (tblOrganizationTODT["firmCode"] != DBNull.Value)
                        tblOrganizationTONew.FirmCode = Convert.ToString(tblOrganizationTODT["firmCode"].ToString());

                    if (tblOrganizationTODT["creditLimit"] != DBNull.Value)
                        tblOrganizationTONew.CreditLimit = Convert.ToDouble(tblOrganizationTODT["creditLimit"].ToString());

                    if (tblOrganizationTODT["txnId"] != DBNull.Value)
                        tblOrganizationTONew.TxnId = Convert.ToString(tblOrganizationTODT["txnId"].ToString());



                    if (tblOrganizationTODT["overdue_ref_id"] != DBNull.Value)
                        tblOrganizationTONew.OverdueRefId = Convert.ToString(tblOrganizationTODT["overdue_ref_id"].ToString());

                    if (tblOrganizationTODT["enq_ref_id"] != DBNull.Value)
                        tblOrganizationTONew.EnqRefId = Convert.ToString(tblOrganizationTODT["enq_ref_id"].ToString());

                    if (tblOrganizationTODT["firmTypeId"] != DBNull.Value)
                        tblOrganizationTONew.FirmTypeId = Convert.ToInt32(tblOrganizationTODT["firmTypeId"].ToString());

                    if (tblOrganizationTODT["influencerTypeId"] != DBNull.Value)
                        tblOrganizationTONew.InfluencerTypeId = Convert.ToInt32(tblOrganizationTODT["influencerTypeId"].ToString());

                    //Priyanka [07-06-2018] : Added for SHIVANGI
                    if (tblOrganizationTODT["isOverdueExist"] != DBNull.Value)
                        tblOrganizationTONew.IsOverdueExist = Convert.ToInt32(tblOrganizationTODT["isOverdueExist"].ToString());

                    //Sudhir[22-APR-2018]
                    if (tblOrganizationTODT["dateOfEstablishment"] != DBNull.Value)
                        tblOrganizationTONew.DateOfEstablishment = Convert.ToDateTime(tblOrganizationTODT["dateOfEstablishment"].ToString());

                    //Priyanka [13-09-2019]
                    if (tblOrganizationTODT["suppDivGroupId"] != DBNull.Value)
                        tblOrganizationTONew.SuppDivGroupId = Convert.ToInt32(tblOrganizationTODT["suppDivGroupId"].ToString());
                    if (tblOrganizationTODT["isRegUnderGST"] != DBNull.Value)
                        tblOrganizationTONew.IsRegUnderGST = Convert.ToInt32(tblOrganizationTODT["isRegUnderGST"].ToString());

                    if (tblOrganizationTODT["orgStatusId"] != DBNull.Value)
                    {
                        tblOrganizationTONew.StatusId = Convert.ToInt32(tblOrganizationTODT["orgStatusId"].ToString());
                        tblOrganizationTONew.OrgStatusId = Convert.ToInt32(tblOrganizationTODT["orgStatusId"].ToString()); //[20220105] Dhananjay Added
                    }
                    if (tblOrganizationTODT["statusName"] != DBNull.Value)
                        tblOrganizationTONew.OrgStatusName = Convert.ToString(tblOrganizationTODT["statusName"].ToString());

                    if (tblOrganizationTODT["colorCode"] != DBNull.Value)
                        tblOrganizationTONew.StatusColorCode = Convert.ToString(tblOrganizationTODT["colorCode"].ToString());

                    if (tblOrganizationTODT["isBlocked"] != DBNull.Value)
                        tblOrganizationTONew.IsBlocked = Convert.ToInt32(tblOrganizationTODT["isBlocked"].ToString());

                    if (tblOrganizationTODT["firmType"] != DBNull.Value)
                        tblOrganizationTONew.FirmType = Convert.ToString(tblOrganizationTODT["firmType"].ToString());

                    if (tblOrganizationTODT["firstOwnerName"] != DBNull.Value)
                        tblOrganizationTONew.FirstOwnerName = Convert.ToString(tblOrganizationTODT["firstOwnerName"].ToString());

                    tblOrganizationTOList.Add(tblOrganizationTONew);
                }
            }
            return tblOrganizationTOList;
        }

        public List<OrgExportRptTO> SelectAllOrgListToExport(Int32 orgTypeId, Int32 parentId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            string sqlQuery = string.Empty;
            try
            {
                conn.Open();

                sqlQuery = " SELECT tblOrganization.* , dimCdStructure.cdValue , dimDelPeriod.deliveryPeriod, addrDtl.*, " +
                           " provGstDtl.licenseValue as provGSTNo, permGstDtl.licenseValue as permGSTNo,orgStatusName.statusName,  " +
                           " panDtl.licenseValue AS PanNO ,cnfDtl.firmName as CnfName " +
                           " ,ISNULL(tblPerson.firstName,'') AS foFirstName,ISNULL(tblPerson.midName,'') AS foMidName,ISNULL(tblPerson.lastName,'') AS foLastName " +
                           " ,ISNULL(tblPerson.mobileNo,'') AS foMobileNo,ISNULL(tblPerson.alternateMobNo,'') AS foAlternateMobNo,ISNULL(tblPerson.phoneNo,'') AS foPhoneNo " +
                           " ,tblPerson.dateOfBirth AS foDateOfBirth,ISNULL(tblPerson.primaryEmail,'') AS foPrimaryEmail,ISNULL(tblPerson.alternateEmail,'') AS foAlternateEmail " +
                           " ,ISNULL(soPerson.firstName,'') AS soFirstName,ISNULL(soPerson.midName,'') AS soMidName,ISNULL(soPerson.lastName,'') AS soLastName " +
                           " ,ISNULL(soPerson.mobileNo,'') AS soMobileNo,ISNULL(soPerson.alternateMobNo,'') AS soAlternateMobNo,ISNULL(soPerson.phoneNo,'') AS soPhoneNo " +
                           " ,soPerson.dateOfBirth AS soDateOfBirth,ISNULL(soPerson.primaryEmail,'') AS soPrimaryEmail,ISNULL(soPerson.alternateEmail,'') AS soAlternateEmail " +
                           " ,CASE WHEN LEN(tblOrganization.registeredMobileNos) > 10 OR LEN(tblOrganization.registeredMobileNos) <= 2 THEN SUBSTRING(tblOrganization.registeredMobileNos, 3, 8000) ELSE tblOrganization.registeredMobileNos END AS smsNo " +
                           " , CASE WHEN LEN(tblPerson.mobileNo) > 10 OR LEN(tblPerson.mobileNo) <= 2 THEN SUBSTRING(tblPerson.mobileNo, 3, 8000) ELSE tblPerson.mobileNo END AS OwnerNo " +
                           ", purchaseManager " +
                           " FROM tblOrganization " +
                           " LEFT JOIN " +
                           " ( " +
                           "     SELECT tblOrgAddress.organizationId, tblAddress.* , ISNULL(districtName,'') districtName, ISNULL(talukaName,'') talukaName, ISNULL(stateName,'') stateName " +
                           "     FROM tblOrgAddress " +
                           "     LEFT JOIN tblAddress ON idAddr = addressId " +
                           "     LEFT JOIN dimDistrict On idDistrict= districtId " +
                           "     LEFT JOIN dimTaluka On idTaluka= talukaId " +
                           "     LEFT JOIN dimState On idState= tblAddress.stateId " +
                           "     WHERE addrTypeId = " + (int)Constants.AddressTypeE.OFFICE_ADDRESS + " " +
                           " ) AS addrDtl ON addrDtl.organizationId = idOrganization " +
                           " LEFT JOIN " +
                           " ( " +
                           "     SELECT organizationId,addressId, licenseValue FROM tblOrgLicenseDtl WHERE licenseId IN (" + (int)Constants.CommercialLicenseE.SGST_NO + ") " +
                           " ) as provGstDtl On provGstDtl.addressId = tblOrganization.addrId " +
                           " LEFT JOIN " +
                           " ( " +
                           "     SELECT organizationId,addressId, licenseValue FROM tblOrgLicenseDtl WHERE licenseId IN (" + (int)Constants.CommercialLicenseE.IGST_NO + ") " +
                           " ) as permGstDtl On permGstDtl.addressId = tblOrganization.addrId " +
                           " LEFT JOIN " +
                           " ( " +
                           "     SELECT organizationId,addressId, licenseValue FROM tblOrgLicenseDtl WHERE licenseId IN (" + (int)Constants.CommercialLicenseE.PAN_NO + ") " +
                           " ) AS panDtl " +
                           " ON panDtl.addressId = tblOrganization.addrId " +
                           " LEFT JOIN tblOrganization cnfDtl " +
                           " ON cnfDtl.idOrganization = tblOrganization.parentId " +
                           " LEFT JOIN tblPerson ON idPerson = tblOrganization.firstOwnerPersonId " +
                           " LEFT JOIN tblPerson soPerson ON soPerson.idPerson = tblOrganization.secondOwnerPersonId " +
                           " LEFT JOIN dimCdStructure ON idCdStructure = tblOrganization.cdStructureId " +
                           " LEFT JOIN dimDelPeriod ON idDelPeriod = tblOrganization.delPeriodId " +
                           " LEFT JOIN dimStatus orgStatusName  on orgStatusName.idStatus = tblOrganization.orgStatusId" +
                           //Prajakta[2020-10-12] Added to show purchase manager of supplier
                           " LEFT JOIN( " +
                           " SELECT  pmOrg.idOrganization, " +
                           " STUFF((SELECT '; ' + tblUser.userDisplayName " +
                           " FROM tblPurchaseManagerSupplier tblPurchaseManagerSupplier " +
                           " left join tblUser tblUser  on tblUser.idUser = tblPurchaseManagerSupplier.userId " +
                           " WHERE tblPurchaseManagerSupplier.organizationId = pmOrg.idOrganization " +
                           " and tblUser.isActive = 1 and tblPurchaseManagerSupplier.isActive = 1 " +
                           " FOR XML PATH('')), 1, 1, '')[purchaseManager]" +
                           " FROM tblOrganization pmOrg" +
                           " GROUP BY pmOrg.idOrganization) finalPm" +
                           " ON finalPm.idOrganization = tblOrganization.idOrganization" +

                           " WHERE tblOrganization.orgTypeId = " + orgTypeId + " AND tblOrganization.isActive = 1";


                if (parentId > 0)
                    cmdSelect.CommandText = sqlQuery + " AND tblOrganization.parentId=" + parentId + " ORDER BY tblOrganization.firmName";
                else
                    cmdSelect.CommandText = sqlQuery;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertReaderToList(rdr);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        /// <summary>
        /// Sudhir[26-July-2018] --Add this Method for District & field officer link to be establish. 
        ///                        Regional manger can see his field office visit list. 
        ///                        Also field office can see their own visits
        /// </summary>
        /// <param name="cnfId"></param>
        /// <returns></returns>
        public List<DropDownTO> SelectDealerListForDropDownForCRM(Int32 cnfId, TblUserRoleTO tblUserRoleTO, int orgTypeId = 0)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String sqlQuery = String.Empty;
            //TblUserRoleTO userRoleTO=BL.TblUserRoleBL.SelectTblUserRoleTO()
            int isConfEn = 0;
            int userId = 0;
            if (tblUserRoleTO != null)
            {
                isConfEn = tblUserRoleTO.EnableAreaAlloc;
                userId = tblUserRoleTO.UserId;
            }

            try
            {
                conn.Open();

                if (cnfId > 0)
                {
                    if (isConfEn == 0)
                    {
                        sqlQuery = " SELECT DISTINCT idOrganization, CASE WHEN villageName IS NULL then  firmName " +
                                   " ELSE firmName +',' + villageName END AS dealerName " +
                                   " FROM tblOrganization " +
                                   " INNER JOIN tblCnfDealers ON dealerOrgId=idOrganization" +
                                   " INNER JOIN " +
                                   " ( " +
                                   " SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                                   " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                                   " ) addrDtl " +
                                   " ON idOrganization = organizationId WHERE tblOrganization.isActive=1 AND tblCnfDealers.isActive=1 AND orgTypeId=" + (int)Constants.OrgTypeE.DEALER + " AND cnfOrgId=" + cnfId;
                    }
                    else
                    {
                        sqlQuery = " SELECT DISTINCT idOrganization, CASE WHEN villageName IS NULL then  firmName " +
                                   " ELSE firmName +',' + villageName END AS dealerName " +
                                   " FROM tblOrganization " +
                                   " INNER JOIN tblCnfDealers ON dealerOrgId=idOrganization" +
                                   " INNER JOIN " +
                                   " ( " +
                                   " SELECT tblAddress.*,organizationId FROM tblOrgAddress " +
                                   " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                                   " ) addrDtl " +
                                   " ON idOrganization = organizationId " +
                                   " INNER JOIN tblUserAreaAllocation areaConf ON addrDtl.districtId=areaConf.districtId" +
                                   " AND areaConf.cnfOrgId=tblCnfDealers.cnfOrgId " +
                                   " WHERE  tblOrganization.isActive=1 AND tblCnfDealers.isActive=1 AND areaConf.cnfOrgId=" + cnfId + " AND areaConf.userId=" + userId + " AND areaConf.isActive=1 ";
                        if (orgTypeId > 0)
                        {
                            sqlQuery += "AND orgTypeId=" + orgTypeId;

                        }
                        else
                        {
                            sqlQuery += "AND orgTypeId=" + (int)Constants.OrgTypeE.DEALER;
                        }
                    }
                }
                else
                {
                    if (isConfEn == 0)
                    {
                        sqlQuery = " SELECT idOrganization,CASE WHEN villageName IS NULL then  firmName ELSE firmName + ',' + villageName END " +
                                  " AS dealerName, isSpecialCnf FROM tblOrganization LEFT JOIN(SELECT tblAddress.*, organizationId FROM tblOrgAddress  " +
                                  " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1) addrDtl " +
                                  " ON idOrganization = organizationId  WHERE isActive = 1 ";

                        if (orgTypeId > 0)
                        {
                            sqlQuery += "AND orgTypeId=" + orgTypeId;

                        }
                        else
                        {
                            sqlQuery += "AND orgTypeId= 2";
                        }
                    }
                    else
                    {
                        //sqlQuery = " SELECT DISTINCT idOrganization, CASE WHEN villageName IS NULL then  firmName " +
                        //           " ELSE firmName +',' + villageName END AS dealerName " +
                        //           " FROM tblOrganization " +
                        //           " INNER JOIN tblCnfDealers ON dealerOrgId=idOrganization" +
                        //           " INNER JOIN " +
                        //           " ( " +
                        //           " SELECT tblAddress.*,organizationId FROM tblOrgAddress " +
                        //           " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                        //           " ) addrDtl " +
                        //           " ON idOrganization = organizationId " +
                        //           " INNER JOIN tblUserAreaAllocation areaConf ON addrDtl.districtId=areaConf.districtId" +
                        //           " AND areaConf.cnfOrgId=tblCnfDealers.cnfOrgId " +
                        //           "WHERE  tblOrganization.isActive=1 AND tblCnfDealers.isActive=1 AND orgTypeId=" + (int)Constants.OrgTypeE.DEALER + " AND areaConf.userId=" + userId + " AND areaConf.isActive=1 ";


                        if (orgTypeId > 0)
                        {
                            sqlQuery = " SELECT idOrganization,CASE WHEN villageName IS NULL then  firmName ELSE firmName + ',' + villageName END " +
                                      " AS dealerName, isSpecialCnf FROM tblOrganization LEFT JOIN(SELECT tblAddress.*, organizationId FROM tblOrgAddress  " +
                                      " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1) addrDtl " +
                                      " ON idOrganization = organizationId  WHERE isActive = 1 ";

                            if (orgTypeId > 0)
                            {
                                sqlQuery += "AND orgTypeId=" + orgTypeId;

                            }
                        }
                        else
                        {
                            sqlQuery = " SELECT DISTINCT idOrganization, " +
                                  " CASE WHEN villageName IS NULL then firmName  ELSE firmName +',' + villageName END AS dealerName " +
                                  " FROM tblOrganization " +
                                  " INNER JOIN   (    SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                                  " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1) addrDtl ON idOrganization = organizationId " +
                                  " INNER JOIN tblUserAreaAllocation areaConf ON addrDtl.districtId = areaConf.districtId " +
                                  " WHERE tblOrganization.isActive = 1 AND areaConf.userId = " + userId + " AND areaConf.isActive = 1 ";


                            sqlQuery += "AND orgTypeId= " + (int)Constants.OrgTypeE.DEALER;
                        }

                    }
                }

                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader tblOrgReader = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                if (tblOrgReader != null)
                {
                    while (tblOrgReader.Read())
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        if (tblOrgReader["idOrganization"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(tblOrgReader["idOrganization"].ToString());
                        if (tblOrgReader["dealerName"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(tblOrgReader["dealerName"].ToString());
                        dropDownTOList.Add(dropDownTO);
                    }
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                Constants.LoggerObj.LogError(1, ex, "Error in Method SelectAllOrganizationListForDropDown at DAO", cnfId);
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<OrgExportRptTO> ConvertReaderToList(SqlDataReader tblOrganizationTODT)
        {
            List<OrgExportRptTO> orgExportRptTOList = new List<OrgExportRptTO>();
            if (tblOrganizationTODT != null)
            {
                while (tblOrganizationTODT.Read())
                {
                    OrgExportRptTO tblOrganizationTONew = new OrgExportRptTO();
                    if (tblOrganizationTODT["idOrganization"] != DBNull.Value)
                        tblOrganizationTONew.IdOrganization = Convert.ToInt32(tblOrganizationTODT["idOrganization"].ToString());
                    if (tblOrganizationTODT["firmName"] != DBNull.Value)
                        tblOrganizationTONew.FirmName = Convert.ToString(tblOrganizationTODT["firmName"].ToString());
                    if (tblOrganizationTODT["CnfName"] != DBNull.Value)
                        tblOrganizationTONew.CnfName = Convert.ToString(tblOrganizationTODT["CnfName"].ToString());
                    if (tblOrganizationTODT["phoneNo"] != DBNull.Value)
                        tblOrganizationTONew.PhoneNo = Convert.ToString(tblOrganizationTODT["phoneNo"].ToString());
                    if (tblOrganizationTODT["faxNo"] != DBNull.Value)
                        tblOrganizationTONew.FaxNo = Convert.ToString(tblOrganizationTODT["faxNo"].ToString());
                    if (tblOrganizationTODT["emailAddr"] != DBNull.Value)
                        tblOrganizationTONew.EmailAddr = Convert.ToString(tblOrganizationTODT["emailAddr"].ToString());
                    if (tblOrganizationTODT["website"] != DBNull.Value)
                        tblOrganizationTONew.Website = Convert.ToString(tblOrganizationTODT["website"].ToString());

                    if (tblOrganizationTODT["registeredMobileNos"] != DBNull.Value)
                        tblOrganizationTONew.RegMobileNo = Convert.ToString(tblOrganizationTODT["registeredMobileNos"].ToString());

                    if (tblOrganizationTODT["cdValue"] != DBNull.Value)
                        tblOrganizationTONew.CdStructure = Convert.ToDouble(tblOrganizationTODT["cdValue"].ToString());
                    if (tblOrganizationTODT["deliveryPeriod"] != DBNull.Value)
                        tblOrganizationTONew.DeliveryPeriod = Convert.ToInt32(tblOrganizationTODT["deliveryPeriod"].ToString());

                    if (tblOrganizationTODT["villageName"] != DBNull.Value)
                        tblOrganizationTONew.Village = Convert.ToString(tblOrganizationTODT["villageName"].ToString());
                    if (tblOrganizationTODT["isSpecialCnf"] != DBNull.Value)
                        tblOrganizationTONew.IsSpecialCnf = Convert.ToInt32(tblOrganizationTODT["isSpecialCnf"].ToString());
                    if (tblOrganizationTODT["talukaName"] != DBNull.Value)
                        tblOrganizationTONew.Taluka = Convert.ToString(tblOrganizationTODT["talukaName"].ToString());
                    if (tblOrganizationTODT["districtName"] != DBNull.Value)
                        tblOrganizationTONew.District = Convert.ToString(tblOrganizationTODT["districtName"].ToString());
                    if (tblOrganizationTODT["stateName"] != DBNull.Value)
                        tblOrganizationTONew.State = Convert.ToString(tblOrganizationTODT["stateName"].ToString());

                    if (tblOrganizationTODT["foFirstName"] != DBNull.Value)
                        tblOrganizationTONew.FoFirstName = Convert.ToString(tblOrganizationTODT["foFirstName"].ToString());
                    if (tblOrganizationTODT["foMidName"] != DBNull.Value)
                        tblOrganizationTONew.FoMiddleName = Convert.ToString(tblOrganizationTODT["foMidName"].ToString());
                    if (tblOrganizationTODT["foLastName"] != DBNull.Value)
                        tblOrganizationTONew.FoLastName = Convert.ToString(tblOrganizationTODT["foLastName"].ToString());
                    if (tblOrganizationTODT["foMobileNo"] != DBNull.Value)
                        tblOrganizationTONew.FoMobileNo = Convert.ToString(tblOrganizationTODT["foMobileNo"].ToString());
                    if (tblOrganizationTODT["foAlternateMobNo"] != DBNull.Value)
                        tblOrganizationTONew.FoAlternateMobileNo = Convert.ToString(tblOrganizationTODT["foAlternateMobNo"].ToString());
                    if (tblOrganizationTODT["foPhoneNo"] != DBNull.Value)
                        tblOrganizationTONew.FoPhoneNo = Convert.ToString(tblOrganizationTODT["foPhoneNo"].ToString());
                    if (tblOrganizationTODT["foDateOfBirth"] != DBNull.Value)
                        tblOrganizationTONew.FoDob = Convert.ToDateTime(tblOrganizationTODT["foDateOfBirth"].ToString()).ToString("dd-MM-yyyy");
                    if (tblOrganizationTODT["foPrimaryEmail"] != DBNull.Value)
                        tblOrganizationTONew.FoEmailAddr = Convert.ToString(tblOrganizationTODT["foPrimaryEmail"].ToString());
                    if (tblOrganizationTODT["foAlternateEmail"] != DBNull.Value)
                        tblOrganizationTONew.FoAlterEmailAddr = Convert.ToString(tblOrganizationTODT["foAlternateEmail"].ToString());

                    if (tblOrganizationTODT["soFirstName"] != DBNull.Value)
                        tblOrganizationTONew.SoFirstName = Convert.ToString(tblOrganizationTODT["soFirstName"].ToString());
                    if (tblOrganizationTODT["soMidName"] != DBNull.Value)
                        tblOrganizationTONew.SoMiddleName = Convert.ToString(tblOrganizationTODT["soMidName"].ToString());
                    if (tblOrganizationTODT["soLastName"] != DBNull.Value)
                        tblOrganizationTONew.SoLastName = Convert.ToString(tblOrganizationTODT["soLastName"].ToString());
                    if (tblOrganizationTODT["soMobileNo"] != DBNull.Value)
                        tblOrganizationTONew.SoMobileNo = Convert.ToString(tblOrganizationTODT["soMobileNo"].ToString());
                    if (tblOrganizationTODT["soAlternateMobNo"] != DBNull.Value)
                        tblOrganizationTONew.SoAlternateMobileNo = Convert.ToString(tblOrganizationTODT["soAlternateMobNo"].ToString());
                    if (tblOrganizationTODT["soPhoneNo"] != DBNull.Value)
                        tblOrganizationTONew.SoPhoneNo = Convert.ToString(tblOrganizationTODT["soPhoneNo"].ToString());
                    if (tblOrganizationTODT["soDateOfBirth"] != DBNull.Value)
                        tblOrganizationTONew.SoDob = Convert.ToDateTime(tblOrganizationTODT["soDateOfBirth"].ToString()).ToString("dd-MM-yyyy");
                    if (tblOrganizationTODT["soPrimaryEmail"] != DBNull.Value)
                        tblOrganizationTONew.SoEmailAddr = Convert.ToString(tblOrganizationTODT["soPrimaryEmail"].ToString());
                    if (tblOrganizationTODT["soAlternateEmail"] != DBNull.Value)
                        tblOrganizationTONew.SoAlterEmailAddr = Convert.ToString(tblOrganizationTODT["soAlternateEmail"].ToString());

                    if (tblOrganizationTODT["plotNo"] != DBNull.Value)
                        tblOrganizationTONew.PlotNo = Convert.ToString(tblOrganizationTODT["plotNo"].ToString());
                    if (tblOrganizationTODT["streetName"] != DBNull.Value)
                        tblOrganizationTONew.StreetName = Convert.ToString(tblOrganizationTODT["streetName"].ToString());
                    if (tblOrganizationTODT["areaName"] != DBNull.Value)
                        tblOrganizationTONew.AreaName = Convert.ToString(tblOrganizationTODT["areaName"].ToString());
                    if (tblOrganizationTODT["pincode"] != DBNull.Value)
                        tblOrganizationTONew.PinCode = Convert.ToString(tblOrganizationTODT["pincode"].ToString());
                    if (tblOrganizationTODT["phoneNo"] != DBNull.Value)
                        tblOrganizationTONew.PhoneNo = Convert.ToString(tblOrganizationTODT["phoneNo"].ToString());
                    if (tblOrganizationTODT["faxNo"] != DBNull.Value)
                        tblOrganizationTONew.FaxNo = Convert.ToString(tblOrganizationTODT["faxNo"].ToString());
                    if (tblOrganizationTODT["emailAddr"] != DBNull.Value)
                        tblOrganizationTONew.EmailAddr = Convert.ToString(tblOrganizationTODT["emailAddr"].ToString());
                    if (tblOrganizationTODT["website"] != DBNull.Value)
                        tblOrganizationTONew.Website = Convert.ToString(tblOrganizationTODT["website"].ToString());
                    if (tblOrganizationTODT["PanNO"] != DBNull.Value)
                        tblOrganizationTONew.PanNo = Convert.ToString(tblOrganizationTODT["PanNO"].ToString());
                    if (tblOrganizationTODT["PanNO"] != DBNull.Value)
                        tblOrganizationTONew.PanNo = Convert.ToString(tblOrganizationTODT["PanNO"].ToString());
                    if (tblOrganizationTODT["provGSTNo"] != DBNull.Value)
                        tblOrganizationTONew.ProvGstNo = Convert.ToString(tblOrganizationTODT["provGSTNo"].ToString());
                    if (tblOrganizationTODT["permGSTNo"] != DBNull.Value)
                        tblOrganizationTONew.PermGstNo = Convert.ToString(tblOrganizationTODT["permGSTNo"].ToString());
                    if (tblOrganizationTODT["purchaseManager"] != DBNull.Value)
                        tblOrganizationTONew.PurchaseManager = Convert.ToString(tblOrganizationTODT["purchaseManager"].ToString());
                    if (tblOrganizationTODT["statusName"] != DBNull.Value)
                        tblOrganizationTONew.OrgStatusName = Convert.ToString(tblOrganizationTODT["statusName"].ToString());


                    Constants.SetNullValuesToEmpty(tblOrganizationTONew);
                    orgExportRptTOList.Add(tblOrganizationTONew);
                }
            }
            return orgExportRptTOList;
        }

        /// <summary>
        /// [2017-11-17] Vijaymala:Added To get organization list of particular region;
        /// </summary>
        /// <param name="orgTypeId"></param>
        /// <param name="districtId"></param>
        /// <returns></returns>
        public List<TblOrganizationTO> SelectOrganizationListByRegion(Int32 orgTypeId, Int32 districtId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " where tblOrganization.orgTypeId = " + orgTypeId + " and addrDtl.districtId = " + districtId;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(rdr);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }

        }

        public TblOrganizationTO SelectTblOrganizationTOByEnqRefId(String enq_ref_id, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE enq_ref_id = " + enq_ref_id + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblOrganizationTO> list = ConvertDTToList(rdr);
                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectTblOrganizationTOByEnqRefId");
                return null;
            }
            finally
            {
                if (rdr != null)
                    rdr.Dispose();
                cmdSelect.Dispose();
            }
        }


        public List<TblOrganizationTO> SelectAllOrganizationListV2()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "SELECT* FROM[tblOrganization] WHERE isActive = 1";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return SimpleConvertDTtoList(rdr);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblOrganizationTO> SimpleConvertDTtoList(SqlDataReader tblOrganizationTODT)
        {
            List<TblOrganizationTO> tblOrganizationTOList = new List<TblOrganizationTO>();
            if (tblOrganizationTODT != null)
            {
                while (tblOrganizationTODT.Read())
                {
                    TblOrganizationTO tblOrganizationTONew = new TblOrganizationTO();
                    if (tblOrganizationTODT["idOrganization"] != DBNull.Value)
                        tblOrganizationTONew.IdOrganization = Convert.ToInt32(tblOrganizationTODT["idOrganization"].ToString());
                    if (tblOrganizationTODT["orgTypeId"] != DBNull.Value)
                        tblOrganizationTONew.OrgTypeId = Convert.ToInt32(tblOrganizationTODT["orgTypeId"].ToString());
                    if (tblOrganizationTODT["addrId"] != DBNull.Value)
                        tblOrganizationTONew.AddrId = Convert.ToInt32(tblOrganizationTODT["addrId"].ToString());
                    if (tblOrganizationTODT["firstOwnerPersonId"] != DBNull.Value)
                        tblOrganizationTONew.FirstOwnerPersonId = Convert.ToInt32(tblOrganizationTODT["firstOwnerPersonId"].ToString());
                    if (tblOrganizationTODT["secondOwnerPersonId"] != DBNull.Value)
                        tblOrganizationTONew.SecondOwnerPersonId = Convert.ToInt32(tblOrganizationTODT["secondOwnerPersonId"].ToString());
                    if (tblOrganizationTODT["parentId"] != DBNull.Value)
                        tblOrganizationTONew.ParentId = Convert.ToInt32(tblOrganizationTODT["parentId"].ToString());
                    if (tblOrganizationTODT["createdBy"] != DBNull.Value)
                        tblOrganizationTONew.CreatedBy = Convert.ToInt32(tblOrganizationTODT["createdBy"].ToString());
                    if (tblOrganizationTODT["createdOn"] != DBNull.Value)
                        tblOrganizationTONew.CreatedOn = Convert.ToDateTime(tblOrganizationTODT["createdOn"].ToString());
                    if (tblOrganizationTODT["firmName"] != DBNull.Value)
                        tblOrganizationTONew.FirmName = Convert.ToString(tblOrganizationTODT["firmName"].ToString());

                    if (tblOrganizationTODT["phoneNo"] != DBNull.Value)
                        tblOrganizationTONew.PhoneNo = Convert.ToString(tblOrganizationTODT["phoneNo"].ToString());
                    if (tblOrganizationTODT["faxNo"] != DBNull.Value)
                        tblOrganizationTONew.FaxNo = Convert.ToString(tblOrganizationTODT["faxNo"].ToString());
                    if (tblOrganizationTODT["emailAddr"] != DBNull.Value)
                        tblOrganizationTONew.EmailAddr = Convert.ToString(tblOrganizationTODT["emailAddr"].ToString());
                    if (tblOrganizationTODT["website"] != DBNull.Value)
                        tblOrganizationTONew.Website = Convert.ToString(tblOrganizationTODT["website"].ToString());

                    if (tblOrganizationTODT["registeredMobileNos"] != DBNull.Value)
                        tblOrganizationTONew.RegisteredMobileNos = Convert.ToString(tblOrganizationTODT["registeredMobileNos"].ToString());

                    if (tblOrganizationTODT["cdStructureId"] != DBNull.Value)
                        tblOrganizationTONew.CdStructureId = Convert.ToInt32(tblOrganizationTODT["cdStructureId"].ToString());
                    if (tblOrganizationTODT["delPeriodId"] != DBNull.Value)
                        tblOrganizationTONew.DelPeriodId = Convert.ToInt32(tblOrganizationTODT["delPeriodId"].ToString());

                    if (tblOrganizationTODT["isActive"] != DBNull.Value)
                        tblOrganizationTONew.IsActive = Convert.ToInt32(tblOrganizationTODT["isActive"].ToString());
                    if (tblOrganizationTODT["remark"] != DBNull.Value)
                        tblOrganizationTONew.Remark = Convert.ToString(tblOrganizationTODT["remark"].ToString());

                    if (tblOrganizationTODT["isSpecialCnf"] != DBNull.Value)
                        tblOrganizationTONew.IsSpecialCnf = Convert.ToInt32(tblOrganizationTODT["isSpecialCnf"].ToString());

                    if (tblOrganizationTODT["digitalSign"] != DBNull.Value)
                        tblOrganizationTONew.DigitalSign = Convert.ToString(tblOrganizationTODT["digitalSign"].ToString());
                    if (tblOrganizationTODT["deactivatedOn"] != DBNull.Value)
                        tblOrganizationTONew.DeactivatedOn = Convert.ToDateTime(tblOrganizationTODT["deactivatedOn"].ToString());


                    if (tblOrganizationTODT["orgLogo"] != DBNull.Value)
                        tblOrganizationTONew.OrgLogo = Convert.ToString(tblOrganizationTODT["orgLogo"].ToString());

                    if (tblOrganizationTODT["firmTypeId"] != DBNull.Value)
                        tblOrganizationTONew.FirmTypeId = Convert.ToInt32(tblOrganizationTODT["firmTypeId"].ToString());

                    if (tblOrganizationTODT["influencerTypeId"] != DBNull.Value)
                        tblOrganizationTONew.InfluencerTypeId = Convert.ToInt32(tblOrganizationTODT["influencerTypeId"].ToString());

                    //Sudhir[22-APR-2018]
                    if (tblOrganizationTODT["dateOfEstablishment"] != DBNull.Value)
                        tblOrganizationTONew.DateOfEstablishment = Convert.ToDateTime(tblOrganizationTODT["dateOfEstablishment"].ToString());

                    if (tblOrganizationTODT["orgStatusId"] != DBNull.Value)
                    {
                        tblOrganizationTONew.StatusId = Convert.ToInt32(tblOrganizationTODT["orgStatusId"].ToString());
                        tblOrganizationTONew.OrgStatusId = Convert.ToInt32(tblOrganizationTODT["orgStatusId"].ToString()); //[20220105] Dhananjay Added
                    }
                    if (tblOrganizationTODT["rebMobNoCntryCode"] != DBNull.Value)
                        tblOrganizationTONew.RebMobNoCntryCode = Convert.ToString(tblOrganizationTODT["rebMobNoCntryCode"].ToString());

                    if (tblOrganizationTODT["firmCode"] != DBNull.Value)
                        tblOrganizationTONew.FirmCode = Convert.ToString(tblOrganizationTODT["firmCode"].ToString());

                    if (tblOrganizationTODT["creditLimit"] != DBNull.Value)
                        tblOrganizationTONew.CreditLimit = Convert.ToDouble(tblOrganizationTODT["creditLimit"].ToString());

                    if (tblOrganizationTODT["txnId"] != DBNull.Value)
                        tblOrganizationTONew.TxnId = Convert.ToString(tblOrganizationTODT["txnId"].ToString());

                    tblOrganizationTOList.Add(tblOrganizationTONew);
                }
            }
            return tblOrganizationTOList;
        }

        public List<DropDownTO> SelectSalesEngineerListForDropDown(Int32 orgId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String sqlQuery = String.Empty;
            try
            {
                conn.Open();

                sqlQuery = " SELECT cnfOrg.idOrganization,(CASE WHEN addrDtl.villageName " +
                              " IS NULL then  cnfOrg.firmName  ELSE cnfOrg.firmName +',' + addrDtl.villageName END)" +
                              " as firmName ," +
                              " cnfOrg.isSpecialCnf FROM tblOrganization cnfOrg " +
                              " LEFT JOIN " +
                              " ( " +
                              " SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                              " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                              " ) addrDtl " +
                              " ON cnfOrg.idOrganization = addrDtl.organizationId " +
                              " Left join tblOrganization dealerOrg on cnfOrg.idOrganization = dealerOrg.parentId " +
                              " WHERE  cnfOrg.isActive=1 AND dealerOrg.idOrganization =" + orgId;


                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader tblOrgReader = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                if (tblOrgReader != null)
                {
                    while (tblOrgReader.Read())
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        if (tblOrgReader["idOrganization"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(tblOrgReader["idOrganization"].ToString());
                        if (tblOrgReader["firmName"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(tblOrgReader["firmName"].ToString());
                        if (tblOrgReader["isSpecialCnf"] != DBNull.Value)
                            dropDownTO.Tag = "isSpecialCnf : " + Convert.ToString(tblOrgReader["isSpecialCnf"].ToString());
                        dropDownTOList.Add(dropDownTO);
                    }
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                Constants.LoggerObj.LogError(1, ex, "Error in Method SelectSalesEngineerListForDropDown at DAO");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<DropDownTO> SelectSupplierListForDropDown(Constants.OrgTypeE orgTypeE)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String sqlQuery = String.Empty;
            SqlDataReader tblOrgRdr = null;
            try
            {
                conn.Open();

                sqlQuery = "SELECT idOrganization,firmName FROM tblOrganization WHERE isActive=1 AND orgTypeId = " + (int)orgTypeE + " ORDER BY tblOrganization.firmName";


                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                tblOrgRdr = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                if (tblOrgRdr != null)
                {
                    while (tblOrgRdr.Read())
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        if (tblOrgRdr["idOrganization"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(tblOrgRdr["idOrganization"].ToString());
                        if (tblOrgRdr["firmName"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(tblOrgRdr["firmName"].ToString());

                        dropDownTOList.Add(dropDownTO);
                    }
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                Constants.LoggerObj.LogError(1, ex, "Error in Method SelectSalesEngineerListForDropDown at DAO");
                return null;
            }
            finally
            {
                if (tblOrgRdr != null)
                    tblOrgRdr.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        //Gokul
        public OrgBasicInfo GetOrganizationBasicInfo(Int32 organizationId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT party.firmName, " +
                                        " CASE WHEN LEN(RTRIM(party.registeredMobileNos)) > 3 THEN party.registeredMobileNos " +
                                        " WHEN LEN(RTRIM(firstOwner.mobileNo)) > 3 THEN firstOwner.mobileNo " +
                                        " WHEN LEN(RTRIM(firstOwner.alternateMobNo)) > 3 THEN firstOwner.alternateMobNo " +
                                        " WHEN LEN(RTRIM(secondOwner.mobileNo)) > 3 THEN firstOwner.mobileNo " +
                                        " WHEN LEN(RTRIM(secondOwner.alternateMobNo)) > 3 THEN firstOwner.alternateMobNo " +
                                        " ELSE '91' END as mobileNo " +
                                        " FROM tblOrganization party " +
                                        " LEFT JOIN tblPerson firstOwner " +
                                        " ON firstOwner.idPerson = party.firstOwnerPersonId " +
                                        " LEFT JOIN tblPerson secondOwner " +
                                        " ON secondOwner.idPerson = party.secondOwnerPersonId " +
                                        " WHERE idOrganization = " + organizationId;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                OrgBasicInfo orgBasicInfo = null;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                while (rdr.Read())
                {
                    orgBasicInfo = new OrgBasicInfo();
                    if (rdr["firmName"] != DBNull.Value)
                        orgBasicInfo.FirmName = Convert.ToString(rdr["firmName"].ToString());
                    if (rdr["mobileNo"] != DBNull.Value)
                        orgBasicInfo.MobileNo = Convert.ToString(rdr["mobileNo"].ToString());
                }

                return orgBasicInfo;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (rdr != null) rdr.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        #endregion

        #region Insertion
        public int InsertTblOrganization(TblOrganizationTO tblOrganizationTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblOrganizationTO, cmdInsert);
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

        public int InsertTblOrganization(TblOrganizationTO tblOrganizationTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblOrganizationTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblOrganizationTO tblOrganizationTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblOrganization]( " +
                            "  [orgTypeId]" +
                            " ,[addrId]" +
                            " ,[firstOwnerPersonId]" +
                            " ,[secondOwnerPersonId]" +
                            " ,[parentId]" +
                            " ,[createdBy]" +
                            " ,[createdOn]" +
                            " ,[firmName]" +
                            " ,[phoneNo]" +
                            " ,[faxNo]" +
                            " ,[emailAddr]" +
                            " ,[website]" +
                            " ,[registeredMobileNos]" +
                            " ,[cdStructureId]" +
                            " ,[isActive]" +
                            " ,[remark]" +
                            " ,[delPeriodId]" +
                            " ,[isSpecialCnf]" +
                            " ,[digitalSign]" +
                            " ,[firmTypeId]" +
                            " ,[influencerTypeId]" +
                            " ,[isOverdueExist]" +              //Priyanka [07-06-2018] : Added for SHIVANGI.
                            " ,[dateOfEstablishment]" +
                            " ,[suppDivGroupId]" +
                            " ,[isRegUnderGST]" +
                            " ,[orgStatusId]" +
                            " ,[creditLimit]" +
                            " ,[rebMobNoCntryCode]" +
                            " ,[firmCode]" +
                            " ,[consumerTypeId]" +       //Sachin Khune [29-05-2020]
                            " ,[orgGrpTypeId]" +    //Harshala[18/09/2020]
                            " ,[currencyId]" +    //Harshala[18/09/2020]
                            " ,[isTcsApplicable]" +
                            " ,[isDeclarationRec]" +
                            " ,[tdsPct]" +
                            " ,[scrapCapacity]" +
                            " ,[supplierGrade]" +
                            " )" +
                " VALUES (" +
                            "  @OrgTypeId " +
                            " ,@AddrId " +
                            " ,@FirstOwnerPersonId " +
                            " ,@SecondOwnerPersonId " +
                            " ,@ParentId " +
                            " ,@CreatedBy " +
                            " ,@CreatedOn " +
                            " ,@FirmName " +
                            " ,@phoneNo " +
                            " ,@faxNo " +
                            " ,@emailAddr " +
                            " ,@website " +
                            " ,@registeredMobileNos " +
                            " ,@cdStructureId " +
                            " ,@isActive " +
                            " ,@remark " +
                            " ,@delPeriodId " +
                            " ,@isSpecialCnf " +
                            " ,@digitalSign " +
                            " ,@FirmTypeId" +
                            " ,@influencerTypeId" +
                            " ,@isOverdueExist" +                           //Priyanka [07-06-2018] : Added for SHIVANGI
                            " ,@DateOfEstablishment" +
                            " ,@SuppDivGroupId" +
                            " ,@IsRegUnderGST" +
                            " ,@orgStatusId" +
                            " ,@creditLimit" +
                            " ,@RebMobNoCntryCode" +
                            " ,@FirmCode" +
                            " ,@consumerTypeId" +               //Sachin Khune [29-05-2020]
                             " ,@orgGrpTypeId" +               //Sachin Khune [18-09-2020]
                              " ,@currencyId" +               //Sachin Khune [18-09-2020]
                              " ,@isTcsApplicable" +
                              " ,@isDeclarationRec" +
                              " ,@tdsPct" +
                              " ,@ScrapCapacity" +
                              " ,@supplierGrade" +
                            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            String sqlSelectIdentityQry = "Select @@Identity";

            //cmdInsert.Parameters.Add("@IdOrganization", System.Data.SqlDbType.Int).Value = tblOrganizationTO.IdOrganization;
            cmdInsert.Parameters.Add("@OrgTypeId", System.Data.SqlDbType.Int).Value = tblOrganizationTO.OrgTypeId;
            cmdInsert.Parameters.Add("@AddrId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.AddrId);
            cmdInsert.Parameters.Add("@FirstOwnerPersonId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.FirstOwnerPersonId);
            cmdInsert.Parameters.Add("@SecondOwnerPersonId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.SecondOwnerPersonId);
            cmdInsert.Parameters.Add("@ParentId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.ParentId);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblOrganizationTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblOrganizationTO.CreatedOn;
            cmdInsert.Parameters.Add("@FirmName", System.Data.SqlDbType.NVarChar).Value = tblOrganizationTO.FirmName;
            cmdInsert.Parameters.Add("@phoneNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.PhoneNo);
            cmdInsert.Parameters.Add("@faxNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.FaxNo);
            cmdInsert.Parameters.Add("@emailAddr", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.EmailAddr);
            cmdInsert.Parameters.Add("@website", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.Website);
            cmdInsert.Parameters.Add("@registeredMobileNos", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.RegisteredMobileNos);
            cmdInsert.Parameters.Add("@cdStructureId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.CdStructureId);
            cmdInsert.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value = tblOrganizationTO.IsActive;
            cmdInsert.Parameters.Add("@remark", System.Data.SqlDbType.NVarChar, 256).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.Remark);
            cmdInsert.Parameters.Add("@delPeriodId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.DelPeriodId);
            cmdInsert.Parameters.Add("@isSpecialCnf", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.IsSpecialCnf);
            cmdInsert.Parameters.Add("@digitalSign", System.Data.SqlDbType.VarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.DigitalSign);
            cmdInsert.Parameters.Add("@FirmTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.FirmTypeId);
            cmdInsert.Parameters.Add("@InfluencerTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.InfluencerTypeId);
            cmdInsert.Parameters.Add("@isOverdueExist", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.IsOverdueExist);         //Priyanka [07-06-2018] : Added for SHIVANGI.
            cmdInsert.Parameters.Add("@DateOfEstablishment", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.DateOfEstablishment); //Sudhir[21-06-2018] 
            cmdInsert.Parameters.Add("@SuppDivGroupId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.SuppDivGroupId);         //Priyanka [13-09-2019]
            cmdInsert.Parameters.Add("@IsRegUnderGST", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.IsRegUnderGST);         //Priyanka [13-09-2019]
            if (tblOrganizationTO.OrgTypeId == (Int32)Constants.OrgTypeE.DEALER) //[20220105] Dhananjay Added
            {
                cmdInsert.Parameters.Add("@OrgStatusId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.StatusId); //[20220105] Dhananjay Added
            }
            else
            {
                cmdInsert.Parameters.Add("@orgStatusId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.OrgStatusId);         //Priyanka [13-09-2019]
            }
            cmdInsert.Parameters.Add("@creditLimit", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.CreditLimit);         //Priyanka [13-09-2019]
            cmdInsert.Parameters.Add("@RebMobNoCntryCode", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.RebMobNoCntryCode);         //Priyanka [13-09-2019]
            cmdInsert.Parameters.Add("@FirmCode", System.Data.SqlDbType.VarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.FirmCode);
            cmdInsert.Parameters.Add("@consumerTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.ConsumerTypeId);         //Sachin Khune [29-05-2020]
            cmdInsert.Parameters.Add("@orgGrpTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.OrgGroupTypeId);         //Harshala [18-09-2020]
            cmdInsert.Parameters.Add("@currencyId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.CurrencyId);         //Harshala [18-09-2020]
            cmdInsert.Parameters.Add("@isTcsApplicable", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.IsTcsApplicable);
            cmdInsert.Parameters.Add("@isDeclarationRec", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.IsDeclarationRec);
            cmdInsert.Parameters.Add("@tdsPct", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.TdsPct);
            cmdInsert.Parameters.Add("@ScrapCapacity", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.ScrapCapacity);
            cmdInsert.Parameters.Add("@supplierGrade", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.supplierGrade);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = sqlSelectIdentityQry;
                tblOrganizationTO.IdOrganization = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion

        #region Updation
        public int UpdateTblOrganization(TblOrganizationTO tblOrganizationTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblOrganizationTO, cmdUpdate);
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

        public int UpdateTblOrganizationRefIds(TblOrganizationTO tblOrganizationTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;

                String sqlQuery = @" UPDATE [tblOrganization] SET " +
                                "  [overdue_ref_id]= @OverdueRefId" +
                                " ,[enq_ref_id]= @EnqRefId" +
                                " ,[updatedBy] = @updatedBy" +
                                " ,[updatedOn] = @updatedOn" +
                                " WHERE [idOrganization] = @IdOrganization";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@IdOrganization", System.Data.SqlDbType.Int).Value = tblOrganizationTO.IdOrganization;
                cmdUpdate.Parameters.Add("@OverdueRefId", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.OverdueRefId);
                cmdUpdate.Parameters.Add("@EnqRefId", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.EnqRefId);
                cmdUpdate.Parameters.Add("@updatedBy", System.Data.SqlDbType.Int).Value = tblOrganizationTO.UpdatedBy;
                cmdUpdate.Parameters.Add("@updatedOn", System.Data.SqlDbType.DateTime).Value = tblOrganizationTO.UpdatedOn;

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

        public int UpdateTxnIdtoTblOrg(TblOrganizationTO tblOrgnizationTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteTblOrgTxnIdCommand(tblOrgnizationTO, cmdUpdate);
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

        public int UpdateTblOrganization(TblOrganizationTO tblOrganizationTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblOrganizationTO, cmdUpdate);
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

        //Priyanka [07-06-2018] : Added for SHIVANGI.
        public int PostUpdateOverdueExistOrNot(Int32 dealerOrgId, Int32 isOverdueExist)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;

                String sqlQuery = @" UPDATE [tblOrganization] SET " +
                                "  [isOverdueExist]=" + isOverdueExist +
                                // "  [updatedBy]= "+ loginUserId +
                                " WHERE [idOrganization] = " + dealerOrgId;

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@IdOrganization", System.Data.SqlDbType.Int).Value = dealerOrgId;
                cmdUpdate.Parameters.Add("@isOverdueExist", System.Data.SqlDbType.Int).Value = isOverdueExist;
                // cmdUpdate.Parameters.Add("@updatedBy", System.Data.SqlDbType.Int).Value = loginUserId;
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


        public int ExecuteTblOrgTxnIdCommand(TblOrganizationTO tblOrganizationTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblOrganization] SET " +
                            "  [txnId]= @TxnId" +
                          " WHERE [idOrganization] = @IdOrganization";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;
            cmdUpdate.Parameters.Add("@IdOrganization", System.Data.SqlDbType.Int).Value = tblOrganizationTO.IdOrganization;
            cmdUpdate.Parameters.Add("@TxnId", System.Data.SqlDbType.NVarChar).Value = tblOrganizationTO.TxnId;
            return cmdUpdate.ExecuteNonQuery();

        }

        public int ExecuteUpdationCommand(TblOrganizationTO tblOrganizationTO, SqlCommand cmdUpdate)
        {

            String sqlQuery = @" UPDATE [tblOrganization] SET " +
                            "  [orgTypeId]= @OrgTypeId" +
                            " ,[addrId]= @AddrId" +
                            " ,[firstOwnerPersonId]= @FirstOwnerPersonId" +
                            " ,[secondOwnerPersonId]= @SecondOwnerPersonId" +
                            " ,[parentId]= @ParentId" +
                            " ,[firmName] = @FirmName" +
                            " ,[updatedBy] = @updatedBy" +
                            " ,[updatedOn] = @updatedOn" +
                            " ,[phoneNo] = @phoneNo" +
                            " ,[faxNo] = @faxNo" +
                            " ,[emailAddr] = @emailAddr" +
                            " ,[website] = @website" +
                            " ,[registeredMobileNos] = @registeredMobileNos" +
                            " ,[cdStructureId] = @cdStructureId" +
                            " ,[isActive] = @isActive" +
                            " ,[remark] = @remark" +
                            " ,[delPeriodId] = @delPeriodId" +
                            " ,[isSpecialCnf] = @isSpecialCnf" +
                            " ,[digitalSign] = @digitalSign" +
                            " ,[deactivatedOn] = @deactivatedOn" +
                            " ,[firmTypeId]=@FirmTypeId " +
                            " ,[influencerTypeId]=@InfluencerTypeId" +
                            " ,[isOverdueExist]=@IsOverdueExist" +                  //Priyanka [07-06-2018] : Added for SHIVANGI.
                            " ,[dateOfEstablishment]=@DateOfEstablishment" +
                            " ,[orgStatusId] =@OrgStatusId" +
                            " ,[suppDivGroupId] = @SuppDivGroupId" +                //Priyanka [13-09-2019]
                            " ,[isRegUnderGST] = @IsRegUnderGST" +
                             " ,[rebMobNoCntryCode] = @RebMobNoCntryCode" +
                             " ,[firmCode] = @FirmCode" +
                             " ,[creditLimit] = @CreditLimit" +
                             ",[consumerTypeId] = @ConsumerTypeId" +
                             ",[orgGrpTypeId] = @orgGrpTypeId" +
                             ",[currencyId] = @currencyId" +
                             ",[isTcsApplicable] = @isTcsApplicable" +
                             ",[isDeclarationRec] = @isDeclarationRec" +
                             ",[scrapCapacity] = @ScrapCapacity" +
                             ",[supplierGrade] = @supplierGrade" +
                             " ,tdsPct=@tdsPct " +
                              " ,isMigration=0 " +
                            " WHERE [idOrganization] = @IdOrganization";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdOrganization", System.Data.SqlDbType.Int).Value = tblOrganizationTO.IdOrganization;
            cmdUpdate.Parameters.Add("@OrgTypeId", System.Data.SqlDbType.Int).Value = tblOrganizationTO.OrgTypeId;
            cmdUpdate.Parameters.Add("@AddrId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.AddrId);
            cmdUpdate.Parameters.Add("@FirstOwnerPersonId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.FirstOwnerPersonId);
            cmdUpdate.Parameters.Add("@SecondOwnerPersonId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.SecondOwnerPersonId);
            cmdUpdate.Parameters.Add("@ParentId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.ParentId);
            cmdUpdate.Parameters.Add("@FirmName", System.Data.SqlDbType.NVarChar).Value = tblOrganizationTO.FirmName;
            cmdUpdate.Parameters.Add("@updatedBy", System.Data.SqlDbType.Int).Value = tblOrganizationTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@updatedOn", System.Data.SqlDbType.DateTime).Value = tblOrganizationTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@phoneNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.PhoneNo);
            cmdUpdate.Parameters.Add("@faxNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.FaxNo);
            cmdUpdate.Parameters.Add("@emailAddr", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.EmailAddr);
            cmdUpdate.Parameters.Add("@website", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.Website);
            cmdUpdate.Parameters.Add("@registeredMobileNos", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.RegisteredMobileNos);
            cmdUpdate.Parameters.Add("@cdStructureId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.CdStructureId);
            cmdUpdate.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value = tblOrganizationTO.IsActive;
            cmdUpdate.Parameters.Add("@remark", System.Data.SqlDbType.NVarChar, 256).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.Remark);
            cmdUpdate.Parameters.Add("@delPeriodId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.DelPeriodId);
            cmdUpdate.Parameters.Add("@isSpecialCnf", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.IsSpecialCnf);
            cmdUpdate.Parameters.Add("@digitalSign", System.Data.SqlDbType.VarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.DigitalSign);
            cmdUpdate.Parameters.Add("@deactivatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.DeactivatedOn);
            cmdUpdate.Parameters.Add("@FirmTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.FirmTypeId);
            cmdUpdate.Parameters.Add("@InfluencerTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.InfluencerTypeId);
            cmdUpdate.Parameters.Add("@isOverdueExist", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.IsOverdueExist);             //Priyanka [07-06-2018] : Added for SHIVANGI.
            cmdUpdate.Parameters.Add("@DateOfEstablishment", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.DateOfEstablishment); //Sudhir[21-June-2018]
            if (tblOrganizationTO.OrgTypeId == (Int32)Constants.OrgTypeE.DEALER) //[20220105] Dhananjay Added
            {
                cmdUpdate.Parameters.Add("@OrgStatusId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.StatusId); //[20220105] Dhananjay Added
            }
            else
            {
                cmdUpdate.Parameters.Add("@OrgStatusId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.OrgStatusId);
            }
            cmdUpdate.Parameters.Add("@SuppDivGroupId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.SuppDivGroupId);             //Priyanka [13-09-2019]
            cmdUpdate.Parameters.Add("@IsRegUnderGST", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.IsRegUnderGST);             //Priyanka [13-09-2019]
            cmdUpdate.Parameters.Add("@RebMobNoCntryCode", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.RebMobNoCntryCode);
            cmdUpdate.Parameters.Add("@FirmCode", System.Data.SqlDbType.VarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.FirmCode);
            cmdUpdate.Parameters.Add("@CreditLimit", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.CreditLimit);
            cmdUpdate.Parameters.Add("@ConsumerTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.ConsumerTypeId);
            cmdUpdate.Parameters.Add("@orgGrpTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.OrgGroupTypeId);
            cmdUpdate.Parameters.Add("@currencyId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.CurrencyId);
            cmdUpdate.Parameters.Add("@isTcsApplicable", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.IsTcsApplicable);
            cmdUpdate.Parameters.Add("@isDeclarationRec", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.IsDeclarationRec);
            cmdUpdate.Parameters.Add("@tdsPct", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.TdsPct);
            cmdUpdate.Parameters.Add("@ScrapCapacity", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.ScrapCapacity);
            cmdUpdate.Parameters.Add("@supplierGrade", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrganizationTO.supplierGrade);
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public int DeleteTblOrganization(Int32 idOrganization)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idOrganization, cmdDelete);
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

        public int DeleteTblOrganization(Int32 idOrganization, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                cmdDelete.CommandTimeout = 60;
                return ExecuteDeletionCommand(idOrganization, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idOrganization, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblOrganization] " +
            " WHERE idOrganization = " + idOrganization + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idOrganization", System.Data.SqlDbType.Int).Value = tblOrganizationTO.IdOrganization;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion


        #region Migration Query

        public int UpdateTblOrgId(String updateQuery, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                cmdUpdate.CommandTimeout = 240;
                String sqlQuery = @updateQuery;

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



        #endregion

        #region //Chetan[2020-12-14] added for deactive duplication Supplier

        public DataTable GetDuplicateSupplierDT(bool isForSupplier)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                if (isForSupplier)
                    cmdSelect.CommandText = "select firmname ,count(firmname),orgTypeId from tblOrganization where isActive=1 and orgTypeId=6 group by firmname,orgTypeId  having count(firmname)>1  order by firmname";
                else
                    cmdSelect.CommandText = "select firmname ,count(firmname),orgTypeId from tblOrganization where isActive=1 and orgTypeId<>6 group by firmname,orgTypeId  having count(firmname)>1  order by firmname";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPayReq", System.Data.SqlDbType.Int).Value = tblPaymentRequestTO.IdPayReq;
                SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {

                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblOrganizationTO> SelectAllTblOrganization(string orgName)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " where tblOrganization.firmname = '" + orgName + "'";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(rdr);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public DataTable GetTblPurchaseManagerSupplier(int orgId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select * from [dbo].[tblPurchaseManagerSupplier] where organizationId= " + orgId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPayReq", System.Data.SqlDbType.Int).Value = tblPaymentRequestTO.IdPayReq;
                SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {

                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public int DeactiveTblOrganization(string orgIdStr)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;

                String sqlQuery = @" UPDATE [tblOrganization] SET " +
                                "  [isActive]= @isActive" +
                                " WHERE [idOrganization] in(" + orgIdStr + ")";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@isActive", System.Data.SqlDbType.Bit).Value = false;
                //cmdUpdate.Parameters.Add("@IdOrganization", System.Data.SqlDbType.Int).Value = tblOrganizationTO.IdOrganization;
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

        public int InsertTblDeactiveDuplicationSupplierDtl(string tranFailOrgIdStr, string deactiveOrgIdStr, string activeInScrapOrgIdStr, string activeInSapOrgIdStr)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;

                String sqlQuery = @"insert into tblDeactiveDuplicationSupplierDtl (tranFailOrgIdStr,deactiveOrgIdStr,activeInScrapOrgIdStr,activeInSapOrgIdStr)" +
                                    " values('" + tranFailOrgIdStr + "','" + deactiveOrgIdStr + "','" + activeInScrapOrgIdStr + "','" + activeInSapOrgIdStr + "')";
                cmdInsert.CommandText = sqlQuery;
                cmdInsert.CommandType = System.Data.CommandType.Text;
                return cmdInsert.ExecuteNonQuery();
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

        public DataTable GetSupplierPresentInSAP(int orgId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.SAP_CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select * from OCRD where CardCode=" + orgId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idPayReq", System.Data.SqlDbType.Int).Value = tblPaymentRequestTO.IdPayReq;
                SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {

                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        public List<DropDownTO> SelectAllOrganizationList(int orgTypeId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String sqlQuery = String.Empty;
            try
            {
                conn.Open();
                sqlQuery = "select * from tblOrganization where isActive=1";

                if (orgTypeId > 0)
                {
                    sqlQuery += "and orgTypeId = " + orgTypeId;
                }

                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader tblOrgReader = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                if (tblOrgReader != null)
                {
                    while (tblOrgReader.Read())
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        if (tblOrgReader["idOrganization"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(tblOrgReader["idOrganization"].ToString());
                        if (tblOrgReader["firmName"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(tblOrgReader["firmName"].ToString());
                        if (tblOrgReader["addrId"] != DBNull.Value)
                            dropDownTO.MappedTxnId = Convert.ToString(tblOrgReader["addrId"].ToString());
                        dropDownTOList.Add(dropDownTO);
                    }
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                Constants.LoggerObj.LogError(1, ex, "Error in Method SelectAllOrganizationListForDropDown at DAO", orgTypeId);
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        #endregion


        #region check if Org details Available in SAP
        public DropDownTO CheckIfOrgIsAvailableInSAP(int orgId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.SAP_CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select CardCode,CardName from OCRD where CardCode = " + orgId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader notifyReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                DropDownTO orgTO = new DropDownTO();
                while ((notifyReader).Read())
                {
                    if ((notifyReader)["CardName"] != DBNull.Value)
                        orgTO.Text = Convert.ToString((notifyReader)["CardName"].ToString());
                    if ((notifyReader)["CardCode"] != DBNull.Value)
                        orgTO.Value = Convert.ToInt32((notifyReader)["CardCode"].ToString());
                }
                if (notifyReader != null) notifyReader.Dispose();
                return orgTO;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<DropDownTO> CheckIfOrgAddressIsAvailableInSAP(int orgId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.SAP_CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select CardCode,GSTType,GSTRegnNo from CRD1 where CardCode = " + orgId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader notifyReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> orgTOList = new List<DropDownTO>();
                while ((notifyReader).Read())
                {
                    DropDownTO orgTO = new DropDownTO();

                    if ((notifyReader)["GSTRegnNo"] != DBNull.Value)
                        orgTO.Text = Convert.ToString((notifyReader)["GSTRegnNo"].ToString());
                    if ((notifyReader)["GSTType"] != DBNull.Value)
                        orgTO.Value = Convert.ToInt32((notifyReader)["GSTType"].ToString());
                    if ((notifyReader)["CardCode"] != DBNull.Value)
                        orgTO.Tag = Convert.ToInt32((notifyReader)["CardCode"].ToString());
                    orgTOList.Add(orgTO);
                }
                if (notifyReader != null) notifyReader.Dispose();
                return orgTOList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public List<DropDownTO> CheckIfOrgCurrencyIsAvailableInSAP(int orgId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.SAP_CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select Currency from OCRD where CardCode = " + orgId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader notifyReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> orgCurrencyTOList = new List<DropDownTO>();
                while ((notifyReader).Read())
                {
                    DropDownTO orgCurrencyTO = new DropDownTO();

                    if ((notifyReader)["Currency"] != DBNull.Value)
                        orgCurrencyTO.Text = Convert.ToString((notifyReader)["Currency"].ToString());

                    orgCurrencyTOList.Add(orgCurrencyTO);
                }
                if (notifyReader != null) notifyReader.Dispose();
                return orgCurrencyTOList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        #endregion
    }
}
