using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
   public interface ITblItemMasterFieldsBL
    {

        #region Selection
        List<TblItemMasterFieldsTO> SelectAllTblItemMasterFields();

        List<TblItemMasterFieldsTO> SelectAllTblItemMasterFieldsList(SqlConnection conn, SqlTransaction tran);

        TblItemMasterFieldsTO SelectTblItemMasterFieldsTO(Int32 idTblItemMasterFields);

        #endregion

        #region Insertion
         int InsertTblItemMasterFields(TblItemMasterFieldsTO tblItemMasterFieldsTO);
         int InsertTblItemMasterFields(TblItemMasterFieldsTO tblItemMasterFieldsTO, SqlConnection conn, SqlTransaction tran);

        #endregion

        #region Updation
         int UpdateTblItemMasterFields(TblItemMasterFieldsTO tblItemMasterFieldsTO);
        int UpdateTblItemMasterFields(TblItemMasterFieldsTO tblItemMasterFieldsTO, SqlConnection conn, SqlTransaction tran);

        #endregion

        #region Deletion
         int DeleteTblItemMasterFields(Int32 idTblItemMasterFields);

         int DeleteTblItemMasterFields(Int32 idTblItemMasterFields, SqlConnection conn, SqlTransaction tran);

        #endregion
    }
}
