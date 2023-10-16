using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL.Interfaces;
using simpliMASTERSAPI.Models;
using ODLMWebAPI.BL;
using ICSharpCode.SharpZipLib.Zip;
using static ODLMWebAPI.StaticStuff.Constants;
using MimeKit;

namespace ODLMWebAPI.DAL
{
    public class TblGstCodeDtlsDAO : ITblGstCodeDtlsDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblGstCodeDtlsDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblGstCodeDtls]";
            return sqlSelectQry;
        }

        public String SqlSelectGstCodeDtlsQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblGstCodeDtls] ";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblGstCodeDtlsTO> SelectAllTblGstCodeDtls()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE isActive=1";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblGstCodeDtlsTO> list = ConvertDTToList(reader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public TblGstCodeDtlsTO SelectTblGstCodeDtls(Int32 idGstCode, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idGstCode = " + idGstCode + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblGstCodeDtlsTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1)
                    return list[0];

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public List<TblGstCodeDtlsTO> SelectTblGstCodeDtlsList(Int32 idGstCode, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectGstCodeDtlsQuery() + " WHERE idGstCode = " + idGstCode + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblGstCodeDtlsTO> list = ConvertDTToList(reader);
                if (list != null && list.Count != 0)
                    return list;

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public TblGstCodeDtlsTO SelectGstCodeDtlsTO(Int32 prodCatId, Int32 prodSpecId, Int32 materialId, Int32 prodItemId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idGstCode In(" + " SELECT gstCodeId FROM tblProdGstCodeDtls WHERE isActive = 1 " +
                                       "AND ISNULL(materialId,0) = " + materialId + " " +
                                       "AND ISNULL(prodCatId,0) = " + prodCatId + " " +
                                       "AND ISNULL(prodSpecId,0) = " + prodSpecId + " " +
                                       "AND ISNULL(prodItemId,0)=" + prodItemId + ")";

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblGstCodeDtlsTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1)
                    return list[0];

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public List<TblGstCodeDtlsTO> ConvertDTToList(SqlDataReader tblGstCodeDtlsTODT)
        {
            List<TblGstCodeDtlsTO> tblGstCodeDtlsTOList = new List<TblGstCodeDtlsTO>();
            if (tblGstCodeDtlsTODT != null)
            {
                while (tblGstCodeDtlsTODT.Read())
                {
                    TblGstCodeDtlsTO tblGstCodeDtlsTONew = new TblGstCodeDtlsTO();
                    if (tblGstCodeDtlsTODT["idGstCode"] != DBNull.Value)
                        tblGstCodeDtlsTONew.IdGstCode = Convert.ToInt32(tblGstCodeDtlsTODT["idGstCode"].ToString());
                    if (tblGstCodeDtlsTODT["codeTypeId"] != DBNull.Value)
                        tblGstCodeDtlsTONew.CodeTypeId = Convert.ToInt32(tblGstCodeDtlsTODT["codeTypeId"].ToString());
                    if (tblGstCodeDtlsTODT["createdBy"] != DBNull.Value)
                        tblGstCodeDtlsTONew.CreatedBy = Convert.ToInt32(tblGstCodeDtlsTODT["createdBy"].ToString());
                    if (tblGstCodeDtlsTODT["updatedBy"] != DBNull.Value)
                        tblGstCodeDtlsTONew.UpdatedBy = Convert.ToInt32(tblGstCodeDtlsTODT["updatedBy"].ToString());
                    if (tblGstCodeDtlsTODT["effectiveFromDt"] != DBNull.Value)
                        tblGstCodeDtlsTONew.EffectiveFromDt = Convert.ToDateTime(tblGstCodeDtlsTODT["effectiveFromDt"].ToString());
                    if (tblGstCodeDtlsTODT["effectiveToDt"] != DBNull.Value)
                        tblGstCodeDtlsTONew.EffectiveToDt = Convert.ToDateTime(tblGstCodeDtlsTODT["effectiveToDt"].ToString());
                    if (tblGstCodeDtlsTODT["createdOn"] != DBNull.Value)
                        tblGstCodeDtlsTONew.CreatedOn = Convert.ToDateTime(tblGstCodeDtlsTODT["createdOn"].ToString());
                    if (tblGstCodeDtlsTODT["updatedOn"] != DBNull.Value)
                        tblGstCodeDtlsTONew.UpdatedOn = Convert.ToDateTime(tblGstCodeDtlsTODT["updatedOn"].ToString());
                    if (tblGstCodeDtlsTODT["taxPct"] != DBNull.Value)
                        tblGstCodeDtlsTONew.TaxPct = Convert.ToDouble(tblGstCodeDtlsTODT["taxPct"].ToString());
                    if (tblGstCodeDtlsTODT["codeDesc"] != DBNull.Value)
                        tblGstCodeDtlsTONew.CodeDesc = Convert.ToString(tblGstCodeDtlsTODT["codeDesc"].ToString());
                    if (tblGstCodeDtlsTODT["codeNumber"] != DBNull.Value)
                        tblGstCodeDtlsTONew.CodeNumber = Convert.ToString(tblGstCodeDtlsTODT["codeNumber"].ToString());
                    if (tblGstCodeDtlsTODT["isActive"] != DBNull.Value)
                        tblGstCodeDtlsTONew.IsActive = Convert.ToInt32(tblGstCodeDtlsTODT["isActive"].ToString());

                    if (tblGstCodeDtlsTODT["sapHsnCode"] != DBNull.Value)
                        tblGstCodeDtlsTONew.SapHsnCode = Convert.ToString(tblGstCodeDtlsTODT["sapHsnCode"].ToString());

                    try
                    {
                        if (tblGstCodeDtlsTODT["merchantExportTaxPct"] != DBNull.Value)
                            tblGstCodeDtlsTONew.MerchantExportTaxPct = Convert.ToDouble(tblGstCodeDtlsTODT["merchantExportTaxPct"].ToString());
                    }
                    catch (Exception ex)
                    {

                    }
                    if (tblGstCodeDtlsTODT["directTypeCatTaxPct"] != DBNull.Value)
                        tblGstCodeDtlsTONew.DirectTypeCatTaxPct = Convert.ToDouble(tblGstCodeDtlsTODT["directTypeCatTaxPct"].ToString());
                    tblGstCodeDtlsTOList.Add(tblGstCodeDtlsTONew);
                }
            }
            return tblGstCodeDtlsTOList;
        }

        public List<TblGstCodeDtlsTO> SearchGSTCodeDetails(string searchedStr, Int32 searchCriteria, Int32 codeTypeId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                String selectQuery = SqlSelectQuery();
                String whereConditionsStr = "WHERE isActive=1 ";

                if (codeTypeId > 0)
                {
                    whereConditionsStr += " AND codeTypeId = " + codeTypeId;
                }

                if (!String.IsNullOrEmpty(searchedStr))
                {
                    if (searchCriteria == (Int32)Constants.GSTCodeDtlsEnum.CODE)
                    {
                        whereConditionsStr += " AND codeNumber = " + searchedStr;
                    }
                    else if (searchCriteria == (Int32)Constants.GSTCodeDtlsEnum.DESCRIPTION)
                    {
                        whereConditionsStr += " AND codeDesc LIKE '%" + searchedStr + "%' ";
                    }
                    else if (searchCriteria == (Int32)Constants.GSTCodeDtlsEnum.PERCENTAGE)
                    {
                        whereConditionsStr += " AND taxPct = " + searchedStr;
                    }
                }

                cmdSelect.CommandText = selectQuery + whereConditionsStr;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblGstCodeDtlsTO> list = ConvertDTToList(reader);
                if (list != null && list.Count != 0)
                    return list;

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }
        public TblGstCodeDtlsTO SelectTblGstCodeDtls(String codeNumberStr, Int32 CodeTypeId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE codeNumber = '" + codeNumberStr + "' AND codeTypeId = " + CodeTypeId + "";
                if (CodeTypeId == 2) //SAC
                {
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE (codeNumber = '" + codeNumberStr + "' OR codeNumber = '" + "00" + codeNumberStr + "') AND codeTypeId = " + CodeTypeId + "";
                }
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblGstCodeDtlsTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1)
                    return list[0];

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }
        #endregion

        #region Insertion
        public int InsertTblGstCodeDtls(TblGstCodeDtlsTO tblGstCodeDtlsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblGstCodeDtlsTO, cmdInsert);
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

        public int InsertTblGstCodeDtls(TblGstCodeDtlsTO tblGstCodeDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblGstCodeDtlsTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblGstCodeDtlsTO tblGstCodeDtlsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblGstCodeDtls]( " +
                                "  [codeTypeId]" +
                                " ,[createdBy]" +
                                " ,[updatedBy]" +
                                " ,[effectiveFromDt]" +
                                " ,[effectiveToDt]" +
                                " ,[createdOn]" +
                                " ,[updatedOn]" +
                                " ,[taxPct]" +
                                " ,[codeDesc]" +
                                " ,[codeNumber]" +
                                " ,[isActive]" +
                                " )" +
                    " VALUES (" +
                                "  @CodeTypeId " +
                                " ,@CreatedBy " +
                                " ,@UpdatedBy " +
                                " ,@EffectiveFromDt " +
                                " ,@EffectiveToDt " +
                                " ,@CreatedOn " +
                                " ,@UpdatedOn " +
                                " ,@TaxPct " +
                                " ,@CodeDesc " +
                                " ,@CodeNumber " +
                                " ,@isActive " +
                                " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdGstCode", System.Data.SqlDbType.Int).Value = tblGstCodeDtlsTO.IdGstCode;
            cmdInsert.Parameters.Add("@CodeTypeId", System.Data.SqlDbType.Int).Value = tblGstCodeDtlsTO.CodeTypeId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblGstCodeDtlsTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblGstCodeDtlsTO.UpdatedBy);
            cmdInsert.Parameters.Add("@EffectiveFromDt", System.Data.SqlDbType.DateTime).Value = tblGstCodeDtlsTO.EffectiveFromDt;
            cmdInsert.Parameters.Add("@EffectiveToDt", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblGstCodeDtlsTO.EffectiveToDt);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblGstCodeDtlsTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblGstCodeDtlsTO.UpdatedOn);
            cmdInsert.Parameters.Add("@TaxPct", System.Data.SqlDbType.NVarChar).Value = tblGstCodeDtlsTO.TaxPct;
            cmdInsert.Parameters.Add("@CodeDesc", System.Data.SqlDbType.NVarChar).Value = tblGstCodeDtlsTO.CodeDesc;
            cmdInsert.Parameters.Add("@CodeNumber", System.Data.SqlDbType.NVarChar).Value = tblGstCodeDtlsTO.CodeNumber;
            cmdInsert.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value = tblGstCodeDtlsTO.IsActive;
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblGstCodeDtlsTO.IdGstCode = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }

            return 0;
        }
        public int InsertGstCodeDtlsInSAP(TblGstCodeDtlsTO tblGstCodeDtlsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.SAP_CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteSAPInsertionCommand(tblGstCodeDtlsTO, cmdInsert);
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
        public int ExecuteSAPInsertionCommand(TblGstCodeDtlsTO tblGstCodeDtlsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [OCHP]( " +
                                "  [AbsEntry]" +
                                " ,[Chapter]" +
                                " ,[Heading]" +
                                " ,[SubHeading]" +
                                " ,[Dscription]" +
                                " ,[ChapterID]" +
                                " )" +
                    " VALUES (" +
                                "  @AbsEntry " +
                                " ,@Chapter " +
                                " ,@Heading " +
                                " ,@SubHeading " +
                                " ,@Dscription " +
                                " ,@ChapterID " +
                                " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            cmdInsert.Parameters.Add("@AbsEntry", System.Data.SqlDbType.Int).Value = tblGstCodeDtlsTO.AbsEntry;
            cmdInsert.Parameters.Add("@Chapter", System.Data.SqlDbType.NVarChar).Value = tblGstCodeDtlsTO.Chapter;
            cmdInsert.Parameters.Add("@Heading", System.Data.SqlDbType.NVarChar).Value = tblGstCodeDtlsTO.Heading;
            cmdInsert.Parameters.Add("@SubHeading", System.Data.SqlDbType.NVarChar).Value = tblGstCodeDtlsTO.SubHeading;
            cmdInsert.Parameters.Add("@Dscription", System.Data.SqlDbType.NVarChar).Value = tblGstCodeDtlsTO.CodeDesc;
            cmdInsert.Parameters.Add("@ChapterID", System.Data.SqlDbType.NVarChar).Value = tblGstCodeDtlsTO.ChapterID;
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                return 1;
            }
            return 0;
        }

        public int InsertGstCodeDtlsInSAPV2(TblGstCodeDtlsTO tblGstCodeDtlsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.SAP_CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteSAPInsertionCommandV2(tblGstCodeDtlsTO, cmdInsert);
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
        public int ExecuteSAPInsertionCommandV2(TblGstCodeDtlsTO tblGstCodeDtlsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [OSAC]( " +
                                "  [AbsEntry]" +
                                " ,[ServName]" +
                                " ,[ServCode]" +
                                " )" +
                    " VALUES (" +
                                "  @AbsEntry " +
                                " ,@ServName " +
                                " ,@ServCode " +
                                " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            cmdInsert.Parameters.Add("@AbsEntry", System.Data.SqlDbType.Int).Value = tblGstCodeDtlsTO.AbsEntry;
            cmdInsert.Parameters.Add("@ServName", System.Data.SqlDbType.NVarChar).Value = tblGstCodeDtlsTO.CodeDesc;
            cmdInsert.Parameters.Add("@ServCode", System.Data.SqlDbType.NVarChar).Value = tblGstCodeDtlsTO.CodeNumber;
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                return 1;
            }
            return 0;
        }


        #endregion

        #region Updation
        public int UpdateTblGstCodeDtls(TblGstCodeDtlsTO tblGstCodeDtlsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblGstCodeDtlsTO, cmdUpdate);
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

        public int UpdateTblGstCodeDtls(TblGstCodeDtlsTO tblGstCodeDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblGstCodeDtlsTO, cmdUpdate);
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
        public int UpdateMappedHsnCodeOfGstDtls(TblGstCodeDtlsTO tblGstCodeDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                String sqlQuery = @" UPDATE [tblGstCodeDtls] SET " +
                           "  [sapHsnCode]= @SapHsnCode" +
                           " WHERE [idGstCode] = @IdGstCode";
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.Parameters.Add("@SapHsnCode", System.Data.SqlDbType.NVarChar).Value = tblGstCodeDtlsTO.SapHsnCode;
                cmdUpdate.Parameters.Add("@IdGstCode", System.Data.SqlDbType.Int).Value = tblGstCodeDtlsTO.IdGstCode;
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
        public int ExecuteUpdationCommand(TblGstCodeDtlsTO tblGstCodeDtlsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblGstCodeDtls] SET " +
                            "  [codeTypeId]= @CodeTypeId" +
                            " ,[updatedBy]= @UpdatedBy" +
                            " ,[effectiveFromDt]= @EffectiveFromDt" +
                            " ,[effectiveToDt]= @EffectiveToDt" +
                            " ,[updatedOn]= @UpdatedOn" +
                            " ,[taxPct]= @TaxPct" +
                            " ,[codeDesc]= @CodeDesc" +
                            " ,[codeNumber] = @CodeNumber" +
                            " ,[isActive] = @isActive" +
                            " WHERE [idGstCode] = @IdGstCode";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdGstCode", System.Data.SqlDbType.Int).Value = tblGstCodeDtlsTO.IdGstCode;
            cmdUpdate.Parameters.Add("@CodeTypeId", System.Data.SqlDbType.Int).Value = tblGstCodeDtlsTO.CodeTypeId;
            //cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblGstCodeDtlsTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblGstCodeDtlsTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@EffectiveFromDt", System.Data.SqlDbType.DateTime).Value = tblGstCodeDtlsTO.EffectiveFromDt;
            cmdUpdate.Parameters.Add("@EffectiveToDt", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblGstCodeDtlsTO.EffectiveToDt);
            //cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblGstCodeDtlsTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblGstCodeDtlsTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@TaxPct", System.Data.SqlDbType.NVarChar).Value = tblGstCodeDtlsTO.TaxPct;
            cmdUpdate.Parameters.Add("@CodeDesc", System.Data.SqlDbType.NVarChar).Value = tblGstCodeDtlsTO.CodeDesc;
            cmdUpdate.Parameters.Add("@CodeNumber", System.Data.SqlDbType.NVarChar).Value = tblGstCodeDtlsTO.CodeNumber;
            //cmdUpdate.Parameters.Add("@SapHsnCode", System.Data.SqlDbType.Int).Value = tblGstCodeDtlsTO.SapHsnCode;
            cmdUpdate.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value = tblGstCodeDtlsTO.IsActive;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public int DeleteTblGstCodeDtls(Int32 idGstCode)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idGstCode, cmdDelete);
            }
            catch (Exception ex)
            {


                return 0;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public int DeleteTblGstCodeDtls(Int32 idGstCode, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idGstCode, cmdDelete);
            }
            catch (Exception ex)
            {


                return 0;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public int ExecuteDeletionCommand(Int32 idGstCode, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblGstCodeDtls] " +
            " WHERE idGstCode = " + idGstCode + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idGstCode", System.Data.SqlDbType.Int).Value = tblGstCodeDtlsTO.IdGstCode;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        public List<TblGstCodeDtlsTO> SelectAllTblGstCodeDtlsForMigration()
        {
            String sqlConnStr = @"Data Source = tcp:internalcentralserver.database.windows.net,1433; Initial Catalog =VERP_CORONA_13APRIL21; Integrated Security = False; Uid = vega; Pwd = simple@123;";
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader tblGstCodeDtlsTODT = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = "SELECT * FROM common.mst_gst_good_rate where percentage in(0,18,12,28,5) and excise_service_type_id = 1 ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                tblGstCodeDtlsTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblGstCodeDtlsTO> tblGstCodeDtlsTOList = new List<TblGstCodeDtlsTO>();
                if (tblGstCodeDtlsTODT != null)
                {
                    while (tblGstCodeDtlsTODT.Read())
                    {
                        TblGstCodeDtlsTO tblGstCodeDtlsTONew = new TblGstCodeDtlsTO();
                        if (tblGstCodeDtlsTODT["excise_service_type_id"] != DBNull.Value)
                            tblGstCodeDtlsTONew.CodeTypeId = Convert.ToInt32(tblGstCodeDtlsTODT["excise_service_type_id"].ToString());
                        if (tblGstCodeDtlsTODT["percentage"] != DBNull.Value)
                            tblGstCodeDtlsTONew.TaxPct = Convert.ToDouble(tblGstCodeDtlsTODT["percentage"].ToString());
                        if (tblGstCodeDtlsTODT["gst_good_rate_description"] != DBNull.Value)
                            tblGstCodeDtlsTONew.CodeDesc = Convert.ToString(tblGstCodeDtlsTODT["gst_good_rate_description"].ToString());
                        if (tblGstCodeDtlsTODT["code"] != DBNull.Value)
                            tblGstCodeDtlsTONew.CodeNumber = Convert.ToString(tblGstCodeDtlsTODT["code"].ToString());

                        tblGstCodeDtlsTOList.Add(tblGstCodeDtlsTONew);
                    }
                }
                return tblGstCodeDtlsTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (tblGstCodeDtlsTODT != null) tblGstCodeDtlsTODT.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblGstCodeDtlsTO> SelectAllTblServiceGstCodeDtlsForMigration()
        {
            String sqlConnStr = @"Data Source = tcp:internalcentralserver.database.windows.net,1433; Initial Catalog =VERP_CORONA_13APRIL21; Integrated Security = False; Uid = vega; Pwd = simple@123;";
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader tblGstCodeDtlsTODT = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = "SELECT * FROM common.mst_gst_good_rate where percentage in(0,18,12,28,5) and excise_service_type_id = 2 ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                tblGstCodeDtlsTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblGstCodeDtlsTO> tblGstCodeDtlsTOList = new List<TblGstCodeDtlsTO>();
                if (tblGstCodeDtlsTODT != null)
                {
                    while (tblGstCodeDtlsTODT.Read())
                    {
                        TblGstCodeDtlsTO tblGstCodeDtlsTONew = new TblGstCodeDtlsTO();
                        if (tblGstCodeDtlsTODT["excise_service_type_id"] != DBNull.Value)
                            tblGstCodeDtlsTONew.CodeTypeId = Convert.ToInt32(tblGstCodeDtlsTODT["excise_service_type_id"].ToString());
                        if (tblGstCodeDtlsTODT["percentage"] != DBNull.Value)
                            tblGstCodeDtlsTONew.TaxPct = Convert.ToDouble(tblGstCodeDtlsTODT["percentage"].ToString());
                        if (tblGstCodeDtlsTODT["gst_good_rate_description"] != DBNull.Value)
                            tblGstCodeDtlsTONew.CodeDesc = Convert.ToString(tblGstCodeDtlsTODT["gst_good_rate_description"].ToString());
                        if (tblGstCodeDtlsTODT["code"] != DBNull.Value)
                            tblGstCodeDtlsTONew.CodeNumber = Convert.ToString(tblGstCodeDtlsTODT["code"].ToString());

                        tblGstCodeDtlsTOList.Add(tblGstCodeDtlsTONew);
                    }
                }
                return tblGstCodeDtlsTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (tblGstCodeDtlsTODT != null) tblGstCodeDtlsTODT.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        


    }

}
