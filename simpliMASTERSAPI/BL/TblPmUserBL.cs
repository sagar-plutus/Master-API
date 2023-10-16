using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System.Linq;
using ODLMWebAPI.BL.Interfaces;
using simpliMASTERSAPI.Models;
using simpliMASTERSAPI.BL.Interfaces;
using simpliMASTERSAPI.DAL.Interfaces;

namespace simpliMASTERSAPI.BL
{
    public class TblPmUserBL : ITblPmUserBL
    {

        private readonly ITblPmUserDAO _iTblPmUserDAO;
        public TblPmUserBL(ITblPmUserDAO iTblPmUserDAO)
        {
            _iTblPmUserDAO = iTblPmUserDAO;
        }

        #region Selection
        public List<TblPmUserTO> SelectAllTblPmUser()
        {
            return _iTblPmUserDAO.SelectAllTblPmUser();
        }

        public  List<TblPmUserTO> SelectAllTblPmUserList()
        {
             return _iTblPmUserDAO.SelectAllTblPmUser();
        }
        public  List<TblPmUserTO> SelectAllPMForUser(Int32 loginUserId)
        {
             return _iTblPmUserDAO.SelectAllPMForUser(loginUserId);
        }

        public  TblPmUserTO SelectTblPmUserTO(Int32 idPmUser)
        {
            List<TblPmUserTO> tblPmUserTOList = _iTblPmUserDAO.SelectTblPmUser(idPmUser);
            if(tblPmUserTOList != null && tblPmUserTOList.Count == 1)
                return tblPmUserTOList[0];
            else
                return null;
        }

      
        #endregion
        
        #region Insertion
        public  int InsertTblPmUser(TblPmUserTO tblPmUserTO)
        {
            return _iTblPmUserDAO.InsertTblPmUser(tblPmUserTO);
        }

        public  int InsertTblPmUser(TblPmUserTO tblPmUserTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPmUserDAO.InsertTblPmUser(tblPmUserTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblPmUser(TblPmUserTO tblPmUserTO)
        {
            return _iTblPmUserDAO.UpdateTblPmUser(tblPmUserTO);
        }

        public  int UpdateTblPmUser(TblPmUserTO tblPmUserTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPmUserDAO.UpdateTblPmUser(tblPmUserTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblPmUser(Int32 idPmUser)
        {
            return _iTblPmUserDAO.DeleteTblPmUser(idPmUser);
        }

        public  int DeleteTblPmUser(Int32 idPmUser, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblPmUserDAO.DeleteTblPmUser(idPmUser, conn, tran);
        }

        #endregion
        
    }
}
