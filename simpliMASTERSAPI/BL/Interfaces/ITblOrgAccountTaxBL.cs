using ODLMWebAPI.Models;
using simpliMASTERSAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simpliMASTERSAPI.BL.Interfaces
{
    public interface ITblOrgAccountTaxBL
    {
        TblOrgAccountTaxTO SelectOrgAccountTaxsList(Int32 orgId);
    }
}
