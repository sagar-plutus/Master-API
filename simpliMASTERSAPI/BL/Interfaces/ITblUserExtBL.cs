using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblUserExtBL
    {
        List<TblUserExtTO> SelectAllTblUserExtList();
        TblUserExtTO SelectTblUserExtTO(Int32 userId);
        int InsertTblUserExt(TblUserExtTO tblUserExtTO);
        int InsertTblUserExt(TblUserExtTO tblUserExtTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblUserExt(TblUserExtTO tblUserExtTO);
        int UpdateTblUserExt(TblUserExtTO tblUserExtTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblUserExt();
        int DeleteTblUserExt(SqlConnection conn, SqlTransaction tran);
        //Priyanka [04-09-2019]
        TblUserExtTO SelectTblUserExtByOrganizationId(Int32 organizationId);
        List<TblUserExtTO> GetAllUserSettingsList();
        ResultMessage UpdateUserSettings(List<TblUserExtTO> tblUserExtTOList);

    }
}