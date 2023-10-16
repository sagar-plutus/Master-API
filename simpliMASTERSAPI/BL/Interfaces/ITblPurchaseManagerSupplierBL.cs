using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblPurchaseManagerSupplierBL
    {

        List<DropDownTO> SelectPurchaseFromRoleForDropDown();
        int selectPurchaseManagerSupplierTo(TblPurchaseManagerSupplierTO purchaseManagerSupplierTO, SqlConnection conn, SqlTransaction tran);
        List<DropDownTO> SelectPurchaseFromRoleForDropDown(SqlConnection conn, SqlTransaction tran);
        List<DropDownTO> GetSupplierByPMDropDownList(int userId);
        List<DropDownTO> GetPurchaseManagerListOfSupplierForDropDown(int supplierId);
        List<DropDownTO> GetPurchaseManagerListOfSupplierForDropDown(int supplierId, SqlConnection conn, SqlTransaction tran);
        Int32 GetSupplierStateId(int supplierID);
        List<TblPurchaseManagerSupplierTO> SelectAllActivePurchaseManagerSupplierList(Int32 userId);
        int InsertUpdateTblPurchaseManagerSupplier(TblPurchaseManagerSupplierTO tblPurchaseManagerSupplierTO);
        int InsertUpdateTblPurchaseManagerSupplier(TblPurchaseManagerSupplierTO tblPurchaseManagerSupplierTO, SqlConnection conn, SqlTransaction tran);
    }
}