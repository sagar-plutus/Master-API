using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.StaticStuff;
namespace ODLMWebAPI.DAL
{
    public class DimOrgTypeDAO : IDimOrgTypeDAO
    {
        private readonly IConnectionString _iConnectionString;
        public DimOrgTypeDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [dimOrgType]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<DimOrgTypeTO> SelectAllDimOrgType()
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
                List<DimOrgTypeTO> list = ConvertDTToList(rdr);
                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                if(rdr!=null) rdr.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        //Priyanka [20-09-2019]
        public DimOrgTypeTO SelectAllDimOrgType(Int32 idOrgType)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idOrgType = " + idOrgType + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimOrgTypeTO> list = ConvertDTToList(rdr);
                if(list != null && list.Count == 1)
                {
                    return list[0];
                }
                return null;
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
        public DimOrgTypeTO SelectDimOrgType(Int32 idOrgType, SqlConnection conn, SqlTransaction tran)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idOrgType = " + idOrgType + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimOrgTypeTO> list = ConvertDTToList(rdr);
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
                if (rdr != null) rdr.Dispose(); 
                cmdSelect.Dispose();
            }
        }

        public DataTable SelectAllDimOrgType(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idOrgType", System.Data.SqlDbType.Int).Value = dimOrgTypeTO.IdOrgType;
                SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
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

        public List<DimOrgTypeTO> ConvertDTToList(SqlDataReader dimOrgTypeTODT)
        {
            List<DimOrgTypeTO> dimOrgTypeTOList = new List<DimOrgTypeTO>();
            if (dimOrgTypeTODT != null)
            {
                while (dimOrgTypeTODT.Read())
                {
                    DimOrgTypeTO dimOrgTypeTONew = new DimOrgTypeTO();
                    if (dimOrgTypeTODT["idOrgType"] != DBNull.Value)
                        dimOrgTypeTONew.IdOrgType = Convert.ToInt32(dimOrgTypeTODT["idOrgType"].ToString());
                    if (dimOrgTypeTODT["isSystem"] != DBNull.Value)
                        dimOrgTypeTONew.IsSystem = Convert.ToInt32(dimOrgTypeTODT["isSystem"].ToString());
                    if (dimOrgTypeTODT["isActive"] != DBNull.Value)
                        dimOrgTypeTONew.IsActive = Convert.ToInt32(dimOrgTypeTODT["isActive"].ToString());
                    if (dimOrgTypeTODT["createUserYn"] != DBNull.Value)
                        dimOrgTypeTONew.CreateUserYn = Convert.ToInt32(dimOrgTypeTODT["createUserYn"].ToString());
                    if (dimOrgTypeTODT["defaultRoleId"] != DBNull.Value)
                        dimOrgTypeTONew.DefaultRoleId = Convert.ToInt32(dimOrgTypeTODT["defaultRoleId"].ToString());
                    if (dimOrgTypeTODT["OrgType"] != DBNull.Value)
                        dimOrgTypeTONew.OrgType = Convert.ToString(dimOrgTypeTODT["OrgType"].ToString());
                    if (dimOrgTypeTODT["isOwnerMandatory"] != DBNull.Value)
                        dimOrgTypeTONew.IsOwnerMandatory = Convert.ToInt32(dimOrgTypeTODT["isOwnerMandatory"].ToString());
                    if (dimOrgTypeTODT["isBankMandatory"] != DBNull.Value)
                        dimOrgTypeTONew.IsBankMandatory = Convert.ToInt32(dimOrgTypeTODT["isBankMandatory"].ToString());

                    if (dimOrgTypeTODT["isAddressMandatory"] != DBNull.Value)
                        dimOrgTypeTONew.IsAddressMandatory = Convert.ToInt32(dimOrgTypeTODT["isAddressMandatory"].ToString());

                    if (dimOrgTypeTODT["isKycYn"] != DBNull.Value)
                        dimOrgTypeTONew.IsKycYn = Convert.ToInt32(dimOrgTypeTODT["isKycYn"].ToString());
                    if (dimOrgTypeTODT["isKycMandatory"] != DBNull.Value)
                        dimOrgTypeTONew.IsKycMandatory = Convert.ToInt32(dimOrgTypeTODT["isKycMandatory"].ToString());
                    if (dimOrgTypeTODT["IsTransferToSAP"] != DBNull.Value)
                        dimOrgTypeTONew.IsTransferToSAP = Convert.ToInt32(dimOrgTypeTODT["IsTransferToSAP"].ToString());
                    if(dimOrgTypeTODT["exportRptTemplateName"] != DBNull.Value)
                        dimOrgTypeTONew.ExportRptTemplateName = Convert.ToString(dimOrgTypeTODT["exportRptTemplateName"].ToString());
                    if (dimOrgTypeTODT["isSendAPKLink"] != DBNull.Value)
                        dimOrgTypeTONew.IsSendAPKLink = Convert.ToInt32(dimOrgTypeTODT["isSendAPKLink"].ToString());

                    // ExportRptTemplateName
                    dimOrgTypeTOList.Add(dimOrgTypeTONew);
                }
            }
            return dimOrgTypeTOList;
        }
        #endregion

        #region Insertion
        public int InsertDimOrgType(DimOrgTypeTO dimOrgTypeTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(dimOrgTypeTO, cmdInsert);
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

        public int InsertDimOrgType(DimOrgTypeTO dimOrgTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(dimOrgTypeTO, cmdInsert);
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

        public int ExecuteInsertionCommand(DimOrgTypeTO dimOrgTypeTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [dimOrgType]( " + 
                                "  [idOrgType]" +
                                " ,[isSystem]" +
                                " ,[isActive]" +
                                " ,[createUserYn]" +
                                " ,[defaultRoleId]" +
                                " ,[OrgType]" +
                                " )" +
                    " VALUES (" +
                                "  @IdOrgType " +
                                " ,@IsSystem " +
                                " ,@IsActive " +
                                " ,@CreateUserYn " +
                                " ,@DefaultRoleId " +
                                " ,@OrgType " +
                                " ,@isOwnerMandatory" +                              
                                " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdOrgType", System.Data.SqlDbType.Int).Value = dimOrgTypeTO.IdOrgType;
            cmdInsert.Parameters.Add("@IsSystem", System.Data.SqlDbType.Int).Value = dimOrgTypeTO.IsSystem;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = dimOrgTypeTO.IsActive;
            cmdInsert.Parameters.Add("@CreateUserYn", System.Data.SqlDbType.Int).Value = dimOrgTypeTO.CreateUserYn;
            cmdInsert.Parameters.Add("@DefaultRoleId", System.Data.SqlDbType.Int).Value = dimOrgTypeTO.DefaultRoleId;
            cmdInsert.Parameters.Add("@OrgType", System.Data.SqlDbType.NVarChar).Value = dimOrgTypeTO.OrgType;
            cmdInsert.Parameters.Add("@isOwnerMandatory", System.Data.SqlDbType.Int).Value = dimOrgTypeTO.IsOwnerMandatory;


            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public int UpdateDimOrgType(DimOrgTypeTO dimOrgTypeTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(dimOrgTypeTO, cmdUpdate);
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

        public int UpdateDimOrgType(DimOrgTypeTO dimOrgTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(dimOrgTypeTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(DimOrgTypeTO dimOrgTypeTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [dimOrgType] SET " + 
            "  [idOrgType] = @IdOrgType" +
            " ,[isSystem]= @IsSystem" +
            " ,[isActive]= @IsActive" +
            " ,[createUserYn]= @CreateUserYn" +
            " ,[defaultRoleId]= @DefaultRoleId" +
            " ,[OrgType] = @OrgType" +
            " ,[isOwnerMandatory] =@isOwnerMandatory " +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdOrgType", System.Data.SqlDbType.Int).Value = dimOrgTypeTO.IdOrgType;
            cmdUpdate.Parameters.Add("@IsSystem", System.Data.SqlDbType.Int).Value = dimOrgTypeTO.IsSystem;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = dimOrgTypeTO.IsActive;
            cmdUpdate.Parameters.Add("@CreateUserYn", System.Data.SqlDbType.Int).Value = dimOrgTypeTO.CreateUserYn;
            cmdUpdate.Parameters.Add("@DefaultRoleId", System.Data.SqlDbType.Int).Value = dimOrgTypeTO.DefaultRoleId;
            cmdUpdate.Parameters.Add("@OrgType", System.Data.SqlDbType.NVarChar).Value = dimOrgTypeTO.OrgType;
            cmdUpdate.Parameters.Add("@isOwnerMandatory", System.Data.SqlDbType.Int).Value = dimOrgTypeTO.IsOwnerMandatory;

            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public int DeleteDimOrgType(Int32 idOrgType)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idOrgType, cmdDelete);
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

        public int DeleteDimOrgType(Int32 idOrgType, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idOrgType, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idOrgType, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [dimOrgType] " +
            " WHERE idOrgType = " + idOrgType +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idOrgType", System.Data.SqlDbType.Int).Value = dimOrgTypeTO.IdOrgType;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
