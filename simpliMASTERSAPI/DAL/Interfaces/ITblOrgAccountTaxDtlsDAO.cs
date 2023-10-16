using simpliMASTERSAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace simpliMASTERSAPI.DAL.Interfaces
{
    public interface ITblOrgAccountTaxDtlsDAO
    {
        List<TblOrgAccountTaxDtlsTO> SelectOrgAccountTaxDtlsList(Int32 orgAccountTaxId);

        int InsertTblOrgAccountTaxDtls(TblOrgAccountTaxDtlsTO orgAccTaxDtlsTO, SqlConnection conn, SqlTransaction tran);

        int UpdateTblOrgAccountTaxDtls(TblOrgAccountTaxTO orgAccTaxTO, SqlConnection conn, SqlTransaction tran);

    }
}
