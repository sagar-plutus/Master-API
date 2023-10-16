using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblProdClassificationDAO
    {
        String SqlSelectQuery();
        List<TblProdClassificationTO> SelectAllTblProdClassification(string prodClassType = "");
        List<TblProdClassificationTO> SelectAllTblProdClassification(SqlConnection conn, SqlTransaction tran, string prodClassType = "");
        List<DropDownTO> SelectAllProdClassificationForDropDown(Int32 parentClassId);
        TblProdClassificationTO SelectTblProdClassification(Int32 idProdClass);
        List<TblProdClassificationTO> checkSubGroupAlreadyExists(TblProdClassificationTO tblProdClassificationTO);
        TblProdClassificationTO SelectTblProdClassificationTOV2(Int32 idProdClass, SqlConnection conn, SqlTransaction tran);
        List<TblProdClassificationTO> SelectAllProdClassificationListyByItemProdCatgE(StaticStuff.Constants.ItemProdCategoryE itemProdCategoryE);
        List<TblProdClassificationTO> ConvertDTToList(SqlDataReader tblProdClassificationTODT);
        List<DropDownTO> SelectMaterialListForDropDown(StaticStuff.Constants.ItemProdCategoryE itemProdCategoryE);
        int InsertTblProdClassification(TblProdClassificationTO tblProdClassificationTO);
        int InsertTblProdClassification(TblProdClassificationTO tblProdClassificationTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblProdClassificationTO tblProdClassificationTO, SqlCommand cmdInsert);
        int UpdateTblProdClassification(TblProdClassificationTO tblProdClassificationTO);
        int SetIsDefaultByClassificationType(TblProdClassificationTO tblProdClassificationTO);
        int UpdateTblProdClassification(TblProdClassificationTO tblProdClassificationTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationSetDefaultCommand(TblProdClassificationTO tblProdClassificationTO, SqlCommand cmdUpdate);
        int ExecuteUpdationCommand(TblProdClassificationTO tblProdClassificationTO, SqlCommand cmdUpdate);
        int DeleteTblProdClassification(Int32 idProdClass);
        int DeleteTblProdClassification(Int32 idProdClass, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idProdClass, SqlCommand cmdDelete);
        //Priyanka [17-06-2019]
        List<TblProdClassificationTO> SelectAllTblProdClassificationByParentId(Int32 prodClassId =0, Int32 itemProdCatId =0);
        List<TblProdClassificationTO> checkCategoryAlreadyExistsOld(Int32 idProdClass, String prodClassType, String prodClassDesc, SqlConnection conn = null, SqlTransaction tran = null);

        List<TblProdClassificationTO> checkCategoryAlreadyExists(Int32 idProdClass, String prodClassType, String prodClassDesc, Int32 parentProdClass, Int32 itemProdCatId, SqlConnection conn = null, SqlTransaction tran = null);

        List<DropDownTO> getProdClassIdsByItemProdCat(Int32 itemProdCatId, string prodClassType = "S");
        TblProdClassificationTO SelectTblProdClassification(bool isScrapProdItem, Int32 parentProdClassId);
        TblProdClassificationTO SelectTblProdClassification(string prodClassDesc, Int32 parentProdClassId,int prodCatId, SqlConnection conn, SqlTransaction tran);
    }
}