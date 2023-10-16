using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.BL.Interfaces;
using simpliMASTERSAPI.DAL.Interfaces;
using ODLMWebAPI.Models;
using simpliMASTERSAPI.BL.Interfaces;

namespace ODLMWebAPI.BL
{
    public class TblModelBL : ITblModelBL
    {
        private readonly IConnectionString _iConnectionString;
        private readonly ITblModelDAO _iTblModelDAO;

        public TblModelBL(IConnectionString iConnectionString, ITblModelDAO iTblModelDAO)
        {         
            _iConnectionString = iConnectionString;
            _iTblModelDAO = iTblModelDAO;
        }
        #region Selection
        public List<TblModelTO> SelectAllTblModel()
        {
            return _iTblModelDAO.SelectAllTblModel();
        }

        public List<TblModelTO> SelectAllTblModelList()
        {
           return _iTblModelDAO.SelectAllTblModel();
        }
        public List<TblModelTO> SelectAllTblModelList(int prodItemId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblModelDAO.SelectAllTblModel(prodItemId,conn,tran);
        }

        public TblModelTO SelectTblModelTO(int idModel)
        {return _iTblModelDAO.SelectTblModel(idModel);
        }


        #endregion
        
        #region Insertion
        public  int InsertTblModel(TblModelTO tblModelTO)
        {
            return _iTblModelDAO.InsertTblModel(tblModelTO);
        }

        public  int InsertTblModel(TblModelTO tblModelTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblModelDAO.InsertTblModel(tblModelTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblModel(TblModelTO tblModelTO)
        {
            return _iTblModelDAO.UpdateTblModel(tblModelTO);
        }

        public  int UpdateTblModel(TblModelTO tblModelTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblModelDAO.UpdateTblModel(tblModelTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblModel(int idModel)
        {
            return _iTblModelDAO.DeleteTblModel(idModel);
        }

        public  int DeleteTblModel(int idModel, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblModelDAO.DeleteTblModel(idModel, conn, tran);
        }

        #endregion
        
    }
}
