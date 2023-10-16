using ODLMWebAPI.Models;
using ODLMWebAPI.TO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace simpliMASTERSAPI.BL.Interfaces
{
  public  interface ITblItemDocumentsBL
    {
        #region Selection

        List<DropDownTO> GetDocumentTypeList();
        List<TblItemDocumentsTO> SelectAllTblItemDocuments();

        List<TblItemDocumentsTO> SelectAllTblItemDocumentsList();
        TblItemDocumentsTO SelectTblItemDocumentsTO(int idItemDocuments);

        List<TblItemDocumentsTO> SelectTblItemDocumentsByItemId(int prodItemId, Boolean isShowImagesOnly);

        #endregion

        #region Insertion
        int InsertTblItemDocuments(TblItemDocumentsTO tblItemDocumentsTO);
        int InsertTblItemDocuments(List<TblItemDocumentsTO> tblItemDocumentsTOList);
        int InsertTblItemDocuments(TblItemDocumentsTO tblItemDocumentsTO, SqlConnection conn, SqlTransaction tran);

        #endregion

        #region Updation
        int UpdateTblItemDocuments(TblItemDocumentsTO tblItemDocumentsTO);

        int UpdateTblItemDocuments(TblItemDocumentsTO tblItemDocumentsTO, SqlConnection conn, SqlTransaction tran);
        #endregion

        #region Deletion
         int DeleteTblItemDocuments(int idItemDocuments);

         int DeleteTblItemDocuments(int idItemDocuments, SqlConnection conn, SqlTransaction tran);
        #endregion
    }
}
