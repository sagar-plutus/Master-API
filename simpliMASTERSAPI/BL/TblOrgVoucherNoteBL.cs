using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ODLMWebAPI.StaticStuff;
using simpliMASTERSAPI.BL.Interfaces;
using ODLMWebAPI.BL.Interfaces;
using simpliMASTERSAPI.Models;
using simpliMASTERSAPI.DAL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.Models;

namespace simpliMASTERSAPI.BL
{
    public class TblOrgVoucherNoteBL : ITblOrgVoucherNoteBL
    {
        private readonly ITblOrgVoucherNoteDAO _iTblOrgVoucherNoteDAO;
        private readonly ICommon _iCommon;
        private readonly IDimensionBL _iDimensionBL;
        private readonly ITblConfigParamsDAO _iTblConfigParamsDAO;
        public TblOrgVoucherNoteBL(ITblConfigParamsDAO iTblConfigParamsDAO, IDimensionBL iDimensionBL, ICommon iCommon, ITblOrgVoucherNoteDAO iTblOrgVoucherNoteDAO)
        {
            _iTblOrgVoucherNoteDAO = iTblOrgVoucherNoteDAO;
            _iCommon = iCommon;
            _iDimensionBL = iDimensionBL;
            _iTblConfigParamsDAO = iTblConfigParamsDAO;
        }
        public ResultMessage GetVoucherNoteList(TblOrgVoucherNoteFilterTO tblOrgVoucherNoteFilterTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                string viewPendingStatusStr = "";
                if(tblOrgVoucherNoteFilterTO.ViewPending == true)
                {
                    TblConfigParamsTO tblConfigParams = _iTblConfigParamsDAO.SelectTblConfigParams(Constants.VOUCHER_NOTE_PENDING_FOR_IMPORT_STATUS_LIST);
                    if (tblConfigParams != null)
                    {
                        viewPendingStatusStr = tblConfigParams.ConfigParamVal;
                    }
                }
                List<TblOrgVoucherNoteTO> TblOrgVoucherNoteTOList = _iTblOrgVoucherNoteDAO.GetVoucherNoteList(tblOrgVoucherNoteFilterTO, viewPendingStatusStr);
                if (TblOrgVoucherNoteTOList == null || TblOrgVoucherNoteTOList.Count == 0)
                {
                    resultMessage.DefaultBehaviour("Record Not Found");
                    return resultMessage;
                }
                resultMessage.DefaultSuccessBehaviour();
                resultMessage.data = TblOrgVoucherNoteTOList;
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "GetVoucherNoteList");
                return resultMessage;
            }
        }
        public ResultMessage AddVoucherNote(TblOrgVoucherNoteTO tblOrgVoucherNoteTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                tblOrgVoucherNoteTO.CreatedOn = _iCommon.ServerDateTime;
                tblOrgVoucherNoteTO.StatusId = (Int32)Constants.VoucherNoteStatusE.NEW;
                int result = _iTblOrgVoucherNoteDAO.AddVoucherNote(tblOrgVoucherNoteTO);
                if(result != 1)
                {
                    resultMessage.DefaultBehaviour("Failed to Add Voucher Entry");
                    return resultMessage;
                }
                resultMessage.DefaultSuccessBehaviour();
                resultMessage.Tag = tblOrgVoucherNoteTO.IdIOrgVoucherNote;
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "AddVoucherNote");
                return resultMessage;
            }
        }
    }
}
