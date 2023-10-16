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
    public class TblLocationDAO : ITblLocationDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblLocationDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT loc.*, parentLoc.locationDesc AS parentLocDesc FROM tblLocation loc " +
                                  " LEFT JOIN tblLocation parentLoc ON loc.parentLocId = parentLoc.idLocation WHERE loc.isActive = 1";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblLocationTO> SelectAllTblLocation()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLocationTO> list = ConvertDTToList(sqlReader);
                if (sqlReader != null)
                    sqlReader.Dispose();
                return list;
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
        public List<TblLocationTO> SelectAllParentLocationWithConnTran(TblLocationTO tblLocationTO, SqlConnection conn, SqlTransaction tran)
        {

            SqlDataReader sqlReader = null;
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " AND loc.locationDesc = '" + tblLocationTO.LocationDesc + "'";
                if (tblLocationTO.IdLocation > 0)
                {
                    cmdSelect.CommandText += " AND loc.idLocation != " + tblLocationTO.IdLocation ;
                }
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                 sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLocationTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                // conn.Close();
                sqlReader.Close();
                cmdSelect.Dispose();
            }
        }

        public List<DropDownTO> SelectAllLocationAndCompartmentsListForDropDown(Boolean onlyCompartments = true)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                string sqlQuery = string.Empty;
                if (onlyCompartments)
                    sqlQuery = " SELECT allLoc.* ,parentLoc.locationDesc as parentLocation " +
                              " FROM tblLocation allLoc LEFT JOIN tblLocation parentLoc ON allLoc.parentLocId = parentLoc.idLocation" +
                              " WHERE allLoc.parentLocId IS NOT NULL";
                else
                    sqlQuery = " SELECT allLoc.* ,parentLoc.locationDesc as parentLocation " +
                               " FROM tblLocation allLoc LEFT JOIN tblLocation parentLoc ON allLoc.parentLocId = parentLoc.idLocation";

                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader tblLocationTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> list = new List<DropDownTO>();

                while (tblLocationTODT.Read())
                {
                    DropDownTO dropDownTO = new DropDownTO();
                    if (tblLocationTODT["idLocation"] != DBNull.Value)
                        dropDownTO.Value = Convert.ToInt32(tblLocationTODT["idLocation"].ToString());

                    string locationDesc = string.Empty;
                    string parentLlocationDesc = string.Empty;
                    if (tblLocationTODT["locationDesc"] != DBNull.Value)
                        locationDesc = Convert.ToString(tblLocationTODT["locationDesc"].ToString());
                    if (tblLocationTODT["parentLocation"] != DBNull.Value)
                        parentLlocationDesc = Convert.ToString(tblLocationTODT["parentLocation"].ToString());

                    if (tblLocationTODT["parentLocId"] != DBNull.Value)
                        dropDownTO.Tag = Convert.ToString(tblLocationTODT["parentLocId"].ToString());

                    if (string.IsNullOrEmpty(parentLlocationDesc))
                        dropDownTO.Text = locationDesc;
                    else
                        dropDownTO.Text = parentLlocationDesc + "-" + locationDesc;

                    if (tblLocationTODT["mappedTxnId"] != DBNull.Value)
                        dropDownTO.MappedTxnId = Convert.ToString(tblLocationTODT["mappedTxnId"].ToString());


                    list.Add(dropDownTO);
                }

                if (tblLocationTODT != null)
                    tblLocationTODT.Dispose();
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


        public List<TblLocationTO> SelectAllTblLocation(Int32 parentLocationId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " AND ISNULL(loc.parentLocId,0)=" + parentLocationId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLocationTO> list = ConvertDTToList(sqlReader);
                if (sqlReader != null)
                    sqlReader.Dispose();
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

        //Reshma Added 
        public List<DropDownTO> SelectLocationFromWarehouse(Int32 warehouseId)
        {
            List<DropDownTO> list = new List<DropDownTO>();
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = " select * from tblLocation where idLocation = " + warehouseId ;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader tblLocationTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLocationTO> tblLocationTOList = new List<TblLocationTO>();
                while (tblLocationTODT.Read())
                {
                    DropDownTO dropDownTO = new DropDownTO();
                    if (tblLocationTODT["idLocation"] != DBNull.Value)
                        dropDownTO.Value = Convert.ToInt32(tblLocationTODT["idLocation"].ToString());

                    string locationDesc = string.Empty;
                    string parentLlocationDesc = string.Empty;
                    if (tblLocationTODT["locationDesc"] != DBNull.Value)
                        locationDesc = Convert.ToString(tblLocationTODT["locationDesc"].ToString());
                    //if (tblLocationTODT["parentLocation"] != DBNull.Value)
                    //    parentLlocationDesc = Convert.ToString(tblLocationTODT["parentLocation"].ToString());

                    if (tblLocationTODT["parentLocId"] != DBNull.Value)
                        dropDownTO.Tag = Convert.ToString(tblLocationTODT["parentLocId"].ToString());

                    if (string.IsNullOrEmpty(parentLlocationDesc))
                        dropDownTO.Text = locationDesc;
                    else
                        dropDownTO.Text = parentLlocationDesc + "-" + locationDesc;

                    if (tblLocationTODT["mappedTxnId"] != DBNull.Value)
                        dropDownTO.MappedTxnId = Convert.ToString(tblLocationTODT["mappedTxnId"].ToString());


                    list.Add(dropDownTO);
                }

                if (tblLocationTODT != null)
                    tblLocationTODT.Dispose();
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
        public List<TblLocationTO> SelectAllParentLocation()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " AND ISNULL(loc.parentLocId,0)=0";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLocationTO> list = ConvertDTToList(sqlReader);
                if (sqlReader != null)
                    sqlReader.Dispose();
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

        public List<TblLocationTO> SelectStkNotTakenCompartmentList(DateTime stockDate)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() +
                                       //" WHERE loc.idLocation NOT IN(Select distinct locationId From tblStockDetails WHERE DAY(createdOn)=" + stockDate.Day + " AND MONTH(createdOn)=" + stockDate.Month + "  AND YEAR(createdOn)=" + stockDate.Year + ")" +

                                       " AND loc.idLocation NOT IN(Select distinct IsNull(locationId,0) From tblStockDetails WHERE totalStock > 0)" +
                                       " AND loc.parentLocId IS NOT NULL ";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLocationTO> list = ConvertDTToList(sqlReader);
                if (sqlReader != null)
                    sqlReader.Dispose();
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

        public TblLocationTO SelectTblLocation(Int32 idLocation)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " AND loc.idLocation = " + idLocation +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLocationTO> list = ConvertDTToList(reader);
                if (reader != null)
                    reader.Dispose();

                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
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

        public List<TblLocationTO> ConvertDTToList(SqlDataReader tblLocationTODT)
        {
            List<TblLocationTO> tblLocationTOList = new List<TblLocationTO>();
            if (tblLocationTODT != null)
            {
                while(tblLocationTODT.Read())
                {
                    TblLocationTO tblLocationTONew = new TblLocationTO();
                    if (tblLocationTODT["idLocation"] != DBNull.Value)
                        tblLocationTONew.IdLocation = Convert.ToInt32(tblLocationTODT["idLocation"].ToString());
                    if (tblLocationTODT["parentLocId"] != DBNull.Value)
                        tblLocationTONew.ParentLocId = Convert.ToInt32(tblLocationTODT["parentLocId"].ToString());
                    if (tblLocationTODT["createdBy"] != DBNull.Value)
                        tblLocationTONew.CreatedBy = Convert.ToInt32(tblLocationTODT["createdBy"].ToString());
                    if (tblLocationTODT["updatedBy"] != DBNull.Value)
                        tblLocationTONew.UpdatedBy = Convert.ToInt32(tblLocationTODT["updatedBy"].ToString());
                    if (tblLocationTODT["createdOn"] != DBNull.Value)
                        tblLocationTONew.CreatedOn = Convert.ToDateTime(tblLocationTODT["createdOn"].ToString());
                    if (tblLocationTODT["updatedOn"] != DBNull.Value)
                        tblLocationTONew.UpdatedOn = Convert.ToDateTime(tblLocationTODT["updatedOn"].ToString());
                    if (tblLocationTODT["locationDesc"] != DBNull.Value)
                        tblLocationTONew.LocationDesc = Convert.ToString(tblLocationTODT["locationDesc"].ToString());
                    if (tblLocationTODT["parentLocDesc"] != DBNull.Value)
                        tblLocationTONew.ParentLocationDesc = Convert.ToString(tblLocationTODT["parentLocDesc"].ToString());
                    if (tblLocationTODT["mappedTxnId"] != DBNull.Value)
                        tblLocationTONew.MappedTxnId = Convert.ToString(tblLocationTODT["mappedTxnId"].ToString());

                    if (tblLocationTODT["stateId"] != DBNull.Value)
                        tblLocationTONew.StateId = Convert.ToInt32(tblLocationTODT["stateId"].ToString());
                    if (tblLocationTODT["countryId"] != DBNull.Value)
                        tblLocationTONew.CountryId = Convert.ToInt32(tblLocationTODT["countryId"].ToString());

                    if (tblLocationTODT["stateName"] != DBNull.Value)
                        tblLocationTONew.StateName = Convert.ToString(tblLocationTODT["stateName"].ToString());
                    if (tblLocationTODT["countryName"] != DBNull.Value)
                        tblLocationTONew.CountryName = Convert.ToString(tblLocationTODT["countryName"].ToString());
                    if (tblLocationTODT["organizationId"] != DBNull.Value)
                        tblLocationTONew.OrganizationId = Convert.ToInt32(tblLocationTODT["organizationId"].ToString());
                    if (tblLocationTODT["addressId"] != DBNull.Value)
                        tblLocationTONew.IdAddress = Convert.ToInt32(tblLocationTODT["addressId"].ToString());
                    if (tblLocationTODT["isActive"] != DBNull.Value)
                        tblLocationTONew.IsActive = Convert.ToInt32(tblLocationTODT["isActive"].ToString());

                    tblLocationTOList.Add(tblLocationTONew);
                }
            }
            return tblLocationTOList;
        }

        #endregion

        #region Insertion
        public int InsertTblLocation(TblLocationTO tblLocationTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblLocationTO, cmdInsert);
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

        public int InsertTblLocation(TblLocationTO tblLocationTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblLocationTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblLocationTO tblLocationTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblLocation]( " +
                                "  [parentLocId]" +
                                " ,[createdBy]" +
                                " ,[updatedBy]" +
                                " ,[createdOn]" +
                                " ,[updatedOn]" +
                                " ,[locationDesc]" +
                                " ,[mappedTxnId]"+
                                " ,[stateId]"+
                                " ,[countryId]" +
                                " ,[stateName]"+
                                " ,[countryName]" +
                                " ,[organizationId]" +
                                 " ,[addressId]" +
                                 " ,[isActive]" +
                                " )" +
                    " VALUES (" +
                                "  @ParentLocId " +
                                " ,@CreatedBy " +
                                " ,@UpdatedBy " +
                                " ,@CreatedOn " +
                                " ,@UpdatedOn " +
                                " ,@LocationDesc " +
                                " ,@MappedTxnId" +
                                " ,@StateId"+
                                " ,@CountryId" +
                                " ,@StateName" +
                                " ,@CountryName"+
                                " ,@OrganizationId" +
                                " ,@AddressId" +
                                " ,@IsActive" +
                                " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdLocation", System.Data.SqlDbType.Int).Value = tblLocationTO.IdLocation;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLocationTO.IsActive);
            cmdInsert.Parameters.Add("@ParentLocId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLocationTO.ParentLocId);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblLocationTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLocationTO.UpdatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblLocationTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblLocationTO.UpdatedOn);
            cmdInsert.Parameters.Add("@LocationDesc", System.Data.SqlDbType.NVarChar).Value = tblLocationTO.LocationDesc;
            cmdInsert.Parameters.Add("@MappedTxnId", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLocationTO.MappedTxnId);
            cmdInsert.Parameters.Add("@StateId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLocationTO.StateId);
            cmdInsert.Parameters.Add("@CountryId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLocationTO.CountryId);
            cmdInsert.Parameters.Add("@StateName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLocationTO.StateName);
            cmdInsert.Parameters.Add("@CountryName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLocationTO.CountryName);
            cmdInsert.Parameters.Add("@OrganizationId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLocationTO.OrganizationId);
            cmdInsert.Parameters.Add("@AddressId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLocationTO.IdAddress);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblLocationTO.IdLocation = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public int UpdateTblLocation(TblLocationTO tblLocationTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblLocationTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                
               
                return 0;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public int UpdateTblLocation(TblLocationTO tblLocationTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblLocationTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                
               
                return 0;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public int ExecuteUpdationCommand(TblLocationTO tblLocationTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblLocation] SET " + 
            
            " [parentLocId]= @ParentLocId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[locationDesc] = @LocationDesc" +
            " ,[mappedTxnId] = @MappedTxnId" +
            " ,[stateId] = @StateId" +
            " ,[countryId] = @CountryId" +
            " ,[stateName] = @StateName"+
            " ,[countryName] = @CountryName"+
            " ,[organizationId] = @OrganizationId" +
             " ,[addressId] = @AdrressId" +
             " ,[isActive] = @IsActive" +
            " WHERE [idLocation] = @IdLocation"; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdLocation", System.Data.SqlDbType.Int).Value = tblLocationTO.IdLocation;
            cmdUpdate.Parameters.Add("@ParentLocId", System.Data.SqlDbType.Int).Value =Constants.GetSqlDataValueNullForBaseValue(tblLocationTO.ParentLocId);
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblLocationTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblLocationTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblLocationTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblLocationTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@LocationDesc", System.Data.SqlDbType.NVarChar).Value = tblLocationTO.LocationDesc;
            cmdUpdate.Parameters.Add("@MappedTxnId", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLocationTO.MappedTxnId);
            cmdUpdate.Parameters.Add("@StateId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLocationTO.StateId);
            cmdUpdate.Parameters.Add("@CountryId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLocationTO.CountryId);
            cmdUpdate.Parameters.Add("@StateName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLocationTO.StateName);
            cmdUpdate.Parameters.Add("@CountryName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLocationTO.CountryName);
            cmdUpdate.Parameters.Add("@OrganizationId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLocationTO.OrganizationId);
            cmdUpdate.Parameters.Add("@AdrressId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLocationTO.IdAddress);
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLocationTO.IsActive);

            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public int DeleteTblLocation(Int32 idLocation)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idLocation, cmdDelete);
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

        public int DeleteTblLocation(Int32 idLocation, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idLocation, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idLocation, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblLocation] " +
            " WHERE idLocation = " + idLocation +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idLocation", System.Data.SqlDbType.Int).Value = tblLocationTO.IdLocation;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
