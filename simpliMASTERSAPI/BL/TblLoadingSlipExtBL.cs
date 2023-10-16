using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using simpliMASTERSAPI.BL.Interfaces;
using simpliMASTERSAPI.DAL;
using simpliMASTERSAPI.DAL.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace simpliMASTERSAPI.BL
{
    public class TblLoadingSlipExtBL : ITblLoadingSlipExtBL
    {
        private readonly ITblConfigParamsBL _tblConfigParamsBL;
        private readonly ITblLoadingDAO _tblLoadingDAO;
        private readonly ITblLoadingSlipExtDAO _tblLoadingSlipExtDAO;
        public TblLoadingSlipExtBL(ITblConfigParamsBL tblConfigParamsBL, ITblLoadingDAO tblLoadingDAO, ITblLoadingSlipExtDAO tblLoadingSlipExtDAO) 
        {
            _tblConfigParamsBL= tblConfigParamsBL;
            _tblLoadingDAO= tblLoadingDAO;
            _tblLoadingSlipExtDAO = tblLoadingSlipExtDAO;
        }

        public  List<TblLoadingSlipExtTO> SelectCnfWiseLoadingMaterialToPostPoneList(SqlConnection conn, SqlTransaction tran)
        {
            List<TblLoadingSlipExtTO> postponeList = null;
            TblConfigParamsTO postponeConfigParamsTO = _tblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_LOADING_SLIPS_AUTO_POSTPONED_STATUS_ID, conn, tran);
            //Sanjay [2017-07-18] The vehicles which are gate in ,loading completed or postponed
            //List<TblLoadingTO> loadingTOToPostponeList = TblLoadingDAO.SelectAllLoadingListByStatus((int)Constants.TranStatusE.LOADING_POSTPONED + "", conn, tran);
            List<TblLoadingTO> loadingTOToPostponeList = _tblLoadingDAO.SelectAllLoadingListByStatus(postponeConfigParamsTO.ConfigParamVal, conn, tran);
            if (loadingTOToPostponeList != null)
            {
                postponeList = new List<TblLoadingSlipExtTO>();
                var loadingIds = string.Join(",", loadingTOToPostponeList.Where(x => x.LoadingTypeE == Constants.LoadingTypeE.REGULAR).Select(p => p.IdLoading.ToString()));
                List<TblLoadingSlipExtTO> extList = _tblLoadingSlipExtDAO.SelectAllLoadingSlipExtListFromLoadingId(loadingIds, conn, tran);
                if (extList != null && extList.Count > 0)
                {
                    postponeList.AddRange(extList);
                }
            }

            return postponeList;
        }
    }
}
