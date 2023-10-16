using ODLMWebAPI.BL.Interfaces;
using simpliMASTERSAPI.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using simpliMASTERSAPI.Models;
using System.Data.SqlClient;
using ODLMWebAPI.StaticStuff;
using System.Data;

namespace simpliMASTERSAPI.DAL
{
    public class TblOrgAccountTaxDAO : ITblOrgAccountTaxDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblOrgAccountTaxDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }

        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " select tblOrgAccountTax.*,dimAssesseType.assesseeName as assesseeTypeName,finLedger.ledgerName as accountPayableName,finLedger.ledgerCode as accountPayableCode,finLedger1.ledgerName as clearingAccountName,finLedger1.ledgerCode as clearingAccountCode,finLedger2.ledgerName as interimAccountName,finLedger2.ledgerCode as interimAccountCode" +
                                   " from tblOrgAccountTax tblOrgAccountTax " +
                                    " LEFT JOIN tblOrganization org ON org.idOrganization = tblOrgAccountTax.organizationId " +
                                   " LEFT JOIN  dimAssesseType dimAssesseType on dimAssesseType.idAssesseeType = tblOrgAccountTax.assesseeTypeId" +
                                   " LEFT JOIN tblFinLedger finLedger on finLedger.idFinLedger = tblOrgAccountTax.accountPayable " +
                                   " LEFT JOIN tblFinLedger finLedger1 on finLedger1.idFinLedger = tblOrgAccountTax.clearingAccount" +
                                   " LEFT JOIN tblFinLedger finLedger2 on finLedger2.idFinLedger = tblOrgAccountTax.interimAccount";


            return sqlSelectQry;
        }
        #endregion
        #region Selection


        public TblOrgAccountTaxTO SelectOrgAccountTaxList(Int32 orgId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE org.idOrganization =" + orgId ;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                if(reader == null) { return null; }
                return ConvertDTToList(reader);
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
        public TblOrgAccountTaxTO ConvertDTToList(SqlDataReader tblOrgAccountTaxTODT)
        {
            
            TblOrgAccountTaxTO tblOrgAccountTaxTONew = new TblOrgAccountTaxTO();

            if (tblOrgAccountTaxTODT != null)
            {
                while (tblOrgAccountTaxTODT.Read())
                {
                    if (tblOrgAccountTaxTODT["idOrgAccountTax"] != DBNull.Value)
                        tblOrgAccountTaxTONew.IdOrgAccountTax = Convert.ToInt32(tblOrgAccountTaxTODT["idOrgAccountTax"].ToString());

                    if (tblOrgAccountTaxTODT["organizationId"] != DBNull.Value)
                        tblOrgAccountTaxTONew.OrganizationId = Convert.ToInt32(tblOrgAccountTaxTODT["organizationId"].ToString());

                    if (tblOrgAccountTaxTODT["assesseeTypeId"] != DBNull.Value)
                        tblOrgAccountTaxTONew.AssesseeTypeId = Convert.ToInt32(tblOrgAccountTaxTODT["assesseeTypeId"].ToString());

                    if (tblOrgAccountTaxTODT["assesseeTypeName"] != DBNull.Value)
                        tblOrgAccountTaxTONew.AssesseeTypeName = Convert.ToString(tblOrgAccountTaxTODT["assesseeTypeName"].ToString());

                    if (tblOrgAccountTaxTODT["subjectToHoldingTax"] != DBNull.Value)
                        tblOrgAccountTaxTONew.SubjectToHoldingTax = Convert.ToBoolean(tblOrgAccountTaxTODT["subjectToHoldingTax"].ToString());

                    if (tblOrgAccountTaxTODT["accrual"] != DBNull.Value)
                        tblOrgAccountTaxTONew.Accrual = Convert.ToBoolean(tblOrgAccountTaxTODT["accrual"].ToString());

                    if (tblOrgAccountTaxTODT["cash"] != DBNull.Value)
                        tblOrgAccountTaxTONew.Cash = Convert.ToBoolean(tblOrgAccountTaxTODT["cash"].ToString());

                    if (tblOrgAccountTaxTODT["threasholdOverlook"] != DBNull.Value)
                        tblOrgAccountTaxTONew.ThreasholdOverlook = Convert.ToBoolean(tblOrgAccountTaxTODT["threasholdOverlook"].ToString());

                    if (tblOrgAccountTaxTODT["surchargeOverlook"] != DBNull.Value)
                        tblOrgAccountTaxTONew.SurchargeOverlook = Convert.ToBoolean(tblOrgAccountTaxTODT["surchargeOverlook"].ToString());

                    if (tblOrgAccountTaxTODT["cerificateNo"] != DBNull.Value)
                        tblOrgAccountTaxTONew.CerificateNo = Convert.ToString(tblOrgAccountTaxTODT["cerificateNo"].ToString());

                    if (tblOrgAccountTaxTODT["certificateExpiryDate"] != DBNull.Value)
                        tblOrgAccountTaxTONew.CertificateExpiryDate = Convert.ToDateTime(tblOrgAccountTaxTODT["certificateExpiryDate"].ToString());

                    if (tblOrgAccountTaxTODT["nINumber"] != DBNull.Value)
                        tblOrgAccountTaxTONew.NINumber = Convert.ToString(tblOrgAccountTaxTODT["nINumber"].ToString());

                    if (tblOrgAccountTaxTODT["wtTaxCategory"] != DBNull.Value)
                        tblOrgAccountTaxTONew.WtTaxCategory = Convert.ToString(tblOrgAccountTaxTODT["wtTaxCategory"].ToString());

                    if (tblOrgAccountTaxTODT["accountPayable"] != DBNull.Value)
                        tblOrgAccountTaxTONew.AccountPayable = Convert.ToInt32(tblOrgAccountTaxTODT["accountPayable"].ToString());

                    if (tblOrgAccountTaxTODT["accountPayableName"] != DBNull.Value)
                        tblOrgAccountTaxTONew.AccountPayableName = Convert.ToString(tblOrgAccountTaxTODT["accountPayableName"].ToString());

                    if (tblOrgAccountTaxTODT["accountPayableCode"] != DBNull.Value)
                        tblOrgAccountTaxTONew.AccountPayableCode = Convert.ToString(tblOrgAccountTaxTODT["accountPayableCode"].ToString());

                    if (tblOrgAccountTaxTODT["clearingAccount"] != DBNull.Value)
                        tblOrgAccountTaxTONew.ClearingAccount = Convert.ToInt32(tblOrgAccountTaxTODT["clearingAccount"].ToString());

                    if (tblOrgAccountTaxTODT["clearingAccountName"] != DBNull.Value)
                        tblOrgAccountTaxTONew.ClearingAccountName = Convert.ToString(tblOrgAccountTaxTODT["clearingAccountName"].ToString());

                    if (tblOrgAccountTaxTODT["clearingAccountCode"] != DBNull.Value)
                        tblOrgAccountTaxTONew.ClearingAccountCode = Convert.ToString(tblOrgAccountTaxTODT["clearingAccountCode"].ToString());


                    if (tblOrgAccountTaxTODT["interimAccount"] != DBNull.Value)
                        tblOrgAccountTaxTONew.InterimAccount = Convert.ToInt32(tblOrgAccountTaxTODT["interimAccount"].ToString());


                    if (tblOrgAccountTaxTODT["interimAccountName"] != DBNull.Value)
                        tblOrgAccountTaxTONew.InterimAccountName = Convert.ToString(tblOrgAccountTaxTODT["interimAccountName"].ToString());


                    if (tblOrgAccountTaxTODT["interimAccountCode"] != DBNull.Value)
                        tblOrgAccountTaxTONew.InterimAccountCode = Convert.ToString(tblOrgAccountTaxTODT["interimAccountCode"].ToString());

                    if (tblOrgAccountTaxTODT["remark1"] != DBNull.Value)
                        tblOrgAccountTaxTONew.Remark1 = Convert.ToInt32(tblOrgAccountTaxTODT["remark1"].ToString());

                    if (tblOrgAccountTaxTODT["certificateFor26Q"] != DBNull.Value)
                        tblOrgAccountTaxTONew.CertificateFor26Q = Convert.ToString(tblOrgAccountTaxTODT["certificateFor26Q"].ToString());
              
                }
            }
            return tblOrgAccountTaxTONew;
        }
        #endregion

        #region Insertion
        public int InsertTblOrgAccountTax(TblOrgAccountTaxTO orgAccTaxTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(orgAccTaxTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblOrgAccountTaxTO orgAccTaxTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [TblOrgAccountTax]( " +
                            "  [organizationId]" +
                            " ,[assesseeTypeId]" +
                            " ,[subjectToHoldingTax]" +
                            " ,[accrual]" +
                            " ,[cash]" +
                            " ,[threasholdOverlook]" +
                            " ,[surchargeOverlook]" +
                            " ,[cerificateNo]" +
                            " ,[certificateExpiryDate]" +
                            " ,[nINumber]" +
                            " ,[wtTaxCategory]" +
                            " ,[accountPayable]" +
                            " ,[clearingAccount]" +
                            " ,[interimAccount]" +
                            " ,[remark1]" +
                            " ,[certificateFor26Q]" +

                            " )" +
                " VALUES (" +
                            "  @organizationId " +
                            " ,@assesseeTypeId " +
                            " ,@subjectToHoldingTax " +
                            " ,@accrual " +
                            " ,@cash " +
                            " ,@threasholdOverlook " +
                            " ,@surchargeOverlook " +
                            " ,@cerificateNo " +
                            " ,@certificateExpiryDate " +
                            " ,@nINumber " +
                            " ,@wtTaxCategory " +
                            " ,@accountPayable " +
                            " ,@clearingAccount " +
                            " ,@interimAccount " +
                            " ,@remark1 " +
                            " ,@certificateFor26Q " +
                            " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@organizationId", System.Data.SqlDbType.Int).Value = orgAccTaxTO.OrganizationId;
            cmdInsert.Parameters.Add("@assesseeTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(orgAccTaxTO.AssesseeTypeId);
            cmdInsert.Parameters.Add("@subjectToHoldingTax", System.Data.SqlDbType.Bit).Value = Constants.GetSqlDataValueNullForBaseValue(orgAccTaxTO.SubjectToHoldingTax);
            cmdInsert.Parameters.Add("@accrual", System.Data.SqlDbType.Bit).Value = Constants.GetSqlDataValueNullForBaseValue(orgAccTaxTO.Accrual);
            cmdInsert.Parameters.Add("@cash", System.Data.SqlDbType.Bit).Value = Constants.GetSqlDataValueNullForBaseValue(orgAccTaxTO.Cash);
            cmdInsert.Parameters.Add("@threasholdOverlook", System.Data.SqlDbType.Bit).Value = Constants.GetSqlDataValueNullForBaseValue(orgAccTaxTO.ThreasholdOverlook);
            cmdInsert.Parameters.Add("@surchargeOverlook", System.Data.SqlDbType.Bit).Value = Constants.GetSqlDataValueNullForBaseValue(orgAccTaxTO.SurchargeOverlook);
            cmdInsert.Parameters.Add("@cerificateNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(orgAccTaxTO.CerificateNo);
            cmdInsert.Parameters.Add("@certificateExpiryDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(orgAccTaxTO.CertificateExpiryDate);
            cmdInsert.Parameters.Add("@nINumber", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(orgAccTaxTO.NINumber);
            cmdInsert.Parameters.Add("@wtTaxCategory", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(orgAccTaxTO.WtTaxCategory);
            cmdInsert.Parameters.Add("@accountPayable", System.Data.SqlDbType.Int).Value = orgAccTaxTO.AccountPayable;
            cmdInsert.Parameters.Add("@clearingAccount", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(orgAccTaxTO.ClearingAccount);
            cmdInsert.Parameters.Add("@interimAccount", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(orgAccTaxTO.InterimAccount);
            cmdInsert.Parameters.Add("@remark1", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(orgAccTaxTO.Remark1);
            cmdInsert.Parameters.Add("@certificateFor26Q", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(orgAccTaxTO.CertificateFor26Q);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                orgAccTaxTO.IdOrgAccountTax = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }

        #endregion


        #region Updation
        public int UpdateTblOrgAccountTax(TblOrgAccountTaxTO orgAccTaxTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(orgAccTaxTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(TblOrgAccountTaxTO orgAccTaxTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [TblOrgAccountTax] SET " +
                             "  [organizationId]= @organizationId" +
                            " , [assesseeTypeId]= @assesseeTypeId" +
                            " ,[subjectToHoldingTax]= @subjectToHoldingTax" +
                            " ,[accrual]= @accrual" +
                            " ,[cash]= @cash" +
                            " ,[threasholdOverlook]= @threasholdOverlook" +
                            " ,[surchargeOverlook]= @surchargeOverlook" +
                             " ,[cerificateNo]= @cerificateNo" +
                              " ,[certificateExpiryDate]= @certificateExpiryDate" +
                               " ,[nINumber]= @nINumber" +
                                " ,[wtTaxCategory]= @wtTaxCategory" +
                                 " ,[accountPayable]= @accountPayable" +
                                  " ,[clearingAccount]= @clearingAccount" +
                                   " ,[interimAccount]= @interimAccount" +
                                    " ,[remark1]= @remark1" +
                                     " ,[certificateFor26Q]= @certificateFor26Q" +



                            " WHERE  [idOrgAccountTax] = @idOrgAccountTax";



            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@organizationId", System.Data.SqlDbType.Int).Value = orgAccTaxTO.OrganizationId;
            cmdUpdate.Parameters.Add("@assesseeTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(orgAccTaxTO.AssesseeTypeId);
            cmdUpdate.Parameters.Add("@subjectToHoldingTax", System.Data.SqlDbType.Bit).Value = Constants.GetSqlDataValueNullForBaseValue(orgAccTaxTO.SubjectToHoldingTax);
            cmdUpdate.Parameters.Add("@accrual", System.Data.SqlDbType.Bit).Value = Constants.GetSqlDataValueNullForBaseValue(orgAccTaxTO.Accrual);
            cmdUpdate.Parameters.Add("@cash", System.Data.SqlDbType.Bit).Value = Constants.GetSqlDataValueNullForBaseValue(orgAccTaxTO.Cash);
            cmdUpdate.Parameters.Add("@threasholdOverlook", System.Data.SqlDbType.Bit).Value = Constants.GetSqlDataValueNullForBaseValue(orgAccTaxTO.ThreasholdOverlook);
            cmdUpdate.Parameters.Add("@surchargeOverlook", System.Data.SqlDbType.Bit).Value = Constants.GetSqlDataValueNullForBaseValue(orgAccTaxTO.SurchargeOverlook);
            cmdUpdate.Parameters.Add("@cerificateNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(orgAccTaxTO.CerificateNo);
            cmdUpdate.Parameters.Add("@certificateExpiryDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(orgAccTaxTO.CertificateExpiryDate);
            cmdUpdate.Parameters.Add("@nINumber", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(orgAccTaxTO.NINumber);
            cmdUpdate.Parameters.Add("@wtTaxCategory", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(orgAccTaxTO.WtTaxCategory);
            cmdUpdate.Parameters.Add("@accountPayable", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(orgAccTaxTO.AccountPayable);
            cmdUpdate.Parameters.Add("@clearingAccount", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(orgAccTaxTO.ClearingAccount);
            cmdUpdate.Parameters.Add("@interimAccount", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(orgAccTaxTO.InterimAccount);
            cmdUpdate.Parameters.Add("@remark1", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(orgAccTaxTO.Remark1);
            cmdUpdate.Parameters.Add("@certificateFor26Q", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(orgAccTaxTO.CertificateFor26Q);

            cmdUpdate.Parameters.Add("@idOrgAccountTax", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(orgAccTaxTO.IdOrgAccountTax);



            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
    }
}
