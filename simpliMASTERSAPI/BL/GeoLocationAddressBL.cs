
using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.StaticStuff;
namespace ODLMWebAPI.BL
{ 
    public class GeoLocationAddressBL : IGeoLocationAddressBL
    {
        private readonly ITblAddressDAO _iTblAddressDAO;
        private readonly IConnectionString _iConnectionString;
        private readonly ITblConfigParamsBL _iTblConfigParamsBL;
        public GeoLocationAddressBL(ITblAddressDAO iTblAddressDAO, IConnectionString iConnectionString, ITblConfigParamsBL iTblConfigParamsBL)
        {
            _iTblConfigParamsBL = iTblConfigParamsBL;
            _iTblAddressDAO = iTblAddressDAO;
            _iConnectionString = iConnectionString;
        }
        public GeoLocationAddressTo convertToProperAddress(GoogleGeoCodeResponse addressDetails)
        {
            List<results> googleGeoCodeResponse = new List<results>();
            googleGeoCodeResponse = addressDetails.results;
            GeoLocationAddressTo obj = new GeoLocationAddressTo();
            if (googleGeoCodeResponse != null && googleGeoCodeResponse.Count > 0)
            {
                string formattedAddress = googleGeoCodeResponse[0].formatted_address;
                string lat = googleGeoCodeResponse[0].geometry.location.lat;
                string lng = googleGeoCodeResponse[0].geometry.location.lng;

                Type addressTo = typeof(GeoLocationAddressTo);
                obj = Activator.CreateInstance<GeoLocationAddressTo>();
                int i = 0;
                string[] AddressType = new string[] { "route", "neighborhood", "sublocality_level_2", "sublocality_level_1", "locality", "administrative_area_level_2", "administrative_area_level_1", "country", "postal_code" };
                foreach (PropertyInfo pro in addressTo.GetProperties())
                {
                    var results = Array.FindAll(AddressType, s => s.Equals(pro.Name));
                    if (results.Count() > 0)
                    {
                        pro.SetValue(obj, createAddressDetils(googleGeoCodeResponse, AddressType[i]), null);
                        i++;
                    }
                }
                obj.formatted_address = formattedAddress;
                obj.lat = lat;
                obj.lng = lng;
            }
            return obj;
        }

        public string createAddressDetils(List<results> googleGeoCodeResponse, string item)
        {

            address_component addressComponent = googleGeoCodeResponse[0].address_components.Where(w => w.types.Contains(item)).FirstOrDefault();
            if (addressComponent != null)
            {
                return addressComponent.long_name;
            }
            return "";
        }

