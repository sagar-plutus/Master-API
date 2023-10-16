using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ODLMWebAPI.Models;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.StaticStuff;
using simpliMASTERSAPI.DAL.Interfaces;

namespace ODLMWebAPI.DAL
{
    public class TblModelDAO : ITblModelDAO
    {
        private readonly IConnectionString _iConnectionString;

        public TblModelDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblModel]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblModelTO> SelectAllTblModel()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                
                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblModelTO> list = ConvertDTToList(rdr);
                if (list != null && list.Count > 0)
                    return list;
                else return null;
            }
            catch(Exception ex)
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

        public TblModelTO SelectTblModel(int idModel)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idModel = " + idModel +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;                
                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblModelTO> list = ConvertDTToList(rdr);
                if (list != null && list.Count > 0)
                    return list[0];
                else return null;

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

        public  List<TblModelTO> SelectAllTblModel(SqlConnection conn, SqlTransaction tran)
        {
            SqlDataReader rdr = null;
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblModelTO> list = ConvertDTToList(rdr);
                if (list != null && list.Count > 0)
                    return list;
                else return null;
            }
            catch(Exception ex)
            {
                
                
                return null;
            }
            finally
            {
                if (rdr != null) rdr.Dispose();
                cmdSelect.Dispose();
            }
        }

        public List<TblModelTO> SelectAllTblModel(int prodItemId, SqlConnection conn, SqlTransaction tran)
        {
            SqlDataReader rdr = null;
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE prodItemId = "+prodItemId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblModelTO> list = ConvertDTToList(rdr);
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
                if (rdr != null) rdr.Dispose();
                cmdSelect.Dispose();
            }
        }
        public List<TblModelTO> ConvertDTToList(SqlDataReader tblModelTODT)
        {
            List<TblModelTO> tblModelTOList = new List<TblModelTO>();
            if (tblModelTODT != null)
            {
                while (tblModelTODT.Read())
                {
                    TblModelTO tblModelTONew = new TblModelTO();
                    if (tblModelTODT["createdOn"] != DBNull.Value)
                        tblModelTONew.CreatedOn = Convert.ToDateTime(tblModelTODT["createdOn"].ToString());
                    if (tblModelTODT["finalizedOn"] != DBNull.Value)
                        tblModelTONew.FinalizedOn = Convert.ToDateTime(tblModelTODT["finalizedOn"].ToString());
                    if (tblModelTODT["idModel"] != DBNull.Value)
                        tblModelTONew.IdModel = Convert.ToInt32(tblModelTODT["idModel"].ToString());
                    if (tblModelTODT["prodItemId"] != DBNull.Value)
                        tblModelTONew.ProdItemId = Convert.ToInt32(tblModelTODT["prodItemId"].ToString());
                    if (tblModelTODT["versionNo"] != DBNull.Value)
                        tblModelTONew.VersionNo = Convert.ToInt32(tblModelTODT["versionNo"].ToString());
                    if (tblModelTODT["revisionNo"] != DBNull.Value)
                        tblModelTONew.RevisionNo = Convert.ToInt32(tblModelTODT["revisionNo"].ToString());
                    if (tblModelTODT["createdBy"] != DBNull.Value)
                        tblModelTONew.CreatedBy = Convert.ToInt32(tblModelTODT["createdBy"].ToString());
                    if (tblModelTODT["finalizedBy"] != DBNull.Value)
                        tblModelTONew.FinalizedBy = Convert.ToInt32(tblModelTODT["finalizedBy"].ToString());
                    tblModelTOList.Add(tblModelTONew);
                }
            }


            return tblModelTOList;
        }

        #endregion

        #region Insertion
        public int InsertTblModel(TblModelTO tblModelTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblModelTO, cmdInsert);
            }
            catch(Exception ex)
            {
                
                
                return 0;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public  int InsertTblModel(TblModelTO tblModelTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblModelTO, cmdInsert);
            }
            catch(Exception ex)
            {             
                
                return 0;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public  int ExecuteInsertionCommand(TblModelTO tblModelTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblModel]( " + 
            "  [createdOn]" +
            " ,[finalizedOn]" +
            " ,[prodItemId]" +
            " ,[versionNo]" +
            " ,[revisionNo]" +
            " ,[createdBy]" +
            " ,[finalizedBy]" +
            " )" +
" VALUES (" +
            "  @CreatedOn " +
            " ,@FinalizedOn " +
            " ,@ProdItemId " +
            " ,@VersionNo " +
            " ,(select count(idModel) from  tblModel where prodItemId = @ProdItemId) " +
            " ,@CreatedBy " +
            " ,@FinalizedBy " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblModelTO.CreatedOn);
            cmdInsert.Parameters.Add("@FinalizedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblModelTO.FinalizedOn);
            cmdInsert.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblModelTO.ProdItemId);
            cmdInsert.Parameters.Add("@RevisionNo", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblModelTO.RevisionNo);
            cmdInsert.Parameters.Add("@VersionNo", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblModelTO.VersionNo);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblModelTO.CreatedBy);
            cmdInsert.Parameters.Add("@FinalizedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblModelTO.FinalizedBy);


            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblModelTO.IdModel = Convert.ToInt32(cmdInsert.ExecuteScalar());

                return 1;
            }
            return 0;
        }
        #endregion
        
        #region Updation
        public  int UpdateTblModel(TblModelTO tblModelTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblModelTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                
                
                return 0;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public  int UpdateTblModel(TblModelTO tblModelTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblModelTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                
                
                return 0;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public  int ExecuteUpdationCommand(TblModelTO tblModelTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblModel] SET " + 
            "  [createdOn] = @CreatedOn" +
            " ,[finalizedOn]= @FinalizedOn" +
            " ,[prodItemId]= @ProdItemId" +
            " ,[versionNo]= @VersionNo" +
            " ,[revisionNo]= @RevisionNo" +
            " ,[createdBy]= @CreatedBy" +
            " ,[finalizedBy] = @FinalizedBy" +
            " WHERE 1 = 1 "; 

            cmdUpdate.CommandText = sqlQuery + " AND idModel = @IdModel";
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value =Constants.GetSqlDataValueNullForBaseValue(tblModelTO.CreatedOn);
            cmdUpdate.Parameters.Add("@FinalizedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblModelTO.FinalizedOn);
            cmdUpdate.Parameters.Add("@IdModel", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblModelTO.IdModel);
            cmdUpdate.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblModelTO.ProdItemId);
            cmdUpdate.Parameters.Add("@VersionNo", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblModelTO.VersionNo);
            cmdUpdate.Parameters.Add("@RevisionNo", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblModelTO.RevisionNo);
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblModelTO.CreatedBy);
            cmdUpdate.Parameters.Add("@FinalizedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblModelTO.FinalizedBy);
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblModel(int idModel)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idModel, cmdDelete);
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

        public  int DeleteTblModel(int idModel, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idModel, cmdDelete);
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

        public  int ExecuteDeletionCommand(int idModel, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblModel] " +
            " WHERE idModel = " + idModel +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idModel", System.Data.SqlDbType.Int).Value = tblModelTO.IdModel;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
