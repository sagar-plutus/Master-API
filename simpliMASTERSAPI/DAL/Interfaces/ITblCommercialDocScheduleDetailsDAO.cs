using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblCommercialDocScheduleDetailsDAO
    {

        #region Selection
        List<TblCommercialDocScheduleDetailsTO> SelectAllTblCommercialDocScheduleDetails(Int32 commercialDocScheduleId);

        List<TblCommercialDocScheduleDetailsTO> SelectTblCommercialDocScheduleDetailsList(Int64 idCommercialDocScheduleDetails);
        TblCommercialDocScheduleDetailsTO SelectTblCommercialDocScheduleDetails(Int64 idCommercialDocScheduleDetails);

        List<TblCommercialDocScheduleDetailsTO> SelectAllTblCommercialDocScheduleDetails(SqlConnection conn, SqlTransaction tran);
        List<TblCommercialDocScheduleDetailsTO> SelectTblCommercialDocScheduleDetailsList(Int64 idCommercialDocScheduleDetails,SqlConnection conn, SqlTransaction tran);

        #endregion

        #region Insertion
        int InsertTblCommercialDocScheduleDetails(TblCommercialDocScheduleDetailsTO tblCommercialDocScheduleDetailsTO);

        int InsertTblCommercialDocScheduleDetails(TblCommercialDocScheduleDetailsTO tblCommercialDocScheduleDetailsTO, SqlConnection conn, SqlTransaction tran);

       #endregion

        #region Updation
        int UpdateTblCommercialDocScheduleDetails(TblCommercialDocScheduleDetailsTO tblCommercialDocScheduleDetailsTO);
        List<TblCommercialDocScheduleDetailsTO> SelectAllTblCommercialDocSchedule(string postr);
        int UpdateTblCommercialDocScheduleDetails(TblCommercialDocScheduleDetailsTO tblCommercialDocScheduleDetailsTO, SqlConnection conn, SqlTransaction tran);

         #endregion

        #region Deletion
        int DeleteTblCommercialDocScheduleDetails(Int32 idCommercialDocScheduleDetails);
         int DeleteTblCommercialDocScheduleDetails(Int32 idCommercialDocScheduleDetails, SqlConnection conn, SqlTransaction tran);
        double SelectPendingScheduleQtyFromPOnoAgainstItemId(long commecialDocumentId, int productItemId, SqlConnection conn, SqlTransaction tran);
        int UpdatePendingQtyofAllSchedule(long commecialDocumentId, int productItemId, double pendingScheduleQty, SqlConnection conn, SqlTransaction tran);
        #endregion
    }
}
