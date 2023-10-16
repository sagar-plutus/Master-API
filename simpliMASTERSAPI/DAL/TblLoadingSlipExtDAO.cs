using ODLMWebAPI.Models;
using simpliMASTERSAPI.DAL.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System;

namespace simpliMASTERSAPI.DAL
{
    public class TblLoadingSlipExtDAO : ITblLoadingSlipExtDAO
    {
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT loadDtl.* ,loadLayers.layerDesc,material.materialSubType " +
                                  " , prodCat.prodCateDesc AS prodCatDesc ,prodSpec.prodSpecDesc " +
                                  ",category.prodClassDesc as categoryName ,subCategory.prodClassDesc as subCategoryName," +
                                  "specification.prodClassDesc as specificationName,item.itemName " +
                                  "  FROM tempLoadingSlipExt loadDtl " +
                                  "  LEFT JOIN dimLoadingLayers loadLayers " +
                                  "  ON loadDtl.loadingLayerid = loadLayers.idLoadingLayer " +
                                  "  LEFT JOIN tblMaterial material " +
                                  "  ON material.idMaterial = loadDtl.materialId " +
                                  " LEFT JOIN  dimProdCat prodCat ON prodCat.idProdCat=loadDtl.prodCatId" +
                                  " LEFT JOIN  dimProdSpec prodSpec ON prodSpec.idProdSpec=loadDtl.prodSpecId" +
                                  " LEFT JOIN tblProductItem item ON item.idProdItem = loadDtl.prodItemId " +
                                  " LEFT JOIN tblProdClassification specification ON item.prodClassId = specification.idProdClass " +
                                  " LEFT JOIN tblProdClassification subCategory ON specification.parentProdClassId = subCategory.idProdClass " +
                                  " LEFT JOIN tblProdClassification category ON subCategory.parentProdClassId = category.idProdClass " +
                                  // Vaibhav [20-Nov-2017] Added to select from finalLoadingSlipExt

                                  " UNION ALL " +
                                  " SELECT loadDtl.* ,loadLayers.layerDesc,material.materialSubType " +
                                  " , prodCat.prodCateDesc AS prodCatDesc ,prodSpec.prodSpecDesc " +
                                  ",category.prodClassDesc as categoryName ,subCategory.prodClassDesc as subCategoryName," +
                                  "specification.prodClassDesc as specificationName,item.itemName " +
                                  "  FROM finalLoadingSlipExt loadDtl " +
                                  "  LEFT JOIN dimLoadingLayers loadLayers " +
                                  "  ON loadDtl.loadingLayerid = loadLayers.idLoadingLayer " +
                                  "  LEFT JOIN tblMaterial material " +
                                  "  ON material.idMaterial = loadDtl.materialId " +
                                  " LEFT JOIN tblProductItem item ON item.idProdItem = loadDtl.prodItemId  " +
                                  " LEFT JOIN  dimProdCat prodCat ON prodCat.idProdCat=loadDtl.prodCatId" +
                                  " LEFT JOIN  dimProdSpec prodSpec ON prodSpec.idProdSpec=loadDtl.prodSpecId" +
                                  "  LEFT JOIN tblProdClassification specification ON item.prodClassId = specification.idProdClass " +
                                  "  LEFT JOIN tblProdClassification subCategory ON specification.parentProdClassId = subCategory.idProdClass " +
                                  "  LEFT JOIN tblProdClassification category ON subCategory.parentProdClassId = category.idProdClass ";

            return sqlSelectQry;
        }
        #endregion
        public List<TblLoadingSlipExtTO> SelectAllLoadingSlipExtListFromLoadingId(String loadingIds, SqlConnection conn, SqlTransaction tran)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE loadingSlipId IN (SELECT idLoadingSlip FROM tempLoadingSlip WHERE loadingId IN(" + loadingIds + ") )" +

