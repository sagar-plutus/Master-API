using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblProdItemMakeBrandBL
    {
        List<TblProdItemMakeBrandTO> SelectAllTblProdItemMakeBrand();
        List<TblProdItemMakeBrandTO> SelectTblProdItemMakeBrand(Int32 idProdMakeBrand);
        List<TblProdItemMakeBrandTO> SelectAllTblProdItemMakeBrand(SqlConnection conn, SqlTransaction tran);
        List<TblProdItemMakeBrandTO> SelectProdItemMakeBrandByProdItemBrandId(TblProdItemMakeBrandTO tblProdItemMakeBrandTO);
        List<TblProdItemMakeBrandTO> SelectedProdItemMakeBrand(Int32 prodItemId);
        int InsertTblProdItemMakeBrand(TblProdItemMakeBrandTO tblProdItemMakeBrandTO);
        int InsertTblProdItemMakeBrand(TblProdItemMakeBrandTO tblProdItemMakeBrandTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblProdItemMakeBrand(TblProdItemMakeBrandTO tblProdItemMakeBrandTO);
        int UpdateTblProdItemMakeBrand(TblProdItemMakeBrandTO tblProdItemMakeBrandTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblProdItemMakeBrand(Int32 idProdMakeBrand);
        int DeleteTblProdItemMakeBrand(Int32 idProdMakeBrand, SqlConnection conn, SqlTransaction tran);
    }
}
