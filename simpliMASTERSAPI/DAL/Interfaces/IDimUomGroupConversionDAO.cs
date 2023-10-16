using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface IDimUomGroupConversionDAO
    {
        String SqlSelectQuery();
        List<DimUomGroupConversionTO> SelectAllDimUomGroupConversion();
        List<DimUomGroupConversionTO> SelectAllDimUomGroupConversion(SqlConnection conn, SqlTransaction tran);
        DimUomGroupConversionTO SelectDimUomGroupConversion(Int32 idUomConversion);
        int InsertDimUomGroupConversion(DimUomGroupConversionTO dimUomGroupConversionTO);
        int InsertDimUomGroupConversion(DimUomGroupConversionTO dimUomGroupConversionTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(DimUomGroupConversionTO dimUomGroupConversionTO, SqlCommand cmdInsert);
        int UpdateDimUomGroupConversion(DimUomGroupConversionTO dimUomGroupConversionTO);
        int UpdateDimUomGroupConversion(DimUomGroupConversionTO dimUomGroupConversionTO, SqlConnection conn, SqlTransaction tran);

        int UpdateDimUomGroupConversionForConversion(Int32 uomGroupId, double altQty,int ConversionUnitOfMeasure, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(DimUomGroupConversionTO dimUomGroupConversionTO, SqlCommand cmdUpdate);
        int DeleteDimUomGroupConversion(Int32 idUomConversion);
        int DeleteDimUomGroupConversion(Int32 idUomConversion, SqlConnection conn, SqlTransaction tran);
        List<DimUomGroupConversionTO> GetAllUOMGroupConversionListByGroupId(int uomGroupId);
    }
}
