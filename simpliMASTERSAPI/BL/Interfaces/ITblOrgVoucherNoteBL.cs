using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ODLMWebAPI.StaticStuff;
using simpliMASTERSAPI.Models;
namespace simpliMASTERSAPI.BL.Interfaces
{
    public interface ITblOrgVoucherNoteBL
    {
        ResultMessage AddVoucherNote(TblOrgVoucherNoteTO tblOrgVoucherNoteTO);
        ResultMessage GetVoucherNoteList(TblOrgVoucherNoteFilterTO tblOrgVoucherNoteFilterTO);
    }
}
