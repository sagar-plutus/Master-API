using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblReceiptLinkingBL
    {
        DataTable SelectAllTblReceiptLinking();
        List<TblReceiptLinkingTO> SelectAllTblReceiptLinkingList();
        TblReceiptLinkingTO SelectTblReceiptLinkingTO(Int32 idReceiptLinking);
        int InsertTblReceiptLinking(TblReceiptLinkingTO tblReceiptLinkingTO);
        int InsertTblReceiptLinking(TblReceiptLinkingTO tblReceiptLinkingTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblReceiptLinking(TblReceiptLinkingTO tblReceiptLinkingTO);
        int UpdateTblReceiptLinking(TblReceiptLinkingTO tblReceiptLinkingTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblReceiptLinking(Int32 idReceiptLinking);
        int DeleteTblReceiptLinking(Int32 idReceiptLinking, SqlConnection conn, SqlTransaction tran);
        List<TblBrsBankStatementDtlTO> SelectAllReceiptStatementDtl();
        int UpdateReceiptStatementDtlStatus(TblBrsBankStatementDtlTO tblBrsBankStatementDtlTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage UpdateReceiptStatementDtlStatus(TblBrsBankStatementDtlTO tblBrsBankStatementDtlTO);
    }
}
