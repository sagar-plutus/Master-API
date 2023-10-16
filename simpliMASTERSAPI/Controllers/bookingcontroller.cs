using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.DashboardModels;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ODLMWebAPI.Controllers
{
    
    [Route("api/[controller]")]
    public class BookingController : Controller
    {

        #region Declaration

        private readonly ILogger loggerObj;
        private readonly ITblConfigParamsBL _iTblConfigParamsBL;
       private readonly ICommon _iCommon;
        private readonly ITblPaymentTermsForBookingBL _iTblPaymentTermsForBookingBL;
        #endregion

        #region Constructor

        public BookingController(ITblPaymentTermsForBookingBL iTblPaymentTermsForBookingBL
            , 
             ICommon iCommon, ILogger<BookingController> logger,
            ITblConfigParamsBL iTblConfigParamsBL)
        {
            loggerObj = logger;
            _iTblConfigParamsBL = iTblConfigParamsBL;
            _iCommon = iCommon;
            _iTblPaymentTermsForBookingBL = iTblPaymentTermsForBookingBL;
            Constants.LoggerObj = logger;
        }

        #endregion

        #region Get


        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("GetBookingOpenCloseForTheDay")]
        [HttpGet]
        public bool GetBookingOpenCloseForTheDay()
        {
            bool allowBooking = true;
            TblConfigParamsTO obj = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.CP_BOOKING_END_TIME);


            //Double a = 0;

            if (obj == null)
            {
                return allowBooking;
            }

            //Double.TryParse(obj.ConfigParamVal, out a);

            if (String.IsNullOrEmpty(obj.ConfigParamVal))
            {
                return allowBooking;
            }


            if (obj.ConfigParamVal == "0" || obj.ConfigParamVal == "00.00")
            {
                return allowBooking;
            }
            else
            {

                DateTime serverDateTime = _iCommon.ServerDateTime;
                //var servertime = Convert.ToDouble(_iCommon.ServerDateTime.ToString("HH:mm"));

                double serveHour = serverDateTime.Hour;
                double ServerMinute = serverDateTime.Minute;


                List<String> list = obj.ConfigParamVal.Split(':').ToList();

                if (list != null && list.Count == 2)
                {
                    Double closureHour = Convert.ToDouble(list[0]);
                    Double closureMin = Convert.ToDouble(list[1]);


                    if (serveHour >= closureHour)
                    {

                        if (serveHour > closureHour)
                            return false;
                        if (ServerMinute > closureMin)
                            return false;
                    }
                }
                //if (Convert.ToDouble(obj.ConfigParamVal) < servertime)
                //    allowBooking = false;
            }
            
            return allowBooking;
        }

     
        [Route("GetMinAndMaxValueConfigForBookingRate")]
        [HttpGet]
        public String GetMinAndMaxValueConfigForBookingRate()
        {
            string configValue = string.Empty;
            TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_BOOKING_RATE_MIN_AND_MAX_BAND);
            if (tblConfigParamsTO != null)
                configValue = Convert.ToString(tblConfigParamsTO.ConfigParamVal);
            return configValue;
        }

     
     
        /// <summary>
        /// Vijaymala[2017-09-11]Added to get booking list to plot graph
        /// </summary>
        /// <param name="OrganizationId"></param>
        /// <param name="userRoleTO"></param>
        /// <returns></returns>
     
     
        /// <summary>
        /// Harshkunj [2018-06-27] Use this call to get booking end time        
        /// </summary>
        [Route("GetAllowedBookingEndTimeDetails")]
        [HttpGet]
        public TblBookingAllowedTimeTO GetAllowedBookingEndTimeDetails(DateTime sysDate)
        {
            if (sysDate == DateTime.MinValue)
                sysDate = _iCommon.ServerDateTime;

                TblBookingAllowedTimeTO tblBookingAllowedTimeTO = new TblBookingAllowedTimeTO();
                TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_BOOKING_END_TIME);
                if(tblConfigParamsTO != null)
            {
                tblBookingAllowedTimeTO.ExtendedHrs = tblConfigParamsTO.ConfigParamVal;

            }
            //Double maxAllowedDelPeriod = Convert.ToDouble(Convert.ToString( tblConfigParamsTO.ConfigParamVal).Replace(" ", string.Empty));
            //DateTime serverDate = _iCommon.ServerDateTime;
            //DateTime curSysDate = Convert.ToDateTime(serverDate.ToShortDateString())  ;
            //DateTime dateToCheck = curSysDate.AddHours(maxAllowedDelPeriod);


            return tblBookingAllowedTimeTO;
        }

        /// <summary>
        /// Priyanka [14-12-2018]
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
     
        /// <summary>
        /// Priyanka [18-01-2019]
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        [Route("GetPaymentTermOptionDetails")]
        [HttpGet]
        public List<TblPaymentTermsForBookingTO> GetPaymentTermOptionDetails(Int32 bookingId = 0, Int32 invoiceId = 0)
        {
            return _iTblPaymentTermsForBookingBL.SelectAllTblPaymentTermsForBookingFromBookingId(bookingId, invoiceId);
        }

        #endregion

   

        #region Put

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        #endregion

        #region Delete

        
        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        #endregion
    }
}
