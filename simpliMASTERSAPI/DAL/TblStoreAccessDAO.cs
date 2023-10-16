using System;
using System.Data.SqlClient;
using System.Data;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL.Interfaces;
using System.Collections.Generic;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;

namespace ODLMWebAPI.DAL
{
    public class TblStoreAccessDAO: ITblStoreAccessDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblStoreAccessDAO(IConnectionString connectionString)
        {
            _iConnectionString = connectionString;
        }
        #region Methods
        public  String SqlSelectQuery()
        {
            //String sqlSelectQry = " SELECT * FROM [tblStoreAccess]"; 
            //parentLoc.locationDesc as parentDesc,

            String sqlSelectQry = "SELECT locations.mappedTxnId,locations.parentLocId,parentLoc.locationDesc as parentDesc,dept.deptDisplayName,locations.locationDesc,tblusers.userDisplayName,tusers.userDisplayName as createdByUser, store.* FROM tblStoreAccess store " +
                                   " LEFT JOIN tblUser tblusers ON tblusers.idUser = store.userId " +
                                   " LEFT JOIN tblUser tusers ON tusers.idUser = store.createdBy " +
                                   " LEFT JOIN dimMstDept dept ON dept.idDept = store.deptId " +
                                   " LEFT JOIN tbllocation locations ON locations.idLocation = store.warehouseId " +
                                   " LEFT JOIN tblLocation parentLoc ON locations.parentLocId = parentLoc.idLocation ";
                                  // " LEFT JOIN tblLocation parentLoc ON parentLoc.parentLocId = store.warehouseId ";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblStoreAccessTO> SelectAllTblStoreAccess(Int32 userId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                string userIdCon = null;
                conn.Open();
                if (userId>0) {
                    userIdCon = " WHERE userId =" + userId;
                }
                cmdSelect.CommandText = SqlSelectQuery() + userIdCon;
                

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(sqlReader);
  
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null) sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose(); conn.Close();
                cmdSelect.Dispose();
            }
        }
        public List<TblStoreAccessTO> SelectAllTblStoreAccess(Int32 userId,Int32 deptId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                string userIdCon = null;
                conn.Open();
                if (userId > 0)
                {
                    userIdCon = " WHERE locations.parentLocId IS NOT NULL AND store.permission='RW' " + "AND (store.userId = "+ userId + " OR store.deptId = "+ deptId +")";
                }
                cmdSelect.CommandText = SqlSelectQuery() + userIdCon;


                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(sqlReader);

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null) sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose(); conn.Close();
                cmdSelect.Dispose();
            }
        }

        public TblStoreAccessTO SelectTblStoreAccess(Int32 idStoreAccess)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idStoreAccess = " + idStoreAccess +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblStoreAccessTO> list = ConvertDTToList(sqlReader);
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
                if (sqlReader != null) sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public TblStoreAccessTO SelectAllTblStoreAccess(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;


                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblStoreAccessTO> list = ConvertDTToList(sqlReader);
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
                cmdSelect.Dispose();
            }
        }

        #endregion
        
        #region Insertion
        public int InsertTblStoreAccess(TblStoreAccessTO tblStoreAccessTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblStoreAccessTO, cmdInsert);
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

        public  int InsertTblStoreAccess(TblStoreAccessTO tblStoreAccessTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblStoreAccessTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblStoreAccessTO tblStoreAccessTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblStoreAccess]( " + 
           // "  [idStoreAccess]" +
            " [deptId]" +
            " ,[userId]" +
            " ,[warehouseId]" +
            " ,[createdBy]" +
            " ,[createdOn]" +
            " ,[permission]" +
            " )" +
" VALUES (" +
          //  "  @IdStoreAccess " +
            " @DeptId " +
            " ,@UserId " +
            " ,@WarehouseId " +
            " ,@CreatedBy " +
            " ,@CreatedOn " +
            " ,@Permission " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            String sqlSelectIdentityQry = "Select @@Identity";
            //cmdInsert.Parameters.Add("@IdStoreAccess", System.Data.SqlDbType.Int).Value = tblStoreAccessTO.IdStoreAccess;
            if (tblStoreAccessTO.DeptId != -1)
                cmdInsert.Parameters.Add("@DeptId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblStoreAccessTO.DeptId);
            else
                cmdInsert.Parameters.Add("@DeptId", System.Data.SqlDbType.Int).Value = DBNull.Value;
            cmdInsert.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblStoreAccessTO.UserId);
            cmdInsert.Parameters.Add("@WarehouseId", System.Data.SqlDbType.Int).Value = tblStoreAccessTO.WarehouseId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblStoreAccessTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblStoreAccessTO.CreatedOn;
            cmdInsert.Parameters.Add("@Permission", System.Data.SqlDbType.NVarChar).Value = tblStoreAccessTO.Permission;
           
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = sqlSelectIdentityQry;
                tblStoreAccessTO.IdStoreAccess = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }

        public List<TblStoreAccessTO> ConvertDTToList(SqlDataReader tblStoreAccessTODT)
        {
            List<TblStoreAccessTO> tblStoreAccessTOList = new List<TblStoreAccessTO>();
            if (tblStoreAccessTODT != null)
            {
                while(tblStoreAccessTODT.Read())
                {
                    TblStoreAccessTO tblStoreAccessTONew = new TblStoreAccessTO();
                    if (tblStoreAccessTODT["idStoreAccess"] != DBNull.Value)
                        tblStoreAccessTONew.IdStoreAccess = Convert.ToInt32(tblStoreAccessTODT["idStoreAccess"].ToString());
                    if (tblStoreAccessTODT["deptId"] != DBNull.Value)
                        tblStoreAccessTONew.DeptId = Convert.ToInt32(tblStoreAccessTODT["deptId"].ToString());
                    if (tblStoreAccessTODT["userId"] != DBNull.Value)
                        tblStoreAccessTONew.UserId = Convert.ToInt32(tblStoreAccessTODT["userId"].ToString());
                    if (tblStoreAccessTODT["warehouseId"] != DBNull.Value)
                        tblStoreAccessTONew.WarehouseId = Convert.ToInt32(tblStoreAccessTODT["warehouseId"].ToString());
                    if (tblStoreAccessTODT["createdBy"] != DBNull.Value)
                        tblStoreAccessTONew.CreatedBy = Convert.ToInt32(tblStoreAccessTODT["createdBy"].ToString());
                    if (tblStoreAccessTODT["createdOn"] != DBNull.Value)
                        tblStoreAccessTONew.CreatedOn = Convert.ToDateTime(tblStoreAccessTODT["createdOn"].ToString());
                    if (tblStoreAccessTODT["permission"] != DBNull.Value)
                        tblStoreAccessTONew.Permission = Convert.ToString(tblStoreAccessTODT["permission"].ToString());
                    if (tblStoreAccessTODT["deptDisplayName"] != DBNull.Value)
                        tblStoreAccessTONew.DeptDisplayName = Convert.ToString(tblStoreAccessTODT["deptDisplayName"].ToString());
                    if (tblStoreAccessTODT["locationDesc"] != DBNull.Value)
                        tblStoreAccessTONew.LocationDesc = Convert.ToString(tblStoreAccessTODT["locationDesc"].ToString());
                    if (tblStoreAccessTODT["userDisplayName"] != DBNull.Value)
                        tblStoreAccessTONew.UserDisplayName = Convert.ToString(tblStoreAccessTODT["userDisplayName"].ToString());
                    if (tblStoreAccessTODT["createdByUser"] != DBNull.Value)
                        tblStoreAccessTONew.CreatedByUser = Convert.ToString(tblStoreAccessTODT["createdByUser"].ToString());
                    if (tblStoreAccessTODT["parentDesc"] != DBNull.Value)
                        tblStoreAccessTONew.ParentDesc = Convert.ToString(tblStoreAccessTODT["parentDesc"].ToString());
                    if (tblStoreAccessTODT["mappedTxnId"] != DBNull.Value)
                        tblStoreAccessTONew.MappedTxnId = Convert.ToString(tblStoreAccessTODT["mappedTxnId"].ToString());
                    if (tblStoreAccessTODT["parentLocId"] != DBNull.Value)
                        tblStoreAccessTONew.ParentLocId = Convert.ToInt32(tblStoreAccessTODT["parentLocId"].ToString());


                    tblStoreAccessTOList.Add(tblStoreAccessTONew);
                }
            }
            return tblStoreAccessTOList;
        }

        #endregion

        #region Updation
        public  int UpdateTblStoreAccess(TblStoreAccessTO tblStoreAccessTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblStoreAccessTO, cmdUpdate);
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

        public  int UpdateTblStoreAccess(TblStoreAccessTO tblStoreAccessTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblStoreAccessTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblStoreAccessTO tblStoreAccessTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblStoreAccess] SET " + 
           // "  [idStoreAccess] = @IdStoreAccess" +
         //   " [deptId]= @DeptId" +
          //  " ,[userId]= @UserId" +
          //  " ,[warehouseId]= @WarehouseId" +
          //  " ,[createdBy]= @CreatedBy" +
           // " ,[createdOn]= @CreatedOn" +
            " [permission] = @Permission" +
            " WHERE idStoreAccess = @IdStoreAccess "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdStoreAccess", System.Data.SqlDbType.Int).Value = tblStoreAccessTO.IdStoreAccess;
           // cmdUpdate.Parameters.Add("@DeptId", System.Data.SqlDbType.Int).Value = tblStoreAccessTO.DeptId;
           // cmdUpdate.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = tblStoreAccessTO.UserId;
          //  cmdUpdate.Parameters.Add("@WarehouseId", System.Data.SqlDbType.Int).Value = tblStoreAccessTO.WarehouseId;
         //   cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblStoreAccessTO.CreatedBy;
          //  cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblStoreAccessTO.CreatedOn;
            cmdUpdate.Parameters.Add("@Permission", System.Data.SqlDbType.NVarChar).Value = tblStoreAccessTO.Permission;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblStoreAccess(Int32 idStoreAccess)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idStoreAccess, cmdDelete);
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

        public  int DeleteTblStoreAccess(Int32 idStoreAccess, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idStoreAccess, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int32 idStoreAccess, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblStoreAccess] " +
            " WHERE idStoreAccess = " + idStoreAccess +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idStoreAccess", System.Data.SqlDbType.Int).Value = tblStoreAccessTO.IdStoreAccess;
            return cmdDelete.ExecuteNonQuery();
        }


        #endregion
        
    }
}
