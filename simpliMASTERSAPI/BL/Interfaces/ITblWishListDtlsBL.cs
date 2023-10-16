using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using simpliMASTERSAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace simpliMASTERSAPI.BL.Interfaces
{
    public interface ITblWishListDtlsBL
    {

        #region  Wishlist Test 
        int InsertTblWishlistDtls(TblWishListDtlsTO tblWishListDtlsTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage SaveWishlistDetails(TblWishListDtlsTO tblWishListDtlsTO);
        List<TblWishListDtlsTO> SelectTblWishListDtlsTO(Int32 userId);
        TblWishListDtlsTO SelectTblWishListDtlsTOById(Int32 wishlistId);
        ResultMessage UpdateWishListDtls(TblWishListDtlsTO tblWishListDtls);
        ResultMessage DeleteTblWishListDtls(TblWishListDtlsTO tblWishListDtls);
        List<TblUserTO> SelectTblUserDtlsTO(Int32 PageNumber, Int32 RowsPerPage, String strsearchtxt, Int32 UserWishlistId);
        #region Export to excel data

        ResultMessage GetAllUserWishlistDetailsToExport(Int32 userId, string userWishlistIds);
        DimOrgTypeTO SelectDimOrgTypeTO(Int32 userId);
        String SelectReportFullName(String reportName);
        TblConfigParamsTO SelectTblConfigParamsValByName(string configParamName);

        #endregion
        #endregion
    }
}
