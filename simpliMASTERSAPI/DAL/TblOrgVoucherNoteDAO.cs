using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using simpliMASTERSAPI.DAL.Interfaces;
using ODLMWebAPI.BL.Interfaces;
using simpliMASTERSAPI.Models;
using ODLMWebAPI.StaticStuff;
using System.Data;

namespace simpliMASTERSAPI.DAL
{
    public class TblOrgVoucherNoteDAO : ITblOrgVoucherNoteDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblOrgVoucherNoteDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        public String SqlSelectQuery()
        {
            String sqlSelectQry = "select tblOrgVoucherNote.*,dimVoucherNoteReason.reasonDesc,tblModule.moduleName,dimTransactionType.transactionName,tblUser.userDisplayName as createdByName, tblUserUpdatedBy.userDisplayName as updatedByName,dimFinVoucherType.FinVoucherType,dimFinVoucherType.type,tblOrganization.firmName,tblOrganization.orgTypeId,dimStatus.statusDesc from tblOrgVoucherNote tblOrgVoucherNote " +
            " JOIN dimFinVoucherType dimFinVoucherType on dimFinVoucherType.idfinVoucherType = tblOrgVoucherNote.voucherTypeId " +
            " JOIN tblOrganization tblOrganization on tblOrganization.idOrganization = tblOrgVoucherNote.organizationId " +
            " JOIN tblModule tblModule on tblModule.idModule = tblOrgVoucherNote.moduleId " +
            " JOIN dimVoucherNoteReason dimVoucherNoteReason on dimVoucherNoteReason.idVoucherNoteReason = tblOrgVoucherNote.voucherNoteReasonId" +
            " LEFT JOIN dimStatus dimStatus on dimStatus.idStatus = tblOrgVoucherNote.statusId " +
            " LEFT JOIN dimTransactionType dimTransactionType on dimTransactionType.idTransactionType = tblOrgVoucherNote.transactionTypeId " +
            " LEFT JOIN tblUser tblUser on tblUser.idUser = tblOrgVoucherNote.createdBy " +
            " LEFT JOIN tblUser tblUserUpdatedBy on tblUserUpdatedBy.idUser = tblOrgVoucherNote.updatedBy " +
            " where 1=1 ";
            return sqlSelectQry;
        }
        #region Get
        public List<TblOrgVoucherNoteTO> GetVoucherNoteList(TblOrgVoucherNoteFilterTO tblOrgVoucherNoteFilterTO, String viewPendingStatusStr = "")
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                if (!String.IsNullOrEmpty(viewPendingStatusStr) && tblOrgVoucherNoteFilterTO.ViewPending == true)
                {
                    cmdSelect.CommandText += " and tblOrgVoucherNote.statusId NOT IN(" + viewPendingStatusStr + ")";
                }
                if (!String.IsNullOrEmpty(tblOrgVoucherNoteFilterTO.StatusIdStr))
                {
                    cmdSelect.CommandText += " and tblOrgVoucherNote.statusId IN("+ tblOrgVoucherNoteFilterTO.StatusIdStr + ")";
                }
                if (!String.IsNullOrEmpty(tblOrgVoucherNoteFilterTO.VoucherTypeIdStr))
                {
                    cmdSelect.CommandText += " and tblOrgVoucherNote.voucherTypeId IN(" + tblOrgVoucherNoteFilterTO.VoucherTypeIdStr + ")";
                }
                if(tblOrgVoucherNoteFilterTO.SkipDateFilter == false)
                {
                    TimeSpan ts = new TimeSpan(00, 00, 0);
                    tblOrgVoucherNoteFilterTO.FromDate = tblOrgVoucherNoteFilterTO.FromDate.Date + ts;
                    ts = new TimeSpan(23, 59, 0);
                    tblOrgVoucherNoteFilterTO.ToDate = tblOrgVoucherNoteFilterTO.ToDate.Date + ts;
                    cmdSelect.CommandText += " and CAST(tblOrgVoucherNote.createdOn AS DATE) BETWEEN @fromDate AND @toDate";
                    cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = tblOrgVoucherNoteFilterTO.FromDate;
                    cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = tblOrgVoucherNoteFilterTO.ToDate;
                }
                cmdSelect.CommandText += " order by tblOrgVoucherNote.idIOrgVoucherNote DESC";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblOrgVoucherNoteTO> list = ConvertDTToList(rdr);
                return list;
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
        public List<TblOrgVoucherNoteTO> ConvertDTToList(SqlDataReader TblOrgVoucherNoteDT)
        {
            List<TblOrgVoucherNoteTO> TblOrgVoucherNoteTOList = new List<TblOrgVoucherNoteTO>();
            if (TblOrgVoucherNoteDT != null)
            {
                while (TblOrgVoucherNoteDT.Read())
                {
                    TblOrgVoucherNoteTO TblOrgVoucherNoteTO = new TblOrgVoucherNoteTO();
                    if (TblOrgVoucherNoteDT["idIOrgVoucherNote"] != DBNull.Value)
                        TblOrgVoucherNoteTO.IdIOrgVoucherNote = Convert.ToInt64(TblOrgVoucherNoteDT["idIOrgVoucherNote"].ToString());
                    if (TblOrgVoucherNoteDT["organizationId"] != DBNull.Value)
                        TblOrgVoucherNoteTO.OrganizationId = Convert.ToInt32(TblOrgVoucherNoteDT["organizationId"].ToString());
                    if (TblOrgVoucherNoteDT["voucherTypeId"] != DBNull.Value)
                        TblOrgVoucherNoteTO.VoucherTypeId = Convert.ToInt32(TblOrgVoucherNoteDT["voucherTypeId"].ToString());
                    if (TblOrgVoucherNoteDT["moduleId"] != DBNull.Value)
                        TblOrgVoucherNoteTO.ModuleId = Convert.ToInt32(TblOrgVoucherNoteDT["moduleId"].ToString());
                    if (TblOrgVoucherNoteDT["moduleName"] != DBNull.Value)
                        TblOrgVoucherNoteTO.ModuleName = Convert.ToString(TblOrgVoucherNoteDT["moduleName"].ToString());
                    if (TblOrgVoucherNoteDT["transactionTypeId"] != DBNull.Value)
                        TblOrgVoucherNoteTO.TransactionTypeId = Convert.ToInt32(TblOrgVoucherNoteDT["transactionTypeId"].ToString());
                    if (TblOrgVoucherNoteDT["transactionId"] != DBNull.Value)
                        TblOrgVoucherNoteTO.TransactionId = Convert.ToInt64(TblOrgVoucherNoteDT["transactionId"].ToString());
                    if (TblOrgVoucherNoteDT["voucherNoteAmt"] != DBNull.Value)
                        TblOrgVoucherNoteTO.VoucherNoteAmt = Convert.ToDouble(TblOrgVoucherNoteDT["voucherNoteAmt"].ToString());
                    if (TblOrgVoucherNoteDT["gstTaxPercentage"] != DBNull.Value)
                        TblOrgVoucherNoteTO.GstTaxPercentage = Convert.ToDouble(TblOrgVoucherNoteDT["gstTaxPercentage"].ToString());
                    if (TblOrgVoucherNoteDT["gstTaxAmt"] != DBNull.Value)
                        TblOrgVoucherNoteTO.GstTaxAmt = Convert.ToDouble(TblOrgVoucherNoteDT["gstTaxAmt"].ToString());
                    if (TblOrgVoucherNoteDT["tdsTaxPercentage"] != DBNull.Value)
                        TblOrgVoucherNoteTO.TdsTaxPercentage = Convert.ToDouble(TblOrgVoucherNoteDT["tdsTaxPercentage"].ToString());
                    if (TblOrgVoucherNoteDT["tdsTaxAmt"] != DBNull.Value)
                        TblOrgVoucherNoteTO.TdsTaxAmt = Convert.ToDouble(TblOrgVoucherNoteDT["tdsTaxAmt"].ToString());
                    if (TblOrgVoucherNoteDT["totalVoucherNoteAmt"] != DBNull.Value)
                        TblOrgVoucherNoteTO.TotalVoucherNoteAmt = Convert.ToDouble(TblOrgVoucherNoteDT["totalVoucherNoteAmt"].ToString());
                    if (TblOrgVoucherNoteDT["voucherId"] != DBNull.Value)
                        TblOrgVoucherNoteTO.VoucherId = Convert.ToInt32(TblOrgVoucherNoteDT["voucherId"].ToString());
                    if (TblOrgVoucherNoteDT["statusId"] != DBNull.Value)
                        TblOrgVoucherNoteTO.StatusId = Convert.ToInt32(TblOrgVoucherNoteDT["statusId"].ToString());
                    if (TblOrgVoucherNoteDT["createdBy"] != DBNull.Value)
                        TblOrgVoucherNoteTO.CreatedBy = Convert.ToInt32(TblOrgVoucherNoteDT["createdBy"].ToString());
                    if (TblOrgVoucherNoteDT["createdOn"] != DBNull.Value)
                        TblOrgVoucherNoteTO.CreatedOn = Convert.ToDateTime(TblOrgVoucherNoteDT["createdOn"].ToString());
                    if (TblOrgVoucherNoteDT["updatedBy"] != DBNull.Value)
                        TblOrgVoucherNoteTO.UpdatedBy = Convert.ToInt32(TblOrgVoucherNoteDT["updatedBy"].ToString());
                    if (TblOrgVoucherNoteDT["updatedOn"] != DBNull.Value)
                        TblOrgVoucherNoteTO.UpdatedOn = Convert.ToDateTime(TblOrgVoucherNoteDT["updatedOn"].ToString());
                    if (TblOrgVoucherNoteDT["FinVoucherType"] != DBNull.Value)
                        TblOrgVoucherNoteTO.FinVoucherType = Convert.ToString(TblOrgVoucherNoteDT["FinVoucherType"].ToString());
                    if (TblOrgVoucherNoteDT["firmName"] != DBNull.Value)
                        TblOrgVoucherNoteTO.FirmName = Convert.ToString(TblOrgVoucherNoteDT["firmName"].ToString());
                    if (TblOrgVoucherNoteDT["statusDesc"] != DBNull.Value)
                        TblOrgVoucherNoteTO.StatusDesc = Convert.ToString(TblOrgVoucherNoteDT["statusDesc"].ToString());
                    if (TblOrgVoucherNoteDT["createdByName"] != DBNull.Value)
                        TblOrgVoucherNoteTO.CreatedByName = Convert.ToString(TblOrgVoucherNoteDT["createdByName"].ToString());
                    if (TblOrgVoucherNoteDT["updatedByName"] != DBNull.Value)
                        TblOrgVoucherNoteTO.UpdatedByName = Convert.ToString(TblOrgVoucherNoteDT["updatedByName"].ToString());
                    if (TblOrgVoucherNoteDT["transactionName"] != DBNull.Value)
                        TblOrgVoucherNoteTO.TransactionName = Convert.ToString(TblOrgVoucherNoteDT["transactionName"].ToString());
                    if (TblOrgVoucherNoteDT["remark"] != DBNull.Value)
                        TblOrgVoucherNoteTO.Remark = Convert.ToString(TblOrgVoucherNoteDT["remark"].ToString());
                    if (TblOrgVoucherNoteDT["narration"] != DBNull.Value)
                        TblOrgVoucherNoteTO.Narration = Convert.ToString(TblOrgVoucherNoteDT["narration"].ToString());
                    if (TblOrgVoucherNoteDT["voucherNoteReasonId"] != DBNull.Value)
                        TblOrgVoucherNoteTO.VoucherNoteReasonId = Convert.ToInt32(TblOrgVoucherNoteDT["voucherNoteReasonId"].ToString());
                    if (TblOrgVoucherNoteDT["reasonDesc"] != DBNull.Value)
                        TblOrgVoucherNoteTO.ReasonDesc = Convert.ToString(TblOrgVoucherNoteDT["reasonDesc"].ToString());
                    if (TblOrgVoucherNoteDT["type"] != DBNull.Value)
                        TblOrgVoucherNoteTO.Type = Convert.ToInt32(TblOrgVoucherNoteDT["type"].ToString());
                    if (TblOrgVoucherNoteDT["orgTypeId"] != DBNull.Value)
                        TblOrgVoucherNoteTO.OrgTypeId = Convert.ToInt32(TblOrgVoucherNoteDT["orgTypeId"].ToString());
                    

                    TblOrgVoucherNoteTOList.Add(TblOrgVoucherNoteTO);
                }
            }
            return TblOrgVoucherNoteTOList;
        }
        #endregion
        #region Insert
        public Int32 AddVoucherNote(TblOrgVoucherNoteTO tblOrgVoucherNoteTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblOrgVoucherNoteTO, cmdInsert);
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
        public int ExecuteInsertionCommand(TblOrgVoucherNoteTO tblOrgVoucherNoteTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblOrgVoucherNote]( " +
                            "  [organizationId]" +
                            " ,[voucherTypeId]" +
                            " ,[voucherNoteReasonId]" +
                            " ,[moduleId]" +
                            " ,[transactionTypeId]" +
                            " ,[transactionId]" +
                            " ,[voucherNoteAmt]" +
                            " ,[gstTaxPercentage]" +
                            " ,[gstTaxAmt]" +
                            " ,[tdsTaxPercentage]" +
                            " ,[tdsTaxAmt]" +
                            " ,[totalVoucherNoteAmt]" +
                             " ,[statusId]" +
                             " ,[remark]" +
                             " ,[narration]" +
                             " ,[createdBy]" +
                             " ,[createdOn]" +
                            " )" +
                " VALUES (" +
                            "  @organizationId " +
                            " ,@voucherTypeId " +
                            " ,@voucherNoteReasonId " +
                            " ,@moduleId " +
                            " ,@transactionTypeId " +
                            " ,@transactionId " +
                            " ,@voucherNoteAmt " +
                            " ,@gstTaxPercentage " +
                            " ,@gstTaxAmt " +
                            " ,@tdsTaxPercentage " +
                            " ,@tdsTaxAmt " +
                            " ,@totalVoucherNoteAmt " +
                            " ,@statusId " +
                            " ,@remark " +
                            " ,@narration " +
                            " ,@createdBy " +
                            " ,@createdOn " +
                             " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            String sqlSelectIdentityQry = "Select @@Identity";
            cmdInsert.Parameters.Add("@organizationId", System.Data.SqlDbType.Int).Value = tblOrgVoucherNoteTO.OrganizationId;
            cmdInsert.Parameters.Add("@voucherTypeId", System.Data.SqlDbType.BigInt).Value = tblOrgVoucherNoteTO.VoucherTypeId;
            cmdInsert.Parameters.Add("@voucherNoteReasonId", System.Data.SqlDbType.BigInt).Value = tblOrgVoucherNoteTO.VoucherNoteReasonId;
            cmdInsert.Parameters.Add("@moduleId", System.Data.SqlDbType.Int).Value = tblOrgVoucherNoteTO.ModuleId;
            cmdInsert.Parameters.Add("@transactionTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrgVoucherNoteTO.TransactionTypeId);
            cmdInsert.Parameters.Add("@transactionId", System.Data.SqlDbType.BigInt).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrgVoucherNoteTO.TransactionId);
            cmdInsert.Parameters.Add("@voucherNoteAmt", System.Data.SqlDbType.Decimal).Value = tblOrgVoucherNoteTO.VoucherNoteAmt;
            cmdInsert.Parameters.Add("@gstTaxPercentage", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrgVoucherNoteTO.GstTaxPercentage);
            cmdInsert.Parameters.Add("@gstTaxAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrgVoucherNoteTO.GstTaxAmt);
            cmdInsert.Parameters.Add("@tdsTaxPercentage", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrgVoucherNoteTO.TdsTaxPercentage);
            cmdInsert.Parameters.Add("@tdsTaxAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrgVoucherNoteTO.TdsTaxAmt);
            cmdInsert.Parameters.Add("@totalVoucherNoteAmt", System.Data.SqlDbType.Decimal).Value = tblOrgVoucherNoteTO.TotalVoucherNoteAmt;
            cmdInsert.Parameters.Add("@statusId", System.Data.SqlDbType.Int).Value = tblOrgVoucherNoteTO.StatusId;
            cmdInsert.Parameters.Add("@remark", System.Data.SqlDbType.NVarChar).Value = tblOrgVoucherNoteTO.Remark;
            cmdInsert.Parameters.Add("@narration", System.Data.SqlDbType.NVarChar).Value = tblOrgVoucherNoteTO.Narration;
            cmdInsert.Parameters.Add("@createdBy", System.Data.SqlDbType.Int).Value = tblOrgVoucherNoteTO.CreatedBy;
            cmdInsert.Parameters.Add("@createdOn", System.Data.SqlDbType.DateTime).Value = tblOrgVoucherNoteTO.CreatedOn;

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = sqlSelectIdentityQry;
                tblOrgVoucherNoteTO.IdIOrgVoucherNote = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
    }
}
