using simpliMASTERSAPI.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System;
using ODLMWebAPI.Models;

namespace simpliMASTERSAPI.DAL.Interfaces
{
    public interface ITblWishListDtlsDAO
    {
        #region  Wishlist Test 
        int InsertTblWishlistDtls(TblWishListDtlsTO tblWishListDtlsTO);
        List<TblWishListDtlsTO> SelectTblWishListDtls(Int32 userId, SqlConnection conn, SqlTransaction tran);
        TblWishListDtlsTO SelectTblWishListDtlsById(Int32 wishlistId, SqlConnection conn, SqlTransaction tran);
        int UpdatetblWishListDtl(TblWishListDtlsTO tblWishListDtls);
        int DeleteTblWishListDtl(TblWishListDtlsTO tblWishListDtls);
        List<TblUserTO> SelectTblUserDtls(Int32 PageNumber, Int32 RowsPerPage, String strsearchtxt, Int32 UserWishlistId, SqlConnection conn, SqlTransaction tran);
        
        #region Export to excel data
        List<TblUserTO> SelectAllUserWishlistDetailsToExport(Int32 userId, string userWishlistIds);
        List<TblWishListDtlsTO> SelectAllUserWishlistDetailsToExport(string userWishlistIds);
        DimOrgTypeTO SelectDimOrgType(Int32 idOrgType, SqlConnection conn, SqlTransaction tran);
        DimReportTemplateTO SelectDimReportTemplate(String reportName);
        TblConfigParamsTO SelectTblConfigParamsValByName(string configParamName);
        #endregion

        #endregion
    }
}
