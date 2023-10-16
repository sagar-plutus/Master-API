using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace simpliMASTERSAPI.DAL.Interfaces
{
    public interface ITblBankAmountDAO
    {
        DataTable SelectAllTblBankAmount();
        DataTable SelectTblBankAmount(Int32 idBankAmount);
        DataTable SelectAllTblBankAmount(SqlConnection conn, SqlTransaction tran);
        int InsertTblBankAmount(TblBankAmountTO tblBankAmountTO);
        int InsertTblBankAmount(TblBankAmountTO tblBankAmountTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblBankAmount(TblBankAmountTO tblBankAmountTO);
        int UpdateTblBankAmount(TblBankAmountTO tblBankAmountTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblBankAmount(Int32 idBankAmount);
        int DeleteTblBankAmount(Int32 idBankAmount, SqlConnection conn, SqlTransaction tran);
    }
}
