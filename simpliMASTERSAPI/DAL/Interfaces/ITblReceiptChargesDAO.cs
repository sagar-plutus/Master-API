using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblReceiptChargesDAO
    {
        DataTable SelectAllTblReceiptCharges();
        DataTable SelectTblReceiptCharges(Int32 idReceiptCharges);
        DataTable SelectAllTblReceiptCharges(SqlConnection conn, SqlTransaction tran);
        int InsertTblReceiptCharges(TblReceiptChargesTO tblReceiptChargesTO);
        int InsertTblReceiptCharges(TblReceiptChargesTO tblReceiptChargesTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblReceiptCharges(TblReceiptChargesTO tblReceiptChargesTO);
        int UpdateTblReceiptCharges(TblReceiptChargesTO tblReceiptChargesTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblReceiptCharges(Int32 idReceiptCharges);
        int DeleteTblReceiptCharges(Int32 idReceiptCharges, SqlConnection conn, SqlTransaction tran);
    }
}
