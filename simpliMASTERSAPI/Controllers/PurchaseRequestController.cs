using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ODLMWebAPI.BL;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using simpliMASTERSAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;


namespace ODLMWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class PurchaseRequestController : Controller
    {
     
        private readonly ITblStoreAccessBL _iTblStoreAccessBL;
        private readonly ICommon _iCommon;

        public PurchaseRequestController(ICommon iCommon, ITblStoreAccessBL iTblStoreAccessBL)
        {
            
            _iTblStoreAccessBL =iTblStoreAccessBL;
            _iCommon = iCommon;
        }

       #region Get
  
       
       
       
        
        [Route("GetStoresDropDownList")]
        [HttpGet]
        public List<TblStoreAccessTO> GetStoresDropDownList( Int32 userId)
        {

            List<TblStoreAccessTO> tblStoreAccessTOList = _iTblStoreAccessBL.SelectAllTblStoreAccess(userId);
            return tblStoreAccessTOList;

        }
        [Route("GetStoresLocationDropDownList")]
        [HttpGet]
        public List<DropDownTO> GetStoresLocationDropDownList(Int32 userId)
        {
            List<DropDownTO> dropDownTOList = _iTblStoreAccessBL.GetStoresLocationDropDownList(userId);
            return dropDownTOList;

        }
   
        #endregion

        #region Insert 


      

        /// <summary>
        /// Priyanka H [05/09/2019]
        /// </summary>
        /// <param name="tblStoreAccessTO"></param>
        /// <returns></returns>
        [Route("PostStorAccess")]
        [HttpPost]
        public ResultMessage PostStorAccess([FromBody] TblStoreAccessTO tblStoreAccessTO)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                tblStoreAccessTO.CreatedOn = _iCommon.ServerDateTime;

                var loginUserId = tblStoreAccessTO.CreatedBy;
                if (tblStoreAccessTO == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "tblStoreAccessTO Found NULL";
                    return resultMessage;

                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "loginUserId Found NULL";
                    return resultMessage;
                }

                return _iTblStoreAccessBL.InsertTblStoreAccess(tblStoreAccessTO);

            }
            catch (Exception ex)
            {
                resultMessage.DefaultBehaviour();
                resultMessage.Text = "Exception Error in API Call";
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                return resultMessage;
            }
        }
        #endregion

        #region update 
        // POST api/values
       
        /// <summary>
        /// Priyanka H [05/09/2019]
        /// </summary>
        /// <param name="tblStoreAccessTO"></param>
        /// <returns></returns>
        [Route("PostStorAccessUpdate")]
        [HttpPost]
        public ResultMessage PostStorAccessUpdate([FromBody] TblStoreAccessTO tblStoreAccessTO)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                
                var loginUserId = tblStoreAccessTO.CreatedBy;
                if (tblStoreAccessTO == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "tblPurchaseRequestTo Found NULL";
                    return resultMessage;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "loginUserId Found NULL";
                    return resultMessage;
                }
                
                return _iTblStoreAccessBL.UpdateTblStoreAccess(tblStoreAccessTO);

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Tag = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "Exception Error in Method PostBookingUpdate";
                return resultMessage;
            }
        }

        #endregion


    }
}