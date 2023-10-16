using ODLMWebAPI.TO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace simpliMASTERSAPI.DAL.Interfaces
{
    public interface IDimReceiptTypeDAO
    {
        DataTable SelectAllDimReceiptType();
        DataTable SelectDimReceiptType(Int32 idReceiptType);
        DataTable SelectAllDimReceiptType(SqlConnection conn, SqlTransaction tran);
        int InsertDimReceiptType(DimReceiptTypeTO dimReceiptTypeTO);
        int InsertDimReceiptType(DimReceiptTypeTO dimReceiptTypeTO, SqlConnection conn, SqlTransaction tran);
        int UpdateDimReceiptType(DimReceiptTypeTO dimReceiptTypeTO);
        int UpdateDimReceiptType(DimReceiptTypeTO dimReceiptTypeTO, SqlConnection conn, SqlTransaction tran);
        int DeleteDimReceiptType(Int32 idReceiptType);
        int DeleteDimReceiptType(Int32 idReceiptType, SqlConnection conn, SqlTransaction tran);
    }
}
