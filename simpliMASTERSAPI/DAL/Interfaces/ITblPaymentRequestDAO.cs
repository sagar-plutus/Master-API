using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace simpliMASTERSAPI.DAL.Interfaces
{
    public interface ITblPaymentRequestDAO
    {
        DataTable SelectAllTblPaymentRequest();
        DataTable SelectTblPaymentRequest(Int32 idPayReq);
        DataTable SelectAllTblPaymentRequest(SqlConnection conn, SqlTransaction tran);
        int InsertTblPaymentRequest(TblPaymentRequestTO tblPaymentRequestTO);
        int InsertTblPaymentRequest(TblPaymentRequestTO tblPaymentRequestTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPaymentRequest(TblPaymentRequestTO tblPaymentRequestTO);
        int UpdateTblPaymentRequest(TblPaymentRequestTO tblPaymentRequestTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPaymentRequest(Int32 idPayReq);
        int DeleteTblPaymentRequest(Int32 idPayReq, SqlConnection conn, SqlTransaction tran);
    }
}
