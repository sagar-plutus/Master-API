using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace simpliMASTERSAPI.BL.Interfaces
{
   public interface ITblFundDisbursedBL
    {
        DataTable SelectAllTblFundDisbursed();
        List<TblFundDisbursedTO> SelectAllTblFundDisbursedList();
        TblFundDisbursedTO SelectTblFundDisbursedTO(Int32 idFundDisbursed);
        int InsertTblFundDisbursed(TblFundDisbursedTO tblFundDisbursedTO);
        int InsertTblFundDisbursed(TblFundDisbursedTO tblFundDisbursedTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblFundDisbursed(TblFundDisbursedTO tblFundDisbursedTO);
        int UpdateTblFundDisbursed(TblFundDisbursedTO tblFundDisbursedTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblFundDisbursed(Int32 idFundDisbursed);
        int DeleteTblFundDisbursed(Int32 idFundDisbursed, SqlConnection conn, SqlTransaction tran);

    }
}
