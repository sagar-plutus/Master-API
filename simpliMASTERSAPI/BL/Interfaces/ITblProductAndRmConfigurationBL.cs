using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
   public interface ITblProductAndRmConfigurationBL
    {
        List<TblProductAndRmConfigurationTO> SelectAllTblProductAndRmConfigurationList();
        ResultMessage InsertTblProductAndRmConfiguration(TblProductAndRmConfigurationTO tblProductAndRmConfigurationTO);
        ResultMessage UpdateTblProductAndRmConfiguration(TblProductAndRmConfigurationTO tblProductAndRmConfigurationTO);

       // TblPurchaseRequestTo GetProductAndRMConfigurationById(Int32 bookingId);
    }
}
