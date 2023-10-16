using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace simpliMASTERSAPI.BL.Interfaces
{
   public interface IDimPaymentTypesBL
    {
        DataTable SelectAllDimPaymentTypes();
        List<DimPaymentTypesTO> SelectAllDimPaymentTypesList();
        DimPaymentTypesTO SelectDimPaymentTypesTO(Int32 idPayType);
        int InsertDimPaymentTypes(DimPaymentTypesTO dimPaymentTypesTO);
        int InsertDimPaymentTypes(DimPaymentTypesTO dimPaymentTypesTO, SqlConnection conn, SqlTransaction tran);
        int UpdateDimPaymentTypes(DimPaymentTypesTO dimPaymentTypesTO);
        int UpdateDimPaymentTypes(DimPaymentTypesTO dimPaymentTypesTO, SqlConnection conn, SqlTransaction tran);
        int DeleteDimPaymentTypes(Int32 idPayType);
        int DeleteDimPaymentTypes(Int32 idPayType, SqlConnection conn, SqlTransaction tran);
    }
}
