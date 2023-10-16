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

namespace ODLMWebAPI.DAL
{

    public class DimUomGroupDAO : IDimUomGroupDAO
    {
        private readonly IConnectionString _iConnectionString;
        public DimUomGroupDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }

        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT '' as baseUOMName,dimUomGrpCon.*, dimUomGroup.* FROM[dimUomGroup]  dimUomGroup " +
                                  " LEFT JOIN dimUomGroupConversion dimUomGrpCon ON dimUomGrpCon.uomGroupId = dimUomGroup.idUomGroup " +
                                  " WHERE ISNULL(isActive,0) =1";
            return sqlSelectQry;
        }
        public List<DimUomGroupTO> SelectAllUomGroupList(DimUomGroupTO dimUomGroupTO, SqlConnection conn, SqlTransaction tran)
        {

            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE uomGroupName = '" + dimUomGroupTO.UomGroupName + "'";
                if (dimUomGroupTO.IdUomGroup > 0)
                {
                    cmdSelect.CommandText += " AND idGroup != " + dimUomGroupTO.IdUomGroup + " AND isActive = 1";
                }
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimUomGroupTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                // conn.Close();
                cmdSelect.Dispose();
            }
        }

        #endregion

        #region Selection
        public List<DimUomGroupTO> SelectAllDimUomGroup()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "SELECT dimUomGroup.*,uom.weightMeasurUnitDesc as baseUOMName, dimUomGrpCon.* FROM[dimUomGroup] dimUomGroup " +
                                        " LEFT JOIN dimUnitMeasures uom ON uom.idWeightMeasurUnit = [dimUomGroup].baseUomId " +
                                        " LEFT JOIN dimUomGroupConversion dimUomGrpCon ON dimUomGrpCon.uomGroupId = dimUomGroup.idUomGroup " +
                                        " WHERE[dimUomGroup].isActive = 1";  //SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimUomGroupTO> list = ConvertDTToList(sqlReader);
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

        public DimUomGroupTO SelectDimUomGroup(Int32 idUomGroup)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " AND idUomGroup = " + idUomGroup + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimUomGroupTO> list = ConvertDTToList(sqlReader);
                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
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

        public List<DimUomGroupTO> SelectAllDimUomGroup(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimUomGroupTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
            }
        }

        public List<DimUomGroupTO> ConvertDTToList(SqlDataReader dimUomGroupTODT)
        {
            List<DimUomGroupTO> dimUomGroupTOList = new List<DimUomGroupTO>();
            if (dimUomGroupTODT != null)
            {
                while (dimUomGroupTODT.Read())
                {
                    DimUomGroupTO dimUomGroupTONew = new DimUomGroupTO();
                    if (dimUomGroupTODT["idUomGroup"] != DBNull.Value)
                        dimUomGroupTONew.IdUomGroup = Convert.ToInt32(dimUomGroupTODT["idUomGroup"].ToString());
                    if (dimUomGroupTODT["baseUomId"] != DBNull.Value)
                        dimUomGroupTONew.BaseUomId = Convert.ToInt32(dimUomGroupTODT["baseUomId"].ToString());
                    if (dimUomGroupTODT["createdBy"] != DBNull.Value)
                        dimUomGroupTONew.CreatedBy = Convert.ToInt32(dimUomGroupTODT["createdBy"].ToString());
                    if (dimUomGroupTODT["updatedBy"] != DBNull.Value)
                        dimUomGroupTONew.UpdatedBy = Convert.ToInt32(dimUomGroupTODT["updatedBy"].ToString());
                    if (dimUomGroupTODT["isActive"] != DBNull.Value)
                        dimUomGroupTONew.IsActive = Convert.ToInt32(dimUomGroupTODT["isActive"].ToString());
                    if (dimUomGroupTODT["createdOn"] != DBNull.Value)
                        dimUomGroupTONew.CreatedOn = Convert.ToDateTime(dimUomGroupTODT["createdOn"].ToString());
                    if (dimUomGroupTODT["updatedOn"] != DBNull.Value)
                        dimUomGroupTONew.UpdatedOn = Convert.ToDateTime(dimUomGroupTODT["updatedOn"].ToString());
                    if (dimUomGroupTODT["uomGroupCode"] != DBNull.Value)
                        dimUomGroupTONew.UomGroupCode = Convert.ToString(dimUomGroupTODT["uomGroupCode"].ToString());
                    if (dimUomGroupTODT["uomGroupName"] != DBNull.Value)
                        dimUomGroupTONew.UomGroupName = Convert.ToString(dimUomGroupTODT["uomGroupName"].ToString());

                    //DimUomGroupConversionTO uomGroupConversionTO = new DimUomGroupConversionTO();
                    //if (dimUomGroupTODT["idUomConversion"] != DBNull.Value)
                    //    uomGroupConversionTO.IdUomConversion = Convert.ToInt32(dimUomGroupTODT["idUomConversion"].ToString());
                    //if (dimUomGroupTODT["uomGroupId"] != DBNull.Value)
                    //    uomGroupConversionTO.UomGroupId = Convert.ToInt32(dimUomGroupTODT["uomGroupId"].ToString());
                    //if (dimUomGroupTODT["uomId"] != DBNull.Value)
                    //    uomGroupConversionTO.UomId = Convert.ToInt32(dimUomGroupTODT["uomId"].ToString());
                    //if (dimUomGroupTODT["altQty"] != DBNull.Value)
                    //    uomGroupConversionTO.AltQty = Convert.ToDouble(dimUomGroupTODT["altQty"].ToString());
                    //if (dimUomGroupTODT["baseQty"] != DBNull.Value)
                    //    uomGroupConversionTO.BaseQty = Convert.ToDouble(dimUomGroupTODT["baseQty"].ToString());

                    if (dimUomGroupTODT["uomId"] != DBNull.Value)
                        dimUomGroupTONew.ConversionUnitOfMeasure = Convert.ToInt32(dimUomGroupTODT["uomId"].ToString());
                    if (dimUomGroupTODT["altQty"] != DBNull.Value)
                        dimUomGroupTONew.ConversionFactor = Convert.ToDouble(dimUomGroupTODT["altQty"].ToString());
                    if (dimUomGroupTODT["mappedUomGroupId"] != DBNull.Value)
                        dimUomGroupTONew.MappedUomGroupId = Convert.ToString(dimUomGroupTODT["mappedUomGroupId"].ToString());

                    if (dimUomGroupTODT["baseUOMName"] != DBNull.Value)
                        dimUomGroupTONew.BaseUOMName = Convert.ToString(dimUomGroupTODT["baseUOMName"].ToString());

                    
                   // dimUomGroupTONew.UomGroupConversionTO = uomGroupConversionTO;
                    dimUomGroupTOList.Add(dimUomGroupTONew);
                }
            }
            return dimUomGroupTOList;
        }

        public DimUomGroupTO SelectDimUomGroup(int weightMeasureUnitId, int conversionUnitOfMeasure, double conversionFactor, SqlConnection conn, SqlTransaction tran)
        {
           SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader=null;
            try
            {              
                cmdSelect.CommandText = SqlSelectQuery() + " AND  baseUomId =" + weightMeasureUnitId;
                if (conversionUnitOfMeasure > 0)
                {
                    cmdSelect.CommandText += " AND dimUomGrpCon.uomId =" + conversionUnitOfMeasure;
                }
                if (conversionFactor > 0)
                {
                    cmdSelect.CommandText += " AND altQty=" + conversionFactor;
                }
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;

                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimUomGroupTO> list = ConvertDTToList(sqlReader);
                if (list != null && list.Count > 0)
                    return list[list.Count-1];
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null) sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }


        #endregion

        #region Insertion
        public int InsertDimUomGroup(DimUomGroupTO dimUomGroupTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(dimUomGroupTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public int InsertDimUomGroup(DimUomGroupTO dimUomGroupTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(dimUomGroupTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public int ExecuteInsertionCommand(DimUomGroupTO dimUomGroupTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [dimUomGroup]( " +
            //"  [idUomGroup]" +
            "  [baseUomId]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[isActive]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[uomGroupCode]" +
            " ,[uomGroupName]" +
            " )" +
" VALUES (" +
            // "  @IdUomGroup " +
            "  @BaseUomId " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@IsActive " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@UomGroupCode " +
            " ,@UomGroupName " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            String sqlSelectIdentityQry = "Select @@Identity";
            //cmdInsert.Parameters.Add("@IdUomGroup", System.Data.SqlDbType.Int).Value = dimUomGroupTO.IdUomGroup;
            cmdInsert.Parameters.Add("@BaseUomId", System.Data.SqlDbType.Int).Value = dimUomGroupTO.BaseUomId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = dimUomGroupTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(dimUomGroupTO.UpdatedBy);
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = dimUomGroupTO.IsActive;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = dimUomGroupTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(dimUomGroupTO.UpdatedOn);
            cmdInsert.Parameters.Add("@UomGroupCode", System.Data.SqlDbType.NVarChar).Value = dimUomGroupTO.UomGroupCode;
            cmdInsert.Parameters.Add("@UomGroupName", System.Data.SqlDbType.NVarChar).Value = dimUomGroupTO.UomGroupName;

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = sqlSelectIdentityQry;
                dimUomGroupTO.IdUomGroup = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
            //return cmdInsert.ExecuteNonQuery();
        }
        #endregion

        #region Updation
        public int UpdateDimUomGroup(DimUomGroupTO dimUomGroupTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(dimUomGroupTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public int UpdateDimUomGroup(DimUomGroupTO dimUomGroupTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(dimUomGroupTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public int ExecuteUpdationCommand(DimUomGroupTO dimUomGroupTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [dimUomGroup] SET " +
            // "  [idUomGroup] = @IdUomGroup" +
            " [baseUomId]= @BaseUomId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[isActive]= @IsActive" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[uomGroupCode]= @UomGroupCode" +
            " ,[uomGroupName] = @UomGroupName" +
             " ,[mappedUomGroupId] = @MappedUomGroupId" +
            " WHERE [idUomGroup] = @IdUomGroup ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdUomGroup", System.Data.SqlDbType.Int).Value = dimUomGroupTO.IdUomGroup;
            cmdUpdate.Parameters.Add("@BaseUomId", System.Data.SqlDbType.Int).Value = dimUomGroupTO.BaseUomId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = dimUomGroupTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = dimUomGroupTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(dimUomGroupTO.IsActive);
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = dimUomGroupTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = dimUomGroupTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@UomGroupCode", System.Data.SqlDbType.NVarChar).Value = dimUomGroupTO.UomGroupCode;
            cmdUpdate.Parameters.Add("@UomGroupName", System.Data.SqlDbType.NVarChar).Value = dimUomGroupTO.UomGroupName;
            cmdUpdate.Parameters.Add("@MappedUomGroupId", System.Data.SqlDbType.NVarChar).Value = dimUomGroupTO.MappedUomGroupId;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public int DeleteDimUomGroup(Int32 idUomGroup)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idUomGroup, cmdDelete);
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

        public int DeleteDimUomGroup(Int32 idUomGroup, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idUomGroup, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idUomGroup, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [dimUomGroup] " +
            " WHERE idUomGroup = " + idUomGroup + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idUomGroup", System.Data.SqlDbType.Int).Value = dimUomGroupTO.IdUomGroup;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
