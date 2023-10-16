using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblReportsBL
    {
        List<dynamic> SelectTallyStockTransferReportList(DateTime frmDt, DateTime toDt,String roleTypeCond);
        List<TblReportsTO> SelectAllModuleWiseReport(Int32 moduleId, Int32 transId,Int32 loginUserId);
        List<TblReportsTO> SelectAllTblReportsList();
        TblReportsTO SelectTblReportsTO(Int32 idReports);
        int InsertTblReports(TblReportsTO tblReportsTO);
        int InsertTblReports(TblReportsTO tblReportsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblReports(TblReportsTO tblReportsTO);
        int UpdateTblReports(TblReportsTO tblReportsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblReports(Int32 idReports);
        int DeleteTblReports(Int32 idReports, SqlConnection conn, SqlTransaction tran);
        IEnumerable<dynamic> GetDynamicData(string cmdText, params SqlParameter[] commandParameters);
        IEnumerable<dynamic> CreateDynamicQuery(TblReportsTO tblReportsTO);
        List<TblWBRptTO> SelectWBForPurchaseSaleUnloadReportList(DateTime frmDt, DateTime toDt);


    }
}