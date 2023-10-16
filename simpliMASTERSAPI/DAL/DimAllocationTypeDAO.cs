using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ODLMWebAPI.DAL.Interfaces;
using TO;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.Models;

namespace ODLMWebAPI.DAL
{
    public class DimAllocationTypeDAO : IDimAllocationTypeDAO
    {
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [dimAllocationType]";
            return sqlSelectQry;
        }

        private readonly IConnectionString _iConnectionString;
        public DimAllocationTypeDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #endregion

        #region Selection

        public List<DropDownTO> getDynamicMaster(DimAllocationTypeTO allocTypeTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = allocTypeTO.Query;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dynamicDropdownList = new List<DropDownTO>();
                if (sqlReader != null)
                {
                    while (sqlReader.Read())
                    {
                        DropDownTO dynamicDropdownTO = new DropDownTO();
                        if (sqlReader["Value"] != DBNull.Value)
                            dynamicDropdownTO.Value = Convert.ToInt32(sqlReader["Value"].ToString());
                        if (sqlReader["Text"] != DBNull.Value)
                            dynamicDropdownTO.Text = Convert.ToString(sqlReader["Text"].ToString());

                        dynamicDropdownList.Add(dynamicDropdownTO);
                    }
                    return dynamicDropdownList;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }

        }
        public List<DimAllocationTypeTO> ConvertDTToList(SqlDataReader tblUserAreaAllocationTODT)
        {
            List<DimAllocationTypeTO> tblUserAreaAllocationTOList = new List<DimAllocationTypeTO>();
            if (tblUserAreaAllocationTODT != null)
            {
                while (tblUserAreaAllocationTODT.Read())
                {
                    DimAllocationTypeTO dimAreaAllocationTONew = new DimAllocationTypeTO();
                    if (tblUserAreaAllocationTODT["idAllocType"] != DBNull.Value)
                        dimAreaAllocationTONew.IdAllocType = Convert.ToInt32(tblUserAreaAllocationTODT["idAllocType"].ToString());
                    if (tblUserAreaAllocationTODT["allocTypeName"] != DBNull.Value)
                        dimAreaAllocationTONew.AllocTypeName = Convert.ToString(tblUserAreaAllocationTODT["allocTypeName"].ToString());

                    if (tblUserAreaAllocationTODT["allocTypeDescription"] != DBNull.Value)
                        dimAreaAllocationTONew.AllocTypeDescription = Convert.ToString(tblUserAreaAllocationTODT["allocTypeDescription"].ToString());

                    if (tblUserAreaAllocationTODT["query"] != DBNull.Value)
                        dimAreaAllocationTONew.Query = Convert.ToString(tblUserAreaAllocationTODT["query"].ToString());

                    if (tblUserAreaAllocationTODT["isActive"] != DBNull.Value)
                        dimAreaAllocationTONew.IsActive = Convert.ToInt32(tblUserAreaAllocationTODT["isActive"].ToString());


                    tblUserAreaAllocationTOList.Add(dimAreaAllocationTONew);
                }
            }
            return tblUserAreaAllocationTOList;
        }

        public List<DimAllocationTypeTO> SelectAllDimAllocationType()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idAllocType", System.Data.SqlDbType.Int).Value = dimAllocationTypeTO.IdAllocType;
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimAllocationTypeTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public DimAllocationTypeTO SelectDimAllocationType(Int32 idAllocType)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;

            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idAllocType = " + idAllocType +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimAllocationTypeTO> list = ConvertDTToList(rdr);
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
                if (rdr != null)
                    rdr.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<DimAllocationTypeTO> SelectAllDimAllocationType(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader sqlReader = null;
                //cmdSelect.Parameters.Add("@idAllocType", System.Data.SqlDbType.Int).Value = dimAllocationTypeTO.IdAllocType;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimAllocationTypeTO> list = ConvertDTToList(sqlReader);
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
        
        #region Insertion
        public  int InsertDimAllocationType(DimAllocationTypeTO dimAllocationTypeTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(dimAllocationTypeTO, cmdInsert);
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

        public  int InsertDimAllocationType(DimAllocationTypeTO dimAllocationTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(dimAllocationTypeTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(DimAllocationTypeTO dimAllocationTypeTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [dimAllocationType]( " + 
            "  [idAllocType]" +
            " ,[isActive]" +
            " ,[allocTypeName]" +
            " ,[allocTypeDescription]" +
            " ,[Query]" +
            " )" +
" VALUES (" +
            "  @IdAllocType " +
            " ,@IsActive " +
            " ,@AllocTypeName " +
            " ,@AllocTypeDescription " +
            " ,@Query " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdAllocType", System.Data.SqlDbType.Int).Value = dimAllocationTypeTO.IdAllocType;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = dimAllocationTypeTO.IsActive;
            cmdInsert.Parameters.Add("@AllocTypeName", System.Data.SqlDbType.VarChar).Value = dimAllocationTypeTO.AllocTypeName;
            cmdInsert.Parameters.Add("@AllocTypeDescription", System.Data.SqlDbType.VarChar).Value = dimAllocationTypeTO.AllocTypeDescription;
            cmdInsert.Parameters.Add("@Query", System.Data.SqlDbType.NVarChar).Value = dimAllocationTypeTO.Query;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public  int UpdateDimAllocationType(DimAllocationTypeTO dimAllocationTypeTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(dimAllocationTypeTO, cmdUpdate);
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

        public  int UpdateDimAllocationType(DimAllocationTypeTO dimAllocationTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(dimAllocationTypeTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(DimAllocationTypeTO dimAllocationTypeTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [dimAllocationType] SET " + 
            "  [idAllocType] = @IdAllocType" +
            " ,[isActive]= @IsActive" +
            " ,[allocTypeName]= @AllocTypeName" +
            " ,[allocTypeDescription]= @AllocTypeDescription" +
            " ,[Query] = @Query" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdAllocType", System.Data.SqlDbType.Int).Value = dimAllocationTypeTO.IdAllocType;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = dimAllocationTypeTO.IsActive;
            cmdUpdate.Parameters.Add("@AllocTypeName", System.Data.SqlDbType.VarChar).Value = dimAllocationTypeTO.AllocTypeName;
            cmdUpdate.Parameters.Add("@AllocTypeDescription", System.Data.SqlDbType.VarChar).Value = dimAllocationTypeTO.AllocTypeDescription;
            cmdUpdate.Parameters.Add("@Query", System.Data.SqlDbType.NVarChar).Value = dimAllocationTypeTO.Query;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteDimAllocationType(Int32 idAllocType)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idAllocType, cmdDelete);
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

        public  int DeleteDimAllocationType(Int32 idAllocType, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idAllocType, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int32 idAllocType, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [dimAllocationType] " +
            " WHERE idAllocType = " + idAllocType +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idAllocType", System.Data.SqlDbType.Int).Value = dimAllocationTypeTO.IdAllocType;
            return cmdDelete.ExecuteNonQuery();
        }

      
        #endregion

    }
}
