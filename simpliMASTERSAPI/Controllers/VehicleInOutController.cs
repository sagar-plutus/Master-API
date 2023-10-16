using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using simpliMASTERSAPI.BL.Interfaces;
using simpliMASTERSAPI.TO;
using ODLMWebAPI.StaticStuff;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using simpliMASTERSAPI.DAL.Interfaces;
using ODLMWebAPI.Models;

namespace simpliMASTERSAPI.Controllers
{
    [Route("api/VehicleInOut")]
    public class VehicleInOutController : Controller
    {
        #region Declaration
        private readonly ITblVehicleInOutDetailsBL _iTblVehicleInOutDetailsBL;
        private readonly ITblPurchaseVehicleSpotEntryBL _iTblPurchaseVehicleSpotEntryBL;
        public VehicleInOutController(ITblVehicleInOutDetailsBL iTblVehicleInOutDetailsBL, ITblPurchaseVehicleSpotEntryBL iTblPurchaseVehicleSpotEntryBL)
        {
            _iTblVehicleInOutDetailsBL = iTblVehicleInOutDetailsBL;
            _iTblPurchaseVehicleSpotEntryBL = iTblPurchaseVehicleSpotEntryBL;

        }
        #endregion
        #region Get
        [Route("GetVehicleINOutDetailsList")]
        [HttpGet]
        public List<TblVehicleInOutDetailsTO> GetVehicleINOutDetailsList(Int32 moduleId,Int32 showVehicleInOut,string fromDate,string toDate,bool skipDatetime,string statusStr)
        {
            List<TblVehicleInOutDetailsTO> TblVehicleInOutDetailsTOList = _iTblVehicleInOutDetailsBL.SelectAllTblVehicleInOutDetailsList(moduleId,showVehicleInOut, fromDate, toDate, skipDatetime, statusStr);           
            return TblVehicleInOutDetailsTOList;
        }

        [Route("GetVehicleINOutDetailsListById")]
        [HttpGet]
        public TblVehicleInOutDetailsTO GetVehicleINOutDetailsListById(Int32 moduleId, Int32 idVehicleInOut)
        {
            TblVehicleInOutDetailsTO TblVehicleInOutDetailsTO = _iTblVehicleInOutDetailsBL.SelectAllTblVehicleInOutDetailsById(moduleId, idVehicleInOut);
            return TblVehicleInOutDetailsTO;
        }
        #endregion
        #region post
        [Route("UpdateVehicleINOutStatus")]
        [HttpPost]
        public ResultMessage UpdateVehicleINOutStatus([FromBody] JObject data)
        {
            TblVehicleInOutDetailsTO tblVehicleInOutDetailsTO = JsonConvert.DeserializeObject<TblVehicleInOutDetailsTO>(data["tblVehicleInOutDetailsTO"].ToString());

            ResultMessage resultMessage = new ResultMessage();
            try
            {
                resultMessage  = _iTblVehicleInOutDetailsBL.UpdateTblVehicleInOutDetailsStatusOnly(tblVehicleInOutDetailsTO);
                if(resultMessage.Result <= 0)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = -1;
                    resultMessage.Text = "Exception Error in UpdateVehicleINOutStatus";
                    return resultMessage;
                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Tag = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "Exception Error in UpdateVehicleINOutStatus";
                return resultMessage;
            }
        }


        [Route("GetVehicleNumberList")]
        [HttpGet]
        public List<VehicleNumber> GetVehicleNumberList()
        {
            return _iTblVehicleInOutDetailsBL.SelectAllVehicles();
        }

        [Route("GetPONoListAgainstSupplier")]
        [HttpGet]
        public List<DropDownTO> GetPONoListAgainstSupplier(Int64 supplierId)
        {
            return _iTblVehicleInOutDetailsBL.GetPONoListAgainstSupplier(supplierId);
        }


        [Route("PostVehicleSpotEnteryDetails")]
        [HttpPost]
        public ResultMessage PostVehicleSpotEnteryDetails([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO = JsonConvert.DeserializeObject<TblPurchaseVehicleSpotEntryTO>(data["purchaseVehicleSpotEntryTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : Login User ID Found NULL";
                    return resultMessage;
                }

                if (tblPurchaseVehicleSpotEntryTO == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "API : PostVehicleSpotEnteryDetails Found NULL";
                    return resultMessage;
                }

                return _iTblPurchaseVehicleSpotEntryBL.SaveVehicleSpotEntry(tblPurchaseVehicleSpotEntryTO, Convert.ToInt32(loginUserId));

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, " PostVehicleSpotEnteryDetails");
                return resultMessage;
            }


        }


