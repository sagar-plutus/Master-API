using ODLMWebAPI.BL.Interfaces;
using simpliMASTERSAPI.DAL.Interfaces;
using simpliMASTERSAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ODLMWebAPI.StaticStuff;
using System.Data;

namespace simpliMASTERSAPI.DAL
{
    public class TblOrgAccountTaxDtlsDAO : ITblOrgAccountTaxDtlsDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblOrgAccountTaxDtlsDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " select tblOrgAccountTaxDtls.* from tblOrgAccountTaxDtls tblOrgAccountTaxDtls" +
                                    " LEFT JOIN tblOrgAccountTax tblOrgAccountTax on tblOrgAccountTax.idOrgAccountTax = tblOrgAccountTaxDtls.orgAccountTaxId ";
                                   
            return sqlSelectQry;
        }
        #endregion
        #region Selection

        public List<TblOrgAccountTaxDtlsTO> SelectOrgAccountTaxDtlsList(Int32 orgAccountTaxId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE tblOrgAccountTaxDtls.isActive=1 and tblOrgAccountTaxDtls.orgAccountTaxId =" + orgAccountTaxId;

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
        public List<TblOrgAccountTaxDtlsTO> ConvertDTToList(SqlDataReader tblOrgAccountTaxDtlsTODT)
        {
            List<TblOrgAccountTaxDtlsTO> tblOrgAccountTaxDtlsTOList = new List<TblOrgAccountTaxDtlsTO>();
            if (tblOrgAccountTaxDtlsTODT != null)
            {
                while (tblOrgAccountTaxDtlsTODT.Read())
                {
                    TblOrgAccountTaxDtlsTO tblOrgAccountTaxDtlsTONew = new TblOrgAccountTaxDtlsTO();
                    if (tblOrgAccountTaxDtlsTODT["orgAccountTaxDtlId"] != DBNull.Value)
                        tblOrgAccountTaxDtlsTONew.IdOrgAccountTaxDtl = Convert.ToInt32(tblOrgAccountTaxDtlsTODT["orgAccountTaxDtlId"].ToString());

                    if (tblOrgAccountTaxDtlsTODT["orgAccountTaxId"] != DBNull.Value)
                        tblOrgAccountTaxDtlsTONew.OrgAccountTaxId = Convert.ToInt32(tblOrgAccountTaxDtlsTODT["orgAccountTaxId"].ToString());

                    if (tblOrgAccountTaxDtlsTODT["withholdTaxId"] != DBNull.Value)
                        tblOrgAccountTaxDtlsTONew.WithholdTaxId = Convert.ToInt32(tblOrgAccountTaxDtlsTODT["withholdTaxId"].ToString());

                    if (tblOrgAccountTaxDtlsTODT["isActive"] != DBNull.Value)
                        tblOrgAccountTaxDtlsTONew.IsActive = Convert.ToBoolean(tblOrgAccountTaxDtlsTODT["isActive"].ToString());

                    tblOrgAccountTaxDtlsTOList.Add(tblOrgAccountTaxDtlsTONew);
                }
            }
            return tblOrgAccountTaxDtlsTOList;
        }
        #endregion

        #region Insertion
        public int InsertTblOrgAccountTaxDtls(TblOrgAccountTaxDtlsTO orgAccTaxDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(orgAccTaxDtlsTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblOrgAccountTaxDtlsTO orgAccTaxDtlsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblOrgAccountTaxDtls]( " +
                            "  [orgAccountTaxId]" +
                            " ,[withholdTaxId]" +
                             " ,[isActive]" +
                            " )" +
                " VALUES (" +
                            "  @orgAccountTaxId " +
                            " ,@withholdTaxId " +
                            " ,@isActive " +
                            " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@orgAccountTaxId", System.Data.SqlDbType.Int).Value = orgAccTaxDtlsTO.OrgAccountTaxId;
            cmdInsert.Parameters.Add("@withholdTaxId", System.Data.SqlDbType.Int).Value = orgAccTaxDtlsTO.WithholdTaxId;
            cmdInsert.Parameters.Add("@isActive", System.Data.SqlDbType.Bit).Value = 1;

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                orgAccTaxDtlsTO.IdOrgAccountTaxDtl = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion

        #region Updation
        public int UpdateTblOrgAccountTaxDtls(TblOrgAccountTaxTO orgAccTaxTO, SqlConnection conn, SqlTransaction tran)
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
            String sqlQuery = @" UPDATE [tblOrgAccountTaxDtls] SET " +
                             "  [isActive]= @isActive" +

                            " WHERE  [orgAccountTaxId] = @orgAccountTaxId";



            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@isActive", System.Data.SqlDbType.Bit).Value =0;
            cmdUpdate.Parameters.Add("@orgAccountTaxId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(orgAccTaxTO.IdOrgAccountTax);

            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
    }
}
