using simpliMASTERSAPI.TO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace simpliMASTERSAPI.DAL.Interfaces
{
    public interface ITblVehicleInOutDetailsHistoryDAO
    {

        #region Selection
        List<TblVehicleInOutDetailsHistoryTO> SelectAllTblVehicleInOutDetailsHistory();

        TblVehicleInOutDetailsHistoryTO SelectTblVehicleInOutDetailsHistory(Int32 idTblVehicleInOutDetailsHistory);
        List<TblVehicleInOutDetailsHistoryTO> SelectAllTblVehicleInOutDetailsHistory(SqlConnection conn, SqlTransaction tran);
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
