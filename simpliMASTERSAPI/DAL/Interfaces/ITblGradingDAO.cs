using simpliMASTERSAPI.Models;
using System.Collections.Generic;

namespace simpliMASTERSAPI.DAL.Interfaces
{
    public interface ITblGradingDAO
    {
        List<TblGradingTo> GetAllGrading(bool? isActive = null);
    }
}
