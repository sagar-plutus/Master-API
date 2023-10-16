﻿using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblItemMasterFieldsDAO
    {

        #region Selection
        List<TblItemMasterFieldsTO> SelectAllTblItemMasterFields();

        TblItemMasterFieldsTO SelectTblItemMasterFields(Int32 idTblItemMasterFields);

        List<TblItemMasterFieldsTO> SelectAllTblItemMasterFields(SqlConnection conn, SqlTransaction tran);

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

        int ExecuteDeletionCommand(Int32 idTblItemMasterFields, SqlCommand cmdDelete);
        #endregion
    }
}
