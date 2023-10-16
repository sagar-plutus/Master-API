using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.DAL.Interfaces
{
   public interface ITblProductAndRmConfigurationDAO
    {
        List<TblProductAndRmConfigurationTO> SelectAllTblProductAndRmConfigurationList();
        int InsertTblProductAndRmConfiguration(TblProductAndRmConfigurationTO tblProductAndRmConfigurationTO);
        int InsertTblProductAndRmConfiguration(TblProductAndRmConfigurationTO tblProductAndRmConfigurationTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblProductAndRmConfiguration(TblProductAndRmConfigurationTO tblProductAndRmConfigurationTO);
        int UpdateTblProductAndRmConfiguration(TblProductAndRmConfigurationTO tblProductAndRmConfigurationTO, SqlConnection conn, SqlTransaction tran);

    }
}
