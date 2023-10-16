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
    public class DimUnitMeasuresDAO : IDimUnitMeasuresDAO
    {
        private readonly IConnectionString _iConnectionString;
        private readonly ICommon _iCommon;
        public DimUnitMeasuresDAO(ICommon iCommon, IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
            _iCommon = iCommon;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [dimUnitMeasures]";
            return sqlSelectQry;
        }



        #endregion

        #region Selection

        /// <summary>
        /// Vaibhav [13-Sep-2017] Added to select all measurement units for drop down
        /// </summary> 
        /// </summary>
        /// <returns></returns>
        public List<DropDownTO> SelectAllUnitMeasuresForDropDown()
        { 
            ResultMessage resultMessage = new ResultMessage();

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                //cmdSelect.CommandText = SqlSelectQuery() + " WHERE isActive = 1 AND unitMeasureTypeId=" + unitMeasureTypeId;
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE isActive = 1 ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader tblUnitMeasuresTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                if (tblUnitMeasuresTODT != null)
                {
                    while (tblUnitMeasuresTODT.Read())
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        if (tblUnitMeasuresTODT["idWeightMeasurUnit"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(tblUnitMeasuresTODT["idWeightMeasurUnit"].ToString());
                        if (tblUnitMeasuresTODT["weightMeasurUnitDesc"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(tblUnitMeasuresTODT["weightMeasurUnitDesc"].ToString());
                        if (tblUnitMeasuresTODT["mappedTxnId"] != DBNull.Value)
                            dropDownTO.MappedTxnId = Convert.ToString(tblUnitMeasuresTODT["mappedTxnId"].ToString());
                        dropDownTOList.Add(dropDownTO);
                    }
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllUnitMeasuresForDropDown");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public List<DropDownTO> SelectAllUnitMeasuresForDropDownByCatId(Int32 unitCatId)
        {
            ResultMessage resultMessage = new ResultMessage();

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                //cmdSelect.CommandText = SqlSelectQuery() + " WHERE isActive = 1 AND unitMeasureTypeId=" + unitMeasureTypeId;
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE isActive = 1 AND unitCatId=" + unitCatId ;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader tblUnitMeasuresTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                if (tblUnitMeasuresTODT != null)
                {
                    while (tblUnitMeasuresTODT.Read())
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        if (tblUnitMeasuresTODT["idWeightMeasurUnit"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(tblUnitMeasuresTODT["idWeightMeasurUnit"].ToString());
                        if (tblUnitMeasuresTODT["weightMeasurUnitDesc"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(tblUnitMeasuresTODT["weightMeasurUnitDesc"].ToString());
                        dropDownTOList.Add(dropDownTO);
                    }
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllUnitMeasuresForDropDown");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<DimUnitMeasuresTO> SelectAllDimUnitMeasures()
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
                List<DimUnitMeasuresTO> list = ConvertDTToList(sqlReader);
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

        public DimUnitMeasuresTO SelectDimUnitMeasures(Int32 idWeightMeasurUnit)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idWeightMeasurUnit = " + idWeightMeasurUnit + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimUnitMeasuresTO> list = ConvertDTToList(sqlReader);
                if (sqlReader != null)
                    sqlReader.Dispose();

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

        public DimUnitMeasuresTO SelectDimUnitMeasures(String weightMeasurUnitDesc)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE weightMeasurUnitDesc = '" + weightMeasurUnitDesc + "' ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimUnitMeasuresTO> list = ConvertDTToList(sqlReader);
                if (sqlReader != null)
                    sqlReader.Dispose();

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

        public List<DimUnitMeasuresTO> ConvertDTToList(SqlDataReader dimUnitMeasuresTODT)
        {
            List<DimUnitMeasuresTO> tblMaterialTOList = new List<DimUnitMeasuresTO>();
            if (dimUnitMeasuresTODT != null)
            {
                while (dimUnitMeasuresTODT.Read())
                {
                    DimUnitMeasuresTO dimUnitMeasuresTONew = new DimUnitMeasuresTO();
                    if (dimUnitMeasuresTODT["idWeightMeasurUnit"] != DBNull.Value)
                        dimUnitMeasuresTONew.IdWeightMeasurUnit = Convert.ToInt32(dimUnitMeasuresTODT["idWeightMeasurUnit"].ToString());
                    if (dimUnitMeasuresTODT["weightMeasurUnitDesc"] != DBNull.Value)
                        dimUnitMeasuresTONew.WeightMeasurUnitDesc = dimUnitMeasuresTODT["weightMeasurUnitDesc"].ToString();
                    if (dimUnitMeasuresTODT["isActive"] != DBNull.Value)
                        dimUnitMeasuresTONew.IsActive = Convert.ToInt32(dimUnitMeasuresTODT["isActive"].ToString());
                    if (dimUnitMeasuresTODT["mappedTxnId"] != DBNull.Value)
                        dimUnitMeasuresTONew.MappedTxnId= Convert.ToInt32(dimUnitMeasuresTODT["mappedTxnId"].ToString());
                    tblMaterialTOList.Add(dimUnitMeasuresTONew);
                }
            }
            return tblMaterialTOList;
        }

        #endregion

        #region Insertion
        public int InsertDimUnitMeasures(DimUnitMeasuresTO dimUnitMeasuresTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(dimUnitMeasuresTO, cmdInsert);
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

        public int InsertDimUnitMeasures(DimUnitMeasuresTO dimUnitMeasuresTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(dimUnitMeasuresTO, cmdInsert);
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

        public int ExecuteInsertionCommand(DimUnitMeasuresTO dimUnitMeasuresTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [dimUnitMeasures]( " +
            "  [idWeightMeasurUnit]" +
            " ,[isActive]" +
            " ,[weightMeasurUnitDesc]" +
            " )" +
            " VALUES (" +
            "  @IdWeightMeasurUnit " +
            " ,@IsActive " +
            " ,@WeightMeasurUnitDesc " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdWeightMeasurUnit", System.Data.SqlDbType.Int).Value = dimUnitMeasuresTO.IdWeightMeasurUnit;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = dimUnitMeasuresTO.IsActive;
            cmdInsert.Parameters.Add("@WeightMeasurUnitDesc", System.Data.SqlDbType.NVarChar).Value = dimUnitMeasuresTO.WeightMeasurUnitDesc;
            return cmdInsert.ExecuteNonQuery();
        }
        public int InsertUOMGroupInSAP(string uomGroupName, Int32 baseUOM, Int32 ugpEntry, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                String sqlQuery = @" INSERT INTO [OUGP]( " +
                "  [UgpEntry]" +
                " ,[UgpCode]" +
                " ,[UgpName]" +
                " ,[BaseUom]" +
                " ,[DataSource]" +
                " ,[UserSign]" +
                " ,[CreateDate]" +
                " ,[Locked]" +
                " )" +
                " VALUES (" +
                "  @UgpEntry " +
                " ,@UgpCode " +
                " ,@UgpName " +
                " ,@BaseUom " +
                " ,@DataSource " +
                " ,@UserSign " +
                " ,@CreateDate " +
                " ,@Locked " +
                " )";
                cmdInsert.CommandText = sqlQuery;
                cmdInsert.CommandType = System.Data.CommandType.Text;

                cmdInsert.Parameters.Add("@UgpEntry", System.Data.SqlDbType.Int).Value = ugpEntry;
                cmdInsert.Parameters.Add("@UgpCode", System.Data.SqlDbType.NVarChar).Value = uomGroupName;
                cmdInsert.Parameters.Add("@UgpName", System.Data.SqlDbType.NVarChar).Value = uomGroupName;
                cmdInsert.Parameters.Add("@BaseUom", System.Data.SqlDbType.Int).Value = baseUOM;
                cmdInsert.Parameters.Add("@DataSource", System.Data.SqlDbType.NChar).Value = 'I';
                cmdInsert.Parameters.Add("@UserSign", System.Data.SqlDbType.Int).Value = 1;
                cmdInsert.Parameters.Add("@CreateDate", System.Data.SqlDbType.DateTime).Value = _iCommon.ServerDateTime;
                cmdInsert.Parameters.Add("@Locked", System.Data.SqlDbType.NVarChar).Value = 'N';
                if (cmdInsert.ExecuteNonQuery() == 1)
                {
                    return 1;
                }
                else return 0;
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
        public int InsertUOMGroupConversionInSAP(Int32 uomEntry, Double altQty, Int32 ugpEntry, Int32 LineNum, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                String sqlQuery = @" INSERT INTO [UGP1]( " +
                "  [UgpEntry]" +
                " ,[UomEntry]" +
                " ,[AltQty]" +
                " ,[BaseQty]" +
                " ,[LineNum]" +
                " )" +
                " VALUES (" +
                "  @UgpEntry " +
                " ,@UomEntry " +
                " ,@AltQty " +
                " ,@BaseQty " +
                " ,(select max(LineNum)+1 from UGP1 where UgpEntry = @UgpEntry) " +
                " )";
                cmdInsert.CommandText = sqlQuery;
                cmdInsert.CommandType = System.Data.CommandType.Text;

                cmdInsert.Parameters.Add("@UgpEntry", System.Data.SqlDbType.Int).Value = ugpEntry;
                cmdInsert.Parameters.Add("@UomEntry", System.Data.SqlDbType.Int).Value = uomEntry;
                cmdInsert.Parameters.Add("@AltQty", System.Data.SqlDbType.Decimal).Value = altQty;
                cmdInsert.Parameters.Add("@BaseQty", System.Data.SqlDbType.Decimal).Value = 1;
                //cmdInsert.Parameters.Add("@LineNum", System.Data.SqlDbType.Int).Value = LineNum;
                if (cmdInsert.ExecuteNonQuery() == 1)
                {
                    return 1;
                }
                else return 0;
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
        #endregion

        #region Updation
        public int UpdateDimUnitMeasures(DimUnitMeasuresTO dimUnitMeasuresTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(dimUnitMeasuresTO, cmdUpdate);
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

        public int UpdateDimUnitMeasures(DimUnitMeasuresTO dimUnitMeasuresTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(dimUnitMeasuresTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(DimUnitMeasuresTO dimUnitMeasuresTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [dimUnitMeasures] SET " +
            "  [idWeightMeasurUnit] = @IdWeightMeasurUnit" +
            " ,[isActive]= @IsActive" +
            " ,[weightMeasurUnitDesc] = @WeightMeasurUnitDesc" +
            " WHERE 1 = 2 ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdWeightMeasurUnit", System.Data.SqlDbType.Int).Value = dimUnitMeasuresTO.IdWeightMeasurUnit;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = dimUnitMeasuresTO.IsActive;
            cmdUpdate.Parameters.Add("@WeightMeasurUnitDesc", System.Data.SqlDbType.NVarChar).Value = dimUnitMeasuresTO.WeightMeasurUnitDesc;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public int DeleteDimUnitMeasures(Int32 idWeightMeasurUnit)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idWeightMeasurUnit, cmdDelete);
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

        public int DeleteDimUnitMeasures(Int32 idWeightMeasurUnit, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idWeightMeasurUnit, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idWeightMeasurUnit, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [dimUnitMeasures] " +
            " WHERE idWeightMeasurUnit = " + idWeightMeasurUnit + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idWeightMeasurUnit", System.Data.SqlDbType.Int).Value = dimUnitMeasuresTO.IdWeightMeasurUnit;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
