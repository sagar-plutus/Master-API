using ODLMWebAPI.StaticStuff;
using simpliMASTERSAPI.TO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ODLMWebAPI.Models;

namespace simpliMASTERSAPI.BL.Interfaces
{
   public  interface ITblVehicleInOutDetailsBL
    {
        #region Selection
        List<TblVehicleInOutDetailsTO> SelectAllTblVehicleInOutDetails();
        List<TblVehicleInOutDetailsTO> SelectAllTblVehicleInOutDetailsList(Int32 moduleId,Int32 showVehicleInOut, string fromDate, string toDate, bool skipDatetime,string statusStr);

        TblVehicleInOutDetailsTO SelectTblVehicleInOutDetailsTO(Int32 idTblVehicleInOutDetails);


        #endregion

        #region Insertion
        int InsertTblVehicleInOutDetails(TblVehicleInOutDetailsTO tblVehicleInOutDetailsTO);

        int InsertTblVehicleInOutDetails(TblVehicleInOutDetailsTO tblVehicleInOutDetailsTO, SqlConnection conn, SqlTransaction tran);

        #endregion

        #region Updation
        int UpdateTblVehicleInOutDetails(TblVehicleInOutDetailsTO tblVehicleInOutDetailsTO);
        ResultMessage UpdateTblVehicleInOutDetailsStatusOnly(TblVehicleInOutDetailsTO tblVehicleInOutDetailsTO);

        int UpdateTblVehicleInOutDetails(TblVehicleInOutDetailsTO tblVehicleInOutDetailsTO, SqlConnection conn, SqlTransaction tran);
        #endregion

        #region Deletion
         int DeleteTblVehicleInOutDetails(Int32 idTblVehicleInOutDetails);

         int DeleteTblVehicleInOutDetails(Int32 idTblVehicleInOutDetails, SqlConnection conn, SqlTransaction tran);
        List<VehicleNumber> SelectAllVehicles();
        List<DropDownTO> GetPONoListAgainstSupplier(Int64 supplierId);
        List<DropDownTO> SelectAllSystemUsersFromRoleTypeWithVehAllocation(int roleTypeId, int nameWithCount);
        TblVehicleInOutDetailsTO SelectAllTblVehicleInOutDetailsById(int moduleId, int idVehicleInOut);
        List<DropDownTO> SelectAllSystemUsersFromRoleType(int roleTypeId);
        List<DropDownTO> SelectAllSystemUsersforUnloadingSuperwisorVehicleIn();
        int InsertTblCommericalDocStatusHistory(TblCommericalDocStatusHistoryTO tblCommericalDocStatusHistoryTO);

        #endregion

    }
}
