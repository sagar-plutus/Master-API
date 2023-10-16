using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface IDimOtherChargesTypeBL
    {
        DataTable SelectAllDimOtherChargesType();
        List<DimOtherChargesTypeTO> SelectAllDimOtherChargesTypeList();
        DimOtherChargesTypeTO SelectDimOtherChargesTypeTO(Int32 idOtherChargesType);
        int InsertDimOtherChargesType(DimOtherChargesTypeTO dimOtherChargesTypeTO);
        int InsertDimOtherChargesType(DimOtherChargesTypeTO dimOtherChargesTypeTO, SqlConnection conn, SqlTransaction tran);
        int UpdateDimOtherChargesType(DimOtherChargesTypeTO dimOtherChargesTypeTO);
        int UpdateDimOtherChargesType(DimOtherChargesTypeTO dimOtherChargesTypeTO, SqlConnection conn, SqlTransaction tran);
        int DeleteDimOtherChargesType(Int32 idOtherChargesType);
        int DeleteDimOtherChargesType(Int32 idOtherChargesType, SqlConnection conn, SqlTransaction tran);
    }
}
