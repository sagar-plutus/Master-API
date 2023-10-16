using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using System.Linq;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;
using simpliMASTERSAPI;

namespace ODLMWebAPI.BL
{
    public class TblProdItemMakeBrandBL : ITblProdItemMakeBrandBL
    {

        private readonly IConnectionString _iConnectionString;
        private readonly ITblConfigParamsDAO _iTblConfigParamsDAO;
        private readonly IDimensionDAO _iDimensionDAO;
        private readonly ICommon _iCommon;
        private readonly ITblProdItemMakeBrandDAO _iTblProdItemMakeBrandDAO;

        public TblProdItemMakeBrandBL(ICommon iCommon, ITblConfigParamsDAO iTblConfigParamsDAO,IConnectionString iConnectionString,
           ITblProdItemMakeBrandDAO iTblProdItemMakeBrandDAO)           
        {
            _iTblProdItemMakeBrandDAO = iTblProdItemMakeBrandDAO;
            _iConnectionString = iConnectionString;
            _iTblConfigParamsDAO = iTblConfigParamsDAO;           
            _iCommon = iCommon; 
        }

        #region Selection
        public List<TblProdItemMakeBrandTO> SelectAllTblProdItemMakeBrand()
        {
            return _iTblProdItemMakeBrandDAO.SelectAllTblProdItemMakeBrand();
        }

        public List<TblProdItemMakeBrandTO> SelectTblProdItemMakeBrand(Int32 idProdMakeBrand)
        {
            return _iTblProdItemMakeBrandDAO.SelectTblProdItemMakeBrand(idProdMakeBrand);
        }
        public List<TblProdItemMakeBrandTO> SelectAllTblProdItemMakeBrand(SqlConnection conn, SqlTransaction tran)
        {
            return _iTblProdItemMakeBrandDAO.SelectAllTblProdItemMakeBrand(conn, tran);
        }
        public List<TblProdItemMakeBrandTO> SelectProdItemMakeBrandByProdItemBrandId(TblProdItemMakeBrandTO tblProdItemMakeBrandTO)
        {
            return _iTblProdItemMakeBrandDAO.SelectProdItemMakeBrandByProdItemBrandId(tblProdItemMakeBrandTO);
        }

        public List<TblProdItemMakeBrandTO> SelectedProdItemMakeBrand(Int32 prodItemId)
        {
            return _iTblProdItemMakeBrandDAO.SelectedProdItemMakeBrand(prodItemId);
        }
        #endregion

        #region Insertion
        public int InsertTblProdItemMakeBrand(TblProdItemMakeBrandTO tblProdItemMakeBrandTO)
        {
            return _iTblProdItemMakeBrandDAO.InsertTblProdItemMakeBrand(tblProdItemMakeBrandTO);
        }

        public int InsertTblProdItemMakeBrand(TblProdItemMakeBrandTO tblProdItemMakeBrandTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblProdItemMakeBrandDAO.InsertTblProdItemMakeBrand(tblProdItemMakeBrandTO, conn, tran);
        }


        #endregion
        
        #region Updation
        public int UpdateTblProdItemMakeBrand(TblProdItemMakeBrandTO tblProdItemMakeBrandTO)
        {
            return _iTblProdItemMakeBrandDAO.UpdateTblProdItemMakeBrand(tblProdItemMakeBrandTO);
        }

        public int UpdateTblProdItemMakeBrand(TblProdItemMakeBrandTO tblProdItemMakeBrandTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblProdItemMakeBrandDAO.UpdateTblProdItemMakeBrand(tblProdItemMakeBrandTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblProdItemMakeBrand(Int32 idProdMakeBrand)
        {
            return _iTblProdItemMakeBrandDAO.DeleteTblProdItemMakeBrand(idProdMakeBrand);
        }

        public int DeleteTblProdItemMakeBrand(Int32 idProdMakeBrand, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblProdItemMakeBrandDAO.DeleteTblProdItemMakeBrand(idProdMakeBrand,conn,tran);
        }
        #endregion

    }
}
