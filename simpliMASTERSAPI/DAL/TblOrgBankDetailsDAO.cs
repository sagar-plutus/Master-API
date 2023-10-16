using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL.Interfaces;

namespace ODLMWebAPI.DAL
{
    public class TblOrgBankDetailsDAO : ITblOrgBankDetailsDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblOrgBankDetailsDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT bankDetails.*,bank.firmName AS bankName,currency.currencyName,accType.accountTypeName " +
                                   " FROM tblOrgBankDtls bankDetails " +
                                   " LEFT JOIN tblOrganization org ON org.idOrganization = bankDetails.orgId " +
                                   " LEFT JOIN tblOrganization bank ON bank.idOrganization = bankDetails.bankOrgId " +
                                   " LEFT JOIN dimCurrency currency on currency.idCurrency=bankDetails.currencyId " +
                                   " LEFT JOIN dimAccountType accType on accType.idAccountType=bankDetails.idAccountType ";


            return sqlSelectQry;
        }
        #endregion

        #region Selection
   

        public List<TblOrgBankDetailsTO> ConvertDTToList(SqlDataReader tblOrgBankDetailsTODT)
        {
            List<TblOrgBankDetailsTO> tblOrgBankDetailsTOList = new List<TblOrgBankDetailsTO>();
            if (tblOrgBankDetailsTODT != null)
            {
                while (tblOrgBankDetailsTODT.Read())
                {
                    TblOrgBankDetailsTO tblOrgBankDetailsTONew = new TblOrgBankDetailsTO();
                    if (tblOrgBankDetailsTODT["idOrgBankDtls"] != DBNull.Value)
                        tblOrgBankDetailsTONew.IdOrgBankDtls = Convert.ToInt32(tblOrgBankDetailsTODT["idOrgBankDtls"].ToString());
                    if (tblOrgBankDetailsTODT["orgId"] != DBNull.Value)
                        tblOrgBankDetailsTONew.OrgId = Convert.ToInt32(tblOrgBankDetailsTODT["orgId"].ToString());
                    if (tblOrgBankDetailsTODT["bankOrgId"] != DBNull.Value)
                        tblOrgBankDetailsTONew.BankOrgId = Convert.ToInt32(tblOrgBankDetailsTODT["bankOrgId"].ToString());
                    if (tblOrgBankDetailsTODT["ifscCode"] != DBNull.Value)
                        tblOrgBankDetailsTONew.IfscCode = Convert.ToString(tblOrgBankDetailsTODT["ifscCode"].ToString());
                    if (tblOrgBankDetailsTODT["accountNo"] != DBNull.Value)
                        tblOrgBankDetailsTONew.AccountNo = Convert.ToString(tblOrgBankDetailsTODT["accountNo"].ToString());
                    if (tblOrgBankDetailsTODT["nameOnCheque"] != DBNull.Value)
                        tblOrgBankDetailsTONew.NameOnCheque = Convert.ToString(tblOrgBankDetailsTODT["nameOnCheque"].ToString());
                    if (tblOrgBankDetailsTODT["isDefault"] != DBNull.Value)
                        tblOrgBankDetailsTONew.IsDefault = Convert.ToInt32(tblOrgBankDetailsTODT["isDefault"].ToString());
                    if (tblOrgBankDetailsTODT["isActive"] != DBNull.Value)
                        tblOrgBankDetailsTONew.IsActive = Convert.ToInt32(tblOrgBankDetailsTODT["isActive"].ToString());
                    if (tblOrgBankDetailsTODT["createdBy"] != DBNull.Value)
                        tblOrgBankDetailsTONew.CreatedBy = Convert.ToInt32(tblOrgBankDetailsTODT["createdBy"].ToString());
                    if (tblOrgBankDetailsTODT["updatedBy"] != DBNull.Value)
                        tblOrgBankDetailsTONew.UpdatedBy = Convert.ToInt32(tblOrgBankDetailsTODT["updatedBy"].ToString());
                    if (tblOrgBankDetailsTODT["createdOn"] != DBNull.Value)
                        tblOrgBankDetailsTONew.CreatedOn = Convert.ToDateTime(tblOrgBankDetailsTODT["createdOn"].ToString());
                    if (tblOrgBankDetailsTODT["updatedOn"] != DBNull.Value)
                        tblOrgBankDetailsTONew.UpdatedOn = Convert.ToDateTime(tblOrgBankDetailsTODT["updatedOn"].ToString());
                    if (tblOrgBankDetailsTODT["remark"] != DBNull.Value)
                        tblOrgBankDetailsTONew.Remark = Convert.ToString(tblOrgBankDetailsTODT["remark"].ToString());
                    if (tblOrgBankDetailsTODT["bankName"] != DBNull.Value)
                        tblOrgBankDetailsTONew.BankName = Convert.ToString(tblOrgBankDetailsTODT["bankName"].ToString());
                    if (tblOrgBankDetailsTODT["idAccountType"] != DBNull.Value)
                        tblOrgBankDetailsTONew.IdAccountType = Convert.ToInt32(tblOrgBankDetailsTODT["idAccountType"].ToString());
                    if (tblOrgBankDetailsTODT["activatedFromDate"] != DBNull.Value)
                        tblOrgBankDetailsTONew.ActivatedFromDate = Convert.ToDateTime(tblOrgBankDetailsTODT["activatedFromDate"].ToString());
                    if (tblOrgBankDetailsTODT["currencyId"] != DBNull.Value)
                        tblOrgBankDetailsTONew.CurrencyId = Convert.ToInt32(tblOrgBankDetailsTODT["currencyId"].ToString());
                    if (tblOrgBankDetailsTODT["swiftCode"] != DBNull.Value)
                        tblOrgBankDetailsTONew.SwiftCode = Convert.ToString(tblOrgBankDetailsTODT["swiftCode"].ToString());
                    if (tblOrgBankDetailsTODT["bgLimit"] != DBNull.Value)
                        tblOrgBankDetailsTONew.BgLimit = Convert.ToString(tblOrgBankDetailsTODT["bgLimit"].ToString());
                    if (tblOrgBankDetailsTODT["primaryAccNo"] != DBNull.Value)
                        tblOrgBankDetailsTONew.PrimaryAccNo = Convert.ToString(tblOrgBankDetailsTODT["primaryAccNo"].ToString());
                    if (tblOrgBankDetailsTODT["customerId"] != DBNull.Value)
                        tblOrgBankDetailsTONew.CustomerId = Convert.ToString(tblOrgBankDetailsTODT["customerId"].ToString());
                    if (tblOrgBankDetailsTODT["branchCode"] != DBNull.Value)
                        tblOrgBankDetailsTONew.BranchCode = Convert.ToString(tblOrgBankDetailsTODT["branchCode"].ToString());
                    if (tblOrgBankDetailsTODT["branchName"] != DBNull.Value)
                        tblOrgBankDetailsTONew.BranchName = Convert.ToString(tblOrgBankDetailsTODT["branchName"].ToString());
                    if (tblOrgBankDetailsTODT["accountTypeName"] != DBNull.Value)
                        tblOrgBankDetailsTONew.AccountTypeName = Convert.ToString(tblOrgBankDetailsTODT["accountTypeName"].ToString());
                    if (tblOrgBankDetailsTODT["currencyName"] != DBNull.Value)
                        tblOrgBankDetailsTONew.CurrencyName = Convert.ToString(tblOrgBankDetailsTODT["currencyName"].ToString());

                    tblOrgBankDetailsTOList.Add(tblOrgBankDetailsTONew);
                }
            }
            return tblOrgBankDetailsTOList;
        }


        public List<TblOrgBankDetailsTO> SelectOrgBankDetailsList(Int32 orgId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE org.idOrganization =" + orgId + " and bankDetails.isActive = 1 ";
               
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
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

        //Harshala[15-11-2019]
        public List<DropDownTO> SelectAccountTypeList()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String sqlQuery = String.Empty;
            try
            {
                conn.Open();
                sqlQuery = "select * from dimAccountType";
                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader tblOrgReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                if (tblOrgReader != null)
                {
                    while (tblOrgReader.Read())
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        if (tblOrgReader["idAccountType"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(tblOrgReader["idAccountType"].ToString());
                        if (tblOrgReader["accountTypeName"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(tblOrgReader["accountTypeName"].ToString());
                        dropDownTOList.Add(dropDownTO);
                    }
                }
                return dropDownTOList;
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
            
        public Boolean isDuplicateMobileNo(String mobileNo, Int32 type, int orgId = 0)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();

                switch (type)
                {
                    case (int)Constants.MobileDupChecktypeE.OrganizationRegMobNo:

                        cmdSelect.CommandText = " Select COUNT(*) as duplicateMobileNo from tblOrganization org " +
                                                " where org.registeredMobileNos = '" + mobileNo + "' ";
                        break;

                    case (int)Constants.MobileDupChecktypeE.ContactDetailsMobNo:

                        cmdSelect.CommandText = " Select COUNT(*) as duplicateMobileNo from tblOrganization org " +
                                               " where org.phoneNo = '" + mobileNo + "' ";
                        break;

                    case (int)Constants.MobileDupChecktypeE.PersonMobNo:
                        cmdSelect.CommandText = " Select COUNT(*) as duplicateMobileNo from tblPerson person " +
                                              " where person.mobileNo ='" + mobileNo + "' ";

                        break;

                    case (int)Constants.MobileDupChecktypeE.AltPersonMobNo:
                        cmdSelect.CommandText = "Select COUNT(*) as  duplicateMobileNo from tblPerson person " +
                       " where person.alternateMobNo= '" + mobileNo + "' ";
                        break;


                    default:
                        break;
                }

                        

                //exclude current org (used for edit calls)
                if (orgId > 0)
                {
                    String excludeCurrentOrgstr = " and idOrganization not in ( " + orgId + " ) ";
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
                        if (sqlReader["duplicateMobileNo"] != DBNull.Value)
                            count = Convert.ToInt32(sqlReader["duplicateMobileNo"].ToString());
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


        #endregion

        #region Insertion
        public int InsertTblOrgBankDetails(TblOrgBankDetailsTO tblOrgBankDetailsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblOrgBankDetailsTO, cmdInsert);
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

        public int InsertTblOrgBankDetails(TblOrgBankDetailsTO tblOrgBankDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblOrgBankDetailsTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblOrgBankDetailsTO tblOrgBankDetailsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblOrgBankDtls]( " +
                                " [orgId]" +
                                " ,[bankOrgId]" +
                                " ,[ifscCode]" +
                                " ,[accountNo]" +
                                " ,[nameOnCheque]" +
                                " ,[isDefault]" +
                                " ,[isActive]" +
                                " ,[createdBy]" +
                                " ,[updatedBy]" +
                                " ,[createdOn]" +
                                " ,[updatedOn]" +
                                " ,[idAccountType]" +
                                " ,[activatedFromDate]" +
                                " ,[currencyId]" +
                                " ,[swiftCode]" +
                                " ,[bgLimit]" +
                                " ,[primaryAccNo]" +
                                " ,[customerId]" +
                                " ,[branchCode]" +
                                " ,[branchName]" +
                                " )" +
                    " VALUES (" +
                                "  @OrgId " +
                                " ,@BankOrgId " +
                                " ,@IFSCCode " +
                                " ,@AccountNo " +
                                " ,@NameOnCheque " +
                                " ,@IsDefault " +
                                " ,@IsActive " +
                                " ,@CreatedBy " +
                                " ,@UpdatedBy " +
                                " ,@CreatedOn " +
                                " ,@UpdatedOn " +
                                " ,@IdAccountType" +
                                " ,@ActivatedFromDate" +
                                " ,@CurrencyId" +
                                " ,@SwiftCode" +
                                " ,@BgLimit" +
                                " ,@PrimaryAccNo" +
                                " ,@CustomerId" +
                                " ,@BranchCode" +
                                " ,@BranchName" +
                                " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            String sqlSelectIdentityQry = "Select @@Identity";

            //cmdInsert.Parameters.Add("@IdOrgAddr", System.Data.SqlDbType.Int).Value = tblOrgAddressTO.IdOrgAddr;
            cmdInsert.Parameters.Add("@OrgId", System.Data.SqlDbType.Int).Value = tblOrgBankDetailsTO.OrgId;
            cmdInsert.Parameters.Add("@BankOrgId", System.Data.SqlDbType.Int).Value = tblOrgBankDetailsTO.BankOrgId;
            cmdInsert.Parameters.Add("@IFSCCode", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblOrgBankDetailsTO.IfscCode);
            cmdInsert.Parameters.Add("@AccountNo", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblOrgBankDetailsTO.AccountNo);
            cmdInsert.Parameters.Add("@NameOnCheque", System.Data.SqlDbType.NVarChar).Value =  StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblOrgBankDetailsTO.NameOnCheque); 
            cmdInsert.Parameters.Add("@IsDefault", System.Data.SqlDbType.Int).Value = tblOrgBankDetailsTO.IsDefault;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblOrgBankDetailsTO.IsActive;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblOrgBankDetailsTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblOrgBankDetailsTO.UpdatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblOrgBankDetailsTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblOrgBankDetailsTO.UpdatedOn);
            cmdInsert.Parameters.Add("@ActivatedFromDate", System.Data.SqlDbType.DateTime).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblOrgBankDetailsTO.ActivatedFromDate);
            cmdInsert.Parameters.Add("@IdAccountType", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblOrgBankDetailsTO.IdAccountType);
            cmdInsert.Parameters.Add("@CurrencyId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblOrgBankDetailsTO.CurrencyId);
            cmdInsert.Parameters.Add("@SwiftCode", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblOrgBankDetailsTO.SwiftCode);
            cmdInsert.Parameters.Add("@BgLimit", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblOrgBankDetailsTO.BgLimit);
            cmdInsert.Parameters.Add("@PrimaryAccNo", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblOrgBankDetailsTO.PrimaryAccNo);
            cmdInsert.Parameters.Add("@CustomerId", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblOrgBankDetailsTO.CustomerId);
            cmdInsert.Parameters.Add("@BranchCode", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblOrgBankDetailsTO.BranchCode);
            cmdInsert.Parameters.Add("@BranchName", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblOrgBankDetailsTO.BranchName);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = sqlSelectIdentityQry;
                tblOrgBankDetailsTO.IdOrgBankDtls = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion


        #region Updation
        public int UpdateTblOrgBankDetails(TblOrgBankDetailsTO tblOrgBankDetailsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblOrgBankDetailsTO, cmdUpdate);
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

        public int UpdateTblOrgBankDetails(TblOrgBankDetailsTO tblOrgBankDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblOrgBankDetailsTO, cmdUpdate);
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
    
        public int ExecuteUpdationCommand(TblOrgBankDetailsTO tblOrgBankDetailsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblOrgBankDtls] SET " +
                             "  [orgId]= @OrgId" +
                            " , [bankOrgId]= @BankOrgId" +
                            " ,[ifscCode]= @IfscCode" +
                            " ,[accountNo]= @AccountNo" +
                            " ,[nameOnCheque]= @NameOnCheque" +
                            " ,[isDefault]= @IsDefault" +
                            " ,[isActive]= @IsActive" +
                             " ,[createdBy]= @CreatedBy" +
                              " ,[createdOn]= @CreatedOn" +
                               " ,[updatedBy]= @UpdatedBy" +
                                " ,[updatedOn]= @UpdatedOn" +
                                 " ,[activatedFromDate]= @ActivatedFromDate" +
                                  " ,[idAccountType]= @IdAccountType" +
                                   " ,[currencyId]= @CurrencyId" +
                                    " ,[swiftCode]= @SwiftCode" +
                                     " ,[bgLimit]= @BgLimit" +
                                      " ,[primaryAccNo]= @PrimaryAccNo" +
                                       " ,[customerId]= @CustomerId" +
                                        " ,[branchCode]= @BranchCode" +
                                         " ,[branchName]= @BranchName" +

                            " WHERE  [idOrgBankDtls] = @IdOrgBankDtls";



            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdOrgBankDtls", System.Data.SqlDbType.Int).Value = tblOrgBankDetailsTO.IdOrgBankDtls;
            cmdUpdate.Parameters.Add("@OrgId", System.Data.SqlDbType.Int).Value = tblOrgBankDetailsTO.OrgId;
            cmdUpdate.Parameters.Add("@BankOrgId", System.Data.SqlDbType.Int).Value = tblOrgBankDetailsTO.BankOrgId;
            cmdUpdate.Parameters.Add("@IFSCCode", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblOrgBankDetailsTO.IfscCode);
            cmdUpdate.Parameters.Add("@AccountNo", System.Data.SqlDbType.NVarChar).Value = tblOrgBankDetailsTO.AccountNo;
            cmdUpdate.Parameters.Add("@NameOnCheque", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblOrgBankDetailsTO.NameOnCheque);
            cmdUpdate.Parameters.Add("@IsDefault", System.Data.SqlDbType.Int).Value = tblOrgBankDetailsTO.IsDefault;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblOrgBankDetailsTO.IsActive;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblOrgBankDetailsTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblOrgBankDetailsTO.UpdatedBy);
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblOrgBankDetailsTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblOrgBankDetailsTO.UpdatedOn);
            cmdUpdate.Parameters.Add("@ActivatedFromDate", System.Data.SqlDbType.DateTime).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblOrgBankDetailsTO.ActivatedFromDate);
            cmdUpdate.Parameters.Add("@IdAccountType", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblOrgBankDetailsTO.IdAccountType);
            cmdUpdate.Parameters.Add("@CurrencyId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblOrgBankDetailsTO.CurrencyId);
            cmdUpdate.Parameters.Add("@SwiftCode", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblOrgBankDetailsTO.SwiftCode);
            cmdUpdate.Parameters.Add("@BgLimit", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblOrgBankDetailsTO.BgLimit);
            cmdUpdate.Parameters.Add("@PrimaryAccNo", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblOrgBankDetailsTO.PrimaryAccNo);
            cmdUpdate.Parameters.Add("@CustomerId", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblOrgBankDetailsTO.CustomerId);
            cmdUpdate.Parameters.Add("@BranchCode", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblOrgBankDetailsTO.BranchCode);
            cmdUpdate.Parameters.Add("@BranchName", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblOrgBankDetailsTO.BranchName);
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

    }
}
