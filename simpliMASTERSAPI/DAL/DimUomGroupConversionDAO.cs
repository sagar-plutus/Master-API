using System;
using System.Collections.Generic;
using System.Collections;
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
    public class DimUomGroupConversionDAO : IDimUomGroupConversionDAO
    {
        private readonly IConnectionString _iConnectionString;
        public DimUomGroupConversionDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }

        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT dimUomGroupConversion.*,dimUnitMeasures.weightMeasurUnitDesc FROM [dimUomGroupConversion] dimUomGroupConversion " +
                " LEFT JOIN dimUnitMeasures dimUnitMeasures on dimUnitMeasures.idWeightMeasurUnit = dimUomGroupConversion.uomId ";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<DimUomGroupConversionTO> SelectAllDimUomGroupConversion()
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

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimUomGroupConversionTO> list = ConvertDTToList(sqlReader);
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

        public List<DimUomGroupConversionTO> GetAllUOMGroupConversionListByGroupId(int uomGroupId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE uomGroupId = " + uomGroupId + " AND dimUnitMeasures.isActive =1 ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimUomGroupConversionTO> list = ConvertDTToList(sqlReader);
                if (list != null && list.Count > 0)
                    return list;
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
        public DimUomGroupConversionTO SelectDimUomGroupConversion(Int32 idUomConversion)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idUomConversion = " + idUomConversion + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimUomGroupConversionTO> list = ConvertDTToList(sqlReader);
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

        public List<DimUomGroupConversionTO> SelectAllDimUomGroupConversion(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimUomGroupConversionTO> list = ConvertDTToList(sqlReader);
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
        public List<DimUomGroupConversionTO> ConvertDTToList(SqlDataReader dimUomGroupConversionTODT)
        {
            List<DimUomGroupConversionTO> dimUomGroupConversionTOList = new List<DimUomGroupConversionTO>();
            if (dimUomGroupConversionTODT != null)
            {
                while (dimUomGroupConversionTODT.Read())
                {
                    DimUomGroupConversionTO dimUomGroupConversionTONew = new DimUomGroupConversionTO();
                    if (dimUomGroupConversionTODT["idUomConversion"] != DBNull.Value)
                        dimUomGroupConversionTONew.IdUomConversion = Convert.ToInt32(dimUomGroupConversionTODT["idUomConversion"].ToString());
                    if (dimUomGroupConversionTODT["uomGroupId"] != DBNull.Value)
                        dimUomGroupConversionTONew.UomGroupId = Convert.ToInt32(dimUomGroupConversionTODT["uomGroupId"].ToString());
                    if (dimUomGroupConversionTODT["uomId"] != DBNull.Value)
                        dimUomGroupConversionTONew.UomId = Convert.ToInt32(dimUomGroupConversionTODT["uomId"].ToString());
                    if (dimUomGroupConversionTODT["altQty"] != DBNull.Value)
                        dimUomGroupConversionTONew.AltQty = Convert.ToDouble(dimUomGroupConversionTODT["altQty"].ToString());
                    if (dimUomGroupConversionTODT["baseQty"] != DBNull.Value)
                        dimUomGroupConversionTONew.BaseQty = Convert.ToDouble(dimUomGroupConversionTODT["baseQty"].ToString());
                    if (dimUomGroupConversionTODT["weightMeasurUnitDesc"] != DBNull.Value)
                        dimUomGroupConversionTONew.WeightMeasurUnitDesc = (dimUomGroupConversionTODT["weightMeasurUnitDesc"].ToString());
                    
                    dimUomGroupConversionTOList.Add(dimUomGroupConversionTONew);
                }
            }
            return dimUomGroupConversionTOList;
        }
        
        #endregion

        #region Insertion
        public int InsertDimUomGroupConversion(DimUomGroupConversionTO dimUomGroupConversionTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(dimUomGroupConversionTO, cmdInsert);
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

        public int InsertDimUomGroupConversion(DimUomGroupConversionTO dimUomGroupConversionTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(dimUomGroupConversionTO, cmdInsert);
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

        public int ExecuteInsertionCommand(DimUomGroupConversionTO dimUomGroupConversionTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [dimUomGroupConversion]( " +
            //"  [idUomConversion]" +
            "  [uomGroupId]" +
            " ,[uomId]" +
            " ,[altQty]" +
            " ,[baseQty]" +
            " )" +
" VALUES (" +
            //"  @IdUomConversion " +
            "  @UomGroupId " +
            " ,@UomId " +
            " ,@AltQty " +
            " ,@BaseQty " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdUomConversion", System.Data.SqlDbType.Int).Value = dimUomGroupConversionTO.IdUomConversion;
            cmdInsert.Parameters.Add("@UomGroupId", System.Data.SqlDbType.Int).Value = dimUomGroupConversionTO.UomGroupId;
            cmdInsert.Parameters.Add("@UomId", System.Data.SqlDbType.Int).Value = dimUomGroupConversionTO.UomId;
            cmdInsert.Parameters.Add("@AltQty", System.Data.SqlDbType.NVarChar).Value = dimUomGroupConversionTO.AltQty;
            cmdInsert.Parameters.Add("@BaseQty", System.Data.SqlDbType.NVarChar).Value = dimUomGroupConversionTO.BaseQty;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion

        #region Updation
        public int UpdateDimUomGroupConversion(DimUomGroupConversionTO dimUomGroupConversionTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(dimUomGroupConversionTO, cmdUpdate);
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

        public int UpdateDimUomGroupConversion(DimUomGroupConversionTO dimUomGroupConversionTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(dimUomGroupConversionTO, cmdUpdate);
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

        public int UpdateDimUomGroupConversionForConversion(Int32 uomGroupId,double altQty,int uomId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                //cmdUpdate.Connection = conn;
                //cmdUpdate.Transaction = tran;
                //return ExecuteUpdationCommand(dimUomGroupConversionTO, cmdUpdate);
                String sqlQuery = @"update dimUomGroupConversion set altQty = " +altQty +
            "  where uomGroupId =" +uomGroupId + " And uomId ="+uomId + "";
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;
                return cmdUpdate.ExecuteNonQuery();
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

        public int ExecuteUpdationCommand(DimUomGroupConversionTO dimUomGroupConversionTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [dimUomGroupConversion] SET " +
            //"  [idUomConversion] = @IdUomConversion" +
            "  [uomGroupId]= @UomGroupId" +
            " ,[uomId]= @UomId" +
            " ,[altQty]= @AltQty" +
            " ,[baseQty] = @BaseQty" +
            " WHERE [idUomConversion] = @IdUomConversion";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdUomConversion", System.Data.SqlDbType.Int).Value = dimUomGroupConversionTO.IdUomConversion;
            cmdUpdate.Parameters.Add("@UomGroupId", System.Data.SqlDbType.Int).Value = dimUomGroupConversionTO.UomGroupId;
            cmdUpdate.Parameters.Add("@UomId", System.Data.SqlDbType.Int).Value = dimUomGroupConversionTO.UomId;
            cmdUpdate.Parameters.Add("@AltQty", System.Data.SqlDbType.NVarChar).Value = dimUomGroupConversionTO.AltQty;
            cmdUpdate.Parameters.Add("@BaseQty", System.Data.SqlDbType.NVarChar).Value = dimUomGroupConversionTO.BaseQty;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public int DeleteDimUomGroupConversion(Int32 idUomConversion)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idUomConversion, cmdDelete);
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

        public int DeleteDimUomGroupConversion(Int32 idUomConversion, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idUomConversion, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idUomConversion, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [dimUomGroupConversion] " +
            " WHERE idUomConversion = " + idUomConversion + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idUomConversion", System.Data.SqlDbType.Int).Value = dimUomGroupConversionTO.IdUomConversion;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
