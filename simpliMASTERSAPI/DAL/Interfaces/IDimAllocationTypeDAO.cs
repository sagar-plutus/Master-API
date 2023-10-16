using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.DAL.Interfaces
{
   public interface IDimAllocationTypeDAO
    {

        String SqlSelectQuery();

        List<DropDownTO> getDynamicMaster(DimAllocationTypeTO allocTypeTO);

        List<DimAllocationTypeTO> SelectAllDimAllocationType();

        DimAllocationTypeTO SelectDimAllocationType(Int32 idAllocType);
        List<DimAllocationTypeTO> SelectAllDimAllocationType(SqlConnection conn, SqlTransaction tran);

        int InsertDimAllocationType(DimAllocationTypeTO dimAllocationTypeTO);
        int InsertDimAllocationType(DimAllocationTypeTO dimAllocationTypeTO, SqlConnection conn, SqlTransaction tran);
        int UpdateDimAllocationType(DimAllocationTypeTO dimAllocationTypeTO);

        int UpdateDimAllocationType(DimAllocationTypeTO dimAllocationTypeTO, SqlConnection conn, SqlTransaction tran);

        int ExecuteUpdationCommand(DimAllocationTypeTO dimAllocationTypeTO, SqlCommand cmdUpdate);

        List<DimAllocationTypeTO> ConvertDTToList(SqlDataReader tblUserAreaAllocationTODT);

         int DeleteDimAllocationType(Int32 idAllocType);
        
        int DeleteDimAllocationType(Int32 idAllocType, SqlConnection conn, SqlTransaction tran);
       
         int ExecuteDeletionCommand(Int32 idAllocType, SqlCommand cmdDelete);
    }
}
