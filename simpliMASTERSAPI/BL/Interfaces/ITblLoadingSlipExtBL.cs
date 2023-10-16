using ODLMWebAPI.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace simpliMASTERSAPI.BL.Interfaces
{
    public interface ITblLoadingSlipExtBL
    {
        List<TblLoadingSlipExtTO> SelectCnfWiseLoadingMaterialToPostPoneList(SqlConnection conn, SqlTransaction tran);
    }
}
