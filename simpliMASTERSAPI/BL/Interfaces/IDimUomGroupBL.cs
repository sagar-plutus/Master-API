using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
namespace ODLMWebAPI.BL.Interfaces
{
    public interface IDimUomGroupBL 
    {
        List<DimUomGroupTO> SelectAllDimUomGroup();
      
        DimUomGroupTO SelectDimUomGroup(Int32 idUomGroup);
        DimUomGroupTO SelectDimUomGroupTO(Int32 idUomGroup, SqlConnection conn, SqlTransaction tran);

        List<DimUomGroupTO> SelectAllDimUomGroupList();
        List<DimUomGroupTO> SelectAllUomGroupList(DimUomGroupTO dimUomGroupTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage SaveAndUpdateUOMGroup(DimUomGroupTO dimUomGroupTO, Int32 loginUserId);
        ResultMessage SaveAndUpdateUOM(DimUomGroupConversionTO dimUomGroupTO, Int32 loginUserId);
        int InsertDimUomGroup(DimUomGroupTO dimUomGroupTO);
        int InsertDimUomGroup(DimUomGroupTO dimUomGroupTO, SqlConnection conn, SqlTransaction tran);
        int UpdateDimUomGroup(DimUomGroupTO dimUomGroupTO);
        int UpdateDimUomGroup(DimUomGroupTO dimUomGroupTO, SqlConnection conn, SqlTransaction tran);
        int DeleteDimUomGroup(Int32 idUomGroup);
        int DeleteDimUomGroup(Int32 idUomGroup, SqlConnection conn, SqlTransaction tran);

    }
}



       