using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace simpliMASTERSAPI.BL.Interfaces
{
   public interface ITblPaymentRequestBL
    {
        DataTable SelectAllTblPaymentRequest();
        List<TblPaymentRequestTO> SelectAllTblPaymentRequestList();
        TblPaymentRequestTO SelectTblPaymentRequestTO(Int32 idPayReq);
        int InsertTblPaymentRequest(TblPaymentRequestTO tblPaymentRequestTO);
        int InsertTblPaymentRequest(TblPaymentRequestTO tblPaymentRequestTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPaymentRequest(TblPaymentRequestTO tblPaymentRequestTO);
        int UpdateTblPaymentRequest(TblPaymentRequestTO tblPaymentRequestTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPaymentRequest(Int32 idPayReq);
        int DeleteTblPaymentRequest(Int32 idPayReq, SqlConnection conn, SqlTransaction tran);
    }
}
