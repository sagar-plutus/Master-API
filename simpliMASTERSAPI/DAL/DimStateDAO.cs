using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL.Interfaces;

namespace ODLMWebAPI.DAL
{
    public class DimStateDAO : IDimStateDAO
    {
        private readonly IConnectionString _iConnectionString;
        public DimStateDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [dimState] where isActive=1 order by stateName asc";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<DimStateTO> SelectAllDimState()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idState", System.Data.SqlDbType.Int).Value = dimStateTO.IdState;
                SqlDataReader allstatesDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimStateTO> allStatesList = ConvertDTToList(allstatesDT);
                if (allStatesList != null)
                    return allStatesList;
                else
                    return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllDimState");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public DimStateTO SelectDimState(Int32 idState)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idState = " + idState + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idState", System.Data.SqlDbType.Int).Value = dimStateTO.IdState;
                SqlDataReader statesDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimStateTO> statesList = ConvertDTToList(statesDT);
                if (statesList != null)
                    return statesList[0];
                else
                    return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectDimState");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<DimStateTO> SelectAllDimState(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idState", System.Data.SqlDbType.Int).Value = dimStateTO.IdState;
                SqlDataReader allstatesDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimStateTO> allStatesList = ConvertDTToList(allstatesDT);
                if (allStatesList != null)
                    return allStatesList;
                else
                    return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllDimState");
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
            }
        }

        #endregion

        public List<DimStateTO> ConvertDTToList(SqlDataReader statesDT)
        {
            List<DimStateTO> stateTOList = new List<DimStateTO>();
            if (stateTOList != null)
            {
                while (statesDT.Read())
                {
                    DimStateTO dimStateTONew = new DimStateTO();
                    if (statesDT["idState"] != DBNull.Value)
                        dimStateTONew.IdState = Convert.ToInt32(statesDT["idState"].ToString());
                    if (statesDT["stateCode"] != DBNull.Value)
                        dimStateTONew.StateCode = statesDT["stateCode"].ToString();
                    if (statesDT["stateName"] != DBNull.Value)
                        dimStateTONew.StateName = statesDT["stateName"].ToString();
                    if (statesDT["country"] != DBNull.Value)
                        dimStateTONew.CountryName = statesDT["country"].ToString();
                    if (statesDT["stateOrUTCode"] != DBNull.Value)
                        dimStateTONew.StateOrUTCode = statesDT["stateOrUTCode"].ToString();
                    if (statesDT["countryId"] != DBNull.Value)
                        dimStateTONew.CountryId = Convert.ToInt32(statesDT["countryId"].ToString());
                    if (statesDT["mappedTxnId"] != DBNull.Value)
                        dimStateTONew.MappedTxnId = statesDT["mappedTxnId"].ToString();
                    stateTOList.Add(dimStateTONew);
                }
            }
            return stateTOList;
        }

        #region Insertion
        public int InsertDimState(DimStateTO dimStateTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(dimStateTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }
         
        public int InsertDimState(DimStateTO dimStateTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(dimStateTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public int ExecuteInsertionCommand(DimStateTO dimStateTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [dimState]( " +
           // "  [idState]" +
            " [stateCode]" +
            " ,[stateName]" +
            " ,[stateOrUTCode]" +
             " ,[countryId]" +
            " ,[isActive]" +
            " ,[country]" +
            " )" +
" VALUES (" +
           //"  @IdState " +
            " @StateCode " +
            " ,@StateName " +
            " ,@StateOrUTCode " +
             " ,@CountryId " +
            " ,@IsActive " +
            " ,@CountryName " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdState", System.Data.SqlDbType.Int).Value = dimStateTO.IdState;
            cmdInsert.Parameters.Add("@StateCode", System.Data.SqlDbType.NVarChar).Value = dimStateTO.StateCode;
            cmdInsert.Parameters.Add("@StateName", System.Data.SqlDbType.NVarChar).Value = dimStateTO.StateName;
            cmdInsert.Parameters.Add("@StateOrUTCode", System.Data.SqlDbType.NVarChar).Value = dimStateTO.StateOrUTCode;
            cmdInsert.Parameters.Add("@CountryId", System.Data.SqlDbType.Int).Value = dimStateTO.CountryId;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = 1;
            cmdInsert.Parameters.Add("@CountryName", System.Data.SqlDbType.NVarChar).Value = dimStateTO.CountryName;

            int isInserted = cmdInsert.ExecuteNonQuery();
            Int32 LastInsertId = 0;
            if (isInserted > 0)
            {
                cmdInsert.CommandText = "Select @@Identity";
                LastInsertId = Convert.ToInt32(cmdInsert.ExecuteScalar());
            }
            return LastInsertId;
        }
        #endregion

        #region Updation
        public int UpdateDimState(DimStateTO dimStateTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(dimStateTO, cmdUpdate);
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

        public int UpdateDimState(DimStateTO dimStateTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(dimStateTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public int ExecuteUpdationCommand(DimStateTO dimStateTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [dimState] SET " +
           // "  [idState] = @IdState" +
            " [stateCode]= @StateCode" +
            " ,[stateName]= @StateName" +
            " ,[stateOrUTCode] = @StateOrUTCode" +
            //" ,[country]= @CountryName" +
            " WHERE [idState] =  @IdState ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdState", System.Data.SqlDbType.Int).Value = dimStateTO.IdState;
            cmdUpdate.Parameters.Add("@StateCode", System.Data.SqlDbType.NVarChar).Value = dimStateTO.StateCode;
            cmdUpdate.Parameters.Add("@StateName", System.Data.SqlDbType.NVarChar).Value = dimStateTO.StateName;
            cmdUpdate.Parameters.Add("@StateOrUTCode", System.Data.SqlDbType.NVarChar).Value = dimStateTO.StateOrUTCode;
          // cmdUpdate.Parameters.Add("@CountryName", System.Data.SqlDbType.NVarChar).Value = dimStateTO.CountryName;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public int DeleteDimState(Int32 idState)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idState, cmdDelete);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public int DeleteDimState(Int32 idState, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idState, cmdDelete);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public int ExecuteDeletionCommand(Int32 idState, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [dimState] " +
            " WHERE idState = " + idState + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idState", System.Data.SqlDbType.Int).Value = dimStateTO.IdState;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
    }
}
