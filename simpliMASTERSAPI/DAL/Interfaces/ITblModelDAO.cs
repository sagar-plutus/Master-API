using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace simpliMASTERSAPI.DAL.Interfaces
{
    public interface ITblModelDAO
    {

        #region Selection
        List<TblModelTO> SelectAllTblModel();

        TblModelTO SelectTblModel(int idModel);

        List<TblModelTO> SelectAllTblModel(SqlConnection conn, SqlTransaction tran);
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
        List<TblModelTO> SelectAllTblModel(int prodItemId, SqlConnection conn, SqlTransaction tran);


        #endregion

    }
}
