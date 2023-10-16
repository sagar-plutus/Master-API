using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Runtime.Serialization.Json;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.StaticStuff;
using System.Dynamic;
using ODLMWebAPI.Models;
using simpliMASTERSAPI;
using simpliMASTERSAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Reflection;

namespace ODLMWebAPI.DAL
{ 
    public class Common : ICommon
    {
        private readonly IConnectionString _iConnectionString;
       
        public Common(IConnectionString iConnectionString )
        {
            _iConnectionString = iConnectionString;
          
        }

        public System.DateTime SelectServerDateTime()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                /*To get Server Date Time for Local DB*/
                String sqlQuery = Startup.SERVER_DATETIME_QUERY_STRING;

                //To get Server Date Time for Azure Server DB
                //string sqlQuery = " declare @dfecha as datetime " +
                //                  " declare @d as datetimeoffset " +
                //                  " set @dfecha= sysdatetime()   " +
                //                  " set @d = convert(datetimeoffset, @dfecha) at time zone 'india standard time'" +
                //                  " select convert(datetime, @d)";

                cmdSelect = new SqlCommand(sqlQuery, conn);

                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);

                while (dateReader.Read())
                {
                    if (TimeZoneInfo.Local.Id != "India Standard Time")
                        return Convert.ToDateTime(dateReader[0]).ToLocalTime();
                    else return Convert.ToDateTime(dateReader[0]);
                }