        [Route("GetVehicleSportEntrydetails")]
        [HttpGet]
        public List<TblPurchaseVehicleSpotEntryTO> GetVehicleSportEntrydetails(string fromDate, string toDate, String loginUserId, Int32 moduleId, Int32 id = 2, bool skipDatetime = false)
        {
            DateTime from_Date = DateTime.MinValue;
            DateTime to_Date = DateTime.MinValue;

            if (Constants.IsDateTime(fromDate))
                from_Date = Convert.ToDateTime(Convert.ToDateTime(fromDate).ToString(Constants.AzureDateFormat));
            if (Constants.IsDateTime(toDate))
                to_Date = Convert.ToDateTime(Convert.ToDateTime(toDate).ToString(Constants.AzureDateFormat));

            List<TblPurchaseVehicleSpotEntryTO> tblPurchaseVehicleSpotEntryTOList = _iTblPurchaseVehicleSpotEntryBL.SelectAllSpotEntryVehicles(from_Date, to_Date, moduleId,loginUserId, id, skipDatetime);
            return tblPurchaseVehicleSpotEntryTOList;
        }

        [Route("UpdateSpotEntryVehicleSupplier")]
        [HttpPost]
        public ResultMessage UpdateSpotEntryVehicleSupplier([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();
            TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO = JsonConvert.DeserializeObject<TblPurchaseVehicleSpotEntryTO>(data["purchaseVehicleSpotEntryTO"].ToString());
            Int32 loginUserId = (Int32)(data["loginUserId"]);

            resultMessage = _iTblPurchaseVehicleSpotEntryBL.UpdateSpotEntryVehicleSupplier(tblPurchaseVehicleSpotEntryTO, loginUserId);
            return resultMessage;
        }
        [Route("PostUpdatedSpotEntryDetails")]
        [HttpPost]
        public ResultMessage PostUpdatedSpotEntryDetails([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();
            TblPurchaseVehicleSpotEntryTO tblPurchaseVehicleSpotEntryTO = JsonConvert.DeserializeObject<TblPurchaseVehicleSpotEntryTO>(data["purchaseVehicleSpotEntryTO"].ToString());
            Int32 result = 0;

            if (tblPurchaseVehicleSpotEntryTO != null && tblPurchaseVehicleSpotEntryTO != null)
            {
                result = _iTblPurchaseVehicleSpotEntryBL.UpdateTblPurchaseVehicleSpotEntry(tblPurchaseVehicleSpotEntryTO);
                if (result >= 1)
                {
                    resultMessage.MessageType = ResultMessageE.Information;
                    resultMessage.Result = 1;
                    resultMessage.Text = "Record Updated Successfully";
                }
                else
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "API: Failed To Update Record";
                }
            }
            return resultMessage;
        }

        [Route("GetActiveUsersDropDownListByRoleTypeIdWithVehAllocation")]
        [HttpGet]
        public List<DropDownTO> GetActiveUsersDropDownListByRoleTypeIdWithVehAllocation(Int32 RoleTypeId, Int32 nameWithCount = 1)
        {
            List<DropDownTO> userList = _iTblVehicleInOutDetailsBL.SelectAllSystemUsersFromRoleTypeWithVehAllocation(RoleTypeId, nameWithCount);
            return userList;
        }

        [Route("SelectAllSystemUsersFromRoleType")]
        [HttpGet]
        public List<DropDownTO> SelectAllSystemUsersFromRoleType(Int32 RoleTypeId)
        {
            List<DropDownTO> userList = _iTblVehicleInOutDetailsBL.SelectAllSystemUsersFromRoleType(RoleTypeId);
            return userList;
        }
        // Add By Samadhan 09 Nov 2022
        [Route("GetActiveUsersDropDownListforUnloadingSuperwisorVehicleIn")]
        [HttpGet]
        public List<DropDownTO> GetActiveUsersDropDownListforUnloadingSuperwisorVehicleIn()
        {
            List<DropDownTO> userList = _iTblVehicleInOutDetailsBL.SelectAllSystemUsersforUnloadingSuperwisorVehicleIn();
            return userList;
        }


        #endregion
    }
}
