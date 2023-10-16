using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface IDimStatusDAO
    {
        String SqlSelectQuery();
        List<DimStatusTO> SelectAllDimStatus();
        List<DimStatusTO> SelectAllDimStatus(int txnTypeId);
        DimStatusTO SelectDimStatus(Int32 idStatus, SqlConnection conn, SqlTransaction tran);
        List<DimStatusTO> ConvertDTToList(SqlDataReader dimStatusTODT);
        int InsertDimStatus(DimStatusTO dimStatusTO);
        int InsertDimStatus(DimStatusTO dimStatusTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(DimStatusTO dimStatusTO, SqlCommand cmdInsert);
        int UpdateDimStatus(DimStatusTO dimStatusTO);
        int UpdateDimStatus(DimStatusTO dimStatusTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(DimStatusTO dimStatusTO, SqlCommand cmdUpdate);
        int DeleteDimStatus(Int32 idStatus);
        int DeleteDimStatus(Int32 idStatus, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idStatus, SqlCommand cmdDelete);
        List<DimConsumerTypeTO> SelectAllConsumerCategoryList(Int32 orgTypeId);

    }
}