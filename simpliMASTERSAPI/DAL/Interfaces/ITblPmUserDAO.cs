using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using simpliMASTERSAPI.Models;

namespace simpliMASTERSAPI.DAL.Interfaces
{
    public interface ITblPmUserDAO
    {
        String SqlSelectQuery();
        List<TblPmUserTO> SelectAllTblPmUser();
        List<TblPmUserTO> SelectTblPmUser(Int32 idPmUser);

        List<TblPmUserTO> SelectAllPMForUser(Int32 loginUserId);

        List<TblPmUserTO> SelectAllTblPmUser(SqlConnection conn, SqlTransaction tran);
        List<TblPmUserTO> ConvertDTToList(SqlDataReader tblPmUserTODT);
        int InsertTblPmUser(TblPmUserTO tblPmUserTO);
        int InsertTblPmUser(TblPmUserTO tblPmUserTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblPmUserTO tblPmUserTO, SqlCommand cmdInsert);
        int UpdateTblPmUser(TblPmUserTO tblPmUserTO);
        int UpdateTblPmUser(TblPmUserTO tblPmUserTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblPmUserTO tblPmUserTO, SqlCommand cmdUpdate);
        int DeleteTblPmUser(Int32 idPmUser);
        int DeleteTblPmUser(Int32 idPmUser, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idPmUser, SqlCommand cmdDelete);

    }
}
