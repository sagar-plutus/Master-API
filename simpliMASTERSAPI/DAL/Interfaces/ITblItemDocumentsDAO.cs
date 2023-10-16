using ODLMWebAPI.TO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ODLMWebAPI.Models;

namespace simpliMASTERSAPI.DAL.Interfaces
{
    public interface ITblItemDocumentsDAO
    {
        #region Selection
        List<TblItemDocumentsTO> SelectAllTblItemDocuments();

        TblItemDocumentsTO SelectTblItemDocuments(int idItemDocuments);

        List<TblItemDocumentsTO> SelectTblItemDocumentsByItemId(int prodItemId, Boolean isShowImagesOnly);

        List<TblItemDocumentsTO> SelectAllTblItemDocuments(SqlConnection conn, SqlTransaction tran);

        #endregion

        #region Insertion
         int InsertTblItemDocuments(TblItemDocumentsTO tblItemDocumentsTO);

        int InsertTblItemDocuments(TblItemDocumentsTO tblItemDocumentsTO, SqlConnection conn, SqlTransaction tran);
        List<DropDownTO> GetDocumentTypeList();

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
