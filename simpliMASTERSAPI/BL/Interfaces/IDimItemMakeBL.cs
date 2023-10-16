using ODLMWebAPI.StaticStuff;
using simpliMASTERSAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simpliMASTERSAPI.BL.Interfaces
{
    public interface IDimItemMakeBL
    {
        ResultMessage InsertDimItemMake(DimItemMakeTO dimItemMakeTO);
    }
}
