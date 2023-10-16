using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL.Interfaces;
using simpliMASTERSAPI.BL.Interfaces;

namespace ODLMWebAPI.BL
{
    public class TblProdItemMakeBrandExtBL : ITblProdItemMakeBrandExtBL
    {
        private readonly ITblProdItemMakeBrandExtDAO _iTblProdItemMakeBrandExtDAO;
        private readonly IConnectionString _iConnectionString;
        private readonly ITblConfigParamsDAO _iTblConfigParamsDAO;
        private readonly IDimensionDAO _iDimensionDAO;
        private readonly ICommon _iCommon;
        public TblProdItemMakeBrandExtBL(ICommon iCommon, ITblConfigParamsDAO iTblConfigParamsDAO, IConnectionString iConnectionString,
         ITblProdItemMakeBrandExtDAO iTblProdItemMakeBrandExtDAO)
        {
            _iTblProdItemMakeBrandExtDAO = iTblProdItemMakeBrandExtDAO;
            _iConnectionString = iConnectionString;
            _iTblConfigParamsDAO = iTblConfigParamsDAO;
            _iCommon = iCommon;
        }
        #region Selection

        public List<TblProdItemMakeBrandExtTO> SelectAllTblProdItemMakeBrandExt()
        {
            return _iTblProdItemMakeBrandExtDAO.SelectAllTblProdItemMakeBrandExt();
        }

        public List<TblProdItemMakeBrandExtTO> SelectAllTblProdItemMakeBrandExtByProdItem(Int32 prodItemId)
        {
            return _iTblProdItemMakeBrandExtDAO.SelectAllTblProdItemMakeBrandExtByProdItem(prodItemId);
        }

        public List<TblProdItemMakeBrandExtTO> SelectAllTblProdItemMakeBrandExt(SqlConnection conn,SqlTransaction tran)
        {
            return _iTblProdItemMakeBrandExtDAO.SelectAllTblProdItemMakeBrandExt(conn,tran);
        }

        public  TblProdItemMakeBrandExtTO SelectTblProdItemMakeBrandExtTO(int idProdItemMakeBrandExt)
        {
          return _iTblProdItemMakeBrandExtDAO.SelectTblProdItemMakeBrandExt(idProdItemMakeBrandExt);
            
        }

     
        #endregion
        
        #region Insertion
        public  int InsertTblProdItemMakeBrandExt(TblProdItemMakeBrandExtTO tblProdItemMakeBrandExtTO)
        {
            return _iTblProdItemMakeBrandExtDAO.InsertTblProdItemMakeBrandExt(tblProdItemMakeBrandExtTO);
        }

        public  int InsertTblProdItemMakeBrandExt(TblProdItemMakeBrandExtTO tblProdItemMakeBrandExtTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblProdItemMakeBrandExtDAO.InsertTblProdItemMakeBrandExt(tblProdItemMakeBrandExtTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblProdItemMakeBrandExt(TblProdItemMakeBrandExtTO tblProdItemMakeBrandExtTO)
        {
            return _iTblProdItemMakeBrandExtDAO.UpdateTblProdItemMakeBrandExt(tblProdItemMakeBrandExtTO);
        }

        public  int UpdateTblProdItemMakeBrandExt(TblProdItemMakeBrandExtTO tblProdItemMakeBrandExtTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblProdItemMakeBrandExtDAO.UpdateTblProdItemMakeBrandExt(tblProdItemMakeBrandExtTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblProdItemMakeBrandExt(int idProdItemMakeBrandExt)
        {
            return _iTblProdItemMakeBrandExtDAO.DeleteTblProdItemMakeBrandExt(idProdItemMakeBrandExt);
        }

        public  int DeleteTblProdItemMakeBrandExt(int idProdItemMakeBrandExt, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblProdItemMakeBrandExtDAO.DeleteTblProdItemMakeBrandExt(idProdItemMakeBrandExt, conn, tran);
        }

        #endregion
        
    }
}
