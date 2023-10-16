using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL.Interfaces;
using simpliMASTERSAPI.Models;
using ODLMWebAPI.BL;
using ICSharpCode.SharpZipLib.Zip;
using simpliMASTERSAPI.DAL.Interfaces;
using System.Drawing;

namespace simpliMASTERSAPI.DAL
{
    public class TblWishListDtlsDAO : ITblWishListDtlsDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblWishListDtlsDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }

        #region  Wishlist Test 
        public int ExecuteInsertionWishlistCommand(TblWishListDtlsTO tblWishListDtlsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblWishlists]( " +
                                "  [userId]" +
                                " ,[wishlist]" +
                                " ,[createdDate]" +
                                " ,[isDelete]" +
                                " )" +
                    " VALUES (" +
                                "  @userId" +
                                " ,@wishlist" +
                                " ,@createdDate" +
                                " ,@isDelete" +
                                " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@useId", System.Data.SqlDbType.Int).Value = tblWishListDtlsTO.UserId;
            cmdInsert.Parameters.Add("@userId", System.Data.SqlDbType.Int).Value = tblWishListDtlsTO.UserId;
            cmdInsert.Parameters.Add("@wishlist", System.Data.SqlDbType.NVarChar).Value = tblWishListDtlsTO.Wishlist;
            cmdInsert.Parameters.Add("@createdDate", System.Data.SqlDbType.DateTime).Value = DateTime.Now;
            cmdInsert.Parameters.Add("@isDelete", System.Data.SqlDbType.Bit).Value = false;
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblWishListDtlsTO.UserId = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            return 0;
        }

        public int InsertTblWishlistDtls(TblWishListDtlsTO tblWishListDtlsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionWishlistCommand(tblWishListDtlsTO, cmdInsert);
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

        public List<TblWishListDtlsTO> ConvertDTToListWishlist(SqlDataReader tblWishListDtlsTODT)
        {
            List<TblWishListDtlsTO> tblWishListDtlsTOList = new List<TblWishListDtlsTO>();
            if (tblWishListDtlsTODT != null)
            {
                while (tblWishListDtlsTODT.Read())
                {
                    TblWishListDtlsTO tblWishListDtlsTONew = new TblWishListDtlsTO();
                    if (tblWishListDtlsTODT["wishlistId"] != DBNull.Value)
                        tblWishListDtlsTONew.WishlistId = Convert.ToInt32(tblWishListDtlsTODT["wishlistId"].ToString());
                    if (tblWishListDtlsTODT["userId"] != DBNull.Value)
                        tblWishListDtlsTONew.UserId = Convert.ToInt32(tblWishListDtlsTODT["userId"].ToString());
                    if (tblWishListDtlsTODT["wishlist"] != DBNull.Value)
                        tblWishListDtlsTONew.Wishlist = Convert.ToString(tblWishListDtlsTODT["wishlist"].ToString());
                    if (tblWishListDtlsTODT["createdDate"] != DBNull.Value)
                        tblWishListDtlsTONew.CreatedDate = Convert.ToDateTime(tblWishListDtlsTODT["createdDate"].ToString());
                    if (tblWishListDtlsTODT["isDelete"] != DBNull.Value)
                        tblWishListDtlsTONew.IsDelete = Convert.ToBoolean(tblWishListDtlsTODT["isDelete"].ToString());

                    tblWishListDtlsTOList.Add(tblWishListDtlsTONew);
                }
            }
            return tblWishListDtlsTOList;
        }

        public List<TblWishListDtlsTO> SelectTblWishListDtls(Int32 userId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                if (userId > 0)
                {
                    cmdSelect.CommandText = "SELECT * FROM [tblWishlists] WHERE ISNULL(isDelete,0) = 0 AND UserId = " + userId + " ";
                }
                else
                {
                    cmdSelect.CommandText = "SELECT * FROM [tblWishlists] WHERE ISNULL(isDelete,0) = 0 ";
                }
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblWishListDtlsTO> list = ConvertDTToListWishlist(reader);
                if (list != null && list.Count != 0)
                    return list;

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public TblWishListDtlsTO SelectTblWishListDtlsById(Int32 wishlistId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = "SELECT * FROM [tblWishlists] WHERE  wishlistId = " + wishlistId + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblWishListDtlsTO> list = ConvertDTToListWishlist(reader);
                if (list != null && list.Count == 1)
                    return list[0];

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }
        public int ExecuteUpdationCommand(TblWishListDtlsTO tblWishListDtlsTO, SqlCommand cmdUpdate)
        {
            string sqlQuery = "";
            if (tblWishListDtlsTO.IsDelete == true)
            {
                sqlQuery = @" UPDATE [tblWishlists] SET " +
                           "  [IsDelete] = @IsDelete" +
                           "  WHERE [wishlistId] = " + tblWishListDtlsTO.WishlistId + " AND [UserId] = " + tblWishListDtlsTO.UserId + " ";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@wishlistId", System.Data.SqlDbType.Int).Value = tblWishListDtlsTO.WishlistId;
                cmdUpdate.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = tblWishListDtlsTO.UserId;
                cmdUpdate.Parameters.Add("@IsDelete", System.Data.SqlDbType.Bit).Value = tblWishListDtlsTO.IsDelete;
            }
            else
            {
                sqlQuery = @" UPDATE [tblWishlists] SET " +
                           "  [Wishlist] = @Wishlist" +
                           "  WHERE [wishlistId] = " + tblWishListDtlsTO.WishlistId + " AND [UserId] = " + tblWishListDtlsTO.UserId + " ";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@wishlist", System.Data.SqlDbType.VarChar).Value = tblWishListDtlsTO.Wishlist;
                cmdUpdate.Parameters.Add("@wishlistId", System.Data.SqlDbType.Int).Value = tblWishListDtlsTO.WishlistId;
                cmdUpdate.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = tblWishListDtlsTO.UserId;
            }
            return cmdUpdate.ExecuteNonQuery();
        }

        public int UpdatetblWishListDtl(TblWishListDtlsTO tblWishListDtls)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblWishListDtls, cmdUpdate);
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

        public int DeleteTblWishListDtl(TblWishListDtlsTO tblWishListDtls)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblWishListDtls, cmdUpdate);
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

        public List<TblUserTO> ConvertDTToListUserWishlist(SqlDataReader tblUserTODT)
        {
            List<TblUserTO> tblUserTOList = new List<TblUserTO>();
            if (tblUserTODT != null)
            {
                while (tblUserTODT.Read())
                {
                    TblUserTO tblUserTONew = new TblUserTO();
                    if (tblUserTODT["idUser"] != DBNull.Value)
                        tblUserTONew.IdUser = Convert.ToInt32(tblUserTODT["idUser"].ToString());
                    if (tblUserTODT["userLogin"] != DBNull.Value)
                        tblUserTONew.UserLogin = Convert.ToString(tblUserTODT["userLogin"].ToString());

                    DataTable dt = tblUserTODT.GetSchemaTable();
                    foreach (DataRow item in dt.Rows)
                    {
                        string ColumnName = item.ItemArray[0].ToString();
                        if (ColumnName == "TotalCount")
                        {
                            if (tblUserTODT["TotalCount"] != DBNull.Value)
                                tblUserTONew.TotalCount = Convert.ToInt32(tblUserTODT["TotalCount"].ToString());
                        }
                        if (ColumnName == "RowNumber")
                        {
                            if (tblUserTODT["rowNumber"] != DBNull.Value)
                                tblUserTONew.RowNumber = Convert.ToInt32(tblUserTODT["rowNumber"].ToString());
                        }
                        if (ColumnName == "WishlistCount")
                        {
                            if (tblUserTODT["wishlistCount"] != DBNull.Value)
                                tblUserTONew.WishlistCount = Convert.ToInt32(tblUserTODT["wishlistCount"].ToString());
                        }
                    }
                    tblUserTOList.Add(tblUserTONew);
                }
            }
            return tblUserTOList;
        }

        public List<TblUserTO> SelectTblUserDtls(Int32 PageNumber, Int32 RowsPerPage, String strsearchtxt, Int32 UserWishlistId, SqlConnection conn, SqlTransaction tran)
        {
            if (strsearchtxt == null)
            { strsearchtxt = "''"; }

            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                if (UserWishlistId == 1)
                {
                    cmdSelect.CommandText = "SELECT * FROM tblUser_All  WHERE idUser IN (SELECT DIstinct(userId) FROM tblWishlists) ";
                }
                else
                {
                    cmdSelect.CommandText = "select * from (select COUNT(*) OVER () as TotalCount,ROW_NUMBER() over (order by idUser ) as RowNumber, * from ( " +

                    //Query for wishlist added user
                    "SELECT tblUser.idUser,tblUser.userLogin, count(tblWishlists.userId) as WishlistCount from tblUser_All as tblUser " +
                    "LEFT JOIN  tblWishlists  on tblUser.idUser = tblWishlists.userId  " +
                    "AND ISNULL(tblWishlists.isDelete,0) = 0 " +
                    "WHERE tblUser.IsActive = 1 " +
                    "GROUP BY tblUser.userLogin,tblUser.idUser" +

                    //Query for all user
                    //"SELECT tblUser.idUser,tblUser.userLogin, count(tblWishlists.userId) as WishlistCount from tblUser_All " +
                    //"LEFT JOIN  tblWishlists  on tblUser.idUser = tblWishlists.userId  " +
                    //"WHERE ISNULL(tblWishlists.isDelete,0) = 0 " +
                    //"GROUP BY tblUser.userLogin,tblUser.idUser" +

                    ")as tbl1  where( ((" + strsearchtxt + " = '') or (tbl1.userLogin like '%' + " + strsearchtxt + " + '%')) " +

                    "))as tbl2 where (" + RowsPerPage + " = 0 " +
                    "or tbl2.RowNumber between ((" + PageNumber + " - 1) * " + RowsPerPage + " + 1) and (" + PageNumber + " * " + RowsPerPage + "))" +
                    "ORDER BY tbl2.idUser";
                }
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblUserTO> list = ConvertDTToListUserWishlist(reader);
                if (list != null && list.Count != 0)
                    return list;

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }


        #endregion

        public List<TblUserTO> ConvertDTToListWishlistDetails(SqlDataReader tblUserTODT)
        {
            List<TblUserTO> tblUserTOList = new List<TblUserTO>();
            if (tblUserTODT != null)
            {
                while (tblUserTODT.Read())
                {
                    TblUserTO tblUserTONew = new TblUserTO();
                    if (tblUserTODT["idUser"] != DBNull.Value)
                        tblUserTONew.IdUser = Convert.ToInt32(tblUserTODT["idUser"].ToString());
                    if (tblUserTODT["userLogin"] != DBNull.Value)
                        tblUserTONew.UserLogin = Convert.ToString(tblUserTODT["userLogin"].ToString());

                    tblUserTOList.Add(tblUserTONew);
                }
            }
            return tblUserTOList;
        }

        public List<TblUserTO> SelectAllUserWishlistDetailsToExport(Int32 userId,string userWishlistIds)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            string sqlQuery = string.Empty;

            try
            {
                conn.Open();
                if (userId > 0)
                {
                    sqlQuery = "SELECT idUser,userLogin FROM tblUser_All  WHERE idUser IN (SELECT DIstinct(userId) FROM tblWishlists WHERE ISNULL(isDelete,0) = 0) ";

                }
                else
                {
                    sqlQuery = "SELECT idUser, userLogin FROM tblUser_All  WHERE idUser IN(SELECT DIstinct(userId) FROM tblWishlists WHERE ISNULL(isDelete, 0) = 0 and userId IN(" + userWishlistIds + ")) ";

                }

                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblUserTO> list = ConvertDTToListWishlistDetails(reader);

                return list;
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

        public List<TblWishListDtlsTO> ConvertReaderToList(SqlDataReader tblWishListDtlsTODT)
        {
            List<TblWishListDtlsTO> userWishExportRptTOList = new List<TblWishListDtlsTO>();
            if (tblWishListDtlsTODT != null)
            {
                while (tblWishListDtlsTODT.Read())
                {
                    TblWishListDtlsTO tblWishListDtlsTONew = new TblWishListDtlsTO();
                    if (tblWishListDtlsTODT["userId"] != DBNull.Value)
                        tblWishListDtlsTONew.UserId = Convert.ToInt32(tblWishListDtlsTODT["userId"].ToString());
                    if (tblWishListDtlsTODT["wishlistId"] != DBNull.Value)
                        tblWishListDtlsTONew.WishlistId = Convert.ToInt32(tblWishListDtlsTODT["wishlistId"].ToString());
                    if (tblWishListDtlsTODT["wishlist"] != DBNull.Value)
                        tblWishListDtlsTONew.Wishlist = Convert.ToString(tblWishListDtlsTODT["wishlist"].ToString());
                    if (tblWishListDtlsTODT["createdDate"] != DBNull.Value)
                        tblWishListDtlsTONew.CreatedDate = Convert.ToDateTime(tblWishListDtlsTODT["createdDate"].ToString());

                    Constants.SetNullValuesToEmpty(tblWishListDtlsTONew);
                    userWishExportRptTOList.Add(tblWishListDtlsTONew);
                }
            }
            return userWishExportRptTOList;
        }


        #region Export to excel data
        public List<TblWishListDtlsTO> SelectAllUserWishlistDetailsToExport(string userWishlistIds)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            string sqlQuery = string.Empty;

            try
            {
                conn.Open();
                if (userWishlistIds != null)
                {
                    sqlQuery = "SELECT userId,wishlistId,wishlist,createdDate FROM tblWishlists WHERE userId IN (SELECT DIstinct(userId) FROM tblWishlists WHERE ISNULL(isDelete,0) = 0 and userId IN (" + userWishlistIds + ")) AND isDelete = 0";
                }
                else
                {
                    sqlQuery = "SELECT userId,wishlistId,wishlist,createdDate  FROM tblWishlists WHERE userId IN (SELECT DIstinct(userId) FROM tblWishlists WHERE ISNULL(isDelete,0) = 0) AND isDelete = 0";

                }
                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblWishListDtlsTO> list = ConvertReaderToList(rdr);
                return list;
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
        public List<DimOrgTypeTO> ConvertDTToList(SqlDataReader dimOrgTypeTODT)
        {
            List<DimOrgTypeTO> dimOrgTypeTOList = new List<DimOrgTypeTO>();
            if (dimOrgTypeTODT != null)
            {
                while (dimOrgTypeTODT.Read())
                {
                    DimOrgTypeTO dimOrgTypeTONew = new DimOrgTypeTO();
                    if (dimOrgTypeTODT["idOrgType"] != DBNull.Value)
                        dimOrgTypeTONew.IdOrgType = Convert.ToInt32(dimOrgTypeTODT["idOrgType"].ToString());
                    if (dimOrgTypeTODT["isSystem"] != DBNull.Value)
                        dimOrgTypeTONew.IsSystem = Convert.ToInt32(dimOrgTypeTODT["isSystem"].ToString());
                    if (dimOrgTypeTODT["isActive"] != DBNull.Value)
                        dimOrgTypeTONew.IsActive = Convert.ToInt32(dimOrgTypeTODT["isActive"].ToString());
                    if (dimOrgTypeTODT["createUserYn"] != DBNull.Value)
                        dimOrgTypeTONew.CreateUserYn = Convert.ToInt32(dimOrgTypeTODT["createUserYn"].ToString());
                    if (dimOrgTypeTODT["defaultRoleId"] != DBNull.Value)
                        dimOrgTypeTONew.DefaultRoleId = Convert.ToInt32(dimOrgTypeTODT["defaultRoleId"].ToString());
                    if (dimOrgTypeTODT["OrgType"] != DBNull.Value)
                        dimOrgTypeTONew.OrgType = Convert.ToString(dimOrgTypeTODT["OrgType"].ToString());
                    if (dimOrgTypeTODT["isOwnerMandatory"] != DBNull.Value)
                        dimOrgTypeTONew.IsOwnerMandatory = Convert.ToInt32(dimOrgTypeTODT["isOwnerMandatory"].ToString());
                    if (dimOrgTypeTODT["isBankMandatory"] != DBNull.Value)
                        dimOrgTypeTONew.IsBankMandatory = Convert.ToInt32(dimOrgTypeTODT["isBankMandatory"].ToString());

                    if (dimOrgTypeTODT["isAddressMandatory"] != DBNull.Value)
                        dimOrgTypeTONew.IsAddressMandatory = Convert.ToInt32(dimOrgTypeTODT["isAddressMandatory"].ToString());

                    if (dimOrgTypeTODT["isKycYn"] != DBNull.Value)
                        dimOrgTypeTONew.IsKycYn = Convert.ToInt32(dimOrgTypeTODT["isKycYn"].ToString());
                    if (dimOrgTypeTODT["isKycMandatory"] != DBNull.Value)
                        dimOrgTypeTONew.IsKycMandatory = Convert.ToInt32(dimOrgTypeTODT["isKycMandatory"].ToString());
                    if (dimOrgTypeTODT["IsTransferToSAP"] != DBNull.Value)
                        dimOrgTypeTONew.IsTransferToSAP = Convert.ToInt32(dimOrgTypeTODT["IsTransferToSAP"].ToString());
                    if (dimOrgTypeTODT["exportRptTemplateName"] != DBNull.Value)
                        dimOrgTypeTONew.ExportRptTemplateName = Convert.ToString(dimOrgTypeTODT["exportRptTemplateName"].ToString());
                    if (dimOrgTypeTODT["isSendAPKLink"] != DBNull.Value)
                        dimOrgTypeTONew.IsSendAPKLink = Convert.ToInt32(dimOrgTypeTODT["isSendAPKLink"].ToString());

                    // ExportRptTemplateName
                    dimOrgTypeTOList.Add(dimOrgTypeTONew);
                }
            }
            return dimOrgTypeTOList;
        }
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [dimOrgType]";
            return sqlSelectQry;
        }
        public DimOrgTypeTO SelectDimOrgType(Int32 idOrgType, SqlConnection conn, SqlTransaction tran)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idOrgType = " + idOrgType + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimOrgTypeTO> list = ConvertDTToList(rdr);
                if (list != null && list.Count == 1)
                    return list[0];

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (rdr != null) rdr.Dispose();
                cmdSelect.Dispose();
            }
        }
        public List<DimReportTemplateTO> ConvertDTToReportList(SqlDataReader dimReportTemplateTODT)
        {
            List<DimReportTemplateTO> dimReportTemplateTOList = new List<DimReportTemplateTO>();
            if (dimReportTemplateTODT != null)
            {
                while (dimReportTemplateTODT.Read())
                {
                    DimReportTemplateTO dimReportTemplateTONew = new DimReportTemplateTO();
                    if (dimReportTemplateTODT["idReport"] != DBNull.Value)
                        dimReportTemplateTONew.IdReport = Convert.ToInt32(dimReportTemplateTODT["idReport"].ToString());
                    if (dimReportTemplateTODT["isDisplayMultisheetAllow"] != DBNull.Value)
                        dimReportTemplateTONew.IsDisplayMultisheetAllow = Convert.ToInt32(dimReportTemplateTODT["isDisplayMultisheetAllow"].ToString());
                    if (dimReportTemplateTODT["createdBy"] != DBNull.Value)
                        dimReportTemplateTONew.CreatedBy = Convert.ToInt32(dimReportTemplateTODT["createdBy"].ToString());
                    if (dimReportTemplateTODT["createdOn"] != DBNull.Value)
                        dimReportTemplateTONew.CreatedOn = Convert.ToDateTime(dimReportTemplateTODT["createdOn"].ToString());
                    if (dimReportTemplateTODT["reportName"] != DBNull.Value)
                        dimReportTemplateTONew.ReportName = Convert.ToString(dimReportTemplateTODT["reportName"].ToString());
                    if (dimReportTemplateTODT["reportFileName"] != DBNull.Value)
                        dimReportTemplateTONew.ReportFileName = Convert.ToString(dimReportTemplateTODT["reportFileName"].ToString());
                    if (dimReportTemplateTODT["reportFileExtension"] != DBNull.Value)
                        dimReportTemplateTONew.ReportFileExtension = Convert.ToString(dimReportTemplateTODT["reportFileExtension"].ToString());
                    if (dimReportTemplateTODT["reportPassword"] != DBNull.Value)
                        dimReportTemplateTONew.ReportPassword = Convert.ToString(dimReportTemplateTODT["reportPassword"].ToString());
                    dimReportTemplateTOList.Add(dimReportTemplateTONew);
                }
            }
            return dimReportTemplateTOList;
        }
        public DimReportTemplateTO SelectDimReportTemplate(String reportName)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM dimReportTemplate WHERE reportName = '" + reportName + "' ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@report_name", System.Data.SqlDbType.NVarChar).Value = mstReportTemplateTO.ReportName;
                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimReportTemplateTO> list = ConvertDTToReportList(rdr);
                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
            }
            catch (Exception ex)
            {
                //String computerName = System.Windows.Forms.SystemInformation.ComputerName;
                //String userName = System.Windows.Forms.SystemInformation.UserName;
                //MessageBox.Show("Computer Name:" + computerName + Environment.NewLine + "User Name:" + userName + Environment.NewLine + "Class Name: MstReportTemplateDAO" + Environment.NewLine + "Method Name:SelectMstReportTemplate(MstReportTemplateTO mstReportTemplateTO)" + Environment.NewLine + "Exception Message:" + ex.Message.ToString() + "");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public List<TblConfigParamsTO> ConvertDTToListConfig(SqlDataReader tblConfigParamsTODT)
        {
            List<TblConfigParamsTO> tblConfigParamsTOList = new List<TblConfigParamsTO>();
            if (tblConfigParamsTODT != null)
            {
                while (tblConfigParamsTODT.Read())
                {
                    TblConfigParamsTO tblConfigParamsTONew = new TblConfigParamsTO();
                    if (tblConfigParamsTODT["idConfigParam"] != DBNull.Value)
                        tblConfigParamsTONew.IdConfigParam = Convert.ToInt32(tblConfigParamsTODT["idConfigParam"].ToString());
                    if (tblConfigParamsTODT["configParamValType"] != DBNull.Value)
                        tblConfigParamsTONew.ConfigParamValType = Convert.ToInt32(tblConfigParamsTODT["configParamValType"].ToString());
                    if (tblConfigParamsTODT["createdOn"] != DBNull.Value)
                        tblConfigParamsTONew.CreatedOn = Convert.ToDateTime(tblConfigParamsTODT["createdOn"].ToString());
                    if (tblConfigParamsTODT["configParamName"] != DBNull.Value)
                        tblConfigParamsTONew.ConfigParamName = Convert.ToString(tblConfigParamsTODT["configParamName"].ToString());
                    if (tblConfigParamsTODT["configParamVal"] != DBNull.Value)
                        tblConfigParamsTONew.ConfigParamVal = Convert.ToString(tblConfigParamsTODT["configParamVal"].ToString());
                    if (tblConfigParamsTODT["moduleId"] != DBNull.Value)

                        tblConfigParamsTONew.ModuleId = Convert.ToInt32(tblConfigParamsTODT["moduleId"].ToString());
                    ///Hrishikesh[27 - 03 - 2018] Added
                    if (tblConfigParamsTODT["isActive"] != DBNull.Value)
                        tblConfigParamsTONew.IsActive = Convert.ToInt32(Convert.ToString(tblConfigParamsTODT["isActive"]));

                    if (tblConfigParamsTODT["configParamDisplayVal"] != DBNull.Value)
                        tblConfigParamsTONew.ConfigParamDisplayVal = Convert.ToString(tblConfigParamsTODT["configParamDisplayVal"].ToString());

                    if (tblConfigParamsTONew.ConfigParamVal == "1")
                    {
                        tblConfigParamsTONew.BooleanFlag = "ON";
                        tblConfigParamsTONew.Slider = true;
                    }
                    else
                    {
                        tblConfigParamsTONew.BooleanFlag = "OFF";
                        tblConfigParamsTONew.Slider = false;
                    }
                    tblConfigParamsTOList.Add(tblConfigParamsTONew);
                }
            }
            return tblConfigParamsTOList;
        }
        public String SqlSelectConfigQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblConfigParams]";
            return sqlSelectQry;
        }
        public TblConfigParamsTO SelectTblConfigParamsValByName(string configParamName)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectConfigQuery() + " WHERE 1=1 "
                    + "and configParamName = '" + configParamName + "'";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblConfigParamsTO> list = ConvertDTToListConfig(sqlReader);
                TblConfigParamsTO tblConfigParamsTO = new TblConfigParamsTO();
                if (list.Count > 0)
                {
                    tblConfigParamsTO = list[0];
                }
                sqlReader.Dispose();
                return tblConfigParamsTO;
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
        #endregion
    }
}
