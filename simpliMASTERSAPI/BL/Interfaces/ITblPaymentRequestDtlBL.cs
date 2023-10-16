using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace simpliMASTERSAPI.BL.Interfaces
{
   public interface ITblPaymentRequestDtlBL
    {
        DataTable SelectAllTblPaymentRequestDtl();
        List<TblPaymentRequestDtlTO> SelectAllTblPaymentRequestDtlList();
        TblPaymentRequestDtlTO SelectTblPaymentRequestDtlTO(Int32 idPayReqDtl);
        List<TblPaymentRequestDtlTO> ConvertDTToList(DataTable tblPaymentRequestDtlTODT);
        int InsertTblPaymentRequestDtl(TblPaymentRequestDtlTO tblPaymentRequestDtlTO);
        int InsertTblPaymentRequestDtl(TblPaymentRequestDtlTO tblPaymentRequestDtlTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPaymentRequestDtl(TblPaymentRequestDtlTO tblPaymentRequestDtlTO);
        int UpdateTblPaymentRequestDtl(TblPaymentRequestDtlTO tblPaymentRequestDtlTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPaymentRequestDtl(Int32 idPayReqDtl);
        int DeleteTblPaymentRequestDtl(Int32 idPayReqDtl, SqlConnection conn, SqlTransaction tran);
    }
}
