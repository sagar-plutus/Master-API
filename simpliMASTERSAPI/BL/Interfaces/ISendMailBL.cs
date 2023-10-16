using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ISendMailBL
    {
        ResultMessage SendEmail(SendMail tblsendTO, String fileName);
        ResultMessage SendEmail(SendMail tblsendTO, TblEmailConfigrationTO DimEmailConfigrationTOExist = null);
        int SendEmailNotification(SendMail tblsendTO);
    }
}
