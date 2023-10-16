using Microsoft.AspNetCore.Mvc;
using ODLMWebAPI.StaticStuff;
using simpliMASTERSAPI.Models;
using System.Collections.Generic;
using System;
using ODLMWebAPI.BL.Interfaces;
using simpliMASTERSAPI.BL.Interfaces;
using ODLMWebAPI.BL;
using ODLMWebAPI.Models;

namespace simpliMASTERSAPI.Controllers
{

    [Route("api/[controller]")]
    public class WishlistController : Controller
    {
        private readonly ITblWishListDtlsBL _iTblWishListDtlsBL;

        public WishlistController(ITblWishListDtlsBL iTblWishListDtlsBL)
        {
            _iTblWishListDtlsBL = iTblWishListDtlsBL;
        }

        #region  Wishlist Test 

        [Route("PostWishlistDetails")]
        [HttpPost]
        public ResultMessage PostWishlistDetails([FromBody] TblWishListDtlsTO data)
        {
            ResultMessage resMsg = new ResultMessage();
            try
            {
                if (data == null)
                {
                    resMsg.DefaultBehaviour();
                    resMsg.Text = "Wishlist Details found null";
                    resMsg.DisplayMessage = Constants.DefaultErrorMsg;
                    resMsg.Result = 0;
                    return resMsg;
                }
                return _iTblWishListDtlsBL.SaveWishlistDetails(data);
            }
            catch (Exception ex)
            {
                resMsg.MessageType = ResultMessageE.Error;
                resMsg.Result = -1;
                resMsg.Exception = ex;
                resMsg.Text = "Exception Error IN API Call PostWishlistDetails :" + ex;
                resMsg.DisplayMessage = Constants.DefaultErrorMsg;
                return resMsg;
            }
        }

        [Route("GetWishlistDetails")]
        [HttpGet]
        public List<TblWishListDtlsTO> GetWishlistDetails(Int32 userId)
        {
            List<TblWishListDtlsTO> getTblWishListDtlsTO = _iTblWishListDtlsBL.SelectTblWishListDtlsTO(userId);
            return getTblWishListDtlsTO;
        }


        [Route("GetWishlistDetailsById")]
        [HttpGet]
        public TblWishListDtlsTO GetWishlistDetailsById(Int32 wishlistId)
        {
            TblWishListDtlsTO getTblWishListDtlsTO = _iTblWishListDtlsBL.SelectTblWishListDtlsTOById(wishlistId);
            return getTblWishListDtlsTO;
        }

        [Route("UpdateWishlistDetailsById")]
        [HttpPost]
        public ResultMessage UpdateWishlistDetailsById([FromBody] TblWishListDtlsTO data)
        {
            ResultMessage resMsg = new ResultMessage();
            try
            {
                if (data == null)
                {
                    resMsg.DefaultBehaviour();
                    resMsg.Text = "Wishlist Details found null";
                    resMsg.DisplayMessage = Constants.DefaultErrorMsg;
                    return resMsg;
                }
                return _iTblWishListDtlsBL.UpdateWishListDtls(data);
            }
            catch (Exception ex)
            {
                resMsg.MessageType = ResultMessageE.Error;
                resMsg.Result = -1;
                resMsg.Exception = ex;
                resMsg.Text = "Exception Error IN API Call UpdateWishlistDetailsById :" + ex;
                resMsg.DisplayMessage = Constants.DefaultErrorMsg;
                return resMsg;
            }
        }

        [Route("DeleteWishlistDetailsById")]
        [HttpPost]
        public ResultMessage DeleteWishlistDetailsById([FromBody] TblWishListDtlsTO data)
        {
            ResultMessage resMsg = new ResultMessage();
            try
            {
                if (data.WishlistId == 0)
                {
                    resMsg.DefaultBehaviour();
                    resMsg.Text = "Wishlist details found null";
                    resMsg.DisplayMessage = Constants.DefaultErrorMsg;
                    return resMsg;
                }
                return _iTblWishListDtlsBL.DeleteTblWishListDtls(data);
            }
            catch (Exception ex)
            {
                resMsg.MessageType = ResultMessageE.Error;
                resMsg.Result = -1;
                resMsg.Exception = ex;
                resMsg.Text = "Exception Error IN API Call DeleteWishlistDetailsById :" + ex;
                resMsg.DisplayMessage = Constants.DefaultErrorMsg;
                return resMsg;
            }
        }

        [Route("GetUserDetails")]
        [HttpGet]
        public List<TblUserTO> GetUserDetails(Int32 UserWishlistId, Int32 PageNumber, Int32 RowsPerPage, String strsearchtxt)
        {
            List<TblUserTO> getTblWishListDtlsTO = _iTblWishListDtlsBL.SelectTblUserDtlsTO(PageNumber, RowsPerPage, strsearchtxt, UserWishlistId);
            return getTblWishListDtlsTO;
        }

        #region Export to excel data

        [Route("GetAllUserWishlistDetailsToExport")]
        [HttpGet]
        public ResultMessage GetAllUserWishlistDetailsToExport(Int32 userId,string userWishlistIds)
        {
            ResultMessage resultMessage = new ResultMessage();
            return resultMessage = _iTblWishListDtlsBL.GetAllUserWishlistDetailsToExport (userId, userWishlistIds);
        }
        #endregion

        #endregion
    }
}
