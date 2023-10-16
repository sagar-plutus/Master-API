using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using Microsoft.Extensions.Logging;
using ODLMWebAPI.StaticStuff;
using System.Linq;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{
    public class TblTranActionsBL : ITblTranActionsBL
    {
        private readonly ITblTranActionsDAO _iTblTranActionsDAO;
        public TblTranActionsBL(ITblTranActionsDAO iTblTranActionsDAO)
        {
            _iTblTranActionsDAO = iTblTranActionsDAO;
        }
        #region Selection
        public List<TblTranActionsTO> SelectAllTblTranActions()
        {
            return _iTblTranActionsDAO.SelectAllTblTranActions();
        }

        public List<TblTranActionsTO> SelectAllTblTranActionsList(TblTranActionsTO tblTranActionsTO)
        {
            return _iTblTranActionsDAO.SelectAllTblTranActionsList(tblTranActionsTO);
        }

        public TblTranActionsTO SelectTblTranActionsTO(Int32 idTranActions)
        {
            return _iTblTranActionsDAO.SelectTblTranActions(idTranActions);
        }

       
        #endregion
        
        #region Insertion
        public int InsertTblTranActions(TblTranActionsTO tblTranActionsTO)
        {
            return _iTblTranActionsDAO.InsertTblTranActions(tblTranActionsTO);
        }

        public int InsertTblTranActions(TblTranActionsTO tblTranActionsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblTranActionsDAO.InsertTblTranActions(tblTranActionsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblTranActions(TblTranActionsTO tblTranActionsTO)
        {
            return _iTblTranActionsDAO.UpdateTblTranActions(tblTranActionsTO);
        }

        public int UpdateTblTranActions(TblTranActionsTO tblTranActionsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblTranActionsDAO.UpdateTblTranActions(tblTranActionsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblTranActions(Int32 idTranActions)
        {
            return _iTblTranActionsDAO.DeleteTblTranActions(idTranActions);
        }

        public int DeleteTblTranActions(Int32 idTranActions, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblTranActionsDAO.DeleteTblTranActions(idTranActions, conn, tran);
        }

        #endregion


       public ResultMessage AddTableFromSourceToDestination(string source, string destination, string tableName)
       {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                SqlConnection connSource = new SqlConnection(source);
                SqlConnection connDestination = new SqlConnection(destination);
                if(!DbTableExists(tableName,destination))
                {
                    resultMessage.DisplayMessage = "Table is not available in destination " + tableName;
                    return resultMessage;
                }
                int result = TruncateDBTable(tableName, destination);
                if (result != -1)
                {
                    resultMessage.DisplayMessage = "Issue while truncating the table";
                    return resultMessage;
                }
                //result = setIdentityInsertONAndOff(tableName, destination,true);
                //if (result <= 0){}
                DataTable table = new DataTable();
                string sqlQuery = "SELECT * FROM "+ tableName;
                SqlConnection sqlConn = new SqlConnection(source);
                SqlDataAdapter sqlDA = new SqlDataAdapter(sqlQuery, sqlConn);
                sqlDA.Fill(table);

                using (var bulkCopy = new SqlBulkCopy(destination, SqlBulkCopyOptions.KeepIdentity))
                {
                    foreach (DataColumn col in table.Columns)
                    {
                        bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                    }
                    bulkCopy.BulkCopyTimeout = 600;
                    bulkCopy.DestinationTableName = tableName;
                    bulkCopy.WriteToServer(table);
                }
                //result = setIdentityInsertONAndOff(tableName, destination,false);
                //if (result <= 0){}
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch(Exception ex)
            {
                resultMessage.DisplayMessage = ex.Message;
                return resultMessage;
            }
            finally
            {

            }

       }

        public bool DbTableExists(string strTableNameAndSchema, string strConnection)
        {
            using (SqlConnection connection = new SqlConnection(strConnection))
            {
                string strCheckTable =
                   String.Format(
                      "IF OBJECT_ID('{0}', 'U') IS NOT NULL SELECT 'true' ELSE SELECT 'false'",
                      strTableNameAndSchema);

                SqlCommand command = new SqlCommand(strCheckTable, connection);
                command.CommandType = CommandType.Text;
                connection.Open();

                return Convert.ToBoolean(command.ExecuteScalar());
            }
        }

        public int TruncateDBTable(string strTableNameAndSchema, string strConnection)
        {
            using (SqlConnection connection = new SqlConnection(strConnection))
            {
                string strCheckTable = "TRUNCATE TABLE " + strTableNameAndSchema;
                SqlCommand command = new SqlCommand(strCheckTable, connection);
                command.CommandType = CommandType.Text;
                connection.Open();

                return command.ExecuteNonQuery();
            }
        }

        public int setIdentityInsertONAndOff(string strTableNameAndSchema, string strConnection,bool isONorOff)
        {
            using (SqlConnection connection = new SqlConnection(strConnection))
            {
                string strCheckTable = "";
                if (isONorOff)
                {
                    strCheckTable = "SET IDENTITY_INSERT " + strTableNameAndSchema + " ON";
                }
                else
                {
                    strCheckTable = "SET IDENTITY_INSERT " + strTableNameAndSchema + " OFF";
                }
                SqlCommand command = new SqlCommand(strCheckTable, connection);
                command.CommandType = CommandType.Text;
                connection.Open();
                return command.ExecuteNonQuery();
            }
        }
    }
}
