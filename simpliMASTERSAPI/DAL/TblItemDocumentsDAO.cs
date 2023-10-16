using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.TO;
using simpliMASTERSAPI.DAL.Interfaces;
using ODLMWebAPI.Models;

namespace ODLMWebAPI.DAL
{
    public class TblItemDocumentsDAO : ITblItemDocumentsDAO
    {

        private readonly IConnectionString _iConnectionString;
        public TblItemDocumentsDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }

        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " select documentDesc,isShowImagesForItem,tblItemDocuments.*,name,path from tblItemDocuments tblItemDocuments"
                                       + " left join tblDocumentDetails tblDocumentDetails"
                                       + " on tblDocumentDetails.idDocument = tblItemDocuments.documentId"
                                       + " left join tblDocumentType tblDocumentType"
                                       + " on tblDocumentType.idDocumentType = tblItemDocuments.documentTypeId ";


            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblItemDocumentsTO> SelectAllTblItemDocuments()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String sqlQuery = String.Empty;
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idItemDocuments", System.Data.SqlDbType.Int).Value = tblItemDocumentsTO.IdItemDocuments;
                SqlDataReader dimMstDeptTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblItemDocumentsTO> list = ConvertDTToList(dimMstDeptTODT);
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

        public TblItemDocumentsTO SelectTblItemDocuments(int idItemDocuments)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idItemDocuments = " + idItemDocuments +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader dimMstDeptTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblItemDocumentsTO> list = ConvertDTToList(dimMstDeptTODT);
                return list[0];
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


        public List<TblItemDocumentsTO> SelectTblItemDocumentsByItemId(int prodItemId, Boolean isShowImagesOnly = false)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText =SqlSelectQuery()  + " where tblItemDocuments.isActive =1 and itemId =" + prodItemId + " ";
                if (isShowImagesOnly == true)
                {
                    cmdSelect.CommandText += " and tblDocumentType.isShowImagesForItem = 1 ";
                }

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader dimMstDeptTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblItemDocumentsTO> list = ConvertDTToList(dimMstDeptTODT);
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



