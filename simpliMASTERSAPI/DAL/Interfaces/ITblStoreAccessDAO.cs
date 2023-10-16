using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblStoreAccessDAO
    {
        String SqlSelectQuery();
        List<TblStoreAccessTO> SelectAllTblStoreAccess(Int32 userId);
        TblStoreAccessTO SelectTblStoreAccess(Int32 idStoreAccess);
        TblStoreAccessTO SelectAllTblStoreAccess(SqlConnection conn, SqlTransaction tran);
        int InsertTblStoreAccess(TblStoreAccessTO tblStoreAccessTO);
        int InsertTblStoreAccess(TblStoreAccessTO tblStoreAccessTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblStoreAccess(TblStoreAccessTO tblStoreAccessTO);
        int UpdateTblStoreAccess(TblStoreAccessTO tblStoreAccessTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblStoreAccess(Int32 idStoreAccess);
        int DeleteTblStoreAccess(Int32 idStoreAccess, SqlConnection conn, SqlTransaction tran);
        List<TblStoreAccessTO> SelectAllTblStoreAccess(Int32 userId, Int32 deptId);


    }
}
