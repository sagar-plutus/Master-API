using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblProdClassificationBL
    {
        List<TblProdClassificationTO> SelectAllTblProdClassificationList(string prodClassType = "");
        List<TblProdClassificationTO> SelectAllTblProdClassificationList(SqlConnection conn, SqlTransaction tran, string prodClassType = "");
        List<DropDownTO> SelectAllProdClassificationForDropDown(Int32 parentClassId);
        List<TblProdClassificationTO> checkSubGroupAlreadyExists(TblProdClassificationTO tblProdClassificationTO);
        TblProdClassificationTO SelectTblProdClassificationTO(Int32 idProdClass);
        TblProdClassificationTO SelectTblProdClassificationTOV2(Int32 idProdClass, SqlConnection conn, SqlTransaction tran);
        List<TblProdClassificationTO> SelectAllProdClassificationListyByItemProdCatgE(StaticStuff.Constants.ItemProdCategoryE itemProdCategoryE);
        void SetProductClassificationDisplayName(TblProdClassificationTO tblProdClassificationTO, List<TblProdClassificationTO> allProdClassificationList);
        void GetDisplayName(List<TblProdClassificationTO> allProdClassificationList, int parentId, List<TblProdClassificationTO> DisplayNameList);
        string SelectProdtClassificationListOnType(Int32 idProdClass);
        void GetIdsofProductClassification(List<TblProdClassificationTO> allList, int parentId, ref String ids);
        List<TblProdClassificationTO> SelectProductClassificationListByProductItemId(Int32 prodItemId);
        List<TblProdClassificationTO> SelectProductChildList(List<TblProdClassificationTO> tblProdClassificationTOlist, Int32 parentId);

        List<DropDownTO> SelectMaterialListForDropDown(StaticStuff.Constants.ItemProdCategoryE itemProdCategoryE);
        //int InsertProdClassification(TblProdClassificationTO tblProdClassificationTO);
        ResultMessage InsertProdClassification(TblProdClassificationTO tblProdClassificationTO);

        List<Int32> GetDefaultProductClassification();
        int InsertTblProdClassification(TblProdClassificationTO tblProdClassificationTO);
        int InsertTblProdClassification(TblProdClassificationTO tblProdClassificationTO, SqlConnection conn, SqlTransaction tran);
        int UpdateDisplayName(List<TblProdClassificationTO> allProdClassificationList, TblProdClassificationTO ProdClassificationTO, ref String idClassStr, SqlConnection conn, SqlTransaction tran);
        ResultMessage UpdateProdClassification(TblProdClassificationTO tblProdClassificationTO);
        int UpdateTblProdClassification(TblProdClassificationTO tblProdClassificationTO);
        int UpdateTblProdClassification(TblProdClassificationTO tblProdClassificationTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblProdClassification(Int32 idProdClass);
        int DeleteTblProdClassification(Int32 idProdClass, SqlConnection conn, SqlTransaction tran);
        //Priyanka [17-06-2019]
        List<TblProdClassificationTO> SelectAllTblProdClassificationListByParentId(Int32 prodClassId, Int32 itemProdCatId);
        //Priyanka [11-07-2019]
        ResultMessage DeactivateCategoryAndRespItem(Int32 idProdClass, Int32 loginUserId);
        string SelectProdtClassificationListOnType(Int32 idProdClass, Int32 subCategoryId, Int32 itemID);

        void GetIdsofProductClassification(List<TblProdClassificationTO> allList, int parentId, int subCategoryId, int itemID, ref String ids);
        List<DropDownTO> getProdClassIdsByItemProdCat(Int32 itemProdCatId, string prodClassType = "S");
        TblProdClassificationTO SelectTblProdClassification(bool isScrapProdItem, Int32 parentProdClassId);
        TblProdClassificationTO SelectTblProdClassification(string prodClassDesc, Int32 parentProdClassId, int prodCatId, SqlConnection conn, SqlTransaction tran);
        ResultMessage InsertProdClassification(TblProdClassificationTO tblProdClassificationTO, SqlConnection conn, SqlTransaction tran);

    }
}