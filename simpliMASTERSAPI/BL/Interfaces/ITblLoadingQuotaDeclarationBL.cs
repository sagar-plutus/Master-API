using ODLMWebAPI.Models;
using System.Collections.Generic;
using System;
using System.Data.SqlClient;

namespace simpliMASTERSAPI.BL.Interfaces
{
    public interface ITblLoadingQuotaDeclarationBL
    {
        int InsertTblLoadingQuotaDeclaration(TblLoadingQuotaDeclarationTO tblLoadingQuotaDeclarationTO, SqlConnection conn, SqlTransaction tran);
        List<TblLoadingQuotaDeclarationTO> SelectLatestCalculatedLoadingQuotaDeclarationList(DateTime stockDate, Int32 cnfOrgId, SqlConnection conn, SqlTransaction tran);
    }
}
