using simpliMASTERSAPI.TO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace simpliMASTERSAPI.BL.Interfaces
{
    public interface ITblVehicleInOutDetailsHistoryBL
    {

        #region Selection
        List<TblVehicleInOutDetailsHistoryTO> SelectAllTblVehicleInOutDetailsHistory();
        List<TblVehicleInOutDetailsHistoryTO> SelectAllTblVehicleInOutDetailsHistoryList();

        TblVehicleInOutDetailsHistoryTO SelectTblVehicleInOutDetailsHistoryTO(Int32 idTblVehicleInOutDetailsHistory);
   
        #endregion

        #region Insertion
        int InsertTblVehicleInOutDetailsHistory(TblVehicleInOutDetailsHistoryTO tblVehicleInOutDetailsHistoryTO);

        int InsertTblVehicleInOutDetailsHistory(TblVehicleInOutDetailsHistoryTO tblVehicleInOutDetailsHistoryTO, SqlConnection conn, SqlTransaction tran);
        #endregion

        #region Updation
        int UpdateTblVehicleInOutDetailsHistory(TblVehicleInOutDetailsHistoryTO tblVehicleInOutDetailsHistoryTO);

        int UpdateTblVehicleInOutDetailsHistory(TblVehicleInOutDetailsHistoryTO tblVehicleInOutDetailsHistoryTO, SqlConnection conn, SqlTransaction tran);
        #endregion

        #region Deletion
        int DeleteTblVehicleInOutDetailsHistory(Int32 idTblVehicleInOutDetailsHistory);

         int DeleteTblVehicleInOutDetailsHistory(Int32 idTblVehicleInOutDetailsHistory, SqlConnection conn, SqlTransaction tran);

        #endregion


    }
}
