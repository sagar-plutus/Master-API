
using ODLMWebAPI.Models;
using simpliMASTERSAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simpliMASTERSAPI.DAL.Interfaces
{
    public interface IDimItemMakeDAO
    {
        int InsertDimItemMake(DimItemMakeTO dimItemMakeTO);
        List<DropDownTO> CheckMakeExistsOrNot(String makeName);
    }
}
