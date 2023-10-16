using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblReceiptChargesBL
    {
        DataTable SelectAllTblReceiptCharges();
        List<TblReceiptChargesTO> SelectAllTblReceiptChargesList();
        TblReceiptChargesTO SelectTblReceiptChargesTO(Int32 idReceiptCharges);
        int InsertTblReceiptCharges(TblReceiptChargesTO tblReceiptChargesTO);
        int InsertTblReceiptCharges(TblReceiptChargesTO tblReceiptChargesTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblReceiptCharges(TblReceiptChargesTO tblReceiptChargesTO);
        int UpdateTblReceiptCharges(TblReceiptChargesTO tblReceiptChargesTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblReceiptCharges(Int32 idReceiptCharges);
        int DeleteTblReceiptCharges(Int32 idReceiptCharges, SqlConnection conn, SqlTransaction tran);

    }
}
