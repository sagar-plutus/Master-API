using simpliMASTERSAPI.StaticStuff;
using simpliMASTERSAPI.Models;
using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simpliMASTERSAPI.BL.Interfaces
{
    public interface ITblPmUserBL
    {
        List<TblPmUserTO> SelectAllTblPmUser();
        List<TblPmUserTO> SelectAllTblPmUserList();
        List<TblPmUserTO> SelectAllPMForUser(Int32 loginUserId);
        TblPmUserTO SelectTblPmUserTO(Int32 idPmUser);

        int InsertTblPmUser(TblPmUserTO tblPmUserTO);
        int InsertTblPmUser(TblPmUserTO tblPmUserTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblPmUser(TblPmUserTO tblPmUserTO);
        int UpdateTblPmUser(TblPmUserTO tblPmUserTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblPmUser(Int32 idPmUser);
        int DeleteTblPmUser(Int32 idPmUser, SqlConnection conn, SqlTransaction tran);
    }
}