        public List<TblItemDocumentsTO> SelectAllTblItemDocuments(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblItemDocumentsTO> list = ConvertDTToList(reader);

                return list;
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


        public List<DropDownTO> GetDocumentTypeList() {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String sqlQuery = String.Empty;
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select idDocumentType,name,isShowImagesForItem from tblDocumentType where isActive = 1 ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idItemDocuments", System.Data.SqlDbType.Int).Value = tblItemDocumentsTO.IdItemDocuments;
                SqlDataReader tblItemDocumentsTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> list = new List<DropDownTO>();
                if (tblItemDocumentsTODT != null)
                {
                    while (tblItemDocumentsTODT.Read())
                    {
                        DropDownTO TO = new DropDownTO();
                        TblItemDocumentsTO tblItemDocumentsTONew = new TblItemDocumentsTO();
                        if (tblItemDocumentsTODT["idDocumentType"] != DBNull.Value)
                            TO.Value = Convert.ToInt32(tblItemDocumentsTODT["idDocumentType"].ToString());
                        if (tblItemDocumentsTODT["name"] != DBNull.Value)
                            TO.Text = tblItemDocumentsTODT["name"].ToString();
                        if (tblItemDocumentsTODT["isShowImagesForItem"] != DBNull.Value)
                            TO.Tag = tblItemDocumentsTODT["isShowImagesForItem"].ToString();

                        list.Add(TO);
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
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        #endregion

        #region Insertion
        public int InsertTblItemDocuments(TblItemDocumentsTO tblItemDocumentsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblItemDocumentsTO, cmdInsert);
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

        public int InsertTblItemDocuments(TblItemDocumentsTO tblItemDocumentsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblItemDocumentsTO, cmdInsert);
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

        public static int ExecuteInsertionCommand(TblItemDocumentsTO tblItemDocumentsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblItemDocuments]( " + 
            "  [createdOn]" +
            " ,[updatedOn]" +
            " ,[itemId]" +
            " ,[documentId]" +
            " ,[documentTypeId]" +
            " ,[isActive]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " )" +
" VALUES (" +
            "  @CreatedOn " +
            " ,@UpdatedOn " +
            " ,@ItemId " +
            " ,@DocumentId " +
            " ,@DocumentTypeId " +
            " ,@IsActive " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblItemDocumentsTO.CreatedOn);
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblItemDocumentsTO.UpdatedOn);
            cmdInsert.Parameters.Add("@ItemId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblItemDocumentsTO.ItemId);
            cmdInsert.Parameters.Add("@DocumentId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblItemDocumentsTO.DocumentId);
            cmdInsert.Parameters.Add("@DocumentTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblItemDocumentsTO.DocumentTypeId);
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblItemDocumentsTO.IsActive);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblItemDocumentsTO.CreatedBy);
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value =Constants.GetSqlDataValueNullForBaseValue(tblItemDocumentsTO.UpdatedBy);
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public  int UpdateTblItemDocuments(TblItemDocumentsTO tblItemDocumentsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblItemDocumentsTO, cmdUpdate);
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

        public  int UpdateTblItemDocuments(TblItemDocumentsTO tblItemDocumentsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblItemDocumentsTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblItemDocumentsTO tblItemDocumentsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblItemDocuments] SET " + 
            "  [createdOn] = @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[itemId]= @ItemId" +
            " ,[documentId]= @DocumentId" +
            " ,[documentTypeId]= @DocumentTypeId" +
            " ,[isActive]= @IsActive" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy] = @UpdatedBy" +
            " WHERE 1 = 1 and idItemDocuments = @IdItemDocuments"; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblItemDocumentsTO.CreatedOn);
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblItemDocumentsTO.UpdatedOn);
            cmdUpdate.Parameters.Add("@IdItemDocuments", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblItemDocumentsTO.IdItemDocuments);
            cmdUpdate.Parameters.Add("@ItemId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblItemDocumentsTO.ItemId);
            cmdUpdate.Parameters.Add("@DocumentId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblItemDocumentsTO.DocumentId);
            cmdUpdate.Parameters.Add("@DocumentTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblItemDocumentsTO.DocumentTypeId);
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblItemDocumentsTO.IsActive);
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblItemDocumentsTO.CreatedBy);
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value =Constants.GetSqlDataValueNullForBaseValue(tblItemDocumentsTO.UpdatedBy);
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblItemDocuments(int idItemDocuments)
        {
              String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idItemDocuments, cmdDelete);
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

        public  int DeleteTblItemDocuments(int idItemDocuments, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idItemDocuments, cmdDelete);
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

        public  int ExecuteDeletionCommand(int idItemDocuments, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblItemDocuments] " +
            " WHERE idItemDocuments = " + idItemDocuments +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idItemDocuments", System.Data.SqlDbType.Int).Value = tblItemDocumentsTO.IdItemDocuments;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion



        public  List<TblItemDocumentsTO> ConvertDTToList(SqlDataReader tblItemDocumentsTODT)
        {
            List<TblItemDocumentsTO> tblItemDocumentsTOList = new List<TblItemDocumentsTO>();
            if (tblItemDocumentsTODT != null)
            {
                while (tblItemDocumentsTODT.Read())
                {
                    TblItemDocumentsTO tblItemDocumentsTONew = new TblItemDocumentsTO();
                    if (tblItemDocumentsTODT["createdOn"] != DBNull.Value)
                        tblItemDocumentsTONew.CreatedOn = Convert.ToDateTime(tblItemDocumentsTODT["createdOn"].ToString());
                    if (tblItemDocumentsTODT["createdOn"] != DBNull.Value)
                        tblItemDocumentsTONew.CreatedOnStr = Convert.ToDateTime(tblItemDocumentsTODT["createdOn"]).ToString("dd - MM - yyyy");
                    if (tblItemDocumentsTODT["updatedOn"] != DBNull.Value)
                        tblItemDocumentsTONew.UpdatedOn = Convert.ToDateTime(tblItemDocumentsTODT["updatedOn"].ToString());
                    if (tblItemDocumentsTODT["idItemDocuments"] != DBNull.Value)
                        tblItemDocumentsTONew.IdItemDocuments = Convert.ToInt32(tblItemDocumentsTODT["idItemDocuments"].ToString());
                    if (tblItemDocumentsTODT["itemId"] != DBNull.Value)
                        tblItemDocumentsTONew.ItemId = Convert.ToInt32(tblItemDocumentsTODT["itemId"].ToString());
                    if (tblItemDocumentsTODT["documentId"] != DBNull.Value)
                        tblItemDocumentsTONew.DocumentId = Convert.ToInt32(tblItemDocumentsTODT["documentId"].ToString());
                    if (tblItemDocumentsTODT["documentTypeId"] != DBNull.Value)
                        tblItemDocumentsTONew.DocumentTypeId = Convert.ToInt32(tblItemDocumentsTODT["documentTypeId"].ToString());
                    if (tblItemDocumentsTODT["isActive"] != DBNull.Value)
                        tblItemDocumentsTONew.IsActive = Convert.ToInt32(tblItemDocumentsTODT["isActive"].ToString());
                    if (tblItemDocumentsTODT["isShowImagesForItem"] != DBNull.Value)
                        tblItemDocumentsTONew.IsShowImagesForItem = Convert.ToInt32(tblItemDocumentsTODT["isShowImagesForItem"].ToString());
                    if (tblItemDocumentsTODT["createdBy"] != DBNull.Value)
                        tblItemDocumentsTONew.CreatedBy = Convert.ToInt32(tblItemDocumentsTODT["createdBy"].ToString());
                    if (tblItemDocumentsTODT["updatedBy"] != DBNull.Value)
                        tblItemDocumentsTONew.UpdatedBy = Convert.ToInt32(tblItemDocumentsTODT["updatedBy"].ToString());
                    if (tblItemDocumentsTODT["path"] != DBNull.Value)
                        tblItemDocumentsTONew.Path = tblItemDocumentsTODT["path"].ToString();
                    if (tblItemDocumentsTODT["name"] != DBNull.Value)
                        tblItemDocumentsTONew.DocTypeName = tblItemDocumentsTODT["name"].ToString();
                    if (tblItemDocumentsTODT["documentDesc"] != DBNull.Value)
                        tblItemDocumentsTONew.DocDesc =tblItemDocumentsTODT["documentDesc"].ToString();
                    tblItemDocumentsTOList.Add(tblItemDocumentsTONew);
                }
            }
            return tblItemDocumentsTOList;
        }

    }
}
