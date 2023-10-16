using ODLMWebAPI.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace simpliMASTERSAPI.DAL.Interfaces
{
    public interface ITblLoadingDAO
    {
        List<TblLoadingTO> SelectAllLoadingListByStatus(string statusId, SqlConnection conn, SqlTransaction tran, int gateId = 0);
    }
}
