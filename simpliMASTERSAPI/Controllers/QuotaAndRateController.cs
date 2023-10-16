using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System.Collections.Generic;
using System;
using System.Linq;
using simpliMASTERSAPI.BL.Interfaces;

namespace simpliMASTERSAPI.Controllers
{
    [Route("api/[controller]")]
    public class QuotaAndRateController : Controller
    {
        private readonly ITblQuotaDeclarationBL _tblQuotaDeclarationBL;
        public QuotaAndRateController(ITblQuotaDeclarationBL tblQuotaDeclarationBL) 
        {
            _tblQuotaDeclarationBL = tblQuotaDeclarationBL;
        }
        [Route("AnnounceRateAndQuota")]
        [HttpPost]
        public ResultMessage AnnounceRateAndQuota([FromBody] JObject data)
        {
            try
            {
                List<TblOrganizationTO> tblOrganizationTOList = JsonConvert.DeserializeObject<List<TblOrganizationTO>>(data["cnfList"].ToString());
                var declaredRate = data["declaredRate"].ToString();
                var loginUserId = data["loginUserId"].ToString();
                var comments = data["comments"].ToString();
                var rateReasonId = data["rateReasonId"].ToString();
                var rateReasonDesc = data["rateReasonDesc"].ToString();
                ResultMessage resultMessage = new ResultMessage();
                if (Convert.ToDouble(declaredRate) == 0)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "declaredRate Found 0";
                    return resultMessage;
                }

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "loginUserId Found 0";
                    return resultMessage;
                }

                if (Convert.ToInt32(rateReasonId) <= 0)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "rateReasonId Found 0";
                    return resultMessage;
                }

                if (tblOrganizationTOList != null && tblOrganizationTOList.Count > 0)
                {
                    // 1. Prepare TblGlobalRateTO

                    DateTime serverDate = Constants.ServerDateTime;
                    TblGlobalRateTO tblGlobalRateTO = new TblGlobalRateTO();
                    tblGlobalRateTO.CreatedOn = serverDate;
                    tblGlobalRateTO.CreatedBy = Convert.ToInt32(loginUserId);
                    tblGlobalRateTO.Rate = Convert.ToDouble(declaredRate);
                    tblGlobalRateTO.Comments = Convert.ToString(comments);
                    tblGlobalRateTO.RateReasonId = Convert.ToInt32(rateReasonId);
                    tblGlobalRateTO.RateReasonDesc = Convert.ToString(rateReasonDesc);

                    //2. Prepare Quota Declaration List
                    List<TblQuotaDeclarationTO> tblQuotaDeclarationTOList = new List<TblQuotaDeclarationTO>();
                    List<TblQuotaDeclarationTO> tblQuotaExtensionTOList = new List<TblQuotaDeclarationTO>();

                    var quotaExtList = tblOrganizationTOList.Where(q => q.ValidUpto > 0).ToList();

                    if (quotaExtList != null && quotaExtList.Count > 0)
                    {
                        for (int i = 0; i < quotaExtList.Count; i++)
                        {
                            TblOrganizationTO orgTO = quotaExtList[i];

                            TblQuotaDeclarationTO tblQuotaDeclarationTO = new TblQuotaDeclarationTO();
                            tblQuotaDeclarationTO.OrgId = orgTO.IdOrganization;
                            tblQuotaDeclarationTO.IdQuotaDeclaration = orgTO.QuotaDeclarationId;
                            tblQuotaDeclarationTO.ValidUpto = orgTO.ValidUpto;
                            tblQuotaDeclarationTO.UpdatedOn = serverDate;
                            tblQuotaDeclarationTO.UpdatedBy = Convert.ToInt32(loginUserId);
                            tblQuotaDeclarationTO.IsActive = 1;
                            tblQuotaExtensionTOList.Add(tblQuotaDeclarationTO);
                        }
                    }

                    for (int i = 0; i < tblOrganizationTOList.Count; i++)
                    {
                        TblOrganizationTO orgTO = tblOrganizationTOList[i];

                        TblQuotaDeclarationTO tblQuotaDeclarationTO = new TblQuotaDeclarationTO();
                        tblQuotaDeclarationTO.OrgId = orgTO.IdOrganization;
                        tblQuotaDeclarationTO.QuotaAllocDate = serverDate;
                        tblQuotaDeclarationTO.RateBand = orgTO.LastRateBand;
                        tblQuotaDeclarationTO.AllocQty = orgTO.LastAllocQty;
                        tblQuotaDeclarationTO.CreatedOn = serverDate;
                        tblQuotaDeclarationTO.CreatedBy = Convert.ToInt32(loginUserId);
                        tblQuotaDeclarationTO.IsActive = 1;

                        tblQuotaDeclarationTO.Tag = orgTO;
                        tblQuotaDeclarationTOList.Add(tblQuotaDeclarationTO);

                    }

                    int result = _tblQuotaDeclarationBL.SaveDeclaredRateAndAllocatedQuota(tblQuotaExtensionTOList, tblQuotaDeclarationTOList, tblGlobalRateTO);
                    if (result != 1)
                    {
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Result = 0;
                        resultMessage.Text = "Error In SaveDeclaredRateAndAllocatedQuota Method";
                        return resultMessage;
                    }

                    resultMessage.MessageType = ResultMessageE.Information;
                    resultMessage.Result = 1;
                    resultMessage.Text = "Booking Quota Announced Sucessfully";
                    return resultMessage;

                }
                else
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "tblOrganizationTOList Found NULL";
                    return resultMessage;
                }

            }
            catch (Exception ex)
            {
                ResultMessage resultMessage = new ResultMessage();
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                resultMessage.Text = "Exception Error IN API Call AnnounceRateAndQuota";
                return resultMessage;
            }
        }
    }
}
