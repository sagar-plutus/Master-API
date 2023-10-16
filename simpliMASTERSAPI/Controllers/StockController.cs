using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ODLMWebAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ODLMWebAPI.StaticStuff;
using System.Net.Http;
using System.Dynamic;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL;
using simpliMASTERSAPI.BL.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ODLMWebAPI.Controllers
{
    
    [Route("api/[controller]")]
    public class StockController : Controller
    {
        private readonly ITblLocationBL _iTblLocationBL;
        private readonly IDimProdCatBL _iDimProdCatBL;
        private readonly IDimProdSpecBL _iDimProdSpecBL;
        private readonly ITblStockSummaryBL _iTblStockSummaryBL;
        private readonly ICommon _iCommon;
        public StockController(ICommon iCommon,
             IDimProdSpecBL iDimProdSpecBL,
            IDimProdCatBL iDimProdCatBL, ITblLocationBL iTblLocationBL, ITblStockSummaryBL iTblStockSummaryBL)
        {
            _iTblLocationBL = iTblLocationBL;
            _iDimProdCatBL = iDimProdCatBL;
            _iDimProdSpecBL = iDimProdSpecBL;
           
            _iCommon = iCommon;
            _iTblStockSummaryBL = iTblStockSummaryBL;
        }
        #region Get

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("GetStockLocationsForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetStockLocationsForDropDown()
        {
            List<TblLocationTO> tblLocationTOList = _iTblLocationBL.SelectAllParentLocation();
            if (tblLocationTOList != null && tblLocationTOList.Count > 0)
            {
                List<DropDownTO> statusReasonList = new List<Models.DropDownTO>();
                for (int i = 0; i < tblLocationTOList.Count; i++)
                {
                    DropDownTO dropDownTO = new DropDownTO();
                    dropDownTO.Text = tblLocationTOList[i].LocationDesc;
                    dropDownTO.Value = tblLocationTOList[i].IdLocation;
                    statusReasonList.Add(dropDownTO);
                }
                return statusReasonList;
            }
            else return null;
        }

        [Route("GetCompartmentsForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetCompartmentsForDropDown(Int32 locationId)
        {
            List<TblLocationTO> tblLocationTOList = _iTblLocationBL.SelectAllCompartmentLocationList(locationId);
            if (tblLocationTOList != null && tblLocationTOList.Count > 0)
            {
                List<DropDownTO> statusReasonList = new List<Models.DropDownTO>();
                for (int i = 0; i < tblLocationTOList.Count; i++)
                {
                    DropDownTO dropDownTO = new DropDownTO();
                    dropDownTO.Text = tblLocationTOList[i].LocationDesc;
                    dropDownTO.Value = tblLocationTOList[i].IdLocation;
                    statusReasonList.Add(dropDownTO);
                }
                return statusReasonList;
            }
            else return null;
        }

        [Route("GetLocationAndCompartmentsForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetLocationAndCompartmentsForDropDown(Boolean onlyCompartments = true)
        {
            return _iTblLocationBL.SelectAllLocationAndCompartmentsListForDropDown(onlyCompartments);

        }
        //Reshma Added For finding location Id From Warehouse Id
        [Route("GetLocationDetailsFromWarehouse")]
        [HttpGet]
        public List<DropDownTO> GetLocationDetailsFromWarehouse(Int32 warehouseId)
        {
            List<DropDownTO> tblLocationTOList = _iTblLocationBL.SelectLocationFromWarehouse(warehouseId);
            if (tblLocationTOList != null && tblLocationTOList.Count > 0)
            {
                return tblLocationTOList;
            }
            else return null;
        }


        [Route("GetProdCategoryForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetProdCategoryForDropDown()
        {
            List<DimProdCatTO> dimProdCatTOList = _iDimProdCatBL.SelectAllDimProdCatList();
            if (dimProdCatTOList != null && dimProdCatTOList.Count > 0)
            {
                List<DropDownTO> statusReasonList = new List<Models.DropDownTO>();
                for (int i = 0; i < dimProdCatTOList.Count; i++)
                {
                    DropDownTO dropDownTO = new DropDownTO();
                    dropDownTO.Text = dimProdCatTOList[i].ProdCateDesc;
                    dropDownTO.Value = dimProdCatTOList[i].IdProdCat;
                    statusReasonList.Add(dropDownTO);
                }
                return statusReasonList;
            }
            else return null;
        }

        [Route("GetProdSepcificationsForDropDown")]
        [HttpGet]
        public List<DropDownTO> GetProdSepcificationsForDropDown()
        {
            List<DimProdSpecTO> dimProdSpecTOList = _iDimProdSpecBL.SelectAllDimProdSpecList();
            if (dimProdSpecTOList != null && dimProdSpecTOList.Count > 0)
            {
                List<DropDownTO> statusReasonList = new List<Models.DropDownTO>();
                for (int i = 0; i < dimProdSpecTOList.Count; i++)
                {

                    if (dimProdSpecTOList[i].IsActive == 1)  //Saket [2018-01-30] Added
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        dropDownTO.Text = dimProdSpecTOList[i].ProdSpecDesc;
                        dropDownTO.Value = dimProdSpecTOList[i].IdProdSpec;
                        statusReasonList.Add(dropDownTO);
                    }
                }
                return statusReasonList;
            }
            else return null;
        }

     
        /// <summary>
        /// Sanjay [2017-05-03] To Get All the compartment whose stock for the given date is not taken
        /// </summary>
        /// <param name="stockDate"></param>
        /// <returns></returns>
        [Route("GetStockNotTakenCompartmentList")]
        [HttpGet]
        public List<TblLocationTO> GetStockNotTakenCompartmentList(DateTime stockDate)
        {
            if (stockDate == DateTime.MinValue)
                stockDate = _iCommon.ServerDateTime.Date;

            return _iTblLocationBL.SelectStkNotTakenCompartmentList(stockDate);
        }


        #endregion

        #region Post

        // POST api/values

        [Route("PostDailyStockUpdate")]
        [HttpPost]
        public ResultMessage PostDailyStockUpdate([FromBody] JObject data)
        {
            ResultMessage returnMsg = new StaticStuff.ResultMessage();
            try
            {
                TblStockSummaryTO stockSummaryTO = JsonConvert.DeserializeObject<TblStockSummaryTO>(data["stockSummaryTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                if (stockSummaryTO == null)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : Stock Object Found Null";
                    return returnMsg;
                }
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : UserID Found Null";
                    return returnMsg;
                }

                if (stockSummaryTO.StockDetailsTOList == null || stockSummaryTO.StockDetailsTOList.Count == 0)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : StockDetailsTOList Found Null";
                    return returnMsg;
                }


                for (int i = 0; i < stockSummaryTO.StockDetailsTOList.Count; i++)
                {
                    stockSummaryTO.StockDetailsTOList[i].CreatedBy = Convert.ToInt32(loginUserId);
                    stockSummaryTO.StockDetailsTOList[i].UpdatedBy = Convert.ToInt32(loginUserId);
                    stockSummaryTO.StockDetailsTOList[i].CreatedOn = Constants.ServerDateTime;
                    stockSummaryTO.StockDetailsTOList[i].UpdatedOn = Constants.ServerDateTime;
                }

                stockSummaryTO.CreatedOn = Constants.ServerDateTime;
                stockSummaryTO.CreatedBy = Convert.ToInt32(loginUserId);
                return _iTblStockSummaryBL.UpdateDailyStock(stockSummaryTO);
            }
            catch (Exception ex)
            {
                returnMsg.MessageType = ResultMessageE.Error;
                returnMsg.Result = -1;
                returnMsg.Exception = ex;
                returnMsg.Text = "API : Exception Error While DailyStockUpdate";
                return returnMsg;
            }
        }

        // POST api/values

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        [Route("PostStockSummaryConfirmation")]
        [HttpPost]
        public ResultMessage PostStockSummaryConfirmation([FromBody] JObject data)
        {
            ResultMessage returnMsg = new StaticStuff.ResultMessage();
            try
            {
                List<SizeSpecWiseStockTO> sizeSpecWiseStockTOList = JsonConvert.DeserializeObject<List<SizeSpecWiseStockTO>>(data["sizeSpecWiseStockTOList"].ToString());
                var loginUserId = data["loginUserId"].ToString();


                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : UserID Found Null";
                    return returnMsg;
                }

                if (sizeSpecWiseStockTOList == null || sizeSpecWiseStockTOList.Count == 0)
                {
                    returnMsg.MessageType = ResultMessageE.Error;
                    returnMsg.Result = 0;
                    returnMsg.Text = "API : sizeSpecWiseStockTOList Found Null";
                    return returnMsg;
                }

                DateTime confirmedDate = Constants.ServerDateTime;
                for (int i = 0; i < sizeSpecWiseStockTOList.Count; i++)
                {
                    sizeSpecWiseStockTOList[i].ConfirmedBy = Convert.ToInt32(loginUserId);
                    sizeSpecWiseStockTOList[i].ConfirmedOn = confirmedDate;
                }

                ResultMessage resMsg = _iTblStockSummaryBL.ConfirmStockSummary(sizeSpecWiseStockTOList);
                return resMsg;
            }
            catch (Exception ex)
            {
                returnMsg.MessageType = ResultMessageE.Error;
                returnMsg.Result = -1;
                returnMsg.Exception = ex;
                returnMsg.Text = "API : Exception Error While PostStockSummaryConfirmation";
                return returnMsg;
            }
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
