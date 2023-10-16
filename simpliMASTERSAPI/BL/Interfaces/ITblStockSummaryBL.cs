using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System.Collections.Generic;

namespace simpliMASTERSAPI.BL.Interfaces
{
    public interface ITblStockSummaryBL
    {
        ResultMessage UpdateDailyStock(TblStockSummaryTO tblStockSummaryTO);
        ResultMessage ConfirmStockSummary(List<SizeSpecWiseStockTO> sizeSpecWiseStockTOList);
    }
}
