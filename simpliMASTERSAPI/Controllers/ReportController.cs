using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ODLMWebAPI.Models;
using Newtonsoft.Json;
using System.Net;
using Newtonsoft.Json.Linq;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.StaticStuff;

namespace ODLMWebAPI.Controllers
{
    
    [Produces("application/json")]
    [Route("api/Report")]
    public class ReportController : Controller
    {
        private readonly ITblReportsBL _iTblReportsBL;
        public ReportController(ITblReportsBL iTblReportsBL)
        {
            _iTblReportsBL = iTblReportsBL;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// Sudhir[04-SEPT-2018] Added for Selection Of Report.
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        [Route("GetReportObject")]
        [HttpGet]
        [ProducesResponseType(typeof(TblReportsTO), 200)]
        [ProducesResponseType(typeof(void), 500)]
        [ProducesResponseType(typeof(EmptyResult), 204)]
        [Produces("application/json")]
        public IActionResult GetReportObject(Int32 reportId)
        {
            try
            {
                TblReportsTO tblReportsTO = _iTblReportsBL.SelectTblReportsTO(reportId);
                if (tblReportsTO != null)
                {
                    return Ok(tblReportsTO);
                }
                else
                {
                    return NotFound(tblReportsTO);
                }
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Sudhir[10-SEPT-2018] Added for Selection Of All Report.
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        [Route("GetAllReportObject")]
        [HttpGet]
        [ProducesResponseType(typeof(List<TblReportsTO>), 200)]
        [ProducesResponseType(typeof(void), 500)]
        [ProducesResponseType(typeof(EmptyResult), 204)]
        [Produces("application/json")]
        public IActionResult GetAllReportObject(Int32 reportId)
        {
            try
            {
                List<TblReportsTO> tblReportsTOList = _iTblReportsBL.SelectAllTblReportsList();
                if (tblReportsTOList != null)
                {
                    return Ok(tblReportsTOList);
                }
                else
                {
                    return NotFound(tblReportsTOList);
                }
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Sudhir[10-SEPT-2018] Added for Selection Of All Report.
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        [Route("GetAllModuleWiseReport")]
        [HttpGet]
        [ProducesResponseType(typeof(List<TblReportsTO>), 200)]
        [ProducesResponseType(typeof(void), 500)]
        [ProducesResponseType(typeof(EmptyResult), 204)]
        [Produces("application/json")]
        public IActionResult GetAllModuleWiseReport(Int32 moduleId = 0, Int32 transId = 0,Int32 loginUserId = 1)
        {
            try
            {
                List<TblReportsTO> tblReportsTOList = _iTblReportsBL.SelectAllModuleWiseReport(moduleId, transId, loginUserId);
                if (tblReportsTOList != null)
                {
                    return Ok(tblReportsTOList);
                }
                else
                {
                    return NotFound(tblReportsTOList);
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }


        [Route("GetDynamicData")]
        [HttpPost]
        [ProducesResponseType(typeof(List<DynamicReportTO>), 200)]
        [ProducesResponseType(typeof(void), 500)]
        [ProducesResponseType(typeof(EmptyResult), 204)]
        [Produces("application/json")]
        public IActionResult GetDynamicData([FromBody] JObject data)
        {

            TblReportsTO tblReportsTO = JsonConvert.DeserializeObject<TblReportsTO>(data.ToString());

            IEnumerable<dynamic> dataList = _iTblReportsBL.CreateDynamicQuery(tblReportsTO);
           
            if (data != null)
            {
                return Ok(dataList);              
            }
            else
            {
                return NotFound("No Data  Found"); ;
            }
        }

        //Added by minal 02 April 2021 Task Id = 1027

        [Route("GetTallyStockTransferReportList")]
        [HttpPost]
        public List<dynamic> GetTallyStockTransferReportList([FromBody] JObject data)
        {
            TblReportsTO tblReportsTO = JsonConvert.DeserializeObject<TblReportsTO>(data["tblReportsTO"].ToString());

            if (tblReportsTO == null)
            {
                return null;
            }

            DateTime fromDate = DateTime.MinValue;
            DateTime toDate = DateTime.MinValue;                       

            if (tblReportsTO.TblFilterReportTOList1 != null && tblReportsTO.TblFilterReportTOList1.Count > 0)
            {
                TblFilterReportTO fromDateTO = tblReportsTO.TblFilterReportTOList1.Where(a => a.SqlDbTypeValue == 33 && a.SqlParameterName == "FromDate").FirstOrDefault();
                if (fromDateTO != null)
                {
                    fromDate = Convert.ToDateTime(fromDateTO.OutputValue);
                }

                TblFilterReportTO toDateTO = tblReportsTO.TblFilterReportTOList1.Where(a => a.SqlDbTypeValue == 33 && a.SqlParameterName == "ToDate").FirstOrDefault();
                if (fromDateTO != null)
                {
                    toDate = Convert.ToDateTime(toDateTO.OutputValue);
                }
            }     

            return _iTblReportsBL.SelectTallyStockTransferReportList(fromDate, toDate, tblReportsTO.RoleTypeCond);
        }

        [Route("GetWBForPurchaseSaleUnloadReportList")]
        [HttpPost]
        public List<TblWBRptTO> GetWBForPurchaseSaleUnloadReportList([FromBody] JObject data)
        {
            TblReportsTO tblReportsTO = JsonConvert.DeserializeObject<TblReportsTO>(data.ToString());

            if (tblReportsTO == null)
            {
                return null;
            }

            DateTime fromDate = DateTime.MinValue;
            DateTime toDate = DateTime.MinValue;

            if (tblReportsTO.TblFilterReportTOList1 != null && tblReportsTO.TblFilterReportTOList1.Count > 0)
            {
                TblFilterReportTO fromDateTO = tblReportsTO.TblFilterReportTOList1.Where(a => a.SqlDbTypeValue == 33 && a.SqlParameterName == "FromDate").FirstOrDefault();
                if (fromDateTO != null)
                {
                    fromDate = Convert.ToDateTime(fromDateTO.OutputValue);
                }

                TblFilterReportTO toDateTO = tblReportsTO.TblFilterReportTOList1.Where(a => a.SqlDbTypeValue == 33 && a.SqlParameterName == "ToDate").FirstOrDefault();
                if (fromDateTO != null)
                {
                    toDate = Convert.ToDateTime(toDateTO.OutputValue);
                }
            }

            return _iTblReportsBL.SelectWBForPurchaseSaleUnloadReportList(fromDate, toDate);
        }


        //Added by minal
    }
}