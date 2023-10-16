using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ODLMWebAPI.StaticStuff;
using simpliMASTERSAPI.BL.Interfaces;
using simpliMASTERSAPI.Models;
namespace simpliMASTERSAPI.Controllers
{
    [Route("api/VoucherNote")]
    public class VoucherNoteController : Controller
    {
        private readonly ITblOrgVoucherNoteBL _iTblOrgVoucherNoteBL;
        public VoucherNoteController(ITblOrgVoucherNoteBL iTblOrgVoucherNoteBL)
        {
            _iTblOrgVoucherNoteBL = iTblOrgVoucherNoteBL;
        }
     
        #region Get
        #endregion
        #region POST
        [Route("AddVoucherNote")]
        [HttpPost]
        public ResultMessage AddVoucherNote([FromBody] TblOrgVoucherNoteTO tblOrgVoucherNoteTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                return _iTblOrgVoucherNoteBL.AddVoucherNote(tblOrgVoucherNoteTO);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "Exception Error in AddConsolidation");
                return resultMessage;
            }
        }
        [Route("GetVoucherNoteList")]
        [HttpPost]
        public ResultMessage GetVoucherNoteList([FromBody] TblOrgVoucherNoteFilterTO tblOrgVoucherNoteFilterTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                return _iTblOrgVoucherNoteBL.GetVoucherNoteList(tblOrgVoucherNoteFilterTO);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "Exception Error in GetVoucherNoteList");
                return resultMessage;
            }
        }
        #endregion
    }
}