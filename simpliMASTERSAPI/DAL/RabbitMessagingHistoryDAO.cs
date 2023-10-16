using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

using ODLMWebAPI.Models;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.StaticStuff;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using simpliMASTERSAPI.DAL.Interfaces;

namespace ODLMWebAPI.DAL

{
    public class RabbitMessagingHistoryDAO : IRabbitMessagingHistoryDAO
    {
        private readonly IConnectionString _iConnectionString;
        public RabbitMessagingHistoryDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public static String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [rabbitMessagingHistory]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  RabbitMessagingHistoryTO SelectRabbitMessagingHistory(Int32 sourceId,Int32 rabbitTransId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = "SELECT top 1 * FROM [rabbitMessagingHistory] WHERE sourceId = " + sourceId + 
                    " AND rabbitTransId = "+ rabbitTransId+" "+ " ORDER BY idRabbitMessagingHistory desc ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<RabbitMessagingHistoryTO> list = ConvertDTToList(sqlReader);
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

        private  List<RabbitMessagingHistoryTO> ConvertDTToList(SqlDataReader rabbitMessagingHistoryTODT)
        {
            List<RabbitMessagingHistoryTO> rabbitMessagingHistoryTOList = new List<RabbitMessagingHistoryTO>();
            if (rabbitMessagingHistoryTODT != null)
            {
               
                    while (rabbitMessagingHistoryTODT.Read())
                    {
                    RabbitMessagingHistoryTO rabbitMessagingHistoryTONew = new RabbitMessagingHistoryTO();
                    if (rabbitMessagingHistoryTODT["idRabbitMessagingHistory"] != DBNull.Value)
                        rabbitMessagingHistoryTONew.IdRabbitMessagingHistory = Convert.ToInt32(rabbitMessagingHistoryTODT["idRabbitMessagingHistory"].ToString());
                    if (rabbitMessagingHistoryTODT["rabbitTransId"] != DBNull.Value)
                        rabbitMessagingHistoryTONew.RabbitTransId = Convert.ToInt32(rabbitMessagingHistoryTODT["rabbitTransId"].ToString());
                    if (rabbitMessagingHistoryTODT["sourceId"] != DBNull.Value)
                        rabbitMessagingHistoryTONew.SourceId = Convert.ToInt32(rabbitMessagingHistoryTODT["sourceId"].ToString());
                    rabbitMessagingHistoryTOList.Add(rabbitMessagingHistoryTONew);
                    }
            }
            return rabbitMessagingHistoryTOList;
        }
        #endregion

        #region Insertion
        public  int InsertRabbitMessagingHistory(RabbitMessagingHistoryTO rabbitMessagingHistoryTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(rabbitMessagingHistoryTO, cmdInsert);
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

        public  int InsertRabbitMessagingHistory(RabbitMessagingHistoryTO rabbitMessagingHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(rabbitMessagingHistoryTO, cmdInsert);
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

        private  int ExecuteInsertionCommand(RabbitMessagingHistoryTO rabbitMessagingHistoryTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [rabbitMessagingHistory]( " + 
            " [rabbitTransId]" +
            " ,[sourceId]" +
            " )" +
" VALUES (" +
            " @RabbitTransId " +
            " ,@SourceId " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@RabbitTransId", System.Data.SqlDbType.Int).Value = rabbitMessagingHistoryTO.RabbitTransId;
            cmdInsert.Parameters.Add("@SourceId", System.Data.SqlDbType.Int).Value = rabbitMessagingHistoryTO.SourceId;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        
            }
}
