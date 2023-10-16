using Newtonsoft.Json.Linq;
using rabbitMessaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;
using simpliMASTERSAPI.DAL.Interfaces;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using simpliMASTERSAPI.Models;
using System.Runtime.Serialization.Json;
using static ODLMWebAPI.Models.TblUserTO;

namespace simpliMASTERSAPI.RabitMQHandler
{
    public class RabbitmqHandler : IMessageHandlerCallback, Microsoft.Extensions.Hosting.IHostedService
    {
        private readonly IMessageHandler _messageHandler;
      
        public RabbitmqHandler (IMessageHandler messageHandler)
        {
            _messageHandler = messageHandler;
         
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _messageHandler.Start(this);
            return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _messageHandler.Stop();
            return Task.CompletedTask;
        }

        public async Task<bool> HandleMessageAsync(string messageType, string message)
        {
            try
            {
                JObject messageObject = MessageSerializer.Deserialize(message);
                TblUserTO tblUserTO = messageObject.ToObject<TblUserTO>();
                String reqOriginStr = null;
                int result = 0;
                if (Constants.Local_API == true)
                {
                    reqOriginStr = Startup.RequestOriginString;
                }
                else
                {
                    JObject o1 = JObject.Parse(System.IO.File.ReadAllText(@".\connection.json"));
                    foreach (var property in o1)
                    {
                        string key = property.Key;
                        if ((string)o1[key][Constants.TENANT_ID] == tblUserTO.TenantId)
                        {
                            reqOriginStr = (string)o1[key][Constants.REQUEST_ORIGIN_STRING];
                            break;
                        }
                       
                    }
                }
                TenantTO tenantTO = getCurrentTenant(reqOriginStr);
                if (tblUserTO.TenantId == tenantTO.TenantId)
                {
                    switch (messageType)
                    {
                        case "UserDeactivatedHrgird":
                            //var abc = messageObject.ToObject<TblUserTO>();
                            result = HandleAsync(tblUserTO, reqOriginStr);
                            break;
                    }
                }
                if (result == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
          
        }

        public TenantTO getCurrentTenant(String reqOriginStr)
        {
            TenantTO tenantTO = new TenantTO();
            String requestUrl = "Masters/GetCurrentTenantConfig";
            String url = Startup.MasterUrl + requestUrl;
            String result;
            StreamWriter myWriter = null;
            WebRequest request = WebRequest.Create(url);
            request.Headers.Add("apiurl", reqOriginStr);
            request.Method = "GET";
            WebResponse objResponse = request.GetResponseAsync().Result;
            using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                tenantTO = JsonConvert.DeserializeObject<TenantTO>(result.ToString());
                sr.Dispose();

            }
            return tenantTO;

        }

        private int HandleAsync(TblUserTO userTO, String reqOriginStr)
        {
            try
            {
                int response = 0;
                userTO.DeactivatedOn = DateTime.Now;
                var values = new JObject();
                values.Add("userTO", JsonConvert.SerializeObject(userTO));

                apiDataUser data = new apiDataUser();
                data.userTO = userTO;

                MemoryStream ms = new MemoryStream();
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(apiDataUser));
                ser.WriteObject(ms, data);
                byte[] json = ms.ToArray();
                ms.Close();

                String notifyUrl = "User/ActivateOrDeactivateUser";

                String url = Startup.MasterUrl + notifyUrl;
                object result;
                StreamWriter myWriter = null;
                WebRequest request = WebRequest.Create(url);
                request.Headers.Add("apiurl", reqOriginStr);
                request.Method = "Post";
                request.ContentType = "application/json";

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(json, 0, json.Length);
                    requestStream.Close();
                }

                WebResponse objResponse = request.GetResponseAsync().Result;
                using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    sr.Dispose();
                }
                if (result != null && result.ToString() != string.Empty && result.ToString() != "")
                {
                    String res = result.ToString();
                     response = Convert.ToInt32(res);                   
                }
                if (response == 1)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
            // TblUserTO user = new TblUserTO();
            // {
            //     user.IdUser = userTO.IdUser;
            //     user.IsActive = 0;
            // };
            // //return 0;
            //return Constants.DeactivateUser(user);
        }

    }
}
