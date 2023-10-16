


using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL.Interfaces;
using System.Linq;

namespace ODLMWebAPI.DAL
{
    public class TblProductAndRmConfigurationDAO : ITblProductAndRmConfigurationDAO

    {
        private readonly IConnectionString _iConnectionString;
        public TblProductAndRmConfigurationDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        public String SqlSelectQuery()
        {
            // String sqlSelectQry = "select * from tblProductItemRmToFGConfig";
            //String sqlSelectQry = "SELECT p.itemName,p.itemDesc,pc.prodCateDesc,dp.prodSpecDesc,m.materialSubType,um.weightMeasurUnitDesc
            //    FROM tblProductItemRmToFGConfig tblProdRMConfig"+"
            //    LEFT JION tblProductItem p
            //    ON p.idProdItem"

            String sqlSelectQry = " SELECT tblProdRMConfig.* ,product.itemName,product.itemDesc,prod.itemName as itemNameRM,prod.itemDesc as itemDescRM,pc.prodCateDesc,dps.prodSpecDesc,mt.materialSubType,um.weightMeasurUnitDesc as weightMeasurUnitDescForFG ,um1.weightMeasurUnitDesc as weightMeasurUnitDescForRM" +
                                  " FROM tblProductItemRmToFGConfig tblProdRMConfig " +

                                  " LEFT JOIN tblProductItem product ON product.idProdItem = tblProdRMConfig.fgProductItemId " +
                                  " LEFT JOIN dimProdCat pc ON pc.idProdCat = tblProdRMConfig.prodCatId " +
                                  " LEFT JOIN dimProdSpec dps ON dps.idProdSpec = tblProdRMConfig.prodSpecId " +
                                  " LEFT JOIN tblMaterial mt ON mt.idMaterial = tblProdRMConfig.materialId " +
                                  " LEFT JOIN dimUnitMeasures um ON um.idWeightMeasurUnit = tblProdRMConfig.fgUomId " +
                                  " LEFT JOIN tblProductItem prod ON prod.idProdItem = tblProdRMConfig.rmProductItemId" +
                                  " LEFT JOIN dimUnitMeasures um1 ON um1.idWeightMeasurUnit = tblProdRMConfig.rmUomId ";




            return sqlSelectQry;
        }
        #region Selection
        public List<TblProductAndRmConfigurationTO> SelectAllTblProductAndRmConfigurationList()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING); ;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + "WHERE tblProdRMConfig.isActive = 1";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProductAndRmConfigurationTO> list = ConvertDTToList(sqlReader);
                if (sqlReader != null)
                    sqlReader.Dispose();
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
        public List<TblProductAndRmConfigurationTO> ConvertDTToList(SqlDataReader tblProductAndRmConfigurationTODT)
        {
            List<TblProductAndRmConfigurationTO> tblProductAndRmConfigurationList = new List<TblProductAndRmConfigurationTO>();
            if (tblProductAndRmConfigurationTODT != null)
            {
                while (tblProductAndRmConfigurationTODT.Read())
                {
                    TblProductAndRmConfigurationTO tblProductAndRmConfigurationNew = new TblProductAndRmConfigurationTO();
                    if (tblProductAndRmConfigurationTODT["idProdItemRmToFgConfig"] != DBNull.Value)
                        tblProductAndRmConfigurationNew.IdProdItemRmToFgConfig = Convert.ToInt32(tblProductAndRmConfigurationTODT["idProdItemRmToFgConfig"].ToString());

                    //if (tblProductAndRmConfigurationTODT["isActive"] != DBNull.Value)
                    //    tblProductAndRmConfigurationNew.IsActive = Convert.ToInt32(tblProductAndRmConfigurationTODT["isActive"].ToString());
                    //if (tblProductAndRmConfigurationTODT["createdBy"] != DBNull.Value)
                    //    tblProductAndRmConfigurationNew.CreatedBy = Convert.ToInt32(tblProductAndRmConfigurationTODT["createdBy"].ToString());
                    //if (tblProductAndRmConfigurationTODT["updatedBy"] != DBNull.Value)
                    //    tblProductAndRmConfigurationNew.UpdatedBy = Convert.ToInt32(tblProductAndRmConfigurationTODT["updatedBy"].ToString());
                    //if (tblProductAndRmConfigurationTODT["createdOn"] != DBNull.Value)
                    //    tblProductAndRmConfigurationNew.CreatedOn = Convert.ToDateTime(tblProductAndRmConfigurationTODT["createdOn"].ToString());
                    //if (tblProductAndRmConfigurationTODT["updatedOn"] != DBNull.Value)
                    //    tblProductAndRmConfigurationNew.UpdatedOn = Convert.ToDateTime(tblProductAndRmConfigurationTODT["updatedOn"].ToString());


                    if (tblProductAndRmConfigurationTODT["fgProductItemId"] != DBNull.Value)
                        tblProductAndRmConfigurationNew.FgProductItemId = Convert.ToInt32(tblProductAndRmConfigurationTODT["fgProductItemId"].ToString());
                    if (tblProductAndRmConfigurationTODT["prodCatId"] != DBNull.Value)
                        tblProductAndRmConfigurationNew.ProdCatId = Convert.ToInt32(tblProductAndRmConfigurationTODT["prodCatId"].ToString());
                    if (tblProductAndRmConfigurationTODT["prodSpecId"] != DBNull.Value)
                        tblProductAndRmConfigurationNew.ProdSpecId = Convert.ToInt32(tblProductAndRmConfigurationTODT["prodSpecId"].ToString());
                    if (tblProductAndRmConfigurationTODT["materialId"] != DBNull.Value)
                        tblProductAndRmConfigurationNew.MaterialId = Convert.ToInt32(tblProductAndRmConfigurationTODT["materialId"].ToString());
                    if (tblProductAndRmConfigurationTODT["fgUomId"] != DBNull.Value)
                        tblProductAndRmConfigurationNew.FgUomId = Convert.ToInt32(tblProductAndRmConfigurationTODT["fgUomId"].ToString());
                    if (tblProductAndRmConfigurationTODT["rmProductItemId"] != DBNull.Value)
                        tblProductAndRmConfigurationNew.RmProductItemId = Convert.ToInt32(tblProductAndRmConfigurationTODT["rmProductItemId"]);
                    if (tblProductAndRmConfigurationTODT["rmUomId"] != DBNull.Value)
                        tblProductAndRmConfigurationNew.RmUomId = Convert.ToInt32(tblProductAndRmConfigurationTODT["rmUomId"]);
                    if (tblProductAndRmConfigurationTODT["rmToFgConversionRatio"] != DBNull.Value)
                        tblProductAndRmConfigurationNew.RmToFgConversionRatio = Convert.ToDecimal(tblProductAndRmConfigurationTODT["rmToFgConversionRatio"]);
                    // p.itemName,p.itemDesc,pc.prodCateDesc,dp.prodSpecDesc,m.materialSubType,um.weightMeasurUnitDesc

                    if (tblProductAndRmConfigurationTODT["itemName"] != DBNull.Value)
                        tblProductAndRmConfigurationNew.ItemName = Convert.ToString(tblProductAndRmConfigurationTODT["itemName"]);
                    if (tblProductAndRmConfigurationTODT["itemDesc"] != DBNull.Value)
                        tblProductAndRmConfigurationNew.ItemDesc = Convert.ToString(tblProductAndRmConfigurationTODT["itemDesc"]);
                    if (tblProductAndRmConfigurationTODT["prodCateDesc"] != DBNull.Value)
                        tblProductAndRmConfigurationNew.ProdCateDesc = Convert.ToString(tblProductAndRmConfigurationTODT["prodCateDesc"]);
                    if (tblProductAndRmConfigurationTODT["prodSpecDesc"] != DBNull.Value)
                        tblProductAndRmConfigurationNew.ProdSpecDesc = Convert.ToString(tblProductAndRmConfigurationTODT["prodSpecDesc"]);
                    if (tblProductAndRmConfigurationTODT["materialSubType"] != DBNull.Value)
                        tblProductAndRmConfigurationNew.MaterialSubType = Convert.ToString(tblProductAndRmConfigurationTODT["materialSubType"]);
                    if (tblProductAndRmConfigurationTODT["weightMeasurUnitDescForFG"] != DBNull.Value)
                        tblProductAndRmConfigurationNew.WeightMeasurUnitDesc = Convert.ToString(tblProductAndRmConfigurationTODT["weightMeasurUnitDescForFG"]);


                    if (tblProductAndRmConfigurationTODT["itemNameRM"] != DBNull.Value)
                        tblProductAndRmConfigurationNew.ItemNameRM = Convert.ToString(tblProductAndRmConfigurationTODT["itemNameRM"]);
                    if (tblProductAndRmConfigurationTODT["itemDescRM"] != DBNull.Value)
                        tblProductAndRmConfigurationNew.ItemDescRM = Convert.ToString(tblProductAndRmConfigurationTODT["itemDescRM"]);
                    if (tblProductAndRmConfigurationTODT["weightMeasurUnitDescForRM"] != DBNull.Value)
                        tblProductAndRmConfigurationNew.WeightMeasurUnitDescForRM = Convert.ToString(tblProductAndRmConfigurationTODT["weightMeasurUnitDescForRM"]);
                    //

                    //Sanjay [13-Nov-2019] to brand condition is added later on
                    var exists = Enumerable.Range(0, tblProductAndRmConfigurationTODT.FieldCount).Any(i => string.Equals(tblProductAndRmConfigurationTODT.GetName(i), "brandId", StringComparison.OrdinalIgnoreCase));
                    if (exists)
                    {
                        if (tblProductAndRmConfigurationTODT["brandId"] != DBNull.Value)
                            tblProductAndRmConfigurationNew.BrandId = Convert.ToInt32(tblProductAndRmConfigurationTODT["brandId"].ToString());
                    }
                    var isFgToFgMapping = Enumerable.Range(0, tblProductAndRmConfigurationTODT.FieldCount).Any(i => string.Equals(tblProductAndRmConfigurationTODT.GetName(i), "isFgToFgMapping", StringComparison.OrdinalIgnoreCase));
                    if (isFgToFgMapping)
                    {
                        if (tblProductAndRmConfigurationTODT["isFgToFgMapping"] != DBNull.Value)
                            tblProductAndRmConfigurationNew.IsFgToFgMapping = Convert.ToInt32(tblProductAndRmConfigurationTODT["isFgToFgMapping"].ToString());
                    }
                    tblProductAndRmConfigurationList.Add(tblProductAndRmConfigurationNew);
                }
            }

            return tblProductAndRmConfigurationList;
        }
        #endregion

