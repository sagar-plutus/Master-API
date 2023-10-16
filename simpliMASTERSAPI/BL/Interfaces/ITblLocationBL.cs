using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblLocationBL
    {
        List<TblLocationTO> SelectAllTblLocationList();
        List<TblLocationTO> SelectAllCompartmentLocationList(Int32 parentLocationId);
        List<TblLocationTO> SelectAllParentLocation();
        List<DropDownTO> SelectAllLocationAndCompartmentsListForDropDown(Boolean onlyCompartments=true);

        List<DropDownTO> SelectLocationFromWarehouse(Int32 warehouseId); 
        TblLocationTO SelectTblLocationTO(Int32 idLocation);
        ResultMessage SaveAndUpdateLocationMaster(TblLocationTO tblLocationTO, Int32 loginUserId);
        List<TblLocationTO> SelectStkNotTakenCompartmentList(DateTime stockDate);
        int InsertTblLocation(TblLocationTO tblLocationTO);
        int InsertTblLocation(TblLocationTO tblLocationTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblLocation(TblLocationTO tblLocationTO);
        int UpdateTblLocation(TblLocationTO tblLocationTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblLocation(Int32 idLocation);
        int DeleteTblLocation(Int32 idLocation, SqlConnection conn, SqlTransaction tran);
    }
}