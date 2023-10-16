using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TO;

namespace ODLMWebAPI.DAL.Interfaces
{
  public interface ITblUserAllocationDAO
    {
        List<TblUserAllocationTO> GetUserAllocationList(Int32? userId, Int32? allocTypeId,Int32? refId);


        int UpdateTblUserAllocation(TblUserAllocationTO dimAllocationTypeTO);
        int UpdateTblUserAllocation(TblUserAllocationTO dimAllocationTypeTO, SqlConnection conn, SqlTransaction tran);
        int InsertTblUserAllocation(TblUserAllocationTO dimAllocationTypeTO);
        int InsertTblUserAllocation(TblUserAllocationTO dimAllocationTypeTO, SqlConnection conn, SqlTransaction tran);



    }
}