        public string myLocationAddress(string lat, string logn)
        {
             String key = "AIzaSyBHzCAnCe-VAqQEdONE0PEewbPnjtqeI2Q";
            String mapMyIndiaKey = "x6a9yupqaxh2ppibyu7847wwfdj8fn9p";
            TblConfigParamsTO mapConfigTO = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.IS_MAP_MY_INDIA);
            TblConfigParamsTO MapAPIConfigTO = new TblConfigParamsTO();
            String url = null;
            //String mapMyIndiaUrl = null;
            if (mapConfigTO != null && Convert.ToInt32(mapConfigTO.ConfigParamVal) == 1)
            {
                MapAPIConfigTO = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.MAP_MY_INDIA_URL_FOR_myLocationAddress);
                if (MapAPIConfigTO != null)
                {
                    url = MapAPIConfigTO.ConfigParamVal;
                    url=url.Replace("@lat", lat);
                    url= url.Replace("@logn", logn);
                }
                else
                {
                    url = "https://apis.mapmyindia.com/advancedmaps/v1/" + mapMyIndiaKey + "/rev_geocode?lat=" + lat + "&lng=" + logn;

                }
            }
            else
            {
                MapAPIConfigTO = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.GOOGLE_MAP_API_URL_FOR_LAT_LONG);
                if (MapAPIConfigTO != null)
                {
                    url = MapAPIConfigTO.ConfigParamVal;
                    url = url.Replace("@lat", lat);
                    url = url.Replace("@logn", logn);
                }
                else
                {
                    url = "https://maps.googleapis.com/maps/api/geocode/json?latlng=" + lat + "," + logn + "&key=" + key;

                }
            }

            StreamWriter myWriter = null;

            HttpWebRequest objRequest;
            //if (mapConfigTO != null && Convert.ToInt32(mapConfigTO.ConfigParamVal) == 1)
            //     objRequest = (HttpWebRequest)WebRequest.Create(mapMyIndiaUrl);
            //else
                objRequest = (HttpWebRequest)WebRequest.Create(url);
            String result;
            objRequest.Method = "POST";
            objRequest.ContentType = "application/x-www-form-urlencoded";
            try
            {
                Stream aa = objRequest.GetRequestStreamAsync().Result;
                myWriter = new StreamWriter(aa);
                myWriter.Write(aa);
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                myWriter.Dispose();
            }

            WebResponse objResponse = objRequest.GetResponseAsync().Result;
            using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
            {
                result = sr.ReadToEnd();
            }
            return result;

        }

        public string myLatLngByAddress(string address)
        {
            if(string.IsNullOrEmpty(address))
            {
                return address;
            }
            String key = "AIzaSyCkLbSDnkG5FxxMMTFwaBzs9JticPPMsRM";
            String mapMyIndiaKey = "x6a9yupqaxh2ppibyu7847wwfdj8fn9p";
            address = address.Replace("#", "");
            //String url = "https://maps.googleapis.com/maps/api/geocode/json?latlng=" + lat + "," + logn + "&key=" + key;
            String url = null;
            TblConfigParamsTO MapAPIConfigTO = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.GOOGLE_MAP_API_URL_FOR_ADDRESS);
            if (MapAPIConfigTO != null)
            {
                url = MapAPIConfigTO.ConfigParamVal;
                url = url.Replace("@address", address);
            }
            else
            {
                url = "https://maps.googleapis.com/maps/api/geocode/json?address=" + address + "&key=" + key;
            }
            
            StreamWriter myWriter = null;
            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);
            String result;
            objRequest.Method = "POST";
            objRequest.ContentType = "application/x-www-form-urlencoded";
            try
            {
                Stream aa = objRequest.GetRequestStreamAsync().Result;
                myWriter = new StreamWriter(aa);
                myWriter.Write(aa);
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                myWriter.Dispose();
            }

            WebResponse objResponse = objRequest.GetResponseAsync().Result;
            using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
            {
                result = sr.ReadToEnd();
            }
            return result;

        }

        public List<newdata> SelectAlllatlngData()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select lat,lng,idtblVisitDetails from tblCRMVisitDetails where  visitePlace IS NULL and lat != 0.000000000 and lng != 0.000000000";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<newdata> list = ConvertDTToList(sqlReader);
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



        public List<newdata> ConvertDTToList(SqlDataReader tblAddressTODT)
        {
            List<newdata> tblAddressTOList = new List<newdata>();
            if (tblAddressTODT != null)
            {
                while (tblAddressTODT.Read())
                {
                    newdata tblAddressTONew = new newdata();
                    if (tblAddressTODT["lat"] != DBNull.Value)
                        tblAddressTONew.Lat = Convert.ToString(tblAddressTODT["lat"].ToString());
                    if (tblAddressTODT["lng"] != DBNull.Value)
                        tblAddressTONew.Lng = Convert.ToString(tblAddressTODT["lng"].ToString());
                    if (tblAddressTODT["idtblVisitDetails"] != DBNull.Value)
                        tblAddressTONew.IdtblVisitDetails = Convert.ToInt32(tblAddressTODT["idtblVisitDetails"].ToString());
                    tblAddressTOList.Add(tblAddressTONew);
                }
            }
            return tblAddressTOList;
        }

        public Int32 insertAddress(GeoLocationAddressTo addressTo, int IdtblVisitDetails)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(addressTo, IdtblVisitDetails, cmdInsert);
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

        public Int32 insertlatlong(GeoLocationAddressTo addressTo, int idaddress)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteUpdateCommand(addressTo, idaddress, cmdInsert);
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

        public Int32 ExecuteInsertionCommand(GeoLocationAddressTo addressTo, int IdtblVisitDetails, SqlCommand cmdInsert)
        {
            String sqlQuery = "update tblCRMVisitDetails set visitePlace = " + addressTo.formatted_address + " where idtblVisitDetails =" + IdtblVisitDetails;
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            return cmdInsert.ExecuteNonQuery();
        }

        public Int32 ExecuteUpdateCommand(GeoLocationAddressTo addressTo, int idaddress, SqlCommand cmdInsert)
        {
            String sqlQuery = "update tblAddress set lat = " + addressTo.lat + ",lng= " + addressTo.lng + "  where idAddr =" + idaddress;
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            return cmdInsert.ExecuteNonQuery();
        }

        public List<TblAddressTO> SelectAllAddress()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                String sqlSelectQry = " SELECT addr.* ,tal.talukaName,dist.districtName,stat.stateName,stat.stateCode " +
                                  " FROM tblAddress addr " +
                                  " LEFT JOIN dimTaluka tal ON tal.idTaluka = addr.talukaId " +
                                  " LEFT JOIN dimDistrict dist ON dist.idDistrict = addr.districtId " +
                                  " LEFT JOIN dimState stat ON stat.idState = addr.stateId where addr.lat is null and addr.lng is null";
                cmdSelect.CommandText = sqlSelectQry;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                List<TblAddressTO> list = new List<TblAddressTO>();
                SqlDataReader tblAddressTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
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
                    list.Add(tblAddressTONew);
                }

                //List<TblAddressTO> list = _iTblAddressDAO.ConvertDTToList(tblAddressTODT);
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
    }
}