        #region Insertion
        public int InsertTblProductAndRmConfiguration(TblProductAndRmConfigurationTO tblProductAndRmConfigurationTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING); ;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblProductAndRmConfigurationTO, cmdInsert);
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

        public int InsertTblProductAndRmConfiguration(TblProductAndRmConfigurationTO tblProductAndRmConfigurationTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblProductAndRmConfigurationTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblProductAndRmConfigurationTO tblProductAndRmConfigurationTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblProductItemRmToFGConfig]( " +
            //" [idProdItemRmToFgConfig]" +
            " [fgProductItemId]" +
            " ,[prodCatId]" +
            " ,[prodSpecId]" +
            " ,[materialId]" +
            " ,[fgUomId]" +
            " ,[rmProductItemId]" +
            " ,[rmUomId]" +
            " ,[rmToFgConversionRatio]" +
            " ,[isActive]" +
            " ,[createdOn]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[updatedOn]" +
            " ,[brandId]" +
            " ,[isFgToFgMapping]" +
            " )" +
" VALUES (" +
            // "  @IdProdItemRmToFgConfig " +
            " @FgProductItemId " +
            " ,@ProdCatId " +
            " ,@ProdSpecId " +
            " ,@MaterialId " +
            " ,@FgUomId " +
            " ,@RmProductItemId " +
            " ,@RmUomId " +
            " ,@RmToFgConversionRatio " +
            " ,@IsActive " +
            " ,@CreatedOn " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@UpdatedOn " +
            " ,@brandId " +
            " ,@isFgToFgMapping " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            // cmdInsert.Parameters.Add("@IdProdItemRmToFgConfig", System.Data.SqlDbType.Int).Value = tblProductAndRmConfigurationTO.IdProdItemRmToFgConfig;
            cmdInsert.Parameters.Add("@FgProductItemId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductAndRmConfigurationTO.FgProductItemId);
            cmdInsert.Parameters.Add("@ProdCatId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductAndRmConfigurationTO.ProdCatId);
            cmdInsert.Parameters.Add("@ProdSpecId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductAndRmConfigurationTO.ProdSpecId);
            cmdInsert.Parameters.Add("@MaterialId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductAndRmConfigurationTO.MaterialId);

            cmdInsert.Parameters.Add("@FgUomId", System.Data.SqlDbType.Int).Value = tblProductAndRmConfigurationTO.FgUomId;
            cmdInsert.Parameters.Add("@RmProductItemId", System.Data.SqlDbType.Int).Value = tblProductAndRmConfigurationTO.RmProductItemId;
            cmdInsert.Parameters.Add("@RmUomId", System.Data.SqlDbType.Int).Value = tblProductAndRmConfigurationTO.RmUomId;

            cmdInsert.Parameters.Add("@RmToFgConversionRatio", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductAndRmConfigurationTO.RmToFgConversionRatio);

            cmdInsert.Parameters.Add("@isActive", SqlDbType.Int).Value = tblProductAndRmConfigurationTO.IsActive;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblProductAndRmConfigurationTO.CreatedOn;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblProductAndRmConfigurationTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductAndRmConfigurationTO.UpdatedBy);
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductAndRmConfigurationTO.UpdatedOn);

            //Sanjay [13-Nov-2019] Added considering Sales Invoice Integration with SAP. Item Mapping is required
            cmdInsert.Parameters.Add("@brandId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductAndRmConfigurationTO.BrandId);
            cmdInsert.Parameters.Add("@isFgToFgMapping", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductAndRmConfigurationTO.IsFgToFgMapping);

            // return cmdInsert.ExecuteNonQuery();
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                return 1;
            }

            return 0;
        }
        #endregion

        #region Updation
        public int UpdateTblProductAndRmConfiguration(TblProductAndRmConfigurationTO tblProductAndRmConfigurationTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING); ;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblProductAndRmConfigurationTO, cmdUpdate);
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

        public int UpdateTblProductAndRmConfiguration(TblProductAndRmConfigurationTO tblProductAndRmConfigurationTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblProductAndRmConfigurationTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(TblProductAndRmConfigurationTO tblProductAndRmConfigurationTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblProductItemRmToFGConfig] SET " +
            " [fgProductItemId]= @FgProductItemId" +
            " ,[prodCatId]= @ProdCatId" +
            " ,[prodSpecId]= @ProdSpecId" +
            " ,[materialId]= @MaterialId" +
            " ,[fgUomId]= @FgUomId" +
            " ,[rmProductItemId]= @RmProductItemId" +
            " ,[rmUomId]= @RmUomId" +
            " ,[rmToFgConversionRatio]= @RmToFgConversionRatio" +
            " ,[isActive]= @IsActive" +
            " ,[createdOn]= @CreatedOn" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[brandId]= @brandId" +
            " ,[isFgToFgMapping]= @isFgToFgMapping" +
            "  WHERE [idProdItemRmToFgConfig] = @IdProdItemRmToFgConfig";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdProdItemRmToFgConfig", System.Data.SqlDbType.Int).Value = tblProductAndRmConfigurationTO.IdProdItemRmToFgConfig;

            cmdUpdate.Parameters.Add("@FgProductItemId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductAndRmConfigurationTO.FgProductItemId);
            cmdUpdate.Parameters.Add("@ProdCatId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductAndRmConfigurationTO.ProdCatId);
            cmdUpdate.Parameters.Add("@ProdSpecId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductAndRmConfigurationTO.ProdSpecId);
            cmdUpdate.Parameters.Add("@MaterialId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductAndRmConfigurationTO.MaterialId);

            cmdUpdate.Parameters.Add("@FgUomId", System.Data.SqlDbType.Int).Value = tblProductAndRmConfigurationTO.FgUomId;
            cmdUpdate.Parameters.Add("@RmProductItemId", System.Data.SqlDbType.Int).Value = tblProductAndRmConfigurationTO.RmProductItemId;
            cmdUpdate.Parameters.Add("@RmUomId", System.Data.SqlDbType.Int).Value = tblProductAndRmConfigurationTO.RmUomId;

            cmdUpdate.Parameters.Add("@RmToFgConversionRatio", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductAndRmConfigurationTO.RmToFgConversionRatio);

            cmdUpdate.Parameters.Add("@isActive", SqlDbType.Int).Value = tblProductAndRmConfigurationTO.IsActive;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblProductAndRmConfigurationTO.CreatedOn;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblProductAndRmConfigurationTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductAndRmConfigurationTO.UpdatedBy);
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductAndRmConfigurationTO.UpdatedOn);

            //Sanjay [13-Nov-2019] Added considering Sales Invoice Integration with SAP. Item Mapping is required
            cmdUpdate.Parameters.Add("@brandId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductAndRmConfigurationTO.BrandId);
            cmdUpdate.Parameters.Add("@isFgToFgMapping", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProductAndRmConfigurationTO.IsFgToFgMapping);

            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

    }
}
