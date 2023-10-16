using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.DAL.Interfaces
{
    public interface IDimOtherChargesTypeDAO
    {
        DataTable SelectAllDimOtherChargesType();
        DataTable SelectDimOtherChargesType(Int32 idOtherChargesType);
        DataTable SelectAllDimOtherChargesType(SqlConnection conn, SqlTransaction tran);
        int InsertDimOtherChargesType(DimOtherChargesTypeTO dimOtherChargesTypeTO);
        int InsertDimOtherChargesType(DimOtherChargesTypeTO dimOtherChargesTypeTO, SqlConnection conn, SqlTransaction tran);
        int UpdateDimOtherChargesType(DimOtherChargesTypeTO dimOtherChargesTypeTO);
        int UpdateDimOtherChargesType(DimOtherChargesTypeTO dimOtherChargesTypeTO, SqlConnection conn, SqlTransaction tran);
        int DeleteDimOtherChargesType(Int32 idOtherChargesType);
        int DeleteDimOtherChargesType(Int32 idOtherChargesType, SqlConnection conn, SqlTransaction tran);
    }
}
