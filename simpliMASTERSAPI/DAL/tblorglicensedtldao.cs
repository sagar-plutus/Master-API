using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ODLMWebAPI.Models;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.StaticStuff;
namespace ODLMWebAPI.DAL
{
    public class TblOrgLicenseDtlDAO : ITblOrgLicenseDtlDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblOrgLicenseDtlDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT orgLicDtl.*,gstType.gstTypeName, dimLic.licenseName FROM tblOrgLicenseDtl orgLicDtl " +
                                  " LEFT JOIN dimCommerLicenceInfo dimLic " +
                                  " ON orgLicDtl.licenseId = dimLic.idLicense" +
                                  " LEFT JOIN dimGstType gstType on orgLicDtl.gstTypeId = gstType.idDimGstType"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblOrgLicenseDtlTO> SelectAllTblOrgLicenseDtl(Int32 orgId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE orgLicDtl.organizationId=" + orgId;
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
                if (sqlReader != null)
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblOrgLicenseDtlTO> SelectAllTblOrgLicenseDtl(Int32 orgId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE orgLicDtl.organizationId=" + orgId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
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
                if (sqlReader != null)
                    sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }
        public List<TblOrgLicenseDtlTO> SelectAllTblOrgLicenseDtl(Int32 orgId, Int32 licenseId, String licenseVal)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE orgLicDtl.organizationId <> " + orgId + " AND orgLicDtl.licenseId=" + licenseId + " AND licenseValue='" + licenseVal + "'";
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
                if (sqlReader != null)
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public TblOrgLicenseDtlTO SelectTblOrgLicenseDtl(Int32 idOrgLicense)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idOrgLicense = " + idOrgLicense + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblOrgLicenseDtlTO> list = ConvertDTToList(sqlReader);
                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
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

        public List<TblOrgLicenseDtlTO> SelectAllOrgList()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = " select distinct organizationId from tblOrgLicenseDtl where addressId is NULL ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                //List<TblOrgLicenseDtlTO> list = ConvertDTToList(sqlReader);
                List<TblOrgLicenseDtlTO> list = new List<TblOrgLicenseDtlTO>();
                if (sqlReader != null)
                {
                    while (sqlReader.Read())
                    {
                        TblOrgLicenseDtlTO tblOrgLicenseDtlTO = new TblOrgLicenseDtlTO();
                        if (sqlReader["organizationId"] != DBNull.Value)
                            tblOrgLicenseDtlTO.OrganizationId = Convert.ToInt32(sqlReader["organizationId"].ToString());
                        list.Add(tblOrgLicenseDtlTO);
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
                if (sqlReader != null)
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        //public Boolean isDuplicateLicenseValue(Int32 licenseType , String licValue,int orgId = 0 )
        public Boolean isDuplicateLicenseValue(Int32 licenseType , String licValue, Int32 orgTypeId, int orgId = 0 )
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = " Select COUNT(*) as duplicateCount from tblOrgLicenseDtl orgLic " +
                                        "  LEFT JOIN tblOrganization tblOrganization on orgLic.organizationId =tblOrganization.idOrganization "+//Reshma[08-03-21] For Active supplier checking
                                     " where orgLic.licenseId = " + licenseType + " and orgLic.licenseValue = '"+ licValue+"' " +
                                     " and tblOrganization .isActive =1 and tblOrganization.orgTypeId = " + orgTypeId + "";
                //exclude current org (used for edit calls)
                if (orgId > 0)
                {
                    String excludeCurrentOrgstr = " and organizationId not in ( " + orgId + " ) ";
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
                        if (sqlReader["duplicateCount"] != DBNull.Value)
                            count = Convert.ToInt32(sqlReader["duplicateCount"].ToString());                       
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

        public Boolean isDuplicateLicenseValue(Int32 licenseType, String licValue, Int32 orgTypeId, SqlConnection conn,SqlTransaction tran, int orgId = 0)
        {
           
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = " Select COUNT(*) as duplicateCount from tblOrgLicenseDtl orgLic " +
                                        "  LEFT JOIN tblOrganization tblOrganization on orgLic.organizationId =tblOrganization.idOrganization " +//Reshma[08-03-21] For Active supplier checking
                                     " where orgLic.licenseId = " + licenseType + " and orgLic.licenseValue = '" + licValue + "' " +
                                     " and tblOrganization .isActive =1 and tblOrganization.orgTypeId = " + orgTypeId + "";
                //exclude current org (used for edit calls)
                if (orgId > 0)
                {
                    String excludeCurrentOrgstr = " and organizationId not in ( " + orgId + " ) ";
                    cmdSelect.CommandText = cmdSelect.CommandText + excludeCurrentOrgstr;
                }
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                int count = 0;
                if (sqlReader != null)
                {
                    while (sqlReader.Read())
                    {
                        if (sqlReader["duplicateCount"] != DBNull.Value)
                            count = Convert.ToInt32(sqlReader["duplicateCount"].ToString());
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
                cmdSelect.Dispose();
            }
        }
        public List<TblOrgLicenseDtlTO> ConvertDTToList(SqlDataReader tblOrgLicenseDtlTODT)
        {
            List<TblOrgLicenseDtlTO> tblOrgLicenseDtlTOList = new List<TblOrgLicenseDtlTO>();
            if (tblOrgLicenseDtlTODT != null)
            {
                while (tblOrgLicenseDtlTODT.Read())
                {
                    TblOrgLicenseDtlTO tblOrgLicenseDtlTONew = new TblOrgLicenseDtlTO();
                    if (tblOrgLicenseDtlTODT["idOrgLicense"] != DBNull.Value)
                        tblOrgLicenseDtlTONew.IdOrgLicense = Convert.ToInt32(tblOrgLicenseDtlTODT["idOrgLicense"].ToString());
                    if (tblOrgLicenseDtlTODT["organizationId"] != DBNull.Value)
                        tblOrgLicenseDtlTONew.OrganizationId = Convert.ToInt32(tblOrgLicenseDtlTODT["organizationId"].ToString());
                    if (tblOrgLicenseDtlTODT["licenseId"] != DBNull.Value)
                        tblOrgLicenseDtlTONew.LicenseId = Convert.ToInt32(tblOrgLicenseDtlTODT["licenseId"].ToString());
                    if (tblOrgLicenseDtlTODT["createdBy"] != DBNull.Value)
                        tblOrgLicenseDtlTONew.CreatedBy = Convert.ToInt32(tblOrgLicenseDtlTODT["createdBy"].ToString());
                    if (tblOrgLicenseDtlTODT["createdOn"] != DBNull.Value)
                        tblOrgLicenseDtlTONew.CreatedOn = Convert.ToDateTime(tblOrgLicenseDtlTODT["createdOn"].ToString());
                    if (tblOrgLicenseDtlTODT["licenseValue"] != DBNull.Value)
                        tblOrgLicenseDtlTONew.LicenseValue = Convert.ToString(tblOrgLicenseDtlTODT["licenseValue"].ToString());
                    if (tblOrgLicenseDtlTODT["licenseName"] != DBNull.Value)
                        tblOrgLicenseDtlTONew.LicenseName = Convert.ToString(tblOrgLicenseDtlTODT["licenseName"].ToString());
                    if(tblOrgLicenseDtlTODT["addressId"] != DBNull.Value)
                        tblOrgLicenseDtlTONew.AddressId = Convert.ToInt32(tblOrgLicenseDtlTODT["addressId"].ToString());
                    if (tblOrgLicenseDtlTODT["gstTypeId"] != DBNull.Value)
                        tblOrgLicenseDtlTONew.GstTypeId = Convert.ToInt32(tblOrgLicenseDtlTODT["gstTypeId"].ToString());

                    if (tblOrgLicenseDtlTODT["gstTypeName"] != DBNull.Value)
                        tblOrgLicenseDtlTONew.GstTypeName = Convert.ToString(tblOrgLicenseDtlTODT["gstTypeName"].ToString());
                    tblOrgLicenseDtlTOList.Add(tblOrgLicenseDtlTONew);
                }
            }
            return tblOrgLicenseDtlTOList;
        }

        #endregion

        #region Insertion
        public int InsertTblOrgLicenseDtl(TblOrgLicenseDtlTO tblOrgLicenseDtlTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblOrgLicenseDtlTO, cmdInsert);
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

        public int InsertTblOrgLicenseDtl(TblOrgLicenseDtlTO tblOrgLicenseDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblOrgLicenseDtlTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblOrgLicenseDtlTO tblOrgLicenseDtlTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblOrgLicenseDtl]( " +
                            "  [organizationId]" +
                            " ,[licenseId]" +
                            " ,[createdBy]" +
                            " ,[createdOn]" +
                            " ,[licenseValue]" +
                            " ,[addressId]" +
                             " ,[gstTypeId]" +
                            " )" +
                " VALUES (" +
                            "  @OrganizationId " +
                            " ,@LicenseId " +
                            " ,@CreatedBy " +
                            " ,@CreatedOn " +
                            " ,@LicenseValue " +
                            " ,@AddressId " +
                            " ,@gstTypeId " +
                            " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdOrgLicense", System.Data.SqlDbType.Int).Value = tblOrgLicenseDtlTO.IdOrgLicense;
            cmdInsert.Parameters.Add("@OrganizationId", System.Data.SqlDbType.Int).Value = tblOrgLicenseDtlTO.OrganizationId;
            cmdInsert.Parameters.Add("@LicenseId", System.Data.SqlDbType.Int).Value = tblOrgLicenseDtlTO.LicenseId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblOrgLicenseDtlTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblOrgLicenseDtlTO.CreatedOn;
            cmdInsert.Parameters.Add("@LicenseValue", System.Data.SqlDbType.NVarChar).Value = tblOrgLicenseDtlTO.LicenseValue;
            cmdInsert.Parameters.Add("@AddressId", System.Data.SqlDbType.Int).Value = tblOrgLicenseDtlTO.AddressId;
            cmdInsert.Parameters.Add("@gstTypeId", System.Data.SqlDbType.Int).Value = tblOrgLicenseDtlTO.GstTypeId;

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = ODLMWebAPI.StaticStuff.Constants.IdentityColumnQuery;
                tblOrgLicenseDtlTO.IdOrgLicense = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public int UpdateTblOrgLicenseDtl(TblOrgLicenseDtlTO tblOrgLicenseDtlTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblOrgLicenseDtlTO, cmdUpdate);
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

        public int UpdateTblOrgLicenseDtl(TblOrgLicenseDtlTO tblOrgLicenseDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblOrgLicenseDtlTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(TblOrgLicenseDtlTO tblOrgLicenseDtlTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblOrgLicenseDtl] SET " + 
                            "  [organizationId]= @OrganizationId" +
                            " ,[licenseId]= @LicenseId" +
                            " ,[licenseValue] = @LicenseValue" +
                            " ,[addressId] = @AddressId" +
                            " ,[gstTypeId] = @gstTypeId" +

                            " WHERE [idOrgLicense] = @IdOrgLicense"; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdOrgLicense", System.Data.SqlDbType.Int).Value = tblOrgLicenseDtlTO.IdOrgLicense;
            cmdUpdate.Parameters.Add("@OrganizationId", System.Data.SqlDbType.Int).Value = tblOrgLicenseDtlTO.OrganizationId;
            cmdUpdate.Parameters.Add("@LicenseId", System.Data.SqlDbType.Int).Value = tblOrgLicenseDtlTO.LicenseId;
            cmdUpdate.Parameters.Add("@LicenseValue", System.Data.SqlDbType.NVarChar).Value = tblOrgLicenseDtlTO.LicenseValue;
            cmdUpdate.Parameters.Add("@AddressId", System.Data.SqlDbType.Int).Value = tblOrgLicenseDtlTO.AddressId;
            cmdUpdate.Parameters.Add("@gstTypeId", System.Data.SqlDbType.Int).Value = tblOrgLicenseDtlTO.GstTypeId;

            return cmdUpdate.ExecuteNonQuery();
        }

        public int UpdateOrgLicense(TblOrgLicenseDtlTO tblOrgLicenseDtlTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;

                String sqlQuery = @" UPDATE [tblOrgLicenseDtl] SET " +
                           " [addressId] = @AddressId" +
                           " WHERE [organizationId] = @OrganizationId AND ISNULL(addressId,0) = 0 ";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@OrganizationId", System.Data.SqlDbType.Int).Value = tblOrgLicenseDtlTO.OrganizationId;
                cmdUpdate.Parameters.Add("@AddressId", System.Data.SqlDbType.Int).Value = tblOrgLicenseDtlTO.AddressId;

                return cmdUpdate.ExecuteNonQuery();


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


        #endregion

        #region Deletion
        public int DeleteTblOrgLicenseDtl(Int32 idOrgLicense)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idOrgLicense, cmdDelete);
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

        public int DeleteTblOrgLicenseDtl(Int32 idOrgLicense, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idOrgLicense, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idOrgLicense, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblOrgLicenseDtl] " +
            " WHERE idOrgLicense = " + idOrgLicense +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idOrgLicense", System.Data.SqlDbType.Int).Value = tblOrgLicenseDtlTO.IdOrgLicense;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
