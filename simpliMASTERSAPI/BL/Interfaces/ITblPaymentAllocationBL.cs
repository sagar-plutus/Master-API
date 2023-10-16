using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace simpliMASTERSAPI.BL.Interfaces
{
    public interface ITblPaymentAllocationBL
    {
        DataTable SelectAllTblPaymentAllocation();
        List<TblPaymentAllocationTO> SelectAllTblPaymentAllocationList();
        TblPaymentAllocationTO SelectTblPaymentAllocationTO(Int32 idPaymentAllocation);
        List<TblPaymentAllocationTO> ConvertDTToList(DataTable tblPaymentAllocationTODT);
        int InsertTblPaymentAllocation(TblPaymentAllocationTO tblPaymentAllocationTO);
        int InsertTblPaymentAllocation(TblPaymentAllocationTO tblPaymentAllocationTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPaymentAllocation(TblPaymentAllocationTO tblPaymentAllocationTO);
        int UpdateTblPaymentAllocation(TblPaymentAllocationTO tblPaymentAllocationTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPaymentAllocation(Int32 idPaymentAllocation);
        int DeleteTblPaymentAllocation(Int32 idPaymentAllocation, SqlConnection conn, SqlTransaction tran);
    }
}
