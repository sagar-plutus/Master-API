using ODLMWebAPI.StaticStuff;
using simpliMASTERSAPI.Models;
using System.Collections.Generic;

namespace simpliMASTERSAPI.BL.Interfaces
{
    public interface ITblGraddingBL
    {
        ResultMessage GetAllGrading(bool? isActive = null);
    }
}
