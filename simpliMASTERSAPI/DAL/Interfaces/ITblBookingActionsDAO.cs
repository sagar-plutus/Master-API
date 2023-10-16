using ODLMWebAPI.Models;
using System.Data.SqlClient;

namespace simpliMASTERSAPI.DAL.Interfaces
{
    public interface ITblBookingActionsDAO
    {
        TblBookingActionsTO SelectLatestBookingActionTO(SqlConnection conn, SqlTransaction tran);
        int InsertTblBookingActions(TblBookingActionsTO tblBookingActionsTO, SqlConnection conn, SqlTransaction tran);
    }
}
