using simpliMASTERSAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace simpliMASTERSAPI.DAL.Interfaces
{
    public interface ITblOrgAccountTaxDAO
    {
        TblOrgAccountTaxTO SelectOrgAccountTaxList(Int32 orgId);
        int InsertTblOrgAccountTax(TblOrgAccountTaxTO orgAccTaxTO, SqlConnection conn, SqlTransaction tran);

        int UpdateTblOrgAccountTax(TblOrgAccountTaxTO orgAccTaxTO, SqlConnection conn, SqlTransaction tran);
    }
}
