using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.Models;
using ODLMWebAPI.BL.Interfaces;

namespace ODLMWebAPI.BL
{
    public class TblItemMasterFieldsBL : ITblItemMasterFieldsBL
    {
        private readonly ITblItemMasterFieldsDAO _iTblItemMasterFieldsDAO;
        public TblItemMasterFieldsBL(ITblItemMasterFieldsDAO iTblItemMasterFieldsDAO) {
            _iTblItemMasterFieldsDAO = iTblItemMasterFieldsDAO;
        }
        #region Selection
        public List<TblItemMasterFieldsTO> SelectAllTblItemMasterFields()
        {
            return _iTblItemMasterFieldsDAO.SelectAllTblItemMasterFields();
        }

        public List<TblItemMasterFieldsTO> SelectAllTblItemMasterFieldsList(SqlConnection conn, SqlTransaction tran)
        {
            return _iTblItemMasterFieldsDAO.SelectAllTblItemMasterFields(conn,tran);
           
        }

        public  TblItemMasterFieldsTO SelectTblItemMasterFieldsTO(Int32 idTblItemMasterFields)
        {
            return _iTblItemMasterFieldsDAO.SelectTblItemMasterFields(idTblItemMasterFields);
           
        }

         #endregion
        
        #region Insertion
        public  int InsertTblItemMasterFields(TblItemMasterFieldsTO tblItemMasterFieldsTO)
        {
            return _iTblItemMasterFieldsDAO.InsertTblItemMasterFields(tblItemMasterFieldsTO);
        }

        public  int InsertTblItemMasterFields(TblItemMasterFieldsTO tblItemMasterFieldsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblItemMasterFieldsDAO.InsertTblItemMasterFields(tblItemMasterFieldsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblItemMasterFields(TblItemMasterFieldsTO tblItemMasterFieldsTO)
        {
            return _iTblItemMasterFieldsDAO.UpdateTblItemMasterFields(tblItemMasterFieldsTO);
        }

        public  int UpdateTblItemMasterFields(TblItemMasterFieldsTO tblItemMasterFieldsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblItemMasterFieldsDAO.UpdateTblItemMasterFields(tblItemMasterFieldsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblItemMasterFields(Int32 idTblItemMasterFields)
        {
            return _iTblItemMasterFieldsDAO.DeleteTblItemMasterFields(idTblItemMasterFields);
        }

        public  int DeleteTblItemMasterFields(Int32 idTblItemMasterFields, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblItemMasterFieldsDAO.DeleteTblItemMasterFields(idTblItemMasterFields, conn, tran);
        }

        #endregion
        
    }
}
