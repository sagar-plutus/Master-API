using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface IDimUomGroupDAO
    {
        String SqlSelectQuery();
        List<DimUomGroupTO> SelectAllDimUomGroup();
        // List<DimUomGroupTO> SelectAllDimBrand(DimUomGroupTO dimUomGroupTO);
        DimUomGroupTO SelectDimUomGroup(Int32 idUomGroup);
        //DimUomGroupTO SelectDimBrand(Int32 idBrand, SqlConnection conn, SqlTransaction tran);
        List<DimUomGroupTO> SelectAllDimUomGroup(SqlConnection conn, SqlTransaction tran);
        List<DimUomGroupTO> SelectAllUomGroupList(DimUomGroupTO dimUomGroupTO, SqlConnection conn, SqlTransaction tran);
        List<DimUomGroupTO> ConvertDTToList(SqlDataReader DimUomGroupTODT);
        int InsertDimUomGroup(DimUomGroupTO dimUomGroupTO);
        int InsertDimUomGroup(DimUomGroupTO dimUomGroupTO, SqlConnection conn, SqlTransaction tran);
        //int ExecuteInsertionCommand(DimUomGroupTO dimUomGroupTO, SqlCommand cmdInsert);
        int UpdateDimUomGroup(DimUomGroupTO dimUomGroupTO);
        int UpdateDimUomGroup(DimUomGroupTO dimUomGroupTO, SqlConnection conn, SqlTransaction tran);
        // int ExecuteUpdationCommand(DimUomGroupTO dimUomGroupTO, SqlCommand cmdUpdate);
        int DeleteDimUomGroup(Int32 idUomGroup);
        int DeleteDimUomGroup(Int32 idUomGroup, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idUomGroup, SqlCommand cmdDelete);
        DimUomGroupTO SelectDimUomGroup(int weightMeasureUnitId, int conversionUnitOfMeasure, double conversionFactor,SqlConnection conn, SqlTransaction tran);
    }
}