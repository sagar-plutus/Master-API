using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using simpliMASTERSAPI.Models;
namespace simpliMASTERSAPI.DAL.Interfaces
{
    public interface ITblOrgVoucherNoteDAO
    {
        Int32 AddVoucherNote(TblOrgVoucherNoteTO tblOrgVoucherNoteTO);
        List<TblOrgVoucherNoteTO> GetVoucherNoteList(TblOrgVoucherNoteFilterTO tblOrgVoucherNoteFilterTO, String viewPendingStatusStr = "");
    }
}
