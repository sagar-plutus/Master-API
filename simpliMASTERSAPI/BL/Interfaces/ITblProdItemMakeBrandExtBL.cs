using ODLMWebAPI;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace simpliMASTERSAPI.BL.Interfaces
{
    public interface ITblProdItemMakeBrandExtBL
    {
        #region Selection

        List<TblProdItemMakeBrandExtTO> SelectAllTblProdItemMakeBrandExt();
        List<TblProdItemMakeBrandExtTO> SelectAllTblProdItemMakeBrandExtByProdItem(Int32 prodItemId);

        List<TblProdItemMakeBrandExtTO> SelectAllTblProdItemMakeBrandExt(SqlConnection conn, SqlTransaction tran);

        TblProdItemMakeBrandExtTO SelectTblProdItemMakeBrandExtTO(int idProdItemMakeBrandExt);


        #endregion

        #region Insertion
        int InsertTblProdItemMakeBrandExt(TblProdItemMakeBrandExtTO tblProdItemMakeBrandExtTO);
        int InsertTblProdItemMakeBrandExt(TblProdItemMakeBrandExtTO tblProdItemMakeBrandExtTO, SqlConnection conn, SqlTransaction tran);
        #endregion

        #region Updation
        int UpdateTblProdItemMakeBrandExt(TblProdItemMakeBrandExtTO tblProdItemMakeBrandExtTO);

        int UpdateTblProdItemMakeBrandExt(TblProdItemMakeBrandExtTO tblProdItemMakeBrandExtTO, SqlConnection conn, SqlTransaction tran);

        #endregion

        #region Deletion
        int DeleteTblProdItemMakeBrandExt(int idProdItemMakeBrandExt);

        int DeleteTblProdItemMakeBrandExt(int idProdItemMakeBrandExt, SqlConnection conn, SqlTransaction tran);

        #endregion
    }
}
