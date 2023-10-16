using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TO;
namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblUserAllocationBL
    {
         List<TblUserAllocationTO> GetUserAllocationList(Int32? userId, Int32? allocTypeId ,Int32? refId);
             ResultMessage UpdateTblUserAllocation(TblUserAllocationTO tblAllocationTO);
        ResultMessage SaveTblUserAllocation(TblUserAllocationTO tblAllocationTO);
        int SaveTblUserAllocation(TblUserAllocationTO tblAllocationTO, SqlConnection conn, SqlTransaction tran);

    }
}
