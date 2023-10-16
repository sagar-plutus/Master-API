using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.StaticStuff;
namespace ODLMWebAPI.DAL
{
    public class TblItemMasterFieldsDAO : ITblItemMasterFieldsDAO
    {
        #region Methods
        private readonly IConnectionString _iConnectionString;
        public TblItemMasterFieldsDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblItemMasterFields]  where isActive = 1"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblItemMasterFieldsTO> SelectAllTblItemMasterFields()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + "order by sequanceNo asc";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblItemMasterFieldsTO> list = ConvertDTToList(reader);
                if (list != null && list.Count > 0)
                    return list;
                else
                    return null;
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

        public TblItemMasterFieldsTO SelectTblItemMasterFields(Int32 idTblItemMasterFields)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;

            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idTblItemMasterFields = " + idTblItemMasterFields +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idTblItemMasterFields", System.Data.SqlDbType.Int).Value = tblItemMasterFieldsTO.IdTblItemMasterFields;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblItemMasterFieldsTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1)
                    return list[0];
                else
                    return null;
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

        public  List<TblItemMasterFieldsTO> SelectAllTblItemMasterFields(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblItemMasterFieldsTO> list = ConvertDTToList(reader);
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

        #endregion


        public List<TblItemMasterFieldsTO> ConvertDTToList(SqlDataReader tblItemMasterFieldsTODT)
        {
            List<TblItemMasterFieldsTO> tblItemMasterFieldsTOList = new List<TblItemMasterFieldsTO>();
            if (tblItemMasterFieldsTODT != null)
            {
                while (tblItemMasterFieldsTODT.Read())
                {
                    TblItemMasterFieldsTO tblItemMasterFieldsTONew = new TblItemMasterFieldsTO();
                    if (tblItemMasterFieldsTODT["idTblItemMasterFields"] != DBNull.Value)
                        tblItemMasterFieldsTONew.IdTblItemMasterFields = Convert.ToInt32(tblItemMasterFieldsTODT["idTblItemMasterFields"].ToString());
                    if (tblItemMasterFieldsTODT["isActive"] != DBNull.Value)
                        tblItemMasterFieldsTONew.IsActive = Convert.ToInt32(tblItemMasterFieldsTODT["isActive"].ToString());
                    if (tblItemMasterFieldsTODT["isMandatory"] != DBNull.Value)
                        tblItemMasterFieldsTONew.IsMandatory = Convert.ToInt32(tblItemMasterFieldsTODT["isMandatory"].ToString());
                    if (tblItemMasterFieldsTODT["isDisabled"] != DBNull.Value)
                        tblItemMasterFieldsTONew.IsDisabled = Convert.ToInt32(tblItemMasterFieldsTODT["isDisabled"].ToString());
                    if (tblItemMasterFieldsTODT["createdBy"] != DBNull.Value)
                        tblItemMasterFieldsTONew.CreatedBy = Convert.ToInt32(tblItemMasterFieldsTODT["createdBy"].ToString());
                    if (tblItemMasterFieldsTODT["updatedBy"] != DBNull.Value)
                        tblItemMasterFieldsTONew.UpdatedBy = Convert.ToInt32(tblItemMasterFieldsTODT["updatedBy"].ToString());
                    if (tblItemMasterFieldsTODT["createdOn"] != DBNull.Value)
                        tblItemMasterFieldsTONew.CreatedOn = Convert.ToDateTime(tblItemMasterFieldsTODT["createdOn"].ToString());
                    if (tblItemMasterFieldsTODT["updatedOn"] != DBNull.Value)
                        tblItemMasterFieldsTONew.UpdatedOn = Convert.ToDateTime(tblItemMasterFieldsTODT["updatedOn"].ToString());
                    if (tblItemMasterFieldsTODT["name"] != DBNull.Value)
                        tblItemMasterFieldsTONew.Name = Convert.ToString(tblItemMasterFieldsTODT["name"].ToString());
                    if (tblItemMasterFieldsTODT["descField"] != DBNull.Value)
                        tblItemMasterFieldsTONew.DescField = Convert.ToString(tblItemMasterFieldsTODT["descField"].ToString());
                    tblItemMasterFieldsTOList.Add(tblItemMasterFieldsTONew);
                }
            }
            return tblItemMasterFieldsTOList;
        }



        #region Insertion
        public int InsertTblItemMasterFields(TblItemMasterFieldsTO tblItemMasterFieldsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblItemMasterFieldsTO, cmdInsert);
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

        public  int InsertTblItemMasterFields(TblItemMasterFieldsTO tblItemMasterFieldsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblItemMasterFieldsTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblItemMasterFieldsTO tblItemMasterFieldsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblItemMasterFields]( " + 
            "  [idTblItemMasterFields]" +
            " ,[isActive]" +
            " ,[isMandatory]" +
            " ,[isDisabled]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[name]" +
            " ,[descField]" +
            " )" +
" VALUES (" +
            "  @IdTblItemMasterFields " +
            " ,@IsActive " +
            " ,@IsMandatory " +
            " ,@IsDisabled " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@Name " +
            " ,@DescField " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdTblItemMasterFields", System.Data.SqlDbType.Int).Value = tblItemMasterFieldsTO.IdTblItemMasterFields;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblItemMasterFieldsTO.IsActive;
            cmdInsert.Parameters.Add("@IsMandatory", System.Data.SqlDbType.Int).Value = tblItemMasterFieldsTO.IsMandatory;
            cmdInsert.Parameters.Add("@IsDisabled", System.Data.SqlDbType.Int).Value = tblItemMasterFieldsTO.IsDisabled;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblItemMasterFieldsTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblItemMasterFieldsTO.UpdatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblItemMasterFieldsTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblItemMasterFieldsTO.UpdatedOn;
            cmdInsert.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar).Value = tblItemMasterFieldsTO.Name;
            cmdInsert.Parameters.Add("@DescField", System.Data.SqlDbType.NVarChar).Value = tblItemMasterFieldsTO.DescField;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public  int UpdateTblItemMasterFields(TblItemMasterFieldsTO tblItemMasterFieldsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblItemMasterFieldsTO, cmdUpdate);
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

        public  int UpdateTblItemMasterFields(TblItemMasterFieldsTO tblItemMasterFieldsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblItemMasterFieldsTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblItemMasterFieldsTO tblItemMasterFieldsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblItemMasterFields] SET " + 
            "  [idTblItemMasterFields] = @IdTblItemMasterFields" +
            " ,[isActive]= @IsActive" +
            " ,[isMandatory]= @IsMandatory" +
            " ,[isDisabled]= @IsDisabled" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[name]= @Name" +
            " ,[descField] = @DescField" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdTblItemMasterFields", System.Data.SqlDbType.Int).Value = tblItemMasterFieldsTO.IdTblItemMasterFields;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblItemMasterFieldsTO.IsActive;
            cmdUpdate.Parameters.Add("@IsMandatory", System.Data.SqlDbType.Int).Value = tblItemMasterFieldsTO.IsMandatory;
            cmdUpdate.Parameters.Add("@IsDisabled", System.Data.SqlDbType.Int).Value = tblItemMasterFieldsTO.IsDisabled;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblItemMasterFieldsTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblItemMasterFieldsTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblItemMasterFieldsTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblItemMasterFieldsTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar).Value = tblItemMasterFieldsTO.Name;
            cmdUpdate.Parameters.Add("@DescField", System.Data.SqlDbType.NVarChar).Value = tblItemMasterFieldsTO.DescField;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblItemMasterFields(Int32 idTblItemMasterFields)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idTblItemMasterFields, cmdDelete);
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

        public  int DeleteTblItemMasterFields(Int32 idTblItemMasterFields, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idTblItemMasterFields, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int32 idTblItemMasterFields, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblItemMasterFields] " +
            " WHERE idTblItemMasterFields = " + idTblItemMasterFields +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idTblItemMasterFields", System.Data.SqlDbType.Int).Value = tblItemMasterFieldsTO.IdTblItemMasterFields;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
