using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblSysElementsDAO
    {
        String SqlSelectQuery();
        List<TblSysElementsTO> SelectAllTblSysElements(int menuPageId, string type, int moduleId);
        TblSysElementsTO SelectTblSysElements(Int32 idSysElement);
        List<TblSysElementsTO> SelectTblSysElementsByModulId(Int32 moduleId);
        
        List<TblSysElementsTO> ConvertDTToList(SqlDataReader tblSysElementsTODT);
        int InsertTblSysElements(TblSysElementsTO tblSysElementsTO);
        int InsertTblSysElements(TblSysElementsTO tblSysElementsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblSysElementsTO tblSysElementsTO, SqlCommand cmdInsert);
        int UpdateTblSysElements(TblSysElementsTO tblSysElementsTO);
        int UpdateTblSysElements(TblSysElementsTO tblSysElementsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblSysElementsTO tblSysElementsTO, SqlCommand cmdUpdate);
        int DeleteTblSysElements(Int32 idSysElement);
        int DeleteTblSysElements(Int32 idSysElement, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idSysElement, SqlCommand cmdDelete);
        int SelectIsImportantPerson(int userId,int sysEleID);
        List<Tuple<int, int>> SelectAllIsImportantPerson(string userId, int sysEleID);

         //Harshala
           List<TblSysElementsTO> SelectgiveAllTblSysElements();
           List<tblViewPermissionTO> selectPermissionswrtRole(int roleId,int userId);
            List<tblViewMenuTO> SelectMenuPermission(int roleId,int userId, int moduleId);
            List<tblViewMenuTO> SelectElementPermission(int roleId,int userId, int moduleId);
            List<DropDownTO> SelectAllPermissionList();
            List<tblRoleUserTO> SelectAllRolewrtPermission(int idSysElement);
             List<tblRoleUserTO> SelectAllUserwrtPermission(int idSysElement);


    }
}