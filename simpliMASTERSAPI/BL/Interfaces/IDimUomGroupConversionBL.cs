using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
namespace ODLMWebAPI.BL.Interfaces
{
    public interface IDimUomGroupConversionBL
    {
        List<DimUomGroupConversionTO> SelectAllDimUomGroupConversion();
        List<DimUomGroupConversionTO> SelectAllDimUomGroupConversion(SqlConnection conn, SqlTransaction tran);
        DimUomGroupConversionTO SelectDimUomGroupConversion(Int32 idUomConversion);
        int InsertDimUomGroupConversion(DimUomGroupConversionTO dimUomGroupConversionTO);
        int InsertDimUomGroupConversion(DimUomGroupConversionTO dimUomGroupConversionTO, SqlConnection conn, SqlTransaction tran);
        int UpdateDimUomGroupConversion(DimUomGroupConversionTO dimUomGroupConversionTO);
        int UpdateDimUomGroupConversion(DimUomGroupConversionTO dimUomGroupConversionTO, SqlConnection conn, SqlTransaction tran);
        int DeleteDimUomGroupConversion(Int32 idUomConversion);
        int DeleteDimUomGroupConversion(Int32 idUomConversion, SqlConnection conn, SqlTransaction tran);
        List<DimUomGroupConversionTO> GetAllUOMGroupConversionListByGroupId(Int32 uomGroupId);
    }
}