using ODLMWebAPI.TO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace simpliMASTERSAPI.BL.Interfaces
{
    public interface IDimReceiptTypeBL
    {
        DataTable SelectAllDimReceiptType();
        List<DimReceiptTypeTO> SelectAllDimReceiptTypeList(int userId);
        DimReceiptTypeTO SelectDimReceiptTypeTO(Int32 idReceiptType);
        int InsertDimReceiptType(DimReceiptTypeTO dimReceiptTypeTO);
        int InsertDimReceiptType(DimReceiptTypeTO dimReceiptTypeTO, SqlConnection conn, SqlTransaction tran);
        int UpdateDimReceiptType(DimReceiptTypeTO dimReceiptTypeTO);
        int UpdateDimReceiptType(DimReceiptTypeTO dimReceiptTypeTO, SqlConnection conn, SqlTransaction tran);
        int DeleteDimReceiptType(Int32 idReceiptType);
        int DeleteDimReceiptType(Int32 idReceiptType, SqlConnection conn, SqlTransaction tran);

    }
}
