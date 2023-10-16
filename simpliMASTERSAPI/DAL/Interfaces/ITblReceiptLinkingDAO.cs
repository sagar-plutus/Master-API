using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblReceiptLinkingDAO
    {
        DataTable SelectAllTblReceiptLinking();
        DataTable SelectTblReceiptLinking(Int32 idReceiptLinking);
        DataTable SelectAllTblReceiptLinking(SqlConnection conn, SqlTransaction tran);
        int InsertTblReceiptLinking(TblReceiptLinkingTO tblReceiptLinkingTO);
        int InsertTblReceiptLinking(TblReceiptLinkingTO tblReceiptLinkingTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblReceiptLinking(TblReceiptLinkingTO tblReceiptLinkingTO);
        int UpdateTblReceiptLinking(TblReceiptLinkingTO tblReceiptLinkingTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblReceiptLinking(Int32 idReceiptLinking);
        int DeleteTblReceiptLinking(Int32 idReceiptLinking, SqlConnection conn, SqlTransaction tran);
        //chetan[2020-10-27] added 
        DataTable SelectAllReceiptStatementDtl();
        int UpdateReceiptStatementDtlStatus(TblBrsBankStatementDtlTO tblBrsBankStatementDtlTO, SqlConnection conn, SqlTransaction tran);
    }
}
