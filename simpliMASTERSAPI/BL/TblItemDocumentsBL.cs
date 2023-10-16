using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.TO;
using simpliMASTERSAPI.DAL.Interfaces;
using simpliMASTERSAPI.BL.Interfaces;
using ODLMWebAPI.Models;

namespace ODLMWebAPI.BL
{
    public class TblItemDocumentsBL : ITblItemDocumentsBL
    {
        private readonly ITblItemDocumentsDAO _ITblItemDocumentsDAO;
        public TblItemDocumentsBL(ITblItemDocumentsDAO iTblItemDocumentsDAO)
        {
            _ITblItemDocumentsDAO = iTblItemDocumentsDAO;
        }

        public List<DropDownTO> GetDocumentTypeList()
        {
            return _ITblItemDocumentsDAO.GetDocumentTypeList();
        }

        #region Selection
        public List<TblItemDocumentsTO> SelectAllTblItemDocuments()
        {
            return _ITblItemDocumentsDAO.SelectAllTblItemDocuments();
        }

        public List<TblItemDocumentsTO> SelectAllTblItemDocumentsList()
        {
            return _ITblItemDocumentsDAO.SelectAllTblItemDocuments();

        }

        public TblItemDocumentsTO SelectTblItemDocumentsTO(int idItemDocuments)
        {
            return _ITblItemDocumentsDAO.SelectTblItemDocuments(idItemDocuments);

        }

        public List<TblItemDocumentsTO> SelectTblItemDocumentsByItemId(int prodItemId, Boolean isShowImagesOnly = false)
        {
            return _ITblItemDocumentsDAO.SelectTblItemDocumentsByItemId(prodItemId,isShowImagesOnly);

        }


        #endregion

        #region Insertion
        public int InsertTblItemDocuments(TblItemDocumentsTO tblItemDocumentsTO)
        {
            return _ITblItemDocumentsDAO.InsertTblItemDocuments(tblItemDocumentsTO);
        }

       public int InsertTblItemDocuments(List<TblItemDocumentsTO> tblItemDocumentsTOList)
        {
            int res = 0;
            if (tblItemDocumentsTOList != null && tblItemDocumentsTOList.Count > 0)
            {
                for (int i = 0; i < tblItemDocumentsTOList.Count; i++) {
                    res = InsertTblItemDocuments(tblItemDocumentsTOList[i]);
                }
            }
            return res;
        }

        public int InsertTblItemDocuments(TblItemDocumentsTO tblItemDocumentsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _ITblItemDocumentsDAO.InsertTblItemDocuments(tblItemDocumentsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateTblItemDocuments(TblItemDocumentsTO tblItemDocumentsTO)
        {
            return _ITblItemDocumentsDAO.UpdateTblItemDocuments(tblItemDocumentsTO);
        }

        public  int UpdateTblItemDocuments(TblItemDocumentsTO tblItemDocumentsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _ITblItemDocumentsDAO.UpdateTblItemDocuments(tblItemDocumentsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblItemDocuments(int idItemDocuments)
        {
            return _ITblItemDocumentsDAO.DeleteTblItemDocuments(idItemDocuments);
        }

        public  int DeleteTblItemDocuments(int idItemDocuments, SqlConnection conn, SqlTransaction tran)
        {
            return _ITblItemDocumentsDAO.DeleteTblItemDocuments(idItemDocuments, conn, tran);
        }

        #endregion
        
    }
}
