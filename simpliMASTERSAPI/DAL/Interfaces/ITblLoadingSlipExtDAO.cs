using ODLMWebAPI.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace simpliMASTERSAPI.DAL.Interfaces
{
    public interface ITblLoadingSlipExtDAO
    {
        List<TblLoadingSlipExtTO> SelectAllLoadingSlipExtListFromLoadingId(String loadingIds, SqlConnection conn, SqlTransaction tran);
    }
}
