using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL.Interfaces;

namespace ODLMWebAPI.DAL
{
    public class TblProdClassificationDAO : ITblProdClassificationDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblProdClassificationDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT prodClass.*,itemProdCatg.itemProdCategory FROM [tblProdClassification] prodClass " +
                                   " LEFT JOIN dimItemProdCateg itemProdCatg ON itemProdCatg.idItemProdCat=prodClass.itemProdCatId ";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblProdClassificationTO> SelectAllTblProdClassification(string prodClassType = "")
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();

                if (string.IsNullOrEmpty(prodClassType))
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE prodClass.isActive=1 order by prodClass.prodClassDesc";
                else
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE prodClass.isActive=1 AND prodClassType='" + prodClassType + "' order by prodClass.prodClassDesc";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProdClassificationTO> list = ConvertDTToList(reader);
                return list;
            }
            catch(Exception ex)
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

        //Priyanka [17-06-2019]
        public List<TblProdClassificationTO> SelectAllTblProdClassificationByParentId(Int32 prodClassId, Int32 itemProdCatId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();

                if (prodClassId > 0)
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE prodClass.isActive=1  AND prodClass.parentProdClassId=" + prodClassId ;
                else
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE prodClass.isActive=1 AND  prodClass.itemProdCatId =" + itemProdCatId + " AND prodClass.parentProdClassId IS NULL";

                cmdSelect.CommandText += " ORDER BY prodClass.prodClassDesc";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProdClassificationTO> list = ConvertDTToList(reader);
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

        //sudhir[12-jan-2018] added connection, transaction.
        public List<TblProdClassificationTO> SelectAllTblProdClassification(SqlConnection conn, SqlTransaction tran, string prodClassType = "")
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {

                if (string.IsNullOrEmpty(prodClassType))
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE prodClass.isActive=1";
                else
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE prodClass.isActive=1 AND prodClassType='" + prodClassType + "'";

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;


                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProdClassificationTO> list = ConvertDTToList(reader);
                return list;
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
        public List<DropDownTO> SelectAllProdClassificationForDropDown(Int32 parentClassId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();

                if (parentClassId == 0)
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE prodClass.isActive=1 AND prodClassType='C' order by prodClass.prodClassDesc";
                else
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE prodClass.isActive=1 AND parentProdClassId = "+ parentClassId + " order by prodClass.prodClassDesc";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> list = new List<DropDownTO>();

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        if (reader["idProdClass"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(reader["idProdClass"].ToString());
                        if (reader["prodClassDesc"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(reader["prodClassDesc"].ToString());
                        if (reader["prodClassType"] != DBNull.Value)
                            dropDownTO.Tag = Convert.ToString(reader["prodClassType"].ToString());
                        if (reader["isConsumable"] != DBNull.Value)
                            dropDownTO.Code = Convert.ToString(reader["isConsumable"].ToString());
                        list.Add(dropDownTO);
                    }
                }
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

        public TblProdClassificationTO SelectTblProdClassification(Int32 idProdClass)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idProdClass = " + idProdClass +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProdClassificationTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1)
                    return list[0];

                return null;
            }
            catch(Exception ex)
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
        ////Reshma
        public TblProdClassificationTO SelectTblProdClassificationTOV2(Int32 idProdClass, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                //conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idProdClass = " + idProdClass + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProdClassificationTO> list = ConvertDTToList(reader);
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
                //conn.Close();
                cmdSelect.Dispose();
            }
        }
        public List<TblProdClassificationTO> SelectAllProdClassificationListyByItemProdCatgE(Constants.ItemProdCategoryE itemProdCategoryE)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();

                cmdSelect.CommandText = SqlSelectQuery() + " WHERE prodClass.isActive=1 AND prodClass.itemProdCatId=" + (int)itemProdCategoryE;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProdClassificationTO> list = ConvertDTToList(reader);
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

        public List<TblProdClassificationTO> checkSubGroupAlreadyExists(TblProdClassificationTO tblProdClassificationTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();

                cmdSelect.CommandText = SqlSelectQuery() + " WHERE prodClass.isActive=1 AND prodClass.parentProdClassId = " + tblProdClassificationTO.ParentProdClassId + " AND prodClass.itemProdCatId=" + tblProdClassificationTO.ItemProdCatId + " AND prodClass.prodClassDesc = '" + tblProdClassificationTO.ProdClassDesc + "' AND prodClass.prodClassType = '" + tblProdClassificationTO.ProdClassType + "'";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProdClassificationTO> list = ConvertDTToList(reader);
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

        public List<DropDownTO> SelectMaterialListForDropDown(Constants.ItemProdCategoryE itemProdCategoryE)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String sqlQuery = String.Empty;
            SqlDataReader tblMRdr = null;
            try
            {
                conn.Open();

                sqlQuery = SqlSelectQuery() + " WHERE prodClass.isActive=1 AND prodClass.itemProdCatId=" + (int)itemProdCategoryE + " ORDER BY prodClass.prodClassDesc";


                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                tblMRdr = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                if (tblMRdr != null)
                {
                    while (tblMRdr.Read())
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        if (tblMRdr["idProdClass"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(tblMRdr["idProdClass"].ToString());
                        if (tblMRdr["prodClassDesc"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(tblMRdr["prodClassDesc"].ToString());

                        dropDownTOList.Add(dropDownTO);
                    }
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                //Constants.LoggerObj.LogError(1, ex, "Error in Method SelectSalesEngineerListForDropDown at DAO");
                return null;
            }
            finally
            {
                if (tblMRdr != null)
                    tblMRdr.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblProdClassificationTO> ConvertDTToList(SqlDataReader tblProdClassificationTODT)
        {
            List<TblProdClassificationTO> tblProdClassificationTOList = new List<TblProdClassificationTO>();
            if (tblProdClassificationTODT != null)
            {
                while (tblProdClassificationTODT.Read())
                {
                    TblProdClassificationTO tblProdClassificationTONew = new TblProdClassificationTO();
                    if (tblProdClassificationTODT["idProdClass"] != DBNull.Value)
                        tblProdClassificationTONew.IdProdClass = Convert.ToInt32(tblProdClassificationTODT["idProdClass"].ToString());
                    if (tblProdClassificationTODT["parentProdClassId"] != DBNull.Value)
                        tblProdClassificationTONew.ParentProdClassId = Convert.ToInt32(tblProdClassificationTODT["parentProdClassId"].ToString());
                    if (tblProdClassificationTODT["createdBy"] != DBNull.Value)
                        tblProdClassificationTONew.CreatedBy = Convert.ToInt32(tblProdClassificationTODT["createdBy"].ToString());
                    if (tblProdClassificationTODT["updatedBy"] != DBNull.Value)
                        tblProdClassificationTONew.UpdatedBy = Convert.ToInt32(tblProdClassificationTODT["updatedBy"].ToString());
                    if (tblProdClassificationTODT["createdOn"] != DBNull.Value)
                        tblProdClassificationTONew.CreatedOn = Convert.ToDateTime(tblProdClassificationTODT["createdOn"].ToString());
                    if (tblProdClassificationTODT["updatedOn"] != DBNull.Value)
                        tblProdClassificationTONew.UpdatedOn = Convert.ToDateTime(tblProdClassificationTODT["updatedOn"].ToString());
                    if (tblProdClassificationTODT["prodClassType"] != DBNull.Value)
                        tblProdClassificationTONew.ProdClassType = Convert.ToString(tblProdClassificationTODT["prodClassType"].ToString());
                    if (tblProdClassificationTODT["prodClassDesc"] != DBNull.Value)
                        tblProdClassificationTONew.ProdClassDesc = Convert.ToString(tblProdClassificationTODT["prodClassDesc"].ToString());
                    if (tblProdClassificationTODT["remark"] != DBNull.Value)
                        tblProdClassificationTONew.Remark = Convert.ToString(tblProdClassificationTODT["remark"].ToString());
                    if (tblProdClassificationTODT["isActive"] != DBNull.Value)
                        tblProdClassificationTONew.IsActive = Convert.ToInt32(tblProdClassificationTODT["isActive"].ToString());
                    if (tblProdClassificationTODT["displayName"] != DBNull.Value)
                        tblProdClassificationTONew.DisplayName = Convert.ToString(tblProdClassificationTODT["displayName"].ToString());
                    
                    //Sanjay [2018-02-19] Added To Distinguish between regular Product and Scrap
                    if (tblProdClassificationTODT["itemProdCatId"] != DBNull.Value)
                        tblProdClassificationTONew.ItemProdCatId = Convert.ToInt32(tblProdClassificationTODT["itemProdCatId"].ToString());
                    if (tblProdClassificationTODT["itemProdCategory"] != DBNull.Value)
                        tblProdClassificationTONew.ItemProdCategory = Convert.ToString(tblProdClassificationTODT["itemProdCategory"].ToString());
                    if (tblProdClassificationTODT["isSetDefault"] != DBNull.Value)
                        tblProdClassificationTONew.IsSetDefault = Convert.ToInt32(tblProdClassificationTODT["isSetDefault"].ToString());
                    
                    //Priyanka [16-05-2018] Added for tax type id
                    if (tblProdClassificationTODT["codeTypeId"] != DBNull.Value)
                        tblProdClassificationTONew.CodeTypeId = Convert.ToInt32(tblProdClassificationTODT["codeTypeId"].ToString());

                    if (tblProdClassificationTODT["mappedTxnId"] != DBNull.Value)
                        tblProdClassificationTONew.MappedTxnId = Convert.ToString(tblProdClassificationTODT["mappedTxnId"].ToString());

                    if (tblProdClassificationTODT["isConsumable"] != DBNull.Value)
                        tblProdClassificationTONew.IsConsumable = Convert.ToBoolean(tblProdClassificationTODT["isConsumable"].ToString());

                    if (tblProdClassificationTODT["isFixedAsset"] != DBNull.Value)
                        tblProdClassificationTONew.IsFixedAsset = Convert.ToBoolean(tblProdClassificationTODT["isFixedAsset"].ToString());


                    tblProdClassificationTOList.Add(tblProdClassificationTONew);
                }
            }
            return tblProdClassificationTOList;
        }

        //chetan[2020-20-01] added
        public TblProdClassificationTO SelectTblProdClassification(bool isScrapProdItem,Int32 parentProdClassId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            int scrapProdItemId = 0;
            if (isScrapProdItem)
                scrapProdItemId = 1;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE isnull(parentProdClassId,0) = " + parentProdClassId + " and isScrapProdItem="+ scrapProdItemId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProdClassificationTO> list = ConvertDTToList(reader);
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
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public TblProdClassificationTO SelectTblProdClassification(string prodClassDesc, Int32 parentProdClassId, int prodCatId,SqlConnection conn,SqlTransaction tran)
        {
           // String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
           // SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                //conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE prodClassDesc='" + prodClassDesc+ "'";
                if(parentProdClassId>0)
                {
                    cmdSelect.CommandText = cmdSelect.CommandText + " AND isnull(parentProdClassId, 0) = " + parentProdClassId ;
                }
                else if(prodCatId>0)
                {
                    cmdSelect.CommandText = cmdSelect.CommandText + "AND isnull(idItemProdCat, 0) = " + prodCatId;
                }
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProdClassificationTO> list = ConvertDTToList(reader);
                if (list != null && list.Count >0)
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
               // conn.Close();
                cmdSelect.Dispose();
            }
        }

        #endregion

        #region Insertion
        public int InsertTblProdClassification(TblProdClassificationTO tblProdClassificationTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblProdClassificationTO, cmdInsert);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public int InsertTblProdClassification(TblProdClassificationTO tblProdClassificationTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblProdClassificationTO, cmdInsert);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public int ExecuteInsertionCommand(TblProdClassificationTO tblProdClassificationTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblProdClassification]( " + 
                            "  [parentProdClassId]" +
                            " ,[createdBy]" +
                            " ,[updatedBy]" +
                            " ,[createdOn]" +
                            " ,[updatedOn]" +
                            " ,[prodClassType]" +
                            " ,[prodClassDesc]" +
                            " ,[remark]" +
                            " ,[isActive]" +
                            " ,[displayName]" +
                            " ,[itemProdCatId]" +
                            " ,[isSetDefault]" +
                            " ,[codeTypeId] " +                 //Priyanka [16-05-2018]
                           " ,[isConsumable] " +
                           " ,[isFixedAsset] " +
                            " )" +
                " VALUES (" +
                            "  @ParentProdClassId " +
                            " ,@CreatedBy " +
                            " ,@UpdatedBy " +
                            " ,@CreatedOn " +
                            " ,@UpdatedOn " +
                            " ,@ProdClassType " +
                            " ,@ProdClassDesc " +
                            " ,@Remark " +
                            " ,@isActive " +
                            " ,@displayName " +
                            " ,@itemProdCatId " +
                            " ,@IsSetDefault " +
                            " ,@CodeTypeId" +                     //Priyanka [16-05-2018]
                            " ,@isConsumable" +
                            " ,@isFixedAsset" +
                            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdProdClass", System.Data.SqlDbType.Int).Value = tblProdClassificationTO.IdProdClass;
            cmdInsert.Parameters.Add("@ParentProdClassId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProdClassificationTO.ParentProdClassId);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblProdClassificationTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProdClassificationTO.UpdatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblProdClassificationTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblProdClassificationTO.UpdatedOn);
            cmdInsert.Parameters.Add("@ProdClassType", System.Data.SqlDbType.Char).Value = tblProdClassificationTO.ProdClassType;
            cmdInsert.Parameters.Add("@ProdClassDesc", System.Data.SqlDbType.NVarChar).Value = tblProdClassificationTO.ProdClassDesc;
            cmdInsert.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value =Constants.GetSqlDataValueNullForBaseValue(tblProdClassificationTO.Remark);
            cmdInsert.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value = tblProdClassificationTO.IsActive;
            cmdInsert.Parameters.Add("@displayName", System.Data.SqlDbType.NVarChar).Value = tblProdClassificationTO.DisplayName;
            cmdInsert.Parameters.Add("@itemProdCatId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProdClassificationTO.ItemProdCatId);
            cmdInsert.Parameters.Add("@IsSetDefault", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProdClassificationTO.IsSetDefault);
            cmdInsert.Parameters.Add("@CodeTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProdClassificationTO.CodeTypeId);   //Priyanka [16-05-2018]
            cmdInsert.Parameters.Add("@isConsumable", System.Data.SqlDbType.Bit).Value = Constants.GetSqlDataValueNullForBaseValue(tblProdClassificationTO.IsConsumable);   //Priyanka [16-05-2018]
            cmdInsert.Parameters.Add("@isFixedAsset", System.Data.SqlDbType.Bit).Value = Constants.GetSqlDataValueNullForBaseValue(tblProdClassificationTO.IsFixedAsset);   //Priyanka [16-05-2018]

            if (cmdInsert.ExecuteNonQuery()==1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblProdClassificationTO.IdProdClass = Convert.ToInt32(cmdInsert.ExecuteScalar());
               // return tblProdClassificationTO.IdProdClass;
                return 1;
            }

            return 0;
        }

        public List<TblProdClassificationTO> checkCategoryAlreadyExists(Int32 idProdClass, String prodClassType, String prodClassDesc, Int32 parentProdClass , Int32 itemProdCatId, SqlConnection conn = null, SqlTransaction tran = null)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader notifyReader = null;
            try
            {
                if (conn != null)
                {
                    cmdSelect.Connection = conn;
                    cmdSelect.Transaction = tran;
                }
                else
                {
                    String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
                    conn = new SqlConnection(sqlConnStr);
                    cmdSelect.Connection = conn;
                    conn.Open();
                }

                cmdSelect.CommandText = "SELECT * FROM tblProdClassification WHERE prodClassDesc = @ProdClassDesc AND ISNULL(isActive,0) = 1 AND ISNULL(parentProdClassId, 0) = @ParentProdClass  ";

                if(idProdClass > 0)
                {
                    cmdSelect.CommandText += " AND idProdClass != @IdProdClass ";
                }
                if (prodClassType != null && prodClassType !="")
                {
                    cmdSelect.CommandText += "and prodClassType = @ProdClassType ";
                    cmdSelect.CommandText += " AND  ISNULL(itemProdCatId, 0) = @ItemProdCatId ";
                }
                //"select * from tblProductItem where prodClassId = @ProdClassId AND itemName =@ItemName AND isActive =1";
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@IdProdClass", System.Data.SqlDbType.Int).Value = idProdClass;
                cmdSelect.Parameters.Add("@ParentProdClass", System.Data.SqlDbType.Int).Value = parentProdClass;
                cmdSelect.Parameters.Add("@ItemProdCatId", System.Data.SqlDbType.Int).Value = itemProdCatId;
                cmdSelect.Parameters.Add("@ProdClassType", System.Data.SqlDbType.NVarChar).Value = prodClassType;
                cmdSelect.Parameters.Add("@ProdClassDesc", System.Data.SqlDbType.NVarChar).Value = prodClassDesc;

                notifyReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConverDTForCategoryList(notifyReader);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (notifyReader != null) notifyReader.Dispose();
                if (tran == null)
                {
                    conn.Close();
                }
                cmdSelect.Dispose();
            }
        }

        public List<TblProdClassificationTO> checkCategoryAlreadyExistsOld(Int32 idProdClass, String prodClassType, String prodClassDesc, SqlConnection conn = null, SqlTransaction tran = null)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader notifyReader = null;
            try
            {
                if (conn != null)
                {
                    cmdSelect.Connection = conn;
                    cmdSelect.Transaction = tran;
                }
                else
                {
                    String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
                    conn = new SqlConnection(sqlConnStr);
                    cmdSelect.Connection = conn;
                    conn.Open();
                }
                if (idProdClass==0)
                {
                    cmdSelect.CommandText = "select * from tblProdClassification where prodClassType = @ProdClassType AND prodClassDesc = @ProdClassDesc AND isActive = 1";
                }
                else
                {
                    cmdSelect.CommandText = "select * from tblProdClassification where idProdClass != @IdProdClass AND prodClassType = @ProdClassType AND prodClassDesc = @ProdClassDesc AND isActive = 1";

                }
                //"select * from tblProductItem where prodClassId = @ProdClassId AND itemName =@ItemName AND isActive =1";
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@IdProdClass", System.Data.SqlDbType.Int).Value = idProdClass;
                cmdSelect.Parameters.Add("@ProdClassType", System.Data.SqlDbType.NVarChar).Value = prodClassType;
                cmdSelect.Parameters.Add("@ProdClassDesc", System.Data.SqlDbType.NVarChar).Value = prodClassDesc;
              
                notifyReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConverDTForCategoryList(notifyReader);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (notifyReader != null) notifyReader.Dispose();
                if (tran == null)
                {
                    conn.Close();
                }
                cmdSelect.Dispose();
            }
        }
        public List<TblProdClassificationTO> ConverDTForCategoryList(SqlDataReader tblProdClassificationTODT)
        {
            List<TblProdClassificationTO> tblProdClassificationTOList = new List<TblProdClassificationTO>();
            if (tblProdClassificationTODT != null)
            {
                while (tblProdClassificationTODT.Read())
                {

                    TblProdClassificationTO tblProdClassificationTONew = new TblProdClassificationTO();
                    if (tblProdClassificationTODT["idProdClass"] != DBNull.Value)
                        tblProdClassificationTONew.IdProdClass = Convert.ToInt32(tblProdClassificationTODT["idProdClass"].ToString());
                    if (tblProdClassificationTODT["prodClassType"] != DBNull.Value)
                        tblProdClassificationTONew.ProdClassType = Convert.ToString(tblProdClassificationTODT["prodClassType"].ToString());
                    if (tblProdClassificationTODT["prodClassDesc"] != DBNull.Value)
                        tblProdClassificationTONew.ProdClassDesc = Convert.ToString(tblProdClassificationTODT["prodClassDesc"].ToString());
                   

                    tblProdClassificationTOList.Add(tblProdClassificationTONew);
                }
            }
            return tblProdClassificationTOList;
        }

        #endregion

        #region Updation
        public int UpdateTblProdClassification(TblProdClassificationTO tblProdClassificationTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblProdClassificationTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }
        public int SetIsDefaultByClassificationType(TblProdClassificationTO tblProdClassificationTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationSetDefaultCommand(tblProdClassificationTO, cmdUpdate);
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
        public int UpdateTblProdClassification(TblProdClassificationTO tblProdClassificationTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblProdClassificationTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }
        public int ExecuteUpdationSetDefaultCommand(TblProdClassificationTO tblProdClassificationTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblProdClassification] SET " +
                                " [isSetDefault] = null" +
                                 " WHERE isSetDefault = 1 AND ( prodClassType = '" + tblProdClassificationTO.ProdClassType + "'";
            if(tblProdClassificationTO.ProdClassType.Trim().Equals("C"))
            {
                sqlQuery += " OR prodClassType = 'SC' OR prodClassType = 'S')";
            }
            if(tblProdClassificationTO.ProdClassType.Trim().Equals("SC"))
            {
                sqlQuery += " OR prodClassType = 'S')";
            }
            if (tblProdClassificationTO.ProdClassType.Trim().Equals("S"))
            {
                sqlQuery += ")";
            }
                cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IsSetDefault", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProdClassificationTO.IsSetDefault);
            return cmdUpdate.ExecuteNonQuery();
        }
        public int ExecuteUpdationCommand(TblProdClassificationTO tblProdClassificationTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblProdClassification] SET " + 
                                "  [parentProdClassId]= @ParentProdClassId" +
                                " ,[updatedBy]= @UpdatedBy" +
                                " ,[updatedOn]= @UpdatedOn" +
                                " ,[prodClassType]= @ProdClassType" +
                                " ,[prodClassDesc]= @ProdClassDesc" +
                                " ,[remark] = @Remark" +
                                " ,[isActive] = @isActive" +
                                " ,[displayName]=@displayName" +
                                " ,[itemProdCatId] = @itemProdCatId" +
                                " ,[isSetDefault] = @IsSetDefault" +
                                " ,[codeTypeId] = @CodeTypeId" +                  //Priyanka [16-05-2018]
                                 " ,[isConsumable] = @isConsumable" +
                                  " ,[isFixedAsset] = @isFixedAsset" +
                                 " WHERE [idProdClass] = @IdProdClass"; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdProdClass", System.Data.SqlDbType.Int).Value = tblProdClassificationTO.IdProdClass;
            cmdUpdate.Parameters.Add("@ParentProdClassId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProdClassificationTO.ParentProdClassId);
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblProdClassificationTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblProdClassificationTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@ProdClassType", System.Data.SqlDbType.Char).Value = tblProdClassificationTO.ProdClassType;
            cmdUpdate.Parameters.Add("@ProdClassDesc", System.Data.SqlDbType.NVarChar).Value = tblProdClassificationTO.ProdClassDesc;
            cmdUpdate.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblProdClassificationTO.Remark);
            cmdUpdate.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value = tblProdClassificationTO.IsActive;
            cmdUpdate.Parameters.Add("@displayName", System.Data.SqlDbType.NVarChar).Value = tblProdClassificationTO.DisplayName;
            cmdUpdate.Parameters.Add("@itemProdCatId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProdClassificationTO.ItemProdCatId);
            cmdUpdate.Parameters.Add("@IsSetDefault", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProdClassificationTO.IsSetDefault);
            cmdUpdate.Parameters.Add("@CodeTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProdClassificationTO.CodeTypeId);     //Priyanka [16-05-2018]
            cmdUpdate.Parameters.Add("@isConsumable", System.Data.SqlDbType.Bit).Value = Constants.GetSqlDataValueNullForBaseValue(tblProdClassificationTO.IsConsumable);     //Priyanka [16-05-2018]
            cmdUpdate.Parameters.Add("@isFixedAsset", System.Data.SqlDbType.Bit).Value = Constants.GetSqlDataValueNullForBaseValue(tblProdClassificationTO.IsFixedAsset);     //Priyanka [16-05-2018]

            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public int DeleteTblProdClassification(Int32 idProdClass)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idProdClass, cmdDelete);
            }
            catch(Exception ex)
            {
                
                
                return 0;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public int DeleteTblProdClassification(Int32 idProdClass, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idProdClass, cmdDelete);
            }
            catch(Exception ex)
            {
                
                
                return 0;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public int ExecuteDeletionCommand(Int32 idProdClass, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblProdClassification] " +
            " WHERE idProdClass = " + idProdClass +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idProdClass", System.Data.SqlDbType.Int).Value = tblProdClassificationTO.IdProdClass;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

        public List<DropDownTO> getProdClassIdsByItemProdCat(Int32 itemProdCatId, string prodClassType = "S")
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select idProdClass,prodClassDesc from tblProdClassification where itemProdCatId = " + itemProdCatId + " and prodClassType = '" + prodClassType + "'";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> list = new List<DropDownTO>();
                while (reader.Read())
                {
                    DropDownTO dropDownTO = new DropDownTO();
                    if (reader["idProdClass"] != DBNull.Value)
                        dropDownTO.Value = Convert.ToInt32(reader["idProdClass"].ToString());

                    if (reader["prodClassDesc"] != DBNull.Value)
                        dropDownTO.Text = Convert.ToString(reader["prodClassDesc"].ToString());

                    list.Add(dropDownTO);
                }
                if (reader != null)
                    reader.Dispose();
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


    }
}
