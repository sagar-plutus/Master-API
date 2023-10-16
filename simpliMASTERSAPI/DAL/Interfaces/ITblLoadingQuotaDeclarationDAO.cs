using ODLMWebAPI.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace simpliMASTERSAPI.DAL.Interfaces
{
    public interface ITblLoadingQuotaDeclarationDAO
    {
        List<TblLoadingQuotaDeclarationTO> SelectLatestCalculatedLoadingQuotaDeclarationList(DateTime stockDate, Int32 cnfId, SqlConnection conn, SqlTransaction tran);
        int InsertTblLoadingQuotaDeclaration(TblLoadingQuotaDeclarationTO tblLoadingQuotaDeclarationTO, SqlConnection conn, SqlTransaction tran);
        int DeactivateAllPrevLoadingQuota(Int32 updatedBy, SqlConnection conn, SqlTransaction tran);
    }
}
