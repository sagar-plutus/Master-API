using ODLMWebAPI;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblProdItemMakeBrandExtDAO
    {

        #region Selection
        List<TblProdItemMakeBrandExtTO> SelectAllTblProdItemMakeBrandExt();

        TblProdItemMakeBrandExtTO SelectTblProdItemMakeBrandExt(int idProdItemMakeBrandExt);

        List<TblProdItemMakeBrandExtTO> SelectAllTblProdItemMakeBrandExt(SqlConnection conn, SqlTransaction tran);

        List<TblProdItemMakeBrandExtTO> SelectAllTblProdItemMakeBrandExtByProdItem(int prodItemId);


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
