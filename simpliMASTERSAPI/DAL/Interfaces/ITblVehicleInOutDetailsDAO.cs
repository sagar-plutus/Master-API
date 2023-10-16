using simpliMASTERSAPI.TO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ODLMWebAPI.Models;

namespace simpliMASTERSAPI.DAL.Interfaces
{
    public interface ITblVehicleInOutDetailsDAO
    {
        #region Selection
        List<TblVehicleInOutDetailsTO> SelectAllTblVehicleInOutDetails();
        List<TblVehicleInOutDetailsTO> SelectAllTblVehicleInOutDetails(Int32 moduleId,Int32 showVehicleInOut, string fromDate, string toDate, bool skipDatetime,string statusStr);

        TblVehicleInOutDetailsTO SelectTblVehicleInOutDetails(Int32 idTblVehicleInOutDetails);

        List<TblVehicleInOutDetailsTO> SelectAllTblVehicleInOutDetails(SqlConnection conn, SqlTransaction tran);
        #endregion
             
        #region Insertion
        int InsertTblVehicleInOutDetails(TblVehicleInOutDetailsTO tblVehicleInOutDetailsTO);

        int InsertTblVehicleInOutDetails(TblVehicleInOutDetailsTO tblVehicleInOutDetailsTO, SqlConnection conn, SqlTransaction tran);

         #endregion

        #region Updation
        int UpdateTblVehicleInOutDetails(TblVehicleInOutDetailsTO tblVehicleInOutDetailsTO);

        int UpdateTblVehicleInOutDetails(TblVehicleInOutDetailsTO tblVehicleInOutDetailsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblVehicleInOutDetailsStatusOnly(TblVehicleInOutDetailsTO tblVehicleInOutDetailsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblScheduleStatusOnly(TblVehicleInOutDetailsTO tblVehicleInOutDetailsTO, SqlConnection conn, SqlTransaction tran);
        
        #endregion

        #region Deletion
        int DeleteTblVehicleInOutDetails(Int32 idTblVehicleInOutDetails);

        int DeleteTblVehicleInOutDetails(Int32 idTblVehicleInOutDetails, SqlConnection conn, SqlTransaction tran);
        List<VehicleNumber> SelectAllVehicles();
        List<DropDownTO> GetPONoListAgainstSupplier(Int64 supplierId);
        List<DropDownTO> SelectAllSystemUsersFromRoleType(int roleTypeId);
        List<DropDownTO> SelectAllSystemUsersforUnloadingSuperwisorVehicleIn();
        TblVehicleInOutDetailsTO SelectAllTblVehicleInOutDetailsById(int moduleId, int idVehicleInOut);
        int InsertTblCommericalDocStatusHistory(TblCommericalDocStatusHistoryTO tblCommericalDocStatusHistoryTO);


        #endregion

    }
}
