using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ODLMWebAPI.BL.Interfaces;
using simpliMASTERSAPI.DAL.Interfaces;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.Models;
using System.Data.SqlClient;
using System.Data;

namespace simpliMASTERSAPI.DAL
{
    public class TblFinLedgerDAO : ITblFinLedgerDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblFinLedgerDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT ledger.*, projects.finProjectName,actType.finAccountTypeName,currency.currnecyCode,createdUser.userDisplayName AS createdByUserName,updatedUser.userDisplayName AS updatedByUserName" +
                                  " FROM tblFinLedger ledger " +
                                  " LEFT JOIN tblFinProject projects ON projects.idFinProject = ledger.finProjectId " +
                                  " LEFT JOIN dimFinAccountType actType ON actType.idFinAccountType = ledger.finAccountTypeId " +
                                  " LEFT JOIN dimCurrency currency ON currency.idCurrency = ledger.currencyId " +
                                  " LEFT JOIN tblUser createdUser ON createdUser.idUser=ledger.createdBy" +
                                  " LEFT JOIN tblUser updatedUser ON updatedUser.idUser = ledger.updatedBy";
            return sqlSelectQry;
        }
        #endregion

        public List<TblFinLedgerTO> SelectChildLedgerList(long parentFinLedgerId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                //if(parentFinLedgerId != 0)
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE ISNULL(ledger.parentFinLedgerId,0)=" + parentFinLedgerId + " AND ledger.isActive=1";
               // else
                   //cmdSelect.CommandText = SqlSelectQuery() + " WHERE ledger.isActive=1";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(reader);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null)
                    reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblFinLedgerTO> SelectLedgerListForSearch(string LedgerName, string LedgerCode, Int32 Type)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
              
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE  ledger.isActive=1 and type= "+ Type + " ";
              
                if(!String.IsNullOrEmpty(LedgerName))
                {
                    cmdSelect.CommandText+=  "AND ledger.ledgerName LIKE '%" + LedgerName + "%'";
                }
              if (!String.IsNullOrEmpty(LedgerCode) )
                {
                    cmdSelect.CommandText+= " AND ledger.ledgerCode LIKE '%" + LedgerCode + "%' ";

                }

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(reader);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null)
                    reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public TblFinLedgerTO SelectLedgerListByParentId(long parentFinLedgerId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();

                cmdSelect.CommandText = SqlSelectQuery() + " WHERE  ledger.isActive=1 and idFinLedger= " + parentFinLedgerId + " ";

               
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblFinLedgerTO> data = new List<TblFinLedgerTO>();
                data = ConvertDTToList(reader);
                if(data!=null && data.Count>0)
                {
                    return data[0];
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null)
                    reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public int GetAutoGeneratedSAPLedgerCode()
        {
            int nLedgerCode = 0;
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.SAP_CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select MAX(REPLACE(acctcode, '_sys', '')) + 1 from OACT where acctcode like '_SYS%'";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                nLedgerCode = Convert.ToInt32(cmdSelect.ExecuteScalar());
                return nLedgerCode;
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<DropDownTO> SelectFinActTypeDropDownList()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = "SELECT * FROM dimFinAccountType WHERE isActive=1";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> list = null;
                if (reader != null)
                {
                    list = new List<DropDownTO>();
                    while (reader.Read())
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        if (reader["idFinAccountType"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(reader["idFinAccountType"].ToString());
                        if (reader["finAccountTypeName"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(reader["finAccountTypeName"].ToString());
                        if (reader["finAccountTypeCode"] != DBNull.Value)
                            dropDownTO.Tag = Convert.ToString(reader["finAccountTypeCode"].ToString());

                        list.Add(dropDownTO);
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
                if (reader != null)
                    reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<DropDownTO> SelectFinProjectsDropDownList()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = "SELECT * FROM tblFinProject WHERE isActive=1";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> list = null;
                if (reader != null)
                {
                    list = new List<DropDownTO>();
                    while (reader.Read())
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        if (reader["idFinProject"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(reader["idFinProject"].ToString());
                        if (reader["finProjectName"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(reader["finProjectName"].ToString());
                        if (reader["finProjectCode"] != DBNull.Value)
                            dropDownTO.MappedTxnId = Convert.ToString(reader["finProjectCode"].ToString());
                        if (reader["organizationId"] != DBNull.Value)
                            dropDownTO.Tag = Convert.ToString(reader["organizationId"].ToString());

                        list.Add(dropDownTO);
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
                if (reader != null)
                    reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblFinLedgerTO> SelectChildLedgerListFromSAP(String parentLedgerCode)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.SAP_CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = "SELECT  * FROM OACT WHERE ISNULL(FatherNum,0)='" + parentLedgerCode + "'";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertSAPDTToList(reader);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null)
                    reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public TblFinLedgerTO SelectLedgerTO(long finLedgerId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE ledger.idFinLedger=" + finLedgerId ;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblFinLedgerTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1)
                    return list[0];
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null)
                    reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblFinLedgerTO> ConvertDTToList(SqlDataReader tblFinLedgerTODT)
        {
            List<TblFinLedgerTO> tblFinLedgerTOList = new List<TblFinLedgerTO>();
            if (tblFinLedgerTODT != null)
            {
                while (tblFinLedgerTODT.Read())
                {
                    TblFinLedgerTO tblFinLedgerTONew = new TblFinLedgerTO();
                    if (tblFinLedgerTODT["level"] != DBNull.Value)
                        tblFinLedgerTONew.Level = Convert.ToInt32(tblFinLedgerTODT["level"].ToString());
                    if (tblFinLedgerTODT["type"] != DBNull.Value)
                        tblFinLedgerTONew.Type = Convert.ToInt32(tblFinLedgerTODT["type"].ToString());
                    if (tblFinLedgerTODT["currencyId"] != DBNull.Value)
                        tblFinLedgerTONew.CurrencyId = Convert.ToInt32(tblFinLedgerTODT["currencyId"].ToString());
                    if (tblFinLedgerTODT["confidential"] != DBNull.Value)
                        tblFinLedgerTONew.Confidential = Convert.ToInt32(tblFinLedgerTODT["confidential"].ToString());
                    if (tblFinLedgerTODT["finAccountTypeId"] != DBNull.Value)
                        tblFinLedgerTONew.FinAccountTypeId = Convert.ToInt32(tblFinLedgerTODT["finAccountTypeId"].ToString());
                    if (tblFinLedgerTODT["controlAct"] != DBNull.Value)
                        tblFinLedgerTONew.ControlAct = Convert.ToInt32(tblFinLedgerTODT["controlAct"].ToString());
                    if (tblFinLedgerTODT["cashAct"] != DBNull.Value)
                        tblFinLedgerTONew.CashAct = Convert.ToInt32(tblFinLedgerTODT["cashAct"].ToString());
                    if (tblFinLedgerTODT["blockManul"] != DBNull.Value)
                        tblFinLedgerTONew.BlockManul = Convert.ToInt32(tblFinLedgerTODT["blockManul"].ToString());
                    if (tblFinLedgerTODT["reval"] != DBNull.Value)
                        tblFinLedgerTONew.Reval = Convert.ToInt32(tblFinLedgerTODT["reval"].ToString());
                    if (tblFinLedgerTODT["cashFlow"] != DBNull.Value)
                        tblFinLedgerTONew.CashFlow = Convert.ToInt32(tblFinLedgerTODT["cashFlow"].ToString());
                    if (tblFinLedgerTODT["finProjectId"] != DBNull.Value)
                        tblFinLedgerTONew.FinProjectId = Convert.ToInt64(tblFinLedgerTODT["finProjectId"].ToString());
                    if (tblFinLedgerTODT["isActive"] != DBNull.Value)
                        tblFinLedgerTONew.IsActive = Convert.ToInt32(tblFinLedgerTODT["isActive"].ToString());
                    if (tblFinLedgerTODT["createdBy"] != DBNull.Value)
                        tblFinLedgerTONew.CreatedBy = Convert.ToInt32(tblFinLedgerTODT["createdBy"].ToString());
                    if (tblFinLedgerTODT["updatedBy"] != DBNull.Value)
                        tblFinLedgerTONew.UpdatedBy = Convert.ToInt32(tblFinLedgerTODT["updatedBy"].ToString());
                    if (tblFinLedgerTODT["createdOn"] != DBNull.Value)
                        tblFinLedgerTONew.CreatedOn = Convert.ToDateTime(tblFinLedgerTODT["createdOn"].ToString());
                    if (tblFinLedgerTODT["updatedOn"] != DBNull.Value)
                        tblFinLedgerTONew.UpdatedOn = Convert.ToDateTime(tblFinLedgerTODT["updatedOn"].ToString());
                    if (tblFinLedgerTODT["idFinLedger"] != DBNull.Value)
                        tblFinLedgerTONew.IdFinLedger = Convert.ToInt64(tblFinLedgerTODT["idFinLedger"].ToString());
                    if (tblFinLedgerTODT["parentFinLedgerId"] != DBNull.Value)
                        tblFinLedgerTONew.ParentFinLedgerId = Convert.ToInt64(tblFinLedgerTODT["parentFinLedgerId"].ToString());
                    if (tblFinLedgerTODT["ledgerCode"] != DBNull.Value)
                        tblFinLedgerTONew.LedgerCode = Convert.ToString(tblFinLedgerTODT["ledgerCode"].ToString());
                    if (tblFinLedgerTODT["ledgerName"] != DBNull.Value)
                        tblFinLedgerTONew.LedgerName = Convert.ToString(tblFinLedgerTODT["ledgerName"].ToString());
                    if (tblFinLedgerTODT["parentLedgerCode"] != DBNull.Value)
                        tblFinLedgerTONew.ParentLedgerCode = Convert.ToString(tblFinLedgerTODT["parentLedgerCode"].ToString());
                    if (tblFinLedgerTODT["createdByUserName"] != DBNull.Value)
                        tblFinLedgerTONew.CreatedByUserName = Convert.ToString(tblFinLedgerTODT["createdByUserName"].ToString());
                    if (tblFinLedgerTODT["updatedByUserName"] != DBNull.Value)
                        tblFinLedgerTONew.UpdatedByUserName = Convert.ToString(tblFinLedgerTODT["updatedByUserName"].ToString());
                    if (tblFinLedgerTODT["finAccountTypeName"] != DBNull.Value)
                        tblFinLedgerTONew.FinAccountTypeName = Convert.ToString(tblFinLedgerTODT["finAccountTypeName"].ToString());
                    if (tblFinLedgerTODT["currnecyCode"] != DBNull.Value)
                        tblFinLedgerTONew.CurrencyCode = Convert.ToString(tblFinLedgerTODT["currnecyCode"].ToString());
                    if (tblFinLedgerTODT["finProjectName"] != DBNull.Value)
                        tblFinLedgerTONew.FinProjectName = Convert.ToString(tblFinLedgerTODT["finProjectName"].ToString());
                    if (tblFinLedgerTODT["withholdTaxId"] != DBNull.Value)
                        tblFinLedgerTONew.WithholdTaxId = Convert.ToInt32(tblFinLedgerTODT["withholdTaxId"].ToString());
                    tblFinLedgerTOList.Add(tblFinLedgerTONew);
                }
            }
            return tblFinLedgerTOList;
        }

        public List<TblFinLedgerTO> ConvertSAPDTToList(SqlDataReader tblFinLedgerTODT)
        {
            List<TblFinLedgerTO> tblFinLedgerTOList = new List<TblFinLedgerTO>();
            if (tblFinLedgerTODT != null)
            {
                while (tblFinLedgerTODT.Read())
                {
                    TblFinLedgerTO tblFinLedgerTONew = new TblFinLedgerTO();
                    if (tblFinLedgerTODT["levels"] != DBNull.Value)
                        tblFinLedgerTONew.Level = Convert.ToInt32(tblFinLedgerTODT["levels"].ToString());

                    if (tblFinLedgerTODT["Postable"] != DBNull.Value)
                    {
                        string Postable = Convert.ToString(tblFinLedgerTODT["Postable"].ToString());
                        if (Postable == "N")
                            tblFinLedgerTONew.Type = 1;
                        else
                            tblFinLedgerTONew.Type = 2;
                    }

                    if (tblFinLedgerTODT["ActCurr"] != DBNull.Value)
                        tblFinLedgerTONew.CurrencyCode = Convert.ToString(tblFinLedgerTODT["ActCurr"].ToString());

                    if (tblFinLedgerTODT["Protected"] != DBNull.Value)
                    {
                        string protectedCode = Convert.ToString(tblFinLedgerTODT["Protected"].ToString());
                        if (protectedCode == "Y")
                            tblFinLedgerTONew.Confidential = 1;
                        else
                            tblFinLedgerTONew.Confidential = 0;
                    }

                    if (tblFinLedgerTODT["ActType"] != DBNull.Value)
                    {
                        string ActType = Convert.ToString(tblFinLedgerTODT["ActType"].ToString());
                        if (ActType == "N")
                            tblFinLedgerTONew.FinAccountTypeId = (int)Constants.FinAccountTypeE.OTHER;
                        if (ActType == "E")
                            tblFinLedgerTONew.FinAccountTypeId = (int)Constants.FinAccountTypeE.EXPENSE;
                        if (ActType == "I")
                            tblFinLedgerTONew.FinAccountTypeId = (int)Constants.FinAccountTypeE.REVENUE;

                    }

                    if (tblFinLedgerTODT["LocManTran"] != DBNull.Value)
                    {
                        string controlAccCode = Convert.ToString(tblFinLedgerTODT["LocManTran"].ToString());
                        if (controlAccCode == "Y")
                            tblFinLedgerTONew.ControlAct = 1;
                        else
                            tblFinLedgerTONew.ControlAct = 0;
                    }

                    if (tblFinLedgerTODT["Finanse"] != DBNull.Value)
                    {
                        string Finanse = Convert.ToString(tblFinLedgerTODT["Finanse"].ToString());
                        if (Finanse == "Y")
                            tblFinLedgerTONew.CashAct = 1;
                        else
                            tblFinLedgerTONew.CashAct = 0;

                    }
                    if (tblFinLedgerTODT["BlocManPos"] != DBNull.Value)
                    {
                        string BlocManPos = Convert.ToString(tblFinLedgerTODT["BlocManPos"].ToString());
                        if (BlocManPos == "Y")
                            tblFinLedgerTONew.BlockManul = 1;
                        else
                            tblFinLedgerTONew.BlockManul = 0;
                    }

                    if (tblFinLedgerTODT["RevalMatch"] != DBNull.Value)
                    {
                        string RevalMatch = Convert.ToString(tblFinLedgerTODT["RevalMatch"].ToString());
                        if (RevalMatch == "Y")
                            tblFinLedgerTONew.Reval = 1;
                        else
                            tblFinLedgerTONew.Reval = 0;
                    }

                    if (tblFinLedgerTODT["CfwRlvnt"] != DBNull.Value)
                    {
                        string CfwRlvnt = Convert.ToString(tblFinLedgerTODT["CfwRlvnt"].ToString());
                        if (CfwRlvnt == "Y")
                            tblFinLedgerTONew.CashFlow = 1;
                        else
                            tblFinLedgerTONew.CashFlow = 0;

                    }

                    //Project Mapping Pending
                    //if (tblFinLedgerTODT["finProjectId"] != DBNull.Value)
                    //    tblFinLedgerTONew.FinProjectId = Convert.ToInt64(tblFinLedgerTODT["finProjectId"].ToString());
                    tblFinLedgerTONew.IsActive = 1;
                    tblFinLedgerTONew.CreatedBy = 1;
                    if (tblFinLedgerTODT["CreateDate"] != DBNull.Value)
                        tblFinLedgerTONew.CreatedOn = Convert.ToDateTime(tblFinLedgerTODT["CreateDate"].ToString());

                    //Will created after creating in SW DB
                    //if (tblFinLedgerTODT["updatedOn"] != DBNull.Value)
                    //    tblFinLedgerTONew.UpdatedOn = Convert.ToDateTime(tblFinLedgerTODT["updatedOn"].ToString());
                    //if (tblFinLedgerTODT["idFinLedger"] != DBNull.Value)
                    //    tblFinLedgerTONew.IdFinLedger = Convert.ToInt64(tblFinLedgerTODT["idFinLedger"].ToString());
                    //if (tblFinLedgerTODT["parentFinLedgerId"] != DBNull.Value)
                    //    tblFinLedgerTONew.ParentFinLedgerId = Convert.ToInt64(tblFinLedgerTODT["parentFinLedgerId"].ToString());

                    if (tblFinLedgerTODT["AcctCode"] != DBNull.Value)
                        tblFinLedgerTONew.LedgerCode = Convert.ToString(tblFinLedgerTODT["AcctCode"].ToString());
                    if (tblFinLedgerTODT["AcctName"] != DBNull.Value)
                        tblFinLedgerTONew.LedgerName = Convert.ToString(tblFinLedgerTODT["AcctName"].ToString());
                    if (tblFinLedgerTODT["FatherNum"] != DBNull.Value)
                        tblFinLedgerTONew.ParentLedgerCode = Convert.ToString(tblFinLedgerTODT["FatherNum"].ToString());


                    tblFinLedgerTOList.Add(tblFinLedgerTONew);
                }
            }
            return tblFinLedgerTOList;
        }

        public int InsertTblFinLedger(TblFinLedgerTO tblFinLedgerTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblFinLedgerTO, cmdInsert);
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

        public Boolean isDuplicateLedgerCode(String LedgerCode, String LedgerName, Int32 type, Int64 idFinLedger = 0)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();

                if (type == 1)
                {
                    cmdSelect.CommandText = " Select COUNT(*) as duplicateLedger from tblFinLedger ledger " +
                                            " where ledger.ledgerCode = '" + LedgerCode + "' ";
                }

                if (type == 2)
                {
                    cmdSelect.CommandText = " Select COUNT(*) as duplicateLedger from tblFinLedger ledger " +
                                            " where ledger.ledgerName = '" + LedgerName + "' ";
                }



                //exclude current org (used for edit calls)
                if (idFinLedger > 0)
                {
                    String excludeCurrentOrgstr = " and idFinLedger not in ( " + idFinLedger + " ) ";
                    cmdSelect.CommandText = cmdSelect.CommandText + excludeCurrentOrgstr;
                }
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                int count = 0;
                if (sqlReader != null)
                {
                    while (sqlReader.Read())
                    {
                        if (sqlReader["duplicateLedger"] != DBNull.Value)
                            count = Convert.ToInt32(sqlReader["duplicateLedger"].ToString());
                    }
                }
                if (count > 0)
                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        public int InsertTblFinLedger(TblFinLedgerTO tblFinLedgerTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblFinLedgerTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblFinLedgerTO tblFinLedgerTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblFinLedger]( " +
                            "  [level]" +
                            " ,[type]" +
                            " ,[currencyId]" +
                            " ,[confidential]" +
                            " ,[finAccountTypeId]" +
                            " ,[controlAct]" +
                            " ,[cashAct]" +
                            " ,[blockManul]" +
                            " ,[reval]" +
                            " ,[cashFlow]" +
                            " ,[finProjectId]" +
                            " ,[isActive]" +
                            " ,[createdBy]" +
                            " ,[updatedBy]" +
                            " ,[createdOn]" +
                            " ,[updatedOn]" +
                            " ,[parentFinLedgerId]" +
                            " ,[ledgerCode]" +
                            " ,[ledgerName]" +
                            " ,[parentLedgerCode]" +
                            " ,[withholdTaxId]" +
                            " ,[statusId]" + //Added Dhananjay [26-12-2020]
                            " )" +
                " VALUES (" +
                            "  @Level " +
                            " ,@Type " +
                            " ,@CurrencyId " +
                            " ,@Confidential " +
                            " ,@FinAccountTypeId " +
                            " ,@ControlAct " +
                            " ,@CashAct " +
                            " ,@BlockManul " +
                            " ,@Reval " +
                            " ,@CashFlow " +
                            " ,@FinProjectId " +
                            " ,@IsActive " +
                            " ,@CreatedBy " +
                            " ,@UpdatedBy " +
                            " ,@CreatedOn " +
                            " ,@UpdatedOn " +
                            " ,@ParentFinLedgerId " +
                            " ,@LedgerCode " +
                            " ,@LedgerName " +
                            " ,@ParentLedgerCode " +
                            " ,@withholdTaxId " +
                            " ,@StatusId " + //Added Dhananjay [26-12-2020]
                            " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@Level", System.Data.SqlDbType.Int).Value = tblFinLedgerTO.Level;
            cmdInsert.Parameters.Add("@Type", System.Data.SqlDbType.Int).Value = tblFinLedgerTO.Type;
            cmdInsert.Parameters.Add("@CurrencyId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblFinLedgerTO.CurrencyId); 
            cmdInsert.Parameters.Add("@Confidential", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblFinLedgerTO.Confidential);
            cmdInsert.Parameters.Add("@FinAccountTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblFinLedgerTO.FinAccountTypeId);
            cmdInsert.Parameters.Add("@ControlAct", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblFinLedgerTO.ControlAct);
            cmdInsert.Parameters.Add("@CashAct", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblFinLedgerTO.CashAct);
            cmdInsert.Parameters.Add("@BlockManul", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblFinLedgerTO.BlockManul);
            cmdInsert.Parameters.Add("@Reval", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblFinLedgerTO.Reval);
            cmdInsert.Parameters.Add("@CashFlow", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblFinLedgerTO.CashFlow);
            cmdInsert.Parameters.Add("@FinProjectId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblFinLedgerTO.FinProjectId);
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblFinLedgerTO.IsActive;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblFinLedgerTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblFinLedgerTO.UpdatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblFinLedgerTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblFinLedgerTO.UpdatedOn);
            //cmdInsert.Parameters.Add("@IdFinLedger", System.Data.SqlDbType.BigInt).Value = tblFinLedgerTO.IdFinLedger;
            cmdInsert.Parameters.Add("@ParentFinLedgerId", System.Data.SqlDbType.BigInt).Value = Constants.GetSqlDataValueNullForBaseValue(tblFinLedgerTO.ParentFinLedgerId);

            cmdInsert.Parameters.Add("@LedgerCode", System.Data.SqlDbType.NVarChar).Value = tblFinLedgerTO.LedgerCode;
            cmdInsert.Parameters.Add("@LedgerName", System.Data.SqlDbType.NVarChar).Value = tblFinLedgerTO.LedgerName;
            cmdInsert.Parameters.Add("@ParentLedgerCode", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblFinLedgerTO.ParentLedgerCode);
            cmdInsert.Parameters.Add("@withholdTaxId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblFinLedgerTO.WithholdTaxId);

            cmdInsert.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblFinLedgerTO.StatusId; //Added Dhananjay [26-12-2020]
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblFinLedgerTO.IdFinLedger = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }

        public int UpdateTblFinLedger(TblFinLedgerTO tblFinLedgerTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblFinLedgerTO, cmdUpdate);
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

        public int UpdateTblFinLedger(TblFinLedgerTO tblFinLedgerTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblFinLedgerTO, cmdUpdate);
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

        public static int ExecuteUpdationCommand(TblFinLedgerTO tblFinLedgerTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblFinLedger] SET " +
                            "  [level] = @Level" +
                            " ,[type]= @Type" +
                            " ,[currencyId]= @CurrencyId" +
                            " ,[confidential]= @Confidential" +
                            " ,[finAccountTypeId]= @FinAccountTypeId" +
                            " ,[controlAct]= @ControlAct" +
                            " ,[cashAct]= @CashAct" +
                            " ,[blockManul]= @BlockManul" +
                            " ,[reval]= @Reval" +
                            " ,[cashFlow]= @CashFlow" +
                            " ,[finProjectId]= @FinProjectId" +
                            " ,[isActive]= @IsActive" +
                            " ,[updatedBy]= @UpdatedBy" +
                            " ,[updatedOn]= @UpdatedOn" +
                            " ,[parentFinLedgerId]= @ParentFinLedgerId" +
                            " ,[ledgerCode]= @LedgerCode" +
                            " ,[ledgerName]= @LedgerName" +
                            " ,[parentLedgerCode] = @ParentLedgerCode" +
                            " ,[withholdTaxId] = @withholdTaxId" +
                            
                            " WHERE [idFinLedger]= @IdFinLedger";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@Level", System.Data.SqlDbType.Int).Value = tblFinLedgerTO.Level;
            cmdUpdate.Parameters.Add("@Type", System.Data.SqlDbType.Int).Value = tblFinLedgerTO.Type;
            cmdUpdate.Parameters.Add("@CurrencyId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblFinLedgerTO.CurrencyId);
            cmdUpdate.Parameters.Add("@Confidential", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblFinLedgerTO.Confidential);
            cmdUpdate.Parameters.Add("@FinAccountTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblFinLedgerTO.FinAccountTypeId);
            cmdUpdate.Parameters.Add("@ControlAct", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblFinLedgerTO.ControlAct);
            cmdUpdate.Parameters.Add("@CashAct", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblFinLedgerTO.CashAct);
            cmdUpdate.Parameters.Add("@BlockManul", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblFinLedgerTO.BlockManul);
            cmdUpdate.Parameters.Add("@Reval", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblFinLedgerTO.Reval);
            cmdUpdate.Parameters.Add("@CashFlow", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblFinLedgerTO.CashFlow);
            cmdUpdate.Parameters.Add("@FinProjectId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblFinLedgerTO.FinProjectId);
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblFinLedgerTO.IsActive;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblFinLedgerTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblFinLedgerTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@IdFinLedger", System.Data.SqlDbType.BigInt).Value = tblFinLedgerTO.IdFinLedger;
            cmdUpdate.Parameters.Add("@ParentFinLedgerId", System.Data.SqlDbType.BigInt).Value = Constants.GetSqlDataValueNullForBaseValue(tblFinLedgerTO.ParentFinLedgerId);
            cmdUpdate.Parameters.Add("@LedgerCode", System.Data.SqlDbType.NVarChar).Value = tblFinLedgerTO.LedgerCode;
            cmdUpdate.Parameters.Add("@LedgerName", System.Data.SqlDbType.NVarChar).Value = tblFinLedgerTO.LedgerName;
            cmdUpdate.Parameters.Add("@ParentLedgerCode", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblFinLedgerTO.ParentLedgerCode);
            cmdUpdate.Parameters.Add("@withholdTaxId", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblFinLedgerTO.WithholdTaxId);
            return cmdUpdate.ExecuteNonQuery();
        }
    }
}
