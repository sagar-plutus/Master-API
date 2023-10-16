using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace simpliMASTERSAPI.BL.Interfaces
{
    public interface ITblBankAmountBL
    {
        DataTable SelectAllTblBankAmount();
        List<TblBankAmountTO> SelectAllTblBankAmountList();
        TblBankAmountTO SelectTblBankAmountTO(Int32 idBankAmount);
        int InsertTblBankAmount(TblBankAmountTO tblBankAmountTO);
        int InsertTblBankAmount(TblBankAmountTO tblBankAmountTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblBankAmount(TblBankAmountTO tblBankAmountTO);
        int DeleteTblBankAmount(Int32 idBankAmount);
        int DeleteTblBankAmount(Int32 idBankAmount, SqlConnection conn, SqlTransaction tran);
    }
}
