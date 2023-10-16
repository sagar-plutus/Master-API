using ODLMWebAPI.Models;
using System.Data.SqlClient;

namespace simpliMASTERSAPI.BL.Interfaces
{
    public interface ITblBookingActionsBL
    {
        int InsertTblBookingActions(TblBookingActionsTO tblBookingActionsTO, SqlConnection conn, SqlTransaction tran);
        TblBookingActionsTO SelectLatestBookingActionTO(SqlConnection conn, SqlTransaction tran);
    }
}
