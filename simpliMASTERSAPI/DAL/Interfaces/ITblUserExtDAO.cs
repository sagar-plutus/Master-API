using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblUserExtDAO
    {
        String SqlSelectQuery();
        List<TblUserExtTO> SelectAllTblUserExt();
        TblUserExtTO SelectTblUserExt(Int32 userId);
        List<TblUserExtTO> ConvertDTToList(SqlDataReader tblUserExtTODT);
        int InsertTblUserExt(TblUserExtTO tblUserExtTO);
        int InsertTblUserExt(TblUserExtTO tblUserExtTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblUserExtTO tblUserExtTO, SqlCommand cmdInsert);
        int UpdateTblUserExt(TblUserExtTO tblUserExtTO);
        int UpdateTblUserExt(TblUserExtTO tblUserExtTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblUserExtTO tblUserExtTO, SqlCommand cmdUpdate);
        int DeleteTblUserExt();
        int DeleteTblUserExt(SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(SqlCommand cmdDelete);
        //Priyanka [04-09-2019]
        TblUserExtTO SelectTblUserExtByOrganizationId(Int32 organizationId);
        List<TblUserExtTO> GetAllUserSettingsList();
        int UpdateTblUserExtSettings(TblUserExtTO tblUserExtTO, SqlConnection conn, SqlTransaction tran);
        List<TblUserExtTO> SelectTblUserExtByPersonId(Int32 personId);
    }
}