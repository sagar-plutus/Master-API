using ODLMWebAPI.Models;
using System.Collections.Generic;

namespace simpliMASTERSAPI.BL.Interfaces
{
    public interface ITblQuotaDeclarationBL
    {
        int SaveDeclaredRateAndAllocatedQuota(List<TblQuotaDeclarationTO> quotaExtList, List<TblQuotaDeclarationTO> quotaList, TblGlobalRateTO tblGlobalRateTO);
    }
}
