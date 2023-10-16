using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace simpliMASTERSAPI.DAL.Interfaces
{
    public interface ITblPaymentAllocationDAO
    {
        DataTable SelectAllTblPaymentAllocation();
        DataTable SelectTblPaymentAllocation(Int32 idPaymentAllocation);
        DataTable SelectAllTblPaymentAllocation(SqlConnection conn, SqlTransaction tran);
        int InsertTblPaymentAllocation(TblPaymentAllocationTO tblPaymentAllocationTO);
        int InsertTblPaymentAllocation(TblPaymentAllocationTO tblPaymentAllocationTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPaymentAllocation(TblPaymentAllocationTO tblPaymentAllocationTO);
        int UpdateTblPaymentAllocation(TblPaymentAllocationTO tblPaymentAllocationTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPaymentAllocation(Int32 idPaymentAllocation);
        int DeleteTblPaymentAllocation(Int32 idPaymentAllocation, SqlConnection conn, SqlTransaction tran);
    }
}
