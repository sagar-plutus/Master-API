using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.StaticStuff;
using simpliMASTERSAPI;

namespace ODLMWebAPI.BL
{
    public class ConnectionString : IConnectionString
    {
        private readonly HttpContext httpContext;
        public ConnectionString(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContext = httpContextAccessor.HttpContext;
        }
        public string GetSubDomain(String url)
        {
            if (url != null)
            {
                Uri fullPath = new Uri(url);
                string hostName = fullPath.Host;
                string[] domains = hostName.Split(new char[] { '.' });
                if (domains.Count() > 1)
                {
                    string subDomain = domains[0];
                    return subDomain;
                }
                else
                {
                    return StaticStuff.Constants.Local_URL;
                }
            }
            return null;
        }
        public string SetConnectionString(String ConfigName)
        {
            if(Startup.IsLocalAPI == true)
            {
                if (Constants.CONNECTION_STRING == ConfigName)
                    return Startup.ConnectionString;
                if(Constants.REQUEST_ORIGIN_STRING == ConfigName)
                    return Startup.RequestOriginString;
                if (Constants.AZURE_CONNECTION_STRING == ConfigName)
                    return Startup.AzureConnectionStr;
                if (Constants.SAP_CONNECTION_STRING == ConfigName)
                    return Startup.SapConnectionStr;
                if (Constants.SAP_CONNECTION_STRING == ConfigName)
                    return Startup.SapConnectionStr;
                if (Constants.TENANT_ID == ConfigName)
                    return Startup.Tenant_id;
            }
            else
            {
                String HostURL = this.httpContext.Request.Headers["apiurl"];
                if (!String.IsNullOrEmpty(HostURL))
                {
                    String SubDomain = GetSubDomain(HostURL);
                    if (!String.IsNullOrEmpty(SubDomain))
                    {
                        JObject o1 = JObject.Parse(System.IO.File.ReadAllText(@".\connection.json"));
                        return (string)o1[SubDomain][ConfigName];
                    }
                }
            }
            return string.Empty;
        }
        public string GetConnectionString(String ConfigName)
        {
            return SetConnectionString(ConfigName);
        }
    }
}
