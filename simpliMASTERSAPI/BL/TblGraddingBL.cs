using ODLMWebAPI.StaticStuff;
using simpliMASTERSAPI.BL.Interfaces;
using simpliMASTERSAPI.DAL.Interfaces;
using simpliMASTERSAPI.Models;
using System.Collections.Generic;

namespace simpliMASTERSAPI.BL
{
    public class TblGraddingBL : ITblGraddingBL
    {
        private readonly ITblGradingDAO _iTblGradingDAO;
        public TblGraddingBL(ITblGradingDAO iTblGradingDAO)
        {
            _iTblGradingDAO = iTblGradingDAO;
        }

        public ResultMessage GetAllGrading(bool? isActive = null)
        {
            ResultMessage resultMessage = new ResultMessage();

            resultMessage.data = _iTblGradingDAO.GetAllGrading(isActive);

            resultMessage.MessageType = ResultMessageE.Information;
            resultMessage.DisplayMessage = resultMessage.data != null ? Constants.DEFAULT_FETCH_SUCCESS_MSG : Constants.DEFAULT_NOTFOUND_MSG;

            return resultMessage;
        }
    }
}

