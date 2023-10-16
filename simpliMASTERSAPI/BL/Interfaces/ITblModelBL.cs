using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace simpliMASTERSAPI.BL.Interfaces
{
   public interface ITblModelBL
    {
        #region Selection
        List<TblModelTO> SelectAllTblModel();

        List<TblModelTO> SelectAllTblModelList();

        TblModelTO SelectTblModelTO(int idModel);

        #endregion

        #region Insertion
        int InsertTblModel(TblModelTO tblModelTO);

        int InsertTblModel(TblModelTO tblModelTO, SqlConnection conn, SqlTransaction tran);

        #endregion

        #region Updation
         int UpdateTblModel(TblModelTO tblModelTO);

        int UpdateTblModel(TblModelTO tblModelTO, SqlConnection conn, SqlTransaction tran);
        #endregion

        #region Deletion
        int DeleteTblModel(int idModel);

        int DeleteTblModel(int idModel, SqlConnection conn, SqlTransaction tran);
        List<TblModelTO> SelectAllTblModelList(int prodItemId, SqlConnection conn, SqlTransaction tran);

        #endregion
    }
}
