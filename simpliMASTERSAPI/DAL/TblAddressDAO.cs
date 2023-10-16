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
    public class TblAddressDAO : ITblAddressDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblAddressDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT addr.*, dimGstType.gstTypeName,orgAddr.addrTypeId,orgAddr.idOrgAddr,tal.talukaName,dist.districtName,stat.stateName,stat.stateCode,country.countryCode,country.countryName " +
                                  " FROM tblAddress addr " +
                                  " LEFT JOIN dimTaluka tal ON tal.idTaluka = addr.talukaId " +
                                  " LEFT JOIN dimDistrict dist ON dist.idDistrict = addr.districtId " +
                                  " LEFT JOIN dimState stat ON stat.idState = addr.stateId " +
                                  " LEFT JOIN dimCountry country on country.idCountry=addr.countryId " +
                                  " LEFT JOIN dimGstType dimGstType ON addr.gstTypeId = dimGstType.idDimGstType "+
                                  " LEFT JOIN tblOrgAddress orgAddr ON addr.idAddr = orgAddr.addressId " +
                                  " Where addr.isAddrVisible = 1";

            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblAddressTO> SelectAllTblAddress()
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
                List<TblAddressTO> list = ConvertDTToList(sqlReader);
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

        public TblAddressTO SelectTblAddress(Int32 idAddr)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " And idAddr = " + idAddr +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblAddressTO> list = ConvertDTToList(reader);
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
        //Priyanka [19-08-2019]
        public TblAddressTO SelectTblAddress(Int32 idAddr, SqlConnection conn,SqlTransaction tran)
        {
            SqlDataReader reader = null;
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " AND idAddr = " + idAddr + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
              
                List<TblAddressTO> list = ConvertDTToList(reader);
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
                if (reader != null)
                    reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public TblAddressTO SelectOrgAddressWrtAddrType(Int32 orgId,StaticStuff.Constants.AddressTypeE addressTypeE,SqlConnection conn = null,SqlTransaction tran = null)
        {
            SqlCommand cmdSelect = new SqlCommand();
            int addressTypeId = (int)addressTypeE;
            SqlDataReader reader = null;
            Boolean isConnection = false;
            try
            {
                if (conn != null)
                {
                    cmdSelect.Connection = conn;
                    cmdSelect.Transaction = tran;
                    isConnection = true;
                }
                else
                {
                    String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
                    conn = new SqlConnection(sqlConnStr);
                    cmdSelect.Connection = conn;
                    conn.Open();
                }
                cmdSelect.CommandText =SqlSelectQuery() + 
                                      
                                        " and organizationId=" + orgId + " AND addrTypeId=" + addressTypeId;

                //cmdSelect.Connection = conn;
                //cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblAddressTO> list = ConvertDTToList(reader);
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
                if (reader != null)
                    reader.Dispose();
                cmdSelect.Dispose();
                if(!isConnection)
                    conn.Close();
            }
        }

        public List<TblAddressTO> SelectOrgAddressList(Int32 orgId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()  +
                                       
                                        " and organizationId=" + orgId;

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

        public List<TblAddressTO> SelectDefaultOrgAddressList(Int32 orgId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT addr.*, orgAddr.addrTypeId,dimGstType.gstTypeName,orgAddr.idOrgAddr,tal.talukaName,dist.districtName,stat.stateName,stat.stateCode,cnt.countryCode,cnt.countryName " +
                                  " FROM tblAddress addr " +
                                  " LEFT JOIN dimTaluka tal ON tal.idTaluka = addr.talukaId " +
                                  " LEFT JOIN dimCountry cnt ON cnt.idcountry = addr.countryId"+
                                  " LEFT JOIN dimDistrict dist ON dist.idDistrict = addr.districtId " +
                                  " LEFT JOIN dimState stat ON stat.idState = addr.stateId " +
                                  " LEFT JOIN dimCountry country on country.idCountry=addr.countryId "+
                                    " LEFT JOIN tblOrgAddress orgAddr ON addr.idAddr = orgAddr.addressId " +

                                  " LEFT JOIN tblOrganization tblOrganization ON addr.idAddr = tblOrganization.addrId " +
                                  " LEFT JOIN dimGstType dimGstType ON dimGstType.idDimGstType = addr.gstTypeId " +
                                  " Where addr.isAddrVisible = 1 and tblOrganization.idOrganization = " + orgId;




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

        public List<TblAddressTO> ConvertDTToList(SqlDataReader tblAddressTODT)
        {
            List<TblAddressTO> tblAddressTOList = new List<TblAddressTO>();
            if (tblAddressTODT != null)
            {
                while (tblAddressTODT.Read())
                {
                    TblAddressTO tblAddressTONew = new TblAddressTO();
                    if (tblAddressTODT["idAddr"] != DBNull.Value)
                        tblAddressTONew.IdAddr = Convert.ToInt32(tblAddressTODT["idAddr"].ToString());
                    if (tblAddressTODT["talukaId"] != DBNull.Value)
                        tblAddressTONew.TalukaId = Convert.ToInt32(tblAddressTODT["talukaId"].ToString());
                    if (tblAddressTODT["districtId"] != DBNull.Value)
                        tblAddressTONew.DistrictId = Convert.ToInt32(tblAddressTODT["districtId"].ToString());
                    if (tblAddressTODT["stateId"] != DBNull.Value)
                        tblAddressTONew.StateId = Convert.ToInt32(tblAddressTODT["stateId"].ToString());
                    if (tblAddressTODT["countryId"] != DBNull.Value)
                        tblAddressTONew.CountryId = Convert.ToInt32(tblAddressTODT["countryId"].ToString());
                    if (tblAddressTODT["pincode"] != DBNull.Value)
                        tblAddressTONew.Pincode = Convert.ToInt32(tblAddressTODT["pincode"].ToString());
                    if (tblAddressTODT["createdBy"] != DBNull.Value)
                        tblAddressTONew.CreatedBy = Convert.ToInt32(tblAddressTODT["createdBy"].ToString());
                    if (tblAddressTODT["createdOn"] != DBNull.Value)
                        tblAddressTONew.CreatedOn = Convert.ToDateTime(tblAddressTODT["createdOn"].ToString());
                    if (tblAddressTODT["plotNo"] != DBNull.Value)
                        tblAddressTONew.PlotNo = Convert.ToString(tblAddressTODT["plotNo"].ToString());
                    if (tblAddressTODT["streetName"] != DBNull.Value)
                        tblAddressTONew.StreetName = Convert.ToString(tblAddressTODT["streetName"].ToString());
                    if (tblAddressTODT["areaName"] != DBNull.Value)
                        tblAddressTONew.AreaName = Convert.ToString(tblAddressTODT["areaName"].ToString());
                    if (tblAddressTODT["villageName"] != DBNull.Value)
                        tblAddressTONew.VillageName = Convert.ToString(tblAddressTODT["villageName"].ToString());
                    if (tblAddressTODT["comments"] != DBNull.Value)
                        tblAddressTONew.Comments = Convert.ToString(tblAddressTODT["comments"].ToString());
                    if (tblAddressTODT["talukaName"] != DBNull.Value)
                        tblAddressTONew.TalukaName = Convert.ToString(tblAddressTODT["talukaName"].ToString());
                    if (tblAddressTODT["districtName"] != DBNull.Value)
                        tblAddressTONew.DistrictName = Convert.ToString(tblAddressTODT["districtName"].ToString());
                    if (tblAddressTODT["stateName"] != DBNull.Value)
                        tblAddressTONew.StateName = Convert.ToString(tblAddressTODT["stateName"].ToString());
                    if (tblAddressTODT["stateCode"] != DBNull.Value)
                        tblAddressTONew.StateCode = Convert.ToString(tblAddressTODT["stateCode"].ToString());
                    if (tblAddressTODT["countryCode"] != DBNull.Value)
                        tblAddressTONew.CountryCode = Convert.ToString(tblAddressTODT["countryCode"].ToString());
                    if (tblAddressTODT["countryName"] != DBNull.Value)
                        tblAddressTONew.CountryName = Convert.ToString(tblAddressTODT["countryName"].ToString());
                    if (tblAddressTODT["addrTypeId"] != DBNull.Value)
                        tblAddressTONew.AddrTypeId = Convert.ToInt32(tblAddressTODT["addrTypeId"].ToString());
                    if (tblAddressTODT["idOrgAddr"] != DBNull.Value)
                        tblAddressTONew.IdOrgAddr = Convert.ToInt32(tblAddressTODT["idOrgAddr"].ToString());

                    if (tblAddressTODT["gstTypeId"] != DBNull.Value)
                        tblAddressTONew.GstTypeId = Convert.ToInt32(tblAddressTODT["gstTypeId"].ToString());

                    if (tblAddressTODT["gstTypeName"] != DBNull.Value)
                        tblAddressTONew.GstTypeName = tblAddressTODT["gstTypeName"].ToString();

                    if (tblAddressTODT["isAddrVisible"] != DBNull.Value)
                        tblAddressTONew.IsActive = Convert.ToInt32(tblAddressTODT["isAddrVisible"].ToString()); 

                    tblAddressTOList.Add(tblAddressTONew);
                }
            }
            return tblAddressTOList;
        }

        /// <summary>
        /// [2017-11-17] Vijaymala:Added To get organization address list of particular type;
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="addressTypeE"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public List <TblAddressTO> SelectOrgAddressDetailOfRegion(string orgId, StaticStuff.Constants.AddressTypeE addressTypeE, SqlConnection conn, SqlTransaction tran)
        {
           
            SqlCommand cmdSelect = new SqlCommand();
            int addressTypeId = (int)addressTypeE;
            SqlDataReader reader = null;

            try
            {
                cmdSelect.CommandText = SqlSelectQuery()  +
                                        
                                        " and organizationId in (" + orgId + ") AND addrTypeId=" + addressTypeId;

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
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
                cmdSelect.Dispose();
               
            }
        }

        //Added By Gokul
        public List<TblBookingScheduleTO> SelectAllTblBookingScheduleList(Int32 bookingId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE bookingId =" + bookingId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader tblBookingScheduleTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingScheduleTO> tblBookingScheduleTOList = new List<TblBookingScheduleTO>();
                if (tblBookingScheduleTODT != null)
                {
                    while (tblBookingScheduleTODT.Read())
                    {
                        TblBookingScheduleTO tblBookingScheduleTONew = new TblBookingScheduleTO();
                        if (tblBookingScheduleTODT["idSchedule"] != DBNull.Value)
                            tblBookingScheduleTONew.IdSchedule = Convert.ToInt32(tblBookingScheduleTODT["idSchedule"].ToString());
                        if (tblBookingScheduleTODT["bookingId"] != DBNull.Value)
                            tblBookingScheduleTONew.BookingId = Convert.ToInt32(tblBookingScheduleTODT["bookingId"].ToString());
                        if (tblBookingScheduleTODT["createdBy"] != DBNull.Value)
                            tblBookingScheduleTONew.CreatedBy = Convert.ToInt32(tblBookingScheduleTODT["createdBy"].ToString());
                        if (tblBookingScheduleTODT["updatedBy"] != DBNull.Value)
                            tblBookingScheduleTONew.UpdatedBy = Convert.ToInt32(tblBookingScheduleTODT["updatedBy"].ToString());
                        if (tblBookingScheduleTODT["scheduleDate"] != DBNull.Value)
                            tblBookingScheduleTONew.ScheduleDate = Convert.ToDateTime(tblBookingScheduleTODT["scheduleDate"].ToString());
                        if (tblBookingScheduleTODT["createdOn"] != DBNull.Value)
                            tblBookingScheduleTONew.CreatedOn = Convert.ToDateTime(tblBookingScheduleTODT["createdOn"].ToString());
                        if (tblBookingScheduleTODT["updatedOn"] != DBNull.Value)
                            tblBookingScheduleTONew.UpdatedOn = Convert.ToDateTime(tblBookingScheduleTODT["updatedOn"].ToString());
                        if (tblBookingScheduleTODT["Qty"] != DBNull.Value)
                            tblBookingScheduleTONew.Qty = Convert.ToDouble(tblBookingScheduleTODT["Qty"].ToString());
                        if (tblBookingScheduleTODT["remark"] != DBNull.Value)
                            tblBookingScheduleTONew.Remark = Convert.ToString(tblBookingScheduleTODT["remark"].ToString());
                        if (tblBookingScheduleTODT["loadingLayerId"] != DBNull.Value)
                            tblBookingScheduleTONew.LoadingLayerId = Convert.ToInt32(tblBookingScheduleTODT["loadingLayerId"].ToString());
                        if (tblBookingScheduleTODT["loadingLayerDesc"] != DBNull.Value)
                            tblBookingScheduleTONew.LoadingLayerDesc = Convert.ToString(tblBookingScheduleTODT["loadingLayerDesc"].ToString());
                        if (tblBookingScheduleTODT["noOfLayers"] != DBNull.Value)
                            tblBookingScheduleTONew.NoOfLayers = Convert.ToInt32(tblBookingScheduleTODT["noOfLayers"].ToString());
                        if (tblBookingScheduleTODT["scheduleGroupId"] != DBNull.Value)
                            tblBookingScheduleTONew.ScheduleGroupId = Convert.ToInt32(tblBookingScheduleTODT["scheduleGroupId"].ToString());
                        if (tblBookingScheduleTODT["isItemized"] != DBNull.Value)
                            tblBookingScheduleTONew.IsItemized = Convert.ToInt32(tblBookingScheduleTODT["isItemized"]);
                        tblBookingScheduleTONew.ScheduleDateStr = tblBookingScheduleTONew.ScheduleDate.ToString("dd/MMM/yyyy");

                        tblBookingScheduleTOList.Add(tblBookingScheduleTONew);
                    }
                }
                tblBookingScheduleTODT.Dispose();
                return tblBookingScheduleTOList;
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

        public List<TblBookingDelAddrTO> SelectAllTblBookingDelAddrListBySchedule(int scheduleId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE scheduleId=" + scheduleId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader tblBookingDelAddrTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                //List<TblBookingDelAddrTO> list = ConvertDTToList(sqlReader);
                //return list;

                List<TblBookingDelAddrTO> tblBookingDelAddrTOList = new List<TblBookingDelAddrTO>();
                if (tblBookingDelAddrTODT != null)
                {
                    while (tblBookingDelAddrTODT.Read())
                    {
                        TblBookingDelAddrTO tblBookingDelAddrTONew = new TblBookingDelAddrTO();
                        if (tblBookingDelAddrTODT["idBookingDelAddr"] != DBNull.Value)
                            tblBookingDelAddrTONew.IdBookingDelAddr = Convert.ToInt32(tblBookingDelAddrTODT["idBookingDelAddr"].ToString());
                        if (tblBookingDelAddrTODT["bookingId"] != DBNull.Value)
                            tblBookingDelAddrTONew.BookingId = Convert.ToInt32(tblBookingDelAddrTODT["bookingId"].ToString());
                        if (tblBookingDelAddrTODT["pincode"] != DBNull.Value)
                            tblBookingDelAddrTONew.Pincode = Convert.ToInt32(tblBookingDelAddrTODT["pincode"].ToString());
                        if (tblBookingDelAddrTODT["address"] != DBNull.Value)
                            tblBookingDelAddrTONew.Address = Convert.ToString(tblBookingDelAddrTODT["address"].ToString());
                        if (tblBookingDelAddrTODT["villageName"] != DBNull.Value)
                            tblBookingDelAddrTONew.VillageName = Convert.ToString(tblBookingDelAddrTODT["villageName"].ToString());
                        if (tblBookingDelAddrTODT["talukaName"] != DBNull.Value)
                            tblBookingDelAddrTONew.TalukaName = Convert.ToString(tblBookingDelAddrTODT["talukaName"].ToString());
                        if (tblBookingDelAddrTODT["districtName"] != DBNull.Value)
                            tblBookingDelAddrTONew.DistrictName = Convert.ToString(tblBookingDelAddrTODT["districtName"].ToString());
                        if (tblBookingDelAddrTODT["comment"] != DBNull.Value)
                            tblBookingDelAddrTONew.Comment = Convert.ToString(tblBookingDelAddrTODT["comment"].ToString());
                        if (tblBookingDelAddrTODT["state"] != DBNull.Value)
                            tblBookingDelAddrTONew.State = Convert.ToString(tblBookingDelAddrTODT["state"].ToString());
                        if (tblBookingDelAddrTODT["country"] != DBNull.Value)
                            tblBookingDelAddrTONew.Country = Convert.ToString(tblBookingDelAddrTODT["country"].ToString());
                        if (tblBookingDelAddrTODT["billingName"] != DBNull.Value)
                            tblBookingDelAddrTONew.BillingName = Convert.ToString(tblBookingDelAddrTODT["billingName"].ToString());
                        if (tblBookingDelAddrTODT["gstNo"] != DBNull.Value)
                            tblBookingDelAddrTONew.GstNo = Convert.ToString(tblBookingDelAddrTODT["gstNo"].ToString());
                        if (tblBookingDelAddrTODT["contactNo"] != DBNull.Value)
                            tblBookingDelAddrTONew.ContactNo = Convert.ToString(tblBookingDelAddrTODT["contactNo"].ToString());
                        if (tblBookingDelAddrTODT["stateId"] != DBNull.Value)
                            tblBookingDelAddrTONew.StateId = Convert.ToInt32(tblBookingDelAddrTODT["stateId"].ToString());
                        if (tblBookingDelAddrTODT["txnAddrTypeId"] != DBNull.Value)
                            tblBookingDelAddrTONew.TxnAddrTypeId = Convert.ToInt32(tblBookingDelAddrTODT["txnAddrTypeId"].ToString());
                        if (tblBookingDelAddrTODT["panNo"] != DBNull.Value)
                            tblBookingDelAddrTONew.PanNo = Convert.ToString(tblBookingDelAddrTODT["panNo"].ToString());
                        if (tblBookingDelAddrTODT["aadharNo"] != DBNull.Value)
                            tblBookingDelAddrTONew.AadharNo = Convert.ToString(tblBookingDelAddrTODT["aadharNo"].ToString());
                        if (tblBookingDelAddrTODT["addrSourceTypeId"] != DBNull.Value)
                            tblBookingDelAddrTONew.AddrSourceTypeId = Convert.ToInt32(tblBookingDelAddrTODT["addrSourceTypeId"].ToString());

                        //Vijaymala Added [13-12-2017]
                        if (tblBookingDelAddrTODT["scheduleId"] != DBNull.Value)
                            tblBookingDelAddrTONew.ScheduleId = Convert.ToInt32(tblBookingDelAddrTODT["scheduleId"].ToString());

                        if (tblBookingDelAddrTODT["billingOrgId"] != DBNull.Value)
                            tblBookingDelAddrTONew.BillingOrgId = Convert.ToInt32(tblBookingDelAddrTODT["billingOrgId"].ToString());

                        //Vijaymala [18-11-2018]Added
                        if (tblBookingDelAddrTODT["loadingLayerId"] != DBNull.Value)
                            tblBookingDelAddrTONew.LoadingLayerId = Convert.ToInt32(tblBookingDelAddrTODT["loadingLayerId"].ToString());
                        tblBookingDelAddrTOList.Add(tblBookingDelAddrTONew);
                    }
                }
                return tblBookingDelAddrTOList;

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

        #region Insertion
        public int InsertTblAddress(TblAddressTO tblAddressTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblAddressTO, cmdInsert);
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

        public int InsertTblAddress(TblAddressTO tblAddressTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblAddressTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblAddressTO tblAddressTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblAddress]( " + 
                                "  [talukaId]" +
                                " ,[districtId]" +
                                " ,[stateId]" +
                                " ,[countryId]" +
                                " ,[pincode]" +
                                " ,[createdBy]" +
                                " ,[createdOn]" +
                                " ,[plotNo]" +
                                " ,[streetName]" +
                                " ,[areaName]" +
                                " ,[villageName]" +
                                " ,[comments]" +
                                " ,[isAddrVisible]" +
                                " ,[gstTypeId]" +
                                " )" +
                    " VALUES (" +
                                "  @TalukaId " +
                                " ,@DistrictId " +
                                " ,@StateId " +
                                " ,@CountryId " +
                                " ,@Pincode " +
                                " ,@CreatedBy " +
                                " ,@CreatedOn " +
                                " ,@PlotNo " +
                                " ,@StreetName " +
                                " ,@AreaName " +
                                " ,@VillageName " +
                                " ,@Comments " +
                                " ,@isActive " +
                                " ,@GstTypeId " +
                                " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdAddr", System.Data.SqlDbType.Int).Value = tblAddressTO.IdAddr;
            cmdInsert.Parameters.Add("@TalukaId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblAddressTO.TalukaId);
            cmdInsert.Parameters.Add("@DistrictId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblAddressTO.DistrictId);
            cmdInsert.Parameters.Add("@StateId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblAddressTO.StateId);
            cmdInsert.Parameters.Add("@CountryId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblAddressTO.CountryId);
            cmdInsert.Parameters.Add("@Pincode", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblAddressTO.Pincode);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblAddressTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblAddressTO.CreatedOn;
            cmdInsert.Parameters.Add("@PlotNo", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblAddressTO.PlotNo);
            cmdInsert.Parameters.Add("@StreetName", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblAddressTO.StreetName);
            cmdInsert.Parameters.Add("@AreaName", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblAddressTO.AreaName);
            cmdInsert.Parameters.Add("@VillageName", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblAddressTO.VillageName);
            cmdInsert.Parameters.Add("@Comments", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue( tblAddressTO.Comments);
            cmdInsert.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblAddressTO.IsActive);
            cmdInsert.Parameters.Add("@GstTypeId", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblAddressTO.GstTypeId);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblAddressTO.IdAddr = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public int UpdateTblAddress(TblAddressTO tblAddressTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblAddressTO, cmdUpdate);
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

        public int UpdateTblAddress(TblAddressTO tblAddressTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblAddressTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(TblAddressTO tblAddressTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblAddress] SET " + 
                            "  [talukaId]= @TalukaId" +
                            " ,[districtId]= @DistrictId" +
                            " ,[stateId]= @StateId" +
                            " ,[countryId]= @CountryId" +
                            " ,[pincode]= @Pincode" +
                            " ,[plotNo]= @PlotNo" +
                            " ,[streetName]= @StreetName" +
                            " ,[areaName]= @AreaName" +
                            " ,[villageName]= @VillageName" +
                            " ,[comments] = @Comments" +
                            " ,[isAddrVisible] = @isActive" +
                            " ,[gstTypeId] = @GstTypeId" +
                            " WHERE  [idAddr] = @IdAddr"; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdAddr", System.Data.SqlDbType.Int).Value = tblAddressTO.IdAddr;
            cmdUpdate.Parameters.Add("@GstTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblAddressTO.GstTypeId);
            cmdUpdate.Parameters.Add("@TalukaId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblAddressTO.TalukaId);
            cmdUpdate.Parameters.Add("@DistrictId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblAddressTO.DistrictId);
            cmdUpdate.Parameters.Add("@StateId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblAddressTO.StateId);
            cmdUpdate.Parameters.Add("@CountryId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblAddressTO.CountryId);
            cmdUpdate.Parameters.Add("@Pincode", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblAddressTO.Pincode);
            cmdUpdate.Parameters.Add("@PlotNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblAddressTO.PlotNo);
            cmdUpdate.Parameters.Add("@StreetName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblAddressTO.StreetName);
            cmdUpdate.Parameters.Add("@AreaName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblAddressTO.AreaName);
            cmdUpdate.Parameters.Add("@VillageName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblAddressTO.VillageName);
            cmdUpdate.Parameters.Add("@Comments", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue( tblAddressTO.Comments);
            cmdUpdate.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblAddressTO.IsActive);
            return cmdUpdate.ExecuteNonQuery();
        }

        public int UpdateTblAddressType(int IdOrgAddr)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(IdOrgAddr, cmdUpdate);
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

        public int ExecuteUpdationCommand(int IdOrgAddr, SqlCommand cmdUpdate)
        {
            String sqlQuery = @"UPDATE tblOrgAddress SET " +
                            " [addrTypeId] = 3" +
                            " WHERE  [idOrgAddr] = @IdOrgAddr";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdOrgAddr", System.Data.SqlDbType.Int).Value = IdOrgAddr;
             return cmdUpdate.ExecuteNonQuery();
        }

        #endregion

        #region Deletion
        public int DeleteTblAddress(Int32 idAddr)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idAddr, cmdDelete);
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

        public int DeleteTblAddress(Int32 idAddr, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idAddr, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idAddr, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblAddress] " +
            " WHERE idAddr = " + idAddr +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idAddr", System.Data.SqlDbType.Int).Value = tblAddressTO.IdAddr;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
