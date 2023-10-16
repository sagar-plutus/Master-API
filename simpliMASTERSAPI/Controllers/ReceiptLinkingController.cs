using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.TO;
using simpliMASTERSAPI.BL.Interfaces;

namespace simpliMASTERSAPI.Controllers
{
    [Route("api/[controller]")]
    public class ReceiptLinkingController : Controller
    {
        private readonly IDimReceiptTypeBL _iDimReceiptTypeBL;
        private readonly ITblReceiptLinkingBL _iTblReceiptLinkingBL;
        private readonly ICommon _iCommon;
        public ReceiptLinkingController(IDimReceiptTypeBL iDimReceiptTypeBL, ITblReceiptLinkingBL iTblReceiptLinkingBL, ICommon iCommon)
        {
            _iDimReceiptTypeBL = iDimReceiptTypeBL;
            _iTblReceiptLinkingBL = iTblReceiptLinkingBL;
            _iCommon = iCommon;
        }
        [Route("GetReceiptTypeList")]
        [HttpGet]
        public List<DimReceiptTypeTO> GetReceiptTypeList(int userId)
        {
            return _iDimReceiptTypeBL.SelectAllDimReceiptTypeList(userId);
        }

        [Route("GetBrsBankStatementDtlList")]
        [HttpGet]
        public List<TblBrsBankStatementDtlTO> GetBrsBankStatementDtlList()
        {
            ResultMessage resultMessage = new ResultMessage();
            //DateTime fromDate = DateTime.MinValue;
            //DateTime toDate = DateTime.MinValue;
            ////if (Constants.IsDateTime(fromDateStr))
            //fromDate = Convert.ToDateTime(Convert.ToDateTime(fromDateStr).ToString(Constants.AzureDateFormat));
            ////if (Constants.IsDateTime(toDateStr))
            ////    toDate = Convert.ToDateTime(Convert.ToDateTime(toDateStr).ToString(Constants.AzureDateFormat));

            ////if (!string.IsNullOrEmpty(fromDateStr))
            ////    fromDate = Convert.ToDateTime(fromDateStr);
            ////if (!string.IsNullOrEmpty(toDateStr))
            ////    toDate = Convert.ToDateTime(toDateStr);

            //try
            //{

            //    resultMessage.DefaultSuccessBehaviour();
            return _iTblReceiptLinkingBL.SelectAllReceiptStatementDtl();
            //    return resultMessage;


            //}
            //catch (Exception ex)
            //{
            //    resultMessage.DisplayMessage = ex.ToString();
            //    return resultMessage;
            //}
        }
        [Route("UpdateReceiptStatementDtlStatus")]
        [HttpPost]
        public ResultMessage UpdateReceiptStatementDtlStatus([FromBody] JObject data)
        {
            TblBrsBankStatementDtlTO tblBrsBankStatementDtlTO = JsonConvert.DeserializeObject<TblBrsBankStatementDtlTO>(data["tblBrsBankStatementDtlTO"].ToString());
            tblBrsBankStatementDtlTO.UpdatedOn = _iCommon.ServerDateTime;
            return _iTblReceiptLinkingBL.UpdateReceiptStatementDtlStatus(tblBrsBankStatementDtlTO);
        }
    }
}