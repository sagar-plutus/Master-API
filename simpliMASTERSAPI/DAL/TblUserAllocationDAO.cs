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
    public class TblUserAllocationDAO : ITblUserAllocationDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblUserAllocationDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT tblUser.userDisplayName,* FROM [tblUserAllocation] tblUserAllocation " +
                                  " LEFT JOIN tblUser tblUser on tblUser.idUser = tblUserAllocation.refId"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblUserAllocationTO> GetUserAllocationList(Int32? userId , Int32? allocTypeId, Int32? refId)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            
            String whereallocTypeClause = " AND tblUserAllocation.allocTypeId =" + allocTypeId;
            String whereUserClause = " AND tblUserAllocation.userId = " + userId;
            String whereRefClause = " AND tblUserAllocation.refId = " + refId;

            String sqlStr = SqlSelectQuery() + " Where tblUserAllocation.isActive = 1 ";
            if (allocTypeId != null && allocTypeId > 0)
            {
                sqlStr = sqlStr + whereallocTypeClause;
            }

            if (userId != null && userId > 0)
            {
                sqlStr = sqlStr + whereUserClause;
            }
            else
            {
                if(refId != null && refId > 0)
                    sqlStr = sqlStr + whereRefClause;
            }

            try
            {
                conn.Open();

                cmdSelect.CommandText = sqlStr;
               // cmdSelect.Parameters.AddWithValue("@userId", DbType.Int32).Value = userId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader sqlDataReader = cmdSelect.ExecuteReader();
                List<TblUserAllocationTO> tblUserAllocationTOList = ConvertDTToList(sqlDataReader);
                return tblUserAllocationTOList;
               
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
        public List<TblUserAllocationTO> ConvertDTToList(SqlDataReader tblUserAllocationTODT)
        {
            List<TblUserAllocationTO> tblUserAllocationTOList = new List<TblUserAllocationTO>();
            if (tblUserAllocationTODT != null)
            {
                while (tblUserAllocationTODT.Read())
                {
                    TblUserAllocationTO tblUserAllocationTO = new TblUserAllocationTO();
                    if (tblUserAllocationTODT["idUserAlloc"] != DBNull.Value)
                        tblUserAllocationTO.IdUserAlloc = Convert.ToInt32(tblUserAllocationTODT["idUserAlloc"].ToString());
                    if (tblUserAllocationTODT["userId"] != DBNull.Value)
                        tblUserAllocationTO.UserId = Convert.ToInt32(tblUserAllocationTODT["userId"].ToString());
                    if (tblUserAllocationTODT["refId"] != DBNull.Value)
                        tblUserAllocationTO.RefId = Convert.ToInt32(tblUserAllocationTODT["refId"].ToString());
                    if (tblUserAllocationTODT["allocTypeId"] != DBNull.Value)
                        tblUserAllocationTO.AllocTypeId = Convert.ToInt32(tblUserAllocationTODT["allocTypeId"].ToString());
                    if (tblUserAllocationTODT["isActive"] != DBNull.Value)
                        tblUserAllocationTO.IsActive = Convert.ToInt32(tblUserAllocationTODT["isActive"].ToString());
                    if (tblUserAllocationTODT["userDisplayName"] != DBNull.Value)
                        tblUserAllocationTO.UserDisplayName = Convert.ToString(tblUserAllocationTODT["userDisplayName"].ToString());

                    tblUserAllocationTOList.Add(tblUserAllocationTO);
                }
            }
            return tblUserAllocationTOList;
        }
        #endregion

        #region Insertion

        public int InsertTblUserAllocation(TblUserAllocationTO tblAllocationTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblAllocationTO, cmdInsert);
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

        public int InsertTblUserAllocation(TblUserAllocationTO dimAllocationTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(dimAllocationTypeTO, cmdInsert);
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

        
        

        public int ExecuteInsertionCommand(TblUserAllocationTO tblUserAllocationTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblUserAllocation]( " +
         //   "  [idUserAlloc]," +
            " [userId]" +
            " ,[refId]" +
            " ,[allocTypeId]" +
            " ,[isActive]" +
            " ,[updatedBy]" +
            " ,[createdBy]" +
            " ,[createdOn]" +
            " )" +
" VALUES (" +
      //      "  @IdUserAlloc " +
            " @UserId " +
            " ,@RefId " +
            " ,@AllocTypeId " +
            " ,@IsActive " +
            " ,@UpdatedBy " +
            " ,@CreatedBy " +
            " ,@CreatedOn " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

          //  cmdInsert.Parameters.Add("@IdUserAlloc", System.Data.SqlDbType.Int).Value = tblUserAllocationTO.IdUserAlloc;
            cmdInsert.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = tblUserAllocationTO.UserId;
            cmdInsert.Parameters.Add("@RefId", System.Data.SqlDbType.Int).Value = tblUserAllocationTO.RefId;
            cmdInsert.Parameters.Add("@AllocTypeId", System.Data.SqlDbType.Int).Value = tblUserAllocationTO.AllocTypeId;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblUserAllocationTO.IsActive;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblUserAllocationTO.UpdatedBy;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblUserAllocationTO.CreatedBy;
         //   cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblUserAllocationTO.UpdatedOn;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblUserAllocationTO.CreatedOn;
            return cmdInsert.ExecuteNonQuery();
        }

        #endregion

        #region updation


        public int UpdateTblUserAllocation(TblUserAllocationTO tblAllocationTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblAllocationTO, cmdUpdate);
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

        public int UpdateTblUserAllocation(TblUserAllocationTO dimAllocationTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(dimAllocationTypeTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(TblUserAllocationTO tblUserAllocationTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblUserAllocation] SET " +
          //  "  [idUserAlloc] = @IdUserAlloc" +
            " [userId]= @UserId" +
            " ,[refId]= @RefId" +
            " ,[allocTypeId]= @AllocTypeId" +
            " ,[isActive]= @IsActive" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedOn]= @UpdatedOn" +

            " WHERE idUserAlloc =  "+ tblUserAllocationTO.IdUserAlloc;
            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;
           // cmdUpdate.Parameters.Add("@IdUserAlloc", System.Data.SqlDbType.Int).Value = tblUserAllocationTO.IdUserAlloc;
            cmdUpdate.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = tblUserAllocationTO.UserId;
            cmdUpdate.Parameters.Add("@RefId", System.Data.SqlDbType.Int).Value = tblUserAllocationTO.RefId;
            cmdUpdate.Parameters.Add("@AllocTypeId", System.Data.SqlDbType.Int).Value = tblUserAllocationTO.AllocTypeId;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblUserAllocationTO.IsActive;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblUserAllocationTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblUserAllocationTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblUserAllocationTO.UpdatedOn;
          //  cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblUserAllocationTO.CreatedOn;
            return cmdUpdate.ExecuteNonQuery();
        }

        #endregion
    }
}
