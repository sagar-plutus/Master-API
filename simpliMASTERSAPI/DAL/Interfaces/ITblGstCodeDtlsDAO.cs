using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
using simpliMASTERSAPI.Models;

namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblGstCodeDtlsDAO
    {
        String SqlSelectQuery();
        List<TblGstCodeDtlsTO> SelectAllTblGstCodeDtls();
        TblGstCodeDtlsTO SelectTblGstCodeDtls(Int32 idGstCode, SqlConnection conn, SqlTransaction tran);
        TblGstCodeDtlsTO SelectGstCodeDtlsTO(Int32 prodCatId, Int32 prodSpecId, Int32 materialId, Int32 prodItemId, SqlConnection conn, SqlTransaction tran);
        List<TblGstCodeDtlsTO> SelectTblGstCodeDtlsList(Int32 gstCodeId, SqlConnection conn, SqlTransaction tran);
        List<TblGstCodeDtlsTO> ConvertDTToList(SqlDataReader tblGstCodeDtlsTODT);
        int InsertTblGstCodeDtls(TblGstCodeDtlsTO tblGstCodeDtlsTO);
        int InsertTblGstCodeDtls(TblGstCodeDtlsTO tblGstCodeDtlsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblGstCodeDtlsTO tblGstCodeDtlsTO, SqlCommand cmdInsert);
        int UpdateTblGstCodeDtls(TblGstCodeDtlsTO tblGstCodeDtlsTO);
        int UpdateTblGstCodeDtls(TblGstCodeDtlsTO tblGstCodeDtlsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblGstCodeDtlsTO tblGstCodeDtlsTO, SqlCommand cmdUpdate);
        int DeleteTblGstCodeDtls(Int32 idGstCode);
        int DeleteTblGstCodeDtls(Int32 idGstCode, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idGstCode, SqlCommand cmdDelete);
        int InsertGstCodeDtlsInSAP(TblGstCodeDtlsTO tblGstCodeDtlsTO);

        int UpdateMappedHsnCodeOfGstDtls(TblGstCodeDtlsTO tblGstCodeDtlsTO, SqlConnection conn, SqlTransaction tran);

        int InsertGstCodeDtlsInSAPV2(TblGstCodeDtlsTO tblGstCodeDtlsTO);
        List<TblGstCodeDtlsTO> SelectAllTblGstCodeDtlsForMigration();
        List<TblGstCodeDtlsTO> SelectAllTblServiceGstCodeDtlsForMigration();
        List<TblGstCodeDtlsTO> SearchGSTCodeDetails(string searchedStr, Int32 searchCriteria, Int32 codeTypeId, SqlConnection conn, SqlTransaction tran);
        TblGstCodeDtlsTO SelectTblGstCodeDtls(String codeNumberStr, Int32 CodeTypeId, SqlConnection conn, SqlTransaction tran);

    
    }
}
