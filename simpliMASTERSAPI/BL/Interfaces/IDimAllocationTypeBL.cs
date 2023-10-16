using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TO;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface IDimAllocationTypeBL
    {

        #region Selection


        List<DropDownTO> getDynamicMaster(DimAllocationTypeTO allocTypeTO);
        List<DimAllocationTypeTO> SelectAllDimAllocationTypeList();


         DimAllocationTypeTO SelectDimAllocationTypeTO(Int32 idAllocType);


        #endregion

        #region Insertion
        int InsertDimAllocationType(DimAllocationTypeTO dimAllocationTypeTO);

        int InsertDimAllocationType(DimAllocationTypeTO dimAllocationTypeTO, SqlConnection conn, SqlTransaction tran);
        

        #endregion

        #region Updation
        int UpdateDimAllocationType(DimAllocationTypeTO dimAllocationTypeTO);
        

        int UpdateDimAllocationType(DimAllocationTypeTO dimAllocationTypeTO, SqlConnection conn, SqlTransaction tran);
        
        #endregion

        #region Deletion
        int DeleteDimAllocationType(Int32 idAllocType);
        
        int DeleteDimAllocationType(Int32 idAllocType, SqlConnection conn, SqlTransaction tran);
        #endregion

    }
}
