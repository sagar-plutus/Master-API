using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblMaterialDAO
    {
        String SqlSelectQuery();
        List<TblMaterialTO> SelectAllTblMaterial();
        List<DropDownTO> SelectAllMaterialForDropDown();
        TblMaterialTO SelectTblMaterial(Int32 idMaterial);
        List<TblMaterialTO> ConvertDTToList(SqlDataReader tblMaterialTODT);
        List<DropDownTO> SelectMaterialTypeDropDownList();
        
        int InsertTblMaterial(TblMaterialTO tblMaterialTO);
        int InsertTblMaterial(TblMaterialTO tblMaterialTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblMaterialTO tblMaterialTO, SqlCommand cmdInsert);
        int UpdateTblMaterial(TblMaterialTO tblMaterialTO);
        int UpdateTblMaterial(TblMaterialTO tblMaterialTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblMaterialTO tblMaterialTO, SqlCommand cmdUpdate);
        int DeleteTblMaterial(Int32 idMaterial);
        int DeleteTblMaterial(Int32 idMaterial, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idMaterial, SqlCommand cmdDelete);
        int UpdateSizeTestingDtl(SizeTestingDtlTO sizeTestingDtlTO, SqlConnection conn, SqlTransaction tran);
        int UpdateSizeTestingDtl(SizeTestingDtlTO sizeTestingDtlTO);
        int InsertSizeTestingDtl(TblMaterialTO sizeTestingDtlTO, SqlConnection conn, SqlTransaction tran);
        int InsertSizeTestingDtlV2(TblMaterialTO sizeTestingDtlTO);

        int InsertSizeTestingDtl(SizeTestingDtlTO sizeTestingDtlTO);
        DataTable SelectAllSizeTestingDtl(SqlConnection conn, SqlTransaction tran);
        DataTable SelectSizeTestingDtl();
        DataTable SelectAllSizeTestingDtl();
    }
}