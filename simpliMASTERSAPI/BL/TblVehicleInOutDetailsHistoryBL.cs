using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using simpliMASTERSAPI.TO;
using simpliMASTERSAPI.BL.Interfaces;
using simpliMASTERSAPI.DAL.Interfaces;

namespace simpliMASTERSAPI.BL
{
    public class TblVehicleInOutDetailsHistoryBL : ITblVehicleInOutDetailsHistoryBL
    {
        private readonly ITblVehicleInOutDetailsHistoryDAO _iTblVehicleInOutDetailsHistoryDAO;
        public TblVehicleInOutDetailsHistoryBL(ITblVehicleInOutDetailsHistoryDAO iTblVehicleInOutDetailsHistoryDAO)
        {
            _iTblVehicleInOutDetailsHistoryDAO = iTblVehicleInOutDetailsHistoryDAO;
        }
        #region Selection
        public  List<TblVehicleInOutDetailsHistoryTO> SelectAllTblVehicleInOutDetailsHistory()
        {
            return _iTblVehicleInOutDetailsHistoryDAO.SelectAllTblVehicleInOutDetailsHistory();
        }

        public  List<TblVehicleInOutDetailsHistoryTO> SelectAllTblVehicleInOutDetailsHistoryList()
        {
            return _iTblVehicleInOutDetailsHistoryDAO.SelectAllTblVehicleInOutDetailsHistory();
           
        }

        public  TblVehicleInOutDetailsHistoryTO SelectTblVehicleInOutDetailsHistoryTO(Int32 idTblVehicleInOutDetailsHistory)
        {
            return _iTblVehicleInOutDetailsHistoryDAO.SelectTblVehicleInOutDetailsHistory(idTblVehicleInOutDetailsHistory);
           
        }

       #endregion
        
        #region Insertion
        public  int InsertTblVehicleInOutDetailsHistory(TblVehicleInOutDetailsHistoryTO tblVehicleInOutDetailsHistoryTO)
        {
            return _iTblVehicleInOutDetailsHistoryDAO.InsertTblVehicleInOutDetailsHistory(tblVehicleInOutDetailsHistoryTO);
        }

        public  int InsertTblVehicleInOutDetailsHistory(TblVehicleInOutDetailsHistoryTO tblVehicleInOutDetailsHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVehicleInOutDetailsHistoryDAO.InsertTblVehicleInOutDetailsHistory(tblVehicleInOutDetailsHistoryTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblVehicleInOutDetailsHistory(TblVehicleInOutDetailsHistoryTO tblVehicleInOutDetailsHistoryTO)
        {
            return _iTblVehicleInOutDetailsHistoryDAO.UpdateTblVehicleInOutDetailsHistory(tblVehicleInOutDetailsHistoryTO);
        }

        public  int UpdateTblVehicleInOutDetailsHistory(TblVehicleInOutDetailsHistoryTO tblVehicleInOutDetailsHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVehicleInOutDetailsHistoryDAO.UpdateTblVehicleInOutDetailsHistory(tblVehicleInOutDetailsHistoryTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblVehicleInOutDetailsHistory(Int32 idTblVehicleInOutDetailsHistory)
        {
            return _iTblVehicleInOutDetailsHistoryDAO.DeleteTblVehicleInOutDetailsHistory(idTblVehicleInOutDetailsHistory);
        }

        public  int DeleteTblVehicleInOutDetailsHistory(Int32 idTblVehicleInOutDetailsHistory, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVehicleInOutDetailsHistoryDAO.DeleteTblVehicleInOutDetailsHistory(idTblVehicleInOutDetailsHistory, conn, tran);
        }

        #endregion
        
    }
}
