using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblStoreAccessBL
    {
        List<TblStoreAccessTO> SelectAllTblStoreAccess(Int32 userId);
        ResultMessage InsertTblStoreAccess(TblStoreAccessTO tblStoreAccessTO);
        int InsertTblStoreAccess(TblStoreAccessTO tblStoreAccessTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage UpdateTblStoreAccess(TblStoreAccessTO tblStoreAccessTO);
        int UpdateTblStoreAccess(TblStoreAccessTO tblStoreAccessTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblStoreAccess(Int32 idStoreAccess);
        int DeleteTblStoreAccess(Int32 idStoreAccess, SqlConnection conn, SqlTransaction tran);
        List<DropDownTO> GetStoresLocationDropDownList(Int32 userId);

    }
}
