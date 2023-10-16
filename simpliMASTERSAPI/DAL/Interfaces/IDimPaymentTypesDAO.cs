using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace simpliMASTERSAPI.DAL.Interfaces
{
   public interface IDimPaymentTypesDAO
    {
        DataTable SelectAllDimPaymentTypes();
        DataTable SelectDimPaymentTypes(Int32 idPayType);
        DataTable SelectAllDimPaymentTypes(SqlConnection conn, SqlTransaction tran);
        int InsertDimPaymentTypes(DimPaymentTypesTO dimPaymentTypesTO);
        int InsertDimPaymentTypes(DimPaymentTypesTO dimPaymentTypesTO, SqlConnection conn, SqlTransaction tran);
        int UpdateDimPaymentTypes(DimPaymentTypesTO dimPaymentTypesTO);
        int UpdateDimPaymentTypes(DimPaymentTypesTO dimPaymentTypesTO, SqlConnection conn, SqlTransaction tran);
        int DeleteDimPaymentTypes(Int32 idPayType);
        int DeleteDimPaymentTypes(Int32 idPayType, SqlConnection conn, SqlTransaction tran);
    }
}
