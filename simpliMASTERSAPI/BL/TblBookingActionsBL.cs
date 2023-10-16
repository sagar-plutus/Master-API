using ODLMWebAPI.Models;
using simpliMASTERSAPI.BL.Interfaces;
using simpliMASTERSAPI.DAL;
using simpliMASTERSAPI.DAL.Interfaces;
using System.Data.SqlClient;

namespace simpliMASTERSAPI.BL
{
    public class TblBookingActionsBL : ITblBookingActionsBL
    {
        private readonly ITblBookingActionsDAO _tblBookingActionsDAO;
        public TblBookingActionsBL(ITblBookingActionsDAO tblBookingActionsDAO) 
        {
            _tblBookingActionsDAO = tblBookingActionsDAO;
        }
        public int InsertTblBookingActions(TblBookingActionsTO tblBookingActionsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _tblBookingActionsDAO.InsertTblBookingActions(tblBookingActionsTO, conn, tran);
        }

        public TblBookingActionsTO SelectLatestBookingActionTO(SqlConnection conn, SqlTransaction tran)
        {
            return _tblBookingActionsDAO.SelectLatestBookingActionTO(conn, tran);
        }
    }
}
