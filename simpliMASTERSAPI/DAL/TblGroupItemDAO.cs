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
    public class TblGroupItemDAO : ITblGroupItemDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblGroupItemDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " select tblGroupItem.* " +
                ", case when tblGroupItem.prodItemId > 0 then (proadClass.displayName  + '/' + prodItem.itemName) else tblMaterial.materialSubType + '-' + dimProdCat.prodCateDesc + '-' + dimProdSpec.prodSpecDesc END as prodItemDesc " +
                " from tblGroupItem tblGroupItem" +
                " LEFT JOIN  tblProductItem prodItem on prodItem.idProdItem=tblGroupItem.prodItemId " +
                " LEFT JOIN tblProdClassification proadClass on prodItem.prodClassId = proadClass.idProdClass " +
                " LEFT JOIN tblMaterial tblMaterial on tblMaterial.idMaterial = tblGroupItem.materialId " +
                " LEFT JOIN dimProdCat dimProdCat on dimProdCat.idProdCat = tblGroupItem.prodCatId " + 
                " LEFT JOIN dimProdSpec dimProdSpec on dimProdSpec.idProdSpec = tblGroupItem.prodSpecId"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblGroupItemTO> SelectAllTblGroupItem()
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
                List<TblGroupItemTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public TblGroupItemTO SelectTblGroupItem(Int32 idGroupItem)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idGroupItem = " + idGroupItem +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;


                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblGroupItemTO> list = ConvertDTToList(sqlReader);
                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public TblGroupItemTO SelectAllTblGroupItem(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblGroupItemTO> list = ConvertDTToList(sqlReader);
                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
            }
        }

        public List<TblGroupItemTO> ConvertDTToList(SqlDataReader tblGroupItemTODT)
        {
            List<TblGroupItemTO> tblGroupItemTOList = new List<TblGroupItemTO>();
            if (tblGroupItemTODT != null)
            {
                while (tblGroupItemTODT.Read())
                {
                    TblGroupItemTO tblGroupItemTONew = new TblGroupItemTO();
                    if (tblGroupItemTODT["idGroupItem"] != DBNull.Value)
                        tblGroupItemTONew.IdGroupItem = Convert.ToInt32(tblGroupItemTODT["idGroupItem"].ToString());
                    if (tblGroupItemTODT["groupId"] != DBNull.Value)
                        tblGroupItemTONew.GroupId = Convert.ToInt32(tblGroupItemTODT["groupId"].ToString());
                    if (tblGroupItemTODT["prodItemId"] != DBNull.Value)
                        tblGroupItemTONew.ProdItemId = Convert.ToInt32(tblGroupItemTODT["prodItemId"].ToString());
                    if (tblGroupItemTODT["createdBy"] != DBNull.Value)
                        tblGroupItemTONew.CreatedBy = Convert.ToInt32(tblGroupItemTODT["createdBy"].ToString());
                    if (tblGroupItemTODT["updatedBy"] != DBNull.Value)
                        tblGroupItemTONew.UpdatedBy = Convert.ToInt32(tblGroupItemTODT["updatedBy"].ToString());
                    if (tblGroupItemTODT["isActive"] != DBNull.Value)
                        tblGroupItemTONew.IsActive = Convert.ToInt32(tblGroupItemTODT["isActive"].ToString());
                    if (tblGroupItemTODT["createdOn"] != DBNull.Value)
                        tblGroupItemTONew.CreatedOn = Convert.ToDateTime(tblGroupItemTODT["createdOn"].ToString());
                    if (tblGroupItemTODT["updatedOn"] != DBNull.Value)
                        tblGroupItemTONew.UpdatedOn = Convert.ToDateTime(tblGroupItemTODT["updatedOn"].ToString());
                    if (tblGroupItemTODT["prodItemDesc"] != DBNull.Value)
                        tblGroupItemTONew.ProdItemDesc = Convert.ToString(tblGroupItemTODT["prodItemDesc"].ToString());
                    tblGroupItemTOList.Add(tblGroupItemTONew);
                }
            }
            return tblGroupItemTOList;
        }


        public TblGroupItemTO SelectTblGroupItemDetails(Int32 prodItemId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE tblGroupItem.isActive=1  AND ISNULL(prodItemId,0)=" + prodItemId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblGroupItemTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1) return list[0];
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




        public TblGroupItemTO SelectTblGroupItemDetails(Int32 prodItemId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE tblGroupItem.isActive=1  AND ISNULL(prodItemId,0)=" + prodItemId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblGroupItemTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1) return list[0];
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


        public List<TblGroupItemTO> SelectAllTblGroupItemDtlsList(Int32 groupId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                if (groupId == 0)
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE tblGroupItem.isActive=1";
                else
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE tblGroupItem.isActive=1 AND groupId=" + groupId;

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblGroupItemTO> list = ConvertDTToList(reader);
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

        #endregion

        #region Insertion
        public int InsertTblGroupItem(TblGroupItemTO tblGroupItemTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblGroupItemTO, cmdInsert);
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

        public int InsertTblGroupItem(TblGroupItemTO tblGroupItemTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblGroupItemTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblGroupItemTO tblGroupItemTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblGroupItem]( " + 
            "  [groupId]" +
            " ,[prodItemId]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[isActive]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " )" +
" VALUES (" +
            "  @GroupId " +
            " ,@ProdItemId " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@IsActive " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@GroupId", System.Data.SqlDbType.Int).Value = tblGroupItemTO.GroupId;
            cmdInsert.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value = tblGroupItemTO.ProdItemId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblGroupItemTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblGroupItemTO.UpdatedBy);
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblGroupItemTO.IsActive;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblGroupItemTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblGroupItemTO.UpdatedOn);
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblGroupItemTO.IdGroupItem = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public int UpdateTblGroupItem(TblGroupItemTO tblGroupItemTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblGroupItemTO, cmdUpdate);
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

        public int UpdateTblGroupItem(TblGroupItemTO tblGroupItemTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblGroupItemTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(TblGroupItemTO tblGroupItemTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblGroupItem] SET " +

            "  [groupId]= @GroupId" +
            " ,[prodItemId]= @ProdItemId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[isActive]= @IsActive" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn] = @UpdatedOn" +
            " WHERE  [idGroupItem] = @IdGroupItem";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdGroupItem", System.Data.SqlDbType.Int).Value = tblGroupItemTO.IdGroupItem;
            cmdUpdate.Parameters.Add("@GroupId", System.Data.SqlDbType.Int).Value = tblGroupItemTO.GroupId;
            cmdUpdate.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value = tblGroupItemTO.ProdItemId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblGroupItemTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblGroupItemTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblGroupItemTO.IsActive;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblGroupItemTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblGroupItemTO.UpdatedOn;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public int DeleteTblGroupItem(Int32 idGroupItem)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idGroupItem, cmdDelete);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public int DeleteTblGroupItem(Int32 idGroupItem, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idGroupItem, cmdDelete);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public int ExecuteDeletionCommand(Int32 idGroupItem, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblGroupItem] " +
            " WHERE idGroupItem = " + idGroupItem +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idGroupItem", System.Data.SqlDbType.Int).Value = tblGroupItemTO.IdGroupItem;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