                                        // Vaibhav [20-Nov-2017] Added to select from finalLoadingSlip
                                        " UNION ALL " + " SELECT * FROM (" + SqlSelectQuery() + ")sq2 WHERE loadingSlipId IN (SELECT idLoadingSlip FROM finalLoadingSlip WHERE loadingId IN(" + loadingIds + ") )";

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipExtTO> list = ConvertDTToList(sqlReader);
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
        public static List<TblLoadingSlipExtTO> ConvertDTToList(SqlDataReader tblLoadingSlipExtTODT)
        {
            List<TblLoadingSlipExtTO> tblLoadingSlipExtTOList = new List<TblLoadingSlipExtTO>();
            if (tblLoadingSlipExtTODT != null)
            {
                while (tblLoadingSlipExtTODT.Read())
                {
                    TblLoadingSlipExtTO tblLoadingSlipExtTONew = new TblLoadingSlipExtTO();
                    if (tblLoadingSlipExtTODT["idLoadingSlipExt"] != DBNull.Value)
                        tblLoadingSlipExtTONew.IdLoadingSlipExt = Convert.ToInt32(tblLoadingSlipExtTODT["idLoadingSlipExt"].ToString());
                    if (tblLoadingSlipExtTODT["bookingId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.BookingId = Convert.ToInt32(tblLoadingSlipExtTODT["bookingId"].ToString());
                    if (tblLoadingSlipExtTODT["loadingSlipId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.LoadingSlipId = Convert.ToInt32(tblLoadingSlipExtTODT["loadingSlipId"].ToString());
                    if (tblLoadingSlipExtTODT["loadingLayerid"] != DBNull.Value)
                        tblLoadingSlipExtTONew.LoadingLayerid = Convert.ToInt32(tblLoadingSlipExtTODT["loadingLayerid"].ToString());
                    if (tblLoadingSlipExtTODT["materialId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.MaterialId = Convert.ToInt32(tblLoadingSlipExtTODT["materialId"].ToString());
                    if (tblLoadingSlipExtTODT["bookingExtId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.BookingExtId = Convert.ToInt32(tblLoadingSlipExtTODT["bookingExtId"].ToString());
                    if (tblLoadingSlipExtTODT["loadingQty"] != DBNull.Value)
                        tblLoadingSlipExtTONew.LoadingQty = Convert.ToDouble(tblLoadingSlipExtTODT["loadingQty"].ToString());
                    if (tblLoadingSlipExtTODT["layerDesc"] != DBNull.Value)
                        tblLoadingSlipExtTONew.LoadingLayerDesc = Convert.ToString(tblLoadingSlipExtTODT["layerDesc"].ToString());
                    if (tblLoadingSlipExtTODT["materialSubType"] != DBNull.Value)
                        tblLoadingSlipExtTONew.MaterialDesc = Convert.ToString(tblLoadingSlipExtTODT["materialSubType"].ToString());

                    if (tblLoadingSlipExtTODT["quotaBforeLoading"] != DBNull.Value)
                        tblLoadingSlipExtTONew.QuotaBforeLoading = Convert.ToDouble(tblLoadingSlipExtTODT["quotaBforeLoading"].ToString());
                    if (tblLoadingSlipExtTODT["quotaAfterLoading"] != DBNull.Value)
                        tblLoadingSlipExtTONew.QuotaAfterLoading = Convert.ToDouble(tblLoadingSlipExtTODT["quotaAfterLoading"].ToString());

                    if (tblLoadingSlipExtTODT["prodCatId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.ProdCatId = Convert.ToInt32(tblLoadingSlipExtTODT["prodCatId"].ToString());
                    if (tblLoadingSlipExtTODT["prodSpecId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.ProdSpecId = Convert.ToInt32(tblLoadingSlipExtTODT["prodSpecId"].ToString());
                    if (tblLoadingSlipExtTODT["prodCatDesc"] != DBNull.Value)
                        tblLoadingSlipExtTONew.ProdCatDesc = Convert.ToString(tblLoadingSlipExtTODT["prodCatDesc"].ToString());
                    if (tblLoadingSlipExtTODT["prodSpecDesc"] != DBNull.Value)
                        tblLoadingSlipExtTONew.ProdSpecDesc = Convert.ToString(tblLoadingSlipExtTODT["prodSpecDesc"].ToString());
                    if (tblLoadingSlipExtTODT["loadingQuotaId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.LoadingQuotaId = Convert.ToInt32(tblLoadingSlipExtTODT["loadingQuotaId"].ToString());

                    if (tblLoadingSlipExtTODT["bundles"] != DBNull.Value)
                        tblLoadingSlipExtTONew.Bundles = Convert.ToDouble(tblLoadingSlipExtTODT["bundles"].ToString());
                    if (tblLoadingSlipExtTODT["parityDtlId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.ParityDtlId = Convert.ToInt32(tblLoadingSlipExtTODT["parityDtlId"].ToString());
                    if (tblLoadingSlipExtTODT["ratePerMT"] != DBNull.Value)
                        tblLoadingSlipExtTONew.RatePerMT = Convert.ToDouble(tblLoadingSlipExtTODT["ratePerMT"].ToString());

                    if (tblLoadingSlipExtTODT["rateCalcDesc"] != DBNull.Value)
                        tblLoadingSlipExtTONew.RateCalcDesc = Convert.ToString(tblLoadingSlipExtTODT["rateCalcDesc"].ToString());

                    if (tblLoadingSlipExtTODT["loadedWeight"] != DBNull.Value)
                        tblLoadingSlipExtTONew.LoadedWeight = Convert.ToDouble(tblLoadingSlipExtTODT["loadedWeight"]);
                    if (tblLoadingSlipExtTODT["loadedBundles"] != DBNull.Value)
                        tblLoadingSlipExtTONew.LoadedBundles = Convert.ToDouble(tblLoadingSlipExtTODT["loadedBundles"]);
                    if (tblLoadingSlipExtTODT["calcTareWeight"] != DBNull.Value)
                        tblLoadingSlipExtTONew.CalcTareWeight = Convert.ToDouble(tblLoadingSlipExtTODT["calcTareWeight"]);
                    if (tblLoadingSlipExtTODT["weightMeasureId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.WeightMeasureId = Convert.ToInt32(tblLoadingSlipExtTODT["weightMeasureId"]);
                    if (tblLoadingSlipExtTODT["isAllowWeighingMachine"] != DBNull.Value)
                        tblLoadingSlipExtTONew.IsAllowWeighingMachine = Convert.ToInt32(tblLoadingSlipExtTODT["isAllowWeighingMachine"]);
                    if (tblLoadingSlipExtTODT["updatedBy"] != DBNull.Value)
                        tblLoadingSlipExtTONew.UpdatedBy = Convert.ToInt32(tblLoadingSlipExtTODT["updatedBy"]);
                    if (tblLoadingSlipExtTODT["updatedOn"] != DBNull.Value)
                        tblLoadingSlipExtTONew.UpdatedOn = Convert.ToDateTime(tblLoadingSlipExtTODT["updatedOn"]);
                    if (tblLoadingSlipExtTODT["cdStructureId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.CdStructureId = Convert.ToInt32(tblLoadingSlipExtTODT["cdStructureId"]);
                    if (tblLoadingSlipExtTODT["cdStructure"] != DBNull.Value)
                        tblLoadingSlipExtTONew.CdStructure = Convert.ToDouble(tblLoadingSlipExtTODT["cdStructure"]);
                    if (tblLoadingSlipExtTODT["prodItemDesc"] != DBNull.Value)
                        tblLoadingSlipExtTONew.ProdItemDesc = Convert.ToString(tblLoadingSlipExtTODT["prodItemDesc"]);
                    if (tblLoadingSlipExtTODT["prodItemId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.ProdItemId = Convert.ToInt32(tblLoadingSlipExtTODT["prodItemId"]);
                    if (tblLoadingSlipExtTODT["taxableRateMT"] != DBNull.Value)
                        tblLoadingSlipExtTONew.TaxableRateMT = Convert.ToDouble(tblLoadingSlipExtTODT["taxableRateMT"]);

                    if (tblLoadingSlipExtTODT["freExpOtherAmt"] != DBNull.Value)
                        tblLoadingSlipExtTONew.FreExpOtherAmt = Convert.ToDouble(tblLoadingSlipExtTODT["freExpOtherAmt"]);
                    if (tblLoadingSlipExtTODT["cdApplicableAmt"] != DBNull.Value)
                        tblLoadingSlipExtTONew.CdApplicableAmt = Convert.ToDouble(tblLoadingSlipExtTODT["cdApplicableAmt"]);
                    if (tblLoadingSlipExtTODT["weighingSequenceNumber"] != DBNull.Value)
                        tblLoadingSlipExtTONew.WeighingSequenceNumber = Convert.ToInt32(tblLoadingSlipExtTODT["weighingSequenceNumber"]);


                    if (tblLoadingSlipExtTODT["categoryName"] != DBNull.Value)
                        tblLoadingSlipExtTONew.CategoryName = Convert.ToString(tblLoadingSlipExtTODT["categoryName"].ToString());
                    if (tblLoadingSlipExtTODT["subCategoryName"] != DBNull.Value)
                        tblLoadingSlipExtTONew.SubCategoryName = Convert.ToString(tblLoadingSlipExtTODT["subCategoryName"].ToString());
                    if (tblLoadingSlipExtTODT["specificationName"] != DBNull.Value)
                        tblLoadingSlipExtTONew.SpecificationName = Convert.ToString(tblLoadingSlipExtTODT["specificationName"].ToString());
                    if (tblLoadingSlipExtTODT["itemName"] != DBNull.Value)
                        tblLoadingSlipExtTONew.ItemName = Convert.ToString(tblLoadingSlipExtTODT["itemName"].ToString());

                    if (tblLoadingSlipExtTODT["modbusRefId"] != DBNull.Value)
                        tblLoadingSlipExtTONew.ModbusRefId = Convert.ToInt32(tblLoadingSlipExtTODT["modbusRefId"]);

                    if (tblLoadingSlipExtTONew.ProdItemId > 0)
                    {
                        tblLoadingSlipExtTONew.DisplayName = tblLoadingSlipExtTONew.CategoryName + "-" + tblLoadingSlipExtTONew.SubCategoryName + "-" + tblLoadingSlipExtTONew.SpecificationName + "-" + tblLoadingSlipExtTONew.ItemName;
                    }
                    else
                    {
                        tblLoadingSlipExtTONew.DisplayName = tblLoadingSlipExtTONew.MaterialDesc + "-" + tblLoadingSlipExtTONew.ProdCatDesc + "-" + tblLoadingSlipExtTONew.ProdSpecDesc;
                    }

                    tblLoadingSlipExtTOList.Add(tblLoadingSlipExtTONew);
                }
            }
            return tblLoadingSlipExtTOList;
        }
    }
}
