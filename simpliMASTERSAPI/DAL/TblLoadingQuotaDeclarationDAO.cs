using ODLMWebAPI.Models;
using simpliMASTERSAPI.DAL.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System;
using ODLMWebAPI.StaticStuff;

namespace simpliMASTERSAPI.DAL
{
    public class TblLoadingQuotaDeclarationDAO : ITblLoadingQuotaDeclarationDAO
    {
        public  List<TblLoadingQuotaDeclarationTO> SelectLatestCalculatedLoadingQuotaDeclarationList(DateTime stockDate, Int32 cnfId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            String whereCond = string.Empty;
            try
            {
                if (cnfId > 0)
                {
                    whereCond = " AND cnfList.idOrganization=" + cnfId;
                }
                //cmdSelect.CommandText = "  SELECT 0 AS idLoadingQuota,0 AS updatedBy , null as updatedOn , cnfList.firmName cnfOrgName ,stockConfig.* " +
                //                        "  FROM tblOrganization cnfList " +
                //                        "  INNER JOIN " +
                //                        "  ( " +
                //                        "    SELECT allConfig.idLoadQuotaConfig AS loadQuotaConfigId ,allConfig.*, material.materialSubType AS materialDesc, stockDtl.totalStock, " +
                //                        "    CASE WHEN ISNULL(stockDtl.totalStock, 0) > 0 THEN " +
                //                        "    CAST( stockDtl.totalStock * ((ISNULL(allocPct, 0) * stockDtl.totalStock) / stockDtl.totalStock) / 100 AS INT)   " +   //To Round Down the values. Suggested By Nitin K [Meeting Ref 31-03-2017]
                //                        "    ELSE 0 END AS allocQuota, " +
                //                        "    CASE WHEN ISNULL(stockDtl.totalStock, 0) > 0 THEN " +
                //                        "    CAST( stockDtl.totalStock * ((ISNULL(allocPct, 0) * stockDtl.totalStock) / stockDtl.totalStock) / 100 AS INT)   " +
                //                        "    ELSE 0 END AS balanceQuota " +
                //                        "    ,prodCat.prodCateDesc AS prodCatDesc" +
                //                        "    ,prodSpec.prodSpecDesc AS prodSpecDesc" +
                //                        "    ,0 AS transferedQuota" +
                //                        "    ,0 AS receivedQuota" +
                //                        "    FROM tblLoadingQuotaConfig allConfig " +
                //                        "    INNER JOIN " +
                //                        "    ( " +
                //                        "        SELECT cnfOrgId, prodCatId ,prodSpecId,materialId, MAX(createdOn)createdOn " +
                //                        "        FROM tblLoadingQuotaConfig " +
                //                        "        GROUP BY cnfOrgId, prodCatId,prodSpecId,materialId " +
                //                        "    ) latestConfig " +
                //                        "    ON allConfig.cnfOrgId = latestConfig.cnfOrgId " +
                //                        "    AND allConfig.materialId = latestConfig.materialId " +
                //                        "    AND allConfig.createdOn = latestConfig.createdOn " +
                //                        "    AND allConfig.prodCatId = latestConfig.prodCatId " +
                //                        "    AND allConfig.prodSpecId = latestConfig.prodSpecId " +
                //                        "    LEFT JOIN " +
                //                        "    ( " +
                //                        "        SELECT stockSummaryId, prodCatId,prodSpecId,materialId, SUM(dtl.noOfBundles)noOfBundles, SUM(dtl.totalStock) - SUM(dtl.removedStock) AS totalStock " +
                //                        "        FROM tblStockDetails dtl " +
                //                        "        INNER JOIN tblStockSummary summary " +
                //                        "        ON dtl.stockSummaryId = summary.idStockSummary " +
                //                        "        WHERE DAY(stockDate)=" + stockDate.Day +
                //                        "        AND MONTH(stockDate)=" + stockDate.Month + " AND YEAR(stockDate) = " + stockDate.Year +
                //                        "        GROUP BY stockSummaryId,prodCatId,prodSpecId, materialId " +
                //                        "    ) stockDtl " +
                //                        "    ON stockDtl.materialId = allConfig.materialId " +
                //                        "    AND allConfig.prodSpecId = stockDtl.prodSpecId " +
                //                        "    AND allConfig.prodCatId = stockDtl.prodCatId " +
                //                        "    LEFT JOIN tblMaterial material " +
                //                        "    ON material.idMaterial = allConfig.materialId " +
                //                        "    LEFT JOIN dimProdCat prodCat " +
                //                        "    ON prodCat.idProdCat = allConfig.prodCatId " +
                //                        "    LEFT JOIN dimProdSpec prodSpec " +
                //                        "    ON prodSpec.idProdSpec = allConfig.prodSpecId " +
                //                        " ) stockConfig " +
                //                        " ON cnfList.idOrganization = stockConfig.cnfOrgId " +
                //                        " WHERE orgTypeId = " + (int)Constants.OrgTypeE.C_AND_F_AGENT + whereCond +
                //                        " ORDER BY materialId ASC";

                cmdSelect.CommandText = "  SELECT 0 AS idLoadingQuota,0 AS updatedBy , null as updatedOn , cnfList.firmName cnfOrgName ,stockConfig.* " +
                                        "  FROM tblOrganization cnfList " +
                                        "  INNER JOIN " +
                                        "  ( " +
                                        "    SELECT allConfig.idLoadQuotaConfig AS loadQuotaConfigId ,allConfig.*, material.materialSubType AS materialDesc, stockDtl.totalStock, " +
                                        "    CASE WHEN ISNULL(stockDtl.totalStock, 0) > 0 THEN " +
                                        "    CAST( stockDtl.totalStock * ((ISNULL(allocPct, 0) * stockDtl.totalStock) / stockDtl.totalStock) / 100 AS INT)   " +   //To Round Down the values. Suggested By Nitin K [Meeting Ref 31-03-2017]
                                        "    ELSE 0 END AS allocQuota, " +
                                        "    CASE WHEN ISNULL(stockDtl.totalStock, 0) > 0 THEN " +
                                        "    CAST( stockDtl.totalStock * ((ISNULL(allocPct, 0) * stockDtl.totalStock) / stockDtl.totalStock) / 100 AS INT)   " +
                                        "    ELSE 0 END AS balanceQuota " +
                                        "    ,prodCat.prodCateDesc AS prodCatDesc" +
                                        "    ,prodSpec.prodSpecDesc AS prodSpecDesc" +
                                        "    ,(prodClass.displayName+'/'+productItem.itemName) as displayName " +
                                        "    ,0 AS transferedQuota" +
                                        "    ,0 AS receivedQuota" +
                                        "    FROM tblLoadingQuotaConfig allConfig " +
                                        "    INNER JOIN " +
                                        "    ( " +
                                        "        SELECT cnfOrgId, prodCatId ,prodSpecId,materialId,prodItemId, MAX(createdOn)createdOn " +
                                        "        FROM tblLoadingQuotaConfig " +
                                        "        GROUP BY cnfOrgId, prodCatId,prodSpecId,materialId,prodItemId " +
                                        "    ) latestConfig " +
                                        "    ON ISNULL(allConfig.cnfOrgId,0) = ISNULL(latestConfig.cnfOrgId,0) " +
                                        "    AND ISNULL(allConfig.materialId,0) = ISNULL(latestConfig.materialId,0) " +
                                        "    AND allConfig.createdOn = latestConfig.createdOn " +
                                        "    AND ISNULL(allConfig.prodCatId,0) = ISNULL(latestConfig.prodCatId,0) " +
                                        "    AND ISNULL(allConfig.prodSpecId,0) = ISNULL(latestConfig.prodSpecId,0) " +
                                        "    AND ISNULL(allConfig.prodItemId,0) = ISNULL(latestConfig.prodItemId,0) " +
                                        "    LEFT JOIN " +
                                        "    ( " +
                                        "        SELECT stockSummaryId, prodCatId,prodSpecId,materialId, SUM(dtl.noOfBundles)noOfBundles, SUM(dtl.totalStock) - SUM(dtl.removedStock) AS totalStock " +
                                        "        ,prodItemId FROM tblStockDetails dtl " +
                                        "        INNER JOIN tblStockSummary summary " +
                                        "        ON dtl.stockSummaryId = summary.idStockSummary " +
                                        "        WHERE DAY(stockDate)=" + stockDate.Day +
                                        "        AND MONTH(stockDate)=" + stockDate.Month + " AND YEAR(stockDate) = " + stockDate.Year +
                                        "        GROUP BY stockSummaryId,prodCatId,prodSpecId, materialId ,prodItemId " +
                                        "    ) stockDtl " +
                                        "    ON ISNULL(stockDtl.materialId,0) = ISNULL(allConfig.materialId,0) " +
                                        "    AND ISNULL(allConfig.prodSpecId,0) = ISNULL(stockDtl.prodSpecId,0) " +
                                        "    AND ISNULL(allConfig.prodCatId,0) = ISNULL(stockDtl.prodCatId,0) " +
                                        "    AND ISNULL(allConfig.prodItemId,0) = ISNULL(stockDtl.prodItemId,0) " +
                                        "    LEFT JOIN tblMaterial material " +
                                        "    ON ISNULL(material.idMaterial,0) = ISNULL(allConfig.materialId,0) " +
                                        "    LEFT JOIN dimProdCat prodCat " +
                                        "    ON ISNULL(prodCat.idProdCat,0) = ISNULL(allConfig.prodCatId,0) " +
                                        "    LEFT JOIN dimProdSpec prodSpec " +
                                        "    ON ISNULL(prodSpec.idProdSpec,0) = ISNULL(allConfig.prodSpecId,0) " +
                                        "    LEFT JOIN tblProductItem productItem  " +
                                        "    ON ISNULL(productItem.idProdItem,0)=ISNULL(allConfig.prodItemId,0) " +
                                        "    LEFT JOIN tblProdClassification prodClass " +
                                        "    ON  prodClass.idProdClass=productItem.prodClassId " +
                                        " ) stockConfig " +
                                        " ON cnfList.idOrganization = stockConfig.cnfOrgId " +
                                        " WHERE orgTypeId = " + (int)Constants.OrgTypeE.C_AND_F_AGENT + whereCond +
                                        " ORDER BY materialId ASC";

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@stockDate", System.Data.SqlDbType.DateTime).Value = stockDate.Date;
                cmdSelect.CommandTimeout = 100;  //Vijaymala added [05-06-2018] to change default timeout.
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingQuotaDeclarationTO> list = ConvertDTToListV2(sqlReader); //Sudhir[09-APR-2018] change Default ConvertDT to V2.
                sqlReader.Dispose();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }
        public  int InsertTblLoadingQuotaDeclaration(TblLoadingQuotaDeclarationTO tblLoadingQuotaDeclarationTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblLoadingQuotaDeclarationTO, cmdInsert);
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

        public int DeactivateAllPrevLoadingQuota(Int32 updatedBy, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;

                String sqlQuery = @" UPDATE [tblLoadingQuotaDeclaration] SET " +
                                "  [isActive]= @IsActive" +
                                " ,[updatedBy]= @UpdatedBy" +
                                " ,[updatedOn]= @UpdatedOn" +
                                " ,[balanceQuota]= @BalanceQuota" +
                                " ,[remark] = @Remark" +
                                "  WHERE isActive=1 ";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = 0;
                cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = updatedBy;
                cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.ServerDateTime;
                cmdUpdate.Parameters.Add("@BalanceQuota", System.Data.SqlDbType.NVarChar).Value = 0;
                cmdUpdate.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = "Old Loading Quota Deactivated";
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

        public  int ExecuteInsertionCommand(TblLoadingQuotaDeclarationTO tblLoadingQuotaDeclarationTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblLoadingQuotaDeclaration]( " +
                                "  [cnfOrgId]" +
                                " ,[materialId]" +
                                " ,[loadQuotaConfigId]" +
                                " ,[isActive]" +
                                " ,[createdBy]" +
                                " ,[updatedBy]" +
                                " ,[createdOn]" +
                                " ,[updatedOn]" +
                                " ,[allocQuota]" +
                                " ,[balanceQuota]" +
                                " ,[remark]" +
                                " ,[prodCatId]" +
                                " ,[prodSpecId]" +
                                " ,[transferedQuota]" +
                                " ,[receivedQuota]" +
                                " ,[prodItemId]" +
                                " )" +
                    " VALUES (" +
                                "  @CnfOrgId " +
                                " ,@MaterialId " +
                                " ,@LoadQuotaConfigId " +
                                " ,@IsActive " +
                                " ,@CreatedBy " +
                                " ,@UpdatedBy " +
                                " ,@CreatedOn " +
                                " ,@UpdatedOn " +
                                " ,@AllocQuota " +
                                " ,@BalanceQuota " +
                                " ,@Remark " +
                                " ,@prodCatId " +
                                " ,@prodSpecId " +
                                " ,@transferedQuota " +
                                " ,@receivedQuota " +
                                " ,@ProdItemId" +
                                " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdLoadingQuota", System.Data.SqlDbType.Int).Value = tblLoadingQuotaDeclarationTO.IdLoadingQuota;
            cmdInsert.Parameters.Add("@CnfOrgId", System.Data.SqlDbType.Int).Value = tblLoadingQuotaDeclarationTO.CnfOrgId;
            cmdInsert.Parameters.Add("@MaterialId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingQuotaDeclarationTO.MaterialId);
            cmdInsert.Parameters.Add("@LoadQuotaConfigId", System.Data.SqlDbType.Int).Value = tblLoadingQuotaDeclarationTO.LoadQuotaConfigId;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblLoadingQuotaDeclarationTO.IsActive;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblLoadingQuotaDeclarationTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingQuotaDeclarationTO.UpdatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblLoadingQuotaDeclarationTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingQuotaDeclarationTO.UpdatedOn);
            cmdInsert.Parameters.Add("@AllocQuota", System.Data.SqlDbType.NVarChar).Value = tblLoadingQuotaDeclarationTO.AllocQuota;
            cmdInsert.Parameters.Add("@BalanceQuota", System.Data.SqlDbType.NVarChar).Value = tblLoadingQuotaDeclarationTO.BalanceQuota;
            cmdInsert.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingQuotaDeclarationTO.Remark);
            cmdInsert.Parameters.Add("@prodCatId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingQuotaDeclarationTO.ProdCatId);
            cmdInsert.Parameters.Add("@prodSpecId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingQuotaDeclarationTO.ProdSpecId);
            cmdInsert.Parameters.Add("@transferedQuota", System.Data.SqlDbType.NVarChar).Value = tblLoadingQuotaDeclarationTO.TransferedQuota;
            cmdInsert.Parameters.Add("@receivedQuota", System.Data.SqlDbType.NVarChar).Value = tblLoadingQuotaDeclarationTO.ReceivedQuota;
            cmdInsert.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingQuotaDeclarationTO.ProdItemId);
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblLoadingQuotaDeclarationTO.IdLoadingQuota = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }

        public  List<TblLoadingQuotaDeclarationTO> ConvertDTToListV2(SqlDataReader tblLoadingQuotaDeclarationTODT)
        {
            List<TblLoadingQuotaDeclarationTO> tblLoadingQuotaDeclarationTOList = new List<TblLoadingQuotaDeclarationTO>();
            if (tblLoadingQuotaDeclarationTODT != null)
            {
                while (tblLoadingQuotaDeclarationTODT.Read())
                {
                    TblLoadingQuotaDeclarationTO tblLoadingQuotaDeclarationTONew = new TblLoadingQuotaDeclarationTO();
                    if (tblLoadingQuotaDeclarationTODT["idLoadingQuota"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.IdLoadingQuota = Convert.ToInt32(tblLoadingQuotaDeclarationTODT["idLoadingQuota"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["cnfOrgId"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.CnfOrgId = Convert.ToInt32(tblLoadingQuotaDeclarationTODT["cnfOrgId"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["materialId"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.MaterialId = Convert.ToInt32(tblLoadingQuotaDeclarationTODT["materialId"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["loadQuotaConfigId"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.LoadQuotaConfigId = Convert.ToInt32(tblLoadingQuotaDeclarationTODT["loadQuotaConfigId"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["isActive"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.IsActive = Convert.ToInt32(tblLoadingQuotaDeclarationTODT["isActive"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["createdBy"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.CreatedBy = Convert.ToInt32(tblLoadingQuotaDeclarationTODT["createdBy"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["updatedBy"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.UpdatedBy = Convert.ToInt32(tblLoadingQuotaDeclarationTODT["updatedBy"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["createdOn"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.CreatedOn = Convert.ToDateTime(tblLoadingQuotaDeclarationTODT["createdOn"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["updatedOn"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.UpdatedOn = Convert.ToDateTime(tblLoadingQuotaDeclarationTODT["updatedOn"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["allocQuota"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.AllocQuota = Convert.ToDouble(tblLoadingQuotaDeclarationTODT["allocQuota"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["balanceQuota"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.BalanceQuota = Convert.ToDouble(tblLoadingQuotaDeclarationTODT["balanceQuota"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["remark"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.Remark = Convert.ToString(tblLoadingQuotaDeclarationTODT["remark"].ToString());

                    if (string.IsNullOrEmpty(tblLoadingQuotaDeclarationTONew.Remark))
                        tblLoadingQuotaDeclarationTONew.Remark = "New Quota Declaration";

                    if (tblLoadingQuotaDeclarationTODT["cnfOrgName"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.CnfOrgName = Convert.ToString(tblLoadingQuotaDeclarationTODT["cnfOrgName"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["materialDesc"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.MaterialDesc = Convert.ToString(tblLoadingQuotaDeclarationTODT["materialDesc"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["prodCatId"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.ProdCatId = Convert.ToInt32(tblLoadingQuotaDeclarationTODT["prodCatId"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["prodSpecId"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.ProdSpecId = Convert.ToInt32(tblLoadingQuotaDeclarationTODT["prodSpecId"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["prodCatDesc"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.ProdCatDesc = Convert.ToString(tblLoadingQuotaDeclarationTODT["prodCatDesc"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["prodSpecDesc"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.ProdSpecDesc = Convert.ToString(tblLoadingQuotaDeclarationTODT["prodSpecDesc"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["transferedQuota"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.TransferedQuota = Convert.ToDouble(tblLoadingQuotaDeclarationTODT["transferedQuota"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["receivedQuota"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.ReceivedQuota = Convert.ToDouble(tblLoadingQuotaDeclarationTODT["receivedQuota"].ToString());

                    if (tblLoadingQuotaDeclarationTODT["prodItemId"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.ProdItemId = Convert.ToInt32(tblLoadingQuotaDeclarationTODT["prodItemId"].ToString());
                    if (tblLoadingQuotaDeclarationTODT["displayName"] != DBNull.Value)
                        tblLoadingQuotaDeclarationTONew.DisplayName = Convert.ToString(tblLoadingQuotaDeclarationTODT["displayName"].ToString());
                    //Not Required at this level. Shifted to Stock Detail Level
                    //if (tblLoadingQuotaDeclarationTODT["removedQuota"] != DBNull.Value)
                    //    tblLoadingQuotaDeclarationTONew.RemovedQuota = Convert.ToDouble(tblLoadingQuotaDeclarationTODT["removedQuota"].ToString());
                    tblLoadingQuotaDeclarationTOList.Add(tblLoadingQuotaDeclarationTONew);
                }
            }
            return tblLoadingQuotaDeclarationTOList;
        }
    }
}