                return new DateTime();
            }
            catch (Exception ex)
            {
                return new DateTime();
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public DateTime ServerDateTime
        {
            get
            {
                return SelectServerDateTime();
            }
        }

        public List<DynamicReportTO> SqlDataReaderToExpando(SqlDataReader reader)
        {
            List<DynamicReportTO> list = new List<DynamicReportTO>();

            while (reader.Read())
            {
                DynamicReportTO dynamicReportTO = new DynamicReportTO();
                List<DropDownTO> dropDownList = new List<DropDownTO>();
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    DropDownTO dropDownTO = new DropDownTO();
                    dropDownTO.Text = reader.GetName(i);
                    dropDownTO.Tag = reader[i];
                    dropDownList.Add(dropDownTO);
                }
                dynamicReportTO.DropDownList = dropDownList;
                list.Add(dynamicReportTO);
            }
            return list;
        }

        public dynamic selectUserReportingListOnUserId(int userId)
        {
            String apiUrl = Startup.CommonUrl + "OrganizationStructure/SelectUserReportingListOnUserId?idUser=" + userId;
            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(apiUrl);
            String result;
            objRequest.Method = "GET";
            objRequest.Headers.Add("apiurl", _iConnectionString.GetConnectionString(Constants.REQUEST_ORIGIN_STRING));
            WebResponse objResponse = objRequest.GetResponseAsync().Result;
            using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
            {
                result = sr.ReadToEnd();
            }
            return result;
        }

        public Boolean UserExistInCommaSeparetedStr(String commaSepatedStr, Int32 userId)
        {
            Boolean result = false;

            if (!String.IsNullOrEmpty(commaSepatedStr))
            {
                List<String> list = commaSepatedStr.Split(',').ToList();

                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] == userId.ToString())
                    {
                        result = true;
                        break;
                    }
                }

            }

            return result;

        }


        public IEnumerable<dynamic> GetDynamicSqlData(string connectionstring, string sql, params SqlParameter[] commandParmeter)
        {
            using (var conn = new SqlConnection(connectionstring))
            {
                using (var comm = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    if (commandParmeter != null)
                    {
                        foreach (SqlParameter parm in commandParmeter)
                        {
                            if (parm.Value == null)
                            {
                                parm.Value = DBNull.Value;
                            }
                            comm.Parameters.Add(parm);
                        }
                    }
                    using (var reader = comm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return SqlDataReaderToExpandoV1(reader);
                        }
                    }
                    conn.Close();
                }
            }
        }

        private dynamic SqlDataReaderToExpandoV1(SqlDataReader reader)
        {
            var expandoObject = new ExpandoObject() as IDictionary<string, object>;

            for (var i = 0; i < reader.FieldCount; i++)
            {
                string name = reader.GetName(i);
                name = name.Replace('_', ' ');
                expandoObject.Add(name, reader[i]);
            }
            return expandoObject;
        }

        public void CalculateBookingsOpeningBalance(String RequestOriginString)
        {
            try
            {
                String requestUrl = "booking/CalculateBookingsOpeningBalance";
                String url = Startup.DeliverUrl + requestUrl;
                String result;
                StreamWriter myWriter = null;
                WebRequest request = WebRequest.Create(url);
                request.Headers.Add("apiurl", RequestOriginString);
                request.Method = "GET";
                WebResponse objResponse = request.GetResponseAsync().Result;
                using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    sr.Dispose();
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public void PostCancelNotConfirmLoadings(String RequestOriginString)
        {
            try
            {
                String requestUrl = "LoadSlip/PostCancelNotConfirmLoadings";
                String url = Startup.DeliverUrl + requestUrl;
                String result;
                StreamWriter myWriter = null;
                WebRequest request = WebRequest.Create(url);
                request.Headers.Add("apiurl", RequestOriginString);
                request.Method = "GET";
                WebResponse objResponse = request.GetResponseAsync().Result;
                using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    sr.Dispose();
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
        public dynamic AddConsolidationByPurchaseRequest(TblPurchaseRequestTo tblPurchaseRequestTo)
        {
            var tRequest = WebRequest.Create(Startup.StockUrl + "Consolidation/AddConsolidationByPurchaseRequest") as HttpWebRequest;
            try
            {
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                var data = new
                {
                    data = tblPurchaseRequestTo,
                };
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(tblPurchaseRequestTo);
                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                using (Stream dataStream = tRequest.GetRequestStreamAsync().Result)
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }
                tRequest.Timeout = 10000000;
                tRequest.Headers.Add("apiurl", _iConnectionString.GetConnectionString(Constants.REQUEST_ORIGIN_STRING));
                var response = (HttpWebResponse)tRequest.GetResponseAsync().Result;
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                return responseString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public dynamic StockTransfer(TblPurchaseRequestTo tblPurchaseRequestTo)
        {
            var tRequest = WebRequest.Create(Startup.StockUrl + "PurchaseRequest/StockTransfer") as HttpWebRequest;
            try
            {
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                var data = new
                {
                    data = tblPurchaseRequestTo,
                };
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(tblPurchaseRequestTo);
                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                using (Stream dataStream = tRequest.GetRequestStreamAsync().Result)
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }
                tRequest.Timeout = 10000000;
                tRequest.Headers.Add("apiurl", _iConnectionString.GetConnectionString(Constants.REQUEST_ORIGIN_STRING));
                var response = (HttpWebResponse)tRequest.GetResponseAsync().Result;
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                return responseString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void PostAutoResetAndDeleteAlerts(String RequestOriginString)
        {
            try
            {
                String requestUrl = "Notify/PostAutoResetAndDeleteAlerts";
                String url = Startup.DeliverUrl + requestUrl;
                String result;
                StreamWriter myWriter = null;
                WebRequest request = WebRequest.Create(url);
                request.Headers.Add("apiurl", RequestOriginString);
                request.Method = "GET";
                WebResponse objResponse = request.GetResponseAsync().Result;
                using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    sr.Dispose();
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public dynamic GetProFlowUserList(ProFlowSettingTO proFlowSettingTO)
        {
            var tRequest = WebRequest.Create(Startup.CFlowIntegrationAPI + "1/getallusers/") as HttpWebRequest;
            try
            {
                ProFlowUserTO proFlowUserTO = new ProFlowUserTO();
                proFlowUserTO.Client_ID = proFlowSettingTO.CLIENT_ID;
                proFlowUserTO.Username = proFlowSettingTO.CLIENT_USER_NAME;
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                var data = new
                {
                    data = proFlowUserTO,
                };
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(proFlowUserTO);
                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                using (Stream dataStream = tRequest.GetRequestStreamAsync().Result)
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }
                tRequest.Timeout = 100000;
                tRequest.Headers.Add("apikey", proFlowSettingTO.API_KEY); 
                tRequest.Headers.Add("userkey", proFlowSettingTO.USER_KEY);
                var response = (HttpWebResponse)tRequest.GetResponseAsync().Result;
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                return responseString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public dynamic CreateProFlowUser(ProFlowUserTO proFlowUserTO, ProFlowSettingTO proFlowSettingTO)
        {
            var tRequest = WebRequest.Create(Startup.CFlowIntegrationAPI + "1/createuser/") as HttpWebRequest;
            try
            {
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                var data = new
                {
                    data = proFlowUserTO,
                };
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(proFlowUserTO);
                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                using (Stream dataStream = tRequest.GetRequestStreamAsync().Result)
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }
                tRequest.Timeout = 100000;
                tRequest.Headers.Add("apikey", proFlowSettingTO.API_KEY);
                tRequest.Headers.Add("userkey", proFlowSettingTO.USER_KEY);
                var response = (HttpWebResponse)tRequest.GetResponseAsync().Result;
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                return responseString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public dynamic UpdateProFlowUser(UpdateProFlowUserTO updateProFlowUserTO, ProFlowSettingTO proFlowSettingTO)
        {
            var tRequest = WebRequest.Create(Startup.CFlowIntegrationAPI + "1/updateuser_multiplefields/") as HttpWebRequest;
            try
            {
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                var data = new
                {
                    data = updateProFlowUserTO,
                };
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(updateProFlowUserTO);
                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                using (Stream dataStream = tRequest.GetRequestStreamAsync().Result)
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }
                tRequest.Timeout = 100000;
                tRequest.Headers.Add("apikey", proFlowSettingTO.API_KEY);
                tRequest.Headers.Add("userkey", proFlowSettingTO.USER_KEY);
                var response = (HttpWebResponse)tRequest.GetResponseAsync().Result;
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                return responseString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public dynamic SendWhatsAppMsg(String WhatsAppMsgRequestTOStr,String Url, String WhatsAppMsgRequestHeaderStr)
        {
            var tRequest = WebRequest.Create(Url) as HttpWebRequest;
            try
            { 
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                var data = new
                {
                    data = Newtonsoft.Json.JsonConvert.DeserializeObject(WhatsAppMsgRequestTOStr),
                };
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(Newtonsoft.Json.JsonConvert.DeserializeObject(WhatsAppMsgRequestTOStr));
                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                using (Stream dataStream = tRequest.GetRequestStreamAsync().Result)
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }
                if (!String.IsNullOrEmpty(WhatsAppMsgRequestHeaderStr))
                {
                    dynamic HeaderObj = Newtonsoft.Json.JsonConvert.DeserializeObject(WhatsAppMsgRequestHeaderStr);
                    foreach (var item in HeaderObj)
                    {
                        tRequest.Headers[Convert.ToString(item.Name)] = Convert.ToString(item.Value);
                        //tRequest.Headers.Add(item.Name, item.Value);
                    }
                }
                tRequest.Timeout = 100000;
                var response = (HttpWebResponse)tRequest.GetResponseAsync().Result;
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                return responseString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public dynamic SendWhatsAppMsgWithFile(String WhatsAppMsgRequestTOStr, String Url, String WhatsAppMsgRequestHeaderStr)
        { 
            var tRequest = WebRequest.Create(Url) as HttpWebRequest;
            try
            { 
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                var data = new
                {
                    data = Newtonsoft.Json.JsonConvert.DeserializeObject(WhatsAppMsgRequestTOStr),
                };
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(Newtonsoft.Json.JsonConvert.DeserializeObject(WhatsAppMsgRequestTOStr));
                //var json = Newtonsoft.Json.JsonConvert.SerializeObject(WhatsAppMsgRequestTOStr);

                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                using (Stream dataStream = tRequest.GetRequestStreamAsync().Result)
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                } 
                if (!String.IsNullOrEmpty(WhatsAppMsgRequestHeaderStr))
                {
                    dynamic HeaderObj = Newtonsoft.Json.JsonConvert.DeserializeObject(WhatsAppMsgRequestHeaderStr);
                    foreach (var item in HeaderObj)
                    {
                        tRequest.Headers[Convert.ToString(item.Name)] = Convert.ToString(item.Value);
                        //tRequest.Headers.Add(item.Name, item.Value);
                    } 
                }
                tRequest.Timeout = 100000;
                var response = (HttpWebResponse)tRequest.GetResponseAsync().Result;
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                return responseString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
        //user Tracking checked login entry if logout time is not null then return true
        public int CheckLogOutEntry(int loginId)
        {
            int count = 0;
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            try
            {

                String sqlQuery = "select count(*) from tbllogin where idLogin= " + loginId + "  and logoutDate IS NOT NULL";
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                conn.Open();
                count = Convert.ToInt32(cmd.ExecuteScalar());
                return count;

            }
            catch (Exception ex)
            {
                throw ex;
                return count;
            }
            finally
            {
                conn.Close();

            }
            return count;


        }  

#region  isDeactivate User
 public  int IsUserDeactivate(int userId)
{
    int active=1;
      String sqlConnStr =  _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
    try{

            String sqlQuery = "select isActive from tblUser where idUser= "+userId;
            SqlCommand cmd=new SqlCommand(sqlQuery,conn);
             conn.Open();
         active= Convert.ToInt32(cmd.ExecuteScalar());
         return active;

    }
    catch(Exception ex)
            {
                throw ex;
                 return active;
            }
            finally
            {
                conn.Close();
                
            }
            return active;


}  
#endregion
         #region get Login id List which register on apk and then uninstall  user Tracking
        public  string SelectApKLoginArray(int userId)
        {
            string LoginIds="";
            
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select idLogin from tbllogin where logoutdate is  null and deviceId is not Null and userId="+userId;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader ApkLoginReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                while(ApkLoginReader.Read())
                {
                    if(!string.IsNullOrEmpty(LoginIds))
                    {
                       LoginIds+=",";

                    }
                    LoginIds+=ApkLoginReader["idLogin"];
                }
                return LoginIds;
            }
            catch (Exception ex)
            {
                return LoginIds;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        public string SelectAllLoginEntries(Int32 IdUser)
        {
            string LoginIds = "";

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "select idLogin from tbllogin where logoutdate is  null and userId=" + IdUser;
              
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader AllLoginEntries = cmdSelect.ExecuteReader(CommandBehavior.Default);
                while (AllLoginEntries.Read())
                {
                    if (!string.IsNullOrEmpty(LoginIds))
                    {
                        LoginIds += ",";

                    }
                    LoginIds += AllLoginEntries["idLogin"];
                }
                return LoginIds;
            }
            catch (Exception ex)
            {
                return LoginIds;
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
