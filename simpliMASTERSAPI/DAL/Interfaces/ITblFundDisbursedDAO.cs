using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace simpliMASTERSAPI.DAL.Interfaces
{
    public interface ITblFundDisbursedDAO
    {
        DataTable SelectAllTblFundDisbursed();
        DataTable SelectTblFundDisbursed(Int32 idFundDisbursed);
        DataTable SelectAllTblFundDisbursed(SqlConnection conn, SqlTransaction tran);
        int InsertTblFundDisbursed(TblFundDisbursedTO tblFundDisbursedTO);
        int InsertTblFundDisbursed(TblFundDisbursedTO tblFundDisbursedTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblFundDisbursed(TblFundDisbursedTO tblFundDisbursedTO);
        int UpdateTblFundDisbursed(TblFundDisbursedTO tblFundDisbursedTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblFundDisbursed(Int32 idFundDisbursed);
        int DeleteTblFundDisbursed(Int32 idFundDisbursed, SqlConnection conn, SqlTransaction tran);
    }
}
